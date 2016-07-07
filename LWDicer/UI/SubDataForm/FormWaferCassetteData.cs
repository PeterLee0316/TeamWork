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
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormWaferCassetteData : Form
    {

        public FormWaferCassetteData()
        {
            InitializeComponent();

            InitGrid();

            foreach (CListHeader info in CMainFrame.LWDicer.m_DataManager.CassetteHeaderList)
            {
                if(info.IsFolder == false)
                {
                    ComboCassette.Items.Add(info.Name);
                }
            }

            ComboCassette.SelectedIndex = ComboCassette.Items.IndexOf(CMainFrame.LWDicer.m_DataManager.ModelData.CassetteName);

            this.Text = $"Wafer Cassette Data [ Current Model : {CMainFrame.LWDicer.m_DataManager.ModelData.Name} ]";

        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridWaferframe.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridWaferframe.Properties.RowHeaders = true;
            GridWaferframe.Properties.ColHeaders = true;

            nCol = 3;
            nRow = 11;

            // Column,Row 개수
            GridWaferframe.ColCount = nCol;
            GridWaferframe.RowCount = nRow;

            // Column 가로 크기설정

            GridWaferframe.ColWidths.SetSize(0, 170);
            GridWaferframe.ColWidths.SetSize(1, 70);
            GridWaferframe.ColWidths.SetSize(2, 80);
            GridWaferframe.ColWidths.SetSize(3, 650);

            for (i = 0; i < nRow + 1; i++)
            {
                GridWaferframe.RowHeights[i] = 35;

            }

            // Text Display
            GridWaferframe[0, 0].Text = "Item";
            GridWaferframe[0, 1].Text = "Unit";
            GridWaferframe[0, 2].Text = "Data";
            GridWaferframe[0, 3].Text = "Description";

            ELanguage language;
            language = CMainFrame.LWDicer.m_DataManager.SystemData.Language;

            int nIndex = 1;

            // Cassette Group 검색
            foreach (CParaInfo info in CMainFrame.LWDicer.m_DataManager.ParaInfoList)
            {
                if(info.Group == "Cassette")
                {
                    GridWaferframe[nIndex, 0].Text = info.DisplayName[(int)language];
                    GridWaferframe[nIndex, 1].Text = info.Unit;
                    GridWaferframe[nIndex, 3].Text = info.Description[(int)language];

                    nIndex++;
                }
            }

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridWaferframe[j, i].Font.Bold = true;

                    GridWaferframe[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i != 3)
                    {
                        GridWaferframe[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }

                    if (i != 0 && i != 1 && j != 0)
                    {
                        GridWaferframe[j, i].TextColor = Color.Black;
                    }
                }
            }

            for(i=0;i<nRow;i++)
            {
                GridWaferframe[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                GridWaferframe[i + 1, 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            GridWaferframe.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridWaferframe.ResizeColsBehavior = 0;
            GridWaferframe.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridWaferframe.Refresh();
        }

        private void UpdateCassetteData(CWaferCassette data)
        {
            GridWaferframe[1, 2].Text = Convert.ToString(data.Diameter);
            GridWaferframe[2, 2].Text = Convert.ToString(data.Slot);
            GridWaferframe[3, 2].Text = Convert.ToString(data.CassetteSetNo);
            GridWaferframe[4, 2].Text = Convert.ToString(data.FramePitch);
            GridWaferframe[5, 2].Text = Convert.ToString(data.CassetteHeight);
            GridWaferframe[6, 2].Text = Convert.ToString(data.ESZeroPoint);
            GridWaferframe[7, 2].Text = Convert.ToString(data.UnloadElevatorPos);
            GridWaferframe[8, 2].Text = Convert.ToString(data.CTZeroPoint);
            GridWaferframe[9, 2].Text = Convert.ToString(data.STZeroPoint);
            GridWaferframe[10, 2].Text = Convert.ToString(data.LoadPushPullPos);
            GridWaferframe[11, 2].Text = Convert.ToString(data.FrameCenterPos);

            for(int i=0;i< GridWaferframe.RowCount;i++)
            {
                GridWaferframe[i + 1, 2].TextColor = Color.Black;
            }
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg(33))
            {
                return;
            }

            CWaferCassette cassetteData = new CWaferCassette();

            cassetteData.Name =             ComboCassette.Text;
            cassetteData.Diameter =         Convert.ToDouble(GridWaferframe[1, 2].Text);
            cassetteData.Slot =             Convert.ToInt16(GridWaferframe[2, 2].Text);
            cassetteData.CassetteSetNo =    Convert.ToInt16(GridWaferframe[3, 2].Text);
            cassetteData.FramePitch =       Convert.ToDouble(GridWaferframe[4, 2].Text);
            cassetteData.CassetteHeight =   Convert.ToDouble(GridWaferframe[5, 2].Text);
            cassetteData.ESZeroPoint =      Convert.ToDouble(GridWaferframe[6, 2].Text);
            cassetteData.UnloadElevatorPos = Convert.ToDouble(GridWaferframe[7, 2].Text);
            cassetteData.CTZeroPoint =      Convert.ToDouble(GridWaferframe[8, 2].Text);
            cassetteData.STZeroPoint =      Convert.ToDouble(GridWaferframe[9, 2].Text);
            cassetteData.LoadPushPullPos =  Convert.ToDouble(GridWaferframe[10, 2].Text);
            cassetteData.FrameCenterPos =   Convert.ToDouble(GridWaferframe[11, 2].Text);

            CMainFrame.LWDicer.m_DataManager.SaveModelData(cassetteData);

            CMainFrame.LWDicer.m_DataManager.LoadCassetteData(ComboCassette.Text);

            UpdateCassetteData(cassetteData);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormWaferCassetteData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void GridWaferframe_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol != 2 || nRow == 0)
            {
                return;
            }

            strCurrent = GridWaferframe[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridWaferframe[nRow, nCol].Text = strModify;
            GridWaferframe[nRow, nCol].TextColor = Color.Red;
        }

        private void ComboCassette_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboCassette = (ComboBox)sender;

            string strCassette = ComboCassette.Text;

            CMainFrame.LWDicer.m_DataManager.LoadCassetteData(strCassette);

            CWaferCassette cassetteData = ObjectExtensions.Copy(CMainFrame.LWDicer.m_DataManager.CassetteData);

            UpdateCassetteData(cassetteData);
        }
    }
}
