using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Control.DEF_Scanner;
using static LWDicer.Control.DEF_System;

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
            string filepath = "config_job.ini";
            
            CMainFrame.LWDicer.m_PolyGonScanner.SendConfig(filepath);

            filepath = "csn_job.ini";
            CMainFrame.LWDicer.m_PolyGonScanner.SendTrueRaster(filepath);

            filepath = "isn_job.ini";
            CMainFrame.LWDicer.m_PolyGonScanner.SendTrueRaster(filepath);

            filepath = "reset.ini";
            CMainFrame.LWDicer.m_PolyGonScanner.SendTrueRaster(filepath);

            Hide();
        }

        private void btnConfigSelect_Click(object sender, EventArgs e)
        {
            string filepath = "config_job.ini";
            
            CMainFrame.LWDicer.m_PolyGonScanner.SendConfig(filepath);

            Hide();
        }

        private void btnCsnSelect_Click(object sender, EventArgs e)
        {
            string filepath = "csn_job.ini";
                
            CMainFrame.LWDicer.m_PolyGonScanner.SendTrueRaster(filepath);            

            filepath = "reset.ini";
            CMainFrame.LWDicer.m_PolyGonScanner.SendTrueRaster(filepath);

            Hide();
        }

        private void btnIsnSelect_Click(object sender, EventArgs e)
        {
            string filepath = "isn_job.ini";            
            
            CMainFrame.LWDicer.m_PolyGonScanner.SendTrueRaster(filepath);

            filepath = "reset.ini";
            CMainFrame.LWDicer.m_PolyGonScanner.SendTrueRaster(filepath);

            Hide();
        }

        private void btnEsc_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
