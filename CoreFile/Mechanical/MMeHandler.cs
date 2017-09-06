using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_MeHandler;
using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_IO;
using static Core.Layers.DEF_Vacuum;

namespace Core.Layers
{
    public class DEF_MeHandler
    {
        public const int ERR_HANDLER_NOT_ORIGIN_RETURNED                        = 1;
        public const int ERR_HANDLER_UNABLE_TO_USE_CYL                          = 2;
        public const int ERR_HANDLER_UNABLE_TO_USE_VCC                          = 3;
        public const int ERR_HANDLER_UNABLE_TO_USE_AXIS                         = 4;
        public const int ERR_HANDLER_INVALID_AXIS                               = 5;
        public const int ERR_HANDLER_FAIL_TO_GET_CURRENT_POS_INFO                          = 6;
        public const int ERR_HANDLER_VACUUM_ON_TIME_OUT                         = 7;
        public const int ERR_HANDLER_VACUUM_OFF_TIME_OUT                        = 8;
        public const int ERR_HANDLER_OBJECT_DETECTED_BUT_NOT_ABSORBED           = 9;
        public const int ERR_HANDLER_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED       = 10;
        public const int ERR_HANDLER_NOT_SAFE_FOR_PUSHPULL                      = 11;

        public enum EHandlerType
        {
            NONE = -1,
            AXIS = 0,
            CYL,
        }

        public enum EHandlerVacuum
        {
            SELF,           // 자체 발생 진공
            FACTORY,        // 공장 진공
            OBJECT,         // LCD 패널의 PCB같은 걸 집는 용도
            EXTRA_SELF,     // 
            EXTRA_FACTORY,  //
            EXTRA_OBJECT,   //
            MAX,
        }

        public enum EHandlerPos
        {
            NONE = -1,
            WAIT,
            PUSHPULL,   // Load, Unload 대신에 명확하게 하기위해 PushPull, Stage Side 로 정의
            STAGE,
//             LOAD = 0,
//             UNLOAD,
            //TURN,
            // Z Safety Up Position은 System Data 에서 관리하도록 한다.
            //LOAD_Z_UP,      // Loading Pos(x,y,t) + Z Up
            //UNLOAD_Z_UP,    // Unloading Pos(x,y,t) + Z Up
            MAX,
        }

        public enum EHandlerXAxZone
        {
            NONE = -1,
            WAIT,
            PUSHPULL,
            STAGE,
            MAX,
        }

        public enum EHandlerYAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EHandlerTAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EHandlerZAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public class CMeHandlerRefComp
        {
            public IIO IO;

            // Cylinder
            // 4축에 대응하는 방식 Upstr/Downstr, Front/Back, TurnCW/TurnCCW, Up/Down
            public ICylinder[] MainCyl = new ICylinder[DEF_MAX_COORDINATE];
            public ICylinder[] SubCyl = new ICylinder[DEF_MAX_COORDINATE];
            public ICylinder[] GuideCyl = new ICylinder[DEF_MAX_COORDINATE];

            // Vacuum
            public IVacuum[] Vacuum = new IVacuum[(int)EHandlerVacuum.MAX];

            // MultiAxes
            public MMultiAxes_YMC AxHandler;

            // Vision Object
            public IVision Vision;
        }

        public class CMeHandlerData
        {
            // Handler Type
            public EHandlerType[] HandlerType = new EHandlerType[DEF_MAX_COORDINATE];

            // Camera Number
            public int CamNo;

            // Detect Object Sensor Address
            public int InDetectObject   = IO_ADDR_NOT_DEFINED;

            // IO Address for manual control cylinder
            public int InUpCylinder     = IO_ADDR_NOT_DEFINED;
            public int InDownCylinder   = IO_ADDR_NOT_DEFINED;

            public int OutUpCylinder    = IO_ADDR_NOT_DEFINED;
            public int OutDownCylinder  = IO_ADDR_NOT_DEFINED;


            // IO Address for manual check axis zone
            public CMAxisZoneCheck HandlerZoneCheck = new CMAxisZoneCheck((int)EHandlerXAxZone.MAX, (int)EHandlerYAxZone.MAX,
            (int)EHandlerTAxZone.MAX, (int)EHandlerZAxZone.MAX);

            // Vacuum
            public bool[] UseVccFlag = new bool[(int)EHandlerVacuum.MAX];

            // Cylinder
            public bool[] UseMainCylFlag = new bool[DEF_MAX_COORDINATE];
            public bool[] UseSubCylFlag = new bool[DEF_MAX_COORDINATE];
            public bool[] UseGuideCylFlag = new bool[DEF_MAX_COORDINATE];

            // Handler Safety Position
            public CPos_XYTZ HandlerSafetyPos;

            public CMeHandlerData()
            {
                ArrayExtensions.Init(HandlerType, EHandlerType.NONE);
                ArrayExtensions.Init(UseVccFlag, false);
            }
        }
    }
    
    public class MMeHandler : MMechanicalLayer
    {
        private CMeHandlerRefComp m_RefComp;
        private CMeHandlerData m_Data;

        // MovingObject
        public CMovingObject AxHandlerInfo { get; private set; } = new CMovingObject((int)EHandlerPos.MAX);

        public MMeHandler(CObjectInfo objInfo, CMeHandlerRefComp refComp, CMeHandlerData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CMeHandlerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CMeHandlerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetPosition_Handler(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            int pIndex = (int)EHandlerPos.WAIT;
            m_Data.HandlerSafetyPos = Pos_Fixed.Pos[pIndex] + Pos_Model.Pos[pIndex] + Pos_Offset.Pos[pIndex];
            AxHandlerInfo.SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
            return SUCCESS;
        }

        #endregion

        #region Cylinder, Vacuum, Detect Object
        public int Absorb(bool bSkipSensor = false)
        {
            bool bStatus;
            int iResult = SUCCESS;
            bool[] bWaitFlag = new bool[(int)EHandlerVacuum.MAX];
            CVacuumTime[] sData = new CVacuumTime[(int)EHandlerVacuum.MAX];
            bool bNeedWait = false;

            for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
            {
                if (m_Data.UseVccFlag[i] == false) continue;

                m_RefComp.Vacuum[i].GetVacuumTime(out sData[i]);
                iResult = m_RefComp.Vacuum[i].IsOn(out bStatus);
                if (iResult != SUCCESS) return iResult;

                // 흡착되지 않은 상태라면 흡착시킴  
                if (bStatus == false)
                {
                    iResult = m_RefComp.Vacuum[i].On(true);
                    if (iResult != SUCCESS) return iResult;

                    bWaitFlag[i] = true;
                    bNeedWait = true;
                }

                Sleep(10);
            }

            if (bSkipSensor == true) return SUCCESS;

            m_waitTimer.StartTimer();
            while (bNeedWait)
            {
                bNeedWait = false;

                for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
                {
                    if (bWaitFlag[i] == false) continue;

                    iResult = m_RefComp.Vacuum[i].IsOn(out bStatus);
                    if (iResult != SUCCESS) return iResult;

                    if (bStatus == true) // if on
                    {
                        bWaitFlag[i] = false;
                        //Sleep(sData[i].OnSettlingTime * 1000);
                    }
                    else // if off
                    {
                        bNeedWait = true;
                        if (m_waitTimer.MoreThan(sData[i].TurningTime, ETimeType.SECOND))
                        {
                            return GenerateErrorCode(ERR_HANDLER_VACUUM_ON_TIME_OUT);
                        }
                    }

                }
            }

            return SUCCESS;
        }

        public int Release(bool bSkipSensor)
        {
            bool bStatus;
            int iResult = SUCCESS;
            bool[] bWaitFlag = new bool[(int)EHandlerVacuum.MAX];
            CVacuumTime[] sData = new CVacuumTime[(int)EHandlerVacuum.MAX];
            bool bNeedWait = false;

            for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
            {
                if (m_Data.UseVccFlag[i] == false) continue;

                m_RefComp.Vacuum[i].GetVacuumTime(out sData[i]);
                iResult = m_RefComp.Vacuum[i].IsOff(out bStatus);
                if (iResult != SUCCESS) return iResult;

                if (bStatus == false)
                {
                    iResult = m_RefComp.Vacuum[i].Off(true);
                    if (iResult != SUCCESS) return iResult;

                    bWaitFlag[i] = true;
                    bNeedWait = true;
                }

                Sleep(10);
            }

            if (bSkipSensor == true) return SUCCESS;

            m_waitTimer.StartTimer();
            while (bNeedWait)
            {
                bNeedWait = false;

                for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
                {
                    if (bWaitFlag[i] == false) continue;

                    iResult = m_RefComp.Vacuum[i].IsOff(out bStatus);
                    if (iResult != SUCCESS) return iResult;

                    if (bStatus == true) // if on
                    {
                        bWaitFlag[i] = false;
                        //Sleep(sData[i].OffSettlingTime * 1000);
                    }
                    else // if off
                    {
                        bNeedWait = true;
                        if (m_waitTimer.MoreThan(sData[i].TurningTime, ETimeType.SECOND))
                        {
                            return GenerateErrorCode(ERR_HANDLER_VACUUM_OFF_TIME_OUT);
                        }
                    }

                }
            }

            return SUCCESS;
        }

        public int IsAbsorbed(out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;
            bool bTemp;

            for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
            {
                if (m_Data.UseVccFlag[i] == false) continue;

                iResult = m_RefComp.Vacuum[i].IsOn(out bTemp);
                if (iResult != SUCCESS) return iResult;

                if (bTemp == false) return SUCCESS;
            }

            bStatus = true;
            return SUCCESS;
        }

        public int IsReleased(out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;
            bool bTemp;

            for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
            {
                if (m_Data.UseVccFlag[i] == false) continue;

                iResult = m_RefComp.Vacuum[i].IsOff(out bTemp);
                if (iResult != SUCCESS) return iResult;

                if (bTemp == false) return SUCCESS;
            }

            bStatus = true;
            return SUCCESS;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InDetectObject, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_Z
        public int IsCylUp(out bool bStatus, int index = DEF_Z)
        {
            int iResult = SUCCESS;
            bStatus = false;

            if (m_Data.UseMainCylFlag[index] == true)
            {
                if(m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].IsUp(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (m_Data.UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.SubCyl[index].IsUp(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int IsCylDown(out bool bStatus, int index = DEF_Z)
        {
            int iResult = SUCCESS;
            bStatus = false;

            if (m_Data.UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].IsDown(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (m_Data.UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.SubCyl[index].IsDown(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int CylUp(bool bSkipSensor = false, int index = DEF_Z)
        {
            // check for safety
            int iResult = CheckForHandlerCylMove();
            if (iResult != SUCCESS) return iResult;

            if (m_Data.UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].Up(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (m_Data.UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.SubCyl[index].Up(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int CylDown(bool bSkipSensor = false, int index = DEF_Z)
        {
            // check for safety
            int iResult = CheckForHandlerCylMove();
            if (iResult != SUCCESS) return iResult;

            if (m_Data.UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].Down(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (m_Data.UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.SubCyl[index].Down(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            
            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_X
        public int IsCylUpstr(out bool bStatus)
        {
            bStatus = false;
            return IsCylUp(out bStatus, DEF_X);
        }

        public int IsCylDownstr(out bool bStatus)
        {
            bStatus = false;
            return IsCylDown(out bStatus, DEF_X);
        }

        public int CylUpstr(bool bSkipSensor = false)
        {
            return CylUp(bSkipSensor, DEF_X);
        }

        public int CylDownstr(bool bSkipSensor = false)
        {
            return CylDown(bSkipSensor, DEF_X);
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_Y
        public int IsCylFront(out bool bStatus)
        {
            bStatus = false;
            return IsCylUp(out bStatus, DEF_Y);
        }

        public int IsCylBack(out bool bStatus)
        {
            bStatus = false;
            return IsCylDown(out bStatus, DEF_Y);
        }

        public int CylFront(bool bSkipSensor = false)
        {
            return CylUp(bSkipSensor, DEF_Y);
        }

        public int CylBack(bool bSkipSensor = false)
        {
            return CylDown(bSkipSensor, DEF_Y);
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_T
        public int IsCylCW(out bool bStatus)
        {
            bStatus = false;
            return IsCylUp(out bStatus, DEF_T);
        }

        public int IsCylCCW(out bool bStatus)
        {
            bStatus = false;
            return IsCylDown(out bStatus, DEF_T);
        }

        public int CylCW(bool bSkipSensor = false)
        {
            return CylUp(bSkipSensor, DEF_T);
        }

        public int CylCCW(bool bSkipSensor = false)
        {
            return CylDown(bSkipSensor, DEF_T);
        }
        #endregion

        public int GetHandlerCurPos(out CPos_XYTZ pos)
        {
            int iResult = m_RefComp.AxHandler.GetCurPos(out pos);
            return iResult;
        }

        /// <summary>
        /// sPos으로 이동하고, PosInfo를 iPos으로 셋팅한다. Backlash는 일단 차후로.
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="bMoveFlag"></param>
        /// <param name="bUseBacklash"></param>
        /// <returns></returns>
        private int MoveHandlerToPos(CPos_XYTZ sPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // safety check
            iResult = CheckSafetyForHandlerAxisMove();
            if (iResult != SUCCESS) return iResult;

            // assume move all axis if bMoveFlag is null
            if(bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, true };
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
                // 1.1 xyt 이동할 경우엔, 무조건 z축을 안전위치로 먼저 이동
                if (m_RefComp.AxHandler.HasAxis(DEF_Z) == true && m_Data.HandlerZoneCheck.UseSafetyMove[DEF_Z] == true)
                {
                    bool bStatus;
                    iResult = IsHandlerAxisInSafetyZone(DEF_Z, out bStatus);
                    if (iResult != SUCCESS) return iResult;
                    if (bStatus == false)
                    {
                        iResult = MoveHandlerZToSafetyPos();
                        if (iResult != SUCCESS) return iResult;
                    }
                }

                // set priority
                if (bUsePriority == true && movePriority != null)
                {
                    m_RefComp.AxHandler.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxHandler.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Handler x y t axis", ELogType.Debug, ELogWType.D_Error);
                    return iResult;
                }
            }

            // 2. move Z Axis
            if (m_RefComp.AxHandler.HasAxis(DEF_Z) == true)
            {
                if (bMoveFlag[DEF_Z] == true)
                {
                    bool[] bTempFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                    iResult = m_RefComp.AxHandler.Move(DEF_ALL_COORDINATE, bTempFlag, dTargetPos);
                    if (iResult != SUCCESS)
                    {
                        WriteLog("fail : move Handler z axis", ELogType.Debug, ELogWType.D_Error);
                        return iResult;
                    }
                }
            }

            string str = $"success : move Handler to pos:{sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        /// <summary>
        /// iPos 좌표로 선택된 축들을 이동시킨다.
        /// </summary>
        /// <param name="iPos">목표 위치</param>
        /// <param name="bUpdatedPosInfo">목표위치값을 update 할지의 여부</param>
        /// <param name="bMoveFlag">이동시킬 축 선택 </param>
        /// <param name="dMoveOffset">임시 옵셋값 </param>
        /// <param name="bUseBacklash"></param>
        /// <param name="bUsePriority">우선순위 이동시킬지 여부 </param>
        /// <param name="movePriority">우선순위 </param>
        /// <returns></returns>
        public int MoveHandlerToPos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // Load Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            if (iPos == (int)EHandlerPos.PUSHPULL)
            {
                AxHandlerInfo.InitAlignOffset();
            }

            CPos_XYTZ sTargetPos = AxHandlerInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            iResult = MoveHandlerToPos(sTargetPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);

            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxHandlerInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }

        /// <summary>
        /// Handler Z축을 안전 Up 위치로 이동
        /// </summary>
        /// <returns></returns>
        public int MoveHandlerZToSafetyPos()
        {
            int iResult = SUCCESS;
            int axis = DEF_Z;
            string str;

            // 0. safety check
            iResult = CheckSafetyForHandlerAxisMove();
            if (iResult != SUCCESS) return iResult;

            // 0.1 trans to array
            double[] dPos = new double[1] { m_Data.HandlerSafetyPos.GetAt(axis) };

            // 0.2 set use flag
            bool[] bTempFlag = new bool[1] { true };

            // 1. Move
            iResult = m_RefComp.AxHandler.Move(axis, bTempFlag, dPos);
            if (iResult != SUCCESS)
            {
                str = $"fail : move Handler to safety pos [axis={axis}]";
                WriteLog(str, ELogType.Debug, ELogWType.D_Error);
                return iResult;
            }

            //str = $"success : move Handler to safety pos [axis={axis}";
            //WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        /// <summary>
        /// Handler를 LOAD, UNLOAD등의 목표위치로 이동시킬때에 좀더 편하게 이동시킬수 있도록 간편화한 함수
        /// Z축만 움직일 경우엔 Position Info를 업데이트 하지 않는다. 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveAllAxis"></param>
        /// <param name="bMoveXYT"></param>
        /// <param name="bMoveZ"></param>
        /// <returns></returns>
        public int MoveHandlerToPos(int iPos, bool bMoveXYT, bool bMoveZ, double[] dMoveOffset = null)
        {
            // 0. move all axis
            if (bMoveXYT && bMoveZ)
            {
                return MoveHandlerToPos(iPos, dMoveOffset : dMoveOffset);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveHandlerToPos(iPos, true, bMoveFlag, dMoveOffset: dMoveOffset);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveHandlerToPos(iPos, false, bMoveFlag, dMoveOffset: dMoveOffset);
            }

            return SUCCESS;
        }

        public int MoveHandlerToPushPullPos(bool bMoveXYT = false, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EHandlerPos.PUSHPULL;

            return MoveHandlerToPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
        }

        public int MoveHandlerToStagePos(bool bMoveXYT = false, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EHandlerPos.STAGE;

            return MoveHandlerToPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
        }

        public int MoveHandlerToWaitPos(bool bMoveXYT = false, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EHandlerPos.WAIT;

            return MoveHandlerToPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
        }

        /// <summary>
        /// 현재 위치와 목표위치의 위치차이 Tolerance check
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="bResult"></param>
        /// <param name="bCheck_TAxis"></param>
        /// <param name="bCheck_ZAxis"></param>
        /// <param name="bSkipError">위치가 틀릴경우 에러 보고할지 여부</param>
        /// <returns></returns>
        public int CompareHandlerPos(CPos_XYTZ sPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            // trans to array
            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = m_RefComp.AxHandler.ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
            if (iResult != SUCCESS) return iResult;

            // skip axis
            if (bCheck_TAxis == false) bJudge[DEF_T] = true;
            if (bCheck_ZAxis == false) bJudge[DEF_Z] = true;

            // error check
            bResult = true;
            foreach(bool bTemp in bJudge)
            {
                if (bTemp == false)
                {
                    bResult = false;
                    break;
                }
            }

            // skip error?
            if (bSkipError == false && bResult == false)
            {
                string str = $"Handler의 현재 위치와 일치하는 Position Info를 찾을수 없습니다. Current Pos : {sPos.ToString()}";
                WriteLog(str, ELogType.Debug, ELogWType.D_Error);

                return GenerateErrorCode(ERR_HANDLER_FAIL_TO_GET_CURRENT_POS_INFO);
            }

            return SUCCESS;
        }

        public int CompareHandlerPos(int iPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            CPos_XYTZ targetPos = AxHandlerInfo.GetTargetPos(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = CompareHandlerPos(targetPos, out bResult, bCheck_TAxis, bCheck_ZAxis, bSkipError);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetHandlerPosInfo(out int posInfo, bool bUpdatePos = true, bool bCheck_ZAxis = false)
        {
            posInfo = (int)EHandlerPos.NONE;
            bool bStatus;
            int iResult = IsHandlerAxisOrignReturned(out bStatus);
            if (iResult != SUCCESS) return iResult;

            // 실시간으로 자기 위치를 체크
            if(bUpdatePos)
            {
                AxHandlerInfo.PosInfo = posInfo;
                for (int i = 0; i < (int)EHandlerPos.MAX; i++)
                {
                    CompareHandlerPos(i, out bStatus, false, bCheck_ZAxis);
                    if (bStatus)
                    {
                        AxHandlerInfo.PosInfo = i;
                        posInfo = i;
                        break;
                    }
                }
            } else
            {
                posInfo = AxHandlerInfo.PosInfo;
            }
            return SUCCESS;
        }

        public void SetHandlerPosInfo(int posInfo)
        {
            AxHandlerInfo.PosInfo = posInfo;
        }

        public int IsHandlerAxisOrignReturned(out bool bStatus)
        {
            bool[] bAxisStatus;
            int iResult = m_RefComp.AxHandler.IsOriginReturned(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);

            return iResult;
        }

        /// <summary>
        /// Handler 해당 축의 모터 위치를 감지할 수 있는 io sensor가 있을 경우에 sensor를 검사해서
        /// sensor on/off에 기반한 모터가 어느 zone에 있는지를 확인해준다.
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="curZone"></param>
        /// <returns></returns>
        public int GetHandlerAxZone(int axis, out int curZone)
        {
            bool bStatus;
            curZone = (int)EHandlerXAxZone.NONE;
            int length = (int)EHandlerXAxZone.MAX;
            switch(axis)
            {
                case DEF_X:
                    length = (int)EHandlerXAxZone.MAX;
                    break;
                case DEF_Y:
                    length = (int)EHandlerYAxZone.MAX;
                    break;
                case DEF_T:
                    length = (int)EHandlerTAxZone.MAX;
                    break;
                case DEF_Z:
                    length = (int)EHandlerZAxZone.MAX;
                    break;
            }

            for (int i = 0; i < length; i++)
            {
                if (m_Data.HandlerZoneCheck.Axis[axis].ZoneAddr[i] == -1) continue; // if io is not allocated, continue;
                int iResult = m_RefComp.IO.IsOn(m_Data.HandlerZoneCheck.Axis[axis].ZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int CheckHandlerSafetyForPushPull()
        {
            bool bStatus = false;
            int curZone;

            // check x axis
            // handler가 pushpull zone에 위치하지 않으면 바로 return SUCCESS
            int iResult = GetHandlerAxZone(DEF_X, out curZone);
            if (iResult != SUCCESS) return iResult;
            if(curZone != (int)EHandlerXAxZone.PUSHPULL)
            {
                bStatus = true;
                return SUCCESS;
            }

            // check z axis
            iResult = GetHandlerAxZone(DEF_Z, out curZone);
            if (iResult != SUCCESS) return iResult;
            if (curZone == (int)EHandlerZAxZone.SAFETY)
            {
                bStatus = true;
                return SUCCESS;
            }

            return GenerateErrorCode(ERR_HANDLER_NOT_SAFE_FOR_PUSHPULL);
        }

        public int CheckHandlerSafetyForStage(out bool bStatus)
        {
            bStatus = false;
            int curZone;

            // check x axis
            int iResult = GetHandlerAxZone(DEF_X, out curZone);
            if (iResult != SUCCESS) return iResult;
            if (curZone != (int)EHandlerXAxZone.STAGE)
            {
                bStatus = true;
                return SUCCESS;
            }

            // check z axis
            iResult = GetHandlerAxZone(DEF_Z, out curZone);
            if (iResult != SUCCESS) return iResult;
            if (curZone == (int)EHandlerZAxZone.SAFETY)
            {
                bStatus = true;
                return SUCCESS;
            }

            return SUCCESS;
        }

        public int IsHandlerAxisInSafetyZone(int axis, out bool bStatus)
        {
#if SIMULATION_TEST
            //bStatus = true;
            //return SUCCESS;
#endif
            bStatus = false;
            int curZone;
            int iResult = GetHandlerAxZone(axis, out curZone);
            if (iResult != SUCCESS) return iResult;

            switch(axis)
            {
                case DEF_X:
                    break;

                case DEF_Y:
                    if (curZone == (int)EHandlerYAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;

                case DEF_T:
                    if (curZone == (int)EHandlerTAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;

                case DEF_Z:
                    if (curZone == (int)EHandlerZAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;
            }
            return SUCCESS;
        }

        public int CheckSafetyForHandlerAxisMove(bool bCheckVacuum = true)
        {
            bool bStatus;

            // check origin
            int iResult = IsHandlerAxisOrignReturned(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if(bStatus == false)
            {
                return GenerateErrorCode(ERR_HANDLER_NOT_ORIGIN_RETURNED);
            }

            // check object
            if(bCheckVacuum == true)
            {
                iResult = CheckForHandlerCylMove();
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int CheckForHandlerCylMove()
        {
            bool bStatus;

            // check object
            int iResult = IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                IsAbsorbed(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false)
                {
                    return GenerateErrorCode(ERR_HANDLER_OBJECT_DETECTED_BUT_NOT_ABSORBED);
                }
            }
            else
            {
                IsReleased(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false)
                {
                    return GenerateErrorCode(ERR_HANDLER_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED);
                }
            }

            return SUCCESS;
        }

    }
}
