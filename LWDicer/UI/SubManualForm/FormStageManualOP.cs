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

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;

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

            CMainFrame.LWDicer.m_MeStage.IsAbsorbed(out bStatus);
            BtnVacuumOn.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            CMainFrame.LWDicer.m_MeStage.IsReleased(out bStatus);
            BtnVacuumOff.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            CMainFrame.LWDicer.m_MeStage.IsClampOpen(out bStatus);
            BtnClampOpen.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            CMainFrame.LWDicer.m_MeStage.IsClampClose(out bStatus);
            BtnClampClose.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;
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

        private void btnLaserProcessStep1_Click(object sender, EventArgs e)
        {
            int iResult = 0;
            var task = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlStage1.LaserProcessStep1());
            //iResult=  await task;


           // CMainFrame.LWDicer.m_ctrlStage1.LaserProcessStep1();
        }

        private void btnLaserProcessStep2_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.LaserProcessStep2();
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

            CMainFrame.LWDicer.m_MeScanner.LaserProcessCount(Convert.ToInt32(strModify));
        }

        private void TimerUI_Tick_1(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_MeScanner.IsScannerBusy()) ChangeLabelText(lblProcessExpoBusy, "On");
            else ChangeLabelText(lblProcessExpoBusy, "Off");

            if (CMainFrame.LWDicer.m_MeScanner.IsScannerJobStart()) ChangeLabelText(lblProcessJobStart, "On");
            else ChangeLabelText(lblProcessJobStart, "Off");

            ChangeLabelText(lblProcessCountRead, Convert.ToString(CMainFrame.LWDicer.m_MeScanner.GetScannerRunCount())); 
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

        private void lblProcessOffsetX1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessOffsetX1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessOffsetX1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX1 = Convert.ToSingle(strModify);

        }

        private void lblProcessOffsetY1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessOffsetY1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessOffsetY1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY1 = Convert.ToSingle(strModify);
        }

        private void lblProcessCount1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessCount1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessCount1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessCount1 = Convert.ToInt32(strModify);
        }

        private void lblProcessOffsetX2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessOffsetX2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessOffsetX2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX2 = Convert.ToSingle(strModify);
        }

        private void lblProcessOffsetY2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessOffsetY2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessOffsetY2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY2 = Convert.ToSingle(strModify);
        }

        private void lblProcessCount2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessCount2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessCount2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessCount2 = Convert.ToInt32(strModify);
        }

        private void lblPatternPitch1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternPitch1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternPitch1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternPitch1 = Convert.ToSingle(strModify);
        }

        private void lblPatternCount1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternCount1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternCount1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternCount1 = Convert.ToInt32(strModify);
        }

        private void lblPatternPitch2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternPitch2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternPitch2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternPitch2 = Convert.ToSingle(strModify);
        }

        private void lblPatternCount2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternCount2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternCount2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternCount2 = Convert.ToInt32(strModify);
        }

        private void lblPatternOffset1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternOffset1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternOffset1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternOffset1 = Convert.ToSingle(strModify);
        }

        private void lblPatternOffset2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternOffset2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternOffset2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternOffset2 = Convert.ToSingle(strModify);
        }

        private void btnProcessDataSave_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage("프로세스 데이터를 저장하시겠습니까?", EMessageType.OK_CANCEL);

            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            UpdateProcessData();


            CMainFrame.LWDicer.SaveModelData(CMainFrame.DataManager.ModelData);
        }
        private void UpdateProcessData()
        {
            lblProcessOffsetX1.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX1);
            lblProcessOffsetY1.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY1);
            lblProcessCount1.Text   = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.ProcessCount1);
            lblProcessOffsetX2.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX2);
            lblProcessOffsetY2.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY2);
            lblProcessCount2.Text   = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.ProcessCount2);

            lblPatternPitch1.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.PatternPitch1);
            lblPatternCount1.Text = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.PatternCount1);
            lblPatternPitch2.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.PatternPitch2);
            lblPatternCount2.Text = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.PatternCount2);            
            lblPatternOffset1.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.PatternOffset1);
            lblPatternOffset2.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.PatternOffset2);
        }

        
    }
}
