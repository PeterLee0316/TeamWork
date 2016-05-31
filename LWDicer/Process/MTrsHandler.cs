using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.ETrsHandlerStep;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.EThreadChannel;
using static LWDicer.Control.DEF_Thread.EAutoRunMode;
using static LWDicer.Control.DEF_Thread.EAutoRunStatus;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_CtrlHandler;

namespace LWDicer.Control
{
    public class CTrsHandlerRefComp
    {
        public MCtrlHandler ctrlHandler;

        public override string ToString()
        {
            return $"CTrsHandlerRefComp : {this}";
        }
    }

    public class CTrsHandlerData
    {
    }

    public class MTrsHandler : MWorkerThread
    {
        private CTrsHandlerRefComp m_RefComp;
        private CTrsHandlerData m_Data;

        // Message 변수
        bool m_bPushPull_RequestLoading;
        bool m_bPushPull_StartUnloading;
        bool m_bPushPull_ReleaseComplete;
        bool m_bPushPull_CompleteUnloading;

        bool m_bPushPull_RequestUnloading;
        bool m_bPushPull_StartLoading;
        bool m_bPushPull_AbsorbComplete;
        bool m_bPushPull_CompleteLoading;

        bool m_bStage1_RequestUnloading;
        bool m_bStage1_StartLoading;
        bool m_bStage1_AbsorbComplete;
        bool m_bStage1_CompleteLoading;

        bool m_bStage1_RequestLoading;
        bool m_bStage1_StartUnloading;
        bool m_bStage1_ReleaseComplete;
        bool m_bStage1_CompleteUnloading;

        public MTrsHandler(CObjectInfo objInfo, EThreadChannel SelfChannelNo,
            CTrsHandlerRefComp refComp, CTrsHandlerData data)
             : base(objInfo, SelfChannelNo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CTrsHandlerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsHandlerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public override int Initialize()
        {
            // Do initialize
            InitializeMsg();
            InitializeInterface();

            // Do Action
            int iResult = m_RefComp.ctrlHandler.Initialize();
            if (iResult != SUCCESS) return iResult;

            int iStep1, iStep2;
            bool bStatus;
            iResult = m_RefComp.ctrlHandler.IsObjectDetected(EHandlerIndex.LOAD_UPPER, out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus) iStep1 = (int)TRS_LHANDLER_WAIT_MOVETO_UNLOADING;
            else iStep1 = (int)TRS_LHANDLER_MOVETO_WAIT1;

            iResult = m_RefComp.ctrlHandler.IsObjectDetected(EHandlerIndex.UNLOAD_LOWER, out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus) iStep2 = (int)TRS_UHANDLER_WAIT_MOVETO_UNLOADING;
            else iStep2 = (int)TRS_UHANDLER_MOVETO_WAIT1;

            // finally
            SetStep1(iStep1);
            SetStep2(iStep2);

            return SUCCESS;
        }

        public int InitializeMsg()
        {
            m_bPushPull_RequestLoading = false;
            m_bPushPull_StartUnloading = false;
            m_bPushPull_ReleaseComplete = false;
            m_bPushPull_CompleteUnloading = false;

            m_bPushPull_RequestUnloading = false;
            m_bPushPull_StartLoading = false;
            m_bPushPull_AbsorbComplete = false;
            m_bPushPull_CompleteLoading = false;

            m_bStage1_RequestUnloading = false;
            m_bStage1_StartLoading = false;
            m_bStage1_AbsorbComplete = false;
            m_bStage1_CompleteLoading = false;

            m_bStage1_RequestLoading = false;
            m_bStage1_StartUnloading = false;
            m_bStage1_ReleaseComplete = false;
            m_bStage1_CompleteUnloading = false;

            return SUCCESS;
        }

        private int InitializeInterface()
        {

            return SUCCESS;
        }
        #endregion

        protected override int ProcessMsg(MEvent evnt)
        {
            Debug.WriteLine($"{ToString()} received message : {evnt}");
            switch (evnt.Msg)
            {
                // if need to change response for common message, then add case state here.
                default:
                    base.ProcessMsg(evnt);
                    break;

                    // with PushPull
                case (int)MSG_PUSHPULL_LHANDLER_REQUEST_LOADING:
                    m_bPushPull_RequestLoading = true;
                    m_bPushPull_StartUnloading = false;
                    m_bPushPull_ReleaseComplete = false;
                    m_bPushPull_CompleteUnloading = false;
                    break;

                case (int)MSG_PUSHPULL_LHANDLER_RELEASE_COMPLETE:
                    m_bPushPull_RequestLoading = false;
                    m_bPushPull_StartUnloading = false;
                    m_bPushPull_ReleaseComplete = true;
                    m_bPushPull_CompleteUnloading = false;
                    break;

                case (int)MSG_PUSHPULL_UHANDLER_REQUEST_UNLOADING:
                    m_bPushPull_RequestUnloading = true;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_AbsorbComplete = false;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_PUSHPULL_UHANDLER_ABSORB_COMPLETE:
                    m_bPushPull_RequestUnloading = false;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_AbsorbComplete = true;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_STAGE1_LHANDLER_REQUEST_UNLOADING:
                    m_bStage1_RequestUnloading = true;
                    m_bStage1_StartLoading = false;
                    m_bStage1_AbsorbComplete = false;
                    m_bStage1_CompleteLoading = false;
                    break;

                case (int)MSG_STAGE1_LHANDLER_ABSORB_COMPLETE:
                    m_bStage1_RequestUnloading = false;
                    m_bStage1_StartLoading = false;
                    m_bStage1_AbsorbComplete = true;
                    m_bStage1_CompleteLoading = false;
                    break;

                case (int)MSG_STAGE1_UHANDLER_REQUEST_LOADING:
                    m_bStage1_RequestLoading = true;
                    m_bStage1_StartUnloading = false;
                    m_bStage1_ReleaseComplete = false;
                    m_bStage1_CompleteUnloading = false;
                    break;

                case (int)MSG_STAGE1_UHANDLER_RELEASE_COMPLETE:
                    m_bStage1_RequestLoading = false;
                    m_bStage1_StartUnloading = false;
                    m_bStage1_ReleaseComplete = true;
                    m_bStage1_CompleteUnloading = false;
                    break;

            }
            return DEF_Error.SUCCESS;
        }

        protected override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bStatus = false;
            EHandlerIndex index;

            bool bManualSts = false;
            bool bErrorSts = false;
            bool bStepStopSts = false;
            bool bSignalWaitErr = false;

            while (true)
            {
                // if thread has been suspended
                if (IsAlive == false)
                {
                    Sleep(ThreadSuspendedTime);
                    continue;
                }

                // check message from other thread
                CheckMsg(1);

                switch (RunStatus)
                {
                    case STS_MANUAL: // Manual Mode
                        m_RefComp.ctrlHandler.SetAutoManual(EAutoManual.MANUAL);
                        break;

                    case STS_ERROR_STOP: // Error Stop
                        break;

                    case STS_STEP_STOP: // Step Stop
                        break;

                    case STS_RUN_READY: // Run Ready
                        break;

                    case STS_CYCLE_STOP: // Cycle Stop
                        //// Cycle Stop 조건 확인은 추후에 다시 필요함
                        //if (ThreadStep1 == (int)TRS_LHANDLER_WAIT_MOVETO_LOADING
                        //    && ThreadStep2 == (int)TRS_UHANDLER_WAIT_MOVETO_LOADING)
                        //{
                        //    break;
                        //}
                        break;

                    case STS_RUN: // auto run
                        m_RefComp.ctrlHandler.SetAutoManual(EAutoManual.AUTO);

                        // Do Thread Step : Load Upper Handler
                        iResult = DoDetailProcess1();
                        if (iResult != SUCCESS) break;

                        // Do Thread Step : Unload Lower Handler
                        iResult = DoDetailProcess2();
                        if (iResult != SUCCESS) break;

                        break;

                    default:
                        break;
                }

                Sleep(ThreadSleepTime);
                //Debug.WriteLine(ToString() + " Thread running..");
            }

        }

        private int DoDetailProcess1()
        {
            int iResult = SUCCESS;
            bool bStatus;
            // Do Thread Step : Load Upper Handler
            EHandlerIndex index = EHandlerIndex.LOAD_UPPER;
            switch (ThreadStep1)
            {
                case (int)TRS_LHANDLER_MOVETO_WAIT1:
                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsPushPull, MSG_LHANDLER_PUSHPULL_WAIT_LOADING_START);
                    SetStep1((int)TRS_LHANDLER_WAIT_MOVETO_LOADING);
                    break;

                case (int)TRS_LHANDLER_WAIT_MOVETO_LOADING:
                    PostMsg_Interval(TrsPushPull, MSG_LHANDLER_PUSHPULL_WAIT_LOADING_START);
                    if (m_bPushPull_RequestLoading == false) break;
                    PostMsg(TrsPushPull, MSG_LHANDLER_PUSHPULL_START_LOADING);

                    SetStep1((int)TRS_LHANDLER_LOADING);
                    break;

                //case (int)TRS_LHANDLER_MOVETO_LOAD_POS:
                //    SetStep1((int)TRS_LHANDLER_LOADING);
                //    break;

                case (int)TRS_LHANDLER_LOADING:
                    iResult = m_RefComp.ctrlHandler.MoveToPushPullPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    iResult = m_RefComp.ctrlHandler.Absorb(index);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsPushPull, MSG_LHANDLER_PUSHPULL_REQUEST_RELEASE);
                    SetStep1((int)TRS_LHANDLER_WAITFOR_PUSHPULL_UNLOAD_COMPLETE);
                    break;

                case (int)TRS_LHANDLER_WAITFOR_PUSHPULL_UNLOAD_COMPLETE:
                    PostMsg_Interval(TrsPushPull, MSG_LHANDLER_PUSHPULL_REQUEST_RELEASE);
                    if (m_bPushPull_StartUnloading == false) break;

                    SetStep1((int)TRS_LHANDLER_MOVETO_WAIT2);
                    break;

                //case (int)TRS_LHANDLER_MOVETO_LOAD_UP_POS:
                //    SetStep1((int)TRS_LHANDLER_MOVETO_WAIT2);
                //    break;

                case (int)TRS_LHANDLER_MOVETO_WAIT2:
                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, true);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsPushPull, MSG_LHANDLER_PUSHPULL_COMPLETE_LOADING);

                    SetStep1((int)TRS_LHANDLER_WAIT_MOVETO_UNLOADING);
                    break;

                case (int)TRS_LHANDLER_WAIT_MOVETO_UNLOADING:
                    PostMsg_Interval(TrsPushPull, MSG_LHANDLER_STAGE1_WAIT_UNLOADING_START);
                    if (m_bStage1_RequestUnloading == false) break;
                    PostMsg(TrsStage1, MSG_LHANDLER_STAGE1_START_UNLOADING);

                    SetStep1((int)TRS_LHANDLER_MOVETO_UNLOAD_POS);
                    break;

                case (int)TRS_LHANDLER_MOVETO_UNLOAD_POS:
                    iResult = m_RefComp.ctrlHandler.MoveToStagePos(index, true);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsStage1, MSG_LHANDLER_STAGE1_REQUEST_ABSORB);

                    SetStep1((int)TRS_LHANDLER_REQUEST_STAGE_LOADING);
                    break;

                case (int)TRS_LHANDLER_REQUEST_STAGE_LOADING:
                    PostMsg_Interval(TrsStage1, MSG_LHANDLER_STAGE1_REQUEST_ABSORB);
                    if (m_bStage1_StartLoading == false) break;

                    SetStep1((int)TRS_LHANDLER_UNLOADING);
                    break;

                case (int)TRS_LHANDLER_UNLOADING:
                    iResult = m_RefComp.ctrlHandler.Release(index);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsStage1, MSG_LHANDLER_STAGE1_COMPLETE_UNLOADING);
                    //SetStep1((int)TRS_LHANDLER_MOVETO_WAIT1);
                    SetStep1((int)TRS_LHANDLER_WAIT_MOVETO_LOADING);
                    break;

                default:
                    break;
            }

            return SUCCESS;
        }

        private int DoDetailProcess2()
        {
            int iResult = SUCCESS;
            bool bStatus;
            // Do Thread Step : Unload Lower Handler
            EHandlerIndex index = EHandlerIndex.UNLOAD_LOWER;
            switch (ThreadStep1)
            {
                case (int)TRS_UHANDLER_MOVETO_WAIT1:
                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsStage1, MSG_UHANDLER_STAGE1_WAIT_LOADING_START);

                    SetStep1((int)TRS_UHANDLER_WAIT_MOVETO_LOADING);
                    break;

                case (int)TRS_UHANDLER_WAIT_MOVETO_LOADING:
                    PostMsg_Interval(TrsStage1, MSG_UHANDLER_STAGE1_WAIT_LOADING_START);
                    if (m_bStage1_RequestLoading == false) break;
                    PostMsg(TrsStage1, MSG_UHANDLER_STAGE1_START_LOADING);

                    SetStep1((int)TRS_UHANDLER_MOVETO_LOAD_POS);
                    break;

                //case (int)TRS_UHANDLER_MOVETO_UNLOAD_POS:
                //    SetStep1((int)TRS_UHANDLER_UNLOADING);
                //    break;

                case (int)TRS_UHANDLER_MOVETO_LOAD_POS:
                    iResult = m_RefComp.ctrlHandler.MoveToStagePos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    iResult = m_RefComp.ctrlHandler.Absorb(index);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsStage1, MSG_UHANDLER_STAGE1_REQUEST_RELEASE);
                    SetStep1((int)TRS_UHANDLER_LOADING);
                    break;

                case (int)TRS_UHANDLER_LOADING:
                    PostMsg_Interval(TrsStage1, MSG_UHANDLER_STAGE1_REQUEST_RELEASE);
                    if (m_bStage1_CompleteUnloading == false) break;

                    SetStep1((int)TRS_UHANDLER_MOVETO_WAIT2);
                    break;

                //case (int)TRS_UHANDLER_MOVETO_UNLOAD_UP_POS:
                //    SetStep1((int)TRS_UHANDLER_MOVETO_WAIT2);
                //    break;

                case (int)TRS_UHANDLER_MOVETO_WAIT2:
                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, true);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsStage1, MSG_UHANDLER_STAGE1_COMPLETE_LOADING);

                    SetStep1((int)TRS_UHANDLER_WAIT_MOVETO_UNLOADING);
                    break;

                case (int)TRS_UHANDLER_WAIT_MOVETO_UNLOADING:
                    PostMsg_Interval(TrsPushPull, MSG_UHANDLER_PUSHPULL_WAIT_UNLOADING_START);
                    if (m_bPushPull_RequestUnloading == false) break;
                    PostMsg(TrsPushPull, MSG_UHANDLER_PUSHPULL_START_UNLOADING);

                    SetStep1((int)TRS_UHANDLER_MOVETO_UNLOAD_POS);
                    break;

                case (int)TRS_UHANDLER_MOVETO_UNLOAD_POS:
                    iResult = m_RefComp.ctrlHandler.MoveToPushPullPos(index, true);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsPushPull, MSG_UHANDLER_PUSHPULL_REQUEST_ABSORB);

                    SetStep1((int)TRS_UHANDLER_WAITFOR_PUSHPULL_LOAD_COMPLETE);
                    break;

                case (int)TRS_UHANDLER_WAITFOR_PUSHPULL_LOAD_COMPLETE:
                    PostMsg_Interval(TrsPushPull, MSG_UHANDLER_PUSHPULL_REQUEST_ABSORB);
                    if (m_bPushPull_CompleteLoading == false) break;

                    SetStep1((int)TRS_UHANDLER_UNLOADING);
                    break;

                case (int)TRS_UHANDLER_UNLOADING:
                    iResult = m_RefComp.ctrlHandler.Release(index);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    PostMsg(TrsPushPull, MSG_UHANDLER_PUSHPULL_COMPLETE_UNLOADING);
                    //SetStep1((int)TRS_UHANDLER_MOVETO_WAIT1);
                    SetStep1((int)TRS_UHANDLER_WAIT_MOVETO_LOADING);
                    break;

                default:
                    break;
            }

            return SUCCESS;
        }
    }
}
