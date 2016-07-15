namespace LWDicer.UI
{
    partial class FormCoaterData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCoaterData));
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle1 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle2 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.GridCtrl = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.LabelPVAQty = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelMovingPVAQty = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelCoatingRate = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelCenterWaitTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelMovingSpeed = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelCoatingArea = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.autoLabel4 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel2 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel3 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel6 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel7 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.ComboMode = new System.Windows.Forms.ComboBox();
            this.autoLabel5 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel8 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel9 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel10 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel11 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel12 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel13 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.autoLabel14 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientLabel17 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnSave
            // 
            this.BtnSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.Location = new System.Drawing.Point(97, 595);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(124, 61);
            this.BtnSave.TabIndex = 754;
            this.BtnSave.Text = " Save";
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(227, 595);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 753;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // GridCtrl
            // 
            this.GridCtrl.ActivateCurrentCellBehavior = Syncfusion.Windows.Forms.Grid.GridCellActivateAction.None;
            this.GridCtrl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GridCtrl.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            this.GridCtrl.Location = new System.Drawing.Point(459, 56);
            this.GridCtrl.Name = "GridCtrl";
            gridRangeStyle1.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle1.StyleInfo.Font.Bold = true;
            gridRangeStyle1.StyleInfo.Font.Facename = "맑은 고딕";
            gridRangeStyle1.StyleInfo.Font.Italic = false;
            gridRangeStyle1.StyleInfo.Font.Size = 11.25F;
            gridRangeStyle1.StyleInfo.Font.Strikeout = false;
            gridRangeStyle1.StyleInfo.Font.Underline = false;
            gridRangeStyle1.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle2.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle2.StyleInfo.Font.Bold = true;
            gridRangeStyle2.StyleInfo.Font.Facename = "맑은 고딕";
            gridRangeStyle2.StyleInfo.Font.Italic = false;
            gridRangeStyle2.StyleInfo.Font.Size = 11.25F;
            gridRangeStyle2.StyleInfo.Font.Strikeout = false;
            gridRangeStyle2.StyleInfo.Font.Underline = false;
            gridRangeStyle2.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            this.GridCtrl.RangeStyles.AddRange(new Syncfusion.Windows.Forms.Grid.GridRangeStyle[] {
            gridRangeStyle1,
            gridRangeStyle2});
            this.GridCtrl.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridCtrl.Size = new System.Drawing.Size(476, 600);
            this.GridCtrl.SmartSizeBox = false;
            this.GridCtrl.TabIndex = 756;
            this.GridCtrl.UseRightToLeftCompatibleTextBox = true;
            this.GridCtrl.CurrentCellShowedDropDown += new System.EventHandler(this.GridSpinner_CurrentCellShowedDropDown);
            this.GridCtrl.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridSpinner_CellClick);
            // 
            // LabelPVAQty
            // 
            this.LabelPVAQty.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelPVAQty.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelPVAQty.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelPVAQty.ForeColor = System.Drawing.Color.Black;
            this.LabelPVAQty.Location = new System.Drawing.Point(198, 62);
            this.LabelPVAQty.Name = "LabelPVAQty";
            this.LabelPVAQty.Size = new System.Drawing.Size(136, 41);
            this.LabelPVAQty.TabIndex = 757;
            this.LabelPVAQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelPVAQty.Click += new System.EventHandler(this.LabelCoatData_Click);
            // 
            // LabelMovingPVAQty
            // 
            this.LabelMovingPVAQty.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelMovingPVAQty.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelMovingPVAQty.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelMovingPVAQty.ForeColor = System.Drawing.Color.Black;
            this.LabelMovingPVAQty.Location = new System.Drawing.Point(198, 109);
            this.LabelMovingPVAQty.Name = "LabelMovingPVAQty";
            this.LabelMovingPVAQty.Size = new System.Drawing.Size(136, 41);
            this.LabelMovingPVAQty.TabIndex = 758;
            this.LabelMovingPVAQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelMovingPVAQty.Click += new System.EventHandler(this.LabelCoatData_Click);
            // 
            // LabelCoatingRate
            // 
            this.LabelCoatingRate.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelCoatingRate.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelCoatingRate.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelCoatingRate.ForeColor = System.Drawing.Color.Black;
            this.LabelCoatingRate.Location = new System.Drawing.Point(198, 156);
            this.LabelCoatingRate.Name = "LabelCoatingRate";
            this.LabelCoatingRate.Size = new System.Drawing.Size(136, 41);
            this.LabelCoatingRate.TabIndex = 759;
            this.LabelCoatingRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelCoatingRate.Click += new System.EventHandler(this.LabelCoatData_Click);
            // 
            // LabelCenterWaitTime
            // 
            this.LabelCenterWaitTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelCenterWaitTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelCenterWaitTime.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelCenterWaitTime.ForeColor = System.Drawing.Color.Black;
            this.LabelCenterWaitTime.Location = new System.Drawing.Point(198, 203);
            this.LabelCenterWaitTime.Name = "LabelCenterWaitTime";
            this.LabelCenterWaitTime.Size = new System.Drawing.Size(136, 41);
            this.LabelCenterWaitTime.TabIndex = 760;
            this.LabelCenterWaitTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelCenterWaitTime.Click += new System.EventHandler(this.LabelCoatData_Click);
            // 
            // LabelMovingSpeed
            // 
            this.LabelMovingSpeed.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelMovingSpeed.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelMovingSpeed.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelMovingSpeed.ForeColor = System.Drawing.Color.Black;
            this.LabelMovingSpeed.Location = new System.Drawing.Point(198, 289);
            this.LabelMovingSpeed.Name = "LabelMovingSpeed";
            this.LabelMovingSpeed.Size = new System.Drawing.Size(136, 41);
            this.LabelMovingSpeed.TabIndex = 762;
            this.LabelMovingSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelMovingSpeed.Click += new System.EventHandler(this.LabelCoatData_Click);
            // 
            // LabelCoatingArea
            // 
            this.LabelCoatingArea.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelCoatingArea.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelCoatingArea.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelCoatingArea.ForeColor = System.Drawing.Color.Black;
            this.LabelCoatingArea.Location = new System.Drawing.Point(198, 336);
            this.LabelCoatingArea.Name = "LabelCoatingArea";
            this.LabelCoatingArea.Size = new System.Drawing.Size(136, 41);
            this.LabelCoatingArea.TabIndex = 763;
            this.LabelCoatingArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelCoatingArea.Click += new System.EventHandler(this.LabelCoatData_Click);
            // 
            // autoLabel4
            // 
            this.autoLabel4.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel4.ForeColor = System.Drawing.Color.Black;
            this.autoLabel4.Location = new System.Drawing.Point(334, 72);
            this.autoLabel4.Name = "autoLabel4";
            this.autoLabel4.Size = new System.Drawing.Size(29, 21);
            this.autoLabel4.TabIndex = 764;
            this.autoLabel4.Text = "ml";
            // 
            // autoLabel1
            // 
            this.autoLabel1.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel1.ForeColor = System.Drawing.Color.Black;
            this.autoLabel1.Location = new System.Drawing.Point(334, 119);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(29, 21);
            this.autoLabel1.TabIndex = 765;
            this.autoLabel1.Text = "ml";
            // 
            // autoLabel2
            // 
            this.autoLabel2.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel2.ForeColor = System.Drawing.Color.Black;
            this.autoLabel2.Location = new System.Drawing.Point(334, 166);
            this.autoLabel2.Name = "autoLabel2";
            this.autoLabel2.Size = new System.Drawing.Size(72, 21);
            this.autoLabel2.TabIndex = 766;
            this.autoLabel2.Text = "ml / sec";
            // 
            // autoLabel3
            // 
            this.autoLabel3.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel3.ForeColor = System.Drawing.Color.Black;
            this.autoLabel3.Location = new System.Drawing.Point(334, 213);
            this.autoLabel3.Name = "autoLabel3";
            this.autoLabel3.Size = new System.Drawing.Size(34, 21);
            this.autoLabel3.TabIndex = 767;
            this.autoLabel3.Text = "sec";
            // 
            // autoLabel6
            // 
            this.autoLabel6.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel6.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel6.ForeColor = System.Drawing.Color.Black;
            this.autoLabel6.Location = new System.Drawing.Point(334, 299);
            this.autoLabel6.Name = "autoLabel6";
            this.autoLabel6.Size = new System.Drawing.Size(100, 21);
            this.autoLabel6.TabIndex = 769;
            this.autoLabel6.Text = "degree /sec";
            // 
            // autoLabel7
            // 
            this.autoLabel7.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel7.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel7.ForeColor = System.Drawing.Color.Black;
            this.autoLabel7.Location = new System.Drawing.Point(334, 346);
            this.autoLabel7.Name = "autoLabel7";
            this.autoLabel7.Size = new System.Drawing.Size(40, 21);
            this.autoLabel7.TabIndex = 770;
            this.autoLabel7.Text = "mm";
            // 
            // ComboMode
            // 
            this.ComboMode.DropDownHeight = 200;
            this.ComboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboMode.DropDownWidth = 100;
            this.ComboMode.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ComboMode.FormattingEnabled = true;
            this.ComboMode.IntegralHeight = false;
            this.ComboMode.Items.AddRange(new object[] {
            "AUTO",
            "MANUAL"});
            this.ComboMode.Location = new System.Drawing.Point(198, 252);
            this.ComboMode.Name = "ComboMode";
            this.ComboMode.Size = new System.Drawing.Size(136, 29);
            this.ComboMode.TabIndex = 795;
            this.ComboMode.SelectedIndexChanged += new System.EventHandler(this.ComboMode_SelectedIndexChanged);
            // 
            // autoLabel5
            // 
            this.autoLabel5.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel5.ForeColor = System.Drawing.Color.Black;
            this.autoLabel5.Location = new System.Drawing.Point(45, 72);
            this.autoLabel5.Name = "autoLabel5";
            this.autoLabel5.Size = new System.Drawing.Size(117, 21);
            this.autoLabel5.TabIndex = 796;
            this.autoLabel5.Text = "Coat Quantity";
            // 
            // autoLabel8
            // 
            this.autoLabel8.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel8.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel8.ForeColor = System.Drawing.Color.Black;
            this.autoLabel8.Location = new System.Drawing.Point(13, 119);
            this.autoLabel8.Name = "autoLabel8";
            this.autoLabel8.Size = new System.Drawing.Size(181, 21);
            this.autoLabel8.TabIndex = 797;
            this.autoLabel8.Text = "Moving Coat Quantity";
            // 
            // autoLabel9
            // 
            this.autoLabel9.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel9.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel9.ForeColor = System.Drawing.Color.Black;
            this.autoLabel9.Location = new System.Drawing.Point(49, 166);
            this.autoLabel9.Name = "autoLabel9";
            this.autoLabel9.Size = new System.Drawing.Size(109, 21);
            this.autoLabel9.TabIndex = 798;
            this.autoLabel9.Text = "Coating Rate";
            // 
            // autoLabel10
            // 
            this.autoLabel10.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel10.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel10.ForeColor = System.Drawing.Color.Black;
            this.autoLabel10.Location = new System.Drawing.Point(31, 213);
            this.autoLabel10.Name = "autoLabel10";
            this.autoLabel10.Size = new System.Drawing.Size(144, 21);
            this.autoLabel10.TabIndex = 799;
            this.autoLabel10.Text = "Center Wait Time";
            // 
            // autoLabel11
            // 
            this.autoLabel11.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel11.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel11.ForeColor = System.Drawing.Color.Black;
            this.autoLabel11.Location = new System.Drawing.Point(44, 256);
            this.autoLabel11.Name = "autoLabel11";
            this.autoLabel11.Size = new System.Drawing.Size(118, 21);
            this.autoLabel11.TabIndex = 800;
            this.autoLabel11.Text = "Moving Mode";
            // 
            // autoLabel12
            // 
            this.autoLabel12.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel12.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel12.ForeColor = System.Drawing.Color.Black;
            this.autoLabel12.Location = new System.Drawing.Point(42, 299);
            this.autoLabel12.Name = "autoLabel12";
            this.autoLabel12.Size = new System.Drawing.Size(122, 21);
            this.autoLabel12.TabIndex = 801;
            this.autoLabel12.Text = "Coating Speed";
            // 
            // autoLabel13
            // 
            this.autoLabel13.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel13.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel13.ForeColor = System.Drawing.Color.Black;
            this.autoLabel13.Location = new System.Drawing.Point(48, 346);
            this.autoLabel13.Name = "autoLabel13";
            this.autoLabel13.Size = new System.Drawing.Size(110, 21);
            this.autoLabel13.TabIndex = 802;
            this.autoLabel13.Text = "Coating Area";
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel2.BackgroundImage")));
            this.gradientPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gradientPanel2.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientPanel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel2.Controls.Add(this.autoLabel14);
            this.gradientPanel2.Location = new System.Drawing.Point(9, 15);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(429, 387);
            this.gradientPanel2.TabIndex = 803;
            // 
            // autoLabel14
            // 
            this.autoLabel14.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel14.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel14.Location = new System.Drawing.Point(162, 6);
            this.autoLabel14.Name = "autoLabel14";
            this.autoLabel14.Size = new System.Drawing.Size(104, 15);
            this.autoLabel14.TabIndex = 31;
            this.autoLabel14.Text = "Coating Data";
            // 
            // gradientLabel17
            // 
            this.gradientLabel17.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel17.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel17.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel17.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel17.Location = new System.Drawing.Point(459, 15);
            this.gradientLabel17.Name = "gradientLabel17";
            this.gradientLabel17.Size = new System.Drawing.Size(476, 38);
            this.gradientLabel17.TabIndex = 930;
            this.gradientLabel17.Text = "Coating Sequence Operation";
            this.gradientLabel17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormCoaterData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 672);
            this.Controls.Add(this.gradientLabel17);
            this.Controls.Add(this.autoLabel13);
            this.Controls.Add(this.autoLabel12);
            this.Controls.Add(this.autoLabel11);
            this.Controls.Add(this.autoLabel10);
            this.Controls.Add(this.autoLabel9);
            this.Controls.Add(this.autoLabel8);
            this.Controls.Add(this.autoLabel5);
            this.Controls.Add(this.ComboMode);
            this.Controls.Add(this.autoLabel7);
            this.Controls.Add(this.autoLabel6);
            this.Controls.Add(this.autoLabel3);
            this.Controls.Add(this.autoLabel2);
            this.Controls.Add(this.autoLabel1);
            this.Controls.Add(this.autoLabel4);
            this.Controls.Add(this.LabelCoatingArea);
            this.Controls.Add(this.LabelMovingSpeed);
            this.Controls.Add(this.LabelCenterWaitTime);
            this.Controls.Add(this.LabelCoatingRate);
            this.Controls.Add(this.LabelMovingPVAQty);
            this.Controls.Add(this.LabelPVAQty);
            this.Controls.Add(this.GridCtrl);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.gradientPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormCoaterData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCoaterData_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).EndInit();
            this.gradientPanel2.ResumeLayout(false);
            this.gradientPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnExit;
        private Syncfusion.Windows.Forms.Grid.GridControl GridCtrl;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelPVAQty;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelMovingPVAQty;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelCoatingRate;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelCenterWaitTime;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelMovingSpeed;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelCoatingArea;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel4;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel2;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel3;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel6;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel7;
        private System.Windows.Forms.ComboBox ComboMode;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel5;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel8;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel9;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel10;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel11;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel12;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel13;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel14;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel17;
    }
}