using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LWDicer.Control;

using static LWDicer.Control.MYaskawa;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_MeHandler;
using static LWDicer.Control.DEF_MeElevator;
using static LWDicer.Control.DEF_MePushPull;
using static LWDicer.Control.DEF_MeSpinner;
using static LWDicer.Control.DEF_MeStage;

using static LWDicer.Control.DEF_System;

using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_ACS;

using MotionYMC;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

namespace LWDicer.UI
{
    public partial class FormSpinner2Teach : Form
    {
        const int Spinner1 = 0;
        const int Spinner2 = 1;

        ButtonAdv[] RotatePos = new ButtonAdv[15];
        ButtonAdv[] NozzlePos = new ButtonAdv[15];

        private int m_nSelectedPos_Rotate = 0;
        private int m_nSelectedPos_Nozzle = 0;

        private int nDataMode = 0;

        private CMovingObject movingCleanObject = CMainFrame.LWDicer.m_MeSpinner2.AxCleanNozzleInfo;
        private CMovingObject movingCoatObject = CMainFrame.LWDicer.m_MeSpinner2.AxCoatNozzleInfo;
        private CMovingObject movingRotateObject = CMainFrame.LWDicer.m_MeSpinner2.AxRotateInfo;

        public FormSpinner2Teach()
        {
            InitializeComponent();

            InitNozzleGrid();
            InitRotateGrid();

            ResouceMapping();
        }

        private void FormClose()
        {
            TmrTeach.Stop();
            this.Hide();
        }

        private void FormSpinner2Teach_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);

            this.Text = "Spinner 2 Part Teaching Screen";

            UpdateRotateTeachPos(0);
            UpdateNozzleTeachPos(0);

            TmrTeach.Enabled = true;
            TmrTeach.Interval = 100;
            TmrTeach.Start();
        }

        private void FormSpinner2Teach_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        public void SetDataMode(int nMode)
        {
            nDataMode = nMode;
        }

        public int GetDataMode()
        {
            return nDataMode;
        }

        private void BtnJog_Click(object sender, EventArgs e)
        {
            CMainFrame.DisplayJog();
        }

        private void BtnRotatePos_Click(object sender, EventArgs e)
        {
            ButtonAdv TeachPos = sender as ButtonAdv;

            UpdateRotateTeachPos(Convert.ToInt16(TeachPos.Tag));
        }

        private void BtnNozzlePos_Click(object sender, EventArgs e)
        {
            ButtonAdv TeachPos = sender as ButtonAdv;

            UpdateNozzleTeachPos(Convert.ToInt16(TeachPos.Tag));
        }

        private void UpdateRotateTeachPos(int selectedPos)
        {
            int nCount = 0, i = 0, j = 0;

            nCount = (int)ERotatePos.MAX;

            for (i = 0; i < nCount; i++)
            {
                RotatePos[i].BackColor = Color.LightYellow;
            }

            for (i = 0; i < GridRotateTeachTable.ColCount + 1; i++)
            {
                for (j = 0; j < GridRotateTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridRotateTeachTable[j, i].TextColor = Color.Black;
                        GridRotateTeachTable[j, i].Text = "";
                    }
                }

                if (GetDataMode() == FixedData)
                {
                    if (i != 0) GridRotateTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridRotateTeachTable[6, i].BackColor = Color.White;
                }

                if (GetDataMode() == OffsetData)
                {
                    if (i != 0) GridRotateTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridRotateTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            RotatePos[selectedPos].BackColor = Color.Tan;

            m_nSelectedPos_Rotate = selectedPos;
            DisplayPos_Rotate();
        }

        private void UpdateNozzleTeachPos(int selectedPos)
        {
            int nCount = 0, i = 0, j = 0;

            nCount = (int)ENozzlePos.MAX;

            for (i = 0; i < nCount; i++)
            {
                NozzlePos[i].BackColor = Color.LightYellow;
            }

            for (i = 0; i < GridNozzleTeachTable.ColCount + 1; i++)
            {
                for (j = 0; j < GridNozzleTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridNozzleTeachTable[j, i].TextColor = Color.Black;
                        GridNozzleTeachTable[j, i].Text = "";
                    }
                }

                if (GetDataMode() == FixedData)
                {
                    if (i != 0) GridNozzleTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridNozzleTeachTable[6, i].BackColor = Color.White;
                }

                if (GetDataMode() == OffsetData)
                {
                    if (i != 0) GridNozzleTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridNozzleTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            NozzlePos[selectedPos].BackColor = Color.Tan;

            m_nSelectedPos_Nozzle = selectedPos;
            DisplayPos_Nozzle();
        }

        private void DisplayPos_Nozzle()
        {
            double dFixedN1Pos = 0, dOffsetN1Pos = 0, dTargetN1Pos = 0, dModelN1Pos = 0, dAlignN1Offset;
            double dFixedN2Pos = 0, dOffsetN2Pos = 0, dTargetN2Pos = 0, dModelN2Pos = 0, dAlignN2Offset;
            int index = m_nSelectedPos_Nozzle;

            dFixedN1Pos = movingCleanObject.FixedPos.Pos[index].dT;
            dOffsetN1Pos = movingCleanObject.OffsetPos.Pos[index].dT;
            dModelN1Pos = movingCleanObject.ModelPos.Pos[index].dT;
            dAlignN1Offset = movingCleanObject.AlignOffset.dT;

            dTargetN1Pos = dFixedN1Pos + dOffsetN1Pos + dModelN1Pos + dAlignN1Offset;

            GridNozzleTeachTable[2, 1].Text = Convert.ToString(dTargetN1Pos);

            dFixedN2Pos = movingCoatObject.FixedPos.Pos[index].dT;
            dOffsetN2Pos = movingCoatObject.OffsetPos.Pos[index].dT;
            dModelN2Pos = movingCoatObject.ModelPos.Pos[index].dT;
            dAlignN2Offset = movingCoatObject.AlignOffset.dT;

            dTargetN2Pos = dFixedN2Pos + dOffsetN2Pos + dModelN2Pos + dAlignN2Offset;

            GridNozzleTeachTable[2, 2].Text = Convert.ToString(dTargetN2Pos);

            // FixedPos
            GridNozzleTeachTable[3, 1].Text = Convert.ToString(dFixedN1Pos);
            GridNozzleTeachTable[3, 2].Text = Convert.ToString(dFixedN2Pos);

            // ModelPos
            GridNozzleTeachTable[4, 1].Text = Convert.ToString(dModelN1Pos);
            GridNozzleTeachTable[4, 2].Text = Convert.ToString(dModelN2Pos);

            // Align Offset
            GridNozzleTeachTable[5, 1].Text = Convert.ToString(dAlignN1Offset);
            GridNozzleTeachTable[5, 2].Text = Convert.ToString(dAlignN2Offset);

            //OffsetPos
            GridNozzleTeachTable[6, 1].Text = Convert.ToString(dOffsetN1Pos);
            GridNozzleTeachTable[6, 2].Text = Convert.ToString(dOffsetN2Pos);
        }

        private void DisplayPos_Rotate()
        {
            double dFixedPos = 0, dOffsetPos = 0, dTargetPos = 0, dModelPos = 0, dAlignOffset;
            int index = m_nSelectedPos_Rotate;

            dFixedPos = movingRotateObject.FixedPos.Pos[index].dT;
            dOffsetPos = movingRotateObject.OffsetPos.Pos[index].dT;
            dModelPos = movingRotateObject.ModelPos.Pos[index].dT;
            dAlignOffset = movingRotateObject.AlignOffset.dT;

            dTargetPos = dFixedPos + dOffsetPos + dModelPos + dAlignOffset;

            GridRotateTeachTable[2, 1].Text = Convert.ToString(dTargetPos);

            // FixedPos
            GridRotateTeachTable[3, 1].Text = Convert.ToString(dFixedPos);

            // ModelPos
            GridRotateTeachTable[4, 1].Text = Convert.ToString(dModelPos);

            // AlignOffsetPos
            GridRotateTeachTable[5, 1].Text = Convert.ToString(dModelPos);

            //OffsetPos
            GridRotateTeachTable[6, 1].Text = Convert.ToString(dOffsetPos);
        }

        private void ResouceMapping()
        {
            NozzlePos[0] = BtnNozzlePos1; NozzlePos[1] = BtnNozzlePos2; NozzlePos[2] = BtnNozzlePos3; NozzlePos[3] = BtnNozzlePos4; NozzlePos[4] = BtnNozzlePos5;
            NozzlePos[5] = BtnNozzlePos6; NozzlePos[6] = BtnNozzlePos7; NozzlePos[7] = BtnNozzlePos8; NozzlePos[8] = BtnNozzlePos9; NozzlePos[9] = BtnNozzlePos10;
            NozzlePos[10] = BtnNozzlePos11; NozzlePos[11] = BtnNozzlePos12; NozzlePos[12] = BtnNozzlePos13; NozzlePos[13] = BtnNozzlePos14; NozzlePos[14] = BtnNozzlePos15;

            RotatePos[0] = BtnRotatePos1; RotatePos[1] = BtnRotatePos2; RotatePos[2] = BtnRotatePos3; RotatePos[3] = BtnRotatePos4; RotatePos[4] = BtnRotatePos5;
            RotatePos[5] = BtnRotatePos6; RotatePos[6] = BtnRotatePos7; RotatePos[7] = BtnRotatePos8; RotatePos[8] = BtnRotatePos9; RotatePos[9] = BtnRotatePos10;
            RotatePos[10] = BtnRotatePos11; RotatePos[11] = BtnRotatePos12; RotatePos[12] = BtnRotatePos13; RotatePos[13] = BtnRotatePos14; RotatePos[14] = BtnRotatePos15;

            int i = 0;

            for (i = 0; i < 15; i++)
            {
                NozzlePos[i].Visible = false;
                RotatePos[i].Visible = false;
            }

            for (i = 0; i < (int)ENozzlePos.MAX; i++)
            {
                NozzlePos[i].Visible = true;
                NozzlePos[i].Text = Convert.ToString(ENozzlePos.SAFETY + i);
            }

            for (i = 0; i < (int)ERotatePos.MAX; i++)
            {
                RotatePos[i].Visible = true;
                RotatePos[i].Text = Convert.ToString(ERotatePos.LOAD + i);
            }
        }

        private void InitNozzleGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridNozzleTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridNozzleTeachTable.Properties.RowHeaders = true;
            GridNozzleTeachTable.Properties.ColHeaders = false;

            nCol = 2;
            nRow = 8;

            // Column,Row 개수
            GridNozzleTeachTable.ColCount = nCol;
            GridNozzleTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridNozzleTeachTable.ColWidths.SetSize(i, 172);
            }

            GridNozzleTeachTable.ColWidths.SetSize(0, 110);

            for (i = 0; i < nRow + 1; i++)
            {
                GridNozzleTeachTable.RowHeights[i] = 40;
            }

            // Text Display
            GridNozzleTeachTable[1, 1].CellType = GridCellTypeName.PushButton;
            GridNozzleTeachTable[1, 2].CellType = GridCellTypeName.PushButton;

            GridNozzleTeachTable[1, 1].CellAppearance = GridCellAppearance.Raised;
            GridNozzleTeachTable[1, 2].CellAppearance = GridCellAppearance.Raised;

            GridNozzleTeachTable[1, 1].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);
            GridNozzleTeachTable[1, 2].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);

            GridNozzleTeachTable[2, 0].Text = "목표 위치";
            GridNozzleTeachTable[3, 0].Text = "고정 좌표";
            GridNozzleTeachTable[4, 0].Text = "모델 좌표";
            GridNozzleTeachTable[5, 0].Text = "Cell Mark 보정";
            GridNozzleTeachTable[6, 0].Text = "Offset 좌표";
            GridNozzleTeachTable[7, 0].Text = "현재 위치";
            GridNozzleTeachTable[8, 0].Text = "보정값";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridNozzleTeachTable[j, i].Font.Bold = true;

                    GridNozzleTeachTable[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridNozzleTeachTable[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    if (i != 0 && j != 0)
                    {
                        GridNozzleTeachTable[j, i].Text = "";
                        GridNozzleTeachTable[j, i].TextColor = Color.Black;
                    }
                }
            }

            GridNozzleTeachTable.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridNozzleTeachTable.ResizeColsBehavior = 0;
            GridNozzleTeachTable.ResizeRowsBehavior = 0;

            for (i = 0; i < nCol; i++)
            {
                GridNozzleTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridNozzleTeachTable[1, i + 1].Description = "";
            }

            GridNozzleTeachTable[1, 0].Text = Convert.ToString(ETeachUnit.CLEANER2);

            GridNozzleTeachTable[1, 1].Description = "Nozzle 1 T Axis";
            GridNozzleTeachTable[1, 1].TextColor = Color.DarkRed;

            GridNozzleTeachTable[1, 2].Description = "Nozzle 2 T Axis";
            GridNozzleTeachTable[1, 2].TextColor = Color.DarkRed;

            // Grid Display Update
            GridNozzleTeachTable.Refresh();
        }

        private void InitRotateGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridRotateTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridRotateTeachTable.Properties.RowHeaders = true;
            GridRotateTeachTable.Properties.ColHeaders = false;

            nCol = 1;
            nRow = 8;

            // Column,Row 개수
            GridRotateTeachTable.ColCount = nCol;
            GridRotateTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridRotateTeachTable.ColWidths.SetSize(i, 172);
            }

            GridRotateTeachTable.ColWidths.SetSize(0, 110);

            for (i = 0; i < nRow + 1; i++)
            {
                GridRotateTeachTable.RowHeights[i] = 40;

            }

            // Text Display
            GridRotateTeachTable[1, 1].CellType = GridCellTypeName.PushButton;

            GridRotateTeachTable[1, 1].CellAppearance = GridCellAppearance.Raised;

            GridRotateTeachTable[1, 1].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);

            GridRotateTeachTable[2, 0].Text = "목표 위치";
            GridRotateTeachTable[3, 0].Text = "고정 좌표";
            GridRotateTeachTable[4, 0].Text = "모델 좌표";
            GridRotateTeachTable[5, 0].Text = "Cell Mark 보정";
            GridRotateTeachTable[6, 0].Text = "Offset 좌표";
            GridRotateTeachTable[7, 0].Text = "현재 위치";
            GridRotateTeachTable[8, 0].Text = "보정값";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridRotateTeachTable[j, i].Font.Bold = true;

                    GridRotateTeachTable[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridRotateTeachTable[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    if (i != 0 && j != 0)
                    {
                        GridRotateTeachTable[j, i].Text = "";
                        GridRotateTeachTable[j, i].TextColor = Color.Black;
                    }
                }
            }

            GridRotateTeachTable.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridRotateTeachTable.ResizeColsBehavior = 0;
            GridRotateTeachTable.ResizeRowsBehavior = 0;

            for (i = 0; i < 1; i++)
            {
                GridRotateTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridRotateTeachTable[1, i + 1].Description = "";
            }

            GridRotateTeachTable[1, 0].Text = Convert.ToString(ETeachUnit.CLEANER2);

            GridRotateTeachTable[1, 1].Description = "Rotate T Axis";
            GridRotateTeachTable[1, 1].TextColor = Color.DarkRed;

            // Grid Display Update
            GridRotateTeachTable.Refresh();
        }

        private void GridNozzleTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";

            strCurrent = GridNozzleTeachTable[2, e.ColIndex].Text;
            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            ChangeNozzleTargetPos(strModify, e.ColIndex);
        }

        private void ChangeNozzleTargetPos(string strTarget, int index)
        {
            double dTargetPos = Convert.ToDouble(strTarget);
            double dOtherSum = Convert.ToDouble(GridNozzleTeachTable[4, index].Text) // Model Pos
                + Convert.ToDouble(GridNozzleTeachTable[5, index].Text); // + Align Mark Pos

            if (GetDataMode() == FixedData)
            {
                dOtherSum += Convert.ToDouble(GridNozzleTeachTable[6, index].Text); // Offset Pos
                double dPos = dTargetPos - dOtherSum;
                GridNozzleTeachTable[2, index].Text = Convert.ToString(dTargetPos);
                GridNozzleTeachTable[3, index].Text = Convert.ToString(dPos);
                GridNozzleTeachTable[3, index].TextColor = Color.Blue;
            }
            else
            {
                dOtherSum += Convert.ToDouble(GridNozzleTeachTable[3, index].Text); // Fixed Pos
                double dPos = dTargetPos - dOtherSum;
                GridNozzleTeachTable[2, index].Text = Convert.ToString(dTargetPos);
                GridNozzleTeachTable[6, index].Text = Convert.ToString(dPos);
                GridNozzleTeachTable[6, index].TextColor = Color.Blue;
            }
        }

        private void GridRotateTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 2 || e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";

            strCurrent = GridRotateTeachTable[2, e.ColIndex].Text;
            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            ChangeRotateTargetPos(strModify, e.ColIndex);
        }

        private void ChangeRotateTargetPos(string strTarget, int index)
        {
            double dTargetPos = Convert.ToDouble(strTarget);
            double dOtherSum = Convert.ToDouble(GridRotateTeachTable[4, index].Text) // Model Pos
                + Convert.ToDouble(GridRotateTeachTable[5, index].Text); // + Align Mark Pos

            if (GetDataMode() == FixedData)
            {
                dOtherSum += Convert.ToDouble(GridRotateTeachTable[6, index].Text); // Offset Pos
                double dPos = dTargetPos - dOtherSum;
                GridRotateTeachTable[2, index].Text = Convert.ToString(dTargetPos);
                GridRotateTeachTable[3, index].Text = Convert.ToString(dPos);
                GridRotateTeachTable[3, index].TextColor = Color.Blue;
            }
            else
            {
                dOtherSum += Convert.ToDouble(GridRotateTeachTable[3, index].Text); // Fixed Pos
                double dPos = dTargetPos - dOtherSum;
                GridRotateTeachTable[2, index].Text = Convert.ToString(dTargetPos);
                GridRotateTeachTable[6, index].Text = Convert.ToString(dPos);
                GridRotateTeachTable[6, index].TextColor = Color.Blue;
            }
        }

        private void BtnNozzleSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save teaching data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            if (GetDataMode() == FixedData)
            {
                strData = GridNozzleTeachTable[3, 1].Text;
                CMainFrame.DataManager.FixedPos.S2_CleanerPos.Pos[m_nSelectedPos_Nozzle].dT = Convert.ToDouble(strData);

                strData = GridNozzleTeachTable[3, 2].Text;
                CMainFrame.DataManager.FixedPos.S2_CoaterPos.Pos[m_nSelectedPos_Nozzle].dT = Convert.ToDouble(strData);

                CMainFrame.DataManager.SavePositionData(true, EPositionObject.S2_CLEAN_NOZZLE);
                CMainFrame.DataManager.SavePositionData(true, EPositionObject.S2_COAT_NOZZLE);
            }

            if (GetDataMode() == OffsetData)
            {
                strData = GridNozzleTeachTable[6, 1].Text;
                CMainFrame.DataManager.OffsetPos.S2_CleanerPos.Pos[m_nSelectedPos_Nozzle].dT = Convert.ToDouble(strData);

                strData = GridNozzleTeachTable[6, 2].Text;
                CMainFrame.DataManager.OffsetPos.S2_CoaterPos.Pos[m_nSelectedPos_Nozzle].dT = Convert.ToDouble(strData);

                CMainFrame.DataManager.SavePositionData(false, EPositionObject.S2_CLEAN_NOZZLE);
                CMainFrame.DataManager.SavePositionData(false, EPositionObject.S2_COAT_NOZZLE);
            }

            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.SPINNER2);

            DisplayPos_Nozzle();
        }

        private void BtnRotateSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save teaching data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            if (GetDataMode() == FixedData)
            {
                strData = GridRotateTeachTable[3, 1].Text;
                CMainFrame.DataManager.FixedPos.S2_RotatePos.Pos[m_nSelectedPos_Rotate].dT = Convert.ToDouble(strData);

                CMainFrame.DataManager.SavePositionData(true, EPositionObject.S2_ROTATE);
            }

            if (GetDataMode() == OffsetData)
            {
                strData = GridRotateTeachTable[6, 1].Text;
                CMainFrame.DataManager.OffsetPos.S2_RotatePos.Pos[m_nSelectedPos_Rotate].dT = Convert.ToDouble(strData);

                CMainFrame.DataManager.SavePositionData(false, EPositionObject.S2_ROTATE);
            }

            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.SPINNER2);

            DisplayPos_Rotate();
        }

        private void BtnNozzleChangeValue_Click(object sender, EventArgs e)
        {
            string strMsg = "Change target position to current position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            string strCurrent = GridNozzleTeachTable[7, 1].Text;
            ChangeNozzleTargetPos(strCurrent, 1);

            strCurrent = GridNozzleTeachTable[7, 2].Text;
            ChangeNozzleTargetPos(strCurrent, 2);
        }

        private void BtnRotateChangeValue_Click(object sender, EventArgs e)
        {
            string strMsg = "Change target position to current position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            string strCurrent = GridRotateTeachTable[7, 1].Text;
            ChangeRotateTargetPos(strCurrent, 1);
        }

        private void TmrTeach_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strCurPos = string.Empty;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.S2_CLEAN_NOZZLE_T].EncoderPos);
            GridNozzleTeachTable[7, 1].Text = strCurPos;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.S2_COAT_NOZZLE_T].EncoderPos);
            GridNozzleTeachTable[7, 2].Text = strCurPos;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.S2_CHUCK_ROTATE_T].EncoderPos);
            GridRotateTeachTable[7, 1].Text = strCurPos;

            // 보정값 Display
            double dValue = 0, dCurPos = 0, dTargetPos = 0;

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.S2_CLEAN_NOZZLE_T].EncoderPos;
            dTargetPos = Convert.ToDouble(GridNozzleTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridNozzleTeachTable[8, 1].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.S2_COAT_NOZZLE_T].EncoderPos;
            dTargetPos = Convert.ToDouble(GridNozzleTeachTable[2, 2].Text);
            dValue = dTargetPos - dCurPos;

            GridNozzleTeachTable[8, 2].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.S2_CHUCK_ROTATE_T].EncoderPos;
            dTargetPos = Convert.ToDouble(GridRotateTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridRotateTeachTable[8, 1].Text = Convert.ToString(dValue);
        }

        private void BtnNozzleTeachMove_Click(object sender, EventArgs e)
        {
            string strMsg = "Move to selected position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;
        }

        private void BtnRotateTeachMove_Click(object sender, EventArgs e)
        {
            string strMsg = "Move to selected position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;
        }

        private void BtnManualOP_Click(object sender, EventArgs e)
        {
            var dlg = new FormSpinnerManualOP();
            dlg.SetSpinner(Spinner2);
            dlg.ShowDialog();
        }
    }
}
