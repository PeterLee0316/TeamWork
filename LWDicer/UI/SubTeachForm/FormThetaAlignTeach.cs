using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormThetaAlignTeach : Form
    {
        public FormThetaAlignTeach()
        {
            InitializeComponent();

            CMainFrame.frmStageJog.Location = new Point(0, 0);
            CMainFrame.frmStageJog.TopLevel = false;
            this.Controls.Add(CMainFrame.frmStageJog);
            CMainFrame.frmStageJog.Parent = this.pnlStageJog;
            CMainFrame.frmStageJog.Dock = DockStyle.Fill;
            CMainFrame.frmStageJog.Show();
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormThetaAlignTeach_Load(object sender, EventArgs e)
        {
#if !SIMULATION_VISION
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, picVision.Handle);
#endif

            DisplayParameter();
        }

        private void DisplayParameter()
        {
            lblCamOffSetAxisX.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.CamEachOffsetX);
            lblCamOffSetAxisY.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.CamEachOffsetY);

            lblMacroIndexAxisX.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.MacroScreenWidth);
            lblMacroIndexAxisY.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.MacroScreenHeight);
            lblMacroIndexAxisT.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.MacroScreenRotate);

            lblMicroIndexAxisX.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.MicroScreenWidth);
            lblMicroIndexAxisY.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.MicroScreenHeight);
            lblMicroIndexAxisT.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.MicroScreenRotate);

            lblDieIndexAxisX.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.DieIndexWidth);
            lblDieIndexAxisY.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.DieIndexHeight);
            lblDieIndexAxisT.Text = string.Format("{0:F3}", CMainFrame.DataManager.SystemData_Align.DieIndexRotate);
        }

        private void UpdateParameter()
        {
            CMainFrame.DataManager.SystemData_Align.CamEachOffsetX = Convert.ToSingle(lblCamOffSetAxisX.Text);
            CMainFrame.DataManager.SystemData_Align.CamEachOffsetY = Convert.ToSingle(lblCamOffSetAxisY.Text);

            CMainFrame.DataManager.SystemData_Align.MacroScreenWidth  = Convert.ToSingle(lblMacroIndexAxisX.Text);
            CMainFrame.DataManager.SystemData_Align.MacroScreenHeight = Convert.ToSingle(lblMacroIndexAxisY.Text);
            CMainFrame.DataManager.SystemData_Align.MacroScreenRotate = Convert.ToSingle(lblMacroIndexAxisT.Text);

            CMainFrame.DataManager.SystemData_Align.MicroScreenWidth  = Convert.ToSingle(lblMicroIndexAxisX.Text);
            CMainFrame.DataManager.SystemData_Align.MicroScreenHeight = Convert.ToSingle(lblMicroIndexAxisY.Text);
            CMainFrame.DataManager.SystemData_Align.MicroScreenRotate = Convert.ToSingle(lblMicroIndexAxisT.Text);

            CMainFrame.DataManager.SystemData_Align.DieIndexWidth  = Convert.ToSingle(lblDieIndexAxisX.Text);
            CMainFrame.DataManager.SystemData_Align.DieIndexHeight = Convert.ToSingle(lblDieIndexAxisY.Text);
            CMainFrame.DataManager.SystemData_Align.DieIndexRotate = Convert.ToSingle(lblDieIndexAxisT.Text);

        }
        private void btnConfigSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg("Parameter 데이터를 저장하시겠습니까 ?")) return;


            UpdateParameter();


            CMainFrame.DataManager.SaveSystemData(null, null,null, null, CMainFrame.DataManager.SystemData_Align, null, null);
        }

        private void ChangeTextData(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            GradientLabel Btn = sender as GradientLabel;
            strCurrent = Btn.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            Btn.Text = strModify;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnChangeCam_Click(object sender, EventArgs e)
        {
            if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == FINE_CAM)
                CMainFrame.LWDicer.m_ctrlStage1.ChangeMacroVision(picVision.Handle);
            else if (CMainFrame.LWDicer.m_ctrlStage1.GetCurrentCam() == PRE__CAM)
                CMainFrame.LWDicer.m_ctrlStage1.ChangeMicroVision(picVision.Handle);
            else
                CMainFrame.DisplayMsg("Cam not defined");


        }
    }
}
