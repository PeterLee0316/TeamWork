using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.IO;

using Excel = Microsoft.Office.Interop.Excel;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_OpPanel;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_DataManager;

using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_Cylinder;
using static LWDicer.Control.DEF_Vacuum;

using static LWDicer.Control.DEF_MeHandler;
using static LWDicer.Control.DEF_MeElevator;
using static LWDicer.Control.DEF_MePushPull;
using static LWDicer.Control.DEF_MeSpinner;
using static LWDicer.Control.DEF_PolygonScanner;
using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_CtrlSpinner;


namespace LWDicer.Control
{
    public class DEF_DataManager
    {
        public const int ERR_DATA_MANAGER_FAIL_BACKUP_DB             = 1;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_DB             = 2;
        public const int ERR_DATA_MANAGER_FAIL_DROP_TABLES           = 3;
        public const int ERR_DATA_MANAGER_FAIL_BACKUP_ROW            = 4;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA      = 5;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA      = 6;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_POSITION_DATA    = 7;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_POSITION_DATA    = 8;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MODEL_DATA       = 9;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA       = 10;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MODEL_LIST       = 11;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST       = 12;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA     = 13;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA     = 14;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_MODEL_DATA     = 15;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_LOGIN_HISTORY    = 16;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_LOGIN_HISTORY    = 17;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_ALARM_INFO       = 18;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_ALARM_INFO       = 19;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_ALARM_HISTORY    = 20;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_ALARM_HISTORY    = 21;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_ROOT_FOLDER    = 22;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_DEFAULT_MODEL  = 23;

        public const int ERR_DATA_MANAGER_IO_DATA_FILE_NOT_EXIST = 1;
        public const int ERR_DATA_MANAGER_IO_DATA_FILE_CLOSE_FAILURE = 2;
        public const int ERR_DATA_MANAGER_CAMERA_NO_OUT_RANGE = 3;
        public const int ERR_DATA_MANAGER_TEACHING_INFO_FILE_NOT_EXIST = 4;
        public const int ERR_DATA_MANAGER_UNIT_INDEX_OUT_RANGE = 5;
        public const int ERR_DATA_MANAGER_POS_INDEX_OUT_RANGE = 6;
        public const int ERR_DATA_MANAGER_ARG_NULL_POINTER = 7;
        public const int ERR_DATA_MANAGER_POS_INFO_FILE_CLOSE_FAILURE = 8;
        public const int ERR_DATA_MANAGER_VACUUM_INDEX_OUT_RANGE = 9;
        public const int ERR_DATA_MANAGER_IO_NOT_INITIALIZED = 10;
        public const int ERR_DATA_MANAGER_MODEL_NAME_NULL = 11;
        public const int ERR_DATA_MANAGER_MODEL_FILE_NOT_EXIST = 12;
        public const int ERR_DATA_MANAGER_MODEL_CHANGE_FAIL = 13;
        public const int ERR_DATA_MANAGER_DELETE_CURRENT_MODEL_FAIL = 14;
        public const int ERR_DATA_MANAGER_COPY_SELF_MODEL_INCORRECT = 15;
        public const int ERR_DATA_MANAGER_CREATE_MODEL_FAIL = 16;
        public const int ERR_DATA_MANAGER_INVALID_FIXEDCOORD_UNIT_ID = 17;

        public const int ERR_SYSTEM_DATA_MANAGER_FILE_SAVE_FAIL = 20;
        public const int ERR_SYSTEM_DATA_MANAGER_NO_SECTIONNAME = 21;

        public const int ERR_MODEL_DATA_MAIN_MODEL_NAME_NULL = 31;
        public const int ERR_MODEL_DATA_MANAGER_NO_SECTIONNAME = 32;


        public const int DEF_MAX_SYSTEM_SECTION = 7;
        public const int DEF_MAX_FIXED_POSITION_SECTION = 14;
        public const int DEF_MAX_OFFSET_POSITION_SECTION = 14;
        public const int DEF_MAX_AXIS_NUM = 4;


        public const int ERR_DATA_MANAGER_IO_EXCEL_FILE_READ_FAIL = 1;
        public const int ERR_DATA_MANAGER_SYSTEM_EXCEL_FILE_READ_FAIL = 2;
        public const int ERR_DATA_MANAGER_SYSTEM_EXCEL_FILE_SAVE_FAIL = 3;



        public class CSystemDataFileNames
        {
            public string SystemDataFile;
            public string LogDataFile;
            public string ProductDataFile;
            public string MotorDataFile;
            public string IndMotorActuatorDataFile;
            public string CylinderTimerDataFile;
            public string VacuumTimerDataFile;
            public string CalibrationDataFile;
            public string TeachingDataFile;
            public string StopCodeFile;
            public string UVLifeFile;
            public string UVCheckFile;
        }

        public class CModelDataFileNames
        {
            public string BaseDir;
            public string ModelName;
            public string PanelDataFile;
            public string FunctionDataFile;
            public string OffsetDataFile;
            public string TeachingDataFile;
        }

        /// <summary>
        /// MultiAxes가 이동시에 특정 축들이 먼저 안전한 위치로 이동 후에 이동해야할경우 (ex, ZAxis)
        /// 사용하는 안전한 위치의 값들을 관리하는 class
        /// </summary>
        public class CSystemData_MAxSafetyPos
        {
            public CPos_XYTZ Elevator_Pos;
            public CPos_XYTZ UHandler_Pos;
            public CPos_XYTZ LHandler_Pos;
        }

        public class CSystemData
        {
            // Axis, Cylinder, Vacuum 등의 class array는 별도의 class에서 처리하도록 한다.
            //
            public string ModelName = NAME_DEFAULT_MODEL;

            public ELanguage Language = ELanguage.KOREAN;

            // SafetyPos for Axis Move
            public CSystemData_MAxSafetyPos MAxSafetyPos = new CSystemData_MAxSafetyPos();

            /////////////////////////////////////////////////////////
            // 아래는 아직 미정리 내역들

            //
            public string PassWord;     // Engineer Password


            public int SystemType;      // 작업변
            public bool UseSafetySensor;
            public DEF_Thread.EAutoRunMode eOpModeStatus;
            public bool UseStepDisplay;
            public string LineControllerIP;
            public int LineControllerPort;
            public int MelsecChannelNo;
            public int MelsecStationNo;
            public int SystemLanguageSelect;
            public DEF_Common.EVelocityMode VelocityMode;
            public double PanelBacklash;
            public bool UseOnLineUse;

            public bool UseInSfaTest;                // SFA 내에서 Test할때 쓰임
            public bool UseDisplayQuitButton;

            // Vision
            public bool UseVisionDisplay;
            public double VisionCenter_Offset_X;
            public double VisionCenter_Offset_Y;
            public bool UseAutoSearch_Panel;
            public double AutoSearchDistance_Panel;
            //	BOOL	bAutoSearch_SubMark;

            public bool UseAlignUseSubMark;  // sub 마크로 Align 할지 여부

            // Stage
            /** Workbench Unit에 진출입할때의 안전한 Y Position **/
            public double Stage1LoadPos_Y;
            public double Stage1UnloadPos_Y;
            public double Stage2LoadPos_Y;
            public double Stage2UnloadPos_Y;
            public double Stage3LoadPos_Y;
            public double Stage3UnloadPos_Y;

            /** Stage의 충돌 방지 봉때문에 Workbench에 간섭을 안주는 안전한 위치의 한계를 정한다. **/
            public double Stage1PlusSafetyLimit_X;
            public double Stage1MinusSafetyLimit_X;
            public double Stage2PlusSafetyLimit_X;
            public double Stage2MinusSafetyLimit_X;
            public double Stage3PlusSafetyLimit_X;
            public double Stage3MinusSafetyLimit_X;

            /** Stage가 회전중심축을 기준으로 180도 턴 했을때 정대칭이 되기 위한 오차. **/
            public double Stage1_Turn180_Offset_X;
            public double Stage1_Turn180_Offset_Y;

            /** Stage가 Workbench와 충돌하지 않는 안전한 Z축위치 **/
            public double Stage1_ZAxis_SafetyUpPos;
            public double Stage1_ZAxis_SafetyDownPos;
            public double Stage2_ZAxis_SafetyUpPos;
            public double Stage2_ZAxis_SafetyDownPos;
            public double Stage3_ZAxis_SafetyUpPos;
            public double Stage3_ZAxis_SafetyDownPos;

            // Dispenser
            public int TrashIntervalTime;           // Auto 아닌 모드에서 Trash할때까지의 wait time
            public int TrashTime;                   // 동작중이 아닐때 Trash 시간
            public int DispenserSpeed;              // 도포 속도
            public int DispenserAccel;              // 도포 속도
            public bool UseDispenserTrashOption;         // 자동Run시 도포전 토출 여부
            public bool UseDispenserRunWaitTrashOption;  // 자동 Run중 대기동작시.. 자동 토출여부
            public int TrashPerPanelCount;          // 자동 Run중 일정 횟수 판넬 생산후 토출기능 수행여부

            public bool UseRunTime_Do_Dispense;              // Run시 도포건 토출 여부
            public bool UseRunTime_Do_Cure;                  // Run시 경화기 사용 여부

            // Head
            public double Workbench_XHalf_Size;
            public double Workbench_YHalf_Size;

            public double NeedlesInterGap;  ///** 도포건 각 Needle 사이의 물리적인 거리 */

            public double Workbench_YShipt;       // SHead1 기준, 현재는 센터라 의미없음
            public double Workbench_XShipt;       // GHead 기준

            public double AfterFailLaser_DispensingHeight;    // Laser값 읽기 실패시 몇미리 떠서 갈지 설정함

            //public bool UseHead_UseLaserSensor[DEF_MAX_HEAD_NO];     // Laser센서 읽기 여부
            public double InterUVLEDDistance; // UVLED각각 채널 사이의 거리
            public double UVLightLowerLimit;      // UVLED Light Value Lower Limit
            public int nUVCheckPerCount;       // 생산중 몇매당 할지..
            public int nPanelCountForUVCheck;  // UV Check 후 생산된 수

            // UVLamp
            public bool UseUVLampRunCheckLight;          // UVLamp의 Run시 광량 체크
            public double UVLampRunLight;             // Run시의 권장 광량
            public double UVLampRunLightLimit;            // Run시의 광량 하한값
            public double UVLampLightControlLimit;        // 권장 광량의 오차 범위
            public bool UseUVLampUseAutoControlLight;        // 자동 광량 조절

            public double UVLampMaxTime;                  // 허용 램프 최대 수명


            // Pumping Job
            public int DoPumpingIntervalTime;       // 펌핑 잡 Interval
            public int DoPumpingTime;               // 펌핑 잡 총 동작 시간
            public int Pumping_OneShot_Interval;        // 매 일회 펌핑 동작당 대기시간

            public bool UseUseWorkbenchVacuum;   // Workbench Vacuum 사용 유무


            // Dispenser측, MMC에서 제어하는 cylinder time
            public double Head_Cyl_MovingTime;
            public double Head_Cyl_AfterOnTime;
            public double Head_Cyl_AfterOffTime;
            public double Head_Cyl_NoSensorWaitTime;

            public double Head_Gun_UV_InterGap;       // from Needle to UV End distance

            public bool UseCheck_Panel_Data;         // run time, check panel data
            public bool UseCheck_Panel_History;      // run time, check panel id History

            public bool UseVIPMode;


            public CSystemData()
            {
            }
        }

        public class CSystemData_Axis
        {
            // YMC Motion Axis
            public CMPMotionData[] MPMotionData = new CMPMotionData[MAX_MP_AXIS];

            public CSystemData_Axis()
            {
                for (int i = 0; i < MPMotionData.Length; i++)
                {
                    MPMotionData[i] = new CMPMotionData();
                }
            }
        }

        public class CSystemData_Cylinder
        {

            // Timer
            public CCylinderTime[] CylinderTimer = new CCylinderTime[(int)EObjectCylinder.MAX];

            public CSystemData_Cylinder()
            {
                for (int i = 0; i < CylinderTimer.Length; i++)
                {
                    CylinderTimer[i] = new CCylinderTime();
                }
            }
        }

        public class CSystemData_Vacuum
        {
            // Timer
            public CVacuumTime[] VacuumTimer = new CVacuumTime[(int)EObjectVacuum.MAX];

            public CSystemData_Vacuum()
            {
                for (int i = 0; i < VacuumTimer.Length; i++)
                {
                    VacuumTimer[i] = new CVacuumTime();
                }
            }
        }

        public class CSystemData_Scanner
        {
            // Polygon Scanner Configure ini Data
            public CPolygonIni[] Scanner = new CPolygonIni[(int)EObjectScanner.MAX];

            public CSystemData_Scanner()
            {
                for (int i = 0; i < Scanner.Length; i++)
                {
                    Scanner[i] = new CPolygonIni();
                }
            }
        }

        public class CPositionData
        {
            // Stage1

            // PushPull
            public CUnitPos PushPullPos = new CUnitPos((int)EPushPullPos.MAX);
            public CUnitPos Centering1Pos = new CUnitPos((int)ECenteringPos.MAX);
            public CUnitPos Centering2Pos = new CUnitPos((int)ECenteringPos.MAX);

            // Elevator

            // MeHandler
            public CUnitPos UHandlerPos = new CUnitPos((int)EHandlerPos.MAX);
            public CUnitPos LHandlerPos = new CUnitPos((int)EHandlerPos.MAX);

            public CUnitPos RotatePos = new CUnitPos((int)ERotatePos.MAX);
            public CUnitPos CoaterPos = new CUnitPos((int)ENozzlePos.MAX);
            public CUnitPos CleanerPos = new CUnitPos((int)ENozzlePos.MAX);

            // Coater

            public CPositionData()
            {
            }
        }

        public class CProductData
        {
            public string Day_ModelName;
            public int Day_ModelProductQuantity;
            public string SW_ModelName;
            public int SW_ModelProductQuantity;
            public string GY_ModelName;
            public int GY_ModelProductQuantity;
            public int ProductQuantity_forOut;  // Out 되는 생산 수량
            public int ProductQuantity_forIn;       // In되는 생산 수량
        }

        public class CMotorParameter
        {
            public double CWSWLimit;
            public double CCWSWLimit;
            public double HomeFastVelocity;
            public double HomeSlowVelocity;
            public int HomeAccelerate;
            public double HomeOffset;
            public double JogPitch;
            public double JogVelocity;
            public double FastRunVelocity;
            public double RunVelocity;
            public double SlowRunVelocity;
            public int RunAccelerate;
            public double LimitTime;
            public double OriginLimitTime;
            public double StabilityTime;
            public double Tolerance;

        }

        public class CSimpleAxisTimer
        {
            public double TurningTime;
            public double SettlingTime;
            public double NoSenseMovingTime;
        }

        public class CCalibrationParameter
        {
            public int Move_Point_X;            // 2D Calibration을 하기 위한 X방향 이동 포인트 수
            public int Move_Point_Y;            // 2D Calibration을 하기 위한 Y방향 이동 포인트 수
            public double Move_Width_X;           // 2D Calibration을 하기 위한 X방향 이동 거리
            public double Move_Width_Y;           // 2D Calibration을 하기 위한 Y방향 이동 거리
            public bool UseComplete_Flag;            // 2D Calibration 수행 결과
            //public double PortingFactor[CALIB_FACTOR_NUMBER]; // 2D Calibration을 수행한 후 나온 Camera Factor 값

            public double FixedMoveX;         // 고정 좌표를 찾기 위해 첫번째 Mark 인식 후 이동할 X 방향 거리
            public double FixedMoveY;         // 고정 좌표를 찾기 위해 첫번째 Mark 인식 후 이동할 Y 방향 거리
            public double FixedMoveT;         // 고정 좌표를 찾기 위해 첫번째 Mark 인식 후 이동할 T 방향 거리

            public double FixedX;             // 계산된 고정 좌표 X 값
            public double FixedY;             // 계산된 고정 좌표 Y 값
        }

        public class CPanelMarkPos
        {
            public bool UseUseFlag;
            public double X;
            public double Y;
            public double Distance;
            public bool UseSubMarkUseFlag;
            public double Sub_Left_X;
            public double Sub_Right_X;
            public double Sub_Left_Y;
            public double Sub_Right_Y;
        }

        public class CWaferData
        {
            public double Size_X;
            public double Size_Y;
            public CPanelMarkPos FiduMarkXu = new CPanelMarkPos();
            //public SPanelMarkPos sFiduMarkXd;
            //public SPanelMarkPos sFiduMarkYl;
            //public SPanelMarkPos sFiduMarkYr;
            //public EInputDirection eInputDirection;
            //public EOutputDirection eOutputDirection;
            public double Thickness;
        }

        public class CFrameData
        {
            public string FrameName;
            public double Diameter;
            public int Slot;
            public int CassetteSetNo;
            public double FramePitch;
            public double CassetteHeight;
            public double UnloadElevatorPos;
            public double ESZeroPoint;
            public double CTZeroPoint;
            public double STZeroPoint;
            public double LoadPushPullPos;
            public double FrameCenterPos;
        }

        public class CInspectionFrameData
        {
            public string FrameName;
            public double StagePos;
            public double UnloadElevatorPos;
            public double LoadPushPullPos;
            public double UnloadPushPullPos;
        }

        public class CLogParameter
        {
            public bool UseLogLevelTactTime;
            public bool UseLogLevelNormal;
            public bool UseLogLevelWarning;
            public bool UseLogLevelError;
            public int LogKeepingDay;

        }

        // define root folder & default model name
        public const string NAME_ROOT_FOLDER = "root";
        public const string NAME_DEFAULT_MODEL = "default";

        /// <summary>
        /// Model의 계층구조를 만들기 위해서 Header만 따로 떼어서 관리.
        /// Folder인 경우엔 IsFolder = true & CModelData는 따로 만들지 않음.
        /// Model인 경우엔 IsFolder = false & CModelData에 같은 이름으로 ModelData가 존재함.
        /// </summary>
        public class CModelHeader
        {
            // Header
            public string Name;   // unique primary key
            public string Comment;
            public string Parent = NAME_ROOT_FOLDER; // if == "root", root
            public bool IsFolder = false; // true, if it is folder.
            public int TreeLevel = -1; // models = -1, root = 0, 1'st generation = 1, 2'nd generation = 2.. 3,4,5

            public void SetRootFolder()
            {
                Name = NAME_ROOT_FOLDER;
                Comment = "Root Folder";
                Parent = "";
                IsFolder = true;
                TreeLevel = 0;
            }

            public void SetDefaultModel()
            {
                Name = NAME_DEFAULT_MODEL;
                Comment = "Default Model";
                Parent = NAME_ROOT_FOLDER;
                IsFolder = false;
                TreeLevel = -1;
            }
        }

        public class CModelData    // Model, Recipe
        {
            ///////////////////////////////////////////////////////////
            // Header
            public string Name = NAME_DEFAULT_MODEL;   // unique primary key

            ///////////////////////////////////////////////////////////
            // Function Parameter

            // Mechanical Layer
            // MMeUpperHandler
            public bool[] MeUH_UseMainCylFlag = new bool[DEF_MAX_COORDINATE];
            public bool[] MeUH_UseSubCylFlag = new bool[DEF_MAX_COORDINATE];
            public bool[] MeUH_UseGuideCylFlag = new bool[DEF_MAX_COORDINATE];
            public bool[] MeUH_UseVccFlag = new bool[(int)EHandlerVacuum.MAX];

            // MMeLowerHandler
            public bool[] MeLH_UseMainCylFlag = new bool[DEF_MAX_COORDINATE];
            public bool[] MeLH_UseSubCylFlag = new bool[DEF_MAX_COORDINATE];
            public bool[] MeLH_UseGuideCylFlag = new bool[DEF_MAX_COORDINATE];
            public bool[] MeLH_UseVccFlag = new bool[(int)EHandlerVacuum.MAX];

            // Control Layer


            public bool Use2Step_Use;

            // Dispenser
            public int uDispensingTwiceTime;
            public int uDisepnsingSpeed;
            public int uUVLEDSpeed;

            public double ThicknessValue; // 판넬 두께

	        public bool UseUHandler_ExtraVccUseFlag; // 2014.02.21 by ranian. Extra Vcc 추가
            public bool UseUHandler_WaitPosUseFlag; // 2014.02.21 by ranian. LP->UP 로 갈 때, WP 사용 여부


            ///////////////////////////////////////////////////////////
            // Wafer Data
            public CWaferData Wafer = new CWaferData();

            // Frame Data
            public CFrameData[] Frame = new CFrameData[(int)EFrameDataNo.MAX];

            // Inspection Frame Data
            public CInspectionFrameData[] InspectionFrame = new CInspectionFrameData[(int)EFrameDataNo.MAX];

            // Spinner Data 
            public CSpinnerData SpinnerData = new CSpinnerData();

            // Wafer Image Line Data
            public LineData WaferLineData = new LineData();

            ///////////////////////////////////////////////////////////
            // Vision Data (Pattern)
            public CSearchData MacroPatternA = new CSearchData();
            public CSearchData MacroPatternB = new CSearchData();
            public CSearchData MicroPatternA = new CSearchData();
            public CSearchData MicroPatternB = new CSearchData();
        }

    }

    public class MDataManager : MObject
    {
        private CLoginData Login = new CLoginData();
        CDBInfo DBInfo;

        // System Data
        public CSystemData SystemData { get; private set; } = new CSystemData();
        public CModelData m_ModelData { get; set; } = new CModelData();

        public CSystemData_Axis SystemData_Axis { get; private set; } = new CSystemData_Axis();
        public CSystemData_Cylinder SystemData_Cylinder { get; private set; } = new CSystemData_Cylinder();
        public CSystemData_Vacuum SystemData_Vacuum { get; private set; } = new CSystemData_Vacuum();
        public CSystemData_Scanner SystemData_Scanner { get; private set; } = new CSystemData_Scanner();

        // Position Data
        public CPositionData FixedPos { get; private set; } = new CPositionData();
        public CPositionData ModelPos { get; private set; } = new CPositionData();
        public CPositionData OffsetPos { get; private set; } = new CPositionData();

        // Model Data
        public CModelData ModelData { get; private set; } = new CModelData();
        public List<CModelHeader> ModelHeaderList { get; set; } = new List<CModelHeader>();

        // Parameter Data
        public DEF_IO.CIOInfo[] InputArray { get; private set; } = new DEF_IO.CIOInfo[DEF_IO.MAX_IO_INPUT];
        public DEF_IO.CIOInfo[] OutputArray { get; private set; } = new DEF_IO.CIOInfo[DEF_IO.MAX_IO_OUTPUT];

        // Error Info는 필요할 때, 하나씩 불러와도 될 것 같은데, db test겸 초기화 편의성 때문에 임시로 만들어 둠
        public List<CAlarmInfo> AlarmInfoList { get; private set; } = new List<CAlarmInfo>();
        public List<CAlarm> AlarmHistory { get; private set; } = new List<CAlarm>();

        public List<CParaInfo> ParaInfoList { get; private set; } = new List<CParaInfo>();

        public MDataManager(CObjectInfo objInfo, CDBInfo dbInfo)
            : base(objInfo)
        {
            DBInfo = dbInfo;
            SetLogin(new CLoginData(), true);

            for(int i = 0; i < DEF_IO.MAX_IO_INPUT; i++)
            {
                InputArray[i] = new DEF_IO.CIOInfo(i+DEF_IO.INPUT_ORIGIN, DEF_IO.EIOType.DI);
            }
            for (int i = 0; i < DEF_IO.MAX_IO_OUTPUT; i++)
            {
                OutputArray[i] = new DEF_IO.CIOInfo(i+DEF_IO.OUTPUT_ORIGIN, DEF_IO.EIOType.DO);
            }

            TestFunction();

            LoadGeneralData();

            // 아래의 네가지 함수 콜은 LWDicer의 Initialize에서 읽어들이는게 맞지만, 생성자에서 한번 더 읽어도 되기에.. 주석처리해도 상관없음
            LoadSystemData();
            LoadPositionData(true);
            LoadModelList();
            MakeDefaultModel();
            ChangeModel(SystemData.ModelName);
        }

        public void TestFunction()
        {
            ///////////////////////////////////////
            if(false)
            {
                CModelHeader header = new CModelHeader();
                ModelHeaderList.Add(header);

                for (int i = 0; i < 3; i++)
                {
                    header = new CModelHeader();
                    header.Name = $"Model{i}";
                    header.Comment = $"Comment{i}";
                    header.Parent = $"Parent{i}";
                    header.IsFolder = false;
                    ModelHeaderList.Add(header);
                }

                SystemData_Cylinder.CylinderTimer[0].SettlingTime1 = 1;
                SystemData_Cylinder.CylinderTimer[1].SettlingTime2 = 2;
                SystemData.Head_Cyl_AfterOffTime = 3.456;
                SystemData.LineControllerIP = "122,333,444";

                SaveSystemData();
                //SaveModelList();
                //SaveModelData();
            }

            if (false)
            {
                for (int i = 0; i < 10; i++)
                {
                    int index = 3200 + i;
                    CAlarmInfo info = new CAlarmInfo(index);
                    info.Description[(int)DEF_Common.ELanguage.KOREAN] = $"{index}번 에러";
                    info.Solution[(int)DEF_Common.ELanguage.KOREAN] = $"{index}번 해결책";
                    AlarmInfoList.Add(info);
                }

                for (int i = 0; i < 10; i++)
                {
                    CParaInfo info = new CParaInfo("Test", "Name" + i.ToString());
                    info.Description[(int)DEF_Common.ELanguage.KOREAN] = $"Name{i} 변수";
                    ParaInfoList.Add(info);
                }

                SaveGeneralData();
            }

            ///////////////////////////////////////

            if (false)
            {
                Type type = typeof(CSystemData);
                Dictionary<string, string> fieldBook = ObjectExtensions.ToStringDictionary(SystemData, type);

                CSystemData systemData2 = new CSystemData();
                ObjectExtensions.FromStringDicionary(systemData2, type, fieldBook);
            }
        }

        public int BackupDB()
        {
            string[] dblist = new string[] { $"{DBInfo.DBConn}", $"{DBInfo.DBConn_Info}",
                $"{DBInfo.DBConn_DLog}", $"{DBInfo.DBConn_ELog}" };

            DateTime time = DateTime.Now;

            foreach(string source in dblist)
            {
                if (DBManager.BackupDB(source, time) == false)
                {
                    WriteLog("fail : backup db.", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_BACKUP_DB);
                }
            }

            WriteLog("success : backup db.", ELogType.Debug);
            return SUCCESS;
        }

        public int DeleteDB()
        {
            string[] dblist = new string[] { $"{DBInfo.DBConn}", $"{DBInfo.DBConn_Backup}",
                $"{DBInfo.DBConn_Info}", $"{DBInfo.DBConn_DLog}", $"{DBInfo.DBConn_ELog}" };

            foreach(string source in dblist)
            {
                if (DBManager.DeleteDB(source) == false)
                {
                    WriteLog("fail : delete db.", ELogType.Debug, ELogWType.Error);
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_DB);
                }
            }

            WriteLog("success : delete db.", ELogType.Debug);
            return SUCCESS;
        }

        public int SaveSystemData(CSystemData system = null, CSystemData_Axis systemAxis = null,
            CSystemData_Cylinder systemCylinder = null, CSystemData_Vacuum systemVacuum = null,
            CSystemData_Scanner systemScanner = null)
        {
            // CSystemData
            if (system != null)
            {
                try
                {
                    SystemData = ObjectExtensions.Copy(system);
                    string output = JsonConvert.SerializeObject(SystemData);

                    if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TableSystem, "name", nameof(CSystemData), output,
                        true, DBInfo.DBConn_Backup) != true)
                    {
                        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                    }
                    WriteLog("success : save CSystemData.", ELogType.SYSTEM, ELogWType.SAVE);
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                }
            }

            // CSystemData_Axis
            if (systemAxis != null)
            {
                try
                {
                    SystemData_Axis = ObjectExtensions.Copy(systemAxis);
                    string output = JsonConvert.SerializeObject(SystemData_Axis);

                    if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TableSystem, "name", nameof(CSystemData_Axis), output,
                        true, DBInfo.DBConn_Backup) != true)
                    {
                        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                    }
                    WriteLog("success : save CSystemData_Axis.", ELogType.SYSTEM, ELogWType.SAVE);
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                }
            }

            // CSystemData_Cylinder
            if (systemCylinder != null)
            {
                try
                {
                    SystemData_Cylinder = ObjectExtensions.Copy(systemCylinder);
                    string output = JsonConvert.SerializeObject(SystemData_Cylinder);

                    if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TableSystem, "name", nameof(CSystemData_Cylinder), output,
                        true, DBInfo.DBConn_Backup) != true)
                    {
                        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                    }
                    WriteLog("success : save CSystemData_Cylinder.", ELogType.SYSTEM, ELogWType.SAVE);
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                }
            }

            // CSystemData_Vacuum
            if (systemVacuum != null)
            {
                try
                {
                    SystemData_Vacuum = ObjectExtensions.Copy(systemVacuum);
                    string output = JsonConvert.SerializeObject(SystemData_Vacuum);

                    if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TableSystem, "name", nameof(CSystemData_Vacuum), output,
                        true, DBInfo.DBConn_Backup) != true)
                    {
                        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                    }
                    WriteLog("success : save CSystemData_Vacuum.", ELogType.SYSTEM, ELogWType.SAVE);
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                }
            }

            // CSystemData_Scanner
            if (systemScanner != null)
            {
                try
                {
                    SystemData_Scanner = ObjectExtensions.Copy(systemScanner);
                    string output = JsonConvert.SerializeObject(SystemData_Scanner);

                    if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TableSystem, "name", nameof(CSystemData_Scanner), output,
                        true, DBInfo.DBConn_Backup) != true)
                    {
                        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                    }
                    WriteLog("success : save CSystemData_Scanner.", ELogType.SYSTEM, ELogWType.SAVE);
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                }
            }
            return SUCCESS;
        }

        public int LoadSystemData(bool loadSystem = true, bool loadAxis = true, bool loadCylinder = true,
            bool loadVacuum = true, bool loadScanner = true)
        {
                string output;

            // CSystemData
            if (loadSystem == true)
            {
                try
                {
                    if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableSystem, out output, new CDBColumn("name", nameof(CSystemData))) == true)
                    {
                        CSystemData data = JsonConvert.DeserializeObject<CSystemData>(output);
                        SystemData = ObjectExtensions.Copy(data);
                        WriteLog("success : load CSystemData.", ELogType.SYSTEM, ELogWType.LOAD);
                    }
                    else
                    {
                        // temporarily do not return error for continuous loading
                        //return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);

                        // save default
                        SystemData = new CSystemData();
                        int iResult = SaveSystemData(SystemData);
                        if (iResult != SUCCESS) return iResult;
                    }
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                }
            }

            // CSystemData_Axis
            if (loadAxis == true)
            {
                try
                {
                    if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableSystem, out output, new CDBColumn("name", nameof(CSystemData_Axis))) == true)
                    {
                        CSystemData_Axis data = JsonConvert.DeserializeObject<CSystemData_Axis>(output);
                        if (SystemData_Axis.MPMotionData.Length == data.MPMotionData.Length)
                        {
                            SystemData_Axis = ObjectExtensions.Copy(data);
                        }
                        else
                        {
                            for (int i = 0; i < SystemData_Axis.MPMotionData.Length; i++)
                            {
                                if (i >= data.MPMotionData.Length) break;
                                SystemData_Axis.MPMotionData[i] = ObjectExtensions.Copy(data.MPMotionData[i]);
                            }
                        }
                        WriteLog("success : load CSystemData_Axis.", ELogType.SYSTEM, ELogWType.LOAD);
            }
                    //else // temporarily do not return error for continuous loading
                    //{
                    //    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                    //}
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                }

                // system data를 읽어왔는데, db에 이전 데이터가 저장되어 있지 않을 때, 필수적으로 초기화 해주어야 할 데이터들
                InitMPMotionData();
            }

            // CSystemData_Cylinder
            if (loadCylinder == true)
            {
                try
                {
                    if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableSystem, out output, new CDBColumn("name", nameof(CSystemData_Cylinder))) == true)
                    {
                        CSystemData_Cylinder data = JsonConvert.DeserializeObject<CSystemData_Cylinder>(output);
                        if(SystemData_Cylinder.CylinderTimer.Length == data.CylinderTimer.Length)
                        {
                            SystemData_Cylinder = ObjectExtensions.Copy(data);
                        }
                        else
                        {
                            for(int i = 0; i < SystemData_Cylinder.CylinderTimer.Length; i++)
                            {
                                if (i >= data.CylinderTimer.Length) break;
                                SystemData_Cylinder.CylinderTimer[i] = ObjectExtensions.Copy(data.CylinderTimer[i]);
                            }
                        }

                        WriteLog("success : load CSystemData_Cylinder.", ELogType.SYSTEM, ELogWType.LOAD);
                    }
                    //else // temporarily do not return error for continuous loading
                    //{
                    //    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                    //}
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                }
            }

            // CSystemData_Vacuum
            if (loadVacuum == true)
            {
                try
                {
                    if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableSystem, out output, new CDBColumn("name", nameof(CSystemData_Vacuum))) == true)
                    {
                        CSystemData_Vacuum data = JsonConvert.DeserializeObject<CSystemData_Vacuum>(output);
                        if (SystemData_Vacuum.VacuumTimer.Length == data.VacuumTimer.Length)
                        {
                            SystemData_Vacuum = ObjectExtensions.Copy(data);
                        }
                        else
                        {
                            for (int i = 0; i < SystemData_Vacuum.VacuumTimer.Length; i++)
                            {
                                if (i >= data.VacuumTimer.Length) break;
                                SystemData_Vacuum.VacuumTimer[i] = ObjectExtensions.Copy(data.VacuumTimer[i]);
                            }
                        }

                        WriteLog("success : load CSystemData_Vacuum.", ELogType.SYSTEM, ELogWType.LOAD);
                    }
                    //else // temporarily do not return error for continuous loading
                    //{
                    //    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                    //}
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                }
            }

            // CSystemData_Scanner
            if (loadScanner == true)
            {
                try
                {
                    if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableSystem, out output, new CDBColumn("name", nameof(CSystemData_Scanner))) == true)
                    {
                        CSystemData_Scanner data = JsonConvert.DeserializeObject<CSystemData_Scanner>(output);
                        if (SystemData_Scanner.Scanner.Length == data.Scanner.Length)
                        {
                            SystemData_Scanner = ObjectExtensions.Copy(data);
                        }
                        else
                        {
                            for (int i = 0; i < SystemData_Scanner.Scanner.Length; i++)
                            {
                                if (i >= data.Scanner.Length) break;
                                SystemData_Scanner.Scanner[i] = ObjectExtensions.Copy(data.Scanner[i]);
                            }
                        }
                        WriteLog("success : load CSystemData_Scanner.", ELogType.SYSTEM, ELogWType.LOAD);
                    }
                    //else // temporarily do not return error for continuous loading
                    //{
                    //    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                    //}
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                }
            }

            return SUCCESS;
        }

        public int SavePositionData(bool bLoadFixed, EPositionObject unit = EPositionObject.ALL)
        {
            string key_value;
            CPositionData tData = OffsetPos;
            string suffix = "_Offset_" + SystemData.ModelName;
            if (bLoadFixed)
            {
                tData = FixedPos;
                suffix = "_Fixed";
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.LOADER)
            {

            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.UHANDLER)
            {
                try
                {
                    key_value = EPositionObject.UHANDLER.ToString() + suffix;
                    string output = JsonConvert.SerializeObject(tData.UHandlerPos);

                    if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TablePos, "name", key_value, output,
                        true, DBInfo.DBConn_Backup) != true)
                    {
                        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_POSITION_DATA);
                    }
                    WriteLog($"success : save {key_value} Position.", ELogType.SYSTEM, ELogWType.SAVE);
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_POSITION_DATA);
                }
            }


            return SUCCESS;
        }

        public int LoadPositionData(bool bLoadFixed, EPositionObject unit = EPositionObject.ALL)
        {
            string output;

            string key_value;
            CPositionData tData = OffsetPos;
            string suffix = "_Offset_" + SystemData.ModelName;
            if (bLoadFixed)
            {
                tData = FixedPos;
                suffix = "_Fixed";
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.LOADER)
            {

            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.UHANDLER)
            {
                try
                {
                    key_value = EPositionObject.UHANDLER.ToString() + suffix;
                    if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TablePos, out output, new CDBColumn("name", key_value)) == true)
                    {
                        CUnitPos data = JsonConvert.DeserializeObject<CUnitPos>(output);
                        tData.UHandlerPos = ObjectExtensions.Copy(data);
                        WriteLog($"success : load {key_value} Position.", ELogType.SYSTEM, ELogWType.LOAD);
                    }
                    //else // temporarily do not return error for continuous loading
                    //{
                    //    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_POSITION_DATA);
                    //}
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_POSITION_DATA);
                }

            }

            return SUCCESS;
        }

        /// <summary>
        /// Model(Panel, Wafer)의 크기에 따라 자동으로 모델 좌표를 생성시켜준다.
        /// </summary>
        /// <returns></returns>
        public int GenerateModelPosition()
        {

            return SUCCESS;
        }

        /// <summary>
        /// UI에서 public 으로 선언된 ModelHeaderList를 편집한 후에 (data 무결성은 UI에서 책임)
        /// 이 함수를 호출하여 ModelHeader List를 저장한다
        /// </summary>
        /// <returns></returns>
        public int SaveModelHeaderList()
        {
            try
            {
                List<string> querys = new List<string>();
                string query;

                // 0. create table
                query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableModelHeader} (name string primary key, data string)";
                querys.Add(query);

                // 1. delete all
                query = $"DELETE FROM {DBInfo.TableModelHeader}";
                querys.Add(query);

                // 2. save model list
                string output;
                foreach (CModelHeader header in ModelHeaderList)
                {
                    output = JsonConvert.SerializeObject(header);
                    query = $"INSERT INTO {DBInfo.TableModelHeader} VALUES ('{header.Name}', '{output}')";
                    querys.Add(query);
                }

                // 3. execute query
                if (DBManager.ExecuteNonQuerys(DBInfo.DBConn, querys) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MODEL_LIST);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MODEL_LIST);
            }

            WriteLog($"success : save model list", ELogType.Debug);
            return SUCCESS;
        }

        public int MakeDefaultModel()
        {
            int iResult;
            bool bStatus = true;

            // make root folder
            if(IsModelHeaderExist(NAME_ROOT_FOLDER) == false)
            {
                CModelHeader header = new CModelHeader();
                header.SetRootFolder();
                ModelHeaderList.Add(header);
                iResult = SaveModelHeaderList();
                if (iResult != SUCCESS) return iResult;
            }

            // make default model
            if (IsModelHeaderExist(NAME_DEFAULT_MODEL) == false)
            {
                CModelHeader header = new CModelHeader();
                header.SetDefaultModel();
                ModelHeaderList.Add(header);
                iResult = SaveModelHeaderList();
                if (iResult != SUCCESS) return iResult;
            }
            if (IsModelExist(NAME_DEFAULT_MODEL) == false)
            {
                CModelData model = new CModelData();
                iResult = SaveModelData(model);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int LoadModelList()
        {
            try
            {
                string query;

                // 0. select table
                query = $"SELECT * FROM {DBInfo.TableModelHeader}";

                // 1. get table
                DataTable datatable;
                if (DBManager.GetTable(DBInfo.DBConn, query, out datatable) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST);
                }

                // 2. delete list
                ModelHeaderList.Clear();

                // 3. get list
                foreach (DataRow row in datatable.Rows)
                {
                    string output = row["data"].ToString();
                    CModelHeader header = JsonConvert.DeserializeObject<CModelHeader>(output);
                    ModelHeaderList.Add(header);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST);
            }

            WriteLog($"success : load model list", ELogType.Debug);
            return SUCCESS;
        }

        bool IsModelHeaderExist(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            foreach (CModelHeader header in ModelHeaderList)
            {
                if(header.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsModelExist(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            try
            {
                // 1. load model
                string output;
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableModel, out output, new CDBColumn("name", name)) == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return false;
            }

            return false;
        }

        bool IsModelFolder(string name)
        {
            foreach (CModelHeader header in ModelHeaderList)
            {
                if (header.Name == name)
                {
                    return header.IsFolder;
                }
            }
            return false;
        }

        int GetModelTreeLevel(string name)
        {
            foreach (CModelHeader header in ModelHeaderList)
            {
                if (header.Name == name)
                {
                    return header.TreeLevel;
                }
            }
            return 0;
        }

        public int DeleteModelHeader(string name)
        {
            if (name == NAME_ROOT_FOLDER) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_ROOT_FOLDER);
            if (name == NAME_DEFAULT_MODEL) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_DEFAULT_MODEL);
            if (IsModelHeaderExist(name) == false) return SUCCESS;

            int index = 0;
            foreach (CModelHeader header in ModelHeaderList)
            {
                if (header.Name == name)
                {
                    ModelHeaderList.RemoveAt(index);
                    break;
                }
                index++;
            }

            int iResult = SaveModelHeaderList();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int DeleteModelData(string name)
        {
            if (name == NAME_DEFAULT_MODEL) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_DEFAULT_MODEL);
            if (IsModelExist(name) == false) return SUCCESS;

            // cannot delete current model
            if (name == SystemData.ModelName) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_DEFAULT_MODEL);

            try
            {
                if (DBManager.DeleteRow(DBInfo.DBConn, DBInfo.TableModel, "name", ModelData.Name, 
                    true, DBInfo.DBConn_Backup) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_MODEL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_MODEL_DATA);
            }

            WriteLog($"success : delete model [{name}].", ELogType.SYSTEM, ELogWType.SAVE);
            return SUCCESS;
        }

        /// <summary>
        /// Model Data 변경시에 저장 
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public int SaveModelData(CModelData modelData)
        {
            try
            {
                ModelData = ObjectExtensions.Copy(modelData);
                string output = JsonConvert.SerializeObject(ModelData);

                if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TableModel, "name", ModelData.Name, output,
                    true, DBInfo.DBConn_Backup) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MODEL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MODEL_DATA);
            }

            WriteLog($"success : save model [{modelData.Name}].", ELogType.SYSTEM, ELogWType.SAVE);
            return SUCCESS;
        }

        public int ChangeModel(string name)
        {
            int iResult;
            // 0. check exist
            if(string.IsNullOrEmpty(name))
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }
            if(IsModelExist(name) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            CModelData modelData = null;
            try
            {
                // 1. load model
                string output;
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableModel, out output, new CDBColumn("name", name)) == true)
                {
                    modelData = JsonConvert.DeserializeObject<CModelData>(output);
                }
                else
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
                }

                // 2. save system data
                string prev_model = SystemData.ModelName;
                SystemData.ModelName = name;
                iResult = SaveSystemData(SystemData);
                if(iResult != SUCCESS)
                {
                    SystemData.ModelName = prev_model;
                    return iResult;
                }
                
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            // 2. finally, set model data
            if(modelData != null)
            {
                ModelData = modelData;
                WriteLog($"success : change model [{ModelData.Name}].", ELogType.SYSTEM, ELogWType.LOAD);
            }

            // 3. load model offset position
            iResult = LoadPositionData(false);
            if (iResult != SUCCESS) return iResult;

            // 4. generate model position
            iResult = GenerateModelPosition();
            if (iResult != SUCCESS) return iResult;

            // 5. make model folder
            System.IO.Directory.CreateDirectory(DBInfo.ModelDir+$"\\{name}");

            return SUCCESS;
        }

        public int ViewModelData(string name, out CModelData modelData)
        {
            modelData = new CModelData();
            // 0. check exist
            if (IsModelExist(name) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            try
            {
                // 1. load model
                string output;
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableModel, out output, new CDBColumn("name", name)) == true)
                {
                    modelData = JsonConvert.DeserializeObject<CModelData>(output);
                }
                else
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            return SUCCESS;
        }

        public CLoginData GetLogin()
        {
            return Login;
        }

        public int SetLogin(CLoginData login, bool IsSystemStart = false)
        {
            if(IsSystemStart == false)
            {
                // Type과 사번이 같다면 동일 인물로 본다.
                if (login.Type == Login.Type && login.Number == Login.Number)
                {
                    return SUCCESS;
                }
            }

            // write login history
            string create_query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableLoginHistory} (logintime datetime, type string, number string, name string)";
            string query = $"INSERT INTO {DBInfo.TableLoginHistory} VALUES ('{DBManager.DateTimeSQLite(login.LoginTime)}', '{login.Type}', '{login.Number}', '{login.Name}')";

            if (DBManager.ExecuteNonQuerys(DBInfo.DBConn_ELog, create_query, query) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_LOGIN_HISTORY);
            }

            Login = login;
            DBManager.SetOperator(Login.Number, Login.Type.ToString());
            WriteLog($"login : {login}", ELogType.LOGIN, ELogWType.LOGIN);

            return SUCCESS;
        }

        //public int SaveAlarmHistory(CAlarm alarm, CAlarmInfo info)
        //{
        //    // write error history
        //    string create_query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableAlarmHistory} (occurtime datetime, resettime datetime, alarm_key string, alarm_text string, alarm_info string)";
        //    string query = $"INSERT INTO {DBInfo.TableAlarmHistory} VALUES ('{DBManager.DateTimeSQLite(alarm.OccurTime)}', '{DBManager.DateTimeSQLite(alarm.ResetTime)}', '{alarm.GetIndex()}', '{alarm}', '{info.GetText()}')";

        //    if (DBManager.ExecuteNonQuerys(DBInfo.DBConn_ELog, create_query, query) == false)
        //    {
        //        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_ALARM_HISTORY);
        //    }

        //    return SUCCESS;
        //}

        public int SaveAlarmHistory(CAlarm alarm)
        {
            try
            {
                List<string> querys = new List<string>();

                // 0. create table
                string create_query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableAlarmHistory} (occurtime datetime, resettime datetime, alarm_key string, data string)";
                querys.Add(create_query);

                // 1. delete all

                // 2. save list
                string output = JsonConvert.SerializeObject(alarm);
                string query = $"INSERT INTO {DBInfo.TableAlarmHistory} VALUES ('{DBManager.DateTimeSQLite(alarm.OccurTime)}', '{DBManager.DateTimeSQLite(alarm.ResetTime)}', '{alarm.GetIndex()}', '{output}')";
                querys.Add(query);

                // 3. execute query
                if (DBManager.ExecuteNonQuerys(DBInfo.DBConn_ELog, querys) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_ALARM_HISTORY);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_ALARM_HISTORY);
            }

            WriteLog($"success : save alarm history", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadAlarmHistory()
        {
            try
            {
                string query;

                // 0. select table
                query = $"SELECT * FROM {DBInfo.TableAlarmHistory}";

                // 1. get table
                DataTable datatable;
                if (DBManager.GetTable(DBInfo.DBConn_ELog, query, out datatable) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_ALARM_HISTORY);
                }

                // 2. delete list
                AlarmHistory.Clear();

                // 3. get list
                foreach (DataRow row in datatable.Rows)
                {
                    string output = row["data"].ToString();
                    CAlarm alarm = JsonConvert.DeserializeObject<CAlarm>(output);

                    AlarmHistory.Add(alarm);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_ALARM_HISTORY);
            }

            WriteLog($"success : load alarm history", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadGeneralData()
        {
            int iResult;

            LoadSystemParaExcelFile();

            iResult = LoadIOList();
            //if (iResult != SUCCESS) return iResult;

            iResult = LoadAlarmInfoList();
            //if (iResult != SUCCESS) return iResult;

            iResult = LoadParameterList();

            return SUCCESS;
        }
        public int LoadIOList()
        { 
            try
            {
                string query;

                // 0. select table
                query = $"SELECT * FROM {DBInfo.TableIO}";

                // 1. get table
                DataTable datatable;
                if (DBManager.GetTable(DBInfo.DBConn_Info, query, out datatable) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA);
                }

                // 2. delete list

                // 3. get list
                foreach (DataRow row in datatable.Rows)
                {
                    int index;
                    if(int.TryParse(row["name"].ToString(), out index))
                    {
                        string output = row["data"].ToString();
                        DEF_IO.CIOInfo ioInfo = JsonConvert.DeserializeObject<DEF_IO.CIOInfo>(output);
                        
                        if(index >= DEF_IO.INPUT_ORIGIN && index < DEF_IO.INPUT_ORIGIN)
                        {
                            InputArray[index - DEF_IO.INPUT_ORIGIN] = ioInfo;
                        } else if (index >= DEF_IO.OUTPUT_ORIGIN && index < DEF_IO.OUTPUT_END)
                        {
                            OutputArray[index - DEF_IO.OUTPUT_ORIGIN] = ioInfo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA);
            }

            WriteLog($"success : load io list", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadAlarmInfoList()
        {
            try
            {
                string query;

                // 0. select table
                query = $"SELECT * FROM {DBInfo.TableAlarmInfo}";

                // 1. get table
                DataTable datatable;
                if (DBManager.GetTable(DBInfo.DBConn_Info, query, out datatable) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_ALARM_INFO);
                }

                // 2. delete list
                AlarmInfoList.Clear();

                // 3. get list
                foreach (DataRow row in datatable.Rows)
                {
                    int index;
                    if (int.TryParse(row["name"].ToString(), out index))
                    {
                        string output = row["data"].ToString();
                        DEF_Error.CAlarmInfo info = JsonConvert.DeserializeObject<DEF_Error.CAlarmInfo>(output);

                        AlarmInfoList.Add(info);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_ALARM_INFO);
            }

            WriteLog($"success : load alarm info list", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadAlarmInfo(int index, out CAlarmInfo info)
        {
            info = new CAlarmInfo();
            if(AlarmInfoList.Count > 0)
            {
                foreach(CAlarmInfo item in AlarmInfoList)
                {
                    if(item.Index == index)
                    {
                        info = ObjectExtensions.Copy(item);
                        return SUCCESS;
                    }
                }
            }

            try
            {
                string output;

                // select row
                if (DBManager.SelectRow(DBInfo.DBConn_Info, DBInfo.TableAlarmInfo, out output, new CDBColumn("name", index.ToString())) == true)
                {
                    info = JsonConvert.DeserializeObject<CAlarmInfo>(output);
                }
                else
                {
                    WriteLog($"fail : load alarm info [index = {index}]", ELogType.Debug);
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_ALARM_INFO);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_ALARM_INFO);
            }

            WriteLog($"success : load alarm info", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadParameterList()
        {
            try
            {
                string query;

                // 0. select table
                query = $"SELECT * FROM {DBInfo.TableParameter}";

                // 1. get table
                DataTable datatable;
                if (DBManager.GetTable(DBInfo.DBConn_Info, query, out datatable) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA);
                }

                // 2. delete list
                ParaInfoList.Clear();

                // 3. get list
                foreach (DataRow row in datatable.Rows)
                {
                    string output = row["data"].ToString();
                    DEF_Common.CParaInfo info = JsonConvert.DeserializeObject<DEF_Common.CParaInfo>(output);

                    //// 저장할 때, Group + "__" + Name 형태로 저장하기 때문에, desirialize시에 "__" + Group + "__" + Name 환원되는 문제 해결
                    //if(info.Name.Length >= 2 && info.Name.Substring(0, 2) == "__")
                    //{
                    //    info.Name = info.Name.Remove(0, 2);
                    //}

                    ParaInfoList.Add(info);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA);
            }

            WriteLog($"success : load para info list", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadParaInfo(string group, string name, out CParaInfo info)
        {
            info = new CParaInfo(group, name);
            if(ParaInfoList.Count > 0)
            {
                foreach(CParaInfo item in ParaInfoList)
                {
                    if(item.Group == group && item.Name == name)
                    {
                        info = ObjectExtensions.Copy(item);
                        return SUCCESS;
                    }
                }
            }

            try
            {
                string output;

                // select row
                if (DBManager.SelectRow(DBInfo.DBConn_Info, DBInfo.TableParameter, out output, new CDBColumn("pgroup", info.Group), new CDBColumn("name", info.Name)) == true)
                {
                    info = JsonConvert.DeserializeObject<CParaInfo>(output);
                }
                else
                {
                    WriteLog($"fail : load para info [group = {info.Group}, name = {info.Name}]", ELogType.Debug);
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA);
            }

            WriteLog($"success : load para info", ELogType.Debug);
            return SUCCESS;
        }

        public int SaveGeneralData()
        {
            int iResult;

            iResult = SaveIOList();
            //if (iResult != SUCCESS) return iResult;

            iResult = SaveAlarmInfoList();
            //if (iResult != SUCCESS) return iResult;

            iResult = SaveParaInfoList();

            return SUCCESS;
        }

        public int SaveIOList()
        {
            try
            {
                List<string> querys = new List<string>();
                string query;

                // 0. create table
                query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableIO} (name string primary key, data string)";
                querys.Add(query);

                // 1. delete all
                query = $"DELETE FROM {DBInfo.TableIO}";
                querys.Add(query);

                // 2. save list
                string output;
                for (int i = 0; i < DEF_IO.MAX_IO_INPUT; i++)
                {
                    output = JsonConvert.SerializeObject(InputArray[i]);
                    query = $"INSERT INTO {DBInfo.TableIO} VALUES ('{i+DEF_IO.INPUT_ORIGIN}', '{output}')";
                    querys.Add(query);
                }
                for (int i = 0; i < DEF_IO.MAX_IO_OUTPUT; i++)
                {
                    output = JsonConvert.SerializeObject(OutputArray[i]);
                    query = $"INSERT INTO {DBInfo.TableIO} VALUES ('{i + DEF_IO.OUTPUT_ORIGIN}', '{output}')";
                    querys.Add(query);
                }

                // 3. execute query
                if (DBManager.ExecuteNonQuerys(DBInfo.DBConn_Info, querys) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
            }

            WriteLog($"success : save io list", ELogType.Debug);
            return SUCCESS;
        }

        public int SaveAlarmInfoList()
        {
            try
            {
                List<string> querys = new List<string>();
                string query;

                // 0. create table
                query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableAlarmInfo} (name string primary key, data string)";
                querys.Add(query);

                // 1. delete all
                query = $"DELETE FROM {DBInfo.TableAlarmInfo}";
                querys.Add(query);

                // 2. save list
                string output;
                foreach (CAlarmInfo info in AlarmInfoList)
                {
                    output = JsonConvert.SerializeObject(info);
                    query = $"INSERT INTO {DBInfo.TableAlarmInfo} VALUES ('{info.Index}', '{output}')";
                    querys.Add(query);
                }

                // 3. execute query
                if (DBManager.ExecuteNonQuerys(DBInfo.DBConn_Info, querys) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
            }

            WriteLog($"success : save error info list", ELogType.Debug);
            return SUCCESS;
        }

        public int SaveParaInfoList()
        {
            try
            {
                List<string> querys = new List<string>();
                string query;

                // 0. create table
                query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableParameter} (pgroup string, name string, data string)";
                querys.Add(query);

                // 1. delete all
                query = $"DELETE FROM {DBInfo.TableParameter}";
                querys.Add(query);

                // 2. save list
                string output;
                foreach (CParaInfo info in ParaInfoList)
                {
                    output = JsonConvert.SerializeObject(info);
                    query = $"INSERT INTO {DBInfo.TableParameter} VALUES ('{info.Group}', '{info.Name}', '{output}')";
                    querys.Add(query);
                }

                // 3. execute query
                if (DBManager.ExecuteNonQuerys(DBInfo.DBConn_Info, querys) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
            }

            WriteLog($"success : save para info list", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadSystemParaExcelFile()
        {
            int i = 0, j = 0, nSheetCount = 0, nCount = 0;

            string strPath = DBInfo.SystemDir + DBInfo.ExcelSystemPara;

            try
            {
                Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbook WorkBook = ExcelApp.Workbooks.Open(strPath);

                // 1. Open 한 Excel File에 Sheet Count
                nSheetCount = WorkBook.Worksheets.Count;

                // 2. Excel Sheet Row, Column 접근을 위한 Range 생성
                Excel.Range[] SheetRange = new Excel.Range[nSheetCount];

                // 3. Excel Sheet Item Data 획득을 위한 Worksheet 생성
                Excel.Worksheet[] Sheet = new Excel.Worksheet[nSheetCount];

                // 4. Excel Sheet 정보를 불러 온다.
                for (i = 0; i < nSheetCount; i++)
                {
                    Sheet[i] = (Excel.Worksheet)WorkBook.Worksheets.get_Item(i + 1);
                    SheetRange[i] = Sheet[i].UsedRange;
                }

                // Parameter Info
                LoadParaInfoFromExcel(SheetRange[(int)EExcel_Sheet.PARA_Info]);

                // Alarm Info
                LoadAlarmInfoFromExcel(SheetRange[(int)EExcel_Sheet.Alarm_Info]);

                // IO 
                LoadExcelIOInfo(SheetRange[(int)EExcel_Sheet.IO_Info]);

                // Motor Data
                LoadMotorDataToExcel(SheetRange[(int)EExcel_Sheet.Motor_Data]);

                WorkBook.Close(true);
                ExcelApp.Quit();
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_IO_EXCEL_FILE_READ_FAIL);
            }

            WriteLog($"success : System Parameter Read Completed", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadExcelIOInfo(Excel.Range SheetRange)
        {
            int nCount = 0 , i = 0;

            try
            {
                for (i = 0; i < 16; i++)
                {
                    InputArray[nCount].Name[0] = (string)(SheetRange.Cells[i + 2, 2] as Excel.Range).Value2;
                    OutputArray[nCount].Name[0] = (string)(SheetRange.Cells[i + 2, 4] as Excel.Range).Value2;

                    InputArray[nCount + 16].Name[0] = (string)(SheetRange.Cells[i + 2, 7] as Excel.Range).Value2;
                    OutputArray[nCount + 16].Name[0] = (string)(SheetRange.Cells[i + 2, 9] as Excel.Range).Value2;

                    InputArray[nCount + 32].Name[0] = (string)(SheetRange.Cells[i + 2, 12] as Excel.Range).Value2;
                    OutputArray[nCount + 32].Name[0] = (string)(SheetRange.Cells[i + 2, 14] as Excel.Range).Value2;

                    InputArray[nCount + 48].Name[0] = (string)(SheetRange.Cells[i + 2, 17] as Excel.Range).Value2;
                    OutputArray[nCount + 48].Name[0] = (string)(SheetRange.Cells[i + 2, 19] as Excel.Range).Value2;

                    nCount++;
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_IO_EXCEL_FILE_READ_FAIL);
            }

            WriteLog($"success : IO Excel File Read Completed", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadMotorDataToExcel(Excel.Range SheetRange)
        {
            int i = 0;

            try
            {
                // Motor Data Sheet
                for (i = 0; i < 19; i++)
                {
                    // Speed
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 3] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 4] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 5] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 6] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 7] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 8] as Excel.Range).Text);

                    // Acc
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 9] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 10] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 11] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 12] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 13] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 14] as Excel.Range).Text);

                    // Dec
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 15] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 16] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 17] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 18] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 19] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 20] as Excel.Range).Text);

                    // S/W Limit
                    SystemData_Axis.MPMotionData[i].PosLimit.Plus = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 21] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].PosLimit.Minus = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 22] as Excel.Range).Text);

                    // Limit Time
                    SystemData_Axis.MPMotionData[i].TimeLimit.tMoveLimit = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 23] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].TimeLimit.tSleepAfterMove = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 24] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].TimeLimit.tOriginLimit = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 25] as Excel.Range).Text);

                    // Home Option
                    SystemData_Axis.MPMotionData[i].OriginData.Method = Convert.ToInt16((string)(SheetRange.Cells[i + 2, 26] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].OriginData.Dir = Convert.ToInt16((string)(SheetRange.Cells[i + 2, 27] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].OriginData.FastSpeed = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 28] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].OriginData.SlowSpeed = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 29] as Excel.Range).Text);
                    SystemData_Axis.MPMotionData[i].OriginData.HomeOffset = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 30] as Excel.Range).Text);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_SYSTEM_EXCEL_FILE_READ_FAIL);
            }

            WriteLog($"success : Load Motor Data", ELogType.Debug);
            return SUCCESS;

        }

        public int LoadAlarmInfoFromExcel(Excel.Range SheetRange)
        {
            int nRowCount = SheetRange.EntireRow.Count;
            int i = 0;

            CAlarmInfo AlarmInfo = new CAlarmInfo();

            AlarmInfoList.Clear();

            for (i = 0; i < nRowCount - 1; i++)
            {
                AlarmInfo.Index = (int)(SheetRange.Cells[i + 2, 1] as Excel.Range).Value2; // Index

                switch (((int)(SheetRange.Cells[i + 2, 2] as Excel.Range).Value2)) // Type
                {
                    case (int)EErrorType.E1:
                        AlarmInfo.Type = EErrorType.E1;
                        break;

                    case (int)EErrorType.E2:
                        AlarmInfo.Type = EErrorType.E2;
                        break;

                    case (int)EErrorType.E3:
                        AlarmInfo.Type = EErrorType.E3;
                        break;
                }

                AlarmInfo.Description[(int)DEF_Common.ELanguage.KOREAN] = (string)(SheetRange.Cells[i + 2, 3] as Excel.Range).Value2; // Description Kor
                AlarmInfo.Description[(int)DEF_Common.ELanguage.ENGLISH] = (string)(SheetRange.Cells[i + 2, 4] as Excel.Range).Value2; // Description Eng
                AlarmInfo.Description[(int)DEF_Common.ELanguage.CHINESE] = (string)(SheetRange.Cells[i + 2, 5] as Excel.Range).Value2; // Description Chn
                AlarmInfo.Description[(int)DEF_Common.ELanguage.JAPANESE] = (string)(SheetRange.Cells[i + 2, 6] as Excel.Range).Value2; // Description Jpn

                AlarmInfo.Solution[(int)DEF_Common.ELanguage.KOREAN] = (string)(SheetRange.Cells[i + 2, 7] as Excel.Range).Value2; // Solution Kor
                AlarmInfo.Solution[(int)DEF_Common.ELanguage.ENGLISH] = (string)(SheetRange.Cells[i + 2, 8] as Excel.Range).Value2; // Solution Eng
                AlarmInfo.Solution[(int)DEF_Common.ELanguage.CHINESE] = (string)(SheetRange.Cells[i + 2, 9] as Excel.Range).Value2; // Solution Chn
                AlarmInfo.Solution[(int)DEF_Common.ELanguage.JAPANESE] = (string)(SheetRange.Cells[i + 2, 10] as Excel.Range).Value2; // Solution Jpn

                AlarmInfoList.Add(AlarmInfo);
            }

            SaveAlarmInfoList();

            return SUCCESS;
        }

        public int LoadParaInfoFromExcel(Excel.Range SheetRange)
        {
            int nRowCount = SheetRange.EntireRow.Count;
            int i = 0, Type = 0;

            CParaInfo ParaInfo = new CParaInfo();

            ParaInfoList.Clear();

            for (i = 0; i < nRowCount - 1; i++)
            {
                ParaInfo.Group = (string)(SheetRange.Cells[i + 2, 1] as Excel.Range).Value2; // Group
                ParaInfo.Name = (string)(SheetRange.Cells[i + 2, 2] as Excel.Range).Value2; // Name

                ParaInfo.Unit = (string)(SheetRange.Cells[i + 2, 3] as Excel.Range).Value2; // Unit

                ParaInfo.Type = (EUnitType)(SheetRange.Cells[i + 2, 4] as Excel.Range).Value2; // Type

                ParaInfo.DisplayName[(int)DEF_Common.ELanguage.KOREAN] = (string)(SheetRange.Cells[i + 2, 5] as Excel.Range).Value2;
                ParaInfo.DisplayName[(int)DEF_Common.ELanguage.ENGLISH] = (string)(SheetRange.Cells[i + 2, 6] as Excel.Range).Value2;
                ParaInfo.DisplayName[(int)DEF_Common.ELanguage.CHINESE] = (string)(SheetRange.Cells[i + 2, 7] as Excel.Range).Value2;
                ParaInfo.DisplayName[(int)DEF_Common.ELanguage.JAPANESE] = (string)(SheetRange.Cells[i + 2, 8] as Excel.Range).Value2;

                ParaInfo.Description[(int)DEF_Common.ELanguage.KOREAN] = (string)(SheetRange.Cells[i + 2, 9] as Excel.Range).Value2;
                ParaInfo.Description[(int)DEF_Common.ELanguage.ENGLISH] = (string)(SheetRange.Cells[i + 2, 10] as Excel.Range).Value2;
                ParaInfo.Description[(int)DEF_Common.ELanguage.CHINESE] = (string)(SheetRange.Cells[i + 2, 11] as Excel.Range).Value2;
                ParaInfo.Description[(int)DEF_Common.ELanguage.JAPANESE] = (string)(SheetRange.Cells[i + 2, 12] as Excel.Range).Value2;

                ParaInfoList.Add(ParaInfo);
            }

            return SUCCESS;
        }

        public int SaveAlarmInfoToExcel()
        {
            int i = 0, nSheetCount = 0, nCount = 0;

            string strPath = DBInfo.SystemDir + DBInfo.ExcelSystemPara;

            try
            {
                Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbook WorkBook = ExcelApp.Workbooks.Open(strPath);

                // 1. Open 한 Excel File에 Sheet Count
                nSheetCount = WorkBook.Worksheets.Count;

                // 2. Excel Sheet Row, Column 접근을 위한 Range 생성
                Excel.Range[] SheetRange = new Excel.Range[nSheetCount];

                // 3. Excel Sheet Item Data 획득을 위한 Worksheet 생성
                Excel.Worksheet[] Sheet = new Excel.Worksheet[nSheetCount];

                // 4. Excel Sheet 정보를 불러 온다.
                for (i = 0; i < nSheetCount; i++)
                {
                    Sheet[i] = (Excel.Worksheet)WorkBook.Worksheets.get_Item(i + 1);
                    SheetRange[i] = Sheet[i].UsedRange;
                }

                nCount = AlarmInfoList.Count;

                for (i=0;i<nCount;i++)
                {
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 1] as Excel.Range).Value2 = AlarmInfoList[i].Index;

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 2] as Excel.Range).Value2 = AlarmInfoList[i].Type;

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 3] as Excel.Range).Value2 = AlarmInfoList[i].Description[(int)DEF_Common.ELanguage.KOREAN];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 4] as Excel.Range).Value2 = AlarmInfoList[i].Description[(int)DEF_Common.ELanguage.ENGLISH];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 5] as Excel.Range).Value2 = AlarmInfoList[i].Description[(int)DEF_Common.ELanguage.CHINESE];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 6] as Excel.Range).Value2 = AlarmInfoList[i].Description[(int)DEF_Common.ELanguage.JAPANESE];

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 7] as Excel.Range).Value2 = AlarmInfoList[i].Solution[(int)DEF_Common.ELanguage.KOREAN];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 8] as Excel.Range).Value2 = AlarmInfoList[i].Solution[(int)DEF_Common.ELanguage.ENGLISH];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 9] as Excel.Range).Value2 = AlarmInfoList[i].Solution[(int)DEF_Common.ELanguage.CHINESE];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 10] as Excel.Range).Value2 = AlarmInfoList[i].Solution[(int)DEF_Common.ELanguage.JAPANESE];
                }

                WorkBook.Save();
                WorkBook.Close(true);
                ExcelApp.Quit();

            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_SYSTEM_EXCEL_FILE_READ_FAIL);
            }

            WriteLog($"success : Save Alarm Info", ELogType.Debug);

            return SUCCESS;
        }

        public int SaveParaInfoToExcel()
        {
            int i = 0, j = 0, nSheetCount = 0, nCount = 0;

            string strPath = DBInfo.SystemDir + DBInfo.ExcelSystemPara;

            try
            {
                Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbook WorkBook = ExcelApp.Workbooks.Open(strPath);

                // 1. Open 한 Excel File에 Sheet Count
                nSheetCount = WorkBook.Worksheets.Count;

                // 2. Excel Sheet Row, Column 접근을 위한 Range 생성
                Excel.Range[] SheetRange = new Excel.Range[nSheetCount];

                // 3. Excel Sheet Item Data 획득을 위한 Worksheet 생성
                Excel.Worksheet[] Sheet = new Excel.Worksheet[nSheetCount];

                // 4. Excel Sheet 정보를 불러 온다.
                for (i = 0; i < nSheetCount; i++)
                {
                    Sheet[i] = (Excel.Worksheet)WorkBook.Worksheets.get_Item(i + 1);
                    SheetRange[i] = Sheet[i].UsedRange;
                }

                nCount = ParaInfoList.Count;

                for (i = 0; i < nCount; i++)
                {
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 1] as Excel.Range).Value2 = ParaInfoList[i].Group;
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 2] as Excel.Range).Value2 = ParaInfoList[i].Name;
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 3] as Excel.Range).Value2 = ParaInfoList[i].Unit;
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 4] as Excel.Range).Value2 = ParaInfoList[i].Type;

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 5] as Excel.Range).Value2 = ParaInfoList[i].DisplayName[(int)DEF_Common.ELanguage.KOREAN];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 6] as Excel.Range).Value2 = ParaInfoList[i].DisplayName[(int)DEF_Common.ELanguage.ENGLISH];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 7] as Excel.Range).Value2 = ParaInfoList[i].DisplayName[(int)DEF_Common.ELanguage.CHINESE];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 8] as Excel.Range).Value2 = ParaInfoList[i].DisplayName[(int)DEF_Common.ELanguage.JAPANESE];

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 9] as Excel.Range).Value2 = ParaInfoList[i].Description[(int)DEF_Common.ELanguage.KOREAN];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 10] as Excel.Range).Value2 = ParaInfoList[i].Description[(int)DEF_Common.ELanguage.ENGLISH];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 11] as Excel.Range).Value2 = ParaInfoList[i].Description[(int)DEF_Common.ELanguage.CHINESE];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 12] as Excel.Range).Value2 = ParaInfoList[i].Description[(int)DEF_Common.ELanguage.JAPANESE];
                }

                WorkBook.Save();
                WorkBook.Close(true);
                ExcelApp.Quit();

            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_SYSTEM_EXCEL_FILE_READ_FAIL);
            }

            WriteLog($"success : Save System Parameter Info", ELogType.Debug);

            return SUCCESS;
        }

        public int SaveMotorDataToExcel(string [,] strParameter)
        {
            int i = 0, j = 0, nSheetCount = 0, nCount = 0;

            string strPath = DBInfo.SystemDir + DBInfo.ExcelSystemPara;

            try
            {
                Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbook WorkBook = ExcelApp.Workbooks.Open(strPath);

                // 1. Open 한 Excel File에 Sheet Count
                nSheetCount = WorkBook.Worksheets.Count;

                // 2. Excel Sheet Row, Column 접근을 위한 Range 생성
                Excel.Range[] SheetRange = new Excel.Range[nSheetCount];

                // 3. Excel Sheet Item Data 획득을 위한 Worksheet 생성
                Excel.Worksheet[] Sheet = new Excel.Worksheet[nSheetCount];

                // 4. Excel Sheet 정보를 불러 온다.
                for (i = 0; i < nSheetCount; i++)
                {
                    Sheet[i] = (Excel.Worksheet)WorkBook.Worksheets.get_Item(i + 1);
                    SheetRange[i] = Sheet[i].UsedRange;
                }

                // Motor Data Sheet
                for (i = 0; i < 19; i++)
                {
                    for(j=0;j<28;j++)
                    {
                        (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, j+3] as Excel.Range).Value2 = strParameter[i,j];
                    }
                }

                WorkBook.Save();
                WorkBook.Close(true);
                ExcelApp.Quit();

            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_SYSTEM_EXCEL_FILE_READ_FAIL);
            }

            WriteLog($"success : Save Motor Data", ELogType.Debug);
            return SUCCESS;
        }


        void InitMPMotionData()
        {
            // Excel에서 읽어오는 방식도 생각해봤으나, 축 이름과 필수적인것들만 초기화하면 될것 같아서 소스코드 내부에서 처리 
            int index;
            CMPMotionData tMotion;

            // null check
            for(int i = 0; i < SystemData_Axis.MPMotionData.Length; i++)
            {
                if (SystemData_Axis.MPMotionData[i] == null)
                {
                    SystemData_Axis.MPMotionData[i] = new CMPMotionData();
                }
            }

            // LOADER_Z
            index = (int)EYMC_Axis.LOADER_Z         ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "LOADER_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // PUSHPULL_Y
            index = (int)EYMC_Axis.PUSHPULL_Y       ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "PUSHPULL_Y";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // PUSHPULL_X1   
            index = (int)EYMC_Axis.PUSHPULL_X1   ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "PUSHPULL_X1";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // C1_CHUCK_ROTATE_T
            index = (int)EYMC_Axis.S1_CHUCK_ROTATE_T;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "C1_CHUCK_ROTATE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // C1_CLEAN_NOZZLE_T
            index = (int)EYMC_Axis.S1_CLEAN_NOZZLE_T;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "C1_CLEAN_NOZZLE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // C1_COAT_NOZZLE_T 
            index = (int)EYMC_Axis.S1_COAT_NOZZLE_T ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "C1_COAT_NOZZLE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // PUSHPULL_X2   
            index = (int)EYMC_Axis.PUSHPULL_X2   ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "PUSHPULL_X2";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // C2_CHUCK_ROTATE_T
            index = (int)EYMC_Axis.S2_CHUCK_ROTATE_T;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "C2_CHUCK_ROTATE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // C2_CLEAN_NOZZLE_T
            index = (int)EYMC_Axis.S2_CLEAN_NOZZLE_T;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "C2_CLEAN_NOZZLE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // C2_COAT_NOZZLE_T 
            index = (int)EYMC_Axis.S2_COAT_NOZZLE_T ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "C2_COAT_NOZZLE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // UHANDLER_X       
            index = (int)EYMC_Axis.UHANDLER_X       ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "UHANDLER_X";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // UHANDLER_Z       
            index = (int)EYMC_Axis.UHANDLER_Z       ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "UHANDLER_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // LHANDLER_X       
            index = (int)EYMC_Axis.LHANDLER_X;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "LHANDLER_X";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // LHANDLER_Z       
            index = (int)EYMC_Axis.LHANDLER_Z       ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "LHANDLER_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // CAMERA1_Z                                           
            index = (int)EYMC_Axis.CAMERA1_Z        ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "CAMERA1_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // LASER1_Z
            index = (int)EYMC_Axis.LASER1_Z         ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "LASER1_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

        }
    }
}
