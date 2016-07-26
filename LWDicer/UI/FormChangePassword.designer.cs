namespace LWDicer.UI
{
    partial class FormChangePassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChangePassword));
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnChange = new System.Windows.Forms.Button();
            this.CurrentPW = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.NewPW1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.NewPW2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel4 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(416, 146);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(128, 71);
            this.BtnExit.TabIndex = 757;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnChange
            // 
            this.BtnChange.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnChange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnChange.Image = ((System.Drawing.Image)(resources.GetObject("BtnChange.Image")));
            this.BtnChange.Location = new System.Drawing.Point(285, 146);
            this.BtnChange.Name = "BtnChange";
            this.BtnChange.Size = new System.Drawing.Size(128, 71);
            this.BtnChange.TabIndex = 756;
            this.BtnChange.Text = "  Save";
            this.BtnChange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnChange.UseVisualStyleBackColor = true;
            this.BtnChange.Click += new System.EventHandler(this.BtnChange_Click);
            // 
            // CurrentPW
            // 
            this.CurrentPW.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.BackwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.PeachPuff);
            this.CurrentPW.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.CurrentPW.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CurrentPW.Location = new System.Drawing.Point(242, 9);
            this.CurrentPW.Name = "CurrentPW";
            this.CurrentPW.Size = new System.Drawing.Size(302, 40);
            this.CurrentPW.TabIndex = 758;
            this.CurrentPW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CurrentPW.Click += new System.EventHandler(this.CurrentPW_Click);
            // 
            // NewPW1
            // 
            this.NewPW1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.BackwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.LemonChiffon);
            this.NewPW1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.NewPW1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.NewPW1.Location = new System.Drawing.Point(242, 53);
            this.NewPW1.Name = "NewPW1";
            this.NewPW1.Size = new System.Drawing.Size(302, 40);
            this.NewPW1.TabIndex = 759;
            this.NewPW1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NewPW1.Click += new System.EventHandler(this.NewPW1_Click);
            // 
            // NewPW2
            // 
            this.NewPW2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.BackwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.LemonChiffon);
            this.NewPW2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.NewPW2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.NewPW2.Location = new System.Drawing.Point(242, 97);
            this.NewPW2.Name = "NewPW2";
            this.NewPW2.Size = new System.Drawing.Size(302, 40);
            this.NewPW2.TabIndex = 760;
            this.NewPW2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NewPW2.Click += new System.EventHandler(this.NewPW2_Click);
            // 
            // gradientLabel4
            // 
            this.gradientLabel4.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.PathEllipse, System.Drawing.Color.White, System.Drawing.SystemColors.ControlLight);
            this.gradientLabel4.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel4.Location = new System.Drawing.Point(14, 9);
            this.gradientLabel4.Name = "gradientLabel4";
            this.gradientLabel4.Size = new System.Drawing.Size(222, 40);
            this.gradientLabel4.TabIndex = 761;
            this.gradientLabel4.Text = "1. Old Password";
            this.gradientLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.PathEllipse, System.Drawing.Color.White, System.Drawing.SystemColors.ControlLight);
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.Location = new System.Drawing.Point(14, 53);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(222, 40);
            this.gradientLabel1.TabIndex = 762;
            this.gradientLabel1.Text = "2. New Password";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.PathEllipse, System.Drawing.Color.White, System.Drawing.SystemColors.ControlLight);
            this.gradientLabel2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel2.Location = new System.Drawing.Point(14, 97);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(222, 40);
            this.gradientLabel2.TabIndex = 763;
            this.gradientLabel2.Text = "3. New Password Confirm";
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 229);
            this.Controls.Add(this.gradientLabel2);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.gradientLabel4);
            this.Controls.Add(this.NewPW2);
            this.Controls.Add(this.NewPW1);
            this.Controls.Add(this.CurrentPW);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnChange);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input Password";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInputPassword_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnChange;
        private Syncfusion.Windows.Forms.Tools.GradientLabel CurrentPW;
        private Syncfusion.Windows.Forms.Tools.GradientLabel NewPW1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel NewPW2;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel4;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel2;
    }
}