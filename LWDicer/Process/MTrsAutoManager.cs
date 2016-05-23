﻿using System;
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
using static LWDicer.Control.DEF_Thread.ERunMode;
using static LWDicer.Control.DEF_Thread.ERunStatus;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_DataManager;
using static LWDicer.Control.DEF_CtrlOpPanel;
using static LWDicer.Control.DEF_TrsAutoManager;

namespace LWDicer.Control
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

            public MDataManager DataManager;

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
        ERunStatus[] m_ThreadStatusArray = new ERunStatus[MAX_THREAD_CHANNEL];

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
            m_RefComp.OpPanel.SetInitFlag(EInitiableUnit.LOADER, true);


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

#if SIMULATION_IO
                // check op panel
                iResult = ProcessOpPanel();
                if (iResult != SUCCESS)
                {
                    SendMsg(MSG_PROCESS_ALARM, ObjInfo.ID, iResult);
                }
                // check interface with other facility
                iResult = ProcessRealInterface();
                if(iResult != SUCCESS)
                {
                    SendMsg(MSG_PROCESS_ALARM, ObjInfo.ID, iResult);
                }

                // process pumping job


                // do other job
#endif

                switch (RunStatus)
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
                case (int)MSG_PROCESS_ALARM:
                    //if (AfxGetApp()->GetMainWnd() != NULL)
                    //{
                    //    if (((CMainFrame*)AfxGetApp()->GetMainWnd())->m_pErrorDlg == NULL)
                    //        return processAlarm(evnt);  // process에서 올라온 알람메세지의 처리
                    //}
                    return ProcessAlarm(evnt);  // process에서 올라온 알람메세지의 처리
                    break;

                // MSG_MANUAL COMMAND에 대한 Thread의 응답
                case (int)MSG_MANUAL_CNF:
                    SetThreadStatus(evnt.wParam, STS_MANUAL); // 메세지를 보낸 Thread를 STS_MANUAL 상태로 놓는다.
                    if (CheckAllThreadStatus(STS_MANUAL))       // 모든 Thread가 STS_MANUAL 상태인지 확인한다.
                    {
                        SetSystemStatus(STS_MANUAL);
                        m_bExchangeMode = false;
                        m_bErrDispMode = false;
                        m_bBuzzerMode = false;
                        // m_bAlarmProcFlag = false;

                        //setVelocityMode(VELOCITY_MODE_SLOW);	// Manual일 때 느린 속도로 이동

                        SendMsgToMainWnd(WM_START_MANUAL_MSG);

                        m_RefComp.ctrlOpPanel.SetAutoManual(EAutoManual.MANUAL);
                    }
                    break;

                // 화면으로 부터의 START_RUN Command (화면 Start 버튼을 누름)
                case (int)MSG_START_RUN_CMD:

                    OnRunReady();

                    break;

                // 	case (int)MSG_CONFIRM_ENG_DOWN:
                // 		// 2010.01.20 by ranian
                // 		// 메인윈도우로 대화상자표시 메세지 날리고 거기서 메세지를 리턴 받은곳에서
                // 		// 처리하도록 변경
                // 		m_bConfirmEngDown = true;
                // 
                // 		break;

                // MSG_START_RUN COMMAND에 대한 Thread의 응답
                case (int)MSG_START_RUN_CNF:
                    SetThreadStatus(evnt.wParam, STS_RUN_READY);  // 메세지를 보낸 Thread를 STS_RUN_READY 상태로 놓는다.
                    if (CheckAllThreadStatus(STS_RUN_READY))        // 모든 Thread가 STS_RUN_READY 상태인지 확인한다.
                    {
                        SetSystemStatus(STS_RUN_READY);

                        m_bExchangeMode = false;
                        m_bErrDispMode = false;
                        m_bBuzzerMode = false;

                        m_RefComp.ctrlOpPanel.SetAutoManual(EAutoManual.AUTO);  // 2004.11.22 pys, Ready 상태에서 Door 열렸을 때 확실히 확인하기 위해 막음..

                        SendMsgToMainWnd(WM_START_READY_MSG);

#if SIMULATION_MOTION
                        OnRun();    //-> TEST용
#endif
                    }
                    break;

                // MSG_START_CMD에 대한 Thread의 응답
                case (int)MSG_START_CNF:
                    SetThreadStatus(evnt.wParam, STS_RUN);    // 메세지를 보낸 Thread를 STS_RUN 상태로 놓는다.
                    if (CheckAllThreadStatus(STS_RUN))      // 모든 Thread가 STS_RUN 상태인지 확인한다.
                    {
                        // Run 시작시 Panel이 존재하는지 Check한다.
                        CheckPanelExist();

                        SetSystemStatus(STS_RUN);

                        m_RefComp.ctrlOpPanel.SetAutoManual(EAutoManual.AUTO);

                        SendMsgToMainWnd(WM_START_RUN_MSG);

                    }

                    break;


                // MSG_STEP_STOP_CMD에 대한 Thread의 응답
                case (int)MSG_STEP_STOP_CNF: // STEP_STOP CMD에 대한 Thread들의 STS_STEP_STOP 확인 메세지
                    SetThreadStatus(evnt.wParam, STS_STEP_STOP);  // 메세지를 보낸 Thread를 STS_STEP_STOP 상태로 놓는다.
                    if (CheckAllThreadStatus(STS_STEP_STOP))        // 모든 Thread가 STS_RUN 상태인지 확인한다.
                    {
                        if (RunStatus == STS_RUN)
                        {
                            /*	clearAllThreadStatus();	// 확인을 위한 Thread의 상태를 Clear한다.
                            BroadcastMsg(MSG_START_RUN_CMD);

                            SetSystemStatus(STS_RUN_READY);	// System의 상태를 STS_RUN_READY 상태로 전환한다.
                            }
                            else 
                            { 임시로 주석 테스트 반드시 필요..*/
                            ClearAllThreadStatus(); // 확인을 위한 Thread의 상태를 Clear한다.
                                                    //BroadcastMsg(MSG_MANUAL_CMD);
                                                    //SetSystemStatus(STS_MANUAL);
                            BroadcastMsg(MSG_STEP_STOP_CMD);
                            SetSystemStatus(STS_STEP_STOP);


                        }
                    }
                    break;

                // MSG_ERROR_STOP_CMD에 대한 Thread의 응답
                case (int)MSG_ERROR_STOP_CNF:    // MSG_ERROR_STOP CMD에 대한 Thread들의 ERROR_STEP_STOP 확인 메세지
                    SetThreadStatus(evnt.wParam, STS_ERROR_STOP); // 메세지를 보낸 Thread를 STS_RUN 상태로 놓는다.
                    if (CheckAllThreadStatus(STS_ERROR_STOP))       // 모든 Thread가 STS_RUN 상태인지 확인한다.
                    {
                        //			clearAllThreadStatus();	// 확인을 위한 Thread의 상태를 Clear한다.
                        //			BroadcastMsg(MSG_MANUAL_CMD);
                    }
                    break;

                // 화면에서 명령을 통해 CYCLE_STOP Command가 오는 경우의 처리
                case (int)MSG_CYCLE_STOP_CMD:    // 화면에서 명령을 통해 CYCLE_STOP Command가 오는 경우의 처리
                    OnCycleStop();          // Cycle Stop을 처리 한다.

                    break;

                // CYCLE_STOP CMD에 대한 Thread의 응답
                case (int)MSG_CYCLE_STOP_CNF:    // CYCLE_STOP CMD에 대한 Thread들의 STS_CYCLE_STOP 확인 메세지
                    SetThreadStatus(evnt.wParam, STS_CYCLE_STOP); // 메세지를 보낸 Thread를 STS_RUN 상태로 놓는다.
                    if (CheckAllThreadStatus(STS_CYCLE_STOP))       // 모든 Thread가 STS_RUN 상태인지 확인한다.
                    {
                        OnStepStop();   // System 전체를 StepStop으로 전환한다..
                    }
                    break;

                // Error Message만 Display한 경우를 위한 Message
                case (int)MSG_ERROR_DISPLAY_REQ:
                    m_bErrDispMode = true;
                    m_bBuzzerMode = true;
                    break;

                // Error Message만 Display한 경우를 완료하기 위한 Message
                case (int)MSG_ERROR_DISPLAY_END:
                    m_bErrDispMode = false;
                    break;
                case (int)MSG_OP_CALL_REQ:
                    m_bOpCallMode = true;
                    m_bBuzzerMode = true;
                    break;

                case (int)MSG_OP_CALL_END:
                    m_bOpCallMode = false;
                    break;

                case (int)MSG_LC_PAUSE:
                    OnStepStop();
                    break;

                case (int)MSG_LC_RESUME:
                    OnRun();
                    break;

                //case (int)MSG_LC_PM:
                //    // 설비가 error 상태일땐 pm을 무시한다. 07.10.26 by ranian + 자동화 박인용
                //    if (RunStatus == STS_ERROR_STOP) break;
                //    //		OnRun();
                //    m_bLC_PM_Mode = true;

                //    PostMsg(TrsStage1, MSG_AUTOMSG_STAGE1_LC_PM); //panel 받지 말기
                //    break;

                //case (int)MSG_LC_NORMAL:
                //    m_bLC_NORMAL_Mode = true;
                //    m_bLC_PM_Mode = false;

                //    PostMsg(TrsStage1, MSG_AUTOMSG_STAGE1_LC_NORMAL); //panel 받지 말기를 이전으로 되돌리기
                //    OnRunReady();
                //    break;

                case (int)MSG_STAGE_LOADING_START:
                    m_bStage_PanelLoading = true;
                    break;

                case (int)MSG_STAGE_LOADING_END:
                    m_bStage_PanelLoading = false;
                    break;

                case (int)MSG_UNLOADHANDLER_UNLOADING_START:
                    m_bUnloadHandler_PanelUnloading = true;
                    break;

                case (int)MSG_UNLOADHANDLER_UNLOADING_END:
                    m_bUnloadHandler_PanelUnloading = false;
                    break;

                case (int)MSG_PANEL_INPUT:   // Panel이 Input.
                    if (RunStatus == STS_RUN)
                    {
                        m_bPanelExist_InFacility = true;
                    }
                    break;

                case (int)MSG_PANEL_OUTPUT:  // Panel이 Output.
                    if (RunStatus == STS_RUN)
                    {
                        CheckPanelExist();
                    }
                    break;
            }

            return DEF_Error.SUCCESS;
        }


        void SetThreadStatus(int iIndex, ERunStatus status)
        {
            m_ThreadStatusArray[iIndex] = status;
        }

        void ClearAllThreadStatus()
        {
            for (int iIndex = 1; iIndex <= GetThreadsCount(); iIndex++)
            {
                m_ThreadStatusArray[iIndex] = ERunStatus.NONE;
            }
        }

        bool CheckAllThreadStatus(ERunStatus status)
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

        void SetSystemStatus(ERunStatus status)
        {
            if (SetRunStatus(status) == false) return;

            bool bStatus;

            if (RunStatus == STS_RUN)
            {
                // 설비가 Live 상태임을 알리는 oUpper_Alive 신호는 On
                //m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOnMsg(PRE_EQ, oUpper_Alive);
                //m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOnMsg(NEXT_EQ, oLower_Alive);
            }
            else
            {
                if (RunStatus_Old == STS_RUN)
                {
                    //// 설비가 Live 상태임을 알리는 oUpper_Alive 신호는 On
                    //m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOffMsg(PRE_EQ, oUpper_Alive);
                    //m_RefComp.m_pC_InterfaceCtrl.SendInterfaceOffMsg(NEXT_EQ, oLower_Alive);

                    ////kshong Door Interlock
                    //m_RefComp.ctrlOpPanel.GetDoorSWStatus(out bStatus);
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
        bool CheckStartRunCondition()
        {
            if (RunStatus == STS_MANUAL) return true;
            else return false;
        }

        /// <summary>
        /// System이 RUN이 되기위한 조건을 체크한다.
        /// </summary>
        /// <returns></returns>
        bool CheckRunCondition()
        {
            if (RunStatus == STS_RUN_READY || RunStatus == STS_STEP_STOP)
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
            if ((RunStatus == STS_RUN) ||
                (RunStatus == STS_RUN_READY) ||
                (RunStatus == STS_ERROR_STOP)//) ||
                                                 //		(RunStatus == STS_STEP_STOP)
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
            if ((RunStatus != STS_MANUAL) &&
                (RunStatus != STS_ERROR_STOP))
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
            if (RunStatus == STS_RUN)
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

        private void SetLampBuzzerMode(ERunStatus status)
        {
            switch (status)
            {
                case ERunStatus.STS_MANUAL:
                    m_LampBuzzerMode = ELampBuzzerMode.STEPSTOP;
                    break;
                case ERunStatus.STS_RUN_READY:
                    m_LampBuzzerMode = ELampBuzzerMode.START;
                    break;
                case ERunStatus.STS_RUN:
                    m_LampBuzzerMode = ELampBuzzerMode.RUN;
                    break;
                case ERunStatus.STS_STEP_STOP:
                    m_LampBuzzerMode = ELampBuzzerMode.STEPSTOP;
                    break;
                case ERunStatus.STS_ERROR_STOP:
                    m_LampBuzzerMode = ELampBuzzerMode.ERRORSTOP_ING;
                    break;
                case ERunStatus.STS_CYCLE_STOP:
                    m_LampBuzzerMode = ELampBuzzerMode.CYCLESTOP_ING;
                    break;
                case ERunStatus.STS_OP_CALL:
                    m_LampBuzzerMode = ELampBuzzerMode.OP_CALL;
                    break;
                case ERunStatus.STS_EXC_MATERIAL:
                    m_LampBuzzerMode = ELampBuzzerMode.PARTSEMPTY;
                    break;
            }
        }

        int ProcessOpPanel()
        {
            bool bStatus;
            int iResult;

            // 장비 START Switch Check
            iResult = m_RefComp.ctrlOpPanel.GetStartSWStatus(out bStatus);

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
            iResult = m_RefComp.ctrlOpPanel.GetResetSWStatus(out bStatus);
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
            iResult = m_RefComp.ctrlOpPanel.GetStopSWStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                ChangeMonitorTouchControl();

                if (!m_bStepStopPressed)    // 이전부터 눌러져 있지않은 경우
                {
                    // 2010.09.29 by ranian
                    // Error 발생상태에서 stop 버튼 누를시 바로 나가도록
                    if (RunStatus == STS_ERROR_STOP)
                    {
                        ClearAllThreadStatus(); // 확인을 위한 Thread의 상태를 Clear한다.
                        BroadcastMsg(MSG_MANUAL_CMD);

                        SetSystemStatus(STS_MANUAL);    // System의 상태를 STS_RUN_READY 상태로 전환한다.
                        SendMsgToMainWnd(WM_START_MANUAL_MSG);
                    }
                    else if (RunStatus != STS_STEP_STOP)
                    {
                        m_bStepStopPressed = true;
                        OnStepStop();   // 장비 STEP_STOP 동작을 수행 한다.
                    }
                    else
                    {
                        bStatus = CheckAllThreadStatus(STS_STEP_STOP);

                        if (bStatus == false)   // 아직 Step Stopping
                        {
                            CheckAllThreadStatus(STS_STEP_STOP);
                            // 					BroadcastMsg(MSG_STEP_STOP_CMD);
                            // 					SetSystemStatus(STS_MANUAL);
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

                            if (RunStatus != STS_RUN_READY)
                            {
                                ClearAllThreadStatus(); // 확인을 위한 Thread의 상태를 Clear한다.
                                BroadcastMsg(MSG_MANUAL_CMD);

                                SetSystemStatus(STS_MANUAL);    // System의 상태를 STS_RUN_READY 상태로 전환한다.
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
            iResult = m_RefComp.ctrlOpPanel.GetEStopSWStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                if (!m_bEStopPressed)   // 이전부터 눌러져 있지않은 경우
                {
                    m_bEStopPressed = true;

                    //			OnErrorStop();	// 장비 ERROR_STOP 동작을 수행 한다.

                    if (m_DoingOriginReturn != true // 원점복귀 동작중엔 안함. dlg에서 처리 by ranian.
                        && RunStatus == STS_RUN)
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
            if ((RunStatus == STS_RUN) || (RunStatus == STS_RUN_READY))
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
            if (RunStatus != STS_RUN)
            {
                // Error Stop 상태이면 
                if (RunStatus == STS_ERROR_STOP)
                {
                    if (m_bBuzzerMode == false)
                        SetLampBuzzerMode(ELampBuzzerMode.ERRORSTOP_NOBUZZER);
                    else
                        SetLampBuzzerMode(ELampBuzzerMode.ERRORSTOP_ING);
                }
                else
                    SetLampBuzzerMode(RunStatus);
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
                if (RunStatus == STS_RUN)
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
            if (m_bEStopPressed == true && RunStatus != STS_ERROR_STOP)
            {
                SetLampBuzzerMode(ELampBuzzerMode.ESTOP_PRESSED);
            }

            //Manual 일때 Amp Fault 상태 Check
            if (RunStatus != STS_RUN)
            {
                m_RefComp.ctrlOpPanel.GetMotorAmpFaultStatus(out bStatus);
                if (bStatus)
                {
                    m_RefComp.ctrlOpPanel.ResetAllInitFlag();
                }
            }

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
            iUnitErrorBase = (int)((evAlarm.lParam & 0x0000ffff) / 100 * 100.0);    //. ErrorBase로 Component 이름 찾아오기 기능 구현 필요
            iUnitErrorCode = (evAlarm.lParam & 0x0000ffff) % 100;
            //	}

            // View에 메세지를 보내서 처리하게 한다.
            //bool bErrorStop;
            //// Setup or Execute.. = run
            //if (m_RefComp.m_pTrsLCNet.m_eEqProcState == 3 || m_RefComp.m_pTrsLCNet.m_eEqProcState == 5)
            //    bErrorStop = true;
            //else bErrorStop = false;

            //if (RunStatus == STS_RUN) bErrorStop = true;
            //else bErrorStop = false;

            //if (bErrorStop == false) return SUCCESS;
            //if (bErrorStop)
            {
                // display dialog
                SendMsgToMainWnd(EWindowMessage.WM_ALARM_MSG, evAlarm.wParam, evAlarm.lParam);
            }

            return SUCCESS;
        }

        /// <summary>
        /// RunReady 요건 발생시 처리
        /// </summary>
        void OnRunReady()
        {
            if (CheckStartRunCondition())
            {
                ClearAllThreadStatus(); // 확인을 위한 Thread의 상태를 Clear한다.
                BroadcastMsg(MSG_START_RUN_CMD);

                SetSystemStatus(STS_RUN_READY);
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
                SetSystemStatus(STS_RUN);

                SendMsgToMainWnd(WM_START_RUN_MSG);
            }
        }

        /// <summary>
        /// StepStop 요건 발생시 처리
        /// </summary>
        void OnStepStop()
        {
            if (RunStatus == STS_MANUAL)
                m_RefComp.ctrlOpPanel.StopAllAxis();

            if (CheckStepStopCondition())   // Step Stop Condition을 만족할 경우만 처리한다.
            {
                SetSystemStatus(STS_STEP_STOP); // System의 상태를 STEP_STOP 상태로 전환한다.
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
                SetSystemStatus(STS_CYCLE_STOP);    // System의 상태를 STS_CYCLE_STOP 상태로 전환한다.
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
                SetSystemStatus(STS_ERROR_STOP);    // System의 상태를 STS_ERROR_STOP 상태로 전환한다.
                m_bBuzzerMode = true;
                m_bErrDispMode = true;

                ClearAllThreadStatus();             // 확인을 위한 Thread의 상태를 Clear한다.

                // 모든 축을 E-Stop 시킨다.
                //		m_RefComp.ctrlOpPanel.EStopAllAxis();		//. Error Stop시 각 Thread에서 하던 작업 마무리하고 전환될 수 있도록 막음
                // 모든 축을 Stop 시킨다.
                m_RefComp.ctrlOpPanel.StopAllAxis();      //. Error Stop시 각 Thread에서 하던 작업 마무리하고 전환될 수 있도록 막음

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
            m_RefComp.ctrlStage1.IsPanelDetected(out m_bPanelExist_InFacility);
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
            m_RefComp.DataManager.SystemData.VelocityMode = mode;
            m_RefComp.DataManager.SaveSystemData(m_RefComp.DataManager.SystemData);

            // Velocity Mode에 맞는 속도 Read
            //for (int i = 0; i < m_RefComp.DataManager.SystemData_Axis.MPMotionData.Length ; i++)
            //{
            //    m_RefComp.DataManager.GetMotorParameter(i, &sMotorData);
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
