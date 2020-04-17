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

using Syncfusion.Windows.Forms;

namespace Core.UI
{
    public partial class FormSpinnerManualOP : Form
    {
        private ESpinnerIndex m_SpinnerIndex = 0;
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

        }

        private void UpdateStatus()
        {
            bool bStatus = false;
            
        }

        private void BtnVacuumOn_Click(object sender, EventArgs e)
        {
        }

        private void BtnVacuumOff_Click(object sender, EventArgs e)
        {
        }

        private void BtnSpinnerUp_Click(object sender, EventArgs e)
        {
        }

        private void BtnSpinnerDown_Click(object sender, EventArgs e)
        {
        }

        private void BtnCleanNozzleOn_Click(object sender, EventArgs e)
        {
        }

        private void BtnCleanNozzleOff_Click(object sender, EventArgs e)
        {
        }

        private void BtnCoatNozzleOn_Click(object sender, EventArgs e)
        {
        }

        private void BtnCoatNozzleOff_Click(object sender, EventArgs e)
        {
        }
        private void BtnCoatingJobStop_Click(object sender, EventArgs e)
        {
           
        }

        private void BtnCleaningJobStop_Click(object sender, EventArgs e)
        {
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            UpdateStatus();

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
