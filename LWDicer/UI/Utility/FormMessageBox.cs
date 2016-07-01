using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LWDicer.Control;

using static LWDicer.Control.DEF_Common;

using static LWDicer.Control.MDataManager;

using static LWDicer.Control.DEF_DataManager;

using static LWDicer.Control.DEF_Error;

namespace LWDicer.UI
{
    public partial class FormMessageBox : Form
    {
        private CMessageInfo MsgInfo = new CMessageInfo();
        private bool IsUpdated = false;

        public FormMessageBox()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.Fixed3D;
        }

        public void SetMessage(int index)
        {
            MsgInfo.Index = index;
            int iResult = CMainFrame.LWDicer.m_DataManager.LoadMessageInfo(index, out MsgInfo);
            if(iResult != SUCCESS) BtnSave.Text = "Add New";
            else BtnSave.Text = "Update";
        }


        public void SetMessage(string strMsg, EMessageType type = EMessageType.OK)
        {
            int iResult = CMainFrame.LWDicer.m_DataManager.LoadMessageInfo(strMsg, out MsgInfo);
            if (iResult != SUCCESS)
            {
                MsgInfo.Message[(int)ELanguage.ENGLISH] = strMsg;
                MsgInfo.Message[(int)MLWDicer.Language] = strMsg;
                MsgInfo.Type = type;
                BtnSave.Text = "Add New";
            } else
            {
                BtnSave.Text = "Save";
            }
        }

        private void FormUtilMsg_Load(object sender, EventArgs e)
        {
            TextEng.Text = MsgInfo.GetMessage(ELanguage.ENGLISH);
            TextSystem.Text = MsgInfo.GetMessage(MLWDicer.Language);
            this.Text = $"Message : {MsgInfo.Index}";

            if(MLWDicer.Language == ELanguage.ENGLISH)
            {
                Label_System.Visible = false;
                TextSystem.Visible = false;
            }

            BtnConfirm.Visible = true;
            BtnCancel.Text = "Cancel";
            switch (MsgInfo.Type)
            {
                case EMessageType.OK:
                    BtnCancel.Visible = false; BtnConfirm.Text = "OK";
                    break;

                case EMessageType.OK_Cancel:
                    BtnCancel.Visible = true; BtnConfirm.Text = "OK";
                    break;

                case EMessageType.Confirm_Cancel:
                    BtnCancel.Visible = true; BtnConfirm.Text = "Confirm";
                    break;
            }

            timer1.Start();
        }
       
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if(IsUpdated == true)
            {
                if (MessageBox.Show("Message is updated, but not updated. is it ok?", "Confirm", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            MsgInfo.Message[(int)ELanguage.ENGLISH] = TextEng.Text;
            MsgInfo.Message[(int)MLWDicer.Language] = TextSystem.Text;
            CMainFrame.LWDicer.m_DataManager.UpdateMessageInfo(MsgInfo);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (IsUpdated == true)
            {
                if (MessageBox.Show("Message is updated, but not updated. is it ok?", "Confirm", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(TextEng.Text != MsgInfo.GetMessage() 
                || TextSystem.Text != MsgInfo.GetMessage(MLWDicer.Language))
            {
                BtnSave.Visible = true;
                IsUpdated = true;
            } else
            {
                BtnSave.Visible = false;
                IsUpdated = false;
            }
        }
    }
}
