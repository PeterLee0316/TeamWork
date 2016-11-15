namespace LWDicer.UI
{
    partial class FormEdgeAlignTeach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEdgeAlignTeach));
            this.gradientLabel16 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSetEdgeDetectArea = new System.Windows.Forms.Button();
            this.btnSearchEdgePoint = new System.Windows.Forms.Button();
            this.btnEdgeTeachNext = new System.Windows.Forms.Button();
            this.btnEdgeTeachPos3 = new System.Windows.Forms.Button();
            this.btnEdgeTeachPos2 = new System.Windows.Forms.Button();
            this.btnEdgeTeachPos1 = new System.Windows.Forms.Button();
            this.btnEdgeAlignDataSave = new System.Windows.Forms.Button();
            this.btnStageCenterPre = new System.Windows.Forms.Button();
            this.picVision = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.pnlStageJog = new System.Windows.Forms.Panel();
            this.gradientLabel2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnThetaAlign = new System.Windows.Forms.Button();
            this.btnSelectStageMove = new System.Windows.Forms.Button();
            this.btnSelectFocus = new System.Windows.Forms.Button();
            this.btnChangeCam = new System.Windows.Forms.Button();
            this.BtnJog = new System.Windows.Forms.Button();
            this.btnRotateCenter = new System.Windows.Forms.Button();
            this.btnRotateCenterCalsInit = new System.Windows.Forms.Button();
            this.btnEdgePos3 = new System.Windows.Forms.Button();
            this.btnEdgePos2 = new System.Windows.Forms.Button();
            this.btnEdgePos1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnEdgePos4 = new System.Windows.Forms.Button();
            this.btnWaferCenterSearchRun = new System.Windows.Forms.Button();
            this.btnWaferCenterPre = new System.Windows.Forms.Button();
            this.btnWaferCenterFine = new System.Windows.Forms.Button();
            this.btnStageCenterFine = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnInpectCam = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
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
            this.gradientLabel16.Location = new System.Drawing.Point(-248, -177);
            this.gradientLabel16.Name = "gradientLabel16";
            this.gradientLabel16.Size = new System.Drawing.Size(594, 38);
            this.gradientLabel16.TabIndex = 933;
            this.gradientLabel16.Text = "Camera Teaching Table";
            this.gradientLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSetEdgeDetectArea);
            this.groupBox4.Controls.Add(this.btnSearchEdgePoint);
            this.groupBox4.Controls.Add(this.btnEdgeTeachNext);
            this.groupBox4.Controls.Add(this.btnEdgeTeachPos3);
            this.groupBox4.Controls.Add(this.btnEdgeTeachPos2);
            this.groupBox4.Controls.Add(this.btnEdgeTeachPos1);
            this.groupBox4.Location = new System.Drawing.Point(820, 125);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(438, 156);
            this.groupBox4.TabIndex = 944;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Manual Control";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // btnSetEdgeDetectArea
            // 
            this.btnSetEdgeDetectArea.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSetEdgeDetectArea.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSetEdgeDetectArea.Location = new System.Drawing.Point(272, 87);
            this.btnSetEdgeDetectArea.Name = "btnSetEdgeDetectArea";
            this.btnSetEdgeDetectArea.Size = new System.Drawing.Size(74, 61);
            this.btnSetEdgeDetectArea.TabIndex = 983;
            this.btnSetEdgeDetectArea.Text = "Set Edge ROI";
            this.btnSetEdgeDetectArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSetEdgeDetectArea.UseVisualStyleBackColor = true;
            this.btnSetEdgeDetectArea.Click += new System.EventHandler(this.btnSetEdgeDetectArea_Click);
            // 
            // btnSearchEdgePoint
            // 
            this.btnSearchEdgePoint.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchEdgePoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSearchEdgePoint.Location = new System.Drawing.Point(197, 87);
            this.btnSearchEdgePoint.Name = "btnSearchEdgePoint";
            this.btnSearchEdgePoint.Size = new System.Drawing.Size(74, 61);
            this.btnSearchEdgePoint.TabIndex = 982;
            this.btnSearchEdgePoint.Text = "Edge Search";
            this.btnSearchEdgePoint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearchEdgePoint.UseVisualStyleBackColor = true;
            this.btnSearchEdgePoint.Click += new System.EventHandler(this.btnSearchEdgePoint_Click);
            // 
            // btnEdgeTeachNext
            // 
            this.btnEdgeTeachNext.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdgeTeachNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEdgeTeachNext.Location = new System.Drawing.Point(6, 20);
            this.btnEdgeTeachNext.Name = "btnEdgeTeachNext";
            this.btnEdgeTeachNext.Size = new System.Drawing.Size(98, 61);
            this.btnEdgeTeachNext.TabIndex = 977;
            this.btnEdgeTeachNext.Text = "Edge Teach";
            this.btnEdgeTeachNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdgeTeachNext.UseVisualStyleBackColor = true;
            this.btnEdgeTeachNext.Click += new System.EventHandler(this.btnEdgeTeachNext_Click);
            // 
            // btnEdgeTeachPos3
            // 
            this.btnEdgeTeachPos3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdgeTeachPos3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEdgeTeachPos3.Location = new System.Drawing.Point(348, 20);
            this.btnEdgeTeachPos3.Name = "btnEdgeTeachPos3";
            this.btnEdgeTeachPos3.Size = new System.Drawing.Size(74, 61);
            this.btnEdgeTeachPos3.TabIndex = 980;
            this.btnEdgeTeachPos3.Text = "Edge Teach Position3";
            this.btnEdgeTeachPos3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdgeTeachPos3.UseVisualStyleBackColor = true;
            this.btnEdgeTeachPos3.Click += new System.EventHandler(this.btnEdgeTeachPos3_Click);
            // 
            // btnEdgeTeachPos2
            // 
            this.btnEdgeTeachPos2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdgeTeachPos2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEdgeTeachPos2.Location = new System.Drawing.Point(272, 20);
            this.btnEdgeTeachPos2.Name = "btnEdgeTeachPos2";
            this.btnEdgeTeachPos2.Size = new System.Drawing.Size(74, 61);
            this.btnEdgeTeachPos2.TabIndex = 979;
            this.btnEdgeTeachPos2.Text = "Edge Teach Position2";
            this.btnEdgeTeachPos2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdgeTeachPos2.UseVisualStyleBackColor = true;
            this.btnEdgeTeachPos2.Click += new System.EventHandler(this.btnEdgeTeachPos2_Click);
            // 
            // btnEdgeTeachPos1
            // 
            this.btnEdgeTeachPos1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdgeTeachPos1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEdgeTeachPos1.Location = new System.Drawing.Point(197, 20);
            this.btnEdgeTeachPos1.Name = "btnEdgeTeachPos1";
            this.btnEdgeTeachPos1.Size = new System.Drawing.Size(74, 61);
            this.btnEdgeTeachPos1.TabIndex = 977;
            this.btnEdgeTeachPos1.Text = "Edge Teach Position1";
            this.btnEdgeTeachPos1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdgeTeachPos1.UseVisualStyleBackColor = true;
            this.btnEdgeTeachPos1.Click += new System.EventHandler(this.btnEdgeTeachPos1_Click);
            // 
            // btnEdgeAlignDataSave
            // 
            this.btnEdgeAlignDataSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdgeAlignDataSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEdgeAlignDataSave.Image = ((System.Drawing.Image)(resources.GetObject("btnEdgeAlignDataSave.Image")));
            this.btnEdgeAlignDataSave.Location = new System.Drawing.Point(1154, 543);
            this.btnEdgeAlignDataSave.Name = "btnEdgeAlignDataSave";
            this.btnEdgeAlignDataSave.Size = new System.Drawing.Size(100, 61);
            this.btnEdgeAlignDataSave.TabIndex = 978;
            this.btnEdgeAlignDataSave.Text = "Data Save";
            this.btnEdgeAlignDataSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdgeAlignDataSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdgeAlignDataSave.UseVisualStyleBackColor = true;
            this.btnEdgeAlignDataSave.Click += new System.EventHandler(this.btnEdgeAlignDataSave_Click);
            // 
            // btnStageCenterPre
            // 
            this.btnStageCenterPre.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStageCenterPre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnStageCenterPre.Location = new System.Drawing.Point(353, 16);
            this.btnStageCenterPre.Name = "btnStageCenterPre";
            this.btnStageCenterPre.Size = new System.Drawing.Size(74, 61);
            this.btnStageCenterPre.TabIndex = 985;
            this.btnStageCenterPre.Text = "Stage Center (Pre)";
            this.btnStageCenterPre.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStageCenterPre.UseVisualStyleBackColor = true;
            this.btnStageCenterPre.Click += new System.EventHandler(this.btnStageCenterPre_Click);
            // 
            // picVision
            // 
            this.picVision.Location = new System.Drawing.Point(11, 53);
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
            this.BtnExit.Location = new System.Drawing.Point(1124, 807);
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
            this.pnlStageJog.Location = new System.Drawing.Point(12, 658);
            this.pnlStageJog.Name = "pnlStageJog";
            this.pnlStageJog.Size = new System.Drawing.Size(797, 218);
            this.pnlStageJog.TabIndex = 947;
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel2.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel2.Location = new System.Drawing.Point(12, -1);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(802, 51);
            this.gradientLabel2.TabIndex = 948;
            this.gradientLabel2.Text = "Edge Align";
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.lblCamPosZ);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lblStagePosT);
            this.groupBox1.Controls.Add(this.lblStagePosY);
            this.groupBox1.Controls.Add(this.lblStagePosX);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(819, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 120);
            this.groupBox1.TabIndex = 950;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stage Position";
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
            // TimerUI
            // 
            this.TimerUI.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnThetaAlign);
            this.groupBox2.Controls.Add(this.btnSelectStageMove);
            this.groupBox2.Controls.Add(this.btnSelectFocus);
            this.groupBox2.Controls.Add(this.btnChangeCam);
            this.groupBox2.Location = new System.Drawing.Point(820, 658);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 216);
            this.groupBox2.TabIndex = 951;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vision && Stage Control";
            // 
            // btnThetaAlign
            // 
            this.btnThetaAlign.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnThetaAlign.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnThetaAlign.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnThetaAlign.Location = new System.Drawing.Point(93, 36);
            this.btnThetaAlign.Name = "btnThetaAlign";
            this.btnThetaAlign.Size = new System.Drawing.Size(75, 77);
            this.btnThetaAlign.TabIndex = 766;
            this.btnThetaAlign.Text = "Theta Align";
            this.btnThetaAlign.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnThetaAlign.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnThetaAlign.UseVisualStyleBackColor = true;
            this.btnThetaAlign.Click += new System.EventHandler(this.btnThetaAlign_Click);
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
            // BtnJog
            // 
            this.BtnJog.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnJog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnJog.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnJog.Location = new System.Drawing.Point(1124, 740);
            this.BtnJog.Name = "BtnJog";
            this.BtnJog.Size = new System.Drawing.Size(124, 61);
            this.BtnJog.TabIndex = 980;
            this.BtnJog.Text = "Jog";
            this.BtnJog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnJog.UseVisualStyleBackColor = true;
            this.BtnJog.Click += new System.EventHandler(this.BtnJog_Click);
            // 
            // btnRotateCenter
            // 
            this.btnRotateCenter.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRotateCenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRotateCenter.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRotateCenter.Location = new System.Drawing.Point(16, 83);
            this.btnRotateCenter.Name = "btnRotateCenter";
            this.btnRotateCenter.Size = new System.Drawing.Size(74, 61);
            this.btnRotateCenter.TabIndex = 981;
            this.btnRotateCenter.Text = "Rotate Center Cals";
            this.btnRotateCenter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRotateCenter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRotateCenter.UseVisualStyleBackColor = true;
            this.btnRotateCenter.Click += new System.EventHandler(this.btnRotateCenter_Click);
            // 
            // btnRotateCenterCalsInit
            // 
            this.btnRotateCenterCalsInit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRotateCenterCalsInit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRotateCenterCalsInit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRotateCenterCalsInit.Location = new System.Drawing.Point(16, 21);
            this.btnRotateCenterCalsInit.Name = "btnRotateCenterCalsInit";
            this.btnRotateCenterCalsInit.Size = new System.Drawing.Size(74, 61);
            this.btnRotateCenterCalsInit.TabIndex = 982;
            this.btnRotateCenterCalsInit.Text = "Rotate Cals Init";
            this.btnRotateCenterCalsInit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRotateCenterCalsInit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRotateCenterCalsInit.UseVisualStyleBackColor = true;
            this.btnRotateCenterCalsInit.Click += new System.EventHandler(this.btnRotateCenterCalsInit_Click);
            // 
            // btnEdgePos3
            // 
            this.btnEdgePos3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdgePos3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEdgePos3.Location = new System.Drawing.Point(274, 20);
            this.btnEdgePos3.Name = "btnEdgePos3";
            this.btnEdgePos3.Size = new System.Drawing.Size(74, 61);
            this.btnEdgePos3.TabIndex = 986;
            this.btnEdgePos3.Text = "Edge Position3";
            this.btnEdgePos3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdgePos3.UseVisualStyleBackColor = true;
            this.btnEdgePos3.Click += new System.EventHandler(this.btnEdgePos3_Click);
            // 
            // btnEdgePos2
            // 
            this.btnEdgePos2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdgePos2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEdgePos2.Location = new System.Drawing.Point(198, 20);
            this.btnEdgePos2.Name = "btnEdgePos2";
            this.btnEdgePos2.Size = new System.Drawing.Size(74, 61);
            this.btnEdgePos2.TabIndex = 985;
            this.btnEdgePos2.Text = "Edge Position2";
            this.btnEdgePos2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdgePos2.UseVisualStyleBackColor = true;
            this.btnEdgePos2.Click += new System.EventHandler(this.btnEdgePos2_Click);
            // 
            // btnEdgePos1
            // 
            this.btnEdgePos1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdgePos1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEdgePos1.Location = new System.Drawing.Point(123, 20);
            this.btnEdgePos1.Name = "btnEdgePos1";
            this.btnEdgePos1.Size = new System.Drawing.Size(74, 61);
            this.btnEdgePos1.TabIndex = 984;
            this.btnEdgePos1.Text = "Edge Position1";
            this.btnEdgePos1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdgePos1.UseVisualStyleBackColor = true;
            this.btnEdgePos1.Click += new System.EventHandler(this.btnEdgePos1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnEdgePos4);
            this.groupBox3.Controls.Add(this.btnWaferCenterSearchRun);
            this.groupBox3.Controls.Add(this.btnEdgePos1);
            this.groupBox3.Controls.Add(this.btnEdgePos3);
            this.groupBox3.Controls.Add(this.btnEdgePos2);
            this.groupBox3.Location = new System.Drawing.Point(819, 287);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(438, 94);
            this.groupBox3.TabIndex = 952;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Edge Search";
            // 
            // btnEdgePos4
            // 
            this.btnEdgePos4.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEdgePos4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEdgePos4.Location = new System.Drawing.Point(350, 20);
            this.btnEdgePos4.Name = "btnEdgePos4";
            this.btnEdgePos4.Size = new System.Drawing.Size(74, 61);
            this.btnEdgePos4.TabIndex = 988;
            this.btnEdgePos4.Text = "Edge Position4";
            this.btnEdgePos4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdgePos4.UseVisualStyleBackColor = true;
            this.btnEdgePos4.Click += new System.EventHandler(this.btnEdgePos4_Click);
            // 
            // btnWaferCenterSearchRun
            // 
            this.btnWaferCenterSearchRun.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWaferCenterSearchRun.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnWaferCenterSearchRun.Location = new System.Drawing.Point(11, 20);
            this.btnWaferCenterSearchRun.Name = "btnWaferCenterSearchRun";
            this.btnWaferCenterSearchRun.Size = new System.Drawing.Size(98, 61);
            this.btnWaferCenterSearchRun.TabIndex = 987;
            this.btnWaferCenterSearchRun.Text = "Search Run";
            this.btnWaferCenterSearchRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnWaferCenterSearchRun.UseVisualStyleBackColor = true;
            this.btnWaferCenterSearchRun.Click += new System.EventHandler(this.btnWaferCenterSearchRun_Click);
            // 
            // btnWaferCenterPre
            // 
            this.btnWaferCenterPre.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWaferCenterPre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnWaferCenterPre.Location = new System.Drawing.Point(278, 16);
            this.btnWaferCenterPre.Name = "btnWaferCenterPre";
            this.btnWaferCenterPre.Size = new System.Drawing.Size(74, 61);
            this.btnWaferCenterPre.TabIndex = 983;
            this.btnWaferCenterPre.Text = "Wafer Center (Pre)";
            this.btnWaferCenterPre.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnWaferCenterPre.UseVisualStyleBackColor = true;
            this.btnWaferCenterPre.Click += new System.EventHandler(this.btnWaferCenterPre_Click);
            // 
            // btnWaferCenterFine
            // 
            this.btnWaferCenterFine.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWaferCenterFine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnWaferCenterFine.Location = new System.Drawing.Point(279, 83);
            this.btnWaferCenterFine.Name = "btnWaferCenterFine";
            this.btnWaferCenterFine.Size = new System.Drawing.Size(74, 61);
            this.btnWaferCenterFine.TabIndex = 984;
            this.btnWaferCenterFine.Text = "Wafer Center (Fine)";
            this.btnWaferCenterFine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnWaferCenterFine.UseVisualStyleBackColor = true;
            this.btnWaferCenterFine.Click += new System.EventHandler(this.btnWaferCenterFine_Click);
            // 
            // btnStageCenterFine
            // 
            this.btnStageCenterFine.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStageCenterFine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnStageCenterFine.Location = new System.Drawing.Point(354, 83);
            this.btnStageCenterFine.Name = "btnStageCenterFine";
            this.btnStageCenterFine.Size = new System.Drawing.Size(74, 61);
            this.btnStageCenterFine.TabIndex = 986;
            this.btnStageCenterFine.Text = "Stage Center (Fine)";
            this.btnStageCenterFine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStageCenterFine.UseVisualStyleBackColor = true;
            this.btnStageCenterFine.Click += new System.EventHandler(this.btnStageCenterFine_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnRotateCenterCalsInit);
            this.groupBox5.Controls.Add(this.btnStageCenterFine);
            this.groupBox5.Controls.Add(this.btnRotateCenter);
            this.groupBox5.Controls.Add(this.btnStageCenterPre);
            this.groupBox5.Controls.Add(this.btnWaferCenterPre);
            this.groupBox5.Controls.Add(this.btnWaferCenterFine);
            this.groupBox5.Location = new System.Drawing.Point(819, 387);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(438, 150);
            this.groupBox5.TabIndex = 987;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "State Rotate Center Search";
            // 
            // btnInpectCam
            // 
            this.btnInpectCam.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInpectCam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnInpectCam.Image = global::LWDicer.Properties.Resources.Vision;
            this.btnInpectCam.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnInpectCam.Location = new System.Drawing.Point(836, 543);
            this.btnInpectCam.Name = "btnInpectCam";
            this.btnInpectCam.Size = new System.Drawing.Size(73, 73);
            this.btnInpectCam.TabIndex = 987;
            this.btnInpectCam.Text = "Inspect";
            this.btnInpectCam.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnInpectCam.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnInpectCam.UseVisualStyleBackColor = true;
            this.btnInpectCam.Click += new System.EventHandler(this.btnInpectCam_Click);
            // 
            // FormEdgeAlignTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 880);
            this.Controls.Add(this.btnInpectCam);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.BtnJog);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gradientLabel2);
            this.Controls.Add(this.btnEdgeAlignDataSave);
            this.Controls.Add(this.pnlStageJog);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.picVision);
            this.Controls.Add(this.gradientLabel16);
            this.Name = "FormEdgeAlignTeach";
            this.Text = "FormEdgeAlignTeach";
            this.Load += new System.EventHandler(this.FormEdgeAlignTeach_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel16;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel picVision;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Panel pnlStageJog;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel2;
        private System.Windows.Forms.GroupBox groupBox1;
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
        private System.Windows.Forms.Timer TimerUI;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSelectStageMove;
        private System.Windows.Forms.Button btnSelectFocus;
        private System.Windows.Forms.Button btnChangeCam;
        private System.Windows.Forms.Button btnEdgeTeachPos1;
        private System.Windows.Forms.Button btnEdgeAlignDataSave;
        private System.Windows.Forms.Button btnEdgeTeachPos2;
        private System.Windows.Forms.Button btnEdgeTeachPos3;
        private System.Windows.Forms.Button btnSearchEdgePoint;
        private System.Windows.Forms.Button btnSetEdgeDetectArea;
        private System.Windows.Forms.Button btnEdgeTeachNext;
        private System.Windows.Forms.Button BtnJog;
        private System.Windows.Forms.Button btnRotateCenter;
        private System.Windows.Forms.Button btnRotateCenterCalsInit;
        private System.Windows.Forms.Button btnEdgePos3;
        private System.Windows.Forms.Button btnEdgePos2;
        private System.Windows.Forms.Button btnEdgePos1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnWaferCenterSearchRun;
        private System.Windows.Forms.Button btnWaferCenterPre;
        private System.Windows.Forms.Button btnWaferCenterFine;
        private System.Windows.Forms.Button btnEdgePos4;
        private System.Windows.Forms.Button btnStageCenterFine;
        private System.Windows.Forms.Button btnStageCenterPre;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnThetaAlign;
        private System.Windows.Forms.Button btnInpectCam;
    }
}