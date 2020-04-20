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

using static Core.Layers.DEF_Vision;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_System;

namespace Core.UI
{
    public partial class FormVisionData : Form
    {
        private CSystemData_Align m_SystemData_Align;

        int iCurrentView = 0;
        int iCurrentMode = 0;
        int iHairLineWidth = 100;
        Size PatternRec = new Size(100, 100);

        public FormVisionData()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormVisionData_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(10, 128);

            InitGrid();

            m_SystemData_Align = ObjectExtensions.Copy(CMainFrame.DataManager.SystemData_Align);

            UpdateCameraData();

        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        private void FormVisionData_FormClosing(object sender, FormClosingEventArgs e)
        {
        }


        private void InitGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridCtrl.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCtrl.Properties.RowHeaders = true;
            GridCtrl.Properties.ColHeaders = true;

            int nCol = (int)ECameraSelect.MAX;
            int nRow = 8;

            // Column,Row 개수
            GridCtrl.ColCount = nCol;
            GridCtrl.RowCount = nRow;

            // Column 가로 크기설정
            for (int i = 0; i < nCol + 1; i++)
            {
                GridCtrl.ColWidths.SetSize(i, 160);
            }

            GridCtrl.ColWidths.SetSize(0, 200);

            for (int i = 0; i < nRow + 1; i++)
            {
                GridCtrl.RowHeights[i] = 36;

            }

            // Text Display
            GridCtrl[0, 0].Text = "항목";
            GridCtrl[0, 1].Text = "PRE CAM";
            GridCtrl[0, 2].Text = "FINE CAM";
            GridCtrl[0, 3].Text = "INSPECT CAM";

            GridCtrl[1, 0].Text = "렌즈 배율 (X)";
            GridCtrl[2, 0].Text = "카메라 Pixel Size (μm)";
            GridCtrl[3, 0].Text = "카메라 Pixel Num (가로)";
            GridCtrl[4, 0].Text = "카메라 Pixel Num (세로)";
            GridCtrl[5, 0].Text = "카메라 Resolution (μm)";
            GridCtrl[6, 0].Text = "카메라 FOV_H (mm)";
            GridCtrl[7, 0].Text = "카메라 FOV_V (mm)";
            GridCtrl[8, 0].Text = "카메라 설치 회전 오차(˚)";


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

            for (int i = 1; i < nCol + 1; i++)
            {
                for (int j = 3; j < nRow + 1; j++)
                {
                    GridCtrl[j, i].BackColor = Color.FromArgb(220, 220, 255);
                }
            }

            GridCtrl.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCtrl.ResizeColsBehavior = 0;
            GridCtrl.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCtrl.Refresh();
        }


        private void UpdateCameraData()
        {
            for(int i=0; i< (int)ECameraSelect.MAX; i++)
            {
                GridCtrl[1, i + 1].Text = String.Format( "{0:f4}", m_SystemData_Align.LenMagnification[i]);
                GridCtrl[2, i + 1].Text = String.Format( "{0:f4}", m_SystemData_Align.CamPixelSize[i]);
                GridCtrl[3, i + 1].Text = String.Format( "{0:0}",  m_SystemData_Align.CamPixelNumX[i]);
                GridCtrl[4, i + 1].Text = String.Format( "{0:0}",  m_SystemData_Align.CamPixelNumY[i]);
                GridCtrl[5, i + 1].Text = String.Format( "{0:f4}", m_SystemData_Align.PixelResolution[i]);
                GridCtrl[6, i + 1].Text = String.Format( "{0:f4}", m_SystemData_Align.CamFovX[i]);
                GridCtrl[7, i + 1].Text = String.Format( "{0:f4}", m_SystemData_Align.CamFovY[i]);
                GridCtrl[8, i + 1].Text = String.Format( "{0:f4}", m_SystemData_Align.CameraTilt[i]);
            }
        }

        private void ReloadCameraData()
        {
            for (int i = 0; i < (int)ECameraSelect.MAX; i++)
            {
                // 렌즈 배율
                if (m_SystemData_Align.LenMagnification[i] == 0.0)
                {
                    if (i == PRE__CAM) m_SystemData_Align.LenMagnification[i] = 0.75;
                    if (i == FINE_CAM) m_SystemData_Align.LenMagnification[i] = 7.5;
                    if (i == INSP_CAM) m_SystemData_Align.LenMagnification[i] = 20.0;
                }

                // Cam Pixel Size
                if (m_SystemData_Align.CamPixelSize[i] == 0.0)  m_SystemData_Align.CamPixelSize[i] = 3.75;
                // Cam Pixel Num
                m_SystemData_Align.CamPixelNumX[i] = CMainFrame.mCore.m_Vision.GetCameraPixelNum(i).Width;
                m_SystemData_Align.CamPixelNumY[i] = CMainFrame.mCore.m_Vision.GetCameraPixelNum(i).Height;
                // Cam Pixel Resolution
                m_SystemData_Align.PixelResolution[i] = m_SystemData_Align.CamPixelSize[i] /
                                                                              m_SystemData_Align.LenMagnification[i];
                // Cam FOV
                m_SystemData_Align.CamFovX[i] = m_SystemData_Align.PixelResolution[i] *
                                                                     m_SystemData_Align.CamPixelNumX[i] / 1000;
                m_SystemData_Align.CamFovY[i] = m_SystemData_Align.PixelResolution[i] *
                                                                     m_SystemData_Align.CamPixelNumY[i] /1000;
                // Cam 설치 회전 오차
                m_SystemData_Align.CameraTilt[i] = 0.0;
            }
        }

        private void GridCtrl_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol == 0 || nRow == 0) return;
            if (nRow != 1 && nRow != 2) return;
            

            strCurrent = GridCtrl[nRow, nCol].Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridCtrl[nRow, nCol].Text = strModify;
            GridCtrl[nRow, nCol].TextColor = Color.Blue;
        }

        private void btnCameraDataLoad_Click(object sender, EventArgs e)
        {
            ReloadCameraData();
            UpdateCameraData();
        }

        private void btnCameraDataSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save Camera data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;

            for (int i = 0; i < (int)ECameraSelect.MAX; i++)
            {
                m_SystemData_Align.LenMagnification[i] = Convert.ToDouble(GridCtrl[1, i + 1].Text);
                m_SystemData_Align.CamPixelSize[i]     = Convert.ToDouble(GridCtrl[2, i + 1].Text);
                m_SystemData_Align.CamPixelNumX[i]     = Convert.ToInt32(GridCtrl[3, i + 1].Text);
                m_SystemData_Align.CamPixelNumY[i]     = Convert.ToInt32(GridCtrl[4, i + 1].Text);
                m_SystemData_Align.PixelResolution[i]  = Convert.ToDouble(GridCtrl[5, i + 1].Text);
                m_SystemData_Align.CamFovX[i]          = Convert.ToDouble(GridCtrl[6, i + 1].Text);
                m_SystemData_Align.CamFovY[i]          = Convert.ToDouble(GridCtrl[7, i + 1].Text);
                m_SystemData_Align.CameraTilt[i]       = Convert.ToDouble(GridCtrl[8, i + 1].Text);
            }

            CMainFrame.DataManager.SaveSystemData(null, null, m_SystemData_Align, null);
            
        }
    }
}
