﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_DataManager;

namespace Core.UI
{
    public partial class FormSystemData : Form
    {
        private int SelLanguage;
        private CSystemData SystemData;

        public FormSystemData()
        {
            SystemData = ObjectExtensions.Copy(CMainFrame.DataManager.SystemData);
            InitializeComponent();
            
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSystemData_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save data?"))
            {
                return;
            }

            SystemData.Language = (ELanguage)SelLanguage;

            CMainFrame.mCore.SaveSystemData(SystemData);
        }

        private void FormSystemData_Load(object sender, EventArgs e)
        {
        }

        private void ComboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboLanguage = (ComboBox)sender;

            SelLanguage = (int)ComboLanguage.SelectedIndex;
        }

    }
}
