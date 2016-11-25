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

using LWDicer.Layers;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_System;

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
            dlg.Type_Fixed = true;
            dlg.ShowDialog();
        }

        private void BtnTeachPushPull_Click(object sender, EventArgs e)
        {
            var dlg = new FormPushPullTeach();
            dlg.Type_Fixed = true;
            dlg.ShowDialog();
        }

        private void BtnTeachSpinner1_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinner1Teach();
            dlg.CtrlSpinner        = CMainFrame.LWDicer.m_ctrlSpinner1;
            dlg.MO_Rotate          = CMainFrame.LWDicer.m_MeSpinner1.AxRotateInfo;
            dlg.MO_CleanNozzle     = CMainFrame.LWDicer.m_MeSpinner1.AxCleanNozzleInfo;
            dlg.MO_CoatNozzle      = CMainFrame.LWDicer.m_MeSpinner1.AxCoatNozzleInfo;
            dlg.TeachUnit          = ETeachUnit.CLEANER1;
            dlg.PIndex_Rotate          = EPositionObject.S1_ROTATE;
            dlg.PIndex_CleanNozzle     = EPositionObject.S1_CLEAN_NOZZLE;
            dlg.PIndex_CoatNozzle      = EPositionObject.S1_COAT_NOZZLE;
            dlg.PositionGroup      = EPositionGroup.SPINNER1;
            dlg.Axis_Rotate_T      = EYMC_Axis.S1_CHUCK_ROTATE_T;
            dlg.Axis_CleanNozzle_T = EYMC_Axis.S1_CLEAN_NOZZLE_T;
            dlg.Axis_CoatNozzle_T  = EYMC_Axis.S1_COAT_NOZZLE_T;

            dlg.Type_Fixed = true;
            dlg.ShowDialog();
        }

        private void BtnTeachSpinner2_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinner1Teach();
            dlg.CtrlSpinner        = CMainFrame.LWDicer.m_ctrlSpinner2;
            dlg.MO_Rotate          = CMainFrame.LWDicer.m_MeSpinner2.AxRotateInfo;
            dlg.MO_CleanNozzle     = CMainFrame.LWDicer.m_MeSpinner2.AxCleanNozzleInfo;
            dlg.MO_CoatNozzle      = CMainFrame.LWDicer.m_MeSpinner2.AxCoatNozzleInfo;
            dlg.TeachUnit          = ETeachUnit.CLEANER2;
            dlg.PIndex_Rotate          = EPositionObject.S2_ROTATE;
            dlg.PIndex_CleanNozzle     = EPositionObject.S2_CLEAN_NOZZLE;
            dlg.PIndex_CoatNozzle      = EPositionObject.S2_COAT_NOZZLE;
            dlg.PositionGroup      = EPositionGroup.SPINNER2;
            dlg.Axis_Rotate_T      = EYMC_Axis.S2_CHUCK_ROTATE_T;
            dlg.Axis_CleanNozzle_T = EYMC_Axis.S2_CLEAN_NOZZLE_T;
            dlg.Axis_CoatNozzle_T  = EYMC_Axis.S2_COAT_NOZZLE_T;

            dlg.Type_Fixed = true;
            dlg.ShowDialog();
        }

        private void BtnTeachHandler_Click(object sender, EventArgs e)
        {
            var dlg = new FormHandlerTeach();
            dlg.Type_Fixed = true;
            dlg.ShowDialog();
        }

        private void BtnTeachStage_Click(object sender, EventArgs e)
        {
            var dlg = new FormWorkStageTeach();
            dlg.Type_Fixed = true;
            dlg.ShowDialog();
        }

        private void BtnCameraScanner_Click(object sender, EventArgs e)
        {
            var dlg = new FormCameraTeach();
            dlg.Type_Fixed = true;
            dlg.ShowDialog();
        }

        private void BtnScanner_Click(object sender, EventArgs e)
        {
            var dlg = new FormScannerTeach();
            dlg.Type_Fixed = true;
            dlg.ShowDialog();
        }

        private void BtnModelLoader_Click(object sender, EventArgs e)
        {
            var dlg = new FormLoaderTeach();
            dlg.Type_Fixed = false;
            dlg.ShowDialog();
        }

        private void BtnModelPushPull_Click(object sender, EventArgs e)
        {
            var dlg = new FormPushPullTeach();
            dlg.Type_Fixed = false;
            dlg.ShowDialog();
        }

        private void BtnModelSpinner1_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinner1Teach();
            dlg.CtrlSpinner        = CMainFrame.LWDicer.m_ctrlSpinner1;
            dlg.MO_Rotate          = CMainFrame.LWDicer.m_MeSpinner1.AxRotateInfo;
            dlg.MO_CleanNozzle     = CMainFrame.LWDicer.m_MeSpinner1.AxCleanNozzleInfo;
            dlg.MO_CoatNozzle      = CMainFrame.LWDicer.m_MeSpinner1.AxCoatNozzleInfo;
            dlg.TeachUnit          = ETeachUnit.CLEANER1;
            dlg.PIndex_Rotate          = EPositionObject.S1_ROTATE;
            dlg.PIndex_CleanNozzle     = EPositionObject.S1_CLEAN_NOZZLE;
            dlg.PIndex_CoatNozzle      = EPositionObject.S1_COAT_NOZZLE;
            dlg.PositionGroup      = EPositionGroup.SPINNER1;
            dlg.Axis_Rotate_T      = EYMC_Axis.S1_CHUCK_ROTATE_T;
            dlg.Axis_CleanNozzle_T = EYMC_Axis.S1_CLEAN_NOZZLE_T;
            dlg.Axis_CoatNozzle_T  = EYMC_Axis.S1_COAT_NOZZLE_T;

            dlg.Type_Fixed = false;
            dlg.ShowDialog();
        }

        private void BtnModelSpinner2_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinner1Teach();
            dlg.CtrlSpinner        = CMainFrame.LWDicer.m_ctrlSpinner2;
            dlg.MO_Rotate          = CMainFrame.LWDicer.m_MeSpinner2.AxRotateInfo;
            dlg.MO_CleanNozzle     = CMainFrame.LWDicer.m_MeSpinner2.AxCleanNozzleInfo;
            dlg.MO_CoatNozzle      = CMainFrame.LWDicer.m_MeSpinner2.AxCoatNozzleInfo;
            dlg.TeachUnit          = ETeachUnit.CLEANER2;
            dlg.PIndex_Rotate          = EPositionObject.S2_ROTATE;
            dlg.PIndex_CleanNozzle     = EPositionObject.S2_CLEAN_NOZZLE;
            dlg.PIndex_CoatNozzle      = EPositionObject.S2_COAT_NOZZLE;
            dlg.PositionGroup      = EPositionGroup.SPINNER2;
            dlg.Axis_Rotate_T      = EYMC_Axis.S2_CHUCK_ROTATE_T;
            dlg.Axis_CleanNozzle_T = EYMC_Axis.S2_CLEAN_NOZZLE_T;
            dlg.Axis_CoatNozzle_T  = EYMC_Axis.S2_COAT_NOZZLE_T;

            dlg.Type_Fixed = false;
            dlg.ShowDialog();
        }

        private void BtnModelHandler_Click(object sender, EventArgs e)
        {
            var dlg = new FormHandlerTeach();
            dlg.Type_Fixed = false;
            dlg.ShowDialog();
        }

        private void BtnModelStage_Click(object sender, EventArgs e)
        {
            var dlg = new FormWorkStageTeach();
            dlg.Type_Fixed = false;
            dlg.ShowDialog();
        }

        private void BtnModelScanner_Click(object sender, EventArgs e)
        {
            var dlg = new FormScannerTeach();
            dlg.Type_Fixed = false;
            dlg.ShowDialog();
        }

        private void BtnModelCamera_Click(object sender, EventArgs e)
        {
            var dlg = new FormCameraTeach();
            dlg.Type_Fixed = false;
            dlg.ShowDialog();
        }



        private void btnEdgeAlign_Click(object sender, EventArgs e)
        {
            var dlg = new FormEdgeAlignTeach();
            dlg.StartPosition = FormStartPosition.Manual;
            dlg.Location = new Point(0, 120);
            dlg.ShowDialog();
        }

        private void btnMacroAlgin_Click(object sender, EventArgs e)
        {
            var dlg = new FormMacroAlignTeach();
            dlg.StartPosition = FormStartPosition.Manual;
            dlg.Location = new Point(0, 120);
            dlg.ShowDialog();
        }

        private void btnMicroAlign_Click(object sender, EventArgs e)
        {
            var dlg = new FormMicroAlignTeach();
            dlg.StartPosition = FormStartPosition.Manual;
            dlg.Location = new Point(0, 120);
            dlg.ShowDialog();
        }

        private void btnLaserAlign_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new FormLaserAlignTeach();
                dlg.StartPosition = FormStartPosition.Manual;
                dlg.Location = new Point(0, 120);
                dlg.ShowDialog();
            }
            catch
            {

            }
        }

        private void btnCamStageTeach_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new FormCameraAlignTeach();
                dlg.StartPosition = FormStartPosition.Manual;
                dlg.Location = new Point(0, 120);
                dlg.ShowDialog();
            }
            catch
            {

            }
        }

        private void btnWaferTeach_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new FormWaferDieTeach();
                dlg.StartPosition = FormStartPosition.Manual;
                dlg.Location = new Point(0, 120);
                dlg.ShowDialog();
            }
            catch
            {

            }
        }
    }
}
