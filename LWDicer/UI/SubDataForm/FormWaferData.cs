using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Syncfusion.Windows.Forms.Tools;

using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormWaferData : Form
    {
        private CModelData m_ModelData;

        public FormWaferData()
        {
            InitializeComponent();

            m_ModelData = ObjectExtensions.Copy(CMainFrame.LWDicer.m_DataManager.ModelData);

            UpdateData();

            this.Text = $"Wafer Data [ Current Model : {CMainFrame.DataManager.ModelData.Name} ]";
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormWaferData_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Save data?"))
            {
                return;
            }

            m_ModelData.Wafer.WaferSize =         Convert.ToDouble(LabelSize.Text);
            m_ModelData.Wafer.WaferThickness =    Convert.ToDouble(LabelThicknessWork.Text);
            m_ModelData.Wafer.TapeThickness =     Convert.ToDouble(LabelThicknessTape.Text);
            m_ModelData.Wafer.Size_X =            Convert.ToDouble(LabelIndexX.Text);
            m_ModelData.Wafer.Size_Y =            Convert.ToDouble(LabelIndexY.Text);

            CMainFrame.LWDicer.SaveModelData(m_ModelData);

            UpdateData();
        }

        private void UpdateData()
        {
            LabelSize.Text =            Convert.ToString(m_ModelData.Wafer.WaferSize);
            LabelThicknessWork.Text =   Convert.ToString(m_ModelData.Wafer.WaferThickness);
            LabelThicknessTape.Text =   Convert.ToString(m_ModelData.Wafer.TapeThickness);
            LabelIndexX.Text =          Convert.ToString(m_ModelData.Wafer.Size_X);
            LabelIndexY.Text =          Convert.ToString(m_ModelData.Wafer.Size_Y);

            LabelSize.ForeColor =           Color.Black;
            LabelThicknessWork.ForeColor =  Color.Black;
            LabelThicknessTape.ForeColor =  Color.Black;
            LabelIndexX.ForeColor =         Color.Black;
            LabelIndexY.ForeColor =         Color.Black;
        }

        private void LabelData_Click(object sender, EventArgs e)
        {
            GradientLabel data = sender as GradientLabel;

            string strCurrent, strModify;

            strCurrent = data.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }
    }
}
