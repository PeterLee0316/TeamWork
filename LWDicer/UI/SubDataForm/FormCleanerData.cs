using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using Syncfusion.Windows.Forms.Tools;
using System.Collections.Specialized;

using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_CtrlSpinner;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormCleanerData : Form
    {
        string[] strOP = new string[(int)ECleanOperation.MAX];

        private CCleanerData CleanerData;
        private ESpinnerIndex m_SpinnerIndex;

        public FormCleanerData()
        {
            InitializeComponent();

            InitGrid();
            this.Text = $"Cleaner Data [ Current Model : {CMainFrame.DataManager.ModelData.Name} ]";

            for(int i = 0; i < (int)ESpinnerIndex.MAX; i++)
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

            for (int i = 0; i < (int)ECleanOperation.MAX; i++)
            {
                strOP[i] = Convert.ToString(ECleanOperation.NO_USE + i);
            }

            StringCollection strColl = new StringCollection();

            strColl.AddRange(strOP);

            for (int i = 0; i < GridCtrl.RowCount + 1; i++)
            {
                GridCtrl.RowHeights[i] = 35;
            }

            for (int i = 0; i < GridCtrl.RowCount; i++)
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

            for (int i = 0; i < GridCtrl.RowCount; i++)
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
            for (int i = 0; i < DEF_MAX_SPINNER_STEP; i++)
            {
                GridCtrl[i + 1, 1].Text = strOP[(int)CleanerData.WorkSteps_Custom[i].Operation];
                GridCtrl[i + 1, 1].TextColor = Color.Black;

                GridCtrl[i + 1, 2].Text = String.Format("{0:0.0}", CleanerData.WorkSteps_Custom[i].OpTime);
                GridCtrl[i + 1, 2].TextColor = Color.Black;

                GridCtrl[i + 1, 3].Text = String.Format("{0:0}", CleanerData.WorkSteps_Custom[i].RPMSpeed);
                GridCtrl[i + 1, 3].TextColor = Color.Black;
            }

            // Work Washing
            LabelTime_PreWashing.Text = Convert.ToString(CleanerData.WorkSteps_General[0].OpTime);
            LabelTime_Washing.Text    = Convert.ToString(CleanerData.WorkSteps_General[1].OpTime);
            LabelTime_Rinsing.Text    = Convert.ToString(CleanerData.WorkSteps_General[2].OpTime);
            LabelTime_Drying.Text     = Convert.ToString(CleanerData.WorkSteps_General[3].OpTime);

            LabelRPM_PreWashing.Text = Convert.ToString(CleanerData.WorkSteps_General[0].RPMSpeed);
            LabelRPM_Washing.Text    = Convert.ToString(CleanerData.WorkSteps_General[1].RPMSpeed);
            LabelRPM_Rinsing.Text    = Convert.ToString(CleanerData.WorkSteps_General[2].RPMSpeed);
            LabelRPM_Drying.Text     = Convert.ToString(CleanerData.WorkSteps_General[3].RPMSpeed);

            // Table Washing
            LabelTime_TableWashing.Text = Convert.ToString(CleanerData.TableSteps[0].OpTime);
            LabelTime_TableDrying.Text = Convert.ToString(CleanerData.TableSteps[1].OpTime);

            LabelRPM_TableWashing.Text = Convert.ToString(CleanerData.TableSteps[0].RPMSpeed);
            LabelRPM_TableDrying.Text = Convert.ToString(CleanerData.TableSteps[1].RPMSpeed);

            checkBox_EnableTableWashing.Checked = CleanerData.EnableThoroughCleaning;
            Label_NozzleSpeed.Text = Convert.ToString(CleanerData.NozzleSpeed);

            // Case / Disk Washing
            LabelTime_CaseWashing.Text = Convert.ToString(CleanerData.CaseSteps[0].OpTime);
            LabelTime_DiskWashing.Text = Convert.ToString(CleanerData.DiskSteps[0].OpTime);

            LabelRPM_CaseWashing.Text = Convert.ToString(CleanerData.CaseSteps[0].RPMSpeed);
            LabelRPM_DiskWashing.Text = Convert.ToString(CleanerData.DiskSteps[0].RPMSpeed);

            // other
            LabelStroke.Text = Convert.ToString(CleanerData.WashStroke);
            LabelStroke.ForeColor = Color.Black;

            checkBox_UseCommon.Checked = (CleanerData.UseWashSteps_General) ? true : false;
            checkBox_UseCustom.Checked = !checkBox_UseCommon.Checked;
        }


        private void FormCleanerData_FormClosing(object sender, FormClosingEventArgs e)
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
                ECleanOperation cnvt = ECleanOperation.NONE;
                try
                {
                    cnvt = (ECleanOperation)Enum.Parse(typeof(ECleanOperation), GridCtrl[i + 1, 1].Text);
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                CleanerData.WorkSteps_Custom[i].Operation = cnvt;
                CleanerData.WorkSteps_Custom[i].OpTime = Convert.ToDouble(GridCtrl[i + 1, 2].Text);
                CleanerData.WorkSteps_Custom[i].RPMSpeed = Convert.ToInt32(GridCtrl[i + 1, 3].Text);
            }


            // Work Washing
            CleanerData.WorkSteps_General[0].OpTime = Convert.ToDouble(LabelTime_PreWashing.Text);
            CleanerData.WorkSteps_General[1].OpTime = Convert.ToDouble(LabelTime_Washing.Text);
            CleanerData.WorkSteps_General[2].OpTime = Convert.ToDouble(LabelTime_Rinsing.Text);
            CleanerData.WorkSteps_General[3].OpTime = Convert.ToDouble(LabelTime_Drying.Text);

            CleanerData.WorkSteps_General[0].RPMSpeed = Convert.ToInt16(LabelRPM_PreWashing.Text);
            CleanerData.WorkSteps_General[1].RPMSpeed = Convert.ToInt16(LabelRPM_Washing.Text);
            CleanerData.WorkSteps_General[2].RPMSpeed = Convert.ToInt16(LabelRPM_Rinsing.Text);
            CleanerData.WorkSteps_General[3].RPMSpeed = Convert.ToInt16(LabelRPM_Drying.Text);

            // Table Washing
            CleanerData.TableSteps[0].OpTime = Convert.ToDouble(LabelTime_TableWashing.Text);
            CleanerData.TableSteps[1].OpTime = Convert.ToDouble(LabelTime_TableDrying.Text);

            CleanerData.TableSteps[0].RPMSpeed = Convert.ToInt16(LabelRPM_TableWashing.Text);
            CleanerData.TableSteps[1].RPMSpeed = Convert.ToInt16(LabelRPM_TableDrying.Text);

            checkBox_EnableTableWashing.Checked = CleanerData.EnableThoroughCleaning;
            CleanerData.NozzleSpeed = Convert.ToDouble(Label_NozzleSpeed.Text);

            // Case / Disk Washing
            CleanerData.CaseSteps[0].OpTime = Convert.ToDouble(LabelTime_CaseWashing.Text);
            CleanerData.DiskSteps[0].OpTime = Convert.ToDouble(LabelTime_DiskWashing.Text);

            CleanerData.CaseSteps[0].RPMSpeed = Convert.ToInt16(LabelRPM_CaseWashing.Text);
            CleanerData.DiskSteps[0].RPMSpeed = Convert.ToInt16(LabelRPM_DiskWashing.Text);

            // other
            CleanerData.WashStroke              = Convert.ToDouble(LabelStroke.Text);
            CleanerData.UseWashSteps_General = (checkBox_UseCommon.Checked) ? true : false;

            // save
            CMainFrame.DataManager.ModelData.SpinnerData[(int)m_SpinnerIndex].CleanerData = ObjectExtensions.Copy(CleanerData);
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

        private void FormCleanerData_Load(object sender, EventArgs e)
        {

        }

        private void ComboSpinnerIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_SpinnerIndex = ESpinnerIndex.SPINNER1 + ComboSpinnerIndex.SelectedIndex;
            CleanerData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData.SpinnerData[(int)m_SpinnerIndex].CleanerData);
            UpdateData();
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

        private void checkBox_UseCustom_Click(object sender, EventArgs e)
        {
            checkBox_UseCommon.Checked = checkBox_UseCustom.Checked;

        }

        private void checkBox_UseCommon_Click(object sender, EventArgs e)
        {
            checkBox_UseCustom.Checked = checkBox_UseCommon.Checked;
        }

        private void BtnLoadFrom_Click(object sender, EventArgs e)
        {
            ESpinnerIndex index = (m_SpinnerIndex == ESpinnerIndex.SPINNER1) ? ESpinnerIndex.SPINNER2 : ESpinnerIndex.SPINNER1;
            CleanerData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData.SpinnerData[(int)index].CleanerData);

            UpdateData();
        }
    }
}
