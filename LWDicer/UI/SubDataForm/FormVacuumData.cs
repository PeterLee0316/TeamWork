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
            if (!CMainFrame.LWDicer.DisplayMsg("", 25))
            {
                return;
            }

            CSystemData_Vacuum data = new CSystemData_Vacuum();

            int i = 0;

            for (i = 0; i < (int)EObjectVacuum.MAX; i++)
            {
                data.VacuumTimer[i].TurningTime = Convert.ToDouble(GridVacuumData[i + 1, 1].Text);
                data.VacuumTimer[i].OnSettlingTime = Convert.ToDouble(GridVacuumData[i + 1, 2].Text);
                data.VacuumTimer[i].OffSettlingTime = Convert.ToDouble(GridVacuumData[i + 1, 3].Text);
            }

            CMainFrame.LWDicer.SaveSystemData(systemVacuum: data);
            UpdateScreen(data);
        }

        private void GridVacuumData_CellClick(object sender, Syncfusion.Windows.Forms.Grid.GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol == 0 || nRow == 0)
            {
                return;
            }

            strCurrent = GridVacuumData[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridVacuumData[nRow, nCol].Text = strModify;
            GridVacuumData[nRow, nCol].TextColor = Color.Red;
        }

        private void FormVacuumData_Load(object sender, EventArgs e)
        {
            UpdateScreen(CMainFrame.LWDicer.m_DataManager.SystemData_Vacuum);
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

            nCol = 3;
            nRow = (int)EObjectVacuum.MAX;

            // Column,Row 개수
            GridVacuumData.ColCount = nCol;
            GridVacuumData.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridVacuumData.ColWidths.SetSize(i, 120);
            }

            GridVacuumData.ColWidths.SetSize(0, 200);

            for (i = 0; i < nRow + 1; i++)
            {
                GridVacuumData.RowHeights[i] = 34;
            }

            // Text Display
            GridVacuumData[0, 0].Text = "Unit";
            GridVacuumData[0, 1].Text = "동작 제한 시간";
            GridVacuumData[0, 2].Text = "After On         Wait Time";
            GridVacuumData[0, 3].Text = "After Off         Wait Time";

            for (i = 0; i < (int)EObjectVacuum.MAX; i++)
            {
                GridVacuumData[i + 1, 0].Text = CMainFrame.LWDicer.m_SystemInfo.GetObjectName(150 + i);
            }

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

            for (i = 1; i < nCol + 1; i++)
            {
                for (j = 1; j < nRow + 1; j++)
                {
                    GridVacuumData[j, i].BackColor = Color.FromArgb(220, 220, 255);
                }
            }

            GridVacuumData.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridVacuumData.ResizeColsBehavior = 0;
            GridVacuumData.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridVacuumData.Refresh();

        }

        private void UpdateScreen(CSystemData_Vacuum systemVacuum)
        {
            string strText;
            int i = 0, j = 0;

            for (i = 0; i < (int)EObjectVacuum.MAX; i++)
            {
                GridVacuumData[i + 1, 1].Text = Convert.ToString(systemVacuum.VacuumTimer[i].TurningTime);
                GridVacuumData[i + 1, 2].Text = Convert.ToString(systemVacuum.VacuumTimer[i].OnSettlingTime);
                GridVacuumData[i + 1, 3].Text = Convert.ToString(systemVacuum.VacuumTimer[i].OffSettlingTime);

                for (j = 0; j < 3; j++)
                {
                    GridVacuumData[i + 1, j + 1].TextColor = Color.Black;
                }
            }
        }
    }
}
