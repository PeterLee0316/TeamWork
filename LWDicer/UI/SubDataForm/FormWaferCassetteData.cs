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

using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormWaferCassetteData : Form
    {

        public FormWaferCassetteData()
        {
            InitializeComponent();

            InitGrid();

            foreach (CListHeader info in CMainFrame.DataManager.CassetteHeaderList)
            {
                if(info.IsFolder == false)
                {
                    ComboCassette.Items.Add(info.Name);
                }
            }

            ComboCassette.SelectedIndex = ComboCassette.Items.IndexOf(CMainFrame.DataManager.ModelData.CassetteName);

            this.Text = $"Wafer Cassette Data [ Current Model : {CMainFrame.DataManager.ModelData.Name} ]";

        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridCtrl.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCtrl.Properties.RowHeaders = true;
            GridCtrl.Properties.ColHeaders = true;

            nCol = 3;
            nRow = 11;

            // Column,Row 개수
            GridCtrl.ColCount = nCol;
            GridCtrl.RowCount = nRow;

            // Column 가로 크기설정

            GridCtrl.ColWidths.SetSize(0, 170);
            GridCtrl.ColWidths.SetSize(1, 70);
            GridCtrl.ColWidths.SetSize(2, 80);
            GridCtrl.ColWidths.SetSize(3, 650);

            for (i = 0; i < nRow + 1; i++)
            {
                GridCtrl.RowHeights[i] = 35;

            }

            // Text Display
            GridCtrl[0, 0].Text = "Item";
            GridCtrl[0, 1].Text = "Unit";
            GridCtrl[0, 2].Text = "Data";
            GridCtrl[0, 3].Text = "Description";

            ELanguage language;
            language = CMainFrame.DataManager.SystemData.Language;

            int nIndex = 1;

            // Cassette Group 검색
            foreach (CParaInfo info in CMainFrame.DataManager.ParaInfoList)
            {
                if(info.Group == "Cassette")
                {
                    GridCtrl[nIndex, 0].Text = info.DisplayName[(int)language];
                    GridCtrl[nIndex, 1].Text = info.Unit;
                    GridCtrl[nIndex, 3].Text = info.Description[(int)language];

                    nIndex++;
                }
            }

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridCtrl[j, i].Font.Bold = true;

                    GridCtrl[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i != 3)
                    {
                        GridCtrl[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }

                    if (i != 0 && i != 1 && j != 0)
                    {
                        GridCtrl[j, i].TextColor = Color.Black;
                    }
                }
            }

            for(i=0;i<nRow;i++)
            {
                GridCtrl[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                GridCtrl[i + 1, 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            GridCtrl.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCtrl.ResizeColsBehavior = 0;
            GridCtrl.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCtrl.Refresh();
        }

        private void UpdateCassetteData(CWaferCassette data)
        {
            GridCtrl[1, 2].Text = String.Format("{0:0.000}", data.Diameter);
            GridCtrl[2, 2].Text = String.Format("{0:0.000}", data.Slot);
            GridCtrl[3, 2].Text = String.Format("{0:0.000}", data.CassetteSetNo);
            GridCtrl[4, 2].Text = String.Format("{0:0.000}", data.FramePitch);
            GridCtrl[5, 2].Text = String.Format("{0:0.000}", data.CassetteHeight);
            GridCtrl[6, 2].Text = String.Format("{0:0.000}", data.ESZeroPoint);
            GridCtrl[7, 2].Text = String.Format("{0:0.000}", data.UnloadElevatorPos);
            GridCtrl[8, 2].Text = String.Format("{0:0.000}", data.CTZeroPoint);
            GridCtrl[9, 2].Text = String.Format("{0:0.000}", data.STZeroPoint);
            GridCtrl[10, 2].Text = String.Format("{0:0.000}", data.LoadPushPullPos);
            GridCtrl[11, 2].Text = String.Format("{0:0.000}", data.FrameCenterPos);

            for(int i=0;i< GridCtrl.RowCount;i++)
            {
                GridCtrl[i + 1, 2].TextColor = Color.Black;
            }
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save Data?"))
            {
                return;
            }

            CWaferCassette cassetteData = new CWaferCassette();

            cassetteData.Name =             ComboCassette.Text;
            cassetteData.Diameter =         Convert.ToDouble(GridCtrl[1, 2].Text);
            cassetteData.Slot =             Convert.ToInt16(GridCtrl[2, 2].Text);
            cassetteData.CassetteSetNo =    Convert.ToInt16(GridCtrl[3, 2].Text);
            cassetteData.FramePitch =       Convert.ToDouble(GridCtrl[4, 2].Text);
            cassetteData.CassetteHeight =   Convert.ToDouble(GridCtrl[5, 2].Text);
            cassetteData.ESZeroPoint =      Convert.ToDouble(GridCtrl[6, 2].Text);
            cassetteData.UnloadElevatorPos = Convert.ToDouble(GridCtrl[7, 2].Text);
            cassetteData.CTZeroPoint =      Convert.ToDouble(GridCtrl[8, 2].Text);
            cassetteData.STZeroPoint =      Convert.ToDouble(GridCtrl[9, 2].Text);
            cassetteData.LoadPushPullPos =  Convert.ToDouble(GridCtrl[10, 2].Text);
            cassetteData.FrameCenterPos =   Convert.ToDouble(GridCtrl[11, 2].Text);

            CMainFrame.DataManager.SaveModelData(cassetteData);

            CMainFrame.DataManager.LoadCassetteData(ComboCassette.Text);

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

            strCurrent = GridCtrl[nRow, nCol].Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridCtrl[nRow, nCol].Text = strModify;
            GridCtrl[nRow, nCol].TextColor = Color.Blue;
        }

        private void ComboCassette_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboCassette = (ComboBox)sender;

            string strCassette = ComboCassette.Text;

            CMainFrame.DataManager.LoadCassetteData(strCassette);

            CWaferCassette cassetteData = ObjectExtensions.Copy(CMainFrame.DataManager.CassetteData);

            UpdateCassetteData(cassetteData);
        }
    }
}
