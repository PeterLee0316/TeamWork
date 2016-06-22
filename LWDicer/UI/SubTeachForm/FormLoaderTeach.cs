﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LWDicer.Control;
using static LWDicer.Control.DEF_Motion;
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
    public partial class FormLoaderTeach : Form
    {
        const int FixedData = 0;
        const int OffsetData = 1;

        ButtonAdv[] TeachPos = new ButtonAdv[15];

        private int nTeachPos = 0;

        private int nDataMode = 0;

        public FormLoaderTeach()
        {
            InitializeComponent();

            InitGrid();

            ResouceMapping();
        }

        private void FormClose()
        {
            TmrTeach.Stop();
            this.Hide();
        }

        private void FormLoaderTeach_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);

            this.Text = "Loader Part Teaching Screen";

            UpdateTeachPos(0);

            TmrTeach.Enabled = true;
            TmrTeach.Interval = 100;
            TmrTeach.Start();
        }

        private void FormLoaderTeach_FormClosing(object sender, FormClosingEventArgs e)
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

        private void ResouceMapping()
        {
            TeachPos[0] = BtnPos1; TeachPos[1] = BtnPos2; TeachPos[2] = BtnPos3; TeachPos[3] = BtnPos4; TeachPos[4] = BtnPos5;
            TeachPos[5] = BtnPos6; TeachPos[6] = BtnPos7; TeachPos[7] = BtnPos8; TeachPos[8] = BtnPos9; TeachPos[9] = BtnPos10;
            TeachPos[10] = BtnPos11; TeachPos[11] = BtnPos12; TeachPos[12] = BtnPos13; TeachPos[13] = BtnPos14; TeachPos[14] = BtnPos15;

            int i = 0;

            for(i=0;i<15;i++)
            {
                TeachPos[i].Visible = false;
            }

            for (i = 0; i < (int)EElevatorPos.MAX; i++)
            {
                TeachPos[i].Visible = true;

                TeachPos[i].Text = Convert.ToString(EElevatorPos.BOTTOM + i);
            }
        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridTeachTable.Properties.RowHeaders = true;
            GridTeachTable.Properties.ColHeaders = false;

            nCol = 1;
            nRow = 8;

            // Column,Row 개수
            GridTeachTable.ColCount = nCol;
            GridTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridTeachTable.ColWidths.SetSize(i, 150);
            }

            GridTeachTable.ColWidths.SetSize(0, 110);

            for (i = 0; i < nRow + 1; i++)
            {
                GridTeachTable.RowHeights[i] = 40;

            }

            // Text Display
            GridTeachTable[1, 1].CellType = GridCellTypeName.PushButton;

            GridTeachTable[1, 1].CellAppearance = GridCellAppearance.Raised;

            GridTeachTable[1, 1].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);

            GridTeachTable[2, 0].Text = "목표 위치";
            GridTeachTable[3, 0].Text = "고정 좌표";
            GridTeachTable[4, 0].Text = "모델 좌표";
            GridTeachTable[5, 0].Text = "Cell Mark 보정";
            GridTeachTable[6, 0].Text = "Offset 좌표";
            GridTeachTable[7, 0].Text = "현재 위치";
            GridTeachTable[8, 0].Text = "보정값";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridTeachTable[j, i].Font.Bold = true;

                    GridTeachTable[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridTeachTable[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    if (i != 0 && j != 0)
                    {
                        GridTeachTable[j, i].Text = "";
                        GridTeachTable[j, i].TextColor = Color.Black;
                    }
                }
            }

            GridTeachTable.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridTeachTable.ResizeColsBehavior = 0;
            GridTeachTable.ResizeRowsBehavior = 0;

            for (i = 0; i < nCol; i++)
            {
                GridTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridTeachTable[1, i + 1].Description = "";
            }

            GridTeachTable[1, 0].Text = Convert.ToString(ETeachUnit.LOADER);

            GridTeachTable[1, 1].Description = "Z Axis";
            GridTeachTable[1, 1].TextColor = Color.DarkRed;

            // Grid Display Update
            GridTeachTable.Refresh();
        }

        private void BtnPos_Click(object sender, EventArgs e)
        {
            ButtonAdv TeachPos = sender as ButtonAdv;

            UpdateTeachPos(Convert.ToInt16(TeachPos.Tag));
        }

        private void UpdateTeachPos(int PosNo)
        {
            int nCount = 0, i = 0, j = 0;

            nCount = (int)EElevatorPos.MAX;

            for (i = 0; i < nCount; i++)
            {
                TeachPos[i].BackColor = Color.LightYellow;
            }

            for (i = 0; i < GridTeachTable.ColCount + 1; i++)
            {
                for (j = 0; j < GridTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridTeachTable[j, i].TextColor = Color.Black;
                        GridTeachTable[j, i].Text = "";
                    }
                }

                if(GetDataMode() == FixedData)
                {
                    if (i != 0) GridTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridTeachTable[6, i].BackColor = Color.White;
                }

                if (GetDataMode() == OffsetData)
                {
                    if (i != 0) GridTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            TeachPos[PosNo].BackColor = Color.Tan;

            SetPosNo(PosNo);

            LoadTeachingData(PosNo);
        }

        private void GridTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 2 || e.ColIndex == 3 || e.ColIndex == 4) return;

            string StrCurrent = "", strModify = "";
            double dPos = 0, dOffsetPos = 0, dTargetPos = 0;

            if(GetDataMode() == FixedData)
            {
                StrCurrent = GridTeachTable[3, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
                {
                    return;
                }

                dPos = Convert.ToDouble(strModify);
                dOffsetPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.LoaderPos.Pos[nTeachPos].dZ;

                dTargetPos = dPos + dOffsetPos;
                GridTeachTable[2, e.ColIndex].Text = Convert.ToString(dTargetPos);

                GridTeachTable[3, e.ColIndex].Text = strModify;
                GridTeachTable[3, e.ColIndex].TextColor = Color.Red;
            }

            if(GetDataMode() == OffsetData)
            {
                StrCurrent = GridTeachTable[6, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
                {
                    return;
                }

                GridTeachTable[6, e.ColIndex].Text = strModify;
                GridTeachTable[6, e.ColIndex].TextColor = Color.Red;
            }
        }

        private void SetPosNo(int nPosNo)
        {
            nTeachPos = nPosNo;
        }

        private int GetPosNo()
        {
            return nTeachPos;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty, strData = string.Empty;

            strMsg = GridTeachTable[1, 0].Text + " Unit에 " + TeachPos[GetPosNo()].Text + " Teaching Data를 저장하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }

            if(GetDataMode() == FixedData)
            {
                strData = GridTeachTable[3, 1].Text;
                CMainFrame.LWDicer.m_DataManager.FixedPos.LoaderPos.Pos[GetPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(true, EPositionObject.LOADER);
            }

            if (GetDataMode() == OffsetData)
            {
                strData = GridTeachTable[6, 1].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.LoaderPos.Pos[GetPosNo()].dZ = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(false, EPositionObject.LOADER);
            }
            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.ALL);

            LoadTeachingData(GetPosNo());
        }

        private void TmrTeach_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strCurPos = string.Empty;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.LOADER_Z].EncoderPos);
            GridTeachTable[7, 1].Text = strCurPos;

            // 보정값 Display
            double dValue = 0, dCurPos = 0, dTargetPos = 0;

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.LOADER_Z].EncoderPos;
            dTargetPos = Convert.ToDouble(GridTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridTeachTable[8, 1].Text = Convert.ToString(dValue);
        }

        private void LoadTeachingData(int nTeachPos)
        {
            string str;
            double dFixedPos = 0, dOffsetPos = 0, dTargetPos = 0, dModelPos = 0, dAlignOffset;

            CMovingObject movingObject = CMainFrame.LWDicer.m_MeElevator.AxElevatorInfo;
            dFixedPos    = movingObject.FixedPos.Pos[nTeachPos].dZ;
            dOffsetPos   = movingObject.OffsetPos.Pos[nTeachPos].dZ;
            dModelPos    = movingObject.ModelPos.Pos[nTeachPos].dZ;
            dAlignOffset = movingObject.AlignOffset.dZ;

            dTargetPos = dFixedPos + dOffsetPos + dModelPos + dAlignOffset;

            GridTeachTable[2, 1].Text = Convert.ToString(dTargetPos);
            GridTeachTable[3, 1].Text = Convert.ToString(dFixedPos);
            GridTeachTable[4, 1].Text = Convert.ToString(dModelPos);
            GridTeachTable[5, 1].Text = Convert.ToString(dAlignOffset);
            GridTeachTable[6, 1].Text = Convert.ToString(dOffsetPos);
        }

        private void BtnChangeValue_Click(object sender, EventArgs e)
        {
            string StrCurrent = "", strMsg = string.Empty;
            double dPos = 0, dOffsetPos = 0, dTargetPos = 0;

            strMsg = TeachPos[GetPosNo()].Text + " 목표 위치를 현재 위치로 변경하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }

            StrCurrent = GridTeachTable[7, 1].Text;

            dPos = Convert.ToDouble(StrCurrent);
            dOffsetPos = CMainFrame.LWDicer.m_DataManager.OffsetPos.LoaderPos.Pos[nTeachPos].dZ;

            dTargetPos = dPos + dOffsetPos;
            
            GridTeachTable[2, 1].Text = Convert.ToString(dTargetPos);

            GridTeachTable[3, 1].Text = Convert.ToString(dPos);
            GridTeachTable[3, 1].TextColor = Color.Red;
        }

        private void BtnTeachMove_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty;

            strMsg = TeachPos[GetPosNo()].Text + " 목표 위치로 이동하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg(strMsg))
            {
                return;
            }
        }
    }
}