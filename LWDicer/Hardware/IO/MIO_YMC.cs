using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using MotionYMC;

using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_IO;

namespace LWDicer.Layers
{
    public class MIO_YMC : MObject, IIO
    {
        UInt32 m_hController; // Yaskawa controller handle

        MTickTimer m_waitTimer = new MTickTimer();
        Thread m_hThread;   // Thread Handle

        public enum EYMCRegisterType
        {
            S,  // System
            M,  // Data
            I,  // Input
            O,  // Output
            C,  // Constant
            D,  // D Register
        }

        public enum EYMCDataType
        {
            B,  // bit
            W,  // int
            L,  // long int
            F,  // float
        }

        public MIO_YMC(CObjectInfo objInfo, UInt32 hController = 0)
            : base(objInfo)
        {
            m_hController = hController;
        }

        ~MIO_YMC()
        {
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns></returns>
        public int Initialize()
        {
            return SUCCESS;
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
#if !SIMULATION_IO
#endif

                Sleep(DEF_Thread.ThreadSleepTime);
            }
        }

        /// <summary>
        /// Stop Communication and close driver handle
        /// </summary>
        /// <returns></returns>
        public int Terminate()
        {
            return SUCCESS;
        }

        /// <summary>
        /// rc를 이용해서 library에서 제공하는 에러 메시지및 코드를 보고
        /// </summary>
        /// <param name="error"></param>
        /// <param name="rc"></param>
        /// <param name="writeLog"></param>
        /// <returns></returns>
        public int GenerateErrorCode(int error, UInt32 rc, bool writeLog = true)
        {
            ErrorSubMsg = String.Format($"0x{rc.ToString("X")}, {CMotionAPI.ErrorDictionary[rc.ToString("X")]}");
            WriteLog(ErrorSubMsg, ELogType.Debug, ELogWType.D_Error, true);

            return base.GenerateErrorCode(error, writeLog, true);
        }

        int GetRegisterDataHandle(int addr, EYMCDataType type, out uint hDataHandle)
        {
            Debug.Assert(INPUT_ORIGIN <= addr && addr <= OUTPUT_END);
            hDataHandle = 0;
            string registerName;

            // 160721 by sjr. 아진의 io board를 사용하는데, 아진의 io board는 메모리의 선두 4바이트가 다른용도로 사용되기때문에
            // 한번에 메모리를 연속적으로 읽을수 없어서, 한국야스카와 전기의 박영섭 대리를 통해서 H Ladder & function 을 받아서
            // I, O 번지를 -> M 레지스터로 배치 copy하기 때문에, 소스 프로그램에서는 M 레지스터를 이용하는것으로 변경함
            // I -> M1000, O -> M2000에 mapping 함
            // 즉, 아진의 io board는 선두 2워드를 건너뜀. module configuration에서 io 모듈의 input address : 0010-0017(H), 
            // output address : 0020-0027(H) 라면, 실제로 register list에서 모니터링할때에는 IB00120, OB00220부터 읽어야하고, 
            // 이때의 M register는 MB010000, MB020000 

            // register type
            //if (addr < OUTPUT_ORIGIN)
            //{
            //    registerName = String.Format($"{EYMCRegisterType.I}{type}{(addr - INPUT_ORIGIN).ToString("D4")}");
            //}
            //else
            //{
            //    registerName = String.Format($"{EYMCRegisterType.O}{type}{(addr - OUTPUT_ORIGIN).ToString("D4")}");
            //}

            // 160907 by sjr. 아진 io board를 사용하면서 input address 1234가 hex address MB100EA로 변환된 주소를 사용하여
            // ymcGetRegisterDataHandle 함수를 콜 했더니 MP_NOTREGSTERNAME 리턴함.
            // 160922 by sjr. 위의 MP_NOTREGSTERNAME 에러는 mp720 래더 프로그램에서 지정한 사용하는 모듈의 갯수 범위를 초과한
            // 주소를 지정해서 에러가 발생하는 거였음.
            // 메모리 지정 하는 형식은 MW100 번지의 8번째 비트 = MB1008
            // 그러나 MW100까지는 decimal 표현 + 비트는 hex 표현해야함. 즉 MW112의 12번째 비트 = MB112C 로 표현되어야 함.
            // addr 2176 = MB20110 (즉, 2011워드의 0번째 비트) = 래더프로그램 (한펑션에 10모듈 사용기준)에서 12번째 모듈에 
            // 지정된 address와 연결된 module 로 출력이 나감
            string s1;
            if (addr < OUTPUT_ORIGIN) s1 = "1";
            else s1 = "2";
            int addr2 = addr % 1000;
            int addr_w = addr2 / 16;
            int addr_b = addr2 % 16;

            if(type == EYMCDataType.B)
            {
                registerName = String.Format($"{EYMCRegisterType.M}{type}{s1}{(addr_w).ToString("D3")}{(addr_b).ToString("X1")}");
                //registerName = String.Format($"{EYMCRegisterType.M}{type}{s1}{(addr2).ToString("X4")}");
                //registerName = String.Format($"{EYMCRegisterType.M}{type}{s1}{(addr2).ToString("D4")}");
            } else
            {
                //registerName = String.Format($"{EYMCRegisterType.M}{type}{s1}{(addr2).ToString("X3")}");
                registerName = String.Format($"{EYMCRegisterType.M}{type}{s1}{(addr2).ToString("D3")}");
            }
            if(s1 == "2" && addr == 2176)
            {
                int k = 0;
            }
#if !SIMULATION_IO
            // get handle
            uint rc = CMotionAPI.ymcGetRegisterDataHandle(registerName, ref hDataHandle);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //string str = $"Error ymcGetRegisterDataHandle ML \nErrorCode [ 0x{rc.ToString("X")} ]";
                //WriteLog(str, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_IO_YMC_FAIL_GET_DATA_HANDLE, rc);
            }
#endif

            return SUCCESS;
        }

        int GetRegisterData(int addr, EYMCDataType type, uint RegisterDataNumber, out Int16[] Reg_ShortData, out Int32[] Reg_LongData)
        {
            Debug.Assert(INPUT_ORIGIN <= addr && addr <= OUTPUT_END);
            Reg_ShortData = new Int16[RegisterDataNumber];
            Reg_LongData = new Int32[RegisterDataNumber];
#if SIMULATION_IO
            return SUCCESS;
#endif

            uint hDataHandle;
            int iResult = GetRegisterDataHandle(addr, type, out hDataHandle);
            if (iResult != SUCCESS) return iResult;

            UInt32 ReadDataNumber = 0;                 // Number of obtained registers
            //UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            //Int16[] Reg_ShortData = new Int16[1];      // W or B size register data storage variable
            //Int32[] Reg_LongData = new Int32[1];       // L size register data storage variable
            uint rc;

            if(type == EYMCDataType.B || type == EYMCDataType.W)
            {
                rc = CMotionAPI.ymcGetRegisterData(hDataHandle, RegisterDataNumber, Reg_ShortData, ref ReadDataNumber);
            }
            else
            {
                rc = CMotionAPI.ymcGetRegisterData(hDataHandle, RegisterDataNumber, Reg_LongData, ref ReadDataNumber);
            }

            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //string str = $"Error ymcGetRegisterData MB \nErrorCode [ 0x{rc.ToString("X")} ]";
                //WriteLog(str, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_IO_YMC_FAIL_GET_DATA, rc);
            }

            return SUCCESS;
        }

        int SetRegisterData(int addr, EYMCDataType type, uint RegisterDataNumber, Int16[] Reg_ShortData, Int32[] Reg_LongData)
        {
            Debug.Assert(INPUT_ORIGIN <= addr && addr <= OUTPUT_END);
#if SIMULATION_IO
            return SUCCESS;
#endif

            uint hDataHandle;
            int iResult = GetRegisterDataHandle(addr, type, out hDataHandle);
            if (iResult != SUCCESS) return iResult;

            UInt32 ReadDataNumber = 0;                 // Number of obtained registers
            //UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            //Int16[] Reg_ShortData = new Int16[1];      // W or B size register data storage variable
            //Int32[] Reg_LongData = new Int32[1];       // L size register data storage variable
            uint rc;

            if (type == EYMCDataType.B || type == EYMCDataType.W)
            {
                rc = CMotionAPI.ymcSetRegisterData(hDataHandle, RegisterDataNumber, Reg_ShortData);
            }
            else
            {
                rc = CMotionAPI.ymcSetRegisterData(hDataHandle, RegisterDataNumber, Reg_LongData);
            }

            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //string str = $"Error ymcSetRegisterData MB \nErrorCode [ 0x{rc.ToString("X")} ]";
                //WriteLog(str, ELogType.Debug, ELogWType.D_Error, true);
                return GenerateErrorCode(ERR_IO_YMC_FAIL_SET_DATA, rc);
            }

            return SUCCESS;
        }

        //////////////////////////////////////////////////
        // Get & Set Bit
        public int GetBit(int addr, out bool bStatus)
        {
            bStatus = false;
#if SIMULATION_IO
            return SUCCESS;
#endif
            Debug.Assert(INPUT_ORIGIN <= addr && addr <= OUTPUT_END);

            Int16[] Reg_ShortData;      // W or B size register data storage variable
            Int32[] Reg_LongData;       // L size register data storage variable
            int iResult = GetRegisterData(addr, EYMCDataType.B, 1, out Reg_ShortData, out Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            if (Reg_ShortData[0] == TRUE) bStatus = true;
            else bStatus = false;

            return SUCCESS;
        }

        public int IsOn(int addr, out bool bStatus)
        {
            bStatus = false;
            if (addr < INPUT_ORIGIN || addr > OUTPUT_END)
                return GenerateErrorCode(ERR_IO_ADDRESS_INVALID);

            bool bTemp;
            int iResult = GetBit(addr, out bTemp);
            if (iResult != SUCCESS) return iResult;

            if (bTemp == true) bStatus = true;
            else bStatus = false;

            return SUCCESS;
        }

        public int IsOff(int addr, out bool bStatus)
        {
            bStatus = false;
            if (addr < INPUT_ORIGIN || addr > OUTPUT_END)
                return GenerateErrorCode(ERR_IO_ADDRESS_INVALID);

            bool bTemp;
            int iResult = GetBit(addr, out bTemp);
            if (iResult != SUCCESS) return iResult;

            if (bTemp == false) bStatus = true;
            else bStatus = false;

            return SUCCESS;
        }

        public int SetBit(int addr, bool bStatus)
        {
#if SIMULATION_IO
            return SUCCESS;
#endif
            if (addr < INPUT_ORIGIN || addr > OUTPUT_END)
                return GenerateErrorCode(ERR_IO_ADDRESS_INVALID);

            Debug.Assert(INPUT_ORIGIN <= addr && addr <= OUTPUT_END);

            UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            Int16[] Reg_ShortData = new Int16[RegisterDataNumber];  // W or B size register data storage variable
            Int32[] Reg_LongData = new Int32[RegisterDataNumber];       // L size register data storage variable

            if (bStatus == true) Reg_ShortData[0] = TRUE;
            else Reg_ShortData[0] = FALSE;

            int iResult = SetRegisterData(addr, EYMCDataType.B, RegisterDataNumber, Reg_ShortData, Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int OutputOn(int addr)
        {
            int iResult = SetBit(addr, true);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        public int OutputOff(int addr)
        {
            int iResult = SetBit(addr, false);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int OutputToggle(int addr)
        {
            bool bStatus;
            int iResult = IsOn(addr, out bStatus);
            if (iResult != SUCCESS) return iResult;

            bStatus = !bStatus;
            iResult = SetBit(addr, bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetBit(string strAddr, out bool bStatus)
        {
            return GetBit(CUtils.IntTryParse(strAddr), out bStatus);
        }
        public int IsOn(string strAddr, out bool bStatus)
        {
            return IsOn(CUtils.IntTryParse(strAddr), out bStatus);
        }
        public int IsOff(string strAddr, out bool bStatus)
        {
            return IsOff(CUtils.IntTryParse(strAddr), out bStatus);
        }

        public int SetBit(string strAddr, bool bStatus)
        {
            return SetBit(CUtils.IntTryParse(strAddr), bStatus);
        }
        public int OutputOn(string strAddr)
        {
            return OutputOn(CUtils.IntTryParse(strAddr));
        }
        public int OutputOff(string strAddr)
        {
            return OutputOff(CUtils.IntTryParse(strAddr));
        }
        public int OutputToggle(string strAddr)
        {
            return OutputToggle(CUtils.IntTryParse(strAddr));
        }

        //////////////////////////////////////////////////
        // Get & Put value
        public int GetInt16(int addr, out Int16 value)
        {
            value = 0;
#if SIMULATION_IO
            return SUCCESS;
#endif
            Debug.Assert(INPUT_ORIGIN <= addr && addr <= OUTPUT_END);

            Int16[] Reg_ShortData;      // W or B size register data storage variable
            Int32[] Reg_LongData;       // L size register data storage variable
            int iResult = GetRegisterData(addr, EYMCDataType.W, 1, out Reg_ShortData, out Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            value = Reg_ShortData[0];

            return SUCCESS;
        }

        public int GetInt32(int addr, out Int32 value)
        {
            value = 0;
#if SIMULATION_IO
            return SUCCESS;
#endif
            Debug.Assert(INPUT_ORIGIN <= addr && addr <= OUTPUT_END);

            Int16[] Reg_ShortData;      // W or B size register data storage variable
            Int32[] Reg_LongData;       // L size register data storage variable
            int iResult = GetRegisterData(addr, EYMCDataType.L, 1, out Reg_ShortData, out Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            value = Reg_LongData[0];

            return SUCCESS;
        }

        public int GetFloat(int addr, out double value)
        {
            // 나중에 구현
            value = 0;

            return GenerateErrorCode(ERR_IO_YMC_NOT_SUPPORT_FUNCTION);
            return SUCCESS;
        }

        public int SetInt16(int addr, Int16 value)
        {
#if SIMULATION_IO
            return SUCCESS;
#endif
            Debug.Assert(INPUT_ORIGIN <= addr && addr <= OUTPUT_END);

            UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            Int16[] Reg_ShortData = new Int16[RegisterDataNumber];  // W or B size register data storage variable
            Int32[] Reg_LongData = new Int32[RegisterDataNumber];       // L size register data storage variable

            Reg_ShortData[0] = value;

            int iResult = SetRegisterData(addr, EYMCDataType.W, RegisterDataNumber, Reg_ShortData, Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SetInt32(int addr, Int32 value)
        {
#if SIMULATION_IO
            return SUCCESS;
#endif
            Debug.Assert(INPUT_ORIGIN <= addr && addr <= OUTPUT_END);

            UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            Int16[] Reg_ShortData = new Int16[RegisterDataNumber];  // W or B size register data storage variable
            Int32[] Reg_LongData = new Int32[RegisterDataNumber];       // L size register data storage variable

            Reg_LongData[0] = value;

            int iResult = SetRegisterData(addr, EYMCDataType.L, RegisterDataNumber, Reg_ShortData, Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SetFloat(int addr, double value)
        {
            // 나중에 구현

            return GenerateErrorCode(ERR_IO_YMC_NOT_SUPPORT_FUNCTION);
            return SUCCESS;
        }

    }
}
