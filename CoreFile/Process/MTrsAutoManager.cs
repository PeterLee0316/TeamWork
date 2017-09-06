using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_LCNet;
using static Core.Layers.DEF_Thread.EWindowMessage;
using static Core.Layers.DEF_Thread.EThreadMessage;
using static Core.Layers.DEF_Thread.EThreadChannel;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_OpPanel;
using static Core.Layers.DEF_CtrlOpPanel;
using static Core.Layers.DEF_TrsAutoManager;

using Core.UI;

namespace Core.Layers
{
    public class DEF_TrsAutoManager
    {
        public const int ERR_TRS_AUTO_MANAGER_LOG_NULL_POINTER     = 1;
        public const int ERR_TRS_AUTO_MANAGER_NULL_DATA            = 2;
        public const int ERR_TRS_AUTO_MANAGER_CANNOT_EXECUTE_CMD   = 3;
        public const int ERR_TRS_AUTO_MANAGER_ESTOP_PRESSED        = 4;
        public const int ERR_TRS_AUTO_MANAGER_MELSEC_REOPEN_FAILED = 5;
        public const int ERR_TRS_AUTO_MANAGER_DOOR_OPENED          = 6;

        public class CTrsAutoManagerRefComp
        {
            public IIO IO;

            public MYaskawa YMC;
            public MACS ACS;

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
            public bool UseVIPMode;

        }
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
        EAutoRunStatus[] m_ThreadStatusArray = new EAutoRunStatus[(int)EThreadChannel.MAX];

        // switch status
        bool m_bStartPressed;
        bool m_bStepStopPressed;
        bool m_bResetPressed;
        bool m_bEStopPressed;
        bool m_bDoorOpened;

        // Run 도중에 Error가 이미 감지됐음
        bool IsErrorDetected_WhenRun;

        // Alarm Logging을 위한 Path 지정
        string m_strLogPath;
        // Alarm 정보를 읽어오기 위한 Path 지정
        string m_strDataPath;

        // Tower Lamp 상태
        ELampBuzzerMode m_LampBuzzerMode;

        // Alram 처리 상태
        bool m_bAlarmProcFlag;

        MTickTimer m_ResetTimer = new MTickTimer();
        MTickTimer m_NoPanelTimer = new MTickTimer();
        MTickTimer m_DeivceIDCheck = new MTickTimer(); //NSMC

        ELCEqStates m_OldEqState            = ELCEqStates.eNormal;      // EQ State for LC
        ELCEqProcStates m_OldEqProcState    = ELCEqProcStates.ePause;   // EQ Process State for LC

        public bool IsInitState;          // 원점잡기나, 초기화 실행하고 있는 상태이면 true
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


        public MTrsAutoManager(CObjectInfo objInfo, EThreadChannel SelfChannelNo, MDataManager DataManager, ELCNetUnitPos LCNetUnitPos,
            CTrsAutoManagerRefComp refComp, CTrsAutoManagerData data)
            : base(objInfo, SelfChannelNo, DataManager, LCNetUnitPos)
        {
            m_RefComp = refComp;
            SetData(data);

            SetSystemStatus(EAutoRunStatus.STS_MANUAL);	// System의 상태를 EAutoRunStatus.STS_MANUAL 상태로 전환한다.
            TSelf = (int)EThreadUnit.AUTOMANAGER;
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
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
            m_RefComp.OpPanel.SetInitFlag(EThreadUnit.LOADER, true);


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
        #endregion

        protected override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bStatus, bStatus1, bStatus2;

            // 160812 by ranian. OpenController 함수에서 com port를 열어주지만, 
            // Yaskawa는 쓰레드마다 comport을 열어줘야하다고 한다.
            m_RefComp.YMC.OpenComPortOnly();

            // timer start if it is needed.

            while (true)
            {
                // if thread has been suspended
                if (IsAlive == false)
                {
                    Sleep(ThreadSuspendedTime);
                    continue;
                }

                // 160905 MainUI가 Load된 후에 process 동작 및 에러보고 할 수 있도록
                if (CMainFrame.IsFormLoaded == false)
                {
                    Sleep(ThreadSleepTime);
                    continue;
                }

                // check message from other thread
                CheckMsg(1);

#if !SIMULATION_IO
                // 160905 런상태에서만 oppanel 및 interface를 점검하는것은 안됨.
                //if( RunStatus == EAutoRunStatus.STS_RUN || RunStatus == EAutoRunStatus.STS_RUN_READY)
                {
                    // check op panel
                    iResult = ProcessOpPanel();
                    if (iResult != SUCCESS)
                    {
                        SendMsg(MSG_PROCESS_ALARM, ObjInfo.ID, iResult);
                    }
                    // check interface with other facility
                    iResult = ProcessRealInterface();
                    if (iResult != SUCCESS)
                    {
                        SendMsg(MSG_PROCESS_ALARM, ObjInfo.ID, iResult);
                    }
                }

                // process pumping job


                // do other job
#endif

#if !SIMULATION_MOTION_YMC
                //m_RefComp.YMC.GetAllServoStatus();
#endif

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
                        break;

                    case EAutoRunStatus.STS_RUN: // auto run

                        // Do Thread Step
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
              Debug.WriteLine($"[{ToString()}] received message : {evnt.ToThreadMessage()}");

            // check message is valid
            //if(Enum.TryParse())
            //if (Enum.IsDefined(typeof(EThreadMessage), evnt.Msg) == false)
            //    return SUCCESS;

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
                case EThreadMessage.MSG_PROCESS_ALARM:
                    //if (AfxGetApp()->GetMainWnd() != NULL)
                    //{
                    //    if (((CMainFrame*)AfxGetApp()->GetMainWnd())->m_pErrorDlg == NULL)
                    //        return processAlarm(evnt);  // process에서 올라온 알람메세지의 처리
                    //}

                    return ProcessAlarm(evnt);  // process에서 올라온 알람메세지의 처리
                    break;

                // MSG_MANUAL COMMAND에 대한 Thread의 응답
                case EThreadMessage.MSG_MANUAL_CNF:
                    SetThreadStatus(evnt.wParam, EAutoRunStatus.STS_MANUAL); // 메세지를 보낸 Thread를 EAutoRunStatus.STS_MANUAL 상태로 놓는다.
                    if (CheckAllThreadStatus(EAutoRunStatus.STS_MANUAL))       // 모든 Thread가 EAutoRunStatus.STS_MANUAL 상태인지 확인한다.
                    {
                        SetSystemStatus(EAutoRunStatus.STS_MANUAL);
                        m_bExchangeMode = false;
                        m_bErrDispMode = false;
                        m_bBuzzerMode = false;
                        // m_bAlarmProcFlag = false;

                        //setVelocityMode(VELOCITY_MODE_SLOW);	// Manual일 때 느린 속도로 이동

                        SendMsgToMainWnd(WM_START_MANUAL_MSG);
                        CMainFrame.Core.SetAutoManual(EAutoManual.MANUAL);
                    }
                    break;

                // 화면으로 부터의 START_RUN Command (화면 Start 버튼을 누름)
                case EThreadMessage.MSG_READY_RUN_CMD:

                    OnRunReady();

                    break;

                // 	case EThreadMessage.MSG_CONFIRM_ENG_DOWN:
                // 		// 2010.01.20 by ranian
                // 		// 메인윈도우로 대화상자표시 메세지 날리고 거기서 메세지를 리턴 받은곳에서
                // 		// 처리하도록 변경
                // 		m_bConfirmEngDown = true;
                // 
                // 		break;

                // MSG_START_RUN COMMAND에 대한 Thread의 응답
                case EThreadMessage.MSG_READY_RUN_CNF:
                    SetThreadStatus(evnt.wParam, EAutoRunStatus.STS_RUN_READY);  // 메세지를 보낸 Thread를 EAutoRunStatus.STS_RUN_READY 상태로 놓는다.
                    if (CheckAllThreadStatus(EAutoRunStatus.STS_RUN_READY))        // 모든 Thread가 EAutoRunStatus.STS_RUN_READY 상태인지 확인한다.
                    {
                        SetSystemStatus(EAutoRunStatus.STS_RUN_READY);

                        m_bExchangeMode = false;
                        m_bErrDispMode = false;
                        m_bBuzzerMode = false;

                        CMainFrame.Core.SetAutoManual(EAutoManual.AUTO);  // Ready 상태에서 Door 열렸을 때 확실히 확인하기 위해 막음..
                        SendMsgToMainWnd(WM_START_READY_MSG);

#if SIMULATION_MOTION
                        OnRun();    //-> TEST용
#endif
                    }
                    break;

                // MSG_START_CMD에 대한 Thread의 응답
                case EThreadMessage.MSG_START_CNF:
                    SetThreadStatus(evnt.wParam, EAutoRunStatus.STS_RUN);    // 메세지를 보낸 Thread를 EAutoRunStatus.STS_RUN 상태로 놓는다.
                    if (CheckAllThreadStatus(EAutoRunStatus.STS_RUN))      // 모든 Thread가 EAutoRunStatus.STS_RUN 상태인지 확인한다.
                    {
                        // Run 시작시 Panel이 존재하는지 Check한다.
                        CheckPanelExist();

                        SetSystemStatus(EAutoRunStatus.STS_RUN);

                        CMainFrame.Core.SetAutoManual(EAutoManual.AUTO);

                        SendMsgToMainWnd(WM_START_RUN_MSG);
                    }
                    break;

                // MSG_STEP_STOP_CMD에 대한 Thread의 응답
                case EThreadMessage.MSG_STEP_STOP_CNF: // STEP_STOP CMD에 대한 Thread들의 EAutoRunStatus.STS_STEP_STOP 확인 메세지
                    SetThreadStatus(evnt.wParam, EAutoRunStatus.STS_STEP_STOP);  // 메세지를 보낸 Thread를 EAutoRunStatus.STS_STEP_STOP 상태로 놓는다.
                    if (CheckAllThreadStatus(EAutoRunStatus.STS_STEP_STOP))        // 모든 Thread가 EAutoRunStatus.STS_RUN 상태인지 확인한다.
                    {
                        if (RunStatus == EAutoRunStatus.STS_RUN)
                        {
                            /*	clearAllThreadStatus();	// 확인을 위한 Thread의 상태를 Clear한다.
                            BroadcastMsg(MSG_READY_RUN_CMD);

                            SetSystemStatus(EAutoRunStatus.STS_RUN_READY);	// System의 상태를 EAutoRunStatus.STS_RUN_READY 상태로 전환한다.
                            }
                            else 
                            { 임시로 주석 테스트 반드시 필요..*/
                            ClearAllThreadStatus(); // 확인을 위한 Thread의 상태를 Clear한다.
                                                    //BroadcastMsg(MSG_MANUAL_CMD);
                                                    //SetSystemStatus(EAutoRunStatus.STS_MANUAL);
                            BroadcastMsg(MSG_STEP_STOP_CMD);
                            SetSystemStatus(EAutoRunStatus.STS_STEP_STOP);
                        }
                    }
                    break;

                // MSG_ERROR_STOP_CMD에 대한 Thread의 응답
                case EThreadMessage.MSG_ERROR_STOP_CNF:    // MSG_ERROR_STOP CMD에 대한 Thread들의 ERROR_STEP_STOP 확인 메세지
                    SetThreadStatus(evnt.wParam, EAutoRunStatus.STS_ERROR_STOP); // 메세지를 보낸 Thread를 EAutoRunStatus.STS_RUN 상태로 놓는다.
                    if (CheckAllThreadStatus(EAutoRunStatus.STS_ERROR_STOP))       // 모든 Thread가 EAutoRunStatus.STS_RUN 상태인지 확인한다.
                    {
                        //			clearAllThreadStatus();	// 확인을 위한 Thread의 상태를 Clear한다.
                        //			BroadcastMsg(MSG_MANUAL_CMD);
                    }
                    break;

                // 화면에서 명령을 통해 CYCLE_STOP Command가 오는 경우의 처리
                case EThreadMessage.MSG_CYCLE_STOP_CMD:    // 화면에서 명령을 통해 CYCLE_STOP Command가 오는 경우의 처리
                    OnCycleStop();          // Cycle Stop을 처리 한다.

                    break;

                // CYCLE_STOP CMD에 대한 Thread의 응답
                case EThreadMessage.MSG_CYCLE_STOP_CNF:    // CYCLE_STOP CMD에 대한 Thread들의 EAutoRunStatus.STS_CYCLE_STOP 확인 메세지
                    SetThreadStatus(evnt.wParam, EAutoRunStatus.STS_CYCLE_STOP); // 메세지를 보낸 Thread를 EAutoRunStatus.STS_RUN 상태로 놓는다.
                    if (CheckAllThreadStatus(EAutoRunStatus.STS_CYCLE_STOP))       // 모든 Thread가 EAutoRunStatus.STS_RUN 상태인지 확인한다.
                    {
                        OnStepStop();   // System 전체를 StepStop으로 전환한다..
                    }
                    break;

                // Error Message만 Display한 경우를 위한 Message
                case EThreadMessage.MSG_ERROR_DISPLAY_REQ:
                    m_bErrDispMode = true;
                    m_bBuzzerMode = true;
                    break;

                // Error Message만 Display한 경우를 완료하기 위한 Message
                case EThreadMessage.MSG_ERROR_DISPLAY_END:
                    m_bErrDispMode = false;
                    break;

                case EThreadMessage.MSG_OP_CALL_REQ:
                    m_bOpCallMode = true;
                    m_bBuzzerMode = true;
                    break;

                case EThreadMessage.MSG_OP_CALL_END:
                    m_bOpCallMode = false;
                    break;

                case EThreadMessage.MSG_LC_PAUSE:
                    OnStepStop();
                    break;

                case EThreadMessage.MSG_LC_RESUME:
                    OnRun();
                    break;

                //case EThreadMessage.MSG_LC_PM:
                //    // 설비가 error 상태일땐 pm을 무시한다. 07.10.26 by ranian + 자동화 박인용
                //    if (RunStatus == EAutoRunStatus.STS_ERROR_STOP) break;
                //    //		OnRun();
                //    m_bLC_PM_Mode = true;

                //    PostMsg(TrsStage1, MSG_AUTOMSG_STAGE1_LC_PM); //panel 받지 말기
                //    break;

                //case EThreadMessage.MSG_LC_NORMAL:
                //    m_bLC_NORMAL_Mode = true;
                //    m_bLC_PM_Mode = false;

                //    PostMsg(TrsStage1, MSG_AUTOMSG_STAGE1_LC_NORMAL); //panel 받지 말기를 이전으로 되돌리기
                //    OnRunReady();
                //    break;

                case EThreadMessage.MSG_STAGE_LOADING_START:
                    m_bStage_PanelLoading = true;
                    break;

                case EThreadMessage.MSG_STAGE_LOADING_END:
                    m_bStage_PanelLoading = false;
                    break;

                case EThreadMessage.MSG_UNLOADHANDLER_UNLOADING_START:
                    m_bUnloadHandler_PanelUnloading = true;
                    break;

                case EThreadMessage.MSG_UNLOADHANDLER_UNLOADING_END:
                    m_bUnloadHandler_PanelUnloading = false;
                    break;

                case EThreadMessage.MSG_PANEL_INPUT:   // Panel이 Input.
                    if (RunStatus == EAutoRunStatus.STS_RUN)
                    {
                        m_bPanelExist_InFacility = true;
                    }
                    break;

                case EThreadMessage.MSG_PANEL_OUTPUT:  // Panel이 Output.
                    if (RunStatus == EAutoRunStatus.STS_RUN)
                    {
                        CheckPanelExist();
                    }
                    break;
            }

            return SUCCESS;
        }


        void SetThreadStatus(int iIndex, EAutoRunStatus status)
        {
            m_ThreadStatusArray[iIndex] = status;
        }

        void ClearAllThreadStatus()
        {
            for (int iIndex = 1; iIndex <= GetThreadsCount(); iIndex++)
            {
                m_ThreadStatusArray[iIndex] = EAutoRunStatus.NONE;
            }
        }

        bool CheckAllThreadStatus(EAutoRunStatus status)
        {
            // self channel 은 제외하고
            // for (int iIndex = 1; iIndex <= GetThreadsCount() ; iIndex++)
            for (int iIndex = 1; iIndex < (int)EThreadChannel.MAX; iIndex++)
            {
                //if (iIndex == (int)EThreadChannel.TrsAutoManager) continue;
                if ((int)GetSelfChannelNo(iIndex) == (int)EThreadChannel.TrsAutoManager) continue;

                if (m_ThreadStatusArray[iIndex] != status)
                {
                    return false;
                }
            }

            return true;
        }

        void SetSystemStatus(EAutoRunStatus status)
        {
            if (SetRunStatus(status) == false) return;

            bool bStatus;

            if (RunStatus == EAutoRunStatus.STS_RUN)
            {
                // 설비가 Live 상태임을 알리는 oUpper_Alive 신호는 On
                //m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOnMsg(PRE_EQ, oUpper_Alive);
                //m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOnMsg(NEXT_EQ, oLower_Alive);
            }
            else
            {
                if (RunStatus_Old == EAutoRunStatus.STS_RUN)
                {
                    //// 설비가 Live 상태임을 알리는 oUpper_Alive 신호는 On
                    //m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOffMsg(PRE_EQ, oUpper_Alive);
                    //m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOffMsg(NEXT_EQ, oLower_Alive);

                    ////kshong Door Interlock
                    //m_RefComp.ctrlOpPanel.CheckDoorSafety(out bStatus);
                    //if (bStatus)
                    //{
                    //    m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOffMsg(PRE_EQ, oUpper_SI_DoorOpen); //B접
                    //    m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOffMsg(NEXT_EQ, oLower_SI_DoorOpen); //B접
                    //}
                    //else
                    //{
                    //    m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOnMsg(PRE_EQ, oUpper_SI_DoorOpen); //B접
                    //    m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOnMsg(NEXT_EQ, oLower_SI_DoorOpen); //B접
                    //}
                }
            }
        }

        /// <summary>
        /// System이 RUN이 되기위한 조건을 체크한다.
        /// </summary>
        /// <returns></returns>
        bool CheckReadyRunCondition()
        {
            if (RunStatus == EAutoRunStatus.STS_MANUAL) return true;
            else return false;
        }

        /// <summary>
        /// System이 RUN이 되기위한 조건을 체크한다.
        /// </summary>
        /// <returns></returns>
        bool CheckRunCondition()
        {
            if (RunStatus == EAutoRunStatus.STS_RUN_READY || RunStatus == EAutoRunStatus.STS_STEP_STOP)
                return true;
            else
                return false;
        }

        /// <summary>
        /// System이 STEP_STOP이 되기위한 조건을 체크한다.
        /// </summary>
        /// <returns></returns>
        bool CheckStepStopCondition()
        {
            if ((RunStatus == EAutoRunStatus.STS_RUN) ||
                (RunStatus == EAutoRunStatus.STS_RUN_READY) ||
                (RunStatus == EAutoRunStatus.STS_ERROR_STOP)//) ||
                                                 //		(RunStatus == EAutoRunStatus.STS_STEP_STOP)
                )
                return true;
            else
                return false;
        }

        /// <summary>
        /// System이 ERROR_STOP이 되기위한 조건을 체크한다.
        /// </summary>
        /// <returns></returns>
        bool CheckErrorStopCondition()
        {
            if ((RunStatus != EAutoRunStatus.STS_MANUAL) &&
                (RunStatus != EAutoRunStatus.STS_ERROR_STOP))
                return true;
            else
                return false;
        }

        /// <summary>
        /// System이 CYCLE_STOP이 되기위한 조건을 체크한다.
        /// </summary>
        /// <returns></returns>
        bool CheckCycleStopCondition()
        {
            if (RunStatus == EAutoRunStatus.STS_RUN || RunStatus == EAutoRunStatus.STS_CYCLE_STOP)
                return true;
            else
                return false;
        }

        /// <summary>
        /// button들이 눌렸는지를 확인한 후에, button이 눌린쪽의 터치 스크린을 활성화 시켜준다.
        /// </summary>
        /// <param name="dSts"></param>
        void ChangeMonitorTouchControl()
        {
            bool bStatus;
            bool[] bTemp = new bool[3];
            m_RefComp.IO.IsOn(DEF_IO.iStart_SWRear, out bTemp[0]);
            m_RefComp.IO.IsOn(DEF_IO.iStop_SWRear, out bTemp[1]);
            m_RefComp.IO.IsOn(DEF_IO.iReset_SWRear, out bTemp[2]);

            bStatus = bTemp[0] || bTemp[1] || bTemp[2];

            if (bStatus)
            {
                m_RefComp.IO.OutputOn(DEF_IO.oMonitior_UsingRear);
            }
            else
            {
                m_RefComp.IO.OutputOff(DEF_IO.oMonitior_UsingRear);
            }
        }

        private void SetLampBuzzerMode(ELampBuzzerMode mode)
        {
            m_LampBuzzerMode = mode;
        }

        private void SetLampBuzzerMode(EAutoRunStatus status)
        {
            switch (status)
            {
                case EAutoRunStatus.STS_MANUAL:
                    m_LampBuzzerMode = ELampBuzzerMode.STEPSTOP;
                    break;
                case EAutoRunStatus.STS_RUN_READY:
                    m_LampBuzzerMode = ELampBuzzerMode.START;
                    break;
                case EAutoRunStatus.STS_RUN:
                    m_LampBuzzerMode = ELampBuzzerMode.RUN;
                    break;
                case EAutoRunStatus.STS_STEP_STOP:
                    m_LampBuzzerMode = ELampBuzzerMode.STEPSTOP;
                    break;
                case EAutoRunStatus.STS_ERROR_STOP:
                    m_LampBuzzerMode = ELampBuzzerMode.ERRORSTOP_ING;
                    break;
                case EAutoRunStatus.STS_CYCLE_STOP:
                    m_LampBuzzerMode = ELampBuzzerMode.CYCLESTOP_ING;
                    break;
                case EAutoRunStatus.STS_OP_CALL:
                    m_LampBuzzerMode = ELampBuzzerMode.OP_CALL;
                    break;
                case EAutoRunStatus.STS_EXC_MATERIAL:
                    m_LampBuzzerMode = ELampBuzzerMode.PARTSEMPTY;
                    break;
            }
        }

        int ProcessOpPanel()
        {
            int iResult = SUCCESS;
            bool bStatus;

            ////////////////////////////////////////////////////////////////////////////////
            // Motion쪽으로 door 상태나 estop 상태를 내려주는 역할
            iResult = m_RefComp.ctrlOpPanel.CheckDoorSafety(out bStatus);
            if (iResult != SUCCESS) m_RefComp.YMC.IsDoorOpened = true;
            else m_RefComp.YMC.IsDoorOpened = false;

            iResult = m_RefComp.ctrlOpPanel.CheckAreaSafety(out bStatus);
            if (iResult != SUCCESS) m_RefComp.YMC.IsAreaDetected = true;
            else m_RefComp.YMC.IsAreaDetected = false;

            ////////////////////////////////////////////////////////////////////////////////
            // 장비 START Switch Check
            iResult = m_RefComp.ctrlOpPanel.IsPanelSWDetected(ESwitch.START, out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                ChangeMonitorTouchControl();

                if (!m_bStartPressed)   // 이전부터 눌러져 있지않은 경우
                {
                    m_bStartPressed = true;
                    OnRun();    // 장비 RUN을 위한 동작을 수행한다.
                }
            }
            else
            {
                m_bStartPressed = false;
            }

            // 장비 RESET Switch Check
            iResult = m_RefComp.ctrlOpPanel.IsPanelSWDetected(ESwitch.RESET, out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                ChangeMonitorTouchControl();
                if (!m_bResetPressed)   // 이전부터 눌러져 있지않은 경우
                {
                    m_bResetPressed = true;
                    OnReset();  // 장비 Reset을 위한 동작을 수행한다.
                }

                double dResetSWTime = m_ResetTimer.GetElapsedTime();
                if (dResetSWTime > 10.0)
                {
                    //. TODO : Reset SW를 10초 정도 눌렀는 때 System를 Reset한다. (System Reset의 정의)
                    //AfxMessageBox("RESET");
                }
            }
            else
            {
                m_bResetPressed = false;
            }

            // 장비 STEP_STOP Switch Check
            iResult = m_RefComp.ctrlOpPanel.IsPanelSWDetected(ESwitch.STOP, out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                ChangeMonitorTouchControl();

                if (!m_bStepStopPressed)    // 이전부터 눌러져 있지않은 경우
                {
                    // 2010.09.29 by ranian
                    // Error 발생상태에서 stop 버튼 누를시 바로 나가도록
                    if (RunStatus == EAutoRunStatus.STS_ERROR_STOP)
                    {
                        ClearAllThreadStatus(); // 확인을 위한 Thread의 상태를 Clear한다.
                        BroadcastMsg(MSG_MANUAL_CMD);

                        SetSystemStatus(EAutoRunStatus.STS_MANUAL);    // System의 상태를 EAutoRunStatus.STS_RUN_READY 상태로 전환한다.
                        SendMsgToMainWnd(WM_START_MANUAL_MSG);
                    }
                    else if (RunStatus != EAutoRunStatus.STS_STEP_STOP)
                    {
                        m_bStepStopPressed = true;
                        OnStepStop();   // 장비 STEP_STOP 동작을 수행 한다.
                    }
                    else
                    {
                        bStatus = CheckAllThreadStatus(EAutoRunStatus.STS_STEP_STOP);

                        if (bStatus == false)   // 아직 Step Stopping
                        {
                            CheckAllThreadStatus(EAutoRunStatus.STS_STEP_STOP);
                            // 					BroadcastMsg(MSG_STEP_STOP_CMD);
                            // 					SetSystemStatus(EAutoRunStatus.STS_MANUAL);
                        }
                        else        // if step stop finished
                        {
                            // 2010.09.29 by ranian
                            // step_stop 상태에서 stop버튼을 누르는 경우이므로 여기서 ePause로 변경함
                            //					if(m_RefComp.m_pTrsLCNet.m_eEqProcState != ePause)
                            {
                                SendMsgToMainWnd(WM_DISP_EQSTOP_MSG); // OnDisplayEqStopDlg();

                                SendMsgToMainWnd(WM_DISP_EQ_PROC_STATE);

                                Sleep(50);
                            }

                            if (RunStatus != EAutoRunStatus.STS_RUN_READY)
                            {
                                ClearAllThreadStatus(); // 확인을 위한 Thread의 상태를 Clear한다.
                                BroadcastMsg(MSG_MANUAL_CMD);

                                SetSystemStatus(EAutoRunStatus.STS_MANUAL);    // System의 상태를 EAutoRunStatus.STS_RUN_READY 상태로 전환한다.
                                SendMsgToMainWnd(WM_START_MANUAL_MSG);
                            }
                        }
                    }
                }
            }
            else
            {
                m_bStepStopPressed = false;
            }

            // 장비 E_STOP Switch Check
            iResult = m_RefComp.ctrlOpPanel.IsPanelSWDetected(ESwitch.ESTOP, out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                if (!m_bEStopPressed)   // 이전부터 눌러져 있지않은 경우
                {
                    m_bEStopPressed = true;

                    //			OnErrorStop();	// 장비 ERROR_STOP 동작을 수행 한다.

                    if (m_DoingOriginReturn != true // 원점복귀 동작중엔 안함. dlg에서 처리 by ranian.
                        && RunStatus == EAutoRunStatus.STS_RUN)
                    {
                        SendMsg(MSG_PROCESS_ALARM, ObjectID, GenerateErrorCode(ERR_TRS_AUTO_MANAGER_ESTOP_PRESSED)); // 에러 처리
                    }

                    // 원점복귀 Flag Reset
                    m_RefComp.ctrlOpPanel.ResetAllOriginFlag();
                    // 초기화 Flag Reset;
                    m_RefComp.ctrlOpPanel.ResetAllInitFlag();

                    //m_RefComp.m_pMotionLib.PositionCompareReset(1);
                    //m_RefComp.m_pMotionLib.PositionCompareReset(2);
                    //m_RefComp.m_pC_CtrlDispenser.StopTrash(DEF_HEAD_ALL);
                }
            }
            else
            {
                m_bEStopPressed = false;
            }

            // Run 상태에서 상태 Check
            if ((RunStatus == EAutoRunStatus.STS_RUN) || (RunStatus == EAutoRunStatus.STS_RUN_READY))
            {
                iResult = m_RefComp.ctrlOpPanel.CheckAutoRun(out m_bExchangeMode);
                if (iResult != SUCCESS)
                {
                    if (!IsErrorDetected_WhenRun)   // 이전부터 감지되어 있지않은 경우
                    {
                        IsErrorDetected_WhenRun = true;
                        //				OnErrorStop();	// 장비 ERROR_STOP 동작을 수행 한다.
                        SendMsg(MSG_PROCESS_ALARM, ObjectID, iResult);   // 에러 처리
                    }
                    return iResult;
                }
                else
                {
                    IsErrorDetected_WhenRun = false;
                }
            }

            //  현재 상태에 따라 OpPanel을 제어한다.
            if (m_Data.UseVIPMode)
            {
                SetLampBuzzerMode(ELampBuzzerMode.RUN);
                goto VIPMODE;
            }

            // Run 상태가 아니면 
            if (RunStatus != EAutoRunStatus.STS_RUN)
            {
                // Error Stop 상태이면 
                if (RunStatus == EAutoRunStatus.STS_ERROR_STOP)
                {
                    if (m_bBuzzerMode == false)
                        SetLampBuzzerMode(ELampBuzzerMode.ERRORSTOP_NOBUZZER);
                    else
                        SetLampBuzzerMode(ELampBuzzerMode.ERRORSTOP_ING);
                }
                else
                {
                    SetLampBuzzerMode(RunStatus);
                }
            }
            // 아니고 Error Display 상태이면 
            else if (m_bErrDispMode == true)
            {
                if (m_bBuzzerMode == false)
                    SetLampBuzzerMode(ELampBuzzerMode.ERRORSTOP_NOBUZZER);
                else
                    SetLampBuzzerMode(ELampBuzzerMode.ERRORSTOP_ING);
            }
            // 아니고 자재 교환 상태이면 
            else if (m_bExchangeMode == true)
            {
                if (m_bBuzzerMode == false)
                    SetLampBuzzerMode(ELampBuzzerMode.PARTSEMPTY_NOBUZZER);
                else
                    SetLampBuzzerMode(ELampBuzzerMode.PARTSEMPTY);
            }
            // 아니면 일반 상태 표시 
            else
            {
                SetLampBuzzerMode(RunStatus);

                // Run 상태라면 
                if (RunStatus == EAutoRunStatus.STS_RUN)
                {
                    if (m_bPanelExist_InFacility)
                        SetLampBuzzerMode(ELampBuzzerMode.RUN);
                    else
                    {
                        if (m_RefComp.ctrlOpPanel.IsNoPanel == true)
                            SetLampBuzzerMode(ELampBuzzerMode.RUN_PANEL_NO_EXIST);
                    }

                    // Panel 정체중이면
                    if (m_RefComp.ctrlOpPanel.IsTrafficJam == true)
                        SetLampBuzzerMode(ELampBuzzerMode.RUN_TRAFFIC_JAM);
                }
            }
            // 아니고 Op call 상태이면 
            if (m_bOpCallMode == true)
            {
                SetLampBuzzerMode(ELampBuzzerMode.OP_CALL);
            }

            // Estop
            if (m_bEStopPressed == true && RunStatus != EAutoRunStatus.STS_ERROR_STOP)
            {
                SetLampBuzzerMode(ELampBuzzerMode.ESTOP_PRESSED);
            }

            //Manual 일때 Amp Fault 상태 Check
            //if (RunStatus != EAutoRunStatus.STS_RUN)
            //{
            //    m_RefComp.ctrlOpPanel.GetMotorAmpFaultStatus(out bStatus);
            //    if (bStatus)
            //    {
            //        m_RefComp.ctrlOpPanel.ResetAllInitFlag();
            //    }
            //}

            VIPMODE:
            // Tower Lamp 표시 
            if ((iResult = m_RefComp.ctrlOpPanel.SetOpPanel(m_LampBuzzerMode))
                != SUCCESS)
            {
                return iResult;
            }

            CheckPanelExist();

            return SUCCESS;
        }

        int ProcessRealInterface()
        {
            int iResult = SUCCESS;
            bool bStatus;
            // 설비가 Auto Run 상태인지 Manual, Stop 상태 인지 알림.
            switch (RunStatus)
            {
                // 수동 모드일 경우 아무것도 안함
                case EAutoRunStatus.STS_MANUAL:

                    break;

                // Error Stop Status에서는 Stage Auto Run 작업정지
                case EAutoRunStatus.STS_ERROR_STOP:
                    break;

                // Step Stop Status에서는 Stage Auto Run 작업정지
                case EAutoRunStatus.STS_STEP_STOP:
                    break;

                // Start Run Status에서는 Stage Auto Run 작업개시
                case EAutoRunStatus.STS_RUN_READY:

                    break;

                // Cycle Stop Status에서는 Cell Loading 까지 작업을 완료함
                case EAutoRunStatus.STS_CYCLE_STOP:
                    break;

                // Run Status에서는
                case EAutoRunStatus.STS_RUN:
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
            // View에 메세지를 보내서 처리하게 한다.
            bool bErrorStop;
            //// Setup or Execute.. = run
            //if (m_RefComp.m_pTrsLCNet.m_eEqProcState == 3 || m_RefComp.m_pTrsLCNet.m_eEqProcState == 5)
            //    bErrorStop = true;
            //else bErrorStop = false;

            if (RunStatus == EAutoRunStatus.STS_RUN) bErrorStop = true;
            else bErrorStop = false;

            if (bErrorStop == false) return SUCCESS;

            OnErrorStop();
            // display dialog
            SendMsgToMainWnd(EWindowMessage.WM_ALARM_MSG, evAlarm.wParam, evAlarm.lParam);

            return SUCCESS;
        }

        /// <summary>
        /// RunReady 요건 발생시 처리
        /// </summary>
        void OnRunReady()
        {
            if (CheckReadyRunCondition())
            {
                ClearAllThreadStatus(); // 확인을 위한 Thread의 상태를 Clear한다.
                BroadcastMsg(MSG_READY_RUN_CMD);

                SetSystemStatus(EAutoRunStatus.STS_RUN_READY);
            }
        }

        /// <summary>
        /// Run 요건 발생시 처리
        /// </summary>
        void OnRun()
        {
            if (CheckRunCondition())    // Run Condition을 만족할 경우만 처리한다.
            {
                SetVelocityMode(EVelocityMode.NORMAL);  // Auto일 때 정상 속도로 이동
                ClearAllThreadStatus(); // 확인을 위한 Thread의 상태를 Clear한다.
                BroadcastMsg(MSG_START_CMD);
                SetSystemStatus(EAutoRunStatus.STS_RUN);

                SendMsgToMainWnd(WM_START_RUN_MSG);
            }
        }

        /// <summary>
        /// StepStop 요건 발생시 처리
        /// </summary>
        void OnStepStop()
        {
            if (RunStatus == EAutoRunStatus.STS_MANUAL)
                m_RefComp.ctrlOpPanel.StopAllAxis();

            if (CheckStepStopCondition())   // Step Stop Condition을 만족할 경우만 처리한다.
            {
                SetSystemStatus(EAutoRunStatus.STS_STEP_STOP); // System의 상태를 STEP_STOP 상태로 전환한다.
                ClearAllThreadStatus();         // 확인을 위한 Thread의 상태를 Clear한다.
                BroadcastMsg(MSG_STEP_STOP_CMD);    // STEP_STOP Broadcasting

                SendMsgToMainWnd(WM_STEPSTOP_MSG);
            }
        }

        /// <summary>
        /// CycleStop 요건 발생시 처리
        /// </summary>
        void OnCycleStop()
        {
            if (CheckCycleStopCondition())  // CycleStop Condition을 만족할 경우만 처리한다.
            {
                SetSystemStatus(EAutoRunStatus.STS_CYCLE_STOP);    // System의 상태를 EAutoRunStatus.STS_CYCLE_STOP 상태로 전환한다.
                ClearAllThreadStatus();             // 확인을 위한 Thread의 상태를 Clear한다.
                BroadcastMsg(MSG_CYCLE_STOP_CMD);   // CYCLE_STOP Broadcasting
            }
        }

        /// <summary>
        /// ErrorStop 요건 발생시 처리
        /// </summary>
        void OnErrorStop()
        {
            // UV Off
            //	m_RefComp.m_pC_CtrlDispenser.SetUVLampAndGunForDispensing(false);
            //	m_RefComp.m_pC_CtrlDispenser.SetUVLEDAndGunForDispensing(false);

            // TESTTEST
            // 도포 관련 MMC Out IO Off
            //	m_RefComp.m_pMotionLib.PositionCompareReset(1);
            //	m_RefComp.m_pMotionLib.PositionCompareReset(2);
            //	m_RefComp.m_pC_CtrlDispenser.StopTrash(DEF_HEAD_ALL);

            if (CheckErrorStopCondition())  // ErrorStop Condition을 만족할 경우만 처리한다.
            {
                SetSystemStatus(EAutoRunStatus.STS_ERROR_STOP);    // System의 상태를 EAutoRunStatus.STS_ERROR_STOP 상태로 전환한다.
                m_bBuzzerMode = true;
                m_bErrDispMode = true;

                ClearAllThreadStatus();             // 확인을 위한 Thread의 상태를 Clear한다.

                // 모든 축을 E-Stop 시킨다.
                //		m_RefComp.ctrlOpPanel.EStopAllAxis();		//. Error Stop시 각 Thread에서 하던 작업 마무리하고 전환될 수 있도록 막음
                // 모든 축을 Stop 시킨다.
                //m_RefComp.ctrlOpPanel.StopAllAxis();      //. Error Stop시 각 Thread에서 하던 작업 마무리하고 전환될 수 있도록 막음

                BroadcastMsg(MSG_ERROR_STOP_CMD);       // ERROR_STOP Broadcasting
                SendMsgToMainWnd(WM_ERRORSTOP_MSG);
            }
        }

        /// <summary>
        /// Reset SW 발생시 처리
        /// </summary>
        void OnReset()
        {
            // Buzzer를 끈다.
            if (m_bBuzzerMode == true)
            {
                m_bBuzzerMode = false;
            }
        }

        /// <summary>
        /// 설비안에 Panel이 있는지를 io를 이용하여 check
        /// </summary>
        void CheckPanelExist()
        {
            m_RefComp.ctrlStage1.IsObjectDetected(out m_bPanelExist_InFacility);
            if (m_bPanelExist_InFacility == true) return;

            m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER, out m_bPanelExist_InFacility);
            if (m_bPanelExist_InFacility == true) return;

            m_RefComp.ctrlHandler.IsObjectDetected(DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER, out m_bPanelExist_InFacility);
            if (m_bPanelExist_InFacility == true) return;
        }

        /// <summary>
        /// Motion의 이동 속도를 설정한다.
        /// </summary>
        /// <param name="mode"></param>
        public void SetVelocityMode(EVelocityMode mode)
        {
            //CSystemData sSysData;
            //CMotorParameter sMotorData;
            //double rgdVelocity[DEF_MAX_MOTION_AXIS_NO];

            // System Data의 Velocity Mode 설정
            DataManager.SystemData.VelocityMode = mode;
            DataManager.SaveSystemData(DataManager.SystemData);

            // Velocity Mode에 맞는 속도 Read
            //for (int i = 0; i < DataManager.SystemData_Axis.MPMotionData.Length ; i++)
            //{
            //    DataManager.GetMotorParameter(i, &sMotorData);
            //    // 정상 이동 속도
            //    if (mode == EVelocityMode.NORMAL)
            //        rgdVelocity[i] = sMotorData.dRunVelocity;
            //    // 빠른 이동 속도
            //    else if (mode == EVelocityMode.FAST)
            //        rgdVelocity[i] = sMotorData.dFastRunVelocity;
            //    // 느린 이동 속도
            //    else
            //        rgdVelocity[i] = sMotorData.dSlowRunVelocity;
            //}

            //// Motion 속도 변환
            //m_RefComp.ctrlOpPanel.SetVelocityMode(rgdVelocity);
        }

    }
}
