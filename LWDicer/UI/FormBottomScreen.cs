using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Layers;

using static LWDicer.Layers.DEF_UI;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

namespace LWDicer.UI
{
    public partial class FormBottomScreen : Form
    {
        ButtonAdv[] BtnPage = new ButtonAdv[6];

        enum EBtnOption
        {
            Click,
            Disable,
            Enable,
            Select,
            Max,
        }

        private EFormType CurrentPage = EFormType.NONE;

        public FormBottomScreen()
        {
            InitializeComponent();

            InitializeForm();

            ResouceMapping();
        }

        private void ResouceMapping()
        {
            BtnPage[0] = BtnAuto;
            BtnPage[1] = BtnManual;
            BtnPage[2] = BtnData;
            BtnPage[3] = BtnTeach;
            BtnPage[4] = BtnLog;
            BtnPage[5] = BtnHelp;

            SelectPage(EFormType.AUTO);
        }

        void SelectPage(EFormType index)
        {
            if (CurrentPage == index) return;
            CurrentPage = index;
            for (int i = 0; i < (int)EFormType.MAX; i++)
            {
                if (i == (int)CurrentPage) continue;
                ButtonDisplay(i, EBtnOption.Enable);
            }

            ButtonDisplay((int)index, EBtnOption.Select);
            CMainFrame.MainFrame?.DisplayManager.FormSelectChange(index);
        }

        public void EnableBottomPage(bool bEnable)
        {
            for (int i = 0; i < (int)EFormType.MAX; i++)
            {
                if (i == (int)CurrentPage) continue;
                if(bEnable) ButtonDisplay(i, EBtnOption.Enable);
                else ButtonDisplay(i, EBtnOption.Disable);
                BtnPage[i].Enabled = bEnable;
            }
        }

        private void ButtonDisplay(int BtnNo, EBtnOption Option)
        {
            BtnPage[BtnNo].Image = ImageList.Images[(int)Option + (BtnNo * 4)];
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.BOT_POS_X, DEF_UI.BOT_POS_Y);
            this.Size = new Size(DEF_UI.BOT_SIZE_WIDTH, DEF_UI.BOT_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;

        }

        private void BtnAuto_Click(object sender, EventArgs e)
        {
            SelectPage(EFormType.AUTO);
        }

        private void BtnManual_Click(object sender, EventArgs e)
        {
            SelectPage(EFormType.MANUAL);
        }

        private void BtnData_Click(object sender, EventArgs e)
        {
            SelectPage(EFormType.DATA);
        }

        private void BtnTeach_Click(object sender, EventArgs e)
        {
            SelectPage(EFormType.TEACH);
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            SelectPage(EFormType.LOG);
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            SelectPage(EFormType.HELP);
        }
    }
}
