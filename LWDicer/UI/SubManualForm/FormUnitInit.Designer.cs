namespace LWDicer.UI
{
    partial class FormUnitInit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUnitInit));
            this.BtnLoader = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPushPull = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnSpinner1 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnSpinner2 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnStage = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnHandler = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnSelectAll = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnCancelAll = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnExecuteIni = new Syncfusion.Windows.Forms.ButtonAdv();
            this.Image = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.LabelProgress = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.buttonAdv1 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAutoManager = new Syncfusion.Windows.Forms.ButtonAdv();
            this.SuspendLayout();
            // 
            // BtnLoader
            // 
            this.BtnLoader.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLoader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnLoader.Location = new System.Drawing.Point(12, 60);
            this.BtnLoader.Name = "BtnLoader";
            this.BtnLoader.Size = new System.Drawing.Size(120, 67);
            this.BtnLoader.TabIndex = 8;
            this.BtnLoader.Tag = "0";
            this.BtnLoader.Text = " Loader";
            this.BtnLoader.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnLoader.Click += new System.EventHandler(this.BtnInitPart_Click);
            // 
            // BtnPushPull
            // 
            this.BtnPushPull.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPushPull.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnPushPull.Location = new System.Drawing.Point(12, 133);
            this.BtnPushPull.Name = "BtnPushPull";
            this.BtnPushPull.Size = new System.Drawing.Size(120, 67);
            this.BtnPushPull.TabIndex = 9;
            this.BtnPushPull.Tag = "4";
            this.BtnPushPull.Text = " Push Pull";
            this.BtnPushPull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnPushPull.Click += new System.EventHandler(this.BtnInitPart_Click);
            // 
            // BtnSpinner1
            // 
            this.BtnSpinner1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSpinner1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSpinner1.Location = new System.Drawing.Point(144, 60);
            this.BtnSpinner1.Name = "BtnSpinner1";
            this.BtnSpinner1.Size = new System.Drawing.Size(120, 67);
            this.BtnSpinner1.TabIndex = 10;
            this.BtnSpinner1.Tag = "1";
            this.BtnSpinner1.Text = " Spinner 1";
            this.BtnSpinner1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSpinner1.Click += new System.EventHandler(this.BtnInitPart_Click);
            // 
            // BtnSpinner2
            // 
            this.BtnSpinner2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSpinner2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSpinner2.Location = new System.Drawing.Point(411, 60);
            this.BtnSpinner2.Name = "BtnSpinner2";
            this.BtnSpinner2.Size = new System.Drawing.Size(120, 67);
            this.BtnSpinner2.TabIndex = 11;
            this.BtnSpinner2.Tag = "2";
            this.BtnSpinner2.Text = " Spinner 2";
            this.BtnSpinner2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSpinner2.Click += new System.EventHandler(this.BtnInitPart_Click);
            // 
            // BtnStage
            // 
            this.BtnStage.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnStage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnStage.Location = new System.Drawing.Point(144, 133);
            this.BtnStage.Name = "BtnStage";
            this.BtnStage.Size = new System.Drawing.Size(120, 67);
            this.BtnStage.TabIndex = 14;
            this.BtnStage.Tag = "";
            this.BtnStage.Text = " Stage";
            this.BtnStage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnStage.Click += new System.EventHandler(this.BtnInitPart_Click);
            // 
            // BtnHandler
            // 
            this.BtnHandler.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnHandler.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnHandler.Location = new System.Drawing.Point(276, 60);
            this.BtnHandler.Name = "BtnHandler";
            this.BtnHandler.Size = new System.Drawing.Size(120, 67);
            this.BtnHandler.TabIndex = 12;
            this.BtnHandler.Tag = "";
            this.BtnHandler.Text = "Handler";
            this.BtnHandler.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnHandler.Click += new System.EventHandler(this.BtnInitPart_Click);
            // 
            // BtnSelectAll
            // 
            this.BtnSelectAll.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSelectAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSelectAll.Location = new System.Drawing.Point(111, 285);
            this.BtnSelectAll.Name = "BtnSelectAll";
            this.BtnSelectAll.Size = new System.Drawing.Size(115, 63);
            this.BtnSelectAll.TabIndex = 752;
            this.BtnSelectAll.Text = "전체 선택";
            this.BtnSelectAll.Click += new System.EventHandler(this.BtnSelectAll_Click);
            // 
            // BtnCancelAll
            // 
            this.BtnCancelAll.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCancelAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnCancelAll.Location = new System.Drawing.Point(243, 285);
            this.BtnCancelAll.Name = "BtnCancelAll";
            this.BtnCancelAll.Size = new System.Drawing.Size(115, 63);
            this.BtnCancelAll.TabIndex = 751;
            this.BtnCancelAll.Text = "전체 취소";
            this.BtnCancelAll.Click += new System.EventHandler(this.BtnCancelAll_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(509, 285);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(115, 63);
            this.BtnExit.TabIndex = 750;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnExecuteIni
            // 
            this.BtnExecuteIni.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExecuteIni.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExecuteIni.Location = new System.Drawing.Point(375, 285);
            this.BtnExecuteIni.Name = "BtnExecuteIni";
            this.BtnExecuteIni.Size = new System.Drawing.Size(115, 63);
            this.BtnExecuteIni.TabIndex = 753;
            this.BtnExecuteIni.Text = "초기화 실행";
            this.BtnExecuteIni.Click += new System.EventHandler(this.BtnExecuteIni_Click);
            // 
            // Image
            // 
            this.Image.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Image.ImageStream")));
            this.Image.TransparentColor = System.Drawing.Color.Transparent;
            this.Image.Images.SetKeyName(0, "Led_Off.png");
            this.Image.Images.SetKeyName(1, "Led_On.png");
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LabelProgress
            // 
            this.LabelProgress.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelProgress.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelProgress.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelProgress.Location = new System.Drawing.Point(12, 9);
            this.LabelProgress.Name = "LabelProgress";
            this.LabelProgress.Size = new System.Drawing.Size(612, 34);
            this.LabelProgress.TabIndex = 754;
            this.LabelProgress.Text = "Progress : ";
            this.LabelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAdv1
            // 
            this.buttonAdv1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonAdv1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonAdv1.Location = new System.Drawing.Point(509, 216);
            this.buttonAdv1.Name = "buttonAdv1";
            this.buttonAdv1.Size = new System.Drawing.Size(115, 63);
            this.buttonAdv1.TabIndex = 755;
            this.buttonAdv1.Text = "초기화 실행 (Test Only)";
            this.buttonAdv1.Click += new System.EventHandler(this.buttonAdv1_Click);
            // 
            // BtnAutoManager
            // 
            this.BtnAutoManager.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnAutoManager.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnAutoManager.Location = new System.Drawing.Point(411, 133);
            this.BtnAutoManager.Name = "BtnAutoManager";
            this.BtnAutoManager.Size = new System.Drawing.Size(120, 67);
            this.BtnAutoManager.TabIndex = 756;
            this.BtnAutoManager.Tag = "0";
            this.BtnAutoManager.Text = "AutoManager";
            this.BtnAutoManager.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnAutoManager.Visible = false;
            // 
            // FormUnitInit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 360);
            this.Controls.Add(this.BtnAutoManager);
            this.Controls.Add(this.buttonAdv1);
            this.Controls.Add(this.LabelProgress);
            this.Controls.Add(this.BtnExecuteIni);
            this.Controls.Add(this.BtnSelectAll);
            this.Controls.Add(this.BtnCancelAll);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnStage);
            this.Controls.Add(this.BtnHandler);
            this.Controls.Add(this.BtnSpinner2);
            this.Controls.Add(this.BtnSpinner1);
            this.Controls.Add(this.BtnPushPull);
            this.Controls.Add(this.BtnLoader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormUnitInit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unit Initialize Screen";
            this.Load += new System.EventHandler(this.FormUnitInit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnLoader;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPushPull;
        private Syncfusion.Windows.Forms.ButtonAdv BtnSpinner1;
        private Syncfusion.Windows.Forms.ButtonAdv BtnSpinner2;
        private Syncfusion.Windows.Forms.ButtonAdv BtnStage;
        private Syncfusion.Windows.Forms.ButtonAdv BtnHandler;
        private Syncfusion.Windows.Forms.ButtonAdv BtnSelectAll;
        private Syncfusion.Windows.Forms.ButtonAdv BtnCancelAll;
        private System.Windows.Forms.Button BtnExit;
        private Syncfusion.Windows.Forms.ButtonAdv BtnExecuteIni;
        private System.Windows.Forms.ImageList Image;
        private System.Windows.Forms.Timer timer1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelProgress;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv1;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAutoManager;
    }
}