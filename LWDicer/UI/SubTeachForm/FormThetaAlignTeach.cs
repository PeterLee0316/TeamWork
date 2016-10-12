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
    public partial class pageAlign : Form
    {
        public pageAlign()
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
            TmrTeach.Enabled = false;

            CMainFrame.LWDicer.m_ctrlStage1.InitThetaAlign();

            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormThetaAlignTeach_Load(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);
            CMainFrame.LWDicer.m_Vision.ShowHairLine();
#endif

            TmrTeach.Enabled = true;
            TmrTeach.Interval = UITimerInterval;
            TmrTeach.Start();

            DisplayParameter();
        }

        private void FormThetaAlignTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void FormThetaAlignTeach_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void DisplayParameter()
        {
            lblCamOffSetAxisX.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.CamEachOffsetX);
            lblCamOffSetAxisY.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.CamEachOffsetY);

            lblMacroIndexAxisX.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.MacroScreenWidth);
            lblMacroIndexAxisY.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.MacroScreenHeight);
            lblMacroIndexAxisT.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.MacroScreenRotate);

            lblMicroIndexAxisX.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.MicroScreenWidth);
            lblMicroIndexAxisY.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.MicroScreenHeight);
            lblMicroIndexAxisT.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.MicroScreenRotate);

            lblDieIndexAxisX.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.DieIndexWidth);
            lblDieIndexAxisY.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.DieIndexHeight);
            lblDieIndexAxisT.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Align.DieIndexRotate);

            lblThetaAlignDistance.Text = string.Format("{0:F1}", CMainFrame.DataManager.SystemData_Align.AlignMarkWidthRatio);
        }

        private void UpdateParameter()
        {
            CMainFrame.DataManager.SystemData_Align.CamEachOffsetX = Convert.ToDouble(lblCamOffSetAxisX.Text);
            CMainFrame.DataManager.SystemData_Align.CamEachOffsetY = Convert.ToDouble(lblCamOffSetAxisY.Text);

            CMainFrame.DataManager.SystemData_Align.MacroScreenWidth  = Convert.ToDouble(lblMacroIndexAxisX.Text);
            CMainFrame.DataManager.SystemData_Align.MacroScreenHeight = Convert.ToDouble(lblMacroIndexAxisY.Text);
            CMainFrame.DataManager.SystemData_Align.MacroScreenRotate = Convert.ToDouble(lblMacroIndexAxisT.Text);

            CMainFrame.DataManager.SystemData_Align.MicroScreenWidth  = Convert.ToDouble(lblMicroIndexAxisX.Text);
            CMainFrame.DataManager.SystemData_Align.MicroScreenHeight = Convert.ToDouble(lblMicroIndexAxisY.Text);
            CMainFrame.DataManager.SystemData_Align.MicroScreenRotate = Convert.ToDouble(lblMicroIndexAxisT.Text);

            
            CMainFrame.DataManager.SystemData_Align.DieIndexRotate = Convert.ToDouble(lblDieIndexAxisT.Text);

            CMainFrame.DataManager.SystemData_Align.AlignMarkWidthRatio = Convert.ToDouble(lblThetaAlignDistance.Text);
            CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen = WAFER_SIZE_12_INCH * CMainFrame.DataManager.SystemData_Align.AlignMarkWidthRatio/100;

            // Stage Center는 Pre Cam 기준으로 정하고, 이후 나머지 위치는 Fine Cam 기준으로 저장한다
            // Fine Cam 기준으로 저장하기 위해 Offset 만큼 더해서 저장한다.
            // (Stage Center & Edge Align Position만 Pre Cam을 기준으로 사용)
            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.THETA_ALIGN_A].dX = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER].dX -
                                                                                             CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen/2 -
                                                                                             CMainFrame.DataManager.SystemData_Align.CamEachOffsetX;
            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.THETA_ALIGN_A].dY = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER].dY -
                                                                                             CMainFrame.DataManager.SystemData_Align.CamEachOffsetY;
            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.THETA_ALIGN_A].dT = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER].dT;

            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.THETA_ALIGN_TURN_A].dX = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER].dX -
                                                                                                  CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen / 2 -
                                                                                                  CMainFrame.DataManager.SystemData_Align.CamEachOffsetX;
            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.THETA_ALIGN_TURN_A].dY = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER].dY -
                                                                                                  CMainFrame.DataManager.SystemData_Align.CamEachOffsetY;
            CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.THETA_ALIGN_TURN_A].dT = CMainFrame.DataManager.FixedPos.Stage1Pos.Pos[(int)EStagePos.STAGE_CENTER].dT + 
                                                                                                  CMainFrame.DataManager.SystemData_Align.DieIndexRotate;


        }
        private void btnConfigSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Parameter 데이터를 저장하시겠습니까 ?")) return;
            
            UpdateParameter();
            
            CMainFrame.DataManager.SavePositionData(true, EPositionObject.STAGE1);
            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.STAGE1);

            CMainFrame.DataManager.SaveSystemData(null, null,null, null, CMainFrame.DataManager.SystemData_Align, null, null);
        }

        private void ChangeTextData(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            GradientLabel Btn = sender as GradientLabel;
            strCurrent = Btn.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            Btn.Text = strModify;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnChangeCam_Click(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == FINE_CAM)
                CMainFrame.LWDicer.m_ctrlStage1.ChangeMacroVision(picVision.Handle, EVisionOverlayMode.HAIR_LINE);
            else if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == PRE__CAM)
                CMainFrame.LWDicer.m_ctrlStage1.ChangeMicroVision(picVision.Handle, EVisionOverlayMode.HAIR_LINE);
            else
                CMainFrame.DisplayMsg("Cam not defined");

        }

        private void btnHairLineWide_Click(object sender, EventArgs e)
        {
#if SIMULATION_VISION
            return ;
#endif
            CMainFrame.LWDicer.m_Vision.WidenHairLine();
        }

        private void btnHairLineNarrow_Click(object sender, EventArgs e)
        {
#if SIMULATION_VISION
            return;
#endif
            CMainFrame.LWDicer.m_Vision.NarrowHairLine();
        }

        private void btnThetaAlign_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.DoThetaAlign();
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

                movePos.dX = (double) moveDistance.X * ratioMove;
                movePos.dY = -(double) moveDistance.Y * ratioMove;

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

        private void picVision_Paint(object sender, PaintEventArgs e)
        {

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

                strCurPos = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.CAMERA1_Z].EncoderPos);
                lblCamPosZ.Text = strCurPos;
            }
            catch
            { }
        }

        private void tabControlAdv1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSelectStageMove_Click(object sender, EventArgs e)
        {
            CMainFrame.frmCamFocus.Hide();
            CMainFrame.frmStageJog.Show();
        }

        private void btnSelectFocus_Click(object sender, EventArgs e)
        {
            CMainFrame.frmStageJog.Hide();
            CMainFrame.frmCamFocus.Show();
        }

        private void pageThetaAlign_Click(object sender, EventArgs e)
        {

        }

        private void btnStageTurn_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToStageTurn();
            CMainFrame.LWDicer.m_ctrlStage1.ThetaAlignStepInit();
        }
        private void btnStageReturn_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToStageReturn();
            CMainFrame.LWDicer.m_ctrlStage1.ThetaAlignStepInit();
        }

        private void btnDieIndexSet1_Click(object sender, EventArgs e)
        {
            string strCurPos = string.Empty;

            strCurPos = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_Y].EncoderPos);
            lblDieIndexSet1.Text = strCurPos;

        }

        private void btnDieIndexSet2_Click(object sender, EventArgs e)
        {
            string strCurPos = string.Empty;

            strCurPos = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_Y].EncoderPos);
            lblDieIndexSet2.Text = strCurPos;

        }
        private void btnCalsDieIndexSize_Click(object sender, EventArgs e)
        {
            double dPos1, dPos2, dDieSize;
            int iDieNum;

            dPos1 = Convert.ToDouble(lblDieIndexSet1.Text);
            dPos2 = Convert.ToDouble(lblDieIndexSet2.Text);
            iDieNum = Convert.ToInt32(lblDieIndexNum.Text);

            if (iDieNum < 0) return;

            dDieSize = Math.Abs(dPos1 - dPos2) / iDieNum;

            lblDieIndexCals.Text = String.Format("{0:0.0000}", dDieSize);
        }

        private void btnDieIndexDataSave_Click(object sender, EventArgs e)
        {
            CMainFrame.DataManager.SystemData_Align.DieIndexWidth  = Convert.ToDouble(lblDieIndexAxisX.Text);
            CMainFrame.DataManager.SystemData_Align.DieIndexHeight = Convert.ToDouble(lblDieIndexAxisY.Text);

            CMainFrame.DataManager.SaveSystemData(null, null, null, null, CMainFrame.DataManager.SystemData_Align, null, null);
        }

        private void btnThetaAlignDataLoad_Click(object sender, EventArgs e)
        {
            // 저장된 위치 데이터를 읽어온다.
            CMainFrame.DataManager.LoadPositionData(true, EPositionGroup.STAGE1);

            var posThetaAlignA = new CPos_XYTZ();
            var posThetaAlignTurnA = new CPos_XYTZ();

            CMainFrame.LWDicer.m_MeStage.GetThetaAlignPosA(out posThetaAlignA);
            CMainFrame.LWDicer.m_MeStage.GetThetaAlignTurnPosA(out posThetaAlignTurnA);

            lblThetaAlignPosX.Text = String.Format("{0:0.0000}", posThetaAlignA.dX);
            lblThetaAlignPosY.Text = String.Format("{0:0.0000}", posThetaAlignA.dY);
            lblThetaAlignPosT.Text = String.Format("{0:0.0000}", posThetaAlignA.dT);

            lblThetaAlignTurnPosX.Text = String.Format("{0:0.0000}", posThetaAlignTurnA.dX);
            lblThetaAlignTurnPosY.Text = String.Format("{0:0.0000}", posThetaAlignTurnA.dY);
            lblThetaAlignTurnPosT.Text = String.Format("{0:0.0000}", posThetaAlignTurnA.dT);

            CMainFrame.LWDicer.m_ctrlStage1.ThetaAlignStepInit();
        }

        private void btnThetaAlignDataSave_Click(object sender, EventArgs e)
        {
            CMainFrame.DataManager.SavePositionData(true, EPositionObject.STAGE1);

            CMainFrame.LWDicer.m_ctrlStage1.ThetaAlignStepInit();
        }

        private void btnThetaAlignDataApply_Click(object sender, EventArgs e)
        {
            var posThetaAlignA = new CPos_XYTZ();
            var posThetaAlignTurnA = new CPos_XYTZ();

            CMainFrame.LWDicer.m_MeStage.GetThetaAlignPosA(out posThetaAlignA);
            CMainFrame.LWDicer.m_MeStage.GetThetaAlignTurnPosA(out posThetaAlignTurnA);

            lblThetaAlignPosX.Text = String.Format("{0:0.0000}", posThetaAlignA.dX);
            lblThetaAlignPosY.Text = String.Format("{0:0.0000}", posThetaAlignA.dY);
            lblThetaAlignPosT.Text = String.Format("{0:0.0000}", posThetaAlignA.dT);

            lblThetaAlignTurnPosX.Text = String.Format("{0:0.0000}", posThetaAlignTurnA.dX);
            lblThetaAlignTurnPosY.Text = String.Format("{0:0.0000}", posThetaAlignTurnA.dY);
            lblThetaAlignTurnPosT.Text = String.Format("{0:0.0000}", posThetaAlignTurnA.dT);

            CMainFrame.LWDicer.m_ctrlStage1.ThetaAlignStepInit();

        }

        private void btnStageTurnPosA_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToThetaAlignPosA();            
        }

        private void btnStageReturnPosA_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToThetaAlignTurnPosA();
        }

        private void btnStageCenter_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToStageCenter();
        }

        private void btnRotateCenter_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.DoRotateCenterCals();
        }
    }
}
