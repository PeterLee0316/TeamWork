using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections.Specialized;

using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;

using static LWDicer.Control.DEF_CtrlSpinner;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormCleanerData : Form
    {
        string[] strOP = new string[(int)ENozzleOpMode.MAX];

        private CModelData CleanerData;

        public FormCleanerData()
        {
            InitializeComponent();

            CleanerData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData);

            InitGrid();

            UpdateData();

            this.Text = $"Cleaner Data [ Current Model : {CMainFrame.DataManager.ModelData.Name} ]";
        }

        private void InitGrid()
        {
            //GridSpinner
            int i = 0, j = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridSpinner.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridSpinner.Properties.RowHeaders = true;
            GridSpinner.Properties.ColHeaders = true;

            // Column,Row 개수
            GridSpinner.ColCount = 3;
            GridSpinner.RowCount = DEF_MAX_SPINNER_STEP;

            // Column 가로 크기설정
            GridSpinner.ColWidths.SetSize(0, 50);
            GridSpinner.ColWidths.SetSize(1, 200);
            GridSpinner.ColWidths.SetSize(2, 110);
            GridSpinner.ColWidths.SetSize(3, 110);

            for (i = 0; i < (int)ENozzleOpMode.MAX; i++)
            {
                strOP[i] = Convert.ToString(ENozzleOpMode.WAIT + i);
            }

            StringCollection strColl = new StringCollection();

            strColl.AddRange(strOP);

            for (i = 0; i < GridSpinner.RowCount + 1; i++)
            {
                GridSpinner.RowHeights[i] = 35;
            }

            for (i = 0; i < GridSpinner.RowCount; i++)
            {
                GridStyleInfo style = GridSpinner.Model[i + 1, 1];

                style.CellType = GridCellTypeName.ComboBox;

                style.ChoiceList = strColl;

                GridSpinner[i + 1, 1].DropDownStyle = GridDropDownStyle.Exclusive;
            }

            GridComboBoxCellModel model = this.GridSpinner.Model.CellModels["ComboBox"] as GridComboBoxCellModel;
            model.ButtonBarSize = new Size(70, 30);

            // Text Display
            GridSpinner[0, 0].Text = "Seq.";
            GridSpinner[0, 1].Text = "Operation";
            GridSpinner[0, 2].Text = "Time [sec]";
            GridSpinner[0, 3].Text = "RPM / min";


            for (i = 0; i < GridSpinner.ColCount + 1; i++)
            {
                for (j = 0; j < GridSpinner.RowCount + 1; j++)
                {
                    // Font Style - Bold
                    GridSpinner[j, i].Font.Bold = true;

                    GridSpinner[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridSpinner[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (i = 0; i < GridSpinner.RowCount; i++)
            {
                GridSpinner[i + 1, 2].BackColor = Color.FromArgb(230, 210, 255);
                GridSpinner[i + 1, 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            GridSpinner.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridSpinner.ResizeColsBehavior = 0;
            GridSpinner.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridSpinner.Refresh();
        }

        private void UpdateData()
        {
            int i;

            for (i = 0; i < DEF_MAX_SPINNER_STEP; i++)
            {
                GridSpinner[i + 1, 1].Text = strOP[(int)CleanerData.SpinnerData.CleanerData.StepSequence[i].OpMode];
                GridSpinner[i + 1, 1].TextColor = Color.Black;

                GridSpinner[i + 1, 2].Text = Convert.ToString(CleanerData.SpinnerData.CleanerData.StepSequence[i].OpTime);
                GridSpinner[i + 1, 2].TextColor = Color.Black;

                GridSpinner[i + 1, 3].Text = Convert.ToString(CleanerData.SpinnerData.CleanerData.StepSequence[i].RPMSpeed);
                GridSpinner[i + 1, 3].TextColor = Color.Black;
            }

            LabelStroke.Text = Convert.ToString(CleanerData.SpinnerData.CleanerData.WashStroke);
            LabelStroke.ForeColor = Color.Black;
        }


        private void FormClose()
        {
            this.Hide();
        }

        private void FormCleanerData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("Save Data?"))
            {
                return;
            }

            int i, nOP;

            for (i = 0; i < DEF_MAX_SPINNER_STEP; i++)
            {
                for (nOP = 0; nOP < (int)ENozzleOpMode.MAX; nOP++)
                {
                    if (GridSpinner[i + 1, 1].Text == Convert.ToString(ENozzleOpMode.WAIT + nOP))
                    {
                        CleanerData.SpinnerData.CleanerData.StepSequence[i].OpMode = (ENozzleOpMode.WAIT + nOP);
                        break;
                    }
                }

                CleanerData.SpinnerData.CleanerData.StepSequence[i].OpTime = Convert.ToInt16(GridSpinner[i + 1, 2].Text);
                CleanerData.SpinnerData.CleanerData.StepSequence[i].RPMSpeed = Convert.ToInt16(GridSpinner[i + 1, 3].Text);
            }

            CleanerData.SpinnerData.CleanerData.WashStroke = Convert.ToDouble(LabelStroke.Text);

            CMainFrame.DataManager.SaveModelData(CleanerData);

            UpdateData();

        }

        private void GridSpinner_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol == 0 || nCol == 1 || nRow == 0)
            {
                return;
            }

            strCurrent = GridSpinner[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridSpinner[nRow, nCol].Text = strModify;
            GridSpinner[nRow, nCol].TextColor = Color.Red;
        }

        private void GridSpinner_CurrentCellShowedDropDown(object sender, EventArgs e)
        {
            GridControlBase grid = sender as GridControlBase;

            if (grid != null)
            {
                GridCurrentCell CurCell = grid.CurrentCell;

                GridSpinner[CurCell.RowIndex, CurCell.ColIndex].TextColor = Color.Red;
            }
        }

        private void LabelStroke_Click(object sender, EventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = LabelStroke.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            LabelStroke.Text = strModify;
            LabelStroke.ForeColor = Color.Red;

        }
    }
}
