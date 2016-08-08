﻿namespace LWDicer.UI
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
            this.tmFormTop = new System.Windows.Forms.Timer(this.components);
            this.BtnUserLogin = new System.Windows.Forms.Button();
            this.LabelCurUser = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextMessage
            // 
            this.TextMessage.BackColor = System.Drawing.SystemColors.Control;
            this.TextMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextMessage.Enabled = false;
            this.TextMessage.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextMessage.Location = new System.Drawing.Point(423, 70);
            this.TextMessage.Name = "TextMessage";
            this.TextMessage.Size = new System.Drawing.Size(617, 15);
            this.TextMessage.TabIndex = 0;
            // 
            // TextTime
            // 
            this.TextTime.BackColor = System.Drawing.SystemColors.Control;
            this.TextTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextTime.Enabled = false;
            this.TextTime.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextTime.Location = new System.Drawing.Point(74, 70);
            this.TextTime.Name = "TextTime";
            this.TextTime.Size = new System.Drawing.Size(241, 15);
            this.TextTime.TabIndex = 1;
            // 
            // tmFormTop
            // 
            this.tmFormTop.Tick += new System.EventHandler(this.tmFormTop_Tick);
            // 
            // BtnUserLogin
            // 
            this.BtnUserLogin.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnUserLogin.Image = ((System.Drawing.Image)(resources.GetObject("BtnUserLogin.Image")));
            this.BtnUserLogin.Location = new System.Drawing.Point(1125, 4);
            this.BtnUserLogin.Name = "BtnUserLogin";
            this.BtnUserLogin.Size = new System.Drawing.Size(130, 67);
            this.BtnUserLogin.TabIndex = 3;
            this.BtnUserLogin.Text = " User  Log-In";
            this.BtnUserLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnUserLogin.UseVisualStyleBackColor = true;
            this.BtnUserLogin.Click += new System.EventHandler(this.BtnUserLogin_Click);
            // 
            // LabelCurUser
            // 
            this.LabelCurUser.AutoSize = true;
            this.LabelCurUser.BackColor = System.Drawing.Color.Transparent;
            this.LabelCurUser.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelCurUser.Location = new System.Drawing.Point(1061, 73);
            this.LabelCurUser.Name = "LabelCurUser";
            this.LabelCurUser.Size = new System.Drawing.Size(114, 20);
            this.LabelCurUser.TabIndex = 4;
            this.LabelCurUser.Text = "Current User : ";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExit.Location = new System.Drawing.Point(1082, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 66);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "종료";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // FormTopScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1278, 98);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.LabelCurUser);
            this.Controls.Add(this.BtnUserLogin);
            this.Controls.Add(this.TextTime);
            this.Controls.Add(this.TextMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTopScreen";
            this.Text = "Top Screen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextMessage;
        private System.Windows.Forms.TextBox TextTime;
        private System.Windows.Forms.Timer tmFormTop;
        private System.Windows.Forms.Button BtnUserLogin;
        private System.Windows.Forms.Label LabelCurUser;
        private System.Windows.Forms.Button btnExit;
    }
}