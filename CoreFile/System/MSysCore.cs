using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO.Ports;

using System.Windows.Forms;

using Core.UI;
using MotionYMC;

using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_IO;

using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_LCNet;
using static Core.Layers.DEF_SocketClient;

using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_Yaskawa;
using static Core.Layers.DEF_ACS;
using static Core.Layers.DEF_MultiAxesYMC;
using static Core.Layers.DEF_MultiAxesACS;
using static Core.Layers.DEF_Cylinder;
using static Core.Layers.DEF_Vacuum;
using static Core.Layers.DEF_Vision;
using static Core.Layers.DEF_Polygon;

using static Core.Layers.DEF_OpPanel;
using static Core.Layers.DEF_MeStage;
using static Core.Layers.DEF_SerialPort;

using static Core.Layers.DEF_CtrlOpPanel;
using static Core.Layers.DEF_CtrlStage;

using static Core.Layers.DEF_TrsAutoManager;


namespace Core.Layers
{
    public class MSysCore : MObject, IDisposable
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
        public MMultiAxes_ACS m_AxStage1;
        public MMultiAxes_ACS m_AxCamera1;
        public MMultiAxes_ACS m_AxScannerZ1;

        // Vision
        public MVisionSystem m_VisionSystem;
        public MVisionCamera[] m_VisionCamera = new MVisionCamera[DEF_MAX_CAMERA_NO];
        public MVisionView[] m_VisionView = new MVisionView[DEF_MAX_CAMERA_NO];
        
        // IO
        public IIO m_IO { get; private set; }

        ///////////////////////////////////////////////////////////////////////
        // Mechanical Layer
        // Cylinder
        public ICylinder m_StageClamp1       ;  // Work Stage Clamp 1 Cylinder [Double]
        public ICylinder m_StageClamp2       ;  // Work Stage Clamp 2 Cylinder [Double]

        // Vacuum
        public IVacuum m_Stage1Vac           ;
        
        // Vision
        public MVision m_Vision { get; set; }

        // Polygon Scanner
        public MMeScannerPolygon m_MeScanner;

        ///////////////////////////////////////////////////////////////////////
        // Mechanical Layer
        
        public MMeStage    m_MeStage;      
        public MOpPanel    m_OpPanel;

        ///////////////////////////////////////////////////////////////////////
        // Control Layer
        public MCtrlOpPanel m_ctrlOpPanel { get; private set; }        
        public MCtrlStage1 m_ctrlStage1 { get; private set; }


        ///////////////////////////////////////////////////////////////////////
        // Process Layer
        public MTrsAutoManager m_trsAutoManager { get; private set; }        
        public MTrsStage1 m_trsStage1 { get; private set; }

        public FormIntro intro = new FormIntro();

        public MSysCore(CObjectInfo objInfo)
            : base(objInfo)
        {
        }

        ~MSysCore()
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

            // self set MSysCore
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

            // Stage1
            m_SystemInfo.GetObjectInfo(303, out objInfo);
            CreateMeStage(objInfo);  

            // Vision 
            m_SystemInfo.GetObjectInfo(308, out objInfo);
            CreateVision(objInfo);
                

            ////////////////////////////////////////////////////////////////////////
            // 3. Control Layer
            ////////////////////////////////////////////////////////////////////////    

            m_SystemInfo.GetObjectInfo(353, out objInfo);
            CreateCtrlStage1(objInfo);

            m_SystemInfo.GetObjectInfo(350, out objInfo);
            CreateCtrlOpPanel(objInfo);

            intro.SetStatus("Init Process Layer", 50);

            ////////////////////////////////////////////////////////////////////////
            // 4. Process Layer
            ////////////////////////////////////////////////////////////////////////
            
            m_SystemInfo.GetObjectInfo(403, out objInfo);
            CreateTrsStage1(objInfo);
            
            m_SystemInfo.GetObjectInfo(400, out objInfo);
            CreateTrsAutoManager(objInfo);

            // temporary set windows
            m_trsStage1.SetWindows_Form1(form1);
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
        
        void CreateTrsAutoManager(CObjectInfo objInfo)
        {
            CTrsAutoManagerRefComp refComp = new CTrsAutoManagerRefComp();
            refComp.IO = m_IO;
            refComp.YMC = m_YMC;
            refComp.ACS = m_ACS;
            refComp.OpPanel = m_OpPanel;

            refComp.ctrlOpPanel = m_ctrlOpPanel;
            refComp.ctrlStage1 = m_ctrlStage1;            

            CTrsAutoManagerData data = new CTrsAutoManagerData();

            m_trsAutoManager = new MTrsAutoManager(objInfo, EThreadChannel.TrsAutoManager, m_DataManager, ELCNetUnitPos.NONE, refComp, data);
        }
        

        void CreateTrsStage1(CObjectInfo objInfo)
        {
            CTrsStage1RefComp refComp = new CTrsStage1RefComp();
            refComp.ctrlStage1 = m_ctrlStage1;

            CTrsStage1Data data = new CTrsStage1Data();

            m_trsStage1 = new MTrsStage1(objInfo, EThreadChannel.TrsStage1, m_DataManager, ELCNetUnitPos.STAGE1, refComp, data);
        }
        
        void SetThreadChannel()
        {
            // AutoManager
            m_trsAutoManager.LinkThread(EThreadChannel.TrsSelfChannel, m_trsAutoManager);
            m_trsAutoManager.LinkThread(EThreadChannel.TrsAutoManager, m_trsAutoManager);
            m_trsAutoManager.LinkThread(EThreadChannel.TrsStage1, m_trsStage1);            

            // Stage1
            m_trsStage1.LinkThread(EThreadChannel.TrsSelfChannel, m_trsStage1);
            m_trsStage1.LinkThread(EThreadChannel.TrsAutoManager, m_trsAutoManager);
        }

        void StartThreads()
        {
            m_trsStage1.ThreadStart();
            m_trsAutoManager.ThreadStart();
        }

        public void StopThreads()
        {
            m_trsStage1.ThreadStop();
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

            MSysCore.bInSfaTest = systemData.UseInSfaTest;
            MSysCore.bUseOnline = systemData.UseOnLineUse;
            MSysCore.Language   = systemData.Language;

            // set system data to each component

            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer
            

            // Stage
            {
                CMeStageData data;
                m_MeStage.GetData(out data);

                data.StageSafetyPos = systemData.MaxSafetyPos.Stage_Pos;

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
            

            // Stage 1
            {
                CMeStageData data;
                m_MeStage.GetData(out data);
                Array.Copy(modelData.MeStage_UseVccFlag, data.UseVccFlag, modelData.MeStage_UseVccFlag.Length);

                //data.StageSafetyPos = m_DataManager.SystemData.MaxSafetyPos.Stage_Pos;

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

            m_MeStage = new MMeStage(objInfo, refComp, data);
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
            m_MeStage.SetAutoManual(mode);

            // control layer
            m_ctrlOpPanel.SetAutoManual(mode);
            m_ctrlStage1.SetAutoManual(mode);

            // process layer
            m_trsAutoManager.SetAutoManual(mode);
            m_trsStage1.SetAutoManual(mode);
        }

        public int EmptyMethod()
        {
            return SUCCESS;
        }
    }
}
