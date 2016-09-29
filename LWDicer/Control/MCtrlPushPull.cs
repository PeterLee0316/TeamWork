﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Motion;
using static LWDicer.Layers.DEF_MePushPull;
using static LWDicer.Layers.DEF_CtrlPushPull;

namespace LWDicer.Layers
{
    public class DEF_CtrlPushPull
    {
        public const int ERR_CTRLPUSHPULL_UNABLE_TO_USE_PUSHPULL = 1;
        public const int ERR_CTRLPUSHPULL_UNABLE_TO_USE_LOG = 2;
        public const int ERR_CTRLPUSHPULL_OBJECT_ABSORBED = 3;
        public const int ERR_CTRLPUSHPULL_OBJECT_NOT_ABSORBED = 4;
        public const int ERR_CTRLPUSHPULL_OBJECT_EXIST = 5;
        public const int ERR_CTRLPUSHPULL_OBJECT_NOT_EXIST = 6;
        public const int ERR_CTRLPUSHPULL_CHECK_RUN_BEFORE_FAILED = 7;
        public const int ERR_CTRLPUSHPULL_CYLINDER_TIMEOUT = 8;
        public const int ERR_CTRLPUSHPULL_NOT_UP = 9;
        public const int ERR_CTRLPUSHPULL_CANNOT_DETECT_POSINFO = 10;
        public const int ERR_CTRLPUSHPULL_PCB_DOOR_OPEN = 11;
        public const int ERR_CTRLPUSHPULL_UPUSHPULL_IN_DOWN_AND_LPUSHPULL_IN_SAME_XZONE = 12;
        public const int ERR_CTRLPUSHPULL_UPUSHPULL_NEED_DOWN_AND_LPUSHPULL_IN_SAME_XZONE = 13;
        public const int ERR_CTRLPUSHPULL_LPUSHPULL_NEED_MOVE_AND_UPUSHPULL_IN_DOWN = 14;
        public const int ERR_CTRLPUSHPULL_YAX_POS_NOT_MATCH_ZONE = 15;

        public class CCtrlPushPullRefComp
        {
            public IIO IO;
            public MMePushPull PushPull;
            public MMeHandler UpperHandler;
            public MMeHandler LowerHandler;

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
            int iResult;
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

        private int CheckLock_forMove(bool bPanelTransfer, bool bCheck_WhenAutoRun = false)
        {
            int iResult = SUCCESS;
            bool bDetected, bAbsorbed;

            // check vacuum
            iResult = IsObjectDetected(out bDetected);
            if (iResult != SUCCESS) return iResult;

            iResult = IsGripLocked(out bAbsorbed);
            if (iResult != SUCCESS) return iResult;

            if (bPanelTransfer)
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

            // check object exist when auto run
            if (AutoManualMode == EAutoManual.AUTO)
            {
                if (AutoRunMode != EAutoRunMode.DRY_RUN) // not dry run
                {
                    if (bDetected != bPanelTransfer)
                    {
                        if (bPanelTransfer)    // Panel이 있어야 할 상황일경우
                        {
                            WriteLog("CtrlPushPull의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT NOT EXIST", ELogType.Debug, ELogWType.D_Error);
                            return GenerateErrorCode(ERR_CTRLPUSHPULL_OBJECT_NOT_EXIST);
                        }
                        else
                        {
                            WriteLog("CtrlPushPull의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.D_Error);
                            return GenerateErrorCode(ERR_CTRLPUSHPULL_OBJECT_EXIST);
                        }
                    }
                }
                else // dry run
                {
                    if (bDetected || bAbsorbed)
                    {
                        WriteLog("CtrlPushPull의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.D_Error);
                        return GenerateErrorCode(ERR_CTRLPUSHPULL_OBJECT_EXIST);
                    }
                }
            }

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
            //            return GenerateErrorCode(ERR_CTRLPUSHPULL_YAX_POS_NOT_MATCH_ZONE);
            //        break;
            //    case (int)EPushPullPos.WAIT:
            //        if (curZone_Y != (int)EPushPullYAxZone.WAIT)
            //            return GenerateErrorCode(ERR_CTRLPUSHPULL_YAX_POS_NOT_MATCH_ZONE);
            //        break;
            //    case (int)EPushPullPos.UNLOAD:
            //        if (curZone_Y != (int)EPushPullYAxZone.UNLOAD)
            //            return GenerateErrorCode(ERR_CTRLPUSHPULL_YAX_POS_NOT_MATCH_ZONE);
            //        break;
            //}
            return SUCCESS;
        }

        public int CheckSafety_forPushPullMoving(int nTargetPos, bool bPanelTransfer)
        {
            int iResult = SUCCESS;

            // 0. init
            int curPos = (int)EPushPullPos.NONE;

            // 0.1 check vacuum
            iResult = CheckLock_forMove(bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            // 1 check object exist
            bool bDetected = false;
            iResult = IsObjectDetected(out bDetected);
            if (iResult != SUCCESS) return iResult;

#if !SIMULATION_TEST
            if (bPanelTransfer == true && bDetected == false)
            {
                return GenerateErrorCode(ERR_CTRLPUSHPULL_OBJECT_NOT_EXIST);
            }
            else if (bPanelTransfer == false && bDetected == true)
            {
                return GenerateErrorCode(ERR_CTRLPUSHPULL_OBJECT_EXIST);
            }
#endif

            // 2. check other unit
            // 2.0 이동할 반대편 위치 (예를들어 spinner1 -> spinner2 구간으로 이동할때)

            // 2.1 handler

            // 2.2 elevator

            // 2.3 coater



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

        public int MoveToLoaderPos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.LOADER, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToLoaderPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToWaitPos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.WAIT, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToWaitPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToTempUnloadPos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.TEMP_UNLOAD, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToTempUnloadPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToReloadPos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.RELOAD, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToReloadPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToHandlerPos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.HANDLER, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToHandlerPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToSpinner1Pos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.SPINNER1, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToSpinner1Pos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToSpinner2Pos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forPushPullMoving((int)EPushPullPos.SPINNER2, bPanelTransfer);
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
