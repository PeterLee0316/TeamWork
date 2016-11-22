namespace LWDicer.UI
{
    partial class FormWaferDieTeach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWaferDieTeach));
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSelectStageMove = new System.Windows.Forms.Button();
            this.btnSelectFocus = new System.Windows.Forms.Button();
            this.btnChangeCam = new System.Windows.Forms.Button();
            this.btnThetaAlign = new System.Windows.Forms.Button();
            this.picVision = new System.Windows.Forms.Panel();
            this.pnlStageJog = new System.Windows.Forms.Panel();
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.lblDieIndexCals = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.btnCalsDieIndexSize = new System.Windows.Forms.Button();
            this.lblDieIndexNum = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.lblDieIndexSet2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.lblDieIndexSet1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.btnDieIndexSet2 = new System.Windows.Forms.Button();
            this.btnDieIndexSet1 = new System.Windows.Forms.Button();
            this.btnDieIndexSelect = new System.Windows.Forms.Label();
            this.btnDieIndexDataSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.gradientLabel26 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel27 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.lblDieIndexAxisY = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel29 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel30 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.lblDieIndexAxisX = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            this.gradientLabel1.Location = new System.Drawing.Point(1, 1);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(802, 51);
            this.gradientLabel1.TabIndex = 938;
            this.gradientLabel1.Text = "Wafer Index Teach";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSelectStageMove);
            this.groupBox4.Controls.Add(this.btnSelectFocus);
            this.groupBox4.Controls.Add(this.btnChangeCam);
            this.groupBox4.Controls.Add(this.btnThetaAlign);
            this.groupBox4.Location = new System.Drawing.Point(809, 661);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(183, 216);
            this.groupBox4.TabIndex = 940;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Vision && Stage Control";
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
            // btnThetaAlign
            // 
            this.btnThetaAlign.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnThetaAlign.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnThetaAlign.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnThetaAlign.Location = new System.Drawing.Point(93, 36);
            this.btnThetaAlign.Name = "btnThetaAlign";
            this.btnThetaAlign.Size = new System.Drawing.Size(75, 77);
            this.btnThetaAlign.TabIndex = 765;
            this.btnThetaAlign.Text = "Theta Align";
            this.btnThetaAlign.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnThetaAlign.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnThetaAlign.UseVisualStyleBackColor = true;
            this.btnThetaAlign.Click += new System.EventHandler(this.btnThetaAlign_Click);
            // 
            // picVision
            // 
            this.picVision.Location = new System.Drawing.Point(1, 55);
            this.picVision.Name = "picVision";
            this.picVision.Size = new System.Drawing.Size(802, 600);
            this.picVision.TabIndex = 939;
            this.picVision.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picVision_MouseDown);
            // 
            // pnlStageJog
            // 
            this.pnlStageJog.Location = new System.Drawing.Point(1, 661);
            this.pnlStageJog.Name = "pnlStageJog";
            this.pnlStageJog.Size = new System.Drawing.Size(802, 218);
            this.pnlStageJog.TabIndex = 948;
            // 
            // TimerUI
            // 
            this.TimerUI.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.lblCamPosZ);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.lblStagePosT);
            this.groupBox2.Controls.Add(this.lblStagePosY);
            this.groupBox2.Controls.Add(this.lblStagePosX);
            this.groupBox2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(809, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(439, 120);
            this.groupBox2.TabIndex = 941;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stage Position";
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
            // lblDieIndexCals
            // 
            this.lblDieIndexCals.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.lblDieIndexCals.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblDieIndexCals.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDieIndexCals.ForeColor = System.Drawing.Color.Black;
            this.lblDieIndexCals.Location = new System.Drawing.Point(156, 258);
            this.lblDieIndexCals.Name = "lblDieIndexCals";
            this.lblDieIndexCals.Size = new System.Drawing.Size(109, 31);
            this.lblDieIndexCals.TabIndex = 969;
            this.lblDieIndexCals.Text = "0";
            this.lblDieIndexCals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCalsDieIndexSize
            // 
            this.btnCalsDieIndexSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnCalsDieIndexSize.Location = new System.Drawing.Point(57, 256);
            this.btnCalsDieIndexSize.Name = "btnCalsDieIndexSize";
            this.btnCalsDieIndexSize.Size = new System.Drawing.Size(98, 33);
            this.btnCalsDieIndexSize.TabIndex = 968;
            this.btnCalsDieIndexSize.Text = "Cals";
            this.btnCalsDieIndexSize.UseVisualStyleBackColor = false;
            this.btnCalsDieIndexSize.Click += new System.EventHandler(this.btnCalsDieIndexSize_Click);
            // 
            // lblDieIndexNum
            // 
            this.lblDieIndexNum.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.lblDieIndexNum.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblDieIndexNum.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDieIndexNum.ForeColor = System.Drawing.Color.Black;
            this.lblDieIndexNum.Location = new System.Drawing.Point(156, 225);
            this.lblDieIndexNum.Name = "lblDieIndexNum";
            this.lblDieIndexNum.Size = new System.Drawing.Size(109, 31);
            this.lblDieIndexNum.TabIndex = 967;
            this.lblDieIndexNum.Text = "1";
            this.lblDieIndexNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDieIndexNum.Click += new System.EventHandler(this.ChangeTextData);
            // 
            // lblDieIndexSet2
            // 
            this.lblDieIndexSet2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.lblDieIndexSet2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblDieIndexSet2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDieIndexSet2.ForeColor = System.Drawing.Color.Black;
            this.lblDieIndexSet2.Location = new System.Drawing.Point(156, 193);
            this.lblDieIndexSet2.Name = "lblDieIndexSet2";
            this.lblDieIndexSet2.Size = new System.Drawing.Size(109, 31);
            this.lblDieIndexSet2.TabIndex = 966;
            this.lblDieIndexSet2.Text = "0";
            this.lblDieIndexSet2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDieIndexSet1
            // 
            this.lblDieIndexSet1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.lblDieIndexSet1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblDieIndexSet1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDieIndexSet1.ForeColor = System.Drawing.Color.Black;
            this.lblDieIndexSet1.Location = new System.Drawing.Point(156, 160);
            this.lblDieIndexSet1.Name = "lblDieIndexSet1";
            this.lblDieIndexSet1.Size = new System.Drawing.Size(109, 31);
            this.lblDieIndexSet1.TabIndex = 965;
            this.lblDieIndexSet1.Text = "0";
            this.lblDieIndexSet1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(57, 223);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 33);
            this.button2.TabIndex = 964;
            this.button2.Text = "Set Num";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnDieIndexSet2
            // 
            this.btnDieIndexSet2.Location = new System.Drawing.Point(57, 191);
            this.btnDieIndexSet2.Name = "btnDieIndexSet2";
            this.btnDieIndexSet2.Size = new System.Drawing.Size(98, 33);
            this.btnDieIndexSet2.TabIndex = 963;
            this.btnDieIndexSet2.Text = "Set 2";
            this.btnDieIndexSet2.UseVisualStyleBackColor = true;
            this.btnDieIndexSet2.Click += new System.EventHandler(this.btnDieIndexSet2_Click);
            // 
            // btnDieIndexSet1
            // 
            this.btnDieIndexSet1.Location = new System.Drawing.Point(57, 159);
            this.btnDieIndexSet1.Name = "btnDieIndexSet1";
            this.btnDieIndexSet1.Size = new System.Drawing.Size(98, 33);
            this.btnDieIndexSet1.TabIndex = 962;
            this.btnDieIndexSet1.Text = "Set 1";
            this.btnDieIndexSet1.UseVisualStyleBackColor = true;
            this.btnDieIndexSet1.Click += new System.EventHandler(this.btnDieIndexSet1_Click);
            // 
            // btnDieIndexSelect
            // 
            this.btnDieIndexSelect.AutoSize = true;
            this.btnDieIndexSelect.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDieIndexSelect.Location = new System.Drawing.Point(19, 131);
            this.btnDieIndexSelect.Name = "btnDieIndexSelect";
            this.btnDieIndexSelect.Size = new System.Drawing.Size(100, 15);
            this.btnDieIndexSelect.TabIndex = 961;
            this.btnDieIndexSelect.Text = "Position Set";
            // 
            // btnDieIndexDataSave
            // 
            this.btnDieIndexDataSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDieIndexDataSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnDieIndexDataSave.Image = ((System.Drawing.Image)(resources.GetObject("btnDieIndexDataSave.Image")));
            this.btnDieIndexDataSave.Location = new System.Drawing.Point(135, 309);
            this.btnDieIndexDataSave.Name = "btnDieIndexDataSave";
            this.btnDieIndexDataSave.Size = new System.Drawing.Size(130, 61);
            this.btnDieIndexDataSave.TabIndex = 950;
            this.btnDieIndexDataSave.Text = "Data Save";
            this.btnDieIndexDataSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDieIndexDataSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDieIndexDataSave.UseVisualStyleBackColor = true;
            this.btnDieIndexDataSave.Click += new System.EventHandler(this.btnDieIndexDataSave_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(19, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 15);
            this.label6.TabIndex = 892;
            this.label6.Text = "Die Index Size";
            // 
            // gradientLabel26
            // 
            this.gradientLabel26.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.LightBlue);
            this.gradientLabel26.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel26.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientLabel26.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel26.ForeColor = System.Drawing.Color.Black;
            this.gradientLabel26.Location = new System.Drawing.Point(225, 77);
            this.gradientLabel26.Name = "gradientLabel26";
            this.gradientLabel26.Size = new System.Drawing.Size(40, 31);
            this.gradientLabel26.TabIndex = 891;
            this.gradientLabel26.Text = "mm";
            this.gradientLabel26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel27
            // 
            this.gradientLabel27.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.LightBlue);
            this.gradientLabel27.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel27.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientLabel27.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel27.ForeColor = System.Drawing.Color.Black;
            this.gradientLabel27.Location = new System.Drawing.Point(57, 77);
            this.gradientLabel27.Name = "gradientLabel27";
            this.gradientLabel27.Size = new System.Drawing.Size(90, 31);
            this.gradientLabel27.TabIndex = 890;
            this.gradientLabel27.Text = "Y Axis";
            this.gradientLabel27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDieIndexAxisY
            // 
            this.lblDieIndexAxisY.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.lblDieIndexAxisY.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblDieIndexAxisY.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDieIndexAxisY.ForeColor = System.Drawing.Color.Black;
            this.lblDieIndexAxisY.Location = new System.Drawing.Point(148, 77);
            this.lblDieIndexAxisY.Name = "lblDieIndexAxisY";
            this.lblDieIndexAxisY.Size = new System.Drawing.Size(77, 31);
            this.lblDieIndexAxisY.TabIndex = 889;
            this.lblDieIndexAxisY.Text = "0";
            this.lblDieIndexAxisY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDieIndexAxisY.Click += new System.EventHandler(this.ChangeTextData);
            // 
            // gradientLabel29
            // 
            this.gradientLabel29.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.LightBlue);
            this.gradientLabel29.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel29.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientLabel29.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel29.ForeColor = System.Drawing.Color.Black;
            this.gradientLabel29.Location = new System.Drawing.Point(225, 46);
            this.gradientLabel29.Name = "gradientLabel29";
            this.gradientLabel29.Size = new System.Drawing.Size(40, 31);
            this.gradientLabel29.TabIndex = 888;
            this.gradientLabel29.Text = "mm";
            this.gradientLabel29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel30
            // 
            this.gradientLabel30.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.LightBlue);
            this.gradientLabel30.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel30.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientLabel30.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel30.ForeColor = System.Drawing.Color.Black;
            this.gradientLabel30.Location = new System.Drawing.Point(57, 46);
            this.gradientLabel30.Name = "gradientLabel30";
            this.gradientLabel30.Size = new System.Drawing.Size(90, 31);
            this.gradientLabel30.TabIndex = 887;
            this.gradientLabel30.Text = "X Axis";
            this.gradientLabel30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDieIndexAxisX
            // 
            this.lblDieIndexAxisX.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.lblDieIndexAxisX.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblDieIndexAxisX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDieIndexAxisX.ForeColor = System.Drawing.Color.Black;
            this.lblDieIndexAxisX.Location = new System.Drawing.Point(148, 46);
            this.lblDieIndexAxisX.Name = "lblDieIndexAxisX";
            this.lblDieIndexAxisX.Size = new System.Drawing.Size(77, 31);
            this.lblDieIndexAxisX.TabIndex = 886;
            this.lblDieIndexAxisX.Text = "0";
            this.lblDieIndexAxisX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDieIndexAxisX.Click += new System.EventHandler(this.ChangeTextData);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(1103, 807);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(130, 61);
            this.BtnExit.TabIndex = 937;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDieIndexCals);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.gradientLabel27);
            this.groupBox1.Controls.Add(this.btnCalsDieIndexSize);
            this.groupBox1.Controls.Add(this.lblDieIndexAxisY);
            this.groupBox1.Controls.Add(this.gradientLabel26);
            this.groupBox1.Controls.Add(this.lblDieIndexNum);
            this.groupBox1.Controls.Add(this.gradientLabel29);
            this.groupBox1.Controls.Add(this.gradientLabel30);
            this.groupBox1.Controls.Add(this.lblDieIndexSet2);
            this.groupBox1.Controls.Add(this.btnDieIndexDataSave);
            this.groupBox1.Controls.Add(this.lblDieIndexAxisX);
            this.groupBox1.Controls.Add(this.lblDieIndexSet1);
            this.groupBox1.Controls.Add(this.btnDieIndexSelect);
            this.groupBox1.Controls.Add(this.btnDieIndexSet1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.btnDieIndexSet2);
            this.groupBox1.Location = new System.Drawing.Point(809, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 391);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wafer Index";
            // 
            // FormWaferDieTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 880);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pnlStageJog);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.picVision);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.BtnExit);
            this.Name = "FormWaferDieTeach";
            this.Text = "Laser Align";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormThetaAlignTeach_FormClosing);
            this.Load += new System.EventHandler(this.FormThetaAlignTeach_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnChangeCam;
        private System.Windows.Forms.Panel picVision;
        private System.Windows.Forms.Panel pnlStageJog;
        private System.Windows.Forms.Button btnThetaAlign;
        private System.Windows.Forms.Timer TimerUI;
        private System.Windows.Forms.GroupBox groupBox2;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblStagePosT;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblStagePosY;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblStagePosX;
        private System.Windows.Forms.Label label6;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel26;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel27;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblDieIndexAxisY;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel29;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel30;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblDieIndexAxisX;
        private System.Windows.Forms.Button btnSelectFocus;
        private System.Windows.Forms.Button btnSelectStageMove;
        private System.Windows.Forms.Button btnDieIndexDataSave;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblCamPosZ;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label btnDieIndexSelect;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblDieIndexNum;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblDieIndexSet2;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblDieIndexSet1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnDieIndexSet2;
        private System.Windows.Forms.Button btnDieIndexSet1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblDieIndexCals;
        private System.Windows.Forms.Button btnCalsDieIndexSize;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}