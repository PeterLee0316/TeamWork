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

using static LWDicer.Control.DEF_System;

using static LWDicer.Control.MYaskawa;
using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_ACS;

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

        private int SelOption = 0;

        private double TargetPos = 0;

        private int SelAxis = 0;

        private bool bAxisVelocity = false;

        ButtonAdv[] AxisNo = new ButtonAdv[19];
        ButtonAdv[] Velocity = new ButtonAdv[2];

        // Jog
        public const bool JOG_DIR_POS = true;
        public const bool JOG_DIR_NEG = false;


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

            SetAxis((int)EYMC_Axis.LOADER_Z);
            SetOption(EMoveOption.JOG);
            SetVelocity((int)EYMC_Axis.LOADER_Z, true);

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

        private bool GetVelocity()
        {
            return bAxisVelocity;
        }

        private void SetVelocity(int nAxis, bool bVelocity)
        {
            if (bVelocity == true)
            {
                // Slow Velocity
                Velocity[0].Image = Image.Images[0];
                Velocity[1].Image = Image.Images[1];

                if (nAxis > 15)
                {
                    // ACS
                    LabelVelocity.Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.ACSMotionData[nAxis - 16].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel);
                }
                else
                {
                    // MP
                    LabelVelocity.Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[nAxis].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel);
                }

                bAxisVelocity = true;
            }
            else
            {
                // Fast Velocity
                Velocity[0].Image = Image.Images[1];
                Velocity[1].Image = Image.Images[0];

                if (nAxis > 15)
                {
                    // ACS
                    LabelVelocity.Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.ACSMotionData[nAxis - 16].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel);
                }
                else
                {
                    // MP
                    LabelVelocity.Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[nAxis].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel);
                }

                bAxisVelocity = false;
            }
        }

        private void BtnAxis_Click(object sender, EventArgs e)
        {
            ButtonAdv Axis = sender as ButtonAdv;

            SetAxis(Convert.ToInt16(Axis.Tag));

            SetVelocity(GetAxis(), true);
        }

        private void BtnVel_Click(object sender, EventArgs e)
        {
            string strText = string.Empty;

            ButtonAdv Vel = sender as ButtonAdv;

            strText = Vel.Text;

            strText = strText.Trim();

            if (strText == "Slow")
            {
                SetVelocity(GetAxis(), true);
            }

            if (strText == "Fast")
            {
                SetVelocity(GetAxis(), false);
            }
        }

        private void SetAxis(int nAxis)
        {
            int i = 0;

            for (i = 0; i < 19; i++)
            {
                AxisNo[i].BackColor = Color.FromArgb(224, 224, 224);
            }

            AxisNo[nAxis].BackColor = Color.YellowGreen;

            SelAxis = nAxis;
        }

        private int GetAxis()
        {
            return SelAxis;
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

                SelOption = (int)EMoveOption.JOG;
            }

            if (nOption == EMoveOption.ABS)
            {
                BtnAbsMove.Show();
                BtnPlus.Hide();
                BtnMinus.Hide();

                BtnSelAbs.BackColor = Color.GreenYellow;

                SelOption = (int)EMoveOption.ABS;
            }

            if (nOption == EMoveOption.INC)
            {
                BtnAbsMove.Hide();
                BtnPlus.Show();
                BtnMinus.Show();

                BtnSelInc.BackColor = Color.GreenYellow;

                SelOption = (int)EMoveOption.INC;
            }
        }

        private void LabelTarget_Click(object sender, EventArgs e)
        {
            string StrCurrent = "", strModify = "";

            StrCurrent = LabelTarget.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            TargetPos = Convert.ToDouble(strModify);

            LabelTarget.Text = strModify;
        }

        private void BtnSelMoveOption_Click(object sender, EventArgs e)
        {
            ButtonAdv Option = sender as ButtonAdv;

            if (Option.Text == "Jog Move")
            {
                SetOption(EMoveOption.JOG);
            }

            if (Option.Text == "Abs Move")
            {
                SetOption(EMoveOption.ABS);
            }

            if (Option.Text == "Inc Move")
            {
                SetOption(EMoveOption.INC);
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            int nAcsAxis = 0;

            if (GetAxis() < 16)
            {
                CMainFrame.LWDicer.m_YMC.ServoMotionStop(GetAxis(), (int)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED);
            }
            else
            {
                nAcsAxis = GetAxis() - 16;
                CMainFrame.LWDicer.m_ACS.ServoMotionStop(nAcsAxis);
            }
        }

        private void BtnPlus_MouseDown(object sender, MouseEventArgs e)
        {
            int nAcsAxis = 0;

            if (SelOption == (int)EMoveOption.JOG)
            {
                BtnPlus.BackColor = Color.LightGoldenrodYellow;

                if (GetAxis() < 16)
                {
                    // MP
                    CMainFrame.LWDicer.m_YMC.JogMoveStart(GetAxis(), JOG_DIR_POS, GetVelocity());
                }
                else
                {
                    // ACS
                    nAcsAxis = GetAxis() - 16;
                    CMainFrame.LWDicer.m_ACS.JogMoveStart(nAcsAxis, JOG_DIR_POS, GetVelocity());
                }
            }

            if (SelOption == (int)EMoveOption.INC)
            {
                double[] dMPIncPos = new double[1];
                double dACSIncPos = 0;

                BtnPlus.BackColor = Color.DarkGoldenrod;

                if (GetAxis() < 16)
                {
                    // MP
                    LWDicer.Control.DEF_Yaskawa.CMotorSpeedData[] AxisSpeed = new Control.DEF_Yaskawa.CMotorSpeedData[1];

                    if (GetVelocity() == true)
                    {
                        // Slow
                        AxisSpeed[0] = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[GetAxis()].Speed[(int)EMotorSpeed.JOG_SLOW];
                    }
                    else
                    {
                        // Fast
                        AxisSpeed[0] = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[GetAxis()].Speed[(int)EMotorSpeed.JOG_FAST];
                    }

                    dMPIncPos[0] = CMainFrame.LWDicer.m_YMC.ServoStatus[GetAxis()].EncoderPos + TargetPos;

                    CMainFrame.LWDicer.m_YMC.MoveToPos(GetAxis(), dMPIncPos, AxisSpeed, (int)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED);
                }
                else
                {
                    // ACS
                    nAcsAxis = GetAxis() - 16;

                    LWDicer.Control.DEF_ACS.CMotorSpeedData AxisSpeed = new Control.DEF_ACS.CMotorSpeedData();

                    if (GetVelocity() == true)
                    {
                        // Slow
                        AxisSpeed = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.ACSMotionData[nAcsAxis].Speed[(int)EMotorSpeed.JOG_SLOW];
                    }
                    else
                    {
                        // Fast
                        AxisSpeed = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.ACSMotionData[nAcsAxis].Speed[(int)EMotorSpeed.JOG_FAST];
                    }

                    dACSIncPos = CMainFrame.LWDicer.m_ACS.ServoStatus[nAcsAxis].EncoderPos + TargetPos;

                    CMainFrame.LWDicer.m_ACS.MoveToPos(nAcsAxis, dACSIncPos, AxisSpeed);
                }
            }
        }

        private void BtnPlus_MouseUp(object sender, MouseEventArgs e)
        {
            int nAcsAxis = 0;

            if (SelOption == (int)EMoveOption.JOG)
            {
                BtnPlus.BackColor = Color.DarkGoldenrod;

                if (GetAxis() < 16)
                {
                    // MP
                    CMainFrame.LWDicer.m_YMC.JogMoveStop(GetAxis());
                }
                else
                {
                    // ACS
                    nAcsAxis = GetAxis() - 16;
                    CMainFrame.LWDicer.m_ACS.JogMoveStop(nAcsAxis);
                }
            }
        }

        private void BtnMinus_MouseDown(object sender, MouseEventArgs e)
        {
            int nAcsAxis = 0;

            if (SelOption == (int)EMoveOption.JOG)
            {
                BtnMinus.BackColor = Color.LightGoldenrodYellow;

                if (GetAxis() < 16)
                {
                    // MP
                    CMainFrame.LWDicer.m_YMC.JogMoveStart(GetAxis(), JOG_DIR_NEG, GetVelocity());
                }
                else
                {
                    // ACS
                    nAcsAxis = GetAxis() - 16;
                    CMainFrame.LWDicer.m_ACS.JogMoveStart(nAcsAxis, JOG_DIR_NEG, GetVelocity());
                }
            }

            if (SelOption == (int)EMoveOption.INC)
            {
                double[] dMPIncPos = new double[1];
                double dACSIncPos = 0;

                BtnMinus.BackColor = Color.DarkGoldenrod;

                if (GetAxis() < 16)
                {
                    // MP
                    LWDicer.Control.DEF_Yaskawa.CMotorSpeedData[] AxisSpeed = new Control.DEF_Yaskawa.CMotorSpeedData[1];

                    if (GetVelocity() == true)
                    {
                        // Slow
                        AxisSpeed[0] = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[GetAxis()].Speed[(int)EMotorSpeed.JOG_SLOW];
                    }
                    else
                    {
                        // Fast
                        AxisSpeed[0] = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[GetAxis()].Speed[(int)EMotorSpeed.JOG_FAST];
                    }

                    dMPIncPos[0] = CMainFrame.LWDicer.m_YMC.ServoStatus[GetAxis()].EncoderPos - TargetPos;

                    CMainFrame.LWDicer.m_YMC.MoveToPos(GetAxis(), dMPIncPos, AxisSpeed, (int)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED);

                }
                else
                {
                    // ACS
                    nAcsAxis = GetAxis() - 16;

                    LWDicer.Control.DEF_ACS.CMotorSpeedData AxisSpeed = new Control.DEF_ACS.CMotorSpeedData();

                    if (GetVelocity() == true)
                    {
                        // Slow
                        AxisSpeed = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.ACSMotionData[nAcsAxis].Speed[(int)EMotorSpeed.JOG_SLOW];
                    }
                    else
                    {
                        // Fast
                        AxisSpeed = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.ACSMotionData[nAcsAxis].Speed[(int)EMotorSpeed.JOG_FAST];
                    }

                    dACSIncPos = CMainFrame.LWDicer.m_ACS.ServoStatus[nAcsAxis].EncoderPos - TargetPos;

                    CMainFrame.LWDicer.m_ACS.MoveToPos(nAcsAxis, dACSIncPos, AxisSpeed);
                }
            }
        }

        private void BtnMinus_MouseUp(object sender, MouseEventArgs e)
        {
            int nAcsAxis = 0;

            if (SelOption == (int)EMoveOption.JOG)
            {
                BtnMinus.BackColor = Color.DarkGoldenrod;

                if (GetAxis() < 16)
                {
                    // MP
                    CMainFrame.LWDicer.m_YMC.JogMoveStop(GetAxis());
                }
                else
                {
                    // ACS
                    nAcsAxis = GetAxis() - 16;
                    CMainFrame.LWDicer.m_ACS.JogMoveStop(nAcsAxis);
                }
            }
        }

        private void BtnAbsMove_MouseClick(object sender, MouseEventArgs e)
        { 
            int nAcsAxis = 0;
            double[] dMPAbsPos = new double[1];
            double dACSAbsPos = 0;

            BtnMinus.BackColor = Color.DarkGoldenrod;

            if (GetAxis() < 16)
            {
                LWDicer.Control.DEF_Yaskawa.CMotorSpeedData[] AxisSpeed = new Control.DEF_Yaskawa.CMotorSpeedData[1];

                // MP
                if (GetVelocity() == true)
                {
                    // Slow
                    AxisSpeed[0] = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[GetAxis()].Speed[(int)EMotorSpeed.JOG_SLOW];
                }
                else
                {
                    // Fast
                    AxisSpeed[0] = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[GetAxis()].Speed[(int)EMotorSpeed.JOG_FAST];
                }

                dMPAbsPos[0] = TargetPos;

                CMainFrame.LWDicer.m_YMC.MoveToPos(GetAxis(), dMPAbsPos, AxisSpeed, (int)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED);
            }
            else
            {
                // ACS
                nAcsAxis = GetAxis() - 16;

                LWDicer.Control.DEF_ACS.CMotorSpeedData AxisSpeed = new Control.DEF_ACS.CMotorSpeedData();

                if (GetVelocity() == true)
                {
                    // Slow
                    AxisSpeed = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.ACSMotionData[nAcsAxis].Speed[(int)EMotorSpeed.JOG_SLOW];
                }
                else
                {
                    // Fast
                    AxisSpeed = CMainFrame.LWDicer.m_DataManager.SystemData_Axis.ACSMotionData[nAcsAxis].Speed[(int)EMotorSpeed.JOG_FAST];
                }

                dACSAbsPos = TargetPos;

                CMainFrame.LWDicer.m_ACS.MoveToPos(nAcsAxis, dACSAbsPos, AxisSpeed);
            }
        }

        private void TmrJog_Tick(object sender, EventArgs e)
        {
            string strCurPos = string.Empty;

            // Jog Operation Servo Encoder Position
            if (AxisNo[GetAxis()].Text == "STAGE X" || AxisNo[GetAxis()].Text == "STAGE Y" || AxisNo[GetAxis()].Text == "STAGE T")
            {
                if (AxisNo[GetAxis()].Text == "STAGE X")
                {
                    LabelCurrent.Text = Convert.ToString(CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_X].EncoderPos);
                }

                if (AxisNo[GetAxis()].Text == "STAGE Y")
                {
                    LabelCurrent.Text = Convert.ToString(CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_Y].EncoderPos);
                }

                if (AxisNo[GetAxis()].Text == "STAGE T")
                {
                    LabelCurrent.Text = Convert.ToString(CMainFrame.LWDicer.m_ACS.ServoStatus[(int)EACS_Axis.STAGE1_T].EncoderPos);
                }
            }
            else
            {
                LabelCurrent.Text = Convert.ToString(CMainFrame.LWDicer.m_YMC.ServoStatus[GetAxis()].EncoderPos);
            }
        }
    }
}
