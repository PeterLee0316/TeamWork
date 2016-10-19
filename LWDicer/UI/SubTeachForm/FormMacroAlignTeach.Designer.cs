namespace LWDicer.UI
{
    partial class FormMacroAlignTeach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMacroAlignTeach));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picPatternMarkB = new System.Windows.Forms.PictureBox();
            this.picPatternMarkA = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSearchMarkB = new System.Windows.Forms.Button();
            this.btnSearchMarkA = new System.Windows.Forms.Button();
            this.btnRegisterMarkB = new System.Windows.Forms.Button();
            this.btnRegisterMarkA = new System.Windows.Forms.Button();
            this.picVision = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.pnlStageJog = new System.Windows.Forms.Panel();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectStageMove = new System.Windows.Forms.Button();
            this.btnSelectFocus = new System.Windows.Forms.Button();
            this.btnChangeCam = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblCamPosZ = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblStagePosT = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.lblStagePosY = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.lblStagePosX = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.btnRoiWidthNarrow = new System.Windows.Forms.Button();
            this.btnRoiWidthWide = new System.Windows.Forms.Button();
            this.btnRoiHeightNarrow = new System.Windows.Forms.Button();
            this.btnRoiHeightWide = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.lblRoiWidth = new System.Windows.Forms.Label();
            this.lblRoiHeight = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TmrTeach = new System.Windows.Forms.Timer(this.components);
            this.lblSearchResult = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPatternMarkB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPatternMarkA)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(822, 324);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 201);
            this.groupBox1.TabIndex = 945;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pattern Mark";
            // 
            // picPatternMarkB
            // 
            this.picPatternMarkB.Location = new System.Drawing.Point(0, 0);
            this.picPatternMarkB.Name = "picPatternMarkB";
            this.picPatternMarkB.Size = new System.Drawing.Size(167, 105);
            this.picPatternMarkB.TabIndex = 1;
            this.picPatternMarkB.TabStop = false;
            // 
            // picPatternMarkA
            // 
            this.picPatternMarkA.Location = new System.Drawing.Point(0, 0);
            this.picPatternMarkA.Name = "picPatternMarkA";
            this.picPatternMarkA.Size = new System.Drawing.Size(167, 116);
            this.picPatternMarkA.TabIndex = 0;
            this.picPatternMarkA.TabStop = false;
            this.picPatternMarkA.Click += new System.EventHandler(this.picPatternMarkA_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblSearchResult);
            this.groupBox4.Controls.Add(this.lblRoiHeight);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.btnSearchMarkB);
            this.groupBox4.Controls.Add(this.btnSearchMarkA);
            this.groupBox4.Controls.Add(this.lblRoiWidth);
            this.groupBox4.Controls.Add(this.btnRoiHeightNarrow);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.btnRoiHeightWide);
            this.groupBox4.Controls.Add(this.btnRoiWidthNarrow);
            this.groupBox4.Controls.Add(this.btnRoiWidthWide);
            this.groupBox4.Controls.Add(this.btnRegisterMarkB);
            this.groupBox4.Controls.Add(this.btnRegisterMarkA);
            this.groupBox4.Location = new System.Drawing.Point(822, 127);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(428, 189);
            this.groupBox4.TabIndex = 944;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Vision Control";
            // 
            // btnSearchMarkB
            // 
            this.btnSearchMarkB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchMarkB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSearchMarkB.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearchMarkB.Location = new System.Drawing.Point(96, 90);
            this.btnSearchMarkB.Name = "btnSearchMarkB";
            this.btnSearchMarkB.Size = new System.Drawing.Size(75, 54);
            this.btnSearchMarkB.TabIndex = 768;
            this.btnSearchMarkB.Text = "Mark Search B";
            this.btnSearchMarkB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearchMarkB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSearchMarkB.UseVisualStyleBackColor = true;
            this.btnSearchMarkB.Click += new System.EventHandler(this.btnSearchMarkB_Click);
            // 
            // btnSearchMarkA
            // 
            this.btnSearchMarkA.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchMarkA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSearchMarkA.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearchMarkA.Location = new System.Drawing.Point(96, 29);
            this.btnSearchMarkA.Name = "btnSearchMarkA";
            this.btnSearchMarkA.Size = new System.Drawing.Size(75, 54);
            this.btnSearchMarkA.TabIndex = 767;
            this.btnSearchMarkA.Text = "Mark Search A";
            this.btnSearchMarkA.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearchMarkA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSearchMarkA.UseVisualStyleBackColor = true;
            this.btnSearchMarkA.Click += new System.EventHandler(this.btnSearchMarkA_Click);
            // 
            // btnRegisterMarkB
            // 
            this.btnRegisterMarkB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegisterMarkB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRegisterMarkB.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRegisterMarkB.Location = new System.Drawing.Point(13, 89);
            this.btnRegisterMarkB.Name = "btnRegisterMarkB";
            this.btnRegisterMarkB.Size = new System.Drawing.Size(75, 54);
            this.btnRegisterMarkB.TabIndex = 766;
            this.btnRegisterMarkB.Text = "Mark 등록 B";
            this.btnRegisterMarkB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRegisterMarkB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRegisterMarkB.UseVisualStyleBackColor = true;
            this.btnRegisterMarkB.Click += new System.EventHandler(this.btnRegisterMarkB_Click);
            // 
            // btnRegisterMarkA
            // 
            this.btnRegisterMarkA.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegisterMarkA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRegisterMarkA.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRegisterMarkA.Location = new System.Drawing.Point(13, 29);
            this.btnRegisterMarkA.Name = "btnRegisterMarkA";
            this.btnRegisterMarkA.Size = new System.Drawing.Size(75, 54);
            this.btnRegisterMarkA.TabIndex = 765;
            this.btnRegisterMarkA.Text = "Mark 등록 A";
            this.btnRegisterMarkA.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRegisterMarkA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRegisterMarkA.UseVisualStyleBackColor = true;
            this.btnRegisterMarkA.Click += new System.EventHandler(this.btnRegisterMarkA_Click);
            // 
            // picVision
            // 
            this.picVision.Location = new System.Drawing.Point(13, 55);
            this.picVision.Name = "picVision";
            this.picVision.Size = new System.Drawing.Size(802, 600);
            this.picVision.TabIndex = 943;
            this.picVision.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picVision_MouseDown);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(1125, 807);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 946;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // pnlStageJog
            // 
            this.pnlStageJog.Location = new System.Drawing.Point(13, 661);
            this.pnlStageJog.Name = "pnlStageJog";
            this.pnlStageJog.Size = new System.Drawing.Size(797, 218);
            this.pnlStageJog.TabIndex = 948;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel1.Location = new System.Drawing.Point(13, 1);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(802, 51);
            this.gradientLabel1.TabIndex = 949;
            this.gradientLabel1.Text = "Macro Align";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSelectStageMove);
            this.groupBox2.Controls.Add(this.btnSelectFocus);
            this.groupBox2.Controls.Add(this.btnChangeCam);
            this.groupBox2.Location = new System.Drawing.Point(822, 660);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 216);
            this.groupBox2.TabIndex = 953;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vision && Stage Control";
            // 
            // btnSelectStageMove
            // 
            this.btnSelectStageMove.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelectStageMove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSelectStageMove.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectStageMove.Location = new System.Drawing.Point(93, 119);
            this.btnSelectStageMove.Name = "btnSelectStageMove";
            this.btnSelectStageMove.Size = new System.Drawing.Size(75, 77);
            this.btnSelectStageMove.TabIndex = 761;
            this.btnSelectStageMove.Text = "Stage Manual";
            this.btnSelectStageMove.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelectStageMove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSelectStageMove.UseVisualStyleBackColor = true;
            this.btnSelectStageMove.Click += new System.EventHandler(this.btnSelectStageMove_Click);
            // 
            // btnSelectFocus
            // 
            this.btnSelectFocus.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelectFocus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSelectFocus.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectFocus.Location = new System.Drawing.Point(12, 119);
            this.btnSelectFocus.Name = "btnSelectFocus";
            this.btnSelectFocus.Size = new System.Drawing.Size(75, 77);
            this.btnSelectFocus.TabIndex = 760;
            this.btnSelectFocus.Text = "Cam Focus";
            this.btnSelectFocus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelectFocus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSelectFocus.UseVisualStyleBackColor = true;
            this.btnSelectFocus.Click += new System.EventHandler(this.btnSelectFocus_Click);
            // 
            // btnChangeCam
            // 
            this.btnChangeCam.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnChangeCam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnChangeCam.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnChangeCam.Location = new System.Drawing.Point(12, 36);
            this.btnChangeCam.Name = "btnChangeCam";
            this.btnChangeCam.Size = new System.Drawing.Size(75, 77);
            this.btnChangeCam.TabIndex = 758;
            this.btnChangeCam.Text = "Change Magnitude";
            this.btnChangeCam.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnChangeCam.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnChangeCam.UseVisualStyleBackColor = true;
            this.btnChangeCam.Click += new System.EventHandler(this.btnChangeCam_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.lblCamPosZ);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lblStagePosT);
            this.groupBox3.Controls.Add(this.lblStagePosY);
            this.groupBox3.Controls.Add(this.lblStagePosX);
            this.groupBox3.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox3.Location = new System.Drawing.Point(821, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(439, 120);
            this.groupBox3.TabIndex = 952;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stage Position";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(193, 93);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(31, 15);
            this.label15.TabIndex = 969;
            this.label15.Text = "mm";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(193, 73);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 15);
            this.label14.TabIndex = 968;
            this.label14.Text = "\"";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(193, 53);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 15);
            this.label13.TabIndex = 967;
            this.label13.Text = "mm";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(193, 34);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 15);
            this.label12.TabIndex = 966;
            this.label12.Text = "mm";
            // 
            // lblCamPosZ
            // 
            this.lblCamPosZ.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))));
            this.lblCamPosZ.BorderAppearance = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCamPosZ.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblCamPosZ.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblCamPosZ.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCamPosZ.ForeColor = System.Drawing.Color.Black;
            this.lblCamPosZ.Location = new System.Drawing.Point(97, 93);
            this.lblCamPosZ.Name = "lblCamPosZ";
            this.lblCamPosZ.Size = new System.Drawing.Size(90, 19);
            this.lblCamPosZ.TabIndex = 965;
            this.lblCamPosZ.Text = "0000";
            this.lblCamPosZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(23, 94);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 15);
            this.label11.TabIndex = 964;
            this.label11.Text = "Z Axis : ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(23, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 15);
            this.label10.TabIndex = 963;
            this.label10.Text = "T Axis : ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(23, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 15);
            this.label9.TabIndex = 962;
            this.label9.Text = "Y Axis : ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(23, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 15);
            this.label8.TabIndex = 961;
            this.label8.Text = "X Axis :";
            // 
            // lblStagePosT
            // 
            this.lblStagePosT.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))));
            this.lblStagePosT.BorderAppearance = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStagePosT.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblStagePosT.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblStagePosT.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStagePosT.ForeColor = System.Drawing.Color.Black;
            this.lblStagePosT.Location = new System.Drawing.Point(97, 73);
            this.lblStagePosT.Name = "lblStagePosT";
            this.lblStagePosT.Size = new System.Drawing.Size(90, 19);
            this.lblStagePosT.TabIndex = 795;
            this.lblStagePosT.Text = "0000";
            this.lblStagePosT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStagePosY
            // 
            this.lblStagePosY.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))));
            this.lblStagePosY.BorderAppearance = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStagePosY.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblStagePosY.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblStagePosY.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStagePosY.ForeColor = System.Drawing.Color.Black;
            this.lblStagePosY.Location = new System.Drawing.Point(97, 53);
            this.lblStagePosY.Name = "lblStagePosY";
            this.lblStagePosY.Size = new System.Drawing.Size(90, 19);
            this.lblStagePosY.TabIndex = 794;
            this.lblStagePosY.Text = "0000";
            this.lblStagePosY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStagePosX
            // 
            this.lblStagePosX.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))));
            this.lblStagePosX.BorderAppearance = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStagePosX.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblStagePosX.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.lblStagePosX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStagePosX.ForeColor = System.Drawing.Color.Black;
            this.lblStagePosX.Location = new System.Drawing.Point(97, 32);
            this.lblStagePosX.Name = "lblStagePosX";
            this.lblStagePosX.Size = new System.Drawing.Size(90, 19);
            this.lblStagePosX.TabIndex = 793;
            this.lblStagePosX.Text = "0000";
            this.lblStagePosX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRoiWidthNarrow
            // 
            this.btnRoiWidthNarrow.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRoiWidthNarrow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRoiWidthNarrow.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRoiWidthNarrow.Location = new System.Drawing.Point(336, 43);
            this.btnRoiWidthNarrow.Name = "btnRoiWidthNarrow";
            this.btnRoiWidthNarrow.Size = new System.Drawing.Size(75, 40);
            this.btnRoiWidthNarrow.TabIndex = 979;
            this.btnRoiWidthNarrow.Text = "ROI Width Narrow";
            this.btnRoiWidthNarrow.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRoiWidthNarrow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRoiWidthNarrow.UseVisualStyleBackColor = true;
            this.btnRoiWidthNarrow.Click += new System.EventHandler(this.btnRoiWidthNarrow_Click);
            // 
            // btnRoiWidthWide
            // 
            this.btnRoiWidthWide.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRoiWidthWide.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRoiWidthWide.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRoiWidthWide.Location = new System.Drawing.Point(258, 43);
            this.btnRoiWidthWide.Name = "btnRoiWidthWide";
            this.btnRoiWidthWide.Size = new System.Drawing.Size(75, 40);
            this.btnRoiWidthWide.TabIndex = 978;
            this.btnRoiWidthWide.Text = "ROI Width Wide";
            this.btnRoiWidthWide.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRoiWidthWide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRoiWidthWide.UseVisualStyleBackColor = true;
            this.btnRoiWidthWide.Click += new System.EventHandler(this.btnRoiWidthWide_Click);
            // 
            // btnRoiHeightNarrow
            // 
            this.btnRoiHeightNarrow.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRoiHeightNarrow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRoiHeightNarrow.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRoiHeightNarrow.Location = new System.Drawing.Point(336, 104);
            this.btnRoiHeightNarrow.Name = "btnRoiHeightNarrow";
            this.btnRoiHeightNarrow.Size = new System.Drawing.Size(75, 40);
            this.btnRoiHeightNarrow.TabIndex = 981;
            this.btnRoiHeightNarrow.Text = "ROI Height Narrow";
            this.btnRoiHeightNarrow.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRoiHeightNarrow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRoiHeightNarrow.UseVisualStyleBackColor = true;
            this.btnRoiHeightNarrow.Click += new System.EventHandler(this.btnRoiHeightNarrow_Click);
            // 
            // btnRoiHeightWide
            // 
            this.btnRoiHeightWide.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRoiHeightWide.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRoiHeightWide.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRoiHeightWide.Location = new System.Drawing.Point(258, 104);
            this.btnRoiHeightWide.Name = "btnRoiHeightWide";
            this.btnRoiHeightWide.Size = new System.Drawing.Size(75, 40);
            this.btnRoiHeightWide.TabIndex = 980;
            this.btnRoiHeightWide.Text = "ROI Height Wide";
            this.btnRoiHeightWide.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRoiHeightWide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRoiHeightWide.UseVisualStyleBackColor = true;
            this.btnRoiHeightWide.Click += new System.EventHandler(this.btnRoiHeightWide_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.Location = new System.Drawing.Point(262, 28);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(67, 13);
            this.label16.TabIndex = 980;
            this.label16.Text = "ROI Width";
            // 
            // lblRoiWidth
            // 
            this.lblRoiWidth.AutoSize = true;
            this.lblRoiWidth.BackColor = System.Drawing.Color.White;
            this.lblRoiWidth.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRoiWidth.Location = new System.Drawing.Point(337, 28);
            this.lblRoiWidth.Name = "lblRoiWidth";
            this.lblRoiWidth.Size = new System.Drawing.Size(72, 13);
            this.lblRoiWidth.TabIndex = 981;
            this.lblRoiWidth.Text = "0.0000 mm";
            // 
            // lblRoiHeight
            // 
            this.lblRoiHeight.AutoSize = true;
            this.lblRoiHeight.BackColor = System.Drawing.Color.White;
            this.lblRoiHeight.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRoiHeight.Location = new System.Drawing.Point(336, 90);
            this.lblRoiHeight.Name = "lblRoiHeight";
            this.lblRoiHeight.Size = new System.Drawing.Size(72, 13);
            this.lblRoiHeight.TabIndex = 983;
            this.lblRoiHeight.Text = "0.0000 mm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(262, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 982;
            this.label2.Text = "ROI Height";
            // 
            // TmrTeach
            // 
            this.TmrTeach.Tick += new System.EventHandler(this.TmrTeach_Tick);
            // 
            // lblSearchResult
            // 
            this.lblSearchResult.AutoSize = true;
            this.lblSearchResult.Location = new System.Drawing.Point(17, 160);
            this.lblSearchResult.Name = "lblSearchResult";
            this.lblSearchResult.Size = new System.Drawing.Size(23, 12);
            this.lblSearchResult.TabIndex = 2;
            this.lblSearchResult.Text = "---";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.picPatternMarkA);
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panel1.Location = new System.Drawing.Point(14, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 172);
            this.panel1.TabIndex = 954;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.picPatternMarkB);
            this.panel2.Location = new System.Drawing.Point(249, 18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(174, 172);
            this.panel2.TabIndex = 955;
            // 
            // FormMacroAlignTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 880);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.pnlStageJog);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.picVision);
            this.Name = "FormMacroAlignTeach";
            this.Text = "FormMacroAlignTeach";
            this.Load += new System.EventHandler(this.FormMacroAlignTeach_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPatternMarkB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPatternMarkA)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picPatternMarkB;
        private System.Windows.Forms.PictureBox picPatternMarkA;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnSearchMarkB;
        private System.Windows.Forms.Button btnSearchMarkA;
        private System.Windows.Forms.Button btnRegisterMarkB;
        private System.Windows.Forms.Button btnRegisterMarkA;
        private System.Windows.Forms.Panel picVision;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Panel pnlStageJog;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSelectStageMove;
        private System.Windows.Forms.Button btnSelectFocus;
        private System.Windows.Forms.Button btnChangeCam;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblCamPosZ;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblStagePosT;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblStagePosY;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblStagePosX;
        private System.Windows.Forms.Label lblRoiHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRoiWidth;
        private System.Windows.Forms.Button btnRoiHeightNarrow;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnRoiHeightWide;
        private System.Windows.Forms.Button btnRoiWidthNarrow;
        private System.Windows.Forms.Button btnRoiWidthWide;
        private System.Windows.Forms.Timer TmrTeach;
        private System.Windows.Forms.Label lblSearchResult;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}