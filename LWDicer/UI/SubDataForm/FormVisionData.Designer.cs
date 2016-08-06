﻿namespace LWDicer.UI
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
            this.picVision = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSearchMarkB = new System.Windows.Forms.Button();
            this.btnSearchMarkA = new System.Windows.Forms.Button();
            this.btnRegisterMarkB = new System.Windows.Forms.Button();
            this.btnRegisterMarkA = new System.Windows.Forms.Button();
            this.btnSelectAxis = new System.Windows.Forms.Button();
            this.btnSizeNarrow = new System.Windows.Forms.Button();
            this.btnSizeWide = new System.Windows.Forms.Button();
            this.btnShowMarkLine = new System.Windows.Forms.Button();
            this.btnShowHairLine = new System.Windows.Forms.Button();
            this.btnChangeCam = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picPatternMarkB = new System.Windows.Forms.PictureBox();
            this.picPatternMarkA = new System.Windows.Forms.PictureBox();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPatternMarkB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPatternMarkA)).BeginInit();
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
            // picVision
            // 
            this.picVision.Location = new System.Drawing.Point(12, 1);
            this.picVision.Name = "picVision";
            this.picVision.Size = new System.Drawing.Size(802, 600);
            this.picVision.TabIndex = 750;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSearchMarkB);
            this.groupBox4.Controls.Add(this.btnSearchMarkA);
            this.groupBox4.Controls.Add(this.btnRegisterMarkB);
            this.groupBox4.Controls.Add(this.btnRegisterMarkA);
            this.groupBox4.Controls.Add(this.btnSelectAxis);
            this.groupBox4.Controls.Add(this.btnSizeNarrow);
            this.groupBox4.Controls.Add(this.btnSizeWide);
            this.groupBox4.Controls.Add(this.btnShowMarkLine);
            this.groupBox4.Controls.Add(this.btnShowHairLine);
            this.groupBox4.Controls.Add(this.btnChangeCam);
            this.groupBox4.Location = new System.Drawing.Point(820, 1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(428, 393);
            this.groupBox4.TabIndex = 758;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Vision Control";
            // 
            // btnSearchMarkB
            // 
            this.btnSearchMarkB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchMarkB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSearchMarkB.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearchMarkB.Location = new System.Drawing.Point(87, 335);
            this.btnSearchMarkB.Name = "btnSearchMarkB";
            this.btnSearchMarkB.Size = new System.Drawing.Size(75, 54);
            this.btnSearchMarkB.TabIndex = 768;
            this.btnSearchMarkB.Text = "Mark Search B";
            this.btnSearchMarkB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearchMarkB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSearchMarkB.UseVisualStyleBackColor = true;
            this.btnSearchMarkB.Click += new System.EventHandler(this.btnSearchMarkB_Click);
            // 
            // btnSearchMarkA
            // 
            this.btnSearchMarkA.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchMarkA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSearchMarkA.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearchMarkA.Location = new System.Drawing.Point(6, 335);
            this.btnSearchMarkA.Name = "btnSearchMarkA";
            this.btnSearchMarkA.Size = new System.Drawing.Size(75, 54);
            this.btnSearchMarkA.TabIndex = 767;
            this.btnSearchMarkA.Text = "Mark Search A";
            this.btnSearchMarkA.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearchMarkA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSearchMarkA.UseVisualStyleBackColor = true;
            this.btnSearchMarkA.Click += new System.EventHandler(this.btnSearchMarkA_Click);
            // 
            // btnRegisterMarkB
            // 
            this.btnRegisterMarkB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegisterMarkB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRegisterMarkB.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRegisterMarkB.Location = new System.Drawing.Point(87, 275);
            this.btnRegisterMarkB.Name = "btnRegisterMarkB";
            this.btnRegisterMarkB.Size = new System.Drawing.Size(75, 54);
            this.btnRegisterMarkB.TabIndex = 766;
            this.btnRegisterMarkB.Text = "Mark 등록 B";
            this.btnRegisterMarkB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRegisterMarkB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRegisterMarkB.UseVisualStyleBackColor = true;
            this.btnRegisterMarkB.Click += new System.EventHandler(this.btnRegisterMarkB_Click);
            // 
            // btnRegisterMarkA
            // 
            this.btnRegisterMarkA.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegisterMarkA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnRegisterMarkA.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRegisterMarkA.Location = new System.Drawing.Point(6, 275);
            this.btnRegisterMarkA.Name = "btnRegisterMarkA";
            this.btnRegisterMarkA.Size = new System.Drawing.Size(75, 54);
            this.btnRegisterMarkA.TabIndex = 765;
            this.btnRegisterMarkA.Text = "Mark 등록 A";
            this.btnRegisterMarkA.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRegisterMarkA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRegisterMarkA.UseVisualStyleBackColor = true;
            this.btnRegisterMarkA.Click += new System.EventHandler(this.btnRegisterMarkA_Click);
            // 
            // btnSelectAxis
            // 
            this.btnSelectAxis.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelectAxis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSelectAxis.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectAxis.Location = new System.Drawing.Point(6, 192);
            this.btnSelectAxis.Name = "btnSelectAxis";
            this.btnSelectAxis.Size = new System.Drawing.Size(75, 77);
            this.btnSelectAxis.TabIndex = 764;
            this.btnSelectAxis.Text = "Select Width";
            this.btnSelectAxis.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelectAxis.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSelectAxis.UseVisualStyleBackColor = true;
            this.btnSelectAxis.Click += new System.EventHandler(this.btnSelectAxis_Click);
            // 
            // btnSizeNarrow
            // 
            this.btnSizeNarrow.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSizeNarrow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSizeNarrow.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSizeNarrow.Location = new System.Drawing.Point(87, 231);
            this.btnSizeNarrow.Name = "btnSizeNarrow";
            this.btnSizeNarrow.Size = new System.Drawing.Size(75, 38);
            this.btnSizeNarrow.TabIndex = 763;
            this.btnSizeNarrow.Text = "Narrow";
            this.btnSizeNarrow.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSizeNarrow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSizeNarrow.UseVisualStyleBackColor = true;
            this.btnSizeNarrow.Click += new System.EventHandler(this.btnSizeNarrow_Click);
            // 
            // btnSizeWide
            // 
            this.btnSizeWide.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSizeWide.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnSizeWide.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSizeWide.Location = new System.Drawing.Point(87, 192);
            this.btnSizeWide.Name = "btnSizeWide";
            this.btnSizeWide.Size = new System.Drawing.Size(75, 38);
            this.btnSizeWide.TabIndex = 761;
            this.btnSizeWide.Text = "Wide";
            this.btnSizeWide.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSizeWide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSizeWide.UseVisualStyleBackColor = true;
            this.btnSizeWide.Click += new System.EventHandler(this.btnSizeWide_Click);
            // 
            // btnShowMarkLine
            // 
            this.btnShowMarkLine.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnShowMarkLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnShowMarkLine.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShowMarkLine.Location = new System.Drawing.Point(87, 109);
            this.btnShowMarkLine.Name = "btnShowMarkLine";
            this.btnShowMarkLine.Size = new System.Drawing.Size(75, 77);
            this.btnShowMarkLine.TabIndex = 760;
            this.btnShowMarkLine.Text = "Align Mark Line";
            this.btnShowMarkLine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowMarkLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnShowMarkLine.UseVisualStyleBackColor = true;
            this.btnShowMarkLine.Click += new System.EventHandler(this.btnShowMarkLine_Click);
            // 
            // btnShowHairLine
            // 
            this.btnShowHairLine.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnShowHairLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnShowHairLine.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShowHairLine.Location = new System.Drawing.Point(6, 109);
            this.btnShowHairLine.Name = "btnShowHairLine";
            this.btnShowHairLine.Size = new System.Drawing.Size(75, 77);
            this.btnShowHairLine.TabIndex = 759;
            this.btnShowHairLine.Text = "Hair Line";
            this.btnShowHairLine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowHairLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnShowHairLine.UseVisualStyleBackColor = true;
            this.btnShowHairLine.Click += new System.EventHandler(this.btnShowHairLine_Click);
            // 
            // btnChangeCam
            // 
            this.btnChangeCam.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnChangeCam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnChangeCam.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnChangeCam.Location = new System.Drawing.Point(6, 26);
            this.btnChangeCam.Name = "btnChangeCam";
            this.btnChangeCam.Size = new System.Drawing.Size(75, 77);
            this.btnChangeCam.TabIndex = 758;
            this.btnChangeCam.Text = "Change Magnitude";
            this.btnChangeCam.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnChangeCam.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnChangeCam.UseVisualStyleBackColor = true;
            this.btnChangeCam.Click += new System.EventHandler(this.btnChangeCam_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.picPatternMarkB);
            this.groupBox1.Controls.Add(this.picPatternMarkA);
            this.groupBox1.Location = new System.Drawing.Point(820, 400);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 201);
            this.groupBox1.TabIndex = 759;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pattern Mark";
            // 
            // picPatternMarkB
            // 
            this.picPatternMarkB.Location = new System.Drawing.Point(227, 34);
            this.picPatternMarkB.Name = "picPatternMarkB";
            this.picPatternMarkB.Size = new System.Drawing.Size(173, 154);
            this.picPatternMarkB.TabIndex = 1;
            this.picPatternMarkB.TabStop = false;
            // 
            // picPatternMarkA
            // 
            this.picPatternMarkA.Location = new System.Drawing.Point(25, 34);
            this.picPatternMarkA.Name = "picPatternMarkA";
            this.picPatternMarkA.Size = new System.Drawing.Size(173, 154);
            this.picPatternMarkA.TabIndex = 0;
            this.picPatternMarkA.TabStop = false;
            // 
            // FormVisionData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 884);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.picVision);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormVisionData";
            this.Text = "Vision Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormVisionData_FormClosing);
            this.Load += new System.EventHandler(this.FormVisionData_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPatternMarkB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPatternMarkA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Panel picVision;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnChangeCam;
        private System.Windows.Forms.Button btnShowMarkLine;
        private System.Windows.Forms.Button btnShowHairLine;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picPatternMarkB;
        private System.Windows.Forms.PictureBox picPatternMarkA;
        private System.Windows.Forms.Button btnSelectAxis;
        private System.Windows.Forms.Button btnSizeNarrow;
        private System.Windows.Forms.Button btnSizeWide;
        private System.Windows.Forms.Button btnRegisterMarkB;
        private System.Windows.Forms.Button btnRegisterMarkA;
        private System.Windows.Forms.Button btnSearchMarkA;
        private System.Windows.Forms.Button btnSearchMarkB;
    }
}