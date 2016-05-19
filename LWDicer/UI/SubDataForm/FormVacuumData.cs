using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Vacuum;

using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormVacuumData : Form
    {
        public FormVacuumData()
        {
            InitializeComponent();

            InitGrid();

            UpdateScreen(CMainFrame.LWDicer.m_DataManager.SystemData_Vacuum);
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("Vacuum Data를 저장 하시겠습니까?"))
            {
                return;
            }

            CSystemData_Vacuum m_data = new CSystemData_Vacuum();

            int i = 0;

            for (i = 0; i < (int)EObjectVacuum.MAX; i++)
            {
                m_data.VacuumTimer[i].TurningTime = Convert.ToDouble(GridVacuumData[i + 1, 2].Text);
                m_data.VacuumTimer[i].OnSettlingTime = Convert.ToDouble(GridVacuumData[i + 1, 3].Text);
                m_data.VacuumTimer[i].OffSettlingTime = Convert.ToDouble(GridVacuumData[i + 1, 4].Text);
            }

            CMainFrame.LWDicer.SaveSystemData(null, null, null, m_data, null);

            // 저장하고 다시 Loading 하자
            CMainFrame.LWDicer.m_DataManager.LoadSystemData(false, false, false, true, false);

            UpdateScreen(m_data);
        }

        private void GridVacuumData_CellClick(object sender, Syncfusion.Windows.Forms.Grid.GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string StrCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol == 0 || nCol == 1 || nRow == 0)
            {
                return;
            }

            StrCurrent = GridVacuumData[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            GridVacuumData[nRow, nCol].Text = strModify;
        }

        private void FormVacuumData_Load(object sender, EventArgs e)
        {

        }

        private void FormVacuumData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridVacuumData.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridVacuumData.Properties.RowHeaders = true;
            GridVacuumData.Properties.ColHeaders = true;

            nCol = 4;
            nRow = (int)EObjectVacuum.MAX;

            // Column,Row 개수
            GridVacuumData.ColCount = nCol;
            GridVacuumData.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridVacuumData.ColWidths.SetSize(i, 120);
            }

            GridVacuumData.ColWidths.SetSize(0, 100);
            GridVacuumData.ColWidths.SetSize(1, 170);

            for (i = 0; i < nRow + 1; i++)
            {
                GridVacuumData.RowHeights[i] = 34;

            }

            GridVacuumData[1, 0].Text = "Work Stage";

            GridVacuumData.Model.CoveredRanges.Add(GridRangeInfo.Cells(2, 0, 3, 0));
            GridVacuumData[2, 0].Text = "Transfer";
            
            GridVacuumData[4, 0].Text = "Spinner 1";

            GridVacuumData[5, 0].Text = "Spinner 2";


            // Text Display
            GridVacuumData[0, 0].Text = "Assy' Unit";
            GridVacuumData[0, 1].Text = "Cylinder Name";
            GridVacuumData[0, 2].Text = "동작 제한 시간";
            GridVacuumData[0, 3].Text = "After On         Wait Time";
            GridVacuumData[0, 4].Text = "After Off         Wait Time";

            GridVacuumData[1, 1].Text = "Chuck Table";
            GridVacuumData[2, 1].Text = "Upper TR";
            GridVacuumData[3, 1].Text = "Lower TR";
            GridVacuumData[4, 1].Text = "Chuck Table";
            GridVacuumData[5, 1].Text = "Chuck Table";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridVacuumData[j, i].Font.Bold = true;

                    GridVacuumData[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridVacuumData[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (i = 0; i < nCol; i++)
            {
                GridVacuumData[1, i + 1].BackColor = Color.FromArgb(255, 240, 240);
                GridVacuumData[2, i + 1].BackColor = Color.FromArgb(220, 220, 255);
                GridVacuumData[3, i + 1].BackColor = Color.FromArgb(220, 220, 255);
                GridVacuumData[4, i + 1].BackColor = Color.FromArgb(255, 240, 240);
                GridVacuumData[5, i + 1].BackColor = Color.FromArgb(220, 220, 255);
            }

            GridStyleInfo style1 = new GridStyleInfo();
            style1.Borders.Bottom = new GridBorder(GridBorderStyle.Solid, Color.Black);
            GridVacuumData.RowStyles[1] = style1;
            GridVacuumData.RowStyles[2] = style1;
            GridVacuumData.RowStyles[4] = style1;
            GridVacuumData.RowStyles[5] = style1;

            GridStyleInfo style2 = new GridStyleInfo();
            style2.Borders.Right = new GridBorder(GridBorderStyle.Solid, Color.Black);
            GridVacuumData.ColStyles[4] = style2;

            GridVacuumData.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridVacuumData.ResizeColsBehavior = 0;
            GridVacuumData.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridVacuumData.Refresh();

        }

        private void UpdateScreen(CSystemData_Vacuum systemVacuum)
        {
            string strText = string.Empty;
            int i = 0;

            for (i = 0; i < (int)EObjectVacuum.MAX; i++)
            {
                GridVacuumData[i + 1, 2].Text = Convert.ToString(systemVacuum.VacuumTimer[i].TurningTime);
                GridVacuumData[i + 1, 3].Text = Convert.ToString(systemVacuum.VacuumTimer[i].OnSettlingTime);
                GridVacuumData[i + 1, 4].Text = Convert.ToString(systemVacuum.VacuumTimer[i].OffSettlingTime);
            }
        }
    }
}
