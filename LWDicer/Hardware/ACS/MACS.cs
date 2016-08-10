using System;
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

        public const int MAX_ACS_AXIS_COUNT = 32;
        public const int MAX_ACS_BUFFER_CNT = 64;

        public const int USE_ACS_AXIS_COUNT = (int)EACS_Axis.MAX;

        public enum EACSStatusInt
        {
            MOTOR_STATUS=0,
            MOTOR_FAULT,
            HOME_FLAG,
            INT_AXIS_STATUS,
        }

        public enum EACSStatusDouble
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

        public class CACSServoStatus
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
            public bool IsServoAlarm;
            // public bool Is;
            public bool DetectMinusSensor;
            public bool DetectPlusSensor;
            public bool DetectHomeSensor;
            
            public bool IsOriginReturned;       // origin return flag
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
            public CMotorSpeedData[] Speed = new CMotorSpeedData[(int)EMotorSpeed.MAX];

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

            public void GetSpeedData(out CMotorSpeedData data, int speedType = (int)EMotorSpeed.MANUAL_SLOW)
            {
                data = ObjectExtensions.Copy(Speed[speedType]);
            }

            public void SetSpeedData(CMotorSpeedData data, int speedType = (int)EMotorSpeed.MANUAL_SLOW)
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
            public static int[,] IntStatus = new int[USE_ACS_AXIS_COUNT, (int)EACSStatusInt.INT_AXIS_STATUS];
            public static double[,] DoubleStatus = new double[USE_ACS_AXIS_COUNT, (int)EACSStatusDouble.REAL_AXIS_STATUS];  
            
        }

        public class CACSChannel
        {
            public Channel ACS;
            private string addressTCP;
            private int portNum;
            public bool IsChannelOpen { get; private set; }


           public CACSMotionData[] MotionData = new CACSMotionData[USE_ACS_AXIS_COUNT];
            //CServoStatusArray ServoStatus;

            public CACSChannel(CACSMotionData[] motions = null)
            {
#if SIMULATION_MOTION_ACS
                return;
#endif
                ACS = new Channel();
                addressTCP = "10.0.0.100";
                portNum = 701;

                IsChannelOpen = false;
                if (motions == null)
                {
                    for (int i = 0; i < USE_ACS_AXIS_COUNT; i++)
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
#if !SIMULATION_MOTION_ACS
                ACS?.OpenCommEthernetTCP(addressTCP, portNum);
                
                IsChannelOpen = true;
#endif
                return SUCCESS;
            }
            public int ChannelClose()
            {
#if !SIMULATION_MOTION_ACS
                ACS?.CloseComm();

                IsChannelOpen = false;
#endif
                return SUCCESS;
            }

            public void GetACSBuffer()
            {
#if !SIMULATION_MOTION_ACS
                object doubleMatrix;
                object[,] objectArray = new object[USE_ACS_AXIS_COUNT, (int)EACSStatusDouble.REAL_AXIS_STATUS];

                try
                {
                    doubleMatrix = ACS?.ReadVariableAsMatrix("M_REAL", ACS.ACSC_NONE, 0, USE_ACS_AXIS_COUNT - 1, 0, (int)EACSStatusDouble.REAL_AXIS_STATUS - 1);
                    //d_Matrix = ACS?.ReadVariableAsMatrix("M_REAL", ACS.ACSC_NONE, 0, 7, 0, 7);
                    if (doubleMatrix == null) return;

                    objectArray = doubleMatrix as object[,];

                    for (int i = 0; i < USE_ACS_AXIS_COUNT; i++)
                        for (int j = 0; j < (int)EACSStatusDouble.REAL_AXIS_STATUS; j++)
                            CStatusArray.DoubleStatus[i, j] = (double)objectArray[i, j];
                }
                catch
                { }

                object intMatrix;
                object[,] objectIntArray = new object[USE_ACS_AXIS_COUNT, (int)EACSStatusInt.INT_AXIS_STATUS];

                try
                {
                    intMatrix = ACS?.ReadVariableAsMatrix("M_INT", ACS.ACSC_NONE, 0, USE_ACS_AXIS_COUNT-1, 0, (int)EACSStatusInt.INT_AXIS_STATUS-1);
                    //d_Matrix = ACS?.ReadVariableAsMatrix("M_INT", ACS.ACSC_NONE, 0, 7, 0, 2);
                    if (intMatrix == null) return;

                    objectIntArray = intMatrix as object[,];

                    for (int i = 0; i < USE_ACS_AXIS_COUNT; i++)
                        for (int j = 0; j < (int)EACSStatusInt.INT_AXIS_STATUS; j++)
                            CStatusArray.IntStatus[i, j] = (int)objectIntArray[i, j];
                }
                catch
                { }
#endif
            }

            public void GetMotionData(int servoNo, out CACSMotionData s)
            {
                s = ObjectExtensions.Copy(MotionData[servoNo]);
            }
            
            public void GetSpeedData(int servoNo, out CMotorSpeedData data, int speedType = (int)EMotorSpeed.MANUAL_SLOW)
            {
                MotionData[servoNo].GetSpeedData(out data);
            }

            public void SetSpeedData(int servoNo, CMotorSpeedData data, int speedType = (int)EMotorSpeed.MANUAL_SLOW)
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

            public CACSData()
            {
            }
        }
        
    }

    public class MACS : MObject, IDisposable
    {
        //
        private CACSRefComp m_RefComp;
        private CACSData m_Data;
        public CACSChannel m_AcsMotion;
        public int InstalledAxisNo; // System에 Install된 max axis

        // remember speed type in this class for easy controlling
        public int SpeedType { get; set; } = (int)EMotorSpeed.MANUAL_SLOW;

        public string LastHWMessage { get; private set; }

        MTickTimer m_waitTimer = new MTickTimer();

        UInt16 APITimeOut = 5000;
        UInt16 APIJogTime = 100;    // Jog Timeout ms

        UInt32[] m_hAxis = new UInt32[USE_ACS_AXIS_COUNT];         // Axis handle
        UInt32[] m_hDevice = new UInt32[USE_ACS_AXIS_COUNT];       // Device handle

        public CACSServoStatus[] ServoStatus { get; private set; } = new CACSServoStatus[USE_ACS_AXIS_COUNT];

        Thread m_hThread;   // Thread Handle

        public bool NeedCheckSafety { get; set; } = false;

        public MACS(CObjectInfo objInfo, CACSRefComp refComp, CACSData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            InstalledAxisNo = USE_ACS_AXIS_COUNT;

            m_AcsMotion = new CACSChannel();

            for (int i = 0; i < USE_ACS_AXIS_COUNT; i++)
            {
                ServoStatus[i] = new CACSServoStatus();
            }
            
            ThreadStart();
        }

        ~MACS()
        {
            Dispose();
        }

        public void Dispose()
        {
            ThreadStop();
#if !SIMULATION_MOTION_ACS
            StopAllServo();
            AllServoOff();
            CloseController();
#endif
        }

        public int SetData(CACSData source)
        {
            m_Data = ObjectExtensions.Copy(source);

            return SUCCESS;
        }

        public int GetData(out CACSData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public void SetACSMotionData(CACSMotionData[] motions)
        {
            for (int i = 0; i < motions.Length; i++)
            {
                m_AcsMotion.MotionData[i] = ObjectExtensions.Copy(motions[i]);
            }
        }

        public int ThreadStart()
        {
            m_hThread = new Thread(ThreadProcess);
            m_hThread.Start();

            return SUCCESS;
        }

        public int ThreadStop()
        {
            m_hThread.Abort();

            return SUCCESS;
        }

        public void ThreadProcess()
        {
            while (true)
            {
#if !SIMULATION_MOTION_ACS
                GetAllServoStatus();
#endif
                Sleep(DEF_Thread.ThreadSleepTime);
            }
        }

        private int IsSafeForMove(bool bStopMotion = false)
        {
            int iResult = SUCCESS;

            return SUCCESS;
        }

        private int CheckAxisStateForMove(int[] axisList)
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
                if (ServoStatus[i].IsOriginReturned == false)
                {
                    return GenerateErrorCode(ERR_ACS_NOT_ORIGIN_RETURNED);
                }

                // plus limit
                if (ServoStatus[i].DetectPlusSensor)
                {
                    return GenerateErrorCode(ERR_ACS_DETECTED_PLUS_LIMIT);
                }

                // alarm
                if (ServoStatus[i].DetectMinusSensor)
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

        private int GetDeviceAxisList(int deviceNo, out int[] axisList)
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

                    default:
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

        private int GetAxis_SpeedData(int servoNo, out CMotorSpeedData speedData, int speedType = (int)EMotorSpeed.MANUAL_SLOW)
        {    
            m_AcsMotion.GetSpeedData(servoNo, out speedData, speedType);               
            
            return SUCCESS;
        }

        private int GetDevice_TimeLimitData(int deviceNo, out CMotorTimeLimitData[] timeLimit)
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

                m_AcsMotion.GetTimeLimitData(servoNo, out timeLimit[i]);
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
                m_AcsMotion.GetMotionData(servoNo, out MotionData[i]);
            }
            return SUCCESS;
        }

        public int OpenController(bool bServoOn = false)
        {
            // 0. init
            int iResult;
            UInt32 rc;

            if(m_AcsMotion.IsChannelOpen == false)
                m_AcsMotion.ChannelOpen();

            return SUCCESS;
        }



        public int CloseController()
        {
            if (m_AcsMotion.IsChannelOpen == true)
                m_AcsMotion.ChannelClose();

            return SUCCESS;
        }        

        public void GetAllServoStatus()
        {
            m_AcsMotion.GetACSBuffer();

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
            ServoStatus[servoNo].EncoderPos             = CStatusArray.DoubleStatus[servoNo,(int)EACSStatusDouble.ACT_POSITION];
            ServoStatus[servoNo].Velocity               = CStatusArray.DoubleStatus[servoNo, (int)EACSStatusDouble.ACT_VELOCITY];
            ServoStatus[servoNo].LoadFactor             = CStatusArray.DoubleStatus[servoNo, (int)EACSStatusDouble.ACT_TORQUE];
            ServoStatus[servoNo].PositionErr            = CStatusArray.DoubleStatus[servoNo, (int)EACSStatusDouble.ACT_POS_ERR];
            ServoStatus[servoNo].CommandPos             = CStatusArray.DoubleStatus[servoNo, (int)EACSStatusDouble.CMD_POSITION];
            ServoStatus[servoNo].CommandVelocity        = CStatusArray.DoubleStatus[servoNo, (int)EACSStatusDouble.CMD_VELOCITY];
            ServoStatus[servoNo].CommandAcceleration    = CStatusArray.DoubleStatus[servoNo, (int)EACSStatusDouble.CMD_ACCELERATION];
            ServoStatus[servoNo].CommandDeceleration    = CStatusArray.DoubleStatus[servoNo, (int)EACSStatusDouble.CMD_DECELERATION];
            
            // 상태 비트 적용
            int nMotorStatus = CStatusArray.IntStatus[servoNo, (int)EACSStatusInt.MOTOR_STATUS];
            if ((nMotorStatus & m_AcsMotion.ACS.ACSC_MST_MOVE) != 0)      ServoStatus[servoNo].IsBusy = true;         else ServoStatus[servoNo].IsBusy = false;
            if ((nMotorStatus & m_AcsMotion.ACS.ACSC_MST_INPOS) != 0)     ServoStatus[servoNo].IsInPosition = true;   else ServoStatus[servoNo].IsInPosition = false;
            if ((nMotorStatus & m_AcsMotion.ACS.ACSC_MST_ACC) != 0)       ServoStatus[servoNo].IsAccelerating = true; else ServoStatus[servoNo].IsAccelerating = false;
            if ((nMotorStatus & m_AcsMotion.ACS.ACSC_MST_ENABLE) != 0)    ServoStatus[servoNo].IsServoOn = true;      else ServoStatus[servoNo].IsServoOn = false;
           
            // 알람 비트 적용
            int nMotorFault = CStatusArray.IntStatus[servoNo, (int)EACSStatusInt.MOTOR_FAULT];
            if (nMotorFault > 0)                                          ServoStatus[servoNo].IsServoAlarm = true;         else ServoStatus[servoNo].IsServoAlarm = false;
            if ((nMotorFault & m_AcsMotion.ACS.ACSC_SAFETY_RL) != 0)      ServoStatus[servoNo].DetectPlusSensor = true;     else ServoStatus[servoNo].DetectPlusSensor = false;
            if ((nMotorFault & m_AcsMotion.ACS.ACSC_SAFETY_LL) != 0)      ServoStatus[servoNo].DetectMinusSensor = true;    else ServoStatus[servoNo].DetectMinusSensor = false;
            if ((nMotorFault & m_AcsMotion.ACS.ACSC_SAFETY_DRIVE) != 0)   ServoStatus[servoNo].IsDriverFault = true;        else ServoStatus[servoNo].IsDriverFault = false;
            if ((nMotorFault & m_AcsMotion.ACS.ACSC_SAFETY_HOT) != 0)     ServoStatus[servoNo].IsMotorOverHeat = true;      else ServoStatus[servoNo].IsMotorOverHeat = false;

            // Home Flag 비트 적용
            int nMotorHome = CStatusArray.IntStatus[servoNo, (int)EACSStatusInt.HOME_FLAG];
            if(nMotorHome != 0) ServoStatus[servoNo].IsOriginReturned = true; else ServoStatus[servoNo].IsOriginReturned = false;

        }

        public int ServoOn(int deviceNo)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null

            int iResult = ResetAlarm(deviceNo);
            if (iResult != SUCCESS) return iResult;

            m_AcsMotion.ACS?.Enable(deviceNo);

            return SUCCESS;
        }

        public int ServoOff(int deviceNo)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null

            m_AcsMotion.ACS?.Disable(deviceNo);

            return SUCCESS;
        }

        public int AllServoOn()
        {
            int iResult = 0;            

            for (int i = 0; i < (int)EACS_Device.ALL; i++)
            {
                // 없는 축은 건너뜀
                if (i == 3 || i == 5) continue;

                iResult = ServoOn(i);

                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int AllServoOff()
        {
            m_AcsMotion.ACS?.DisableAll();

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

            bComplete = ServoStatus[servoNo].IsOriginReturned;

            return SUCCESS;
        }

        public int CheckMoveComplete(int servoNo, out bool bComplete)
        {
            bComplete = false;

            bComplete = ServoStatus[servoNo].IsInPosition;
            
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

        public int StopAllServo()
        {
            int iResult = StopServoMotion((int)EACS_Device.ALL);

            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }

        public int StopJogMove(int deviceNo)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null
            
            return SUCCESS;
        }

        public int StartJogMove(int deviceNo, bool jogDir, bool bJogFastMove = false)
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

        public int StopServoMotion(int deviceNo )
        {
            if(deviceNo == (int)EACS_Device.ALL)
            {
                int[] m_arrAxis = new int[(int)EACS_Device.ALL];

                foreach (int nAxis in m_arrAxis)  m_arrAxis[nAxis] = nAxis;
                m_arrAxis[(int)EACS_Device.ALL - 1] = -1;

                m_AcsMotion.ACS?.HaltM(m_arrAxis);
            }
            else
                m_AcsMotion.ACS?.Halt(deviceNo);

            return SUCCESS;
        }

        public int StopAllServoMotion()
        {
            StopServoMotion((int)EACS_Device.ALL);

            return SUCCESS;
        }
        
        private int CompareSpeedData(int AxisNo, CMotorSpeedData pSpeedData, out bool bCheck)
        {
            if(pSpeedData.Vel == ServoStatus[AxisNo].CommandVelocity &&
               pSpeedData.Acc == ServoStatus[AxisNo].CommandAcceleration &&
               pSpeedData.Dec == ServoStatus[AxisNo].CommandDeceleration)
            {
                bCheck = true;
                return SUCCESS;
            }

            bCheck = false;
            return SUCCESS;

        }

        private int SetSpeedData(int AxisNo, CMotorSpeedData pSpeedData)
        {
            m_AcsMotion.ACS?.SetVelocity(AxisNo, pSpeedData.Vel);
            m_AcsMotion.ACS?.SetAcceleration(AxisNo, pSpeedData.Acc);
            m_AcsMotion.ACS?.SetDeceleration(AxisNo, pSpeedData.Dec);
            
            return SUCCESS;
        }

        public int StartMoveToPos(int AxisNo, double pos, CMotorSpeedData tempSpeed = null)
        {
            return MoveToPos(AxisNo, pos, tempSpeed);
        }

        /// <summary>
        /// device의 모든 축을 함께 이동시킬 때 호출 
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <param name="pos"></param>
        /// <param name="tempSpeed"></param>
        /// <param name="waitMode"></param>
        /// <returns></returns>
        public int MoveToPos(int AxisNo, double pos, CMotorSpeedData tempSpeed = null)
        {
#if !SIMULATION_MOTION_ACS
            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;

            // 선택한 axisList 확인
            if (AxisNo == (int)EACS_Device.NULL) return GenerateErrorCode(ERR_ACS_SELECTED_AXIS_NONE);

            // 0. init data
            // 0.1 get device length            

            bool bCheck = false;
            CMotorSpeedData[] SpeedData = new CMotorSpeedData[1];

            SpeedData[0] = new CMotorSpeedData();
            //0.2 Motion Profile 적용


            // ACS에 설정된 Vel,Acc,Dec와 비교를 함
            if (tempSpeed == null) GetAxis_SpeedData(AxisNo, out SpeedData[0]);
            else
                SpeedData[0] = tempSpeed;

            CompareSpeedData(AxisNo, SpeedData[0], out bCheck);
            if (bCheck == false)
            {
                SetSpeedData(AxisNo, SpeedData[0]);
                Sleep(50);
            }

            try
            {
                // 0.3 Motion Position 적용
                m_AcsMotion.ACS.ToPoint(0, AxisNo, pos);
            }
            catch
            { }

            // 0.4 Motion Moving 지령
            //m_AcsMotion.ACS.GoM(AxisNo);
            Sleep(50);            

            // 0.5 Motion Complete 확인
            int[] AxisList = new int[1];
            AxisList[0] = AxisNo;
            bool[] bUse = new bool[1];
            bUse[0] = true;

            //iResult = Wait4Done(AxisList, bUse);

            //if (iResult != SUCCESS) return iResult;            
            
#endif
            return SUCCESS;
        }

        public int StartMoveToPos(int[] axisList, bool[] useAxis, double[] pos, CMotorSpeedData[] tempSpeed = null)
        {
            return MoveToPos(axisList, useAxis, pos, tempSpeed);
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
        public int MoveToPos(int[] axisList, bool[] useAxis, double[] pos, CMotorSpeedData[] tempSpeed = null)
        {

            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;

            // 선택한 axisList 확인
            if(axisList==null) return GenerateErrorCode(ERR_ACS_SELECTED_AXIS_NONE);

            // 0. init data
            // 0.1 get device length            

            int length = 0;
            bool bCheck = false;
            CMotorSpeedData[] SpeedData = new CMotorSpeedData[1];

            //0.2 Motion Profile 적용
            for (int i = 0; i < axisList.Length; i++)
            {
                if (axisList[i] == (int)EACS_Axis.NULL || useAxis[i] == false) continue;
                                
                // ACS에 설정된 Vel,Acc,Dec와 비교를 함
                if (tempSpeed == null) GetAxis_SpeedData(axisList[i], out SpeedData[0]);
                
                CompareSpeedData(axisList[i], SpeedData[0], out bCheck);
                if (bCheck == false)
                {
                    SetSpeedData(axisList[i], SpeedData[0]);
                    Sleep(50);
                }

                length++;
            }

            if (length == 0)
            {
                return GenerateErrorCode(ERR_ACS_SELECTED_AXIS_NONE);
            }

#if SIMULATION_MOTION_ACS
            return SUCCESS;
#endif
            try
            {
                // 0.3 Motion Position 적용
                m_AcsMotion.ACS?.ToPointM(m_AcsMotion.ACS.ACSC_AMF_WAIT, axisList, pos);

                // 0.4 Motion Moving 지령
                m_AcsMotion.ACS?.GoM(axisList);
            }
            catch
            { }

            Sleep(50);

            // 0.5 Motion Complete 확인
            iResult = Wait4Done(axisList, useAxis);

            if (iResult != SUCCESS) return iResult;

            return SUCCESS;

        }

        public int Wait4Done(int[] axisList, bool[] useAxis, bool bWait4Home = false)
        {
#if !SIMULATION_MOTION_ACS
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
#endif
            return SUCCESS;
        }

        public int Wait4HomeDone(int[] axisList, bool[] useAxis)
        {
            return Wait4Done(axisList, useAxis, true);
        }

        public int OriginReturn(int[] axisList, bool[] useAxis)
        {
#if !SIMULATION_MOTION_ACS
            // 0. init data
            // 0.1 get device length
            int length = 0;
            for (int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL || useAxis[i] == false) continue;

                OriginReturn(servoNo);
                length++;
            }

            if (length == 0)
            {
                return GenerateErrorCode(ERR_ACS_SELECTED_AXIS_NONE);
            }
#endif
            return SUCCESS;
        }

        public int OriginReturn(int servoNo)
        {
#if !SIMULATION_MOTION_ACS
            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;

            m_AcsMotion.ACS?.RunBuffer(servoNo, null);
#endif
            return SUCCESS;
        }

        public bool IsOriginReturned(int servoNo)
        {
            return ServoStatus[servoNo].IsOriginReturned;
        }


        public int GetServoPos(int servoNo, out double pos)
        {
            // this is obsolete because servo status is updated by thread
            pos = 0;

            pos = ServoStatus[servoNo].EncoderPos;

            return SUCCESS;
        }

        public bool IsServoWarning(int servoNo)
        {
            return ServoStatus[servoNo].IsServoAlarm;            
        }
    }
}
