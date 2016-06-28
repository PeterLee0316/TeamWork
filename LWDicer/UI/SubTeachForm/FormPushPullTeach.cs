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
    public partial class FormPushPullTeach : Form
    {
        ButtonAdv[] PushPullPos = new ButtonAdv[15];
        ButtonAdv[] CenterPos = new ButtonAdv[15];

        private int nPushPullPos = 0;
        private int nCenterPos = 0;

        private int nDataMode = 0;

        private CMovingObject movingPushPullObject = CMainFrame.LWDicer.m_MePushPull.AxPushPullInfo;
        private CMovingObject[] movingCenterObject = new CMovingObject[(int)ECenterPos.MAX];

        public FormPushPullTeach()
        {
            InitializeComponent();

            for(int i=0;i< (int)ECenterPos.MAX;i++)
            {
                movingCenterObject[i] = CMainFrame.LWDicer.m_MePushPull.AxCenterInfo[i];
            }

            InitCenterXGrid();
            InitPushPullGrid();

            ResouceMapping();
        }

        private void FormClose()
        {
            TmrTeach.Stop();
            this.Hide();
        }

        private void FormPushPullTeach_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);

            this.Text = "Push Pull Part Teaching Screen";

            UpdatePushPullTeachPos(0);
            UpdateCenterTeachPos(0);

            TmrTeach.Enabled = true;
            TmrTeach.Interval = 100;
            TmrTeach.Start();
        }

        private void FormPushPullTeach_FormClosing(object sender, FormClosingEventArgs e)
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
            FormJogOperation dlg = new FormJogOperation();
            dlg.ShowDialog();
        }

        private void BtnPushPullPos_Click(object sender, EventArgs e)
        {
            ButtonAdv TeachPos = sender as ButtonAdv;

            UpdatePushPullTeachPos(Convert.ToInt16(TeachPos.Tag));
        }

        private void BtnCenterPos_Click(object sender, EventArgs e)
        {
            ButtonAdv TeachPos = sender as ButtonAdv;

            UpdateCenterTeachPos(Convert.ToInt16(TeachPos.Tag));
        }

        private void UpdatePushPullTeachPos(int PosNo)
        {
            int nCount = 0, i = 0, j = 0;

            nCount = (int)EPushPullPos.MAX;

            for (i = 0; i < nCount; i++)
            {
                PushPullPos[i].BackColor = Color.LightYellow;
            }

            for (i = 0; i < GridPushPullYTeachTable.ColCount + 1; i++)
            {
                for (j = 0; j < GridPushPullYTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridPushPullYTeachTable[j, i].TextColor = Color.Black;
                        GridPushPullYTeachTable[j, i].Text = "";
                    }
                }

                if (GetDataMode() == FixedData)
                {
                    if (i != 0) GridPushPullYTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridPushPullYTeachTable[6, i].BackColor = Color.White;
                }

                if (GetDataMode() == OffsetData)
                {
                    if (i != 0) GridPushPullYTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridPushPullYTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            PushPullPos[PosNo].BackColor = Color.Tan;

            SetPushPullPosNo(PosNo);

            LoadPushPullTeachingData(PosNo);
        }

        private void UpdateCenterTeachPos(int PosNo)
        {
            int nCount = 0, i = 0, j = 0;

            nCount = (int)ECenterPos.MAX;

            for (i = 0; i < nCount; i++)
            {
                CenterPos[i].BackColor = Color.LightYellow;
            }

            for (i = 0; i < GridCenterXTeachTable.ColCount + 1; i++)
            {
                for (j = 0; j < GridCenterXTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridCenterXTeachTable[j, i].TextColor = Color.Black;
                        GridCenterXTeachTable[j, i].Text = "";
                    }
                }

                if (GetDataMode() == FixedData)
                {
                    if (i != 0) GridCenterXTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridCenterXTeachTable[6, i].BackColor = Color.White;
                }

                if (GetDataMode() == OffsetData)
                {
                    if (i != 0) GridCenterXTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridCenterXTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            CenterPos[PosNo].BackColor = Color.Tan;

            SetCenterPosNo(PosNo);

            LoadCenterTeachingData(PosNo);
        }

        private void SetPushPullPosNo(int nPosNo)
        {
            nPushPullPos = nPosNo;
        }

        private int GetPushPullPosNo()
        {
            return nPushPullPos;
        }

        private void SetCenterPosNo(int nPosNo)
        {
            nCenterPos = nPosNo;
        }

        private int GetCenterPosNo()
        {
            return nCenterPos;
        }

        private void LoadCenterTeachingData(int nTeachPos)
        {
            double dFixedX1Pos = 0, dOffsetX1Pos = 0, dTargetX1Pos = 0, dModelX1Pos = 0, dAlignX1Offset;
            double dFixedX2Pos = 0, dOffsetX2Pos = 0, dTargetX2Pos = 0, dModelX2Pos = 0, dAlignX2Offset;

            dFixedX1Pos = movingCenterObject[0].FixedPos.Pos[nTeachPos].dX;
            dOffsetX1Pos = movingCenterObject[0].OffsetPos.Pos[nTeachPos].dX;
            dModelX1Pos = movingCenterObject[0].ModelPos.Pos[nTeachPos].dX;
            dAlignX1Offset = movingCenterObject[0].AlignOffset.dX;

            dTargetX1Pos = dFixedX1Pos + dOffsetX1Pos + dModelX1Pos + dAlignX1Offset;

            GridCenterXTeachTable[2, 1].Text = Convert.ToString(dTargetX1Pos);

            dFixedX2Pos = movingCenterObject[1].FixedPos.Pos[nTeachPos].dX;
            dOffsetX2Pos = movingCenterObject[1].OffsetPos.Pos[nTeachPos].dX;
            dModelX2Pos = movingCenterObject[1].ModelPos.Pos[nTeachPos].dX;
            dAlignX2Offset = movingCenterObject[1].AlignOffset.dX;

            dTargetX2Pos = dFixedX2Pos + dOffsetX2Pos + dModelX2Pos + dAlignX2Offset;

            GridCenterXTeachTable[2, 2].Text = Convert.ToString(dTargetX2Pos);

            // FixedPos
            GridCenterXTeachTable[3, 1].Text = Convert.ToString(dFixedX1Pos);
            GridCenterXTeachTable[3, 2].Text = Convert.ToString(dFixedX2Pos);

            // ModelPos
            GridCenterXTeachTable[4, 1].Text = Convert.ToString(dModelX1Pos);
            GridCenterXTeachTable[4, 2].Text = Convert.ToString(dModelX2Pos);

            // Align Offset
            GridCenterXTeachTable[5, 1].Text = Convert.ToString(dAlignX1Offset);
            GridCenterXTeachTable[5, 2].Text = Convert.ToString(dAlignX2Offset);

            //OffsetPos
            GridCenterXTeachTable[6, 1].Text = Convert.ToString(dOffsetX1Pos);
            GridCenterXTeachTable[6, 2].Text = Convert.ToString(dOffsetX2Pos);
        }

        private void LoadPushPullTeachingData(int nTeachPos)
        {
            double dFixedPos = 0, dOffsetPos = 0, dTargetPos = 0, dModelPos = 0, dAlignOffset;

            dFixedPos = movingPushPullObject.FixedPos.Pos[nTeachPos].dY;
            dOffsetPos = movingPushPullObject.OffsetPos.Pos[nTeachPos].dY;
            dModelPos = movingPushPullObject.ModelPos.Pos[nTeachPos].dY;
            dAlignOffset = movingPushPullObject.AlignOffset.dY;

            dTargetPos = dFixedPos + dOffsetPos + dModelPos + dAlignOffset;

            GridPushPullYTeachTable[2, 1].Text = Convert.ToString(dTargetPos);

            // FixedPos
            GridPushPullYTeachTable[3, 1].Text = Convert.ToString(dFixedPos);

            // ModelPos
            GridPushPullYTeachTable[4, 1].Text = Convert.ToString(dModelPos);

            // AlignOffsetPos
            GridPushPullYTeachTable[5, 1].Text = Convert.ToString(dAlignOffset);

            //OffsetPos
            GridPushPullYTeachTable[6, 1].Text = Convert.ToString(dOffsetPos);
        }

        private void ResouceMapping()
        {
            CenterPos[0] = BtnXPos1; CenterPos[1] = BtnXPos2; CenterPos[2] = BtnXPos3; CenterPos[3] = BtnXPos4; CenterPos[4] = BtnXPos5;
            CenterPos[5] = BtnXPos6; CenterPos[6] = BtnXPos7; CenterPos[7] = BtnXPos8; CenterPos[8] = BtnXPos9; CenterPos[9] = BtnXPos10;
            CenterPos[10] = BtnXPos11; CenterPos[11] = BtnXPos12; CenterPos[12] = BtnXPos13; CenterPos[13] = BtnXPos14; CenterPos[14] = BtnXPos15;

            PushPullPos[0] = BtnYPos1; PushPullPos[1] = BtnYPos2; PushPullPos[2] = BtnYPos3; PushPullPos[3] = BtnYPos4; PushPullPos[4] = BtnYPos5;
            PushPullPos[5] = BtnYPos6; PushPullPos[6] = BtnYPos7; PushPullPos[7] = BtnYPos8; PushPullPos[8] = BtnYPos9; PushPullPos[9] = BtnYPos10;
            PushPullPos[10] = BtnYPos11; PushPullPos[11] = BtnYPos12; PushPullPos[12] = BtnYPos13; PushPullPos[13] = BtnYPos14; PushPullPos[14] = BtnYPos15;

            int i = 0;

            for(i=0;i<15;i++)
            {
                CenterPos[i].Visible = false;
                PushPullPos[i].Visible = false;
            }

            for (i = 0; i < (int)ECenterPos.MAX; i++)
            {
                CenterPos[i].Visible = true;
                CenterPos[i].Text = Convert.ToString(ECenterPos.WAIT + i);
            }

            for (i = 0; i < (int)EPushPullPos.MAX; i++)
            {
                PushPullPos[i].Visible = true;
                PushPullPos[i].Text = Convert.ToString(EPushPullPos.WAIT + i);
            }
        }

        private void InitCenterXGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridCenterXTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCenterXTeachTable.Properties.RowHeaders = true;
            GridCenterXTeachTable.Properties.ColHeaders = false;

            nCol = 2;
            nRow = 8;

            // Column,Row 개수
            GridCenterXTeachTable.ColCount = nCol;
            GridCenterXTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridCenterXTeachTable.ColWidths.SetSize(i, 172);
            }

            GridCenterXTeachTable.ColWidths.SetSize(0, 110);

            for (i = 0; i < nRow + 1; i++)
            {
                GridCenterXTeachTable.RowHeights[i] = 40;
            }

            // Text Display
            GridCenterXTeachTable[1, 1].CellType = GridCellTypeName.PushButton;
            GridCenterXTeachTable[1, 2].CellType = GridCellTypeName.PushButton;

            GridCenterXTeachTable[1, 1].CellAppearance = GridCellAppearance.Raised;
            GridCenterXTeachTable[1, 2].CellAppearance = GridCellAppearance.Raised;

            GridCenterXTeachTable[1, 1].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);
            GridCenterXTeachTable[1, 2].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);

            GridCenterXTeachTable[2, 0].Text = "목표 위치";
            GridCenterXTeachTable[3, 0].Text = "고정 좌표";
            GridCenterXTeachTable[4, 0].Text = "모델 좌표";
            GridCenterXTeachTable[5, 0].Text = "Cell Mark 보정";
            GridCenterXTeachTable[6, 0].Text = "Offset 좌표";
            GridCenterXTeachTable[7, 0].Text = "현재 위치";
            GridCenterXTeachTable[8, 0].Text = "보정값";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridCenterXTeachTable[j, i].Font.Bold = true;

                    GridCenterXTeachTable[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCenterXTeachTable[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    if (i != 0 && j != 0)
                    {
                        GridCenterXTeachTable[j, i].Text = "";
                        GridCenterXTeachTable[j, i].TextColor = Color.Black;
                    }
                }
            }

            GridCenterXTeachTable.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCenterXTeachTable.ResizeColsBehavior = 0;
            GridCenterXTeachTable.ResizeRowsBehavior = 0;

            for (i = 0; i < nCol; i++)
            {
                GridCenterXTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridCenterXTeachTable[1, i + 1].Description = "";
            }

            GridCenterXTeachTable[1, 0].Text = Convert.ToString(ETeachUnit.PUSHPULL);

            GridCenterXTeachTable[1, 1].Description = "X1 Axis";
            GridCenterXTeachTable[1, 1].TextColor = Color.DarkRed;

            GridCenterXTeachTable[1, 2].Description = "X2 Axis";
            GridCenterXTeachTable[1, 2].TextColor = Color.DarkRed;

            // Grid Display Update
            GridCenterXTeachTable.Refresh();
        }

        private void InitPushPullGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridPushPullYTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridPushPullYTeachTable.Properties.RowHeaders = true;
            GridPushPullYTeachTable.Properties.ColHeaders = false;

            nCol = 1;
            nRow = 8;

            // Column,Row 개수
            GridPushPullYTeachTable.ColCount = nCol;
            GridPushPullYTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridPushPullYTeachTable.ColWidths.SetSize(i, 172);
            }

            GridPushPullYTeachTable.ColWidths.SetSize(0, 110);

            for (i = 0; i < nRow + 1; i++)
            {
                GridPushPullYTeachTable.RowHeights[i] = 40;

            }

            // Text Display
            GridPushPullYTeachTable[1, 1].CellType = GridCellTypeName.PushButton;

            GridPushPullYTeachTable[1, 1].CellAppearance = GridCellAppearance.Raised;

            GridPushPullYTeachTable[1, 1].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);

            GridPushPullYTeachTable[2, 0].Text = "목표 위치";
            GridPushPullYTeachTable[3, 0].Text = "고정 좌표";
            GridPushPullYTeachTable[4, 0].Text = "모델 좌표";
            GridPushPullYTeachTable[5, 0].Text = "Cell Mark 보정";
            GridPushPullYTeachTable[6, 0].Text = "Offset 좌표";
            GridPushPullYTeachTable[7, 0].Text = "현재 위치";
            GridPushPullYTeachTable[8, 0].Text = "보정값";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridPushPullYTeachTable[j, i].Font.Bold = true;

                    GridPushPullYTeachTable[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridPushPullYTeachTable[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    if (i != 0 && j != 0)
                    {
                        GridPushPullYTeachTable[j, i].Text = "";
                        GridPushPullYTeachTable[j, i].TextColor = Color.Black;
                    }
                }

                if (i != 0) GridPushPullYTeachTable[3, i].BackColor = Color.LightYellow;
            }

            GridPushPullYTeachTable.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridPushPullYTeachTable.ResizeColsBehavior = 0;
            GridPushPullYTeachTable.ResizeRowsBehavior = 0;

            for (i = 0; i < 1; i++)
            {
                GridPushPullYTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridPushPullYTeachTable[1, i + 1].Description = "";
            }

            GridPushPullYTeachTable[1, 0].Text = Convert.ToString(ETeachUnit.PUSHPULL);

            GridPushPullYTeachTable[1, 1].Description = "Y Axis";
            GridPushPullYTeachTable[1, 1].TextColor = Color.DarkRed;

            // Grid Display Update
            GridPushPullYTeachTable.Refresh();
        }

        private void GridCenterXTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";
            double dPos = 0, dOffsetPos = 0, dTargetPos = 0;

            if(GetDataMode() == FixedData)
            {
                strCurrent = GridCenterXTeachTable[3, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
                {
                    return;
                }

                dPos = Convert.ToDouble(strModify);

                if (e.ColIndex == 1)
                {
                    dOffsetPos = movingCenterObject[0].OffsetPos.Pos[GetCenterPosNo()].dX;

                    dTargetPos = dPos + dOffsetPos;
                }

                if (e.ColIndex == 2)
                {
                    dOffsetPos = movingCenterObject[1].OffsetPos.Pos[GetCenterPosNo()].dX;

                    dTargetPos = dPos + dOffsetPos;
                }

                GridCenterXTeachTable[2, e.ColIndex].Text = Convert.ToString(dTargetPos);

                GridCenterXTeachTable[3, e.ColIndex].Text = strModify;
                GridCenterXTeachTable[3, e.ColIndex].TextColor = Color.Red;
            }

            if(GetDataMode() == OffsetData)
            {
                strCurrent = GridCenterXTeachTable[6, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
                {
                    return;
                }

                GridCenterXTeachTable[6, e.ColIndex].Text = strModify;
                GridCenterXTeachTable[6, e.ColIndex].TextColor = Color.Red;
            }
        }

        private void GridPushPullYTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 2 || e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";
            double dPos = 0, dOffsetPos = 0, dTargetPos = 0;

            if(GetDataMode() == FixedData)
            {
                strCurrent = GridPushPullYTeachTable[3, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
                {
                    return;
                }

                dPos = Convert.ToDouble(strModify);

                dOffsetPos = movingPushPullObject.OffsetPos.Pos[GetCenterPosNo()].dY;

                dTargetPos = dPos + dOffsetPos;

                GridPushPullYTeachTable[2, e.ColIndex].Text = Convert.ToString(dTargetPos);

                GridPushPullYTeachTable[3, e.ColIndex].Text = strModify;
                GridPushPullYTeachTable[3, e.ColIndex].TextColor = Color.Red;
            }

            if(GetDataMode() == OffsetData)
            {
                strCurrent = GridPushPullYTeachTable[6, e.ColIndex].Text;

                if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
                {
                    return;
                }

                GridPushPullYTeachTable[6, e.ColIndex].Text = strModify;
                GridPushPullYTeachTable[6, e.ColIndex].TextColor = Color.Red;
            }
        }

        private void BtnCenterXSave_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty, strData = string.Empty;

            strMsg = GridCenterXTeachTable[1, 0].Text + " Unit에 " + CenterPos[GetCenterPosNo()].Text + " Teaching Data를 저장하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg("", strMsg))
            {
                return;
            }

            if(GetDataMode() == FixedData)
            {
                strData = GridCenterXTeachTable[3, 1].Text;
                CMainFrame.LWDicer.m_DataManager.FixedPos.Centering1Pos.Pos[GetCenterPosNo()].dX = Convert.ToDouble(strData);

                strData = GridCenterXTeachTable[3, 2].Text;
                CMainFrame.LWDicer.m_DataManager.FixedPos.Centering2Pos.Pos[GetCenterPosNo()].dX = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(true, EPositionObject.PUSHPULL_CENTER1);
                CMainFrame.LWDicer.m_DataManager.SavePositionData(true, EPositionObject.PUSHPULL_CENTER2);
            }

            if(GetDataMode() == OffsetData)
            {
                strData = GridCenterXTeachTable[6, 1].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.Centering1Pos.Pos[GetCenterPosNo()].dX = Convert.ToDouble(strData);

                strData = GridCenterXTeachTable[6, 2].Text;
                CMainFrame.LWDicer.m_DataManager.OffsetPos.Centering2Pos.Pos[GetCenterPosNo()].dX = Convert.ToDouble(strData);

                CMainFrame.LWDicer.m_DataManager.SavePositionData(false, EPositionObject.PUSHPULL_CENTER1);
                CMainFrame.LWDicer.m_DataManager.SavePositionData(false, EPositionObject.PUSHPULL_CENTER2);
            }

            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.PUSHPULL);

            LoadCenterTeachingData(GetCenterPosNo());
        }

        private void BtnPushPullYSave_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty, strData = string.Empty;

            strMsg = GridPushPullYTeachTable[1, 0].Text + " Unit에 " + PushPullPos[GetPushPullPosNo()].Text + " Teaching Data를 저장하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg("", strMsg))
            {
                return;
            }

            if(GetDataMode() == FixedData)
            {
                strData = GridPushPullYTeachTable[3, 1].Text;

                CMainFrame.LWDicer.m_DataManager.FixedPos.PushPullPos.Pos[GetPushPullPosNo()].dY = Convert.ToDouble(strData);
                CMainFrame.LWDicer.m_DataManager.SavePositionData(true, EPositionObject.PUSHPULL);
            }

            if(GetDataMode() == OffsetData)
            {
                strData = GridPushPullYTeachTable[6, 1].Text;

                CMainFrame.LWDicer.m_DataManager.OffsetPos.PushPullPos.Pos[GetPushPullPosNo()].dY = Convert.ToDouble(strData);
                CMainFrame.LWDicer.m_DataManager.SavePositionData(false, EPositionObject.PUSHPULL);
            }

            CMainFrame.LWDicer.SetPositionDataToComponent(EPositionGroup.PUSHPULL);

            LoadPushPullTeachingData(GetPushPullPosNo());
        }

        private void BtnXChangeValue_Click(object sender, EventArgs e)
        {
            string StrX1Current = "", StrX2Current = "", strMsg = string.Empty;
            double dX1Pos = 0, dOffsetX1Pos = 0, dTargetX1Pos = 0;
            double dX2Pos = 0, dOffsetX2Pos = 0, dTargetX2Pos = 0;

            strMsg = CenterPos[GetCenterPosNo()].Text + " 목표 위치를 현재 위치로 변경하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg("", strMsg))
            {
                return;
            }

            StrX1Current = GridCenterXTeachTable[7, 1].Text;

            dX1Pos = Convert.ToDouble(StrX1Current);
            dOffsetX1Pos = movingCenterObject[0].OffsetPos.Pos[GetCenterPosNo()].dX;

            dTargetX1Pos = dX1Pos + dOffsetX1Pos;

            GridCenterXTeachTable[2, 1].Text = Convert.ToString(dTargetX1Pos);

            GridCenterXTeachTable[3, 1].Text = Convert.ToString(dX1Pos);
            GridCenterXTeachTable[3, 1].TextColor = Color.Red;


            StrX2Current = GridCenterXTeachTable[7, 2].Text;

            dX2Pos = Convert.ToDouble(StrX2Current);
            dOffsetX2Pos = movingCenterObject[1].OffsetPos.Pos[GetCenterPosNo()].dX;

            dTargetX2Pos = dX2Pos + dOffsetX2Pos;

            GridCenterXTeachTable[2, 2].Text = Convert.ToString(dTargetX2Pos);

            GridCenterXTeachTable[3, 2].Text = Convert.ToString(dX2Pos);
            GridCenterXTeachTable[3, 2].TextColor = Color.Red;
        }

        private void BtnYChangeValue_Click(object sender, EventArgs e)
        {
            string StrX1Current = "", strMsg = string.Empty;
            double dYPos = 0, dOffsetYPos = 0, dTargetYPos = 0;

            strMsg = PushPullPos[GetPushPullPosNo()].Text + " 목표 위치를 현재 위치로 변경하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg("", strMsg))
            {
                return;
            }

            StrX1Current = GridPushPullYTeachTable[7, 1].Text;

            dYPos = Convert.ToDouble(StrX1Current);
            dOffsetYPos = movingPushPullObject.OffsetPos.Pos[GetCenterPosNo()].dY;

            dTargetYPos = dYPos + dOffsetYPos;

            GridPushPullYTeachTable[2, 1].Text = Convert.ToString(dTargetYPos);

            GridPushPullYTeachTable[3, 1].Text = Convert.ToString(dYPos);
            GridPushPullYTeachTable[3, 1].TextColor = Color.Red;
        }

        private void TmrTeach_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strCurPos = string.Empty;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_X1].EncoderPos);
            GridCenterXTeachTable[7, 1].Text = strCurPos;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_X1].EncoderPos);
            GridCenterXTeachTable[7, 2].Text = strCurPos;

            strCurPos = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_Y].EncoderPos);
            GridPushPullYTeachTable[7, 1].Text = strCurPos;

            // 보정값 Display
            double dValue = 0, dCurPos = 0, dTargetPos = 0;

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_X1].EncoderPos;
            dTargetPos = Convert.ToDouble(GridCenterXTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridCenterXTeachTable[8, 1].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_X1].EncoderPos;
            dTargetPos = Convert.ToDouble(GridCenterXTeachTable[2, 2].Text);
            dValue = dTargetPos - dCurPos;

            GridCenterXTeachTable[8, 2].Text = Convert.ToString(dValue);

            dCurPos = CMainFrame.LWDicer.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_Y].EncoderPos;
            dTargetPos = Convert.ToDouble(GridPushPullYTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridPushPullYTeachTable[8, 1].Text = Convert.ToString(dValue);

        }

        private void BtnXTeachMove_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty;

            strMsg = CenterPos[GetCenterPosNo()].Text + " 목표 위치로 이동하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg("", strMsg))
            {
                return;
            }
        }

        private void BtnYTeachMove_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty;

            strMsg = PushPullPos[GetPushPullPosNo()].Text + " 목표 위치로 이동하시겠습니까?";

            if (!CMainFrame.LWDicer.DisplayMsg("", strMsg))
            {
                return;
            }
        }

        private void BtnManualOP_Click(object sender, EventArgs e)
        {
            var dlg = new FormPushPullManualOP();
            dlg.ShowDialog();
        }
    }
}
