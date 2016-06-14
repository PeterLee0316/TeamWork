﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.ETrsPushPullStep;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.EThreadChannel;
using static LWDicer.Control.DEF_Thread.EAutoRunMode;
using static LWDicer.Control.DEF_Thread.EAutoRunStatus;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_LCNet;

using static LWDicer.Control.DEF_CtrlPushPull;
using static LWDicer.Control.DEF_CtrlLoader;
using static LWDicer.Control.DEF_CtrlSpinner;
using static LWDicer.Control.DEF_CtrlHandler;

namespace LWDicer.Control
{
    public class CTrsPushPullRefComp
    {
        public MCtrlPushPull ctrlPushPull;

        public override string ToString()
        {
            return $"CTrsPushPullRefComp : {this}";
        }
    }

    public class CTrsPushPullData
    {
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
            int iStep1 = (int)TRS_PUSHPULL_WAITFOR_MESSAGE;

            // finally
            SetStep1(iStep1);

            return SUCCESS;
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
            Debug.WriteLine($"{ToString()} received message : {evnt}");

            // spinner index 정리
            int index = (int)ESpinnerIndex.SPINNER1;
            if(evnt.wParam == (int)EThreadChannel.TrsSpinner2) index = (int)ESpinnerIndex.SPINNER2;

            switch (evnt.Msg)
            {
                // if need to change response for common message, then add case state here.
                default:
                    base.ProcessMsg(evnt);
                    break;

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
            return DEF_Error.SUCCESS;
        }

        protected override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bStatus = false;
            EProcessPhase processPhase;
            int spinnerIndex;

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
                        //m_RefComp.ctrlPushPull.SetAutoManual(EAutoManual.MANUAL);
                        break;

                    case STS_ERROR_STOP: // Error Stop
                        break;

                    case STS_STEP_STOP: // Step Stop
                        break;

                    case STS_RUN_READY: // Run Ready
                        break;

                    case STS_CYCLE_STOP: // Cycle Stop
                        //if (ThreadStep1 == TRS_LOADER_MOVETO_LOAD)
                        break;

                    case STS_RUN: // auto run
                        //m_RefComp.ctrlPushPull.SetAutoManual(EAutoManual.AUTO);

                        // Do Thread Step
                        switch (ThreadStep1)
                        {
                            case (int)TRS_PUSHPULL_WAITFOR_MESSAGE:

                                // 0. check default status;

                                // 1. if detected wafer
                                m_RefComp.ctrlPushPull.IsObjectDetected(out bStatus);
                                if (bStatus)
                                {
                                    // wafer의 다음 해야할 일을 보고, 해당 unit에게 unload하는 분기점으로 이동시킨다.
                                    GetMyWorkPiece().GetNextPhase(out processPhase);

                                    switch (processPhase)
                                    {
                                        // unload to coater
                                        case EProcessPhase.PUSHPULL_UNLOAD_TO_COATER:
                                            //SetStep1((int)TRS_PUSHPULL_REQUEST_COATER_LOADING);
                                            break;

                                        // unload to handler
                                        case EProcessPhase.PUSHPULL_UNLOAD_TO_HANDLER:
                                            SetStep1((int)TRS_PUSHPULL_REQUEST_HANDLER_LOADING);
                                            break;

                                        // unload to cleaner
                                        case EProcessPhase.PUSHPULL_UNLOAD_TO_CLEANER:
                                            //SetStep1((int)TRS_PUSHPULL_REQUEST_CLEANER_LOADING);
                                            break;

                                        // unload to loader
                                        case EProcessPhase.PUSHPULL_UNLOAD_TO_LOADER:
                                            SetStep1((int)TRS_PUSHPULL_REQUEST_LOADER_LOADING);
                                            break;
                                    }
                                }
                                // 2. if not detected wafer
                                else
                                {
                                    bool bStepBreak = false;
                                    // 2.0.1 get spinners empty count
                                    int nEmptyCount_Spinner = 0; 

                                    // 2.1 spinner가 unload ready 상태라면,
                                    for(int i = 0; i < m_bSpinner_ReadyUnloading.Length; i++)
                                    {
                                        if(m_bSpinner_ReadyUnloading[i] == true)
                                        {
                                            // 2.1.1 spinner의 cleaning공정이 끝났다면 바로 -> loader 진행
                                            if(GetWorkPiece((int)ELCNetUnitPos.SPINNER1 + i).GetNextPhase() == (int)EProcessPhase.CLEANER_UNLOAD)
                                            {
                                                bStepBreak = true;
                                                spinnerIndex = i;
                                                SetStep1((int)TRS_PUSHPULL_LOADING_FROM_SPINNER);
                                                break;
                                            }

                                            // 2.1.2 Spinner의 coating 공정이 끝났고, Upper Handler가 비었다면 바로 -> handler 진행
                                            // Handler의 상태를 보는 이유는 정체를 피하기 위해서
                                            if (GetWorkPiece((int)ELCNetUnitPos.SPINNER1 + i).GetNextPhase() == (int)EProcessPhase.COATER_UNLOAD
                                                && m_bUpperHandler_WaitLoadingStart == true)
                                            {
                                                bStepBreak = true;
                                                spinnerIndex = i;
                                                SetStep1((int)TRS_PUSHPULL_LOADING_FROM_SPINNER);
                                                break;
                                            }
                                        }
                                    }
                                    if (bStepBreak == true) break; // for break switch case

                                    // 2.2 Unload Handler가 Unload Ready 상태라면
                                    if (m_bLowerHandler_WaitUnloadingStart == true)
                                    {
                                        // 2.2.1 Spinner에 빈자리가 있다면
                                        if (nEmptyCount_Spinner > 0)
                                        {
                                            bStepBreak = true;
                                            SetStep1((int)TRS_PUSHPULL_LOADING_FROM_HANDLER);
                                            break;
                                        }
                                    }
                                    if (bStepBreak == true) break; // for break switch case

                                    // 2.3 Spinner가 두군데 모두 비어있다면, Coater작업을 위한 zone은 확보되어있으므로
                                    if (nEmptyCount_Spinner > 1)
                                    {
                                        // Loader에 빈 slot이 있는지는 상세 스텝에서 질의 하는것이 맞을듯함.
                                        bStepBreak = true;
                                        SetStep1((int)TRS_PUSHPULL_LOADING_FROM_LOADER);
                                        break;
                                    }
                                    if (bStepBreak == true) break; // for break switch case

                                    // 2.4 Spinner가 한군데 비어있고, Loader가 비어있다면

                                }

                                break;

                                ///////////////////////////////////////////////////
                                // transfer wafer to loader from wafer
                            //case (int)TRS_PUSHPULL_REQUEST_LOADER_LOADING:
                            //    PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING);

                            //    SetStep1((int)TRS_PUSHPULL_WAITFOR_LOADER_READY_LOADING);
                            //    break;

                            //case (int)TRS_PUSHPULL_WAITFOR_LOADER_READY_LOADING:
                            //    if (m_bLoader_ReadyLoading == false) break;
                            //    m_bLoader_ReadyLoading = false;

                            //    SetStep1((int)TRS_PUSHPULL_WAITFOR_LOADER_COMPLETE_LOADING);
                            //    break;

                            //case (int)TRS_PUSHPULL_START_UNLOADING_TO_LOADER:
                            //    // move to loader

                            //    PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_START_UNLOADING);

                            //    SetStep1((int)TRS_PUSHPULL_WAITFOR_LOADER_COMPLETE_LOADING);
                            //    break;

                            //case (int)TRS_PUSHPULL_WAITFOR_LOADER_COMPLETE_LOADING:
                            //    PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING);

                            //    SetStep1((int)TRS_PUSHPULL_COMPLETE_UNLOADING_TO_LOADER);
                            //    break;

                            //case (int)TRS_PUSHPULL_COMPLETE_UNLOADING_TO_LOADER:
                            //    PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING);

                            //    SetStep1((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
                            //    break;

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
