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

using LWDicer.UI;
using LWDicer.Control;

using static LWDicer.Control.DEF_PolygonScanner;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_System;

namespace LWDicer.UI
{
    public partial class FormTeachScreen : Form
    {
        private FormCameraTeach m_CameraTeach;
        private FormHandlerTeach m_HandlerTeach;
        private FormLoaderTeach m_LoaderTeach;
        private FormPushPullTeach m_PushPullTeach;
        private FormScannerTeach m_ScannerTeach;
        private FormSpinner1Teach m_Spinner1Teach;
        private FormSpinner2Teach m_Spinner2Teach;
        private FormWorkStageTeach m_WorkStageTeach;

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
            m_LoaderTeach = new FormLoaderTeach();
            m_LoaderTeach.SetDataMode(FixedData);
            m_LoaderTeach.ShowDialog();
        }

        private void BtnTeachPushPull_Click(object sender, EventArgs e)
        {
            m_PushPullTeach = new FormPushPullTeach();
            m_PushPullTeach.SetDataMode(FixedData);
            m_PushPullTeach.ShowDialog();
        }

        private void BtnTeachSpinner1_Click(object sender, EventArgs e)
        {
            m_Spinner1Teach = new FormSpinner1Teach();
            m_Spinner1Teach.SetDataMode(FixedData);
            m_Spinner1Teach.ShowDialog();
        }

        private void BtnTeachSpinner2_Click(object sender, EventArgs e)
        {
            m_Spinner2Teach = new FormSpinner2Teach();
            m_Spinner2Teach.SetDataMode(FixedData);
            m_Spinner2Teach.ShowDialog();
        }

        private void BtnTeachHandler_Click(object sender, EventArgs e)
        {
            m_HandlerTeach = new FormHandlerTeach();
            m_HandlerTeach.SetDataMode(FixedData);
            m_HandlerTeach.ShowDialog();
        }

        private void BtnTeachStage_Click(object sender, EventArgs e)
        {
            m_WorkStageTeach = new FormWorkStageTeach();
            m_WorkStageTeach.SetDataMode(FixedData);
            m_WorkStageTeach.ShowDialog();
        }

        private void BtnCameraScanner_Click(object sender, EventArgs e)
        {
            m_CameraTeach = new FormCameraTeach();
            m_CameraTeach.SetDataMode(FixedData);
            m_CameraTeach.ShowDialog();
        }

        private void BtnScanner_Click(object sender, EventArgs e)
        {
            m_ScannerTeach = new FormScannerTeach();
            m_ScannerTeach.SetDataMode(FixedData);
            m_ScannerTeach.ShowDialog();
        }

        private void BtnModelLoader_Click(object sender, EventArgs e)
        {
            m_LoaderTeach = new FormLoaderTeach();
            m_LoaderTeach.SetDataMode(OffsetData);
            m_LoaderTeach.ShowDialog();
        }

        private void BtnModelPushPull_Click(object sender, EventArgs e)
        {
            m_PushPullTeach = new FormPushPullTeach();
            m_PushPullTeach.SetDataMode(OffsetData);
            m_PushPullTeach.ShowDialog();
        }

        private void BtnModelSpinner1_Click(object sender, EventArgs e)
        {
            m_Spinner1Teach = new FormSpinner1Teach();
            m_Spinner1Teach.SetDataMode(OffsetData);
            m_Spinner1Teach.ShowDialog();
        }

        private void BtnModelSpinner2_Click(object sender, EventArgs e)
        {
            m_Spinner2Teach = new FormSpinner2Teach();
            m_Spinner2Teach.SetDataMode(OffsetData);
            m_Spinner2Teach.ShowDialog();
        }

        private void BtnModelHandler_Click(object sender, EventArgs e)
        {
            m_HandlerTeach = new FormHandlerTeach();
            m_HandlerTeach.SetDataMode(OffsetData);
            m_HandlerTeach.ShowDialog();
        }

        private void BtnModelStage_Click(object sender, EventArgs e)
        {
            m_WorkStageTeach = new FormWorkStageTeach();
            m_WorkStageTeach.SetDataMode(OffsetData);
            m_WorkStageTeach.ShowDialog();
        }

        private void BtnModelScanner_Click(object sender, EventArgs e)
        {
            m_ScannerTeach = new FormScannerTeach();
            m_ScannerTeach.SetDataMode(OffsetData);
            m_ScannerTeach.ShowDialog();
        }

        private void BtnModelCamera_Click(object sender, EventArgs e)
        {
            m_CameraTeach = new FormCameraTeach();
            m_CameraTeach.SetDataMode(OffsetData);
            m_CameraTeach.ShowDialog();
        }
    }
}
