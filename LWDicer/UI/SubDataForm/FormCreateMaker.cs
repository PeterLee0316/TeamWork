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

namespace LWDicer.UI
{
    public partial class FormCreateMaker : Form
    {
        private CModelHeader NewHeader;

        public FormCreateMaker()
        {
            InitializeComponent();

            NewHeader = new CModelHeader();
        }

        private void LabelMakerName_Click(object sender, EventArgs e)
        {
            string strModify = "";

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify))
            {
                return;
            }

            LabelMakerName.Text = strModify;

            NewHeader.Name = strModify;
        }

        private void LabelMakerComment_Click(object sender, EventArgs e)
        {
            string strModify = "";

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify))
            {
                return;
            }

            LabelMakerComment.Text = strModify;

            NewHeader.Comment = strModify;

        }

        private void LabelMakerParent_Click(object sender, EventArgs e)
        {
            string strModify = "";

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify))
            {
                return;
            }

            LabelMakerParent.Text = strModify;

            NewHeader.Parent = strModify;

        }

        private void LabelMakerDir_Click(object sender, EventArgs e)
        {
            string StrCurrent = "", strModify = "";

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            LabelMakerDir.Text = strModify;

            int nCount = CMainFrame.LWDicer.m_DataManager.ModelHeaderList.Count;

            if (strModify == "1")
            {
                NewHeader.IsFolder = true;
            }
            else if (strModify == "0")
            {
                NewHeader.IsFolder = false;
            }
        }

        private void FormClose(DialogResult DlgResult)
        {
            if(DlgResult == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }

            if (DlgResult == DialogResult.Cancel)
            {
                this.DialogResult = DialogResult.Cancel;
            }

            this.Hide();
        }

        private void BtnHeaderCreate_Click(object sender, EventArgs e)
        {
            if(LabelMakerName.Text == "" || LabelMakerName.Text == null)
            {
                CMainFrame.LWDicer.DisplayMsg("Maker Name을 입력 하십시오");
                return;
            }

            // Maker Name 중복 검사
            int i = 0;

            for(i=0;i< CMainFrame.LWDicer.m_DataManager.ModelHeaderList.Count;i++)
            {
                if(NewHeader.Name == CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name)
                {
                    CMainFrame.LWDicer.DisplayMsg("현재 등록되어 있는 Maker 입니다.");
                    return;
                }
            }

            NewHeader.Parent = NewHeader.Name;
            NewHeader.IsFolder = true;
            NewHeader.TreeLevel = 1;

            CMainFrame.LWDicer.m_DataManager.ModelHeaderList.Add(NewHeader);

            CMainFrame.LWDicer.m_DataManager.SaveModelHeaderList();

            CMainFrame.LWDicer.m_DataManager.LoadModelList();

            FormClose(DialogResult.OK);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            FormClose(DialogResult.Cancel);
        }

        private void FormCreateMaker_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FormCreateMaker_Load(object sender, EventArgs e)
        {
            LabelMakerName.Text = string.Empty;
            LabelMakerComment.Text = string.Empty;
            LabelMakerParent.Text = string.Empty;
            LabelMakerDir.Text = string.Empty;
        }
    }
}
