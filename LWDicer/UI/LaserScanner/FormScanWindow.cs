using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using static LWDicer.Layers.DEF_Scanner;
using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;

using LWDicer.Layers;

namespace LWDicer.UI
{
    public partial class FormScanWindow : Form
    {
        private int SelectObjectListView = -1;
        private Size OriginFormSize = new Size(0, 0);
        public WindowCanvas DrawCanvas = new WindowCanvas();

        public bool GridViewMode = true;

        public FormScanWindow()
        {
            InitializeComponent();

            Initailze();
        }
        private void Initailze()
        {
            InitailzeListView();

            OriginFormSize.Width = this.Width;
            OriginFormSize.Height = this.Height;

            // Canvas Form  생성 및 붙이기

            this.DrawCanvas.Location = new System.Drawing.Point(CANVAS_MARGIN, CANVAS_MARGIN);
            DrawCanvas.TopLevel = false;
            this.Controls.Add(DrawCanvas);
            DrawCanvas.Parent = this.pnlCanvas;
            DrawCanvas.Dock = DockStyle.Fill;


            this.OnResize(EventArgs.Empty);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            DrawCanvas.Show();
        }
        public void InitailzeListView()
        {
            this.ShapeListView.View = View.Details;
            //this.ShapeListView
            this.ShapeListView.GridLines = true;
            this.ShapeListView.FullRowSelect = true;
            this.ShapeListView.CheckBoxes = false;

            this.ShapeListView.Columns.Add("No", 30);
            this.ShapeListView.Columns.Add("Name", 60);
            this.ShapeListView.Columns.Add("Use", 50);
            this.ShapeListView.Columns.Add("Tag", 40);

        }

        public void AddObjectList(CMarkingObject shapes)
        {
            if (shapes == null) return;

            ListViewItem item;
            ShapeListView.BeginUpdate();

            item = new ListViewItem(CMarkingObject.CreateSortNum.ToString());

            item.SubItems.Add(shapes.ObjectName);

            item.SubItems.Add("Yes");

            item.SubItems.Add(shapes.ObjectSortFlag.ToString());

            ShapeListView.Items.Add(item);

            ShapeListView.EndUpdate();

        }

        public void ReDrawCanvas()
        {
            Rectangle pRect = new Rectangle(0, 0, 0, 0);
            //pRect.X = pnlCanvas.Location.X;
            //pRect.Y = pnlCanvas.Location.Y;
            pRect.X = 0;
            pRect.Y = 0;
            pRect.Width = pnlCanvas.Width;
            pRect.Height = pnlCanvas.Height;

            DrawCanvas.Invalidate();

        }
        /////////////////////////////////////////////////////////////////////////////////////////

        #region 매뉴 바 동작
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 현재 Form 닫기
            this.Hide();

        }
        

        private void 밝음ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetCanvasColorMode(ECanvasColorStyle.BRIGHT);
            DrawCanvas.BackColor = Color.White;

            // Pen 을 설정하고.. 같은 색으로 Brush를 설정한다.
            SetDrawPen(EDrawPenType.ACTIVE_DARK);
            SetDrawBrush(BaseDrawPen[(int)EDrawPenType.DRAW].Color);
        }

        private void 어두움ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetCanvasColorMode(ECanvasColorStyle.DARK);
            DrawCanvas.BackColor = Color.Black;

            // Pen 을 설정하고.. 같은 색으로 Brush를 설정한다.
            SetDrawPen(EDrawPenType.ACTIVE_BRIGHT);
            SetDrawBrush(BaseDrawPen[(int)EDrawPenType.DRAW].Color);
        }

        private void 생성ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridViewMode = true;
            ReDrawCanvas();
        }

        private void 비활성화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridViewMode = false;
            ReDrawCanvas();
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = string.Empty;
            SaveFileDialog imgSaveDlg = new SaveFileDialog();
            imgSaveDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
            imgSaveDlg.Filter = "DAT(*.dat)|*.dat";
            if (imgSaveDlg.ShowDialog() == DialogResult.OK)
            {
                filename = imgSaveDlg.FileName;

                // BinaryFormatter 방식 저장 ===========================================
                Stream ws = new FileStream(filename, FileMode.Create);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(ws, CMainFrame.LWDicer.m_MeScanner.GetObjectAllList());
                ws.Close();
                ws.Dispose();

                // JsonConvert 방식 저장 ===========================================
                //try
                //{
                //    using (StreamWriter file = File.CreateText(filename))
                //    {
                //        JsonSerializer serializer = new JsonSerializer();
                //        serializer.Serialize(file, CMainFrame.LWDicer.m_MeScanner.m_RefComp.Manager.ObjectList);
                //    }
                //}
                //catch(Exception ex)
                //{

                //}
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = string.Empty;
            var imgOpenDlg = new OpenFileDialog();
            imgOpenDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
            imgOpenDlg.Filter = "DAT(*.dat)|*.dat";
            if (imgOpenDlg.ShowDialog() == DialogResult.OK)
            {
                filename = imgOpenDlg.FileName;

                // BinaryFormatter 방식 읽기 ===========================================
                Stream rs = new FileStream(filename, FileMode.Open);
                BinaryFormatter deserializer = new BinaryFormatter();
                CMainFrame.LWDicer.m_MeScanner.SetObjectAllList( (List<CMarkingObject>)deserializer.Deserialize(rs));
                rs.Close();
                rs.Dispose();

                // JsonConvert 방식 읽기 ===========================================
                //using (StreamReader file = File.OpenText(filename))
                //{
                //    var serializer = new JsonSerializer();
                //    var objectList =
                //        serializer.Deserialize(file,typeof(List<CMarkingObject>)) as List<CMarkingObject>;

                //    CMainFrame.LWDicer.m_MeScanner.m_RefComp.Manager.ObjectList = objectList.
                //}

                // 기존 ListView 삭제
                foreach (ListViewItem item in ShapeListView.Items)
                {
                    ShapeListView.Items.Remove(item);
                }

                // ListView에 신규 Object 추가
                foreach (CMarkingObject shape in CMainFrame.LWDicer.m_MeScanner.GetObjectAllList())
                {
                    CMainFrame.LWDicer.m_MeScanner.AddListView(shape);
                }


            }
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////

        #region 함수
        private void tblPnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void WindowControl_Load(object sender, EventArgs e)
        {

        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void CWindowForm_Resize(object sender, EventArgs e)
        {

            SizeF formScale = new SizeF(0, 0);
            Size formSize = new Size(0, 0);
            Point compPos = new Point(0, 0);
            float fontSize = 0;

            // 해결 했다.

            formSize.Width = this.Width;
            formSize.Height = this.Height;

            formScale.Width = (float)formSize.Width / (float)OriginFormSize.Width;
            formScale.Height = (float)formSize.Height / (float)OriginFormSize.Height;

            var component = GetAllControl(this);
            
            foreach (System.Windows.Forms.Control each in component)
            {
                each.Width = (int)((float)each.Width * formScale.Width + 0.5f);
                each.Height = (int)((float)each.Height * formScale.Height + 0.5f);

                compPos.X = (int)((float)each.Location.X * formScale.Width + 0.5f);
                compPos.Y = (int)((float)each.Location.Y * formScale.Height + 0.5f);
                each.Location = compPos;

                fontSize = (each.Font.Size * formScale.Height);
                each.Font = new System.Drawing.Font(each.Font.Name, fontSize);

                //if (each is Panel)
                //{

                //}
                //else
                //{
                //    fontSize = (each.Font.Size * formScale.Height);
                //    each.Font = new System.Drawing.Font(each.Font.Name, fontSize);
                //}
            }

            OriginFormSize.Width = this.Width;
            OriginFormSize.Height = this.Height;

            SetCanvasSize();
        }

        public IEnumerable<System.Windows.Forms.Control> GetAllControl(System.Windows.Forms.Control control, Type type = null)
        {
            var controls = control.Controls.Cast<System.Windows.Forms.Control>();

            //check the all value, if true then get all the controls
            //otherwise get the controls of the specified type
            if (type == null)
                return controls.SelectMany(ctrl => GetAllControl(ctrl, type)).Concat(controls);
            else
                return controls.SelectMany(ctrl => GetAllControl(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }

        public void SetCanvasSize(Size pSize)
        {
            if (pSize.Width < pnlCanvas.Width - CANVAS_MARGIN ||
                pSize.Height < pnlCanvas.Height - CANVAS_MARGIN)

                SetCanvasSize();

        }

        public int GetCanvasSize(out Size pSize)
        {
            pSize = DrawCanvas.Size;

            return SUCCESS;
        }

        public int GetCanvasPnlSize(out Size pSize)
        {
            pSize = pnlCanvas.Size;

            return SUCCESS;
        }

        public void SetCanvasSize()
        {
            DrawCanvas.Width = (int)((float)pnlCanvas.Width * BaseZoomFactor + 0.5) - CANVAS_MARGIN * 2;
            DrawCanvas.Height = (int)((float)pnlCanvas.Height * BaseZoomFactor + 0.5) - CANVAS_MARGIN * 2;

            PointF ratioField = new PointF(0, 0);
            ratioField.X = (float)DrawCanvas.Width / BaseScanFieldSize.Width;
            ratioField.Y = (float)DrawCanvas.Height / BaseScanFieldSize.Height;

            SetCalibFactor(ratioField);
        }

        private void btnShapeCopy_Click(object sender, EventArgs e)
        {

        }

        private void btnShapeInsert_Click(object sender, EventArgs e)
        {

        }

        private void btnShapeDelete_Click(object sender, EventArgs e)
        {
            if (ShapeListView.Items.Count < 1) return;
            foreach (ListViewItem item in ShapeListView.SelectedItems)
            {
                int nIndex = item.Index;
                m_ScanManager.DeleteObject(nIndex);
                ShapeListView.Items.Remove(item);
            }

            ReDrawCanvas();
        }

        private void btnObjectGroup_Click(object sender, EventArgs e)
        {
            if (ShapeListView.Items.Count < 1) return;

            // Select 개수로 Array를 생성함.
            int itemNum = ShapeListView.SelectedItems.Count;
            CMarkingObject[] pGroup = new CMarkingObject[itemNum];

            int groupCount = 0;
            foreach (ListViewItem item in ShapeListView.SelectedItems)
            {
                int nIndex = item.Index;
                m_ScanManager.ObjectList[nIndex].IsSelectedObject = false;
                pGroup[groupCount] = m_ScanManager.ObjectList[nIndex];

                groupCount++;
            }

            PointF pStart = new PointF(0, 0);
            PointF pEnd = new PointF(0, 0);

            m_ScanManager.AddObject(EObjectType.GROUP, pStart, pEnd, pGroup);
            AddObjectList(m_ScanManager.GetLastObject());

            btnShapeDelete_Click(sender, e);
        }

        private void btnObjectUngroup_Click(object sender, EventArgs e)
        {
            if (ShapeListView.Items.Count < 1) return;

            CMarkingObject[] pGroup = new CMarkingObject[3];

            int groupCount = 0;
            foreach (ListViewItem item in ShapeListView.SelectedItems)
            {
                int nIndex = item.Index;

                // Group가 아니면... 빠져나감.
                if (m_ScanManager.ObjectList[nIndex].ObjectType != EObjectType.GROUP) continue;
                // Group의 객체 수를 읽어서.. Array Size를 다시 설정함.
                Array.Resize<CMarkingObject>(ref pGroup, m_ScanManager.ObjectList[nIndex].GroupObjectCount);

                while (m_ScanManager.ObjectList[nIndex].GroupObjectCount > 0)
                {
                    pGroup[groupCount] = m_ScanManager.ObjectList[nIndex].PullGroupObject();
                    m_ScanManager.AddObject(pGroup[groupCount]);
                    AddObjectList(pGroup[groupCount]);

                    groupCount++;
                }

                m_ScanManager.DeleteObject(nIndex);
                ShapeListView.Items.Remove(item);
                groupCount = 0;
            }

            ReDrawCanvas();

        }

        private void btnShapeDeleteAll_Click(object sender, EventArgs e)
        {
            // Manager에서 생성 객체 삭제
            m_ScanManager.DeleteAllObject();

            // ListView에서 삭제
            foreach (ListViewItem item in ShapeListView.Items)
            {
                ShapeListView.Items.Remove(item);
            }

            ReDrawCanvas();

        }

        private void ShapeListView_Click(object sender, EventArgs e)
        {
            //try
            //{
            int listNum = m_ScanManager.ObjectList.Count;
                //for (int i = 0; i < ShapeListView.Items.Count-1; i++)
                for (int i = 0; i < listNum; i++)
                    //if(m_ScanManager.ObjectList.Count > i)
                    m_ScanManager.ObjectList[i].IsSelectedObject = false;

                if (ShapeListView.Items.Count < 1)
                {
                    InsetObjectProperty(SelectObjectListView);
                    return;
                }

                var SelectCol = ShapeListView.SelectedIndices;

                for (int i = SelectCol.Count - 1; i >= 0; i--)
                {
                    SelectObjectListView = SelectCol[i];
                    m_ScanManager.ObjectList[SelectObjectListView].IsSelectedObject = true;
                }

                ReDrawCanvas();

                InsetObjectProperty(SelectObjectListView);
            //}
            //catch
            //{ }
        }


        private void ShapeListView_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void InsetObjectProperty(int nIndex)
        {
            if (nIndex >= 0)
            {
                txtObjectStartPosX.Text = string.Format("{0:F3}", m_ScanManager.ObjectList[nIndex].ptObjectStartPos.X);
                txtObjectStartPosY.Text = string.Format("{0:F3}", m_ScanManager.ObjectList[nIndex].ptObjectStartPos.Y);
                txtObjectEndPosX.Text = string.Format("{0:F3}", m_ScanManager.ObjectList[nIndex].ptObjectEndPos.X);
                txtObjectEndPosY.Text = string.Format("{0:F3}", m_ScanManager.ObjectList[nIndex].ptObjectEndPos.Y);
                txtObjectAngle.Text = string.Format("{0:F3}", m_ScanManager.ObjectList[nIndex].ObjectRotateAngle);

            }
            else
            {
                txtObjectStartPosX.Text = "0";
                txtObjectStartPosY.Text = "0";
                txtObjectEndPosX.Text = "0";
                txtObjectEndPosY.Text = "0";
                txtObjectAngle.Text = "0";
            }
        }

        private void btnObjectChange_Click(object sender, EventArgs e)
        {
            //선택된 것이 없으면 실행하지 않는다.
            if (ShapeListView.Items.Count < 1) return;
            if (SelectObjectListView < 0) return;
            if (ShapeListView.Items.Count <= SelectObjectListView) return;
            if (m_ScanManager.ObjectList[SelectObjectListView] == null) return;
            //Group 타입이면 변경을 하지 않는다
            if (m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.GROUP) return;

            PointF pPos = new PointF(0, 0);

            pPos.X = float.Parse(txtObjectStartPosX.Text);
            pPos.Y = float.Parse(txtObjectStartPosY.Text);
            m_ScanManager.ObjectList[SelectObjectListView].SetObjectStartPos(pPos);

            pPos.X = float.Parse(txtObjectEndPosX.Text);
            pPos.Y = float.Parse(txtObjectEndPosY.Text);
            m_ScanManager.ObjectList[SelectObjectListView].SetObjectEndPos(pPos);

            m_ScanManager.ObjectList[SelectObjectListView].SetObjectRatateAngle(float.Parse(txtObjectAngle.Text));

            ReDrawCanvas();

            //---------------------------
            // 변경
            stObjectInfo objectInfo = new stObjectInfo();
            objectInfo.SelectNum = SelectObjectListView;
            objectInfo.pStartPos = pPos;

            objectInfo.pEndPos = pPos;
            objectInfo.pAngle = float.Parse(txtObjectAngle.Text);

        }

        private void CanvasObjectMove(PointF pPos, float pAngle)
        {
            if (SelectObjectListView < 0) return;

            PointF objectCurrentPos = new PointF(0f, 0f);
            float objectcurrentAngle = 0f;

            m_ScanManager.ObjectList[SelectObjectListView].MoveObject(pPos);
            //--------------------------------------------------------------------------------
            // Angle Rotate
            objectcurrentAngle = m_ScanManager.ObjectList[SelectObjectListView].ObjectRotateAngle;
            pAngle += objectcurrentAngle;
            m_ScanManager.ObjectList[SelectObjectListView].SetObjectRatateAngle(pAngle);

            ReDrawCanvas();
            InsetObjectProperty(SelectObjectListView);

        }

        private void btnObjectMove_Click(object sender, EventArgs e)
        {
            PointF objectMovePos = new PointF(0f, 0f);
            float objectMoveAngle = 0f;

            try
            {
                objectMovePos.X = float.Parse(txtObjectMoveX.Text);
                objectMovePos.Y = float.Parse(txtObjectMoveY.Text);
                objectMoveAngle = float.Parse(txtObjectMoveT.Text);

                // Object Move Call
                CanvasObjectMove(objectMovePos, objectMoveAngle);
            }
            catch
            {
                return;
            }

        }

        private void btnObjectMoveUp_Click(object sender, EventArgs e)
        {
            PointF objectMovePos = new PointF(0f, 0f);
            float objectMoveAngle = 0f;

            try
            {
                objectMovePos.Y = -float.Parse(txtObjectMoveY.Text);

                // Object Move Call
                CanvasObjectMove(objectMovePos, objectMoveAngle);
            }
            catch
            {
                return;
            }
        }

        private void btnObjectMoveDn_Click(object sender, EventArgs e)
        {
            PointF objectMovePos = new PointF(0f, 0f);
            float objectMoveAngle = 0f;

            try
            {
                objectMovePos.Y = float.Parse(txtObjectMoveY.Text);

                // Object Move Call
                CanvasObjectMove(objectMovePos, objectMoveAngle);
            }
            catch
            {
                return;
            }
        }

        private void btnObjectMoveLeft_Click(object sender, EventArgs e)
        {
            PointF objectMovePos = new PointF(0f, 0f);
            float objectMoveAngle = 0f;
            try
            {
                objectMovePos.X = -float.Parse(txtObjectMoveX.Text);

                // Object Move Call
                CanvasObjectMove(objectMovePos, objectMoveAngle);
            }
            catch
            {
                return;
            }
        }

        private void btnObjectMoveRight_Click(object sender, EventArgs e)
        {
            PointF objectMovePos = new PointF(0f, 0f);
            float objectMoveAngle = 0f;
            try
            {
                objectMovePos.X = float.Parse(txtObjectMoveX.Text);

                // Object Move Call
                CanvasObjectMove(objectMovePos, objectMoveAngle);
            }
            catch
            {
                return;
            }
        }


        private void btnObjectArrayCopy_Click(object sender, EventArgs e)
        {

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void WindowControl_Load_1(object sender, EventArgs e)
        {

        }

        private void penToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void btnObjectArrayCopy_Click_1(object sender, EventArgs e)
        {
            if (SelectObjectListView < 0) return;
            if (ShapeListView.Items.Count <= SelectObjectListView) return;

            int arrayNumX = int.Parse(txtArrayNumX.Text);
            float arrayGapX = float.Parse(txtArrayGapX.Text);
            int arrayNumY = int.Parse(txtArrayNumY.Text);
            float arrayGapY = float.Parse(txtArrayGapY.Text);

            PointF posStart = new PointF(0, 0);
            PointF posEnd = new PointF(0, 0);
            PointF posMove = new PointF(0, 0);

            // 초기 X,Y Axis 값을 초기화 한다.
            posStart = m_ScanManager.ObjectList[SelectObjectListView].ptObjectStartPos;
            posEnd = m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.DOT ?
                       m_ScanManager.ObjectList[SelectObjectListView].ptObjectStartPos :
                       m_ScanManager.ObjectList[SelectObjectListView].ptObjectEndPos;

            for (int i = 0; i < arrayNumY; i++)
            {
                // 초기 X Axis 값을 초기화 한다.
                posStart.X = m_ScanManager.ObjectList[SelectObjectListView].ptObjectStartPos.X;
                posEnd.X = m_ScanManager.ObjectList[SelectObjectListView].ptObjectEndPos.X;
                posMove.X = 0;

                for (int j = 0; j < arrayNumX; j++)
                {
                    // 첫 번째는 복사에서 제외한다.
                    if (i == 0 && j == 0)
                    {
                        posStart.X += arrayGapX;
                        posEnd.X += arrayGapX;
                        posMove.X += arrayGapX;
                        continue;
                    }

                    if (m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.GROUP)
                    {
                        m_ScanManager.AddObject(m_ScanManager.ObjectList[SelectObjectListView]);
                        m_ScanManager.GetLastObject().MoveObject(posMove);
                    }
                    else
                    {
                        m_ScanManager.AddObject(m_ScanManager.ObjectList[SelectObjectListView].ObjectType, posStart, posEnd);
                    }

                    AddObjectList(m_ScanManager.GetLastObject());

                    // X Axis 값을 간격으로 증가시킨다.
                    posStart.X += arrayGapX;
                    posEnd.X += arrayGapX;
                    posMove.X += arrayGapX;
                }
                // Y Axis 값을 간격으로 증가시킨다.
                posStart.Y += arrayGapY;
                posEnd.Y += arrayGapY;
                posMove.Y += arrayGapY;
            }

            ReDrawCanvas();
        }


        private void btnLaserRun_Click(object sender, EventArgs e)
        {

        }


        private void polygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // m_FormPolgon.ShowDialog();
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.DOT);

        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.LINE);
        }

        private void btnRect_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.RECTANGLE);
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.ELLIPSE);
        }


        private void btnFont_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.FONT);
        }

        private void btnBmp_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.BMP);
        }

        private void btnDxf_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.DXF);
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.NONE);
        }

        private void FormScanWindow_Load(object sender, EventArgs e)
        {
            //   DrawCanvas.Show();
        }

        private void btnLaserStop_Click(object sender, EventArgs e)
        {

        }

        private void btnImageSave_Click(object sender, EventArgs e)
        {
            string filename = string.Empty;
            SaveFileDialog imgSaveDlg = new SaveFileDialog();
            imgSaveDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
            imgSaveDlg.Filter = "BMP(*.bmp)|*.bmp";
            if (imgSaveDlg.ShowDialog() == DialogResult.OK)
            {
                filename = imgSaveDlg.FileName;
                //string filepath = string.Format("{0:s}{1:s}.bmp", CMainFrame.DBInfo.ImageDataDir, "Polygon");

                CMainFrame.LWDicer.m_MeScanner.SetSizeBmp();
                CMainFrame.LWDicer.m_MeScanner.ConvertBmpFile(filename);
            }
        }


        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


    }


}
