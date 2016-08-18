using LWDicer.Control;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScanWindow));
            this.statusBottom = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuScanner = new System.Windows.Forms.MenuStrip();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.penToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pen1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pen1ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pen2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pen3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pen4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pen5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.배경ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.밝음ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.어두움ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.생성ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.비활성화ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.panList = new System.Windows.Forms.Panel();
            this.btnObjectDeleteAll = new System.Windows.Forms.Button();
            this.btnObjectDelete = new System.Windows.Forms.Button();
            this.btnObjectUngroup = new System.Windows.Forms.Button();
            this.btnObjectGroup = new System.Windows.Forms.Button();
            this.ShapeListView = new System.Windows.Forms.ListView();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.tabProperty = new System.Windows.Forms.TabControl();
            this.pageProperty = new System.Windows.Forms.TabPage();
            this.lblMousePos = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnObjectArrayCopy = new System.Windows.Forms.Button();
            this.txtArrayGapY = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtArrayNumY = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtArrayGapX = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtArrayNumX = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
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
            this.btnNone = new System.Windows.Forms.Button();
            this.btnDxf = new System.Windows.Forms.Button();
            this.btnBmp = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnZoomAll = new System.Windows.Forms.Button();
            this.btnSortRight = new System.Windows.Forms.Button();
            this.btnZoomMouse = new System.Windows.Forms.Button();
            this.btnFont = new System.Windows.Forms.Button();
            this.btnSortDn = new System.Windows.Forms.Button();
            this.btnZoomMinus = new System.Windows.Forms.Button();
            this.btnCircle = new System.Windows.Forms.Button();
            this.btnSortUp = new System.Windows.Forms.Button();
            this.btnZoomPlus = new System.Windows.Forms.Button();
            this.btnRect = new System.Windows.Forms.Button();
            this.btnSortLeft = new System.Windows.Forms.Button();
            this.btnLine = new System.Windows.Forms.Button();
            this.btnDot = new System.Windows.Forms.Button();
            this.pnlObject = new System.Windows.Forms.Panel();
            this.btnImageSave = new System.Windows.Forms.Button();
            this.statusBottom.SuspendLayout();
            this.menuScanner.SuspendLayout();
            this.panList.SuspendLayout();
            this.tabProperty.SuspendLayout();
            this.pageProperty.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlObject.SuspendLayout();
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
            this.statusBottom.Size = new System.Drawing.Size(1225, 22);
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
            this.menuScanner.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuScanner.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.penToolStripMenuItem,
            this.layerToolStripMenuItem,
            this.설정ToolStripMenuItem});
            this.menuScanner.Location = new System.Drawing.Point(0, 0);
            this.menuScanner.Name = "menuScanner";
            this.menuScanner.Size = new System.Drawing.Size(1225, 24);
            this.menuScanner.TabIndex = 3;
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.homeToolStripMenuItem.Text = "Main";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.homeToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.openToolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.printToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(93, 22);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem1.Text = "New";
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem1.Text = "Open";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.printToolStripMenuItem.Text = "Print";
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
            this.pen1ToolStripMenuItem,
            this.배경ToolStripMenuItem,
            this.gridToolStripMenuItem});
            this.penToolStripMenuItem.Name = "penToolStripMenuItem";
            this.penToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.penToolStripMenuItem.Text = "Draw";
            this.penToolStripMenuItem.Click += new System.EventHandler(this.penToolStripMenuItem_Click);
            // 
            // pen1ToolStripMenuItem
            // 
            this.pen1ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pen1ToolStripMenuItem1,
            this.pen2ToolStripMenuItem,
            this.pen3ToolStripMenuItem,
            this.pen4ToolStripMenuItem,
            this.pen5ToolStripMenuItem});
            this.pen1ToolStripMenuItem.Name = "pen1ToolStripMenuItem";
            this.pen1ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.pen1ToolStripMenuItem.Text = "Pen";
            // 
            // pen1ToolStripMenuItem1
            // 
            this.pen1ToolStripMenuItem1.Name = "pen1ToolStripMenuItem1";
            this.pen1ToolStripMenuItem1.Size = new System.Drawing.Size(101, 22);
            this.pen1ToolStripMenuItem1.Text = "Pen1";
            // 
            // pen2ToolStripMenuItem
            // 
            this.pen2ToolStripMenuItem.Name = "pen2ToolStripMenuItem";
            this.pen2ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.pen2ToolStripMenuItem.Text = "Pen2";
            // 
            // pen3ToolStripMenuItem
            // 
            this.pen3ToolStripMenuItem.Name = "pen3ToolStripMenuItem";
            this.pen3ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.pen3ToolStripMenuItem.Text = "Pen3";
            // 
            // pen4ToolStripMenuItem
            // 
            this.pen4ToolStripMenuItem.Name = "pen4ToolStripMenuItem";
            this.pen4ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.pen4ToolStripMenuItem.Text = "Pen4";
            // 
            // pen5ToolStripMenuItem
            // 
            this.pen5ToolStripMenuItem.Name = "pen5ToolStripMenuItem";
            this.pen5ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.pen5ToolStripMenuItem.Text = "Pen5";
            // 
            // 배경ToolStripMenuItem
            // 
            this.배경ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.밝음ToolStripMenuItem,
            this.어두움ToolStripMenuItem});
            this.배경ToolStripMenuItem.Name = "배경ToolStripMenuItem";
            this.배경ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.배경ToolStripMenuItem.Text = "배경";
            // 
            // 밝음ToolStripMenuItem
            // 
            this.밝음ToolStripMenuItem.Name = "밝음ToolStripMenuItem";
            this.밝음ToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.밝음ToolStripMenuItem.Text = "밝음";
            this.밝음ToolStripMenuItem.Click += new System.EventHandler(this.밝음ToolStripMenuItem_Click);
            // 
            // 어두움ToolStripMenuItem
            // 
            this.어두움ToolStripMenuItem.Name = "어두움ToolStripMenuItem";
            this.어두움ToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.어두움ToolStripMenuItem.Text = "어두움";
            this.어두움ToolStripMenuItem.Click += new System.EventHandler(this.어두움ToolStripMenuItem_Click);
            // 
            // gridToolStripMenuItem
            // 
            this.gridToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.생성ToolStripMenuItem,
            this.비활성화ToolStripMenuItem});
            this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
            this.gridToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.gridToolStripMenuItem.Text = "격자";
            // 
            // 생성ToolStripMenuItem
            // 
            this.생성ToolStripMenuItem.Name = "생성ToolStripMenuItem";
            this.생성ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.생성ToolStripMenuItem.Text = "활성화";
            this.생성ToolStripMenuItem.Click += new System.EventHandler(this.생성ToolStripMenuItem_Click);
            // 
            // 비활성화ToolStripMenuItem
            // 
            this.비활성화ToolStripMenuItem.Name = "비활성화ToolStripMenuItem";
            this.비활성화ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.비활성화ToolStripMenuItem.Text = "비활성화";
            this.비활성화ToolStripMenuItem.Click += new System.EventHandler(this.비활성화ToolStripMenuItem_Click);
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
            this.layerToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
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
            this.설정ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.설정ToolStripMenuItem.Text = "설정";
            // 
            // polygonToolStripMenuItem
            // 
            this.polygonToolStripMenuItem.Name = "polygonToolStripMenuItem";
            this.polygonToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.polygonToolStripMenuItem.Text = "Polygon";
            this.polygonToolStripMenuItem.Click += new System.EventHandler(this.polygonToolStripMenuItem_Click);
            // 
            // panList
            // 
            this.panList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panList.Controls.Add(this.btnObjectDeleteAll);
            this.panList.Controls.Add(this.btnObjectDelete);
            this.panList.Controls.Add(this.btnObjectUngroup);
            this.panList.Controls.Add(this.btnObjectGroup);
            this.panList.Controls.Add(this.ShapeListView);
            this.panList.Location = new System.Drawing.Point(6, 174);
            this.panList.Name = "panList";
            this.panList.Size = new System.Drawing.Size(181, 645);
            this.panList.TabIndex = 9;
            // 
            // btnObjectDeleteAll
            // 
            this.btnObjectDeleteAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectDeleteAll.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnObjectDeleteAll.Location = new System.Drawing.Point(106, 613);
            this.btnObjectDeleteAll.Name = "btnObjectDeleteAll";
            this.btnObjectDeleteAll.Size = new System.Drawing.Size(35, 30);
            this.btnObjectDeleteAll.TabIndex = 11;
            this.btnObjectDeleteAll.Text = "전체 삭제";
            this.btnObjectDeleteAll.UseVisualStyleBackColor = true;
            this.btnObjectDeleteAll.Click += new System.EventHandler(this.btnShapeDeleteAll_Click);
            // 
            // btnObjectDelete
            // 
            this.btnObjectDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectDelete.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnObjectDelete.Location = new System.Drawing.Point(71, 613);
            this.btnObjectDelete.Name = "btnObjectDelete";
            this.btnObjectDelete.Size = new System.Drawing.Size(35, 30);
            this.btnObjectDelete.TabIndex = 10;
            this.btnObjectDelete.Text = "삭제";
            this.btnObjectDelete.UseVisualStyleBackColor = true;
            this.btnObjectDelete.Click += new System.EventHandler(this.btnShapeDelete_Click);
            // 
            // btnObjectUngroup
            // 
            this.btnObjectUngroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectUngroup.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnObjectUngroup.Location = new System.Drawing.Point(36, 613);
            this.btnObjectUngroup.Name = "btnObjectUngroup";
            this.btnObjectUngroup.Size = new System.Drawing.Size(35, 30);
            this.btnObjectUngroup.TabIndex = 9;
            this.btnObjectUngroup.Text = "그룹해제";
            this.btnObjectUngroup.UseVisualStyleBackColor = true;
            this.btnObjectUngroup.Click += new System.EventHandler(this.btnObjectUngroup_Click);
            // 
            // btnObjectGroup
            // 
            this.btnObjectGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectGroup.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnObjectGroup.Location = new System.Drawing.Point(0, 613);
            this.btnObjectGroup.Name = "btnObjectGroup";
            this.btnObjectGroup.Size = new System.Drawing.Size(35, 30);
            this.btnObjectGroup.TabIndex = 8;
            this.btnObjectGroup.Text = "그룹";
            this.btnObjectGroup.UseVisualStyleBackColor = true;
            this.btnObjectGroup.Click += new System.EventHandler(this.btnObjectGroup_Click);
            // 
            // ShapeListView
            // 
            this.ShapeListView.AutoArrange = false;
            this.ShapeListView.Dock = System.Windows.Forms.DockStyle.Top;
            this.ShapeListView.GridLines = true;
            this.ShapeListView.Location = new System.Drawing.Point(0, 0);
            this.ShapeListView.Name = "ShapeListView";
            this.ShapeListView.Size = new System.Drawing.Size(181, 610);
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
            this.pnlCanvas.Location = new System.Drawing.Point(192, 30);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(769, 790);
            this.pnlCanvas.TabIndex = 10;
            // 
            // tabProperty
            // 
            this.tabProperty.Controls.Add(this.pageProperty);
            this.tabProperty.Controls.Add(this.pageEntity);
            this.tabProperty.Controls.Add(this.pageHatch);
            this.tabProperty.Location = new System.Drawing.Point(975, 175);
            this.tabProperty.Name = "tabProperty";
            this.tabProperty.SelectedIndex = 0;
            this.tabProperty.Size = new System.Drawing.Size(243, 645);
            this.tabProperty.TabIndex = 12;
            // 
            // pageProperty
            // 
            this.pageProperty.Controls.Add(this.lblMousePos);
            this.pageProperty.Controls.Add(this.groupBox3);
            this.pageProperty.Controls.Add(this.groupBox2);
            this.pageProperty.Controls.Add(this.groupBox1);
            this.pageProperty.Location = new System.Drawing.Point(4, 22);
            this.pageProperty.Name = "pageProperty";
            this.pageProperty.Padding = new System.Windows.Forms.Padding(3);
            this.pageProperty.Size = new System.Drawing.Size(235, 619);
            this.pageProperty.TabIndex = 0;
            this.pageProperty.Text = "속성";
            this.pageProperty.UseVisualStyleBackColor = true;
            // 
            // lblMousePos
            // 
            this.lblMousePos.AutoSize = true;
            this.lblMousePos.Location = new System.Drawing.Point(16, 437);
            this.lblMousePos.Name = "lblMousePos";
            this.lblMousePos.Size = new System.Drawing.Size(41, 12);
            this.lblMousePos.TabIndex = 7;
            this.lblMousePos.Text = "개수 Y";
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.btnObjectArrayCopy);
            this.groupBox3.Controls.Add(this.txtArrayGapY);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtArrayNumY);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtArrayGapX);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.txtArrayNumX);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Location = new System.Drawing.Point(3, 282);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(223, 134);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "배열";
            // 
            // btnObjectArrayCopy
            // 
            this.btnObjectArrayCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnObjectArrayCopy.Location = new System.Drawing.Point(151, 20);
            this.btnObjectArrayCopy.Name = "btnObjectArrayCopy";
            this.btnObjectArrayCopy.Size = new System.Drawing.Size(63, 97);
            this.btnObjectArrayCopy.TabIndex = 10;
            this.btnObjectArrayCopy.Text = "적용";
            this.btnObjectArrayCopy.UseVisualStyleBackColor = true;
            this.btnObjectArrayCopy.Click += new System.EventHandler(this.btnObjectArrayCopy_Click_1);
            // 
            // txtArrayGapY
            // 
            this.txtArrayGapY.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtArrayGapY.Location = new System.Drawing.Point(71, 96);
            this.txtArrayGapY.Name = "txtArrayGapY";
            this.txtArrayGapY.Size = new System.Drawing.Size(65, 22);
            this.txtArrayGapY.TabIndex = 7;
            this.txtArrayGapY.Text = "1";
            this.txtArrayGapY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "간격 Y";
            // 
            // txtArrayNumY
            // 
            this.txtArrayNumY.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtArrayNumY.Location = new System.Drawing.Point(71, 71);
            this.txtArrayNumY.Name = "txtArrayNumY";
            this.txtArrayNumY.Size = new System.Drawing.Size(65, 22);
            this.txtArrayNumY.TabIndex = 5;
            this.txtArrayNumY.Text = "1";
            this.txtArrayNumY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 4;
            this.label11.Text = "개수 Y";
            // 
            // txtArrayGapX
            // 
            this.txtArrayGapX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtArrayGapX.Location = new System.Drawing.Point(71, 45);
            this.txtArrayGapX.Name = "txtArrayGapX";
            this.txtArrayGapX.Size = new System.Drawing.Size(65, 22);
            this.txtArrayGapX.TabIndex = 3;
            this.txtArrayGapX.Text = "1";
            this.txtArrayGapX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 2;
            this.label12.Text = "간격 X";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtArrayNumX
            // 
            this.txtArrayNumX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtArrayNumX.Location = new System.Drawing.Point(71, 20);
            this.txtArrayNumX.Name = "txtArrayNumX";
            this.txtArrayNumX.Size = new System.Drawing.Size(65, 22);
            this.txtArrayNumX.TabIndex = 1;
            this.txtArrayNumX.Text = "1";
            this.txtArrayNumX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "개수 X";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.pageEntity.Location = new System.Drawing.Point(4, 22);
            this.pageEntity.Name = "pageEntity";
            this.pageEntity.Padding = new System.Windows.Forms.Padding(3);
            this.pageEntity.Size = new System.Drawing.Size(235, 619);
            this.pageEntity.TabIndex = 1;
            this.pageEntity.Text = "Entity";
            this.pageEntity.UseVisualStyleBackColor = true;
            // 
            // pageHatch
            // 
            this.pageHatch.Location = new System.Drawing.Point(4, 22);
            this.pageHatch.Name = "pageHatch";
            this.pageHatch.Padding = new System.Windows.Forms.Padding(3);
            this.pageHatch.Size = new System.Drawing.Size(235, 619);
            this.pageHatch.TabIndex = 2;
            this.pageHatch.Text = "Hatch";
            this.pageHatch.UseVisualStyleBackColor = true;
            // 
            // btnNone
            // 
            this.btnNone.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNone.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNone.Location = new System.Drawing.Point(132, 35);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(32, 32);
            this.btnNone.TabIndex = 8;
            this.btnNone.Text = "해제";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // btnDxf
            // 
            this.btnDxf.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDxf.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDxf.Location = new System.Drawing.Point(101, 35);
            this.btnDxf.Name = "btnDxf";
            this.btnDxf.Size = new System.Drawing.Size(32, 32);
            this.btnDxf.TabIndex = 7;
            this.btnDxf.Text = "도면";
            this.btnDxf.UseVisualStyleBackColor = true;
            this.btnDxf.Click += new System.EventHandler(this.btnDxf_Click);
            // 
            // btnBmp
            // 
            this.btnBmp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBmp.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBmp.Location = new System.Drawing.Point(70, 35);
            this.btnBmp.Name = "btnBmp";
            this.btnBmp.Size = new System.Drawing.Size(32, 32);
            this.btnBmp.TabIndex = 6;
            this.btnBmp.Text = "그림";
            this.btnBmp.UseVisualStyleBackColor = true;
            this.btnBmp.Click += new System.EventHandler(this.btnBmp_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(1, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "줌";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(1, 77);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(39, 15);
            this.label14.TabIndex = 4;
            this.label14.Text = "정렬";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(1, 10);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(39, 15);
            this.label15.TabIndex = 3;
            this.label15.Text = "객체";
            // 
            // btnZoomAll
            // 
            this.btnZoomAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnZoomAll.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnZoomAll.Location = new System.Drawing.Point(132, 101);
            this.btnZoomAll.Name = "btnZoomAll";
            this.btnZoomAll.Size = new System.Drawing.Size(32, 32);
            this.btnZoomAll.TabIndex = 0;
            this.btnZoomAll.Text = "All";
            this.btnZoomAll.UseVisualStyleBackColor = true;
            // 
            // btnSortRight
            // 
            this.btnSortRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSortRight.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSortRight.Location = new System.Drawing.Point(132, 70);
            this.btnSortRight.Name = "btnSortRight";
            this.btnSortRight.Size = new System.Drawing.Size(32, 32);
            this.btnSortRight.TabIndex = 0;
            this.btnSortRight.Text = "우측";
            this.btnSortRight.UseVisualStyleBackColor = true;
            // 
            // btnZoomMouse
            // 
            this.btnZoomMouse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnZoomMouse.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnZoomMouse.Location = new System.Drawing.Point(101, 101);
            this.btnZoomMouse.Name = "btnZoomMouse";
            this.btnZoomMouse.Size = new System.Drawing.Size(32, 32);
            this.btnZoomMouse.TabIndex = 0;
            this.btnZoomMouse.Text = "부분";
            this.btnZoomMouse.UseVisualStyleBackColor = true;
            // 
            // btnFont
            // 
            this.btnFont.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFont.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFont.Location = new System.Drawing.Point(39, 35);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(32, 32);
            this.btnFont.TabIndex = 0;
            this.btnFont.Text = "글짜";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // btnSortDn
            // 
            this.btnSortDn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSortDn.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSortDn.Location = new System.Drawing.Point(101, 70);
            this.btnSortDn.Name = "btnSortDn";
            this.btnSortDn.Size = new System.Drawing.Size(32, 32);
            this.btnSortDn.TabIndex = 0;
            this.btnSortDn.Text = "아래";
            this.btnSortDn.UseVisualStyleBackColor = true;
            // 
            // btnZoomMinus
            // 
            this.btnZoomMinus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnZoomMinus.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnZoomMinus.Location = new System.Drawing.Point(70, 101);
            this.btnZoomMinus.Name = "btnZoomMinus";
            this.btnZoomMinus.Size = new System.Drawing.Size(32, 32);
            this.btnZoomMinus.TabIndex = 0;
            this.btnZoomMinus.Text = "축소";
            this.btnZoomMinus.UseVisualStyleBackColor = true;
            // 
            // btnCircle
            // 
            this.btnCircle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCircle.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCircle.Location = new System.Drawing.Point(132, 3);
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(32, 32);
            this.btnCircle.TabIndex = 0;
            this.btnCircle.Text = "원";
            this.btnCircle.UseVisualStyleBackColor = true;
            this.btnCircle.Click += new System.EventHandler(this.btnCircle_Click);
            // 
            // btnSortUp
            // 
            this.btnSortUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSortUp.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSortUp.Location = new System.Drawing.Point(70, 70);
            this.btnSortUp.Name = "btnSortUp";
            this.btnSortUp.Size = new System.Drawing.Size(32, 32);
            this.btnSortUp.TabIndex = 0;
            this.btnSortUp.Text = "위";
            this.btnSortUp.UseVisualStyleBackColor = true;
            // 
            // btnZoomPlus
            // 
            this.btnZoomPlus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnZoomPlus.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnZoomPlus.Location = new System.Drawing.Point(39, 101);
            this.btnZoomPlus.Name = "btnZoomPlus";
            this.btnZoomPlus.Size = new System.Drawing.Size(32, 32);
            this.btnZoomPlus.TabIndex = 0;
            this.btnZoomPlus.Text = "확대";
            this.btnZoomPlus.UseVisualStyleBackColor = true;
            // 
            // btnRect
            // 
            this.btnRect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRect.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRect.Location = new System.Drawing.Point(101, 3);
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(32, 32);
            this.btnRect.TabIndex = 0;
            this.btnRect.Text = "네모";
            this.btnRect.UseVisualStyleBackColor = true;
            this.btnRect.Click += new System.EventHandler(this.btnRect_Click);
            // 
            // btnSortLeft
            // 
            this.btnSortLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSortLeft.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSortLeft.Location = new System.Drawing.Point(39, 70);
            this.btnSortLeft.Name = "btnSortLeft";
            this.btnSortLeft.Size = new System.Drawing.Size(32, 32);
            this.btnSortLeft.TabIndex = 0;
            this.btnSortLeft.Text = "좌측";
            this.btnSortLeft.UseVisualStyleBackColor = true;
            // 
            // btnLine
            // 
            this.btnLine.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLine.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLine.Location = new System.Drawing.Point(70, 3);
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(32, 32);
            this.btnLine.TabIndex = 0;
            this.btnLine.Text = "선";
            this.btnLine.UseVisualStyleBackColor = true;
            this.btnLine.Click += new System.EventHandler(this.btnLine_Click);
            // 
            // btnDot
            // 
            this.btnDot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDot.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDot.Location = new System.Drawing.Point(39, 3);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(32, 32);
            this.btnDot.TabIndex = 0;
            this.btnDot.Text = "점";
            this.btnDot.UseVisualStyleBackColor = true;
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);
            // 
            // pnlObject
            // 
            this.pnlObject.AutoSize = true;
            this.pnlObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlObject.Controls.Add(this.btnNone);
            this.pnlObject.Controls.Add(this.btnDxf);
            this.pnlObject.Controls.Add(this.btnBmp);
            this.pnlObject.Controls.Add(this.label7);
            this.pnlObject.Controls.Add(this.label14);
            this.pnlObject.Controls.Add(this.label15);
            this.pnlObject.Controls.Add(this.btnZoomAll);
            this.pnlObject.Controls.Add(this.btnSortRight);
            this.pnlObject.Controls.Add(this.btnZoomMouse);
            this.pnlObject.Controls.Add(this.btnFont);
            this.pnlObject.Controls.Add(this.btnSortDn);
            this.pnlObject.Controls.Add(this.btnZoomMinus);
            this.pnlObject.Controls.Add(this.btnCircle);
            this.pnlObject.Controls.Add(this.btnSortUp);
            this.pnlObject.Controls.Add(this.btnZoomPlus);
            this.pnlObject.Controls.Add(this.btnRect);
            this.pnlObject.Controls.Add(this.btnSortLeft);
            this.pnlObject.Controls.Add(this.btnLine);
            this.pnlObject.Controls.Add(this.btnDot);
            this.pnlObject.Location = new System.Drawing.Point(6, 36);
            this.pnlObject.Name = "pnlObject";
            this.pnlObject.Size = new System.Drawing.Size(176, 138);
            this.pnlObject.TabIndex = 13;
            // 
            // btnImageSave
            // 
            this.btnImageSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnImageSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnImageSave.Image = ((System.Drawing.Image)(resources.GetObject("btnImageSave.Image")));
            this.btnImageSave.Location = new System.Drawing.Point(975, 30);
            this.btnImageSave.Name = "btnImageSave";
            this.btnImageSave.Size = new System.Drawing.Size(130, 61);
            this.btnImageSave.TabIndex = 754;
            this.btnImageSave.Text = "Image Save";
            this.btnImageSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImageSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImageSave.UseVisualStyleBackColor = true;
            this.btnImageSave.Click += new System.EventHandler(this.btnImageSave_Click);
            // 
            // FormScanWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1225, 845);
            this.ControlBox = false;
            this.Controls.Add(this.btnImageSave);
            this.Controls.Add(this.pnlObject);
            this.Controls.Add(this.tabProperty);
            this.Controls.Add(this.pnlCanvas);
            this.Controls.Add(this.panList);
            this.Controls.Add(this.statusBottom);
            this.Controls.Add(this.menuScanner);
            this.MainMenuStrip = this.menuScanner;
            this.Name = "FormScanWindow";
            this.Text = "Laser Scan System";
            this.Load += new System.EventHandler(this.FormScanWindow_Load);
            this.Resize += new System.EventHandler(this.CWindowForm_Resize);
            this.statusBottom.ResumeLayout(false);
            this.statusBottom.PerformLayout();
            this.menuScanner.ResumeLayout(false);
            this.menuScanner.PerformLayout();
            this.panList.ResumeLayout(false);
            this.tabProperty.ResumeLayout(false);
            this.pageProperty.ResumeLayout(false);
            this.pageProperty.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlObject.ResumeLayout(false);
            this.pnlObject.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusBottom;
        private System.Windows.Forms.MenuStrip menuScanner;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem penToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pen1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pen1ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pen2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pen3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pen4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pen5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 배경ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 밝음ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 어두움ToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 생성ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 비활성화ToolStripMenuItem;
        //private CWindowControl WindowControl;
        private System.Windows.Forms.Panel panList;
        private System.Windows.Forms.Button btnObjectDeleteAll;
        private System.Windows.Forms.Button btnObjectDelete;
        private System.Windows.Forms.Button btnObjectUngroup;
        private System.Windows.Forms.Button btnObjectGroup;
        private System.Windows.Forms.ListView ShapeListView;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.TabControl tabProperty;
        private System.Windows.Forms.TabPage pageProperty;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnObjectArrayCopy;
        private System.Windows.Forms.TextBox txtArrayGapY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtArrayNumY;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtArrayGapX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtArrayNumX;
        private System.Windows.Forms.Label label13;
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
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.Button btnDxf;
        private System.Windows.Forms.Button btnBmp;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnZoomAll;
        private System.Windows.Forms.Button btnSortRight;
        private System.Windows.Forms.Button btnZoomMouse;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.Button btnSortDn;
        private System.Windows.Forms.Button btnZoomMinus;
        private System.Windows.Forms.Button btnCircle;
        private System.Windows.Forms.Button btnSortUp;
        private System.Windows.Forms.Button btnZoomPlus;
        private System.Windows.Forms.Button btnRect;
        private System.Windows.Forms.Button btnSortLeft;
        private System.Windows.Forms.Button btnLine;
        private System.Windows.Forms.Button btnDot;
        private System.Windows.Forms.Panel pnlObject;
        private System.Windows.Forms.Button btnImageSave;
    }
}