namespace Core.UI
{
    partial class FormTeachScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTeachScreen));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnScanner = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnTeachStage = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnCamera = new Syncfusion.Windows.Forms.ButtonAdv();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.BtnCamera);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BtnTeachStage);
            this.panel1.Controls.Add(this.BtnScanner);
            this.panel1.ForeColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(295, 117);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(686, 878);
            this.panel1.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(229, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Teaching GUI";
            // 
            // BtnScanner
            // 
            this.BtnScanner.AutoEllipsis = true;
            this.BtnScanner.BackColor = System.Drawing.SystemColors.Control;
            this.BtnScanner.FlatAppearance.BorderSize = 5;
            this.BtnScanner.Font = new System.Drawing.Font("Gulim", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnScanner.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnScanner.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnScanner.Location = new System.Drawing.Point(51, 313);
            this.BtnScanner.Margin = new System.Windows.Forms.Padding(5);
            this.BtnScanner.Name = "BtnScanner";
            this.BtnScanner.Size = new System.Drawing.Size(239, 136);
            this.BtnScanner.TabIndex = 18;
            this.BtnScanner.Text = "Scanner";
            this.BtnScanner.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnScanner.Click += new System.EventHandler(this.BtnScanner_Click);
            // 
            // BtnTeachStage
            // 
            this.BtnTeachStage.AutoEllipsis = true;
            this.BtnTeachStage.BackColor = System.Drawing.SystemColors.Control;
            this.BtnTeachStage.FlatAppearance.BorderSize = 5;
            this.BtnTeachStage.Font = new System.Drawing.Font("Gulim", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTeachStage.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnTeachStage.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnTeachStage.Location = new System.Drawing.Point(51, 167);
            this.BtnTeachStage.Margin = new System.Windows.Forms.Padding(5);
            this.BtnTeachStage.Name = "BtnTeachStage";
            this.BtnTeachStage.Size = new System.Drawing.Size(239, 136);
            this.BtnTeachStage.TabIndex = 16;
            this.BtnTeachStage.Text = "Work Stage";
            this.BtnTeachStage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnTeachStage.Click += new System.EventHandler(this.BtnTeachStage_Click);
            // 
            // BtnCamera
            // 
            this.BtnCamera.AutoEllipsis = true;
            this.BtnCamera.BackColor = System.Drawing.SystemColors.Control;
            this.BtnCamera.FlatAppearance.BorderSize = 5;
            this.BtnCamera.Font = new System.Drawing.Font("Gulim", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCamera.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnCamera.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnCamera.Location = new System.Drawing.Point(348, 167);
            this.BtnCamera.Margin = new System.Windows.Forms.Padding(5);
            this.BtnCamera.Name = "BtnCamera";
            this.BtnCamera.Size = new System.Drawing.Size(239, 136);
            this.BtnCamera.TabIndex = 17;
            this.BtnCamera.Text = "Camera";
            this.BtnCamera.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnCamera.Click += new System.EventHandler(this.BtnCameraScanner_Click);
            // 
            // FormTeachScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(3012, 1572);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FormTeachScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Teach Screen";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private Syncfusion.Windows.Forms.ButtonAdv BtnScanner;
        private Syncfusion.Windows.Forms.ButtonAdv BtnTeachStage;
        private Syncfusion.Windows.Forms.ButtonAdv BtnCamera;
    }
}