using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using LWDicer.UI;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Thread.EThreadChannel;
using static LWDicer.Layers.DEF_Thread.EThreadMessage;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_LCNet;

namespace LWDicer.Layers
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
        protected EThreadChannel GetSelfChannelNo(int index) { return m_LinkedThreadArray[index].SelfChannelNo; }
        public EAutoManual      AutoManualMode { get; private set; } = EAutoManual.MANUAL; // AUTO, MANUAL
        public EAutoRunMode     AutoRunMode { get; private set; } = EAutoRunMode.NORMAL_RUN; // NORMAL_RUN, PASS_RUN, DRY_RUN, REPAIR_RUN
        public EAutoRunStatus   RunStatus { get; private set; } = EAutoRunStatus.STS_MANUAL; // EAutoRunStatus.STS_MANUAL, EAutoRunStatus.STS_RUN_READY, EAutoRunStatus.STS_RUN,STS_STEP_STOP, 
        public EAutoRunStatus   RunStatus_Old { get; private set; } = EAutoRunStatus.STS_RUN; // Old RunStatus


        // 평소엔 ThreadStep 만 사용함
        // Process에 따라서 Multi Process를 운용할 필요가 있기때문에, ThreadStep1,2를 사용
        protected EThreadStep ThreadStep  = EThreadStep.STEP_NONE;
        protected EThreadStep ThreadStep1 = EThreadStep.STEP_NONE;
        protected EThreadStep ThreadStep2 = EThreadStep.STEP_NONE;

        // for communication with UI
        protected CMainFrame MainFrame;
        protected delegate void ProcessMsgDelegate(MEvent evnt);

        // interval post msg
        private MTickTimer PostTimer = new MTickTimer();
        private double LastPostTime;
        private long PostIntervalTime = 2;  // second

        // DataManager and WorkPiece
        protected MDataManager DataManager;
        protected ELCNetUnitPos LCNetUnitPos;

        // for inter thread communication
        public static CThreadInterface TInterface = new CThreadInterface();
        protected MTickTimer TTimer = new MTickTimer(); // interface timer
        protected int TSelf;        // 자기가 error가 발생했다는것을 표시하는 용도
        protected int TOpponent;    // handshake opponent

        public MWorkerThread(CObjectInfo objInfo, EThreadChannel SelfChannelNo
            , MDataManager DataManager, ELCNetUnitPos LCNetUnitPos) : base(objInfo)
        {
            ThreadID = GetUniqueThreadID();
            this.SelfChannelNo = SelfChannelNo;
            stThreadList.Add(this);
            this.DataManager = DataManager;
            this.LCNetUnitPos = LCNetUnitPos;

            PostTimer.StartTimer();
            LastPostTime = PostTimer.GetElapsedTime(ETimeType.SECOND);

            Debug.WriteLine(ToString());
        }

        public void Dispose()
        {
            if (m_hThread == null) return;
            if (m_hThread.IsAlive == true)
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

            return SUCCESS;
        }

        public int ThreadStop()
        {
            IsAlive = false;
            m_hThread.Abort();

            return SUCCESS;
        }

        public int Suspend()
        {
            IsAlive = false;
            //m_hThread.Suspend();

            return SUCCESS;
        }

        public int Resume()
        {
            IsAlive = true;
            //m_hThread.Resume();

            return SUCCESS;
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
            SetRunStatus(EAutoRunStatus.STS_RUN_READY);

            return SUCCESS;
        }


        protected bool SetRunStatus(EAutoRunStatus status)
        {
            // 이전 thread 상태와 다를 경우에 true를 리턴
            if (RunStatus == status) return false;

            RunStatus_Old = RunStatus;
            RunStatus = status;
            return true;
        }

        protected void SetStep(EThreadStep step)
        {
            ThreadStep = step;
        }

        protected void SetStep1(EThreadStep step)
        {
            ThreadStep1 = step;
        }

        protected void SetStep2(EThreadStep step)
        {
            ThreadStep2 = step;
        }

        virtual public string GetStep()
        {
            return ThreadStep.ToString();
        }

        virtual public string GetStep1()
        {
            return ThreadStep1.ToString();
        }

        virtual public string GetStep2()
        {
            return ThreadStep2.ToString();
        }

        virtual public string GetRunStatus()
        {
            return RunStatus.ToString();
        }

        protected override int ProcessMsg(MEvent evnt)
        {
            Debug.WriteLine($"[MWorkerThread] received message : {evnt}");

            EThreadMessage msg = EThreadMessage.NONE;
            try
            {
                msg = (EThreadMessage)Enum.Parse(typeof(EThreadMessage), evnt.Msg.ToString());
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }
            switch (msg)
            {
                case MSG_MANUAL_CMD:
                    SetRunStatus(EAutoRunStatus.STS_MANUAL);

                    PostMsg(TrsAutoManager, (int)MSG_MANUAL_CNF);
                    break;

                case MSG_READY_RUN_CMD:

                    OnStartRun();

                    PostMsg(TrsAutoManager, (int)MSG_READY_RUN_CNF);
                    break;

                case MSG_START_CMD:
                    if (RunStatus == EAutoRunStatus.STS_RUN_READY || RunStatus == EAutoRunStatus.STS_STEP_STOP ||
                        RunStatus == EAutoRunStatus.STS_ERROR_STOP)
                    {
                        SetRunStatus(EAutoRunStatus.STS_RUN);

                        PostMsg(TrsAutoManager, (int)MSG_START_CNF);
                    }
                    break;

                case MSG_ERROR_STOP_CMD:
                    SetRunStatus(EAutoRunStatus.STS_ERROR_STOP);

                    PostMsg(TrsAutoManager, (int)MSG_ERROR_STOP_CNF);
                    break;

                case MSG_STEP_STOP_CMD:
                    if (RunStatus == EAutoRunStatus.STS_STEP_STOP || RunStatus == EAutoRunStatus.STS_ERROR_STOP)
                    {
                        SetRunStatus(EAutoRunStatus.STS_MANUAL);
                    }
                    else
                    {
                        SetRunStatus(EAutoRunStatus.STS_STEP_STOP);
                    }

                    PostMsg(TrsAutoManager, (int)MSG_STEP_STOP_CNF);
                    break;

                case MSG_CYCLE_STOP_CMD:
                    SetRunStatus(EAutoRunStatus.STS_CYCLE_STOP);
                    PostMsg(TrsAutoManager, (int)MSG_CYCLE_STOP_CNF);
                    break;

            }
            return SUCCESS;
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
                    case EAutoRunStatus.STS_MANUAL: // Manual Mode
                        //m_RefComp.m_pC_CtrlStage1->SetAutoManual(MANUAL);
                        break;

                    case EAutoRunStatus.STS_ERROR_STOP: // Error Stop
                        break;

                    case EAutoRunStatus.STS_STEP_STOP: // Step Stop
                        break;

                    case EAutoRunStatus.STS_RUN_READY: // Run Ready
                        break;

                    case EAutoRunStatus.STS_CYCLE_STOP: // Cycle Stop
                        //if (ThreadStep1 == TRS_STAGE1_MOVETO_LOAD)
                        break;

                    case EAutoRunStatus.STS_RUN: // auto run
                        //m_RefComp.m_pC_CtrlStage1->SetAutoManual(AUTO);

                        // Do Thread Step
                        switch (ThreadStep)
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
        public void BroadcastMsg(int msg, int wParam = -1, int lParam = -1, bool bDirect = false, bool bExceptSelf = true)
        {
            //if (wParam == 0) wParam = ThreadID; // Set Sender ID
            if (wParam == -1) wParam = (int)SelfChannelNo; // Set Sender ID
            if (bDirect)
            {
                for (int i = 0; i < stThreadList?.Count; i++)
                {
                    if(bExceptSelf == true)
                    {
                        // stThreadList에는 ThreadID 순서로 들어있기 때문에 ThreadID로 자신인지 비교
                        //if (i == (int)SelfChannelNo) continue;
                        if (i == (int)ThreadID) continue;
                    }
                    if (lParam == -1)
                    {
                        stThreadList[i].SendMsg(msg, wParam, (int)stThreadList[i].SelfChannelNo); // Set Target ID
                    }
                    else
                    {
                        stThreadList[i].SendMsg(msg, wParam, lParam);
                    }
                }
            }
            else
            {
                for (int i = 0; i < (int)EThreadChannel.MAX ; i++)
                {
                    if (bExceptSelf == true)
                    {
                        //if (i == (int)SelfChannelNo) continue;
                        if (this.SelfChannelNo == m_LinkedThreadArray[i].SelfChannelNo) continue;
                    }

                    if (lParam == -1)
                    {
                        PostMsg(i, msg, wParam, (int)m_LinkedThreadArray[i].SelfChannelNo); // Set Target ID
                    }
                    else
                    {
                        PostMsg(i, msg, wParam, lParam);
                    }
                }
            }
        }

        public void BroadcastMsg(EThreadMessage msg, int wParam = -1, int lParam = -1, bool bDirect = false, bool bExceptSelf = true)
        {
            BroadcastMsg((int)msg, wParam, lParam, bDirect, bExceptSelf);
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
            return SUCCESS;
        }

        public int LinkThread(EThreadChannel channel, MWorkerThread pThread)
        {
            return LinkThread((int)channel, pThread);
        }

        public int PostMsg(int target, MEvent evnt)
        {
            if ((target < 0) || (target >= (int)EThreadChannel.MAX)) return -1;
            // because when doing linkthread, link self to zero channel.
            if (target == (int)SelfChannelNo)
            {
                target = 0;
            }

            if (m_LinkedThreadArray[target] == null) return -1;
            return (m_LinkedThreadArray[target].PostMsg(evnt));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">보통은 EThreadChannel Target 정보</param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public int PostMsg(int target, int msg, int wParam = -1, int lParam = -1)
        {
            //if (wParam == 0) wParam = ThreadID; // Set Sender ID
            if (wParam == -1) wParam = (int)SelfChannelNo; // Set Sender ID
            if (lParam == -1) lParam = (int)target; // Set Target ID
            // because when doing linkthread, link self to zero channel.
            if (target == (int)SelfChannelNo)
            {
                target = 0;
            }
            if (m_LinkedThreadArray[target] == null) return -1;
            return PostMsg(target, new MEvent(msg, wParam, lParam));
        }

        public int PostMsg(EThreadChannel target, int msg, int wParam = -1, int lParam = -1)
        {
            return PostMsg((int)target, msg, wParam, lParam);
        }

        public int PostMsg(EThreadChannel target, EThreadMessage msg, int wParam = -1, int lParam = -1)
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
        public int PostMsg_Interval(EThreadChannel target, EThreadMessage msg, int wParam = -1, int lParam = -1)
        {
            if (PostTimer.MoreThan(LastPostTime + PostIntervalTime, ETimeType.SECOND) == false)
                return SUCCESS;

            LastPostTime += PostIntervalTime;
            return PostMsg((int)target, (int)msg, wParam, lParam);
        }

        /// <summary>
        /// 자신의 RunStatus를 Error Stop으로 변경 및 Alarm을 선택한 채널 (TrsAutoManager)에게 보고하는 함수
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public int ReportAlarm(int alarm, int target = (int)TrsAutoManager)
        {
            RunStatus = EAutoRunStatus.STS_ERROR_STOP;
            TInterface.ErrorOccured[TSelf] = true;

            MEvent evnt = new MEvent((int)MSG_PROCESS_ALARM, wParam:(int)SelfChannelNo, lParam:alarm);
            return PostMsg(target, evnt);
        }

        protected int GetThreadsCount()
        {
            return stIndex;
        }

        private void SendMsgToMainWnd(int msg, int wParam = -1, int lParam = -1)
        {
            //if (wParam == 0) wParam = ThreadID; // Set Sender ID
            if (wParam == 0) wParam = (int)SelfChannelNo; // Set Sender ID
            if (lParam == -1) lParam = (int)9999; // Set Target ID

            WriteLog($"send msg to main wnd : {msg}");

            if (CMainFrame.IsFormLoaded == false) return;
            //MainFrame?.Invoke(new ProcessMsgDelegate(MainFrame.ProcessMsg), new MEvent(msg, wParam, lParam));
            Delegate msgDelegate = new ProcessMsgDelegate(MainFrame.ProcessMsg);
            MEvent msgEvent = new MEvent(msg, wParam, lParam);
            MainFrame?.Invoke(msgDelegate, msgEvent);
        }

        public void SendMsgToMainWnd(EWindowMessage msg, int wParam = -1, int lParam = -1)
        {
            SendMsgToMainWnd((int)msg, wParam, lParam);
        }

        public void SetWindows_Form1(CMainFrame MainFrame)
        {
            this.MainFrame = MainFrame;
        }

        public virtual int Initialize()
        {
            // Interface & Error Reset
            TInterface.ResetInterface(TSelf);
            return SUCCESS;
        }

        protected CWorkPiece GetWorkPiece(ELCNetUnitPos pos)
        {
            return GetWorkPiece((int)pos);
        }

        protected CWorkPiece GetWorkPiece(ESpinnerIndex pos)
        {
            if(pos == ESpinnerIndex.SPINNER1)
                return GetWorkPiece((int)ELCNetUnitPos.SPINNER1);
            else return GetWorkPiece((int)ELCNetUnitPos.SPINNER2);
        }

        protected CWorkPiece GetWorkPiece(int pos)
        {
            return DataManager.WorkPieceArray[pos];
        }

        protected CWorkPiece GetMyWorkPiece()
        {
            return GetWorkPiece((LCNetUnitPos != ELCNetUnitPos.NONE ? LCNetUnitPos : ELCNetUnitPos.PUSHPULL));
        }

        protected int LoadWorkPieceFromCassette()
        {
            return DataManager.LoadWorkPieceFromCassette();
        }

        protected int UnloadWorkPieceToCassette()
        {
            return DataManager.UnloadWorkPieceToCassette();
        }

        protected EProcessPhase GetMyNextWorkPhase()
        {
            CWorkPiece wPiece = GetMyWorkPiece();
            return wPiece.GetNextPhase();
        }
    }
}
