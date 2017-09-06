namespace Core.UI
{
    partial class FormVacuumData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVacuumData));
            this.BtnSave = new System.Windows.Forms.Button();
            this.GridCtrl = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.BtnExit = new System.Windows.Forms.Button();
            this.checkBoxAll = new Syncfusion.Windows.Forms.Tools.CheckBoxAdv();
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxAll)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnSave
            // 
            this.BtnSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.Location = new System.Drawing.Point(318, 301);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(124, 61);
            this.BtnSave.TabIndex = 755;
            this.BtnSave.Text = " Save";
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // GridCtrl
            // 
            this.GridCtrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridCtrl.Location = new System.Drawing.Point(12, 12);
            this.GridCtrl.Name = "GridCtrl";
            this.GridCtrl.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridCtrl.Size = new System.Drawing.Size(562, 274);
            this.GridCtrl.SmartSizeBox = false;
            this.GridCtrl.TabIndex = 754;
            this.GridCtrl.UseRightToLeftCompatibleTextBox = true;
            this.GridCtrl.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridVacuumData_CellClick);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(448, 301);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 753;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.Border3DStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            this.checkBoxAll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkBoxAll.CheckedImage = ((System.Drawing.Image)(resources.GetObject("checkBoxAll.CheckedImage")));
            this.checkBoxAll.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBoxAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxAll.ImageCheckBox = true;
            this.checkBoxAll.ImageCheckBoxSize = new System.Drawing.Size(35, 35);
            this.checkBoxAll.Location = new System.Drawing.Point(12, 301);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(124, 61);
            this.checkBoxAll.Style = Syncfusion.Windows.Forms.Tools.CheckBoxAdvStyle.Office2007;
            this.checkBoxAll.TabIndex = 756;
            this.checkBoxAll.Text = "Input All";
            this.checkBoxAll.ThemesEnabled = false;
            this.checkBoxAll.UncheckedImage = ((System.Drawing.Image)(resources.GetObject("checkBoxAll.UncheckedImage")));
            // 
            // FormVacuumData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 376);
            this.Controls.Add(this.checkBoxAll);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.GridCtrl);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormVacuumData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vacuum Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormVacuumData_FormClosing);
            this.Load += new System.EventHandler(this.FormVacuumData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxAll)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private Syncfusion.Windows.Forms.Grid.GridControl GridCtrl;
        private System.Windows.Forms.Button BtnExit;
        private Syncfusion.Windows.Forms.Tools.CheckBoxAdv checkBoxAll;
    }
}