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

using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_DataManager;

namespace Core.UI
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

                    TitleCurModel.Text = "current Model";
                    LabelMaker.Text = "Maker List";
                    LabelModel.Text = "Model List";

                    BtnModelSelect.Visible = true;

                    break;

                case EListHeaderType.WAFERFRAME:
                    HeaderList = CMainFrame.DataManager.WaferFrameHeaderList;
                    CurrentUsing_ModelName = CMainFrame.DataManager.ModelData.WaferFrameName;
                    DisplayTypeName = "Wafer Frame";
                    this.Text = "Wafer Frame Data";

                    TitleCurModel.Text = "current Wafer Frame";
                    LabelMaker.Text = "Wafer Frame Folder";
                    LabelModel.Text = "Wafer Frame List";

                    BtnModelSelect.Visible = false;

                    break;

                case EListHeaderType.USERINFO:
                    HeaderList = CMainFrame.DataManager.UserInfoHeaderList;
                    CurrentUsing_ModelName = CMainFrame.DataManager.LoginInfo.User.Name;
                    DisplayTypeName = "User Info";
                    this.Text = "User Info Data";

                    TitleCurMaker.Visible = false;
                    LabelCurMaker.Visible = false;
                    TitleCurModel.Text = "current user info";
                    LabelMaker.Text = "User Group";
                    LabelModel.Text = "User List";

                    BtnModelSelect.Visible = false;
                    BtnMakerCreate.Visible = false;
                    BtnMakerDelete.Visible = false;

                    break;
            }

            nTreeIndex = 0;

            InitMakerTreeView();

            m_node = new TreeNode(NAME_ROOT_FOLDER);
        }


        private void InitMakerTreeView()
        {
            MakerTreeView.ItemHeight = 60;

            MakerTreeView.ImageIndex = 1;

        }

        private void InitGrid(int nRowCount)
        {
            // Cell Click 시 커서가 생성되지 않게함.
            GridCtrl.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCtrl.Properties.RowHeaders = false;
            GridCtrl.Properties.ColHeaders = false;

            int nCol = 2;
            int nRow = nRowCount;

            // Column,Row 개수
            GridCtrl.ColCount = nCol;
            GridCtrl.RowCount = nRow;

            GridCtrl.ColWidths.SetSize(1, 300);
            GridCtrl.ColWidths.SetSize(2, 330);

            for (int i = 0; i < nRow + 1; i++)
            {
                GridCtrl.RowHeights[i] = 40;
            }

            GridCtrl.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCtrl.ResizeColsBehavior = 0;
            GridCtrl.ResizeRowsBehavior = 0;

            //GridModelList.VScrollBehavior = GridScrollbarMode.Disabled;
            GridCtrl.HScrollBehavior = GridScrollbarMode.Disabled;

            for (int i = 0; i < nCol + 1; i++)
            {
                for (int j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridCtrl[j, i].Font.Bold = true;
                    GridCtrl[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCtrl[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }
            // Grid Display Update
            GridCtrl.Refresh();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
        }

        private void MakerlTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int nModelCount = 0;

            nTreeIndex = e.Node.Index;
            strSelMakerName = e.Node.Text;
            strSelModelName = "";

            UpdateModelData();
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
                    if (!CMainFrame.InquireMsg("Create new maker?"))
                    {
                        return;
                    }
                    break;
                case EListHeaderType.WAFERFRAME:
                    if (!CMainFrame.InquireMsg("Make new folder?"))
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

            if (!CMainFrame.InquireMsg("Delete selected Maker?"))
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
                if (!CMainFrame.InquireMsg($"data exist in selected maker. Delete all?"))
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
            if (ListType == EListHeaderType.USERINFO)
            {
                if(!(strSelMakerName == ELoginType.OPERATOR.ToString() || strSelMakerName == ELoginType.ENGINEER.ToString()))
                    return;
            }

            if (!CMainFrame.InquireMsg($"Create new data?"))
            {
                return;
            }

            string strModify = "";
            string strName, strComment, strPass1 = "", strPass2;

            // Name
            if (!CMainFrame.GetKeyboard(out strModify, "Input Name"))
                return;

            if (String.IsNullOrWhiteSpace(strModify))
            {
                CMainFrame.DisplayMsg($"Input Name.");
                return;
            }
            strName = strModify;

            // Model Name 중복 검사
            if (CMainFrame.DataManager.IsModelHeaderExist(strName, ListType))
            {
                CMainFrame.DisplayMsg("already exist same name[Model or Maker]");
                return;
            }

            // Password
            if(ListType == EListHeaderType.USERINFO)
            {
                CMainFrame.GetKeyboard(out strModify, "Input Password", true);
                strPass1 = strModify;

                CMainFrame.GetKeyboard(out strModify, "Input Password Repeat", true);
                strPass2 = strModify;

                if(strPass1 != strPass2)
                {
                    CMainFrame.DisplayMsg("Password are not same.");
                    return;
                }
            }

            // Comment
            CMainFrame.GetKeyboard(out strModify, "Input Comment");
            strComment = strModify;


            // create model header
            CListHeader header = new CListHeader();
            header.Name = strName;
            header.Comment = strComment;
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
                    CMainFrame.Core.SaveModelData(modelData);
                    break;
                case EListHeaderType.WAFERFRAME:
                    CWaferFrameData waferFrameData = ObjectExtensions.Copy(CMainFrame.DataManager.WaferFrameData);
                    waferFrameData.Name = header.Name;
                    CMainFrame.Core.SaveModelData(waferFrameData);
                    break;
                case EListHeaderType.USERINFO:
                    ELoginType Login = ELoginType.OPERATOR;

                    if (strSelMakerName == ELoginType.OPERATOR.ToString()) Login = ELoginType.OPERATOR;
                    if (strSelMakerName == ELoginType.ENGINEER.ToString()) Login = ELoginType.ENGINEER;

                    CUserInfo userInfoData = new CUserInfo(strName, strComment, strPass1, Login);
                    CMainFrame.Core.SaveUserData(userInfoData);
                    break;
            }

            int nRow = GridCtrl.Data.RowCount + 1;
            InitGrid(nRow);

            GridCtrl[nRow, 1].Text = strModify;
            UpdateModelData();
        }

        private void BtnModelDelete_Click(object sender, EventArgs e)
        {
            if (strSelModelName == "" || strSelModelName == null)
            {
                CMainFrame.DisplayMsg($"Need select data.");
                return;
            }

            if (!CMainFrame.InquireMsg($"Delete selected data?"))
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

            UpdateModelData();
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

            if (!CMainFrame.InquireMsg($"Change to selected model?"))
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
            strSelModelName = GridCtrl[e.RowIndex, e.ColIndex].Text;

            SelectGridCell(e.RowIndex, e.ColIndex);
        }

        private void SelectGridCell(int nRow, int nCol)
        {
            for (int i = 0; i < GridCtrl.RowCount; i++)
            {
                GridCtrl[i + 1, 1].BackColor = Color.White;
            }

            GridCtrl[nRow, nCol].BackColor = Color.LightSteelBlue;
        }

        private void UpdateModelData()
        {
            // UI Update
            LabelCurMaker.Text = strSelMakerName;
            LabelCurModel.Text = CurrentUsing_ModelName;

            // Model Data Update
            int nModelCount = 0;
            // 전체 Model List에서 사용자가 선택한 Maker Folder에 속해 있는 모든 Model을 Display 한다.
            foreach(CListHeader header in HeaderList)
            {
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
                        GridCtrl[index + 1, 1].Text = header.Name;
                        GridCtrl[index + 1, 2].Text = header.Comment;
                        index++;
                    }
                }
            }
        }
    }
}
