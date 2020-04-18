namespace Core.UI
{
    partial class FormTopScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTopScreen));
            this.TextMessage = new System.Windows.Forms.TextBox();
            this.TextTime = new System.Windows.Forms.TextBox();
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.BtnUserLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnEMO = new System.Windows.Forms.Button();
            this.LabelTowerR = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelTowerY = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelTowerG = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelBuzzer1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelBuzzer2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelBuzzer3 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelBuzzer4 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.textVersion = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TextMessage
            // 
            this.TextMessage.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TextMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextMessage.Enabled = false;
            this.TextMessage.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextMessage.Location = new System.Drawing.Point(1056, 7);
            this.TextMessage.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.TextMessage.Multiline = true;
            this.TextMessage.Name = "TextMessage";
            this.TextMessage.Size = new System.Drawing.Size(524, 159);
            this.TextMessage.TabIndex = 0;
            // 
            // TextTime
            // 
            this.TextTime.BackColor = System.Drawing.SystemColors.Control;
            this.TextTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextTime.Enabled = false;
            this.TextTime.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextTime.Location = new System.Drawing.Point(1713, 105);
            this.TextTime.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.TextTime.Name = "TextTime";
            this.TextTime.Size = new System.Drawing.Size(291, 27);
            this.TextTime.TabIndex = 1;
            // 
            // TimerUI
            // 
            this.TimerUI.Tick += new System.EventHandler(this.TimerUI_Tick);
            // 
            // BtnUserLogin
            // 
            this.BtnUserLogin.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnUserLogin.Image = ((System.Drawing.Image)(resources.GetObject("BtnUserLogin.Image")));
            this.BtnUserLogin.Location = new System.Drawing.Point(1581, 7);
            this.BtnUserLogin.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.BtnUserLogin.Name = "BtnUserLogin";
            this.BtnUserLogin.Size = new System.Drawing.Size(174, 93);
            this.BtnUserLogin.TabIndex = 3;
            this.BtnUserLogin.Text = " User  Log-In";
            this.BtnUserLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnUserLogin.UseVisualStyleBackColor = true;
            this.BtnUserLogin.Click += new System.EventHandler(this.BtnUserLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExit.Location = new System.Drawing.Point(1815, 7);
            this.btnExit.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(91, 93);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "종료";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStart.Location = new System.Drawing.Point(717, 7);
            this.btnStart.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(124, 93);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStop.Location = new System.Drawing.Point(850, 7);
            this.btnStop.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(116, 93);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Transparent;
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReset.Location = new System.Drawing.Point(976, 7);
            this.btnReset.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(71, 42);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnEMO
            // 
            this.btnEMO.BackColor = System.Drawing.Color.Transparent;
            this.btnEMO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEMO.Location = new System.Drawing.Point(976, 52);
            this.btnEMO.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnEMO.Name = "btnEMO";
            this.btnEMO.Size = new System.Drawing.Size(71, 42);
            this.btnEMO.TabIndex = 9;
            this.btnEMO.Text = "EMO";
            this.btnEMO.UseVisualStyleBackColor = false;
            this.btnEMO.Click += new System.EventHandler(this.btnEMO_Click);
            // 
            // LabelTowerR
            // 
            this.LabelTowerR.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelTowerR.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTowerR.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTowerR.Location = new System.Drawing.Point(723, 114);
            this.LabelTowerR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelTowerR.Name = "LabelTowerR";
            this.LabelTowerR.Size = new System.Drawing.Size(39, 42);
            this.LabelTowerR.TabIndex = 755;
            this.LabelTowerR.Text = "R";
            this.LabelTowerR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelTowerY
            // 
            this.LabelTowerY.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelTowerY.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTowerY.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTowerY.Location = new System.Drawing.Point(762, 114);
            this.LabelTowerY.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelTowerY.Name = "LabelTowerY";
            this.LabelTowerY.Size = new System.Drawing.Size(39, 42);
            this.LabelTowerY.TabIndex = 756;
            this.LabelTowerY.Text = "Y";
            this.LabelTowerY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelTowerG
            // 
            this.LabelTowerG.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelTowerG.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelTowerG.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelTowerG.Location = new System.Drawing.Point(801, 114);
            this.LabelTowerG.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelTowerG.Name = "LabelTowerG";
            this.LabelTowerG.Size = new System.Drawing.Size(39, 42);
            this.LabelTowerG.TabIndex = 757;
            this.LabelTowerG.Text = "G";
            this.LabelTowerG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelBuzzer1
            // 
            this.LabelBuzzer1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelBuzzer1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelBuzzer1.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelBuzzer1.Location = new System.Drawing.Point(841, 114);
            this.LabelBuzzer1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelBuzzer1.Name = "LabelBuzzer1";
            this.LabelBuzzer1.Size = new System.Drawing.Size(39, 42);
            this.LabelBuzzer1.TabIndex = 758;
            this.LabelBuzzer1.Text = "B1";
            this.LabelBuzzer1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelBuzzer2
            // 
            this.LabelBuzzer2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelBuzzer2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelBuzzer2.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelBuzzer2.Location = new System.Drawing.Point(880, 114);
            this.LabelBuzzer2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelBuzzer2.Name = "LabelBuzzer2";
            this.LabelBuzzer2.Size = new System.Drawing.Size(39, 42);
            this.LabelBuzzer2.TabIndex = 759;
            this.LabelBuzzer2.Text = "B2";
            this.LabelBuzzer2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelBuzzer3
            // 
            this.LabelBuzzer3.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelBuzzer3.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelBuzzer3.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelBuzzer3.Location = new System.Drawing.Point(919, 114);
            this.LabelBuzzer3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelBuzzer3.Name = "LabelBuzzer3";
            this.LabelBuzzer3.Size = new System.Drawing.Size(39, 42);
            this.LabelBuzzer3.TabIndex = 760;
            this.LabelBuzzer3.Text = "B3";
            this.LabelBuzzer3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelBuzzer4
            // 
            this.LabelBuzzer4.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.LabelBuzzer4.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelBuzzer4.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelBuzzer4.Location = new System.Drawing.Point(959, 114);
            this.LabelBuzzer4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelBuzzer4.Name = "LabelBuzzer4";
            this.LabelBuzzer4.Size = new System.Drawing.Size(39, 42);
            this.LabelBuzzer4.TabIndex = 761;
            this.LabelBuzzer4.Text = "B4";
            this.LabelBuzzer4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textVersion
            // 
            this.textVersion.BackColor = System.Drawing.SystemColors.Control;
            this.textVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textVersion.Enabled = false;
            this.textVersion.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textVersion.Location = new System.Drawing.Point(1615, 114);
            this.textVersion.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.textVersion.Name = "textVersion";
            this.textVersion.Size = new System.Drawing.Size(291, 27);
            this.textVersion.TabIndex = 762;
            // 
            // FormTopScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1920, 172);
            this.ControlBox = false;
            this.Controls.Add(this.textVersion);
            this.Controls.Add(this.LabelBuzzer4);
            this.Controls.Add(this.LabelBuzzer3);
            this.Controls.Add(this.LabelBuzzer2);
            this.Controls.Add(this.LabelBuzzer1);
            this.Controls.Add(this.LabelTowerG);
            this.Controls.Add(this.LabelTowerY);
            this.Controls.Add(this.LabelTowerR);
            this.Controls.Add(this.btnEMO);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.BtnUserLogin);
            this.Controls.Add(this.TextTime);
            this.Controls.Add(this.TextMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FormTopScreen";
            this.Text = "Top Screen";
            this.Load += new System.EventHandler(this.FormTopScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextMessage;
        private System.Windows.Forms.TextBox TextTime;
        private System.Windows.Forms.Timer TimerUI;
        private System.Windows.Forms.Button BtnUserLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnEMO;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTowerR;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTowerY;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelTowerG;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelBuzzer1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelBuzzer2;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelBuzzer3;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelBuzzer4;
        private System.Windows.Forms.TextBox textVersion;
    }
}