using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_DataManager;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_CtrlSpinner;

namespace LWDicer.Control
{
    public class DEF_CtrlSpinner
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

        public enum ESpinnerIndex
        {
            SPINNER1,
            SPINNER2,
            MAX,
        }

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

        public enum ECleanerOP
        {
            NONE = -1,
            PRE_WASH,
            WASH,
            RINS,
            DRY,
            MAX,
        }

        public enum ESpinnerStep
        {
            NONE = -1,
            STEP_1 = 0,
            STEP_2,
            STEP_3,
            STEP_4,
            STEP_5,
            STEP_6,
            STEP_7,
            STEP_8,
            STEP_9,
            STEP_10,
            STEP_11,
            STEP_12,
            STEP_13,
            STEP_14,
            STEP_15,
            STEP_16,
            MAX,
        }

        public class CCtrlSpinnerRefComp
        {
            public MMeSpinner SpinCleaner;
            public MMeSpinner SpinCoater;

            public CCtrlSpinnerRefComp()
            {

            }
        }

        public class CSpinnerData
        {
            public CCoatingData CoaterData = new CCoatingData();
            public CCleaningData CleanerData = new CCleaningData();
        }

        public class CCoatingData
        {
            // Coating Data
            public int PVAQty;
            public int MovingPVAQty;
            public int CoatingRate;
            public int CenterWaitTime;
            public int MoveMode;
            public int MovingSpeed;
            public double CoatingArea;

            // Coating Sequence
            public CCoatingOperation[] CoatSequence = new CCoatingOperation[(int)ESpinnerStep.MAX];

            public CCoatingData()
            {
                for (int i = 0; i < CoatSequence.Length; i++)
                {
                    CoatSequence[i] = new CCoatingOperation();
                }
            }
        }

        public class CCoatingOperation
        {
            public bool Use;
            public int OpTime;
            public int OpRPM;

            public int CoterStep = -1;
            public int CleanStep = -1;

            public bool StopAfterOp;

            public int Operation;
            public int Time;
            public int RPM;
        }

        public class CCleaningData
        {
            public double WashStroke;

            public CCleaningSquence[] CleanSequence = new CCleaningSquence[(int)ESpinnerStep.MAX];

            public CCleaningData()
            {
                for (int i = 0; i < CleanSequence.Length; i++)
                {
                    CleanSequence[i] = new CCleaningSquence();
                }
            }
        }

        public class CCleaningSquence
        {
            public int Operation;
            public int Time;
            public int RPM;
        }
    }

    public class MCtrlSpinner : MCtrlLayer
    {
        private CCtrlSpinnerRefComp m_RefComp;
        private CSpinnerData m_Data;

        MTickTimer m_tmrOperation = new MTickTimer();

        public MCtrlSpinner(CObjectInfo objInfo, CCtrlSpinnerRefComp refComp, CSpinnerData data) : base(objInfo)
        {
            SetData(data);
        }

        public int SetData(CSpinnerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CSpinnerData target)
        {
            target = ObjectExtensions.Copy(m_Data);
            return SUCCESS;
        }

        public MMeSpinner GetSpinner(ESpinnerIndex index)
        {
            if (index == ESpinnerIndex.SPINNER1)
            {
                return m_RefComp.SpinCleaner;
            }
            else
            {
                return m_RefComp.SpinCoater;
            }
        }

        public MMeSpinner GetOtherSpinner(ESpinnerIndex index)
        {
            if (index == ESpinnerIndex.SPINNER1)
            {
                return m_RefComp.SpinCleaner;
            }
            else
            {
                return m_RefComp.SpinCoater;
            }
        }

        public int Absorb(ESpinnerIndex index, bool bSkipSensor = false)
        {
            int iResult = GetSpinner(index).Absorb(bSkipSensor);
            return iResult;
        }

        public int Release(ESpinnerIndex index, bool bSkipSensor = false)
        {
            int iResult = GetSpinner(index).Release(bSkipSensor);
            return iResult;
        }

        public int IsObjectDetected(ESpinnerIndex index, out bool bStatus)
        {
            bStatus = false;

            int iResult = GetSpinner(index).IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            
            return SUCCESS;
        }

        public int IsAbsorbed(ESpinnerIndex index, out bool bStatus)
        {
            bStatus = false;

            int iResult = GetSpinner(index).IsAbsorbed(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsReleased(ESpinnerIndex index, out bool bStatus)
        {
            bStatus = false;
            int iResult = GetSpinner(index).IsReleased(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        
        public int TableUp(ESpinnerIndex index, bool bSkipSensor)
        {
            if (CheckSafetyInterlock(index) != SUCCESS)
            {
                if(index == ESpinnerIndex.SPINNER1)
                {
                    WriteLog("Cleaner의 Rotate 가동 전 Cleaner Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
                }
                else
                {
                    WriteLog("Coater의 Rotate 가동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
                }
            }

            int iResult = GetSpinner(index).CylUp();
            return SUCCESS;
        }


        public int TableDown(ESpinnerIndex index, bool bSkipSensor)
        {
            int iResult = GetSpinner(index).CylDown();
            return SUCCESS;
        }

        
        public int IsTableUp(ESpinnerIndex index, out bool bStatus)
        {
            bStatus = false;
            int iResult = GetSpinner(index).IsCylUp(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsTableDown(ESpinnerIndex index, out bool bStatus)
        {
            bStatus = false;
            int iResult = GetSpinner(index).IsCylDown(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }


        public int IsRotateLoadPos(ESpinnerIndex index)
        {
            int iResult = GetSpinner(index).CheckForRotateLoad();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCoatNozzleSafetyPos(ESpinnerIndex index)
        {
            int iResult = GetSpinner(index).CheckForCoatNozzleSafety();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCleanNozzleSafetyPos(ESpinnerIndex index)
        {
            int iResult = GetSpinner(index).CheckForCleanNozzleSafety();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int DoSpinnerOperation(ESpinnerIndex index, int nSeq)
        {
            int nTime = 0, nRPM = 0, nSpinnerOP = -1;

            if (index == ESpinnerIndex.SPINNER1)
            {
                nTime = m_Data.CleanerData.CleanSequence[nSeq].Time;            // Run Time
                nRPM = m_Data.CleanerData.CleanSequence[nSeq].RPM;              // RPM
                nSpinnerOP = m_Data.CleanerData.CleanSequence[nSeq].Operation;  // Nozzle Operation
            }
            else
            {
                nTime = m_Data.CoaterData.CoatSequence[nSeq].Time;              // Run Time
                nRPM = m_Data.CoaterData.CoatSequence[nSeq].RPM;                // RPM
                nSpinnerOP = m_Data.CoaterData.CoatSequence[nSeq].Operation;    // Nozzle Operation
            }

            // 1. Vacuum & Chuck Table Check
            if (CheckSafetyInterlock(index) != SUCCESS)
            {
                if (index == ESpinnerIndex.SPINNER1)
                {
                    WriteLog("Cleaner의 Rotate 가동 전 Cleaner Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
                }
                else
                {
                    WriteLog("Coater의 Rotate 가동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
                }
            }

            // 2. Rotate Run
            StartRotateCW(index,nRPM);

            // 4. Nozzle Operation
            NozzeleOperation(index, nSpinnerOP);

            return SUCCESS;
        }

        private int NozzeleOperation(ESpinnerIndex index, int nSpinnerOP)
        {
            if(index == ESpinnerIndex.SPINNER2)
            {
                CoaterNozzleOP(index, nSpinnerOP);
            }
            else
            {
                ClenerNozzleOP(index, nSpinnerOP);
            }

            return SUCCESS;
        }

        private int CoaterNozzleOP(ESpinnerIndex index, int NozzleOP)
        {
            switch (NozzleOP)
            {
                case (int)ECoaterOp.P_WASH:
                    break;

                case (int)ECoaterOp.DRY:
                    break;

                case (int)ECoaterOp.DRY_C:
                    break;

                case (int)ECoaterOp.COAT:
                    break;

                case (int)ECoaterOp.COAT_M:
                    break;

                case (int)ECoaterOp.FCLN_E:
                    break;

                case (int)ECoaterOp.WAIT:
                    break;
            }

            return SUCCESS;
        }

        private int ClenerNozzleOP(ESpinnerIndex index, int NozzleOP)
        {
            switch (NozzleOP)
            {
                case (int)ECleanerOP.PRE_WASH:
                    break;

                case (int)ECleanerOP.WASH:
                    break;

                case (int)ECleanerOP.RINS:
                    break;

                case (int)ECleanerOP.DRY:
                    break;
            }

            return SUCCESS;
        }


        private int CheckSafetyInterlock(ESpinnerIndex index)
        {
            int iResult = 0;
            bool bStatus = false;

            if(AutoRunMode != EAutoRunMode.DRY_RUN)
            {
                // 1. Wafer Vac Check
                iResult = IsAbsorbed(index, out bStatus);
                if (iResult != SUCCESS)
                {
                    if (index == ESpinnerIndex.SPINNER1)
                    {
                        WriteLog("Cleaner의 Rotate Chuck Table Vacuum Off.", ELogType.Debug, ELogWType.Error);
                        return iResult;
                    }
                    else
                    {
                        WriteLog("Coater의 Rotate Chuck Table Vacuum Off.", ELogType.Debug, ELogWType.Error);
                        return iResult;
                    }
                }
            }

            // 2. Chuck Table Down Check
            iResult = IsTableDown(index, out bStatus);
            if (iResult != SUCCESS)
            {
                if (index == ESpinnerIndex.SPINNER1)
                {
                    WriteLog("Cleaner의 Rotate Chuck Table Not Down Pos.", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
                else
                {
                    WriteLog("Coater의 Rotate Chuck Table Not Down Pos.", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }

            return SUCCESS;
        }

        private int CheckVacuum_forRotateMove(ESpinnerIndex index, bool bWaferTransfer)
        {
            int iResult = SUCCESS;
            bool bDetected, bAbsorbed;

            // check vacuum
            iResult = IsObjectDetected(index, out bDetected);
            if (iResult != SUCCESS) return iResult;

            iResult = IsAbsorbed(index, out bAbsorbed);
            if (iResult != SUCCESS) return iResult;

            if (bWaferTransfer)
            {
                if (bDetected == true && bAbsorbed == false)
                {
                    iResult = Absorb(index);
                    if (iResult != SUCCESS) return iResult;

                    bAbsorbed = true;
                }
            }

            // check object exist when auto run
            if (AutoManualMode == EAutoManual.AUTO)
            {
                if (AutoRunMode != EAutoRunMode.DRY_RUN)
                {
                    if (bDetected != bWaferTransfer)
                    {
                        if (bWaferTransfer)    // Wafer Exist
                        {
                            if (index == ESpinnerIndex.SPINNER1)
                            {
                                WriteLog("Cleaner의 Rotate Chuck Table Vacuum Check Fail. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                                return GenerateErrorCode(ERR_CTRL_CLEANER_OBJECT_NOT_EXIST);
                            }
                            else
                            {
                                WriteLog("Coater의 Rotate Chuck Table Vacuum Check Fail. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                                return GenerateErrorCode(ERR_CTRL_COATER_OBJECT_NOT_EXIST);
                            }
                        }
                        else
                        {
                            if (index == ESpinnerIndex.SPINNER1)
                            {
                                WriteLog("Cleaner의 Rotate Chuck Table Vacuum Check Fail. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                                return GenerateErrorCode(ERR_CTRL_CLEANER_OBJECT_EXIST);
                            }
                            else
                            {
                                WriteLog("Coater의 Rotate Chuck Table Vacuum Check Fail. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                                return GenerateErrorCode(ERR_CTRL_COATER_OBJECT_EXIST);
                            }
                        }
                    }
                }
                else // dry run
                {
                    if (bDetected || bAbsorbed)
                    {
                        if (index == ESpinnerIndex.SPINNER1)
                        {
                            WriteLog("Cleaner의 Rotate Chuck Table Vacuum Check Fail. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                            return GenerateErrorCode(ERR_CTRL_CLEANER_OBJECT_EXIST);
                        }
                        else
                        {
                            WriteLog("Coater의 Rotate Chuck Table Vacuum Check Fail. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                            return GenerateErrorCode(ERR_CTRL_COATER_OBJECT_EXIST);
                        }
                    }
                }
            }

            return SUCCESS;
        }


        public int RotateStop(ESpinnerIndex index)
        {
            return GetSpinner(index).RotateStop();
        }

        public int StartRotateCW(ESpinnerIndex index, int nRPM)
        {
            if (CheckSafetyInterlock(index) != SUCCESS)
            {
                if (index == ESpinnerIndex.SPINNER1)
                {
                    WriteLog("Cleaner의 Rotate 가동 전 Cleaner Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
                }
                else
                {
                    WriteLog("Coater의 Rotate 가동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
                }
            }

            GetSpinner(index).StartRotateCW(nRPM);

            return SUCCESS;
        }

        public int StartRotateCCW(ESpinnerIndex index, int nRPM)
        {
            if (CheckSafetyInterlock(index) != SUCCESS)
            {
                if(index == ESpinnerIndex.SPINNER1)
                {
                    WriteLog("Cleaner의 Rotate 가동 전 Cleaner Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
                }
                else
                {
                    WriteLog("Coater의 Rotate 가동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
                }
            }

            GetSpinner(index).StartRotateCCW(nRPM);

            return SUCCESS;
        }

        public int MoveRotateToLoadPos(ESpinnerIndex index, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            return GetSpinner(index).MoveRotateToLoadPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveCoatNozzleToSafetyPos(ESpinnerIndex index, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            return GetSpinner(index).MoveCoatNozzletoSafetyPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveCoatNozzleToStartPos(ESpinnerIndex index, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            int iResult = 0;

            if(CheckSafetyInterlock(index) != SUCCESS)
            {
                if(index == ESpinnerIndex.SPINNER1)
                {
                    WriteLog("Cleaner의 Start 이동 전 Cleaner Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
                }
                else
                {
                    WriteLog("Coater의 Start 이동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
                }
            }

            return GetSpinner(index).MoveCoatNozzleToStartPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveCoatNozzleToEndPos(ESpinnerIndex index, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            int iResult = 0;

            if (CheckSafetyInterlock(index) != SUCCESS)
            {
                if (index == ESpinnerIndex.SPINNER1)
                {
                    WriteLog("Cleaner의 Start 이동 전 Cleaner Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_CLEANER_CHECK_RUN_BEFORE_FAILED);
                }
                else
                {
                    WriteLog("Coater의 Start 이동 전 Coater Interlock", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_CTRL_COATER_CHECK_RUN_BEFORE_FAILED);
                }
            }

            return GetSpinner(index).MoveCoatNozzleToEndPos(bMoveAllAxis, bMoveXYT, bMoveZ);
        }
    }
}
