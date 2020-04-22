using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            

            m_SystemData_Align = ObjectExtensions.Copy(CMainFrame.DataManager.SystemData_Align);
            

        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        private void FormVisionData_FormClosing(object sender, FormClosingEventArgs e)
        {
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


        private void btnCameraDataLoad_Click(object sender, EventArgs e)
        {
            ReloadCameraData();
        }

        private void btnCameraDataSave_Click(object sender, EventArgs e)
        {
            string strData = string.Empty;
            string strMsg = "Save Camera data?";
            if (!CMainFrame.InquireMsg(strMsg)) return;
            
            CMainFrame.DataManager.SaveSystemData(null, null, m_SystemData_Align, null);
            
        }
    }
}
