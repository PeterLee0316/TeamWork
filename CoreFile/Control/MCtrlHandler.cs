using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_MeHandler;
using static Core.Layers.DEF_CtrlHandler;

namespace Core.Layers
{
    public class DEF_CtrlHandler
    {
        public const int ERR_CTRLHANDLER_NOT_ORIGIN_RETURNED = 1;
        public const int ERR_CTRLHANDLER_SPARE2                             = 2;
        public const int ERR_CTRLHANDLER_OBJECT_ABSORBED                               = 3;
        public const int ERR_CTRLHANDLER_OBJECT_NOT_ABSORBED                           = 4;
        public const int ERR_CTRLHANDLER_OBJECT_DETECTED                                  = 5;
        public const int ERR_CTRLHANDLER_OBJECT_NOT_DETECTED                              = 6;
        public const int ERR_CTRLHANDLER_OBJECT_NOT_DETECTED_BUT_ABSORBED                 = 7;
        public const int ERR_CTRLHANDLER_CHECK_RUN_BEFORE_FAILED                       = 8;
        public const int ERR_CTRLHANDLER_CYLINDER_TIMEOUT                              = 9;
        public const int ERR_CTRLHANDLER_NOT_UP                                        = 10;
        public const int ERR_CTRLHANDLER_CANNOT_DETECT_POSINFO                         = 11;
        public const int ERR_CTRLHANDLER_SPARE3                             = 12;
        public const int ERR_CTRLHANDLER_UPPER_IN_DOWN_AND_LOWER_IN_SAME_XZONE         = 13;
        public const int ERR_CTRLHANDLER_UPPER_NEED_DOWN_AND_LOWER_IN_SAME_XZONE       = 14;
        public const int ERR_CTRLHANDLER_LOWER_NEED_MOVE_AND_UPPER_IN_DOWN             = 15;
        public const int ERR_CTRLHANDLER_XAX_POS_NOT_MATCH_ZONE                        = 16;
        public const int ERR_CTRLHANDLER_MAY_COLLIDE_WITH_OPPOSITE_HANDLER             = 17;

        /// <summary>
        /// Handler가 Upper/Lower 두 종류인데, 각각 Upper = LOAD, Lower = UNLOAD 용도로 사용
        /// </summary>
        public enum EHandlerIndex
        {
            LOAD_UPPER,     // Use UpperHandler for Loading
            UNLOAD_LOWER,   // Use LowerHandler for Unloading
            MAX,
        }

        public class CCtrlHandlerRefComp
        {
            // MeHandler
            public MMeHandler UpperHandler;
            public MMeHandler LowerHandler;

            // MeStage
            //public MMeStage1 Stage1;

            // MePushPull
            //public MMePushPull PushPull;

            public CCtrlHandlerRefComp()
            {
            }

            public override string ToString()
            {
                return $"CCtrlHandlerRefComp : ";
            }
        }

        public class CCtrlHandlerData
        {
            public CCtrlHandlerData()
            {

            }
        }
    }

    public class MCtrlHandler : MCtrlLayer
    {
        private CCtrlHandlerRefComp m_RefComp;
        private CCtrlHandlerData m_Data;

        public MCtrlHandler(CObjectInfo objInfo, CCtrlHandlerRefComp refComp, CCtrlHandlerData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CCtrlHandlerData source)
        {
            m_Data = ObjectExtensions.Copy(source);

            return SUCCESS;
        }

        public int GetData(out CCtrlHandlerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public MMeHandler GetHandler(EHandlerIndex index)
        {
            if(index == EHandlerIndex.LOAD_UPPER)
            {
                return m_RefComp.UpperHandler;
            }
            else
            {
                return m_RefComp.LowerHandler;
            }
        }

        public MMeHandler GetOtherHandler(EHandlerIndex index)
        {
            if (index == EHandlerIndex.LOAD_UPPER)
            {
                return m_RefComp.LowerHandler;
            }
            else
            {
                return m_RefComp.UpperHandler;
            }
        }

        public EHandlerIndex GetOtherIndex(EHandlerIndex index)
        {
            if (index == EHandlerIndex.LOAD_UPPER)
            {
                return EHandlerIndex.UNLOAD_LOWER;
            }
            else
            {
                return EHandlerIndex.LOAD_UPPER;
            }
        }

        public override int Initialize()
        {
            int iResult = SUCCESS;
            bool bStatus, bStatus1;
            // UpperHandler
            // 0. check vacuum
            EHandlerIndex index = EHandlerIndex.LOAD_UPPER;
            iResult = IsObjectDetected(index, out bStatus);
            if (iResult != SUCCESS) return iResult;

            if (bStatus)
            {
                iResult = Absorb(index);
                if (iResult != SUCCESS) return iResult;
            }
            else
            {
                iResult = IsReleased(index, out bStatus1);
                if (iResult != SUCCESS) return iResult;
                if (bStatus1 == false) return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_NOT_DETECTED_BUT_ABSORBED);
            }

            // move to wait pos
            iResult = MoveToWaitPos(index, bStatus);
            if (iResult != SUCCESS) return iResult;

            // LowerHandler
            // 0. check vacuum
            index = EHandlerIndex.UNLOAD_LOWER;
            iResult = IsObjectDetected(index, out bStatus);
            if (iResult != SUCCESS) return iResult;

            if (bStatus)
            {
                iResult = Absorb(index);
                if (iResult != SUCCESS) return iResult;
            }
            else
            {
                iResult = IsReleased(index, out bStatus1);
                if (iResult != SUCCESS) return iResult;
                if (bStatus1 == false) return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_NOT_DETECTED_BUT_ABSORBED);
            }

            // move to wait pos
            iResult = MoveToWaitPos(index, bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        #endregion

        #region Cylinder, Vacuum, Detect Object
        public int IsObjectDetected(EHandlerIndex index, out bool bStatus)
        {
            bStatus = false;
            int iResult = GetHandler(index).IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsAbsorbed(EHandlerIndex index, out bool bStatus)
        {
            bStatus = false;
            int iResult = GetHandler(index).IsAbsorbed(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsReleased(EHandlerIndex index, out bool bStatus)
        {
            bStatus = false;
            int iResult = GetHandler(index).IsReleased(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int Absorb(EHandlerIndex index, bool bSkipSensor = false)
        {
            int iResult = GetHandler(index).Absorb(bSkipSensor);
            return iResult;
        }

        public int Release(EHandlerIndex index, bool bSkipSensor = false)
        {
            int iResult = GetHandler(index).Absorb(bSkipSensor);
            return iResult;
        }
        #endregion

        /// <summary>
        /// Handler의 이동전에 Run Mode에 따라서, Object가 감지되어야 하는지 및 진공 여부를 체크
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bTransfer">Object가 있어야 되는지</param>
        /// <param name="bCheck_WhenAutoRun">AutoRun모드에서 Error 발생 여부</param>
        /// <returns></returns>
        private int CheckVacuum_forMoving(EHandlerIndex index, bool bTransfer, bool bCheck_WhenAutoRun = false)
        {
            int iResult = SUCCESS;
            bool bDetected, bAbsorbed;

            // check vacuum
            iResult = IsObjectDetected(index, out bDetected);
            if (iResult != SUCCESS) return iResult;

            iResult = IsAbsorbed(index, out bAbsorbed);
            if (iResult != SUCCESS) return iResult;

            if (bTransfer)
            {
                if (bDetected == true && bAbsorbed == false)
                {
                    iResult = Absorb(index);
                    if (iResult != SUCCESS) return iResult;

                    bAbsorbed = true;
                }
            }

            // Panel이 있든 없든 상관없는 위치들, 가령 대기, 마크 등등의 위치를 위해서
            if (bCheck_WhenAutoRun == true) return SUCCESS;

            // check object exist when auto run
            if (AutoManualMode == EAutoManual.AUTO)
            {
                if (AutoRunMode != EAutoRunMode.DRY_RUN) // not dry run
                {
                    if (bDetected != bTransfer)
                    {
                        if (bTransfer)    // Panel이 있어야 할 상황일경우
                        {
                            WriteLog("CtrlHandler의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT NOT EXIST", ELogType.Debug, ELogWType.D_Error);
                            return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_NOT_DETECTED);
                        }
                        else
                        {
                            WriteLog("CtrlHandler의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.D_Error);
                            return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_DETECTED);
                        }
                    }
                }
                else // dry run
                {
                    if (bDetected || bAbsorbed)
                    {
                        WriteLog("CtrlHandler의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.D_Error);
                        return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_DETECTED);
                    }
                }
            }

            return SUCCESS;
        }

        /// <summary>
        /// Handler의 현재 위치를 확인해서, position info를 리턴한다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="curPos">position info</param>
        /// <param name="firstCheckPos">특정 position info부터 체크할 필요가 있을때</param>
        /// <param name="bCheck_ZAxis">z축도 검사할건지</param>
        /// <returns></returns>
        public int CheckHandlerPosition(EHandlerIndex index, out int curPos, 
            int firstCheckPos = (int)EHandlerPos.NONE, bool bCheck_ZAxis = true)
        {
            int iResult = SUCCESS;

            // Init
            curPos = (int)EHandlerPos.NONE;

            // 1. check first position
            if(firstCheckPos != (int)EHandlerPos.NONE)
            {
                bool bResult;
                iResult = GetHandler(index).CompareHandlerPos(firstCheckPos, out bResult, false, bCheck_ZAxis);
                if (iResult != SUCCESS) return iResult;
                if(bResult)
                {
                    curPos = firstCheckPos;
                    return SUCCESS;
                }
            }

            // Get Position Info
            iResult = GetHandler(index).GetHandlerPosInfo(out curPos, true, bCheck_ZAxis);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// X축의 Position Info 와 구간 감지sensor를 이용한 zone information이 일치하는지를 확인한다
        /// </summary>
        /// <param name="curPos"></param>
        /// <param name="curZone_X"></param>
        /// <returns></returns>
        private int CheckXAxMatchZone(int curPos, int curZone_X)
        {
#if SIMULATION_TEST
            return SUCCESS;
#endif

            switch (curPos)
            {
                case (int)EHandlerPos.WAIT:
                    if (curZone_X != (int)EHandlerXAxZone.WAIT)
                        return GenerateErrorCode(ERR_CTRLHANDLER_XAX_POS_NOT_MATCH_ZONE);
                    break;
                case (int)EHandlerPos.PUSHPULL:
                    if (curZone_X != (int)EHandlerXAxZone.PUSHPULL)
                        return GenerateErrorCode(ERR_CTRLHANDLER_XAX_POS_NOT_MATCH_ZONE);
                    break;
                case (int)EHandlerPos.STAGE:
                    if (curZone_X != (int)EHandlerXAxZone.STAGE)
                        return GenerateErrorCode(ERR_CTRLHANDLER_XAX_POS_NOT_MATCH_ZONE);
                    break;
            }
            return SUCCESS;
        }

        /// <summary>
        /// Handler가 이동하기전에, 반대편 Handler가 충돌 전에 있는지를 확인하여 이동 가능 여부 확인
        /// Opposite Handler가 충돌 위치에 있을 때, 자동 운전 모드에서는 error를 return 하지 않는다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="nTargetPos"></param>
        /// <param name="bMozeZAxis"></param>
        /// <param name="capableMove"></param>
        /// <returns></returns>
        public int CheckOppositeHandler_forMoving(EHandlerIndex index, int nTargetPos, bool bMozeZAxis, out bool capableMove)
        {
#if SIMULATION_TEST
            capableMove = true;
            return SUCCESS;
#endif

            int iResult = SUCCESS;
            capableMove = false;

            // 0. init
            int curPos = (int)EHandlerPos.NONE;

            // 1. get current pos through motor position
            // need to decide check position interlock.. -> don't need because handler process is only one. don't need to concern multi process
            iResult = CheckHandlerPosition(index, out curPos, nTargetPos, false);
            if (iResult != SUCCESS) return iResult;
            if (curPos == (int)EHandlerPos.NONE)
                return GenerateErrorCode(ERR_CTRLHANDLER_CANNOT_DETECT_POSINFO);

            int other_curPos;
            iResult = CheckHandlerPosition(GetOtherIndex(index), out other_curPos, nTargetPos, false);
            if (iResult != SUCCESS) return iResult;
            if (other_curPos == (int)EHandlerPos.NONE)
                return GenerateErrorCode(ERR_CTRLHANDLER_CANNOT_DETECT_POSINFO);

            // 2. get current zone through detect sensor
            int curZone_X, other_curZone_X;
            iResult = GetHandler(index).GetHandlerAxZone(DEF_X, out curZone_X);
            if (iResult != SUCCESS) return iResult;
            iResult = GetOtherHandler(index).GetHandlerAxZone(DEF_X, out other_curZone_X);
            if (iResult != SUCCESS) return iResult;

            int curZone_Z, other_curZone_Z;
            iResult = GetHandler(index).GetHandlerAxZone(DEF_Z, out curZone_Z);
            if (iResult != SUCCESS) return iResult;
            iResult = GetOtherHandler(index).GetHandlerAxZone(DEF_Z, out other_curZone_Z);
            if (iResult != SUCCESS) return iResult;

            // 3. check curPos matching with cur zone
            iResult = CheckXAxMatchZone(curPos, curZone_X);
            if (iResult != SUCCESS) return iResult;

            iResult = CheckXAxMatchZone(other_curPos, other_curZone_X);
            if (iResult != SUCCESS) return iResult;

            // 4. check interlock opposite handler
            if (index == (int)EHandlerIndex.LOAD_UPPER) // Upper Handler
            {
                // Upper Handler가 Up이 아니면서, Lower Handler도 같은 구간에 있을때, Z축이 상승하면서 충돌.
                if (curZone_Z != (int)EHandlerZAxZone.SAFETY)
                {
                    if (curZone_X == other_curZone_X)
                    {
                        if(AutoManualMode == EAutoManual.MANUAL)
                            return GenerateErrorCode(ERR_CTRLHANDLER_UPPER_IN_DOWN_AND_LOWER_IN_SAME_XZONE);
                    }
                }

                // Z축도 이동시킬 때, Lower Handler가 목표 구간과 같은 구간에 있다면, Z축이 다운하면서 충돌.
                if (bMozeZAxis == true &&
                    (nTargetPos == (int)EHandlerPos.PUSHPULL || nTargetPos == (int)EHandlerPos.STAGE))
                {
                    if (nTargetPos == other_curPos)
                    {
                        if (AutoManualMode == EAutoManual.MANUAL)
                            return GenerateErrorCode(ERR_CTRLHANDLER_UPPER_NEED_DOWN_AND_LOWER_IN_SAME_XZONE);
                    }
                }
            }
            else // Lower Handler
            {
                // Upper Handler의 Z축이 Up 상태라면 Lower Handler가 이동해도 충돌 발생하지 않음.
                // 즉, Upper Handler가 다운상태일때,
                if (other_curZone_Z != (int)EHandlerZAxZone.SAFETY)
                {
                    // Upper Handler가 목표 구간과 같은 구간에 있다면, 충돌.
                    if (curZone_X == other_curZone_X)
                    {
                        if (AutoManualMode == EAutoManual.MANUAL)
                            return GenerateErrorCode(ERR_CTRLHANDLER_UPPER_IN_DOWN_AND_LOWER_IN_SAME_XZONE);
                    }

                    // Upper Handler가 중간 지점 wait zone에서 down 되어있을경우, 충돌.
                    if (other_curZone_X == (int)EHandlerXAxZone.WAIT)
                    {
                        if (AutoManualMode == EAutoManual.MANUAL)
                            return GenerateErrorCode(ERR_CTRLHANDLER_LOWER_NEED_MOVE_AND_UPPER_IN_DOWN);
                    }

                    // Upper Handler가 Lower Handler와 같은 구간에 있을때, 충돌.
                    if (nTargetPos == other_curPos)
                    {
                        if (AutoManualMode == EAutoManual.MANUAL)
                            return GenerateErrorCode(ERR_CTRLHANDLER_LOWER_NEED_MOVE_AND_UPPER_IN_DOWN);
                    }
                }
            }

            capableMove = true;
            return SUCCESS;
        }

        public int MoveToWaitPos(EHandlerIndex index, bool bTransfer, bool bMoveXYT = true, bool bMoveZ = true, double dZMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Z] = dZMoveOffset;

            int iResult = CheckVacuum_forMoving(index, bTransfer, bMoveZ);
            if (iResult != SUCCESS) return iResult;

            bool capableMove;
            iResult = CheckOppositeHandler_forMoving(index, (int)EHandlerPos.WAIT, bMoveZ, out capableMove);
            if (iResult != SUCCESS) return iResult;
            if (capableMove == false) return GenerateErrorCode(ERR_CTRLHANDLER_MAY_COLLIDE_WITH_OPPOSITE_HANDLER);

            iResult = GetHandler(index).MoveHandlerToWaitPos(bMoveXYT, bMoveZ, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToPushPullPos(EHandlerIndex index, bool bTransfer, bool bMoveXYT = true, bool bMoveZ = true, double dZMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Z] = dZMoveOffset;

            int iResult = CheckVacuum_forMoving(index, bTransfer, bMoveZ);
            if (iResult != SUCCESS) return iResult;

            bool capableMove;
            iResult = CheckOppositeHandler_forMoving(index, (int)EHandlerPos.PUSHPULL, bMoveZ, out capableMove);
            if (iResult != SUCCESS) return iResult;
            if (capableMove == false) return GenerateErrorCode(ERR_CTRLHANDLER_MAY_COLLIDE_WITH_OPPOSITE_HANDLER);

            iResult = GetHandler(index).MoveHandlerToPushPullPos(bMoveXYT, bMoveZ, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToStagePos(EHandlerIndex index, bool bTransfer, bool bMoveXYT = true, bool bMoveZ = true, double dZMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Z] = dZMoveOffset;

            int iResult = CheckVacuum_forMoving(index, bTransfer, bMoveZ);
            if (iResult != SUCCESS) return iResult;

            bool capableMove;
            iResult = CheckOppositeHandler_forMoving(index, (int)EHandlerPos.STAGE, bMoveZ, out capableMove);
            if (iResult != SUCCESS) return iResult;
            if (capableMove == false) return GenerateErrorCode(ERR_CTRLHANDLER_MAY_COLLIDE_WITH_OPPOSITE_HANDLER);

            iResult = GetHandler(index).MoveHandlerToStagePos(bMoveXYT, bMoveZ, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveZToSafetyPos(EHandlerIndex index, bool bTransfer, double dZMoveOffset = 0)
        {
            // 0. init
            int curPos = (int)EHandlerPos.NONE;

            // 1. get current pos through motor position
            // need to decide check position interlock.. -> don't need because handler process is only one. don't need to concern multi process
            int iResult = CheckHandlerPosition(index, out curPos, -1, false);
            if (iResult != SUCCESS) return iResult;
            if (curPos == (int)EHandlerPos.NONE)
                return GenerateErrorCode(ERR_CTRLHANDLER_CANNOT_DETECT_POSINFO);

            // 2. check safety
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Z] = dZMoveOffset;

            iResult = CheckVacuum_forMoving(index, bTransfer, true);
            if (iResult != SUCCESS) return iResult;

            bool capableMove;
            iResult = CheckOppositeHandler_forMoving(index, curPos, true, out capableMove);
            if (iResult != SUCCESS) return iResult;
            if (capableMove == false) return GenerateErrorCode(ERR_CTRLHANDLER_MAY_COLLIDE_WITH_OPPOSITE_HANDLER);

            // 3. vacuum on/off
            if(bTransfer)
            {
                iResult = Release(index);
                if (iResult != SUCCESS) return iResult;
            } else
            {
                iResult = Absorb(index);
                if (iResult != SUCCESS) return iResult;
            }

            // 4. move
            iResult = GetHandler(index).MoveHandlerZToSafetyPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveZToLoadUnloadPos(EHandlerIndex index, bool bTransfer, double dZMoveOffset = 0)
        {
            // 0. init
            int curPos = (int)EHandlerPos.NONE;

            // 1. get current pos through motor position
            // need to decide check position interlock.. -> don't need because handler process is only one. don't need to concern multi process
            int iResult = CheckHandlerPosition(index, out curPos, -1, false);
            if (iResult != SUCCESS) return iResult;
            if (curPos == (int)EHandlerPos.NONE)
                return GenerateErrorCode(ERR_CTRLHANDLER_CANNOT_DETECT_POSINFO);

            // 2. check safety
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Z] = dZMoveOffset;

            iResult = CheckVacuum_forMoving(index, bTransfer, true);
            if (iResult != SUCCESS) return iResult;

            bool capableMove;
            iResult = CheckOppositeHandler_forMoving(index, curPos, true, out capableMove);
            if (iResult != SUCCESS) return iResult;
            if (capableMove == false) return GenerateErrorCode(ERR_CTRLHANDLER_MAY_COLLIDE_WITH_OPPOSITE_HANDLER);

            // 3. vacuum on/off
            if (bTransfer)
            {
                iResult = Release(index);
                if (iResult != SUCCESS) return iResult;
            }
            else
            {
                iResult = Absorb(index);
                if (iResult != SUCCESS) return iResult;
            }

            // 4. move
            iResult = GetHandler(index).MoveHandlerToPos(curPos, false, true, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
    }
}
