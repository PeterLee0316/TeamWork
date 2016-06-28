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
using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Error;

namespace LWDicer.UI
{
    public partial class FormOriginReturn : Form
    {
        private bool [] SelectedAxis = new bool[(int)EAxis.MAX];

        private ButtonAdv[] BtnList = new ButtonAdv[(int)EAxis.MAX];


        public FormOriginReturn()
        {
            InitializeComponent();

            ResouceMapping();
        }

        private void ResouceMapping()
        {
            BtnList[0]  = BtnAxis1;  BtnList[1]  = BtnAxis2;  BtnList[2]  = BtnAxis3;  BtnList[3]  = BtnAxis4;  BtnList[4]  = BtnAxis5;
            BtnList[5]  = BtnAxis6;  BtnList[6]  = BtnAxis7;  BtnList[7]  = BtnAxis8;  BtnList[8]  = BtnAxis9;  BtnList[9]  = BtnAxis10;
            BtnList[10] = BtnAxis11; BtnList[11] = BtnAxis12; BtnList[12] = BtnAxis13; BtnList[13] = BtnAxis14; BtnList[14] = BtnAxis15;
            BtnList[15] = BtnAxis16; BtnList[16] = BtnAxis17; BtnList[17] = BtnAxis18; BtnList[18] = BtnAxis19;

            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                BtnList[i].Tag = i;
                BtnList[i].Image = Image.Images[0];
            }
        }

        private void BtnAxis_Click(object sender, EventArgs e)
        {
            int nNo = 0;

            ButtonAdv BtnList = sender as ButtonAdv;

            nNo = Convert.ToInt16(BtnList.Tag);

            SelectAxis(nNo);
        }

        private void SelectAxis(int nNo)
        {
            if(SelectedAxis[nNo] == false)
            {
                SelectedAxis[nNo] = true;
                BtnList[nNo].Image = Image.Images[1];
            }
            else
            {
                SelectedAxis[nNo] = false;
                BtnList[nNo].Image = Image.Images[0];
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for(int i=0;i< (int)EAxis.MAX; i++)
            {
                SelectedAxis[i] = true;
                BtnList[i].Image = Image.Images[1];
            }

        }

        private void BtnCancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                SelectedAxis[i] = false;
                BtnList[i].Image = Image.Images[0];
            }
        }

        void UpdateUnitStatus()
        {
            bool bStatus;
            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                CMainFrame.LWDicer.m_OpPanel.GetOriginFlag(i, out bStatus);
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

        private void BtnOriginReturn_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "선택한 축에 대해 원점 복귀를 하시겠습니까?"))
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
            int part = (int)EAxis.LOADER_Z;
            if (SelectedAxis[part] == true)
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
            CMainFrame.LWDicer.DisplayMsg("", "초기화가 완료되었습니다.");
        }


        private void BtnServoOn_Click(object sender, EventArgs e)
        {
            if(!CMainFrame.LWDicer.DisplayMsg("", "선택한 축을 Servo On 하시겠습니까?"))
            {
                return;
            }
        }

        private void BtnServoOff_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "선택한 축을 Servo Off 하시겠습니까?"))
            {
                return;
            }
        }

        private void FormOriginReturn_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 100;
            timer1.Start();
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormOriginReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateUnitStatus();
        }

        void SetInitFlag(int sel, bool flag)
        {
            CMainFrame.LWDicer.m_OpPanel.SetInitFlag(sel, flag);
            SelectAxis(sel);
        }

    }
}
