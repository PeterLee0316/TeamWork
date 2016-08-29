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
            FormClose();
        }

        private void FormPushPullManualOP_Load(object sender, EventArgs e)
        {
            TmrManualOP.Enabled = true;
            TmrManualOP.Interval = UITimerInterval;
            TmrManualOP.Start();
        }

        private void FormPushPullManualOP_FormClosing(object sender, FormClosingEventArgs e)
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

            CMainFrame.LWDicer.m_MePushPull.IsGripLocked(out bStatus);
            if(bStatus == true) BtnGripLock.BackColor = Color.LawnGreen; else BtnGripUnLock.BackColor = Color.LightGray;

            CMainFrame.LWDicer.m_MePushPull.IsGripReleased(out bStatus);
            if (bStatus == true) BtnGripUnLock.BackColor = Color.LawnGreen; else BtnGripLock.BackColor = Color.LightGray;

            CMainFrame.LWDicer.m_MePushPull.IsCylUp(out bStatus);
            if (bStatus == true) BtnPushPullUp.BackColor = Color.LawnGreen; else BtnPushPullDown.BackColor = Color.LightGray;

            CMainFrame.LWDicer.m_MePushPull.IsCylDown(out bStatus);
            if (bStatus == true) BtnPushPullDown.BackColor = Color.LawnGreen; else BtnPushPullUp.BackColor = Color.LightGray;
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
