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
using static LWDicer.Control.DEF_Error;

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
            if (!CMainFrame.LWDicer.DisplayMsg("", "Parameter Data를 Import 하시겠습니까?"))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.PARA_Info);
            CMainFrame.LWDicer.AlarmDisplay(iResult);
        }

        private void BtnAlarmInfoImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "Alarm Info Data를 Import 하시겠습니까?"))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.Alarm_Info);
            CMainFrame.LWDicer.AlarmDisplay(iResult);
        }

        private void BtnIOInfoImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "I/O Info Data를 Import 하시겠습니까?"))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.IO_Info);
            CMainFrame.LWDicer.AlarmDisplay(iResult);
        }

        private void BtnMotorDataImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "Motor Data를 Import 하시겠습니까?"))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.Motor_Data);
            CMainFrame.LWDicer.AlarmDisplay(iResult);
        }

        private void BtnParaInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "Parameter Data를 Export 하시겠습니까?"))
            {
                return;
            }

            int iResult = SUCCESS;
            CMainFrame.LWDicer.AlarmDisplay(iResult);
        }

        private void BtnAlarmInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "Alarm Info Data를 Export 하시겠습니까?"))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ExportDataToExcel(EExcel_Sheet.Alarm_Info);
            CMainFrame.LWDicer.AlarmDisplay(iResult);
        }

        private void BtnIOInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "I/O Info Data를 Export 하시겠습니까?"))
            {
                return;
            }

            int iResult = SUCCESS;
            CMainFrame.LWDicer.AlarmDisplay(iResult);
        }

        private void BtnMotorDataExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "Motor Data를 Export 하시겠습니까?"))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ExportDataToExcel(EExcel_Sheet.Motor_Data);
            CMainFrame.LWDicer.AlarmDisplay(iResult);
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
