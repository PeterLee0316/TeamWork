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

using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_MeStage;
using static Core.Layers.DEF_ACS;


using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

namespace Core.UI
{
    public partial class FormScannerTeach : Form
    {
        ButtonAdv[] TeachPos = new ButtonAdv[15];

        private int m_nSelectedPos = 0;

        public bool Type_Fixed; // 고정좌표, 옵셋좌표 구분

        private CMovingObject MO_Scanner = CMainFrame.mCore.m_MeStage.AxScannerInfo;

        public FormScannerTeach()
        {
            InitializeComponent();

            InitGrid();

            ResouceMapping();
        }

        private void FormScannerTeach_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(10, 128);

            this.Text = "Scanner Part Teaching Screen";

            UpdateTeachPos(0);

            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        private void FormScannerTeach_FormClosing(object sender, FormClosingEventArgs e)
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

        private void ResouceMapping()
        {
            TeachPos[0] = BtnPos1; TeachPos[1] = BtnPos2; TeachPos[2] = BtnPos3; TeachPos[3] = BtnPos4; TeachPos[4] = BtnPos5;
            TeachPos[5] = BtnPos6; TeachPos[6] = BtnPos7; TeachPos[7] = BtnPos8; TeachPos[8] = BtnPos9; TeachPos[9] = BtnPos10;
            TeachPos[10] = BtnPos11; TeachPos[11] = BtnPos12; TeachPos[12] = BtnPos13; TeachPos[13] = BtnPos14; TeachPos[14] = BtnPos15;

            for (int i = 0; i < 15; i++)
            {
                TeachPos[i].Visible = false;
            }

            for (int i = 0; i < (int)EScannerPos.MAX; i++)
            {
                TeachPos[i].Visible = true;
                TeachPos[i].Text = Convert.ToString(EScannerPos.WAIT + i);
            }
        }

        private void InitGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridTeachTable.Properties.RowHeaders = true;
            GridTeachTable.Properties.ColHeaders = false;

            int nCol = 1;
            int nRow = 8;

            // Column,Row 개수
            GridTeachTable.ColCount = nCol;
            GridTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                GridTeachTable.ColWidths.SetSize(i, 150);
            }

            GridTeachTable.ColWidths.SetSize(0, 110);

            for (int i = 0; i < nRow + 1; i++)
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

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
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

            for (int i = 0; i < nCol; i++)
            {
                GridTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridTeachTable[1, i + 1].Description = "";
            }

            GridTeachTable[1, 0].Text = Convert.ToString(ETeachUnit.SCANNER);

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

        private void UpdateTeachPos(int selectedPos)
        {
            int nCount = (int)EScannerPos.MAX;

            for (int i = 0; i < nCount; i++)
            {
                TeachPos[i].BackColor = Color.LightYellow;
            }

            for (int i = 0; i < GridTeachTable.ColCount + 1; i++)
            {
                for (int j = 0; j < GridTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridTeachTable[j, i].TextColor = Color.Black;
                        GridTeachTable[j, i].Text = "";
                    }
                }

                if (Type_Fixed == true)
                {
                    if (i != 0) GridTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridTeachTable[6, i].BackColor = Color.White;
                }
                else
                {
                    if (i != 0) GridTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            TeachPos[selectedPos].BackColor = Color.Tan;

            m_nSelectedPos = selectedPos;

            DisplayPos();
        }

        private void GridTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 2 || e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";

            strCurrent = GridTeachTable[2, e.ColIndex].Text;
            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            ChangeTargetPos(strModify, e.ColIndex);
        }

        private void ChangeTargetPos(string strTarget, int index)
        {
            double dTargetPos = Convert.ToDouble(strTarget);
            double dOtherSum = Convert.ToDouble(GridTeachTable[4, index].Text) // Model Pos
                + Convert.ToDouble(GridTeachTable[5, index].Text); // + Align Mark Pos

            if (Type_Fixed == true)
            {
                dOtherSum += Convert.ToDouble(GridTeachTable[6, index].Text); // Offset Pos
                double dPos = dTargetPos - dOtherSum;
                GridTeachTable[2, index].Text = String.Format("{0:0.0000}", dTargetPos);
                GridTeachTable[3, index].Text = String.Format("{0:0.0000}", dPos);
                GridTeachTable[3, index].TextColor = Color.Blue;
            }
            else
            {
                dOtherSum += Convert.ToDouble(GridTeachTable[3, index].Text); // Fixed Pos
                double dPos = dTargetPos - dOtherSum;
                GridTeachTable[2, index].Text = String.Format("{0:0.0000}", dTargetPos);
                GridTeachTable[6, index].Text = String.Format("{0:0.0000}", dPos);
                GridTeachTable[6, index].TextColor = Color.Blue;
            }
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save teaching data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            CPositionGroup tGroup;
            CMainFrame.mCore.GetPositionGroup(out tGroup, Type_Fixed);
            EPositionObject pIndex = EPositionObject.SCANNER1;
            int direction = DEF_Z;
            strData = (Type_Fixed == true) ? GridTeachTable[3, 1].Text : strData = GridTeachTable[6, 1].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos].SetPosition(direction, Convert.ToDouble(strData));

            CMainFrame.mCore.SavePosition(tGroup, Type_Fixed, pIndex);
            DisplayPos();
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strCurPos = string.Empty;
            double dValue = 0, dCurPos = 0, dTargetPos = 0;

#if EQUIP_DICING_DEV
            strCurPos= String.Format("{0:0.000}", CMainFrame.mCore.m_YMC.ServoStatus[(int)EYMC_Axis.SCANNER_Z1].EncoderPos);
            dCurPos = CMainFrame.mCore.m_YMC.ServoStatus[(int)EYMC_Axis.SCANNER_Z1].EncoderPos;
#else
            strCurPos = String.Format("{0:0.0000}", CMainFrame.mCore.m_ACS.ServoStatus[(int)EACS_Axis.SCANNER_Z1].EncoderPos);
            dCurPos = CMainFrame.mCore.m_ACS.ServoStatus[(int)EACS_Axis.SCANNER_Z1].EncoderPos;
#endif
            GridTeachTable[7, 1].Text = strCurPos;

            // 보정값 Display

            dTargetPos = Convert.ToDouble(GridTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridTeachTable[8, 1].Text= String.Format("{0:0.0000}", dValue);
        }

        private void DisplayPos()
        {
            int index = m_nSelectedPos;
            double dFixedPos, dModelPos, dOffsetPos, dAlignOffset, dTargetPos;
            dTargetPos = MO_Scanner.GetPosition(index, DEF_Z, out dFixedPos, out dModelPos, out dOffsetPos, out dAlignOffset);

            GridTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos);
            GridTeachTable[3, 1].Text = String.Format("{0:0.0000}", dFixedPos);
            GridTeachTable[4, 1].Text = String.Format("{0:0.0000}", dModelPos);
            GridTeachTable[5, 1].Text = String.Format("{0:0.0000}", dAlignOffset);
            GridTeachTable[6, 1].Text = String.Format("{0:0.0000}", dOffsetPos);
        }

        private void BtnChangeValue_Click(object sender, EventArgs e)
        {
            string strMsg = "Change target position to current position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            string strCurrent = GridTeachTable[7, 1].Text;
            ChangeTargetPos(strCurrent, 1);
        }

        private void BtnTeachMove_Click(object sender, EventArgs e)
        {
            string strMsg = "Move to selected position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            if (TeachPos[m_nSelectedPos].Text == Convert.ToString(EScannerPos.WAIT)) CMainFrame.mCore.m_ctrlStage1.MoveToScannerWaitPos();
            if (TeachPos[m_nSelectedPos].Text == Convert.ToString(EScannerPos.WORK)) CMainFrame.mCore.m_ctrlStage1.MoveToScannerWorkPos();
            if (TeachPos[m_nSelectedPos].Text == Convert.ToString(EScannerPos.FOCUS_1)) CMainFrame.mCore.m_ctrlStage1.MoveToScannerFocusPos1();
            if (TeachPos[m_nSelectedPos].Text == Convert.ToString(EScannerPos.FOCUS_2)) CMainFrame.mCore.m_ctrlStage1.MoveToScannerFocusPos2();
            if (TeachPos[m_nSelectedPos].Text == Convert.ToString(EScannerPos.FOCUS_3)) CMainFrame.mCore.m_ctrlStage1.MoveToScannerFocusPos3();
        }
    }
}
