using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_MePushPull;
using static LWDicer.Control.DEF_CtrlPushPull;

namespace LWDicer.Control
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

            return SUCCESS;
        }

        public int IsObjectDetected_Front(out bool bStatus)
        {
            int iResult = m_RefComp.PushPull.IsObjectDetected_Front(out bStatus);
            return iResult;

            return SUCCESS;
        }

        public int IsObjectDetected_Rear(out bool bStatus)
        {
            int iResult = m_RefComp.PushPull.IsObjectDetected_Rear(out bStatus);
            return iResult;

            return SUCCESS;
        }

        private int CheckLock_forMove(bool bPanelTransfer, bool bCheckAutoRun = false)
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
            if (bCheckAutoRun == true) return SUCCESS;

            // check object exist when auto run
            if (AutoManualMode == EAutoManual.AUTO)
            {
                if (AutoRunMode != EAutoRunMode.DRY_RUN) // not dry run
                {
                    if (bDetected != bPanelTransfer)
                    {
                        if (bPanelTransfer)    // Panel이 있어야 할 상황일경우
                        {
                            WriteLog("CtrlPushPull의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                            return GenerateErrorCode(ERR_CTRLPUSHPULL_OBJECT_NOT_EXIST);
                        }
                        else
                        {
                            WriteLog("CtrlPushPull의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.Error);
                            return GenerateErrorCode(ERR_CTRLPUSHPULL_OBJECT_EXIST);
                        }
                    }
                }
                else // dry run
                {
                    if (bDetected || bAbsorbed)
                    {
                        WriteLog("CtrlPushPull의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.Error);
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

        public int CheckSafety_forMoving(int nTargetPos, bool bPanelTransfer)
        {
            int iResult = SUCCESS;

            // 0. init
            int curPos = (int)EPushPullPos.NONE;

            // 0.1 check vacuum
            iResult = CheckLock_forMove(bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            // 1 check object exist
            bool bObjectExist = false;
            iResult = IsObjectDetected(out bObjectExist);
            if (iResult != SUCCESS) return iResult;

            if (bPanelTransfer == true && bObjectExist == false)
            {
                return GenerateErrorCode(ERR_CTRLPUSHPULL_OBJECT_NOT_EXIST);
            }
            else if (bPanelTransfer == false && bObjectExist == true)
            {
                return GenerateErrorCode(ERR_CTRLPUSHPULL_OBJECT_EXIST);
            }

            // 2. check other unit
            // 2.1 handler

            // 2.2 elevator

            // 2.3 coater



            return SUCCESS;
        }

        public int MoveToLoadPos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forMoving((int)EPushPullPos.LOAD, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToLoadPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToWaitPos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forMoving((int)EPushPullPos.WAIT, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToWaitPos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToUnload1Pos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forMoving((int)EPushPullPos.UNLOAD1, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToUnload1Pos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToUnload2Pos(bool bPanelTransfer, double dYMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Y] = dYMoveOffset;

            int iResult = CheckSafety_forMoving((int)EPushPullPos.UNLOAD2, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.PushPull.MovePushPullToUnload1Pos(true, false, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

    }
}
