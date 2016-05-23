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
using static LWDicer.Control.DEF_Thread.ERunMode;
using static LWDicer.Control.DEF_Thread.ERunStatus;
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

        bool m_bCleaner1_ReadyLoading;
        bool m_bCleaner1_StartLoading;
        bool m_bCleaner1_CompleteLoading;
        bool m_bCleaner1_ReadyUnloading;
        bool m_bCleaner1_StartUnloading;
        bool m_bCleaner1_CompleteUnloading;

        bool m_bCleaner2_ReadyLoading;
        bool m_bCleaner2_StartLoading;
        bool m_bCleaner2_CompleteLoading;
        bool m_bCleaner2_ReadyUnloading;
        bool m_bCleaner2_StartUnloading;
        bool m_bCleaner2_CompleteUnloading;

        bool m_bHandler_ReadyLoading;
        bool m_bHandler_StartLoading;
        bool m_bHandler_CompleteLoading;
        bool m_bHandler_ReadyUnloading;
        bool m_bHandler_StartUnloading;
        bool m_bHandler_CompleteUnloading;

        public MTrsPushPull(CObjectInfo objInfo, EThreadChannel SelfChannelNo,
            CTrsPushPullRefComp refComp, CTrsPushPullData data)
             : base(objInfo, SelfChannelNo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

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

        public int Initialize()
        {
            // Do initialize
            InitializeMsg();
            InitializeInterface();

            // Do Action
            int iStep = (int)TRS_PUSHPULL_WAITFOR_MESSAGE;

            // finally
            ThreadStep = iStep;

            return SUCCESS;
        }

        public int InitializeMsg()
        {
            m_bLoader_ReadyUnloading = false;
            m_bLoader_ReadyLoading = false;
            m_bLoader_AllWaferWorked = false;
            m_bLoader_StacksFull = false;

            m_bCleaner1_ReadyLoading = false;
            m_bCleaner1_StartLoading = false;
            m_bCleaner1_CompleteLoading = false;
            m_bCleaner1_ReadyUnloading = false;
            m_bCleaner1_StartUnloading = false;
            m_bCleaner1_CompleteUnloading = false;

            m_bCleaner2_ReadyLoading = false;
            m_bCleaner2_StartLoading = false;
            m_bCleaner2_CompleteLoading = false;
            m_bCleaner2_ReadyUnloading = false;
            m_bCleaner2_StartUnloading = false;
            m_bCleaner2_CompleteUnloading = false;

            m_bHandler_ReadyLoading = false;
            m_bHandler_StartLoading = false;
            m_bHandler_CompleteLoading = false;
            m_bHandler_ReadyUnloading = false;
            m_bHandler_StartUnloading = false;
            m_bHandler_CompleteUnloading = false;

            return SUCCESS;
        }

        private int InitializeInterface()
        {

            return SUCCESS;
        }

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
                case (int)MSG_LOADER_PUSHPULL_READY_UNLOADING:
                    m_bLoader_ReadyUnloading = true;
                    break;

                case (int)MSG_LOADER_PUSHPULL_READY_LOADING:
                    m_bLoader_ReadyLoading = true;
                    break;

                case (int)MSG_LOADER_PUSHPULL_ALL_WAFER_WORKED:
                    m_bLoader_AllWaferWorked = true;
                    break;

                case (int)MSG_LOADER_PUSHPULL_STACKS_FULL:
                    m_bLoader_StacksFull = true;
                    break;

                // with Cleaner1
                case (int)MSG_CLEANER1_PUSHPULL_READY_LOADING:
                    m_bCleaner1_ReadyLoading = true;
                    break;

                case (int)MSG_CLEANER1_PUSHPULL_START_LOADING:
                    m_bCleaner1_StartLoading = true;
                    break;

                case (int)MSG_CLEANER1_PUSHPULL_COMPLETE_LOADING:
                    m_bCleaner1_CompleteLoading = true;
                    break;

                case (int)MSG_CLEANER1_PUSHPULL_READY_UNLOADING:
                    m_bCleaner1_ReadyUnloading = true;
                    break;

                case (int)MSG_CLEANER1_PUSHPULL_START_UNLOADING:
                    m_bCleaner1_StartUnloading = true;
                    break;

                case (int)MSG_CLEANER1_PUSHPULL_COMPLETE_UNLOADING:
                    m_bCleaner1_CompleteUnloading = true;
                    break;

                // with Cleaner2
                case (int)MSG_CLEANER2_PUSHPULL_READY_LOADING:
                    m_bCleaner2_ReadyLoading = true;
                    break;

                case (int)MSG_CLEANER2_PUSHPULL_START_LOADING:
                    m_bCleaner2_StartLoading = true;
                    break;

                case (int)MSG_CLEANER2_PUSHPULL_COMPLETE_LOADING:
                    m_bCleaner2_CompleteLoading = true;
                    break;

                case (int)MSG_CLEANER2_PUSHPULL_READY_UNLOADING:
                    m_bCleaner2_ReadyUnloading = true;
                    break;

                case (int)MSG_CLEANER2_PUSHPULL_START_UNLOADING:
                    m_bCleaner2_StartUnloading = true;
                    break;

                case (int)MSG_CLEANER2_PUSHPULL_COMPLETE_UNLOADING:
                    m_bCleaner2_CompleteUnloading = true;
                    break;

                // with Handler
                case (int)MSG_HANDLER_PUSHPULL_READY_LOADING:
                    m_bHandler_ReadyLoading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_START_LOADING:
                    m_bHandler_StartLoading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_COMPLETE_LOADING:
                    m_bHandler_CompleteLoading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_READY_UNLOADING:
                    m_bHandler_ReadyUnloading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_START_UNLOADING:
                    m_bHandler_StartUnloading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_COMPLETE_UNLOADING:
                    m_bHandler_CompleteUnloading = true;
                    break;

            }
            return DEF_Error.SUCCESS;
        }

        public override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bState = false;

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
                        //m_RefComp.ctrlPushPull.SetAutoManual(MANUAL);
                        break;

                    case STS_ERROR_STOP: // Error Stop
                        break;

                    case STS_STEP_STOP: // Step Stop
                        break;

                    case STS_RUN_READY: // Run Ready
                        break;

                    case STS_CYCLE_STOP: // Cycle Stop
                        //if (ThreadStep == TRS_LOADER_MOVETO_LOAD)
                        break;

                    case STS_RUN: // auto run
                        //m_RefComp.ctrlPushPull.SetAutoManual(AUTO);

                        switch (ThreadStep)
                        {
                            case (int)TRS_PUSHPULL_WAITFOR_MESSAGE:

                                // 0. check default status;

                                // 1. if detected wafer
                                m_RefComp.ctrlPushPull.IsObjectDetected(out bState);
                                if (bState)
                                {
                                    // 1.1 unload to loader
                                    //SetStep((int)TRS_PUSHPULL_REQUEST_LOADER_LOADING);
                                    //break;

                                    //// 1.2 unload to cleaner
                                    //SetStep((int)TRS_PUSHPULL_REQUEST_CLEANER_LOADING);
                                    //break;

                                    //// 1.3 unload to coater
                                    //SetStep((int)TRS_PUSHPULL_REQUEST_COATER_LOADING);
                                    //break;

                                    //// 1.4 unload to handler
                                    //SetStep((int)TRS_PUSHPULL_REQUEST_HANDLER_LOADING);
                                    //break;
                                }
                                // 2. if not detected wafer
                                else
                                {
                                    // 2.1 load from loader
                                    //SetStep((int)TRS_PUSHPULL_REQUEST_LOADER_UNLOADING);
                                    //break;

                                    //// 2.2 load from cleaner
                                    //SetStep((int)TRS_PUSHPULL_REQUEST_CLEANER_UNLOADING);
                                    //break;

                                    //// 2.3 load from coater
                                    //SetStep((int)TRS_PUSHPULL_REQUEST_COATER_UNLOADING);
                                    //break;

                                    //// 2.4 load from handler
                                    //SetStep((int)TRS_PUSHPULL_REQUEST_HANDLER_UNLOADING);
                                    //break;
                                }

                                break;

                                ///////////////////////////////////////////////////
                                // transfer wafer to loader from wafer
                            //case (int)TRS_PUSHPULL_REQUEST_LOADER_LOADING:
                            //    PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING);

                            //    SetStep((int)TRS_PUSHPULL_WAITFOR_LOADER_READY_LOADING);
                            //    break;

                            //case (int)TRS_PUSHPULL_WAITFOR_LOADER_READY_LOADING:
                            //    if (m_bLoader_ReadyLoading == false) break;
                            //    m_bLoader_ReadyLoading = false;

                            //    SetStep((int)TRS_PUSHPULL_WAITFOR_LOADER_COMPLETE_LOADING);
                            //    break;

                            //case (int)TRS_PUSHPULL_START_UNLOADING_TO_LOADER:
                            //    // move to loader

                            //    PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_START_UNLOADING);

                            //    SetStep((int)TRS_PUSHPULL_WAITFOR_LOADER_COMPLETE_LOADING);
                            //    break;

                            //case (int)TRS_PUSHPULL_WAITFOR_LOADER_COMPLETE_LOADING:
                            //    PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING);

                            //    SetStep((int)TRS_PUSHPULL_COMPLETE_UNLOADING_TO_LOADER);
                            //    break;

                            //case (int)TRS_PUSHPULL_COMPLETE_UNLOADING_TO_LOADER:
                            //    PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING);

                            //    SetStep((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
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
