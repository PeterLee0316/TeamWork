﻿namespace LWDicer.UI
{
    partial class FormSpinnerManualOP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSpinnerManualOP));
            this.BtnExit = new System.Windows.Forms.Button();
            this.TmrManualOP = new System.Windows.Forms.Timer(this.components);
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel16 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnCleanNozzleOff = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnCleanNozzleOn = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnSpinnerDown = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnSpinnerUp = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientLabel2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnCoatNozzleOff = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnCoatNozzleOn = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientLabel3 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnVacuumOff = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnVacuumOn = new Syncfusion.Windows.Forms.ButtonAdv();
            this.LabelVACTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel4 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelUpDnTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel6 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelN1OnOffTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel8 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelN2OnOffTime = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.gradientLabel10 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnExit.Location = new System.Drawing.Point(230, 464);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(124, 61);
            this.BtnExit.TabIndex = 757;
            this.BtnExit.Text = " Exit";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel1.Location = new System.Drawing.Point(335, 160);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(237, 38);
            this.gradientLabel1.TabIndex = 939;
            this.gradientLabel1.Text = "Clean Nozzle Valve";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel16
            // 
            this.gradientLabel16.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel16.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel16.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel16.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel16.Location = new System.Drawing.Point(335, 9);
            this.gradientLabel16.Name = "gradientLabel16";
            this.gradientLabel16.Size = new System.Drawing.Size(237, 38);
            this.gradientLabel16.TabIndex = 938;
            this.gradientLabel16.Text = "Spinner Up/Down";
            this.gradientLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnCleanNozzleOff
            // 
            this.BtnCleanNozzleOff.BackColor = System.Drawing.Color.LightGray;
            this.BtnCleanNozzleOff.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnCleanNozzleOff.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnCleanNozzleOff.FlatAppearance.BorderSize = 5;
            this.BtnCleanNozzleOff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCleanNozzleOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCleanNozzleOff.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCleanNozzleOff.Location = new System.Drawing.Point(455, 199);
            this.BtnCleanNozzleOff.Name = "BtnCleanNozzleOff";
            this.BtnCleanNozzleOff.Size = new System.Drawing.Size(117, 64);
            this.BtnCleanNozzleOff.TabIndex = 937;
            this.BtnCleanNozzleOff.Tag = "1";
            this.BtnCleanNozzleOff.Text = "Off";
            this.BtnCleanNozzleOff.Click += new System.EventHandler(this.BtnCleanNozzleOff_Click);
            // 
            // BtnCleanNozzleOn
            // 
            this.BtnCleanNozzleOn.BackColor = System.Drawing.Color.LightGray;
            this.BtnCleanNozzleOn.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnCleanNozzleOn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnCleanNozzleOn.FlatAppearance.BorderSize = 5;
            this.BtnCleanNozzleOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCleanNozzleOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCleanNozzleOn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCleanNozzleOn.Location = new System.Drawing.Point(335, 199);
            this.BtnCleanNozzleOn.Name = "BtnCleanNozzleOn";
            this.BtnCleanNozzleOn.Size = new System.Drawing.Size(117, 64);
            this.BtnCleanNozzleOn.TabIndex = 936;
            this.BtnCleanNozzleOn.Tag = "0";
            this.BtnCleanNozzleOn.Text = "On";
            this.BtnCleanNozzleOn.Click += new System.EventHandler(this.BtnCleanNozzleOn_Click);
            // 
            // BtnSpinnerDown
            // 
            this.BtnSpinnerDown.BackColor = System.Drawing.Color.LightGray;
            this.BtnSpinnerDown.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnSpinnerDown.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnSpinnerDown.FlatAppearance.BorderSize = 5;
            this.BtnSpinnerDown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnSpinnerDown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnSpinnerDown.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSpinnerDown.Location = new System.Drawing.Point(455, 48);
            this.BtnSpinnerDown.Name = "BtnSpinnerDown";
            this.BtnSpinnerDown.Size = new System.Drawing.Size(117, 64);
            this.BtnSpinnerDown.TabIndex = 935;
            this.BtnSpinnerDown.Tag = "1";
            this.BtnSpinnerDown.Text = "Down";
            this.BtnSpinnerDown.Click += new System.EventHandler(this.BtnSpinnerDown_Click);
            // 
            // BtnSpinnerUp
            // 
            this.BtnSpinnerUp.BackColor = System.Drawing.Color.LightGray;
            this.BtnSpinnerUp.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnSpinnerUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnSpinnerUp.FlatAppearance.BorderSize = 5;
            this.BtnSpinnerUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnSpinnerUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnSpinnerUp.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSpinnerUp.Location = new System.Drawing.Point(335, 48);
            this.BtnSpinnerUp.Name = "BtnSpinnerUp";
            this.BtnSpinnerUp.Size = new System.Drawing.Size(117, 64);
            this.BtnSpinnerUp.TabIndex = 934;
            this.BtnSpinnerUp.Tag = "0";
            this.BtnSpinnerUp.Text = "Up";
            this.BtnSpinnerUp.Click += new System.EventHandler(this.BtnSpinnerUp_Click);
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel2.Location = new System.Drawing.Point(335, 310);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(237, 38);
            this.gradientLabel2.TabIndex = 942;
            this.gradientLabel2.Text = "Coat Nozzle Valve";
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnCoatNozzleOff
            // 
            this.BtnCoatNozzleOff.BackColor = System.Drawing.Color.LightGray;
            this.BtnCoatNozzleOff.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnCoatNozzleOff.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnCoatNozzleOff.FlatAppearance.BorderSize = 5;
            this.BtnCoatNozzleOff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCoatNozzleOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCoatNozzleOff.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCoatNozzleOff.Location = new System.Drawing.Point(455, 349);
            this.BtnCoatNozzleOff.Name = "BtnCoatNozzleOff";
            this.BtnCoatNozzleOff.Size = new System.Drawing.Size(117, 64);
            this.BtnCoatNozzleOff.TabIndex = 941;
            this.BtnCoatNozzleOff.Tag = "1";
            this.BtnCoatNozzleOff.Text = "Off";
            this.BtnCoatNozzleOff.Click += new System.EventHandler(this.BtnCoatNozzleOff_Click);
            // 
            // BtnCoatNozzleOn
            // 
            this.BtnCoatNozzleOn.BackColor = System.Drawing.Color.LightGray;
            this.BtnCoatNozzleOn.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnCoatNozzleOn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnCoatNozzleOn.FlatAppearance.BorderSize = 5;
            this.BtnCoatNozzleOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCoatNozzleOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCoatNozzleOn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCoatNozzleOn.Location = new System.Drawing.Point(335, 349);
            this.BtnCoatNozzleOn.Name = "BtnCoatNozzleOn";
            this.BtnCoatNozzleOn.Size = new System.Drawing.Size(117, 64);
            this.BtnCoatNozzleOn.TabIndex = 940;
            this.BtnCoatNozzleOn.Tag = "0";
            this.BtnCoatNozzleOn.Text = "On";
            this.BtnCoatNozzleOn.Click += new System.EventHandler(this.BtnCoatNozzleOn_Click);
            // 
            // gradientLabel3
            // 
            this.gradientLabel3.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel3.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.gradientLabel3.Location = new System.Drawing.Point(12, 9);
            this.gradientLabel3.Name = "gradientLabel3";
            this.gradientLabel3.Size = new System.Drawing.Size(237, 38);
            this.gradientLabel3.TabIndex = 945;
            this.gradientLabel3.Text = "Chuck Vacuum";
            this.gradientLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnVacuumOff
            // 
            this.BtnVacuumOff.BackColor = System.Drawing.Color.LightGray;
            this.BtnVacuumOff.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnVacuumOff.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnVacuumOff.FlatAppearance.BorderSize = 5;
            this.BtnVacuumOff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOff.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnVacuumOff.Location = new System.Drawing.Point(132, 48);
            this.BtnVacuumOff.Name = "BtnVacuumOff";
            this.BtnVacuumOff.Size = new System.Drawing.Size(117, 64);
            this.BtnVacuumOff.TabIndex = 944;
            this.BtnVacuumOff.Tag = "1";
            this.BtnVacuumOff.Text = "Off";
            this.BtnVacuumOff.Click += new System.EventHandler(this.BtnVacuumOff_Click);
            // 
            // BtnVacuumOn
            // 
            this.BtnVacuumOn.BackColor = System.Drawing.Color.LightGray;
            this.BtnVacuumOn.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnVacuumOn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnVacuumOn.FlatAppearance.BorderSize = 5;
            this.BtnVacuumOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnVacuumOn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnVacuumOn.Location = new System.Drawing.Point(12, 48);
            this.BtnVacuumOn.Name = "BtnVacuumOn";
            this.BtnVacuumOn.Size = new System.Drawing.Size(117, 64);
            this.BtnVacuumOn.TabIndex = 943;
            this.BtnVacuumOn.Tag = "0";
            this.BtnVacuumOn.Text = "On";
            this.BtnVacuumOn.Click += new System.EventHandler(this.BtnVacuumOn_Click);
            // 
            // LabelVACTime
            // 
            this.LabelVACTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelVACTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelVACTime.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.LabelVACTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelVACTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LabelVACTime.Location = new System.Drawing.Point(132, 113);
            this.LabelVACTime.Name = "LabelVACTime";
            this.LabelVACTime.Size = new System.Drawing.Size(117, 30);
            this.LabelVACTime.TabIndex = 961;
            this.LabelVACTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel4
            // 
            this.gradientLabel4.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.Silver, System.Drawing.Color.Maroon);
            this.gradientLabel4.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel4.Location = new System.Drawing.Point(12, 113);
            this.gradientLabel4.Name = "gradientLabel4";
            this.gradientLabel4.Size = new System.Drawing.Size(117, 30);
            this.gradientLabel4.TabIndex = 960;
            this.gradientLabel4.Text = "수행 시간";
            this.gradientLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelUpDnTime
            // 
            this.LabelUpDnTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelUpDnTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelUpDnTime.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.LabelUpDnTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelUpDnTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LabelUpDnTime.Location = new System.Drawing.Point(455, 113);
            this.LabelUpDnTime.Name = "LabelUpDnTime";
            this.LabelUpDnTime.Size = new System.Drawing.Size(117, 30);
            this.LabelUpDnTime.TabIndex = 963;
            this.LabelUpDnTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel6
            // 
            this.gradientLabel6.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.Silver, System.Drawing.Color.Maroon);
            this.gradientLabel6.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel6.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel6.Location = new System.Drawing.Point(335, 113);
            this.gradientLabel6.Name = "gradientLabel6";
            this.gradientLabel6.Size = new System.Drawing.Size(117, 30);
            this.gradientLabel6.TabIndex = 962;
            this.gradientLabel6.Text = "수행 시간";
            this.gradientLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelN1OnOffTime
            // 
            this.LabelN1OnOffTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelN1OnOffTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelN1OnOffTime.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.LabelN1OnOffTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelN1OnOffTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LabelN1OnOffTime.Location = new System.Drawing.Point(455, 264);
            this.LabelN1OnOffTime.Name = "LabelN1OnOffTime";
            this.LabelN1OnOffTime.Size = new System.Drawing.Size(117, 30);
            this.LabelN1OnOffTime.TabIndex = 965;
            this.LabelN1OnOffTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel8
            // 
            this.gradientLabel8.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.Silver, System.Drawing.Color.Maroon);
            this.gradientLabel8.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel8.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel8.Location = new System.Drawing.Point(335, 264);
            this.gradientLabel8.Name = "gradientLabel8";
            this.gradientLabel8.Size = new System.Drawing.Size(117, 30);
            this.gradientLabel8.TabIndex = 964;
            this.gradientLabel8.Text = "수행 시간";
            this.gradientLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelN2OnOffTime
            // 
            this.LabelN2OnOffTime.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelN2OnOffTime.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelN2OnOffTime.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.LabelN2OnOffTime.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelN2OnOffTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.LabelN2OnOffTime.Location = new System.Drawing.Point(455, 414);
            this.LabelN2OnOffTime.Name = "LabelN2OnOffTime";
            this.LabelN2OnOffTime.Size = new System.Drawing.Size(117, 30);
            this.LabelN2OnOffTime.TabIndex = 967;
            this.LabelN2OnOffTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel10
            // 
            this.gradientLabel10.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.ForwardDiagonal, System.Drawing.Color.Silver, System.Drawing.Color.Maroon);
            this.gradientLabel10.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel10.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.gradientLabel10.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gradientLabel10.Location = new System.Drawing.Point(335, 414);
            this.gradientLabel10.Name = "gradientLabel10";
            this.gradientLabel10.Size = new System.Drawing.Size(117, 30);
            this.gradientLabel10.TabIndex = 966;
            this.gradientLabel10.Text = "수행 시간";
            this.gradientLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormSpinnerManualOP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 537);
            this.Controls.Add(this.LabelN2OnOffTime);
            this.Controls.Add(this.gradientLabel10);
            this.Controls.Add(this.LabelN1OnOffTime);
            this.Controls.Add(this.gradientLabel8);
            this.Controls.Add(this.LabelUpDnTime);
            this.Controls.Add(this.gradientLabel6);
            this.Controls.Add(this.LabelVACTime);
            this.Controls.Add(this.gradientLabel4);
            this.Controls.Add(this.gradientLabel3);
            this.Controls.Add(this.BtnVacuumOff);
            this.Controls.Add(this.BtnVacuumOn);
            this.Controls.Add(this.gradientLabel2);
            this.Controls.Add(this.BtnCoatNozzleOff);
            this.Controls.Add(this.BtnCoatNozzleOn);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.gradientLabel16);
            this.Controls.Add(this.BtnCleanNozzleOff);
            this.Controls.Add(this.BtnCleanNozzleOn);
            this.Controls.Add(this.BtnSpinnerDown);
            this.Controls.Add(this.BtnSpinnerUp);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSpinnerManualOP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormSpinnerManualOP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSpinnerManualOP_FormClosing);
            this.Load += new System.EventHandler(this.FormSpinnerManualOP_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Timer TmrManualOP;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel16;
        private Syncfusion.Windows.Forms.ButtonAdv BtnCleanNozzleOff;
        private Syncfusion.Windows.Forms.ButtonAdv BtnCleanNozzleOn;
        private Syncfusion.Windows.Forms.ButtonAdv BtnSpinnerDown;
        private Syncfusion.Windows.Forms.ButtonAdv BtnSpinnerUp;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel2;
        private Syncfusion.Windows.Forms.ButtonAdv BtnCoatNozzleOff;
        private Syncfusion.Windows.Forms.ButtonAdv BtnCoatNozzleOn;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel3;
        private Syncfusion.Windows.Forms.ButtonAdv BtnVacuumOff;
        private Syncfusion.Windows.Forms.ButtonAdv BtnVacuumOn;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelVACTime;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel4;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelUpDnTime;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel6;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelN1OnOffTime;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel8;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelN2OnOffTime;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel10;
    }
}