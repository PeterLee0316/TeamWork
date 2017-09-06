using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using System.Threading.Tasks;
using Syncfusion.Drawing;
using System.Diagnostics;

using Core.Layers;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_LCNet;

namespace Core.UI
{
    public partial class FormManualScreen : Form
    {
        const int INPUT = 0;
        const int OUTPUT = 1;
        const int LoHandler = 0;
        const int UpHandler = 1;
        const int Spinner1 = 0;
        const int Spinner2 = 1;

        public FormManualScreen()
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

        private void BtnInput_Click(object sender, EventArgs e)
        {
            var dlg = new FormIO();
            dlg.ShowDialog();
        }

        private void BtnLimitSensor_Click(object sender, EventArgs e)
        {
            var dlg = new FormLimitSensor();
            dlg.ShowDialog();
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

        private void BtnManualPushPull_Click(object sender, EventArgs e)
        {
            var dlg = new FormPushPullManualOP();
            dlg.ShowDialog();
        }
        private void BtnManualUpHandler_Click(object sender, EventArgs e)
        {
        }
        private void BtnManualLoHandler_Click(object sender, EventArgs e)
        {
        }
        private void BtnManualSpinner1_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinnerManualOP();
            dlg.SetSpinner(ESpinnerIndex.SPINNER1);
            dlg.ShowDialog();
        }

        private void BtnManualSpinner2_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinnerManualOP();
            dlg.SetSpinner(ESpinnerIndex.SPINNER2);
            dlg.ShowDialog();
        }
        private void BtnManualStage_Click(object sender, EventArgs e)
        {
            var dlg = new FormStageManualOP();
            dlg.ShowDialog();
        }
        private void FormManualScreen_Activated(object sender, EventArgs e)
        {
            if (CMainFrame.DataManager.SystemData.UseSpinnerSeparately)
            {
                if (CMainFrame.DataManager.SystemData.UCoaterIndex == ESpinnerIndex.SPINNER1)
                {
                    Panel_Spinner1_Nozzle1.Text = "Coater Nozzle1";
                    Panel_Spinner1_Nozzle2.Text = "Coater Nozzle2";
                    Panel_Spinner1_Rotate.Text = "Coater Rotate";
                    BtnManualSpinner1.Text = "Coater";
                    Panel_Spinner2_Nozzle1.Text = "Cleaner Nozzle1";
                    Panel_Spinner2_Nozzle2.Text = "Cleaner Nozzle2";
                    Panel_Spinner2_Rotate.Text = "Cleaner Rotate";
                    BtnManualSpinner2.Text = "Cleaner";
                }
                else
                {
                    Panel_Spinner1_Nozzle1.Text = "Cleaner Nozzle1";
                    Panel_Spinner1_Nozzle2.Text = "Cleaner Nozzle2";
                    Panel_Spinner1_Rotate.Text = "Cleaner Rotate";
                    BtnManualSpinner1.Text = "Cleaner";
                    Panel_Spinner2_Nozzle1.Text = "Coater Nozzle1";
                    Panel_Spinner2_Nozzle2.Text = "Coater Nozzle2";
                    Panel_Spinner2_Rotate.Text = "Coater Rotate";
                    BtnManualSpinner2.Text = "Coater";
                }
            }
            else
            {
                Panel_Spinner1_Nozzle1.Text = "Spinner1 Nozzle1";
                Panel_Spinner1_Nozzle2.Text = "Spinner1 Nozzle2";
                Panel_Spinner1_Rotate.Text  = "Spinner1 Rotate";
                BtnManualSpinner1.Text = "Spinner1";
                Panel_Spinner2_Nozzle1.Text = "Spinner2 Nozzle1";
                Panel_Spinner2_Nozzle2.Text = "Spinner2 Nozzle2";
                Panel_Spinner2_Rotate.Text = "Spinner2 Rotate";
                BtnManualSpinner2.Text = "Spinner2";
            }
        }

        private async void Btn_Click(object sender, EventArgs e)
        {
            // check button
            Button btn = sender as Button;
            string unit = btn.Tag?.ToString();
            string cmd = btn.Text?.ToString();
            if (String.IsNullOrWhiteSpace(unit) || String.IsNullOrWhiteSpace(cmd)) return;

            // confirm
            if (CMainFrame.Core.IsSafeForAxisMove() == false) return;
            string strMsg = "Move to selected position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            // set btn enable
            btn.BackColor = CMainFrame.BtnBackColor_On;
            SetButtonsEnable(false);

            // do
            int iResult = SUCCESS;
            bool bStatus, bTransfer;
            Task<int> task1 = Task< int >.Run(() => CMainFrame.Core.EmptyMethod());
            CMainFrame.StartTimer();

           
            ///////////////////////////////////////////////////////////////////////////////
            // Stage1
            if (unit == "Stage1")
            {
                iResult = CMainFrame.Core.m_ctrlStage1.IsObjectDetected(out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "Wait")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlStage1.MoveToStageWaitPos());
                    iResult = await task1;
                }
                else if (cmd == "Load")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlStage1.MoveToStageLoadPos());
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // Scanner
            else if (unit == "Scanner")
            {
                if (cmd == "Wait")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlStage1.MoveToScannerWaitPos());
                    iResult = await task1;
                }
                else if (cmd == "Work")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlStage1.MoveToScannerWorkPos());
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // Camera
            else if (unit == "Camera")
            {
                if (cmd == "Wait")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlStage1.MoveToCameraWaitPos());
                    iResult = await task1;
                }
                else if (cmd == "Work")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlStage1.MoveToCameraWorkPos());
                    iResult = await task1;
                }
            }

            ERROR_OCCURED:

            LabelTime_Value.Text = CMainFrame.GetElapsedTIme_Text();

            // set btn enable
            btn.BackColor = CMainFrame.BtnBackColor_Off;
            SetButtonsEnable(true);

            // display alarm
            CMainFrame.DisplayAlarm(iResult);
        }

        private void btnStopAction_Click(object sender, EventArgs e)
        {
            MYaskawa.IsCancelJob_byManual = true;
        }

        private void SetButtonsEnable(bool bEnable)
        {
            CMainFrame.MainFrame.BottomScreen.EnableBottomPage(bEnable);

            var btns = CMainFrame.MainFrame.GetAllControl(this, typeof(Syncfusion.Windows.Forms.ButtonAdv));
            foreach(var btn in btns)
            {
                Syncfusion.Windows.Forms.ButtonAdv abtn = btn as Syncfusion.Windows.Forms.ButtonAdv;
                abtn.Enabled = bEnable;
            }
            btns = CMainFrame.MainFrame.GetAllControl(this, typeof(System.Windows.Forms.Button));
            foreach (var btn in btns)
            {
                System.Windows.Forms.Button abtn = btn as System.Windows.Forms.Button;
                abtn.Enabled = bEnable;
            }

            btnStopAction.Enabled = true;
        }
    }
}