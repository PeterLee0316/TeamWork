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
using static Core.Layers.DEF_CtrlHandler;

using static Core.Layers.DEF_Yaskawa;
using static Core.Layers.DEF_ACS;

using MotionYMC;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

namespace Core.UI
{
    public partial class FormHandlerTeach : Form
    {
        ButtonAdv[] TeachUpPos = new ButtonAdv[4];
        ButtonAdv[] TeachLoPos = new ButtonAdv[4];

        private int m_nSelectedPos_UpperHandler = 0;
        private int m_nSelectedPos_LowerHandler = 0;

        public bool Type_Fixed; // 고정좌표, 옵셋좌표 구분

        private CMovingObject[] MO_Handler = new CMovingObject[(int)EHandlerIndex.MAX];

        public FormHandlerTeach()
        {
            InitializeComponent();

            MO_Handler[(int)EHandlerIndex.LOAD_UPPER] = CMainFrame.Core.m_MeUpperHandler.AxHandlerInfo;
            MO_Handler[(int)EHandlerIndex.UNLOAD_LOWER] = CMainFrame.Core.m_MeLowerHandler.AxHandlerInfo;

            InitUpperHandlerGrid();
            InitLowerHandlerGrid();

            ResouceMapping();
        }

        private void FormHandlerTeach_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(10, 128);

            this.Text = "Handler Part Teaching Screen";

            UpdateUpperTeachPos(0);
            UpdateLowerTeachPos(0);

            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        private void ResouceMapping()
        {
            TeachUpPos[0] = BtnUpPos1; TeachUpPos[1] = BtnUpPos2; TeachUpPos[2] = BtnUpPos3; TeachUpPos[3] = BtnUpPos4; 
            TeachLoPos[0] = BtnLoPos1; TeachLoPos[1] = BtnLoPos2; TeachLoPos[2] = BtnLoPos3; TeachLoPos[3] = BtnLoPos4; 

            for (int i = 0; i < TeachUpPos.Length ; i++)
            {
                TeachLoPos[i].Visible = false;
                TeachUpPos[i].Visible = false;
            }

            for (int i = 0; i < (int)EHandlerPos.MAX; i++)
            {
                TeachUpPos[i].Visible = true;
                TeachLoPos[i].Visible = true;

                TeachUpPos[i].Text = Convert.ToString(EHandlerPos.WAIT + i);
                TeachLoPos[i].Text = Convert.ToString(EHandlerPos.WAIT + i);
            }
        }

        private void InitUpperHandlerGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridUpHandlerTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridUpHandlerTeachTable.Properties.RowHeaders = true;
            GridUpHandlerTeachTable.Properties.ColHeaders = false;

            int nCol = 2;
            int nRow = 8;

            // Column,Row 개수
            GridUpHandlerTeachTable.ColCount = nCol;
            GridUpHandlerTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                GridUpHandlerTeachTable.ColWidths.SetSize(i, 172);
            }

            GridUpHandlerTeachTable.ColWidths.SetSize(0, 110);

            for (int i = 0; i < nRow + 1; i++)
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

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
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

            for (int i = 0; i < nCol; i++)
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
            // Cell Click 시 커서가 생성되지 않게함.
            GridLoHandlerTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridLoHandlerTeachTable.Properties.RowHeaders = true;
            GridLoHandlerTeachTable.Properties.ColHeaders = false;

            int nCol = 2;
            int nRow = 8;

            // Column,Row 개수
            GridLoHandlerTeachTable.ColCount = nCol;
            GridLoHandlerTeachTable.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                GridLoHandlerTeachTable.ColWidths.SetSize(i, 172);
            }

            GridLoHandlerTeachTable.ColWidths.SetSize(0, 110);

            for (int i = 0; i < nRow + 1; i++)
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

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
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

            for (int i = 0; i < nCol; i++)
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

        private void UpdateUpperTeachPos(int selectedPos)
        {
            int nCount = (int)EHandlerPos.MAX;

            for (int i = 0; i < nCount; i++)
            {
                TeachUpPos[i].BackColor = Color.LightYellow;
            }

            for (int i = 0; i < GridUpHandlerTeachTable.ColCount + 1; i++)
            {
                for (int j = 0; j < GridUpHandlerTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridUpHandlerTeachTable[j, i].TextColor = Color.Black;
                        GridUpHandlerTeachTable[j, i].Text = "";
                    }
                }

                if (Type_Fixed == true)
                {
                    if (i != 0) GridUpHandlerTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridUpHandlerTeachTable[6, i].BackColor = Color.White;
                }
                else
                {
                    if (i != 0) GridUpHandlerTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridUpHandlerTeachTable[6, i].BackColor = Color.LightYellow;
                }
            }

            TeachUpPos[selectedPos].BackColor = Color.Tan;

            m_nSelectedPos_UpperHandler = selectedPos;

            DisplayPos_UpperHandler();
        }

        private void UpdateLowerTeachPos(int selectedPos)
        {
            int nCount = (int)EHandlerPos.MAX;

            for (int i = 0; i < nCount; i++)
            {
                TeachLoPos[i].BackColor = Color.LightYellow;
            }

            for (int i = 0; i < GridLoHandlerTeachTable.ColCount + 1; i++)
            {
                for (int j = 0; j < GridLoHandlerTeachTable.RowCount; j++)
                {
                    if (i != 0 && j > 1)
                    {
                        GridLoHandlerTeachTable[j, i].TextColor = Color.Black;
                        GridLoHandlerTeachTable[j, i].Text = "";
                    }
                }

                if (Type_Fixed == true)
                {
                    if (i != 0) GridLoHandlerTeachTable[3, i].BackColor = Color.LightYellow;
                    if (i != 0) GridLoHandlerTeachTable[6, i].BackColor = Color.White;
                }
                else
                {
                    if (i != 0) GridLoHandlerTeachTable[3, i].BackColor = Color.White;
                    if (i != 0) GridLoHandlerTeachTable[6, i].BackColor = Color.LightYellow;
                }

            }

            TeachLoPos[selectedPos].BackColor = Color.Tan;

            m_nSelectedPos_LowerHandler = selectedPos;

            DisplayPos_LowerHandler();
        }

        private void DisplayPos_UpperHandler()
        {
            int index = m_nSelectedPos_UpperHandler;
            double dFixedPos, dOffsetPos, dTargetPos, dModelPos, dAlignOffset;
            int nHIndex = (int)EHandlerIndex.LOAD_UPPER;
            int direction = DEF_X;
            dTargetPos = MO_Handler[nHIndex].GetPosition(index, direction, out dFixedPos, out dModelPos, out dOffsetPos, out dAlignOffset);

            GridUpHandlerTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos);
            GridUpHandlerTeachTable[3, 1].Text = String.Format("{0:0.0000}", dFixedPos);
            GridUpHandlerTeachTable[4, 1].Text = String.Format("{0:0.0000}", dModelPos);
            GridUpHandlerTeachTable[5, 1].Text = String.Format("{0:0.0000}", dAlignOffset);
            GridUpHandlerTeachTable[6, 1].Text = String.Format("{0:0.0000}", dOffsetPos);

            direction = DEF_Z;
            dTargetPos = MO_Handler[nHIndex].GetPosition(index, direction, out dFixedPos, out dModelPos, out dOffsetPos, out dAlignOffset);

            GridUpHandlerTeachTable[2, 2].Text = String.Format("{0:0.0000}", dTargetPos);
            GridUpHandlerTeachTable[3, 2].Text = String.Format("{0:0.0000}", dFixedPos);
            GridUpHandlerTeachTable[4, 2].Text = String.Format("{0:0.0000}", dModelPos);
            GridUpHandlerTeachTable[5, 2].Text = String.Format("{0:0.0000}", dAlignOffset);
            GridUpHandlerTeachTable[6, 2].Text = String.Format("{0:0.0000}", dOffsetPos);
        }

        private void DisplayPos_LowerHandler()
        {
            int index = m_nSelectedPos_LowerHandler;
            double dFixedPos, dOffsetPos, dTargetPos, dModelPos, dAlignOffset;
            int nHIndex = (int)EHandlerIndex.UNLOAD_LOWER;
            int direction = DEF_X;
            dTargetPos = MO_Handler[nHIndex].GetPosition(index, direction, out dFixedPos, out dModelPos, out dOffsetPos, out dAlignOffset);

            GridLoHandlerTeachTable[2, 1].Text = String.Format("{0:0.0000}", dTargetPos);
            GridLoHandlerTeachTable[3, 1].Text = String.Format("{0:0.0000}", dFixedPos);
            GridLoHandlerTeachTable[4, 1].Text = String.Format("{0:0.0000}", dModelPos);
            GridLoHandlerTeachTable[5, 1].Text = String.Format("{0:0.0000}", dAlignOffset);
            GridLoHandlerTeachTable[6, 1].Text = String.Format("{0:0.0000}", dOffsetPos);

            direction = DEF_Z;
            dTargetPos = MO_Handler[nHIndex].GetPosition(index, direction, out dFixedPos, out dModelPos, out dOffsetPos, out dAlignOffset);

            GridLoHandlerTeachTable[2, 2].Text = String.Format("{0:0.0000}", dTargetPos);
            GridLoHandlerTeachTable[3, 2].Text = String.Format("{0:0.0000}", dFixedPos);
            GridLoHandlerTeachTable[4, 2].Text = String.Format("{0:0.0000}", dModelPos);
            GridLoHandlerTeachTable[5, 2].Text = String.Format("{0:0.0000}", dAlignOffset);
            GridLoHandlerTeachTable[6, 2].Text = String.Format("{0:0.0000}", dOffsetPos);
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            // Current Position Display
            string strCurPos = string.Empty;

            strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_X].EncoderPos);
            GridUpHandlerTeachTable[7, 1].Text = strCurPos;

            strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_Z].EncoderPos);
            GridUpHandlerTeachTable[7, 2].Text = strCurPos;

            strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_X].EncoderPos);
            GridLoHandlerTeachTable[7, 1].Text = strCurPos;

            strCurPos = String.Format("{0:0.0000}", CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_Z].EncoderPos);
            GridLoHandlerTeachTable[7, 2].Text = strCurPos;


            // 보정값 Display
            double dValue = 0, dCurPos = 0, dTargetPos = 0;

            dCurPos = CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_X].EncoderPos;
            dTargetPos = Convert.ToDouble(GridUpHandlerTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridUpHandlerTeachTable[8, 1].Text = String.Format("{0:0.0000}", dValue);

            dCurPos = CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.UPPER_HANDLER_Z].EncoderPos;
            dTargetPos = Convert.ToDouble(GridUpHandlerTeachTable[2, 2].Text);
            dValue = dTargetPos - dCurPos;

            GridUpHandlerTeachTable[8, 2].Text = String.Format("{0:0.0000}", dValue);

            dCurPos = CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_X].EncoderPos;
            dTargetPos = Convert.ToDouble(GridLoHandlerTeachTable[2, 1].Text);
            dValue = dTargetPos - dCurPos;

            GridLoHandlerTeachTable[8, 1].Text = String.Format("{0:0.0000}", dValue);

            dCurPos = CMainFrame.Core.m_YMC.ServoStatus[(int)EYMC_Axis.LOWER_HANDLER_Z].EncoderPos;
            dTargetPos = Convert.ToDouble(GridLoHandlerTeachTable[2, 2].Text);
            dValue = dTargetPos - dCurPos;

            GridLoHandlerTeachTable[8, 2].Text = String.Format("{0:0.0000}", dValue);
        }

        private void BtnUpChangeValue_Click(object sender, EventArgs e)
        {
            string strMsg = "Change target position to current position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            string strCurrent = GridUpHandlerTeachTable[7, 1].Text;
            ChangeUpHandlerTargetPos(strCurrent, 1);

            strCurrent = GridUpHandlerTeachTable[7, 2].Text;
            ChangeUpHandlerTargetPos(strCurrent, 2);

        }

        private void BtnLoChangeValue_Click(object sender, EventArgs e)
        {
            string strMsg = "Change target position to current position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            string strCurrent = GridLoHandlerTeachTable[7, 1].Text;
            ChangeLoHandlerTargetPos(strCurrent, 1);

            strCurrent = GridLoHandlerTeachTable[7, 2].Text;
            ChangeLoHandlerTargetPos(strCurrent, 2);

        }

        private void BtnLoSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save teaching data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            CPositionGroup tGroup;
            CMainFrame.Core.GetPositionGroup(out tGroup, Type_Fixed);
            EPositionObject pIndex = EPositionObject.LOWER_HANDLER;
            int direction = DEF_X;
            strData = (Type_Fixed == true) ? GridLoHandlerTeachTable[3, 1].Text : strData = GridLoHandlerTeachTable[6, 1].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_LowerHandler].SetPosition(direction, Convert.ToDouble(strData));

            direction = DEF_Z;
            strData = (Type_Fixed == true) ? GridLoHandlerTeachTable[3, 2].Text : strData = GridLoHandlerTeachTable[6, 2].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_LowerHandler].SetPosition(direction, Convert.ToDouble(strData));

            CMainFrame.Core.SavePosition(tGroup, Type_Fixed, pIndex);
            DisplayPos_LowerHandler();
        }

        private void BtnUpSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save teaching data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            CPositionGroup tGroup;
            CMainFrame.Core.GetPositionGroup(out tGroup, Type_Fixed);
            EPositionObject pIndex = EPositionObject.UPPER_HANDLER;
            int direction = DEF_X;
            strData = (Type_Fixed == true) ? GridUpHandlerTeachTable[3, 1].Text : strData = GridUpHandlerTeachTable[6, 1].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_UpperHandler].SetPosition(direction, Convert.ToDouble(strData));

            direction = DEF_Z;
            strData = (Type_Fixed == true) ? GridUpHandlerTeachTable[3, 2].Text : strData = GridUpHandlerTeachTable[6, 2].Text;
            tGroup.Pos_Array[(int)pIndex].Pos[m_nSelectedPos_UpperHandler].SetPosition(direction, Convert.ToDouble(strData));

            CMainFrame.Core.SavePosition(tGroup, Type_Fixed, pIndex);
            DisplayPos_UpperHandler();
        }

        private void GridUpHandlerTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";

            strCurrent = GridUpHandlerTeachTable[2, e.ColIndex].Text;
            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
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

            if (Type_Fixed == true)
            {
                dOtherSum += Convert.ToDouble(GridUpHandlerTeachTable[6, index].Text); // Offset Pos
                double dPos = dTargetPos - dOtherSum;
                GridUpHandlerTeachTable[2, index].Text = String.Format("{0:0.0000}", dTargetPos);
                GridUpHandlerTeachTable[3, index].Text = String.Format("{0:0.0000}", dPos);
                GridUpHandlerTeachTable[3, index].TextColor = Color.Blue;
            }
            else
            {
                dOtherSum += Convert.ToDouble(GridUpHandlerTeachTable[3, index].Text); // Fixed Pos
                double dPos = dTargetPos - dOtherSum;
                GridUpHandlerTeachTable[2, index].Text = String.Format("{0:0.0000}", dTargetPos);
                GridUpHandlerTeachTable[6, index].Text = String.Format("{0:0.0000}", dPos);
                GridUpHandlerTeachTable[6, index].TextColor = Color.Blue;
            }
        }

        private void ChangeLoHandlerTargetPos(string strTarget, int index)
        {
            double dTargetPos = Convert.ToDouble(strTarget);
            double dOtherSum = Convert.ToDouble(GridLoHandlerTeachTable[4, index].Text) // Model Pos
                + Convert.ToDouble(GridLoHandlerTeachTable[5, index].Text); // + Align Mark Pos

            if (Type_Fixed == true)
            {
                dOtherSum += Convert.ToDouble(GridLoHandlerTeachTable[6, index].Text); // Offset Pos
                double dPos = dTargetPos - dOtherSum;
                GridLoHandlerTeachTable[2, index].Text = String.Format("{0:0.0000}", dTargetPos);
                GridLoHandlerTeachTable[3, index].Text = String.Format("{0:0.0000}", dPos);
                GridLoHandlerTeachTable[3, index].TextColor = Color.Blue;
            }
            else
            {
                dOtherSum += Convert.ToDouble(GridLoHandlerTeachTable[3, index].Text); // Fixed Pos
                double dPos = dTargetPos - dOtherSum;
                GridLoHandlerTeachTable[2, index].Text = String.Format("{0:0.0000}", dTargetPos);
                GridLoHandlerTeachTable[6, index].Text = String.Format("{0:0.0000}", dPos);
                GridLoHandlerTeachTable[6, index].TextColor = Color.Blue;
            }
        }


        private void GridLoHandlerTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if (e.ColIndex == 3 || e.ColIndex == 4) return;

            string strCurrent = "", strModify = "";

            strCurrent = GridLoHandlerTeachTable[2, e.ColIndex].Text;
            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            ChangeLoHandlerTargetPos(strModify, e.ColIndex);
        }

        private void BtnUpTeachMove_Click(object sender, EventArgs e)
        {
            // check button
            Button btn = sender as Button;
            string unit = "UpperHandler_Z";
            string cmd = btn.Tag?.ToString();

            if(cmd != "4" && cmd != "9")
            {
                unit = "UpperHandler_X";
                cmd = $"{m_nSelectedPos_UpperHandler}";
            }
            MoveHandlerToPos(unit, cmd);
        }

        private void BtnLoTeachMove_Click(object sender, EventArgs e)
        {
            // check button
            Button btn = sender as Button;
            string unit = "LowerHandler_Z";
            string cmd = btn.Tag?.ToString();

            if (cmd != "4" && cmd != "9")
            {
                unit = "LowerHandler_X";
                cmd = $"{m_nSelectedPos_LowerHandler}";
            }
            MoveHandlerToPos(unit, cmd);
        }

        private async void MoveHandlerToPos(string unit, string cmd)
        {
            // confirm
            if (CMainFrame.Core.IsSafeForAxisMove() == false) return;
            string strMsg = "Move to selected position?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            // set btn enable
            //btn.BackColor = CMainFrame.BtnBackColor_On;
            SetButtonsEnable(false);

            // do
            int iResult = SUCCESS;
            bool bStatus, bTransfer;
            Task<int> task1 = Task<int>.Run(() => CMainFrame.Core.EmptyMethod());
            CMainFrame.StartTimer();

            ///////////////////////////////////////////////////////////////////////////////
            // UpperHandler
            if (unit == "UpperHandler_X")
            {
                DEF_CtrlHandler.EHandlerIndex index = DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER;
                iResult = CMainFrame.Core.m_ctrlHandler.IsObjectDetected(index, out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "0")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveToWaitPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "1")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveToPushPullPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "2")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveToStagePos(index, bTransfer));
                    iResult = await task1;
                }
            }
            else if (unit == "UpperHandler_Z")
            {
                DEF_CtrlHandler.EHandlerIndex index = DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER;
                iResult = CMainFrame.Core.m_ctrlHandler.IsObjectDetected(index, out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "4")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveZToSafetyPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "9")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveZToLoadUnloadPos(index, bTransfer));
                    iResult = await task1;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////
            // LowerHandler
            else if (unit == "LowerHandler_X")
            {
                DEF_CtrlHandler.EHandlerIndex index = DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER;
                iResult = CMainFrame.Core.m_ctrlHandler.IsObjectDetected(index, out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "0")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveToWaitPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "1")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveToPushPullPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "2")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveToStagePos(index, bTransfer));
                    iResult = await task1;
                }
            }
            else if (unit == "LowerHandler_Z")
            {
                DEF_CtrlHandler.EHandlerIndex index = DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER;
                iResult = CMainFrame.Core.m_ctrlHandler.IsObjectDetected(index, out bStatus);
                if (iResult != SUCCESS) goto ERROR_OCCURED;
                bTransfer = false;
                if (bStatus)
                {
                    strMsg = "Wafer is detected. Take wafer to move?";
                    if (CMainFrame.InquireMsg(strMsg)) bTransfer = true;
                }

                if (cmd == "4")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveZToSafetyPos(index, bTransfer));
                    iResult = await task1;
                }
                else if (cmd == "9")
                {
                    task1 = Task<int>.Run(() => CMainFrame.Core.m_ctrlHandler.MoveZToLoadUnloadPos(index, bTransfer));
                    iResult = await task1;
                }
            }

            ERROR_OCCURED:

            LabelTime_Value.Text = CMainFrame.GetElapsedTIme_Text();

            // set btn enable
            //btn.BackColor = CMainFrame.BtnBackColor_Off;
            SetButtonsEnable(true);

            // display alarm
            CMainFrame.DisplayAlarm(iResult);
        }

        private void SetButtonsEnable(bool bEnable)
        {
            CMainFrame.MainFrame.BottomScreen.EnableBottomPage(bEnable);

            var btns = CMainFrame.MainFrame.GetAllControl(this, typeof(Syncfusion.Windows.Forms.ButtonAdv));
            foreach (var btn in btns)
            {
                Syncfusion.Windows.Forms.ButtonAdv abtn = btn as Syncfusion.Windows.Forms.ButtonAdv;
                abtn.Enabled = bEnable;
            }
            btns = CMainFrame.MainFrame.GetAllControl(this, typeof(System.Windows.Forms.Button));
            foreach (var btn in btns)
            {
                System.Windows.Forms.Button abtn = btn as System.Windows.Forms.Button;
                abtn.Enabled = bEnable;
            }
            btnStopAction.Enabled = true;
        }

        private void BtnManualOP_Click(object sender, EventArgs e)
        {
            Button Handler = sender as Button;
            var dlg = new FormHandlerManualOP();

            if(Handler.Name == "BtnUpperManualOP")
            {
                dlg.SetHandler(DEF_CtrlHandler.EHandlerIndex.LOAD_UPPER);
            } else
            {
                dlg.SetHandler(DEF_CtrlHandler.EHandlerIndex.UNLOAD_LOWER);
            }
            dlg.ShowDialog();
        }

        private void btnStopAction_Click(object sender, EventArgs e)
        {
            MYaskawa.IsCancelJob_byManual = true;
        }
    }
}
