using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Grid;

using Core.Layers;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_LCNet;

namespace Core.UI
{
    public partial class FormWorkPieceInquiry : Form
    {
        private CWorkPiece m_WorkPiece = new CWorkPiece();
        public FormWorkPieceInquiry()
        {
            InitializeComponent();

            InitGrid();
            InitCombo();
        }

        private void FormWorkPieceInquiry_Load(object sender, EventArgs e)
        {

        }

        public void InitCombo()
        {
            int index = ComboType.SelectedIndex;
            ComboType.Items.Clear();

            // workpiece list
            for (int i = 0; i < (int)ELCNetUnitPos.MAX; i++)
            {
                ComboType.Items.Add($"{(ELCNetUnitPos.PUSHPULL + i).ToString()}");

            }

            for (int i = 0; i < CMainFrame.DataManager.WorkPiece_InputBuffer.Count; i++)
            {
                ComboType.Items.Add($"InputBuffer_{i}");
            }

            for (int i = 0; i < CMainFrame.DataManager.WorkPiece_OutputBuffer.Count; i++)
            {
                ComboType.Items.Add($"OutputBuffer_{i}");
            }

            if(index < 0 || index >= ComboType.Items.Count)
            {
                ComboType.SelectedIndex = 0;
            } else
            {
                ComboType.SelectedIndex = index;
            }

        }

        private void InitGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridCont.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCont.Properties.RowHeaders = true;
            GridCont.Properties.ColHeaders = true;

            int nCol = 5;
            int nRow = 0;

            // Column,Row 개수
            GridCont.ColCount = nCol;
            GridCont.RowCount = nRow;

            // Column 가로 크기설정
            GridCont.ColWidths.SetSize(0, 40);   // No.
            GridCont.ColWidths.SetSize(1, 220);
            GridCont.ColWidths.SetSize(2, 60); 
            GridCont.ColWidths.SetSize(3, 140);
            GridCont.ColWidths.SetSize(4, 140);
            GridCont.ColWidths.SetSize(5, 80);                   

            for (int i = 0; i < nRow + 1; i++)
            {
                GridCont.RowHeights[i] = 30;
                GridCont[0, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                GridCont[0, i].VerticalAlignment = GridVerticalAlignment.Middle;
            }

            // Text Display
            GridCont[0, 0].Text = "No";
            GridCont[0, 1].Text = "Process";
            GridCont[0, 2].Text = "worked";
            GridCont[0, 3].Text = "begin time";
            GridCont[0, 4].Text = "finish time";
            GridCont[0, 5].Text = "elapsed time";

            GridCont.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCont.ResizeColsBehavior = 0;
            GridCont.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCont.Refresh();
        }

        private void DisplayGrid()
        {
            //
            Label_ID.Text = $"ID : {m_WorkPiece.ID}";
            string str = m_WorkPiece.Time_Created.Year > 2000 ? m_WorkPiece.Time_Created.ToString("yyyy-MM-dd HH:mm:ss") : "";
            Label_Created.Text = $"created : {str}";
            str = m_WorkPiece.Time_LoadFromCassette.Year > 2000 ? m_WorkPiece.Time_LoadFromCassette.ToString("yyyy-MM-dd HH:mm:ss") : "";
            Label_TimeBegin.Text = $"from cassette : {str}";
            str = m_WorkPiece.Time_UnloadToCassette.Year > 2000 ? m_WorkPiece.Time_UnloadToCassette.ToString("yyyy-MM-dd HH:mm:ss") : "";
            Label_TimeFinish.Text = $"to cassette : {str}";

            // grid control
            GridCont.RowCount = (int)EProcessPhase.MAX;
            GridCont.Clear(true);

            for (int i = 0; i < GridCont.RowCount; i++)
            {
                int nIndex = i + 1;

                GridCont[i + 1, 0].Text = String.Format("{0:0}", nIndex);
                GridCont[i + 1, 1].Text = (EProcessPhase.PUSHPULL_LOAD_FROM_LOADER + i).ToString();
                GridCont[i + 1, 2].Text = m_WorkPiece.Process[i].IsFinished ? "done" : "not yet";
                GridCont[i + 1, 3].Text = m_WorkPiece.Process[i].Time_Start.Year > 2000 ? m_WorkPiece.Process[i].Time_Start.ToString("yyyy-MM-dd HH:mm:ss") : "";
                GridCont[i + 1, 4].Text = m_WorkPiece.Process[i].Time_Finish.Year > 2000 ? m_WorkPiece.Process[i].Time_Finish.ToString("yyyy-MM-dd HH:mm:ss") : "";

                if(m_WorkPiece.Process[i].IsFinished)
                {
                    TimeSpan t = m_WorkPiece.Process[i].Time_Finish - m_WorkPiece.Process[i].Time_Start;
                    GridCont[i + 1, 5].Text = $"{t.Hours}:{t.Minutes}:{t.Seconds}";
                } else
                {
                    GridCont[i + 1, 5].Text = "";
                }
                GridCont[i + 1, 0].VerticalAlignment = GridVerticalAlignment.Middle;
                GridCont[i + 1, 0].HorizontalAlignment = GridHorizontalAlignment.Center;

                GridCont.RowStyles[i + 1].HorizontalAlignment = GridHorizontalAlignment.Center;
                GridCont.RowStyles[i + 1].VerticalAlignment = GridVerticalAlignment.Middle;

                GridCont[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                GridCont[i + 1, 2].BackColor = Color.FromArgb(230, 210, 255);
                GridCont[i + 1, 3].BackColor = Color.White;
                GridCont[i + 1, 4].BackColor = Color.White;
                GridCont[i + 1, 5].BackColor = Color.White;

                GridCont.RowHeights[i + 1] = 20;
            }

            GridCont.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCont.ResizeColsBehavior = 0;
            GridCont.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCont.Refresh();
        }

        private void ComboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index;
            string str = ComboType.Text;
            
            if(str.IndexOf("InputBuffer_") >= 0)
            {
                index = Convert.ToInt32(str.Replace("InputBuffer_", ""));
                if(index >= CMainFrame.DataManager.WorkPiece_InputBuffer.Count)
                {
                    InitCombo();
                    return;
                }
                m_WorkPiece = CMainFrame.DataManager.WorkPiece_InputBuffer[index];
            } else if (str.IndexOf("OutputBuffer_") >= 0)
            {
                index = Convert.ToInt32(str.Replace("OutputBuffer_", ""));
                if (index >= CMainFrame.DataManager.WorkPiece_OutputBuffer.Count)
                {
                    InitCombo();
                    return;
                }
                m_WorkPiece = CMainFrame.DataManager.WorkPiece_OutputBuffer[index];
            } else
            {
                for(int i = 0; i < (int)ELCNetUnitPos.MAX; i++)
                {
                    if (str != (ELCNetUnitPos.PUSHPULL + i).ToString()) continue;
                    m_WorkPiece = CMainFrame.DataManager.WorkPieceArray[i];
                    break;
                }
            }
            DisplayGrid();
        }

        private void FormWorkPieceInquiry_Activated(object sender, EventArgs e)
        {
            InitCombo();
            DisplayGrid();
        }
    }
}
