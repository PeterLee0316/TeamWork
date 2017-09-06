namespace Core.UI
{
    partial class FormVisionData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVisionData));
            this.BtnExit = new System.Windows.Forms.Button();
            this.GridCtrl = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.btnCameraDataLoad = new System.Windows.Forms.Button();
            this.btnCameraDataSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(1124, 811);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 749;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // GridCtrl
            // 
            this.GridCtrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridCtrl.Location = new System.Drawing.Point(12, 74);
            this.GridCtrl.Name = "GridCtrl";
            this.GridCtrl.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridCtrl.Size = new System.Drawing.Size(682, 345);
            this.GridCtrl.SmartSizeBox = false;
            this.GridCtrl.TabIndex = 839;
            this.GridCtrl.UseRightToLeftCompatibleTextBox = true;
            this.GridCtrl.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridCtrl_CellClick);
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel1.Location = new System.Drawing.Point(12, 20);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(682, 51);
            this.gradientLabel1.TabIndex = 939;
            this.gradientLabel1.Text = "카메라  정보";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCameraDataLoad
            // 
            this.btnCameraDataLoad.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCameraDataLoad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            //this.btnCameraDataLoad.Image = global::Core.Properties.Resources.export;
            this.btnCameraDataLoad.Location = new System.Drawing.Point(700, 20);
            this.btnCameraDataLoad.Name = "btnCameraDataLoad";
            this.btnCameraDataLoad.Size = new System.Drawing.Size(130, 61);
            this.btnCameraDataLoad.TabIndex = 963;
            this.btnCameraDataLoad.Text = "Data Reload";
            this.btnCameraDataLoad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCameraDataLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCameraDataLoad.UseVisualStyleBackColor = true;
            this.btnCameraDataLoad.Click += new System.EventHandler(this.btnCameraDataLoad_Click);
            // 
            // btnCameraDataSave
            // 
            this.btnCameraDataSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCameraDataSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnCameraDataSave.Image = ((System.Drawing.Image)(resources.GetObject("btnCameraDataSave.Image")));
            this.btnCameraDataSave.Location = new System.Drawing.Point(700, 87);
            this.btnCameraDataSave.Name = "btnCameraDataSave";
            this.btnCameraDataSave.Size = new System.Drawing.Size(130, 61);
            this.btnCameraDataSave.TabIndex = 962;
            this.btnCameraDataSave.Text = "Data Save";
            this.btnCameraDataSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCameraDataSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCameraDataSave.UseVisualStyleBackColor = true;
            this.btnCameraDataSave.Click += new System.EventHandler(this.btnCameraDataSave_Click);
            // 
            // FormVisionData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 884);
            this.Controls.Add(this.btnCameraDataLoad);
            this.Controls.Add(this.btnCameraDataSave);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.GridCtrl);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormVisionData";
            this.Text = "Vision Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormVisionData_FormClosing);
            this.Load += new System.EventHandler(this.FormVisionData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private Syncfusion.Windows.Forms.Grid.GridControl GridCtrl;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private System.Windows.Forms.Button btnCameraDataLoad;
        private System.Windows.Forms.Button btnCameraDataSave;
    }
}