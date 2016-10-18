using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;


using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_MeStage;

using LWDicer.Layers;

namespace LWDicer.UI
{
    public partial class FormMacroAlignTeach : Form
    {
        public FormMacroAlignTeach()
        {
            InitializeComponent();

            CMainFrame.frmStageJog.Location = new Point(0, 0);
            CMainFrame.frmStageJog.TopLevel = false;
            this.Controls.Add(CMainFrame.frmStageJog);
            CMainFrame.frmStageJog.Parent = this.pnlStageJog;
            CMainFrame.frmStageJog.Dock = DockStyle.Fill;
            CMainFrame.frmStageJog.Show();
        }

        private void FormClose()
        {
            this.Hide();
        }


        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormMacroAlignTeach_Load(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);
#endif
        }


       

        private void picVision_MouseDown(object sender, MouseEventArgs e)
        {
            Size picSize = picVision.Size;
            Point clickPos = e.Location;

            CMainFrame.LWDicer.m_ctrlStage1.ScreenClickMove(picSize, clickPos);

            return;

            Point centerPic = new Point(0, 0);
            Point moveDistance = new Point(0, 0);

            double ratioMove = 0.0;
            CPos_XYTZ movePos = new CPos_XYTZ();

            centerPic.X = picSize.Width / 2;
            centerPic.Y = picSize.Height / 2;

            moveDistance.X = centerPic.X - clickPos.X;
            moveDistance.Y = centerPic.Y - clickPos.Y;


            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == FINE_CAM)
            {
                ratioMove = CMainFrame.DataManager.SystemData_Align.MicroScreenWidth / (double)picSize.Width;

                movePos.dX = (double)moveDistance.X * ratioMove;
                movePos.dY = -(double)moveDistance.Y * ratioMove;

                CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative(movePos);
            }

            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == PRE__CAM)
            {
                ratioMove = CMainFrame.DataManager.SystemData_Align.MacroScreenWidth / (double)picSize.Width;

                movePos.dX = (double)moveDistance.X * ratioMove;
                movePos.dY = -(double)moveDistance.Y * ratioMove;

                CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative(movePos);
            }
        }
    }
}
