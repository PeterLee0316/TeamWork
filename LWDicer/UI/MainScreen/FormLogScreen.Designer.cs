namespace LWDicer.UI
{
    partial class FormLogScreen
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
            this.BtnAlarm700 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm600 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm500 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm400 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm300 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm200 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm100 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.SuspendLayout();
            // 
            // BtnAlarm700
            // 
            this.BtnAlarm700.Location = new System.Drawing.Point(1023, 368);
            this.BtnAlarm700.Name = "BtnAlarm700";
            this.BtnAlarm700.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm700.TabIndex = 13;
            this.BtnAlarm700.Tag = "700";
            this.BtnAlarm700.Text = "Alarm Test 700";
            this.BtnAlarm700.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm600
            // 
            this.BtnAlarm600.Location = new System.Drawing.Point(875, 368);
            this.BtnAlarm600.Name = "BtnAlarm600";
            this.BtnAlarm600.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm600.TabIndex = 12;
            this.BtnAlarm600.Tag = "600";
            this.BtnAlarm600.Text = "Alarm Test 600";
            this.BtnAlarm600.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm500
            // 
            this.BtnAlarm500.Location = new System.Drawing.Point(727, 368);
            this.BtnAlarm500.Name = "BtnAlarm500";
            this.BtnAlarm500.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm500.TabIndex = 11;
            this.BtnAlarm500.Tag = "500";
            this.BtnAlarm500.Text = "Alarm Test 500";
            this.BtnAlarm500.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm400
            // 
            this.BtnAlarm400.Location = new System.Drawing.Point(579, 368);
            this.BtnAlarm400.Name = "BtnAlarm400";
            this.BtnAlarm400.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm400.TabIndex = 10;
            this.BtnAlarm400.Tag = "400";
            this.BtnAlarm400.Text = "Alarm Test 400";
            this.BtnAlarm400.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm300
            // 
            this.BtnAlarm300.Location = new System.Drawing.Point(431, 368);
            this.BtnAlarm300.Name = "BtnAlarm300";
            this.BtnAlarm300.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm300.TabIndex = 9;
            this.BtnAlarm300.Tag = "300";
            this.BtnAlarm300.Text = "Alarm Test 300";
            this.BtnAlarm300.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm200
            // 
            this.BtnAlarm200.Location = new System.Drawing.Point(283, 368);
            this.BtnAlarm200.Name = "BtnAlarm200";
            this.BtnAlarm200.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm200.TabIndex = 8;
            this.BtnAlarm200.Tag = "200";
            this.BtnAlarm200.Text = "Alarm Test 200";
            this.BtnAlarm200.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm100
            // 
            this.BtnAlarm100.Location = new System.Drawing.Point(135, 368);
            this.BtnAlarm100.Name = "BtnAlarm100";
            this.BtnAlarm100.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm100.TabIndex = 7;
            this.BtnAlarm100.Tag = "100";
            this.BtnAlarm100.Text = "Alarm Test 100";
            this.BtnAlarm100.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // FormLogScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1278, 817);
            this.Controls.Add(this.BtnAlarm700);
            this.Controls.Add(this.BtnAlarm600);
            this.Controls.Add(this.BtnAlarm500);
            this.Controls.Add(this.BtnAlarm400);
            this.Controls.Add(this.BtnAlarm300);
            this.Controls.Add(this.BtnAlarm200);
            this.Controls.Add(this.BtnAlarm100);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogScreen";
            this.Text = "Log Screen";
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm700;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm600;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm500;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm400;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm300;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm200;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm100;
    }
}