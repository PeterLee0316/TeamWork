﻿namespace LWDicer.UI
{
    partial class FormAlarmDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlarmDisplay));
            this.BtnBuzzerOff = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnReset = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnExit = new Syncfusion.Windows.Forms.ButtonAdv();
            this.TmrAlarm = new System.Windows.Forms.Timer(this.components);
            this.BtnEdit = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnSave = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.LabelTrouble2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.LabelTrouble1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.LabelAlarmText2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.PicAlarmPos = new System.Windows.Forms.PictureBox();
            this.LabelAlarmCode = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.LabelAlarmText1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicAlarmPos)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnBuzzerOff
            // 
            this.BtnBuzzerOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnBuzzerOff.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnBuzzerOff.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnBuzzerOff.Location = new System.Drawing.Point(660, 511);
            this.BtnBuzzerOff.Name = "BtnBuzzerOff";
            this.BtnBuzzerOff.Size = new System.Drawing.Size(119, 74);
            this.BtnBuzzerOff.TabIndex = 2;
            this.BtnBuzzerOff.Text = "Buzzer Off";
            this.BtnBuzzerOff.Click += new System.EventHandler(this.BtnBuzzerOff_Click);
            // 
            // BtnReset
            // 
            this.BtnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnReset.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnReset.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnReset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BtnReset.Location = new System.Drawing.Point(783, 511);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(119, 74);
            this.BtnReset.TabIndex = 3;
            this.BtnReset.Text = "Reset";
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.BackColor = System.Drawing.Color.Tan;
            this.BtnExit.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnExit.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.Location = new System.Drawing.Point(905, 511);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(119, 74);
            this.BtnExit.TabIndex = 4;
            this.BtnExit.Text = "Exit";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // TmrAlarm
            // 
            this.TmrAlarm.Tick += new System.EventHandler(this.TmrAlarm_Tick);
            // 
            // BtnEdit
            // 
            this.BtnEdit.BackColor = System.Drawing.Color.LightGray;
            this.BtnEdit.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnEdit.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnEdit.Location = new System.Drawing.Point(783, 455);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(119, 50);
            this.BtnEdit.TabIndex = 31;
            this.BtnEdit.Text = "Edit";
            this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.LightGray;
            this.BtnSave.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnSave.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSave.Location = new System.Drawing.Point(905, 455);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(119, 50);
            this.BtnSave.TabIndex = 32;
            this.BtnSave.Text = "Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel1.BackgroundImage")));
            this.gradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gradientPanel1.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel1.Controls.Add(this.LabelTrouble2);
            this.gradientPanel1.Controls.Add(this.autoLabel1);
            this.gradientPanel1.Controls.Add(this.LabelTrouble1);
            this.gradientPanel1.Location = new System.Drawing.Point(645, 12);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(389, 437);
            this.gradientPanel1.TabIndex = 29;
            // 
            // LabelTrouble2
            // 
            this.LabelTrouble2.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelTrouble2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTrouble2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTrouble2.Location = new System.Drawing.Point(12, 230);
            this.LabelTrouble2.Name = "LabelTrouble2";
            this.LabelTrouble2.Padding = new System.Windows.Forms.Padding(10);
            this.LabelTrouble2.Size = new System.Drawing.Size(364, 187);
            this.LabelTrouble2.TabIndex = 31;
            this.LabelTrouble2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoLabel1
            // 
            this.autoLabel1.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel1.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel1.Location = new System.Drawing.Point(25, 8);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(127, 15);
            this.autoLabel1.TabIndex = 30;
            this.autoLabel1.Text = "Troubleshooting";
            // 
            // LabelTrouble1
            // 
            this.LabelTrouble1.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelTrouble1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTrouble1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTrouble1.Location = new System.Drawing.Point(12, 43);
            this.LabelTrouble1.Name = "LabelTrouble1";
            this.LabelTrouble1.Padding = new System.Windows.Forms.Padding(10);
            this.LabelTrouble1.Size = new System.Drawing.Size(364, 187);
            this.LabelTrouble1.TabIndex = 5;
            this.LabelTrouble1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel2.BackgroundImage")));
            this.gradientPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gradientPanel2.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientPanel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel2.Controls.Add(this.LabelAlarmText2);
            this.gradientPanel2.Controls.Add(this.PicAlarmPos);
            this.gradientPanel2.Controls.Add(this.LabelAlarmCode);
            this.gradientPanel2.Controls.Add(this.LabelAlarmText1);
            this.gradientPanel2.Location = new System.Drawing.Point(12, 12);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(627, 573);
            this.gradientPanel2.TabIndex = 30;
            // 
            // LabelAlarmText2
            // 
            this.LabelAlarmText2.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelAlarmText2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelAlarmText2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelAlarmText2.Location = new System.Drawing.Point(16, 85);
            this.LabelAlarmText2.Name = "LabelAlarmText2";
            this.LabelAlarmText2.Padding = new System.Windows.Forms.Padding(5);
            this.LabelAlarmText2.Size = new System.Drawing.Size(598, 48);
            this.LabelAlarmText2.TabIndex = 33;
            this.LabelAlarmText2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PicAlarmPos
            // 
            this.PicAlarmPos.BackColor = System.Drawing.Color.White;
            this.PicAlarmPos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PicAlarmPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicAlarmPos.Location = new System.Drawing.Point(16, 137);
            this.PicAlarmPos.Name = "PicAlarmPos";
            this.PicAlarmPos.Size = new System.Drawing.Size(598, 421);
            this.PicAlarmPos.TabIndex = 32;
            this.PicAlarmPos.TabStop = false;
            this.PicAlarmPos.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PicAlarmPos_MouseClick);
            // 
            // LabelAlarmCode
            // 
            this.LabelAlarmCode.BackColor = System.Drawing.Color.Transparent;
            this.LabelAlarmCode.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelAlarmCode.Location = new System.Drawing.Point(36, 8);
            this.LabelAlarmCode.Name = "LabelAlarmCode";
            this.LabelAlarmCode.Size = new System.Drawing.Size(92, 15);
            this.LabelAlarmCode.TabIndex = 31;
            this.LabelAlarmCode.Text = "Alarm Code";
            // 
            // LabelAlarmText1
            // 
            this.LabelAlarmText1.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelAlarmText1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelAlarmText1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelAlarmText1.Location = new System.Drawing.Point(16, 37);
            this.LabelAlarmText1.Name = "LabelAlarmText1";
            this.LabelAlarmText1.Padding = new System.Windows.Forms.Padding(5);
            this.LabelAlarmText1.Size = new System.Drawing.Size(598, 48);
            this.LabelAlarmText1.TabIndex = 0;
            this.LabelAlarmText1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormAlarmDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 598);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnEdit);
            this.Controls.Add(this.gradientPanel1);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnReset);
            this.Controls.Add(this.BtnBuzzerOff);
            this.Controls.Add(this.gradientPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormAlarmDisplay";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAlarmDisplay_FormClosing);
            this.Load += new System.EventHandler(this.FormAlarmDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).EndInit();
            this.gradientPanel2.ResumeLayout(false);
            this.gradientPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicAlarmPos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelAlarmText1;
        private Syncfusion.Windows.Forms.ButtonAdv BtnBuzzerOff;
        private Syncfusion.Windows.Forms.ButtonAdv BtnReset;
        private Syncfusion.Windows.Forms.ButtonAdv BtnExit;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTrouble1;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.AutoLabel LabelAlarmCode;
        private System.Windows.Forms.Timer TmrAlarm;
        private System.Windows.Forms.PictureBox PicAlarmPos;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTrouble2;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelAlarmText2;
        private Syncfusion.Windows.Forms.ButtonAdv BtnEdit;
        private Syncfusion.Windows.Forms.ButtonAdv BtnSave;
    }
}