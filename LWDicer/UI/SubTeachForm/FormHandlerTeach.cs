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
    public partial class FormHandlerTeach : Form
    {
        const int LoHandler = 0;
        const int UpHandler = 1;

        ButtonAdv[] TeachUpPos = new ButtonAdv[15];
        ButtonAdv[] TeachLoPos = new ButtonAdv[15];

        private int nTeachUpPos = 0;
        private int nTeachLoPos = 0;

        private int nDataMode = 0;

        private FormHandlerManualOP m_HandlerManualOP = new FormHandlerManualOP();

        private CMovingObject movingUpperObject = CMainFrame.LWDicer.m_MeUpperHandler.AxHandlerInfo;
        private CMovingObject movingLowerObject = CMainFrame.LWDicer.m_MeLowerHandler.AxHandlerInfo;

        public FormHandlerTeach()
        {
            InitializeComponent();

            InitUpperHandlerGrid();
            InitLowerHandlerGrid();

            ResouceMapping();
        }

        private void FormClose()
        {
            TmrTeach.Stop();
            this.Hide();
        }

        private void FormHandlerTeach_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);

            this.Text = "Handler Part Teaching Screen";

            UpdateUpperTeachPos(0);
            UpdateLowerTeachPos(0);

            TmrTeach.Enabled = true;
            TmrTeach.Interval = 100;
            TmrTeach.Start();
        }

        private void ResouceMapping()
        {
            TeachUpPos[0] = BtnUpPos1; TeachUpPos[1] = BtnUpPos2; TeachUpPos[2] = BtnUpPos3; TeachUpPos[3] = BtnUpPos4; TeachUpPos[4] = BtnUpPos5;
            TeachUpPos[5] = BtnUpPos6; TeachUpPos[6] = BtnUpPos7; TeachUpPos[7] = BtnUpPos8; TeachUpPos[8] = BtnUpPos9; TeachUpPos[9] = BtnUpPos10;
            TeachUpPos[10] = BtnUpPos11; TeachUpPos[11] = BtnUpPos12; TeachUpPos[12] = BtnUpPos13; TeachUpPos[13] = BtnUpPos14; TeachUpPos[14] = BtnUpPos15;

            TeachLoPos[0] = BtnLoPos1; TeachLoPos[1] = BtnLoPos2; TeachLoPos[2] = BtnLoPos3; TeachLoPos[3] = BtnLoPos4; TeachLoPos[4] = BtnLoPos5;
            TeachLoPos[5] = BtnLoPos6; TeachLoPos[6] = BtnLoPos7; TeachLoPos[7] = BtnLoPos8; TeachLoPos[8] = BtnLoPos9; TeachLoPos[9] = BtnLoPos10;
            TeachLoPos[10] = BtnLoPos11; TeachLoPos[11] = BtnLoPos12; TeachLoPos[12] = BtnLoPos13; TeachLoPos[13] = BtnLoPos14; TeachLoPos[14] = BtnLoPos15;

            int i = 0;

            for(i=0;i<15;i++)
            {
                TeachLoPos[i].Visible = false;
                TeachUpPos[i].Visible = false;
            }

            for (i = 0; i < (int)EHandlerPos.MAX; i++)
            {
                TeachUpPos[i].Visible = true;
                TeachLoPos[i].Visible = true;

                TeachUpPos[i].Text = Convert.ToString(EHandlerPos.WAIT + i);
                TeachLoPos[i].Text = Convert.ToString(EHandlerPos.WAIT + i);
            }
        }

        private void InitUpperHandlerGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridUpHandlerTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridUpHandlerTeachTable.Properties.RowHeaders = true;
            GridUpHandlerTeachTable.Properties.ColHeaders = false;

            nCol = 2;
            nRow = 8;

            // Column,Row 개수
            GridUpHandlerTeachTable.ColCount = nCol;
            GridUpHandlerTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridUpHandlerTeachTable.ColWidths.SetSize(i, 172);
            }

            GridUpHandlerTeachTable.ColWidths.SetSize(0, 110);

            for (i = 0; i < nRow + 1; i++)
            {
                GridUpHandlerTeachTable.RowHeights[i] = 40;
            }

            // Text Display
            GridUpHandlerTeachTable[1, 1].CellType = GridCellTypeName.PushButton;
            GridUpHandlerTeachTable[1, 2].CellType = GridCellTypeName.PushButton;

            GridUpHandlerTeachTable[1, 1].CellAppearance = GridCellAppearance.Raised;
            GridUpHandlerTeachTable[1, 2].CellAppearance = GridCellAppearance.Raised;

            GridUpHandlerTeachTable[1, 1].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);
            GridUpHandlerTeachTable[1, 2].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);

            GridUpHandlerTeachTable[2, 0].Text = "목표 위치";
            GridUpHandlerTeachTable[3, 0].Text = "고정 좌표";
            GridUpHandlerTeachTable[4, 0].Text = "모델 좌표";
            GridUpHandlerTeachTable[5, 0].Text = "Cell Mark 보정";
            GridUpHandlerTeachTable[6, 0].Text = "Offset 좌표";
            GridUpHandlerTeachTable[7, 0].Text = "현재 위치";
            GridUpHandlerTeachTable[8, 0].Text = "보정값";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridUpHandlerTeachTable[j, i].Font.Bold = true;

                    GridUpHandlerTeachTable[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridUpHandlerTeachTable[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    if (i != 0 && j != 0)
                    {
                        GridUpHandlerTeachTable[j, i].Text = "";
                        GridUpHandlerTeachTable[j, i].TextColor = Color.Black;
                    }
                }
            }

            GridUpHandlerTeachTable.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridUpHandlerTeachTable.ResizeColsBehavior = 0;
            GridUpHandlerTeachTable.ResizeRowsBehavior = 0;

            for (i = 0; i < nCol; i++)
            {
                GridUpHandlerTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridUpHandlerTeachTable[1, i + 1].Description = "";
            }

            GridUpHandlerTeachTable[1, 0].Text = Convert.ToString(ETeachUnit.HANDLER);

            GridUpHandlerTeachTable[1, 1].Description = "Up X Axis";
            GridUpHandlerTeachTable[1, 1].TextColor = Color.DarkRed;

            GridUpHandlerTeachTable[1, 2].Description = "Up Z Axis";
            GridUpHandlerTeachTable[1, 2].TextColor = Color.DarkRed;

            // Grid Display Update
            GridUpHandlerTeachTable.Refresh();
        }

        private void InitLowerHandlerGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridLoHandlerTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridLoHandlerTeachTable.Properties.RowHeaders = true;
            GridLoHandlerTeachTable.Properties.ColHeaders = false;

            nCol = 2;
            nRow = 8;

            // Column,Row 개수
            GridLoHandlerTeachTable.ColCount = nCol;
            GridLoHandlerTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridLoHandlerTeachTable.ColWidths.SetSize(i, 172);
            }

            GridLoHandlerTeachTable.ColWidths.SetSize(0, 110);

            for (i = 0; i < nRow + 1; i++)
            {
                GridLoHandlerTeachTable.RowHeights[i] = 40;

            }

            // Text Display
            GridLoHandlerTeachTable[1, 1].CellType = GridCellTypeName.PushButton;
            GridLoHandlerTeachTable[1, 2].CellType = GridCellTypeName.PushButton;

            GridLoHandlerTeachTable[1, 1].CellAppearance = GridCellAppearance.Raised;
            GridLoHandlerTeachTable[1, 2].CellAppearance = GridCellAppearance.Raised;

            GridLoHandlerTeachTable[1, 1].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);
            GridLoHandlerTeachTable[1, 2].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);

            GridLoHandlerTeachTable[2, 0].Text = "목표 위치";
            GridLoHandlerTeachTable[3, 0].Text = "고정 좌표";
            GridLoHandlerTeachTable[4, 0].Text = "모델 좌표";
            GridLoHandlerTeachTable[5, 0].Text = "Cell Mark 보정";
            GridLoHandlerTeachTable[6, 0].Text = "Offset 좌표";
            GridLoHandlerTeachTable[7, 0].Text = "현재 위치";
            GridLoHandlerTeachTable[8, 0].Text = "보정값";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridLoHandlerTeachTable[j, i].Font.Bold = true;

                    GridLoHandlerTeachTable[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridLoHandlerTeachTable[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    if (i != 0 && j != 0)
                    {
                        GridLoHandlerTeachTable[j, i].Text = "";
                        GridLoHandlerTeachTable[j, i].TextColor = Color.Black;
                    }
                }
            }

            GridLoHandlerTeachTable.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridLoHandlerTeachTable.ResizeColsBehavior = 0;
            GridLoHandlerTeachTable.ResizeRowsBehavior = 0;

            for (i = 0; i < nCol; i++)
            {
                GridLoHandlerTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridLoHandlerTeachTable[1, i + 1].Description = "";
            }

            GridLoHandlerTeachTable[1, 0].Text = Convert.ToString(ETeachUnit.HANDLER);

            GridLoHandlerTeachTable[1, 1].Description = "Lo X Axis";
            GridLoHandlerTeachTable[1, 1].TextColor = Color.DarkRed;

            GridLoHandlerTeachTable[1, 2].Description = "Lo Z Axis";
            GridLoHandlerTeachTable[1, 2].TextColor = Color.DarkRed;

            // Grid Display Update
            GridLoHandlerTeachTable.Refresh();
        }

        private void FormHandlerTeach_FormClosing(object sender, FormClosingEventArgs e)
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
            FormJogOperation m_Jog = new FormJogOperation();
            m_Jog.ShowDialog();
        }

        private void BtnUpPos_Click(object sender, EventArgs e)
        {
            ButtonAdv TeachPos = sender as ButtonAdv;

            UpdateUpperTeachPos(Convert.ToInt16(TeachPos.Tag));
        }

        private void BtnLoPos_Click(object sender, EventArgs e)
        {
            ButtonAdv TeachPos = sender as ButtonAdv;

            UpdateLowerTeachPos(Convert.ToInt16(TeachPos.Tag));
        }

        private void UpdateUpperTeachPos(int PosNo)
        {
            int nCount = 0, i = 0, j = 0;

            nCount = (int)EHandlerPos.MAX;

            for (i = 0; i < nCount; i++)
            {
                TeachUpPos[i].BackColor = Color.LightYellow;
            }

            for (i = 0; i < GridUpHandlerTeachTable.ColCount + 1; i++)
            {
                for (j = 0; j < GridUpHandlerTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridUpHandlerTeachTable[j, i].TextColor = Color.Black;
                        GridUpHandlerTeachTable[j, i].Text = "";
                    }
                }

                if (GetDataMode() == FixedData)
                {
                    if (i != 0) GridUpHandlerTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridUpHandlerTeachTable[6, i].BackColor = Color.White;
                }

                if (GetDataMode() == OffsetData)
                {
                    if (i != 0) GridUpHandlerTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridUpHandlerTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            TeachUpPos[PosNo].BackColor = Color.Tan;

            SetUpPosNo(PosNo);

            LoadUpTeachingData(PosNo);
        }

        private void UpdateLowerTeachPos(int PosNo)
        {
            int nCount = 0, i = 0, j = 0;

            nCount = (int)EHandlerPos.MAX;

            for (i = 0; i < nCount; i++)
            {
                TeachLoPos[i].BackColor = Color.LightYellow;
            }

            for (i = 0; i < GridLoHandlerTeachTable.ColCount + 1; i++)
            {
                for (j = 0; j < GridLoHandlerTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridLoHandlerTeachTable[j, i].TextColor = Color.Black;
                        GridLoHandlerTeachTable[j, i].Text = "";
                    }
                }

                if (GetDataMode() == FixedData)
                {
                    if (i != 0) GridLoHandlerTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridLoHandlerTeachTable[6, i].BackColor = Color.White;
                }

                if (GetDataMode() == OffsetData)
                {
                    if (i != 0) GridLoHandlerTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridLoHandlerTeachTable[6, i].BackColor = Color.LightYellow;
                }

            }

            TeachLoPos[PosNo].BackColor = Color.Tan;

            SetLoPosNo(PosNo);

            LoadLoTeachingData(PosNo);
        }


        private void SetUpPosNo(int nPosNo)
        {
            nTeachUpPos = nPosNo;
        }

        private int GetUpPosNo()
        {
            return nTeachUpPos;
        }

        private void SetLoPosNo(int nPosNo)
        {
            nTeachLoPos = nPosNo;
        }

        private int GetLoPosNo()
        {
            return nTeachLoPos;
        }

        private void LoadUpTeachingData(int nTeachPos)
        {
            string strTargetPos;

            double dFixedXPos = 0, dOffsetXPos = 0, dTargetXPos = 0, dModelXPos = 0, dXAlignOffset;
            double dFixedZPos = 0, dOffsetZPos = 0, dTargetZPos = 0, dModelZPos = 0, dZAlignOffset;

            dFixedXPos = movingUpperObject.FixedPos.Pos[nTeachPos].dX;
            dOffsetXPos = movingUpperObject.OffsetPos.Pos[nTeachPos].dX;
            dModelXPos = movingUpperObject.ModelPos.Pos[nTeachPos].dX;
            dXAlignOffset = movingUpperObject.AlignOffset.dX;

            dTargetXPos = dFixedXPos + dOffsetXPos + dModelXPos + dXAlignOffset;

            strTargetPos = Convert.ToString(dTargetXPos);
            GridUpHandlerTeachTable[2, 1].Text = strTargetPos;

            dFixedZPos = movingUpperObject.FixedPos.Pos[nTeachPos].dZ;
            dOffsetZPos = movingUpperObject.OffsetPos.Pos[nTeachPos].dZ;
            dModelZPos = movingUpperObject.ModelPos.Pos[nTeachPos].dZ;
            dZAlignOffset = movingUpperObject.AlignOffset.dZ;

            dTargetZPos = dFixedZPos + dOffsetZPos + dModelZPos + dZAlignOffset;

            strTargetPos = Convert.ToString(dTargetZPos);
            GridUpHandlerTeachTable[2, 2].Text = strTargetPos;

            // FixedPos
            GridUpHandlerTeachTable[3, 1].Text = Convert.ToString(dFixedXPos);
            GridUpHandlerTeachTable[3, 2].Text = Convert.ToString(dFixedZPos);

            // ModelPos
            GridUpHandlerTeachTable[4, 1].Text = Convert.ToString(dModelXPos); 
            GridUpHandlerTeachTable[4, 2].Text = Convert.ToString(dModelZPos);

            // Align Offset
            GridUpHandlerTeachTable[5, 1].Text = Convert.ToString(dXAlignOffset);
            GridUpHandlerTeachTable[5, 2].Text = Convert.ToString(dZAlignOffset);

            //OffsetPos
            GridUpHandlerTeachTable[6, 1].Text = Convert.ToString(dOffsetXPos);
            GridUpHandlerTeachTable[6, 2].Text = Convert.ToString(dOffsetZPos);
        }

        private void LoadLoTeachingData(int nTeachPos)
        {
            string strTargetPos;

            double dFixedXPos = 0, dOffsetXPos = 0, dTargetXPos = 0, dModelXPos = 0, dXAlignOffset;
            double dFixedZPos = 0, dOffsetZPos = 0, dTargetZPos = 0, dModelZPos = 0, dZAlignOffset;

            dFixedXPos = movingLowerObject.FixedPos.Pos[nTeachPos].dX;
            dOffsetXPos = movingLowerObject.OffsetPos.Pos[nTeachPos].dX;
            dModelXPos = movingLowerObject.ModelPos.Pos[nTeachPos].dX;
            dXAlignOffset = movingLowerObject.AlignOffset.dX;

            dTargetXPos = dFixedXPos + dOffsetXPos + dModelXPos + dXAlignOffset;

            strTargetPos = Convert.ToString(dTargetXPos);
            GridLoHandlerTeachTable[2, 1].Text = strTargetPos;

            dFixedZPos = movingLowerObject.FixedPos.Pos[nTeachPos].dZ;
            dOffsetZPos = movingLowerObject.OffsetPos.Pos[nTeachPos].dZ;
            dModelZPos = movingLowerObject.ModelPos.Pos[nTeachPos].dZ;
            dZAlignOffset = movingLowerObject.AlignOffset.dZ;

            dTargetZPos = dFixedZPos + dOffsetZPos + dModelZPos + dZAlignOffset;

            strTargetPos = Convert.ToString(dTargetZPos);
            GridLoHandlerTeachTable[2, 2].Text = strTargetPos;

            // FixedPos
            GridLoHandlerTeachTable[3, 1].Text = Convert.ToString(dFixedXPos);
            GridLoHandlerTeachTable[3, 2].Text = Convert.ToString(dFixedZPos);

            // ModelPos
            GridLoHandlerTeachTable[4, 1].Text = Convert.ToString(dModelXPos);
            GridLoHandlerTeachTable[4, 2].Text = Convert.ToString(dModelZPos);

            // Align Offset
            GridLoHandlerTeachTable[5, 1].Text = Convert.ToString(dXAlignOffset);
            GridLoHandlerTeachTable[5, 2].Text = Convert.ToString(dZAlignOffset);

            //OffsetPos
            GridLoHandlerTeachTable[6, 1].Text = Convert.ToString(dOffsetXPos);
            GridLoHandlerTeachTable[6, 2].Text = Convert.ToString(dOffsetZPos);
        }

        private void TmrTeach_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strCurPos = string.Empty;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_X].EncoderPos);
            GridUpHandlerTeachTable[7, 1].Text = strCurPos;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_Z].EncoderPos);
            GridUpHandlerTeachTable[7, 2].Text = strCurPos;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_X].EncoderPos);
            GridLoHandlerTeachTable[7, 1].Text = strCurPos;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_Z].EncoderPos);
            GridLoHandlerTeachTable[7, 2].Text = strCurPos;


            // 보정값 Display
            double dValue = 0, dCurPos = 0, dTargetPos = 0;

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_X].EncoderPos;
            dTargetPos = Convert.ToDouble(GridUpHandlerTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridUpHandlerTeachTable[8, 1].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_Z].EncoderPos;
            dTargetPos = Convert.ToDouble(GridUpHandlerTeachTable[2, 2].Text);
            dValue = dTargetPos - dCurPos;

            GridUpHandlerTeachTable[8, 2].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_X].EncoderPos;
            dTargetPos = Convert.ToDouble(GridLoHandlerTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridLoHandlerTeachTable[8, 1].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_Z].EncoderPos;
            dTargetPos = Convert.ToDouble(GridLoHandlerTeachTable[2, 2].Text);
            dValue = dTargetPos - dCurPos;

            GridLoHandlerTeachTable[8, 2].Text = Convert.ToString(dValue);
        }

        private void BtnUpChangeValue_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strMsg = string.Empty;

            strMsg = TeachUpPos[GetUpPosNo()].Text + " 목표 위치를 현재 위치로 변경하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }

            strCurrent = GridUpHandlerTeachTable[7, 1].Text;
            ChangeUpHandlerTargetPos(strCurrent, 1);

            strCurrent = GridUpHandlerTeachTable[7, 2].Text;
            ChangeUpHandlerTargetPos(strCurrent, 2);

        }

        private void BtnLoChangeValue_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strMsg = string.Empty;

            strMsg = TeachLoPos[GetUpPosNo()].Text + " 목표 위치를 현재 위치로 변경하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }

            strCurrent = GridLoHandlerTeachTable[7, 1].Text;
            ChangeLoHandlerTargetPos(strCurrent, 1);

            strCurrent = GridLoHandlerTeachTable[7, 2].Text;
            ChangeLoHandlerTargetPos(strCurrent, 2);

        }

        private void BtnLoSave_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty, strData = string.Empty;

            strMsg = GridLoHandlerTeachTable[1, 0].Text + " Unit에 " + TeachLoPos[GetLoPosNo()].Text + " Teaching Data를 저장하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }

            if (GetDataMode() == FixedData)
            {
                strData = GridLoHandlerTeachTable[3, 1].Text;
                CMainFrame.LWDicer.m_DataManager.FixedPos.LowerHandlerPos.Pos[GetLoPosNo()].dX = Convert.ToDouble(strData);

                strData = GridLoHandlerTeachTable[3, 2].Text;
                CMainFrame.LWDicer.m_DataManager.FixedPos.LowerHandlerPos.Pos[GetLoPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(true, EPositionObject.LOWER_HANDLER);
            }

            if (GetDataMode() == OffsetData)
            {
                strData = GridLoHandlerTeachTable[6, 1].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[GetLoPosNo()].dX = Convert.ToDouble(strData);

                strData = GridLoHandlerTeachTable[6, 2].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[GetLoPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(false, EPositionObject.LOWER_HANDLER);
            }

            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.HANDLER);

            LoadUpTeachingData(GetLoPosNo());
        }

        private void BtnUpSave_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty, strData = string.Empty;

            strMsg = GridUpHandlerTeachTable[1, 0].Text + " Unit에 " + TeachUpPos[GetUpPosNo()].Text + " Teaching Data를 저장하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }

            if (GetDataMode() == FixedData)
            {
                strData = GridUpHandlerTeachTable[3, 1].Text;
                CMainFrame.LWDicer.m_DataManager.FixedPos.UpperHandlerPos.Pos[GetUpPosNo()].dX = Convert.ToDouble(strData);

                strData = GridUpHandlerTeachTable[3, 2].Text;
                CMainFrame.LWDicer.m_DataManager.FixedPos.UpperHandlerPos.Pos[GetUpPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(true, EPositionObject.UPPER_HANDLER);
            }

            if (GetDataMode() == OffsetData)
            {
                strData = GridUpHandlerTeachTable[6, 1].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[GetUpPosNo()].dX = Convert.ToDouble(strData);

                strData = GridUpHandlerTeachTable[6, 2].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[GetUpPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(false, EPositionObject.UPPER_HANDLER);
            }

            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.HANDLER);

            LoadLoTeachingData(GetUpPosNo());
        }

        private void GridUpHandlerTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";

            strCurrent = GridUpHandlerTeachTable[2, e.ColIndex].Text;
            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            ChangeUpHandlerTargetPos(strModify, e.ColIndex);
        }


        private void ChangeUpHandlerTargetPos(string strTarget, int index)
        {
            double dTargetPos = Convert.ToDouble(strTarget);
            double dOtherSum = Convert.ToDouble(GridUpHandlerTeachTable[4, index].Text) // Model Pos
                + Convert.ToDouble(GridUpHandlerTeachTable[5, index].Text); // + Align Mark Pos

            if (GetDataMode() == FixedData)
            {
                dOtherSum += Convert.ToDouble(GridUpHandlerTeachTable[6, index].Text); // Offset Pos
                double dPos = dTargetPos - dOtherSum;
                GridUpHandlerTeachTable[2, index].Text = Convert.ToString(dTargetPos);
                GridUpHandlerTeachTable[3, index].Text = Convert.ToString(dPos);
                GridUpHandlerTeachTable[3, index].TextColor = Color.Red;
            }
            else
            {
                dOtherSum += Convert.ToDouble(GridUpHandlerTeachTable[3, index].Text); // Fixed Pos
                double dPos = dTargetPos - dOtherSum;
                GridUpHandlerTeachTable[2, index].Text = Convert.ToString(dTargetPos);
                GridUpHandlerTeachTable[6, index].Text = Convert.ToString(dPos);
                GridUpHandlerTeachTable[6, index].TextColor = Color.Red;
            }
        }

        private void ChangeLoHandlerTargetPos(string strTarget, int index)
        {
            double dTargetPos = Convert.ToDouble(strTarget);
            double dOtherSum = Convert.ToDouble(GridLoHandlerTeachTable[4, index].Text) // Model Pos
                + Convert.ToDouble(GridLoHandlerTeachTable[5, index].Text); // + Align Mark Pos

            if (GetDataMode() == FixedData)
            {
                dOtherSum += Convert.ToDouble(GridLoHandlerTeachTable[6, index].Text); // Offset Pos
                double dPos = dTargetPos - dOtherSum;
                GridLoHandlerTeachTable[2, index].Text = Convert.ToString(dTargetPos);
                GridLoHandlerTeachTable[3, index].Text = Convert.ToString(dPos);
                GridLoHandlerTeachTable[3, index].TextColor = Color.Red;
            }
            else
            {
                dOtherSum += Convert.ToDouble(GridLoHandlerTeachTable[3, index].Text); // Fixed Pos
                double dPos = dTargetPos - dOtherSum;
                GridLoHandlerTeachTable[2, index].Text = Convert.ToString(dTargetPos);
                GridLoHandlerTeachTable[6, index].Text = Convert.ToString(dPos);
                GridLoHandlerTeachTable[6, index].TextColor = Color.Red;
            }
        }


        private void GridLoHandlerTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";

            strCurrent = GridLoHandlerTeachTable[2, e.ColIndex].Text;
            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            ChangeLoHandlerTargetPos(strModify, e.ColIndex);
        }

        private void BtnUpTeachMove_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty;

            strMsg = TeachUpPos[GetUpPosNo()].Text + " 목표 위치로 이동하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }
        }

        private void BtnLoTeachMove_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty;

            strMsg = TeachLoPos[GetLoPosNo()].Text + " 목표 위치로 이동하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }
        }

        private void BtnManualOP_Click(object sender, EventArgs e)
        {
            Button Handler = sender as Button;

            if(Handler.Name == "BtnUpperManualOP")
            {
                m_HandlerManualOP.SetHandler(UpHandler);
                m_HandlerManualOP.ShowDialog();
            }

            if (Handler.Name == "BtnLowerManualOP")
            {
                m_HandlerManualOP.SetHandler(LoHandler);
                m_HandlerManualOP.ShowDialog();
            }
        }
    }
}
