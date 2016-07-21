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
using static LWDicer.Control.MDataManager;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormLogScreen : Form
    {
        private DateTime m_StartDate;
        private DateTime m_EndDate;

        private List<CAlarm> m_AlarmList;

        private int [] nProcessID;

        private int nPageNo;

        private DataTable datatable;

        int nLogOption;

        const int SelAlarm = 0;
        const int SelEvent = 1;
        const int SelDev = 2;

        public FormLogScreen()
        {
            InitializeComponent();

            InitializeForm();

            InitGrid(SelAlarm);

            DateStart.Value = DateTime.Today;
            DateEnd.Value = DateTime.Today;

            m_StartDate = DateTime.Today;
            m_EndDate = DateTime.Today;

            m_AlarmList = CMainFrame.LWDicer.m_DataManager.AlarmHistory;

            SetLogOption(BtnSelectAlarm);

        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void InitGrid(int nLogOption)
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridCont.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCont.Properties.RowHeaders = true;
            GridCont.Properties.ColHeaders = true;

            switch (nLogOption)
            {
                case SelAlarm:
                    nCol = 7;
                    nRow = 0;

                    // Column,Row 개수
                    GridCont.ColCount = nCol;
                    GridCont.RowCount = nRow;

                    // Column 가로 크기설정
                    GridCont.ColWidths.SetSize(0, 40);   // No.
                    GridCont.ColWidths.SetSize(1, 120);  // Process Name
                    GridCont.ColWidths.SetSize(2, 120);  // Obj.Name
                    GridCont.ColWidths.SetSize(3, 60);  // Code
                    GridCont.ColWidths.SetSize(4, 60);  // Type
                    GridCont.ColWidths.SetSize(5, 160);   // 발생시간  
                    GridCont.ColWidths.SetSize(6, 160);  // 해제시간
                    GridCont.ColWidths.SetSize(7, 455);   // 내용

                    for (i = 0; i < nRow + 1; i++)
                    {
                        GridCont.RowHeights[i] = 30;
                    }

                    // Text Display
                    GridCont[0, 0].Text = "No";
                    GridCont[0, 1].Text = "Pro.Name";
                    GridCont[0, 2].Text = "Obj.Name";
                    GridCont[0, 3].Text = "Code";
                    GridCont[0, 4].Text = "Type";
                    GridCont[0, 5].Text = "발생시간";
                    GridCont[0, 6].Text = "해제시간";
                    GridCont[0, 7].Text = "내                           용";
                    break;


                case SelEvent:
                case SelDev:
                    nCol = 6;
                    nRow = 0;

                    // Column,Row 개수
                    GridCont.ColCount = nCol;
                    GridCont.RowCount = nRow;

                    // Column 가로 크기설정
                    GridCont.ColWidths.SetSize(0, 60);   // No.
                    GridCont.ColWidths.SetSize(1, 160);  // Time
                    GridCont.ColWidths.SetSize(2, 140);  // Name
                    GridCont.ColWidths.SetSize(3, 75);  // Type
                    GridCont.ColWidths.SetSize(4, 500);  // Comment
                    GridCont.ColWidths.SetSize(5, 140);   // File  
                    GridCont.ColWidths.SetSize(6, 75);   // Line  

                    for (i = 0; i < nRow + 1; i++)
                    {
                        GridCont.RowHeights[i] = 30;
                    }

                    // Text Display
                    GridCont[0, 0].Text = "No";
                    GridCont[0, 1].Text = "Time";
                    GridCont[0, 2].Text = "Name";
                    GridCont[0, 3].Text = "Type";
                    GridCont[0, 4].Text = "Comment";
                    GridCont[0, 5].Text = "File";
                    GridCont[0, 6].Text = "Line";
                    break;
            }

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridCont[j, i].Font.Bold = true;

                    GridCont[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i == 6 && j > 0)
                    {
                        GridCont[j, i].HorizontalAlignment = GridHorizontalAlignment.Left;
                    }
                    else
                    {
                        GridCont[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }
                }
            }

            GridCont.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCont.ResizeColsBehavior = 0;
            GridCont.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCont.Refresh();

        }

        private void UpdataScreen(int nOption)
        {
            InitGrid(nOption);

            switch (nOption)
            {
                case SelAlarm:
                    UpdataAlarm();
                    break;

                case SelEvent:
                case SelDev:
                    UpdataEvent(nOption);
                    break;
            }
        }

        private void UpdataAlarm()
        {
            int nEventCount = 0;

            CMainFrame.LWDicer.m_DataManager.LoadAlarmHistory();

            nProcessID = new int[m_AlarmList.Count];

            foreach (CAlarm AlarmInfo in m_AlarmList)
            {
                if (AlarmInfo.OccurTime.Date >= m_StartDate.Date && AlarmInfo.OccurTime.Date <= m_EndDate.Date)
                {
                    nProcessID[nEventCount] = AlarmInfo.ProcessID;

                    nEventCount++;

                    GridCont.RowCount = nEventCount;

                    GridCont[nEventCount, 0].Text = Convert.ToString(nEventCount);
                    GridCont[nEventCount, 1].Text = Convert.ToString(AlarmInfo.ProcessName);
                    GridCont[nEventCount, 2].Text = Convert.ToString(AlarmInfo.ObjectName);
                    GridCont[nEventCount, 3].Text = Convert.ToString(AlarmInfo.Info.Index);
                    GridCont[nEventCount, 4].Text = Convert.ToString(AlarmInfo.Info.Type);
                    GridCont[nEventCount, 5].Text = Convert.ToString(AlarmInfo.OccurTime);
                    GridCont[nEventCount, 6].Text = Convert.ToString(AlarmInfo.ResetTime);
                    GridCont[nEventCount, 7].Text = Convert.ToString(AlarmInfo.Info.Description[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language]);

                    GridCont[nEventCount, 0].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCont[nEventCount, 0].HorizontalAlignment = GridHorizontalAlignment.Center;

                    GridCont.RowStyles[nEventCount].HorizontalAlignment = GridHorizontalAlignment.Center;
                    GridCont.RowStyles[nEventCount].VerticalAlignment = GridVerticalAlignment.Middle;

                    GridCont[nEventCount, 1].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[nEventCount, 2].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[nEventCount, 3].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[nEventCount, 4].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[nEventCount, 5].BackColor = Color.FromArgb(255, 230, 255);
                    GridCont[nEventCount, 6].BackColor = Color.FromArgb(255, 230, 255);

                    GridCont.RowHeights[nEventCount] = 30;
                }
            }

            LabelCount.Text = Convert.ToString(nEventCount);

            GridCont.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCont.ResizeColsBehavior = 0;
            GridCont.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCont.Refresh();
        }

        private int UpdataEvent(int nOption)
        {
            // 뭔지 모르겠으나 Query 문 검색 스타트 날자 조건이 걸리지 않아 강제로 하루를 늦춤
            DateTime time = m_StartDate.AddDays(-1);
            m_StartDate = time;

            if (nOption == SelEvent)
            {
                string query = $"SELECT * FROM {CMainFrame.LWDicer.m_DataManager.DBInfo.TableEventLog} WHERE Time BETWEEN '{m_StartDate.Date}' AND '{m_EndDate.Date}' ORDER BY Time DESC";

                if (DBManager.GetTable(CMainFrame.LWDicer.m_DataManager.DBInfo.DBConn_ELog, query, out datatable) != true)
                {
                    return CMainFrame.LWDicer.GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST);
                }
            }
            else
            {
                string query = $"SELECT * FROM {CMainFrame.LWDicer.m_DataManager.DBInfo.TableDebugLog} WHERE Time BETWEEN '{m_StartDate.Date}' AND '{m_EndDate.Date}' ORDER BY Time DESC";

                if (DBManager.GetTable(CMainFrame.LWDicer.m_DataManager.DBInfo.DBConn_DLog, query, out datatable) != true)
                {
                    return CMainFrame.LWDicer.GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST);
                }
            }

            DisplayGrid(0);

            return SUCCESS;
        }

        private void DisplayGrid(int nPage)
        {
            int nEventCount = 0;
            int nTotalPage;

            GridCont.RowCount = 23;
            GridCont.Clear(true);

            nTotalPage = datatable.Rows.Count / GridCont.RowCount;

            for(int i=0;i< GridCont.RowCount;i++)
            {
                if(datatable.Rows[i + (nPage * GridCont.RowCount)]["Time"].ToString() == "")
                {
                    break;
                }

                GridCont[i + 1, 0].Text = Convert.ToString(i + (nPage * GridCont.RowCount));
                GridCont[i+1, 1].Text = Convert.ToString(datatable.Rows[i + (nPage * GridCont.RowCount)]["Time"].ToString());
                GridCont[i + 1, 2].Text = Convert.ToString(datatable.Rows[i + (nPage* GridCont.RowCount)]["Name"].ToString());
                GridCont[i + 1, 3].Text = Convert.ToString(datatable.Rows[i + (nPage* GridCont.RowCount)]["Type"].ToString());
                GridCont[i + 1, 4].Text = Convert.ToString(datatable.Rows[i + (nPage* GridCont.RowCount)]["Comment"].ToString());
                GridCont[i + 1, 5].Text = Convert.ToString(datatable.Rows[i + (nPage* GridCont.RowCount)]["File"].ToString());
                GridCont[i + 1, 6].Text = Convert.ToString(datatable.Rows[i + (nPage * GridCont.RowCount)]["Line"].ToString());

                GridCont[i + 1, 0].VerticalAlignment = GridVerticalAlignment.Middle;
                GridCont[i + 1, 0].HorizontalAlignment = GridHorizontalAlignment.Center;

                GridCont.RowStyles[i + 1].HorizontalAlignment = GridHorizontalAlignment.Center;
                GridCont.RowStyles[i + 1].VerticalAlignment = GridVerticalAlignment.Middle;

                GridCont[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                GridCont[i + 1, 2].BackColor = Color.FromArgb(230, 210, 255);
                GridCont[i + 1, 3].BackColor = Color.FromArgb(230, 210, 255);
                GridCont[i + 1, 4].BackColor = Color.White;
                GridCont[i + 1, 5].BackColor = Color.FromArgb(255, 230, 255);
                GridCont[i + 1, 6].BackColor = Color.FromArgb(255, 230, 255);

                GridCont.RowHeights[i + 1] = 30;
            }

            LabelCount.Text = Convert.ToString(datatable.Rows.Count);

            GridCont.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCont.ResizeColsBehavior = 0;
            GridCont.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCont.Refresh();
        }

        private void FormLogScreen_Activated(object sender, EventArgs e)
        {

        }

        private void BtnSerch_Click(object sender, EventArgs e)
        {
            UpdataScreen(nLogOption);
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
            GridCont.RowCount = 0;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            int iRow;
            int iCol;
            String strBuf, strFileName=string.Empty;

            if (nLogOption == SelAlarm) strFileName = $"Alarm_{m_StartDate.Year}{m_StartDate.Month}{m_StartDate.Day}_{m_EndDate.Year}{m_EndDate.Month}{m_EndDate.Day}";
            if (nLogOption == SelEvent) strFileName = $"Event_{m_StartDate.Year}{m_StartDate.Month}{m_StartDate.Day}_{m_EndDate.Year}{m_EndDate.Month}{m_EndDate.Day}";
            if (nLogOption == SelDev) strFileName = $"Dev_{m_StartDate.Year}{m_StartDate.Month}{m_StartDate.Day}_{m_EndDate.Year}{m_EndDate.Month}{m_EndDate.Day}";

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

                    for (iRow = 0; iRow <= GridCont.RowCount; iRow++)
                    {
                        strBuf = "";
                        for (iCol = 0; iCol <= GridCont.ColCount; iCol++)
                        {
                            if (iCol == 0 && iRow != 0)
                            {
                                strBuf += Convert.ToString(iRow);
                            }
                            else
                            {
                                strBuf += GridCont[iRow, iCol].Text;
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
      
        private void GridEvent_CellDoubleClick(object sender, GridCellClickEventArgs e)
        {
            if(e.RowIndex == 0 || e.ColIndex == 0 || nLogOption != SelAlarm)
            {
                return;
            }

            int nCode = Convert.ToInt16(GridCont[e.RowIndex, 3].Text);

            CAlarm alarm = CMainFrame.LWDicer.GetAlarmInfo(nCode, nProcessID[e.RowIndex-1], false);
            var dlg = new FormAlarmDisplay(alarm);
            dlg.ShowDialog();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            Button BtnSel = sender as Button;
            SetLogOption(BtnSel);
        }

        private void SetLogOption(Button BtnSel)
        {
            BtnSelectAlarm.Image = Image.Images[0]; BtnSelectAlarm.BackColor = Color.LightGray;
            BtnSelectEvent.Image = Image.Images[0]; BtnSelectEvent.BackColor = Color.LightGray;
            BtnSelectDev.Image = Image.Images[0];   BtnSelectDev.BackColor = Color.LightGray;

            BtnSel.Image = Image.Images[1];
            BtnSel.BackColor = Color.LightSalmon;

            if (BtnSelectAlarm.Name == BtnSel.Name) { nLogOption = SelAlarm; };
            if (BtnSelectEvent.Name == BtnSel.Name) { nLogOption = SelEvent; };
            if (BtnSelectDev.Name == BtnSel.Name) { nLogOption = SelDev; };
                        
        }

        private void BtnPageTop_Click(object sender, EventArgs e)
        {
            nPageNo = 0;
            DisplayGrid(nPageNo);
        }

        private void BtnPageUp_Click(object sender, EventArgs e)
        {
            nPageNo--;

            if (nPageNo < 0)
            {
                nPageNo = 0;
                return;
            }

            DisplayGrid(nPageNo);
        }

        private void BtnPageDown_Click(object sender, EventArgs e)
        {
            int nTotalPage;
            nTotalPage = datatable.Rows.Count / GridCont.RowCount;

            if(nTotalPage < nPageNo)
            {
                return;
            }

            nPageNo++;
            DisplayGrid(nPageNo);
        }

        private void BtnPageBot_Click(object sender, EventArgs e)
        {
            nPageNo = (datatable.Rows.Count / GridCont.RowCount) - 1;

            DisplayGrid(nPageNo);
        }
    }
}
