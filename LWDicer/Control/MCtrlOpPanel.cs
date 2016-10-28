using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_CtrlOpPanel;
using static LWDicer.Layers.DEF_OpPanel;
using static LWDicer.Layers.DEF_Thread;

namespace LWDicer.Layers
{
    public class DEF_CtrlOpPanel
    {
        // Touch Panel 앞/뒷면 ID define
        public const int DEF_CTRLOPPANEL_NONE_PANEL_ID = 0;
        public const int DEF_CTRLOPPANEL_FRONT_PANEL_ID = 1;
        public const int DEF_CTRLOPPANEL_BACK_PANEL_ID = 2;

        // Jog 관련 define
        public const int DEF_CTRLOPPANEL_JOG_NO_USE = -1;

        public const double DEF_CTRLOPPANEL_BLINK_RATE = 0.5;

        public enum ELampBuzzerMode
        {
            STEPSTOP,           
            START,              
            RUN,                
            STEPSTOP_ING,       
            ERRORSTOP_ING,      
            CYCLESTOP_ING,      
            PARTSEMPTY,         
            ERRORSTOP_NOBUZZER, 
            PARTSEMPTY_NOBUZZER,
            OP_CALL,            
            RUN_PANEL_NO_EXIST, 
            RUN_TRAFFIC_JAM,    
            ESTOP_PRESSED,      
            NCMC_BUZZER              
        }

        public class CCtrlOpPanelRefComp
        {
            public IIO IO;
            public MOpPanel OpPanel;

            //public CCtrlOpPanelRefComp()
            //{
            //}
            public override string ToString()
            {
                return $"CCtrlOpPanelRefComp : ";
            }
        }

        public class CCtrlOpPanelData
        {
        }
    }

    public class MCtrlOpPanel : MCtrlLayer
    {
        private CCtrlOpPanelRefComp m_RefComp;
        private CCtrlOpPanelData m_Data;

        // Blink Rate
        double m_dBlinkRate;

        // SafeSensor Check 여부
        public bool UseSafeSensor { get; set; } = false;

        // Jog로 이동할 Motion에 대한 정보 Index
        int m_iJogIndex;
        int m_iJogIndexExtra;

        // Jog Key 이전 값 저장 용 변수
        int m_iPrevJogVal_X;
        int m_iPrevJogVal_Y;
        int m_iPrevJogVal_T;
        int m_iPrevJogVal_Z;

        bool m_bPrevDir_X;
        bool m_bPrevDir_Y;
        bool m_bPrevDir_T;
        bool m_bPrevDir_Z;

        // Tower Lamp Blink 동작을 위한 Timer
        MTickTimer m_BlinkTimer = new MTickTimer();

        // Blink Interval 계산을 위한 변수
        bool m_bBlinkState;

        // IO Check Dialog
        bool m_bIOCheck;

        // Panel이 정체되었을때 On
        public bool IsTrafficJam { get; set; } = false;
        // 설비내에 Panel이 없을때 On
        public bool IsNoPanel { get; set; } = false;


        public MCtrlOpPanel(CObjectInfo objInfo, CCtrlOpPanelRefComp refComp, CCtrlOpPanelData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            // Blink Rate 
            m_dBlinkRate = DEF_CTRLOPPANEL_BLINK_RATE;

            // Jog로 이동할 Motion에 대한 정보 Index 
            m_iJogIndex = DEF_CTRLOPPANEL_JOG_NO_USE;
            m_iJogIndexExtra = DEF_OPPANEL_NO_JOGKEY;

            // Blink Interval 계산을 위한 변수 
            m_bBlinkState = false;

            // Blink를 위한 Timer 시작 
            m_BlinkTimer.StartTimer();

            m_bIOCheck = false;

            // Jog Key 이전 값 저장 용 변수 
            m_iPrevJogVal_X = JOG_KEY_NON;
            m_iPrevJogVal_Y = JOG_KEY_NON;
            m_iPrevJogVal_T = JOG_KEY_NON;
            m_iPrevJogVal_Z = JOG_KEY_NON;

            m_bPrevDir_X = DIR_POSITIVE;
            m_bPrevDir_Y = DIR_POSITIVE;
            m_bPrevDir_T = DIR_POSITIVE;
            m_bPrevDir_Z = DIR_POSITIVE;

        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CCtrlOpPanelData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CCtrlOpPanelData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }
        public override int Initialize()
        {
            return SUCCESS;
        }
        #endregion

        /// <summary>
        /// Motion 이동 속도 Mode를 설정한다.
        /// </summary>
        /// <param name="rgdVelocity">설정할 Motion 속도 (배열 Index 순서는 MMC 축 ID 순서)</param>
        public void SetVelocityMode(double[/*DEF_MAX_MOTION_AXIS*/] rgdVelocity)
        {
            m_RefComp.OpPanel.SetVelocityMode(rgdVelocity);
        }

        /// <summary>
        /// 자동운전 전의 운전 가능 상태를 읽는다.
        /// </summary>
        /// <returns></returns>
        public int CheckBeforeAutoRun()
        {
#if SIMULATION_MOTION
            return SUCCESS;
#endif
            bool bStatus;
            bool bEmptyAll, bEmptyPart;
            bool bStatus1 = false;
            bool bStatus2 = false;
            int iResult = SUCCESS;

            // 1. 원점복귀 여부 확인 
            bool[] bOriginSts;
            if (m_RefComp.OpPanel.CheckAllOrigin(out bOriginSts) == false)
                return GenerateErrorCode(ERR_MNGOPPANEL_NOT_ALL_ORIGIN);

            // 2. 초기화 여부 확인 
            bool[] bInitSts;
            if (m_RefComp.OpPanel.CheckAllInit(out bInitSts) == false)
                return GenerateErrorCode(ERR_MNGOPPANEL_NOT_ALL_INIT);

            // 3. Tool CP 확인 
            //	if ((iResult = m_RefComp.OpPanel.GetCPTripStatus(out bStatus);
            //		return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_CP_TRIP);

            // 4. Tank 잔량 확인 
            if (AutoRunMode != EAutoRunMode.DRY_RUN)
            {
                iResult = CheckSiliconeRemain(out bEmptyAll, out bEmptyPart);
                if (iResult != SUCCESS) return iResult;

                if (bEmptyAll == true)
                    return GenerateErrorCode(ERR_MNGOPPANEL_SILICONE_EMPTY_ALL);
            }

            // 5. Door Open 확인 
            if (UseSafeSensor == true)
            {
                iResult = GetDoorSWStatus(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                    return GenerateErrorCode(ERR_MNGOPPANEL_DOOR_OPEN);
            }

            // 6. AMP Fault 상태 확인 
            //iResult = GetMotorAmpFaultStatus(out bStatus);
            //if (iResult != SUCCESS) return iResult;
            //if (bStatus == true)
            //    return GenerateErrorCode(ERR_MNGOPPANEL_AMP_FAULT);

            // 7. Air 확인 
            iResult = m_RefComp.OpPanel.GetAirErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
#if !SIMULATION_TEST
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_AIR_ERROR);
#endif

            // 8. Vacuum 확인 
            iResult = m_RefComp.OpPanel.GetVacuumErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
#if !SIMULATION_TEST
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_VACCUM_ERROR);
#endif

            // 9. EFD 확인 

            // 10. DC POWER 확인 
            iResult = m_RefComp.OpPanel.GetDcPWErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
#if !SIMULATION_TEST
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_DC_POWER_ON_ERROR);
#endif

#if DEF_NEW_CLEAN_SYSTEM
            // 9. N2 확인 
            //	iResult = m_RefComp.OpPanel.GetN2ErrorStatus(out bStatus);
            //	if(iResult) return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_N2_ERROR);

            // 10. Cleaner Detect 확인 
            //	iResult = m_RefComp.OpPanel.GetCleanerDetect1ErrorStatus(out bStatus);
            //	if(iResult) return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_CLEANER_DETECT1_ERROR);

            //	iResult = m_RefComp.OpPanel.GetCleanerDetect2ErrorStatus(out bStatus);
            //	if(iResult) return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_CLEANER_DETECT2_ERROR);
#endif //DEF_NEW_CLEAN_SYSTEM

            return SUCCESS;
        }

        /// <summary>
        /// 자동운전 중의 운전 가능 상태를 읽는다.
        /// </summary>
        /// <param name="bEmptyPart"></param>
        /// <returns></returns>
        public int CheckAutoRun(out bool bEmptyPart)
        {
#if SIMULATION_MOTION
            bEmptyPart = false;
            return SUCCESS;
#endif
            bool bStatus;
            bool bEmptyAll;
            bool bStatus1 = false;
            bool bStatus2 = false;
            int iResult = SUCCESS;
            bEmptyPart = false;

            // 3. Tool CP 확인 
            //	if ((iResult = m_RefComp.OpPanel.GetCPTripStatus(out bStatus);
            //		return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_CP_TRIP);

            // 4. Silicone 잔량 확인 
            if (AutoRunMode != EAutoRunMode.DRY_RUN)
            {
                iResult = CheckSiliconeRemain(out bEmptyAll, out bEmptyPart);
                if (iResult != SUCCESS) return iResult;
                if (bEmptyAll == true)
                    return GenerateErrorCode(ERR_MNGOPPANEL_SILICONE_EMPTY_ALL);

            }

            // 5. Door Open 확인 
            if (UseSafeSensor == true)
            {
                iResult = GetDoorSWStatus(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    EStopAllAxis();
                    return GenerateErrorCode(ERR_MNGOPPANEL_DOOR_OPEN);
                }
            }

#if DEF_USE_AREA_SENSOR
            // 5.1 Area Sensor 확인 
            if (UseSafeSensor == true)
            {
                iResult = GetAreaSWStatus(&bStatus1);
                if (iResult != SUCCESS) return iResult;

                if (false == bStatus1)
                {
                    EStopAllAxis();
                    return GenerateErrorCode(ERR_MNGOPPANEL_FRONT_BACK_AREA_SENSOR_DETECTED_ERROR);
                }

            }
#endif

// 6. AMP Fault 상태 확인 
//iResult = GetMotorAmpFaultStatus(out bStatus);
//if (iResult != SUCCESS) return iResult;
//if (bStatus == true)
//    return GenerateErrorCode(ERR_MNGOPPANEL_AMP_FAULT);

            // 7. Air 확인 
            iResult = m_RefComp.OpPanel.GetAirErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
#if !SIMULATION_TEST
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_AIR_ERROR);
#endif

            // 8. Vacuum 확인 
            iResult = m_RefComp.OpPanel.GetVacuumErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
#if !SIMULATION_TEST
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_VACCUM_ERROR);
#endif

#if DEF_NEW_CLEAN_SYSTEM
            // 9. N2 확인 
            //	iResult = m_RefComp.OpPanel.GetN2ErrorStatus(out bStatus);
            //	if(iResult) return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_N2_ERROR);
#endif
            return SUCCESS;
        }

        /// <summary>
        /// Start Switch의 눌린 상태 읽어온다.
        /// </summary>
        /// <param name="bStatus">Start Switch 눌린 상태 (0:OFF , 1:FRONT , 2:BACK)</param>
        /// <returns></returns>
        public int GetStartSWStatus(out bool bStatus)
        {
            int iResult = SUCCESS;

            // Start Switch 눌린 상태 읽기
            iResult = m_RefComp.OpPanel.GetStartButtonStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        private int TempOnSWStatus(int addr)
        {
            int iResult = m_RefComp.IO.SetBit(addr, true);
            if (iResult != SUCCESS) return iResult;

            Sleep(100);
            iResult = m_RefComp.IO.SetBit(addr, false);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int TempOnStartSWStatus()
        {
            return TempOnSWStatus(DEF_IO.iStart_SWFront);
        }

        public int TempOnStopSWStatus()
        {
            return TempOnSWStatus(DEF_IO.iStop_SWFront);
        }

        public int TempOnResetSWStatus()
        {
            return TempOnSWStatus(DEF_IO.iReset_SWFront);
        }

        public int TempOnEMOSWStatus()
        {
            return TempOnSWStatus(DEF_IO.iEMO_SW);
        }

        /// <summary>
        /// Stop Switch의 눌린 상태 읽어온다.
        /// </summary>
        /// <param name="bStatus">Stop Switch 눌린 상태 (0:OFF , 1:FRONT , 2:BACK)</param>
        /// <returns></returns>
        public int GetStopSWStatus(out bool bStatus)
        {
            int iResult = SUCCESS;

            // Stop Switch 눌린 상태 읽기
            iResult = m_RefComp.OpPanel.GetStopButtonStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;

            if (bStatus == true)
            {
                //		StopAllReturnOrigin();
                //		StopAllAxis();
            }
            return SUCCESS;
        }

        /// <summary>
        /// E-Stop Switch의 눌린 상태 읽어온다.
        /// </summary>
        /// <param name="bStatus">E-Stop Switch 눌린 상태 (true:ON, false:OFF)</param>
        /// <returns></returns>
        public int GetEStopSWStatus(out bool bStatus)
        {
            int iResult = SUCCESS;

            // E-Stop Switch 눌린 상태 읽기
            iResult = m_RefComp.OpPanel.GetEStopButtonStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;

            if (bStatus == true)
            {
                //		StopAllReturnOrigin();
                //		StopAllAxis();
            }
            return SUCCESS;
        }

        /// <summary>
        /// Reset Switch의 눌린 상태 읽어온다.
        /// </summary>
        /// <param name="bStatus">Reset Switch 눌린 상태 (0:OFF , 1:FRONT , 2:BACK)</param>
        /// <returns></returns>
        public int GetResetSWStatus(out bool bStatus)
        {
            int iResult = SUCCESS;

            // Reset Switch 눌린 상태 읽기
            iResult = m_RefComp.OpPanel.GetResetButtonStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// Door의 상태를 읽어온다.
        /// </summary>
        /// <param name="bStatus">Door 상태 (true:CLOSE, false:OPEN)</param>
        /// <returns></returns>
        public int GetDoorSWStatus(out bool bStatus)
        {
            int iResult = SUCCESS;

            // Door 열린 상태 읽기
            iResult = m_RefComp.OpPanel.GetSafeDoorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// Area의 상태를 읽어온다.
        /// </summary>
        /// <param name="bStatus">Area 상태 (true:CLOSE, false:OPEN)</param>
        /// <returns></returns>
        public int GetAreaSWStatus(out bool bStatus)
        {
            int iResult = SUCCESS;

            iResult = m_RefComp.OpPanel.GetAreaFrontBackStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }


        /// <summary>
        /// Motor AMP Fault 상태를 읽는다.
        /// </summary>
        /// <param name="bStatus">Motor AMP Fault의 상태 (true : Fault, false : Normal)</param>
        /// <returns></returns>
        public int GetMotorAmpFaultStatus(int servoIndex, out bool bStatus)
        {
            return m_RefComp.OpPanel.GetMotorAmpFaultStatus(servoIndex, out bStatus);
        }

        /// <summary>
        /// Motion Power Relay On/Off 를 설정한다.
        /// </summary>
        /// <param name="bStatus">Motion Power Relay의 동작 (true : ON, false : OFF)</param>
        /// <returns></returns>
        public int SetMotionPowerRelayStatus(bool bStatus)
        {
            return m_RefComp.OpPanel.SetMotionPowerRelayStatus(bStatus);
        }

        /// <summary>
        /// OpPanel의 Switch 및 Tower Lamp의 On/Off를 지시한다.
        /// 
        ///    Mode                     Start	Step	TowerR	TowerY	TowerG	Buzzer
        /// 1:Step Stop 완료 상태		  X		 O		   O	  X		  X		  X
        /// 2:Start(Run Ready) 상태		  O		 X		   X	  O		  O		  X
        /// 3:Run 상태					  O		 X		   X	  X		  O		  X
        /// 4:Step Stop 진행 상태		  X		 O		   B	  X		  X		  X
        /// 5:Error Stop 진행 상태		  O		 X		   B	  X		  B		  O
        /// 6:Cycle Stop 진행 상태		  O		 X		   X	  X		  B		  X
        /// 7:Parts Empty 상태			  O		 X		   X	  B		  B		  B
        ///   (Operator Call 상태)
        /// 8:Error Stop 진행 상태		  O		 X		   B	  X		  B		  X
        ///   (Buzzer No)
        /// 9:Parts Empty 상태			  O		 X		   X	  B		  B		  B
        ///   (Buzzer No)
        /// 10: 정체 상태				  0		 X		   O	  O		  O		  X
        /// 
        /// </summary>
        /// <param name="towerLampMode"></param>
        /// <returns></returns>
        public int SetOpPanel(ELampBuzzerMode towerLampMode)
        {
            int iResult = SUCCESS;

            if (m_bIOCheck) return iResult;

            // Blink Interval 계산 
            if (m_BlinkTimer.MoreThan(m_dBlinkRate, ETimeType.SECOND) == true)
            {
                // Blink 반전 
                m_bBlinkState = !m_bBlinkState;
#if !SIMULATION_TEST
                // Timer 재기동 
                m_BlinkTimer.StartTimer();
#endif
            }

            iResult = m_RefComp.OpPanel.SetResetLamp(false);
            if (iResult != SUCCESS) return iResult;

            // Switch LED 및 Tower Lamp의 On/Off Mode에 따라 동작 
            switch (towerLampMode)
            {
                // Step Stop 진행 상태 
                case ELampBuzzerMode.STEPSTOP_ING:
                    // Start Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStartLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStopLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Step Stop 완료 상태 
                case ELampBuzzerMode.STEPSTOP:
                    // Start Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStartLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStopLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp On 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Cycle Stop 진행 상태 
                case ELampBuzzerMode.CYCLESTOP_ING:
                    // Start Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Error Stop 진행 상태 
                case ELampBuzzerMode.ERRORSTOP_ING:
                    // Start Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(true, DEF_OPPANEL_BUZZER_K2);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Error Stop 확인 상태 
                case ELampBuzzerMode.ERRORSTOP_NOBUZZER:
                    // Start Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Start (Run Ready) 상태 
                case ELampBuzzerMode.START:
                    // Start Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp On 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Run 상태 
                case ELampBuzzerMode.RUN:
                    // Start Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Run 상태 - but Panel non exist 
                case ELampBuzzerMode.RUN_PANEL_NO_EXIST:
                    // Start Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Run 상태 - but Panel Traffic Jam 
                case ELampBuzzerMode.RUN_TRAFFIC_JAM:
                    // Start Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Run 상태 - but Panel Traffic Jam 
                case ELampBuzzerMode.ESTOP_PRESSED:
                    // Start Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStartLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStopLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Parts Empty 상태 
                case ELampBuzzerMode.PARTSEMPTY:
                    // Start Switch LED On 
                    // 		iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    // 		if(iResult) return iResult;
                    // Step Stop Switch LED Off 
                    // 		iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    // 		if(iResult) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(true, DEF_OPPANEL_BUZZER_K4);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Parts Empty 상태 
                case ELampBuzzerMode.PARTSEMPTY_NOBUZZER:
                    // Start Switch LED On 
                    // 		iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    // 		if(iResult) return iResult;
                    // Step Stop Switch LED Off 
                    // 		iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    // 		if(iResult) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(false, DEF_OPPANEL_BUZZER_ALL);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Line Controller로 부터 Op Call이 온 상태 
                case ELampBuzzerMode.OP_CALL:
                    // Start Switch LED On 
                    iResult = m_RefComp.OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_RefComp.OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerRedLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerYellowLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Blink 
                    iResult = m_RefComp.OpPanel.SetTowerGreenLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_RefComp.OpPanel.SetBuzzerStatus(true, DEF_OPPANEL_BUZZER_K2);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // NSMC 상태
                case ELampBuzzerMode.NCMC_BUZZER:
                    if ((iResult = m_RefComp.OpPanel.SetStartLamp(true)) != SUCCESS) return iResult;
                    if ((iResult = m_RefComp.OpPanel.SetStopLamp(false)) != SUCCESS) return iResult;
                    if ((iResult = m_RefComp.OpPanel.SetTowerRedLamp(m_bBlinkState)) != SUCCESS) return iResult;
                    if ((iResult = m_RefComp.OpPanel.SetTowerYellowLamp(m_bBlinkState)) != SUCCESS) return iResult;
                    if ((iResult = m_RefComp.OpPanel.SetTowerGreenLamp(m_bBlinkState)) != SUCCESS) return iResult;
                    if ((iResult = m_RefComp.OpPanel.SetBuzzerStatus(true, DEF_OPPANEL_BUZZER_K2)) != SUCCESS) return iResult;
                    break;
                default:
                    return GenerateErrorCode(ERR_MNGOPPANEL_INVALID_SET_OPPANEL_STATE);
            }

            return SUCCESS;
        }

        /// <summary>
        /// Jog Key 확인하여 해당 Jog 축을 이동/정지한다.
        /// </summary>
        /// <returns></returns>
        public int MoveJog()
        {
            int iResult = SUCCESS;

            // Jog Key 값 변수
            bool bXpStatus = false;
            bool bXnStatus = false;
            bool bYpStatus = false;
            bool bYnStatus = false;
            bool bTpStatus = false;
            bool bTnStatus = false;
            bool bZpStatus = false;
            bool bZnStatus = false;

            // Jog Key Check 
            // X(+) Key Read 
            iResult = m_RefComp.OpPanel.GetJogXPlusButtonStatus(out bXpStatus);
            if (iResult != SUCCESS) return iResult;
            // X(-) Key Read 
            iResult = m_RefComp.OpPanel.GetJogXMinusButtonStatus(out bXnStatus);
            if (iResult != SUCCESS) return iResult;
            // Y(+) Key Read 
            iResult = m_RefComp.OpPanel.GetJogYPlusButtonStatus(out bYpStatus);
            if (iResult != SUCCESS) return iResult;
            // Y(-) Key Read 
            iResult = m_RefComp.OpPanel.GetJogYMinusButtonStatus(out bYnStatus);
            if (iResult != SUCCESS) return iResult;
            // T(+) Key Read 
            iResult = m_RefComp.OpPanel.GetJogTPlusButtonStatus(out bTpStatus);
            if (iResult != SUCCESS) return iResult;
            // T(-) Key Read 
            iResult = m_RefComp.OpPanel.GetJogTMinusButtonStatus(out bTnStatus);
            if (iResult != SUCCESS) return iResult;
            // Z(+) Key Read 
            iResult = m_RefComp.OpPanel.GetJogZPlusButtonStatus(out bZpStatus);
            if (iResult != SUCCESS) return iResult;
            // Z(-) Key Read 
            iResult = m_RefComp.OpPanel.GetJogZMinusButtonStatus(out bZnStatus);
            if (iResult != SUCCESS) return iResult;

            // 이동 모드이면 Jog 이동 
            if (m_iJogIndex > DEF_CTRLOPPANEL_JOG_NO_USE)
            {
                // X(+) Pitch 이동 
                if (bXpStatus && !bXnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_X != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_X = JOG_KEY_POS;
                        m_bPrevDir_X = DIR_POSITIVE;
                        //m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
                    }
                    // X(+) 방향으로 Pitch 이동 실시 
                    m_RefComp.OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X, DIR_POSITIVE);
                }
                // X(-) Pitch 이동 
                else if (!bXpStatus && bXnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_X != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_X = JOG_KEY_POS;
                        m_bPrevDir_X = DIR_NEGATIVE;
                        //m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
                    }
                    // X(-) 방향으로 Pitch 이동 실시 
                    m_RefComp.OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X, DIR_NEGATIVE);
                }
                // X(+/-) Velocity 이동 
                else if (bXpStatus && bXnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_X != JOG_KEY_ALL)
                    {
                        m_iPrevJogVal_X = JOG_KEY_ALL;
                        //m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
                    }
                    // X(+/) 방향으로 Velocity 이동 실시 
                    m_RefComp.OpPanel.MoveJogVelocity(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X, m_bPrevDir_X);
                }
                // 아무 것도 안 눌렸을 때 
                else
                {
                    // 처음 안 눌리는 것이면 
                    if (m_iPrevJogVal_X != JOG_KEY_NON)
                    {
                        m_iPrevJogVal_X = JOG_KEY_NON;
                        m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
                    }
                }


                // Y(+) Pitch 이동 
                if (bYpStatus && !bYnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Y != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_Y = JOG_KEY_POS;
                        m_bPrevDir_Y = DIR_POSITIVE;
                        //m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
                    }
                    // Y(+) 방향으로 Pitch 이동 실시 
                    m_RefComp.OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y, DIR_POSITIVE);
                }
                // Y(-) Pitch 이동 
                else if (!bYpStatus && bYnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Y != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_Y = JOG_KEY_POS;
                        m_bPrevDir_Y = DIR_NEGATIVE;
                        //m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
                    }
                    // Y(-) 방향으로 Pitch 이동 실시 
                    m_RefComp.OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y, DIR_NEGATIVE);
                }
                // Y(+/-) Velocity 이동 
                else if (bYpStatus && bYnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Y != JOG_KEY_ALL)
                    {
                        m_iPrevJogVal_Y = JOG_KEY_ALL;
                        //m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
                    }
                    // Y(+/) 방향으로 Velocity 이동 실시 
                    m_RefComp.OpPanel.MoveJogVelocity(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y, m_bPrevDir_Y);
                }
                // 아무 것도 안 눌렸을 때 
                else
                {
                    // 처음 안 눌리는 것이면 
                    if (m_iPrevJogVal_Y != JOG_KEY_NON)
                    {
                        m_iPrevJogVal_Y = JOG_KEY_NON;
                        m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
                    }
                }


                // T(+) Pitch 이동 
                if (bTpStatus && !bTnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_T != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_T = JOG_KEY_POS;
                        m_bPrevDir_T = DIR_POSITIVE;
                        //m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
                    }
                    // T(+) 방향으로 Pitch 이동 실시 
                    m_RefComp.OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T, DIR_POSITIVE);
                }
                // T(-) Pitch 이동 
                else if (!bTpStatus && bTnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_T != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_T = JOG_KEY_POS;
                        m_bPrevDir_T = DIR_NEGATIVE;
                        //m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
                    }
                    // T(-) 방향으로 Pitch 이동 실시 
                    m_RefComp.OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T, DIR_NEGATIVE);
                }
                // T(+/-) Velocity 이동 
                else if (bTpStatus && bTnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_T != JOG_KEY_ALL)
                    {
                        m_iPrevJogVal_T = JOG_KEY_ALL;
                        //m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
                    }
                    // T(+/) 방향으로 Velocity 이동 실시 
                    m_RefComp.OpPanel.MoveJogVelocity(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T, m_bPrevDir_T);
                }
                // 아무 것도 안 눌렸을 때 
                else
                {
                    // 처음 안 눌리는 것이면 
                    if (m_iPrevJogVal_T != JOG_KEY_NON)
                    {
                        m_iPrevJogVal_T = JOG_KEY_NON;
                        m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
                    }
                }


                // Z(+) Pitch 이동 
                if (bZpStatus && !bZnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Z != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_Z = JOG_KEY_POS;
                        m_bPrevDir_Z = DIR_POSITIVE;
                        //				m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
                    }
                    // Z(+) 방향으로 Pitch 이동 실시 
                    m_RefComp.OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z, DIR_POSITIVE);
                }
                // Z(-) Pitch 이동 
                else if (!bZpStatus && bZnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Z != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_Z = JOG_KEY_POS;
                        m_bPrevDir_Z = DIR_NEGATIVE;
                        //				m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
                    }
                    // Z(-) 방향으로 Pitch 이동 실시 
                    m_RefComp.OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z, DIR_NEGATIVE);
                }
                // Z(+/-) Velocity 이동 
                else if (bZpStatus && bZnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Z != JOG_KEY_ALL)
                    {
                        m_iPrevJogVal_Z = JOG_KEY_ALL;
                        //				m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
                    }
                    // Z(+/) 방향으로 Velocity 이동 실시 
                    m_RefComp.OpPanel.MoveJogVelocity(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z, m_bPrevDir_Z);
                }
                // 아무 것도 안 눌렸을 때 
                else
                {
                    // 처음 안 눌리는 것이면 
                    if (m_iPrevJogVal_Z != JOG_KEY_NON)
                    {
                        m_iPrevJogVal_Z = JOG_KEY_NON;
                        m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
                    }
                }
            }
            // 이동 모드 아니면 Touch Panel 전환 점검 
            else
            {
                // X(-)와 Y(+)가 동시에 눌리면 
                if (bXnStatus && bYpStatus)
                {
                    // Touch Panel 앞면으로 전환 
                    iResult = m_RefComp.OpPanel.ChangeOpPanel(DEF_CTRLOPPANEL_FRONT_PANEL_ID);
                    if (iResult != SUCCESS) return iResult;
                }
                // X(+)와 Y(-)가 동시에 눌리면 
                else if (bXpStatus && bYnStatus)
                {
                    // Touch Panel 뒷면으로 전환 
                    iResult = m_RefComp.OpPanel.ChangeOpPanel(DEF_CTRLOPPANEL_BACK_PANEL_ID);
                    if (iResult != SUCCESS) return iResult;
                }
            }

            return SUCCESS;
        }

        /// <summary>
        /// 모든 축들에 대해 원점복귀 동작을 정지한다.
        /// </summary>
        /// <returns></returns>
        public int StopAllReturnOrigin()
        {
            return m_RefComp.OpPanel.StopAllReturnOrigin();
        }

        /// <summary>
        /// 모든 축들에 대해 Servo AMP를 Enable한다.
        /// </summary>
        /// <returns></returns>
        public int AllServoOn()
        {
            return m_RefComp.OpPanel.AllServoOn();
        }

        /// <summary>
        /// 모든 축들에 대해 Servo AMP를 Disable한다.
        /// </summary>
        /// <returns></returns>
        public int AllServoOff()
        {
            return m_RefComp.OpPanel.AllServoOff();
        }

        public int ServoOn(int index)
        {
            return m_RefComp.OpPanel.ServoOn(index);
        }

        public int ServoOff(int index)
        {
            return m_RefComp.OpPanel.ServoOff(index);
        }

        /// <summary>
        /// 모든 축들에 대해 동작을 정지한다.
        /// </summary>
        /// <returns></returns>
        public int StopAllAxis()
        {
            return m_RefComp.OpPanel.StopAllAxis();
        }

        /// <summary>
        /// 모든 축들에 대해 동작을 E-STOP 정지한다.
        /// </summary>
        /// <returns></returns>
        public int EStopAllAxis()
        {
            return m_RefComp.OpPanel.EStopAllAxis();
        }

        /// <summary>
        /// Tower Lamp Blink 속도 설정하기
        /// </summary>
        /// <param name="dRate">(OPTION=0.5) 설정할 Blink 속도</param>
        public void SetBlinkRate(double dRate)
        {
            m_dBlinkRate = dRate;
        }

        /// <summary>
        /// Jog에 사용할 Unit Index를 설정한다.
        /// </summary>
        /// <param name="iUnitIndex">(OPTION=-1) Jog에 사용할 Unit Index</param>
        public void SetJogUnit(int iUnitIndex)
        {
            // Jog로 이동할 Motion에 대한 정보 Index
            m_iJogIndex = iUnitIndex;

            COpPanelIOAddr sOpPanelIO = new COpPanelIOAddr();
            m_RefComp.OpPanel.GetIOAddress(ref sOpPanelIO);
            //if (m_iJogIndex == DEF_JOG_CAMERA1)
            //{
            //    sOpPanelIO.FrontPanel.XpInputAddr = iJog_X_Backward_SWFront;
            //    sOpPanelIO.FrontPanel.XnInputAddr = iJog_X_Forward_SWFront;

            //    sOpPanelIO.BackPanel.XpInputAddr = iJog_X_Backward_SWRear;
            //    sOpPanelIO.BackPanel.XnInputAddr = iJog_X_Forward_SWRear;
            //}
            //else
            //{
            //    sOpPanelIO.FrontPanel.XpInputAddr = iJog_X_Forward_SWFront;
            //    sOpPanelIO.FrontPanel.XnInputAddr = iJog_X_Backward_SWFront;

            //    sOpPanelIO.BackPanel.XpInputAddr = iJog_X_Forward_SWRear;
            //    sOpPanelIO.BackPanel.XnInputAddr = iJog_X_Backward_SWRear;
            //}
            m_RefComp.OpPanel.SetIOAddress(sOpPanelIO);
        }

        /// <summary>
        /// Jog에 사용할 Unit Index를 설정한다.
        /// </summary>
        /// <param name="iUnitIndex">(OPTION=-1) Jog에 사용할 Unit Index</param>
        public void SetJogUnitExtra(int iUnitIndex)
        {
            // Jog로 이동할 Motion에 대한 정보 Index
            if (iUnitIndex != m_iJogIndex)
            {
            m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
            m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
            m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
            m_RefComp.OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
            //		m_RefComp.OpPanel.StopAllAxis();
            }

            // Jog로 이동할 Motion에 대한 정보 Index
            m_iJogIndexExtra = iUnitIndex;
        }

        /// <summary>
        /// 설정된 Jog에 사용할 Unit Index를 읽는다.
        /// </summary>
        /// <returns></returns>
        public int GetJogUnit()
        {
            // Jog로 이동할 Motion에 대한 정보 Index
            return m_iJogIndex;
        }

        /// <summary>
        /// 설정된 Jog에 사용할 Unit Index를 읽는다.
        /// </summary>
        /// <returns></returns>
        public int GetJogUnitExtra()
        {
            // Jog로 이동할 Motion에 대한 정보 Index
            return m_iJogIndexExtra;
        }

        public int OriginReturn(int index)
        {
            return m_RefComp.OpPanel.OriginReturn(index);
        }
        /// <summary>
        /// Unit의 원점복귀 Flag를 설정한다.
        /// </summary>
        public void ResetAllOriginFlag()
        {
            m_RefComp.OpPanel.ResetAllOriginFlag();
        }

        public void ResetAllInitFlag()
        {
            int i = 0;
            for (i = 0; i < (int)EThreadUnit.MAX ; i++)
                m_RefComp.OpPanel.SetInitFlag(i, false);
        }

        public int CheckSiliconeRemain(out bool pbEmptyAll, out bool pbEmptyPart)
        {
            int iResult = SUCCESS;

            iResult = m_RefComp.OpPanel.CheckAllTank_Empty(out pbEmptyAll, out pbEmptyPart);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public void SetIOCheck(bool bCheck)
        {
            m_bIOCheck = bCheck;
        }

        public int SetAlarmForAlignMsg(bool bSet)
        {
            int iResult = SUCCESS;
            m_bIOCheck = bSet;
            if (bSet == true)
            {
                // Tower Lamp Buzzer On
                iResult = m_RefComp.OpPanel.SetBuzzerStatus(true, DEF_OPPANEL_BUZZER_K3);
                if (iResult != SUCCESS) return iResult;
            }

            return iResult;
        }

        public bool SetMotionStatus()
        {
            //bool bStatus;
            //if (m_RefComp.OpPanel.SetTouchStatus(&bStatus)) return true;
            //else return false;
            return true;
        }

        public int CheckSafetyBeforeAxisMove()
        {
#if SIMULATION_MOTION
            return SUCCESS;
#endif
            bool bStatus;

            m_RefComp.OpPanel.GetEStopButtonStatus(out bStatus);
            if (bStatus == true)
            {
                return GenerateErrorCode(ERR_MNGOPPANEL_EMERGENCY);
            }

            m_RefComp.OpPanel.GetSafeDoorStatus(out bStatus);
            if (bStatus == true && UseSafeSensor == true)
            {
                return GenerateErrorCode(ERR_MNGOPPANEL_DOOR_OPEN);
            }

            bool[] bOriginSts;
            bStatus = m_RefComp.OpPanel.CheckAllOrigin(out bOriginSts);
            if (bStatus == false)
            {
                return GenerateErrorCode(ERR_MNGOPPANEL_NOT_ALL_ORIGIN);
            }

            return SUCCESS;
        }

        public int CheckSafetyBeforeCylinderMove()
        {
#if SIMULATION_MOTION
            return SUCCESS;
#endif
            bool bStatus;

            //	m_RefComp.OpPanel.GetEStopButtonStatus(out bStatus);
            //	if(bStatus == true)
            //	{
            //		return GenerateErrorCode(ERR_MNGOPPANEL_EMERGENCY);
            //	}

            m_RefComp.OpPanel.GetSafeDoorStatus(out bStatus);
            if (bStatus == true && UseSafeSensor == true)
            {
                return GenerateErrorCode(ERR_MNGOPPANEL_DOOR_OPEN);
            }

            return SUCCESS;
        }
        
    }
}
