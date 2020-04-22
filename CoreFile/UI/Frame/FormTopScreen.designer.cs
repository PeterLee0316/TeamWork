namespace Core.UI
{
    partial class FormTopScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTopScreen));
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.BtnUserLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.BtnMainPage = new System.Windows.Forms.Button();
            this.ImgList = new System.Windows.Forms.ImageList(this.components);
            this.BtnManualPage = new System.Windows.Forms.Button();
            this.BtnTeachPage = new System.Windows.Forms.Button();
            this.BtnDataPage = new System.Windows.Forms.Button();
            this.BtnHelpPage = new System.Windows.Forms.Button();
            this.BtnLogPage = new System.Windows.Forms.Button();
            this.BtnPlayback = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.lblSwVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TimerUI
            // 
            this.TimerUI.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // BtnUserLogin
            // 
            this.BtnUserLogin.BackColor = System.Drawing.Color.Transparent;
            this.BtnUserLogin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnUserLogin.BackgroundImage")));
            this.BtnUserLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnUserLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUserLogin.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnUserLogin.Location = new System.Drawing.Point(2609, 1);
            this.BtnUserLogin.Margin = new System.Windows.Forms.Padding(5);
            this.BtnUserLogin.Name = "BtnUserLogin";
            this.BtnUserLogin.Size = new System.Drawing.Size(187, 84);
            this.BtnUserLogin.TabIndex = 3;
            this.BtnUserLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnUserLogin.UseVisualStyleBackColor = false;
            this.BtnUserLogin.Click += new System.EventHandler(this.BtnUserLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(2896, 2);
            this.btnExit.Margin = new System.Windows.Forms.Padding(5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(120, 120);
            this.btnExit.TabIndex = 5;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // BtnMainPage
            // 
            this.BtnMainPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnMainPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnMainPage.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.BtnMainPage.ImageList = this.ImgList;
            this.BtnMainPage.Location = new System.Drawing.Point(788, 1);
            this.BtnMainPage.Name = "BtnMainPage";
            this.BtnMainPage.Size = new System.Drawing.Size(120, 120);
            this.BtnMainPage.TabIndex = 763;
            this.BtnMainPage.UseVisualStyleBackColor = false;
            this.BtnMainPage.Click += new System.EventHandler(this.BtnMainPage_Click);
            // 
            // ImgList
            // 
            this.ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImgList.ImageStream")));
            this.ImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImgList.Images.SetKeyName(0, "Main_Enable.png");
            this.ImgList.Images.SetKeyName(1, "Main_Press.png");
            this.ImgList.Images.SetKeyName(2, "Manual_Enable.png");
            this.ImgList.Images.SetKeyName(3, "Manual_Press.png");
            this.ImgList.Images.SetKeyName(4, "Data_Enable.png");
            this.ImgList.Images.SetKeyName(5, "Data_Press.png");
            this.ImgList.Images.SetKeyName(6, "Teaching_Enable.png");
            this.ImgList.Images.SetKeyName(7, "Teaching_Press.png");
            this.ImgList.Images.SetKeyName(8, "Log_Enable.png");
            this.ImgList.Images.SetKeyName(9, "Log_Press.png");
            this.ImgList.Images.SetKeyName(10, "Help_Enable.png");
            this.ImgList.Images.SetKeyName(11, "Help_Press.png");
            // 
            // BtnManualPage
            // 
            this.BtnManualPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnManualPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnManualPage.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.BtnManualPage.Location = new System.Drawing.Point(907, 1);
            this.BtnManualPage.Name = "BtnManualPage";
            this.BtnManualPage.Size = new System.Drawing.Size(120, 120);
            this.BtnManualPage.TabIndex = 764;
            this.BtnManualPage.UseVisualStyleBackColor = false;
            this.BtnManualPage.Click += new System.EventHandler(this.BtnManualPage_Click);
            // 
            // BtnTeachPage
            // 
            this.BtnTeachPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnTeachPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnTeachPage.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.BtnTeachPage.Location = new System.Drawing.Point(1026, 1);
            this.BtnTeachPage.Name = "BtnTeachPage";
            this.BtnTeachPage.Size = new System.Drawing.Size(120, 120);
            this.BtnTeachPage.TabIndex = 765;
            this.BtnTeachPage.UseVisualStyleBackColor = false;
            this.BtnTeachPage.Click += new System.EventHandler(this.BtnTeachPage_Click);
            // 
            // BtnDataPage
            // 
            this.BtnDataPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnDataPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnDataPage.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.BtnDataPage.Location = new System.Drawing.Point(1145, 1);
            this.BtnDataPage.Name = "BtnDataPage";
            this.BtnDataPage.Size = new System.Drawing.Size(120, 120);
            this.BtnDataPage.TabIndex = 766;
            this.BtnDataPage.UseVisualStyleBackColor = false;
            this.BtnDataPage.Click += new System.EventHandler(this.BtnDataPage_Click);
            // 
            // BtnHelpPage
            // 
            this.BtnHelpPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnHelpPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnHelpPage.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.BtnHelpPage.Location = new System.Drawing.Point(1383, 1);
            this.BtnHelpPage.Name = "BtnHelpPage";
            this.BtnHelpPage.Size = new System.Drawing.Size(120, 120);
            this.BtnHelpPage.TabIndex = 768;
            this.BtnHelpPage.UseVisualStyleBackColor = false;
            this.BtnHelpPage.Click += new System.EventHandler(this.BtnHelpPage_Click);
            // 
            // BtnLogPage
            // 
            this.BtnLogPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnLogPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnLogPage.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.BtnLogPage.Location = new System.Drawing.Point(1264, 1);
            this.BtnLogPage.Name = "BtnLogPage";
            this.BtnLogPage.Size = new System.Drawing.Size(120, 120);
            this.BtnLogPage.TabIndex = 767;
            this.BtnLogPage.UseVisualStyleBackColor = false;
            this.BtnLogPage.Click += new System.EventHandler(this.BtnLogPage_Click);
            // 
            // BtnPlayback
            // 
            this.BtnPlayback.BackColor = System.Drawing.Color.Transparent;
            this.BtnPlayback.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnPlayback.BackgroundImage")));
            this.BtnPlayback.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnPlayback.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.BtnPlayback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPlayback.Location = new System.Drawing.Point(2341, 1);
            this.BtnPlayback.Name = "BtnPlayback";
            this.BtnPlayback.Size = new System.Drawing.Size(113, 120);
            this.BtnPlayback.TabIndex = 769;
            this.BtnPlayback.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(2464, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 120);
            this.button1.TabIndex = 770;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.BackColor = System.Drawing.Color.Transparent;
            this.lblDateTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateTime.Location = new System.Drawing.Point(2603, 90);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(142, 32);
            this.lblDateTime.TabIndex = 771;
            this.lblDateTime.Text = "Date-Time";
            // 
            // lblSwVersion
            // 
            this.lblSwVersion.AutoSize = true;
            this.lblSwVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblSwVersion.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSwVersion.Location = new System.Drawing.Point(534, 77);
            this.lblSwVersion.Name = "lblSwVersion";
            this.lblSwVersion.Size = new System.Drawing.Size(162, 32);
            this.lblSwVersion.TabIndex = 772;
            this.lblSwVersion.Text = "SW-Version";
            // 
            // FormTopScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(3020, 130);
            this.ControlBox = false;
            this.Controls.Add(this.lblSwVersion);
            this.Controls.Add(this.lblDateTime);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnPlayback);
            this.Controls.Add(this.BtnHelpPage);
            this.Controls.Add(this.BtnLogPage);
            this.Controls.Add(this.BtnDataPage);
            this.Controls.Add(this.BtnTeachPage);
            this.Controls.Add(this.BtnManualPage);
            this.Controls.Add(this.BtnMainPage);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.BtnUserLogin);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormTopScreen";
            this.Text = "Top Screen";
            this.Load += new System.EventHandler(this.FormTopScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer TimerUI;
        private System.Windows.Forms.Button BtnUserLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button BtnMainPage;
        private System.Windows.Forms.Button BtnManualPage;
        private System.Windows.Forms.Button BtnTeachPage;
        private System.Windows.Forms.Button BtnDataPage;
        private System.Windows.Forms.ImageList ImgList;
        private System.Windows.Forms.Button BtnHelpPage;
        private System.Windows.Forms.Button BtnLogPage;
        private System.Windows.Forms.Button BtnPlayback;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblSwVersion;
    }
}