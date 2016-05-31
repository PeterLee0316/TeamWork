namespace LWDicer.UI
{
    partial class FormCylinderData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCylinderData));
            this.BtnImageDataSave = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.GridCylinderData = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.BtnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GridCylinderData)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnImageDataSave
            // 
            this.BtnImageDataSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnImageDataSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnImageDataSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnImageDataSave.Image")));
            this.BtnImageDataSave.Location = new System.Drawing.Point(568, 412);
            this.BtnImageDataSave.Name = "BtnImageDataSave";
            this.BtnImageDataSave.Size = new System.Drawing.Size(124, 61);
            this.BtnImageDataSave.TabIndex = 752;
            this.BtnImageDataSave.Text = " Save";
            this.BtnImageDataSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnImageDataSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnImageDataSave.UseVisualStyleBackColor = true;
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(358, 405);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 749;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // GridCylinderData
            // 
            this.GridCylinderData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridCylinderData.Location = new System.Drawing.Point(12, 12);
            this.GridCylinderData.Name = "GridCylinderData";
            this.GridCylinderData.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridCylinderData.Size = new System.Drawing.Size(682, 376);
            this.GridCylinderData.SmartSizeBox = false;
            this.GridCylinderData.TabIndex = 750;
            this.GridCylinderData.UseRightToLeftCompatibleTextBox = true;
            this.GridCylinderData.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridCylinderData_CellClick);
            // 
            // BtnSave
            // 
            this.BtnSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.Location = new System.Drawing.Point(228, 405);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(124, 61);
            this.BtnSave.TabIndex = 752;
            this.BtnSave.Text = " Save";
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // FormCylinderData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 479);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.GridCylinderData);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormCylinderData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cylinder Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCylinderData_FormClosing);
            this.Load += new System.EventHandler(this.FormCylinderData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridCylinderData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnImageDataSave;
        private System.Windows.Forms.Button BtnExit;
        private Syncfusion.Windows.Forms.Grid.GridControl GridCylinderData;
        private System.Windows.Forms.Button BtnSave;
    }
}