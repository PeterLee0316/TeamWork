namespace Core.UI
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
            this.BtnExport = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnSerch = new System.Windows.Forms.Button();
            this.DateEnd = new System.Windows.Forms.DateTimePicker();
            this.DateStart = new System.Windows.Forms.DateTimePicker();
            this.Image = new System.Windows.Forms.ImageList(this.components);
            this.BtnPageTop = new System.Windows.Forms.Button();
            this.BtnPageBot = new System.Windows.Forms.Button();
            this.BtnPageUp = new System.Windows.Forms.Button();
            this.BtnPageDown = new System.Windows.Forms.Button();
            this.ComboType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LabelCount = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // BtnExport
            // 
            this.BtnExport.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExport.Image = ((System.Drawing.Image)(resources.GetObject("BtnExport.Image")));
            this.BtnExport.Location = new System.Drawing.Point(1507, 20);
            this.BtnExport.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(185, 86);
            this.BtnExport.TabIndex = 780;
            this.BtnExport.Text = "  Export";
            this.BtnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExport.UseVisualStyleBackColor = true;
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnClear.Image = ((System.Drawing.Image)(resources.GetObject("BtnClear.Image")));
            this.BtnClear.Location = new System.Drawing.Point(1320, 20);
            this.BtnClear.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(185, 86);
            this.BtnClear.TabIndex = 777;
            this.BtnClear.Text = "   Clear";
            this.BtnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(747, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 40);
            this.label1.TabIndex = 776;
            this.label1.Text = "~";
            // 
            // BtnSerch
            // 
            this.BtnSerch.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSerch.Image = ((System.Drawing.Image)(resources.GetObject("BtnSerch.Image")));
            this.BtnSerch.Location = new System.Drawing.Point(1133, 20);
            this.BtnSerch.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnSerch.Name = "BtnSerch";
            this.BtnSerch.Size = new System.Drawing.Size(185, 86);
            this.BtnSerch.TabIndex = 775;
            this.BtnSerch.Text = " Search";
            this.BtnSerch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnSerch.UseVisualStyleBackColor = true;
            this.BtnSerch.Click += new System.EventHandler(this.BtnSerch_Click);
            // 
            // DateEnd
            // 
            this.DateEnd.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.DateEnd.Location = new System.Drawing.Point(793, 40);
            this.DateEnd.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.DateEnd.Name = "DateEnd";
            this.DateEnd.Size = new System.Drawing.Size(317, 39);
            this.DateEnd.TabIndex = 774;
            this.DateEnd.ValueChanged += new System.EventHandler(this.DateEnd_ValueChanged);
            // 
            // DateStart
            // 
            this.DateStart.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.DateStart.Location = new System.Drawing.Point(420, 40);
            this.DateStart.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.DateStart.Name = "DateStart";
            this.DateStart.Size = new System.Drawing.Size(317, 39);
            this.DateStart.TabIndex = 773;
            this.DateStart.ValueChanged += new System.EventHandler(this.DateStart_ValueChanged);
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
            this.BtnPageTop.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPageTop.Image = ((System.Drawing.Image)(resources.GetObject("BtnPageTop.Image")));
            this.BtnPageTop.Location = new System.Drawing.Point(1700, 122);
            this.BtnPageTop.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnPageTop.Name = "BtnPageTop";
            this.BtnPageTop.Size = new System.Drawing.Size(118, 150);
            this.BtnPageTop.TabIndex = 785;
            this.BtnPageTop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnPageTop.UseVisualStyleBackColor = false;
            this.BtnPageTop.Click += new System.EventHandler(this.BtnPageTop_Click);
            // 
            // BtnPageBot
            // 
            this.BtnPageBot.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnPageBot.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPageBot.Image = ((System.Drawing.Image)(resources.GetObject("BtnPageBot.Image")));
            this.BtnPageBot.Location = new System.Drawing.Point(1700, 1071);
            this.BtnPageBot.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnPageBot.Name = "BtnPageBot";
            this.BtnPageBot.Size = new System.Drawing.Size(118, 150);
            this.BtnPageBot.TabIndex = 786;
            this.BtnPageBot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnPageBot.UseVisualStyleBackColor = false;
            this.BtnPageBot.Click += new System.EventHandler(this.BtnPageBot_Click);
            // 
            // BtnPageUp
            // 
            this.BtnPageUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnPageUp.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPageUp.Image = ((System.Drawing.Image)(resources.GetObject("BtnPageUp.Image")));
            this.BtnPageUp.Location = new System.Drawing.Point(1700, 273);
            this.BtnPageUp.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnPageUp.Name = "BtnPageUp";
            this.BtnPageUp.Size = new System.Drawing.Size(118, 398);
            this.BtnPageUp.TabIndex = 787;
            this.BtnPageUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnPageUp.UseVisualStyleBackColor = false;
            this.BtnPageUp.Click += new System.EventHandler(this.BtnPageUp_Click);
            // 
            // BtnPageDown
            // 
            this.BtnPageDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnPageDown.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnPageDown.Image = ((System.Drawing.Image)(resources.GetObject("BtnPageDown.Image")));
            this.BtnPageDown.Location = new System.Drawing.Point(1700, 672);
            this.BtnPageDown.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnPageDown.Name = "BtnPageDown";
            this.BtnPageDown.Size = new System.Drawing.Size(118, 398);
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
            this.ComboType.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ComboType.FormattingEnabled = true;
            this.ComboType.IntegralHeight = false;
            this.ComboType.Location = new System.Drawing.Point(17, 8);
            this.ComboType.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ComboType.Name = "ComboType";
            this.ComboType.Size = new System.Drawing.Size(292, 37);
            this.ComboType.TabIndex = 843;
            this.ComboType.SelectedIndexChanged += new System.EventHandler(this.ComboType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightGreen;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 57);
            this.label2.TabIndex = 844;
            this.label2.Text = "Count";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelCount
            // 
            this.LabelCount.BackColor = System.Drawing.Color.White;
            this.LabelCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabelCount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LabelCount.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCount.Location = new System.Drawing.Point(155, 55);
            this.LabelCount.Name = "LabelCount";
            this.LabelCount.Size = new System.Drawing.Size(163, 57);
            this.LabelCount.TabIndex = 845;
            this.LabelCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Location = new System.Drawing.Point(17, 115);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1675, 1106);
            this.panel1.TabIndex = 846;
            // 
            // FormLogScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(2738, 1347);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LabelCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ComboType);
            this.Controls.Add(this.BtnPageDown);
            this.Controls.Add(this.BtnPageUp);
            this.Controls.Add(this.BtnPageBot);
            this.Controls.Add(this.BtnPageTop);
            this.Controls.Add(this.BtnExport);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnSerch);
            this.Controls.Add(this.DateEnd);
            this.Controls.Add(this.DateStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "FormLogScreen";
            this.Text = "Log Screen";
            this.Activated += new System.EventHandler(this.FormLogScreen_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnExport;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnSerch;
        private System.Windows.Forms.DateTimePicker DateEnd;
        private System.Windows.Forms.DateTimePicker DateStart;
        private System.Windows.Forms.ImageList Image;
        private System.Windows.Forms.Button BtnPageTop;
        private System.Windows.Forms.Button BtnPageBot;
        private System.Windows.Forms.Button BtnPageUp;
        private System.Windows.Forms.Button BtnPageDown;
        private System.Windows.Forms.ComboBox ComboType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LabelCount;
        private System.Windows.Forms.Panel panel1;
    }
}