using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Specialized;

using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;


using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Vision;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_MeStage;
using static Core.Layers.DEF_CtrlStage;

using Core.Layers;


namespace Core.UI
{
    public partial class FormLaserPatternData : Form
    {
        string[] strOP = new string[(int)ELaserOperation.MAX];

        private int selectedSequenceNum;
        private CLaserProcessData LaserProcessData;
        private EScannerIndex m_ScannerIndex;

        public FormLaserPatternData()
        {
            InitializeComponent();

            m_ScannerIndex = EScannerIndex.SCANNER1;

            // ComboBox Init
            for (int i = 0; i < (int)EScannerIndex.MAX; i++)
            {
                ComboScannerIndex.Items.Add(EScannerIndex.SCANNER1 + i);
            }
            ComboScannerIndex.SelectedIndex = 0;

            // Process Data Copy
            LaserProcessData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData.LaserProcessData);

            DisplayProcessData();

            this.Text = $"Laser Process Data [ Current Model : {CMainFrame.DataManager.ModelData.Name} ]";
            
        }

        private void FormLaserProcessData_Load(object sender, EventArgs e)
        {

        }
        
        
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCleanerData_Load(object sender, EventArgs e)
        {

        }
        
        private void LabelData_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }
        
        private void BtnLoadFrom_Click(object sender, EventArgs e)
        {
            DisplayProcessData();
        }
        
        private void LabelMarkPosX_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        private void LabelMarkPosY_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        private void LabelMarkPosT_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        private void LabelMarkOffsetX_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        private void LabelMarkOffsetY_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        private void LabelMarkCount_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        private void LabelPatternOffsetX_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        private void LabelPatternOffsetY_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        private void LabelPatternCount_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }

        

        private void UpdateProcessData()
        {
            int num = (int)m_ScannerIndex;

            CMainFrame.DataManager.SystemData_Scan.Config[num].InScanResolution = Convert.ToSingle(lblResolutionX.Text);
            CMainFrame.DataManager.SystemData_Scan.Config[num].CrossScanResolution = Convert.ToSingle(lblResolutionY.Text);

            CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize = Convert.ToSingle(lblWaferSize.Text);
            CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeX = Convert.ToSingle(lblDiePitchX.Text);
            CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeY = Convert.ToSingle(lblDiePitchY.Text);
            CMainFrame.DataManager.ModelData.ProcData.MarginWidth = Convert.ToSingle(lblMarginWidth.Text);
            CMainFrame.DataManager.ModelData.ProcData.MarginHeight = Convert.ToSingle(lblMarginHeight.Text);
            CMainFrame.DataManager.ModelData.ProcData.ProcessLineNum = Convert.ToInt32(lblOverlapCount.Text);
        }
               

        private void DisplayProcessData()
        {
            int num = (int)m_ScannerIndex;

            lblResolutionX.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Scan.Config[num].InScanResolution);
            lblResolutionY.Text = string.Format("{0:F4}", CMainFrame.DataManager.SystemData_Scan.Config[num].CrossScanResolution);

            lblWaferSize.Text = string.Format("{0:F1}", CMainFrame.DataManager.ModelData.ProcData.ProcessWaferSize);
            lblDiePitchX.Text = string.Format("{0:F4}", CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeX);
            lblDiePitchY.Text = string.Format("{0:F4}", CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeY);
            lblMarginWidth.Text = string.Format("{0:F1}", CMainFrame.DataManager.ModelData.ProcData.MarginWidth);
            lblMarginHeight.Text = string.Format("{0:F1}", CMainFrame.DataManager.ModelData.ProcData.MarginHeight);
            lblOverlapCount.Text = string.Format("{0:F0}", CMainFrame.DataManager.ModelData.ProcData.ProcessLineNum);

        }

        private void ComboScannerIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_ScannerIndex = EScannerIndex.SCANNER1 + ComboScannerIndex.SelectedIndex;
            DisplayProcessData();
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

            int num = 0;

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
                    startLine.dY = endLine.dY = CMainFrame.DataManager.SystemData_Scan.Config[num].CrossScanResolution * i;

                    CMainFrame.Core.m_MeScanner.AddLine(startLine, endLine);
                }

                string filename = string.Format("{0:s}{1:s}{2:F0}.bmp", CMainFrame.DBInfo.ImageDataDir, "CutAxisX_", bmpFileNameNum + 1);

                CMainFrame.Core.m_MeScanner.SetSizeBmp();
                CMainFrame.Core.m_MeScanner.ConvertBmpFile(filename);

                CMainFrame.Core.m_MeScanner.DeleteAllObject();
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
                    startLine.dY = endLine.dY = CMainFrame.DataManager.SystemData_Scan.Config[num].CrossScanResolution * i;
                    CMainFrame.Core.m_MeScanner.AddLine(startLine, endLine);
                }

                string filename = string.Format("{0:s}{1:s}{2:F0}.bmp", CMainFrame.DBInfo.ImageDataDir, "CutAxisY_", bmpFileNameNum + 1);

                CMainFrame.Core.m_MeScanner.SetSizeBmp();
                CMainFrame.Core.m_MeScanner.ConvertBmpFile(filename);

                CMainFrame.Core.m_MeScanner.DeleteAllObject();
                //m_ScanManager.DeleteAllObject();
                bmpFileNameNum++;
                Y -= CMainFrame.DataManager.ModelData.ProcData.WaferDieSizeY;

            } while (Y > cutLastPos);

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("Save Wafer Cutting Bitmap Data ?")) return;


            UpdateProcessData();


            CMainFrame.Core.SaveModelData(CMainFrame.DataManager.ModelData);
        }

        private void btnProcessDataCreate_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("Create Wafer Cutting Bitmap ?")) return;

            UpdateProcessData();

            MakeWaferEllipseOutLine();
        }
    }
}
