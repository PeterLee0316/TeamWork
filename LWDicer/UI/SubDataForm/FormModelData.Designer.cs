namespace LWDicer.UI
{
    partial class FormModelData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModelData));
            this.BtnExit = new System.Windows.Forms.Button();
            this.ServoTitle = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnMakerCreate = new System.Windows.Forms.Button();
            this.BtnMakerDelete = new System.Windows.Forms.Button();
            this.BtnModelSelect = new System.Windows.Forms.Button();
            this.MakerlTreeView = new System.Windows.Forms.TreeView();
            this.imageDir = new System.Windows.Forms.ImageList(this.components);
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
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
            // ServoTitle
            // 
            this.ServoTitle.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.BackwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255))))));
            this.ServoTitle.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.ServoTitle.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ServoTitle.ForeColor = System.Drawing.Color.Black;
            this.ServoTitle.Location = new System.Drawing.Point(304, 51);
            this.ServoTitle.Name = "ServoTitle";
            this.ServoTitle.Size = new System.Drawing.Size(461, 35);
            this.ServoTitle.TabIndex = 751;
            this.ServoTitle.Tag = "";
            this.ServoTitle.Text = "Model List";
            this.ServoTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnMakerCreate
            // 
            this.BtnMakerCreate.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnMakerCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnMakerCreate.Image = ((System.Drawing.Image)(resources.GetObject("BtnMakerCreate.Image")));
            this.BtnMakerCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnMakerCreate.Location = new System.Drawing.Point(27, 642);
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
            this.BtnMakerDelete.Location = new System.Drawing.Point(161, 642);
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
            this.BtnModelSelect.Location = new System.Drawing.Point(649, 642);
            this.BtnModelSelect.Name = "BtnModelSelect";
            this.BtnModelSelect.Size = new System.Drawing.Size(115, 61);
            this.BtnModelSelect.TabIndex = 754;
            this.BtnModelSelect.Text = "  선택";
            this.BtnModelSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnModelSelect.UseVisualStyleBackColor = true;
            this.BtnModelSelect.Click += new System.EventHandler(this.BtnModelSelect_Click);
            // 
            // MakerlTreeView
            // 
            this.MakerlTreeView.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MakerlTreeView.ImageIndex = 1;
            this.MakerlTreeView.ImageList = this.imageDir;
            this.MakerlTreeView.Location = new System.Drawing.Point(8, 89);
            this.MakerlTreeView.Name = "MakerlTreeView";
            this.MakerlTreeView.SelectedImageIndex = 0;
            this.MakerlTreeView.Size = new System.Drawing.Size(290, 537);
            this.MakerlTreeView.TabIndex = 759;
            this.MakerlTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.MakerlTreeView_NodeMouseClick);
            // 
            // imageDir
            // 
            this.imageDir.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageDir.ImageStream")));
            this.imageDir.TransparentColor = System.Drawing.Color.Transparent;
            this.imageDir.Images.SetKeyName(0, "OPEN_DIR.png");
            this.imageDir.Images.SetKeyName(1, "CLOSE_DIR.png");
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.BackwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0))))));
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.Black;
            this.gradientLabel1.Location = new System.Drawing.Point(8, 51);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(290, 35);
            this.gradientLabel1.TabIndex = 761;
            this.gradientLabel1.Tag = "";
            this.gradientLabel1.Text = "Maker List";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnModelDelete
            // 
            this.BtnModelDelete.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnModelDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnModelDelete.Image = ((System.Drawing.Image)(resources.GetObject("BtnModelDelete.Image")));
            this.BtnModelDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnModelDelete.Location = new System.Drawing.Point(527, 642);
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
            this.BtnModelCreate.Location = new System.Drawing.Point(405, 642);
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
            this.GridModelList.Location = new System.Drawing.Point(304, 89);
            this.GridModelList.Name = "GridModelList";
            this.GridModelList.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridModelList.Size = new System.Drawing.Size(461, 537);
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
            this.TitleCurModel.Location = new System.Drawing.Point(304, 9);
            this.TitleCurModel.Name = "TitleCurModel";
            this.TitleCurModel.Size = new System.Drawing.Size(137, 32);
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
            this.LabelCurModel.Location = new System.Drawing.Point(443, 9);
            this.LabelCurModel.Name = "LabelCurModel";
            this.LabelCurModel.Size = new System.Drawing.Size(322, 32);
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
            this.TitleCurMaker.Size = new System.Drawing.Size(113, 32);
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
            this.LabelCurMaker.Location = new System.Drawing.Point(123, 9);
            this.LabelCurMaker.Name = "LabelCurMaker";
            this.LabelCurMaker.Size = new System.Drawing.Size(175, 32);
            this.LabelCurMaker.TabIndex = 765;
            this.LabelCurMaker.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormModelData
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
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.MakerlTreeView);
            this.Controls.Add(this.BtnModelSelect);
            this.Controls.Add(this.BtnMakerDelete);
            this.Controls.Add(this.BtnMakerCreate);
            this.Controls.Add(this.ServoTitle);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormModelData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Model Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormModelData_FormClosing);
            this.Load += new System.EventHandler(this.FormModelData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridModelList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private Syncfusion.Windows.Forms.Tools.GradientLabel ServoTitle;
        private System.Windows.Forms.Button BtnMakerCreate;
        private System.Windows.Forms.Button BtnMakerDelete;
        private System.Windows.Forms.Button BtnModelSelect;
        private System.Windows.Forms.TreeView MakerlTreeView;
        private System.Windows.Forms.ImageList imageDir;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private System.Windows.Forms.Button BtnModelDelete;
        private System.Windows.Forms.Button BtnModelCreate;
        private Syncfusion.Windows.Forms.Grid.GridControl GridModelList;
        private Syncfusion.Windows.Forms.Tools.GradientLabel TitleCurModel;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelCurModel;
        private Syncfusion.Windows.Forms.Tools.GradientLabel TitleCurMaker;
        public Syncfusion.Windows.Forms.Tools.GradientLabel LabelCurMaker;
    }
}