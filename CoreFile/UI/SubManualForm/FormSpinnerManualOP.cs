using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Layers;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_CtrlSpinner;

using Syncfusion.Windows.Forms;

namespace Core.UI
{
    public partial class FormSpinnerManualOP : Form
    {
        private ESpinnerIndex m_SpinnerIndex = 0;
        private MMeSpinner m_MeSpinner;
        private MCtrlSpinner m_CtrlSpinner;
        private MTickTimer m_ActionTimer = new MTickTimer();

        public FormSpinnerManualOP()
        {
            InitializeComponent();
        }

        public void SetSpinner(ESpinnerIndex index)
        {
            m_SpinnerIndex = index;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSpinnerManualOP_Load(object sender, EventArgs e)
        {
            if(m_SpinnerIndex == ESpinnerIndex.SPINNER1)
            {
                this.Text = "Spinner 1 Manual Operation";
                m_MeSpinner = CMainFrame.Core.m_MeSpinner1;
                m_CtrlSpinner = CMainFrame.Core.m_ctrlSpinner1;
            } else
            {
                this.Text = "Spinner 2 Manual Operation";
                m_MeSpinner = CMainFrame.Core.m_MeSpinner2;
                m_CtrlSpinner = CMainFrame.Core.m_ctrlSpinner2;
            }
/*
            if (CMainFrame.DataManager.SystemData.UseSpinnerSeparately)
            {
                if (CMainFrame.DataManager.SystemData.UCoaterIndex == ESpinnerIndex.SPINNER1)
                {
                    if(m_SpinnerIndex == ESpinnerIndex.SPINNER1)
                    {
                        this.Text = "Coater Manual Operation";
                        LabelCleaningTitle.Visible = false;
                        BtnCleaningJobStart.Visible = false;
                        BtnCleaningJobStop.Visible = false;
                        LabelCleaningTime_Title.Visible = false;
                        LabelTime_Cleaning.Visible = false;
                        LabelStep_Cleaning.Visible = false;
                    } else
                    {
                        this.Text = "Cleaner Manual Operation";
                        LabelCoatingTitle.Visible = false;
                        BtnCoatingJobStart.Visible = false;
                        BtnCoatingJobStop.Visible = false;
                        LabelCoatingTime_Title.Visible = false;
                        LabelTime_Coating.Visible = false;
                        LabelStep_Coating.Visible = false;
                    }
                }
                else
                {
                    if (m_SpinnerIndex == ESpinnerIndex.SPINNER1)
                    {
                        this.Text = "Cleaner Manual Operation";
                        LabelCoatingTitle.Visible = false;
                        BtnCoatingJobStart.Visible = false;
                        BtnCoatingJobStop.Visible = false;
                        LabelCoatingTime_Title.Visible = false;
                        LabelTime_Coating.Visible = false;
                        LabelStep_Coating.Visible = false;
                    }
                    else
                    {
                        this.Text = "Coater Manual Operation";
                        LabelCleaningTitle.Visible = false;
                        BtnCleaningJobStart.Visible = false;
                        BtnCleaningJobStop.Visible = false;
                        LabelCleaningTime_Title.Visible = false;
                        LabelTime_Cleaning.Visible = false;
                        LabelStep_Cleaning.Visible = false;
                    }
                }
            }
            else
            {
            }
*/
            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        private void FormSpinnerManualOP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_CtrlSpinner.IsDoingJob_Coating || m_CtrlSpinner.IsDoingJob_Cleaning)
            {
                CMainFrame.DisplayMsg("The requested operation is in progress.");
                e.Cancel = true;
            }
        }

        private void UpdateStatus()
        {
            bool bStatus = false;

            m_MeSpinner.IsAbsorbed(out bStatus);
            BtnVacuumOn.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            m_MeSpinner.IsReleased(out bStatus);
            BtnVacuumOff.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            m_MeSpinner.IsChuckTableUp(out bStatus);
            BtnSpinnerUp.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            m_MeSpinner.IsChuckTableDown(out bStatus);
            BtnSpinnerDown.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            m_MeSpinner.IsCleanNozzleValveOpen(out bStatus);
            BtnCleanNozzleOn.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            m_MeSpinner.IsCleanNozzleValveClose(out bStatus);
            BtnCleanNozzleOff.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            m_MeSpinner.IsCoatNozzleValveOpen(out bStatus);
            BtnCoatNozzleOn.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            m_MeSpinner.IsCoatNozzleValveClose(out bStatus);
            BtnCoatNozzleOff.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;
        }

        private void BtnVacuumOn_Click(object sender, EventArgs e)
        {
            CMainFrame.StartTimer();
            int iResult = m_MeSpinner.Absorb();
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_ChuckVac.Text = CMainFrame.GetElapsedTIme_Text();
        }

        private void BtnVacuumOff_Click(object sender, EventArgs e)
        {
            CMainFrame.StartTimer();
            int iResult = m_MeSpinner.Release();
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_ChuckVac.Text = CMainFrame.GetElapsedTIme_Text();
        }

        private void BtnSpinnerUp_Click(object sender, EventArgs e)
        {
            if (CMainFrame.Core.IsSafeForCylinderMove() == false) return;
            CMainFrame.StartTimer();
            int iResult = m_CtrlSpinner.TableUp();
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_UpDn.Text = CMainFrame.GetElapsedTIme_Text();
        }

        private void BtnSpinnerDown_Click(object sender, EventArgs e)
        {
            if (CMainFrame.Core.IsSafeForCylinderMove() == false) return;
            CMainFrame.StartTimer();
            int iResult = m_CtrlSpinner.TableDown();
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_UpDn.Text = CMainFrame.GetElapsedTIme_Text();
        }

        private void BtnCleanNozzleOn_Click(object sender, EventArgs e)
        {
            CMainFrame.StartTimer();
            int iResult = m_MeSpinner.CleanNozzleValveOpen();
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_CleanValve.Text = CMainFrame.GetElapsedTIme_Text();
        }

        private void BtnCleanNozzleOff_Click(object sender, EventArgs e)
        {
            CMainFrame.StartTimer();
            int iResult = m_MeSpinner.CleanNozzleValveClose();
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_CleanValve.Text = CMainFrame.GetElapsedTIme_Text();
        }

        private void BtnCoatNozzleOn_Click(object sender, EventArgs e)
        {
            CMainFrame.StartTimer();
            int iResult = m_MeSpinner.CoatNozzleValveOpen();
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_CoatValve.Text = CMainFrame.GetElapsedTIme_Text();
        }

        private void BtnCoatNozzleOff_Click(object sender, EventArgs e)
        {
            CMainFrame.StartTimer();
            int iResult = m_MeSpinner.CoatNozzleValveClose();
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_CoatValve.Text = CMainFrame.GetElapsedTIme_Text();
        }

        private async void BtnCleaningJobStart_Click(object sender, EventArgs e)
        {
            if (m_CtrlSpinner.IsDoingJob_Coating || m_CtrlSpinner.IsDoingJob_Cleaning)
            {
                CMainFrame.DisplayMsg("The requested job is in progress.");
                return;
            }
            if (CMainFrame.Core.IsSafeForAxisMove() == false) return;

            BtnCleaningJobStart.BackColor = CMainFrame.BtnBackColor_On;
            SetButtonsEnable(false);

            CMainFrame.StartTimer();
            var task1 = Task<int>.Run(() => m_CtrlSpinner.DoCleanOperation());
            int iResult = await task1;

            LabelTime_Cleaning.Text = CMainFrame.GetElapsedTIme_Text();
            SetButtonsEnable(true);
            BtnCleaningJobStart.BackColor = CMainFrame.BtnBackColor_Off;

            if (m_CtrlSpinner.IsCancelJob_byManual)
            {
                CMainFrame.DisplayMsg("The requested job was canceled by user");
            }
            else
            {
                CMainFrame.DisplayAlarm(iResult);
            }
        }

        private async void BtnCoatingJobStart_Click(object sender, EventArgs e)
        {
            if (m_CtrlSpinner.IsDoingJob_Coating || m_CtrlSpinner.IsDoingJob_Cleaning)
            {
                CMainFrame.DisplayMsg("The requested job is in progress.");
                return;
            }
            if (CMainFrame.Core.IsSafeForAxisMove() == false) return;

            BtnCoatingJobStart.BackColor = CMainFrame.BtnBackColor_On;
            SetButtonsEnable(false);

            CMainFrame.StartTimer();
            var task1 = Task<int>.Run(() => m_CtrlSpinner.DoCoatOperation());
            int iResult = await task1;

            LabelTime_Coating.Text = CMainFrame.GetElapsedTIme_Text();
            SetButtonsEnable(true);
            BtnCoatingJobStart.BackColor = CMainFrame.BtnBackColor_Off;

            if (m_CtrlSpinner.IsCancelJob_byManual)
            {
                CMainFrame.DisplayMsg("The requested job was canceled by user");
            }
            else
            {
                CMainFrame.DisplayAlarm(iResult);
            }
        }

        private void BtnCoatingJobStop_Click(object sender, EventArgs e)
        {
            m_CtrlSpinner.IsCancelJob_byManual = true;
        }

        private void BtnCleaningJobStop_Click(object sender, EventArgs e)
        {
            m_CtrlSpinner.IsCancelJob_byManual = true;
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            UpdateStatus();

            int nline = 38;
            if(m_CtrlSpinner.IsDoingJob_Coating)
            {
                LabelTime_Coating.Text = $"{m_CtrlSpinner.GetJobElapsedTime()}";
                string str = m_CtrlSpinner.CurStep_Coating.ToString();
                if (str.Length > nline)
                {
                    LabelStep_Coating.Text = str.Substring(0, nline) + Environment.NewLine + str.Substring(nline);

                }
            }

            if (m_CtrlSpinner.IsDoingJob_Cleaning)
            {
                LabelTime_Cleaning.Text = $"{m_CtrlSpinner.GetJobElapsedTime()}";
                string str = m_CtrlSpinner.CurStep_Cleaning.ToString();
                if (str.Length > nline)
                {
                    LabelStep_Cleaning.Text = str.Substring(0, nline) + Environment.NewLine + str.Substring(nline);

                }
            }

            //if (m_CtrlSpinner.IsDoingJob_Coating || m_CtrlSpinner.IsDoingJob_Cleaning)
            //{
            //    BtnSpinnerUp.Enabled = false;
            //    BtnSpinnerDown.Enabled = false;
            //    BtnVacuumOn.Enabled = false;
            //    BtnVacuumOff.Enabled = false;
            //    BtnCoatNozzleOn.Enabled = false;
            //    BtnCoatNozzleOff.Enabled = false;
            //    BtnCleanNozzleOn.Enabled = false;
            //    BtnCleanNozzleOff.Enabled = false;
            //} else
            //{
            //    BtnSpinnerUp.Enabled = true;
            //    BtnSpinnerDown.Enabled = true;
            //    BtnVacuumOn.Enabled = true;
            //    BtnVacuumOff.Enabled = true;
            //    BtnCoatNozzleOn.Enabled = true;
            //    BtnCoatNozzleOff.Enabled = true;
            //    BtnCleanNozzleOn.Enabled = true;
            //    BtnCleanNozzleOff.Enabled = true;
            //}
        }

        private void SetButtonsEnable(bool bEnable)
        {
            CMainFrame.MainFrame.BottomScreen.EnableBottomPage(bEnable);

            var btns = CMainFrame.MainFrame.GetAllControl(this, typeof(Syncfusion.Windows.Forms.ButtonAdv));
            foreach (var btn in btns)
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

            BtnCoatingJobStop.Enabled = true;
            BtnCleaningJobStop.Enabled = true;
        }
    }
}
