namespace LWDicer.UI
{
    partial class FormMessageBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessageBox));
            this.BtnCancel = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnOK = new Syncfusion.Windows.Forms.ButtonAdv();
            this.LabelText = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCancel.ForeColor = System.Drawing.Color.Navy;
            this.BtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("BtnCancel.Image")));
            this.BtnCancel.Location = new System.Drawing.Point(500, 207);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(113, 54);
            this.BtnCancel.TabIndex = 6;
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
            this.BtnOK.Location = new System.Drawing.Point(382, 207);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(113, 54);
            this.BtnOK.TabIndex = 5;
            this.BtnOK.Text = "OK";
            this.BtnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // LabelText
            // 
            this.LabelText.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelText.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelText.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.LabelText.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelText.Location = new System.Drawing.Point(14, 10);
            this.LabelText.Name = "LabelText";
            this.LabelText.Size = new System.Drawing.Size(598, 90);
            this.LabelText.TabIndex = 8;
            this.LabelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.BackwardDiagonal, System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))), System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel1.BorderAppearance = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.Location = new System.Drawing.Point(3, 2);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(620, 277);
            this.gradientLabel1.TabIndex = 9;
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.gradientLabel2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientLabel2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientLabel2.Location = new System.Drawing.Point(14, 101);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(598, 90);
            this.gradientLabel2.TabIndex = 10;
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(625, 281);
            this.Controls.Add(this.gradientLabel2);
            this.Controls.Add(this.LabelText);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.gradientLabel1);
            this.Name = "FormMessageBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Messgae Box";
            this.Load += new System.EventHandler(this.FormUtilMsg_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Syncfusion.Windows.Forms.ButtonAdv BtnCancel;
        private Syncfusion.Windows.Forms.ButtonAdv BtnOK;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelText;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel2;
    }
}