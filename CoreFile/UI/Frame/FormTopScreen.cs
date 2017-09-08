using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Core.Layers;
using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_OpPanel;

namespace Core.UI
{
    public partial class FormTopScreen : Form
    {
        public static FormTopScreen TopMenu = null;

        public FormTopScreen()
        {
            InitializeComponent();

            InitializeForm();

            TopMenu = this;
            textVersion.Text = SYSTEM_VER;
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.TOP_POS_X, DEF_UI.TOP_POS_Y);
            this.Size = new Size(DEF_UI.TOP_SIZE_WIDTH, DEF_UI.TOP_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;

            TimerUI.Interval = UITimerInterval;
            TimerUI.Enabled = true;
            TimerUI.Start();

            btnStart.UseVisualStyleBackColor = true;
            btnStop.UseVisualStyleBackColor = true;
            btnReset.UseVisualStyleBackColor = true;
            btnEMO.UseVisualStyleBackColor = true;

        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            TextTime.Text = DateTime.Now.ToString("yyyy-MM-dd [ddd] <tt> HH:mm:ss");

            //LabelCurUser.Text = $"Current User : {CMainFrame.DataManager.LoginInfo.User.Name}";
            BtnUserLogin.Text = $"Login : {CMainFrame.DataManager.LoginInfo.User.Name}";            
        }

        public void SetMessage(string strMsg)
        {
            TextMessage.Text = strMsg;
        }

        private void BtnUserLogin_Click(object sender, EventArgs e)
        {
            FormUserLogin dlg = new FormUserLogin();
            dlg.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (CMainFrame.InquireMsg("Exit System?"))
            {
                CMainFrame.Core.CloseSystem();

                Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            CMainFrame.Core.m_ctrlOpPanel.TempOnStartSWStatus();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            CMainFrame.Core.m_ctrlOpPanel.TempOnStopSWStatus();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            CMainFrame.Core.m_ctrlOpPanel.TempOnResetSWStatus();
        }

        private void btnEMO_Click(object sender, EventArgs e)
        {
            CMainFrame.Core.m_ctrlOpPanel.TempOnEMOSWStatus();
        }

        private void FormTopScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
