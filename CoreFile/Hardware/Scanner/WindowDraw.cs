using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Core.Layers.DEF_Scanner;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_System;
using Core.UI;

namespace Core.Layers
{
    public partial class WindowDraw : UserControl
    {
        private Point ptMouseStartPos = new Point(0, 0);
        private Point ptMouseEndPos = new Point(0, 0);
        private bool IsObjectDrag = false;
        protected bool CheckWheelZoomMode = false;

        public WindowDraw()
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

            return SUCCESS;
        }

        #region Canvas 이벤트 , 그리기 삭제
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

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
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left && m_ScanWindow.SelectObjectType != EObjectType.NONE)
            {
                // 시작 포인트와 끝 포이트가 같으면 Shape를 생성하지 않는다.
                if (ptMouseStartPos == ptMouseEndPos && m_ScanWindow.SelectObjectType != EObjectType.DOT) return;

                IsObjectDrag = false;

                // Add Object                
                m_ScanWindow.AddObject();

                Invalidate();
            }

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

            if (e.Button == MouseButtons.Left)
            {
                ptMouseEndPos = e.Location;

                if (m_ScanWindow.SelectObjectType != EObjectType.NONE)
                    m_ScanWindow.SetObjectEndPos(PixelToField(ptMouseEndPos));

                IsObjectDrag = true;
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

            // Ctrl Key ------------------------
            if ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control)
                CheckWheelZoomMode = true;

            // Esc Key ------------------------
            if (e.KeyData == Keys.Escape)
                m_ScanWindow.SetObjectType(EObjectType.NONE);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            // Ctrl Key ------------------------
            // if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            if (e.KeyData == Keys.ControlKey)
                CheckWheelZoomMode = false;
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            // Wheel Zoom
            if (CheckWheelZoomMode)
            {
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
                    m_ScanWindow.MoveFieldPointView(e.Location, 1 / changeZoom);
                }
                return;
            }
            base.OnMouseWheel(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 더블 버퍼링을 위한 코드
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

                    bg.Graphics.DrawArc(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG], rectCircle, 0, 379);
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

            // POLYGON_SCAN_WIDTH
            ScanFieldSize.Width = (int)CMainFrame.DataManager.SystemData_Scan.ScanFieldWidth;
            ScanFieldSize.Height= (int)CMainFrame.DataManager.SystemData_Scan.ScanFieldHeight;

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
            for (double scanField = 0.0; scanField < ScanFieldSize.Width; scanField += POLYGON_SCAN_WIDTH)
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

                for (int i = (int)scanField; i <= scanField + POLYGON_SCAN_WIDTH; i += gridMajor)
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

                    objectEndPos.dX = scanField + POLYGON_SCAN_WIDTH;
                    objectEndPos.dY = (double)i;
                    pixelEndPos = AbsFieldToPixel(objectEndPos);

                    bg.Graphics.DrawLine(drawPen, pixelStartPos, pixelEndPos);
                }
                scanNum++;
            }

            drawPen.Dispose();
        }



        #endregion

    }
}
