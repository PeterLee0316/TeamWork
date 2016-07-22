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

using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormWaferData : Form
    {
        private CModelData WaferData;

        public FormWaferData()
        {
            InitializeComponent();

            WaferData = ObjectExtensions.Copy(CMainFrame.LWDicer.m_DataManager.ModelData);

            UpdateData();

            this.Text = $"Wafer Data [ Current Model : {CMainFrame.DataManager.ModelData.Name} ]";
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormWaferData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("Save Data?"))
            {
                return;
            }

            WaferData.Wafer.WaferSize =         Convert.ToDouble(LabelSize.Text);
            WaferData.Wafer.WaferThickness =    Convert.ToDouble(LabelThicknessWork.Text);
            WaferData.Wafer.TapeThickness =     Convert.ToDouble(LabelThicknessTape.Text);
            WaferData.Wafer.Size_X =            Convert.ToDouble(LabelIndexX.Text);
            WaferData.Wafer.Size_Y =            Convert.ToDouble(LabelIndexY.Text);

            CMainFrame.DataManager.SaveModelData(WaferData);

            UpdateData();
        }

        private void UpdateData()
        {
            LabelSize.Text =            Convert.ToString(WaferData.Wafer.WaferSize);
            LabelThicknessWork.Text =   Convert.ToString(WaferData.Wafer.WaferThickness);
            LabelThicknessTape.Text =   Convert.ToString(WaferData.Wafer.TapeThickness);
            LabelIndexX.Text =          Convert.ToString(WaferData.Wafer.Size_X);
            LabelIndexY.Text =          Convert.ToString(WaferData.Wafer.Size_Y);

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

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            data.Text = strModify;
            data.ForeColor = Color.Red;
        }
    }
}
