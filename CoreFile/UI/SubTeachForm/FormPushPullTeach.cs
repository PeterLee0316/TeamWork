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
using static Core.Layers.DEF_MeHandler;
using static Core.Layers.DEF_MeElevator;
using static Core.Layers.DEF_MePushPull;
using static Core.Layers.DEF_MeSpinner;
using static Core.Layers.DEF_MeStage;

using static Core.Layers.DEF_Yaskawa;
using static Core.Layers.DEF_ACS;

using MotionYMC;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

namespace Core.UI
{
    public partial class FormPushPullTeach : Form
    {
        ButtonAdv[] Pos_PushPull = new ButtonAdv[15];
        ButtonAdv[] CenterPos = new ButtonAdv[15];

        private int m_nSelectedPos_PushPull = 0;
        private int m_nSelectedPos_Center = 0;

        public bool Type_Fixed; // 고정좌표, 옵셋좌표 구분

        private CMovingObject MO_PushPull = CMainFrame.Core.m_MePushPull.AxPushPullInfo;
        private CMovingObject[] MO_Centering = new CMovingObject[(int)ECenterIndex.MAX];

        public FormPushPullTeach()
        {
            InitializeComponent();

            for (int i = 0; i < (int)ECenterIndex.MAX; i++)
            {
                MO_Centering[i] = CMainFrame.Core.m_MePushPull.AxCenterInfo[i];
            }

            InitCenterXGrid();
            InitPushPullGrid();

            ResouceMapping();
        }

        private void FormPushPullTeach_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(10, 128);

            this.Text = "Push Pull Part Teaching Screen";

            UpdatePushPullTeachPos(0);
            UpdateCenterTeachPos(0);

            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        private void FormPushPullTeach_FormClosing(object sender, FormClosingEventArgs e)
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

        private void UpdatePushPullTeachPos(int selectedPos)
        {
            int nCount = (int)EPushPullPos.MAX;

            for (int i = 0; i < nCount; i++)
            {
                Pos_PushPull[i].BackColor = Color.LightYellow;
            }

            for (int i = 0; i < GridPushPullYTeachTable.ColCount + 1; i++)
            {
                for (int j = 0; j < GridPushPullYTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridPushPullYTeachTable[j, i].TextColor = Color.Black;
                        GridPushPullYTeachTable[j, i].Text = "";
                    }
                }

                if (Type_Fixed == true)
                {
                    if (i != 0) GridPushPullYTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridPushPullYTeachTable[6, i].BackColor = Color.White;
                }
                else
                {
                    if (i != 0) GridPushPullYTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridPushPullYTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            Pos_PushPull[selectedPos].BackColor = Color.Tan;

            m_nSelectedPos_PushPull = selectedPos;
            DisplayPos_PushPull();
        }

        private void UpdateCenterTeachPos(int selectedPos)
        {
            int nCount = (int)ECenterPos.MAX;

            for (int i = 0; i < nCount; i++)
            {
                CenterPos[i].BackColor = Color.LightYellow;
            }

            for (int i = 0; i < GridCenterXTeachTable.ColCount + 1; i++)
            {
                for (int j = 0; j < GridCenterXTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridCenterXTeachTable[j, i].TextColor = Color.Black;
                        GridCenterXTeachTable[j, i].Text = "";
                    }
                }

                if (Type_Fixed == true)
                {
                    if (i != 0) GridCenterXTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridCenterXTeachTable[6, i].BackColor = Color.White;
                }
                else
                {
                    if (i != 0) GridCenterXTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridCenterXTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            CenterPos[selectedPos].BackColor = Color.Tan;

            m_nSelectedPos_Center = selectedPos;
            DisplayPos_Center();
        }
        
        private void DisplayPos_Center()
        {
            int index = m_nSelectedPos_Center;
            double dFixedX1Pos = 0, dOffsetX1Pos = 0, dTargetX1Pos = 0, dModelX1Pos = 0, dAlignX1Offset;
            double dFixedX2Pos = 0, dOffsetX2Pos = 0, dTargetX2Pos = 0, dModelX2Pos = 0, dAlignX2Offset;

            double[] dFixedPos    = new double[(int)ECenterIndex.MAX];
            double[] dModelPos    = new double[(int)ECenterIndex.MAX];
            double[] dOffsetPos   = new double[(int)ECenterIndex.MAX];
            double[] dAlignOffset = new double[(int)ECenterIndex.MAX];
            double[] dTargetPos   = new double[(int)ECenterIndex.MAX];

            int nCIndex = (int)ECenterIndex.LEFT;
            dTargetPos[nCIndex] = MO_Centering[nCIndex].GetPosition(index, DEF_X, out dFixedPos[nCIndex], out dModelPos[nCIndex], out dOffsetPos[nCIndex], out dAlignOffset[nCIndex]);
            nCIndex = (int)ECenterIndex.RIGHT;
            dTargetPos[nCIndex] = MO_Centering[nCIndex].GetPosition(index, DEF_X, out dFixedPos[nCIndex], out dModelPos[nCIndex], out dOffsetPos[nCIndex], out dAlignOffset[nCIndex]);

            // LEFT
            nCIndex = (int)ECenterIndex.LEFT;
            GridCenterXTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos[nCIndex]);
            GridCenterXTeachTable[3, 1].Text = String.Format("{0:0.0000}", dFixedPos[nCIndex]);
            GridCenterXTeachTable[4, 1].Text = String.Format("{0:0.0000}", dModelPos[nCIndex]);
            GridCenterXTeachTable[5, 1].Text = String.Format("{0:0.0000}", dAlignOffset[nCIndex]);
            GridCenterXTeachTable[6, 1].Text = String.Format("{0:0.0000}", dOffsetPos[nCIndex]);

            // RIGHT
            nCIndex = (int)ECenterIndex.RIGHT;
            GridCenterXTeachTable[2, 2].Text = String.Format("{0:0.0000}", dTargetPos[nCIndex]);
            GridCenterXTeachTable[3, 2].Text = String.Format("{0:0.0000}", dFixedPos[nCIndex]);
            GridCenterXTeachTable[4, 2].Text = String.Format("{0:0.0000}", dModelPos[nCIndex]);
            GridCenterXTeachTable[5, 2].Text = String.Format("{0:0.0000}", dAlignOffset[nCIndex]);
            GridCenterXTeachTable[6, 2].Text = String.Format("{0:0.0000}", dOffsetPos[nCIndex]);
        }

        private void DisplayPos_PushPull()
        {
            int index = m_nSelectedPos_PushPull;
            double dFixedPos, dModelPos, dOffsetPos, dAlignOffset, dTargetPos;
            dTargetPos = MO_PushPull.GetPosition(index, DEF_Y, out dFixedPos, out dModelPos, out dOffsetPos, out dAlignOffset);

            GridPushPullYTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos);
            GridPushPullYTeachTable[3, 1].Text = String.Format("{0:0.0000}", dFixedPos);
            GridPushPullYTeachTable[4, 1].Text = String.Format("{0:0.0000}", dModelPos);
            GridPushPullYTeachTable[5, 1].Text = String.Format("{0:0.0000}", dAlignOffset);
            GridPushPullYTeachTable[6, 1].Text = String.Format("{0:0.0000}", dOffsetPos);
        }

        private void ResouceMapping()
        {
            CenterPos[0] = BtnXPos1; CenterPos[1] = BtnXPos2; CenterPos[2] = BtnXPos3; CenterPos[3] = BtnXPos4; CenterPos[4] = BtnXPos5;
            CenterPos[5] = BtnXPos6; CenterPos[6] = BtnXPos7; CenterPos[7] = BtnXPos8; CenterPos[8] = BtnXPos9; CenterPos[9] = BtnXPos10;
            CenterPos[10] = BtnXPos11; CenterPos[11] = BtnXPos12; CenterPos[12] = BtnXPos13; CenterPos[13] = BtnXPos14; CenterPos[14] = BtnXPos15;

            Pos_PushPull[0] = BtnYPos1; Pos_PushPull[1] = BtnYPos2; Pos_PushPull[2] = BtnYPos3; Pos_PushPull[3] = BtnYPos4; Pos_PushPull[4] = BtnYPos5;
            Pos_PushPull[5] = BtnYPos6; Pos_PushPull[6] = BtnYPos7; Pos_PushPull[7] = BtnYPos8; Pos_PushPull[8] = BtnYPos9; Pos_PushPull[9] = BtnYPos10;
            Pos_PushPull[10] = BtnYPos11; Pos_PushPull[11] = BtnYPos12; Pos_PushPull[12] = BtnYPos13; Pos_PushPull[13] = BtnYPos14; Pos_PushPull[14] = BtnYPos15;

            for (int i = 0; i < 15; i++)
            {
                CenterPos[i].Visible = false;
                Pos_PushPull[i].Visible = false;
            }

            for (int i = 0; i < (int)ECenterPos.MAX; i++)
            {
                CenterPos[i].Visible = true;
                CenterPos[i].Text = Convert.ToString(ECenterPos.WAIT + i);
            }

            for (int i = 0; i < (int)EPushPullPos.MAX; i++)
            {
                Pos_PushPull[i].Visible = true;
                Pos_PushPull[i].Text = Convert.ToString(EPushPullPos.WAIT + i);
            }
        }

        private void InitCenterXGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridCenterXTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCenterXTeachTable.Properties.RowHeaders = true;
            GridCenterXTeachTable.Properties.ColHeaders = false;

            int nCol = 2;
            int nRow = 8;

            // Column,Row 개수
            GridCenterXTeachTable.ColCount = nCol;
            GridCenterXTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                GridCenterXTeachTable.ColWidths.SetSize(i, 172);
            }

            GridCenterXTeachTable.ColWidths.SetSize(0, 110);

            for (int i = 0; i < nRow + 1; i++)
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

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
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

            for (int i = 0; i < nCol; i++)
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
            // Cell Click 시 커서가 생성되지 않게함.
            GridPushPullYTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridPushPullYTeachTable.Properties.RowHeaders = true;
            GridPushPullYTeachTable.Properties.ColHeaders = false;

            int nCol = 1;
            int nRow = 8;

            // Column,Row 개수
            GridPushPullYTeachTable.ColCount = nCol;
            GridPushPullYTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                GridPushPullYTeachTable.ColWidths.SetSize(i, 172);
            }

            GridPushPullYTeachTable.ColWidths.SetSize(0, 110);

            for (int i = 0; i < nRow + 1; i++)
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

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
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

            for (int i = 0; i < 1; i++)
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
            double dFixedPos = 0, dOffsetPos = 0, dTargetPos = 0;

            // get modify pos
            strCurrent = GridCenterXTeachTable[2, e.ColIndex].Text;
            if (!CMainFrame.GetKeyPad(strCurrent, out strModify)) return;
            dTargetPos = Convert.ToDouble(strModify);

            //
            if (Type_Fixed == true)
            {
                if (e.ColIndex == 1)
                {
                    dOffsetPos = MO_Centering[0].Pos_Offset.Pos[m_nSelectedPos_Center].dX;
                } else
                {
                    dOffsetPos = MO_Centering[1].Pos_Offset.Pos[m_nSelectedPos_Center].dX;
                }

                dFixedPos = dTargetPos - dOffsetPos;
                GridCenterXTeachTable[2, e.ColIndex].Text = String.Format("{0:0.0000}", dTargetPos);
                GridCenterXTeachTable[3, e.ColIndex].Text = String.Format("{0:0.0000}", dFixedPos);
                GridCenterXTeachTable[3, e.ColIndex].TextColor = Color.Blue;
            }
            else
            {
                if (e.ColIndex == 1)
                {
                    dFixedPos = MO_Centering[0].Pos_Fixed.Pos[m_nSelectedPos_Center].dX;
                }
                else
                {
                    dFixedPos = MO_Centering[1].Pos_Fixed.Pos[m_nSelectedPos_Center].dX;
                }

                dOffsetPos = dTargetPos - dFixedPos;
                GridCenterXTeachTable[2, e.ColIndex].Text = String.Format("{0:0.0000}", dTargetPos);
                GridCenterXTeachTable[6, e.ColIndex].Text = String.Format("{0:0.0000}", dOffsetPos);
                GridCenterXTeachTable[6, e.ColIndex].TextColor = Color.Blue;
            }
        }

        private void GridPushPullYTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 2 || e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";
            double dFixedPos = 0, dOffsetPos = 0, dTargetPos = 0;

            // get modify pos
            strCurrent = GridPushPullYTeachTable[2, e.ColIndex].Text;
            if (!CMainFrame.GetKeyPad(strCurrent, out strModify)) return;
            dTargetPos = Convert.ToDouble(strModify);

            //
            dFixedPos = MO_PushPull.Pos_Fixed.Pos[m_nSelectedPos_PushPull].dY;
            dOffsetPos = MO_PushPull.Pos_Offset.Pos[m_nSelectedPos_PushPull].dY;
            if (Type_Fixed == true)
            {
                dFixedPos = dTargetPos - dOffsetPos;

                GridPushPullYTeachTable[2, e.ColIndex].Text = String.Format("{0:0.0000}", dTargetPos);
                GridPushPullYTeachTable[3, e.ColIndex].Text = String.Format("{0:0.0000}", dFixedPos);
                GridPushPullYTeachTable[3, e.ColIndex].TextColor = Color.Blue;
            }
            else
            {
                dOffsetPos = dTargetPos - dFixedPos;

                GridPushPullYTeachTable[2, e.ColIndex].Text = String.Format("{0:0.0000}", dTargetPos);
                GridPushPullYTeachTable[6, e.ColIndex].Text = String.Format("{0:0.0000}", dOffsetPos);
                GridPushPullYTeachTable[6, e.ColIndex].TextColor = Color.Blue;
            }
        }

        private void BtnCenterXSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save teaching data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            CPositionGroup tGroup;
            CMainFrame.Core.GetPositionGroup(out tGroup, Type_Fixed);
            EPositionObject pIndex = EPositionObject.PUSHPULL_CENTER1;
            int direction = DEF_X;
            strData = (Type_Fixed == true) ? GridCenterXTeachTable[3, 1].Text : strData = GridCenterXTeachTable[6, 1].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_Center].SetPosition(direction, Convert.ToDouble(strData));

            CMainFrame.Core.SavePosition(tGroup, Type_Fixed, pIndex);

            pIndex = EPositionObject.PUSHPULL_CENTER2;
            direction = DEF_X;
            strData = (Type_Fixed == true) ? GridCenterXTeachTable[3, 2].Text : strData = GridCenterXTeachTable[6, 2].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_Center].SetPosition(direction, Convert.ToDouble(strData));

            CMainFrame.Core.SavePosition(tGroup, Type_Fixed, pIndex);
            DisplayPos_Center();
        }

        private void BtnPushPullYSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save teaching data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            CPositionGroup tGroup;
            CMainFrame.Core.GetPositionGroup(out tGroup, Type_Fixed);
            EPositionObject pIndex = EPositionObject.PUSHPULL;
            int direction = DEF_Y;
            strData = (Type_Fixed == true) ? GridPushPullYTeachTable[3, 1].Text : strData = GridPushPullYTeachTable[6, 1].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_PushPull].SetPosition(direction, Convert.ToDouble(strData));

            CMainFrame.Core.SavePosition(tGroup, Type_Fixed, pIndex);
            DisplayPos_PushPull();
        }

        private void BtnXChangeValue_Click(object sender, EventArgs e)
        {
            string strCurrent;
            double dTargetPos, dOffsetPos, dFixedPos;

            string strMsg = "Change target position to current position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            // x1
            strCurrent = GridCenterXTeachTable[7, 1].Text;
            dTargetPos = Convert.ToDouble(strCurrent);
            dFixedPos = MO_Centering[0].Pos_Fixed.Pos[m_nSelectedPos_Center].dX;
            dOffsetPos = MO_Centering[0].Pos_Offset.Pos[m_nSelectedPos_Center].dX;

            if(Type_Fixed == true)
            {
                dFixedPos = dTargetPos - dOffsetPos;
                GridCenterXTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos);
                GridCenterXTeachTable[3, 1].Text = String.Format("{0:0.0000}", dFixedPos);
                GridCenterXTeachTable[3, 1].TextColor = Color.Blue;
            } else
            {
                dOffsetPos = dTargetPos - dFixedPos;
                GridCenterXTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos);
                GridCenterXTeachTable[6, 1].Text = String.Format("{0:0.0000}", dOffsetPos);
                GridCenterXTeachTable[6, 1].TextColor = Color.Blue;
            }


            // x2
            strCurrent = GridCenterXTeachTable[7, 2].Text;
            dTargetPos = Convert.ToDouble(strCurrent);
            dFixedPos = MO_Centering[1].Pos_Fixed.Pos[m_nSelectedPos_Center].dX;
            dOffsetPos = MO_Centering[1].Pos_Offset.Pos[m_nSelectedPos_Center].dX;

            if (Type_Fixed == true)
            {
                dFixedPos = dTargetPos - dOffsetPos;
                GridCenterXTeachTable[2, 2].Text = String.Format("{0:0.0000}", dTargetPos);
                GridCenterXTeachTable[3, 2].Text = String.Format("{0:0.0000}", dFixedPos);
                GridCenterXTeachTable[3, 2].TextColor = Color.Blue;
            }
            else
            {
                dOffsetPos = dTargetPos - dFixedPos;
                GridCenterXTeachTable[2, 2].Text = String.Format("{0:0.0000}", dTargetPos);
                GridCenterXTeachTable[6, 2].Text = String.Format("{0:0.0000}", dOffsetPos);
                GridCenterXTeachTable[6, 2].TextColor = Color.Blue;
            }
        }

        private void BtnYChangeValue_Click(object sender, EventArgs e)
        {
            string strCurrent;
            double dTargetPos, dOffsetPos, dFixedPos;

            string strMsg = "Change target position to current position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            strCurrent = GridPushPullYTeachTable[7, 1].Text;
            dTargetPos = Convert.ToDouble(strCurrent);
            dFixedPos = MO_PushPull.Pos_Fixed.Pos[m_nSelectedPos_PushPull].dY;
            dOffsetPos = MO_PushPull.Pos_Offset.Pos[m_nSelectedPos_PushPull].dY;

            if(Type_Fixed == true)
            {
                dFixedPos = dTargetPos - dOffsetPos;
                GridPushPullYTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos);
                GridPushPullYTeachTable[3, 1].Text = String.Format("{0:0.0000}", dFixedPos);
                GridPushPullYTeachTable[3, 1].TextColor = Color.Blue;
            } else
            {
                dOffsetPos = dTargetPos - dFixedPos;
                GridPushPullYTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos);
                GridPushPullYTeachTable[6, 1].Text = String.Format("{0:0.0000}", dOffsetPos);
                GridPushPullYTeachTable[6, 1].TextColor = Color.Blue;
            }
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strCurPos = string.Empty;

            strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_X1].EncoderPos);
            GridCenterXTeachTable[7, 1].Text = strCurPos;

            strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_X2].EncoderPos);
            GridCenterXTeachTable[7, 2].Text = strCurPos;

            strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_Y].EncoderPos);
            GridPushPullYTeachTable[7, 1].Text = strCurPos;

            // 보정값 Display
            double dValue = 0, dCurPos = 0, dTargetPos = 0;

            dCurPos = CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_X1].EncoderPos;
            dTargetPos = Convert.ToDouble(GridCenterXTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;
            GridCenterXTeachTable[8, 1].Text = String.Format("{0:0.0000}", dValue);

            dCurPos = CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_X2].EncoderPos;
            dTargetPos = Convert.ToDouble(GridCenterXTeachTable[2, 2].Text);
            dValue = dTargetPos - dCurPos;
            GridCenterXTeachTable[8, 2].Text = String.Format("{0:0.0000}", dValue);

            dCurPos = CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.PUSHPULL_Y].EncoderPos;
            dTargetPos = Convert.ToDouble(GridPushPullYTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;
            GridPushPullYTeachTable[8, 1].Text = String.Format("{0:0.0000}", dValue);

        }

        private void BtnXTeachMove_Click(object sender, EventArgs e)
        {
            // 0. ask move?
            string strMsg = "Move to selected position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            // 1. check safety
            if (CMainFrame.Core.IsSafeForAxisMove() == false) return;

            // 2. ask transfer wafer
            bool bDetected;
            int iResult = CMainFrame.Core.m_ctrlPushPull.IsObjectDetected(out bDetected);
            if (iResult != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult);
                return;
            }
            if (bDetected)
            {
                // Check Spinner is ready
            }

            // 3. move
            switch (m_nSelectedPos_Center)
            {
                case (int)ECenterPos.WAIT:
                    iResult = CMainFrame.Core.m_ctrlPushPull.MoveAllCenterUnitToWaitPos();
                    break;
                case (int)ECenterPos.CENTERING:
                    iResult = CMainFrame.Core.m_ctrlPushPull.MoveAllCenterUnitToCenteringPos();
                    break;
            }
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnYTeachMove_Click(object sender, EventArgs e)
        {
            // 0. ask move?
            string strMsg = "Move to selected position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            // 1. check safety
            if (CMainFrame.Core.IsSafeForAxisMove() == false) return;

            // 2. ask transfer wafer
            bool bDetected;
            int iResult = CMainFrame.Core.m_ctrlPushPull.IsObjectDetected(out bDetected);
            if (iResult != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult);
                return;
            }
            bool bTransfer = false;
            if(bDetected)
            {
                strMsg = "Transfer Wafer?";
                if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
            }

            // 3. move
            switch (m_nSelectedPos_PushPull)
            {
                case (int)EPushPullPos.WAIT:
                    iResult = CMainFrame.Core.m_ctrlPushPull.MoveToWaitPos(bTransfer);
                    break;
                case (int)EPushPullPos.LOADER:
                    iResult = CMainFrame.Core.m_ctrlPushPull.MoveToLoaderPos(bTransfer);
                    break;
                case (int)EPushPullPos.HANDLER:
                    iResult = CMainFrame.Core.m_ctrlPushPull.MoveToHandlerPos(bTransfer);
                    break;
                case (int)EPushPullPos.SPINNER1:
                    iResult = CMainFrame.Core.m_ctrlPushPull.MoveToSpinner1Pos(bTransfer);
                    break;
                case (int)EPushPullPos.SPINNER2:
                    iResult = CMainFrame.Core.m_ctrlPushPull.MoveToSpinner2Pos(bTransfer);
                    break;
                case (int)EPushPullPos.TEMP_UNLOAD:
                    iResult = CMainFrame.Core.m_ctrlPushPull.MoveToTempUnloadPos(bTransfer);
                    break;
                case (int)EPushPullPos.RELOAD:
                    iResult = CMainFrame.Core.m_ctrlPushPull.MoveToReloadPos(bTransfer);
                    break;
            }
            CMainFrame.DisplayAlarm(iResult);
        }

        private void BtnManualOP_Click(object sender, EventArgs e)
        {
            var dlg = new FormPushPullManualOP();
            dlg.ShowDialog();
        }
    }
}
