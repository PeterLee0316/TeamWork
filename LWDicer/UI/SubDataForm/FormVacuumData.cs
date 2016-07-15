﻿using System;
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

            UpdateScreen(CMainFrame.DataManager.SystemData_Vacuum);
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
            if (!CMainFrame.DisplayMsg("Save Data?"))
            {
                return;
            }

            CSystemData_Vacuum data = new CSystemData_Vacuum();

            int i = 0;

            for (i = 0; i < (int)EObjectVacuum.MAX; i++)
            {
                data.VacuumTimer[i].TurningTime = Convert.ToDouble(GridCtrl[i + 1, 1].Text);
                data.VacuumTimer[i].OnSettlingTime = Convert.ToDouble(GridCtrl[i + 1, 2].Text);
                data.VacuumTimer[i].OffSettlingTime = Convert.ToDouble(GridCtrl[i + 1, 3].Text);
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

            strCurrent = GridCtrl[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            if (checkBoxAll.Checked == true)
            {
                for (int i = 1; i < GridCtrl.RowCount + 1; i++)
                {
                    GridCtrl[i, nCol].Text = strModify;
                    GridCtrl[i, nCol].TextColor = Color.Blue;
                }
            }
            else
            {
                GridCtrl[nRow, nCol].Text = strModify;
                GridCtrl[nRow, nCol].TextColor = Color.Blue;
            }
        }

        private void FormVacuumData_Load(object sender, EventArgs e)
        {
            UpdateScreen(CMainFrame.DataManager.SystemData_Vacuum);
        }

        private void FormVacuumData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridCtrl.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCtrl.Properties.RowHeaders = true;
            GridCtrl.Properties.ColHeaders = true;

            nCol = 3;
            nRow = (int)EObjectVacuum.MAX;

            // Column,Row 개수
            GridCtrl.ColCount = nCol;
            GridCtrl.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridCtrl.ColWidths.SetSize(i, 120);
            }

            GridCtrl.ColWidths.SetSize(0, 200);

            for (i = 0; i < nRow + 1; i++)
            {
                GridCtrl.RowHeights[i] = 34;
            }

            // Text Display
            GridCtrl[0, 0].Text = "Unit";
            GridCtrl[0, 1].Text = "동작 제한 시간";
            GridCtrl[0, 2].Text = "After On         Wait Time";
            GridCtrl[0, 3].Text = "After Off         Wait Time";

            for (i = 0; i < (int)EObjectVacuum.MAX; i++)
            {
                GridCtrl[i + 1, 0].Text = CMainFrame.LWDicer.m_SystemInfo.GetObjectName(150 + i);
            }

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridCtrl[j, i].Font.Bold = true;

                    GridCtrl[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCtrl[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (i = 1; i < nCol + 1; i++)
            {
                for (j = 1; j < nRow + 1; j++)
                {
                    GridCtrl[j, i].BackColor = Color.FromArgb(220, 220, 255);
                }
            }

            GridCtrl.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCtrl.ResizeColsBehavior = 0;
            GridCtrl.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCtrl.Refresh();

        }

        private void UpdateScreen(CSystemData_Vacuum systemVacuum)
        {
            string strText;
            int i = 0, j = 0;

            for (i = 0; i < (int)EObjectVacuum.MAX; i++)
            {
                GridCtrl[i + 1, 1].Text = Convert.ToString(systemVacuum.VacuumTimer[i].TurningTime);
                GridCtrl[i + 1, 2].Text = Convert.ToString(systemVacuum.VacuumTimer[i].OnSettlingTime);
                GridCtrl[i + 1, 3].Text = Convert.ToString(systemVacuum.VacuumTimer[i].OffSettlingTime);

                for (j = 0; j < 3; j++)
                {
                    GridCtrl[i + 1, j + 1].TextColor = Color.Black;
                }
            }
        }
    }
}
