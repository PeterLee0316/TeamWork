using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO.Ports;

using System.Windows.Forms;

using LWDicer.UI;
using MotionYMC;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_IO;

using static LWDicer.Control.DEF_OpPanel;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_DataManager;
using static LWDicer.Control.DEF_LCNet;

using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_ACS;
using static LWDicer.Control.DEF_MultiAxesYMC;
using static LWDicer.Control.DEF_Cylinder;
using static LWDicer.Control.DEF_Vacuum;
using static LWDicer.Control.DEF_Vision;

using static LWDicer.Control.DEF_OpPanel;
using static LWDicer.Control.DEF_MeElevator;
using static LWDicer.Control.DEF_MeHandler;
using static LWDicer.Control.DEF_MeStage;
using static LWDicer.Control.DEF_MePushPull;
using static LWDicer.Control.DEF_SerialPort;
using static LWDicer.Control.DEF_PolygonScanner;
using static LWDicer.Control.DEF_MeSpinner;

using static LWDicer.Control.DEF_CtrlOpPanel;
using static LWDicer.Control.DEF_CtrlHandler;
using static LWDicer.Control.DEF_CtrlSpinner;
using static LWDicer.Control.DEF_CtrlPushPull;
using static LWDicer.Control.DEF_CtrlLoader;
using static LWDicer.Control.DEF_CtrlStage;

using static LWDicer.Control.DEF_TrsAutoManager;

using static LWDicer.Control.DEF_Thread.EThreadChannel;

namespace LWDicer.Control
{
    public class MLWDicer : MObject, IDisposable
    {
        // static common data
        public static bool bUseOnline { get; private set; }
        public static bool bInSfaTest { get; private set; }

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
        public MMultiAxes_ACS m_AxStage1;
        public MMultiAxes_YMC m_AxLoader;
        public MMultiAxes_YMC m_AxPushPull;
        public MMultiAxes_YMC m_AxCentering1;
        public MMultiAxes_YMC m_AxRotate1;
        public MMultiAxes_YMC m_AxCleanNozzle1;
        public MMultiAxes_YMC m_AxCoatNozzle1;
        public MMultiAxes_YMC m_AxCentering2;
        public MMultiAxes_YMC m_AxRotate2;
        public MMultiAxes_YMC m_AxCleanNozzle2;
        public MMultiAxes_YMC m_AxCoatNozzle2;
        public MMultiAxes_YMC m_AxUpperHandler;
        public MMultiAxes_YMC m_AxLowerHandler;
        public MMultiAxes_YMC m_AxCamera1;
        public MMultiAxes_YMC m_AxLaser1;

        // IO
        public IIO m_IO { get; private set; }

        ///////////////////////////////////////////////////////////////////////
        // Mechanical Layer
        // Cylinder
        public ICylinder m_PushPullGripperCyl;
        public ICylinder m_PushPullUDCyl;

        public ICylinder m_Spinner1UDCyl;     // Wafer Coater Up Down Cylinder [Double]
        public ICylinder m_Spinner1DICyl;     // Wafer Coater DI Nozzle On Off [Single]
        public ICylinder m_Spinner1PVACyl;    // Wafer Coater PVA Nozzle On Off [Single]

        public ICylinder m_Spinner2UDCyl;     // Wafer Coater Up Down Cylinder [Double]
        public ICylinder m_Spinner2DICyl;    // Wafer Cleaner DI Nozzle On Off [Single]
        public ICylinder m_Spinner2PVACyl;    // Wafer Cleaner N2 Nozzle On Off [Single]

        public ICylinder m_StageClamp1;  // Work Stage Clamp 1 Cylinder [Double]
        public ICylinder m_StageClamp2;  // Work Stage Clamp 2 Cylinder [Double]

        // Vacuum
        public IVacuum m_Stage1Vac;

        public IVacuum m_UHandlerSelfVac;
        public IVacuum m_UHandlerFactoryVac;
        public IVacuum m_LHandlerSelfVac;
        public IVacuum m_LHandlerFactoryVac;

        public IVacuum m_Spinner1Vac;
        public IVacuum m_Spinner2Vac;

        // Serial
        public ISerialPort m_PolygonComPort;

        // Scanner
        public IPolygonScanner[] m_Scanner = new IPolygonScanner[(int)EObjectScanner.MAX];

        public MVisionSystem m_VisionSystem;
        public MVisionCamera[] m_VisionCamera;
        public MVisionView[] m_VisionView;

        // Vision
        public MVision m_Vision { get; set; }

        ///////////////////////////////////////////////////////////////////////
        // Mechanical Layer

        public MMeElevator m_MeElevator;            // Cassette Loader 용 Elevator
        public MMeHandler m_MeUpperHandler;         // UpperHandler of 2Layer
        public MMeHandler m_MeLowerHandler;         // LowerHandler of 2Layer
        public MMeStage m_MeStage;         

        public MMePushPull m_MePushPull;

        public MMeSpinner m_MeSpinner1;
        public MMeSpinner m_MeSpinner2;

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

        public CLoginData GetLogin()
        {
            return m_DataManager?.GetLogin();
        }

        public void SetLogin(CLoginData login)
        {
            m_DataManager?.SetLogin(login);
        }

        public void TestFunction_BeforeInit()
        {
        }

        public void TestFunction_AfterInit()
        {
            // test io
            if(false)
            {
                bool bStatus;
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

        public CAlarm GetAlarmInfo(int pid, int alarmcode, bool saveLog = true)
        {
            CAlarm alarm = new CAlarm();
            alarm.ProcessID = pid;
            alarm.ObjectID = (int)((alarmcode & 0xffff0000) >> 16);
            alarm.ErrorBase = (int)((alarmcode & 0x0000ffff) / 100) * 100;
            alarm.ErrorCode = (int)((alarmcode & 0x0000ffff) % 100);

            alarm.ProcessName = m_SystemInfo.GetObjectName(alarm.ProcessID);
            alarm.TypeName = m_SystemInfo.GetTypeName(alarm.ObjectID);
            alarm.ObjectName = m_SystemInfo.GetObjectName(alarm.ObjectID);
            m_DataManager.LoadAlarmInfo(alarm.GetIndex(), out alarm.Info);

            if(saveLog == true)
            {
                m_DataManager.SaveAlarmHistory(alarm);
            }
            return alarm;
        }

        public string GetAlarmText(int alarmcode, ELanguage type = ELanguage.NONE)
        {
            CAlarm alarm = GetAlarmInfo(0, alarmcode, false);
            return alarm.Info.Description[(int)type];
        }

        public string GetAlarmSolution(int alarmcode, ELanguage type = ELanguage.NONE)
        {
            CAlarm alarm = GetAlarmInfo(0, alarmcode, false);
            return alarm.Info.Solution[(int)type];
        }

        public int Initialize(CMainFrame form1 = null)
        {
            TestFunction_BeforeInit();

            ////////////////////////////////////////////////////////////////////////
            // 0. Common Class
            ////////////////////////////////////////////////////////////////////////
            // init data file name
            CDBInfo dbInfo;
            InitDataFileNames(out dbInfo);
            CObjectInfo.DBInfo = dbInfo;
            MLog.DBInfo = dbInfo;

            CObjectInfo objInfo;
            m_SystemInfo = new MSystemInfo();

            // self set MLWDicer
            m_SystemInfo.GetObjectInfo(0, out objInfo);
            this.ObjInfo = objInfo;

            // DataManager
            m_SystemInfo.GetObjectInfo(1, out objInfo);
            m_DataManager = new MDataManager(objInfo, dbInfo);

            ////////////////////////////////////////////////////////////////////////
            // 1. Hardware Layer
            ////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////
            // Motion
            m_SystemInfo.GetObjectInfo(3, out objInfo);
            CreateYMCBoard(objInfo);

            m_SystemInfo.GetObjectInfo(4, out objInfo);
            CreateACSChannel(objInfo);

            ////////////////////////////////////////////////////////////////////////
            // MultiAxes
            CreateMultiAxes_YMC();
            m_AxUpperHandler.UpdateAxisStatus();

            ////////////////////////////////////////////////////////////////////////
            // IO
            m_SystemInfo.GetObjectInfo(6, out objInfo);
            m_IO = new MIO_YMC(objInfo);
            m_IO.OutputOn(oUHandler_Self_Vac_On);

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
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.PUSHPULL_GRIPPER, out m_PushPullGripperCyl);

            // PushPullUDCyl
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.UP_DOWN;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.UpSensor[0] = iUHandler_Up1;
            cylData.DownSensor[0] = iUHandler_Down2;
            cylData.Solenoid[0] = oUHandler_Up1;
            cylData.Solenoid[1] = oUHandler_Down2;

            m_SystemInfo.GetObjectInfo(101, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.PUSHPULL_UD, out m_PushPullUDCyl);

            // Spinner1
            // Spin Coater Up & Down Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.UP_DOWN;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.UpSensor[0] = iStage1Up;
            cylData.DownSensor[0] = iStage1Down;
            cylData.Solenoid[0] = oStage1_Up;
            cylData.Solenoid[1] = oStage1_Down;

            m_SystemInfo.GetObjectInfo(110, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER1_UD, out m_Spinner1UDCyl);

            // Spin Coater DI Valve Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.SINGLE_SOLENOID;
            cylData.Solenoid[0] = oCoat_DI;

            m_SystemInfo.GetObjectInfo(111, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER1_DI, out m_Spinner1DICyl);

            // Spin Coater PVA Valve Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.SINGLE_SOLENOID;
            cylData.Solenoid[0] = oCoat_PVA;

            m_SystemInfo.GetObjectInfo(112, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER1_PVA, out m_Spinner1PVACyl);

            // Spin Cleaner Up & Down Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.UP_DOWN;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.UpSensor[0] = iStage2Up;
            cylData.DownSensor[0] = iStage2Down;
            cylData.Solenoid[0] = oStage2_Up;
            cylData.Solenoid[1] = oStage2_Down;

            m_SystemInfo.GetObjectInfo(113, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER2_UD, out m_Spinner2UDCyl);


            // Spin Cleaner DI Valve Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.SINGLE_SOLENOID;
            cylData.Solenoid[0] = oClean_DI;

            m_SystemInfo.GetObjectInfo(114, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER2_DI, out m_Spinner2DICyl);

            // Spin Cleaner N2 Valve Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.SINGLE_SOLENOID;
            cylData.Solenoid[0] = oClean_N2;

            m_SystemInfo.GetObjectInfo(115, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.SPINNER2_PVA, out m_Spinner2PVACyl);

            // Stage Clamp 1 Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.Solenoid[0] = oStageClamp1_Open;
            cylData.Solenoid[1] = oStageClamp1_Close;

            m_SystemInfo.GetObjectInfo(116, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.STAGE_CLAMP1, out m_StageClamp1);

            // Stage Clamp 2 Open Close Cylinder
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.OPEN_CLOSE;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.Solenoid[0] = oStageClamp2_Open;
            cylData.Solenoid[1] = oStageClamp2_Close;

            m_SystemInfo.GetObjectInfo(117, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.STAGE_CLAMP2, out m_StageClamp2);

            ////////////////////////////////////////////////////////////////////////
            // Vacuum
            // Stage1 Vacuum
            CVacuumData vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iStage1_Vac_On;
            vacData.Solenoid[0] = oStage1_Vac_On;
            vacData.Solenoid[1] = oStage1_Vac_Off;

            m_SystemInfo.GetObjectInfo(150, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.STAGE1, out m_Stage1Vac);

            // UpperHandler
            // UpperHandler Self Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iUHandler_Self_Vac_On;
            vacData.Solenoid[0] = oUHandler_Self_Vac_On;
            vacData.Solenoid[1] = oUHandler_Self_Vac_Off;

            m_SystemInfo.GetObjectInfo(151, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.UPPER_HANDLER_SELF, out m_UHandlerSelfVac);

            // UpperHandler Factory Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iUHandler_Self_Vac_On;
            vacData.Solenoid[0] = oUHandler_Self_Vac_On;
            vacData.Solenoid[1] = oUHandler_Self_Vac_Off;

            m_SystemInfo.GetObjectInfo(152, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.UPPER_HANDLER_FACTORY, out m_UHandlerSelfVac);

            // LowerHandler
            // LowerHandler Self Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            //vacData.Sensor[0] = iLHandler_Self_Vac_On;
            //vacData.Solenoid[0] = oLHandler_Self_Vac_On;
            //vacData.Solenoid[1] = oLHandler_Self_Vac_Off;

            m_SystemInfo.GetObjectInfo(153, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.LOWER_HANDLER_SELF, out m_LHandlerSelfVac);

            // LowerHandler Factory Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            //vacData.Sensor[0] = iLHandler_Self_Vac_On;
            //vacData.Solenoid[0] = oLHandler_Self_Vac_On;
            //vacData.Solenoid[1] = oLHandler_Self_Vac_Off;

            m_SystemInfo.GetObjectInfo(154, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.LOWER_HANDLER_FACTORY, out m_LHandlerSelfVac);

            // Spinner1 Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.DOUBLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iStage2_PanelDetect;
            vacData.Solenoid[0] = oStage2_Vac_On;
            vacData.Solenoid[1] = oStage2_Vac_Off;
            vacData.Solenoid[2] = oStage2_Blow;

            m_SystemInfo.GetObjectInfo(152, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.SPINNER1, out m_Spinner1Vac);

            // Spinner2 Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.DOUBLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iStage3_PanelDetect;
            vacData.Solenoid[0] = oStage3_Vac_On;
            vacData.Solenoid[1] = oStage3_Vac_Off;
            vacData.Solenoid[2] = oStage3_Blow;

            m_SystemInfo.GetObjectInfo(153, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.SPINNER2, out m_Spinner2Vac);

            ////////////////////////////////////////////////////////////////////////
            // ComPort
            // Polygon Scanner Serial Com Port
            m_SystemInfo.GetObjectInfo(30, out objInfo);
            CreatePolygonSerialPort(objInfo, out m_PolygonComPort);

            CPolygonIni PolygonIni = new CPolygonIni();
            m_SystemInfo.GetObjectInfo(200, out objInfo);
            CreatePolygonScanner(objInfo, PolygonIni, (int)EObjectScanner.SCANNER1, m_PolygonComPort);

            ////////////////////////////////////////////////////////////////////////
            // Vision
            // Vision System
            m_SystemInfo.GetObjectInfo(40, out objInfo);
            CreateVisionSystem(objInfo);
            // Vision Camera
            m_SystemInfo.GetObjectInfo(42, out objInfo);
            CreateVisionCamera(objInfo);
            // Vision Display
            m_SystemInfo.GetObjectInfo(46, out objInfo);
            MVisionView(objInfo);

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
            m_trsAutoManager.SetWindows_Form1(form1);

            ////////////////////////////////////////////////////////////////////////
            // 5. Set Data
            ////////////////////////////////////////////////////////////////////////
            SetSystemDataToComponent();
            SetModelDataToComponent();
            SetPositionDataToComponent();

            ////////////////////////////////////////////////////////////////////////
            // 6. Start Thread & System
            ////////////////////////////////////////////////////////////////////////
            m_YMC.ThreadStart();

            SetThreadChannel();
            StartThreads();


            TestFunction_AfterInit();

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

#if !SIMULATION_MOTION
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

#if !SIMULATION_MOTION
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

            for(int i = 0; i < DEF_MAX_COORDINATE; i++)
                {
                initArray[i] = DEF_AXIS_NONE_ID;
                }

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

            // CAMERA1
            deviceNo = (int)EYMC_Device.CAMERA1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EYMC_Axis.CAMERA1_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(263, out objInfo);
            m_AxCamera1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // SCANNER1
            deviceNo = (int)EYMC_Device.SCANNER1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EYMC_Axis.SCANNER1_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(264, out objInfo);
            m_AxLaser1 = new MMultiAxes_YMC(objInfo, refComp, data);

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
#if SIMULATION_VISION
                return SUCCESS;
#endif

            int iResult = 0;
            // Vision System 생성
            m_VisionSystem = new MVisionSystem(objInfo);
            // GigE Cam초기화 & MIL 초기화
            iResult = m_VisionSystem.Initialize();

            if (iResult != SUCCESS) return iResult;

            // GigE Camera 개수 확인
            int iGetCamNum = m_VisionSystem.GetCamNum();
            if (iGetCamNum != DEF_MAX_CAMERA_NO) return ERR_VISION_ERROR;

            return SUCCESS;
        }

        int CreateVisionCamera(CObjectInfo objInfo)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            m_VisionCamera = new MVisionCamera[DEF_MAX_CAMERA_NO];

            // Camera & View 를 생성함.
            for (int iIndex = 0; iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                // Camera를 생성함.
                m_VisionCamera[iIndex] = new MVisionCamera(objInfo);
                // Vision Library MIL
                m_VisionCamera[iIndex].SetMil_ID(m_VisionSystem.GetMilSystem());
                // Camera 초기화
                m_VisionCamera[iIndex].Initialize(iIndex, m_VisionSystem.GetSystem());
                
            }

            return SUCCESS;
        }
        int MVisionView(CObjectInfo objInfo)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            m_VisionView = new MVisionView[DEF_MAX_CAMERA_NO];

            // Camera & View 를 생성함.
            for (int iIndex = 0; iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                // Display View 생성함.
                m_VisionView[iIndex] = new MVisionView(objInfo);
                // Vision Library MIL
                m_VisionView[iIndex].SetMil_ID(m_VisionSystem.GetMilSystem());
                // Display 초기화
                m_VisionView[iIndex].Initialize(iIndex, m_VisionCamera[iIndex]);

            }
            return SUCCESS;
        }

        void CreateVision(CObjectInfo objInfo)
        {
#if !SIMULATION_VISION
            bool VisionHardwareCheck = true;
            if(m_VisionSystem.m_iResult != SUCCESS)
            {
                VisionHardwareCheck = false;
            }
            CVisionData data = new CVisionData();
            CVisionRefComp refComp = new CVisionRefComp();

            // 생성된 Vision System,Camera, View 를 RefComp로 연결
            refComp.System = m_VisionSystem;

            for (int iIndex = 0; iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                if(m_VisionCamera[iIndex].m_iResult != SUCCESS || m_VisionView[iIndex].m_iResult != SUCCESS)
                {
                    VisionHardwareCheck = false;
                    break;
                }
                refComp.Camera[iIndex] = m_VisionCamera[iIndex];
                refComp.View[iIndex]   = m_VisionView[iIndex];

                // Display와 Camera와 System을 연결
                refComp.Camera[iIndex].SelectView(refComp.View[iIndex]);
                refComp.System.SelectCamera(refComp.Camera[iIndex]);
                refComp.System.SelectView(refComp.View[iIndex]);
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

            // Pattern Model Data Read & Apply
            CModelData pModelData;
            m_DataManager.ViewModelData("Default", out pModelData);
            m_DataManager.m_ModelData = pModelData;

            m_Vision.ReLoadPatternMark(PRE__CAM, PATTERN_A, m_DataManager.m_ModelData.MacroPatternA);
            m_Vision.ReLoadPatternMark(PRE__CAM, PATTERN_B, m_DataManager.m_ModelData.MacroPatternB);
            m_Vision.ReLoadPatternMark(FINE_CAM, PATTERN_A, m_DataManager.m_ModelData.MicroPatternA);
            m_Vision.ReLoadPatternMark(FINE_CAM, PATTERN_B, m_DataManager.m_ModelData.MicroPatternB);

#endif

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

            m_ctrlStage1 = new MCtrlStage1(objInfo, refComp, data);
        }

        void CreateCtrlLoader(CObjectInfo objInfo)
        {
            CCtrlLoaderRefComp refComp = new CCtrlLoaderRefComp();
            CCtrlLoaderData data = new CCtrlLoaderData();

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

            m_ctrlPushPull = new MCtrlPushPull(objInfo, refComp, data);
        }

        void CreateCtrlSpinner1(CObjectInfo objInfo)
        {
            CCtrlSpinnerRefComp refComp = new CCtrlSpinnerRefComp();
            CSpinnerData data = m_DataManager.ModelData.SpinnerData;

            refComp.Spinner = m_MeSpinner1;

            m_ctrlSpinner1 = new MCtrlSpinner(objInfo, refComp, data);
        }

        void CreateCtrlSpinner2(CObjectInfo objInfo)
        {
            CCtrlSpinnerRefComp refComp = new CCtrlSpinnerRefComp();
            CSpinnerData data = m_DataManager.ModelData.SpinnerData;

            refComp.Spinner = m_MeSpinner2;

            m_ctrlSpinner2 = new MCtrlSpinner(objInfo, refComp, data);
        }

        void CreateTrsAutoManager(CObjectInfo objInfo)
        {
            CTrsAutoManagerRefComp refComp = new CTrsAutoManagerRefComp();
            refComp.IO = m_IO;
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

            m_trsAutoManager = new MTrsAutoManager(objInfo, TrsAutoManager, m_DataManager, ELCNetUnitPos.NONE, refComp, data);
        }

        void CreateTrsLoader(CObjectInfo objInfo)
        {
            CTrsLoaderRefComp refComp = new CTrsLoaderRefComp();
            refComp.ctrlLoader = m_ctrlLoader;

            CTrsLoaderData data = new CTrsLoaderData();

            m_trsLoader = new MTrsLoader(objInfo, TrsLoader, m_DataManager, ELCNetUnitPos.NONE, refComp, data);
        }

        void CreateTrsPushPull(CObjectInfo objInfo)
        {
            CTrsPushPullRefComp refComp = new CTrsPushPullRefComp();
            refComp.ctrlPushPull = m_ctrlPushPull;

            CTrsPushPullData data = new CTrsPushPullData();

            m_trsPushPull = new MTrsPushPull(objInfo, TrsLoader, m_DataManager, ELCNetUnitPos.PUSHPULL, refComp, data);
        }

        void CreateTrsSpinner1(CObjectInfo objInfo)
        {
            CTrsSpinnerRefComp refComp = new CTrsSpinnerRefComp();
            refComp.ctrlSpinner = m_ctrlSpinner1;
            refComp.ctrlPushPull = m_ctrlPushPull;

            CTrsSpinnerData data = new CTrsSpinnerData();

            m_trsSpinner1 = new MTrsSpinner(objInfo, TrsSpinner1, m_DataManager, ELCNetUnitPos.SPINNER1, refComp, data);
        }

        void CreateTrsSpinner2(CObjectInfo objInfo)
        {
            CTrsSpinnerRefComp refComp = new CTrsSpinnerRefComp();
            refComp.ctrlSpinner = m_ctrlSpinner2;
            refComp.ctrlPushPull = m_ctrlPushPull;

            CTrsSpinnerData data = new CTrsSpinnerData();

            m_trsSpinner2 = new MTrsSpinner(objInfo, TrsSpinner2, m_DataManager, ELCNetUnitPos.SPINNER2, refComp, data);
        }

        void CreateTrsHandler(CObjectInfo objInfo)
        {
            CTrsHandlerRefComp refComp = new CTrsHandlerRefComp();
            refComp.ctrlHandler = m_ctrlHandler;
            refComp.ctrlPushPull = m_ctrlPushPull;

            CTrsHandlerData data = new CTrsHandlerData();

            m_trsHandler = new MTrsHandler(objInfo, TrsHandler, m_DataManager, ELCNetUnitPos.UPPER_HANDLER, refComp, data);
        }

        void CreateTrsStage1(CObjectInfo objInfo)
        {
            CTrsStage1RefComp refComp = new CTrsStage1RefComp();
            refComp.ctrlStage1 = m_ctrlStage1;

            CTrsStage1Data data = new CTrsStage1Data();

            m_trsStage1 = new MTrsStage1(objInfo, TrsLoader, m_DataManager, ELCNetUnitPos.STAGE1, refComp, data);
        }

        void CreatePolygonScanner(CObjectInfo objInfo, CPolygonIni PolygonIni, int objIndex, ISerialPort m_ComPort)
        {
            m_DataManager.SystemData_Scanner.Scanner[objIndex] = PolygonIni;

            m_DataManager.SystemData_Scanner.Scanner[objIndex].strIP = "192.168.1.161";
            m_DataManager.SystemData_Scanner.Scanner[objIndex].strPort = "70";

            m_Scanner[objIndex] = new MPolygonScanner(objInfo, m_DataManager.SystemData_Scanner.Scanner[objIndex], objIndex, m_ComPort);
        }

        void CreatePolygonSerialPort(CObjectInfo objInfo, out ISerialPort pComport)
        {
            // Polygon Scanner Serial Port 
            string PortName = "COM3";
            int BaudRate = 57600;
            Parity _Parity = Parity.None;
            int DataBits = 8;
            StopBits _StopBits = StopBits.One;

            CSerialPortData SerialCom = new CSerialPortData(PortName, BaudRate, _Parity, DataBits, _StopBits);

            pComport = new MSerialPort(objInfo, SerialCom);

        }

        void SetThreadChannel()
        {
            // AutoManager
            m_trsAutoManager.LinkThread(TrsSelfChannel, m_trsAutoManager);
            m_trsAutoManager.LinkThread(TrsLoader, m_trsLoader);
            m_trsAutoManager.LinkThread(TrsPushPull, m_trsPushPull);
            m_trsAutoManager.LinkThread(TrsStage1, m_trsStage1);

            // Loader
            m_trsLoader.LinkThread(TrsSelfChannel, m_trsLoader);
            m_trsLoader.LinkThread(TrsAutoManager, m_trsAutoManager);
            m_trsLoader.LinkThread(TrsPushPull, m_trsPushPull);
            m_trsLoader.LinkThread(TrsStage1, m_trsStage1);

            // PushPull
            m_trsPushPull.LinkThread(TrsSelfChannel, m_trsPushPull);
            m_trsPushPull.LinkThread(TrsAutoManager, m_trsAutoManager);
            m_trsPushPull.LinkThread(TrsLoader, m_trsLoader);
            m_trsPushPull.LinkThread(TrsSpinner1, m_trsSpinner1);
            m_trsPushPull.LinkThread(TrsSpinner2, m_trsSpinner2);
            m_trsPushPull.LinkThread(TrsHandler, m_trsHandler);

            // Spinner
            m_trsSpinner1.LinkThread(TrsSelfChannel, m_trsSpinner1);
            m_trsSpinner1.LinkThread(TrsAutoManager, m_trsAutoManager);
            m_trsSpinner1.LinkThread(TrsPushPull, m_trsPushPull);

            m_trsSpinner2.LinkThread(TrsSelfChannel, m_trsSpinner2);
            m_trsSpinner2.LinkThread(TrsAutoManager, m_trsAutoManager);
            m_trsSpinner2.LinkThread(TrsPushPull, m_trsPushPull);

            // Handler
            m_trsHandler.LinkThread(TrsSelfChannel, m_trsHandler);
            m_trsHandler.LinkThread(TrsAutoManager, m_trsAutoManager);
            m_trsHandler.LinkThread(TrsPushPull, m_trsPushPull);
            m_trsHandler.LinkThread(TrsStage1, m_trsStage1);

            // Stage1
            m_trsStage1.LinkThread(TrsSelfChannel, m_trsStage1);
            m_trsStage1.LinkThread(TrsAutoManager, m_trsAutoManager);
            m_trsStage1.LinkThread(TrsHandler, m_trsHandler);
        }

        void StartThreads()
        {
            m_trsLoader.ThreadStart();
            m_trsPushPull.ThreadStart();
            m_trsStage1.ThreadStart();
            m_trsAutoManager.ThreadStart();
        }

        public void StopThreads()
        {
            m_trsLoader.ThreadStop();
            m_trsPushPull.ThreadStop();
            m_trsStage1.ThreadStop();
            m_trsAutoManager.ThreadStop();
        }

        public int SaveSystemData(CSystemData system = null, CSystemData_Axis systemAxis = null,
            CSystemData_Cylinder systemCylinder = null, CSystemData_Vacuum systemVacuum = null,
            CSystemData_Scanner systemScanner = null)
        {
            int iResult;

            // save
            iResult = m_DataManager.SaveSystemData(system, systemAxis, systemCylinder, systemVacuum, systemScanner);
            if (iResult != SUCCESS) return SUCCESS;

            // set
            SetSystemDataToComponent();

            return SUCCESS;
        }

        private void SetSystemDataToComponent()
        {
            m_DataManager.LoadSystemData();
            m_DataManager.LoadModelList();

            MLWDicer.bInSfaTest = m_DataManager.SystemData.UseInSfaTest;
            MLWDicer.bUseOnline = m_DataManager.SystemData.UseOnLineUse;

            // set system data to each component

            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer

            // MeElevator
            {
                CMeElevatorData data;
                m_MeElevator.GetData(out data);
                data.ElevatorSafetyPos = m_DataManager.SystemData.MAxSafetyPos.Elevator_Pos;
                m_MeElevator.SetData(data);
            }

            // MeHandler
            {
                // Load Upper Handler
                CMeHandlerData data;
                m_MeUpperHandler.GetData(out data);
                data.HandlerSafetyPos = m_DataManager.SystemData.MAxSafetyPos.UHandler_Pos;

                m_MeUpperHandler.SetData(data);

                // Unload Lower Handler
                m_MeLowerHandler.GetData(out data);
                data.HandlerSafetyPos = m_DataManager.SystemData.MAxSafetyPos.LHandler_Pos;

                m_MeLowerHandler.SetData(data);
            }

            // Spinner
            {
                CMeSpinnerData data;
                m_MeSpinner1.GetData(out data);
                data.CleanNozzleSafetyPos = m_DataManager.SystemData.MAxSafetyPos.S1_CleanNozzel_Pos;
                data.CoatNozzleSafetyPos = m_DataManager.SystemData.MAxSafetyPos.S1_CoatNozzel_Pos;
                m_MeSpinner1.SetData(data);

                m_MeSpinner2.GetData(out data);
                data.CleanNozzleSafetyPos = m_DataManager.SystemData.MAxSafetyPos.S2_CleanNozzel_Pos;
                data.CoatNozzleSafetyPos = m_DataManager.SystemData.MAxSafetyPos.S2_CoatNozzel_Pos;
                m_MeSpinner2.SetData(data);
            }

            // PushPull
            {
                CMePushPullData data;
                m_MePushPull.GetData(out data);

                data.PushPullSafetyPos = m_DataManager.SystemData.MAxSafetyPos.PushPull_Pos;
                data.CenterSafetyPos = m_DataManager.SystemData.MAxSafetyPos.Centering_Pos;
                m_MePushPull.SetData(data);
            }

            // Stage
            {
                CMeStageData data;
                m_MeStage.GetData(out data);

                data.StageSafetyPos = m_DataManager.SystemData.MAxSafetyPos.Stage_Pos;
                m_MeStage.SetData(data);
            }

            //////////////////////////////////////////////////////////////////
            // Control Layer


            //////////////////////////////////////////////////////////////////
            // Process Layer
            {
                CTrsAutoManagerData data;
                m_trsAutoManager.GetData(out data);
                data.UseVIPMode = data.UseVIPMode;
                m_trsAutoManager.SetData(data);
            }

        }

        public int SaveModelData(CModelData modelData)
        {
            int iResult;

            // save
            iResult = m_DataManager.SaveModelData(modelData);
            if (iResult != SUCCESS) return SUCCESS;

            // set
            SetModelDataToComponent();

            return SUCCESS;
        }

        public void SetModelDataToComponent()
        {
            m_DataManager.ChangeModel(m_DataManager.SystemData.ModelName);

            // set model data to each component


            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer

            // MMeHandler
            m_MeUpperHandler.SetCylUseFlag(m_DataManager.ModelData.MeUH_UseMainCylFlag,
                m_DataManager.ModelData.MeUH_UseSubCylFlag, m_DataManager.ModelData.MeUH_UseGuideCylFlag);
            m_MeUpperHandler.SetVccUseFlag(m_DataManager.ModelData.MeUH_UseVccFlag);

            m_MeLowerHandler.SetCylUseFlag(m_DataManager.ModelData.MeLH_UseMainCylFlag,
                m_DataManager.ModelData.MeLH_UseSubCylFlag, m_DataManager.ModelData.MeLH_UseGuideCylFlag);
            m_MeLowerHandler.SetVccUseFlag(m_DataManager.ModelData.MeLH_UseVccFlag);

            //////////////////////////////////////////////////////////////////
            // Control Layer




            //////////////////////////////////////////////////////////////////
            // Process Layer


        }

        public void SetPositionDataToComponent(EPositionObject unit = EPositionObject.ALL)
        {
            m_DataManager.LoadPositionData(true, unit);
            m_DataManager.LoadPositionData(false, unit);
            m_DataManager.GenerateModelPosition();

            CPositionData FixedPos = m_DataManager.FixedPos;
            CPositionData ModelPos = m_DataManager.ModelPos;
            CPositionData OffsetPos = m_DataManager.OffsetPos;

            // set position data to each component

            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer

            // Loader
            m_MeElevator.SetElevatorPosition(FixedPos.LoaderPos, ModelPos.LoaderPos, OffsetPos.LoaderPos);

            // PushPull
            m_MePushPull.SetPushPullPosition(FixedPos.PushPullPos, ModelPos.PushPullPos, OffsetPos.PushPullPos);
            m_MePushPull.SetCenteringPosition(DEF_MePushPull.ECenterIndex.LEFT, FixedPos.Centering1Pos, ModelPos.Centering1Pos, OffsetPos.Centering1Pos);
            m_MePushPull.SetCenteringPosition(DEF_MePushPull.ECenterIndex.RIGHT, FixedPos.Centering2Pos, ModelPos.Centering2Pos, OffsetPos.Centering2Pos);

            // Spinner
            m_MeSpinner1.SetRotatePosition(FixedPos.S1_RotatePos, ModelPos.S1_RotatePos, OffsetPos.S1_RotatePos);
            m_MeSpinner1.SetCleanPosition(FixedPos.S1_CleanerPos, ModelPos.S1_CleanerPos, OffsetPos.S1_CleanerPos);
            m_MeSpinner1.SetCoatPosition(FixedPos.S1_CoaterPos, ModelPos.S1_CoaterPos, OffsetPos.S1_CoaterPos);

            m_MeSpinner2.SetRotatePosition(FixedPos.S2_RotatePos, ModelPos.S2_RotatePos, OffsetPos.S2_RotatePos);
            m_MeSpinner2.SetCleanPosition(FixedPos.S2_CleanerPos, ModelPos.S2_CleanerPos, OffsetPos.S2_CleanerPos);
            m_MeSpinner2.SetCoatPosition(FixedPos.S2_CoaterPos, ModelPos.S2_CoaterPos, OffsetPos.S2_CoaterPos);

            // Handler
            m_MeUpperHandler.SetHandlerPosition(FixedPos.LowerHandlerPos, ModelPos.LowerHandlerPos, OffsetPos.LowerHandlerPos);
            m_MeLowerHandler.SetHandlerPosition(FixedPos.UpperHandlerPos, ModelPos.UpperHandlerPos, OffsetPos.UpperHandlerPos);

            // Stage
            m_MeStage.SetStagePosition(FixedPos.Stage1Pos, ModelPos.Stage1Pos, OffsetPos.Stage1Pos);
            m_MeStage.SetCameraPosition(FixedPos.Camera1Pos, ModelPos.Camera1Pos, OffsetPos.Camera1Pos);
            m_MeStage.SetLaserPosition(FixedPos.Scanner1Pos, ModelPos.Scanner1Pos, OffsetPos.Scanner1Pos);

            //////////////////////////////////////////////////////////////////
            // Control Layer

            //////////////////////////////////////////////////////////////////
            // Process Layer

        }

        public bool GetKeyPad(string StrCurrent, out string strModify)
        {
            FormKeyPad KeyPad = new FormKeyPad();
            KeyPad.SetValue(StrCurrent);
            KeyPad.ShowDialog();

            if (KeyPad.DialogResult == DialogResult.OK)
            {
                if (KeyPad.ModifyNo.Text == "")
                {
                    strModify = "0";
                }
                else
                {
                    strModify = KeyPad.ModifyNo.Text;
                }
            }
            else
            {
                strModify = StrCurrent;
                KeyPad.Dispose();
                return false;
            }
            KeyPad.Dispose();
            return true;
        }

        public bool GetKeyboard(out string strModify)
        {
            FormKeyBoard Keyboard = new FormKeyBoard();
            Keyboard.ShowDialog();

            if (Keyboard.DialogResult == DialogResult.OK)
            {
                strModify = Keyboard.PresentNo.Text;
            }
            else
            {
                strModify = "";
                Keyboard.Dispose();
                return false;
            }
            Keyboard.Dispose();
            return true;
        }

        public bool DisplayMsg(string strMsg)
        {
            FormMessageBox Msg = new FormMessageBox();
            Msg.SetText(strMsg);
            Msg.ShowDialog();

            if (Msg.DialogResult == DialogResult.OK)
            {
                Msg.Dispose();
                return true;
            }
            else
            {
                Msg.Dispose();
                return false;
            }
        }

        public void AlarmDisplay(int nCodeNo)
        {
            FormAlarmDisplay Alarm = new FormAlarmDisplay();
            Alarm.SetAlarmCode(nCodeNo);
            Alarm.ShowDialog();
        }

        void CreateMeElevator(CObjectInfo objInfo)
        {
            CMeElevatorRefComp refComp = new CMeElevatorRefComp();
            CMeElevatorData data = new CMeElevatorData();

            refComp.IO = m_IO;
            refComp.AxElevator = m_AxLoader;            

            // IO 설정은 임시로 진행함.
            data.InDetectWafer = iUHandler_PanelDetect;
            data.InDetectCassette[CASSETTE_DETECT_SENSOR_1] = iInterface_00;
            data.InDetectCassette[CASSETTE_DETECT_SENSOR_2] = iInterface_00;
            data.InDetectCassette[CASSETTE_DETECT_SENSOR_3] = iInterface_00;
            data.InDetectCassette[CASSETTE_DETECT_SENSOR_4] = iInterface_00;

            data.ElevatorZone.UseSafetyMove[DEF_Z] = true;
            data.ElevatorZone.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = 111; // need updete io address

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

            data.CenterZone[(int)ECenterIndex.LEFT].Axis[DEF_X].ZoneAddr[(int)ECenterXAxZone.SAFETY] = 111; // need updete io address
            data.CenterZone[(int)ECenterIndex.RIGHT].Axis[DEF_X].ZoneAddr[(int)ECenterXAxZone.SAFETY] = 111; // need updete io address

            m_MePushPull = new MMePushPull(objInfo, refComp, data);
        }

        void CreateMeOpPanel(CObjectInfo objInfo)
        {
            COpPanelRefComp refComp = new COpPanelRefComp();
            COpPanelData data = new COpPanelData();

            refComp.IO = m_IO;
            refComp.Yaskawa_Motion = m_YMC;
            refComp.ACS_Motion = m_ACS;

            //data.bUseMaterialAlarm = 

            COpPanelIOAddr sPanelIOAddr = new COpPanelIOAddr();

            CJogTable sJogTable = new CJogTable();

            m_OpPanel = new MOpPanel(objInfo, refComp, data, sPanelIOAddr, sJogTable);
        }

        void CreateMeStage(CObjectInfo objInfo)
        {
            CMeStageRefComp refComp = new CMeStageRefComp();
            CMeStageData data = new CMeStageData();

            refComp.IO = m_IO;
            refComp.AxStage = m_AxStage1;
            refComp.Vacuum[(int)EStageVacuum.SELF] = m_Stage1Vac;
            
            data.InDetectObject = iUHandler_PanelDetect;

            data.StageZone.UseSafetyMove[DEF_Z] = true;
            data.StageZone.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = 111; // need updete io address

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
            data.InRotateLoadPos = 111; // need updete io address

            data.CleanNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY] = 111; // need updete io address
            data.CoatNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY] = 111; // need updete io address

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
            data.InRotateLoadPos = 111; // need updete io address

            data.CleanNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY] = 111; // need updete io address
            data.CoatNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY] = 111; // need updete io address

            m_MeSpinner2 = new MMeSpinner(objInfo, refComp, data);
        }

    }
}
