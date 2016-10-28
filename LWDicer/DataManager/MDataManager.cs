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
using System.Diagnostics;

using Excel = Microsoft.Office.Interop.Excel;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_OpPanel;
using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_DataManager;
using static LWDicer.Layers.DEF_LCNet;

using static LWDicer.Layers.DEF_ACS;
using static LWDicer.Layers.DEF_Yaskawa;
using static LWDicer.Layers.DEF_Motion;
using static LWDicer.Layers.DEF_Cylinder;
using static LWDicer.Layers.DEF_Vacuum;

using static LWDicer.Layers.DEF_MeStage;
using static LWDicer.Layers.DEF_MeHandler;
using static LWDicer.Layers.DEF_MeElevator;
using static LWDicer.Layers.DEF_MePushPull;
using static LWDicer.Layers.DEF_MeSpinner;
using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_CtrlSpinner;


namespace LWDicer.Layers
{
    public class DEF_DataManager
    {
        public const int ERR_DATA_MANAGER_FAIL_BACKUP_DB                = 1;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_DB                = 2;
        public const int ERR_DATA_MANAGER_FAIL_DROP_TABLES              = 3;
        public const int ERR_DATA_MANAGER_FAIL_BACKUP_ROW               = 4;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA        = 5;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA        = 6;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA         = 7;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA         = 8;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_POSITION_DATA       = 9;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_POSITION_DATA       = 10;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MODEL_DATA          = 11;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA          = 12;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MODEL_LIST          = 13;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_MODEL_DATA        = 14;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST          = 15;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_LOGIN_HISTORY       = 16;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_LOGIN_HISTORY       = 17;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_ALARM_INFO          = 18;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_ALARM_INFO          = 19;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MESSAGE_INFO        = 20;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MESSAGE_INFO        = 21;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_PARAMETER_INFO      = 22;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_PARAMETER_INFO      = 23;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_ALARM_HISTORY       = 24;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_ALARM_HISTORY       = 25;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_ROOT_FOLDER       = 26;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_DEFAULT_MODEL     = 27;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_CURRENT_MODEL     = 28;
        public const int ERR_DATA_MANAGER_FAIL_EXCEL_IMPORT             = 29;
        public const int ERR_DATA_MANAGER_FAIL_EXCEL_EXPORT             = 30;
        public const int ERR_DATA_MANAGER_INPUT_BUFFER_EMPTY            = 31;

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

            // SafetyPos for Axis Move 
            // Teching 화면에서 Teaching하는 UnitPos.WaitPos 과는 다른 용도로, make engineer가 
            // 시스템적으로 지정하는 절대 안전 위치
            public CSystemData_MAxSafetyPos MAxSafetyPos = new CSystemData_MAxSafetyPos();

            // thread 간의 handshake를 one step으로 처리할지 여부
            public bool ThreadHandshake_byOneStep = true;

            // spinner
            public bool UseSpinnerSeparately = true;  // spinner를 coater, cleaner로 구분하여 사용할지 여부
            public ESpinnerIndex UCoaterIndex = ESpinnerIndex.SPINNER1;  // spinner를 구분지어 사용할 때, coater의 spinner index
            public ESpinnerIndex UCleanerIndex = ESpinnerIndex.SPINNER2; // spinner를 구분지어 사용할 때, cleaner의 spinner index

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
            public EVelocityMode VelocityMode;
            public double PanelBacklash;
            public bool UseOnLineUse;

            public bool UseInSfaTest;                // SFA 내에서 Test할때 쓰임
            public bool UseDisplayQuitButton;
            
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
            public int Pumping_OneShot_Interval;    // 매 일회 펌핑 동작당 대기시간
            public bool UseUseWorkbenchVacuum;      // Workbench Vacuum 사용 유무
            
            // Dispenser측, MMC에서 제어하는 cylinder time
            public double Head_Cyl_MovingTime;
            public double Head_Cyl_AfterOnTime;
            public double Head_Cyl_AfterOffTime;
            public double Head_Cyl_NoSensorWaitTime;

            public double Head_Gun_UV_InterGap;       // from Needle to UV End distance
            public bool UseCheck_Panel_Data;          // run time, check panel data
            public bool UseCheck_Panel_History;       // run time, check panel id History

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

        public class CSystemData_Align
        {
            // Vision ===========================================================================

            public double[] LenMagnification = new double[(int)ECameraSelect.MAX];
            // 렌즈 Resolution & 카메라 Position
            public double[] CamPixelSize = new double[(int)ECameraSelect.MAX];
            public int[] CamPixelNumX = new int[(int)ECameraSelect.MAX];
            public int[] CamPixelNumY = new int[(int)ECameraSelect.MAX];
            public double[] PixelResolution = new double[(int)ECameraSelect.MAX];

            public double[] CamFovX = new double[(int)ECameraSelect.MAX];    // 이 수치는 자동 계산됨
            public double[] CamFovY = new double[(int)ECameraSelect.MAX];    // 이 수치는 자동 계산됨

            public CPos_XY[] Position   = new CPos_XY[(int)ECameraSelect.MAX];
            public double[] CameraTilt  = new double[(int)ECameraSelect.MAX];
            public CCameraData[] Camera = new CCameraData[(int)ECameraSelect.MAX];

            public double CamEachOffsetX;
            public double CamEachOffsetY;

            // Edge Align
            public double WaferOffsetX;
            public double WaferOffsetY;
            public double WaferSizeOffset;
            

            // Stage ===========================================================================

            // Index Move Length
            public double DieIndexWidth;
            public double DieIndexHeight;
            public double DieIndexRotate;
            
            // Screen Move Length 
            public double MacroScreenWidth;
            public double MacroScreenHeight;
            public double MacroScreenRotate;
            public double MicroScreenWidth;
            public double MicroScreenHeight;
            public double MicroScreenRotate;
            

            public double AlignMarkWidthLen;
            public double AlignMarkWidthRatio;

            public CSystemData_Align()
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

        /// <summary>
        /// 시스템의 전체 좌표셋. ( Pos_Fixed, Pos_Model, Pos_Offset 세 종류로 사용됨)
        /// </summary>
        public class CPositionGroup
        {
            // total : 실체는 각각의 이름으로 지정되어 있지만, 연산하기 편하도록 배열로도 관리할 수 있도록 함
            public CPositionSet[] Pos_Array          = new CPositionSet[(int)EPositionObject.MAX];
            
            // Loader
            public CPositionSet Pos_Loader           = new CPositionSet((int)EElevatorPos.MAX);

            // PushPull
            public CPositionSet Pos_PushPull         = new CPositionSet((int)EPushPullPos.MAX);
            public CPositionSet Pos_PushPull_Center1 = new CPositionSet((int)ECenterPos.MAX);
            public CPositionSet Pos_PushPull_Center2 = new CPositionSet((int)ECenterPos.MAX);

            // Spinner1
            public CPositionSet Pos_S1_Rotate        = new CPositionSet((int)ERotatePos.MAX);
            public CPositionSet Pos_S1_CoatNozzle    = new CPositionSet((int)ENozzlePos.MAX);
            public CPositionSet Pos_S1_CleanNozzle   = new CPositionSet((int)ENozzlePos.MAX);

            // Spinner2
            public CPositionSet Pos_S2_Rotate        = new CPositionSet((int)ERotatePos.MAX);
            public CPositionSet Pos_S2_CoatNozzle    = new CPositionSet((int)ENozzlePos.MAX);
            public CPositionSet Pos_S2_CleanNozzle   = new CPositionSet((int)ENozzlePos.MAX);

            // Handler
            public CPositionSet Pos_UpperHandler     = new CPositionSet((int)EHandlerPos.MAX);
            public CPositionSet Pos_LowerHandler     = new CPositionSet((int)EHandlerPos.MAX);

            // Stage
            public CPositionSet Pos_Stage1           = new CPositionSet((int)EStagePos.MAX);
            public CPositionSet Pos_Camera1          = new CPositionSet((int)ECameraPos.MAX);
            public CPositionSet Pos_Scanner1         = new CPositionSet((int)EScannerPos.MAX);

            public CPositionGroup()
            {
                int index = 0;
                Pos_Array[index++] = Pos_Loader;
                Pos_Array[index++] = Pos_PushPull;
                Pos_Array[index++] = Pos_PushPull_Center1;
                Pos_Array[index++] = Pos_PushPull_Center2;
                Pos_Array[index++] = Pos_S1_Rotate;
                Pos_Array[index++] = Pos_S1_CoatNozzle;
                Pos_Array[index++] = Pos_S1_CleanNozzle;
                Pos_Array[index++] = Pos_S2_Rotate;
                Pos_Array[index++] = Pos_S2_CoatNozzle;
                Pos_Array[index++] = Pos_S2_CleanNozzle;
                Pos_Array[index++] = Pos_UpperHandler;
                Pos_Array[index++] = Pos_LowerHandler;
                Pos_Array[index++] = Pos_Stage1;
                Pos_Array[index++] = Pos_Camera1;
                Pos_Array[index++] = Pos_Scanner1;

                for (int i=0; i< (int)EPositionObject.MAX;i++)
                {
                    for (int j=0; j < Pos_Array[i].Pos.Length; j++)
                    {
                        Pos_Array[i].Pos[j] = new CPos_XYTZ();
                    }
                }
            }

            public void UpdatePositionSet(CPositionGroup tGroup, EPositionObject index)
            {
                Pos_Array[(int)index] = ObjectExtensions.Copy(tGroup.Pos_Array[(int)index]);
            }

            public void UpdatePositionSet(CPositionSet tSet, EPositionObject index)
            {
                Pos_Array[(int)index] = ObjectExtensions.Copy(tSet);
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
            public int SlotNumber;           // 슬롯갯수            ex) 13ea
            public int[] SlotStatus = new int[CASSETTE_MAX_SLOT_NUM]; // 각 슬롯 상태 및 Wafer 처리여부 데이터
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
        public const string NAME_ROOT_FOLDER            = "root";
        public const string NAME_DEFAULT_MODEL          = "default";
        //public const string NAME_ENGINEER_FOLDER      = "Engineer";
        //public const string NAME_OPERATOR_FOLDER      = "Operator";
        public const string NAME_DEFAULT_OPERATOR       = "Operator";
        public const string NAME_MAKER                  = "Maker";


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
            public CCtrlSpinnerData[] SpinnerData = new CCtrlSpinnerData[(int)ESpinnerIndex.MAX];


            ///////////////////////////////////////////////////////////
            // Vision Data (Pattern)
            public CSearchData MacroPatternA = new CSearchData();
            public CSearchData MacroPatternB = new CSearchData();
            public CSearchData MicroPatternA = new CSearchData();
            public CSearchData MicroPatternB = new CSearchData();

            ///////////////////////////////////////////////////////////
            // Edge Aling Position
            public CPos_XYTZ[] EdgeTeachPos = new CPos_XYTZ[3];

            ///////////////////////////////////////////////////////////
            // Scanner Parameter
            public CSystemData_Scanner ScanData = new CSystemData_Scanner();

            // Scan Process 
            public CProcessData ProcData = new CProcessData();


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

            public CModelData()
            {
                for(int i = 0; i < (int)ESpinnerIndex.MAX; i++)
                {
                    SpinnerData[i] = new CCtrlSpinnerData();
                }
            }
        }

        public class CProcessData
        {
            // Wafer Dicing Process
            public float ProcessWaferSize;
            public float WaferDieSizeX;
            public float WaferDieSizeY;
            public float MarginWidth;
            public float MarginHeight;
            public int ProcessLineNum;

            // Stemco Process
            public float ProcessOffsetX1;
            public float ProcessOffsetY1;
            public int   ProcessCount1;
            public float ProcessOffsetX2;
            public float ProcessOffsetY2;
            public int   ProcessCount2;

            public float PatternPitch1;
            public int   PatternCount1;
            public float PatternPitch2;
            public int   PatternCount2;
            public float PatternOffset1;
            public float PatternOffset2;

            public int ProcessInterval;


            public bool ProcessStop=false;

        }

        public class CSystemData_Scanner
        {

            // Comm Setting
            public string ControlHostAddress;
            public int ControlHostPort;
            public string ScanHeadHostAddress;
            public int ScanHeadHostPort;

            #region Polygon Config INI Parameter
            /* [Job Settings] */
            public float InScanResolution;     // USER ENABLE
            public float CrossScanResolution;  // USER ENABLE
            public float InScanOffset;         // USER ENABLE
            public int StopMotorBetweenJobs;    // USER ENABLE
            public int PixInvert;               // USER ENABLE
            public int JobStartBufferTime;      // USER ENABLE
            public int PrecedingBlankLines;     // USER ENABLE

            /* [Laser Configuration] */
            public int LaserOperationMode;         // USER ENABLE 
            public float SeedClockFrequency;      // USER ENABLE
            public float RepetitionRate;          // USER ENABLE
            public int PulsePickWidth;             // Seed Clock과 Rep Rate값이 변하면 Width 변경해야함
            public int PixelWidth;                 // Seed Clock과 Rep Rate값이 변하면 Width 변경해야함
            public int PulsePickAlgor;

            /* [CrossScan Configuration] */
            public float CrossScanEncoderResol;    // USER ENABLE
            public float CrossScanMaxAccel;        // USER ENABLE  
            public int EnCarSig;                    // USER ENABLE
            public int SwapCarSig;                  // USER ENABLE

            /* [Head Configuration] */
            public string SerialNumber;
            public string FThetaConstant;
            public float ExposeLineLength;
            public int EncoderIndexDelay;
            public float FacetFineDelay0;
            public float FacetFineDelay1;
            public float FacetFineDelay2;
            public float FacetFineDelay3;
            public float FacetFineDelay4;
            public float FacetFineDelay5;
            public float FacetFineDelay6;
            public float FacetFineDelay7;
            public int InterleaveRatio;         // USER ENABLE
            public float FacetFineDelayOffset0;// USER ENABLE
            public float FacetFineDelayOffset1;// USER ENABLE
            public float FacetFineDelayOffset2;// USER ENABLE
            public float FacetFineDelayOffset3;// USER ENABLE
            public float FacetFineDelayOffset4;// USER ENABLE
            public float FacetFineDelayOffset5;// USER ENABLE
            public float FacetFineDelayOffset6;// USER ENABLE
            public float FacetFineDelayOffset7;// USER ENABLE
            public int StartFacet;              // USER ENABLE
            public int AutoIncrementStartFacet; // USER ENABLE

            /* [Polygon motor Configuration] */
            //public int InternalMotorDriverClk;
            public int MotorDriverType;
            //public int MotorSpeed;
            //public int SimEncSel;
            public float MinMotorSpeed;
            public float MaxMotorSpeed;
            public int MotorEffectivePoles;
            public int SyncWaitTime;
            public int MotorStableTime;         // USER ENABLE
            public int ShaftEncoderPulseCount;

            /* [Other Settings] */
            public int InterruptFreq;
            public int HWDebugSelection;
            public int AutoRepeat;
            public int ExpoDebugSelection;
            public int PixAlwaysOn;
            public int ExtCamTrig;
            public int EncoderExpo;
            public int FacetTest;
            public int SWTest;
            public int JobstartAutorepeat;

            #endregion

            #region TrueRaster INI Parameter

            // ISN.ini
            public int IsnEnabled;
            public int IsnHome;
            public int IsnProfileCtrl;

            public int IsnPF0S;
            public int IsnPF0E;
            public int IsnPF1S;
            public int IsnPF1E;
            public int IsnPF2S;
            public int IsnPF2E;
            public int IsnPF3S;
            public int IsnPF3E;
            public int IsnPF4S;
            public int IsnPF4E;
            public int IsnPF5S;
            public int IsnPF5E;
            public int IsnPF6S;
            public int IsnPF6E;
            public int IsnPF7S;
            public int IsnPF7E;


            public int FacetCorrectFirstXpos1;
            public int FacetCorrectLast_Xpos1;
            public int FacetCorrectFirstXpos2;
            public int FacetCorrectLast_Xpos2;
            public int FacetCorrectFirstXpos3;
            public int FacetCorrectLast_Xpos3;
            public int FacetCorrectFirstXpos4;
            public int FacetCorrectLast_Xpos4;
            public int FacetCorrectFirstXpos5;
            public int FacetCorrectLast_Xpos5;
            public int FacetCorrectFirstXpos6;
            public int FacetCorrectLast_Xpos6;
            public int FacetCorrectFirstXpos7;
            public int FacetCorrectLast_Xpos7;
            public int FacetCorrectFirstXpos8;
            public int FacetCorrectLast_Xpos8;

            // CSN.ini
            public int CsnEnabled;
            public int CsnHome;
            public int CsnProfileCtrl;

            public int CsnPF0S;
            public int CsnPF0E;
            public int CsnPF1S;
            public int CsnPF1E;
            public int CsnPF2S;
            public int CsnPF2E;
            public int CsnPF3S;
            public int CsnPF3E;
            public int CsnPF4S;
            public int CsnPF4E;
            public int CsnPF5S;
            public int CsnPF5E;
            public int CsnPF6S;
            public int CsnPF6E;
            public int CsnPF7S;
            public int CsnPF7E;

            public int FacetCorrectFirstYpos1;
            public int FacetCorrectLast_Ypos1;
            public int FacetCorrectFirstYpos2;
            public int FacetCorrectLast_Ypos2;
            public int FacetCorrectFirstYpos3;
            public int FacetCorrectLast_Ypos3;
            public int FacetCorrectFirstYpos4;
            public int FacetCorrectLast_Ypos4;
            public int FacetCorrectFirstYpos5;
            public int FacetCorrectLast_Ypos5;
            public int FacetCorrectFirstYpos6;
            public int FacetCorrectLast_Ypos6;
            public int FacetCorrectFirstYpos7;
            public int FacetCorrectLast_Ypos7;
            public int FacetCorrectFirstYpos8;
            public int FacetCorrectLast_Ypos8;

            #endregion

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

        public CSystemData_Scanner SystemData_Scan { get; private set; } = new CSystemData_Scanner();
        public CSystemData_Align SystemData_Align { get; private set; } = new CSystemData_Align();
        public CSystemData_Light SystemData_Light { get; private set; } = new CSystemData_Light();

        /////////////////////////////////////////////////////////////////////////////////
        // Position Data
        public CPositionGroup Pos_Fixed { get; private set; } = new CPositionGroup();
        public CPositionGroup Pos_Model { get; private set; } = new CPositionGroup();
        public CPositionGroup Pos_Offset { get; private set; } = new CPositionGroup();

        /////////////////////////////////////////////////////////////////////////////////
        // User Info
        public CLoginInfo LoginInfo { get; private set; }
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
        public List<CWorkPiece> WorkPiece_InputBuffer { get; private set; } = new List<CWorkPiece>(); // buffer for input
        public List<CWorkPiece> WorkPiece_OutputBuffer { get; private set; } = new List<CWorkPiece>(); // buffer for output

        public MDataManager(CObjectInfo objInfo, CDBInfo dbInfo)
            : base(objInfo)
        {
            DBInfo = dbInfo;
            Login("", true);

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
        }

        public int Initialize()
        {

            int iResult;
            iResult = LoadGeneralData();
            if (iResult != SUCCESS) return iResult;

            // 아래의 네가지 함수 콜은 LWDicer의 Initialize에서 읽어들이는게 맞지만, 생성자에서 한번 더 읽어도 되기에.. 주석처리해도 상관없음
            iResult = LoadSystemData();
            if (iResult != SUCCESS) return iResult;
            iResult = LoadPositionData(true, EPositionObject.ALL);
            if (iResult != SUCCESS) return iResult;
            iResult = LoadPositionData(false, EPositionObject.ALL);
            if (iResult != SUCCESS) return iResult;
            iResult = LoadModelList();
            if (iResult != SUCCESS) return iResult;

            // MakeDefaultModel();
            iResult = ChangeModel(SystemData.ModelName);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
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
                    info.Description[(int)ELanguage.KOREAN] = $"{index}번 에러";
                    info.Solution[(int)ELanguage.KOREAN] = $"{index}번 해결책";
                    AlarmInfoList.Add(info);
                }

                for (int i = 0; i < 10; i++)
                {
                    CParaInfo info = new CParaInfo("Test", "Name" + i.ToString());
                    info.Description[(int)ELanguage.KOREAN] = $"Name{i} 변수";
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
                SavePositionData(Pos_Fixed, true);
                SavePositionData(Pos_Offset, false);
            }

            // WorkPiece and Process Phase를 한번 쭉~ test routine
            if (false)
            {
                CWorkPiece pointer = WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL];
                ELCNetUnitPos pos = ELCNetUnitPos.PUSHPULL;
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_LOAD_FROM_LOADER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_LOAD_FROM_LOADER);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_COATER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_COATER);

                pos = ELCNetUnitPos.SPINNER1;
                ChangeWorkPieceUnit(ELCNetUnitPos.PUSHPULL, pos);
                StartWorkPiecePhase(pos, EProcessPhase.COATER_LOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.COATER_LOAD);
                StartWorkPiecePhase(pos, EProcessPhase.PRE_COATING);
                FinishWorkPiecePhas(pos, EProcessPhase.PRE_COATING);
                StartWorkPiecePhase(pos, EProcessPhase.COATING);
                FinishWorkPiecePhas(pos, EProcessPhase.COATING);
                StartWorkPiecePhase(pos, EProcessPhase.POST_COATING);
                FinishWorkPiecePhas(pos, EProcessPhase.POST_COATING);
                StartWorkPiecePhase(pos, EProcessPhase.COATER_WAIT_UNLOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.COATER_WAIT_UNLOAD);
                StartWorkPiecePhase(pos, EProcessPhase.COATER_UNLOAD_TO_PUSHPULL);
                FinishWorkPiecePhas(pos, EProcessPhase.COATER_UNLOAD_TO_PUSHPULL);

                pos = ELCNetUnitPos.PUSHPULL;
                ChangeWorkPieceUnit(ELCNetUnitPos.SPINNER1, pos);
                StartWorkPiecePhase(pos, EProcessPhase.COATER_UNLOAD_TO_PUSHPULL);
                FinishWorkPiecePhas(pos, EProcessPhase.COATER_UNLOAD_TO_PUSHPULL);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_HANDLER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_HANDLER);

                pos = ELCNetUnitPos.UPPER_HANDLER;
                ChangeWorkPieceUnit(ELCNetUnitPos.PUSHPULL, pos);
                //StartWorkPiecePhase(pos, EProcessPhase.UPPER_HANDLER_LOAD);
                //FinishWorkPiecePhas(pos, EProcessPhase.UPPER_HANDLER_LOAD);
                StartWorkPiecePhase(pos, EProcessPhase.UPPER_HANDLER_UNLOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.UPPER_HANDLER_UNLOAD);

                pos = ELCNetUnitPos.STAGE1;
                ChangeWorkPieceUnit(ELCNetUnitPos.UPPER_HANDLER, pos);
                //StartWorkPiecePhase(pos, EProcessPhase.STAGE_LOAD);
                //FinishWorkPiecePhas(pos, EProcessPhase.STAGE_LOAD);
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
                //StartWorkPiecePhase(pos, EProcessPhase.LOWER_HANDLER_LOAD);
                //FinishWorkPiecePhas(pos, EProcessPhase.LOWER_HANDLER_LOAD);
                StartWorkPiecePhase(pos, EProcessPhase.LOWER_HANDLER_UNLOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.LOWER_HANDLER_UNLOAD);

                pos = ELCNetUnitPos.PUSHPULL;
                ChangeWorkPieceUnit(ELCNetUnitPos.LOWER_HANDLER, pos);
                //StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_LOAD_FROM_HANDLER);
                //FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_LOAD_FROM_HANDLER);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_CLEANER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_CLEANER);

                pos = ELCNetUnitPos.SPINNER1;
                ChangeWorkPieceUnit(ELCNetUnitPos.PUSHPULL, pos);
                StartWorkPiecePhase(pos, EProcessPhase.CLEANER_LOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.CLEANER_LOAD);
                StartWorkPiecePhase(pos, EProcessPhase.CLEANING);
                FinishWorkPiecePhas(pos, EProcessPhase.CLEANING);
                StartWorkPiecePhase(pos, EProcessPhase.CLEANER_WAIT_UNLOAD);
                FinishWorkPiecePhas(pos, EProcessPhase.CLEANER_WAIT_UNLOAD);

                pos = ELCNetUnitPos.PUSHPULL;
                ChangeWorkPieceUnit(ELCNetUnitPos.SPINNER1, pos);
                StartWorkPiecePhase(pos, EProcessPhase.CLEANER_UNLOAD_TO_PUSHPULL);
                FinishWorkPiecePhas(pos, EProcessPhase.CLEANER_UNLOAD_TO_PUSHPULL);
                StartWorkPiecePhase(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_LOADER);
                FinishWorkPiecePhas(pos, EProcessPhase.PUSHPULL_UNLOAD_TO_LOADER);
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
            CSystemData_Align systemAlign =null, CSystemData_Scanner systemScanner = null,
            CSystemData_Light systemLight = null)
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
            

            // CSystemData_Align
            if (systemAlign != null)
            {
                try
                {
                    SystemData_Align = ObjectExtensions.Copy(systemAlign);
                    string output = JsonConvert.SerializeObject(SystemData_Align);

                    if (DBManager.InsertRow(DBInfo.DBConn, DBInfo.TableSystem, "name", nameof(CSystemData_Align), output,
                        true, DBInfo.DBConn_Backup) != true)
                    {
                        return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                    }
                    WriteLog("success : save CSystemData_Align.", ELogType.SYSTEM, ELogWType.SAVE);
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
                    SystemData_Scan = ObjectExtensions.Copy(systemScanner);
                    string output = JsonConvert.SerializeObject(SystemData_Scan);

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
                    WriteLog("success : save CSystemData_Align.", ELogType.SYSTEM, ELogWType.SAVE);
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
                        if (SystemData_Axis.MPMotionData.Length == data.MPMotionData.Length
                            && SystemData_Axis.ACSMotionData.Length == data.ACSMotionData.Length)
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

            

            // CSystemData_Align
            if (loadVision == true)
            {
                try
                {
                    if (DBManager.SelectRow(DBInfo.DBConn, DBInfo.TableSystem, out output, new CDBColumn("name", nameof(CSystemData_Align))) == true)
                    {
                        CSystemData_Align data = JsonConvert.DeserializeObject<CSystemData_Align>(output);
                        if (SystemData_Align.Camera.Length == data.Camera.Length)
                        {
                            SystemData_Align = ObjectExtensions.Copy(data);
                        }
                        else
                        {
                            for (int i = 0; i < SystemData_Align.Camera.Length; i++)
                            {
                                if (i >= data.Camera.Length) break;
                                SystemData_Align.Camera[i] = ObjectExtensions.Copy(data.Camera[i]);
                            }
                        }
                        WriteLog("success : load CSystemData_Align.", ELogType.SYSTEM, ELogWType.LOAD);
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

        public int SavePositionData(CPositionGroup tGroup, bool bType_Fixed, EPositionObject unit = EPositionObject.ALL)
        {
            int iResult;
            string suffix;
            CPositionGroup source;
            if (bType_Fixed)
            {
                source = Pos_Fixed;
                suffix = "_Fixed";
            } else
            {
                source = Pos_Offset;
                suffix = "_Offset_" + SystemData.ModelName;
            }

            string key_value, output;
            for (int i = 0; i < (int)EPositionObject.MAX; i++)
            {
                EPositionObject tUnit = EPositionObject.LOADER + i;
                if (unit != EPositionObject.ALL && tUnit != unit) continue;

                key_value = $"{tUnit}{suffix}";
                output = JsonConvert.SerializeObject(tGroup.Pos_Array[i]);

                iResult = SaveUnitPositionData(key_value, output);
                if (iResult != SUCCESS) return iResult;

                source.UpdatePositionSet(tGroup, tUnit);
            }

            return SUCCESS;
        }
 
        /*       
        public int SavePositionData(bool bType_Fixed, EPositionGroup unit)
        {
            int iResult;

            // Loader
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.LOADER)
            {
                iResult = SavePositionData(bType_Fixed, EPositionObject.LOADER);
                if (iResult != SUCCESS) return iResult;
            }

            // PushPull
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.PUSHPULL)
            {
                iResult = SavePositionData(bType_Fixed, EPositionObject.PUSHPULL);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bType_Fixed, EPositionObject.PUSHPULL_CENTER1);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bType_Fixed, EPositionObject.PUSHPULL_CENTER2);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner1
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.SPINNER1)
            {
                iResult = SavePositionData(bType_Fixed, EPositionObject.S1_ROTATE);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bType_Fixed, EPositionObject.S1_CLEAN_NOZZLE);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bType_Fixed, EPositionObject.S1_COAT_NOZZLE);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner2
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.SPINNER2)
            {
                iResult = SavePositionData(bType_Fixed, EPositionObject.S2_ROTATE);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bType_Fixed, EPositionObject.S2_CLEAN_NOZZLE);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bType_Fixed, EPositionObject.S2_COAT_NOZZLE);
                if (iResult != SUCCESS) return iResult;
            }

            // Handler
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.HANDLER)
            {
                iResult = SavePositionData(bType_Fixed, EPositionObject.LOWER_HANDLER);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bType_Fixed, EPositionObject.UPPER_HANDLER);
                if (iResult != SUCCESS) return iResult;
            }

            // Stage
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.STAGE1)
            {
                iResult = SavePositionData(bType_Fixed, EPositionObject.STAGE1);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bType_Fixed, EPositionObject.CAMERA1);
                if (iResult != SUCCESS) return iResult;
                iResult = SavePositionData(bType_Fixed, EPositionObject.SCANNER1);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }
        */

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

        public int LoadPositionData(bool bType_Fixed, EPositionObject unit = EPositionObject.ALL)
        {
            int iResult;
            CPositionGroup source;
            string suffix;
            if (bType_Fixed)
            {
                source = Pos_Fixed;
                suffix = "_Fixed";
            } else
            {
                source = Pos_Offset;
                suffix = "_Offset_" + SystemData.ModelName;
            }

            string key_value, output;
            for (int i = 0; i < (int)EPositionObject.MAX; i++)
            {
                EPositionObject tUnit = EPositionObject.LOADER + i;
                if (unit != EPositionObject.ALL && tUnit != unit) continue;

                key_value = $"{tUnit}{suffix}";
                iResult = LoadUnitPositionData(key_value, out output);
                if (iResult != SUCCESS) return iResult;

                CPositionSet data = JsonConvert.DeserializeObject<CPositionSet>(output);
                if (data != null && data.Length > 0)
                {
                    source.UpdatePositionSet(data, tUnit);
                }
            }

            /*
                        // Stage1
                        if (unit == EPositionObject.ALL || unit == EPositionObject.STAGE1)
                        {
                            key_value = EPositionObject.STAGE1.ToString() + suffix;
                            iResult = LoadUnitPositionData(key_value, out output);
                            if (iResult != SUCCESS) return iResult;

                            CPositionSet data = JsonConvert.DeserializeObject<CPositionSet>(output);
                            if(data != null && data.Length > 0)
                                tData.Pos_Stage1 = ObjectExtensions.Copy(data);


                            /////////////////////////////////////////////////////////////////////
                            // Copy될때 Array의 크기가 변함.
                            if(tData.Pos_Stage1.Pos.Length < (int)EStagePos.MAX)
                            {
                                Array.Resize(ref tData.Pos_Stage1.Pos, (int)EStagePos.MAX);

                                for(int i= tData.Pos_Stage1.Length; i < (int)EStagePos.MAX;i++)
                                {
                                        tData.Pos_Stage1.Pos[i] = new CPos_XYTZ();
                                }
                            }
                            /////////////////////////////////////////////////////////////////////

                        }

                        if (unit == EPositionObject.ALL || unit == EPositionObject.CAMERA1)
                        {
                            key_value = EPositionObject.CAMERA1.ToString() + suffix;
                            iResult = LoadUnitPositionData(key_value, out output);
                            if (iResult != SUCCESS) return iResult;

                            CPositionSet data = JsonConvert.DeserializeObject<CPositionSet>(output);
                            if(data != null && data.Length > 0)
                                tData.Pos_Camera1 = ObjectExtensions.Copy(data);

                            /////////////////////////////////////////////////////////////////////
                            // Copy될때 Array의 크기가 변함.
                            if (tData.Pos_Camera1.Pos.Length < (int)ECameraPos.MAX)
                            {
                                Array.Resize(ref tData.Pos_Camera1.Pos, (int)ECameraPos.MAX);

                                for (int i = tData.Pos_Camera1.Length; i < (int)ECameraPos.MAX; i++)
                                {
                                    tData.Pos_Camera1.Pos[i] = new CPos_XYTZ();
                                }
                            }
                            /////////////////////////////////////////////////////////////////////
                        }

                        if (unit == EPositionObject.ALL || unit == EPositionObject.SCANNER1)
                        {
                            key_value = EPositionObject.SCANNER1.ToString() + suffix;
                            iResult = LoadUnitPositionData(key_value, out output);
                            if (iResult != SUCCESS) return iResult;

                            CPositionSet data = JsonConvert.DeserializeObject<CPositionSet>(output);
                            if(data != null && data.Length > 0)
                                tData.Pos_Scanner1 = ObjectExtensions.Copy(data);

                            /////////////////////////////////////////////////////////////////////
                            // Copy될때 Array의 크기가 변함.
                            if (tData.Pos_Scanner1.Pos.Length < (int)EScannerPos.MAX)
                            {
                                Array.Resize(ref tData.Pos_Scanner1.Pos, (int)EScannerPos.MAX);

                                for (int i = tData.Pos_Scanner1.Length; i < (int)EScannerPos.MAX; i++)
                                {
                                    tData.Pos_Scanner1.Pos[i] = new CPos_XYTZ();
                                }
                            }
                            /////////////////////////////////////////////////////////////////////
                        }
            */

            return SUCCESS;
        }

/*
        public int LoadPositionData(bool bType_Fixed, EPositionGroup unit)
        {
            int iResult;

            // Loader
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.LOADER)
            {
                iResult = LoadPositionData(bType_Fixed, EPositionObject.LOADER);
                if (iResult != SUCCESS) return iResult;
            }

            // PushPull
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.PUSHPULL)
            {
                iResult = LoadPositionData(bType_Fixed, EPositionObject.PUSHPULL);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bType_Fixed, EPositionObject.PUSHPULL_CENTER1);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bType_Fixed, EPositionObject.PUSHPULL_CENTER2);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner1
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.SPINNER1)
            {
                iResult = LoadPositionData(bType_Fixed, EPositionObject.S1_ROTATE);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bType_Fixed, EPositionObject.S1_CLEAN_NOZZLE);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bType_Fixed, EPositionObject.S1_COAT_NOZZLE);
                if (iResult != SUCCESS) return iResult;
            }

            // Spinner2
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.SPINNER2)
            {
                iResult = LoadPositionData(bType_Fixed, EPositionObject.S2_ROTATE);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bType_Fixed, EPositionObject.S2_CLEAN_NOZZLE);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bType_Fixed, EPositionObject.S2_COAT_NOZZLE);
                if (iResult != SUCCESS) return iResult;
            }

            // Handler
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.HANDLER)
            {
                iResult = LoadPositionData(bType_Fixed, EPositionObject.LOWER_HANDLER);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bType_Fixed, EPositionObject.UPPER_HANDLER);
                if (iResult != SUCCESS) return iResult;
            }

            // Stage
            if (unit == EPositionGroup.ALL || unit == EPositionGroup.STAGE1)
            {
                iResult = LoadPositionData(bType_Fixed, EPositionObject.STAGE1);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bType_Fixed, EPositionObject.CAMERA1);
                if (iResult != SUCCESS) return iResult;
                iResult = LoadPositionData(bType_Fixed, EPositionObject.SCANNER1);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }
*/

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
            if (IsModelHeaderExist(ELoginType.ENGINEER.ToString(), type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetFolder(ELoginType.ENGINEER.ToString(), "", NAME_ROOT_FOLDER);
                UserInfoHeaderList.Add(header);
                //iResult = SaveModelHeaderList(type);
                //if (iResult != SUCCESS) return iResult;
            }

            // make operator folder
            if (IsModelHeaderExist(ELoginType.OPERATOR.ToString(), type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetFolder(ELoginType.OPERATOR.ToString(), "", NAME_ROOT_FOLDER);
                UserInfoHeaderList.Add(header);
                //iResult = SaveModelHeaderList(type);
                //if (iResult != SUCCESS) return iResult;
            }

            // make maker folder
            if (IsModelHeaderExist(ELoginType.MAKER.ToString(), type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetFolder(ELoginType.MAKER.ToString(), "", NAME_ROOT_FOLDER);
                UserInfoHeaderList.Add(header);
                //iResult = SaveModelHeaderList(type);
                //if (iResult != SUCCESS) return iResult;
            }

            // save header list
            iResult = SaveModelHeaderList(type);
            if (iResult != SUCCESS) return iResult;

            // make default operator
            if (IsModelHeaderExist(NAME_DEFAULT_OPERATOR, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetModel(NAME_DEFAULT_OPERATOR, NAME_DEFAULT_OPERATOR, ELoginType.OPERATOR.ToString());
                UserInfoHeaderList.Add(header);
                iResult = SaveModelHeaderList(type);
                if (iResult != SUCCESS) return iResult;
            }

            if (IsModelExist(NAME_DEFAULT_OPERATOR, type) == false)
            {
                CUserInfo data = new CUserInfo(NAME_DEFAULT_OPERATOR, NAME_DEFAULT_OPERATOR, "", ELoginType.OPERATOR);
                iResult = SaveUserData(data);
                if (iResult != SUCCESS) return iResult;
            }

            // make maker
            if (IsModelHeaderExist(NAME_MAKER, type) == false)
            {
                CListHeader header = new CListHeader();
                header.SetModel(NAME_MAKER, NAME_MAKER, ELoginType.MAKER.ToString());
                UserInfoHeaderList.Add(header);
                iResult = SaveModelHeaderList(type);
                if (iResult != SUCCESS) return iResult;
            }
            if (IsModelExist(NAME_MAKER, type) == false)
            {
                CUserInfo data = new CUserInfo(NAME_MAKER, NAME_MAKER, "", ELoginType.MAKER);
                iResult = SaveUserData(data);
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

        /// <summary>
        /// CUserInfo List는 ModelData는 아니지만, 함수를 같이 사용하기 위해서 SaveModelData를 이용해서 호출할수 있도록
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int SaveUserData(CUserInfo data)
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

            WriteLog($"success : save {type} user [{data.Name}].", ELogType.SYSTEM, ELogWType.SAVE);
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
            iResult = LoadPositionData(false);
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

        public int Logout()
        {
            int iResult = Login(NAME_DEFAULT_OPERATOR);
            return iResult;
        }

        public int Login(string name, bool IsSystemStart = false)
        {
            CUserInfo user = new CUserInfo();
            if(IsSystemStart == false)
            {
                if (LoginInfo.User.Name == name)
                    return SUCCESS;

                // 1. logout and save
                LoginInfo.AccessTime = DateTime.Now;
                LoginInfo.AccessType = false;
                SaveLoginHistory(LoginInfo);
                DBManager.SetOperator(LoginInfo.User.Name, LoginInfo.User.Type.ToString());
                WriteLog($"{LoginInfo}", ELogType.LOGINOUT, ELogWType.LOGOUT);

                // 2. login and save
                LoadUserInfo(name, out user);
                LoginInfo = new CLoginInfo(user);
                LoginInfo.AccessTime = DateTime.Now;
                LoginInfo.AccessType = true;
                SaveLoginHistory(LoginInfo);
                DBManager.SetOperator(user.Name, user.Type.ToString());
                WriteLog($"{LoginInfo}", ELogType.LOGINOUT, ELogWType.LOGIN);

            }
            else
            {
                user.SetMaker();

                // 2. login and save
                LoginInfo = new CLoginInfo(user);
                LoginInfo.AccessTime = DateTime.Now;
                LoginInfo.AccessType = true;
                SaveLoginHistory(LoginInfo);
                DBManager.SetOperator(user.Name, user.Type.ToString());
                WriteLog($"{LoginInfo}", ELogType.LOGINOUT, ELogWType.LOGIN);
            }

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

            //ImportDataFromExcel(EInfoExcel_Sheet.Skip);

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
                        
                        if(index >= DEF_IO.INPUT_ORIGIN && index < DEF_IO.OUTPUT_ORIGIN)
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
            CAlarmInfo prevInfo = ObjectExtensions.Copy(info);
            for(int i = 0; i < AlarmInfoList.Count; i++)
            {
                if(AlarmInfoList[i].Index == info.Index)
                {
                    if (AlarmInfoList[i].IsEqual(info)) return SUCCESS;
                    prevInfo = ObjectExtensions.Copy(AlarmInfoList[i]);
                    AlarmInfoList.RemoveAt(i);
                    break;
                }
            }

            if(bMerge && prevInfo.Index != 0)
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

        public int UpdateMessageInfo(CMessageInfo info, bool bSaveToDB = true)
        {
            if(info.Index != -1) // update
            {
                CMessageInfo prevInfo = ObjectExtensions.Copy(info);
                for (int i = 0; i < MessageInfoList.Count; i++)
                {
                    if (MessageInfoList[i].Index == info.Index)
                    {
                        if (MessageInfoList[i].IsEqual(info)) return SUCCESS;
                        prevInfo = ObjectExtensions.Copy(MessageInfoList[i]);
                        MessageInfoList.RemoveAt(i);
                        break;
                    }
                }
                prevInfo.Update(info);
                MessageInfoList.Add(prevInfo);
            } else // add new
            {
                info.Index = GetNextMessageIndex();
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
                if (item.Index >= index)
                {
                    index = item.Index + 1;
                }

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
                    CParaInfo info = JsonConvert.DeserializeObject<CParaInfo>(output);

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
            CParaInfo prevInfo = ObjectExtensions.Copy(info);
            for (int i = 0; i < ParaInfoList.Count; i++)
            {
                if (ParaInfoList[i].Name == info.Name && ParaInfoList[i].Group == info.Group)
                {
                    if (ParaInfoList[i].IsEqual(info)) return SUCCESS;
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

            AlarmInfoList.Sort(delegate (CAlarmInfo a, CAlarmInfo b) { return a.Index.CompareTo(b.Index); });
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
                    // index = 0 은 저장할 필요 없음
                    if (info.Index == 0) continue;
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
            if (string.IsNullOrEmpty(table)) return SUCCESS;
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
               

        public int ImportDataFromExcel(string strPath, EInfoExcel_Sheet nSheet)
        {
            if(nSheet == EInfoExcel_Sheet.NONE)
            {
                WriteLog($"success : System Parameter Read Skip", ELogType.Debug);
                return SUCCESS;
            }

            int nSheetCount = 0;
            int iResult;
            try
            {
                Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbook WorkBook = ExcelApp.Workbooks.Open(strPath);

                // 1. Open 한 Excel File에 Sheet Count
                nSheetCount = WorkBook.Worksheets.Count;

                // 2. Excel Sheet Row, Column 접근을 위한 Range 생성
                Excel.Range[] array_SheetRange = new Excel.Range[nSheetCount];

                // 3. Excel Sheet Item Data 획득을 위한 Worksheet 생성
                Excel.Worksheet[] array_Sheet = new Excel.Worksheet[nSheetCount];

                // 4. Excel Sheet 정보를 불러 온다.
                for (int i = 0; i < nSheetCount; i++)
                {
                    array_Sheet[i] = (Excel.Worksheet)WorkBook.Worksheets.get_Item(i + 1);
                    array_SheetRange[i] = array_Sheet[i].UsedRange;
                }

                // Message Info
                if (nSheet == EInfoExcel_Sheet.MAX || nSheet == EInfoExcel_Sheet.MESSAGE)
                {
                    Excel.Range sheetRange = array_SheetRange[(int)EInfoExcel_Sheet.MESSAGE];
                    //MessageInfoList.Clear();
                    int startRow = 2;
                    int nCount = sheetRange.EntireRow.Count;
                    for (int i = 0; i < nCount - 1; i++)
                    {
                        CMessageInfo MessageInfo = new CMessageInfo();

                        // Index
                        MessageInfo.Index = (int)(sheetRange.Cells[i + startRow, 1] as Excel.Range).Value2;

                        // Type
                        if ((string)(sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2 == Convert.ToString(EMessageType.OK)) MessageInfo.Type = EMessageType.OK;
                        if ((string)(sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2 == Convert.ToString(EMessageType.OK_CANCEL)) MessageInfo.Type = EMessageType.OK_CANCEL;
                        if ((string)(sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2 == Convert.ToString(EMessageType.CONFIRM_CANCEL)) MessageInfo.Type = EMessageType.CONFIRM_CANCEL;

                        // Message
                        MessageInfo.Message[(int)ELanguage.KOREAN] = (string)(sheetRange.Cells[i + startRow, 3] as Excel.Range).Value2;
                        MessageInfo.Message[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 4] as Excel.Range).Value2;
                        MessageInfo.Message[(int)ELanguage.CHINESE] = (string)(sheetRange.Cells[i + startRow, 5] as Excel.Range).Value2;
                        MessageInfo.Message[(int)ELanguage.JAPANESE] = (string)(sheetRange.Cells[i + startRow, 6] as Excel.Range).Value2;

                        //MessageInfoList.Add(MessageInfo);
                        UpdateMessageInfo(MessageInfo, false);
                    }

                    SaveMessageInfoList();
                }

                // Parameter Info
                if (nSheet == EInfoExcel_Sheet.MAX || nSheet == EInfoExcel_Sheet.PARAMETER)
                {
                    Excel.Range sheetRange = array_SheetRange[(int)EInfoExcel_Sheet.PARAMETER];
                    int nRowCount = sheetRange.EntireRow.Count;
                    int startRow = 2;
                    //ParaInfoList.Clear();
                    for (int i = 0; i < nRowCount - 1; i++)
                    {
                        CParaInfo ParaInfo = new CParaInfo();

                        ParaInfo.Group = (string)(sheetRange.Cells[i + startRow, 1] as Excel.Range).Value2; // Group
                        ParaInfo.Name = (string)(sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2; // Name
                        ParaInfo.Unit = (string)(sheetRange.Cells[i + startRow, 3] as Excel.Range).Value2; // Unit
                        ParaInfo.Type = (EUnitType)(sheetRange.Cells[i + startRow, 4] as Excel.Range).Value2; // Type

                        ParaInfo.DisplayName[(int)ELanguage.KOREAN] = (string)(sheetRange.Cells[i + startRow, 5] as Excel.Range).Value2;
                        ParaInfo.DisplayName[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 6] as Excel.Range).Value2;
                        ParaInfo.DisplayName[(int)ELanguage.CHINESE] = (string)(sheetRange.Cells[i + startRow, 7] as Excel.Range).Value2;
                        ParaInfo.DisplayName[(int)ELanguage.JAPANESE] = (string)(sheetRange.Cells[i + startRow, 8] as Excel.Range).Value2;

                        ParaInfo.Description[(int)ELanguage.KOREAN] = (string)(sheetRange.Cells[i + startRow, 9] as Excel.Range).Value2;
                        ParaInfo.Description[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 10] as Excel.Range).Value2;
                        ParaInfo.Description[(int)ELanguage.CHINESE] = (string)(sheetRange.Cells[i + startRow, 11] as Excel.Range).Value2;
                        ParaInfo.Description[(int)ELanguage.JAPANESE] = (string)(sheetRange.Cells[i + startRow, 12] as Excel.Range).Value2;

                        //ParaInfoList.Add(ParaInfo);
                        UpdateParaInfo(ParaInfo, false, false);
                    }

                    SaveParaInfoList();
                }

                    // Alarm Info
                if (nSheet == EInfoExcel_Sheet.MAX || nSheet == EInfoExcel_Sheet.ALARM)
                {
                    Excel.Range sheetRange = array_SheetRange[(int)EInfoExcel_Sheet.ALARM];
                    int nRowCount = sheetRange.EntireRow.Count;
                    int startRow = 2;
                    //AlarmInfoList.Clear();
                    for (int i = 0; i < nRowCount - 1; i++)
                    {
                        CAlarmInfo AlarmInfo = new CAlarmInfo();
                        try
                        {
                            AlarmInfo.Group = (EAlarmGroup)Enum.Parse(typeof(EAlarmGroup), (sheetRange.Cells[i + startRow, 1] as Excel.Range).Value2); // Group
                            AlarmInfo.Esc = (string)(sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2; // ESC
                            AlarmInfo.Index = (int)(sheetRange.Cells[i + startRow, 3] as Excel.Range).Value2; // Index
                            AlarmInfo.Type = (EErrorType)Enum.Parse(typeof(EErrorType), (sheetRange.Cells[i + startRow, 4] as Excel.Range).Value2); // Type
                        }
                        catch (System.Exception ex)
                        {
                            AlarmInfo.Group = EAlarmGroup.NONE; // Group
                            AlarmInfo.Esc = (string)(sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2; // ESC
                            AlarmInfo.Index = (int)(sheetRange.Cells[i + startRow, 3] as Excel.Range).Value2; // Index
                            AlarmInfo.Type = EErrorType.E1; // Type
                        }

                        AlarmInfo.Description[(int)ELanguage.KOREAN] = (string)(sheetRange.Cells[i + startRow, 5] as Excel.Range).Value2.Trim();
                        AlarmInfo.Description[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 6] as Excel.Range).Value2.Trim();
                        AlarmInfo.Description[(int)ELanguage.CHINESE] = (string)(sheetRange.Cells[i + startRow, 7] as Excel.Range).Value2.Trim();
                        AlarmInfo.Description[(int)ELanguage.JAPANESE] = (string)(sheetRange.Cells[i + startRow, 8] as Excel.Range).Value2.Trim();

                        AlarmInfo.Solution[(int)ELanguage.KOREAN] = (string)(sheetRange.Cells[i + startRow, 9] as Excel.Range).Value2.Trim();
                        AlarmInfo.Solution[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 10] as Excel.Range).Value2.Trim();
                        AlarmInfo.Solution[(int)ELanguage.CHINESE] = (string)(sheetRange.Cells[i + startRow, 11] as Excel.Range).Value2.Trim();
                        AlarmInfo.Solution[(int)ELanguage.JAPANESE] = (string)(sheetRange.Cells[i + startRow, 12] as Excel.Range).Value2.Trim();

                        //AlarmInfoList.Add(AlarmInfo);
                        UpdateAlarmInfo(AlarmInfo, false, false);
                    }

                    SaveAlarmInfoList();
                }

                   // IO 
                if (nSheet == EInfoExcel_Sheet.MAX || nSheet == EInfoExcel_Sheet.IO)
                {
                    Excel.Range sheetRange = array_SheetRange[(int)EInfoExcel_Sheet.IO];
                    int nCount = 0;
                    int startRow = 2;
                    for (int i = 0; i < 16; i++)
                    {
                        InputArray[nCount].Name[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2;
                        OutputArray[nCount].Name[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 4] as Excel.Range).Value2;

                        InputArray[nCount + 16].Name[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 7] as Excel.Range).Value2;
                        OutputArray[nCount + 16].Name[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 9] as Excel.Range).Value2;

                        InputArray[nCount + 32].Name[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 12] as Excel.Range).Value2;
                        OutputArray[nCount + 32].Name[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 14] as Excel.Range).Value2;

                        InputArray[nCount + 48].Name[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 17] as Excel.Range).Value2;
                        OutputArray[nCount + 48].Name[(int)ELanguage.ENGLISH] = (string)(sheetRange.Cells[i + startRow, 19] as Excel.Range).Value2;

                        nCount++;
                    }

                    for (int i = 0; i < InputArray.Length; i++)
                    {
                        InputArray[i].Name[(int)ELanguage.ENGLISH] = InputArray[i].Name[(int)ELanguage.ENGLISH].Trim();
                    }

                    for (int i = 0; i < InputArray.Length; i++)
                    {
                        OutputArray[i].Name[(int)ELanguage.ENGLISH] = OutputArray[i].Name[(int)ELanguage.ENGLISH].Trim();
                    }

                    SaveIOList();
                }

                    // Motor Data
                if (nSheet == EInfoExcel_Sheet.MAX || nSheet == EInfoExcel_Sheet.MOTOR)
                {
                    Excel.Range sheetRange = array_SheetRange[(int)EInfoExcel_Sheet.MOTOR];
                    int startRow = 2;
                    for (int i = 0; i < (int)EYMC_Axis.MAX; i++)
                    {
                        // Speed
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 3] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 4] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 5] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 6] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 7] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 8] as Excel.Range).Text);

                        // Acc
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 9] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 10] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 11] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 12] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 13] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 14] as Excel.Range).Text);

                        // Dec
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 15] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 16] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 17] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 18] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 19] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 20] as Excel.Range).Text);

                        // S/W Limit
                        SystemData_Axis.MPMotionData[i].PosLimit.Plus = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 21] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].PosLimit.Minus = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 22] as Excel.Range).Text);

                        // Limit Time
                        SystemData_Axis.MPMotionData[i].TimeLimit.tMoveLimit = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 23] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].TimeLimit.tSleepAfterMove = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 24] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].TimeLimit.tOriginLimit = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 25] as Excel.Range).Text);

                        // Home Option
                        SystemData_Axis.MPMotionData[i].OriginData.Method = Convert.ToInt16((string)(sheetRange.Cells[i + startRow, 26] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].OriginData.Dir = Convert.ToInt16((string)(sheetRange.Cells[i + startRow, 27] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].OriginData.FastSpeed = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 28] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].OriginData.SlowSpeed = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 29] as Excel.Range).Text);
                        SystemData_Axis.MPMotionData[i].OriginData.HomeOffset = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 30] as Excel.Range).Text);
                    }

                    startRow += (int)EYMC_Axis.MAX;
                    for (int i = 0; i < (int)EACS_Axis.MAX; i++)
                    {
                        // Speed
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 3] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 4] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 5] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 6] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 7] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 8] as Excel.Range).Text);

                        // Acc
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 9] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 10] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 11] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 12] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 13] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 14] as Excel.Range).Text);

                        // Dec
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 15] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 16] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 17] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 18] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 19] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 20] as Excel.Range).Text);

                        // S/W Limit
                        SystemData_Axis.ACSMotionData[i].PosLimit.Plus = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 21] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].PosLimit.Minus = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 22] as Excel.Range).Text);

                        // Limit Time
                        SystemData_Axis.ACSMotionData[i].TimeLimit.tMoveLimit = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 23] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].TimeLimit.tSleepAfterMove = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 24] as Excel.Range).Text);
                        SystemData_Axis.ACSMotionData[i].TimeLimit.tOriginLimit = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 25] as Excel.Range).Text);

                        // Home Option
                        //SystemData_Axis.ACSMotionData[i].OriginData.Method = Convert.ToInt16((string)(sheetRange.Cells[i + startRow, 26] as Excel.Range).Text);
                        //SystemData_Axis.ACSMotionData[i].OriginData.Dir = Convert.ToInt16((string)(sheetRange.Cells[i + startRow, 27] as Excel.Range).Text);
                        //SystemData_Axis.ACSMotionData[i].OriginData.FastSpeed = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 28] as Excel.Range).Text);
                        //SystemData_Axis.ACSMotionData[i].OriginData.SlowSpeed = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 29] as Excel.Range).Text);
                        //SystemData_Axis.ACSMotionData[i].OriginData.HomeOffset = Convert.ToDouble((string)(sheetRange.Cells[i + startRow, 30] as Excel.Range).Text);
                    }

                    SaveSystemData(systemAxis: SystemData_Axis);
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

        public int ExportDataToExcel(string strPath, EInfoExcel_Sheet nSheet)
        {
            int iResult = SUCCESS;

            int nSheetCount = 0, nCount = 0;
            Excel.Application ExcelApp = null;
            Excel.Workbook WorkBook = null;
            try
            {
                ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                WorkBook = ExcelApp.Workbooks.Open(strPath);

                // 1. Open 한 Excel File에 Sheet Count
                nSheetCount = WorkBook.Worksheets.Count;

                // 2. Excel Sheet Row, Column 접근을 위한 Range 생성
                Excel.Range[] array_SheetRange = new Excel.Range[nSheetCount];

                // 3. Excel Sheet Item Data 획득을 위한 Worksheet 생성
                Excel.Worksheet[] array_Sheet = new Excel.Worksheet[nSheetCount];

                // 4. Excel Sheet 정보를 불러 온다.
                for (int i = 0; i < nSheetCount; i++)
                {
                    array_Sheet[i] = (Excel.Worksheet)WorkBook.Worksheets.get_Item(i + 1);
                    array_SheetRange[i] = array_Sheet[i].UsedRange;
                }

                ////////////////////////////////////////////////////////////////////////////////////
                // Message Info
                if (nSheet == EInfoExcel_Sheet.MAX || nSheet == EInfoExcel_Sheet.MESSAGE)
                {
                    MessageInfoList.Sort(delegate (CMessageInfo a, CMessageInfo b) { return a.Index.CompareTo(b.Index); });
                    nCount = MessageInfoList.Count;
                    int startRow = 2;
                    Excel.Range sheetRange = array_SheetRange[(int)EInfoExcel_Sheet.MESSAGE];
                    for (int i = 0; i < nCount; i++)
                    {
                        Debug.WriteLine($"i = {i}");
                        (sheetRange.Cells[i + startRow, 1] as Excel.Range).Value2 = MessageInfoList[i].Index;

                        if (MessageInfoList[i].Type == EMessageType.OK) (sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2 = Convert.ToString(EMessageType.OK);
                        if (MessageInfoList[i].Type == EMessageType.OK_CANCEL) (sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2 = Convert.ToString(EMessageType.OK_CANCEL);
                        if (MessageInfoList[i].Type == EMessageType.CONFIRM_CANCEL) (sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2 = Convert.ToString(EMessageType.CONFIRM_CANCEL);

                        for (int j = 0; j < (int)ELanguage.MAX; j++)
                        {
                            string msg = (MessageInfoList[i].Message[(int)ELanguage.KOREAN + j] != null) ? MessageInfoList[i].Message[(int)ELanguage.KOREAN + j].Trim() : "";
                            (sheetRange.Cells[i + startRow, 3 + j] as Excel.Range).Value2 = msg;
                        }
                    }
                    WriteLog($"success : export message info to excel", ELogType.Debug);
                    WorkBook.Save();
                }

                ////////////////////////////////////////////////////////////////////////////////////
                // Parameter Info
                if (nSheet == EInfoExcel_Sheet.MAX || nSheet == EInfoExcel_Sheet.PARAMETER)
                {
                    nCount = ParaInfoList.Count;
                    int startRow = 2;
                    Excel.Range sheetRange = array_SheetRange[(int)EInfoExcel_Sheet.PARAMETER];
                    for (int i = 0; i < nCount; i++)
                    {
                        (sheetRange.Cells[i + startRow, 1] as Excel.Range).Value2 = ParaInfoList[i].Group;
                        (sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2 = ParaInfoList[i].Name;
                        (sheetRange.Cells[i + startRow, 3] as Excel.Range).Value2 = ParaInfoList[i].Unit;
                        (sheetRange.Cells[i + startRow, 4] as Excel.Range).Value2 = ParaInfoList[i].Type;

                        for (int j = 0; j < (int)ELanguage.MAX; j++)
                        {
                            string msg = (ParaInfoList[i].DisplayName[(int)ELanguage.KOREAN + j] != null) ? ParaInfoList[i].DisplayName[(int)ELanguage.KOREAN + j].Trim() : "";
                            (sheetRange.Cells[i + startRow, 5 + j] as Excel.Range).Value2 = msg;
                        }
                        for (int j = 0; j < (int)ELanguage.MAX; j++)
                        {
                            string msg = (ParaInfoList[i].Description[(int)ELanguage.KOREAN + j] != null) ? ParaInfoList[i].Description[(int)ELanguage.KOREAN + j].Trim() : "";
                            (sheetRange.Cells[i + startRow, 9 + j] as Excel.Range).Value2 = msg;
                        }
                    }
                    WriteLog($"success : export parameter info to excel", ELogType.Debug);
                    WorkBook.Save();
                }

                ////////////////////////////////////////////////////////////////////////////////////
                // Alarm Info
                if (nSheet == EInfoExcel_Sheet.MAX || nSheet == EInfoExcel_Sheet.ALARM)
                {
                    AlarmInfoList.Sort(delegate (CAlarmInfo a, CAlarmInfo b) { return a.Index.CompareTo(b.Index); });
                    nCount = AlarmInfoList.Count;
                    int startRow = 2;
                    Excel.Range sheetRange = array_SheetRange[(int)EInfoExcel_Sheet.ALARM];
                    for (int i = 0; i < nCount; i++)
                    {
                        (sheetRange.Cells[i + startRow, 1] as Excel.Range).Value2 = AlarmInfoList[i].Group.ToString();
                        (sheetRange.Cells[i + startRow, 2] as Excel.Range).Value2 = AlarmInfoList[i].Esc;
                        (sheetRange.Cells[i + startRow, 3] as Excel.Range).Value2 = AlarmInfoList[i].Index;
                        (sheetRange.Cells[i + startRow, 4] as Excel.Range).Value2 = AlarmInfoList[i].Type.ToString();

                        for (int j = 0; j < (int)ELanguage.MAX; j++)
                        {
                            string msg = (AlarmInfoList[i].Description[(int)ELanguage.KOREAN + j] != null) ? AlarmInfoList[i].Description[(int)ELanguage.KOREAN + j].Trim() : "";
                            (sheetRange.Cells[i + startRow, 5 + j] as Excel.Range).Value2 = msg;
                        }
                        for (int j = 0; j < (int)ELanguage.MAX; j++)
                        {
                            string msg = (AlarmInfoList[i].Solution[(int)ELanguage.KOREAN + j] != null) ? AlarmInfoList[i].Solution[(int)ELanguage.KOREAN + j].Trim() : "";
                            (sheetRange.Cells[i + startRow, 9 + j] as Excel.Range).Value2 = msg;
                        }
                    }
                    WriteLog($"success : export alarm info to excel", ELogType.Debug);
                    WorkBook.Save();
                }

                ////////////////////////////////////////////////////////////////////////////////////
                // Motor Data
                if (nSheet == EInfoExcel_Sheet.MAX || nSheet == EInfoExcel_Sheet.MOTOR)
                {
                    int startRow = 2;
                    Excel.Range sheetRange = array_SheetRange[(int)EInfoExcel_Sheet.MOTOR];
                    for (int i = 0; i < (int)EYMC_Axis.MAX; i++)
                    {
                        // Speed
                        (sheetRange.Cells[i + startRow, 3] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel;
                        (sheetRange.Cells[i + startRow, 4] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel;
                        (sheetRange.Cells[i + startRow, 5] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel;
                        (sheetRange.Cells[i + startRow, 6] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel;
                        (sheetRange.Cells[i + startRow, 7] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel;
                        (sheetRange.Cells[i + startRow, 8] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel;

                        // Acc
                        (sheetRange.Cells[i + startRow, 9] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc;
                        (sheetRange.Cells[i + startRow, 10] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc;
                        (sheetRange.Cells[i + startRow, 11] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc;
                        (sheetRange.Cells[i + startRow, 12] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc;
                        (sheetRange.Cells[i + startRow, 13] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc;
                        (sheetRange.Cells[i + startRow, 14] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc;

                        // Dec
                        (sheetRange.Cells[i + startRow, 15] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec;
                        (sheetRange.Cells[i + startRow, 16] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec;
                        (sheetRange.Cells[i + startRow, 17] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec;
                        (sheetRange.Cells[i + startRow, 18] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec;
                        (sheetRange.Cells[i + startRow, 19] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec;
                        (sheetRange.Cells[i + startRow, 20] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec;

                        // S/W Limit
                        (sheetRange.Cells[i + startRow, 21] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].PosLimit.Plus;
                        (sheetRange.Cells[i + startRow, 22] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].PosLimit.Minus;

                        // Limit Time
                        (sheetRange.Cells[i + startRow, 23] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].TimeLimit.tMoveLimit;
                        (sheetRange.Cells[i + startRow, 24] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].TimeLimit.tSleepAfterMove;
                        (sheetRange.Cells[i + startRow, 25] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].TimeLimit.tOriginLimit;

                        // Home Option
                        (sheetRange.Cells[i + startRow, 26] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.Method;
                        (sheetRange.Cells[i + startRow, 27] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.Dir;
                        (sheetRange.Cells[i + startRow, 28] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.FastSpeed;
                        (sheetRange.Cells[i + startRow, 29] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.SlowSpeed;
                        (sheetRange.Cells[i + startRow, 30] as Excel.Range).Value2 = SystemData_Axis.MPMotionData[i].OriginData.HomeOffset;
                    }

                    startRow += (int)EYMC_Axis.MAX;
                    for (int i = 0; i < (int)EACS_Axis.MAX; i++)
                    {
                        // Speed
                        (sheetRange.Cells[i + startRow, 3] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel;
                        (sheetRange.Cells[i + startRow, 4] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel;
                        (sheetRange.Cells[i + startRow, 5] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel;
                        (sheetRange.Cells[i + startRow, 6] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel;
                        (sheetRange.Cells[i + startRow, 7] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel;
                        (sheetRange.Cells[i + startRow, 8] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel;

                        // Acc                                              
                        (sheetRange.Cells[i + startRow, 9] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc;
                        (sheetRange.Cells[i + startRow, 10] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc;
                        (sheetRange.Cells[i + startRow, 11] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc;
                        (sheetRange.Cells[i + startRow, 12] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc;
                        (sheetRange.Cells[i + startRow, 13] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc;
                        (sheetRange.Cells[i + startRow, 14] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc;

                        // Dec                                              
                        (sheetRange.Cells[i + startRow, 15] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec;
                        (sheetRange.Cells[i + startRow, 16] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec;
                        (sheetRange.Cells[i + startRow, 17] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec;
                        (sheetRange.Cells[i + startRow, 18] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec;
                        (sheetRange.Cells[i + startRow, 19] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec;
                        (sheetRange.Cells[i + startRow, 20] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec;

                        // S/W Limit                                        
                        (sheetRange.Cells[i + startRow, 21] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].PosLimit.Plus;
                        (sheetRange.Cells[i + startRow, 22] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].PosLimit.Minus;

                        // Limit Time                                       
                        (sheetRange.Cells[i + startRow, 23] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].TimeLimit.tMoveLimit;
                        (sheetRange.Cells[i + startRow, 24] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].TimeLimit.tSleepAfterMove;
                        (sheetRange.Cells[i + startRow, 25] as Excel.Range).Value2 = SystemData_Axis.ACSMotionData[i].TimeLimit.tOriginLimit;
                    }
                    WriteLog($"success : export motor data to excel", ELogType.Debug);
                    WorkBook.Save();
                }
            }
            catch (System.Exception ex)
            {
                WriteExLog(ex.ToString());
                iResult = ERR_DATA_MANAGER_FAIL_EXCEL_EXPORT;

            }
            finally
            {
                WorkBook.Close(true);
                ExcelApp.Quit();
            }

            return iResult;
        }

        public int ImportPolygonData(EPolygonPara file)
        {
            string section = "";
            string key = "";
            string value = "";
            string filePath = "";
            string fileName = "";

            // Job 파일을 설정한다.
            filePath = DBInfo.ScannerDataDir;

            switch (file)
            {
                case EPolygonPara.CONFIG:

                    #region Load Config.ini File
                    fileName = "config.ini";
                    if (!File.Exists(filePath + fileName)) return RUN_FAIL;
                    filePath = filePath + fileName;

                    //----------------------------------------------------------------------
                    section = "Job Settings";

                    key = "InScanResolution";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.000100";
                    ModelData.ScanData.InScanResolution = Convert.ToSingle(value) * 1000.0f;

                    key = "CrossScanResolution";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.000100";
                    ModelData.ScanData.CrossScanResolution = Convert.ToSingle(value) * 1000.0f;

                    key = "InScanOffset";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.0000";
                    ModelData.ScanData.InScanOffset = Convert.ToSingle(value) * 1000.0f;

                    key = "StopMotorBetweenJobs";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.StopMotorBetweenJobs = Convert.ToInt32(value);

                    key = "PixInvert";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.PixInvert = Convert.ToInt32(value);

                    key = "JobStartBufferTime";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.JobStartBufferTime = Convert.ToInt32(value);

                    key = "PrecedingBlankLines";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.PrecedingBlankLines = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "Laser Configuration";

                    key = "LaserOperationMode";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "4";
                    ModelData.ScanData.LaserOperationMode = Convert.ToInt32(value);

                    key = "SeedClockFrequency";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "5000000";
                    ModelData.ScanData.SeedClockFrequency = Convert.ToSingle(value) / 1000.0f;

                    key = "RepetitionRate";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000000";
                    ModelData.ScanData.RepetitionRate = Convert.ToSingle(value) / 1000.0f;

                    ////////////////////////////////////
                    // 이하 4개 값은 자동 계산되는 파라미터임.

                    key = "PulsePickWidth";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.PulsePickWidth = Convert.ToInt32(value);

                    key = "PixelWidth";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.PixelWidth = Convert.ToInt32(value);

                    key = "PulsePickAlgor";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.PulsePickAlgor = Convert.ToInt32(value);


                    //----------------------------------------------------------------------
                    section = "CrossScan Configuration";

                    key = "CrossScanEncoderResol";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.00001";
                    ModelData.ScanData.CrossScanEncoderResol = Convert.ToSingle(value) * 1000.0f;

                    key = "CrossScanMaxAccel";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.1";
                    ModelData.ScanData.CrossScanMaxAccel = Convert.ToSingle(value) ;

                    key = "EnCarSig";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.EnCarSig = Convert.ToInt32(value);

                    key = "SwapCarSig";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.SwapCarSig = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "Head Configuration";

                    key = "SerialNumber";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.SerialNumber = value;

                    key = "FThetaConstant";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.FThetaConstant = value;

                    key = "ExposeLineLength";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.3";
                    ModelData.ScanData.ExposeLineLength = Convert.ToSingle(value) * 1000.0f;

                    key = "EncoderIndexDelay";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.EncoderIndexDelay = Convert.ToInt32(value);

                    key = "FacetFineDelay0";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay0 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay1";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay1 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay2";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay2 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay3";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay3 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay4";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay4 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay5";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay5 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay6";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay6 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay7";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay7 = Convert.ToSingle(value) * 1000.0f;

                    key = "InterleaveRatio";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.InterleaveRatio = Convert.ToInt32(value);

                    key = "FacetFineDelayOffset0";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset0 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset1";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset1 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset2";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset2 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset3";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset3 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset4";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset4 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset5";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset5 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset6";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset6 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset7";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset7 = Convert.ToSingle(value) * 1000.0f;

                    key = "StartFacet";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.StartFacet = Convert.ToInt32(value);

                    key = "AutoIncrementStartFacet";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.AutoIncrementStartFacet = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "Polygon motor Configuration";

                    //key = "InternalMotorDriverClk";
                    //value = CUtils.GetValue(section, key, filePath);
                    //if (value == "") value = "0";
                    //ModelData.ScanData.InternalMotorDriverClk = Convert.ToInt32(value);

                    key = "MotorDriverType";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.MotorDriverType = Convert.ToInt32(value);

                    //key = "MotorSpeed";
                    //value = CUtils.GetValue(section, key, filePath);
                    //if (value == "") value = "0";
                    //ModelData.ScanData.MotorSpeed = Convert.ToInt32(value);

                    //key = "SimEncSel";
                    //value = CUtils.GetValue(section, key, filePath);
                    //if (value == "") value = "0";
                    //ModelData.ScanData.SimEncSel = Convert.ToInt32(value);

                    key = "MinMotorSpeed";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "12.5";
                    ModelData.ScanData.MinMotorSpeed = Convert.ToSingle(value);

                    key = "MaxMotorSpeed";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "55.0";
                    ModelData.ScanData.MaxMotorSpeed = Convert.ToSingle(value);

                    key = "MotorEffectivePoles";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "32";
                    ModelData.ScanData.MotorEffectivePoles = Convert.ToInt32(value);

                    key = "SyncWaitTime";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "25000";
                    ModelData.ScanData.SyncWaitTime = Convert.ToInt32(value);

                    key = "MotorStableTime";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "3000";
                    ModelData.ScanData.MotorStableTime = Convert.ToInt32(value);

                    key = "ShaftEncoderPulseCount";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "5000";
                    ModelData.ScanData.ShaftEncoderPulseCount = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "Other Settings";

                    key = "InterruptFreq";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.InterruptFreq = Convert.ToInt32(value);

                    key = "HWDebugSelection";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.HWDebugSelection = Convert.ToInt32(value);

                    key = "ExpoDebugSelection";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.ExpoDebugSelection = Convert.ToInt32(value);

                    key = "AutoRepeat";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.AutoRepeat = Convert.ToInt32(value);

                    key = "PixAlwaysOn";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.PixAlwaysOn = Convert.ToInt32(value);

                    key = "ExtCamTrig";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.ExtCamTrig = Convert.ToInt32(value);

                    key = "EncoderExpo";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.EncoderExpo = Convert.ToInt32(value);

                    key = "FacetTest";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetTest = Convert.ToInt32(value);

                    key = "SWTest";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.SWTest = Convert.ToInt32(value);

                    key = "JobstartAutorepeat";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "20";
                    ModelData.ScanData.JobstartAutorepeat = Convert.ToInt32(value);

                    #endregion

                    break;

                case EPolygonPara.ISN:

                    #region Load isn.ini File
                    fileName = "isn.ini";
                    if (!File.Exists(filePath + fileName)) return RUN_FAIL;

                    filePath = filePath + fileName;
                    //----------------------------------------------------------------------
                    section = "Global";

                    key = "Enabled";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.IsnEnabled = Convert.ToInt32(value);

                    key = "Home";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.IsnHome = Convert.ToInt32(value);

                    key = "ProfileCtrl";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.IsnProfileCtrl = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "CTRLPOS";

                    key = "PF0S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.IsnPF0S = Convert.ToInt32(value);

                    key = "PF0E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "175";
                    ModelData.ScanData.IsnPF0E = Convert.ToInt32(value);

                    key = "PF1S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "250";
                    ModelData.ScanData.IsnPF1S = Convert.ToInt32(value);

                    key = "PF1E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "425";
                    ModelData.ScanData.IsnPF1E = Convert.ToInt32(value);

                    key = "PF2S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "500";
                    ModelData.ScanData.IsnPF2S = Convert.ToInt32(value);

                    key = "PF2E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "675";
                    ModelData.ScanData.IsnPF2E = Convert.ToInt32(value);

                    key = "PF3S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "750";
                    ModelData.ScanData.IsnPF3S = Convert.ToInt32(value);

                    key = "PF3E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "925";
                    ModelData.ScanData.IsnPF3E = Convert.ToInt32(value);

                    key = "PF4S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.IsnPF4S = Convert.ToInt32(value);

                    key = "PF4E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1175";
                    ModelData.ScanData.IsnPF4E = Convert.ToInt32(value);

                    key = "PF5S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1250";
                    ModelData.ScanData.IsnPF5S = Convert.ToInt32(value);

                    key = "PF5E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1425";
                    ModelData.ScanData.IsnPF5E = Convert.ToInt32(value);

                    key = "PF6S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1500";
                    ModelData.ScanData.IsnPF6S = Convert.ToInt32(value);

                    key = "PF6E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1675";
                    ModelData.ScanData.IsnPF6E = Convert.ToInt32(value);

                    key = "PF7S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1750";
                    ModelData.ScanData.IsnPF7S = Convert.ToInt32(value);

                    key = "PF7E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1925";
                    ModelData.ScanData.IsnPF7E = Convert.ToInt32(value);


                    //----------------------------------------------------------------------
                    section = "CTRLVAL";

                    key = "VF0S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos1 = Convert.ToInt32(value);

                    key = "VF0E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos1 = Convert.ToInt32(value);

                    key = "VF1S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos2 = Convert.ToInt32(value);

                    key = "VF1E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos2 = Convert.ToInt32(value);

                    key = "VF2S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos3 = Convert.ToInt32(value);

                    key = "VF2E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos3 = Convert.ToInt32(value);

                    key = "VF3S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos4 = Convert.ToInt32(value);

                    key = "VF3E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos4 = Convert.ToInt32(value);

                    key = "VF4S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos5 = Convert.ToInt32(value);

                    key = "VF4E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos5 = Convert.ToInt32(value);

                    key = "VF5S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos6 = Convert.ToInt32(value);

                    key = "VF5E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos6 = Convert.ToInt32(value);

                    key = "VF6S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos7 = Convert.ToInt32(value);

                    key = "VF6E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos7 = Convert.ToInt32(value);

                    key = "VF7S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos8 = Convert.ToInt32(value);

                    key = "VF7E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos8 = Convert.ToInt32(value);

                    #endregion

                    break;

                case EPolygonPara.CSN:

                    #region Load csn.ini File
                    fileName = "csn.ini";
                    if (!File.Exists(filePath + fileName)) return RUN_FAIL;

                    filePath = filePath + fileName;
                    //----------------------------------------------------------------------
                    section = "Global";

                    key = "Enabled";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.CsnEnabled = Convert.ToInt32(value);

                    key = "Home";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.CsnHome = Convert.ToInt32(value);

                    key = "ProfileCtrl";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.CsnProfileCtrl = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "CTRLPOS";

                    key = "PF0S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.CsnPF0S = Convert.ToInt32(value);

                    key = "PF0E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "175";
                    ModelData.ScanData.CsnPF0E = Convert.ToInt32(value);

                    key = "PF1S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "250";
                    ModelData.ScanData.CsnPF1S = Convert.ToInt32(value);

                    key = "PF1E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "425";
                    ModelData.ScanData.CsnPF1E = Convert.ToInt32(value);

                    key = "PF2S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "500";
                    ModelData.ScanData.CsnPF2S = Convert.ToInt32(value);

                    key = "PF2E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "675";
                    ModelData.ScanData.CsnPF2E = Convert.ToInt32(value);

                    key = "PF3S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "750";
                    ModelData.ScanData.CsnPF3S = Convert.ToInt32(value);

                    key = "PF3E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "925";
                    ModelData.ScanData.CsnPF3E = Convert.ToInt32(value);

                    key = "PF4S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.CsnPF4S = Convert.ToInt32(value);

                    key = "PF4E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1175";
                    ModelData.ScanData.CsnPF4E = Convert.ToInt32(value);

                    key = "PF5S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1250";
                    ModelData.ScanData.CsnPF5S = Convert.ToInt32(value);

                    key = "PF5E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1425";
                    ModelData.ScanData.CsnPF5E = Convert.ToInt32(value);

                    key = "PF6S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1500";
                    ModelData.ScanData.CsnPF6S = Convert.ToInt32(value);

                    key = "PF6E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1675";
                    ModelData.ScanData.CsnPF6E = Convert.ToInt32(value);

                    key = "PF7S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1750";
                    ModelData.ScanData.CsnPF7S = Convert.ToInt32(value);

                    key = "PF7E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1925";
                    ModelData.ScanData.CsnPF7E = Convert.ToInt32(value);
                    //----------------------------------------------------------------------
                    section = "CTRLVAL";

                    key = "VF0S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos1 = Convert.ToInt32(value);

                    key = "VF0E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos1 = Convert.ToInt32(value);

                    key = "VF1S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos2 = Convert.ToInt32(value);

                    key = "VF1E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos2 = Convert.ToInt32(value);

                    key = "VF2S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos3 = Convert.ToInt32(value);

                    key = "VF2E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos3 = Convert.ToInt32(value);

                    key = "VF3S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos4 = Convert.ToInt32(value);

                    key = "VF3E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos4 = Convert.ToInt32(value);

                    key = "VF4S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos5 = Convert.ToInt32(value);

                    key = "VF4E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos5 = Convert.ToInt32(value);

                    key = "VF5S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos6 = Convert.ToInt32(value);

                    key = "VF5E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos6 = Convert.ToInt32(value);

                    key = "VF6S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos7 = Convert.ToInt32(value);

                    key = "VF6E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos7 = Convert.ToInt32(value);

                    key = "VF7S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos8 = Convert.ToInt32(value);

                    key = "VF7E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos8 = Convert.ToInt32(value);

                    #endregion

                    break;
            }

            return SUCCESS;
        }


        public int ImportPolygonData(EPolygonPara file, string loadFilePath)
        {
            string section = "";
            string key = "";
            string value = "";
            string filePath = "";

            // Job 파일을 설정한다.
            filePath = DBInfo.ScannerDataDir;

            switch (file)
            {
                case EPolygonPara.CONFIG:

                    #region Load Config.ini File
                    if (!File.Exists(loadFilePath)) return RUN_FAIL;
                    filePath = loadFilePath;

                    //----------------------------------------------------------------------
                    section = "Job Settings";

                    key = "InScanResolution";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.000100";
                    ModelData.ScanData.InScanResolution = Convert.ToSingle(value) * 1000.0f;

                    key = "CrossScanResolution";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.000100";
                    ModelData.ScanData.CrossScanResolution = Convert.ToSingle(value) * 1000.0f;

                    key = "InScanOffset";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.0000";
                    ModelData.ScanData.InScanOffset = Convert.ToSingle(value) * 1000.0f;

                    key = "StopMotorBetweenJobs";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.StopMotorBetweenJobs = Convert.ToInt32(value);

                    key = "PixInvert";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.PixInvert = Convert.ToInt32(value);

                    key = "JobStartBufferTime";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.JobStartBufferTime = Convert.ToInt32(value);

                    key = "PrecedingBlankLines";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.PrecedingBlankLines = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "Laser Configuration";

                    key = "LaserOperationMode";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "4";
                    ModelData.ScanData.LaserOperationMode = Convert.ToInt32(value);

                    key = "SeedClockFrequency";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "5000000";
                    ModelData.ScanData.SeedClockFrequency = Convert.ToSingle(value) / 1000.0f;

                    key = "RepetitionRate";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000000";
                    ModelData.ScanData.RepetitionRate = Convert.ToSingle(value) / 1000.0f;

                    ////////////////////////////////////
                    // 이하 4개 값은 자동 계산되는 파라미터임.

                    key = "PulsePickWidth";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.PulsePickWidth = Convert.ToInt32(value);

                    key = "PixelWidth";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.PixelWidth = Convert.ToInt32(value);

                    key = "PulsePickAlgor";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.PulsePickAlgor = Convert.ToInt32(value);


                    //----------------------------------------------------------------------
                    section = "CrossScan Configuration";

                    key = "CrossScanEncoderResol";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.00001";
                    ModelData.ScanData.CrossScanEncoderResol = Convert.ToSingle(value) * 1000.0f;

                    key = "CrossScanMaxAccel";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.1";
                    ModelData.ScanData.CrossScanMaxAccel = Convert.ToSingle(value);

                    key = "EnCarSig";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.EnCarSig = Convert.ToInt32(value);

                    key = "SwapCarSig";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.SwapCarSig = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "Head Configuration";

                    key = "SerialNumber";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.SerialNumber = value;

                    key = "FThetaConstant";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.FThetaConstant = value;

                    key = "ExposeLineLength";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0.3";
                    ModelData.ScanData.ExposeLineLength = Convert.ToSingle(value) * 1000.0f;

                    key = "EncoderIndexDelay";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.EncoderIndexDelay = Convert.ToInt32(value);

                    key = "FacetFineDelay0";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay0 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay1";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay1 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay2";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay2 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay3";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay3 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay4";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay4 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay5";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay5 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay6";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay6 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelay7";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelay7 = Convert.ToSingle(value) * 1000.0f;

                    key = "InterleaveRatio";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.InterleaveRatio = Convert.ToInt32(value);

                    key = "FacetFineDelayOffset0";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset0 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset1";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset1 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset2";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset2 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset3";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset3 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset4";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset4 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset5";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset5 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset6";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset6 = Convert.ToSingle(value) * 1000.0f;

                    key = "FacetFineDelayOffset7";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetFineDelayOffset7 = Convert.ToSingle(value) * 1000.0f;

                    key = "StartFacet";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.StartFacet = Convert.ToInt32(value);

                    key = "AutoIncrementStartFacet";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.AutoIncrementStartFacet = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "Polygon motor Configuration";

                    //key = "InternalMotorDriverClk";
                    //value = CUtils.GetValue(section, key, filePath);
                    //if (value == "") value = "0";
                    //ModelData.ScanData.InternalMotorDriverClk = Convert.ToInt32(value);

                    key = "MotorDriverType";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.MotorDriverType = Convert.ToInt32(value);

                    //key = "MotorSpeed";
                    //value = CUtils.GetValue(section, key, filePath);
                    //if (value == "") value = "0";
                    //ModelData.ScanData.MotorSpeed = Convert.ToInt32(value);

                    //key = "SimEncSel";
                    //value = CUtils.GetValue(section, key, filePath);
                    //if (value == "") value = "0";
                    //ModelData.ScanData.SimEncSel = Convert.ToInt32(value);

                    key = "MinMotorSpeed";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "12.5";
                    ModelData.ScanData.MinMotorSpeed = Convert.ToSingle(value);

                    key = "MaxMotorSpeed";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "55.0";
                    ModelData.ScanData.MaxMotorSpeed = Convert.ToSingle(value);

                    key = "MotorEffectivePoles";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "32";
                    ModelData.ScanData.MotorEffectivePoles = Convert.ToInt32(value);

                    key = "SyncWaitTime";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "25000";
                    ModelData.ScanData.SyncWaitTime = Convert.ToInt32(value);

                    key = "MotorStableTime";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "3000";
                    ModelData.ScanData.MotorStableTime = Convert.ToInt32(value);

                    key = "ShaftEncoderPulseCount";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "5000";
                    ModelData.ScanData.ShaftEncoderPulseCount = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "Other Settings";

                    key = "InterruptFreq";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.InterruptFreq = Convert.ToInt32(value);

                    key = "HWDebugSelection";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.HWDebugSelection = Convert.ToInt32(value);

                    key = "ExpoDebugSelection";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.ExpoDebugSelection = Convert.ToInt32(value);

                    key = "AutoRepeat";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.AutoRepeat = Convert.ToInt32(value);

                    key = "PixAlwaysOn";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.PixAlwaysOn = Convert.ToInt32(value);

                    key = "ExtCamTrig";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.ExtCamTrig = Convert.ToInt32(value);

                    key = "EncoderExpo";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.EncoderExpo = Convert.ToInt32(value);

                    key = "FacetTest";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetTest = Convert.ToInt32(value);

                    key = "SWTest";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.SWTest = Convert.ToInt32(value);

                    key = "JobstartAutorepeat";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "20";
                    ModelData.ScanData.JobstartAutorepeat = Convert.ToInt32(value);

                    #endregion

                    break;

                case EPolygonPara.ISN:

                    #region Load isn.ini File
                    if (!File.Exists(loadFilePath)) return RUN_FAIL;

                    filePath = loadFilePath;
                    //----------------------------------------------------------------------
                    section = "Global";

                    key = "Enabled";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.IsnEnabled = Convert.ToInt32(value);

                    key = "Home";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.IsnHome = Convert.ToInt32(value);

                    key = "ProfileCtrl";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.IsnProfileCtrl = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "CTRLPOS";

                    key = "PF0S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.IsnPF0S = Convert.ToInt32(value);

                    key = "PF0E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "175";
                    ModelData.ScanData.IsnPF0E = Convert.ToInt32(value);

                    key = "PF1S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "250";
                    ModelData.ScanData.IsnPF1S = Convert.ToInt32(value);

                    key = "PF1E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "425";
                    ModelData.ScanData.IsnPF1E = Convert.ToInt32(value);

                    key = "PF2S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "500";
                    ModelData.ScanData.IsnPF2S = Convert.ToInt32(value);

                    key = "PF2E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "675";
                    ModelData.ScanData.IsnPF2E = Convert.ToInt32(value);

                    key = "PF3S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "750";
                    ModelData.ScanData.IsnPF3S = Convert.ToInt32(value);

                    key = "PF3E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "925";
                    ModelData.ScanData.IsnPF3E = Convert.ToInt32(value);

                    key = "PF4S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.IsnPF4S = Convert.ToInt32(value);

                    key = "PF4E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1175";
                    ModelData.ScanData.IsnPF4E = Convert.ToInt32(value);

                    key = "PF5S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1250";
                    ModelData.ScanData.IsnPF5S = Convert.ToInt32(value);

                    key = "PF5E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1425";
                    ModelData.ScanData.IsnPF5E = Convert.ToInt32(value);

                    key = "PF6S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1500";
                    ModelData.ScanData.IsnPF6S = Convert.ToInt32(value);

                    key = "PF6E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1675";
                    ModelData.ScanData.IsnPF6E = Convert.ToInt32(value);

                    key = "PF7S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1750";
                    ModelData.ScanData.IsnPF7S = Convert.ToInt32(value);

                    key = "PF7E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1925";
                    ModelData.ScanData.IsnPF7E = Convert.ToInt32(value);


                    //----------------------------------------------------------------------
                    section = "CTRLVAL";

                    key = "VF0S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos1 = Convert.ToInt32(value);

                    key = "VF0E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos1 = Convert.ToInt32(value);

                    key = "VF1S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos2 = Convert.ToInt32(value);

                    key = "VF1E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos2 = Convert.ToInt32(value);

                    key = "VF2S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos3 = Convert.ToInt32(value);

                    key = "VF2E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos3 = Convert.ToInt32(value);

                    key = "VF3S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos4 = Convert.ToInt32(value);

                    key = "VF3E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos4 = Convert.ToInt32(value);

                    key = "VF4S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos5 = Convert.ToInt32(value);

                    key = "VF4E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos5 = Convert.ToInt32(value);

                    key = "VF5S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos6 = Convert.ToInt32(value);

                    key = "VF5E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos6 = Convert.ToInt32(value);

                    key = "VF6S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos7 = Convert.ToInt32(value);

                    key = "VF6E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos7 = Convert.ToInt32(value);

                    key = "VF7S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstXpos8 = Convert.ToInt32(value);

                    key = "VF7E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Xpos8 = Convert.ToInt32(value);

                    #endregion

                    break;

                case EPolygonPara.CSN:

                    #region Load csn.ini File
                    if (!File.Exists(loadFilePath)) return RUN_FAIL;

                    filePath = loadFilePath;
                    //----------------------------------------------------------------------
                    section = "Global";

                    key = "Enabled";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.CsnEnabled = Convert.ToInt32(value);

                    key = "Home";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.CsnHome = Convert.ToInt32(value);

                    key = "ProfileCtrl";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1";
                    ModelData.ScanData.CsnProfileCtrl = Convert.ToInt32(value);

                    //----------------------------------------------------------------------
                    section = "CTRLPOS";

                    key = "PF0S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.CsnPF0S = Convert.ToInt32(value);

                    key = "PF0E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "175";
                    ModelData.ScanData.CsnPF0E = Convert.ToInt32(value);

                    key = "PF1S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "250";
                    ModelData.ScanData.CsnPF1S = Convert.ToInt32(value);

                    key = "PF1E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "425";
                    ModelData.ScanData.CsnPF1E = Convert.ToInt32(value);

                    key = "PF2S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "500";
                    ModelData.ScanData.CsnPF2S = Convert.ToInt32(value);

                    key = "PF2E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "675";
                    ModelData.ScanData.CsnPF2E = Convert.ToInt32(value);

                    key = "PF3S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "750";
                    ModelData.ScanData.CsnPF3S = Convert.ToInt32(value);

                    key = "PF3E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "925";
                    ModelData.ScanData.CsnPF3E = Convert.ToInt32(value);

                    key = "PF4S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1000";
                    ModelData.ScanData.CsnPF4S = Convert.ToInt32(value);

                    key = "PF4E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1175";
                    ModelData.ScanData.CsnPF4E = Convert.ToInt32(value);

                    key = "PF5S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1250";
                    ModelData.ScanData.CsnPF5S = Convert.ToInt32(value);

                    key = "PF5E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1425";
                    ModelData.ScanData.CsnPF5E = Convert.ToInt32(value);

                    key = "PF6S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1500";
                    ModelData.ScanData.CsnPF6S = Convert.ToInt32(value);

                    key = "PF6E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1675";
                    ModelData.ScanData.CsnPF6E = Convert.ToInt32(value);

                    key = "PF7S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1750";
                    ModelData.ScanData.CsnPF7S = Convert.ToInt32(value);

                    key = "PF7E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "1925";
                    ModelData.ScanData.CsnPF7E = Convert.ToInt32(value);
                    //----------------------------------------------------------------------
                    section = "CTRLVAL";

                    key = "VF0S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos1 = Convert.ToInt32(value);

                    key = "VF0E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos1 = Convert.ToInt32(value);

                    key = "VF1S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos2 = Convert.ToInt32(value);

                    key = "VF1E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos2 = Convert.ToInt32(value);

                    key = "VF2S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos3 = Convert.ToInt32(value);

                    key = "VF2E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos3 = Convert.ToInt32(value);

                    key = "VF3S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos4 = Convert.ToInt32(value);

                    key = "VF3E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos4 = Convert.ToInt32(value);

                    key = "VF4S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos5 = Convert.ToInt32(value);

                    key = "VF4E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos5 = Convert.ToInt32(value);

                    key = "VF5S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos6 = Convert.ToInt32(value);

                    key = "VF5E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos6 = Convert.ToInt32(value);

                    key = "VF6S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos7 = Convert.ToInt32(value);

                    key = "VF6E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos7 = Convert.ToInt32(value);

                    key = "VF7S";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectFirstYpos8 = Convert.ToInt32(value);

                    key = "VF7E";
                    value = CUtils.GetValue(section, key, filePath);
                    if (value == "") value = "0";
                    ModelData.ScanData.FacetCorrectLast_Ypos8 = Convert.ToInt32(value);

                    #endregion

                    break;
            }

            return SUCCESS;
        }

        public int ExportPolygonData(EPolygonPara file, string loadFilePath)
        {
            string section = "";
            string key = "";
            string value = "";
            string filePath = "";
            bool bRet = false;

            // Job 파일을 설정한다.
            filePath = DBInfo.ScannerDataDir;

            switch (file)
            {
                case EPolygonPara.CONFIG:

                    #region Load Config.ini File
                    filePath = loadFilePath;
                    if (!File.Exists(filePath)) File.Create(filePath);


                    //----------------------------------------------------------------------
                    //----------------------------------------------------------------------
                    section = "Job Settings";

                    key = "InScanResolution";
                    value = string.Format("{0:F6}", ModelData.ScanData.InScanResolution / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "CrossScanResolution";
                    value = string.Format("{0:F7}", ModelData.ScanData.CrossScanResolution / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "InScanOffset";
                    value = string.Format("{0:F6}", ModelData.ScanData.InScanOffset / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "StopMotorBetweenJobs";
                    value = Convert.ToString(ModelData.ScanData.StopMotorBetweenJobs);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PixInvert";
                    value = Convert.ToString(ModelData.ScanData.PixInvert);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "JobStartBufferTime";
                    value = Convert.ToString(ModelData.ScanData.JobStartBufferTime);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PrecedingBlankLines";
                    value = Convert.ToString(ModelData.ScanData.PrecedingBlankLines);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "Laser Configuration";

                    key = "LaserOperationMode";
                    value = Convert.ToString(ModelData.ScanData.LaserOperationMode);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "SeedClockFrequency";
                    value = string.Format("{0:F0}", ModelData.ScanData.SeedClockFrequency * 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "RepetitionRate";
                    value = Convert.ToString(ModelData.ScanData.RepetitionRate * 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PulsePickWidth";
                    value = Convert.ToString(ModelData.ScanData.PulsePickWidth);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PixelWidth";
                    value = Convert.ToString(ModelData.ScanData.PixelWidth);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PulsePickAlgor";
                    value = Convert.ToString(ModelData.ScanData.PulsePickAlgor);
                    bRet = CUtils.SetValue(section, key, value, filePath);


                    //----------------------------------------------------------------------
                    section = "CrossScan Configuration";

                    key = "CrossScanEncoderResol";
                    value = string.Format("{0:F7}", ModelData.ScanData.CrossScanEncoderResol / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "CrossScanMaxAccel";
                    value = string.Format("{0:F2}", ModelData.ScanData.CrossScanMaxAccel);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "EnCarSig";
                    value = Convert.ToString(ModelData.ScanData.EnCarSig);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "SwapCarSig";
                    value = Convert.ToString(ModelData.ScanData.SwapCarSig);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "Head Configuration";

                    key = "SerialNumber";
                    value = string.Format("{0:F7}", ModelData.ScanData.SerialNumber);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FThetaConstant";
                    value = string.Format("{0:F7}", ModelData.ScanData.FThetaConstant);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ExposeLineLength";
                    value = string.Format("{0:F6}", ModelData.ScanData.ExposeLineLength / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "EncoderIndexDelay";
                    value = Convert.ToString(ModelData.ScanData.EncoderIndexDelay);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay0";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay0 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay1";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay1 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay2";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay2 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay3";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay3 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay4";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay4 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay5";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay5 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay6";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay6 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay7";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay7 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "InterleaveRatio";
                    value = Convert.ToString(ModelData.ScanData.InterleaveRatio);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset0";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset0 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset1";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset1 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset2";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset2 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset3";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset3 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset4";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset4 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset5";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset5 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset6";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset6 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset7";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset7 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "StartFacet";
                    value = Convert.ToString(ModelData.ScanData.StartFacet);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "AutoIncrementStartFacet";
                    value = Convert.ToString(ModelData.ScanData.AutoIncrementStartFacet);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "Polygon motor Configuration";

                    //key = "InternalMotorDriverClk";
                    //value = Convert.ToString(ModelData.ScanData.InternalMotorDriverClk);
                    //bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MotorDriverType";
                    value = Convert.ToString(ModelData.ScanData.MotorDriverType);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //key = "MotorSpeed";
                    //value = Convert.ToString(ModelData.ScanData.MotorSpeed);
                    //bRet = CUtils.SetValue(section, key, value, filePath);

                    //key = "SimEncSel";
                    //value = Convert.ToString(ModelData.ScanData.SimEncSel);
                    //bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MinMotorSpeed";
                    value = string.Format("{0:F2}", ModelData.ScanData.MinMotorSpeed);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MaxMotorSpeed";
                    value = string.Format("{0:F2}", ModelData.ScanData.MaxMotorSpeed);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MotorEffectivePoles";
                    value = Convert.ToString(ModelData.ScanData.MotorEffectivePoles);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "SyncWaitTime";
                    value = Convert.ToString(ModelData.ScanData.SyncWaitTime);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MotorStableTime";
                    value = Convert.ToString(ModelData.ScanData.MotorStableTime);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ShaftEncoderPulseCount";
                    value = Convert.ToString(ModelData.ScanData.ShaftEncoderPulseCount);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "Other Settings";

                    key = "InterruptFreq";
                    value = Convert.ToString(ModelData.ScanData.InterruptFreq);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "HWDebugSelection";
                    value = Convert.ToString(ModelData.ScanData.HWDebugSelection);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ExpoDebugSelection";
                    value = Convert.ToString(ModelData.ScanData.ExpoDebugSelection);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "AutoRepeat";
                    value = Convert.ToString(ModelData.ScanData.AutoRepeat);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PixAlwaysOn";
                    value = Convert.ToString(ModelData.ScanData.PixAlwaysOn);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ExtCamTrig";
                    value = Convert.ToString(ModelData.ScanData.ExtCamTrig);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "EncoderExpo";
                    value = Convert.ToString(ModelData.ScanData.EncoderExpo);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetTest";
                    value = Convert.ToString(ModelData.ScanData.FacetTest);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "SWTest";
                    value = Convert.ToString(ModelData.ScanData.SWTest);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "JobstartAutorepeat";
                    value = Convert.ToString(ModelData.ScanData.JobstartAutorepeat);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    #endregion

                    break;

                case EPolygonPara.ISN:

                    #region Load isn.ini File
                    filePath = loadFilePath;
                    if (!File.Exists(filePath)) File.Create(filePath);
                    //----------------------------------------------------------------------
                    section = "Global";

                    key = "Enabled";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnEnabled);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "Home";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnHome);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ProfileCtrl";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnProfileCtrl);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "CTRLPOS";

                    key = "PF0S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF0S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF0E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF0E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF1S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF1S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF1E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF1E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF2S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF2S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF2E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF2E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF3S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF3S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF3E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF3E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF4S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF4S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF4E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF4E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF5S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF5S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF5E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF5E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF6S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF6S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF6E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF6E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF7S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF7S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF7E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF7E);
                    bRet = CUtils.SetValue(section, key, value, filePath);


                    //----------------------------------------------------------------------
                    section = "CTRLVAL";

                    key = "VF0S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos1);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF0E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos1);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF1S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos2);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF1E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos2);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF2S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos3);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF2E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos3);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF3S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos4);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF3E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos4);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF4S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos5);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF4E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos5);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF5S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos6);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF5E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos6);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF6S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos7);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF6E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos7);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF7S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos8);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF7E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos8);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    #endregion

                    break;

                case EPolygonPara.CSN:

                    #region Load csn.ini File
                    filePath = loadFilePath;
                    if (!File.Exists(filePath)) File.Create(filePath);
                    //----------------------------------------------------------------------
                    section = "Global";

                    key = "Enabled";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnEnabled);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "Home";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnHome);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ProfileCtrl";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnProfileCtrl);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "CTRLPOS";

                    key = "PF0S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF0S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF0E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF0E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF1S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF1S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF1E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF1E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF2S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF2S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF2E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF2E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF3S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF3S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF3E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF3E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF4S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF4S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF4E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF4E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF5S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF5S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF5E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF5E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF6S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF6S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF6E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF6E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF7S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF7S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF7E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF7E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "CTRLVAL";

                    key = "VF0S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos1);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF0E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos1);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF1S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos2);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF1E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos2);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF2S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos3);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF2E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos3);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF3S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos4);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF3E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos4);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF4S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos5);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF4E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos5);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF5S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos6);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF5E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos6);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF6S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos7);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF6E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos7);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF7S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos8);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF7E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos8);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    #endregion

                    break;
            }

            return SUCCESS;
        }


        public int ExportPolygonData(EPolygonPara file)
        {
            string section = "";
            string key = "";
            string value = "";
            string filePath = "";
            string fileName = "";
            bool bRet = false;

            // Job 파일을 설정한다.
            filePath = DBInfo.ScannerDataDir;

            switch (file)
            {
                case EPolygonPara.CONFIG:

                    #region Load Config.ini File
                    fileName = "config.ini";
                    filePath = filePath + fileName;
                    if (!File.Exists(filePath)) File.Create(filePath);


                    //----------------------------------------------------------------------
                    //----------------------------------------------------------------------
                    section = "Job Settings";

                    key = "InScanResolution";
                    value = string.Format("{0:F6}", ModelData.ScanData.InScanResolution / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "CrossScanResolution";
                    value = string.Format("{0:F7}", ModelData.ScanData.CrossScanResolution / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "InScanOffset";
                    value = string.Format("{0:F6}", ModelData.ScanData.InScanOffset / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "StopMotorBetweenJobs";
                    value = Convert.ToString(ModelData.ScanData.StopMotorBetweenJobs);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PixInvert";
                    value = Convert.ToString(ModelData.ScanData.PixInvert);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "JobStartBufferTime";
                    value = Convert.ToString(ModelData.ScanData.JobStartBufferTime);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PrecedingBlankLines";
                    value = Convert.ToString(ModelData.ScanData.PrecedingBlankLines);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "Laser Configuration";

                    key = "LaserOperationMode";
                    value = Convert.ToString(ModelData.ScanData.LaserOperationMode);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "SeedClockFrequency";
                    value = string.Format("{0:F0}", ModelData.ScanData.SeedClockFrequency * 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "RepetitionRate";
                    value = Convert.ToString(ModelData.ScanData.RepetitionRate * 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PulsePickWidth";
                    value = Convert.ToString(ModelData.ScanData.PulsePickWidth);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PixelWidth";
                    value = Convert.ToString(ModelData.ScanData.PixelWidth);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PulsePickAlgor";
                    value = Convert.ToString(ModelData.ScanData.PulsePickAlgor);
                    bRet = CUtils.SetValue(section, key, value, filePath);


                    //----------------------------------------------------------------------
                    section = "CrossScan Configuration";

                    key = "CrossScanEncoderResol";
                    value = string.Format("{0:F7}", ModelData.ScanData.CrossScanEncoderResol / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "CrossScanMaxAccel";
                    value = string.Format("{0:F2}", ModelData.ScanData.CrossScanMaxAccel);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "EnCarSig";
                    value = Convert.ToString(ModelData.ScanData.EnCarSig);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "SwapCarSig";
                    value = Convert.ToString(ModelData.ScanData.SwapCarSig);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "Head Configuration";

                    key = "SerialNumber";
                    value = string.Format("{0:F7}", ModelData.ScanData.SerialNumber);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FThetaConstant";
                    value = string.Format("{0:F7}", ModelData.ScanData.FThetaConstant);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ExposeLineLength";
                    value = string.Format("{0:F6}", ModelData.ScanData.ExposeLineLength / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "EncoderIndexDelay";
                    value = Convert.ToString(ModelData.ScanData.EncoderIndexDelay);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay0";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay0 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay1";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay1 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay2";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay2 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay3";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay3 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay4";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay4 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay5";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay5 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay6";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay6 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelay7";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelay7 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "InterleaveRatio";
                    value = Convert.ToString(ModelData.ScanData.InterleaveRatio);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset0";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset0 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset1";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset1 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset2";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset2 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset3";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset3 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset4";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset4 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset5";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset5 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset6";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset6 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetFineDelayOffset7";
                    value = string.Format("{0:F6}", ModelData.ScanData.FacetFineDelayOffset7 / 1000.0f);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "StartFacet";
                    value = Convert.ToString(ModelData.ScanData.StartFacet);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "AutoIncrementStartFacet";
                    value = Convert.ToString(ModelData.ScanData.AutoIncrementStartFacet);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "Polygon motor Configuration";

                    //key = "InternalMotorDriverClk";
                    //value = Convert.ToString(ModelData.ScanData.InternalMotorDriverClk);
                    //bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MotorDriverType";
                    value = Convert.ToString(ModelData.ScanData.MotorDriverType);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //key = "MotorSpeed";
                    //value = Convert.ToString(ModelData.ScanData.MotorSpeed);
                    //bRet = CUtils.SetValue(section, key, value, filePath);

                    //key = "SimEncSel";
                    //value = Convert.ToString(ModelData.ScanData.SimEncSel);
                    //bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MinMotorSpeed";
                    value = string.Format("{0:F2}", ModelData.ScanData.MinMotorSpeed);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MaxMotorSpeed";
                    value = string.Format("{0:F2}", ModelData.ScanData.MaxMotorSpeed);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MotorEffectivePoles";
                    value = Convert.ToString(ModelData.ScanData.MotorEffectivePoles);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "SyncWaitTime";
                    value = Convert.ToString(ModelData.ScanData.SyncWaitTime);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "MotorStableTime";
                    value = Convert.ToString(ModelData.ScanData.MotorStableTime);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ShaftEncoderPulseCount";
                    value = Convert.ToString(ModelData.ScanData.ShaftEncoderPulseCount);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "Other Settings";

                    key = "InterruptFreq";
                    value = Convert.ToString(ModelData.ScanData.InterruptFreq);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "HWDebugSelection";
                    value = Convert.ToString(ModelData.ScanData.HWDebugSelection);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ExpoDebugSelection";
                    value = Convert.ToString(ModelData.ScanData.ExpoDebugSelection);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "AutoRepeat";
                    value = Convert.ToString(ModelData.ScanData.AutoRepeat);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PixAlwaysOn";
                    value = Convert.ToString(ModelData.ScanData.PixAlwaysOn);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ExtCamTrig";
                    value = Convert.ToString(ModelData.ScanData.ExtCamTrig);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "EncoderExpo";
                    value = Convert.ToString(ModelData.ScanData.EncoderExpo);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "FacetTest";
                    value = Convert.ToString(ModelData.ScanData.FacetTest);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "SWTest";
                    value = Convert.ToString(ModelData.ScanData.SWTest);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "JobstartAutorepeat";
                    value = Convert.ToString(ModelData.ScanData.JobstartAutorepeat);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    #endregion

                    break;

                case EPolygonPara.ISN:

                    #region Load isn.ini File
                    fileName = "isn.ini";
                    filePath = filePath + fileName;
                    if (!File.Exists(filePath)) File.Create(filePath);
                    //----------------------------------------------------------------------
                    section = "Global";

                    key = "Enabled";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnEnabled);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "Home";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnHome);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ProfileCtrl";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnProfileCtrl);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "CTRLPOS";

                    key = "PF0S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF0S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF0E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF0E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF1S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF1S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF1E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF1E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF2S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF2S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF2E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF2E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF3S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF3S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF3E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF3E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF4S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF4S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF4E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF4E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF5S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF5S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF5E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF5E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF6S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF6S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF6E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF6E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF7S";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF7S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF7E";
                    value = string.Format("{0:F0}", ModelData.ScanData.IsnPF7E);
                    bRet = CUtils.SetValue(section, key, value, filePath);


                    //----------------------------------------------------------------------
                    section = "CTRLVAL";

                    key = "VF0S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos1);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF0E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos1);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF1S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos2);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF1E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos2);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF2S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos3);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF2E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos3);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF3S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos4);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF3E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos4);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF4S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos5);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF4E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos5);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF5S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos6);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF5E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos6);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF6S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos7);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF6E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos7);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF7S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstXpos8);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF7E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Xpos8);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    #endregion

                    break;

                case EPolygonPara.CSN:

                    #region Load csn.ini File
                    fileName = "csn.ini";
                    filePath = filePath + fileName;
                    if (!File.Exists(filePath)) File.Create(filePath);
                    //----------------------------------------------------------------------
                    section = "Global";

                    key = "Enabled";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnEnabled);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "Home";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnHome);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "ProfileCtrl";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnProfileCtrl);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "CTRLPOS";

                    key = "PF0S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF0S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF0E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF0E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF1S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF1S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF1E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF1E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF2S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF2S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF2E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF2E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF3S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF3S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF3E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF3E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF4S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF4S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF4E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF4E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF5S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF5S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF5E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF5E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF6S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF6S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF6E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF6E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF7S";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF7S);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "PF7E";
                    value = string.Format("{0:F0}", ModelData.ScanData.CsnPF7E);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    //----------------------------------------------------------------------
                    section = "CTRLVAL";

                    key = "VF0S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos1);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF0E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos1);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF1S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos2);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF1E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos2);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF2S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos3);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF2E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos3);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF3S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos4);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF3E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos4);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF4S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos5);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF4E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos5);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF5S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos6);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF5E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos6);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF6S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos7);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF6E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos7);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF7S";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectFirstYpos8);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    key = "VF7E";
                    value = string.Format("{0:F0}", ModelData.ScanData.FacetCorrectLast_Ypos8);
                    bRet = CUtils.SetValue(section, key, value, filePath);

                    #endregion

                    break;
            }

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

            // STAGE T
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

#if EQUIP_DICING_DEV
            // CAMERA1_Z                                           
            index = (int)EYMC_Axis.CAMERA1_Z;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "CAM1_Z"; // "CAMERA1_Z";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }

            // SCANNER_Z1
            index = (int)EYMC_Axis.SCANNER_Z1;
            if (SystemData_Axis.MPMotionData[index].Name == "NotExist")
            {
                tMotion = new CMPMotionData();
                tMotion.Name = "SCAN_Z1"; // "SCANNER_Z1";
                tMotion.Exist = true;

                SystemData_Axis.MPMotionData[index] = ObjectExtensions.Copy(tMotion);
            }
#endif
        }

        public int Generate_InputBuffer()
        {
            WorkPiece_InputBuffer.Clear();
            for(int i = 0; i < 10; i++)
            {
                CWorkPiece wp = new CWorkPiece();
                wp.GenerateID($"_{i}");
                WorkPiece_InputBuffer.Add(wp);
                Sleep(100);
            }

            return SUCCESS;
        }

        public string GetID_InputReady()
        {
            if (WorkPiece_InputBuffer.Count <= 0) return "";
            return WorkPiece_InputBuffer[0].ID;
        }

        public string GetID_LastOutput()
        {
            if (WorkPiece_OutputBuffer.Count <= 0) return "";
            return WorkPiece_OutputBuffer[WorkPiece_OutputBuffer.Count-1].ID;
        }

        public int GetCount_InputBuffer()
        {
            return WorkPiece_InputBuffer.Count;
        }

        public int GetCount_OutputBuffer()
        {
            return WorkPiece_OutputBuffer.Count;
        }

        public int LoadWorkPieceFromCassette()
        {
            if (WorkPiece_InputBuffer.Count <= 0)
                return GenerateErrorCode(ERR_DATA_MANAGER_INPUT_BUFFER_EMPTY);

            WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL] = ObjectExtensions.Copy(WorkPiece_InputBuffer[0]);
            WorkPiece_InputBuffer.RemoveAt(0);

            return SUCCESS;
        }

        public int UnloadWorkPieceToCassette()
        {
            WorkPiece_OutputBuffer.Add(ObjectExtensions.Copy(WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL]));
            //WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL] = new CWorkPiece();
            WorkPieceArray[(int)ELCNetUnitPos.PUSHPULL].Init();

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
#if SIMULATION_TEST
            Sleep(SimulationSleepTime); // for test
#endif
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
#if SIMULATION_TEST
            Sleep(SimulationSleepTime); // for test
#endif
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
