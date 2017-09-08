using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_OpPanel;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_IO;

namespace Core.Layers
{
    public class DEF_OpPanel
    {
        public const int DEF_OPPANEL_NONE_PANEL_ID = 0;
        public const int DEF_OPPANEL_FRONT_PANEL_ID = 1;
        public const int DEF_OPPANEL_BACK_PANEL_ID = 2;

        // Jog 관련 define
        public const int DEF_OPPANEL_NO_JOGKEY = -1;
        public const int DEF_OPPANEL_JOG_X_KEY = 0;
        public const int DEF_OPPANEL_JOG_Y_KEY = 1;
        public const int DEF_OPPANEL_JOG_T_KEY = 2;
        public const int DEF_OPPANEL_JOG_Z_KEY = 3;

        // Tower Lamp 관련 define
        public enum EBuzzer
        {
            ALL = -1,
            K1,
            K2,
            K3,
            K4,
            MAX,
        }

        // Max. Value
        public const int DEF_OPPANEL_MAX_JOG_LIST = 16;
        //public const int DEF_OPPANEL_MAX_ESTOP_RELAY_NO = 2;
        public const int DEF_OPPANEL_MAX_CP_TRIP_NO = 16;

        //public const int DEF_OPPANEL_MAX_DOOR_SENSOR = 7;
        //        public const int DEF_OPPANEL_MAX_DOOR_GROUP = 1;

        /// <summary>
        /// DEF_OPPANEL_MAX_DOOR_GROUP 대신에 DoorGroup을 선언해서 사용
        /// </summary>
        public enum EDoorGroup
        {
            ALL = -1,
            FRONT = 0,
            LEFT,
            RIGHT,
            BACK,
            MAX,
        }

        /// <summary>
        /// DEF_OPPANEL_MAX_DOOR_SENSOR 대신에 DoorIndex를 선언해서 사용
        /// </summary>
        public enum EDoorIndex
        {
            ALL = -1,
            INDEX_1 = 0,
            INDEX_2,
            INDEX_3,
            INDEX_4,
            INDEX_5,
            MAX,
        }

        public enum EAreaSensor
        {
            ALL = -1,
            FRONT,
            LEFT,
            RIGHT,
            BACK,
            MAX,
        }

        public enum ESwitch
        {
            NONE = -1,

            // Switch
            START,
            STOP,
            RESET,
            ESTOP,

            // Jog
            JOG_XP, // X Positive
            JOG_XN, // X Negative
            JOG_YP,
            JOG_YN,
            JOG_TP,
            JOG_TN,
            JOG_ZP,
            JOG_ZN,

            MAX,
        }

        public enum EOPPanel
        {
            FRONT,
            BACK,
            TP, // Teach Pendant
            MAX,
        }

        public class CPanelIOAddr
        {
            // Push Switch IO Address
            public int[] RunInputAddr    = new int[(int)EOPPanel.MAX];
            public int[] StopInputAddr   = new int[(int)EOPPanel.MAX];
            public int[] ResetInputAddr  = new int[(int)EOPPanel.MAX];
            public int[] EStopInputAddr  = new int[(int)EOPPanel.MAX];

            // Switch LED IO Address
            public int[] RunOutputAddr   = new int[(int)EOPPanel.MAX];
            public int[] StopOutputAddr  = new int[(int)EOPPanel.MAX];
            public int[] ResetOutputAddr = new int[(int)EOPPanel.MAX];

            // Jog +,- Switch IO Address
            public int[] XpInputAddr     = new int[(int)EOPPanel.MAX];
            public int[] XnInputAddr     = new int[(int)EOPPanel.MAX];
            public int[] YpInputAddr     = new int[(int)EOPPanel.MAX];
            public int[] YnInputAddr     = new int[(int)EOPPanel.MAX];

            public int[] TpInputAddr     = new int[(int)EOPPanel.MAX];
            public int[] TnInputAddr     = new int[(int)EOPPanel.MAX];
            public int[] ZpInputAddr     = new int[(int)EOPPanel.MAX];
            public int[] ZnInputAddr     = new int[(int)EOPPanel.MAX];

            public CPanelIOAddr()
            {
                // Push Switch IO Address
                ArrayExtensions.Init(RunInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(StopInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(ResetInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(EStopInputAddr, IO_ADDR_NOT_DEFINED);

                // Switch LED IO Address
                ArrayExtensions.Init(RunOutputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(StopOutputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(ResetOutputAddr, IO_ADDR_NOT_DEFINED);

                // Jog +,- Switch IO Address
                ArrayExtensions.Init(XpInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(XnInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(YpInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(YnInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(TpInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(TnInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(ZpInputAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(ZnInputAddr, IO_ADDR_NOT_DEFINED);
            }
        }

        /// <summary>
        /// This structure is defined configuration of tower lamp.
        /// </summary>
        public class CTowerIOAddr
        {
            // Tower Lamp IO Address
            public int RedLampAddr;
            public int YellowLampAddr;
            public int GreenLampAddr;
            // Buzzer Operate IO Address
            public int[] BuzzerArray = new int[(int)EBuzzer.MAX];

            public CTowerIOAddr()
            {
            }

            public CTowerIOAddr(int RedLampAddr, int YellowLampAddr, int GreenLampAddr, params int[] BuzzerArray)
            {
                this.RedLampAddr = RedLampAddr;
                this.YellowLampAddr = YellowLampAddr;
                this.GreenLampAddr = GreenLampAddr;
                ArrayExtensions.Init(BuzzerArray, -1);
                for (int i = 0; i < (int)EBuzzer.MAX && i < BuzzerArray.Length; i++)
                {
                    this.BuzzerArray[i] = BuzzerArray[i];
                }
            }

        }

        /// <summary>
        /// This structure is defined configuration of all op-panel.
        /// </summary>
        public class COpPanelIOAddr
        {
            // Panel IO Address Table 
            public CPanelIOAddr OpPanel = new CPanelIOAddr();

            // Tower Lamp IO Address Table 
            public CTowerIOAddr TowerLamp = new CTowerIOAddr();

            // Touch Panel 선택 IO Address 
            public int TouchSelectAddr;

            // 판넬 마크용 외부모니터 선택 IO Address 
            public int MarkMoniterAddr1;
            public int MarkMoniterAddr2;

            // 안전센서 (Door) IO Address 
            public int[,] SafeDoorAddr = new int[(int)EDoorGroup.MAX, (int)EDoorIndex.MAX];
            // Area Sensor
            public int[] AreaSensorAddr = new int[(int)EAreaSensor.MAX];


            // Air Check IO Address 
            public int MainAirAddr;
            public int SubAirAddr;

            // Vacuum Check IO Address 
            public int MainVacuumAddr;
            public int SubVacuumAddr;

            // N2
            public int MainN2Addr;

            // CP Trip IO Address 
            public int[] CPTripArray = new int[DEF_OPPANEL_MAX_CP_TRIP_NO];

            // Cleaner Detect IO Address 
            public int CleanerDetect1Addr;
            public int CleanerDetect2Addr;

            // EFD Ready Check IO Address 
            public int EFDReadyS1Addr;
            public int EFDReadyS2Addr;
            public int EFDReadyG1Addr;
            public int EFDReadyG2Addr;

            // DC Power Check IO Address 
            public int DcPowerAddr;

            public int[] TankEmptyAddr = new int[(int)ECoatTank.MAX];  // 탱크 주소

            public COpPanelIOAddr()
            {
                ArrayExtensions.Init(SafeDoorAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(AreaSensorAddr, IO_ADDR_NOT_DEFINED);
                ArrayExtensions.Init(TankEmptyAddr, IO_ADDR_NOT_DEFINED);
            }
        }

        /// <summary>
        /// This structure is defined one configuration of jog moving table.
        /// </summary>
        public class CJogMotion
        {
            // Jog로 움직일 Motion 대상
            public IMultiAxes m_plnkJog;

            // Jog Key로 움직일 Motion 축
            public int AxisIndex;
        }

        /// <summary>
        /// This structure is defined one configuration of jog moving table.
        /// </summary>
        public class CJogMotionTable
        {
            // Jog Key(X +/-)로 움직일 Motion 축
            public CJogMotion m_XKey = new CJogMotion();
            public CJogMotion m_YKey = new CJogMotion();
            public CJogMotion m_TKey = new CJogMotion();
            public CJogMotion m_ZKey = new CJogMotion();

            public CJogMotionTable()
            {

            }
        }

        /// <summary>
        /// This structure is defined all configuration of jog moving table.
        /// </summary>
        public class CJogTable
        {
            // Jog로 움직일 대상의 개수
            public int ListNo;

            // Jog로 움직일 Motion에 대한 정보 List
            public CJogMotionTable[] MotionArray = new CJogMotionTable[DEF_OPPANEL_MAX_JOG_LIST];

            public CJogTable()
            {
                for (int i = 0; i < MotionArray.Length; i++)
                {
                    MotionArray[i] = new CJogMotionTable();
                }
            }
        }

        public class COpPanelRefComp
        {
            public IIO IO;
            public MACS ACS_Motion;
        }

        public class COpPanelData
        {
            // Use Door Sensor : if true, check door status whether opened.
            public bool[,] UseDoorStatus = new bool[(int)EDoorGroup.MAX, (int)EDoorIndex.MAX];
            // Use Area Sensor
            public bool[] UseAreaSensor = new bool[(int)EAreaSensor.MAX];

            public bool[] UseTankAlarm = new bool[(int)ECoatTank.MAX]; // 자재 교체요청 알람 사용 여부
            
            public COpPanelData()
            {
                ArrayExtensions.Init(UseTankAlarm, false);
                ArrayExtensions.Init(UseDoorStatus, false);
                ArrayExtensions.Init(UseAreaSensor, false);
            }
        }
    }

    public class MOpPanel : MMechanicalLayer
    {
        private COpPanelRefComp m_RefComp;
        private COpPanelData m_Data;

        COpPanelIOAddr m_IOAddrTable;

        CJogTable m_JogTable;

        // Unit 초기화 Flag
        bool[] m_bInitFlag = new bool[(int)EThreadUnit.MAX];

        // Switch Status (previous)
        bool m_bRunSWOld;
        bool m_bStopSWOld;
        bool m_bEStopSWOld;
        bool m_bResetSWOld;
        bool m_bDoorSWOld;
        bool m_bCPTripOld;
        bool m_bAirErrOld;

        // Jog Status (previous)
        bool m_bJogXpOld;
        bool m_bJogXnOld;
        bool m_bJogXOld; // Jog Key로 이동한 마지막 방향

        bool m_bJogYpOld;
        bool m_bJogYnOld;
        bool m_bJogYOld;

        bool m_bJogTpOld;
        bool m_bJogTnOld;
        bool m_bJogTOld;

        bool m_bJogZpOld;
        bool m_bJogZnOld;
        bool m_bJogZOld;

        public MOpPanel(CObjectInfo objInfo, COpPanelRefComp refComp, COpPanelData data,
            COpPanelIOAddr sPanelIOAddr, CJogTable sJogTable)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
            m_IOAddrTable = ObjectExtensions.Copy(sPanelIOAddr);
            m_JogTable = ObjectExtensions.Copy(sJogTable);

            m_JogTable.ListNo = 10;

            // 사용하지 않는 정보에 대한 부분 초기화
            for (int i = m_JogTable.ListNo; i < DEF_OPPANEL_MAX_JOG_LIST; i++)
            {
                m_JogTable.MotionArray[i].m_XKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_XKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_YKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_YKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_TKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_TKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_ZKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_ZKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
            }

            // Unit의 초기화 Flag를 Reset해야 한다.
            ArrayExtensions.Init(m_bInitFlag, false);

            // SW 상태 (previous)
            m_bRunSWOld = false;
            m_bStopSWOld = false;
            m_bEStopSWOld = false;
            m_bResetSWOld = false;
            m_bDoorSWOld = false;
            m_bCPTripOld = false;
            m_bAirErrOld = false;

            // Jog status (previous)
            m_bJogXOld = false;
            m_bJogYOld = false;
            m_bJogTOld = false;
            m_bJogZOld = false;
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(COpPanelData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out COpPanelData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public void SetIOAddress(COpPanelIOAddr opIOAddress)
        {
            // IO Address Table 설정
            m_IOAddrTable = ObjectExtensions.Copy(opIOAddress);
        }

        public void GetIOAddress(ref COpPanelIOAddr pOpPanelIOAddr)
        {
            // IO Address Table 전달
            pOpPanelIOAddr = ObjectExtensions.Copy(m_IOAddrTable);
        }

        public void SetJogTable(CJogTable sJogTable)
        {
            // Jog 정보 설정
            m_JogTable = ObjectExtensions.Copy(sJogTable);

            // 사용하지 않는 정보에 대한 부분 초기화
            for (int i = m_JogTable.ListNo; i < DEF_OPPANEL_MAX_JOG_LIST; i++)
            {
                m_JogTable.MotionArray[i].m_XKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_XKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_YKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_YKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_TKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_TKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_ZKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_ZKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
            }
        }

        public void GetJogTable(out CJogTable sJogTable)
        {
            // Jog 정보 설정
            sJogTable = ObjectExtensions.Copy(m_JogTable);

            // 사용하지 않는 정보에 대한 부분 초기화
            for (int i = m_JogTable.ListNo; i < DEF_OPPANEL_MAX_JOG_LIST; i++)
            {
                sJogTable.MotionArray[i].m_XKey.m_plnkJog = null;
                sJogTable.MotionArray[i].m_XKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                sJogTable.MotionArray[i].m_YKey.m_plnkJog = null;
                sJogTable.MotionArray[i].m_YKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                sJogTable.MotionArray[i].m_TKey.m_plnkJog = null;
                sJogTable.MotionArray[i].m_TKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                sJogTable.MotionArray[i].m_ZKey.m_plnkJog = null;
                sJogTable.MotionArray[i].m_ZKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
            }
        }
        #endregion

        /// <summary>
        /// Pitch 단위의 Jog 이동 동작을 수행한다.
        /// </summary>
        /// <param name="iUnitIndex">Jog로 움직일 Motion에 대한 정보 Table의 Index</param>
        /// <param name="iUnitIndex2"></param>
        /// <param name="iKey">이동할 Jog Key 종류 (0:X, 1:Y, 2:T, 3:Z)</param>
        /// <param name="bDir">이동할 방향 (true: +, false: -)</param>
        /// <returns></returns>
        public int MoveJogPitch(int iUnitIndex, int iUnitIndex2, int iKey, bool bDir)
        {
            int iResult = SUCCESS;
            //bool bDone;
            //int iState;
            //int iSource;
            //int iCoordID;
            //int iEvent;
            //double dDistance = 1.0;
            //double dVelocity;
            //int iAccelerate;
            //int iDecelerate;

            ////  Unit Index 범위 점검
            //if ((iUnitIndex < 0) || (iUnitIndex > m_JogTable.ListNo))
            //    return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_UNIT_INDEX);

            ////  눌려진 Jog Key에 따라 동작
            //switch (iKey)
            //{
            //    //  X-key (Left/Right)
            //    case DEF_OPPANEL_JOG_X_KEY:
            //        //  X-key에 설정이 되어 있으면
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            //  Motion 정지 확인
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            if (bDone == true)
            //            {
            //                //  Pitch 단위 이동
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                {
            //                    if (bDir != m_bJogXOld)     // 진행할 방향이 E-Stop 걸리게 된 방향과 반대이면
            //                    {
            //                        //  Motion 상태 확인
            //                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetAxisState(iCoordID, out iState)) != SUCCESS)
            //                        {
            //                            if (iState == DEF_E_STOP_EVENT)
            //                            {
            //                                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetAxisStatus(iCoordID, out iSource)) != SUCCESS)
            //                                {
            //                                    if (iSource & DEF_ST_HOME_SWITCH)
            //                                    {
            //                                        //  E-Stop Event 해제
            //                                        iEvent = DEF_NO_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                        //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                        dDistance = bDir ? dDistance : -dDistance;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                        iDecelerate = iAccelerate;
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                            return iResult;

            //                                        //  E-Stop Event 설정
            //                                        iEvent = DEF_E_STOP_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                        //  Pitch 단위 이동
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                            return iResult;
            //                                    }
            //                                    else if (iSource & DEF_ST_POS_LIMIT)
            //                                    {
            //                                        //  E-Stop Event 해제
            //                                        iEvent = DEF_NO_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                        //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                        dDistance = bDir ? dDistance : -dDistance;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                        iDecelerate = iAccelerate;
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                            return iResult;

            //                                        //  E-Stop Event 설정
            //                                        iEvent = DEF_E_STOP_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                        //  Pitch 단위 이동
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                            return iResult;
            //                                    }
            //                                    else if (iSource & DEF_ST_NEG_LIMIT)
            //                                    {
            //                                        //  E-Stop Event 해제
            //                                        iEvent = DEF_NO_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                        //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                        dDistance = bDir ? dDistance : -dDistance;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                        iDecelerate = iAccelerate;
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                            return iResult;

            //                                        //  E-Stop Event 설정
            //                                        iEvent = DEF_E_STOP_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                        //  Pitch 단위 이동
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                            return iResult;
            //                                    }
            //                                    else
            //                                        return iResult;
            //                                }
            //                                else
            //                                    return iResult;
            //                            }
            //                            else
            //                                return iResult;
            //                        }
            //                        else
            //                            return iResult;
            //                    }
            //                    else
            //                        return iResult;
            //                }
            //            }

            //            m_bJogXOld = bDir;

            //        }
            //        break;

            //    //  Y-key (For/Back)
            //    case DEF_OPPANEL_JOG_Y_KEY:
            //        //  Y-key에 설정이 되어 있으면
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            //  Motion 정지 확인
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            //  Pitch 단위 이동
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //            {
            //                if (bDir != m_bJogYOld)     // 진행할 방향이 E-Stop 걸리게 된 방향과 반대이면
            //                {
            //                    //  Motion 상태 확인
            //                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetAxisState(iCoordID, out iState)) != SUCCESS)
            //                    {
            //                        if (iState == DEF_E_STOP_EVENT)
            //                        {
            //                            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetAxisStatus(iCoordID, out iSource)) != SUCCESS)
            //                            {
            //                                if (iSource & DEF_ST_HOME_SWITCH)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_POS_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_NEG_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else
            //                                    return iResult;
            //                            }
            //                            else
            //                                return iResult;
            //                        }
            //                        else
            //                            return iResult;
            //                    }
            //                    else
            //                        return iResult;
            //                }
            //                else
            //                    return iResult;
            //            }

            //            m_bJogYOld = bDir;
            //        }
            //        break;

            //    //  T-key (CW/CCW)
            //    case DEF_OPPANEL_JOG_T_KEY:
            //        //  T-key에 설정이 되어 있으면
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            //  Motion 정지 확인
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            //  Pitch 단위 이동
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //            {
            //                if (bDir != m_bJogTOld)     // 진행할 방향이 E-Stop 걸리게 된 방향과 반대이면
            //                {
            //                    //  Motion 상태 확인
            //                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetAxisState(iCoordID, out iState)) != SUCCESS)
            //                    {
            //                        if (iState == DEF_E_STOP_EVENT)
            //                        {
            //                            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetAxisStatus(iCoordID, out iSource)) != SUCCESS)
            //                            {
            //                                if (iSource & DEF_ST_HOME_SWITCH)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_POS_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_NEG_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else
            //                                    return iResult;
            //                            }
            //                            else
            //                                return iResult;
            //                        }
            //                        else
            //                            return iResult;
            //                    }
            //                    else
            //                        return iResult;
            //                }
            //                else
            //                    return iResult;
            //            }

            //            m_bJogTOld = bDir;
            //        }
            //        break;

            //    //  Z-key (Up/Down)
            //    case DEF_OPPANEL_JOG_Z_KEY:
            //        //  Z-key에 설정이 되어 있으면
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            //  Motion 정지 확인
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            //  Pitch 단위 이동
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //            {
            //                if (bDir != m_bJogZOld)     // 진행할 방향이 E-Stop 걸리게 된 방향과 반대이면
            //                {
            //                    //  Motion 상태 확인
            //                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetAxisState(iCoordID, out iState)) != SUCCESS)
            //                    {
            //                        if (iState == DEF_E_STOP_EVENT)
            //                        {
            //                            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetAxisStatus(iCoordID, out iSource)) != SUCCESS)
            //                            {
            //                                if (iSource & DEF_ST_HOME_SWITCH)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_POS_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_NEG_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else
            //                                    return iResult;
            //                            }
            //                            else
            //                                return iResult;
            //                        }
            //                        else
            //                            return iResult;
            //                    }
            //                    else
            //                        return iResult;
            //                }
            //                else
            //                    return iResult;
            //            }

            //            m_bJogZOld = bDir;
            //        }
            //        break;

            //    default:
            //        return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_KEY_TYPE);
            //}

            return iResult;
        }

        /// <summary>
        /// Velocity 단위의 Jog 이동 동작을 수행한다.
        /// </summary>
        /// <param name="iUnitIndex">Jog로 움직일 Motion에 대한 정보 Table의 Index</param>
        /// <param name="iUnitIndex2"></param>
        /// <param name="iKey">이동할 Jog Key 종류 (0:X, 1:Y, 2:T, 3:Z)</param>
        /// <param name="bDir">이동할 방향 (true: +, false: -)</param>
        /// <returns></returns>
        public int MoveJogVelocity(int iUnitIndex, int iUnitIndex2, int iKey, bool bDir)
        {
            int iResult = SUCCESS;
            //bool bDone;
            //int iCoordID;

            //// Unit Index 범위 점검 
            //if ((iUnitIndex < 0) || (iUnitIndex > m_JogTable.ListNo))
            //    return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_UNIT_INDEX);

            //if (iUnitIndex2 == DEF_CtrlOpPanel.DEF_CTRLOPPANEL_JOG_NO_USE) // UnitIndex2가 안되어 있을때, 오류를 막기위해
            //    iUnitIndex2 = iUnitIndex;

            //// 눌려진 Jog Key에 따라 동작 
            //switch (iKey)
            //{
            //    // X-key (Left/Right) 
            //    case DEF_OPPANEL_JOG_X_KEY:
            //        // X-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 정지한 상태이면 
            //            //if (bDone == true)
            //            //{
            //            // Pitch 단위 이동 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMoveVelocity(iCoordID, bDir)) != SUCCESS)
            //                return iResult;
            //            //}
            //        }
            //        break;

            //    // Y-key (For/Back) 
            //    case DEF_OPPANEL_JOG_Y_KEY:
            //        // Y-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 정지한 상태이면 
            //            //if (bDone == true)
            //            //{
            //            // Pitch 단위 이동 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.JogMoveVelocity(iCoordID, bDir)) != SUCCESS)
            //                return iResult;
            //            //}
            //        }
            //        break;

            //    // T-key (CW/CCW) 
            //    case DEF_OPPANEL_JOG_T_KEY:
            //        // T-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 정지한 상태이면 
            //            //if (bDone == true)
            //            //{
            //            // Pitch 단위 이동 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMoveVelocity(iCoordID, bDir)) != SUCCESS)
            //                return iResult;
            //            //}
            //        }
            //        break;

            //    // Z-key (Up/Down) 
            //    case DEF_OPPANEL_JOG_Z_KEY:
            //        // Z-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 정지한 상태이면 
            //            //if (bDone == true)
            //            //{
            //            // Pitch 단위 이동 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMoveVelocity(iCoordID, bDir)) != SUCCESS)
            //                return iResult;
            //            //}
            //        }
            //        break;

            //    default:
            //        return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_KEY_TYPE);
            //}

            return iResult;
        }

        /// <summary>
        /// Jog로 이동한 것에 대한 정지 동작을 수행한다.
        /// </summary>
        /// <param name="iUnitIndex">Jog로 움직일 Motion에 대한 정보 Table의 Index</param>
        /// <param name="iUnitIndex2"></param>
        /// <param name="iKey">정지할 Jog Key 종류 (0:X, 1:Y, 2:T, 3:Z)</param>
        /// <returns></returns>
        public int StopJog(int iUnitIndex, int iUnitIndex2, int iKey)
        {
            int iResult = SUCCESS;
            //bool bDone;
            //int iCoordID;

            //// Unit Index 범위 점검 
            //if ((iUnitIndex < 0) || (iUnitIndex > m_JogTable.ListNo))
            //    return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_UNIT_INDEX);

            //if (iUnitIndex2 == DEF_CTRLOPPANEL_JOG_NO_USE) // UnitIndex2가 안되어 있을때, 오류를 막기위해
            //    iUnitIndex2 = iUnitIndex;

            //// 눌려진 Jog Key에 따라 동작 
            //switch (iKey)
            //{
            //    // X-key (Left/Right) 
            //    case DEF_OPPANEL_JOG_X_KEY:
            //        // X-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 이동 중 상태이면 
            //            if (bDone == false)
            //            {
            //                // 이동 정지 
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.VStop(iCoordID)) != SUCCESS)
            //                    return iResult;
            //            }
            //        }
            //        break;

            //    // Y-key (For/Back) 
            //    case DEF_OPPANEL_JOG_Y_KEY:
            //        // Y-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 이동 중 상태이면 
            //            if (bDone == false)
            //            {
            //                // 이동 정지 
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.VStop(iCoordID)) != SUCCESS)
            //                    return iResult;
            //            }
            //        }
            //        break;

            //    // T-key (CW/CCW) 
            //    case DEF_OPPANEL_JOG_T_KEY:
            //        // T-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 이동 중 상태이면 
            //            if (bDone == false)
            //            {
            //                // 이동 정지 
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.VStop(iCoordID)) != SUCCESS)
            //                    return iResult;
            //            }
            //        }
            //        break;

            //    // Z-key (Up/Down) 
            //    case DEF_OPPANEL_JOG_Z_KEY:
            //        // Z-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 이동 중 상태이면 
            //            if (bDone == false)
            //            {
            //                // 이동 정지 
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.VStop(iCoordID)) != SUCCESS)
            //                    return iResult;
            //            }
            //        }
            //        break;

            //    default:
            //        return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_KEY_TYPE);
            //}

            return iResult;
        }

        
        public int StopAllReturnOrigin()
        {
            int iResult = SUCCESS;

            for (int i = 0; i < m_JogTable.ListNo; i++)
            {
                if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
                    iResult = m_JogTable.MotionArray[i].m_XKey.m_plnkJog.StopReturnOrigin();
                if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
                    iResult = m_JogTable.MotionArray[i].m_YKey.m_plnkJog.StopReturnOrigin();
                if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
                    iResult = m_JogTable.MotionArray[i].m_TKey.m_plnkJog.StopReturnOrigin();
                if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
                    iResult = m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.StopReturnOrigin();
            }

            return iResult;
        }

        private int AdjustToACSIndex(int servoIndex)
        {
            return servoIndex - (int)EYMC_Axis.MAX;
        }

        public int GetAxisSensorStatus(int servoIndex, int sensorType, out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;

            if (servoIndex < (int)EYMC_Axis.MAX)
            {
                switch(sensorType)
                {
                    case (int)DEF_HOME_SENSOR:
                        //bStatus = m_RefComp.Yaskawa_Motion.ServoStatus[servoIndex].DetectHomeSensor;
                        break;
                    case (int)DEF_POSITIVE_SENSOR:
                        //bStatus = m_RefComp.Yaskawa_Motion.ServoStatus[servoIndex].DetectPlusSensor;
                        break;
                    case (int)DEF_NEGATIVE_SENSOR:
                        //bStatus = m_RefComp.Yaskawa_Motion.ServoStatus[servoIndex].DetectMinusSensor;
                        break;
                }
            }
            else
            {
                servoIndex = AdjustToACSIndex(servoIndex);
                switch (sensorType)
                {
                    case (int)DEF_HOME_SENSOR:
                        bStatus = m_RefComp.ACS_Motion.ServoStatus[servoIndex].DetectHomeSensor;
                        break;
                    case (int)DEF_POSITIVE_SENSOR:
                        bStatus = m_RefComp.ACS_Motion.ServoStatus[servoIndex].DetectPlusSensor;
                        break;
                    case (int)DEF_NEGATIVE_SENSOR:
                        bStatus = m_RefComp.ACS_Motion.ServoStatus[servoIndex].DetectMinusSensor;
                        break;
                }
            }

            return iResult;
        }

        public int ServoOn(int servoIndex)
        {
            int iResult = SUCCESS;

            servoIndex = AdjustToACSIndex(servoIndex);
            iResult = m_RefComp.ACS_Motion.ServoOn(servoIndex);
            
            return iResult;
        }

        public int ServoOff(int servoIndex)
        {
            int iResult = SUCCESS;

            servoIndex = AdjustToACSIndex(servoIndex);
            iResult = m_RefComp.ACS_Motion.ServoOff(servoIndex);            

            return iResult;
        }

        public int AllServoOn()
        {
            int iResult = SUCCESS;

            if (m_RefComp.ACS_Motion != null)
            {
                iResult = m_RefComp.ACS_Motion.AllServoOn();
                if (iResult != SUCCESS) return iResult;
            }

            return iResult;
        }

        public int AllServoOff()
        {
            int iResult = SUCCESS;

 
            if (m_RefComp.ACS_Motion != null)
            {
                iResult = m_RefComp.ACS_Motion.AllServoOff();
                if (iResult != SUCCESS) return iResult;
            }

            return iResult;
        }

        public int EStopAllAxis()
        {
            int iResult = SUCCESS;
            

            if (m_RefComp.ACS_Motion != null)
            {
                iResult = m_RefComp.ACS_Motion.StopAllServo();
                if (iResult != SUCCESS) return iResult;
            }

            //for (int i = 0; i < m_JogTable.ListNo; i++)
            //{
            //    if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
            //    {
            //        iResult = m_JogTable.MotionArray[i].m_XKey.m_plnkJog.EStop(m_JogTable.MotionArray[i].m_XKey.AxisIndex);
            //        m_JogTable.MotionArray[i].m_XKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_XKey.AxisIndex);
            //    }
            //    if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
            //    {
            //        iResult = m_JogTable.MotionArray[i].m_YKey.m_plnkJog.EStop(m_JogTable.MotionArray[i].m_YKey.AxisIndex);
            //        m_JogTable.MotionArray[i].m_YKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_YKey.AxisIndex);
            //    }
            //    if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
            //    {
            //        iResult = m_JogTable.MotionArray[i].m_TKey.m_plnkJog.EStop(m_JogTable.MotionArray[i].m_TKey.AxisIndex);
            //        m_JogTable.MotionArray[i].m_TKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_TKey.AxisIndex);
            //    }
            //    if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
            //    {
            //        iResult = m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.EStop(m_JogTable.MotionArray[i].m_ZKey.AxisIndex);
            //        m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_ZKey.AxisIndex);
            //    }
            //}

            //// Unit의 초기화 Flag를 Reset해야 한다.
            //for (int i = 0; i < (int)EThreadUnit.MAX; i++)
            //    m_bInitFlag[i] = false;

            return iResult;
        }

        public int StopAllAxis()
        {
            int iResult = SUCCESS;

            if (m_RefComp.ACS_Motion != null)
            {
                iResult = m_RefComp.ACS_Motion.StopAllServo();
                if (iResult != SUCCESS) return iResult;
            }

            //for (int i = 0; i < m_JogTable.ListNo; i++)
            //{
            //    if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_XKey.m_plnkJog.VStop(DEF_ALL_COORDINATE);
            //    if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_YKey.m_plnkJog.VStop(DEF_ALL_COORDINATE);
            //    if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_TKey.m_plnkJog.VStop(DEF_ALL_COORDINATE);
            //    if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.VStop(DEF_ALL_COORDINATE);
            //}

            return iResult;
        }

        public int OriginReturn(int servoIndex)
        {
            int iResult = SUCCESS;
            servoIndex = AdjustToACSIndex(servoIndex);
            if (servoIndex == 1) return GenerateErrorCode(ERR_OPPANEL_INVALID_INIT_UNIT_INDEX);
            iResult = m_RefComp.ACS_Motion.OriginReturn(servoIndex);            

            return iResult;
        }
        /// <summary>
        /// Unit의 초기화 Flag를 설정한다.
        /// </summary>
        /// <param name="iUnitIndex">초기화 Flag 설정할 Unit의 Index (DefSystem.h에 정의되어 있음)</param>
        /// <param name="bSts">설정할 Flag 값</param>
        /// <returns></returns>
        public int SetInitFlag(int iUnitIndex, bool bSts)
        {
            int iResult = SUCCESS;

            if (iUnitIndex < 0 || iUnitIndex > (int)EThreadUnit.MAX)
            {
                return GenerateErrorCode(ERR_OPPANEL_INVALID_INIT_UNIT_INDEX);
            }

            m_bInitFlag[iUnitIndex] = bSts;

            return iResult;
        }

        public int SetInitFlag(EThreadUnit index, bool bSts)
        {
            return SetInitFlag((int)index, bSts);
        }


        /// <summary>
        /// Unit의 초기화 Flag를 읽는다
        /// </summary>
        /// <param name="iUnitIndex"></param>
        /// <param name="bStatus"></param>
        /// <returns></returns>
        public int GetInitFlag(int iUnitIndex, out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;

            if (iUnitIndex < 0 || iUnitIndex > (int)EThreadUnit.MAX)
            {
                return GenerateErrorCode(ERR_OPPANEL_INVALID_INIT_UNIT_INDEX);
            }

            bStatus = m_bInitFlag[iUnitIndex];

            return iResult;
        }

        /// <summary>
        /// 시스템의 모든 Unit들이 초기화되어 있는지 확인한다.
        /// </summary>
        /// <param name="bInitSts">각 Unit별로 초기화 상태 (true:초기화되어 있음, false:아님)</param>
        /// <returns></returns>
        public bool CheckAllInit(out bool[] bInitSts)
        {
            bool bSts = false;
            bool bResult = true;

            bInitSts = new bool[(int)EThreadUnit.MAX];

            bool bEStopSts;
            IsPanelSWDetected(ESwitch.ESTOP, out bEStopSts);
            for (int i = 0; i < (int)EThreadUnit.MAX; i++)
            {
                if (bEStopSts == true)
                {
                    bResult = false;
                    SetInitFlag(i, false);
                }
                else
                {
                    GetInitFlag(i, out bSts);

                    bResult &= bSts;

                    if (bInitSts != null)
                        bInitSts[i] = bSts;
                }
            }

            return bResult;
        }

        public void ResetAllOriginFlag()
        {
            for (int i = 0; i < m_JogTable.ListNo; i++)
            {
                if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
                    m_JogTable.MotionArray[i].m_XKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_XKey.AxisIndex);
                if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
                    m_JogTable.MotionArray[i].m_YKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_YKey.AxisIndex);
                if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
                    m_JogTable.MotionArray[i].m_TKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_TKey.AxisIndex);
                if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
                    m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_ZKey.AxisIndex);
            }
        }

        public int GetOriginFlag(int iUnitIndex, out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;

            if (iUnitIndex < 0 || iUnitIndex > (int)EAxis.MAX)
            {
                return GenerateErrorCode(ERR_OPPANEL_INVALID_SERVO_UNIT_INDEX);
            }

            int servoNum=0;

            int acsStartIndex = (int)EAxis.STAGE1_X;

            servoNum = iUnitIndex - acsStartIndex;
            bStatus = m_RefComp.ACS_Motion.IsOriginReturned(servoNum);           

            return iResult;
        }

        /// <summary>
        /// for test
        /// simulation 상황에서 원점 복귀 flag를 셋팅하기 위해서
        /// </summary>
        /// <param name="iUnitIndex"></param>
        /// <param name="bStatus"></param>
        /// <returns></returns>
        public int SetOriginFlag(int iUnitIndex, bool bStatus = true)
        {
            int iResult = SUCCESS;

            if (iUnitIndex < 0 || iUnitIndex > (int)EAxis.MAX)
            {
                return GenerateErrorCode(ERR_OPPANEL_INVALID_SERVO_UNIT_INDEX);
            }

            int servoNum = 0;

            int acsStartIndex = (int)EAxis.STAGE1_X;

            servoNum = iUnitIndex - acsStartIndex;
            bStatus = m_RefComp.ACS_Motion.ServoStatus[servoNum].IsOriginReturned = bStatus;
            

            return iResult;
        }

        public int GetServoOnStatus(int iUnitIndex, out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;

            if (iUnitIndex < 0 || iUnitIndex > (int)EAxis.MAX)
            {
                return GenerateErrorCode(ERR_OPPANEL_INVALID_SERVO_UNIT_INDEX);
            }

            int servoNum = 0;
            int acsStartIndex = (int)EAxis.STAGE1_X;

            servoNum = iUnitIndex - acsStartIndex;
            bStatus = m_RefComp.ACS_Motion.ServoStatus[servoNum].IsServoOn;
           

            return iResult;
        }

        public bool CheckAllOrigin(out bool[/*DEF_MAX_MOTION_AXIS*/] bOriginSts)
        {
            bOriginSts = new bool[(int)EAxis.MAX];
            //m_JogTable.MotionArray[]
            return true;
            //int i = 0;
            //bool rgbResult[4] = { false, false, false, false };
            //bool bSts = true;
            //bool rgbUse[4] = { true, true, true, true };

            //// Motion MultiAxes Component - STAGE1
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_STAGE1].m_XKey.m_plnkJog.IsOriginReturned(-1, rgbUse, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE1_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE1_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE1_T] = rgbResult[DEF_T];
            //bSts &= rgbResult[DEF_T];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE1_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - STAGE2
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_STAGE2].m_XKey.m_plnkJog.IsOriginReturned(-1, rgbUse, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE2_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE2_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - STAGE3
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_STAGE3].m_YKey.m_plnkJog.IsOriginReturned(-1, rgbUse, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE3_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE3_T] = rgbResult[DEF_T];
            //bSts &= rgbResult[DEF_T];

            //// Motion MultiAxes Component - WORKBENCH1 
            //// 	for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //// 	m_JogTable.MotionArray[DEF_JOG_WORKBENCH].m_YKey.m_plnkJog.IsOriginReturned(0, null, rgbResult);
            //// 	if (bOriginSts != null)
            //// 		bOriginSts[DEF_AXIS_MMC_WORKBENCH_Y] = rgbResult[DEF_X];
            //// 	bSts &= rgbResult[DEF_X];

            //// Motion MultiAxes Component - SHEAD1 
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_SHEAD1].m_YKey.m_plnkJog.IsOriginReturned(-1, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_SHEAD1_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_SHEAD1_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - SHEAD2 
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_SHEAD2].m_YKey.m_plnkJog.IsOriginReturned(-1, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_SHEAD2_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_SHEAD2_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - GHEAD1 
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_GHEAD1].m_XKey.m_plnkJog.IsOriginReturned(-1, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD1_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD1_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD1_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - GHEAD2 
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_GHEAD2].m_XKey.m_plnkJog.IsOriginReturned(-1, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD2_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD2_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD2_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - CAMERA1 
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_CAMERA1].m_XKey.m_plnkJog.IsOriginReturned(0, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_CAMERA1_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];

            //// Motion MultiAxes Component - CAMERA2 
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_CAMERA2].m_XKey.m_plnkJog.IsOriginReturned(0, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_CAMERA2_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];

            //// Motion MultiAxes Component - UPPER_HANDLER 
            //for (int i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_UHANDLER].m_XKey.m_plnkJog.IsOriginReturned(0, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_UPPER_HANDLER_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];

            //return bSts;
        }

        public int Initialize()
        {
            int iResult = SUCCESS;

            if ((iResult = m_RefComp.IO.Initialize()) != SUCCESS)
            {
                string strLog = "IO Component Object의 초기화에 실패하였습니다.";
                WriteLog(strLog, ELogType.Debug, ELogWType.D_Error);

                return iResult;
            }

            return SUCCESS;
        }

        /// <summary>
        /// 동일한 종류의 스위치들을 조사해서 감지되었는지 확인.
        /// </summary>
        /// <param name="bDetected"></param>
        /// <param name="addresses"></param>
        /// <returns></returns>
        private int IsPanelSWDetected(out bool bDetected, int[] address)
        {
            int iResult = SUCCESS;
            bDetected = false;
             
            return SUCCESS;
        }

        /// <summary>
        /// Panel Switch 종류별로 확인해서 해당 스위치가 눌렸는지를 리턴
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bDetected"></param>
        /// <returns></returns>
        public int IsPanelSWDetected(ESwitch type, out bool bDetected)
        {
            int iResult = SUCCESS;
            bDetected = false;

            switch (type)
            {
                ////////////////////////////////////////////////////////////////////////
                // Switch
                case ESwitch.START:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.RunInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.STOP:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.StopInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.RESET:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.ResetInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.ESTOP:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.EStopInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                ////////////////////////////////////////////////////////////////////////
                // Jog
                case ESwitch.JOG_XP:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.XpInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.JOG_XN:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.XnInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.JOG_YP:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.YpInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.JOG_YN:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.YnInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.JOG_TP:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.TpInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.JOG_TN:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.TnInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.JOG_ZP:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.ZpInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

                case ESwitch.JOG_ZN:
                    iResult = IsPanelSWDetected(out bDetected, m_IOAddrTable.OpPanel.ZnInputAddr);
                    if (iResult != SUCCESS) return iResult;
                    break;

            }

            return iResult;
        }

        /// <summary>
        /// Door 상태를 체크하는 함수, 문이 열린 도어가 있으면 확인 중단후에 바로 return
        /// b접점 이용. normal on. off when door opened.
        /// </summary>
        /// <param name="bDoorOpened"></param>
        /// <param name="iGroup">door group</param>
        /// <param name="iIndex">index of door in door group</param>
        /// <returns></returns>
        public int CheckDoorStatus(out bool bDoorOpened, int iGroup = -1, int iIndex = -1)
        {
            int iResult = SUCCESS;
            bDoorOpened = true;

            bool bTemp;
            // 도어 센서 그룹 전체 확인 
            if (iGroup == -1)
            {
                for (int i = 0; i < (int)EDoorGroup.MAX; i++)
                {
                    iResult = CheckDoorGroup(out bTemp, i, iIndex);
                    if (iResult != SUCCESS) return iResult;

                    if(bTemp == false)
                    {
                        return SUCCESS;
                    }
                }
            }
            // 센서 그룹 하나만 확인 
            else
            {
                iResult = CheckDoorGroup(out bTemp, iGroup, iIndex);
                if (iResult != SUCCESS) return iResult;

                if (bTemp == false)
                {
                    return SUCCESS;
                }
            }

            // 모든 도어 상태를 확인 완료 후에는
            bDoorOpened = false;
            return iResult;
        }

        /// <summary>
        /// Door 상태를 체크하는 함수, 문이 열린 도어가 있으면 확인 중단후에 바로 bDoorClosed = false & return
        /// b접점 이용. normal on. off when door opened.
        /// </summary>
        /// <param name="bDoorClosed"></param>
        /// <param name="iGroup">door group</param>
        /// <param name="iIndex">index of door in door group</param>
        /// <returns></returns>
        private int CheckDoorGroup(out bool bDoorClosed, int iGroup, int iIndex)
        {
            int iResult = SUCCESS;
            bDoorClosed = false;
            if (iGroup == -1) return GenerateErrorCode(ERR_OPPANEL_INVALID_DOOR_GROUP);

            bool bTemp;
            // 도어 센서 그룹 내부 전체 확인
            if (iIndex == -1)
            {
                for (int j = 0; j < (int)EDoorIndex.MAX; j++)
                {
                    if (m_Data.UseDoorStatus[iGroup, j] == false) continue;

                    int addr = m_IOAddrTable.SafeDoorAddr[iGroup, j];
                    if (addr == -1) return GenerateErrorCode(ERR_OPPANEL_DOOR_ADDRESS_NOT_DEFINED);

                    if ((iResult = m_RefComp.IO.IsOn(addr, out bTemp)) != SUCCESS) return iResult;

                    if (bTemp == false)
                    {
                        bDoorClosed = false;
                        return SUCCESS;
                    }
                }
            }
            else // 특정 도어만 확인
            {
                if (m_Data.UseDoorStatus[iGroup, iIndex] == false) return SUCCESS;

                int addr = m_IOAddrTable.SafeDoorAddr[iGroup, iIndex];
                if (addr == -1) return GenerateErrorCode(ERR_OPPANEL_DOOR_ADDRESS_NOT_DEFINED);

                if ((iResult = m_RefComp.IO.IsOn(addr, out bTemp)) != SUCCESS)
                    return iResult;

                if (bTemp == false)
                {
                    bDoorClosed = false;
                    return SUCCESS;
                }
            }

            // 모든 도어 상태를 확인 완료 후에는
            bDoorClosed = true;
            return iResult;
        }

        /// <summary>
        /// CP Trip의 상태를 읽는다.
        /// </summary>
        /// <param name="bStatus">CP Trip의 상태 (true : ON, false : OFF)</param>
        /// <param name="iIndex">몇번째 센서인지 번호 (-1이면 전체 센서를 확인하여 하나라도 ON이면 결과를 ON으로 알린다.)</param>
        /// <returns></returns>
        public int GetCPTripStatus(int iIndex, out bool bStatus)
        {
            int i = 0;
            bool bSts = false;
            bool bTemp;
            int iResult = SUCCESS;

            bStatus = false;

            //if (iIndex == -1)
            //{
            //    for (int i = 0; i < DEF_OPPANEL_MAX_CP_TRIP_NO; i++)
            //    {
                    
            //        if (m_IOAddrTable.iCPTripAddr[i] != 0)
            //        {
            //            if ((iResult = m_RefComp.IO.IsOn(m_IOAddrTable.iCPTripAddr[i], out bTemp)) != SUCCESS)
            //                return iResult;

            //            bSts = bSts || bTemp;
            //        }
            //        // Sensor Address가 할당되어 있지 않으면 확인 중단
            //        else
            //            break;
            //    }

            //    *bStatus = bSts;
            //}
            //else
            //{
                
            //    if (m_IOAddrTable.iCPTripAddr[iIndex] != 0)
            //    {
            //        if ((iResult = m_RefComp.IO.IsOn(m_IOAddrTable.iCPTripAddr[iIndex], out bTemp)) != SUCCESS)
            //            return iResult;

            //        *bStatus = bTemp;
            //    }
            //    else
            //        *bStatus = false;
            //}

            return iResult;
        }

        public int GetAirErrorStatus(out bool bStatus)
        {
            int iResult = SUCCESS;

            bStatus = false;
            if (m_IOAddrTable.MainAirAddr != 0)
            {
                if ((iResult = m_RefComp.IO.IsOff(m_IOAddrTable.MainAirAddr, out bStatus)) != SUCCESS)
                    return iResult;
            }

            return SUCCESS;
        }

        public int GetDcPWErrorStatus(out bool bStatus)
        {
            int iResult = SUCCESS;

            bStatus = false;
            if (m_IOAddrTable.DcPowerAddr != 0)
            {
                if ((iResult = m_RefComp.IO.IsOff(m_IOAddrTable.DcPowerAddr, out bStatus)) != SUCCESS)
                    return iResult;
            }

            return SUCCESS;
        }

        public int GetVacuumErrorStatus(out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;
            if (m_IOAddrTable.MainVacuumAddr != 0)
            {
                if ((iResult = m_RefComp.IO.IsOff(m_IOAddrTable.MainVacuumAddr, out bStatus)) != SUCCESS)
                    return iResult;
            }

            if (bStatus == false)  // false 이면 비정상이므로 일단 하나만 안되도 안되므로
                return iResult;

            if (m_IOAddrTable.SubVacuumAddr != 0)
            {
                if ((iResult = m_RefComp.IO.IsOff(m_IOAddrTable.SubVacuumAddr, out bStatus)) != SUCCESS)
                    return iResult;
            }

            return SUCCESS;
        }

        public int GetCleanerDetect1ErrorStatus(out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;
            if (m_IOAddrTable.CleanerDetect1Addr != 0)
            {
                if ((iResult = m_RefComp.IO.IsOff(m_IOAddrTable.CleanerDetect1Addr, out bStatus)) != SUCCESS)
                    return iResult;
            }

            return SUCCESS;
        }

        public int GetMotorAmpFaultStatus(int iUnitIndex, out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;

            if (iUnitIndex < 0 || iUnitIndex > (int)EAxis.MAX)
            {
                return GenerateErrorCode(ERR_OPPANEL_INVALID_SERVO_UNIT_INDEX);
            }

            int servoNum = 0;
            int acsStartIndex = (int)EAxis.STAGE1_X;

            servoNum = iUnitIndex - acsStartIndex;
            bStatus = m_RefComp.ACS_Motion.ServoStatus[servoNum].IsDriverFault;            

            return iResult;
        }

        public int GetMotionPowerRelayStatus(out bool bStatus)
        {
            bool bSts1 = true, bSts2 = true;
            int iResult = SUCCESS;
            int iCount = 0;


            //for (int i = 0; i < DEF_OPPANEL_MAX_MOTION_RELAY_NO; i++)
            //{
            //    if (m_IOAddrTable.iMotionRelayAddr[i] != 0)
            //    {
            //        if ((iResult = m_RefComp.IO.IsOn(m_IOAddrTable.iMotionRelayAddr[i], out bSts1)) != SUCCESS)
            //            return iResult;

            //        bSts2 = bSts2 && bSts1;
            //        iCount++;
            //    }
            //}

            //if (iCount != 0)
            //    *bStatus = bSts2;
            //else
            //    *bStatus = false;

            bStatus = true;
            return iResult;
        }

        public int SetMotionPowerRelayStatus(bool bStatus)
        {
            bool bSts1 = true, bSts2 = true;
            int iResult = SUCCESS;
            int iCount = 0;


            //for (int i = 0; i < DEF_OPPANEL_MAX_MOTION_RELAY_NO; i++)
            //{
            //    if (m_IOAddrTable.iMotionRelayAddr[i] != 0)
            //    {
            //        if (bStatus == true)
            //        {
            //            if ((iResult = m_RefComp.IO.OutputOn(m_IOAddrTable.iMotionRelayAddr[i])) != SUCCESS)
            //                return iResult;
            //        }
            //        else
            //        {
            //            if ((iResult = m_RefComp.IO.OutputOff(m_IOAddrTable.iMotionRelayAddr[i])) != SUCCESS)
            //                return iResult;
            //        }
            //    }
            //}

            return iResult;
        }

        public void SetVelocityMode(double[/*DEF_MAX_MOTION_AXIS*/] rgdVelocity)
        {
            //. Motion 속도 수정...
            int i = 0;
            int iResult = SUCCESS;
            bool bFault = false;
            int iAxisID;
            double dVel;
            int iAcc;

            //for (int i = 0; i < m_JogTable.ListNo; i++)
            //{
            //    if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
            //    {
            //        m_JogTable.MotionArray[i].m_XKey.m_plnkJog.GetAxisID(m_JogTable.MotionArray[i].m_XKey.AxisIndex, out iAxisID);
            //        m_JogTable.MotionArray[i].m_XKey.m_plnkJog.GetMovingVelocity(m_JogTable.MotionArray[i].m_XKey.AxisIndex, out dVel, out iAcc);
            //        dVel = rgdVelocity[iAxisID];
            //        iResult = m_JogTable.MotionArray[i].m_XKey.m_plnkJog.SetMovingVelocity(m_JogTable.MotionArray[i].m_XKey.AxisIndex, out dVel, out iAcc);
            //    }
            //    if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
            //    {
            //        m_JogTable.MotionArray[i].m_YKey.m_plnkJog.GetAxisID(m_JogTable.MotionArray[i].m_YKey.AxisIndex, out iAxisID);
            //        m_JogTable.MotionArray[i].m_YKey.m_plnkJog.GetMovingVelocity(m_JogTable.MotionArray[i].m_YKey.AxisIndex, out dVel, out iAcc);
            //        dVel = rgdVelocity[iAxisID];
            //        iResult = m_JogTable.MotionArray[i].m_YKey.m_plnkJog.SetMovingVelocity(m_JogTable.MotionArray[i].m_YKey.AxisIndex, out dVel, out iAcc);
            //    }
            //    if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
            //    {
            //        m_JogTable.MotionArray[i].m_TKey.m_plnkJog.GetAxisID(m_JogTable.MotionArray[i].m_TKey.AxisIndex, out iAxisID);
            //        m_JogTable.MotionArray[i].m_TKey.m_plnkJog.GetMovingVelocity(m_JogTable.MotionArray[i].m_TKey.AxisIndex, out dVel, out iAcc);
            //        dVel = rgdVelocity[iAxisID];
            //        iResult = m_JogTable.MotionArray[i].m_TKey.m_plnkJog.SetMovingVelocity(m_JogTable.MotionArray[i].m_TKey.AxisIndex, out dVel, out iAcc);
            //    }
            //    if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
            //    {
            //        m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.GetAxisID(m_JogTable.MotionArray[i].m_ZKey.AxisIndex, out iAxisID);
            //        m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.GetMovingVelocity(m_JogTable.MotionArray[i].m_ZKey.AxisIndex, out dVel, out iAcc);
            //        dVel = rgdVelocity[iAxisID];
            //        iResult = m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.SetMovingVelocity(m_JogTable.MotionArray[i].m_ZKey.AxisIndex, out dVel, out iAcc);
            //    }
            //}
        }

        /// <summary>
        /// Door Sensor 점검여부를 설정한다.
        /// </summary>
        /// <param name="bFlag">점검 여부 (true:점검, false:무시)</param>
        /// <param name="iGroup">Door Sensor Group 번호 (-1이면 모든 Group내 설정)</param>
        /// <param name="iIndex">Door Snesor Group 내 Index 번호 (-1이면 Group내 모든 Index 설정)</param>
        public void SetDoorCheckFlag(bool bFlag, int iGroup, int iIndex)
        {
            int iResult = SUCCESS;

            if (iGroup == -1)
            {
                for (int i = 0; i < (int)EDoorGroup.MAX; i++)
                {
                    if (iIndex == -1)
                    {
                        for (int j = 0; j < (int)EDoorIndex.MAX; j++)
                        {

                            if (m_IOAddrTable.SafeDoorAddr[i, j] != 0)
                                m_Data.UseDoorStatus[i, j] = bFlag;
                            // Sensor Address가 할당되어 있지 않으면 확인 중단
                            else
                                j = (int)EDoorIndex.MAX;
                        }
                    }
                    else
                    {

                        if (m_IOAddrTable.SafeDoorAddr[i, iIndex] != 0)
                            m_Data.UseDoorStatus[i, iIndex] = bFlag;
                    }
                }
            }
            else
            {
                if (iIndex == -1)
                {
                    for (int j = 0; j < (int)EDoorIndex.MAX; j++)
                    {

                        if (m_IOAddrTable.SafeDoorAddr[iGroup, j] != 0)
                            m_Data.UseDoorStatus[iGroup, j] = bFlag;
                        // Sensor Address가 할당되어 있지 않으면 확인 중단
                        else
                            j = (int)EDoorIndex.MAX;
                    }
                }
                else
                {

                    if (m_IOAddrTable.SafeDoorAddr[iGroup, iIndex] != 0)
                        m_Data.UseDoorStatus[iGroup, iIndex] = bFlag;
                }
            }
        }

        public int SetStartLamp(bool bStatus)
        {
            return setPanelLedStatus(bStatus, m_IOAddrTable.OpPanel.RunOutputAddr);
        }

        public int SetStopLamp(bool bStatus)
        {
            return setPanelLedStatus(bStatus, m_IOAddrTable.OpPanel.StopOutputAddr);
        }

        public int SetResetLamp(bool bStatus)
        {
            return setPanelLedStatus(bStatus, m_IOAddrTable.OpPanel.ResetOutputAddr);
        }

        public int SetTowerRedLamp(bool bStatus)
        {
            return setTowerLampStatus(bStatus, m_IOAddrTable.TowerLamp.RedLampAddr);
        }

        public int SetTowerYellowLamp(bool bStatus)
        {
            return setTowerLampStatus(bStatus, m_IOAddrTable.TowerLamp.YellowLampAddr);
        }

        public int SetTowerGreenLamp(bool bStatus)
        {
            return setTowerLampStatus(bStatus, m_IOAddrTable.TowerLamp.GreenLampAddr);
        }

        public int SetBuzzerStatus(bool bStatus, EBuzzer index = EBuzzer.ALL)
        {
            int iResult = SUCCESS;

            // Tower Lamp의 Buzzer에 대한 전체 출력 모드가 선택된 경우
            if (index == EBuzzer.ALL)
            {
                foreach (int addr in m_IOAddrTable.TowerLamp.BuzzerArray)
                {
                    if ((iResult = setTowerLampStatus(bStatus, addr)) != SUCCESS)
                        return iResult;
                }
            }
            // Tower Lamp의 Buzzer에 대한 하나의 출력 모드가 선택된 경우 
            else
            {
                if ((iResult = setTowerLampStatus(bStatus, m_IOAddrTable.TowerLamp.BuzzerArray[(int)index])) != SUCCESS)
                    return iResult;
            }

            return iResult;
        }

        public int GetEnabledOpPanelID(out int piOpPanelID)
        {
            int iResult = SUCCESS;
            bool bStatus = false;
            string strLog;

            piOpPanelID = DEF_OPPANEL_NONE_PANEL_ID;
            //if ((iResult = m_RefComp.IO.IsOn(m_IOAddrTable.iTouchSelectAddr, out bStatus)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLog = String.Format("Op Panel ID의 상태를 읽는데 실패했습니다.");
            //    WriteLog(strLog, ELogType.Debug, ELogWType.D_Error);

            //    piOpPanelID = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}
            //else
            //{
            //    // IO가 true이면 앞 Touch Panel
            //    if (bStatus == true)
            //        *piOpPanelID = DEF_OPPANEL_FRONT_PANEL_ID;
            //    // IO가 false이면 뒷 Touch Panel
            //    else
            //        *piOpPanelID = DEF_OPPANEL_BACK_PANEL_ID;
            //}

            return SUCCESS;
        }

        public int ChangeOpPanel(int iOpPanelID)
        {
            int iResult = SUCCESS;
            string strLog;
            int iCurOpPanelID;

            // 현재 활성화되어 있는 Touch Panel을 알아온다.
            if ((iResult = GetEnabledOpPanelID(out iCurOpPanelID)) != SUCCESS)
                return iResult;

            // 전환할 Touch Panel이 현재 활성화된 Touch Panel과 일치하면 Pass
            if (iOpPanelID == iCurOpPanelID)
                return SUCCESS;

            //if (iOpPanelID == DEF_OPPANEL_FRONT_PANEL_ID)
            //{
            //    if ((iResult = m_RefComp.IO.OutputOn(m_IOAddrTable.TouchSelectAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLog = String.Format("앞면으로 Touch Panel 사용권 전환에 실패했습니다.");
            //        WriteLog(strLog, ELogType.Debug, ELogWType.D_Error);

            //        return iResult;
            //    }

            //    // 정상 동작 Log
            //    strLog = String.Format("앞면으로 Touch Panel 사용권 전환하였습니다.");
            //    //		m_plogMng.WriteLog(strLog, __FILE__, __LINE__);
            //}
            //else if (iOpPanelID == DEF_OPPANEL_BACK_PANEL_ID)
            //{
            //    if ((iResult = m_RefComp.IO.OutputOff(m_IOAddrTable.iTouchSelectAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLog = String.Format("뒷면으로 Touch Panel 사용권 전환에 실패했습니다.");
            //        WriteLog(strLog, ELogType.Debug, ELogWType.D_Error);

            //        return iResult;
            //    }

            //    // 정상 동작 Log
            //    strLog = String.Format("뒷면으로 Touch Panel 사용권 전환하였습니다.");
            //    //		m_plogMng.WriteLog(strLog, __FILE__, __LINE__);
            //}

            return SUCCESS;
        }

        public int GetOpPanelSelectSW(out int iStatus)
        {
            int iResult = SUCCESS;
            string strLog;
            bool bStatus1, bStatus2, bStatus3, bStatus4;

            iStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //if ((iResult = GetJogXMinusButtonStatus(out bStatus1)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    //strLog = String.Format("Touch Panel Select Switch(X-) 상태 읽기에 실패했습니다.");
            //    //WriteLog(strLog, ELogType.Debug, ELogWType.D_Error);

            //    iStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}

            //if ((iResult = GetJogYPlusButtonStatus(out bStatus2)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLog = String.Format("Touch Panel Select Switch(Y+) 상태 읽기에 실패했습니다.");
            //    WriteLog(strLog, ELogType.Debug, ELogWType.D_Error);

            //    iStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}

            //if ((iResult = GetJogXPlusButtonStatus(out bStatus3)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLog = String.Format("Touch Panel Select Switch(X+) 상태 읽기에 실패했습니다.");
            //    WriteLog(strLog, ELogType.Debug, ELogWType.D_Error);

            //    iStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}

            //if ((iResult = GetJogYMinusButtonStatus(out bStatus4)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLog = String.Format("Touch Panel Select Switch(Y-) 상태 읽기에 실패했습니다.");
            //    WriteLog(strLog, ELogType.Debug, ELogWType.D_Error);

            //    iStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}

            //// X(-)와 Y(+)가 동시에 눌려졌으면 - 앞면으로 전환 
            //if ((bStatus1 && bStatus2 && bStatus3 && bStatus4) == true)
            //{
            //    *iStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    // 정상 동작 Log
            //    strLog = String.Format("Touch Panel 전환 Switch가 전부 눌렸습니다.");
            //    //		m_plogMng.WriteLog(strLog, __FILE__, __LINE__);

            //    return SUCCESS;
            //}
            //// X(-)와 Y(+)가 동시에 눌려졌으면 - 앞면으로 전환 
            //else if ((bStatus1 && bStatus2) == true)
            //{
            //    *iStatus = DEF_OPPANEL_FRONT_PANEL_ID;

            //    // 정상 동작 Log
            //    strLog = String.Format("앞면 Touch Panel 전환 Switch가 눌렸습니다.");
            //    //		m_plogMng.WriteLog(strLog, __FILE__, __LINE__);

            //    return SUCCESS;
            //}
            //// X(+)와 Y(-)가 동시에 눌려졌으면 - 뒷면으로 전환 
            //else if ((bStatus3 && bStatus4) == true)
            //{
            //    *iStatus = DEF_OPPANEL_BACK_PANEL_ID;

            //    // 정상 동작 Log
            //    strLog = String.Format("뒷면 Touch Panel 전환 Switch가 눌렸습니다.");
            //    //		m_plogMng.WriteLog(strLog, __FILE__, __LINE__);

            //    return SUCCESS;
            //}
            //// 해당 사항 없을 때 
            //else
            //{
            //    *iStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return SUCCESS;
            //}

            return SUCCESS;
        }

        /// <summary>
        /// 앞, 뒷 Panel의 특정 LED의 동작을 설정한다.
        /// </summary>
        /// <param name="bStatus"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public int setPanelLedStatus(bool bStatus, int[] address)
        {
            int iResult = SUCCESS;
            foreach (int addr in address)
            {
                if (addr == -1) continue;
                iResult = m_RefComp.IO.SetBit(addr, bStatus);
                if (iResult != SUCCESS) return iResult;
            }
            return SUCCESS;
        }

        /// <summary>
        /// Tower Lamp의 Lamp, Buzzer의 동작을 설정한다.
        /// </summary>
        /// <param name="bStatus"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        public int setTowerLampStatus(bool bStatus, int addr)
        {
            int iResult = SUCCESS;
            if (addr == -1) return SUCCESS;

            iResult = m_RefComp.IO.SetBit(addr, bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int CheckAllTank_Empty(out bool bEmptyAll, out bool bEmptyPart)
        {
            int iResult = SUCCESS;
            bool bStatus1 = false;
            bool bStatus2 = false;

            bEmptyAll = false;
            bEmptyPart = false;

            for (int i = 0; i < DEF_MAX_HEAD_NO; i++)
            {
                iResult = CheckTank_Empty(i, out bStatus1, out bStatus2);
                if (iResult != SUCCESS) return iResult;

                if (bStatus1 == true) bEmptyAll = true;
                if (bStatus2 == true) bEmptyPart = true;

            }

            return SUCCESS;
        }

        public int CheckTank_Empty(int nHeadNo, out bool bEmptyAll, out bool bEmptyPart)
        {
            int iResult = SUCCESS;

            bool[] bStatus = new bool[(int)ECoatTank.MAX];

            bEmptyAll = false;
            bEmptyPart = false;
            bool bInit = false;

            for (int i = 0; i < (int)ECoatTank.MAX; i++)
            {
                if (m_Data.UseTankAlarm[i] == false) continue;

                iResult = m_RefComp.IO.IsOn(m_IOAddrTable.TankEmptyAddr[i], out bStatus[i]);
                if (iResult != SUCCESS) return iResult;

                if (bInit == false)
                {
                    bInit = true;
                    bEmptyAll = true;
                }

                if (bStatus[i] == true) bEmptyPart = true;

                if (bStatus[i] == false) bEmptyAll = false;
            }

            return SUCCESS;
        }

        /// <summary>
        /// check area sensor. 
        /// b접점 이용. normal on. off when area sensor detected.
        /// </summary>
        /// <param name="bDetected"></param>
        /// <returns></returns>
        public int CheckAreaSensorStatus(out bool bDetected)
        {
            int iResult = SUCCESS;
            bDetected = true;
            bool bTemp = true;
            foreach(int addr in m_IOAddrTable.AreaSensorAddr)
            {
                if (addr == IO_ADDR_NOT_DEFINED) continue;
                iResult = m_RefComp.IO.IsOn(addr, out bTemp);
                if (bTemp == false) return SUCCESS;
            }
            bDetected = false;
            return SUCCESS;
        }
    }
}
