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
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;

using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;

using static Core.Layers.MYaskawa;
using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_Yaskawa;
using static Core.Layers.DEF_ACS;
using static Core.Layers.DEF_Error;

using MotionYMC;

namespace Core.UI
{
    public partial class FormJogOperation : Form
    {
        enum EMoveOption
        {
            JOG = 0,
            ABS,
            INC,
            MAX,
        }

        CMotorSpeedData AxisSpeedData = new CMotorSpeedData();

        private int SelectedAxis;

        private bool IsFastMove = false;

        ButtonAdv[] AxisNo = new ButtonAdv[19];
        ButtonAdv[] Velocity = new ButtonAdv[2];

        private EMoveOption m_MoveOption;
        private EMotionSelect m_MotionSelect;
        private string m_strTarget_ABS;
        private string m_strTarget_INC;

        public FormJogOperation()
        {
            InitializeComponent();

            TimerUI.Enabled = true;
            TimerUI.Interval = UITimerInterval;

            ResouceMapping();
        }

        private void ResouceMapping()
        {
            AxisNo[0] = BtnAxis1;
            AxisNo[1] = BtnAxis2;
            AxisNo[2] = BtnAxis3;
            AxisNo[3] = BtnAxis4;
            AxisNo[4] = BtnAxis5;
            AxisNo[5] = BtnAxis6;
            AxisNo[6] = BtnAxis7;
            AxisNo[7] = BtnAxis8;
            AxisNo[8] = BtnAxis9;
            AxisNo[9] = BtnAxis10;
            AxisNo[10] = BtnAxis11;
            AxisNo[11] = BtnAxis12;
            AxisNo[12] = BtnAxis13;
            AxisNo[13] = BtnAxis14;
            AxisNo[14] = BtnAxis15;
            AxisNo[15] = BtnAxis16;
            AxisNo[16] = BtnAxis17;
            AxisNo[17] = BtnAxis18;
            AxisNo[18] = BtnAxis19;

            Velocity[0] = BtnFastVel;
            Velocity[1] = BtnSlowVel;
            
            AxisNo[0].Text  = Convert.ToString(EAxis.LOADER_Z);
            AxisNo[1].Text  = Convert.ToString(EAxis.PUSHPULL_Y);
            AxisNo[2].Text  = Convert.ToString(EAxis.PUSHPULL_X1);
            AxisNo[3].Text  = Convert.ToString(EAxis.PUSHPULL_X2);
            AxisNo[4].Text  = Convert.ToString(EAxis.S1_CHUCK_ROTATE_T);
            AxisNo[5].Text  = Convert.ToString(EAxis.S1_CLEAN_NOZZLE_T);
            AxisNo[6].Text  = Convert.ToString(EAxis.S1_COAT_NOZZLE_T);
            AxisNo[7].Text  = Convert.ToString(EAxis.S2_CHUCK_ROTATE_T);
            AxisNo[8].Text  = Convert.ToString(EAxis.S2_CLEAN_NOZZLE_T);
            AxisNo[9].Text  = Convert.ToString(EAxis.S2_COAT_NOZZLE_T);
            AxisNo[10].Text = Convert.ToString(EAxis.UPPER_HANDLER_X);
            AxisNo[11].Text = Convert.ToString(EAxis.UPPER_HANDLER_Z);
            AxisNo[12].Text = Convert.ToString(EAxis.LOWER_HANDLER_X);
            AxisNo[13].Text = Convert.ToString(EAxis.LOWER_HANDLER_Z);
            AxisNo[14].Text = Convert.ToString(EAxis.CAMERA1_Z);
            AxisNo[15].Text = Convert.ToString(EAxis.SCANNER_Z1);
            AxisNo[16].Text = Convert.ToString(EAxis.STAGE1_X);
            AxisNo[17].Text = Convert.ToString(EAxis.STAGE1_Y);
            AxisNo[18].Text = Convert.ToString(EAxis.STAGE1_T);

#if EQUIP_266_DEV
            AxisNo[15].Text = Convert.ToString(EAxis.CAMERA1_Z);
            AxisNo[14].Text = Convert.ToString(EAxis.SCANNER_Z1);

            BtnAxis15.Tag = 0;
            BtnAxis15.Click -= BtnYMCAxis_Click;
            BtnAxis15.Click += BtnACSAxis_Click;

            BtnAxis16.Tag = 2;
            BtnAxis16.Click -= BtnYMCAxis_Click;
            BtnAxis16.Click += BtnACSAxis_Click;

            BtnAxis17.Tag = 6;
            BtnAxis18.Tag = 4;
            BtnAxis19.Tag = 7;
#endif
        }

        private void FormJogOperation_Load(object sender, EventArgs e)
        {
             this.Text = "Jog Operation";

            IsFastMove = false;
            SetOption(EMoveOption.JOG);

            // set default
            SetYMCAxis((int)EYMC_Axis.LOADER_Z);
            SetVelocity();
            BtnAxis1.BackColor = Color.YellowGreen;


            TimerUI.Start();
        }

        private void FormJogOperation_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            //this.Close();
        }

        private void SetVelocity()
        {
            int speedIndex = (int)EMotorSpeed.MANUAL_SLOW;

            if (m_MoveOption == EMoveOption.JOG)
            {
                speedIndex = IsFastMove ? (int)EMotorSpeed.JOG_FAST: (int)EMotorSpeed.JOG_SLOW;
            } else
            {
                speedIndex = IsFastMove ? (int)EMotorSpeed.MANUAL_FAST: (int)EMotorSpeed.MANUAL_SLOW;
            }

            if (m_MotionSelect == EMotionSelect.YMC)
            {
                AxisSpeedData = CMainFrame.DataManager.SystemData_Axis.MPMotionData[SelectedAxis].Speed[speedIndex];
            }
            else
            {
                int selectAxis = SelectedAxis;
                AxisSpeedData = CMainFrame.DataManager.SystemData_Axis.ACSMotionData[selectAxis].Speed[speedIndex];
            }
            LabelVelocity.Text = Convert.ToString(AxisSpeedData.Vel);

            if (IsFastMove == true)
            {
                // Fast Velocity
                Velocity[0].Image = Image.Images[1];
                Velocity[1].Image = Image.Images[0];
            }
            else
            {
                // Slow Velocity
                Velocity[0].Image = Image.Images[0];
                Velocity[1].Image = Image.Images[1];
            }
        }

        private void BtnYMCAxis_Click(object sender, EventArgs e)
        {
            ButtonAdv Axis = sender as ButtonAdv;

            SetYMCAxis(Convert.ToInt16(Axis.Tag));

            // Button 색상 변경
            Axis.BackColor = Color.YellowGreen;

            SetVelocity();
        }
        private void BtnACSAxis_Click(object sender, EventArgs e)
        {
            ButtonAdv Axis = sender as ButtonAdv;

            SetACSAxis(Convert.ToInt16(Axis.Tag));

            // Button 색상 변경
            Axis.BackColor = Color.YellowGreen;

            SetVelocity();
        }

        private void BtnVel_Click(object sender, EventArgs e)
        {
            string strText = string.Empty;

            ButtonAdv Vel = sender as ButtonAdv;

            strText = Vel.Text.Trim();
            IsFastMove = (strText == "Fast") ? true : false;
            SetVelocity();
        }

        private void SetYMCAxis(int nAxis)
        {
            for (int i = 0; i < AxisNo.Length; i++)
            {
                AxisNo[i].BackColor = Color.FromArgb(224, 224, 224);
            }

            SelectedAxis = nAxis;
            m_MotionSelect = EMotionSelect.YMC;
        }

        private void SetACSAxis(int nAxis)
        {
            for (int i = 0; i < AxisNo.Length; i++)
            {
                AxisNo[i].BackColor = Color.FromArgb(224, 224, 224);
            }


            SelectedAxis = nAxis;
            m_MotionSelect = EMotionSelect.ACS;
        }

        private void SetOption(EMoveOption nOption)
        {
            BtnSelJog.BackColor = Color.FromArgb(224, 224, 224);
            BtnSelAbs.BackColor = Color.FromArgb(224, 224, 224);
            BtnSelInc.BackColor = Color.FromArgb(224, 224, 224);

            m_MoveOption = nOption;
            if (nOption == EMoveOption.JOG)
            {
                BtnAbsMove.Hide();
                BtnPlus.Show();
                BtnMinus.Show();

                BtnSelJog.BackColor = Color.GreenYellow;
            } else if (nOption == EMoveOption.ABS)
            {
                BtnAbsMove.Show();
                BtnPlus.Hide();
                BtnMinus.Hide();

                BtnSelAbs.BackColor = Color.GreenYellow;
            } else if (nOption == EMoveOption.INC)
            {
                BtnAbsMove.Hide();
                BtnPlus.Show();
                BtnMinus.Show();

                BtnSelInc.BackColor = Color.GreenYellow;
            }

            SetVelocity();
        }

        private void LabelTarget_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = LabelTarget.Text;

            if (!CMainFrame.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            if (m_MoveOption == EMoveOption.ABS) m_strTarget_ABS = strModify;
            if (m_MoveOption == EMoveOption.INC) m_strTarget_INC = strModify;

            LabelTarget.Text = strModify;
        }

        private void BtnSelMoveOption_Click(object sender, EventArgs e)
        {
            ButtonAdv Option = sender as ButtonAdv;

            if (Option.Text == "Jog Move")
            {
                LabelTarget.Text = "0";
                LabelTitleTarget.Text = "Target Pos";
                SetOption(EMoveOption.JOG);
            } else if (Option.Text == "Abs Move")
            {
                LabelTarget.Text = m_strTarget_ABS;
                LabelTitleTarget.Text = "Target Pos";
                SetOption(EMoveOption.ABS);
            } else if (Option.Text == "Inc Move")
            {
                LabelTarget.Text = m_strTarget_INC;
                LabelTitleTarget.Text = "Increment Value";
                SetOption(EMoveOption.INC);
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            StopAxis();
        }

        private void MoveAxis(bool bDirection)
        {
            int iResult = SUCCESS;

            if (m_MoveOption == EMoveOption.JOG)
            {
                if (m_MotionSelect == EMotionSelect.YMC)
                {
                    iResult = CMainFrame.Core.m_YMC.StartJogMove(SelectedAxis, bDirection, IsFastMove);
                }
                else
                {
                    iResult = CMainFrame.Core.m_ACS.StartJogMove(SelectedAxis, bDirection, IsFastMove);
                }
            }
            else if (m_MoveOption == EMoveOption.INC)
            {
                double[] dTargetPos = new double[1];

                if(bDirection == DIR_POSITIVE)
                    dTargetPos[0] = Convert.ToDouble(LabelCurrent.Text) + Convert.ToDouble(LabelTarget.Text);
                else dTargetPos[0] = Convert.ToDouble(LabelCurrent.Text) - Convert.ToDouble(LabelTarget.Text);

                if (m_MotionSelect == EMotionSelect.YMC)
                {
                    CMotorSpeedData[] tSpeed = new CMotorSpeedData[1];
                    tSpeed[0] = AxisSpeedData;
                    iResult = CMainFrame.Core.m_YMC.StartMoveToPos(SelectedAxis, dTargetPos, tSpeed);
                }
                else
                {
                    iResult = CMainFrame.Core.m_ACS.MoveToPos(SelectedAxis, dTargetPos[0], AxisSpeedData);
                }
            }
            if (iResult != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult);
            }
        }

        private void StopAxis()
        {
            int iResult = SUCCESS;

            if (m_MoveOption == EMoveOption.JOG)
            {
                if (m_MotionSelect == EMotionSelect.YMC)
                {
                    iResult = CMainFrame.Core.m_YMC.StopJogMove(SelectedAxis);
                }
                else
                {
                    CMainFrame.Core.m_ACS.StopJogMove(SelectedAxis);
                }
            } else
            {
                if (m_MotionSelect == EMotionSelect.YMC)
                {
                    iResult = CMainFrame.Core.m_YMC.StopServoMotion(SelectedAxis);
                }
                else
                {
                    CMainFrame.Core.m_ACS.StopServoMotion(SelectedAxis);
                }
            }
            if (iResult != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult);
            }
        }

        private void BtnPlus_MouseDown(object sender, MouseEventArgs e)
        {
            BtnPlus.BackColor = Color.LightGoldenrodYellow;
            MoveAxis(DIR_POSITIVE);
        }

        private void BtnPlus_MouseUp(object sender, MouseEventArgs e)
        {
            BtnPlus.BackColor = Color.DarkGoldenrod;
            if (m_MoveOption == EMoveOption.JOG)
                StopAxis();
        }

        private void BtnMinus_MouseDown(object sender, MouseEventArgs e)
        {
            BtnMinus.BackColor = Color.LightGoldenrodYellow;
            MoveAxis(DIR_NEGATIVE);
        }

        private void BtnMinus_MouseUp(object sender, MouseEventArgs e)
        {
            BtnMinus.BackColor = Color.DarkGoldenrod;
            if (m_MoveOption == EMoveOption.JOG)
                StopAxis();
        }

        private void BtnAbsMove_MouseClick(object sender, MouseEventArgs e)
        {
            int iResult = SUCCESS;

            double[] dTargetPos = new double[1];
            dTargetPos[0] = Convert.ToDouble(LabelTarget.Text);
            if (m_MotionSelect == EMotionSelect.YMC)
            {
                CMotorSpeedData[] tSpeed = new CMotorSpeedData[1];
                tSpeed[0] = AxisSpeedData;
                iResult = CMainFrame.Core.m_YMC.StartMoveToPos(SelectedAxis, dTargetPos, tSpeed);
            }
            else
            {
                iResult = CMainFrame.Core.m_ACS.MoveToPos(SelectedAxis, dTargetPos[0], AxisSpeedData);
            }
            if (iResult != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult);
            }
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            string strCurPos = string.Empty;

            // Jog Operation Servo Encoder Position
            if (m_MotionSelect == EMotionSelect.YMC)
            {
                LabelCurrent.Text= String.Format("{0:0.0000}",CMainFrame.Core.m_YMC.ServoStatus[SelectedAxis].EncoderPos);
            } else
            {
                LabelCurrent.Text= String.Format("{0:0.0000}", CMainFrame.Core.m_ACS.ServoStatus[SelectedAxis].EncoderPos);
            }
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {

        }

        private void BtnMinus_Click(object sender, EventArgs e)
        {

        }

        private void BtnAbsMove_Click(object sender, EventArgs e)
        {

        }
    }
}
