using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;

namespace Core.UI
{
    public partial class FormStageManualOP : Form
    {
        public FormStageManualOP()
        {
            InitializeComponent();

            this.Text = "Stage Manual Operation";
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormStageManualOP_Load(object sender, EventArgs e)
        {
            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();

            UpdateProcessData();
        }

        private void FormStageManualOP_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            bool bStatus = false;

            CMainFrame.mCore.m_MeStage.IsAbsorbed(out bStatus);
            BtnVacuumOn.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            CMainFrame.mCore.m_MeStage.IsReleased(out bStatus);
            BtnVacuumOff.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;
            
        }

        private void BtnVacuumOn_Click(object sender, EventArgs e)
        {
            CMainFrame.mCore.m_MeStage.Absorb(false);
        }

        private void BtnVacuumOff_Click(object sender, EventArgs e)
        {
            CMainFrame.mCore.m_MeStage.Release(false);
        }

        private void BtnClampOpen_Click(object sender, EventArgs e)
        {
        }

        private void BtnClampClose_Click(object sender, EventArgs e)
        {
        }


        private void TimerUI_Tick_1(object sender, EventArgs e)
        {
            
        }

        private void ChangeLabelText(GradientLabel objectLabel, string strMsg)        
        {
            if (objectLabel.Text == strMsg)
                return;
            else
            {
                objectLabel.Text = strMsg;
            }

        }

        private void UpdateProcessData()
        {

        }

        
    }
}
