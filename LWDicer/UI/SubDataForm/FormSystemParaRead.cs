using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Control.DEF_System;

namespace LWDicer.UI
{
    public partial class FormSystemParaRead : Form
    {
        public FormSystemParaRead()
        {
            InitializeComponent();
        }

        private void BtnParaInfoImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("Parameter Data를 Loading 하시겠습니까?"))
            {
                return;
            }

            CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.PARA_Info);
        }

        private void BtnAlarmInfoImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("Alarm Info Data를 Loading 하시겠습니까?"))
            {
                return;
            }

            CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.Alarm_Info);
        }

        private void BtnIOInfoImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("I/O Info Data를 Loading 하시겠습니까?"))
            {
                return;
            }

            CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.IO_Info);
        }

        private void BtnMotorDataImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("Motor Data를 Loading 하시겠습니까?"))
            {
                return;
            }

            CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.Motor_Data);

            CMainFrame.LWDicer.m_DataManager.SaveSystemData(null, CMainFrame.LWDicer.m_DataManager.SystemData_Axis, null,null,null);
        }

        private void BtnParaInfoExport_Click(object sender, EventArgs e)
        {

        }

        private void BtnAlarmInfoExport_Click(object sender, EventArgs e)
        {

        }

        private void BtnIOInfoExport_Click(object sender, EventArgs e)
        {

        }

        private void BtnMotorDataExport_Click(object sender, EventArgs e)
        {

        }

        private void BtnConfigureExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void FormSystemParaRead_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }
    }
}
