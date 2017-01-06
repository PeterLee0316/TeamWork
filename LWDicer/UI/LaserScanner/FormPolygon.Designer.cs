namespace LWDicer.UI
{
    partial class FormPolygon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPolygon));
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle9 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle10 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle11 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle12 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle13 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle14 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle15 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle16 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            this.tabPolygonForm = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            this.tabPageProcess = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BtnLaserPattern = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnLaserProcess = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnConfigureExit = new System.Windows.Forms.Button();
            this.tabPageScanner = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.btnHeadReconnect = new System.Windows.Forms.Button();
            this.btnControlReconnect = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnHeadLogClear = new System.Windows.Forms.Button();
            this.btnControlLogClear = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbHeadStatus = new System.Windows.Forms.RichTextBox();
            this.rtbControllerStatus = new System.Windows.Forms.RichTextBox();
            this.tabPageVision = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.btnPreCam = new System.Windows.Forms.Button();
            this.btnInpectCam = new System.Windows.Forms.Button();
            this.btnFineCam = new System.Windows.Forms.Button();
            this.btnMeasureClear = new System.Windows.Forms.Button();
            this.btnJogShow = new System.Windows.Forms.Button();
            this.picVisionZoom = new System.Windows.Forms.Panel();
            this.lblMousePos = new System.Windows.Forms.Label();
            this.btnVisionHalt = new System.Windows.Forms.Button();
            this.btnMeasure = new System.Windows.Forms.Button();
            this.btnVisionLive = new System.Windows.Forms.Button();
            this.btnCalibration = new System.Windows.Forms.Button();
            this.btnVisionSaveZoom = new System.Windows.Forms.Button();
            this.tabPageConfig = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ComboScannerIndex = new System.Windows.Forms.ComboBox();
            this.gradientLabel4 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.GridConfigure = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.btnDataUpdate = new System.Windows.Forms.Button();
            this.btnImageUpdate = new System.Windows.Forms.Button();
            this.btnImportConfig = new System.Windows.Forms.Button();
            this.btnConfigSave = new System.Windows.Forms.Button();
            this.btnExportConfig = new System.Windows.Forms.Button();
            this.tabPageDrawing = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnObjectDxf = new System.Windows.Forms.Button();
            this.tabPageLaser = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.gridControl1 = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.LabelStageIndex = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnPatternDraw = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnLoadFrom = new System.Windows.Forms.Button();
            this.windowDxf = new LWDicer.Layers.WindowDxf();
            ((System.ComponentModel.ISupportInitialize)(this.tabPolygonForm)).BeginInit();
            this.tabPolygonForm.SuspendLayout();
            this.tabPageProcess.SuspendLayout();
            this.tabPageScanner.SuspendLayout();
            this.tabPageVision.SuspendLayout();
            this.tabPageConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridConfigure)).BeginInit();
            this.tabPageDrawing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPolygonForm
            // 
            this.tabPolygonForm.Controls.Add(this.tabPageProcess);
            this.tabPolygonForm.Controls.Add(this.tabPageScanner);
            this.tabPolygonForm.Controls.Add(this.tabPageVision);
            this.tabPolygonForm.Controls.Add(this.tabPageConfig);
            this.tabPolygonForm.Controls.Add(this.tabPageDrawing);
            this.tabPolygonForm.Controls.Add(this.tabPageLaser);
            this.tabPolygonForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPolygonForm.ImageList = this.imgList;
            this.tabPolygonForm.ItemSize = new System.Drawing.Size(100, 35);
            this.tabPolygonForm.Location = new System.Drawing.Point(0, 0);
            this.tabPolygonForm.MultilineText = true;
            this.tabPolygonForm.Name = "tabPolygonForm";
            this.tabPolygonForm.Size = new System.Drawing.Size(1260, 880);
            this.tabPolygonForm.TabIndex = 1;
            this.tabPolygonForm.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererVS2008);
            // 
            // tabPageProcess
            // 
            this.tabPageProcess.Controls.Add(this.BtnPatternDraw);
            this.tabPageProcess.Controls.Add(this.groupBox3);
            this.tabPageProcess.Controls.Add(this.BtnLaserProcess);
            this.tabPageProcess.Controls.Add(this.BtnLaserPattern);
            this.tabPageProcess.Controls.Add(this.BtnConfigureExit);
            this.tabPageProcess.Image = null;
            this.tabPageProcess.ImageSize = new System.Drawing.Size(16, 16);
            this.tabPageProcess.Location = new System.Drawing.Point(1, 42);
            this.tabPageProcess.Name = "tabPageProcess";
            this.tabPageProcess.Size = new System.Drawing.Size(1257, 836);
            this.tabPageProcess.TabIndex = 2;
            this.tabPageProcess.Text = "Laser Process";
            this.tabPageProcess.ThemesEnabled = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(709, 714);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Process Parameter";
            // 
            // BtnLaserPattern
            // 
            this.BtnLaserPattern.AutoEllipsis = true;
            this.BtnLaserPattern.BackColor = System.Drawing.SystemColors.Control;
            this.BtnLaserPattern.ButtonType = Syncfusion.Windows.Forms.Tools.ButtonTypes.Calculator;
            this.BtnLaserPattern.FlatAppearance.BorderSize = 5;
            this.BtnLaserPattern.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLaserPattern.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnLaserPattern.Location = new System.Drawing.Point(152, 760);
            this.BtnLaserPattern.Name = "BtnLaserPattern";
            this.BtnLaserPattern.Size = new System.Drawing.Size(124, 67);
            this.BtnLaserPattern.TabIndex = 780;
            this.BtnLaserPattern.Text = "Laser Pattern Data";
            this.BtnLaserPattern.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnLaserPattern.Click += new System.EventHandler(this.BtnLaserPattern_Click);
            // 
            // BtnLaserProcess
            // 
            this.BtnLaserProcess.AutoEllipsis = true;
            this.BtnLaserProcess.BackColor = System.Drawing.SystemColors.Control;
            this.BtnLaserProcess.FlatAppearance.BorderSize = 5;
            this.BtnLaserProcess.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLaserProcess.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnLaserProcess.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnLaserProcess.Location = new System.Drawing.Point(22, 759);
            this.BtnLaserProcess.Name = "BtnLaserProcess";
            this.BtnLaserProcess.Size = new System.Drawing.Size(124, 67);
            this.BtnLaserProcess.TabIndex = 779;
            this.BtnLaserProcess.Text = "Laser Process Data";
            this.BtnLaserProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnLaserProcess.Click += new System.EventHandler(this.BtnLaserProcess_Click);
            // 
            // BtnConfigureExit
            // 
            this.BtnConfigureExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnConfigureExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnConfigureExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnConfigureExit.Image")));
            this.BtnConfigureExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnConfigureExit.Location = new System.Drawing.Point(426, 759);
            this.BtnConfigureExit.Name = "BtnConfigureExit";
            this.BtnConfigureExit.Size = new System.Drawing.Size(124, 67);
            this.BtnConfigureExit.TabIndex = 751;
            this.BtnConfigureExit.Text = " Exit";
            this.BtnConfigureExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnConfigureExit.UseVisualStyleBackColor = true;
            this.BtnConfigureExit.Click += new System.EventHandler(this.BtnConfigureExit_Click);
            // 
            // tabPageScanner
            // 
            this.tabPageScanner.Controls.Add(this.btnHeadReconnect);
            this.tabPageScanner.Controls.Add(this.btnControlReconnect);
            this.tabPageScanner.Controls.Add(this.textBox1);
            this.tabPageScanner.Controls.Add(this.btnHeadLogClear);
            this.tabPageScanner.Controls.Add(this.btnControlLogClear);
            this.tabPageScanner.Controls.Add(this.label5);
            this.tabPageScanner.Controls.Add(this.label1);
            this.tabPageScanner.Controls.Add(this.rtbHeadStatus);
            this.tabPageScanner.Controls.Add(this.rtbControllerStatus);
            this.tabPageScanner.Image = null;
            this.tabPageScanner.ImageSize = new System.Drawing.Size(16, 16);
            this.tabPageScanner.Location = new System.Drawing.Point(1, 42);
            this.tabPageScanner.Name = "tabPageScanner";
            this.tabPageScanner.Size = new System.Drawing.Size(1257, 836);
            this.tabPageScanner.TabIndex = 1;
            this.tabPageScanner.Text = "Log";
            this.tabPageScanner.ThemesEnabled = false;
            // 
            // btnHeadReconnect
            // 
            this.btnHeadReconnect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnHeadReconnect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnHeadReconnect.Location = new System.Drawing.Point(887, 3);
            this.btnHeadReconnect.Name = "btnHeadReconnect";
            this.btnHeadReconnect.Size = new System.Drawing.Size(81, 24);
            this.btnHeadReconnect.TabIndex = 761;
            this.btnHeadReconnect.Text = "ReConnet";
            this.btnHeadReconnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHeadReconnect.UseVisualStyleBackColor = true;
            // 
            // btnControlReconnect
            // 
            this.btnControlReconnect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnControlReconnect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnControlReconnect.Location = new System.Drawing.Point(367, 3);
            this.btnControlReconnect.Name = "btnControlReconnect";
            this.btnControlReconnect.Size = new System.Drawing.Size(81, 24);
            this.btnControlReconnect.TabIndex = 760;
            this.btnControlReconnect.Text = "ReConnet";
            this.btnControlReconnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnControlReconnect.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(5, 622);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(512, 21);
            this.textBox1.TabIndex = 759;
            // 
            // btnHeadLogClear
            // 
            this.btnHeadLogClear.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnHeadLogClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnHeadLogClear.Location = new System.Drawing.Point(800, 3);
            this.btnHeadLogClear.Name = "btnHeadLogClear";
            this.btnHeadLogClear.Size = new System.Drawing.Size(81, 24);
            this.btnHeadLogClear.TabIndex = 758;
            this.btnHeadLogClear.Text = "Log Clear";
            this.btnHeadLogClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHeadLogClear.UseVisualStyleBackColor = true;
            this.btnHeadLogClear.Click += new System.EventHandler(this.btnHeadLogClear_Click);
            // 
            // btnControlLogClear
            // 
            this.btnControlLogClear.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnControlLogClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnControlLogClear.Location = new System.Drawing.Point(280, 3);
            this.btnControlLogClear.Name = "btnControlLogClear";
            this.btnControlLogClear.Size = new System.Drawing.Size(81, 24);
            this.btnControlLogClear.TabIndex = 757;
            this.btnControlLogClear.Text = "Log Clear";
            this.btnControlLogClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnControlLogClear.UseVisualStyleBackColor = true;
            this.btnControlLogClear.Click += new System.EventHandler(this.btnControlLogClear_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(523, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(271, 24);
            this.label5.TabIndex = 42;
            this.label5.Text = "Scan Head Status";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 24);
            this.label1.TabIndex = 41;
            this.label1.Text = "LSE Controller Status";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rtbHeadStatus
            // 
            this.rtbHeadStatus.BackColor = System.Drawing.Color.White;
            this.rtbHeadStatus.Location = new System.Drawing.Point(523, 29);
            this.rtbHeadStatus.Name = "rtbHeadStatus";
            this.rtbHeadStatus.ReadOnly = true;
            this.rtbHeadStatus.Size = new System.Drawing.Size(512, 587);
            this.rtbHeadStatus.TabIndex = 39;
            this.rtbHeadStatus.Text = "";
            // 
            // rtbControllerStatus
            // 
            this.rtbControllerStatus.BackColor = System.Drawing.Color.White;
            this.rtbControllerStatus.Location = new System.Drawing.Point(5, 29);
            this.rtbControllerStatus.Name = "rtbControllerStatus";
            this.rtbControllerStatus.ReadOnly = true;
            this.rtbControllerStatus.Size = new System.Drawing.Size(512, 587);
            this.rtbControllerStatus.TabIndex = 0;
            this.rtbControllerStatus.Text = "";
            // 
            // tabPageVision
            // 
            this.tabPageVision.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPageVision.Controls.Add(this.btnPreCam);
            this.tabPageVision.Controls.Add(this.btnInpectCam);
            this.tabPageVision.Controls.Add(this.btnFineCam);
            this.tabPageVision.Controls.Add(this.btnMeasureClear);
            this.tabPageVision.Controls.Add(this.btnJogShow);
            this.tabPageVision.Controls.Add(this.picVisionZoom);
            this.tabPageVision.Controls.Add(this.lblMousePos);
            this.tabPageVision.Controls.Add(this.btnVisionHalt);
            this.tabPageVision.Controls.Add(this.btnMeasure);
            this.tabPageVision.Controls.Add(this.btnVisionLive);
            this.tabPageVision.Controls.Add(this.btnCalibration);
            this.tabPageVision.Controls.Add(this.btnVisionSaveZoom);
            this.tabPageVision.Image = null;
            this.tabPageVision.ImageIndex = 0;
            this.tabPageVision.ImageSize = new System.Drawing.Size(16, 16);
            this.tabPageVision.Location = new System.Drawing.Point(1, 42);
            this.tabPageVision.Name = "tabPageVision";
            this.tabPageVision.Size = new System.Drawing.Size(1257, 836);
            this.tabPageVision.TabIndex = 8;
            this.tabPageVision.Text = "Vision";
            this.tabPageVision.ThemesEnabled = false;
            // 
            // btnPreCam
            // 
            this.btnPreCam.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPreCam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnPreCam.Image = global::LWDicer.Properties.Resources.Vision1;
            this.btnPreCam.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPreCam.Location = new System.Drawing.Point(1073, 296);
            this.btnPreCam.Name = "btnPreCam";
            this.btnPreCam.Size = new System.Drawing.Size(84, 68);
            this.btnPreCam.TabIndex = 764;
            this.btnPreCam.Text = "Pre";
            this.btnPreCam.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPreCam.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPreCam.UseVisualStyleBackColor = true;
            this.btnPreCam.Click += new System.EventHandler(this.btnPreCam_Click);
            // 
            // btnInpectCam
            // 
            this.btnInpectCam.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInpectCam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnInpectCam.Image = global::LWDicer.Properties.Resources.Vision;
            this.btnInpectCam.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnInpectCam.Location = new System.Drawing.Point(1073, 370);
            this.btnInpectCam.Name = "btnInpectCam";
            this.btnInpectCam.Size = new System.Drawing.Size(84, 68);
            this.btnInpectCam.TabIndex = 763;
            this.btnInpectCam.Text = "Inspect";
            this.btnInpectCam.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnInpectCam.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnInpectCam.UseVisualStyleBackColor = true;
            this.btnInpectCam.Click += new System.EventHandler(this.btnInpectCam_Click);
            // 
            // btnFineCam
            // 
            this.btnFineCam.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFineCam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnFineCam.Image = global::LWDicer.Properties.Resources.Vision1;
            this.btnFineCam.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFineCam.Location = new System.Drawing.Point(1163, 296);
            this.btnFineCam.Name = "btnFineCam";
            this.btnFineCam.Size = new System.Drawing.Size(84, 68);
            this.btnFineCam.TabIndex = 762;
            this.btnFineCam.Text = "Fine";
            this.btnFineCam.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFineCam.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnFineCam.UseVisualStyleBackColor = true;
            this.btnFineCam.Click += new System.EventHandler(this.btnFineCam_Click);
            // 
            // btnMeasureClear
            // 
            this.btnMeasureClear.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMeasureClear.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMeasureClear.Image = ((System.Drawing.Image)(resources.GetObject("btnMeasureClear.Image")));
            this.btnMeasureClear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMeasureClear.Location = new System.Drawing.Point(1163, 74);
            this.btnMeasureClear.Name = "btnMeasureClear";
            this.btnMeasureClear.Size = new System.Drawing.Size(84, 68);
            this.btnMeasureClear.TabIndex = 761;
            this.btnMeasureClear.Text = "Clear";
            this.btnMeasureClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMeasureClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMeasureClear.UseVisualStyleBackColor = true;
            this.btnMeasureClear.Click += new System.EventHandler(this.btnMeasureClear_Click);
            // 
            // btnJogShow
            // 
            this.btnJogShow.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogShow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnJogShow.Image = global::LWDicer.Properties.Resources.ManualRun;
            this.btnJogShow.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnJogShow.Location = new System.Drawing.Point(1073, 222);
            this.btnJogShow.Name = "btnJogShow";
            this.btnJogShow.Size = new System.Drawing.Size(84, 68);
            this.btnJogShow.TabIndex = 760;
            this.btnJogShow.Text = "Motion Jog";
            this.btnJogShow.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnJogShow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnJogShow.UseVisualStyleBackColor = true;
            this.btnJogShow.Click += new System.EventHandler(this.btnJogShow_Click);
            // 
            // picVisionZoom
            // 
            this.picVisionZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picVisionZoom.Location = new System.Drawing.Point(3, 3);
            this.picVisionZoom.Name = "picVisionZoom";
            this.picVisionZoom.Size = new System.Drawing.Size(1011, 756);
            this.picVisionZoom.TabIndex = 3;
            this.picVisionZoom.Paint += new System.Windows.Forms.PaintEventHandler(this.picVisionZoom_Paint);
            this.picVisionZoom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picVisionZoom_MouseDown);
            this.picVisionZoom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picVisionZoom_MouseMove);
            this.picVisionZoom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picVisionZoom_MouseUp);
            // 
            // lblMousePos
            // 
            this.lblMousePos.AutoSize = true;
            this.lblMousePos.Location = new System.Drawing.Point(1082, 731);
            this.lblMousePos.Name = "lblMousePos";
            this.lblMousePos.Size = new System.Drawing.Size(23, 12);
            this.lblMousePos.TabIndex = 757;
            this.lblMousePos.Text = "---";
            // 
            // btnVisionHalt
            // 
            this.btnVisionHalt.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnVisionHalt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnVisionHalt.Image = global::LWDicer.Properties.Resources.stop;
            this.btnVisionHalt.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnVisionHalt.Location = new System.Drawing.Point(1163, 148);
            this.btnVisionHalt.Name = "btnVisionHalt";
            this.btnVisionHalt.Size = new System.Drawing.Size(84, 68);
            this.btnVisionHalt.TabIndex = 757;
            this.btnVisionHalt.Text = "Halt";
            this.btnVisionHalt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVisionHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnVisionHalt.UseVisualStyleBackColor = true;
            this.btnVisionHalt.Click += new System.EventHandler(this.btnVisionHalt_Click);
            // 
            // btnMeasure
            // 
            this.btnMeasure.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMeasure.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMeasure.Image = ((System.Drawing.Image)(resources.GetObject("btnMeasure.Image")));
            this.btnMeasure.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMeasure.Location = new System.Drawing.Point(1073, 74);
            this.btnMeasure.Name = "btnMeasure";
            this.btnMeasure.Size = new System.Drawing.Size(84, 68);
            this.btnMeasure.TabIndex = 759;
            this.btnMeasure.Text = "measure";
            this.btnMeasure.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMeasure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMeasure.UseVisualStyleBackColor = true;
            this.btnMeasure.Click += new System.EventHandler(this.btnMeasure_Click);
            // 
            // btnVisionLive
            // 
            this.btnVisionLive.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnVisionLive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnVisionLive.Image = global::LWDicer.Properties.Resources.run;
            this.btnVisionLive.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnVisionLive.Location = new System.Drawing.Point(1073, 148);
            this.btnVisionLive.Name = "btnVisionLive";
            this.btnVisionLive.Size = new System.Drawing.Size(84, 68);
            this.btnVisionLive.TabIndex = 756;
            this.btnVisionLive.Text = "Live";
            this.btnVisionLive.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVisionLive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnVisionLive.UseVisualStyleBackColor = true;
            this.btnVisionLive.Click += new System.EventHandler(this.btnVisionLive_Click);
            // 
            // btnCalibration
            // 
            this.btnCalibration.BackColor = System.Drawing.SystemColors.Control;
            this.btnCalibration.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCalibration.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCalibration.Image = ((System.Drawing.Image)(resources.GetObject("btnCalibration.Image")));
            this.btnCalibration.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCalibration.Location = new System.Drawing.Point(1073, 0);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Size = new System.Drawing.Size(84, 68);
            this.btnCalibration.TabIndex = 758;
            this.btnCalibration.Text = "Calibration";
            this.btnCalibration.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCalibration.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCalibration.UseVisualStyleBackColor = false;
            this.btnCalibration.Click += new System.EventHandler(this.btnCalibration_Click);
            // 
            // btnVisionSaveZoom
            // 
            this.btnVisionSaveZoom.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnVisionSaveZoom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnVisionSaveZoom.Image = ((System.Drawing.Image)(resources.GetObject("btnVisionSaveZoom.Image")));
            this.btnVisionSaveZoom.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnVisionSaveZoom.Location = new System.Drawing.Point(1163, 0);
            this.btnVisionSaveZoom.Name = "btnVisionSaveZoom";
            this.btnVisionSaveZoom.Size = new System.Drawing.Size(84, 68);
            this.btnVisionSaveZoom.TabIndex = 755;
            this.btnVisionSaveZoom.Text = "Image Save";
            this.btnVisionSaveZoom.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVisionSaveZoom.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnVisionSaveZoom.UseVisualStyleBackColor = true;
            this.btnVisionSaveZoom.Click += new System.EventHandler(this.btnVisionSaveZoom_Click);
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Controls.Add(this.BtnLoadFrom);
            this.tabPageConfig.Controls.Add(this.ComboScannerIndex);
            this.tabPageConfig.Controls.Add(this.gradientLabel4);
            this.tabPageConfig.Controls.Add(this.btnDataUpdate);
            this.tabPageConfig.Controls.Add(this.btnImageUpdate);
            this.tabPageConfig.Controls.Add(this.btnImportConfig);
            this.tabPageConfig.Controls.Add(this.btnConfigSave);
            this.tabPageConfig.Controls.Add(this.btnExportConfig);
            this.tabPageConfig.Controls.Add(this.GridConfigure);
            this.tabPageConfig.Image = null;
            this.tabPageConfig.ImageSize = new System.Drawing.Size(16, 16);
            this.tabPageConfig.Location = new System.Drawing.Point(1, 42);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Size = new System.Drawing.Size(1257, 836);
            this.tabPageConfig.TabIndex = 3;
            this.tabPageConfig.Text = "Scanner";
            this.tabPageConfig.ThemesEnabled = false;
            this.tabPageConfig.Enter += new System.EventHandler(this.tabPageConfig_Enter);
            // 
            // ComboScannerIndex
            // 
            this.ComboScannerIndex.DropDownHeight = 200;
            this.ComboScannerIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboScannerIndex.DropDownWidth = 260;
            this.ComboScannerIndex.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ComboScannerIndex.FormattingEnabled = true;
            this.ComboScannerIndex.IntegralHeight = false;
            this.ComboScannerIndex.Location = new System.Drawing.Point(887, 51);
            this.ComboScannerIndex.Name = "ComboScannerIndex";
            this.ComboScannerIndex.Size = new System.Drawing.Size(185, 27);
            this.ComboScannerIndex.TabIndex = 1050;
            this.ComboScannerIndex.SelectedIndexChanged += new System.EventHandler(this.ComboScannerIndex_SelectedIndexChanged);
            // 
            // gradientLabel4
            // 
            this.gradientLabel4.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64))))));
            this.gradientLabel4.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientLabel4.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel4.ForeColor = System.Drawing.Color.White;
            this.gradientLabel4.Location = new System.Drawing.Point(887, 12);
            this.gradientLabel4.Name = "gradientLabel4";
            this.gradientLabel4.Size = new System.Drawing.Size(185, 36);
            this.gradientLabel4.TabIndex = 1049;
            this.gradientLabel4.Text = "Scanner Index";
            this.gradientLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gradientLabel4.Click += new System.EventHandler(this.gradientLabel4_Click);
            // 
            // GridConfigure
            // 
            gridBaseStyle9.Name = "Header";
            gridBaseStyle9.StyleInfo.Borders.Bottom = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle9.StyleInfo.Borders.Left = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle9.StyleInfo.Borders.Right = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle9.StyleInfo.Borders.Top = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle9.StyleInfo.CellType = "Header";
            gridBaseStyle9.StyleInfo.Font.Bold = true;
            gridBaseStyle9.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Vertical, System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(199)))), ((int)(((byte)(184))))), System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(216))))));
            gridBaseStyle9.StyleInfo.VerticalAlignment = Syncfusion.Windows.Forms.Grid.GridVerticalAlignment.Middle;
            gridBaseStyle10.Name = "Standard";
            gridBaseStyle10.StyleInfo.Font.Facename = "Tahoma";
            gridBaseStyle10.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(System.Drawing.SystemColors.Window);
            gridBaseStyle11.Name = "Column Header";
            gridBaseStyle11.StyleInfo.BaseStyle = "Header";
            gridBaseStyle11.StyleInfo.HorizontalAlignment = Syncfusion.Windows.Forms.Grid.GridHorizontalAlignment.Center;
            gridBaseStyle12.Name = "Row Header";
            gridBaseStyle12.StyleInfo.BaseStyle = "Header";
            gridBaseStyle12.StyleInfo.HorizontalAlignment = Syncfusion.Windows.Forms.Grid.GridHorizontalAlignment.Left;
            gridBaseStyle12.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(199)))), ((int)(((byte)(184))))), System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(216))))));
            this.GridConfigure.BaseStylesMap.AddRange(new Syncfusion.Windows.Forms.Grid.GridBaseStyle[] {
            gridBaseStyle9,
            gridBaseStyle10,
            gridBaseStyle11,
            gridBaseStyle12});
            this.GridConfigure.ColWidthEntries.AddRange(new Syncfusion.Windows.Forms.Grid.GridColWidth[] {
            new Syncfusion.Windows.Forms.Grid.GridColWidth(0, 35)});
            this.GridConfigure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridConfigure.Location = new System.Drawing.Point(0, 0);
            this.GridConfigure.Name = "GridConfigure";
            this.GridConfigure.RowHeightEntries.AddRange(new Syncfusion.Windows.Forms.Grid.GridRowHeight[] {
            new Syncfusion.Windows.Forms.Grid.GridRowHeight(0, 21)});
            this.GridConfigure.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridConfigure.Size = new System.Drawing.Size(1257, 836);
            this.GridConfigure.SmartSizeBox = false;
            this.GridConfigure.TabIndex = 2;
            this.GridConfigure.Text = "gridControl";
            this.GridConfigure.UseRightToLeftCompatibleTextBox = true;
            this.GridConfigure.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridConfigure_CellClick);
            // 
            // btnDataUpdate
            // 
            this.btnDataUpdate.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDataUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnDataUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnDataUpdate.Image")));
            this.btnDataUpdate.Location = new System.Drawing.Point(887, 466);
            this.btnDataUpdate.Name = "btnDataUpdate";
            this.btnDataUpdate.Size = new System.Drawing.Size(185, 61);
            this.btnDataUpdate.TabIndex = 754;
            this.btnDataUpdate.Text = "Data to Scanner";
            this.btnDataUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDataUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDataUpdate.UseVisualStyleBackColor = true;
            this.btnDataUpdate.Click += new System.EventHandler(this.btnDataUpdate_Click);
            // 
            // btnImageUpdate
            // 
            this.btnImageUpdate.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnImageUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnImageUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnImageUpdate.Image")));
            this.btnImageUpdate.Location = new System.Drawing.Point(887, 399);
            this.btnImageUpdate.Name = "btnImageUpdate";
            this.btnImageUpdate.Size = new System.Drawing.Size(185, 61);
            this.btnImageUpdate.TabIndex = 755;
            this.btnImageUpdate.Text = "Image to Scanner";
            this.btnImageUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImageUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImageUpdate.UseVisualStyleBackColor = true;
            this.btnImageUpdate.Click += new System.EventHandler(this.btnImageUpdate_Click);
            // 
            // btnImportConfig
            // 
            this.btnImportConfig.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnImportConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnImportConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnImportConfig.Image")));
            this.btnImportConfig.Location = new System.Drawing.Point(887, 84);
            this.btnImportConfig.Name = "btnImportConfig";
            this.btnImportConfig.Size = new System.Drawing.Size(185, 61);
            this.btnImportConfig.TabIndex = 758;
            this.btnImportConfig.Text = "Import Config.ini";
            this.btnImportConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportConfig.UseVisualStyleBackColor = true;
            this.btnImportConfig.Click += new System.EventHandler(this.btnImportConfig_Click);
            // 
            // btnConfigSave
            // 
            this.btnConfigSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConfigSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnConfigSave.Image = ((System.Drawing.Image)(resources.GetObject("btnConfigSave.Image")));
            this.btnConfigSave.Location = new System.Drawing.Point(887, 218);
            this.btnConfigSave.Name = "btnConfigSave";
            this.btnConfigSave.Size = new System.Drawing.Size(185, 61);
            this.btnConfigSave.TabIndex = 752;
            this.btnConfigSave.Text = "Data Save";
            this.btnConfigSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfigSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConfigSave.UseVisualStyleBackColor = true;
            this.btnConfigSave.Click += new System.EventHandler(this.BtnConfigSave_Click);
            // 
            // btnExportConfig
            // 
            this.btnExportConfig.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExportConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnExportConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnExportConfig.Image")));
            this.btnExportConfig.Location = new System.Drawing.Point(887, 151);
            this.btnExportConfig.Name = "btnExportConfig";
            this.btnExportConfig.Size = new System.Drawing.Size(185, 61);
            this.btnExportConfig.TabIndex = 757;
            this.btnExportConfig.Text = "Export Config.ini";
            this.btnExportConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExportConfig.UseVisualStyleBackColor = true;
            this.btnExportConfig.Click += new System.EventHandler(this.btnExportConfig_Click);
            // 
            // tabPageDrawing
            // 
            this.tabPageDrawing.Controls.Add(this.splitContainer1);
            this.tabPageDrawing.Image = null;
            this.tabPageDrawing.ImageSize = new System.Drawing.Size(16, 16);
            this.tabPageDrawing.Location = new System.Drawing.Point(1, 42);
            this.tabPageDrawing.Name = "tabPageDrawing";
            this.tabPageDrawing.Size = new System.Drawing.Size(1257, 836);
            this.tabPageDrawing.TabIndex = 9;
            this.tabPageDrawing.Text = "Drawing";
            this.tabPageDrawing.ThemesEnabled = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.windowDxf);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnObjectDxf);
            this.splitContainer1.Size = new System.Drawing.Size(1257, 836);
            this.splitContainer1.SplitterDistance = 934;
            this.splitContainer1.TabIndex = 12;
            // 
            // btnObjectDxf
            // 
            this.btnObjectDxf.Location = new System.Drawing.Point(7, 3);
            this.btnObjectDxf.Name = "btnObjectDxf";
            this.btnObjectDxf.Size = new System.Drawing.Size(60, 43);
            this.btnObjectDxf.TabIndex = 0;
            this.btnObjectDxf.Text = "Dxf";
            this.btnObjectDxf.UseVisualStyleBackColor = true;
            this.btnObjectDxf.Click += new System.EventHandler(this.btnObjectDxf_Click);
            // 
            // tabPageLaser
            // 
            this.tabPageLaser.Image = null;
            this.tabPageLaser.ImageSize = new System.Drawing.Size(16, 16);
            this.tabPageLaser.Location = new System.Drawing.Point(1, 42);
            this.tabPageLaser.Name = "tabPageLaser";
            this.tabPageLaser.Size = new System.Drawing.Size(1257, 836);
            this.tabPageLaser.TabIndex = 6;
            this.tabPageLaser.Text = "Laser";
            this.tabPageLaser.ThemesEnabled = false;
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "Vision.bmp");
            // 
            // gridControl1
            // 
            gridBaseStyle13.Name = "Header";
            gridBaseStyle13.StyleInfo.Borders.Bottom = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle13.StyleInfo.Borders.Left = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle13.StyleInfo.Borders.Right = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle13.StyleInfo.Borders.Top = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle13.StyleInfo.CellType = "Header";
            gridBaseStyle13.StyleInfo.Font.Bold = true;
            gridBaseStyle13.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Vertical, System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(199)))), ((int)(((byte)(184))))), System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(216))))));
            gridBaseStyle13.StyleInfo.VerticalAlignment = Syncfusion.Windows.Forms.Grid.GridVerticalAlignment.Middle;
            gridBaseStyle14.Name = "Standard";
            gridBaseStyle14.StyleInfo.Font.Facename = "Tahoma";
            gridBaseStyle14.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(System.Drawing.SystemColors.Window);
            gridBaseStyle15.Name = "Column Header";
            gridBaseStyle15.StyleInfo.BaseStyle = "Header";
            gridBaseStyle15.StyleInfo.HorizontalAlignment = Syncfusion.Windows.Forms.Grid.GridHorizontalAlignment.Center;
            gridBaseStyle16.Name = "Row Header";
            gridBaseStyle16.StyleInfo.BaseStyle = "Header";
            gridBaseStyle16.StyleInfo.HorizontalAlignment = Syncfusion.Windows.Forms.Grid.GridHorizontalAlignment.Left;
            gridBaseStyle16.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(199)))), ((int)(((byte)(184))))), System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(216))))));
            this.gridControl1.BaseStylesMap.AddRange(new Syncfusion.Windows.Forms.Grid.GridBaseStyle[] {
            gridBaseStyle13,
            gridBaseStyle14,
            gridBaseStyle15,
            gridBaseStyle16});
            this.gridControl1.ColWidthEntries.AddRange(new Syncfusion.Windows.Forms.Grid.GridColWidth[] {
            new Syncfusion.Windows.Forms.Grid.GridColWidth(0, 35)});
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RowHeightEntries.AddRange(new Syncfusion.Windows.Forms.Grid.GridRowHeight[] {
            new Syncfusion.Windows.Forms.Grid.GridRowHeight(0, 21)});
            this.gridControl1.Size = new System.Drawing.Size(130, 80);
            this.gridControl1.SmartSizeBox = false;
            // 
            // LabelStageIndex
            // 
            this.LabelStageIndex.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64))))));
            this.LabelStageIndex.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelStageIndex.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.LabelStageIndex.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelStageIndex.ForeColor = System.Drawing.Color.White;
            this.LabelStageIndex.Location = new System.Drawing.Point(451, 98);
            this.LabelStageIndex.Name = "LabelStageIndex";
            this.LabelStageIndex.Size = new System.Drawing.Size(185, 36);
            this.LabelStageIndex.TabIndex = 1047;
            this.LabelStageIndex.Text = "Stage Index";
            this.LabelStageIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnSave
            // 
            this.BtnSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.Location = new System.Drawing.Point(451, 611);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(185, 61);
            this.BtnSave.TabIndex = 1041;
            this.BtnSave.Text = " Save";
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSave.UseVisualStyleBackColor = true;
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(451, 678);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(185, 61);
            this.BtnExit.TabIndex = 1040;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            // 
            // BtnPatternDraw
            // 
            this.BtnPatternDraw.AutoEllipsis = true;
            this.BtnPatternDraw.BackColor = System.Drawing.SystemColors.Control;
            this.BtnPatternDraw.ButtonType = Syncfusion.Windows.Forms.Tools.ButtonTypes.Calculator;
            this.BtnPatternDraw.FlatAppearance.BorderSize = 5;
            this.BtnPatternDraw.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPatternDraw.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnPatternDraw.Location = new System.Drawing.Point(282, 759);
            this.BtnPatternDraw.Name = "BtnPatternDraw";
            this.BtnPatternDraw.Size = new System.Drawing.Size(124, 67);
            this.BtnPatternDraw.TabIndex = 781;
            this.BtnPatternDraw.Text = "Laser Pattern Draw";
            this.BtnPatternDraw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnPatternDraw.Click += new System.EventHandler(this.BtnPatternDraw_Click);
            // 
            // BtnLoadFrom
            // 
            this.BtnLoadFrom.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLoadFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnLoadFrom.Image = ((System.Drawing.Image)(resources.GetObject("BtnLoadFrom.Image")));
            this.BtnLoadFrom.Location = new System.Drawing.Point(887, 285);
            this.BtnLoadFrom.Name = "BtnLoadFrom";
            this.BtnLoadFrom.Size = new System.Drawing.Size(185, 61);
            this.BtnLoadFrom.TabIndex = 1051;
            this.BtnLoadFrom.Text = "load from other data";
            this.BtnLoadFrom.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnLoadFrom.UseVisualStyleBackColor = true;
            // 
            // windowDxf
            // 
            this.windowDxf.BackColor = System.Drawing.Color.Black;
            this.windowDxf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowDxf.Location = new System.Drawing.Point(0, 0);
            this.windowDxf.Model = null;
            this.windowDxf.Name = "windowDxf";
            this.windowDxf.Size = new System.Drawing.Size(934, 836);
            this.windowDxf.TabIndex = 0;
            // 
            // FormPolygon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 880);
            this.ControlBox = false;
            this.Controls.Add(this.tabPolygonForm);
            this.Name = "FormPolygon";
            this.Text = "CWindowPolygonForm";
            this.Activated += new System.EventHandler(this.FormPolygon_Activated);
            this.Load += new System.EventHandler(this.FormPolygon_Load);
            this.Shown += new System.EventHandler(this.FormPolygon_Shown);
            this.SizeChanged += new System.EventHandler(this.Form_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.tabPolygonForm)).EndInit();
            this.tabPolygonForm.ResumeLayout(false);
            this.tabPageProcess.ResumeLayout(false);
            this.tabPageScanner.ResumeLayout(false);
            this.tabPageScanner.PerformLayout();
            this.tabPageVision.ResumeLayout(false);
            this.tabPageVision.PerformLayout();
            this.tabPageConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridConfigure)).EndInit();
            this.tabPageDrawing.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Syncfusion.Windows.Forms.Tools.TabControlAdv tabPolygonForm;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageScanner;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageProcess;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageConfig;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageLaser;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageVision;
        private Syncfusion.Windows.Forms.Grid.GridControl GridConfigure;
        public System.Windows.Forms.RichTextBox rtbControllerStatus;
        private System.Windows.Forms.Button btnConfigSave;
        private System.Windows.Forms.Button BtnConfigureExit;
        public System.Windows.Forms.RichTextBox rtbHeadStatus;
        private Syncfusion.Windows.Forms.Grid.GridControl gridControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImageUpdate;
        private System.Windows.Forms.Button btnDataUpdate;
        private System.Windows.Forms.Button btnHeadLogClear;
        private System.Windows.Forms.Button btnControlLogClear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnHeadReconnect;
        private System.Windows.Forms.Button btnControlReconnect;
        private System.Windows.Forms.ImageList imgList;
        public System.Windows.Forms.Panel picVisionZoom;
        private System.Windows.Forms.Button btnVisionSaveZoom;
        private System.Windows.Forms.Button btnVisionHalt;
        private System.Windows.Forms.Button btnVisionLive;
        private System.Windows.Forms.Label lblMousePos;
        private System.Windows.Forms.Button btnMeasure;
        private System.Windows.Forms.Button btnCalibration;
        private System.Windows.Forms.Button btnJogShow;
        private System.Windows.Forms.Button btnMeasureClear;
        private System.Windows.Forms.Button btnInpectCam;
        private System.Windows.Forms.Button btnFineCam;
        private System.Windows.Forms.Button btnImportConfig;
        private System.Windows.Forms.Button btnExportConfig;
        private System.Windows.Forms.Button btnPreCam;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageDrawing;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Layers.WindowDxf windowDxf;
        private System.Windows.Forms.Button btnObjectDxf;
        private System.Windows.Forms.Button BtnSave;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelStageIndex;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.ComboBox ComboScannerIndex;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel4;
        private Syncfusion.Windows.Forms.ButtonAdv BtnLaserProcess;
        private Syncfusion.Windows.Forms.ButtonAdv BtnLaserPattern;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPatternDraw;
        private System.Windows.Forms.Button BtnLoadFrom;
    }
}