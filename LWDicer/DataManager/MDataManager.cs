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
using static LWDicer.Control.DEF_LCNet;

using static LWDicer.Control.DEF_ACS;
using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_Cylinder;
using static LWDicer.Control.DEF_Vacuum;

using static LWDicer.Control.DEF_MeStage;
using static LWDicer.Control.DEF_MeHandler;
using static LWDicer.Control.DEF_MeElevator;
using static LWDicer.Control.DEF_MePushPull;
using static LWDicer.Control.DEF_MeSpinner;
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
        public const int ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA     = 5;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA     = 6;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA      = 7;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA      = 8;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_POSITION_DATA    = 9;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_POSITION_DATA    = 10;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MODEL_DATA       = 11;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA       = 12;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MODEL_LIST       = 13;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_MODEL_DATA     = 14;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST       = 15;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_LOGIN_HISTORY    = 16;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_LOGIN_HISTORY    = 17;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_ALARM_INFO       = 18;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_ALARM_INFO       = 19;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MESSAGE_INFO     = 20;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MESSAGE_INFO     = 21;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_PARAMETER_INFO   = 22;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_PARAMETER_INFO   = 23;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_ALARM_HISTORY    = 24;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_ALARM_HISTORY    = 25;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_ROOT_FOLDER    = 26;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_DEFAULT_MODEL  = 27;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_CURRENT_MODEL  = 28;
        public const int ERR_DATA_MANAGER_FAIL_EXCEL_IMPORT          = 29;
        public const int ERR_DATA_MANAGER_FAIL_EXCEL_EXPORT          = 30;


        //public const int ERR_DATA_MANAGER_IO_DATA_FILE_NOT_EXIST = 1;
        //public const int ERR_DATA_MANAGER_IO_DATA_FILE_CLOSE_FAILURE = 2;
        //public const int ERR_DATA_MANAGER_CAMERA_NO_OUT_RANGE = 3;
        //public const int ERR_DATA_MANAGER_TEACHING_INFO_FILE_NOT_EXIST = 4;
        //public const int ERR_DATA_MANAGER_UNIT_INDEX_OUT_RANGE = 5;
        //public const int ERR_DATA_MANAGER_POS_INDEX_OUT_RANGE = 6;
        //public const int ERR_DATA_MANAGER_ARG_NULL_POINTER = 7;
        //public const int ERR_DATA_MANAGER_POS_INFO_FILE_CLOSE_FAILURE = 8;
        //public const int ERR_DATA_MANAGER_VACUUM_INDEX_OUT_RANGE = 9;
        //public const int ERR_DATA_MANAGER_IO_NOT_INITIALIZED = 10;
        //public const int ERR_DATA_MANAGER_MODEL_NAME_NULL = 11;
        //public const int ERR_DATA_MANAGER_MODEL_FILE_NOT_EXIST = 12;
        //public const int ERR_DATA_MANAGER_MODEL_CHANGE_FAIL = 13;
        //public const int ERR_DATA_MANAGER_DELETE_CURRENT_MODEL_FAIL = 14;
        //public const int ERR_DATA_MANAGER_COPY_SELF_MODEL_INCORRECT = 15;
        //public const int ERR_DATA_MANAGER_CREATE_MODEL_FAIL = 16;
        //public const int ERR_DATA_MANAGER_INVALID_FIXEDCOORD_UNIT_ID = 17;

        //public const int ERR_SYSTEM_DATA_MANAGER_FILE_SAVE_FAIL = 20;
        //public const int ERR_SYSTEM_DATA_MANAGER_NO_SECTIONNAME = 21;

        //public const int ERR_MODEL_DATA_MAIN_MODEL_NAME_NULL = 31;
        //public const int ERR_MODEL_DATA_MANAGER_NO_SECTIONNAME = 32;

        //public const int DEF_MAX_SYSTEM_SECTION = 7;
        //public const int DEF_MAX_FIXED_POSITION_SECTION = 14;
        //public const int DEF_MAX_OFFSET_POSITION_SECTION = 14;
        //public const int DEF_MAX_AXIS_NUM = 4;

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
            // Loader
            public CPos_XYTZ Elevator_Pos;

            // PushPull
            public CPos_XYTZ PushPull_Pos;
            public CPos_XYTZ Centering_Pos; // PushPull Centering

            // Spinner
            public CPos_XYTZ S1_Rotate_Pos;
            public CPos_XYTZ S1_CleanNozzel_Pos;
            public CPos_XYTZ S1_CoatNozzel_Pos;

            public CPos_XYTZ S2_Rotate_Pos;
            public CPos_XYTZ S2_CleanNozzel_Pos;
            public CPos_XYTZ S2_CoatNozzel_Pos;

            // Handler
            public CPos_XYTZ UHandler_Pos;
            public CPos_XYTZ LHandler_Pos;

            // Stage
            public CPos_XYTZ Stage_Pos;
            public CPos_XYTZ Camera_Pos;
            public CPos_XYTZ Scanner_Pos;
        }

        public class CSystemData
        {
            // Axis, Cylinder, Vacuum 등의 class array는 별도의 class에서 처리하도록 한다.
            //
            public string ModelName = NAME_DEFAULT_MODEL;

            public ELanguage Language = ELanguage.KOREAN;

            public string UserName = NAME_DEFAULT_OPERATOR;

            // SafetyPos for Axis Move 
            // Teching 화면에서 Teaching하는 UnitPos.WaitPos 과는 다른 용도로, make engineer가 
            // 시스템적으로 지정하는 절대 안전 위치
            public CSystemData_MAxSafetyPos MAxSafetyPos = new CSystemData_MAxSafetyPos();

            /////////////////////////////////////////////////////////
            // 아래는 아직 미정리 내역들. 
            // * 혹시, 아래에서 사용하는것들은 이 주석 위로 올려주기 바람
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


            public double Stage1IndexRotate = 90.0;
            public double Stage1JogSpeed = 10.0;
            public double Stage1ThetaJogSpeed = 10.0;

            
            public double VisionLaserDistance = 0.0;

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
            // ACS Motion Axis
            public CACSMotionData[] ACSMotionData = new CACSMotionData[USE_ACS_AXIS_COUNT];
            // YMC Motion Axis
            public CMPMotionData[] MPMotionData = new CMPMotionData[MAX_MP_AXIS];

            public CSystemData_Axis()
            {
                // ACS 모션
                for (int i = 0; i < ACSMotionData.Length; i++)
                {
                    ACSMotionData[i] = new CACSMotionData();
                }
                // Yaskawa 모션
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



        public class CSystemData_Vision
        {
            //public int[] LenMagnification = new int[(int)ECameraSelect.MAX];

            //// 렌즈 Resolution & 카메라 Position
            //public int[] CamPixelNumX = new int[(int)ECameraSelect.MAX];
            //public int[] CamPixelNumY = new int[(int)ECameraSelect.MAX];
            //public double[] PixelResolutionX = new double[(int)ECameraSelect.MAX];
            //public double[] PixelResolutionY = new double[(int)ECameraSelect.MAX];

            //public double[] CamFovX = new double[(int)ECameraSelect.MAX];    // 이 수치는 자동 계산됨
            //public double[] CamFovY = new double[(int)ECameraSelect.MAX];    // 이 수치는 자동 계산됨

            //public CPos_XY[] Position = new CPos_XY[(int)ECameraSelect.MAX];            
            //public double[] CameraTilt = new double[(int)ECameraSelect.MAX];

            public CCameraData[] Camera= new CCameraData[(int)ECameraSelect.MAX];

            public CSystemData_Vision()
            {
                for (int i = 0; i < (int)ECameraSelect.MAX; i++)
                {
                    Camera[i] = new CCameraData();
                    Camera[i].CamFovX = Camera[i].CamPixelNumX * Camera[i].PixelResolutionX;
                    Camera[i].CamFovY = Camera[i].CamPixelNumY * Camera[i].PixelResolutionY;
                }
            }

        }

        public class CSystemData_Light
        {
            public CLightData[] Light = new CLightData[(int)ELightController.MAX];

        }

        public class CPositionData
        {
            // Loader
            public CPosition LoaderPos = new CPosition((int)EElevatorPos.MAX);

            // Stage1
            public CPosition Stage1Pos = new CPosition((int)EStagePos.MAX);
            // Camera1
            public CPosition Camera1Pos = new CPosition((int)ECameraPos.MAX);
            // Scanner1
            public CPosition Scanner1Pos = new CPosition((int)EScannerPos.MAX);

            // PushPull
            public CPosition PushPullPos = new CPosition((int)EPushPullPos.MAX);
            public CPosition Centering1Pos = new CPosition((int)ECenterPos.MAX);
            public CPosition Centering2Pos = new CPosition((int)ECenterPos.MAX);

            // Handler
            public CPosition UpperHandlerPos = new CPosition((int)EHandlerPos.MAX);
            public CPosition LowerHandlerPos = new CPosition((int)EHandlerPos.MAX);

            // Spinner1
            public CPosition S1_RotatePos = new CPosition((int)ERotatePos.MAX);
            public CPosition S1_CoaterPos = new CPosition((int)ENozzlePos.MAX);
            public CPosition S1_CleanerPos = new CPosition((int)ENozzlePos.MAX);

            // Spinner2
            public CPosition S2_RotatePos = new CPosition((int)ERotatePos.MAX);
            public CPosition S2_CoaterPos = new CPosition((int)ENozzlePos.MAX);
            public CPosition S2_CleanerPos = new CPosition((int)ENozzlePos.MAX);

            public CPositionData()
            {
                for(int i=0; i< (int)EStagePos.MAX;i++)
                {
                    Stage1Pos.Pos[i] = new CPos_XYTZ();
                }
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
            public double WaferSize;
            public double Size_X;
            public double Size_Y;
            public CPanelMarkPos FiduMarkXu = new CPanelMarkPos();
            //public SPanelMarkPos sFiduMarkXd;
            //public SPanelMarkPos sFiduMarkYl;
            //public SPanelMarkPos sFiduMarkYr;
            //public EInputDirection eInputDirection;
            //public EOutputDirection eOutputDirection;
            public double WaferThickness;
            public double TapeThickness;
        }

        /// <summary>
        /// Define Wafer Cassette Data
        /// </summary>
        public class CWaferCassette
        {
            public string Name;
            public double Diameter;          // Cassette Frame 지름 ex) 380mm
            public int Slot;                 // 슬롯갯수            ex) 13ea
            public int[] SlotData = new int[CASSETTE_MAX_SLOT_NUM]; // 각 슬롯 상태 및 Wafer 처리여부 데이터
            public int CassetteSetNo;        // Cassette 갯수 ex) 2ea
            public double FramePitch;        // Cassette Slot Fitch ex) 9.5mm
            public double CassetteHeight;    // Cassette 높이 ex)155mm
            public double UnloadElevatorPos; // Elevator Start Origin Position에서 Unloading을 위하여 Cassette Offset 높이 ex) -1mm 하강
            public double ESZeroPoint;       // Elevator Start Origin Position ex) Teaching Position 258.3mm
            public double CTZeroPoint;	     // Chuck Table Angle?
            public double STZeroPoint;       // Spinner Table Angle?
            public double LoadPushPullPos;   // PushPull 끝단에 설치 되어 있는 감지센서 부터 Cassette에 적재되어 있는 Wafer 까지 거리 ex) 61mm
            public double FrameCenterPos;    // Centering Unit Wafer Centering Teaching Position ex) 52.4mm
        }

        /// <summary>
        /// Define Wafer Frame Data
        /// </summary>
        public class CWaferFrame
        {
            public string Name;
            public double StagePos;          // Inspection Stage에 적재 되어 있는 Cassette에 PushPull이 Unloading 가능한 Elevator 의 Teaching 높이 ex) 450.5mm
            public double UnloadElevatorPos; // Elevator Start Origin Position에서 Unloading을 위하여 Cassette Offset 높이 ex) -1mm 하강
            public double LoadPushPullPos;   // PushPull 끝단에 설치 되어 있는 감지센서 부터 Cassette에 적재되어 있는 Wafer 까지 거리 ex) 61mm
            public double UnloadPushPullPos; // PushPull 끝단에 설치 되어 있는 감지센서 부터 Cassette에 적재되어 있는 Wafer 까지 거리 ex) 61mm
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
        public const string NAME_ROOT_FOLDER          = "root";
        public const string NAME_DEFAULT_MODEL        = "default";
        public const string NAME_ENGINEER_FOLDER      = "Engineer";
        public const string NAME_OPERATOR_FOLDER      = "Operator";
        public const string NAME_DEFAULT_OPERATOR     = "OP0001";


        public enum EListHeaderType
        {
            MODEL = 0,
            CASSETTE,
            WAFERFRAME,
            USERINFO,
            MAX,
        }

        /// <summary>
        /// Model, Cassette, WaferFrame Data 의 계층구조를 만들기 위해서 Header만 따로 떼어서 관리.
        /// Folder인 경우엔 IsFolder = true & CModelData는 따로 만들지 않음.
        /// Model인 경우엔 IsFolder = false & CModelData에 같은 이름으로 ModelData가 존재함.
        /// </summary>
        public class CListHeader
        {
            // Header
            public string Name;   // unique primary key
            public string Comment;
            public string Parent = NAME_ROOT_FOLDER; // if == "root", root
            public bool IsFolder = false; // true, if it is folder.
            public int TreeLevel = -1; // models = -1, root = 0, 1'st generation = 1, 2'nd generation = 2.. 3,4,5

            public void SetRootFolder()
            {
                SetFolder(NAME_ROOT_FOLDER, "Root Folder", "");
                TreeLevel = 0;
            }

            public void SetFolder(string Name, string Comment, string Parent)
            {
                SetModel(Name, Comment, Parent);
                IsFolder = true;
                TreeLevel = 1;                
            }

            public void SetDefaultModel()
            {
                SetModel(NAME_DEFAULT_MODEL, "Default Model", NAME_ROOT_FOLDER);
            }

            public void SetModel(string Name, string Comment, string Parent)
            {
                this.Name = Name;
                this.Comment = Comment;
                this.Parent = Parent;
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
            // Wafer Data
            public CWaferData Wafer = new CWaferData();
            public string CassetteName = NAME_DEFAULT_MODEL;    // wafer cassette
            public string WaferFrameName = NAME_DEFAULT_MODEL;       // wafer frame

            // Spinner Data 
            public CSpinnerData SpinnerData = new CSpinnerData();


            ///////////////////////////////////////////////////////////
            // Vision Data (Pattern)
            public CSearchData MacroPatternA = new CSearchData();
            public CSearchData MacroPatternB = new CSearchData();
            public CSearchData MicroPatternA = new CSearchData();
            public CSearchData MicroPatternB = new CSearchData();

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


            ///////////////////////////////////////////////////////////////
            // 이하 아래부분 미정리한것들임
            public bool Use2Step_Use;

            // Dispenser
            public int uDispensingTwiceTime;
            public int uDisepnsingSpeed;
            public int uUVLEDSpeed;

            public double ThicknessValue; // 판넬 두께

            public bool UseUHandler_ExtraVccUseFlag; // 2014.02.21 by ranian. Extra Vcc 추가
            public bool UseUHandler_WaitPosUseFlag; // 2014.02.21 by ranian. LP->UP 로 갈 때, WP 사용 여부

        }
    }

    public class MDataManager : MObject
    {
        public CDBInfo DBInfo { get; private set; }

        /////////////////////////////////////////////////////////////////////////////////
        // System Model Data
        public CSystemData SystemData { get; private set; } = new CSystemData();


        public CSystemData_Axis SystemData_Axis { get; private set; } = new CSystemData_Axis();
        public CSystemData_Cylinder SystemData_Cylinder { get; private set; } = new CSystemData_Cylinder();
        public CSystemData_Vacuum SystemData_Vacuum { get; private set; } = new CSystemData_Vacuum();
        public CSystemData_Vision SystemData_Vision { get; private set; } = new CSystemData_Vision();
        public CSystemData_Light SystemData_Light { get; private set; } = new CSystemData_Light();

        /////////////////////////////////////////////////////////////////////////////////
        // Position Data
        public CPositionData FixedPos { get; private set; } = new CPositionData();
        public CPositionData ModelPos { get; private set; } = new CPositionData();
        public CPositionData OffsetPos { get; private set; } = new CPositionData();

        /////////////////////////////////////////////////////////////////////////////////
        // User Info
        private CLoginInfo LoginInfo;
        public List<CListHeader> UserInfoHeaderList { get; set; } = new List<CListHeader>();

        /////////////////////////////////////////////////////////////////////////////////
        // Model Data
        public CModelData ModelData { get; private set; } = new CModelData();
        public List<CListHeader> ModelHeaderList { get; set; } = new List<CListHeader>();

        // Wafer Cassette Data
        public CWaferCassette CassetteData { get; private set; } = new CWaferCassette();
        public List<CListHeader> CassetteHeaderList { get; set; } = new List<CListHeader>();

        // WaferFrame Data
        public CWaferFrame WaferFrameData { get; private set; } = new CWaferFrame();
        public List<CListHeader> WaferFrameHeaderList { get; set; } = new List<CListHeader>();

        /////////////////////////////////////////////////////////////////////////////////
        // General Information Data 
        // IO Name
        public DEF_IO.CIOInfo[] InputArray { get; private set; } = new DEF_IO.CIOInfo[DEF_IO.MAX_IO_INPUT];
        public DEF_IO.CIOInfo[] OutputArray { get; private set; } = new DEF_IO.CIOInfo[DEF_IO.MAX_IO_OUTPUT];

        // Alarm Information & History
        public List<CAlarmInfo> AlarmInfoList { get; private set; } = new List<CAlarmInfo>();
        public List<CAlarm> AlarmHistory { get; private set; } = new List<CAlarm>();

        // Message Information for Message 표시할 때..
        public List<CMessageInfo> MessageInfoList { get; private set; } = new List<CMessageInfo>();

        // Parameter Information for Display에 보여줄때 다국어 지원을 위해서 관리
        public List<CParaInfo> ParaInfoList { get; private set; } = new List<CParaInfo>();

        /////////////////////////////////////////////////////////////////////////////////
        // WorkPiece Data
        // 설비안의 각 위치에 있는 WorkPiece의 정보를 관리
        public CWorkPiece[] WorkPieceArray { get; private set; } = new CWorkPiece[(int)ELCNetUnitPos.MAX];
        // 임시로 우선 공정에 들어가기 전, 후의 List를 관리하려고 선언
        public List<CWorkPiece> WorkPieceList_NF { get; private set; } = new List<CWorkPiece>();
        public List<CWorkPiece> WorkPieceList_Finished { get; private set; } = new List<CWorkPiece>();

        public MDataManager(CObjectInfo objInfo, CDBInfo dbInfo)
            : base(objInfo)
        {
            DBInfo = dbInfo;
            SetLogin(true);

            for(int i = 0; i < DEF_IO.MAX_IO_INPUT; i++)
            {
                InputArray[i] = new DEF_IO.CIOInfo(i+DEF_IO.INPUT_ORIGIN, DEF_IO.EIOType.DI);
            }
            for (int i = 0; i < DEF_IO.MAX_IO_OUTPUT; i++)
            {
                OutputArray[i] = new DEF_IO.CIOInfo(i+DEF_IO.OUTPUT_ORIGIN, DEF_IO.EIOType.DO);
            }

            for (int i = 0; i < WorkPieceArray.Length ; i++)
            {
                WorkPieceArray[i] = new CWorkPiece();
            }

            TestFunction();

            LoadGeneralData();

            // 아래의 네가지 함수 콜은 LWDicer의 Initialize에서 읽어들이는게 맞지만, 생성자에서 한번 더 읽어도 되기에.. 주석처리해도 상관없음
            LoadSystemData();
            LoadPositionData(true, EPositionObject.ALL);
            LoadPositionData(false, EPositionObject.ALL);
            LoadModelList();
           // MakeDefaultModel();
            ChangeModel(SystemData.ModelName);
        }

        public void TestFunction()
        {
            ///////////////////////////////////////
            if(false)
            {
                CListHeader header = new CListHeader();
                ModelHeaderList.Add(header);

                for (int i = 0; i < 3; i++)
                {
                    header = new CListHeader();
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

            // 초기에 alarm info 목록 저장 test routine
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

            // 프로그램 시작시에 Position Data db에 초기에 저장 test routine
            if (false)
            {
                SavePositionData(true, EPositionGroup.ALL);
                SavePositionData(false, EPositionGroup.ALL);
            }

            // WorkPiece and Process Phase를 한번 쭉~ test routine
            if (false)
            {
                CWorkPiece pointer = WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL];
                LoadWorkPieceFromCassette();
                ELCNetUnitPos pos = ELCNetUnitPos.PUSHPULL;
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_LOAD_FROM_LOADER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_LOAD_FROM_LOADER);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_COATER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_COATER);

                pos = ELCNetUnitPos.SPINNER1;
                ChangeWorkPieceUnit(ELCNetUnitPos.PUSHPULL, pos);
                StartWorkPiecePhase(pos, EProcessPhase.COATER_LOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.COATER_LOAD);
                StartWorkPiecePhase(pos, EProcessPhase.COATING);
                FinishWorkPiecePhas(pos, EProcessPhase.COATING);
                StartWorkPiecePhase(pos, EProcessPhase.COATER_UNLOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.COATER_UNLOAD);

                pos = ELCNetUnitPos.PUSHPULL;
                ChangeWorkPieceUnit(ELCNetUnitPos.SPINNER1, pos);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_LOAD_FROM_COATER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_LOAD_FROM_COATER);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_HANDLER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_HANDLER);

                pos = ELCNetUnitPos.UPPER_HANDLER;
                ChangeWorkPieceUnit(ELCNetUnitPos.PUSHPULL, pos);
                StartWorkPiecePhase(pos, EProcessPhase.UPPER_HANDLER_LOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.UPPER_HANDLER_LOAD);
                StartWorkPiecePhase(pos, EProcessPhase.UPPER_HANDLER_UNLOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.UPPER_HANDLER_UNLOAD);

                pos = ELCNetUnitPos.STAGE1;
                ChangeWorkPieceUnit(ELCNetUnitPos.UPPER_HANDLER, pos);
                StartWorkPiecePhase(pos, EProcessPhase.STAGE_LOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.STAGE_LOAD);
                StartWorkPiecePhase(pos, EProcessPhase.MACRO_ALIGN);
                FinishWorkPiecePhas(pos, EProcessPhase.MACRO_ALIGN);
                StartWorkPiecePhase(pos, EProcessPhase.MICRO_ALIGN);
                FinishWorkPiecePhas(pos, EProcessPhase.MICRO_ALIGN);
                StartWorkPiecePhase(pos, EProcessPhase.DICING);
                FinishWorkPiecePhas(pos, EProcessPhase.DICING);
                StartWorkPiecePhase(pos, EProcessPhase.STAGE_UNLOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.STAGE_UNLOAD);

                pos = ELCNetUnitPos.LOWER_HANDLER;
                ChangeWorkPieceUnit(ELCNetUnitPos.STAGE1, pos);
                StartWorkPiecePhase(pos, EProcessPhase.LOWER_HANDLER_LOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.LOWER_HANDLER_LOAD);
                StartWorkPiecePhase(pos, EProcessPhase.LOWER_HANDLER_UNLOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.LOWER_HANDLER_UNLOAD);

                pos = ELCNetUnitPos.PUSHPULL;
                ChangeWorkPieceUnit(ELCNetUnitPos.LOWER_HANDLER, pos);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_LOAD_FROM_HANDLER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_LOAD_FROM_HANDLER);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_CLEANER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_CLEANER);

                pos = ELCNetUnitPos.SPINNER1;
                ChangeWorkPieceUnit(ELCNetUnitPos.PUSHPULL, pos);
                StartWorkPiecePhase(pos, EProcessPhase.CLEANER_LOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.CLEANER_LOAD);
                StartWorkPiecePhase(pos, EProcessPhase.CLEANING);
                FinishWorkPiecePhas(pos, EProcessPhase.CLEANING);
                StartWorkPiecePhase(pos, EProcessPhase.CLEANER_UNLOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.CLEANER_UNLOAD);

                pos = ELCNetUnitPos.PUSHPULL;
                ChangeWorkPieceUnit(ELCNetUnitPos.SPINNER1, pos);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_LOAD_FROM_CLEANER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_LOAD_FROM_CLEANER);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_LOADER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_LOADER);

                LoadWorkPieceToCassette();
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
                    WriteLog("fail : backup db.", ELogType.Debug, ELogWType.D_Error);
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_BACKUP_DB);
                }
            }

            WriteLog("success : backup db.", ELogType.Debug);
            return SUCCESS;
        }

        public int DeleteDB(string dbConn = "")
        {
            string[] dblist;
            if (dbConn == "")
            {
                dblist = new string[] { $"{DBInfo.DBConn}", $"{DBInfo.DBConn_Backup}",
                $"{DBInfo.DBConn_Info}", $"{DBInfo.DBConn_DLog}", $"{DBInfo.DBConn_ELog}" };
            } else
            {
                dblist = new string[] { $"{dbConn}"};
            }

            foreach (string source in dblist)
            {
                if (DBManager.DeleteDB(source) == false)
                {
                    WriteLog("fail : delete db.", ELogType.Debug, ELogWType.D_Error);
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_DB);
                }
            }

            WriteLog("success : delete db.", ELogType.Debug);
            return SUCCESS;
        }

        public int SaveSystemData(CSystemData system = null, CSystemData_Axis systemAxis = null,
            CSystemData_Cylinder systemCylinder = null, CSystemData_Vacuum systemVacuum = null,
            CSystemData_Vision systemVision =null, CSystemData_Light systemLight = null)
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



            // CSystemData_Vision
            if (systemVision != null)
            {
                try
                {
                    SystemData_Vision = ObjectExtensions.Copy(systemVision);
                    string output = JsonConvert.SerializeObject(SystemData_Vision);

                    if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TableSystem, "name", nameof(CSystemData_Vision), output,
                        true, DBInfo.DBConn_Backup) != true)
                    {
                        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                    }
                    WriteLog("success : save CSystemData_Vision.", ELogType.SYSTEM, ELogWType.SAVE);
                }
                catch (Exception ex)
                {
                    WriteExLog(ex.ToString());
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                }
            }

            // CSystemData_Light
            if (systemLight != null)
            {
                try
                {
                    SystemData_Light = ObjectExtensions.Copy(systemLight);
                    string output = JsonConvert.SerializeObject(SystemData_Light);

                    if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TableSystem, "name", nameof(CSystemData_Light), output,
                        true, DBInfo.DBConn_Backup) != true)
                    {
                        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                    }
                    WriteLog("success : save CSystemData_Vision.", ELogType.SYSTEM, ELogWType.SAVE);
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
            bool loadVacuum = true, bool loadScanner = true, bool loadVision = true, bool loadLight = true)
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

                        if (SystemData_Axis.ACSMotionData.Length == data.ACSMotionData.Length)
                        {
                            SystemData_Axis = ObjectExtensions.Copy(data);
                        }
                        else
                        {
                            for (int i = 0; i < SystemData_Axis.ACSMotionData.Length; i++)
                            {
                                if (i >= data.ACSMotionData.Length) break;
                                SystemData_Axis.ACSMotionData[i] = ObjectExtensions.Copy(data.ACSMotionData[i]);
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
                InitACSMotionData();
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

            

            // CSystemData_Vision
            if (loadVision == true)
            {
                try
                {
                    if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableSystem, out output, new CDBColumn("name", nameof(CSystemData_Vision))) == true)
                    {
                        CSystemData_Vision data = JsonConvert.DeserializeObject<CSystemData_Vision>(output);
                        if (SystemData_Vision.Camera.Length == data.Camera.Length)
                        {
                            SystemData_Vision = ObjectExtensions.Copy(data);
                        }
                        else
                        {
                            for (int i = 0; i < SystemData_Vision.Camera.Length; i++)
                            {
                                if (i >= data.Camera.Length) break;
                                SystemData_Vision.Camera[i] = ObjectExtensions.Copy(data.Camera[i]);
                            }
                        }
                        WriteLog("success : load CSystemData_Vision.", ELogType.SYSTEM, ELogWType.LOAD);
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

            // CSystemData_Light
            if (loadLight == true)
            {
                try
                {
                    if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableSystem, out output, new CDBColumn("name", nameof(CSystemData_Light))) == true)
                    {
                        CSystemData_Light data = JsonConvert.DeserializeObject<CSystemData_Light>(output);
                        if (SystemData_Light.Light.Length == data.Light.Length)
                        {
                            SystemData_Light = ObjectExtensions.Copy(data);
                        }
                        else
                        {
                            for (int i = 0; i < SystemData_Light.Light.Length; i++)
                            {
                                if (i >= data.Light.Length) break;
                                SystemData_Light.Light[i] = ObjectExtensions.Copy(data.Light[i]);
                            }
                        }
                        WriteLog("success : load CSystemData_Light.", ELogType.SYSTEM, ELogWType.LOAD);
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

        private int SaveUnitPositionData(string key_value, string output)
        {
            try
            {
                if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TablePos, "name", key_value, output,
                    true, DBInfo.DBConn_Backup) != true)
                {
                    WriteLog($"fail : save {key_value} Position.", ELogType.SYSTEM, ELogWType.SAVE);
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_POSITION_DATA);
                }
                WriteLog($"success : save {key_value} Position.", ELogType.SYSTEM, ELogWType.SAVE);
            }
            catch (Exception ex)
            {
                WriteLog($"fail : save {key_value} Position.", ELogType.SYSTEM, ELogWType.SAVE);
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_POSITION_DATA);
            }
            return SUCCESS;
        }

        public int SavePositionData(bool bLoadFixed, EPositionObject unit)
        {
            int iResult;
            string key_value, output;
            CPositionData tData = OffsetPos;
            string suffix = "_Offset_" + SystemData.ModelName;
            if (bLoadFixed)
            {
                tData = FixedPos;
                suffix = "_Fixed";
            }

            // Loader
            if (unit == EPositionObject.ALL || unit == EPositionObject.LOADER)
            {
                key_value = EPositionObject.LOADER.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.LoaderPos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            // PushPull
            if (unit == EPositionObject.ALL || unit == EPositionObject.PUSHPULL)
            {
                key_value = EPositionObject.PUSHPULL.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.PushPullPos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.PUSHPULL_CENTER1)
            {
                key_value = EPositionObject.PUSHPULL_CENTER1.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.Centering1Pos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.PUSHPULL_CENTER2)
            {
                key_value = EPositionObject.PUSHPULL_CENTER2.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.Centering2Pos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner1
            if (unit == EPositionObject.ALL || unit == EPositionObject.S1_ROTATE)
            {
                key_value = EPositionObject.S1_ROTATE.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.S1_RotatePos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.S1_CLEAN_NOZZLE)
            {
                key_value = EPositionObject.S1_CLEAN_NOZZLE.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.S1_CleanerPos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.S1_COAT_NOZZLE)
            {
                key_value = EPositionObject.S1_COAT_NOZZLE.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.S1_CoaterPos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner2
            if (unit == EPositionObject.ALL || unit == EPositionObject.S2_ROTATE)
            {
                key_value = EPositionObject.S2_ROTATE.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.S2_RotatePos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.S2_CLEAN_NOZZLE)
            {
                key_value = EPositionObject.S2_CLEAN_NOZZLE.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.S2_CleanerPos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.S2_COAT_NOZZLE)
            {
                key_value = EPositionObject.S2_COAT_NOZZLE.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.S2_CoaterPos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            // Handler
            if (unit == EPositionObject.ALL || unit == EPositionObject.LOWER_HANDLER)
            {
                key_value = EPositionObject.LOWER_HANDLER.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.UpperHandlerPos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.UPPER_HANDLER)
            {
                key_value = EPositionObject.UPPER_HANDLER.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.LowerHandlerPos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            // Stage
            if (unit == EPositionObject.ALL || unit == EPositionObject.STAGE1)
            {
                key_value = EPositionObject.STAGE1.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.Stage1Pos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.CAMERA1)
            {
                key_value = EPositionObject.CAMERA1.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.Camera1Pos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.SCANNER1)
            {
                key_value = EPositionObject.SCANNER1.ToString() + suffix;
                output = JsonConvert.SerializeObject(tData.Scanner1Pos);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int SavePositionData(bool bLoadFixed, EPositionGroup unit)
        {
            int iResult;

            // Loader
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.LOADER)
            {
                iResult = SavePositionData(bLoadFixed, EPositionObject.LOADER);
                if (iResult != SUCCESS) return iResult;
            }

            // PushPull
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.PUSHPULL)
            {
                iResult = SavePositionData(bLoadFixed, EPositionObject.PUSHPULL);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bLoadFixed, EPositionObject.PUSHPULL_CENTER1);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bLoadFixed, EPositionObject.PUSHPULL_CENTER2);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner1
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.SPINNER1)
            {
                iResult = SavePositionData(bLoadFixed, EPositionObject.S1_ROTATE);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bLoadFixed, EPositionObject.S1_CLEAN_NOZZLE);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bLoadFixed, EPositionObject.S1_COAT_NOZZLE);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner2
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.SPINNER2)
            {
                iResult = SavePositionData(bLoadFixed, EPositionObject.S2_ROTATE);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bLoadFixed, EPositionObject.S2_CLEAN_NOZZLE);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bLoadFixed, EPositionObject.S2_COAT_NOZZLE);
                if (iResult != SUCCESS) return iResult;
            }

            // Handler
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.HANDLER)
            {
                iResult = SavePositionData(bLoadFixed, EPositionObject.LOWER_HANDLER);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bLoadFixed, EPositionObject.UPPER_HANDLER);
                if (iResult != SUCCESS) return iResult;
            }

            // Stage
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.STAGE1)
            {
                iResult = SavePositionData(bLoadFixed, EPositionObject.STAGE1);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bLoadFixed, EPositionObject.CAMERA1);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bLoadFixed, EPositionObject.SCANNER1);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        private int LoadUnitPositionData(string key_value, out string output)
        {
            // db에 없을 경우나, error가 발생했을때를 대비해서, 기본으로 초기화 시켜주기위해서
            string default_key = "{\"default\":\"default\"}";
            output = default_key;
            try
            {
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TablePos, out output, new CDBColumn("name", key_value)) == true)
                {
                    WriteLog($"success : load {key_value} Position.", ELogType.SYSTEM, ELogWType.LOAD);
                }
                else // temporarily do not return error for continuous loading
                {
                    output = default_key;
                    WriteLog($"fail : load {key_value} Position.", ELogType.SYSTEM, ELogWType.LOAD);
                    //return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_POSITION_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteLog($"fail : load {key_value} Position.", ELogType.SYSTEM, ELogWType.LOAD);
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_POSITION_DATA);
            }

            return SUCCESS;
        }

        public int LoadPositionData(bool bLoadFixed, EPositionObject unit)
        {
            int iResult;
            string output;
            string key_value;
            CPositionData tData = OffsetPos;
            string suffix = "_Offset_" + SystemData.ModelName;
            if (bLoadFixed)
            {
                tData = FixedPos;
                suffix = "_Fixed";
            }

            // Loader
            if (unit == EPositionObject.ALL || unit == EPositionObject.LOADER)
            {
                key_value = EPositionObject.LOADER.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.LoaderPos = ObjectExtensions.Copy(data);
            }

            // PushPull
            if (unit == EPositionObject.ALL || unit == EPositionObject.PUSHPULL)
            {
                key_value = EPositionObject.PUSHPULL.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.PushPullPos = ObjectExtensions.Copy(data);
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.PUSHPULL_CENTER1)
            {
                key_value = EPositionObject.PUSHPULL_CENTER1.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.Centering1Pos = ObjectExtensions.Copy(data);
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.PUSHPULL_CENTER2)
            {
                key_value = EPositionObject.PUSHPULL_CENTER2.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.Centering2Pos = ObjectExtensions.Copy(data);
            }

            // Spinner1
            if (unit == EPositionObject.ALL || unit == EPositionObject.S1_ROTATE)
            {
                key_value = EPositionObject.S1_ROTATE.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.S1_RotatePos = ObjectExtensions.Copy(data);
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.S1_CLEAN_NOZZLE)
            {
                key_value = EPositionObject.S1_CLEAN_NOZZLE.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.S1_CleanerPos = ObjectExtensions.Copy(data);
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.S1_COAT_NOZZLE)
            {
                key_value = EPositionObject.S1_COAT_NOZZLE.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.S1_CoaterPos = ObjectExtensions.Copy(data);
            }

            // Spinner2
            if (unit == EPositionObject.ALL || unit == EPositionObject.S2_ROTATE)
            {
                key_value = EPositionObject.S2_ROTATE.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.S2_RotatePos = ObjectExtensions.Copy(data);
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.S2_CLEAN_NOZZLE)
            {
                key_value = EPositionObject.S2_CLEAN_NOZZLE.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.S2_CleanerPos = ObjectExtensions.Copy(data);
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.S2_COAT_NOZZLE)
            {
                key_value = EPositionObject.S2_COAT_NOZZLE.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.S2_CoaterPos = ObjectExtensions.Copy(data);
            }

            // Handler
            if (unit == EPositionObject.ALL || unit == EPositionObject.LOWER_HANDLER)
            {
                key_value = EPositionObject.LOWER_HANDLER.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.UpperHandlerPos = ObjectExtensions.Copy(data);
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.UPPER_HANDLER)
            {
                key_value = EPositionObject.UPPER_HANDLER.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.LowerHandlerPos = ObjectExtensions.Copy(data);
            }

            // Stage1
            if (unit == EPositionObject.ALL || unit == EPositionObject.STAGE1)
            {
                key_value = EPositionObject.STAGE1.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.Stage1Pos = ObjectExtensions.Copy(data);


                /////////////////////////////////////////////////////////////////////
                // Copy될때 Array의 크기가 변함.
                if(tData.Stage1Pos.Pos.Length < (int)EStagePos.MAX)
                {
                    Array.Resize(ref tData.Stage1Pos.Pos, (int)EStagePos.MAX);
                    
                    for(int i= tData.Stage1Pos.Length; i < (int)EStagePos.MAX;i++)
                    {
                            tData.Stage1Pos.Pos[i] = new CPos_XYTZ();
                    }
                }
                /////////////////////////////////////////////////////////////////////

            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.CAMERA1)
            {
                key_value = EPositionObject.CAMERA1.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.Camera1Pos = ObjectExtensions.Copy(data);
            }

            if (unit == EPositionObject.ALL || unit == EPositionObject.SCANNER1)
            {
                key_value = EPositionObject.SCANNER1.ToString() + suffix;
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPosition data = JsonConvert.DeserializeObject<CPosition>(output);
                if(data != null && data.Length > 0)
                    tData.Scanner1Pos = ObjectExtensions.Copy(data);
            }

            if (bLoadFixed) FixedPos = tData; else OffsetPos = tData;

            return SUCCESS;
        }

        public int LoadPositionData(bool bLoadFixed, EPositionGroup unit)
        {
            int iResult;

            // Loader
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.LOADER)
            {
                iResult = LoadPositionData(bLoadFixed, EPositionObject.LOADER);
                if (iResult != SUCCESS) return iResult;
            }

            // PushPull
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.PUSHPULL)
            {
                iResult = LoadPositionData(bLoadFixed, EPositionObject.PUSHPULL);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bLoadFixed, EPositionObject.PUSHPULL_CENTER1);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bLoadFixed, EPositionObject.PUSHPULL_CENTER2);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner1
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.SPINNER1)
            {
                iResult = LoadPositionData(bLoadFixed, EPositionObject.S1_ROTATE);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bLoadFixed, EPositionObject.S1_CLEAN_NOZZLE);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bLoadFixed, EPositionObject.S1_COAT_NOZZLE);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner2
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.SPINNER2)
            {
                iResult = LoadPositionData(bLoadFixed, EPositionObject.S2_ROTATE);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bLoadFixed, EPositionObject.S2_CLEAN_NOZZLE);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bLoadFixed, EPositionObject.S2_COAT_NOZZLE);
                if (iResult != SUCCESS) return iResult;
            }

            // Handler
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.HANDLER)
            {
                iResult = LoadPositionData(bLoadFixed, EPositionObject.LOWER_HANDLER);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bLoadFixed, EPositionObject.UPPER_HANDLER);
                if (iResult != SUCCESS) return iResult;
            }

            // Stage
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.STAGE1)
            {
                iResult = LoadPositionData(bLoadFixed, EPositionObject.STAGE1);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bLoadFixed, EPositionObject.CAMERA1);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bLoadFixed, EPositionObject.SCANNER1);
                if (iResult != SUCCESS) return iResult;
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

        private void GetTypeHeaderInfo(EListHeaderType type, out List<CListHeader> headerList, out string tableName)
        {
            switch (type)
            {
                case EListHeaderType.MODEL:
                    headerList = ModelHeaderList;
                    tableName = DBInfo.TableModelHeader;
                    break;
                case EListHeaderType.CASSETTE:
                    headerList = CassetteHeaderList;
                    tableName = DBInfo.TableCassetteHeader;
                    break;
                case EListHeaderType.WAFERFRAME:
                    headerList = WaferFrameHeaderList;
                    tableName = DBInfo.TableWaferFrameHeader;
                    break;
                case EListHeaderType.USERINFO:
                default:
                    headerList = UserInfoHeaderList;
                    tableName = DBInfo.TableUserInfoHeader;
                    break;
            }
        }

        private void GetTypeInfo(EListHeaderType type, out List<CListHeader> headerList, out string tableName)
        {
            switch (type)
            {
                case EListHeaderType.MODEL:
                    headerList = ModelHeaderList;
                    tableName = DBInfo.TableModel;
                    break;
                case EListHeaderType.CASSETTE:
                    headerList = CassetteHeaderList;
                    tableName = DBInfo.TableCassette;
                    break;
                case EListHeaderType.WAFERFRAME:
                    headerList = WaferFrameHeaderList;
                    tableName = DBInfo.TableWaferFrame;
                    break;
                case EListHeaderType.USERINFO:
                default:
                    headerList = UserInfoHeaderList;
                    tableName = DBInfo.TableUserInfo;
                    break;
            }
        }

        /// <summary>
        /// UI에서 public 으로 선언된 ModelHeaderList를 편집한 후에 (data 무결성은 UI에서 책임)
        /// 이 함수를 호출하여 ModelHeader List를 저장한다
        /// </summary>
        /// <returns></returns>
        public int SaveModelHeaderList(EListHeaderType type)
        {
            List<CListHeader> headerList;
            string tableName;
            GetTypeHeaderInfo(type, out headerList, out tableName);

            try
            {
                List<string> querys = new List<string>();
                string query;

                // 0. create table
                query = $"CREATE TABLE IF NOT EXISTS {tableName} (name string primary key, data string)";
                querys.Add(query);

                // 1. delete all
                query = $"DELETE FROM {tableName}";
                querys.Add(query);

                // 2. save model list
                string output;
                foreach (CListHeader header in headerList)
                {
                    output = JsonConvert.SerializeObject(header);
                    query = $"INSERT INTO {tableName} VALUES ('{header.Name}', '{output}')";
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

            WriteLog($"success : save {type} header list", ELogType.Debug);
            return SUCCESS;
        }

        public int MakeDefaultModel()
        {
            int iResult;
            bool bStatus = true;

            ////////////////////////////////////////////////////////////////////////////////
            // Model
            EListHeaderType type = EListHeaderType.MODEL;
            // make root folder
            if(IsModelHeaderExist(NAME_ROOT_FOLDER, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetRootFolder();
                ModelHeaderList.Add(header);
                iResult = SaveModelHeaderList(type);
                if (iResult != SUCCESS) return iResult;
            }

            // make default data
            if (IsModelHeaderExist(NAME_DEFAULT_MODEL, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetDefaultModel();
                ModelHeaderList.Add(header);
                iResult = SaveModelHeaderList(type);
                if (iResult != SUCCESS) return iResult;
            }
            if (IsModelExist(NAME_DEFAULT_MODEL, type) == false)
            {
                CModelData data = new CModelData();
                iResult = SaveModelData(data);
                if (iResult != SUCCESS) return iResult;
            }

            ////////////////////////////////////////////////////////////////////////////////
            // UserInfo
            type = EListHeaderType.USERINFO;
            // make root folder
            if (IsModelHeaderExist(NAME_ROOT_FOLDER, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetRootFolder();
                UserInfoHeaderList.Add(header);
                //iResult = SaveModelHeaderList(type);
                //if (iResult != SUCCESS) return iResult;
            }
            // make engineer folder
            if (IsModelHeaderExist(NAME_ENGINEER_FOLDER, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetFolder(NAME_ENGINEER_FOLDER, "", NAME_ROOT_FOLDER);
                UserInfoHeaderList.Add(header);
                //iResult = SaveModelHeaderList(type);
                //if (iResult != SUCCESS) return iResult;
            }
            // make operator folder
            if (IsModelHeaderExist(NAME_OPERATOR_FOLDER, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetFolder(NAME_OPERATOR_FOLDER, "", NAME_ROOT_FOLDER);
                UserInfoHeaderList.Add(header);
                //iResult = SaveModelHeaderList(type);
                //if (iResult != SUCCESS) return iResult;
            }
            iResult = SaveModelHeaderList(type);
            if (iResult != SUCCESS) return iResult;

            // make default operator
            if (IsModelHeaderExist(NAME_DEFAULT_OPERATOR, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetModel(NAME_DEFAULT_OPERATOR, NAME_DEFAULT_OPERATOR, NAME_OPERATOR_FOLDER);
                UserInfoHeaderList.Add(header);
                iResult = SaveModelHeaderList(type);
                if (iResult != SUCCESS) return iResult;
            }
            if (IsModelExist(NAME_DEFAULT_OPERATOR, type) == false)
            {
                CUserInfo data = new CUserInfo(NAME_DEFAULT_OPERATOR, NAME_DEFAULT_OPERATOR, "");
                iResult = SaveModelData(data);
                if (iResult != SUCCESS) return iResult;
            }

            ////////////////////////////////////////////////////////////////////////////////
            // WaferCassette
            type = EListHeaderType.CASSETTE;
            // make root folder
            if (IsModelHeaderExist(NAME_ROOT_FOLDER, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetRootFolder();
                CassetteHeaderList.Add(header);
                iResult = SaveModelHeaderList(type);
                if (iResult != SUCCESS) return iResult;
            }

            // make default data
            if (IsModelHeaderExist(NAME_DEFAULT_MODEL, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetDefaultModel();
                CassetteHeaderList.Add(header);
                iResult = SaveModelHeaderList(type);
                if (iResult != SUCCESS) return iResult;
            }
            if (IsModelExist(NAME_DEFAULT_MODEL, type) == false)
            {
                CWaferCassette data = new CWaferCassette();
                iResult = SaveModelData(data);
                if (iResult != SUCCESS) return iResult;
            }

            ////////////////////////////////////////////////////////////////////////////////
            // WaferFrame
            type = EListHeaderType.WAFERFRAME;
            // make root folder
            if (IsModelHeaderExist(NAME_ROOT_FOLDER, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetRootFolder();
                WaferFrameHeaderList.Add(header);
                iResult = SaveModelHeaderList(type);
                if (iResult != SUCCESS) return iResult;
            }

            // make default data
            if (IsModelHeaderExist(NAME_DEFAULT_MODEL, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetDefaultModel();
                WaferFrameHeaderList.Add(header);
                iResult = SaveModelHeaderList(type);
                if (iResult != SUCCESS) return iResult;
            }
            if (IsModelExist(NAME_DEFAULT_MODEL, type) == false)
            {
                CWaferFrame data = new CWaferFrame();
                iResult = SaveModelData(data);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int LoadModelList()
        {
            LoadModelList(EListHeaderType.MODEL);
            LoadModelList(EListHeaderType.CASSETTE);
            LoadModelList(EListHeaderType.WAFERFRAME);
            LoadModelList(EListHeaderType.USERINFO);

            return SUCCESS;
        }

        public int LoadModelList(EListHeaderType type)
        {
            List<CListHeader> headerList;
            string tableName;
            GetTypeHeaderInfo(type, out headerList, out tableName);

            try
            {
                string query;

                // 0. select table
                query = $"SELECT * FROM {tableName}";

                // 1. get table
                DataTable datatable;
                if (DBManager.GetTable(DBInfo.DBConn, query, out datatable) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST);
                }

                // 2. delete list
                headerList.Clear();

                // 3. get list
                foreach (DataRow row in datatable.Rows)
                {
                    string output = row["data"].ToString();
                    CListHeader header = JsonConvert.DeserializeObject<CListHeader>(output);
                    headerList.Add(header);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST);
            }

            switch (type)
            {
                case EListHeaderType.MODEL:
                    ModelHeaderList = ObjectExtensions.Copy(headerList);
                    break;
                case EListHeaderType.CASSETTE:
                    CassetteHeaderList = ObjectExtensions.Copy(headerList);
                    break;
                case EListHeaderType.WAFERFRAME:
                    WaferFrameHeaderList = ObjectExtensions.Copy(headerList);
                    break;
                case EListHeaderType.USERINFO:
                    UserInfoHeaderList = ObjectExtensions.Copy(headerList);
                    break;
            }

            WriteLog($"success : load {type} header list", ELogType.Debug);
            return SUCCESS;
        }

        public int GetModelHeaderCount(EListHeaderType type)
        {
            List<CListHeader> headerList;
            string tableName;
            GetTypeHeaderInfo(type, out headerList, out tableName);

            int nCount = headerList.Count;
            return nCount;
        }

        public bool IsModelHeaderExist(string name, EListHeaderType type)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            List<CListHeader> headerList;
            string tableName;
            GetTypeHeaderInfo(type, out headerList, out tableName);

            foreach (CListHeader header in headerList)
            {
                if(header.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsModelExist(string name, EListHeaderType type)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            List<CListHeader> headerList;
            string tableName;
            GetTypeInfo(type, out headerList, out tableName);

            try
            {
                // 1. load model
                string output;
                if (DBManager.SelectRow(DBInfo.DBConn, tableName, out output, new CDBColumn("name", name)) == true)
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

        public bool IsModelFolder(string name, EListHeaderType type)
        {
            List<CListHeader> headerList;
            string tableName;
            GetTypeHeaderInfo(type, out headerList, out tableName);

            foreach (CListHeader header in headerList)
            {
                if (header.Name == name)
                {
                    return header.IsFolder;
                }
            }
            return false;
        }

        public int GetModelTreeLevel(string name, EListHeaderType type)
        {
            List<CListHeader> headerList;
            string tableName;
            GetTypeHeaderInfo(type, out headerList, out tableName);

            foreach (CListHeader header in headerList)
            {
                if (header.Name == name)
                {
                    return header.TreeLevel;
                }
            }
            return 0;
        }

        public int DeleteModelHeader(string name, EListHeaderType type)
        {
            List<CListHeader> headerList;
            string tableName;
            GetTypeHeaderInfo(type, out headerList, out tableName);

            if (name == NAME_ROOT_FOLDER) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_ROOT_FOLDER);
            if (name == NAME_DEFAULT_MODEL) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_DEFAULT_MODEL);
            if (IsModelHeaderExist(name, type) == false) return SUCCESS;

            int index = 0;
            foreach (CListHeader header in headerList)
            {
                if (header.Name == name)
                {
                    headerList.RemoveAt(index);
                    break;
                }
                index++;
            }

            int iResult = SaveModelHeaderList(type);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int DeleteModelData(string name, EListHeaderType type)
        {
            List<CListHeader> headerList;
            string tableName;
            GetTypeInfo(type, out headerList, out tableName);

            if (name == NAME_DEFAULT_MODEL) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_DEFAULT_MODEL);
            if (IsModelExist(name, type) == false) return SUCCESS;

            // cannot delete current model
            switch (type)
            {
                case EListHeaderType.MODEL:
                    if (name == SystemData.ModelName) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_CURRENT_MODEL);
                    break;
                case EListHeaderType.CASSETTE:
                    if (name == ModelData.CassetteName) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_CURRENT_MODEL);
                    break;
                case EListHeaderType.WAFERFRAME:
                    if (name == ModelData.WaferFrameName) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_CURRENT_MODEL);
                    break;
                case EListHeaderType.USERINFO:
                    if (name == LoginInfo.User.Name) return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_CURRENT_MODEL);
                    break;
            }

            try
            {
                if (DBManager.DeleteRow(DBInfo.DBConn, tableName, "name", name, true, DBInfo.DBConn_Backup) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_MODEL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_MODEL_DATA);
            }

            WriteLog($"success : delete {type} name : {name}.", ELogType.SYSTEM, ELogWType.SAVE);
            return SUCCESS;
        }

        /// <summary>
        /// Model Data 변경시에 저장 
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public int SaveModelData(CModelData data)
        {
            EListHeaderType type = EListHeaderType.MODEL;
            string tableName = DBInfo.TableModel;
            try
            {
                ModelData = ObjectExtensions.Copy(data);
                string output = JsonConvert.SerializeObject(data);

                if (DBManager.InsertRow(DBInfo.DBConn, tableName, "name", data.Name, output,
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

            WriteLog($"success : save {type} model [{data.Name}].", ELogType.SYSTEM, ELogWType.SAVE);
            return SUCCESS;
        }

        public int SaveModelData(CWaferCassette data)
        {
            EListHeaderType type = EListHeaderType.MODEL;
            string tableName = DBInfo.TableCassette;
            try
            {
                CassetteData = ObjectExtensions.Copy(data);
                string output = JsonConvert.SerializeObject(data);

                if (DBManager.InsertRow(DBInfo.DBConn, tableName, "name", data.Name, output, true, DBInfo.DBConn_Backup) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
            }

            WriteLog($"success : save {type} model [{data.Name}].", ELogType.SYSTEM, ELogWType.SAVE);
            return SUCCESS;
        }

        public int SaveModelData(CWaferFrame data)
        {
            EListHeaderType type = EListHeaderType.WAFERFRAME;
            string tableName = DBInfo.TableWaferFrame;
            try
            {
                WaferFrameData = ObjectExtensions.Copy(data);
                string output = JsonConvert.SerializeObject(data);

                if (DBManager.InsertRow(DBInfo.DBConn, tableName, "name", data.Name, output,
                    true, DBInfo.DBConn_Backup) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
            }

            WriteLog($"success : save {type} model [{data.Name}].", ELogType.SYSTEM, ELogWType.SAVE);
            return SUCCESS;
        }

        public int SaveModelData(CUserInfo data)
        {
            EListHeaderType type = EListHeaderType.USERINFO;
            string tableName = DBInfo.TableUserInfo;
            try
            {
                //ModelData = ObjectExtensions.Copy(data);
                string output = JsonConvert.SerializeObject(data);

                if (DBManager.InsertRow(DBInfo.DBConn, tableName, "name", data.Name, output,
                    true, DBInfo.DBConn_Backup) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
            }

            WriteLog($"success : save {type} model [{data.Name}].", ELogType.SYSTEM, ELogWType.SAVE);
            return SUCCESS;
        }

        public int ChangeModel(string name)
        {
            EListHeaderType type = EListHeaderType.MODEL;
            int iResult;
            // 0. check exist
            if(string.IsNullOrWhiteSpace(name))
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }
            if(IsModelExist(name, type) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            CModelData data = null;
            try
            {
                string output;
                // 1. load model
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableModel, out output, new CDBColumn("name", name)) == true)
                {
                    data = JsonConvert.DeserializeObject<CModelData>(output);
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

                // 3. set data
                if (data != null)
                {
                    ModelData = ObjectExtensions.Copy(data);
                }
                
                // 3.1 load cassette data
                iResult = LoadCassetteData(data.CassetteName);
                if (iResult != SUCCESS) return iResult;

                // 3.2 load waferframe data
                iResult = LoadWaferFrameData(data.WaferFrameName);
                if (iResult != SUCCESS) return iResult;

                WriteLog($"success : change model : {ModelData.Name}.", ELogType.SYSTEM, ELogWType.LOAD);
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            // 3. load model offset position
            iResult = LoadPositionData(false, EPositionGroup.ALL);
            if (iResult != SUCCESS) return iResult;

            // 4. generate model position
            iResult = GenerateModelPosition();
            if (iResult != SUCCESS) return iResult;

            // 5. make model folder
            System.IO.Directory.CreateDirectory(DBInfo.ModelDir+$"\\{name}");

            return SUCCESS;
        }

        public int LoadUserInfo(string name, out CUserInfo data)
        {
            data = new CUserInfo();
            EListHeaderType type = EListHeaderType.USERINFO;
            int iResult;
            // 0. check exist
            if (string.IsNullOrWhiteSpace(name))
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }
            if (IsModelExist(name, type) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            try
            {
                string output;
                // 1.2. load cassette data
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableUserInfo, out output, new CDBColumn("name", name)) == true)
                {
                    data = JsonConvert.DeserializeObject<CUserInfo>(output);
                }
                else
                {
                    //return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            return SUCCESS;
        }

        public int LoadCassetteData(string name)
        {
            EListHeaderType type = EListHeaderType.CASSETTE;
            int iResult;
            // 0. check exist
            if (string.IsNullOrWhiteSpace(name))
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }
            if (IsModelExist(name, type) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            CWaferCassette data = null;
            try
            {
                string output;
                // 1.2. load cassette data
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableCassette, out output, new CDBColumn("name", name)) == true)
                {
                    data = JsonConvert.DeserializeObject<CWaferCassette>(output);
                }
                else
                {
                    //return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
                }

                // 2. finally, set model data
                if (data != null)
                {
                    CassetteData = ObjectExtensions.Copy(data);
                    WriteLog($"success : change {type} : {name}.", ELogType.SYSTEM, ELogWType.LOAD);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            return SUCCESS;
        }

        public int LoadWaferFrameData(string name)
        {
            EListHeaderType type = EListHeaderType.WAFERFRAME;
            int iResult;
            // 0. check exist
            if (string.IsNullOrWhiteSpace(name))
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }
            if (IsModelExist(name, type) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            CWaferFrame data = null;
            try
            {
                string output;
                // 1.3. load waferframe data
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableWaferFrame, out output, new CDBColumn("name", name)) == true)
                {
                    data = JsonConvert.DeserializeObject<CWaferFrame>(output);
                }
                else
                {
                    //return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
                }

                // 2. finally, set model data
                if (data != null)
                {
                    WaferFrameData = ObjectExtensions.Copy(data);
                    WriteLog($"success : change {type} : {name}.", ELogType.SYSTEM, ELogWType.LOAD);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            return SUCCESS;
        }

        public int ViewModelData(string name, out CModelData data)
        {
            data = new CModelData();
            // 0. check exist
            if (IsModelExist(name, EListHeaderType.MODEL) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            try
            {
                // 1. load model
                string output;
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableModel, out output, new CDBColumn("name", name)) == true)
                {
                    data = JsonConvert.DeserializeObject<CModelData>(output);
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

        public int ViewModelData(string name, out CWaferCassette data)
        {
            data = new CWaferCassette();
            // 0. check exist
            if (IsModelExist(name, EListHeaderType.MODEL) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            try
            {
                // 1. load model
                string output;
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableCassette, out output, new CDBColumn("name", name)) == true)
                {
                    data = JsonConvert.DeserializeObject<CWaferCassette>(output);
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

        public int ViewModelData(string name, out CWaferFrame data)
        {
            data = new CWaferFrame();
            // 0. check exist
            if (IsModelExist(name, EListHeaderType.MODEL) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            try
            {
                // 1. load model
                string output;
                if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableWaferFrame, out output, new CDBColumn("name", name)) == true)
                {
                    data = JsonConvert.DeserializeObject<CWaferFrame>(output);
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

        public CLoginInfo GetLogin()
        {
            return LoginInfo;
        }

        public int SetLogin(bool IsSystemStart = false)
        {
            CUserInfo user = new CUserInfo();
            if(IsSystemStart == false)
            {
                if (LoginInfo.User.Name == SystemData.UserName)
                    return SUCCESS;

                LoadUserInfo(SystemData.UserName, out user);

                // 1. logout and save
                LoginInfo.AccessTime = DateTime.Now;
                LoginInfo.AccessType = false;
                SaveLoginHistory(LoginInfo);

                // 2. login and save
                LoginInfo = new CLoginInfo(user);
                LoginInfo.AccessTime = DateTime.Now;
                LoginInfo.AccessType = true;
                SaveLoginHistory(LoginInfo);
            } else
            {
                user.SetMaker();

                // 2. login and save
                LoginInfo = new CLoginInfo(user);
                LoginInfo.AccessTime = DateTime.Now;
                LoginInfo.AccessType = true;
                SaveLoginHistory(LoginInfo);
            }


            DBManager.SetOperator(user.Name, user.Type.ToString());
            WriteLog($"login : {LoginInfo}", ELogType.LOGIN, ELogWType.LOGIN);

            return SUCCESS;
        }
        
        public int SaveLoginHistory(CLoginInfo login)
        {
            // write login history
            string create_query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableLoginHistory} (accesstime datetime, accesstype string, name string, comment string, type string)";
            string query = $"INSERT INTO {DBInfo.TableLoginHistory} VALUES ('{DBManager.DateTimeSQLite(login.AccessTime)}', '{login.GetAccessType()}', '{login.User.Name}', '{login.User.Comment}', '{login.User.Type}')";

            if (DBManager.ExecuteNonQuerys(DBInfo.DBConn_ELog, create_query, query) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_LOGIN_HISTORY);
            }

            return SUCCESS;
        }

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
                query = $"SELECT * FROM {DBInfo.TableAlarmHistory} WHERE occurtime ORDER BY occurtime DESC";

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

            ImportDataFromExcel(EExcel_Sheet.Skip);

            iResult = LoadIOList();
            //if (iResult != SUCCESS) return iResult;

            iResult = LoadAlarmInfoList();
            //if (iResult != SUCCESS) return iResult;

            iResult = LoadMessageInfoList();
            //if (iResult != SUCCESS) return iResult;

            iResult = LoadParameterList();
            //if (iResult != SUCCESS) return iResult;

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
                        CAlarmInfo info = JsonConvert.DeserializeObject<CAlarmInfo>(output);

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

        public int UpdateAlarmInfo(CAlarmInfo info, bool bMerge = true, bool bSaveToDB = true)
        {
            CAlarmInfo prevInfo = new CAlarmInfo();
            for(int i = 0; i < AlarmInfoList.Count; i++)
            {
                if(AlarmInfoList[i].Index == info.Index)
                {
                    prevInfo = ObjectExtensions.Copy(AlarmInfoList[i]);
                    AlarmInfoList.RemoveAt(i);
                    break;
                }
            }

            if(bMerge)
            {
                prevInfo.Update(info);
                AlarmInfoList.Add(prevInfo);
            } else
            {
                AlarmInfoList.Add(info);
            }

            if (bSaveToDB == false) return SUCCESS;
            return SaveAlarmInfoList();
        }

        public int LoadAlarmInfo(int index, out CAlarmInfo info)
        {
            info = new CAlarmInfo();
            info.Index = index;
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
                    return SUCCESS;
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_ALARM_INFO);
            }

            WriteLog($"fail : load alarm info [index = {index}]", ELogType.Debug);
            return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_ALARM_INFO);
        }

        public int LoadMessageInfoList()
        {
            try
            {
                string query;

                // 0. select table
                query = $"SELECT * FROM {DBInfo.TableMessageInfo}";

                // 1. get table
                DataTable datatable;
                if (DBManager.GetTable(DBInfo.DBConn_Info, query, out datatable) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MESSAGE_INFO);
                }

                // 2. delete list
                MessageInfoList.Clear();

                // 3. get list
                foreach (DataRow row in datatable.Rows)
                {
                    int index;
                    if (int.TryParse(row["name"].ToString(), out index))
                    {
                        string output = row["data"].ToString();
                        CMessageInfo info = JsonConvert.DeserializeObject<CMessageInfo>(output);

                        MessageInfoList.Add(info);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MESSAGE_INFO);
            }

            //WriteLog($"success : load message info list", ELogType.Debug);
            return SUCCESS;
        }

        public int UpdateMessageInfo(CMessageInfo info, bool bMerge = true, bool bSaveToDB = true)
        {
            CMessageInfo prevInfo = new CMessageInfo();
            for (int i = 0; i < MessageInfoList.Count; i++)
            {
                if (MessageInfoList[i].Index == info.Index)
                {
                    prevInfo = ObjectExtensions.Copy(MessageInfoList[i]);
                    MessageInfoList.RemoveAt(i);
                    break;
                }
            }

            if (bMerge)
            {
                prevInfo.Update(info);
                MessageInfoList.Add(prevInfo);
            }
            else
            {
                MessageInfoList.Add(info);
            }

            if (bSaveToDB == false) return SUCCESS;
            return SaveMessageInfoList();
        }

        public int LoadMessageInfo(int index, out CMessageInfo info)
        {
            info = new CMessageInfo();
            info.Index = index;
            if (MessageInfoList.Count > 0)
            {
                foreach (CMessageInfo item in MessageInfoList)
                {
                    if (item.Index == index)
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
                if (DBManager.SelectRow(DBInfo.DBConn_Info, DBInfo.TableMessageInfo, out output, new CDBColumn("name", index.ToString())) == true)
                {
                    info = JsonConvert.DeserializeObject<CMessageInfo>(output);
                    return SUCCESS;
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MESSAGE_INFO);
            }

            WriteLog($"fail : load message info [index = {index}]", ELogType.Debug);
            return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MESSAGE_INFO);
        }

        public int LoadMessageInfo(string strMsg, out CMessageInfo info)
        {
            info = new CMessageInfo();
            info.Index = GetNextMessageIndex();
            if (MessageInfoList.Count > 0)
            {
                foreach (CMessageInfo item in MessageInfoList)
                {
                    if (item.IsEqual(strMsg))
                    {
                        info = ObjectExtensions.Copy(item);
                        return SUCCESS;
                    }
                }
            }

            WriteLog($"fail : load message info", ELogType.Debug);
            return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MESSAGE_INFO);
        }

        public int GetNextMessageIndex(int nStartAfter = 0)
        {
            int index = nStartAfter + 1;
            foreach (CMessageInfo item in MessageInfoList)
            {
                if (item.Index > index) index = item.Index + 1;
            }
            return index;
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
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_PARAMETER_INFO);
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
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_PARAMETER_INFO);
            }

            WriteLog($"success : load para info list", ELogType.Debug);
            return SUCCESS;
        }

        public int UpdateParaInfo(CParaInfo info, bool bMerge = true, bool bSaveToDB = true)
        {
            CParaInfo prevInfo = new CParaInfo();
            for (int i = 0; i < ParaInfoList.Count; i++)
            {
                if (ParaInfoList[i].Name == info.Name && ParaInfoList[i].Group == info.Group)
                {
                    prevInfo = ObjectExtensions.Copy(ParaInfoList[i]);
                    ParaInfoList.RemoveAt(i);
                    break;
                }
            }

            if (bMerge)
            {
                prevInfo.Update(info);
                ParaInfoList.Add(prevInfo);
            }
            else
            {
                ParaInfoList.Add(info);
            }

            if (bSaveToDB == false) return SUCCESS;
            return SaveParaInfoList();
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
                    return SUCCESS;
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_PARAMETER_INFO);
            }

            WriteLog($"fail : load para info [group = {info.Group}, name = {info.Name}]", ELogType.Debug);
            return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_PARAMETER_INFO);
        }

        public int SaveGeneralData()
        {
            int iResult;

            iResult = SaveIOList();
            //if (iResult != SUCCESS) return iResult;

            iResult = SaveAlarmInfoList();
            //if (iResult != SUCCESS) return iResult;

            iResult = SaveMessageInfoList();
            //if (iResult != SUCCESS) return iResult;

            iResult = SaveParaInfoList();
            //if (iResult != SUCCESS) return iResult;

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
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_ALARM_INFO);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_ALARM_INFO);
            }

            WriteLog($"success : save alarm info list", ELogType.Debug);
            return SUCCESS;
        }

        public int SaveMessageInfoList()
        {
            try
            {
                List<string> querys = new List<string>();
                string query;

                // 0. create table
                query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableMessageInfo} (name string primary key, data string)";
                querys.Add(query);

                // 1. delete all
                query = $"DELETE FROM {DBInfo.TableMessageInfo}";
                querys.Add(query);

                // 2. save list
                string output;
                foreach (CMessageInfo info in MessageInfoList)
                {
                    output = JsonConvert.SerializeObject(info);
                    query = $"INSERT INTO {DBInfo.TableMessageInfo} VALUES ('{info.Index}', '{output}')";
                    querys.Add(query);
                }

                // 3. execute query
                if (DBManager.ExecuteNonQuerys(DBInfo.DBConn_Info, querys) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MESSAGE_INFO);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MESSAGE_INFO);
            }

            WriteLog($"success : save message info list", ELogType.Debug);
            return SUCCESS;
        }

        public int DeleteInfoTable(string table)
        {
            if(table == DBInfo.TableAlarmInfo)
            {
                AlarmInfoList.Clear();
            } else if (table == DBInfo.TableMessageInfo)
            {
                MessageInfoList.Clear();
            }
            else if (table == DBInfo.TableParameter)
            {
                ParaInfoList.Clear();
            }

            try
            {
                List<string> querys = new List<string>();
                string query;

                // 0. drop table
                query = $"DROP TABLE IF EXISTS {table}";
                querys.Add(query);

                // 3. execute query
                if (DBManager.ExecuteNonQuerys(DBInfo.DBConn_Info, querys) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DROP_TABLES);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DROP_TABLES);
            }

            WriteLog($"success : drop info table", ELogType.Debug);
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
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_PARAMETER_INFO);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_PARAMETER_INFO);
            }

            WriteLog($"success : save para info list", ELogType.Debug);
            return SUCCESS;
        }
               

        public int ImportDataFromExcel(EExcel_Sheet nSheet)
        {
            if(nSheet == EExcel_Sheet.Skip)
            {
                WriteLog($"success : System Parameter Read Skip", ELogType.Debug);
                return SUCCESS;
            }

            int i = 0, j = 0, nSheetCount = 0, nCount = 0;

            string strPath = DBInfo.SystemDir + DBInfo.ExcelSystemPara;
            int iResult;
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

                if(nSheet == EExcel_Sheet.MAX || nSheet == EExcel_Sheet.PARA_Info)
                {
                    // Parameter Info
                    iResult = ImportParaDataFromExcel(SheetRange[(int)EExcel_Sheet.PARA_Info]);
                    if(iResult == SUCCESS)
                    {
                        SaveParaInfoList();
                    }
                }

                if (nSheet == EExcel_Sheet.MAX || nSheet == EExcel_Sheet.Alarm_Info)
                {
                    // Alarm Info
                    iResult = ImportAlarmDataFromExcel(SheetRange[(int)EExcel_Sheet.Alarm_Info]);
                    if (iResult == SUCCESS)
                    {
                        SaveAlarmInfoList();
                    }
                }

                if (nSheet == EExcel_Sheet.MAX || nSheet == EExcel_Sheet.Message_Info)
                {
                    // Message Info
                    iResult = ImportMessageDataFromExcel(SheetRange[(int)EExcel_Sheet.Message_Info]);
                    if (iResult == SUCCESS)
                    {
                        SaveMessageInfoList();
                    }
                }

                if (nSheet == EExcel_Sheet.MAX || nSheet == EExcel_Sheet.IO_Info)
                {
                    // IO 
                    iResult = ImportIODataFromExcel(SheetRange[(int)EExcel_Sheet.IO_Info]);
                    if (iResult == SUCCESS)
                    {
                        SaveIOList();
                    }
                }

                if (nSheet == EExcel_Sheet.MAX || nSheet == EExcel_Sheet.Motor_Data)
                {
                    // Motor Data
                    iResult = ImportMotorDataFromExcel(SheetRange[(int)EExcel_Sheet.Motor_Data]);
                    if (iResult == SUCCESS)
                    {
                        SaveSystemData(systemAxis: SystemData_Axis);
                    }
                }

                WorkBook.Close(true);
                ExcelApp.Quit();
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_IMPORT);
            }

            WriteLog($"success : Import Data from Excel", ELogType.Debug);
            return SUCCESS;
        }

        public int ImportIODataFromExcel(Excel.Range SheetRange)
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
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_IMPORT);
            }

            WriteLog($"success : Import IO Data From Excel", ELogType.Debug);
            return SUCCESS;
        }

        public int ImportMotorDataFromExcel(Excel.Range SheetRange)
        {
            int i = 0;

            try
            {
                // Motor Data Sheet
                for (i = 0; i < 16; i++)
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

                for (i = 0; i < 3; i++)
                {
                    // Speed
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 3] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 4] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 5] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 6] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 7] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 8] as Excel.Range).Text);

                    // Acc
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 9] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 10] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 11] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 12] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 13] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 14] as Excel.Range).Text);

                    // Dec
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 15] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 16] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 17] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 18] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 19] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 20] as Excel.Range).Text);

                    // S/W Limit
                    SystemData_Axis.ACSMotionData[i].PosLimit.Plus = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 21] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].PosLimit.Minus = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 22] as Excel.Range).Text);

                    // Limit Time
                    SystemData_Axis.ACSMotionData[i].TimeLimit.tMoveLimit = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 23] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].TimeLimit.tSleepAfterMove = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 24] as Excel.Range).Text);
                    SystemData_Axis.ACSMotionData[i].TimeLimit.tOriginLimit = Convert.ToDouble((string)(SheetRange.Cells[i + 18, 25] as Excel.Range).Text);

                    // Home Option
                    //SystemData_Axis.ACSMotionData[i].OriginData.Method = Convert.ToInt16((string)(SheetRange.Cells[i + 2, 26] as Excel.Range).Text);
                    //SystemData_Axis.ACSMotionData[i].OriginData.Dir = Convert.ToInt16((string)(SheetRange.Cells[i + 2, 27] as Excel.Range).Text);
                    //SystemData_Axis.ACSMotionData[i].OriginData.FastSpeed = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 28] as Excel.Range).Text);
                    //SystemData_Axis.ACSMotionData[i].OriginData.SlowSpeed = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 29] as Excel.Range).Text);
                    //SystemData_Axis.ACSMotionData[i].OriginData.HomeOffset = Convert.ToDouble((string)(SheetRange.Cells[i + 2, 30] as Excel.Range).Text);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_IMPORT);
            }

            WriteLog($"success : Import Motor Data From Excel", ELogType.Debug);
            return SUCCESS;
        }

        public int ImportAlarmDataFromExcel(Excel.Range SheetRange)
        {
            int nRowCount = SheetRange.EntireRow.Count;
            int i = 0;
            string strGroup;

            try
            {
                //AlarmInfoList.Clear();

                for (i = 0; i < nRowCount - 1; i++)
                {
                    CAlarmInfo AlarmInfo = new CAlarmInfo();
                                      
                    strGroup = (string)(SheetRange.Cells[i + 2, 1] as Excel.Range).Value2; // Group

                    if (strGroup == Convert.ToString(EAlarmGroup.SYSTEM)) AlarmInfo.Group = EAlarmGroup.SYSTEM;
                    if (strGroup == Convert.ToString(EAlarmGroup.LOADER)) AlarmInfo.Group = EAlarmGroup.LOADER;
                    if (strGroup == Convert.ToString(EAlarmGroup.PUSHPULL)) AlarmInfo.Group = EAlarmGroup.PUSHPULL;
                    if (strGroup == Convert.ToString(EAlarmGroup.HANDLER)) AlarmInfo.Group = EAlarmGroup.HANDLER;
                    if (strGroup == Convert.ToString(EAlarmGroup.SPINNER)) AlarmInfo.Group = EAlarmGroup.SPINNER;
                    if (strGroup == Convert.ToString(EAlarmGroup.STAGE)) AlarmInfo.Group = EAlarmGroup.STAGE;
                    if (strGroup == Convert.ToString(EAlarmGroup.SCANNER)) AlarmInfo.Group = EAlarmGroup.SCANNER;
                    if (strGroup == Convert.ToString(EAlarmGroup.LASER)) AlarmInfo.Group = EAlarmGroup.LASER;

                    AlarmInfo.Esc = (string)(SheetRange.Cells[i + 2, 2] as Excel.Range).Value2; // ESC

                    AlarmInfo.Index = (int)(SheetRange.Cells[i + 2, 3] as Excel.Range).Value2; // Index

                    switch (((int)(SheetRange.Cells[i + 2, 4] as Excel.Range).Value2)) // Type
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

                    AlarmInfo.Description[(int)DEF_Common.ELanguage.KOREAN] = (string)(SheetRange.Cells[i + 2, 5] as Excel.Range).Value2; // Description Kor
                    AlarmInfo.Description[(int)DEF_Common.ELanguage.ENGLISH] = (string)(SheetRange.Cells[i + 2, 6] as Excel.Range).Value2; // Description Eng
                    AlarmInfo.Description[(int)DEF_Common.ELanguage.CHINESE] = (string)(SheetRange.Cells[i + 2, 7] as Excel.Range).Value2; // Description Chn
                    AlarmInfo.Description[(int)DEF_Common.ELanguage.JAPANESE] = (string)(SheetRange.Cells[i + 2, 8] as Excel.Range).Value2; // Description Jpn

                    AlarmInfo.Solution[(int)DEF_Common.ELanguage.KOREAN] = (string)(SheetRange.Cells[i + 2, 9] as Excel.Range).Value2; // Solution Kor
                    AlarmInfo.Solution[(int)DEF_Common.ELanguage.ENGLISH] = (string)(SheetRange.Cells[i + 2, 10] as Excel.Range).Value2; // Solution Eng
                    AlarmInfo.Solution[(int)DEF_Common.ELanguage.CHINESE] = (string)(SheetRange.Cells[i + 2, 11] as Excel.Range).Value2; // Solution Chn
                    AlarmInfo.Solution[(int)DEF_Common.ELanguage.JAPANESE] = (string)(SheetRange.Cells[i + 2, 12] as Excel.Range).Value2; // Solution Jpn

                    //AlarmInfoList.Add(AlarmInfo);
                    UpdateAlarmInfo(AlarmInfo, false, false);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_IMPORT);
            }

            WriteLog($"success : Import Alarm Info From Excel", ELogType.Debug);
            return SUCCESS;
        }

        public int ImportMessageDataFromExcel(Excel.Range SheetRange)
        {
            int nCount = SheetRange.EntireRow.Count, i;

            try
            {
                //MessageInfoList.Clear();

                for (i = 0; i < nCount - 1; i++)
                {
                    CMessageInfo MessageInfo = new CMessageInfo();

                    // Index
                    MessageInfo.Index = (int)(SheetRange.Cells[i + 2, 1] as Excel.Range).Value2;

                    // Type
                    if ((string)(SheetRange.Cells[i + 2, 2] as Excel.Range).Value2 == Convert.ToString(EMessageType.OK)) MessageInfo.Type = EMessageType.OK;
                    if ((string)(SheetRange.Cells[i + 2, 2] as Excel.Range).Value2 == Convert.ToString(EMessageType.OK_Cancel)) MessageInfo.Type = EMessageType.OK_Cancel;
                    if ((string)(SheetRange.Cells[i + 2, 2] as Excel.Range).Value2 == Convert.ToString(EMessageType.Confirm_Cancel)) MessageInfo.Type = EMessageType.Confirm_Cancel;

                    // Message
                    MessageInfo.Message[(int)DEF_Common.ELanguage.KOREAN] = (string)(SheetRange.Cells[i + 2, 3] as Excel.Range).Value2;
                    MessageInfo.Message[(int)DEF_Common.ELanguage.ENGLISH] = (string)(SheetRange.Cells[i + 2, 4] as Excel.Range).Value2;
                    MessageInfo.Message[(int)DEF_Common.ELanguage.CHINESE] = (string)(SheetRange.Cells[i + 2, 5] as Excel.Range).Value2;
                    MessageInfo.Message[(int)DEF_Common.ELanguage.JAPANESE] = (string)(SheetRange.Cells[i + 2, 6] as Excel.Range).Value2;

                    //MessageInfoList.Add(MessageInfo);
                    UpdateMessageInfo(MessageInfo, false, false);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_IMPORT);
            }

            WriteLog($"success : Import Message Info From Excel", ELogType.Debug);
            return SUCCESS;
        }

        public int ImportParaDataFromExcel(Excel.Range SheetRange)
        {
            int nRowCount = SheetRange.EntireRow.Count;
            int i = 0, Type = 0;

            try
            {
                //ParaInfoList.Clear();

                for (i = 0; i < nRowCount - 1; i++)
                {
                    CParaInfo ParaInfo = new CParaInfo();

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

                    //ParaInfoList.Add(ParaInfo);
                    UpdateParaInfo(ParaInfo, false, false);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_IMPORT);
            }

            WriteLog($"success : Import Para Info From Excel", ELogType.Debug);
            return SUCCESS;
        }

        public int ExportDataToExcel(EExcel_Sheet nSheet)
        {
            int iResult = -1;
            if (nSheet == EExcel_Sheet.MAX || nSheet == EExcel_Sheet.PARA_Info)
            {
                // Parameter Info
                iResult = ExportParaDataToExcel();
            }

            if (nSheet == EExcel_Sheet.MAX || nSheet == EExcel_Sheet.Alarm_Info)
            {
                // Alarm Info
                iResult = ExportAlarmInfoToExcel();
            }

            if (nSheet == EExcel_Sheet.MAX || nSheet == EExcel_Sheet.Motor_Data)
            {
                // Motor Data
                iResult = ExportMotorDataToExcel();
            }

            if (nSheet == EExcel_Sheet.MAX || nSheet == EExcel_Sheet.Message_Info)
            {
                // Message Info
                iResult = ExportMsgInfoToExcel();
            }

            return iResult;
        }


        public int ExportAlarmInfoToExcel()
        {
            int i = 0, j = 0, nSheetCount = 0, nCount = 0;

            string strPath = DBInfo.SystemDir + DBInfo.ExcelSystemPara, strGroup=string.Empty;
            int iResult;

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
                    if (AlarmInfoList[i].Group == EAlarmGroup.SYSTEM) strGroup = Convert.ToString(EAlarmGroup.SYSTEM);
                    if (AlarmInfoList[i].Group == EAlarmGroup.LOADER) strGroup = Convert.ToString(EAlarmGroup.LOADER);
                    if (AlarmInfoList[i].Group == EAlarmGroup.PUSHPULL) strGroup = Convert.ToString(EAlarmGroup.PUSHPULL);
                    if (AlarmInfoList[i].Group == EAlarmGroup.HANDLER) strGroup = Convert.ToString(EAlarmGroup.HANDLER);
                    if (AlarmInfoList[i].Group == EAlarmGroup.SPINNER) strGroup = Convert.ToString(EAlarmGroup.SPINNER);
                    if (AlarmInfoList[i].Group == EAlarmGroup.STAGE) strGroup = Convert.ToString(EAlarmGroup.STAGE);
                    if (AlarmInfoList[i].Group == EAlarmGroup.SCANNER) strGroup = Convert.ToString(EAlarmGroup.SCANNER);
                    if (AlarmInfoList[i].Group == EAlarmGroup.LASER) strGroup = Convert.ToString(EAlarmGroup.LASER);

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 1] as Excel.Range).Value2 = strGroup;

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 2] as Excel.Range).Value2 = AlarmInfoList[i].Esc;

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 3] as Excel.Range).Value2 = AlarmInfoList[i].Index;

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 4] as Excel.Range).Value2 = AlarmInfoList[i].Type;

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 5] as Excel.Range).Value2 = AlarmInfoList[i].Description[(int)DEF_Common.ELanguage.KOREAN];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 6] as Excel.Range).Value2 = AlarmInfoList[i].Description[(int)DEF_Common.ELanguage.ENGLISH];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 7] as Excel.Range).Value2 = AlarmInfoList[i].Description[(int)DEF_Common.ELanguage.CHINESE];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 8] as Excel.Range).Value2 = AlarmInfoList[i].Description[(int)DEF_Common.ELanguage.JAPANESE];

                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 9] as Excel.Range).Value2 = AlarmInfoList[i].Solution[(int)DEF_Common.ELanguage.KOREAN];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 10] as Excel.Range).Value2 = AlarmInfoList[i].Solution[(int)DEF_Common.ELanguage.ENGLISH];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 11] as Excel.Range).Value2 = AlarmInfoList[i].Solution[(int)DEF_Common.ELanguage.CHINESE];
                    (SheetRange[(int)EExcel_Sheet.Alarm_Info].Cells[i + 2, 12] as Excel.Range).Value2 = AlarmInfoList[i].Solution[(int)DEF_Common.ELanguage.JAPANESE];
                }

                WorkBook.Save();
                WorkBook.Close(true);
                ExcelApp.Quit();

            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_EXPORT);
            }

            WriteLog($"success : Export Alarm Info Data to Excel", ELogType.Debug);
            return SUCCESS;
        }

        public int ExportParaDataToExcel()
        {
            int i = 0, j = 0, nSheetCount = 0, nCount = 0;

            string strPath = DBInfo.SystemDir + DBInfo.ExcelSystemPara;
            int iResult;
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
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 1] as Excel.Range).Value2 = ParaInfoList[i].Group;
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 2] as Excel.Range).Value2 = ParaInfoList[i].Name;
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 3] as Excel.Range).Value2 = ParaInfoList[i].Unit;
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 4] as Excel.Range).Value2 = ParaInfoList[i].Type;

                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 5] as Excel.Range).Value2 = ParaInfoList[i].DisplayName[(int)DEF_Common.ELanguage.KOREAN];
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 6] as Excel.Range).Value2 = ParaInfoList[i].DisplayName[(int)DEF_Common.ELanguage.ENGLISH];
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 7] as Excel.Range).Value2 = ParaInfoList[i].DisplayName[(int)DEF_Common.ELanguage.CHINESE];
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 8] as Excel.Range).Value2 = ParaInfoList[i].DisplayName[(int)DEF_Common.ELanguage.JAPANESE];

                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 9] as Excel.Range).Value2 = ParaInfoList[i].Description[(int)DEF_Common.ELanguage.KOREAN];
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 10] as Excel.Range).Value2 = ParaInfoList[i].Description[(int)DEF_Common.ELanguage.ENGLISH];
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 11] as Excel.Range).Value2 = ParaInfoList[i].Description[(int)DEF_Common.ELanguage.CHINESE];
                    (SheetRange[(int)EExcel_Sheet.PARA_Info].Cells[i + 2, 12] as Excel.Range).Value2 = ParaInfoList[i].Description[(int)DEF_Common.ELanguage.JAPANESE];
                }

                WorkBook.Save();
                WorkBook.Close(true);
                ExcelApp.Quit();

            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_EXPORT);
            }

            WriteLog($"success : Export Parameter Data to Excel", ELogType.Debug);
            return SUCCESS;

        }

        public int ExportMotorDataToExcel()
        {
            int i = 0, j = 0, nSheetCount = 0, nCount = 0;
            string strPath = DBInfo.SystemDir + DBInfo.ExcelSystemPara;
            Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook WorkBook;        

            try
            {
                WorkBook = ExcelApp.Workbooks.Open(strPath);

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
                for (i = 0; i < 16; i++)
                {
                    // Speed
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 3] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 4] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 5] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 6] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 7] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 8] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel;

                    // Acc
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 9] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 10] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 11] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 12] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 13] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 14] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc;

                    // Dec
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 15] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 16] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 17] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 18] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 19] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 20] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec;

                    // S/W Limit
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 21] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].PosLimit.Plus;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 22] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].PosLimit.Minus;

                    // Limit Time
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 23] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].TimeLimit.tMoveLimit;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 24] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].TimeLimit.tSleepAfterMove;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 25] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].TimeLimit.tOriginLimit;

                    // Home Option
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 26] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.Method;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 27] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.Dir;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 28] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.FastSpeed;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 29] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.SlowSpeed;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 2, 30] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.HomeOffset;
                }

                for (i = 0; i < 3; i++)
                {
                    // Speed
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 3] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 4] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 5] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 6] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 7] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 8] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel;
                                                                        
                    // Acc                                              
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 9] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 10] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 11] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 12] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 13] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 14] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc;
                                                                        
                    // Dec                                              
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 15] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 16] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 17] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 18] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 19] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 20] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec;
                                                                        
                    // S/W Limit                                        
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 21] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].PosLimit.Plus;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 22] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].PosLimit.Minus;
                                                                        
                    // Limit Time                                       
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 23] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].TimeLimit.tMoveLimit;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 24] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].TimeLimit.tSleepAfterMove;
                    (SheetRange[(int)EExcel_Sheet.Motor_Data].Cells[i + 18, 25] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].TimeLimit.tOriginLimit;
                }

                WorkBook.Save();
                WorkBook.Close(true);
                ExcelApp.Quit();

            }
            catch (Exception ex)
            {
                ExcelApp.Quit();
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_EXPORT);
            }

            WriteLog($"success : Export Motor Data to Excel", ELogType.Debug);
            return SUCCESS;
        }

        public int ExportMsgInfoToExcel()
        {
            int i = 0, j = 0, nSheetCount = 0, nCount = 0;

            string strPath = DBInfo.SystemDir + DBInfo.ExcelSystemPara;

            Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook WorkBook;

            try
            {
                WorkBook = ExcelApp.Workbooks.Open(strPath);

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

                nCount = MessageInfoList.Count;

                // Message Info
                for(i=0;i<nCount;i++)
                {
                    (SheetRange[(int)EExcel_Sheet.Message_Info].Cells[i + 2, 1] as Excel.Range).Value2 = MessageInfoList[i].Index;

                    if (MessageInfoList[i].Type == EMessageType.OK) (SheetRange[(int)EExcel_Sheet.Message_Info].Cells[i + 2, 2] as Excel.Range).Value2 = Convert.ToString(EMessageType.OK);
                    if (MessageInfoList[i].Type == EMessageType.OK_Cancel) (SheetRange[(int)EExcel_Sheet.Message_Info].Cells[i + 2, 2] as Excel.Range).Value2 = Convert.ToString(EMessageType.OK_Cancel);
                    if (MessageInfoList[i].Type == EMessageType.Confirm_Cancel) (SheetRange[(int)EExcel_Sheet.Message_Info].Cells[i + 2, 2] as Excel.Range).Value2 = Convert.ToString(EMessageType.Confirm_Cancel);

                    (SheetRange[(int)EExcel_Sheet.Message_Info].Cells[i + 2, 3] as Excel.Range).Value2 = MessageInfoList[i].Message[(int)DEF_Common.ELanguage.KOREAN];
                    (SheetRange[(int)EExcel_Sheet.Message_Info].Cells[i + 2, 4] as Excel.Range).Value2 = MessageInfoList[i].Message[(int)DEF_Common.ELanguage.ENGLISH];
                    (SheetRange[(int)EExcel_Sheet.Message_Info].Cells[i + 2, 5] as Excel.Range).Value2 = MessageInfoList[i].Message[(int)DEF_Common.ELanguage.CHINESE];
                    (SheetRange[(int)EExcel_Sheet.Message_Info].Cells[i + 2, 6] as Excel.Range).Value2 = MessageInfoList[i].Message[(int)DEF_Common.ELanguage.JAPANESE];
                }

                WorkBook.Save();
                WorkBook.Close(true);
                ExcelApp.Quit();

            }
            catch (Exception ex)
            {
                ExcelApp.Quit();
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_EXCEL_EXPORT);
            }

            WriteLog($"success : Export Motor Data to Excel", ELogType.Debug);
            return SUCCESS;
        }

        void InitACSMotionData()
        {
            // Excel에서 읽어오는 방식도 생각해봤으나, 축 이름과 필수적인것들만 초기화하면 될것 같아서 소스코드 내부에서 처리 
            int index;
            CACSMotionData tMotion;

            // null check
            for (int i = 0; i < SystemData_Axis.ACSMotionData.Length; i++)
            {
                if (SystemData_Axis.ACSMotionData[i] == null)
                {
                    SystemData_Axis.ACSMotionData[i] = new CACSMotionData();
                }
            }

            // STAGE X
            index = (int)EACS_Axis.STAGE1_X;
            if (SystemData_Axis.ACSMotionData[index].Name == "NotExist")
            {
                tMotion = new CACSMotionData();
                tMotion.Name = "STAGE1_X";
                tMotion.Exist = true;

                SystemData_Axis.ACSMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // STAGE Y
            index = (int)EACS_Axis.STAGE1_Y;
            if (SystemData_Axis.ACSMotionData[index].Name == "NotExist")
            {
                tMotion = new CACSMotionData();
                tMotion.Name = "STAGE1_Y";
                tMotion.Exist = true;

                SystemData_Axis.ACSMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // STAGE X
            index = (int)EACS_Axis.STAGE1_T;
            if (SystemData_Axis.ACSMotionData[index].Name == "NotExist")
            {
                tMotion = new CACSMotionData();
                tMotion.Name = "STAGE1_T";
                tMotion.Exist = true;

                SystemData_Axis.ACSMotionData[index] = ObjectExtensions.Copy(tMotion);
            }
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

            // yaskawa api call 할 때, Name이 8자 제한때문에 축약해서 이름 사용함.
            // LOADER_Z
            index = (int)EYMC_Axis.LOADER_Z         ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "LD_Z"; //"LOADER_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // PUSHPULL_Y
            index = (int)EYMC_Axis.PUSHPULL_Y       ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "PP_Y"; //"PUSHPULL_Y";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // PUSHPULL_X1   
            index = (int)EYMC_Axis.PUSHPULL_X1   ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "PP_X1"; //"PUSHPULL_X1";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // S1_ROTATE_T
            index = (int)EYMC_Axis.S1_CHUCK_ROTATE_T;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "S1_R_T"; //"C1_CHUCK_ROTATE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // S1_CLEAN_NOZZLE_T
            index = (int)EYMC_Axis.S1_CLEAN_NOZZLE_T;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "S1_CL_T"; //"C1_CLEAN_NOZZLE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // S1_COAT_NOZZLE_T 
            index = (int)EYMC_Axis.S1_COAT_NOZZLE_T ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "S1_CO_T"; //"C1_COAT_NOZZLE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // PUSHPULL_X2   
            index = (int)EYMC_Axis.PUSHPULL_X2;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "PP_X2"; //"PUSHPULL_X2";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // S2_ROTATE_T
            index = (int)EYMC_Axis.S2_CHUCK_ROTATE_T;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "S2_R_T"; //"C2_CHUCK_ROTATE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // S2_CLEAN_NOZZLE_T
            index = (int)EYMC_Axis.S2_CLEAN_NOZZLE_T;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "S2_CL_T"; //"C2_CLEAN_NOZZLE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // S2_COAT_NOZZLE_T 
            index = (int)EYMC_Axis.S2_COAT_NOZZLE_T;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "S2_CO_T"; //"C2_COAT_NOZZLE_T";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // UPPER_HANDLER_X       
            index = (int)EYMC_Axis.UPPER_HANDLER_X       ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "UH_X"; // "UPPER_HANDLER_X";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // UPPER_HANDLER_Z       
            index = (int)EYMC_Axis.UPPER_HANDLER_Z       ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "UH_Z"; // "UPPER_HANDLER_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // LOWER_HANDLER_X       
            index = (int)EYMC_Axis.LOWER_HANDLER_X;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "LH_X"; // "LOWER_HANDLER_X";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // LOWER_HANDLER_Z       
            index = (int)EYMC_Axis.LOWER_HANDLER_Z       ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "LH_Z"; // "LOWER_HANDLER_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // CAMERA1_Z                                           
            index = (int)EACS_Axis.SCANNER_Z1        ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "CAM1_Z"; // "CAMERA1_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // SCANNER1_Z
            index = (int)EACS_Axis.CAMERA_Z         ;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "SCAN1_Z"; // "SCANNER1_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }
        }

        public int LoadWorkPieceFromCassette()
        {
            //WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL] = new CWorkPiece();
            WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL].Init(true);
            WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL].LoadFromCassette();

            return SUCCESS;
        }

        public int LoadWorkPieceToCassette()
        {
            WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL].LoadToCassette();
            WorkPieceList_Finished.Add(WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL]);
            //WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL] = new CWorkPiece();
            WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL].Init(false);

            return SUCCESS;
        }

        public int ChangeWorkPieceUnit(ELCNetUnitPos from, ELCNetUnitPos to)
        {
            WorkPieceArray[(int)to] = ObjectExtensions.Copy(WorkPieceArray[(int)from]);
            WorkPieceArray[(int)from].Init();
            return SUCCESS;
        }

        public int StartWorkPiecePhase(ELCNetUnitPos pos, EProcessPhase phase)
        {
            Sleep(600); // for test
            WorkPieceArray[(int)pos].StartPhase(phase);
            return SUCCESS;
        }

        /// <summary>
        /// 해당 Unit에서 작업 공정의 종료시에 호출하는 함수
        /// StartWorkPiecePhase와 글자수를 같게 하기위해서 일부러 끝에 e를 빼서 글자수를 맞췄음.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="phase"></param>
        /// <returns></returns>
        public int FinishWorkPiecePhas(ELCNetUnitPos pos, EProcessPhase phase)
        {
            Sleep(600); // for test
            WorkPieceArray[(int)pos].FinishPhase(phase);
            return SUCCESS;
        }

        public int GetActiveWorkPieceCount(bool includePushPull)
        {
            int count = 0;
            for (int i = 0; i < WorkPieceArray.Length; i++)
            {
                if (includePushPull == false && i == (int)ELCNetUnitPos.PUSHPULL)
                    continue;
                if (WorkPieceArray[i].ID != "") count++;
            }
            return count;
        }
    }
}
