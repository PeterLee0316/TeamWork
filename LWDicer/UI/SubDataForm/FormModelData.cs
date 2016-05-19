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

namespace LWDicer.UI
{
    public partial class FormModelData : Form
    {
        private int nTreeIndex;
        private string strSelName;

        public FormModelData()
        {
            InitializeComponent();

            nTreeIndex = 0;

            InitMakerTreeView();

            InitGrid();

        }


        private void InitMakerTreeView()
        {
            MakerlTreeView.ItemHeight = 80;

            MakerlTreeView.ImageIndex = 1;

        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridModelList.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridModelList.Properties.RowHeaders = false;
            GridModelList.Properties.ColHeaders = false;

            nCol = 1;
            nRow = 1;

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
            string strCurModel = string.Empty, strCurMaker = string.Empty;

            int nMakerCount = 0, i = 0;

            strCurModel = CMainFrame.LWDicer.m_DataManager.ModelData.Name;

            LabelCurMaker.Text = strCurModel;

            nMakerCount = CMainFrame.LWDicer.m_DataManager.ModelList.Count;

            string strMaker = string.Empty;

            for(i=0;i<nMakerCount;i++)
            {
                strMaker = CMainFrame.LWDicer.m_DataManager.ModelList[i].Name;

                MakerlTreeView.Nodes.Add(strMaker);
            }
        }

        private void FormModelData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void MakerlTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            nTreeIndex = e.Node.Index;
            strSelName = e.Node.Text;
        }

        private void BtnMakerCreate_Click(object sender, EventArgs e)
        {

            if (!CMainFrame.LWDicer.DisplayMsg("새로운 Maker를 생성 하시겠습니까?"))
            {
                return;
            }

            string strModify = "";

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify))
            {
                return;
            }

            MakerlTreeView.Nodes.Add(strModify);
        }

        private void BtnMakerDelete_Click(object sender, EventArgs e)
        {
            if(MakerlTreeView.Nodes.Count==0)
            {
                return;
            }

            if (!CMainFrame.LWDicer.DisplayMsg("선택하신 Maker를 삭제 시겠습니까?"))
            {
                return;
            }

            MakerlTreeView.Nodes.RemoveAt(nTreeIndex);
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





        }

        private void BtnModelDelete_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("선택하신 Model Data를 삭제 시겠습니까?"))
            {
                return;
            }
        }

        private void BtnModelSelect_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("선택하신 Model Data를 실행 하시겠습니까?"))
            {
                return;
            }
        }

        private void GridModelList_CellClick(object sender, GridCellClickEventArgs e)
        {

        }
    }
}
