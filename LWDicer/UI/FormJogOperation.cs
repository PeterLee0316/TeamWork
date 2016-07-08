﻿using System;
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

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;

using static LWDicer.Control.MYaskawa;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_ACS;
using static LWDicer.Control.DEF_Error;

using MotionYMC;

namespace LWDicer.UI
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

        private int AxisMoveOption;

        private double TargetPos;

        private int SelectedAxis;

        private bool IsFastMove = false;

        ButtonAdv[] AxisNo = new ButtonAdv[19];
        ButtonAdv[] Velocity = new ButtonAdv[2];


        public FormJogOperation()
        {
            InitializeComponent();

            TmrJog.Enabled = true;
            TmrJog.Interval = 100;

            ResouceMapping();
        }

        private void FormClose()
        {
            TmrJog.Stop();
            this.Hide();
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
            
            AxisNo[0].Text = Convert.ToString(EYMC_Axis.LOADER_Z);
            AxisNo[1].Text = Convert.ToString(EYMC_Axis.PUSHPULL_Y);
            AxisNo[2].Text = Convert.ToString(EYMC_Axis.PUSHPULL_X1);
            AxisNo[3].Text = Convert.ToString(EYMC_Axis.PUSHPULL_X2);
            AxisNo[4].Text = Convert.ToString(EYMC_Axis.S1_CHUCK_ROTATE_T);
            AxisNo[5].Text = Convert.ToString(EYMC_Axis.S1_CLEAN_NOZZLE_T);
            AxisNo[6].Text = Convert.ToString(EYMC_Axis.S1_COAT_NOZZLE_T);
            AxisNo[7].Text = Convert.ToString(EYMC_Axis.S2_CHUCK_ROTATE_T);
            AxisNo[8].Text = Convert.ToString(EYMC_Axis.S2_CLEAN_NOZZLE_T);
            AxisNo[9].Text = Convert.ToString(EYMC_Axis.S2_COAT_NOZZLE_T);
            AxisNo[10].Text = Convert.ToString(EYMC_Axis.UPPER_HANDLER_X);
            AxisNo[11].Text = Convert.ToString(EYMC_Axis.UPPER_HANDLER_Z);
            AxisNo[12].Text = Convert.ToString(EYMC_Axis.LOWER_HANDLER_X);
            AxisNo[13].Text = Convert.ToString(EYMC_Axis.LOWER_HANDLER_Z);
            AxisNo[14].Text = Convert.ToString(EYMC_Axis.CAMERA1_Z);
            AxisNo[15].Text = Convert.ToString(EYMC_Axis.SCANNER1_Z);
            AxisNo[16].Text = Convert.ToString(EACS_Axis.STAGE1_X);
            AxisNo[17].Text = Convert.ToString(EACS_Axis.STAGE1_Y);
            AxisNo[18].Text = Convert.ToString(EACS_Axis.STAGE1_T);
        }

        private void FormJogOperation_Load(object sender, EventArgs e)
        {
             this.Text = "Jog Operation";

            IsFastMove = false;
            SetAxis((int)EYMC_Axis.LOADER_Z);
            SetOption(EMoveOption.JOG);
            SetVelocity();

            TmrJog.Start();
        }

        private void FormJogOperation_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void SetVelocity()
        {
            int speedIndex = (int)EMotorSpeed.MANUAL_SLOW;
            if (AxisMoveOption == (int)EMoveOption.JOG)
            {
                speedIndex = IsFastMove ? (int)EMotorSpeed.JOG_FAST: (int)EMotorSpeed.JOG_SLOW;
            } else
            {
                speedIndex = IsFastMove ? (int)EMotorSpeed.MANUAL_FAST: (int)EMotorSpeed.MANUAL_SLOW;
            }

            if (SelectedAxis < (int)EYMC_Axis.MAX)
            {
                AxisSpeedData = CMainFrame.DataManager.SystemData_Axis.MPMotionData[SelectedAxis].Speed[speedIndex];
            }
            else
            {
                AxisSpeedData = CMainFrame.DataManager.SystemData_Axis.ACSMotionData[SelectedAxis].Speed[speedIndex - (int)EYMC_Axis.MAX];
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

        private void BtnAxis_Click(object sender, EventArgs e)
        {
            ButtonAdv Axis = sender as ButtonAdv;

            SetAxis(Convert.ToInt16(Axis.Tag));
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

        private void SetAxis(int nAxis)
        {
            for (int i = 0; i < AxisNo.Length; i++)
            {
                AxisNo[i].BackColor = Color.FromArgb(224, 224, 224);
            }

            AxisNo[nAxis].BackColor = Color.YellowGreen;

            SelectedAxis = nAxis;
        }

        private void SetOption(EMoveOption nOption)
        {
            BtnSelJog.BackColor = Color.FromArgb(224, 224, 224);
            BtnSelAbs.BackColor = Color.FromArgb(224, 224, 224);
            BtnSelInc.BackColor = Color.FromArgb(224, 224, 224);

            if (nOption == EMoveOption.JOG)
            {
                BtnAbsMove.Hide();
                BtnPlus.Show();
                BtnMinus.Show();

                BtnSelJog.BackColor = Color.GreenYellow;

                AxisMoveOption = (int)EMoveOption.JOG;
            } else if (nOption == EMoveOption.ABS)
            {
                BtnAbsMove.Show();
                BtnPlus.Hide();
                BtnMinus.Hide();

                BtnSelAbs.BackColor = Color.GreenYellow;

                AxisMoveOption = (int)EMoveOption.ABS;
            } else if (nOption == EMoveOption.INC)
            {
                BtnAbsMove.Hide();
                BtnPlus.Show();
                BtnMinus.Show();

                BtnSelInc.BackColor = Color.GreenYellow;

                AxisMoveOption = (int)EMoveOption.INC;
            }

            SetVelocity();
        }

        private void LabelTarget_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = LabelTarget.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            TargetPos = Convert.ToDouble(strModify);

            LabelTarget.Text = strModify;
        }

        private void BtnSelMoveOption_Click(object sender, EventArgs e)
        {
            ButtonAdv Option = sender as ButtonAdv;

            LabelTarget.Text = "0";
            if (Option.Text == "Jog Move")
            {
                LabelTitleTarget.Text = "Target Pos";
                SetOption(EMoveOption.JOG);
            } else if (Option.Text == "Abs Move")
            {
                LabelTitleTarget.Text = "Target Pos";
                SetOption(EMoveOption.ABS);
            } else if (Option.Text == "Inc Move")
            {
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
            if (AxisMoveOption == (int)EMoveOption.JOG)
            {
                if (SelectedAxis < (int)EYMC_Axis.MAX)
                {
                    iResult = CMainFrame.LWDicer.m_YMC.StartJogMove(SelectedAxis, bDirection, IsFastMove);
                }
                else
                {
                    iResult = CMainFrame.LWDicer.m_ACS.StartJogMove(SelectedAxis - (int)EYMC_Axis.MAX, bDirection, IsFastMove);
                }
            }
            else if (AxisMoveOption == (int)EMoveOption.INC)
            {
                double[] dTargetPos = new double[1];
                if(bDirection == DIR_POSITIVE)
                    dTargetPos[0] = Convert.ToDouble(LabelCurrent.Text) + Convert.ToDouble(LabelTarget.Text);
                else dTargetPos[0] = Convert.ToDouble(LabelCurrent.Text) - Convert.ToDouble(LabelTarget.Text);

                if (SelectedAxis < (int)EYMC_Axis.MAX)
                {
                    CMotorSpeedData[] tSpeed = new CMotorSpeedData[1];
                    tSpeed[0] = AxisSpeedData;
                    iResult = CMainFrame.LWDicer.m_YMC.MoveToPos(SelectedAxis, dTargetPos, tSpeed, (int)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED);
                }
                else
                {
                    iResult = CMainFrame.LWDicer.m_ACS.MoveToPos(SelectedAxis - (int)EYMC_Axis.MAX, dTargetPos[0], AxisSpeedData);
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

            if (AxisMoveOption == (int)EMoveOption.JOG)
            {
                if (SelectedAxis < (int)EYMC_Axis.MAX)
                {
                    iResult = CMainFrame.LWDicer.m_YMC.StopJogMove(SelectedAxis);
                }
                else
                {
                    CMainFrame.LWDicer.m_ACS.StopJogMove(SelectedAxis - (int)EYMC_Axis.MAX);
                }
            } else
            {
                if (SelectedAxis < (int)EYMC_Axis.MAX)
                {
                    iResult = CMainFrame.LWDicer.m_YMC.StopServoMotion(SelectedAxis);
                }
                else
                {
                    CMainFrame.LWDicer.m_ACS.StopServoMotion(SelectedAxis - (int)EYMC_Axis.MAX);
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
            StopAxis();
        }

        private void BtnAbsMove_MouseClick(object sender, MouseEventArgs e)
        {
            int iResult = SUCCESS;

            double[] dTargetPos = new double[1];
            dTargetPos[0] = Convert.ToDouble(LabelTarget.Text);
            if (SelectedAxis < (int)EYMC_Axis.MAX)
            {
                CMotorSpeedData[] tSpeed = new CMotorSpeedData[1];
                tSpeed[0] = AxisSpeedData;
                iResult = CMainFrame.LWDicer.m_YMC.MoveToPos(SelectedAxis, dTargetPos, tSpeed, (int)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED);
            }
            else
            {
                iResult = CMainFrame.LWDicer.m_ACS.MoveToPos(SelectedAxis - (int)EYMC_Axis.MAX, dTargetPos[0], AxisSpeedData);
            }
            if (iResult != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult);
            }
        }

        private void TmrJog_Tick(object sender, EventArgs e)
        {
            string strCurPos = string.Empty;

            // Jog Operation Servo Encoder Position
            if (SelectedAxis < (int)EYMC_Axis.MAX)
            {
                LabelCurrent.Text = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[SelectedAxis].EncoderPos);
            } else
            {
                LabelCurrent.Text = Convert.ToString(CMainFrame.LWDicer.m_ACS.ServoStatus[SelectedAxis - (int)EYMC_Axis.MAX].EncoderPos);
            }
        }
    }
}
