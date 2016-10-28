using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Thread.EThreadStep;
using static LWDicer.Layers.DEF_Thread.EThreadMessage;
using static LWDicer.Layers.DEF_Thread.EThreadChannel;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_LCNet;

using static LWDicer.Layers.DEF_CtrlPushPull;
using static LWDicer.Layers.DEF_CtrlLoader;
using static LWDicer.Layers.DEF_MeElevator;

namespace LWDicer.Layers
{
    public class CTrsLoaderRefComp
    {
        public MCtrlLoader ctrlLoader;

        public override string ToString()
        {
            return $"CTrsLoaderRefComp : {this}";
        }
    }

    public class CTrsLoaderData
    {
        public bool ThreadHandshake_byOneStep = true;
    }

    public class MTrsLoader : MWorkerThread
    {
        private CTrsLoaderRefComp m_RefComp;
        private CTrsLoaderData m_Data;

        // Message 변수
        bool m_bPushPull_RequestUnloading;
        bool m_bPushPull_StartLoading;
        bool m_bPushPull_CompleteAbsorb;
        bool m_bPushPull_CompleteLoading;

        bool m_bPushPull_RequestLoading;
        bool m_bPushPull_StartUnloading;
        bool m_bPushPull_ReadyUnloading;
        bool m_bPushPull_CompleteUnloading;

        bool m_bAuto_RequestLoadCassette;
        bool m_bAuto_RequestUnloadCassette;

        bool m_bSupplyCassette = false;
        bool m_bSupplyWafer = true;

        public MTrsLoader(CObjectInfo objInfo, EThreadChannel SelfChannelNo, MDataManager DataManager, ELCNetUnitPos LCNetUnitPos,
            CTrsLoaderRefComp refComp, CTrsLoaderData data)
             : base(objInfo, SelfChannelNo, DataManager, LCNetUnitPos)
        {
            m_RefComp = refComp;
            SetData(data);
            TSelf = (int)EThreadUnit.LOADER;
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CTrsLoaderData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsLoaderData target)
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
            int iResult = m_RefComp.ctrlLoader.Initialize();
            if (iResult != SUCCESS) return iResult;

            EThreadStep iStep1 = TRS_LOADER_WAITFOR_MESSAGE;

            // finally
            SetStep(iStep1);

            return base.Initialize();
        }

        public int InitializeMsg()
        {
            m_bPushPull_RequestUnloading = false;
            m_bPushPull_StartLoading = false;
            m_bPushPull_CompleteLoading = false;
            m_bPushPull_RequestLoading = false;
            m_bPushPull_StartUnloading = false;
            m_bPushPull_CompleteUnloading = false;

            m_bAuto_RequestLoadCassette = false;
            m_bAuto_RequestUnloadCassette = false;

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

                case (int)MSG_START_CASSETTE_SUPPLY:
                    m_bSupplyCassette = true;
                    break;

                case (int)MSG_STOP_CASSETTE_SUPPLY:
                    m_bSupplyCassette = false;
                    break;

                case (int)MSG_START_WAFER_SUPPLY:
                    m_bSupplyWafer = true;
                    break;

                case (int)MSG_STOP_WAFER_SUPPLY:
                    m_bSupplyWafer = false;
                    break;
                    /*
                case (int)MSG_PUSHPULL_LOADER_REQUEST_UNLOADING:
                    m_bPushPull_RequestUnloading = true;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_CompleteAbsorb = false;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_PUSHPULL_LOADER_START_LOADING:
                    m_bPushPull_RequestUnloading = false;
                    m_bPushPull_StartLoading = true;
                    m_bPushPull_CompleteAbsorb = false;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_PUSHPULL_LOADER_COMPLETE_ABSORB:
                    m_bPushPull_RequestUnloading = false;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_CompleteAbsorb = true;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_PUSHPULL_LOADER_COMPLETE_LOADING:
                    m_bPushPull_RequestUnloading = false;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_CompleteAbsorb = false;
                    m_bPushPull_CompleteLoading = true;
                    break;
                    
                case (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING:
                    m_bPushPull_RequestLoading = true;
                    break;

                case (int)MSG_PUSHPULL_LOADER_START_UNLOADING:
                    m_bPushPull_StartUnloading = true;
                    break;

                case (int)MSG_PUSHPULL_LOADER_COMPLETE_UNLOADING:
                    m_bPushPull_CompleteUnloading = true;
                    break;
                    */
                case (int)MSG_AUTO_LOADER_REQUEST_LOAD_CASSETTE:
                    m_bAuto_RequestLoadCassette = true;
                    break;

                case (int)MSG_AUTO_LOADER_REQUEST_UNLOAD_CASSETTE:
                    m_bAuto_RequestUnloadCassette = true;
                    break;

            }
            return SUCCESS;
        }

        protected override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bStatus, bStatus1, bStatus2;
            int index;

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
                        //m_RefComp.ctrlLoader.SetAutoManual(EAutoManual.MANUAL);
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
                        //m_RefComp.ctrlLoader.SetAutoManual(EAutoManual.AUTO);

                        // Do Thread Step
                        switch (ThreadStep)
                        {
                            case TRS_LOADER_WAITFOR_MESSAGE:

                                // 0. check wafer cassette status;

                                // 1. response to loading cassette
                                if (m_bAuto_RequestLoadCassette && m_bSupplyCassette)
                                {
                                    SetStep(TRS_LOADER_READY_LOAD_CASSETTE);
                                    break;
                                }

                                // 2. response to unloading cassette
                                if (m_bAuto_RequestUnloadCassette)
                                {
                                    SetStep(TRS_LOADER_READY_UNLOAD_CASSETTE);
                                    break;
                                }

                                // 3. response to loading wafer
                                if (TInterface.PushPull_Loader_BeginHandshake_Unload)
                                {
                                    SetStep(TRS_LOADER_LOADING_FROM_PUSHPULL_ONESTEP);
                                    break;
                                }

                                // 4. response to unloading wafer
                                if (TInterface.PushPull_Loader_BeginHandshake_Load && m_bSupplyWafer)
                                {
                                    SetStep(TRS_LOADER_UNLOADING_TO_PUSHPULL_ONESTEP);
                                    break;
                                }
                                break;

                            case TRS_LOADER_READY_LOAD_CASSETTE:

                                SetStep(TRS_LOADER_WAITFOR_CASSETTE_LOADED);
                                break;

                            case TRS_LOADER_WAITFOR_CASSETTE_LOADED:

                                SetStep(TRS_LOADER_LOAD_CASSETTE);
                                break;

                            case TRS_LOADER_LOAD_CASSETTE:

                                SetStep(TRS_LOADER_CHECK_STACK_OF_CASSETTE);
                                break;

                            case TRS_LOADER_CHECK_STACK_OF_CASSETTE:

                                SetStep(TRS_LOADER_WAITFOR_MESSAGE);
                                break;

                            case TRS_LOADER_READY_UNLOAD_CASSETTE:

                                SetStep(TRS_LOADER_WAITFOR_CASSETTE_REMOVED);
                                break;

                            case TRS_LOADER_WAITFOR_CASSETTE_REMOVED:

                                SetStep(TRS_LOADER_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process load with pushpull
                            case TRS_LOADER_LOADING_FROM_PUSHPULL_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_LOADER_BEGIN_LOADING_FROM_PUSHPULL);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.PUSHPULL;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                TInterface.Loader_PushPull_BeginHandshake_Load = true;

                                // step1
                                iResult = m_RefComp.ctrlLoader.MoveToNextEmptySlot(out index);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.Loader_PushPull_LoadStep1 = true;
                                while (TInterface.PushPull_Loader_UnloadStep1 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_LOADER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                // vacuum
                                TInterface.Loader_PushPull_LoadStep2 = true;
                                while (TInterface.PushPull_Loader_UnloadStep2 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_LOADER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                m_RefComp.ctrlLoader.SetCassetteSlotStatus(index, ECassetteSlotStatus.AFTER_PROCESS);

                                TInterface.Loader_PushPull_FinishHandshake_Load = true;
                                while (TInterface.PushPull_Loader_FinishHandshake_Unload == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_LOADER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);
                                SetStep(TRS_LOADER_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process unload with pushpull
                            case TRS_LOADER_UNLOADING_TO_PUSHPULL_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_LOADER_BEGIN_UNLOADING_TO_PUSHPULL);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.PUSHPULL;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                TInterface.Loader_PushPull_BeginHandshake_Unload = true;

                                // step1
                                iResult = m_RefComp.ctrlLoader.MoveToNextPreProcessSlot(out index);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.Loader_PushPull_UnloadStep1 = true;
                                while (TInterface.PushPull_Loader_LoadStep1 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_LOADER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                // vacuum
                                TInterface.Loader_PushPull_UnloadStep2 = true;
                                while (TInterface.PushPull_Loader_LoadStep2 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_LOADER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                m_RefComp.ctrlLoader.SetCassetteSlotStatus(index, ECassetteSlotStatus.EMPTY);

                                TInterface.Loader_PushPull_FinishHandshake_Unload = true;
                                while (TInterface.PushPull_Loader_FinishHandshake_Load == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_LOADER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);
                                SetStep(TRS_LOADER_WAITFOR_MESSAGE);
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
