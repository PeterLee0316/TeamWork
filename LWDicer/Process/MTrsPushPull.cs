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
        public bool ThreadHandshake_byOneStep = true;

        public bool UseSpinnerSeparately   = true;  // spinner를 coater, cleaner로 구분하여 사용할지 여부
        public ESpinnerIndex UCoaterIndex  = ESpinnerIndex.SPINNER1;  // spinner를 구분지어 사용할 때, coater의 spinner index
        public ESpinnerIndex UCleanerIndex = ESpinnerIndex.SPINNER2; // spinner를 구분지어 사용할 때, cleaner의 spinner index
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

        public override int Initialize()
        {
            // Do initialize
            InitializeMsg();
            InitializeInterface();

            // Do Action
            int iResult = m_RefComp.ctrlPushPull.Initialize();
            if (iResult != SUCCESS) return iResult;

            EThreadStep iStep1 = TRS_PUSHPULL_WAITFOR_MESSAGE;

            // finally
            SetStep(iStep1);

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
            EProcessPhase tPhase = GetMyNextWorkPhase();
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
                            case TRS_PUSHPULL_MOVETO_WAIT_POS:
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            case TRS_PUSHPULL_WAITFOR_MESSAGE:

                                // 0. check default status;

                                // 1. if wafer detected
                                iResult = m_RefComp.ctrlPushPull.IsObjectDetected(out bStatus);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                // wafer의 다음 해야할 일을 보고, 해당 unit에게 unload하는 분기점으로 이동시킨다.
                                tPhase = GetMyNextWorkPhase();
                                if (bStatus)
                                {
                                    if(tPhase == EProcessPhase.PUSHPULL_UNLOAD_TO_COATER)
                                    {
                                        if (m_Data.UseSpinnerSeparately) // spinner를 구분지어 사용한다면
                                        {
                                            iResult = m_RefComp.ctrlSpinner[(int)m_Data.UCoaterIndex].IsObjectDetected(out bStatus1);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                            if (bStatus1 == true)
                                            {
                                                // 원래는 대기해야하나, spinner가 각각 전용으로 쓰일 경우엔 wafer 정체가 일어나기때문에 에러 처리함.
                                                ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_OBJECT_DETECTED_ON_COATER));
                                                break;
                                            }

                                            if(m_Data.UCoaterIndex == ESpinnerIndex.SPINNER1)
                                                SetStep(TRS_PUSHPULL_UNLOADING_TO_SPINNER1_ONESTEP);
                                            else SetStep(TRS_PUSHPULL_UNLOADING_TO_SPINNER2_ONESTEP);
                                        }
                                        else
                                        {
                                            iResult = m_RefComp.ctrlSpinner[(int)ESpinnerIndex.SPINNER1].IsObjectDetected(out bStatus1);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                            iResult = m_RefComp.ctrlSpinner[(int)ESpinnerIndex.SPINNER2].IsObjectDetected(out bStatus2);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                            if(bStatus1 && bStatus2) // spinner가 꽉 찼다면 역시 에러
                                            {
                                                ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_ALL_SPINNER_IS_FULL));
                                                break;
                                            }

                                            if(bStatus1 == false)
                                            {
                                                SetStep(TRS_PUSHPULL_UNLOADING_TO_SPINNER1_ONESTEP);
                                            } else if (bStatus2 == false)
                                            {
                                                SetStep(TRS_PUSHPULL_UNLOADING_TO_SPINNER2_ONESTEP);
                                            }
                                        }
                                    } else if (tPhase == EProcessPhase.PUSHPULL_UNLOAD_TO_CLEANER)
                                    {
                                        if (m_Data.UseSpinnerSeparately) // spinner를 구분지어 사용한다면
                                        {
                                            iResult = m_RefComp.ctrlSpinner[(int)m_Data.UCleanerIndex].IsObjectDetected(out bStatus1);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                            if (bStatus1 == true)
                                            {
                                                // 원래는 대기해야하나, spinner가 각각 전용으로 쓰일 경우엔 에러 처리가 맞는듯.
                                                ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_OBJECT_DETECTED_ON_CLEANER));
                                                break;
                                            }

                                            if (m_Data.UCleanerIndex == ESpinnerIndex.SPINNER1)
                                                SetStep(TRS_PUSHPULL_UNLOADING_TO_SPINNER1_ONESTEP);
                                            else SetStep(TRS_PUSHPULL_UNLOADING_TO_SPINNER2_ONESTEP);
                                        }
                                        else
                                        {
                                            iResult = m_RefComp.ctrlSpinner[(int)ESpinnerIndex.SPINNER1].IsObjectDetected(out bStatus1);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                            iResult = m_RefComp.ctrlSpinner[(int)ESpinnerIndex.SPINNER2].IsObjectDetected(out bStatus2);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                            if (bStatus1 && bStatus2) // spinner가 꽉 찼다면 역시 에러
                                            {
                                                ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_ALL_SPINNER_IS_FULL));
                                                break;
                                            }

                                            if (bStatus1 == false)
                                            {
                                                SetStep(TRS_PUSHPULL_UNLOADING_TO_SPINNER1_ONESTEP);
                                            }
                                            else if (bStatus2 == false)
                                            {
                                                SetStep(TRS_PUSHPULL_UNLOADING_TO_SPINNER2_ONESTEP);
                                            }
                                        }
                                    }
                                    else if (tPhase == EProcessPhase.PUSHPULL_UNLOAD_TO_LOADER)
                                    {
                                        if (m_RefComp.ctrlLoader.GetEmptySlotCount() > 0)
                                            SetStep(TRS_PUSHPULL_UNLOADING_TO_LOADER_ONESTEP);
                                    }
                                    else if (tPhase == EProcessPhase.PUSHPULL_UNLOAD_TO_HANDLER)
                                    {
                                        SetStep(TRS_PUSHPULL_UNLOADING_TO_HANDLER_ONESTEP);
                                    }
                                    break;
                                }
                                // 2. if wafer not detected
                                else
                                {
                                    bool bStepBreak = false;

                                    if(m_Data.UseSpinnerSeparately) // spinner를 구분지어 사용한다면
                                    {
                                        // 2.1 coater에 작업 완료된 wafer가 있고, load handler가 비어있다면
                                        iResult = m_RefComp.ctrlSpinner[(int)m_Data.UCoaterIndex].IsObjectDetected(out bStatus1);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                        iResult = m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER, out bStatus2);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                        if(bStatus1 == true && bStatus2 == false)
                                        {
                                            if (GetWorkPiece(m_Data.UCoaterIndex).GetNextPhase() == EProcessPhase.COATER_WAIT_UNLOAD)
                                            {
                                                bStepBreak = true;
                                                if (m_Data.UCoaterIndex == ESpinnerIndex.SPINNER1)
                                                    SetStep(TRS_PUSHPULL_LOADING_FROM_SPINNER1_ONESTEP);
                                                else SetStep(TRS_PUSHPULL_LOADING_FROM_SPINNER2_ONESTEP);
                                                break;
                                            }
                                        }
                                        if (bStepBreak == true) break; // for break switch case

                                        // 2.2 cleaner에 작업 완료된 wafer가 있고, loader가 비어있다면
                                        iResult = m_RefComp.ctrlSpinner[(int)m_Data.UCleanerIndex].IsObjectDetected(out bStatus1);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                        nSlotCount = m_RefComp.ctrlLoader.GetEmptySlotCount();

                                        if (bStatus1 == true && nSlotCount > 0)
                                        {
                                            if (GetWorkPiece(m_Data.UCleanerIndex).GetNextPhase() == EProcessPhase.CLEANER_WAIT_UNLOAD)
                                            {
                                                bStepBreak = true;
                                                if (m_Data.UCleanerIndex == ESpinnerIndex.SPINNER1)
                                                    SetStep(TRS_PUSHPULL_LOADING_FROM_SPINNER1_ONESTEP);
                                                else SetStep(TRS_PUSHPULL_LOADING_FROM_SPINNER2_ONESTEP);
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
                                            SetStep(TRS_PUSHPULL_LOADING_FROM_HANDLER_ONESTEP);
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
                                            SetStep(TRS_PUSHPULL_LOADING_FROM_LOADER_ONESTEP);
                                            break;
                                        }
                                        if (bStepBreak == true) break; // for break switch case
                                    }
                                    else // spinner를 공용으로 사용한다면
                                    {
                                        // 2.0.1 get spinners empty count
                                        int nEmptyCount_Spinner = 0;

                                        // 2.1 spinner가 unload ready 상태라면,
                                        for (int i = 0; i < (int)ESpinnerIndex.MAX; i++)
                                        {
                                            iResult = m_RefComp.ctrlSpinner[i].IsObjectDetected(out bStatus);
                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                            if (bStatus == false)
                                            {
                                                nEmptyCount_Spinner++;
                                                continue;
                                            }

                                            tPhase = GetWorkPiece(i == 0 ? ESpinnerIndex.SPINNER1 : ESpinnerIndex.SPINNER2).GetNextPhase();
                                            // 2.1.1 spinner에 작업 완료된 wafer가 있고
                                            if (tPhase == EProcessPhase.COATER_WAIT_UNLOAD) // if coater
                                            {
                                                // 2.1.1.1 handler에 빈자리가 있는지 확인
                                                iResult = m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER, out bStatus2);
                                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                                if (bStatus2 == true) continue;

                                                bStepBreak = true;
                                                if (i == (int)ESpinnerIndex.SPINNER1)
                                                    SetStep(TRS_PUSHPULL_LOADING_FROM_SPINNER1_ONESTEP);
                                                else SetStep(TRS_PUSHPULL_LOADING_FROM_SPINNER2_ONESTEP);
                                                break;
                                            }
                                            else if(tPhase == EProcessPhase.CLEANER_WAIT_UNLOAD) // if cleaner
                                            {
                                                // 2.1.1.1 loader에 빈자리가 있는지 확인
                                                nSlotCount = m_RefComp.ctrlLoader.GetEmptySlotCount();
                                                if (nSlotCount <= 0) continue;

                                                bStepBreak = true;
                                                if (i == (int)ESpinnerIndex.SPINNER1)
                                                    SetStep(TRS_PUSHPULL_LOADING_FROM_SPINNER1_ONESTEP);
                                                else SetStep(TRS_PUSHPULL_LOADING_FROM_SPINNER2_ONESTEP);
                                                break;
                                            }
                                        }
                                        if (bStepBreak == true) break; // for break switch case

                                        // 2.2 unload handler에 wafer가 있고, cleaner가 비어있다면
                                        iResult = m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER, out bStatus2);
                                        if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                        if (bStatus2 == true && nEmptyCount_Spinner > 0)
                                        {
                                            bStepBreak = true;
                                            SetStep(TRS_PUSHPULL_LOADING_FROM_HANDLER_ONESTEP);
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
                                                Sleep(1000); // for test

                                                bStepBreak = true;
                                                TInterface.ResetInterface(TSelf);
                                                SetStep(TRS_PUSHPULL_LOADING_FROM_LOADER_ONESTEP);
                                                break;
                                            }
                                        }
                                        if (bStepBreak == true) break; // for break switch case
                                    }
                                }
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process load with loader
                            case TRS_PUSHPULL_LOADING_FROM_LOADER_ONESTEP:
                                // branch
                                if(m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_PUSHPULL_BEGIN_LOADING_FROM_LOADER);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.LOADER;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                LoadWorkPieceFromCassette();
                                GetMyWorkPiece().StartPhase(EProcessPhase.PUSHPULL_LOAD_FROM_LOADER);

                                TInterface.PushPull_Loader_BeginHandshake_Load = true;
                                while(TInterface.Loader_PushPull_BeginHandshake_Unload == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1
                                while (TInterface.Loader_PushPull_UnloadStep1 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                iResult = m_RefComp.ctrlPushPull.MoveToLoaderPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                iResult = m_RefComp.ctrlPushPull.GripLock();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Loader_LoadStep1 = true;

                                // step2
                                while (TInterface.Loader_PushPull_UnloadStep2 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Loader_LoadStep2 = true;

                                // finish
                                while (TInterface.Loader_PushPull_FinishHandshake_Unload == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                GetMyWorkPiece().FinishPhase(EProcessPhase.PUSHPULL_LOAD_FROM_LOADER);
                                TInterface.PushPull_Loader_FinishHandshake_Load = true;
                                Sleep(TInterface.TimeKeepOn);

                                // reset
                                TInterface.ResetInterface(TSelf);

                                SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;
                            /*                                
                                                        case TRS_PUSHPULL_BEGIN_LOADING_FROM_LOADER: // grip release, send request unload signal
                                                            PostMsg_Interval(TrsLoader, MSG_PUSHPULL_LOADER_REQUEST_UNLOADING);
                                                            if (m_bLoader_StartUnloading == false) break;

                                                            iResult = m_RefComp.ctrlPushPull.GripRelease();
                                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                                            PostMsg(TrsLoader, MSG_PUSHPULL_LOADER_START_LOADING);
                                                            SetStep(TRS_PUSHPULL_LOAD_FROM_LOADER);
                                                            break;

                                                        case TRS_PUSHPULL_LOAD_FROM_LOADER: // move to load pos, vacuum absorb, send load ready signal
                                                            iResult = m_RefComp.ctrlPushPull.MoveToLoaderPos(false);
                                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                                            iResult = m_RefComp.ctrlPushPull.GripLock();
                                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                                            PostMsg(TrsLoader, MSG_PUSHPULL_LOADER_COMPLETE_ABSORB);
                                                            SetStep(TRS_PUSHPULL_WAITFOR_LOADER_UNLOAD_COMPLETE);
                                                            break;

                                                        case TRS_PUSHPULL_WAITFOR_LOADER_UNLOAD_COMPLETE: // wait for response signal
                                                            PostMsg_Interval(TrsLoader, MSG_PUSHPULL_LOADER_COMPLETE_ABSORB);
                                                            if (m_bLoader_ReadyUnloading == false) break;

                                                            // do something..

                                                            SetStep(TRS_PUSHPULL_FINISH_LOADING_FROM_LOADER);
                                                            break;

                                                        case TRS_PUSHPULL_FINISH_LOADING_FROM_LOADER: // move to wait pos: send load complete signal
                                                            iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(true);
                                                            if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                                            PostMsg(TrsLoader, MSG_PUSHPULL_LOADER_COMPLETE_LOADING);
                                                            SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
                                                            break;
                            */
                            ///////////////////////////////////////////////////////////////////
                            // process unload with loader
                            case TRS_PUSHPULL_UNLOADING_TO_LOADER_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_PUSHPULL_BEGIN_UNLOADING_TO_LOADER);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.LOADER;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                GetMyWorkPiece().StartPhase(EProcessPhase.PUSHPULL_UNLOAD_TO_LOADER);

                                TInterface.PushPull_Loader_BeginHandshake_Unload = true;
                                while (TInterface.Loader_PushPull_BeginHandshake_Load == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1
                                while (TInterface.Loader_PushPull_LoadStep1 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                iResult = m_RefComp.ctrlPushPull.MoveToLoaderPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Loader_UnloadStep1 = true;

                                // step2
                                while (TInterface.Loader_PushPull_LoadStep2 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                iResult = m_RefComp.ctrlPushPull.GripRelease();
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Loader_UnloadStep2 = true;

                                // finish
                                while (TInterface.Loader_PushPull_FinishHandshake_Load == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                GetMyWorkPiece().FinishPhase(EProcessPhase.PUSHPULL_UNLOAD_TO_LOADER);
                                TInterface.PushPull_Loader_FinishHandshake_Unload = true;
                                Sleep(TInterface.TimeKeepOn);

                                // reset
                                TInterface.ResetInterface(TSelf);
                                UnloadWorkPieceToCassette();

                                SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process load with spinner1
                            case TRS_PUSHPULL_LOADING_FROM_SPINNER1_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_PUSHPULL_BEGIN_LOADING_FROM_SPINNER1);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.SPINNER1;
                                spinnerIndex = (int)ESpinnerIndex.SPINNER1;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                // tPhase는 wafer의 소유권이 있는 곳에서 관리
                                //tPhase = GetMyNextWorkPhase();
                                //GetMyWorkPiece().StartPhase(tPhase);

                                TInterface.PushPull_Spinner_BeginHandshake_Load[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_BeginHandshake_Unload[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1
                                iResult = m_RefComp.ctrlPushPull.MoveToSpinner1Pos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                // guide open

                                TInterface.PushPull_Spinner_LoadStep1[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_UnloadStep1[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                // guide close

                                TInterface.PushPull_Spinner_LoadStep2[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_UnloadStep2[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Spinner_FinishHandshake_Load[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_FinishHandshake_Unload[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);
                                //GetMyWorkPiece().FinishPhase(tPhase);
                                DataManager.ChangeWorkPieceUnit(ELCNetUnitPos.SPINNER1, ELCNetUnitPos.PUSHPULL);

                                SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process unload with spinner1
                            case TRS_PUSHPULL_UNLOADING_TO_SPINNER1_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_PUSHPULL_BEGIN_UNLOADING_TO_SPINNER1);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.SPINNER1;
                                spinnerIndex = (int)ESpinnerIndex.SPINNER1;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                tPhase = GetMyNextWorkPhase();
                                GetMyWorkPiece().StartPhase(tPhase);

                                TInterface.PushPull_Spinner_BeginHandshake_Unload[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_BeginHandshake_Load[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1
                                iResult = m_RefComp.ctrlPushPull.MoveToSpinner1Pos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Spinner_UnloadStep1[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_LoadStep1[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                // guide open

                                TInterface.PushPull_Spinner_UnloadStep2[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_LoadStep2[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                // guide close
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Spinner_FinishHandshake_Unload[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_FinishHandshake_Load[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);
                                GetMyWorkPiece().FinishPhase(tPhase);
                                DataManager.ChangeWorkPieceUnit(ELCNetUnitPos.PUSHPULL, ELCNetUnitPos.SPINNER1);

                                SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process load with spinner2
                            case TRS_PUSHPULL_LOADING_FROM_SPINNER2_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_PUSHPULL_BEGIN_LOADING_FROM_SPINNER2);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.SPINNER2;
                                spinnerIndex = (int)ESpinnerIndex.SPINNER2;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                // tPhase는 wafer의 소유권이 있는 곳에서 관리
                                //tPhase = GetMyNextWorkPhase();
                                //GetMyWorkPiece().StartPhase(tPhase);

                                TInterface.PushPull_Spinner_BeginHandshake_Load[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_BeginHandshake_Unload[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1
                                iResult = m_RefComp.ctrlPushPull.MoveToSpinner2Pos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                // guide open

                                TInterface.PushPull_Spinner_LoadStep1[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_UnloadStep1[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                // guide close

                                TInterface.PushPull_Spinner_LoadStep2[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_UnloadStep2[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Spinner_FinishHandshake_Load[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_FinishHandshake_Unload[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);
                                //GetMyWorkPiece().FinishPhase(tPhase);
                                DataManager.ChangeWorkPieceUnit(ELCNetUnitPos.SPINNER2, ELCNetUnitPos.PUSHPULL);

                                SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process unload with spinner2
                            case TRS_PUSHPULL_UNLOADING_TO_SPINNER2_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_PUSHPULL_BEGIN_UNLOADING_TO_SPINNER2);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.SPINNER2;
                                spinnerIndex = (int)ESpinnerIndex.SPINNER2;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                tPhase = GetMyNextWorkPhase();
                                GetMyWorkPiece().StartPhase(tPhase);

                                TInterface.PushPull_Spinner_BeginHandshake_Unload[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_BeginHandshake_Load[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1
                                iResult = m_RefComp.ctrlPushPull.MoveToSpinner2Pos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Spinner_UnloadStep1[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_LoadStep1[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                // guide open

                                TInterface.PushPull_Spinner_UnloadStep2[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_LoadStep2[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                // guide close
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Spinner_FinishHandshake_Unload[spinnerIndex] = true;
                                while (TInterface.Spinner_PushPull_FinishHandshake_Load[spinnerIndex] == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);
                                GetMyWorkPiece().FinishPhase(tPhase);
                                DataManager.ChangeWorkPieceUnit(ELCNetUnitPos.PUSHPULL, ELCNetUnitPos.SPINNER2);

                                SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process load with handler
                            case TRS_PUSHPULL_LOADING_FROM_HANDLER_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_PUSHPULL_BEGIN_LOADING_FROM_HANDLER);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.HANDLER;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                // tPhase는 wafer의 소유권이 있는 곳에서 관리
                                //tPhase = GetMyNextWorkPhase();
                                //GetMyWorkPiece().StartPhase(tPhase);

                                TInterface.PushPull_Handler_BeginHandshake_Load = true;
                                while (TInterface.Handler_PushPull_BeginHandshake_Unload == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1
                                iResult = m_RefComp.ctrlPushPull.MoveToHandlerPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }
                                // guide open

                                TInterface.PushPull_Handler_LoadStep1 = true;
                                while (TInterface.Handler_PushPull_UnloadStep1 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                // guide close

                                TInterface.PushPull_Handler_LoadStep2 = true;
                                while (TInterface.Handler_PushPull_UnloadStep2 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Handler_FinishHandshake_Load = true;
                                while (TInterface.Handler_PushPull_FinishHandshake_Unload == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);
                                //GetMyWorkPiece().FinishPhase(tPhase);
                                DataManager.ChangeWorkPieceUnit(ELCNetUnitPos.LOWER_HANDLER, ELCNetUnitPos.PUSHPULL);

                                SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process unload with handler
                            case TRS_PUSHPULL_UNLOADING_TO_HANDLER_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_PUSHPULL_BEGIN_UNLOADING_TO_HANDLER);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.HANDLER;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                tPhase = GetMyNextWorkPhase();
                                GetMyWorkPiece().StartPhase(tPhase);

                                TInterface.PushPull_Handler_BeginHandshake_Unload = true;
                                while (TInterface.Handler_PushPull_BeginHandshake_Load == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1
                                iResult = m_RefComp.ctrlPushPull.MoveToHandlerPos(true);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Handler_UnloadStep1 = true;
                                while (TInterface.Handler_PushPull_LoadStep1 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                // guide open

                                TInterface.PushPull_Handler_UnloadStep2 = true;
                                while (TInterface.Handler_PushPull_LoadStep2 == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                // guide close
                                iResult = m_RefComp.ctrlPushPull.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.PushPull_Handler_FinishHandshake_Unload = true;
                                while (TInterface.Handler_PushPull_FinishHandshake_Load == false)
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
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_PUSHPULL_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);
                                GetMyWorkPiece().FinishPhase(tPhase);
                                DataManager.ChangeWorkPieceUnit(ELCNetUnitPos.PUSHPULL, ELCNetUnitPos.UPPER_HANDLER);

                                SetStep(TRS_PUSHPULL_WAITFOR_MESSAGE);
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
