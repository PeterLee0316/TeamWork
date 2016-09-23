﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LWDicer.Layers;
using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;

namespace LWDicer.UI
{
    public partial class FormTopScreen : Form
    {
        public static FormTopScreen TopMenu = null;

        public FormTopScreen()
        {
            InitializeComponent();

            InitializeForm();

            TopMenu = this;
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.TOP_POS_X, DEF_UI.TOP_POS_Y);
            this.Size = new Size(DEF_UI.TOP_SIZE_WIDTH, DEF_UI.TOP_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;

            tmFormTop.Interval = UITimerInterval;
            tmFormTop.Enabled = true;
            tmFormTop.Start();

            btnStart.UseVisualStyleBackColor = true;
            btnStop.UseVisualStyleBackColor = true;
            btnReset.UseVisualStyleBackColor = true;
            btnEMO.UseVisualStyleBackColor = true;

        }

        private void tmFormTop_Tick(object sender, EventArgs e)
        {
            TextTime.Text = DateTime.Now.ToString("yyyy-MM-dd [ddd] <tt> HH:mm:ss");

            //LabelCurUser.Text = $"Current User : {CMainFrame.DataManager.LoginInfo.User.Name}";
            BtnUserLogin.Text = $"Login : {CMainFrame.DataManager.LoginInfo.User.Name}";

            // OP Switch
            bool bStatus;
            CMainFrame.LWDicer.m_ctrlOpPanel.GetStartSWStatus(out bStatus);
            if (bStatus) btnStart.BackColor = Color.LightGreen;
            else btnStart.BackColor = Color.LightGray;

            CMainFrame.LWDicer.m_ctrlOpPanel.GetStopSWStatus(out bStatus);
            if (bStatus) btnStop.BackColor = Color.LightGreen;
            else btnStop.BackColor = Color.LightGray;

            CMainFrame.LWDicer.m_ctrlOpPanel.GetResetSWStatus(out bStatus);
            if (bStatus) btnReset.BackColor = Color.LightBlue;
            else btnReset.BackColor = Color.LightGray;

            CMainFrame.LWDicer.m_ctrlOpPanel.GetEStopSWStatus(out bStatus);
            if (bStatus) btnEMO.BackColor = Color.Red;
            else btnEMO.BackColor = Color.LightGray;

            // TowerLamp
            CMainFrame.LWDicer.m_IO.GetBit(DEF_IO.oTower_LampRed, out bStatus);
            if (bStatus) LabelTowerR.BackgroundColor = CMainFrame.Brush_R;
            else LabelTowerR.BackgroundColor = CMainFrame.Brush_Gray;

            CMainFrame.LWDicer.m_IO.GetBit(DEF_IO.oTower_LampYellow, out bStatus);
            if (bStatus) LabelTowerY.BackgroundColor = CMainFrame.Brush_Y;
            else LabelTowerY.BackgroundColor = CMainFrame.Brush_Gray;

            CMainFrame.LWDicer.m_IO.GetBit(DEF_IO.oTower_LampGreen, out bStatus);
            if (bStatus) LabelTowerG.BackgroundColor = CMainFrame.Brush_G;
            else LabelTowerG.BackgroundColor = CMainFrame.Brush_Gray;

            bStatus = false;
            bool bStatus1;
            CMainFrame.LWDicer.m_IO.GetBit(DEF_IO.oBuzzer_1, out bStatus1);
            bStatus |= bStatus1;
            CMainFrame.LWDicer.m_IO.GetBit(DEF_IO.oBuzzer_2, out bStatus1);
            bStatus |= bStatus1;
            CMainFrame.LWDicer.m_IO.GetBit(DEF_IO.oBuzzer_3, out bStatus1);
            bStatus |= bStatus1;
            CMainFrame.LWDicer.m_IO.GetBit(DEF_IO.oBuzzer_4, out bStatus1);
            bStatus |= bStatus1;
            if (bStatus) LabelBuzzer.BackgroundColor = CMainFrame.Brush_B;
            else LabelBuzzer.BackgroundColor = CMainFrame.Brush_Gray;
        }

        public void SetMessage(string strMsg)
        {
            TextMessage.Text = strMsg;
        }

        private void BtnUserLogin_Click(object sender, EventArgs e)
        {
            FormUserLogin dlg = new FormUserLogin();
            dlg.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (CMainFrame.InquireMsg("Exit System?"))
            {
                CMainFrame.LWDicer.CloseSystem();

                Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlOpPanel.TempOnStartSWStatus();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlOpPanel.TempOnStopSWStatus();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlOpPanel.TempOnResetSWStatus();
        }

        private void btnEMO_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlOpPanel.TempOnEMOSWStatus();
        }
    }
}
