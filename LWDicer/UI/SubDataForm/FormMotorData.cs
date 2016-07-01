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

using static LWDicer.Control.DEF_DataManager;
using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_Yaskawa;

namespace LWDicer.UI
{
    public partial class FormMotorData : Form
    {
        CSystemData_Axis SystemData_Axis;

        public FormMotorData()
        {
            SystemData_Axis = ObjectExtensions.Copy(CMainFrame.LWDicer.m_DataManager.SystemData_Axis);

            InitializeComponent();
            InitializeForm();
        }

        protected virtual void InitializeForm()
        {
            InitGrid();

            UpdateScreen();
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormMotorData_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);

            UpdateScreen();
        }

        private void FormMotorData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridMotorPara.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridMotorPara.Properties.RowHeaders = true;
            GridMotorPara.Properties.ColHeaders = true;

            nCol = 29;
            nRow = 19;
            //nRow = (int)EYMC_Axis.MAX;

            // Column,Row 개수
            GridMotorPara.ColCount = nCol;
            GridMotorPara.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridMotorPara.ColWidths.SetSize(i, 120);
            }

            GridMotorPara.ColWidths.SetSize(0, 40);
            GridMotorPara.ColWidths.SetSize(1, 170);

            for (i = 0; i < nRow + 1; i++)
            {
                GridMotorPara.RowHeights[i] = 34;

            }

            for (i = 0; i < nRow; i++)
            {
                GridMotorPara[i + 1, 0].Text = string.Format("#{0:d}", i + 1);
            }


            // Text Display
            GridMotorPara[0, 0].Text = "No.";
            GridMotorPara[0, 1].Text = "Axis Name";
            GridMotorPara[0, 2].Text = "Manual Slow Vel";
            GridMotorPara[0, 3].Text = "Manual Fast Vel";
            GridMotorPara[0, 4].Text = "Auto Slow Vel";
            GridMotorPara[0, 5].Text = "Auto Fast Vel";
            GridMotorPara[0, 6].Text = "Jog Slow Vel";
            GridMotorPara[0, 7].Text = "Jog Fast Vel";
            GridMotorPara[0, 8].Text = "Manual Fast Acc";
            GridMotorPara[0, 9].Text = "Manual Slow Acc";
            GridMotorPara[0, 10].Text = "Auto Slow Acc";
            GridMotorPara[0, 11].Text = "Auto Fast Vcc";
            GridMotorPara[0, 12].Text = "Jog Slow Vcc";
            GridMotorPara[0, 13].Text = "Jog Fast Vcc";
            GridMotorPara[0, 14].Text = "Manual Fast Dec";
            GridMotorPara[0, 15].Text = "Manual Slow Dec";
            GridMotorPara[0, 16].Text = "Auto Slow Dec";
            GridMotorPara[0, 17].Text = "Auto Fast Dec";
            GridMotorPara[0, 18].Text = "Jog Slow Dec";
            GridMotorPara[0, 19].Text = "Jog Fast Dec";
            GridMotorPara[0, 20].Text = "S/W P Limit";
            GridMotorPara[0, 21].Text = "S/W N Limit";
            GridMotorPara[0, 22].Text = "Move Limit";
            GridMotorPara[0, 23].Text = "After Move";
            GridMotorPara[0, 24].Text = "Origin Limit";
            GridMotorPara[0, 25].Text = "Home Method";
            GridMotorPara[0, 26].Text = "Home Dir";
            GridMotorPara[0, 27].Text = "Home Fast Speed";
            GridMotorPara[0, 28].Text = "Home Slow Speed";
            GridMotorPara[0, 29].Text = "Home Offset";

            for(i=0;i< (int)EYMC_Axis.MAX;i++)
            {
                GridMotorPara[i+1, 1].Text = Convert.ToString(EYMC_Axis.LOADER_Z+i);
            }

            for (i = 0; i < (int)EACS_Axis.MAX; i++)
            {
                GridMotorPara[i + 17, 1].Text = Convert.ToString(EACS_Axis.STAGE1_X + i);
            }

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridMotorPara[j, i].Font.Bold = true;

                    GridMotorPara[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridMotorPara[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (i = 0; i < nRow; i++)
            {
                GridMotorPara[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
            }

            GridMotorPara.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridMotorPara.ResizeColsBehavior = 0;
            GridMotorPara.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridMotorPara.Refresh();
        }

        private void UpdateScreen()
        {
            int i = 0, j = 0;

            // MP
            for (i = 0; i < 16; i++)
            {
                // Speed
                GridMotorPara[i + 1, 2].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel);
                GridMotorPara[i + 1, 3].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel);
                GridMotorPara[i + 1, 4].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel);
                GridMotorPara[i + 1, 5].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel);
                GridMotorPara[i + 1, 6].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel);
                GridMotorPara[i + 1, 7].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel);

                // Acc
                GridMotorPara[i + 1, 8].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc);
                GridMotorPara[i + 1, 9].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc);
                GridMotorPara[i + 1, 10].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc);
                GridMotorPara[i + 1, 11].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc);
                GridMotorPara[i + 1, 12].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc);
                GridMotorPara[i + 1, 13].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc);

                // Dec
                GridMotorPara[i + 1, 14].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec);
                GridMotorPara[i + 1, 15].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec);
                GridMotorPara[i + 1, 16].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec);
                GridMotorPara[i + 1, 17].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec);
                GridMotorPara[i + 1, 18].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec);
                GridMotorPara[i + 1, 19].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec);

                // S/W Limit
                GridMotorPara[i + 1, 20].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].PosLimit.Plus);
                GridMotorPara[i + 1, 21].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].PosLimit.Minus);

                // Limit Time
                GridMotorPara[i + 1, 22].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].TimeLimit.tMoveLimit);
                GridMotorPara[i + 1, 23].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].TimeLimit.tSleepAfterMove);
                GridMotorPara[i + 1, 24].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].TimeLimit.tOriginLimit);

                // Home Option
                GridMotorPara[i + 1, 25].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].OriginData.Method);
                GridMotorPara[i + 1, 26].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].OriginData.Dir);
                GridMotorPara[i + 1, 27].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].OriginData.FastSpeed);
                GridMotorPara[i + 1, 28].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].OriginData.SlowSpeed);
                GridMotorPara[i + 1, 29].Text = Convert.ToString(SystemData_Axis.MPMotionData[i].OriginData.HomeOffset);
            }

            // ACS
            for (i = 0; i < 3; i++)
            {
                // Speed
                GridMotorPara[i + 17, 2].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel);
                GridMotorPara[i + 17, 3].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel);
                GridMotorPara[i + 17, 4].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel);
                GridMotorPara[i + 17, 5].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel);
                GridMotorPara[i + 17, 6].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel);
                GridMotorPara[i + 17, 7].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel);

                // Acc
                GridMotorPara[i + 17, 8].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc);
                GridMotorPara[i + 17, 9].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc);
                GridMotorPara[i + 17, 10].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc);
                GridMotorPara[i + 17, 11].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc);
                GridMotorPara[i + 17, 12].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc);
                GridMotorPara[i + 17, 13].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc);

                // Dec
                GridMotorPara[i + 17, 14].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec);
                GridMotorPara[i + 17, 15].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec);
                GridMotorPara[i + 17, 16].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec);
                GridMotorPara[i + 17, 17].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec);
                GridMotorPara[i + 17, 18].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec);
                GridMotorPara[i + 17, 19].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec);

                // S/W Limit
                GridMotorPara[i + 17, 20].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].PosLimit.Plus);
                GridMotorPara[i + 17, 21].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].PosLimit.Minus);

                // Limit Time
                GridMotorPara[i + 17, 22].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].TimeLimit.tMoveLimit);
                GridMotorPara[i + 17, 23].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].TimeLimit.tSleepAfterMove);
                GridMotorPara[i + 17, 24].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].TimeLimit.tOriginLimit);

                // Home Option
                //GridMotorPara[i + 17, 25].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].OriginData.Method);
                //GridMotorPara[i + 17, 26].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].OriginData.Dir);
                //GridMotorPara[i + 17, 27].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].OriginData.FastSpeed);
                //GridMotorPara[i + 17, 28].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].OriginData.SlowSpeed);
                //GridMotorPara[i + 17, 29].Text = Convert.ToString(SystemData_Axis.ACSMotionData[i].OriginData.HomeOffset);
            }

            for (i = 0; i < 19; i++)
            {
                for (j = 0; j < 26; j++)
                {
                    GridMotorPara[i + 1, j + 2].TextColor = Color.Black;
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

            strCurrent = GridMotorPara[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridMotorPara[nRow, nCol].Text = strModify;
            GridMotorPara[nRow, nCol].TextColor = Color.Red;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            int i = 0;

            if (!CMainFrame.LWDicer.DisplayMsg("", 11))
            {
                return;
            }

            // Motor Data Sheet
            for (i = 0; i < 16; i++)
            {
                // Speed
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel = Convert.ToDouble(GridMotorPara[i + 1, 2].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel = Convert.ToDouble(GridMotorPara[i + 1, 3].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel = Convert.ToDouble(GridMotorPara[i + 1, 4].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel = Convert.ToDouble(GridMotorPara[i + 1, 5].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel = Convert.ToDouble(GridMotorPara[i + 1, 6].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel = Convert.ToDouble(GridMotorPara[i + 1, 7].Text);

                // Acc
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc = Convert.ToDouble(GridMotorPara[i + 1, 8].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc = Convert.ToDouble(GridMotorPara[i + 1, 9].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc = Convert.ToDouble(GridMotorPara[i + 1, 10].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc = Convert.ToDouble(GridMotorPara[i + 1, 11].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc = Convert.ToDouble(GridMotorPara[i + 1, 12].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc = Convert.ToDouble(GridMotorPara[i + 1, 13].Text);

                // Dec
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec = Convert.ToDouble(GridMotorPara[i + 1, 14].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec = Convert.ToDouble(GridMotorPara[i + 1, 15].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec = Convert.ToDouble(GridMotorPara[i + 1, 16].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec = Convert.ToDouble(GridMotorPara[i + 1, 17].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec = Convert.ToDouble(GridMotorPara[i + 1, 18].Text);
                SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec = Convert.ToDouble(GridMotorPara[i + 1, 19].Text);

                // S/W Limit
                SystemData_Axis.MPMotionData[i].PosLimit.Plus = Convert.ToDouble(GridMotorPara[i + 1, 20].Text);
                SystemData_Axis.MPMotionData[i].PosLimit.Minus = Convert.ToDouble(GridMotorPara[i + 1, 21].Text);

                // Limit Time
                SystemData_Axis.MPMotionData[i].TimeLimit.tMoveLimit = Convert.ToDouble(GridMotorPara[i + 1, 22].Text);
                SystemData_Axis.MPMotionData[i].TimeLimit.tSleepAfterMove = Convert.ToDouble(GridMotorPara[i + 1, 23].Text);
                SystemData_Axis.MPMotionData[i].TimeLimit.tOriginLimit = Convert.ToDouble(GridMotorPara[i + 1, 24].Text);

                // Home Option
                SystemData_Axis.MPMotionData[i].OriginData.Method = Convert.ToInt16(GridMotorPara[i + 1, 25].Text);
                SystemData_Axis.MPMotionData[i].OriginData.Dir = Convert.ToInt16(GridMotorPara[i + 1, 26].Text);
                SystemData_Axis.MPMotionData[i].OriginData.FastSpeed = Convert.ToDouble(GridMotorPara[i + 1, 27].Text);
                SystemData_Axis.MPMotionData[i].OriginData.SlowSpeed = Convert.ToDouble(GridMotorPara[i + 1, 28].Text);
                SystemData_Axis.MPMotionData[i].OriginData.HomeOffset = Convert.ToDouble(GridMotorPara[i + 1, 29].Text);
            }

            for (i = 0; i < 3; i++)
            {
                // Speed
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel = Convert.ToDouble(GridMotorPara[i + 17, 2].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel = Convert.ToDouble(GridMotorPara[i + 17, 3].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel = Convert.ToDouble(GridMotorPara[i + 17, 4].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel = Convert.ToDouble(GridMotorPara[i + 17, 5].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel = Convert.ToDouble(GridMotorPara[i + 17, 6].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel = Convert.ToDouble(GridMotorPara[i + 17, 7].Text);

                // Acc
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc = Convert.ToDouble(GridMotorPara[i + 17, 8].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc = Convert.ToDouble(GridMotorPara[i + 17, 9].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc = Convert.ToDouble(GridMotorPara[i + 17, 10].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc = Convert.ToDouble(GridMotorPara[i + 17, 11].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc = Convert.ToDouble(GridMotorPara[i + 17, 12].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc = Convert.ToDouble(GridMotorPara[i + 17, 13].Text);

                // Dec
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec = Convert.ToDouble(GridMotorPara[i + 17, 14].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec = Convert.ToDouble(GridMotorPara[i + 17, 15].Text); 
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec = Convert.ToDouble(GridMotorPara[i + 17, 16].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec = Convert.ToDouble(GridMotorPara[i + 17, 17].Text);
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec = Convert.ToDouble(GridMotorPara[i + 17, 18].Text); 
                SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec = Convert.ToDouble(GridMotorPara[i + 17, 19].Text);

                // S/W Limit
                SystemData_Axis.ACSMotionData[i].PosLimit.Plus = Convert.ToDouble(GridMotorPara[i + 17, 20].Text);
                SystemData_Axis.ACSMotionData[i].PosLimit.Minus = Convert.ToDouble(GridMotorPara[i + 17, 21].Text);

                // Limit Time
                SystemData_Axis.ACSMotionData[i].TimeLimit.tMoveLimit = Convert.ToDouble(GridMotorPara[i + 17, 22].Text);
                SystemData_Axis.ACSMotionData[i].TimeLimit.tSleepAfterMove = Convert.ToDouble(GridMotorPara[i + 17, 23].Text);
                SystemData_Axis.ACSMotionData[i].TimeLimit.tOriginLimit = Convert.ToDouble(GridMotorPara[i + 17, 24].Text);
            }

            CMainFrame.LWDicer.m_DataManager.SaveSystemData(systemAxis: SystemData_Axis);
        }
    }
}
