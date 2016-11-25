using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO.Ports;

using System.Windows.Forms;

using LWDicer.UI;
using MotionYMC;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_IO;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_LCNet;
using static LWDicer.Layers.DEF_SocketClient;

using static LWDicer.Layers.DEF_Motion;
using static LWDicer.Layers.DEF_Yaskawa;
using static LWDicer.Layers.DEF_ACS;
using static LWDicer.Layers.DEF_MultiAxesYMC;
using static LWDicer.Layers.DEF_MultiAxesACS;
using static LWDicer.Layers.DEF_Cylinder;
using static LWDicer.Layers.DEF_Vacuum;
using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_Polygon;

using static LWDicer.Layers.DEF_OpPanel;
using static LWDicer.Layers.DEF_MeElevator;
using static LWDicer.Layers.DEF_MeHandler;
using static LWDicer.Layers.DEF_MeStage;
using static LWDicer.Layers.DEF_MePushPull;
using static LWDicer.Layers.DEF_SerialPort;
using static LWDicer.Layers.DEF_MeSpinner;

using static LWDicer.Layers.DEF_CtrlOpPanel;
using static LWDicer.Layers.DEF_CtrlHandler;
using static LWDicer.Layers.DEF_CtrlSpinner;
using static LWDicer.Layers.DEF_CtrlPushPull;
using static LWDicer.Layers.DEF_CtrlLoader;
using static LWDicer.Layers.DEF_CtrlStage;

using static LWDicer.Layers.DEF_TrsAutoManager;


namespace LWDicer.Layers
{
    public class MLWDicer : MObject, IDisposable
    {
        // static common data
        public static bool bUseOnline { get; private set; }
        public static bool bInSfaTest { get; private set; }
        public static ELanguage Language { get; private set; } = ELanguage.KOREAN;

        ///////////////////////////////////////////////////////////////////////
        // Common Class
        public MSystemInfo m_SystemInfo { get; private set; }
        public MDataManager m_DataManager { get; private set; }

        ///////////////////////////////////////////////////////////////////////
        // Hardware Layer

        // Motion
        public MYaskawa m_YMC;
        public MACS m_ACS;

        // MultiAxes
        public MMultiAxes_YMC m_AxLoader      ;
        public MMultiAxes_YMC m_AxPushPull    ;
        public MMultiAxes_YMC m_AxCentering1  ;
        public MMultiAxes_YMC m_AxRotate1     ;
        public MMultiAxes_YMC m_AxCleanNozzle1;
        public MMultiAxes_YMC m_AxCoatNozzle1 ;
        public MMultiAxes_YMC m_AxCentering2  ;
        public MMultiAxes_YMC m_AxRotate2     ;
        public MMultiAxes_YMC m_AxCleanNozzle2;
        public MMultiAxes_YMC m_AxCoatNozzle2 ;
        public MMultiAxes_YMC m_AxUpperHandler;
        public MMultiAxes_YMC m_AxLowerHandler;
        public MMultiAxes_ACS m_AxStage1      ;

        // Vision
        public MVisionSystem m_VisionSystem;
        public MVisionCamera[] m_VisionCamera = new MVisionCamera[DEF_MAX_CAMERA_NO];
        public MVisionView[] m_VisionView = new MVisionView[DEF_MAX_CAMERA_NO];

        // Laser Scanner
        //public CMarkingManager m_ScanManager;
        //public CMarkingWindow m_ScanWindow;
        public MSocketClient m_ControlComm;
        public MSocketClient m_ScanHeadComm;
        //public FormScanWindow m_FormScanner;


#if EQUIP_DICING_DEV
        public MMultiAxes_YMC m_AxCamera1;
        public MMultiAxes_YMC m_AxScannerZ1;
#endif

#if EQUIP_266_DEV
        public MMultiAxes_ACS m_AxCamera1     ;
        public MMultiAxes_ACS m_AxScannerZ1   ;
#endif

        // IO
        public IIO m_IO { get; private set; }

        ///////////////////////////////////////////////////////////////////////
        // Mechanical Layer
        // Cylinder
        public ICylinder m_PushPullGripperCyl;
        public ICylinder m_PushPullUDCyl     ;

        public ICylinder m_Spinner1UDCyl     ;     // Wafer Coater Up Down Cylinder [Double]
        public ICylinder m_Spinner1DICyl     ;     // Wafer Coater DI Nozzle On Off [Single]
        public ICylinder m_Spinner1PVACyl    ;    // Wafer Coater PVA Nozzle On Off [Single]

        public ICylinder m_Spinner2UDCyl     ;     // Wafer Coater Up Down Cylinder [Double]
        public ICylinder m_Spinner2DICyl     ;    // Wafer Cleaner DI Nozzle On Off [Single]
        public ICylinder m_Spinner2PVACyl    ;    // Wafer Cleaner N2 Nozzle On Off [Single]

        public ICylinder m_StageClamp1       ;  // Work Stage Clamp 1 Cylinder [Double]
        public ICylinder m_StageClamp2       ;  // Work Stage Clamp 2 Cylinder [Double]

        // Vacuum
        public IVacuum m_Stage1Vac           ;
        public IVacuum m_UHandlerSelfVac     ;
        public IVacuum m_UHandlerFactoryVac  ;
        public IVacuum m_LHandlerSelfVac     ;
        public IVacuum m_LHandlerFactoryVac  ;
        public IVacuum m_Spinner1Vac         ;
        public IVacuum m_Spinner2Vac         ;
        
        // Vision
        public MVision m_Vision { get; set; }

        // Polygon Scanner
        public MMeScannerPolygon m_MeScanner;

        ///////////////////////////////////////////////////////////////////////
        // Mechanical Layer

        public MMeElevator m_MeElevator    ;         // Cassette Loader 용 Elevator
        public MMeHandler  m_MeUpperHandler;         // UpperHandler of 2Layer
        public MMeHandler  m_MeLowerHandler;         // LowerHandler of 2Layer
        public MMeStage    m_MeStage       ;         
        public MMePushPull m_MePushPull    ;
        public MMeSpinner  m_MeSpinner1    ;
        public MMeSpinner  m_MeSpinner2    ;

        public MOpPanel m_OpPanel;

        ///////////////////////////////////////////////////////////////////////
        // Control Layer
        public MCtrlOpPanel m_ctrlOpPanel { get; private set; }
        public MCtrlLoader m_ctrlLoader { get; private set; }
        public MCtrlPushPull m_ctrlPushPull { get; private set; }
        public MCtrlSpinner m_ctrlSpinner1 { get; private set; }
        public MCtrlSpinner m_ctrlSpinner2 { get; private set; }
        public MCtrlHandler m_ctrlHandler { get; private set; }
        public MCtrlStage1 m_ctrlStage1 { get; private set; }


        ///////////////////////////////////////////////////////////////////////
        // Process Layer
        public MTrsAutoManager m_trsAutoManager { get; private set; }
        public MTrsLoader m_trsLoader { get; private set; }
        public MTrsPushPull m_trsPushPull { get; private set; }
        public MTrsSpinner m_trsSpinner1 { get; private set; }
        public MTrsSpinner m_trsSpinner2 { get; private set; }
        public MTrsHandler m_trsHandler { get; private set; }
        public MTrsStage1 m_trsStage1 { get; private set; }

        public FormIntro intro = new FormIntro();

        public MLWDicer(CObjectInfo objInfo)
            : base(objInfo)
        {
        }

        ~MLWDicer()
        {
            Dispose();
        }

        public void Dispose()
        {
            // close handle
            
        }

        public void CloseSystem()
        {
            StopThreads();

#if !SIMULATION_VISION
            m_Vision.CloseVisionSystem();
            m_ACS.CloseController();
#endif
        }

        public void TestFunction_BeforeInit()
        {
            if(false)
            {
                string a1 = "";
                string a2 = "     ";
                string a3 = "null";
                string a4 = null;

                bool b;
                b = string.IsNullOrEmpty(a1);
                b = string.IsNullOrEmpty(a2);
                b = string.IsNullOrEmpty(a3);
                b = string.IsNullOrEmpty(a4);

                b = string.IsNullOrWhiteSpace(a1);
                b = string.IsNullOrWhiteSpace(a2);
                b = string.IsNullOrWhiteSpace(a3);
                b = string.IsNullOrWhiteSpace(a4);
            }

        }

        public void TestFunction_AfterInit()
        {
            // test io
            if(false)
            {
                bool bStatus;
                int addr = oUHandler_Self_Vac_On;
                m_IO.OutputOn(addr);
                m_IO.IsOn(addr, out bStatus);
                m_IO.OutputOff(addr);
                m_IO.IsOn(addr, out bStatus);
                m_IO.OutputOn(addr);
                m_IO.IsOn(addr, out bStatus);

                m_ctrlHandler.IsObjectDetected(EHandlerIndex.LOAD_UPPER, out bStatus);
                m_ctrlHandler.IsObjectDetected(EHandlerIndex.UNLOAD_LOWER, out bStatus);
                m_ctrlHandler.IsObjectDetected(EHandlerIndex.LOAD_UPPER, out bStatus);
            }

            // test load alarm info
            if(false)
            {
                int iResult = m_ctrlPushPull.MoveToLoaderPos(false);
                CAlarm alarm = GetAlarmInfo(0, iResult);

                iResult += 1;
                alarm = GetAlarmInfo(0, iResult);

                m_DataManager.LoadAlarmHistory();
            }

            // test load parameter
            if (false)
            {
                CParaInfo pinfo;
                GetParameterInfo("Test", "Name1", out pinfo);
            }

            // test process WorkPiece
            if (false)
            {
                
        }

            // test post message
            if (false)
            {
                m_trsSpinner1.PostMsg(EThreadChannel.TrsPushPull, DEF_Thread.EThreadMessage.MSG_SPINNER_PUSHPULL_COMPLETE_LOADING);
                m_trsSpinner1.PostMsg(EThreadChannel.TrsPushPull, DEF_Thread.EThreadMessage.MSG_SPINNER_PUSHPULL_COMPLETE_UNLOADING);
                m_trsSpinner1.PostMsg(EThreadChannel.TrsPushPull, DEF_Thread.EThreadMessage.MSG_SPINNER_PUSHPULL_START_UNLOADING);

                m_trsSpinner2.PostMsg(EThreadChannel.TrsPushPull, DEF_Thread.EThreadMessage.MSG_SPINNER_PUSHPULL_COMPLETE_LOADING);
                m_trsSpinner2.PostMsg(EThreadChannel.TrsPushPull, DEF_Thread.EThreadMessage.MSG_SPINNER_PUSHPULL_COMPLETE_UNLOADING);
                m_trsSpinner2.PostMsg(EThreadChannel.TrsPushPull, DEF_Thread.EThreadMessage.MSG_SPINNER_PUSHPULL_START_UNLOADING);
            }
        }

        public void GetParameterInfo(string group, string name, out CParaInfo pinfo)
        {
            m_DataManager.LoadParaInfo(group, name, out pinfo);
        }

        public CAlarm GetAlarmInfo(int alarmcode, int pid = 0, bool saveLog = true)
        {
            CAlarm alarm = new CAlarm();
            // Process ID가 400부터 시작하기 때문에
            alarm.ProcessID = (pid == 0) ? 400 : pid + 400 - 1; 
            if(pid < 400)
            {
                if (pid == 0) alarm.ProcessID = 400;
                else alarm.ProcessID = pid + 400 - 1;
            } else
            {
                alarm.ProcessID = pid;
            }
            alarm.ObjectID = (int)((alarmcode & 0xffff0000) >> 16);
            alarm.ErrorBase = (int)((alarmcode & 0x0000ffff) / 100) * 100;
            alarm.ErrorCode = (int)((alarmcode & 0x0000ffff) % 100);

            alarm.ProcessName = m_SystemInfo.GetObjectName(alarm.ProcessID);
            alarm.ProcessType = m_SystemInfo.GetTypeName(alarm.ProcessID);
            alarm.ObjectName = m_SystemInfo.GetObjectName(alarm.ObjectID);
            alarm.ObjectType = m_SystemInfo.GetTypeName(alarm.ObjectID);
            m_DataManager.LoadAlarmInfo(alarm.GetIndex(), out alarm.Info);

            if(saveLog == true)
            {
                m_DataManager.SaveAlarmHistory(alarm);
            }
            return alarm;
        }

        public string GetAlarmText(int alarmcode, ELanguage language = ELanguage.ENGLISH)
        {
            CAlarm alarm = GetAlarmInfo(alarmcode, 0, false);
            return alarm.Info.Description[(int)language];
        }

        public string GetAlarmSolution(int alarmcode, ELanguage language = ELanguage.ENGLISH)
        {
            CAlarm alarm = GetAlarmInfo(alarmcode, 0, false);
            return alarm.Info.Solution[(int)language];
        }

        public void ShowAlarmWhileInit(int alarmcode)
        {
            string str = GetAlarmText(alarmcode);
            CMainFrame.DisplayMsg(str);
        }

        /// <summary>
        /// UI 와의 연결을 위해서 form 과 MDataManager를 먼저 define
        /// </summary>
        /// <param name="form1"></param>
        /// <param name="dataManager"></param>
        /// <returns></returns>
        public int Initialize(CMainFrame form1, out MDataManager dataManager)
        {
            int iResult = SUCCESS;
            TestFunction_BeforeInit();

            intro.Show();
            intro.SetStatus("Init Common Class", 10);

            ////////////////////////////////////////////////////////////////////////
            // 0. Common Class
            ////////////////////////////////////////////////////////////////////////
            // init data file name
            CDBInfo dbInfo;
            InitDataFileNames(out dbInfo);
            CObjectInfo.DBInfo = dbInfo;
            MLog.DBInfo = dbInfo;
            CMainFrame.DBInfo = dbInfo;

            CObjectInfo objInfo;
            m_SystemInfo = new MSystemInfo();

            // self set MLWDicer
            m_SystemInfo.GetObjectInfo(0, out objInfo);
            this.ObjInfo = objInfo;

            // DataManager
            m_SystemInfo.GetObjectInfo(1, out objInfo);
            m_DataManager = new MDataManager(objInfo, dbInfo);
            dataManager = m_DataManager;
           iResult = m_DataManager.Initialize();
            CMainFrame.DisplayAlarmOnly(iResult);

            intro.SetStatus("Init Hardware Layer", 20);

            ////////////////////////////////////////////////////////////////////////
            // 1. Hardware Layer
            ////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////
            // Motion
            m_SystemInfo.GetObjectInfo(3, out objInfo);
            iResult = CreateYMCBoard(objInfo);
            CMainFrame.DisplayAlarmOnly(iResult);

            m_SystemInfo.GetObjectInfo(4, out objInfo);
            iResult = CreateACSChannel(objInfo);
            CMainFrame.DisplayAlarmOnly(iResult);

            ////////////////////////////////////////////////////////////////////////
            // MultiAxes
            iResult = CreateMultiAxes_YMC();
            CMainFrame.DisplayAlarmOnly(iResult);

            iResult = CreateMultiAxes_ACS();
            CMainFrame.DisplayAlarmOnly(iResult);

            ////////////////////////////////////////////////////////////////////////
            // IO
            m_SystemInfo.GetObjectInfo(6, out objInfo);
            m_IO = new MIO_YMC(objInfo);
            iResult = m_IO.Initialize();
            CMainFrame.DisplayAlarmOnly(iResult);

            ////////////////////////////////////////////////////////////////////////
            // Cylinder
            CCylinderData cylData;

            // PushPullGripperCyl
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.UP_DOWN;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.UpSensor[0] = iUHandler_Up1;
            cylData.DownSensor[0] = iUHandler_Down2;
            cylData.Solenoid[0] = oUHandler_Up1;
            cylData.Solenoid[1] = oUHandler_Down2;

            m_SystemInfo.GetObjectInfo(100, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.PUSHPULL_GRIPPER, out m_PushPullGripperCyl);
            CMainFrame.DisplayAlarmOnly(iResult);

            // PushPullUDCyl
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.UP_DOWN;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.UpSensor[0] = iUHandler_Up1;
            cylData.DownSensor[0] = iUHandler_Down2;
            cylData.Solenoid[0] = oUHandler_Up1;
            cylData.Solenoid[1] = oUHandler_Down2;

            m_SystemInfo.GetObjectInfo(101, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.PUSHPULL_UD, out m_PushPullUDCyl);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Spinner1
            // Spin Coater Up & Down Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.UP_DOWN;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.UpSensor[0] = iStage1Up;
            cylData.DownSensor[0] = iStage1Down;
            cylData.Solenoid[0] = oStage1_Up;
            cylData.Solenoid[1] = oStage1_Down;

            m_SystemInfo.GetObjectInfo(102, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER1_UD, out m_Spinner1UDCyl);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Spin Coater DI Valve Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.SINGLE_SOLENOID;
            cylData.Solenoid[0] = oCoat_DI;

            m_SystemInfo.GetObjectInfo(103, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER1_DI, out m_Spinner1DICyl);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Spin Coater PVA Valve Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.SINGLE_SOLENOID;
            cylData.Solenoid[0] = oCoat_PVA;

            m_SystemInfo.GetObjectInfo(104, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER1_PVA, out m_Spinner1PVACyl);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Spin Cleaner Up & Down Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.UP_DOWN;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.UpSensor[0] = iStage2Up;
            cylData.DownSensor[0] = iStage2Down;
            cylData.Solenoid[0] = oStage2_Up;
            cylData.Solenoid[1] = oStage2_Down;

            m_SystemInfo.GetObjectInfo(105, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER2_UD, out m_Spinner2UDCyl);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Spin Cleaner DI Valve Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.SINGLE_SOLENOID;
            cylData.Solenoid[0] = oClean_DI;

            m_SystemInfo.GetObjectInfo(106, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER2_DI, out m_Spinner2DICyl);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Spin Cleaner N2 Valve Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.SINGLE_SOLENOID;
            cylData.Solenoid[0] = oClean_N2;

            m_SystemInfo.GetObjectInfo(107, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER2_PVA, out m_Spinner2PVACyl);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Stage Clamp 1 Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.Solenoid[0] = oStageClamp1_Open;
            cylData.Solenoid[1] = oStageClamp1_Close;

            m_SystemInfo.GetObjectInfo(108, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.STAGE_CLAMP1, out m_StageClamp1);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Stage Clamp 2 Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.Solenoid[0] = oStageClamp2_Open;
            cylData.Solenoid[1] = oStageClamp2_Close;

            m_SystemInfo.GetObjectInfo(109, out objInfo);
            iResult = CreateCylinder(objInfo, cylData, (int)EObjectCylinder.STAGE_CLAMP2, out m_StageClamp2);
            CMainFrame.DisplayAlarmOnly(iResult);

            ////////////////////////////////////////////////////////////////////////
            // Vacuum
            // Stage1 Vacuum
            CVacuumData vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iStage1_Vac_On;
            vacData.Solenoid[0] = oStage1_Vac_On;
            vacData.Solenoid[1] = oStage1_Vac_Off;

            m_SystemInfo.GetObjectInfo(150, out objInfo);
            iResult = CreateVacuum(objInfo, vacData, (int)EObjectVacuum.STAGE1, out m_Stage1Vac);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Spinner1 Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.DOUBLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iStage2_PanelDetect;
            vacData.Solenoid[0] = oStage2_Vac_On;
            vacData.Solenoid[1] = oStage2_Vac_Off;
            vacData.Solenoid[2] = oStage2_Blow;

            m_SystemInfo.GetObjectInfo(151, out objInfo);
            iResult = CreateVacuum(objInfo, vacData, (int)EObjectVacuum.SPINNER1, out m_Spinner1Vac);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Spinner2 Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.DOUBLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iStage3_PanelDetect;
            vacData.Solenoid[0] = oStage3_Vac_On;
            vacData.Solenoid[1] = oStage3_Vac_Off;
            vacData.Solenoid[2] = oStage3_Blow;

            m_SystemInfo.GetObjectInfo(152, out objInfo);
            iResult = CreateVacuum(objInfo, vacData, (int)EObjectVacuum.SPINNER2, out m_Spinner2Vac);
            CMainFrame.DisplayAlarmOnly(iResult);

            // UpperHandler
            // UpperHandler Self Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iUHandler_Self_Vac_On;
            vacData.Solenoid[0] = oUHandler_Self_Vac_On;
            vacData.Solenoid[1] = oUHandler_Self_Vac_Off;

            m_SystemInfo.GetObjectInfo(153, out objInfo);
            iResult = CreateVacuum(objInfo, vacData, (int)EObjectVacuum.UPPER_HANDLER_SELF, out m_UHandlerSelfVac);
            CMainFrame.DisplayAlarmOnly(iResult);

            // UpperHandler Factory Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iUHandler_Self_Vac_On;
            vacData.Solenoid[0] = oUHandler_Self_Vac_On;
            vacData.Solenoid[1] = oUHandler_Self_Vac_Off;

            m_SystemInfo.GetObjectInfo(154, out objInfo);
            iResult = CreateVacuum(objInfo, vacData, (int)EObjectVacuum.UPPER_HANDLER_FACTORY, out m_UHandlerFactoryVac);
            CMainFrame.DisplayAlarmOnly(iResult);

            // LowerHandler
            // LowerHandler Self Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            //vacData.Sensor[0] = iLHandler_Self_Vac_On;
            //vacData.Solenoid[0] = oLHandler_Self_Vac_On;
            //vacData.Solenoid[1] = oLHandler_Self_Vac_Off;

            m_SystemInfo.GetObjectInfo(155, out objInfo);
            iResult = CreateVacuum(objInfo, vacData, (int)EObjectVacuum.LOWER_HANDLER_SELF, out m_LHandlerSelfVac);
            CMainFrame.DisplayAlarmOnly(iResult);

            // LowerHandler Factory Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            //vacData.Sensor[0] = iLHandler_Self_Vac_On;
            //vacData.Solenoid[0] = oLHandler_Self_Vac_On;
            //vacData.Solenoid[1] = oLHandler_Self_Vac_Off;

            m_SystemInfo.GetObjectInfo(156, out objInfo);
            iResult = CreateVacuum(objInfo, vacData, (int)EObjectVacuum.LOWER_HANDLER_FACTORY, out m_LHandlerFactoryVac);
            CMainFrame.DisplayAlarmOnly(iResult);

            ////////////////////////////////////////////////////////////////////////
            // Vision
            // Vision System

            m_SystemInfo.GetObjectInfo(40, out objInfo);
            iResult = CreateVisionSystem(objInfo);
            CMainFrame.DisplayAlarmOnly(iResult);

            // Vision Camera
            m_SystemInfo.GetObjectInfo(42, out objInfo);
            iResult = CreateVisionCamera(objInfo, PRE__CAM);
            CMainFrame.DisplayAlarmOnly(iResult);

            m_SystemInfo.GetObjectInfo(43, out objInfo);
            iResult = CreateVisionCamera(objInfo, FINE_CAM);
            CMainFrame.DisplayAlarmOnly(iResult);

            m_SystemInfo.GetObjectInfo(44, out objInfo);
            iResult = CreateVisionCamera(objInfo, INSP_CAM);
            CMainFrame.DisplayAlarmOnly(iResult);

//#if !SIMULATION_VISION
            // Vision Display
            m_SystemInfo.GetObjectInfo(46, out objInfo);
            iResult = CreateVisionVisionView(objInfo, PRE__CAM);
            CMainFrame.DisplayAlarmOnly(iResult);

            m_SystemInfo.GetObjectInfo(47, out objInfo);
            iResult = CreateVisionVisionView(objInfo, FINE_CAM);
            CMainFrame.DisplayAlarmOnly(iResult);

            m_SystemInfo.GetObjectInfo(47, out objInfo);
            iResult = CreateVisionVisionView(objInfo, INSP_CAM);
            CMainFrame.DisplayAlarmOnly(iResult);
//#endif


            intro.SetStatus("Init Mechanical Layer", 30);

            ////////////////////////////////////////////////////////////////////////
            // 2. Mechanical Layer
            ////////////////////////////////////////////////////////////////////////

            // OpPanel
            m_SystemInfo.GetObjectInfo(300, out objInfo);
            CreateMeOpPanel(objInfo);

            // Elevator
            m_SystemInfo.GetObjectInfo(301, out objInfo);
            CreateMeElevator(objInfo);

            // PushPull
            m_SystemInfo.GetObjectInfo(302, out objInfo);
            CreateMePushPull(objInfo);

            // Stage1
            m_SystemInfo.GetObjectInfo(303, out objInfo);
            CreateMeStage(objInfo);

            // Spinner
            m_SystemInfo.GetObjectInfo(304, out objInfo);
            CreateMeSpinner1(objInfo);

            m_SystemInfo.GetObjectInfo(305, out objInfo);
            CreateMeSpinner2(objInfo);

            // Handler
            m_SystemInfo.GetObjectInfo(306, out objInfo);
            CreateMeUHandler(objInfo);

            m_SystemInfo.GetObjectInfo(307, out objInfo);
            CreateMeLHandler(objInfo);

            // Vision 
            m_SystemInfo.GetObjectInfo(308, out objInfo);
            CreateVision(objInfo);

            // Scanner
            m_SystemInfo.GetObjectInfo(309, out objInfo);
            CreateScanner(objInfo);

            intro.SetStatus("Init Control Layer", 40);

            ////////////////////////////////////////////////////////////////////////
            // 3. Control Layer
            ////////////////////////////////////////////////////////////////////////
            m_SystemInfo.GetObjectInfo(351, out objInfo);
            CreateCtrlLoader(objInfo);

            m_SystemInfo.GetObjectInfo(352, out objInfo);
            CreateCtrlPushPull(objInfo);

            m_SystemInfo.GetObjectInfo(353, out objInfo);
            CreateCtrlStage1(objInfo);

            m_SystemInfo.GetObjectInfo(354, out objInfo);
            CreateCtrlSpinner1(objInfo);

            m_SystemInfo.GetObjectInfo(355, out objInfo);
            CreateCtrlSpinner2(objInfo);

            m_SystemInfo.GetObjectInfo(356, out objInfo);
            CreateCtrlHandler(objInfo);

            m_SystemInfo.GetObjectInfo(350, out objInfo);
            CreateCtrlOpPanel(objInfo);

            intro.SetStatus("Init Process Layer", 50);

            ////////////////////////////////////////////////////////////////////////
            // 4. Process Layer
            ////////////////////////////////////////////////////////////////////////
            m_SystemInfo.GetObjectInfo(401, out objInfo);
            CreateTrsLoader(objInfo);

            m_SystemInfo.GetObjectInfo(402, out objInfo);
            CreateTrsPushPull(objInfo);

            m_SystemInfo.GetObjectInfo(403, out objInfo);
            CreateTrsStage1(objInfo);

            m_SystemInfo.GetObjectInfo(404, out objInfo);
            CreateTrsSpinner1(objInfo);

            m_SystemInfo.GetObjectInfo(405, out objInfo);
            CreateTrsSpinner2(objInfo);

            m_SystemInfo.GetObjectInfo(406, out objInfo);
            CreateTrsHandler(objInfo);

            m_SystemInfo.GetObjectInfo(400, out objInfo);
            CreateTrsAutoManager(objInfo);

            // temporary set windows
            m_trsLoader.SetWindows_Form1(form1);
            m_trsPushPull.SetWindows_Form1(form1);
            m_trsStage1.SetWindows_Form1(form1);
            m_trsSpinner1.SetWindows_Form1(form1);
            m_trsSpinner2.SetWindows_Form1(form1);
            m_trsHandler.SetWindows_Form1(form1);
            m_trsAutoManager.SetWindows_Form1(form1);

            ////////////////////////////////////////////////////////////////////////
            // 5. Set Data
            ////////////////////////////////////////////////////////////////////////
            intro.SetStatus("Loading System Data", 60);
            iResult = SetSystemDataToComponent(false);
            CMainFrame.DisplayAlarmOnly(iResult);

            intro.SetStatus("Loading Model Data", 70);
            iResult = SetModelDataToComponent();
            CMainFrame.DisplayAlarmOnly(iResult);

            intro.SetStatus("Loading Position Data", 80);
            iResult = SetPositionDataToComponent();
            CMainFrame.DisplayAlarmOnly(iResult);

            ////////////////////////////////////////////////////////////////////////
            // 6. Start Thread & System
            ////////////////////////////////////////////////////////////////////////
            iResult = m_YMC.ThreadStart();
            CMainFrame.DisplayAlarmOnly(iResult);

            intro.SetStatus("Process Start", 90);

            SetThreadChannel();
            StartThreads();

            TestFunction_AfterInit();

            intro.Hide();

            iResult = m_DataManager.Logout(); // logoff maker
            CMainFrame.DisplayAlarmOnly(iResult);

            return SUCCESS;
        }

        void InitDataFileNames(out CDBInfo dbInfo)
        {
            dbInfo = new CDBInfo();
        }

        int CreateYMCBoard(CObjectInfo objInfo)
        {
            CYaskawaRefComp refComp = new CYaskawaRefComp();
            CYaskawaData data = new CYaskawaData();

            m_YMC = new MYaskawa(objInfo, refComp, data);
            m_YMC.SetMPMotionData(m_DataManager.SystemData_Axis.MPMotionData);

#if !SIMULATION_MOTION_YMC
            int iResult = m_YMC.OpenController();
            if (iResult != SUCCESS) return iResult;
#endif

            return SUCCESS;
        }

        int CreateACSChannel(CObjectInfo objInfo)
        {
            CACSRefComp refComp = new CACSRefComp();
            CACSData data = new CACSData();

            m_ACS = new MACS(objInfo, refComp, data);
            m_ACS.SetACSMotionData(m_DataManager.SystemData_Axis.ACSMotionData);

#if !SIMULATION_MOTION_ACS
            int iResult = m_ACS.OpenController();
            if (iResult != SUCCESS) return iResult;
#endif

            return SUCCESS;
        }
        
        int CreateMultiAxes_YMC()
        {
            CObjectInfo objInfo;
            CMutliAxesYMCRefComp refComp = new CMutliAxesYMCRefComp();
            CMultiAxesYMCData data;
            int deviceNo;
            int[] axisList = new int[DEF_MAX_COORDINATE];
            int[] initArray = new int[DEF_MAX_COORDINATE];
            ArrayExtensions.Init(initArray, DEF_AXIS_NONE_ID);

            refComp.Motion = m_YMC;

            // Loader
            deviceNo = (int)EYMC_Device.LOADER;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EYMC_Axis.LOADER_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(251, out objInfo);
            m_AxLoader = new MMultiAxes_YMC(objInfo, refComp, data);

            // PushPull
            deviceNo = (int)EYMC_Device.PUSHPULL;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Y] = (int)EYMC_Axis.PUSHPULL_Y;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(252, out objInfo);
            m_AxPushPull = new MMultiAxes_YMC(objInfo, refComp, data);

            // CENTERING1
            deviceNo = (int)EYMC_Device.CENTERING1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.PUSHPULL_X1;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(253, out objInfo);
            m_AxCentering1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // S1_ROTATE
            deviceNo = (int)EYMC_Device.S1_ROTATE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.S1_CHUCK_ROTATE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(254, out objInfo);
            m_AxRotate1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // S1_CLEAN_NOZZLE
            deviceNo = (int)EYMC_Device.S1_CLEAN_NOZZLE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.S1_CLEAN_NOZZLE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(255, out objInfo);
            m_AxCleanNozzle1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // S1_COAT_NOZZLE
            deviceNo = (int)EYMC_Device.S1_COAT_NOZZLE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.S1_COAT_NOZZLE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(256, out objInfo);
            m_AxCoatNozzle1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // CENTERING2
            deviceNo = (int)EYMC_Device.CENTERING2;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.PUSHPULL_X2;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(257, out objInfo);
            m_AxCentering2 = new MMultiAxes_YMC(objInfo, refComp, data);

            // S2_ROTATE
            deviceNo = (int)EYMC_Device.S2_ROTATE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.S2_CHUCK_ROTATE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(258, out objInfo);
            m_AxRotate2 = new MMultiAxes_YMC(objInfo, refComp, data);

            // S2_CLEAN_NOZZLE
            deviceNo = (int)EYMC_Device.S2_CLEAN_NOZZLE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.S2_CLEAN_NOZZLE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(259, out objInfo);
            m_AxCleanNozzle2 = new MMultiAxes_YMC(objInfo, refComp, data);

            // S2_COAT_NOZZLE
            deviceNo = (int)EYMC_Device.S2_COAT_NOZZLE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.S2_COAT_NOZZLE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(260, out objInfo);
            m_AxCoatNozzle2 = new MMultiAxes_YMC(objInfo, refComp, data);

            // UPPER_HANDLER
            deviceNo = (int)EYMC_Device.UPPER_HANDLER;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_X] = (int)EYMC_Axis.UPPER_HANDLER_X;
            axisList[DEF_Z] = (int)EYMC_Axis.UPPER_HANDLER_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(261, out objInfo);
            m_AxUpperHandler = new MMultiAxes_YMC(objInfo, refComp, data);

            // LOWER_HANDLER
            deviceNo = (int)EYMC_Device.LOWER_HANDLER;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_X] = (int)EYMC_Axis.LOWER_HANDLER_X;
            axisList[DEF_Z] = (int)EYMC_Axis.LOWER_HANDLER_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(262, out objInfo);
            m_AxLowerHandler = new MMultiAxes_YMC(objInfo, refComp, data);

#if EQUIP_DICING_DEV
            // CAMERA1
            deviceNo = (int)EYMC_Device.CAMERA1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EYMC_Axis.CAMERA1_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(263, out objInfo);
            m_AxCamera1 = new MMultiAxes_YMC(objInfo, refComp, data);

            //// SCANNER1
            deviceNo = (int)EYMC_Device.SCANNER1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EYMC_Axis.SCANNER_Z1;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(264, out objInfo);
            m_AxScannerZ1 = new MMultiAxes_YMC(objInfo, refComp, data);
#endif

            return SUCCESS;
        }

        int CreateMultiAxes_ACS()
        {
            CObjectInfo objInfo;
            CMutliAxesACSRefComp refComp = new CMutliAxesACSRefComp();
            CMultiAxesACSData data;
            int deviceNo;
            int[] axisList = new int[DEF_MAX_COORDINATE];
            int[] initArray = new int[DEF_MAX_COORDINATE];
            ArrayExtensions.Init(initArray, DEF_AXIS_NONE_ID);

            refComp.Motion = m_ACS;

#if EQUIP_266_DEV
            // Scanner Z
            deviceNo = (int)EACS_Device.SCANNER_Z1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EACS_Device.SCANNER_Z1;
            data = new CMultiAxesACSData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(270, out objInfo);
            m_AxScannerZ1 = new MMultiAxes_ACS(objInfo, refComp, data);

            // Camera Z
            deviceNo = (int)EACS_Device.CAMERA1_Z;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EACS_Device.CAMERA1_Z;
            data = new CMultiAxesACSData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(271, out objInfo);
            m_AxCamera1 = new MMultiAxes_ACS(objInfo, refComp, data);
#endif

            // Stage
            deviceNo = (int)EACS_Device.STAGE1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_X] = (int)EACS_Device.STAGE1_X;
            axisList[DEF_Y] = (int)EACS_Device.STAGE1_Y;
            axisList[DEF_T] = (int)EACS_Device.STAGE1_T;
            data = new CMultiAxesACSData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(272, out objInfo);
            m_AxStage1 = new MMultiAxes_ACS(objInfo, refComp, data);

            return SUCCESS;
        }

        int CreateCylinder(CObjectInfo objInfo, CCylinderData data, int objIndex, out ICylinder pCylinder)
        {
            int iResult = SUCCESS;

            data.Time = m_DataManager.SystemData_Cylinder.CylinderTimer[objIndex];
            pCylinder = new MCylinder(objInfo, m_IO, data);

            return iResult;
        }

        int CreateVacuum(CObjectInfo objInfo, CVacuumData data, int objIndex, out IVacuum pVacuum)
        {
            int iResult = SUCCESS;

            data.Time = m_DataManager.SystemData_Vacuum.VacuumTimer[objIndex];
            pVacuum = new MVacuum(objInfo, m_IO, data);

            return iResult;
        }

        int CreateVisionSystem(CObjectInfo objInfo)
        {
//#if SIMULATION_VISION
//                return SUCCESS;
//#endif

            int iResult = 0;
            // Vision System 생성
            m_VisionSystem = new MVisionSystem(objInfo);
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // GigE Cam초기화 & MIL 초기화
            iResult = m_VisionSystem.Initialize();

            if (iResult != SUCCESS) return iResult;

            // GigE Camera 개수 확인
            int iGetCamNum = m_VisionSystem.GetCamNum();
            if (iGetCamNum != DEF_MAX_CAMERA_NO) return ERR_VISION_ERROR;

            return SUCCESS;
        }

        int CreateVisionCamera(CObjectInfo objInfo,int iNum)
        {
            // Camera를 생성함.
            m_VisionCamera[iNum] = new MVisionCamera(objInfo);

#if SIMULATION_VISION
            return SUCCESS;
#endif

            // Vision Library MIL
            m_VisionCamera[iNum].SetMil_ID(m_VisionSystem.GetMilSystem());
            // Camera 초기화
            m_VisionCamera[iNum].Initialize(iNum, m_VisionSystem.GetSystem());
                
            return SUCCESS;
        }
        int CreateVisionVisionView(CObjectInfo objInfo, int iNum)
        {
            // Display View 생성함.
            m_VisionView[iNum] = new MVisionView(objInfo);

#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision Library MIL
            m_VisionView[iNum].SetMil_ID(m_VisionSystem.GetMilSystem());
            // Display 초기화
            m_VisionView[iNum].Initialize(iNum, m_VisionCamera[iNum]);
            
            return SUCCESS;
        }

        void CreateVision(CObjectInfo objInfo)
        {
//#if SIMULATION_VISION
//            return;
//#endif
            bool VisionHardwareCheck = true;
            if (m_VisionSystem.m_iResult != SUCCESS)
            {
                VisionHardwareCheck = false;
            }
            CCameraData data = new CCameraData();
            CVisionRefComp refComp = new CVisionRefComp();

            // 생성된 Vision System,Camera, View 를 RefComp로 연결
            refComp.System = m_VisionSystem;

            for (int iNum = 0; iNum < DEF_MAX_CAMERA_NO; iNum++)
            {
                if(m_VisionCamera[iNum].m_iResult != SUCCESS || m_VisionView[iNum].m_iResult != SUCCESS)
                {
                    VisionHardwareCheck = false;
                    break;
                }
                refComp.Camera[iNum] = m_VisionCamera[iNum];
                refComp.View[iNum]   = m_VisionView[iNum];

                // Display와 Camera와 System을 연결
                refComp.Camera[iNum].SelectView(refComp.View[iNum]);
                refComp.System.SelectCamera(refComp.Camera[iNum]);
                refComp.System.SelectView(refComp.View[iNum]);
            }

            m_Vision = new MVision(objInfo, refComp, data);

            if(VisionHardwareCheck==false)
            {
                m_Vision.m_bSystemInit = false;
            }
            else
            {
                m_Vision.m_bSystemInit = true;
            }

            // Cam Live Set
            m_Vision.LiveVideo(PRE__CAM);
            m_Vision.LiveVideo(FINE_CAM);
            
          
        }

        void CreateScanner(CObjectInfo objInfo)
        {
            string hostAddress;
            int hostPort;

            hostAddress = m_DataManager.SystemData_Scan.ControlHostAddress;
            hostPort = m_DataManager.SystemData_Scan.ControlHostPort;
            var controlCommData = new CSocketClientData(hostAddress, hostPort);

            hostAddress = m_DataManager.SystemData_Scan.ScanHeadHostAddress;
            hostPort = m_DataManager.SystemData_Scan.ScanHeadHostPort;
            var scanHeadCommData = new CSocketClientData(hostAddress, hostPort);

            //m_SystemInfo.GetObjectInfo(10, out objInfo);
            //m_ScanManager = new CMarkingManager(objInfo);
            //m_SystemInfo.GetObjectInfo(11, out objInfo);
            //m_ScanWindow = new CMarkingWindow(objInfo);
            //m_FormScanner = new FormScanWindow();

            m_SystemInfo.GetObjectInfo(12, out objInfo);
            m_ControlComm = new MSocketClient(objInfo, controlCommData);
            m_SystemInfo.GetObjectInfo(13, out objInfo);
            m_ScanHeadComm = new MSocketClient(objInfo, scanHeadCommData);
                        

            CScannerRefComp refComp = new CScannerRefComp();

            //refComp.Manager = m_ScanManager;
            //refComp.Window = m_ScanWindow;
            //refComp.FormScanner = m_FormScanner;

            refComp.ControlComm     = m_ControlComm;
            refComp.ScanHeadComm    = m_ScanHeadComm;            
            refComp.Process         = m_ACS;
            refComp.DataManager     = m_DataManager;

            m_MeScanner = new MMeScannerPolygon(objInfo, refComp);
        }

            void CreateCtrlOpPanel(CObjectInfo objInfo)
        {
            CCtrlOpPanelRefComp refComp = new CCtrlOpPanelRefComp();
            CCtrlOpPanelData data = new CCtrlOpPanelData();

            refComp.IO = m_IO;
            refComp.OpPanel = m_OpPanel;

            m_ctrlOpPanel = new MCtrlOpPanel(objInfo, refComp, data);
        }

        void CreateCtrlStage1(CObjectInfo objInfo)
        {
            CCtrlStage1RefComp refComp = new CCtrlStage1RefComp();
            CCtrlStage1Data data = new CCtrlStage1Data();

            refComp.Stage   = m_MeStage;
            refComp.Scanner = m_MeScanner;
            refComp.Vision  = m_Vision;

            m_ctrlStage1 = new MCtrlStage1(objInfo, refComp, data);
        }

        void CreateCtrlLoader(CObjectInfo objInfo)
        {
            CCtrlLoaderRefComp refComp = new CCtrlLoaderRefComp();
            CCtrlLoaderData data = new CCtrlLoaderData();

            refComp.IO = m_IO;
            refComp.Elevator = m_MeElevator;

            m_ctrlLoader = new MCtrlLoader(objInfo, refComp, data);
        }

        void CreateCtrlHandler(CObjectInfo objInfo)
        {
            CCtrlHandlerRefComp refComp = new CCtrlHandlerRefComp();
            CCtrlHandlerData data = new CCtrlHandlerData();

            refComp.UpperHandler = m_MeUpperHandler;
            refComp.LowerHandler = m_MeLowerHandler;

            m_ctrlHandler = new MCtrlHandler(objInfo, refComp, data);
        }

        void CreateCtrlPushPull(CObjectInfo objInfo)
        {
            CCtrlPushPullRefComp refComp = new CCtrlPushPullRefComp();
            CCtrlPushPullData data = new CCtrlPushPullData();

            refComp.IO = m_IO;
            refComp.PushPull = m_MePushPull;
            refComp.LowerHandler = m_MeLowerHandler;
            refComp.UpperHandler = m_MeUpperHandler;
            refComp.Spinner1 = m_MeSpinner1;
            refComp.Spinner2 = m_MeSpinner2;
            refComp.Elevator = m_MeElevator;

            m_ctrlPushPull = new MCtrlPushPull(objInfo, refComp, data);
        }

        void CreateCtrlSpinner1(CObjectInfo objInfo)
        {
            CCtrlSpinnerRefComp refComp = new CCtrlSpinnerRefComp();
            CCtrlSpinnerData data = m_DataManager.ModelData.SpinnerData[(int)ESpinnerIndex.SPINNER1];

            refComp.Spinner = m_MeSpinner1;

            m_ctrlSpinner1 = new MCtrlSpinner(objInfo, refComp, data);
        }

        void CreateCtrlSpinner2(CObjectInfo objInfo)
        {
            CCtrlSpinnerRefComp refComp = new CCtrlSpinnerRefComp();
            CCtrlSpinnerData data = m_DataManager.ModelData.SpinnerData[(int)ESpinnerIndex.SPINNER2];

            refComp.Spinner = m_MeSpinner2;

            m_ctrlSpinner2 = new MCtrlSpinner(objInfo, refComp, data);
        }

        void CreateTrsAutoManager(CObjectInfo objInfo)
        {
            CTrsAutoManagerRefComp refComp = new CTrsAutoManagerRefComp();
            refComp.IO = m_IO;
            refComp.YMC = m_YMC;
            refComp.ACS = m_ACS;
            refComp.OpPanel = m_OpPanel;

            refComp.ctrlOpPanel = m_ctrlOpPanel;
            refComp.ctrlLoader = m_ctrlLoader;
            refComp.ctrlPushPull = m_ctrlPushPull;
            refComp.ctrlSpinner1 = m_ctrlSpinner1;
            refComp.ctrlSpinner2 = m_ctrlSpinner2;
            refComp.ctrlHandler = m_ctrlHandler;
            refComp.ctrlStage1 = m_ctrlStage1;

            refComp.trsLoader = m_trsLoader;
            refComp.trsPushPull = m_trsPushPull;

            CTrsAutoManagerData data = new CTrsAutoManagerData();

            m_trsAutoManager = new MTrsAutoManager(objInfo, EThreadChannel.TrsAutoManager, m_DataManager, ELCNetUnitPos.NONE, refComp, data);
        }

        void CreateTrsLoader(CObjectInfo objInfo)
        {
            CTrsLoaderRefComp refComp = new CTrsLoaderRefComp();
            refComp.ctrlLoader = m_ctrlLoader;

            CTrsLoaderData data = new CTrsLoaderData();

            m_trsLoader = new MTrsLoader(objInfo, EThreadChannel.TrsLoader, m_DataManager, ELCNetUnitPos.NONE, refComp, data);
        }

        void CreateTrsPushPull(CObjectInfo objInfo)
        {
            CTrsPushPullRefComp refComp = new CTrsPushPullRefComp();
            refComp.ctrlPushPull = m_ctrlPushPull;
            refComp.ctrlLoader = m_ctrlLoader;
            refComp.ctrlHandler = m_ctrlHandler;
            refComp.ctrlCleaner = m_ctrlSpinner1;
            refComp.ctrlCoater = m_ctrlSpinner2;
            refComp.ctrlSpinner[(int)ESpinnerIndex.SPINNER1] = m_ctrlSpinner1;
            refComp.ctrlSpinner[(int)ESpinnerIndex.SPINNER2] = m_ctrlSpinner2;

            CTrsPushPullData data = new CTrsPushPullData();

            m_trsPushPull = new MTrsPushPull(objInfo, EThreadChannel.TrsPushPull, m_DataManager, ELCNetUnitPos.PUSHPULL, refComp, data);
        }

        void CreateTrsSpinner1(CObjectInfo objInfo)
        {
            CTrsSpinnerRefComp refComp = new CTrsSpinnerRefComp();
            refComp.ctrlSpinner = m_ctrlSpinner1;
            refComp.ctrlPushPull = m_ctrlPushPull;

            CTrsSpinnerData data = new CTrsSpinnerData();
            data.SpinnerIndex = ESpinnerIndex.SPINNER1;

            m_trsSpinner1 = new MTrsSpinner(objInfo, EThreadChannel.TrsSpinner1, m_DataManager, ELCNetUnitPos.SPINNER1, refComp, data);
        }

        void CreateTrsSpinner2(CObjectInfo objInfo)
        {
            CTrsSpinnerRefComp refComp = new CTrsSpinnerRefComp();
            refComp.ctrlSpinner = m_ctrlSpinner2;
            refComp.ctrlPushPull = m_ctrlPushPull;

            CTrsSpinnerData data = new CTrsSpinnerData();
            data.SpinnerIndex = ESpinnerIndex.SPINNER2;

            m_trsSpinner2 = new MTrsSpinner(objInfo, EThreadChannel.TrsSpinner2, m_DataManager, ELCNetUnitPos.SPINNER2, refComp, data);
        }

        void CreateTrsHandler(CObjectInfo objInfo)
        {
            CTrsHandlerRefComp refComp = new CTrsHandlerRefComp();
            refComp.ctrlHandler = m_ctrlHandler;
            refComp.ctrlPushPull = m_ctrlPushPull;

            CTrsHandlerData data = new CTrsHandlerData();

            m_trsHandler = new MTrsHandler(objInfo, EThreadChannel.TrsHandler, m_DataManager, ELCNetUnitPos.UPPER_HANDLER, refComp, data);
        }

        void CreateTrsStage1(CObjectInfo objInfo)
        {
            CTrsStage1RefComp refComp = new CTrsStage1RefComp();
            refComp.ctrlStage1 = m_ctrlStage1;
            refComp.ctrlHandler = m_ctrlHandler;

            CTrsStage1Data data = new CTrsStage1Data();

            m_trsStage1 = new MTrsStage1(objInfo, EThreadChannel.TrsStage1, m_DataManager, ELCNetUnitPos.STAGE1, refComp, data);
        }
        
        void SetThreadChannel()
        {
            // AutoManager
            m_trsAutoManager.LinkThread(EThreadChannel.TrsSelfChannel, m_trsAutoManager);
            m_trsAutoManager.LinkThread(EThreadChannel.TrsAutoManager, m_trsAutoManager);
            m_trsAutoManager.LinkThread(EThreadChannel.TrsLoader, m_trsLoader);
            m_trsAutoManager.LinkThread(EThreadChannel.TrsPushPull, m_trsPushPull);
            m_trsAutoManager.LinkThread(EThreadChannel.TrsStage1, m_trsStage1);
            m_trsAutoManager.LinkThread(EThreadChannel.TrsSpinner1, m_trsSpinner1);
            m_trsAutoManager.LinkThread(EThreadChannel.TrsSpinner2, m_trsSpinner2);
            m_trsAutoManager.LinkThread(EThreadChannel.TrsHandler, m_trsHandler);

            // Loader
            m_trsLoader.LinkThread(EThreadChannel.TrsSelfChannel, m_trsLoader);
            m_trsLoader.LinkThread(EThreadChannel.TrsAutoManager, m_trsAutoManager);
            m_trsLoader.LinkThread(EThreadChannel.TrsPushPull, m_trsPushPull);
            m_trsLoader.LinkThread(EThreadChannel.TrsStage1, m_trsStage1);

            // PushPull
            m_trsPushPull.LinkThread(EThreadChannel.TrsSelfChannel, m_trsPushPull);
            m_trsPushPull.LinkThread(EThreadChannel.TrsAutoManager, m_trsAutoManager);
            m_trsPushPull.LinkThread(EThreadChannel.TrsLoader, m_trsLoader);
            m_trsPushPull.LinkThread(EThreadChannel.TrsSpinner1, m_trsSpinner1);
            m_trsPushPull.LinkThread(EThreadChannel.TrsSpinner2, m_trsSpinner2);
            m_trsPushPull.LinkThread(EThreadChannel.TrsHandler, m_trsHandler);

            // Spinner
            m_trsSpinner1.LinkThread(EThreadChannel.TrsSelfChannel, m_trsSpinner1);
            m_trsSpinner1.LinkThread(EThreadChannel.TrsAutoManager, m_trsAutoManager);
            m_trsSpinner1.LinkThread(EThreadChannel.TrsPushPull, m_trsPushPull);

            m_trsSpinner2.LinkThread(EThreadChannel.TrsSelfChannel, m_trsSpinner2);
            m_trsSpinner2.LinkThread(EThreadChannel.TrsAutoManager, m_trsAutoManager);
            m_trsSpinner2.LinkThread(EThreadChannel.TrsPushPull, m_trsPushPull);

            // Handler
            m_trsHandler.LinkThread(EThreadChannel.TrsSelfChannel, m_trsHandler);
            m_trsHandler.LinkThread(EThreadChannel.TrsAutoManager, m_trsAutoManager);
            m_trsHandler.LinkThread(EThreadChannel.TrsPushPull, m_trsPushPull);
            m_trsHandler.LinkThread(EThreadChannel.TrsStage1, m_trsStage1);

            // Stage1
            m_trsStage1.LinkThread(EThreadChannel.TrsSelfChannel, m_trsStage1);
            m_trsStage1.LinkThread(EThreadChannel.TrsAutoManager, m_trsAutoManager);
            m_trsStage1.LinkThread(EThreadChannel.TrsHandler, m_trsHandler);
        }

        void StartThreads()
        {
            m_trsLoader.ThreadStart();
            m_trsPushPull.ThreadStart();
            m_trsStage1.ThreadStart();
            m_trsHandler.ThreadStart();
            m_trsSpinner1.ThreadStart();
            m_trsSpinner2.ThreadStart();
            m_trsAutoManager.ThreadStart();
        }

        public void StopThreads()
        {
            m_trsLoader.ThreadStop();
            m_trsPushPull.ThreadStop();
            m_trsStage1.ThreadStop();
            m_trsHandler.ThreadStop();
            m_trsSpinner1.ThreadStop();
            m_trsSpinner2.ThreadStop();
            m_trsAutoManager.ThreadStop();
        }

        public int SaveSystemData(CSystemData system = null, CSystemData_Axis systemAxis = null,
            CSystemData_Cylinder systemCylinder = null, CSystemData_Vacuum systemVacuum = null,
            CSystemData_Align systemAlign = null, CSystemData_Scanner systemScanner = null,
            CSystemData_Light systemLight = null)
        {
            int iResult = SUCCESS;

            // save
            iResult = m_DataManager.SaveSystemData(system, systemAxis, systemCylinder, systemVacuum, systemAlign, systemScanner, systemLight);
            if (iResult != SUCCESS) return iResult;

            // set
            iResult = SetSystemDataToComponent();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        private int SetSystemDataToComponent(bool bLoadFromDB = true)
        {
            int iResult = SUCCESS;
            if (bLoadFromDB)
            {
                iResult = m_DataManager.LoadSystemData();
                if (iResult != SUCCESS) return iResult;
                iResult = m_DataManager.LoadModelList();
                if (iResult != SUCCESS) return iResult;
            }

            CSystemData systemData = m_DataManager.SystemData;

            MLWDicer.bInSfaTest = systemData.UseInSfaTest;
            MLWDicer.bUseOnline = systemData.UseOnLineUse;
            MLWDicer.Language   = systemData.Language;

            // set system data to each component

            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer

            // Loader
            {
                CMeElevatorData data;
                m_MeElevator.GetData(out data);
                data.ElevatorSafetyPos = systemData.MAxSafetyPos.Elevator_Pos;
                m_MeElevator.SetData(data);
            }

            // Handler
            {
                CMeHandlerData data;
                m_MeUpperHandler.GetData(out data);
                data.HandlerSafetyPos = systemData.MAxSafetyPos.UHandler_Pos;

                m_MeUpperHandler.SetData(data);
            }

            {
                CMeHandlerData data;
                m_MeLowerHandler.GetData(out data);
                data.HandlerSafetyPos = systemData.MAxSafetyPos.LHandler_Pos;

                m_MeLowerHandler.SetData(data);
            }

            // Spinner
            {
                CMeSpinnerData data;
                m_MeSpinner1.GetData(out data);
                data.CleanNozzleSafetyPos = systemData.MAxSafetyPos.S1_CleanNozzel_Pos;
                data.CoatNozzleSafetyPos = systemData.MAxSafetyPos.S1_CoatNozzel_Pos;
                m_MeSpinner1.SetData(data);
            }

            { 
                CMeSpinnerData data;
                m_MeSpinner2.GetData(out data);
                data.CleanNozzleSafetyPos = systemData.MAxSafetyPos.S2_CleanNozzel_Pos;
                data.CoatNozzleSafetyPos = systemData.MAxSafetyPos.S2_CoatNozzel_Pos;
                m_MeSpinner2.SetData(data);
            }

            // PushPull
            {
                CMePushPullData data;
                m_MePushPull.GetData(out data);

                data.PushPullSafetyPos = systemData.MAxSafetyPos.PushPull_Pos;
                data.CenterSafetyPos = systemData.MAxSafetyPos.Centering_Pos;
                m_MePushPull.SetData(data);
            }

            // Stage
            {
                CMeStageData data;
                m_MeStage.GetData(out data);

                data.StageSafetyPos = systemData.MAxSafetyPos.Stage_Pos;

                ////Screen Move Length (Camera의 FOV 대입한다)
               
                //data.MacroScreenWidth   = m_DataManager.SystemData_Align.Camera[(int)ECameraSelect.MACRO].CamFovX;
                //data.MacroScreenHeight  = m_DataManager.SystemData_Align.Camera[(int)ECameraSelect.MACRO].CamFovY;
                //data.MicroScreenWidth   = m_DataManager.SystemData_Align.Camera[(int)ECameraSelect.MICRO].CamFovX;
                //data.MicroScreenHeight  = m_DataManager.SystemData_Align.Camera[(int)ECameraSelect.MICRO].CamFovY;

                //// Rotate 값을 연산값으로 적용한다.
                //double dCamPosX = m_DataManager.SystemData_Align.Camera[(int)ECameraSelect.MACRO].Position.dX;
                //double dCamPosY = m_DataManager.SystemData_Align.Camera[(int)ECameraSelect.MACRO].Position.dY;
                //double dDistanceCam = Math.Sqrt(dCamPosX * dCamPosX + dCamPosY * dCamPosY);
                //double dAngle = Math.Atan(data.MacroScreenHeight / dDistanceCam / 2);

                //data.MacroScreenRotate = 2 * dAngle * 180 / Math.PI; // (To Degree)

                //dCamPosX = m_DataManager.SystemData_Align.Camera[(int)ECameraSelect.MICRO].Position.dX;
                //dCamPosY = m_DataManager.SystemData_Align.Camera[(int)ECameraSelect.MICRO].Position.dY;
                //dDistanceCam = Math.Sqrt(dCamPosX * dCamPosX + dCamPosY * dCamPosY);
                //dAngle = Math.Atan(data.MicroScreenHeight / dDistanceCam / 2);

                //data.MicroScreenRotate = 2 * dAngle * 180 / Math.PI; // (To Degree)
                

                // Jog Speed

                m_MeStage.SetData(data);
            }

            // OpPanel
            {
                COpPanelData data;
                m_OpPanel.GetData(out data);

                data.UseTankAlarm = ObjectExtensions.Copy(systemData.UseTankAlarm);
                data.UseDoorStatus = ObjectExtensions.Copy(systemData.UseDoorStatus);
                m_OpPanel.SetData(data);
            }

            //////////////////////////////////////////////////////////////////
            // Control Layer

            // Loader
            {
                CCtrlLoaderData data;
                m_ctrlLoader.GetData(out data);

                m_ctrlLoader.SetData(data);
            }

            // PushPull
            {
                CCtrlPushPullData data;
                m_ctrlPushPull.GetData(out data);

                m_ctrlPushPull.SetData(data);
            }

            // Spinner
            {
                CCtrlSpinnerData data;
                m_ctrlSpinner1.GetData(out data);

                m_ctrlSpinner1.SetData(data);
            }

            {
                CCtrlSpinnerData data;
                m_ctrlSpinner2.GetData(out data);
                 
                m_ctrlSpinner2.SetData(data);
            }

            // Handler
            {
                CCtrlHandlerData data;
                m_ctrlHandler.GetData(out data);

                m_ctrlHandler.SetData(data);
            }

            // Stage1
            {
                CCtrlStage1Data data;
                m_ctrlStage1.GetData(out data);

                // System Data에 있는 Vision Data를 적용한다.
                data.Vision = m_DataManager.SystemData_Align;                

                m_ctrlStage1.SetData(data);
            }

            // CtrlOpPanel
            {
                CCtrlOpPanelData data;
                m_ctrlOpPanel.GetData(out data);

                data.CheckSafety_AutoMode = systemData.CheckSafety_AutoMode;
                data.CheckSafety_ManualMode = systemData.CheckSafety_ManualMode;
                data.EnableCylinderMove_EStop = systemData.EnableCylinderMove_EStop;
                m_ctrlOpPanel.SetData(data);
            }

            //////////////////////////////////////////////////////////////////
            // Process Layer
            {
                CTrsAutoManagerData data;
                m_trsAutoManager.GetData(out data);
                data.UseVIPMode = systemData.UseVIPMode;
                m_trsAutoManager.SetData(data);
            }

            {
                CTrsLoaderData data;
                m_trsLoader.GetData(out data);
                data.ThreadHandshake_byOneStep = systemData.ThreadHandshake_byOneStep;
                m_trsLoader.SetData(data);
            }

            {
                CTrsPushPullData data;
                m_trsPushPull.GetData(out data);
                data.ThreadHandshake_byOneStep = systemData.ThreadHandshake_byOneStep;
                data.UseSpinnerSeparately      = systemData.UseSpinnerSeparately;
                data.UCoaterIndex              = systemData.UCoaterIndex;
                data.UCleanerIndex             = systemData.UCleanerIndex;
                m_trsPushPull.SetData(data);
            }

            {
                CTrsSpinnerData data;
                m_trsSpinner1.GetData(out data);
                data.ThreadHandshake_byOneStep = systemData.ThreadHandshake_byOneStep;
                m_trsSpinner1.SetData(data);
            }

            {
                CTrsSpinnerData data;
                m_trsSpinner2.GetData(out data);
                data.ThreadHandshake_byOneStep = systemData.ThreadHandshake_byOneStep;
                m_trsSpinner2.SetData(data);
            }

            {
                CTrsHandlerData data;
                m_trsHandler.GetData(out data);
                data.ThreadHandshake_byOneStep = systemData.ThreadHandshake_byOneStep;
                m_trsHandler.SetData(data);
            }

            {
                CTrsStage1Data data;
                m_trsStage1.GetData(out data);
                data.ThreadHandshake_byOneStep = systemData.ThreadHandshake_byOneStep;
                m_trsStage1.SetData(data);
            }

            return SUCCESS;   
        }

        public int SaveModelData(CModelData modelData)
        {
            // save
            int iResult = m_DataManager.SaveModelData(modelData);
            if (iResult != SUCCESS) return iResult;

            // set
            iResult = SetModelDataToComponent();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SaveModelData(CWaferFrameData data)
        {
            // save
            int iResult = m_DataManager.SaveModelData(data);
            if (iResult != SUCCESS) return iResult;

            // set
            iResult = SetModelDataToComponent();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SaveUserData(CUserInfo data)
        {
            // save
            int iResult = m_DataManager.SaveUserData(data);
            if (iResult != SUCCESS) return iResult;

            // set
            //iResult = SetModelDataToComponent();
            //if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SetModelDataToComponent()
        {
            int iResult = SUCCESS;
            //m_DataManager.ChangeModel(m_DataManager.SystemData.ModelName);

            CModelData modelData = m_DataManager.ModelData;

            // set model data to each component


            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer

            // Loader
            {
                CMeElevatorData data;
                m_MeElevator.GetData(out data);

                m_MeElevator.SetData(data);
            }

            // PushPull
            {
                CMePushPullData data;
                m_MePushPull.GetData(out data);

                m_MePushPull.SetData(data);
            }

            // Spinner
            {
                CMeSpinnerData data;
                m_MeSpinner1.GetData(out data);
                Array.Copy(modelData.MeSpinner1_UseVccFlag, data.UseVccFlag, modelData.MeSpinner1_UseVccFlag.Length);

                m_MeSpinner1.SetData(data);
            }

            {
                CMeSpinnerData data;
                m_MeSpinner2.GetData(out data);
                Array.Copy(modelData.MeSpinner2_UseVccFlag, data.UseVccFlag, modelData.MeSpinner2_UseVccFlag.Length);

                m_MeSpinner2.SetData(data);
            }


            // Handler
            {
                CMeHandlerData data;
                m_MeUpperHandler.GetData(out data);
                Array.Copy(modelData.MeUH_UseVccFlag, data.UseVccFlag, modelData.MeUH_UseVccFlag.Length);
                Array.Copy(modelData.MeUH_UseMainCylFlag, data.UseMainCylFlag, modelData.MeUH_UseMainCylFlag.Length);
                Array.Copy(modelData.MeUH_UseSubCylFlag, data.UseSubCylFlag, modelData.MeUH_UseSubCylFlag.Length);
                Array.Copy(modelData.MeUH_UseGuideCylFlag, data.UseGuideCylFlag, modelData.MeUH_UseGuideCylFlag.Length);

                m_MeUpperHandler.SetData(data);
            }

            {
                CMeHandlerData data;
                m_MeLowerHandler.GetData(out data);
                Array.Copy(modelData.MeLH_UseVccFlag, data.UseVccFlag, modelData.MeLH_UseVccFlag.Length);
                Array.Copy(modelData.MeLH_UseMainCylFlag, data.UseMainCylFlag, modelData.MeLH_UseMainCylFlag.Length);
                Array.Copy(modelData.MeLH_UseSubCylFlag, data.UseSubCylFlag, modelData.MeLH_UseSubCylFlag.Length);
                Array.Copy(modelData.MeLH_UseGuideCylFlag, data.UseGuideCylFlag, modelData.MeLH_UseGuideCylFlag.Length);

                m_MeLowerHandler.SetData(data);
            }


            // Stage 1
            {
                CMeStageData data;
                m_MeStage.GetData(out data);
                Array.Copy(modelData.MeStage_UseVccFlag, data.UseVccFlag, modelData.MeStage_UseVccFlag.Length);

                //data.StageSafetyPos = m_DataManager.SystemData.MAxSafetyPos.Stage_Pos;

                ////Intex Move Length  // LJJ 수정
                //data.IndexWidth = modelData.StageIndexWidth;
                //data.IndexHeight = modelData.StageIndexHeight;
                //data.IndexRotate = modelData.StageIndexRotate;

                //// Align Mark A,B의 거리를 적용한다.
                //// 비율을 설정하면 Wafer의 사이즈에 따라서.. 거리를 계산한다.
                //data.AlignMarkWidthRatio = modelData.AlignMarkWidthRatio;
                //data.AlignMarkWidthLen = modelData.Wafer.Size_X * data.AlignMarkWidthRatio;


                m_MeStage.SetData(data);
            }


            // Jog Speed



            //////////////////////////////////////////////////////////////////
            // Control Layer

            // Loader
            {
                CCtrlLoaderData data;
                m_ctrlLoader.GetData(out data);

                m_ctrlLoader.SetData(data);
            }

            // PushPull
            {
                CCtrlPushPullData data;
                m_ctrlPushPull.GetData(out data);

                m_ctrlPushPull.SetData(data);
            }

            // Spinner
            {
                CCtrlSpinnerData data;
                m_ctrlSpinner1.GetData(out data);

                data.CoaterData = ObjectExtensions.Copy(modelData.SpinnerData[(int)ESpinnerIndex.SPINNER1].CoaterData);
                data.CleanerData = ObjectExtensions.Copy(modelData.SpinnerData[(int)ESpinnerIndex.SPINNER1].CleanerData);

                m_ctrlSpinner1.SetData(data);
            }

            {
                CCtrlSpinnerData data;
                m_ctrlSpinner2.GetData(out data);

                data.CoaterData = ObjectExtensions.Copy(modelData.SpinnerData[(int)ESpinnerIndex.SPINNER2].CoaterData);
                data.CleanerData = ObjectExtensions.Copy(modelData.SpinnerData[(int)ESpinnerIndex.SPINNER2].CleanerData);

                m_ctrlSpinner2.SetData(data);
            }

            // Handler
            {
                CCtrlHandlerData data;
                m_ctrlHandler.GetData(out data);

                m_ctrlHandler.SetData(data);
            }

            // Stage1
            {
                CCtrlStage1Data data;
                m_ctrlStage1.GetData(out data);

                // Model Data에 있는 Vision Data를 적용한다.
                data.Align = ObjectExtensions.Copy(modelData.AlignData);
                
                // Laser Process Data Copy
                data.MarkingData = ObjectExtensions.Copy(modelData.LaserProcessData);

                m_ctrlStage1.SetData(data);
            }

            //////////////////////////////////////////////////////////////////
            // Process Layer
            // Loader
            {
                CTrsLoaderData data;
                m_trsLoader.GetData(out data);

                m_trsLoader.SetData(data);
            }

            // PushPull
            {
                CTrsPushPullData data;
                m_trsPushPull.GetData(out data);

                m_trsPushPull.SetData(data);
            }

            // Spinner
            {
                CTrsSpinnerData data;
                m_trsSpinner1.GetData(out data);

                m_trsSpinner1.SetData(data);
            }

            {
                CTrsSpinnerData data;
                m_trsSpinner2.GetData(out data);

                m_trsSpinner2.SetData(data);
            }

            // Handler
            {
                CTrsHandlerData data;
                m_trsHandler.GetData(out data);

                m_trsHandler.SetData(data);
            }

            // Stage1
            {
                CTrsStage1Data data;
                m_trsStage1.GetData(out data);

                m_trsStage1.SetData(data);
            }

            return SUCCESS;
        }

        public int GetPositionGroup(out CPositionGroup tGroup, bool bType_Fixed)
        {
            if (bType_Fixed == true) tGroup = ObjectExtensions.Copy(m_DataManager.Pos_Fixed);
            else tGroup = ObjectExtensions.Copy(m_DataManager.Pos_Offset);

            return SUCCESS;
        }

        public int SavePosition(CPositionGroup tGroup, bool bType_Fixed, EPositionObject unit)
        {
            // save
            int iResult = m_DataManager.SavePositionData(tGroup, bType_Fixed, unit);
            if (iResult != SUCCESS) return iResult;

            // set to component
            iResult = SetPositionDataToComponent(unit);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        private int SetPositionDataToComponent(EPositionObject unit = EPositionObject.ALL)
        {
            int iResult = SUCCESS;
            CPositionGroup Pos_Fixed = m_DataManager.Pos_Fixed;
            CPositionGroup Pos_Model = m_DataManager.Pos_Model;
            CPositionGroup Pos_Offset = m_DataManager.Pos_Offset;

            // set position data to each component
            int index;
            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer

            // Loader
            if (unit == EPositionObject.ALL || unit == EPositionObject.LOADER)
            {
                index = (int)EPositionObject.LOADER;
                iResult = m_MeElevator.SetPosition_Elevator(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }

            // PushPull
            if (unit == EPositionObject.ALL || unit == EPositionObject.PUSHPULL)
            {
                index = (int)EPositionObject.PUSHPULL;
                iResult = m_MePushPull.SetPosition_PushPull(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }
            if (unit == EPositionObject.ALL || unit == EPositionObject.PUSHPULL_CENTER1)
            {
                index = (int)EPositionObject.PUSHPULL_CENTER1;
                iResult = m_MePushPull.SetPosition_Centering(DEF_MePushPull.ECenterIndex.LEFT, Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }
            if (unit == EPositionObject.ALL || unit == EPositionObject.PUSHPULL_CENTER2)
            {
                index = (int)EPositionObject.PUSHPULL_CENTER2;
                iResult = m_MePushPull.SetPosition_Centering(DEF_MePushPull.ECenterIndex.RIGHT, Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner
            if (unit == EPositionObject.ALL || unit == EPositionObject.S1_ROTATE)
            {
                index = (int)EPositionObject.S1_ROTATE;
                iResult = m_MeSpinner1.SetPosition_Rotate(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }
            if (unit == EPositionObject.ALL || unit == EPositionObject.S1_CLEAN_NOZZLE)
            {
                index = (int)EPositionObject.S1_CLEAN_NOZZLE;
                iResult = m_MeSpinner1.SetPosition_CleanNozzle(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }
            if (unit == EPositionObject.ALL || unit == EPositionObject.S1_COAT_NOZZLE)
            {
                index = (int)EPositionObject.S1_COAT_NOZZLE;
                iResult = m_MeSpinner1.SetPosition_CoatNozzle(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.S2_ROTATE)
            {
                index = (int)EPositionObject.S2_ROTATE;
                iResult = m_MeSpinner2.SetPosition_Rotate(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }
            if (unit == EPositionObject.ALL || unit == EPositionObject.S2_CLEAN_NOZZLE)
            {
                index = (int)EPositionObject.S2_CLEAN_NOZZLE;
                iResult = m_MeSpinner2.SetPosition_CleanNozzle(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }
            if (unit == EPositionObject.ALL || unit == EPositionObject.S2_COAT_NOZZLE)
            {
                index = (int)EPositionObject.S2_COAT_NOZZLE;
                iResult = m_MeSpinner2.SetPosition_CoatNozzle(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }

            // Handler
            if (unit == EPositionObject.ALL || unit == EPositionObject.UPPER_HANDLER)
            {
                index = (int)EPositionObject.UPPER_HANDLER;
                iResult = m_MeUpperHandler.SetPosition_Handler(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }
            if (unit == EPositionObject.ALL || unit == EPositionObject.LOWER_HANDLER)
            {
                index = (int)EPositionObject.LOWER_HANDLER;
                iResult = m_MeLowerHandler.SetPosition_Handler(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }

            // Stage
            if (unit == EPositionObject.ALL || unit == EPositionObject.STAGE1)
            {
                index = (int)EPositionObject.STAGE1;
                iResult = m_MeStage.SetPosition_Stage(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }
            if (unit == EPositionObject.ALL || unit == EPositionObject.CAMERA1)
            {
                index = (int)EPositionObject.CAMERA1;
                iResult = m_MeStage.SetPosition_Camera(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }
            if (unit == EPositionObject.ALL || unit == EPositionObject.SCANNER1)
            {
                index = (int)EPositionObject.SCANNER1;
                iResult = m_MeStage.SetPosition_Scanner(Pos_Fixed.Pos_Array[index], Pos_Model.Pos_Array[index], Pos_Offset.Pos_Array[index]);
                if (iResult != SUCCESS) return iResult;
            }

            //////////////////////////////////////////////////////////////////
            // Control Layer

            //////////////////////////////////////////////////////////////////
            // Process Layer

            return SUCCESS;
        }

        private int SetAllPositionDataToComponent()
        {
            int iResult = SUCCESS;
            iResult = m_DataManager.LoadPositionData(true);
            if (iResult != SUCCESS) return iResult;
            iResult = m_DataManager.LoadPositionData(false);
            if (iResult != SUCCESS) return iResult;
            iResult = m_DataManager.GenerateModelPosition();
            if (iResult != SUCCESS) return iResult;

            iResult = SetPositionDataToComponent();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        void CreateMeElevator(CObjectInfo objInfo)
        {
            CMeElevatorRefComp refComp = new CMeElevatorRefComp();
            CMeElevatorData data = new CMeElevatorData();

            refComp.IO = m_IO;
            refComp.AxElevator = m_AxLoader;            

            // IO 설정은 임시로 진행함.
            data.InWaferDetected = iUHandler_PanelDetect;
            data.InPushPullDetected = 1230;
            data.InCassetteDetected[(int)ECassetteDetectedSensor.SENSOR1] = iInterface_00;
            data.InCassetteDetected[(int)ECassetteDetectedSensor.SENSOR2] = iInterface_00;
            data.InCassetteDetected[(int)ECassetteDetectedSensor.SENSOR3] = iInterface_00;
            data.InCassetteDetected[(int)ECassetteDetectedSensor.SENSOR4] = iInterface_00;

            data.ElevatorZone.UseSafetyMove[DEF_Z] = true;
            data.ElevatorZone.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = 1234; // need updete io address

            m_MeElevator = new MMeElevator(objInfo, refComp, data);
        }

        void CreateMeUHandler(CObjectInfo objInfo)
        {
            CMeHandlerRefComp refComp = new CMeHandlerRefComp();
            CMeHandlerData data = new CMeHandlerData();

            refComp.IO = m_IO;
            refComp.AxHandler = m_AxUpperHandler;
            refComp.Vacuum[(int)EHandlerVacuum.SELF] = m_UHandlerSelfVac;

            data.HandlerType[DEF_X] = EHandlerType.AXIS;
            data.HandlerType[DEF_Z] = EHandlerType.AXIS;

            data.InDetectObject = iUHandler_PanelDetect;

            data.HandlerZoneCheck.UseSafetyMove[DEF_X] = false;
            //data.HandlerZoneCheck.Axis[DEF_X].ZoneAddr[(int)EHandlerXAxZone.WAIT] = iUHandler_XPos_Wait;
            //data.HandlerZoneCheck.Axis[DEF_X].ZoneAddr[(int)EHandlerXAxZone.PUSHPULL] = iUHandler_XPos_PushPull;
            //data.HandlerZoneCheck.Axis[DEF_X].ZoneAddr[(int)EHandlerXAxZone.STAGE] = iUHandler_XPos_Stage;
            data.HandlerZoneCheck.UseSafetyMove[DEF_Z] = true;
            //data.HandlerZoneCheck.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = iUHandler_ZPos_Safety;


            m_MeUpperHandler = new MMeHandler(objInfo, refComp, data);
        }

        void CreateMeLHandler(CObjectInfo objInfo)
        {
            CMeHandlerRefComp refComp = new CMeHandlerRefComp();
            CMeHandlerData data = new CMeHandlerData();

            refComp.IO = m_IO;
            refComp.AxHandler = m_AxLowerHandler;
            refComp.Vacuum[(int)EHandlerVacuum.SELF] = m_LHandlerSelfVac;

            data.HandlerType[DEF_X] = EHandlerType.AXIS;
            data.HandlerType[DEF_Z] = EHandlerType.AXIS;

            data.InDetectObject = iUHandler_PanelDetect;

            data.HandlerZoneCheck.UseSafetyMove[DEF_X] = false;
            //data.HandlerZoneCheck.Axis[DEF_X].ZoneAddr[(int)EHandlerXAxZone.WAIT] = iLHandler_XPos_Wait;
            //data.HandlerZoneCheck.Axis[DEF_X].ZoneAddr[(int)EHandlerXAxZone.PUSHPULL] = iLHandler_XPos_PushPull;
            //data.HandlerZoneCheck.Axis[DEF_X].ZoneAddr[(int)EHandlerXAxZone.STAGE] = iLHandler_XPos_Stage;
            data.HandlerZoneCheck.UseSafetyMove[DEF_Z] = true;
            //data.HandlerZoneCheck.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = iLHandler_ZPos_Safety;


            m_MeLowerHandler = new MMeHandler(objInfo, refComp, data);
        }

        void CreateMePushPull(CObjectInfo objInfo)
        {
            CMePushPullRefComp refComp = new CMePushPullRefComp();
            CMePushPullData data = new CMePushPullData();

            refComp.IO = m_IO;
            refComp.Gripper = m_PushPullGripperCyl;
            refComp.UDCyl = m_PushPullUDCyl;
            refComp.AxPushPull = m_AxPushPull;
            refComp.AxCenter[(int)ECenterIndex.LEFT] = m_AxCentering1;
            refComp.AxCenter[(int)ECenterIndex.RIGHT] = m_AxCentering2;
            

            data.PushPullType[DEF_Y] = EPushPullType.AXIS;

            data.InDetectObject_Front = iUHandler_PanelDetect;
            data.InDetectObject_Rear = iUHandler_PanelDetect;

            data.CenterZone[(int)ECenterIndex.LEFT].Axis[DEF_X].ZoneAddr[(int)ECenterXAxZone.SAFETY] = 1234; // need updete io address
            data.CenterZone[(int)ECenterIndex.RIGHT].Axis[DEF_X].ZoneAddr[(int)ECenterXAxZone.SAFETY] = 1234; // need updete io address

            m_MePushPull = new MMePushPull(objInfo, refComp, data);
        }

        void CreateMeOpPanel(CObjectInfo objInfo)
        {
            COpPanelRefComp refComp = new COpPanelRefComp();
            COpPanelData data = new COpPanelData();

            refComp.IO = m_IO;
            refComp.Yaskawa_Motion = m_YMC;
            refComp.ACS_Motion = m_ACS;

            COpPanelIOAddr sPanelIOAddr = new COpPanelIOAddr();
            // Front
            int index = (int)EOPPanel.FRONT;
            sPanelIOAddr.OpPanel.RunInputAddr[index]     = iStart_SWFront;
            sPanelIOAddr.OpPanel.StopInputAddr[index]    = iStop_SWFront;
            sPanelIOAddr.OpPanel.ResetInputAddr[index]   = iReset_SWFront;
            sPanelIOAddr.OpPanel.EStopInputAddr[index]   = iEMO_SW;
            sPanelIOAddr.OpPanel.RunOutputAddr[index]    = oStart_LampFront;
            sPanelIOAddr.OpPanel.StopOutputAddr[index]   = oStop_LampFront;
            sPanelIOAddr.OpPanel.ResetOutputAddr[index]  = oReset_LampFront;
            sPanelIOAddr.OpPanel.XpInputAddr[index]      = iJog_X_Forward_SWFront;
            sPanelIOAddr.OpPanel.XnInputAddr[index]      = iJog_X_Backward_SWFront;
            sPanelIOAddr.OpPanel.YpInputAddr[index]      = iJog_Y_Forward_SWFront;
            sPanelIOAddr.OpPanel.YnInputAddr[index]      = iJog_Y_Backward_SWFront;
            sPanelIOAddr.OpPanel.TpInputAddr[index]      = iJog_T_CW_SWFront;
            sPanelIOAddr.OpPanel.TnInputAddr[index]      = iJog_T_CCW_SWFront;
            sPanelIOAddr.OpPanel.ZpInputAddr[index]      = iJog_Z_UP_SWFront;
            sPanelIOAddr.OpPanel.ZnInputAddr[index]      = iJog_Z_DOWN_SWFront;

            // Rear
            index = (int)EOPPanel.BACK;
            sPanelIOAddr.OpPanel.RunInputAddr[index]     = iStart_SWRear;
            sPanelIOAddr.OpPanel.StopInputAddr[index]    = iStop_SWRear;
            sPanelIOAddr.OpPanel.ResetInputAddr[index]   = iReset_SWRear;
            sPanelIOAddr.OpPanel.EStopInputAddr[index]   = iEMO_SWRear;
            sPanelIOAddr.OpPanel.RunOutputAddr[index]    = oStart_LampRear;
            sPanelIOAddr.OpPanel.StopOutputAddr[index]   = oStop_LampRear;
            sPanelIOAddr.OpPanel.ResetOutputAddr[index]  = oReset_LampRear;
            sPanelIOAddr.OpPanel.XpInputAddr[index]      = iJog_X_Forward_SWRear;
            sPanelIOAddr.OpPanel.XnInputAddr[index]      = iJog_X_Backward_SWRear;
            sPanelIOAddr.OpPanel.YpInputAddr[index]      = iJog_Y_Forward_SWRear;
            sPanelIOAddr.OpPanel.YnInputAddr[index]      = iJog_Y_Backward_SWRear;
            sPanelIOAddr.OpPanel.TpInputAddr[index]      = iJog_T_CW_SWRear;
            sPanelIOAddr.OpPanel.TnInputAddr[index]      = iJog_T_CCW_SWRear;
            sPanelIOAddr.OpPanel.ZpInputAddr[index]      = iJog_Z_UP_SWRear;
            sPanelIOAddr.OpPanel.ZnInputAddr[index]      = iJog_Z_DOWN_SWRear;

            // TeachPendant

            sPanelIOAddr.MainAirAddr = iMain_Air_On;
            //sPanelIOAddr.MainN2Addr = iMain_N
            sPanelIOAddr.MainVacuumAddr = iMain_Vac1_On;

            sPanelIOAddr.TowerLamp = new CTowerIOAddr(oTower_LampRed, oTower_LampYellow, oTower_LampGreen,
                oBuzzer_1, oBuzzer_2, oBuzzer_3, oBuzzer_4);

            sPanelIOAddr.TankEmptyAddr[(int)ECoatTank.TANK_1] = iSHead1_Tank1_Empty;
            sPanelIOAddr.TankEmptyAddr[(int)ECoatTank.TANK_2] = iSHead1_Tank2_Empty;

            sPanelIOAddr.SafeDoorAddr[(int)EDoorGroup.FRONT, (int)EDoorIndex.INDEX_1] = iDoor_Front;
            sPanelIOAddr.SafeDoorAddr[(int)EDoorGroup.FRONT, (int)EDoorIndex.INDEX_2] = iDoor_Front2;
            sPanelIOAddr.SafeDoorAddr[(int)EDoorGroup.BACK, (int)EDoorIndex.INDEX_1]  = iDoor_Back;
            sPanelIOAddr.SafeDoorAddr[(int)EDoorGroup.BACK, (int)EDoorIndex.INDEX_2]  = iDoor_Back2;
            sPanelIOAddr.SafeDoorAddr[(int)EDoorGroup.LEFT, (int)EDoorIndex.INDEX_1]  = iDoor_Side;
            sPanelIOAddr.SafeDoorAddr[(int)EDoorGroup.LEFT, (int)EDoorIndex.INDEX_2]  = iDoor_Side2;

            // SetSystemData 에서 관리
            //data.UseDoorStatus[(int)EDoorGroup.FRONT, (int)EDoorIndex.INDEX1] = true;
            //data.UseDoorStatus[(int)EDoorGroup.FRONT, (int)EDoorIndex.INDEX2] = true;
            //data.UseDoorStatus[(int)EDoorGroup.REAR, (int)EDoorIndex.INDEX1]  = true;
            //data.UseDoorStatus[(int)EDoorGroup.REAR, (int)EDoorIndex.INDEX2]  = true;
            //data.UseDoorStatus[(int)EDoorGroup.LEFT, (int)EDoorIndex.INDEX1]  = true;
            //data.UseDoorStatus[(int)EDoorGroup.LEFT, (int)EDoorIndex.INDEX2] = true;

            CJogTable sJogTable = new CJogTable();

            m_OpPanel = new MOpPanel(objInfo, refComp, data, sPanelIOAddr, sJogTable);
        }

        void CreateMeStage(CObjectInfo objInfo)
        {
            CMeStageRefComp refComp = new CMeStageRefComp();
            CMeStageData data = new CMeStageData();

            refComp.IO = m_IO;
            refComp.AxStage = m_AxStage1;
            refComp.AxCamera = m_AxCamera1;
            refComp.AxScanner = m_AxScannerZ1;

            refComp.Vacuum[(int)EStageVacuum.SELF] = m_Stage1Vac;
            
            data.InDetectObject = iUHandler_PanelDetect;

            data.StageZone.UseSafetyMove[DEF_Z] = true;
            data.StageZone.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = 1234; // need updete io address

            m_MeStage = new MMeStage(objInfo, refComp, data);
        }

        void CreateMeSpinner1(CObjectInfo objInfo)
        {
            CMeSpinnerRefComp refComp = new CMeSpinnerRefComp();
            CMeSpinnerData data = new CMeSpinnerData();

            refComp.IO = m_IO;

            refComp.AxSpinRotate      = m_AxRotate1;
            refComp.AxSpinCleanNozzle = m_AxCleanNozzle1;
            refComp.AxSpinCoatNozzle  = m_AxCoatNozzle1;

            refComp.Vacuum[(int)EChuckVacuum.SELF] = m_Spinner1Vac;

            refComp.ChuckTableUDCyl = m_Spinner1UDCyl;
            refComp.CleanNozzleSolCyl = m_Spinner1DICyl;
            refComp.CoatNozzleSolCyl  = m_Spinner1PVACyl;

            data.InDetectObject = iStage2_PanelDetect;
            data.OutRingBlow = oSpinner1_Ring_Blow;
            data.InRotateLoadPos = 1234; // need updete io address

            data.CleanNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY] = 1234; // need updete io address
            data.CoatNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY] = 1234; // need updete io address

            m_MeSpinner1 = new MMeSpinner(objInfo, refComp, data);
        }

        void CreateMeSpinner2(CObjectInfo objInfo)
        {
            CMeSpinnerRefComp refComp = new CMeSpinnerRefComp();
            CMeSpinnerData data = new CMeSpinnerData();

            refComp.IO = m_IO;

            refComp.AxSpinRotate = m_AxRotate2;
            refComp.AxSpinCleanNozzle = m_AxCleanNozzle2;
            refComp.AxSpinCoatNozzle = m_AxCoatNozzle2;

            refComp.Vacuum[(int)EChuckVacuum.SELF] = m_Spinner2Vac;

            refComp.ChuckTableUDCyl = m_Spinner2UDCyl;
            refComp.CleanNozzleSolCyl = m_Spinner2DICyl;
            refComp.CoatNozzleSolCyl = m_Spinner2PVACyl;

            data.InDetectObject = iStage3_PanelDetect;
            data.OutRingBlow = oSpinner2_Ring_Blow;
            data.InRotateLoadPos = 1234; // need updete io address

            data.CleanNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY] = 1234; // need updete io address
            data.CoatNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY] = 1234; // need updete io address

            m_MeSpinner2 = new MMeSpinner(objInfo, refComp, data);
        }

        public bool IsSafeForAxisMove(bool bDisplayMsg = true)
        {
            int iResult = m_ctrlOpPanel.CheckSafetyBeforeAxisMove();
            if (iResult != SUCCESS)
            {
                if(bDisplayMsg == true) CMainFrame.DisplayAlarm(iResult);
                return false;
            }
            return true;
        }

        public bool IsSafeForCylinderMove(bool bDisplayMsg = true)
        {
            int iResult = m_ctrlOpPanel.CheckSafetyBeforeCylinderMove();
            if (iResult != SUCCESS)
            {
                if (bDisplayMsg == true) CMainFrame.DisplayAlarm(iResult);
                return false;
            }
            return true;
        }

        public bool CheckAxisSensorLimit(int servoIndex)
        {
            // 2010.10.26 by ranian
            // 간단하게나마 원점 복귀시에 +,-, Home 센서가 둘다 들어와있는지 체크해서 sensorlevel 검증
            bool bSts_Negative, bSts_Positive, bSts_Home;
            m_OpPanel.GetAxisSensorStatus(servoIndex, DEF_HOME_SENSOR, out bSts_Home);
            m_OpPanel.GetAxisSensorStatus(servoIndex, DEF_POSITIVE_SENSOR, out bSts_Positive);
            m_OpPanel.GetAxisSensorStatus(servoIndex, DEF_NEGATIVE_SENSOR, out bSts_Negative);

            int nSum = 0;
            if (bSts_Home == true) nSum++;
            if (bSts_Positive == true) nSum++;
            if (bSts_Negative == true) nSum++;

            if (nSum >= 2) return false;

            return true;
        }

        public bool CheckSystemConfig_ForRun(out string strErr)
        {
            strErr = "";
            return true;
        }

        public void SetAutoManual(EAutoManual mode)
        {
            // mechanical layer
            m_OpPanel.SetAutoManual(mode);
            m_MeElevator.SetAutoManual(mode);
            m_MePushPull.SetAutoManual(mode);
            m_MeSpinner1.SetAutoManual(mode);
            m_MeSpinner2.SetAutoManual(mode);
            m_MeLowerHandler.SetAutoManual(mode);
            m_MeUpperHandler.SetAutoManual(mode);
            m_MeStage.SetAutoManual(mode);

            // control layer
            m_ctrlOpPanel.SetAutoManual(mode);
            m_ctrlLoader.SetAutoManual(mode);
            m_ctrlPushPull.SetAutoManual(mode);
            m_ctrlSpinner1.SetAutoManual(mode);
            m_ctrlSpinner2.SetAutoManual(mode);
            m_ctrlHandler.SetAutoManual(mode);
            m_ctrlStage1.SetAutoManual(mode);

            // process layer
            m_trsAutoManager.SetAutoManual(mode);
            m_trsLoader.SetAutoManual(mode);
            m_trsPushPull.SetAutoManual(mode);
            m_trsSpinner1.SetAutoManual(mode);
            m_trsSpinner2.SetAutoManual(mode);
            m_trsHandler.SetAutoManual(mode);
            m_trsStage1.SetAutoManual(mode);
        }

        public int EmptyMethod()
        {
            return SUCCESS;
        }
    }
}
