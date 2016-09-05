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
using LWDicer.Layers;
using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Error;

namespace LWDicer.UI
{  
    public partial class FormOriginReturn : Form
    {
        private bool [] SelectedAxis = new bool[(int)EAxis.MAX];

        private ButtonAdv[] BtnList = new ButtonAdv[(int)EAxis.MAX];
        private string[] BtnName = new string[(int)EAxis.MAX];
        private MTickTimer m_waitTimer = new MTickTimer();


        public FormOriginReturn()
        {
            InitializeComponent();

            ResouceMapping();
        }

        private void ResouceMapping()
        {
            int index = 0;
            BtnList[index++]  = BtnAxis1;  BtnList[index++]  = BtnAxis2;  BtnList[index++]  = BtnAxis3;  BtnList[index++]  = BtnAxis4;  BtnList[index++]  = BtnAxis5;
            BtnList[index++]  = BtnAxis6;  BtnList[index++]  = BtnAxis7;  BtnList[index++]  = BtnAxis8;  BtnList[index++]  = BtnAxis9;  BtnList[index++]  = BtnAxis10;
            BtnList[index++] = BtnAxis11; BtnList[index++] = BtnAxis12; BtnList[index++] = BtnAxis13; BtnList[index++] = BtnAxis14; BtnList[index++] = BtnAxis15;
            BtnList[index++] = BtnAxis16; BtnList[index++] = BtnAxis17; BtnList[index++] = BtnAxis18; BtnList[index++] = BtnAxis19;

            if(BtnList.Length > index)
            {
                BtnList[index++] = BtnAxis20;
            } else
            {
                BtnAxis20.Visible = false;
            }

            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                BtnName[i] = BtnList[i].Text;

                //BtnList[i].Tag = i;
                if (BtnList[i] == null) continue;
                BtnList[i].Image = Image.Images[0];
            }
        }

        private void BtnAxis_Click(object sender, EventArgs e)
        {
            int nNo = 0;

            ButtonAdv BtnList = sender as ButtonAdv;

            if (BtnList == null) return;

            nNo = Convert.ToInt16(BtnList.Tag);

            SelectAxis(nNo);

            if (SelectedAxis[nNo] == true)
            {
                BtnList.Image = Image.Images[1];
            }
            else
            {
                BtnList.Image = Image.Images[0];
            }
        }
        

        private void SelectAxis(int nNo)
        {
            if(SelectedAxis[nNo] == false)
            {
                SelectedAxis[nNo] = true;
            }
            else
            {
                SelectedAxis[nNo] = false;
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for(int i=0;i< (int)EAxis.MAX; i++)
            {
                SelectedAxis[i] = true;
                if (BtnList[i] == null) continue;
                BtnList[i].Image = Image.Images[1];
            }
        }

        private void BtnCancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                SelectedAxis[i] = false;
                if (BtnList[i] == null) continue;
                BtnList[i].Image = Image.Images[0];
            }
        }

        void UpdateUnitStatus()
        {
            bool bStatus;
            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                if (BtnList[i] == null) continue;

                // 서보 on/off 상태
                CMainFrame.LWDicer.m_OpPanel.GetServoOnStatus(i, out bStatus);
                if (bStatus == true)
                {
                    if (BtnList[i].Text.IndexOf("[On]") < 0) BtnList[i].Text = BtnName[i] + "\r\n[On]";
                }
                else
                {
                    if (BtnList[i].Text.IndexOf("[Off]") < 0) BtnList[i].Text = BtnName[i] + "\r\n[Off]";
                }

                // 원점 복귀 상태
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
            if (!CMainFrame.InquireMsg("Do axis home retrurn?"))
            {
                return;
            }

            // 0. init
            m_waitTimer.StartTimer();
            CMainFrame.LWDicer.m_trsAutoManager.IsInitState = true;

            // 1.
            //InitRun();
            OriginReturn();
            // 2.
            UpdateUnitStatus();

            // 3. 
            CMainFrame.LWDicer.m_trsAutoManager.IsInitState = false;

        }

        private void ShowStepInform(string str)
        {
            LabelProgress.Text  = String.Format($"ElapsedTime : {m_waitTimer},  Progress : {str}");
            LabelProgress.Refresh();
        }

        private void OriginReturn()
        {
            bool bRunCheckBit = false;

            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                if (SelectedAxis[i] == false) continue;

                bRunCheckBit = true;
                int iResult = CMainFrame.LWDicer.m_ctrlOpPanel.OriginReturn(i);
                if (iResult != SUCCESS)
                    CMainFrame.DisplayAlarm(iResult);
            }

            //if (bRunCheckBit) CMainFrame.DisplayAlarm(SUCCESS);
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

            ShowStepInform("Check Safety");
            
            // EStop 및 모든 축 원점 복귀 체크
            if (CMainFrame.LWDicer.IsSafeForAxisMove() == false)
                return;

            ShowStepInform("인터페이스 신호 초기화");

            //// 0. Loader
            //ShowStepInform("LOADER");
            //int part = (int)EAxis.LOADER_Z;
            //if (SelectedAxis[part] == true)
            //{
            //    // LOADER Unit 초기화 
            //    if ((iResult = CMainFrame.LWDicer.m_trsLoader.Initialize()) != SUCCESS)
            //    {
            //        // Display Alarm
            //        SetInitFlag(part, false);
            //        return;
            //    }
            //    else
            //    {
            //        SetInitFlag(part, true);
            //    }
            //}

            // Last.
            CMainFrame.DisplayMsg("Initialization completed.");
        }


        private void BtnServoOn_Click(object sender, EventArgs e)
        {
            bool bRunCheckBit = false;
            if (!CMainFrame.InquireMsg("On selected servo?"))
            {
                return;
            }

            for(int i = 0; i < (int)EAxis.MAX; i++)
            {
                if (SelectedAxis[i] == false) continue;

                bRunCheckBit = true;

                int iResult = CMainFrame.LWDicer.m_ctrlOpPanel.ServoOn(i);
                if(iResult != SUCCESS) CMainFrame.DisplayAlarm(iResult);
            }
            //if(bRunCheckBit)  CMainFrame.DisplayAlarm(SUCCESS);
        }

        private void BtnServoOff_Click(object sender, EventArgs e)
        {
            bool bRunCheckBit = false;

            if (!CMainFrame.InquireMsg("Off selected servo?"))
            {
                return;
            }

            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                if (SelectedAxis[i] == false) continue;

                bRunCheckBit = true;
                int iResult = CMainFrame.LWDicer.m_ctrlOpPanel.ServoOff(i);
                if (iResult != SUCCESS)  CMainFrame.DisplayAlarm(iResult);
            }
            if (bRunCheckBit) CMainFrame.DisplayAlarm(SUCCESS);
        }

        private void FormOriginReturn_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = UITimerInterval;
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
