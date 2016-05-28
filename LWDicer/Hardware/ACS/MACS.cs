﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SPIIPLUSCOM660Lib;	// ACS COM-Library
using System.Runtime.InteropServices;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_ACS;
using static LWDicer.Control.DEF_IO;
using static LWDicer.Control.DEF_Motion;

namespace LWDicer.Control
{
    public class DEF_ACS
    {
        public const int ERR_ACS_INVALID_CONTROLLER = 1;
        public const int ERR_ACS_FAIL_OPEN_YMC = 2;
        public const int ERR_ACS_FAIL_SET_TIMEOUT = 3;
        public const int ERR_ACS_FAIL_CHANGE_CONTROLLER = 4;
        public const int ERR_ACS_FAIL_DECLARE_AXIS = 5;
        public const int ERR_ACS_FAIL_CLEAR_AXIS = 6;
        public const int ERR_ACS_FAIL_DECLARE_DEVICE = 7;
        public const int ERR_ACS_FAIL_CLEAR_DEVICE = 8;
        public const int ERR_ACS_FAIL_SERVO_ON = 9;
        public const int ERR_ACS_FAIL_SERVO_OFF = 10;
        public const int ERR_ACS_FAIL_RESET_ALARM = 11;
        public const int ERR_ACS_FAIL_GET_MOTION_PARAM = 12;
        public const int ERR_ACS_FAIL_SERVO_STOP = 13;
        public const int ERR_ACS_FAIL_SERVO_MOVE_JOG = 14;
        public const int ERR_ACS_FAIL_SERVO_MOVE_DRIVING_POSITIONING = 15;
        public const int ERR_ACS_FAIL_SERVO_MOVE_HOME = 16;
        public const int ERR_ACS_FAIL_SERVO_GET_POS = 17;
        public const int ERR_ACS_FAIL_GET_REGISTER_DATA_HANDLE = 18;
        public const int ERR_ACS_FAIL_GET_REGISTER_DATA = 19;
        public const int ERR_ACS_FAIL_SERVO_MOVE_IN_LIMIT_TIME = 20;
        public const int ERR_ACS_FAIL_SERVO_HOME_IN_LIMIT_TIME = 21;
        public const int ERR_ACS_TARGET_POS_EXCEED_PLUS_LIMIT = 22;
        public const int ERR_ACS_TARGET_POS_EXCEED_MINUS_LIMIT = 23;
        public const int ERR_ACS_DETECTED_DOOR_OPEN = 24;
        public const int ERR_ACS_DETECTED_ESTOP = 25;
        public const int ERR_ACS_DETECTED_PLUS_LIMIT = 26;
        public const int ERR_ACS_DETECTED_MINUS_LIMIT = 27;
        public const int ERR_ACS_DETECTED_SERVO_ALARM = 28;
        public const int ERR_ACS_NOT_ORIGIN_RETURNED = 29;
        public const int ERR_ACS_NOT_SERVO_ON = 30;
        public const int ERR_ACS_SELECTED_AXIS_NONE = 31;
        public const int ERR_ACS_OBSOLETE_FUNCTION = 32;

        public const int MAX_AXIS_COUNT = 32;
        public const int MAX_BUFFER_CNT = 64;

        public const int USE_AXIS_COUNT = 4;

        public enum EStatusInt
        {
            MOTOR_STATUS=0,
            MOTOR_FAULT,
            HOME_FLAG,
            INT_AXIS_STATUS,
        }

        public enum EStatusDouble
        {
            ACT_POSITION=0,            
            ACT_VELOCITY,
            ACT_TORQUE,
            ACT_POS_ERR,
            CMD_POSITION,
            CMD_VELOCITY,
            CMD_ACCELERATION,
            CMD_DECELERATION,
            REAL_AXIS_STATUS

        }

        public class CServoStatus
        {
            public double EncoderPos;            
            public double Velocity;     //Servo 현재 속도
            public double LoadFactor;
            public double PositionErr;
            public double CommandPos;
            public double CommandVelocity;
            public double CommandAcceleration;
            public double CommandDeceleration;

            public bool IsServoOn;
            public bool IsInPosition;
            public bool IsBusy;
            public bool IsAccelerating;

            public bool IsDriverFault;
            public bool IsMotorOverHeat;
           // public bool Is;
            public bool IsMinusSensor;
            public bool IsPlusSensor;
            public bool IsHomeSensor;
            
            
            public bool IsHomeComplete;       // origin return flag
        }

        public class CMotorSpeedData
        {
            public double Vel;  // Feeding speed [reference unit/s], Offset speed
            public double Acc;  // Acceleration [reference unit/s2], acceleration time constant [ms]
            public double Dec;  // Deceleration [reference unit/s2], deceleration time constant [ms]

            public CMotorSpeedData(double Vel = 0, double Acc = 0, double Dec = 0)
            {
                this.Vel = Vel;
                this.Acc = Acc;
                this.Dec = Dec;
            }
        }

        public enum EMotorSpeedACS // Motor speed type
        {
            MANUAL_SLOW,
            MANUAL_FAST,
            AUTO_SLOW,
            AUTO_FAST,
            JOG_SLOW,
            JOG_FAST,
            MAX,
        }

        public class CMotorTimeLimitData
        {
            public double tMoveLimit;        // Time Limit for Moving           
            public double tSleepAfterMove;   // Sleep Time after Moving
            public double tOriginLimit;      // Time Limit for Origin Return

            public CMotorTimeLimitData(double tMoveLimit = 0, double tSleepAfterMove = 0, double tOriginLimit = 0)
            {
                this.tMoveLimit = tMoveLimit;
                this.tSleepAfterMove = tSleepAfterMove;
                this.tOriginLimit = tOriginLimit;
            }
        }

        public class CMotorSWLimit
        {
            // PosLimit
            public double Plus;              // software + Limit
            public double Minus;              // software - Limit

            public CMotorSWLimit(double Plus = 100, double Minus = -100)
            {
                this.Plus = Plus;
                this.Minus = Minus;
            }
        }

        public class CACSMotionData
        {
            // General
            public string Name; // Name of Axis
            public bool Exist;    // Use of Axis. if false, Axis not exist.
            public double Tolerance;            // Position Tolerance

            // Software Limit
            public CMotorSWLimit PosLimit;

            // Speed
            public double MaxVelocity;          // Maximum feeding speed [reference unit/s]
            public CMotorSpeedData[] Speed = new CMotorSpeedData[(int)EMotorSpeedACS.MAX];

            // Time Limit
            public CMotorTimeLimitData TimeLimit;        
            
            public CACSMotionData()
            {
                // General
                Name = "NotExist";
                Exist = false;
                Tolerance = 0.001;

                // Software Limit
                PosLimit = new CMotorSWLimit();

                // Speed
                MaxVelocity = 1000;
                for (int i = 0; i < Speed.Length; i++)
                {
                    Speed[i] = new CMotorSpeedData(10, 100, 100);
                }

                // Time Limit
                TimeLimit = new CMotorTimeLimitData(10, 0.01, 30);
                                         // All parameters directly specified
            }

            public void GetSpeedData(out CMotorSpeedData data, int speedType = (int)EMotorSpeedACS.MANUAL_SLOW)
            {
                data = ObjectExtensions.Copy(Speed[speedType]);
            }

            public void SetSpeedData(CMotorSpeedData data, int speedType = (int)EMotorSpeedACS.MANUAL_SLOW)
            {
                Speed[speedType] = ObjectExtensions.Copy(data);
            }

            public void GetTimeLimitData(out CMotorTimeLimitData data)
            {
                data = ObjectExtensions.Copy(TimeLimit);
            }

            public void CheckSWLimit(double targetPos, out bool bExceedPlusLimit, out bool bExceedMinusLimit)
            {
                if (targetPos >= PosLimit.Plus) bExceedPlusLimit = true;
                else bExceedPlusLimit = false;

                if (targetPos <= PosLimit.Minus) bExceedMinusLimit = true;
                else bExceedMinusLimit = false;
            }

        }

        public static class CStatusArray
        {
            public static int[,] IntStatus = new int[USE_AXIS_COUNT, (int)EStatusInt.INT_AXIS_STATUS];
            public static double[,] DoubleStatus = new double[USE_AXIS_COUNT, (int)EStatusDouble.REAL_AXIS_STATUS];            
        }

        public class CACSChannel
        {
            public Channel ACS;
            private string addressTCP;
            private int portNum;
            public bool IsChannelOpen { get; private set; }


           public CACSMotionData[] MotionData = new CACSMotionData[USE_AXIS_COUNT];
            //CServoStatusArray ServoStatus;

            public CACSChannel(CACSMotionData[] motions = null)
            {
                Channel ACS = new Channel();
                string addressTCP = "10.0.0.100";
                int portNum = 701;

                IsChannelOpen = false;
                if (motions == null)
                {
                    for (int i = 0; i < USE_AXIS_COUNT; i++)
                    {
                        MotionData[i] = new CACSMotionData();
                    }
                }
                else
                {
                    for (int i = 0; i < motions?.Length; i++)
                    {
                        MotionData[i] = ObjectExtensions.Copy(motions[i]);
                    }
                }

            }

            public void SetAddress(string strTCP)
            {
                addressTCP = strTCP;
            }
            public void SetPortNum(int PortNum)
            {
                portNum = PortNum;
            }

            public int ChannelOpen()
            {
                ACS.OpenCommEthernetTCP(addressTCP, portNum);
                
                IsChannelOpen = true;

                return SUCCESS;
            }
            public int ChannelClose()
            {

                ACS.CloseComm();

                IsChannelOpen = false;

                return SUCCESS;
            }

            public void GetACSBuffer()
            {
                object d_Matrix;
                d_Matrix = ACS.ReadVariableAsMatrix("M_REAL", ACS.ACSC_NONE, 0, USE_AXIS_COUNT, 0, (int)EStatusDouble.REAL_AXIS_STATUS);
                if (d_Matrix == null) return;

                CStatusArray.DoubleStatus = d_Matrix as double[,];

                d_Matrix = ACS.ReadVariableAsMatrix("M_INT", ACS.ACSC_NONE, 0, USE_AXIS_COUNT, 0, (int)EStatusInt.INT_AXIS_STATUS);
                if (d_Matrix == null) return;

                CStatusArray.IntStatus = d_Matrix as int[,];
            }

            public void GetMotionData(int servoNo, out CACSMotionData s)
            {               
                s = ObjectExtensions.Copy(MotionData[servoNo]);
            }



            public void GetSpeedData(int servoNo, out CMotorSpeedData data, int speedType = (int)EMotorSpeedACS.MANUAL_SLOW)
            {
                MotionData[servoNo].GetSpeedData(out data);
            }

            public void SetSpeedData(int servoNo, CMotorSpeedData data, int speedType = (int)EMotorSpeedACS.MANUAL_SLOW)
            {
                MotionData[servoNo].SetSpeedData(data, speedType);
            }

            public void GetTimeLimitData(int servoNo, out CMotorTimeLimitData data)
            {
                MotionData[servoNo].GetTimeLimitData(out data);
            }

            public void CheckSWLimit(int servoNo, double targetPos, out bool bExceedPlusLimit, out bool bExceedMinusLimit)
            {
                MotionData[servoNo].CheckSWLimit(targetPos, out bExceedPlusLimit, out bExceedMinusLimit);
            }
        }
        public class CACSRefComp
        {
            
        }

        public class CACSData
        {
            public CACSChannel Motion;

            public CACSData()
            {
                Motion = new CACSChannel();
            }

            public void SetACSMotionData(CACSMotionData[] motions)
            {
                for (int i = 0; i < motions.Length; i++)
                {
                    Motion.MotionData[i] = ObjectExtensions.Copy(motions[i]);
                }
            }
        }
        
    }

    public class MACS : MObject, IDisposable
    {
        //
        private CACSRefComp m_RefComp;
        private CACSData m_Data;
        public int InstalledAxisNo; // System에 Install된 max axis

        // remember speed type in this class for easy controlling
        public int SpeedType { get; set; } = (int)EMotorSpeedACS.MANUAL_SLOW;

        public string LastHWMessage { get; private set; }

        MTickTimer m_waitTimer = new MTickTimer();

        UInt16 APITimeOut = 5000;
        UInt16 APIJogTime = 100;    // Jog Timeout ms

        UInt32[] m_hAxis = new UInt32[USE_AXIS_COUNT];         // Axis handle
        UInt32[] m_hDevice = new UInt32[USE_AXIS_COUNT];       // Device handle

        public CServoStatus[] ServoStatus = new CServoStatus[USE_AXIS_COUNT];

        Thread m_hThread;   // Thread Handle

        public bool NeedCheckSafety { get; set; } = false;

        public MACS(CObjectInfo objInfo, CACSRefComp refComp, CACSData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            for (int i = 0; i < USE_AXIS_COUNT; i++)
            {
                ServoStatus[i] = new CServoStatus();
            }
        }

        ~MACS()
        {
            Dispose();
        }

        public void Dispose()
        {
            ThreadStop();
#if !SIMULATION_MOTION 
            AllServoStop();
            AllServoOff();
            CloseController();
#endif
        }

        public int SetData(CACSData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            //InstalledAxisNo = m_Data.CpuNo * MP_AXIS_PER_CPU;

            return SUCCESS;
        }

        public int GetData(out CACSData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public void SetACSMotionData(CACSMotionData[] motions)
        {
            m_Data.SetACSMotionData(motions);
        }

        public int ThreadStart()
        {
            m_hThread = new Thread(ThreadProcess);
            m_hThread.Start();

            return DEF_Error.SUCCESS;
        }

        public int ThreadStop()
        {
            m_hThread.Abort();

            return DEF_Error.SUCCESS;
        }

        public void ThreadProcess()
        {
            while (true)
            {
#if !SIMULATION_MOTION
                GetAllServoStatus();
#endif

                Sleep(DEF_Thread.ThreadSleepTime);
            }
        }

        public int IsSafeForMove(bool bStopMotion = false)
        {
            int iResult = SUCCESS;


            return SUCCESS;
        }

        public int CheckAxisStateForMove(int[] axisList)
        {
            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;


            for (int i = 0; i < axisList.Length; i++)
            {
                GetServoStatus(i);

                // servo on
                if (ServoStatus[i].IsServoOn == false)
                {
                    return GenerateErrorCode(ERR_ACS_NOT_SERVO_ON);
                }

                // alarm
                if (ServoStatus[i].IsDriverFault)
                {
                    return GenerateErrorCode(ERR_ACS_DETECTED_SERVO_ALARM);
                }

                // origin return
                if (ServoStatus[i].IsHomeComplete == false)
                {
                    return GenerateErrorCode(ERR_ACS_NOT_ORIGIN_RETURNED);
                }

                // plus limit
                if (ServoStatus[i].IsPlusSensor)
                {
                    return GenerateErrorCode(ERR_ACS_DETECTED_PLUS_LIMIT);
                }

                // alarm
                if (ServoStatus[i].IsMinusSensor)
                {
                    return GenerateErrorCode(ERR_ACS_DETECTED_MINUS_LIMIT);
                }
            }

            return SUCCESS;
        }


        private int GetDeviceLength(int deviceNo)
        {
            int length = 1;
            
            switch (deviceNo)
            {
                case (int)EACS_Device.ALL:
                    length = (int)EACS_Axis.MAX;
                    break;

                case (int)EACS_Device.STAGE1_X:
                    length = 1;
                    break;

                case (int)EACS_Device.STAGE1_Y:
                    length = 1;
                    break;

                case (int)EACS_Device.STAGE1_T:
                    length = 1;
                    break;                        

                default:
                    length = 1;
                    break;
            }           

            return length;
        }

        public int GetDeviceAxisList(int deviceNo, out int[] axisList)
        {
            int length = GetDeviceLength(deviceNo);
            axisList = new int[length];
            if (deviceNo < (int)EACS_Axis.ALL)
            {
                axisList[0] = deviceNo;
            }
            else
            {
                int index = 0;
                switch (deviceNo)
                {
                    case (int)EACS_Axis.ALL:
                        axisList[index++] = (int)EACS_Axis.STAGE1_X;
                        axisList[index++] = (int)EACS_Axis.STAGE1_Y;
                        axisList[index++] = (int)EACS_Axis.STAGE1_T;
                        break;

                    case (int)EACS_Axis.STAGE1_X:
                        axisList[index++] = (int)EACS_Axis.STAGE1_X;
                        break;

                    case (int)EACS_Axis.STAGE1_Y:
                        axisList[index++] = (int)EACS_Axis.STAGE1_Y;
                        break;

                    case (int)EACS_Axis.STAGE1_T:
                        axisList[index++] = (int)EACS_Axis.STAGE1_T;
                        break;
                        
                        break;
                }
            }

            return SUCCESS;
        }
        
        /// <summary>
        /// Device에 속한 축의 handle을 반환
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <param name="hAxis"></param>
        /// <returns></returns>
        private int GetDeviceAxisHandle(int deviceNo, out UInt32[] hAxis)
        {
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);

            return GetAxisHandleList(axisList, out hAxis);
        }

        /// <summary>
        /// 축의 번호를 가지고 Axis Handle을 반환
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="hAxis"></param>
        /// <returns></returns>
        private int GetAxisHandleList(int[] axisList, out UInt32[] hAxis)
        {
            hAxis = new UInt32[axisList.Length];
            for (int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }

               hAxis[i] = m_hAxis[servoNo];
            }

            return SUCCESS;
        }

        public int GetDevice_SpeedData(int deviceNo, out CMotorSpeedData[] speedData, int speedType = (int)EMotorSpeedACS.MANUAL_SLOW)
        {
            int length = GetDeviceLength(deviceNo);
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            speedData = new CMotorSpeedData[length];
          
            for (int i = 0; i < length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                m_Data.Motion.GetSpeedData(servoNo, out speedData[i], speedType);
                
            }

            return SUCCESS;
        }

        public int GetDevice_TimeLimitData(int deviceNo, out CMotorTimeLimitData[] timeLimit)
        {
            int length = GetDeviceLength(deviceNo);
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            timeLimit = new CMotorTimeLimitData[length];
           
            for (int i = 0; i < length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }

                m_Data.Motion.GetTimeLimitData(servoNo, out timeLimit[i]);
            }

            return SUCCESS;
        }

        private int GetDevice_MotionData(int deviceNo, out CACSMotionData[] MotionData)
        {
            int length = GetDeviceLength(deviceNo);
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            MotionData = new CACSMotionData[length];       

            for (int i = 0; i < length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                m_Data.Motion.GetMotionData(servoNo, out MotionData[i]);
            }

            return SUCCESS;
        }

        public int OpenController(bool bServoOn = false)
        {
            // 0. init
            int iResult;
            UInt32 rc;

            if(m_Data.Motion.IsChannelOpen == false)
                m_Data.Motion.ChannelOpen();

            return SUCCESS;
        }

        private int DeclareDevice(int length, UInt32[] hAxis, ref UInt32 hDevice)
        {


            return SUCCESS;
        }

        private int DeclareTempDevice(int length, int[] axisList, bool[] useAxis, ref UInt32 tDevice)
        {
            int iResult;
            UInt32[] hAxis;
            iResult = GetAxisHandleList(axisList, out hAxis);
            if (iResult != SUCCESS) return iResult;

            UInt32[] thAxis = new UInt32[length];
            for (int i = 0, j = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL || useAxis[i] == false) continue;
                thAxis[j++] = hAxis[i];
            }

            iResult = DeclareDevice(length, thAxis, ref tDevice);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        private int ClearDevice(UInt32 hDevice)
        {
            return SUCCESS;
        }

        public int CloseController()
        {
            if (m_Data.Motion.IsChannelOpen == true)
                m_Data.Motion.ChannelClose();

            return SUCCESS;
        }        

        public void GetAllServoStatus()
        {
            m_Data.Motion.GetACSBuffer();

            for (int i = 0; i < InstalledAxisNo; i++)
            {
                GetServoStatus(i);
            }
        }

        /// <summary>
        /// 속도 체크 필요함
        /// </summary>
        /// <param name="servoNo"></param>
        public void GetServoStatus(int servoNo)
        {
            UInt32 rc = 0;
            UInt32 returnValue = 0;

            // Double형 데이터 대입
            ServoStatus[servoNo].EncoderPos             = CStatusArray.DoubleStatus[servoNo,(int)EStatusDouble.ACT_POSITION];
            ServoStatus[servoNo].Velocity               = CStatusArray.DoubleStatus[servoNo, (int)EStatusDouble.ACT_VELOCITY];
            ServoStatus[servoNo].LoadFactor             = CStatusArray.DoubleStatus[servoNo, (int)EStatusDouble.ACT_TORQUE];
            ServoStatus[servoNo].PositionErr            = CStatusArray.DoubleStatus[servoNo, (int)EStatusDouble.ACT_POS_ERR];
            ServoStatus[servoNo].CommandPos             = CStatusArray.DoubleStatus[servoNo, (int)EStatusDouble.CMD_POSITION];
            ServoStatus[servoNo].CommandVelocity        = CStatusArray.DoubleStatus[servoNo, (int)EStatusDouble.CMD_VELOCITY];
            ServoStatus[servoNo].CommandAcceleration    = CStatusArray.DoubleStatus[servoNo, (int)EStatusDouble.CMD_ACCELERATION];
            ServoStatus[servoNo].CommandDeceleration    = CStatusArray.DoubleStatus[servoNo, (int)EStatusDouble.CMD_DECELERATION];
            // 상태 비트 적용
            int nMotorStatus = CStatusArray.IntStatus[servoNo, (int)EStatusInt.MOTOR_STATUS];
            if ((nMotorStatus & m_Data.Motion.ACS.ACSC_MST_MOVE) != 0)      ServoStatus[servoNo].IsBusy = true;         else ServoStatus[servoNo].IsBusy = false;
            if ((nMotorStatus & m_Data.Motion.ACS.ACSC_MST_INPOS) != 0)     ServoStatus[servoNo].IsInPosition = true;   else ServoStatus[servoNo].IsInPosition = false;
            if ((nMotorStatus & m_Data.Motion.ACS.ACSC_MST_ACC) != 0)       ServoStatus[servoNo].IsAccelerating = true; else ServoStatus[servoNo].IsAccelerating = false;
            if ((nMotorStatus & m_Data.Motion.ACS.ACSC_MST_ENABLE) != 0)    ServoStatus[servoNo].IsServoOn = true;      else ServoStatus[servoNo].IsServoOn = false;
           
            // 알람 비트 적용
            int nMotorFault = CStatusArray.IntStatus[servoNo, (int)EStatusInt.MOTOR_FAULT];
            if ((nMotorFault & m_Data.Motion.ACS.ACSC_SAFETY_RL) != 0)      ServoStatus[servoNo].IsPlusSensor = true;   else ServoStatus[servoNo].IsPlusSensor = false;
            if ((nMotorFault & m_Data.Motion.ACS.ACSC_SAFETY_LL) != 0)      ServoStatus[servoNo].IsMinusSensor = true;  else ServoStatus[servoNo].IsMinusSensor = false;
            if ((nMotorFault & m_Data.Motion.ACS.ACSC_SAFETY_DRIVE) != 0)   ServoStatus[servoNo].IsDriverFault = true;  else ServoStatus[servoNo].IsDriverFault = false;
            if ((nMotorFault & m_Data.Motion.ACS.ACSC_SAFETY_HOT) != 0)     ServoStatus[servoNo].IsMotorOverHeat = true;else ServoStatus[servoNo].IsMotorOverHeat = false;

            // Home Flag 비트 적용
            int nMotorHome = CStatusArray.IntStatus[servoNo, (int)EStatusInt.HOME_FLAG];
            if(nMotorHome != 0) ServoStatus[servoNo].IsHomeComplete = true; else ServoStatus[servoNo].IsHomeComplete = false;

        }

        public int ServoOn(int deviceNo)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null

            int iResult = ResetAlarm(deviceNo);
            if (iResult != SUCCESS) return iResult;

            m_Data.Motion.ACS.Enable(deviceNo);

            return SUCCESS;
        }

        public int ServoOff(int deviceNo)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null

            m_Data.Motion.ACS.Disable(deviceNo);

            return SUCCESS;
        }

        public int AllServoOn()
        {
            int iResult = 0;            

            for (int i = 0; i < (int)EACS_Device.ALL; i++)
            {
                iResult = ServoOn(i);

                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int AllServoOff()
        {
            m_Data.Motion.ACS.DisableAll();

            return SUCCESS;
        }        

        public int ResetAlarm(int deviceNo = (int)EACS_Device.ALL)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null

            UInt32 rc;

            return SUCCESS;
        }

        public int CheckHomeComplete(int servoNo, out bool bComplete)
        {
            bComplete = false;

            bComplete = ServoStatus[servoNo].IsHomeComplete;

            return SUCCESS;
        }

        public int CheckMoveComplete(int servoNo, out bool bComplete)
        {
            bComplete = false;

            bComplete = !ServoStatus[servoNo].IsBusy;
            
            return SUCCESS;
        }

        public int CheckMoveComplete(int[] axisList, out bool[] bComplete)
        {
            bComplete = new bool[axisList.Length];
            for (int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }

                int iResult = CheckMoveComplete(servoNo, out bComplete[i]);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int ComparePosition(int[] axisList, double[] dPos, out bool[] bJudge)
        {
            bJudge = new bool[axisList.Length];

            CACSMotionData[] motionData;

            for (int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    bJudge[i] = true;
                    continue;
                }
                double dCurPos = ServoStatus[servoNo].EncoderPos;
                GetDevice_MotionData(servoNo, out motionData);

                if (Math.Abs(dCurPos - dPos[i]) <= motionData[0].Tolerance) bJudge[i] = true;
                else bJudge[i] = false;
            }
            return SUCCESS;
        }

        public int AllServoStop()
        {
            int iResult = ServoMotionStop((int)EACS_Device.ALL);

            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }

        public int JogMoveStop(int deviceNo)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null
            
            return SUCCESS;
        }

        public int JogMoveStart(int deviceNo, bool jogDir, bool bJogFastMove = false)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null

            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;

            // Jog함수는 multi axis device를 고려하지 않고 작성
            if (deviceNo >= (int)EACS_Device.ALL) return SUCCESS;

            //============================================================================
            // Executes JOG operation.										
            //============================================================================
            // Motion data setting
            Int16[] Direction = new Int16[1];
            UInt16[] TimeOut = new UInt16[1] { APIJogTime };

    

            return SUCCESS;
        }

        public int ServoMotionStop(int deviceNo )
        {
            if(deviceNo == (int)EACS_Device.ALL)
            {
                int[] m_arrAxis = new int[(int)EACS_Device.ALL];

                foreach (int nAxis in m_arrAxis)  m_arrAxis[nAxis] = nAxis;
                m_arrAxis[(int)EACS_Device.ALL - 1] = -1;

                m_Data.Motion.ACS.HaltM(m_arrAxis);
            }
            else
                m_Data.Motion.ACS.Halt(deviceNo);

            return SUCCESS;
        }

        public int AllServoMotionStop()
        {
            ServoMotionStop((int)EACS_Device.ALL);

            return SUCCESS;
        }

        public int StartMoveToPos(int deviceNo, double[] pos, CMotorSpeedData[] tempSpeed = null, ushort waitMode = 0)
        {
            return MoveToPos(deviceNo, pos, tempSpeed, waitMode);

        }

        /// <summary>
        /// device의 모든 축을 함께 이동시킬 때 호출 
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <param name="pos"></param>
        /// <param name="tempSpeed"></param>
        /// <param name="waitMode"></param>
        /// <returns></returns>
        public int MoveToPos(int deviceNo, double[] pos, CMotorSpeedData[] tempSpeed = null, ushort waitMode = 0)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null

            ushort[] WaitForCompletion;


            return SUCCESS;
        }

        public int StartMoveToPos(int[] axisList, bool[] useAxis, double[] pos, CMotorSpeedData[] tempSpeed = null, ushort waitMode = 0)
        {
            return MoveToPos(axisList, useAxis, pos, tempSpeed, waitMode);

        }

        /// <summary>
        /// 축 번호 리스트와 useAxis등을 이용해서 Move to position
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="useAxis"></param>
        /// <param name="pos"></param>
        /// <param name="tempSpeed"></param>
        /// <param name="waitMode"></param>
        /// <returns></returns>
        public int MoveToPos(int[] axisList, bool[] useAxis, double[] pos, CMotorSpeedData[] tempSpeed = null, ushort waitMode = 0)
        {
            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;

            
            return SUCCESS;
        }

        public int Wait4Done(int[] axisList, bool[] useAxis, bool bWait4Home = false)
        {
            int iResult;
            // 0. init data
            // 0.1 get device length
            int length = 0;
            for (int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL || useAxis[i] == false) continue;
                length++;
            }
            if (length == 0)
            {
                return GenerateErrorCode(ERR_ACS_SELECTED_AXIS_NONE);
            }

            // 0.2 allocate temp device

            // 0.3 allocate data buffer
            CMotorTimeLimitData[] timeLimit = new CMotorTimeLimitData[length];
            bool[] bDone = new bool[length];
            int[] servoList = new int[length];

            // 0.4 copy motion data to buffer
            CMotorTimeLimitData[] tLimit;

            for (int i = 0, j = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL || useAxis[i] == false) continue;
                GetDevice_TimeLimitData(servoNo, out tLimit);

                timeLimit[j] = tLimit[0];
                bDone[j] = false;
                servoList[j] = servoNo;
                j++;
            }

            // 1. wait for done
            int sum = 0;
            m_waitTimer.StartTimer();
            while (true)
            {
                // 1.1 check safety
                iResult = IsSafeForMove(true);
                if (iResult != SUCCESS) return iResult;

                // 1.2 check all done
                if (sum == length)
                {
                    m_waitTimer.StopTimer();
                    return SUCCESS;
                }

                // 1.3 check each axis
                for (int i = 0; i < length; i++)
                {
                    if (bDone[i] == true) continue;

                    if (bWait4Home == false) // wait for move done
                    {
                        // 1.3.1 check each axis done
                        iResult = CheckMoveComplete(servoList[i], out bDone[i]);
                        if (iResult != SUCCESS) return iResult;
                        if (bDone[i] == true)
                        {
                            sum++;
                            Sleep(timeLimit[i].tSleepAfterMove * 1000);
                        }

                        // 1.3.2 check time limit
                        if (m_waitTimer.MoreThan(timeLimit[i].tMoveLimit * 1000))
                        {
                            return GenerateErrorCode(ERR_ACS_FAIL_SERVO_MOVE_IN_LIMIT_TIME);
                        }
                    }
                    else // wait for home done
                    {
                        // 1.3.1 check each axis done
                        iResult = CheckHomeComplete(servoList[i], out bDone[i]);
                        if (iResult != SUCCESS) return iResult;
                        if (bDone[i] == true)
                        {
                            sum++;
                            Sleep(timeLimit[i].tSleepAfterMove * 1000);
                        }

                        // 1.3.2 check time limit
                        if (m_waitTimer.MoreThan(timeLimit[i].tOriginLimit * 1000))
                        {
                            return GenerateErrorCode(ERR_ACS_FAIL_SERVO_HOME_IN_LIMIT_TIME);
                        }
                    }
                }

                Sleep(WhileSleepTime);
            }




            return SUCCESS;
        }

        public int Wait4HomeDone(int[] axisList, bool[] useAxis)
        {
            return Wait4Done(axisList, useAxis, true);
        }

        public int StartOriginReturn(int[] axisList, bool[] useAxis)
        {
            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;


            return SUCCESS;
        }

        public bool IsOriginReturn(int servoNo)
        {
            return true;
        }

        public void SetOriginFlag(int deviceNo, bool bFlag)
        {

        }

        public int OriginReturn(int deviceNo = (int)EACS_Device.ALL)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null

            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;


            // set origin flag
            SetOriginFlag(deviceNo, true);

            return SUCCESS;
        }

        public int GetServoPos(int servoNo, out double pos)
        {
            // this is obsolete because servo status is updated by thread
            pos = 0;


            return SUCCESS;
        }

        public bool IsServoWarning(int servoNo)
        {

            UInt32 returnValue = 0;

            return Convert.ToBoolean((returnValue >> 8) & 0x1);
        }
    }
}
