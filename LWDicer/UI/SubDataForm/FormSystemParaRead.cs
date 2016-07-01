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
            if (!CMainFrame.LWDicer.DisplayMsg("",15))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.PARA_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnAlarmInfoImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("",16))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.Alarm_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnIOInfoImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", 17))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.IO_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnMotorDataImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", 18))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.Motor_Data);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnParaInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", 19))
            {
                return;
            }

            int iResult = SUCCESS;
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnAlarmInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", 20))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ExportDataToExcel(EExcel_Sheet.Alarm_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnIOInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", 21))
            {
                return;
            }

            int iResult = SUCCESS;
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnMotorDataExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("",22))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ExportDataToExcel(EExcel_Sheet.Motor_Data);
            CMainFrame.DisplayAlarm(iResult);
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

        private void BtnMsgInfoImport_Click(object sender, EventArgs e)
        {
            //if (!CMainFrame.LWDicer.DisplayMsg("", 23))
            //{
            //    return;
            //}

            int iResult = CMainFrame.LWDicer.m_DataManager.ImportDataFromExcel(EExcel_Sheet.Message_Info);
            CMainFrame.LWDicer.AlarmDisplay(iResult);
        }

        private void BtnMsgInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", 24))
            {
                return;
            }

            int iResult = CMainFrame.LWDicer.m_DataManager.ExportDataToExcel(EExcel_Sheet.Message_Info);
            CMainFrame.LWDicer.AlarmDisplay(iResult);
        }
    }
}
