using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MotionYMC;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_IO;
using static LWDicer.Control.DEF_Motion;

namespace LWDicer.Control
{
    public class DEF_Yaskawa
    {
        public const int ERR_YASKAWA_INVALID_CONTROLLER                  = 1;
        public const int ERR_YASKAWA_FAIL_OPEN_YMC                       = 2;
        public const int ERR_YASKAWA_FAIL_SET_TIMEOUT                    = 3;
        public const int ERR_YASKAWA_FAIL_CHANGE_CONTROLLER              = 4;
        public const int ERR_YASKAWA_FAIL_DECLARE_AXIS                   = 5;
        public const int ERR_YASKAWA_FAIL_CLEAR_AXIS                     = 6;
        public const int ERR_YASKAWA_FAIL_DECLARE_DEVICE                 = 7;
        public const int ERR_YASKAWA_FAIL_CLEAR_DEVICE                   = 8;
        public const int ERR_YASKAWA_FAIL_SERVO_ON                       = 9;
        public const int ERR_YASKAWA_FAIL_SERVO_OFF                      = 10;
        public const int ERR_YASKAWA_FAIL_RESET_ALARM                    = 11;
        public const int ERR_YASKAWA_FAIL_GET_MOTION_PARAM               = 12;
        public const int ERR_YASKAWA_FAIL_SERVO_STOP                     = 13;
        public const int ERR_YASKAWA_FAIL_SERVO_MOVE_JOG                 = 14;
        public const int ERR_YASKAWA_FAIL_SERVO_MOVE_DRIVING_POSITIONING = 15;
        public const int ERR_YASKAWA_FAIL_SERVO_MOVE_HOME                = 16;
        public const int ERR_YASKAWA_FAIL_SERVO_GET_POS                  = 17;
        public const int ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE       = 18;
        public const int ERR_YASKAWA_FAIL_GET_REGISTER_DATA              = 19;
        public const int ERR_YASKAWA_FAIL_SERVO_MOVE_IN_LIMIT_TIME       = 20;
        public const int ERR_YASKAWA_FAIL_SERVO_HOME_IN_LIMIT_TIME       = 21;
        public const int ERR_YASKAWA_TARGET_POS_EXCEED_PLUS_LIMIT        = 22;
        public const int ERR_YASKAWA_TARGET_POS_EXCEED_MINUS_LIMIT       = 23;
        public const int ERR_YASKAWA_DETECTED_DOOR_OPEN                  = 24;
        public const int ERR_YASKAWA_DETECTED_ESTOP                      = 25;
        public const int ERR_YASKAWA_DETECTED_PLUS_LIMIT                 = 26;
        public const int ERR_YASKAWA_DETECTED_MINUS_LIMIT                = 27;
        public const int ERR_YASKAWA_DETECTED_SERVO_ALARM                = 28;
        public const int ERR_YASKAWA_NOT_ORIGIN_RETURNED                 = 29;
        public const int ERR_YASKAWA_NOT_SERVO_ON                        = 30;
        public const int ERR_YASKAWA_SELECTED_AXIS_NONE                  = 31;
        public const int ERR_YASKAWA_OBSOLETE_FUNCTION                   = 32;

        public const int MAX_MP_CPU = 4;    // pci board EA
        public const int MAX_MP_PORT = 2;   // ports per board
        public const int MP_AXIS_PER_PORT = 8; // physical axis per port
        public const int MP_AXIS_PER_CPU = 16; // MAX_MP_PORT * MP_AXIS_PER_PORT; // physical axis per cpu
        public const int MAX_MP_AXIS = 64; // MAX_MP_CPU * MAX_MP_PORT * MP_AXIS_PER_PORT;

        public const int UNIT_REF = 1000;// 0.001 Reference Unit( 1mm )

        public enum EMPRegister
        {
            S,  // System
            M,  // Data
            I,  // Input
            O,  // Output
            C,  // Constant
            D,  // D Register
        }

        public enum EMPData
        {
            B,  // bit
            W,  // int
            L,  // long int
            F,  // float
        }

        /// <summary>
        /// Yaskawa Motion Bord Type
        /// </summary>
        public enum EMPBoard
        {
            // cpu 갯수는 board의 갯수
            // port 1 : physical axis 1~16, logical axis 1~16
            // port 2 : physical axis 1~16 * 2EA, logical axis 1~32
            MP2100,     // cpu 1, port 1
            MP2100M,    // cpu 1, port 2
            MP2101,
            MP2101M,
            MP2101T,
            MP2101TM,   // cpu 1, port 2
        }

        /// <summary>
        /// Motor의 원점 복귀에 필요한 설정을 정의
        /// </summary>
        public class CMPMotorOriginData
        {
            public int Method;          // = (UInt16)CMotionAPI.ApiDefs.HMETHOD_INPUT_C;
            public int Dir;             // = (UInt16)CMotionAPI.ApiDefs.DIRECTION_NEGATIVE; // Home Direction
            public double FastSpeed;    // Approach speed [reference unit/s], 원점복귀 접근 속도
            public double SlowSpeed;    // Creep speed [reference unit/s], C상 pulse rising -> falling 이동 속도
            public double HomeOffset;   // C상 pulse falling후의 원점 복귀 offset

            public CMPMotorOriginData(double FastSpeed = 5, double SlowSpeed = 1, double HomeOffset = 10,
                int Dir = (int)CMotionAPI.ApiDefs.DIRECTION_NEGATIVE, int Method = (int)CMotionAPI.ApiDefs.HMETHOD_INPUT_C)
            {
                this.Method = Method;
                this.Dir = Dir;
                this.FastSpeed = FastSpeed;
                this.SlowSpeed = SlowSpeed;
                this.HomeOffset = HomeOffset;
            }
        }

        /// <summary>
        /// Servo Motor 의 현재 상태를 정의한다.
        /// </summary>
        public class CMPServoStatus
        {
            public double EncoderPos;
            public double Velocity;     //Servo 현재 속도
            public bool IsReady;
            public bool IsAlarm;
            public bool IsServoOn;

            public bool DetectMinusSensor;
            public bool DetectPlusSensor;
            public bool DetectHomeSensor;
            public int LoadFactor;

            public int AlarmCode;
            public bool IsOriginReturned;       // origin return flag
        }

        /// <summary>
        /// Yaskawa Motion Board에 연결되는 각각의 Motion (Axis)를 정의
        /// </summary>
        public class CMPMotionData
        {
            // General
            public string Name;         // Name of Axis, YMC API Call 할 때, Max 8자 제한 있음
            public bool Exist;          // Use of Axis. if false, Axis not exist.
            public double Tolerance;    // Position Tolerance

            // Software Limit
            public CMotorSWLimit PosLimit;

            // Speed
            public double MaxVelocity;          // Maximum feeding speed [reference unit/s]
            public CMotorSpeedData[] Speed = new CMotorSpeedData[(int)EMotorSpeed.MAX];

            // Time Limit
            public CMotorTimeLimitData TimeLimit;

            // Home
            public CMPMotorOriginData OriginData;
            
            // below list is defined in MOTION_DATA of YMCMotion
            public Int16 CoordinateSystem;      // Coordinate system specified
            public Int16 MoveType;              // Motion type
            public Int16 VelocityType;          // Speed type
            public Int16 AccDecType;            // Acceleration type
            public Int16 FilterType;            // Filter type
            public Int16 DataType;              // Data type (0: immediate, 1: indirect designation)
            public Int32 FilterTime;            // Filter time [ms]
            //public Int32 MaxVelocity;           // Maximum feeding speed [reference unit/s]
            //public Int32 Acceleration;          // Acceleration [reference unit/s2], acceleration time constant [ms]
            //public Int32 Deceleration;          // Deceleration [reference unit/s2], deceleration time constant [ms]
            //public Int32 Velocity;              // Feeding speed [reference unit/s], Offset speed
            //public Int32 ApproachVelocity;      // Approach speed [reference unit/s]
            //public Int32 CreepVelocity;         // Creep speed [reference unit/s]

            public CMPMotionData()
            {
                // General
                Name = "NotExist";
                Exist = false;
                Tolerance = 0.001;

                // Software Limit
                PosLimit = new CMotorSWLimit();

                // Speed
                MaxVelocity = 100;
                for(int i = 0; i < Speed.Length; i++)
                {
                    Speed[i] = new CMotorSpeedData(10, 100, 100);
                }

                // Time Limit
                TimeLimit = new CMotorTimeLimitData(10, 0.01, 20);

                // Home
                OriginData = new CMPMotorOriginData();

                // MOTION_DATA
                CoordinateSystem = (Int16)CMotionAPI.ApiDefs.WORK_SYSTEM;
                MoveType = (Int16)CMotionAPI.ApiDefs.MTYPE_RELATIVE;

                FilterTime = 10;                // Filter time [0.1 ms]
                VelocityType = (Int16)CMotionAPI.ApiDefs.VTYPE_UNIT_PAR;    // Speed [reference unit/s]
                AccDecType = (Int16)CMotionAPI.ApiDefs.ATYPE_UNIT_PAR;  // Time constant specified [ms] //ATYPE_TIME
                FilterType = (Int16)CMotionAPI.ApiDefs.FTYPE_S_CURVE;   // Moving average filter (simplified S-curve)
                DataType = 0;                                           // All parameters directly specified
            }
            
            public void GetMotionData(ref CMotionAPI.MOTION_DATA s, int speedType = (int)EMotorSpeed.MANUAL_SLOW, CMotorSpeedData tempSpeed = null)
            {
                // speed value를 UNIT_REF 적용해서 MOTION_DATA로 변환 
                s.CoordinateSystem = CoordinateSystem;
                s.MoveType         = MoveType;
                s.VelocityType     = VelocityType;
                s.AccDecType       = AccDecType;
                s.FilterType       = FilterType;
                s.DataType         = DataType;
                s.FilterTime       = FilterTime;
                s.MaxVelocity      = (int)MaxVelocity * UNIT_REF;
                s.Velocity         = (int)Speed[speedType].Vel * UNIT_REF;
                s.Acceleration     = (int)Speed[speedType].Acc * UNIT_REF;
                s.Deceleration     = (int)Speed[speedType].Dec * UNIT_REF;
                s.ApproachVelocity = (int)OriginData.FastSpeed * UNIT_REF;
                s.CreepVelocity    = (int)OriginData.SlowSpeed * UNIT_REF;

                if (tempSpeed != null)
                {
                    if (tempSpeed.Vel != 0)
                        s.Velocity = (int)tempSpeed.Vel * UNIT_REF;
                    if (tempSpeed.Acc != 0)
                        s.Acceleration = (int)tempSpeed.Acc * UNIT_REF;
                    if (tempSpeed.Dec != 0)
                        s.Deceleration = (int)tempSpeed.Dec * UNIT_REF;
                }
            }
            
            public void GetMotion_HomeData(ref CMotionAPI.MOTION_DATA s, 
                out UInt16 Method, out UInt16 Dir, out CMotionAPI.POSITION_DATA Position)
            {
                GetMotionData(ref s);

                Method = (UInt16)OriginData.Method;
                Dir = (UInt16)OriginData.Dir;
                Position.PositionData = (int)OriginData.HomeOffset * UNIT_REF;
                Position.DataType = (UInt16)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;
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

        /// <summary>
        /// Yaskawa Motion의 Module Configurator에서 구성하는 CPU, IO, SVC, SVR 등의 각 Unit의
        /// 위치 설정을 정의
        /// </summary>
        public class CMPRackTable
        {
            public int RackNo = 1;
            public int SlotNo = 0;
            public int SubSlotNo = 3;
        }

        public class CYaskawaRefComp
        {

        }

        /// <summary>
        /// Yaskawa Motion Board 정의
        /// </summary>
        public class CMPBoard
        {
            public int CPUIndex = 1;                    // CPU 개수
            public EMPBoard Type = EMPBoard.MP2101TM;   // Board Type
            public int SlotLength;                      // Board 뒷면의 slot 개수

            public CMPMotionData[] MotionData = new CMPMotionData[MP_AXIS_PER_CPU];

            // SVB, SVB-01, SVC, SVC-01
            public CMPRackTable[] SPort = new CMPRackTable[MAX_MP_PORT];
            public CMPRackTable VPort = new CMPRackTable();  // SVR, Virtual Port

            public CMPBoard(int CPUIndex = 1, EMPBoard Type = EMPBoard.MP2101TM, CMPMotionData[] motions = null)
            {
                this.CPUIndex = CPUIndex;
                this.Type = Type;
                if(motions == null)
                {
                    for (int i = 0; i < MP_AXIS_PER_CPU; i++)
                    {
                        MotionData[i] = new CMPMotionData();
                    }
                }
                else
                {
                    for (int i = 0; i < motions?.Length; i++)
                    {
                        MotionData[i] = ObjectExtensions.Copy(motions[i]);
                    }
                }
                for (int i = 0; i < MAX_MP_PORT; i++)
                {
                    SPort[i] = new CMPRackTable();
                }

                switch (Type)
                {
                    case EMPBoard.MP2100:
                    case EMPBoard.MP2101:
                    case EMPBoard.MP2101T:
                        SPort[0].RackNo = 1;
                        SPort[0].SlotNo = 0;
                        SPort[0].SubSlotNo = 3;

                        VPort.RackNo = 1;
                        VPort.SlotNo = 0;
                        VPort.SubSlotNo = 4;

                        SlotLength = 1;
                        break;
                    case EMPBoard.MP2100M:
                    case EMPBoard.MP2101M:
                    case EMPBoard.MP2101TM:
                        SPort[0].RackNo = 1;
                        SPort[0].SlotNo = 0;
                        SPort[0].SubSlotNo = 3;

                        SPort[1].RackNo = 1;
                        SPort[1].SlotNo = 1;
                        SPort[1].SubSlotNo = 1;

                        VPort.RackNo = 1;
                        VPort.SlotNo = 0;
                        VPort.SubSlotNo = 4;

                        SlotLength = 2;
                        break;
                }
            }

            public string GetMotionRegAddr(int servoNo)
            {
                // I/O W 8000 + (LineNo-1) x 800h + (AxisNo - 1) x 80h
                int addr = 0x8000 + (CPUIndex * 0x800) + servoNo * 0x80;
                string regAddr = addr.ToString("X4");
                return regAddr;
            }
            public void GetMotionData(int servoNo, out CMPMotionData s)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                s = ObjectExtensions.Copy(MotionData[servoNo]);
            }

            public void GetMotionData(int servoNo, ref CMotionAPI.MOTION_DATA s, int speedType = (int)EMotorSpeed.MANUAL_SLOW, CMotorSpeedData tempSpeed = null)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                MotionData[servoNo].GetMotionData(ref s, speedType, tempSpeed);
            }

            public void GetMotion_HomeData(int servoNo, ref CMotionAPI.MOTION_DATA s,
                out UInt16 Method, out UInt16 Dir, out CMotionAPI.POSITION_DATA Position)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                MotionData[servoNo].GetMotion_HomeData(ref s, out Method, out Dir, out Position);
            }

            public void GetSpeedData(int servoNo, out CMotorSpeedData data, int speedType = (int)EMotorSpeed.MANUAL_SLOW)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                MotionData[servoNo].GetSpeedData(out data);
            }

            public void SetSpeedData(int servoNo, CMotorSpeedData data, int speedType = (int)EMotorSpeed.MANUAL_SLOW)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                MotionData[servoNo].SetSpeedData(data, speedType);
            }

            public void GetTimeLimitData(int servoNo, out CMotorTimeLimitData data)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                MotionData[servoNo].GetTimeLimitData(out data);
            }

            public void CheckSWLimit(int servoNo, double targetPos, out bool bExceedPlusLimit, out bool bExceedMinusLimit)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                MotionData[servoNo].CheckSWLimit(targetPos, out bExceedPlusLimit, out bExceedMinusLimit);
            }
        }

        public class CYaskawaData
        {
            //
            public int MPComPort = 1;      // MP Background 프로그램에서 설정하는 communication port number, 거의 고정 1

            public int CpuNo = 1;       // PCI 모드일때는 보드 숫자라고 생각하면 됨
            public CMPBoard[] MPBoard = new CMPBoard[MAX_MP_CPU];

            public CYaskawaData(int MPComPort = 1, int CpuNo = 1, CMPBoard[] boards = null)
            {
                this.MPComPort = MPComPort;
                this.CpuNo = CpuNo;

                if (boards == null)
                {
                    for (int i = 0; i < MAX_MP_CPU; i++)
                    {
                        MPBoard[i] = new CMPBoard();
                    }
                } else
                {
                    for(int i = 0; i < boards?.Length; i++)
                    {
                        MPBoard[i] = ObjectExtensions.Copy(boards[i]);
                    }
                }
            }

            public void SetMPMotionData(CMPMotionData[] motions)
            {
                for(int i = 0; i < motions.Length; i++)
                {
                    int boardNo = i / MP_AXIS_PER_CPU;
                    int motionNo = i % MP_AXIS_PER_CPU;
                    MPBoard[boardNo].MotionData[motionNo] = ObjectExtensions.Copy(motions[i]);
                }
            }
        }
    }

    /// <summary>
    /// 우선 작성. ChangeController 함수는 만들어는 놨으나 호출은 고려하지 않고 우선 작성함
    /// 사용한 Yaskawa의 라이브러리는 아래와 같음
    /// English Version Released on September 20 2013
    /// Package version  :  Ver.2.00
    /// API DLL version  :  Ver.2.0.0.0
    /// Driver version     :  Ver.2.0.0.0
    /// Applicable firmware version v2.46 or later
    /// </summary>
    public class MYaskawa : MObject, IDisposable
    {
        //
        private CYaskawaRefComp m_RefComp;
        private CYaskawaData m_Data;
        public int InstalledAxisNo { get; private set; } // System에 Install된 max axis

        // remember speed type in this class for easy controlling
        public int SpeedType { get; set; } = (int)EMotorSpeed.MANUAL_SLOW;

        public string LastHWMessage { get; private set; }

        MTickTimer m_waitTimer = new MTickTimer();
        UInt16 APITimeOut = 5000;
        UInt16 APIJogTime = 100;    // Jog Timeout ms

        //
        UInt32[] m_hController = new UInt32[MAX_MP_CPU];    // Yaskawa controller handle
        UInt32[] m_hAxis = new UInt32[MAX_MP_AXIS];         // Axis handle
        UInt32[] m_hDevice = new UInt32[MAX_MP_AXIS];       // Device handle
        public CMPServoStatus[] ServoStatus { get; private set; } = new CMPServoStatus[MAX_MP_AXIS];

        //
        Thread m_hThread;   // Thread Handle

        //
        public bool NeedCheckSafety { get; set; } = false;

        public Dictionary<string, string> ErrorDictionary;

        public MYaskawa(CObjectInfo objInfo, CYaskawaRefComp refComp, CYaskawaData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            for(int i = 0; i < MAX_MP_AXIS; i++)
            {
                ServoStatus[i] = new CMPServoStatus();
            }

            MakeErrorDictionary();
        }

        ~MYaskawa()
        {
            Dispose();
        }

        public void Dispose()
        {
            ThreadStop();
#if !SIMULATION_MOTION_YMC 
            AllServoStop();
            AllServoOff();
            CloseController();
#endif
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CYaskawaData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            InstalledAxisNo = m_Data.CpuNo * MP_AXIS_PER_CPU;

            return SUCCESS;
        }

        public int GetData(out CYaskawaData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public void SetMPMotionData(CMPMotionData[] motions)
        {
            m_Data.SetMPMotionData(motions);
        }
        #endregion

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
#if !SIMULATION_MOTION_YMC
                GetAllServoStatus();
#endif

                Sleep(DEF_Thread.ThreadSleepTime);
            }
        }

        public int IsSafeForMove(bool bStopMotion = false)
        {
            int iResult = SUCCESS;

            //// check estop pushed
            //if()
            //{
            //    return GenerateErrorCode(ERR_YASKAWA_DETECTED_ESTOP);
            //}

            //// check door opened
            //if ()
            //{
            //    if(bStopMotion)
            //    {
            //        AllServoStop();
            //    }
            //    return GenerateErrorCode(ERR_YASKAWA_DETECTED_DOOR_OPEN);
            //}

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
                    return GenerateErrorCode(ERR_YASKAWA_NOT_SERVO_ON);
                }

                // alarm
                if(ServoStatus[i].IsAlarm)
                {
                    return GenerateErrorCode(ERR_YASKAWA_DETECTED_SERVO_ALARM);
                }

                // origin return
                if (ServoStatus[i].IsOriginReturned == false)
                {
                    return GenerateErrorCode(ERR_YASKAWA_NOT_ORIGIN_RETURNED);
                }

                // plus limit
                if (ServoStatus[i].DetectPlusSensor)
                {
                    return GenerateErrorCode(ERR_YASKAWA_DETECTED_PLUS_LIMIT);
                }

                // alarm
                if (ServoStatus[i].DetectMinusSensor)
                {
                    return GenerateErrorCode(ERR_YASKAWA_DETECTED_MINUS_LIMIT);
                }
            }

            return SUCCESS;
        }

        private int GetDeviceLength(int deviceNo)
        {
            int length = 1;
            if (deviceNo < (int)EYMC_Device.ALL)
            {
                // axis, null, default
                length = 1;
            }
            else
            {
                switch (deviceNo)
                {
                    case (int)EYMC_Device.ALL:
                        length = (int)EYMC_Axis.MAX;
                        break;

                    case (int)EYMC_Device.LOADER:
                        length = 1;
                        break;

                    case (int)EYMC_Device.PUSHPULL:
                        length = 1;
                        break;

                    case (int)EYMC_Device.CENTERING1:
                        length = 1;
                        break;

                    case (int)EYMC_Device.S1_ROTATE:
                        length = 1;
                        break;

                    case (int)EYMC_Device.S1_CLEAN_NOZZLE:
                        length = 1;
                        break;

                    case (int)EYMC_Device.S1_COAT_NOZZLE:
                        length = 1;
                        break;

                    case (int)EYMC_Device.CENTERING2:
                        length = 1;
                        break;

                    case (int)EYMC_Device.S2_ROTATE:
                        length = 1;
                        break;

                    case (int)EYMC_Device.S2_CLEAN_NOZZLE:
                        length = 1;
                        break;

                    case (int)EYMC_Device.S2_COAT_NOZZLE:
                        length = 1;
                        break;

                    case (int)EYMC_Device.UPPER_HANDLER:
                        length = 2;
                        break;

                    case (int)EYMC_Device.LOWER_HANDLER:
                        length = 2;
                        break;

                    case (int)EYMC_Device.CAMERA1:
                        length = 1;
                        break;

                    case (int)EYMC_Device.SCANNER1:
                        length = 1;
                        break;

                    default:
                        length = 1;
                        break;
                }
            }

            return length;
        }

        public int GetDeviceAxisList(int deviceNo, out int[] axisList)
        {
            int length = GetDeviceLength(deviceNo);
            axisList = new int[length];
            if (deviceNo < (int)EYMC_Device.ALL)
            {
                axisList[0] = deviceNo;
            }
            else
            {
                int index = 0;
                switch (deviceNo)
                {
                    case (int)EYMC_Device.ALL:
                        axisList[index++] = (int)EYMC_Axis.LOADER_Z;
                        axisList[index++] = (int)EYMC_Axis.PUSHPULL_Y;
                        axisList[index++] = (int)EYMC_Axis.PUSHPULL_X1;
                        axisList[index++] = (int)EYMC_Axis.S1_CHUCK_ROTATE_T;
                        axisList[index++] = (int)EYMC_Axis.S1_CLEAN_NOZZLE_T;
                        axisList[index++] = (int)EYMC_Axis.S1_COAT_NOZZLE_T;
                        axisList[index++] = (int)EYMC_Axis.PUSHPULL_X2;
                        axisList[index++] = (int)EYMC_Axis.S2_CHUCK_ROTATE_T;
                        axisList[index++] = (int)EYMC_Axis.S2_CLEAN_NOZZLE_T;
                        axisList[index++] = (int)EYMC_Axis.S2_COAT_NOZZLE_T;
                        axisList[index++] = (int)EYMC_Axis.UPPER_HANDLER_X;
                        axisList[index++] = (int)EYMC_Axis.UPPER_HANDLER_Z;
                        axisList[index++] = (int)EYMC_Axis.LOWER_HANDLER_X;
                        axisList[index++] = (int)EYMC_Axis.LOWER_HANDLER_Z;
                        axisList[index++] = (int)EYMC_Axis.CAMERA1_Z;
                        axisList[index++] = (int)EYMC_Axis.SCANNER1_Z;
                        break;

                    case (int)EYMC_Device.LOADER:
                        axisList[index++] = (int)EYMC_Axis.LOADER_Z;
                        break;

                    case (int)EYMC_Device.PUSHPULL:
                        axisList[index++] = (int)EYMC_Axis.PUSHPULL_Y;
                        break;

                    case (int)EYMC_Device.CENTERING1:
                        axisList[index++] = (int)EYMC_Axis.PUSHPULL_X1;
                        break;

                    case (int)EYMC_Device.S1_ROTATE:
                        axisList[index++] = (int)EYMC_Axis.S1_CHUCK_ROTATE_T;
                        break;

                    case (int)EYMC_Device.S1_CLEAN_NOZZLE:
                        axisList[index++] = (int)EYMC_Axis.S1_CLEAN_NOZZLE_T;
                        break;

                    case (int)EYMC_Device.S1_COAT_NOZZLE:
                        axisList[index++] = (int)EYMC_Axis.S1_COAT_NOZZLE_T;
                        break;

                    case (int)EYMC_Device.CENTERING2:
                        axisList[index++] = (int)EYMC_Axis.PUSHPULL_X2;
                        break;

                    case (int)EYMC_Device.S2_ROTATE:
                        axisList[index++] = (int)EYMC_Axis.S2_CHUCK_ROTATE_T;
                        break;

                    case (int)EYMC_Device.S2_CLEAN_NOZZLE:
                        axisList[index++] = (int)EYMC_Axis.S2_CLEAN_NOZZLE_T;
                        break;

                    case (int)EYMC_Device.S2_COAT_NOZZLE:
                        axisList[index++] = (int)EYMC_Axis.S2_COAT_NOZZLE_T;
                        break;

                    case (int)EYMC_Device.UPPER_HANDLER:
                        axisList[index++] = (int)EYMC_Axis.UPPER_HANDLER_X;
                        axisList[index++] = (int)EYMC_Axis.UPPER_HANDLER_Z;
                        break;

                    case (int)EYMC_Device.LOWER_HANDLER:
                        axisList[index++] = (int)EYMC_Axis.LOWER_HANDLER_X;
                        axisList[index++] = (int)EYMC_Axis.LOWER_HANDLER_Z;
                        break;

                    case (int)EYMC_Device.CAMERA1:
                        axisList[index++] = (int)EYMC_Axis.CAMERA1_Z;
                        break;

                    case (int)EYMC_Device.SCANNER1:
                        axisList[index++] = (int)EYMC_Axis.SCANNER1_Z;
                        break;

                    default:
                        axisList[index++] = (int)EYMC_Axis.NULL;
                        break;
                }
            }

            return SUCCESS;
        }

        private int GetDeviceWaitCompletion(int deviceNo, out UInt16[] waits, ushort mode = (ushort)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED)
        {
            int length = GetDeviceLength(deviceNo);
            waits = new ushort[length];
            for (int i = 0; i < waits.Length; i++)
            {
                waits[i] = mode;
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
            for(int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }

                hAxis[i] = m_hAxis[servoNo];
            }

            return SUCCESS;
        }

        public int GetDevice_SpeedData(int deviceNo, out CMotorSpeedData[] speedData, int speedType = (int)EMotorSpeed.MANUAL_SLOW)
        {
            int length = GetDeviceLength(deviceNo);
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            speedData = new CMotorSpeedData[length];
            int boardNo = GetBoardIndex(deviceNo);

            for (int i = 0; i < length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                m_Data.MPBoard[boardNo].GetSpeedData(servoNo, out speedData[i], speedType);
            }

            return SUCCESS;
        }

        public int GetDevice_TimeLimitData(int deviceNo, out CMotorTimeLimitData[] timeLimit)
        {
            int length = GetDeviceLength(deviceNo);
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            timeLimit = new CMotorTimeLimitData[length];
            int boardNo = GetBoardIndex(deviceNo);

            for (int i = 0; i < length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                m_Data.MPBoard[boardNo].GetTimeLimitData(servoNo, out timeLimit[i]);
            }

            return SUCCESS;
        }

        private int GetDevice_MotionData(int deviceNo, out CMPMotionData[] MotionData)
        {
            int length = GetDeviceLength(deviceNo);
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            MotionData = new CMPMotionData[length];
            int boardNo = GetBoardIndex(deviceNo);

            for (int i = 0; i < length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                m_Data.MPBoard[boardNo].GetMotionData(servoNo, out MotionData[i]);
            }

            return SUCCESS;
        }

        private int GetDevice_MotionData(int deviceNo, out CMotionAPI.MOTION_DATA[] MotionData, int speedType = (int)EMotorSpeed.MANUAL_SLOW, CMotorSpeedData[] tempSpeed = null)
        {
            int length = GetDeviceLength(deviceNo);
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            MotionData = new CMotionAPI.MOTION_DATA[length];
            int boardNo = GetBoardIndex(deviceNo);

            for (int i = 0; i < length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                m_Data.MPBoard[boardNo].GetMotionData(servoNo, ref MotionData[i], speedType, tempSpeed?[i]);
            }

            return SUCCESS;
        }

        private int GetDeviceMotionData_Home(int deviceNo, out CMotionAPI.MOTION_DATA[] MotionData,
            out UInt16[] Method, out UInt16[] Dir, out CMotionAPI.POSITION_DATA[] Position)
        {
            int length = GetDeviceLength(deviceNo);
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            MotionData = new CMotionAPI.MOTION_DATA[length];
            int boardNo = GetBoardIndex(deviceNo);
            Method = new UInt16[length];
            Dir = new UInt16[length];
            Position = new CMotionAPI.POSITION_DATA[length];

            for (int i = 0; i < length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                m_Data.MPBoard[boardNo].GetMotion_HomeData(servoNo, ref MotionData[i], out Method[i], out Dir[i], out Position[i]);
            }

            return SUCCESS;
        }

        private int GetDevicePositon(int deviceNo, out CMotionAPI.POSITION_DATA[] Position, double[] pos, 
            ushort type = (ushort)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE)
        {
            int length = GetDeviceLength(deviceNo);
            Position = new CMotionAPI.POSITION_DATA[length];

            for(int i = 0; i < length; i++)
            {
                Position[i].DataType = (ushort)type;
                Position[i].PositionData = (int)(pos[i] * UNIT_REF);
            }

            int boardNo = GetBoardIndex(deviceNo);
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            bool bExceedPlusLimit, bExceedMinusLimit;

            for (int i = 0; i < length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                m_Data.MPBoard[boardNo].CheckSWLimit(servoNo, pos[i], out bExceedPlusLimit, out bExceedMinusLimit);
                if (bExceedPlusLimit == true) return GenerateErrorCode(ERR_YASKAWA_TARGET_POS_EXCEED_PLUS_LIMIT);
                if (bExceedMinusLimit == true) return GenerateErrorCode(ERR_YASKAWA_TARGET_POS_EXCEED_MINUS_LIMIT);
            }

            return SUCCESS;
        }

        private int GetBoardIndex(int servoNo)
        {
            // 우선은 보드 한장 기준으로 작업하자.
            return 0;

//             return servoNo / MP_AXIS_PER_CPU;
        }


        public int OpenController(bool bServoOn = false)
        {
            // 0. init
            int iResult;
            UInt32 rc;
            CMotionAPI.COM_DEVICE ComDevice;

            // 1. Open Controller
            for (int i = 0; i < m_Data.CpuNo; i++)
            {
                // Sets the ymcOpenController parameters.		
                ComDevice.ComDeviceType = (UInt16)CMotionAPI.ApiDefs.COMDEVICETYPE_PCI_MODE;
                ComDevice.PortNumber = (UInt16)m_Data.MPComPort;
                ComDevice.CpuNumber = Convert.ToUInt16(i + 1);    //cpuno;
                ComDevice.NetworkNumber = 0;
                ComDevice.StationNumber = 0;
                ComDevice.UnitNumber = 0;
                ComDevice.IPAddress = "";
                ComDevice.Timeout = APITimeOut;

                rc = CMotionAPI.ymcOpenController(ref ComDevice, ref m_hController[i]);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    LastHWMessage = String.Format($"Error ymcOpenController Board {0} ErrorCode [ 0x{1} ]", i, rc.ToString("X"));
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                    return GenerateErrorCode(ERR_YASKAWA_FAIL_OPEN_YMC);
                }

                // Sets the motion API timeout. 		
                rc = CMotionAPI.ymcSetAPITimeoutValue(30000);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    LastHWMessage = String.Format($"Error ymcSetAPITimeoutValue : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                    return GenerateErrorCode(ERR_YASKAWA_FAIL_SET_TIMEOUT);
                }

                iResult = ChangeController(i);
                if (iResult != SUCCESS) return iResult;

                // Deletes the axis handle that is held by the Machine Controller.
                rc = CMotionAPI.ymcClearAllAxes();
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    LastHWMessage = String.Format($"Error ClearAllAxes  Board : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                    return GenerateErrorCode(ERR_YASKAWA_FAIL_CLEAR_AXIS);
                }

                for (int j = 0; j < MP_AXIS_PER_CPU; j++)
                {
                    if (m_Data.MPBoard[i].SlotLength == 1 && j >= MP_AXIS_PER_PORT) break;
                    if (m_Data.MPBoard[i].MotionData[j].Exist == false) continue;

                    int port = 0;
                    if (j >= MP_AXIS_PER_PORT) port = 1;

                    // Logical ServoNo는 cpu마다 1~32 일까 아니면, 1~32, 33~64, 65~96 순서일까.
                    int logicalAxisNo = i * MP_AXIS_PER_CPU + j;
                    string axisName = m_Data.MPBoard[i].MotionData[j].Name;

                    UInt16 rackNo = (ushort)m_Data.MPBoard[i].SPort[port].RackNo;
                    UInt16 slotNo = (ushort)m_Data.MPBoard[i].SPort[port].SlotNo;
                    UInt16 subSlotNo = (ushort)m_Data.MPBoard[i].SPort[port].SubSlotNo;
                    // create axis handle
                    rc = CMotionAPI.ymcDeclareAxis(rackNo, slotNo, subSlotNo,
                        (UInt16)(j+1), (UInt16)(logicalAxisNo+1), (UInt16)CMotionAPI.ApiDefs.REAL_AXIS, 
                        axisName, ref m_hAxis[logicalAxisNo]);
                    if (rc != CMotionAPI.MP_SUCCESS)
                    {
                        LastHWMessage = String.Format($"Error ymcDeclareAxis Board  : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                        WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                        return GenerateErrorCode(ERR_YASKAWA_FAIL_DECLARE_AXIS);
                    }
                }
            }

            // create device handle
            for (int i = 0; i < (int)EYMC_Device.MAX; i++)
            {
                UInt32[] hAxis;
                iResult = GetDeviceAxisHandle(i, out hAxis);
                if (iResult != SUCCESS) return iResult;

                iResult = DeclareDevice(GetDeviceLength(i), hAxis, ref m_hDevice[i]);
                if (iResult != SUCCESS) return iResult;
            }

            // servo on
            if (bServoOn == true)
            {
                iResult = AllServoOn();
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        private int DeclareDevice(int length, UInt32[] hAxis, ref UInt32 hDevice)
        {
            UInt32 rc = CMotionAPI.ymcDeclareDevice((UInt16)length, hAxis, ref hDevice);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format($"Error ymcDeclareDevice  Board : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_DECLARE_DEVICE);
            }

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
                if (servoNo == (int)EYMC_Axis.NULL || useAxis[i] == false) continue;
                thAxis[j++] = hAxis[i];
            }

            iResult = DeclareDevice(length, thAxis, ref tDevice);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        private int ClearDevice(UInt32 hDevice)
        {
            UInt32 rc = CMotionAPI.ymcClearDevice(hDevice);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format($"Error ymcClearDevice : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_CLEAR_DEVICE);
            }

            return SUCCESS;
        }

        public int CloseController()
        {
            for (int i = 0; i < m_Data.CpuNo; i++)
            {
                if (m_hController[i] == 0) continue;
                uint rc = CMotionAPI.ymcCloseController(m_hController[i]);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    LastHWMessage = String.Format($"Error ymcCloseController Board {0} ErrorCode [ 0x{1} ]", i, rc.ToString("X"));
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                }
            }

            return SUCCESS;
        }

        /// <summary>
        /// thread와 sync의 관계 확인될때까진, class 작성의 한계로 우선은 보드 한장 사용이라고 가정하고
        /// 함수는 작성해놓으나, 실행은  막아놓음. by sjr
        /// 16.06.23 최초 open controller에선 ymcGetController 함수 호출에 문제가 없으나, 나중에 thread에서
        /// 호출할 때는 계속해서 com not opened 란 에러가 발생해서 막아놓음.
        /// </summary>
        /// <param name="cpuIndex"></param>
        /// <returns></returns>
        private int ChangeController(int cpuIndex)
        {
            return SUCCESS;

            if (cpuIndex >= m_hController.Length || m_hController[cpuIndex] == 0)
            {
                return GenerateErrorCode(ERR_YASKAWA_INVALID_CONTROLLER);
            }

            UInt32 hCurrent = 0;
            uint rc = CMotionAPI.ymcGetController(ref hCurrent);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format($"Error ymcGetController Board : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_CHANGE_CONTROLLER);
            }
            if (hCurrent == m_hController[cpuIndex]) return SUCCESS;

            rc = CMotionAPI.ymcSetController(m_hController[cpuIndex]);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format($"Error ymcSetController Board : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_CHANGE_CONTROLLER);
            }

            return SUCCESS;
        }

        private int ChangeControllerByServo(int servoNo)
        {
            int cpuIndex = servoNo / (MP_AXIS_PER_CPU);

            return ChangeController(cpuIndex);
        }

        private void GetAllServoStatus()
        {
            for (int i = 0; i < InstalledAxisNo ; i++)
            {
                GetServoStatus(i);
           }

        }

        /// <summary>
        /// 속도 체크 필요함
        /// </summary>
        /// <param name="servoNo"></param>
        private void GetServoStatus(int servoNo)
        {
#if SIMULATION_MOTION_YMC
            return;
#endif
            UInt32 rc = 0;
            UInt32 returnValue = 0;

            int iResult = ChangeControllerByServo(servoNo);
            //if (iResult != SUCCESS) return iResult;

            ////Servo Position 값 Read 
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                    (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_APOS, ref returnValue); //Machine coordinate system feedback position (APOS)
            if (rc == CMotionAPI.MP_SUCCESS)
            {
                ServoStatus[servoNo].EncoderPos = (double)returnValue / UNIT_REF;
            }

            //Servo 속도값 Read 
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                    (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_FSPD, ref returnValue);
            if (rc == CMotionAPI.MP_SUCCESS)
            {
                ServoStatus[servoNo].Velocity = (double)returnValue / UNIT_REF;
            }

            //Servo Status Read 
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,    //110208
                    (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_RUNSTS, ref returnValue);
            if (rc == CMotionAPI.MP_SUCCESS)
            {
                //Servo Ready
                ServoStatus[servoNo].IsReady = Convert.ToBoolean((returnValue >> 3) & 0x1);
                //Servo On/Off
                ServoStatus[servoNo].IsServoOn = Convert.ToBoolean((returnValue >> 1) & 0x1);
            }

            //Servo Alarm Read 
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,    //110208
                    (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_ALARM, ref returnValue);
            if (rc == CMotionAPI.MP_SUCCESS)
            {
                //Servo Alarm
                ServoStatus[servoNo].IsAlarm = Convert.ToBoolean(returnValue != 0);
            }

            // Servo Command Input Signal
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
            (UInt16)40, ref returnValue);
            if (rc == CMotionAPI.MP_SUCCESS)
            {
                //Servo + Limit Sensor
                ServoStatus[servoNo].DetectPlusSensor = Convert.ToBoolean((returnValue >> 2) & 0x1);
                //Servo - Limit Sensor
                ServoStatus[servoNo].DetectMinusSensor = Convert.ToBoolean((returnValue >> 3) & 0x1);
                //Servo Origin
                ServoStatus[servoNo].DetectHomeSensor = Convert.ToBoolean((returnValue >> 4) & 0x1);
            }

            //UInt16[] reg_IW = new UInt16[1];
            //UInt32 numOfReadData = 0;

            ////============================================================================
            //// Gets the IW Register handle.	
            ////============================================================================
            //rc = CMotionAPI.ymcGetRegisterDataHandle("IW8000", ref g_hRegister_IW);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle IW : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
            //    return;
            //}
            ////Motion Controller 상태 Read
            //rc = CMotionAPI.ymcGetRegisterData(g_hRegister_IW, 1, reg_IW, ref numOfReadData);
            //if (rc == CMotionAPI.MP_SUCCESS)
            //{
            //    if ((reg_IW[0] & 0x01) == 1)
            //        MotionStatus = 1;  //Ready
            //    else
            //        MotionStatus = 0;
            //}


            ////Warning 
            //rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,    //110208
            //     (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_WARNING, ref returnValue);
            //if (rc == CMotionAPI.MP_SUCCESS)
            //{
            //    //Servo - Limit Sensor
            //    ServoStatus[servoNo].Plus = Convert.ToBoolean((returnValue >> 6) & 0x1);
            //    //Servo + Limit Sensor
            //    ServoStatus[servoNo].Minus = Convert.ToBoolean((returnValue >> 7) & 0x1);
            //}

            //rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,    //110208
            //     (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_POSSTS, ref returnValue);
            //if (rc == CMotionAPI.MP_SUCCESS)
            //{
            //    //Servo Origin
            //    ServoStatus[servoNo].Origin = Convert.ToBoolean((returnValue >> 4) & 0x1);
            //}

            ////Servo 상태 Read
            //string registerName = "IW" + (8000 + servoNo * 0x80).ToString();
            //rc = CMotionAPI.ymcGetRegisterDataHandle(registerName, ref g_hRegister_IW);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle IW : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
            //    return;
            //}
            //rc = CMotionAPI.ymcGetRegisterData(g_hRegister_IW, 1, reg_IW, ref numOfReadData);
            //if (rc == CMotionAPI.MP_SUCCESS)
            //{
            //    ushort servoValue = reg_IW[0];
            //    ////Servo Ready
            //    //ServoStatus[servoNo].Ready = Convert.ToBoolean((servoValue >> 2) & 0x1);
            //    ////Servo Alarm
            //    //ServoStatus[servoNo].Alarm = Convert.ToBoolean((servoValue >> 0) & 0x1);
            //    ////Servo - Limit Sensor
            //    //ServoStatus[servoNo].Minus = Convert.ToBoolean((servoValue >> 13) & 0x1);
            //    ////Servo + Limit Sensor
            //    //ServoStatus[servoNo].Plus = Convert.ToBoolean((servoValue >> 12) & 0x1);
            //    ////Servo Origin
            //    //ServoStatus[servoNo].Origin = Convert.ToBoolean((servoValue >> 6) & 0x1);
            //    ////Servo On/Off
            //    //ServoStatus[servoNo].ServoOn = Convert.ToBoolean((servoValue >> 3) & 0x1);
            // }

        }

        public int ServoOn(int deviceNo)
        {
            int iResult = ResetAlarm(deviceNo);
            if (iResult != SUCCESS) return iResult;

            UInt32 rc = CMotionAPI.ymcServoControl(m_hDevice[deviceNo], (UInt16)CMotionAPI.ApiDefs.SERVO_ON, APITimeOut);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format($"Error ymcServoControl ServoOn : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_ON);
            }

            return SUCCESS;
        }

        public int AllServoOn()
        {
            int iResult = ServoOn((int)EYMC_Device.ALL);
            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }

        public int AllServoOff()
        {
            int iResult = ServoOff((int)EYMC_Device.ALL);
            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }

        public int ServoOff(int deviceNo)
        {
            if (deviceNo == (int)EYMC_Device.NULL) return SUCCESS; // return success if device is null

            UInt32 rc = CMotionAPI.ymcServoControl(m_hDevice[deviceNo], (UInt16)CMotionAPI.ApiDefs.SERVO_OFF, APITimeOut);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format($"Error ymcServoControl ServoOff : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_ON);
            }

            return SUCCESS;
        }

        public int ResetAlarm(int deviceNo = (int)EYMC_Device.ALL)
        {
            if (deviceNo == (int)EYMC_Device.NULL) return SUCCESS; // return success if device is null

            UInt32 rc;

            // Clears all the Machine Controller alarms. 
            rc = CMotionAPI.ymcClearAlarm(0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcClearAlarm : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                LastHWMessage = String.Format($"Error ymcClearAlarm : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_RESET_ALARM);
            }

            // check warning
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            for(int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }

                if (IsServoWarning(servoNo) == true)
                {
                    ServoOff(servoNo);
                }

                rc = CMotionAPI.ymcClearServoAlarm(m_hAxis[servoNo]);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    //MessageBox.Show(String.Format("Error ymcClearServoAlarm : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                    LastHWMessage = String.Format($"Error ymcClearServoAlarm : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                    return GenerateErrorCode(ERR_YASKAWA_FAIL_RESET_ALARM);
                }
            }

            return SUCCESS;
        }

        public int CheckHomeComplete(int servoNo, out bool bComplete)
        {
            return CheckMoveComplete(servoNo, out bComplete);
        }

        public int CheckMoveComplete(int servoNo, out bool bComplete)
        {
            UInt32 returnValue = 0;
            bComplete = false;

            UInt32 rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo],
                                                              (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                                                              (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_POSSTS,
                                                              ref returnValue);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetMotionParameterValue : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                LastHWMessage = String.Format($"Error ymcGetMotionParameterValue : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_MOTION_PARAM);
            }

            //Move 완료 확인 
            if (((returnValue >> (int)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED) & 0x1) == 1)
                bComplete = true;
            else bComplete = false;

            return SUCCESS;
        }

        public int CheckMoveComplete(int[] axisList, out bool[] bComplete)
        {
            bComplete = new bool[axisList.Length];
            for(int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
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
            CMPMotionData[] motionData;

            for (int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if(servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
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
            int iResult = ServoMotionStop((int)EYMC_Device.ALL);
            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }

        public int JogMoveStop(int deviceNo)
        {
            if (deviceNo == (int)EYMC_Device.NULL) return SUCCESS; // return success if device is null

            ushort[] WaitForCompletion;
            GetDeviceWaitCompletion(deviceNo, out WaitForCompletion);
            UInt32 rc = CMotionAPI.ymcStopJOG(m_hDevice[deviceNo], 0, "STOP", WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcStopJOG : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                LastHWMessage = String.Format($"Error ymcStopJOG : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);

                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_STOP);
            }

            return SUCCESS;
        }

        public int JogMoveStart(int deviceNo, bool jogDir, bool bJogFastMove = false)
        {
            if (deviceNo == (int)EYMC_Device.NULL) return SUCCESS; // return success if device is null

            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;

            // Jog함수는 multi axis device를 고려하지 않고 작성
            if (deviceNo >= (int)EYMC_Device.ALL) return SUCCESS;

            //============================================================================
            // Executes JOG operation.										
            //============================================================================
            // Motion data setting
            Int16[] Direction = new Int16[1];
            UInt16[] TimeOut = new UInt16[1] { APIJogTime };

            if (jogDir == JOG_DIR_POS)
            {
                //Jog +
                if (ServoStatus[deviceNo].IsReady && ServoStatus[deviceNo].IsServoOn)
                {
                    if (ServoStatus[deviceNo].DetectPlusSensor)
                    {
                        LastHWMessage = "Servo No[" + deviceNo + "] : + Limit";
                        WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Warning, true);
                        return GenerateErrorCode(ERR_YASKAWA_DETECTED_PLUS_LIMIT);
                    }
                    else
                    {
                        Direction[0] = (Int16)CMotionAPI.ApiDefs.DIRECTION_POSITIVE;
                    }
                }
            }
            else
            {
                //Jog -
                if (ServoStatus[deviceNo].IsReady && ServoStatus[deviceNo].IsServoOn)
                {
                    if (ServoStatus[deviceNo].DetectMinusSensor)
                    {
                        LastHWMessage = "Servo No[" + deviceNo + "] : - Limit";
                        WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Warning, true);
                        return GenerateErrorCode(ERR_YASKAWA_DETECTED_MINUS_LIMIT);
                    }
                    else
                    {
                        Direction[0] = (Int16)CMotionAPI.ApiDefs.DIRECTION_NEGATIVE;
                    }
                }
            }

            ushort TimeOut1 = 0;
            CMotionAPI.MOTION_DATA[] MotionData;
            int speedType = (bJogFastMove == true) ? (int)EMotorSpeed.JOG_FAST : (int)EMotorSpeed.JOG_SLOW;
            GetDevice_MotionData(deviceNo, out MotionData, speedType);
            UInt32 rc = CMotionAPI.ymcMoveJOG(m_hDevice[deviceNo], MotionData, Direction, TimeOut, 0, "JOG", 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format($"Error ymcMoveJOG : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_MOVE_JOG);
            }

            return SUCCESS;
        }

        public int ServoMotionStop(int deviceNo, ushort mode = (ushort)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED)
        {
            if (deviceNo == (int)EYMC_Device.NULL) return SUCCESS; // return success if device is null

            CMotionAPI.MOTION_DATA[] MotionData;
            GetDevice_MotionData(deviceNo, out MotionData);
            ushort[] WaitForCompletion;
            GetDeviceWaitCompletion(deviceNo, out WaitForCompletion, mode);

            UInt32 rc = CMotionAPI.ymcStopMotion(m_hDevice[deviceNo], MotionData, "STOP", WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcStopMotion : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                LastHWMessage = String.Format($"Error ymcStopMotion : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_STOP);
            }

            return SUCCESS;
        }

        public int StartMoveToPos(int deviceNo, double[] pos, CMotorSpeedData[] tempSpeed = null, ushort waitMode = (ushort)CMotionAPI.ApiDefs.COMMAND_STARTED)
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
        public int MoveToPos(int deviceNo, double[] pos, CMotorSpeedData[] tempSpeed = null, ushort waitMode = (ushort)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED)
        {
            if (deviceNo == (int)EYMC_Device.NULL) return SUCCESS; // return success if device is null

            // 0. init data
            CMotionAPI.POSITION_DATA[] PositionData;
            CMotionAPI.MOTION_DATA[] MotionData;
            ushort[] WaitForCompletion;
            int iResult = GetDevicePositon(deviceNo, out PositionData, pos, (UInt16)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE);
            if (iResult != SUCCESS) return iResult;
            GetDevice_MotionData(deviceNo, out MotionData, SpeedType, tempSpeed);
            GetDeviceWaitCompletion(deviceNo, out WaitForCompletion, waitMode);

            // 0.8 check axis state for move
            int[] axisList;
            iResult = GetDeviceAxisList(deviceNo, out axisList);
            if (iResult != SUCCESS) return iResult;
            iResult = CheckAxisStateForMove(axisList);
            if (iResult != SUCCESS) return iResult;

            // 1. call api
            // ymcMovePositioning 함수는 motion controller 가 부하를 담당하고, ymcMoveDriverPositioning는 driver가 부하를 담당
            UInt32 rc = CMotionAPI.ymcMoveDriverPositioning(m_hDevice[deviceNo], MotionData, PositionData, 0, "Move", WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcMoveDriverPositioning ErrorCode [ 0x{0} ]",rc.ToString("X")));
                LastHWMessage = String.Format($"Error ymcMoveDriverPositioning : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_MOVE_DRIVING_POSITIONING);
            }

            return SUCCESS;
        }

        public int StartMoveToPos(int[] axisList, bool[] useAxis, double[] pos, CMotorSpeedData[] tempSpeed = null, ushort waitMode = (ushort)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED)
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
        public int MoveToPos(int[] axisList, bool[] useAxis, double[] pos, CMotorSpeedData[] tempSpeed = null, ushort waitMode = (ushort)CMotionAPI.ApiDefs.DISTRIBUTION_COMPLETED)
        {
            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;

            // 0. init data
            // 0.1 get device length
            int length = 0;
            for(int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL || useAxis[i] == false) continue;
                length++;
            }
            if (length == 0)
            {
                return GenerateErrorCode(ERR_YASKAWA_SELECTED_AXIS_NONE);
            }

            // 0.2 allocate temp device
            UInt32 tDevice = 0;
            iResult = DeclareTempDevice(length, axisList, useAxis, ref tDevice);
            if (iResult != SUCCESS) return iResult;

            // 0.3 allocate data buffer
            CMotionAPI.MOTION_DATA[] MotionData = new CMotionAPI.MOTION_DATA[length];
            CMotionAPI.POSITION_DATA[] PositionData = new CMotionAPI.POSITION_DATA[length];
            ushort[] WaitForCompletion = new ushort[length];
            int[] tAxisList = new int[length];

            // 0.4 copy motion data to buffer
            CMotionAPI.MOTION_DATA[] tMotion;
            CMotionAPI.POSITION_DATA[] tPosition;
            ushort[] tWait;
            CMotorSpeedData[] tSpeed = new CMotorSpeedData[1];

            for (int i = 0, j = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL || useAxis[i] == false) continue;
                if (tempSpeed != null)
                {
                    tSpeed[0] = tempSpeed[i];
                }
                GetDevice_MotionData(servoNo, out tMotion, SpeedType, (tempSpeed != null) ? tSpeed : null);
                iResult = GetDevicePositon(servoNo, out tPosition, pos, (UInt16)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE);
                if (iResult != SUCCESS) return iResult;
                GetDeviceWaitCompletion(servoNo, out tWait, waitMode);

                MotionData[j] = tMotion[0];
                PositionData[j] = tPosition[0];
                WaitForCompletion[j] = tWait[0];
                tAxisList[j] = servoNo;
                j++;
            }

            // 0.8 check axis state for move
            iResult = CheckAxisStateForMove(tAxisList);
            if (iResult != SUCCESS) return iResult;

            // 1. call api
            // ymcMovePositioning 함수는 motion controller 가 부하를 담당하고, ymcMoveDriverPositioning는 driver가 부하를 담당
            UInt32 rc = CMotionAPI.ymcMoveDriverPositioning(tDevice, MotionData, PositionData, 0, "Move", WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcMoveDriverPositioning ErrorCode [ 0x{0} ]",rc.ToString("X")));
                LastHWMessage = String.Format($"Error ymcMoveDriverPositioning : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_MOVE_DRIVING_POSITIONING);
            }

            // 2. clear device
            iResult = ClearDevice(tDevice);
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
                if (servoNo == (int)EYMC_Axis.NULL || useAxis[i] == false) continue;
                length++;
            }
            if (length == 0)
            {
                return GenerateErrorCode(ERR_YASKAWA_SELECTED_AXIS_NONE);
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
                if (servoNo == (int)EYMC_Axis.NULL || useAxis[i] == false) continue;
                GetDevice_TimeLimitData(servoNo, out tLimit);

                timeLimit[j] = tLimit[0];
                bDone[j] = false;
                servoList[j] = servoNo;
                j++;
            }

            // 1. wait for done
            int sum = 0;
            m_waitTimer.StartTimer();
            while(true)
            {
                // 1.1 check safety
                iResult = IsSafeForMove(true);
                if (iResult != SUCCESS) return iResult;

                // 1.2 check all done
                if(sum == length)
                {
                    m_waitTimer.StopTimer();
                    return SUCCESS;
                }

                // 1.3 check each axis
                for (int i = 0; i < length; i++)
                {
                    if (bDone[i] == true) continue;

                    if(bWait4Home == false) // wait for move done
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
                            return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_MOVE_IN_LIMIT_TIME);
                        }
                    } else // wait for home done
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
                            return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_HOME_IN_LIMIT_TIME);
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

            // 0. init data
            // 0.1 get device length
            int length = 0;
            for (int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL || useAxis[i] == false) continue;
                length++;
            }
            if (length == 0)
            {
                return GenerateErrorCode(ERR_YASKAWA_SELECTED_AXIS_NONE);
            }


            // 0.2 allocate temp device
            UInt32 tDevice = 0;
            iResult = DeclareTempDevice(length, axisList, useAxis, ref tDevice);
            if (iResult != SUCCESS) return iResult;

            // 0.3 allocate data buffer
            CMotionAPI.MOTION_DATA[] MotionData = new CMotionAPI.MOTION_DATA[length];
            CMotionAPI.POSITION_DATA[] PositionData = new CMotionAPI.POSITION_DATA[length];
            ushort[] WaitForCompletion = new ushort[length];
            ushort[] Method = new ushort[length];
            ushort[] Dir = new ushort[length];

            // 0.4 copy motion data to buffer
            CMotionAPI.MOTION_DATA[] tMotion;
            CMotionAPI.POSITION_DATA[] tPosition;
            ushort[] tWait;
            ushort[] tMethod;
            ushort[] tDir;

            for (int i = 0, j = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL || useAxis[i] == false) continue;

                GetDeviceMotionData_Home(servoNo, out tMotion, out tMethod, out tDir, out tPosition);
                GetDeviceWaitCompletion(servoNo, out tWait, (UInt16)CMotionAPI.ApiDefs.COMMAND_STARTED);

                MotionData[j] = tMotion[0];
                PositionData[j] = tPosition[0];
                WaitForCompletion[j] = tWait[0];
                Method[j] = tMethod[0];
                Dir[j] = tDir[0];
                j++;
            }

            // 1. call api
            UInt32 rc = CMotionAPI.ymcMoveHomePosition(tDevice, MotionData, PositionData, Method, Dir, 0, null, WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcMoveHomePositioning : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                LastHWMessage = String.Format($"Error ymcMoveHomePositioning : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_MOVE_HOME);
            }

            // 2. clear device
            iResult = ClearDevice(tDevice);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public bool IsOriginReturned(int servoNo)
        {
            return ServoStatus[servoNo].IsOriginReturned;
        }

        public void SetOriginFlag(int deviceNo, bool bFlag)
        {
            int[] axisList;
            GetDeviceAxisList(deviceNo, out axisList);
            for (int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EYMC_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                ServoStatus[servoNo].IsOriginReturned = bFlag;
            }
        }

        public int OriginReturn(int deviceNo = (int)EYMC_Device.ALL)
        {
            if (deviceNo == (int)EYMC_Device.NULL) return SUCCESS; // return success if device is null

            // check safety
            int iResult = IsSafeForMove();
            if (iResult != SUCCESS) return iResult;

            CMotionAPI.MOTION_DATA[] MotionData;
            CMotionAPI.POSITION_DATA[] PositionData;
            ushort[] WaitForCompletion;
            ushort[] Method;
            ushort[] Dir;
            GetDeviceMotionData_Home(deviceNo, out MotionData, out Method, out Dir, out PositionData);
            GetDeviceWaitCompletion(deviceNo, out WaitForCompletion, (UInt16)CMotionAPI.ApiDefs.COMMAND_STARTED);

            // reset origin flag
            SetOriginFlag(deviceNo, false);

            // home return
            UInt32 rc = CMotionAPI.ymcMoveHomePosition(m_hDevice[deviceNo], MotionData, PositionData, Method, Dir, 0, null, WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcMoveHomePositioning : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                LastHWMessage = String.Format($"Error ymcMoveHomePositioning : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_MOVE_HOME);
            }

            // set origin flag
            SetOriginFlag(deviceNo, true);

            return SUCCESS;
        }

        public int GetServoPos(int servoNo, out double pos)
        {
            // this is obsolete because servo status is updated by thread
            pos = 0;
            return GenerateErrorCode(ERR_YASKAWA_OBSOLETE_FUNCTION);

            UInt32 servoPosi = 0;
            UInt32 rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                 (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_APOS, ref servoPosi);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format($"Error ymcGetMotionParameterValue : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_GET_POS);
            }
            pos = servoPosi / UNIT_REF;

            return SUCCESS;
        }

        public bool IsServoWarning(int servoNo)
        {

            UInt32 returnValue = 0;

            UInt32 rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo],
                                                          (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                                                          (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_WARNING,
                                                          ref returnValue);
            return Convert.ToBoolean((returnValue >> 8) & 0x1);
        }
        /*
        public void ManualPacket()
        {
            UInt32 rc;
            UInt32 hRegister_ML;                     // Register data handle for ML register
            UInt32 hRegister_ML_Read;                     // Register data handle for ML register
            UInt32 hRegister_MB;                     // Register data handle for MB register

            Int32[] Reg_LongData = new Int32[59];      // L size register data storage variable ydh150717
            //UInt16[] Reg_ShortData = new UInt16[4];    // W or B size register data storage variable
            short Reg_ShortData = 0;                     // W or B size register data storage variable
            UInt32 Reg_IntData = 0;//ydh150803
            //UInt32 Reg_IntData_1214 = 0;
            UInt32[] Reg_IntData_1214 = new UInt32[5];

            //UInt32 Reg_IntData_1300 = 0;
            Int32[] Reg_LongData_1300 = new Int32[54];
            String cRegisterName_MB;                 // MB register name storage variable
            String cRegisterName_ML;                 // ML register name storage variable
            String cRegisterName_ML_1214;                 // ML register name storage variable
            String cRegisterName_ML_1300;                 // ML register name storage variable
            UInt32 RegisterDataNumber;               // Number of read-in registers
            UInt32 RegisterDataNumber_b;
            UInt32 RegisterDataNumber_1214;               // Number of read-in registers
            UInt32 RegisterDataNumber_1300;               // Number of read-in registers
            UInt32 ReadDataNumber;                   // Number of obtained registers
            UInt32 ReadDataNumber_b;                   // Number of obtained registers
            hRegister_ML = 0x00000000;
            hRegister_ML_Read = 0x00000000;
            hRegister_MB = 0x00000000;
            ReadDataNumber = 00000000;
            ReadDataNumber_b = 00000000;
            RegisterDataNumber_1214 = 0;
            RegisterDataNumber_1300 = 0;

            // MB Register
            cRegisterName_ML = "ML01000";   //ML Register 주소는 짝수로 설정해야함. 홀수일때 핸들에러 발생
            cRegisterName_MB = "MB000000";

            cRegisterName_ML_1214 = "ML01214";   //ydh150803 ML Register 주소는 짝수로 설정해야함. 홀수일때 핸들에러 발생
            cRegisterName_ML_1300 = "ML01300";
#region MB 영역 read/write
            // MB Register 핸들
            rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_MB, ref hRegister_MB);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle  MB : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE);
                return;
            }

            // MB Register 읽기
            RegisterDataNumber_b = 1;
            rc = CMotionAPI.ymcGetRegisterData(hRegister_MB, RegisterDataNumber_b, ref Reg_ShortData, ref ReadDataNumber_b);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterData MB : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SET_REGISTER_DATA);
                return;
            }

            if (Convert.ToBoolean(Reg_ShortData))
                Reg_ShortData = 0;
            else
                Reg_ShortData = 1;

            // MB Register 쓰기 (HeartBit 용 PC <-> MP 간)
            RegisterDataNumber_b = 1;
            rc = CMotionAPI.ymcSetRegisterData(hRegister_MB, RegisterDataNumber_b, ref Reg_ShortData);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcSetRegisterData MB : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SET_REGISTER_DATA_FAIL);
                return;
            }
#endregion

            //RegisterDataNumber = 0;

#region ML 영역 read/write (ML1000)
            // ML Register 전송용 핸들
            rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_ML, ref hRegister_ML);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle ML : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE_FAIL);
                return;
            }

            // ML Register 수신data
            //rc = CMotionAPI.ymcGetRegisterData(hRegister_ML, RegisterDataNumber, ref Reg_IntData, ref ReadDataNumber);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterData ML : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
            //    return;
            //}

            //YDH150803 ML1214를 읽어 보여주자(GO CMD를 받은 시점부터의 이동거리 값을 읽어온다.MF1212 = MF1212 + IL8040(SER_FSPD)
            UInt32 GoCmdFromStartPos;
            GoCmdFromStartPos = Reg_IntData;

            // mode
            Reg_LongData[0] = (OHTInform.Inform.Mode);//ML1000

            // status
            Reg_LongData[1] = (OHTInform.OHTStatus);//ML1002

            //reset
            Reg_LongData[2] = (OHTInform.DrivingInform.Reset);//ML1004

            //비상정지
            Reg_LongData[3] = (UtilityClass.ValueToInt(OHTAlarm.EMFlag));//ML1006

            //정방향(0), 역방향 (1)
            Reg_LongData[4] = (OHTInform.DrivingInform.MoveDirection);//ML1008

            //command speed
            if (OHTInform.Inform.IsCleanVehicle)
            {
                // 청소차량 속도제한 설정
                if (OHTInform.DrivingInform.Velocity > CLEANVEHICLE_SPEED_LIMIT)
                {
                    Reg_LongData[5] = CLEANVEHICLE_SPEED_LIMIT;
                }
                else
                {
                    Reg_LongData[5] = (OHTInform.DrivingInform.Velocity);//ML1010
                }
            }
            else
            {
                Reg_LongData[5] = (OHTInform.DrivingInform.Velocity);//ML1010
            }

            // accel
            Reg_LongData[6] = (OHTInform.DrivingInform.Acceleration) * UNIT_REF;//ML1012

            //khc 151120 : 대인 감지할 경우 감속도 변경, 직진에서는 2단계만 감지해도 정지 수준으로 변경해야 함.
            if ((OHTDetect.DetectDir == (int)DetectDirection.STRAIGHT) && ((OHTInOut.X[DEF_IO.X_OBSTACLE_DETECT_STOP] == DEF_BIT.OFF) || TOHSMain.EStopFlag))
            {
                OHTInform.DrivingInform.Deceleration = 3000;
            }
            else if ((OHTDetect.DetectDir == (int)DetectDirection.STRAIGHT) && (OHTInOut.X[DEF_IO.X_OBSTACLE_DETECT_WARNING] == DEF_BIT.OFF))
            {
                OHTInform.DrivingInform.Deceleration = 2000;
            }
            else
            {
                OHTInform.DrivingInform.Deceleration = (int)ServoInform[(int)ServoList.DRIVING1].Deceleration;
            }
            // decel
            Reg_LongData[7] = (OHTInform.DrivingInform.Deceleration) * UNIT_REF;//ML1014

            // curve(1) / 직선(2)
            //int _tmp = (OHTInform.PositionFlag.SetPlcCurveMode) ? 1 : 0;   // 0 : 직선, 1: 90 도 커브, 2: 180 도 커브, 3: N Curve(90도 연속커브)
            Reg_LongData[8] = OHTInform.DrivingInform.CurCurveType;//ML1016

            //khc 151126 : auto 에서는 무조건 0을 써주자.
            if (OHTInform.Inform.Mode == (int)OHTInform.ModeList.AUTO) OHTInform.DrivingInform.DrivingAxis = 0;
            //Driving axis
            Reg_LongData[9] = (OHTInform.DrivingInform.DrivingAxis);////ML1018



            // 거리 reset request
            Reg_LongData[10] = (UtilityClass.ValueToInt(OHTInform.PositionFlag.PLCSyncReqFlag));//ML1020

            // 설정된 Creep speed
            Reg_LongData[11] = OHTInform.RunningSpeed.CreepSpeed;  //ML1022

            //Reg_LongData[12] = 5;

            //RegisterDataNumber = 13;

            for (int i = 12; i < 20; i++)
            {
                Reg_LongData[i] = 0;
            }
            // Distance to Goal
            Reg_LongData[20] = (OHTInform.PositionValue.FromStartToGoal);//ML1040


            // Distance to Juncture 1
            //Reg_LongData[21] = (OHTInform.FromStartToJunction[0].Dist);//ML1042
            //// Distance to Juncture 2
            //Reg_LongData[22] = (OHTInform.FromStartToJunction[1].Dist);//ML1044
            //// Distance to Juncture 3
            //Reg_LongData[23] = (OHTInform.FromStartToJunction[2].Dist);//ML1046
            //// Distance to Juncture 4
            //Reg_LongData[24] = (OHTInform.FromStartToJunction[3].Dist);//ML1048
            //// Distance to Curve 1
            //Reg_LongData[25] = (OHTInform.FromStartToCurve[0].Dist);//ML1050
            //// Distance to Curve 2
            //Reg_LongData[26] = (OHTInform.FromStartToCurve[1].Dist);//ML1052
            //// Distance to Curve 3
            //Reg_LongData[27] = (OHTInform.FromStartToCurve[2].Dist);//ML1054

            Reg_LongData[21] = (OHTInform.FromStartToCurve[0].Dist);//ML1042
            // Distance to Juncture 2
            Reg_LongData[22] = (OHTInform.FromStartToCurve[1].Dist);//ML1044
            // Distance to Juncture 3
            Reg_LongData[23] = (OHTInform.FromStartToCurve[2].Dist);//ML1046
            // Distance to Juncture 4
            Reg_LongData[24] = (OHTInform.FromStartToJunction[0].Dist);//ML1048
            // Distance to Curve 1
            Reg_LongData[25] = (OHTInform.FromStartToJunction[1].Dist);//ML1050
            // Distance to Curve 2
            Reg_LongData[26] = (OHTInform.FromStartToJunction[2].Dist);//ML1052
            // Distance to Curve 3
            Reg_LongData[27] = (OHTInform.FromStartToJunction[3].Dist);//ML1054

            //Reg_LongData[28] = (OHTInform.PositionValue.FromStartToCurve[3]);//ML1056

            // Speed For Stop ( 커브 감속 )
            Reg_LongData[30] = (VEHICLE_DEST_BCD_SEARCH_VELOCITY);//ML1060
            //// Speed For Juncture 1
            //Reg_LongData[31] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1062
            //// Speed For Juncture 2
            //Reg_LongData[32] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1064
            //// Speed For Juncture 3
            //Reg_LongData[33] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1066
            //// Speed For Juncture 4
            //Reg_LongData[34] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1068
            //// Speed For Curve 1
            ////Reg_LongData[35] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1070
            //Reg_LongData[35] = (OHTInform.RunningSpeed.CurveRunningSpeed);
            //// Speed For Curve 2
            ////Reg_LongData[36] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1072
            //Reg_LongData[36] = (OHTInform.RunningSpeed.CurveRunningSpeed);
            //// Speed For Curve 3
            ////Reg_LongData[37] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1074
            //Reg_LongData[37] = (OHTInform.RunningSpeed.CurveRunningSpeed);

            // Speed For Juncture 1
            Reg_LongData[31] = (OHTInform.RunningSpeed.CurveRunningSpeed);//ML1062
            // Speed For Juncture 2
            Reg_LongData[32] = (OHTInform.RunningSpeed.CurveRunningSpeed);//ML1064
            // Speed For Juncture 3
            Reg_LongData[33] = (OHTInform.RunningSpeed.CurveRunningSpeed);//ML1066
            // Speed For Juncture 4
            Reg_LongData[34] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1068
            // Speed For Curve 1
            //Reg_LongData[35] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1070
            Reg_LongData[35] = (OHTInform.RunningSpeed.JunctionRunningSpeed);
            // Speed For Curve 2
            //Reg_LongData[36] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1072
            Reg_LongData[36] = (OHTInform.RunningSpeed.JunctionRunningSpeed);
            // Speed For Curve 3
            //Reg_LongData[37] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1074
            Reg_LongData[37] = (OHTInform.RunningSpeed.JunctionRunningSpeed);

            // offset 거리
            Reg_LongData[50] = (OHTInform.OnPosiOffsetDist);//ML1100

            // 커브 돌때 속도
            Reg_LongData[51] = (OHTInform.RunningSpeed.CurveRunningSpeed);//ML1102

            // 대차감지속도1(감속시작)
            Reg_LongData[52] = (OHTDetect.DetectVelocity[0]);//ML1104

            // 대차감지속도2
            Reg_LongData[53] = (OHTDetect.DetectVelocity[1]);//ML1106

            // 대차감지속도3
            Reg_LongData[54] = (OHTDetect.DetectVelocity[2]);//ML1108

            // 대차감지속도4
            Reg_LongData[55] = (OHTDetect.DetectVelocity[3]);//ML1110

            // 대차감지속도5
            Reg_LongData[56] = (OHTDetect.DetectVelocity[4]);//ML1112

            // 대차감지속도(정지)
            Reg_LongData[57] = (0);//ML1114

            // 대인감지속도(감속시작)
            //Reg_LongData[58] = (OHTObstacle.DetectVelocity[1]);//ML1116
            Reg_LongData[58] = (0);//ML1116

            RegisterDataNumber = 59;

            rc = CMotionAPI.ymcSetRegisterData(hRegister_ML, RegisterDataNumber, ref Reg_LongData[0]);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcSetRegisterData ML : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SET_REGISTER_DATA_FAIL);
                return;
            }
#endregion

#region ML 영역 read/write (ML 1210)
            // ML Register 전송용 핸들
            rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_ML_1214, ref hRegister_ML_Read);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle ML : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE_FAIL);
                return;
            }
            RegisterDataNumber_1214 = 5;
            // ML Register 수신data
            rc = CMotionAPI.ymcGetRegisterData(hRegister_ML_Read, RegisterDataNumber_1214, ref Reg_IntData_1214[0], ref RegisterDataNumber_1214);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                // MessageBox.Show(String.Format("Error ymcGetRegisterData ML : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_FAIL);
                return;
            }

            //YDH150803 ML1214를 읽어 보여주자(GO CMD를 받은 시점부터의 이동거리 값을 읽어온다.MF1212 = MF1212 + IL8040(SER_FSPD)
            //UInt32 GoCmdFromStartPos;
            OHTInform.FromStartDist = (int)Reg_IntData_1214[0];  // ML1214

            // ML1216
            OHTInform.PlcSetCurve = (int)Reg_IntData_1214[1];
            // ML1218
            //= (int)Reg_IntData_1214[2]
            // ML1220
            //= (int)Reg_IntData_1214[3]
            // ML1222
            OHTInform.OverShootDist = (int)Reg_IntData_1214[4];  // ML1222


#endregion

#region ML 영역 read/write (ML 1300)
            // ML Register 전송용 핸들
            rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_ML_1300, ref hRegister_ML_Read);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle ML : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE_FAIL);
                return;
            }
            RegisterDataNumber_1300 = 53;
            // ML Register 수신data
            rc = CMotionAPI.ymcGetRegisterData(hRegister_ML_Read, RegisterDataNumber_1300, ref Reg_LongData_1300[0], ref RegisterDataNumber_1300);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterData ML : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_FAIL);
                return;
            }

            //YDH150803 ML1214를 읽어 보여주자(GO CMD를 받은 시점부터의 이동거리 값을 읽어온다.MF1212 = MF1212 + IL8040(SER_FSPD)
            //UInt32 GoCmdFromStartPos;
            //xxx = (int)Reg_LongData_1300[0];
            OHTInform.FdbSpeed = (int)Reg_LongData_1300[1];
            //  .... xxx = (int)Reg_LongData_1300[53];

            //khc 각 축별로 부하율
            ServoStatus[0].LoadFactor = (int)Reg_LongData_1300[2];
            ServoStatus[1].LoadFactor =
                (int)Reg_LongData_1300[17];
            ServoStatus[2].LoadFactor = (int)Reg_LongData_1300[32];
            ServoStatus[3].LoadFactor = (int)Reg_LongData_1300[47];

            ServoStatus[0].AlarmCode = (int)Reg_LongData_1300[7];  //ml1314
            ServoStatus[1].AlarmCode = (int)Reg_LongData_1300[22];  //ml1344

            if (Math.Abs(ServoStatus[0].LoadFactor) >= 250)
            {
                MainForm.LOG(LOGType.Warning, "(Front LoadFactor) BCD No : " + OHTBarcode.CurCtrlBcd.BCDNo.ToString() + " >>>> " + ServoStatus[0].LoadFactor.ToString() + OHTInform.LogggingDistanceInfo());
                MainForm.LOG(LOGType.Warning, "(Rear LoadFactor) BCD No : " + OHTBarcode.CurCtrlBcd.BCDNo.ToString() + " >>>> " + ServoStatus[1].LoadFactor.ToString() + OHTInform.LogggingDistanceInfo());
            }
            else if (Math.Abs(ServoStatus[1].LoadFactor) >= 200)
            {
                MainForm.LOG(LOGType.Warning, "(Front LoadFactor) BCD No : " + OHTBarcode.CurCtrlBcd.BCDNo.ToString() + " >>>> " + ServoStatus[0].LoadFactor.ToString() + OHTInform.LogggingDistanceInfo());
                MainForm.LOG(LOGType.Warning, "(Rear LoadFactor) BCD No : " + OHTBarcode.CurCtrlBcd.BCDNo.ToString() + " >>>> " + ServoStatus[1].LoadFactor.ToString() + OHTInform.LogggingDistanceInfo());
            }

            if ((ServoStatus[0].AlarmCode > 0 && (int)Reg_LongData_1300[6] > 0) && !TOHSMain.ServoAlarmCodeWriteLogFlag[0])
            {
                TOHSMain.ServoAlarmCodeWriteLogFlag[0] = true;
                MainForm.LOG(LOGType.Warning, "Front Servo Alarm Code :: " + ServoStatus[0].AlarmCode.ToString());
            }
            if ((ServoStatus[1].AlarmCode > 0 && (int)Reg_LongData_1300[21] > 0) && !TOHSMain.ServoAlarmCodeWriteLogFlag[1])
            {
                TOHSMain.ServoAlarmCodeWriteLogFlag[1] = true;
                MainForm.LOG(LOGType.Warning, "Rear Servo Alarm Code :: " + ServoStatus[1].AlarmCode.ToString());
            }
#endregion

            //UInt32 rc;
            //UInt32 hRegister_ML;                     // Register data handle for ML register
            //UInt32 hRegister_MB;                     // Register data handle for MB register

            //Int32[] Reg_LongData = new Int32[10];      // L size register data storage variable
            ////UInt16[] Reg_ShortData = new UInt16[4];    // W or B size register data storage variable
            //short Reg_ShortData = 0;                     // W or B size register data storage variable
            //String cRegisterName_MB;                 // MB register name storage variable
            //String cRegisterName_ML;                 // ML register name storage variable
            //UInt32 RegisterDataNumber;               // Number of read-in registers
            //UInt32 ReadDataNumber;                   // Number of obtained registers
            //hRegister_ML = 0x00000000;
            //hRegister_MB = 0x00000000;
            //ReadDataNumber = 00000000;

            //// MB Register
            //cRegisterName_ML = "ML01000";   //ML Register 주소는 짝수로 설정해야함. 홀수일때 핸들에러 발생
            //cRegisterName_MB = "MB000000";

            //// MB Register 핸들
            //rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_MB, ref hRegister_MB);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle  MB : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
            //    return;
            //}

            //// MB Register 읽기
            //RegisterDataNumber = 1;
            //rc = CMotionAPI.ymcGetRegisterData(hRegister_MB, RegisterDataNumber, ref Reg_ShortData, ref ReadDataNumber);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterData MB : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
            //    return;
            //}

            //if (Convert.ToBoolean(Reg_ShortData))
            //    Reg_ShortData = 0;
            //else
            //    Reg_ShortData = 1;

            //// MB Register 쓰기 (HeartBit 용 PC <-> MP 간)
            //RegisterDataNumber = 1;
            //rc = CMotionAPI.ymcSetRegisterData(hRegister_MB, RegisterDataNumber,ref Reg_ShortData);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcSetRegisterData MB : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
            //    return;
            //}

            //// ML Register 전송용 핸들
            //rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_ML, ref hRegister_ML);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle ML : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
            //    return;
            //}

            //Reg_LongData[0] = (OHTInform.Inform.Mode);
            //Reg_LongData[1] = (OHTInform.OHTStatus);
            //Reg_LongData[2] = (OHTInform.DrivingInform.Reset);
            //Reg_LongData[3] = (UtilityClass.ValueToInt(OHTAlarm.EMFlag));
            //Reg_LongData[4] = (OHTInform.DrivingInform.MoveDirection);
            //Reg_LongData[5] = (OHTInform.DrivingInform.Velocity);
            //Reg_LongData[6] = (OHTInform.DrivingInform.Acceleration) * UNIT_REF;
            //Reg_LongData[7] = (OHTInform.DrivingInform.Deceleration) * UNIT_REF;
            //Reg_LongData[8] = (UtilityClass.ValueToInt(OHTInform.PositionFlag.InCurve));
            //Reg_LongData[9] = (OHTInform.DrivingInform.DrivingAxis);
            //Reg_LongData[10] = (OHTInform.DecelStopPosition.DestDist);  // Hyun 150619
            //RegisterDataNumber = 11;  // 10 -> 11 Hyun 150619

            //rc = CMotionAPI.ymcSetRegisterData(hRegister_ML, RegisterDataNumber, ref Reg_LongData[0]);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcSetRegisterData ML : 0x{rc.ToString("X")}, {ErrorDictionary[rc.ToString("X")]}");
            //    return;
            //}
        }
        */
        //public void GetInput(int startAddr, int ioAddr)
        //{
        //    unsafe
        //    {
        //        uint returnValue = 0;
        //        uint ustartAddr = 0;
        //        ustartAddr = Convert.ToUInt32(startAddr);
        //        Pci_Read16(ustartAddr , &returnValue);
        //        OHTInOut.TX16[ioAddr] = Convert.ToInt32(returnValue);

        //        OHTInOut.IOPoolX[ioAddr] = Convert.ToInt32(returnValue);//YDH150804 x값 배열명 변경
        //    }
        //}

        //public void SetOutput(int startAddr, int ioAddr)
        //{
        //    unsafe
        //    {
        //        uint setValue;
        //        uint ustartAddr = 0;
        //        ustartAddr = Convert.ToUInt32(startAddr);
        //        setValue = Convert.ToUInt32(OHTInOut.TY16[ioAddr]);

        //        setValue = Convert.ToUInt32(OHTInOut.IOPoolY[ioAddr]);//YDH150804 y값 배열명 변경

        //        Pci_Write16(ustartAddr, &setValue);
        //    }
        //}

        private void MakeErrorDictionary()
        {
            //ErrorDictionary.Add();
            ErrorDictionary = new Dictionary<string, string>()
            {
                { "00000000", "MP_SUCCESS                               " },
                { "4000FFFF", "MP_FAIL 							        " },
                { "81000001", "WDT_OVER_ERR 						    " },
                { "82000020", "MANUAL_RESET_ERR 					    " },
                { "82000140", "TLB_MLTHIT_ERR 						    " },
                { "820001E0", "UBRK_ERR 							    " },
                { "820000E0", "ADR_RD_ERR 							    " },
                { "82000040", "TLB_MIS_RD_ERR 						    " },
                { "820000A0", "TLB_PROTECTION_RD_ERR 				    " },
                { "82000180", "GENERAL_INVALID_INS_ERR 			        " },
                { "820001A0", "SLOT_ERR 							    " },
                { "82000800", "GENERAL_FPU_DISABLE_ERR 			        " },
                { "82000820", "SLOT_FPU_ERR 						    " },
                { "82000100", "ADR_WR_ERR 							    " },
                { "82000060", "TLB_MIS_WR_ERR 						    " },
                { "820000C0", "TLB_PROTECTION_WR_ERR 				    " },
                { "82000120", "FPU_EXP_ERR 						        " },
                { "82000080", "INITIAL_PAGE_EXP_ERR 				    " },
                { "81000041", "ROM_ERR 							        " },
                { "81000042", "RAM_ERR 							        " },
                { "81000043", "MPU_ERR 							        " },
                { "81000044", "FPU_ERR 							        " },
                { "81000049", "CERF_ERR 							    " },
                { "81000050", "EXIO_ERR 							    " },
                { "8100005F", "BUSIF_ERR 							    " },
                //{ "00000000", "ALM_NO_ALM 							    " },
                { "67050300", "ALM_MK_DEBUG 						    " },
                { "67050301", "ALM_MK_ROUND_ERR 					    " },
                { "67050302", "ALM_MK_FSPEED_OVER 					    " },
                { "67050303", "ALM_MK_FSPEED_NOSPEC 				    " },
                { "67050304", "ALM_MK_PRM_OVER 					        " },
                { "67050305", "ALM_MK_ARCLEN_OVER 					    " },
                { "67050306", "ALM_MK_VERT_NOSPEC 					    " },
                { "67050307", "ALM_MK_HORZ_NOSPEC 					    " },
                { "67050308", "ALM_MK_TURN_OVER 					    " },
                { "67050309", "ALM_MK_RADIUS_OVER 					    " },
                { "6705030A", "ALM_MK_CENTER_ERR 					    " },
                { "6705030B", "ALM_MK_BLOCK_OVER 					    " },
                { "6705030C", "ALM_MK_MAXF_NOSPEC 					    " },
                { "6705030D", "ALM_MK_TDATA_ERR 					    " },
                { "6705030E", "ALM_MK_REG_ERR 						    " },
                { "6705030F", "ALM_MK_COMMAND_CODE_ERR 			        " },
                { "67050310", "ALM_MK_AXIS_CONFLICT 				    " },
                { "67050311", "ALM_MK_POSMAX_OVER 					    " },
                { "67050312", "ALM_MK_DIST_OVER 					    " },
                { "67050313", "ALM_MK_MODE_ERR 					        " },
                { "67050314", "ALM_MK_CMD_CONFLICT 				        " },
                { "67050315", "ALM_MK_RCMD_CONFLICT 				    " },
                { "67050316", "ALM_MK_CMD_MODE_ERR 				        " },
                { "67050317", "ALM_MK_CMD_NOT_ALLOW 				    " },
                { "67050318", "ALM_MK_CMD_DEN_FAIL 				        " },
                { "67050319", "ALM_MK_H_MOVE_ERR 					    " },
                { "6705031A", "ALM_MK_MOVE_NOT_SUPPORT 			        " },
                { "6705031B", "ALM_MK_EVENT_NOT_SUPPORT 			    " },
                { "6705031C", "ALM_MK_ACTION_NOT_SUPPORT 			    " },
                { "6705031D", "ALM_MK_MOVE_TYPE_ERR 				    " },
                { "6705031E", "ALM_MK_VTYPE_ERR 					    " },
                { "6705031F", "ALM_MK_ATYPE_ERR 					    " },
                { "67050320", "ALM_MK_HOMING_METHOD_ERR 			    " },
                { "67050321", "ALM_MK_ACC_ERR 						    " },
                { "67050322", "ALM_MK_DEC_ERR 						    " },
                { "67050323", "ALM_MK_POS_TYPE_ERR 				        " },
                { "67050324", "ALM_MK_INVALID_EVENT_ERR 			    " },
                { "67050325", "ALM_MK_INVALID_ACTION_ERR 			    " },
                { "67050326", "ALM_MK_MOVE_NOT_ACTIVE 				    " },
                { "67050327", "ALM_MK_MOVELIST_NOT_ACTIVE 			    " },
                { "67050328", "ALM_MK_TBL_INVALID_DATA 			        " },
                { "67050329", "ALM_MK_TBL_INVALID_SEG_NUM 			    " },
                { "6705032A", "ALM_MK_TBL_INVALID_AXIS_NUM 		        " },
                { "6705032B", "ALM_MK_TBL_INVALID_ST_SEG 			    " },
                { "6705032C", "ALM_MK_STBL_INVALID_EXE 			        " },
                { "6705032D", "ALM_MK_DTBL_DUPLICATE_EXE 			    " },
                { "6705032E", "ALM_MK_LATCH_CONFLICT 				    " },
                { "6705032F", "ALM_MK_INVALID_AXISTYPE 			        " },
                { "67050330", "ALM_MK_NOT_SVCRDY 					    " },
                { "67050331", "ALM_MK_NOT_SVCRUN 					    " },
                { "67050332", "ALM_MK_MDALARM 						    " },
                { "67050333", "ALM_MK_SUPERPOSE_MASTER_ERR 		        " },
                { "67050334", "ALM_MK_SUPERPOSE_SLAVE_ERR 			    " },
                { "57050335", "ALM_MK_MDWARNING 					    " },
                { "57050336", "ALM_MK_MDWARNING_POSERR 			        " },
                { "67050337", "ALM_MK_NOT_INFINITE_ABS 			        " },
                { "67050338", "ALM_MK_INVALID_LOGICAL_NUM 			    " },
                { "67050339", "ALM_MK_MAX_VELOCITY_ERR 			        " },
                { "6705033A", "ALM_MK_VELOCITY_ERR 				        " },
                { "6705033B", "ALM_MK_APPROACH_ERR 				        " },
                { "6705033C", "ALM_MK_CREEP_ERR 					    " },
                { "83050340", "ALM_MK_OS_ERR_MBOX1 				        " },
                { "83050341", "ALM_MK_OS_ERR_MBOX2 				        " },
                { "83050342", "ALM_MK_OS_ERR_SEND_MSG1 			        " },
                { "83050343", "ALM_MK_OS_ERR_SEND_MSG2 			        " },
                { "83050344", "ALM_MK_OS_ERR_SEND_MSG3 			        " },
                { "83050345", "ALM_MK_OS_ERR_SEND_MSG4 			        " },
                { "53050346", "ALM_MK_ACTION_ERR1 					    " },
                { "53050347", "ALM_MK_ACTION_ERR2 					    " },
                { "53050348", "ALM_MK_ACTION_ERR3 					    " },
                { "53050349", "ALM_MK_RCV_INV_MSG1 				        " },
                { "5305034A", "ALM_MK_RCV_INV_MSG2 				        " },
                { "5305034B", "ALM_MK_RCV_INV_MSG3 				        " },
                { "5305034C", "ALM_MK_RCV_INV_MSG4 				        " },
                { "5305034D", "ALM_MK_RCV_INV_MSG5 				        " },
                { "5305034E", "ALM_MK_RCV_INV_MSG6 				        " },
                { "5305034F", "ALM_MK_RCV_INV_MSG7 				        " },
                { "53050350", "ALM_MK_RCV_INV_MSG8 				        " },
                { "67050360", "ALM_MK_AXIS_HANDLE_ERROR 			    " },
                { "67050361", "ALM_MK_SLAVE_USED_AS_MASTER 		        " },
                { "67050362", "ALM_MK_MASTER_USED_AS_SLAVE 		        " },
                { "67050363", "ALM_MK_SLAVE_HAS_2_MASTERS 			    " },
                { "67050364", "ALM_MK_SLAVE_HAS_NOT_WORK 			    " },
                { "67050365", "ALM_MK_PARAM_OUT_OF_RANGE 			    " },
                { "67050366", "ALM_MK_NNUM_MAX_OVER 				    " },
                { "67050367", "ALM_MK_FGNTBL_INVALID 				    " },
                { "67050368", "ALM_MK_PARAM_OVERFLOW 				    " },
                { "67050369", "ALM_MK_ALREADY_COMMANDED 			    " },
                { "6705036A", "ALM_MK_MULTIPLE_SHIFTS 				    " },
                { "6705036B", "ALM_MK_CAMTBL_INVALID 				    " },
                { "6705036C", "ALM_MK_ABORTED_BY_STOPMTN 			    " },
                { "6705036D", "ALM_MK_HMETHOD_INVALID 				    " },
                { "6705036E", "ALM_MK_MASTER_INVALID 				    " },
                { "6705036F", "ALM_MK_DATA_HANDLE_INVALID 			    " },
                { "67050370", "ALM_MK_UNKNOWN_CAM_GEAR_ERR 		        " },
                { "67050371", "ALM_MK_REG_SIZE_INVALID 			        " },
                { "67050372", "ALM_MK_ACTION_HANDLE_ERROR 			    " },
                { "83040380", "ALM_MM_OS_ERR_MBOX1 				        " },
                { "83040381", "ALM_MM_OS_ERR_SEND_MSG1 			        " },
                { "83040382", "ALM_MM_OS_ERR_SEND_MSG2 			        " },
                { "83040383", "ALM_MM_OS_ERR_RCV_MSG1 				    " },
                { "67040384", "ALM_MM_MK_BUSY 						    " },
                { "53040385", "ALM_MM_RCV_INV_MSG1 				        " },
                { "53040386", "ALM_MM_RCV_INV_MSG2 				        " },
                { "53040387", "ALM_MM_RCV_INV_MSG3 				        " },
                { "53040388", "ALM_MM_RCV_INV_MSG4 				        " },
                { "53040389", "ALM_MM_RCV_INV_MSG5 				        " },
                { "53060480", "ALM_IM_DEVICEID_ERR 				        " },
                { "53060481", "ALM_IM_REGHANDLE_ERR 				    " },
                { "53060482", "ALM_IM_GLOBALHANDLE_ERR 			        " },
                { "53060483", "ALM_IM_DEVICETYPE_ERR 				    " },
                { "53060484", "ALM_IM_OFFSET_ERR 					    " },
                { "57020501", "AM_ER_UNDEF_COMMAND 				        " },
                { "57020502", "AM_ER_UNDEF_CMNDTYPE 				    " },
                { "57020503", "AM_ER_UNDEF_OBJTYPE 				        " },
                { "57020504", "AM_ER_UNDEF_HANDLETYPE 				    " },
                { "57020505", "AM_ER_UNDEF_PKTDAT 					    " },
                { "57020506", "AM_ER_UNDEF_AXIS 					    " },
                { "57020510", "AM_ER_MSGBUF_GET_FAULT 				    " },
                { "57020511", "AM_ER_ACTSIZE_GET_FAULT 			        " },
                { "57020512", "AM_ER_APIBUF_GET_FAULT 				    " },
                { "57020513", "AM_ER_MOVEOBJ_GET_FAULT 			        " },
                { "57020514", "AM_ER_EVTTBL_GET_FAULT 				    " },
                { "57020515", "AM_ER_ACTTBL_GET_FAULT 				    " },
                { "57020516", "AM_ER_1BY1APIBUF_GET_FAULT 			    " },
                { "57020517", "AM_ER_AXSTBL_GET_FAULT 				    " },
                { "57020518", "AM_ER_SUPERPOSEOBJ_GET_FAULT 		    " },
                { "57020519", "AM_ER_SUPERPOSEOBJ_CLEAR_FAULT 		    " },
                { "5702051A", "AM_ER_AXIS_IN_USE 					    " },
                { "5702051B", "AM_ER_HASHTBL_GET_FAULT 			        " },
                { "57020530", "AM_ER_UNMATCH_OBJHNDL 				    " },
                { "57020531", "AM_ER_UNMATCH_OBJECT 				    " },
                { "57020532", "AM_ER_UNMATCH_APIBUF 				    " },
                { "57020533", "AM_ER_UNMATCH_MSGBUF 				    " },
                { "57020534", "AM_ER_UNMATCH_ACTBUF 				    " },
                { "57020535", "AM_ER_UNMATH_SEQUENCE 				    " },
                { "57020536", "AM_ER_UNMATCH_1BY1APIBUF 			    " },
                { "57020537", "AM_ER_UNMATCH_MOVEOBJTABLE 			    " },
                { "57020538", "AM_ER_UNMATCH_MOVELISTTABLE 		        " },
                { "57020539", "AM_ER_UNMATCH_MOVELIST_OBJECT 		    " },
                { "5702053A", "AM_ER_UNMATCH_MOVELIST_OBJHNDL 		    " },
                { "57020550", "AM_ER_UNGET_MOVEOBJTABLE 			    " },
                { "57020551", "AM_ER_UNGET_MOVELISTTABLE 			    " },
                { "57020552", "AM_ER_UNGET_1BY1APIBUFTABLE 		        " },
                { "57020560", "AM_ER_NOEMPTYTBL_ERROR 				    " },
                { "57020561", "AM_ER_NOTGETSEM_ERROR 				    " },
                { "57020562", "AM_ER_NOTGETTBLADD_ERROR 			    " },
                { "57020563", "AM_ER_NOTWRTTBL_ERROR 				    " },
                { "57020564", "AM_ER_TBLINDEX_ERROR 				    " },
                { "57020565", "AM_ER_ILLTBLTYPE_ERROR 				    " },
                { "57020570", "AM_ER_UNSUPORTED_EVENT 				    " },
                { "57020571", "AM_ER_WRONG_SEQUENCE 				    " },
                { "57020572", "AM_ER_MOVEOBJ_BUSY 					    " },
                { "57020573", "AM_ER_MOVELIST_BUSY 				        " },
                { "57020574", "AM_ER_MOVELIST_ADD_FAULT 			    " },
                { "57020575", "AM_ER_CONFLICT_PHI_AXS 				    " },
                { "57020576", "AM_ER_CONFLICT_LOG_AXS 				    " },
                { "57020577", "AM_ER_PKTSTS_ERROR 					    " },
                { "57020578", "AM_ER_CONFLICT_NAME 				        " },
                { "57020579", "AM_ER_ILLEGAL_NAME 					    " },
                { "5702057A", "AM_ER_SEMAPHORE_ERROR 				    " },
                { "5702057B", "AM_ER_LOG_AXS_OVER 					    " },
                { "55060B00", "IM_STATION_ERR 						    " },
                { "55060B01", "IM_IO_ERR 							    " },
                { "53168001", "MP_FILE_ERR_GENERAL 				        " },
                { "53168002", "MP_FILE_ERR_NOT_SUPPORTED 			    " },
                { "53168003", "MP_FILE_ERR_INVALID_ARGUMENT 		    " },
                { "53168004", "MP_FILE_ERR_INVALID_HANDLE 			    " },
                { "53168064", "MP_FILE_ERR_NO_FILE 				        " },
                { "53168065", "MP_FILE_ERR_INVALID_PATH 			    " },
                { "53168066", "MP_FILE_ERR_EOF 					        " },
                { "53168067", "MP_FILE_ERR_PERMISSION_DENIED 		    " },
                { "53168068", "MP_FILE_ERR_TOO_MANY_FILES 			    " },
                { "53168069", "MP_FILE_ERR_FILE_BUSY 				    " },
                { "5316806A", "MP_FILE_ERR_TIMEOUT 				        " },
                { "531680C8", "MP_FILE_ERR_BAD_FS 					    " },
                { "531680C9", "MP_FILE_ERR_FILESYSTEM_FULL 		        " },
                { "531680CA", "MP_FILE_ERR_INVALID_LFM 			        " },
                { "531680CB", "MP_FILE_ERR_TOO_MANY_LFM 			    " },
                { "5316812C", "MP_FILE_ERR_INVALID_PDM 			        " },
                { "5316812D", "MP_FILE_ERR_INVALID_MEDIA 			    " },
                { "5316812E", "MP_FILE_ERR_TOO_MANY_PDM 			    " },
                { "5316812F", "MP_FILE_ERR_TOO_MANY_MEDIA 			    " },
                { "53168130", "MP_FILE_ERR_WRITE_PROTECTED 		        " },
                { "53168190", "MP_FILE_ERR_INVALID_DEVICE 			    " },
                { "53168191", "MP_FILE_ERR_DEVICE_IO 				    " },
                { "53168192", "MP_FILE_ERR_DEVICE_BUSY 			        " },
                { "5316A711", "MP_FILE_ERR_NO_CARD 				        " },
                { "5316A712", "MP_FILE_ERR_CARD_POWER 				    " },
                { "53178FFF", "MP_CARD_SYSTEM_ERR 					    " },
                { "83001A01", "ERROR_CODE_GET_DIREC_OFFSET 		        " },
                { "83001A02", "ERROR_CODE_GET_DIREC_INFO 			    " },
                { "83001A03", "ERROR_CODE_FUNC_TABLE                    " },
                { "83001A04", "ERROR_CODE_SLEEP_TASK                    " },
                { "43001A41", "ERROR_CODE_DEVICE_HANDLE_FULL            " },
                { "43001A42", "ERROR_CODE_ALLOC_MEMORY                  " },
                { "43001A43", "ERROR_CODE_BUFCOPY                       " },
                { "43001A44", "ERROR_CODE_GET_COMMEM_OFFSET             " },
                { "43001A45", "ERROR_CODE_CREATE_SEMPH                  " },
                { "43001A46", "ERROR_CODE_DELETE_SEMPH                  " },
                { "43001A47", "ERROR_CODE_LOCK_SEMPH                    " },
                { "43001A48", "ERROR_CODE_UNLOCK_SEMPH                  " },
                { "43001A49", "ERROR_CODE_PACKETSIZE_OVER               " },
                { "43001A4A", "ERROR_CODE_UNREADY                       " },
                { "43001A4B", "ERROR_CODE_CPUSTOP                       " },
                { "470B1A81", "ERROR_CODE_CNTRNO                        " },
                { "470B1A82", "ERROR_CODE_SELECTION                     " },
                { "470B1A83", "ERROR_CODE_LENGTH                        " },
                { "470B1A84", "ERROR_CODE_OFFSET                        " },
                { "470B1A85", "ERROR_CODE_DATACOUNT                     " },
                { "46001A86", "ERROR_CODE_DATREAD                       " },
                { "46001A87", "ERROR_CODE_DATWRITE                      " },
                { "46001A88", "ERROR_CODE_BITWRITE                      " },
                { "46001A89", "ERROR_CODE_DEVCNTR                       " },
                { "460F1A8A", "ERROR_CODE_NOTINIT                       " },
                { "41001A8B", "ERROR_CODE_SEMPHLOCK                     " },
                { "41001A8C", "ERROR_CODE_SEMPHUNLOCK                   " },
                { "460F1A8D", "ERROR_CODE_DRV_PROC                      " },
                { "460F1A8E", "ERROR_CODE_GET_DRIVER_HANDLE             " },
                { "450E1AC1", "ERROR_CODE_SEND_MSG                      " },
                { "450E1AC2", "ERROR_CODE_RECV_MSG                      " },
                { "450E1AC3", "ERROR_CODE_INVALID_RESPONSE              " },
                { "450E1AC4", "ERROR_CODE_INVALID_ID                    " },
                { "450E1AC5", "ERROR_CODE_INVALID_STATUS                " },
                { "450E1AC6", "ERROR_CODE_INVALID_CMDCODE               " },
                { "450E1AC7", "ERROR_CODE_INVALID_SEQNO                 " },
                { "450E1AC8", "ERROR_CODE_SEND_RETRY_OVER               " },
                { "450E1AC9", "ERROR_CODE_RECV_RETRY_OVER               " },
                { "450E1ACA", "ERROR_CODE_RESPONSE_TIMEOUT              " },
                { "450E1ACB", "ERROR_CODE_WAIT_FOR_EVENT                " },
                { "450E1ACC", "ERROR_CODE_EVENT_OPEN                    " },
                { "450E1ACD", "ERROR_CODE_EVENT_RESET                   " },
                { "450E1ACE", "ERROR_CODE_EVENT_READY                   " },
                { "43001B01", "ERROR_CODE_PROCESSNUM                    " },
                { "43001B02", "ERROR_CODE_GET_PROC_INFO                 " },
                { "43001B03", "ERROR_CODE_THREADNUM                     " },
                { "43001B04", "ERROR_CODE_GET_THRD_INFO                 " },
                { "43001B05", "ERROR_CODE_CREATE_MBOX                   " },
                { "43001B06", "ERROR_CODE_DELETE_MBOX                   " },
                { "83001B07", "ERROR_CODE_GET_TASKID                    " },
                { "43001B08", "ERROR_CODE_NO_THREADINFO                 " },
                { "43001B09", "ERROR_CODE_COM_INITIALIZE                " },
                { "430F1B41", "ERROR_CODE_COMDEVICENUM                  " },
                { "430F1B42", "ERROR_CODE_GET_COM_DEVICE_HANDLE         " },
                { "430F1B43", "ERROR_CODE_COM_DEVICE_FULL               " },
                { "430F1B44", "ERROR_CODE_CREATE_PANELOBJ               " },
                { "430F1B45", "ERROR_CODE_OPEN_PANELOBJ                 " },
                { "430F1B46", "ERROR_CODE_CLOSE_PANELOBJ                " },
                { "430F1B47", "ERROR_CODE_PROC_PANELOBJ                 " },
                { "430F1B48", "ERROR_CODE_CREATE_CNTROBJ                " },
                { "430F1B49", "ERROR_CODE_OPEN_CNTROBJ                  " },
                { "430F1B4A", "ERROR_CODE_CLOSE_CNTROBJ                 " },
                { "430F1B4B", "ERROR_CODE_PROC_CNTROBJ                  " },
                { "430F1B4C", "ERROR_CODE_CREATE_COMDEV_MUTEX           " },
                { "430F1B4D", "ERROR_CODE_CREATE_COMDEV_MAPFILE         " },
                { "430F1B4E", "ERROR_CODE_OPEN_COMDEV_MAPFILE           " },
                { "430F1B4F", "ERROR_CODE_NOT_OBJECT_TYPE               " },
                { "430F1B50", "ERROR_CODE_COM_NOT_OPENED                " },
                { "43081B80", "ERROR_CODE_PNLCMD_CPU_CONTROL            " },
                { "43081B81", "ERROR_CODE_PNLCMD_SFILE_READ             " },
                { "43081B82", "ERROR_CODE_PNLCMD_SFILE_WRITE            " },
                { "43081B83", "ERROR_CODE_PNLCMD_REGISTER_READ          " },
                { "43081B84", "ERROR_CODE_PNLCMD_REGISTER_WRITE         " },
                { "43081B85", "ERROR_CODE_PNLCMD_API_SEND               " },
                { "43081B86", "ERROR_CODE_PNLCMD_API_RECV               " },
                { "43081B87", "ERROR_CODE_PNLCMD_NO_RESPONSE            " },
                { "43081B88", "ERROR_CODE_PNLCMD_PACKET_READ            " },
                { "43081B89", "ERROR_CODE_PNLCMD_PACKET_WRITE           " },
                { "43081B8A", "ERROR_CODE_PNLCMD_STATUS_READ            " },
                { "440D1BA0", "ERROR_CODE_NOT_CONTROLLER_RDY            " },
                { "440D1BA1", "ERROR_CODE_DOUBLE_CMD                    " },
                { "440D1BA2", "ERROR_CODE_DOUBLE_RCMD                   " },
                { "43001BC1", "ERROR_CODE_FILE_NOT_OPENED               " },
                { "43001BC2", "ERROR_CODE_OPENFILE_NUM                  " },
                { "4308106f", "MP_CTRL_SYS_ERROR                        " },
                { "43081092", "MP_CTRL_PARAM_ERR                        " },
                { "43081094", "MP_CTRL_FILE_NOT_FOUND                   " },
                { "470B1100", "MP_NOTMOVEHANDLE 					    " },
                { "470B1101", "MP_NOTTIMERHANDLE 					    " },
                { "470B1102", "MP_NOTINTERRUPT 					        " },
                { "470B1103", "MP_NOTEVENTHANDLE 					    " },
                { "470B1104", "MP_NOTMESSAGEHANDLE 				        " },
                { "470B1105", "MP_NOTUSERFUNCTIONHANDLE 			    " },
                { "470B1106", "MP_NOTACTIONHANDLE 					    " },
                { "470B1107", "MP_NOTSUBSCRIBEHANDLE 				    " },
                { "470B1108", "MP_NOTDEVICEHANDLE 					    " },
                { "470B1109", "MP_NOTAXISHANDLE 					    " },
                { "470B110A", "MP_NOTMOVELISTHANDLE 				    " },
                { "470B110B", "MP_NOTTRACEHANDLE 					    " },
                { "470B110C", "MP_NOTGLOBALDATAHANDLE 				    " },
                { "470B110D", "MP_NOTSUPERPOSEHANDLE 				    " },
                { "470B110E", "MP_NOTCONTROLLERHANDLE 				    " },
                { "470B110F", "MP_NOTFILEHANDLE 					    " },
                { "470B1110", "MP_NOTREGISTERDATAHANDLE 			    " },
                { "470B1111", "MP_NOTALARMHANDLE 					    " },
                { "470B1112", "MP_NOTCAMHANDLE 					        " },
                { "470B11FF", "MP_NOTHANDLE 						    " },
                { "470B1200", "MP_NOTEVENTTYPE 					        " },
                { "470B1201", "MP_NOTDEVICETYPE 					    " },
                { "4B0B1202", "MP_NOTDATAUNITSIZE 					    " },
                { "470B1203", "MP_NOTDEVICE 						    " },
                { "470B1204", "MP_NOTACTIONTYPE 					    " },
                { "4B0B1205", "MP_NOTPARAMNAME 					        " },
                { "470B1206", "MP_NOTDATATYPE 						    " },
                { "470B1207", "MP_NOTBUFFERTYPE 					    " },
                { "4B0B1208", "MP_NOTMOVETYPE 						    " },
                { "470B1209", "MP_NOTANGLETYPE 					        " },
                { "4B0B120A", "MP_NOTDIRECTION 					        " },
                { "4B0B120B", "MP_NOTAXISTYPE 						    " },
                { "4B0B120C", "MP_NOTUNITTYPE 						    " },
                { "470B120D", "MP_NOTCOMDEVICETYPE 				        " },
                { "470B120E", "MP_NOTCONTROLTYPE 					    " },
                { "4B0B120F", "MP_NOTFILETYPE 						    " },
                { "470B1210", "MP_NOTSEMAPHORETYPE 				        " },
                { "470B1211", "MP_NOTSYSTEMOPTION 					    " },
                { "470B1212", "MP_TIMEOUT_ERR 						    " },
                { "470B1213", "MP_TIMEOUT 							    " },
                { "470B1214", "MP_NOTSUBSCRIBETYPE 				        " },
                { "4B0B1214", "MP_NOTSCANTYPE 						    " },
                { "470B1300", "MP_DEVICEOFFSETOVER 				        " },
                { "470B1301", "MP_DEVICEBITOFFSETOVER 				    " },
                { "4B0B1302", "MP_UNITCOUNTOVER 					    " },
                { "4B0B1303", "MP_COMPAREVALUEOVER 				        " },
                { "4B0B1304", "MP_FCOMPAREVALUEOVER 				    " },
                { "470B1305", "MP_EVENTCOUNTOVER 					    " },
                { "470B1306", "MP_VALUEOVER 						    " },
                { "470B1307", "MP_FVALUEOVER 						    " },
                { "470B1308", "MP_PSTOREDVALUEOVER 				        " },
                { "470B1309", "MP_PSTOREDFVALUEOVER 				    " },
                { "470B130A", "MP_SIZEOVER 						        " },
                { "470B1310", "MP_RECEIVETIMEROVER 				        " },
                { "470B1311", "MP_RECNUMOVER 						    " },
                { "4B0B1312", "MP_PARAMOVER 						    " },
                { "470B1313", "MP_FRAMEOVER 						    " },
                { "4B0B1314", "MP_RADIUSOVER 						    " },
                { "470B1315", "MP_CONTROLLERNOOVER 				        " },
                { "4B0B1316", "MP_AXISNUMOVER 						    " },
                { "4B0B1317", "MP_DIGITOVER 						    " },
                { "4B0B1318", "MP_CALENDARVALUEOVER 				    " },
                { "470B1319", "MP_OFFSETOVER 						    " },
                { "470B131A", "MP_NUMBEROVER 						    " },
                { "470B131B", "MP_RACKOVER 						        " },
                { "470B131C", "MP_SLOTOVER 						        " },
                { "470B131D", "MP_FIXVALUEOVER 					        " },
                { "470B131E", "MP_REGISTERINFOROVER                     " },
                { "430B1400", "PC_MEMORY_ERR 						    " },
                { "470B1500", "MP_NOCOMMUDEVICETYPE 				    " },
                { "470B1501", "MP_NOTPROTOCOLTYPE 					    " },
                { "470B1502", "MP_NOTFUNCMODE 						    " },
                { "470B1503", "MP_MYADDROVER 						    " },
                { "470B1504", "MP_NOTPORTTYPE 						    " },
                { "470B1505", "MP_NOTPROTMODE 						    " },
                { "470B1506", "MP_NOTCHARSIZE 						    " },
                { "470B1507", "MP_NOTPARITYBIT 					        " },
                { "470B1508", "MP_NOTSTOPBIT 						    " },
                { "470B1509", "MP_NOTBAUDRAT 						    " },
                { "470B1510", "MP_TRANSDELAYOVER 					    " },
                { "470B1511", "MP_PEERADDROVER 					        " },
                { "470B1512", "MP_SUBNETMASKOVER 					    " },
                { "470B1513", "MP_GWADDROVER 						    " },
                { "470B1514", "MP_DIAGPORTOVER 					        " },
                { "470B1515", "MP_PROCRETRYOVER 					    " },
                { "470B1516", "MP_TCPZEROWINOVER 					    " },
                { "470B1517", "MP_TCPRETRYOVER 					        " },
                { "470B1518", "MP_TCPFINOVER 						    " },
                { "470B1519", "MP_IPASSEMBLEOVER 					    " },
                { "470B1520", "MP_MAXPKTLENOVER 					    " },
                { "470B1521", "MP_PEERPORTOVER 					        " },
                { "470B1522", "MP_MYPORTOVER 						    " },
                { "470B1523", "MP_NOTTRANSPROT 					        " },
                { "470B1524", "MP_NOTAPPROT 						    " },
                { "470B1525", "MP_NOTCODETYPE 						    " },
                { "470B1526", "MP_CYCTIMOVER 						    " },
                { "470B1527", "MP_NOTIOMAPCOM 						    " },
                { "470B1528", "MP_NOTIOTYPE 						    " },
                { "470B1529", "MP_IOOFFSETOVER 					        " },
                { "470B1530", "MP_IOSIZEOVER 						    " },
                { "470B1531", "MP_TIOSIZEOVER 						    " },
                { "470B1532", "MP_MEMORY_ERR 						    " },
                { "470B1533", "MP_NOTPTR 							    " },
                { "43001800", "MP_TABLEOVER 						    " },
                { "43001801", "MP_ALARM 							    " },
                { "43001802", "MP_MEMORYOVER 						    " },
                { "470B1803", "MP_EXEC_ERR 						        " },
                { "470B1804", "MP_NOTLOGICALAXIS 					    " },
                { "470B1805", "MP_NOTSUPPORTED 					        " },
                { "470B1806", "MP_ILLTEXT 							    " },
                { "470B1807", "MP_NOFILENAME 						    " },
                { "470B1808", "MP_NOTREGSTERNAME 					    " },
                { "4B0B1809", "MP_FILEALREADYOPEN 					    " },
                { "470B180A", "MP_NOFILEPACKET 					        " },
                { "470B180B", "MP_NOTFILEPACKETSIZE 				    " },
                { "4B0B180C", "MP_NOTUSERFUNCTION 					    " },
                { "4B0B180D", "MP_NOTPARAMETERTYPE 				        " },
                { "470B180E", "MP_ILLREGHANDLETYPE 				        " },
                { "470B1810", "MP_ILLREGTYPE 						    " },
                { "470B1811", "MP_ILLREGSIZE 						    " },
                { "470B1812", "MP_REGNUMOVER 						    " },
                { "470B1813", "MP_RELEASEWAIT 						    " },
                { "470B1814", "MP_PRIORITYOVER 					        " },
                { "470B1815", "MP_NOTCHANGEPRIORITY 				    " },
                { "470B1816", "MP_SEMAPHOREOVER 					    " },
                { "470B1817", "MP_NOTSEMAPHOREHANDLE 				    " },
                { "470B1818", "MP_SEMAPHORELOCKED 					    " },
                { "470B1819", "MP_CONTINUE_RELEASEWAIT 			        " },
                { "4B0B1820", "MP_NOTTABLENAME 					        " },
                { "470B1821", "MP_ILLTABLETYPE 					        " },
                { "470B1822", "MP_TIMEOUTVALUEOVER 				        " },
                { "470B19FF", "MP_OTHER_ERR 						    " },
            };
        }
    }
}
