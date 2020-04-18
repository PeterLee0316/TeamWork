namespace Core.UI
{
    partial class FormAutoScreen
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
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.gradientLabel9 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.GridAlignPos = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GridAlignPos)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TimerUI
            // 
            this.TimerUI.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // gradientLabel9
            // 
            this.gradientLabel9.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel9.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel9.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel9.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel9.Location = new System.Drawing.Point(53, 56);
            this.gradientLabel9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.gradientLabel9.Name = "gradientLabel9";
            this.gradientLabel9.Size = new System.Drawing.Size(1227, 66);
            this.gradientLabel9.TabIndex = 926;
            this.gradientLabel9.Text = "Target && Object Coordinate";
            this.gradientLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GridAlignPos
            // 
            this.GridAlignPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridAlignPos.ColWidthEntries.AddRange(new Syncfusion.Windows.Forms.Grid.GridColWidth[] {
            new Syncfusion.Windows.Forms.Grid.GridColWidth(0, 35)});
            this.GridAlignPos.GridVisualStyles = Syncfusion.Windows.Forms.GridVisualStyles.Office2003;
            this.GridAlignPos.Location = new System.Drawing.Point(58, 130);
            this.GridAlignPos.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.GridAlignPos.Name = "GridAlignPos";
            this.GridAlignPos.RowHeightEntries.AddRange(new Syncfusion.Windows.Forms.Grid.GridRowHeight[] {
            new Syncfusion.Windows.Forms.Grid.GridRowHeight(0, 21)});
            this.GridAlignPos.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridAlignPos.Size = new System.Drawing.Size(1221, 734);
            this.GridAlignPos.SmartSizeBox = false;
            this.GridAlignPos.TabIndex = 925;
            this.GridAlignPos.Text = "gridControl1";
            this.GridAlignPos.UseRightToLeftCompatibleTextBox = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1348, 93);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 21);
            this.label1.TabIndex = 927;
            this.label1.Text = "P4";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SkyBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(1388, 93);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(489, 770);
            this.panel1.TabIndex = 928;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightCyan;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(30, 35);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(431, 700);
            this.panel2.TabIndex = 929;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1348, 844);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 21);
            this.label2.TabIndex = 929;
            this.label2.Text = "P1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1887, 844);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 21);
            this.label3.TabIndex = 931;
            this.label3.Text = "P2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1887, 93);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 21);
            this.label4.TabIndex = 930;
            this.label4.Text = "P3";
            // 
            // FormAutoScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1920, 1430);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gradientLabel9);
            this.Controls.Add(this.GridAlignPos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FormAutoScreen";
            this.Text = "Auto Screen";
            this.Activated += new System.EventHandler(this.FormAutoScreen_Activated);
            this.Load += new System.EventHandler(this.FormAutoScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridAlignPos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer TimerUI;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel9;
        private Syncfusion.Windows.Forms.Grid.GridControl GridAlignPos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}