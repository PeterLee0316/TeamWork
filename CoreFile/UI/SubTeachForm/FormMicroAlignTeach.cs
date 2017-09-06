using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Vision;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_DataManager;

namespace Core.UI
{
    public partial class FormMicroAlignTeach : Form
    {
        public FormMicroAlignTeach()
        {
            InitializeComponent();

            CMainFrame.frmStageJog.Location = new Point(0, 0);
            CMainFrame.frmStageJog.TopLevel = false;
            this.Controls.Add(CMainFrame.frmStageJog);
            CMainFrame.frmStageJog.Parent = this.pnlStageJog;
            CMainFrame.frmStageJog.Dock = DockStyle.Fill;
            CMainFrame.frmStageJog.Show();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMicroAlignTeach_Load(object sender, EventArgs e)
        {
            CMainFrame.Core.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);
            this.DesktopLocation = new Point(10, 128);
        }
    }
}
