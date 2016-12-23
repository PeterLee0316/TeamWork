namespace LWDicer.UI
{
    partial class FormPolygonSelectPara
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPolygonSelectPara));
            this.btnEsc = new System.Windows.Forms.Button();
            this.btnIsnSelect = new System.Windows.Forms.Button();
            this.btnCsnSelect = new System.Windows.Forms.Button();
            this.btnConfigSelect = new System.Windows.Forms.Button();
            this.btnConfigSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEsc
            // 
            this.btnEsc.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEsc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnEsc.Image = global::LWDicer.Properties.Resources.Exit;
            this.btnEsc.Location = new System.Drawing.Point(272, 12);
            this.btnEsc.Name = "btnEsc";
            this.btnEsc.Size = new System.Drawing.Size(87, 127);
            this.btnEsc.TabIndex = 757;
            this.btnEsc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEsc.UseVisualStyleBackColor = true;
            this.btnEsc.Click += new System.EventHandler(this.btnEsc_Click);
            // 
            // btnIsnSelect
            // 
            this.btnIsnSelect.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnIsnSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnIsnSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnIsnSelect.Image")));
            this.btnIsnSelect.Location = new System.Drawing.Point(142, 79);
            this.btnIsnSelect.Name = "btnIsnSelect";
            this.btnIsnSelect.Size = new System.Drawing.Size(124, 61);
            this.btnIsnSelect.TabIndex = 756;
            this.btnIsnSelect.Text = "Isn.ini";
            this.btnIsnSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIsnSelect.UseVisualStyleBackColor = true;
            this.btnIsnSelect.Click += new System.EventHandler(this.btnIsnSelect_Click);
            // 
            // btnCsnSelect
            // 
            this.btnCsnSelect.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCsnSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnCsnSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnCsnSelect.Image")));
            this.btnCsnSelect.Location = new System.Drawing.Point(12, 79);
            this.btnCsnSelect.Name = "btnCsnSelect";
            this.btnCsnSelect.Size = new System.Drawing.Size(124, 61);
            this.btnCsnSelect.TabIndex = 755;
            this.btnCsnSelect.Text = "Cns.ini";
            this.btnCsnSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCsnSelect.UseVisualStyleBackColor = true;
            this.btnCsnSelect.Click += new System.EventHandler(this.btnCsnSelect_Click);
            // 
            // btnConfigSelect
            // 
            this.btnConfigSelect.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConfigSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnConfigSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnConfigSelect.Image")));
            this.btnConfigSelect.Location = new System.Drawing.Point(142, 12);
            this.btnConfigSelect.Name = "btnConfigSelect";
            this.btnConfigSelect.Size = new System.Drawing.Size(124, 61);
            this.btnConfigSelect.TabIndex = 754;
            this.btnConfigSelect.Text = "Job.ini";
            this.btnConfigSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConfigSelect.UseVisualStyleBackColor = true;
            this.btnConfigSelect.Click += new System.EventHandler(this.btnConfigSelect_Click);
            // 
            // btnConfigSave
            // 
            this.btnConfigSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConfigSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnConfigSave.Image = global::LWDicer.Properties.Resources.import;
            this.btnConfigSave.Location = new System.Drawing.Point(12, 12);
            this.btnConfigSave.Name = "btnConfigSave";
            this.btnConfigSave.Size = new System.Drawing.Size(124, 61);
            this.btnConfigSave.TabIndex = 753;
            this.btnConfigSave.Text = "Config";
            this.btnConfigSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConfigSave.UseVisualStyleBackColor = true;
            this.btnConfigSave.Click += new System.EventHandler(this.btnConfigSave_Click);
            // 
            // FormPolygonSelectPara
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 151);
            this.ControlBox = false;
            this.Controls.Add(this.btnEsc);
            this.Controls.Add(this.btnIsnSelect);
            this.Controls.Add(this.btnCsnSelect);
            this.Controls.Add(this.btnConfigSelect);
            this.Controls.Add(this.btnConfigSave);
            this.Location = new System.Drawing.Point(1200, 0);
            this.Name = "FormPolygonSelectPara";
            this.Text = "FormPolygonSelectPara";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConfigSave;
        private System.Windows.Forms.Button btnConfigSelect;
        private System.Windows.Forms.Button btnCsnSelect;
        private System.Windows.Forms.Button btnIsnSelect;
        private System.Windows.Forms.Button btnEsc;
    }
}