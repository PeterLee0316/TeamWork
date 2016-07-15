namespace LWDicer.UI
{
    partial class FormWaferCassetteData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWaferCassetteData));
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.GridCtrl = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.ComboCassette = new System.Windows.Forms.ComboBox();
            this.LabelCassetteName = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(861, 497);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 758;
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
            this.BtnSave.Location = new System.Drawing.Point(731, 497);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(124, 61);
            this.BtnSave.TabIndex = 759;
            this.BtnSave.Text = " Save";
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // GridCtrl
            // 
            this.GridCtrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridCtrl.Location = new System.Drawing.Point(12, 59);
            this.GridCtrl.Name = "GridCtrl";
            this.GridCtrl.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridCtrl.Size = new System.Drawing.Size(973, 422);
            this.GridCtrl.SmartSizeBox = false;
            this.GridCtrl.TabIndex = 838;
            this.GridCtrl.UseRightToLeftCompatibleTextBox = true;
            this.GridCtrl.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridWaferframe_CellClick);
            // 
            // ComboCassette
            // 
            this.ComboCassette.DropDownHeight = 200;
            this.ComboCassette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboCassette.DropDownWidth = 260;
            this.ComboCassette.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ComboCassette.FormattingEnabled = true;
            this.ComboCassette.IntegralHeight = false;
            this.ComboCassette.Location = new System.Drawing.Point(190, 15);
            this.ComboCassette.Name = "ComboCassette";
            this.ComboCassette.Size = new System.Drawing.Size(314, 27);
            this.ComboCassette.TabIndex = 840;
            this.ComboCassette.SelectedIndexChanged += new System.EventHandler(this.ComboCassette_SelectedIndexChanged);
            // 
            // LabelCassetteName
            // 
            this.LabelCassetteName.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64))))));
            this.LabelCassetteName.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelCassetteName.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.LabelCassetteName.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelCassetteName.ForeColor = System.Drawing.Color.White;
            this.LabelCassetteName.Location = new System.Drawing.Point(10, 13);
            this.LabelCassetteName.Name = "LabelCassetteName";
            this.LabelCassetteName.Size = new System.Drawing.Size(174, 31);
            this.LabelCassetteName.TabIndex = 839;
            this.LabelCassetteName.Text = "Cassette Name";
            this.LabelCassetteName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormWaferCassetteData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 574);
            this.Controls.Add(this.ComboCassette);
            this.Controls.Add(this.LabelCassetteName);
            this.Controls.Add(this.GridCtrl);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormWaferCassetteData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wafer Cassette Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWaferCassetteData_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnSave;
        private Syncfusion.Windows.Forms.Grid.GridControl GridCtrl;
        private System.Windows.Forms.ComboBox ComboCassette;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelCassetteName;
    }
}