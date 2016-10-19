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

            CMainFrame.frmCamFocus.Location = new Point(0, 0);
            CMainFrame.frmCamFocus.TopLevel = false;
            this.Controls.Add(CMainFrame.frmCamFocus);
            CMainFrame.frmCamFocus.Parent = this.pnlStageJog;
            CMainFrame.frmCamFocus.Dock = DockStyle.Fill;


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
            CMainFrame.LWDicer.m_Vision.ShowRectRoi();
#endif

            TmrTeach.Enabled = true;
            TmrTeach.Interval = UITimerInterval;
            TmrTeach.Start();
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

        private void TmrTeach_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strShowData = string.Empty;
            try
            {
                double dValue = 0, dCurPos = 0, dTargetPos = 0;

                strShowData = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_X].EncoderPos);
                lblStagePosX.Text = strShowData;

                strShowData = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_Y].EncoderPos);
                lblStagePosY.Text = strShowData;

                strShowData = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_T].EncoderPos);
                lblStagePosT.Text = strShowData;

                strShowData = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.CAMERA1_Z].EncoderPos);
                lblCamPosZ.Text = strShowData;
            }
            catch
            { }
            
            strShowData = String.Format("{0:0.0000} mm", CMainFrame.LWDicer.m_ctrlStage1.GetRoiSize().Width);
            lblRoiWidth.Text = strShowData;

            strShowData = String.Format("{0:0.0000} mm", CMainFrame.LWDicer.m_ctrlStage1.GetRoiSize().Height);
            lblRoiHeight.Text = strShowData;
        }

        private void btnChangeCam_Click(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == FINE_CAM)
                CMainFrame.LWDicer.m_ctrlStage1.ChangeMacroVision(picVision.Handle, EVisionOverlayMode.EDGE);
            else if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == PRE__CAM)
                CMainFrame.LWDicer.m_ctrlStage1.ChangeMicroVision(picVision.Handle, EVisionOverlayMode.EDGE);
            else
                CMainFrame.DisplayMsg("Cam not defined");
        }

        private void btnSelectFocus_Click(object sender, EventArgs e)
        {
            CMainFrame.frmStageJog.Hide();
            CMainFrame.frmCamFocus.Show();
        }

        private void btnSelectStageMove_Click(object sender, EventArgs e)
        {
            CMainFrame.frmCamFocus.Hide();
            CMainFrame.frmStageJog.Show();
        }

        private void btnRoiWidthWide_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.WidenRoiWidth();
        }

        private void btnRoiWidthNarrow_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.NarrowRoiWidth();
        }

        private void btnRoiHeightWide_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.WidenRoiHeight();
        }

        private void btnRoiHeightNarrow_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.NarrowRoiHeight();
        }

        private void btnRegisterMarkA_Click(object sender, EventArgs e)
        {
            var rectSearch = new Rectangle();
            var rectModel = new Rectangle();
            var sizeModel = new Size();
            string strModel = CMainFrame.DataManager.ModelData.Name;
            
            rectSearch.Width = CMainFrame.LWDicer.m_Vision.GetCameraPixelNum(PRE__CAM).Width;
            rectSearch.Height = CMainFrame.LWDicer.m_Vision.GetCameraPixelNum(PRE__CAM).Height;
            
            sizeModel = CMainFrame.LWDicer.m_Vision.GetRoiSize();
            rectModel.Width  = sizeModel.Width;
            rectModel.Height = sizeModel.Height;

            CMainFrame.LWDicer.m_Vision.RegisterPatternMark(PRE__CAM, strModel, PATTERN_A, rectSearch, rectModel);

            picPatternMarkA.Width = rectModel.Width/2;
            picPatternMarkA.Height = rectModel.Height/2;

            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(PRE__CAM, PATTERN_A, picPatternMarkA.Handle);
        }

        private void btnRegisterMarkB_Click(object sender, EventArgs e)
        {

        }

        private void btnSearchMarkA_Click(object sender, EventArgs e)
        {
            CResultData searchData = new CResultData();
            CMainFrame.LWDicer.m_Vision.RecognitionPatternMark(PRE__CAM, PATTERN_A, out searchData);
            lblSearchResult.Text = searchData.m_strResult;

            //CMainFrame.LWDicer.m_Vision.ShowRectRoi();
        }

        private void btnSearchMarkB_Click(object sender, EventArgs e)
        {

        }

        private void picPatternMarkA_Click(object sender, EventArgs e)
        {

        }
    }
}
