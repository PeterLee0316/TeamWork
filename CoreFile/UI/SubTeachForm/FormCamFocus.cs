using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.UI
{
    public partial class FormCamFocus : Form
    {
        public FormCamFocus()
        {
            InitializeComponent();
        }
        

        private void btnMoveTeachFocus_Click(object sender, EventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.MoveToCameraFocusPosFine();
        }

        private void btnMoveTeachFocus2_Click(object sender, EventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.MoveToCameraFocusPosInpect();
        }

        private void btnMoveTeachFocus3_Click(object sender, EventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.MoveToCameraFocusPos3();
        }

        private void btnIndexUpLow_MouseDown(object sender, MouseEventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.MoveCameraJog(false, false);
        }

        private void btnIndexUpLow_MouseUp(object sender, MouseEventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.CameraJogStop();
        }

        private void btnIndexDnLow_MouseDown(object sender, MouseEventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.MoveCameraJog(true, false);
        }

        private void btnIndexDnLow_MouseUp(object sender, MouseEventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.CameraJogStop();
        }

        private void btnIndexUpHigh_MouseDown(object sender, MouseEventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.MoveCameraJog(false, true);
        }

        private void btnIndexUpHigh_MouseUp(object sender, MouseEventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.CameraJogStop();
        }

        private void btnIndexDnHigh_MouseDown(object sender, MouseEventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.MoveCameraJog(true, true);
        }

        private void btnIndexDnHigh_MouseUp(object sender, MouseEventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.CameraJogStop();
        }

        private void btnIndexUpHigh_Click(object sender, EventArgs e)
        {

        }
    }
}
