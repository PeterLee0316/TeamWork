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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogScreen));
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle1 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle2 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle3 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle4 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle5 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle6 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle7 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle8 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            this.BtnExport = new System.Windows.Forms.Button();
            this.TitleCount = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelCount = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnSerch = new System.Windows.Forms.Button();
            this.DateEnd = new System.Windows.Forms.DateTimePicker();
            this.DateStart = new System.Windows.Forms.DateTimePicker();
            this.GridCont = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.Image = new System.Windows.Forms.ImageList(this.components);
            this.BtnPageTop = new System.Windows.Forms.Button();
            this.BtnPageBot = new System.Windows.Forms.Button();
            this.BtnPageUp = new System.Windows.Forms.Button();
            this.BtnPageDown = new System.Windows.Forms.Button();
            this.ComboType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.GridCont)).BeginInit();
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
            // GridCont
            // 
            this.GridCont.ActivateCurrentCellBehavior = Syncfusion.Windows.Forms.Grid.GridCellActivateAction.None;
            this.GridCont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridCont.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.GridCont.Location = new System.Drawing.Point(9, 81);
            this.GridCont.Name = "GridCont";
            gridRangeStyle1.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle1.StyleInfo.Font.Bold = true;
            gridRangeStyle1.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle1.StyleInfo.Font.Italic = false;
            gridRangeStyle1.StyleInfo.Font.Size = 8.25F;
            gridRangeStyle1.StyleInfo.Font.Strikeout = false;
            gridRangeStyle1.StyleInfo.Font.Underline = false;
            gridRangeStyle1.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle2.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle2.StyleInfo.Font.Bold = true;
            gridRangeStyle2.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle2.StyleInfo.Font.Italic = false;
            gridRangeStyle2.StyleInfo.Font.Size = 8.25F;
            gridRangeStyle2.StyleInfo.Font.Strikeout = false;
            gridRangeStyle2.StyleInfo.Font.Underline = false;
            gridRangeStyle2.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle3.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle3.StyleInfo.Font.Bold = true;
            gridRangeStyle3.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle3.StyleInfo.Font.Italic = false;
            gridRangeStyle3.StyleInfo.Font.Size = 8.25F;
            gridRangeStyle3.StyleInfo.Font.Strikeout = false;
            gridRangeStyle3.StyleInfo.Font.Underline = false;
            gridRangeStyle3.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle4.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle4.StyleInfo.Font.Bold = true;
            gridRangeStyle4.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle4.StyleInfo.Font.Italic = false;
            gridRangeStyle4.StyleInfo.Font.Size = 8.25F;
            gridRangeStyle4.StyleInfo.Font.Strikeout = false;
            gridRangeStyle4.StyleInfo.Font.Underline = false;
            gridRangeStyle4.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle5.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle5.StyleInfo.Font.Bold = true;
            gridRangeStyle5.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle5.StyleInfo.Font.Italic = false;
            gridRangeStyle5.StyleInfo.Font.Size = 8.25F;
            gridRangeStyle5.StyleInfo.Font.Strikeout = false;
            gridRangeStyle5.StyleInfo.Font.Underline = false;
            gridRangeStyle5.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle6.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle6.StyleInfo.Font.Bold = true;
            gridRangeStyle6.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle6.StyleInfo.Font.Italic = false;
            gridRangeStyle6.StyleInfo.Font.Size = 8.25F;
            gridRangeStyle6.StyleInfo.Font.Strikeout = false;
            gridRangeStyle6.StyleInfo.Font.Underline = false;
            gridRangeStyle6.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle7.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle7.StyleInfo.Font.Bold = true;
            gridRangeStyle7.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle7.StyleInfo.Font.Italic = false;
            gridRangeStyle7.StyleInfo.Font.Size = 8.25F;
            gridRangeStyle7.StyleInfo.Font.Strikeout = false;
            gridRangeStyle7.StyleInfo.Font.Underline = false;
            gridRangeStyle7.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle8.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle8.StyleInfo.Font.Bold = true;
            gridRangeStyle8.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle8.StyleInfo.Font.Italic = false;
            gridRangeStyle8.StyleInfo.Font.Size = 8.25F;
            gridRangeStyle8.StyleInfo.Font.Strikeout = false;
            gridRangeStyle8.StyleInfo.Font.Underline = false;
            gridRangeStyle8.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            this.GridCont.RangeStyles.AddRange(new Syncfusion.Windows.Forms.Grid.GridRangeStyle[] {
            gridRangeStyle1,
            gridRangeStyle2,
            gridRangeStyle3,
            gridRangeStyle4,
            gridRangeStyle5,
            gridRangeStyle6,
            gridRangeStyle7,
            gridRangeStyle8});
            this.GridCont.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridCont.Size = new System.Drawing.Size(1177, 733);
            this.GridCont.SmartSizeBox = false;
            this.GridCont.TabIndex = 772;
            this.GridCont.UseRightToLeftCompatibleTextBox = true;
            this.GridCont.CellDoubleClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridEvent_CellDoubleClick);
            // 
            // Image
            // 
            this.Image.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Image.ImageStream")));
            this.Image.TransparentColor = System.Drawing.Color.Transparent;
            this.Image.Images.SetKeyName(0, "Led_Off.png");
            this.Image.Images.SetKeyName(1, "Led_On.png");
            // 
            // BtnPageTop
            // 
            this.BtnPageTop.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnPageTop.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPageTop.Location = new System.Drawing.Point(1190, 93);
            this.BtnPageTop.Name = "BtnPageTop";
            this.BtnPageTop.Size = new System.Drawing.Size(83, 95);
            this.BtnPageTop.TabIndex = 785;
            this.BtnPageTop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnPageTop.UseVisualStyleBackColor = false;
            this.BtnPageTop.Click += new System.EventHandler(this.BtnPageTop_Click);
            // 
            // BtnPageBot
            // 
            this.BtnPageBot.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnPageBot.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPageBot.Location = new System.Drawing.Point(1190, 717);
            this.BtnPageBot.Name = "BtnPageBot";
            this.BtnPageBot.Size = new System.Drawing.Size(83, 95);
            this.BtnPageBot.TabIndex = 786;
            this.BtnPageBot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnPageBot.UseVisualStyleBackColor = false;
            this.BtnPageBot.Click += new System.EventHandler(this.BtnPageBot_Click);
            // 
            // BtnPageUp
            // 
            this.BtnPageUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnPageUp.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPageUp.Location = new System.Drawing.Point(1190, 189);
            this.BtnPageUp.Name = "BtnPageUp";
            this.BtnPageUp.Size = new System.Drawing.Size(83, 263);
            this.BtnPageUp.TabIndex = 787;
            this.BtnPageUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnPageUp.UseVisualStyleBackColor = false;
            this.BtnPageUp.Click += new System.EventHandler(this.BtnPageUp_Click);
            // 
            // BtnPageDown
            // 
            this.BtnPageDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnPageDown.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPageDown.Location = new System.Drawing.Point(1190, 453);
            this.BtnPageDown.Name = "BtnPageDown";
            this.BtnPageDown.Size = new System.Drawing.Size(83, 263);
            this.BtnPageDown.TabIndex = 788;
            this.BtnPageDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnPageDown.UseVisualStyleBackColor = false;
            this.BtnPageDown.Click += new System.EventHandler(this.BtnPageDown_Click);
            // 
            // ComboType
            // 
            this.ComboType.DropDownHeight = 200;
            this.ComboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboType.DropDownWidth = 260;
            this.ComboType.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ComboType.FormattingEnabled = true;
            this.ComboType.IntegralHeight = false;
            this.ComboType.Location = new System.Drawing.Point(12, 5);
            this.ComboType.Name = "ComboType";
            this.ComboType.Size = new System.Drawing.Size(210, 27);
            this.ComboType.TabIndex = 843;
            this.ComboType.SelectedIndexChanged += new System.EventHandler(this.ComboType_SelectedIndexChanged);
            // 
            // FormLogScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1278, 817);
            this.Controls.Add(this.ComboType);
            this.Controls.Add(this.BtnPageDown);
            this.Controls.Add(this.BtnPageUp);
            this.Controls.Add(this.BtnPageBot);
            this.Controls.Add(this.BtnPageTop);
            this.Controls.Add(this.BtnExport);
            this.Controls.Add(this.TitleCount);
            this.Controls.Add(this.LabelCount);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnSerch);
            this.Controls.Add(this.DateEnd);
            this.Controls.Add(this.DateStart);
            this.Controls.Add(this.GridCont);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogScreen";
            this.Text = "Log Screen";
            this.Activated += new System.EventHandler(this.FormLogScreen_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.GridCont)).EndInit();
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
        private Syncfusion.Windows.Forms.Grid.GridControl GridCont;
        private System.Windows.Forms.ImageList Image;
        private System.Windows.Forms.Button BtnPageTop;
        private System.Windows.Forms.Button BtnPageBot;
        private System.Windows.Forms.Button BtnPageUp;
        private System.Windows.Forms.Button BtnPageDown;
        private System.Windows.Forms.ComboBox ComboType;
    }
}