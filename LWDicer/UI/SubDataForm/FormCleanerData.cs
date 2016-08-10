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
        string[] strOP = new string[(int)ECleanMode.MAX];

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
            GridCtrl.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCtrl.Properties.RowHeaders = true;
            GridCtrl.Properties.ColHeaders = true;

            // Column,Row 개수
            GridCtrl.ColCount = 3;
            GridCtrl.RowCount = DEF_MAX_SPINNER_STEP;

            // Column 가로 크기설정
            GridCtrl.ColWidths.SetSize(0, 50);
            GridCtrl.ColWidths.SetSize(1, 200);
            GridCtrl.ColWidths.SetSize(2, 110);
            GridCtrl.ColWidths.SetSize(3, 110);

            for (i = 0; i < (int)ECleanMode.MAX; i++)
            {
                strOP[i] = Convert.ToString(ECleanMode.NO_USE + i);
            }

            StringCollection strColl = new StringCollection();

            strColl.AddRange(strOP);

            for (i = 0; i < GridCtrl.RowCount + 1; i++)
            {
                GridCtrl.RowHeights[i] = 35;
            }

            for (i = 0; i < GridCtrl.RowCount; i++)
            {
                GridStyleInfo style = GridCtrl.Model[i + 1, 1];

                style.CellType = GridCellTypeName.ComboBox;

                style.ChoiceList = strColl;

                GridCtrl[i + 1, 1].DropDownStyle = GridDropDownStyle.Exclusive;
            }

            GridComboBoxCellModel model = this.GridCtrl.Model.CellModels["ComboBox"] as GridComboBoxCellModel;
            model.ButtonBarSize = new Size(70, 30);

            // Text Display
            GridCtrl[0, 0].Text = "Seq.";
            GridCtrl[0, 1].Text = "Operation";
            GridCtrl[0, 2].Text = "Time [sec]";
            GridCtrl[0, 3].Text = "RPM / min";


            for (i = 0; i < GridCtrl.ColCount + 1; i++)
            {
                for (j = 0; j < GridCtrl.RowCount + 1; j++)
                {
                    // Font Style - Bold
                    GridCtrl[j, i].Font.Bold = true;

                    GridCtrl[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCtrl[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (i = 0; i < GridCtrl.RowCount; i++)
            {
                GridCtrl[i + 1, 2].BackColor = Color.FromArgb(230, 210, 255);
                GridCtrl[i + 1, 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            GridCtrl.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCtrl.ResizeColsBehavior = 0;
            GridCtrl.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCtrl.Refresh();
        }

        private void UpdateData()
        {
            int i;

            for (i = 0; i < DEF_MAX_SPINNER_STEP; i++)
            {
                GridCtrl[i + 1, 1].Text = strOP[(int)CleanerData.SpinnerData.CleanerData.Steps[i].Mode];
                GridCtrl[i + 1, 1].TextColor = Color.Black;

                GridCtrl[i + 1, 2].Text = Convert.ToString(CleanerData.SpinnerData.CleanerData.Steps[i].OpTime);
                GridCtrl[i + 1, 2].TextColor = Color.Black;

                GridCtrl[i + 1, 3].Text = Convert.ToString(CleanerData.SpinnerData.CleanerData.Steps[i].RPMSpeed);
                GridCtrl[i + 1, 3].TextColor = Color.Black;
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
            if (!CMainFrame.InquireMsg("Save Data?"))
            {
                return;
            }

            for (int i = 0; i < DEF_MAX_SPINNER_STEP; i++)
            {
                for (int nOP = 0; nOP < (int)ECleanMode.MAX; nOP++)
                {
                    if (GridCtrl[i + 1, 1].Text == Convert.ToString(ECleanMode.NO_USE + nOP))
                    {
                        CleanerData.SpinnerData.CleanerData.Steps[i].Mode = (ECleanMode.NO_USE + nOP);
                        break;
                    }
                }

                CleanerData.SpinnerData.CleanerData.Steps[i].OpTime = Convert.ToInt16(GridCtrl[i + 1, 2].Text);
                CleanerData.SpinnerData.CleanerData.Steps[i].RPMSpeed = Convert.ToInt16(GridCtrl[i + 1, 3].Text);
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

            strCurrent = GridCtrl[nRow, nCol].Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridCtrl[nRow, nCol].Text = strModify;
            GridCtrl[nRow, nCol].TextColor = Color.Blue;
        }

        private void GridSpinner_CurrentCellShowedDropDown(object sender, EventArgs e)
        {
            GridControlBase grid = sender as GridControlBase;

            if (grid != null)
            {
                GridCurrentCell CurCell = grid.CurrentCell;

                GridCtrl[CurCell.RowIndex, CurCell.ColIndex].TextColor = Color.Blue;
            }
        }

        private void LabelStroke_Click(object sender, EventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = LabelStroke.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            LabelStroke.Text = strModify;
            LabelStroke.ForeColor = Color.Red;

        }
    }
}
