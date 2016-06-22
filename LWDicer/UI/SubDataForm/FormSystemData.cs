using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormSystemData : Form
    {
        private int SelLanguage;
        private CSystemData SystemData;

        public FormSystemData()
        {
            SystemData = ObjectExtensions.Copy(CMainFrame.LWDicer.m_DataManager.SystemData);
            InitializeComponent();

            ComboLanguage.Items.Add("KOREAN");
            ComboLanguage.Items.Add("ENGLISH");
            ComboLanguage.Items.Add("CHINESE");
            ComboLanguage.Items.Add("JAPANESE");
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void FormSystemData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("System Data를 저장 하시겠습니까?"))
            {
                return;
            }

            SystemData.Language = (ELanguage)SelLanguage;

            CMainFrame.LWDicer.m_DataManager.SaveSystemData(SystemData);
        }

        private void FormSystemData_Load(object sender, EventArgs e)
        {
            // Model Name
            LabelModelName.Text = SystemData.ModelName;

            // System Display Language
            UpdateComboLanguage();

        }

        private void ComboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboLanguage = (ComboBox)sender;

            SelLanguage = (int)ComboLanguage.SelectedIndex;
        }

        private void UpdateComboLanguage()
        {
            switch(SystemData.Language)
            {
                case ELanguage.KOREAN:
                    ComboLanguage.SelectedIndex = (int)ELanguage.KOREAN;
                    break;

                case ELanguage.ENGLISH:
                    ComboLanguage.SelectedIndex = (int)ELanguage.ENGLISH;
                    break;

                case ELanguage.CHINESE:
                    ComboLanguage.SelectedIndex = (int)ELanguage.CHINESE;
                    break;

                case ELanguage.JAPANESE:
                    ComboLanguage.SelectedIndex = (int)ELanguage.JAPANESE;
                    break;
            }
        }
    }
}
