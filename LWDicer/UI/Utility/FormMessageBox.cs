﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LWDicer.UI
{
    public partial class FormMessageBox : Form
    {
        private string strTextKor, strTextEng;
        private bool bBtnOption;

        public FormMessageBox()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.Fixed3D;
        }

        public void SetMessage(string strMsg, bool bOkCancel)
        {
            bBtnOption = bOkCancel;
            strTextKor = strMsg;
        }

        private void FormUtilMsg_Load(object sender, EventArgs e)
        {
            LabelTextKor.Text = strTextKor;
            LabelTextEng.Text = strTextEng;

            if (bBtnOption == true)
            {
                BtnConfirm.Visible = false;
                BtnOK.Visible = true;
                BtnCancel.Visible = true;
            }
            else
            {
                BtnOK.Visible = false;
                BtnCancel.Visible = false;
                BtnConfirm.Visible = true;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
