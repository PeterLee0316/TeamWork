using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Error;

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
            if (!CMainFrame.InquireMsg("Import data from excel?"))
            {
                return;
            }

            int iResult = CMainFrame.DataManager.ImportDataFromExcel(EExcel_Sheet.PARA_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnAlarmInfoImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Import data from excel?"))
            {
                return;
            }

            int iResult = CMainFrame.DataManager.ImportDataFromExcel(EExcel_Sheet.Alarm_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnIOInfoImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Import data from excel?"))
            {
                return;
            }

            int iResult = CMainFrame.DataManager.ImportDataFromExcel(EExcel_Sheet.IO_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnMotorDataImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Import data from excel?"))
            {
                return;
            }

            int iResult = CMainFrame.DataManager.ImportDataFromExcel(EExcel_Sheet.Motor_Data);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnParaInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Export data to excel?"))
            {
                return;
            }

            int iResult = SUCCESS;
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnAlarmInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Export data to excel?"))
            {
                return;
            }

            int iResult = CMainFrame.DataManager.ExportDataToExcel(EExcel_Sheet.Alarm_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnIOInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Export data to excel?"))
            {
                return;
            }

            int iResult = SUCCESS;
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnMotorDataExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Export data to excel?"))
            {
                return;
            }

            int iResult = CMainFrame.DataManager.ExportDataToExcel(EExcel_Sheet.Motor_Data);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnConfigureExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSystemParaRead_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnMsgInfoImport_Click(object sender, EventArgs e)
        {
            //if (!CMainFrame.InquireMsg("Import data from excel?"))
            //{
            //    return;
            //}

            int iResult = CMainFrame.DataManager.ImportDataFromExcel(EExcel_Sheet.Message_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnMsgInfoExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Export data to excel?"))
            {
                return;
            }

            int iResult = CMainFrame.DataManager.ExportDataToExcel(EExcel_Sheet.Message_Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnMotorDataDelete_Click(object sender, EventArgs e)
        {
        }

        private void BtnIOInfoDelete_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Delete all data in database?")) return;

            int iResult = CMainFrame.DataManager.DeleteInfoTable(CMainFrame.DataManager.DBInfo.TableIO);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnAlarmInfoDelete_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Delete all data in database?")) return;

            int iResult = CMainFrame.DataManager.DeleteInfoTable(CMainFrame.DataManager.DBInfo.TableAlarmInfo);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnMsgInfoDelete_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Delete all data in database?")) return;

            int iResult = CMainFrame.DataManager.DeleteInfoTable(CMainFrame.DataManager.DBInfo.TableMessageInfo);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnParaInfoDelete_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Delete all data in database?")) return;

            int iResult = CMainFrame.DataManager.DeleteInfoTable(CMainFrame.DataManager.DBInfo.TableParameter);
            CMainFrame.DisplayAlarm(iResult);
        }
    }
}
