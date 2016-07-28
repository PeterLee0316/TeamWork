using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        ELogDisplayType LogType;

        const int ResultRowCount = 23;

        enum ELogDisplayType
        {
            ALARM,
            EVENT,
            LOGIN,
            DEVELOPER,
        }

        public FormLogScreen()
        {
            InitializeComponent();

            InitializeForm();

            DateStart.Value = DateTime.Today;
            DateEnd.Value = DateTime.Today;

            m_StartDate = DateTime.Today;
            m_EndDate = DateTime.Today;

            m_AlarmList = CMainFrame.LWDicer.m_DataManager.AlarmHistory;

            ComboType.Items.Add($"{ELogDisplayType.ALARM}");
            ComboType.Items.Add($"{ELogDisplayType.EVENT}");
            ComboType.Items.Add($"{ELogDisplayType.LOGIN}");

            // test 기간동안은 풀어놓자
            //if(CMainFrame.DataManager.LoginInfo.User.Type == ELoginType.MAKER)
            {
                ComboType.Items.Add($"{ELogDisplayType.DEVELOPER}");
            }
            ComboType.SelectedIndex = 0;

            InitGrid();
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridCont.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCont.Properties.RowHeaders = true;
            GridCont.Properties.ColHeaders = true;

            switch (LogType)
            {
                case ELogDisplayType.ALARM:
                    nCol = 8;
                    nRow = 0;

                    // Column,Row 개수
                    GridCont.ColCount = nCol;
                    GridCont.RowCount = nRow;

                    // Column 가로 크기설정
                    GridCont.ColWidths.SetSize(0, 40);   // No.
                    GridCont.ColWidths.SetSize(1, 115);  // Process Name
                    GridCont.ColWidths.SetSize(2, 115);  // Obj.Name
                    GridCont.ColWidths.SetSize(3, 60);  // Code
                    GridCont.ColWidths.SetSize(4, 60);  // Type
                    GridCont.ColWidths.SetSize(5, 160);   // 발생시간  
                    GridCont.ColWidths.SetSize(6, 160);  // 해제시간
                    GridCont.ColWidths.SetSize(7, 400);   // 내용
                    GridCont.ColWidths.SetSize(8, 55);   // pid

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
                    GridCont[0, 8].Text = "P.ID";
                    break;

                case ELogDisplayType.EVENT:
                case ELogDisplayType.DEVELOPER:
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

                case ELogDisplayType.LOGIN:
                    nCol = 5;
                    nRow = 0;

                    // Column,Row 개수
                    GridCont.ColCount = nCol;
                    GridCont.RowCount = nRow;

                    // Column 가로 크기설정
                    GridCont.ColWidths.SetSize(0, 60);   // No.
                    GridCont.ColWidths.SetSize(1, 160);  // access time
                    GridCont.ColWidths.SetSize(2, 160);  // access type
                    GridCont.ColWidths.SetSize(3, 160);  // name
                    GridCont.ColWidths.SetSize(4, 160);  // Comment
                    GridCont.ColWidths.SetSize(5, 160);   // group

                    for (i = 0; i < nRow + 1; i++)
                    {
                        GridCont.RowHeights[i] = 30;
                    }

                    // Text Display
                    GridCont[0, 0].Text = "No";
                    GridCont[0, 1].Text = "Access Time";
                    GridCont[0, 2].Text = "Access Type";
                    GridCont[0, 3].Text = "Name";
                    GridCont[0, 4].Text = "Comment";
                    GridCont[0, 5].Text = "Group";
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

        private void UpdataScreen()
        {
            InitGrid();
            UpdataEvent();
        }

        private void UpdataEvent()
        {
            // 뭔지 모르겠으나 Query 문 검색 스타트 날자 조건이 걸리지 않아 강제로 하루를 늦춤
            DateTime time = m_StartDate.AddDays(-1);
            m_StartDate = time;

            if (LogType == ELogDisplayType.ALARM)
            {
                string query = $"SELECT * FROM {CMainFrame.LWDicer.m_DataManager.DBInfo.TableAlarmHistory} WHERE OccurTime BETWEEN '{m_StartDate.Date}' AND '{m_EndDate.Date}' ORDER BY OccurTime DESC";

                if (DBManager.GetTable(CMainFrame.LWDicer.m_DataManager.DBInfo.DBConn_ELog, query, out datatable) != true)
                {
                    CMainFrame.DisplayMsg("Failed to query database");
                    return;
                }
            }
            else if (LogType == ELogDisplayType.EVENT)
            {
                string query = $"SELECT * FROM {CMainFrame.LWDicer.m_DataManager.DBInfo.TableEventLog} WHERE Time BETWEEN '{m_StartDate.Date}' AND '{m_EndDate.Date}' ORDER BY Time DESC";

                if (DBManager.GetTable(CMainFrame.LWDicer.m_DataManager.DBInfo.DBConn_ELog, query, out datatable) != true)
                {
                    CMainFrame.DisplayMsg("Failed to query database");
                    return;
                }
            }
            else if (LogType == ELogDisplayType.LOGIN)
            {
                string query = $"SELECT * FROM {CMainFrame.LWDicer.m_DataManager.DBInfo.TableLoginHistory} WHERE AccessTime BETWEEN '{m_StartDate.Date}' AND '{m_EndDate.Date}' ORDER BY AccessTime DESC";

                if (DBManager.GetTable(CMainFrame.LWDicer.m_DataManager.DBInfo.DBConn_ELog, query, out datatable) != true)
                {
                    CMainFrame.DisplayMsg("Failed to query database");
                    return;
                }
            }
            else if (LogType == ELogDisplayType.DEVELOPER)
            {
                string query = $"SELECT * FROM {CMainFrame.LWDicer.m_DataManager.DBInfo.TableDebugLog} WHERE Time BETWEEN '{m_StartDate.Date}' AND '{m_EndDate.Date}' ORDER BY Time DESC";

                if (DBManager.GetTable(CMainFrame.LWDicer.m_DataManager.DBInfo.DBConn_DLog, query, out datatable) != true)
                {
                    CMainFrame.DisplayMsg("Failed to query database");
                    return;
                }
            }

            nPageNo = 0;
            DisplayGrid();
        }

        private void DisplayGrid()
        {
            if (datatable == null) return;
            int nEventCount = 0;
            int nTotalPage;

            GridCont.RowCount = ResultRowCount;
            GridCont.Clear(true);

            nTotalPage = datatable.Rows.Count / GridCont.RowCount;

            for(int i=0;i< GridCont.RowCount;i++)
            {
                int nIndex = i + (nPageNo * GridCont.RowCount);

                if(nIndex >= datatable.Rows.Count)
                {
                    GridCont.Rows.RemoveRange(i + 1, GridCont.RowCount);
                    break;
                }

                GridCont[i + 1, 0].Text = Convert.ToString(nIndex);
                if(LogType == ELogDisplayType.DEVELOPER || LogType == ELogDisplayType.EVENT)
                {
                    GridCont[i + 1, 1].Text = Convert.ToString(datatable.Rows[nIndex]["Time"].ToString());
                    GridCont[i + 1, 2].Text = Convert.ToString(datatable.Rows[nIndex]["Name"].ToString());
                    GridCont[i + 1, 3].Text = Convert.ToString(datatable.Rows[nIndex]["Type"].ToString());
                    GridCont[i + 1, 4].Text = Convert.ToString(datatable.Rows[nIndex]["Comment"].ToString());
                    GridCont[i + 1, 5].Text = Convert.ToString(datatable.Rows[nIndex]["File"].ToString());
                    GridCont[i + 1, 6].Text = Convert.ToString(datatable.Rows[nIndex]["Line"].ToString());

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
                }
                else if (LogType == ELogDisplayType.LOGIN)
                {
                    GridCont[i + 1, 1].Text = Convert.ToString(datatable.Rows[nIndex]["accesstime"].ToString());
                    GridCont[i + 1, 2].Text = Convert.ToString(datatable.Rows[nIndex]["accesstype"].ToString());
                    GridCont[i + 1, 3].Text = Convert.ToString(datatable.Rows[nIndex]["name"].ToString());
                    GridCont[i + 1, 4].Text = Convert.ToString(datatable.Rows[nIndex]["Comment"].ToString());
                    GridCont[i + 1, 5].Text = Convert.ToString(datatable.Rows[nIndex]["type"].ToString());

                    GridCont[i + 1, 0].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCont[i + 1, 0].HorizontalAlignment = GridHorizontalAlignment.Center;

                    GridCont.RowStyles[i + 1].HorizontalAlignment = GridHorizontalAlignment.Center;
                    GridCont.RowStyles[i + 1].VerticalAlignment = GridVerticalAlignment.Middle;

                    GridCont[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[i + 1, 2].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[i + 1, 3].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[i + 1, 4].BackColor = Color.White;
                    GridCont[i + 1, 5].BackColor = Color.FromArgb(255, 230, 255);
                }
                else if (LogType == ELogDisplayType.ALARM)
                {
                    CAlarm alarm = JsonConvert.DeserializeObject<CAlarm>(datatable.Rows[nIndex]["data"].ToString());
                    GridCont[i + 1, 1].Text = Convert.ToString(alarm.ProcessName);
                    GridCont[i + 1, 2].Text = Convert.ToString(alarm.ObjectName);
                    GridCont[i + 1, 3].Text = Convert.ToString(alarm.Info.Index);
                    GridCont[i + 1, 4].Text = Convert.ToString(alarm.Info.Type);
                    GridCont[i + 1, 5].Text = Convert.ToString(alarm.OccurTime);
                    GridCont[i + 1, 6].Text = Convert.ToString(alarm.ResetTime);
                    GridCont[i + 1, 7].Text = Convert.ToString(alarm.Info.Description[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language]);
                    GridCont[i + 1, 8].Text = Convert.ToString(alarm.ProcessID);

                    GridCont[i + 1, 0].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCont[i + 1, 0].HorizontalAlignment = GridHorizontalAlignment.Center;

                    GridCont.RowStyles[i + 1].HorizontalAlignment = GridHorizontalAlignment.Center;
                    GridCont.RowStyles[i + 1].VerticalAlignment = GridVerticalAlignment.Middle;

                    GridCont[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[i + 1, 2].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[i + 1, 3].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[i + 1, 4].BackColor = Color.FromArgb(230, 210, 255);
                    GridCont[i + 1, 5].BackColor = Color.FromArgb(255, 230, 255);
                    GridCont[i + 1, 6].BackColor = Color.FromArgb(255, 230, 255);
                }

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
            GridCont.RowCount = 0;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            int iRow;
            int iCol;
            String strBuf, strFileName = $"_{m_StartDate.Year}{m_StartDate.Month}{m_StartDate.Day}_{m_EndDate.Year}{m_EndDate.Month}{m_EndDate.Day}";

            if (datatable == null) return;

            switch (LogType)
            {
                case ELogDisplayType.ALARM:
                    strFileName = "Alarm" + strFileName;
                    break;

                case ELogDisplayType.EVENT:
                    strFileName = "Event" + strFileName;
                    break;

                case ELogDisplayType.LOGIN:
                    strFileName = "Login" + strFileName;
                    break;

                case ELogDisplayType.DEVELOPER:
                default:
                    strFileName = "Dev" + strFileName;
                    break;
            }

            //  저장할 경로 확인...
            SaveFileDialog savefile = new SaveFileDialog();

            savefile.CreatePrompt = true;       //존재하지 않는 파일을 지정할 때 파일을 새로 만들 것인지 대화 상자에 표시되면 true이고 ,대화 상자에서 자동으로 새 파일을 만들면 false. 
            savefile.OverwritePrompt = true;    //이미 존재하는 파일 이름을 지정할 때 Save As 대화 상자에 경고가 표시되는지 여부를 나타내는 값을 가져오거나 설정. 
            savefile.FileName = strFileName;
            savefile.DefaultExt = "csv";
            savefile.Filter = "Excel files (*.csv)|*.csv";
            // savefile.InitialDirectory

            DialogResult result = savefile.ShowDialog();
            if (result != DialogResult.OK) return;

            try
            {
                StreamWriter sw = new StreamWriter(savefile.FileName, true, Encoding.GetEncoding("ks_c_5601-1987"));

                if (LogType == ELogDisplayType.DEVELOPER || LogType == ELogDisplayType.EVENT)
                {
                    // 첫줄 타이틀
                    strBuf = "Time," + "Name," + "Type," + "Comment," + "File," + "Line,";
                    sw.WriteLine(strBuf);

                    for (iRow = 0 ; iRow < datatable.Rows.Count ; iRow++)
                    {
                        strBuf = "";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["Time"].ToString()) + " ,";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["Name"].ToString()) + " ,";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["Type"].ToString()) + " ,";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["Comment"].ToString()).Replace(",", ".") + " ,";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["File"].ToString()) + " ,";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["Line"].ToString()) + " ,";

                        sw.WriteLine(strBuf);
                    }
                }

                if (LogType == ELogDisplayType.LOGIN)
                {
                    // 첫줄 타이틀
                    strBuf = "Access Time," + "Access Type," + "Name," + "Comment," + "Group,";
                    sw.WriteLine(strBuf);

                    for (iRow = 0; iRow < datatable.Rows.Count; iRow++)
                    {
                        strBuf = "";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["accesstime"].ToString()) + " ,";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["accesstype"].ToString()) + " ,";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["name"].ToString()) + " ,";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["Comment"].ToString()).Replace(",", ".") + " ,";
                        strBuf = strBuf + Convert.ToString(datatable.Rows[iRow]["type"].ToString()) + " ,";

                        sw.WriteLine(strBuf);
                    }
                }

                if (LogType == ELogDisplayType.ALARM)
                {
                    // 첫줄 타이틀
                    strBuf = "Pro.Name," + "Obj.Name," + "Code," + "Type," + "발생시간," + "해제시간," + "내용," + "Process ID,";
                    sw.WriteLine(strBuf);

                    for (iRow = 0; iRow < datatable.Rows.Count; iRow++)
                    {
                        CAlarm alarm = JsonConvert.DeserializeObject<CAlarm>(datatable.Rows[iRow]["data"].ToString());

                        strBuf = "";
                        strBuf = strBuf + Convert.ToString(alarm.ProcessName) + " ,";
                        strBuf = strBuf + Convert.ToString(alarm.ObjectName) + " ,";
                        strBuf = strBuf + Convert.ToString(alarm.Info.Index) + " ,";
                        strBuf = strBuf + Convert.ToString(alarm.Info.Type) + " ,";
                        strBuf = strBuf + Convert.ToString(alarm.OccurTime) + " ,";
                        strBuf = strBuf + Convert.ToString(alarm.ResetTime) + " ,";
                        strBuf = strBuf + Convert.ToString(alarm.Info.Description[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language]).Replace(",", ".") + " ,";
                        strBuf = strBuf + Convert.ToString(alarm.ProcessID) + " ,";

                        sw.WriteLine(strBuf);
                    }
                }

                sw.Close();
                sw.Dispose();
            }
            catch (Exception)
            {
            }
        }
      
        private void GridEvent_CellDoubleClick(object sender, GridCellClickEventArgs e)
        {
            if(e.RowIndex == 0 || e.ColIndex == 0 || LogType != ELogDisplayType.ALARM)
            {
                return;
            }

            int nCode = Convert.ToInt16(GridCont[e.RowIndex, 3].Text);
            int nPID = Convert.ToInt16(GridCont[e.RowIndex, 8].Text);

            CAlarm alarm = CMainFrame.LWDicer.GetAlarmInfo(nCode, nPID, false);
            var dlg = new FormAlarmDisplay(alarm);
            dlg.ShowDialog();
        }

        private void BtnPageTop_Click(object sender, EventArgs e)
        {
            nPageNo = 0;
            DisplayGrid();
        }

        private void BtnPageUp_Click(object sender, EventArgs e)
        {
            nPageNo--;

            if (nPageNo < 0)
            {
                nPageNo = 0;
                return;
            }

            DisplayGrid();
        }

        private void BtnPageDown_Click(object sender, EventArgs e)
        {
            if (datatable == null) return;
            int nTotalPage = datatable.Rows.Count / ResultRowCount;
            if (datatable.Rows.Count % ResultRowCount > 0) nTotalPage++;

            if(nPageNo + 1 >= nTotalPage)
            {
                return;
            }

            nPageNo++;
            DisplayGrid();
        }

        private void BtnPageBot_Click(object sender, EventArgs e)
        {
            if (datatable == null) return;
            int nTotalPage = datatable.Rows.Count / ResultRowCount;
            if (datatable.Rows.Count % ResultRowCount > 0) nTotalPage++;
            nPageNo = nTotalPage - 1;

            DisplayGrid();
        }

        private void ComboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboType.SelectedIndex == 0) LogType = ELogDisplayType.ALARM;
            if (ComboType.SelectedIndex == 1) LogType = ELogDisplayType.EVENT;
            if (ComboType.SelectedIndex == 2) LogType = ELogDisplayType.LOGIN;
            if (ComboType.SelectedIndex == 3) LogType = ELogDisplayType.DEVELOPER;

            bool bVisible = true;

            BtnPageTop.Visible = bVisible;
            BtnPageUp.Visible = bVisible;
            BtnPageDown.Visible = bVisible;
            BtnPageBot.Visible = bVisible;

            datatable = null;
        }
    }
}
