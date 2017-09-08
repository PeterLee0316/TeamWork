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

using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_ACS;

namespace Core.UI
{
    public partial class FormMotorData : Form
    {
        CSystemData_Axis SystemData_Axis;

        public FormMotorData()
        {
            SystemData_Axis = ObjectExtensions.Copy(CMainFrame.DataManager.SystemData_Axis);

            InitializeComponent();
            InitializeForm();
        }

        protected virtual void InitializeForm()
        {
            InitGrid();

            UpdateScreen();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMotorData_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(10, 128);

            UpdateScreen();
        }

        private void FormMotorData_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void InitGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridCtrl.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCtrl.Properties.RowHeaders = true;
            GridCtrl.Properties.ColHeaders = true;

            int nCol = 30;
            int nRow = 19;
            //nRow = (int)EYMC_Axis.MAX;

            // Column,Row 개수
            GridCtrl.ColCount = nCol;
            GridCtrl.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                GridCtrl.ColWidths.SetSize(i, 120);
            }

            GridCtrl.ColWidths.SetSize(0, 40);
            GridCtrl.ColWidths.SetSize(1, 170);

            for (int i = 0; i < nRow + 1; i++)
            {
                GridCtrl.RowHeights[i] = 34;

            }

            for (int i = 0; i < nRow; i++)
            {
                GridCtrl[i + 1, 0].Text = string.Format("#{0:d}", i + 1);
            }


            // Text Display
            GridCtrl[0, 0].Text = "No.";
            GridCtrl[0, 1].Text = "Axis Name";
            GridCtrl[0, 2].Text = "Manual Slow Vel";
            GridCtrl[0, 3].Text = "Manual Fast Vel";
            GridCtrl[0, 4].Text = "Auto Slow Vel";
            GridCtrl[0, 5].Text = "Auto Fast Vel";
            GridCtrl[0, 6].Text = "Jog Slow Vel";
            GridCtrl[0, 7].Text = "Jog Fast Vel";
            GridCtrl[0, 8].Text = "Manual Fast Acc";
            GridCtrl[0, 9].Text = "Manual Slow Acc";
            GridCtrl[0, 10].Text = "Auto Slow Acc";
            GridCtrl[0, 11].Text = "Auto Fast Acc";
            GridCtrl[0, 12].Text = "Jog Slow Acc";
            GridCtrl[0, 13].Text = "Jog Fast Acc";
            GridCtrl[0, 14].Text = "Manual Fast Dec";
            GridCtrl[0, 15].Text = "Manual Slow Dec";
            GridCtrl[0, 16].Text = "Auto Slow Dec";
            GridCtrl[0, 17].Text = "Auto Fast Dec";
            GridCtrl[0, 18].Text = "Jog Slow Dec";
            GridCtrl[0, 19].Text = "Jog Fast Dec";
            GridCtrl[0, 20].Text = "S/W P Limit";
            GridCtrl[0, 21].Text = "S/W N Limit";
            GridCtrl[0, 22].Text = "Move Limit (sec)";
            GridCtrl[0, 23].Text = "After Move (sec)";
            GridCtrl[0, 24].Text = "Origin Limit (sec)";
            GridCtrl[0, 25].Text = "Home Method";
            GridCtrl[0, 26].Text = "Home Dir";
            GridCtrl[0, 27].Text = "Home Fast Speed";
            GridCtrl[0, 28].Text = "Home Slow Speed";
            GridCtrl[0, 29].Text = "Home Offset";
            GridCtrl[0, 30].Text = "Tolerance";

            for (int i = 0; i <  (int)EYMC_Axis.MAX; i++)
            {
                GridCtrl[i+1, 1].Text = Convert.ToString(EYMC_Axis.LOADER_Z+i);
            }

            //for (int i = 0; i < (int)EACS_Axis.MAX; i++)            
            //{
            //    GridCtrl[i + 17, 1].Text = Convert.ToString(EACS_Axis.STAGE1_X + i);
            //}
            GridCtrl[15, 1].Text = Convert.ToString(EAxis.CAMERA1_Z);
            GridCtrl[16, 1].Text = Convert.ToString(EAxis.SCANNER_Z1);
            GridCtrl[17, 1].Text = Convert.ToString(EAxis.STAGE1_X);
            GridCtrl[18, 1].Text = Convert.ToString(EAxis.STAGE1_Y);
            GridCtrl[19, 1].Text = Convert.ToString(EAxis.STAGE1_T);

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

            for (int i = 0; i < nRow; i++)
            {
                GridCtrl[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
            }

            GridCtrl.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCtrl.ResizeColsBehavior = 0;
            GridCtrl.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCtrl.Refresh();
        }

        private void UpdateScreen()
        {
            

            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    GridCtrl[i + 1, j + 2].TextColor = Color.Black;
                }
            }
        }

        private void GridMotorPara_CellClick(object sender, GridCellClickEventArgs e)
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save data?"))
            {
                return;
            }

            // Motor Data Sheet
            int gridIndex = 1;           
           

            for (int i = 0; i < (int)EACS_Axis.MAX; i++)
            {
                if (i == 1 || i == 3 || i == 5)
                {
                    continue;
                }

                // Speed
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel    = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 2].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel    = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 3].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel      = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 4].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel      = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 5].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel       = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 6].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel       = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 7].Text);

                // Acc
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc    = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 8].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc    = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 9].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc      = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 10].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc      = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 11].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc       = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 12].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc       = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 13].Text);

                // Dec
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec    = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 14].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec    = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 15].Text); 
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec      = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 16].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec      = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 17].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec       = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 18].Text); 
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec       = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 19].Text);

                // S/W Limit
                SystemData_Axis.ACSMotionData[i].PosLimit.Plus                              = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 20].Text);
                SystemData_Axis.ACSMotionData[i].PosLimit.Minus                             = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 21].Text);

                // Limit Time
                SystemData_Axis.ACSMotionData[i].TimeLimit.tMoveLimit                       = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 22].Text);
                SystemData_Axis.ACSMotionData[i].TimeLimit.tSleepAfterMove                  = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 23].Text);
                SystemData_Axis.ACSMotionData[i].TimeLimit.tOriginLimit                     = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 24].Text);

                // Tolerance
                SystemData_Axis.ACSMotionData[i].Tolerance = Convert.ToDouble(GridCtrl[gridIndex + (int)EYMC_Axis.MAX, 30].Text);

                gridIndex++;
            }

            CMainFrame.Core.SaveSystemData(systemAxis: SystemData_Axis);            
            CMainFrame.Core.m_ACS.SetACSMotionData(CMainFrame.Core.m_DataManager.SystemData_Axis.ACSMotionData);

        }
    }
}
