using System;
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

            ComboLanguage.Items.Add("KOREAN");
            ComboLanguage.Items.Add("ENGLISH");
            ComboLanguage.Items.Add("CHINESE");
            ComboLanguage.Items.Add("JAPANESE");
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
            SystemData.CheckSafety_AutoMode = checkBox_CheckSafety_AutoMode.Checked;
            SystemData.CheckSafety_ManualMode = checkBox_CheckSafety_ManualMode.Checked;
            SystemData.EnableCylinderMove_EStop = checkBox_EnableCylinderMove.Checked;

            CMainFrame.Core.SaveSystemData(SystemData);
        }

        private void FormSystemData_Load(object sender, EventArgs e)
        {
            // Model Name
            LabelModelName.Text = SystemData.ModelName;

            // System Display Language
            UpdateComboLanguage();

            checkBox_CheckSafety_AutoMode.Checked = SystemData.CheckSafety_AutoMode;
            checkBox_CheckSafety_ManualMode.Checked = SystemData.CheckSafety_ManualMode;
            checkBox_EnableCylinderMove.Checked = SystemData.EnableCylinderMove_EStop;
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
