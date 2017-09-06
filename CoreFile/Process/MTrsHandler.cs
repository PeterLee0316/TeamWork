using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Thread.EThreadStep;
using static Core.Layers.DEF_Thread.EThreadMessage;
using static Core.Layers.DEF_Thread.EThreadChannel;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_LCNet;
using static Core.Layers.DEF_CtrlHandler;

namespace Core.Layers
{
    public class CTrsHandlerRefComp
    {
        public MCtrlHandler ctrlHandler;
        public MCtrlPushPull ctrlPushPull;

        public override string ToString()
        {
            return $"CTrsHandlerRefComp : {this}";
        }
    }

    public class CTrsHandlerData
    {
        public bool ThreadHandshake_byOneStep;
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

        public MTrsHandler(CObjectInfo objInfo, EThreadChannel SelfChannelNo, MDataManager DataManager, ELCNetUnitPos LCNetUnitPos,
            CTrsHandlerRefComp refComp, CTrsHandlerData data)
             : base(objInfo, SelfChannelNo, DataManager, LCNetUnitPos)
        {
            m_RefComp = refComp;
            SetData(data);
            TSelf = (int)EThreadUnit.HANDLER;
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

            EThreadStep iStep1, iStep2;
            bool bStatus;
            iResult = m_RefComp.ctrlHandler.IsObjectDetected(EHandlerIndex.LOAD_UPPER, out bStatus);
            if (iResult != SUCCESS) return iResult;
            iStep1 = TRS_UPPER_HANDLER_WAITFOR_MESSAGE;

            iResult = m_RefComp.ctrlHandler.IsObjectDetected(EHandlerIndex.UNLOAD_LOWER, out bStatus);
            if (iResult != SUCCESS) return iResult;
            iStep2 = TRS_LOWER_HANDLER_WAITFOR_MESSAGE;

            // finally
            SetStep1(iStep1);
            SetStep2(iStep2);

            return base.Initialize();
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
              Debug.WriteLine($"[{ToString()}] received message : {evnt.ToThreadMessage()}");
            switch (evnt.Msg)
            {
                // if need to change response for common message, then add case state here.
                default:
                    base.ProcessMsg(evnt);
                    break;

                    //////////////////////////////////////////////////////////////////////////////
                    // with PushPull
                    case (int)MSG_PUSHPULL_UPPER_HANDLER_REQUEST_LOADING:         // wafer : P -> H
                    m_bPushPull_RequestLoading = true;
                    m_bPushPull_StartUnloading = false;
                    m_bPushPull_ReleaseComplete = false;
                    m_bPushPull_CompleteUnloading = false;
                    break;

                case (int)MSG_PUSHPULL_UPPER_HANDLER_START_UNLOADING:         // wafer : P -> H
                    m_bPushPull_RequestLoading = false;
                    m_bPushPull_StartUnloading = true;
                    m_bPushPull_ReleaseComplete = false;
                    m_bPushPull_CompleteUnloading = false;
                    break;

                case (int)MSG_PUSHPULL_UPPER_HANDLER_RELEASE_COMPLETE:        // wafer : P -> H
                    m_bPushPull_RequestLoading = false;
                    m_bPushPull_StartUnloading = false;
                    m_bPushPull_ReleaseComplete = true;
                    m_bPushPull_CompleteUnloading = false;
                    break;

                case (int)MSG_PUSHPULL_UPPER_HANDLER_COMPLETE_UNLOADING:      // wafer : P -> H
                    m_bPushPull_RequestLoading = false;
                    m_bPushPull_StartUnloading = false;
                    m_bPushPull_ReleaseComplete = false;
                    m_bPushPull_CompleteUnloading = true;
                    break;

                case (int)MSG_PUSHPULL_LOWER_HANDLER_REQUEST_UNLOADING:       // wafer : H -> P
                    m_bPushPull_RequestUnloading = true;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_AbsorbComplete = false;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_PUSHPULL_LOWER_HANDLER_START_LOADING:           // wafer : H -> P
                    m_bPushPull_RequestUnloading = false;
                    m_bPushPull_StartLoading = true;
                    m_bPushPull_AbsorbComplete = false;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_PUSHPULL_LOWER_HANDLER_ABSORB_COMPLETE:         // wafer : H -> P
                    m_bPushPull_RequestUnloading = false;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_AbsorbComplete = true;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_PUSHPULL_LOWER_HANDLER_COMPLETE_LOADING:        // wafer : H -> P
                    m_bPushPull_RequestUnloading = false;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_AbsorbComplete = false;
                    m_bPushPull_CompleteLoading = true;
                    break;

                //////////////////////////////////////////////////////////////////////////////
                // with stage
                case (int)MSG_STAGE1_LOWER_HANDLER_REQUEST_UNLOADING:
                    m_bStage1_RequestUnloading = true;
                    m_bStage1_StartLoading = false;
                    m_bStage1_AbsorbComplete = false;
                    m_bStage1_CompleteLoading = false;
                    break;

                case (int)MSG_STAGE1_LOWER_HANDLER_ABSORB_COMPLETE:
                    m_bStage1_RequestUnloading = false;
                    m_bStage1_StartLoading = false;
                    m_bStage1_AbsorbComplete = true;
                    m_bStage1_CompleteLoading = false;
                    break;

                case (int)MSG_STAGE1_UPPER_HANDLER_REQUEST_LOADING:
                    m_bStage1_RequestLoading = true;
                    m_bStage1_StartUnloading = false;
                    m_bStage1_ReleaseComplete = false;
                    m_bStage1_CompleteUnloading = false;
                    break;

                case (int)MSG_STAGE1_UPPER_HANDLER_RELEASE_COMPLETE:
                    m_bStage1_RequestLoading = false;
                    m_bStage1_StartUnloading = false;
                    m_bStage1_ReleaseComplete = true;
                    m_bStage1_CompleteUnloading = false;
                    break;
            }

            return SUCCESS;
        }

        protected override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bStatus, bStatus1, bStatus2;
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
                    case EAutoRunStatus.STS_MANUAL: // Manual Mode
                        break;

                    case EAutoRunStatus.STS_ERROR_STOP: // Error Stop
                        break;

                    case EAutoRunStatus.STS_STEP_STOP: // Step Stop
                        break;

                    case EAutoRunStatus.STS_RUN_READY: // Run Ready
                        break;

                    case EAutoRunStatus.STS_CYCLE_STOP: // Cycle Stop
                        //// Cycle Stop 조건 확인은 추후에 다시 필요함
                        //if (ThreadStep1 == (int)TRS_LOWER_HANDLER_WAIT_MOVETO_LOADING
                        //    && ThreadStep2 == (int)TRS_UPPER_HANDLER_WAIT_MOVETO_LOADING)
                        //{
                        //    break;
                        //}
                        break;

                    case EAutoRunStatus.STS_RUN: // auto run

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
            bool bStatus, bStatus1, bStatus2;
            EProcessPhase tPhase = GetWorkPiece(ELCNetUnitPos.UPPER_HANDLER).GetNextPhase();

            // Do Thread Step : Load Upper Handler
            EHandlerIndex index = EHandlerIndex.LOAD_UPPER;
            switch (ThreadStep1)
            {
                case TRS_UPPER_HANDLER_MOVETO_WAIT1:
                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    SetStep1(TRS_UPPER_HANDLER_WAITFOR_MESSAGE);
                    break;

                case TRS_UPPER_HANDLER_WAITFOR_MESSAGE:
                    iResult = m_RefComp.ctrlHandler.IsObjectDetected(index, out bStatus);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    if(bStatus == true)
                    {
                        if(GetWorkPiece(ELCNetUnitPos.UPPER_HANDLER).Process[(int)EProcessPhase.UPPER_HANDLER_WAIT_UNLOAD].IsStarted == false)
                        {
                            GetWorkPiece(ELCNetUnitPos.UPPER_HANDLER).StartPhase(EProcessPhase.UPPER_HANDLER_WAIT_UNLOAD);
                        }

                        if(TInterface.Stage1_Handler_BeginHandshake_Load == true)
                        {
                            GetWorkPiece(ELCNetUnitPos.UPPER_HANDLER).FinishPhase(EProcessPhase.UPPER_HANDLER_WAIT_UNLOAD);
                            GetWorkPiece(ELCNetUnitPos.UPPER_HANDLER).StartPhase(EProcessPhase.UPPER_HANDLER_UNLOAD);
                            SetStep1(TRS_UPPER_HANDLER_UNLOADING_TO_STAGE_ONESTEP);
                        }
                    }
                    else if(bStatus == false && TInterface.PushPull_Handler_BeginHandshake_Unload == true)
                    {
                        SetStep1(TRS_UPPER_HANDLER_LOADING_FROM_PUSHPULL_ONESTEP);
                    }
                    break;

                ///////////////////////////////////////////////////////////////////
                // process load with pushpull
                case TRS_UPPER_HANDLER_LOADING_FROM_PUSHPULL_ONESTEP:
                    // branch
                    if (m_Data.ThreadHandshake_byOneStep == false)
                    {
                        SetStep1(TRS_UPPER_HANDLER_MOVETO_LOAD_POS);
                        break;
                    }

                    // init
                    TInterface.ResetInterface(TSelf);
                    TOpponent = (int)EThreadUnit.PUSHPULL;
                    if (TInterface.ErrorOccured[TOpponent]) break;

                    // begin
                    TTimer.StartTimer();
                    TInterface.Handler_PushPull_BeginHandshake_Load = true;

                    // step1
                    while (TInterface.PushPull_Handler_UnloadStep1 == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    iResult = m_RefComp.ctrlHandler.MoveToPushPullPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                    iResult = m_RefComp.ctrlHandler.Absorb(index);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    TInterface.Handler_PushPull_LoadStep1 = true;

                    // step2
                    while (TInterface.PushPull_Handler_UnloadStep2 == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, true);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                    TInterface.Handler_PushPull_LoadStep2 = true;

                    // finish
                    while (TInterface.PushPull_Handler_FinishHandshake_Unload == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    TInterface.Handler_PushPull_FinishHandshake_Load = true;
                    Sleep(TInterface.TimeKeepOn);

                    // reset
                    TInterface.ResetInterface(TSelf);
                    SetStep1(TRS_UPPER_HANDLER_WAITFOR_MESSAGE);
                    break;

                ///////////////////////////////////////////////////////////////////
                // process unload with stage
                case TRS_UPPER_HANDLER_UNLOADING_TO_STAGE_ONESTEP:
                    // branch
                    if (m_Data.ThreadHandshake_byOneStep == false)
                    {
                        SetStep1(TRS_UPPER_HANDLER_MOVETO_UNLOAD_POS);
                        break;
                    }

                    // init
                    TInterface.ResetInterface(TSelf);
                    TOpponent = (int)EThreadUnit.STAGE1;
                    if (TInterface.ErrorOccured[TOpponent]) break;

                    // begin
                    TTimer.StartTimer();
                    TInterface.Handler_Stage1_BeginHandshake_Unload = true;

                    // step1
                    while (TInterface.Stage1_Handler_LoadStep1 == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    iResult = m_RefComp.ctrlHandler.MoveToStagePos(index, true);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    TInterface.Handler_Stage1_UnloadStep1 = true;

                    // step2
                    while (TInterface.Stage1_Handler_LoadStep2 == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    iResult = m_RefComp.ctrlHandler.Release(index);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                    TInterface.Handler_Stage1_UnloadStep2 = true;

                    // finish
                    while (TInterface.Stage1_Handler_FinishHandshake_Load == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    GetWorkPiece(ELCNetUnitPos.UPPER_HANDLER).FinishPhase(EProcessPhase.UPPER_HANDLER_UNLOAD);
                    TInterface.Handler_Stage1_FinishHandshake_Unload = true;
                    Sleep(TInterface.TimeKeepOn);

                    // reset
                    TInterface.ResetInterface(TSelf);
                    SetStep1(TRS_UPPER_HANDLER_WAITFOR_MESSAGE);
                    break;

                default:
                    break;
            }

            return SUCCESS;
        }

        private int DoDetailProcess2()
        {
            int iResult = SUCCESS;
            bool bStatus, bStatus1, bStatus2;
            EProcessPhase tPhase = GetWorkPiece(ELCNetUnitPos.LOWER_HANDLER).GetNextPhase();

            // Do Thread Step : Unload Lower Handler
            EHandlerIndex index = EHandlerIndex.UNLOAD_LOWER;
            switch (ThreadStep2)
            {
                case TRS_LOWER_HANDLER_MOVETO_WAIT1:
                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    SetStep2(TRS_LOWER_HANDLER_WAITFOR_MESSAGE);
                    break;

                case TRS_LOWER_HANDLER_WAITFOR_MESSAGE:
                    iResult = m_RefComp.ctrlHandler.IsObjectDetected(index, out bStatus);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    if (bStatus == true)
                    {
                        if (GetWorkPiece(ELCNetUnitPos.LOWER_HANDLER).Process[(int)EProcessPhase.LOWER_HANDLER_WAIT_UNLOAD].IsStarted == false)
                        {
                            GetWorkPiece(ELCNetUnitPos.LOWER_HANDLER).StartPhase(EProcessPhase.LOWER_HANDLER_WAIT_UNLOAD);
                        }

                        if (TInterface.PushPull_Handler_BeginHandshake_Load == true)
                        {
                            GetWorkPiece(ELCNetUnitPos.LOWER_HANDLER).FinishPhase(EProcessPhase.LOWER_HANDLER_WAIT_UNLOAD);
                            GetWorkPiece(ELCNetUnitPos.LOWER_HANDLER).StartPhase(EProcessPhase.LOWER_HANDLER_UNLOAD);
                            SetStep2(TRS_LOWER_HANDLER_UNLOADING_TO_PUSHPULL_ONESTEP);
                        }
                    }
                    else if (bStatus == false && TInterface.Stage1_Handler_BeginHandshake_Unload == true)
                    {
                        SetStep2(TRS_LOWER_HANDLER_LOADING_FROM_STAGE_ONESTEP);
                    }
                    break;

                ///////////////////////////////////////////////////////////////////
                // process load with stage
                case TRS_LOWER_HANDLER_LOADING_FROM_STAGE_ONESTEP:
                    // branch
                    if (m_Data.ThreadHandshake_byOneStep == false)
                    {
                        SetStep2(TRS_LOWER_HANDLER_MOVETO_LOAD_POS);
                        break;
                    }

                    // init
                    TInterface.ResetInterface(TSelf);
                    TOpponent = (int)EThreadUnit.STAGE1;
                    if (TInterface.ErrorOccured[TOpponent]) break;

                    // begin
                    TTimer.StartTimer();
                    TInterface.Handler_Stage1_BeginHandshake_Load = true;

                    // step1
                    while (TInterface.Stage1_Handler_UnloadStep1 == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    iResult = m_RefComp.ctrlHandler.MoveToStagePos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                    iResult = m_RefComp.ctrlHandler.Absorb(index);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    TInterface.Handler_Stage1_LoadStep1 = true;

                    // step2
                    while (TInterface.Stage1_Handler_UnloadStep2 == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, true);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                    TInterface.Handler_Stage1_LoadStep2 = true;

                    // finish
                    while (TInterface.Stage1_Handler_FinishHandshake_Unload == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    TInterface.Handler_Stage1_FinishHandshake_Load = true;
                    Sleep(TInterface.TimeKeepOn);

                    // reset
                    TInterface.ResetInterface(TSelf);
                    SetStep2(TRS_LOWER_HANDLER_WAITFOR_MESSAGE);
                    break;

                ///////////////////////////////////////////////////////////////////
                // process unload with pushpull
                case TRS_LOWER_HANDLER_UNLOADING_TO_PUSHPULL_ONESTEP:
                    // branch
                    if (m_Data.ThreadHandshake_byOneStep == false)
                    {
                        SetStep2(TRS_LOWER_HANDLER_MOVETO_UNLOAD_POS);
                        break;
                    }

                    // init
                    TInterface.ResetInterface(TSelf);
                    TOpponent = (int)EThreadUnit.PUSHPULL;
                    if (TInterface.ErrorOccured[TOpponent]) break;

                    // begin
                    TTimer.StartTimer();
                    TInterface.Handler_PushPull_BeginHandshake_Unload = true;

                    // step1
                    while (TInterface.PushPull_Handler_LoadStep1 == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    iResult = m_RefComp.ctrlHandler.MoveToPushPullPos(index, true);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    TInterface.Handler_PushPull_UnloadStep1 = true;

                    // step2
                    while (TInterface.PushPull_Handler_LoadStep2 == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    iResult = m_RefComp.ctrlHandler.Release(index);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                    iResult = m_RefComp.ctrlHandler.MoveToWaitPos(index, false);
                    if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                    TInterface.Handler_PushPull_UnloadStep2 = true;

                    // finish
                    while (TInterface.PushPull_Handler_FinishHandshake_Load == false)
                    {
                        if (TInterface.ErrorOccured[TOpponent]) break;
                        if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                        {
                            TInterface.TimeOver[TSelf] = true;
                            break;
                        }
                        Sleep(ThreadSleepTime);
                    }
                    if (TInterface.ErrorOccured[TOpponent]) break;
                    if (TInterface.TimeOver[TSelf])
                    {
                        // do something, if it is needed
                        TInterface.ResetInterface(TSelf);
                        ReportAlarm(GenerateErrorCode(ERR_TRS_HANDLER_INTERFACE_TIMELIMIT_OVER));
                        break;
                    }

                    GetWorkPiece(ELCNetUnitPos.LOWER_HANDLER).FinishPhase(EProcessPhase.LOWER_HANDLER_UNLOAD);
                    TInterface.Handler_PushPull_FinishHandshake_Unload = true;
                    Sleep(TInterface.TimeKeepOn);

                    // reset
                    TInterface.ResetInterface(TSelf);
                    SetStep2(TRS_LOWER_HANDLER_WAITFOR_MESSAGE);
                    break;

                default:
                    break;
            }

            return SUCCESS;
        }
    }
}
