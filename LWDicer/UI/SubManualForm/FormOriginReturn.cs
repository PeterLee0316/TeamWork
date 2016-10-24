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
        private const int ERR_DLG_MOTORORIGIN_ERROR = 1;

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
                if (BtnList[i] == null) continue;
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
                int length = BtnList[i].Text.IndexOf("[Servo");
                if (length >= 0) BtnList[i].Text.Substring(0, length);
                if (bStatus == true)
                {
                    BtnList[i].Text = BtnName[i] + "\r\n[Servo On]";
                }
                else
                {
                    BtnList[i].Text = BtnName[i] + "\r\n[Servo Off]";
                }

                // 원점 복귀 상태
                CMainFrame.LWDicer.m_OpPanel.GetOriginFlag(i, out bStatus);
                //length = BtnList[i].Text.IndexOf("[Home");
                //if (length >= 0) BtnList[i].Text = BtnList[i].Text.Substring(0, length);
                if (bStatus == true)
                {
                    BtnList[i].Text = BtnList[i].Text + "\r\n[Home On]";
                    BtnList[i].BackColor = Color.Yellow;
                }
                else
                {
                    BtnList[i].Text = BtnList[i].Text + "\r\n[Home Off]";
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

            // 1.
            CMainFrame.LWDicer.m_trsAutoManager.IsInitState = true;
            OriginReturn();

            // 2.
            UpdateUnitStatus();

            // 3. 
            CMainFrame.LWDicer.m_trsAutoManager.IsInitState = false;

        }

        private int ReturnOrigin_SafetyCheck()
        {
            int iResult = SUCCESS;
            bool bStatus;

            // 0. Check EStop
            CMainFrame.LWDicer.m_OpPanel.GetEStopButtonStatus(out bStatus);
            if (bStatus == true)
            {
                CMainFrame.DisplayMsg("EStop S/W Pressed", "Error");
                return ERR_DLG_MOTORORIGIN_ERROR;
            }

            // 1. Check Door
            if (CMainFrame.LWDicer.IsSafeForCylinderMove() == false)
            {
                CMainFrame.DisplayMsg("Door is opend.", "Error");
                return ERR_DLG_MOTORORIGIN_ERROR;
            }

            // 2. Check Other Facility : Dicing 같은 단독설비에서는 check할 필요가 없음

            // 3. check scanner, laser, utility

            // 4. Move Cylinder to Safety Pos
            // 4.1 Handler Up
            //if ((iResult = m_pC_CtrlUHandler.Up()) != SUCCESS)
            //{
            //    return iResult;
            //}

            // 5. Check Wafer
            // 5.1 Stage
            if (SelectedAxis[(int)EAxis.STAGE1_X] == true
                || SelectedAxis[(int)EAxis.STAGE1_Y] == true
                || SelectedAxis[(int)EAxis.STAGE1_T] == true)
            {
                iResult = CMainFrame.LWDicer.m_ctrlStage1.IsObjectDetected(out bStatus);
                if (bStatus)
                {
                    if (CMainFrame.InquireMsg("Stage1에 Wafer가 감지됩니다. 원점복귀 동작중 Wafer가 충돌할수 있습니다.\n원점복귀 동작을 계속 수행하시겠습니까?", "Confirm"))
                        return ERR_DLG_MOTORORIGIN_ERROR;

                    //iResult = CMainFrame.LWDicer.m_ctrlStage1.Absorb();
                    //if (iResult != SUCCESS) return iResult;
                }
            }

            // 5.2 Spinner

            // 5.3 Handler

            // 5.4 PushPull

            // 5.5 Loader

            // 6. Etc

            return SUCCESS;
        }

        private void OriginReturn()
        {
            int iResult = SUCCESS;

            // 1. Check Safety
            // 1.1 Check Safety
            SetTitle("Check Safety");
            iResult = ReturnOrigin_SafetyCheck();
            if (iResult != SUCCESS) goto ERROR_PROCESS;

            // 1.2 Check Motor Limit Sensor
            SetTitle("Check Limit Sensor");
            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                if (SelectedAxis[i] == false) continue;

                if(CMainFrame.LWDicer.CheckAxisSensorLimit(i) == false)
                {
                    CMainFrame.DisplayMsg("Axis sensor status is abnormal", "Error");
                    goto ERROR_PROCESS;
                }
            }

            // 2. Return Origin
            SetTitle("Origin Return");
            bool bRunCheckBit = false;

            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                if (SelectedAxis[i] == false) continue;

                bRunCheckBit = true;
                iResult = CMainFrame.LWDicer.m_ctrlOpPanel.OriginReturn(i);
                if (iResult != SUCCESS) goto ERROR_PROCESS;
            }

            //if (bRunCheckBit) CMainFrame.DisplayAlarm(SUCCESS);

            // 6. 
            SetTitle("HomeReturn Finished");
            CMainFrame.DisplayMsg("HomeReturn Finished Successfully");

            return;
            
            ERROR_PROCESS:
            SetTitle("HomeReturn Failed");
            if (iResult != ERR_DLG_MOTORORIGIN_ERROR)
            {
                CMainFrame.DisplayAlarm(iResult);
            }
        }

        private void SetTitle(string str)
        {
            LabelProgress.Text  = String.Format($"ElapsedTime : {m_waitTimer},  Progress : {str}");
            LabelProgress.Refresh();
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
            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;
            TimerUI.Start();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormOriginReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            UpdateUnitStatus();
        }

        void SetInitFlag(int sel, bool flag)
        {
            CMainFrame.LWDicer.m_OpPanel.SetInitFlag(sel, flag);
            SelectAxis(sel);
        }

        private void buttonAdv1_Click(object sender, EventArgs e)
        {
            // 1.2 Check Motor Limit Sensor
            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                if (SelectedAxis[i] == false) continue;

                CMainFrame.LWDicer.m_OpPanel.SetOriginFlag(i, true);
            }

            // 6. 
            SetTitle("HomeReturn Finished");
            CMainFrame.DisplayMsg("HomeReturn Finished Successfully");

        }

        private void buttonAdv2_Click(object sender, EventArgs e)
        {
            // 1.2 Check Motor Limit Sensor
            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                if (SelectedAxis[i] == false) continue;

                CMainFrame.LWDicer.m_OpPanel.SetOriginFlag(i, false);
            }
        }
    }
}
