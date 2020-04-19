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
            this.TextTime = new System.Windows.Forms.TextBox();
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.BtnUserLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.textVersion = new System.Windows.Forms.TextBox();
            this.BtnMainPage = new System.Windows.Forms.Button();
            this.BtnManualPage = new System.Windows.Forms.Button();
            this.BtnTeachPage = new System.Windows.Forms.Button();
            this.BtnDataPage = new System.Windows.Forms.Button();
            this.ImgList = new System.Windows.Forms.ImageList(this.components);
            this.BtnHelpPage = new System.Windows.Forms.Button();
            this.BtnLogPage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextTime
            // 
            this.TextTime.BackColor = System.Drawing.SystemColors.Control;
            this.TextTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextTime.Enabled = false;
            this.TextTime.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextTime.Location = new System.Drawing.Point(2529, 117);
            this.TextTime.Margin = new System.Windows.Forms.Padding(5);
            this.TextTime.Name = "TextTime";
            this.TextTime.Size = new System.Drawing.Size(291, 27);
            this.TextTime.TabIndex = 1;
            // 
            // TimerUI
            // 
            this.TimerUI.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // BtnUserLogin
            // 
            this.BtnUserLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnUserLogin.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnUserLogin.Image = ((System.Drawing.Image)(resources.GetObject("BtnUserLogin.Image")));
            this.BtnUserLogin.Location = new System.Drawing.Point(2646, 14);
            this.BtnUserLogin.Margin = new System.Windows.Forms.Padding(5);
            this.BtnUserLogin.Name = "BtnUserLogin";
            this.BtnUserLogin.Size = new System.Drawing.Size(174, 93);
            this.BtnUserLogin.TabIndex = 3;
            this.BtnUserLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnUserLogin.UseVisualStyleBackColor = true;
            this.BtnUserLogin.Click += new System.EventHandler(this.BtnUserLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.Location = new System.Drawing.Point(2903, 18);
            this.btnExit.Margin = new System.Windows.Forms.Padding(5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(91, 93);
            this.btnExit.TabIndex = 5;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // textVersion
            // 
            this.textVersion.BackColor = System.Drawing.Color.Gold;
            this.textVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textVersion.Enabled = false;
            this.textVersion.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textVersion.Location = new System.Drawing.Point(745, 66);
            this.textVersion.Margin = new System.Windows.Forms.Padding(5);
            this.textVersion.Name = "textVersion";
            this.textVersion.Size = new System.Drawing.Size(291, 27);
            this.textVersion.TabIndex = 762;
            this.textVersion.Text = "Version";
            this.textVersion.TextChanged += new System.EventHandler(this.textVersion_TextChanged);
            // 
            // BtnMainPage
            // 
            this.BtnMainPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnMainPage.ImageList = this.ImgList;
            this.BtnMainPage.Location = new System.Drawing.Point(1233, 2);
            this.BtnMainPage.Name = "BtnMainPage";
            this.BtnMainPage.Size = new System.Drawing.Size(120, 120);
            this.BtnMainPage.TabIndex = 763;
            this.BtnMainPage.UseVisualStyleBackColor = true;
            this.BtnMainPage.Click += new System.EventHandler(this.BtnMainPage_Click);
            // 
            // BtnManualPage
            // 
            this.BtnManualPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnManualPage.Location = new System.Drawing.Point(1352, 2);
            this.BtnManualPage.Name = "BtnManualPage";
            this.BtnManualPage.Size = new System.Drawing.Size(120, 120);
            this.BtnManualPage.TabIndex = 764;
            this.BtnManualPage.UseVisualStyleBackColor = true;
            this.BtnManualPage.Click += new System.EventHandler(this.BtnManualPage_Click);
            // 
            // BtnTeachPage
            // 
            this.BtnTeachPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnTeachPage.Location = new System.Drawing.Point(1471, 2);
            this.BtnTeachPage.Name = "BtnTeachPage";
            this.BtnTeachPage.Size = new System.Drawing.Size(120, 120);
            this.BtnTeachPage.TabIndex = 765;
            this.BtnTeachPage.UseVisualStyleBackColor = true;
            this.BtnTeachPage.Click += new System.EventHandler(this.BtnTeachPage_Click);
            // 
            // BtnDataPage
            // 
            this.BtnDataPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnDataPage.Location = new System.Drawing.Point(1590, 2);
            this.BtnDataPage.Name = "BtnDataPage";
            this.BtnDataPage.Size = new System.Drawing.Size(120, 120);
            this.BtnDataPage.TabIndex = 766;
            this.BtnDataPage.UseVisualStyleBackColor = true;
            this.BtnDataPage.Click += new System.EventHandler(this.BtnDataPage_Click);
            // 
            // ImgList
            // 
            this.ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImgList.ImageStream")));
            this.ImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImgList.Images.SetKeyName(0, "Main_Enable.png");
            this.ImgList.Images.SetKeyName(1, "Main_Press.png");
            this.ImgList.Images.SetKeyName(2, "Manual_Enable.png");
            this.ImgList.Images.SetKeyName(3, "Manual_Press.png");
            this.ImgList.Images.SetKeyName(4, "Teaching_Enable.png");
            this.ImgList.Images.SetKeyName(5, "Teaching_Press.png");
            this.ImgList.Images.SetKeyName(6, "Data_Enable.png");
            this.ImgList.Images.SetKeyName(7, "Data_Press.png");
            this.ImgList.Images.SetKeyName(8, "Log_Enable.png");
            this.ImgList.Images.SetKeyName(9, "Log_Press.png");
            this.ImgList.Images.SetKeyName(10, "Help_Enable.png");
            this.ImgList.Images.SetKeyName(11, "Help_Press.png");
            // 
            // BtnHelpPage
            // 
            this.BtnHelpPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnHelpPage.Location = new System.Drawing.Point(1828, 2);
            this.BtnHelpPage.Name = "BtnHelpPage";
            this.BtnHelpPage.Size = new System.Drawing.Size(120, 120);
            this.BtnHelpPage.TabIndex = 768;
            this.BtnHelpPage.UseVisualStyleBackColor = true;
            this.BtnHelpPage.Click += new System.EventHandler(this.BtnHelpPage_Click);
            // 
            // BtnLogPage
            // 
            this.BtnLogPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnLogPage.Location = new System.Drawing.Point(1709, 2);
            this.BtnLogPage.Name = "BtnLogPage";
            this.BtnLogPage.Size = new System.Drawing.Size(120, 120);
            this.BtnLogPage.TabIndex = 767;
            this.BtnLogPage.UseVisualStyleBackColor = true;
            this.BtnLogPage.Click += new System.EventHandler(this.BtnLogPage_Click);
            // 
            // FormTopScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(3020, 129);
            this.ControlBox = false;
            this.Controls.Add(this.BtnHelpPage);
            this.Controls.Add(this.BtnLogPage);
            this.Controls.Add(this.BtnDataPage);
            this.Controls.Add(this.BtnTeachPage);
            this.Controls.Add(this.BtnManualPage);
            this.Controls.Add(this.BtnMainPage);
            this.Controls.Add(this.textVersion);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.BtnUserLogin);
            this.Controls.Add(this.TextTime);
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
        private System.Windows.Forms.TextBox TextTime;
        private System.Windows.Forms.Timer TimerUI;
        private System.Windows.Forms.Button BtnUserLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox textVersion;
        private System.Windows.Forms.Button BtnMainPage;
        private System.Windows.Forms.Button BtnManualPage;
        private System.Windows.Forms.Button BtnTeachPage;
        private System.Windows.Forms.Button BtnDataPage;
        private System.Windows.Forms.ImageList ImgList;
        private System.Windows.Forms.Button BtnHelpPage;
        private System.Windows.Forms.Button BtnLogPage;
    }
}