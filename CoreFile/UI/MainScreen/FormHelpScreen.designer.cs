namespace Core.UI
{
    partial class FormHelpScreen
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
            this.BtnTestFunction = new Syncfusion.Windows.Forms.ButtonAdv();
            this.SuspendLayout();
            // 
            // BtnTestFunction
            // 
            this.BtnTestFunction.AutoEllipsis = true;
            this.BtnTestFunction.BackColor = System.Drawing.SystemColors.Control;
            this.BtnTestFunction.FlatAppearance.BorderSize = 5;
            this.BtnTestFunction.Font = new System.Drawing.Font("Gulim", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTestFunction.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnTestFunction.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnTestFunction.Location = new System.Drawing.Point(548, 472);
            this.BtnTestFunction.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.BtnTestFunction.Name = "BtnTestFunction";
            this.BtnTestFunction.Size = new System.Drawing.Size(239, 136);
            this.BtnTestFunction.TabIndex = 26;
            this.BtnTestFunction.Text = "Test Function";
            this.BtnTestFunction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnTestFunction.Click += new System.EventHandler(this.BtnTestFunction_Click);
            // 
            // FormHelpScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(3012, 1572);
            this.Controls.Add(this.BtnTestFunction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FormHelpScreen";
            this.Text = "Help Screen";
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnTestFunction;
    }
}