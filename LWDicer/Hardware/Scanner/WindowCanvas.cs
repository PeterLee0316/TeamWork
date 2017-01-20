using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_Scanner;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_System;

using LWDicer.UI;

namespace LWDicer.Layers
{
    public partial class WindowCanvas : Form
    {
        private Point ptMouseStartPos = new Point(0, 0);
        private Point ptMouseEndPos = new Point(0, 0);
        private Point ptArcStartPos = new Point(0, 0);
        private Point ptArcEndPos = new Point(0, 0);

        private bool IsObjectDrag = false;
        
        public WindowCanvas()
        {
            InitializeComponent();
            Initialize();
        }

        public int Initialize()
        {
            // 배경 화면 설정
            if (BaseCanavsColorMode == ECanvasColorStyle.BRIGHT)
            {
                this.BackColor = Color.LightGray;
                // Pen 을 설정하고.. 같은 색으로 Brush를 설정한다.
                SetDrawPen(EDrawPenType.ACTIVE_BRIGHT);
                SetDrawBrush(BaseDrawPen[(int)EDrawPenType.DRAW].Color);
            }
            else
            {
                this.BackColor = Color.Black;
                // Pen 을 설정하고.. 같은 색으로 Brush를 설정한다.
                SetDrawPen(EDrawPenType.ACTIVE_BRIGHT);
                SetDrawBrush(BaseDrawPen[(int)EDrawPenType.DRAW].Color);
            }

            // 더블 버퍼링 설정
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            return SUCCESS;

        }

        #region Canvas 이벤트 , 그리기 삭제
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.Focus();
            //m_ScanWindow.SetObjectType(EObjectType.LINE);

            if (e.Button == MouseButtons.Left)  // 마우스 왼쪽 버튼
            {
                ptMouseStartPos = e.Location;
                
                if (m_ScanWindow.SelectObjectType != EObjectType.NONE)
                {
                    m_ScanWindow.SetObjectStartPos(PixelToField(ptMouseStartPos));

                    if (m_ScanWindow.SelectObjectType == EObjectType.DOT)
                    {
                        IsObjectDrag = true;
                        m_ScanWindow.SetObjectEndPos(new CPos_XY());
                        this.Invalidate();
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)   // 마우스 오른쪽 버튼
            {
                
            }
            else if (e.Button == MouseButtons.Middle)   // 마우스 가운데 버튼
            {
                // Cursor 변경
                this.Cursor = Cursors.Hand;

                // Mouse 위치 기록
                m_ScanWindow.ptPanStartPos = e.Location;
                m_ScanWindow.ptCurrentViewCorner = GetViewCorner();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            
            if (e.Button == MouseButtons.Left && m_ScanWindow.SelectObjectType != EObjectType.NONE)
            {
                // 시작 포인트와 끝 포이트가 같으면 Shape를 생성하지 않는다.
                // 단  Dot의 경우는 제외함.
                if (ptMouseStartPos == ptMouseEndPos && m_ScanWindow.SelectObjectType != EObjectType.DOT) return;

                // Arc의 경우엔 여러번 클릭해야 함.
                if (m_ScanWindow.SelectObjectType != EObjectType.ARC)
                {
                    IsObjectDrag = false;
                    // Add Object                
                    m_ScanWindow.AddObject();

                    Invalidate();
                }
            }
            

            // Mouse Drag Zoom을 확인한다.
            if(m_ScanWindow.MouseDragZoom)
            {
                Rectangle zoomSize = new Rectangle();
                Point startPos = new Point();
                Point endPos = new Point();

                startPos = ptMouseStartPos;
                endPos = ptMouseEndPos;                

                zoomSize.X = startPos.X < endPos.X ? startPos.X : endPos.X;
                zoomSize.Y = startPos.Y < endPos.Y ? startPos.Y : endPos.Y;
                zoomSize.Width  = startPos.X > endPos.X ? (startPos.X - endPos.X) : -(startPos.X - endPos.X);
                zoomSize.Height = startPos.Y > endPos.Y ? (startPos.Y - endPos.Y) : -(startPos.Y - endPos.Y);

                m_ScanWindow.SelectFieldZoom(zoomSize);
            }

            // 마우스 커서를 원위치 함.
            this.Cursor = Cursors.Arrow;

            // Mouse Drag Zoom을 해제한다.
            m_ScanWindow.MouseDragZoom = false;

            // Object가 선택 되지 않을 경우에 
            if (m_ScanWindow.SelectObjectType == EObjectType.NONE)
            {
                ptMouseStartPos = ptMouseEndPos;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            m_ScanWindow.ptMousePos = PixelToField(e.Location);

            if (m_ScanWindow.MouseDragZoom) this.Cursor = Cursors.Cross;
            
            if (e.Button == MouseButtons.Left)
            {
                ptMouseEndPos = e.Location;

                if (m_ScanWindow.SelectObjectType != EObjectType.NONE)
                    m_ScanWindow.SetObjectEndPos(PixelToField(ptMouseEndPos));

                IsObjectDrag = true;
                this.Invalidate();
            }
            else if (e.Button == MouseButtons.Middle)   // 마우스 가운데 버튼
            {
                // Mouse Pan Drag 동작 (마우스로 Field를 이동함)
                Point moveLength = new Point();

                // Mouse Click위치에서 이동 거리 측정
                moveLength.X = m_ScanWindow.ptPanStartPos.X - e.Location.X;
                moveLength.Y = m_ScanWindow.ptPanStartPos.Y - e.Location.Y;
                
                Point pPoint = new Point();
                
                // 현재 ViewCorner 읽기
                pPoint = m_ScanWindow.ptCurrentViewCorner;
                // 이동 거리 적용
                pPoint.X -= moveLength.X;
                pPoint.Y -= moveLength.Y;

                SetViewCorner(pPoint);

                this.Invalidate();

            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            m_ScanWindow.SetObjectType(EObjectType.NONE);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Shift Key ------------------------
            if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                m_ScanWindow.MouseDragZoom = true;
                this.Cursor = Cursors.Cross;

                m_ScanWindow.SetObjectType(EObjectType.NONE);
            }
            // Esc Key ------------------------
            if (e.KeyData == Keys.Escape)
                m_ScanWindow.SetObjectType(EObjectType.NONE);

        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);


            // Shift Key ------------------------
            if (e.KeyData == Keys.ShiftKey)
            {
                m_ScanWindow.MouseDragZoom = false;
                this.Cursor = Cursors.Arrow;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            // Wheel Zoom
            
            if ((e.Delta / 120) < 0)
            {
                float currentZoom = BaseZoomFactor;
                float changeZoom = 1.1f;
                currentZoom *= changeZoom;                    

                if (m_ScanWindow.ChangeCanvasZoom(currentZoom) != SUCCESS) return;
                m_ScanWindow.MoveFieldPointView(e.Location, changeZoom);
            }
            else
            {
                float currentZoom = BaseZoomFactor;
                float changeZoom = 1.1f;
                currentZoom /= changeZoom;

                if (m_ScanWindow.ChangeCanvasZoom(currentZoom) != SUCCESS) return;
                m_ScanWindow.MoveFieldPointView(e.Location, 1/changeZoom);
            }
            return;

            base.OnMouseWheel(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Painting 속도를 높이기 위해서 Buffer에 graphic을 그린 다음 한번에 랜더링 처리함.
            using (BufferedGraphics bufferedgraphic = BufferedGraphicsManager.Current.Allocate(e.Graphics, this.ClientRectangle))
            {
                bufferedgraphic.Graphics.Clear(Color.Black);
                bufferedgraphic.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                bufferedgraphic.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bufferedgraphic.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

                // Grid 그리기
                GridDraw(bufferedgraphic);

                if (m_ScanWindow == null || m_ScanManager == null) return;

                // 기존 Object 그리기
                m_ScanManager.DrawObject(bufferedgraphic);

                // 현재 Drag 모양 그리기
                if (IsObjectDrag)
                    DragShapeDraw(bufferedgraphic);

                bufferedgraphic.Render(e.Graphics);
            }            
        }

        private void DragShapeDraw(BufferedGraphics bg)
        {
            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);
            int radius = 0;
            Rectangle rectCircle = new Rectangle();

            StartPos = ptMouseStartPos;
            EndPos = ptMouseEndPos;

            switch (m_ScanWindow.SelectObjectType)
            {
                case EObjectType.NONE:
                    bg.Graphics.DrawRectangle(BaseDrawPen[(int)EDrawPenType.SELECT],
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
                    break;
                case EObjectType.BMP:
                    bg.Graphics.DrawRectangle(BaseDrawPen[(int)EDrawPenType.SELECT],
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
                    break;

                case EObjectType.DOT:
                    Rectangle rectDot = new Rectangle(StartPos.X - DRAW_DOT_SIZE / 2, 
                                                      StartPos.Y - DRAW_DOT_SIZE / 2,
                                                      DRAW_DOT_SIZE, 
                                                      DRAW_DOT_SIZE);
                    bg.Graphics.FillRectangle(BaseDrawBrush[(int)EDrawBrushType.OBJECT_DRAG], rectDot);
                    break;

                case EObjectType.LINE:
                    bg.Graphics.DrawLine(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG], StartPos, EndPos);
                    break;

                case EObjectType.ARC:
                    radius = (int)Math.Sqrt(Math.Pow((StartPos.X - EndPos.X), 2) +
                                                Math.Pow((StartPos.Y - EndPos.Y), 2));

                    rectCircle.X = (StartPos.X - radius);
                    rectCircle.Y = (StartPos.Y - radius);
                    rectCircle.Width = radius * 2;
                    rectCircle.Height = radius * 2;

                    bg.Graphics.DrawArc(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG], rectCircle,0,379);
                    break;

                case EObjectType.RECTANGLE:
                    bg.Graphics.DrawRectangle(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG],
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
                    break;

                case EObjectType.CIRCLE:
                    radius = (int)Math.Sqrt(Math.Pow((StartPos.X - EndPos.X), 2) +
                                            Math.Pow((StartPos.Y - EndPos.Y), 2));
                   
                    rectCircle.X = (StartPos.X - radius);
                    rectCircle.Y = (StartPos.Y - radius);
                    rectCircle.Width = radius * 2;
                    rectCircle.Height = radius * 2;

                    bg.Graphics.DrawEllipse(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG], rectCircle);

                    //// X축 기준으로 진행함
                    //g.DrawEllipse(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG],
                    //        (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                    //        (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                    //        (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                    //        (StartPos.Y > EndPos.Y ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)));
                    break;

                case EObjectType.ELLIPSE:
                    bg.Graphics.DrawEllipse(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG],
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
                    break;

                default:

                    break;
            }

            //g.Dispose();
        }

        private void GridDraw(BufferedGraphics bg)
        {
            Point pixelStartPos = new Point(0, 0);
            Point pixelEndPos = new Point(0, 0);
            CPos_XY objectStartPos = new CPos_XY();
            CPos_XY objectEndPos = new CPos_XY();            
            Size ScanFieldSize = new Size(0, 0);            

            int gridMinor = 5;
            int gridMajor = gridMinor * 5;

            // POLYGON_SCAN_FIELD
            ScanFieldSize.Width = CMainFrame.DataManager.SystemData_Scan.ScanFieldWidth;
            ScanFieldSize.Height = CMainFrame.DataManager.SystemData_Scan.ScanFieldHeight;
            
            Pen drawPen = new Pen(System.Drawing.Color.FromArgb(30, 30, 30));

            // Minor Grid Draw ==============================================
            #region Minor Grid
            for (int i = 0; i < ScanFieldSize.Width; i += gridMinor)
            {
                objectStartPos.dX = (double)i;
                objectStartPos.dY = 0.0;
                pixelStartPos = AbsFieldToPixel(objectStartPos);

                objectEndPos.dX = (double)i;
                objectEndPos.dY = (double)ScanFieldSize.Height;
                pixelEndPos = AbsFieldToPixel(objectEndPos);

                bg.Graphics.DrawLine(drawPen, pixelStartPos, pixelEndPos);     

            }

            for (int i = 0; i < ScanFieldSize.Height; i += gridMinor)
            {
                objectStartPos.dX = 0.0;
                objectStartPos.dY = (double)i;
                pixelStartPos = AbsFieldToPixel(objectStartPos);

                objectEndPos.dX = (double)ScanFieldSize.Width;
                objectEndPos.dY = (double)i;
                pixelEndPos = AbsFieldToPixel(objectEndPos);

                bg.Graphics.DrawLine(drawPen, pixelStartPos, pixelEndPos);
            }
            #endregion

            // Major Grid Draw ==============================================

            int scanNum = 0;
            double startPos = 0;
            for (double scanField = 0.0; scanField < ScanFieldSize.Width; scanField += POLYGON_SCAN_FIELD)
            {
                // Color Change
                if (scanNum == 0)
                {
                    drawPen.Color = System.Drawing.Color.FromArgb(30, 30, 70);
                    //startPos = 
                }
                if (scanNum == 1)
                {
                    drawPen.Color = System.Drawing.Color.FromArgb(30, 50, 30);
                }
                if (scanNum == 2)
                {
                    drawPen.Color = System.Drawing.Color.FromArgb(50, 30, 30);
                }
                
                for (int i = (int)scanField; i <= scanField + POLYGON_SCAN_FIELD; i += gridMajor)
                {
                    objectStartPos.dX = (double)i;
                    objectStartPos.dY = 0.0;
                    pixelStartPos = AbsFieldToPixel(objectStartPos);

                    objectEndPos.dX = (double)i;
                    objectEndPos.dY = (double)ScanFieldSize.Height;
                    pixelEndPos = AbsFieldToPixel(objectEndPos);

                    bg.Graphics.DrawLine(drawPen, pixelStartPos, pixelEndPos);

                }

                for (int i = 0; i <= ScanFieldSize.Height; i += gridMajor)
                {
                    objectStartPos.dX = scanField;
                    objectStartPos.dY = (double)i;
                    pixelStartPos = AbsFieldToPixel(objectStartPos);

                    objectEndPos.dX = scanField + POLYGON_SCAN_FIELD;
                    objectEndPos.dY = (double)i;
                    pixelEndPos = AbsFieldToPixel(objectEndPos);

                    bg.Graphics.DrawLine(drawPen, pixelStartPos, pixelEndPos);
                }                    
                scanNum++;
            }

            drawPen.Dispose();            
        }
        

        #endregion

        private void CWindowCanvas_SizeChanged(object sender, EventArgs e)
        {
            // Canvas Size 설정
            SetCanvasSize(this.Size);

            // Canvas Center 설정
            Point pPoint = new Point(this.Size.Width / 2, this.Size.Height / 2);

            SetBaseCenter(pPoint);

            // Scan Field Size 설정
            //Size pSize = new Size( (int)((float)this.Size.Width  * BaseZoomFactor * SCAN_FIELD_RATIO), 
            //                       (int)((float)this.Size.Height * BaseZoomFactor * SCAN_FIELD_RATIO));

            //SetScanFieldSize(pSize);
        }

        private void CWindowCanvas_Paint(object sender, PaintEventArgs e)
        {

        }

        private void WindowCanvas_Load(object sender, EventArgs e)
        {

        }

        private void tmrView_Tick(object sender, EventArgs e)
        {

        }
    }

}
