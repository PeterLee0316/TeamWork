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
        private CListHeader NewHeader;
        private EListHeaderType ListType;
        private List<CListHeader> HeaderList;

        public FormCreateMaker(EListHeaderType type, string parentName)
        {
            InitializeComponent();

            NewHeader = new CListHeader();
            ListType = type;
            NewHeader.Parent = parentName;
            switch (type)
            {
                case EListHeaderType.MODEL:
                    HeaderList = CMainFrame.LWDicer.m_DataManager.ModelHeaderList;
                    break;
                case EListHeaderType.CASSETTE:
                    HeaderList = CMainFrame.LWDicer.m_DataManager.CassetteHeaderList;
                    break;
                case EListHeaderType.WAFERFRAME:
                    HeaderList = CMainFrame.LWDicer.m_DataManager.WaferFrameHeaderList;
                    break;
            }
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
            string strCurrent = "", strModify = "";

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            LabelMakerDir.Text = strModify;

            int nCount = HeaderList.Count;

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
                CMainFrame.DisplayMsg("", "Maker Name을 입력 하십시오", false);
                return;
            }

            // Maker Name 중복 검사
            int i = 0;

            for(i=0;i< HeaderList.Count;i++)
            {
                if(NewHeader.Name == HeaderList[i].Name)
                {
                    CMainFrame.DisplayMsg("", "현재 등록되어 있는 Maker 입니다.",false);
                    return;
                }
            }

            NewHeader.IsFolder = true;
            NewHeader.TreeLevel = 1;

            HeaderList.Add(NewHeader);
            CMainFrame.LWDicer.m_DataManager.SaveModelHeaderList(ListType);

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
            //LabelMakerName.Text;
            //LabelMakerComment.Text;
            //LabelMakerParent.Text;
            //LabelMakerDir.Text;
        }
    }
}
