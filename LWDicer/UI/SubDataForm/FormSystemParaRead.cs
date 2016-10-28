using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Syncfusion.Windows.Forms;

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

        private void BtnConfigureExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSystemParaRead_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Import data from excel?"))
            {
                return;
            }

            var dlg = new OpenFileDialog();
            dlg.InitialDirectory = CMainFrame.DataManager.DBInfo.SystemDir;
            dlg.Filter = "Excel Files|*.xlsx";
            dlg.Title = "Select a System Parameter File";

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            Button btn = sender as Button;
            EInfoExcel_Sheet sheet = EInfoExcel_Sheet.MAX;
            try
            {
                sheet = (EInfoExcel_Sheet)Enum.Parse(typeof(EInfoExcel_Sheet), btn.Tag.ToString());
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }

            try
            {
                int iResult = CMainFrame.DataManager.ImportDataFromExcel(dlg.FileName, sheet);
                CMainFrame.DisplayAlarm(iResult);
            }
            catch (System.Exception ex)
            {
                CMainFrame.DisplayMsg("Error occured when import parameter file");
                return;
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Export data to excel?"))
            {
                return;
            }

            Button btn = sender as Button;
            EInfoExcel_Sheet sheet = EInfoExcel_Sheet.MAX;
            try
            {
                sheet = (EInfoExcel_Sheet)Enum.Parse(typeof(EInfoExcel_Sheet), btn.Tag.ToString());
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }

            try
            {
                string strDefault = CMainFrame.DBInfo.GetExcelParaName_Default();
                string strNew = CMainFrame.DBInfo.GetExcelParaName_New();
                if(File.Exists(strNew) == false)
                {
                    if(File.Exists(strDefault) == false)
                    {
                        CMainFrame.DisplayMsg("Default parameter excel file is not exist");
                        return;
                    }
                    File.Copy(strDefault, strNew);
                }
                int iResult = CMainFrame.DataManager.ExportDataToExcel(strNew, sheet);
                CMainFrame.DisplayAlarm(iResult);
            }
            catch (System.Exception ex)
            {
                CMainFrame.DisplayMsg("Error occured when export parameter file");
                return;            	
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Delete all data in database?")) return;

            Button btn = sender as Button;
            int iResult = CMainFrame.DataManager.DeleteInfoTable(btn.Tag.ToString());
            //int iResult = CMainFrame.DataManager.DeleteInfoTable(CMainFrame.DataManager.DBInfo.TableParameter);
            CMainFrame.DisplayAlarm(iResult);
        }
    }
}
