using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;

namespace Core.UI
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
        }

        private void BtnGripLock_Click(object sender, EventArgs e)
        {

        }

        private void BtnGripUnLock_Click(object sender, EventArgs e)
        {
        }

        private void BtnPushPullUp_Click(object sender, EventArgs e)
        {
        }

        private void BtnPushPullDown_Click(object sender, EventArgs e)
        {
        }
    }
}
