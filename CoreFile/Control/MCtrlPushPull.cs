using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_MePushPull;
using static Core.Layers.DEF_CtrlPushPull;

namespace Core.Layers
{
    public class DEF_CtrlPushPull
    {
        public const int ERR_CTRL_PUSHPULL_NOT_ORIGIN_RETURNED                                   = 1;
        public const int ERR_CTRL_PUSHPULL_SPARE2                                     = 2;
        public const int ERR_CTRL_PUSHPULL_OBJECT_GRIP_LOCKED                                    = 3;
        public const int ERR_CTRL_PUSHPULL_OBJECT_GRIP_RELEASED                                  = 4;
        public const int ERR_CTRL_PUSHPULL_OBJECT_DETECTED                                          = 5;
        public const int ERR_CTRL_PUSHPULL_OBJECT_NOT_EXIST                                      = 6;
        public const int ERR_CTRL_PUSHPULL_OBJECT_DETECTED_BUT_GRIP_NOT_LOCKED                   = 7;
        public const int ERR_CTRL_PUSHPULL_OBJECT_NOT_DETECTED_BUT_GRIP_NOT_RELEASED             = 8;
        public const int ERR_CTRL_PUSHPULL_CHECK_RUN_BEFORE_FAILED                               = 9;
        public const int ERR_CTRL_PUSHPULL_CYLINDER_TIMEOUT                                      = 10;
        public const int ERR_CTRL_PUSHPULL_CANNOT_DETECT_POSINFO                                 = 11;
        public const int ERR_CTRL_PUSHPULL_UPPER_HANDLER_IS_NOT_UP                               = 12;
        public const int ERR_CTRL_PUSHPULL_LOWER_HANDLER_IS_NOT_UP                               = 13;
        public const int ERR_CTRL_PUSHPULL_SPINNER1_IS_NOT_DOWN                                  = 14;
        public const int ERR_CTRL_PUSHPULL_SPINNER2_IS_NOT_DOWN                                  = 15;
        public const int ERR_CTRL_PUSHPULL_ELEVATOR_IS_NOT_SAFE                                  = 16;

        public class CCtrlPushPullRefComp
        {
            public IIO IO;
            public MMePushPull PushPull;
            public MMeHandler UpperHandler;
            public MMeHandler LowerHandler;
            public MMeSpinner Spinner1;
            public MMeSpinner Spinner2;
            public MMeElevator Elevator;

            //public CCtrlPushPullRefComp()
            //{
            //}
            public override string ToString()
            {
                return $"CCtrlPushPullRefComp : ";
            }
        }

        public class CCtrlPushPullData
        {
        }
    }

    public class MCtrlPushPull : MCtrlLayer
    {
        private CCtrlPushPullRefComp m_RefComp;
        private CCtrlPushPullData m_Data;

        public MCtrlPushPull(CObjectInfo objInfo,
            CCtrlPushPullRefComp refComp, CCtrlPushPullData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CCtrlPushPullData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CCtrlPushPullData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public override int Initialize()
        {
            int iResult = SUCCESS;
            bool bStatus, bStatus1;


            return SUCCESS;
        }
        #endregion

        #region Cylinder, Vacuum, Detect Object
        public int GripLock(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.PushPull.GripLock(bSkipSensor);
            return iResult;
        }

        public int GripRelease(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.PushPull.GripRelease(bSkipSensor);
            return iResult;
        }

        public int IsGripLocked(out bool bStatus)
        {
            int iResult = m_RefComp.PushPull.IsGripLocked(out bStatus);
            return iResult;
        }

        public int IsGripReleased(out bool bStatus)
        {
            int iResult = m_RefComp.PushPull.IsGripReleased(out bStatus);
            return iResult;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            int iResult = m_RefComp.PushPull.IsObjectDetected(out bStatus);
            return iResult;
        }

        public int IsObjectDetected_Front(out bool bStatus)
        {
            int iResult = m_RefComp.PushPull.IsObjectDetected_Front(out bStatus);
            return iResult;
        }

        public int IsObjectDetected_Rear(out bool bStatus)
        {
            int iResult = m_RefComp.PushPull.IsObjectDetected_Rear(out bStatus);
            return iResult;
        }
        #endregion

        private int CheckLock_forMove(bool bTransfer, bool bCheck_WhenAutoRun = false)
        {
            int iResult = SUCCESS;
            bool bDetected, bAbsorbed;

            // check vacuum
            iResult = IsObjectDetected(out bDetected);
            if (iResult != SUCCESS) return iResult;

            iResult = IsGripLocked(out bAbsorbed);
            if (iResult != SUCCESS) return iResult;

            if (bTransfer)
            {
                if (bDetected == true && bAbsorbed == false)
                {
                    iResult = GripLock();
                    if (iResult != SUCCESS) return iResult;

                    bAbsorbed = true;
                }
            }

            // Panel이 있든 없든 상관없는 위치들, 가령 대기, 마크 등등의 위치를 위해서
            if (bCheck_WhenAutoRun == true) return SUCCESS;

#if !SIMULATION_TEST
            // check object exist when auto run
            if (AutoManualMode == EAutoManual.AUTO)
            {
                if (AutoRunMode != EAutoRunMode.DRY_RUN) // not dry run
                {
                    if (bDetected != bTransfer)
                    {
                        if (bTransfer)    // Panel이 있어야 할 상황일경우
                        {
                            WriteLog("CtrlPushPull의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT NOT EXIST", ELogType.Debug, ELogWType.D_Error);
                            return GenerateErrorCode(ERR_CTRL_PUSHPULL_OBJECT_NOT_EXIST);
                        }
                        else
                        {
                            WriteLog("CtrlPushPull의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.D_Error);
                            return GenerateErrorCode(ERR_CTRL_PUSHPULL_OBJECT_DETECTED);
                        }
                    }
                }
                else // dry run
                {
                    if (bDetected || bAbsorbed)
                    {
                        WriteLog("CtrlPushPull의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.D_Error);
                        return GenerateErrorCode(ERR_CTRL_PUSHPULL_OBJECT_DETECTED);
                    }
                }
            }
#endif
            return SUCCESS;
        }

        public int CheckPushPullPosition(out int curPos, int firstCheckPos = (int)EPushPullPos.NONE)
        {
            int iResult = SUCCESS;

            // Init
            curPos = (int)EPushPullPos.NONE;

            // 1. check first position
            if (firstCheckPos != (int)EPushPullPos.NONE)
            {
                bool bResult;
                iResult = m_RefComp.PushPull.ComparePushPullPos(firstCheckPos, out bResult, false, false);
                if (iResult != SUCCESS) return iResult;
                if (bResult)
                {
                    curPos = firstCheckPos;
                    return SUCCESS;
                }
            }

            // Get Position Info
            iResult = m_RefComp.PushPull.GetPushPullPosInfo(out curPos, true, false);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        private int CheckYAxMatchZone(int curPos, int curZone_Y)
        {
            //switch (curPos)
            //{
            //    case (int)EPushPullPos.LOAD:
            //        if (curZone_Y != (int)EPushPullYAxZone.LOAD)
            //            return GenerateErrorCode(ERR_CTRL_PUSHPULL_YAX_POS_NOT_MATCH_ZONE);
            //        break;
            //    case (int)EPushPullPos.WAIT:
            //        if (curZone_Y != (int)EPushPullYAxZone.WAIT)
            //            return GenerateErrorCode(ERR_CTRL_PUSHPULL_YAX_POS_NOT_MATCH_ZONE);
            //        break;
            //    case (int)EPushPullPos.UNLOAD:
            //        if (curZone_Y != (int)EPushPullYAxZone.UNLOAD)
            //            return GenerateErrorCode(ERR_CTRL_PUSHPULL_YAX_POS_NOT_MATCH_ZONE);
            //        break;
            //}
            return SUCCESS;
        }

        public int CheckSafety_forPushPullMoving(int nTargetPos, bool bTransfer)
        {
            int iResult = SUCCESS;

            // 0. init
            int curPos = (int)EPushPullPos.NONE;

            // 0.1 check origin return
            bool bStatus;
            iResult = m_RefComp.PushPull.IsAllAxisOrignReturned(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return GenerateErrorCode(ERR_CTRL_PUSHPULL_NOT_ORIGIN_RETURNED);

            // 0.1 check vacuum
            iResult = CheckLock_forMove(bTransfer);
            if (iResult != SUCCESS) return iResult;

            // 1 check object exist
            iResult = IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;

#if !SIMULATION_TEST
            if (bTransfer == true && bStatus == false)
            {
                return GenerateErrorCode(ERR_CTRL_PUSHPULL_OBJECT_NOT_EXIST);
            }
            else if (bTransfer == false && bStatus == true)
            {
                return GenerateErrorCode(ERR_CTRL_PUSHPULL_OBJECT_DETECTED);
            }

            // 2. check safety
            // 2.1 check safety : handler down
            iResult = m_RefComp.UpperHandler.CheckHandlerSafetyForPushPull();
            if(iResult != SUCCESS ) return GenerateErrorCode(ERR_CTRL_PUSHPULL_UPPER_HANDLER_IS_NOT_UP);

            iResult = m_RefComp.LowerHandler.CheckHandlerSafetyForPushPull();
            if (iResult != SUCCESS) return GenerateErrorCode(ERR_CTRL_PUSHPULL_LOWER_HANDLER_IS_NOT_UP);

            // 2.2 check safety : spinner up
            iResult = m_RefComp.Spinner1.IsChuckTableDown(out bStatus);
            if (iResult != SUCCESS || bStatus == false)
                return GenerateErrorCode(ERR_CTRL_PUSHPULL_SPINNER1_IS_NOT_DOWN);

            iResult = m_RefComp.Spinner2.IsChuckTableDown(out bStatus);
            if (iResult != SUCCESS || bStatus == false)
                return GenerateErrorCode(ERR_CTRL_PUSHPULL_SPINNER2_IS_NOT_DOWN);

            // 2.3 check elevater : wafer를 가지고 이동한다면, elevator엔 wafer가 없어야 함
            iResult = m_RefComp.Elevator.CheckSafetyForPushPull(!bTransfer);
            if (iResult != SUCCESS) return GenerateErrorCode(ERR_CTRL_PUSHPULL_ELEVATOR_IS_NOT_SAFE);
#endif

            return SUCCESS;
        }

        public int CheckSafety_forCenterMoving(int nTargetPos)
        {
            int iResult = SUCCESS;

            // 0. init
            int curPos = (int)ECenterPos.NONE;

            // 1 check object exist
            bool bDetected = false;
            iResult = IsObjectDetected(out bDetected);
            if (iResult != SUCCESS) return iResult;

            // 2. object가 감지될때, center unit을 wait위치로 이동시킬경우엔 
            //    pushpull의 grip lock 혹은 handler, spinner등이 wafer를 받치고 있는지 체크해야하는데..우선은 나중으로.

            // 2. check other unit
            // 2.1 handler

            // 2.2 elevator

            // 2.3 coater



            return SUCCESS;
        }

        public int MoveToLoaderPos(bool bTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.LOADER, bTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToLoaderPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToWaitPos(bool bTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.WAIT, bTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToWaitPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToTempUnloadPos(bool bTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.TEMP_UNLOAD, bTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToTempUnloadPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToReloadPos(bool bTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.RELOAD, bTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToReloadPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToHandlerPos(bool bTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.HANDLER, bTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToHandlerPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToSpinner1Pos(bool bTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.SPINNER1, bTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToSpinner1Pos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToSpinner2Pos(bool bTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.SPINNER2, bTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToSpinner1Pos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveCenterToPos(ECenterIndex index, int iPos, double dXMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_X] = dXMoveOffset;

            int iResult = CheckSafety_forCenterMoving(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MoveCenterToPos(index, iPos, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GuideOpen(double dXMoveOffset = 0)
        {
            return MoveAllCenterUnitToWaitPos(dXMoveOffset);
        }

        public int GuideClose(double dXMoveOffset = 0)
        {
            return MoveAllCenterUnitToCenteringPos(dXMoveOffset);
        }

        public int MoveAllCenterUnitToWaitPos(double dXMoveOffset = 0)
        {
            int iPos = (int)ECenterPos.WAIT;

            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_X] = dXMoveOffset;

            int iResult = CheckSafety_forCenterMoving(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MoveAllCenterUnitToWaitPos(dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveAllCenterUnitToCenteringPos(double dXMoveOffset = 0)
        {
            int iPos = (int)ECenterPos.CENTERING;

            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_X] = dXMoveOffset;

            int iResult = CheckSafety_forCenterMoving(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MoveAllCenterUnitToCenteringPos(dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
    }
}
