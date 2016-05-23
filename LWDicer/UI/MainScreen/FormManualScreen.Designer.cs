namespace LWDicer.UI
{
    partial class FormManualScreen
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
            this.BtnInput = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnLimitSensor = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnOriginReturn = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnUnitInit = new Syncfusion.Windows.Forms.ButtonAdv();
            this.SuspendLayout();
            // 
            // BtnInput
            // 
            this.BtnInput.AutoEllipsis = true;
            this.BtnInput.BackColor = System.Drawing.SystemColors.Control;
            this.BtnInput.FlatAppearance.BorderSize = 5;
            this.BtnInput.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnInput.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnInput.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnInput.Location = new System.Drawing.Point(12, 12);
            this.BtnInput.Name = "BtnInput";
            this.BtnInput.Size = new System.Drawing.Size(128, 58);
            this.BtnInput.TabIndex = 10;
            this.BtnInput.Text = "Input";
            this.BtnInput.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnInput.Click += new System.EventHandler(this.BtnInput_Click);
            // 
            // BtnLimitSensor
            // 
            this.BtnLimitSensor.AutoEllipsis = true;
            this.BtnLimitSensor.BackColor = System.Drawing.SystemColors.Control;
            this.BtnLimitSensor.FlatAppearance.BorderSize = 5;
            this.BtnLimitSensor.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLimitSensor.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnLimitSensor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnLimitSensor.Location = new System.Drawing.Point(12, 76);
            this.BtnLimitSensor.Name = "BtnLimitSensor";
            this.BtnLimitSensor.Size = new System.Drawing.Size(128, 58);
            this.BtnLimitSensor.TabIndex = 12;
            this.BtnLimitSensor.Text = "Limit Sensor";
            this.BtnLimitSensor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnLimitSensor.Click += new System.EventHandler(this.BtnLimitSensor_Click);
            // 
            // BtnOriginReturn
            // 
            this.BtnOriginReturn.AutoEllipsis = true;
            this.BtnOriginReturn.BackColor = System.Drawing.SystemColors.Control;
            this.BtnOriginReturn.FlatAppearance.BorderSize = 5;
            this.BtnOriginReturn.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnOriginReturn.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnOriginReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnOriginReturn.Location = new System.Drawing.Point(12, 140);
            this.BtnOriginReturn.Name = "BtnOriginReturn";
            this.BtnOriginReturn.Size = new System.Drawing.Size(128, 58);
            this.BtnOriginReturn.TabIndex = 13;
            this.BtnOriginReturn.Text = "원점 복귀";
            this.BtnOriginReturn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnOriginReturn.Click += new System.EventHandler(this.BtnOriginReturn_Click);
            // 
            // BtnUnitInit
            // 
            this.BtnUnitInit.AutoEllipsis = true;
            this.BtnUnitInit.BackColor = System.Drawing.SystemColors.Control;
            this.BtnUnitInit.FlatAppearance.BorderSize = 5;
            this.BtnUnitInit.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnUnitInit.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnUnitInit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnUnitInit.Location = new System.Drawing.Point(12, 204);
            this.BtnUnitInit.Name = "BtnUnitInit";
            this.BtnUnitInit.Size = new System.Drawing.Size(128, 58);
            this.BtnUnitInit.TabIndex = 14;
            this.BtnUnitInit.Text = "Unit 초기화";
            this.BtnUnitInit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnUnitInit.Click += new System.EventHandler(this.BtnUnitInit_Click);
            // 
            // FormManualScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1278, 817);
            this.Controls.Add(this.BtnUnitInit);
            this.Controls.Add(this.BtnOriginReturn);
            this.Controls.Add(this.BtnLimitSensor);
            this.Controls.Add(this.BtnInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormManualScreen";
            this.Text = "Manual Screen";
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnInput;
        private Syncfusion.Windows.Forms.ButtonAdv BtnLimitSensor;
        private Syncfusion.Windows.Forms.ButtonAdv BtnOriginReturn;
        private Syncfusion.Windows.Forms.ButtonAdv BtnUnitInit;
    }
}