namespace LWDicer.UI
{
    partial class FormLogScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogScreen));
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle1 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            this.BtnExport = new System.Windows.Forms.Button();
            this.TitleCount = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelCount = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnSerch = new System.Windows.Forms.Button();
            this.DateEnd = new System.Windows.Forms.DateTimePicker();
            this.DateStart = new System.Windows.Forms.DateTimePicker();
            this.GridEvent = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            this.LabelTrouble = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            ((System.ComponentModel.ISupportInitialize)(this.GridEvent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnExport
            // 
            this.BtnExport.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExport.Image = ((System.Drawing.Image)(resources.GetObject("BtnExport.Image")));
            this.BtnExport.Location = new System.Drawing.Point(830, 12);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(129, 57);
            this.BtnExport.TabIndex = 780;
            this.BtnExport.Text = "  Export";
            this.BtnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExport.UseVisualStyleBackColor = true;
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // TitleCount
            // 
            this.TitleCount.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.LightSteelBlue);
            this.TitleCount.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.TitleCount.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.TitleCount.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TitleCount.ForeColor = System.Drawing.Color.Black;
            this.TitleCount.Location = new System.Drawing.Point(8, 35);
            this.TitleCount.Name = "TitleCount";
            this.TitleCount.Size = new System.Drawing.Size(97, 40);
            this.TitleCount.TabIndex = 779;
            this.TitleCount.Text = "검색 개수";
            this.TitleCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelCount
            // 
            this.LabelCount.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelCount.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelCount.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelCount.ForeColor = System.Drawing.Color.Black;
            this.LabelCount.Location = new System.Drawing.Point(107, 35);
            this.LabelCount.Name = "LabelCount";
            this.LabelCount.Size = new System.Drawing.Size(111, 40);
            this.LabelCount.TabIndex = 778;
            this.LabelCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnClear
            // 
            this.BtnClear.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnClear.Image = ((System.Drawing.Image)(resources.GetObject("BtnClear.Image")));
            this.BtnClear.Location = new System.Drawing.Point(699, 12);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(129, 57);
            this.BtnClear.TabIndex = 777;
            this.BtnClear.Text = "   Clear";
            this.BtnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(456, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 25);
            this.label1.TabIndex = 776;
            this.label1.Text = "~";
            // 
            // BtnSerch
            // 
            this.BtnSerch.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSerch.Image = ((System.Drawing.Image)(resources.GetObject("BtnSerch.Image")));
            this.BtnSerch.Location = new System.Drawing.Point(568, 12);
            this.BtnSerch.Name = "BtnSerch";
            this.BtnSerch.Size = new System.Drawing.Size(129, 57);
            this.BtnSerch.TabIndex = 775;
            this.BtnSerch.Text = " Search";
            this.BtnSerch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSerch.UseVisualStyleBackColor = true;
            this.BtnSerch.Click += new System.EventHandler(this.BtnSerch_Click);
            // 
            // DateEnd
            // 
            this.DateEnd.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.DateEnd.Location = new System.Drawing.Point(336, 46);
            this.DateEnd.Name = "DateEnd";
            this.DateEnd.Size = new System.Drawing.Size(223, 29);
            this.DateEnd.TabIndex = 774;
            this.DateEnd.ValueChanged += new System.EventHandler(this.DateEnd_ValueChanged);
            // 
            // DateStart
            // 
            this.DateStart.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.DateStart.Location = new System.Drawing.Point(228, 12);
            this.DateStart.Name = "DateStart";
            this.DateStart.Size = new System.Drawing.Size(223, 29);
            this.DateStart.TabIndex = 773;
            this.DateStart.ValueChanged += new System.EventHandler(this.DateStart_ValueChanged);
            // 
            // GridEvent
            // 
            this.GridEvent.ActivateCurrentCellBehavior = Syncfusion.Windows.Forms.Grid.GridCellActivateAction.None;
            this.GridEvent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridEvent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.GridEvent.Location = new System.Drawing.Point(4, 87);
            this.GridEvent.Name = "GridEvent";
            gridRangeStyle1.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle1.StyleInfo.Font.Bold = true;
            gridRangeStyle1.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle1.StyleInfo.Font.Italic = false;
            gridRangeStyle1.StyleInfo.Font.Size = 8.25F;
            gridRangeStyle1.StyleInfo.Font.Strikeout = false;
            gridRangeStyle1.StyleInfo.Font.Underline = false;
            gridRangeStyle1.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            this.GridEvent.RangeStyles.AddRange(new Syncfusion.Windows.Forms.Grid.GridRangeStyle[] {
            gridRangeStyle1});
            this.GridEvent.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridEvent.Size = new System.Drawing.Size(962, 721);
            this.GridEvent.SmartSizeBox = false;
            this.GridEvent.TabIndex = 772;
            this.GridEvent.UseRightToLeftCompatibleTextBox = true;
            this.GridEvent.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridEvent_CellClick);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel1.BackgroundImage")));
            this.gradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gradientPanel1.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel1.Controls.Add(this.LabelTrouble);
            this.gradientPanel1.Controls.Add(this.autoLabel1);
            this.gradientPanel1.Location = new System.Drawing.Point(970, 87);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(306, 323);
            this.gradientPanel1.TabIndex = 781;
            // 
            // LabelTrouble
            // 
            this.LabelTrouble.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelTrouble.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTrouble.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTrouble.Location = new System.Drawing.Point(8, 35);
            this.LabelTrouble.Name = "LabelTrouble";
            this.LabelTrouble.Padding = new System.Windows.Forms.Padding(10);
            this.LabelTrouble.Size = new System.Drawing.Size(291, 269);
            this.LabelTrouble.TabIndex = 31;
            this.LabelTrouble.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoLabel1
            // 
            this.autoLabel1.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel1.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel1.Location = new System.Drawing.Point(25, 4);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(127, 15);
            this.autoLabel1.TabIndex = 30;
            this.autoLabel1.Text = "Troubleshooting";
            // 
            // FormLogScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1278, 817);
            this.Controls.Add(this.gradientPanel1);
            this.Controls.Add(this.BtnExport);
            this.Controls.Add(this.TitleCount);
            this.Controls.Add(this.LabelCount);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnSerch);
            this.Controls.Add(this.DateEnd);
            this.Controls.Add(this.DateStart);
            this.Controls.Add(this.GridEvent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogScreen";
            this.Text = "Log Screen";
            this.Activated += new System.EventHandler(this.FormLogScreen_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.GridEvent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnExport;
        private Syncfusion.Windows.Forms.Tools.GradientLabel TitleCount;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelCount;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnSerch;
        private System.Windows.Forms.DateTimePicker DateEnd;
        private System.Windows.Forms.DateTimePicker DateStart;
        private Syncfusion.Windows.Forms.Grid.GridControl GridEvent;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTrouble;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
    }
}