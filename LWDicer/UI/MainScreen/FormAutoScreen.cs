using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using LWDicer.Layers;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_LCNet;

namespace LWDicer.UI
{
    public partial class FormAutoScreen : Form
    {
        int m_nStartReady = 0;      // 0:Off, 1:Ready, 2:Run

        //FormWorkPieceInquiry Dlg_WPInquiry = new FormWorkPieceInquiry();

        public FormAutoScreen()
        {
            InitializeComponent();
            InitializeForm();

            //timer1.Interval = UITimerInterval;
            timer1.Interval = 10;
            timer1.Enabled = true;
            timer1.Start();

        }
        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }


        private void FormAutoScreen_Activated(object sender, EventArgs e)
        {

        }

        private void FormAutoScreen_Deactivate(object sender, EventArgs e)
        {

        }

        private void FormAutoScreen_Shown(object sender, EventArgs e)
        {
            this.Activate();
        }

        protected override void WndProc(ref Message wMessage)
        {
            switch (wMessage.Msg)
            {
                default:
                    break;
            }

            base.WndProc(ref wMessage);
        }

        public void WindowProc(MEvent evnt)
        {
            string msg = "FormAutoScreen got message from MainFrame : " + evnt.ToWindowMessage();
            Debug.WriteLine("===================================================");
            Debug.WriteLine(msg);
            Debug.WriteLine("===================================================");

            // 변수 선언 및 작업의 편리성을 위해서 일부러 switch대신에 if/else if 구문을 사용함
            if (false)
            {

            }
            else if (evnt.Msg == (int)EWindowMessage.WM_START_READY_MSG)
            {
                //m_dlgStart.ShowWindow(SW_SHOW);
                //m_dlgErrorStop.ShowWindow(SW_HIDE);
                //m_dlgStepStop.ShowWindow(SW_HIDE);
                LabelButtonGuide.Text = "Press start button to start auto run.";
                LabelButtonGuide.Visible = true;

                // Button Disable 
                m_nStartReady = 1;
                SetButtonStatus(true);
            }
            else if (evnt.Msg == (int)EWindowMessage.WM_START_RUN_MSG)
            {
                //m_dlgStart.ShowWindow(SW_HIDE);
                //m_dlgErrorStop.ShowWindow(SW_HIDE);
                //m_dlgStepStop.ShowWindow(SW_HIDE);
                LabelButtonGuide.Text = "Auto Running....";
                LabelButtonGuide.Visible = true;

                // Button Disable 
                m_nStartReady = 2;
                SetButtonStatus(true);
            }
            else if (evnt.Msg == (int)EWindowMessage.WM_START_MANUAL_MSG)
            {
                //m_dlgStart.ShowWindow(SW_HIDE);
                //m_dlgErrorStop.ShowWindow(SW_HIDE);
                //m_dlgStepStop.ShowWindow(SW_HIDE);
                LabelButtonGuide.Text = "Manual Mode....";
                LabelButtonGuide.Visible = true;

                // Button Disable 
                m_nStartReady = 0;
                SetButtonStatus(false);
            }
            else if (evnt.Msg == (int)EWindowMessage.WM_STEPSTOP_MSG)
            {
                //m_dlgStart.ShowWindow(SW_HIDE);
                //m_dlgErrorStop.ShowWindow(SW_HIDE);
                //m_dlgStepStop.ShowWindow(SW_SHOW);
                LabelButtonGuide.Text = "Step Stop Mode";
                LabelButtonGuide.Visible = true;

                SetButtonStatus(true);
            }
            else if (evnt.Msg == (int)EWindowMessage.WM_ERRORSTOP_MSG)
            {
                //m_dlgStart.ShowWindow(SW_HIDE);
                //m_dlgErrorStop.ShowWindow(SW_SHOW);
                //m_dlgStepStop.ShowWindow(SW_HIDE);
                LabelButtonGuide.Text = "Error Stop Mode";
                LabelButtonGuide.Visible = true;

            }
            else if (evnt.Msg == (int)EWindowMessage.WM_ALARM_MSG)
            {
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_PANEL_DISTANCE_MSG1)
            {
                //m_sMeasuredDistCell1.SetWindowText(m_pTrsStage1->GetAlignResult());
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_TACTTIME_MSG)
            {
                //// EQ Tact Time 기록하기 
                //str.Format("%.2f", *(double*)wParam);
                //m_LblEqTactTime.SetWindowText(str);

                //// Line Tact Time 기록하기 
                //str.Format("%.2f", *(double*)lParam);
                //m_LblLineTactTime.SetWindowText(str);
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_PRODUCT_IN_MSG)
            {
                //m_ProductData.uiProductQuantity_forIn++;
                //str.Format("IN %d, OUT %d", m_ProductData.uiProductQuantity_forIn, m_ProductData.uiProductQuantity_forOut);
                //m_LblProductCnt.SetWindowText(str);
                //MSiSystem.SaveProductData(m_ProductData);
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_PRODUCT_OUT_MSG)
            {
                //m_ProductData.uiProductQuantity_forOut++;
                //str.Format("IN %d, OUT %d", m_ProductData.uiProductQuantity_forIn, m_ProductData.uiProductQuantity_forOut);
                //m_LblProductCnt.SetWindowText(str);
                //MSiSystem.SaveProductData(m_ProductData);

                //str.Format("%s", *(CString*)wParam);
                //m_LblAfterEquipId.SetWindowText(str);
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_EQ_STATE)
            {
                //m_LblEqState.SetWindowText(m_strEqState[m_pTrsLCNet->m_eEqState]);
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_EQ_PROC_STATE)
            {
                //m_LblEqProcessState.SetWindowText(m_strEqProcState[m_pTrsLCNet->m_eEqProcState]);
            }
            /*	else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_TERMINAL_MSG)
                {
                    m_ListTerminalMsg.DeleteString(1);
                    CTime t = CTime::GetCurrentTime();
                    CString temp;
                    temp = *(CString*)lParam;
                    sprintf(buf, "[%02d:%02d:%02d] %s", t.GetHour(), t.GetMinute(), t.GetSecond(), temp);

                    m_ListTerminalMsg.InsertString(0, buf);
                    if(m_ListTerminalMsg.GetCount()>5)
                        m_ListTerminalMsg.DeleteString(m_ListTerminalMsg.GetCount()-1);
                    m_ListTerminalMsg.ShowWindow(SW_SHOW);
                    m_ListTerminalMsg.UpdateWindow();
                }
            */
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_RUN_MODE)
            {
                //UpdateDataMembers();
            }
            //else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_MODEL_NAME)
            //{
            //    UpdateDataMembers();
            //}
            //else if (evnt.Msg ==(int)EWindowMessage.WM_NSMC_CONTROL_PANEL_SUPPLY_START)
            //{
            //    BOOL bTest = FALSE;
            //    m_BtnCellSupplyStop.SetValue(bTest);

            //    if (bTest == (int)EWindowMessage.TRUE)  // 버튼 눌려졌을 때
            //    {
            //        m_pTrsStage1->SendMsg(MSG_PANEL_SUPPLY_STOP);
            //    }
            //    else
            //    {
            //        m_pTrsStage1->SendMsg(MSG_PANEL_SUPPLY_START);
            //    }
            //}
            //else if (evnt.Msg ==(int)EWindowMessage.WM_NSMC_CONTROL_PANEL_SUPPLY_STOP)
            //{
            //    BOOL bTest = TRUE;
            //    m_BtnCellSupplyStop.SetValue(bTest);

            //    if (bTest == (int)EWindowMessage.TRUE)  // 버튼 눌려졌을 때
            //    {
            //        m_pTrsStage1->SendMsg(MSG_PANEL_SUPPLY_STOP);
            //    }
            //    else
            //    {
            //        m_pTrsStage1->SendMsg(MSG_PANEL_SUPPLY_START);
            //    }
            //}
            else
            {
                msg = "FormAutoScreen unknown message : " + evnt;
                Debug.WriteLine("***************************************************");
                Debug.WriteLine(msg);
                Debug.WriteLine("***************************************************");
            }

        }

        private void BtnOriginReturn_Click(object sender, EventArgs e)
        {
            var dlg = new FormOriginReturn();
            dlg.ShowDialog();
        }

        private void BtnUnitInit_Click(object sender, EventArgs e)
        {
            var dlg = new FormUnitInit();
            dlg.ShowDialog();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (CMainFrame.IsAlarmPopup) return; // popup 된 alarm이 확인완료될때까지 기다림
            if(BtnStart.Text == "Start AutoRun")
            {
                StartAutoRun();
            } else
            {
                StopAutoRun();
            }

        }

        private void StartAutoRun()
        {
            string strErr;
            int iResult = SUCCESS;
            bool bStatus;

            CSystemData systemData = CMainFrame.LWDicer.m_DataManager.SystemData;
            if (systemData.UseSafetySensor)
            {
                iResult = CMainFrame.LWDicer.m_ctrlOpPanel.GetDoorSWStatus(out bStatus);
                if (bStatus == true)
                {
                    CMainFrame.DisplayMsg("Door가 아직 전부 닫히지 않았습니다. 확인 후 다시 시도해주세요.", "Error");
                    return;
                }
            }

            if (CMainFrame.LWDicer.CheckSystemConfig_ForRun(out strErr) == false)
            {
                CMainFrame.DisplayMsg(strErr, "Error");
                return;
            }

            if (systemData.eOpModeStatus == EAutoRunMode.PASS_RUN)
            {
                if (CMainFrame.InquireMsg("[물류 운전] 모드입니다. 계속 진행 하시겠습니까?", "Question") == false)
                    return;
            }

            if (systemData.eOpModeStatus == EAutoRunMode.DRY_RUN)
            {
                if (CMainFrame.InquireMsg("[공 운전] 모드입니다. 계속 진행 하시겠습니까?", "Question") == false)
                    return;
            }

            //if (systemData.eOpModeStatus == EAutoRunMode.NORMAL_RUN && systemData.bRunTime_Do_Dispense == false)
            //{
            //    if (CMainFrame.InquireMsg("[자동 운전] 모드에서 도포 동작 중 토출을 하지 않습니다. 계속 진행 하시겠습니까?", "Question") == false)
            //        return;
            //}

            //if (systemData.eOpModeStatus == EAutoRunMode.REPAIR_RUN && m_pTrsStage1->GetRepairCount() <= 0)
            //{
            //    CMainFrame.InquireMsg("[REPAIR 운전] 모드에서 Rework Panel 수량이 0매입니다. 확인해주세요", "Error", M_ICONERROR);
            //    return;
            //}

            //if (systemData.eOpModeStatus == EAutoRunMode.REPAIR_RUN && systemData.bRunTime_Do_Dispense == false)
            //{
            //    if (CMainFrame.InquireMsg("[REPAIR 운전] 모드에서 도포 동작 중 토출을 하지 않습니다. 계속 진행 하시겠습니까?", "Question") == false)
            //        return;
            //}

            //if (systemData.eOpModeStatus == EAutoRunMode.NORMAL_RUN && systemData.bRunTime_Do_Cure == false)
            //{
            //    if (CMainFrame.InquireMsg("[자동 운전] 모드에서 도포 동작 중 경화를 하지 않습니다. 계속 진행 하시겠습니까?", "Question") == false)
            //        return;
            //}

            //if (systemData.eOpModeStatus == EAutoRunMode.REPAIR_RUN && systemData.bRunTime_Do_Cure == false)
            //{
            //    if (CMainFrame.InquireMsg("[REPAIR 운전] 모드에서 도포 동작 중 경화를 하지 않습니다. 계속 진행 하시겠습니까?", "Question") == false)
            //        return;
            //}
            //// check laser & scanner
            //if (systemData.eOpModeStatus == EAutoRunMode.NORMAL_RUN || systemData.eOpModeStatus == EAutoRunMode.REPAIR_RUN)
            //{
            //    if (systemData.bRunTime_Do_Dispense == true)
            //    {
            //        iResult = m_pCDispenser->CheckEFDReadyStatus(true);
            //        if (iResult)
            //        {
            //            strErr = CMainFrame.LWDicer.GetErrorMessage(iResult);
            //            CMainFrame.DisplayMsg(strErr.GetBuffer(strErr.GetLength()), "Error", M_ICONERROR);
            //            return;
            //        }
            //    }
            //}

            bool[] bInitSts;
            if (!CMainFrame.LWDicer.m_OpPanel.CheckAllInit(out bInitSts))
            {
                CMainFrame.DisplayMsg("System 초기화를 먼저 실행하세요.", "Error");
                return;
            }

            // 자동운전 사전조건 확인
            if ((iResult = CMainFrame.LWDicer.m_ctrlOpPanel.CheckBeforeAutoRun()) != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult, 0, false);
                return;
            }

            if (CMainFrame.InquireMsg("Start AutoRun?", "Question") == false)
                return;

            // button 상태 제어는 WindowProc 함수에서 automanager에서 날아온 message에 의해 제어함
            //m_nStartReady = 1;
            //SetButtonStatus(true);
            CMainFrame.LWDicer.m_trsAutoManager.SendMsg(EThreadMessage.MSG_READY_RUN_CMD);

            // Message Pop up
            // RUN SW 또는 Stop SW 눌릴 때 까지 대기
#if SIMULATION_TEST
            //m_nStartReady = 2;
            //CMainFrame.LWDicer.Sleep(1000);
            //CMainFrame.LWDicer.m_trsAutoManager.SendMsg(EThreadMessage.MSG_START_CMD);
#else

            //m_dlgStart.m_LblMessage.SetWindowText("START Button을 눌러주세요~!");
            //m_dlgStart.ShowWindow(SW_SHOW);
#endif
        }

        private void StopAutoRun()
        {
            if (CMainFrame.InquireMsg("Stop AutoRun?", "Question") == false)
                return;

            // button 상태 제어는 WindowProc 함수에서 automanager에서 날아온 message에 의해 제어함
            //m_nStartReady = 0;
            //SetButtonStatus(false);
            CMainFrame.LWDicer.m_trsAutoManager.SendMsg(EThreadMessage.MSG_CYCLE_STOP_CMD);
        }

        void SetButtonStatus(bool bRunStart)
        {
            bool bEnable = bRunStart ? false : true;
            if(bRunStart)
            {
                BtnStart.Text = "Stop AutoRun";

            }
            else
            {
                BtnStart.Text = "Start AutoRun";

            }
            //BtnStart.Enabled = bEnable;
            //		m_BtnOpMode1.Enabled = bEnable;
            //BtnModeSelect.Enabled = bEnable;
            BtnOriginReturn.Enabled = bEnable;
            BtnUnitInit.Enabled = bEnable;
            //		m_BtnCellSupplyStop.Enabled = bEnable; // For Message Transfer
            //		m_BtnPcbSupplyStop.Enabled = bEnable;
            //BtnMonitorChange.Enabled = bEnable;
            //		m_BtnCameraChange.Enabled = bEnable;

            //		testTemp->EnableWindow(bEnable);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // display thread status
            Label_StatusAutoManager.Text = CMainFrame.LWDicer.m_trsAutoManager.GetRunStatus().Replace("STS_", "");
            Label_StatusLoader.Text = CMainFrame.LWDicer.m_trsLoader.GetRunStatus().Replace("STS_", "");
            Label_StatusPushPull.Text = CMainFrame.LWDicer.m_trsPushPull.GetRunStatus().Replace("STS_", "");
            Label_StatusSpinner1.Text = CMainFrame.LWDicer.m_trsSpinner1.GetRunStatus().Replace("STS_", "");
            Label_StatusSpinner2.Text = CMainFrame.LWDicer.m_trsSpinner2.GetRunStatus().Replace("STS_", "");
            Label_StatusUHandler.Text = CMainFrame.LWDicer.m_trsHandler.GetRunStatus().Replace("STS_", "");
            Label_StatusLHandler.Text = CMainFrame.LWDicer.m_trsHandler.GetRunStatus().Replace("STS_", "");
            Label_StatusStage.Text = CMainFrame.LWDicer.m_trsStage1.GetRunStatus().Replace("STS_", "");

            // display thread step
            Label_StepAutoManager.Text      = $"InputBuffer : {CMainFrame.DataManager.GetCount_InputBuffer()}, OutputBuffer : { CMainFrame.DataManager.GetCount_OutputBuffer()}";
            Label_StepLoader.Text = CMainFrame.LWDicer.m_trsLoader.GetStep().Replace("TRS_LOADER_", "");
            Label_StepPushPull.Text      = CMainFrame.LWDicer.m_trsPushPull.GetStep().Replace("TRS_PUSHPULL_", "");
            Label_StepSpinner1.Text      = CMainFrame.LWDicer.m_trsSpinner1.GetStep().Replace("TRS_SPINNER_", "");
            Label_StepSpinner2.Text      = CMainFrame.LWDicer.m_trsSpinner2.GetStep().Replace("TRS_SPINNER_", "");
            Label_StepUHandler.Text      = CMainFrame.LWDicer.m_trsHandler.GetStep1().Replace("TRS_UPPER_HANDLER_", "");
            Label_StepLHandler.Text      = CMainFrame.LWDicer.m_trsHandler.GetStep2().Replace("TRS_LOWER_HANDLER_", "");
            Label_StepStage.Text         = CMainFrame.LWDicer.m_trsStage1.GetStep().Replace("TRS_STAGE1_", "");

            // display id
            Label_IDLoader.Text          = "I : " + CMainFrame.DataManager.GetID_InputReady().ToString();
            Label_IDPushPull.Text        = CMainFrame.DataManager.WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL].ID;
            Label_IDSpinner1.Text        = CMainFrame.DataManager.WorkPieceArray[(int)ELCNetUnitPos.SPINNER1].ID;
            Label_IDSpinner2.Text        = CMainFrame.DataManager.WorkPieceArray[(int)ELCNetUnitPos.SPINNER2].ID;
            Label_IDUHandler.Text        = CMainFrame.DataManager.WorkPieceArray[(int)ELCNetUnitPos.UPPER_HANDLER].ID;
            Label_IDLHandler.Text        = CMainFrame.DataManager.WorkPieceArray[(int)ELCNetUnitPos.LOWER_HANDLER].ID;
            Label_IDStage.Text           = CMainFrame.DataManager.WorkPieceArray[(int)ELCNetUnitPos.STAGE1].ID;
            Label_IDAutoManager.Text     = "O : " + CMainFrame.DataManager.GetID_LastOutput().ToString();
        }

        private void FormAutoScreen_Load(object sender, EventArgs e)
        {

        }

        private void btnWPInquiry_Click(object sender, EventArgs e)
        {
            // 같은 폼이 2개 이상 뜨지 않도록 처리하는 코드
            foreach(Form form in Application.OpenForms)
            {
                if(form.Name == "FormWorkPieceInquiry")
                {
                    if(form.WindowState == FormWindowState.Minimized)
                    {
                        form.WindowState = FormWindowState.Normal;
                    }
                    form.Activate();
                    return;
                }
            }
            var dlg = new FormWorkPieceInquiry();
            dlg.Show();
        }
    }
}
