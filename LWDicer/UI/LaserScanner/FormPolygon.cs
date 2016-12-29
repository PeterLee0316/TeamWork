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
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;


//using static LWDicer.Layers.DEF_Scanner;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_MeStage;

using LWDicer.Layers;

namespace LWDicer.UI
{
    public partial class FormPolygon : Form
    {
        private Size OriginFormSize = new Size(0, 0);
        private Point ptMouseStartPos = new Point(0, 0);
        private Point ptMouseEndPos = new Point(0, 0);

        private EVisionMode eVisionMode;

        private string m_strLastDir;
        private string m_strLastFile;

        public FormPolygon()
        {
            InitializeComponent();

            InitTabPage();
            InitConfigureGrid();

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
                        this.tabPageScanner,
                        this.tabPageLaser,
                        this.tabPageVision,
                        this.tabPageConfig,
                        this.tabPageDrawing,
                     });
        }
        // Polygon Scanner Configure Data
        private void InitConfigureGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridConfigureNo1.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridConfigureNo1.Properties.RowHeaders = true;
            GridConfigureNo1.Properties.ColHeaders = true;

            // Column,Row 개수
            GridConfigureNo1.ColCount = 3;
            GridConfigureNo1.RowCount = 56;

            // Column 가로 크기설정
            GridConfigureNo1.ColWidths.SetSize(0, 200);
            GridConfigureNo1.ColWidths.SetSize(1, 80);
            GridConfigureNo1.ColWidths.SetSize(2, 100);
            GridConfigureNo1.ColWidths.SetSize(3, 500);

            for (int i = 0; i < GridConfigureNo1.RowCount+1; i++)
            {
                GridConfigureNo1.RowHeights[i] = 27;
            }

            for (int i = 1; i < GridConfigureNo1.RowCount+1; i++)
            {
                GridConfigureNo1[i , 1].BackColor = Color.FromArgb(230, 210, 255);
                GridConfigureNo1[i , 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            // Text Display
            GridConfigureNo1[0, 0].Text = "Parameter";
            GridConfigureNo1[0, 1].Text = "Unit";
            GridConfigureNo1[0, 2].Text = "Data";
            GridConfigureNo1[0, 3].Text = "Description";

            GridConfigureNo1[1, 0].Text = "InScanResolution";
            GridConfigureNo1[2, 0].Text = "CrossScanResolution";
            GridConfigureNo1[3, 0].Text = "InScanOffset";
            GridConfigureNo1[4, 0].Text = "StopMotorBetweenJobs";
            GridConfigureNo1[5, 0].Text = "PixInvert";
            GridConfigureNo1[6, 0].Text = "JobStartBufferTime";
            GridConfigureNo1[7, 0].Text = "PrecedingBlankLines";
            GridConfigureNo1[8, 0].Text = "LaserOperationMode";
            GridConfigureNo1[9, 0].Text = "SeedClockFrequency";
            GridConfigureNo1[10, 0].Text = "RepetitionRate";
            GridConfigureNo1[11, 0].Text = "PulsePickWidth";
            GridConfigureNo1[12, 0].Text = "PixelWidth";
            GridConfigureNo1[13, 0].Text = "PulsePickAlgor";

            GridConfigureNo1[14, 0].Text = "CrossScanEncoderResol";
            GridConfigureNo1[15, 0].Text = "CrossScanMaxAccel";
            GridConfigureNo1[16, 0].Text = "EnCarSig";
            GridConfigureNo1[17, 0].Text = "SwapCarSig";

            GridConfigureNo1[18, 0].Text = "SerialNumber";
            GridConfigureNo1[19, 0].Text = "FThetaConstant";
            GridConfigureNo1[20, 0].Text = "ExposeLineLength";
            GridConfigureNo1[21, 0].Text = "EncoderIndexDelay";
            GridConfigureNo1[22, 0].Text = "FacetFineDelay0";
            GridConfigureNo1[23, 0].Text = "FacetFineDelay1";
            GridConfigureNo1[24, 0].Text = "FacetFineDelay2";
            GridConfigureNo1[25, 0].Text = "FacetFineDelay3";
            GridConfigureNo1[26, 0].Text = "FacetFineDelay4";
            GridConfigureNo1[27, 0].Text = "FacetFineDelay5";
            GridConfigureNo1[28, 0].Text = "FacetFineDelay6";
            GridConfigureNo1[29, 0].Text = "FacetFineDelay7";
            GridConfigureNo1[30, 0].Text = "InterLeaveRatio";
            GridConfigureNo1[31, 0].Text = "FacetFineDelayOffset0";
            GridConfigureNo1[32, 0].Text = "FacetFineDelayOffset1";
            GridConfigureNo1[33, 0].Text = "FacetFineDelayOffset2";
            GridConfigureNo1[34, 0].Text = "FacetFineDelayOffset3";
            GridConfigureNo1[35, 0].Text = "FacetFineDelayOffset4";
            GridConfigureNo1[36, 0].Text = "FacetFineDelayOffset5";
            GridConfigureNo1[37, 0].Text = "FacetFineDelayOffset6";
            GridConfigureNo1[38, 0].Text = "FacetFineDelayOffset7";
            GridConfigureNo1[39, 0].Text = "StartFacet";
            GridConfigureNo1[40, 0].Text = "AutoIncrementStartFacet";

            GridConfigureNo1[41, 0].Text = "MotorDriverType";
            GridConfigureNo1[42, 0].Text = "MinMotorSpeed";
            GridConfigureNo1[43, 0].Text = "MaxMotorSpeed";
            GridConfigureNo1[44, 0].Text = "SyncWaitTime";
            GridConfigureNo1[45, 0].Text = "MotorStableTime";
            GridConfigureNo1[46, 0].Text = "ShaftEncoderPulseCount";
            GridConfigureNo1[47, 0].Text = "SwapReferenceSignals";

            GridConfigureNo1[48, 0].Text = "InterruptFreq";
            GridConfigureNo1[49, 0].Text = "HWDebugSelection";
            GridConfigureNo1[50, 0].Text = "ExpoDebugSelection";
            GridConfigureNo1[51, 0].Text = "AutoRepeat";
            GridConfigureNo1[52, 0].Text = "JobstartAutorepeat";
            GridConfigureNo1[53, 0].Text = "TelnetTimeout";
            GridConfigureNo1[54, 0].Text = "TelnetDebugTimeout";
            GridConfigureNo1[55, 0].Text = "SyncSlaves";
            GridConfigureNo1[56, 0].Text = "SyncTimeout";

            GridConfigureNo1[1, 1].Text = "[mm]";
            GridConfigureNo1[2, 1].Text = "[mm]";
            GridConfigureNo1[3, 1].Text = "[mm]";
            GridConfigureNo1[4, 1].Text = "[-]";
            GridConfigureNo1[5, 1].Text = "[-]";
            GridConfigureNo1[6, 1].Text = "[sec]";
            GridConfigureNo1[7, 1].Text = "[-]";

            GridConfigureNo1[8, 1].Text = "[-]";
            GridConfigureNo1[9, 1].Text = "[kHz]";
            GridConfigureNo1[10, 1].Text = "[kHz]";
            GridConfigureNo1[11, 1].Text = "[-]";
            GridConfigureNo1[12, 1].Text = "[-]";
            GridConfigureNo1[13, 1].Text = "[-]";

            GridConfigureNo1[14, 1].Text = "[mm]";
            GridConfigureNo1[15, 1].Text = "[m/s^2]";
            GridConfigureNo1[16, 1].Text = "[-]";
            GridConfigureNo1[17, 1].Text = "[-]";

            GridConfigureNo1[18, 1].Text = "[-]";
            GridConfigureNo1[19, 1].Text = "[-]";
            GridConfigureNo1[20, 1].Text = "[-]";
            GridConfigureNo1[21, 1].Text = "[-]";
            GridConfigureNo1[22, 1].Text = "[mm]";
            GridConfigureNo1[23, 1].Text = "[mm]";
            GridConfigureNo1[24, 1].Text = "[mm]";
            GridConfigureNo1[25, 1].Text = "[mm]";
            GridConfigureNo1[26, 1].Text = "[mm]";
            GridConfigureNo1[27, 1].Text = "[mm]";
            GridConfigureNo1[28, 1].Text = "[mm]";
            GridConfigureNo1[29, 1].Text = "[mm]";
            GridConfigureNo1[30, 1].Text = "[-]";
            GridConfigureNo1[31, 1].Text = "[mm]";
            GridConfigureNo1[32, 1].Text = "[mm]";
            GridConfigureNo1[33, 1].Text = "[mm]";
            GridConfigureNo1[34, 1].Text = "[mm]";
            GridConfigureNo1[35, 1].Text = "[mm]";
            GridConfigureNo1[36, 1].Text = "[mm]";
            GridConfigureNo1[37, 1].Text = "[mm]";
            GridConfigureNo1[38, 1].Text = "[mm]";
            GridConfigureNo1[39, 1].Text = "[-]";
            GridConfigureNo1[40, 1].Text = "[-]";

            GridConfigureNo1[41, 1].Text = "[-]";
            GridConfigureNo1[42, 1].Text = "[rps]";
            GridConfigureNo1[43, 1].Text = "[rps]";
            GridConfigureNo1[44, 1].Text = "[ms]";
            GridConfigureNo1[45, 1].Text = "[ms]";
            GridConfigureNo1[46, 1].Text = "[-]";
            GridConfigureNo1[47, 1].Text = "[-]";

            GridConfigureNo1[47, 1].Text = "[-]";
            GridConfigureNo1[47, 1].Text = "[-]";
            GridConfigureNo1[50, 1].Text = "[-]";
            GridConfigureNo1[51, 1].Text = "[-]";
            GridConfigureNo1[52, 1].Text = "[-]";
            GridConfigureNo1[53, 1].Text = "[-]";
            GridConfigureNo1[54, 1].Text = "[-]";
            GridConfigureNo1[55, 1].Text = "[-]";
            GridConfigureNo1[56, 1].Text = "[-]";

            GridConfigureNo1[1, 3].Text = "[Job Settings] X Direct spot distance";
            GridConfigureNo1[2, 3].Text = "[Job Settings] Y Direct spot distance";
            GridConfigureNo1[3, 3].Text = "[Job Settings] All Facet X direct Offset";
            GridConfigureNo1[4, 3].Text = "[Job Settings] After process, Polygon Mirror Run/Stop Setting";
            GridConfigureNo1[5, 3].Text = "[Job Settings] Invert bmp data";
            GridConfigureNo1[6, 3].Text = "[Job Settings] Amount of bitmap data to start jop";
            GridConfigureNo1[7, 3].Text = "[Job Settings] Number of Dummy scanline";

            GridConfigureNo1[8, 3].Text = "[Laser Configuration] Laser Mode Set";
            GridConfigureNo1[9, 3].Text = "[Laser Configuration] Laser Seed Clock Frequency";
            GridConfigureNo1[10, 3].Text = "[Laser Configuration] Laser beam pulse Frequency";
            GridConfigureNo1[11, 3].Text = "[Laser Configuration] Pulse Width";
            GridConfigureNo1[12, 3].Text = "[Laser Configuration] Bmp pixel width";
            GridConfigureNo1[13, 3].Text = "[Laser Configuration] Do Not Change";
            GridConfigureNo1[14, 3].Text = "[CrossScan Configuration] Stage Encoder Resolution";
            GridConfigureNo1[15, 3].Text = "[CrossScan Configuration] Stage Max Accelation";
            GridConfigureNo1[16, 3].Text = "[CrossScan Configuration] Use generate Enc signal & interface";
            GridConfigureNo1[17, 3].Text = "[CrossScan Configuration] Stage movement direction";

            GridConfigureNo1[18, 3].Text = "[Head Configuration] Serial Number";
            GridConfigureNo1[19, 3].Text = "[Head Configuration] Scan line Ratio (bigger, scan line shorter)";
            GridConfigureNo1[20, 3].Text = "[Head Configuration] Max scan line set";
            GridConfigureNo1[21, 3].Text = "[Head Configuration] All scan line start pos set";
            GridConfigureNo1[22, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigureNo1[23, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigureNo1[24, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigureNo1[25, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigureNo1[26, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigureNo1[27, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigureNo1[28, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigureNo1[29, 3].Text = "[Head Configuration] Each scanline start pos delay (Only +)";
            GridConfigureNo1[30, 3].Text = "[Head Configuration] Auto-calculate FacetFineDelayOffset(0,2,4,8)";
            GridConfigureNo1[31, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigureNo1[32, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigureNo1[33, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigureNo1[34, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigureNo1[35, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigureNo1[36, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigureNo1[37, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigureNo1[38, 3].Text = "[Head Configuration] Each scanline start pos delay offset (Only +)";
            GridConfigureNo1[39, 3].Text = "[Head Configuration] Set Facet that first scan line";
            GridConfigureNo1[40, 3].Text = "[Head Configuration] Start Facet change when new process";

            GridConfigureNo1[41, 3].Text = "[Motor Configuration] Digital Type Set 1 or 0";
            GridConfigureNo1[42, 3].Text = "[Motor Configuration] Default Setting 7.3";
            GridConfigureNo1[43, 3].Text = "[Motor Configuration] Default Setting 30.0";
            GridConfigureNo1[44, 3].Text = "[Motor Configuration] Motor SpeedUp TimeOut";
            GridConfigureNo1[45, 3].Text = "[Motor Configuration] Exposure Timedelay after morot speedup";
            GridConfigureNo1[46, 3].Text = "[Motor Configuration] Do Not Change";
            GridConfigureNo1[47, 3].Text = "[Motor Configuration] Do Not Change";

            GridConfigureNo1[48, 3].Text = "[Other Settings] Frequency of the interrups routine running";
            GridConfigureNo1[49, 3].Text = "[Other Settings] Do Not Change ( Set to 3 )";
            GridConfigureNo1[50, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigureNo1[51, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigureNo1[52, 3].Text = "[Other Settings] LowPassFilter of Job Start Signal";
            GridConfigureNo1[53, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigureNo1[54, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigureNo1[55, 3].Text = "[Other Settings] Do Not Change ( Set to 0 )";
            GridConfigureNo1[56, 3].Text = "[Other Settings] Do Not Change ( Set to 60 )";
                       

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < GridConfigureNo1.RowCount+1; j++)
                {
                    // Font Style - Bold
                    GridConfigureNo1[j, i].Font.Bold = true;

                    GridConfigureNo1[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i != 3)
                    {
                        GridConfigureNo1[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }
                }
            }

            GridConfigureNo1.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridConfigureNo1.ResizeColsBehavior = 0;
            GridConfigureNo1.ResizeRowsBehavior = 0;

            //GetPolygonPara(scannerIndex);

            // Grid Display Update
            GridConfigureNo1.Refresh();
        }


        private void UpdateConfigureData(CSystemData_Scanner para)
        {
            //====================================================================================================
            // Config.ini
            
            GridConfigureNo1[1, 2].Text  = string.Format("{0:f4}", para.InScanResolution );
            GridConfigureNo1[2, 2].Text  = string.Format("{0:f4}", para.CrossScanResolution);
            GridConfigureNo1[3, 2].Text  = string.Format("{0:f4}", para.InScanOffset);
            GridConfigureNo1[4, 2].Text  = string.Format("{0:f0}", para.StopMotorBetweenJobs);
            GridConfigureNo1[5, 2].Text  = string.Format("{0:f0}", para.PixInvert);
            GridConfigureNo1[6, 2].Text  = string.Format("{0:f0}", para.JobStartBufferTime);
            GridConfigureNo1[7, 2].Text  = string.Format("{0:f0}", para.PrecedingBlankLines);

            GridConfigureNo1[8, 2].Text  = string.Format("{0:f0}", para.LaserOperationMode);
            GridConfigureNo1[9, 2].Text  = string.Format("{0:f0}", para.SeedClockFrequency);
            GridConfigureNo1[10, 2].Text = string.Format("{0:f0}", para.RepetitionRate);
            GridConfigureNo1[11, 2].Text = string.Format("{0:f0}", para.PulsePickWidth);
            GridConfigureNo1[12, 2].Text = string.Format("{0:f0}", para.PixelWidth);
            GridConfigureNo1[13, 2].Text = string.Format("{0:f0}", para.PulsePickAlgor);

            GridConfigureNo1[14, 2].Text = string.Format("{0:f4}", para.CrossScanEncoderResol);
            GridConfigureNo1[15, 2].Text = string.Format("{0:f4}", para.CrossScanMaxAccel);
            GridConfigureNo1[16, 2].Text = string.Format("{0:f0}", para.EnCarSig);
            GridConfigureNo1[17, 2].Text = string.Format("{0:f0}", para.SwapCarSig);

            GridConfigureNo1[18, 2].Text = para.SerialNumber;
            GridConfigureNo1[19, 2].Text = string.Format("{0:f7}", para.FThetaConstant);
            GridConfigureNo1[20, 2].Text = string.Format("{0:f3}", para.ExposeLineLength);
            GridConfigureNo1[21, 2].Text = string.Format("{0:f0}", para.EncoderIndexDelay);           
            GridConfigureNo1[22, 2].Text = string.Format("{0:f4}", para.FacetFineDelay0 );
            GridConfigureNo1[23, 2].Text = string.Format("{0:f4}", para.FacetFineDelay1 );
            GridConfigureNo1[24, 2].Text = string.Format("{0:f4}", para.FacetFineDelay2 );
            GridConfigureNo1[25, 2].Text = string.Format("{0:f4}", para.FacetFineDelay3 );
            GridConfigureNo1[26, 2].Text = string.Format("{0:f4}", para.FacetFineDelay4 );
            GridConfigureNo1[27, 2].Text = string.Format("{0:f4}", para.FacetFineDelay5 );
            GridConfigureNo1[28, 2].Text = string.Format("{0:f4}", para.FacetFineDelay6 );
            GridConfigureNo1[29, 2].Text = string.Format("{0:f4}", para.FacetFineDelay7 );
            GridConfigureNo1[30, 2].Text = string.Format("{0:f0}", para.InterleaveRatio);
            GridConfigureNo1[31, 2].Text = string.Format("{0:f4}", para.FacetFineDelayOffset0);
            GridConfigureNo1[32, 2].Text = string.Format("{0:f4}", para.FacetFineDelayOffset1);
            GridConfigureNo1[33, 2].Text = string.Format("{0:f4}", para.FacetFineDelayOffset2);
            GridConfigureNo1[34, 2].Text = string.Format("{0:f4}", para.FacetFineDelayOffset3);
            GridConfigureNo1[35, 2].Text = string.Format("{0:f4}", para.FacetFineDelayOffset4);
            GridConfigureNo1[36, 2].Text = string.Format("{0:f4}", para.FacetFineDelayOffset5);
            GridConfigureNo1[37, 2].Text = string.Format("{0:f4}", para.FacetFineDelayOffset6);
            GridConfigureNo1[38, 2].Text = string.Format("{0:f4}", para.FacetFineDelayOffset7);
            GridConfigureNo1[39, 2].Text = string.Format("{0:f0}", para.StartFacet);
            GridConfigureNo1[40, 2].Text = string.Format("{0:f0}", para.AutoIncrementStartFacet);
            
            GridConfigureNo1[41, 2].Text = string.Format("{0:f0}", para.MotorDriverType);
            GridConfigureNo1[42, 2].Text = string.Format("{0:f1}", para.MinMotorSpeed);
            GridConfigureNo1[43, 2].Text = string.Format("{0:f1}", para.MaxMotorSpeed);
            GridConfigureNo1[44, 2].Text = string.Format("{0:f0}", para.SyncWaitTime);
            GridConfigureNo1[45, 2].Text = string.Format("{0:f0}", para.MotorStableTime);
            GridConfigureNo1[46, 2].Text = string.Format("{0:f0}", para.ShaftEncoderPulseCount);
            GridConfigureNo1[47, 2].Text = string.Format("{0:f0}", para.SwapReferenceSignals);

            GridConfigureNo1[48, 2].Text = string.Format("{0:f0}", para.InterruptFreq);
            GridConfigureNo1[49, 2].Text = string.Format("{0:f0}", para.HWDebugSelection);
            GridConfigureNo1[50, 2].Text = string.Format("{0:f0}", para.ExpoDebugSelection);
            GridConfigureNo1[51, 2].Text = string.Format("{0:f0}", para.AutoRepeat);
            GridConfigureNo1[52, 2].Text = string.Format("{0:f0}", para.JobstartAutorepeat);
            GridConfigureNo1[53, 2].Text = string.Format("{0:f0}", para.TelnetTimeout);
            GridConfigureNo1[54, 2].Text = string.Format("{0:f0}", para.TelnetDebugTimeout);
            GridConfigureNo1[55, 2].Text = string.Format("{0:f0}", para.SyncSlaves);
            GridConfigureNo1[56, 2].Text = string.Format("{0:f0}", para.SyncTimeout);


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

            strCurrent = GridConfigureNo1[nRow, nCol].Text;
            
            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridConfigureNo1[nRow, nCol].Text = strModify;
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
            if (!CMainFrame.DisplayMsg("Save data?")) return;

            try
            {
                //====================================================================================================
                // Config.ini

                CMainFrame.DataManager.ModelData.ScanData.InScanResolution      = Convert.ToDouble(GridConfigureNo1[1, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.CrossScanResolution   = Convert.ToDouble(GridConfigureNo1[2, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.InScanOffset          = Convert.ToDouble(GridConfigureNo1[3, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.StopMotorBetweenJobs  = Convert.ToInt32(GridConfigureNo1[4, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.PixInvert             = Convert.ToInt32(GridConfigureNo1[5, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.JobStartBufferTime    = Convert.ToInt32(GridConfigureNo1[6, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.PrecedingBlankLines   = Convert.ToInt32(GridConfigureNo1[7, 2].Text);

                CMainFrame.DataManager.ModelData.ScanData.LaserOperationMode    = Convert.ToInt32(GridConfigureNo1[8, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.SeedClockFrequency    = Convert.ToDouble(GridConfigureNo1[9, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.RepetitionRate        = Convert.ToDouble(GridConfigureNo1[10, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.PulsePickWidth        = Convert.ToInt32(GridConfigureNo1[11, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.PixelWidth            = Convert.ToInt32(GridConfigureNo1[12, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.PulsePickAlgor        = Convert.ToInt32(GridConfigureNo1[13, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.CrossScanEncoderResol = Convert.ToDouble(GridConfigureNo1[14, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.CrossScanMaxAccel     = Convert.ToDouble(GridConfigureNo1[15, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.EnCarSig              = Convert.ToInt32(GridConfigureNo1[16, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.SwapCarSig            = Convert.ToInt32(GridConfigureNo1[17, 2].Text);

                CMainFrame.DataManager.ModelData.ScanData.SerialNumber          = Convert.ToString(GridConfigureNo1[18, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FThetaConstant        = Convert.ToDouble(GridConfigureNo1[19, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.ExposeLineLength      = Convert.ToDouble(GridConfigureNo1[20, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.EncoderIndexDelay     = Convert.ToInt32(GridConfigureNo1[21, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelay0       = Convert.ToDouble(GridConfigureNo1[22, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelay1       = Convert.ToDouble(GridConfigureNo1[23, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelay2       = Convert.ToDouble(GridConfigureNo1[24, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelay3       = Convert.ToDouble(GridConfigureNo1[25, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelay4       = Convert.ToDouble(GridConfigureNo1[26, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelay5       = Convert.ToDouble(GridConfigureNo1[27, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelay6       = Convert.ToDouble(GridConfigureNo1[28, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelay7       = Convert.ToDouble(GridConfigureNo1[29, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.InterleaveRatio       = Convert.ToInt32(GridConfigureNo1[30, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset0 = Convert.ToDouble(GridConfigureNo1[31, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset1 = Convert.ToDouble(GridConfigureNo1[32, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset2 = Convert.ToDouble(GridConfigureNo1[33, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset3 = Convert.ToDouble(GridConfigureNo1[34, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset4 = Convert.ToDouble(GridConfigureNo1[35, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset5 = Convert.ToDouble(GridConfigureNo1[36, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset6 = Convert.ToDouble(GridConfigureNo1[37, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.FacetFineDelayOffset7 = Convert.ToDouble(GridConfigureNo1[38, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.StartFacet            = Convert.ToInt32(GridConfigureNo1[39, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.AutoIncrementStartFacet= Convert.ToInt32(GridConfigureNo1[40, 2].Text);

                CMainFrame.DataManager.ModelData.ScanData.MotorDriverType       = Convert.ToInt32(GridConfigureNo1[41, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.MinMotorSpeed         = Convert.ToDouble(GridConfigureNo1[42, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.MaxMotorSpeed         = Convert.ToDouble(GridConfigureNo1[43, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.SyncWaitTime          = Convert.ToInt32(GridConfigureNo1[44, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.MotorStableTime       = Convert.ToInt32(GridConfigureNo1[45, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.ShaftEncoderPulseCount= Convert.ToInt32(GridConfigureNo1[46, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.SwapReferenceSignals  = Convert.ToInt32(GridConfigureNo1[47, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.InterruptFreq         = Convert.ToInt32(GridConfigureNo1[48, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.HWDebugSelection      = Convert.ToInt32(GridConfigureNo1[49, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.ExpoDebugSelection    = Convert.ToInt32(GridConfigureNo1[50, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.AutoRepeat            = Convert.ToInt32(GridConfigureNo1[51, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.JobstartAutorepeat    = Convert.ToInt32(GridConfigureNo1[52, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.TelnetTimeout         = Convert.ToInt32(GridConfigureNo1[53, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.TelnetDebugTimeout    = Convert.ToInt32(GridConfigureNo1[54, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.SyncSlaves            = Convert.ToInt32(GridConfigureNo1[55, 2].Text);
                CMainFrame.DataManager.ModelData.ScanData.SyncTimeout           = Convert.ToInt32(GridConfigureNo1[56, 2].Text);
                                

                // DB Save
                CMainFrame.LWDicer.SaveModelData(CMainFrame.DataManager.ModelData);
            }
            catch
            {
                CMainFrame.DisplayMsg("Failed to save data");
            }

        }

        private void tabPolygonForm_Click(object sender, EventArgs e)
        {
           // UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
           // DisplayProcessData();
        }

        private void tabPolygonForm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        

        //private void btnWindowShow_Click(object sender, EventArgs e)
        //{
        //    //    var m_FormScanWindow = new FormScanWindow();
        //    //    m_FormScanWindow.ShowDialog();

        //    CMainFrame.LWDicer.m_MeScanner.ShowScanWindow();
        //}

        
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
            //UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
        }

        private void FormPolygon_Shown(object sender, EventArgs e)
        {
            UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
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
            if (!CMainFrame.DisplayMsg("프로세스 데이터를 저장하시겠습니까 ?")) return;


            UpdateProcessData();


            CMainFrame.LWDicer.SaveModelData(CMainFrame.DataManager.ModelData);
        }

        private void btnProcessDataCreate_Click(object sender, EventArgs e)
        {       
            if (!CMainFrame.DisplayMsg("비트맵 파일을 생성하시겠습니까 ?")) return;

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
            CPos_XY startLine = new CPos_XY();
            CPos_XY endLine = new CPos_XY();
            

              
            // 타원의 Y축 상부 시작 위치
            double Y = cutBeginPos;
            int bmpFileNameNum = 0;

            do
            {
                ellipseX1 = ellipseMove - (float)(Math.Sqrt(A * A * B * B - A * A * Y * Y) / B);
                ellipseX2 = ellipseMove + (float)(Math.Sqrt(A * A * B * B - A * A * Y * Y) / B);

                if (ellipseX1 < 0) ellipseX1 = 0;
                if (ellipseX2 > CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize) ellipseX2 = CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize;

                startLine.dX = ellipseX1;
                endLine.dX   = ellipseX2;
                startLine.dY = endLine.dY = 0;

                for (int i = 0; i < CMainFrame.DataManager.ModelData.ProcData.ProcessLineNum; i++)
                {
                    startLine.dY = endLine.dY = CMainFrame.DataManager.ModelData.ScanData.CrossScanResolution * i;

                    CMainFrame.LWDicer.m_MeScanner.AddLine(startLine, endLine);
                }

                string filename = string.Format("{0:s}{1:s}{2:F0}.bmp", CMainFrame.DBInfo.ImageDataDir, "CutAxisX_", bmpFileNameNum+1);

                CMainFrame.LWDicer.m_MeScanner.SetSizeBmp();
                CMainFrame.LWDicer.m_MeScanner.ConvertBmpFile(filename);
                
                CMainFrame.LWDicer.m_MeScanner.DeleteAllObject();
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

                startLine.dX = ellipseX1;
                endLine.dX = ellipseX2;
                startLine.dY = endLine.dY = 0;

                for (int i = 0; i < CMainFrame.DataManager.ModelData.ProcData.ProcessLineNum; i++)
                {
                    startLine.dY = endLine.dY = CMainFrame.DataManager.ModelData.ScanData.CrossScanResolution * i;
                    CMainFrame.LWDicer.m_MeScanner.AddLine(startLine, endLine);
                }

                string filename = string.Format("{0:s}{1:s}{2:F0}.bmp", CMainFrame.DBInfo.ImageDataDir, "CutAxisY_", bmpFileNameNum+1);

                CMainFrame.LWDicer.m_MeScanner.SetSizeBmp();
                CMainFrame.LWDicer.m_MeScanner.ConvertBmpFile(filename);

                CMainFrame.LWDicer.m_MeScanner.DeleteAllObject();
                //m_ScanManager.DeleteAllObject();
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
            lblResolutionX.Text     = string.Format("{0:F4}", CMainFrame.DataManager.ModelData.ScanData.InScanResolution);
            lblResolutionY.Text     = string.Format("{0:F4}", CMainFrame.DataManager.ModelData.ScanData.CrossScanResolution);
            lblOffsetX.Text         = string.Format("{0:F4}", CMainFrame.DataManager.ModelData.ScanData.InScanOffset);
            lblRepetitionLaser.Text = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ScanData.RepetitionRate);
            cbbOverlap.Text         = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ScanData.InterleaveRatio);

            lblWaferSize.Text       = string.Format("{0:F1}", CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize);
            lblDiePitchX.Text       = string.Format("{0:F4}", CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeX);
            lblDiePitchY.Text       = string.Format("{0:F4}", CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeY);
            lblMarginWidth.Text     = string.Format("{0:F1}", CMainFrame.DataManager.ModelData.ProcData.MarginWidth);
            lblMarginHeight.Text    = string.Format("{0:F1}", CMainFrame.DataManager.ModelData.ProcData.MarginHeight);
            lblOverlapCount.Text    = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.ProcessLineNum);
                        

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
                CMainFrame.DataManager.ExportPolygonData(EPolygonPara.CONFIG, filename);
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
                CMainFrame.DataManager.ImportPolygonData(EPolygonPara.CONFIG, filename);

                UpdateConfigureData(CMainFrame.DataManager.ModelData.ScanData);
            }            
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

        private void BtnModelCamera_Click(object sender, EventArgs e)
        {

        }

        private void cbbSelectCam_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void lblCsnStartOffset_Click(object sender, EventArgs e)
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

        private void lblCsnEndOffset_Click(object sender, EventArgs e)
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

                if (eVisionMode == EVisionMode.MEASUREMENT)
                {
                    lenX *= CMainFrame.DataManager.SystemData_Align.PixelResolution[INSP_CAM];
                    lenY *= CMainFrame.DataManager.SystemData_Align.PixelResolution[INSP_CAM];

                    lengthPixel = Math.Sqrt(lenX * lenX + lenY * lenY);
                    textMsg = string.Format("{0:f2} um", lengthPixel);

                    //CMainFrame.LWDicer.m_Vision.ClearOverlay();
                    CMainFrame.LWDicer.m_Vision.DrawOverlayText(INSP_CAM, textMsg, ptEnd);
                    
                    CMainFrame.LWDicer.m_Vision.DrawOverlayLine(INSP_CAM, ptStart, ptEnd, Color.Red);

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
                    CMainFrame.DataManager.SystemData_Align.PixelResolution[INSP_CAM] = resolutionPixel;
                    CMainFrame.DataManager.SystemData_Align.PixelResolution[INSP_CAM] = resolutionPixel;

                    textMsg = string.Format("픽셀당 거리는 {0:F2} um입니다.", resolutionPixel);
                    CMainFrame.DisplayMsg(textMsg);

                    CMainFrame.LWDicer.SaveSystemData(null,null, null, null,CMainFrame.DataManager.SystemData_Align, null);

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
           g.DrawLine(Pens.Red, ptMouseStartPos, ptMouseEndPos);
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

        private void btnMeasureClear_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.ClearOverlay();
        }
               

        private void btnPatternProcessSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("프로세스 데이터를 저장하시겠습니까 ?")) return;

            CMainFrame.LWDicer.SaveModelData(CMainFrame.DataManager.ModelData);

            DisplayProcessData();

        }

        private void btnLaserProcessStep1_Click(object sender, EventArgs e)
        {
            int iResult = 0;
            CMainFrame.DataManager.ModelData.ProcData.ProcessStop = false;
            var task = Task<int>.Run(() => CMainFrame.LWDicer.m_ctrlStage1.RunLaserProcess());
        }

        private void btnLaserProcessStep2_Click(object sender, EventArgs e)
        {

        }

        private void tabPageProcess266_Click(object sender, EventArgs e)
        {

        }

        private void lblProcessInterval_Click(object sender, EventArgs e)
        {

        }

        private void btnLaserProcessStop_Click(object sender, EventArgs e)
        {
            CMainFrame.DataManager.ModelData.ProcData.ProcessStop = true;
        }

        private void lblProcessCount_Click(object sender, EventArgs e)
        {

        }

        private void btnLaserProcessMofStop_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_MeScanner.LaserProcessStop();
        }

        private void btnJogShow_Click(object sender, EventArgs e)
        {
            CMainFrame.DisplayJog();
        }

        private void lblMousePos_Click(object sender, EventArgs e)
        {

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

        private void btnMoveToLaser_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative((int)EStagePos.VISION_LASER_GAP);
        }

        private void btnMoveToVision_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_ctrlStage1.MoveStageRelative((int)EStagePos.VISION_LASER_GAP, false);
        }

        private void tabPageTrueRaster_Click(object sender, EventArgs e)
        {

        }
    }
}
