﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Layers;

using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

using Excel = Microsoft.Office.Interop.Excel;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Yaskawa;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormDataScreen : Form
    {

        public FormDataScreen()
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

        private void FormDataScreen_Activated(object sender, EventArgs e)
        {

        }

        private void FormDataScreen_Deactivate(object sender, EventArgs e)
        {

        }

        private void BtnScannerData_Click(object sender, EventArgs e)
        {
            var dlg = new FormPolygon();
            dlg.ShowDialog();
        }

        private void BtnMotorData_Click(object sender, EventArgs e)
        {
            var dlg = new FormMotorData();
            dlg.ShowDialog();
        }

        private void BtnCylinderData_Click(object sender, EventArgs e)
        {
            var dlg = new FormCylinderData();
            dlg.ShowDialog();
        }

        private void BtnVisionData_Click(object sender, EventArgs e)
        {
            var dlg = new FormVisionData();
            //dlg.ShowDialog();
            dlg.Show();
        }

        private void BtnModelList_Click(object sender, EventArgs e)
        {
            var dlg = new FormModelList(EListHeaderType.MODEL);
            dlg.ShowDialog();
        }

        private void BtnWaferList_Click(object sender, EventArgs e)
        {
            var dlg = new FormModelList(EListHeaderType.WAFERFRAME);
            dlg.ShowDialog();
        }

        private void BtnVacuum_Click(object sender, EventArgs e)
        {
            var dlg = new FormVacuumData();
            dlg.ShowDialog();
        }

        private void BtnExcelLoad_Click(object sender, EventArgs e)
        {
            var dlg = new FormSystemParaRead();
            dlg.ShowDialog();
        }

        private void BtnSystemData_Click(object sender, EventArgs e)
        {
            var dlg = new FormSystemData();
            dlg.ShowDialog();
        }

        private void BtnModelData_Click(object sender, EventArgs e)
        {
            var dlg = new FormModelData();
            dlg.ShowDialog();
        }

        private void BtnWaferData_Click(object sender, EventArgs e)
        {
            var dlg = new FormWaferData();
            dlg.ShowDialog();
        }

        private void BtnCleaner_Click(object sender, EventArgs e)
        {
            var dlg = new FormCleanerData();
            dlg.ShowDialog();
        }

        private void BtnCoater_Click(object sender, EventArgs e)
        {
            var dlg = new FormCoaterData();
            dlg.ShowDialog();
        }

        private void FormDataScreen_Load(object sender, EventArgs e)
        {

        }

        private void BtnUserList_Click(object sender, EventArgs e)
        {
            var dlg = new FormModelList(EListHeaderType.USERINFO);
            dlg.ShowDialog();

        }

        private void BtnWaferFrameData_Click(object sender, EventArgs e)
        {
            var dlg = new FormWaferFrameData();
            dlg.ShowDialog();
        }

        private void BtnLaserProcess_Click(object sender, EventArgs e)
        {
            var dlg = new FormLaserProcessData();
            dlg.ShowDialog();
        }
    }
}
