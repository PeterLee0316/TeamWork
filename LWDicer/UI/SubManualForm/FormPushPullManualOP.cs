using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;

namespace LWDicer.UI
{
    public partial class FormPushPullManualOP : Form
    {
        public FormPushPullManualOP()
        {
            InitializeComponent();

            this.Text = "Push Pull Manual Operation";
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormPushPullManualOP_Load(object sender, EventArgs e)
        {
            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        private void FormPushPullManualOP_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            bool bStatus = false;

            CMainFrame.LWDicer.m_MePushPull.IsGripLocked(out bStatus);
            BtnGripLock.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            CMainFrame.LWDicer.m_MePushPull.IsGripReleased(out bStatus);
            BtnGripUnLock.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            CMainFrame.LWDicer.m_MePushPull.IsCylUp(out bStatus);
            BtnPushPullUp.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            CMainFrame.LWDicer.m_MePushPull.IsCylDown(out bStatus);
            BtnPushPullDown.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;
        }

        private void BtnGripLock_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MePushPull.GripLock();
        }

        private void BtnGripUnLock_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MePushPull.GripRelease();
        }

        private void BtnPushPullUp_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MePushPull.CylUp();
        }

        private void BtnPushPullDown_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MePushPull.CylDown();
        }
    }
}
