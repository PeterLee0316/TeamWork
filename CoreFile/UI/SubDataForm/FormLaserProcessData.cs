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

using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_MeStage;
using static Core.Layers.DEF_CtrlStage;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_DataManager;

using Core.Layers;


namespace Core.UI
{
    public partial class FormLaserProcessData : Form
    {
        string[] strOP = new string[(int)ELaserOperation.MAX];

        private int selectedSequenceNum;
        private CLaserProcessData LaserProcessData;

        public FormLaserProcessData()
        {
            InitializeComponent();

            InitGrid();

            // Motion Pos Data read
            for (int i = 0; i < (int)EStagePos.MAX; i++)
            {
                ComboStageIndex.Items.Add(EStagePos.WAIT + i);
            }

            // Index Init
            selectedSequenceNum = 0;
            ComboStageIndex.SelectedIndex = 0;

            // Process Data Copy
            LaserProcessData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData.LaserProcessData);

            UpdateData();

            this.Text = $"Laser Process Data [ Current Model : {CMainFrame.DataManager.ModelData.Name} ]";
            
        }

        private void FormLaserProcessData_Load(object sender, EventArgs e)
        {

        }

        private void InitGrid()
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridCtrl.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCtrl.Properties.RowHeaders = true;
            GridCtrl.Properties.ColHeaders = true;

            // Column,Row 개수
            GridCtrl.ColCount = 1;
            GridCtrl.RowCount = DEF_MAX_LASER_PROCESS_STEP;

            // Column 가로 크기설정
            GridCtrl.ColWidths.SetSize(0, 50);
            GridCtrl.ColWidths.SetSize(1, 200);
            //GridCtrl.ColWidths.SetSize(2, 110);
            //GridCtrl.ColWidths.SetSize(3, 110);

            for (int i = 0; i < (int)ELaserOperation.MAX; i++)
            {
                strOP[i] = Convert.ToString(ELaserOperation.NONE + i);
            }

            StringCollection strColl = new StringCollection();

            strColl.AddRange(strOP);

            for (int i = 0; i < GridCtrl.RowCount + 1; i++)
            {
                GridCtrl.RowHeights[i] = 35;
            }

            for (int i = 0; i < GridCtrl.RowCount; i++)
            {
                GridStyleInfo style = GridCtrl.Model[i + 1, 1];

                style.CellType = GridCellTypeName.ComboBox;

                style.ChoiceList = strColl;

                GridCtrl[i + 1, 1].DropDownStyle = GridDropDownStyle.Exclusive;
            }

            GridComboBoxCellModel model = this.GridCtrl.Model.CellModels["ComboBox"] as GridComboBoxCellModel;
            model.ButtonBarSize = new Size(70, 30);

            // Text Display
            GridCtrl[0, 0].Text = "Seq.";
            GridCtrl[0, 1].Text = "Operation";

            for (int i = 0; i < GridCtrl.ColCount + 1; i++)
            {
                for (int j = 0; j < GridCtrl.RowCount + 1; j++)
                {
                    // Font Style - Bold
                    GridCtrl[j, i].Font.Bold = true;

                    GridCtrl[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCtrl[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }
            

            GridCtrl.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCtrl.ResizeColsBehavior = 0;
            GridCtrl.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCtrl.Refresh();
        }

        private void UpdateData(int IndexNum=0)
        {
            for (int i = 0; i < DEF_MAX_LASER_PROCESS_STEP; i++)
            {
                GridCtrl[i + 1, 1].Text = strOP[(int)LaserProcessData.WorkSteps_General[i].Operation];
                GridCtrl[i + 1, 1].TextColor = Color.Black;
            }

            LabelDefaultConfigFile.Text = LaserProcessData.DefaultScannerConfigFile;

            LabelMarkJobFile.Text = LaserProcessData.WorkSteps_General[IndexNum].ScannerJobFile;
            LabelMarkBmpFile.Text = LaserProcessData.WorkSteps_General[IndexNum].ScannerBmpFile;

            LabelMarkPosX.Text = String.Format("{0:F4}", LaserProcessData.WorkSteps_General[IndexNum].MarkPos.dX);
            LabelMarkPosY.Text = String.Format("{0:F4}", LaserProcessData.WorkSteps_General[IndexNum].MarkPos.dY);
            LabelMarkPosT.Text = String.Format("{0:F4}", LaserProcessData.WorkSteps_General[IndexNum].MarkPos.dT);
            LabelInPositionDelay.Text = String.Format("{0:F0}", LaserProcessData.WorkSteps_General[IndexNum].InPosDelay);


            LabelMarkOffsetX.Text = String.Format("{0:F4}", LaserProcessData.WorkSteps_General[IndexNum].MarkOffset.dX);
            LabelMarkOffsetY.Text = String.Format("{0:F4}", LaserProcessData.WorkSteps_General[IndexNum].MarkOffset.dY);
            LabelMarkCount.Text = String.Format("{0:F0}", LaserProcessData.WorkSteps_General[IndexNum].MarkCount);

            LabelPatternOffsetX.Text = String.Format("{0:F4}", LaserProcessData.WorkSteps_General[IndexNum].PatternOffset.dX);
            LabelPatternOffsetY.Text = String.Format("{0:F4}", LaserProcessData.WorkSteps_General[IndexNum].PatternOffset.dY);
            LabelPatternCount.Text = String.Format("{0:F0}", LaserProcessData.WorkSteps_General[IndexNum].PatternCount);

            LabelTextColorToBlack();
        }
        
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save data?")) return;
            
            int indexNum = selectedSequenceNum;

            LaserProcessData.DefaultScannerConfigFile   = LabelDefaultConfigFile.Text;

            LaserProcessData.WorkSteps_General[indexNum].ScannerJobFile = LabelMarkJobFile.Text;
            LaserProcessData.WorkSteps_General[indexNum].ScannerBmpFile = LabelMarkBmpFile.Text;

            LaserProcessData.WorkSteps_General[indexNum].MarkPos.dX = Convert.ToDouble(LabelMarkPosX.Text);
            LaserProcessData.WorkSteps_General[indexNum].MarkPos.dY = Convert.ToDouble(LabelMarkPosY.Text);
            LaserProcessData.WorkSteps_General[indexNum].MarkPos.dT = Convert.ToDouble(LabelMarkPosT.Text);
            LaserProcessData.WorkSteps_General[indexNum].InPosDelay = Convert.ToInt32(LabelInPositionDelay.Text);

            LaserProcessData.WorkSteps_General[indexNum].MarkOffset.dX   = Convert.ToDouble(LabelMarkOffsetX.Text);
            LaserProcessData.WorkSteps_General[indexNum].MarkOffset.dY   = Convert.ToDouble(LabelMarkOffsetY.Text);
            LaserProcessData.WorkSteps_General[indexNum].MarkCount       = Convert.ToInt32(LabelMarkCount.Text);

            LaserProcessData.WorkSteps_General[indexNum].PatternOffset.dX = Convert.ToDouble(LabelPatternOffsetX.Text);
            LaserProcessData.WorkSteps_General[indexNum].PatternOffset.dY = Convert.ToDouble(LabelPatternOffsetY.Text);
            LaserProcessData.WorkSteps_General[indexNum].PatternCount = Convert.ToInt32(LabelPatternCount.Text);

            for (int i = 0; i < DEF_MAX_LASER_PROCESS_STEP; i++)
            {
                ELaserOperation cnvt = ELaserOperation.NONE;
                try
                {
                    cnvt = (ELaserOperation)Enum.Parse(typeof(ELaserOperation), GridCtrl[i + 1, 1].Text);
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                LaserProcessData.WorkSteps_General[i].Operation = cnvt;
            }
            
           
            // save
            CMainFrame.DataManager.ModelData.LaserProcessData = ObjectExtensions.Copy(LaserProcessData);

            CMainFrame.mCore.SaveModelData(CMainFrame.DataManager.ModelData);

            UpdateData(selectedSequenceNum);
        }

        private void GridSpinner_CellClick(object sender, GridCellClickEventArgs e)
        {
            if (e.RowIndex < 1) return;

            selectedSequenceNum = e.RowIndex-1;            

            LabelSequenceNum.Text = e.RowIndex.ToString();

            UpdateData(selectedSequenceNum);

        }

        private void GridSpinner_CurrentCellShowedDropDown(object sender, EventArgs e)
        {
            GridControlBase grid = sender as GridControlBase;

            if (grid != null)
            {
                GridCurrentCell CurCell = grid.CurrentCell;

                GridCtrl[CurCell.RowIndex, CurCell.ColIndex].TextColor = Color.Blue;
            }
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
            LaserProcessData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData.LaserProcessData);

            UpdateData();
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
                data.ForeColor = Color.Red;
            }

            
        }

        private void LabelDefaultBmpFile_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;
            
            string filename = string.Empty;
            OpenFileDialog fileOpenDlg = new OpenFileDialog();
            fileOpenDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
            fileOpenDlg.Filter = "BMP(*.bmp,*.lse)|*.bmp;*.lse";

            if (fileOpenDlg.ShowDialog() == DialogResult.OK)
            {
                data.Text = fileOpenDlg.FileName;
                data.ForeColor = Color.Red;
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
                data.ForeColor = Color.Red;
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
                data.ForeColor = Color.Red;
            }            
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
            Btn.ForeColor = Color.Red;
        }

        private void LabelTextColorToBlack()
        {
            LabelDefaultConfigFile.ForeColor = Color.Black;
            LabelMarkJobFile.ForeColor = Color.Black;
            LabelMarkBmpFile.ForeColor = Color.Black;
            LabelMarkPosX.ForeColor = Color.Black;
            LabelMarkPosY.ForeColor = Color.Black;
            LabelMarkPosT.ForeColor = Color.Black;
            LabelInPositionDelay.ForeColor = Color.Black;
            LabelMarkOffsetX.ForeColor = Color.Black;
            LabelMarkOffsetY.ForeColor = Color.Black;
            LabelMarkCount.ForeColor = Color.Black;
            LabelPatternOffsetX.ForeColor = Color.Black;
            LabelPatternOffsetY.ForeColor = Color.Black;
            LabelPatternCount.ForeColor = Color.Black;
        }

        private void BtnDeleteJobName_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("delete Job file path?")) return;
            
            LabelMarkJobFile.Text = "";
        }

        private void BtnDeleteBmpName_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("delete bmp file path?")) return;

            LabelMarkBmpFile.Text = "";
        }

        private void BtnStageChangeValue_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Read Current Stage Position?")) return;
           
            int iResult = -1;
            CPos_XYTZ stageCurPos;
            iResult = CMainFrame.mCore.m_ctrlStage1.GetStagePos(out stageCurPos);

            if (iResult != SUCCESS) return;

            LabelMarkPosX.Text = String.Format("{0:F4}", stageCurPos.dX);
            LabelMarkPosY.Text = String.Format("{0:F4}", stageCurPos.dY);
            LabelMarkPosT.Text = String.Format("{0:F4}", stageCurPos.dT);

            LabelMarkPosX.ForeColor = Color.Red;
            LabelMarkPosY.ForeColor = Color.Red;
            LabelMarkPosT.ForeColor = Color.Red;

        }

        private void BtnJog_Click(object sender, EventArgs e)
        {
            CMainFrame.DisplayJog();
        }

        private void BtnStageMove_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Move to Teach Position?")) return;

            int index = ComboStageIndex.SelectedIndex;

            switch(index)
            {
                case (int)EStagePos.WAIT:
                    CMainFrame.mCore.m_ctrlStage1.MoveToStageWaitPos();
                    break;

                case (int)EStagePos.LOAD:
                    CMainFrame.mCore.m_ctrlStage1.MoveToStageLoadPos();
                    break;

                case (int)EStagePos.UNLOAD:
                    CMainFrame.mCore.m_ctrlStage1.MoveToStageUnloadPos();
                    break;

                case (int)EStagePos.STAGE_CENTER_PRE:
                    CMainFrame.mCore.m_ctrlStage1.MoveToStageCenterPre();
                    break;

                case (int)EStagePos.STAGE_CENTER_FINE:
                    CMainFrame.mCore.m_ctrlStage1.MoveToStageCenterFine();
                    break;

                case (int)EStagePos.STAGE_CENTER_INSPECT:
                    CMainFrame.mCore.m_ctrlStage1.MoveToStageCenterInspect();
                    break;       

                case (int)EStagePos.EDGE_ALIGN_1:
                    CMainFrame.mCore.m_ctrlStage1.MoveToEdgeAlignPos1();
                    break;

                case (int)EStagePos.MACRO_CAM_POS:
                    CMainFrame.mCore.m_ctrlStage1.MoveToMacroCam();
                    break;

                case (int)EStagePos.MACRO_ALIGN:
                    CMainFrame.mCore.m_ctrlStage1.MoveToMacroAlignA();
                    break;

                case (int)EStagePos.MICRO_ALIGN:
                    CMainFrame.mCore.m_ctrlStage1.MoveToMicroAlignA();
                    break;

                case (int)EStagePos.MICRO_ALIGN_TURN:
                    CMainFrame.mCore.m_ctrlStage1.MoveToMicroAlignTurnA();
                    break;

                case (int)EStagePos.LASER_PROCESS:
                    CMainFrame.mCore.m_ctrlStage1.MoveToProcessPos();
                    break;

                case (int)EStagePos.LASER_PROCESS_TURN:
                    CMainFrame.mCore.m_ctrlStage1.MoveToProcessTurnPos();
                    break;

            }
        }

        
    }
}
