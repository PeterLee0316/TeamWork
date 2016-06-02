namespace LWDicer.UI
{
    partial class FormSystemData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSystemData));
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.LabelLanguageTitle = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelNameTitle = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelModelName = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.ComboLanguage = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(218, 153);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 750;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.Location = new System.Drawing.Point(88, 154);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(124, 61);
            this.BtnSave.TabIndex = 753;
            this.BtnSave.Text = " Save";
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // LabelLanguageTitle
            // 
            this.LabelLanguageTitle.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64))))));
            this.LabelLanguageTitle.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelLanguageTitle.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.LabelLanguageTitle.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelLanguageTitle.ForeColor = System.Drawing.Color.White;
            this.LabelLanguageTitle.Location = new System.Drawing.Point(12, 47);
            this.LabelLanguageTitle.Name = "LabelLanguageTitle";
            this.LabelLanguageTitle.Size = new System.Drawing.Size(158, 32);
            this.LabelLanguageTitle.TabIndex = 792;
            this.LabelLanguageTitle.Text = "Language";
            this.LabelLanguageTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelNameTitle
            // 
            this.LabelNameTitle.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0))))));
            this.LabelNameTitle.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelNameTitle.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.LabelNameTitle.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelNameTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LabelNameTitle.Location = new System.Drawing.Point(10, 9);
            this.LabelNameTitle.Name = "LabelNameTitle";
            this.LabelNameTitle.Size = new System.Drawing.Size(158, 32);
            this.LabelNameTitle.TabIndex = 791;
            this.LabelNameTitle.Text = "Model Name";
            this.LabelNameTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelModelName
            // 
            this.LabelModelName.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelModelName.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelModelName.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelModelName.ForeColor = System.Drawing.Color.Black;
            this.LabelModelName.Location = new System.Drawing.Point(174, 9);
            this.LabelModelName.Name = "LabelModelName";
            this.LabelModelName.Size = new System.Drawing.Size(260, 32);
            this.LabelModelName.TabIndex = 787;
            this.LabelModelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ComboLanguage
            // 
            this.ComboLanguage.DropDownHeight = 200;
            this.ComboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboLanguage.DropDownWidth = 260;
            this.ComboLanguage.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ComboLanguage.FormattingEnabled = true;
            this.ComboLanguage.IntegralHeight = false;
            this.ComboLanguage.Location = new System.Drawing.Point(176, 51);
            this.ComboLanguage.Name = "ComboLanguage";
            this.ComboLanguage.Size = new System.Drawing.Size(260, 24);
            this.ComboLanguage.TabIndex = 794;
            this.ComboLanguage.SelectedIndexChanged += new System.EventHandler(this.ComboLanguage_SelectedIndexChanged);
            // 
            // FormSystemData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 240);
            this.Controls.Add(this.ComboLanguage);
            this.Controls.Add(this.LabelLanguageTitle);
            this.Controls.Add(this.LabelNameTitle);
            this.Controls.Add(this.LabelModelName);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSystemData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSystemData_FormClosing);
            this.Load += new System.EventHandler(this.FormSystemData_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnSave;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelLanguageTitle;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelNameTitle;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelModelName;
        private System.Windows.Forms.ComboBox ComboLanguage;
    }
}