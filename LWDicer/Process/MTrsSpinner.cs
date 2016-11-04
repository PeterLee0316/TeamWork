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
using static LWDicer.Layers.DEF_CtrlSpinner;

namespace LWDicer.Layers
{
    public class CTrsSpinnerRefComp
    {
        public MCtrlSpinner ctrlSpinner;
        public MCtrlPushPull ctrlPushPull;

        public override string ToString()
        {
            return $"CTrsSpinnerRefComp : {this}";
        }
    }

    public class CTrsSpinnerData
    {
        public bool ThreadHandshake_byOneStep = true;

        public ESpinnerIndex SpinnerIndex = ESpinnerIndex.SPINNER1;
        public bool DoPreClean;
        public bool DoPostClean;
        public bool DoCoat;
    }

    public class MTrsSpinner : MWorkerThread
    {
        private CTrsSpinnerRefComp m_RefComp;
        private CTrsSpinnerData m_Data;

        // Message 변수
        bool m_bPushPull_RequestLoading;
        bool m_bPushPull_StartUnloading;
        bool m_bPushPull_CompleteUnloading;

        bool m_bPushPull_ReadyLoading;
        bool m_bPushPull_StartLoading;
        bool m_bPushPull_CompleteLoading;

        bool m_bRequest_Do_PreCleaing;
        bool m_bRequest_Do_Coating;
        bool m_bRequest_Do_PostClening;

        public MTrsSpinner(CObjectInfo objInfo, EThreadChannel SelfChannelNo, MDataManager DataManager, ELCNetUnitPos LCNetUnitPos,
            CTrsSpinnerRefComp refComp, CTrsSpinnerData data)
             : base(objInfo, SelfChannelNo, DataManager, LCNetUnitPos)
        {
            m_RefComp = refComp;
            SetData(data);
            if(SelfChannelNo == EThreadChannel.TrsSpinner1)
                TSelf = (int)EThreadUnit.SPINNER1;
            else TSelf = (int)EThreadUnit.SPINNER2;
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CTrsSpinnerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsSpinnerData target)
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
            int iResult = m_RefComp.ctrlSpinner.Initialize();
            if (iResult != SUCCESS) return iResult;

            EThreadStep iStep1 = TRS_SPINNER_MOVETO_WAIT_POS;

            // finally
            SetStep(iStep1);

            return base.Initialize();
        }

        public int InitializeMsg()
        {
            m_bPushPull_RequestLoading = false;
            m_bPushPull_StartUnloading = false;
            m_bPushPull_CompleteUnloading = false;

            m_bPushPull_ReadyLoading = false;
            m_bPushPull_StartLoading = false;
            m_bPushPull_CompleteLoading = false;


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

                // with PushPull
                case (int)MSG_PUSHPULL_SPINNER_REQUEST_LOADING:
                    m_bPushPull_RequestLoading = true;
                    m_bPushPull_StartUnloading = false;
                    m_bPushPull_CompleteUnloading = false;
                    break;

                case (int)MSG_PUSHPULL_SPINNER_START_UNLOADING:
                    m_bPushPull_RequestLoading = false;
                    m_bPushPull_StartUnloading = true;
                    m_bPushPull_CompleteUnloading = false;
                    break;

                case (int)MSG_PUSHPULL_SPINNER_COMPLETE_UNLOADING:
                    m_bPushPull_RequestLoading = false;
                    m_bPushPull_StartUnloading = false;
                    m_bPushPull_CompleteUnloading = true;
                    break;

                case (int)MSG_PUSHPULL_SPINNER_READY_LOADING:
                    m_bPushPull_ReadyLoading = true;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_PUSHPULL_SPINNER_START_LOADING:
                    m_bPushPull_ReadyLoading = false;
                    m_bPushPull_StartLoading = true;
                    m_bPushPull_CompleteLoading = false;
                    break;

                case (int)MSG_PUSHPULL_SPINNER_COMPLETE_LOADING:
                    m_bPushPull_ReadyLoading = false;
                    m_bPushPull_StartLoading = false;
                    m_bPushPull_CompleteLoading = true;
                    break;
            }
            return SUCCESS;
        }

        protected override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bStatus, bStatus1, bStatus2;
            EProcessPhase tPhase = GetMyNextWorkPhase();

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
                        //if (ThreadStep1 == TRS_LOADER_MOVETO_LOAD)
                        break;

                    case EAutoRunStatus.STS_RUN: // auto run

                        // Do Thread Step
                        switch (ThreadStep)
                        {
                            case TRS_SPINNER_MOVETO_WAIT_POS:
                                iResult = m_RefComp.ctrlSpinner.TableDown();
                                if(iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                SetStep(TRS_SPINNER_WAITFOR_MESSAGE);
                                break;

                            case TRS_SPINNER_WAITFOR_MESSAGE:
                                // 0. check default status;

                                // 1. if wafer detected
                                iResult = m_RefComp.ctrlSpinner.IsObjectDetected(out bStatus);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                tPhase = GetMyNextWorkPhase();
                                if (bStatus) // if wafer detected
                                {
                                    if(tPhase == EProcessPhase.COATER_LOAD)
                                    {
                                        GetMyWorkPiece().StartPhase(tPhase);
                                        SetStep(TRS_SPINNER_DO_PRE_COAT);
                                    } else if (tPhase == EProcessPhase.CLEANER_LOAD)
                                    {
                                        GetMyWorkPiece().StartPhase(tPhase);
                                        SetStep(TRS_SPINNER_DO_PRE_CLEAN);
                                    }
                                    else if (tPhase == EProcessPhase.COATER_WAIT_UNLOAD || tPhase == EProcessPhase.CLEANER_WAIT_UNLOAD)
                                    {
                                        if (GetMyWorkPiece().Process[(int)tPhase].IsStarted == false)
                                        {
                                            GetMyWorkPiece().StartPhase(tPhase); // 대기 시작
                                        }

                                        if (TInterface.PushPull_Spinner_BeginHandshake_Load[(int)m_Data.SpinnerIndex] == true)
                                        {
                                            GetMyWorkPiece().FinishPhase(tPhase); // 대기 종료
                                            if (tPhase == EProcessPhase.COATER_WAIT_UNLOAD)
                                            {
                                                tPhase = EProcessPhase.COATER_UNLOAD_TO_PUSHPULL;
                                            }
                                            else
                                            {
                                                tPhase = EProcessPhase.CLEANER_UNLOAD_TO_PUSHPULL;
                                            }
                                            GetMyWorkPiece().StartPhase(tPhase);
                                            SetStep(TRS_SPINNER_UNLOADING_TO_PUSHPULL_ONESTEP);
                                        }
                                    }
                                    else
                                    {
                                        // Test 할 동안 임시로 막아놓음
                                        //ReportAlarm(GenerateErrorCode(ERR_TRS_SPINNER_NEXT_PROCESS_IS_ABNORMAL));
                                    }
                                } else // if not
                                {
                                    if(TInterface.PushPull_Spinner_BeginHandshake_Unload[(int)m_Data.SpinnerIndex] == true)
                                        SetStep(TRS_SPINNER_LOADING_FROM_PUSHPULL_ONESTEP);
                                }

                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process load with pushpull
                            case TRS_SPINNER_LOADING_FROM_PUSHPULL_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_SPINNER_BEGIN_LOADING_FROM_PUSHPULL);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.PUSHPULL;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                TInterface.Spinner_PushPull_BeginHandshake_Load[(int)m_Data.SpinnerIndex] = true;

                                // step1
                                while (TInterface.PushPull_Spinner_UnloadStep1[(int)m_Data.SpinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_SPINNER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                iResult = m_RefComp.ctrlSpinner.TableUp();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                iResult = m_RefComp.ctrlSpinner.Absorb();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.Spinner_PushPull_LoadStep1[(int)m_Data.SpinnerIndex] = true;

                                // step2
                                while (TInterface.PushPull_Spinner_UnloadStep2[(int)m_Data.SpinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_SPINNER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                iResult = m_RefComp.ctrlSpinner.TableDown();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                TInterface.Spinner_PushPull_LoadStep2[(int)m_Data.SpinnerIndex] = true;

                                // finish
                                while (TInterface.PushPull_Spinner_FinishHandshake_Unload[(int)m_Data.SpinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_SPINNER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                TInterface.Spinner_PushPull_FinishHandshake_Load[(int)m_Data.SpinnerIndex] = true;
                                Sleep(TInterface.TimeKeepOn);

                                // reset
                                TInterface.ResetInterface(TSelf);
                                SetStep(TRS_SPINNER_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process unload with pushpull
                            case TRS_SPINNER_UNLOADING_TO_PUSHPULL_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_SPINNER_BEGIN_UNLOADING_TO_PUSHPULL);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.PUSHPULL;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                TInterface.Spinner_PushPull_BeginHandshake_Unload[(int)m_Data.SpinnerIndex] = true;

                                // step1
                                while (TInterface.PushPull_Spinner_LoadStep1[(int)m_Data.SpinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_SPINNER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                iResult = m_RefComp.ctrlSpinner.TableUp();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.Spinner_PushPull_UnloadStep1[(int)m_Data.SpinnerIndex] = true;

                                // step2
                                while (TInterface.PushPull_Spinner_LoadStep2[(int)m_Data.SpinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_SPINNER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                iResult = m_RefComp.ctrlSpinner.Release();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                iResult = m_RefComp.ctrlSpinner.TableDown();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                TInterface.Spinner_PushPull_UnloadStep2[(int)m_Data.SpinnerIndex] = true;

                                // finish
                                while (TInterface.PushPull_Spinner_FinishHandshake_Load[(int)m_Data.SpinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_SPINNER_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                GetMyWorkPiece().FinishPhase(tPhase);
                                TInterface.Spinner_PushPull_FinishHandshake_Unload[(int)m_Data.SpinnerIndex] = true;
                                Sleep(TInterface.TimeKeepOn);

                                // reset
                                TInterface.ResetInterface(TSelf);
                                SetStep(TRS_SPINNER_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // coating
                            case TRS_SPINNER_DO_PRE_COAT:               // do work if it is needed.
                                GetMyWorkPiece().FinishPhase(EProcessPhase.COATER_LOAD);
                                GetMyWorkPiece().StartPhase(EProcessPhase.PRE_COATING);
                                SetStep(TRS_SPINNER_DO_AFTER_PRE_COAT);
                                break;

                            case TRS_SPINNER_DO_AFTER_PRE_COAT:
                                GetMyWorkPiece().FinishPhase(EProcessPhase.PRE_COATING);
                                SetStep(TRS_SPINNER_DO_COAT);
                                break;

                            case TRS_SPINNER_DO_COAT:                    // do work if it is needed.
                                GetMyWorkPiece().StartPhase(EProcessPhase.COATING);

                                // do coat

                                GetMyWorkPiece().FinishPhase(EProcessPhase.COATING);
                                SetStep(TRS_SPINNER_DO_POST_COAT);
                                break;

                            case TRS_SPINNER_DO_POST_COAT:
                                GetMyWorkPiece().StartPhase(EProcessPhase.POST_COATING);
                                SetStep(TRS_SPINNER_DO_AFTER_POST_COAT);
                                break;

                            case TRS_SPINNER_DO_AFTER_POST_COAT:

                                GetMyWorkPiece().FinishPhase(EProcessPhase.POST_COATING);
                                SetStep(TRS_SPINNER_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // cleaning
                            case TRS_SPINNER_DO_PRE_CLEAN:               // do work if it is needed.
                                GetMyWorkPiece().FinishPhase(EProcessPhase.CLEANER_LOAD);
                                GetMyWorkPiece().StartPhase(EProcessPhase.PRE_CLEANING);
                                SetStep(TRS_SPINNER_DO_AFTER_PRE_CLEAN);
                                break;

                            case TRS_SPINNER_DO_AFTER_PRE_CLEAN:
                                GetMyWorkPiece().FinishPhase(EProcessPhase.PRE_CLEANING);
                                SetStep(TRS_SPINNER_DO_CLEAN);
                                break;

                            case TRS_SPINNER_DO_CLEAN:                    // do work if it is needed.
                                GetMyWorkPiece().StartPhase(EProcessPhase.CLEANING);

                                // do clean

                                GetMyWorkPiece().FinishPhase(EProcessPhase.CLEANING);
                                SetStep(TRS_SPINNER_DO_POST_CLEAN);
                                break;

                            case TRS_SPINNER_DO_POST_CLEAN:
                                GetMyWorkPiece().StartPhase(EProcessPhase.POST_CLEANING);
                                SetStep(TRS_SPINNER_DO_AFTER_POST_CLEAN);
                                break;

                            case TRS_SPINNER_DO_AFTER_POST_CLEAN:

                                GetMyWorkPiece().FinishPhase(EProcessPhase.POST_CLEANING);
                                SetStep(TRS_SPINNER_WAITFOR_MESSAGE);
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

        public new int PostMsg(EThreadChannel target, int msg, int wParam = -1, int lParam = -1)
        {
            if(m_Data.SpinnerIndex == ESpinnerIndex.SPINNER1)
            {
                ; // class 본문에선 spinner1 기준으로 작성해놓고
            }
            else
            {
                //msg += 100; // gap between spinner 1 and spinner2
            }

            return base.PostMsg(target, msg, wParam, lParam);
        }

    }
}
