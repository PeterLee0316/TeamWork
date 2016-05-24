﻿
// define구문은 실제로는 프로젝트 속성에서 정의해야 함
/*
#define SIMULATION_MOTION
#define SIMULATION_IO
#define SIMULATION_VISION
#define SIMULATION_SECGEM

#define WRITE_LOG_ON_FILE

#define DEF_LINE_1
//#define DEF_LINE_2

#if DEF_LINE_1
#define DEF_NORMAL_LINE
#endif

#if DEF_LINE_2
#define DEF_MIRROR_LINE
#endif
*/

#define SIMULATION_VISION

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace LWDicer.Control
{
    public class DEF_System
    {

        // SYSTEM_VER 및 개인의 작업 내용에 대한 History 관리는 History.txt 에 기록합니다.
        public const string SYSTEM_VER = "Ver 0.0.3";

        // Motion
        public enum EYMC_Board
        {
            BOARD1,
            //BOARD2,
            //BOARD3,
            //BOARD4,
            MAX,
        }
        public enum EYMC_Axis
        {
            NULL = -1        ,
            LOADER_Z = 0     ,
            PUSHPULL_Y       ,
            PUSHPULL_X1,
            S1_CHUCK_ROTATE_T,        // Spinner & Coater 1
            S1_CLEAN_NOZZLE_T,
            S1_COAT_NOZZLE_T,
            PUSHPULL_X2,
            S2_CHUCK_ROTATE_T,        // Spinner & Coater 2
            S2_CLEAN_NOZZLE_T,
            S2_COAT_NOZZLE_T,
            UHANDLER_X,
            UHANDLER_Z       ,
            LHANDLER_X       ,
            LHANDLER_Z       ,
            CAMERA1_Z        ,
            LASER1_Z         ,
            // Stage는 ACS 모션으로 설정할 것 같아서 일단 주석 처리
            //STAGE1_X = 0,
            //STAGE1_Y,
            //STAGE1_T,
            MAX,
        }

        public enum EYMC_Device
        {
            NULL = -1,
            // 개별 축 제어를 위한 device, Device[index] = Axis[index]를 위해서 NULL 축까지도 선언함 
            LOADER_Z = 0,
            PUSHPULL_Y,
            PUSHPULL_X1,
            S1_CHUCK_ROTATE_T,        // Spinner & Coater 1
            S1_CLEAN_NOZZLE_T,
            S1_COAT_NOZZLE_T,
            PUSHPULL_X2,
            S2_CHUCK_ROTATE_T,        // Spinner & Coater 2
            S2_CLEAN_NOZZLE_T,
            S2_COAT_NOZZLE_T,
            UHANDLER_X,
            UHANDLER_Z,
            LHANDLER_X,
            LHANDLER_Z,
            CAMERA1_Z,
            LASER1_Z,
            // Stage는 ACS 모션으로 사용하기로 해서 우선 주석 처리
            //STAGE1_X = 0,
            //STAGE1_Y,
            //STAGE1_T,

            // 그룹으로 축 제어를 위해 선언, MultiAxes
            ALL,    // for control all axis at a time
            LOADER,
            PUSHPULL,
            CENTERING1,     
            S1_ROTATE,      // Spinner & Coater 1
            S1_CLEAN_NOZZLE,
            S1_COAT_NOZZLE,
            CENTERING2,     
            S2_ROTATE,      // Spinner & Coater 2
            S2_CLEAN_NOZZLE,
            S2_COAT_NOZZLE,
            UHANDLER,
            LHANDLER,
            CAMERA1,
            LASER1,
            //STAGE1,
            MAX,
        }

        // Head 정의
        public const int DEF_SHEAD1 = 0;
        public const int DEF_SHEAD2 = 1;
        public const int DEF_GHEAD1 = 2;
        public const int DEF_GHEAD2 = 3;
        public const int DEF_MAX_HEAD_NO = 4;

        // 도포 Tank 정의
        public const int DEF_TANK_1 = 0;
        public const int DEF_TANK_2 = 1;
        public const int DEF_MAX_TANK_NO = 2;

        // Command에 쓰이는 용도, 위의 Head정의와 겹치면 안된다.
        public const int DEF_HEAD_ALL = -1;    // Command to All Head
        public const int DEF_SHEAD_ALL = 5;    // Command to SHead All
        public const int DEF_GHEAD_ALL = 6;    // Command to GHead All

        // UVLAMP 번호 정의
        public const int DEF_UVLAMP_ALL = -1;
        public const int DEF_UVLAMP_1 = 0;
        public const int DEF_UVLAMP_2 = 1;
        public const int DEF_UVLAMP_3 = 2;
        public const int DEF_UVLAMP_4 = 3;
        public const int DEF_MAX_UVLAMP_NO = 4;

        // UVLED 번호 정의
        public const int DEF_UVLED_ALL = -1;
        public const int DEF_UVLED_1 = 0;
        public const int DEF_UVLED_2 = 1;
        public const int DEF_UVLED_3 = 2;
        public const int DEF_UVLED_4 = 3;
        public const int DEF_MAX_UVLED_NO = 4;

        public const int DEF_UVLED_CH_ALL = -1;
        public const int DEF_UVLED_CH1 = 0;
        public const int DEF_UVLED_CH2 = 1;
        public const int DEF_UVLED_CH3 = 2;
        public const int DEF_UVLED_CH4 = 3;
        public const int DEF_UVLED_CH5 = 4;
        public const int DEF_UVLED_CH6 = 5;
        public const int DEF_MAX_UVLED_CH = 6; // 하나의 Head당 달리는 UV 수

        // Value Define 
        public const int DEF_MAX_OBJECT_NO = 256;
        public const int DEF_MAX_OBJECT_INFO_NO = 100;
        public const int DEF_MAX_OBJ_KINDS = 100;
        public const int DEF_MAX_OBJ_NAME_LENGTH = 64;
        public const int DEF_MAX_OBJ_LOG_PATH_NAME_LENGTH = 256;

        // Offset Define 
        public const int DEF_SYSTEMINFO_CYLINDER_OFFSET = 20;
        public const int DEF_SYSTEMINFO_VACUUM_OFFSET = 40;

        // Value Define 
        public const int DEF_MAX_SYSTEM_CYLINDER_OBJ_NO = 22;
        public const int DEF_MAX_SYSTEM_VACUUM_OBJ_NO = 20;
        public const int DEF_MAX_SYSTEM_INDUCTIONMTR_OBJ_NO = 20;
        public const int DEF_MAX_SYSTEM_CAMERA_NO = 2;
        public const int DEF_MAX_SYSTEM_AXIS_NO = 24;
        public const int DEF_MAX_SYSTEM_SIMPLEAXIS_OBJ_NO = 10;
        public const int DEF_MAX_SYSTEM_MULTIAXES_OBJ_NO = 10;
        public const int DEF_MAX_SYSTEM_LOG_ITEM = 200;


        public enum EObjectCylinder
        {
            PUSHPULL_GRIPPER,
            SPINNER1_UD,
            SPINNER1_DI,
            SPINNER1_PVA,
            SPINNER2_UD,
            SPINNER2_DI,
            SPINNER2_PVA,
            PUSHPULL_UD,
            STAGE_CLAMP1,
            STAGE_CLAMP2,
            MAX,
        }

        public enum EObjectVacuum
        {
            STAGE1,
            UHANDLER_SELF,  // Upper Handler
            UHANDLER_FACTORY,
            LHANDLER_SELF,  // Lower Handler
            LHANDLER_FACTORY,
            SPINNER1,
            SPINNER2,
            MAX,
        }

        public enum EObjectScanner
        {
            SCANNER1,
            SCANNER2,
            SCANNER3,
            SCANNER4,
            MAX,
        }

        public enum EObjectLayer
        {
            // Common Class
            OBJ_NONE = -1,
            OBJ_SYSTEM = 0,
            OBJ_DATAMANAGER,

            // Hardware Layer
            OBJ_HL_IO = 10,
            OBJ_HL_CYLINDER,
            OBJ_HL_VACUUM,
            OBJ_HL_INDUCTION_MOTOR,
            OBJ_HL_ACTUATOR,
            OBJ_HL_SERIAL,
            OBJ_HL_ETHERNET,
            OBJ_HL_MELSEC,
            OBJ_HL_RFID,
            OBJ_HL_BARCODE,
            OBJ_HL_MULTIAXES,
            OBJ_HL_MULTIAXES_YMC,
            OBJ_HL_MULTIAXES_ACS,
            OBJ_HL_MOTION_LIB,
            OBJ_HL_VISION,

            // Mechanical Layer
            OBJ_ML_OP_PANEL = 40,
            OBJ_ML_LIGHTENING,
            OBJ_ML_ONLINE,
            OBJ_ML_BARCODE,
            OBJ_ML_RFID,
            OBJ_ML_STAGE,
            OBJ_ML_HANDLER,
            OBJ_ML_WORKBENCH,
            OBJ_ML_DISPENSER,
            OBJ_ML_UVLAMP,
            OBJ_ML_POLYGON,
            OBJ_ML_VISION,
            OBJ_ML_PUSHPULL,
            OBJ_ML_ELEVATOR,
            OBJ_ML_SPINNER,

            // Control Layer - Common
            OBJ_CL_LOADER = 70,
            OBJ_CL_PUSHPULL,
            OBJ_CL_STAGE1,
            OBJ_CL_ALIGN_PANEL,
            //OBJ_CL_WORKBENCH,
            //OBJ_CL_DISPENSE,
            //OBJ_CL_STAGE2,
            //OBJ_CL_STAGE3,
            OBJ_CL_HANDLER,
            OBJ_CL_OP_PANEL,
            OBJ_CL_HW_TEACH,
            OBJ_CL_VISION_CALIBRATION,
            OBJ_CL_MANAGE_PRODUCT,
            OBJ_CL_INTERFACE_CTRL,
            OBJ_CL_SPINNER,

            // Process Layer
            OBJ_PL_TRS_AUTO_MANAGER = 100,
            OBJ_PL_TRS_LOADER,
            OBJ_PL_TRS_PUSHPULL,
            OBJ_PL_TRS_STAGE1,
            OBJ_PL_TRS_SPINNER,
            OBJ_PL_TRS_HANDLER,
            OBJ_PL_TRS_MANAGE_PRODUCT,
            OBJ_PL_TRS_JOG,
            OBJ_PL_TRS_LCNET,
        }

        public enum ELCNetUnitPos
        {
            LCNET_STAGE1 = 0,
            LCNET_WORKBENCH,
            LCNET_STAGE2,
            LCNET_STAGE3,
            LCNET_UHANDLER,
            LCNET_MAX_UNIT, // MAX UNIT
        }

        public enum EPositionObject // 좌표셋을 저장할 수 있는 단위
        {
            ALL = -1,
            LOADER,
            PUSHPULL,
            PUSHPULL_CENTER1,
            S1_ROTATE,
            S1_CLEAN_NOZZLE,
            S1_COAT_NOZZLE,
            PUSHPULL_CENTER2,     
            S2_ROTATE,
            S2_CLEAN_NOZZLE,
            S2_COAT_NOZZLE,
            UHANDLER,
            LHANDLER,
            CAMERA1,
            LASER1,
            STAGE1,
            MAX,
        }

    }

    public class DEF_Common
    {
        //
        public const int WhileSleepTime         = 10; // while interval time

        //
        public const int TRUE                   = 1;
        public const int FALSE                  = 0;

        //
        public const int BIT_ON                 = 1;
        public const int BIT_OFF                = 0;

        //
        public const bool NOT_USE               = false;
        public const bool SET_USE               = true;

        // Jog
        public const bool JOG_DIR_POS = true;
        public const bool JOG_DIR_NEG = false;

        public const int JOG_KEY_X = 0;
        public const int JOG_KEY_Y = 1;
        public const int JOG_KEY_T = 2;
        public const int JOG_KEY_Z = 3;

        public const int JOG_KEY_NON = 0;
        public const int JOG_KEY_POS = 1;
        public const int JOG_KEY_NEG = 2;
        public const int JOG_KEY_ALL = 3;

        // Language
        public enum ELanguage
        {
            KOREAN,
            ENGLISH,
            CHINESE,
            JAPANESE,
            MAX,
        }

        // Login
        public enum ELoginType
        {
            OPERATOR = 0,
            ENGINEER,
            MAKER,
        }

        public class USERTYPE
        {
            public static readonly String OPERATOR = "OPERATOR";
            public static readonly String ENGINEER = "ENGINEER";
            public static readonly String MAKER    = "MAKER";
        }

        public class CLoginData
        {
            public string Number      = "Default";              // unique primary key
            public string Name        = "SystemStart Default";
            public ELoginType Type    = ELoginType.OPERATOR;    // 필수
            public DateTime LoginTime = DateTime.Now;

            public override string ToString()
            {
                return $"{Type}, {Number}, {Name}";
            }
        }

        //
        public const int ERR_MLOG_FILE_OPEN_ERROR        = 1;
        public const int ERR_MLOG_TOO_SHORT_KEEPING_DAYS = 2;

        public const byte DEF_MLOG_NONE_LOG_LEVEL        = 0x00;    // Log 안 함
        public const byte DEF_MLOG_ERROR_LOG_LEVEL       = 0x01;    // Error관련 Log
        public const byte DEF_MLOG_WARNING_LOG_LEVEL     = 0x02;    // Warning 관련 Log
        public const byte DEF_MLOG_NORMAL_LOG_LEVEL      = 0x04;    // 정상 동작 관련 Log
        public const byte DEF_MLOG_TACT_TIME_LOG_LEVEL   = 0x10;
        public const int LOG_ALL = DEF_MLOG_NONE_LOG_LEVEL | DEF_MLOG_ERROR_LOG_LEVEL | DEF_MLOG_WARNING_LOG_LEVEL | DEF_MLOG_NORMAL_LOG_LEVEL;
        //public const int LOG_ALL = DEF_MLOG_ERROR_LOG_LEVEL;

        public const int DEF_MLOG_DEFAULT_KEEPING_DAYS   = 30;
        public const int DEF_MLOG_NUM_FILES_TOBE_DELETED = 20;
        public const int DEF_MLOG_NUM_VIEW_DISPLAY_LOG   = 100;

        public const int LOG_DAY = DEF_MLOG_DEFAULT_KEEPING_DAYS;
        public const int DEF_SYSTEMINFO_MIN_LOG_KEEPING_DAYS = 5;
        public const int DEF_SYSTEMINFO_MAX_LOG_KEEPING_DAYS = 90;

        public enum ELogType
        {
            // Debug, Tact는 별도의 DB에 저장하고
            Debug = 0,
            Tact,

            // 이하의 DB는 ELog에 저장하는 형태로
            LOGIN,
            SECGEM,
            SYSTEM,
            RUN,
            //INI,
            //PON,
            //DMW,
        }

        public enum ELogWType
        {
            // 기본 Debug에서 쓰이는 3ea type
            // 소문자 Error는 Debug.Error이고, 대문자 ERROR는 자동운전중의 ERROR로 일단 구분해놓음
            Normal,
            Warning,
            Error,

            // 이하, 각 LogType에서 쓰이는 상세 type
            LOGIN,
            LOGOUT,
            SAVE,
            LOAD,
            FAIL,
            ERROR,
            START,
            COMPLETE,
        }

        public class CLogItem
        {
            int m_iMyOrder;
            string m_strDate;
            int m_iObjectID;
            byte m_ucLevel;
            string m_strLogMsg;
            string m_strFileName;
            int m_strLineNumber;
        }

        public class CTactLogItem
        {
            int m_iMyOrder;
            string m_strDate;
            int m_iObjectID;
            byte m_ucLevel;
            string m_strTactStep;
            double m_dTactTime;
        }

        //public class CLogViewItem
        //{
        //    int iNumLog;
        //    int iCurrentIndex;
        //    SLogItem logItemView[DEF_MLOG_NUM_VIEW_DISPLAY_LOG];
        //}

        //public class CTactLogViewItem
        //{
        //    int iNumLog;
        //    int iCurrentIndex;
        //    STactLogItem logItemView[DEF_MLOG_NUM_VIEW_DISPLAY_LOG];
        //}

        public class CDBInfo
        {
            /////////////////////////////////////////////////////////////////////
            // Database
            private string DBDir             ; // DB Directory
            private string DBName            ; // Main System and Model Database
            private string DBName_Backup     ; // backup for main db
            private string DBName_Info       ; // Information DB

            private string DBDir_Log         ; // Log Directory
            private string DBName_DLog       ; // 개발자용 Log를 남기는 DB를 따로 만들어 둠.
            private string DBName_ELog       ; // Alarm, Login, Event 등의 History를 관리하는 DB

            /////////////////////////////////////////////////////////////////////
            // Database Connection
            public string DBConn            ; // DB Connection string
            public string DBConn_Backup     ; // DB Connection string for backup
            public string DBConn_Info       ; // DB Connection string for Information
            public string DBConn_DLog       ; // DB Connection string for DLog
            public string DBConn_ELog       ; // DB Connection string for ELog

            /////////////////////////////////////////////////////////////////////
            // Table
            public string TableSystem       { get; private set; } // System Data
            public string TableModel        { get; private set; } // Model Data
            public string TableModelHeader  { get; private set; } // Model and Parent directory Header
            public string TablePos          { get; private set; } // Position Data
            public string TableIO           { get; private set; } // IO Information
            public string TableAlarmInfo    { get; private set; } // Alarm Information
            public string TableMsgInfo      { get; private set; } // Message Information
            public string TableParameter    { get; private set; } // Parameter Description

            public string TableLoginHistory { get; private set; } // Login History
            public string TableAlarmHistory { get; private set; } // Alarm History
            public string TableDebugLog     { get; private set; } // 개발자용 Log
            public string TableEventLog     { get; private set; } // Event History

            /////////////////////////////////////////////////////////////////////
            // Common Directory
            public string SystemDir         { get; private set; } // System Data가 저장되는 디렉토리
            public string ModelDir          { get; private set; } // Model Data가 저장되는 디렉토리 
            public string ScannerLogDir     { get; private set; } // Poligon Scanner와의 전송에 필요한 image, ini file 저장용
            public string ImageLogDir       { get; private set; } // Vision에서 모델에 관계없이 image file 저장할 필요가 있을때 사용

            /////////////////////////////////////////////////////////////////////
            // Excel
            public string ExcelIOList       { get; private set; } // Excel File IO List
            public string ExcelSystemData   { get; private set; } // Excel System Data List

            public CDBInfo()
            {
                // System and Model DB
                DBDir             = ConfigurationManager.AppSettings["AppFilePath"]/* + @"\Data\"*/;
                DBName            = "LWD_Data_v01.db3";
                DBName_Backup     = "LWD_DataB_v01.db3";
                DBConn            = $"Data Source={DBDir}{DBName}";
                DBConn_Backup     = $"Data Source={DBDir}{DBName_Backup}";
                DBName_Info       = "LWD_Info_v01.db3";
                DBConn_Info       = $"Data Source={DBDir}{DBName_Info}";

                TableSystem       = "SystemDB";
                TableModel        = "ModelDB";
                TableModelHeader  = "ModelHeader";
                TablePos          = "PositionDB";

                TableIO           = "IO";
                TableAlarmInfo    = "AlarmInfo";
                TableMsgInfo      = "MessageInfo";
                TableParameter    = "Parameter";

                // Developer's and Event Log DB
                DBDir_Log         = ConfigurationManager.AppSettings["AppFilePath"]/* + @"\Log\"*/;
                DBName_DLog       = "LWD_DLog_v01.db3";
                DBConn_DLog       = $"Data Source={DBDir_Log}{DBName_DLog}";
                DBName_ELog       = "LWD_ELog_v01.db3";
                DBConn_ELog       = $"Data Source={DBDir_Log}{DBName_ELog}";

                TableLoginHistory = "LoginHistory";
                TableAlarmHistory = "AlarmHistory";
                TableDebugLog     = "DLog";
                TableEventLog     = "ELog";

                ExcelIOList       = "LWDicer_IO_List.xlsx";
                ExcelSystemData   = "SystemData.xlsx";

                // Model Dir
                SystemDir = ConfigurationManager.AppSettings["AppFilePath"] + @"\SystemData\";
                ModelDir = ConfigurationManager.AppSettings["AppFilePath"] + @"\ModelData\";
                ScannerLogDir = ConfigurationManager.AppSettings["AppFilePath"] + @"\ScannerLog\";
                ImageLogDir = ConfigurationManager.AppSettings["AppFilePath"] + @"\ImageLog\";

                System.IO.Directory.CreateDirectory(DBDir);
                System.IO.Directory.CreateDirectory(DBDir_Log);
                System.IO.Directory.CreateDirectory(SystemDir);
                System.IO.Directory.CreateDirectory(ModelDir);
                System.IO.Directory.CreateDirectory(ScannerLogDir);
                System.IO.Directory.CreateDirectory(ImageLogDir);
            }
        }

        public class CObjectInfo
        {
            public int Type;
            public string TypeName;
            public int ID;
            public string Name;
            public int ErrorBase;
            
            // Log
            public string DebugTableName;
            public byte LogLevel;
            public int LogDays;

            static public CDBInfo DBInfo;

            public CObjectInfo()
            {
            }

            public CObjectInfo(int Type, string TypeName, int ID, string Name,
                int ErrorBase, string DebugTableName, byte LogLevel, int LogDays)
            {
                this.Type        = Type;
                this.TypeName    = TypeName;
                this.ID          = ID;
                this.Name        = Name;
                this.ErrorBase   = ErrorBase;
                this.DebugTableName = DebugTableName;
                this.LogLevel    = LogLevel;
                this.LogDays     = LogDays;
            }
        }

        public enum EUnitType
        {
            // Text
            text,

            // bool
            boolean,

            // Length
            km,
            m,
            cm,
            mm,
            um,
            nm,
            inch,

            // square
            m2,

            // Weight
            g,
            kg,
            lb, // pound

            // Speed
            m_sec,
            km_hour,
            inch_sec,
            rpm,
            rad_sec,    // radius / sec

            // Time
            year,
            month,
            week,
            day,
            hour,
            minute,
            sec,

            // Hz
            MHz,
            KHz,
            Hz,

            // Newton
            N,

        }

        /// <summary>
        /// System, Model, Laser, Scanner 등의 class에서 쓰이는 Parameter에 대한 정보를 관리하는 class
        /// Parameter Name 중복 관리를 피하기 위해서 Group, Name 각각의 필드를 이용해서 관리
        /// </summary>
        public class CParaInfo
        {
            public string Group;    // ex) System, Model, Scanner, Laser, etc
            public string Name;     // ex) Password, ModelName, InScanResolution, etc
            public string Unit;     // Unit ex) km, km/s, m/s^2 
            public EUnitType Type;  // Unit Type을 지정해서 자동으로 텍스트로 환산 및 계산하려고 하지만, 너무 많아서 일단 자리만 잡아놓고 not use

            public string[] DisplayName = new string[(int)DEF_Common.ELanguage.MAX];
            public string[] Description = new string[(int)DEF_Common.ELanguage.MAX];

            public CParaInfo(string Group = "group", string Name = "parameter", string Unit = "[-]", EUnitType Type = EUnitType.mm)
            {
                this.Group = Group;
                this.Name = Name;
                this.Unit = Unit;
                this.Type = Type; // temporarily 

                DisplayName[(int)DEF_Common.ELanguage.KOREAN]   = Name;
                DisplayName[(int)DEF_Common.ELanguage.ENGLISH]  = Name;
                DisplayName[(int)DEF_Common.ELanguage.CHINESE]  = Name;
                DisplayName[(int)DEF_Common.ELanguage.JAPANESE] = Name;

                Description[(int)DEF_Common.ELanguage.KOREAN]   = "Parameter Description";
                Description[(int)DEF_Common.ELanguage.ENGLISH]  = "Parameter Description";
                Description[(int)DEF_Common.ELanguage.CHINESE]  = "Parameter Description";
                Description[(int)DEF_Common.ELanguage.JAPANESE] = "Parameter Description";
            }
        }

        public enum EVelocityMode
        {
            NORMAL,
            FAST,
            SLOW,
        }
    }

    public class DEF_LCNet
    {
        public enum ELCEqStates
        {
            eEqNothing = 0,
            eNormal = 1,
            eFault = 2,
            ePM = 3
        };

        public enum ELCEqProcStates
        {
            eEqPNothing = 0,
            eInit = 1,
            eIdle = 2,
            eSetup = 3,
            eReady = 4,
            eExecute = 5,
            ePause = 6
        };
    }

    public class DEF_Thread
    {
        /// <summary>
        /// define auto / manual mode
        /// </summary>
        public enum EAutoManual
        {
            AUTO,     // 자동 동작 모드
            MANUAL,   // 수동 동작 모드
        }

        /// <summary>
        /// define each mode when auto run
        /// </summary>
        public enum ERunMode
        {
            NORMAL_RUN,    // 정상 운전
            PASS_RUN,      // 통과 운전
            DRY_RUN,       // 물류 흐름 없는 운전
            REPAIR_RUN,    // 수리후 운전 for Rework Panel
        }

        /// <summary>
        /// Thread Status, RunStatus
        /// </summary>
        public enum ERunStatus
        {
            NONE = -1,
            STS_MANUAL       = 0,    // System 수동 동작 상태
            STS_RUN_READY       ,    // View Start 버튼이 눌러졌음
            STS_RUN             ,    // System RUN 상태
            STS_STEP_STOP       ,    // STEP_STOP을 진행중임
            STS_ERROR_STOP      ,    // ERROR_STOP을 진행중미
            STS_CYCLE_STOP      ,    // CYCLE_STOP을 진행중임
            STS_OP_CALL         ,    // Operator CALL
            STS_EXC_MATERIAL    ,    // Exchange Material
        }

        /// <summary>
        /// Thread ID, List
        /// </summary>
        public enum EThreadChannel
        {
            TrsSelfChannel       = 0,
            TrsAutoManager       ,
            TrsLoader            ,
            TrsPushPull          ,
            TrsCleaner1          ,
            TrsCleaner2          ,
            TrsHandler           ,
            TrsStage1            ,
            TrsStage2            ,
        }
        public const int MAX_THREAD_CHANNEL  = 15;

        // Thread Run
        public const int ThreadSleepTime     = 10;
        public const int ThreadSuspendedTime = 100;

        /// <summary>
        /// initialize thread unit index
        /// </summary>
        public enum EInitiableUnit
        {
            LOADER,
            PUSHPULL,
            CLEANER1,
            CLEANER2,
            HANDLER,
            STAGE1,
            MAX,
        }

        // Common Thread Message inter Threads
        public enum EThreadMessage
        {
            // _CNF command는 _CMD command에 대한 response 임
            MSG_MANUAL_CMD = 10,                 // 수동 모드로의 전환
            MSG_MANUAL_CNF,                      // 
            MSG_START_RUN_CMD,                   // 화면에서 시작 버튼을 누름
            MSG_START_RUN_CNF,                   // 
            MSG_START_CMD,                       // OP Panel에서 시작 버튼을 누름
            MSG_START_CNF,                       //
            MSG_ERROR_STOP_CMD,                  // Error Stop 스위치를 누름
            MSG_ERROR_STOP_CNF,
            MSG_STEP_STOP_CMD,                   // Step Stop 스위치를 누름
            MSG_STEP_STOP_CNF,
            MSG_CYCLE_STOP_CMD,                  // Cycle Stop 스위치를 누름
            MSG_CYCLE_STOP_CNF,
            MSG_PANEL_SUPPLY_START,              // 화면에서 Panel Supply Stop 버튼을 누름
            MSG_PANEL_SUPPLY_STOP,
            MSG_START_CASSETTE_SUPPLY,
            MSG_STOP_CASSETTE_SUPPLY,
            MSG_START_WAFER_SUPPLY,
            MSG_STOP_WAFER_SUPPLY,
            MSG_ERROR_DISPLAY_REQ,
            MSG_ERROR_DISPLAY_END,
            MSG_OP_CALL_REQ,
            MSG_OP_CALL_END,
            MSG_LC_PAUSE,
            MSG_LC_RESUME,
            MSG_LC_PM,
            MSG_LC_NORMAL,
            MSG_STAGE_LOADING_START,             // AK 측과의 물류시 안전신호 체크용으로 쓰임
            MSG_STAGE_LOADING_END,
            MSG_PANEL_INPUT,            // BUFFER(S), WORKBENCH(G)측의 PANEL 투입시점 Check
            MSG_PANEL_OUTPUT,			// WORKBENCH(S), UNLOADHANDLER(G)측의 PANEL 아웃지점 Check
            MSG_UNLOADHANDLER_UNLOADING_START,   // PCB 측과의 물류시 안전신호 체크용으로 쓰임
            MSG_UNLOADHANDLER_UNLOADING_END,
            MSG_CONFIRM_ENG_DOWN,

            MSG_AUTO_LOADER_REQUEST_LOAD_CASSETTE,
            MSG_AUTO_LOADER_REQUEST_UNLOAD_CASSETTE,

            MSG_PROCESS_ALARM = 99,	                 // Process Alarm 메세지

            // TrsLoader Message
            MSG_LOADER_PUSHPULL_READY_UNLOADING = 100,
            MSG_LOADER_PUSHPULL_READY_LOADING,
            MSG_LOADER_PUSHPULL_ALL_WAFER_WORKED,
            MSG_LOADER_PUSHPULL_STACKS_FULL,


            // TrsPushPull Message
            MSG_PUSHPULL_LOADER_REQUEST_UNLOADING = 200,    // wafer : L -> P
            MSG_PUSHPULL_LOADER_START_LOADING,              // wafer : L -> P
            MSG_PUSHPULL_LOADER_COMPLETE_LOADING,           // wafer : L -> P
            MSG_PUSHPULL_LOADER_REQUEST_LOADING,            // wafer : P -> L
            MSG_PUSHPULL_LOADER_START_UNLOADING,            // wafer : P -> L
            MSG_PUSHPULL_LOADER_COMPLETE_UNLOADING,         // wafer : P -> L

            MSG_PUSHPULL_CLEANER1_REQUEST_LOADING,           // wafer : P -> C
            MSG_PUSHPULL_CLEANER1_START_UNLOADING,           // wafer : P -> C
            MSG_PUSHPULL_CLEANER1_COMPLETE_UNLOADING,        // wafer : P -> C
            MSG_PUSHPULL_CLEANER1_READY_LOADING,             // wafer : C -> P
            MSG_PUSHPULL_CLEANER1_START_LOADING,             // wafer : C -> P
            MSG_PUSHPULL_CLEANER1_COMPLETE_LOADING,          // wafer : C -> P

            MSG_PUSHPULL_CLEANER2_REQUEST_LOADING,           // wafer : P -> C
            MSG_PUSHPULL_CLEANER2_START_UNLOADING,           // wafer : P -> C
            MSG_PUSHPULL_CLEANER2_COMPLETE_UNLOADING,        // wafer : P -> C
            MSG_PUSHPULL_CLEANER2_READY_LOADING,             // wafer : C -> P
            MSG_PUSHPULL_CLEANER2_START_LOADING,             // wafer : C -> P
            MSG_PUSHPULL_CLEANER2_COMPLETE_LOADING,          // wafer : C -> P

            MSG_PUSHPULL_HANDLER_REQUEST_LOADING,           // wafer : P -> H
            MSG_PUSHPULL_HANDLER_START_UNLOADING,           // wafer : P -> H
            MSG_PUSHPULL_HANDLER_COMPLETE_UNLOADING,        // wafer : P -> H
            MSG_PUSHPULL_HANDLER_READY_LOADING,             // wafer : H -> P
            MSG_PUSHPULL_HANDLER_START_LOADING,             // wafer : H -> P
            MSG_PUSHPULL_HANDLER_COMPLETE_LOADING,          // wafer : H -> P

            // TrsCleaner1 Message
            MSG_CLEANER1_PUSHPULL_READY_LOADING = 300,
            MSG_CLEANER1_PUSHPULL_START_LOADING,
            MSG_CLEANER1_PUSHPULL_COMPLETE_LOADING,
            MSG_CLEANER1_PUSHPULL_READY_UNLOADING,
            MSG_CLEANER1_PUSHPULL_START_UNLOADING,
            MSG_CLEANER1_PUSHPULL_COMPLETE_UNLOADING,

            // TrsCleaner2 Message
            MSG_CLEANER2_PUSHPULL_READY_LOADING = 400,
            MSG_CLEANER2_PUSHPULL_START_LOADING,
            MSG_CLEANER2_PUSHPULL_COMPLETE_LOADING,
            MSG_CLEANER2_PUSHPULL_READY_UNLOADING,
            MSG_CLEANER2_PUSHPULL_START_UNLOADING,
            MSG_CLEANER2_PUSHPULL_COMPLETE_UNLOADING,

            // TrsHandler Message
            MSG_HANDLER_PUSHPULL_READY_LOADING = 500,
            MSG_HANDLER_PUSHPULL_START_LOADING,
            MSG_HANDLER_PUSHPULL_COMPLETE_LOADING,
            MSG_HANDLER_PUSHPULL_READY_UNLOADING,
            MSG_HANDLER_PUSHPULL_START_UNLOADING,
            MSG_HANDLER_PUSHPULL_COMPLETE_UNLOADING,

            MSG_HANDLER_STAGE1_READY_UNLOADING,
            MSG_HANDLER_STAGE1_START_UNLOADING,
            MSG_HANDLER_STAGE1_COMPLETE_UNLOADING,
            MSG_HANDLER_STAGE1_READY_LOADING,
            MSG_HANDLER_STAGE1_START_LOADING,
            MSG_HANDLER_STAGE1_COMPLETE_LOADING,

            // TrsStage1 Message
            MSG_STAGE1_HANDLER_READY_LOADING = 600,
            MSG_STAGE1_HANDLER_START_LOADING,
            MSG_STAGE1_HANDLER_COMPLETE_LOADING,
            MSG_STAGE1_HANDLER_READY_UNLOADING,
            MSG_STAGE1_HANDLER_START_UNLOADING,
            MSG_STAGE1_HANDLER_COMPLETE_UNLOADING,

            MSG_STAGE1_WORKBENCH_UNLOAD_READY,
            MSG_STAGE1_WORKBENCH_UNLOAD_COMPLETE,
            MSG_STAGE1_WORKBENCH_SAFETY_POS,
            MSG_STAGE1_WORKBENCH_UNSAFETY_POS,
            MSG_STAGE1_WORKBENCH_RELOAD_READY,
            MSG_STAGE1_WORKBENCH_RELOAD_COMPLETE,
            MSG_STAGE1_DISPENSER_DISPENSING_READY,
            MSG_STAGE1_STAGE2_START_TACT_TIME,
            MSG_STAGE1_DISPENSER_UVCHECK_REQUEST,

            // TrsWorkbench Message
            MSG_WORKBENCH_STAGE1_LOAD_REQUEST = 800,
            MSG_WORKBENCH_STAGE1_LOAD_READY,
            MSG_WORKBENCH_STAGE1_LOAD_COMPLETE,
            MSG_WORKBENCH_STAGE1_SAFETY_POS,
            MSG_WORKBENCH_STAGE1_UNSAFETY_POS,
            MSG_WORKBENCH_STAGE2_UNLOAD_REQUEST,
            MSG_WORKBENCH_STAGE2_UNLOAD_READY,
            MSG_WORKBENCH_STAGE2_UNLOAD_COMPLETE,
            MSG_WORKBENCH_STAGE2_SAFETY_POS,
            MSG_WORKBENCH_STAGE2_UNSAFETY_POS,
            MSG_WORKBENCH_DISPENSER_DISPENSING_READY,
            MSG_WORKBENCH_DISPENSER_DISPENSING_REQUEST,
            MSG_WORKBENCH_DISPENSER_SAFETY_POS,
            MSG_WORKBENCH_DISPENSER_UNSAFETY_POS,
            MSG_WORKBENCH_STAGE1_REUNLOAD_REQUEST,
            MSG_WORKBENCH_STAGE1_REUNLOAD_READY,
            MSG_WORKBENCH_STAGE1_REUNLOAD_COMPLETE,

        }

        public enum EWindowMessage
        {
            // message from control class to GUI
            WM_SW_STATUS,
            WM_AUTO_STATUS,
            WM_ALARM_MSG,
            WM_STEP_DISPLAY_END,
            WM_HEIGHT_DISPLAY_END,
            WM_DISPLAY_HELPVIEW,
            WM_DISPLAY_HELP_ID,

            WM_MSGBOX_MSG,          // MyMessageBox 띄우기
            WM_ALIGN_MSG,           // Align 인식 실패 시 처리하는 Dialog 띄우기
            WM_DISPLAYUPDATE_MSG,   // Display Update Msg
            WM_START_RUN_MSG,
            WM_START_READY_MSG,
            WM_START_MANUAL_MSG,
            WM_ERRORSTOP_MSG,
            WM_STEPSTOP_MSG,
            WM_CHECK_ENG_DOWN_MSG,

            WM_DISPLAY_STATUSBAR_1,
            WM_DISPLAY_STATUSBAR_2,
            WM_DISPLAY_STATUSBAR_3,
            WM_DISPLAY_STATUSBAR_4,
            WM_DISPLAY_STATUSBAR_5,
            WM_DISPLAY_STATUSBAR_6,

            WM_DISP_PANEL_DISTANCE_MSG,
            WM_DISP_PANEL_DISTANCE_MSG1,
            WM_DISP_PANEL_DISTANCE_MSG2,
            WM_DISP_TACTTIME_MSG,

            // 생산량 증가 MSG
            WM_DISP_PRODUCT_OUT_MSG,
            WM_DISP_PRODUCT_IN_MSG,

            // LCNet
            WM_DISP_EQ_STATE,
            WM_DISP_EQ_PROC_STATE,
            WM_DISP_TERMINAL_MSG,

            // Panel Supply
            WM_CELL_SUPPLY_STOP_MSG,

            WM_DISP_RUN_MODE,
            WM_DISP_EQSTOP_MSG,

            WM_DISP_DISPLAY_UVLED_LIGHT,
            WM_DISP_REPORT_AUTO_UV_CHECK,
        }

        // TrsLoader Step
        public enum ETrsLoaderStep
        {
            //
            TRS_LOADER_WAITFOR_MESSAGE,

            // handle cassette
            TRS_LOADER_READY_LOAD_CASSETTE,
            TRS_LOADER_WAITFOR_CASSETTE_LOADED,
            TRS_LOADER_LOAD_CASSETTE,
            TRS_LOADER_CHECK_STACK_OF_CASSETTE,

            TRS_LOADER_READY_UNLOAD_CASSETTE,
            TRS_LOADER_WAITFOR_CASSETTE_REMOVED,

            // process with pushpull
            TRS_LOADER_READY_UNLOADING_WAFER,
            TRS_LOADER_WAITFOR_PUSHPULL_START_LOADING,
            TRS_LOADER_UNLOAD_WAFER,
            TRS_LOADER_WAITFOR_PUSHPULL_COMPLETE_LOADING,

            TRS_LOADER_READY_LOADING_WAFER,
            TRS_LOADER_WAITFOR_PUSHPULL_START_UNLOADING,
            TRS_LOADER_LOAD_WAFER,
            TRS_LOADER_WAITFOR_PUSHPULL_COMPLETE_UNLOADING,
        }

        // TrsPushPull Step
        public enum ETrsPushPullStep
        {
            TRS_PUSHPULL_MOVETO_WAIT_POS,
            TRS_PUSHPULL_WAITFOR_MESSAGE,

            ///////////////////////////////////////////////////////////////////
            // with loader
            // wafer : loader -> pushpull
            TRS_PUSHPULL_MOVETO_LOAD_LOADER_POS,          // move to load pos from loader
            TRS_PUSHPULL_PRE_LOADING_FROM_LOADER,         // extend guide, send load ready signal to loader
            TRS_PUSHPULL_WAITFOR_LOADER_UNLOAD_READY,     // wait for response from loader
            TRS_PUSHPULL_LOADING_FROM_LOADER,             // withdraw guide, send load complete signal to loader
            TRS_PUSHPULL_WAITFOR_LOADER_UNLOAD_COMPLETE,  // wait for response from loader

            // wafer : pushpull -> loader
            TRS_PUSHPULL_MOVETO_UNLOAD_LOADER_POS,        // move to unload pos to loader
            TRS_PUSHPULL_REQUEST_LOADER_LOADING,          // send load request signal to loader
            TRS_PUSHPULL_WAITFOR_LOADER_LOAD_READY,       // wait for response from loader
            TRS_PUSHPULL_UNLOADING_TO_LOADER,             // extend guide, send unload complete signal to loader
            TRS_PUSHPULL_WAITFOR_LOADER_LOAD_COMPLETE,    // wait for response from loader

            ///////////////////////////////////////////////////////////////////
            // with cleaner1
            // wafer : spinner -> pushpull
            TRS_PUSHPULL_MOVETO_LOAD_CLEANER1_POS,          // move to load pos from cleaner1
            TRS_PUSHPULL_PRE_LOADING_FROM_CLEANER1,         // extend guide, send load ready signal to cleaner1
            TRS_PUSHPULL_WAITFOR_CLEANER1_UNLOAD_READY,     // wait for response from cleaner1
            TRS_PUSHPULL_LOADING_FROM_CLEANER1,             // withdraw guide, send load complete signal to cleaner1
            TRS_PUSHPULL_WAITFOR_CLEANER1_UNLOAD_COMPLETE,  // wait for response from cleaner1

            // wafer : pushpull -> spinner
            TRS_PUSHPULL_MOVETO_UNLOAD_CLEANER1_POS,        // move to unload pos to cleaner1
            TRS_PUSHPULL_REQUEST_CLEANER1_LOADING,          // send load request signal to cleaner1
            TRS_PUSHPULL_WAITFOR_CLEANER1_LOAD_READY,       // wait for response from cleaner1
            TRS_PUSHPULL_UNLOADING_TO_CLEANER1,             // extend guide, send unload complete signal to cleaner1
            TRS_PUSHPULL_WAITFOR_CLEANER1_LOAD_COMPLETE,    // wait for response from cleaner1

            ///////////////////////////////////////////////////////////////////
            // with cleaner2
            // wafer : spinner -> pushpull
            TRS_PUSHPULL_MOVETO_LOAD_CLEANER2_POS,          // move to load pos from cleaner2
            TRS_PUSHPULL_PRE_LOADING_FROM_CLEANER2,         // extend guide, send load ready signal to cleaner2
            TRS_PUSHPULL_WAITFOR_CLEANER2_UNLOAD_READY,     // wait for response from cleaner2
            TRS_PUSHPULL_LOADING_FROM_CLEANER2,             // withdraw guide, send load complete signal to cleaner2
            TRS_PUSHPULL_WAITFOR_CLEANER2_UNLOAD_COMPLETE,  // wait for response from cleaner2

            // wafer : pushpull -> spinner
            TRS_PUSHPULL_MOVETO_UNLOAD_CLEANER2_POS,        // move to unload pos to cleaner2
            TRS_PUSHPULL_REQUEST_CLEANER2_LOADING,          // send load request signal to cleaner2
            TRS_PUSHPULL_WAITFOR_CLEANER2_LOAD_READY,       // wait for response from cleaner2
            TRS_PUSHPULL_UNLOADING_TO_CLEANER2,             // extend guide, send unload complete signal to cleaner2
            TRS_PUSHPULL_WAITFOR_CLEANER2_LOAD_COMPLETE,    // wait for response from cleaner2

            ///////////////////////////////////////////////////////////////////
            // with handler
            // wafer : handler -> pushpull
            TRS_PUSHPULL_MOVETO_LOAD_HANDLER_POS,          // move to load pos from handler
            TRS_PUSHPULL_PRE_LOADING_FROM_HANDLER,         // extend guide, send load ready signal to handler
            TRS_PUSHPULL_WAITFOR_HANDLER_UNLOAD_READY,     // wait for response from handler
            TRS_PUSHPULL_LOADING_FROM_HANDLER,             // withdraw guide, send load complete signal to handler
            TRS_PUSHPULL_WAITFOR_HANDLER_UNLOAD_COMPLETE,  // wait for response from handler

            // wafer : pushpull -> handler
            TRS_PUSHPULL_MOVETO_UNLOAD_HANDLER_POS,        // move to unload pos to handler
            TRS_PUSHPULL_REQUEST_HANDLER_LOADING,          // send load request signal to handler
            TRS_PUSHPULL_WAITFOR_HANDLER_LOAD_READY,       // wait for response from handler
            TRS_PUSHPULL_UNLOADING_TO_HANDLER,             // extend guide, send unload complete signal to handler
            TRS_PUSHPULL_WAITFOR_HANDLER_LOAD_COMPLETE,    // wait for response from handler
        }

        public enum ETrsHandlerStep
        {
            // Upper/Load Handler
            TRS_HANDLER_LHANDLER_MOVETO_WAIT1,
            TRS_HANDLER_LHANDLER_WAIT_MOVETO_LOAD,      // wait for load request signal from pushpull
            TRS_HANDLER_LHANDLER_MOVETO_LOAD_POS,
            TRS_HANDLER_LHANDLER_LOADING,
            TRS_HANDLER_LHANDLER_WAITFOR_PUSHPULL_UNLOAD_READY,
            TRS_HANDLER_LHANDLER_MOVETO_LOAD_UP_POS,    // after move up, send load complete signal to pushpull
            TRS_HANDLER_LHANDLER_MOVETO_WAIT2,
            TRS_HANDLER_LHANDLER_WAIT_MOVETO_UNLOAD,    // wait for unload request signal from stage
            TRS_HANDLER_LHANDLER_MOVETO_UNLOAD_POS,
            TRS_HANDLER_LHANDLER_REQUEST_STAGE_LOADING, // request stage to vacuum absorb
            TRS_HANDLER_LHANDLER_UNLOADING,             // after vacuum release + move up, send unload complete signal to stage

            // Lower/Unload Handler
            TRS_HANDLER_UHANDLER_MOVETO_WAIT1,
            TRS_HANDLER_UHANDLER_WAIT_MOVETO_LOAD,      // wait for load request signal from stage
            TRS_HANDLER_UHANDLER_MOVETO_LOAD_POS,
            TRS_HANDLER_UHANDLER_LOADING,
            TRS_HANDLER_UHANDLER_WAITFOR_STAGE_UNLOAD_READY,
            TRS_HANDLER_UHANDLER_MOVETO_LOAD_UP_POS,    // after move up, send load complete signal to stage
            TRS_HANDLER_UHANDLER_MOVETO_WAIT2,
            TRS_HANDLER_UHANDLER_WAIT_MOVETO_UNLOAD,    // wait for unload request signal from pushpull
            TRS_HANDLER_UHANDLER_MOVETO_UNLOAD_POS,
            TRS_HANDLER_UHANDLER_REQUEST_PUSHPULL_LOADING, // request pushpull to vacuum absorb
            TRS_HANDLER_UHANDLER_UNLOADING,             // after vacuum release + move up, send unload complete signal to pushpull
        }

        public enum ETrsSpinnerStep
        {
            TRS_SPINNER_MOVETO_WAIT_POS,
            TRS_SPINNER_WAITFOR_PUSHPULL_LOAD_REQUEST,      // request pushpull to unload wafer
            TRS_SPINNER_MOVETO_LOAD_POS,                    //
            TRS_SPINNER_LOADING,                            // after vacuum absorb, send load ready signal to pushpull
            TRS_SPINNER_WAITFOR_PUSHPULL_UNLOAD_READY,      // wait for response from pushpull
            TRS_SPINNER_MOVETO_WORK_POS,                    // move to work pos, send load complete signal to pushpull
            TRS_SPINNER_DO_CLEAN,                           // do work if it is needed.
            TRS_SPINNER_DO_AFTER_CLEAN,
            TRS_SPINNER_DO_COAT,                            // do work if it is needed.
            TRS_SPINNER_DO_AFTER_COAT,
            TRS_SPINNER_REQUEST_PUSHPULL_LOADING,           // request pushpull to load wafer
            TRS_SPINNER_WAITFOR_PUSHPULL_UNLOAD_REQUEST,    // wait for response from pushpull
            TRS_SPINNER_MOVETO_UNLOAD_POS,                  // 
            TRS_SPINNER_WAITFOR_PUSHPULL_LOAD_READY,        // wait for response from pushpull
            TRS_SPINNER_UNLOADING,                          // after vacuum release + move to wait, send unload complete signal to pushpull
        }

        // TrsStage1 Step
        public enum ETrsStage1Step
        {
            TRS_STAGE1_MOVETO_WAIT_POS,
            TRS_STAGE1_MOVETO_LOAD_POS,
            TRS_STAGE1_REQUEST_HANDLER_UNLOADING,
            TRS_STAGE1_WAITFOR_HANDLER_UNLOAD_READY,
            TRS_STAGE1_LOADING,
            TRS_STAGE1_WAITFOR_HANDLER_UNLOAD_COMPLETE,
            TRS_STAGE1_MOVETO_ALIGN_POS,
            TRS_STAGE1_DO_ALIGN,
            TRS_STAGE1_MOVETO_DICING_POS,
            TRS_STAGE1_DO_DICING,
            TRS_STAGE1_MOVETO_UNLOAD_POS,
            TRS_STAGE1_REQUEST_HANDLER_LOADING,
            TRS_STAGE1_WAITFOR_HANDLER_LOAD_READY,
            TRS_STAGE1_UNLOADING,
        };
    }

    public class DEF_Error
    {
        public enum EErrorType
        {
            E1, // Error의 경알람 중알람 등등을 정의?
            E2,
            E3,
        }

        /// <summary>
        /// Alarm 에 대한 정보 class
        /// ErrorBase와 ErrorCode 조합 형식때문에 각 ErrorBase당 ErrorCode는 최대 100개로 제한됨
        /// </summary>
        public class CAlarmInfo
        {
            public int Index;           // Primary Key : ErrorBase + ErrorCode 조합
            public EErrorType Type;     // Error Type
            public string[] Description = new string[(int)DEF_Common.ELanguage.MAX];
            public string[] Solution = new string[(int)DEF_Common.ELanguage.MAX];

            public CAlarmInfo(int Index = 0, EErrorType Type = EErrorType.E1)
            {
                this.Index = Index;
                this.Type = Type;
                //for (int i = 0; i < (int)DEF_Common.ELanguage.MAX; i++)
                //{
                //    Name[i] = "reserved";
                //}
                Description[(int)DEF_Common.ELanguage.KOREAN] = "예약";
                Description[(int)DEF_Common.ELanguage.ENGLISH] = "reserved";
                Description[(int)DEF_Common.ELanguage.CHINESE] = "预留";
                Description[(int)DEF_Common.ELanguage.JAPANESE] = "リザーブド";

                Solution[(int)DEF_Common.ELanguage.KOREAN] = "해결방법";
                Solution[(int)DEF_Common.ELanguage.ENGLISH] = "solution";
                Solution[(int)DEF_Common.ELanguage.CHINESE] = "解法";
                Solution[(int)DEF_Common.ELanguage.JAPANESE] = "解決策";
            }

            public string GetText(int lang = (int)DEF_Common.ELanguage.ENGLISH)
            {
                if(lang == (int)DEF_Common.ELanguage.ENGLISH)
                {
                    return $"Text : {Description[lang]}, Solution : {Solution[lang]}";
                }
                else
                {
                    return $"Text : {Description[(int)DEF_Common.ELanguage.ENGLISH]} // {Description[lang]}, Solution : {Solution[(int)DEF_Common.ELanguage.ENGLISH]} // {Solution[lang]}";
                }
            }
        }

        /// <summary>
        /// CAlarmInfo를 가지고 있는 실제 발생한 Alarm 정보
        /// </summary>
        public class CAlarm
        {
            public int ProcessID;
            public int ObjectID;
            public int ErrorBase;
            public int ErrorCode;

            public string ProcessName;
            public string TypeName;
            public string ObjectName;

            public DateTime OccurTime = DateTime.Now; // 발생시간
            public DateTime ResetTime = DateTime.Now; // 조치시간 (현재는 미정)

            public CAlarmInfo Info = new CAlarmInfo();

            public int GetIndex()
            {
                return ErrorBase + ErrorCode;
            }

            public override string ToString()
            {
                return $"Index : {GetIndex()}, Process : {ProcessName}, Object : {ObjectName}, Type : {TypeName}";
            }
        }


        public const int SUCCESS                                                  = 0;

        ////////////////////////////////////////////////////////////////////
        // Process Layer
        ////////////////////////////////////////////////////////////////////
        // TrsStage1
        public const int ERR_TRS_STAGE1_PANEL_DATA_NULL                           = 1;
        public const int ERR_TRS_STAGE1_PANEL_ID_NOT_SAME                         = 2;
        public const int ERR_TRS_STAGE1_PANEL_HISTORY                             = 3;
        public const int ERR_TRS_STAGE1_REPAIR_COUNT                              = 4;
        public const int ERR_TRS_STAGE1_PANEL_DETECTED_BEFORE_LOADING             = 5;
        public const int ERR_TRS_STAGE1_EXCEED_MAX_WAIT_TIME_FOR_SIGNAL           = 6;

        ////////////////////////////////////////////////////////////////////
        // Control Layer
        ////////////////////////////////////////////////////////////////////
        // MCtrlOpPanel
        public const int ERR_MNGOPPANEL_INVALID_OBJECTID                          = 1;
        public const int ERR_MNGOPPANEL_INVALID_ERRORBASE                         = 2;
        public const int ERR_MNGOPPANEL_INVALID_POINTER                           = 3;
        public const int ERR_MNGOPPANEL_INVALID_OPPANEL_OBEJCT                    = 4;
        public const int ERR_MNGOPPANEL_GETSTARTSWSTATUS_INVALID_ARGUMENT_POINTER = 5;
        public const int ERR_MNGOPPANEL_GETSTOPSWSTATUS_INVALID_ARGUMENT_POINTER  = 6;
        public const int ERR_MNGOPPANEL_GETESTOPSWSTATUS_INVALID_ARGUMENT_POINTER = 7;
        public const int ERR_MNGOPPANEL_GETRESETSWSTATUS_INVALID_ARGUMENT_POINTER = 8;
        public const int ERR_MNGOPPANEL_GETDOORSWSTATUS_INVALID_ARGUMENT_POINTER  = 9;
        public const int ERR_MNGOPPANEL_INVALID_JOG_KEY_TYPE                      = 10;
        public const int ERR_MNGOPPANEL_INVALID_JOG_UNIT_INDEX                    = 11;
        public const int ERR_MNGOPPANEL_INVALID_SET_OPPANEL_STATE                 = 12;
        public const int ERR_MNGOPPANEL_CP_TRIP                                   = 13;
        public const int ERR_MNGOPPANEL_DOOR_OPEN                                 = 14;
        public const int ERR_MNGOPPANEL_HEATER_ERROR                              = 15;
        public const int ERR_MNGOPPANEL_MAIN_AIR_ERROR                            = 16;
        public const int ERR_MNGOPPANEL_SUB_AIR_ERROR                             = 17;
        public const int ERR_MNGOPPANEL_EMERGENCY                                 = 18;
        public const int ERR_MNGOPPANEL_NOT_ALL_ORIGIN                            = 19;
        public const int ERR_MNGOPPANEL_NOT_ALL_INIT                              = 20;
        public const int ERR_MNGOPPANEL_AMP_FAULT                                 = 21;
        public const int ERR_MNGOPPANEL_TRAY_AREA_ERROR                           = 22;
        public const int ERR_MNGOPPANEL_BACKUP_HEATER_ERROR                       = 23;
        public const int ERR_MNGOPPANEL_SILICONE_EMPTY_ALL                        = 24;
        public const int ERR_MNGOPPANEL_MAIN_VACCUM_ERROR                         = 25;
        public const int ERR_MNGOPPANEL_MAIN_N2_ERROR                             = 26;
        public const int ERR_MNGOPPANEL_CLEANER_DETECT1_ERROR                     = 27;
        public const int ERR_MNGOPPANEL_CLEANER_DETECT2_ERROR                     = 28;
        public const int ERR_MNGOPPANEL_FRONT_BACK_AREA_SENSOR_DETECTED_ERROR     = 29;
        public const int ERR_MNGOPPANEL_TANK_AREA_SENSOR_DETECTED_ERROR           = 30;
        public const int ERR_MNGOPPANEL_EFD_READY_ON_ERROR                        = 31;
        public const int ERR_MNGOPPANEL_DC_POWER_ON_ERROR                         = 32;

        ////////////////////////////////////////////////////////////////////
        // Mechanical Layer
        ////////////////////////////////////////////////////////////////////
        // MOpPanel
        public const int ERR_OPPANEL_INVALID_OBJECTID                             = 1;
        public const int ERR_OPPANEL_INVALID_ERRORBASE                            = 2;
        public const int ERR_OPPANEL_INVALID_POINTER                              = 3;
        public const int ERR_OPPANEL_INVALID_JOG_KEY_TYPE                         = 4;
        public const int ERR_OPPANEL_INVALID_JOG_UNIT_INDEX                       = 5;
        public const int ERR_OPPANEL_INVALID_INIT_UNIT_INDEX                      = 6;
        public const int ERR_OPPANEL_AMP_FAULT                                    = 7;

        ////////////////////////////////////////////////////////////////////
        // Hardware Layer
        ////////////////////////////////////////////////////////////////////
    }

    public class DEF_IO
    {
        public const int ERR_YMC_NOT_SUPPORT_FUNCTION = 1;
        public const int ERR_YMC_FAIL_GET_DATA_HANDLE = 2;
        public const int ERR_YMC_FAIL_GET_DATA        = 3;
        public const int ERR_YMC_FAIL_SET_DATA        = 4;

        public enum EIOType
        {
            DI,
            DO,
            AI,
            AO,
        }

        public class CIOInfo
        {
            public int Addr;
            public EIOType Type;
            public string[] Name = new string[(int)DEF_Common.ELanguage.MAX];

            public CIOInfo(int Addr = 0, EIOType Type = EIOType.DI)
            {
                this.Addr = Addr;
                this.Type = Type;
                //for (int i = 0; i < (int)DEF_Common.ELanguage.MAX; i++)
                //{
                //    Name[i] = "reserved";
                //}
                Name[(int)DEF_Common.ELanguage.KOREAN]   = "예약";
                Name[(int)DEF_Common.ELanguage.ENGLISH]  = "reserved";
                Name[(int)DEF_Common.ELanguage.CHINESE]  = "预留";
                Name[(int)DEF_Common.ELanguage.JAPANESE] = "リザーブド";
            }
        }

        public const int MAX_IO_INPUT = 256;
        public const int MAX_IO_OUTPUT = 256;

        //
        public const int INPUT_ORIGIN = 1000;
        public const int OUTPUT_ORIGIN = 2000;
        public const int OUTPUT_END = 3000;

        public const int IO_ADDR_NOT_DEFINED = -1;

        // Input X000 
        public const int iStart_SWFront              = 1000;
        public const int iStop_SWFront               = 1001;
        public const int iReset_SWFront              = 1002;
        public const int iStart_SWRear               = 1003;
        public const int iStop_SWRear                = 1004;
        public const int iReset_SWRear               = 1005;
        public const int iEMO_SW                     = 1006;
        public const int iEMO_SWRear                 = 1007;

        public const int iJog_X_Forward_SWFront      = 1008;
        public const int iJog_X_Backward_SWFront     = 1009;
        public const int iJog_Y_Forward_SWFront      = 1010;
        public const int iJog_Y_Backward_SWFront     = 1011;
        public const int iJog_T_CW_SWFront           = 1012;
        public const int iJog_T_CCW_SWFront          = 1013;
        public const int iJog_Z_UP_SWFront           = 1014;
        public const int iJog_Z_DOWN_SWFront         = 1015;

        // Input X010 
        public const int iJog_X_Forward_SWRear       = 1016;
        public const int iJog_X_Backward_SWRear      = 1017;
        public const int iJog_Y_Forward_SWRear       = 1018;
        public const int iJog_Y_Backward_SWRear      = 1019;
        public const int iJog_T_CW_SWRear            = 1020;
        public const int iJog_T_CCW_SWRear           = 1021;
        public const int iJog_Z_UP_SWRear            = 1022;
        public const int iJog_Z_DOWN_SWRear          = 1023;

        public const int iMain_Air_On                = 1024;
        public const int iMain_Vac1_On               = 1025;
        public const int iMain_Vac2_On               = 1026;
        public const int iDoor_HardLock              = 1027;
        public const int iDoor_Front                 = 1028;
        public const int iDoor_Back                  = 1029;
        public const int iDoor_Side                  = 1030;
        public const int iMain_DC_POWER              = 1031; //DV POWER SUPPLY 전력량 모니터링으로 사용함

        // Input X020 
        public const int iStage1_Vac_On              = 1032;
        public const int iStage1_PanelDetect         = 1033;
        public const int iStage2_Vac_On              = 1034;
        public const int iStage2_PanelDetect         = 1035;
        public const int iStage3_Vac_On              = 1036;
        public const int iStage3_PanelDetect         = 1037;
        public const int iStage1Up                   = 1038;
        public const int iStage1Down                 = 1039;

        public const int iWorkbench_Inner_Vac_On     = 1040;
        public const int iWorkbench_PanelDetect      = 1041;
        public const int iWorkbench_Outer_Vac_On     = 1042;
        public const int iStage1Unload               = 1043;
        public const int iSHead1_HeightBase_Backward = 1044;
        public const int iSHead1_HeightBase_Forward  = 1045;
        public const int iSHead2_HeightBase_Backward = 1046;
        public const int iSHead2_HeightBase_Forward  = 1047;

        // Input X030 
        public const int iStage1_BArea_Sensor1       = 1048;
        public const int iStage1_BArea_Sensor2       = 1049;
        public const int iStage1_BArea_Sensor3       = 1050;
        public const int iStage2_FArea_Sensor1       = 1051;
        public const int iStage2_FArea_Sensor2       = 1052;
        public const int iStage2_FArea_Sensor3       = 1053;
        public const int iStage2Up                   = 1054;
        public const int iStage2Down                 = 1055;

        public const int iStage2_BArea_Sensor1       = 1056;
        public const int iStage2_BArea_Sensor2       = 1057;
        public const int iStage2_BArea_Sensor3       = 1058;
        public const int iStage3_FArea_Sensor1       = 1059;
        public const int iStage3_FArea_Sensor2       = 1060;
        public const int iStage3_FArea_Sensor3       = 1061;
        public const int iStage2Load                 = 1062;
        public const int iStage2Unload               = 1063;

        // Input X040 
        public const int iSHead1_UVLED_Ready         = 1064;
        public const int iSHead1_UVLED_Busy1         = 1065;
        public const int iSHead1_UVLED_Busy2         = 1066;
        public const int iSHead1_UVLED_Busy3         = 1067;
        public const int iSHead1_UVLED_Busy4         = 1068;
        public const int iSHead1_UVLED_Error         = 1069;
        public const int iSHead1_UVLED_Alarm         = 1070;
        public const int iDoor_Front2                = 1071;

        public const int iSHead2_UVLED_Ready         = 1072;
        public const int iSHead2_UVLED_Busy1         = 1073;
        public const int iSHead2_UVLED_Busy2         = 1074;
        public const int iSHead2_UVLED_Busy3         = 1075;
        public const int iSHead2_UVLED_Busy4         = 1076;
        public const int iSHead2_UVLED_Error         = 1077;
        public const int iSHead2_UVLED_Alarm         = 1078;
        public const int iDoor_Back2                 = 1079;

        // Input X050 
        public const int iGHead1_UVLED_Ready         = 1080;
        public const int iGHead1_UVLED_Busy1         = 1081;
        public const int iGHead1_UVLED_Busy2         = 1082;
        public const int iGHead1_UVLED_Busy3         = 1083;
        public const int iGHead1_UVLED_Busy4         = 1084;
        public const int iGHead1_UVLED_Error         = 1085;
        public const int iGHead1_UVLED_Alarm         = 1086;
        public const int iDoor_Side2                 = 1087;

        public const int iGHead2_UVLED_Ready         = 1088;
        public const int iGHead2_UVLED_Busy1         = 1089;
        public const int iGHead2_UVLED_Busy2         = 1090;
        public const int iGHead2_UVLED_Busy3         = 1091;
        public const int iGHead2_UVLED_Busy4         = 1092;
        public const int iGHead2_UVLED_Error         = 1093;
        public const int iGHead2_UVLED_Alarm         = 1094;
        public const int iSpare1095                  = 1095;

        // Input X060 
        public const int iUHandler_Self_Vac_On       = 1096;
        public const int iUHandler_PanelDetect       = 1097;
        public const int iUHandler_Factory_Vac_On    = 1098;
        public const int iStage3T90                  = 1099;  // 0.5T 개조하면서, 추가 대형 전용 진공
        public const int iUHandler_Up1               = 1100;
        public const int iUHandler_Down1             = 1101;
        public const int iUHandler_Down2             = 1102;        // SI side에서 쓰는 단동실린더
        public const int iUHandler_Extra_Vac_On      = 1103;

        public const int iSHead1_Tank1_Empty         = 1104;
        public const int iSHead1_Tank2_Empty         = 1105;
        public const int iSHead2_Tank1_Empty         = 1106;
        public const int iSHead2_Tank2_Empty         = 1107;
        public const int iGHead1_Tank1_Empty         = 1108;
        public const int iGHead1_Tank2_Empty         = 1109;
        public const int iGHead2_Tank1_Empty         = 1110;
        public const int iGHead2_Tank2_Empty         = 1111;

        // Input X070 
        public const int iGHead1_Area_Sensor1        = 1112;
        public const int iGHead1_Area_Sensor2        = 1113;
        public const int iGHead1_Area_Sensor3        = 1114;
        public const int iGHead1_Area_Sensor4        = 1115;
        public const int iGHead1_Area_Sensor5        = 1116;
        public const int iGHead2_Area_Sensor1        = 1117;
        public const int iGHead2_Area_Sensor2        = 1118;
        public const int iGHead2_Area_Sensor3        = 1119;

        public const int iGHead2_Area_Sensor4        = 1120;
        public const int iGHead2_Area_Sensor5        = 1121;
        public const int iSHead1_Area_Sensor1        = 1122;
        public const int iSHead1_Area_Sensor2        = 1123;
        public const int iSHead1_Area_Sensor3        = 1124;
        public const int iSHead2_Area_Sensor1        = 1125;
        public const int iSHead2_Area_Sensor2        = 1126;
        public const int iSHead2_Area_Sensor3        = 1127;

        // Input X080 
        public const int iInterface_00               = 1128;
        public const int iInterface_01               = 1129;
        public const int iInterface_02               = 1130;
        public const int iInterface_03               = 1131;
        public const int iInterface_04               = 1132;
        public const int iInterface_05               = 1133;
        public const int iInterface_06               = 1134;
        public const int iInterface_07               = 1135;

        public const int iInterface_08               = 1136;
        public const int iInterface_09               = 1137;
        public const int iInterface_10               = 1138;
        public const int iInterface_11               = 1139;
        public const int iInterface_12               = 1140;
        public const int iInterface_13               = 1141;
        public const int iInterface_14               = 1142;
        public const int iInterface_15               = 1143;

        // Input X090 
        public const int iCamera1_cyl_Up             = 1144; //0.5T 개조
        public const int iCamera1_cyl_Down           = 1145;
        public const int iCamera2_cyl_Up             = 1146;
        public const int iCamera2_cyl_Down           = 1147;
        public const int iSHead1_UVCyl3_Forward      = 1148;
        public const int iSHead1_UVCyl3_Backward     = 1149;
        public const int iSHead2_UVCyl1_Forward      = 1150;
        public const int iSHead2_UVCyl1_Backward     = 1151;

        public const int iSPARE1152                  = 1152;
        public const int iWorkbench_Cyl_Forward      = 1153; //0.5T 개조
        public const int iWorkbench_Cyl_Backward     = 1154;
        public const int iWorkbench_Cyl_Up           = 1155;
        public const int iWorkbench_Cyl_Down         = 1156;
        public const int iGHead1_UVCyl1_Backward     = 1157;
        public const int iGHead2_UVCyl1_Forward      = 1158;
        public const int iGHead2_UVCyl1_Backward     = 1159;

        // Input X0A0 
        public const int iSHead1_UVCyl1_Up           = 1160;
        public const int iSHead1_UVCyl1_Down         = 1161;
        public const int iSHead2_UVCyl1_Up           = 1162;
        public const int iSHead2_UVCyl1_Down         = 1163;
        public const int iSHead1_Cleaner_Up          = 1164;
        public const int iSHead1_Cleaner_Down        = 1165;
        public const int iSHead2_Cleaner_Up          = 1166;
        public const int iSHead2_Cleaner_Down        = 1167;

        public const int iGHead1_Cleaner_Up          = 1168;
        public const int iGHead1_Cleaner_Down        = 1169;
        public const int iGHead2_Cleaner_Up          = 1170;
        public const int iGHead2_Cleaner_Down        = 1171;
        public const int iSHead1_EFD_Ready           = 1172;
        public const int iSHead2_EFD_Ready           = 1173;
        public const int iGHead1_EFD_Ready           = 1174;
        public const int iGHead2_EFD_Ready           = 1175;


        // Input X0B0 
        public const int iSHead1_SHead2_UVLED_Ready  = 1176;
        public const int iSHead1_UVLED_Busy5         = 1177;
        public const int iSHead1_UVLED_Busy6         = 1178;
        public const int iSHead2_UVLED_Busy5         = 1179;
        public const int iSHead2_UVLED_Busy6         = 1180;
        public const int iSHead1_SHead2_UVLED_Error  = 1181;
        public const int iSHead1_SHead2_UVLED_Alarm  = 1182;
        public const int iGHead1_GHead2_UVLED_Ready  = 1183;

        public const int iGHead1_UVLED_Busy5         = 1184;
        public const int iGHead1_UVLED_Busy6         = 1185;
        public const int iGHead2_UVLED_Busy5         = 1186;
        public const int iGHead2_UVLED_Busy6         = 1187;
        public const int iGHead1_GHead2_UVLED_Error  = 1188;
        public const int iGHead1_GHead2_UVLED_Alarm  = 1189;
        public const int iSpare1190                  = 1190;
        public const int iSpare1191                  = 1191;

#if DEF_DEVICENET_INPUT_ADD_ONEMORE
        public const int iSHead1_EFD_TempWarning     = 1192;
        public const int iSHead1_EFD_Alarm           = 1193;
        public const int iSHead2_EFD_TempWarning     = 1194;
        public const int iSHead2_EFD_Alarm           = 1195;
        public const int iGHead1_EFD_TempWarning     = 1196;
        public const int iGHead1_EFD_Alarm           = 1197;
        public const int iGHead2_EFD_TempWarning     = 1198;
        public const int iGHead2_EFD_Alarm           = 1199;

        public const int iSpare1200                  = 1200;
        public const int iSpare1201                  = 1201;
        public const int iSpare1202                  = 1202;
        public const int iSpare1203                  = 1203;
        public const int iSpare1204                  = 1204;
        public const int iSpare1205                  = 1205;
        public const int iSpare1206                  = 1206;
        public const int iSpare1207                  = 1207;

        // Needle : 003----8, Laser : 020----8
        // AD 1
        public const int iSHead1_Laser_Height        = 1144 + 64;
        public const int iSHead2_Laser_Height        = 1160 + 64;
        public const int iSHead1_Needle_Height       = 1176 + 64;
        public const int iSHead2_Needle_Height       = 1192 + 64;

        // AD 2
        public const int iGHead1_Laser_Height        = 1208 + 64;
        public const int iGHead2_Laser_Height        = 1224 + 64;
        public const int iGHead1_Needle_Height       = 1240 + 64;
        public const int iGHead2_Needle_Height       = 1256 + 64;

#else

        // Needle : 003----8, Laser : 020----8
        // AD 1
        public const int iSHead1_Laser_Height        = 1144 + 48;
        public const int iSHead2_Laser_Height        = 1160 + 48;
        public const int iSHead1_Needle_Height       = 1176 + 48;
        public const int iSHead2_Needle_Height       = 1192 + 48;

        // AD 2
        public const int iGHead1_Laser_Height        = 1208 + 48;
        public const int iGHead2_Laser_Height        = 1224 + 48;
        public const int iGHead1_Needle_Height       = 1240 + 48;
        public const int iGHead2_Needle_Height       = 1256 + 48;
#endif

        //////////////////////////////////////////////////////////////////////////
        //                      Output IO Address
        //////////////////////////////////////////////////////////////////////////

        // Output Y000 
        public const int oStart_LampFront            = 2000;
        public const int oStop_LampFront             = 2001;
        public const int oReset_LampFront            = 2002;
        public const int oStart_LampRear             = 2003;
        public const int oStop_LampRear              = 2004;
        public const int oReset_LampRear             = 2005;
        public const int oSpare2006                  = 2006;
        public const int oSpare2007                  = 2007;

        public const int oServo_On                   = 2008;
        public const int oManual_Door_Sleep          = 2009;
        public const int oMarkMoniter_Use1           = 2010;
        public const int oMarkMoniter_Use2           = 2011;
        public const int oSpare2012                  = 2012;
        public const int oSpare2013                  = 2013;
        public const int oSpare2014                  = 2014;
        public const int oMonitior_UsingRear         = 2015;

        // Output Y010 
        public const int oTower_LampRed              = 2016;
        public const int oTower_LampYellow           = 2017;
        public const int oTower_LampGreen            = 2018;
        public const int oSpare2019                  = 2019;
        public const int oBuzzer_1                   = 2020;
        public const int oBuzzer_2                   = 2021;
        public const int oBuzzer_3                   = 2022;
        public const int oBuzzer_4                   = 2023;

        public const int oSpare2024                  = 2024;
        public const int oSpare2025                  = 2025;
        public const int oSpare2026                  = 2026;
        public const int oSpare2027                  = 2027;
#if DEF_USE_SUCTION
        public const int oSuction1                   = 2028;
        public const int oSuction2                   = 2029;
        public const int oSuction3                   = 2030;
        public const int oSuction4                   = 2031;
#else
        public const int oSpare2028                  = 2028;
        public const int oSpare2029                  = 2029;
        public const int oSpare2030                  = 2030;
        public const int oSpare2031                  = 2031;
#endif

        // Output Y020 
        public const int oStage1_Vac_On              = 2032;
        public const int oStage1_Vac_Off             = 2033;
        public const int oStage2_Vac_On              = 2034;
        public const int oStage2_Vac_Off             = 2035;
        public const int oStage3_Vac_On              = 2036;
        public const int oStage3_Vac_Off             = 2037;
        public const int oSpare2038                  = 2038;
        public const int oSpare2039                  = 2039;

        public const int oWorkbench_Inner_Vac_On     = 2040;
        public const int oWorkbench_Inner_Vac_Off    = 2041;
        public const int oWorkbench_Outer_Vac_On     = 2042;
        public const int oWorkbench_Outer_Vac_Off    = 2043;
        public const int oSHead1_HeightBase_Forward  = 2044;
        public const int oSpare2045                  = 2045;
        public const int oSHead2_HeightBase_Forward  = 2046;
        public const int oSpare2047                  = 2047;

        // Output Y030 
        public const int oStage1_Up                  = 2048;
        public const int oStage1_Down                = 2049;
        public const int oStage2_Up                  = 2050;
        public const int oStage2_Down                = 2051;
        public const int oCoat_DI                    = 2052;
        public const int oCoat_PVA                   = 2053;
        public const int oClean_DI                   = 2054;
        public const int oClean_N2                   = 2055;

        public const int oStage1_Blow                = 2056;
        public const int oStage2_Blow                = 2057;
        public const int oStage3_Blow                = 2058;
        public const int oCoater_Ring_Blow           = 2059;
        public const int oCleaner_Ring_Blow          = 2060;
        public const int oSpare2061                  = 2061;
        public const int oSpare2062                  = 2062;
        public const int oSpare2063                  = 2063;

        // Output Y040 
        public const int oStageClamp1_Open           = 2064;
        public const int oStageClamp1_Close          = 2065;
        public const int oStageClamp2_Open           = 2066;
        public const int oStageClamp2_Close          = 2067;
        public const int oSHead1_UVLED_Stop          = 2068;
        public const int oSHead1_UVLED_Mnl_On        = 2069;
        public const int oSHead1_UVLED_EMO_Stop      = 2070;
        public const int oSpare2071                  = 2071;

        public const int oSHead2_UVLED_Start1        = 2072;
        public const int oSHead2_UVLED_Start2        = 2073;
        public const int oSHead2_UVLED_Start3        = 2074;
        public const int oSHead2_UVLED_Start4        = 2075;
        public const int oSHead2_UVLED_Stop          = 2076;
        public const int oSHead2_UVLED_Mnl_On        = 2077;
        public const int oSHead2_UVLED_EMO_Stop      = 2078;
        public const int oSpare2079                  = 2079;

        // Output Y050 
        public const int oGHead1_UVLED_Start1        = 2080;
        public const int oGHead1_UVLED_Start2        = 2081;
        public const int oGHead1_UVLED_Start3        = 2082;
        public const int oGHead1_UVLED_Start4        = 2083;
        public const int oGHead1_UVLED_Stop          = 2084;
        public const int oGHead1_UVLED_Mnl_On        = 2085;
        public const int oGHead1_UVLED_EMO_Stop      = 2086;
        public const int oSpare2087                  = 2087;

        public const int oGHead2_UVLED_Start1        = 2088;
        public const int oGHead2_UVLED_Start2        = 2089;
        public const int oGHead2_UVLED_Start3        = 2090;
        public const int oGHead2_UVLED_Start4        = 2091;
        public const int oGHead2_UVLED_Stop          = 2092;
        public const int oGHead2_UVLED_Mnl_On        = 2093;
        public const int oGHead2_UVLED_EMO_Stop      = 2094;
        public const int oSpare2095                  = 2095;

        // Output Y060 
        public const int oUHandler_Self_Vac_On       = 2096;
        public const int oUHandler_Self_Vac_Off      = 2097;
        public const int oUHandler_Factory_Vac_On    = 2098;
        public const int oUHandler_Factory_Vac_Off   = 2099;
        public const int oUHandler_Up1               = 2100;
        public const int oUHandler_Down1             = 2101;
        public const int oUHandler_Down2             = 2102;        // SI side에서 쓰는 단동실린더
        public const int oUHandler_Extra_Vac_On      = 2103; // 0.5T 개조하면서, 추가 대형 전용 진공

        public const int oSHead1_Tank_Using          = 2104;
        public const int oUHandler_Extra_Vac_Off     = 2105;
        public const int oSHead2_Tank_Using          = 2106;
        public const int oSpare2107                  = 2107;
        public const int oGHead1_Tank_Using          = 2108;
        public const int oSpare2109                  = 2109;
        public const int oGHead2_Tank_Using          = 2110;
        public const int oSpare2111                  = 2111;

        // Output Y070 
#if DEF_USE_UV_ACTINOMETER
        public const int oSHead_UVCyl_Forward        = 2112;
        public const int oSHead_UVCyl_Backward       = 2113;
        public const int oSHead_UVCyl_Up             = 2114;
        public const int oSHead_UVCyl_Down           = 2115;
        public const int oUVCyl_Shutter_Forward      = 2116;
        public const int oUVCyl_Shutter_Backward     = 2117;
#else
        public const int oCamera1_cyl_Up             = 2112; //0.5T
        public const int oCamera1_cyl_Down           = 2113;
        public const int oCamera2_cyl_Up             = 2114;
        public const int oCamera2_cyl_Down           = 2115;
        public const int oSpare_2116                 = 2116;
        public const int oSpare_2117                 = 2117;
#endif

#if DEF_NEW_CLEAN_SYSTEM
        public const int oCleanerCyl_Up              = 2118;
        public const int oCleanerCyl_Down            = 2119;
#else
        public const int oSpare_2118                 = 2118;
        public const int oSpare_2119                 = 2119;
#endif

#if DEF_NEW_CLEAN_SYSTEM
        public const int oCleaningPump1_On           = 2120;
#else
        public const int oSpare_2120                 = 2120;
#endif
        public const int oWorkbench_Cyl_Forward      = 2121; //0.5T
        public const int oWorkbench_Cyl_Backward     = 2122;
        public const int oWorkbench_Cyl_Up           = 2123;
        public const int oWorkbench_Cyl_Down         = 2124;
        public const int oSpare_2125                 = 2125;
        public const int oSpare_2126                 = 2126;
        public const int oSpare_2127                 = 2127;

        // Output Y080 
        public const int oInterface_00               = 2128;
        public const int oInterface_01               = 2129;
        public const int oInterface_02               = 2130;
        public const int oInterface_03               = 2131;
        public const int oInterface_04               = 2132;
        public const int oInterface_05               = 2133;
        public const int oInterface_06               = 2134;
        public const int oInterface_07               = 2135;

        public const int oInterface_08               = 2136;
        public const int oInterface_09               = 2137;
        public const int oInterface_10               = 2138;
        public const int oInterface_11               = 2139;
        public const int oInterface_12               = 2140;
        public const int oInterface_13               = 2141;
        public const int oInterface_14               = 2142;
        public const int oInterface_15               = 2143;

#if DEF_UV_6_LAMP
        // Output Y090     // 추가된 I/O
        public const int oSHead1_UVLED_Start5        = 2144;
        public const int oSHead1_UVLED_Start6        = 2145;
        public const int oSHead2_UVLED_Start5        = 2146;
        public const int oSHead2_UVLED_Start6        = 2147;
        public const int oSHead1_SHead2_UVLED_Stop   = 2148;
        public const int oSHead1_SHead2_UVLED_Mnl_On = 2149;
        public const int oSpare2150                  = 2150;
        public const int oGHead1_UVLED_Start5        = 2151;

        public const int oGHead1_UVLED_Start6        = 2152;
        public const int oGHead2_UVLED_Start5        = 2153;
        public const int oGHead2_UVLED_Start6        = 2154;
        public const int oGHead1_GHead2_UVLED_Stop   = 2155;
        public const int oGHead1_GHead2_UVLED_Mnl_On = 2156;
        public const int oSpare2157                  = 2157;
        public const int oSpare2158                  = 2158;
        public const int oSpare2159                  = 2159;
#endif

        // OUTPUT 모듈이 추가되면 + 0 부분을 추가되는 점수만큼 증가 시킬것.
        public const int oStage1_Cam1_LED_Light      = 2144 + 16;
        public const int oStage1_Cam2_LED_Light      = 2160 + 16;

        public const int oStage1_Cam1_LED_Ring_Light = 2176 + 16;
        public const int oStage1_Cam2_LED_Ring_Light = 2192 + 16;

    }


    public class DEF_UI
    {
        /// <summary>
        /// Main UI
        /// </summary>
        public static readonly int FORM_SIZE_WIDTH = 1284;  // Main Frame은 약간 크게 
        public static readonly int FORM_SIZE_HEIGHT = 1024; 
        public static readonly int FORM_POS_X = 0;
        public static readonly int FORM_POS_Y = 0;

        public static readonly int MAIN_SIZE_WIDTH = 1278;
        public static readonly int MAIN_SIZE_HEIGHT = 818;

        public static readonly int MAIN_POS_X = 0;
        public static readonly int MAIN_POS_Y = 100;

        public static readonly int TOP_SIZE_WIDTH = 1278;
        public static readonly int TOP_SIZE_HEIGHT = 98;

        public static readonly int TOP_POS_X = 0;
        public static readonly int TOP_POS_Y = 0;

        public static readonly int BOT_SIZE_WIDTH = 1278;
        public static readonly int BOT_SIZE_HEIGHT = 100;

        public static readonly int BOT_POS_X = 0;
        public static readonly int BOT_POS_Y = 920;
        
        public enum SelectScreenType
        {
            Auto_Scr,
            Manual_Scr,
            Data_Scr,
            Teach_Scr,
            Log_Scr,
            Help_Scr,
        }
    }
}