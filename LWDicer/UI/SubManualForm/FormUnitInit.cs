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
using LWDicer.Control;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Error;

namespace LWDicer.UI
{
    public partial class FormUnitInit : Form
    {

        private bool[] SelectedPart = new bool[(int)EInitiableUnit.MAX];

        private ButtonAdv[] BtnList = new ButtonAdv[(int)EInitiableUnit.MAX];

        public FormUnitInit()
        {
            InitializeComponent();

            ResouceMapping();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BtnInitPart_Click(object sender, EventArgs e)
        {
            int nNo = 0;

            ButtonAdv BtnList = sender as ButtonAdv;

            nNo = Convert.ToInt16(BtnList.Tag);

            SelectPart(nNo);
        }

        private void ResouceMapping()
        {
            BtnList[0] = BtnLoader;
            BtnList[1] = BtnSpinner1;
            BtnList[2] = BtnSpinner2;
            BtnList[3] = BtnHandler;
            BtnList[4] = BtnPushPull;
            BtnList[5] = BtnStage;


            for (int i = 0; i < (int)EInitiableUnit.MAX; i++)
            {
                BtnList[i].Tag = i;
                BtnList[i].Image = Image.Images[0];
            }
        }

        private void SelectPart(int nNo)
        {
            if (SelectedPart[nNo] == false)
            {
                SelectedPart[nNo] = true;
                BtnList[nNo].Image = Image.Images[1];
            }
            else
            {
                SelectedPart[nNo] = false;
                BtnList[nNo].Image = Image.Images[0];
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EInitiableUnit.MAX; i++)
            {
                SelectedPart[i] = true;
                BtnList[i].Image = Image.Images[1];
            }
        }

        private void BtnCancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EInitiableUnit.MAX; i++)
            {
                SelectedPart[i] = false;
                BtnList[i].Image = Image.Images[0];
            }
        }

        void UpdateUnitStatus()
        {
            bool bStatus;
            for (int i = 0; i < (int)EInitiableUnit.MAX; i++)
            {
                CMainFrame.LWDicer.m_OpPanel.GetInitFlag(i, out bStatus);
                if (bStatus == true)
                {
                    BtnList[i].BackColor = Color.Yellow;
                }
                else
                {
                    BtnList[i].BackColor = Color.AntiqueWhite;
                }
            }
        }

        private void BtnExecuteIni_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", 30))
            {
                return;
            }

            // 0. set init state to true
            CMainFrame.LWDicer.m_trsAutoManager.IsInitState = true;

            // 1.
            InitRun();

            // 2.
            UpdateUnitStatus();

            // 3. set init state to false
            CMainFrame.LWDicer.m_trsAutoManager.IsInitState = false;
        }

        private void ShowStepInform(string inform)
        {
            //m_lblStatus.SetWindowText(inform);
            //m_lblStatus.Refresh();
        }

        private void InitRun()
        {
            if (SelectedPart[(int)EInitiableUnit.LOADER] == true)
            {
                //CMainFrame.LWDicer.m_trs
            }

            int i = 0;
            int iResult = SUCCESS;
            string strTemp;
            bool bSts = false;
            bool bRtnSts = false;
            //	bool rgbOriginSts[DEF_MAX_MOTION_AXIS_NO];

            string strErr;
            // EStop 및 모든 축 원점 복귀 체크
            if (CMainFrame.LWDicer.IsSafeForAxisMove() == false)
                return;

            ShowStepInform("진행 상황 : 인터페이스 신호 초기화");

            // 0. Loader
            ShowStepInform("진행 상황 : LOADER");
            int part = (int)EInitiableUnit.LOADER;
            if (SelectedPart[part] == true)
            {
                // LOADER Unit 초기화 
                if ((iResult = CMainFrame.LWDicer.m_trsLoader.Initialize()) != SUCCESS)
                {
                    // Display Alarm

                    SetInitFlag(part, false);
                    return;
                }
                else
                {
                    SetInitFlag(part, true);
                }
            }


            // Last.
            CMainFrame.LWDicer.DisplayMsg("", 27);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateUnitStatus();

        }

        private void FormUnitInit_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 100;
            timer1.Start();
        }

        void SetInitFlag(int sel, bool flag)
        {
            CMainFrame.LWDicer.m_OpPanel.SetInitFlag(sel, flag);
            SelectPart(sel);
        }

    }
}
