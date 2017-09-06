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
using Core.Layers;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Error;

namespace Core.UI
{
    public partial class FormUnitInit : Form
    {

        private bool[] SelectedPart = new bool[(int)EThreadUnit.MAX];
        private ButtonAdv[] BtnList = new ButtonAdv[(int)EThreadUnit.MAX];
        private string[] BtnName = new string[(int)EThreadUnit.MAX];

        public FormUnitInit()
        {
            InitializeComponent();

            ResouceMapping();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
            BtnList[(int)EThreadUnit.AUTOMANAGER] = BtnAutoManager;
            BtnList[(int)EThreadUnit.LOADER]      = BtnLoader;
            BtnList[(int)EThreadUnit.SPINNER1]    = BtnSpinner1;
            BtnList[(int)EThreadUnit.SPINNER2]    = BtnSpinner2;
            BtnList[(int)EThreadUnit.HANDLER]     = BtnHandler;
            BtnList[(int)EThreadUnit.PUSHPULL]    = BtnPushPull;
            BtnList[(int)EThreadUnit.STAGE1]      = BtnStage;


            for (int i = 0; i < (int)EThreadUnit.MAX; i++)
            {
                BtnName[i] = BtnList[i].Text;
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
            for (int i = 0; i < (int)EThreadUnit.MAX; i++)
            {
                SelectedPart[i] = true;
                BtnList[i].Image = Image.Images[1];
            }
        }

        private void BtnCancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EThreadUnit.MAX; i++)
            {
                SelectedPart[i] = false;
                BtnList[i].Image = Image.Images[0];
            }
        }

        void UpdateUnitStatus()
        {
            bool bStatus;
            for (int i = 0; i < (int)EThreadUnit.MAX; i++)
            {
                CMainFrame.Core.m_OpPanel.GetInitFlag(i, out bStatus);
                if (bStatus == true)
                {
                    BtnList[i].Text = BtnName[i] + "\r\n[Init On]";
                    BtnList[i].BackColor = Color.Yellow;
                }
                else
                {
                    BtnList[i].Text = BtnName[i] + "\r\n[Init Off]";
                    BtnList[i].BackColor = Color.AntiqueWhite;
                }
            }
        }

        private void BtnExecuteIni_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Do part initialization?"))
            {
                return;
            }

            // 0. init
            CMainFrame.StartTimer();
            CMainFrame.Core.m_trsAutoManager.IsInitState = true;

            // 1.
            InitRun();

            // 2.
            UpdateUnitStatus();

            // 3.
            CMainFrame.Core.m_trsAutoManager.IsInitState = false;
        }

        private void SetTitle(string str)
        {
            LabelProgress.Text = String.Format($"ElapsedTime : {CMainFrame.GetElapsedTIme_Text()},  Progress : {str}");
            LabelProgress.Refresh();
        }

        private void InitRun()
        {
            int iResult = SUCCESS;
            string strTemp;
            bool bSts = false;
            bool bRtnSts = false;
            //	bool rgbOriginSts[DEF_MAX_MOTION_AXIS_NO];
            string strErr;

            SetTitle("Check Safety");
            // EStop 및 모든 축 원점 복귀 체크
            if (CMainFrame.Core.IsSafeForAxisMove() == false)
                return;

            SetTitle("인터페이스 신호 초기화");

            int part;

            // 0. AUTOMANAGER
            SetTitle("AUTOMANAGER");
            part = (int)EThreadUnit.AUTOMANAGER;
            SelectedPart[part] = true; // AutoManager는 invisible button. always init
            if (SelectedPart[part] == true)
            {
                iResult = CMainFrame.Core.m_trsAutoManager.Initialize();
                if (iResult != SUCCESS)
                {
                    CMainFrame.DisplayAlarm(iResult);
                    SetInitFlag(part, false);
                    return;
                }
                else
                {
                    SetInitFlag(part, true);
                }
            }

            // 0. HANDLER
            SetTitle("HANDLER");
            part = (int)EThreadUnit.HANDLER;
            if (SelectedPart[part] == true)
            {
                iResult = CMainFrame.Core.m_trsHandler.Initialize();
                if (iResult != SUCCESS)
                {
                    CMainFrame.DisplayAlarm(iResult);
                    SetInitFlag(part, false);
                    return;
                }
                else
                {
                    SetInitFlag(part, true);
                }
            }

            // 0. STAGE1
            SetTitle("STAGE1");
            part = (int)EThreadUnit.STAGE1;
            if (SelectedPart[part] == true)
            {
                iResult = CMainFrame.Core.m_trsStage1.Initialize();
                if (iResult != SUCCESS)
                {
                    CMainFrame.DisplayAlarm(iResult);
                    SetInitFlag(part, false);
                    return;
                }
                else
                {
                    SetInitFlag(part, true);
                }
            }

            // 0. SPINNER1
            SetTitle("SPINNER1");
            part = (int)EThreadUnit.SPINNER1;
            if (SelectedPart[part] == true)
            {
                iResult = CMainFrame.Core.m_trsSpinner1.Initialize();
                if (iResult != SUCCESS)
                {
                    CMainFrame.DisplayAlarm(iResult);
                    SetInitFlag(part, false);
                    return;
                }
                else
                {
                    SetInitFlag(part, true);
                }
            }

            // 0. SPINNER2
            SetTitle("SPINNER2");
            part = (int)EThreadUnit.SPINNER2;
            if (SelectedPart[part] == true)
            {
                iResult = CMainFrame.Core.m_trsSpinner2.Initialize();
                if (iResult != SUCCESS)
                {
                    CMainFrame.DisplayAlarm(iResult);
                    SetInitFlag(part, false);
                    return;
                }
                else
                {
                    SetInitFlag(part, true);
                }
            }

            // 0. Loader
            SetTitle("LOADER");
            part = (int)EThreadUnit.LOADER;
            if (SelectedPart[part] == true)
            {
                iResult = CMainFrame.DataManager.Generate_InputBuffer();
                iResult = CMainFrame.Core.m_trsLoader.Initialize();
                if (iResult != SUCCESS)
                {
                    CMainFrame.DisplayAlarm(iResult);
                    SetInitFlag(part, false);
                    return;
                }
                else
                {
                    SetInitFlag(part, true);
                }
            }

            // 0. PUSHPULL
            SetTitle("PUSHPULL");
            part = (int)EThreadUnit.PUSHPULL;
            if (SelectedPart[part] == true)
            {
                iResult = CMainFrame.Core.m_trsPushPull.Initialize();
                if (iResult != SUCCESS)
                {
                    CMainFrame.DisplayAlarm(iResult);
                    SetInitFlag(part, false);
                    return;
                }
                else
                {
                    SetInitFlag(part, true);
                }
            }

            // Last.
            CMainFrame.DisplayMsg("Initialization completed successfully.");
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            UpdateUnitStatus();

        }

        private void FormUnitInit_Load(object sender, EventArgs e)
        {
            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        void SetInitFlag(int sel, bool flag)
        {
            CMainFrame.Core.m_OpPanel.SetInitFlag(sel, flag);
            SelectPart(sel);
        }

        private void buttonAdv1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EThreadUnit.MAX; i++)
            {
                SetInitFlag(i, true);
                CMainFrame.Core.Sleep(100);
            }
            // Last.
            CMainFrame.DisplayMsg("Initialization completed successfully.");
        }
    }
}
