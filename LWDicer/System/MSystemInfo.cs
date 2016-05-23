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

	            new CObjectInfo( (int)OBJ_HL_MOTION_LIB, "MotionLib", 2, "MMC Board", 500, "MotionLib", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MOTION_LIB, "MotionLib", 3, "YMC Board", 600, "YMCLib", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MOTION_LIB, "MotionLib", 4, "ACS Board", 700, "ACSLib", LOG_ALL, LOG_DAY ),

	            new CObjectInfo( (int)OBJ_HL_IO, "IO", 6, "Device Net", 1000, "IO", LOG_ALL, LOG_DAY ),	
		
	            new CObjectInfo( (int)OBJ_HL_MELSEC, "Melsec", 11, "Melsec", 1100, "Melsec", LOG_ALL, LOG_DAY ),
		
	            // 30-39 : Serial ------------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial", 30, "RS232 SHead1", 1200, "RS232_SHead1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial", 31, "RS232 SHead2", 1200, "RS232_SHead2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial", 32, "RS232 GHead1", 1200, "RS232_GHead1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_SERIAL, "Serial", 33, "RS232 GHead2", 1200, "RS232_GHead2", LOG_ALL, LOG_DAY ),
		
                // 40-59 : Vision ------------------------------------------------------------------------
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 40, "HardWare : Vision System",   1500, "System", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 42, "HardWare : Vision Camera1",  1600, "Camera1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 43, "HardWare : Vision Camera2",  1600, "Camera2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 46, "HardWare : Vision Display1", 1700, "Display1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VISION, "Vision", 47, "HardWare : Vision Display2", 1700, "Display2", LOG_ALL, LOG_DAY ),
		
	            // 40-49 : Dummy Reserved
	            // 50-59 : Ethernet Reserved
	            // 60-69 : BarCode Reserved
	            // 70-79 : Melsec
		
	            // 80-99 : Reserved
		
	            // 100-149 : Cylinders--------------------------------------------------------------------
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 100, "PushPull Gripper Lock/Unlock", 2000, "PushPullGripper", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 101, "PushPull Up/Down", 2000, "PushPullUD", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 110, "Spinner1 Up/Down", 2000, "S1_UDCyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 111, "Spinner1 DI Valve", 2000, "S1_DICyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 112, "Spinner1 PVA Valve", 2000, "S1_PVACyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 113, "Spinner2 Up/Down", 2000, "S2_UDCyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 114, "Spinner2 DI Valve", 2000, "S2_DICyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 115, "Spinner2 PVA Valve", 2000, "S2_PVACyl", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 116, "Work Stage Clamp 1 Open/Close", 8000, "StageClamp1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_CYLINDER, "Cylinder", 117, "Work Stage Clamp 2 Open/Close", 8000, "StageClamp2", LOG_ALL, LOG_DAY ),

	            // 150-199 : Vacuums ------------------------------------------------------------------------------	
	            new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 150, "Stage1 Vacuum", 2100, "Stage1Vac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 151, "Spinner1 Vacuum", 2100, "Spinner1Vac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 152, "Spinner1 Vacuum", 2100, "Spinner2Vac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 153, "UHandler Self Vacuum", 2100, "UHandlerSelfVac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 154, "UHandler Factory Vacuum", 2100, "UHandlerFactoryVac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 155, "LHandler Self Vacuum", 2100, "LHandlerSelfVac", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 156, "LHandler Factory Vacuum", 2100, "LHandlerFactoryVac", LOG_ALL, LOG_DAY ),

	            //new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 153, "Workbench Inner Vacuum", 2100, "WorkbenchInnerVac", LOG_ALL, LOG_DAY ),
             //   new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 154, "Workbench Outer Vacuum", 2100, "WorkbenchOuterVac", LOG_ALL, LOG_DAY ),
             //   new CObjectInfo( (int)OBJ_HL_VACUUM, "Vacuum", 157, "UHandler Extra Vacuum", 2100, "UHandlerExtraVac", LOG_ALL, LOG_DAY ),
		
	            // 200-209 : Scanner & Laser  -------------------------------------------------------------	
                new CObjectInfo( (int)OBJ_ML_POLYGON,   "PolygonScanner",    200, "Polygon Scanner",  2200, "PolygonScanner",  LOG_ALL, LOG_DAY ),
	            // 210-249 : Multi Actuator, Induction Motor, etc Reserved -------------------------------------------------------------

	            // 250-299 : Motion Multi Axes --------------------------------------------------------------------		
	            new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 250, "Stage1 Motion",               2500, "MA_Stage1",        LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 251, "Loader Motion",               2500, "MA_Loader",        LOG_ALL, LOG_DAY ),	
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 252, "PushPull Motion",             2500, "MA_PushPull",      LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 253, "Coater1 Centering Motion",    2500, "MA_Centering1",    LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 254, "Coater1 ChuckRotate Motion",  2500, "MA_Chuck1",        LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 255, "Coater1 CleanNozzle Motion",  2500, "MA_CleanNozzle1",  LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 256, "Coater1 CoatNozzle Motion",   2500, "MA_CoatNozzle1",   LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 257, "Coater2 Centering Motion",    2500, "MA_Centering2",    LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 258, "Coater2 ChuckRotate Motion",  2500, "MA_Chuck2",        LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 259, "Coater2 CleanNozzle Motion",  2500, "MA_CleanNozzle2",  LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 260, "Coater2 CoatNozzle Motion",   2500, "MA_CoatNozzle2",   LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 261, "Handler1 Motion",             2500, "MA_Handler1",      LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 262, "Handler2 Motion",             2500, "MA_Handler2",      LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 263, "Camera1 Motion",              2500, "MA_Camera1",       LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_HL_MULTIAXES_YMC, "MultiAxes_YMC", 264, "Laser1 Motion",               2500, "MA_Laser1",        LOG_ALL, LOG_DAY ),
		
	            // 300-349 : Mechanical Layer --------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_ML_OP_PANEL  , "OpPanel",    300, "Mechanical : OpPanel",  3000, "OpPanel", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_ELEVATOR  , "Loader",     301, "Mechanical : Loader",   3100, "Loader", LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_ML_STAGE     , "Stage1",     302, "Mechanical : Stage1",   3200, "Stage1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_PUSHPULL  , "PushPull",   303, "Mechanical : PushPull", 3300, "PushPull", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_SPINNER   , "Spinner1",   304, "Mechanical : Spinner1", 3400, "Spinner1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_SPINNER   , "Spinner2",   305, "Mechanical : Spinner2", 3400, "Spinner2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_HANDLER   , "UHandler",   306, "Mechanical : UHandler", 3500, "UHandler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_HANDLER   , "LHandler",   307, "Mechanical : LHandler", 3500, "LHandler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_ML_VISION    , "Vision",     308, "Mechanical : Vision",   3600, "Vision", LOG_ALL, LOG_DAY ),

	            // 350-399 : Control Layer --------------------------------------------------------------------
	            new CObjectInfo( (int)OBJ_CL_OP_PANEL          , "CtrlOpPanel",       350, "Control : OPPanel",         5000, "C_OpPanel", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_LOADER            , "CtrlLoader",        351, "Control : Loader",          5100, "C_Loader", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_PUSHPULL          , "CtrlPushPull",      352, "Control : PushPull",        5200, "C_PushPull", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_STAGE1            , "CtrlStage1",        353, "Control : Stage1",          5300, "C_Stage1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_SPINNER           , "CtrlSpinner1",      354, "Control : Spinner1",        5400, "C_Spinner1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_SPINNER           , "CtrlSpinner2",      355, "Control : Spinner2",        5400, "C_Spinner2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_HANDLER           , "CtrlHandler",       356, "Control : Handler",         5500, "C_Handler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_VISION_CALIBRATION, "VisionCalibration", 360, "Control : Calibration1",    5600, "C_VisionCalib1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_VISION_CALIBRATION, "VisionCalibration", 361, "Control : Calibration2",    5600, "C_VisionCalib2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_HW_TEACH          , "HWTeach",           362, "Control : HW Teach",        5700, "C_HWTeach", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_INTERFACE_CTRL    , "InterfaceCtrl1",    363, "Control : Interface Ctrl1", 5800, "C_InterfaceCtrl1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_INTERFACE_CTRL    , "InterfaceCtrl2",    364, "Control : Interface Ctrl2", 5800, "C_InterfaceCtrl2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_CL_MANAGE_PRODUCT    , "ManageProduct",     365, "Control : Manage Product",  5900, "C_ManageProduct", LOG_ALL, LOG_DAY ),


	            // 400-459 : Process Layer --------------------------------------------------------------------
                new CObjectInfo( (int)OBJ_PL_TRS_AUTO_MANAGER  , "TrsAutoManager",   400, "Process : TrsAuto Manager",  7000, "TrsAutoManager", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_LOADER        , "TrsLoader",        401, "Process : TrsLoader",        7100, "TrsLoader", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_PUSHPULL      , "TrsPushPull",      402, "Process : TrsPushPull",      7200, "TrsPushPull", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_STAGE1        , "TrsStage1",        403, "Process : TrsStage1",        7300, "TrsStage1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_SPINNER       , "TrsSpinner1",      404, "Process : Spinner1",         7400, "TrsSpinner1", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_SPINNER       , "TrsSpinner2",      405, "Process : Spinner2",         7400, "TrsSpinner2", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_HANDLER       , "TrsHandler",       406, "Process : Handler",          7500, "TrsHandler", LOG_ALL, LOG_DAY ),
                new CObjectInfo( (int)OBJ_PL_TRS_JOG           , "TrsJog",           410, "Process : TrsJog",           8000, "TrsJog", LOG_ALL, LOG_DAY ),
	            new CObjectInfo( (int)OBJ_PL_TRS_LCNET         , "TrsLCNet",         411, "Process : TrsLCNet",         8100, "TrsLCNet", LOG_ALL, LOG_DAY ),
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
    }
}
