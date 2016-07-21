using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Control.DEF_DataManager;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.UI
{
    public partial class FormChangePassword : Form
    {
        public FormChangePassword()
        {
            InitializeComponent();
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnChange_Click(object sender, EventArgs e)
        {
            if (NewPW1.Text != NewPW2.Text)
            {
                NewPW1.Text = "";
                NewPW2.Text = "";
                CMainFrame.DisplayMsg("The new password was not matched.");
                return;
            }

            if (NewPW1.Text == "" || NewPW2.Text == "")
            {
                NewPW1.Text = "";
                NewPW2.Text = "";
                CMainFrame.DisplayMsg("Please input new password.");
                return;
            }

            if (!CMainFrame.DisplayMsg("Do you like to change the new password?"))
            {
                return;
            }

            CMainFrame.DataManager.GetLogin().User.Password = NewPW2.Text;

            CUserInfo userInfoData = new CUserInfo(CMainFrame.DataManager.GetLogin().User.Name, CMainFrame.DataManager.GetLogin().User.Comment, CMainFrame.DataManager.GetLogin().User.Password, CMainFrame.DataManager.GetLogin().User.Type);
            CMainFrame.DataManager.SaveModelData(userInfoData);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormInputPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void CurrentPW_Click(object sender, EventArgs e)
        {
            string strModify;

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify, "Input Current Password"))
            {
                CurrentPW.Text = ""; NewPW1.Text = ""; NewPW2.Text = "";
                return;
            }

            if (CMainFrame.DataManager.GetLogin().User.Password != strModify)
            {
                CurrentPW.Text = ""; NewPW1.Text = ""; NewPW2.Text = "";
                CMainFrame.DisplayMsg("You have inputted the wrong password.");
                return;
            }

            CurrentPW.Text = strModify;
        }

        private void NewPW1_Click(object sender, EventArgs e)
        {
            string strModify;

            if(CurrentPW.Text == "" || CMainFrame.DataManager.GetLogin().User.Password != CurrentPW.Text)
            {
                NewPW1.Text = ""; NewPW2.Text = "";
                return;
            }

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify, "Input New Password"))
            {
                NewPW1.Text = ""; NewPW2.Text = "";
                return;
            }

            NewPW1.Text = strModify;
            NewPW2.Text = "";
        }

        private void NewPW2_Click(object sender, EventArgs e)
        {
            string strModify;

            if (CurrentPW.Text == "" || NewPW1.Text == "")
            {
                NewPW2.Text = "";
                return;
            }

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify, "Confirm New Password"))
            {
                NewPW2.Text = "";
                return;
            }

            if(NewPW1.Text != strModify)
            {
                NewPW2.Text = "";
                CMainFrame.DisplayMsg("The new password was not matched.");
                return;
            }

            NewPW2.Text = strModify;
        }
    }
}
