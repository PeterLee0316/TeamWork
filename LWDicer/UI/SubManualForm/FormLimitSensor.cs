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
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Drawing;

using LWDicer.Layers;
using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;

using static LWDicer.Layers.DEF_Motion;
using static LWDicer.Layers.DEF_Yaskawa;
using static LWDicer.Layers.DEF_ACS;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.UI
{
    public partial class FormLimitSensor : Form
    {
        private const int MaxRowSize = 19;
        private GradientLabel[] MotorNo     = new GradientLabel[MaxRowSize];
        private GradientLabel[] MotorPos    = new GradientLabel[MaxRowSize];
        private GradientLabel[] MotorServo  = new GradientLabel[MaxRowSize];
        private GradientLabel[] MotorNLimit = new GradientLabel[MaxRowSize];
        private GradientLabel[] MotorHome   = new GradientLabel[MaxRowSize];
        private GradientLabel[] MotorPLimit = new GradientLabel[MaxRowSize];
        private GradientLabel[] MotorAlarm  = new GradientLabel[MaxRowSize];

        private Syncfusion.Drawing.BrushInfo Brush_Default = new Syncfusion.Drawing.BrushInfo(Color.LightGray);
        private Syncfusion.Drawing.BrushInfo Brush_ServoOn = new Syncfusion.Drawing.BrushInfo(Color.Yellow);
        private Syncfusion.Drawing.BrushInfo Brush_LimitDetected = new Syncfusion.Drawing.BrushInfo(Color.Red);

        public FormLimitSensor()
        {
            InitializeComponent();

            ResouceMapping();
        }

        private void ResouceMapping()
        {
            MotorNo[0]      = Motor1; MotorNo[1] = Motor2; MotorNo[2] = Motor3; MotorNo[3] = Motor4; MotorNo[4] = Motor5;
            MotorNo[5]      = Motor6; MotorNo[6] = Motor7; MotorNo[7] = Motor8; MotorNo[8] = Motor9; MotorNo[9] = Motor10;
            MotorNo[10]     = Motor11; MotorNo[11] = Motor12; MotorNo[12] = Motor13; MotorNo[13] = Motor14; MotorNo[14] = Motor15;
            MotorNo[15]     = Motor16; MotorNo[16] = Motor17; MotorNo[17] = Motor18; MotorNo[18] = Motor19;

            MotorPos[0]     = Motor1_Pos; MotorPos[1] = Motor2_Pos; MotorPos[2] = Motor3_Pos; MotorPos[3] = Motor4_Pos; MotorPos[4] = Motor5_Pos;
            MotorPos[5]     = Motor6_Pos; MotorPos[6] = Motor7_Pos; MotorPos[7] = Motor8_Pos; MotorPos[8] = Motor9_Pos; MotorPos[9] = Motor10_Pos;
            MotorPos[10]    = Motor11_Pos; MotorPos[11] = Motor12_Pos; MotorPos[12] = Motor13_Pos; MotorPos[13] = Motor14_Pos; MotorPos[14] = Motor15_Pos;
            MotorPos[15]    = Motor16_Pos; MotorPos[16] = Motor17_Pos; MotorPos[17] = Motor18_Pos; MotorPos[18] = Motor19_Pos;

            MotorServo[0]   = Motor1_Servo; MotorServo[1] = Motor2_Servo; MotorServo[2] = Motor3_Servo; MotorServo[3] = Motor4_Servo; MotorServo[4] = Motor5_Servo;
            MotorServo[5]   = Motor6_Servo; MotorServo[6] = Motor7_Servo; MotorServo[7] = Motor8_Servo; MotorServo[8] = Motor9_Servo; MotorServo[9] = Motor10_Servo;
            MotorServo[10]  = Motor11_Servo; MotorServo[11] = Motor12_Servo; MotorServo[12] = Motor13_Servo; MotorServo[13] = Motor14_Servo; MotorServo[14] = Motor15_Servo;
            MotorServo[15]  = Motor16_Servo; MotorServo[16] = Motor17_Servo; MotorServo[17] = Motor18_Servo; MotorServo[18] = Motor19_Servo;

            MotorNLimit[0]  = Motor1_N_Limit; MotorNLimit[1] = Motor2_N_Limit; MotorNLimit[2] = Motor3_N_Limit; MotorNLimit[3] = Motor4_N_Limit; MotorNLimit[4] = Motor5_N_Limit;
            MotorNLimit[5]  = Motor6_N_Limit; MotorNLimit[6] = Motor7_N_Limit; MotorNLimit[7] = Motor8_N_Limit; MotorNLimit[8] = Motor9_N_Limit; MotorNLimit[9] = Motor10_N_Limit;
            MotorNLimit[10] = Motor11_N_Limit; MotorNLimit[11] = Motor12_N_Limit; MotorNLimit[12] = Motor13_N_Limit; MotorNLimit[13] = Motor14_N_Limit; MotorNLimit[14] = Motor15_N_Limit;
            MotorNLimit[15] = Motor16_N_Limit; MotorNLimit[16] = Motor17_N_Limit; MotorNLimit[17] = Motor18_N_Limit; MotorNLimit[18] = Motor19_N_Limit;

            MotorHome[0]    = Motor1_Home; MotorHome[1] = Motor2_Home; MotorHome[2] = Motor3_Home; MotorHome[3] = Motor4_Home; MotorHome[4] = Motor5_Home;
            MotorHome[5]    = Motor6_Home; MotorHome[6] = Motor7_Home; MotorHome[7] = Motor8_Home; MotorHome[8] = Motor9_Home; MotorHome[9] = Motor10_Home;
            MotorHome[10]   = Motor11_Home; MotorHome[11] = Motor12_Home; MotorHome[12] = Motor13_Home; MotorHome[13] = Motor14_Home; MotorHome[14] = Motor15_Home;
            MotorHome[15]   = Motor16_Home; MotorHome[16] = Motor17_Home; MotorHome[17] = Motor18_Home; MotorHome[18] = Motor19_Home;

            MotorPLimit[0]  = Motor1_P_Limit; MotorPLimit[1] = Motor2_P_Limit; MotorPLimit[2] = Motor3_P_Limit; MotorPLimit[3] = Motor4_P_Limit; MotorPLimit[4] = Motor5_P_Limit;
            MotorPLimit[5]  = Motor6_P_Limit; MotorPLimit[6] = Motor7_P_Limit; MotorPLimit[7] = Motor8_P_Limit; MotorPLimit[8] = Motor9_P_Limit; MotorPLimit[9] = Motor10_P_Limit;
            MotorPLimit[10] = Motor11_P_Limit; MotorPLimit[11] = Motor12_P_Limit; MotorPLimit[12] = Motor13_P_Limit; MotorPLimit[13] = Motor14_P_Limit; MotorPLimit[14] = Motor15_P_Limit;
            MotorPLimit[15] = Motor16_P_Limit; MotorPLimit[16] = Motor17_P_Limit; MotorPLimit[17] = Motor18_P_Limit; MotorPLimit[18] = Motor19_P_Limit;

            MotorAlarm[0]   = Motor1_Alarm; MotorAlarm[1] = Motor2_Alarm; MotorAlarm[2] = Motor3_Alarm; MotorAlarm[3] = Motor4_Alarm; MotorAlarm[4] = Motor5_Alarm;
            MotorAlarm[5]   = Motor6_Alarm; MotorAlarm[6] = Motor7_Alarm; MotorAlarm[7] = Motor8_Alarm; MotorAlarm[8] = Motor9_Alarm; MotorAlarm[9] = Motor10_Alarm;
            MotorAlarm[10]  = Motor11_Alarm; MotorAlarm[11] = Motor12_Alarm; MotorAlarm[12] = Motor13_Alarm; MotorAlarm[13] = Motor14_Alarm; MotorAlarm[14] = Motor15_Alarm;
            MotorAlarm[15]  = Motor16_Alarm; MotorAlarm[16] = Motor17_Alarm; MotorAlarm[17] = Motor18_Alarm; MotorAlarm[18] = Motor19_Alarm;

            for (int i = 0; i < MaxRowSize; i++)
            {
                if (i < (int)EYMC_Axis.MAX)
                {
                    MotorNo[i].Text = Convert.ToString(EYMC_Axis.LOADER_Z + i);
                } else
                {
                    MotorNo[i].Text = Convert.ToString(EACS_Axis.STAGE1_X + i - (int)EYMC_Axis.MAX);
                }
            }
        }

        private void FormLimitSensor_Load(object sender, EventArgs e)
        {
            TmrMotor.Enabled = true;
            TmrMotor.Interval = UITimerInterval;
            TmrMotor.Start();
        }

        private void FormClose()
        {
            TmrMotor.Enabled = true;
            TmrMotor.Stop();

            this.Hide();
        }

        private void FormLimitSensor_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void TmrMotor_Tick(object sender, EventArgs e)
        {
            double dPos = 0;
            string on = "On";
            string off = "Off";

            CMPServoStatus mpStatus;
            for(int i = 0; i < (int)EYMC_Axis.MAX; i++)
            {
                mpStatus = CMainFrame.LWDicer.m_YMC.ServoStatus[i];

                // 1.Encoder Position
                MotorPos[i].Text = String.Format("{0:0.000}", mpStatus.EncoderPos);

                // 2.Servo On, Off
                MotorServo[i].Text = mpStatus.IsServoOn ? on : off;
                if (mpStatus.IsServoOn) MotorServo[i].BackgroundColor = Brush_ServoOn;
                else MotorServo[i].BackgroundColor = Brush_Default;

                // 3.- Limit Sensor
                MotorNLimit[i].Text = mpStatus.DetectMinusSensor ? on : off;
                if (mpStatus.DetectMinusSensor) MotorNLimit[i].BackgroundColor = Brush_LimitDetected;
                else MotorNLimit[i].BackgroundColor = Brush_Default;

                // 4.Home Sensor
                MotorHome[i].Text = mpStatus.DetectHomeSensor ? on : off;
                if (mpStatus.DetectHomeSensor) MotorHome[i].BackgroundColor = Brush_ServoOn;
                else MotorHome[i].BackgroundColor = Brush_Default;

                // 5.+Limit Sensor
                MotorPLimit[i].Text = mpStatus.DetectPlusSensor ? on : off;
                if (mpStatus.DetectPlusSensor) MotorPLimit[i].BackgroundColor = Brush_LimitDetected;
                else MotorPLimit[i].BackgroundColor = Brush_Default;

                // 6.Driver Alarm
                //MotorAlarm[i].Text = "ER0001";
                MotorAlarm[i].Text = mpStatus.AlarmCode.ToString();
                if (MotorAlarm[i].Text != "0") MotorAlarm[i].BackgroundColor = Brush_LimitDetected;
                else MotorAlarm[i].BackgroundColor = Brush_Default;
            }

            CACSServoStatus acsStatus;
            for (int i = (int)EYMC_Axis.MAX, j = 0; i < MaxRowSize; i++, j++)
            {
                acsStatus = CMainFrame.LWDicer.m_ACS.ServoStatus[j];

                // 1.Encoder Position
                MotorPos[i].Text = string.Format("{0:f4}", acsStatus.EncoderPos);

                // 2.Servo On, Off
                MotorServo[i].Text = acsStatus.IsServoOn ? on : off;
                if (acsStatus.IsServoOn) MotorServo[i].BackgroundColor = Brush_ServoOn;
                else MotorServo[i].BackgroundColor = Brush_Default;

                // 3.- Limit Sensor
                MotorNLimit[i].Text = acsStatus.DetectMinusSensor ? on : off;
                if (acsStatus.DetectMinusSensor) MotorNLimit[i].BackgroundColor = Brush_LimitDetected;
                else MotorNLimit[i].BackgroundColor = Brush_Default;

                // 4.Home Sensor
                MotorHome[i].Text = acsStatus.DetectHomeSensor ? on : off;
                if (acsStatus.DetectHomeSensor) MotorHome[i].BackgroundColor = Brush_ServoOn;
                else MotorHome[i].BackgroundColor = Brush_Default;

                // 5.+Limit Sensor
                MotorPLimit[i].Text = acsStatus.DetectPlusSensor ? on : off;
                if (acsStatus.DetectPlusSensor) MotorPLimit[i].BackgroundColor = Brush_LimitDetected;
                else MotorPLimit[i].BackgroundColor = Brush_Default;

                // 6.Driver Alarm
                //MotorAlarm[i].Text = "ER0001";
                //MotorAlarm[i].Text = acsStatus.AlarmCode.ToString();
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }
    }
}
