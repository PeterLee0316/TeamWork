using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;

namespace LWDicer.UI
{
    public partial class FormHandlerManualOP : Form
    {
        const int LoHandler = 0;
        const int UpHandler = 1;

        private int nHandler = 0;

        public FormHandlerManualOP()
        {
            InitializeComponent();
        }

        public void SetHandler(int nNo)
        {
            nHandler = nNo;
        }

        public int GetHandler()
        {
            return nHandler;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormHandlerManualOP_Load(object sender, EventArgs e)
        {
            if (GetHandler() == LoHandler)
            {
                this.Text = "Lower Handler Manual Operation";
            }

            if (GetHandler() == UpHandler)
            {
                this.Text = "Upper Handler Manual Operation";
            }

            TmrManualOP.Enabled = true;
            TmrManualOP.Interval = UITimerInterval;
            TmrManualOP.Start();
        }

        private void FormHandlerManualOP_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void FormClose()
        {
            TmrManualOP.Stop();
            this.Hide();
        }

        private void TmrManualOP_Tick(object sender, EventArgs e)
        {
            SensorStatus(GetHandler());
        }

        private void SensorStatus(int nNo)
        {
            bool bStatus = false;

            if (nNo == LoHandler)
            {
                CMainFrame.LWDicer.m_MeLowerHandler.IsAbsorbed(out bStatus);
                if (bStatus == true) BtnVacuumOn.BackColor = Color.LawnGreen; else BtnVacuumOff.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeLowerHandler.IsReleased(out bStatus);
                if (bStatus == true) BtnVacuumOff.BackColor = Color.LawnGreen; else BtnVacuumOn.BackColor = Color.LightGray;
            }

            if(nNo == UpHandler)
            {
                CMainFrame.LWDicer.m_MeUpperHandler.IsAbsorbed(out bStatus);
                if (bStatus == true) BtnVacuumOn.BackColor = Color.LawnGreen; else BtnVacuumOff.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeUpperHandler.IsReleased(out bStatus);
                if (bStatus == true) BtnVacuumOff.BackColor = Color.LawnGreen; else BtnVacuumOn.BackColor = Color.LightGray;
            }
        }

        private void BtnVacuumOn_Click(object sender, EventArgs e)
        {
            if(GetHandler() == LoHandler)
            {
                CMainFrame.LWDicer.m_MeLowerHandler.Absorb();
            }

            if(GetHandler() == UpHandler)
            {
                CMainFrame.LWDicer.m_MeUpperHandler.Absorb();
            }
        }

        private void BtnVacuumOff_Click(object sender, EventArgs e)
        {
            if (GetHandler() == LoHandler)
            {
                CMainFrame.LWDicer.m_MeLowerHandler.Release(false);
            }

            if (GetHandler() == UpHandler)
            {
                CMainFrame.LWDicer.m_MeUpperHandler.Release(false);
            }
        }
    }
}
