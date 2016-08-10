using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Control;

namespace LWDicer.UI
{
    public partial class FormTopScreen : Form
    {
        public static FormTopScreen TopMenu = null;

        public FormTopScreen()
        {
            InitializeComponent();

            InitializeForm();

            TopMenu = this;
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.TOP_POS_X, DEF_UI.TOP_POS_Y);
            this.Size = new Size(DEF_UI.TOP_SIZE_WIDTH, DEF_UI.TOP_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;

            tmFormTop.Interval = 1000;
            tmFormTop.Enabled = true;
            tmFormTop.Start();
        }

        private void tmFormTop_Tick(object sender, EventArgs e)
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
                Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}
