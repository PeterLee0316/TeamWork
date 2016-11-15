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
    public partial class FormWaferFrameData : Form
    {
        CWaferFrameData m_WaferFrameData = new CWaferFrameData();
       
        public FormWaferFrameData()
        {
            InitializeComponent();

            InitGrid();

            foreach (CListHeader info in CMainFrame.DataManager.WaferFrameHeaderList)
            {
                if(info.IsFolder == false)
                {
                    ComboWaferFrame.Items.Add(info.Name);
                }
            }

            try
            {
                ComboWaferFrame.SelectedIndex = ComboWaferFrame.Items.IndexOf(CMainFrame.DataManager.ModelData.WaferFrameName);
            }
            catch (System.Exception ex)
            {
                ComboWaferFrame.SelectedIndex = -1;
            }

            this.Text = $"Wafer Frame Data [ Current Model : {CMainFrame.DataManager.ModelData.WaferFrameName} ]";

        }

        private void InitGridColumn(int index, CParaInfo info)
        {
            ELanguage language = CMainFrame.DataManager.SystemData.Language;
            GridCtrl[index, 0].Text = info.DisplayName[(int)language];
            GridCtrl[index, 1].Text = info.Unit;
            GridCtrl[index, 3].Text = info.Description[(int)language];
        }

        private void InitGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridCtrl.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCtrl.Properties.RowHeaders = true;
            GridCtrl.Properties.ColHeaders = true;

            int nCol = 3;
            int nRow = 13;

            // Column,Row 개수
            GridCtrl.ColCount = nCol;
            GridCtrl.RowCount = nRow;

            // Column 가로 크기설정

            GridCtrl.ColWidths.SetSize(0, 170);
            GridCtrl.ColWidths.SetSize(1, 70);
            GridCtrl.ColWidths.SetSize(2, 80);
            GridCtrl.ColWidths.SetSize(3, 650);

            for (int i = 0; i < nRow + 1; i++)
            {
                GridCtrl.RowHeights[i] = 35;

            }

            // Text Display
            GridCtrl[0, 0].Text = "Item";
            GridCtrl[0, 1].Text = "Unit";
            GridCtrl[0, 2].Text = "Data";
            GridCtrl[0, 3].Text = "Description";

            ELanguage language = CMainFrame.DataManager.SystemData.Language;

            int index = 1;

            // Cassette Group 검색
            CParaInfo info;
            string group = "WaferFrame";
            int iResult = CMainFrame.DataManager.LoadParaInfo(group, "Frame Diameter", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "Slot", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "Cassette Set No", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "Frame Pitch", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "Cassette Height", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "E/S-0 Point", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "C/T-0 Point", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "S/T-0 Point", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "Coater fream centering", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "Stage Pos", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "Elevator Pos at Unload", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "PushPull Pos at Load", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);
            CMainFrame.DataManager.LoadParaInfo(group, "PushPull Pos at Unload", out info);
            if (iResult != SUCCESS) CMainFrame.DisplayAlarmOnly(iResult);
            InitGridColumn(index++, info);

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
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

            for (int i = 0; i < nRow; i++)
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

        private void UpdateData()
        {
            GridCtrl[1, 2].Text  = String.Format("{0:0.000}", m_WaferFrameData.Diameter);
            GridCtrl[2, 2].Text  = String.Format("{0:0.000}", m_WaferFrameData.SlotNumber);
            GridCtrl[3, 2].Text  = String.Format("{0:0.000}", m_WaferFrameData.CassetteSetNo);
            GridCtrl[4, 2].Text  = String.Format("{0:0.000}", m_WaferFrameData.FramePitch);
            GridCtrl[5, 2].Text  = String.Format("{0:0.000}", m_WaferFrameData.CassetteHeight);
            GridCtrl[6, 2].Text  = String.Format("{0:0.000}", m_WaferFrameData.ESZeroPoint);
            GridCtrl[7, 2].Text  = String.Format("{0:0.000}", m_WaferFrameData.CTZeroPoint);
            GridCtrl[8, 2].Text  = String.Format("{0:0.000}", m_WaferFrameData.STZeroPoint);
            GridCtrl[9, 2].Text = String.Format("{0:0.000}", m_WaferFrameData.FrameCenterPos);
            GridCtrl[10, 2].Text = String.Format("{0:0.000}", m_WaferFrameData.StagePos);
            GridCtrl[11, 2].Text  = String.Format("{0:0.000}", m_WaferFrameData.ElevatorPos_atUnload);
            GridCtrl[12, 2].Text = String.Format("{0:0.000}", m_WaferFrameData.PushPullPos_atLoad);
            GridCtrl[13, 2].Text = String.Format("{0:0.000}", m_WaferFrameData.PushPullPos_atUnload);

            for (int i = 0; i < GridCtrl.RowCount; i++)
            {
                GridCtrl[i + 1, 2].TextColor = Color.Black;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save data?"))
            {
                return;
            }

            m_WaferFrameData.Name                   = ComboWaferFrame.Text;
            m_WaferFrameData.Diameter               = Convert.ToDouble(GridCtrl[1, 2].Text);
            m_WaferFrameData.SlotNumber             = (int)(Convert.ToDouble(GridCtrl[2, 2].Text));
            m_WaferFrameData.CassetteSetNo          = (int)(Convert.ToDouble(GridCtrl[3, 2].Text));
            m_WaferFrameData.FramePitch             = Convert.ToDouble(GridCtrl[4, 2].Text);
            m_WaferFrameData.CassetteHeight         = Convert.ToDouble(GridCtrl[5, 2].Text);
            m_WaferFrameData.ESZeroPoint            = Convert.ToDouble(GridCtrl[6, 2].Text);
            m_WaferFrameData.CTZeroPoint            = Convert.ToDouble(GridCtrl[7, 2].Text);
            m_WaferFrameData.STZeroPoint            = Convert.ToDouble(GridCtrl[8, 2].Text);
            m_WaferFrameData.FrameCenterPos         = Convert.ToDouble(GridCtrl[9, 2].Text);
            m_WaferFrameData.StagePos               = Convert.ToDouble(GridCtrl[10, 2].Text);
            m_WaferFrameData.ElevatorPos_atUnload   = Convert.ToDouble(GridCtrl[11, 2].Text);
            m_WaferFrameData.PushPullPos_atLoad     = Convert.ToDouble(GridCtrl[12, 2].Text);
            m_WaferFrameData.PushPullPos_atUnload   = Convert.ToDouble(GridCtrl[13, 2].Text);

            CMainFrame.LWDicer.SaveModelData(m_WaferFrameData);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormWaferCassetteData_FormClosing(object sender, FormClosingEventArgs e)
        {
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

        private void ComboWaferFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = ((ComboBox)sender).Text;
            int iResult = CMainFrame.DataManager.LoadWaferFrameData(str);
            if (iResult != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult);
                return;
            }

            m_WaferFrameData = ObjectExtensions.Copy(CMainFrame.DataManager.WaferFrameData);
            UpdateData();
        }
    }
}
