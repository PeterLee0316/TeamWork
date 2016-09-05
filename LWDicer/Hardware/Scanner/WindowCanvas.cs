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
using LWDicer.UI;

namespace LWDicer.Layers
{
    public partial class WindowCanvas : Form
    {
        private Point ptMouseStartPos = new Point(0, 0);
        private Point ptMouseEndPos = new Point(0, 0);
        private bool CheckDragDraw = false;
        protected bool CheckWheelZoomMode = false;
        
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
                        CheckDragDraw = true;
                        m_ScanWindow.SetObjectEndPos(Point.Empty);
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

                CheckDragDraw = false;

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

                CheckDragDraw = true;
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
                    currentZoom *= 1.1f;
                    m_ScanWindow.ChangeCanvasZoom(currentZoom);
                }
                else
                {
                    float currentZoom = BaseZoomFactor;
                    currentZoom /= 1.1f;
                    m_ScanWindow.ChangeCanvasZoom(currentZoom);
                }
                return;
            }
            base.OnMouseWheel(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_ScanWindow == null || m_ScanManager == null) return;

            // 기존 Object 그리기
            m_ScanManager.DrawObject(e);

            // 현재 Drag 모양 그리기
            if (CheckDragDraw)
                DragShapeDraw(e.Graphics);

            // Grid 그리기
            GridDraw(e.Graphics);

        }

        private void DragShapeDraw(Graphics g)
        {
            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);

            StartPos = ptMouseStartPos;
            EndPos = ptMouseEndPos;

            switch (m_ScanWindow.SelectObjectType)
            {
                case EObjectType.NONE:
                    g.DrawRectangle(BaseDrawPen[(int)EDrawPenType.SELECT],
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
                    break;
                case EObjectType.BMP:
                    g.DrawRectangle(BaseDrawPen[(int)EDrawPenType.SELECT],
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
                    break;

                case EObjectType.DOT:
                    Rectangle rectDot = new Rectangle(StartPos.X - DRAW_DOT_SIZE / 2, StartPos.Y - DRAW_DOT_SIZE / 2,
                                              DRAW_DOT_SIZE, DRAW_DOT_SIZE);
                    g.FillRectangle(BaseDrawBrush[(int)EDrawBrushType.OBJECT_DRAG], rectDot);
                    break;

                case EObjectType.LINE:
                    g.DrawLine(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG], StartPos, EndPos);
                    break;

                case EObjectType.RECTANGLE:
                    g.DrawRectangle(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG],
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
                    break;

                case EObjectType.CIRCLE:
                    g.DrawEllipse(BaseDrawPen[(int)EDrawPenType.OBJECT_DRAG],
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
                    break;

                default:

                    break;
            }
        }


        private void GridDraw(Graphics g)
        {
            Point startPos = new Point(0, 0);
            Point endPos = new Point(0, 0);
            Size pnlSize = new Size(0, 0);
            Size gridDieSize = new Size(0, 0);

            int gridPitch = 10;

            // Canvas의 Panel Size를 읽어온다.
            pnlSize = this.Size;

            gridDieSize.Width = pnlSize.Width / gridPitch;
            gridDieSize.Height = pnlSize.Height / gridPitch;

            for (int i = 0; i < pnlSize.Width; i += gridPitch)
            {
                startPos.X = i;
                startPos.Y = 0;
                endPos.X = i;
                endPos.Y = pnlSize.Height;                
                
               g.DrawLine(BaseDrawPen[(int)EDrawPenType.GRID_BRIGHT], startPos, endPos);
                
            }

            for (int i = 0; i < pnlSize.Height; i += gridPitch)
            {
                startPos.X = 0;
                startPos.Y = i;
                endPos.X = pnlSize.Width;
                endPos.Y = i;

                g.DrawLine(BaseDrawPen[(int)EDrawPenType.GRID_BRIGHT], startPos, endPos);
            }


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

    }

}
