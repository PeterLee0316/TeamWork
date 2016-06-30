using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Control;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

namespace LWDicer.UI
{
    public partial class FormBottomScreen : Form
    {
        ButtonAdv[] BtnOP = new ButtonAdv[6];

        enum EBtnOption
        {
            Click,
            Disable,
            Enable,
            Select,
            Max,
        }

        enum EBtnNo
        {
            Auto,
            Manual,
            Data,
            Teach,
            Log,
            Help,
            Max,
        }

        public FormBottomScreen()
        {
            InitializeComponent();

            InitializeForm();

            ResouceMapping();
        }

        private void ResouceMapping()
        {
            BtnOP[0] = BtnAuto;
            BtnOP[1] = BtnManual;
            BtnOP[2] = BtnData;
            BtnOP[3] = BtnTeach;
            BtnOP[4] = BtnLog;
            BtnOP[5] = BtnHelp;

            for (int i = 0; i < (int)EBtnNo.Max; i++)
            {
                ButtonDisplay(i, EBtnOption.Enable);
            }

            ButtonDisplay((int)EBtnNo.Auto, EBtnOption.Select);
        }

        private void ButtonDisplay(int BtnNo, EBtnOption Option)
        {
            BtnOP[BtnNo].Image = ImageList.Images[(int)Option + (BtnNo * 4)];
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
            for (int i = 0; i < (int)EBtnNo.Max; i++)
            {
                ButtonDisplay(i, EBtnOption.Enable);
            }

            ButtonDisplay((int)EBtnNo.Auto, EBtnOption.Select);

            CMainFrame.MainFrame.DisplayManager.FormSelectChange(DEF_UI.SelectScreenType.Auto_Scr);
        }

        private void BtnManual_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EBtnNo.Max; i++)
            {
                ButtonDisplay(i, EBtnOption.Enable);
            }

            ButtonDisplay((int)EBtnNo.Manual, EBtnOption.Select);

            CMainFrame.MainFrame.DisplayManager.FormSelectChange(DEF_UI.SelectScreenType.Manual_Scr);
        }

        private void BtnData_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EBtnNo.Max; i++)
            {
                ButtonDisplay(i, EBtnOption.Enable);
            }

            ButtonDisplay((int)EBtnNo.Data, EBtnOption.Select);

            CMainFrame.MainFrame.DisplayManager.FormSelectChange(DEF_UI.SelectScreenType.Data_Scr);
        }

        private void BtnTeach_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EBtnNo.Max; i++)
            {
                ButtonDisplay(i, EBtnOption.Enable);
            }

            ButtonDisplay((int)EBtnNo.Teach, EBtnOption.Select);

            CMainFrame.MainFrame.DisplayManager.FormSelectChange(DEF_UI.SelectScreenType.Teach_Scr);
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EBtnNo.Max; i++)
            {
                ButtonDisplay(i, EBtnOption.Enable);
            }

            ButtonDisplay((int)EBtnNo.Log, EBtnOption.Select);

            CMainFrame.MainFrame.DisplayManager.FormSelectChange(DEF_UI.SelectScreenType.Log_Scr);
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EBtnNo.Max; i++)
            {
                ButtonDisplay(i, EBtnOption.Enable);
            }

            ButtonDisplay((int)EBtnNo.Help, EBtnOption.Select);

            CMainFrame.MainFrame.DisplayManager.FormSelectChange(DEF_UI.SelectScreenType.Help_Scr);
        }
    }
}
