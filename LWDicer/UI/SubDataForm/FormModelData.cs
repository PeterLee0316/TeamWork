using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormModelData : Form
    {
        private string strCassette;

        private CModelData modelData;

        public FormModelData()
        {
            InitializeComponent();

            modelData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData);

            foreach (CListHeader info in CMainFrame.DataManager.CassetteHeaderList)
            {
                if (info.IsFolder == false)
                {
                    ComboCassette.Items.Add(info.Name);
                }
            }

            ComboCassette.SelectedIndex = ComboCassette.Items.IndexOf(modelData.CassetteName);

            this.Text = $"Model Data [ Current Model : {modelData.Name} ]";
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void FormModelData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save Data?"))
            {
                return;
            }

            modelData.CassetteName = strCassette;

            CMainFrame.DataManager.SaveModelData(modelData);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void ComboCassette_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboCassette = (ComboBox)sender;

            strCassette = ComboCassette.Text;
        }
    }
}
