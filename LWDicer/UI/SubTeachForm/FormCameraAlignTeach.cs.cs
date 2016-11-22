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
using static LWDicer.Layers.DEF_Motion;
using static LWDicer.Layers.DEF_MeStage;
using static LWDicer.Layers.DEF_CtrlStage;

using LWDicer.Layers;

namespace LWDicer.UI
{
    public partial class FormCameraAlignTeach : Form
    {
        private CCtrlAlignData AlignData;
        private CSystemData_Align SystemAlignData;
        private CPos_XYTZ MacroRotateCenterPos = new CPos_XYTZ();
        private CPos_XYTZ MicroRotateCenterPos = new CPos_XYTZ();
        private CPos_XYTZ InspectRotateCenterPos = new CPos_XYTZ();
        public FormCameraAlignTeach()
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            // Local View 해제
            int iCam = CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam();
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(iCam);
            this.Close();
        }

        private void FormCameraAlignTeach_Load(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            //int iCam = CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam();
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);
            CMainFrame.LWDicer.m_Vision.ShowRectRoi();
#endif      
            
            // Align Data Copy
            AlignData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData.AlignData);

            // Motion Position Copy
            bool Type_Fixed = true;
            CPositionGroup tGroup;
            CMainFrame.LWDicer.GetPositionGroup(out tGroup, Type_Fixed);
            EPositionObject pIndex = EPositionObject.STAGE1;
            int nSelectedPos;

            nSelectedPos = (int)EStagePos.STAGE_CENTER_PRE;
            MacroRotateCenterPos = tGroup.Pos_Array[(int)pIndex].Pos[nSelectedPos].Copy();

            nSelectedPos = (int)EStagePos.STAGE_CENTER_FINE;
            MicroRotateCenterPos = tGroup.Pos_Array[(int)pIndex].Pos[nSelectedPos].Copy();

            // System Data(CamOffser) Copy
            SystemAlignData = ObjectExtensions.Copy(CMainFrame.DataManager.SystemData_Align);

            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        private void TimerUI_Tick(object sender, EventArgs e)
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
            else 
                CMainFrame.LWDicer.m_ctrlStage1.ChangeMicroVision(picVision.Handle, EVisionOverlayMode.EDGE);

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

            // 임시 적용
#if EQUIP_266_DEV
            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == INSP_CAM)
            {
                ratioMove = CMainFrame.DataManager.SystemData_Align.MicroScreenWidth / (double)picSize.Width;

                movePos.dX = (double)moveDistance.X * ratioMove;
                movePos.dY = -(double)moveDistance.Y * ratioMove;

                CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative(movePos);
            }
#endif
        }
        
        private void BtnJog_Click(object sender, EventArgs e)
        {
            CMainFrame.DisplayJog();
        }

        private void btnRotateCenter_Click(object sender, EventArgs e)
        {
            if(CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam()== PRE__CAM)
                CMainFrame.LWDicer.m_ctrlStage1.DoRotateCenterCals(ref MacroRotateCenterPos);
            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == FINE_CAM)
                CMainFrame.LWDicer.m_ctrlStage1.DoRotateCenterCals(ref MicroRotateCenterPos);
            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == INSP_CAM)
                CMainFrame.LWDicer.m_ctrlStage1.DoRotateCenterCals(ref InspectRotateCenterPos);
        }

        private void btnRotateCenterCalsInit_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.InitRotateCenterCals();
        }

        private void btnEdgeAlignDataSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save Edge Align data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            // Model Data Save
            CMainFrame.DataManager.ModelData.AlignData = ObjectExtensions.Copy(AlignData);
            CMainFrame.LWDicer.SaveModelData(CMainFrame.DataManager.ModelData);
                      
            // Position Data Save
            bool Type_Fixed = true;
            CPositionGroup tGroup;
            CMainFrame.LWDicer.GetPositionGroup(out tGroup, Type_Fixed);
            EPositionObject pIndex = EPositionObject.STAGE1;
            int nSelectedPos;

            nSelectedPos = (int)EStagePos.STAGE_CENTER_PRE;
            tGroup.Pos_Array[(int)pIndex].Pos[nSelectedPos] = MacroRotateCenterPos.Copy();

            nSelectedPos = (int)EStagePos.STAGE_CENTER_FINE;
            tGroup.Pos_Array[(int)pIndex].Pos[nSelectedPos] = MicroRotateCenterPos.Copy();

            nSelectedPos = (int)EStagePos.STAGE_CENTER_INSPECT;
            tGroup.Pos_Array[(int)pIndex].Pos[nSelectedPos] = InspectRotateCenterPos.Copy();

            CMainFrame.LWDicer.SavePosition(tGroup, Type_Fixed, pIndex);

            // System Data Save
            SystemAlignData.CamEachOffset.dX = MacroRotateCenterPos.dX - MicroRotateCenterPos.dX;
            SystemAlignData.CamEachOffset.dY = MacroRotateCenterPos.dY - MicroRotateCenterPos.dY;

            CMainFrame.LWDicer.SaveSystemData(null, null, null, null, SystemAlignData, null, null);

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

        private void btnThetaAlign_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.DoThetaAlign();
        }
        
    }
}
