using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_MePushPull;
using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_IO;
using static Core.Layers.DEF_Vacuum;

namespace Core.Layers
{
    public class DEF_MePushPull
    {
        public const int ERR_PUSHPULL_NOT_ORIGIN_RETURNED                       = 1;
        public const int ERR_PUSHPULL_UNABLE_TO_USE_CYL                         = 2;
        public const int ERR_PUSHPULL_UNABLE_TO_USE_VCC                         = 3;
        public const int ERR_PUSHPULL_UNABLE_TO_USE_AXIS                        = 4;
        public const int ERR_PUSHPULL_FAIL_TO_GET_CURRENT_POS_INFO                         = 5;
        public const int ERR_PUSHPULL_OBJECT_DETECTED_BUT_GRIP_NOT_LOCKED       = 6;
        public const int ERR_PUSHPULL_OBJECT_NOT_DETECTED_BUT_GRIP_NOT_RELEASED = 7;

        public enum ECenterIndex
        {
            LEFT,       // Center 1
            RIGHT,      // Center 2
            MAX,
        }

        public enum EPushPullType
        {
            NONE = -1,
            AXIS = 0,
            CYL,
        }

        public enum EPushPullVacuum
        {
            MAX,
        }

        public enum EPushPullPos
        {
            NONE = -1,
            WAIT,
            LOADER,         // with loader
            HANDLER,        // with handler
            SPINNER1,       // with spinner1
            SPINNER2,       // with spinner2

            // front grip으로 wafer를 가져와서 temp_unload 위치에 놓은 후에, reload위치로 이동해서 
            // rear grip으로 다시 wafer를 grip
            // Load -> Temp_Unload, ungrip -> Reload, grip -> Unload2
            TEMP_UNLOAD,    
            RELOAD,         
            MAX,
        }

        public enum ECenterPos
        {
            NONE = -1,
            WAIT,       // release wafer pos
            CENTERING,  // centering wafer pos
            MAX,
        }

        public enum EPushPullXAxZone
        {
            NONE = -1,
            MAX,
        }

        public enum EPushPullYAxZone
        {
            NONE = -1,
            MAX,
        }

        public enum EPushPullTAxZone
        {
            NONE = -1,
            MAX,
        }

        public enum EPushPullZAxZone
        {
            NONE = -1,
            MAX,
        }

        public enum ECenterXAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum ECenterYAxZone
        {
            NONE = -1,
            MAX,
        }

        public enum ECenterTAxZone
        {
            NONE = -1,
            MAX,
        }

        public enum ECenterZAxZone
        {
            NONE = -1,
            MAX,
        }

        public class CMePushPullRefComp
        {
            public IIO IO;

            // Cylinder
            // Gripper
            public ICylinder Gripper;
            public ICylinder UDCyl;

            // Vacuum

            // MultiAxes
            public MMultiAxes_YMC AxPushPull;
            public MMultiAxes_YMC[] AxCenter = new MMultiAxes_YMC[(int)ECenterIndex.MAX];

            // Vision Object
        }

        public class CMePushPullData
        {
            // PushPull Type
            public EPushPullType[] PushPullType = new EPushPullType[DEF_MAX_COORDINATE];

            // Camera Number

            // Detect Object Sensor Address
            public int InDetectObject_Front = IO_ADDR_NOT_DEFINED;
            public int InDetectObject_Rear = IO_ADDR_NOT_DEFINED;

            // Physical check zone sensor. 원점복귀 여부와 상관없이 축의 물리적인 위치를 체크 및
            // 안전위치 이동 check
            public CMAxisZoneCheck PushPullZone;
            public CPos_XYTZ PushPullSafetyPos;

            public CMAxisZoneCheck[] CenterZone = new CMAxisZoneCheck[(int)ECenterIndex.MAX];
            public CPos_XYTZ CenterSafetyPos;

            public CMePushPullData(EPushPullType[] PushPullType = null)
            {
                if (PushPullType == null)
                {
                    for (int i = 0; i < this.PushPullType.Length; i++)
                    {
                        this.PushPullType[i] = EPushPullType.NONE;
                    }
                }
                else
                {
                    Array.Copy(PushPullType, this.PushPullType, PushPullType.Length);
                }

                PushPullZone = new CMAxisZoneCheck((int)EPushPullXAxZone.MAX, (int)EPushPullYAxZone.MAX,
                    (int)EPushPullTAxZone.MAX, (int)EPushPullZAxZone.MAX);

                for(int i = 0; i < CenterZone.Length; i++)
                {
                    CenterZone[i] = new CMAxisZoneCheck((int)ECenterXAxZone.MAX, (int)ECenterYAxZone.MAX,
                    (int)ECenterTAxZone.MAX, (int)ECenterZAxZone.MAX);
                }
            }
        }
    }

    public class MMePushPull : MMechanicalLayer
    {
        private CMePushPullRefComp m_RefComp;
        private CMePushPullData m_Data;

        // MovingObject
        public CMovingObject AxPushPullInfo { get; private set; } = new CMovingObject((int)EPushPullPos.MAX);
        public CMovingObject[] AxCenterInfo { get; private set; } = new CMovingObject[(int)ECenterIndex.MAX];

        // Cylinder

        // Vacuum

        public MMePushPull(CObjectInfo objInfo, CMePushPullRefComp refComp, CMePushPullData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            for(int i = 0; i < AxCenterInfo.Length; i++)
            {
                AxCenterInfo[i] = new CMovingObject((int)ECenterPos.MAX);
            }
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CMePushPullData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CMePushPullData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetPosition_PushPull(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            AxPushPullInfo.SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
            return SUCCESS;
        }

        public int SetPosition_Centering(ECenterIndex index, CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            AxCenterInfo[(int)index].SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
            return SUCCESS;
        }
        #endregion

        public int GripLock(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.Gripper.Down(bSkipSensor);
            return iResult;
        }

        public int GripRelease(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.Gripper.Up(bSkipSensor);
            return iResult;
        }

        public int IsGripLocked(out bool bStatus)
        {
            int iResult = m_RefComp.Gripper.IsDown(out bStatus);
            return iResult;
        }

        public int IsGripReleased(out bool bStatus)
        {
            int iResult = m_RefComp.Gripper.IsUp(out bStatus);
            return iResult;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            bStatus = true;
            bool bStatus1, bStatus2;
            int iResult = IsObjectDetected_Front(out bStatus1);
            if (iResult != SUCCESS) return iResult;
            iResult = IsObjectDetected_Rear(out bStatus2);
            if (iResult != SUCCESS) return iResult;

            // for safety, return false when all sensor is off
            if (bStatus1 == false && bStatus2 == false) bStatus = false;

            return SUCCESS;
        }

        public int IsObjectDetected_Front(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InDetectObject_Front, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsObjectDetected_Rear(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InDetectObject_Rear, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsAllAxisOrignReturned(out bool bStatus)
        {
#if SIMULATION_TEST
            // for test
            bStatus = true;
            return SUCCESS;
#endif

            bool[] bAxisStatus;
            int iResult = m_RefComp.AxPushPull.IsOriginReturned(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            for (int i = 0; i < m_RefComp.AxCenter.Length; i++)
            {
                iResult = m_RefComp.AxCenter[i].IsOriginReturned(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return iResult;
        }

        //////////////////////////////////////////////////////////////////////////////
        // PushPull
        public int GetPushPullCurPos(out CPos_XYTZ pos)
        {
            int iResult = m_RefComp.AxPushPull.GetCurPos(out pos);
            return iResult;
        }

        public int MovePushPullToSafetyPos(int axis)
        {
            int iResult = SUCCESS;
            string str;
            // 0. safety check
            iResult = CheckForPushPullMoving();
            if (iResult != SUCCESS) return iResult;

            // 0.1 trans to array
            double[] dPos = new double[1] { m_Data.PushPullSafetyPos.GetAt(axis) };

            // 0.2 set use flag
            bool[] bTempFlag = new bool[1] { true };

            // 1. Move
            iResult = m_RefComp.AxPushPull.Move(axis, bTempFlag, dPos);
            if (iResult != SUCCESS)
            {
                str = $"fail : move PushPull to safety pos [axis={axis}]";
                WriteLog(str, ELogType.Debug, ELogWType.D_Error);
                return iResult;
            }

            str = $"success : move PushPull to safety pos [axis={axis}";
            WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        /// <summary>
        /// sPos으로 이동하고, PosInfo를 iPos으로 셋팅한다. Backlash는 일단 차후로.
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="iPos"></param>
        /// <param name="bMoveFlag"></param>
        /// <param name="bUseBacklash"></param>
        /// <returns></returns>
        private int MovePushPullPos(CPos_XYTZ sPos, int iPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // safety check
            iResult = CheckForPushPullMoving();
            if (iResult != SUCCESS) return iResult;

            // assume move all axis if bMoveFlag is null
            if (bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, true };
            }

            // trans to array
            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // backlash
            if (bUseBacklash)
            {
                // 나중에 작업
            }

            // 1. move Z Axis to Safety Up. but when need to move z axis only, don't need to move z to safety pos
            if(m_RefComp.AxPushPull.HasAxis(DEF_Z) == true &&  m_Data.PushPullZone.UseSafetyMove[DEF_Z] == true)
            {
                if (bMoveFlag[DEF_X] == false && bMoveFlag[DEF_Y] == false && bMoveFlag[DEF_T] == false
                    && bMoveFlag[DEF_Z] == true)
                {
                    ;
                }
                else
                {
                    bool bStatus;
                    iResult = IsPushPullAxisInSafetyZone(DEF_Z, out bStatus);
                    if (iResult != SUCCESS) return iResult;
                    if (bStatus == false)
                    {
                        iResult = MovePushPullToSafetyPos(DEF_Z);
                        if (iResult != SUCCESS) return iResult;
                    }
                }
            }

            // 2. move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                // set priority
                if (bUsePriority == true && movePriority != null)
                {
                    m_RefComp.AxPushPull.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxPushPull.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move PushPull x y t axis", ELogType.Debug, ELogWType.D_Error);
                    return iResult;
                }
            }

            // 3. move Z Axis
            if(m_RefComp.AxPushPull.HasAxis(DEF_Z) == true)
            {
                if (bMoveFlag[DEF_Z] == true)
                {
                    bool[] bTempFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                    iResult = m_RefComp.AxPushPull.Move(DEF_ALL_COORDINATE, bTempFlag, dTargetPos);
                    if (iResult != SUCCESS)
                    {
                        WriteLog("fail : move PushPull z axis", ELogType.Debug, ELogWType.D_Error);
                        return iResult;
                    }
                }
            }

            string str = $"success : move PushPull to pos:{iPos} {sPos.ToString()}";
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
        public int MovePushPullPos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // Load Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            if (iPos == (int)EPushPullPos.LOADER)
            {
                AxPushPullInfo.InitAlignOffset();
            }

            CPos_XYTZ sTargetPos = AxPushPullInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            iResult = MovePushPullPos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxPushPullInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }

        /// <summary>
        /// PushPull Z축을 안전 Up 위치로 이동
        /// </summary>
        /// <returns></returns>
        public int MovePushPullToSafetyUp()
        {
            return MovePushPullToSafetyPos(DEF_Z);
        }

        /// <summary>
        /// PushPull를 LOAD, UNLOAD등의 목표위치로 이동시킬때에 좀더 편하게 이동시킬수 있도록 간편화한 함수
        /// Z축만 움직일 경우엔 Position Info를 업데이트 하지 않는다. 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveAllAxis"></param>
        /// <param name="bMoveXYT"></param>
        /// <param name="bMoveZ"></param>
        /// <returns></returns>
        public int MovePushPullPos(int iPos, bool bMoveXYT, bool bMoveZ, double[] dMoveOffset = null)
        {
            // 0. move all axis
            if (bMoveXYT && bMoveZ)
            {
                return MovePushPullPos(iPos, dMoveOffset: dMoveOffset);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MovePushPullPos(iPos, true, bMoveFlag, dMoveOffset: dMoveOffset);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MovePushPullPos(iPos, false, bMoveFlag, dMoveOffset: dMoveOffset);
            }

            return SUCCESS;
        }

        public int MovePushPullToLoaderPos(bool bMoveXYT = true, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EPushPullPos.LOADER;

            return MovePushPullPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
        }

        public int MovePushPullToHandlerPos(bool bMoveXYT = true, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EPushPullPos.HANDLER;

            return MovePushPullPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
        }

        public int MovePushPullToSpinner1Pos(bool bMoveXYT = true, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EPushPullPos.SPINNER1;

            return MovePushPullPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
        }

        public int MovePushPullToSpinner2Pos(bool bMoveXYT = true, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EPushPullPos.SPINNER2;

            return MovePushPullPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
        }

        public int MovePushPullToTempUnloadPos(bool bMoveXYT = true, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EPushPullPos.TEMP_UNLOAD;

            return MovePushPullPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
        }

        public int MovePushPullToReloadPos(bool bMoveXYT = true, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EPushPullPos.RELOAD;

            return MovePushPullPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
        }

        public int MovePushPullToWaitPos(bool bMoveXYT = true, bool bMoveZ = false, double[] dMoveOffset = null)
        {
            int iPos = (int)EPushPullPos.WAIT;

            return MovePushPullPos(iPos, bMoveXYT, bMoveZ, dMoveOffset);
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
        public int ComparePushPullPos(CPos_XYTZ sPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            // trans to array
            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = m_RefComp.AxPushPull.ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
            if (iResult != SUCCESS) return iResult;

            // skip axis
            if (bCheck_TAxis == false) bJudge[DEF_T] = true;
            if (bCheck_ZAxis == false) bJudge[DEF_Z] = true;

            // error check
            bResult = true;
            foreach (bool bTemp in bJudge)
            {
                if (bTemp == false) bResult = false;
            }

            // skip error?
            if (bSkipError == false && bResult == false)
            {
                string str = $"PushPull의 현재 위치와 일치하는 Position Info를 찾을수 없습니다. Current Pos : {sPos.ToString()}";
                WriteLog(str, ELogType.Debug, ELogWType.D_Error);

                return GenerateErrorCode(ERR_PUSHPULL_FAIL_TO_GET_CURRENT_POS_INFO);
            }

            return SUCCESS;
        }

        public int ComparePushPullPos(int iPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            CPos_XYTZ targetPos = AxPushPullInfo.GetTargetPos(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = ComparePushPullPos(targetPos, out bResult, bCheck_TAxis, bCheck_ZAxis, bSkipError);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetPushPullPosInfo(out int posInfo, bool bUpdatePos = true, bool bCheck_ZAxis = false)
        {
            posInfo = (int)EPushPullPos.NONE;
            bool bStatus;
            int iResult = IsPushPullAxisOrignReturned(out bStatus);
            if (iResult != SUCCESS) return iResult;

            // 실시간으로 자기 위치를 체크
            if (bUpdatePos)
            {
                for (int i = 0; i < (int)EPushPullPos.MAX; i++)
                {
                    ComparePushPullPos(i, out bStatus, false, bCheck_ZAxis);
                    if (bStatus)
                    {
                        AxPushPullInfo.PosInfo = i;
                        break;
                    }
                }
            }

            posInfo = AxPushPullInfo.PosInfo;
            return SUCCESS;
        }

        public void SetPushPullPosInfo(int posInfo)
        {
            AxPushPullInfo.PosInfo = posInfo;
        }

        public int IsPushPullAxisOrignReturned(out bool bStatus)
        {
            bool[] bAxisStatus;
            int iResult = m_RefComp.AxPushPull.IsOriginReturned(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return iResult;
        }

        public int GetPushPullAxZone(int axis, out int curZone)
        {
            bool bStatus;
            curZone = (int)EPushPullXAxZone.NONE;
            for (int i = 0; i < (int)EPushPullXAxZone.MAX; i++)
            {
                if (m_Data.PushPullZone.Axis[axis].ZoneAddr[i] == -1) continue; // if io is not allocated, continue;
                int iResult = m_RefComp.IO.IsOn(m_Data.PushPullZone.Axis[axis].ZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int IsPushPullAxisInSafetyZone(int axis, out bool bStatus)
        {
            bStatus = false;
            int curZone;
            int iResult = GetPushPullAxZone(axis, out curZone);
            if (iResult != SUCCESS) return iResult;

            switch (axis)
            {
                case DEF_X:
                    break;

                case DEF_Y:
                    //if (curZone == (int)EPushPullYAxZone.SAFETY)
                    //{
                    //    bStatus = true;
                    //}
                    break;

                case DEF_T:
                    break;

                case DEF_Z:
                    break;
            }
            return SUCCESS;
        }

        public int CheckForPushPullMoving(bool bCheckGripLock = true)
        {
            bool bStatus;

            // check origin
            int iResult = IsAllAxisOrignReturned(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false)
            {
                return GenerateErrorCode(ERR_PUSHPULL_NOT_ORIGIN_RETURNED);
            }

#if SIMULATION_TEST
            // for test
            return SUCCESS;
#endif

            // check lock
            if (bCheckGripLock == true)
            {
                iResult = IsObjectDetected(out bStatus);
                if (iResult != SUCCESS) return iResult;

                if (bStatus == true)
                {
                    iResult = IsGripLocked(out bStatus);
                    if (iResult != SUCCESS) return iResult;
                    if (bStatus == false)
                    {
                        return GenerateErrorCode(ERR_PUSHPULL_OBJECT_DETECTED_BUT_GRIP_NOT_LOCKED);
                    }
                }
                else
                {
                    iResult = IsGripReleased(out bStatus);
                    if (iResult != SUCCESS) return iResult;
                    if (bStatus == false)
                    {
                        return GenerateErrorCode(ERR_PUSHPULL_OBJECT_NOT_DETECTED_BUT_GRIP_NOT_RELEASED);
                    }
                }
            }

            // check cylinder

            return SUCCESS;
        }

        //////////////////////////////////////////////////////////////////////////////
        // Center Unit
        public MMultiAxes_YMC GetCenterAx(ECenterIndex index)
        {
            return m_RefComp.AxCenter[(int)index];
        }

        public int GetCenterCurPos(ECenterIndex index, out CPos_XYTZ pos)
        {
            int iResult = GetCenterAx(index).GetCurPos(out pos);
            return iResult;
        }

        public int MoveCenterToSafetyPos(ECenterIndex index, int axis)
        {
            int iResult = SUCCESS;
            string str;
            // 0. safety check
            iResult = CheckForCenterAxisMove(index);
            if (iResult != SUCCESS) return iResult;

            // 0.1 trans to array
            double[] dPos = new double[1] { m_Data.CenterSafetyPos.GetAt(axis) };

            // 0.2 set use flag
            bool[] bTempFlag = new bool[1] { true };

            // 1. Move
            iResult = GetCenterAx(index).Move(axis, bTempFlag, dPos);
            if (iResult != SUCCESS)
            {
                str = $"fail : move Center Unit to safety pos [axis={axis}]";
                WriteLog(str, ELogType.Debug, ELogWType.D_Error);
                return iResult;
            }

            str = $"success : move Center Unit to safety pos [axis={axis}";
            WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        /// <summary>
        /// sPos으로 이동하고, PosInfo를 iPos으로 셋팅한다. Backlash는 일단 차후로.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="sPos"></param>
        /// <param name="iPos"></param>
        /// <param name="bWaitDone"></param>
        /// <param name="bMoveFlag"></param>
        /// <param name="bUseBacklash"></param>
        /// <param name="bUsePriority"></param>
        /// <param name="movePriority"></param>
        /// <returns></returns>
        private int MoveCenterToPos(ECenterIndex index, CPos_XYTZ sPos, int iPos, bool bWaitDone, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // safety check
            iResult = CheckForCenterAxisMove(index);
            if (iResult != SUCCESS) return iResult;
            
            // assume move all axis if bMoveFlag is null
            if (bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, false, false, false};
            }

            // trans to array
            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // backlash
            if (bUseBacklash)
            {
                // 나중에 작업
            }

            // 1. move Z Axis to Safety Up. but when need to move z axis only, don't need to move z to safety pos
            //if (GetCenterAx(index).HasAxis(DEF_Z) == true)
            //{
            //    if (bMoveFlag[DEF_X] == false && bMoveFlag[DEF_Y] == false && bMoveFlag[DEF_T] == false
            //        && bMoveFlag[DEF_Z] == true)
            //    {
            //        ;
            //    }
            //    else
            //    {
            //        bool bStatus;
            //        iResult = IsCenteringAxisInSafetyZone(DEF_Z, out bStatus);
            //        if (iResult != SUCCESS) return iResult;
            //        if (bStatus == false)
            //        {
            //            iResult = MoveCenteringToSafetyPos(DEF_Z);
            //            if (iResult != SUCCESS) return iResult;
            //        }
            //    }
            //}

            // 2. move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                // set priority
                if (bUsePriority == true && movePriority != null)
                {
                    GetCenterAx(index).SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;

                if(bWaitDone == true)
                {
                    iResult = GetCenterAx(index).Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                } else
                {
                    iResult = GetCenterAx(index).StartMove(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos);
                }
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Center Unit x y t axis", ELogType.Debug, ELogWType.D_Error);
                    return iResult;
                }
            }

            // 3. move Z Axis
            //if (GetCenterAx(index).HasAxis(DEF_Z) == true)
            //{
            //    if (bMoveFlag[DEF_Z] == true)
            //    {
            //        bool[] bTempFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
            //        iResult = GetCenterAx(index).Move(DEF_ALL_COORDINATE, bTempFlag, dTargetPos);
            //        if (iResult != SUCCESS)
            //        {
            //            WriteLog("fail : move Center Unit z axis", ELogType.Debug, ELogWType.D_Error);
            //            return iResult;
            //        }
            //    }
            //}

            string str = $"success : move Center Unit to pos:{iPos} {sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        private int WaitForMoveCenterToPos(ECenterIndex index, bool[] bMoveFlag = null)
        {
            int iResult = SUCCESS;

            // assume move all axis if bMoveFlag is null
            if (bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, false, false, false };
            }

            // wait for done
            iResult = GetCenterAx(index).Wait4Done(DEF_ALL_COORDINATE, bMoveFlag);
            if (iResult != SUCCESS)
            {
                WriteLog($"fail : wait for cetering unit{index} move done", ELogType.Debug, ELogWType.D_Error);
                return iResult;
            }

            string str = $"success : wait for cetering unit{index} move done";
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
        public int MoveCenterToPos(ECenterIndex index, int iPos, bool bWaitDone, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // Load Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            //if (iPos == (int)ECenterPos.LOAD)
            //{
            //    AxCenterInfo[(int)index].InitAlignOffset();
            //}

            CPos_XYTZ sTargetPos = AxCenterInfo[(int)index].GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            iResult = MoveCenterToPos(index, sTargetPos, iPos, bWaitDone, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxCenterInfo[(int)index].PosInfo = iPos;
            }

            return SUCCESS;
        }

        /// <summary>
        /// Centering를 LOAD, UNLOAD등의 목표위치로 이동시킬때에 좀더 편하게 이동시킬수 있도록 간편화한 함수
        /// Z축만 움직일 경우엔 Position Info를 업데이트 하지 않는다. 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveAllAxis"></param>
        /// <param name="bMoveXYT"></param>
        /// <param name="bMoveZ"></param>
        /// <returns></returns>
        public int MoveCenterToPos(ECenterIndex index, int iPos, double[] dMoveOffset = null)
        {
            // 0. move all axis
            return MoveCenterToPos(index, iPos, bWaitDone:true, dMoveOffset:dMoveOffset);

        }

        public int MoveAllCenterUnitToWaitPos(double[] dMoveOffset = null)
        {
            int iPos = (int)ECenterPos.WAIT;

            int iResult = MoveCenterToPos(ECenterIndex.LEFT, iPos, bWaitDone:false, dMoveOffset:dMoveOffset);
            if (iResult != SUCCESS) return iResult;
            iResult = MoveCenterToPos(ECenterIndex.RIGHT, iPos, bWaitDone: true, dMoveOffset: dMoveOffset);
            if (iResult != SUCCESS) return iResult;
            iResult = WaitForMoveCenterToPos(ECenterIndex.LEFT);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveAllCenterUnitToCenteringPos(double[] dMoveOffset = null)
        {
            int iPos = (int)ECenterPos.CENTERING;

            int iResult = MoveCenterToPos(ECenterIndex.LEFT, iPos, bWaitDone: false, dMoveOffset: dMoveOffset);
            if (iResult != SUCCESS) return iResult;
            iResult = MoveCenterToPos(ECenterIndex.RIGHT, iPos, bWaitDone: true, dMoveOffset: dMoveOffset);
            if (iResult != SUCCESS) return iResult;
            iResult = WaitForMoveCenterToPos(ECenterIndex.LEFT);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
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
        public int CompareCenterPos(ECenterIndex index, CPos_XYTZ sPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            // trans to array
            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = GetCenterAx(index).ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
            if (iResult != SUCCESS) return iResult;

            // skip axis
            if (bCheck_TAxis == false) bJudge[DEF_T] = true;
            if (bCheck_ZAxis == false) bJudge[DEF_Z] = true;

            // error check
            bResult = true;
            foreach (bool bTemp in bJudge)
            {
                if (bTemp == false) bResult = false;
            }

            // skip error?
            if (bSkipError == false && bResult == false)
            {
                string str = $"Center Unit의 현재 위치와 일치하는 Position Info를 찾을수 없습니다. Current Pos : {sPos.ToString()}";
                WriteLog(str, ELogType.Debug, ELogWType.D_Error);

                return GenerateErrorCode(ERR_PUSHPULL_FAIL_TO_GET_CURRENT_POS_INFO);
            }

            return SUCCESS;
        }

        public int CompareCenterPos(ECenterIndex index, int iPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            CPos_XYTZ targetPos = AxCenterInfo[(int)index].GetTargetPos(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = CompareCenterPos(index, targetPos, out bResult, bCheck_TAxis, bCheck_ZAxis, bSkipError);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetCenterPosInfo(ECenterIndex index, out int posInfo, bool bUpdatePos = true, bool bCheck_ZAxis = false)
        {
            posInfo = (int)ECenterPos.NONE;
            bool bStatus;
            int iResult = IsCenterAxisOrignReturned(index, out bStatus);
            if (iResult != SUCCESS) return iResult;

            // 실시간으로 자기 위치를 체크
            if (bUpdatePos)
            {
                for (int i = 0; i < (int)ECenterPos.MAX; i++)
                {
                    CompareCenterPos(index, i, out bStatus, false, bCheck_ZAxis);
                    if (bStatus)
                    {
                        AxCenterInfo[(int)index].PosInfo = i;
                        break;
                    }
                }
            }

            posInfo = AxCenterInfo[(int)index].PosInfo;
            return SUCCESS;
        }

        public void SetCenterPosInfo(ECenterIndex index, int posInfo)
        {
            AxCenterInfo[(int)index].PosInfo = posInfo;
        }

        public int IsCenterAxisOrignReturned(ECenterIndex index, out bool bStatus)
        {
            bool[] bAxisStatus;
            int iResult = GetCenterAx(index).IsOriginReturned(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return iResult;
        }

        public int GetCenterAxZone(ECenterIndex index, int axis, out int curZone)
        {
            bool bStatus;
            curZone = (int)ECenterXAxZone.NONE;
            for (int i = 0; i < (int)ECenterXAxZone.MAX; i++)
            {
                if (m_Data.CenterZone[(int)index].Axis[axis].ZoneAddr[i] == -1) continue; // if io is not allocated, continue;
                int iResult = m_RefComp.IO.IsOn(m_Data.CenterZone[(int)index].Axis[axis].ZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int IsCenterAxisInSafetyZone(ECenterIndex index, int axis, out bool bStatus)
        {
            bStatus = false;
            int curZone;
            int iResult = GetCenterAxZone(index, axis, out curZone);
            if (iResult != SUCCESS) return iResult;

            switch (axis)
            {
                case DEF_X:
                    if (curZone == (int)ECenterXAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;

                case DEF_Y:
                    break;

                case DEF_T:
                    break;

                case DEF_Z:
                    break;
            }
            return SUCCESS;
        }

        public int CheckForCenterAxisMove(ECenterIndex index, bool bCheckGripLock = true)
        {
            bool bStatus;

            // check origin
            int iResult = IsAllAxisOrignReturned(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false)
            {
                return GenerateErrorCode(ERR_PUSHPULL_NOT_ORIGIN_RETURNED);
            }

#if SIMULATION_TEST
            // for test
            return SUCCESS;
#endif

            // check lock
            if (bCheckGripLock == true)
            {
                iResult = IsObjectDetected(out bStatus);
                if (iResult != SUCCESS) return iResult;

                if (bStatus == true)
                {
                    iResult = IsGripLocked(out bStatus);
                    if (iResult != SUCCESS) return iResult;
                    if (bStatus == false)
                    {
                        return GenerateErrorCode(ERR_PUSHPULL_OBJECT_DETECTED_BUT_GRIP_NOT_LOCKED);
                    }
                }
                else
                {
                    iResult = IsGripReleased(out bStatus);
                    if (iResult != SUCCESS) return iResult;
                    if (bStatus == false)
                    {
                        return GenerateErrorCode(ERR_PUSHPULL_OBJECT_NOT_DETECTED_BUT_GRIP_NOT_RELEASED);
                    }
                }
            }

            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_Z
        public int IsCylUp(out bool bStatus)
        {
            bStatus = false;
            if (m_RefComp.UDCyl == null) return GenerateErrorCode(ERR_PUSHPULL_UNABLE_TO_USE_CYL);
            int iResult = m_RefComp.UDCyl.IsUp(out bStatus);

            return iResult;
        }

        public int IsCylDown(out bool bStatus)
        {
            bStatus = false;
            if (m_RefComp.UDCyl == null) return GenerateErrorCode(ERR_PUSHPULL_UNABLE_TO_USE_CYL);
            int iResult = m_RefComp.UDCyl.IsDown(out bStatus);

            return iResult;
        }

        public int CylUp(bool bSkipSensor = false)
        {
            // check for safety
            int iResult = CheckForUDCylMove();
            if (iResult != SUCCESS) return iResult;

            if (m_RefComp.UDCyl == null) return GenerateErrorCode(ERR_PUSHPULL_UNABLE_TO_USE_CYL);
            iResult = m_RefComp.UDCyl.Up(bSkipSensor);

            return iResult;
        }

        public int CylDown(bool bSkipSensor = false, int index = DEF_Z)
        {
            // check for safety
            int iResult = CheckForUDCylMove();
            if (iResult != SUCCESS) return iResult;

            if (m_RefComp.UDCyl == null) return GenerateErrorCode(ERR_PUSHPULL_UNABLE_TO_USE_CYL);
            iResult = m_RefComp.UDCyl.Down(bSkipSensor);

            return iResult;
        }

        public int CheckForUDCylMove(bool bCheckGripLock = true)
        {
            // check cylinder
            // 나중에 구체적으로 정해지면 인터락 상관관계 다시한번 잡아야 하기에..

            return SUCCESS;
        }

    }
}
