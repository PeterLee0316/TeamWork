﻿namespace Core.UI
{
    partial class FormIntro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIntro));
            this.StatusBar = new System.Windows.Forms.ProgressBar();
            this.LabelStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(1006, 926);
            this.StatusBar.Margin = new System.Windows.Forms.Padding(5);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(613, 30);
            this.StatusBar.TabIndex = 1;
            this.StatusBar.Value = 50;
            // 
            // LabelStatus
            // 
            this.LabelStatus.AutoSize = true;
            this.LabelStatus.BackColor = System.Drawing.Color.Transparent;
            this.LabelStatus.Font = new System.Drawing.Font("Malgun Gothic", 14.14286F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelStatus.ForeColor = System.Drawing.Color.White;
            this.LabelStatus.Location = new System.Drawing.Point(1007, 866);
            this.LabelStatus.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(90, 46);
            this.LabelStatus.TabIndex = 5;
            this.LabelStatus.Text = "Text";
            // 
            // FormIntro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1652, 983);
            this.Controls.Add(this.LabelStatus);
            this.Controls.Add(this.StatusBar);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.DarkRed;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormIntro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Intro";
            this.Load += new System.EventHandler(this.FormIntro_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar StatusBar;
        private System.Windows.Forms.Label LabelStatus;
    }
}