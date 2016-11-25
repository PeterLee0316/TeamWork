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
using static LWDicer.Layers.DEF_CtrlStage;


using LWDicer.Layers;

namespace LWDicer.UI
{
    public partial class FormWaferDieTeach : Form
    {
        private CSystemData_Align m_SystemData_Align;
        

        public FormWaferDieTeach()
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

            m_SystemData_Align = ObjectExtensions.Copy(CMainFrame.DataManager.SystemData_Align);

            CMainFrame.frmStageJog.Show();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {

            // Local View 해제
            int iCam = CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam();
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(iCam);
            this.Close();
        }

        private void FormThetaAlignTeach_Load(object sender, EventArgs e)
        {

           // int iCam = CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam();
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);
            CMainFrame.LWDicer.m_Vision.ShowHairLine();

            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();

            DisplayThetaAlignData();
            DisplayParameter();
        }

        private void FormThetaAlignTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void DisplayParameter()
        {
            lblDieIndexAxisX.Text = string.Format("{0:F4}", m_SystemData_Align.DieIndexWidth);
            lblDieIndexAxisY.Text = string.Format("{0:F4}", m_SystemData_Align.DieIndexHeight);
        }

        private void UpdateParameter()
        {

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

        
        private void btnChangeCam_Click(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == FINE_CAM)
                CMainFrame.LWDicer.m_ctrlStage1.ChangeMacroVision(picVision.Handle, EVisionOverlayMode.HAIR_LINE);
            else if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == PRE__CAM)
                CMainFrame.LWDicer.m_ctrlStage1.ChangeMicroVision(picVision.Handle, EVisionOverlayMode.HAIR_LINE);
            else
                CMainFrame.DisplayMsg("Cam not defined");

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
                ratioMove = m_SystemData_Align.MicroScreenWidth / (double)picSize.Width;

                movePos.dX = (double) moveDistance.X * ratioMove;
                movePos.dY = -(double) moveDistance.Y * ratioMove;

                CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative(movePos);
            }

            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == PRE__CAM)
            {
                ratioMove = m_SystemData_Align.MacroScreenWidth / (double)picSize.Width;

                movePos.dX = (double)moveDistance.X * ratioMove;
                movePos.dY = -(double)moveDistance.Y * ratioMove;

                CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative(movePos);
            }
        }
               

        private void TimerUI_Tick(object sender, EventArgs e)
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

#if EQUIP_266_DEV
                strShowData = String.Format("{0:0.0000}", CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.CAMERA1_Z].EncoderPos);
                lblCamPosZ.Text = strShowData;
#endif
            }
            catch
            { }

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
            m_SystemData_Align.DieIndexWidth  = Convert.ToDouble(lblDieIndexAxisX.Text);
            m_SystemData_Align.DieIndexHeight = Convert.ToDouble(lblDieIndexAxisY.Text);

            CMainFrame.LWDicer.SaveSystemData(null, null, null, null, m_SystemData_Align, null, null);
        }

        private void btnThetaAlignDataLoad_Click(object sender, EventArgs e)
        {
            DisplayThetaAlignData();
        }

        private void DisplayThetaAlignData()
        {           
            CMainFrame.LWDicer.m_ctrlStage1.ThetaAlignStepInit();
        }


        private void btnThetaAlignDataSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save Theta Align data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;
            

            // ThetaAlign Step 초기화
            CMainFrame.LWDicer.m_ctrlStage1.ThetaAlignStepInit();
        }
        
    }
}
