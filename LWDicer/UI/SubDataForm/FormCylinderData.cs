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
using static LWDicer.Control.DEF_Cylinder;

using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormCylinderData : Form
    {
        public FormCylinderData()
        {
            InitializeComponent();

            InitGrid();

            UpdateScreen(CMainFrame.LWDicer.m_DataManager.SystemData_Cylinder);
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormCylinderData_Load(object sender, EventArgs e)
        {
            UpdateScreen(CMainFrame.LWDicer.m_DataManager.SystemData_Cylinder);
        }

        private void FormCylinderData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridCylinderData.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCylinderData.Properties.RowHeaders = true;
            GridCylinderData.Properties.ColHeaders = true;

            nCol = 4;
            nRow = (int)EObjectCylinder.MAX;

            // Column,Row 개수
            GridCylinderData.ColCount = nCol;
            GridCylinderData.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridCylinderData.ColWidths.SetSize(i, 120);
            }

            GridCylinderData.ColWidths.SetSize(0, 200);

            for (i = 0; i < nRow + 1; i++)
            {
                GridCylinderData.RowHeights[i] = 34;

            }

            // Text Display
            GridCylinderData[0, 0].Text = "Cylinder";
            GridCylinderData[0, 1].Text = "동작 제한 시간";
            GridCylinderData[0, 2].Text = "Wait Time 1";
            GridCylinderData[0, 3].Text = "Wait Time 2";
            GridCylinderData[0, 4].Text = "No Sensor         Wait Time";


            for (i = 0; i < (int)EObjectCylinder.MAX; i++)
            {
                GridCylinderData[i + 1, 0].Text = CMainFrame.LWDicer.m_SystemInfo.GetObjectName(100 + i);
            }

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridCylinderData[j, i].Font.Bold = true;

                    GridCylinderData[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCylinderData[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (i = 1; i < nCol + 1; i++)
            {
                for (j = 1; j < nRow + 1; j++)
                {
                    GridCylinderData[j, i].BackColor = Color.FromArgb(220, 220, 255);
                }
            }

            GridCylinderData.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCylinderData.ResizeColsBehavior = 0;
            GridCylinderData.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCylinderData.Refresh();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("Cylinder Data를 저장 하시겠습니까?"))
            {
                return;
            }

            CSystemData_Cylinder m_data = new CSystemData_Cylinder();

            int i = 0;

            for (i = 0; i < (int)EObjectCylinder.MAX; i++)
            {
                m_data.CylinderTimer[i].MovingTime = Convert.ToDouble(GridCylinderData[i + 1, 1].Text);
                m_data.CylinderTimer[i].SettlingTime1 = Convert.ToDouble(GridCylinderData[i + 1, 2].Text);
                m_data.CylinderTimer[i].SettlingTime1 = Convert.ToDouble(GridCylinderData[i + 1, 3].Text);
                m_data.CylinderTimer[i].NoSenMovingTime = Convert.ToDouble(GridCylinderData[i + 1, 4].Text);
            }

            CMainFrame.LWDicer.SaveSystemData(null, null, m_data, null, null);

            // 저장하고 다시 Loading 하자
            CMainFrame.LWDicer.m_DataManager.LoadSystemData(false, false, true, false, false);

            UpdateScreen(m_data);
        }

        private void GridCylinderData_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string StrCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol == 0 || nRow == 0)
            {
                return;
            }

            StrCurrent = GridCylinderData[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            GridCylinderData[nRow, nCol].Text = strModify;
            GridCylinderData[nRow, nCol].TextColor = Color.Red;

        }

        private void UpdateScreen(CSystemData_Cylinder systemCylinder)
        {
            string strText = string.Empty;
            int i = 0, j = 0;

            for (i = 0; i < (int)EObjectCylinder.MAX; i++)
            {
                GridCylinderData[i + 1, 1].Text = Convert.ToString(systemCylinder.CylinderTimer[i].MovingTime);
                GridCylinderData[i + 1, 2].Text = Convert.ToString(systemCylinder.CylinderTimer[i].SettlingTime1);
                GridCylinderData[i + 1, 3].Text = Convert.ToString(systemCylinder.CylinderTimer[i].SettlingTime2);
                GridCylinderData[i + 1, 4].Text = Convert.ToString(systemCylinder.CylinderTimer[i].NoSenMovingTime);

                for (j = 0; j < 4; j++)
                {
                    GridCylinderData[i + 1, j + 1].TextColor = Color.Black;
                }
            }
        }
    }
}
