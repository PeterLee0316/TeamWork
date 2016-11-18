using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LWDicer.Layers;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_CtrlHandler;

namespace LWDicer.UI
{
    public partial class FormHandlerManualOP : Form
    {
        private EHandlerIndex m_HandlerIndex = 0;
        private MMeHandler m_MeHandler;

        public FormHandlerManualOP()
        {
            InitializeComponent();
        }

        public void SetHandler(EHandlerIndex index)
        {
            m_HandlerIndex = index;
            if(index == EHandlerIndex.LOAD_UPPER)
            {
                m_MeHandler = CMainFrame.LWDicer.m_MeUpperHandler;
            } else
            {
                m_MeHandler = CMainFrame.LWDicer.m_MeLowerHandler;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormHandlerManualOP_Load(object sender, EventArgs e)
        {
            if (m_HandlerIndex == EHandlerIndex.LOAD_UPPER)
            {
                this.Text = "Upper Handler Manual Operation";
            } else 
            {
                this.Text = "Lower Handler Manual Operation";
            }

            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        private void FormHandlerManualOP_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            bool bStatus = false;

            m_MeHandler.IsAbsorbed(out bStatus);
            BtnVacuumOn.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;

            m_MeHandler.IsReleased(out bStatus);
            BtnVacuumOff.BackColor = (bStatus == true) ? CMainFrame.BtnBackColor_On : CMainFrame.BtnBackColor_Off;
        }

        private void BtnVacuumOn_Click(object sender, EventArgs e)
        {
            CMainFrame.StartTimer();
            int iResult = m_MeHandler.Absorb();
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_Vac.Text = CMainFrame.GetElapsedTIme_Text();
        }

        private void BtnVacuumOff_Click(object sender, EventArgs e)
        {
            CMainFrame.StartTimer();
            int iResult = m_MeHandler.Release(false);
            CMainFrame.DisplayAlarm(iResult);
            LabelTime_Vac.Text = CMainFrame.GetElapsedTIme_Text();
        }
    }
}
