using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Thread.ETrsPushPullStep;
using static LWDicer.Layers.DEF_Thread.EThreadMessage;
using static LWDicer.Layers.DEF_Thread.EThreadChannel;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_LCNet;

using static LWDicer.Layers.DEF_CtrlPushPull;
using static LWDicer.Layers.DEF_CtrlLoader;
using static LWDicer.Layers.DEF_CtrlSpinner;
using static LWDicer.Layers.DEF_CtrlHandler;

namespace LWDicer.Layers
{
    public class CTrsPushPullRefComp
    {
        public MCtrlPushPull ctrlPushPull;
        public MCtrlLoader ctrlLoader;
        public MCtrlHandler ctrlHandler;
        public MCtrlSpinner ctrlCleaner;
        public MCtrlSpinner ctrlCoater;

        public MCtrlSpinner[] ctrlSpinner = new MCtrlSpinner[(int)ESpinnerIndex.MAX];

        public override string ToString()
        {
            return $"CTrsPushPullRefComp : {this}";
        }
    }

    public class CTrsPushPullData
    {
        public bool UseSpinnerSeparately   = false;  // spinner를 coater, cleaner로 구분하여 사용할지 여부
        public ELCNetUnitPos UCleanerIndex = ELCNetUnitPos.SPINNER1; // spinner를 구분지어 사용할 때, cleaner의 spinner index
        public ELCNetUnitPos UCoaterIndex  = ELCNetUnitPos.SPINNER2;  // spinner를 구분지어 사용할 때, coater의 spinner index
    }

    public class MTrsPushPull : MWorkerThread
    {
        private CTrsPushPullRefComp m_RefComp;
        private CTrsPushPullData m_Data;

        // Message 변수
        // with loader
        bool m_bLoader_ReadyLoading;
        bool m_bLoader_StartLoading;
        bool m_bLoader_CompleteLoading;
        bool m_bLoader_ReadyUnloading;
        bool m_bLoader_StartUnloading;
        bool m_bLoader_CompleteUnloading;
        bool m_bLoader_AllWaferWorked;
        bool m_bLoader_StacksFull;

        // with spinner
        bool[] m_bSpinner_ReadyLoading      = new bool[(int)ESpinnerIndex.MAX];
        bool[] m_bSpinner_StartLoading      = new bool[(int)ESpinnerIndex.MAX];
        bool[] m_bSpinner_CompleteLoading   = new bool[(int)ESpinnerIndex.MAX];
        bool[] m_bSpinner_ReadyUnloading    = new bool[(int)ESpinnerIndex.MAX];
        bool[] m_bSpinner_StartUnloading    = new bool[(int)ESpinnerIndex.MAX];
        bool[] m_bSpinner_CompleteUnloading = new bool[(int)ESpinnerIndex.MAX];

        // with handler
        bool m_bUpperHandler_WaitLoadingStart;
        bool m_bUpperHandler_StartLoading;
        bool m_bUpperHandler_RequestRelease;
        bool m_bUpperHandler_CompleteLoading;
        bool m_bLowerHandler_WaitUnloadingStart;
        bool m_bLowerHandler_StartUnloading;
        bool m_bLowerHandler_RequestAbsorb;
        bool m_bLowerHandler_CompleteUnloading;

        public MTrsPushPull(CObjectInfo objInfo, EThreadChannel SelfChannelNo, MDataManager DataManager, ELCNetUnitPos LCNetUnitPos,
            CTrsPushPullRefComp refComp, CTrsPushPullData data)
             : base(objInfo, SelfChannelNo, DataManager, LCNetUnitPos)
        {
            m_RefComp = refComp;
            SetData(data);
            TSelf = (int)EThreadUnit.PUSHPULL;
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CTrsPushPullData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsPushPullData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        override public string GetStep1()
        {
            ETrsPushPullStep cnvt = (ETrsPushPullStep)Enum.Parse(typeof(ETrsPushPullStep), ThreadStep1.ToString());
            return cnvt.ToString();
        }

        public override int Initialize()
        {
            // Do initialize
            InitializeMsg();
            InitializeInterface();

            // Do Action
            int iResult = m_RefComp.ctrlPushPull.Initialize();
            if (iResult != SUCCESS) return iResult;

            int iStep1 = (int)TRS_PUSHPULL_WAITFOR_MESSAGE;

            // finally
            SetStep1(iStep1);

            return base.Initialize();
        }

        public int InitializeMsg()
        {
            m_bLoader_ReadyUnloading = false;
            m_bLoader_ReadyLoading = false;
            m_bLoader_AllWaferWorked = false;
            m_bLoader_StacksFull = false;

            for(int i = 0; i < (int)ESpinnerIndex.MAX; i++)
            {
                m_bSpinner_ReadyLoading[i]      = false;
                m_bSpinner_StartLoading[i]      = false;
                m_bSpinner_CompleteLoading[i]   = false;
                m_bSpinner_ReadyUnloading[i]    = false;
                m_bSpinner_StartUnloading[i]    = false;
                m_bSpinner_CompleteUnloading[i] = false;
            }

            m_bUpperHandler_WaitLoadingStart = false;
            m_bUpperHandler_StartLoading = false;
            m_bUpperHandler_RequestRelease = false;
            m_bUpperHandler_CompleteLoading = false;
            m_bLowerHandler_WaitUnloadingStart = false;
            m_bLowerHandler_StartUnloading = false;
            m_bLowerHandler_RequestAbsorb = false;
            m_bLowerHandler_CompleteUnloading = false;

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

            // spinner index 정리
            int index = (int)ESpinnerIndex.SPINNER1;
            if(evnt.wParam == (int)EThreadChannel.TrsSpinner2) index = (int)ESpinnerIndex.SPINNER2;

            switch (evnt.Msg)
            {
                // if need to change response for common message, then add case state here.
                default:
                    base.ProcessMsg(evnt);
                    break;
/*
                // with Loader
                case (int)MSG_LOADER_PUSHPULL_WAIT_LOADING_START:
                    m_bLoader_ReadyLoading = true;
                    m_bLoader_StartLoading = false;
                    m_bLoader_CompleteLoading = false;
                    break;

                case (int)MSG_LOADER_PUSHPULL_START_LOADING:
                    m_bLoader_ReadyLoading = false;
                    m_bLoader_StartLoading = true;
                    m_bLoader_CompleteLoading = false;
                    break;

                case (int)MSG_LOADER_PUSHPULL_COMPLETE_LOADING:
                    m_bLoader_ReadyLoading = false;
                    m_bLoader_StartLoading = false;
                    m_bLoader_CompleteLoading = true;
                    break;

                case (int)MSG_LOADER_PUSHPULL_WAIT_UNLOADING_START:
                    m_bLoader_ReadyUnloading = true;
                    m_bLoader_StartUnloading = false;
                    m_bLoader_CompleteUnloading = false;
                    break;

                case (int)MSG_LOADER_PUSHPULL_START_UNLOADING:
                    m_bLoader_ReadyUnloading = false;
                    m_bLoader_StartUnloading = true;
                    m_bLoader_CompleteUnloading = false;
                    break;

                case (int)MSG_LOADER_PUSHPULL_COMPLETE_UNLOADING:
                    m_bLoader_ReadyUnloading = false;
                    m_bLoader_StartUnloading = false;
                    m_bLoader_CompleteUnloading = true;
                    break;

                case (int)MSG_LOADER_PUSHPULL_ALL_WAFER_WORKED:
                    m_bLoader_AllWaferWorked = true;
                    break;

                case (int)MSG_LOADER_PUSHPULL_STACKS_FULL:
                    m_bLoader_StacksFull = true;
                    break;
                    */

                //// with Spinner
                case (int)MSG_SPINNER_PUSHPULL_WAIT_LOADING_START:
                    m_bSpinner_ReadyLoading[index]      = true;
                    m_bSpinner_StartLoading[index]      = false;
                    m_bSpinner_CompleteLoading[index]   = false;
                    break;

                case (int)MSG_SPINNER_PUSHPULL_START_LOADING:
                    m_bSpinner_ReadyLoading[index]      = false;
                    m_bSpinner_StartLoading[index]      = true;
                    m_bSpinner_CompleteLoading[index]   = false;
                    break;

                case (int)MSG_SPINNER_PUSHPULL_COMPLETE_LOADING:
                    m_bSpinner_ReadyLoading[index]      = false;
                    m_bSpinner_StartLoading[index]      = false;
                    m_bSpinner_CompleteLoading[index]   = true;
                    break;

                case (int)MSG_SPINNER_PUSHPULL_WAIT_UNLOADING_START:
                    m_bSpinner_ReadyUnloading[index]    = true;
                    m_bSpinner_StartUnloading[index]    = false;
                    m_bSpinner_CompleteUnloading[index] = false;
                    break;

                case (int)MSG_SPINNER_PUSHPULL_START_UNLOADING:
                    m_bSpinner_ReadyUnloading[index]    = false;
                    m_bSpinner_StartUnloading[index]    = true;
                    m_bSpinner_CompleteUnloading[index] = false;
                    break;

                case (int)MSG_SPINNER_PUSHPULL_COMPLETE_UNLOADING:
                    m_bSpinner_ReadyUnloading[index]    = false;
                    m_bSpinner_StartUnloading[index]    = false;
                    m_bSpinner_CompleteUnloading[index] = true;
                    break;

                // with Handler
                case (int)MSG_UPPER_HANDLER_PUSHPULL_WAIT_LOADING_START:
                    m_bUpperHandler_WaitLoadingStart = true;
                    m_bUpperHandler_StartLoading = false;
                    m_bUpperHandler_RequestRelease = false;
                    m_bUpperHandler_CompleteLoading = false;
                    break;

                case (int)MSG_UPPER_HANDLER_PUSHPULL_START_LOADING:
                    m_bUpperHandler_WaitLoadingStart = false;
                    m_bUpperHandler_StartLoading = true;
                    m_bUpperHandler_RequestRelease = false;
                    m_bUpperHandler_CompleteLoading = false;
                    break;

                case (int)MSG_UPPER_HANDLER_PUSHPULL_REQUEST_RELEASE:
                    m_bUpperHandler_WaitLoadingStart = false;
                    m_bUpperHandler_StartLoading = false;
                    m_bUpperHandler_RequestRelease = true;
                    m_bUpperHandler_CompleteLoading = false;
                    break;

                case (int)MSG_UPPER_HANDLER_PUSHPULL_COMPLETE_LOADING:
                    m_bUpperHandler_WaitLoadingStart = false;
                    m_bUpperHandler_StartLoading = false;
                    m_bUpperHandler_RequestRelease = false;
                    m_bUpperHandler_CompleteLoading = true;
                    break;

                case (int)MSG_LOWER_HANDLER_PUSHPULL_WAIT_UNLOADING_START:
                    m_bLowerHandler_WaitUnloadingStart = true;
                    m_bLowerHandler_StartUnloading = false;
                    m_bLowerHandler_RequestAbsorb = false;
                    m_bLowerHandler_CompleteUnloading = false;
                    break;

                case (int)MSG_LOWER_HANDLER_PUSHPULL_START_UNLOADING:
                    m_bLowerHandler_WaitUnloadingStart = false;
                    m_bLowerHandler_StartUnloading = true;
                    m_bLowerHandler_RequestAbsorb = false;
                    m_bLowerHandler_CompleteUnloading = false;
                    break;

                case (int)MSG_LOWER_HANDLER_PUSHPULL_REQUEST_ABSORB:
                    m_bLowerHandler_WaitUnloadingStart = false;
                    m_bLowerHandler_StartUnloading = false;
                    m_bLowerHandler_RequestAbsorb = true;
                    m_bLowerHandler_CompleteUnloading = false;
                    break;

                case (int)MSG_LOWER_HANDLER_PUSHPULL_COMPLETE_UNLOADING:
                    m_bLowerHandler_WaitUnloadingStart = false;
                    m_bLowerHandler_StartUnloading = false;
                    m_bLowerHandler_RequestAbsorb = false;
                    m_bLowerHandler_CompleteUnloading = true;
                    break;

            }
            return SUCCESS;
        }

        protected override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bStatus, bStatus1, bStatus2;
            EProcessPhase processPhase;
            int spinnerIndex;
            int nSlotCount;

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
                        //m_RefComp.ctrlPushPull.SetAutoManual(EAutoManual.MANUAL);
                        break;

                    case EAutoRunStatus.STS_ERROR_STOP: // Error Stop
                        break;

                    case EAutoRunStatus.STS_STEP_STOP: // Step Stop
                        break;

                    case EAutoRunStatus.STS_RUN_READY: // Run Ready
                        break;

                    case EAutoRunStatus.STS_CYCLE_STOP: // Cycle Stop
                        //if (ThreadStep1 == TRS_LOADER_MOVETO_LOAD)
                        break;

                    case EAutoRunStatus.STS_RUN: // auto run
                        //m_RefComp.ctrlPushPull.SetAutoManual(EAutoManual.AUTO);

                        // Do Thread Step
                        switch (ThreadStep1)
                        {
                            case (int)TRS_PUSHPULL_MOVETO_WAIT_POS:
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                SetStep1((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            case (int)TRS_PUSHPULL_WAITFOR_MESSAGE:

                                // 0. check default status;

                                // 1. if detected wafer
                                m_RefComp.ctrlPushPull.IsObjectDetected(out bStatus);
                                if (bStatus)
                                {
                                    // wafer의 다음 해야할 일을 보고, 해당 unit에게 unload하는 분기점으로 이동시킨다.
                                    GetMyWorkPiece().GetNextPhase(out processPhase);

                                    if(processPhase == EProcessPhase.PUSHPULL_UNLOAD_TO_COATER)
                                    {
                                        if (m_Data.UseSpinnerSeparately) // spinner를 구분지어 사용한다면
                                        {
                                            iResult = m_RefComp.ctrlCoater.IsObjectDetected(out bStatus1);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                            if (bStatus1 == true)
                                            {
                                                ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_OBJECT_DETECTED_ON_COATER));
                                                break;
                                            }
                                        }
                                        else
                                        {

                                        }

                                        SetStep1((int)TRS_PUSHPULL_BEGIN_UNLOADING_TO_SPINNER);

                                    }
                                    switch (processPhase)
                                    {
                                        // unload to coater
                                        case EProcessPhase.PUSHPULL_UNLOAD_TO_COATER:
                                            if (m_Data.UseSpinnerSeparately) // spinner를 구분지어 사용한다면
                                            {
                                                iResult = m_RefComp.ctrlCoater.IsObjectDetected(out bStatus1);
                                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                                if(bStatus1 == true)
                                                {

                                                }
                                            }
                                            else
                                            {

                                            }

                                                SetStep1((int)TRS_PUSHPULL_BEGIN_UNLOADING_TO_SPINNER);
                                            break;

                                        // unload to handler
                                        case EProcessPhase.PUSHPULL_UNLOAD_TO_HANDLER:
                                            SetStep1((int)TRS_PUSHPULL_BEGIN_UNLOADING_TO_HANDLER);
                                            break;

                                        // unload to cleaner
                                        case EProcessPhase.PUSHPULL_UNLOAD_TO_CLEANER:
                                            SetStep1((int)TRS_PUSHPULL_BEGIN_UNLOADING_TO_SPINNER);
                                            break;

                                        // unload to loader
                                        case EProcessPhase.PUSHPULL_UNLOAD_TO_LOADER:
                                            if(m_RefComp.ctrlLoader.GetEmptySlotCount() > 0)
                                                SetStep1((int)TRS_PUSHPULL_BEGIN_UNLOADING_TO_LOADER);
                                            break;
                                    }
                                }
                                // 2. if not detected wafer
                                else
                                {
                                    bool bStepBreak = false;

                                    if(m_Data.UseSpinnerSeparately) // spinner를 구분지어 사용한다면
                                    {
                                        // 2.1 coater에 작업 완료된 wafer가 있고, load handler가 비어있다면
                                        iResult = m_RefComp.ctrlCoater.IsObjectDetected(out bStatus1);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                        iResult = m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER, out bStatus2);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                        if(bStatus1 == true && bStatus2 == false)
                                        {
                                            if (GetWorkPiece(m_Data.UCoaterIndex).GetNextPhase() == (int)EProcessPhase.COATER_UNLOAD)
                                            {
                                                bStepBreak = true;
                                                SetStep1((int)TRS_PUSHPULL_BEGIN_LOADING_FROM_COATER);
                                                break;
                                            }
                                        }
                                        if (bStepBreak == true) break; // for break switch case

                                        // 2.2 cleaner에 작업 완료된 wafer가 있고, loader가 비어있다면
                                        iResult = m_RefComp.ctrlCleaner.IsObjectDetected(out bStatus1);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                        nSlotCount = m_RefComp.ctrlLoader.GetEmptySlotCount();

                                        if (bStatus1 == true && nSlotCount > 0)
                                        {
                                            if (GetWorkPiece(m_Data.UCleanerIndex).GetNextPhase() == (int)EProcessPhase.CLEANER_UNLOAD)
                                            {
                                                bStepBreak = true;
                                                SetStep1((int)TRS_PUSHPULL_BEGIN_LOADING_FROM_CLEANER);
                                                break;
                                            }
                                        }
                                        if (bStepBreak == true) break; // for break switch case

                                        // 2.3 unload handler에 wafer가 있고, cleaner가 비어있다면
                                        iResult = m_RefComp.ctrlCleaner.IsObjectDetected(out bStatus1);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                        iResult = m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER, out bStatus2);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                        if (bStatus1 == false && bStatus2 == true)
                                        {
                                            bStepBreak = true;
                                            SetStep1((int)TRS_PUSHPULL_BEGIN_LOADING_FROM_HANDLER);
                                            break;
                                        }
                                        if (bStepBreak == true) break; // for break switch case

                                        // 2.4 coater가 비어있고, loader에 wafer가 대기하고 있다면
                                        iResult = m_RefComp.ctrlCoater.IsObjectDetected(out bStatus1);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                        nSlotCount = m_RefComp.ctrlLoader.GetPreProcessWaferCount();

                                        if (bStatus1 == false && nSlotCount > 0)
                                        {
                                            bStepBreak = true;
                                            SetStep1((int)TRS_PUSHPULL_BEGIN_LOADING_FROM_LOADER);
                                            break;
                                        }
                                        if (bStepBreak == true) break; // for break switch case
                                    }
                                    else // spinner를 공용으로 사용한다면
                                    {
                                        // 2.0.1 get spinners empty count
                                        int nEmptyCount_Spinner = 0;

                                        // 2.1 spinner가 unload ready 상태라면,
                                        for (int i = 0; i < m_bSpinner_ReadyUnloading.Length; i++)
                                        {
                                            iResult = m_RefComp.ctrlSpinner[i].IsObjectDetected(out bStatus);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                            if (bStatus == false) nEmptyCount_Spinner++;
                                            if (m_bSpinner_ReadyUnloading[i] == true)
                                            {
                                                // 2.1.1 coater에 작업 완료된 wafer가 있고, load handler가 비어있다면
                                                iResult = m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER, out bStatus2);
                                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                                if (GetWorkPiece((int)ELCNetUnitPos.SPINNER1 + i).GetNextPhase() == (int)EProcessPhase.COATER_UNLOAD
                                                    && bStatus2 == false)
                                                {
                                                    bStepBreak = true;
                                                    spinnerIndex = i;
                                                    SetStep1((int)TRS_PUSHPULL_BEGIN_LOADING_FROM_COATER);
                                                    break;
                                                }

                                                // 2.1.2 cleaner에 작업 완료된 wafer가 있고, loader가 비어있다면
                                                nSlotCount = m_RefComp.ctrlLoader.GetEmptySlotCount();
                                                if (GetWorkPiece((int)ELCNetUnitPos.SPINNER1 + i).GetNextPhase() == (int)EProcessPhase.CLEANER_UNLOAD
                                                    && nSlotCount > 0)
                                                {
                                                    bStepBreak = true;
                                                    spinnerIndex = i;
                                                    SetStep1((int)TRS_PUSHPULL_BEGIN_LOADING_FROM_CLEANER);
                                                    break;
                                                }
                                            }
                                        }
                                        if (bStepBreak == true) break; // for break switch case

                                        // 2.2 unload handler에 wafer가 있고, cleaner가 비어있다면
                                        iResult = m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER, out bStatus2);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                        if (bStatus2 == true && nEmptyCount_Spinner > 0)
                                        {
                                            bStepBreak = true;
                                            SetStep1((int)TRS_PUSHPULL_BEGIN_LOADING_FROM_HANDLER);
                                            break;
                                        }
                                        if (bStepBreak == true) break; // for break switch case

                                        // 2.3 Loader로부터 새로운 제품을 loading
                                        // 조건 0 : Loader에 pre process 대기중인 wafer가 있고,
                                        // 조건 1 : Spinner가 두군데 모두 비어있다면, Coater작업을 위한 zone은 확보되어있으므로
                                        // 조건 2 : Spinner가 한군데 비어있고, Load Handler가 비어있다면
                                        nSlotCount = m_RefComp.ctrlLoader.GetPreProcessWaferCount();
                                        if (nSlotCount > 0)
                                        {
                                            iResult = m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER, out bStatus2);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                            if (nEmptyCount_Spinner > 1
                                                || (nEmptyCount_Spinner == 1 && bStatus2 == false))
                                            {
                                                bStepBreak = true;
                                                TInterface.ResetInterface(TSelf);
                                                SetStep1((int)TRS_PUSHPULL_LOADING_FROM_LOADER);
                                                break;
                                            }
                                        }
                                        if (bStepBreak == true) break; // for break switch case
                                    }
                                }
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // with loader // wafer : loader -> pushpull               
                            case (int)TRS_PUSHPULL_LOADING_FROM_LOADER:
                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.LOADER;
                                if (TInterface.ErrorOccured[TOpponent]) break;
                                TTimer.StartTimer();

                                GetMyWorkPiece().StartPhase(EProcessPhase.PUSHPULL_LOAD_FROM_LOADER);

                                // begin
                                TInterface.PushPull_Loader_RequestUnloading = true;
                                while(TInterface.Loader_PushPull_RequestLoading == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit))
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // loading
                                iResult = m_RefComp.ctrlPushPull.MoveToLoaderPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                iResult = m_RefComp.ctrlPushPull.GripLock();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Loader_LoadReady = true;
                                while (TInterface.Loader_PushPull_UnloadReady == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit))
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Loader_FinishLoading = true;
                                while (TInterface.Loader_PushPull_FinishUnloading == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit))
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                GetMyWorkPiece().FinishPhase(EProcessPhase.PUSHPULL_LOAD_FROM_LOADER);

                                SetStep1((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;
                                /*
                            case (int)TRS_PUSHPULL_BEGIN_LOADING_FROM_LOADER: // grip release, send request unload signal
                                PostMsg_Interval(TrsLoader, MSG_PUSHPULL_LOADER_REQUEST_UNLOADING);
                                if (m_bLoader_StartUnloading == false) break;

                                iResult = m_RefComp.ctrlPushPull.GripRelease();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                PostMsg(TrsLoader, MSG_PUSHPULL_LOADER_START_LOADING);
                                SetStep1((int)TRS_PUSHPULL_LOAD_FROM_LOADER);
                                break;

                            case (int)TRS_PUSHPULL_LOAD_FROM_LOADER: // move to load pos, grip lock, send load ready signal
                                iResult = m_RefComp.ctrlPushPull.MoveToLoaderPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                iResult = m_RefComp.ctrlPushPull.GripLock();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                PostMsg(TrsLoader, MSG_PUSHPULL_LOADER_COMPLETE_ABSORB);
                                SetStep1((int)TRS_PUSHPULL_WAITFOR_LOADER_UNLOAD_COMPLETE);
                                break;

                            case (int)TRS_PUSHPULL_WAITFOR_LOADER_UNLOAD_COMPLETE: // wait for response signal
                                PostMsg_Interval(TrsLoader, MSG_PUSHPULL_LOADER_COMPLETE_ABSORB);
                                if (m_bLoader_ReadyUnloading == false) break;

                                // do something..

                                SetStep1((int)TRS_PUSHPULL_FINISH_LOADING_FROM_LOADER);
                                break;

                            case (int)TRS_PUSHPULL_FINISH_LOADING_FROM_LOADER: // move to wait pos: send load complete signal
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                PostMsg(TrsLoader, MSG_PUSHPULL_LOADER_COMPLETE_LOADING);
                                SetStep1((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;
                                */
                            ///////////////////////////////////////////////////////////////////
                            // with loader // wafer : pushpull -> loader                  
                            case (int)TRS_PUSHPULL_BEGIN_UNLOADING_TO_LOADER:      // move to unload pos
                            case (int)TRS_PUSHPULL_REQUEST_LOADER_LOADING:            // send load request signal
                            case (int)TRS_PUSHPULL_WAITFOR_LOADER_LOAD_READY:         // wait for response signal
                            case (int)TRS_PUSHPULL_UNLOAD_TO_LOADER:               // extend guide: send vacuum complete signal
                            case (int)TRS_PUSHPULL_WAITFOR_LOADER_LOAD_COMPLETE:      // wait for response signal
                            case (int)TRS_PUSHPULL_FINISH_UNLOADING_TO_LOADER:     // move to wait pos: send unload complete signal
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                SetStep1((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // with Spinner // wafer : spinner -> pushpull
                            case (int)TRS_PUSHPULL_BEGIN_LOADING_FROM_SPINNER:     // move to load pos
                            case (int)TRS_PUSHPULL_PRE_LOADING_FROM_SPINNER:          // extend guide: send load ready signal
                            case (int)TRS_PUSHPULL_WAITFOR_SPINNER_UNLOAD_READY:      // wait for response signal
                            case (int)TRS_PUSHPULL_LOAD_FROM_SPINNER:              // withdraw guide: send vacuum complete signal
                            case (int)TRS_PUSHPULL_WAITFOR_SPINNER_UNLOAD_COMPLETE:   // wait for response signal
                            case (int)TRS_PUSHPULL_FINISH_LOADING_FROM_SPINNER:    // move to wait pos: send load complete signal
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                SetStep1((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // with Spinner // wafer : pushpull -> spinner
                            case (int)TRS_PUSHPULL_BEGIN_UNLOADING_TO_SPINNER:     // move to unload pos
                            case (int)TRS_PUSHPULL_REQUEST_SPINNER_LOADING:           // send load request signal
                            case (int)TRS_PUSHPULL_WAITFOR_SPINNER_LOAD_READY:        // wait for response signal
                            case (int)TRS_PUSHPULL_UNLOAD_TO_SPINNER:              // extend guide: send vacuum complete signal
                            case (int)TRS_PUSHPULL_WAITFOR_SPINNER_LOAD_COMPLETE:     // wait for response signal
                            case (int)TRS_PUSHPULL_FINISH_UNLOADING_TO_SPINNER:    // move to wait pos: send unload complete signal
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                SetStep1((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // with handler // wafer : handler -> pushpull
                            case (int)TRS_PUSHPULL_BEGIN_LOADING_FROM_HANDLER:     // move to load pos
                                iResult = m_RefComp.ctrlPushPull.MoveToHandlerPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                iResult = m_RefComp.ctrlPushPull.GripRelease();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                iResult = m_RefComp.ctrlPushPull.MoveAllCenterUnitToWaitPos();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                PostMsg(TrsHandler, MSG_PUSHPULL_LOWER_HANDLER_REQUEST_UNLOADING);
                                SetStep1((int)TRS_PUSHPULL_PRE_LOADING_FROM_HANDLER);
                                break;

                            case (int)TRS_PUSHPULL_PRE_LOADING_FROM_HANDLER:          // extend guide: send load ready signal
                                PostMsg_Interval(TrsHandler, MSG_PUSHPULL_LOWER_HANDLER_REQUEST_UNLOADING);
                                if (m_bLowerHandler_StartUnloading == false) break;

                                PostMsg(TrsHandler, MSG_PUSHPULL_LOWER_HANDLER_START_LOADING);
                                SetStep1((int)TRS_PUSHPULL_WAITFOR_HANDLER_UNLOAD_READY);
                                break;

                            case (int)TRS_PUSHPULL_WAITFOR_HANDLER_UNLOAD_READY:      // wait for response signal
                                PostMsg_Interval(TrsHandler, MSG_PUSHPULL_LOWER_HANDLER_START_LOADING);
                                if (m_bLowerHandler_RequestAbsorb == false) break;

                                SetStep1((int)TRS_PUSHPULL_LOAD_FROM_HANDLER);
                                break;

                            case (int)TRS_PUSHPULL_LOAD_FROM_HANDLER:              // withdraw guide: send vacuum complete signal
                                iResult = m_RefComp.ctrlPushPull.GripLock();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                PostMsg(TrsHandler, MSG_PUSHPULL_LOWER_HANDLER_ABSORB_COMPLETE);
                                SetStep1((int)TRS_PUSHPULL_WAITFOR_HANDLER_UNLOAD_COMPLETE);
                                break;

                            case (int)TRS_PUSHPULL_WAITFOR_HANDLER_UNLOAD_COMPLETE:   // wait for response signal
                                PostMsg_Interval(TrsHandler, MSG_PUSHPULL_LOWER_HANDLER_ABSORB_COMPLETE);
                                if (m_bLowerHandler_CompleteUnloading == false) break;

                                iResult = m_RefComp.ctrlPushPull.MoveAllCenterUnitToCenteringPos();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                PostMsg(TrsHandler, MSG_PUSHPULL_LOWER_HANDLER_COMPLETE_LOADING);
                                SetStep1((int)TRS_PUSHPULL_FINISH_LOADING_FROM_HANDLER);
                                break;

                            case (int)TRS_PUSHPULL_FINISH_LOADING_FROM_HANDLER:    // move to wait pos: send load complete signal
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                SetStep1((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // with handler // wafer : pushpull -> handler
                            case (int)TRS_PUSHPULL_BEGIN_UNLOADING_TO_HANDLER:     // move to unload pos
                                iResult = m_RefComp.ctrlPushPull.MoveToHandlerPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                PostMsg(TrsHandler, MSG_PUSHPULL_UPPER_HANDLER_REQUEST_LOADING);
                                SetStep1((int)TRS_PUSHPULL_REQUEST_HANDLER_LOADING);
                                break;

                            case (int)TRS_PUSHPULL_REQUEST_HANDLER_LOADING:           // send load request signal
                                PostMsg_Interval(TrsHandler, MSG_PUSHPULL_UPPER_HANDLER_REQUEST_LOADING);
                                if (m_bUpperHandler_StartLoading == false) break;

                                SetStep1((int)TRS_PUSHPULL_WAITFOR_HANDLER_LOAD_READY);
                                break;

                            case (int)TRS_PUSHPULL_WAITFOR_HANDLER_LOAD_READY:        // wait for response signal
                                PostMsg_Interval(TrsHandler, MSG_PUSHPULL_UPPER_HANDLER_REQUEST_LOADING);
                                if (m_bUpperHandler_RequestRelease == false) break;

                                iResult = m_RefComp.ctrlPushPull.GripRelease();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                iResult = m_RefComp.ctrlPushPull.MoveAllCenterUnitToWaitPos();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                SetStep1((int)TRS_PUSHPULL_UNLOAD_TO_HANDLER);
                                break;

                            case (int)TRS_PUSHPULL_UNLOAD_TO_HANDLER:              // extend guide: send vacuum complete signal
                                PostMsg_Interval(TrsHandler, MSG_PUSHPULL_UPPER_HANDLER_RELEASE_COMPLETE);
                                if (m_bUpperHandler_CompleteLoading == false) break;

                                PostMsg(TrsHandler, MSG_PUSHPULL_UPPER_HANDLER_COMPLETE_UNLOADING);
                                SetStep1((int)TRS_PUSHPULL_WAITFOR_HANDLER_LOAD_COMPLETE);
                                break;

                            case (int)TRS_PUSHPULL_WAITFOR_HANDLER_LOAD_COMPLETE:     // wait for response signal
                                SetStep1((int)TRS_PUSHPULL_FINISH_UNLOADING_TO_HANDLER);
                                break;

                            case (int)TRS_PUSHPULL_FINISH_UNLOADING_TO_HANDLER:    // move to wait pos: send unload complete signal
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                SetStep1((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                Sleep(ThreadSleepTime);
                //Debug.WriteLine(ToString() + " Thread running..");
            }

        }

    }
}
