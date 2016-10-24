using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Drawing;

using LWDicer.Layers;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_LCNet;

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
            dlg.SetHandler(DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER);
            dlg.ShowDialog();
        }

        private void BtnManualLoHandler_Click(object sender, EventArgs e)
        {
            var dlg = new FormHandlerManualOP();
            dlg.SetHandler(DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER);
            dlg.ShowDialog();
        }

        private void BtnManualSpinner1_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinnerManualOP();
            dlg.SetSpinner(ESpinnerIndex.SPINNER1);
            dlg.ShowDialog();
        }

        private void BtnManualSpinner2_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinnerManualOP();
            dlg.SetSpinner(ESpinnerIndex.SPINNER2);
            dlg.ShowDialog();
        }

        private void BtnManualStage_Click(object sender, EventArgs e)
        {
            var dlg = new FormStageManualOP();
            dlg.ShowDialog();
        }

        private void FormManualScreen_Activated(object sender, EventArgs e)
        {
            if (CMainFrame.DataManager.SystemData.UseSpinnerSeparately)
            {
                if (CMainFrame.DataManager.SystemData.UCoaterIndex == ESpinnerIndex.SPINNER1)
                {
                    Panel_Spinner1_Nozzle1.Text = "Coater Nozzle1";
                    Panel_Spinner1_Nozzle2.Text = "Coater Nozzle2";
                    Panel_Spinner1_Rotate.Text = "Coater Rotate";
                    BtnManualSpinner1.Text = "Coater";

                    Panel_Spinner2_Nozzle1.Text = "Cleaner Nozzle1";
                    Panel_Spinner2_Nozzle2.Text = "Cleaner Nozzle2";
                    Panel_Spinner2_Rotate.Text = "Cleaner Rotate";
                    BtnManualSpinner2.Text = "Cleaner";
                }
                else
                {
                    Panel_Spinner1_Nozzle1.Text = "Cleaner Nozzle1";
                    Panel_Spinner1_Nozzle2.Text = "Cleaner Nozzle2";
                    Panel_Spinner1_Rotate.Text = "Cleaner Rotate";
                    BtnManualSpinner1.Text = "Cleaner";

                    Panel_Spinner2_Nozzle1.Text = "Coater Nozzle1";
                    Panel_Spinner2_Nozzle2.Text = "Coater Nozzle2";
                    Panel_Spinner2_Rotate.Text = "Coater Rotate";
                    BtnManualSpinner2.Text = "Coater";
                }
            }
            else
            {
                Panel_Spinner1_Nozzle1.Text = "Spinner1 Nozzle1";
                Panel_Spinner1_Nozzle2.Text = "Spinner1 Nozzle2";
                Panel_Spinner1_Rotate.Text  = "Spinner1 Rotate";
                BtnManualSpinner1.Text = "Spinner1";

                Panel_Spinner2_Nozzle1.Text = "Spinner2 Nozzle1";
                Panel_Spinner2_Nozzle2.Text = "Spinner2 Nozzle2";
                Panel_Spinner2_Rotate.Text = "Spinner2 Rotate";
                BtnManualSpinner2.Text = "Spinner2";
            }
        }
    }
}
