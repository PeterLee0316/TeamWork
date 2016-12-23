using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_Scanner;
using static LWDicer.Layers.DEF_System;

namespace LWDicer.UI
{
    public partial class FormPolygonSelectPara : Form
    {
        public FormPolygonSelectPara()
        {
            InitializeComponent();
        }

        private void btnConfigSave_Click(object sender, EventArgs e)
        {

            string filename = string.Empty;
            OpenFileDialog iniOpenDlg = new OpenFileDialog();
            iniOpenDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            iniOpenDlg.Filter = "INI(*.ini)|*.ini";
            if (iniOpenDlg.ShowDialog() == DialogResult.OK)
            {
                filename = iniOpenDlg.FileName;
                CMainFrame.LWDicer.m_MeScanner.SendConfig(filename);
            }

            //string strFile;
            //string strPath;

            //strFile = "job.ini";
            //strPath = string.Format("{0:s}{1:s}", CMainFrame.LWDicer.m_DataManager.DBInfo.ScannerDataDir, strFile);
            //CMainFrame.LWDicer.m_MeScanner.SendConfig(strPath);

            //strFile = "csn_job.ini";
            //strPath = string.Format("{0:s}{1:s}", CMainFrame.LWDicer.m_DataManager.DBInfo.ScannerDataDir, strFile);           
            //CMainFrame.LWDicer.m_MeScanner.SendTrueRaster(strPath);
            
            //strFile = "isn_job.ini";
            //strPath = string.Format("{0:s}{1:s}", CMainFrame.LWDicer.m_DataManager.DBInfo.ScannerDataDir, strFile);
            //CMainFrame.LWDicer.m_MeScanner.SendTrueRaster(strPath);
            
            //strFile = "reset.ini";
            //strPath = string.Format("{0:s}{1:s}", CMainFrame.LWDicer.m_DataManager.DBInfo.ScannerDataDir, strFile);
            //CMainFrame.LWDicer.m_MeScanner.SendTrueRaster(strPath);

            Hide();
        }

        private void btnConfigSelect_Click(object sender, EventArgs e)
        {
            string strFile = "job.ini";
            string strPath = string.Format("{0:s}{1:s}", CMainFrame.LWDicer.m_DataManager.DBInfo.ScannerDataDir, strFile);              
            CMainFrame.LWDicer.m_MeScanner.SendConfig(strPath);

            Hide();
        }

        private void btnCsnSelect_Click(object sender, EventArgs e)
        {
            string strFile;
            string strPath;

            strFile = "csn_job.ini";
            strPath = string.Format("{0:s}{1:s}", CMainFrame.LWDicer.m_DataManager.DBInfo.ScannerDataDir, strFile);
            CMainFrame.LWDicer.m_MeScanner.SendTrueRaster(strPath);

            strFile = "reset.ini";
            strPath = string.Format("{0:s}{1:s}", CMainFrame.LWDicer.m_DataManager.DBInfo.ScannerDataDir, strFile);
            CMainFrame.LWDicer.m_MeScanner.SendTrueRaster(strPath);

            Hide();
        }

        private void btnIsnSelect_Click(object sender, EventArgs e)
        {
            string strFile;
            string strPath;

            strFile = "isn_job.ini";
            strPath = string.Format("{0:s}{1:s}", CMainFrame.LWDicer.m_DataManager.DBInfo.ScannerDataDir, strFile);
            CMainFrame.LWDicer.m_MeScanner.SendTrueRaster(strPath);

            strFile = "reset.ini";
            strPath = string.Format("{0:s}{1:s}", CMainFrame.LWDicer.m_DataManager.DBInfo.ScannerDataDir, strFile);
            CMainFrame.LWDicer.m_MeScanner.SendTrueRaster(strPath);

            Hide();
        }

        private void btnEsc_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
