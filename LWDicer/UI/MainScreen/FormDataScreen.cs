using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Control;

using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

using Excel = Microsoft.Office.Interop.Excel;

using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormDataScreen : Form
    {

        private FormWaferData m_WaferDataForm;
        private FormModelData m_ModelDataForm;
        private FormScannerData m_ScannerDataForm;
        private FormCylinderData m_CylinderDataForm;
        private FormMotorData m_MotorDataForm;
        private FormVisionData m_VisionDataForm;
        private FormVacuumData m_VacuumDataForm;
        private FormSystemParaRead m_SystemParaReadForm;
        private FormSystemData m_SystemDataForm;


        public FormDataScreen()
        {
            InitializeComponent();

            InitializeForm();

            m_WaferDataForm = new FormWaferData();
            m_ModelDataForm = new FormModelData(EListHeaderType.MODEL);
            m_ScannerDataForm = new FormScannerData();
            m_CylinderDataForm = new FormCylinderData();
            m_MotorDataForm = new FormMotorData();
            m_VisionDataForm = new FormVisionData();
            m_VacuumDataForm = new FormVacuumData();
            m_SystemParaReadForm = new FormSystemParaRead();
            m_SystemDataForm = new FormSystemData();
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
            m_ScannerDataForm.ShowDialog();
        }

        private void BtnMotorData_Click(object sender, EventArgs e)
        {
            m_MotorDataForm.ShowDialog();
        }

        private void BtnCylinderData_Click(object sender, EventArgs e)
        {
            m_CylinderDataForm.ShowDialog();
        }

        private void BtnVisionData_Click(object sender, EventArgs e)
        {
            m_VisionDataForm.ShowDialog();
        }

        private void BtnModelData_Click(object sender, EventArgs e)
        {
            m_ModelDataForm.ShowDialog();
        }

        private void BtnWaferData_Click(object sender, EventArgs e)
        {
            m_WaferDataForm.ShowDialog();
        }

        private void BtnVacuum_Click(object sender, EventArgs e)
        {
            m_VacuumDataForm.ShowDialog();
        }

        private void BtnExcelLoad_Click(object sender, EventArgs e)
        {
            m_SystemParaReadForm.ShowDialog();
        }

        private void BtnSystemData_Click(object sender, EventArgs e)
        {
            m_SystemDataForm.ShowDialog();
        }
    }
}
