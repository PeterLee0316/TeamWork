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
    public partial class FormEdgeAlignTeach : Form
    {
        public FormEdgeAlignTeach()
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
            // Local View 해제
            int iCam = CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam();
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(iCam);
            
            this.Close();
        }


        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormEdgeAlignTeach_Load(object sender, EventArgs e)
        {
            //int iCam = CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam();
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);
            CMainFrame.LWDicer.m_Vision.ShowRectRoi();

            TmrTeach.Enabled = true;
            TmrTeach.Interval = UITimerInterval;
            TmrTeach.Start();
        }

        private void TmrTeach_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strCurPos = string.Empty;
            try
            {
                double dValue = 0, dCurPos = 0, dTargetPos = 0;

                strCurPos = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_X].EncoderPos);
                lblStagePosX.Text = strCurPos;

                strCurPos = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_Y].EncoderPos);
                lblStagePosY.Text = strCurPos;

                strCurPos = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_T].EncoderPos);
                lblStagePosT.Text = strCurPos;

#if EQUIP_266_DEV
                strCurPos = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.CAMERA1_Z].EncoderPos);
                lblCamPosZ.Text = strCurPos;
#endif
            }
            catch
            { }

            int nHairLineWidth =  CMainFrame.LWDicer.m_Vision.GetHairLineWidth();

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

        private void btnStageCenter_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToStageCenter();
        }
                
        private void btnThetaAlignDataSave_Click(object sender, EventArgs e)
        {
            double dLength = 0.0;

            dLength = WAFER_SIZE_12_INCH / 2.0 * Math.Cos(Math.PI / 180 * 45);

            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.EDGE_ALIGN_1].dX = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER_PRE].dX +
                                                                                            dLength;
            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.EDGE_ALIGN_1].dY = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER_PRE].dY -
                                                                                            dLength;
            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.EDGE_ALIGN_1].dT = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER_PRE].dT;



            CMainFrame.DataManager.SavePositionData(true, EPositionObject.STAGE1);
            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.STAGE1);
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void btnEdgeTeachPos1_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToEdgeAlignTeachPos1();
        }

        private void btnEdgeTeachPos2_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToEdgeAlignTeachPos2();
        }

        private void btnEdgeTeachPos3_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToEdgeAlignTeachPos3();
        }

        private void btnEdgePos1_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToEdgeAlignPos1();
        }

        private void btnEdgePos2_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToEdgeAlignPos2();            
        }

        private void btnEdgePos3_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToEdgeAlignPos3();            
        }

        private void btnEdgePos4_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToEdgeAlignPos4();
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

        private void btnSearchEdgePoint_Click(object sender, EventArgs e)
        {
            CPos_XY posEdge = new CPos_XY();
            CMainFrame.LWDicer.m_ctrlStage1.FindEdgePoint(out posEdge);

            return;
        }

        private void btnSetEdgeDetectArea_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.SetEdgeFinderArea(PRE__CAM);
        }

        

        private void btnEdgeTeachNext_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.SetEdgeTeachPosNext();
        }

        private void btnEdgeAlignDataInit_Click(object sender, EventArgs e)
        {
            double dLength = 0.0;

            dLength = WAFER_SIZE_12_INCH / 2.0 * Math.Cos(Math.PI / 180 * 45);

            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.EDGE_ALIGN_1].dX = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER_PRE].dX +
                                                                                            dLength;
            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.EDGE_ALIGN_1].dY = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER_PRE].dY -
                                                                                            dLength;
            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.EDGE_ALIGN_1].dT = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER_PRE].dT;

            // Wafer의 중심 Offset 적용
            CMainFrame.DataManager.SystemData_Align.WaferOffsetX = 0.0;
            CMainFrame.DataManager.SystemData_Align.WaferOffsetY = 0.0;
            CMainFrame.DataManager.SystemData_Align.WaferSizeOffset = 0.0;

            CMainFrame.DataManager.SavePositionData(true, EPositionObject.STAGE1);
            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.STAGE1);
        }

        private void BtnJog_Click(object sender, EventArgs e)
        {
            CMainFrame.DisplayJog();
        }

        private void btnRotateCenter_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.DoRotateCenterCals();
        }

        private void btnRotateCenterCalsInit_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.InitRotateCenterCals();
        }

        private void btnEdgeAlignDataSave_Click(object sender, EventArgs e)
        {
            CMainFrame.DataManager.SaveModelData(CMainFrame.DataManager.ModelData);

            CMainFrame.DataManager.SavePositionData(true, EPositionObject.STAGE1);
            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.STAGE1);
        }

        private void btnWaferCenterSearchRun_Click(object sender, EventArgs e)
        {
            // Pre Cam으로 변경
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);
            //CMainFrame.LWDicer.m_ctrlStage1.ChangeMacroVision(picVision.Handle, EVisionOverlayMode.EDGE);
            CMainFrame.LWDicer.m_Vision.SetEdgeFinderArea(PRE__CAM);            

            CMainFrame.LWDicer.m_ctrlStage1.DoEdgeAlign();
        }

        private void btnWaferCenterPre_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToWaferCenterPre();
        }

        private void btnWaferCenterFine_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToWaferCenterFine();
        }

        private void btnStageCenterPre_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToStageCenterPre();
        }

        private void btnStageCenterFine_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToStageCenterFine();
        }

        
    }
}
