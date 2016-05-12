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
    public partial class FormBottomScreen : Form
    {
        public FormBottomScreen()
        {
            InitializeComponent();

            InitializeForm();
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
            CMainFrame.MainFrame.m_DisPlayManager.FormSelectChange(DEF_UI.SelectScreenType.Auto_Scr);
        }

        private void BtnManual_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.m_DisPlayManager.FormSelectChange(DEF_UI.SelectScreenType.Manual_Scr);
        }

        private void BtnData_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.m_DisPlayManager.FormSelectChange(DEF_UI.SelectScreenType.Data_Scr);
        }

        private void BtnTeach_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.m_DisPlayManager.FormSelectChange(DEF_UI.SelectScreenType.Teach_Scr);
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.m_DisPlayManager.FormSelectChange(DEF_UI.SelectScreenType.Log_Scr);
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.m_DisPlayManager.FormSelectChange(DEF_UI.SelectScreenType.Help_Scr);
        }
    }
}
