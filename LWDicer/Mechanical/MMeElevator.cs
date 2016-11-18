using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_MeElevator;
using static LWDicer.Layers.DEF_Motion;
using static LWDicer.Layers.DEF_IO;
using static LWDicer.Layers.DEF_Vacuum;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.Layers
{
    public class DEF_MeElevator
    {
        public const int ERR_ELEVATOR_NOT_ORIGIN_RETURNED                    = 1;
        public const int ERR_ELEVATOR_UNABLE_TO_USE_CYL                      = 2;
        public const int ERR_ELEVATOR_UNABLE_TO_USE_VCC                      = 3;
        public const int ERR_ELEVATOR_UNABLE_TO_USE_AXIS                     = 4;
        public const int ERR_ELEVATOR_FAIL_TO_GET_CURRENT_POS_INFO           = 5;
        public const int ERR_ELEVATOR_MOVE_FAIL                              = 6;
        public const int ERR_ELEVATOR_CASSETTE_NOT_READY                     = 7;
        public const int ERR_ELEVATOR_CASSETTE_DETECTED_ABNORMAL             = 8;
        public const int ERR_ELEVATOR_OBJECT_DETECTED                        = 9;
        public const int ERR_ELEVATOR_OBJECT_NOT_DETECTED                    = 10;
        public const int ERR_ELEVATOR_NOT_EXIST_PRE_PROCESS_WAFER            = 11;
        public const int ERR_ELEVATOR_NOT_EXIST_AFTER_PROCESS_WAFER          = 12;
        public const int ERR_ELEVATOR_NOT_EXIST_EMPTY_SLOT                   = 13;
        

        public enum EElevatorPos
        {
            NONE = -1,
            BOTTOM = 0,
            LOAD,
            SLOT,
            TOP,
            SAFETY,
            MAX,
        }

        public enum EElevatorXAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EElevatorYAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EElevatorTAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EElevatorZAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        //===============================================================================
        //  Cassette Info
        public const double CASSETTE_DEFAULT_PITCH = 10.0;
        public const int CASSETTE_MAX_SLOT_NUM = 20;

        public enum ECassetteDetectedSensor
        {
            SENSOR1 = 0,
            SENSOR2,
            SENSOR3,
            SENSOR4,
            MAX,
        }

        public enum ECassetteSlotStatus
        {
            NONE = -1,
            EMPTY   = 0,    // empty slot
            PRE_PROCESS,    // before start work
            AFTER_PROCESS,  // after complete work
            MAX,
        }

        public enum ECassetteWaferSize
        {
            NONE = -1,
            INCH_8 = 0,
            INCH_12,
            MAX,
        }


        //===============================================================================

        public class CMeElevatorRefComp
        {
            public IIO IO;        
            // MultiAxes
            public MMultiAxes_YMC AxElevator;
        }

        public class CMeElevatorData
        {
            // Cassette Info 
            public CWaferFrameData CassetteData = new CWaferFrameData();

            public int CurrentSlotNum = 0;

            // Detect Object Sensor Address
            public int InWaferDetected   = IO_ADDR_NOT_DEFINED;
            public int[] InCassetteDetected = new int[(int)ECassetteDetectedSensor.MAX];

            // Push-Pull 안전 Sensor
            public int InPushPullDetected = IO_ADDR_NOT_DEFINED;

            // Physical check zone sensor. 원점복귀 여부와 상관없이 축의 물리적인 위치를 체크 및
            // 안전위치 이동 check
            public CMAxisZoneCheck ElevatorZone;
            public CPos_XYTZ ElevatorSafetyPos = new CPos_XYTZ();

            public CMeElevatorData()
            {
                ArrayExtensions.Init(InCassetteDetected, IO_ADDR_NOT_DEFINED);

                CassetteData.SlotNumber = CASSETTE_MAX_SLOT_NUM;
                CassetteData.FramePitch = CASSETTE_DEFAULT_PITCH;
                ArrayExtensions.Init(CassetteData.SlotStatus, (int)ECassetteSlotStatus.NONE);

                ElevatorZone = new CMAxisZoneCheck((int)EElevatorXAxZone.MAX, (int)EElevatorYAxZone.MAX,
                    (int)EElevatorTAxZone.MAX, (int)EElevatorZAxZone.MAX);
            }
        }
    }
    
    public class MMeElevator : MMechanicalLayer
    {
        private CMeElevatorRefComp m_RefComp;
        private CMeElevatorData m_Data;

        // MovingObject
        public CMovingObject AxElevatorInfo { get; private set; } = new CMovingObject((int)EElevatorPos.MAX);
    
        public MMeElevator(CObjectInfo objInfo, CMeElevatorRefComp refComp, CMeElevatorData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CMeElevatorData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CMeElevatorData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetPosition_Elevator(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            AxElevatorInfo.SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
            
            return SUCCESS;
        }        
        
        public int GetElevatorCurPos(out CPos_XYTZ pos)
        {
            int iResult = m_RefComp.AxElevator.GetCurPos(out pos);
            return iResult;
        }

        public void SetCassetteSlotStatus(int index, ECassetteSlotStatus status)
        {
            m_Data.CassetteData.SlotStatus[index] = (int)status;
        }

        public void GetCassetteSlotStatus(int index, out int status)
        {
            status = m_Data.CassetteData.SlotStatus[index];
        }
        #endregion

        /// <summary>
        /// sPos으로 이동하고, PosInfo를 iPos으로 셋팅한다. Backlash는 일단 차후로.
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="bMoveFlag"></param>
        /// <param name="bUseBacklash"></param>
        /// <returns></returns>
        public int MoveToPos(CPos_XYTZ sPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;
            // safety check
            iResult = CheckSafety_forMoving();
            if (iResult != SUCCESS) return iResult;

            // assume move Z axis if bMoveFlag is null
            if(bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
            }

            // trans to array
            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // backlash
            if(bUseBacklash)
            {
                // 나중에 작업
            }
            
            // 1. move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                // set priority
                if(bUsePriority == true && movePriority != null)
                {
                    m_RefComp.AxElevator.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxElevator.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Elevator x y t axis", ELogType.Debug, ELogWType.D_Error);
                    return iResult;
                }
            }

            // 2. move Z Axis
            if (bMoveFlag[DEF_Z] == true)
            {
                bool[] bTempFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                iResult = m_RefComp.AxElevator.Move(DEF_ALL_COORDINATE, bTempFlag, dTargetPos);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Elevator z axis", ELogType.Debug, ELogWType.D_Error);
                    return iResult;
                }
            }

            string str = $"success : move Elevator to pos:{sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        /// <summary>
        /// iPos 좌표로 선택된 축들을 이동시킨다.
        /// </summary>
        /// <param name="iPos">목표 위치</param>
        /// <param name="SlotNum">목표 Slow 위치</param>
        /// <param name="bUpdatedPosInfo">목표위치값을 update 할지의 여부</param>
        /// <param name="bMoveFlag">이동시킬 축 선택 </param>
        /// <param name="dMoveOffset">임시 옵셋값 </param>
        /// <param name="bUseBacklash"></param>
        /// <param name="bUsePriority">우선순위 이동시킬지 여부 </param>
        /// <param name="movePriority">우선순위 </param>
        /// <returns></returns>
        public int MoveToPos(int iPos, int SlotNum =0, bool bUpdatedPosInfo = true, 
            bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;            

            // Load Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            if (iPos == (int)EElevatorPos.LOAD)
            {
                AxElevatorInfo.InitAlignOffset();
            }
            // Slot Position으로 가는 것이면 Slot번호와 Pitch를 곱해서 Offset을 적용한다.
            if (iPos == (int)EElevatorPos.SLOT)
            {
                if (dMoveOffset == null) dMoveOffset = new double[DEF_XYTZ];
                dMoveOffset[DEF_X] = 0.0;
                dMoveOffset[DEF_Y] = 0.0;
                dMoveOffset[DEF_T] = 0.0;
                dMoveOffset[DEF_Z] = (double)SlotNum * m_Data.CassetteData.FramePitch;

            }
            // 이동할 위치의 값을 읽어옴.
            CPos_XYTZ sTargetPos = AxElevatorInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            iResult = MoveToPos(sTargetPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;

            if (bUpdatedPosInfo == true)
            {
                AxElevatorInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }
        
        /// <summary>
        /// Elevator를 LOAD, UNLOAD등의 목표위치로 이동시킬때에 좀더 편하게 이동시킬수 있도록 간편화한 함수
        /// Z축만 움직일 경우엔 Position Info를 업데이트 하지 않는다. 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveAllAxis"></param>
        /// <param name="bMoveXYT"></param>
        /// <param name="bMoveZ"></param>
        /// <returns></returns>
        public int MoveToPos(int iPos, int SlotNum=0, bool bMoveAllAxis=false, bool bMoveXYT=false, bool bMoveZ=true)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, true };
                return MoveToPos(iPos, SlotNum, true, bMoveFlag);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveToPos(iPos, SlotNum, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveToPos(iPos, SlotNum, false, bMoveFlag);
            }

            return SUCCESS;
        }

        public int MoveToBottomPos(bool bMoveAllAxis = false, bool bMoveXYT = false, bool bMoveZ = true)
        {
            int nPos = (int)EElevatorPos.BOTTOM;
            int Slot = 0;
            
            return MoveToPos(nPos, Slot, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToLoadPos(bool bMoveAllAxis = false, bool bMoveXYT = false, bool bMoveZ = true)
        {
            int nPos = (int)EElevatorPos.LOAD;
            int Slot = 0;
            return MoveToPos(nPos, Slot, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToSlotPos(int Slot=0, bool bMoveAllAxis = false, bool bMoveXYT = false, bool bMoveZ = true)
        {
            int nPos = (int)EElevatorPos.SLOT;
            return MoveToPos(nPos, Slot, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToTopPos(bool bMoveAllAxis = false, bool bMoveXYT = false, bool bMoveZ = true)
        {
            int nPos = (int)EElevatorPos.TOP;
            int Slot = 0;
            return MoveToPos(nPos, Slot,bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToSafetyPos(bool bMoveAllAxis = false, bool bMoveXYT = false, bool bMoveZ = true)
        {
            int nPos = (int)EElevatorPos.SAFETY;
            int Slot = 0;
            return MoveToPos(nPos, Slot, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        /// <summary>
        /// 다음 차례의 작업 완료된 wafer가 있는 slot으로 이동한다.
        /// </summary>
        /// <returns></returns>
        public int MoveToNextAfterProcessSlot(out int index, int nAfterIndex = -1)
        {
            bool bMoveAllAxis = false;
            bool bMoveXYT = false;
            bool bMoveZ = true;

            int iResult = 0;
            int nElevatorPos = (int)EElevatorPos.SLOT;
            index = 0;
            int nCurSlotNum = 0;

            // Cassette의 Wafer Data를 아래부터 읽어 필요한 Slot 위치를 찾는다.
            for (int i = 0; i < m_Data.CassetteData.SlotNumber; i++)
            {
                if (i <= nAfterIndex) continue;
                if (m_Data.CassetteData.SlotStatus[i] == (int)ECassetteSlotStatus.AFTER_PROCESS)
                {
                    index = i;
                    break;
                }
            }

            // 해당 Slot이 없을 경우 에러를 리턴함.
            if (index == (int)ECassetteSlotStatus.NONE)
                return GenerateErrorCode(ERR_ELEVATOR_NOT_EXIST_AFTER_PROCESS_WAFER);

            // 해당 위치로 이동함.
            iResult = MoveToPos(nElevatorPos, index, bMoveAllAxis, bMoveXYT, bMoveZ);
            if (iResult != SUCCESS) GenerateErrorCode(ERR_ELEVATOR_MOVE_FAIL);

            Sleep(500);

            // 이동 완료후 wafer 유무 확인
            bool bStatus;
            iResult = m_RefComp.IO.IsOn(m_Data.InWaferDetected, out bStatus);
            if (iResult != SUCCESS) return iResult;

            if (!bStatus) return GenerateErrorCode(ERR_ELEVATOR_OBJECT_NOT_DETECTED);

            return SUCCESS;
        }

        /// <summary>
        /// 다음 차례의 비어있는 slot으로 이동한다.
        /// </summary>
        /// <returns></returns>
        public int MoveToNextEmptySlot(out int index)
        {
            bool bMoveAllAxis = false;
            bool bMoveXYT = false;
            bool bMoveZ = true;

            int iResult = 0;
            int nElevatorPos = (int)EElevatorPos.SLOT;
            index = 0;
            int nCurSlotNum = 0;

            // Cassette의 Wafer Data를 아래부터 읽어 필요한 Slot 위치를 찾는다.
            for (int i = 0; i < m_Data.CassetteData.SlotNumber; i++)
            {
                if (m_Data.CassetteData.SlotStatus[i] == (int)ECassetteSlotStatus.EMPTY)
                {
                    index = i;
                    break;
                }
            }

            // 해당 Slot이 없을 경우 에러를 리턴함.
            if (index == (int)ECassetteSlotStatus.NONE)
                return GenerateErrorCode(ERR_ELEVATOR_NOT_EXIST_EMPTY_SLOT);

            // 해당 위치로 이동함.
            iResult = MoveToPos(nElevatorPos, index, bMoveAllAxis, bMoveXYT, bMoveZ);
            if (iResult != SUCCESS) GenerateErrorCode(ERR_ELEVATOR_MOVE_FAIL);

            Sleep(500);

            // 이동 완료후 wafer 유무 확인
            bool bStatus;
            iResult = m_RefComp.IO.IsOn(m_Data.InWaferDetected, out bStatus);
            if (iResult != SUCCESS) return iResult;

            if (bStatus) return GenerateErrorCode(ERR_ELEVATOR_OBJECT_DETECTED);

            return SUCCESS;
        }

        /// <summary>
        /// 다음 차례의 작업 대기중인 wafer가 있는 slot으로 이동한다.
        /// </summary>
        /// <returns></returns>
        public int MoveToNextPreProcessSlot(out int index)
        {
            bool bMoveAllAxis = false;
            bool bMoveXYT = false;
            bool bMoveZ = true;

            int iResult = 0;
            int nElevatorPos = (int)EElevatorPos.SLOT;
            index = 0;
            int nCurSlotNum = 0;

            // Cassette의 Wafer Data를 아래부터 읽어 필요한 Slot 위치를 찾는다.
            for (int i = 0; i < m_Data.CassetteData.SlotNumber; i++)
            {
                if (m_Data.CassetteData.SlotStatus[i] == (int)ECassetteSlotStatus.PRE_PROCESS)
                {
                    index = i;
                    break;
                }
            }

            // 해당 Slot이 없을 경우 에러를 리턴함.
            if (index == (int)ECassetteSlotStatus.NONE)
                return GenerateErrorCode(ERR_ELEVATOR_NOT_EXIST_PRE_PROCESS_WAFER);

            // 해당 위치로 이동함.
            iResult = MoveToPos(nElevatorPos, index, bMoveAllAxis, bMoveXYT, bMoveZ);
            if (iResult != SUCCESS) GenerateErrorCode(ERR_ELEVATOR_MOVE_FAIL);

            Sleep(500);

            // 이동 완료후 wafer 유무 확인
            bool bStatus;
            iResult = m_RefComp.IO.IsOn(m_Data.InWaferDetected, out bStatus);
            if (iResult != SUCCESS) return iResult;

#if SIMULATION_TEST
            bStatus = true;
#endif
            if (!bStatus) return GenerateErrorCode(ERR_ELEVATOR_OBJECT_NOT_DETECTED);

            return SUCCESS;
        }

        /// <summary>
        /// cassette에 해당 slot count를 읽는다.
        /// </summary>
        /// <returns></returns>
        public int GetEmptySlotCount()
        {
            int sum = 0;
            for (int i = 0; i < m_Data.CassetteData.SlotNumber; i++)
            {
                if (m_Data.CassetteData.SlotStatus[i] == (int)ECassetteSlotStatus.EMPTY)
                    sum++;
            }
            return sum;
        }

        /// <summary>
        /// cassette에 해당 slot count를 읽는다.
        /// </summary>
        /// <returns></returns>
        public int GetPreProcessWaferCount()
        {
            int sum = 0;
            for (int i = 0; i < m_Data.CassetteData.SlotNumber; i++)
            {
                if (m_Data.CassetteData.SlotStatus[i] == (int)ECassetteSlotStatus.PRE_PROCESS)
                    sum++;
            }
            return sum;
        }

        /// <summary>
        /// cassette에 해당 slot count를 읽는다.
        /// </summary>
        /// <returns></returns>
        public int GetAfterProcessWaferCount()
        {
            int sum = 0;
            for (int i = 0; i < m_Data.CassetteData.SlotNumber; i++)
            {
                if (m_Data.CassetteData.SlotStatus[i] == (int)ECassetteSlotStatus.AFTER_PROCESS)
                    sum++;
            }
            return sum;
        }

        public int SearchElevatorCassetteWafer()
        {
            bool bMoveAllAxis = false;
            bool bMoveXYT = false;
            bool bMoveZ = true;
            bool bStatus = false;

            int iResult = -1;
            int nElevatorPos = (int)EElevatorPos.SLOT;

            // Cassette 유무를 확인한다.


            // Slot 위치를 확인하며 Wafer의 유무를 확인한다.            
            for (int i = 0; i < m_Data.CassetteData.SlotNumber; i++)
            {
#if !SIMULATION_TEST    // Test시에 시간이 너무 오래 걸려서
                // 해당 위치로 이동함.
                iResult = MoveToPos(nElevatorPos, i, bMoveAllAxis, bMoveXYT, bMoveZ);
                if (iResult != SUCCESS) GenerateErrorCode(ERR_ELEVATOR_MOVE_FAIL);
#endif

                // Wafer Frame 감지 센서를 확인하여 Empty 여부를 확인한다.               
                iResult = m_RefComp.IO.IsOn(m_Data.InWaferDetected, out bStatus);
                if (iResult != SUCCESS) return iResult;

#if SIMULATION_TEST
                if (i % 3 == 0) bStatus = true;
#endif
                if (bStatus)
                {
                    m_Data.CassetteData.SlotStatus[i] = (int)ECassetteSlotStatus.PRE_PROCESS;
                }
                else
                {
                    m_Data.CassetteData.SlotStatus[i] = (int)ECassetteSlotStatus.EMPTY;
                }

#if !SIMULATION_TEST
                Sleep(SimulationSleepTime);
#endif

            }

            return SUCCESS;
        }
        /// <summary>
        /// 현재 위치와 목표위치의 위치차이 Tolerance check
        /// </summary>
        /// <param name="sPos"> 목표 위치값</param>
        /// <param name="bResult"></param>
        /// <param name="bCheck_XAxis"></param>
        /// <param name="bCheck_YAxis"></param>
        /// <param name="bCheck_TAxis"></param>
        /// <param name="bSkipError">위치가 틀릴경우 에러 보고할지 여부</param>
        /// <returns></returns>
        public int CompareElevatorPos(CPos_XYTZ sPos, out bool bResult, 
                        bool bCheck_XAxis, bool bCheck_YAxis, bool bCheck_TAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            // trans to array
            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = m_RefComp.AxElevator.ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
            if (iResult != SUCCESS) return iResult;

            // skip axis
            if (bCheck_XAxis == false) bJudge[DEF_X] = true;
            if (bCheck_YAxis == false) bJudge[DEF_Y] = true;
            if (bCheck_TAxis == false) bJudge[DEF_T] = true;            

            // error check
            bResult = true;
            foreach(bool bTemp in bJudge)
            {
                if (bTemp == false) bResult = false;
            }

            // skip error?
            if(bSkipError == false && bResult == false)
            {
                string str = $"Elevator의 현재 위치와 일치하는 Position Info를 찾을수 없습니다. Current Pos : {sPos.ToString()}";
                WriteLog(str, ELogType.Debug, ELogWType.D_Error);

                return GenerateErrorCode(ERR_ELEVATOR_FAIL_TO_GET_CURRENT_POS_INFO);
            }

            return SUCCESS;
        }

        public int CompareElevatorPos(int iPos, out bool bResult, out int Slot, bool bSkipError = true)
        {
            int iResult = SUCCESS;
            bResult = false;

            bool bCheck_XAxis = false;
            bool bCheck_YAxis = false;
            bool bCheck_TAxis = false;

            Slot = -1;

            CPos_XYTZ targetPos = AxElevatorInfo.GetTargetPos(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = CompareElevatorPos(targetPos, out bResult, bCheck_XAxis, bCheck_YAxis, bCheck_TAxis, bSkipError);
            if (iResult != SUCCESS) return iResult;

            // Slot Position Cals (Result가 true일 경우)
            if(iPos== (int)EElevatorPos.SLOT && bResult)
            {
                double dReferencePos = 0.0;
                CPos_XYTZ LoadPos = AxElevatorInfo.GetTargetPos((int)EPosition.LOAD);
                dReferencePos = targetPos.dZ - LoadPos.dZ;
                m_Data.CurrentSlotNum = (int)(dReferencePos / m_Data.CassetteData.FramePitch);

                Slot = m_Data.CurrentSlotNum;
            }
            else
            {
                Slot = -1;
            }

            return SUCCESS;
        }

        public int GetElevatorPosInfo(out int posInfo, out int Slot, bool bUpdatePos = true, bool bSkipError = false)
        {
            posInfo = (int)EElevatorPos.NONE;
            Slot = -1;

            bool bStatus;
            int iResult = IsElevatorOrignReturn(out bStatus);
            if (iResult != SUCCESS) return iResult;

            // 실시간으로 자기 위치를 체크
            if(bUpdatePos)
            {
                for (int i = 0; i < (int)EElevatorPos.MAX; i++)
                {
                    CompareElevatorPos(i, out bStatus, out Slot, bSkipError);
                    if (bStatus)
                    {
                        AxElevatorInfo.PosInfo = i;
                        break;
                    }
                }
            }

            posInfo = AxElevatorInfo.PosInfo;
            return SUCCESS;
        }

        public void SetElevatorPosInfo(int posInfo)
        {
            AxElevatorInfo.PosInfo = posInfo;
        }

        public int IsElevatorOrignReturn(out bool bStatus)
        {
            bool[] bAxisStatus;
            int iResult = m_RefComp.AxElevator.IsOriginReturned(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);

            return iResult;
        }

        ////////////////////////////////////////////////////////////////////////

        public int GetElevatorAxZone(int axis, out int curZone)
        {
            bool bStatus;
            curZone = (int)EElevatorXAxZone.NONE;
            for(int i = 0; i < (int)EElevatorXAxZone.MAX; i++)
            {
                if (m_Data.ElevatorZone.Axis[axis].ZoneAddr[i] == -1) continue; // if io is not allocated, continue;
                int iResult = m_RefComp.IO.IsOn(m_Data.ElevatorZone.Axis[axis].ZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int IsElevatorAxisInSafetyZone(int axis, out bool bStatus)
        {
            bStatus = false;
            int curZone;
            int iResult = GetElevatorAxZone(axis, out curZone);
            if (iResult != SUCCESS) return iResult;

            switch(axis)
            {
                case DEF_X:
                    break;

                case DEF_Y:
                    if (curZone == (int)EElevatorYAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;

                case DEF_T:
                    if (curZone == (int)EElevatorTAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;

                case DEF_Z:
                    if (curZone == (int)EElevatorZAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;
            }
            return SUCCESS;
        }

        public int CheckSafety_forMoving()
        {
            // check origin
            bool bStatus;
            int iResult = IsElevatorOrignReturn(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return GenerateErrorCode(ERR_ELEVATOR_NOT_ORIGIN_RETURNED);

            // Cassette 감지 센서 확인 (정위치 확인 or Cassette없음 Check)
            iResult = IsCassetteExist(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// PushPull이 Elevator에 push/pop wafer하기전에 안전 체크
        /// </summary>
        /// <param name="bStatus"></param>
        /// <returns></returns>
        public int CheckSafetyForPushPull(bool bNeedWaferNotExist)
        {
            int iResult = SUCCESS;
            bool bStatus = false;
            iResult = IsElevatorOrignReturn(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false)
            {
                return GenerateErrorCode(ERR_ELEVATOR_NOT_ORIGIN_RETURNED);
            }

            // Cassette 감지 센서 확인, Cassette가 없으면 PushPull이 들어올 필요가 없으므로.
            iResult = IsCassetteExist(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return GenerateErrorCode(ERR_ELEVATOR_CASSETTE_NOT_READY);

            // PushPull이 웨이퍼를 가지고 들어오기전에 체크할 경우엔
            if(bNeedWaferNotExist)
            {
                iResult = IsWaferDetected(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus) return GenerateErrorCode(ERR_ELEVATOR_OBJECT_DETECTED);
            }

            return SUCCESS;
        }


        public int IsWaferDetected(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InWaferDetected, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// Cassette가 있는지를 체크. 
        /// 센서 상태가 통일되지 않았을 경우엔 error를 return
        /// </summary>
        /// <param name="bStatus"></param>
        /// <returns></returns>
        public int IsCassetteExist(out bool bStatus)
        {
            bStatus = false;
            int iResult = SUCCESS;

            // Wafer Frame 감지 센서 확인
            bool[] bCheckIO = new bool[(int)ECassetteDetectedSensor.MAX];

            for (int i = 0; i < (int)ECassetteDetectedSensor.MAX; i++)
            {
                // 4개 센서를 확인한다.
                iResult = m_RefComp.IO.IsOn(m_Data.InCassetteDetected[i], out bCheckIO[i]);
                if (iResult != SUCCESS) return iResult;
            }

            // 4개의 센서 상태가 모두 일치하는지 체크
            bool bTSum = bCheckIO[0];
            bool bFSum = bCheckIO[0];
            foreach (bool bDetected in bCheckIO)
            {
                bTSum &= bDetected;
                bFSum |= bDetected;
            }

            if ((bCheckIO[0] == true && bTSum == false)
                || (bCheckIO[0] == false && bFSum == true))
                return GenerateErrorCode(ERR_ELEVATOR_CASSETTE_DETECTED_ABNORMAL);

            if(bCheckIO[0] == true && bTSum == true) bStatus = true;
            return SUCCESS;
        }

        /// <summary>
        /// Push-Pull Unit 간섭위치 확인 (센서로 확인함)
        /// 필요시에 말굽 센서를 추가 설치함.
        /// </summary>
        /// <param name="bStatus"></param>
        /// <returns></returns>
        public int IsPushPullDetected(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InPushPullDetected, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

    }
}
