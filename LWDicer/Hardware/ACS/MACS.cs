using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
        //public CServoStatus[] ServoStatus = new CServoStatus[MAX_MP_AXIS];

        //
        Thread m_hThread;   // Thread Handle

        //
        public bool NeedCheckSafety { get; set; } = false;

        public MACS(CObjectInfo objInfo, CACSRefComp refComp, CACSData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            //for (int i = 0; i < MAX_MP_AXIS; i++)
            //{
            //    ServoStatus[i] = new CServoStatus();
            //}
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

        //public void SetMPMotionData(CMPMotionData[] motions)
        //{
        //    m_Data.SetMPMotionData(motions);
        //}

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

    }
}
