﻿namespace LWDicer.UI
{
    partial class FormCleanerData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCleanerData));
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle1 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle2 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.GridCtrl = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.gradientLabel17 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.autoLabel5 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.autoLabel4 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.LabelStroke = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.autoLabel14 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnSave
            // 
            this.BtnSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.Location = new System.Drawing.Point(97, 589);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(124, 61);
            this.BtnSave.TabIndex = 754;
            this.BtnSave.Text = " Save";
            this.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(227, 589);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 753;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // GridCtrl
            // 
            this.GridCtrl.ActivateCurrentCellBehavior = Syncfusion.Windows.Forms.Grid.GridCellActivateAction.None;
            this.GridCtrl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GridCtrl.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            this.GridCtrl.Location = new System.Drawing.Point(461, 50);
            this.GridCtrl.Name = "GridCtrl";
            gridRangeStyle1.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle1.StyleInfo.Font.Bold = true;
            gridRangeStyle1.StyleInfo.Font.Facename = "맑은 고딕";
            gridRangeStyle1.StyleInfo.Font.Italic = false;
            gridRangeStyle1.StyleInfo.Font.Size = 11.25F;
            gridRangeStyle1.StyleInfo.Font.Strikeout = false;
            gridRangeStyle1.StyleInfo.Font.Underline = false;
            gridRangeStyle1.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle2.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle2.StyleInfo.Font.Bold = true;
            gridRangeStyle2.StyleInfo.Font.Facename = "맑은 고딕";
            gridRangeStyle2.StyleInfo.Font.Italic = false;
            gridRangeStyle2.StyleInfo.Font.Size = 11.25F;
            gridRangeStyle2.StyleInfo.Font.Strikeout = false;
            gridRangeStyle2.StyleInfo.Font.Underline = false;
            gridRangeStyle2.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            this.GridCtrl.RangeStyles.AddRange(new Syncfusion.Windows.Forms.Grid.GridRangeStyle[] {
            gridRangeStyle1,
            gridRangeStyle2});
            this.GridCtrl.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridCtrl.Size = new System.Drawing.Size(476, 600);
            this.GridCtrl.SmartSizeBox = false;
            this.GridCtrl.TabIndex = 755;
            this.GridCtrl.UseRightToLeftCompatibleTextBox = true;
            this.GridCtrl.CurrentCellShowedDropDown += new System.EventHandler(this.GridSpinner_CurrentCellShowedDropDown);
            this.GridCtrl.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridSpinner_CellClick);
            // 
            // gradientLabel17
            // 
            this.gradientLabel17.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel17.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel17.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel17.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel17.Location = new System.Drawing.Point(461, 9);
            this.gradientLabel17.Name = "gradientLabel17";
            this.gradientLabel17.Size = new System.Drawing.Size(476, 38);
            this.gradientLabel17.TabIndex = 931;
            this.gradientLabel17.Text = "Cleaning Sequence Operation";
            this.gradientLabel17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoLabel5
            // 
            this.autoLabel5.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel5.ForeColor = System.Drawing.Color.Black;
            this.autoLabel5.Location = new System.Drawing.Point(21, 66);
            this.autoLabel5.Name = "autoLabel5";
            this.autoLabel5.Size = new System.Drawing.Size(260, 21);
            this.autoLabel5.TabIndex = 945;
            this.autoLabel5.Text = "Washing Stroke  :  Work Size + ";
            // 
            // autoLabel4
            // 
            this.autoLabel4.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel4.ForeColor = System.Drawing.Color.Black;
            this.autoLabel4.Location = new System.Drawing.Point(396, 66);
            this.autoLabel4.Name = "autoLabel4";
            this.autoLabel4.Size = new System.Drawing.Size(40, 21);
            this.autoLabel4.TabIndex = 938;
            this.autoLabel4.Text = "mm";
            // 
            // LabelStroke
            // 
            this.LabelStroke.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelStroke.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelStroke.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelStroke.ForeColor = System.Drawing.Color.Black;
            this.LabelStroke.Location = new System.Drawing.Point(282, 56);
            this.LabelStroke.Name = "LabelStroke";
            this.LabelStroke.Size = new System.Drawing.Size(110, 41);
            this.LabelStroke.TabIndex = 932;
            this.LabelStroke.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelStroke.Click += new System.EventHandler(this.LabelStroke_Click);
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel2.BackgroundImage")));
            this.gradientPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gradientPanel2.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientPanel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel2.Controls.Add(this.autoLabel14);
            this.gradientPanel2.Location = new System.Drawing.Point(15, 9);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(429, 387);
            this.gradientPanel2.TabIndex = 952;
            // 
            // autoLabel14
            // 
            this.autoLabel14.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel14.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel14.Location = new System.Drawing.Point(162, 6);
            this.autoLabel14.Name = "autoLabel14";
            this.autoLabel14.Size = new System.Drawing.Size(111, 15);
            this.autoLabel14.TabIndex = 31;
            this.autoLabel14.Text = "Cleaning Data";
            // 
            // FormCleanerData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 672);
            this.Controls.Add(this.autoLabel5);
            this.Controls.Add(this.autoLabel4);
            this.Controls.Add(this.LabelStroke);
            this.Controls.Add(this.gradientPanel2);
            this.Controls.Add(this.gradientLabel17);
            this.Controls.Add(this.GridCtrl);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormCleanerData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCleanerData";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCleanerData_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.GridCtrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel2)).EndInit();
            this.gradientPanel2.ResumeLayout(false);
            this.gradientPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnExit;
        private Syncfusion.Windows.Forms.Grid.GridControl GridCtrl;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel17;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel5;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel4;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelStroke;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel14;
    }
}