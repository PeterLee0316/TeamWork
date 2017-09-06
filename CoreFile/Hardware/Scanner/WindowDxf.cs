using System;
using System.Xml;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

using WW.Actions;
using WW.Cad.Base;
using WW.Cad.Drawing;
using WW.Cad.Drawing.GDI;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Objects;
using WW.Cad.Model.Tables;
using WW.Drawing;
using WW.Math;
using WW.Math.Geometry;
using WW.Windows;

namespace Core.Layers
{


    /// <summary>
    /// This is a control that shows a DxfModel.
    /// Dragging with the mouse pans the drawing.
    /// Clicking on the drawing selects the closest entity and
    /// shows it in the property grid.
    /// Using the scroll wheel zooms in on the mouse position.
    /// </summary>
    /// 

    public partial class WindowDxf : UserControl
    {
        private DxfModel model;
        private GDIGraphics3D m_Graphics3D;
        private WireframeGraphicsCache m_GraphicsCache;
        private GraphicsHelper m_GraphicsHelper;
        private Bounds3D bounds;
        private Matrix4D MatToPos;
        private Matrix4D MatToPixel;

        public ulong selectEntity;
        private Point mouseClickLocation;
        private Point mouseClickStartPos;
        private Point mouseClickEndPos;

        private bool shiftPressed;
        private bool mouseDown;
        private bool mouseLeft;
        private bool mouseRight;
        private bool mouseMiddle;

        // HightLight
        private RenderedEntityInfo highlightedEntity;
        public IList<RenderedEntityInfo> selectEntities;
        private ArgbColor highlightColor = ArgbColors.Magenta;
        private ArgbColor secondaryHighlightColor = ArgbColors.Cyan;

        #region zooming and panning
        private SimpleTransformationProvider3D transformationProvider;
        private SimplePanInteractor panInteractor;
        private SimpleRectZoomInteractor rectZoomInteractor;
        private SimpleZoomWheelInteractor zoomWheelInteractor;
        private IInteractorWinFormsDrawable rectZoomInteractorDrawable;
        private IInteractorWinFormsDrawable currentInteractorDrawable;
        #endregion

        public event EventHandler<EntityEventArgs> EntitySelected;

        public WindowDxf()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);

            mouseClickStartPos = new Point(0, 0);
            mouseClickEndPos = new Point(0, 0);

            // Grahics Config 설정
            GraphicsConfig graphicsConfig = new GraphicsConfig();
            graphicsConfig.BackColor = BackColor;
            graphicsConfig.CorrectColorForBackgroundColor = true;

            m_Graphics3D = new GDIGraphics3D(graphicsConfig);
            m_Graphics3D.EnableDrawablesUpdate();
            m_GraphicsCache = new WireframeGraphicsCache(false, true);
            m_GraphicsCache.Config = graphicsConfig;
            m_GraphicsHelper = new GraphicsHelper(System.Drawing.Color.Blue);
            bounds = new Bounds3D();            
        }

        public DxfModel Model
        {
            get { return model; }
            set
            {
                model = value;

                if (model != null)
                {
                    m_GraphicsCache.CreateDrawables(model);
                    m_GraphicsCache.Draw(m_Graphics3D.CreateGraphicsFactory());

                    // Uncomment for rotation example.
                    // transformationProvider.ModelOrientation = QuaternionD.FromAxisAngle(Vector3D.XAxis, 30d * Math.PI / 180d);

                    transformationProvider = new SimpleTransformationProvider3D();
                    transformationProvider.TransformsChanged += new EventHandler(transformationProvider_TransformsChanged);

                    panInteractor = new SimplePanInteractor(transformationProvider);
                    rectZoomInteractor = new SimpleRectZoomInteractor(transformationProvider);
                    zoomWheelInteractor = new SimpleZoomWheelInteractor(transformationProvider);
                    rectZoomInteractorDrawable = new SimpleRectZoomInteractor.WinFormsDrawable(rectZoomInteractor);
                    
                    m_Graphics3D.BoundingBox(bounds, transformationProvider.WorldTransform);
                    transformationProvider.ResetTransforms(bounds);

                    CalculateTo2DTransform();
                    Invalidate();
                }
            }
        }

        public GDIGraphics3D GdiGraphics3D
        {
            get { return m_Graphics3D; }
        }

        public Point2D GetModelSpaceCoordinates(Point2D CanvasCoordinates)
        {
            return MatToPos.TransformTo2D(CanvasCoordinates);
        }

        public Point2D GetModelPixelCoordinates(Point2D realCoordinates)
        {
            return MatToPixel.TransformTo2D(realCoordinates);
        }

        //m_Graphics3D
        public void UpdateGraphic(DxfModel pModel)
        {
            m_GraphicsCache.CreateDrawables(pModel);
            m_GraphicsCache.Draw(m_Graphics3D.CreateGraphicsFactory());
            m_Graphics3D.CreateDrawables(pModel);

            DrawHighlightEntity();

            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            // Grid 그리기
            DrawCanvasGrid(e.Graphics);

            // Mouse Drag 그리기
            if (mouseLeft)
                DrawDragRect(e.Graphics);

            // Drag Zoom Size 그리기
            if (currentInteractorDrawable != null)
            {
                InteractionContext context = new InteractionContext(GetClientRectangle2D(),
                                                                    transformationProvider.CompleteTransform,
                                                                    true, new ArgbColor(BackColor.ToArgb()));
                currentInteractorDrawable.Draw(e, m_GraphicsHelper, context);
            }

            // Model 그리기
            m_Graphics3D.Draw(e.Graphics, ClientRectangle);
            
        }

        private void DrawDragRect(Graphics g)
        {
            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);
            Pen drawPen = new Pen(System.Drawing.Color.FromArgb(100, 100, 100));

            StartPos = mouseClickStartPos;
            EndPos = mouseClickEndPos;

            g.DrawRectangle(drawPen,
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
        }

        private void DrawCanvasGrid(Graphics g)
        {
            Point2D startPos = new Point2D(0, 0);
            Point2D endPos = new Point2D(0, 0);

            Point startPixel = new Point(0, 0);
            Point endPixel = new Point(0, 0);

            Size2D pnlSize = new Size2D(0, 0);

            Pen drawPen = new Pen(System.Drawing.Color.FromArgb(50, 50, 50));

            int gridMinor = 5;
            int gridMajor = gridMinor * 5;

            // Canvas의 Panel Size를 읽어온다.
            pnlSize.X = 300.0;
            pnlSize.Y = 300.0;


            drawPen.Color = System.Drawing.Color.FromArgb(30, 30, 30);
            // Minor Column Line Draw
            for (double i = 0; i <= pnlSize.X; i += gridMinor)
            {
                startPos.X = i;
                startPos.Y = 0;
                endPos.X = i;
                endPos.Y = pnlSize.Y;

                startPos = GetModelPixelCoordinates(startPos);
                startPixel.X = (int)startPos.X;
                startPixel.Y = (int)startPos.Y;

                endPos = GetModelPixelCoordinates(endPos);
                endPixel.X = (int)endPos.X;
                endPixel.Y = (int)endPos.Y;

                g.DrawLine(drawPen, startPixel, endPixel);

            }
            // Minor Row Line Draw
            for (int i = 0; i <= pnlSize.Y; i += gridMinor)
            {
                startPos.X = 0;
                startPos.Y = i;
                endPos.X = pnlSize.X;
                endPos.Y = i;

                startPos = GetModelPixelCoordinates(startPos);
                startPixel.X = (int)startPos.X;
                startPixel.Y = (int)startPos.Y;

                endPos = GetModelPixelCoordinates(endPos);
                endPixel.X = (int)endPos.X;
                endPixel.Y = (int)endPos.Y;

                g.DrawLine(drawPen, startPixel, endPixel);
            }

            drawPen.Color = System.Drawing.Color.FromArgb(30, 30, 70);
            // Major Column Line Draw
            for (int i = 0; i <= pnlSize.X; i += gridMajor)
            {
                startPos.X = i;
                startPos.Y = 0;
                endPos.X = i;
                endPos.Y = pnlSize.Y;

                startPos = GetModelPixelCoordinates(startPos);
                startPixel.X = (int)startPos.X;
                startPixel.Y = (int)startPos.Y;

                endPos = GetModelPixelCoordinates(endPos);
                endPixel.X = (int)endPos.X;
                endPixel.Y = (int)endPos.Y;

                g.DrawLine(drawPen, startPixel, endPixel);
            }
            // Major Row Line Draw
            for (int i = 0; i <= pnlSize.Y; i += gridMajor)
            {
                startPos.X = 0;
                startPos.Y = i;
                endPos.X = pnlSize.X;
                endPos.Y = i;

                startPos = GetModelPixelCoordinates(startPos);
                startPixel.X = (int)startPos.X;
                startPixel.Y = (int)startPos.Y;

                endPos = GetModelPixelCoordinates(endPos);
                endPixel.X = (int)endPos.X;
                endPixel.Y = (int)endPos.Y;

                g.DrawLine(drawPen, startPixel, endPixel);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Be careful to check that the control was already initialized, 
            // in some cases the InitializeComponent call triggers an OnResize call.
            if (transformationProvider != null)
            {
                CalculateTo2DTransform();
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouseClickLocation = e.Location;
            mouseDown = true;

            shiftPressed = ModifierKeys == Keys.Shift;

            if (shiftPressed == false)
            {
                mouseLeft = MouseButtons == MouseButtons.Left;
                mouseRight = MouseButtons == MouseButtons.Right;
                mouseMiddle = MouseButtons == MouseButtons.Middle;
            }

            if (shiftPressed)
            {
                rectZoomInteractor.Activate();
                rectZoomInteractor.ProcessMouseButtonDown(new CanonicalMouseEventArgs(e), GetInteractionContext());
                currentInteractorDrawable = rectZoomInteractorDrawable;
            }
            else if (mouseMiddle)
            {
                panInteractor.Activate();
                panInteractor.ProcessMouseButtonDown(new CanonicalMouseEventArgs(e), GetInteractionContext());
            }
            else if (mouseLeft)
            {
                mouseClickStartPos = e.Location;
            }
            else if (mouseRight)
            {
                //ContextMenu conMenu = new ContextMenu();
                //MenuItem m1 = new MenuItem("delete");
                //MenuItem m2 = new MenuItem("copy");
                //MenuItem m3 = new MenuItem("group");

            }
            else
            {
                //currentInteractorDrawable = rectZoomInteractorDrawable;                
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseDown == true)
            {
                if (shiftPressed)
                {
                    rectZoomInteractor.ProcessMouseMove(new CanonicalMouseEventArgs(e), GetInteractionContext());
                }
                else if (mouseMiddle)
                {
                    panInteractor.ProcessMouseMove(new CanonicalMouseEventArgs(e), GetInteractionContext());
                }
                else if (mouseLeft)
                {
                    mouseClickEndPos = e.Location;
                }
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

           // if (selectEntities == null) return;

            // Highlight된 Entities 해제
            if (selectEntities?.Count > 0)
            {
                foreach (RenderedEntityInfo Entity in selectEntities)
                {
                    IList<IWireframeDrawable> drawables = m_GraphicsCache.GetDrawables(Entity);
                    IWireframeGraphicsFactory graphicsFactory = null;

                    m_Graphics3D.UpdateDrawables(Entity,
                                                  () => {
                                                      foreach (IWireframeDrawable drawable in drawables)
                                                      { drawable.Draw(graphicsFactory); }
                                                  },
                                                  o => graphicsFactory = o);
                }

                // 선택한 객체 해제
                selectEntities = null;
                highlightedEntity = null;

            }

            // Use shift key for rectangle zoom.
            if (shiftPressed)
            {
                rectZoomInteractor.ProcessMouseButtonUp(new CanonicalMouseEventArgs(e), GetInteractionContext());
                rectZoomInteractor.Deactivate();
            }
            // Entity Click Select
            else if (mouseLeft)
            {
                panInteractor.Deactivate();
                // Select entity at mouse location if mouse didn't move
                // and show entity in property grid.
                if (mouseClickLocation == e.Location)
                {

                    Point2D referencePoint = new Point2D(e.X, e.Y);
                    double distance = 5;
                    if(model != null)
                    selectEntities = EntitySelector.GetEntitiesCloseToPoint(model,
                                                    GraphicsConfig.BlackBackgroundCorrectForBackColor,
                                                    m_Graphics3D.To2DTransform,
                                                    referencePoint,
                                                    distance);

                    DrawHighlightEntity();
                }
                // Entity Drag Select
                else 
                {
                    // Rectagle의 Point의 위치 비교함. 
                    mouseClickStartPos.X = (mouseClickLocation.X < e.X) ? mouseClickLocation.X : e.X;
                    mouseClickStartPos.Y = (mouseClickLocation.Y < e.Y) ? mouseClickLocation.Y : e.Y;
                    mouseClickEndPos.X = (mouseClickLocation.X > e.X) ? mouseClickLocation.X : e.X;
                    mouseClickEndPos.Y = (mouseClickLocation.Y > e.Y) ? mouseClickLocation.Y : e.Y;

                    // Rectagle 생성
                    Point2D connerPoint1 = new Point2D(mouseClickStartPos.X, mouseClickStartPos.Y);
                    Point2D connerPoint2 = new Point2D(mouseClickEndPos.X, mouseClickEndPos.Y);
                    Rectangle2D referenceRect = new Rectangle2D(connerPoint1, connerPoint2);

                    if (mouseClickLocation.X < e.X)
                    {
                        if(model != null)
                        selectEntities = EntitySelector.GetEntitiesInRectangle(model,
                                                    GraphicsConfig.BlackBackgroundCorrectForBackColor,
                                                    m_Graphics3D.To2DTransform,
                                                    referenceRect);
                    }
                    else
                    {
                        if (model != null)
                        selectEntities = EntitySelector.GetEntitiesPartiallyInRectangle(model,
                                                    GraphicsConfig.BlackBackgroundCorrectForBackColor,
                                                    m_Graphics3D.To2DTransform,
                                                    referenceRect);
                    }

                    DrawHighlightEntity();

                }
            }

            currentInteractorDrawable = null;

            mouseDown = false;
            mouseLeft = false;
            mouseRight = false;
            Invalidate();
        }

        private void DrawHighlightEntity()
        {
            if (selectEntities?.Count > 0)
            {
                foreach (RenderedEntityInfo Entity in selectEntities)
                {
                    // Chose the last entity as it is drawn last, so will be on top.
                    highlightedEntity = Entity;
                    IList<IWireframeDrawable> drawables = m_GraphicsCache.GetDrawables(highlightedEntity);
                    WireframeGraphicsFactoryColorChanger graphicsFactoryColorChanger = null;

                    m_Graphics3D.UpdateDrawables(
                        highlightedEntity,
                        () => { foreach (IWireframeDrawable drawable in drawables) { drawable.Draw(graphicsFactoryColorChanger); } },
                        o => graphicsFactoryColorChanger = new WireframeGraphicsFactoryColorChanger(o, ColorChanger)
                    );
                }

                // Property Show
                DxfEntity entity = highlightedEntity.Entity;
                OnEntitySelected(new EntityEventArgs(entity));
            }
        }

        public IList<ulong> GetEntitiesHandle()
        {
            if (selectEntities == null) return null;

            IList<ulong> entitiesHandle = new List<ulong>();

            foreach (RenderedEntityInfo entInfo in selectEntities)
            {
                entitiesHandle.Add(entInfo.Entity.Handle);
            }

            return entitiesHandle;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            zoomWheelInteractor.Activate();
            zoomWheelInteractor.ProcessMouseWheel(new CanonicalMouseEventArgs(e), GetInteractionContext());
            zoomWheelInteractor.Deactivate();

            Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //Key key;
            //ModifierKeys modifierKeys;
            //InputUtil.GetWindowsKey(keyData, out key, out modifierKeys);
            //IInteractor interactor = new ShowPositionInteractor();
            //Point p = PointToClient(MousePosition);

            //bool handled = interactor.ProcessKeyDown(
            //    new CanonicalMouseEventArgs(new MouseEventArgs(MouseButtons.None, 0, p.X, p.Y, 0)),
            //    key,
            //    modifierKeys,
            //    GetInteractionContext()
            //);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected virtual void OnEntitySelected(EntityEventArgs e)
        {
            if (EntitySelected != null)
            {
                EntitySelected(this, e);
            }
        }

        private Matrix4D CalculateTo2DTransform()
        {
            transformationProvider.ViewWindow = GetClientRectangle2D();

            Matrix4D to2DTransform = Matrix4D.Identity;

            if (model != null && bounds != null)
            {
                to2DTransform = transformationProvider.CompleteTransform;
            }
            m_Graphics3D.To2DTransform = to2DTransform;

            // 좌표 변환 Matrix 구함.
            MatToPos = m_Graphics3D.To2DTransform.GetInverse();
            MatToPixel = m_Graphics3D.To2DTransform;

            return to2DTransform;
        }

        private Rectangle2D GetClientRectangle2D()
        {
            return new Rectangle2D(
                ClientRectangle.Left,
                ClientRectangle.Top,
                ClientRectangle.Right,
                ClientRectangle.Bottom
            );
        }

        private void transformationProvider_TransformsChanged(object sender, EventArgs e)
        {
            CalculateTo2DTransform();
            Invalidate();
        }

        private InteractionContext GetInteractionContext()
        {
            return new InteractionContext(
                new Rectangle2D(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Right, ClientRectangle.Bottom),
                transformationProvider.CompleteTransform,
                true,
                BackColor
            );
        }

        private ArgbColor ColorChanger(ArgbColor argbColor)
        {
            ArgbColor result = highlightColor;
            if (argbColor == result)
            {
                result = secondaryHighlightColor;
            }
            return result;
        }
    }

}
