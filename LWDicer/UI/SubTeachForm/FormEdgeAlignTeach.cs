﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LWDicer.UI
{
    public partial class FormEdgeAlignTeach : Form
    {
        public FormEdgeAlignTeach()
        {
            InitializeComponent();
        }

        private void FormClose()
        {
            this.Hide();
        }


        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }
    }
}