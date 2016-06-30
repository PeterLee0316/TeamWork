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
        private int nMsgCode;
        private string strMsg_Eng, StrMsg_Sys;
        private EMessageType msgtype;

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

        public void SetMessage(string strText, int nCode = 0)
        {
            bool bExist = false;

            if(strText != "")
            {
                List<CMessageInfo> MessageInfo = CMainFrame.LWDicer.m_DataManager.MessageInfoList;

                // 사용자가 입력한 Text가 Message List에 있는지 검색
                foreach (CMessageInfo info in MessageInfo)
                {
                    // Messgae List에 있는 경우
                    if (info.Message[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language] == strText)
                    {
                        nMsgCode = info.Index;
                        strMsg_Eng = info.Message[(int)ELanguage.ENGLISH];
                        StrMsg_Sys = info.Message[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language];

                        this.msgtype = CMainFrame.LWDicer.m_DataManager.MessageInfoList[nMsgCode].Type;
                        bExist = true;
                        break;
                    }
                    else
                    {
                        bExist = false;
                    }
                }

                // Messgae List에 없는 경우
                if (bExist == false)
                {
                    nMsgCode = CMainFrame.LWDicer.m_DataManager.MessageInfoList.Count;
                    StrMsg_Sys = strText;
                    strMsg_Eng = strText;
                    this.msgtype = EMessageType.OK_Cancel;

                    BtnSave.Text = "Add New";
                }
            }
            else
            {
                nMsgCode = nCode;
                StrMsg_Sys = CMainFrame.LWDicer.m_DataManager.MessageInfoList[nMsgCode].Message[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language];
                strMsg_Eng = CMainFrame.LWDicer.m_DataManager.MessageInfoList[nMsgCode].Message[(int)ELanguage.ENGLISH];
                this.msgtype = CMainFrame.LWDicer.m_DataManager.MessageInfoList[nCode].Type;

                BtnSave.Text = "Save";
            }
        }


        private void FormUtilMsg_Load(object sender, EventArgs e)
        {
            TextEng.Text = strMsg_Eng;
            TextSystem.Text = StrMsg_Sys;

            if (msgtype == EMessageType.OK) { BtnConfirm.Visible = true; BtnCancel.Visible = false; BtnConfirm.Text = "OK"; }
            if (msgtype == EMessageType.OK_Cancel) { BtnConfirm.Visible = true; BtnCancel.Visible = true; BtnConfirm.Text = "OK"; BtnCancel.Text = "Cancel"; }
            if (msgtype == EMessageType.Confirm_Cancel) { BtnConfirm.Visible = true; BtnCancel.Visible = true; BtnConfirm.Text = "Confirm"; BtnCancel.Text = "Cancel"; }

            this.Text = string.Format("Message : {0:d}", nMsgCode);
        }

       
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            bool bExist = false;

            List<CMessageInfo> MessageInfo = CMainFrame.LWDicer.m_DataManager.MessageInfoList;

            foreach (CMessageInfo info in MessageInfo)
            {
                if(info.Index == nMsgCode)
                {
                    info.Message[(int)ELanguage.ENGLISH] = TextEng.Text;
                    info.Message[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language] = TextSystem.Text;

                    CMainFrame.LWDicer.m_DataManager.MessageInfoList.RemoveAt(info.Index);
                    CMainFrame.LWDicer.m_DataManager.MessageInfoList.Insert(info.Index, info);

                    bExist = true;

                    break;
                }
                else
                {
                    bExist = false;
                }
            }

            // 신규등록
            if(bExist==false)
            {
                CMessageInfo MsgInfo = new CMessageInfo();

                MsgInfo.Index = nMsgCode;
                MsgInfo.Type = EMessageType.OK_Cancel;
                MsgInfo.Message[(int)ELanguage.ENGLISH] = TextEng.Text;
                MsgInfo.Message[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language] = TextSystem.Text;

                CMainFrame.LWDicer.m_DataManager.MessageInfoList.Add(MsgInfo);
            }

            CMainFrame.LWDicer.m_DataManager.SaveMessageInfoList();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
