namespace LWDicer.UI
{
    partial class FormHandlerManualOP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHandlerManualOP));
            this.BtnExit = new System.Windows.Forms.Button();
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.gradientLabel3 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnVacuumOff = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnVacuumOn = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelVACTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(433, 359);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 757;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // gradientLabel3
            // 
            this.gradientLabel3.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel3.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel3.Location = new System.Drawing.Point(12, 9);
            this.gradientLabel3.Name = "gradientLabel3";
            this.gradientLabel3.Size = new System.Drawing.Size(263, 38);
            this.gradientLabel3.TabIndex = 948;
            this.gradientLabel3.Text = "Handler Vacuum";
            this.gradientLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnVacuumOff
            // 
            this.BtnVacuumOff.BackColor = System.Drawing.Color.LightGray;
            this.BtnVacuumOff.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnVacuumOff.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnVacuumOff.FlatAppearance.BorderSize = 5;
            this.BtnVacuumOff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOff.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnVacuumOff.Location = new System.Drawing.Point(145, 48);
            this.BtnVacuumOff.Name = "BtnVacuumOff";
            this.BtnVacuumOff.Size = new System.Drawing.Size(130, 64);
            this.BtnVacuumOff.TabIndex = 947;
            this.BtnVacuumOff.Tag = "1";
            this.BtnVacuumOff.Text = "Off";
            this.BtnVacuumOff.Click += new System.EventHandler(this.BtnVacuumOff_Click);
            // 
            // BtnVacuumOn
            // 
            this.BtnVacuumOn.BackColor = System.Drawing.Color.LightGray;
            this.BtnVacuumOn.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnVacuumOn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnVacuumOn.FlatAppearance.BorderSize = 5;
            this.BtnVacuumOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnVacuumOn.Location = new System.Drawing.Point(12, 48);
            this.BtnVacuumOn.Name = "BtnVacuumOn";
            this.BtnVacuumOn.Size = new System.Drawing.Size(130, 64);
            this.BtnVacuumOn.TabIndex = 946;
            this.BtnVacuumOn.Tag = "0";
            this.BtnVacuumOn.Text = "On";
            this.BtnVacuumOn.Click += new System.EventHandler(this.BtnVacuumOn_Click);
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.Silver, System.Drawing.Color.Maroon);
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel1.Location = new System.Drawing.Point(12, 113);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(130, 30);
            this.gradientLabel1.TabIndex = 949;
            this.gradientLabel1.Text = "수행 시간";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelVACTime
            // 
            this.LabelVACTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelVACTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelVACTime.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.LabelVACTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelVACTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LabelVACTime.Location = new System.Drawing.Point(145, 113);
            this.LabelVACTime.Name = "LabelVACTime";
            this.LabelVACTime.Size = new System.Drawing.Size(130, 30);
            this.LabelVACTime.TabIndex = 950;
            this.LabelVACTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormHandlerManualOP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 466);
            this.Controls.Add(this.LabelVACTime);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.gradientLabel3);
            this.Controls.Add(this.BtnVacuumOff);
            this.Controls.Add(this.BtnVacuumOn);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormHandlerManualOP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormHandlerManualOP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHandlerManualOP_FormClosing);
            this.Load += new System.EventHandler(this.FormHandlerManualOP_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Timer TimerUI;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel3;
        private Syncfusion.Windows.Forms.ButtonAdv BtnVacuumOff;
        private Syncfusion.Windows.Forms.ButtonAdv BtnVacuumOn;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelVACTime;
    }
}