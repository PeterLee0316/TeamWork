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
    public partial class FormSpinnerManualOP : Form
    {
        const int Spinner1 = 0;
        const int Spinner2 = 1;

        private int nSpinner = 0;

        public FormSpinnerManualOP()
        {
            InitializeComponent();
        }

        public void SetSpinner(int nNo)
        {
            nSpinner = nNo;
        }

        public int GetSpinner()
        {
            return nSpinner;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormSpinnerManualOP_Load(object sender, EventArgs e)
        {
            if(GetSpinner() == Spinner1)
            {
                this.Text = "Spinner 1 Manual Operation";
            }

            if (GetSpinner() == Spinner2)
            {
                this.Text = "Spinner 2 Manual Operation";
            }

            TmrManualOP.Enabled = true;
            TmrManualOP.Interval = UITimerInterval;
            TmrManualOP.Start();
        }

        private void FormSpinnerManualOP_FormClosing(object sender, FormClosingEventArgs e)
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
            SensorStatus(GetSpinner());
        }

        private void SensorStatus(int nNo)
        {
            bool bStatus = false;

            if(nNo == Spinner1)
            {
                CMainFrame.LWDicer.m_MeSpinner1.IsAbsorbed(out bStatus);
                if (bStatus == true) BtnVacuumOn.BackColor = Color.LawnGreen; else BtnVacuumOff.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner1.IsReleased(out bStatus);
                if (bStatus == true) BtnVacuumOff.BackColor = Color.LawnGreen; else BtnVacuumOn.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner1.IsChuckTableUp(out bStatus);
                if (bStatus == true) BtnSpinnerUp.BackColor = Color.LawnGreen; else BtnSpinnerDown.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner1.IsChuckTableDown(out bStatus);
                if (bStatus == true) BtnSpinnerUp.BackColor = Color.LawnGreen; else BtnSpinnerDown.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner1.IsCleanNozzleValveOpen(out bStatus);
                if (bStatus == true) BtnCleanNozzleOn.BackColor = Color.LawnGreen; else BtnCleanNozzleOff.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner1.IsCleanNozzleValveClose(out bStatus);
                if (bStatus == true) BtnCleanNozzleOff.BackColor = Color.LawnGreen; else BtnCleanNozzleOn.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner1.IsCoatNozzleValveOpen(out bStatus);
                if (bStatus == true) BtnCoatNozzleOn.BackColor = Color.LawnGreen; else BtnCoatNozzleOff.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner1.IsCoatNozzleValveClose(out bStatus);
                if (bStatus == true) BtnCoatNozzleOff.BackColor = Color.LawnGreen; else BtnCoatNozzleOn.BackColor = Color.LightGray;
            }

            if(nNo == Spinner2)
            {
                CMainFrame.LWDicer.m_MeSpinner2.IsAbsorbed(out bStatus);
                if (bStatus == true) BtnVacuumOn.BackColor = Color.LawnGreen; else BtnVacuumOff.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner2.IsReleased(out bStatus);
                if (bStatus == true) BtnVacuumOff.BackColor = Color.LawnGreen; else BtnVacuumOn.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner2.IsChuckTableUp(out bStatus);
                if (bStatus == true) BtnSpinnerUp.BackColor = Color.LawnGreen; else BtnSpinnerDown.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner2.IsChuckTableDown(out bStatus);
                if (bStatus == true) BtnSpinnerUp.BackColor = Color.LawnGreen; else BtnSpinnerDown.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner2.IsCleanNozzleValveOpen(out bStatus);
                if (bStatus == true) BtnCleanNozzleOn.BackColor = Color.LawnGreen; else BtnCleanNozzleOff.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner2.IsCleanNozzleValveClose(out bStatus);
                if (bStatus == true) BtnCleanNozzleOff.BackColor = Color.LawnGreen; else BtnCleanNozzleOn.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner2.IsCoatNozzleValveOpen(out bStatus);
                if (bStatus == true) BtnCoatNozzleOn.BackColor = Color.LawnGreen; else BtnCoatNozzleOff.BackColor = Color.LightGray;

                CMainFrame.LWDicer.m_MeSpinner2.IsCoatNozzleValveClose(out bStatus);
                if (bStatus == true) BtnCoatNozzleOff.BackColor = Color.LawnGreen; else BtnCoatNozzleOn.BackColor = Color.LightGray;
            }
        }

        private void BtnVacuumOn_Click(object sender, EventArgs e)
        {
            if (GetSpinner() == Spinner1)
            {
                CMainFrame.LWDicer.m_MeSpinner1.Absorb();
            }

            if (GetSpinner() == Spinner2)
            {
                CMainFrame.LWDicer.m_MeSpinner2.Absorb();
            }
        }

        private void BtnVacuumOff_Click(object sender, EventArgs e)
        {
            if (GetSpinner() == Spinner1)
            {
                CMainFrame.LWDicer.m_MeSpinner1.Release();
            }

            if (GetSpinner() == Spinner2)
            {
                CMainFrame.LWDicer.m_MeSpinner2.Release();
            }
        }

        private void BtnSpinnerUp_Click(object sender, EventArgs e)
        {
            if (GetSpinner() == Spinner1)
            {
                CMainFrame.LWDicer.m_MeSpinner1.ChuckTableUp();
            }

            if (GetSpinner() == Spinner2)
            {
                CMainFrame.LWDicer.m_MeSpinner2.ChuckTableUp();
            }
        }

        private void BtnSpinnerDown_Click(object sender, EventArgs e)
        {
            if (GetSpinner() == Spinner1)
            {
                CMainFrame.LWDicer.m_MeSpinner1.ChuckTableDown();
            }

            if (GetSpinner() == Spinner2)
            {
                CMainFrame.LWDicer.m_MeSpinner2.ChuckTableDown();
            }
        }

        private void BtnCleanNozzleOn_Click(object sender, EventArgs e)
        {
            if (GetSpinner() == Spinner1)
            {
                CMainFrame.LWDicer.m_MeSpinner1.CleanNozzleValveOpen();
            }

            if (GetSpinner() == Spinner2)
            {
                CMainFrame.LWDicer.m_MeSpinner2.CleanNozzleValveOpen();
            }
        }

        private void BtnCleanNozzleOff_Click(object sender, EventArgs e)
        {
            if (GetSpinner() == Spinner1)
            {
                CMainFrame.LWDicer.m_MeSpinner1.CleanNozzleValveClose();
            }

            if (GetSpinner() == Spinner2)
            {
                CMainFrame.LWDicer.m_MeSpinner2.CleanNozzleValveClose();
            }
        }

        private void BtnCoatNozzleOn_Click(object sender, EventArgs e)
        {
            if (GetSpinner() == Spinner1)
            {
                CMainFrame.LWDicer.m_MeSpinner1.CoatNozzleValveOpen();
            }

            if (GetSpinner() == Spinner2)
            {
                CMainFrame.LWDicer.m_MeSpinner2.CoatNozzleValveOpen();
            }
        }

        private void BtnCoatNozzleOff_Click(object sender, EventArgs e)
        {
            if (GetSpinner() == Spinner1)
            {
                CMainFrame.LWDicer.m_MeSpinner1.CoatNozzleValveClose();
            }

            if (GetSpinner() == Spinner2)
            {
                CMainFrame.LWDicer.m_MeSpinner2.CoatNozzleValveClose();
            }
        }

        private void BtnCleaningJobStart_Click(object sender, EventArgs e)
        {

        }

        private void BtnCoatingJobStart_Click(object sender, EventArgs e)
        {

        }
    }
}
