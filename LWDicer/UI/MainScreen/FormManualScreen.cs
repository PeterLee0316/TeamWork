using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Control;

using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Drawing;

namespace LWDicer.UI
{
    public partial class FormManualScreen : Form
    {
        const int INPUT = 0;
        const int OUTPUT = 1;

        const int LoHandler = 0;
        const int UpHandler = 1;

        const int Spinner1 = 0;
        const int Spinner2 = 1;

        public FormManualScreen()
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

        private void BtnInput_Click(object sender, EventArgs e)
        {
            var dlg = new FormIO();
            dlg.ShowDialog();
        }

        private void BtnLimitSensor_Click(object sender, EventArgs e)
        {
            var dlg = new FormLimitSensor();
            dlg.ShowDialog();
        }

        private void BtnOriginReturn_Click(object sender, EventArgs e)
        {
            var dlg = new FormOriginReturn();
            dlg.ShowDialog();
        }

        private void BtnUnitInit_Click(object sender, EventArgs e)
        {
            var dlg = new FormUnitInit();
            dlg.ShowDialog();
        }

        private void BtnManualPushPull_Click(object sender, EventArgs e)
        {
            var dlg = new FormPushPullManualOP();
            dlg.ShowDialog();
        }

        private void BtnManualUpHandler_Click(object sender, EventArgs e)
        {
            var dlg = new FormHandlerManualOP();
            dlg.SetHandler(UpHandler);
            dlg.ShowDialog();
        }

        private void BtnManualLoHandler_Click(object sender, EventArgs e)
        {
            var dlg = new FormHandlerManualOP();
            dlg.SetHandler(LoHandler);
            dlg.ShowDialog();
        }

        private void BtnManualSpinner1_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinnerManualOP();
            dlg.SetSpinner(Spinner1);
            dlg.ShowDialog();
        }

        private void BtnManualSpinner2_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinnerManualOP();
            dlg.SetSpinner(Spinner2);
            dlg.ShowDialog();
        }

        private void BtnManualStage_Click(object sender, EventArgs e)
        {
            var dlg = new FormStageManualOP();
            dlg.ShowDialog();
        }
    }
}
