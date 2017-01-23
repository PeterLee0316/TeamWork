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

            //================================================================================
            // Scan Field Size Set
            SizeF fieldSize = new SizeF(0, 0);
            fieldSize.Width = (float) CMainFrame.DataManager.SystemData_Scan.ScanFieldWidth;
            fieldSize.Height = (float)CMainFrame.DataManager.SystemData_Scan.ScanFieldHeight;
            m_ScanManager.SetFieldSize(fieldSize);                      

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
            //Rectangle pRect = new Rectangle(0, 0, 0, 0);
            ////pRect.X = pnlCanvas.Location.X;
            ////pRect.Y = pnlCanvas.Location.Y;
            //pRect.X = 0;
            //pRect.Y = 0;
            //pRect.Width = pnlCanvas.Width;
            //pRect.Height = pnlCanvas.Height;

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

        private void CWindowForm_Resize(object sender, EventArgs e)
        {
            // 사용하지 않음.
            return;
            SizeF formScale = new SizeF(0, 0);
            Size formSize = new Size(0, 0);
            Point compPos = new Point(0, 0);
            float fontSize = 0;
            

            formSize.Width = this.Width;
            formSize.Height = this.Height;

            formScale.Width = (float)formSize.Width / (float)OriginFormSize.Width;
            formScale.Height = (float)formSize.Height / (float)OriginFormSize.Height;

            var component = GetAllControl(this);
            
            foreach (System.Windows.Forms.Control each in component)
            {
                if (each is Panel)
                {
                    each.Width = (int)((float)each.Width * formScale.Width + 0.5f);
                    each.Height = (int)((float)each.Height * formScale.Height + 0.5f);

                    compPos.X = (int)((float)each.Location.X * formScale.Width + 0.5f);
                    compPos.Y = (int)((float)each.Location.Y * formScale.Height + 0.5f);
                    each.Location = compPos;
                }
                //fontSize = (each.Font.Size * formScale.Height);
                //each.Font = new System.Drawing.Font(each.Font.Name, fontSize);

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
            DrawCanvas.Width = (int)((float)pnlCanvas.Width * BaseZoomFactor + 0.5);// - CANVAS_MARGIN * 2;
            DrawCanvas.Height = (int)((float)pnlCanvas.Height * BaseZoomFactor + 0.5);//; - CANVAS_MARGIN * 2;

            PointF ratioField = new PointF(0, 0);
            ratioField.X = (float)(DrawCanvas.Width - CANVAS_MARGIN ) / BaseScanFieldSize.Width;
            ratioField.Y = (float)(DrawCanvas.Height- CANVAS_MARGIN ) / BaseScanFieldSize.Height;

            // 가로 세로 배율을 같이 적용함 (최소값으로 통일)
            ratioField.X = ratioField.X < ratioField.Y ? ratioField.X : ratioField.Y;
            ratioField.Y = ratioField.X < ratioField.Y ? ratioField.X : ratioField.Y;

            SetCalibFactor(ratioField);
        }
        

        private void ShapeListView_Click(object sender, EventArgs e)
        {
            //try
            //{
            int listNum = m_ScanManager.ObjectList.Count;
            //for (int i = 0; i < ShapeListView.Items.Count-1; i++)
            for (int i = 0; i < listNum; i++)
            {
                //if(m_ScanManager.ObjectList.Count > i)
                m_ScanManager.ObjectList[i].IsSelectedObject = false;
            }

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
                // Dimension
                tooltxtStartPointX.Text = string.Format("{0:F4}", m_ScanManager.ObjectList[nIndex].ptObjectStartPos.dX);
                tooltxtStartPointY.Text = string.Format("{0:F4}", m_ScanManager.ObjectList[nIndex].ptObjectStartPos.dY);
                tooltxtEndPointX.Text = string.Format("{0:F4}", m_ScanManager.ObjectList[nIndex].ptObjectEndPos.dX);
                tooltxtEndPointY.Text = string.Format("{0:F4}", m_ScanManager.ObjectList[nIndex].ptObjectEndPos.dY);
                tooltxtCurrentAngle.Text = string.Format("{0:F4}", m_ScanManager.ObjectList[nIndex].ObjectRotateAngle);
                
                // Properties
                
                tooltxtObjectWidth.Visible = true;
                tooltxtObjectHeight.Visible = true;

                // 중심 위치 

                tooltxtObjectCenterX.Text = string.Format("{0:F4}", m_ScanManager.ObjectList[nIndex].ptObjectCenterPos.dX);
                tooltxtObjectCenterY.Text = string.Format("{0:F4}", m_ScanManager.ObjectList[nIndex].ptObjectCenterPos.dY);
                tooltxtObjectWidth.Text  = string.Format("{0:F4}", m_ScanManager.ObjectList[nIndex].ObjectWidth);
                tooltxtObjectHeight.Text = string.Format("{0:F4}", m_ScanManager.ObjectList[nIndex].ObjectHeight);

                if (m_ScanManager.ObjectList[nIndex].ObjectType == EObjectType.DOT)
                {
                    toolLblWidth.Text = "";
                    toolLblHeigth.Text = "";
                    tooltxtObjectWidth.Visible = false;
                    tooltxtObjectHeight.Visible = false;
                }

                if (m_ScanManager.ObjectList[nIndex].ObjectType == EObjectType.LINE)
                {
                    toolLblWidth.Text = "Length";
                    toolLblHeigth.Text = "";
                    tooltxtObjectHeight.Visible = false;                    
                }

                if (m_ScanManager.ObjectList[nIndex].ObjectType == EObjectType.RECTANGLE ||
                    m_ScanManager.ObjectList[nIndex].ObjectType == EObjectType.ELLIPSE)
                {
                    toolLblWidth.Text = "Width";
                    toolLblHeigth.Text = "Height";
                }

                if (m_ScanManager.ObjectList[nIndex].ObjectType == EObjectType.CIRCLE)
                {
                    toolLblWidth.Text = "Radius";
                    toolLblHeigth.Text = "";
                    tooltxtObjectHeight.Visible = false;

                }

            }
            else
            {
                tooltxtStartPointX.Text = "0";
                tooltxtStartPointY.Text = "0";
                tooltxtEndPointX.Text = "0";
                tooltxtEndPointY.Text = "0";
                tooltxtCurrentAngle.Text = "0";
            }
        }
        

        private void CanvasObjectMove(CPos_XY pPos, double pAngle)
        {
            if (SelectObjectListView < 0) return;         

            foreach(CMarkingObject pObject in m_ScanManager.ObjectList)
            {
                // 선택 되지 않은 것들은 Pass
                if (!pObject.IsSelectedObject) continue;
                
                pObject.MoveObject(pPos);
                ////--------------------------------------------------------------------------------
                //// Angle Rotate
                //objectcurrentAngle = pObject.ObjectRotateAngle;
                //pAngle += objectcurrentAngle;
                //pObject.SetObjectRatateAngle(pAngle);
            }

            ReDrawCanvas();
           // InsetObjectProperty(SelectObjectListView);

        }

        private void FormScanWindow_Load(object sender, EventArgs e)
        {
            DrawCanvas.Show();
            toolBtnZoomAll_Click(sender,e);
        }


        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            //if ((e.Delta / 120) < 0)
            //{
            //    float currentZoom = BaseZoomFactor;
            //    currentZoom *= 1.1f;
            //    m_ScanWindow.ChangeCanvasZoom(currentZoom);
            //}
            //else
            //{
            //    float currentZoom = BaseZoomFactor;
            //    currentZoom /= 1.1f;
            //    m_ScanWindow.ChangeCanvasZoom(currentZoom);
            //}

        }

        private void btnPanLeft_Click(object sender, EventArgs e)
        {
            Point movePos = new Point();
            movePos = GetViewCorner();
            movePos.X -= 1;

            SetViewCorner(movePos);
            DrawCanvas.Invalidate();
        }

        private void btnPanRight_Click(object sender, EventArgs e)
        {
            Point movePos = new Point();
            movePos = GetViewCorner();
            movePos.X += 1;

            SetViewCorner(movePos);
            DrawCanvas.Invalidate();
        }

        private void btnPanUp_Click(object sender, EventArgs e)
        {
            Point movePos = new Point();
            movePos = GetViewCorner();
            movePos.Y -= 1;

            SetViewCorner(movePos);
            DrawCanvas.Invalidate();
        }

        private void btnPanDn_Click(object sender, EventArgs e)
        {
            Point movePos = new Point();
            movePos = GetViewCorner();
            movePos.Y += 1;

            SetViewCorner(movePos);
            DrawCanvas.Invalidate();
        }

        private void tmrView_Tick(object sender, EventArgs e)
        {
            
        }

        private void FormScanWindow_Activated(object sender, EventArgs e)
        {
            //================================================================================
            // Scan Field Size Set
            SizeF fieldSize = new SizeF(0, 0);
            fieldSize.Width = (float)CMainFrame.DataManager.SystemData_Scan.ScanFieldWidth;
            fieldSize.Height = (float)CMainFrame.DataManager.SystemData_Scan.ScanFieldHeight;
            m_ScanManager.SetFieldSize(fieldSize);
        }
        
        private void ribbonControl_Click(object sender, EventArgs e)
        {

        }


        private void ribbonControl_OfficeMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        

        private void toolBtnDot_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.DOT);
        }

        private void toolBtnArc_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.ARC);
        }

        private void toolBtnLine_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.LINE);
        }

        private void toolBtnRectacgle_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.RECTANGLE);
        }

        private void toolBtnCircle_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.CIRCLE);
        }

        private void toolBtnEllipse_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.ELLIPSE);
        }
              

        private void toolBtnSaveBmp_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            int index = 0;

            SaveFileDialog imgSaveDlg = new SaveFileDialog();
            imgSaveDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
            imgSaveDlg.Filter = "BMP(*.bmp)|*.bmp";
            if (imgSaveDlg.ShowDialog() == DialogResult.OK)
            {
                // Scan Field가 300mm 이하일 경우엔.. BMP를 한개만 생성한다.
                if (CMainFrame.DataManager.SystemData_Scan.ScanFieldWidth <= POLYGON_SCAN_FIELD)
                {
                    fileName = imgSaveDlg.FileName;
                    if (CMainFrame.LWDicer.m_MeScanner.SetSizeBmp() != SUCCESS) return;
                    CMainFrame.LWDicer.m_MeScanner.ConvertBmpFile(fileName);
                }

                // Scan Field가 300mm 이상일 경우엔.. BMP를 각각 Field만큼 생성한다.
                if (CMainFrame.DataManager.SystemData_Scan.ScanFieldWidth > POLYGON_SCAN_FIELD)
                {
                    index = imgSaveDlg.FileName.IndexOf('.');
                    fileName = imgSaveDlg.FileName.Insert(index, "-1");
                    if (CMainFrame.LWDicer.m_MeScanner.SetSizeBmp(EScannerIndex.SCANNER1) != SUCCESS) return;
                    CMainFrame.LWDicer.m_MeScanner.ConvertBmpFile(fileName, POLYGON_SCAN_FIELD * 0);

                    index = imgSaveDlg.FileName.IndexOf('.');
                    fileName = imgSaveDlg.FileName.Insert(index, "-2");
                    if (CMainFrame.LWDicer.m_MeScanner.SetSizeBmp(EScannerIndex.SCANNER1) != SUCCESS) return;
                    CMainFrame.LWDicer.m_MeScanner.ConvertBmpFile(fileName, POLYGON_SCAN_FIELD * 1);

                    index = imgSaveDlg.FileName.IndexOf('.');
                    fileName = imgSaveDlg.FileName.Insert(index, "-3");
                    if (CMainFrame.LWDicer.m_MeScanner.SetSizeBmp(EScannerIndex.SCANNER1) != SUCCESS) return;
                    CMainFrame.LWDicer.m_MeScanner.ConvertBmpFile(fileName, POLYGON_SCAN_FIELD * 2);
                }
            }
        }

        private void toolBtnSaveLse_Click(object sender, EventArgs e)
        {
            string filename = string.Empty;
            SaveFileDialog imgSaveDlg = new SaveFileDialog();
            imgSaveDlg.InitialDirectory = CMainFrame.DBInfo.ImageDataDir;
            imgSaveDlg.Filter = "BMP(*.lse)|*.lse";

            if (imgSaveDlg.ShowDialog() == DialogResult.OK)
            {
                filename = imgSaveDlg.FileName;
                //string filepath = string.Format("{0:s}{1:s}.bmp", CMainFrame.DBInfo.ImageDataDir, "Polygon");
                if (CMainFrame.LWDicer.m_MeScanner.CalsSizeBmpByte() != SUCCESS) return;
                CMainFrame.LWDicer.m_MeScanner.SaveScanFile(filename);
            }
        }

        private void toolBtnZoomPlus_Click(object sender, EventArgs e)
        {
            m_ScanWindow.FieldZoomPlus();
        }

        private void toolBtnZoomMinus_Click(object sender, EventArgs e)
        {
            m_ScanWindow.FieldZoomMinus();
        }

        private void toolBtnZoomAll_Click(object sender, EventArgs e)
        {
            m_ScanWindow.FieldZoomAll();
        }

        private void toolBtnZoomDraw_Click(object sender, EventArgs e)
        {
            m_ScanWindow.MouseDragZoom = true;            
        }

        private void toolBtnFont_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.FONT);
        }

        private void toolBtnBmp_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.BMP);
        }

        private void toolBtnDxf_Click(object sender, EventArgs e)
        {
            //m_ScanWindow.SetObjectType(EObjectType.DXF);

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

            m_ScanManager.LoadCadFile(filename);
        }

        private void toolBtnNone_Click(object sender, EventArgs e)
        {
            m_ScanWindow.SetObjectType(EObjectType.NONE);
        }

        private void toolBtnArrayCopy_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Array Copy Object ?")) return;

            int arrayNumX = 0, arrayNumY = 0;
            double arrayGapX = 0.0, arrayGapY = 0.0;

            try
            {
                arrayNumX = Convert.ToInt32(txtCopyNumX.Text);
                arrayGapX = Convert.ToDouble(txtCopyPitchX.Text);
                arrayNumY = Convert.ToInt32(txtCopyNumY.Text);
                arrayGapY = Convert.ToDouble(txtCopyPitchY.Text);
            }
            catch
            {
                // 오류 발생
            }

            try
            {
                if (SelectObjectListView < 0) return;
                if (m_ScanManager.ObjectList[SelectObjectListView] == null) return;

                // BMP 파일은 복사를 하지 않는다.
                if (m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.BMP) return;

                if (SelectObjectListView < 0) return;
                if (ShapeListView.Items.Count <= SelectObjectListView) return;

                if (arrayNumX <= 0 || arrayNumY <= 0) return;
            }
            catch
            {
                // 오류 발생

                return;
            }

            CPos_XY posCenter = new CPos_XY();
            CPos_XY posMove = new CPos_XY();

            CMarkingObject pObject = ObjectExtensions.Copy(m_ScanManager.ObjectList[SelectObjectListView]);

            // 초기 X,Y Axis 값을 초기화 한다.
            posCenter = pObject.ptObjectCenterPos.Copy();

            int iObjectNum = arrayNumX * arrayNumY;

            //Parallel.For(0, BmpScanLine.Length, index => BmpScanLine[index] = 255);

            CMarkingObject[] pGroup = new CMarkingObject[iObjectNum];
            int iGroupCount = 0;
            for (int i = 0; i < arrayNumY; i++)
            {
                // 초기 X Axis 값을 초기화 한다.
                posCenter.dX = pObject.ptObjectCenterPos.dX;
                posMove.dX = 0;

                for (int j = 0; j < arrayNumX; j++)
                //Parallel.For(0, arrayNumX, (int j) =>
                {
                    // 복사 Object가 Group일 경우
                    if (pObject.ObjectType == EObjectType.GROUP)
                    {
                        pObject.IsSelectedObject = false;
                        pGroup[iGroupCount] = ObjectExtensions.Copy(pObject);
                        pGroup[iGroupCount].MoveObject(posMove);
                        iGroupCount++;
                    }
                    else
                    {
                        pObject.IsSelectedObject = false;
                        pGroup[iGroupCount] = m_ScanManager.MakeObject(pObject, posCenter);
                        iGroupCount++;
                    }
                    // X Axis 값을 간격으로 증가시킨다.
                    posCenter.dX += arrayGapX;
                    posMove.dX += arrayGapX;
                }
                // Y Axis 값을 간격으로 증가시킨다.
                posCenter.dY += arrayGapY;
                posMove.dY += arrayGapY;
            }

            // Group을 추가함.
            var start = new CPos_XY();
            var end = new CPos_XY();
            m_ScanManager.AddObject(EObjectType.GROUP, start, end, pGroup);
            m_FormScanner.AddObjectList(m_ScanManager.GetLastObject());

            // 복사된 Object 삭제
            foreach (ListViewItem item in ShapeListView.SelectedItems)
            {
                int nIndex = item.Index;
                m_ScanManager.DeleteObject(nIndex);
                ShapeListView.Items.Remove(item);
            }

            ReDrawCanvas();
        }

        private void toolBtnGroup_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Grouping Object ?")) return;

            if (ShapeListView.Items.Count < 1) return;

            // Select 개수로 Array를 생성함.
            int itemNum = ShapeListView.SelectedItems.Count;
            CMarkingObject[] pGroup = new CMarkingObject[itemNum];

            int groupCount = 0;
            // 선택된 객체만 Group에 삽입함.
            foreach (ListViewItem item in ShapeListView.SelectedItems)
            {
                int nIndex = item.Index;
                m_ScanManager.ObjectList[nIndex].IsSelectedObject = false;
                pGroup[groupCount] = ObjectExtensions.Copy(m_ScanManager.ObjectList[nIndex]);

                groupCount++;
            }

            CPos_XY pStart = new CPos_XY();
            CPos_XY pEnd = new CPos_XY();

            // Group을 추가함.
            m_ScanManager.AddObject(EObjectType.GROUP, pStart, pEnd, pGroup);
            AddObjectList(m_ScanManager.GetLastObject());

            // 그룹화된 Object를 삭제함.
            DeleteObject();
        }

        private void toolBtnUnGroup_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Ungrouping Object ?")) return;

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

        private void toolBtnDelete_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Delete Object ?")) return;

            DeleteObject();
        }

        private void DeleteObject()
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

        private void toolBtnDeleteAll_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Delete Object All?")) return;
            // Manager에서 생성 객체 삭제
            m_ScanManager.DeleteAllObject();

            // ListView에서 삭제
            foreach (ListViewItem item in ShapeListView.Items)
            {
                ShapeListView.Items.Remove(item);
            }

            ReDrawCanvas();
        }
        

        private void toolBtnMoveU_Click(object sender, EventArgs e)
        {
            CPos_XY objectMovePos = new CPos_XY();
            double objectMoveAngle = 0.0;

            try
            {
                objectMovePos.dY = -Convert.ToDouble(tooltxtDistance.Text);

                // Object Move Call
                CanvasObjectMove(objectMovePos, objectMoveAngle);

                InsetObjectProperty(SelectObjectListView);
            }
            catch
            {
                return;
            }
        }

        private void toolBtnMoveD_Click(object sender, EventArgs e)
        {
            CPos_XY objectMovePos = new CPos_XY();
            double objectMoveAngle = 0.0;

            try
            {
                objectMovePos.dY = Convert.ToDouble(tooltxtDistance.Text);

                // Object Move Call
                CanvasObjectMove(objectMovePos, objectMoveAngle);

                InsetObjectProperty(SelectObjectListView);
            }
            catch
            {
                return;
            }
        }

        private void toolBtnMoveL_Click(object sender, EventArgs e)
        {
            CPos_XY objectMovePos = new CPos_XY();
            double objectMoveAngle = 0.0;

            try
            {
                objectMovePos.dX = -Convert.ToDouble(tooltxtDistance.Text);

                // Object Move Call
                CanvasObjectMove(objectMovePos, objectMoveAngle);

                InsetObjectProperty(SelectObjectListView);
            }
            catch
            {
                return;
            }
        }

        private void toolBtnMoveR_Click(object sender, EventArgs e)
        {
            CPos_XY objectMovePos = new CPos_XY();
            double objectMoveAngle = 0.0;

            try
            {
                objectMovePos.dX = Convert.ToDouble(tooltxtDistance.Text);

                // Object Move Call
                CanvasObjectMove(objectMovePos, objectMoveAngle);
                
                InsetObjectProperty(SelectObjectListView);                    
                
            }
            catch
            {
                return;
            }
        }

        private void toolBtnClose_Click(object sender, EventArgs e)
        {
            // 현재 Form 닫기
            this.Hide();
        }

        private void toolBtnRotateCCW_Click(object sender, EventArgs e)
        {
            if (SelectObjectListView < 0) return;

            if(m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.DOT ||
               m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.CIRCLE)
            {
                return;
            }
            
            double currentAngle = Convert.ToDouble(tooltxtCurrentAngle.Text);
            currentAngle -= Convert.ToDouble(tooltxtRotateAngle.Text);

            double widthObject = m_ScanManager.ObjectList[SelectObjectListView].ObjectWidth;
            double widthHeight = m_ScanManager.ObjectList[SelectObjectListView].ObjectHeight;

            CPos_XY currentCenter = new CPos_XY();
            currentCenter = m_ScanManager.ObjectList[SelectObjectListView].ptObjectCenterPos;
            
            m_ScanManager.ObjectList[SelectObjectListView].SetObjectProperty(currentCenter, widthObject, widthHeight, currentAngle);

            InsetObjectProperty(SelectObjectListView);
            ReDrawCanvas();
        }

        private void toolBtnRotateCW_Click(object sender, EventArgs e)
        {
            if (SelectObjectListView < 0) return;

            if (m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.DOT ||
               m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.CIRCLE)
            {
                return;
            }

            double currentAngle = Convert.ToDouble(tooltxtCurrentAngle.Text);
            currentAngle += Convert.ToDouble(tooltxtRotateAngle.Text);

            double widthObject = m_ScanManager.ObjectList[SelectObjectListView].ObjectWidth;
            double widthHeight = m_ScanManager.ObjectList[SelectObjectListView].ObjectHeight;

            CPos_XY currentCenter = new CPos_XY();
            currentCenter = m_ScanManager.ObjectList[SelectObjectListView].ptObjectCenterPos;

            m_ScanManager.ObjectList[SelectObjectListView].SetObjectProperty(currentCenter, widthObject, widthHeight, currentAngle);

            InsetObjectProperty(SelectObjectListView);
            ReDrawCanvas();
        }

        private void toolBtnPropertyChange_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Change Property of Object ?")) return;

            //선택된 것이 없으면 실행하지 않는다.
            if (ShapeListView.Items.Count < 1) return;
            if (SelectObjectListView < 0) return;
            if (ShapeListView.Items.Count <= SelectObjectListView) return;
            if (m_ScanManager.ObjectList[SelectObjectListView] == null) return;
            //Group 타입이면 변경을 하지 않는다
            if (m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.GROUP) return;

            CPos_XY centerPos = new CPos_XY();
            double widthObject,heightObject, angleObject;

            centerPos.dX = Convert.ToDouble(tooltxtObjectCenterX.Text);
            centerPos.dY = Convert.ToDouble(tooltxtObjectCenterY.Text);
            widthObject  = Convert.ToDouble(tooltxtObjectWidth.Text);
            heightObject = Convert.ToDouble(tooltxtObjectHeight.Text);
            angleObject  = Convert.ToDouble(tooltxtCurrentAngle.Text);

            m_ScanManager.ObjectList[SelectObjectListView].SetObjectProperty(centerPos, widthObject, heightObject, angleObject);

            InsetObjectProperty(SelectObjectListView);
            ReDrawCanvas();
        }

        private void toolBtnDimensionChange_Click(object sender, EventArgs e)
        {
            // Circle의 경우엔 Dimension 변경을 적용하지 않는다.
            if (m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.CIRCLE)
            {
                InsetObjectProperty(SelectObjectListView);
                return;
            }

            if (!CMainFrame.InquireMsg("Change Demension of Object ?")) return;

            //선택된 것이 없으면 실행하지 않는다.
            if (ShapeListView.Items.Count < 1) return;
            if (SelectObjectListView < 0) return;
            if (ShapeListView.Items.Count <= SelectObjectListView) return;
            if (m_ScanManager.ObjectList[SelectObjectListView] == null) return;
            //Group 타입이면 변경을 하지 않는다
            if (m_ScanManager.ObjectList[SelectObjectListView].ObjectType == EObjectType.GROUP) return;

            CPos_XY startPos = new CPos_XY();
            CPos_XY endPos = new CPos_XY();

            startPos.dX = Convert.ToDouble(tooltxtStartPointX.Text);
            startPos.dY = Convert.ToDouble(tooltxtStartPointY.Text);
            endPos.dX = Convert.ToDouble(tooltxtEndPointX.Text);
            endPos.dY = Convert.ToDouble(tooltxtEndPointY.Text);

            m_ScanManager.ObjectList[SelectObjectListView].SetObjectProperty(startPos, endPos);
            InsetObjectProperty(SelectObjectListView);

            ReDrawCanvas();
        }



        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


    }


}
