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
        public const int ERR_CTRL_SPINNER_UNABLE_TO_USE_SPINNER                         = 1;
        public const int ERR_CTRL_SPINNER_UNABLE_TO_USE_LOG                             = 2;
        public const int ERR_CTRL_SPINNER_OBJECT_ABSORBED                               = 3;
        public const int ERR_CTRL_SPINNER_OBJECT_NOT_ABSORBED                           = 4;
        public const int ERR_CTRL_SPINNER_OBJECT_EXIST                                  = 5;
        public const int ERR_CTRL_SPINNER_OBJECT_NOT_EXIST                              = 6;
        public const int ERR_CTRL_SPINNER_CHECK_RUN_BEFORE_FAILED                       = 7;
        public const int ERR_CTRL_SPINNER_CHUCKTABLE_NOT_UP = 8;
        public const int ERR_CTRL_SPINNER_CHUCKTABLE_NOT_DOWN = 9;


        public enum ESpinnerIndex
        {
            SPINNER1,
            SPINNER2,
            MAX,
        }

        public enum ENozzleOpMode
        {
            NONE = -1,
            WAIT,
            PRE_WASH,
            DRY,
            DRY_C,
            COAT,
            COAT_M,
            FCLN_E,
            RINS,
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

        public class CSpinnerData
        {
            public CCoatingData CoaterData = new CCoatingData();
            public CCleaningData CleanerData = new CCleaningData();
        }

        public class CCoatingData
        {
            // Coating Data
            public int PVAQty;          // Coating 용액량 ex)40ml
            public int MovingPVAQty;    // Coating 용액 보관 통에서 노즐까지의 용액량 ex) 35ml
            public int CoatingRate;     // 도포량 5ml/sec
            public int CenterWaitTime;  // Nozzle이 도포 시작 위치에 와서, 도포하기전에 잠시 대기 시간 ex) 0
            public int MoveMode;        // ex) manual , auto
            public int MovingSpeed;     // Nozzle 왕복 이동 속도 ex) 6deg/sec
            public double CoatingArea;  // wafer에서 도포 되는 영역. ex) 280mm

            // Coating Sequence
            public CSpinStep[] StepSequence = new CSpinStep[DEF_MAX_SPINNER_STEP];

            public CCoatingData()
            {
                for (int i = 0; i < StepSequence.Length; i++)
                {
                    StepSequence[i] = new CSpinStep();
                }
            }
        }

        public class CSpinStep
        {
            public bool Use;              // 사용 여부
            public ENozzleOpMode OpMode;  // 동작의 종류 ex) wash, coating, dry
            public int OpTime;            // 스텝 지속시간 ex) 30sec when prewash, 60sec when dry
            public int RPMSpeed;             // rotate 회전속도 ex) 500rpm when prewash, 1500rpm when dry
        }

        public class CCleaningData
        {
            public double WashStroke;   // Table Cleaning도 겸할 수 있도록 Nozzle의 이동거리 ex) 10mm

            public CSpinStep[] StepSequence = new CSpinStep[DEF_MAX_SPINNER_STEP];

            public CCleaningData()
            {
                for (int i = 0; i < StepSequence.Length; i++)
                {
                    StepSequence[i] = new CSpinStep();
                }
            }
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

        #region Common : Manage Data, Position, Use Flag and Initialize
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

        public int DoSpinnerOperation(bool bCleanMode, int nSeq)
        {
            CSpinStep step;
            if(bCleanMode)
            {
                step = m_Data.CleanerData.StepSequence[nSeq];
            } else
            {
                step = m_Data.CoaterData.StepSequence[nSeq];
            }

            // 1. Check Interlock
            int iResult = CheckSafetyInterlock();
            if (iResult != SUCCESS) return iResult;

            // 2. Rotate Run
            StartRotateCW(step.RPMSpeed);

            // 3. Do Nozzle Operation
            switch (step.OpMode)
            {
                case ENozzleOpMode.WAIT:
                    break;

                case ENozzleOpMode.PRE_WASH:
                    break;

                case ENozzleOpMode.DRY:
                    break;

                case ENozzleOpMode.DRY_C:
                    break;

                case ENozzleOpMode.COAT:
                    break;

                case ENozzleOpMode.COAT_M:
                    break;

                case ENozzleOpMode.FCLN_E:
                    break;

                case ENozzleOpMode.RINS:
                    break;
            }
            return SUCCESS;
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


        public int RotateStop()
        {
            return m_RefComp.Spinner.RotateStop();
        }

        public int StartRotateCW(int nRPM)
        {
            int iResult = CheckSafetyInterlock();
            if (iResult != SUCCESS) return SUCCESS;

            iResult = m_RefComp.Spinner.StartRotateCW(nRPM);
            if (iResult != SUCCESS) return SUCCESS;

            return SUCCESS;
        }

        public int StartRotateCCW(int nRPM)
        {
            int iResult = CheckSafetyInterlock();
            if (iResult != SUCCESS) return SUCCESS;

            iResult = m_RefComp.Spinner.StartRotateCCW(nRPM);
            if (iResult != SUCCESS) return SUCCESS;

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
