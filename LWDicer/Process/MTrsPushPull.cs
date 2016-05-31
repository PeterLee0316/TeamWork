using System;
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
        bool m_bLoader_ReadyUnloading;
        bool m_bLoader_ReadyLoading;
        bool m_bLoader_AllWaferWorked;
        bool m_bLoader_StacksFull;

        bool m_bSpinner1_ReadyLoading;
        bool m_bSpinner1_StartLoading;
        bool m_bSpinner1_CompleteLoading;
        bool m_bSpinner1_ReadyUnloading;
        bool m_bSpinner1_StartUnloading;
        bool m_bSpinner1_CompleteUnloading;

        bool m_bSpinner2_ReadyLoading;
        bool m_bSpinner2_StartLoading;
        bool m_bSpinner2_CompleteLoading;
        bool m_bSpinner2_ReadyUnloading;
        bool m_bSpinner2_StartUnloading;
        bool m_bSpinner2_CompleteUnloading;

        // with handler
        bool m_bLHandler_WaitLoadingStart;
        bool m_bLHandler_StartLoading;
        bool m_bLHandler_RequestRelease;
        bool m_bLHandler_CompleteLoading;
        bool m_bUHandler_WaitUnloadingStart;
        bool m_bUHandler_StartUnloading;
        bool m_bUHandler_RequestAbsorb;
        bool m_bUHandler_CompleteUnloading;

        public MTrsPushPull(CObjectInfo objInfo, EThreadChannel SelfChannelNo,
            CTrsPushPullRefComp refComp, CTrsPushPullData data)
             : base(objInfo, SelfChannelNo)
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

            m_bSpinner1_ReadyLoading = false;
            m_bSpinner1_StartLoading = false;
            m_bSpinner1_CompleteLoading = false;
            m_bSpinner1_ReadyUnloading = false;
            m_bSpinner1_StartUnloading = false;
            m_bSpinner1_CompleteUnloading = false;

            m_bSpinner2_ReadyLoading = false;
            m_bSpinner2_StartLoading = false;
            m_bSpinner2_CompleteLoading = false;
            m_bSpinner2_ReadyUnloading = false;
            m_bSpinner2_StartUnloading = false;
            m_bSpinner2_CompleteUnloading = false;

            m_bLHandler_WaitLoadingStart = false;
            m_bLHandler_StartLoading = false;
            m_bLHandler_RequestRelease = false;
            m_bLHandler_CompleteLoading = false;
            m_bUHandler_WaitUnloadingStart = false;
            m_bUHandler_StartUnloading = false;
            m_bUHandler_RequestAbsorb = false;
            m_bUHandler_CompleteUnloading = false;

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

                // with Loader
                case (int)MSG_LOADER_PUSHPULL_WAIT_LOADING_START:
                    break;

                case (int)MSG_LOADER_PUSHPULL_START_LOADING:
                    break;

                case (int)MSG_LOADER_PUSHPULL_COMPLETE_LOADING:
                    break;

                case (int)MSG_LOADER_PUSHPULL_WAIT_UNLOADING_START:
                    break;

                case (int)MSG_LOADER_PUSHPULL_START_UNLOADING:
                    break;

                case (int)MSG_LOADER_PUSHPULL_COMPLETE_UNLOADING:
                    break;

                case (int)MSG_LOADER_PUSHPULL_ALL_WAFER_WORKED:
                    break;

                case (int)MSG_LOADER_PUSHPULL_STACKS_FULL:
                    break;


                //// with Spinner1
                //case (int)MSG_SPINNER1_PUSHPULL_READY_LOADING:
                //    m_bSpinner1_ReadyLoading = true;
                //    break;

                //case (int)MSG_SPINNER1_PUSHPULL_START_LOADING:
                //    m_bSpinner1_StartLoading = true;
                //    break;

                //case (int)MSG_SPINNER1_PUSHPULL_COMPLETE_LOADING:
                //    m_bSpinner1_CompleteLoading = true;
                //    break;

                //case (int)MSG_SPINNER1_PUSHPULL_READY_UNLOADING:
                //    m_bSpinner1_ReadyUnloading = true;
                //    break;

                //case (int)MSG_SPINNER1_PUSHPULL_START_UNLOADING:
                //    m_bSpinner1_StartUnloading = true;
                //    break;

                //case (int)MSG_SPINNER1_PUSHPULL_COMPLETE_UNLOADING:
                //    m_bSpinner1_CompleteUnloading = true;
                //    break;

                //// with Spinner2
                //case (int)MSG_SPINNER2_PUSHPULL_READY_LOADING:
                //    m_bSpinner2_ReadyLoading = true;
                //    break;

                //case (int)MSG_SPINNER2_PUSHPULL_START_LOADING:
                //    m_bSpinner2_StartLoading = true;
                //    break;

                //case (int)MSG_SPINNER2_PUSHPULL_COMPLETE_LOADING:
                //    m_bSpinner2_CompleteLoading = true;
                //    break;

                //case (int)MSG_SPINNER2_PUSHPULL_READY_UNLOADING:
                //    m_bSpinner2_ReadyUnloading = true;
                //    break;

                //case (int)MSG_SPINNER2_PUSHPULL_START_UNLOADING:
                //    m_bSpinner2_StartUnloading = true;
                //    break;

                //case (int)MSG_SPINNER2_PUSHPULL_COMPLETE_UNLOADING:
                //    m_bSpinner2_CompleteUnloading = true;
                //    break;

                // with Handler
                case (int)MSG_LHANDLER_PUSHPULL_WAIT_LOADING_START:
                    m_bLHandler_WaitLoadingStart = true;
                    m_bLHandler_StartLoading = false;
                    m_bLHandler_RequestRelease = false;
                    m_bLHandler_CompleteLoading = false;
                    break;

                case (int)MSG_LHANDLER_PUSHPULL_START_LOADING:
                    m_bLHandler_WaitLoadingStart = false;
                    m_bLHandler_StartLoading = true;
                    m_bLHandler_RequestRelease = false;
                    m_bLHandler_CompleteLoading = false;
                    break;

                case (int)MSG_LHANDLER_PUSHPULL_REQUEST_RELEASE:
                    m_bLHandler_WaitLoadingStart = false;
                    m_bLHandler_StartLoading = false;
                    m_bLHandler_RequestRelease = true;
                    m_bLHandler_CompleteLoading = false;
                    break;

                case (int)MSG_LHANDLER_PUSHPULL_COMPLETE_LOADING:
                    m_bLHandler_WaitLoadingStart = false;
                    m_bLHandler_StartLoading = false;
                    m_bLHandler_RequestRelease = false;
                    m_bLHandler_CompleteLoading = true;
                    break;

                case (int)MSG_UHANDLER_PUSHPULL_WAIT_UNLOADING_START:
                    m_bUHandler_WaitUnloadingStart = true;
                    m_bUHandler_StartUnloading = false;
                    m_bUHandler_RequestAbsorb = false;
                    m_bUHandler_CompleteUnloading = false;
                    break;

                case (int)MSG_UHANDLER_PUSHPULL_START_UNLOADING:
                    m_bUHandler_WaitUnloadingStart = false;
                    m_bUHandler_StartUnloading = true;
                    m_bUHandler_RequestAbsorb = false;
                    m_bUHandler_CompleteUnloading = false;
                    break;

                case (int)MSG_UHANDLER_PUSHPULL_REQUEST_ABSORB:
                    m_bUHandler_WaitUnloadingStart = false;
                    m_bUHandler_StartUnloading = false;
                    m_bUHandler_RequestAbsorb = true;
                    m_bUHandler_CompleteUnloading = false;
                    break;

                case (int)MSG_UHANDLER_PUSHPULL_COMPLETE_UNLOADING:
                    m_bUHandler_WaitUnloadingStart = false;
                    m_bUHandler_StartUnloading = false;
                    m_bUHandler_RequestAbsorb = false;
                    m_bUHandler_CompleteUnloading = true;
                    break;

            }
            return DEF_Error.SUCCESS;
        }

        protected override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bStatus = false;

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
                                    // 1.1 unload to loader
                                    //SetStep1((int)TRS_PUSHPULL_REQUEST_LOADER_LOADING);
                                    //break;

                                    //// 1.2 unload to cleaner
                                    //SetStep1((int)TRS_PUSHPULL_REQUEST_CLEANER_LOADING);
                                    //break;

                                    //// 1.3 unload to coater
                                    //SetStep1((int)TRS_PUSHPULL_REQUEST_COATER_LOADING);
                                    //break;

                                    //// 1.4 unload to handler
                                    //SetStep1((int)TRS_PUSHPULL_REQUEST_HANDLER_LOADING);
                                    //break;
                                }
                                // 2. if not detected wafer
                                else
                                {
                                    // 2.1 load from loader
                                    //SetStep1((int)TRS_PUSHPULL_REQUEST_LOADER_UNLOADING);
                                    //break;

                                    //// 2.2 load from cleaner
                                    //SetStep1((int)TRS_PUSHPULL_REQUEST_CLEANER_UNLOADING);
                                    //break;

                                    //// 2.3 load from coater
                                    //SetStep1((int)TRS_PUSHPULL_REQUEST_COATER_UNLOADING);
                                    //break;

                                    //// 2.4 load from handler
                                    //SetStep1((int)TRS_PUSHPULL_REQUEST_HANDLER_UNLOADING);
                                    //break;
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
