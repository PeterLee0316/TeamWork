using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormModelData : Form
    {
        private CModelData m_ModelData;

        public FormModelData()
        {
            InitializeComponent();

            m_ModelData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData);

            foreach (CListHeader info in CMainFrame.DataManager.WaferFrameHeaderList)
            {
                if (info.IsFolder == false)
                {
                    ComboWaferFrame.Items.Add(info.Name);
                }
            }

            try
            {
                ComboWaferFrame.SelectedIndex = ComboWaferFrame.Items.IndexOf(m_ModelData.WaferFrameName);
            }
            catch (System.Exception ex)
            {
                ComboWaferFrame.SelectedIndex = -1;
            }

            this.Text = $"Model Data [ Current Model : {m_ModelData.Name} ]";
        }

        private void FormModelData_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save data?"))
            {
                return;
            }

            m_ModelData.WaferFrameName = ComboWaferFrame.Text;

            CMainFrame.LWDicer.SaveModelData(m_ModelData);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
