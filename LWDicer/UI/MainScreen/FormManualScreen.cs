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

using LWDicer.Layers;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_LCNet;

namespace LWDicer.UI
{
    public partial class FormManualScreen : Form
    {
        const int INPUT = 0;
        const int OUTPUT = 1;
        const int LoHandler = 0;
        const int UpHandler = 1;
        const int Spinner1 = 0;
        const int Spinner2 = 1;

        private bool IsDoingJob = false;

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
            var dlg = new FormHandlerManualOP();
            dlg.SetHandler(DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER);
            dlg.ShowDialog();
        }
        private void BtnManualLoHandler_Click(object sender, EventArgs e)
        {
            var dlg = new FormHandlerManualOP();
            dlg.SetHandler(DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER);
            dlg.ShowDialog();
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
            if (CMainFrame.LWDicer.IsSafeForAxisMove() == false) return;
            string strMsg = "Move to selected position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            // set btn enable
            btn.BackColor = CMainFrame.BtnBackColor_On;
            SetButtonsEnable(false);

            // do
            int iResult = SUCCESS;
            bool bStatus, bTransfer;
            Task<int> task1 = Task< int >.Run(() => CMainFrame.LWDicer.EmptyMethod());
            CMainFrame.StartTimer();

            ///////////////////////////////////////////////////////////////////////////////
            // Loader
            if (unit == "Loader")
            {
                if(cmd == "Bottom")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlLoader.MoveToBottomPos());
                    iResult = await task1;
                }
                else if (cmd == "Top")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlLoader.MoveToTopPos());
                    iResult = await task1;
                }
                else if (cmd == "Slot")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlLoader.MoveToSlotPos());
                    iResult = await task1;
                }
                else if (cmd == "Load")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlLoader.MoveToSlotPos());
                    iResult = await task1;
                }
                else if (cmd == "Slot")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlLoader.MoveToSlotPos());
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // PushPull
            else if (unit == "PushPull")
            {
                iResult = CMainFrame.LWDicer.m_ctrlPushPull.IsObjectDetected(out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if(bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "Wait")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlPushPull.MoveToWaitPos(bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Load")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlPushPull.MoveToLoaderPos(bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Spinner 1")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlPushPull.MoveToSpinner1Pos(bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Spinner 2")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlPushPull.MoveToSpinner2Pos(bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Handler")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlPushPull.MoveToHandlerPos(bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Temp Unload")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlPushPull.MoveToTempUnloadPos(bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Reload")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlPushPull.MoveToReloadPos(bTransfer));
                    iResult = await task1;
                }
            }
            else if (unit == "Centering")
            {
                if (cmd == "Open")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlPushPull.MoveAllCenterUnitToWaitPos());
                    iResult = await task1;
                }
                else if (cmd == "Close")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlPushPull.MoveAllCenterUnitToCenteringPos());
                    iResult = await task1;
                }

            }
            ///////////////////////////////////////////////////////////////////////////////
            // Spinner1
            else if (unit == "Spinner1_CoatNozzle")
            {
                if (cmd == "Safety")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner1.MoveCoatNozzleToSafetyPos());
                    iResult = await task1;
                }
                else if (cmd == "Dispensing Start")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner1.MoveCoatNozzleToStartPos());
                    iResult = await task1;
                }
                else if (cmd == "Dispensing End")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner1.MoveCoatNozzleToEndPos());
                    iResult = await task1;
                }
            }
            else if (unit == "Spinner1_CleanNozzle")
            {
                if (cmd == "Safety")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner1.MoveCleanNozzleToSafetyPos());
                    iResult = await task1;
                }
                else if (cmd == "Dispensing Start")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner1.MoveCleanNozzleToStartPos());
                    iResult = await task1;
                }
                else if (cmd == "Dispensing End")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner1.MoveCleanNozzleToEndPos());
                    iResult = await task1;
                }
            }
            else if (unit == "Spinner1_Rotate")
            {
                if (cmd == "Home")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner1.MoveRotateToLoadPos());
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // Spinner2
            else if (unit == "Spinner2_CoatNozzle")
            {
                if (cmd == "Safety")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner2.MoveCoatNozzleToSafetyPos());
                    iResult = await task1;
                }
                else if (cmd == "Dispensing Start")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner2.MoveCoatNozzleToStartPos());
                    iResult = await task1;
                }
                else if (cmd == "Dispensing End")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner2.MoveCoatNozzleToEndPos());
                    iResult = await task1;
                }
            }
            else if (unit == "Spinner2_CleanNozzle")
            {
                if (cmd == "Safety")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner2.MoveCleanNozzleToSafetyPos());
                    iResult = await task1;
                }
                else if (cmd == "Dispensing Start")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner2.MoveCleanNozzleToStartPos());
                    iResult = await task1;
                }
                else if (cmd == "Dispensing End")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner2.MoveCleanNozzleToEndPos());
                    iResult = await task1;
                }
            }
            else if (unit == "Spinner2_Rotate")
            {
                if (cmd == "Home")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlSpinner2.MoveRotateToLoadPos());
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // UpperHandler
            else if (unit == "UpperHandler_X")
            {
                DEF_CtrlHandler.EHandlerIndex index = DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER;
                iResult = CMainFrame.LWDicer.m_ctrlHandler.IsObjectDetected(index, out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "Wait")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveToWaitPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Load [PushPull]")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveToPushPullPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Unload [Stage]")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveToStagePos(index, bTransfer));
                    iResult = await task1;
                }
            }
            else if (unit == "UpperHandler_Z")
            {
                DEF_CtrlHandler.EHandlerIndex index = DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER;
                iResult = CMainFrame.LWDicer.m_ctrlHandler.IsObjectDetected(index, out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "Safety")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveZToSafetyPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Load / Unload")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveZToLoadUnloadPos(index, bTransfer));
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // LowerHandler
            else if (unit == "LowerHandler_X")
            {
                DEF_CtrlHandler.EHandlerIndex index = DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER;
                iResult = CMainFrame.LWDicer.m_ctrlHandler.IsObjectDetected(index, out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "Wait")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveToWaitPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Load [PushPull]")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveToPushPullPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Unload [Stage]")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveToStagePos(index, bTransfer));
                    iResult = await task1;
                }
            }
            else if (unit == "LowerHandler_Z")
            {
                DEF_CtrlHandler.EHandlerIndex index = DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER;
                iResult = CMainFrame.LWDicer.m_ctrlHandler.IsObjectDetected(index, out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "Safety")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveZToSafetyPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "Load / Unload")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlHandler.MoveZToLoadUnloadPos(index, bTransfer));
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // Stage1
            else if (unit == "Stage1")
            {
                iResult = CMainFrame.LWDicer.m_ctrlStage1.IsObjectDetected(out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "Wait")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlStage1.MoveToStageWaitPos());
                    iResult = await task1;
                }
                else if (cmd == "Load")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlStage1.MoveToStageLoadPos());
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // Scanner
            else if (unit == "Scanner")
            {
                if (cmd == "Wait")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlStage1.MoveToScannerWaitPos());
                    iResult = await task1;
                }
                else if (cmd == "Work")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlStage1.MoveToScannerWorkPos());
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // Camera
            else if (unit == "Camera")
            {
                if (cmd == "Wait")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlStage1.MoveToCameraWaitPos());
                    iResult = await task1;
                }
                else if (cmd == "Work")
                {
                    task1 = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlStage1.MoveToCameraWorkPos());
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
            IsDoingJob = !bEnable;
            CMainFrame.MainFrame.BottomScreen.EnableBottomPage(bEnable);

            var btns = GetAll(this, typeof(Syncfusion.Windows.Forms.ButtonAdv));
            foreach(var btn in btns)
            {
                Syncfusion.Windows.Forms.ButtonAdv abtn = btn as Syncfusion.Windows.Forms.ButtonAdv;
                abtn.Enabled = bEnable;
            }
            btnStopAction.Enabled = true;
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private void buttonAdv6_Click(object sender, EventArgs e)
        {
        }
    }
}