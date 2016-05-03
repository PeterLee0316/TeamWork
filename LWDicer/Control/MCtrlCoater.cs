using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_DataManager;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_CtrlCoater;

namespace LWDicer.Control
{
    public class DEF_CtrlCoater
    {
        public const int ERR_CTRL_COATER_UNABLE_TO_USE_HANDLER = 1;
        public const int ERR_CTRL_COATER_UNABLE_TO_USE_LOG = 2;
        public const int ERR_CTRL_COATER_OBJECT_ABSORBED = 3;
        public const int ERR_CTRL_COATER_OBJECT_NOT_ABSORBED = 4;
        public const int ERR_CTRL_COATER_OBJECT_EXIST = 5;
        public const int ERR_CTRL_COATER_OBJECT_NOT_EXIST = 6;
        public const int ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED = 7;
        public const int ERR_CTRL_COATER_CYLINDER_TIMEOUT = 8;
        public const int ERR_CTRL_COATER_NOT_UP = 9;
        public const int ERR_CTRL_COATERR_CANNOT_DETECT_POSINFO = 10;
        public const int ERR_CTRL_COATER_PCB_DOOR_OPEN = 11;
        public const int ERR_CTRL_COATER_UHANDLER_IN_DOWN_AND_LHANDLER_IN_SAME_XZONE = 12;
        public const int ERR_CTRL_COATER_UHANDLER_NEED_DOWN_AND_LHANDLER_IN_SAME_XZONE = 13;
        public const int ERR_CTRL_COATER_LHANDLER_NEED_MOVE_AND_UHANDLER_IN_DOWN = 14;
        public const int ERR_CTRL_COATER_XAX_POS_NOT_MATCH_ZONE = 15;

        public enum ECoaterOp
        {
            NONE = -1,
            P_WASH,
            DRY,
            DRY_C,
            COAT,
            COAT_M,
            FCLN_E,
            WAIT,
            MAX,
        }


        public class CCtrlCoatingRefComp
        {
            public MMeSpinner SpinCoater;

            public CCtrlCoatingRefComp()
            {

            }
        }
    }


    public class MCtrlCoater : MCtrlLayer
    {
        private CCtrlCoatingRefComp m_RefComp;
        private CCoatingData m_Data;

        MTickTimer m_tmrOperation = new MTickTimer();


        public MCtrlCoater(CObjectInfo objInfo, CCtrlCoatingRefComp refComp, CCoatingData data) : base(objInfo)
        {
            SetData(data);
        }

        public int SetData(CCoatingData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CCoatingData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int Absorb(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.SpinCoater.Absorb(bSkipSensor);
            return iResult;
        }

        public int Release(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.SpinCoater.Release(bSkipSensor);
            return iResult;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            bStatus = false;

            int iResult = m_RefComp.SpinCoater.IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            
            return SUCCESS;
        }

        public int IsAbsorbed(out bool bStatus)
        {
            bStatus = false;

            int iResult = m_RefComp.SpinCoater.IsAbsorbed(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsReleased(out bool bStatus)
        {
            bStatus = false;
            int iResult = m_RefComp.SpinCoater.IsReleased(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        
        public int IsTableUp(out bool bStatus)
        {
            bStatus = false;
            int iResult = m_RefComp.SpinCoater.IsCylUp(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsTableDown(out bool bStatus)
        {
            bStatus = false;
            int iResult = m_RefComp.SpinCoater.IsCylDown(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsRotateLoadPos()
        {
            int iResult = m_RefComp.SpinCoater.CheckForRotateLoad();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCoatNozzleSafetyPos()
        {
            int iResult = m_RefComp.SpinCoater.CheckForCoatNozzleSafety();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCleanNozzleSafetyPos()
        {
            int iResult = m_RefComp.SpinCoater.CheckForCleanNozzleSafety();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }


        private int CheckSafetyInterlock()
        {
            int iResult = 0;
            bool bStatus = false;

            // 1. Wafer Vac Check
            iResult  = IsAbsorbed(out bStatus);
            if (iResult != SUCCESS)
            {
                WriteLog("CtrlCoater의 Rotate Chuck Table Vacuum Off.", ELogType.Debug, ELogWType.Error);
                return iResult;
            }

            // 2. Chuck Table Down Check
            iResult = IsTableDown(out bStatus);
            if (iResult != SUCCESS)
            {
                WriteLog("CtrlCoater의 Rotate Chuck Table Not Down Pos.", ELogType.Debug, ELogWType.Error);
                return iResult;
            }

            return SUCCESS;
        }

        private int CheckVacuum_forRotateMove(bool bPanelTransfer)
        {
            int iResult = SUCCESS;
            bool bDetected, bAbsorbed;

            // check vacuum
            iResult = IsObjectDetected(out bDetected);
            if (iResult != SUCCESS) return iResult;

            iResult = IsAbsorbed(out bAbsorbed);
            if (iResult != SUCCESS) return iResult;

            if (bPanelTransfer)
            {
                if (bDetected == true && bAbsorbed == false)
                {
                    iResult = Absorb();
                    if (iResult != SUCCESS) return iResult;

                    bAbsorbed = true;
                }
            }

            // check object exist when auto run
            if (AutoManual == EAutoManual.AUTO)
            {
                if (OpMode != EOpMode.DRY_RUN)
                {
                    if (bDetected != bPanelTransfer)
                    {
                        if (bPanelTransfer)    // Wafer Exist
                        {
                            WriteLog("CtrlCoater의 Rotate Chuck Table Vacuum Check Fail. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                            return GenerateErrorCode(ERR_CTRL_COATER_OBJECT_NOT_EXIST);
                        }
                        else
                        {
                            WriteLog("CtrlCoater의 Rotate Chuck Table Vacuum Check Fail. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                            return GenerateErrorCode(ERR_CTRL_COATER_OBJECT_EXIST);
                        }
                    }
                }
                else // dry run
                {
                    if (bDetected || bAbsorbed)
                    {
                        WriteLog("CtrlCoater의 Rotate Chuck Table Vacuum Check Fail. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                        return GenerateErrorCode(ERR_CTRL_COATER_OBJECT_EXIST);
                    }
                }
            }

            return SUCCESS;
        }


        public int RotateStop()
        {
            return m_RefComp.SpinCoater.RotateStop();
        }

        public int StartRotateCW(int nSeq)
        {
            int nTime = 0, nRPM = 0;

            nTime = m_Data.CoatSequence[nSeq].Time;  // Run Time
            nRPM = m_Data.CoatSequence[nSeq].RPM;    // RPM

            if (CheckSafetyInterlock() != SUCCESS)
            {
                WriteLog("CtrlCoater의 Rotate 가동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
            }

            if (m_RefComp.SpinCoater.StartRotateCW(nRPM) == SUCCESS)
            {
                m_tmrOperation.StartTimer();

                while (true)
                {
                    if(m_tmrOperation.MoreThan(nTime, MTickTimer.ETimeType.TIME_SECOND))
                    {
                        m_tmrOperation.StopTimer();
                        break;
                    }
                }
            }

            return SUCCESS;
        }

        public int StartRotateCCW(int nSeq)
        {
            int nTime = 0, nRPM = 0;

            nTime = m_Data.CoatSequence[nSeq].Time;  // Run Time
            nRPM = m_Data.CoatSequence[nSeq].RPM;    // RPM

            int iResult = CheckSafetyInterlock();

            if (CheckSafetyInterlock() != SUCCESS)
            {
                WriteLog("CtrlCoater의 Rotate 가동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
            }

            if (m_RefComp.SpinCoater.StartRotateCCW(nRPM) == SUCCESS)
            {
                m_tmrOperation.StartTimer();

                while (true)
                {
                    if (m_tmrOperation.MoreThan(nTime, MTickTimer.ETimeType.TIME_SECOND))
                    {
                        m_tmrOperation.StopTimer();
                        break;
                    }
                }
            }

            return SUCCESS;
        }

        public int MoveToRotateLoadPos(bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            return m_RefComp.SpinCoater.MoveToRotateLoadPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToCoatNozzleSafetyPos(bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            return m_RefComp.SpinCoater.MoveCoatNozzletoSafetyPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToCoatNozzleStartPos(bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            int iResult = 0;

            if(CheckSafetyInterlock() != SUCCESS)
            {
                WriteLog("CtrlCoater의 Start 이동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
            }

            return m_RefComp.SpinCoater.MoveToCoatNozzleStartPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToCoatNozzleEndPos(bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            int iResult = 0;

            if (CheckSafetyInterlock() != SUCCESS)
            {
                WriteLog("CtrlCoater의 Start 이동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
            }

            return m_RefComp.SpinCoater.MoveToCoatNozzleEndPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }
    }
}
