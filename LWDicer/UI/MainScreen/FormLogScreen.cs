using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using LWDicer.Control;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Grid;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;


namespace LWDicer.UI
{
    public partial class FormLogScreen : Form
    {
        private DateTime m_StartDate;
        private DateTime m_EndDate;

        private List<CAlarm> m_AlarmList;

        public FormLogScreen()
        {
            InitializeComponent();

            InitializeForm();

            InitGrid();

            DateStart.Value = DateTime.Today;
            DateEnd.Value = DateTime.Today;

            m_StartDate = DateTime.Today;
            m_EndDate = DateTime.Today;

            m_AlarmList = CMainFrame.LWDicer.m_DataManager.AlarmHistory;
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void InitGrid(int nCount = 0)
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridEvent.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridEvent.Properties.RowHeaders = true;
            GridEvent.Properties.ColHeaders = true;

            nCol = 7;
            nRow = nCount;

            // Column,Row 개수
            GridEvent.ColCount = nCol;
            GridEvent.RowCount = nRow;

            // Column 가로 크기설정
            GridEvent.ColWidths.SetSize(0, 40);   // No.
            GridEvent.ColWidths.SetSize(1, 100);  // Process Name
            GridEvent.ColWidths.SetSize(2, 100);  // Obj.Name
            GridEvent.ColWidths.SetSize(3, 40);  // Code
            GridEvent.ColWidths.SetSize(4, 40);  // Type
            GridEvent.ColWidths.SetSize(5, 147);   // 발생시간  
            GridEvent.ColWidths.SetSize(6, 147);  // 해제시간
            GridEvent.ColWidths.SetSize(7, 325);   // 내용

            for (i = 0; i < nRow + 1; i++)
            {
                GridEvent.RowHeights[i] = 30;
            }

            // Text Display
            GridEvent[0, 0].Text = "No";
            GridEvent[0, 1].Text = "Pro.Name";
            GridEvent[0, 2].Text = "Obj.Name";
            GridEvent[0, 3].Text = "Code";
            GridEvent[0, 4].Text = "Type";
            GridEvent[0, 5].Text = "발생시간";
            GridEvent[0, 6].Text = "해제시간";
            GridEvent[0, 7].Text = "내                           용";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridEvent[j, i].Font.Bold = true;

                    GridEvent[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i == 6 && j > 0)
                    {
                        GridEvent[j, i].HorizontalAlignment = GridHorizontalAlignment.Left;
                    }
                    else
                    {
                        GridEvent[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }
                }
            }

            GridEvent.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridEvent.ResizeColsBehavior = 0;
            GridEvent.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridEvent.Refresh();

        }

        private void UpdataScreen()
        {
            int nEventCount = 0;

            CMainFrame.LWDicer.m_DataManager.LoadAlarmHistory();

            foreach (CAlarm AlarmInfo in m_AlarmList)
            {
                if(AlarmInfo.OccurTime.Date >= m_StartDate.Date && AlarmInfo.OccurTime.Date <= m_EndDate.Date)
                {
                    nEventCount++;

                    GridEvent.RowCount = nEventCount;

                    GridEvent[nEventCount, 0].Text = Convert.ToString(nEventCount);
                    GridEvent[nEventCount, 1].Text = Convert.ToString(AlarmInfo.ProcessName);
                    GridEvent[nEventCount, 2].Text = Convert.ToString(AlarmInfo.ObjectName);
                    GridEvent[nEventCount, 3].Text = Convert.ToString(AlarmInfo.Info.Index);
                    GridEvent[nEventCount, 4].Text = Convert.ToString(AlarmInfo.Info.Type);
                    GridEvent[nEventCount, 5].Text = Convert.ToString(AlarmInfo.OccurTime);
                    GridEvent[nEventCount, 5].Font.Size = 8;
                    GridEvent[nEventCount, 6].Text = Convert.ToString(AlarmInfo.ResetTime);
                    GridEvent[nEventCount, 6].Font.Size = 8;
                    GridEvent[nEventCount, 7].Text = Convert.ToString(AlarmInfo.Info.Description[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language]);

                    GridEvent[nEventCount, 0].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridEvent[nEventCount, 0].HorizontalAlignment = GridHorizontalAlignment.Center;

                    GridEvent.RowStyles[nEventCount].HorizontalAlignment = GridHorizontalAlignment.Center;
                    GridEvent.RowStyles[nEventCount].VerticalAlignment = GridVerticalAlignment.Middle;

                    GridEvent[nEventCount, 1].BackColor = Color.FromArgb(230, 210, 255);
                    GridEvent[nEventCount, 2].BackColor = Color.FromArgb(230, 210, 255);
                    GridEvent[nEventCount, 3].BackColor = Color.FromArgb(230, 210, 255);
                    GridEvent[nEventCount, 4].BackColor = Color.FromArgb(230, 210, 255);
                    GridEvent[nEventCount, 5].BackColor = Color.FromArgb(255, 230, 255);
                    GridEvent[nEventCount, 6].BackColor = Color.FromArgb(255, 230, 255);

                    GridEvent.RowHeights[nEventCount] = 30;
                }
            }

            LabelCount.Text = Convert.ToString(nEventCount);

            GridEvent.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridEvent.ResizeColsBehavior = 0;
            GridEvent.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridEvent.Refresh();
        }


        private void FormLogScreen_Activated(object sender, EventArgs e)
        {

        }

        private void BtnSerch_Click(object sender, EventArgs e)
        {
            UpdataScreen();
        }

        private void DateStart_ValueChanged(object sender, EventArgs e)
        {
            m_StartDate = DateStart.Value.Date;
        }

        private void DateEnd_ValueChanged(object sender, EventArgs e)
        {
            m_EndDate = DateEnd.Value.Date;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            LabelCount.Text = "";
            GridEvent.RowCount = 0;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            int iRow;
            int iCol;
            String strBuf, strFileName;

            strFileName = $"Alarm_{m_StartDate.Year}{m_StartDate.Month}{m_StartDate.Day}_{m_EndDate.Year}{m_EndDate.Month}{m_EndDate.Day}";

            //  저장할 경로 확인...
            SaveFileDialog savefile = new SaveFileDialog();

            savefile.CreatePrompt = true;       //존재하지 않는 파일을 지정할 때 파일을 새로 만들 것인지 대화 상자에 표시되면 true이고 ,대화 상자에서 자동으로 새 파일을 만들면 false. 
            savefile.OverwritePrompt = true;    //이미 존재하는 파일 이름을 지정할 때 Save As 대화 상자에 경고가 표시되는지 여부를 나타내는 값을 가져오거나 설정. 
            savefile.FileName = strFileName;
            savefile.DefaultExt = "csv";
            savefile.Filter = "Excel files (*.csv)|*.csv";
            // savefile.InitialDirectory

            DialogResult result = savefile.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(savefile.FileName, true, Encoding.GetEncoding("ks_c_5601-1987"));

                    for (iRow = 0; iRow <= GridEvent.RowCount; iRow++)
                    {
                        strBuf = "";
                        for (iCol = 0; iCol <= GridEvent.ColCount; iCol++)
                        {
                            if (iCol == 0 && iRow != 0)
                            {
                                strBuf += Convert.ToString(iRow);
                            }
                            else
                            {
                                strBuf += GridEvent[iRow, iCol].Text;
                            }

                            strBuf += " ,";
                        }
                        sw.WriteLine(strBuf);
                    }

                    sw.Close();
                    sw.Dispose();
                }
                catch (Exception)
                {

                }
            }
        }

        private void GridEvent_CellClick(object sender, GridCellClickEventArgs e)
        {
            if(e.ColIndex == 0 || e.RowIndex == 0)
            {
                return;
            }

            if(CMainFrame.LWDicer.m_DataManager.AlarmInfoList.Count < Convert.ToInt16(GridEvent[e.RowIndex, 3].Text))
            {
                LabelTrouble.Text = "";
                return;
            }

            LabelTrouble.TextAlign = ContentAlignment.TopLeft;

            LabelTrouble.Text = 
                CMainFrame.LWDicer.m_DataManager.AlarmInfoList[Convert.ToInt16(GridEvent[e.RowIndex, 3].Text)].Solution[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language];
        }
    }
}
