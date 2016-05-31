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

using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormModelData : Form
    {
        private int nTreeIndex;
        private string strSelMakerName;
        private string strSelModelName;

        private CModelHeader NewHeader;
        private CModelData NewModel;
        private TreeNode m_node;


        private FormCreateMaker m_CreateMakerForm;

        public FormModelData()
        {
            InitializeComponent();

            m_CreateMakerForm = new FormCreateMaker();

            nTreeIndex = 0;

            InitMakerTreeView();

            InitGrid(0);

            NewHeader = new CModelHeader();
            NewModel = new CModelData();
            m_node = new TreeNode(NAME_ROOT_FOLDER);

            string strMaker = string.Empty, strName = string.Empty;

            // Model List에 등록되어 있는 Model이 없으면 Return
            if (CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount() == 0)
            {
                return;
            }

            UpdateMakerNode();

            // 등록되어 있는 Maker가 Root 하나이면 Root Folder 생성 
            if (MakerTreeView.Nodes.Count == 0)
            {
                MakerTreeView.Nodes.Add(m_node);
            }

            UpdateModelData();
        }


        private void InitMakerTreeView()
        {
            MakerTreeView.ItemHeight = 60;

            MakerTreeView.ImageIndex = 1;

        }

        private void InitGrid(int nRowCount)
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridModelList.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridModelList.Properties.RowHeaders = false;
            GridModelList.Properties.ColHeaders = false;

            nCol = 1;
            nRow = nRowCount;

            // Column,Row 개수
            GridModelList.ColCount = nCol;
            GridModelList.RowCount = nRow;

            GridModelList.ColWidths.SetSize(1, 461);

            for (i = 0; i < nRow + 1; i++)
            {
                GridModelList.RowHeights[i] = 40;
            }

            GridModelList.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridModelList.ResizeColsBehavior = 0;
            GridModelList.ResizeRowsBehavior = 0;

            //GridModelList.VScrollBehavior = GridScrollbarMode.Disabled;
            GridModelList.HScrollBehavior = GridScrollbarMode.Disabled;

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridModelList[j, i].Font.Bold = true;

                    GridModelList[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridModelList[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }


            // Grid Display Update
            GridModelList.Refresh();
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormModelData_Load(object sender, EventArgs e)
        {
            // Model List에 등록되어 있는 Model이 없으면 Return
            if (CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount() == 0)
            {
                return;
            }

            UpdateMakerNode();
            UpdateModelData();
        }

        private void FormModelData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void MakerlTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string strParent = string.Empty, strName = string.Empty;

            int nMakerCount = 0, nModelCount = 0, i = 0;

            nTreeIndex = e.Node.Index;
            strSelMakerName = e.Node.Text;
            strSelModelName = null;

            InitGrid(0);

            nMakerCount = CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount();

            for (i = 0; i < nMakerCount; i++)
            {
                // 전체 Model List에서 사용자가 선택한 Maker Folder에 속해 있는 모든 Model을 Display 한다.
                if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Parent == strSelMakerName)
                {
                    strName = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name;

                    if (CMainFrame.LWDicer.m_DataManager.IsModelFolder(strName) == false)
                    {
                        nModelCount++;
                        InitGrid(nModelCount);
                        GridModelList[nModelCount, 1].Text = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name;
                    }
                }
            }
        }

        private void UpdateMakerNode()
        {
            int nMakerCount = 0, nNodeCount = 0, i = 0;

            string strMaker = string.Empty;

            nMakerCount = CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount();

            m_node.Nodes.Clear();

            MakerTreeView.Nodes.Clear();

            for (i = 0; i < nMakerCount;i++)
            {
                strMaker = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name;

                if(strMaker != NAME_ROOT_FOLDER && strMaker != NAME_DEFAULT_MODEL)
                {
                    if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].IsFolder == true)
                    {
                        m_node.Nodes.Add(CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name);
                    }
                }
            }

            MakerTreeView.Nodes.Add(m_node);

            MakerTreeView.ExpandAll();
        }

        private void BtnMakerCreate_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("새로운 Maker를 생성 하시겠습니까?"))
            {
                return;
            }

            m_CreateMakerForm.ShowDialog();

            if (m_CreateMakerForm.DialogResult != DialogResult.OK)
            {
                return;
            }

            UpdateMakerNode();
            
        }

        private void BtnMakerDelete_Click(object sender, EventArgs e)
        {
            if (MakerTreeView.Nodes.Count == 0)
            {
                return;
            }

            if (!CMainFrame.LWDicer.DisplayMsg("선택하신 Maker를 삭제 시겠습니까?"))
            {
                return;
            }

            // 삭제 하고자 하는 Maker에 해당하는 Model Data가 있는지 Check
            int i = 0, nCount = 0;
            string strCurModel = string.Empty, strName = string.Empty;

            for (i = 0; i < CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount(); i++)
            {
                if (strSelMakerName == CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Parent)
                {
                    strName = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name;

                    if (CMainFrame.LWDicer.m_DataManager.IsModelFolder(strName) == false)
                    {
                        nCount++;
                    }

                    if (CMainFrame.LWDicer.m_DataManager.SystemData.ModelName == CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name)
                    {
                        strCurModel = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name;
                    }
                }
            }

            if (nCount != 0)
            {
                if (!CMainFrame.LWDicer.DisplayMsg("선택하신 Maker에 하위 Model Data가 존재 합니다. 모두 삭제 시겠습니까?"))
                {
                    return;
                }
            }

            if (strCurModel == CMainFrame.LWDicer.m_DataManager.SystemData.ModelName)
            {
                CMainFrame.LWDicer.DisplayMsg("설비에 적용되어 있는 현재 Model Data는 삭제 할수 없습니다.");
                return;
            }

            if (strSelMakerName == NAME_ROOT_FOLDER)
            {
                CMainFrame.LWDicer.DisplayMsg("Root Folder는 삭제 할수 없습니다.");
                return;
            }


            // 하위 Model Data Delete
            RECOUNT:
            int nModelCount = CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount();

            for (i = 0; i < nModelCount; i++)
            {
                // 하위 Model Data Delete
                if (strSelMakerName == CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Parent && CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].IsFolder == false)
                {
                    CMainFrame.LWDicer.m_DataManager.DeleteModelHeader(CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name);

                    goto RECOUNT;
                }
            }

            // 상위 Maker Delete
            CMainFrame.LWDicer.m_DataManager.DeleteModelHeader(strSelMakerName);

            UpdateMakerNode();

            UpdateModelData();
        }

        private void BtnModelCreate_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("새로운 Model Data를 생성 하시겠습니까?"))
            {
                return;
            }

            string strModify = "";

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify))
            {
                return;
            }

            if (strModify == "" || strModify == null)
            {
                CMainFrame.LWDicer.DisplayMsg("Model Data Name을 입력하여 주십시시오");
                return;
            }

            NewHeader.Name = strModify;
            NewHeader.Comment = strModify;
            NewHeader.Parent = strSelMakerName;
            NewHeader.IsFolder = false;
            NewHeader.TreeLevel = -1;

            // Model Name 중복 검사
            int i = 0;
            for (i = 0; i < CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount(); i++)
            {
                if (strModify == CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name)
                {
                    CMainFrame.LWDicer.DisplayMsg("현재 등록되어 있는 Model 입니다.");
                    return;
                }
            }

            CMainFrame.LWDicer.m_DataManager.ModelHeaderList.Add(NewHeader);

            CMainFrame.LWDicer.m_DataManager.SaveModelHeaderList();

            CMainFrame.LWDicer.m_DataManager.LoadModelList();

            NewModel = CMainFrame.LWDicer.m_DataManager.ModelData;

            NewModel.Name = NewHeader.Name;

            CMainFrame.LWDicer.m_DataManager.SaveModelData(NewModel);

            int nRow = 0;

            nRow = GridModelList.Data.RowCount + 1;

            InitGrid(nRow);

            GridModelList[nRow, 1].Text = strModify;

            UpdateModelData();

        }

        private void BtnModelDelete_Click(object sender, EventArgs e)
        {
            if (strSelModelName == "" || strSelModelName == null)
            {
                CMainFrame.LWDicer.DisplayMsg("삭제 하고자 하는 Model Data를 선택하여 주십시시오");
                return;
            }

            if (!CMainFrame.LWDicer.DisplayMsg("선택하신 Model Data를 삭제 시겠습니까?"))
            {
                return;
            }

            if (strSelModelName == CMainFrame.LWDicer.m_DataManager.SystemData.ModelName)
            {
                CMainFrame.LWDicer.DisplayMsg("설비에 적용되어 있는 현재 Model Data는 삭제 할수 없습니다.");
                return;
            }

            int i = 0, nNo = 0, nCount = 0, nIndex = 0;

            CMainFrame.LWDicer.m_DataManager.DeleteModelData(strSelModelName);

            for (i = 0; i < CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount(); i++)
            {
                if (strSelModelName == CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name)
                {
                    nNo = i;
                    break;
                }
            }

            CMainFrame.LWDicer.m_DataManager.ModelHeaderList.RemoveAt(nNo);

            CMainFrame.LWDicer.m_DataManager.SaveModelHeaderList();

            for (i = 0; i < CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount(); i++)
            {
                // 전체 Model List에서 사용자가 선택한 Maker Folder에 속해 있는 모든 Model을 Display 한다.
                if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Parent == strSelMakerName)
                {
                    if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].IsFolder == false) // Model Data
                    {
                        nCount++;
                    }
                }
            }

            InitGrid(nCount);

            for (i = 0; i < CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount(); i++)
            {
                if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Parent == strSelMakerName)
                {
                    if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].IsFolder == false)
                    {
                        nIndex++;
                        GridModelList[nIndex, 1].Text = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name;
                    }
                }
            }

        }

        private void BtnModelSelect_Click(object sender, EventArgs e)
        {
            if (strSelModelName == "" || strSelModelName == null)
            {
                CMainFrame.LWDicer.DisplayMsg("Model을 선택해 주십시오");
                return;
            }

            if (!CMainFrame.LWDicer.DisplayMsg("선택하신 Model Data를 실행 하시겠습니까?"))
            {
                return;
            }

            CMainFrame.LWDicer.m_DataManager.SystemData.ModelName = strSelModelName;

            UpdateModelData();
        }

        private void GridModelList_CellClick(object sender, GridCellClickEventArgs e)
        {
            strSelModelName = GridModelList[e.RowIndex, e.ColIndex].Text;

            SelectGridCell(e.RowIndex, e.ColIndex);
        }

        private void SelectGridCell(int nRow, int nCol)
        {
            for (int i = 0; i < GridModelList.RowCount; i++)
            {
                GridModelList[i + 1, 1].BackColor = Color.White;
            }

            GridModelList[nRow, nCol].BackColor = Color.LightSteelBlue;
        }

        private void UpdateModelData()
        {
            int nModelNo = 0, i = 0, nCurMakerNo = 0, nModelCount = 0;

            CModelData ChangeModel = new CModelData();

            for (i = 0; i < CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount(); i++)
            {
                if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].IsFolder == false)
                {
                    if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name == CMainFrame.LWDicer.m_DataManager.SystemData.ModelName)
                    {
                        nModelNo = i;
                    }
                }
            }

            ChangeModel.Name = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[nModelNo].Name;

            CMainFrame.LWDicer.m_DataManager.ChangeModel(ChangeModel.Name);

            for (i = 0; i < CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount(); i++)
            {
                if (CMainFrame.LWDicer.m_DataManager.SystemData.ModelName == CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name)
                {
                    nCurMakerNo = i;
                    break;
                }
            }

            // UI Update
            LabelCurMaker.Text = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[nCurMakerNo].Parent;

            strSelMakerName = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[nCurMakerNo].Parent;

            LabelCurModel.Text = CMainFrame.LWDicer.m_DataManager.SystemData.ModelName;

            // Model Data Update
            for (i = 0; i < CMainFrame.LWDicer.m_DataManager.GetModelHeaderCount(); i++)
            {
                // 전체 Model List에서 사용자가 선택한 Maker Folder에 속해 있는 모든 Model을 Display 한다.
                if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[nCurMakerNo].Parent == CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Parent)
                {
                    if (CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].IsFolder == false) // Model Data
                    {
                        nModelCount++;
                        InitGrid(nModelCount);
                        GridModelList[nModelCount, 1].Text = CMainFrame.LWDicer.m_DataManager.ModelHeaderList[i].Name;
                    }
                }
            }

            for (i = 0; i < GridModelList.RowCount; i++)
            {
                if (CMainFrame.LWDicer.m_DataManager.SystemData.ModelName == GridModelList[i + 1, 1].Text)
                {
                    SelectGridCell(i + 1, 1);
                }
            }
        }
    }
}
