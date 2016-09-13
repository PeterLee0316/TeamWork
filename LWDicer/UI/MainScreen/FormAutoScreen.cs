using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using LWDicer.Layers;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormAutoScreen : Form
    {
        int m_nStartReady = 0;		// 0:Off, 1:Ready, 2:Run

        public FormAutoScreen()
        {
            InitializeComponent();
            InitializeForm();
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

            // Message Pop up
            // RUN SW 또는 Stop SW 눌릴 때 까지 대기

            BtnStart.Text = "Stop AutoRun";
            //BtnStart.Enabled = false;
            //		m_BtnOpMode1.Enabled = false;
            //BtnModeSelect.Enabled = false;
            BtnOriginReturn.Enabled = false;
            BtnUnitInit.Enabled = false;
            //		m_BtnCellSupplyStop.Enabled = false; // For Message Transfer
            //		m_BtnPcbSupplyStop.Enabled = false;
            //BtnMonitorChange.Enabled = false;
            //		m_BtnCameraChange.Enabled = false;

            //		testTemp->EnableWindow(false);

            CMainFrame.LWDicer.m_trsAutoManager.SendMsg(EThreadMessage.MSG_START_RUN_CMD);
        }

        private void StopAutoRun()
        {
            if (CMainFrame.InquireMsg("Stop AutoRun?", "Question") == false)
                return;

            // Message Pop up
            // RUN SW 또는 Stop SW 눌릴 때 까지 대기

            BtnStart.Text = "Start AutoRun";
            //BtnStart.Enabled = true;
            //		m_BtnOpMode1.Enabled = true;
            //BtnModeSelect.Enabled = true;
            BtnOriginReturn.Enabled = true;
            BtnUnitInit.Enabled = true;
            //		m_BtnCellSupplyStop.Enabled = true; // For Message Transfer
            //		m_BtnPcbSupplyStop.Enabled = true;
            //BtnMonitorChange.Enabled = true;
            //		m_BtnCameraChange.Enabled = true;

            //		testTemp->EnableWindow(true);

            CMainFrame.LWDicer.m_trsAutoManager.SendMsg(EThreadMessage.MSG_STEP_STOP_CMD);
        }
    }
}
