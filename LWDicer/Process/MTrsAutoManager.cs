using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_LCNet;
using static LWDicer.Control.DEF_Thread.EWindowMessage;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.EThreadChannel;
using static LWDicer.Control.DEF_Thread.EOpMode;
using static LWDicer.Control.DEF_Thread.EOpStatus;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CTrsAutoManagerRefComp
    {
        public MOpPanel OpPanel;

        public MCtrlOpPanel ctrlOpPanel;
        public MCtrlLoader ctrlLoader;
        public MCtrlPushPull ctrlPushPull;
        public MCtrlSpinner ctrlSpinner1;
        public MCtrlSpinner ctrlSpinner2;
        public MCtrlHandler ctrlHandler;
        public MCtrlStage1 ctrlStage1;

        public MTrsLoader trsLoader;
        public MTrsPushPull trsPushPull;

        //public CTrsAutoManagerRefComp(MCtrlLoader ctrlLoader)
        //{
        //    this.ctrlLoader = ctrlLoader;
        //}
        public override string ToString()
        {
            return $"CTrsAutoManagerRefComp : {this}";
        }
    }

    public class CTrsAutoManagerData
    {
    }

    public class MTrsAutoManager : MWorkerThread
    {
        private CTrsAutoManagerRefComp m_RefComp;
        private CTrsAutoManagerData m_Data;

        // Mode
        // Part Empty - Exchange Mode
        bool m_bExchangeMode;

        // Buzzer On Mode
        bool m_bBuzzerMode;

        // Line controller Op call Mode
        bool m_bOpCallMode;

        // Error Display를 위한 Mode
        bool m_bErrDispMode;

        // Thread가 해당 상태로 전환했는지 확인하기 위한 테이블
        //  Thread에게 명령을 내려 보내기전에 Clear 한다. 
        EOpStatus[] m_ThreadStatusArray = new EOpStatus[MAX_THREAD_CHANNEL];

        // switch status
        bool m_bStartPressed;
        bool m_bStepStopPressed;
        bool m_bResetPressed;
        bool m_bEStopPressed;
        bool m_bDoorOpened;

        // Run 도중에 Error가 이미 감지됐음
        bool m_bIsErrorDetected_WhenRun;

        // Alarm Logging을 위한 Path 지정
        string m_strLogPath;
        // Alarm 정보를 읽어오기 위한 Path 지정
        string m_strDataPath;

        // Tower Lamp 상태
        int m_iTowerLampSts;

        // Alram 처리 상태
        bool m_bAlarmProcFlag;

        MTickTimer m_ResetTimer = new MTickTimer();
        MTickTimer m_NoPanelTimer = new MTickTimer();
        MTickTimer m_DeivceIDCheck = new MTickTimer(); //NSMC

        ELCEqStates m_OldEqState            = ELCEqStates.eNormal;      // EQ State for LC
        ELCEqProcStates m_OldEqProcState    = ELCEqProcStates.ePause;   // EQ Process State for LC
        bool m_bInitState;          // 원점잡기나, 초기화 실행하고 있는 상태이면 true
        bool m_bLC_PM_Mode;         // LC에서 PM 명령이 온 상태면 true
        bool m_bLC_NORMAL_Mode;     // LC에서 PM 명령이 와서 실행된 후 Normal 명령이 왔을 때 true
        bool m_bCurrent_PM_Mode;        // 현재 PM 모드 상태인지
        string m_strPM_Code;
        bool m_bFaultState;//1208

        // BUFFER PANEL LOADING
        bool m_bStage_PanelLoading; // 런중 버퍼 판넬 로딩시 안전신호 온

        // UNLOAD HANDLER PANEL LOADING
        bool m_bUnloadHandler_PanelUnloading;   // Unload Handler가 Panel을 Unloading중인가를 나타내는 Flag

        bool m_bPanelExist_InFacility;  // 현재 설비에 Panel이 존재함.
        bool m_bNoPanel_TowerLamp_Flag; // 현재 설비에 Panel이 없음.
        bool m_bTimerStarFalg;

        // Clean System
        bool m_bForcedPumpingJob;       // 강제적으로 Pumping Job 실행, while(true){do always;}

	    bool m_DoingOriginReturn;	// 원점 복귀중인지의 flag

        // Message 변수
        bool m_bPushPull_RequestUnloading;
        bool m_bPushPull_StartLoading;
        bool m_bPushPull_CompleteLoading;
        bool m_bPushPull_RequestLoading;
        bool m_bPushPull_StartUnloading;
        bool m_bPushPull_CompleteUnloading;

        bool m_bAuto_RequestLoadCassette;
        bool m_bAuto_RequestUnloadCassette;

        bool m_bSupplyCassette = false;
        bool m_bSupplyWafer = false;


        public MTrsAutoManager(CObjectInfo objInfo, EThreadChannel SelfChannelNo,
            CTrsAutoManagerRefComp refComp, CTrsAutoManagerData data)
            : base(objInfo, SelfChannelNo)
        {
            m_RefComp = refComp;
            SetData(data);

            SetSystemStatus(STS_MANUAL);	// System의 상태를 STS_MANUAL 상태로 전환한다.

        }

        public int SetData(CTrsAutoManagerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsAutoManagerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        int InitializeAllThread()
        {
            int iResult = SUCCESS;

            iResult = m_RefComp.trsLoader.Initialize();
            if (iResult != SUCCESS) return iResult;
            m_RefComp.OpPanel.SetInitFlag(EUnitIndex.LOADER, true);


            return iResult;
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

        public override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bState = false;

            // timer start if it is needed.
            
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

                // 
                iResult = ProcessRealInterface();
                if(iResult != SUCCESS)
                {
                    SendMsg(MSG_PROCESS_ALARM, ObjInfo.ID, iResult);
                }

                // do other job

                switch (eOpStatus)
                {
                    case STS_MANUAL: // Manual Mode
                        break;

                    case STS_ERROR_STOP: // Error Stop
                        break;

                    case STS_STEP_STOP: // Step Stop
                        break;

                    case STS_RUN_READY: // Run Ready
                        break;

                    case STS_CYCLE_STOP: // Cycle Stop
                        break;

                    case STS_RUN: // auto run
                        switch (ThreadStep)
                        {
                            default:
                                break;
                        }
                        break;
                }

                Sleep(ThreadSleepTime);
                //Debug.WriteLine(ToString() + " Thread running..");
            }

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

                case (int)MSG_MANUAL_CNF:
                    SetThreadStatus(evnt.Sender, STS_MANUAL); // 메세지를 보낸 Thread를 STS_MANUAL 상태로 놓는다.
                    if (CheckAllThreadStatus(STS_MANUAL))       // 모든 Thread가 STS_MANUAL 상태인지 확인한다.
                    {
                        SetOpStatus(STS_MANUAL);
                        m_bExchangeMode = false;
                        m_bErrDispMode = false;
                        m_bBuzzerMode = false;
                        // m_bAlarmProcFlag = false;

                        //setVelocityMode(VELOCITY_MODE_SLOW);	// Manual일 때 느린 속도로 이동

                        SendMessageToMainWnd(WM_START_MANUAL_MSG);

                        //m_RefComp.m_pManageOpPanel->SetAutoManual(MANUAL);
                    }
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

                case (int)MSG_PROCESS_ALARM:
                    //if (AfxGetApp()->GetMainWnd() != NULL)
                    //{
                    //    if (((CMainFrame*)AfxGetApp()->GetMainWnd())->m_pErrorDlg == NULL)
                    //        return processAlarm(evnt);  // process에서 올라온 알람메세지의 처리
                    //}
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

            }
            return DEF_Error.SUCCESS;
        }


        void SetThreadStatus(int iIndex, EOpStatus status)
        {
            m_ThreadStatusArray[iIndex] = status;
        }

        void ClearAllThreadStatus()
        {
            for (int iIndex = 1; iIndex <= GetThreadsCount(); iIndex++)
            {
                m_ThreadStatusArray[iIndex] = EOpStatus.NONE;
            }
        }

        bool CheckAllThreadStatus(EOpStatus status)
        {
            for (int iIndex = 1; iIndex <= GetThreadsCount() ; iIndex++)
            {
                if (iIndex == (int)EThreadChannel.TrsAutoManager) continue;

                if (m_ThreadStatusArray[iIndex] != status)
                {
                    return false;
                }
            }

            return true;
        }

        void SetSystemStatus(EOpStatus iStatus)
        {
            if (SetOpStatus(iStatus) == false) return;

            bool bStatus;

            if (eOpStatus == STS_RUN)
            {
                // 설비가 Live 상태임을 알리는 oUpper_Alive 신호는 On
                //m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOnMsg(PRE_EQ, oUpper_Alive);
                //m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOnMsg(NEXT_EQ, oLower_Alive);
            }
            else
            {
                if (eOpStatus_Old == STS_RUN)
                {
                    //// 설비가 Live 상태임을 알리는 oUpper_Alive 신호는 On
                    //m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOffMsg(PRE_EQ, oUpper_Alive);
                    //m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOffMsg(NEXT_EQ, oLower_Alive);

                    ////kshong Door Interlock
                    //m_RefComp.m_pManageOpPanel->GetDoorSWStatus(&bStatus);
                    //if (bStatus)
                    //{
                    //    m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOffMsg(PRE_EQ, oUpper_SI_DoorOpen); //B접
                    //    m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOffMsg(NEXT_EQ, oLower_SI_DoorOpen); //B접
                    //}
                    //else
                    //{
                    //    m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOnMsg(PRE_EQ, oUpper_SI_DoorOpen); //B접
                    //    m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOnMsg(NEXT_EQ, oLower_SI_DoorOpen); //B접
                    //}
                }
            }
        }

        int ProcessRealInterface()
        {
            int iResult = SUCCESS;
            bool bStatus;
            // 설비가 Auto Run 상태인지 Manual, Stop 상태 인지 알림.
            switch (eOpStatus)
            {
                // 수동 모드일 경우 아무것도 안함
                case STS_MANUAL:

                    break;

                // Error Stop Status에서는 Stage Auto Run 작업정지
                case STS_ERROR_STOP:
                    break;

                // Step Stop Status에서는 Stage Auto Run 작업정지
                case STS_STEP_STOP:
                    break;

                // Start Run Status에서는 Stage Auto Run 작업개시
                case STS_RUN_READY:

                    break;

                // Cycle Stop Status에서는 Cell Loading 까지 작업을 완료함
                case STS_CYCLE_STOP:
                    break;

                // Run Status에서는
                case STS_RUN:
                    break;
            }

            // 상류, 하류 설비에 MelsecNet, EStop, 유닛 포지션에 따라 IO를 이용한 신호 전송

            return SUCCESS;
        }

        /// <summary>
        /// Process에서 발생한 알람을 처리한다.
        /// </summary>
        /// <returns></returns>
        private int ProcessAlarm(MEvent evAlarm)
        {
            int iProcessID = evAlarm.wParam;
            int iUnitObjectID = (int)((evAlarm.lParam & 0xffff0000) >> 16);
            int iUnitErrorBase;
            int iUnitErrorCode;

            int tt = (int)(evAlarm.lParam & 0xffff0000);

            // 	if(iUnitObjectID >= 30000)
            // 	{
            // 		iUnitErrorBase = 30000;
            // 		iUnitErrorCode = (evAlarm.lParam & 0x0000ffff) % 200;
            // 	}
            // 	else if(iUnitObjectID >= 10000)
            // 	{
            // 		iUnitErrorBase = 10000;
            // 		iUnitErrorCode = (evAlarm.lParam & 0x0000ffff) % 200;
            // 	}
            // 	else
            // 	{
            iUnitErrorBase = (int)((evAlarm.lParam & 0x0000ffff) / 100 * 100.0);    //-> ErrorBase로 Component 이름 찾아오기 기능 구현 필요
            iUnitErrorCode = (evAlarm.lParam & 0x0000ffff) % 100;
            //	}

            // View에 메세지를 보내서 처리하게 한다.
            //bool bErrorStop;
            //// Setup or Execute.. = run
            //if (m_RefComp.m_pTrsLCNet->m_eEqProcState == 3 || m_RefComp.m_pTrsLCNet->m_eEqProcState == 5)
            //    bErrorStop = true;
            //else bErrorStop = false;

            //if (m_iRunStopSts == STS_RUN) bErrorStop = true;
            //else bErrorStop = false;

            //if (bErrorStop == false) return SUCCESS;
            //if (bErrorStop)
            {
                // display dialog
                SendMessageToMainWnd(EWindowMessage.WM_ALARM_MSG, evAlarm.wParam, evAlarm.lParam);
            }

            return SUCCESS;
        }

    }
}
