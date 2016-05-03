using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_DataManager;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_CtrlCleaner;
using static LWDicer.Control.DEF_CtrlHandler;

namespace LWDicer.Control
{
    public class DEF_CtrlCleaner
    {
        public const int ERR_CTRL_CLEANER_UNABLE_TO_USE_HANDLER = 1;
        public const int ERR_CTRL_CLEANER_UNABLE_TO_USE_LOG = 2;
        public const int ERR_CTRL_CLEANER_OBJECT_ABSORBED = 3;
        public const int ERR_CTRL_CLEANER_OBJECT_NOT_ABSORBED = 4;
        public const int ERR_CTRL_CLEANER_OBJECT_EXIST = 5;
        public const int ERR_CTRL_CLEANER_OBJECT_NOT_EXIST = 6;
        public const int ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED = 7;
        public const int ERR_CTRL_CLEANER_CYLINDER_TIMEOUT = 8;
        public const int ERR_CTRL_CLEANER_NOT_UP = 9;
        public const int ERR_CTRL_CLEANER_CANNOT_DETECT_POSINFO = 10;
        public const int ERR_CTRL_CLEANER_PCB_DOOR_OPEN = 11;
        public const int ERR_CTRL_CLEANER_UHANDLER_IN_DOWN_AND_LHANDLER_IN_SAME_XZONE = 12;
        public const int ERR_CTRL_CLEANER_UHANDLER_NEED_DOWN_AND_LHANDLER_IN_SAME_XZONE = 13;
        public const int ERR_CTRL_CLEANER_LHANDLER_NEED_MOVE_AND_UHANDLER_IN_DOWN = 14;
        public const int ERR_CTRL_CLEANER_XAX_POS_NOT_MATCH_ZONE = 15;

        public class CCtrlCleaningRefComp
        {
            public MMeSpinner SpinCleaner;

            public CCtrlCleaningRefComp()
            {

            }
        }
    }

    public class MCtrlCleaner : MCtrlLayer
    {
        private CCtrlCleaningRefComp m_RefComp;
        private CCleaningData m_Data;

        MTickTimer m_tmrOperation = new MTickTimer();

        public MCtrlCleaner(CObjectInfo objInfo, CCtrlCleaningRefComp refComp, CCleaningData data) : base(objInfo)
        {
            SetData(data);
        }

        public int SetData(CCleaningData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CCleaningData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int Absorb(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.SpinCleaner.Absorb(bSkipSensor);
            return iResult;
        }

        public int Release(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.SpinCleaner.Release(bSkipSensor);
            return iResult;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            bStatus = false;

            int iResult = m_RefComp.SpinCleaner.IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsAbsorbed(out bool bStatus)
        {
            bStatus = false;

            int iResult = m_RefComp.SpinCleaner.IsAbsorbed(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsReleased(out bool bStatus)
        {
            bStatus = false;
            int iResult = m_RefComp.SpinCleaner.IsReleased(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
           
        public int IsTableUp(out bool bStatus)
        {
            bStatus = false;
            int iResult = m_RefComp.SpinCleaner.IsCylUp(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsTableDown(out bool bStatus)
        {
            bStatus = false;
            int iResult = m_RefComp.SpinCleaner.IsCylDown(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsRotateLoadPos()
        {
            int iResult = m_RefComp.SpinCleaner.CheckForRotateLoad();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCoatNozzleSafetyPos()
        {
            int iResult = m_RefComp.SpinCleaner.CheckForCleanNozzleSafety();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCleanNozzleSafetyPos()
        {
            int iResult = m_RefComp.SpinCleaner.CheckForCleanNozzleSafety();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        private int CheckSafetyInterlock()
        {
            int iResult = 0;
            bool bStatus = false;

            // 1. Wafer Vac Check
            iResult = IsAbsorbed(out bStatus);
            if (iResult != SUCCESS)
            {
                WriteLog("CtrlCleaner의 Rotate Chuck Table Vacuum Off.", ELogType.Debug, ELogWType.Error);
                return iResult;
            }

            // 2. Chuck Table Down Check
            iResult = IsTableDown(out bStatus);
            if (iResult != SUCCESS)
            {
                WriteLog("CtrlCleaner의 Rotate Chuck Table Not Down Pos.", ELogType.Debug, ELogWType.Error);
                return iResult;
            }

            return SUCCESS;
        }

        public int RotateStop()
        {
            return m_RefComp.SpinCleaner.RotateStop();
        }

        public int StartRotateCW(int nSeq)
        {
            int nTime = 0, nRPM = 0;

            nTime = m_Data.CleanSequence[nSeq].Time;  // Run Time
            nRPM = m_Data.CleanSequence[nSeq].RPM;    // RPM

            if (CheckSafetyInterlock() != SUCCESS)
            {
                WriteLog("CtrlCleaner의 Rotate 가동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
            }

            if (m_RefComp.SpinCleaner.StartRotateCW(nRPM) == SUCCESS)
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

        public int StartRotateCCW(int nSeq)
        {
            int nTime = 0, nRPM = 0;

            nTime = m_Data.CleanSequence[nSeq].Time;  // Run Time
            nRPM = m_Data.CleanSequence[nSeq].RPM;    // RPM

            int iResult = CheckSafetyInterlock();

            if (CheckSafetyInterlock() != SUCCESS)
            {
                WriteLog("CtrlCoater의 Rotate 가동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
            }

            if (m_RefComp.SpinCleaner.StartRotateCCW(nRPM) == SUCCESS)
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
            return m_RefComp.SpinCleaner.MoveToRotateLoadPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToCleanNozzleSafetyPos(bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            return m_RefComp.SpinCleaner.MoveToCleanNozzleSafetyPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToCleanNozzleStartPos(bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            int iResult = 0;

            if (CheckSafetyInterlock() != SUCCESS)
            {
                WriteLog("CtrlCleaner의 Start 이동 전 Cleaner Interlock", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
            }

            return m_RefComp.SpinCleaner.MoveToCleanNozzleStartPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToCleanNozzleEndPos(bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            int iResult = 0;

            if (CheckSafetyInterlock() != SUCCESS)
            {
                WriteLog("CtrlCleaner의 Start 이동 전 Cleaner Interlock", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
            }

            return m_RefComp.SpinCleaner.MoveToCleanNozzleEndPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }
    }
}
