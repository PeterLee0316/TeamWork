namespace LWDicer.UI
{
    partial class FormTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTest));
            this.LabelTest1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.TextTest1 = new System.Windows.Forms.TextBox();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.TextAlarmIndex = new System.Windows.Forms.TextBox();
            this.BtnCancel = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnOK = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv1 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.comboControll = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.comboProcess = new Syncfusion.Windows.Forms.Tools.ComboBoxAdv();
            this.autoLabel1 = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            this.gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            ((System.ComponentModel.ISupportInitialize)(this.comboControll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelTest1
            // 
            this.LabelTest1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelTest1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTest1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTest1.Location = new System.Drawing.Point(25, 216);
            this.LabelTest1.Name = "LabelTest1";
            this.LabelTest1.Size = new System.Drawing.Size(175, 36);
            this.LabelTest1.TabIndex = 25;
            this.LabelTest1.Text = "GetServoStatus";
            this.LabelTest1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelTest1.Click += new System.EventHandler(this.LabelTest1_Click);
            // 
            // TextTest1
            // 
            this.TextTest1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextTest1.Location = new System.Drawing.Point(208, 216);
            this.TextTest1.Margin = new System.Windows.Forms.Padding(5);
            this.TextTest1.Name = "TextTest1";
            this.TextTest1.Size = new System.Drawing.Size(145, 25);
            this.TextTest1.TabIndex = 18;
            this.TextTest1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.Location = new System.Drawing.Point(9, 61);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(122, 40);
            this.gradientLabel1.TabIndex = 23;
            this.gradientLabel1.Text = "Alarm (CtrlLayer + TrsLayerl)";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextAlarmIndex
            // 
            this.TextAlarmIndex.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextAlarmIndex.Location = new System.Drawing.Point(137, 61);
            this.TextAlarmIndex.Margin = new System.Windows.Forms.Padding(5);
            this.TextAlarmIndex.Name = "TextAlarmIndex";
            this.TextAlarmIndex.Size = new System.Drawing.Size(92, 25);
            this.TextAlarmIndex.TabIndex = 17;
            this.TextAlarmIndex.Text = "1";
            this.TextAlarmIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCancel.ForeColor = System.Drawing.Color.Navy;
            this.BtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("BtnCancel.Image")));
            this.BtnCancel.Location = new System.Drawing.Point(480, 190);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(116, 62);
            this.BtnCancel.TabIndex = 22;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOK
            // 
            this.BtnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnOK.CustomManagedColor = System.Drawing.Color.White;
            this.BtnOK.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnOK.ForeColor = System.Drawing.Color.Navy;
            this.BtnOK.Image = ((System.Drawing.Image)(resources.GetObject("BtnOK.Image")));
            this.BtnOK.Location = new System.Drawing.Point(361, 190);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(116, 62);
            this.BtnOK.TabIndex = 21;
            this.BtnOK.Text = "OK";
            this.BtnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnOK.UseVisualStyleBackColor = true;
            // 
            // buttonAdv1
            // 
            this.buttonAdv1.Location = new System.Drawing.Point(264, 38);
            this.buttonAdv1.Name = "buttonAdv1";
            this.buttonAdv1.Size = new System.Drawing.Size(127, 36);
            this.buttonAdv1.TabIndex = 26;
            this.buttonAdv1.Text = "Generate Alarm";
            this.buttonAdv1.Click += new System.EventHandler(this.buttonAdv1_Click);
            // 
            // comboControll
            // 
            this.comboControll.Location = new System.Drawing.Point(9, 38);
            this.comboControll.Name = "comboControll";
            this.comboControll.Size = new System.Drawing.Size(122, 20);
            this.comboControll.TabIndex = 28;
            this.comboControll.Text = "comboBoxAdv1";
            // 
            // comboProcess
            // 
            this.comboProcess.Location = new System.Drawing.Point(137, 38);
            this.comboProcess.Name = "comboProcess";
            this.comboProcess.Size = new System.Drawing.Size(121, 20);
            this.comboProcess.TabIndex = 29;
            this.comboProcess.Text = "comboBoxAdv2";
            // 
            // autoLabel1
            // 
            this.autoLabel1.BackColor = System.Drawing.Color.Transparent;
            this.autoLabel1.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoLabel1.Location = new System.Drawing.Point(10, 8);
            this.autoLabel1.Name = "autoLabel1";
            this.autoLabel1.Size = new System.Drawing.Size(96, 15);
            this.autoLabel1.TabIndex = 30;
            this.autoLabel1.Text = "ReportAlarm";
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel1.BackgroundImage")));
            this.gradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gradientPanel1.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gradientPanel1.Controls.Add(this.buttonAdv1);
            this.gradientPanel1.Controls.Add(this.TextAlarmIndex);
            this.gradientPanel1.Controls.Add(this.autoLabel1);
            this.gradientPanel1.Controls.Add(this.gradientLabel1);
            this.gradientPanel1.Controls.Add(this.comboProcess);
            this.gradientPanel1.Controls.Add(this.comboControll);
            this.gradientPanel1.Location = new System.Drawing.Point(12, 12);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(401, 115);
            this.gradientPanel1.TabIndex = 30;
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 268);
            this.Controls.Add(this.LabelTest1);
            this.Controls.Add(this.TextTest1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.gradientPanel1);
            this.Name = "FormTest";
            this.Text = "FormTest";
            this.Load += new System.EventHandler(this.FormTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.comboControll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradientPanel1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTest1;
        private System.Windows.Forms.TextBox TextTest1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private System.Windows.Forms.TextBox TextAlarmIndex;
        private Syncfusion.Windows.Forms.ButtonAdv BtnCancel;
        private Syncfusion.Windows.Forms.ButtonAdv BtnOK;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv1;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv comboControll;
        private Syncfusion.Windows.Forms.Tools.ComboBoxAdv comboProcess;
        private Syncfusion.Windows.Forms.Tools.AutoLabel autoLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
    }
}