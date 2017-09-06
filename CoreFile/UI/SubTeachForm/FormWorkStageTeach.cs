using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Layers;
using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_Motion;
using static Core.Layers.MYaskawa;

using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_MeStage;

using static Core.Layers.DEF_Yaskawa;
using static Core.Layers.DEF_ACS;

using MotionYMC;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

namespace Core.UI
{
    public partial class FormWorkStageTeach : Form
    {
        ButtonAdv[] StagePos = new ButtonAdv[15]; // Max Teaching Position : 15

        private int m_nSelectedPos_Stage = 0;

        public bool Type_Fixed; // 고정좌표, 옵셋좌표 구분

        private CMovingObject MO_Stage = CMainFrame.Core.m_MeStage.AxStageInfo;

        public FormWorkStageTeach()
        {
            InitializeComponent();

            InitGrid();

            ResouceMapping();
        }

        private void FormWorkStageTeach_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(10, 128);

            this.Text = "Work Stage Part Teaching Screen";

            UpdateStageTeachPos(0);

            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        private void FormWorkStageTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            CMainFrame.HideJog();
            this.Close();
        }

        private void BtnJog_Click(object sender, EventArgs e)
        {
            CMainFrame.DisplayJog();
        }

        private void InitGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridStageTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridStageTeachTable.Properties.RowHeaders = true;
            GridStageTeachTable.Properties.ColHeaders = false;

            int nCol = 3;
            int nRow = 8;

            // Column,Row 개수
            GridStageTeachTable.ColCount = nCol;
            GridStageTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                GridStageTeachTable.ColWidths.SetSize(i, 160);
            }

            GridStageTeachTable.ColWidths.SetSize(0, 110);

            for (int i = 0; i < nRow + 1; i++)
            {
                GridStageTeachTable.RowHeights[i] = 40;
            }

            // Text Display
            GridStageTeachTable[1, 1].CellType = GridCellTypeName.PushButton;
            GridStageTeachTable[1, 2].CellType = GridCellTypeName.PushButton;
            GridStageTeachTable[1, 3].CellType = GridCellTypeName.PushButton;

            GridStageTeachTable[1, 1].CellAppearance = GridCellAppearance.Raised;
            GridStageTeachTable[1, 2].CellAppearance = GridCellAppearance.Raised;
            GridStageTeachTable[1, 3].CellAppearance = GridCellAppearance.Raised;

            GridStageTeachTable[1, 1].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);
            GridStageTeachTable[1, 2].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);
            GridStageTeachTable[1, 3].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);

            GridStageTeachTable[2, 0].Text = "목표 위치";
            GridStageTeachTable[3, 0].Text = "고정 좌표";
            GridStageTeachTable[4, 0].Text = "모델 좌표";
            GridStageTeachTable[5, 0].Text = "Cell Mark 보정";
            GridStageTeachTable[6, 0].Text = "Offset 좌표";
            GridStageTeachTable[7, 0].Text = "현재 위치";
            GridStageTeachTable[8, 0].Text = "보정값";

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridStageTeachTable[j, i].Font.Bold = true;

                    GridStageTeachTable[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridStageTeachTable[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    if (i != 0 && j != 0)
                    {
                        GridStageTeachTable[j, i].Text = "";
                        GridStageTeachTable[j, i].TextColor = Color.Black;
                    }
                }
            }

            GridStageTeachTable.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridStageTeachTable.ResizeColsBehavior = 0;
            GridStageTeachTable.ResizeRowsBehavior = 0;

            for (int i = 0; i < nCol; i++)
            {
                GridStageTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridStageTeachTable[1, i + 1].Description = "";
            }

            GridStageTeachTable[1, 0].Text = Convert.ToString(ETeachUnit.STAGE);

            GridStageTeachTable[1, 1].Description = "X Axis";
            GridStageTeachTable[1, 1].TextColor = Color.DarkRed;

            GridStageTeachTable[1, 2].Description = "Y Axis";
            GridStageTeachTable[1, 2].TextColor = Color.DarkRed;

            GridStageTeachTable[1, 3].Description = "T Axis";
            GridStageTeachTable[1, 3].TextColor = Color.DarkRed;

            // Grid Display Update
            GridStageTeachTable.Refresh();
        }

        private void ResouceMapping()
        {
            StagePos[0] = BtnPos1; StagePos[1] = BtnPos2; StagePos[2] = BtnPos3; StagePos[3] = BtnPos4; StagePos[4] = BtnPos5;
            StagePos[5] = BtnPos6; StagePos[6] = BtnPos7; StagePos[7] = BtnPos8; StagePos[8] = BtnPos9; StagePos[9] = BtnPos10;
            StagePos[10] = BtnPos11; StagePos[11] = BtnPos12; StagePos[12] = BtnPos13; StagePos[13] = BtnPos14; StagePos[14] = BtnPos15;

            for (int i = 0; i < 15; i++)
            {
                StagePos[i].Visible = false;
            }
            
            for (int i = 0; i < (int)EStagePos.MAX; i++)
            {
                StagePos[i].Text = Convert.ToString(EStagePos.WAIT + i);
            }
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strCurPos = string.Empty;
            try
            {
                double dValue = 0, dCurPos = 0, dTargetPos = 0;

                strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_X].EncoderPos);
                GridStageTeachTable[7, 1].Text = strCurPos;

                strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_Y].EncoderPos);
                GridStageTeachTable[7, 2].Text = strCurPos;

                strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_T].EncoderPos);
                GridStageTeachTable[7, 3].Text = strCurPos;

                // 보정값 Display
                dCurPos = CMainFrame.Core.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_X].EncoderPos;
                dTargetPos = Convert.ToDouble(GridStageTeachTable[2, 1].Text);
                dValue = dTargetPos - dCurPos;
                GridStageTeachTable[8, 1].Text= String.Format("{0:0.0000}", dValue);

                dCurPos = CMainFrame.Core.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_Y].EncoderPos;
                dTargetPos = Convert.ToDouble(GridStageTeachTable[2, 2].Text);
                dValue = dTargetPos - dCurPos;
                GridStageTeachTable[8, 2].Text= String.Format("{0:0.0000}", dValue);

                dCurPos = CMainFrame.Core.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_T].EncoderPos;
                dTargetPos = Convert.ToDouble(GridStageTeachTable[2, 3].Text);
                dValue = dTargetPos - dCurPos;
                GridStageTeachTable[8, 3].Text= String.Format("{0:0.0000}", dValue);
            }
            catch
            { }
        }

        private void UpdateStageTeachPos(int selectedPos)
        {
            int nCount = (int)EStagePos.MAX;

            for (int i = 0; i < nCount; i++)
            {
                StagePos[i].BackColor = Color.LightYellow;

                StagePos[i].Visible = true;
            }

            for (int i = 0; i < GridStageTeachTable.ColCount + 1; i++)
            {
                for (int j = 0; j < GridStageTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridStageTeachTable[j, i].TextColor = Color.Black;
                        GridStageTeachTable[j, i].Text = "";
                    }
                }

                if (Type_Fixed == true)
                {
                    if (i != 0) GridStageTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridStageTeachTable[6, i].BackColor = Color.White;
                }
                else
                {
                    if (i != 0) GridStageTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridStageTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            StagePos[selectedPos].BackColor = Color.Tan;

            m_nSelectedPos_Stage = selectedPos;

            DisplayPos_Stage();
        }
        
        private void DisplayPos_Stage()
        {
            if (MO_Stage.Pos_Fixed.Pos.Length <= m_nSelectedPos_Stage) return;

            int index = m_nSelectedPos_Stage;
            double dFixedPos, dModelPos, dOffsetPos, dAlignOffset, dTargetPos;
            int direction = DEF_X;
            dTargetPos = MO_Stage.GetPosition(index, direction, out dFixedPos, out dModelPos, out dOffsetPos, out dAlignOffset);

            GridStageTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos);
            GridStageTeachTable[3, 1].Text = String.Format("{0:0.0000}", dFixedPos);
            GridStageTeachTable[4, 1].Text = String.Format("{0:0.0000}", dModelPos);
            GridStageTeachTable[5, 1].Text = String.Format("{0:0.0000}", dModelPos);
            GridStageTeachTable[6, 1].Text = String.Format("{0:0.0000}", dOffsetPos);

            direction = DEF_Y;
            dTargetPos = MO_Stage.GetPosition(index, direction, out dFixedPos, out dModelPos, out dOffsetPos, out dAlignOffset);

            GridStageTeachTable[2, 2].Text = String.Format("{0:0.0000}", dTargetPos);
            GridStageTeachTable[3, 2].Text = String.Format("{0:0.0000}", dFixedPos);
            GridStageTeachTable[4, 2].Text = String.Format("{0:0.0000}", dModelPos);
            GridStageTeachTable[5, 2].Text = String.Format("{0:0.0000}", dModelPos);
            GridStageTeachTable[6, 2].Text = String.Format("{0:0.0000}", dOffsetPos);

            direction = DEF_T;
            dTargetPos = MO_Stage.GetPosition(index, direction, out dFixedPos, out dModelPos, out dOffsetPos, out dAlignOffset);

            GridStageTeachTable[2, 3].Text = String.Format("{0:0.0000}", dTargetPos);
            GridStageTeachTable[3, 3].Text = String.Format("{0:0.0000}", dFixedPos);
            GridStageTeachTable[4, 3].Text = String.Format("{0:0.0000}", dModelPos);
            GridStageTeachTable[5, 3].Text = String.Format("{0:0.0000}", dModelPos);
            GridStageTeachTable[6, 3].Text = String.Format("{0:0.0000}", dOffsetPos);
        }

        private void GridStageTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";
            double dPos = 0, dOffsetPos = 0, dTargetPos = 0;

            if (Type_Fixed == true)
            {
                strCurrent = GridStageTeachTable[3, e.ColIndex].Text;

                if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
                {
                    return;
                }

                dPos = Convert.ToDouble(strModify);

                if (e.ColIndex == 1)
                {
                    dOffsetPos = MO_Stage.Pos_Offset.Pos[m_nSelectedPos_Stage].dX;

                    dTargetPos = dPos + dOffsetPos;
                }

                if (e.ColIndex == 2)
                {
                    dOffsetPos = MO_Stage.Pos_Offset.Pos[m_nSelectedPos_Stage].dY;

                    dTargetPos = dPos + dOffsetPos;
                }

                if (e.ColIndex == 3)
                {
                    dOffsetPos = MO_Stage.Pos_Offset.Pos[m_nSelectedPos_Stage].dT;

                    dTargetPos = dPos + dOffsetPos;
                }

                GridStageTeachTable[2, e.ColIndex].Text = String.Format("{0:0.0000}", dTargetPos);

                GridStageTeachTable[3, e.ColIndex].Text = strModify;
                GridStageTeachTable[3, e.ColIndex].TextColor = Color.Blue;
            }
            else
            {
                strCurrent = GridStageTeachTable[6, e.ColIndex].Text;

                if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
                {
                    return;
                }

                GridStageTeachTable[6, e.ColIndex].Text = strModify;
                GridStageTeachTable[6, e.ColIndex].TextColor = Color.Blue;
            }
        }

        private void BtnStageSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save teaching data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            CPositionGroup tGroup;
            CMainFrame.Core.GetPositionGroup(out tGroup, Type_Fixed);
            EPositionObject pIndex = EPositionObject.STAGE1;
            int direction = DEF_X;
            strData = (Type_Fixed == true) ? GridStageTeachTable[3, 1].Text : strData = GridStageTeachTable[6, 1].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_Stage].SetPosition(direction, Convert.ToDouble(strData));

            direction = DEF_Y;
            strData = (Type_Fixed == true) ? GridStageTeachTable[3, 2].Text : strData = GridStageTeachTable[6, 2].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_Stage].SetPosition(direction, Convert.ToDouble(strData));

            direction = DEF_T;
            strData = (Type_Fixed == true) ? GridStageTeachTable[3, 3].Text : strData = GridStageTeachTable[6, 3].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_Stage].SetPosition(direction, Convert.ToDouble(strData));

            CMainFrame.Core.SavePosition(tGroup, Type_Fixed, pIndex);
            DisplayPos_Stage();
        }

        private void BtnStageChangeValue_Click(object sender, EventArgs e)
        {
            string StrXCurrent = "", StrYCurrent = "", StrTCurrent = "";
            double dXPos = 0, dOffsetXPos = 0, dTargetXPos = 0;
            double dYPos = 0, dOffsetYPos = 0, dTargetYPos = 0;
            double dTPos = 0, dOffsetTPos = 0, dTargetTPos = 0;

            string strMsg = "Change target position to current position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            StrXCurrent = GridStageTeachTable[7, 1].Text;

            dXPos = Convert.ToDouble(StrXCurrent);
            dOffsetXPos = MO_Stage.Pos_Offset.Pos[m_nSelectedPos_Stage].dX;

            dTargetXPos = dXPos + dOffsetXPos;

            GridStageTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetXPos);
            GridStageTeachTable[3, 1].Text = String.Format("{0:0.0000}", dXPos);
            GridStageTeachTable[3, 1].TextColor = Color.Blue;
            
            StrYCurrent = GridStageTeachTable[7, 2].Text;

            dYPos = Convert.ToDouble(StrYCurrent);
            dOffsetYPos = MO_Stage.Pos_Offset.Pos[m_nSelectedPos_Stage].dY;

            dTargetYPos = dYPos + dOffsetYPos;

            GridStageTeachTable[2, 2].Text = String.Format("{0:0.0000}", dTargetYPos);
            GridStageTeachTable[3, 2].Text = String.Format("{0:0.0000}", dYPos);
            GridStageTeachTable[3, 2].TextColor = Color.Blue;

            StrTCurrent = GridStageTeachTable[7, 3].Text;

            dTPos = Convert.ToDouble(StrTCurrent);
            dOffsetTPos = MO_Stage.Pos_Offset.Pos[m_nSelectedPos_Stage].dT;

            dTargetTPos = dTPos + dOffsetTPos;

            GridStageTeachTable[2, 3].Text = String.Format("{0:0.0000}", dTargetTPos);
            GridStageTeachTable[3, 3].Text = String.Format("{0:0.0000}", dTPos);
            GridStageTeachTable[3, 3].TextColor = Color.Blue;

        }

        private void BtnPos_Click(object sender, EventArgs e)
        {
            ButtonAdv TeachPos = sender as ButtonAdv;

            UpdateStageTeachPos(Convert.ToInt16(TeachPos.Tag));
        }

        private void BtnStageTeachMove_Click(object sender, EventArgs e)
        {
            string strMsg = "Move to selected position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.WAIT))              CMainFrame.Core.m_ctrlStage1.MoveToStageWaitPos();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.LOAD))              CMainFrame.Core.m_ctrlStage1.MoveToStageLoadPos();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.UNLOAD))            CMainFrame.Core.m_ctrlStage1.MoveToStageUnloadPos();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.STAGE_CENTER_PRE))  CMainFrame.Core.m_ctrlStage1.MoveToStageCenterPre();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.STAGE_CENTER_FINE)) CMainFrame.Core.m_ctrlStage1.MoveToStageCenterFine();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.STAGE_CENTER_INSPECT))     CMainFrame.Core.m_ctrlStage1.MoveToStageCenterInspect();

            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.EDGE_ALIGN_1))      CMainFrame.Core.m_ctrlStage1.MoveToEdgeAlignPos1();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.MACRO_CAM_POS))     CMainFrame.Core.m_ctrlStage1.MoveToMacroCam();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.MACRO_ALIGN))       CMainFrame.Core.m_ctrlStage1.MoveToMacroAlignA();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.MICRO_ALIGN))       CMainFrame.Core.m_ctrlStage1.MoveToMicroAlignA();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.MICRO_ALIGN_TURN))  CMainFrame.Core.m_ctrlStage1.MoveToMicroAlignTurnA();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.LASER_PROCESS))     CMainFrame.Core.m_ctrlStage1.MoveToProcessPos();
            if (StagePos[m_nSelectedPos_Stage].Text == Convert.ToString(EStagePos.LASER_PROCESS_TURN)) CMainFrame.Core.m_ctrlStage1.MoveToProcessTurnPos();

        }

        private void BtnManualOP_Click(object sender, EventArgs e)
        {
            var dlg = new FormStageManualOP();
            dlg.ShowDialog();
        }

        private void btnMoveToLaser_Click(object sender, EventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.MoveStageRelative((int)EStagePos.VISION_LASER_GAP);
        }

        private void btnMoveToVision_Click(object sender, EventArgs e)
        {
            CMainFrame.Core.m_ctrlStage1.MoveStageRelative((int)EStagePos.VISION_LASER_GAP,false);
        }
    }
}
