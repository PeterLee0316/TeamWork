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
using static LWDicer.Control.DEF_Thread.EAutoRunStatus;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_LCNet;

namespace LWDicer.Control
{
    public abstract class MWorkerThread : MCmdTarget, IDisposable
    {
        // for creating thread
        static List<MWorkerThread> stThreadList = new List<MWorkerThread>();
        static int stIndex = 0;
        MWorkerThread[] m_LinkedThreadArray = new MWorkerThread[(int)EThreadChannel.MAX];

        // Thread Information
        Thread m_hThread;   // Thread Handle
        public int ThreadID { get; private set; }
        public bool IsAlive { get; private set; }

        private EThreadChannel  SelfChannelNo;
        public EAutoManual      AutoManualMode { get; private set; } = EAutoManual.MANUAL; // AUTO, MANUAL
        public EAutoRunMode     AutoRunMode { get; private set; } = EAutoRunMode.NORMAL_RUN; // NORMAL_RUN, PASS_RUN, DRY_RUN, REPAIR_RUN
        public EAutoRunStatus   RunStatus { get; private set; } = EAutoRunStatus.STS_MANUAL; // STS_MANUAL, STS_RUN_READY, STS_RUN,STS_STEP_STOP, 
        public EAutoRunStatus   RunStatus_Old { get; private set; } = EAutoRunStatus.STS_RUN; // Old RunStatus

        // Process에 따라서 Multi Process를 운용할 필요가 있기때문에, ThreadStep1,2로 define
        // 평소엔 ThreadStep1 만 사용함
        public int ThreadStep1 { get; protected set; } = 0;
        public int ThreadStep2 { get; protected set; } = 0;

        // for communication with UI
        protected CMainFrame MainFrame;
        protected delegate void ProcessMsgDelegate(MEvent evnt);

        // interval post msg
        private MTickTimer PostTimer = new MTickTimer();
        private long LastPostTime;
        private long PostIntervalTime = 2;  // second

        // DataManager and WorkPiece
        protected MDataManager DataManager;
        protected ELCNetUnitPos LCNetUnitPos;

        public MWorkerThread(CObjectInfo objInfo, EThreadChannel SelfChannelNo
            , MDataManager DataManager, ELCNetUnitPos LCNetUnitPos) : base(objInfo)
        {
            ThreadID = GetUniqueThreadID();
            this.SelfChannelNo = SelfChannelNo;
            stThreadList.Add(this);
            this.DataManager = DataManager;
            this.LCNetUnitPos = LCNetUnitPos;

            PostTimer.StartTimer();
            LastPostTime = PostTimer.GetElapsedTime(MTickTimer.ETimeType.TIME_SECOND);

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

        public void SetOperationMode(EAutoRunMode mode)
        {
            if(AutoRunMode != mode)
            AutoRunMode = mode;
        }

        public void SetAutoManual(EAutoManual mode)
        {
            if(AutoManualMode != mode)
            AutoManualMode = mode;
        }

        protected int OnStartRun()
        {
            SetRunStatus(RunStatus);

            return DEF_Error.SUCCESS;
        }


        protected bool SetRunStatus(EAutoRunStatus status)
        {
            if (RunStatus == status) return false;

            RunStatus_Old = RunStatus;
            RunStatus = status;
            return true;
        }

        protected void SetStep1(int step)
        {
            ThreadStep1 = step;
        }

        protected void SetStep2(int step)
        {
            ThreadStep2 = step;
        }

        protected override int ProcessMsg(MEvent evnt)
        {
            Debug.WriteLine($"{ToString()} received message : {evnt}");
            switch (evnt.Msg)
            {
                case (int)MSG_MANUAL_CMD:
                    SetRunStatus(STS_MANUAL);

                    PostMsg(TrsAutoManager, (int)MSG_MANUAL_CNF);
                    break;

                case (int)MSG_START_RUN_CMD:

                    OnStartRun();

                    PostMsg(TrsAutoManager, (int)MSG_START_RUN_CNF);
                    break;

                case (int)MSG_START_CMD:
                    if (RunStatus == STS_RUN_READY || RunStatus == STS_STEP_STOP ||
                        RunStatus == STS_ERROR_STOP)
                    {
                        SetRunStatus(STS_RUN);

                        PostMsg(TrsAutoManager, (int)MSG_START_CNF);
                    }
                    break;

                case (int)MSG_ERROR_STOP_CMD:
                    SetRunStatus(STS_ERROR_STOP);

                    PostMsg(TrsAutoManager, (int)MSG_ERROR_STOP_CNF);
                    break;

                case (int)MSG_STEP_STOP_CMD:
                    if (RunStatus == STS_STEP_STOP || RunStatus == STS_ERROR_STOP)
                    {
                        SetRunStatus(STS_MANUAL);
                    }
                    else
                    {
                        SetRunStatus(STS_STEP_STOP);
                    }

                    PostMsg(TrsAutoManager, (int)MSG_STEP_STOP_CNF);
                    break;

                case (int)MSG_CYCLE_STOP_CMD:
                    SetRunStatus(STS_CYCLE_STOP);
                    PostMsg(TrsAutoManager, (int)MSG_CYCLE_STOP_CNF);
                    break;

            }
            return DEF_Error.SUCCESS;
        }

        protected virtual void ThreadProcess()
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

                switch (RunStatus)
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
                        //if (ThreadStep1 == TRS_STAGE1_MOVETO_LOAD)
                        break;

                    case STS_RUN: // auto run
                        //m_RefComp.m_pC_CtrlStage1->SetAutoManual(AUTO);

                        // Do Thread Step
                        switch (ThreadStep1)
                        {
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

        /// <summary>
        /// 관리중인 Thread List에 Message를 전파한다.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="bDirect"></param>
        public void BroadcastMsg(int msg, int wParam = 0, int lParam = 0, bool bDirect = false)
        {
            //if (wParam == 0) wParam = ThreadID; // Set Sender ID
            if (wParam == 0) wParam = (int)SelfChannelNo; // Set Sender ID
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
                for (i = 0; i < (int)EThreadChannel.MAX ; i++)
                {
                    if (i == (int)SelfChannelNo) continue;

                    PostMsg(i, msg, wParam, lParam);
                }
            }
        }

        public void BroadcastMsg(EThreadMessage msg, int wParam = 0, int lParam = 0, bool bDirect = false)
        {
            BroadcastMsg((int)msg, wParam, lParam, bDirect);
        }

        /// <summary>
        /// Message를 보내며 통신하고 싶은 Thread를 List로 관리한다.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="pThread"></param>
        /// <returns></returns>
        public int LinkThread(int channel, MWorkerThread pThread)
        {
            if ((channel < 0) || (channel >= (int)EThreadChannel.MAX)) return -1;
            m_LinkedThreadArray[channel] = pThread;

            Debug.WriteLine($"[LinkThread] Channel{channel} : {m_LinkedThreadArray[channel]}");
            return DEF_Error.SUCCESS;
        }

        public int LinkThread(EThreadChannel channel, MWorkerThread pThread)
        {
            return LinkThread((int)channel, pThread);
        }

        public int PostMsg(int target, MEvent evnt)
        {
            if ((target < 0) || (target >= (int)EThreadChannel.MAX)) return -1;

            if (m_LinkedThreadArray[target] == null) return -1;
            return (m_LinkedThreadArray[target].PostMsg(evnt));
        }

        public int PostMsg(int target, int msg, int wParam = 0, int lParam = 0)
        {
            //if (wParam == 0) wParam = ThreadID; // Set Sender ID
            if (wParam == 0) wParam = (int)SelfChannelNo; // Set Sender ID
            // because when doing linkthread, link self to zero channel.
            if (target == (int)SelfChannelNo)
            {
                target = 0;
            }
            if (m_LinkedThreadArray[target] == null) return -1;
            return PostMsg(target, new MEvent(msg, wParam, lParam));
        }

        public int PostMsg(EThreadChannel target, int msg, int wParam = 0, int lParam = 0)
        {
            return PostMsg((int)target, msg, wParam, lParam);
        }

        public int PostMsg(EThreadChannel target, EThreadMessage msg, int wParam = 0, int lParam = 0)
        {
            return PostMsg((int)target, (int)msg, wParam, lParam);
        }

        /// <summary>
        /// 일정시간마다 한번씩 Msg를 날려주는 함수
        /// </summary>
        /// <param name="target"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public int PostMsg_Interval(EThreadChannel target, EThreadMessage msg, int wParam = 0, int lParam = 0)
        {
            if (PostTimer.MoreThan(LastPostTime + PostIntervalTime, MTickTimer.ETimeType.TIME_SECOND) == false)
                return SUCCESS;

            LastPostTime += PostIntervalTime;
            return PostMsg((int)target, (int)msg, wParam, lParam);
        }

        /// <summary>
        /// 자신의 RunStatus를 Error Stop으로 변경 및 Alarm을 선택한 채널 (TrsAutoManager)에게 보고하는 함수
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        protected int ReportAlarm(int alarm, int target = (int)TrsAutoManager)
        {
            RunStatus = STS_ERROR_STOP;

            MEvent evnt = new MEvent((int)MSG_PROCESS_ALARM, wParam:ThreadID, lParam:alarm);
            return PostMsg(target, evnt);
        }

        protected int GetThreadsCount()
        {
            return stIndex;
        }

        private void SendMsgToMainWnd(int msg, int wParam = 0, int lParam = 0)
        {
            //if (wParam == 0) wParam = ThreadID; // Set Sender ID
            if (wParam == 0) wParam = (int)SelfChannelNo; // Set Sender ID

            WriteLog($"send msg to main wnd : {msg}");
            MainFrame?.Invoke(new ProcessMsgDelegate(MainFrame.ProcessMsg), new MEvent(msg, wParam, lParam));
        }

        public void SendMsgToMainWnd(EWindowMessage msg, int wParam = 0, int lParam = 0)
        {
            SendMsgToMainWnd((int)msg, wParam, lParam);
        }

        public void SetWindows_Form1(CMainFrame MainFrame)
        {
            this.MainFrame = MainFrame;
        }

        public virtual int Initialize()
        {
            return SUCCESS;
        }

        protected CWorkPiece GetWorkPiece(ELCNetUnitPos pos)
        {
            return GetWorkPiece((int)pos);
        }

        protected CWorkPiece GetWorkPiece(int pos)
        {
            return DataManager.WorkPieceArray[pos];
        }

        protected CWorkPiece GetMyWorkPiece()
        {
            return GetWorkPiece((LCNetUnitPos != ELCNetUnitPos.NONE ? LCNetUnitPos : ELCNetUnitPos.PUSHPULL));
        }
    }
}
