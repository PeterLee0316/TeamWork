namespace LWDicer.UI
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
            this.LabelAlarmText = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnBuzzerOff = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnReset = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnExit = new Syncfusion.Windows.Forms.ButtonAdv();
            this.LabelTrouble = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.PicAlarmPos = new System.Windows.Forms.PictureBox();
            this.LabelAlarmCode = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.BtnKor = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnEng = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnChn = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnJpn = new Syncfusion.Windows.Forms.ButtonAdv();
            this.TmrAlarm = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicAlarmPos)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelAlarmText
            // 
            this.LabelAlarmText.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelAlarmText.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelAlarmText.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelAlarmText.Location = new System.Drawing.Point(16, 37);
            this.LabelAlarmText.Name = "LabelAlarmText";
            this.LabelAlarmText.Size = new System.Drawing.Size(598, 64);
            this.LabelAlarmText.TabIndex = 0;
            this.LabelAlarmText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // LabelTrouble
            // 
            this.LabelTrouble.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelTrouble.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTrouble.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTrouble.Location = new System.Drawing.Point(12, 43);
            this.LabelTrouble.Name = "LabelTrouble";
            this.LabelTrouble.Padding = new System.Windows.Forms.Padding(10);
            this.LabelTrouble.Size = new System.Drawing.Size(364, 373);
            this.LabelTrouble.TabIndex = 5;
            this.LabelTrouble.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel1.BackgroundImage")));
            this.gradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gradientPanel1.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel1.Controls.Add(this.autoLabel1);
            this.gradientPanel1.Controls.Add(this.LabelTrouble);
            this.gradientPanel1.Location = new System.Drawing.Point(645, 70);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(389, 437);
            this.gradientPanel1.TabIndex = 29;
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
            // gradientPanel2
            // 
            this.gradientPanel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel2.BackgroundImage")));
            this.gradientPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gradientPanel2.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientPanel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel2.Controls.Add(this.PicAlarmPos);
            this.gradientPanel2.Controls.Add(this.LabelAlarmCode);
            this.gradientPanel2.Controls.Add(this.LabelAlarmText);
            this.gradientPanel2.Location = new System.Drawing.Point(12, 12);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(627, 573);
            this.gradientPanel2.TabIndex = 30;
            // 
            // PicAlarmPos
            // 
            this.PicAlarmPos.BackColor = System.Drawing.Color.White;
            this.PicAlarmPos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PicAlarmPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicAlarmPos.Location = new System.Drawing.Point(16, 104);
            this.PicAlarmPos.Name = "PicAlarmPos";
            this.PicAlarmPos.Size = new System.Drawing.Size(598, 452);
            this.PicAlarmPos.TabIndex = 32;
            this.PicAlarmPos.TabStop = false;
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
            // BtnKor
            // 
            this.BtnKor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnKor.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnKor.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnKor.Location = new System.Drawing.Point(660, 9);
            this.BtnKor.Name = "BtnKor";
            this.BtnKor.Size = new System.Drawing.Size(90, 57);
            this.BtnKor.TabIndex = 31;
            this.BtnKor.Text = "KOR";
            this.BtnKor.Click += new System.EventHandler(this.BtnLanguage_Click);
            // 
            // BtnEng
            // 
            this.BtnEng.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnEng.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnEng.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnEng.Location = new System.Drawing.Point(751, 9);
            this.BtnEng.Name = "BtnEng";
            this.BtnEng.Size = new System.Drawing.Size(90, 57);
            this.BtnEng.TabIndex = 32;
            this.BtnEng.Text = "ENG";
            this.BtnEng.Click += new System.EventHandler(this.BtnLanguage_Click);
            // 
            // BtnChn
            // 
            this.BtnChn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnChn.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnChn.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnChn.Location = new System.Drawing.Point(842, 9);
            this.BtnChn.Name = "BtnChn";
            this.BtnChn.Size = new System.Drawing.Size(90, 57);
            this.BtnChn.TabIndex = 33;
            this.BtnChn.Text = "CHN";
            this.BtnChn.Click += new System.EventHandler(this.BtnLanguage_Click);
            // 
            // BtnJpn
            // 
            this.BtnJpn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnJpn.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnJpn.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnJpn.Location = new System.Drawing.Point(933, 9);
            this.BtnJpn.Name = "BtnJpn";
            this.BtnJpn.Size = new System.Drawing.Size(90, 57);
            this.BtnJpn.TabIndex = 34;
            this.BtnJpn.Text = "JPN";
            this.BtnJpn.Click += new System.EventHandler(this.BtnLanguage_Click);
            // 
            // TmrAlarm
            // 
            this.TmrAlarm.Tick += new System.EventHandler(this.TmrAlarm_Tick);
            // 
            // FormAlarmDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 598);
            this.Controls.Add(this.BtnJpn);
            this.Controls.Add(this.BtnChn);
            this.Controls.Add(this.BtnEng);
            this.Controls.Add(this.BtnKor);
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

        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelAlarmText;
        private Syncfusion.Windows.Forms.ButtonAdv BtnBuzzerOff;
        private Syncfusion.Windows.Forms.ButtonAdv BtnReset;
        private Syncfusion.Windows.Forms.ButtonAdv BtnExit;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTrouble;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.AutoLabel LabelAlarmCode;
        private Syncfusion.Windows.Forms.ButtonAdv BtnKor;
        private Syncfusion.Windows.Forms.ButtonAdv BtnEng;
        private Syncfusion.Windows.Forms.ButtonAdv BtnChn;
        private Syncfusion.Windows.Forms.ButtonAdv BtnJpn;
        private System.Windows.Forms.Timer TmrAlarm;
        private System.Windows.Forms.PictureBox PicAlarmPos;
    }
}