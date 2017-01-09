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
using Syncfusion.Windows.Forms.Grid;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_MeStage;
using static LWDicer.Layers.DEF_CtrlStage;
using static LWDicer.Layers.DEF_NST_Scanner;

using LWDicer.Layers;

namespace LWDicer.UI
{
    public partial class FormLaserAlignTeach : Form
    {
        private CSystemData_Align m_SystemData_Align;

        //private CPos_XYTZ PositionThetaAlignA = new CPos_XYTZ();
        //private CPos_XYTZ PositionThetaAlignTurnA = new CPos_XYTZ();

        private int m_InScanIndex = 0;
        private int m_CrossScanIndex = 0;

        private int m_BowScanIndex = 0; // current
        private int m_BowScanFacet = 0;
        private int m_BowScanIndex_Next = 0; // next
        private int m_BowScanFacet_Next = 0;

        private CScanCorrection m_ScanCorrection = new CScanCorrection();

        public FormLaserAlignTeach()
        {
            InitializeComponent();

            InitGrid_ScanCorrection();
            InitGrid_BowCorrection();

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

            CMainFrame.HideJog();
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
            
        }

        private void FormThetaAlignTeach_FormClosing(object sender, FormClosingEventArgs e)
        {

        }        

        private void InitGrid_ScanCorrection()
        {
            GridControl grid = GridCtrl_ScanCorrection;
            // Cell Click 시 커서가 생성되지 않게함.
            grid.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            grid.Rows.HeaderCount = 0;
            grid.Cols.HeaderCount = 0;

            grid.Properties.RowHeaders = true;
            grid.Properties.ColHeaders = true;

            int nCol = 10;
            int nRow = (int)EFacet.MAX;

            // Column,Row 개수
            grid.ColCount = nCol;
            grid.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                grid.ColWidths.SetSize(i, 80);
            }

            grid.ColWidths.SetSize(0, 80);

            for (int i = 0; i < nRow + 1; i++)
            {
                grid.RowHeights[i] = 24;
            }

            // Text Display
            grid[0, 0].Text = "Index";
            grid[0, 1].Text = "X1 Pos";
            grid[0, 2].Text = "X2 Pos";
            grid[0, 3].Text = "ISa Old";
            grid[0, 4].Text = "ISa New";
            grid[0, 5].Text = "Y1 Pos";
            grid[0, 6].Text = "CSa1 Old";
            grid[0, 7].Text = "CSa1 New";
            grid[0, 8].Text = "Y2 Pos";
            grid[0, 9].Text = "CSa2 Old";
            grid[0, 10].Text = "CSa2 New";

            for (int i = 0; i < (int)EFacet.MAX; i++)
            {
                grid[i+1, 0].Text = $"Facet {i}";
            }

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    grid[j, i].Font.Bold = true;

                    grid[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    grid[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (int i = 1; i < nCol + 1; i++)
            {
                for (int j = 3; j < nRow + 1; j++)
                {
                    //grid[j, i].BackColor = Color.FromArgb(220, 220, 255);
                }
            }

            grid.GridVisualStyles = GridVisualStyles.Office2007Blue;
            grid.ResizeColsBehavior = 0;
            grid.ResizeRowsBehavior = 0;

            // Grid Display Update
            grid.Refresh();
        }

        private void InitGrid_BowCorrection()
        {
            GridControl grid = GridCtrl_BowCorrection;
            // Cell Click 시 커서가 생성되지 않게함.
            grid.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            grid.Rows.HeaderCount = 0;
            grid.Cols.HeaderCount = 0;

            grid.Properties.RowHeaders = true;
            grid.Properties.ColHeaders = true;

            int nCol = 7;
            int nRow = (int)EFacet.MAX * MAX_BOW_CORRECTION;

            // Column,Row 개수
            grid.ColCount = nCol;
            grid.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                grid.ColWidths.SetSize(i, 80);
            }

            grid.ColWidths.SetSize(0, 80);

            for (int i = 0; i < nRow + 1; i++)
            {
                grid.RowHeights[i] = 24;
            }

            // Text Display
            grid[0, 0].Text = "Index";
            grid[0, 1].Text = "X1 Pos";
            grid[0, 2].Text = "X2 Pos";
            grid[0, 3].Text = "BowX Old";
            grid[0, 4].Text = "BowX New";
            grid[0, 5].Text = "Y1 Pos";
            grid[0, 6].Text = "BowY1 Old";
            grid[0, 7].Text = "BowY1 New";

            for (int i = 0; i < (int)EFacet.MAX; i++)
            {
                for (int j = 0; j < MAX_BOW_CORRECTION; j++)
                {
                    grid[i * MAX_BOW_CORRECTION + j + 1, 0].Text = $"Facet {i}_{j}";
                }
            }

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    grid[j, i].Font.Bold = true;

                    grid[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    grid[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (int i = 1; i < nCol + 1; i++)
            {
                for (int j = 3; j < nRow + 1; j++)
                {
                    //grid[j, i].BackColor = Color.FromArgb(220, 220, 255);
                }
            }

            grid.GridVisualStyles = GridVisualStyles.Office2007Blue;
            grid.ResizeColsBehavior = 0;
            grid.ResizeRowsBehavior = 0;

            // Grid Display Update
            grid.Refresh();
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

        private void btnHairLineWide_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.WidenHairLine();
        }

        private void btnHairLineNarrow_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.NarrowHairLine();
        }

        private void btnHairLineWideVertical_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.WidenHairLine(EHairLineType.VERTICAL);
        }

        private void btnHairLineNarrowVertical_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.NarrowHairLine(EHairLineType.VERTICAL);
        }

        private void btnCircleLineWide_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.WidenCircleRadius();
        }

        private void btnCircleLineNarrow_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.NarrowCircleRadius();
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
            

            strShowData = String.Format("{0:0.0000} mm", CMainFrame.LWDicer.m_ctrlStage1.GetHairLineWidth());
            lblHairLineWidth.Text = strShowData;

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
        
        private void btnInitLaserAlign_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.LaserAlignInit();
        }

        private void btnMoveLaserStartPos_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveLaserAlignPosA();
        }

        private void btnMoveLaserEndMove_Click(object sender, EventArgs e)
        {
            double dLenth = Convert.ToDouble(lblLaserLengthH.Text);

            CMainFrame.LWDicer.m_ctrlStage1.MoveLaserAlignPosB(dLenth);
        }

        private void lblLaserLength_Click(object sender, EventArgs e)
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

        private void btnMoveToLaser_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative((int)EStagePos.VISION_LASER_GAP);
        }

        private void btnMoveToVision_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative((int)EStagePos.VISION_LASER_GAP, false);
        }

        private async void btnLaserProcessStep1_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Laser Process Run ?")) return;

            int iResult = 0;
            CMainFrame.DataManager.ModelData.ProcData.ProcessStop = false;
            var task = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlStage1.RunLaserProcess());

            iResult = await task;
        }

        private void BtnJog_Click(object sender, EventArgs e)
        {
            CMainFrame.DisplayJog();
        }

        private void btnStageCenter_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveToStageLoadPos();
        }


        private void btnMoveLaserEndMoveV_Click(object sender, EventArgs e)
        {
            double dLenth = Convert.ToDouble(lblLaserLengthV.Text);

            CMainFrame.LWDicer.m_ctrlStage1.MoveLaserAlignHeightPosB(dLenth);
        }

        private void btnMoveLaserStartPosV_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveLaserAlignHeightPosA();
        }

        private void btnMoveToLaserH_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative((int)EStagePos.LASER_PROCESS_TURN);
        }

        private void btnMoveToVisionH_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative((int)EStagePos.LASER_PROCESS_TURN, false);
        }

        private void btnLaserProcessStop_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Laser Process Stop ?")) return;

            CMainFrame.DataManager.ModelData.ProcData.ProcessStop = true;

            CMainFrame.LWDicer.m_ctrlStage1.IsCancelJob_byManual = true;
        }

        private void btnInpectCam_Click(object sender, EventArgs e)
        {
#if EQUIP_266_DEV
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(PRE__CAM);
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(INSP_CAM, picVision.Handle);
            CMainFrame.LWDicer.m_Vision.LiveVideo(INSP_CAM);

            CMainFrame.LWDicer.m_MeStage.MoveCameraToFocusPosInspect();

            CMainFrame.LWDicer.m_Vision.ShowHairLine();
#endif
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void lblStagePosX_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void pageCalibration_Click(object sender, EventArgs e)
        {

        }

        private void lblLaserLengthV_Click(object sender, EventArgs e)
        {

        }

        private void LabelDefaultConfigFile_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            string filename = string.Empty;
            var dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = false;
            dlg.SelectedPath = CMainFrame.DBInfo.ScannerDataDir;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                data.Text = dlg.SelectedPath;
                data.ForeColor = Color.Red;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_InScanIndex = 0;
            UpdateIndexBtnText();
        }

        private void UpdateIndexBtnText()
        {
            btnSetInScan.Text = $"Set Pos to InScan [{m_InScanIndex}]";
            btnSetCrossScan.Text = $"Set Pos to CrossScan [{m_CrossScanIndex}]";
            btnSetBowScan.Text = $"Set Pos to BowPos [{m_BowScanFacet}_{m_BowScanIndex}]";

            labelBowFacetIndex.Text = $"{m_BowScanFacet}";
            m_BowScanFacet_Next = m_BowScanFacet;
            m_BowScanIndex_Next = m_BowScanIndex + 1;
            if (m_BowScanIndex_Next >= MAX_BOW_CORRECTION)
            {
                m_BowScanIndex_Next = 0;
                m_BowScanFacet_Next++;
            }
            if(m_BowScanFacet_Next >= (int)EFacet.MAX)
            {
                m_BowScanFacet_Next = 0;
            }
            btnMoveXBowNext.Text = $"Move Next [{m_BowScanFacet_Next}_{m_BowScanIndex_Next}]";
        }

        private void btnSetCrossScan_Click(object sender, EventArgs e)
        {
            GridControl grid = GridCtrl_ScanCorrection;

            // backup x pos for inscan later
            m_ScanCorrection.Facets_InScan[m_CrossScanIndex].Pos1 = Convert.ToDouble(lblStagePosX.Text);
            grid[m_CrossScanIndex + 1, 1].Text = $"{m_ScanCorrection.Facets_InScan[m_CrossScanIndex].Pos1}";

            m_ScanCorrection.CalcCrossScan1(m_CrossScanIndex, Convert.ToDouble(lblStagePosY.Text));

            int posIndex = 5;
            int corrIndex = 7;
            grid[m_CrossScanIndex + 1, posIndex].Text = $"{m_ScanCorrection.Facets_CrossScan1[m_CrossScanIndex].Pos1}";
            grid[m_CrossScanIndex + 1, corrIndex].Text = String.Format("{0:0.0000}", m_ScanCorrection.Facets_CrossScan1[m_CrossScanIndex].Correction_New);

            // calculate next index
            m_CrossScanIndex++;
            if (m_CrossScanIndex >= (int)EFacet.MAX) m_CrossScanIndex = 0;
            UpdateIndexBtnText();

        }

        private void labelInScanPitch_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";
            strCurrent = $"{m_ScanCorrection.ScanPitch_InScan}";

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            try
            {
                double d = Convert.ToDouble(strModify);
                m_ScanCorrection.ScanPitch_InScan = d;
                GradientLabel data = sender as GradientLabel;
                data.Text = $"{m_ScanCorrection.ScanPitch_InScan} mm";
            }
            catch
            {
            }

        }

        private void btnSetInScan_Click(object sender, EventArgs e)
        {
            GridControl grid = GridCtrl_ScanCorrection;

            // calculate inscan
            m_ScanCorrection.CalcInScan(m_InScanIndex, Convert.ToDouble(lblStagePosX.Text));

            int posIndex = 2;
            int corrIndex = 4;
            grid[m_InScanIndex + 1, posIndex].Text = $"{m_ScanCorrection.Facets_InScan[m_InScanIndex].Pos2}";
            grid[m_InScanIndex + 1, corrIndex].Text = String.Format("{0:0.0000}", m_ScanCorrection.Facets_InScan[m_InScanIndex].Correction_New);

            // calculate crossscan
            m_ScanCorrection.CalcCrossScan2(m_InScanIndex, Convert.ToDouble(lblStagePosY.Text));

            posIndex = 8;
            corrIndex = 10;
            grid[m_InScanIndex + 1, posIndex].Text = $"{m_ScanCorrection.Facets_CrossScan2[m_InScanIndex].Pos1}";
            grid[m_InScanIndex + 1, corrIndex].Text = String.Format("{0:0.0000}", m_ScanCorrection.Facets_CrossScan2[m_InScanIndex].Correction_New);

            // calculate next index
            m_InScanIndex++;
            if (m_InScanIndex >= (int)EFacet.MAX) m_InScanIndex = 0;
            UpdateIndexBtnText();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_CrossScanIndex = 0;
            UpdateIndexBtnText();
        }

        private void labelCrossScanPitch_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";
            strCurrent = $"{m_ScanCorrection.ScanPitch_CrossScan * 1000}";

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            try
            {
                double d = Convert.ToDouble(strModify);
                m_ScanCorrection.ScanPitch_CrossScan = (d == 0) ? 0 : Convert.ToDouble(strModify) / 1000;
                GradientLabel data = sender as GradientLabel;
                data.Text = $"{m_ScanCorrection.ScanPitch_CrossScan} mm";
            }
            catch
            {
            }
        }

        private void gradientLabel20_Click(object sender, EventArgs e)
        {

        }

        private void labelBowFacetIndex_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";
            strCurrent = $"{m_BowScanFacet}";

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            try
            {
                int n = Convert.ToInt32(strModify);
                if (n < 0 || n >= (int)EFacet.MAX) return;
                m_BowScanFacet = n;
            }
            catch
            {

            }

            GradientLabel data = sender as GradientLabel;
            data.Text = $"{m_BowScanFacet}";
            UpdateIndexBtnText();
        }

        private void btnSetBowScan_Click(object sender, EventArgs e)
        {
            GridControl grid = GridCtrl_BowCorrection;

            // calculate inscan
            double dx1;
            double dx2 = Convert.ToDouble(lblStagePosX.Text);
            double dy1 = Convert.ToDouble(lblStagePosY.Text);
            if (m_BowScanIndex == 0)
            {
                dx1 = dx2;
            }
            else
            {
                dx1 = m_ScanCorrection.Facets_BowScan[m_BowScanFacet, m_BowScanIndex - 1].X1 + m_ScanCorrection.ScanPitch_BowScan;
            }
            // calculate bowscan
            m_ScanCorrection.CalcBowScan(m_BowScanFacet, m_BowScanIndex, dx1, dx2, dy1);
            CBowCorrection bc = m_ScanCorrection.Facets_BowScan[m_BowScanFacet, m_BowScanIndex];

            int rowIndex = m_BowScanFacet * MAX_BOW_CORRECTION + m_BowScanIndex + 1;
            grid[rowIndex, 1].Text = String.Format("{0:0.0000}", bc.X1);
            grid[rowIndex, 2].Text = String.Format("{0:0.0000}", bc.X2);
            grid[rowIndex, 4].Text = String.Format("{0:0.0000}", bc.Correction_New.dX);
            grid[rowIndex, 5].Text = String.Format("{0:0.0000}", bc.Y1);
            grid[rowIndex, 7].Text = String.Format("{0:0.0000}", bc.Correction_New.dY);

            // bow correction은 move next에서 다음 index를 계산함.
        }

        private void btnResetBowIndex_Click(object sender, EventArgs e)
        {
            m_BowScanIndex = 0;
            UpdateIndexBtnText();
        }

        private void labelBowScanPitch_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";
            strCurrent = $"{m_ScanCorrection.ScanPitch_BowScan}";

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            try
            {
                double d = Convert.ToDouble(strModify);
                m_ScanCorrection.ScanPitch_BowScan = d;
                GradientLabel data = sender as GradientLabel;
                data.Text = $"{m_ScanCorrection.ScanPitch_BowScan} mm";
            }
            catch {
            }
        }

        private void btnMoveXBowNext_Click(object sender, EventArgs e)
        {
            CBowCorrection bc = m_ScanCorrection.Facets_BowScan[m_BowScanFacet, m_BowScanIndex];

            double dx;
            if(m_BowScanIndex_Next == 0) // 첫열로 돌아갈경우
            {
                if(m_BowScanFacet_Next == 0) // 0번 facet 즉, [0,0] 원점이면 check init
                {
                    if (m_ScanCorrection.Facets_BowScan[0, 0].IsInited == false)
                    {
                        CMainFrame.DisplayMsg("Current Point is not inited. Set current point first.");
                        return;
                    }
                    dx = m_ScanCorrection.Facets_BowScan[0, 0].X1;
                }
                else // 1번이하 facet
                {
                    if (m_ScanCorrection.Facets_BowScan[m_BowScanFacet_Next, 0].IsInited == false)
                    {
                        if (m_ScanCorrection.Facets_BowScan[0, 0].IsInited == false)
                        {
                            CMainFrame.DisplayMsg("Current Point is not inited. Set current point first.");
                            return;
                        }
                        dx = m_ScanCorrection.Facets_BowScan[0, 0].X1;
                    }
                    else
                    {
                        dx = m_ScanCorrection.Facets_BowScan[m_BowScanFacet_Next, 0].X1;
                    }
                }
            }
            else // 다음 열로 이동할 경우
            {
                if (bc.IsInited == false)
                {
                    CMainFrame.DisplayMsg("Current Point is not inited. Set current point first.");
                    return;
                }
                else
                {
                    dx = bc.X1 + m_ScanCorrection.ScanPitch_BowScan;
                }
            }

            int iResult;
            CPos_XYTZ curPos;
            CMainFrame.LWDicer.m_ctrlStage1.GetStagePos(out curPos);
            curPos.dX = dx;
            iResult = CMainFrame.LWDicer.m_MeStage.MoveStagePos(curPos);
            CMainFrame.DisplayAlarmOnly(iResult);

            // calculate next index
            m_BowScanIndex++;
            if (m_BowScanIndex >= MAX_BOW_CORRECTION)
            {
                m_BowScanIndex = 0;
                m_BowScanFacet++;
            }
            if (m_BowScanFacet >= (int)EFacet.MAX) m_BowScanFacet = 0;
            UpdateIndexBtnText();
        }
    }
}
