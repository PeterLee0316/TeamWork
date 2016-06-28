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

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormModelData : Form
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

        public FormModelData(EListHeaderType type)
        {
            InitializeComponent();

            ListType = type;
            switch (type)
            {
                case EListHeaderType.MODEL:
                    HeaderList = CMainFrame.LWDicer.m_DataManager.ModelHeaderList;
                    CurrentUsing_ModelName = CMainFrame.LWDicer.m_DataManager.SystemData.ModelName;
                    DisplayTypeName = "Model";
                    break;
                case EListHeaderType.CASSETTE:
                    HeaderList = CMainFrame.LWDicer.m_DataManager.CassetteHeaderList;
                    CurrentUsing_ModelName = CMainFrame.LWDicer.m_DataManager.ModelData.CassetteName;
                    DisplayTypeName = "Wafer Cassette";
                    break;
                case EListHeaderType.WAFERFRAME:
                    HeaderList = CMainFrame.LWDicer.m_DataManager.WaferFrameHeaderList;
                    CurrentUsing_ModelName = CMainFrame.LWDicer.m_DataManager.ModelData.WaferFrameName;
                    DisplayTypeName = "Wafer Frame";
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
                    if (CMainFrame.LWDicer.m_DataManager.IsModelFolder(header.Name, ListType) == false)
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
            if (!CMainFrame.LWDicer.DisplayMsg("", "새로운 Maker를 생성 하시겠습니까?"))
            {
                return;
            }

            // Maker는 root밑에 한세대만으로 강제 고정함
            //FormCreateMaker dlg = new FormCreateMaker(ListType, strSelMakerName);
            FormCreateMaker dlg = new FormCreateMaker(ListType, NAME_ROOT_FOLDER);
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
                CMainFrame.LWDicer.DisplayMsg("", "Root Directory는 삭제 할수 없습니다.", false);
                return;
            }

            if (!CMainFrame.LWDicer.DisplayMsg("", "선택하신 Maker를 삭제 시겠습니까?"))
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
                CMainFrame.LWDicer.DisplayMsg("", "현재 사용중인 Model이 속한 Directory는 삭제 할수 없습니다.", false);
                return;
            }

            if (nCount > 0)
            {
                if (!CMainFrame.LWDicer.DisplayMsg("", "선택하신 Maker에 하위 {DisplayTypeName} Data가 존재 합니다. 모두 삭제 시겠습니까?"))
                {
                    return;
                }
            }

            // 하위 Model Data Delete
            foreach(string name in childList)
            {
                CMainFrame.LWDicer.m_DataManager.DeleteModelData(name, ListType);
                CMainFrame.LWDicer.m_DataManager.DeleteModelHeader(name, ListType);

            }
            // 상위 Maker Delete
            CMainFrame.LWDicer.m_DataManager.DeleteModelHeader(strSelMakerName, ListType);

            UpdateMakerNode();
            UpdateModelData();
        }

        private void BtnModelCreate_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "새로운 {DisplayTypeName} Data를 생성 하시겠습니까?"))
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
                CMainFrame.LWDicer.DisplayMsg("", "{DisplayTypeName} Data Name을 입력하여 주십시시오",false);
                return;
            }

            // Model Name 중복 검사
            if (CMainFrame.LWDicer.m_DataManager.IsModelHeaderExist(strModify, ListType))
            {
                CMainFrame.LWDicer.DisplayMsg("", "이미 동일한 이름의 Model or Maker가 존재합니다.",false);
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
            CMainFrame.LWDicer.m_DataManager.SaveModelHeaderList(ListType);

            // create model by using current model
            switch (ListType)
            {
                case EListHeaderType.MODEL:
                    CModelData modelData = ObjectExtensions.Copy(CMainFrame.LWDicer.m_DataManager.ModelData);
                    modelData.Name = header.Name;
                    CMainFrame.LWDicer.m_DataManager.SaveModelData(modelData);
                    break;
                case EListHeaderType.CASSETTE:
                    CWaferCassette cassetteData = ObjectExtensions.Copy(CMainFrame.LWDicer.m_DataManager.CassetteData);
                    cassetteData.Name = header.Name;
                    CMainFrame.LWDicer.m_DataManager.SaveModelData(cassetteData);
                    break;
                case EListHeaderType.WAFERFRAME:
                    CWaferFrame waferFrameData = ObjectExtensions.Copy(CMainFrame.LWDicer.m_DataManager.WaferFrameData);
                    waferFrameData.Name = header.Name;
                    CMainFrame.LWDicer.m_DataManager.SaveModelData(waferFrameData);
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
                CMainFrame.LWDicer.DisplayMsg("", "삭제 하고자 하는 {DisplayTypeName} Data를 선택하여 주십시시오",false);
                return;
            }

            if (!CMainFrame.LWDicer.DisplayMsg("", "선택하신 {DisplayTypeName} Data를 삭제 시겠습니까?"))
            {
                return;
            }

            if (strSelModelName == CurrentUsing_ModelName)
            {
                CMainFrame.LWDicer.DisplayMsg("", "현재 사용중인 {DisplayTypeName} Data는 삭제 할수 없습니다.",false);
                return;
            }

            int i = 0, nNo = 0, nCount = 0, nIndex = 0;

            CMainFrame.LWDicer.m_DataManager.DeleteModelData(strSelModelName, ListType);
            CMainFrame.LWDicer.m_DataManager.DeleteModelHeader(strSelModelName, ListType);

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
                CMainFrame.LWDicer.DisplayMsg("", "Model을 선택해 주십시오",false);
                return;
            }

            if (CurrentUsing_ModelName == strSelModelName)
            {
                CMainFrame.LWDicer.DisplayMsg("", "현재 사용중인 Data 입니다.",false);
                return;
            }

            if (!CMainFrame.LWDicer.DisplayMsg("", "선택하신 {DisplayTypeName} Data로 변경 하시겠습니까?"))
            {
                return;
            }


            int iResult = SUCCESS;
            // change model
            switch (ListType)
            {
                case EListHeaderType.MODEL:
                    iResult = CMainFrame.LWDicer.m_DataManager.ChangeModel(strSelModelName);
                    break;
                case EListHeaderType.CASSETTE:
                    iResult = CMainFrame.LWDicer.m_DataManager.LoadCassetteData(strSelModelName);
                    break;
                case EListHeaderType.WAFERFRAME:
                    iResult = CMainFrame.LWDicer.m_DataManager.LoadWaferFrameData(strSelModelName);
                    break;
            }

            if(iResult != SUCCESS)
            {
                CMainFrame.LWDicer.AlarmDisplay(iResult);
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
