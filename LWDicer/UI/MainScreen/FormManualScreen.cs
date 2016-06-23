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

        private FormIO m_IOScreen;
        private FormLimitSensor m_LimitSensor;
        private FormOriginReturn m_OriginReturn;
        private FormUnitInit m_UnitInit;

        private FormHandlerManualOP m_HandlerManualOP;
        private FormPushPullManualOP m_PushPullManualOP;
        private FormSpinnerManualOP m_SpinnerManualOP;
        private FormStageManualOP m_StageManualOP;


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
            m_IOScreen = new FormIO();
            m_IOScreen.ShowDialog();
        }

        private void BtnLimitSensor_Click(object sender, EventArgs e)
        {
            m_LimitSensor = new FormLimitSensor();
            m_LimitSensor.ShowDialog();
        }

        private void BtnOriginReturn_Click(object sender, EventArgs e)
        {
            m_OriginReturn = new FormOriginReturn();
            m_OriginReturn.ShowDialog();
        }

        private void BtnUnitInit_Click(object sender, EventArgs e)
        {
            m_UnitInit = new FormUnitInit();
            m_UnitInit.ShowDialog();
        }

        private void BtnManualPushPull_Click(object sender, EventArgs e)
        {
            m_PushPullManualOP = new FormPushPullManualOP();
            m_PushPullManualOP.ShowDialog();
        }

        private void BtnManualUpHandler_Click(object sender, EventArgs e)
        {
            m_HandlerManualOP = new FormHandlerManualOP();
            m_HandlerManualOP.SetHandler(UpHandler);
            m_HandlerManualOP.ShowDialog();
        }

        private void BtnManualLoHandler_Click(object sender, EventArgs e)
        {
            m_HandlerManualOP = new FormHandlerManualOP();
            m_HandlerManualOP.SetHandler(LoHandler);
            m_HandlerManualOP.ShowDialog();
        }

        private void BtnManualSpinner1_Click(object sender, EventArgs e)
        {
            m_SpinnerManualOP = new FormSpinnerManualOP();
            m_SpinnerManualOP.SetSpinner(Spinner1);
            m_SpinnerManualOP.ShowDialog();
        }

        private void BtnManualSpinner2_Click(object sender, EventArgs e)
        {
            m_SpinnerManualOP = new FormSpinnerManualOP();
            m_SpinnerManualOP.SetSpinner(Spinner2);
            m_SpinnerManualOP.ShowDialog();
        }

        private void BtnManualStage_Click(object sender, EventArgs e)
        {
            m_StageManualOP = new FormStageManualOP();
            m_StageManualOP.ShowDialog();
        }
    }
}
