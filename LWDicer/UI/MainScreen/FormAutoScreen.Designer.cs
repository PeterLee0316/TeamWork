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
            this.BtnOriginReturn = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnUnitInit = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnStart = new Syncfusion.Windows.Forms.ButtonAdv();
            this.LabelButtonGuide = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.SuspendLayout();
            // 
            // BtnOriginReturn
            // 
            this.BtnOriginReturn.AutoEllipsis = true;
            this.BtnOriginReturn.BackColor = System.Drawing.Color.DarkKhaki;
            this.BtnOriginReturn.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnOriginReturn.FlatAppearance.BorderSize = 5;
            this.BtnOriginReturn.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnOriginReturn.ForeColor = System.Drawing.Color.Black;
            this.BtnOriginReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnOriginReturn.Location = new System.Drawing.Point(1082, 566);
            this.BtnOriginReturn.Name = "BtnOriginReturn";
            this.BtnOriginReturn.Size = new System.Drawing.Size(145, 68);
            this.BtnOriginReturn.TabIndex = 15;
            this.BtnOriginReturn.Text = "원점 복귀";
            this.BtnOriginReturn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnOriginReturn.Click += new System.EventHandler(this.BtnOriginReturn_Click);
            // 
            // BtnUnitInit
            // 
            this.BtnUnitInit.AutoEllipsis = true;
            this.BtnUnitInit.BackColor = System.Drawing.Color.DarkKhaki;
            this.BtnUnitInit.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnUnitInit.FlatAppearance.BorderSize = 5;
            this.BtnUnitInit.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnUnitInit.ForeColor = System.Drawing.Color.Black;
            this.BtnUnitInit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnUnitInit.Location = new System.Drawing.Point(1082, 651);
            this.BtnUnitInit.Name = "BtnUnitInit";
            this.BtnUnitInit.Size = new System.Drawing.Size(145, 68);
            this.BtnUnitInit.TabIndex = 16;
            this.BtnUnitInit.Text = "Unit 초기화";
            this.BtnUnitInit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnUnitInit.Click += new System.EventHandler(this.BtnUnitInit_Click);
            // 
            // BtnStart
            // 
            this.BtnStart.AutoEllipsis = true;
            this.BtnStart.BackColor = System.Drawing.Color.DarkKhaki;
            this.BtnStart.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnStart.FlatAppearance.BorderSize = 5;
            this.BtnStart.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnStart.ForeColor = System.Drawing.Color.Black;
            this.BtnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnStart.Location = new System.Drawing.Point(1082, 737);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(145, 68);
            this.BtnStart.TabIndex = 17;
            this.BtnStart.Text = "Start AutoRun";
            this.BtnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // LabelButtonGuide
            // 
            this.LabelButtonGuide.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Vertical, System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(240)))), ((int)(((byte)(247))))), System.Drawing.Color.LightCyan);
            this.LabelButtonGuide.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelButtonGuide.Location = new System.Drawing.Point(359, 156);
            this.LabelButtonGuide.Name = "LabelButtonGuide";
            this.LabelButtonGuide.Size = new System.Drawing.Size(454, 68);
            this.LabelButtonGuide.TabIndex = 18;
            this.LabelButtonGuide.Text = "gradientLabel1";
            this.LabelButtonGuide.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormAutoScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1278, 817);
            this.Controls.Add(this.LabelButtonGuide);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.BtnOriginReturn);
            this.Controls.Add(this.BtnUnitInit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAutoScreen";
            this.Text = "Auto Screen";
            this.Activated += new System.EventHandler(this.FormAutoScreen_Activated);
            this.Deactivate += new System.EventHandler(this.FormAutoScreen_Deactivate);
            this.Shown += new System.EventHandler(this.FormAutoScreen_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnOriginReturn;
        private Syncfusion.Windows.Forms.ButtonAdv BtnUnitInit;
        private Syncfusion.Windows.Forms.ButtonAdv BtnStart;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelButtonGuide;
    }
}