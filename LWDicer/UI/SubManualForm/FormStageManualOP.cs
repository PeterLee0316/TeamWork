using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LWDicer.UI
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
            FormClose();
        }

        private void FormStageManualOP_Load(object sender, EventArgs e)
        {
            TmrManualOP.Enabled = true;
            TmrManualOP.Interval = 100;
            TmrManualOP.Start();
        }

        private void FormStageManualOP_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void FormClose()
        {
            TmrManualOP.Stop();
            this.Hide();
        }

        private void TmrManualOP_Tick(object sender, EventArgs e)
        {
            SensorStatus();
        }

        private void SensorStatus()
        {
            bool bStatus = false;

            CMainFrame.LWDicer.m_MeStage.IsAbsorbed(out bStatus);
            if (bStatus == true) BtnVacuumOn.BackColor = Color.LawnGreen; else BtnVacuumOff.BackColor = Color.LightGray;

            CMainFrame.LWDicer.m_MeStage.IsReleased(out bStatus);
            if (bStatus == true) BtnVacuumOff.BackColor = Color.LawnGreen; else BtnVacuumOn.BackColor = Color.LightGray;

            CMainFrame.LWDicer.m_MeStage.IsClampOpen(out bStatus);
            if (bStatus == true) BtnClampOpen.BackColor = Color.LawnGreen; else BtnClampClose.BackColor = Color.LightGray;

            CMainFrame.LWDicer.m_MeStage.IsClampClose(out bStatus);
            if (bStatus == true) BtnClampClose.BackColor = Color.LawnGreen; else BtnClampOpen.BackColor = Color.LightGray;
        }

        private void BtnVacuumOn_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeStage.Absorb(false);
        }

        private void BtnVacuumOff_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeStage.Release(false);
        }

        private void BtnClampOpen_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeStage.ClampOpen();
        }

        private void BtnClampClose_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeStage.ClampClose();
        }

        private void btnLaserProcessMof_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.LaserProcessMof();
        }

        private void btnLaserProcessStep_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.LaserProcessStill();
        }

        private void lblProcessCount_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessCount.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }
            
            lblProcessCount.Text = strModify;

            CMainFrame.LWDicer.m_MeStage.LaserProcessCount(Convert.ToInt32(strModify));
        }

        private void TmrManualOP_Tick_1(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_MeStage.IsScannerBusy()) ChangeLabelText(lblProcessExpoBusy, "On");
            else ChangeLabelText(lblProcessExpoBusy, "Off");

            if (CMainFrame.LWDicer.m_MeStage.IsScannerJobStart()) ChangeLabelText(lblProcessJobStart, "On");
            else ChangeLabelText(lblProcessJobStart, "Off");

            ChangeLabelText(lblProcessCountRead, Convert.ToString(CMainFrame.LWDicer.m_MeStage.GetScannerRunCount())); 
        }

        private void ChangeLabelText(Syncfusion.Windows.Forms.Tools.GradientLabel objectLabel, string strMsg)        
        {
            if (objectLabel.Text == strMsg)
                return;
            else
            {
                objectLabel.Text = strMsg;
            }

        }
    }
}
