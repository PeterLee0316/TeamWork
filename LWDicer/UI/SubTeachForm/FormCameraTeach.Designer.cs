namespace LWDicer.UI
{
    partial class FormCameraTeach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCameraTeach));
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnJog = new System.Windows.Forms.Button();
            this.TmrTeach = new System.Windows.Forms.Timer(this.components);
            this.GridTeachTable = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.BtnChangeValue = new System.Windows.Forms.Button();
            this.BtnTeachMove = new System.Windows.Forms.Button();
            this.gradientLabel7 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel6 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel5 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelTeach1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel4 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel3 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnPos2 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos1 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnSave = new System.Windows.Forms.Button();
            this.gradientLabel16 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnPos3 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos4 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos5 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos10 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos9 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos8 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos7 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos6 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos15 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos14 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos13 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos12 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPos11 = new Syncfusion.Windows.Forms.ButtonAdv();
            ((System.ComponentModel.ISupportInitialize)(this.GridTeachTable)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(1124, 807);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 755;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnJog
            // 
            this.BtnJog.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnJog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnJog.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnJog.Location = new System.Drawing.Point(1124, 740);
            this.BtnJog.Name = "BtnJog";
            this.BtnJog.Size = new System.Drawing.Size(124, 61);
            this.BtnJog.TabIndex = 757;
            this.BtnJog.Text = "Jog";
            this.BtnJog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnJog.UseVisualStyleBackColor = true;
            this.BtnJog.Click += new System.EventHandler(this.BtnJog_Click);
            // 
            // TmrTeach
            // 
            this.TmrTeach.Tick += new System.EventHandler(this.TmrTeach_Tick);
            // 
            // GridTeachTable
            // 
            this.GridTeachTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridTeachTable.Location = new System.Drawing.Point(21, 57);
            this.GridTeachTable.Name = "GridTeachTable";
            this.GridTeachTable.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridTeachTable.Size = new System.Drawing.Size(453, 322);
            this.GridTeachTable.SmartSizeBox = false;
            this.GridTeachTable.TabIndex = 837;
            this.GridTeachTable.Text = "gridControl1";
            this.GridTeachTable.UseRightToLeftCompatibleTextBox = true;
            this.GridTeachTable.PushButtonClick += new Syncfusion.Windows.Forms.Grid.GridCellPushButtonClickEventHandler(this.GridTeachTable_PushButtonClick);
            // 
            // BtnChangeValue
            // 
            this.BtnChangeValue.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnChangeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnChangeValue.Location = new System.Drawing.Point(484, 133);
            this.BtnChangeValue.Name = "BtnChangeValue";
            this.BtnChangeValue.Size = new System.Drawing.Size(131, 72);
            this.BtnChangeValue.TabIndex = 875;
            this.BtnChangeValue.Text = "목표 위치를 현재값으로 변경";
            this.BtnChangeValue.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnChangeValue.UseVisualStyleBackColor = true;
            this.BtnChangeValue.Click += new System.EventHandler(this.BtnChangeValue_Click);
            // 
            // BtnTeachMove
            // 
            this.BtnTeachMove.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTeachMove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnTeachMove.Location = new System.Drawing.Point(484, 57);
            this.BtnTeachMove.Name = "BtnTeachMove";
            this.BtnTeachMove.Size = new System.Drawing.Size(131, 72);
            this.BtnTeachMove.TabIndex = 874;
            this.BtnTeachMove.Text = "목표 위치로 이동";
            this.BtnTeachMove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnTeachMove.UseVisualStyleBackColor = true;
            this.BtnTeachMove.Click += new System.EventHandler(this.BtnTeachMove_Click);
            // 
            // gradientLabel7
            // 
            this.gradientLabel7.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0))))));
            this.gradientLabel7.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel7.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientLabel7.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel7.Location = new System.Drawing.Point(318, 437);
            this.gradientLabel7.Name = "gradientLabel7";
            this.gradientLabel7.Size = new System.Drawing.Size(147, 40);
            this.gradientLabel7.TabIndex = 873;
            this.gradientLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel6
            // 
            this.gradientLabel6.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0))))));
            this.gradientLabel6.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel6.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientLabel6.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel6.Location = new System.Drawing.Point(318, 396);
            this.gradientLabel6.Name = "gradientLabel6";
            this.gradientLabel6.Size = new System.Drawing.Size(147, 40);
            this.gradientLabel6.TabIndex = 872;
            this.gradientLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel5
            // 
            this.gradientLabel5.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0))))));
            this.gradientLabel5.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel5.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientLabel5.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel5.Location = new System.Drawing.Point(21, 437);
            this.gradientLabel5.Name = "gradientLabel5";
            this.gradientLabel5.Size = new System.Drawing.Size(147, 40);
            this.gradientLabel5.TabIndex = 871;
            this.gradientLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelTeach1
            // 
            this.LabelTeach1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0))))));
            this.LabelTeach1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTeach1.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.LabelTeach1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTeach1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LabelTeach1.Location = new System.Drawing.Point(21, 396);
            this.LabelTeach1.Name = "LabelTeach1";
            this.LabelTeach1.Size = new System.Drawing.Size(147, 40);
            this.LabelTeach1.TabIndex = 870;
            this.LabelTeach1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel4
            // 
            this.gradientLabel4.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.gradientLabel4.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel4.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel4.ForeColor = System.Drawing.Color.Black;
            this.gradientLabel4.Location = new System.Drawing.Point(466, 437);
            this.gradientLabel4.Name = "gradientLabel4";
            this.gradientLabel4.Size = new System.Drawing.Size(148, 40);
            this.gradientLabel4.TabIndex = 869;
            this.gradientLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel3
            // 
            this.gradientLabel3.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.gradientLabel3.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel3.ForeColor = System.Drawing.Color.Black;
            this.gradientLabel3.Location = new System.Drawing.Point(466, 396);
            this.gradientLabel3.Name = "gradientLabel3";
            this.gradientLabel3.Size = new System.Drawing.Size(148, 40);
            this.gradientLabel3.TabIndex = 868;
            this.gradientLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.gradientLabel2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel2.ForeColor = System.Drawing.Color.Black;
            this.gradientLabel2.Location = new System.Drawing.Point(169, 437);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(148, 40);
            this.gradientLabel2.TabIndex = 867;
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.Black;
            this.gradientLabel1.Location = new System.Drawing.Point(169, 396);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(148, 40);
            this.gradientLabel1.TabIndex = 866;
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnPos2
            // 
            this.BtnPos2.BackColor = System.Drawing.Color.Tan;
            this.BtnPos2.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos2.FlatAppearance.BorderSize = 5;
            this.BtnPos2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos2.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos2.Location = new System.Drawing.Point(140, 488);
            this.BtnPos2.Name = "BtnPos2";
            this.BtnPos2.Size = new System.Drawing.Size(117, 64);
            this.BtnPos2.TabIndex = 865;
            this.BtnPos2.Tag = "1";
            this.BtnPos2.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos1
            // 
            this.BtnPos1.BackColor = System.Drawing.Color.Tan;
            this.BtnPos1.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos1.FlatAppearance.BorderSize = 5;
            this.BtnPos1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos1.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos1.Location = new System.Drawing.Point(21, 488);
            this.BtnPos1.Name = "BtnPos1";
            this.BtnPos1.Size = new System.Drawing.Size(117, 64);
            this.BtnPos1.TabIndex = 861;
            this.BtnPos1.Tag = "0";
            this.BtnPos1.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.Location = new System.Drawing.Point(484, 209);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(131, 72);
            this.BtnSave.TabIndex = 860;
            this.BtnSave.Text = " Save";
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // gradientLabel16
            // 
            this.gradientLabel16.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel16.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel16.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel16.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel16.Location = new System.Drawing.Point(21, 14);
            this.gradientLabel16.Name = "gradientLabel16";
            this.gradientLabel16.Size = new System.Drawing.Size(594, 38);
            this.gradientLabel16.TabIndex = 931;
            this.gradientLabel16.Text = "Camera Teaching Table";
            this.gradientLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnPos3
            // 
            this.BtnPos3.BackColor = System.Drawing.Color.Tan;
            this.BtnPos3.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos3.FlatAppearance.BorderSize = 5;
            this.BtnPos3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos3.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos3.Location = new System.Drawing.Point(259, 488);
            this.BtnPos3.Name = "BtnPos3";
            this.BtnPos3.Size = new System.Drawing.Size(117, 64);
            this.BtnPos3.TabIndex = 932;
            this.BtnPos3.Tag = "3";
            this.BtnPos3.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos4
            // 
            this.BtnPos4.BackColor = System.Drawing.Color.Tan;
            this.BtnPos4.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos4.FlatAppearance.BorderSize = 5;
            this.BtnPos4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos4.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos4.Location = new System.Drawing.Point(378, 488);
            this.BtnPos4.Name = "BtnPos4";
            this.BtnPos4.Size = new System.Drawing.Size(117, 64);
            this.BtnPos4.TabIndex = 933;
            this.BtnPos4.Tag = "4";
            this.BtnPos4.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos5
            // 
            this.BtnPos5.BackColor = System.Drawing.Color.Tan;
            this.BtnPos5.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos5.FlatAppearance.BorderSize = 5;
            this.BtnPos5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos5.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos5.Location = new System.Drawing.Point(497, 488);
            this.BtnPos5.Name = "BtnPos5";
            this.BtnPos5.Size = new System.Drawing.Size(117, 64);
            this.BtnPos5.TabIndex = 934;
            this.BtnPos5.Tag = "5";
            this.BtnPos5.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos10
            // 
            this.BtnPos10.BackColor = System.Drawing.Color.Tan;
            this.BtnPos10.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos10.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos10.FlatAppearance.BorderSize = 5;
            this.BtnPos10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos10.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos10.Location = new System.Drawing.Point(497, 558);
            this.BtnPos10.Name = "BtnPos10";
            this.BtnPos10.Size = new System.Drawing.Size(117, 64);
            this.BtnPos10.TabIndex = 939;
            this.BtnPos10.Tag = "10";
            this.BtnPos10.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos9
            // 
            this.BtnPos9.BackColor = System.Drawing.Color.Tan;
            this.BtnPos9.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos9.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos9.FlatAppearance.BorderSize = 5;
            this.BtnPos9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos9.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos9.Location = new System.Drawing.Point(378, 558);
            this.BtnPos9.Name = "BtnPos9";
            this.BtnPos9.Size = new System.Drawing.Size(117, 64);
            this.BtnPos9.TabIndex = 938;
            this.BtnPos9.Tag = "9";
            this.BtnPos9.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos8
            // 
            this.BtnPos8.BackColor = System.Drawing.Color.Tan;
            this.BtnPos8.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos8.FlatAppearance.BorderSize = 5;
            this.BtnPos8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos8.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos8.Location = new System.Drawing.Point(259, 558);
            this.BtnPos8.Name = "BtnPos8";
            this.BtnPos8.Size = new System.Drawing.Size(117, 64);
            this.BtnPos8.TabIndex = 937;
            this.BtnPos8.Tag = "8";
            this.BtnPos8.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos7
            // 
            this.BtnPos7.BackColor = System.Drawing.Color.Tan;
            this.BtnPos7.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos7.FlatAppearance.BorderSize = 5;
            this.BtnPos7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos7.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos7.Location = new System.Drawing.Point(140, 558);
            this.BtnPos7.Name = "BtnPos7";
            this.BtnPos7.Size = new System.Drawing.Size(117, 64);
            this.BtnPos7.TabIndex = 936;
            this.BtnPos7.Tag = "7";
            this.BtnPos7.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos6
            // 
            this.BtnPos6.BackColor = System.Drawing.Color.Tan;
            this.BtnPos6.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos6.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos6.FlatAppearance.BorderSize = 5;
            this.BtnPos6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos6.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos6.Location = new System.Drawing.Point(21, 558);
            this.BtnPos6.Name = "BtnPos6";
            this.BtnPos6.Size = new System.Drawing.Size(117, 64);
            this.BtnPos6.TabIndex = 935;
            this.BtnPos6.Tag = "6";
            this.BtnPos6.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos15
            // 
            this.BtnPos15.BackColor = System.Drawing.Color.Tan;
            this.BtnPos15.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos15.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos15.FlatAppearance.BorderSize = 5;
            this.BtnPos15.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos15.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos15.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos15.Location = new System.Drawing.Point(497, 628);
            this.BtnPos15.Name = "BtnPos15";
            this.BtnPos15.Size = new System.Drawing.Size(117, 64);
            this.BtnPos15.TabIndex = 944;
            this.BtnPos15.Tag = "15";
            this.BtnPos15.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos14
            // 
            this.BtnPos14.BackColor = System.Drawing.Color.Tan;
            this.BtnPos14.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos14.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos14.FlatAppearance.BorderSize = 5;
            this.BtnPos14.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos14.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos14.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos14.Location = new System.Drawing.Point(378, 628);
            this.BtnPos14.Name = "BtnPos14";
            this.BtnPos14.Size = new System.Drawing.Size(117, 64);
            this.BtnPos14.TabIndex = 943;
            this.BtnPos14.Tag = "14";
            this.BtnPos14.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos13
            // 
            this.BtnPos13.BackColor = System.Drawing.Color.Tan;
            this.BtnPos13.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos13.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos13.FlatAppearance.BorderSize = 5;
            this.BtnPos13.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos13.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos13.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos13.Location = new System.Drawing.Point(259, 628);
            this.BtnPos13.Name = "BtnPos13";
            this.BtnPos13.Size = new System.Drawing.Size(117, 64);
            this.BtnPos13.TabIndex = 942;
            this.BtnPos13.Tag = "13";
            this.BtnPos13.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos12
            // 
            this.BtnPos12.BackColor = System.Drawing.Color.Tan;
            this.BtnPos12.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos12.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos12.FlatAppearance.BorderSize = 5;
            this.BtnPos12.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos12.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos12.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos12.Location = new System.Drawing.Point(140, 628);
            this.BtnPos12.Name = "BtnPos12";
            this.BtnPos12.Size = new System.Drawing.Size(117, 64);
            this.BtnPos12.TabIndex = 941;
            this.BtnPos12.Tag = "12";
            this.BtnPos12.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // BtnPos11
            // 
            this.BtnPos11.BackColor = System.Drawing.Color.Tan;
            this.BtnPos11.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPos11.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPos11.FlatAppearance.BorderSize = 5;
            this.BtnPos11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPos11.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPos11.Location = new System.Drawing.Point(21, 628);
            this.BtnPos11.Name = "BtnPos11";
            this.BtnPos11.Size = new System.Drawing.Size(117, 64);
            this.BtnPos11.TabIndex = 940;
            this.BtnPos11.Tag = "11";
            this.BtnPos11.Click += new System.EventHandler(this.BtnPos_Click);
            // 
            // FormCameraTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 884);
            this.Controls.Add(this.BtnPos15);
            this.Controls.Add(this.BtnPos14);
            this.Controls.Add(this.BtnPos13);
            this.Controls.Add(this.BtnPos12);
            this.Controls.Add(this.BtnPos11);
            this.Controls.Add(this.BtnPos10);
            this.Controls.Add(this.BtnPos9);
            this.Controls.Add(this.BtnPos8);
            this.Controls.Add(this.BtnPos7);
            this.Controls.Add(this.BtnPos6);
            this.Controls.Add(this.BtnPos5);
            this.Controls.Add(this.BtnPos4);
            this.Controls.Add(this.BtnPos3);
            this.Controls.Add(this.gradientLabel16);
            this.Controls.Add(this.GridTeachTable);
            this.Controls.Add(this.BtnChangeValue);
            this.Controls.Add(this.BtnTeachMove);
            this.Controls.Add(this.gradientLabel7);
            this.Controls.Add(this.gradientLabel6);
            this.Controls.Add(this.gradientLabel5);
            this.Controls.Add(this.LabelTeach1);
            this.Controls.Add(this.gradientLabel4);
            this.Controls.Add(this.gradientLabel3);
            this.Controls.Add(this.gradientLabel2);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.BtnPos2);
            this.Controls.Add(this.BtnPos1);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnJog);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormCameraTeach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCameraTeach";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCameraTeach_FormClosing);
            this.Load += new System.EventHandler(this.FormCameraTeach_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridTeachTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnJog;
        private System.Windows.Forms.Timer TmrTeach;
        private Syncfusion.Windows.Forms.Grid.GridControl GridTeachTable;
        private System.Windows.Forms.Button BtnChangeValue;
        private System.Windows.Forms.Button BtnTeachMove;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel7;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel6;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel5;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTeach1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel4;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel3;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel2;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos2;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos1;
        private System.Windows.Forms.Button BtnSave;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel16;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos3;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos4;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos5;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos10;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos9;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos8;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos7;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos6;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos15;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos14;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos13;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos12;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPos11;
    }
}