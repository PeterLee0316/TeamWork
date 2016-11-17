namespace LWDicer.UI
{
    partial class FormStageManualOP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStageManualOP));
            this.BtnExit = new System.Windows.Forms.Button();
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.gradientLabel3 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnVacuumOff = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnVacuumOn = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientLabel16 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnClampClose = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnClampOpen = new Syncfusion.Windows.Forms.ButtonAdv();
            this.LabelVACTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelClampTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel4 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.btnLaserProcessMof = new Syncfusion.Windows.Forms.ButtonAdv();
            this.btnLaserProcessStep1 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.lblProcessCount = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel5 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel6 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.lblProcessCountRead = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel7 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.lblProcessExpoBusy = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel9 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.lblProcessJobStart = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel8 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.btnProcessDataSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(740, 469);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 757;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // TimerUI
            // 
            this.TimerUI.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // gradientLabel3
            // 
            this.gradientLabel3.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel3.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel3.Location = new System.Drawing.Point(12, 9);
            this.gradientLabel3.Name = "gradientLabel3";
            this.gradientLabel3.Size = new System.Drawing.Size(261, 38);
            this.gradientLabel3.TabIndex = 957;
            this.gradientLabel3.Text = "Chuck Vacuum";
            this.gradientLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnVacuumOff
            // 
            this.BtnVacuumOff.BackColor = System.Drawing.Color.LightGray;
            this.BtnVacuumOff.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnVacuumOff.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnVacuumOff.FlatAppearance.BorderSize = 5;
            this.BtnVacuumOff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOff.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnVacuumOff.Location = new System.Drawing.Point(144, 48);
            this.BtnVacuumOff.Name = "BtnVacuumOff";
            this.BtnVacuumOff.Size = new System.Drawing.Size(130, 64);
            this.BtnVacuumOff.TabIndex = 956;
            this.BtnVacuumOff.Tag = "1";
            this.BtnVacuumOff.Text = "Off";
            this.BtnVacuumOff.Click += new System.EventHandler(this.BtnVacuumOff_Click);
            // 
            // BtnVacuumOn
            // 
            this.BtnVacuumOn.BackColor = System.Drawing.Color.LightGray;
            this.BtnVacuumOn.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnVacuumOn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnVacuumOn.FlatAppearance.BorderSize = 5;
            this.BtnVacuumOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnVacuumOn.Location = new System.Drawing.Point(12, 48);
            this.BtnVacuumOn.Name = "BtnVacuumOn";
            this.BtnVacuumOn.Size = new System.Drawing.Size(130, 64);
            this.BtnVacuumOn.TabIndex = 955;
            this.BtnVacuumOn.Tag = "0";
            this.BtnVacuumOn.Text = "On";
            this.BtnVacuumOn.Click += new System.EventHandler(this.BtnVacuumOn_Click);
            // 
            // gradientLabel16
            // 
            this.gradientLabel16.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel16.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel16.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel16.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel16.Location = new System.Drawing.Point(12, 152);
            this.gradientLabel16.Name = "gradientLabel16";
            this.gradientLabel16.Size = new System.Drawing.Size(261, 38);
            this.gradientLabel16.TabIndex = 950;
            this.gradientLabel16.Text = "Clamp Open/Close";
            this.gradientLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnClampClose
            // 
            this.BtnClampClose.BackColor = System.Drawing.Color.LightGray;
            this.BtnClampClose.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnClampClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnClampClose.FlatAppearance.BorderSize = 5;
            this.BtnClampClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnClampClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnClampClose.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnClampClose.Location = new System.Drawing.Point(144, 191);
            this.BtnClampClose.Name = "BtnClampClose";
            this.BtnClampClose.Size = new System.Drawing.Size(130, 64);
            this.BtnClampClose.TabIndex = 947;
            this.BtnClampClose.Tag = "1";
            this.BtnClampClose.Text = "Close";
            this.BtnClampClose.Click += new System.EventHandler(this.BtnClampClose_Click);
            // 
            // BtnClampOpen
            // 
            this.BtnClampOpen.BackColor = System.Drawing.Color.LightGray;
            this.BtnClampOpen.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnClampOpen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnClampOpen.FlatAppearance.BorderSize = 5;
            this.BtnClampOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnClampOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnClampOpen.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnClampOpen.Location = new System.Drawing.Point(12, 191);
            this.BtnClampOpen.Name = "BtnClampOpen";
            this.BtnClampOpen.Size = new System.Drawing.Size(130, 64);
            this.BtnClampOpen.TabIndex = 946;
            this.BtnClampOpen.Tag = "0";
            this.BtnClampOpen.Text = "Open";
            this.BtnClampOpen.Click += new System.EventHandler(this.BtnClampOpen_Click);
            // 
            // LabelVACTime
            // 
            this.LabelVACTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelVACTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelVACTime.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.LabelVACTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelVACTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LabelVACTime.Location = new System.Drawing.Point(144, 113);
            this.LabelVACTime.Name = "LabelVACTime";
            this.LabelVACTime.Size = new System.Drawing.Size(130, 30);
            this.LabelVACTime.TabIndex = 959;
            this.LabelVACTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.Silver, System.Drawing.Color.Maroon);
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel1.Location = new System.Drawing.Point(12, 113);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(130, 30);
            this.gradientLabel1.TabIndex = 958;
            this.gradientLabel1.Text = "수행 시간";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelClampTime
            // 
            this.LabelClampTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelClampTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelClampTime.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.LabelClampTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelClampTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LabelClampTime.Location = new System.Drawing.Point(144, 256);
            this.LabelClampTime.Name = "LabelClampTime";
            this.LabelClampTime.Size = new System.Drawing.Size(130, 30);
            this.LabelClampTime.TabIndex = 961;
            this.LabelClampTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel4
            // 
            this.gradientLabel4.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.Silver, System.Drawing.Color.Maroon);
            this.gradientLabel4.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel4.Location = new System.Drawing.Point(12, 256);
            this.gradientLabel4.Name = "gradientLabel4";
            this.gradientLabel4.Size = new System.Drawing.Size(130, 30);
            this.gradientLabel4.TabIndex = 960;
            this.gradientLabel4.Text = "수행 시간";
            this.gradientLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel2.Location = new System.Drawing.Point(311, 363);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(267, 38);
            this.gradientLabel2.TabIndex = 963;
            this.gradientLabel2.Text = "Laser MOF Process";
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLaserProcessMof
            // 
            this.btnLaserProcessMof.BackColor = System.Drawing.Color.LightGray;
            this.btnLaserProcessMof.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.btnLaserProcessMof.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnLaserProcessMof.FlatAppearance.BorderSize = 5;
            this.btnLaserProcessMof.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnLaserProcessMof.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnLaserProcessMof.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLaserProcessMof.Location = new System.Drawing.Point(311, 481);
            this.btnLaserProcessMof.Name = "btnLaserProcessMof";
            this.btnLaserProcessMof.Size = new System.Drawing.Size(267, 49);
            this.btnLaserProcessMof.TabIndex = 962;
            this.btnLaserProcessMof.Tag = "0";
            this.btnLaserProcessMof.Text = "Run";
            this.btnLaserProcessMof.Click += new System.EventHandler(this.btnLaserProcessMof_Click);
            // 
            // btnLaserProcessStep1
            // 
            this.btnLaserProcessStep1.BackColor = System.Drawing.Color.LightGray;
            this.btnLaserProcessStep1.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.btnLaserProcessStep1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnLaserProcessStep1.FlatAppearance.BorderSize = 5;
            this.btnLaserProcessStep1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnLaserProcessStep1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnLaserProcessStep1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLaserProcessStep1.Location = new System.Drawing.Point(311, 288);
            this.btnLaserProcessStep1.Name = "btnLaserProcessStep1";
            this.btnLaserProcessStep1.Size = new System.Drawing.Size(267, 49);
            this.btnLaserProcessStep1.TabIndex = 964;
            this.btnLaserProcessStep1.Tag = "0";
            this.btnLaserProcessStep1.Text = "Run ( Mode 1 )";
            this.btnLaserProcessStep1.Click += new System.EventHandler(this.btnLaserProcessStep1_Click);
            // 
            // lblProcessCount
            // 
            this.lblProcessCount.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.lblProcessCount.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblProcessCount.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.lblProcessCount.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProcessCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblProcessCount.Location = new System.Drawing.Point(503, 402);
            this.lblProcessCount.Name = "lblProcessCount";
            this.lblProcessCount.Size = new System.Drawing.Size(75, 38);
            this.lblProcessCount.TabIndex = 965;
            this.lblProcessCount.Text = "1";
            this.lblProcessCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProcessCount.Click += new System.EventHandler(this.lblProcessCount_Click);
            // 
            // gradientLabel5
            // 
            this.gradientLabel5.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel5.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel5.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel5.Location = new System.Drawing.Point(311, 402);
            this.gradientLabel5.Name = "gradientLabel5";
            this.gradientLabel5.Size = new System.Drawing.Size(192, 38);
            this.gradientLabel5.TabIndex = 966;
            this.gradientLabel5.Text = "Process Count Write";
            this.gradientLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel6
            // 
            this.gradientLabel6.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel6.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel6.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel6.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel6.Location = new System.Drawing.Point(311, 440);
            this.gradientLabel6.Name = "gradientLabel6";
            this.gradientLabel6.Size = new System.Drawing.Size(192, 38);
            this.gradientLabel6.TabIndex = 968;
            this.gradientLabel6.Text = "Process Count Read";
            this.gradientLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProcessCountRead
            // 
            this.lblProcessCountRead.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.lblProcessCountRead.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblProcessCountRead.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.lblProcessCountRead.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProcessCountRead.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblProcessCountRead.Location = new System.Drawing.Point(503, 440);
            this.lblProcessCountRead.Name = "lblProcessCountRead";
            this.lblProcessCountRead.Size = new System.Drawing.Size(75, 38);
            this.lblProcessCountRead.TabIndex = 967;
            this.lblProcessCountRead.Text = "1";
            this.lblProcessCountRead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel7
            // 
            this.gradientLabel7.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel7.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel7.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel7.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel7.Location = new System.Drawing.Point(12, 401);
            this.gradientLabel7.Name = "gradientLabel7";
            this.gradientLabel7.Size = new System.Drawing.Size(130, 38);
            this.gradientLabel7.TabIndex = 972;
            this.gradientLabel7.Text = "Exposure Busy";
            this.gradientLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProcessExpoBusy
            // 
            this.lblProcessExpoBusy.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.lblProcessExpoBusy.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblProcessExpoBusy.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.lblProcessExpoBusy.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProcessExpoBusy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblProcessExpoBusy.Location = new System.Drawing.Point(148, 401);
            this.lblProcessExpoBusy.Name = "lblProcessExpoBusy";
            this.lblProcessExpoBusy.Size = new System.Drawing.Size(84, 38);
            this.lblProcessExpoBusy.TabIndex = 971;
            this.lblProcessExpoBusy.Text = "1";
            this.lblProcessExpoBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel9
            // 
            this.gradientLabel9.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel9.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel9.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel9.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel9.Location = new System.Drawing.Point(12, 363);
            this.gradientLabel9.Name = "gradientLabel9";
            this.gradientLabel9.Size = new System.Drawing.Size(130, 38);
            this.gradientLabel9.TabIndex = 970;
            this.gradientLabel9.Text = "Job Start";
            this.gradientLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProcessJobStart
            // 
            this.lblProcessJobStart.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.lblProcessJobStart.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.lblProcessJobStart.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.lblProcessJobStart.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProcessJobStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblProcessJobStart.Location = new System.Drawing.Point(148, 363);
            this.lblProcessJobStart.Name = "lblProcessJobStart";
            this.lblProcessJobStart.Size = new System.Drawing.Size(84, 38);
            this.lblProcessJobStart.TabIndex = 969;
            this.lblProcessJobStart.Text = "1";
            this.lblProcessJobStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel8
            // 
            this.gradientLabel8.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel8.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel8.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel8.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel8.Location = new System.Drawing.Point(311, 10);
            this.gradientLabel8.Name = "gradientLabel8";
            this.gradientLabel8.Size = new System.Drawing.Size(554, 38);
            this.gradientLabel8.TabIndex = 973;
            this.gradientLabel8.Text = "Laser Step && Go Process";
            this.gradientLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProcessDataSave
            // 
            this.btnProcessDataSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnProcessDataSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnProcessDataSave.Image = ((System.Drawing.Image)(resources.GetObject("btnProcessDataSave.Image")));
            this.btnProcessDataSave.Location = new System.Drawing.Point(610, 469);
            this.btnProcessDataSave.Name = "btnProcessDataSave";
            this.btnProcessDataSave.Size = new System.Drawing.Size(124, 61);
            this.btnProcessDataSave.TabIndex = 980;
            this.btnProcessDataSave.Text = "Data Save";
            this.btnProcessDataSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcessDataSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProcessDataSave.UseVisualStyleBackColor = true;
            this.btnProcessDataSave.Click += new System.EventHandler(this.btnProcessDataSave_Click);
            // 
            // FormStageManualOP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 537);
            this.Controls.Add(this.btnProcessDataSave);
            this.Controls.Add(this.gradientLabel8);
            this.Controls.Add(this.gradientLabel7);
            this.Controls.Add(this.lblProcessExpoBusy);
            this.Controls.Add(this.gradientLabel9);
            this.Controls.Add(this.lblProcessJobStart);
            this.Controls.Add(this.gradientLabel6);
            this.Controls.Add(this.lblProcessCountRead);
            this.Controls.Add(this.gradientLabel5);
            this.Controls.Add(this.lblProcessCount);
            this.Controls.Add(this.btnLaserProcessStep1);
            this.Controls.Add(this.gradientLabel2);
            this.Controls.Add(this.btnLaserProcessMof);
            this.Controls.Add(this.LabelClampTime);
            this.Controls.Add(this.gradientLabel4);
            this.Controls.Add(this.LabelVACTime);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.gradientLabel3);
            this.Controls.Add(this.BtnVacuumOff);
            this.Controls.Add(this.BtnVacuumOn);
            this.Controls.Add(this.gradientLabel16);
            this.Controls.Add(this.BtnClampClose);
            this.Controls.Add(this.BtnClampOpen);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormStageManualOP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormStageManualOP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStageManualOP_FormClosing);
            this.Load += new System.EventHandler(this.FormStageManualOP_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Timer TimerUI;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel3;
        private Syncfusion.Windows.Forms.ButtonAdv BtnVacuumOff;
        private Syncfusion.Windows.Forms.ButtonAdv BtnVacuumOn;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel16;
        private Syncfusion.Windows.Forms.ButtonAdv BtnClampClose;
        private Syncfusion.Windows.Forms.ButtonAdv BtnClampOpen;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelVACTime;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelClampTime;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel4;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel2;
        private Syncfusion.Windows.Forms.ButtonAdv btnLaserProcessMof;
        private Syncfusion.Windows.Forms.ButtonAdv btnLaserProcessStep1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblProcessCount;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel5;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel6;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblProcessCountRead;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel7;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblProcessExpoBusy;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel9;
        private Syncfusion.Windows.Forms.Tools.GradientLabel lblProcessJobStart;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel8;
        private System.Windows.Forms.Button btnProcessDataSave;
    }
}