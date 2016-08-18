using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;

using LWDicer.Control;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_System;

namespace LWDicer.UI
{
    public partial class FormTeachScreen : Form
    {
        public FormTeachScreen()
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

        private void BtnTeachLoader_Click(object sender, EventArgs e)
        {
            var dlg = new FormLoaderTeach();
            dlg.SetDataMode(FixedData);
            dlg.ShowDialog();
        }

        private void BtnTeachPushPull_Click(object sender, EventArgs e)
        {
            var dlg = new FormPushPullTeach();
            dlg.SetDataMode(FixedData);
            dlg.ShowDialog();
        }

        private void BtnTeachSpinner1_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinner1Teach();
            dlg.SetDataMode(FixedData);
            dlg.ShowDialog();
        }

        private void BtnTeachSpinner2_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinner2Teach();
            dlg.SetDataMode(FixedData);
            dlg.ShowDialog();
        }

        private void BtnTeachHandler_Click(object sender, EventArgs e)
        {
            var dlg = new FormHandlerTeach();
            dlg.SetDataMode(FixedData);
            dlg.ShowDialog();
        }

        private void BtnTeachStage_Click(object sender, EventArgs e)
        {
            var dlg = new FormWorkStageTeach();
            dlg.SetDataMode(FixedData);
            dlg.ShowDialog();
        }

        private void BtnCameraScanner_Click(object sender, EventArgs e)
        {
            var dlg = new FormCameraTeach();
            dlg.SetDataMode(FixedData);
            dlg.ShowDialog();
        }

        private void BtnScanner_Click(object sender, EventArgs e)
        {
            var dlg = new FormScannerTeach();
            dlg.SetDataMode(FixedData);
            dlg.ShowDialog();
        }

        private void BtnModelLoader_Click(object sender, EventArgs e)
        {
            var dlg = new FormLoaderTeach();
            dlg.SetDataMode(OffsetData);
            dlg.ShowDialog();
        }

        private void BtnModelPushPull_Click(object sender, EventArgs e)
        {
            var dlg = new FormPushPullTeach();
            dlg.SetDataMode(OffsetData);
            dlg.ShowDialog();
        }

        private void BtnModelSpinner1_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinner1Teach();
            dlg.SetDataMode(OffsetData);
            dlg.ShowDialog();
        }

        private void BtnModelSpinner2_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinner2Teach();
            dlg.SetDataMode(OffsetData);
            dlg.ShowDialog();
        }

        private void BtnModelHandler_Click(object sender, EventArgs e)
        {
            var dlg = new FormHandlerTeach();
            dlg.SetDataMode(OffsetData);
            dlg.ShowDialog();
        }

        private void BtnModelStage_Click(object sender, EventArgs e)
        {
            var dlg = new FormWorkStageTeach();
            dlg.SetDataMode(OffsetData);
            dlg.ShowDialog();
        }

        private void BtnModelScanner_Click(object sender, EventArgs e)
        {
            var dlg = new FormScannerTeach();
            dlg.SetDataMode(OffsetData);
            dlg.ShowDialog();
        }

        private void BtnModelCamera_Click(object sender, EventArgs e)
        {
            var dlg = new FormCameraTeach();
            dlg.SetDataMode(OffsetData);
            dlg.ShowDialog();
        }

        private void btnThetaAlign_Click(object sender, EventArgs e)
        {
            var dlg = new FormThetaAlignTeach();
            dlg.ShowDialog();
        }

        private void btnEdgeAlign_Click(object sender, EventArgs e)
        {
            var dlg = new FormEdgeAlignTeach();
            dlg.ShowDialog();
        }

        private void btnMacroAlgin_Click(object sender, EventArgs e)
        {
            var dlg = new FormMacroAlignTeach();
            dlg.ShowDialog();
        }

        private void btnMicroAlign_Click(object sender, EventArgs e)
        {
            var dlg = new FormMicroAlignTeach();
            dlg.ShowDialog();
        }
    }
}
