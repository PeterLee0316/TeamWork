using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_MeHandler;
using static LWDicer.Control.DEF_MeElevator;
using static LWDicer.Control.DEF_MePushPull;
using static LWDicer.Control.DEF_MeSpinner;
using static LWDicer.Control.DEF_MeStage;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

namespace LWDicer.UI
{
    public partial class FormTeaching : Form
    {
        enum EMoveOption
        {
            JOG = 0,
            ABS,
            INC,
            MAX,
        }

        //enum EAxis
        //{
        //    X_Axis = 1,
        //    Y_Axis,
        //    T_Axis,
        //    Z_Axis,
        //    MAX
        //}

        private int AssUnit;

        private int SelOption = 0;

        private double TargetPos = 0;

        private int SelAxis = 0;
        private int TeachCount = 0;

        ButtonAdv[] AxisNo = new ButtonAdv[19];
        ButtonAdv[] TeachPos = new ButtonAdv[20];

        private string[,] strTeachPos = new string[(int)ETeachUnit.MAX, 12];

        public FormTeaching()
        {
            InitializeComponent();

            ResouceMapping();

            InitializeForm();
        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridTeachTable.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridTeachTable.Properties.RowHeaders = true;
            GridTeachTable.Properties.ColHeaders = false;

            nCol = 4;
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
            GridTeachTable[1, 2].CellType = GridCellTypeName.PushButton;
            GridTeachTable[1, 3].CellType = GridCellTypeName.PushButton;
            GridTeachTable[1, 4].CellType = GridCellTypeName.PushButton;

            GridTeachTable[1, 1].CellAppearance = GridCellAppearance.Raised;
            GridTeachTable[1, 2].CellAppearance = GridCellAppearance.Raised;
            GridTeachTable[1, 3].CellAppearance = GridCellAppearance.Raised;
            GridTeachTable[1, 4].CellAppearance = GridCellAppearance.Raised;

            GridTeachTable[1, 1].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);
            GridTeachTable[1, 2].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);
            GridTeachTable[1, 3].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);
            GridTeachTable[1, 4].Borders.All = new GridBorder(GridBorderStyle.Solid, Color.DarkSlateBlue, GridBorderWeight.Thin);

            if (GetUnit() == (int)ETeachUnit.LOADER) GridTeachTable[1, 0].Text = "Loader";
            if (GetUnit() == (int)ETeachUnit.PUSHPULL) GridTeachTable[1, 0].Text = "Push Pull";
            if (GetUnit() == (int)ETeachUnit.CLEANER1) GridTeachTable[1, 0].Text = "Spinner 1";
            if (GetUnit() == (int)ETeachUnit.CLEANER2) GridTeachTable[1, 0].Text = "Spinner 2";
            if (GetUnit() == (int)ETeachUnit.HANDLER) GridTeachTable[1, 0].Text = "Handler";
            if (GetUnit() == (int)ETeachUnit.STAGE) GridTeachTable[1, 0].Text = "Stage";
            if (GetUnit() == (int)ETeachUnit.CAMERA) GridTeachTable[1, 0].Text = "Camera";
            if (GetUnit() == (int)ETeachUnit.SCANNER) GridTeachTable[1, 0].Text = "Scanner";
                     
            GridTeachTable[2, 0].Text = "목표 위치";
            GridTeachTable[3, 0].Text = "고정 좌표";
            GridTeachTable[4, 0].Text = "모델 좌표";
            GridTeachTable[5, 0].Text = "Cell Mark 보정";
            GridTeachTable[6, 0].Text = "OffSet 좌표";
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
                }

                if(i != 0) GridTeachTable[3, i].BackColor = Color.LightYellow;
            }

            GridTeachTable.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridTeachTable.ResizeColsBehavior = 0;
            GridTeachTable.ResizeRowsBehavior = 0;

            for(i=0;i<4;i++)
            {
                GridTeachTable[1, i + 1].TextColor = Color.LightGray;
                GridTeachTable[1, i + 1].Description = "";
            }

            if (GetUnit() == (int)ETeachUnit.LOADER || GetUnit() == (int)ETeachUnit.CAMERA || GetUnit() == (int)ETeachUnit.SCANNER)
            {
                GridTeachTable[1, 1].Description = "Z Axis";
                GridTeachTable[1, 1].TextColor = Color.DarkRed; 
            }


            if (GetUnit() == (int)ETeachUnit.PUSHPULL)
            {
                GridTeachTable[1, 1].Description = "Y Axis";
                GridTeachTable[1, 2].Description = "X1 Axis";
                GridTeachTable[1, 3].Description = "X2 Axis";

                GridTeachTable[1, 1].TextColor = Color.DarkRed;
                GridTeachTable[1, 2].TextColor = Color.DarkRed;
                GridTeachTable[1, 3].TextColor = Color.DarkRed;   
            }

            if (GetUnit() == (int)ETeachUnit.CLEANER1 || GetUnit() == (int)ETeachUnit.CLEANER2)
            {
                GridTeachTable[1, 1].Description = "Nozzle 1 Axis";
                GridTeachTable[1, 2].Description = "Nozzle 2 Axis";
                GridTeachTable[1, 3].Description = "Rotate Axis";

                GridTeachTable[1, 1].TextColor = Color.DarkRed;
                GridTeachTable[1, 2].TextColor = Color.DarkRed;
                GridTeachTable[1, 3].TextColor = Color.DarkRed; 
            }

            if (GetUnit() == (int)ETeachUnit.STAGE)
            {
                GridTeachTable[1, 1].Description = "X Axis";
                GridTeachTable[1, 2].Description = "Y Axis";
                GridTeachTable[1, 3].Description = "T Axis";

                GridTeachTable[1, 1].TextColor = Color.DarkRed; 
                GridTeachTable[1, 2].TextColor = Color.DarkRed; 
                GridTeachTable[1, 3].TextColor = Color.DarkRed; 
            }

            if (GetUnit() == (int)ETeachUnit.HANDLER)
            {
                GridTeachTable[1, 1].Description = "Up Y Axis";
                GridTeachTable[1, 2].Description = "Up Z Axis";
                GridTeachTable[1, 3].Description = "Lo Y Axis";
                GridTeachTable[1, 4].Description = "Lo Z Axis";

                GridTeachTable[1, 1].TextColor = Color.DarkRed;
                GridTeachTable[1, 2].TextColor = Color.DarkRed;
                GridTeachTable[1, 3].TextColor = Color.DarkRed;
                GridTeachTable[1, 4].TextColor = Color.DarkRed; 
            }

            // Grid Display Update
            GridTeachTable.Refresh();
        }

        private void ResouceMapping()
        {
            AxisNo[0] = BtnAxis1;
            AxisNo[1] = BtnAxis2;
            AxisNo[2] = BtnAxis3;
            AxisNo[3] = BtnAxis4;
            AxisNo[4] = BtnAxis5;
            AxisNo[5] = BtnAxis6;
            AxisNo[6] = BtnAxis7;
            AxisNo[7] = BtnAxis8;
            AxisNo[8] = BtnAxis9;
            AxisNo[9] = BtnAxis10;
            AxisNo[10] = BtnAxis11;
            AxisNo[11] = BtnAxis12;
            AxisNo[12] = BtnAxis13;
            AxisNo[13] = BtnAxis14;
            AxisNo[14] = BtnAxis15;
            AxisNo[15] = BtnAxis16;
            AxisNo[16] = BtnAxis17;
            AxisNo[17] = BtnAxis18;
            AxisNo[18] = BtnAxis19;


            AxisNo[0].Text = "LIFTER";
            AxisNo[1].Text = "PUSHPULL";
            AxisNo[2].Text = "CENTERING 1";
            AxisNo[3].Text = "CENTERING 2";
            AxisNo[4].Text = "SPINNER 1 ROTATE";
            AxisNo[5].Text = "SPINNER 1 NOZZLE 1";
            AxisNo[6].Text = "SPINNER 1 NOZZLE 2";
            AxisNo[7].Text = "SPINNER 2 ROTATE";
            AxisNo[8].Text = "SPINNER 2 NOZZLE 1";
            AxisNo[9].Text = "SPINNER 2 NOZZLE 2";
            AxisNo[10].Text = "TR HAND 1 Z";
            AxisNo[11].Text = "TR HAND 1 Y";
            AxisNo[12].Text = "TR HAND 2 Z";
            AxisNo[13].Text = "TR HAND 2 Y";
            AxisNo[14].Text = "STAGE X";
            AxisNo[15].Text = "STAGE Y";
            AxisNo[16].Text = "STAGE T";
            AxisNo[17].Text = "SCANNER Z";
            AxisNo[18].Text = "CAMERA Z";

            TeachPos[0] = BtnPos1; TeachPos[1] = BtnPos2; TeachPos[2] = BtnPos3; TeachPos[3] = BtnPos4; TeachPos[4] = BtnPos5;
            TeachPos[5] = BtnPos6; TeachPos[6] = BtnPos7; TeachPos[7] = BtnPos8; TeachPos[8] = BtnPos9; TeachPos[9] = BtnPos10;
            TeachPos[10] = BtnPos11; TeachPos[11] = BtnPos12;

            int i = 0;

            for(i=0;i< (int)EElevatorPos.MAX; i++)
            {
                strTeachPos[(int)ETeachUnit.LOADER, i] = Convert.ToString(EElevatorPos.BOTTOM+i);
            }

            for (i = 0; i < (int)EPushPullPos.MAX; i++)
            {
                strTeachPos[(int)ETeachUnit.PUSHPULL, i] = Convert.ToString(EPushPullPos.WAIT + i);
            }

            for (i = 0; i < (int)ENozzlePos.MAX; i++)
            {
                strTeachPos[(int)ETeachUnit.CLEANER1, i] = Convert.ToString(ENozzlePos.SAFETY + i);
                strTeachPos[(int)ETeachUnit.CLEANER2, i] = Convert.ToString(ENozzlePos.SAFETY + i);
            }

            for (i = 0; i < (int)EHandlerPos.MAX; i++)
            {
                strTeachPos[(int)ETeachUnit.HANDLER, i] = Convert.ToString(EHandlerPos.WAIT + i);
            }

            for (i = 0; i < (int)EStagePos.MAX; i++)
            {
                strTeachPos[(int)ETeachUnit.STAGE, i] = Convert.ToString(EStagePos.WAIT + i);
            }

            for (i = 0; i < (int)ECameraPos.MAX; i++)
            {
                strTeachPos[(int)ETeachUnit.CAMERA, i] = Convert.ToString(ECameraPos.MACRO + i);
            }

            for (i = 0; i < (int)EScannerPos.MAX; i++)
            {
                strTeachPos[(int)ETeachUnit.SCANNER, i] = Convert.ToString(EScannerPos.WAIT + i);
            }
        }

        protected virtual void InitializeForm()
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormClose()
        {
            TmrTeach.Stop();
            this.Hide();
        }

        private void FormTeaching_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);

            SetAxis(0);
            SetOption(EMoveOption.JOG);

            InitGrid();

            InitTechButton();

            TmrTeach.Enabled = true;
            TmrTeach.Interval = 100;
            TmrTeach.Start();
        }

        private void InitTechButton()
        {
            UpdateTeachPos(100);

            if (GetUnit() == (int)ETeachUnit.LOADER) { this.Text = "Loader Part Teaching Screen"; }
            if (GetUnit() == (int)ETeachUnit.PUSHPULL) { this.Text = "Push Pull Part Teaching Screen"; }
            if (GetUnit() == (int)ETeachUnit.CLEANER1) { this.Text = "Spinner 1 Part Teaching Screen"; }
            if (GetUnit() == (int)ETeachUnit.CLEANER2) { this.Text = "Spinner 2 Part Teaching Screen"; }
            if (GetUnit() == (int)ETeachUnit.HANDLER) { this.Text = "Handler Part Teaching Screen"; }
            if (GetUnit() == (int)ETeachUnit.STAGE) { this.Text = "Stage Part Teaching Screen"; }
            if (GetUnit() == (int)ETeachUnit.CAMERA) { this.Text = "Camera Part Teaching Screen"; }
            if (GetUnit() == (int)ETeachUnit.SCANNER) { this.Text = "Scanner Part Teaching Screen"; }

        }

        private void SetTeachCount(int nCount)
        {
            TeachCount = nCount;
        }

        private int GetTeachCount()
        {
            return TeachCount;
        }

        private void FormTeaching_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        public void SetUnit(int Unit)
        {
            AssUnit = Unit;
        }

        public int GetUnit()
        {
            return AssUnit;
        }

        private void BtnAxis_Click(object sender, EventArgs e)
        {
            ButtonAdv Axis = sender as ButtonAdv;

            SetAxis(Convert.ToInt16(Axis.Tag));
        }

        private void SetAxis(int nAxis)
        {
            int i = 0;

            for (i = 0; i < 19; i++)
            {
                AxisNo[i].BackColor = Color.FromArgb(224, 224, 224);
            }

            AxisNo[nAxis].BackColor = Color.YellowGreen;

            SelAxis = nAxis;
        }

        private void SetOption(EMoveOption nOption)
        {
            BtnSelJog.BackColor = Color.FromArgb(224, 224, 224);
            BtnSelAbs.BackColor = Color.FromArgb(224, 224, 224);
            BtnSelInc.BackColor = Color.FromArgb(224, 224, 224);

            if (nOption == EMoveOption.JOG)
            {
                BtnAbsMove.Hide();
                BtnPlus.Show();
                BtnMinus.Show();

                BtnSelJog.BackColor = Color.GreenYellow;

                SelOption = (int)EMoveOption.JOG;
            }

            if(nOption == EMoveOption.ABS)
            {
                BtnAbsMove.Show();
                BtnPlus.Hide();
                BtnMinus.Hide();

                BtnSelAbs.BackColor = Color.GreenYellow;

                SelOption = (int)EMoveOption.ABS;
            }

            if(nOption == EMoveOption.INC)
            {
                BtnAbsMove.Hide();
                BtnPlus.Show();
                BtnMinus.Show();

                BtnSelInc.BackColor = Color.GreenYellow;

                SelOption = (int)EMoveOption.INC;
            }
        }

        private void LabelTarget_Click(object sender, EventArgs e)
        {
            string StrCurrent = "", strModify = "";

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            TargetPos = Convert.ToDouble(strModify);

            LabelTarget.Text = strModify;
        }

        private void BtnSelMoveOption_Click(object sender, EventArgs e)
        {
            ButtonAdv Option = sender as ButtonAdv;

            if(Option.Text == "Jog Move")
            {
                SetOption(EMoveOption.JOG);
            }

            if (Option.Text == "Abs Move")
            {
                SetOption(EMoveOption.ABS);
            }

            if (Option.Text == "Inc Move")
            {
                SetOption(EMoveOption.INC);
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {

        }

        private void BtnPlus_MouseDown(object sender, MouseEventArgs e)
        {
            if(SelOption == (int)EMoveOption.JOG)
            {
                BtnPlus.BackColor = Color.LightGoldenrodYellow;
            }

            if (SelOption == (int)EMoveOption.INC)
            {
                BtnPlus.BackColor = Color.DarkGoldenrod;
            }
        }

        private void BtnPlus_MouseUp(object sender, MouseEventArgs e)
        {
            if (SelOption == (int)EMoveOption.JOG)
            {
                BtnPlus.BackColor = Color.DarkGoldenrod;
            }
        }

        private void BtnMinus_MouseDown(object sender, MouseEventArgs e)
        {
            if (SelOption == (int)EMoveOption.JOG)
            {
                BtnMinus.BackColor = Color.LightGoldenrodYellow;
            }

            if (SelOption == (int)EMoveOption.INC)
            {
                BtnPlus.BackColor = Color.DarkGoldenrod;
            }
        }

        private void BtnMinus_MouseUp(object sender, MouseEventArgs e)
        {
            if (SelOption == (int)EMoveOption.JOG)
            {
                BtnMinus.BackColor = Color.DarkGoldenrod;
            }
        }

        private void BtnAbsMove_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void GridTeachTable_PushButtonClick(object sender, GridCellPushButtonClickEventArgs e)
        {
            if(GetUnit() == (int)ETeachUnit.LOADER || GetUnit() == (int)ETeachUnit.CAMERA || GetUnit() == (int)ETeachUnit.SCANNER)
            {
                if (e.ColIndex == 2 || e.ColIndex == 3 || e.ColIndex == 4) return;
            }

            if (GetUnit() == (int)ETeachUnit.PUSHPULL || GetUnit() == (int)ETeachUnit.CLEANER1 || GetUnit() == (int)ETeachUnit.CLEANER2 || GetUnit() == (int)ETeachUnit.STAGE)
            {
                if (e.ColIndex == 4) return;
            }

            string StrCurrent = "", strModify = "";

            StrCurrent= GridTeachTable[3, e.ColIndex].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            GridTeachTable[3, e.ColIndex].Text = strModify;
        }

        private void BtnPos_Click(object sender, EventArgs e)
        {
            ButtonAdv TeachPos = sender as ButtonAdv;

            UpdateTeachPos(Convert.ToInt16(TeachPos.Tag)); // Teaching Position Button Tag는 100번대
        }

        private void UpdateTeachPos(int PosNo)
        {
            int nCount = 0, i = 0, nTP = 0;

            for (i = 0; i < 12; i++)
            {
                TeachPos[i].Visible = false;
            }

            for (i = 0; i < 12; i++)
            {
                if (strTeachPos[(int)GetUnit(), i] == null)
                {
                    break;
                }
                nCount++;
            }

            SetTeachCount(nCount);

            for (i = 0; i < nCount; i++)
            {
                TeachPos[i].Visible = true;
                TeachPos[i].BackColor = Color.LightYellow;
                TeachPos[i].Text = strTeachPos[(int)GetUnit(), i];
            }

            nTP = PosNo - 100; // Teaching Position Button Tag는 100번대

            TeachPos[nTP].BackColor = Color.Tan;
        }

        private void TmrTeach_Tick(object sender, EventArgs e)
        {


        }

        private void BtnTeachMove_Click(object sender, EventArgs e)
        {

        }

        private void BtnChangeValue_Click(object sender, EventArgs e)
        {

        }
    }
}
