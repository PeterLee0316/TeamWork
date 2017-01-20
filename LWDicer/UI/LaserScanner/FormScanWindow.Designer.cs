using LWDicer.Layers;

namespace LWDicer.UI
{
    partial class FormScanWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScanWindow));
            this.statusBottom = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ShapeListView = new System.Windows.Forms.ListView();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.tmrView = new System.Windows.Forms.Timer(this.components);
            this.ribbonControl = new Syncfusion.Windows.Forms.Tools.RibbonControlAdv();
            this.toolTabDraw = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.toolDraw = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolPnlDraw = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnDot = new System.Windows.Forms.ToolStripButton();
            this.toolBtnLine = new System.Windows.Forms.ToolStripButton();
            this.toolBtnArc = new System.Windows.Forms.ToolStripButton();
            this.toolBtnRectacgle = new System.Windows.Forms.ToolStripButton();
            this.toolBtnCircle = new System.Windows.Forms.ToolStripButton();
            this.toolBtnEllipse = new System.Windows.Forms.ToolStripButton();
            this.toolStripDraw2 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem3 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnFont = new System.Windows.Forms.ToolStripButton();
            this.toolBtnBmp = new System.Windows.Forms.ToolStripButton();
            this.toolBtnDxf = new System.Windows.Forms.ToolStripButton();
            this.toolBtnNone = new System.Windows.Forms.ToolStripButton();
            this.toolStripDelete = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem7 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolBtnDeleteAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoom = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem2 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnZoomPlus = new System.Windows.Forms.ToolStripButton();
            this.toolBtnZoomDraw = new System.Windows.Forms.ToolStripButton();
            this.toolBtnZoomMinus = new System.Windows.Forms.ToolStripButton();
            this.toolBtnZoomAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripCopy = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem1 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.txtCopyNumX = new System.Windows.Forms.ToolStripTextBox();
            this.txtCopyPitchX = new System.Windows.Forms.ToolStripTextBox();
            this.txtCopyNumY = new System.Windows.Forms.ToolStripTextBox();
            this.txtCopyPitchY = new System.Windows.Forms.ToolStripTextBox();
            this.toolBtnArrayCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripGroup = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem5 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnGroup = new System.Windows.Forms.ToolStripButton();
            this.toolBtnUnGroup = new System.Windows.Forms.ToolStripButton();
            this.toolTabProperty = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.toolStripProperty = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem9 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolStripLabel12 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel13 = new System.Windows.Forms.ToolStripLabel();
            this.toolLblWidth = new System.Windows.Forms.ToolStripLabel();
            this.toolLblHeigth = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtObjectCenterX = new System.Windows.Forms.ToolStripTextBox();
            this.tooltxtObjectCenterY = new System.Windows.Forms.ToolStripTextBox();
            this.tooltxtObjectWidth = new System.Windows.Forms.ToolStripTextBox();
            this.tooltxtObjectHeight = new System.Windows.Forms.ToolStripTextBox();
            this.toolBtnPropertyChange = new System.Windows.Forms.ToolStripButton();
            this.toolStripRotate = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem4 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolStripLabel10 = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtCurrentAngle = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel11 = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtRotateAngle = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripPanelItem8 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnRotateCCW = new System.Windows.Forms.ToolStripButton();
            this.toolBtnRotateCW = new System.Windows.Forms.ToolStripButton();
            this.toolStripDimension = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem6 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtStartPointX = new System.Windows.Forms.ToolStripTextBox();
            this.tooltxtStartPointY = new System.Windows.Forms.ToolStripTextBox();
            this.tooltxtEndPointX = new System.Windows.Forms.ToolStripTextBox();
            this.tooltxtEndPointY = new System.Windows.Forms.ToolStripTextBox();
            this.toolBtnDimensionChange = new System.Windows.Forms.ToolStripButton();
            this.toolStripMove = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolMoveDataPnl = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tooltxtDistance = new System.Windows.Forms.ToolStripTextBox();
            this.toolBtnMovePnl = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnMoveL = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnMoveU = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnMoveD = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnMoveR = new System.Windows.Forms.ToolStripButton();
            this.toolTabConvert = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.toolStripSaveBmp = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolBtnSaveBmp = new System.Windows.Forms.ToolStripButton();
            this.toolBtnSaveLse = new System.Windows.Forms.ToolStripButton();
            this.toolBtnClose = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.statusBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.ribbonControl.SuspendLayout();
            this.toolTabDraw.Panel.SuspendLayout();
            this.toolDraw.SuspendLayout();
            this.toolStripDraw2.SuspendLayout();
            this.toolStripDelete.SuspendLayout();
            this.toolStripZoom.SuspendLayout();
            this.toolStripCopy.SuspendLayout();
            this.toolStripGroup.SuspendLayout();
            this.toolTabProperty.Panel.SuspendLayout();
            this.toolStripProperty.SuspendLayout();
            this.toolStripRotate.SuspendLayout();
            this.toolStripDimension.SuspendLayout();
            this.toolStripMove.SuspendLayout();
            this.toolTabConvert.Panel.SuspendLayout();
            this.toolStripSaveBmp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBottom
            // 
            this.statusBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel2});
            this.statusBottom.Location = new System.Drawing.Point(0, 823);
            this.statusBottom.Name = "statusBottom";
            this.statusBottom.Size = new System.Drawing.Size(1253, 22);
            this.statusBottom.TabIndex = 2;
            this.statusBottom.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(121, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(121, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // ShapeListView
            // 
            this.ShapeListView.AutoArrange = false;
            this.ShapeListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShapeListView.GridLines = true;
            this.ShapeListView.Location = new System.Drawing.Point(0, 0);
            this.ShapeListView.Name = "ShapeListView";
            this.ShapeListView.Size = new System.Drawing.Size(215, 631);
            this.ShapeListView.TabIndex = 7;
            this.ShapeListView.UseCompatibleStateImageBehavior = false;
            this.ShapeListView.SelectedIndexChanged += new System.EventHandler(this.ShapeListView_SelectedIndexChanged);
            this.ShapeListView.Click += new System.EventHandler(this.ShapeListView_Click);
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.AutoScroll = true;
            this.pnlCanvas.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlCanvas.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.pnlCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 0);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(1034, 631);
            this.pnlCanvas.TabIndex = 10;
            // 
            // tmrView
            // 
            this.tmrView.Enabled = true;
            this.tmrView.Tick += new System.EventHandler(this.tmrView_Tick);
            // 
            // ribbonControl
            // 
            this.ribbonControl.AutoSize = true;
            this.ribbonControl.Dock = Syncfusion.Windows.Forms.Tools.DockStyleEx.Fill;
            this.ribbonControl.Header.AddMainItem(toolTabDraw);
            this.ribbonControl.Header.AddMainItem(toolTabProperty);
            this.ribbonControl.Header.AddMainItem(toolTabConvert);
            this.ribbonControl.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl.MenuButtonImage = ((System.Drawing.Image)(resources.GetObject("ribbonControl.MenuButtonImage")));
            this.ribbonControl.MenuButtonWidth = 50;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.OfficeColorScheme = Syncfusion.Windows.Forms.Tools.ToolStripEx.ColorScheme.Managed;
            // 
            // ribbonControl.OfficeMenu
            // 
            this.ribbonControl.OfficeMenu.MainPanel.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ribbonControl.OfficeMenu.MainPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnClose});
            this.ribbonControl.OfficeMenu.Name = "OfficeMenu";
            this.ribbonControl.OfficeMenu.ShowItemToolTips = true;
            this.ribbonControl.OfficeMenu.Size = new System.Drawing.Size(62, 98);
            this.ribbonControl.OfficeMenu.Text = "Draw";
            this.ribbonControl.OfficeMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ribbonControl_OfficeMenu_ItemClicked);
            this.ribbonControl.Size = new System.Drawing.Size(1253, 188);
            this.ribbonControl.SystemText.QuickAccessDialogDropDownName = "Start menu";
            this.ribbonControl.TabIndex = 757;
            this.ribbonControl.Text = "Draw";
            this.ribbonControl.Click += new System.EventHandler(this.ribbonControl_Click);
            // 
            // toolTabDraw
            // 
            this.toolTabDraw.BackColor = System.Drawing.Color.White;
            this.toolTabDraw.Name = "toolTabDraw";
            this.toolTabDraw.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            // 
            // ribbonControl.ribbonPanel1
            // 
            this.toolTabDraw.Panel.BackColor = System.Drawing.Color.White;
            this.toolTabDraw.Panel.Controls.Add(this.toolDraw);
            this.toolTabDraw.Panel.Controls.Add(this.toolStripDraw2);
            this.toolTabDraw.Panel.Controls.Add(this.toolStripDelete);
            this.toolTabDraw.Panel.Controls.Add(this.toolStripZoom);
            this.toolTabDraw.Panel.Controls.Add(this.toolStripCopy);
            this.toolTabDraw.Panel.Controls.Add(this.toolStripGroup);
            this.toolTabDraw.Panel.Name = "ribbonPanel1";
            this.toolTabDraw.Panel.ScrollPosition = 0;
            this.toolTabDraw.Panel.TabIndex = 2;
            this.toolTabDraw.Panel.Text = "Draw";
            this.toolTabDraw.Position = 0;
            this.toolTabDraw.Size = new System.Drawing.Size(48, 20);
            this.toolTabDraw.Text = "Draw";
            // 
            // toolDraw
            // 
            this.toolDraw.Dock = System.Windows.Forms.DockStyle.None;
            this.toolDraw.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolDraw.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolDraw.Image = null;
            this.toolDraw.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolPnlDraw});
            this.toolDraw.Location = new System.Drawing.Point(0, 1);
            this.toolDraw.Name = "toolDraw";
            this.toolDraw.Size = new System.Drawing.Size(121, 127);
            this.toolDraw.TabIndex = 8;
            this.toolDraw.Text = "Draw 1";
            // 
            // toolPnlDraw
            // 
            this.toolPnlDraw.CausesValidation = false;
            this.toolPnlDraw.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolPnlDraw.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnDot,
            this.toolBtnLine,
            this.toolBtnArc,
            this.toolBtnRectacgle,
            this.toolBtnCircle,
            this.toolBtnEllipse});
            this.toolPnlDraw.Name = "toolPnlDraw";
            this.toolPnlDraw.RowCount = 2;
            this.toolPnlDraw.Size = new System.Drawing.Size(112, 109);
            this.toolPnlDraw.Text = "toolStripPanelItem1";
            this.toolPnlDraw.Transparent = true;
            // 
            // toolBtnDot
            // 
            this.toolBtnDot.AutoSize = false;
            this.toolBtnDot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolBtnDot.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnDot.Image")));
            this.toolBtnDot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnDot.Name = "toolBtnDot";
            this.toolBtnDot.Size = new System.Drawing.Size(36, 50);
            this.toolBtnDot.Text = "Point";
            this.toolBtnDot.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnDot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnDot.ToolTipText = "Dot";
            this.toolBtnDot.Click += new System.EventHandler(this.toolBtnDot_Click);
            // 
            // toolBtnLine
            // 
            this.toolBtnLine.AutoSize = false;
            this.toolBtnLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolBtnLine.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnLine.Image")));
            this.toolBtnLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnLine.Name = "toolBtnLine";
            this.toolBtnLine.Size = new System.Drawing.Size(36, 50);
            this.toolBtnLine.Text = "Line";
            this.toolBtnLine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnLine.ToolTipText = "Line";
            this.toolBtnLine.Click += new System.EventHandler(this.toolBtnLine_Click);
            // 
            // toolBtnArc
            // 
            this.toolBtnArc.AutoSize = false;
            this.toolBtnArc.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnArc.Image")));
            this.toolBtnArc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnArc.Name = "toolBtnArc";
            this.toolBtnArc.Size = new System.Drawing.Size(36, 50);
            this.toolBtnArc.Text = "Arc";
            this.toolBtnArc.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnArc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnArc.ToolTipText = "Arc";
            this.toolBtnArc.Click += new System.EventHandler(this.toolBtnArc_Click);
            // 
            // toolBtnRectacgle
            // 
            this.toolBtnRectacgle.AutoSize = false;
            this.toolBtnRectacgle.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnRectacgle.Image")));
            this.toolBtnRectacgle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnRectacgle.Name = "toolBtnRectacgle";
            this.toolBtnRectacgle.Size = new System.Drawing.Size(36, 50);
            this.toolBtnRectacgle.Text = "Rect";
            this.toolBtnRectacgle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnRectacgle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnRectacgle.ToolTipText = "Rect";
            this.toolBtnRectacgle.Click += new System.EventHandler(this.toolBtnRectacgle_Click);
            // 
            // toolBtnCircle
            // 
            this.toolBtnCircle.AutoSize = false;
            this.toolBtnCircle.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnCircle.Image")));
            this.toolBtnCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnCircle.Name = "toolBtnCircle";
            this.toolBtnCircle.Size = new System.Drawing.Size(36, 50);
            this.toolBtnCircle.Text = "Circle";
            this.toolBtnCircle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnCircle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnCircle.ToolTipText = "Circle";
            this.toolBtnCircle.Click += new System.EventHandler(this.toolBtnCircle_Click);
            // 
            // toolBtnEllipse
            // 
            this.toolBtnEllipse.AutoSize = false;
            this.toolBtnEllipse.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnEllipse.Image")));
            this.toolBtnEllipse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnEllipse.Name = "toolBtnEllipse";
            this.toolBtnEllipse.Size = new System.Drawing.Size(36, 50);
            this.toolBtnEllipse.Text = "Ellipse";
            this.toolBtnEllipse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnEllipse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnEllipse.ToolTipText = "Ellipse";
            this.toolBtnEllipse.Click += new System.EventHandler(this.toolBtnEllipse_Click);
            // 
            // toolStripDraw2
            // 
            this.toolStripDraw2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripDraw2.Image = null;
            this.toolStripDraw2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem3});
            this.toolStripDraw2.Location = new System.Drawing.Point(123, 1);
            this.toolStripDraw2.Name = "toolStripDraw2";
            this.toolStripDraw2.Size = new System.Drawing.Size(94, 127);
            this.toolStripDraw2.TabIndex = 7;
            this.toolStripDraw2.Text = "Draw 2";
            // 
            // toolStripPanelItem3
            // 
            this.toolStripPanelItem3.CausesValidation = false;
            this.toolStripPanelItem3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnFont,
            this.toolBtnBmp,
            this.toolBtnDxf,
            this.toolBtnNone});
            this.toolStripPanelItem3.Name = "toolStripPanelItem3";
            this.toolStripPanelItem3.RowCount = 2;
            this.toolStripPanelItem3.Size = new System.Drawing.Size(76, 109);
            this.toolStripPanelItem3.Text = "toolStripPanelItem3";
            this.toolStripPanelItem3.Transparent = true;
            // 
            // toolBtnFont
            // 
            this.toolBtnFont.AutoSize = false;
            this.toolBtnFont.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnFont.Image")));
            this.toolBtnFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnFont.Name = "toolBtnFont";
            this.toolBtnFont.Size = new System.Drawing.Size(36, 50);
            this.toolBtnFont.Text = "Text";
            this.toolBtnFont.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnFont.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnFont.Click += new System.EventHandler(this.toolBtnFont_Click);
            // 
            // toolBtnBmp
            // 
            this.toolBtnBmp.AutoSize = false;
            this.toolBtnBmp.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnBmp.Image")));
            this.toolBtnBmp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnBmp.Name = "toolBtnBmp";
            this.toolBtnBmp.Size = new System.Drawing.Size(36, 50);
            this.toolBtnBmp.Text = "Bmp";
            this.toolBtnBmp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnBmp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnBmp.Click += new System.EventHandler(this.toolBtnBmp_Click);
            // 
            // toolBtnDxf
            // 
            this.toolBtnDxf.AutoSize = false;
            this.toolBtnDxf.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnDxf.Image")));
            this.toolBtnDxf.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnDxf.Name = "toolBtnDxf";
            this.toolBtnDxf.Size = new System.Drawing.Size(36, 50);
            this.toolBtnDxf.Text = "Dxf";
            this.toolBtnDxf.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnDxf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnDxf.Click += new System.EventHandler(this.toolBtnDxf_Click);
            // 
            // toolBtnNone
            // 
            this.toolBtnNone.AutoSize = false;
            this.toolBtnNone.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnNone.Image")));
            this.toolBtnNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnNone.Name = "toolBtnNone";
            this.toolBtnNone.Size = new System.Drawing.Size(36, 50);
            this.toolBtnNone.Text = "None";
            this.toolBtnNone.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnNone.Click += new System.EventHandler(this.toolBtnNone_Click);
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripDelete.Image = null;
            this.toolStripDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem7});
            this.toolStripDelete.Location = new System.Drawing.Point(219, 1);
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Size = new System.Drawing.Size(82, 127);
            this.toolStripDelete.TabIndex = 6;
            this.toolStripDelete.Text = "Delete";
            // 
            // toolStripPanelItem7
            // 
            this.toolStripPanelItem7.CausesValidation = false;
            this.toolStripPanelItem7.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem7.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnDelete,
            this.toolBtnDeleteAll});
            this.toolStripPanelItem7.Name = "toolStripPanelItem7";
            this.toolStripPanelItem7.RowCount = 2;
            this.toolStripPanelItem7.Size = new System.Drawing.Size(64, 109);
            this.toolStripPanelItem7.Text = "toolStripPanelItem5";
            this.toolStripPanelItem7.Transparent = true;
            // 
            // toolBtnDelete
            // 
            this.toolBtnDelete.AutoSize = false;
            this.toolBtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnDelete.Image")));
            this.toolBtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnDelete.Name = "toolBtnDelete";
            this.toolBtnDelete.Size = new System.Drawing.Size(60, 50);
            this.toolBtnDelete.Text = "Delete";
            this.toolBtnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnDelete.Click += new System.EventHandler(this.toolBtnDelete_Click);
            // 
            // toolBtnDeleteAll
            // 
            this.toolBtnDeleteAll.AutoSize = false;
            this.toolBtnDeleteAll.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnDeleteAll.Image")));
            this.toolBtnDeleteAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnDeleteAll.Name = "toolBtnDeleteAll";
            this.toolBtnDeleteAll.Size = new System.Drawing.Size(60, 50);
            this.toolBtnDeleteAll.Text = "Delete All";
            this.toolBtnDeleteAll.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnDeleteAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnDeleteAll.Click += new System.EventHandler(this.toolBtnDeleteAll_Click);
            // 
            // toolStripZoom
            // 
            this.toolStripZoom.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripZoom.Image = null;
            this.toolStripZoom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem2});
            this.toolStripZoom.Location = new System.Drawing.Point(303, 1);
            this.toolStripZoom.Name = "toolStripZoom";
            this.toolStripZoom.Size = new System.Drawing.Size(94, 127);
            this.toolStripZoom.TabIndex = 3;
            this.toolStripZoom.Text = "Zoom";
            // 
            // toolStripPanelItem2
            // 
            this.toolStripPanelItem2.CausesValidation = false;
            this.toolStripPanelItem2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnZoomPlus,
            this.toolBtnZoomDraw,
            this.toolBtnZoomMinus,
            this.toolBtnZoomAll});
            this.toolStripPanelItem2.Name = "toolStripPanelItem2";
            this.toolStripPanelItem2.RowCount = 2;
            this.toolStripPanelItem2.Size = new System.Drawing.Size(76, 109);
            this.toolStripPanelItem2.Text = "toolStripPanelItem2";
            this.toolStripPanelItem2.Transparent = true;
            // 
            // toolBtnZoomPlus
            // 
            this.toolBtnZoomPlus.AutoSize = false;
            this.toolBtnZoomPlus.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnZoomPlus.Image")));
            this.toolBtnZoomPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnZoomPlus.Name = "toolBtnZoomPlus";
            this.toolBtnZoomPlus.Size = new System.Drawing.Size(36, 50);
            this.toolBtnZoomPlus.Text = "+";
            this.toolBtnZoomPlus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnZoomPlus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnZoomPlus.Click += new System.EventHandler(this.toolBtnZoomPlus_Click);
            // 
            // toolBtnZoomDraw
            // 
            this.toolBtnZoomDraw.AutoSize = false;
            this.toolBtnZoomDraw.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnZoomDraw.Image")));
            this.toolBtnZoomDraw.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnZoomDraw.Name = "toolBtnZoomDraw";
            this.toolBtnZoomDraw.Size = new System.Drawing.Size(36, 50);
            this.toolBtnZoomDraw.Text = "Drag";
            this.toolBtnZoomDraw.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnZoomDraw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnZoomDraw.Click += new System.EventHandler(this.toolBtnZoomDraw_Click);
            // 
            // toolBtnZoomMinus
            // 
            this.toolBtnZoomMinus.AutoSize = false;
            this.toolBtnZoomMinus.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnZoomMinus.Image")));
            this.toolBtnZoomMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnZoomMinus.Name = "toolBtnZoomMinus";
            this.toolBtnZoomMinus.Size = new System.Drawing.Size(36, 50);
            this.toolBtnZoomMinus.Text = "-";
            this.toolBtnZoomMinus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnZoomMinus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnZoomMinus.Click += new System.EventHandler(this.toolBtnZoomMinus_Click);
            // 
            // toolBtnZoomAll
            // 
            this.toolBtnZoomAll.AutoSize = false;
            this.toolBtnZoomAll.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnZoomAll.Image")));
            this.toolBtnZoomAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnZoomAll.Name = "toolBtnZoomAll";
            this.toolBtnZoomAll.Size = new System.Drawing.Size(36, 50);
            this.toolBtnZoomAll.Text = "ALL";
            this.toolBtnZoomAll.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnZoomAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnZoomAll.Click += new System.EventHandler(this.toolBtnZoomAll_Click);
            // 
            // toolStripCopy
            // 
            this.toolStripCopy.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripCopy.Image = null;
            this.toolStripCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem1,
            this.toolBtnArrayCopy});
            this.toolStripCopy.Location = new System.Drawing.Point(399, 1);
            this.toolStripCopy.Name = "toolStripCopy";
            this.toolStripCopy.Size = new System.Drawing.Size(169, 127);
            this.toolStripCopy.TabIndex = 1;
            this.toolStripCopy.Text = "Array Copy";
            // 
            // toolStripPanelItem1
            // 
            this.toolStripPanelItem1.AutoSize = false;
            this.toolStripPanelItem1.CausesValidation = false;
            this.toolStripPanelItem1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripLabel3,
            this.toolStripLabel4,
            this.toolStripLabel5,
            this.txtCopyNumX,
            this.txtCopyPitchX,
            this.txtCopyNumY,
            this.txtCopyPitchY});
            this.toolStripPanelItem1.Name = "toolStripPanelItem1";
            this.toolStripPanelItem1.RowCount = 4;
            this.toolStripPanelItem1.Size = new System.Drawing.Size(101, 98);
            this.toolStripPanelItem1.Text = "toolStripPanelItem1";
            this.toolStripPanelItem1.Transparent = true;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(45, 15);
            this.toolStripLabel2.Text = "X Num";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(45, 15);
            this.toolStripLabel3.Text = "X Pitch";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(45, 15);
            this.toolStripLabel4.Text = "Y Num";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(45, 15);
            this.toolStripLabel5.Text = "Y Pitch";
            // 
            // txtCopyNumX
            // 
            this.txtCopyNumX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopyNumX.Name = "txtCopyNumX";
            this.txtCopyNumX.Size = new System.Drawing.Size(50, 23);
            this.txtCopyNumX.Text = "1";
            this.txtCopyNumX.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCopyPitchX
            // 
            this.txtCopyPitchX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopyPitchX.Name = "txtCopyPitchX";
            this.txtCopyPitchX.Size = new System.Drawing.Size(50, 23);
            this.txtCopyPitchX.Text = "1";
            this.txtCopyPitchX.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCopyNumY
            // 
            this.txtCopyNumY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopyNumY.Name = "txtCopyNumY";
            this.txtCopyNumY.Size = new System.Drawing.Size(50, 23);
            this.txtCopyNumY.Text = "1";
            this.txtCopyNumY.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCopyPitchY
            // 
            this.txtCopyPitchY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopyPitchY.Name = "txtCopyPitchY";
            this.txtCopyPitchY.Size = new System.Drawing.Size(50, 23);
            this.txtCopyPitchY.Text = "1";
            this.txtCopyPitchY.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolBtnArrayCopy
            // 
            this.toolBtnArrayCopy.AutoSize = false;
            this.toolBtnArrayCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnArrayCopy.Image")));
            this.toolBtnArrayCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnArrayCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnArrayCopy.Name = "toolBtnArrayCopy";
            this.toolBtnArrayCopy.Size = new System.Drawing.Size(50, 107);
            this.toolBtnArrayCopy.Text = "Copy";
            this.toolBtnArrayCopy.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnArrayCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnArrayCopy.Click += new System.EventHandler(this.toolBtnArrayCopy_Click);
            // 
            // toolStripGroup
            // 
            this.toolStripGroup.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripGroup.Image = null;
            this.toolStripGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem5});
            this.toolStripGroup.Location = new System.Drawing.Point(570, 1);
            this.toolStripGroup.Name = "toolStripGroup";
            this.toolStripGroup.Size = new System.Drawing.Size(82, 127);
            this.toolStripGroup.TabIndex = 0;
            this.toolStripGroup.Text = "Group";
            // 
            // toolStripPanelItem5
            // 
            this.toolStripPanelItem5.CausesValidation = false;
            this.toolStripPanelItem5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnGroup,
            this.toolBtnUnGroup});
            this.toolStripPanelItem5.Name = "toolStripPanelItem5";
            this.toolStripPanelItem5.RowCount = 2;
            this.toolStripPanelItem5.Size = new System.Drawing.Size(64, 109);
            this.toolStripPanelItem5.Text = "toolStripPanelItem5";
            this.toolStripPanelItem5.Transparent = true;
            // 
            // toolBtnGroup
            // 
            this.toolBtnGroup.AutoSize = false;
            this.toolBtnGroup.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnGroup.Image")));
            this.toolBtnGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnGroup.Name = "toolBtnGroup";
            this.toolBtnGroup.Size = new System.Drawing.Size(60, 50);
            this.toolBtnGroup.Text = "Group";
            this.toolBtnGroup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnGroup.Click += new System.EventHandler(this.toolBtnGroup_Click);
            // 
            // toolBtnUnGroup
            // 
            this.toolBtnUnGroup.AutoSize = false;
            this.toolBtnUnGroup.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnUnGroup.Image")));
            this.toolBtnUnGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnUnGroup.Name = "toolBtnUnGroup";
            this.toolBtnUnGroup.Size = new System.Drawing.Size(60, 50);
            this.toolBtnUnGroup.Text = "UnGroup";
            this.toolBtnUnGroup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnUnGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnUnGroup.Click += new System.EventHandler(this.toolBtnUnGroup_Click);
            // 
            // toolTabProperty
            // 
            this.toolTabProperty.Name = "toolTabProperty";
            // 
            // ribbonControl.ribbonPanel2
            // 
            this.toolTabProperty.Panel.Controls.Add(this.toolStripProperty);
            this.toolTabProperty.Panel.Controls.Add(this.toolStripRotate);
            this.toolTabProperty.Panel.Controls.Add(this.toolStripDimension);
            this.toolTabProperty.Panel.Controls.Add(this.toolStripMove);
            this.toolTabProperty.Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolTabProperty.Panel.Name = "ribbonPanel2";
            this.toolTabProperty.Panel.ScrollPosition = 0;
            this.toolTabProperty.Panel.TabIndex = 3;
            this.toolTabProperty.Panel.Text = "Property";
            this.toolTabProperty.Position = 1;
            this.toolTabProperty.Size = new System.Drawing.Size(58, 18);
            this.toolTabProperty.Text = "Property";
            // 
            // toolStripProperty
            // 
            this.toolStripProperty.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripProperty.Image = null;
            this.toolStripProperty.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem9,
            this.toolBtnPropertyChange});
            this.toolStripProperty.Location = new System.Drawing.Point(0, 1);
            this.toolStripProperty.Name = "toolStripProperty";
            this.toolStripProperty.Size = new System.Drawing.Size(219, 0);
            this.toolStripProperty.TabIndex = 8;
            this.toolStripProperty.Text = "Property";
            // 
            // toolStripPanelItem9
            // 
            this.toolStripPanelItem9.AutoSize = false;
            this.toolStripPanelItem9.CausesValidation = false;
            this.toolStripPanelItem9.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem9.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel12,
            this.toolStripLabel13,
            this.toolLblWidth,
            this.toolLblHeigth,
            this.tooltxtObjectCenterX,
            this.tooltxtObjectCenterY,
            this.tooltxtObjectWidth,
            this.tooltxtObjectHeight});
            this.toolStripPanelItem9.Name = "toolStripPanelItem9";
            this.toolStripPanelItem9.RowCount = 4;
            this.toolStripPanelItem9.Size = new System.Drawing.Size(151, 100);
            this.toolStripPanelItem9.Text = "toolStripPanelItem1";
            this.toolStripPanelItem9.Transparent = true;
            // 
            // toolStripLabel12
            // 
            this.toolStripLabel12.Name = "toolStripLabel12";
            this.toolStripLabel12.Size = new System.Drawing.Size(53, 15);
            this.toolStripLabel12.Text = "Center X";
            // 
            // toolStripLabel13
            // 
            this.toolStripLabel13.Name = "toolStripLabel13";
            this.toolStripLabel13.Size = new System.Drawing.Size(54, 15);
            this.toolStripLabel13.Text = "          Y";
            // 
            // toolLblWidth
            // 
            this.toolLblWidth.Name = "toolLblWidth";
            this.toolLblWidth.Size = new System.Drawing.Size(39, 15);
            this.toolLblWidth.Text = "Width";
            // 
            // toolLblHeigth
            // 
            this.toolLblHeigth.Name = "toolLblHeigth";
            this.toolLblHeigth.Size = new System.Drawing.Size(43, 15);
            this.toolLblHeigth.Text = "Height";
            // 
            // tooltxtObjectCenterX
            // 
            this.tooltxtObjectCenterX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tooltxtObjectCenterX.Name = "tooltxtObjectCenterX";
            this.tooltxtObjectCenterX.Size = new System.Drawing.Size(70, 23);
            this.tooltxtObjectCenterX.Text = "1";
            this.tooltxtObjectCenterX.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tooltxtObjectCenterY
            // 
            this.tooltxtObjectCenterY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tooltxtObjectCenterY.Name = "tooltxtObjectCenterY";
            this.tooltxtObjectCenterY.Size = new System.Drawing.Size(70, 23);
            this.tooltxtObjectCenterY.Text = "1";
            this.tooltxtObjectCenterY.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tooltxtObjectWidth
            // 
            this.tooltxtObjectWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tooltxtObjectWidth.Name = "tooltxtObjectWidth";
            this.tooltxtObjectWidth.Size = new System.Drawing.Size(70, 23);
            this.tooltxtObjectWidth.Text = "1";
            this.tooltxtObjectWidth.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tooltxtObjectHeight
            // 
            this.tooltxtObjectHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tooltxtObjectHeight.Name = "tooltxtObjectHeight";
            this.tooltxtObjectHeight.Size = new System.Drawing.Size(70, 23);
            this.tooltxtObjectHeight.Text = "1";
            this.tooltxtObjectHeight.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolBtnPropertyChange
            // 
            this.toolBtnPropertyChange.AutoSize = false;
            this.toolBtnPropertyChange.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnPropertyChange.Image")));
            this.toolBtnPropertyChange.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnPropertyChange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnPropertyChange.Name = "toolBtnPropertyChange";
            this.toolBtnPropertyChange.Size = new System.Drawing.Size(50, 107);
            this.toolBtnPropertyChange.Text = "Change";
            this.toolBtnPropertyChange.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnPropertyChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnPropertyChange.Click += new System.EventHandler(this.toolBtnPropertyChange_Click);
            // 
            // toolStripRotate
            // 
            this.toolStripRotate.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripRotate.Image = null;
            this.toolStripRotate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem4,
            this.toolStripPanelItem8});
            this.toolStripRotate.Location = new System.Drawing.Point(221, 1);
            this.toolStripRotate.Name = "toolStripRotate";
            this.toolStripRotate.Size = new System.Drawing.Size(165, 0);
            this.toolStripRotate.TabIndex = 7;
            this.toolStripRotate.Text = "Rotate";
            // 
            // toolStripPanelItem4
            // 
            this.toolStripPanelItem4.AutoSize = false;
            this.toolStripPanelItem4.CausesValidation = false;
            this.toolStripPanelItem4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel10,
            this.tooltxtCurrentAngle,
            this.toolStripLabel11,
            this.tooltxtRotateAngle});
            this.toolStripPanelItem4.Name = "toolStripPanelItem4";
            this.toolStripPanelItem4.RowCount = 4;
            this.toolStripPanelItem4.Size = new System.Drawing.Size(106, 100);
            this.toolStripPanelItem4.Text = "toolStripPanelItem1";
            this.toolStripPanelItem4.Transparent = true;
            // 
            // toolStripLabel10
            // 
            this.toolStripLabel10.Name = "toolStripLabel10";
            this.toolStripLabel10.Size = new System.Drawing.Size(82, 15);
            this.toolStripLabel10.Text = "Current Angle";
            // 
            // tooltxtCurrentAngle
            // 
            this.tooltxtCurrentAngle.Name = "tooltxtCurrentAngle";
            this.tooltxtCurrentAngle.ReadOnly = true;
            this.tooltxtCurrentAngle.Size = new System.Drawing.Size(100, 23);
            this.tooltxtCurrentAngle.Text = "0";
            this.tooltxtCurrentAngle.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel11
            // 
            this.toolStripLabel11.Name = "toolStripLabel11";
            this.toolStripLabel11.Size = new System.Drawing.Size(76, 15);
            this.toolStripLabel11.Text = "Rotate Angle";
            // 
            // tooltxtRotateAngle
            // 
            this.tooltxtRotateAngle.Name = "tooltxtRotateAngle";
            this.tooltxtRotateAngle.Size = new System.Drawing.Size(100, 23);
            this.tooltxtRotateAngle.Text = "0";
            this.tooltxtRotateAngle.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripPanelItem8
            // 
            this.toolStripPanelItem8.AutoSize = false;
            this.toolStripPanelItem8.CausesValidation = false;
            this.toolStripPanelItem8.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem8.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnRotateCCW,
            this.toolBtnRotateCW});
            this.toolStripPanelItem8.Name = "toolStripPanelItem8";
            this.toolStripPanelItem8.RowCount = 2;
            this.toolStripPanelItem8.Size = new System.Drawing.Size(39, 80);
            this.toolStripPanelItem8.Text = "toolStripPanelItem8";
            this.toolStripPanelItem8.Transparent = true;
            // 
            // toolBtnRotateCCW
            // 
            this.toolBtnRotateCCW.AutoSize = false;
            this.toolBtnRotateCCW.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnRotateCCW.Image")));
            this.toolBtnRotateCCW.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnRotateCCW.Name = "toolBtnRotateCCW";
            this.toolBtnRotateCCW.Size = new System.Drawing.Size(35, 35);
            this.toolBtnRotateCCW.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnRotateCCW.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnRotateCCW.Click += new System.EventHandler(this.toolBtnRotateCCW_Click);
            // 
            // toolBtnRotateCW
            // 
            this.toolBtnRotateCW.AutoSize = false;
            this.toolBtnRotateCW.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnRotateCW.Image")));
            this.toolBtnRotateCW.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnRotateCW.Name = "toolBtnRotateCW";
            this.toolBtnRotateCW.Size = new System.Drawing.Size(35, 35);
            this.toolBtnRotateCW.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnRotateCW.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnRotateCW.Click += new System.EventHandler(this.toolBtnRotateCW_Click);
            // 
            // toolStripDimension
            // 
            this.toolStripDimension.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripDimension.Image = null;
            this.toolStripDimension.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem6,
            this.toolBtnDimensionChange});
            this.toolStripDimension.Location = new System.Drawing.Point(388, 1);
            this.toolStripDimension.Name = "toolStripDimension";
            this.toolStripDimension.Size = new System.Drawing.Size(219, 0);
            this.toolStripDimension.TabIndex = 5;
            this.toolStripDimension.Text = "Dimension";
            // 
            // toolStripPanelItem6
            // 
            this.toolStripPanelItem6.AutoSize = false;
            this.toolStripPanelItem6.CausesValidation = false;
            this.toolStripPanelItem6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.toolStripLabel7,
            this.toolStripLabel8,
            this.toolStripLabel9,
            this.tooltxtStartPointX,
            this.tooltxtStartPointY,
            this.tooltxtEndPointX,
            this.tooltxtEndPointY});
            this.toolStripPanelItem6.Name = "toolStripPanelItem6";
            this.toolStripPanelItem6.RowCount = 4;
            this.toolStripPanelItem6.Size = new System.Drawing.Size(151, 100);
            this.toolStripPanelItem6.Text = "toolStripPanelItem1";
            this.toolStripPanelItem6.Transparent = true;
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(75, 15);
            this.toolStripLabel6.Text = "Start Point X";
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(75, 15);
            this.toolStripLabel7.Text = "Start Point Y";
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(70, 15);
            this.toolStripLabel8.Text = "End Point X";
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(70, 15);
            this.toolStripLabel9.Text = "End Point Y";
            // 
            // tooltxtStartPointX
            // 
            this.tooltxtStartPointX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tooltxtStartPointX.Name = "tooltxtStartPointX";
            this.tooltxtStartPointX.Size = new System.Drawing.Size(70, 23);
            this.tooltxtStartPointX.Text = "1";
            this.tooltxtStartPointX.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tooltxtStartPointY
            // 
            this.tooltxtStartPointY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tooltxtStartPointY.Name = "tooltxtStartPointY";
            this.tooltxtStartPointY.Size = new System.Drawing.Size(70, 23);
            this.tooltxtStartPointY.Text = "1";
            this.tooltxtStartPointY.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tooltxtEndPointX
            // 
            this.tooltxtEndPointX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tooltxtEndPointX.Name = "tooltxtEndPointX";
            this.tooltxtEndPointX.Size = new System.Drawing.Size(70, 23);
            this.tooltxtEndPointX.Text = "1";
            this.tooltxtEndPointX.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tooltxtEndPointY
            // 
            this.tooltxtEndPointY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tooltxtEndPointY.Name = "tooltxtEndPointY";
            this.tooltxtEndPointY.Size = new System.Drawing.Size(70, 23);
            this.tooltxtEndPointY.Text = "1";
            this.tooltxtEndPointY.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolBtnDimensionChange
            // 
            this.toolBtnDimensionChange.AutoSize = false;
            this.toolBtnDimensionChange.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnDimensionChange.Image")));
            this.toolBtnDimensionChange.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnDimensionChange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnDimensionChange.Name = "toolBtnDimensionChange";
            this.toolBtnDimensionChange.Size = new System.Drawing.Size(50, 107);
            this.toolBtnDimensionChange.Text = "Change";
            this.toolBtnDimensionChange.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnDimensionChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnDimensionChange.Click += new System.EventHandler(this.toolBtnDimensionChange_Click);
            // 
            // toolStripMove
            // 
            this.toolStripMove.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripMove.Image = null;
            this.toolStripMove.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMoveDataPnl,
            this.toolBtnMovePnl});
            this.toolStripMove.Location = new System.Drawing.Point(609, 1);
            this.toolStripMove.Name = "toolStripMove";
            this.toolStripMove.Size = new System.Drawing.Size(236, 0);
            this.toolStripMove.TabIndex = 2;
            this.toolStripMove.Text = "Move";
            // 
            // toolMoveDataPnl
            // 
            this.toolMoveDataPnl.AutoSize = false;
            this.toolMoveDataPnl.CausesValidation = false;
            this.toolMoveDataPnl.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolMoveDataPnl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tooltxtDistance});
            this.toolMoveDataPnl.Name = "toolMoveDataPnl";
            this.toolMoveDataPnl.RowCount = 2;
            this.toolMoveDataPnl.Size = new System.Drawing.Size(106, 50);
            this.toolMoveDataPnl.Text = "toolStripPanelItem1";
            this.toolMoveDataPnl.Transparent = true;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(87, 15);
            this.toolStripLabel1.Text = "Move Distance";
            // 
            // tooltxtDistance
            // 
            this.tooltxtDistance.Name = "tooltxtDistance";
            this.tooltxtDistance.Size = new System.Drawing.Size(100, 23);
            this.tooltxtDistance.Text = "5";
            this.tooltxtDistance.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolBtnMovePnl
            // 
            this.toolBtnMovePnl.AutoSize = false;
            this.toolBtnMovePnl.CausesValidation = false;
            this.toolBtnMovePnl.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolBtnMovePnl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolBtnMoveL,
            this.toolStripSeparator2,
            this.toolBtnMoveU,
            this.toolStripSeparator4,
            this.toolBtnMoveD,
            this.toolStripSeparator3,
            this.toolBtnMoveR});
            this.toolBtnMovePnl.Name = "toolBtnMovePnl";
            this.toolBtnMovePnl.Size = new System.Drawing.Size(110, 110);
            this.toolBtnMovePnl.Text = "toolStripPanelItem8";
            this.toolBtnMovePnl.Transparent = true;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(35, 0);
            // 
            // toolBtnMoveL
            // 
            this.toolBtnMoveL.AutoSize = false;
            this.toolBtnMoveL.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnMoveL.Image")));
            this.toolBtnMoveL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnMoveL.Name = "toolBtnMoveL";
            this.toolBtnMoveL.Size = new System.Drawing.Size(35, 35);
            this.toolBtnMoveL.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnMoveL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnMoveL.Click += new System.EventHandler(this.toolBtnMoveL_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(35, 0);
            // 
            // toolBtnMoveU
            // 
            this.toolBtnMoveU.AutoSize = false;
            this.toolBtnMoveU.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnMoveU.Image")));
            this.toolBtnMoveU.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnMoveU.Name = "toolBtnMoveU";
            this.toolBtnMoveU.Size = new System.Drawing.Size(35, 35);
            this.toolBtnMoveU.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnMoveU.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnMoveU.Click += new System.EventHandler(this.toolBtnMoveU_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.AutoSize = false;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(35, 0);
            // 
            // toolBtnMoveD
            // 
            this.toolBtnMoveD.AutoSize = false;
            this.toolBtnMoveD.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnMoveD.Image")));
            this.toolBtnMoveD.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnMoveD.Name = "toolBtnMoveD";
            this.toolBtnMoveD.Size = new System.Drawing.Size(35, 35);
            this.toolBtnMoveD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnMoveD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnMoveD.Click += new System.EventHandler(this.toolBtnMoveD_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(35, 0);
            // 
            // toolBtnMoveR
            // 
            this.toolBtnMoveR.AutoSize = false;
            this.toolBtnMoveR.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnMoveR.Image")));
            this.toolBtnMoveR.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnMoveR.Name = "toolBtnMoveR";
            this.toolBtnMoveR.Size = new System.Drawing.Size(35, 35);
            this.toolBtnMoveR.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnMoveR.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnMoveR.Click += new System.EventHandler(this.toolBtnMoveR_Click);
            // 
            // toolTabConvert
            // 
            this.toolTabConvert.Name = "toolTabConvert";
            // 
            // ribbonControl.ribbonPanel3
            // 
            this.toolTabConvert.Panel.Controls.Add(this.toolStripSaveBmp);
            this.toolTabConvert.Panel.Name = "ribbonPanel3";
            this.toolTabConvert.Panel.ScrollPosition = 0;
            this.toolTabConvert.Panel.TabIndex = 4;
            this.toolTabConvert.Panel.Text = "Save to BMP";
            this.toolTabConvert.Position = 2;
            this.toolTabConvert.Size = new System.Drawing.Size(84, 18);
            this.toolTabConvert.Text = "Save to BMP";
            // 
            // toolStripSaveBmp
            // 
            this.toolStripSaveBmp.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripSaveBmp.Image = null;
            this.toolStripSaveBmp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnSaveBmp,
            this.toolBtnSaveLse});
            this.toolStripSaveBmp.Location = new System.Drawing.Point(0, 1);
            this.toolStripSaveBmp.Name = "toolStripSaveBmp";
            this.toolStripSaveBmp.Size = new System.Drawing.Size(416, 0);
            this.toolStripSaveBmp.TabIndex = 4;
            this.toolStripSaveBmp.Text = "Convert";
            // 
            // toolBtnSaveBmp
            // 
            this.toolBtnSaveBmp.AutoSize = false;
            this.toolBtnSaveBmp.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnSaveBmp.Image")));
            this.toolBtnSaveBmp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnSaveBmp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnSaveBmp.Name = "toolBtnSaveBmp";
            this.toolBtnSaveBmp.Size = new System.Drawing.Size(200, 107);
            this.toolBtnSaveBmp.Text = "Save to BMP";
            this.toolBtnSaveBmp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnSaveBmp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnSaveBmp.Click += new System.EventHandler(this.toolBtnSaveBmp_Click);
            // 
            // toolBtnSaveLse
            // 
            this.toolBtnSaveLse.AutoSize = false;
            this.toolBtnSaveLse.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnSaveLse.Image")));
            this.toolBtnSaveLse.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnSaveLse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnSaveLse.Name = "toolBtnSaveLse";
            this.toolBtnSaveLse.Size = new System.Drawing.Size(200, 107);
            this.toolBtnSaveLse.Text = "Save to LSE";
            this.toolBtnSaveLse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnSaveLse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnSaveLse.Click += new System.EventHandler(this.toolBtnSaveLse_Click);
            // 
            // toolBtnClose
            // 
            this.toolBtnClose.AutoSize = false;
            this.toolBtnClose.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnClose.Image")));
            this.toolBtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnClose.Name = "toolBtnClose";
            this.toolBtnClose.Size = new System.Drawing.Size(50, 50);
            this.toolBtnClose.Text = "Close";
            this.toolBtnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolBtnClose.ToolTipText = "Close";
            this.toolBtnClose.Click += new System.EventHandler(this.toolBtnClose_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlCanvas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ShapeListView);
            this.splitContainer1.Size = new System.Drawing.Size(1253, 631);
            this.splitContainer1.SplitterDistance = 1034;
            this.splitContainer1.TabIndex = 758;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ribbonControl);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(1253, 823);
            this.splitContainer2.SplitterDistance = 188;
            this.splitContainer2.TabIndex = 759;
            // 
            // FormScanWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1253, 845);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.statusBottom);
            this.Name = "FormScanWindow";
            this.Text = "Laser Scan System";
            this.Activated += new System.EventHandler(this.FormScanWindow_Activated);
            this.Load += new System.EventHandler(this.FormScanWindow_Load);
            this.Resize += new System.EventHandler(this.CWindowForm_Resize);
            this.statusBottom.ResumeLayout(false);
            this.statusBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.ribbonControl.ResumeLayout(false);
            this.ribbonControl.PerformLayout();
            this.toolTabDraw.Panel.ResumeLayout(false);
            this.toolTabDraw.Panel.PerformLayout();
            this.toolDraw.ResumeLayout(false);
            this.toolDraw.PerformLayout();
            this.toolStripDraw2.ResumeLayout(false);
            this.toolStripDraw2.PerformLayout();
            this.toolStripDelete.ResumeLayout(false);
            this.toolStripDelete.PerformLayout();
            this.toolStripZoom.ResumeLayout(false);
            this.toolStripZoom.PerformLayout();
            this.toolStripCopy.ResumeLayout(false);
            this.toolStripCopy.PerformLayout();
            this.toolStripGroup.ResumeLayout(false);
            this.toolStripGroup.PerformLayout();
            this.toolTabProperty.Panel.ResumeLayout(false);
            this.toolTabProperty.Panel.PerformLayout();
            this.toolStripProperty.ResumeLayout(false);
            this.toolStripProperty.PerformLayout();
            this.toolStripRotate.ResumeLayout(false);
            this.toolStripRotate.PerformLayout();
            this.toolStripDimension.ResumeLayout(false);
            this.toolStripDimension.PerformLayout();
            this.toolStripMove.ResumeLayout(false);
            this.toolStripMove.PerformLayout();
            this.toolTabConvert.Panel.ResumeLayout(false);
            this.toolTabConvert.Panel.PerformLayout();
            this.toolStripSaveBmp.ResumeLayout(false);
            this.toolStripSaveBmp.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusBottom;
        private System.Windows.Forms.ListView ShapeListView;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Timer tmrView;
        private Syncfusion.Windows.Forms.Tools.RibbonControlAdv ribbonControl;
        private Syncfusion.Windows.Forms.Tools.ToolStripTabItem toolTabDraw;
        private Syncfusion.Windows.Forms.Tools.ToolStripTabItem toolTabProperty;
        private Syncfusion.Windows.Forms.Tools.ToolStripTabItem toolTabConvert;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripCopy;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripGroup;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem1;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripZoom;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripSaveBmp;
        private System.Windows.Forms.ToolStripButton toolBtnSaveBmp;
        private System.Windows.Forms.ToolStripButton toolBtnSaveLse;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem2;
        private System.Windows.Forms.ToolStripButton toolBtnZoomPlus;
        private System.Windows.Forms.ToolStripButton toolBtnZoomDraw;
        private System.Windows.Forms.ToolStripButton toolBtnZoomMinus;
        private System.Windows.Forms.ToolStripButton toolBtnZoomAll;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripMove;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox txtCopyPitchX;
        private System.Windows.Forms.ToolStripTextBox txtCopyNumX;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripTextBox txtCopyNumY;
        private System.Windows.Forms.ToolStripTextBox txtCopyPitchY;
        private System.Windows.Forms.ToolStripButton toolBtnArrayCopy;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem5;
        private System.Windows.Forms.ToolStripButton toolBtnGroup;
        private System.Windows.Forms.ToolStripButton toolBtnUnGroup;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolDraw;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolPnlDraw;
        private System.Windows.Forms.ToolStripButton toolBtnDot;
        private System.Windows.Forms.ToolStripButton toolBtnLine;
        private System.Windows.Forms.ToolStripButton toolBtnArc;
        private System.Windows.Forms.ToolStripButton toolBtnRectacgle;
        private System.Windows.Forms.ToolStripButton toolBtnCircle;
        private System.Windows.Forms.ToolStripButton toolBtnEllipse;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripDraw2;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem3;
        private System.Windows.Forms.ToolStripButton toolBtnFont;
        private System.Windows.Forms.ToolStripButton toolBtnBmp;
        private System.Windows.Forms.ToolStripButton toolBtnDxf;
        private System.Windows.Forms.ToolStripButton toolBtnNone;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripDelete;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem7;
        private System.Windows.Forms.ToolStripButton toolBtnDelete;
        private System.Windows.Forms.ToolStripButton toolBtnDeleteAll;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolMoveDataPnl;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tooltxtDistance;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolBtnMovePnl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolBtnMoveL;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolBtnMoveU;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolBtnMoveD;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolBtnMoveR;
        private System.Windows.Forms.ToolStripButton toolBtnClose;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripDimension;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel9;
        private System.Windows.Forms.ToolStripTextBox tooltxtStartPointX;
        private System.Windows.Forms.ToolStripTextBox tooltxtStartPointY;
        private System.Windows.Forms.ToolStripTextBox tooltxtEndPointX;
        private System.Windows.Forms.ToolStripTextBox tooltxtEndPointY;
        private System.Windows.Forms.ToolStripButton toolBtnDimensionChange;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripProperty;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem9;
        private System.Windows.Forms.ToolStripLabel toolStripLabel12;
        private System.Windows.Forms.ToolStripLabel toolStripLabel13;
        private System.Windows.Forms.ToolStripLabel toolLblWidth;
        private System.Windows.Forms.ToolStripLabel toolLblHeigth;
        private System.Windows.Forms.ToolStripTextBox tooltxtObjectCenterX;
        private System.Windows.Forms.ToolStripTextBox tooltxtObjectCenterY;
        private System.Windows.Forms.ToolStripTextBox tooltxtObjectWidth;
        private System.Windows.Forms.ToolStripTextBox tooltxtObjectHeight;
        private System.Windows.Forms.ToolStripButton toolBtnPropertyChange;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripRotate;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel10;
        private System.Windows.Forms.ToolStripTextBox tooltxtCurrentAngle;
        private System.Windows.Forms.ToolStripLabel toolStripLabel11;
        private System.Windows.Forms.ToolStripTextBox tooltxtRotateAngle;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem8;
        private System.Windows.Forms.ToolStripButton toolBtnRotateCCW;
        private System.Windows.Forms.ToolStripButton toolBtnRotateCW;
    }
}