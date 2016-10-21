using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_Common;

namespace LWDicer.UI
{
    public partial class FormVisionData : Form
    {

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
            this.DesktopLocation = new Point(1, 100);
#if !SIMULATION_VISION
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);
#endif

        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
#if !SIMULATION_VISION
            iCurrentView = PRE__CAM;

            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(iCurrentView, PATTERN_A, picPatternMarkA.Handle);
            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(iCurrentView, PATTERN_B, picPatternMarkB.Handle);
#endif
        }

        private void FormVisionData_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void btnChangeCam_Click(object sender, EventArgs e)
        {
 #if !SIMULATION_VISION
            if (iCurrentView == PRE__CAM)
            {
                CMainFrame.LWDicer.m_Vision.DestroyLocalView(PRE__CAM);
                CMainFrame.LWDicer.m_Vision.InitialLocalView(FINE_CAM, picVision.Handle);

                iCurrentView = FINE_CAM;
            }
            else
            {
                CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
                CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);

                iCurrentView = PRE__CAM;
            }

            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(iCurrentView, PATTERN_A, picPatternMarkA.Handle);
            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(iCurrentView, PATTERN_B, picPatternMarkB.Handle);
#endif
        }

        private void btnShowHairLine_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.ClearOverlay();
            CMainFrame.LWDicer.m_Vision.DrawOverLayHairLine(iCurrentView, iHairLineWidth);
            iCurrentMode = 1;

        }

        private void btnShowMarkLine_Click(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            CMainFrame.LWDicer.m_Vision.ClearOverlay();
            PatternRec =  CMainFrame.LWDicer.m_Vision.GetOverlayAreaRect(iCurrentView);
            CMainFrame.LWDicer.m_Vision.DrawOverlayAreaRect(iCurrentView, PatternRec);
            iCurrentMode = 2;
#endif
        }

        private void btnSelectAxis_Click(object sender, EventArgs e)
        {
            if((string)btnSelectAxis.Tag =="Height" )
            {
                btnSelectAxis.Text = "Select Height";
                btnSelectAxis.Tag = "Width";
            }
            else
            {
                btnSelectAxis.Text = "Select Width";
                btnSelectAxis.Tag = "Height";
            }
        }

        private void btnSizeWide_Click(object sender, EventArgs e)
        {
 #if !SIMULATION_VISION
            if (iCurrentMode == 1)
                CMainFrame.LWDicer.m_Vision.WidenHairLine();
            else
            {
                if ((string)btnSelectAxis.Tag == "Height")
                {
                    CMainFrame.LWDicer.m_Vision.WidenRoiHeight();
                }
                else
                {
                    CMainFrame.LWDicer.m_Vision.WidenRoiWidth();
                }
            }
#endif
        }

        private void btnSizeNarrow_Click(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            if (iCurrentMode == 1)
                CMainFrame.LWDicer.m_Vision.NarrowHairLine();
            else
            {
                if ((string)btnSelectAxis.Tag == "Height")
                {
                    CMainFrame.LWDicer.m_Vision.NarrowRoiHeight();
                }
                else
                {
                    CMainFrame.LWDicer.m_Vision.NarrowRoiWidth();
                }
            }
#endif
        }

        private void btnRegisterMarkA_Click(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            int iResult = 0;

            var sizeRoi = new Size(0,0);
            string strName = CMainFrame.LWDicer.m_DataManager.ModelData.Name;

            sizeRoi = CMainFrame.LWDicer.m_Vision.GetCameraPixelSize(iCurrentView);    
            var searchSize = new Rectangle(0, 0, sizeRoi.Width, sizeRoi.Height);
            sizeRoi = CMainFrame.LWDicer.m_Vision.GetOverlayAreaRect(iCurrentView);
            var modelSize = new Rectangle(0, 0, sizeRoi.Width, sizeRoi.Height);

            iResult = CMainFrame.LWDicer.m_Vision.RegisterPatternMark(iCurrentView, strName, PATTERN_A, searchSize, modelSize);

            if (iResult != SUCCESS) return;

            strName = strName + $"_Cam_{iCurrentView}_Type_{PATTERN_A}.bmp";

            if(iCurrentView == PRE__CAM)
                CMainFrame.LWDicer.m_DataManager.ModelData.MacroPatternA.m_strFileName = strName;
            if (iCurrentView == FINE_CAM)
                CMainFrame.LWDicer.m_DataManager.ModelData.MicroPatternA.m_strFileName = strName;

            CMainFrame.LWDicer.m_DataManager.SaveModelData(CMainFrame.LWDicer.m_DataManager.ModelData);


            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(iCurrentView, PATTERN_A, picPatternMarkA.Handle);
#endif
        }

        private void btnRegisterMarkB_Click(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            int iResult = 0;

            var sizeRoi = new Size(0, 0);
            string strName = CMainFrame.LWDicer.m_DataManager.ModelData.Name;

            sizeRoi = CMainFrame.LWDicer.m_Vision.GetCameraPixelSize(iCurrentView);
            var searchSize = new Rectangle(0, 0, sizeRoi.Width, sizeRoi.Height);
            sizeRoi = CMainFrame.LWDicer.m_Vision.GetOverlayAreaRect(iCurrentView);
            var modelSize = new Rectangle(0, 0, sizeRoi.Width, sizeRoi.Height);
            CMainFrame.LWDicer.m_Vision.RegisterPatternMark(iCurrentView, strName, PATTERN_B, searchSize, modelSize);

            if (iResult != SUCCESS) return;

            strName = strName + $"_Cam_{iCurrentView}_Type_{PATTERN_A}.bmp";

            if (iCurrentView == PRE__CAM)
                CMainFrame.LWDicer.m_DataManager.ModelData.MacroPatternB.m_strFileName = strName;
            if (iCurrentView == FINE_CAM)
                CMainFrame.LWDicer.m_DataManager.ModelData.MicroPatternB.m_strFileName = strName;

            CMainFrame.LWDicer.m_DataManager.SaveModelData(CMainFrame.LWDicer.m_DataManager.ModelData);

            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(iCurrentView, PATTERN_B, picPatternMarkB.Handle);
#endif
        }

        private void btnSearchMarkA_Click(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            var searchData = new CResultData();
            CMainFrame.LWDicer.m_Vision.RecognitionPatternMark(iCurrentView, PATTERN_A, out searchData);
#endif
        }

        private void btnSearchMarkB_Click(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            var searchData = new CResultData();
            CMainFrame.LWDicer.m_Vision.RecognitionPatternMark(iCurrentView, PATTERN_B, out searchData);
#endif
        }

        
    }
}
