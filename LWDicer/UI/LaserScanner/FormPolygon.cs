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

using static LWDicer.Control.DEF_Scanner;
using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_DataManager;

using LWDicer.Control;

namespace LWDicer.UI
{
    public partial class FormPolygon : Form
    {
        private Size OriginFormSize = new Size(0, 0);
        private Point ptMouseStartPos = new Point(0, 0);
        private Point ptMouseEndPos = new Point(0, 0);

        private EVisionMode eVisionMode;

        public FormPolygon()
        {
            InitializeComponent();

            InitTabPage();
            InitConfigureGrid();
            InitTrueRasterGrid();

            DisplayProcessData();            

            string[] comboBoxData = { "0", "2", "4", "8" };
            cbbOverlap.Items.AddRange(comboBoxData);
            cbbOverlap.ItemHeight = 31;
            
            OriginFormSize.Width = this.Width;
            OriginFormSize.Height = this.Height;

            eVisionMode = EVisionMode.MEASUREMENT;           

        }

        private void FormPolygon_Load(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            CMainFrame.LWDicer.m_Vision.InitialLocalView(ZOOM_CAM, picVisionZoom.Handle);
#endif
        }

        private void Form_Resize(object sender, EventArgs e)
        {

            return;

            //SizeF formScale = new SizeF(0, 0);
            //Size formSize = new Size(0, 0);
            //Point compPos = new Point(0, 0);
            //float fontSize = 0;

            //// 해결 했다.

            //formSize.Width = this.Width;
            //formSize.Height = this.Height;

            //formScale.Width = (float)formSize.Width / (float)OriginFormSize.Width;
            //formScale.Height = (float)formSize.Height / (float)OriginFormSize.Height;

            //var component = GetAllControl(this);

            //foreach (Control each in component)
            //{
            //    each.Width = (int)((float)each.Width * formScale.Width + 0.5f);
            //    each.Height = (int)((float)each.Height * formScale.Height + 0.5f);

            //    compPos.X = (int)((float)each.Location.X * formScale.Width + 0.5f);
            //    compPos.Y = (int)((float)each.Location.Y * formScale.Height + 0.5f);
            //    each.Location = compPos;

            //    if (each is Panel || each is UserControl)
            //    {

            //    }
            //    else
            //    {
            //        fontSize = (each.Font.Size * formScale.Height);
            //        each.Font = new System.Drawing.Font(each.Font.Name, fontSize);
            //    }
            //}

            //OriginFormSize.Width = this.Width;
            //OriginFormSize.Height = this.Height;
            
        }

        //public IEnumerable<Control> GetAllControl(Control control, Type type = null)
        //{
        //    var controls = control.Controls.Cast<Control>();

        //    //check the all value, if true then get all the controls
        //    //otherwise get the controls of the specified type
        //    if (type == null)
        //        return controls.SelectMany(ctrl => GetAllControl(ctrl, type)).Concat(controls);
        //    else
        //        return controls.SelectMany(ctrl => GetAllControl(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        //}

        private void InitTabPage()
        {
            this.tabPolygonForm.TabPages.Clear();

            this.tabPolygonForm.TabPages.AddRange(new TabPageAdv[]
                     {
                        this.tabPageProcess,
                        this.tabPageProcess266,
                        this.tabPageScanner,
                        this.tabPageLaser,
                        //this.tabPageMotion,
                        this.tabPageVision,
                        this.tabPagePara,
                        this.tabPageConfig,
                        this.tabPageTrueRaster

                     });
        }
        // Polygon Scanner Configure Data
        private void InitConfigureGrid()
        {
            int i = 0, j = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridConfigure.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridConfigure.Properties.RowHeaders = true;
            GridConfigure.Properties.ColHeaders = true;

            // Column,Row 개수
            GridConfigure.ColCount = 3;
            GridConfigure.RowCount = 27;

            // Column 가로 크기설정
            GridConfigure.ColWidths.SetSize(0, 200);
            GridConfigure.ColWidths.SetSize(1, 80);
            GridConfigure.ColWidths.SetSize(2, 100);
            GridConfigure.ColWidths.SetSize(3, 650);

            for (i = 0; i < GridConfigure.RowCount+1; i++)
            {
                GridConfigure.RowHeights[i] = 27;
            }

            for (i = 1; i < GridConfigure.RowCount+1; i++)
            {
                GridConfigure[i , 1].BackColor = Color.FromArgb(230, 210, 255);
                GridConfigure[i , 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            // Text Display
            GridConfigure[0, 0].Text = "Parameter";
            GridConfigure[0, 1].Text = "Unit";
            GridConfigure[0, 2].Text = "Data";
            GridConfigure[0, 3].Text = "Description";

            GridConfigure[1, 0].Text = "InScanResolution";
            GridConfigure[2, 0].Text = "CrossScanResolution";
            GridConfigure[3, 0].Text = "InScanOffset";
            GridConfigure[4, 0].Text = "StopMotorBetweenJobs";
            GridConfigure[5, 0].Text = "PixInvert";
            GridConfigure[6, 0].Text = "JobStartBufferTime";
            GridConfigure[7, 0].Text = "PrecedingBlankLines";
            GridConfigure[8, 0].Text = "SeedClockFrequency";
            GridConfigure[9, 0].Text = "RepetitionRate";
            GridConfigure[10, 0].Text = "PulsePickWidth";
            GridConfigure[11, 0].Text = "PixelWidth";
            GridConfigure[12, 0].Text = "CrossScanEncoderResol";
            GridConfigure[13, 0].Text = "CrossScanMaxAccel";
            GridConfigure[14, 0].Text = "EnCarSig";
            GridConfigure[15, 0].Text = "SwapCarSig";
            GridConfigure[16, 0].Text = "InterLeaveRatio";
            GridConfigure[17, 0].Text = "FacetFineDelayOffset0";
            GridConfigure[18, 0].Text = "FacetFineDelayOffset1";
            GridConfigure[19, 0].Text = "FacetFineDelayOffset2";
            GridConfigure[20, 0].Text = "FacetFineDelayOffset3";
            GridConfigure[21, 0].Text = "FacetFineDelayOffset4";
            GridConfigure[22, 0].Text = "FacetFineDelayOffset5";
            GridConfigure[23, 0].Text = "FacetFineDelayOffset6";
            GridConfigure[24, 0].Text = "FacetFineDelayOffset7";
            GridConfigure[25, 0].Text = "StartFacet";
            GridConfigure[26, 0].Text = "AutoIncrementStartFacet";
            GridConfigure[27, 0].Text = "MotorStableTime";

            GridConfigure[1, 1].Text = "[mm]";
            GridConfigure[2, 1].Text = "[mm]";
            GridConfigure[3, 1].Text = "[mm]";
            GridConfigure[4, 1].Text = "[-]";
            GridConfigure[5, 1].Text = "[-]";
            GridConfigure[6, 1].Text = "[sec]";
            GridConfigure[7, 1].Text = "[-]";
            GridConfigure[8, 1].Text = "[kHz]";
            GridConfigure[9, 1].Text = "[kHz]";
            GridConfigure[10, 1].Text = "[-]";
            GridConfigure[11, 1].Text = "[-]";
            GridConfigure[12, 1].Text = "[mm]";
            GridConfigure[13, 1].Text = "[m/s^2]";
            GridConfigure[14, 1].Text = "[-]";
            GridConfigure[15, 1].Text = "[-]";
            GridConfigure[16, 1].Text = "[-]";
            GridConfigure[17, 1].Text = "[mm]";
            GridConfigure[18, 1].Text = "[mm]";
            GridConfigure[19, 1].Text = "[mm]";
            GridConfigure[20, 1].Text = "[mm]";
            GridConfigure[21, 1].Text = "[mm]";
            GridConfigure[22, 1].Text = "[mm]";
            GridConfigure[23, 1].Text = "[mm]";
            GridConfigure[24, 1].Text = "[mm]";
            GridConfigure[25, 1].Text = "[-]";
            GridConfigure[26, 1].Text = "[-]";
            GridConfigure[27, 1].Text = "[ms]";

            GridConfigure[1, 3].Text = "[Job Settings] Scanline에서 이웃한 두 pixel 사이 X축 거리";
            GridConfigure[2, 3].Text = "[Job Settings] 이웃한 두 Scanline 사이 Y축 거리";
            GridConfigure[3, 3].Text = "[Job Settings] 모든 Scanline의 X축 시작 위치를 조정";
            GridConfigure[4, 3].Text = "[Job Settings] Exposure 이후 polygonmirror 정지 여부 결정";
            GridConfigure[5, 3].Text = "[Job Settings] LaserPulse Exposure 되는 Bitmap의 색상 선택";
            GridConfigure[6, 3].Text = "[Job Settings] Bitmap Uploading 시, exposure 하기전 대기 시간";
            GridConfigure[7, 3].Text = "[Job Settings] Stage 가속시의 충분한 Settle-time을 위한 Dummy scanline 수";
            GridConfigure[8, 3].Text = "[Laser Configuration] Laser의 Seed Clock 주파수 설정";
            GridConfigure[9, 3].Text = "[Laser Configuration] 가공에 적용할 펄스 반복률(REP_RATE) 설정";
            GridConfigure[10, 3].Text = "[Laser Configuration] Pulse의  폭을 설정한다.";
            GridConfigure[11, 3].Text = "[Laser Configuration] Pixel의 폭을 설정한다.";
            GridConfigure[12, 3].Text = "[CrossScan Configuration] StageEncoder 분해능 값 설정";
            GridConfigure[13, 3].Text = "[CrossScan Configuration] Stagestart-up 과정의 최대 가속도";
            GridConfigure[14, 3].Text = "[CrossScan Configuration] Stage Control Encoder signal 출력 여부";
            GridConfigure[15, 3].Text = "[CrossScan Configuration] Stage movement direction 선택";
            GridConfigure[16, 3].Text = "[Head Configuration] FacetFineDelayOffset 자동 설정 기능";
            GridConfigure[17, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[18, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[19, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[20, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[21, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[22, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[23, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[24, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[25, 3].Text = "[Head Configuration] exposure의 첫 scanline에 해당하는 facet 지정";
            GridConfigure[26, 3].Text = "[Head Configuration] 새로운job started 마다 StartFacet 값 증가";
            GridConfigure[27, 3].Text = "[Polygon motor Configuration] speed-up 이후 exposure 시작 이전 대기시간";


            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < GridConfigure.RowCount+1; j++)
                {
                    // Font Style - Bold
                    GridConfigure[j, i].Font.Bold = true;

                    GridConfigure[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i != 3)
                    {
                        GridConfigure[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }
                }
            }

            GridConfigure.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridConfigure.ResizeColsBehavior = 0;
            GridConfigure.ResizeRowsBehavior = 0;

            //GetPolygonPara(scannerIndex);

            // Grid Display Update
            GridConfigure.Refresh();
        }

        private void InitTrueRasterGrid()
        {

            int i = 0, j = 0;
            //===============================================================================
            // CSN Grid 설정

            // Cell Click 시 커서가 생성되지 않게함.
            GridCSN.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCSN.Properties.RowHeaders = true;
            GridCSN.Properties.ColHeaders = true;

            // Column,Row 개수
            GridCSN.ColCount = 3;
            GridCSN.RowCount = 16;

            // Column 가로 크기설정
            GridCSN.ColWidths.SetSize(0, 120);
            GridCSN.ColWidths.SetSize(1, 60);
            GridCSN.ColWidths.SetSize(2, 80);
            GridCSN.ColWidths.SetSize(3, 250);

            for (i = 0; i < 17; i++)
            {
                GridCSN.RowHeights[i] = 27;
            }

            for (i = 0; i < 16; i++)
            {
                GridCSN[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                GridCSN[i + 1, 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            // Text Display
            GridCSN[0, 0].Text = "Parameter";
            GridCSN[0, 1].Text = "Unit";
            GridCSN[0, 2].Text = "Data";
            GridCSN[0, 3].Text = "Description";

            GridCSN[1, 0].Text    = "Facet 1 Start";
            GridCSN[2, 0].Text    = "Facet 1 End";
            GridCSN[3, 0].Text    = "Facet 2 Start";
            GridCSN[4, 0].Text    = "Facet 2 End";
            GridCSN[5, 0].Text    = "Facet 3 Start";
            GridCSN[6, 0].Text    = "Facet 3 End";
            GridCSN[7, 0].Text    = "Facet 4 Start";
            GridCSN[8, 0].Text    = "Facet 4 End";
            GridCSN[9, 0].Text    = "Facet 5 Start";
            GridCSN[10, 0].Text   = "Facet 5 End";
            GridCSN[11, 0].Text   = "Facet 6 Start";
            GridCSN[12, 0].Text   = "Facet 6 End";
            GridCSN[13, 0].Text   = "Facet 7 Start";
            GridCSN[14, 0].Text   = "Facet 7 End";
            GridCSN[15, 0].Text   = "Facet 8 Start";
            GridCSN[16, 0].Text   = "Facet 8 End";

            GridCSN[1, 1].Text    = "[um]";
            GridCSN[2, 1].Text    = "[um]";
            GridCSN[3, 1].Text    = "[um]";
            GridCSN[4, 1].Text    = "[um]";
            GridCSN[5, 1].Text    = "[um]";
            GridCSN[6, 1].Text    = "[um]";
            GridCSN[7, 1].Text    = "[um]";
            GridCSN[8, 1].Text    = "[um]";
            GridCSN[9, 1].Text    = "[um]";
            GridCSN[10, 1].Text   = "[um]";
            GridCSN[11, 1].Text   = "[um]";
            GridCSN[12, 1].Text   = "[um]";
            GridCSN[13, 1].Text   = "[um]";
            GridCSN[14, 1].Text   = "[um]";
            GridCSN[15, 1].Text   = "[um]";
            GridCSN[16, 1].Text   = "[um]";

            GridCSN[1, 3].Text    = "Facet 1 의 Start 세로 위치(Y 방향) 보정";
            GridCSN[2, 3].Text    = "Facet 1 의 End 세로 위치(Y 방향) 보정";
            GridCSN[3, 3].Text    = "Facet 2 의 Start 세로 위치(Y 방향) 보정";
            GridCSN[4, 3].Text    = "Facet 2 의 End 세로 위치(Y 방향) 보정";
            GridCSN[5, 3].Text    = "Facet 3 의 Start 세로 위치(Y 방향) 보정";
            GridCSN[6, 3].Text    = "Facet 3 의 End 세로 위치(Y 방향) 보정";
            GridCSN[7, 3].Text    = "Facet 4 의 Start 세로 위치(Y 방향) 보정";
            GridCSN[8, 3].Text    = "Facet 4 의 End 세로 위치(Y 방향) 보정";
            GridCSN[9, 3].Text    = "Facet 5 의 Start 세로 위치(Y 방향) 보정";
            GridCSN[10, 3].Text   = "Facet 5 의 End 세로 위치(Y 방향) 보정";
            GridCSN[11, 3].Text   = "Facet 6 의 Start 세로 위치(Y 방향) 보정";
            GridCSN[12, 3].Text   = "Facet 6 의 End 세로 위치(Y 방향) 보정";
            GridCSN[13, 3].Text   = "Facet 7 의 Start 세로 위치(Y 방향) 보정";
            GridCSN[14, 3].Text   = "Facet 7 의 End 세로 위치(Y 방향) 보정";
            GridCSN[15, 3].Text   = "Facet 8 의 Start 세로 위치(Y 방향) 보정";
            GridCSN[16, 3].Text   = "Facet 8 의 End 세로 위치(Y 방향) 보정";

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 17; j++)
                {
                    // Font Style - Bold
                    GridCSN[j, i].Font.Bold = true;

                    GridCSN[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i != 3)
                    {
                        GridCSN[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }
                }
            }

            GridCSN.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCSN.ResizeColsBehavior = 0;
            GridCSN.ResizeRowsBehavior = 0;

            //GetPolygonPara(scannerIndex);

            // Grid Display Update
            GridCSN.Refresh();


            //===============================================================================
            // ISN Grid 설정

            // Cell Click 시 커서가 생성되지 않게함.
            GridISN.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridISN.Properties.RowHeaders = true;
            GridISN.Properties.ColHeaders = true;

            // Column,Row 개수
            GridISN.ColCount = 3;
            GridISN.RowCount = 16;

            // Column 가로 크기설정
            GridISN.ColWidths.SetSize(0, 120);
            GridISN.ColWidths.SetSize(1, 60);
            GridISN.ColWidths.SetSize(2, 80);
            GridISN.ColWidths.SetSize(3, 250);

            for (i = 0; i < 17; i++)
            {
                GridISN.RowHeights[i] = 27;
            }

            for (i = 0; i < 16; i++)
            {
                GridISN[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                GridISN[i + 1, 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            // Text Display
            GridISN[0, 0].Text = "Parameter";
            GridISN[0, 1].Text = "Unit";
            GridISN[0, 2].Text = "Data";
            GridISN[0, 3].Text = "Description";

            GridISN[1, 0].Text = "Facet 1 Start";
            GridISN[2, 0].Text = "Facet 1 End";
            GridISN[3, 0].Text = "Facet 2 Start";
            GridISN[4, 0].Text = "Facet 2 End";
            GridISN[5, 0].Text = "Facet 3 Start";
            GridISN[6, 0].Text = "Facet 3 End";
            GridISN[7, 0].Text = "Facet 4 Start";
            GridISN[8, 0].Text = "Facet 4 End";
            GridISN[9, 0].Text = "Facet 5 Start";
            GridISN[10, 0].Text = "Facet 5 End";
            GridISN[11, 0].Text = "Facet 6 Start";
            GridISN[12, 0].Text = "Facet 6 End";
            GridISN[13, 0].Text = "Facet 7 Start";
            GridISN[14, 0].Text = "Facet 7 End";
            GridISN[15, 0].Text = "Facet 8 Start";
            GridISN[16, 0].Text = "Facet 8 End";

            GridISN[1, 1].Text = "[um]";
            GridISN[2, 1].Text = "[um]";
            GridISN[3, 1].Text = "[um]";
            GridISN[4, 1].Text = "[um]";
            GridISN[5, 1].Text = "[um]";
            GridISN[6, 1].Text = "[um]";
            GridISN[7, 1].Text = "[um]";
            GridISN[8, 1].Text = "[um]";
            GridISN[9, 1].Text = "[um]";
            GridISN[10, 1].Text = "[um]";
            GridISN[11, 1].Text = "[um]";
            GridISN[12, 1].Text = "[um]";
            GridISN[13, 1].Text = "[um]";
            GridISN[14, 1].Text = "[um]";
            GridISN[15, 1].Text = "[um]";
            GridISN[16, 1].Text = "[um]";

            GridISN[1, 3].Text = "Facet 1 의 Start 가로 위치(X 방향) 보정";
            GridISN[2, 3].Text = "Facet 1 의 End 가로 위치(X 방향) 보정";
            GridISN[3, 3].Text = "Facet 2 의 Start 가로 위치(X 방향) 보정";
            GridISN[4, 3].Text = "Facet 2 의 End 가로 위치(X 방향) 보정";
            GridISN[5, 3].Text = "Facet 3 의 Start 가로 위치(X 방향) 보정";
            GridISN[6, 3].Text = "Facet 3 의 End 가로 위치(X 방향) 보정";
            GridISN[7, 3].Text = "Facet 4 의 Start 가로 위치(X 방향) 보정";
            GridISN[8, 3].Text = "Facet 4 의 End 가로 위치(X 방향) 보정";
            GridISN[9, 3].Text = "Facet 5 의 Start 가로 위치(X 방향) 보정";
            GridISN[10, 3].Text = "Facet 5 의 End 가로 위치(X 방향) 보정";
            GridISN[11, 3].Text = "Facet 6 의 Start 가로 위치(X 방향) 보정";
            GridISN[12, 3].Text = "Facet 6 의 End 가로 위치(X 방향) 보정";
            GridISN[13, 3].Text = "Facet 7 의 Start 가로 위치(X 방향) 보정";
            GridISN[14, 3].Text = "Facet 7 의 End 가로 위치(X 방향) 보정";
            GridISN[15, 3].Text = "Facet 8 의 Start 가로 위치(X 방향) 보정";
            GridISN[16, 3].Text = "Facet 8 의 End 가로 위치(X 방향) 보정";


            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 17; j++)
                {
                    // Font Style - Bold
                    GridISN[j, i].Font.Bold = true;

                    GridISN[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i != 3)
                    {
                        GridISN[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }
                }
            }

            GridISN.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridISN.ResizeColsBehavior = 0;
            GridISN.ResizeRowsBehavior = 0;

            //GetPolygonPara(scannerIndex);

            // Grid Display Update
            GridISN.Refresh();


        }
        private void UpdateConfigureData(CSystemData_Scanner m_PolygonPara)
        {
            //====================================================================================================
            // Config.ini
            
            GridConfigure[1, 2].Text  = string.Format("{0:f3}", m_PolygonPara.InScanResolution );
            GridConfigure[2, 2].Text  = string.Format("{0:f3}", m_PolygonPara.CrossScanResolution);
            GridConfigure[3, 2].Text  = string.Format("{0:f3}", m_PolygonPara.InScanOffset);
            GridConfigure[4, 2].Text  = Convert.ToString(m_PolygonPara.StopMotorBetweenJobs);
            GridConfigure[5, 2].Text  = Convert.ToString(m_PolygonPara.PixInvert);
            GridConfigure[6, 2].Text  = Convert.ToString(m_PolygonPara.JobStartBufferTime);
            GridConfigure[7, 2].Text  = Convert.ToString(m_PolygonPara.PrecedingBlankLines);

            GridConfigure[8, 2].Text  = string.Format("{0:f0}", m_PolygonPara.SeedClockFrequency);
            GridConfigure[9, 2].Text  = string.Format("{0:f0}", m_PolygonPara.RepetitionRate);
            GridConfigure[10, 2].Text = string.Format("{0:f0}", m_PolygonPara.PulsePickWidth);
            GridConfigure[11, 2].Text = string.Format("{0:f0}", m_PolygonPara.PixelWidth);
            GridConfigure[12, 2].Text = string.Format("{0:f4}", m_PolygonPara.CrossScanEncoderResol);

            GridConfigure[13, 2].Text = Convert.ToString(m_PolygonPara.CrossScanMaxAccel);
            GridConfigure[14, 2].Text = Convert.ToString(m_PolygonPara.EnCarSig);
            GridConfigure[15, 2].Text = Convert.ToString(m_PolygonPara.SwapCarSig);
            GridConfigure[16, 2].Text = Convert.ToString(m_PolygonPara.InterleaveRatio);
            GridConfigure[17, 2].Text = string.Format("{0:f3}", m_PolygonPara.FacetFineDelayOffset0 );
            GridConfigure[18, 2].Text = string.Format("{0:f3}", m_PolygonPara.FacetFineDelayOffset1 );
            GridConfigure[19, 2].Text = string.Format("{0:f3}", m_PolygonPara.FacetFineDelayOffset2 );
            GridConfigure[20, 2].Text = string.Format("{0:f3}", m_PolygonPara.FacetFineDelayOffset3 );
            GridConfigure[21, 2].Text = string.Format("{0:f3}", m_PolygonPara.FacetFineDelayOffset4 );
            GridConfigure[22, 2].Text = string.Format("{0:f3}", m_PolygonPara.FacetFineDelayOffset5 );
            GridConfigure[23, 2].Text = string.Format("{0:f3}", m_PolygonPara.FacetFineDelayOffset6 );
            GridConfigure[24, 2].Text = string.Format("{0:f3}", m_PolygonPara.FacetFineDelayOffset7 );
            GridConfigure[25, 2].Text = Convert.ToString(m_PolygonPara.StartFacet);
            GridConfigure[26, 2].Text = Convert.ToString(m_PolygonPara.AutoIncrementStartFacet);
            GridConfigure[27, 2].Text = Convert.ToString(m_PolygonPara.MotorStableTime);

            //====================================================================================================
            // isn.ini
            GridISN[1,  2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstXpos1);
            GridISN[2,  2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Xpos1);
            GridISN[3,  2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstXpos2);
            GridISN[4,  2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Xpos2);
            GridISN[5,  2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstXpos3);
            GridISN[6,  2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Xpos3);
            GridISN[7,  2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstXpos4);
            GridISN[8,  2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Xpos4);
            GridISN[9,  2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstXpos5);
            GridISN[10, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Xpos5);
            GridISN[11, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstXpos6);
            GridISN[12, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Xpos6);
            GridISN[13, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstXpos7);
            GridISN[14, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Xpos7);
            GridISN[15, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstXpos8);
            GridISN[16, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Xpos8);

            //====================================================================================================
            // csn.ini
            GridCSN[1, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstYpos1);
            GridCSN[2, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Ypos1);
            GridCSN[3, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstYpos2);
            GridCSN[4, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Ypos2);
            GridCSN[5, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstYpos3);
            GridCSN[6, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Ypos3);
            GridCSN[7, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstYpos4);
            GridCSN[8, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Ypos4);
            GridCSN[9, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstYpos5);
            GridCSN[10, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Ypos5);
            GridCSN[11, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstYpos6);
            GridCSN[12, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Ypos6);
            GridCSN[13, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstYpos7);
            GridCSN[14, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Ypos7);
            GridCSN[15, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectFirstYpos8);
            GridCSN[16, 2].Text = string.Format("{0:f0}", m_PolygonPara.FacetCorrectLast_Ypos8);

        }

        private void BtnConfigureExit_Click(object sender, EventArgs e)
        {            

            this.Hide();

        }

        private void GridConfigure_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol != 2 || nRow == 0)
            {
                return;
            }

            strCurrent = GridConfigure[nRow, nCol].Text;
            
            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridConfigure[nRow, nCol].Text = strModify;
        }

        private void lblControlAddress_Click(object sender, EventArgs e)
        {

        }

        private void lblControlPort_Click(object sender, EventArgs e)
        {

        }

        private void lblHeadAddress_Click(object sender, EventArgs e)
        {

        }

        private void lblHeadPort_Click(object sender, EventArgs e)
        {

        }

        private void BtnConfigSave_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage("Save parameter ?", EMessageType.OK_CANCEL);
            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //====================================================================================================
            // Config.ini

            CMainFrame.DataManager.ModelData.ScanData.InScanResolution           = Convert.ToSingle(GridConfigure[1, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.CrossScanResolution        = Convert.ToSingle(GridConfigure[2, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.InScanOffset               = Convert.ToSingle(GridConfigure[3, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.StopMotorBetweenJobs       = Convert.ToInt16(GridConfigure[4, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.PixInvert                  = Convert.ToInt16(GridConfigure[5, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.JobStartBufferTime         = Convert.ToInt16(GridConfigure[6, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.PrecedingBlankLines        = Convert.ToInt16(GridConfigure[7, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.SeedClockFrequency         = Convert.ToSingle(GridConfigure[8, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.RepetitionRate             = Convert.ToSingle(GridConfigure[9, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.PulsePickWidth             = Convert.ToInt16(GridConfigure[10, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.PixelWidth                 = Convert.ToInt16(GridConfigure[11, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.CrossScanEncoderResol      = Convert.ToSingle(GridConfigure[12, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.CrossScanMaxAccel          = Convert.ToSingle(GridConfigure[13, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.EnCarSig                   = Convert.ToInt16(GridConfigure[14, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.SwapCarSig                 = Convert.ToInt16(GridConfigure[15, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.InterleaveRatio            = Convert.ToInt16(GridConfigure[16, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset0      = Convert.ToSingle(GridConfigure[17, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset1      = Convert.ToSingle(GridConfigure[18, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset2      = Convert.ToSingle(GridConfigure[19, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset3      = Convert.ToSingle(GridConfigure[20, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset4      = Convert.ToSingle(GridConfigure[21, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset5      = Convert.ToSingle(GridConfigure[22, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset6      = Convert.ToSingle(GridConfigure[23, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset7      = Convert.ToSingle(GridConfigure[24, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.StartFacet                 = Convert.ToInt16(GridConfigure[25, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.AutoIncrementStartFacet    = Convert.ToInt16(GridConfigure[26, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.MotorStableTime            = Convert.ToInt16(GridConfigure[27, 2].Text);                         
            
            //====================================================================================================
            // isn.ini

            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstXpos1 = Convert.ToInt32(GridISN[1,  2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos1 = Convert.ToInt32(GridISN[2,  2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstXpos2 = Convert.ToInt32(GridISN[3,  2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos2 = Convert.ToInt32(GridISN[4,  2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstXpos3 = Convert.ToInt32(GridISN[5,  2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos3 = Convert.ToInt32(GridISN[6,  2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstXpos4 = Convert.ToInt32(GridISN[7,  2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos4 = Convert.ToInt32(GridISN[8,  2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstXpos5 = Convert.ToInt32(GridISN[9,  2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos5 = Convert.ToInt32(GridISN[10, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstXpos6 = Convert.ToInt32(GridISN[11, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos6 = Convert.ToInt32(GridISN[12, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstXpos7 = Convert.ToInt32(GridISN[13, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos7 = Convert.ToInt32(GridISN[14, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstXpos8 = Convert.ToInt32(GridISN[15, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos8 = Convert.ToInt32(GridISN[16, 2].Text);

            //====================================================================================================
            // csn.ini

            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos1 = Convert.ToInt32(GridCSN[1, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos1 = Convert.ToInt32(GridCSN[2, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos2 = Convert.ToInt32(GridCSN[3, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos2 = Convert.ToInt32(GridCSN[4, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos3 = Convert.ToInt32(GridCSN[5, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos3 = Convert.ToInt32(GridCSN[6, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos4 = Convert.ToInt32(GridCSN[7, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos4 = Convert.ToInt32(GridCSN[8, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos5 = Convert.ToInt32(GridCSN[9, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos5 = Convert.ToInt32(GridCSN[10, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos6 = Convert.ToInt32(GridCSN[11, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos6 = Convert.ToInt32(GridCSN[12, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos7 = Convert.ToInt32(GridCSN[13, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos7 = Convert.ToInt32(GridCSN[14, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos8 = Convert.ToInt32(GridCSN[15, 2].Text);
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos8 = Convert.ToInt32(GridCSN[16, 2].Text);

            // DB Save
            CMainFrame.DataManager.SaveModelData(CMainFrame.DataManager.ModelData);

        }

        private void tabPolygonForm_Click(object sender, EventArgs e)
        {
           // UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
           // DisplayProcessData();
        }

        private void tabPolygonForm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GridISN_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol != 2 || nRow == 0)
            {
                return;
            }

            strCurrent = GridISN[nRow, nCol].Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridISN[nRow, nCol].Text = strModify;
        }

        private void GridCSN_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol != 2 || nRow == 0)
            {
                return;
            }

            strCurrent = GridCSN[nRow, nCol].Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridCSN[nRow, nCol].Text = strModify;
        }

        private void btnWindowShow_Click(object sender, EventArgs e)
        {
        //    var m_FormScanWindow = new FormScanWindow();
        //    m_FormScanWindow.ShowDialog();
            CMainFrame.LWDicer.m_MeScanner.m_RefComp.FormScanner.Show();
        }

        
        private void btnImageUpdate_Click(object sender, EventArgs e)
        {
            string filename = string.Empty;
            OpenFileDialog imgOpenDlg = new OpenFileDialog();
            imgOpenDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
            imgOpenDlg.Filter = "BMP(*.bmp)|*.bmp";
            if (imgOpenDlg.ShowDialog() == DialogResult.OK)
            {
                filename = imgOpenDlg.FileName;                 
                CMainFrame.LWDicer.m_MeScanner.SendBitmap(filename);
            }            
        }

        private void btnImageChange_Click(object sender, EventArgs e)
        {
            Bitmap sourceBitmap;
            Bitmap changeBitmap;
            string filename = string.Empty;

            OpenFileDialog imgOpenDlg = new OpenFileDialog();
            imgOpenDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
            imgOpenDlg.Filter = "BMP(*.bmp)|*.bmp";
            if (imgOpenDlg.ShowDialog() == DialogResult.OK)
            {
                // bmp file Load
                filename = imgOpenDlg.FileName;
                sourceBitmap = new Bitmap(filename);
                
                // bmp expand by 8 (default)
                changeBitmap = CMainFrame.LWDicer.m_MeScanner.ExpandBmpFile(sourceBitmap);

                changeBitmap.Save(filename+"Ex");
            }
        }


        private void btnDataUpdate_Click(object sender, EventArgs e)
        {
            // ini File 저장
            CMainFrame.LWDicer.m_MeScanner.SaveConfigPara("config_job");

            // ini File 저장
            CMainFrame.LWDicer.m_MeScanner.SaveIsnPara("isn_job");

            // ini File 저장
            CMainFrame.LWDicer.m_MeScanner.SaveCsnPara("csn_job");

            FormPolygonSelectPara formSelect = new FormPolygonSelectPara();

            formSelect.ShowDialog();
            
        }


        private void FormPolygon_Activated(object sender, EventArgs e)
        {
            //UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
        }

        private void FormPolygon_Shown(object sender, EventArgs e)
        {
            UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
        }

        private void btnControlSendConnect_Click_1(object sender, EventArgs e)
        {
            //CMainFrame.LWDicer.m_MeScanner.ConnetTelnet(EMonitorMode.Controller);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            
        }      

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                CMainFrame.LWDicer.m_MeScanner.TestCmd(textBox1.Text);

                ////rtbControllerStatus.Text = strData;
                //rtbControllerStatus.AppendText(strData + Environment.NewLine);
                //rtbControllerStatus.ScrollToCaret();

                textBox1.Text = "";
            }
        }

        private void btnControlReconnect_Click(object sender, EventArgs e)
        {
            //CMainFrame.LWDicer.m_MeScanner.ConnetTelnet(EMonitorMode.Controller);
            //rtbControllerStatus.Clear();
        }

        private void btnHeadReconnect_Click(object sender, EventArgs e)
        {
            //CMainFrame.LWDicer.m_MeScanner.ConnetTelnet(EMonitorMode.Head);
            //rtbHeadStatus.Clear();
        }

        private void btnControlLogClear_Click(object sender, EventArgs e)
        {
            rtbControllerStatus.Clear();
        }

        private void btnHeadLogClear_Click(object sender, EventArgs e)
        {
            rtbHeadStatus.Clear();
        }

        private void ChangeTextData(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            GradientLabel Btn = sender as GradientLabel;
            strCurrent = Btn.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            Btn.Text = strModify;
        }

        private void btnProcessDataSave_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage("프로세스 데이터를 저장하시겠습니까?", EMessageType.OK_CANCEL);
            
            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            UpdateProcessData();


            CMainFrame.DataManager.SaveModelData(CMainFrame.DataManager.ModelData);
        }

        private void btnProcessDataCreate_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage( "비트맵 파일을 생성하시겠습니까?", EMessageType.OK_CANCEL);
            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            UpdateProcessData();

            MakeWaferEllipseOutLine();
        }

        private void MakeWaferEllipseOutLine()
        {
            double X1, X2, Y1, Y2;

            // Wafer상의 설정된 두점을  읽어온다
            X1 = CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize / 2.0f;
            X2 = CMainFrame.DataManager.ModelData.ProcData.MarginWidth / 2.0f;

            Y1 = CMainFrame.DataManager.ModelData.ProcData.MarginHeight / 2.0f;
            Y2 = CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize / 2.0f;

            double EllipseA, EllipseB;

            // 설정된 두점을 기준으로 타원의 공식을 구한다.
            EllipseA = Math.Sqrt((X2 * X2 * Y1 * Y1 - X1 * X1 * Y2 * Y2) / (Y1 * Y1 - Y2 * Y2));
            EllipseB = Math.Sqrt((X1 * X1 * Y2 * Y2 - X2 * X2 * Y1 * Y1) / (X1 * X1 - X2 * X2));

            MakeWaferCutPattern(EllipseA, EllipseB);
        }

        private void MakeWaferCutPattern(double A, double B)
        {
            // Cut Pattern을 Draw하기 위한 변수 설정
            float cutBeginPos = CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize / 2.0f;
            float cutLastPos = -CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize / 2.0f;
            float ellipseMove = CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize / 2.0f;

            // 타원의 수평이 같은 X1,X2 위치값
            float ellipseX1, ellipseX2;
            PointF startLine = new PointF(0, 0);
            PointF endLine = new PointF(0, 0);

              
            // 타원의 Y축 상부 시작 위치
            double Y = cutBeginPos;
            int bmpFileNameNum = 0;

            do
            {
                ellipseX1 = ellipseMove - (float)(Math.Sqrt(A * A * B * B - A * A * Y * Y) / B);
                ellipseX2 = ellipseMove + (float)(Math.Sqrt(A * A * B * B - A * A * Y * Y) / B);

                if (ellipseX1 < 0) ellipseX1 = 0;
                if (ellipseX2 > CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize) ellipseX2 = CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize;

                startLine.X = ellipseX1;
                endLine.X   = ellipseX2;
                startLine.Y = endLine.Y = 0;

                for (int i = 0; i < CMainFrame.DataManager.ModelData.ProcData.ProcessLineNum; i++)
                {
                    startLine.Y = endLine.Y = CMainFrame.DataManager.ModelData.ScanData.CrossScanResolution * i;
                    CMainFrame.LWDicer.m_MeScanner.m_RefComp.Manager.AddObject(EObjectType.LINE, startLine, endLine);   
                }

                string filename = string.Format("{0:s}{1:s}{2:F0}.bmp", CMainFrame.DBInfo.ImageDataDir, "CutAxisX_", bmpFileNameNum+1);

                CMainFrame.LWDicer.m_MeScanner.SetSizeBmp();
                CMainFrame.LWDicer.m_MeScanner.ConvertBmpFile(filename);

                CMainFrame.LWDicer.m_MeScanner.m_RefComp.Manager.DeleteAllObject();
                bmpFileNameNum++;
                Y -= CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeX;

            } while (Y > cutLastPos);

            // 타원의 Y축 상부 시작 위치
            Y = cutBeginPos;
            bmpFileNameNum = 0;

            do
            {
                ellipseX1 = ellipseMove - (float)(Math.Sqrt(A * A * B * B - A * A * Y * Y) / B);
                ellipseX2 = ellipseMove + (float)(Math.Sqrt(A * A * B * B - A * A * Y * Y) / B);

                if (ellipseX1 < 0) ellipseX1 = 0;
                if (ellipseX2 > CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize) ellipseX2 = CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize;

                startLine.X = ellipseX1;
                endLine.X = ellipseX2;
                startLine.Y = endLine.Y = 0;

                for (int i = 0; i < CMainFrame.DataManager.ModelData.ProcData.ProcessLineNum; i++)
                {
                    startLine.Y = endLine.Y = CMainFrame.DataManager.ModelData.ScanData.CrossScanResolution * i;
                    CMainFrame.LWDicer.m_MeScanner.m_RefComp.Manager.AddObject(EObjectType.LINE, startLine, endLine);
                }

                string filename = string.Format("{0:s}{1:s}{2:F0}.bmp", CMainFrame.DBInfo.ImageDataDir, "CutAxisY_", bmpFileNameNum+1);

                CMainFrame.LWDicer.m_MeScanner.SetSizeBmp();
                CMainFrame.LWDicer.m_MeScanner.ConvertBmpFile(filename);

                CMainFrame.LWDicer.m_MeScanner.m_RefComp.Manager.DeleteAllObject();
                bmpFileNameNum++;
                Y -= CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeY;

            } while (Y > cutLastPos);

        }
        private void UpdateProcessData()
        {
            CMainFrame.DataManager.ModelData.ScanData.InScanResolution    = Convert.ToSingle(lblResolutionX.Text);
            CMainFrame.DataManager.ModelData.ScanData.CrossScanResolution = Convert.ToSingle(lblResolutionY.Text);
            CMainFrame.DataManager.ModelData.ScanData.InScanOffset        = Convert.ToSingle(lblOffsetX.Text);
            CMainFrame.DataManager.ModelData.ScanData.RepetitionRate      = Convert.ToSingle(lblRepetitionLaser.Text);
            CMainFrame.DataManager.ModelData.ScanData.InterleaveRatio     = Convert.ToInt32(cbbOverlap.Text);

            CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize    = Convert.ToSingle(lblWaferSize.Text);
            CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeX       = Convert.ToSingle(lblDiePitchX.Text);
            CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeY       = Convert.ToSingle(lblDiePitchY.Text);
            CMainFrame.DataManager.ModelData.ProcData.MarginWidth         = Convert.ToSingle(lblMarginWidth.Text);
            CMainFrame.DataManager.ModelData.ProcData.MarginHeight        = Convert.ToSingle(lblMarginHeight.Text);
            CMainFrame.DataManager.ModelData.ProcData.ProcessLineNum      = Convert.ToInt32(lblOverlapCount.Text);


            
        }

        private void DisplayProcessData()
        {
            lblResolutionX.Text     = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ScanData.InScanResolution);
            lblResolutionY.Text     = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ScanData.CrossScanResolution);
            lblOffsetX.Text         = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ScanData.InScanOffset);
            lblRepetitionLaser.Text = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ScanData.RepetitionRate);
            cbbOverlap.Text         = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ScanData.InterleaveRatio);

            lblWaferSize.Text       = string.Format("{0:F1}", CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize);
            lblDiePitchX.Text       = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeX);
            lblDiePitchY.Text       = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeY);
            lblMarginWidth.Text     = string.Format("{0:F1}", CMainFrame.DataManager.ModelData.ProcData.MarginWidth);
            lblMarginHeight.Text    = string.Format("{0:F1}", CMainFrame.DataManager.ModelData.ProcData.MarginHeight);
            lblOverlapCount.Text    = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.ProcessLineNum);


            lblProcessOffsetX1.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX1);
            lblProcessOffsetY1.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY1);
            lblProcessCount1.Text = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.ProcessCount1);
            lblProcessOffsetX2.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX2);
            lblProcessOffsetY2.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY2);
            lblProcessCount2.Text = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.ProcessCount2);

            lblPatternPitch1.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.PatternPitch1);
            lblPatternCount1.Text = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.PatternCount1);
            lblPatternPitch2.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.PatternPitch2);
            lblPatternCount2.Text = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.PatternCount2);
            lblPatternOffset1.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.PatternOffset1);
            lblPatternOffset2.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.PatternOffset2);

            lblProcessInterval.Text = string.Format("{0:F3}", CMainFrame.DataManager.ModelData.ProcData.ProcessInterval);

        }
        private void cbbOverlap_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


        private void btnExportConfig_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage("Config.ini 파일을 덮어 쓰시겠습니까?", EMessageType.OK_CANCEL);
            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filename = string.Empty;
            SaveFileDialog IniSaveDlg = new SaveFileDialog();
            IniSaveDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            IniSaveDlg.Filter = "INI(*.ini)|*.ini";
            if (IniSaveDlg.ShowDialog() == DialogResult.OK)
            {
                filename = IniSaveDlg.FileName;
            }

            CMainFrame.DataManager.ExportPolygonData(EPolygonPara.CONFIG, filename);
        }

        private void btnExportIsn_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage("Isn.ini 파일을 덮어 쓰시겠습니까?", EMessageType.OK_CANCEL);
            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filename = string.Empty;
            SaveFileDialog IniSaveDlg = new SaveFileDialog();
            IniSaveDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            IniSaveDlg.Filter = "INI(*.ini)|*.ini";
            if (IniSaveDlg.ShowDialog() == DialogResult.OK)
            {
                filename = IniSaveDlg.FileName;
            }

            CMainFrame.DataManager.ExportPolygonData(EPolygonPara.ISN,filename);
        }

        private void btnExportCsn_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage("Csn.ini 파일을 덮어 쓰시겠습니까?", EMessageType.OK_CANCEL);
            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filename = string.Empty;
            SaveFileDialog IniSaveDlg = new SaveFileDialog();
            IniSaveDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            IniSaveDlg.Filter = "INI(*.ini)|*.ini";
            if (IniSaveDlg.ShowDialog() == DialogResult.OK)
            {
                filename = IniSaveDlg.FileName;
            }

            CMainFrame.DataManager.ExportPolygonData(EPolygonPara.CSN,filename);

        }

        private void btnImportConfig_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage( "Config.ini 파일을 읽어오시겠습니까?", EMessageType.OK_CANCEL);
            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filename = string.Empty;
            OpenFileDialog iniOpenDlg = new OpenFileDialog();
            iniOpenDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            iniOpenDlg.Filter = "INI(*.ini)|*.ini";
            if (iniOpenDlg.ShowDialog() == DialogResult.OK)
            {
                filename = iniOpenDlg.FileName;
            }
            //CMainFrame.DataManager.ImportPolygonData(EPolygonPara.CONFIG);
            CMainFrame.DataManager.ImportPolygonData(EPolygonPara.CONFIG,filename);

            UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
        }

        private void btnImportIsn_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage( "Isn.ini 파일을 읽어오시겠습니까?", EMessageType.OK_CANCEL);
            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filename = string.Empty;
            OpenFileDialog iniOpenDlg = new OpenFileDialog();
            iniOpenDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            iniOpenDlg.Filter = "INI(*.ini)|*.ini";
            if (iniOpenDlg.ShowDialog() == DialogResult.OK)
            {
                filename = iniOpenDlg.FileName;
            }

            //CMainFrame.DataManager.ImportPolygonData(EPolygonPara.ISN);
            CMainFrame.DataManager.ImportPolygonData(EPolygonPara.ISN,filename);

            UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
        }

        private void btnImportCsn_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage( "Csn.ini 파일을 읽어오시겠습니까?", EMessageType.OK_CANCEL);
            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filename = string.Empty;
            OpenFileDialog iniOpenDlg = new OpenFileDialog();
            iniOpenDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            iniOpenDlg.Filter = "INI(*.ini)|*.ini";
            if (iniOpenDlg.ShowDialog() == DialogResult.OK)
            {
                filename = iniOpenDlg.FileName;                
            }

            //CMainFrame.DataManager.ImportPolygonData(EPolygonPara.CSN);
            CMainFrame.DataManager.ImportPolygonData(EPolygonPara.CSN, filename);

            UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
        }


        private void tabPageConfig_Enter(object sender, EventArgs e)
        {
            UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
        }


        private void btnVisionSaveZoom_Click(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_Vision == null) return;

            string filename = string.Empty;
            SaveFileDialog imgSaveDlg = new SaveFileDialog();
            imgSaveDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;

            if (imgSaveDlg.ShowDialog() == DialogResult.OK)
            {
                filename = imgSaveDlg.FileName;
                CMainFrame.LWDicer.m_Vision.SaveImage(ZOOM_CAM, filename);

            }
        }

        private void btnVisionLive_Click(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_Vision == null) return;
                       

            CMainFrame.LWDicer.m_Vision.LiveVideo(ZOOM_CAM);
        }

        private void btnVisionHalt_Click(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_Vision == null) return;
            
            CMainFrame.LWDicer.m_Vision.HaltVideo(ZOOM_CAM);
        }

        private void BtnModelCamera_Click(object sender, EventArgs e)
        {

        }

        private void cbbSelectCam_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnChangeCsnGap_Click(object sender, EventArgs e)
        {
            int csnGapPitch = Convert.ToInt32(lblCsnGapPitch.Text);

            string filename = string.Empty;
            OpenFileDialog iniOpenDlg = new OpenFileDialog();
            iniOpenDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            iniOpenDlg.Filter = "INI(*.ini)|*.ini";
            if (iniOpenDlg.ShowDialog() == DialogResult.OK)
            {
                filename = iniOpenDlg.FileName;
            }
            else
            {
                return;
            }

            //CMainFrame.DataManager.ImportPolygonData(EPolygonPara.CSN);
            CMainFrame.DataManager.ImportPolygonData(EPolygonPara.CSN, filename);

            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos1 -= csnGapPitch * 0;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos1 -= csnGapPitch * 0;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos2 -= csnGapPitch * 1;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos2 -= csnGapPitch * 1;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos3 -= csnGapPitch * 2;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos3 -= csnGapPitch * 2;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos4 -= csnGapPitch * 3;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos4 -= csnGapPitch * 3;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos5 -= csnGapPitch * 4;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos5 -= csnGapPitch * 4;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos6 -= csnGapPitch * 5;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos6 -= csnGapPitch * 5;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos7 -= csnGapPitch * 6;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos7 -= csnGapPitch * 6;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectFirstYpos8 -= csnGapPitch * 7;
            CMainFrame.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos8 -= csnGapPitch * 7;

            UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
        }

        private void lblCsnGapPitch_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            GradientLabel Btn = sender as GradientLabel;
            strCurrent = Btn.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            Btn.Text = strModify;
        }

        private void picVisionZoom_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)  // 마우스 왼쪽 버튼
            {
                ptMouseStartPos = e.Location;
                ptMouseEndPos   = e.Location;
            }
            else if (e.Button == MouseButtons.Right)   // 마우스 오른쪽 버튼
            {

            }
        }

        private void picVisionZoom_MouseMove(object sender, MouseEventArgs e)
        {
            lblMousePos.Text = e.Location.ToString();

            if (e.Button == MouseButtons.Left)
            {
                ptMouseEndPos = e.Location;
            }
        }

        private void picVisionZoom_MouseUp(object sender, MouseEventArgs e)
        {
            double lengthPixel = 0.0;
            double lengthReal = 0.0;
            double resolutionPixel = 0.0;
            double lenX = (double) (ptMouseEndPos.X - ptMouseStartPos.X);
            double lenY = (double) (ptMouseEndPos.Y - ptMouseStartPos.Y);

            string textMsg;

            if (e.Button == MouseButtons.Left)
            {
                

                if (eVisionMode == EVisionMode.MEASUREMENT)
                {
                    lenX *= CMainFrame.DataManager.SystemData_Vision.PixelResolutionX[ZOOM_CAM];
                    lenY *= CMainFrame.DataManager.SystemData_Vision.PixelResolutionY[ZOOM_CAM];

                    lengthPixel = Math.Sqrt(lenX * lenX + lenY * lenY);
                    textMsg = string.Format("{0:f2} um", lengthPixel);

                    CMainFrame.LWDicer.m_Vision.ClearOverlay();
                    CMainFrame.LWDicer.m_Vision.DrawOverlayText(ZOOM_CAM, textMsg, ptMouseEndPos);
                }
                if (eVisionMode == EVisionMode.CALIBRATION)
                {
                    lengthPixel = Math.Sqrt(lenX * lenX + lenY * lenY);

                    string strCurrent = "", strModify = "";
                    if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
                    {
                        eVisionMode = EVisionMode.MEASUREMENT;
                        btnCalibration.ForeColor = Color.Black;
                        return;
                    }

                    lengthReal = Convert.ToDouble(strModify);
                    // Calibration Set
                    resolutionPixel = lengthReal / lengthPixel;
                    CMainFrame.DataManager.SystemData_Vision.PixelResolutionX[ZOOM_CAM] = resolutionPixel;
                    CMainFrame.DataManager.SystemData_Vision.PixelResolutionY[ZOOM_CAM] = resolutionPixel;

                    textMsg = string.Format("픽셀당 거리는 {0:F2} um입니다.", resolutionPixel);
                    CMainFrame.DisplayMsg(textMsg);

                    CMainFrame.DataManager.SaveSystemData(null,null, null, null,CMainFrame.DataManager.SystemData_Vision, null);

                    eVisionMode = EVisionMode.MEASUREMENT;
                    btnCalibration.ForeColor = Color.Black;
                }
            }            
        }

        private void picVisionZoom_Paint(object sender, PaintEventArgs e)
        {
            DrawMouseLine(e.Graphics);
        }
        private void DrawMouseLine(Graphics g)
        {
            g.DrawLine(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG], ptMouseStartPos, ptMouseEndPos);
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            eVisionMode = EVisionMode.CALIBRATION;
            CMainFrame.LWDicer.m_Vision.ClearOverlay();
            btnCalibration.ForeColor = Color.Red;
        }

        private void btnMeasure_Click(object sender, EventArgs e)
        {
            eVisionMode = EVisionMode.MEASUREMENT;
            btnCalibration.ForeColor = Color.Black;
        }

        private void btnControlSendConnect_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeScanner.SetControlAddress(lblControlAddress.Text,1111);
            CMainFrame.DataManager.SystemData_Scan.ControlHostAddress    = CMainFrame.LWDicer.m_MeScanner.GetControlAddress();
            CMainFrame.DataManager.SystemData_Scan.ControlHostPort       = CMainFrame.LWDicer.m_MeScanner.GetControlPortNum();
            
            CMainFrame.DataManager.SaveSystemData(null, null, null, null, null, CMainFrame.DataManager.SystemData_Scan, null);

            bool bCheckConnect = false;
            CMainFrame.LWDicer.m_MeScanner.m_RefComp.ControlComm.ConnectServer();
            bCheckConnect = CMainFrame.LWDicer.m_MeScanner.m_RefComp.ControlComm.IsConnected();

        }

        private void btnHeadSendConnect_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeScanner.SetScanHeadAddress(lblHeadAddress.Text, 1111);
            CMainFrame.DataManager.SystemData_Scan.ScanHeadHostAddress   = CMainFrame.LWDicer.m_MeScanner.GetScanHeadAddress();
            CMainFrame.DataManager.SystemData_Scan.ScanHeadHostPort      = CMainFrame.LWDicer.m_MeScanner.GetScanHeadPortNum();

            CMainFrame.DataManager.SaveSystemData(null, null, null, null, null, CMainFrame.DataManager.SystemData_Scan, null);

            CMainFrame.LWDicer.m_MeScanner.m_RefComp.ScanHeadComm.ConnectServer();

        }


        private void lblProcessOffsetX1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessOffsetX1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessOffsetX1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX1 = Convert.ToSingle(strModify);

        }

        private void lblProcessOffsetY1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessOffsetY1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessOffsetY1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY1 = Convert.ToSingle(strModify);
        }

        private void lblProcessCount1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessCount1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessCount1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessCount1 = Convert.ToInt32(strModify);
        }

        private void lblProcessOffsetX2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessOffsetX2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessOffsetX2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX2 = Convert.ToSingle(strModify);
        }

        private void lblProcessOffsetY2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessOffsetY2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessOffsetY2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY2 = Convert.ToSingle(strModify);
        }

        private void lblProcessCount2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessCount2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessCount2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessCount2 = Convert.ToInt32(strModify);
        }

        private void lblPatternPitch1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternPitch1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternPitch1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternPitch1 = Convert.ToSingle(strModify);
        }

        private void lblPatternCount1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternCount1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternCount1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternCount1 = Convert.ToInt32(strModify);
        }

        private void lblPatternPitch2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternPitch2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternPitch2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternPitch2 = Convert.ToSingle(strModify);
        }

        private void lblPatternCount2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternCount2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternCount2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternCount2 = Convert.ToInt32(strModify);
        }

        private void lblPatternOffset1_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternOffset1.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternOffset1.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternOffset1 = Convert.ToSingle(strModify);
        }

        private void lblPatternOffset2_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblPatternOffset2.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblPatternOffset2.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.PatternOffset2 = Convert.ToSingle(strModify);
        }

        private void btnPatternProcessSave_Click(object sender, EventArgs e)
        {
            FormMessageBox MsgBox = new FormMessageBox();

            MsgBox.SetMessage("프로세스 데이터를 저장하시겠습니까?", EMessageType.OK_CANCEL);

            if (MsgBox.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            CMainFrame.DataManager.SaveModelData(CMainFrame.DataManager.ModelData);

            DisplayProcessData();

        }

        private void btnLaserProcessMof_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.LaserProcessMof();

        }
        private void btnLaserProcessStep1_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.LaserProcessStep1();
        }

        private void btnLaserProcessStep2_Click(object sender, EventArgs e)
        {
            int iResult;
            CMainFrame.LWDicer.m_ctrlStage1.LaserProcessStep2();
        }

        private void tabPageProcess266_Click(object sender, EventArgs e)
        {

        }

        private void lblProcessInterval_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessInterval.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessInterval.Text = strModify;

            CMainFrame.DataManager.ModelData.ProcData.ProcessInterval = Convert.ToInt32(strModify);
        }

        private void btnLaserProcessStop_Click(object sender, EventArgs e)
        {
            CMainFrame.DataManager.ModelData.ProcData.ProcessStop = true;
        }

        private void lblProcessCount_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = lblProcessCount.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            lblProcessCount.Text = strModify;

            CMainFrame.LWDicer.m_MeScanner.LaserProcessCount(Convert.ToInt32(strModify));
        }

        private void btnLaserProcessMofStop_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeScanner.LaserProcessStop();
        }

        
    }
}
