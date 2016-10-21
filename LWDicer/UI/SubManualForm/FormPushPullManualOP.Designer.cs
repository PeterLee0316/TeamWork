namespace LWDicer.UI
{
    partial class FormPushPullManualOP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPushPullManualOP));
            this.BtnExit = new System.Windows.Forms.Button();
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.BtnGripUnLock = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnGripLock = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPushPullDown = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnPushPullUp = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientLabel16 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelGripTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel10 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelUpDnTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel3 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(231, 393);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 756;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // TimerUI
            // 
            this.TimerUI.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // BtnGripUnLock
            // 
            this.BtnGripUnLock.BackColor = System.Drawing.Color.LightGray;
            this.BtnGripUnLock.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnGripUnLock.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnGripUnLock.FlatAppearance.BorderSize = 5;
            this.BtnGripUnLock.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnGripUnLock.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnGripUnLock.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnGripUnLock.Location = new System.Drawing.Point(144, 57);
            this.BtnGripUnLock.Name = "BtnGripUnLock";
            this.BtnGripUnLock.Size = new System.Drawing.Size(117, 64);
            this.BtnGripUnLock.TabIndex = 867;
            this.BtnGripUnLock.Tag = "1";
            this.BtnGripUnLock.Text = "Unlock";
            this.BtnGripUnLock.Click += new System.EventHandler(this.BtnGripUnLock_Click);
            // 
            // BtnGripLock
            // 
            this.BtnGripLock.BackColor = System.Drawing.Color.LightGray;
            this.BtnGripLock.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnGripLock.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnGripLock.FlatAppearance.BorderSize = 5;
            this.BtnGripLock.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnGripLock.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnGripLock.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnGripLock.Location = new System.Drawing.Point(24, 57);
            this.BtnGripLock.Name = "BtnGripLock";
            this.BtnGripLock.Size = new System.Drawing.Size(117, 64);
            this.BtnGripLock.TabIndex = 866;
            this.BtnGripLock.Tag = "0";
            this.BtnGripLock.Text = "Lock";
            this.BtnGripLock.Click += new System.EventHandler(this.BtnGripLock_Click);
            // 
            // BtnPushPullDown
            // 
            this.BtnPushPullDown.BackColor = System.Drawing.Color.LightGray;
            this.BtnPushPullDown.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPushPullDown.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPushPullDown.FlatAppearance.BorderSize = 5;
            this.BtnPushPullDown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPushPullDown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPushPullDown.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPushPullDown.Location = new System.Drawing.Point(447, 57);
            this.BtnPushPullDown.Name = "BtnPushPullDown";
            this.BtnPushPullDown.Size = new System.Drawing.Size(117, 64);
            this.BtnPushPullDown.TabIndex = 869;
            this.BtnPushPullDown.Tag = "1";
            this.BtnPushPullDown.Text = "Down";
            this.BtnPushPullDown.Click += new System.EventHandler(this.BtnPushPullDown_Click);
            // 
            // BtnPushPullUp
            // 
            this.BtnPushPullUp.BackColor = System.Drawing.Color.LightGray;
            this.BtnPushPullUp.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnPushPullUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnPushPullUp.FlatAppearance.BorderSize = 5;
            this.BtnPushPullUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPushPullUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnPushPullUp.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPushPullUp.Location = new System.Drawing.Point(327, 57);
            this.BtnPushPullUp.Name = "BtnPushPullUp";
            this.BtnPushPullUp.Size = new System.Drawing.Size(117, 64);
            this.BtnPushPullUp.TabIndex = 868;
            this.BtnPushPullUp.Tag = "0";
            this.BtnPushPullUp.Text = "Up";
            this.BtnPushPullUp.Click += new System.EventHandler(this.BtnPushPullUp_Click);
            // 
            // gradientLabel16
            // 
            this.gradientLabel16.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel16.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel16.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel16.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel16.Location = new System.Drawing.Point(24, 17);
            this.gradientLabel16.Name = "gradientLabel16";
            this.gradientLabel16.Size = new System.Drawing.Size(237, 38);
            this.gradientLabel16.TabIndex = 932;
            this.gradientLabel16.Text = "Gripper";
            this.gradientLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel1.Location = new System.Drawing.Point(327, 17);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(237, 38);
            this.gradientLabel1.TabIndex = 933;
            this.gradientLabel1.Text = "PushPull Up/Down";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelGripTime
            // 
            this.LabelGripTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelGripTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelGripTime.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.LabelGripTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelGripTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LabelGripTime.Location = new System.Drawing.Point(144, 123);
            this.LabelGripTime.Name = "LabelGripTime";
            this.LabelGripTime.Size = new System.Drawing.Size(117, 30);
            this.LabelGripTime.TabIndex = 969;
            this.LabelGripTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel10
            // 
            this.gradientLabel10.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.Silver, System.Drawing.Color.Maroon);
            this.gradientLabel10.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel10.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel10.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel10.Location = new System.Drawing.Point(24, 123);
            this.gradientLabel10.Name = "gradientLabel10";
            this.gradientLabel10.Size = new System.Drawing.Size(117, 30);
            this.gradientLabel10.TabIndex = 968;
            this.gradientLabel10.Text = "수행 시간";
            this.gradientLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelUpDnTime
            // 
            this.LabelUpDnTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelUpDnTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelUpDnTime.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.LabelUpDnTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelUpDnTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LabelUpDnTime.Location = new System.Drawing.Point(447, 123);
            this.LabelUpDnTime.Name = "LabelUpDnTime";
            this.LabelUpDnTime.Size = new System.Drawing.Size(117, 30);
            this.LabelUpDnTime.TabIndex = 971;
            this.LabelUpDnTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel3
            // 
            this.gradientLabel3.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.Silver, System.Drawing.Color.Maroon);
            this.gradientLabel3.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel3.Location = new System.Drawing.Point(327, 123);
            this.gradientLabel3.Name = "gradientLabel3";
            this.gradientLabel3.Size = new System.Drawing.Size(117, 30);
            this.gradientLabel3.TabIndex = 970;
            this.gradientLabel3.Text = "수행 시간";
            this.gradientLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormPushPullManualOP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 466);
            this.Controls.Add(this.LabelUpDnTime);
            this.Controls.Add(this.gradientLabel3);
            this.Controls.Add(this.LabelGripTime);
            this.Controls.Add(this.gradientLabel10);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.gradientLabel16);
            this.Controls.Add(this.BtnPushPullDown);
            this.Controls.Add(this.BtnPushPullUp);
            this.Controls.Add(this.BtnGripUnLock);
            this.Controls.Add(this.BtnGripLock);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormPushPullManualOP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormPushPullManualOP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPushPullManualOP_FormClosing);
            this.Load += new System.EventHandler(this.FormPushPullManualOP_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Timer TimerUI;
        private Syncfusion.Windows.Forms.ButtonAdv BtnGripUnLock;
        private Syncfusion.Windows.Forms.ButtonAdv BtnGripLock;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPushPullDown;
        private Syncfusion.Windows.Forms.ButtonAdv BtnPushPullUp;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel16;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelGripTime;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel10;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelUpDnTime;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel3;
    }
}