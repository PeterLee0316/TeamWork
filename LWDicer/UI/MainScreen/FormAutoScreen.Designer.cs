namespace LWDicer.UI
{
    partial class FormAutoScreen
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
            this.BtnAlarm100 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm200 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm300 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm400 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm500 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm600 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAlarm700 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.SuspendLayout();
            // 
            // BtnAlarm100
            // 
            this.BtnAlarm100.Location = new System.Drawing.Point(186, 312);
            this.BtnAlarm100.Name = "BtnAlarm100";
            this.BtnAlarm100.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm100.TabIndex = 0;
            this.BtnAlarm100.Tag = "100";
            this.BtnAlarm100.Text = "Alarm Test 100";
            this.BtnAlarm100.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm200
            // 
            this.BtnAlarm200.Location = new System.Drawing.Point(334, 312);
            this.BtnAlarm200.Name = "BtnAlarm200";
            this.BtnAlarm200.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm200.TabIndex = 1;
            this.BtnAlarm200.Tag = "200";
            this.BtnAlarm200.Text = "Alarm Test 200";
            this.BtnAlarm200.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm300
            // 
            this.BtnAlarm300.Location = new System.Drawing.Point(482, 312);
            this.BtnAlarm300.Name = "BtnAlarm300";
            this.BtnAlarm300.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm300.TabIndex = 2;
            this.BtnAlarm300.Tag = "300";
            this.BtnAlarm300.Text = "Alarm Test 300";
            this.BtnAlarm300.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm400
            // 
            this.BtnAlarm400.Location = new System.Drawing.Point(630, 312);
            this.BtnAlarm400.Name = "BtnAlarm400";
            this.BtnAlarm400.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm400.TabIndex = 3;
            this.BtnAlarm400.Tag = "400";
            this.BtnAlarm400.Text = "Alarm Test 400";
            this.BtnAlarm400.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm500
            // 
            this.BtnAlarm500.Location = new System.Drawing.Point(778, 312);
            this.BtnAlarm500.Name = "BtnAlarm500";
            this.BtnAlarm500.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm500.TabIndex = 4;
            this.BtnAlarm500.Tag = "500";
            this.BtnAlarm500.Text = "Alarm Test 500";
            this.BtnAlarm500.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm600
            // 
            this.BtnAlarm600.Location = new System.Drawing.Point(926, 312);
            this.BtnAlarm600.Name = "BtnAlarm600";
            this.BtnAlarm600.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm600.TabIndex = 5;
            this.BtnAlarm600.Tag = "600";
            this.BtnAlarm600.Text = "Alarm Test 600";
            this.BtnAlarm600.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // BtnAlarm700
            // 
            this.BtnAlarm700.Location = new System.Drawing.Point(1074, 312);
            this.BtnAlarm700.Name = "BtnAlarm700";
            this.BtnAlarm700.Size = new System.Drawing.Size(121, 81);
            this.BtnAlarm700.TabIndex = 6;
            this.BtnAlarm700.Tag = "700";
            this.BtnAlarm700.Text = "Alarm Test 700";
            this.BtnAlarm700.Click += new System.EventHandler(this.BtnAlarmTest_Click);
            // 
            // FormAutoScreen
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
            this.Name = "FormAutoScreen";
            this.Text = "Auto Screen";
            this.Activated += new System.EventHandler(this.FormAutoScreen_Activated);
            this.Deactivate += new System.EventHandler(this.FormAutoScreen_Deactivate);
            this.Shown += new System.EventHandler(this.FormAutoScreen_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm100;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm200;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm300;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm400;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm500;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm600;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAlarm700;
    }
}