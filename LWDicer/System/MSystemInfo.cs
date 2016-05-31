using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_System.EObjectLayer;

namespace LWDicer.Control
{
    public class MSystemInfo
    {
        CObjectInfo[] arrayObjectInfo;

        public MSystemInfo()
        {
            arrayObjectInfo = new CObjectInfo[]
            {
                // 0-39 : Common & Hardware
                new CObjectInfo( (int)OBJ_SYSTEM, "System", 0, "MLWDicer", 0, "System", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_DATAMANAGER, "DataManger", 1, "DataManager", 100, "DataManager", LOG_ALL, LOG_DAY ),

	            new CObjectInfo( (int)OBJ_HL_MOTION_LIB, "MotionLib", 2, "MMC", 500, "MotionLib", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MOTION_LIB, "MotionLib", 3, "YMC", 600, "YMCLib", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MOTION_LIB, "MotionLib", 4, "ACS", 700, "ACSLib", LOG_ALL, LOG_DAY ),

	            new CObjectInfo( (int)OBJ_HL_IO, "IO", 6, "Device Net", 1000, "IO", LOG_ALL, LOG_DAY ),	
		
	            new CObjectInfo( (int)OBJ_HL_MELSEC, "Melsec", 11, "Melsec", 1100, "Melsec", LOG_ALL, LOG_DAY ),
		
	            // 30-39 : Serial ------------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial RS232", 30, "SHead1", 1200, "RS232_SHead1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial RS232", 31, "SHead2", 1200, "RS232_SHead2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial RS232", 32, "GHead1", 1200, "RS232_GHead1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial RS232", 33, "GHead2", 1200, "RS232_GHead2", LOG_ALL, LOG_DAY ),
		
                // 40-59 : Vision ------------------------------------------------------------------------
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 40, "System",   1500, "VisionSystem", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 42, "Camera1",  1600, "VisionCamera1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 43, "Camera2",  1600, "VisionCamera2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 46, "Display1", 1700, "VisionDisplay1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 47, "Display2", 1700, "VisionDisplay2", LOG_ALL, LOG_DAY ),
		
	            // 40-49 : Dummy Reserved
	            // 50-59 : Ethernet Reserved
	            // 60-69 : BarCode Reserved
	            // 70-79 : Melsec
		
	            // 80-99 : Reserved
		
	            // 100-149 : Cylinders--------------------------------------------------------------------
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 100, "PushPull Gripper Lock/Unlock", 2000, "PushPullGripper", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 101, "PushPull Up/Down",             2000, "PushPullUD", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 102, "Spinner1 Up/Down",             2000, "S1_UDCyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 103, "Spinner1 DI Valve",            2000, "S1_DICyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 104, "Spinner1 PVA Valve",           2000, "S1_PVACyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 105, "Spinner2 Up/Down",             2000, "S2_UDCyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 106, "Spinner2 DI Valve",            2000, "S2_DICyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 107, "Spinner2 PVA Valve",           2000, "S2_PVACyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 108, "Stage Clamp1 Open/Close",      2000, "StageClamp1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 109, "Stage Clamp2 Open/Close",      2000, "StageClamp2", LOG_ALL, LOG_DAY ),

	            // 150-199 : Vacuums ------------------------------------------------------------------------------	
	            new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 150, "Stage1",           2100, "Stage1Vac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 151, "Spinner1",         2100, "Spinner1Vac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 152, "Spinner1",         2100, "Spinner2Vac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 153, "UHandler Self",    2100, "UHandlerSelfVac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 154, "UHandler Factory", 2100, "UHandlerFactoryVac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 155, "LHandler Self",    2100, "LHandlerSelfVac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 156, "LHandler Factory", 2100, "LHandlerFactoryVac", LOG_ALL, LOG_DAY ),

	            //new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 153, "Workbench Inner", 2100, "WorkbenchInnerVac", LOG_ALL, LOG_DAY ),
                //new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 154, "Workbench Outer", 2100, "WorkbenchOuterVac", LOG_ALL, LOG_DAY ),
                //new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 157, "UHandler Extra", 2100, "UHandlerExtraVac", LOG_ALL, LOG_DAY ),
		
	            // 200-209 : Scanner & Laser  -------------------------------------------------------------	
                new CObjectInfo( (int)OBJ_ML_POLYGON,   "PolygonScanner",    200, "Polygon Scanner",  2200, "PolygonScanner",  LOG_ALL, LOG_DAY ),
	            // 210-249 : Multi Actuator, Induction Motor, etc Reserved -------------------------------------------------------------

	            // 250-299 : Motion Multi Axes --------------------------------------------------------------------		
	            new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 250, "Stage1",               2500, "MA_Stage1",        LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 251, "Loader",               2500, "MA_Loader",        LOG_ALL, LOG_DAY ),	
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 252, "PushPull",             2500, "MA_PushPull",      LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 253, "Coater1 Centering",    2500, "MA_Centering1",    LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 254, "Coater1 ChuckRotate",  2500, "MA_Chuck1",        LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 255, "Coater1 CleanNozzle",  2500, "MA_CleanNozzle1",  LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 256, "Coater1 CoatNozzle",   2500, "MA_CoatNozzle1",   LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 257, "Coater2 Centering",    2500, "MA_Centering2",    LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 258, "Coater2 ChuckRotate",  2500, "MA_Chuck2",        LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 259, "Coater2 CleanNozzle",  2500, "MA_CleanNozzle2",  LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 260, "Coater2 CoatNozzle",   2500, "MA_CoatNozzle2",   LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 261, "Handler1",             2500, "MA_Handler1",      LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 262, "Handler2",             2500, "MA_Handler2",      LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 263, "Camera1",              2500, "MA_Camera1",       LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 264, "Laser1",               2500, "MA_Laser1",        LOG_ALL, LOG_DAY ),
		
	            // 300-349 : Mechanical Layer --------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_ML_OP_PANEL  , "Mechanical",   300, "OpPanel",  3000, "OpPanel", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_ELEVATOR  , "Mechanical",   301, "Loader",   3100, "Loader", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_PUSHPULL  , "Mechanical",   302, "PushPull", 3200, "PushPull", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_STAGE     , "Mechanical",   303, "Stage1",   3300, "Stage1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_SPINNER   , "Mechanical",   304, "Spinner1", 3400, "Spinner1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_SPINNER   , "Mechanical",   305, "Spinner2", 3400, "Spinner2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_HANDLER   , "Mechanical",   306, "UHandler", 3500, "UHandler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_HANDLER   , "Mechanical",   307, "LHandler", 3500, "LHandler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_VISION    , "Mechanical",   308, "Vision",   3600, "Vision", LOG_ALL, LOG_DAY ),

	            // 350-399 : Control Layer --------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_CL_OP_PANEL          , "Control", 350, "OPPanel",         5000, "C_OpPanel", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_LOADER            , "Control", 351, "Loader",          5100, "C_Loader", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_PUSHPULL          , "Control", 352, "PushPull",        5200, "C_PushPull", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_STAGE1            , "Control", 353, "Stage1",          5300, "C_Stage1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_SPINNER           , "Control", 354, "Spinner1",        5400, "C_Spinner1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_SPINNER           , "Control", 355, "Spinner2",        5400, "C_Spinner2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_HANDLER           , "Control", 356, "Handler",         5500, "C_Handler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_VISION_CALIBRATION, "Control", 360, "Calibration1",    5600, "C_VisionCalib1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_VISION_CALIBRATION, "Control", 361, "Calibration2",    5600, "C_VisionCalib2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_HW_TEACH          , "Control", 362, "HW Teach",        5700, "C_HWTeach", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_INTERFACE_CTRL    , "Control", 363, "Interface1",      5800, "C_Interface1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_INTERFACE_CTRL    , "Control", 364, "Interface2",      5800, "C_Interface2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_MANAGE_PRODUCT    , "Control", 365, "Manage Product",  5900, "C_ManageProduct", LOG_ALL, LOG_DAY ),


	            // 400-459 : Process Layer --------------------------------------------------------------------
                new CObjectInfo( (int)OBJ_PL_TRS_AUTO_MANAGER  , "Process",    400, "AutoManager",  7000, "TrsAutoManager", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_LOADER        , "Process",    401, "Loader",       7100, "TrsLoader", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_PUSHPULL      , "Process",    402, "PushPull",     7200, "TrsPushPull", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_STAGE1        , "Process",    403, "Stage1",       7300, "TrsStage1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_SPINNER       , "Process",    404, "Spinner1",     7400, "TrsSpinner1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_SPINNER       , "Process",    405, "Spinner2",     7400, "TrsSpinner2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_HANDLER       , "Process",    406, "Handler",      7500, "TrsHandler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_JOG           , "Process",    410, "Jog",          8000, "TrsJog", LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_PL_TRS_LCNET         , "Process",    411, "LCNet",        8100, "TrsLCNet", LOG_ALL, LOG_DAY ),
            };

        }

        public bool GetObjectInfo(int ID, out CObjectInfo objInfo)
        {
            objInfo = new CObjectInfo();
            foreach(CObjectInfo objectInfo in arrayObjectInfo)
            {
                if(objectInfo.ID == ID)
                {
                    objInfo = objectInfo;
                    return true;
                }
            }

            //return ERR_SYSTEMINFO_NOT_REGISTED_OBJECTID;
            return false;
        }

        public CObjectInfo GetObjectInfo(int ID)
        {
            CObjectInfo objInfo = new CObjectInfo();
            foreach (CObjectInfo objectInfo in arrayObjectInfo)
            {
                if (objectInfo.ID == ID)
                {
                    objInfo = objectInfo;
                    break;
                }
            }
            return objInfo;
        }

        public string GetObjectName(int ID)
        {
            CObjectInfo objInfo = new CObjectInfo();
            foreach (CObjectInfo objectInfo in arrayObjectInfo)
            {
                if (objectInfo.ID == ID)
                {
                    objInfo = objectInfo;
                    break;
                }
            }
            return objInfo.Name;
        }

        public string GetTypeName(int ID)
        {
            CObjectInfo objInfo = new CObjectInfo();
            foreach (CObjectInfo objectInfo in arrayObjectInfo)
            {
                if (objectInfo.ID == ID)
                {
                    objInfo = objectInfo;
                    break;
                }
            }
            return objInfo.TypeName;
        }
    }
}
