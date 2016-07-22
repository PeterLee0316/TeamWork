using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormUserLogin : Form
    {
        private List<CListHeader> HeaderList;

        private CUserInfo userinfo = new CUserInfo();

        private string[] StrUser;

        private int selectindex;


        public FormUserLogin()
        {
            int nCount=0, i, curIndex=0;

            string strName;

            InitializeComponent();

            HeaderList = CMainFrame.DataManager.UserInfoHeaderList;

            StrUser = new string[HeaderList.Count];

            for(i=0;i<HeaderList.Count;i++)
            {
                StrUser[i] = "";
            }

            foreach (CListHeader info in CMainFrame.DataManager.UserInfoHeaderList)
            {
                if (info.IsFolder == false)
                {
                    if (CMainFrame.DataManager.GetLogin().User.Name == info.Name) curIndex = nCount;

                    strName = $"{info.Parent} : {info.Name}";

                    ComboUser.Items.Add(strName);

                    StrUser[nCount] = info.Name;

                    nCount++;
                }
            }

            ComboUser.SelectedIndex = curIndex;

            LabelComment.Text = CMainFrame.DataManager.GetLogin().User.Comment;
            LabelType.Text = Convert.ToString(CMainFrame.DataManager.GetLogin().User.Type);

            this.Text = $"User Login [ Current Model : {CMainFrame.DataManager.ModelData.Name} ]";

        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("Do you like to change user mode?"))
            {
                return;
            }

            // Password Check
            string strModify;

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify, $"Input {CMainFrame.DataManager.GetLogin().User.Name} Password"))
            {
                return;
            }

            if (strModify != CMainFrame.DataManager.GetLogin().User.Password)
            {
                CMainFrame.DisplayMsg("You have inputted the wrong password.");
                return;
            }

            CMainFrame.LWDicer.m_DataManager.SystemData.UserName = StrUser[selectindex];

            CMainFrame.DataManager.SetLogin();

            CMainFrame.LWDicer.m_DataManager.SaveSystemData(CMainFrame.LWDicer.m_DataManager.SystemData);

            FormClose();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void ComboUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox User = (ComboBox)sender;

            selectindex = User.SelectedIndex;

            foreach (CListHeader info in CMainFrame.DataManager.UserInfoHeaderList)
            {
                if (info.IsFolder == false)
                {
                    if(StrUser[selectindex] == info.Name)
                    {
                        LabelComment.Text = info.Comment;
                        LabelType.Text = info.Parent;
                        break;
                    }
                }
            }
        }

        private void FormUserLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void BtnChangePW_Click(object sender, EventArgs e)
        {
            
            FormChangePassword dlg = new FormChangePassword();
            dlg.ShowDialog();
        }
    }
}
