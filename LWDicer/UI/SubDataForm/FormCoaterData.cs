using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using System.Collections.Specialized;
using System.Diagnostics;

using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_CtrlSpinner;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormCoaterData : Form
    {
        string[] strOP = new string[(int)ECoatOperation.MAX];

        private CCoaterData CoaterData;
        private ESpinnerIndex m_SpinnerIndex;

        private GradientLabel[] LabelCoatData = new GradientLabel[6];

        private int selMode;
        
        public FormCoaterData()
        {
            InitializeComponent();

            LabelCoatData[0] = LabelPVAQty; 
            LabelCoatData[1] = LabelMovingPVAQty;
            LabelCoatData[2] = LabelCoatingRate;
            LabelCoatData[3] = LabelCenterWaitTime;
            LabelCoatData[4] = LabelMovingSpeed;
            LabelCoatData[5] = LabelCoatingArea;

            InitGrid();
            this.Text = $"Coater Data [ Current Model : {CMainFrame.DataManager.ModelData.Name} ]";

            for (int i = 0; i < (int)ESpinnerIndex.MAX; i++)
            {
                ComboSpinnerIndex.Items.Add(ESpinnerIndex.SPINNER1 + i);
            }
            ComboSpinnerIndex.SelectedIndex = 0;
        }

        private void InitGrid()
        {
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

            for (int i = 0; i < (int)ECoatOperation.MAX; i++)
            {
                strOP[i] = Convert.ToString(ECoatOperation.NO_USE + i);
            }

            StringCollection strColl = new StringCollection();

            strColl.AddRange(strOP);

            for (int i = 0; i < GridCtrl.RowCount + 1; i++)
            {
                GridCtrl.RowHeights[i] = 35;
            }

            for (int i = 0; i < GridCtrl.RowCount; i++)
            {
                GridStyleInfo style = GridCtrl.Model[i+1,1];

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


            for (int i = 0; i < GridCtrl.ColCount + 1; i++)
            {
                for (int j = 0; j < GridCtrl.RowCount + 1; j++)
                {
                    // Font Style - Bold
                    GridCtrl[j, i].Font.Bold = true;

                    GridCtrl[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCtrl[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            GridCtrl.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCtrl.ResizeColsBehavior = 0;
            GridCtrl.ResizeRowsBehavior = 0;

            for (int i = 0; i < GridCtrl.RowCount; i++)
            {
                GridCtrl[i + 1, 2].BackColor = Color.FromArgb(230, 210, 255);
                GridCtrl[i + 1, 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            // Grid Display Update
            GridCtrl.Refresh();
        }

        private void UpdateData()
        {
            for (int i = 0; i < DEF_MAX_SPINNER_STEP; i++)
            {
                GridCtrl[i + 1, 1].Text = strOP[(int)CoaterData.WorkSteps_Custom[i].Operation];
                GridCtrl[i + 1, 1].TextColor = Color.Black;

                GridCtrl[i + 1, 2].Text = String.Format("{0:0.0}", CoaterData.WorkSteps_Custom[i].OpTime);
                GridCtrl[i + 1, 2].TextColor = Color.Black;

                GridCtrl[i + 1, 3].Text = String.Format("{0:0}", CoaterData.WorkSteps_Custom[i].RPMSpeed);
                GridCtrl[i + 1, 3].TextColor = Color.Black;
            }

            LabelCoatData[0].Text = Convert.ToString(CoaterData.PVAQty);
            LabelCoatData[1].Text = Convert.ToString(CoaterData.MovingPVAQty);
            LabelCoatData[2].Text = Convert.ToString(CoaterData.CoatingRate);
            LabelCoatData[3].Text = Convert.ToString(CoaterData.CenterWaitTime);
            LabelCoatData[4].Text = Convert.ToString(CoaterData.NozzleSpeed);
            LabelCoatData[5].Text = Convert.ToString(CoaterData.CoatingArea);

            for (int i = 0; i <6; i++)
            {
                LabelCoatData[i].ForeColor = Color.Black;
            }

            ComboMode.SelectedIndex = CoaterData.MoveMode;
        }


        private void FormCoaterData_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save data?"))
            {
                return;
            }

            for (int i = 0; i < DEF_MAX_SPINNER_STEP; i++)
            {
                ECoatOperation cnvt = ECoatOperation.NONE;
                try
                {
                    cnvt = (ECoatOperation)Enum.Parse(typeof(ECoatOperation), GridCtrl[i + 1, 1].Text);
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                CoaterData.WorkSteps_Custom[i].Operation = cnvt;
                CoaterData.WorkSteps_Custom[i].OpTime = Convert.ToDouble(GridCtrl[i + 1, 2].Text);
                CoaterData.WorkSteps_Custom[i].RPMSpeed = Convert.ToInt32(GridCtrl[i + 1, 3].Text);
            }

            CoaterData.PVAQty = Convert.ToInt16(LabelCoatData[0].Text);
            CoaterData.MovingPVAQty = Convert.ToInt16(LabelCoatData[1].Text);
            CoaterData.CoatingRate = Convert.ToInt16(LabelCoatData[2].Text);
            CoaterData.CenterWaitTime = Convert.ToInt16(LabelCoatData[3].Text);
            CoaterData.NozzleSpeed = Convert.ToDouble(LabelCoatData[4].Text);
            CoaterData.CoatingArea = Convert.ToDouble(LabelCoatData[5].Text);
            CoaterData.MoveMode = selMode;

            // save
            CMainFrame.DataManager.ModelData.SpinnerData[(int)m_SpinnerIndex].CoaterData = ObjectExtensions.Copy(CoaterData);
            //CMainFrame.LWDicer.SaveModelData(CMainFrame.DataManager.ModelData);
            CMainFrame.LWDicer.SaveModelData(CMainFrame.DataManager.ModelData);

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

        private void LabelData_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        private void ComboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            selMode = (int)ComboMode.SelectedIndex;
        }

        private void ComboSpinnerIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_SpinnerIndex = ESpinnerIndex.SPINNER1 + ComboSpinnerIndex.SelectedIndex;
            CoaterData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData.SpinnerData[(int)m_SpinnerIndex].CoaterData);
            UpdateData();
        }

        private void BtnLoadFrom_Click(object sender, EventArgs e)
        {
            ESpinnerIndex index = (m_SpinnerIndex == ESpinnerIndex.SPINNER1) ? ESpinnerIndex.SPINNER2 : ESpinnerIndex.SPINNER1;
            CoaterData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData.SpinnerData[(int)index].CoaterData);

            UpdateData();
        }
    }
}
