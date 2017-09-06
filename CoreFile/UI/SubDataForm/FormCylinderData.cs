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

using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Cylinder;

using static Core.Layers.DEF_DataManager;

namespace Core.UI
{
    public partial class FormCylinderData : Form
    {
        public FormCylinderData()
        {
            InitializeComponent();

            InitGrid();

            UpdateScreen(CMainFrame.DataManager.SystemData_Cylinder);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCylinderData_Load(object sender, EventArgs e)
        {
            UpdateScreen(CMainFrame.DataManager.SystemData_Cylinder);
        }

        private void FormCylinderData_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void InitGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridCtrl.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCtrl.Properties.RowHeaders = true;
            GridCtrl.Properties.ColHeaders = true;

            int nCol = 4;
            int nRow = (int)EObjectCylinder.MAX;

            // Column,Row 개수
            GridCtrl.ColCount = nCol;
            GridCtrl.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                GridCtrl.ColWidths.SetSize(i, 120);
            }

            GridCtrl.ColWidths.SetSize(0, 200);

            for (int i = 0; i < nRow + 1; i++)
            {
                GridCtrl.RowHeights[i] = 34;

            }

            // Text Display
            GridCtrl[0, 0].Text = "Cylinder";
            GridCtrl[0, 1].Text = "동작 제한 시간";
            GridCtrl[0, 2].Text = "Wait Time 1";
            GridCtrl[0, 3].Text = "Wait Time 2";
            GridCtrl[0, 4].Text = "No Sensor         Wait Time";


            for (int i = 0; i < (int)EObjectCylinder.MAX; i++)
            {
                GridCtrl[i + 1, 0].Text = CMainFrame.Core.m_SystemInfo.GetObjectName(100 + i);
            }

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridCtrl[j, i].Font.Bold = true;

                    GridCtrl[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCtrl[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (int i = 1; i < nCol + 1; i++)
            {
                for (int j = 1; j < nRow + 1; j++)
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save data?"))
            {
                return;
            }

            CSystemData_Cylinder data = new CSystemData_Cylinder();

            for (int i = 0; i < (int)EObjectCylinder.MAX; i++)
            {
                data.CylinderTimer[i].MovingTime = Convert.ToDouble(GridCtrl[i + 1, 1].Text);
                data.CylinderTimer[i].SettlingTime1 = Convert.ToDouble(GridCtrl[i + 1, 2].Text);
                data.CylinderTimer[i].SettlingTime2 = Convert.ToDouble(GridCtrl[i + 1, 3].Text);
                data.CylinderTimer[i].NoSenMovingTime = Convert.ToDouble(GridCtrl[i + 1, 4].Text);
            }

            CMainFrame.Core.SaveSystemData(systemCylinder:data);
            UpdateScreen(data);
        }

        private void GridCylinderData_CellClick(object sender, GridCellClickEventArgs e)
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

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            if (checkBoxAll.Checked == true)
            {
                for(int i = 1; i < GridCtrl.RowCount + 1; i++)
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

        private void UpdateScreen(CSystemData_Cylinder systemCylinder)
        {
            for (int i = 0; i < (int)EObjectCylinder.MAX; i++)
            {
                GridCtrl[i + 1, 1].Text = String.Format("{0:0.000}", systemCylinder.CylinderTimer[i].MovingTime);
                GridCtrl[i + 1, 2].Text = String.Format("{0:0.000}", systemCylinder.CylinderTimer[i].SettlingTime1);
                GridCtrl[i + 1, 3].Text = String.Format("{0:0.000}", systemCylinder.CylinderTimer[i].SettlingTime2);
                GridCtrl[i + 1, 4].Text = String.Format("{0:0.000}", systemCylinder.CylinderTimer[i].NoSenMovingTime);

                for (int j = 0; j < 4; j++)
                {
                    GridCtrl[i + 1, j + 1].TextColor = Color.Black;
                }
            }
        }
    }
}
