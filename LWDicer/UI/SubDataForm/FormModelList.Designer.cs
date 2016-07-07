namespace LWDicer.UI
{
    partial class FormModelList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModelList));
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle5 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle6 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle7 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle8 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            this.BtnExit = new System.Windows.Forms.Button();
            this.LabelModel = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnMakerCreate = new System.Windows.Forms.Button();
            this.BtnMakerDelete = new System.Windows.Forms.Button();
            this.BtnModelSelect = new System.Windows.Forms.Button();
            this.MakerTreeView = new System.Windows.Forms.TreeView();
            this.imageDir = new System.Windows.Forms.ImageList(this.components);
            this.LabelMaker = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnModelDelete = new System.Windows.Forms.Button();
            this.BtnModelCreate = new System.Windows.Forms.Button();
            this.GridModelList = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.TitleCurModel = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelCurModel = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.TitleCurMaker = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelCurMaker = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            ((System.ComponentModel.ISupportInitialize)(this.GridModelList)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(798, 641);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 749;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // LabelModel
            // 
            this.LabelModel.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.BackwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255))))));
            this.LabelModel.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelModel.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelModel.ForeColor = System.Drawing.Color.Black;
            this.LabelModel.Location = new System.Drawing.Point(359, 51);
            this.LabelModel.Name = "LabelModel";
            this.LabelModel.Size = new System.Drawing.Size(563, 35);
            this.LabelModel.TabIndex = 751;
            this.LabelModel.Tag = "";
            this.LabelModel.Text = "Model List";
            this.LabelModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnMakerCreate
            // 
            this.BtnMakerCreate.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnMakerCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnMakerCreate.Image = ((System.Drawing.Image)(resources.GetObject("BtnMakerCreate.Image")));
            this.BtnMakerCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnMakerCreate.Location = new System.Drawing.Point(54, 642);
            this.BtnMakerCreate.Name = "BtnMakerCreate";
            this.BtnMakerCreate.Size = new System.Drawing.Size(115, 61);
            this.BtnMakerCreate.TabIndex = 752;
            this.BtnMakerCreate.Text = "  생성";
            this.BtnMakerCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnMakerCreate.UseVisualStyleBackColor = true;
            this.BtnMakerCreate.Click += new System.EventHandler(this.BtnMakerCreate_Click);
            // 
            // BtnMakerDelete
            // 
            this.BtnMakerDelete.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnMakerDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnMakerDelete.Image = ((System.Drawing.Image)(resources.GetObject("BtnMakerDelete.Image")));
            this.BtnMakerDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnMakerDelete.Location = new System.Drawing.Point(188, 642);
            this.BtnMakerDelete.Name = "BtnMakerDelete";
            this.BtnMakerDelete.Size = new System.Drawing.Size(115, 61);
            this.BtnMakerDelete.TabIndex = 753;
            this.BtnMakerDelete.Text = "  삭제";
            this.BtnMakerDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnMakerDelete.UseVisualStyleBackColor = true;
            this.BtnMakerDelete.Click += new System.EventHandler(this.BtnMakerDelete_Click);
            // 
            // BtnModelSelect
            // 
            this.BtnModelSelect.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnModelSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnModelSelect.Image = ((System.Drawing.Image)(resources.GetObject("BtnModelSelect.Image")));
            this.BtnModelSelect.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnModelSelect.Location = new System.Drawing.Point(602, 642);
            this.BtnModelSelect.Name = "BtnModelSelect";
            this.BtnModelSelect.Size = new System.Drawing.Size(171, 61);
            this.BtnModelSelect.TabIndex = 754;
            this.BtnModelSelect.Text = "  선택한 모델로 변경";
            this.BtnModelSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnModelSelect.UseVisualStyleBackColor = true;
            this.BtnModelSelect.Click += new System.EventHandler(this.BtnModelSelect_Click);
            // 
            // MakerTreeView
            // 
            this.MakerTreeView.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MakerTreeView.ImageIndex = 1;
            this.MakerTreeView.ImageList = this.imageDir;
            this.MakerTreeView.Location = new System.Drawing.Point(8, 89);
            this.MakerTreeView.Name = "MakerTreeView";
            this.MakerTreeView.SelectedImageIndex = 0;
            this.MakerTreeView.Size = new System.Drawing.Size(345, 537);
            this.MakerTreeView.TabIndex = 759;
            this.MakerTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.MakerlTreeView_NodeMouseClick);
            // 
            // imageDir
            // 
            this.imageDir.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageDir.ImageStream")));
            this.imageDir.TransparentColor = System.Drawing.Color.Transparent;
            this.imageDir.Images.SetKeyName(0, "OPEN_DIR.png");
            this.imageDir.Images.SetKeyName(1, "CLOSE_DIR.png");
            // 
            // LabelMaker
            // 
            this.LabelMaker.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.BackwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0))))));
            this.LabelMaker.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelMaker.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelMaker.ForeColor = System.Drawing.Color.Black;
            this.LabelMaker.Location = new System.Drawing.Point(8, 51);
            this.LabelMaker.Name = "LabelMaker";
            this.LabelMaker.Size = new System.Drawing.Size(345, 35);
            this.LabelMaker.TabIndex = 761;
            this.LabelMaker.Tag = "";
            this.LabelMaker.Text = "Maker List";
            this.LabelMaker.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnModelDelete
            // 
            this.BtnModelDelete.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnModelDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnModelDelete.Image = ((System.Drawing.Image)(resources.GetObject("BtnModelDelete.Image")));
            this.BtnModelDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnModelDelete.Location = new System.Drawing.Point(481, 642);
            this.BtnModelDelete.Name = "BtnModelDelete";
            this.BtnModelDelete.Size = new System.Drawing.Size(115, 61);
            this.BtnModelDelete.TabIndex = 763;
            this.BtnModelDelete.Text = "  삭제";
            this.BtnModelDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnModelDelete.UseVisualStyleBackColor = true;
            this.BtnModelDelete.Click += new System.EventHandler(this.BtnModelDelete_Click);
            // 
            // BtnModelCreate
            // 
            this.BtnModelCreate.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnModelCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnModelCreate.Image = ((System.Drawing.Image)(resources.GetObject("BtnModelCreate.Image")));
            this.BtnModelCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnModelCreate.Location = new System.Drawing.Point(359, 642);
            this.BtnModelCreate.Name = "BtnModelCreate";
            this.BtnModelCreate.Size = new System.Drawing.Size(115, 61);
            this.BtnModelCreate.TabIndex = 762;
            this.BtnModelCreate.Text = "  생성";
            this.BtnModelCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnModelCreate.UseVisualStyleBackColor = true;
            this.BtnModelCreate.Click += new System.EventHandler(this.BtnModelCreate_Click);
            // 
            // GridModelList
            // 
            this.GridModelList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridModelList.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.GridModelList.Location = new System.Drawing.Point(359, 89);
            this.GridModelList.Name = "GridModelList";
            gridRangeStyle5.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle5.StyleInfo.Font.Bold = true;
            gridRangeStyle5.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle5.StyleInfo.Font.Italic = false;
            gridRangeStyle5.StyleInfo.Font.Size = 11.25F;
            gridRangeStyle5.StyleInfo.Font.Strikeout = false;
            gridRangeStyle5.StyleInfo.Font.Underline = false;
            gridRangeStyle5.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle6.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle6.StyleInfo.Font.Bold = true;
            gridRangeStyle6.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle6.StyleInfo.Font.Italic = false;
            gridRangeStyle6.StyleInfo.Font.Size = 11.25F;
            gridRangeStyle6.StyleInfo.Font.Strikeout = false;
            gridRangeStyle6.StyleInfo.Font.Underline = false;
            gridRangeStyle6.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle7.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle7.StyleInfo.Font.Bold = true;
            gridRangeStyle7.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle7.StyleInfo.Font.Italic = false;
            gridRangeStyle7.StyleInfo.Font.Size = 11.25F;
            gridRangeStyle7.StyleInfo.Font.Strikeout = false;
            gridRangeStyle7.StyleInfo.Font.Underline = false;
            gridRangeStyle7.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle8.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle8.StyleInfo.Font.Bold = true;
            gridRangeStyle8.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle8.StyleInfo.Font.Italic = false;
            gridRangeStyle8.StyleInfo.Font.Size = 11.25F;
            gridRangeStyle8.StyleInfo.Font.Strikeout = false;
            gridRangeStyle8.StyleInfo.Font.Underline = false;
            gridRangeStyle8.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            this.GridModelList.RangeStyles.AddRange(new Syncfusion.Windows.Forms.Grid.GridRangeStyle[] {
            gridRangeStyle5,
            gridRangeStyle6,
            gridRangeStyle7,
            gridRangeStyle8});
            this.GridModelList.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridModelList.Size = new System.Drawing.Size(563, 537);
            this.GridModelList.SmartSizeBox = false;
            this.GridModelList.TabIndex = 764;
            this.GridModelList.UseRightToLeftCompatibleTextBox = true;
            this.GridModelList.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridModelList_CellClick);
            // 
            // TitleCurModel
            // 
            this.TitleCurModel.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.LightSteelBlue);
            this.TitleCurModel.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.TitleCurModel.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.TitleCurModel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TitleCurModel.ForeColor = System.Drawing.Color.Black;
            this.TitleCurModel.Location = new System.Drawing.Point(359, 9);
            this.TitleCurModel.Name = "TitleCurModel";
            this.TitleCurModel.Size = new System.Drawing.Size(179, 32);
            this.TitleCurModel.TabIndex = 768;
            this.TitleCurModel.Text = "현재 Model";
            this.TitleCurModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelCurModel
            // 
            this.LabelCurModel.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelCurModel.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelCurModel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelCurModel.ForeColor = System.Drawing.Color.Black;
            this.LabelCurModel.Location = new System.Drawing.Point(541, 9);
            this.LabelCurModel.Name = "LabelCurModel";
            this.LabelCurModel.Size = new System.Drawing.Size(381, 32);
            this.LabelCurModel.TabIndex = 767;
            this.LabelCurModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TitleCurMaker
            // 
            this.TitleCurMaker.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.Firebrick);
            this.TitleCurMaker.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.TitleCurMaker.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.TitleCurMaker.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TitleCurMaker.ForeColor = System.Drawing.Color.White;
            this.TitleCurMaker.Location = new System.Drawing.Point(8, 9);
            this.TitleCurMaker.Name = "TitleCurMaker";
            this.TitleCurMaker.Size = new System.Drawing.Size(137, 32);
            this.TitleCurMaker.TabIndex = 766;
            this.TitleCurMaker.Text = "현재 Maker";
            this.TitleCurMaker.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelCurMaker
            // 
            this.LabelCurMaker.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelCurMaker.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelCurMaker.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelCurMaker.ForeColor = System.Drawing.Color.Black;
            this.LabelCurMaker.Location = new System.Drawing.Point(148, 9);
            this.LabelCurMaker.Name = "LabelCurMaker";
            this.LabelCurMaker.Size = new System.Drawing.Size(205, 32);
            this.LabelCurMaker.TabIndex = 765;
            this.LabelCurMaker.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormModelList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 715);
            this.Controls.Add(this.TitleCurModel);
            this.Controls.Add(this.LabelCurModel);
            this.Controls.Add(this.TitleCurMaker);
            this.Controls.Add(this.LabelCurMaker);
            this.Controls.Add(this.GridModelList);
            this.Controls.Add(this.BtnModelDelete);
            this.Controls.Add(this.BtnModelCreate);
            this.Controls.Add(this.LabelMaker);
            this.Controls.Add(this.MakerTreeView);
            this.Controls.Add(this.BtnModelSelect);
            this.Controls.Add(this.BtnMakerDelete);
            this.Controls.Add(this.BtnMakerCreate);
            this.Controls.Add(this.LabelModel);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormModelList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Model Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormModelData_FormClosing);
            this.Load += new System.EventHandler(this.FormModelData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridModelList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelModel;
        private System.Windows.Forms.Button BtnMakerCreate;
        private System.Windows.Forms.Button BtnMakerDelete;
        private System.Windows.Forms.Button BtnModelSelect;
        private System.Windows.Forms.TreeView MakerTreeView;
        private System.Windows.Forms.ImageList imageDir;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelMaker;
        private System.Windows.Forms.Button BtnModelDelete;
        private System.Windows.Forms.Button BtnModelCreate;
        private Syncfusion.Windows.Forms.Grid.GridControl GridModelList;
        private Syncfusion.Windows.Forms.Tools.GradientLabel TitleCurModel;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelCurModel;
        private Syncfusion.Windows.Forms.Tools.GradientLabel TitleCurMaker;
        public Syncfusion.Windows.Forms.Tools.GradientLabel LabelCurMaker;
    }
}