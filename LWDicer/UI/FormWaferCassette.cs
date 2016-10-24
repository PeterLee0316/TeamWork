using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_DataManager;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

namespace LWDicer.UI
{
    public partial class FormWaferCassette : Form
    {
        public FormWaferCassette()
        {
            InitializeComponent();

            InitGrid(20);

            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
        }

        private void InitGrid(int nSlotCount)
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridCassette1.ActivateCurrentCellBehavior = GridCellActivateAction.None;
            GridCassette2.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCassette1.Properties.RowHeaders = true;
            GridCassette1.Properties.ColHeaders = true;

            GridCassette2.Properties.RowHeaders = true;
            GridCassette2.Properties.ColHeaders = true;

            nCol = 2;
            nRow = nSlotCount;

            // Column,Row 개수
            GridCassette1.ColCount = nCol;
            GridCassette1.RowCount = nRow;

            GridCassette2.ColCount = nCol;
            GridCassette2.RowCount = nRow;

            GridCassette1.ColWidths.SetSize(1, 141);
            GridCassette1.ColWidths.SetSize(2, 141);

            GridCassette2.ColWidths.SetSize(1, 141);
            GridCassette2.ColWidths.SetSize(2, 141);

            for (i = 0; i < nRow + 1; i++)
            {
                GridCassette1.RowHeights.SetSize(i, 26);
                GridCassette2.RowHeights.SetSize(i, 26);
            }

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridCassette1[j, i].Font.Bold = true;
                    GridCassette2[j, i].Font.Bold = true;

                    GridCassette1[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCassette1[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    GridCassette2[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCassette2[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;

                    if (i != 0 && j != 0)
                    {
                        GridCassette1[j, i].Text = "";
                        GridCassette1[j, i].TextColor = Color.Black;

                        GridCassette2[j, i].Text = "";
                        GridCassette2[j, i].TextColor = Color.Black;
                    }
                }
            }
            
            GridCassette1[0, 1].Text = "Status";
            GridCassette1[0, 2].Text = "ID";

            GridCassette2[0, 1].Text = "Status";
            GridCassette2[0, 2].Text = "ID";

            GridCassette1.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCassette1.ResizeColsBehavior = 0;
            GridCassette1.ResizeRowsBehavior = 0;

            GridCassette2.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCassette2.ResizeColsBehavior = 0;
            GridCassette2.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCassette1.Refresh();
            GridCassette2.Refresh();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {

        }

        private void FormWaferCassette_Load(object sender, EventArgs e)
        {
            InitGrid(20);
            TimerUI.Start();
        }

        private void FormWaferCassette_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
