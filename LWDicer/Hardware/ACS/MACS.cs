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
        public const int MAX_AXIS_COUNT = 32;
        public const int MAX_BUFFER_CNT = 64;

        public const int USE_AXIS_COUNT = 4;
        public const int INT_AXIS_STATUS = 3;
        public const int REAL_AXIS_STATUS = 5;

        public class CServoStatus
        {
            public double EncoderPos;
            public double CommandPos;
            public double Velocity;     //Servo 현재 속도
            public double LoadFactor;
            public double PositionErr;

            public bool IsServoOn;
            public bool IsInOpenLoop;
            public bool IsInPosition;
            public bool IsBusy;
            public bool IsAccelerating;

            public bool IsDriverFault;
            public bool MotorOverHeat;
            public bool Is;
            public bool IsMinusSensor;
            public bool IsPlusSensor;
            public bool IsHomeSensor;
            
            
            public bool OriginFlag;       // origin return flag
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

        public enum EMotorSpeed // Motor speed type
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
        
        public class CServoStatusArray
        {
            public int[,] IntStatus = new int[USE_AXIS_COUNT, INT_AXIS_STATUS];
            public double[,] DoubleStatus = new double[USE_AXIS_COUNT, REAL_AXIS_STATUS];

            public CServoStatusArray()
            {

            }
        }

        public class CACSChannel
        {
            private Channel ACS;
            private string addressTCP;
            private int portNum;
            private bool IsChannelOpen;

            CServoStatusArray ServoStatus;

            Thread m_hThread;
            
            public CACSChannel()
            {
                Channel ACS = new Channel();
                string addressTCP = "10.0.0.100";
                int portNum = 701;

                IsChannelOpen = false;
            }

            public int ChannelOpen()
            {
                ACS.OpenCommEthernetTCP(addressTCP, portNum);

                ThreadStart();
                IsChannelOpen = true;

                return SUCCESS;
            }
            public int ChannelClose()
            {
                ThreadStop();

                ACS.CloseComm();

                IsChannelOpen = false;

                return SUCCESS;
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

                    Thread.Sleep(DEF_Thread.ThreadSleepTime);
                }
            }
        }
        public class CACSRefComp
        {

        }

        public class CACSData
        {

        }
        
    }

    public class MACS : MObject, IDisposable
    {
        //
        private CACSRefComp m_RefComp;
        private CACSData m_Data;
        public int InstalledAxisNo; // System에 Install된 max axis

        // remember speed type in this class for easy controlling
        //public int SpeedType { get; set; } = (int)EMotorSpeed.MANUAL_SLOW;

        public string LastHWMessage { get; private set; }

        MTickTimer m_waitTimer = new MTickTimer();
        UInt16 APITimeOut = 5000;
        UInt16 APIJogTime = 100;    // Jog Timeout ms

        //
        //UInt32[] m_hController = new UInt32[MAX_MP_CPU];    // ACS controller handle
        //UInt32[] m_hAxis = new UInt32[MAX_MP_AXIS];         // Axis handle
        //UInt32[] m_hDevice = new UInt32[MAX_MP_AXIS];       // Device handle
        public CServoStatus[] ServoStatus = new CServoStatus[MAX_AXIS_COUNT];

        //
        Thread m_hThread;   // Thread Handle

        //
        public bool NeedCheckSafety { get; set; } = false;

        public MACS(CObjectInfo objInfo, CACSRefComp refComp, CACSData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            for (int i = 0; i < MAX_AXIS_COUNT; i++)
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



        private int GetDeviceLength(int deviceNo)
        {
            int length = 1;
            if (deviceNo < (int)EACS_Device.ALL)
            {
                // axis, null, default
                length = 1;
            }
            else
            {
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

        private int GetDeviceWaitCompletion(int deviceNo, out UInt16[] waits, ushort mode)
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
            for (int i = 0; i < axisList.Length; i++)
            {
                int servoNo = axisList[i];
                if (servoNo == (int)EACS_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }

               // hAxis[i] = m_hAxis[servoNo];
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
                if (servoNo == (int)EACS_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                //m_Data.MPBoard[boardNo].GetSpeedData(servoNo, out speedData[i], speedType);
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
                if (servoNo == (int)EACS_Axis.NULL) // skip if axis not exist.
                {
                    continue;
                }
                //m_Data.MPBoard[boardNo].GetTimeLimitData(servoNo, out timeLimit[i]);
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


            return SUCCESS;
        }




        public void GetAllServoStatus()
        {
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
            
        }

        public int ServoOn(int deviceNo)
        {
            int iResult = ResetAlarm(deviceNo);
            if (iResult != SUCCESS) return iResult;


            return SUCCESS;
        }

        public int AllServoOn()
        {
            int iResult = ServoOn((int)EACS_Device.ALL);
            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }

        public int AllServoOff()
        {
            int iResult = ServoOff((int)EACS_Device.ALL);
            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }

        public int ServoOff(int deviceNo)
        {
            if (deviceNo == (int)EACS_Device.NULL) return SUCCESS; // return success if device is null

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
            return CheckMoveComplete(servoNo, out bComplete);
        }

        public int CheckMoveComplete(int servoNo, out bool bComplete)
        {
            UInt32 returnValue = 0;
            bComplete = false;

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

        public int ServoMotionStop(int deviceNo, ushort mode = 0 )
        {

            return SUCCESS;
        }

        public int AllServoMotionStop()
        {

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
