namespace LWDicer.UI
{
    partial class FormAlarmEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlarmEdit));
            this.BtnCancel = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnOK = new Syncfusion.Windows.Forms.ButtonAdv();
            this.TextAlarm_Eng = new System.Windows.Forms.TextBox();
            this.TextTrouble_Eng = new System.Windows.Forms.TextBox();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelAlarm_System = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.TextAlarm_System = new System.Windows.Forms.TextBox();
            this.LabelTrouble_System = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.TextTrouble_System = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCancel.ForeColor = System.Drawing.Color.Navy;
            this.BtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("BtnCancel.Image")));
            this.BtnCancel.Location = new System.Drawing.Point(540, 178);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(116, 62);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnOK.CustomManagedColor = System.Drawing.Color.White;
            this.BtnOK.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnOK.ForeColor = System.Drawing.Color.Navy;
            this.BtnOK.Image = ((System.Drawing.Image)(resources.GetObject("BtnOK.Image")));
            this.BtnOK.Location = new System.Drawing.Point(421, 178);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(116, 62);
            this.BtnOK.TabIndex = 4;
            this.BtnOK.Text = "OK";
            this.BtnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // TextAlarm_Eng
            // 
            this.TextAlarm_Eng.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextAlarm_Eng.Location = new System.Drawing.Point(208, 9);
            this.TextAlarm_Eng.Margin = new System.Windows.Forms.Padding(5);
            this.TextAlarm_Eng.Name = "TextAlarm_Eng";
            this.TextAlarm_Eng.Size = new System.Drawing.Size(448, 25);
            this.TextAlarm_Eng.TabIndex = 0;
            this.TextAlarm_Eng.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextTrouble_Eng
            // 
            this.TextTrouble_Eng.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextTrouble_Eng.Location = new System.Drawing.Point(208, 96);
            this.TextTrouble_Eng.Margin = new System.Windows.Forms.Padding(5);
            this.TextTrouble_Eng.Name = "TextTrouble_Eng";
            this.TextTrouble_Eng.Size = new System.Drawing.Size(448, 25);
            this.TextTrouble_Eng.TabIndex = 2;
            this.TextTrouble_Eng.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.Location = new System.Drawing.Point(10, 9);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(190, 25);
            this.gradientLabel1.TabIndex = 11;
            this.gradientLabel1.Text = "Alarm Text [ENG]";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel2.Location = new System.Drawing.Point(10, 96);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(190, 25);
            this.gradientLabel2.TabIndex = 12;
            this.gradientLabel2.Text = "Troubleshooting [ENG]";
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelAlarm_System
            // 
            this.LabelAlarm_System.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelAlarm_System.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelAlarm_System.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelAlarm_System.Location = new System.Drawing.Point(10, 43);
            this.LabelAlarm_System.Name = "LabelAlarm_System";
            this.LabelAlarm_System.Size = new System.Drawing.Size(190, 25);
            this.LabelAlarm_System.TabIndex = 14;
            this.LabelAlarm_System.Text = "Alarm Text [SYSTEM]";
            this.LabelAlarm_System.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextAlarm_System
            // 
            this.TextAlarm_System.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextAlarm_System.Location = new System.Drawing.Point(208, 43);
            this.TextAlarm_System.Margin = new System.Windows.Forms.Padding(5);
            this.TextAlarm_System.Name = "TextAlarm_System";
            this.TextAlarm_System.Size = new System.Drawing.Size(448, 25);
            this.TextAlarm_System.TabIndex = 1;
            this.TextAlarm_System.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelTrouble_System
            // 
            this.LabelTrouble_System.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.LabelTrouble_System.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTrouble_System.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTrouble_System.Location = new System.Drawing.Point(10, 130);
            this.LabelTrouble_System.Name = "LabelTrouble_System";
            this.LabelTrouble_System.Size = new System.Drawing.Size(190, 25);
            this.LabelTrouble_System.TabIndex = 16;
            this.LabelTrouble_System.Text = "Troubleshooting [SYSTEM]";
            this.LabelTrouble_System.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextTrouble_System
            // 
            this.TextTrouble_System.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextTrouble_System.Location = new System.Drawing.Point(208, 130);
            this.TextTrouble_System.Margin = new System.Windows.Forms.Padding(5);
            this.TextTrouble_System.Name = "TextTrouble_System";
            this.TextTrouble_System.Size = new System.Drawing.Size(448, 25);
            this.TextTrouble_System.TabIndex = 3;
            this.TextTrouble_System.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormAlarmEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 251);
            this.Controls.Add(this.LabelTrouble_System);
            this.Controls.Add(this.TextTrouble_System);
            this.Controls.Add(this.LabelAlarm_System);
            this.Controls.Add(this.TextAlarm_System);
            this.Controls.Add(this.gradientLabel2);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.TextTrouble_Eng);
            this.Controls.Add(this.TextAlarm_Eng);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormAlarmEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarm Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAlarmEdit_FormClosing);
            this.Load += new System.EventHandler(this.FormAlarmEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnCancel;
        private Syncfusion.Windows.Forms.ButtonAdv BtnOK;
        private System.Windows.Forms.TextBox TextAlarm_Eng;
        private System.Windows.Forms.TextBox TextTrouble_Eng;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel2;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelAlarm_System;
        private System.Windows.Forms.TextBox TextAlarm_System;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTrouble_System;
        private System.Windows.Forms.TextBox TextTrouble_System;
    }
}