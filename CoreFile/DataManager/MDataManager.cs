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

//### using Excel = Microsoft.Office.Interop.Excel;

using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_OpPanel;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_LCNet;

using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_Cylinder;
using static Core.Layers.DEF_Vacuum;

using static Core.Layers.DEF_MeStage;
using static Core.Layers.DEF_Vision;
using static Core.Layers.DEF_CtrlStage;

namespace Core.Layers
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
        

        public class CSystemDataFileNames
        {
            public string SystemDataFile;
            public string LogDataFile;
            public string ProductDataFile;
            public string CalibrationDataFile;
            public string TeachingDataFile;
    
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
        public class CSystemData_MaxSafetyPos
        {
            // Stage
            public CPos_XYTZ Stage_Pos;
            public CPos_XYTZ Camera_Pos;
            public CPos_XYTZ Scanner_Pos;
        }

        public class CSystemData
        {
            //////////////////////////////////////////////////////////////////////////////
            // System General
            public ELanguage Language = ELanguage.KOREAN;
            public string ModelName = NAME_DEFAULT_MODEL;

            public bool CheckSafety_AutoMode = true; // AutoMode에서 동작 전에 SafeSensor Check 여부
            public bool CheckSafety_ManualMode = true; // AutoMode에서 동작 전에 SafeSensor Check 여부
            public bool EnableCylinderMove_EStop = false; // EStop 상태에서 cylinder move 가능 여부

            // Use Door Sensor : if true, check door status whether opened.
            public bool[,] UseDoorStatus = new bool[(int)EDoorGroup.MAX, (int)EDoorIndex.MAX];
            // Use Area Sensor
            public bool[] UseAreaSensor = new bool[(int)EAreaSensor.MAX];

            public bool[] UseTankAlarm = new bool[(int)ECoatTank.MAX]; // 자재 교체요청 알람 사용 여부

            // thread 간의 handshake를 one step으로 처리할지 여부
            public bool ThreadHandshake_byOneStep = true;

            // SafetyPos for Axis Move 
            // Teching 화면에서 Teaching하는 UnitPos.WaitPos 과는 다른 용도로, maker & engineer가 시스템적으로 지정하는 절대 안전 위치
            public CSystemData_MaxSafetyPos MaxSafetyPos = new CSystemData_MaxSafetyPos();
            

            //////////////////////////////////////////////////////////////////////////////
            // 아래는 아직 미정리 내역들. 
            // * 혹시, 아래에서 사용하는것들은 이 주석 위로 올려주기 바람
            //
            public string PassWord;     // Engineer Password


            public int SystemType;      // 작업변
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
            public double Stage1JogSpeed    = 10.0;
            public double Stage1ThetaJogSpeed = 10.0;
            
            public double VisionLaserDistance = 0.0;

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
            

            public double UVLampMaxTime;                  // 허용 램프 최대 수명

            // Pumping Job
            public int DoPumpingIntervalTime;       // 펌핑 잡 Interval
            public int DoPumpingTime;               // 펌핑 잡 총 동작 시간
            public int Pumping_OneShot_Interval;    // 매 일회 펌핑 동작당 대기시간
            public bool UseUseWorkbenchVacuum;      // Workbench Vacuum 사용 유무
            
            public CSystemData()
            {
                ArrayExtensions.Init(UseTankAlarm, false);
                ArrayExtensions.Init(UseDoorStatus, false);
                ArrayExtensions.Init(UseAreaSensor, false);
            }
        }

        public class CSystemData_Axis
        {

            
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

            public CPos_XY CamEachOffset = new CPos_XY();

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
            

            // Stage
            public CPositionSet Pos_Stage1           = new CPositionSet((int)EStagePos.MAX);
            public CPositionSet Pos_Camera1          = new CPositionSet((int)ECameraPos.MAX);
            public CPositionSet Pos_Scanner1         = new CPositionSet((int)EScannerPos.MAX);

            public CPositionGroup()
            {
                int index = 0;
                Pos_Array[index++] = Pos_Stage1;
                Pos_Array[index++] = Pos_Camera1;
                Pos_Array[index++] = Pos_Scanner1;

                for (int i=0; i< (int)EPositionObject.MAX; i++)
                {
                    for (int j=0; j < Pos_Array[i].Pos.Length; j++)
                    {
                        Pos_Array[i].Pos[j] = new CPos_XYTZ();
                    }
                }
            }

            public void UpdatePositionSet(CPositionGroup tGroup, EPositionObject index)
            {
                //Pos_Array[(int)index] = ObjectExtensions.Copy(tGroup.Pos_Array[(int)index]);
                // 프로그램 완성 과정중에, position 이 추가/삭제 되면서 배열 크기가 달라지고
                // ObjectExtensions.Copy 과정중에서 array size가 변경되기때문에, 우선은 있는것들만 복사하도록 변경
                int size = (Pos_Array[(int)index].Length < tGroup.Pos_Array[(int)index].Length) ? Pos_Array[(int)index].Length : tGroup.Pos_Array[(int)index].Length;
                for (int i = 0; i < size; i++)
                {
                    Pos_Array[(int)index].Pos[i] = ObjectExtensions.Copy(tGroup.Pos_Array[(int)index].Pos[i]);
                }
            }

            public void UpdatePositionSet(CPositionSet tSet, EPositionObject index)
            {

                //Pos_Array[(int)index] = ObjectExtensions.Copy(tSet);
                // 프로그램 완성 과정중에, position 이 추가/삭제 되면서 배열 크기가 달라지고
                // ObjectExtensions.Copy 과정중에서 array size가 변경되기때문에, 우선은 있는것들만 복사하도록 변경
                int size = (Pos_Array[(int)index].Length < tSet.Length) ? Pos_Array[(int)index].Length : tSet.Length;
                for (int i = 0; i < size; i++)
                {
                    Pos_Array[(int)index].Pos[i] = ObjectExtensions.Copy(tSet.Pos[i]);
                }
            }
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
        public const string NAME_DEFAULT_ENGINEER       = "Engineer";
        public const string NAME_MAKER                  = "Maker";


        public enum EListHeaderType
        {
            MODEL = 0,
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
            // Align Data
            public CCtrlAlignData AlignData = new CCtrlAlignData();
            
            
            // Function Parameter

            // Mechanical Layer

            // MeStage
            public bool[] MeStage_UseVccFlag = new bool[(int)EStageVacuum.MAX];

            // Control Layer


            ///////////////////////////////////////////////////////////////
            // 이하 아래부분 미정리한것들임
            public bool Use2Step_Use;
            
            public bool UseUHandler_ExtraVccUseFlag; // 2014.02.21 by ranian. Extra Vcc 추가
            public bool UseUHandler_WaitPosUseFlag; // 2014.02.21 by ranian. LP->UP 로 갈 때, WP 사용 여부

            public CModelData()
            {

            }
        }
        
    }

    public class MDataManager : MObject
    {
        public CDBInfo DBInfo { get; private set; }

        /////////////////////////////////////////////////////////////////////////////////
        // System Model Data
        public CSystemData SystemData { get; private set; } = new CSystemData();

        public CSystemData_Axis SystemData_Axis { get; private set; } = new CSystemData_Axis();
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

            int iResult = SUCCESS;
            iResult = LoadGeneralData();
            if (iResult != SUCCESS) return iResult;

            // 아래의 네가지 함수 콜은 Core의 Initialize에서 읽어들이는게 맞지만, 생성자에서 한번 더 읽어도 되기에.. 주석처리해도 상관없음
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
            //iResult = ChangeModel(ModelData.Name);
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
            CSystemData_Align systemAlign =null,CSystemData_Light systemLight = null)
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
            int iResult = SUCCESS;
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
                EPositionObject tUnit = EPositionObject.STAGE1 + i;
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
            int iResult = SUCCESS;

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
            int iResult = SUCCESS;
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
                EPositionObject tUnit = EPositionObject.STAGE1 + i;
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

                                for(int i= tData.Pos_Stage1.Length; i < (int)EStagePos.MAX; i++)
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
            int iResult = SUCCESS;

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
            int iResult = SUCCESS;
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
            

            return SUCCESS;
        }

        public int LoadModelList()
        {
            LoadModelList(EListHeaderType.MODEL);
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
            int iResult = SUCCESS;
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
                
                // 3.1 load waferframe data
                //iResult = LoadWaferFrameData(data.WaferFrameName);
                //if (iResult != SUCCESS) return iResult;
                

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
            int iResult = SUCCESS;
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
            int iResult = SUCCESS;

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
                if (ParaInfoList[i].Name.ToLower() == info.Name.ToLower() && ParaInfoList[i].Group.ToLower() == info.Group.ToLower())
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
            name = name.ToLower();
            group = group.ToLower();
            if(ParaInfoList.Count > 0)
            {
                foreach(CParaInfo item in ParaInfoList)
                {
                    if(item.Group.ToLower() == group && item.Name.ToLower() == name)
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
            int iResult = SUCCESS;

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
            if (string.IsNullOrWhiteSpace(table)) return SUCCESS;
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
            int iResult = SUCCESS;
            try
            {
                /* ###
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
            */
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
            /* ###
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
            
        */

            return 0;
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
