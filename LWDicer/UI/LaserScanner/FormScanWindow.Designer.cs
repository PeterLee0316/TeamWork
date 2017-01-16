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
            this.menuScanner = new System.Windows.Forms.MenuStrip();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.penToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rectangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ellipseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer6ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layer10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.polygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShapeListView = new System.Windows.Forms.ListView();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.tabProperty = new System.Windows.Forms.TabControl();
            this.pageProperty = new System.Windows.Forms.TabPage();
            this.lblMousePos = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnObjectMoveDn = new System.Windows.Forms.Button();
            this.btnObjectMoveRight = new System.Windows.Forms.Button();
            this.btnObjectMoveLeft = new System.Windows.Forms.Button();
            this.btnObjectMoveUp = new System.Windows.Forms.Button();
            this.btnObjectMove = new System.Windows.Forms.Button();
            this.txtObjectMoveT = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtObjectMoveY = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtObjectMoveX = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnObjectChange = new System.Windows.Forms.Button();
            this.txtObjectAngle = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtObjectEndPosY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtObjectEndPosX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtObjectStartPosY = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtObjectStartPosX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pageEntity = new System.Windows.Forms.TabPage();
            this.pageHatch = new System.Windows.Forms.TabPage();
            this.pnlObject = new System.Windows.Forms.Panel();
            this.btnPanDn = new System.Windows.Forms.Button();
            this.btnPanUp = new System.Windows.Forms.Button();
            this.btnPanRight = new System.Windows.Forms.Button();
            this.btnPanLeft = new System.Windows.Forms.Button();
            this.tmrView = new System.Windows.Forms.Timer(this.components);
            this.BtnConfigureExit = new System.Windows.Forms.Button();
            this.ribbonControl = new Syncfusion.Windows.Forms.Tools.RibbonControlAdv();
            this.toolTabDraw = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.toolStripEx2 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem2 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnZoomPlus = new System.Windows.Forms.ToolStripButton();
            this.toolBtnZoomDraw = new System.Windows.Forms.ToolStripButton();
            this.toolBtnZoomMinus = new System.Windows.Forms.ToolStripButton();
            this.toolBtnZoomAll = new System.Windows.Forms.ToolStripButton();
            this.toolArrayCopy = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem1 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.txtCopyPitchX = new System.Windows.Forms.ToolStripTextBox();
            this.txtCopyNumX = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.txtCopyNumY = new System.Windows.Forms.ToolStripTextBox();
            this.txtCopyPitchY = new System.Windows.Forms.ToolStripTextBox();
            this.toolGroup = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolTabProperty = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.toolStripEx6 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem4 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripEx3 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolTabConvert = new Syncfusion.Windows.Forms.Tools.ToolStripTabItem();
            this.toolStripEx4 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolBtnSaveBmp = new System.Windows.Forms.ToolStripButton();
            this.toolBtnSaveLse = new System.Windows.Forms.ToolStripButton();
            this.toolBtnArrayCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripPanelItem5 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnGroup = new System.Windows.Forms.ToolStripButton();
            this.toolBtnUnGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripEx1 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem6 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBox3 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBox4 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBox5 = new System.Windows.Forms.ToolStripTextBox();
            this.toolBtnChange = new System.Windows.Forms.ToolStripButton();
            this.toolStripEx7 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem7 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolBtnDeleteAll = new System.Windows.Forms.ToolStripButton();
            this.toolDraw = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolPnlDraw = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnDot = new System.Windows.Forms.ToolStripButton();
            this.toolBtnLine = new System.Windows.Forms.ToolStripButton();
            this.toolBtnArc = new System.Windows.Forms.ToolStripButton();
            this.toolBtnRectacgle = new System.Windows.Forms.ToolStripButton();
            this.toolBtnCircle = new System.Windows.Forms.ToolStripButton();
            this.toolBtnEllipse = new System.Windows.Forms.ToolStripButton();
            this.toolStripEx5 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.toolStripPanelItem3 = new Syncfusion.Windows.Forms.Tools.ToolStripPanelItem();
            this.toolBtnFont = new System.Windows.Forms.ToolStripButton();
            this.toolBtnBmp = new System.Windows.Forms.ToolStripButton();
            this.toolBtnDxf = new System.Windows.Forms.ToolStripButton();
            this.toolBtnNone = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.statusBottom.SuspendLayout();
            this.menuScanner.SuspendLayout();
            this.tabProperty.SuspendLayout();
            this.pageProperty.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pageEntity.SuspendLayout();
            this.pnlObject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.ribbonControl.SuspendLayout();
            this.toolTabDraw.Panel.SuspendLayout();
            this.toolStripEx2.SuspendLayout();
            this.toolArrayCopy.SuspendLayout();
            this.toolGroup.SuspendLayout();
            this.toolTabProperty.Panel.SuspendLayout();
            this.toolStripEx6.SuspendLayout();
            this.toolStripEx3.SuspendLayout();
            this.toolTabConvert.Panel.SuspendLayout();
            this.toolStripEx4.SuspendLayout();
            this.toolStripEx1.SuspendLayout();
            this.toolStripEx7.SuspendLayout();
            this.toolDraw.SuspendLayout();
            this.toolStripEx5.SuspendLayout();
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
            // menuScanner
            // 
            this.menuScanner.AutoSize = false;
            this.menuScanner.Dock = System.Windows.Forms.DockStyle.None;
            this.menuScanner.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuScanner.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.penToolStripMenuItem,
            this.layerToolStripMenuItem,
            this.설정ToolStripMenuItem});
            this.menuScanner.Location = new System.Drawing.Point(874, 3);
            this.menuScanner.Name = "menuScanner";
            this.menuScanner.Size = new System.Drawing.Size(352, 45);
            this.menuScanner.TabIndex = 3;
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(46, 41);
            this.homeToolStripMenuItem.Text = "Main";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.homeToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // penToolStripMenuItem
            // 
            this.penToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointToolStripMenuItem,
            this.lineToolStripMenuItem,
            this.arcToolStripMenuItem,
            this.rectangleToolStripMenuItem,
            this.circleToolStripMenuItem,
            this.ellipseToolStripMenuItem,
            this.fontToolStripMenuItem});
            this.penToolStripMenuItem.Name = "penToolStripMenuItem";
            this.penToolStripMenuItem.Size = new System.Drawing.Size(47, 41);
            this.penToolStripMenuItem.Text = "Draw";
            this.penToolStripMenuItem.Click += new System.EventHandler(this.penToolStripMenuItem_Click);
            // 
            // pointToolStripMenuItem
            // 
            this.pointToolStripMenuItem.Name = "pointToolStripMenuItem";
            this.pointToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.pointToolStripMenuItem.Text = "Point";
            this.pointToolStripMenuItem.Click += new System.EventHandler(this.pointToolStripMenuItem_Click);
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.lineToolStripMenuItem.Text = "Line";
            this.lineToolStripMenuItem.Click += new System.EventHandler(this.lineToolStripMenuItem_Click);
            // 
            // arcToolStripMenuItem
            // 
            this.arcToolStripMenuItem.Name = "arcToolStripMenuItem";
            this.arcToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.arcToolStripMenuItem.Text = "Arc";
            this.arcToolStripMenuItem.Click += new System.EventHandler(this.arcToolStripMenuItem_Click);
            // 
            // rectangleToolStripMenuItem
            // 
            this.rectangleToolStripMenuItem.Name = "rectangleToolStripMenuItem";
            this.rectangleToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.rectangleToolStripMenuItem.Text = "Rectangle";
            this.rectangleToolStripMenuItem.Click += new System.EventHandler(this.rectangleToolStripMenuItem_Click);
            // 
            // circleToolStripMenuItem
            // 
            this.circleToolStripMenuItem.Name = "circleToolStripMenuItem";
            this.circleToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.circleToolStripMenuItem.Text = "Circle";
            this.circleToolStripMenuItem.Click += new System.EventHandler(this.circleToolStripMenuItem_Click);
            // 
            // ellipseToolStripMenuItem
            // 
            this.ellipseToolStripMenuItem.Name = "ellipseToolStripMenuItem";
            this.ellipseToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.ellipseToolStripMenuItem.Text = "Ellipse";
            this.ellipseToolStripMenuItem.Click += new System.EventHandler(this.ellipseToolStripMenuItem_Click);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.fontToolStripMenuItem.Text = "Font";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // layerToolStripMenuItem
            // 
            this.layerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layer1ToolStripMenuItem,
            this.layer2ToolStripMenuItem,
            this.layer3ToolStripMenuItem,
            this.layer4ToolStripMenuItem,
            this.layer5ToolStripMenuItem,
            this.layer6ToolStripMenuItem,
            this.layer7ToolStripMenuItem,
            this.layer8ToolStripMenuItem,
            this.layer9ToolStripMenuItem,
            this.layer10ToolStripMenuItem});
            this.layerToolStripMenuItem.Name = "layerToolStripMenuItem";
            this.layerToolStripMenuItem.Size = new System.Drawing.Size(47, 41);
            this.layerToolStripMenuItem.Text = "Layer";
            // 
            // layer1ToolStripMenuItem
            // 
            this.layer1ToolStripMenuItem.Name = "layer1ToolStripMenuItem";
            this.layer1ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer1ToolStripMenuItem.Text = "Layer1";
            // 
            // layer2ToolStripMenuItem
            // 
            this.layer2ToolStripMenuItem.Name = "layer2ToolStripMenuItem";
            this.layer2ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer2ToolStripMenuItem.Text = "Layer2";
            // 
            // layer3ToolStripMenuItem
            // 
            this.layer3ToolStripMenuItem.Name = "layer3ToolStripMenuItem";
            this.layer3ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer3ToolStripMenuItem.Text = "Layer3";
            // 
            // layer4ToolStripMenuItem
            // 
            this.layer4ToolStripMenuItem.Name = "layer4ToolStripMenuItem";
            this.layer4ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer4ToolStripMenuItem.Text = "Layer4";
            // 
            // layer5ToolStripMenuItem
            // 
            this.layer5ToolStripMenuItem.Name = "layer5ToolStripMenuItem";
            this.layer5ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer5ToolStripMenuItem.Text = "Layer5";
            // 
            // layer6ToolStripMenuItem
            // 
            this.layer6ToolStripMenuItem.Name = "layer6ToolStripMenuItem";
            this.layer6ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer6ToolStripMenuItem.Text = "Layer6";
            // 
            // layer7ToolStripMenuItem
            // 
            this.layer7ToolStripMenuItem.Name = "layer7ToolStripMenuItem";
            this.layer7ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer7ToolStripMenuItem.Text = "Layer7";
            // 
            // layer8ToolStripMenuItem
            // 
            this.layer8ToolStripMenuItem.Name = "layer8ToolStripMenuItem";
            this.layer8ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer8ToolStripMenuItem.Text = "Layer8";
            // 
            // layer9ToolStripMenuItem
            // 
            this.layer9ToolStripMenuItem.Name = "layer9ToolStripMenuItem";
            this.layer9ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer9ToolStripMenuItem.Text = "Layer9";
            // 
            // layer10ToolStripMenuItem
            // 
            this.layer10ToolStripMenuItem.Name = "layer10ToolStripMenuItem";
            this.layer10ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.layer10ToolStripMenuItem.Text = "Layer10";
            // 
            // 설정ToolStripMenuItem
            // 
            this.설정ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.polygonToolStripMenuItem});
            this.설정ToolStripMenuItem.Name = "설정ToolStripMenuItem";
            this.설정ToolStripMenuItem.Size = new System.Drawing.Size(43, 41);
            this.설정ToolStripMenuItem.Text = "설정";
            // 
            // polygonToolStripMenuItem
            // 
            this.polygonToolStripMenuItem.Name = "polygonToolStripMenuItem";
            this.polygonToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.polygonToolStripMenuItem.Text = "Polygon";
            this.polygonToolStripMenuItem.Click += new System.EventHandler(this.polygonToolStripMenuItem_Click);
            // 
            // ShapeListView
            // 
            this.ShapeListView.AutoArrange = false;
            this.ShapeListView.GridLines = true;
            this.ShapeListView.Location = new System.Drawing.Point(6, 6);
            this.ShapeListView.Name = "ShapeListView";
            this.ShapeListView.Size = new System.Drawing.Size(191, 411);
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
            this.pnlCanvas.Size = new System.Drawing.Size(979, 631);
            this.pnlCanvas.TabIndex = 10;
            // 
            // tabProperty
            // 
            this.tabProperty.Controls.Add(this.pageProperty);
            this.tabProperty.Controls.Add(this.pageEntity);
            this.tabProperty.Controls.Add(this.pageHatch);
            this.tabProperty.Location = new System.Drawing.Point(17, 17);
            this.tabProperty.Name = "tabProperty";
            this.tabProperty.SelectedIndex = 0;
            this.tabProperty.Size = new System.Drawing.Size(243, 474);
            this.tabProperty.TabIndex = 12;
            // 
            // pageProperty
            // 
            this.pageProperty.Controls.Add(this.groupBox2);
            this.pageProperty.Controls.Add(this.lblMousePos);
            this.pageProperty.Controls.Add(this.groupBox1);
            this.pageProperty.Controls.Add(this.pnlObject);
            this.pageProperty.Controls.Add(this.BtnConfigureExit);
            this.pageProperty.Location = new System.Drawing.Point(4, 22);
            this.pageProperty.Name = "pageProperty";
            this.pageProperty.Padding = new System.Windows.Forms.Padding(3);
            this.pageProperty.Size = new System.Drawing.Size(235, 448);
            this.pageProperty.TabIndex = 0;
            this.pageProperty.Text = "속성";
            this.pageProperty.UseVisualStyleBackColor = true;
            // 
            // lblMousePos
            // 
            this.lblMousePos.AutoSize = true;
            this.lblMousePos.Location = new System.Drawing.Point(19, 433);
            this.lblMousePos.Name = "lblMousePos";
            this.lblMousePos.Size = new System.Drawing.Size(41, 12);
            this.lblMousePos.TabIndex = 7;
            this.lblMousePos.Text = "개수 Y";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.btnObjectMoveDn);
            this.groupBox2.Controls.Add(this.btnObjectMoveRight);
            this.groupBox2.Controls.Add(this.btnObjectMoveLeft);
            this.groupBox2.Controls.Add(this.btnObjectMoveUp);
            this.groupBox2.Controls.Add(this.btnObjectMove);
            this.groupBox2.Controls.Add(this.txtObjectMoveT);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtObjectMoveY);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtObjectMoveX);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(3, 166);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 110);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "상대 이동";
            // 
            // btnObjectMoveDn
            // 
            this.btnObjectMoveDn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectMoveDn.Location = new System.Drawing.Point(164, 70);
            this.btnObjectMoveDn.Name = "btnObjectMoveDn";
            this.btnObjectMoveDn.Size = new System.Drawing.Size(25, 25);
            this.btnObjectMoveDn.TabIndex = 14;
            this.btnObjectMoveDn.Text = "하";
            this.btnObjectMoveDn.UseVisualStyleBackColor = true;
            this.btnObjectMoveDn.Click += new System.EventHandler(this.btnObjectMoveDn_Click);
            // 
            // btnObjectMoveRight
            // 
            this.btnObjectMoveRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectMoveRight.Location = new System.Drawing.Point(189, 45);
            this.btnObjectMoveRight.Name = "btnObjectMoveRight";
            this.btnObjectMoveRight.Size = new System.Drawing.Size(25, 25);
            this.btnObjectMoveRight.TabIndex = 13;
            this.btnObjectMoveRight.Text = "우";
            this.btnObjectMoveRight.UseVisualStyleBackColor = true;
            this.btnObjectMoveRight.Click += new System.EventHandler(this.btnObjectMoveRight_Click);
            // 
            // btnObjectMoveLeft
            // 
            this.btnObjectMoveLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectMoveLeft.Location = new System.Drawing.Point(139, 45);
            this.btnObjectMoveLeft.Name = "btnObjectMoveLeft";
            this.btnObjectMoveLeft.Size = new System.Drawing.Size(25, 25);
            this.btnObjectMoveLeft.TabIndex = 12;
            this.btnObjectMoveLeft.Text = "좌";
            this.btnObjectMoveLeft.UseVisualStyleBackColor = true;
            this.btnObjectMoveLeft.Click += new System.EventHandler(this.btnObjectMoveLeft_Click);
            // 
            // btnObjectMoveUp
            // 
            this.btnObjectMoveUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectMoveUp.Location = new System.Drawing.Point(164, 20);
            this.btnObjectMoveUp.Name = "btnObjectMoveUp";
            this.btnObjectMoveUp.Size = new System.Drawing.Size(25, 25);
            this.btnObjectMoveUp.TabIndex = 11;
            this.btnObjectMoveUp.Text = "상";
            this.btnObjectMoveUp.UseVisualStyleBackColor = true;
            this.btnObjectMoveUp.Click += new System.EventHandler(this.btnObjectMoveUp_Click);
            // 
            // btnObjectMove
            // 
            this.btnObjectMove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectMove.Location = new System.Drawing.Point(164, 45);
            this.btnObjectMove.Name = "btnObjectMove";
            this.btnObjectMove.Size = new System.Drawing.Size(25, 25);
            this.btnObjectMove.TabIndex = 10;
            this.btnObjectMove.Text = "A";
            this.btnObjectMove.UseVisualStyleBackColor = true;
            this.btnObjectMove.Click += new System.EventHandler(this.btnObjectMove_Click);
            // 
            // txtObjectMoveT
            // 
            this.txtObjectMoveT.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtObjectMoveT.Location = new System.Drawing.Point(71, 72);
            this.txtObjectMoveT.Name = "txtObjectMoveT";
            this.txtObjectMoveT.Size = new System.Drawing.Size(65, 22);
            this.txtObjectMoveT.TabIndex = 9;
            this.txtObjectMoveT.Text = "0";
            this.txtObjectMoveT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "각도";
            // 
            // txtObjectMoveY
            // 
            this.txtObjectMoveY.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtObjectMoveY.Location = new System.Drawing.Point(71, 45);
            this.txtObjectMoveY.Name = "txtObjectMoveY";
            this.txtObjectMoveY.Size = new System.Drawing.Size(65, 22);
            this.txtObjectMoveY.TabIndex = 3;
            this.txtObjectMoveY.Text = "5";
            this.txtObjectMoveY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "위치 Y";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtObjectMoveX
            // 
            this.txtObjectMoveX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtObjectMoveX.Location = new System.Drawing.Point(71, 20);
            this.txtObjectMoveX.Name = "txtObjectMoveX";
            this.txtObjectMoveX.Size = new System.Drawing.Size(65, 22);
            this.txtObjectMoveX.TabIndex = 1;
            this.txtObjectMoveX.Text = "5";
            this.txtObjectMoveX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "위치 X";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.btnObjectChange);
            this.groupBox1.Controls.Add(this.txtObjectAngle);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtObjectEndPosY);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtObjectEndPosX);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtObjectStartPosY);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtObjectStartPosX);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 154);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "위치";
            // 
            // btnObjectChange
            // 
            this.btnObjectChange.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectChange.Location = new System.Drawing.Point(151, 20);
            this.btnObjectChange.Name = "btnObjectChange";
            this.btnObjectChange.Size = new System.Drawing.Size(63, 122);
            this.btnObjectChange.TabIndex = 10;
            this.btnObjectChange.Text = "변경";
            this.btnObjectChange.UseVisualStyleBackColor = true;
            this.btnObjectChange.Click += new System.EventHandler(this.btnObjectChange_Click);
            // 
            // txtObjectAngle
            // 
            this.txtObjectAngle.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtObjectAngle.Location = new System.Drawing.Point(71, 121);
            this.txtObjectAngle.Name = "txtObjectAngle";
            this.txtObjectAngle.Size = new System.Drawing.Size(65, 22);
            this.txtObjectAngle.TabIndex = 9;
            this.txtObjectAngle.Text = "0";
            this.txtObjectAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "각도";
            // 
            // txtObjectEndPosY
            // 
            this.txtObjectEndPosY.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtObjectEndPosY.Location = new System.Drawing.Point(71, 96);
            this.txtObjectEndPosY.Name = "txtObjectEndPosY";
            this.txtObjectEndPosY.Size = new System.Drawing.Size(65, 22);
            this.txtObjectEndPosY.TabIndex = 7;
            this.txtObjectEndPosY.Text = "0";
            this.txtObjectEndPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "2nd 위치 Y";
            // 
            // txtObjectEndPosX
            // 
            this.txtObjectEndPosX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtObjectEndPosX.Location = new System.Drawing.Point(71, 71);
            this.txtObjectEndPosX.Name = "txtObjectEndPosX";
            this.txtObjectEndPosX.Size = new System.Drawing.Size(65, 22);
            this.txtObjectEndPosX.TabIndex = 5;
            this.txtObjectEndPosX.Text = "0";
            this.txtObjectEndPosX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "2nd 위치 X";
            // 
            // txtObjectStartPosY
            // 
            this.txtObjectStartPosY.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtObjectStartPosY.Location = new System.Drawing.Point(71, 45);
            this.txtObjectStartPosY.Name = "txtObjectStartPosY";
            this.txtObjectStartPosY.Size = new System.Drawing.Size(65, 22);
            this.txtObjectStartPosY.TabIndex = 3;
            this.txtObjectStartPosY.Text = "0";
            this.txtObjectStartPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "1st 위치 Y";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtObjectStartPosX
            // 
            this.txtObjectStartPosX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtObjectStartPosX.Location = new System.Drawing.Point(71, 20);
            this.txtObjectStartPosX.Name = "txtObjectStartPosX";
            this.txtObjectStartPosX.Size = new System.Drawing.Size(65, 22);
            this.txtObjectStartPosX.TabIndex = 1;
            this.txtObjectStartPosX.Text = "0";
            this.txtObjectStartPosX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "1st 위치 X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pageEntity
            // 
            this.pageEntity.Controls.Add(this.ShapeListView);
            this.pageEntity.Location = new System.Drawing.Point(4, 22);
            this.pageEntity.Name = "pageEntity";
            this.pageEntity.Padding = new System.Windows.Forms.Padding(3);
            this.pageEntity.Size = new System.Drawing.Size(235, 448);
            this.pageEntity.TabIndex = 1;
            this.pageEntity.Text = "Entity";
            this.pageEntity.UseVisualStyleBackColor = true;
            // 
            // pageHatch
            // 
            this.pageHatch.Location = new System.Drawing.Point(4, 22);
            this.pageHatch.Name = "pageHatch";
            this.pageHatch.Padding = new System.Windows.Forms.Padding(3);
            this.pageHatch.Size = new System.Drawing.Size(235, 448);
            this.pageHatch.TabIndex = 2;
            this.pageHatch.Text = "Hatch";
            this.pageHatch.UseVisualStyleBackColor = true;
            // 
            // pnlObject
            // 
            this.pnlObject.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlObject.Controls.Add(this.btnPanDn);
            this.pnlObject.Controls.Add(this.btnPanUp);
            this.pnlObject.Controls.Add(this.btnPanRight);
            this.pnlObject.Controls.Add(this.btnPanLeft);
            this.pnlObject.Controls.Add(this.menuScanner);
            this.pnlObject.Location = new System.Drawing.Point(12, 282);
            this.pnlObject.Name = "pnlObject";
            this.pnlObject.Size = new System.Drawing.Size(143, 40);
            this.pnlObject.TabIndex = 13;
            // 
            // btnPanDn
            // 
            this.btnPanDn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPanDn.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPanDn.Location = new System.Drawing.Point(97, -1);
            this.btnPanDn.Name = "btnPanDn";
            this.btnPanDn.Size = new System.Drawing.Size(32, 32);
            this.btnPanDn.TabIndex = 9;
            this.btnPanDn.Text = "dn";
            this.btnPanDn.UseVisualStyleBackColor = true;
            this.btnPanDn.Click += new System.EventHandler(this.btnPanDn_Click);
            // 
            // btnPanUp
            // 
            this.btnPanUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPanUp.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPanUp.Location = new System.Drawing.Point(66, -1);
            this.btnPanUp.Name = "btnPanUp";
            this.btnPanUp.Size = new System.Drawing.Size(32, 32);
            this.btnPanUp.TabIndex = 10;
            this.btnPanUp.Text = "up";
            this.btnPanUp.UseVisualStyleBackColor = true;
            this.btnPanUp.Click += new System.EventHandler(this.btnPanUp_Click);
            // 
            // btnPanRight
            // 
            this.btnPanRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPanRight.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPanRight.Location = new System.Drawing.Point(35, -1);
            this.btnPanRight.Name = "btnPanRight";
            this.btnPanRight.Size = new System.Drawing.Size(32, 32);
            this.btnPanRight.TabIndex = 11;
            this.btnPanRight.Text = ">";
            this.btnPanRight.UseVisualStyleBackColor = true;
            this.btnPanRight.Click += new System.EventHandler(this.btnPanRight_Click);
            // 
            // btnPanLeft
            // 
            this.btnPanLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPanLeft.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPanLeft.Location = new System.Drawing.Point(4, -1);
            this.btnPanLeft.Name = "btnPanLeft";
            this.btnPanLeft.Size = new System.Drawing.Size(32, 32);
            this.btnPanLeft.TabIndex = 12;
            this.btnPanLeft.Text = "<";
            this.btnPanLeft.UseVisualStyleBackColor = true;
            this.btnPanLeft.Click += new System.EventHandler(this.btnPanLeft_Click);
            // 
            // tmrView
            // 
            this.tmrView.Enabled = true;
            this.tmrView.Tick += new System.EventHandler(this.tmrView_Tick);
            // 
            // BtnConfigureExit
            // 
            this.BtnConfigureExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnConfigureExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnConfigureExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnConfigureExit.Image")));
            this.BtnConfigureExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnConfigureExit.Location = new System.Drawing.Point(15, 350);
            this.BtnConfigureExit.Name = "BtnConfigureExit";
            this.BtnConfigureExit.Size = new System.Drawing.Size(124, 67);
            this.BtnConfigureExit.TabIndex = 756;
            this.BtnConfigureExit.Text = " Exit";
            this.BtnConfigureExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnConfigureExit.UseVisualStyleBackColor = true;
            this.BtnConfigureExit.Click += new System.EventHandler(this.BtnConfigureExit_Click);
            // 
            // ribbonControl
            // 
            this.ribbonControl.AutoSize = true;
            this.ribbonControl.Dock = Syncfusion.Windows.Forms.Tools.DockStyleEx.Fill;
            this.ribbonControl.Header.AddMainItem(toolTabDraw);
            this.ribbonControl.Header.AddMainItem(toolTabProperty);
            this.ribbonControl.Header.AddMainItem(toolTabConvert);
            this.ribbonControl.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl.Name = "ribbonControl";
            // 
            // ribbonControl.OfficeMenu
            // 
            this.ribbonControl.OfficeMenu.Name = "OfficeMenu";
            this.ribbonControl.OfficeMenu.ShowItemToolTips = true;
            this.ribbonControl.OfficeMenu.Size = new System.Drawing.Size(12, 65);
            this.ribbonControl.OfficeMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ribbonControl_OfficeMenu_ItemClicked);
            this.ribbonControl.Size = new System.Drawing.Size(1253, 188);
            this.ribbonControl.SystemText.QuickAccessDialogDropDownName = "Start menu";
            this.ribbonControl.TabIndex = 757;
            this.ribbonControl.Text = "ribbonControlAdv1";
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
            this.toolTabDraw.Panel.Controls.Add(this.toolStripEx5);
            this.toolTabDraw.Panel.Controls.Add(this.toolStripEx7);
            this.toolTabDraw.Panel.Controls.Add(this.toolStripEx2);
            this.toolTabDraw.Panel.Controls.Add(this.toolArrayCopy);
            this.toolTabDraw.Panel.Controls.Add(this.toolGroup);
            this.toolTabDraw.Panel.Name = "ribbonPanel1";
            this.toolTabDraw.Panel.ScrollPosition = 0;
            this.toolTabDraw.Panel.TabIndex = 2;
            this.toolTabDraw.Panel.Text = "Draw";
            this.toolTabDraw.Position = 0;
            this.toolTabDraw.Size = new System.Drawing.Size(48, 20);
            this.toolTabDraw.Text = "Draw";
            // 
            // toolStripEx2
            // 
            this.toolStripEx2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx2.Image = null;
            this.toolStripEx2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem2});
            this.toolStripEx2.Location = new System.Drawing.Point(303, 1);
            this.toolStripEx2.Name = "toolStripEx2";
            this.toolStripEx2.Size = new System.Drawing.Size(94, 132);
            this.toolStripEx2.TabIndex = 3;
            this.toolStripEx2.Text = "Zoom";
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
            this.toolStripPanelItem2.Size = new System.Drawing.Size(76, 110);
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
            // toolArrayCopy
            // 
            this.toolArrayCopy.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolArrayCopy.Image = null;
            this.toolArrayCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem1,
            this.toolBtnArrayCopy});
            this.toolArrayCopy.Location = new System.Drawing.Point(399, 1);
            this.toolArrayCopy.Name = "toolArrayCopy";
            this.toolArrayCopy.Size = new System.Drawing.Size(169, 132);
            this.toolArrayCopy.TabIndex = 1;
            this.toolArrayCopy.Text = "Array Copy";
            // 
            // toolStripPanelItem1
            // 
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
            this.toolStripPanelItem1.Size = new System.Drawing.Size(101, 110);
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
            // txtCopyPitchX
            // 
            this.txtCopyPitchX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopyPitchX.Name = "txtCopyPitchX";
            this.txtCopyPitchX.Size = new System.Drawing.Size(50, 23);
            this.txtCopyPitchX.Text = "1";
            this.txtCopyPitchX.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCopyNumX
            // 
            this.txtCopyNumX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopyNumX.Name = "txtCopyNumX";
            this.txtCopyNumX.Size = new System.Drawing.Size(50, 23);
            this.txtCopyNumX.Text = "1";
            this.txtCopyNumX.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // toolGroup
            // 
            this.toolGroup.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolGroup.Image = null;
            this.toolGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem5});
            this.toolGroup.Location = new System.Drawing.Point(570, 1);
            this.toolGroup.Name = "toolGroup";
            this.toolGroup.Size = new System.Drawing.Size(82, 132);
            this.toolGroup.TabIndex = 0;
            this.toolGroup.Text = "Group";
            // 
            // toolTabProperty
            // 
            this.toolTabProperty.Name = "toolTabProperty";
            // 
            // ribbonControl.ribbonPanel2
            // 
            this.toolTabProperty.Panel.Controls.Add(this.toolStripEx1);
            this.toolTabProperty.Panel.Controls.Add(this.toolStripEx6);
            this.toolTabProperty.Panel.Controls.Add(this.toolStripEx3);
            this.toolTabProperty.Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolTabProperty.Panel.Name = "ribbonPanel2";
            this.toolTabProperty.Panel.ScrollPosition = 0;
            this.toolTabProperty.Panel.TabIndex = 3;
            this.toolTabProperty.Panel.Text = "Property";
            this.toolTabProperty.Panel.Click += new System.EventHandler(this.RibbonPanel_Click);
            this.toolTabProperty.Position = 1;
            this.toolTabProperty.Size = new System.Drawing.Size(58, 18);
            this.toolTabProperty.Text = "Property";
            // 
            // toolStripEx6
            // 
            this.toolStripEx6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx6.Image = null;
            this.toolStripEx6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem4});
            this.toolStripEx6.Location = new System.Drawing.Point(171, 1);
            this.toolStripEx6.Name = "toolStripEx6";
            this.toolStripEx6.Size = new System.Drawing.Size(68, 132);
            this.toolStripEx6.TabIndex = 2;
            this.toolStripEx6.Text = "Modify";
            // 
            // toolStripPanelItem4
            // 
            this.toolStripPanelItem4.CausesValidation = false;
            this.toolStripPanelItem4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton8,
            this.toolStripButton9,
            this.toolStripButton10,
            this.toolStripButton11,
            this.toolStripButton12,
            this.toolStripButton13});
            this.toolStripPanelItem4.Name = "toolStripPanelItem4";
            this.toolStripPanelItem4.Size = new System.Drawing.Size(50, 110);
            this.toolStripPanelItem4.Text = "toolStripPanelItem1";
            this.toolStripPanelItem4.Transparent = true;
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton8.Text = "toolStripButton1";
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton9.Image")));
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton9.Text = "toolStripButton2";
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton10.Image")));
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton10.Text = "toolStripButton3";
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton11.Image")));
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton11.Text = "toolStripButton4";
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton12.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton12.Image")));
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton12.Text = "toolStripButton5";
            // 
            // toolStripButton13
            // 
            this.toolStripButton13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton13.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton13.Image")));
            this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton13.Name = "toolStripButton13";
            this.toolStripButton13.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton13.Text = "toolStripButton6";
            // 
            // toolStripEx3
            // 
            this.toolStripEx3.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripEx3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEx3.Image = null;
            this.toolStripEx3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripTextBox1,
            this.toolStripButton7});
            this.toolStripEx3.Location = new System.Drawing.Point(241, 1);
            this.toolStripEx3.Name = "toolStripEx3";
            this.toolStripEx3.Size = new System.Drawing.Size(220, 132);
            this.toolStripEx3.TabIndex = 0;
            this.toolStripEx3.Text = "toolStripEx3";
            this.toolStripEx3.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripEx3_ItemClicked);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(88, 107);
            this.toolStripLabel1.Text = "toolStripLabel1";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 110);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 107);
            this.toolStripButton7.Text = "toolStripButton7";
            // 
            // toolTabConvert
            // 
            this.toolTabConvert.Name = "toolTabConvert";
            // 
            // ribbonControl.ribbonPanel3
            // 
            this.toolTabConvert.Panel.Controls.Add(this.toolStripEx4);
            this.toolTabConvert.Panel.Name = "ribbonPanel3";
            this.toolTabConvert.Panel.ScrollPosition = 0;
            this.toolTabConvert.Panel.TabIndex = 4;
            this.toolTabConvert.Panel.Text = "Save to BMP";
            this.toolTabConvert.Position = 2;
            this.toolTabConvert.Size = new System.Drawing.Size(84, 18);
            this.toolTabConvert.Text = "Save to BMP";
            // 
            // toolStripEx4
            // 
            this.toolStripEx4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx4.Image = null;
            this.toolStripEx4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnSaveBmp,
            this.toolBtnSaveLse});
            this.toolStripEx4.Location = new System.Drawing.Point(0, 1);
            this.toolStripEx4.Name = "toolStripEx4";
            this.toolStripEx4.Size = new System.Drawing.Size(416, 128);
            this.toolStripEx4.TabIndex = 4;
            this.toolStripEx4.Text = "Convert";
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
            // toolStripPanelItem5
            // 
            this.toolStripPanelItem5.CausesValidation = false;
            this.toolStripPanelItem5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnGroup,
            this.toolBtnUnGroup});
            this.toolStripPanelItem5.Name = "toolStripPanelItem5";
            this.toolStripPanelItem5.RowCount = 2;
            this.toolStripPanelItem5.Size = new System.Drawing.Size(64, 110);
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
            // toolStripEx1
            // 
            this.toolStripEx1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx1.Image = null;
            this.toolStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem6,
            this.toolBtnChange});
            this.toolStripEx1.Location = new System.Drawing.Point(0, 1);
            this.toolStripEx1.Name = "toolStripEx1";
            this.toolStripEx1.Size = new System.Drawing.Size(169, 132);
            this.toolStripEx1.TabIndex = 3;
            this.toolStripEx1.Text = "Property";
            // 
            // toolStripPanelItem6
            // 
            this.toolStripPanelItem6.CausesValidation = false;
            this.toolStripPanelItem6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripPanelItem6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.toolStripLabel7,
            this.toolStripLabel8,
            this.toolStripLabel9,
            this.toolStripTextBox2,
            this.toolStripTextBox3,
            this.toolStripTextBox4,
            this.toolStripTextBox5});
            this.toolStripPanelItem6.Name = "toolStripPanelItem6";
            this.toolStripPanelItem6.RowCount = 4;
            this.toolStripPanelItem6.Size = new System.Drawing.Size(101, 110);
            this.toolStripPanelItem6.Text = "toolStripPanelItem1";
            this.toolStripPanelItem6.Transparent = true;
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(45, 15);
            this.toolStripLabel6.Text = "X Num";
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(45, 15);
            this.toolStripLabel7.Text = "X Pitch";
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(45, 15);
            this.toolStripLabel8.Text = "Y Num";
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(45, 15);
            this.toolStripLabel9.Text = "Y Pitch";
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(50, 23);
            this.toolStripTextBox2.Text = "1";
            this.toolStripTextBox2.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripTextBox3
            // 
            this.toolStripTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolStripTextBox3.Name = "toolStripTextBox3";
            this.toolStripTextBox3.Size = new System.Drawing.Size(50, 23);
            this.toolStripTextBox3.Text = "1";
            this.toolStripTextBox3.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripTextBox4
            // 
            this.toolStripTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolStripTextBox4.Name = "toolStripTextBox4";
            this.toolStripTextBox4.Size = new System.Drawing.Size(50, 23);
            this.toolStripTextBox4.Text = "1";
            this.toolStripTextBox4.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripTextBox5
            // 
            this.toolStripTextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolStripTextBox5.Name = "toolStripTextBox5";
            this.toolStripTextBox5.Size = new System.Drawing.Size(50, 23);
            this.toolStripTextBox5.Text = "1";
            this.toolStripTextBox5.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolBtnChange
            // 
            this.toolBtnChange.AutoSize = false;
            this.toolBtnChange.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnChange.Image")));
            this.toolBtnChange.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnChange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnChange.Name = "toolBtnChange";
            this.toolBtnChange.Size = new System.Drawing.Size(50, 107);
            this.toolBtnChange.Text = "Change";
            this.toolBtnChange.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolBtnChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripEx7
            // 
            this.toolStripEx7.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx7.Image = null;
            this.toolStripEx7.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem7});
            this.toolStripEx7.Location = new System.Drawing.Point(219, 1);
            this.toolStripEx7.Name = "toolStripEx7";
            this.toolStripEx7.Size = new System.Drawing.Size(82, 132);
            this.toolStripEx7.TabIndex = 6;
            this.toolStripEx7.Text = "Delete";
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
            this.toolStripPanelItem7.Size = new System.Drawing.Size(64, 110);
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
            this.toolDraw.Size = new System.Drawing.Size(121, 132);
            this.toolDraw.TabIndex = 8;
            this.toolDraw.Text = "Draw";
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
            this.toolPnlDraw.Size = new System.Drawing.Size(112, 110);
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
            // toolStripEx5
            // 
            this.toolStripEx5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx5.Image = null;
            this.toolStripEx5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPanelItem3});
            this.toolStripEx5.Location = new System.Drawing.Point(123, 1);
            this.toolStripEx5.Name = "toolStripEx5";
            this.toolStripEx5.Size = new System.Drawing.Size(94, 132);
            this.toolStripEx5.TabIndex = 7;
            this.toolStripEx5.Text = "Draw 2";
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
            this.toolStripPanelItem3.Size = new System.Drawing.Size(76, 110);
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlCanvas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabProperty);
            this.splitContainer1.Size = new System.Drawing.Size(1253, 631);
            this.splitContainer1.SplitterDistance = 979;
            this.splitContainer1.TabIndex = 758;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.MainMenuStrip = this.menuScanner;
            this.Name = "FormScanWindow";
            this.Text = "Laser Scan System";
            this.Activated += new System.EventHandler(this.FormScanWindow_Activated);
            this.Load += new System.EventHandler(this.FormScanWindow_Load);
            this.Resize += new System.EventHandler(this.CWindowForm_Resize);
            this.statusBottom.ResumeLayout(false);
            this.statusBottom.PerformLayout();
            this.menuScanner.ResumeLayout(false);
            this.menuScanner.PerformLayout();
            this.tabProperty.ResumeLayout(false);
            this.pageProperty.ResumeLayout(false);
            this.pageProperty.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pageEntity.ResumeLayout(false);
            this.pnlObject.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.ribbonControl.ResumeLayout(false);
            this.ribbonControl.PerformLayout();
            this.toolTabDraw.Panel.ResumeLayout(false);
            this.toolTabDraw.Panel.PerformLayout();
            this.toolStripEx2.ResumeLayout(false);
            this.toolStripEx2.PerformLayout();
            this.toolArrayCopy.ResumeLayout(false);
            this.toolArrayCopy.PerformLayout();
            this.toolGroup.ResumeLayout(false);
            this.toolGroup.PerformLayout();
            this.toolTabProperty.Panel.ResumeLayout(false);
            this.toolTabProperty.Panel.PerformLayout();
            this.toolStripEx6.ResumeLayout(false);
            this.toolStripEx6.PerformLayout();
            this.toolStripEx3.ResumeLayout(false);
            this.toolStripEx3.PerformLayout();
            this.toolTabConvert.Panel.ResumeLayout(false);
            this.toolTabConvert.Panel.PerformLayout();
            this.toolStripEx4.ResumeLayout(false);
            this.toolStripEx4.PerformLayout();
            this.toolStripEx1.ResumeLayout(false);
            this.toolStripEx1.PerformLayout();
            this.toolStripEx7.ResumeLayout(false);
            this.toolStripEx7.PerformLayout();
            this.toolDraw.ResumeLayout(false);
            this.toolDraw.PerformLayout();
            this.toolStripEx5.ResumeLayout(false);
            this.toolStripEx5.PerformLayout();
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
        private System.Windows.Forms.MenuStrip menuScanner;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem penToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer6ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer7ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer9ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layer10ToolStripMenuItem;
        private System.Windows.Forms.ListView ShapeListView;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.TabControl tabProperty;
        private System.Windows.Forms.TabPage pageProperty;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnObjectMoveDn;
        private System.Windows.Forms.Button btnObjectMoveRight;
        private System.Windows.Forms.Button btnObjectMoveLeft;
        private System.Windows.Forms.Button btnObjectMoveUp;
        private System.Windows.Forms.Button btnObjectMove;
        private System.Windows.Forms.TextBox txtObjectMoveT;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtObjectMoveY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtObjectMoveX;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnObjectChange;
        private System.Windows.Forms.TextBox txtObjectAngle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtObjectEndPosY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtObjectEndPosX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtObjectStartPosY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtObjectStartPosX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage pageEntity;
        private System.Windows.Forms.TabPage pageHatch;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        public System.Windows.Forms.Label lblMousePos;
        private System.Windows.Forms.ToolStripMenuItem 설정ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem polygonToolStripMenuItem;
        private System.Windows.Forms.Panel pnlObject;
        private System.Windows.Forms.Button BtnConfigureExit;
        private System.Windows.Forms.Button btnPanDn;
        private System.Windows.Forms.Button btnPanUp;
        private System.Windows.Forms.Button btnPanRight;
        private System.Windows.Forms.Button btnPanLeft;
        private System.Windows.Forms.Timer tmrView;
        private System.Windows.Forms.ToolStripMenuItem pointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rectangleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ellipseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private Syncfusion.Windows.Forms.Tools.RibbonControlAdv ribbonControl;
        private Syncfusion.Windows.Forms.Tools.ToolStripTabItem toolTabDraw;
        private Syncfusion.Windows.Forms.Tools.ToolStripTabItem toolTabProperty;
        private Syncfusion.Windows.Forms.Tools.ToolStripTabItem toolTabConvert;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolArrayCopy;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolGroup;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem1;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx2;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx4;
        private System.Windows.Forms.ToolStripButton toolBtnSaveBmp;
        private System.Windows.Forms.ToolStripButton toolBtnSaveLse;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem2;
        private System.Windows.Forms.ToolStripButton toolBtnZoomPlus;
        private System.Windows.Forms.ToolStripButton toolBtnZoomDraw;
        private System.Windows.Forms.ToolStripButton toolBtnZoomMinus;
        private System.Windows.Forms.ToolStripButton toolBtnZoomAll;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx6;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem4;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
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
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx1;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel9;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox3;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox4;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox5;
        private System.Windows.Forms.ToolStripButton toolBtnChange;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolDraw;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolPnlDraw;
        private System.Windows.Forms.ToolStripButton toolBtnDot;
        private System.Windows.Forms.ToolStripButton toolBtnLine;
        private System.Windows.Forms.ToolStripButton toolBtnArc;
        private System.Windows.Forms.ToolStripButton toolBtnRectacgle;
        private System.Windows.Forms.ToolStripButton toolBtnCircle;
        private System.Windows.Forms.ToolStripButton toolBtnEllipse;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx5;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem3;
        private System.Windows.Forms.ToolStripButton toolBtnFont;
        private System.Windows.Forms.ToolStripButton toolBtnBmp;
        private System.Windows.Forms.ToolStripButton toolBtnDxf;
        private System.Windows.Forms.ToolStripButton toolBtnNone;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx7;
        private Syncfusion.Windows.Forms.Tools.ToolStripPanelItem toolStripPanelItem7;
        private System.Windows.Forms.ToolStripButton toolBtnDelete;
        private System.Windows.Forms.ToolStripButton toolBtnDeleteAll;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}