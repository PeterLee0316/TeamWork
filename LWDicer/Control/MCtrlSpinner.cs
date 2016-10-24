using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_CtrlSpinner;

namespace LWDicer.Layers
{
    public class DEF_CtrlSpinner
    {
        public const int ERR_CTRL_SPINNER_UNABLE_TO_USE_SPINNER                         = 1;
        public const int ERR_CTRL_SPINNER_UNABLE_TO_USE_LOG                             = 2;
        public const int ERR_CTRL_SPINNER_OBJECT_ABSORBED                               = 3;
        public const int ERR_CTRL_SPINNER_OBJECT_NOT_ABSORBED                           = 4;
        public const int ERR_CTRL_SPINNER_OBJECT_EXIST                                  = 5;
        public const int ERR_CTRL_SPINNER_OBJECT_NOT_EXIST                              = 6;
        public const int ERR_CTRL_SPINNER_CHECK_RUN_BEFORE_FAILED                       = 7;
        public const int ERR_CTRL_SPINNER_CHUCKTABLE_NOT_UP                             = 8;
        public const int ERR_CTRL_SPINNER_CHUCKTABLE_NOT_DOWN                           = 9;
        public const int ERR_CTRL_SPINNER_CANCEL_COATING_JOB                            = 10;
        public const int ERR_CTRL_SPINNER_CANCEL_CLEANING_JOB                           = 11;

        /// <summary>
        /// type of Cleaning Operation
        /// </summary>
        public enum ECleanOperation
        {
            NO_USE,
            WAIT,
            PRE_WASH,
            WASH,
            RINSE,
            DRY,
            MAX,
        }

        /// <summary>
        /// type of Coating Operation
        /// </summary>
        public enum ECoatOperation
        {
            NO_USE,
            WAIT,
            PRE_WASH,
            DRY,
            DRY_C,
            COAT,
            COAT_M,
            FCLN_S,
            FCLN_E,
            MAX,
        }

        public const int DEF_MAX_SPINNER_STEP = 16;

        public class CCtrlSpinnerRefComp
        {
            public MMeSpinner Spinner;

            public CCtrlSpinnerRefComp()
            {

            }
        }

        public class CCtrlSpinnerData
        {
            public CCoaterData CoaterData = new CCoaterData();
            public CCleanerData CleanerData = new CCleanerData();
        }

        /// <summary>
        /// define operation step. 
        /// operate when use == true && operation != no_use && optime > 0
        /// </summary>
        public class CCoatingStep
        {
            public bool Use;                    // 사용 여부
            public ECoatOperation Operation;    // 동작의 종류 ex) wash, coating, dry
            public double OpTime;                  // 스텝 지속시간 ex) 30sec when prewash, 60sec when dry
            public int RPMSpeed;                // rotate 회전속도 ex) 500rpm when prewash, 1500rpm when dry

            public CCoatingStep()
            {
                Use = true;     // 기본적으로는 Use는 사용으로, Operation은 No_Use
                Operation = ECoatOperation.NO_USE;
            }
        }

        public class CCoaterData
        {
            public bool RotateCW = true;            // default rotate direction

            // Coating Data
            public int PVAQty;                      // Coating 용액량 ex)40ml
            public int MovingPVAQty;                // Coating 용액 보관 통에서 노즐까지의 용액량 ex) 35ml
            public int CoatingRate;                 // 도포량 5ml/sec
            public int CenterWaitTime;              // Nozzle이 도포 시작 위치에 와서, 도포하기전에 잠시 대기 시간 ex) 0
            public int MoveMode;                    // ex) manual , auto
            public double NozzleSpeed;              // Nozzle 왕복 이동 속도 ex) 6deg/sec
            public double CoatingArea;              // wafer에서 도포 되는 영역. ex) 280mm
            public double CoatingTime_Calculated;   // coating에 걸리는 시간 : auto calculated, read only
            public double MovingTime_Calculated;    // coating에 걸리는 시간 : auto calculated, read only

            // Coating Sequence
            public CCoatingStep[] WorkSteps_Custom = new CCoatingStep[DEF_MAX_SPINNER_STEP];    // custom coating steps

            public CCoaterData()
            {
                for (int i = 0; i < WorkSteps_Custom.Length; i++)
                {
                    WorkSteps_Custom[i] = new CCoatingStep();
                }
            }
        }

        /// <summary>
        /// define operation step. 
        /// operate when use == true && operation != no_use && optime > 0
        /// </summary>
        public class CCleaningStep
        {
            public bool Use;                      // 사용 여부
            public ECleanOperation Operation;     // 동작의 종류 ex) wash, coating, dry
            public double OpTime;                    // 스텝 지속시간 ex) 30sec when prewash, 60sec when dry
            public int RPMSpeed;                  // rotate 회전속도 ex) 500rpm when prewash, 1500rpm when dry

            public CCleaningStep()
            {
                Use = true;     // 기본적으로는 Use는 사용으로, Operation은 No_Use
                Operation = ECleanOperation.NO_USE;
            }
        }

        public class CCleanerData
        {
            public bool RotateCW = true;            // default rotate direction

            public bool UseWashSteps_General;       // use general / custom wash steps
            public double WashStroke;               // Table Cleaning도 겸할 수 있도록 Nozzle의 이동거리 ex) work size + 10mm

            public bool EnableThoroughCleaning;     // Nozzle 을 움직이면서 (use NozzleSpeed) cleaing 할지 여부
            public double NozzleSpeed;              // Nozzle 왕복 이동 속도 ex) 6deg/sec

            public CCleaningStep[] WorkSteps_General;      // general washing steps. fixed.
            public CCleaningStep[] WorkSteps_Custom = new CCleaningStep[DEF_MAX_SPINNER_STEP];    // custom washing steps

            public CCleaningStep[] TableSteps;             // general washing steps. fixed.
            public CCleaningStep[] CaseSteps;              // general washing steps. fixed.
            public CCleaningStep[] DiskSteps;              // general washing steps. fixed.

            public CCleanerData()
            {
                // work washing steps
                WorkSteps_General = new CCleaningStep[4];
                for (int i = 0; i < WorkSteps_General.Length; i++)
                {
                    WorkSteps_General[i] = new CCleaningStep();
                }
                WorkSteps_General[0].Operation = ECleanOperation.PRE_WASH;
                WorkSteps_General[1].Operation = ECleanOperation.WASH;
                WorkSteps_General[2].Operation = ECleanOperation.RINSE;
                WorkSteps_General[3].Operation = ECleanOperation.DRY;

                for (int i = 0; i < WorkSteps_Custom.Length; i++)
                {
                    WorkSteps_Custom[i] = new CCleaningStep();
                }

                // table washing steps
                TableSteps = new CCleaningStep[2];
                for (int i = 0; i < TableSteps.Length; i++)
                {
                    TableSteps[i] = new CCleaningStep();
                }
                TableSteps[0].Operation = ECleanOperation.WASH;
                TableSteps[1].Operation = ECleanOperation.DRY;

                // case washing steps
                CaseSteps = new CCleaningStep[1];
                for (int i = 0; i < CaseSteps.Length; i++)
                {
                    CaseSteps[i] = new CCleaningStep();
                }
                CaseSteps[0].Operation = ECleanOperation.WASH;

                // disk washing steps
                DiskSteps = new CCleaningStep[2];
                for (int i = 0; i < DiskSteps.Length; i++)
                {
                    DiskSteps[i] = new CCleaningStep();
                }
                DiskSteps[0].Operation = ECleanOperation.WASH;
            }
        }
    }

    public class MCtrlSpinner : MCtrlLayer
    {
        private CCtrlSpinnerRefComp m_RefComp;
        private CCtrlSpinnerData m_Data;

        public bool IsDoingJob { get; private set; } = false;
        public bool IsCancelJob_byManual = false;       // cancel coating/cleaning job by manual, return success
        public bool IsCancelJob_byAuto = false;         // cancel coating/cleaning job by auto, return error

        public MCtrlSpinner(CObjectInfo objInfo, CCtrlSpinnerRefComp refComp, CCtrlSpinnerData data) : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CCtrlSpinnerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CCtrlSpinnerData target)
        {
            target = ObjectExtensions.Copy(m_Data);
            return SUCCESS;
        }

        public override int Initialize()
        {
            int iResult;
            bool bStatus;

            iResult = CheckVacuumSafety();
            if (iResult != SUCCESS) return iResult;

            iResult = TableDown();
            if (iResult != SUCCESS) return iResult;

            iResult = MoveCleanNozzleToSafetyPos();
            if (iResult != SUCCESS) return iResult;

            iResult = MoveCoatNozzleToSafetyPos();
            if (iResult != SUCCESS) return iResult;

            iResult = MoveRotateToLoadPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        #endregion

        #region Cylinder, Vacuum, Detect Object
        public int Absorb(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.Spinner.Absorb(bSkipSensor);
            return iResult;
        }

        public int Release(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.Spinner.Release(bSkipSensor);
            return iResult;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            bStatus = false;

            int iResult = m_RefComp.Spinner.IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            
            return SUCCESS;
        }

        public int IsAbsorbed(out bool bStatus)
        {
            bStatus = false;

            int iResult = m_RefComp.Spinner.IsAbsorbed(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsReleased(out bool bStatus)
        {
            bStatus = false;
            int iResult = m_RefComp.Spinner.IsReleased(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        
        public int TableUp(bool bSkipSensor = false)
        {
            // 0. check pushpull

            // 1. check vacuum
            int iResult = CheckVacuumSafety();
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Spinner.ChuckTableUp();
            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }


        public int TableDown(bool bSkipSensor = false)
        {
            // 0. check pushpull

            // 1. check vacuum
            int iResult = CheckVacuumSafety();
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Spinner.ChuckTableDown();
            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }


        public int IsTableUp(out bool bStatus)
        {
            int iResult = m_RefComp.Spinner.IsChuckTableUp(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsTableDown(out bool bStatus)
        {
            int iResult = m_RefComp.Spinner.IsChuckTableDown(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        #endregion

        public int IsRotateInLoadPos(out bool bStatus)
        {
            int iResult = m_RefComp.Spinner.IsRotateInLoadPos(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCoatNozzleInSafetyPos(out bool bStatus)
        {
            int iResult = m_RefComp.Spinner.CheckCoatNozzleInSafetyZone(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCleanNozzleInSafetyPos(out bool bStatus)
        {
            int iResult = m_RefComp.Spinner.CheckCleanNozzleInSafetyZone(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int DoCleanOperation(int nSeq)
        {
            //CCleaningStep step = m_Data.CleanerData.Steps[nSeq];

            //// 1. Check Interlock
            //int iResult = CheckSafetyInterlock();
            //if (iResult != SUCCESS) return iResult;

            //// 2. Rotate Run
            //StartRotateCW(step.RPMSpeed);

            //// 3. Do Nozzle Operation
            //switch (step.Mode)
            //{
            //    case ECleanOperation.WAIT:
            //        break;

            //    case ECleanOperation.PRE_WASH:
            //        break;

            //    case ECleanOperation.WASH:
            //        break;

            //    case ECleanOperation.DRY:
            //        break;

            //    case ECleanOperation.RINSE:
            //        break;
            //}
            return SUCCESS;
        }

        public int DoCoatOperation()
        {
            // 0. Check Interlock
            int iResult = CheckSafetyInterlock();
            if (iResult != SUCCESS) return iResult;

            // 0.5 check material

            // 1. move nozzle to work pos ?

            // 2. do coating
            IsCancelJob_byAuto = IsCancelJob_byManual = false;
            IsDoingJob = true;
            MTickTimer tTimer = new MTickTimer();
            for(int i = 0; i < m_Data.CoaterData.WorkSteps_Custom.Length; i++)
            {
                CCoatingStep step = m_Data.CoaterData.WorkSteps_Custom[i];
                Debug.WriteLine(step);
                if (step.Use == false || step.Operation == ECoatOperation.NO_USE || step.OpTime <= 0)
                    continue;

                // 2.1 rotate 
                iResult = StartRotate(step.RPMSpeed, m_Data.CoaterData.RotateCW);
                if (iResult != SUCCESS) goto ERROR_OCCURED;

                // 2.2 open nozzle
                switch (step.Operation)
                {
                    case ECoatOperation.WAIT:
                        break;

                    case ECoatOperation.PRE_WASH:
                        break;

                    case ECoatOperation.DRY:
                        break;

                    case ECoatOperation.DRY_C:
                        break;

                    case ECoatOperation.COAT:
                        break;

                    case ECoatOperation.COAT_M:
                        break;

                    case ECoatOperation.FCLN_E:
                        break;
                }

                tTimer.StartTimer();
                while(tTimer.LessThan(step.OpTime, MTickTimer.ETimeType.SECOND))
                {
                    if(IsCancelJob_byAuto || IsCancelJob_byManual)
                    {
                        goto ERROR_OCCURED;
                    }
                }
            }
            return SUCCESS;

            ERROR_OCCURED:
            // close valve
            m_RefComp.Spinner.CleanNozzleValveClose();
            m_RefComp.Spinner.CoatNozzleValveClose();

            // stop rotate
            StopRotate();

            IsDoingJob = false;
            if (IsCancelJob_byManual) return SUCCESS;
            if (IsCancelJob_byAuto) return GenerateErrorCode(ERR_CTRL_SPINNER_CANCEL_COATING_JOB);

            return iResult;
        }

        private int CheckSafetyInterlock()
        {
            int iResult;
            bool bStatus;

            // 1. check vacuum
            iResult = CheckVacuumSafety();
            if (iResult != SUCCESS) return iResult;

            // 2. Chuck Table Down Check
            iResult = IsTableDown(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus != false) return GenerateErrorCode(ERR_CTRL_SPINNER_CHUCKTABLE_NOT_DOWN);

            return SUCCESS;
        }

        private int CheckVacuumSafety()
        {
            bool bStatus;
            int iResult = IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (AutoRunMode == EAutoRunMode.DRY_RUN)
            {
                if (bStatus) return GenerateErrorCode(ERR_CTRL_SPINNER_OBJECT_EXIST);
            }
            if (bStatus)
            {
                iResult = Absorb();
                if (iResult != SUCCESS) return iResult;
            }
            else
            {
                iResult = Release();
                if (iResult != SUCCESS) return iResult;
            }


            return SUCCESS;
        }


        public int StopRotate()
        {
            return m_RefComp.Spinner.StopRotate();
        }

        public int StartRotate(int nRPM, bool bCWDir = true)
        {
            int iResult = CheckSafetyInterlock();
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Spinner.StartRotate(nRPM, bCWDir);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveRotateToLoadPos(double dMoveOffset = 0)
        {
            // Up/Down이 아닌 Rotate의 회전, Nozzle의 이동 동작 interlock은 Mechanical Layer에서 처리했음
            return m_RefComp.Spinner.MoveRotateToLoadPos(dMoveOffset);
        }

        public int MoveCoatNozzleToSafetyPos(double dMoveOffset = 0)
        {
            // Up/Down이 아닌 Rotate의 회전, Nozzle의 이동 동작 interlock은 Mechanical Layer에서 처리했음
            return m_RefComp.Spinner.MoveCoatNozzleToSafetyPos(dMoveOffset);
        }

        public int MoveCoatNozzleToStartPos(double dMoveOffset = 0)
        {
            // Up/Down이 아닌 Rotate의 회전, Nozzle의 이동 동작 interlock은 Mechanical Layer에서 처리했음
            return m_RefComp.Spinner.MoveCoatNozzleToStartPos(dMoveOffset);
        }

        public int MoveCoatNozzleToEndPos(double dMoveOffset = 0)
        {
            // Up/Down이 아닌 Rotate의 회전, Nozzle의 이동 동작 interlock은 Mechanical Layer에서 처리했음
            return m_RefComp.Spinner.MoveCoatNozzleToStartPos(dMoveOffset);
        }

        public int MoveCleanNozzleToSafetyPos(double dMoveOffset = 0)
        {
            // Up/Down이 아닌 Rotate의 회전, Nozzle의 이동 동작 interlock은 Mechanical Layer에서 처리했음
            return m_RefComp.Spinner.MoveCleanNozzleToSafetyPos(dMoveOffset);
        }

        public int MoveCleanNozzleToStartPos(double dMoveOffset = 0)
        {
            // Up/Down이 아닌 Rotate의 회전, Nozzle의 이동 동작 interlock은 Mechanical Layer에서 처리했음
            return m_RefComp.Spinner.MoveCleanNozzleToStartPos(dMoveOffset);
        }

        public int MoveCleanNozzleToEndPos(double dMoveOffset = 0)
        {
            // Up/Down이 아닌 Rotate의 회전, Nozzle의 이동 동작 interlock은 Mechanical Layer에서 처리했음
            return m_RefComp.Spinner.MoveCleanNozzleToStartPos(dMoveOffset);
        }
    }
}
