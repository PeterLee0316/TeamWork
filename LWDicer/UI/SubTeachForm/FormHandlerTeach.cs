﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        const int FixedData = 0;
        const int OffSetData = 1;

        ButtonAdv[] TeachUpPos = new ButtonAdv[15];
        ButtonAdv[] TeachLoPos = new ButtonAdv[15];

        private int nTeachUpPos = 0;
        private int nTeachLoPos = 0;

        private int nDataMode = 0;

        private FormHandlerManualOP m_HandlerManualOP = new FormHandlerManualOP();

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
            GridUpHandlerTeachTable[6, 0].Text = "OffSet 좌표";
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
            GridLoHandlerTeachTable[6, 0].Text = "OffSet 좌표";
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

                if (GetDataMode() == OffSetData)
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

                if (GetDataMode() == OffSetData)
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
            string strFixedPos = string.Empty, strOffSetPos = string.Empty, strTargetPos = string.Empty, strModelPos = string.Empty;
            double dFixedXPos = 0, dOffsetXPos = 0, dTargetXPos = 0, dModelXPos = 0;
            double dFixedZPos = 0, dOffsetZPos = 0, dTargetZPos = 0, dModelZPos = 0;

            dFixedXPos = CMainFrame.LWDicer.m_DataManager.FixedPos.LowerHandlerPos.Pos[nTeachPos].dX;
            dOffsetXPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[nTeachPos].dX;
            dModelXPos = CMainFrame.LWDicer.m_DataManager.ModelPos.LowerHandlerPos.Pos[nTeachPos].dX;

            dTargetXPos = dFixedXPos + dOffsetXPos + dModelXPos;

            strTargetPos = Convert.ToString(dTargetXPos);
            GridUpHandlerTeachTable[2, 1].Text = strTargetPos;

            dFixedZPos = CMainFrame.LWDicer.m_DataManager.FixedPos.LowerHandlerPos.Pos[nTeachPos].dZ;
            dOffsetZPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[nTeachPos].dZ;
            dModelZPos = CMainFrame.LWDicer.m_DataManager.ModelPos.LowerHandlerPos.Pos[nTeachPos].dZ;

            dTargetZPos = dFixedZPos + dOffsetZPos + dModelZPos;

            strTargetPos = Convert.ToString(dTargetZPos);
            GridUpHandlerTeachTable[2, 2].Text = strTargetPos;

            // FixedPos
            strFixedPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.FixedPos.LowerHandlerPos.Pos[nTeachPos].dX);
            GridUpHandlerTeachTable[3, 1].Text = strFixedPos;

            strFixedPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.FixedPos.LowerHandlerPos.Pos[nTeachPos].dZ);
            GridUpHandlerTeachTable[3, 2].Text = strFixedPos;

            // ModelPos
            strModelPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.ModelPos.LowerHandlerPos.Pos[nTeachPos].dX);
            GridUpHandlerTeachTable[4, 1].Text = strModelPos;

            strModelPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.ModelPos.LowerHandlerPos.Pos[nTeachPos].dZ);
            GridUpHandlerTeachTable[4, 2].Text = strModelPos;

            //OffsetPos
            strOffSetPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[nTeachPos].dX);
            GridUpHandlerTeachTable[6, 1].Text = strOffSetPos;

            strOffSetPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[nTeachPos].dZ);
            GridUpHandlerTeachTable[6, 2].Text = strOffSetPos;
        }

        private void LoadLoTeachingData(int nTeachPos)
        {
            string strFixedPos = string.Empty, strOffSetPos = string.Empty, strTargetPos = string.Empty, strModelPos = string.Empty;
            double dFixedXPos = 0, dOffsetXPos = 0, dTargetXPos = 0, dModelXPos = 0;
            double dFixedZPos = 0, dOffsetZPos = 0, dTargetZPos = 0, dModelZPos = 0;

            dFixedXPos = CMainFrame.LWDicer.m_DataManager.FixedPos.UpperHandlerPos.Pos[nTeachPos].dX;
            dOffsetXPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[nTeachPos].dX;
            dModelXPos = CMainFrame.LWDicer.m_DataManager.ModelPos.UpperHandlerPos.Pos[nTeachPos].dX;

            dTargetXPos = dFixedXPos + dOffsetXPos + dModelXPos;

            strTargetPos = Convert.ToString(dTargetXPos);
            GridLoHandlerTeachTable[2, 1].Text = strTargetPos;

            dFixedZPos = CMainFrame.LWDicer.m_DataManager.FixedPos.UpperHandlerPos.Pos[nTeachPos].dZ;
            dOffsetZPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[nTeachPos].dZ;
            dModelZPos = CMainFrame.LWDicer.m_DataManager.ModelPos.UpperHandlerPos.Pos[nTeachPos].dZ;

            dTargetZPos = dFixedZPos + dOffsetZPos + dModelZPos;

            strTargetPos = Convert.ToString(dTargetZPos);
            GridLoHandlerTeachTable[2, 2].Text = strTargetPos;

            // FixedPos
            strFixedPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.FixedPos.UpperHandlerPos.Pos[nTeachPos].dX);
            GridLoHandlerTeachTable[3, 1].Text = strFixedPos;

            strFixedPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.FixedPos.UpperHandlerPos.Pos[nTeachPos].dZ);
            GridLoHandlerTeachTable[3, 2].Text = strFixedPos;

            // ModelPos
            strModelPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.ModelPos.UpperHandlerPos.Pos[nTeachPos].dX);
            GridLoHandlerTeachTable[4, 1].Text = strModelPos;

            strModelPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.ModelPos.UpperHandlerPos.Pos[nTeachPos].dZ);
            GridLoHandlerTeachTable[4, 2].Text = strModelPos;

            //OffsetPos
            strOffSetPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[nTeachPos].dX);
            GridLoHandlerTeachTable[6, 1].Text = strOffSetPos;

            strOffSetPos = Convert.ToString(CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[nTeachPos].dZ);
            GridLoHandlerTeachTable[6, 2].Text = strOffSetPos;
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

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_X].EncoderPos;
            dTargetPos = Convert.ToDouble(GridUpHandlerTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridUpHandlerTeachTable[8, 1].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_Z].EncoderPos;
            dTargetPos = Convert.ToDouble(GridUpHandlerTeachTable[2, 2].Text);
            dValue = dTargetPos - dCurPos;

            GridUpHandlerTeachTable[8, 2].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_X].EncoderPos;
            dTargetPos = Convert.ToDouble(GridLoHandlerTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridLoHandlerTeachTable[8, 1].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_Z].EncoderPos;
            dTargetPos = Convert.ToDouble(GridLoHandlerTeachTable[2, 2].Text);
            dValue = dTargetPos - dCurPos;

            GridLoHandlerTeachTable[8, 2].Text = Convert.ToString(dValue);
        }

        private void BtnUpChangeValue_Click(object sender, EventArgs e)
        {
            string StrXCurrent = "", StrZCurrent = "", strMsg = string.Empty;
            double dXPos = 0, dOffsetXPos = 0, dTargetXPos = 0;
            double dZPos = 0, dOffsetZPos = 0, dTargetZPos = 0;

            strMsg = TeachUpPos[GetUpPosNo()].Text + " 목표 위치를 현재 위치로 변경하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }

            StrXCurrent = GridUpHandlerTeachTable[7, 1].Text;
            
            dXPos = Convert.ToDouble(StrXCurrent);
            dOffsetXPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[GetUpPosNo()].dX;

            dTargetXPos = dXPos + dOffsetXPos;

            GridUpHandlerTeachTable[2, 1].Text = Convert.ToString(dTargetXPos);

            GridUpHandlerTeachTable[3, 1].Text = Convert.ToString(dXPos);
            GridUpHandlerTeachTable[3, 1].TextColor = Color.Red;


            StrZCurrent = GridUpHandlerTeachTable[7, 2].Text;

            dZPos = Convert.ToDouble(StrZCurrent);
            dOffsetZPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[GetUpPosNo()].dZ;

            dTargetZPos = dZPos + dOffsetZPos;

            GridUpHandlerTeachTable[2, 2].Text = Convert.ToString(dTargetZPos);

            GridUpHandlerTeachTable[3, 2].Text = Convert.ToString(dZPos);
            GridUpHandlerTeachTable[3, 2].TextColor = Color.Red;

        }

        private void BtnLoChangeValue_Click(object sender, EventArgs e)
        {
            string StrXCurrent = "", StrZCurrent = "", strMsg = string.Empty;
            double dXPos = 0, dOffsetXPos = 0, dTargetXPos = 0;
            double dZPos = 0, dOffsetZPos = 0, dTargetZPos = 0;

            strMsg = TeachLoPos[GetLoPosNo()].Text + " 목표 위치를 현재 위치로 변경하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }

            StrXCurrent = GridLoHandlerTeachTable[7, 1].Text;

            dXPos = Convert.ToDouble(StrXCurrent);
            dOffsetXPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[GetLoPosNo()].dX;

            dTargetXPos = dXPos + dOffsetXPos;

            GridLoHandlerTeachTable[2, 1].Text = Convert.ToString(dTargetXPos);

            GridLoHandlerTeachTable[3, 1].Text = Convert.ToString(dXPos);
            GridLoHandlerTeachTable[3, 1].TextColor = Color.Red;


            StrZCurrent = GridLoHandlerTeachTable[7, 2].Text;

            dZPos = Convert.ToDouble(StrZCurrent);
            dOffsetZPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[GetLoPosNo()].dZ;

            dTargetZPos = dZPos + dOffsetZPos;

            GridLoHandlerTeachTable[2, 2].Text = Convert.ToString(dTargetZPos);

            GridLoHandlerTeachTable[3, 2].Text = Convert.ToString(dZPos);
            GridLoHandlerTeachTable[3, 2].TextColor = Color.Red;

        }

        private void BtnLoSave_Click(object sender, EventArgs e)
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
                CMainFrame.LWDicer.m_DataManager.FixedPos.LowerHandlerPos.Pos[GetUpPosNo()].dX = Convert.ToDouble(strData);

                strData = GridUpHandlerTeachTable[3, 2].Text;
                CMainFrame.LWDicer.m_DataManager.FixedPos.LowerHandlerPos.Pos[GetUpPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(true, EPositionObject.LOWER_HANDLER);
                CMainFrame.LWDicer.m_DataManager.LoadPositionData(true, EPositionObject.LOWER_HANDLER);
            }

            if (GetDataMode() == OffSetData)
            {
                strData = GridUpHandlerTeachTable[6, 1].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[GetUpPosNo()].dX = Convert.ToDouble(strData);

                strData = GridUpHandlerTeachTable[6, 2].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[GetUpPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(false, EPositionObject.LOWER_HANDLER);
                CMainFrame.LWDicer.m_DataManager.LoadPositionData(false, EPositionObject.LOWER_HANDLER);
            }

            LoadUpTeachingData(GetUpPosNo());
        }

        private void BtnUpSave_Click(object sender, EventArgs e)
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
                CMainFrame.LWDicer.m_DataManager.FixedPos.UpperHandlerPos.Pos[GetLoPosNo()].dX = Convert.ToDouble(strData);

                strData = GridLoHandlerTeachTable[3, 2].Text;
                CMainFrame.LWDicer.m_DataManager.FixedPos.UpperHandlerPos.Pos[GetLoPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(true, EPositionObject.UPPER_HANDLER);
                CMainFrame.LWDicer.m_DataManager.LoadPositionData(true, EPositionObject.UPPER_HANDLER);
            }

            if (GetDataMode() == OffSetData)
            {
                strData = GridLoHandlerTeachTable[6, 1].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[GetLoPosNo()].dX = Convert.ToDouble(strData);

                strData = GridLoHandlerTeachTable[6, 2].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[GetLoPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(false, EPositionObject.UPPER_HANDLER);
                CMainFrame.LWDicer.m_DataManager.LoadPositionData(false, EPositionObject.UPPER_HANDLER);
            }

            LoadLoTeachingData(GetLoPosNo());
        }

        private void GridUpHandlerTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 3 || e.ColIndex == 4) return;

            string StrCurrent = "", strModify = "";
            double dPos = 0, dOffsetPos = 0, dTargetPos = 0;

            if (GetDataMode() == FixedData)
            {
                StrCurrent = GridUpHandlerTeachTable[3, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
                {
                    return;
                }

                dPos = Convert.ToDouble(strModify);

                if (e.ColIndex == 1)
                {
                    dOffsetPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[GetUpPosNo()].dX;

                    dTargetPos = dPos + dOffsetPos;
                }

                if (e.ColIndex == 2)
                {
                    dOffsetPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.LowerHandlerPos.Pos[GetUpPosNo()].dZ;

                    dTargetPos = dPos + dOffsetPos;
                }

                GridUpHandlerTeachTable[2, e.ColIndex].Text = Convert.ToString(dTargetPos);

                GridUpHandlerTeachTable[3, e.ColIndex].Text = strModify;
                GridUpHandlerTeachTable[3, e.ColIndex].TextColor = Color.Red;
            }

            if(GetDataMode() == OffSetData)
            {
                StrCurrent = GridUpHandlerTeachTable[6, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
                {
                    return;
                }

                GridUpHandlerTeachTable[6, e.ColIndex].Text = strModify;
                GridUpHandlerTeachTable[6, e.ColIndex].TextColor = Color.Red;
            }
        }

        private void GridLoHandlerTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 3 || e.ColIndex == 4) return;

            string StrCurrent = "", strModify = "";
            double dPos = 0, dOffsetPos = 0, dTargetPos = 0;

            if(GetDataMode() == FixedData)
            {
                StrCurrent = GridLoHandlerTeachTable[3, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
                {
                    return;
                }

                dPos = Convert.ToDouble(strModify);

                if (e.ColIndex == 1)
                {
                    dOffsetPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[GetUpPosNo()].dX;

                    dTargetPos = dPos + dOffsetPos;
                }

                if (e.ColIndex == 2)
                {
                    dOffsetPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.UpperHandlerPos.Pos[GetUpPosNo()].dZ;

                    dTargetPos = dPos + dOffsetPos;
                }

                GridLoHandlerTeachTable[2, e.ColIndex].Text = Convert.ToString(dTargetPos);

                GridLoHandlerTeachTable[3, e.ColIndex].Text = strModify;
                GridLoHandlerTeachTable[3, e.ColIndex].TextColor = Color.Red;
            }

            if(GetDataMode() == OffSetData)
            {
                StrCurrent = GridLoHandlerTeachTable[6, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
                {
                    return;
                }

                GridLoHandlerTeachTable[6, e.ColIndex].Text = strModify;
                GridLoHandlerTeachTable[6, e.ColIndex].TextColor = Color.Red;
            }
            
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
