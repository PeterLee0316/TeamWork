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

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormModelList : Form
    {
        private int nTreeIndex;
        private string strSelMakerName;
        private string strSelModelName;

        private TreeNode m_node;

        // 아래 4개의 멤버는 공용으로 쓸 수 있도록 added by sjr
        private EListHeaderType ListType;
        private List<CListHeader> HeaderList;
        private string CurrentUsing_ModelName;
        private string DisplayTypeName;

        public FormModelList(EListHeaderType type)
        {
            InitializeComponent();

            ListType = type;
            switch (type)
            {
                case EListHeaderType.MODEL:
                    HeaderList = CMainFrame.DataManager.ModelHeaderList;
                    CurrentUsing_ModelName = CMainFrame.DataManager.SystemData.ModelName;
                    DisplayTypeName = "Model";
                    this.Text = "Model Data";

                    TitleCurModel.Text = "현재 Model";
                    LabelMaker.Text = "Maker List";
                    LabelModel.Text = "Model List";

                    BtnModelSelect.Visible = true;

                    break;

                case EListHeaderType.CASSETTE:
                    HeaderList = CMainFrame.DataManager.CassetteHeaderList;
                    CurrentUsing_ModelName = CMainFrame.DataManager.ModelData.CassetteName;
                    DisplayTypeName = "Wafer Cassette";
                    this.Text = "Wafer Cassette Data";

                    TitleCurModel.Text = "현재 Cassette";
                    LabelMaker.Text = "Cassette Folder";
                    LabelModel.Text = "Cassette List";

                    BtnModelSelect.Visible = false;

                    break;

                case EListHeaderType.WAFERFRAME:
                    HeaderList = CMainFrame.DataManager.WaferFrameHeaderList;
                    CurrentUsing_ModelName = CMainFrame.DataManager.ModelData.WaferFrameName;
                    DisplayTypeName = "Wafer Frame";
                    this.Text = "Wafer Frame Data";

                    TitleCurModel.Text = "현재 Wafer Frame";
                    LabelMaker.Text = "Wafer Frame Folder";
                    LabelModel.Text = "Wafer Frame List";

                    BtnModelSelect.Visible = false;

                    break;
            }

            nTreeIndex = 0;

            InitMakerTreeView();

            InitGrid(0);

            m_node = new TreeNode(NAME_ROOT_FOLDER);

            string strMaker, strName;

            // Model List에 등록되어 있는 Model이 없으면 Return
            if (HeaderList.Count == 0)
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

            GridModelList.ColWidths.SetSize(1, 560);

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
            if (HeaderList.Count == 0)
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
            int nModelCount = 0;

            nTreeIndex = e.Node.Index;
            strSelMakerName = e.Node.Text;
            strSelModelName = null;

            InitGrid(0);

            foreach(CListHeader header in HeaderList)
            {
                // 전체 Model List에서 사용자가 선택한 Maker Folder에 속해 있는 모든 Model을 Display 한다.
                if (header.Parent == strSelMakerName)
                {
                    if (CMainFrame.DataManager.IsModelFolder(header.Name, ListType) == false)
                    {
                        nModelCount++;
                        InitGrid(nModelCount);
                        GridModelList[nModelCount, 1].Text = header.Name;
                    }
                }
            }
        }

        private void UpdateMakerNode()
        {
            int nNodeCount = 0;

            m_node.Nodes.Clear();
            MakerTreeView.Nodes.Clear();

            foreach(CListHeader header in HeaderList)
            {
                if(header.Name != NAME_ROOT_FOLDER && header.Name != NAME_DEFAULT_MODEL)
                {
                    if (header.IsFolder == true)
                    {
                        m_node.Nodes.Add(header.Name);
                    }
                }
            }

            MakerTreeView.Nodes.Add(m_node);
            MakerTreeView.ExpandAll();
        }

        private void BtnMakerCreate_Click(object sender, EventArgs e)
        {

            switch(ListType)
            {
                case EListHeaderType.MODEL:
                    if (!CMainFrame.DisplayMsg("Create new maker?"))
                    {
                        return;
                    }
                    break;
                case EListHeaderType.CASSETTE:
                    if (!CMainFrame.DisplayMsg("Make new folder?"))
                    {
                        return;
                    }
                    break;
                case EListHeaderType.WAFERFRAME:
                    if (!CMainFrame.DisplayMsg("Make new folder?"))
                    {
                        return;
                    }
                    break;

            }

            // Maker는 root밑에 한세대만으로 강제 고정함
            //var dlg = new FormCreateMaker(ListType, strSelMakerName);
            var dlg = new FormCreateMaker(ListType, NAME_ROOT_FOLDER);
            dlg.ShowDialog();

            if (dlg.DialogResult != DialogResult.OK)
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

            if (strSelMakerName == NAME_ROOT_FOLDER)
            {
                CMainFrame.DisplayMsg("Cannot delete Root Directory.");
                return;
            }

            if (!CMainFrame.DisplayMsg("Delete selected Maker?"))
            {
                return;
            }

            // 삭제 하고자 하는 Maker에 해당하는 Model Data가 있는지 Check
            int nCount = 0;
            bool bIsCurrentParent = false;
            List<string> childList = new List<string>();
            foreach(CListHeader header in HeaderList)
            {
                if (strSelMakerName == header.Parent)
                {
                    nCount++;
                    childList.Add(header.Name);

                    if(header.Name == CurrentUsing_ModelName)
                    {
                        bIsCurrentParent = true;
                    }
                }
            }

            if (bIsCurrentParent)
            {
                CMainFrame.DisplayMsg("Cannot delete Directory that include current model.");
                return;
            }

            if (nCount > 0)
            {
                if (!CMainFrame.DisplayMsg($"data exist in selected maker. Delete all?"))
                {
                    return;
                }
            }

            // 하위 Model Data Delete
            foreach (string name in childList)
            {
                CMainFrame.DataManager.DeleteModelData(name, ListType);
                CMainFrame.DataManager.DeleteModelHeader(name, ListType);

            }
            // 상위 Maker Delete
            CMainFrame.DataManager.DeleteModelHeader(strSelMakerName, ListType);

            UpdateMakerNode();
            UpdateModelData();
        }

        private void BtnModelCreate_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg($"Create new data?"))
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
                CMainFrame.DisplayMsg($"Input data name.");
                return;
            }

            // Model Name 중복 검사
            if (CMainFrame.DataManager.IsModelHeaderExist(strModify, ListType))
            {
                CMainFrame.DisplayMsg("already exist same name[Model or Maker]");
                return;
            }

            // create model header
            CListHeader header = new CListHeader();
            header.Name = strModify;
            header.Comment = strModify;
            header.Parent = strSelMakerName;
            header.IsFolder = false;
            header.TreeLevel = -1;

            HeaderList.Add(header);
            CMainFrame.DataManager.SaveModelHeaderList(ListType);

            // create model by using current model
            switch (ListType)
            {
                case EListHeaderType.MODEL:
                    CModelData modelData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData);
                    modelData.Name = header.Name;
                    CMainFrame.DataManager.SaveModelData(modelData);
                    break;
                case EListHeaderType.CASSETTE:
                    CWaferCassette cassetteData = ObjectExtensions.Copy(CMainFrame.DataManager.CassetteData);
                    cassetteData.Name = header.Name;
                    CMainFrame.DataManager.SaveModelData(cassetteData);
                    break;
                case EListHeaderType.WAFERFRAME:
                    CWaferFrame waferFrameData = ObjectExtensions.Copy(CMainFrame.DataManager.WaferFrameData);
                    waferFrameData.Name = header.Name;
                    CMainFrame.DataManager.SaveModelData(waferFrameData);
                    break;
            }

            int nRow = GridModelList.Data.RowCount + 1;
            InitGrid(nRow);

            GridModelList[nRow, 1].Text = strModify;
            UpdateModelData();
        }

        private void BtnModelDelete_Click(object sender, EventArgs e)
        {
            if (strSelModelName == "" || strSelModelName == null)
            {
                CMainFrame.DisplayMsg($"Need select data.");
                return;
            }

            if (!CMainFrame.DisplayMsg($"Delete selected data?"))
            {
                return;
            }

            if (strSelModelName == CurrentUsing_ModelName)
            {
                CMainFrame.DisplayMsg($"Cannot delete current using model.");
                return;
            }

            int i = 0, nNo = 0, nCount = 0, nIndex = 0;

            CMainFrame.DataManager.DeleteModelData(strSelModelName, ListType);
            CMainFrame.DataManager.DeleteModelHeader(strSelModelName, ListType);

            for (i = 0; i < HeaderList.Count; i++)
            {
                // 전체 Model List에서 사용자가 선택한 Maker Folder에 속해 있는 모든 Model을 Display 한다.
                if (HeaderList[i].Parent == strSelMakerName)
                {
                    if (HeaderList[i].IsFolder == false) // Model Data
                    {
                        nCount++;
                    }
                }
            }

            InitGrid(nCount);

            for (i = 0; i < HeaderList.Count; i++)
            {
                if (HeaderList[i].Parent == strSelMakerName)
                {
                    if (HeaderList[i].IsFolder == false)
                    {
                        nIndex++;
                        GridModelList[nIndex, 1].Text = HeaderList[i].Name;
                    }
                }
            }

        }

        private void BtnModelSelect_Click(object sender, EventArgs e)
        {
            if (strSelModelName == "" || strSelModelName == null)
            {
                CMainFrame.DisplayMsg("Need select Model.");
                return;
            }

            if (CurrentUsing_ModelName == strSelModelName)
            {
                CMainFrame.DisplayMsg("already using selected Model.");
                return;
            }

            if (!CMainFrame.DisplayMsg($"Change selected model?"))
            {
                return;
            }


            int iResult = SUCCESS;
            // change model
            switch (ListType)
            {
                case EListHeaderType.MODEL:
                    iResult = CMainFrame.DataManager.ChangeModel(strSelModelName);
                    break;
                case EListHeaderType.CASSETTE:
                    iResult = CMainFrame.DataManager.LoadCassetteData(strSelModelName);
                    break;
                case EListHeaderType.WAFERFRAME:
                    iResult = CMainFrame.DataManager.LoadWaferFrameData(strSelModelName);
                    break;
            }

            if(iResult != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult);
                return;
            }

            CurrentUsing_ModelName = strSelModelName;
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
            // UI Update
            LabelCurMaker.Text = strSelMakerName;
            LabelCurModel.Text = CurrentUsing_ModelName;

            // Model Data Update
            int nModelCount = 0;
            foreach(CListHeader header in HeaderList)
            {
                // 전체 Model List에서 사용자가 선택한 Maker Folder에 속해 있는 모든 Model을 Display 한다.
                if (strSelMakerName == header.Parent)
                {
                    if (header.IsFolder == false) // Model Data
                    {
                        nModelCount++;
                    }
                }
            }

            InitGrid(nModelCount);
            int index = 0;
            foreach (CListHeader header in HeaderList)
            {
                // 전체 Model List에서 사용자가 선택한 Maker Folder에 속해 있는 모든 Model을 Display 한다.
                if (strSelMakerName == header.Parent)
                {
                    if (header.IsFolder == false) // Model Data
                    {
                        GridModelList[index+1, 1].Text = header.Name;
                        index++;
                    }
                }
            }
        }
    }
}
