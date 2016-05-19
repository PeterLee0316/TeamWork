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

            nCol = 5;
            nRow = (int)EObjectCylinder.MAX;

            // Column,Row 개수
            GridCylinderData.ColCount = nCol;
            GridCylinderData.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridCylinderData.ColWidths.SetSize(i, 120);
            }

            GridCylinderData.ColWidths.SetSize(0, 100);
            GridCylinderData.ColWidths.SetSize(1, 170);

            for (i = 0; i < nRow + 1; i++)
            {
                GridCylinderData.RowHeights[i] = 34;

            }

            // Text Display
            GridCylinderData.Model.CoveredRanges.Add(GridRangeInfo.Cells(1, 0, 2, 0));
            GridCylinderData[1, 0].Text = "Push Pull";

            GridCylinderData.Model.CoveredRanges.Add(GridRangeInfo.Cells(3, 0, 5, 0));
            GridCylinderData[3, 0].Text = "Spinner 1";

            GridCylinderData.Model.CoveredRanges.Add(GridRangeInfo.Cells(6, 0, 8, 0));
            GridCylinderData[6, 0].Text = "Spinner 2";

            GridCylinderData.Model.CoveredRanges.Add(GridRangeInfo.Cells(9, 0, 10, 0));
            GridCylinderData[9, 0].Text = "Work Stage";

            GridCylinderData[0, 0].Text = "Assy' Unit";
            GridCylinderData[0, 1].Text = "Cylinder Name";
            GridCylinderData[0, 2].Text = "동작 제한 시간";
            GridCylinderData[0, 3].Text = "Wait Time 1";
            GridCylinderData[0, 4].Text = "Wait Time 2";
            GridCylinderData[0, 5].Text = "No Sensor         Wait Time";

            GridCylinderData[1, 1].Text = "Wafer Feeder Clamp";
            GridCylinderData[2, 1].Text = "Feeder Clamp Pick Arm";
            GridCylinderData[3, 1].Text = "Chuck Up/Down";
            GridCylinderData[4, 1].Text = "Nozzle Valve 1";
            GridCylinderData[5, 1].Text = "Nozzle Valve 2";
            GridCylinderData[6, 1].Text = "Chuck Up/Down";
            GridCylinderData[7, 1].Text = "Nozzle Valve 1";
            GridCylinderData[8, 1].Text = "Nozzle Valve 2";
            GridCylinderData[9, 1].Text = "Wafer Clamp 1";
            GridCylinderData[10, 1].Text = "Wafer Clamp 2";

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

            for (i = 0; i < nCol; i++)
            {
                GridCylinderData[1, i + 1].BackColor = Color.FromArgb(255, 240, 240);
                GridCylinderData[2, i + 1].BackColor = Color.FromArgb(255, 240, 240);

                GridCylinderData[3, i + 1].BackColor = Color.FromArgb(220, 220, 255);
                GridCylinderData[4, i + 1].BackColor = Color.FromArgb(220, 220, 255);
                GridCylinderData[5, i + 1].BackColor = Color.FromArgb(220, 220, 255);

                GridCylinderData[6, i + 1].BackColor = Color.FromArgb(255, 240, 240);
                GridCylinderData[7, i + 1].BackColor = Color.FromArgb(255, 240, 240);
                GridCylinderData[8, i + 1].BackColor = Color.FromArgb(255, 240, 240);

                GridCylinderData[9, i + 1].BackColor = Color.FromArgb(220, 220, 255);
                GridCylinderData[10, i + 1].BackColor = Color.FromArgb(220, 220, 255);
            }


            GridStyleInfo style1 = new GridStyleInfo();
            style1.Borders.Bottom = new GridBorder(GridBorderStyle.Solid, Color.Black);
            GridCylinderData.RowStyles[2] = style1;
            GridCylinderData.RowStyles[5] = style1;
            GridCylinderData.RowStyles[8] = style1;
            GridCylinderData.RowStyles[10] = style1;

            GridStyleInfo style2 = new GridStyleInfo();
            style2.Borders.Right = new GridBorder(GridBorderStyle.Solid, Color.Black);
            GridCylinderData.ColStyles[5] = style2;

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

            for(i=0;i< (int)EObjectCylinder.MAX;i++)
            {
                m_data.CylinderTimer[i].MovingTime      = Convert.ToDouble(GridCylinderData[i + 1, 2].Text);
                m_data.CylinderTimer[i].SettlingTime1   = Convert.ToDouble(GridCylinderData[i + 1, 3].Text);
                m_data.CylinderTimer[i].SettlingTime1   = Convert.ToDouble(GridCylinderData[i + 1, 4].Text);
                m_data.CylinderTimer[i].NoSenMovingTime = Convert.ToDouble(GridCylinderData[i + 1, 5].Text);
            }
            
            CMainFrame.LWDicer.SaveSystemData(null,null, m_data,null,null);

            // 저장하고 다시 Loading 하자
            CMainFrame.LWDicer.m_DataManager.LoadSystemData(false,false,true,false,false);
            
            UpdateScreen(m_data);
        }

        private void GridCylinderData_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string StrCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol == 0 || nCol == 1 || nRow == 0)
            {
                return;
            }

            StrCurrent = GridCylinderData[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            GridCylinderData[nRow, nCol].Text = strModify;
        }

        private void UpdateScreen(CSystemData_Cylinder systemCylinder)
        {
            string strText = string.Empty;
            int i = 0;

            for (i = 0; i < (int)EObjectCylinder.MAX; i++)
            {
                GridCylinderData[i + 1, 2].Text = Convert.ToString(systemCylinder.CylinderTimer[i].MovingTime);
                GridCylinderData[i + 1, 3].Text = Convert.ToString(systemCylinder.CylinderTimer[i].SettlingTime1);
                GridCylinderData[i + 1, 4].Text = Convert.ToString(systemCylinder.CylinderTimer[i].SettlingTime2);
                GridCylinderData[i + 1, 5].Text = Convert.ToString(systemCylinder.CylinderTimer[i].NoSenMovingTime);
            }
        }
    }
}
