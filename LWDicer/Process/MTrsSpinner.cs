using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.ETrsSpinnerStep;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.EThreadChannel;
using static LWDicer.Control.DEF_Thread.EAutoRunMode;
using static LWDicer.Control.DEF_Thread.EAutoRunStatus;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_LCNet;
using static LWDicer.Control.DEF_CtrlSpinner;

namespace LWDicer.Control
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
        public ESpinnerIndex Index = ESpinnerIndex.SPINNER1;
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
            int iStep1 = (int)TRS_SPINNER_MOVETO_WAIT_POS;

            // finally
            SetStep1(iStep1);

            return SUCCESS;
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
            Debug.WriteLine($"{ToString()} received message : {evnt}");
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
            bool bStatus = false;
            EProcessPhase processPhase;

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
                        m_RefComp.ctrlSpinner.SetAutoManual(EAutoManual.MANUAL);
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
                        m_RefComp.ctrlSpinner.SetAutoManual(EAutoManual.AUTO);

                        // Do Thread Step
                        switch (ThreadStep1)
                        {
                            case (int)TRS_SPINNER_MOVETO_WAIT_POS:
                                //iResult = m_RefComp.ctrlSpinner.MoveToWaitPos(false);
                                if(iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                //PostMsg(TrsPushPull, (int)MSG_SPINNER1_PUSHPULL_READY_LOADING);
                                SetStep1((int)TRS_SPINNER_WAITFOR_PUSHPULL_LOAD_REQUEST);
                                break;

                            case (int)TRS_SPINNER_WAITFOR_PUSHPULL_LOAD_REQUEST:
                                if (m_bPushPull_RequestLoading == false) break;

                                SetStep1((int)TRS_SPINNER_MOVETO_LOAD_POS);
                                break;

                            case (int)TRS_SPINNER_MOVETO_LOAD_POS:

                                SetStep1((int)TRS_SPINNER_LOADING);
                                break;

                            case (int)TRS_SPINNER_LOADING:

                                SetStep1((int)TRS_SPINNER_WAITFOR_PUSHPULL_UNLOAD_READY);
                                break;

                            case (int)TRS_SPINNER_WAITFOR_PUSHPULL_UNLOAD_READY:

                                SetStep1((int)TRS_SPINNER_MOVETO_WORK_POS);
                                break;

                            case (int)TRS_SPINNER_MOVETO_WORK_POS:
                                
                                SetStep1((int)TRS_SPINNER_DO_PRE_CLEAN);
                                break;

                            case (int)TRS_SPINNER_DO_PRE_CLEAN:
                                if(m_Data.DoPreClean == false)
                                {
                                    SetStep1((int)TRS_SPINNER_DO_COAT);
                                    break;
                                }

                                SetStep1((int)TRS_SPINNER_DO_AFTER_PRE_CLEAN);
                                break;

                            case (int)TRS_SPINNER_DO_AFTER_PRE_CLEAN:

                                SetStep1((int)TRS_SPINNER_DO_COAT);
                                break;

                            case (int)TRS_SPINNER_DO_COAT:
                                if (m_Data.DoCoat == false)
                                {
                                    SetStep1((int)TRS_SPINNER_DO_POST_CLEAN);
                                    break;
                                }

                                SetStep1((int)TRS_SPINNER_DO_AFTER_COAT);
                                break;

                            case (int)TRS_SPINNER_DO_AFTER_COAT:

                                SetStep1((int)TRS_SPINNER_DO_POST_CLEAN);
                                break;

                            case (int)TRS_SPINNER_DO_POST_CLEAN:
                                if (m_Data.DoPostClean == false)
                                {
                                    SetStep1((int)TRS_SPINNER_REQUEST_PUSHPULL_LOADING);
                                    break;
                                }

                                SetStep1((int)TRS_SPINNER_DO_AFTER_POST_CLEAN);
                                break;

                            case (int)TRS_SPINNER_DO_AFTER_POST_CLEAN:

                                SetStep1((int)TRS_SPINNER_REQUEST_PUSHPULL_LOADING);
                                break;

                            case (int)TRS_SPINNER_REQUEST_PUSHPULL_LOADING:

                                SetStep1((int)TRS_SPINNER_WAITFOR_PUSHPULL_UNLOAD_REQUEST);
                                break;

                            case (int)TRS_SPINNER_WAITFOR_PUSHPULL_UNLOAD_REQUEST:

                                SetStep1((int)TRS_SPINNER_MOVETO_UNLOAD_POS);
                                break;

                            case (int)TRS_SPINNER_MOVETO_UNLOAD_POS:

                                SetStep1((int)TRS_SPINNER_WAITFOR_PUSHPULL_LOAD_READY);
                                break;

                            case (int)TRS_SPINNER_WAITFOR_PUSHPULL_LOAD_READY:

                                SetStep1((int)TRS_SPINNER_UNLOADING);
                                break;

                            case (int)TRS_SPINNER_UNLOADING:

                                SetStep1((int)TRS_SPINNER_MOVETO_WAIT_POS);
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

        public new int PostMsg(EThreadChannel target, int msg, int wParam = 0, int lParam = 0)
        {
            if(m_Data.Index == ESpinnerIndex.SPINNER1)
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
