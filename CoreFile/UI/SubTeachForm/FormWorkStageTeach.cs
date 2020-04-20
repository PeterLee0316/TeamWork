using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Layers;
using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_MeStage;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

namespace Core.UI
{
    public partial class FormWorkStageTeach : Form
    {
        ButtonAdv[] StagePos = new ButtonAdv[15]; // Max Teaching Position : 15

        private int m_nSelectedPos_Stage = 0;

        public bool Type_Fixed; // 고정좌표, 옵셋좌표 구분

        private CMovingObject MO_Stage = CMainFrame.mCore.m_MeStage.AxStageInfo;

        public FormWorkStageTeach()
        {
            InitializeComponent();
            
        }

        private void FormWorkStageTeach_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(10, 128);

            this.Text = "Work Stage Part Teaching Screen";
        }

        private void FormWorkStageTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
