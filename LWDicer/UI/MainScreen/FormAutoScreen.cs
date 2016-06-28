﻿using System;
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

using static LWDicer.Control.DEF_Error;

namespace LWDicer.UI
{
    public partial class FormAutoScreen : Form
    {
        public FormAutoScreen()
        {
            InitializeComponent();
            InitializeForm();
        }
        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }


        private void FormAutoScreen_Activated(object sender, EventArgs e)
        {

        }

        private void FormAutoScreen_Deactivate(object sender, EventArgs e)
        {

        }

        private void FormAutoScreen_Shown(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void BtnAlarmTest_Click(object sender, EventArgs e)
        {
            ButtonAdv BtnAlarm = sender as ButtonAdv;

            if (Convert.ToInt16(BtnAlarm.Tag) == 100) CMainFrame.LWDicer.AlarmDisplay(EAlarmGroup.LOADER, Convert.ToInt16(BtnAlarm.Tag));
            if (Convert.ToInt16(BtnAlarm.Tag) == 200) CMainFrame.LWDicer.AlarmDisplay(EAlarmGroup.PUSHPULL, Convert.ToInt16(BtnAlarm.Tag));
            if (Convert.ToInt16(BtnAlarm.Tag) == 300) CMainFrame.LWDicer.AlarmDisplay(EAlarmGroup.HANDLER, Convert.ToInt16(BtnAlarm.Tag));
            if (Convert.ToInt16(BtnAlarm.Tag) == 400) CMainFrame.LWDicer.AlarmDisplay(EAlarmGroup.HANDLER, Convert.ToInt16(BtnAlarm.Tag));
            if (Convert.ToInt16(BtnAlarm.Tag) == 500) CMainFrame.LWDicer.AlarmDisplay(EAlarmGroup.SPINNER, Convert.ToInt16(BtnAlarm.Tag));
            if (Convert.ToInt16(BtnAlarm.Tag) == 600) CMainFrame.LWDicer.AlarmDisplay(EAlarmGroup.SPINNER, Convert.ToInt16(BtnAlarm.Tag));
            if (Convert.ToInt16(BtnAlarm.Tag) == 700) CMainFrame.LWDicer.AlarmDisplay(EAlarmGroup.STAGE, Convert.ToInt16(BtnAlarm.Tag));
        }
    }
}
