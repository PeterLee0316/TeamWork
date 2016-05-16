using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using LWDicer.UI;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.EThreadChannel;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.EOpStatus;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public abstract class MWorkerThread : MCmdTarget, IDisposable
    {
        // for creating thread
        static List<MWorkerThread> stThreadList = new List<MWorkerThread>();
        static int stIndex = 0;
        MWorkerThread[] m_LinkedThreadArray = new MWorkerThread[MAX_THREAD_CHANNEL];

        // Thread Information
        Thread m_hThread;   // Thread Handle
        public int ThreadID { get; private set; }
        public bool IsAlive { get; private set; }

        private EThreadChannel SelfChannelNo;
        protected EAutoManual   eAutoManual;    // EAutoManual : AUTO, MANUAL
        protected EOpMode       eOpMode;        // EOpMode : NORMAL_RUN, PASS_RUN, DRY_RUN, REPAIR_RUN
        protected EOpStatus     eOpStatus;      // eOpStatus : STS_MANUAL, STS_RUN_READY, STS_RUN,STS_STEP_STOP, 
        protected EOpStatus     eOpStatus_Old;  // Old eOpStatus

        public int ThreadStep { get; protected set; } = 0;

        // for communication with UI
        protected CMainFrame MainFrame;
        protected delegate void ProcessMsgDelegate(MEvent evnt);

        public MWorkerThread(CObjectInfo objInfo, EThreadChannel SelfChannelNo) : base(objInfo)
        {
            eAutoManual   = EAutoManual.MANUAL;
            eOpMode       = EOpMode.NORMAL_RUN;
            eOpStatus     = EOpStatus.STS_MANUAL;
            eOpStatus_Old = EOpStatus.STS_RUN;

            ThreadID = GetUniqueThreadID();
            this.SelfChannelNo = SelfChannelNo;

            stThreadList.Add(this);

            Debug.WriteLine(ToString());

        }

        public void Dispose()
        {
            if(m_hThread.IsAlive == true)
            {
                m_hThread.Abort();
            }
        }

        ~MWorkerThread()
        {
            Dispose();
        }

        public int ThreadStart()
        {
            IsAlive = true;

            m_hThread = new Thread(ThreadProcess);
            m_hThread.Start();

            return DEF_Error.SUCCESS;
        }

        public int ThreadStop()
        {
            IsAlive = false;
            m_hThread.Abort();

            return DEF_Error.SUCCESS;
        }

        public int Suspend()
        {
            IsAlive = false;
            //m_hThread.Suspend();

            return DEF_Error.SUCCESS;
        }

        public int Resume()
        {
            IsAlive = true;
            //m_hThread.Resume();

            return DEF_Error.SUCCESS;
        }

        public void SetOperationMode(EOpMode mode)
        {
            eOpMode = mode;
        }

        public void SetAutoManual(EAutoManual mode)
        {
            eAutoManual = mode;
        }

        protected int OnStartRun()
        {
            SetOpStatus(eOpStatus);

            return DEF_Error.SUCCESS;
        }

        protected bool SetOpStatus(EOpStatus status)
        {
            if (eOpStatus == status) return false;

            eOpStatus_Old = eOpStatus;
            eOpStatus = status;
            return true;
        }

        protected void SetStep(int Step)
        {
            ThreadStep = Step;
        }

        protected override int ProcessMsg(MEvent evnt)
        {
            Debug.WriteLine($"{ToString()} received message : {evnt}");
            switch (evnt.Msg)
            {
                case (int)MSG_MANUAL_CMD:
                    SetOpStatus(STS_MANUAL);

                    PostMsg(TrsAutoManager, (int)MSG_MANUAL_CNF);
                    break;

                case (int)MSG_START_RUN_CMD:

                    OnStartRun();

                    PostMsg(TrsAutoManager, (int)MSG_START_RUN_CNF);
                    break;

                case (int)MSG_START_CMD:
                    if (eOpStatus == STS_RUN_READY || eOpStatus == STS_STEP_STOP ||
                        eOpStatus == STS_ERROR_STOP)
                    {
                        SetOpStatus(STS_RUN);

                        PostMsg(TrsAutoManager, (int)MSG_START_CNF);
                    }
                    break;

                case (int)MSG_ERROR_STOP_CMD:
                    SetOpStatus(STS_ERROR_STOP);

                    PostMsg(TrsAutoManager, (int)MSG_ERROR_STOP_CNF);
                    break;

                case (int)MSG_STEP_STOP_CMD:
                    if (eOpStatus == STS_STEP_STOP || eOpStatus == STS_ERROR_STOP)
                    {
                        SetOpStatus(STS_MANUAL);
                    }
                    else
                    {
                        SetOpStatus(STS_STEP_STOP);
                    }

                    PostMsg(TrsAutoManager, (int)MSG_STEP_STOP_CNF);
                    break;

                case (int)MSG_CYCLE_STOP_CMD:
                    SetOpStatus(STS_CYCLE_STOP);
                    PostMsg(TrsAutoManager, (int)MSG_CYCLE_STOP_CNF);
                    break;

            }
            return DEF_Error.SUCCESS;
        }

        public virtual void ThreadProcess()
        {
            int iResult = SUCCESS;

            while (true)
            {
                // if thread has been suspended
                if(IsAlive == false)
                {
                    Sleep(ThreadSuspendedTime);
                    continue;
                }

                // check message from other thread
                CheckMsg(1);

                switch (eOpStatus)
                {
                    case STS_MANUAL: // Manual Mode
                        //m_RefComp.m_pC_CtrlStage1->SetAutoManual(MANUAL);
                        break;

                    case STS_ERROR_STOP: // Error Stop
                        break;

                    case STS_STEP_STOP: // Step Stop
                        break;

                    case STS_RUN_READY: // Run Ready
                        break;

                    case STS_CYCLE_STOP: // Cycle Stop
                        //if (ThreadStep == TRS_STAGE1_MOVETO_LOAD)
                        break;

                    case STS_RUN: // auto run
                        //m_RefComp.m_pC_CtrlStage1->SetAutoManual(AUTO);

                        switch (ThreadStep)
                        {
                            // Do Thread Auto Run Job
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

        int GetUniqueThreadID()
        {
            return stIndex++;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, ThreadId : {ThreadID}";
        }

        MWorkerThread GetThreadFromIndex(int idx)
        {
            if (idx < 0 || stThreadList.Count <= idx) return null;

            return (MWorkerThread)stThreadList[idx];
        }

        public void BroadcastMsg(int msg, int wParam = 0, int lParam = 0, bool bDirect = false)
        {
            int i = 0;
            if (bDirect)
            {
                for (i = 0; i < stThreadList?.Count; i++)
                {
                    stThreadList[i].SendMsg(msg, wParam, lParam);
                }
            }
            else
            {
                for (i = 0; i < MAX_THREAD_CHANNEL ; i++)
                {
                    if (i == (int)SelfChannelNo) continue;

                    PostMsg(i, msg, wParam, lParam);
                }
            }
        }

        public int LinkThread(int channel, MWorkerThread pThread)
        {
            if ((channel < 0) || (channel >= MAX_THREAD_CHANNEL)) return -1;
            m_LinkedThreadArray[channel] = pThread;

            Debug.WriteLine($"[LinkThread] Channel{channel} : {m_LinkedThreadArray[channel]}");
            return DEF_Error.SUCCESS;
        }

        public int LinkThread(EThreadChannel channel, MWorkerThread pThread)
        {
            return LinkThread((int)channel, pThread);
        }

        public int PostMsg(int channel, MEvent evnt)
        {
            if ((channel < 0) || (channel >= MAX_THREAD_CHANNEL)) return -1;

            if (m_LinkedThreadArray[channel] == null) return -1;
            return (m_LinkedThreadArray[channel].PostMsg(evnt));
        }

        public int PostMsg(int channel, int msg, int wParam = 0, int lParam = 0)
        {
            // because when doing linkthread, link self to zero channel.
            if(channel == (int)SelfChannelNo)
            {
                channel = 0;
            }
            if (m_LinkedThreadArray[channel] == null) return -1;
            return PostMsg(channel, new MEvent(msg, wParam, lParam, ThreadID));
        }

        public int PostMsg(EThreadChannel channel, int msg, int wParam = 0, int lParam = 0)
        {
            return PostMsg((int)channel, msg, wParam, lParam);
        }

        public int SendAlarmTo(int iAlaramCode, int channel = (int)TrsAutoManager)
        {
            eOpStatus = STS_ERROR_STOP;

            MEvent evnt = new MEvent((int)MSG_PROCESS_ALARM, iAlaramCode, channel, ThreadID);
            return PostMsg(channel, evnt);
        }

        protected int GetThreadsCount()
        {
            return stIndex;
        }

        private void SendMessageToMainWnd(int msg, int wParam = 0, int lParam = 0)
        {
            WriteLog($"send msg to main wnd : {msg}");
            MainFrame?.Invoke(new ProcessMsgDelegate(MainFrame.ProcessMsg), new MEvent(msg, lParam, wParam, ThreadID));
        }

        public void SendMessageToMainWnd(EWindowMessage msg, int wParam = 0, int lParam = 0)
        {
            SendMessageToMainWnd((int)msg, wParam, lParam);
        }

        public void SetWindows_Form1(CMainFrame MainFrame)
        {
            this.MainFrame = MainFrame;
        }

    }
}
