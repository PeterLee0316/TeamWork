using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Collections.Specialized;

using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;

//using static LWDicer.Layers.DEF_Scanner;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_MeStage;
using static LWDicer.Layers.DEF_CtrlStage;

using WW.Actions;
using WW.Cad.Drawing;
using WW.Cad.Drawing.GDI;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Drawing;
using WW.Math;
using WW.Math.Geometry;
using WW.Windows;
using WW.Cad.IO;

using LWDicer.Layers;

namespace LWDicer.UI
{
    public partial class FormPolygon : Form
    {
        private Size OriginFormSize = new Size(0, 0);
        private Point ptMouseStartPos = new Point(0, 0);
        private Point ptMouseEndPos = new Point(0, 0);

        private string[] strOP = new string[(int)ELaserOperation.MAX];
        private int selectedSequenceNum;
        private CLaserProcessData LaserProcessData;

        private EVisionMode m_VisionMode;
        private EScannerIndex m_ScannerIndex;

        private string m_strLastDir;
        private string m_strLastFile;

        private DxfModel CadDrawing;

        public FormPolygon()
        {
            InitializeComponent();

            InitTabPage();
            InitConfigureGrid();

            OriginFormSize.Width = this.Width;
            OriginFormSize.Height = this.Height;
            m_VisionMode = EVisionMode.MEASUREMENT;

            // ComboBox Init
            for (int i = 0; i < (int)EScannerIndex.MAX; i++)
            {
                ComboScannerIndex.Items.Add(EScannerIndex.SCANNER1 + i);
            }
            ComboScannerIndex.SelectedIndex = 0;
            
            
            //======================================================================
            // Drawing Data Init
            CadDrawing = new DxfModel();
            // 임시 Line 생성
            DxfLine xaxis = new DxfLine(new Point2D(0d, 0d), new Point2D(10d, 0d));
            xaxis.Color = EntityColors.LightGray;
            CadDrawing.Entities.Add(xaxis);

            windowDxf.Model = CadDrawing;
        }

        private void FormPolygon_Load(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.InitialLocalView(INSP_CAM, picVisionZoom.Handle);
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
                        this.tabPageConfig,
                        this.tabPageDrawing,
                        this.tabPageScanner,
                        this.tabPageLaser,
                        this.tabPageVision,
                     });
        }

        

        // Polygon Scanner Configure Data
        private void InitConfigureGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridConfigure.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridConfigure.Properties.RowHeaders = true;
            GridConfigure.Properties.ColHeaders = true;

            // Column,Row 개수
            GridConfigure.ColCount = 3;
            GridConfigure.RowCount = 56;

            // Column 가로 크기설정
            GridConfigure.ColWidths.SetSize(0, 200);
            GridConfigure.ColWidths.SetSize(1, 80);
            GridConfigure.ColWidths.SetSize(2, 100);
            GridConfigure.ColWidths.SetSize(3, 500);

            for (int i = 0; i < GridConfigure.RowCount+1; i++)
            {
                GridConfigure.RowHeights[i] = 27;
            }

            for (int i = 1; i < GridConfigure.RowCount+1; i++)
            {
                GridConfigure[i , 1].BackColor = System.Drawing.Color.FromArgb(230, 210, 255);
                GridConfigure[i , 3].BackColor = System.Drawing.Color.FromArgb(255, 230, 255);
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
            GridConfigure[8, 0].Text = "LaserOperationMode";
            GridConfigure[9, 0].Text = "SeedClockFrequency";
            GridConfigure[10, 0].Text = "RepetitionRate";
            GridConfigure[11, 0].Text = "PulsePickWidth";
            GridConfigure[12, 0].Text = "PixelWidth";
            GridConfigure[13, 0].Text = "PulsePickAlgor";

            GridConfigure[14, 0].Text = "CrossScanEncoderResol";
            GridConfigure[15, 0].Text = "CrossScanMaxAccel";
            GridConfigure[16, 0].Text = "EnCarSig";
            GridConfigure[17, 0].Text = "SwapCarSig";

            GridConfigure[18, 0].Text = "SerialNumber";
            GridConfigure[19, 0].Text = "FThetaConstant";
            GridConfigure[20, 0].Text = "ExposeLineLength";
            GridConfigure[21, 0].Text = "EncoderIndexDelay";
            GridConfigure[22, 0].Text = "FacetFineDelay0";
            GridConfigure[23, 0].Text = "FacetFineDelay1";
            GridConfigure[24, 0].Text = "FacetFineDelay2";
            GridConfigure[25, 0].Text = "FacetFineDelay3";
            GridConfigure[26, 0].Text = "FacetFineDelay4";
            GridConfigure[27, 0].Text = "FacetFineDelay5";
            GridConfigure[28, 0].Text = "FacetFineDelay6";
            GridConfigure[29, 0].Text = "FacetFineDelay7";
            GridConfigure[30, 0].Text = "InterLeaveRatio";
            GridConfigure[31, 0].Text = "FacetFineDelayOffset0";
            GridConfigure[32, 0].Text = "FacetFineDelayOffset1";
            GridConfigure[33, 0].Text = "FacetFineDelayOffset2";
            GridConfigure[34, 0].Text = "FacetFineDelayOffset3";
            GridConfigure[35, 0].Text = "FacetFineDelayOffset4";
            GridConfigure[36, 0].Text = "FacetFineDelayOffset5";
            GridConfigure[37, 0].Text = "FacetFineDelayOffset6";
            GridConfigure[38, 0].Text = "FacetFineDelayOffset7";
            GridConfigure[39, 0].Text = "StartFacet";
            GridConfigure[40, 0].Text = "AutoIncrementStartFacet";

            GridConfigure[41, 0].Text = "MotorDriverType";
            GridConfigure[42, 0].Text = "MinMotorSpeed";
            GridConfigure[43, 0].Text = "MaxMotorSpeed";
            GridConfigure[44, 0].Text = "SyncWaitTime";
            GridConfigure[45, 0].Text = "MotorStableTime";
            GridConfigure[46, 0].Text = "ShaftEncoderPulseCount";
            GridConfigure[47, 0].Text = "SwapReferenceSignals";

            GridConfigure[48, 0].Text = "InterruptFreq";
            GridConfigure[49, 0].Text = "HWDebugSelection";
            GridConfigure[50, 0].Text = "ExpoDebugSelection";
            GridConfigure[51, 0].Text = "AutoRepeat";
            GridConfigure[52, 0].Text = "JobstartAutorepeat";
            GridConfigure[53, 0].Text = "TelnetTimeout";
            GridConfigure[54, 0].Text = "TelnetDebugTimeout";
            GridConfigure[55, 0].Text = "SyncSlaves";
            GridConfigure[56, 0].Text = "SyncTimeout";

            GridConfigure[1, 1].Text = "[mm]";
            GridConfigure[2, 1].Text = "[mm]";
            GridConfigure[3, 1].Text = "[mm]";
            GridConfigure[4, 1].Text = "[-]";
            GridConfigure[5, 1].Text = "[-]";
            GridConfigure[6, 1].Text = "[sec]";
            GridConfigure[7, 1].Text = "[-]";

            GridConfigure[8, 1].Text = "[-]";
            GridConfigure[9, 1].Text = "[kHz]";
            GridConfigure[10, 1].Text = "[kHz]";
            GridConfigure[11, 1].Text = "[-]";
            GridConfigure[12, 1].Text = "[-]";
            GridConfigure[13, 1].Text = "[-]";

            GridConfigure[14, 1].Text = "[mm]";
            GridConfigure[15, 1].Text = "[m/s^2]";
            GridConfigure[16, 1].Text = "[-]";
            GridConfigure[17, 1].Text = "[-]";

            GridConfigure[18, 1].Text = "[-]";
            GridConfigure[19, 1].Text = "[-]";
            GridConfigure[20, 1].Text = "[-]";
            GridConfigure[21, 1].Text = "[-]";
            GridConfigure[22, 1].Text = "[mm]";
            GridConfigure[23, 1].Text = "[mm]";
            GridConfigure[24, 1].Text = "[mm]";
            GridConfigure[25, 1].Text = "[mm]";
            GridConfigure[26, 1].Text = "[mm]";
            GridConfigure[27, 1].Text = "[mm]";
            GridConfigure[28, 1].Text = "[mm]";
            GridConfigure[29, 1].Text = "[mm]";
            GridConfigure[30, 1].Text = "[-]";
            GridConfigure[31, 1].Text = "[mm]";
            GridConfigure[32, 1].Text = "[mm]";
            GridConfigure[33, 1].Text = "[mm]";
            GridConfigure[34, 1].Text = "[mm]";
            GridConfigure[35, 1].Text = "[mm]";
            GridConfigure[36, 1].Text = "[mm]";
            GridConfigure[37, 1].Text = "[mm]";
            GridConfigure[38, 1].Text = "[mm]";
            GridConfigure[39, 1].Text = "[-]";
            GridConfigure[40, 1].Text = "[-]";

            GridConfigure[41, 1].Text = "[-]";
            GridConfigure[42, 1].Text = "[rps]";
            GridConfigure[43, 1].Text = "[rps]";
            GridConfigure[44, 1].Text = "[ms]";
            GridConfigure[45, 1].Text = "[ms]";
            GridConfigure[46, 1].Text = "[-]";
            GridConfigure[47, 1].Text = "[-]";

            GridConfigure[47, 1].Text = "[-]";
            GridConfigure[47, 1].Text = "[-]";
            GridConfigure[50, 1].Text = "[-]";
            GridConfigure[51, 1].Text = "[-]";
            GridConfigure[52, 1].Text = "[-]";
            GridConfigure[53, 1].Text = "[-]";
            GridConfigure[54, 1].Text = "[-]";
            GridConfigure[55, 1].Text = "[-]";
            GridConfigure[56, 1].Text = "[-]";

            GridConfigure[1, 3].Text = "[Job Settings] X Direct spot distance";
            GridConfigure[2, 3].Text = "[Job Settings] Y Direct spot distance";
            GridConfigure[3, 3].Text = "[Job Settings] All Facet X direct Offset";
            GridConfigure[4, 3].Text = "[Job Settings] After process, Polygon Mirror Run/Stop Setting";
            GridConfigure[5, 3].Text = "[Job Settings] Invert bmp data";
            GridConfigure[6, 3].Text = "[Job Settings] Amount of bitmap data to start jop";
            GridConfigure[7, 3].Text = "[Job Settings] Number of Dummy scanline";

            GridConfigure[8, 3].Text = "[Laser Configuration] Laser Mode Set";
            GridConfigure[9, 3].Text = "[Laser Configuration] Laser Seed Clock Frequency";
            GridConfigure[10, 3].Text = "[Laser Configuration] Laser beam pulse Frequency";
            GridConfigure[11, 3].Text = "[Laser Configuration] Pulse Width";
            GridConfigure[12, 3].Text = "[Laser Configuration] Bmp pixel width";
            GridConfigure[13, 3].Text = "[Laser Configuration] Do Not Change";
            GridConfigure[14, 3].Text = "[CrossScan Configuration] Stage Encoder Resolution";
            GridConfigure[15, 3].Text = "[CrossScan Configuration] Stage Max Accelation";
            GridConfigure[16, 3].Text = "[CrossScan Configuration] Use generate Enc signal & interface";
            GridConfigure[17, 3].Text = "[CrossScan Configuration] Stage movement direction";

            GridConfigure[18, 3].Text = "[Head Configuration] Serial Number";
            GridConfigure[19, 3].Text = "[Head Configuration] Scan line Ratio (bigger, scan line shorter)";
            GridConfigure[20, 3].Text = "[Head Configuration] Max scan line set";
            GridConfigure[21, 3].Text = "[Head Configuration] All scan line start pos set";
            GridConfigure[22, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigure[23, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigure[24, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigure[25, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigure[26, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigure[27, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigure[28, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigure[29, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigure[30, 3].Text = "[Head Configuration] Auto-calculate FacetFineDelayOffset(0,2,4,8)";
            GridConfigure[31, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigure[32, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigure[33, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigure[34, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigure[35, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigure[36, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigure[37, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigure[38, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigure[39, 3].Text = "[Head Configuration] Set Facet that first scan line";
            GridConfigure[40, 3].Text = "[Head Configuration] Start Facet change when new process";

            GridConfigure[41, 3].Text = "[Motor Configuration] Digital Type Set 1 or 0";
            GridConfigure[42, 3].Text = "[Motor Configuration] Default Setting 7.3";
            GridConfigure[43, 3].Text = "[Motor Configuration] Default Setting 30.0";
            GridConfigure[44, 3].Text = "[Motor Configuration] Motor SpeedUp TimeOut";
            GridConfigure[45, 3].Text = "[Motor Configuration] Exposure Timedelay after morot speedup";
            GridConfigure[46, 3].Text = "[Motor Configuration] Do Not Change";
            GridConfigure[47, 3].Text = "[Motor Configuration] Do Not Change";

            GridConfigure[48, 3].Text = "[Other Settings] Frequency of the interrups routine running";
            GridConfigure[49, 3].Text = "[Other Settings] Do Not Change ( Set to 3 )";
            GridConfigure[50, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigure[51, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigure[52, 3].Text = "[Other Settings] LowPassFilter of Job Start Signal";
            GridConfigure[53, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigure[54, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigure[55, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigure[56, 3].Text = "[Other Settings] Do Not Change ( Set to 60 )";
                       

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < GridConfigure.RowCount+1; j++)
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

        private void UpdateConfigureData(CSystemData_Scanner para,EScannerIndex Index = EScannerIndex.SCANNER1)
        {
            int num = (int)Index;
            //====================================================================================================
            // Config.ini
            
            GridConfigure[1, 2].Text  = string.Format("{0:f4}", para.Config[num].InScanResolution );
            GridConfigure[2, 2].Text  = string.Format("{0:f4}", para.Config[num].CrossScanResolution);
            GridConfigure[3, 2].Text  = string.Format("{0:f4}", para.Config[num].InScanOffset);
            GridConfigure[4, 2].Text  = string.Format("{0:f0}", para.Config[num].StopMotorBetweenJobs);
            GridConfigure[5, 2].Text  = string.Format("{0:f0}", para.Config[num].PixInvert);
            GridConfigure[6, 2].Text  = string.Format("{0:f0}", para.Config[num].JobStartBufferTime);
            GridConfigure[7, 2].Text  = string.Format("{0:f0}", para.Config[num].PrecedingBlankLines);

            GridConfigure[8, 2].Text  = string.Format("{0:f0}", para.Config[num].LaserOperationMode);
            GridConfigure[9, 2].Text  = string.Format("{0:f0}", para.Config[num].SeedClockFrequency);
            GridConfigure[10, 2].Text = string.Format("{0:f0}", para.Config[num].RepetitionRate);
            GridConfigure[11, 2].Text = string.Format("{0:f0}", para.Config[num].PulsePickWidth);
            GridConfigure[12, 2].Text = string.Format("{0:f0}", para.Config[num].PixelWidth);
            GridConfigure[13, 2].Text = string.Format("{0:f0}", para.Config[num].PulsePickAlgor);

            GridConfigure[14, 2].Text = string.Format("{0:f4}", para.Config[num].CrossScanEncoderResol);
            GridConfigure[15, 2].Text = string.Format("{0:f4}", para.Config[num].CrossScanMaxAccel);
            GridConfigure[16, 2].Text = string.Format("{0:f0}", para.Config[num].EnCarSig);
            GridConfigure[17, 2].Text = string.Format("{0:f0}", para.Config[num].SwapCarSig);

            GridConfigure[18, 2].Text = para.Config[num].SerialNumber;
            GridConfigure[19, 2].Text = string.Format("{0:f7}", para.Config[num].FThetaConstant);
            GridConfigure[20, 2].Text = string.Format("{0:f3}", para.Config[num].ExposeLineLength);
            GridConfigure[21, 2].Text = string.Format("{0:f0}", para.Config[num].EncoderIndexDelay);           
            GridConfigure[22, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelay0 );
            GridConfigure[23, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelay1 );
            GridConfigure[24, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelay2 );
            GridConfigure[25, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelay3 );
            GridConfigure[26, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelay4 );
            GridConfigure[27, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelay5 );
            GridConfigure[28, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelay6 );
            GridConfigure[29, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelay7 );
            GridConfigure[30, 2].Text = string.Format("{0:f0}", para.Config[num].InterleaveRatio);
            GridConfigure[31, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelayOffset0);
            GridConfigure[32, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelayOffset1);
            GridConfigure[33, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelayOffset2);
            GridConfigure[34, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelayOffset3);
            GridConfigure[35, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelayOffset4);
            GridConfigure[36, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelayOffset5);
            GridConfigure[37, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelayOffset6);
            GridConfigure[38, 2].Text = string.Format("{0:f4}", para.Config[num].FacetFineDelayOffset7);
            GridConfigure[39, 2].Text = string.Format("{0:f0}", para.Config[num].StartFacet);
            GridConfigure[40, 2].Text = string.Format("{0:f0}", para.Config[num].AutoIncrementStartFacet);
            
            GridConfigure[41, 2].Text = string.Format("{0:f0}", para.Config[num].MotorDriverType);
            GridConfigure[42, 2].Text = string.Format("{0:f1}", para.Config[num].MinMotorSpeed);
            GridConfigure[43, 2].Text = string.Format("{0:f1}", para.Config[num].MaxMotorSpeed);
            GridConfigure[44, 2].Text = string.Format("{0:f0}", para.Config[num].SyncWaitTime);
            GridConfigure[45, 2].Text = string.Format("{0:f0}", para.Config[num].MotorStableTime);
            GridConfigure[46, 2].Text = string.Format("{0:f0}", para.Config[num].ShaftEncoderPulseCount);
            GridConfigure[47, 2].Text = string.Format("{0:f0}", para.Config[num].SwapReferenceSignals);

            GridConfigure[48, 2].Text = string.Format("{0:f0}", para.Config[num].InterruptFreq);
            GridConfigure[49, 2].Text = string.Format("{0:f0}", para.Config[num].HWDebugSelection);
            GridConfigure[50, 2].Text = string.Format("{0:f0}", para.Config[num].ExpoDebugSelection);
            GridConfigure[51, 2].Text = string.Format("{0:f0}", para.Config[num].AutoRepeat);
            GridConfigure[52, 2].Text = string.Format("{0:f0}", para.Config[num].JobstartAutorepeat);
            GridConfigure[53, 2].Text = string.Format("{0:f0}", para.Config[num].TelnetTimeout);
            GridConfigure[54, 2].Text = string.Format("{0:f0}", para.Config[num].TelnetDebugTimeout);
            GridConfigure[55, 2].Text = string.Format("{0:f0}", para.Config[num].SyncSlaves);
            GridConfigure[56, 2].Text = string.Format("{0:f0}", para.Config[num].SyncTimeout);


        }

        private void BtnConfigureExit_Click(object sender, EventArgs e)
        {
            CMainFrame.HideJog();
            this.Close();
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
        

        private void BtnConfigSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("Save data?")) return;

            int num = (int)m_ScannerIndex;

            try
            {
                //====================================================================================================
                // Config.ini

                CMainFrame.DataManager.SystemData_Scan.Config[num].InScanResolution      = Convert.ToDouble(GridConfigure[1, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].CrossScanResolution   = Convert.ToDouble(GridConfigure[2, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].InScanOffset          = Convert.ToDouble(GridConfigure[3, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].StopMotorBetweenJobs  = Convert.ToInt32(GridConfigure[4, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].PixInvert             = Convert.ToInt32(GridConfigure[5, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].JobStartBufferTime    = Convert.ToInt32(GridConfigure[6, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].PrecedingBlankLines   = Convert.ToInt32(GridConfigure[7, 2].Text);

                CMainFrame.DataManager.SystemData_Scan.Config[num].LaserOperationMode    = Convert.ToInt32(GridConfigure[8, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].SeedClockFrequency    = Convert.ToDouble(GridConfigure[9, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].RepetitionRate        = Convert.ToDouble(GridConfigure[10, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].PulsePickWidth        = Convert.ToInt32(GridConfigure[11, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].PixelWidth            = Convert.ToInt32(GridConfigure[12, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].PulsePickAlgor        = Convert.ToInt32(GridConfigure[13, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].CrossScanEncoderResol = Convert.ToDouble(GridConfigure[14, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].CrossScanMaxAccel     = Convert.ToDouble(GridConfigure[15, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].EnCarSig              = Convert.ToInt32(GridConfigure[16, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].SwapCarSig            = Convert.ToInt32(GridConfigure[17, 2].Text);

                CMainFrame.DataManager.SystemData_Scan.Config[num].SerialNumber          = Convert.ToString(GridConfigure[18, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FThetaConstant        = Convert.ToDouble(GridConfigure[19, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].ExposeLineLength      = Convert.ToDouble(GridConfigure[20, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].EncoderIndexDelay     = Convert.ToInt32(GridConfigure[21, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelay0       = Convert.ToDouble(GridConfigure[22, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelay1       = Convert.ToDouble(GridConfigure[23, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelay2       = Convert.ToDouble(GridConfigure[24, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelay3       = Convert.ToDouble(GridConfigure[25, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelay4       = Convert.ToDouble(GridConfigure[26, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelay5       = Convert.ToDouble(GridConfigure[27, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelay6       = Convert.ToDouble(GridConfigure[28, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelay7       = Convert.ToDouble(GridConfigure[29, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].InterleaveRatio       = Convert.ToInt32(GridConfigure[30, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelayOffset0 = Convert.ToDouble(GridConfigure[31, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelayOffset1 = Convert.ToDouble(GridConfigure[32, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelayOffset2 = Convert.ToDouble(GridConfigure[33, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelayOffset3 = Convert.ToDouble(GridConfigure[34, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelayOffset4 = Convert.ToDouble(GridConfigure[35, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelayOffset5 = Convert.ToDouble(GridConfigure[36, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelayOffset6 = Convert.ToDouble(GridConfigure[37, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].FacetFineDelayOffset7 = Convert.ToDouble(GridConfigure[38, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].StartFacet            = Convert.ToInt32(GridConfigure[39, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].AutoIncrementStartFacet= Convert.ToInt32(GridConfigure[40, 2].Text);

                CMainFrame.DataManager.SystemData_Scan.Config[num].MotorDriverType       = Convert.ToInt32(GridConfigure[41, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].MinMotorSpeed         = Convert.ToDouble(GridConfigure[42, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].MaxMotorSpeed         = Convert.ToDouble(GridConfigure[43, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].SyncWaitTime          = Convert.ToInt32(GridConfigure[44, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].MotorStableTime       = Convert.ToInt32(GridConfigure[45, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].ShaftEncoderPulseCount= Convert.ToInt32(GridConfigure[46, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].SwapReferenceSignals  = Convert.ToInt32(GridConfigure[47, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].InterruptFreq         = Convert.ToInt32(GridConfigure[48, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].HWDebugSelection      = Convert.ToInt32(GridConfigure[49, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].ExpoDebugSelection    = Convert.ToInt32(GridConfigure[50, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].AutoRepeat            = Convert.ToInt32(GridConfigure[51, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].JobstartAutorepeat    = Convert.ToInt32(GridConfigure[52, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].TelnetTimeout         = Convert.ToInt32(GridConfigure[53, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].TelnetDebugTimeout    = Convert.ToInt32(GridConfigure[54, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].SyncSlaves            = Convert.ToInt32(GridConfigure[55, 2].Text);
                CMainFrame.DataManager.SystemData_Scan.Config[num].SyncTimeout           = Convert.ToInt32(GridConfigure[56, 2].Text);
                                

                // DB Save
                
                CMainFrame.LWDicer.SaveSystemData(null, null, null, null, null, CMainFrame.DataManager.SystemData_Scan, null);
            }
            catch
            {
                CMainFrame.DisplayMsg("Failed to save data");
            }

        }

        //private void btnWindowShow_Click(object sender, EventArgs e)
        //{
        //    //    var m_FormScanWindow = new FormScanWindow();
        //    //    m_FormScanWindow.ShowDialog();

        //    CMainFrame.LWDicer.m_MeScanner.ShowScanWindow();
        //}

        private void btnDrawForm_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeScanner.ShowScanWindow();
        }

        private void btnImageUpdate_Click(object sender, EventArgs e)
        {
            int fileStreamSize = 4092 * 1024;
            var dlg = new OpenFileDialog();
            dlg.Filter = "BMP(*.bmp,*.lse)|*.bmp;*.lse"; //(*.dxf, *.dwg)|*.dxf;*.dwg";
            dlg.InitialDirectory = (m_strLastDir == "") ? CMainFrame.DBInfo.ImageDataDir : m_strLastDir;
            dlg.FileName = m_strLastFile;
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var fileData = new FileInfo(dlg.FileName);
                m_strLastFile = fileData.Name;
                m_strLastDir = fileData.DirectoryName;
                if(fileData.Length < fileStreamSize)
                    CMainFrame.LWDicer.m_MeScanner.SendBitmap(dlg.FileName);
                else
                    CMainFrame.LWDicer.m_MeScanner.SendBitmap(dlg.FileName, true);
            }            
        }

        //private void btnImageChange_Click(object sender, EventArgs e)
        //{
        //    Bitmap sourceBitmap;
        //    Bitmap changeBitmap;
        //    string m_strLastFile = string.Empty;
        //    string changeName = string.Empty;

        //    var dlg = new OpenFileDialog();
        //    dlg.Filter = "BMP(*.bmp)|*.bmp";
        //    dlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
        //    if (dlg.ShowDialog() == DialogResult.OK)
        //    {
        //        var fileData = new FileInfo(dlg.FileName);
        //        m_strLastFile = fileData.Name;
        //        m_strLastDir = fileData.DirectoryName;

        //        // bmp file Load
        //        sourceBitmap = new Bitmap(dlg.FileName);
                
        //        // bmp expand by 8 (default)
        //        changeBitmap = CMainFrame.LWDicer.m_MeScanner.ExpandBmpFile(sourceBitmap);
                
        //        int insertPos = m_strLastFile.Length;
        //        changeName = m_strLastFile.Insert(insertPos-4, "_EX");
        //        // bmp Save
        //        if(changeBitmap != null)
        //            changeBitmap.Save(changeName,ImageFormat.Bmp);
        //    }
        //}


        private void btnDataUpdate_Click(object sender, EventArgs e)
        {
            // ini File 저장
            CMainFrame.LWDicer.m_MeScanner.SaveConfigPara("job");            

            FormPolygonSelectPara formSelect = new FormPolygonSelectPara();

            formSelect.ShowDialog();
            
        }

        private void FormPolygon_Activated(object sender, EventArgs e)
        {
        }

        private void FormPolygon_Shown(object sender, EventArgs e)
        {
            UpdateConfigureData(CMainFrame.DataManager.SystemData_Scan);
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

        }

      
        
        private void cbbOverlap_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnExportConfig_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("Write Config.ini to Disc ?")) return;

            string filename = string.Empty;
            SaveFileDialog IniSaveDlg = new SaveFileDialog();
            IniSaveDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            IniSaveDlg.Filter = "INI(*.ini)|*.ini";
            if (IniSaveDlg.ShowDialog() == DialogResult.OK)
            {
                filename = IniSaveDlg.FileName;
                CMainFrame.DataManager.ExportPolygonData(m_ScannerIndex, filename);
            }            
        }
        
        private void btnImportConfig_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("Read Config.ini from Disc?")) return;

            string filename = string.Empty;
            OpenFileDialog iniOpenDlg = new OpenFileDialog();
            iniOpenDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            iniOpenDlg.Filter = "INI(*.ini)|*.ini";
            if (iniOpenDlg.ShowDialog() == DialogResult.OK)
            {
                filename = iniOpenDlg.FileName;
                //CMainFrame.DataManager.ImportPolygonData(EPolygonPara.CONFIG);
                CMainFrame.DataManager.ImportPolygonData(m_ScannerIndex, filename);

                UpdateConfigureData(CMainFrame.DataManager.SystemData_Scan);
            }            
        }
        
        
        private void tabPageConfig_Enter(object sender, EventArgs e)
        {
            UpdateConfigureData(CMainFrame.DataManager.SystemData_Scan);
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
                CMainFrame.LWDicer.m_Vision.SaveImage(INSP_CAM, filename);
            }
        }

        private void btnVisionLive_Click(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_Vision == null) return;                       

            CMainFrame.LWDicer.m_Vision.LiveVideo(INSP_CAM);
        }

        private void btnVisionHalt_Click(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_Vision == null) return;
            
            CMainFrame.LWDicer.m_Vision.HaltVideo(INSP_CAM);
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

            Size sizeCam = CMainFrame.LWDicer.m_Vision.GetCameraPixelNum(INSP_CAM);

            double ratioX = (double) sizeCam.Width / (double)picVisionZoom.Width;
            double ratioY = (double)sizeCam.Height / (double)picVisionZoom.Height;

            Point ptStart = new Point(0, 0);
            Point ptEnd = new Point(0, 0);

            ptStart.X = (int)(ptMouseStartPos.X * ratioX);
            ptStart.Y = (int)(ptMouseStartPos.Y * ratioY);

            ptEnd.X = (int)(ptMouseEndPos.X * ratioX);
            ptEnd.Y = (int)(ptMouseEndPos.Y * ratioY);

            string textMsg;

            if (e.Button == MouseButtons.Left)
            {

                if (m_VisionMode == EVisionMode.MEASUREMENT)
                {
                    lenX *= CMainFrame.DataManager.SystemData_Align.PixelResolution[INSP_CAM];
                    lenY *= CMainFrame.DataManager.SystemData_Align.PixelResolution[INSP_CAM];

                    lengthPixel = Math.Sqrt(lenX * lenX + lenY * lenY);
                    textMsg = string.Format("{0:f2} um", lengthPixel);

                    //CMainFrame.LWDicer.m_Vision.ClearOverlay();
                    CMainFrame.LWDicer.m_Vision.DrawOverlayText(INSP_CAM, textMsg, ptEnd);
                    
                    CMainFrame.LWDicer.m_Vision.DrawOverlayLine(INSP_CAM, ptStart, ptEnd, System.Drawing.Color.Red);

                }
                if (m_VisionMode == EVisionMode.CALIBRATION)
                {
                    lengthPixel = Math.Sqrt(lenX * lenX + lenY * lenY);

                    string strCurrent = "", strModify = "";
                    if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
                    {
                        m_VisionMode = EVisionMode.MEASUREMENT;
                        btnCalibration.ForeColor = System.Drawing.Color.Black;
                        return;
                    }

                    lengthReal = Convert.ToDouble(strModify);
                    // Calibration Set
                    resolutionPixel = lengthReal / lengthPixel;
                    CMainFrame.DataManager.SystemData_Align.PixelResolution[INSP_CAM] = resolutionPixel;
                    CMainFrame.DataManager.SystemData_Align.PixelResolution[INSP_CAM] = resolutionPixel;

                    textMsg = string.Format("픽셀당 거리는 {0:F2} um입니다.", resolutionPixel);
                    CMainFrame.DisplayMsg(textMsg);

                    CMainFrame.LWDicer.SaveSystemData(null,null, null, null,CMainFrame.DataManager.SystemData_Align, null);

                    m_VisionMode = EVisionMode.MEASUREMENT;
                    btnCalibration.ForeColor = System.Drawing.Color.Black;
                }
            }            
        }

        private void picVisionZoom_Paint(object sender, PaintEventArgs e)
        {
            DrawMouseLine(e.Graphics);            
        }
        private void DrawMouseLine(Graphics g)
        {
           g.DrawLine(Pens.Red, ptMouseStartPos, ptMouseEndPos);
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            m_VisionMode = EVisionMode.CALIBRATION;
            
            CMainFrame.LWDicer.m_Vision.ClearOverlay();
            
            btnCalibration.ForeColor = System.Drawing.Color.Red;
        }

        private void btnMeasure_Click(object sender, EventArgs e)
        {
            m_VisionMode = EVisionMode.MEASUREMENT;
            btnCalibration.ForeColor = System.Drawing.Color.Black;
        }

        private void btnMeasureClear_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.ClearOverlay();
        }               
        

        private void btnLaserProcessMofStop_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeScanner.LaserProcessStop();
        }

        private void btnJogShow_Click(object sender, EventArgs e)
        {
            CMainFrame.DisplayJog();
        }

        private void btnPreCam_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(INSP_CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVisionZoom.Handle);
        }

        private void btnFineCam_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(PRE__CAM);
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(INSP_CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(FINE_CAM, picVisionZoom.Handle);

           // CMainFrame.LWDicer.m_MeStage.MoveCameraToFocusPosFine();
        }
        
        private void btnInpectCam_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(PRE__CAM);
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(INSP_CAM, picVisionZoom.Handle);

          //  CMainFrame.LWDicer.m_MeStage.MoveCameraToFocusPosInspect();
        }


        private void btnObjectDxf_Click(object sender, EventArgs e)
        {
            string filename = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "AutoCad files (*.dxf, *.dwg)|*.dxf;*.dwg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filename = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occurred: " + ex.Message);
                }
            }
            else
            {
                return;
            }
                        
            string extension = Path.GetExtension(filename);
            if (string.Compare(extension, ".dwg", true) == 0)
            {
                CadDrawing = DwgReader.Read(filename);
            }
            else
            {
                CadDrawing = DxfReader.Read(filename);
            }

            //for (int i = 0; i < windowDxf.Model.Entities.Count; i++)
            //{
            //    windowDxf.Model.Entities.RemoveAt(i);
            //}

            if(windowDxf.Model!= null)
                windowDxf.Model.Dispose();
            windowDxf.Model = CadDrawing;

            // Graphic Update
            windowDxf.GdiGraphics3D.CreateDrawables(windowDxf.Model);
            windowDxf.Invalidate();
            
        }

        private void LabelDefaultConfigFile_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            string filename = string.Empty;
            OpenFileDialog fileOpenDlg = new OpenFileDialog();
            fileOpenDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            fileOpenDlg.Filter = "INI(*.ini)|*.ini";

            if (fileOpenDlg.ShowDialog() == DialogResult.OK)
            {
                data.Text = fileOpenDlg.FileName;
                data.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void LabelMarkJobFile_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            string filename = string.Empty;
            OpenFileDialog fileOpenDlg = new OpenFileDialog();
            fileOpenDlg.InitialDirectory = CMainFrame.DBInfo.ScannerDataDir;
            fileOpenDlg.Filter = "INI(*.ini)|*.ini";

            if (fileOpenDlg.ShowDialog() == DialogResult.OK)
            {
                data.Text = fileOpenDlg.FileName;
                data.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void LabelMarkBmpFile_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            string filename = string.Empty;
            OpenFileDialog fileOpenDlg = new OpenFileDialog();
            fileOpenDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
            fileOpenDlg.Filter = "BMP(*.bmp,*.lse)|*.bmp;*.lse";

            if (fileOpenDlg.ShowDialog() == DialogResult.OK)
            {
                data.Text = fileOpenDlg.FileName;
                data.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void ComboScannerIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_ScannerIndex = EScannerIndex.SCANNER1 + ComboScannerIndex.SelectedIndex;
            UpdateConfigureData(CMainFrame.DataManager.SystemData_Scan,m_ScannerIndex);
        }

        private void BtnLaserProcess_Click(object sender, EventArgs e)
        {
            var dlg = new FormLaserProcessData();
            dlg.ShowDialog();
        }

        private void BtnLaserPattern_Click(object sender, EventArgs e)
        {
            var dlg = new FormLaserPatternData();
            dlg.ShowDialog();
        }

        private void BtnPatternDraw_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeScanner.ShowScanWindow();
        }

        private void gradientLabel4_Click(object sender, EventArgs e)
        {

        }
    }
}
