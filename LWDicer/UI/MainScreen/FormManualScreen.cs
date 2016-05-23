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

        private FormIO m_IOScreen;
        private FormLimitSensor m_LimitSensor;
        private FormOriginReturn m_OriginReturn;
        private FormUnitInit m_UnitInit;

        public FormManualScreen()
        {
            InitializeComponent();

            InitializeForm();

            m_IOScreen = new FormIO();
            m_LimitSensor = new FormLimitSensor();
            m_OriginReturn = new FormOriginReturn();
            m_UnitInit = new FormUnitInit();

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
            m_IOScreen.ShowDialog();
        }

        private void BtnLimitSensor_Click(object sender, EventArgs e)
        {
            m_LimitSensor.ShowDialog();
        }

        private void BtnOriginReturn_Click(object sender, EventArgs e)
        {
            m_OriginReturn.ShowDialog();
        }

        private void BtnUnitInit_Click(object sender, EventArgs e)
        {
            m_UnitInit.ShowDialog();
        }
    }
}
