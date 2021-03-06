using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_MeStage;
using static Core.Layers.DEF_Motion;
using static Core.Layers.DEF_IO;
using static Core.Layers.DEF_System;
using Core.UI;

namespace Core.Layers
{
    public class DEF_MeStage
    {
        #region Data Define

        // Error Define
        public const int ERR_STAGE_NOT_ORIGIN_RETURNED                      = 1;
        public const int ERR_STAGE_UNABLE_TO_USE_CYL                        = 2;
        public const int ERR_STAGE_UNABLE_TO_USE_VCC                        = 3;
        public const int ERR_STAGE_UNABLE_TO_USE_AXIS                       = 4;
        public const int ERR_STAGE_TOO_LOW_JOG_SPEED                        = 9;
        public const int ERR_STAGE_FAIL_TO_GET_CURRENT_POS_INFO             = 20;
        public const int ERR_STAGE_UNABLE_TO_USE_POSITION                   = 21;
        public const int ERR_STAGE_MOVE_FAIL                                = 22;
        public const int ERR_STAGE_READ_CURRENT_POSITION                    = 23;
        public const int ERR_STAGE_CASSETTE_NOT_READY                       = 24;
        public const int ERR_STAGE_VACUUM_ON_TIME_OUT                       = 40;
        public const int ERR_STAGE_VACUUM_OFF_TIME_OUT                      = 41;
        public const int ERR_STAGE_INVALID_PARAMETER                        = 42;
        public const int ERR_STAGE_OBJECT_DETECTED_BUT_NOT_ABSORBED         = 43;
        public const int ERR_STAGE_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED     = 44;

        // System Define
        public const int WAFER_CLAMP_CYL_1                                  = 0;
        public const int WAFER_CLAMP_CYL_2                                  = 1;
        public const int WAFER_CLAMP_CYL_NUM                                = 2;

        public enum EStageCtrlMode
        {
            LASER =0,
            PC
        }
        public enum EStageVacuum
        {
            SELF,           // 자체 발생 진공
            FACTORY,        // 공장 진공
            OBJECT,         // LCD 패널의 PCB같은 걸 집는 용도
            EXTRA_SELF,     // 
            EXTRA_FACTORY,  //
            EXTRA_OBJECT,   //
            MAX,
        }


        /// <summary>
        /// Camera Unit Position
        /// </summary>
        public enum ECameraPos
        {
            NONE = -1,
            WAIT,
            WORK,
            INSPEC_FOCUS,
            FINE_FOCUS,
            FOCUS_3,
            MAX,
        }

        /// <summary>
        /// Scanner Unit Position
        /// </summary>
        public enum EScannerPos
        {
            NONE = -1,
            WAIT,
            WORK,
            FOCUS_1,
            FOCUS_2,
            FOCUS_3,
            MAX,
        }

        public enum EStagePos
        {
            NONE = -1,
            WAIT,
            LOAD,
            UNLOAD,
            STAGE_CENTER_PRE,       // Stage의 Center 위치 (Pre Cam 기준)
            STAGE_CENTER_FINE,      // Stage의 Center 위치 (Fine Cam 기준)
            STAGE_CENTER_INSPECT,   // Stage의 Center 위치 (Inspect Cam 기준)
            THETA_ALIGN_TURN_A,     // Theta Align Turn 시 "A" 위치
            EDGE_ALIGN_1,           // EDGE Detect "0"도 위치
            MACRO_CAM_POS,          // KEY가 MACRO CAM영상의 CENTER일때 STAGE 위치
            MACRO_ALIGN,            // MACRO Align "A" Mark 위치
            MICRO_ALIGN,            // MICRO Align "A" Mark 위치
            MICRO_ALIGN_TURN,       // MICRO Align Turn 후 "A" Mark 위치
            LASER_PROCESS,          // Laser Cutting할 첫 위치 (가로 방향)
            LASER_PROCESS_TURN,     // Laser Cutting할 첫 위치 (세로 방향)

            VISION_LASER_GAP,
            MAX,
        }

        public enum EAlignCameraPos
        {
            NONE = -1,
            MACRO,
            MICRO,
            MAX,
        }

        public enum EStageXAxZone
        {
            NONE = -1,
            LOAD,
            WAIT,
            UNLOAD,
            SAFETY,
            MAX,
        }

        public enum EStageYAxZone
        {
            NONE = -1,
            LOAD,
            WAIT,
            UNLOAD,
            SAFETY,
            MAX,
        }

        public enum EStageTAxZone
        {
            NONE = -1,
            LOAD,
            WAIT,
            UNLOAD,
            SAFETY,
            MAX,
        }

        public enum EStageZAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public class CMeStageRefComp
        {
            public IIO IO;

        }

        public class CMeStageData
        {
            // Detect Object Sensor Address
            public int InDetectObject   = IO_ADDR_NOT_DEFINED;

            // IO Address for manual control cylinder
            public int InClampOpen1     = IO_ADDR_NOT_DEFINED;
            public int InClampClose1    = IO_ADDR_NOT_DEFINED;
            public int InClampOpen2     = IO_ADDR_NOT_DEFINED;            
            public int InClampClose2    = IO_ADDR_NOT_DEFINED;

            public int OutClampOpen1    = IO_ADDR_NOT_DEFINED;
            public int OutClampClose1   = IO_ADDR_NOT_DEFINED;
            public int OutClampOpen2    = IO_ADDR_NOT_DEFINED;
            public int OutClampClose2   = IO_ADDR_NOT_DEFINED;

            // Vacuum
            public bool[] UseVccFlag = new bool[(int)EStageVacuum.MAX];

            // Physical check zone sensor. 원점복귀 여부와 상관없이 축의 물리적인 위치를 체크 및
            // 안전위치 이동 check 
            public CMAxisZoneCheck StageZone;
            public CPos_XYTZ StageSafetyPos;

            public CMeStageData()
            {
                StageZone = new CMAxisZoneCheck((int)EStageXAxZone.MAX, (int)EStageYAxZone.MAX,
                    (int)EStageTAxZone.MAX, (int)EStageZAxZone.MAX);
            }            
        }

#endregion
    }

    public class MMeStage : MMechanicalLayer
    {
        private CMeStageRefComp m_RefComp;
        private CMeStageData m_Data;

        // MovingObject
        public CMovingObject AxStageInfo { get; private set; } = new CMovingObject((int)EStagePos.MAX);
        public CMovingObject AxCameraInfo { get; private set; } = new CMovingObject((int)ECameraPos.MAX);
        public CMovingObject AxScannerInfo { get; private set; } = new CMovingObject((int)EScannerPos.MAX);

        // Cylinder
        private bool[] UseMainCylFlag   = new bool[WAFER_CLAMP_CYL_NUM];
        private bool[] UseSubCylFlag    = new bool[WAFER_CLAMP_CYL_NUM];
        private bool[] UseGuideCylFlag  = new bool[WAFER_CLAMP_CYL_NUM];

        public MMeStage(CObjectInfo objInfo, CMeStageRefComp refComp, CMeStageData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        // Data & Flag 설정
        #region Data & Flag 설정
        public int SetData(CMeStageData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CMeStageData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetPosition_Stage(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            return AxStageInfo.SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
        }

        public int GetStagePosition(out CPositionSet Pos_Fixed, out CPositionSet Pos_Model, out CPositionSet Pos_Offset)
        {
            return AxStageInfo.GetPositionSet(out Pos_Fixed,out Pos_Model,out Pos_Offset);
        }

        public int SetPosition_Camera(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            return AxCameraInfo.SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
        }

        public int GetCameraPosition(out CPositionSet Pos_Fixed, out CPositionSet Pos_Model, out CPositionSet Pos_Offset)
        {
            return AxCameraInfo.GetPositionSet(out Pos_Fixed, out Pos_Model, out Pos_Offset);
        }

        public int SetPosition_Scanner(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            return AxScannerInfo.SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
        }

        public int GetScannerPosition(out CPositionSet Pos_Fixed, out CPositionSet Pos_Model, out CPositionSet Pos_Offset)
        {
            return AxScannerInfo.GetPositionSet(out Pos_Fixed, out Pos_Model, out Pos_Offset);
        }

        public CPos_XYTZ GetTargetPosition(int index,bool withAlign=true)
        {
            return AxStageInfo.GetTargetPos(index,withAlign);
        }
        

        public int SetCylUseFlag(bool[] UseMainCylFlag = null, bool[] UseSubCylFlag = null, bool[] UseGuideCylFlag = null)
        {
            if(UseMainCylFlag != null)
            {
                Array.Copy(UseMainCylFlag, this.UseMainCylFlag, UseMainCylFlag.Length);
            }
            if (UseSubCylFlag != null)
            {
                Array.Copy(UseSubCylFlag, this.UseSubCylFlag, UseSubCylFlag.Length);
            }
            if (UseGuideCylFlag != null)
            {
                Array.Copy(UseGuideCylFlag, this.UseGuideCylFlag, UseGuideCylFlag.Length);
            }

            return SUCCESS;
        }

        #endregion
        
            
        // Stage Servo 구동
        #region Stage Move 동작

        public int GetStageCurPos(out CPos_XYTZ pos)
        {
            int iResult = 0;
            CPos_XYTZ temp = new CPos_XYTZ();
            pos = temp;

            return 0;
        }

        public int GetStageCmdPos(out CPos_XYTZ pos)
        {
            int iResult = 0;
            CPos_XYTZ temp = new CPos_XYTZ();
            pos = temp;

            return 0;
        }

        public bool IsStageBusy()
        {
            return true;
        }
        public CPos_XYTZ GetStageTeachPos(int iPos)
        {
            return AxStageInfo.GetTargetPos(iPos);
        }
        

        public int MoveStagePos(CPos_XYTZ sPos)
        {
            return MoveStagePos(sPos, null, false, false, null, false);
        }
        public int MoveStagePos(CPos_XYTZ sPos, bool bHighSpeed = false)
        {
            return MoveStagePos(sPos, null, false, false, null, bHighSpeed);
        }
        /// <summary>
        /// sPos으로 이동하고, PosInfo를 iPos으로 셋팅한다. Backlash는 일단 차후로.
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="bMoveFlag"></param>
        /// <param name="bUseBacklash"></param>
        /// <returns></returns>
        public int MoveStagePos(CPos_XYTZ sPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null,bool bHighSpeed=false)
        {
            int iResult = SUCCESS;


            // Limit check ???

            // assume move all axis if bMoveFlag is null
            if(bMoveFlag == null)
            {
                // Stage는 Z축이 없다 (X,Y,T축)
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
            }

            // trans to array
            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // backlash
            if(bUseBacklash)
            {
                // 나중에 작업
            }

            // 1. move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                
            }

            // 2. move Z Axis
            if (bMoveFlag[DEF_Z] == true)
            {
                
            }

            string str = $"success : move Stage to pos:{sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        /// <summary>
        /// iPos 좌표로 선택된 축들을 이동시킨다.
        /// </summary>
        /// <param name="iPos">목표 위치</param>
        /// <param name="bUpdatedPosInfo">목표위치값을 update 할지의 여부</param>
        /// <param name="bMoveFlag">이동시킬 축 선택 </param>
        /// <param name="dMoveOffset">임시 옵셋값 </param>
        /// <param name="bUseBacklash"></param>
        /// <param name="bUsePriority">우선순위 이동시킬지 여부 </param>
        /// <param name="movePriority">우선순위 </param>
        /// <returns></returns>
        public int MoveStagePos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // Stage 구동 중인지 확인함.
            if (IsStageBusy()) return GenerateErrorCode(ERR_STAGE_MOVE_FAIL);

            // Load / Unload Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            if (iPos == (int)EStagePos.LOAD || iPos == (int)EStagePos.UNLOAD)
            {
                AxStageInfo.InitAlignOffset();
            }

            CPos_XYTZ sTargetPos = AxStageInfo.GetTargetPos(iPos);

            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            iResult = MoveStagePos(sTargetPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxStageInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }

        public int MoveStageRelative(CPos_XYTZ sPos,bool[] bMoveFlag = null)
        {
            int iResult = SUCCESS;

            // Stage 구동 중인지 확인함.
            if (IsStageBusy()) return GenerateErrorCode(ERR_STAGE_MOVE_FAIL);

            // 이동 Position 선택
            int iPos = (int)EStagePos.NONE;

            bool bUsePriority = false;
            bool bUseBacklash = false;
            int[] movePriority = null;

            // 현재 위치를 읽어옴 (Command 값을 사용하는 것이 좋을 듯)
            CPos_XYTZ sTargetPos;

            iResult = GetStageCmdPos(out sTargetPos);
            if (iResult != SUCCESS) GenerateErrorCode(ERR_STAGE_READ_CURRENT_POSITION);
            // Index 거리를 해당 축에 더하여 거리를 산출함.
            sTargetPos += sPos; 

            iResult = MoveStagePos(sTargetPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveStageRelativeXYT(CPos_XYTZ sPos)
        {
            int iResult = SUCCESS;
            // 이동 거리가 없을 경우 Success
            if (sPos.dX == 0.0 && sPos.dY == 0.0 && sPos.dT == 0.0 && sPos.dZ == 0.0) return SUCCESS;
            
            // 이동 Position 선택
            int iPos = (int)EStagePos.NONE;
            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };

            iResult = MoveStageRelative(sPos,bMoveFlag);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        public int MoveStageRelative(int iAxis, double dMoveLength,  bool bUseBacklash = false)
        {
            int iResult = SUCCESS;

            // Stage 구동 중인지 확인함.
            if(IsStageBusy()) return GenerateErrorCode(ERR_STAGE_MOVE_FAIL);

            // 이동 Position 선택
            CPos_XYTZ sTargetPos = new CPos_XYTZ();
            iResult = GetStageCmdPos(out sTargetPos);
            if (iResult != SUCCESS) return iResult;

            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, false };           

            if (iAxis == DEF_X)
            {
                bMoveFlag[DEF_X] = true;
                sTargetPos.dX += dMoveLength;
            }
            if (iAxis == DEF_Y)
            {
                bMoveFlag[DEF_Y] = true;
                sTargetPos.dY += dMoveLength;
            }
            if (iAxis == DEF_T)
            {
                bMoveFlag[DEF_T] = true;
                sTargetPos.dT += dMoveLength;
            }
            
            iResult = MoveStagePos(sTargetPos, bMoveFlag, bUseBacklash);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// Stage의 각축의 상대 이동
        /// </summary>
        /// <param name="dMoveLength"></param>
        /// <returns></returns>
        public int MoveStageRelativeX(double dMoveLength)
        {
            MoveStageRelative(DEF_X, dMoveLength);
            return SUCCESS;
        }
        public int MoveStageRelativeY(double dMoveLength)
        {
            MoveStageRelative(DEF_Y, dMoveLength);
            return SUCCESS;
        }
        public int MoveStageRelativeT(double dMoveLength)
        {
            MoveStageRelative(DEF_T, dMoveLength);
            return SUCCESS;
        }

        /// <summary>
        /// Stage Index Move : 지정된 Pitch거리로 이동함.
        /// 일반적으로 Die 사이즈의 거리만큼 이동함.
        /// </summary>
        /// <returns></returns>
        public int MoveStageIndexPlusX(bool bTurn=false)
        {
            int iResult = SUCCESS;
            double moveDistance;
            if (bTurn == false)
                moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexWidth;
            else
                moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexHeight;
            // + 방향으로 이동
            iResult= MoveStageRelativeX(moveDistance);

            return iResult;
        }

        public int MoveStageIndexPlusY(bool bTurn = false)
        {
            int iResult = SUCCESS;
            double moveDistance;
            if (bTurn == false)
                moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexHeight;
            else
                moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexWidth;
            // + 방향으로 이동
            iResult = MoveStageRelativeY(moveDistance);

            return iResult;
        }

        public int MoveStageIndexPlusT()
        {
            double moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexRotate;
            // + 방향으로 이동
            MoveStageRelativeT(moveDistance);

            return SUCCESS;
        }

        public int MoveStageIndexMinusX(bool bTurn=false)
        {
            int iResult = SUCCESS;
            double moveDistance;
            if (bTurn == false)
                moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexWidth;
            else
                moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexHeight;
            // + 방향으로 이동
            iResult = MoveStageRelativeX(-moveDistance);

            return iResult;
        }

        public int MoveStageIndexMinusY(bool bTurn=false)
        {
            int iResult = SUCCESS;
            double moveDistance;
            if (bTurn == false)
                moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexHeight;
            else
                moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexWidth;
            // + 방향으로 이동
            iResult = MoveStageRelativeY(-moveDistance);

            return iResult;
        }

        public int MoveStageIndexMinusT()
        {
            double moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexRotate;
            // - 방향으로 이동
            MoveStageRelativeT(-moveDistance);

            return SUCCESS;
        }

        /// <summary>
        /// Stage Screen Move :Vision의 FOV 만큼 Pitch 이동함.
        /// </summary>
        /// <returns></returns>
        public int MoveStageScreenPlusX(ECameraSelect eMode)
        {
            double moveDistance = 0.0;

            // + 방향으로 이동
            if (eMode == ECameraSelect.MACRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MacroScreenWidth;
                MoveStageRelativeX(moveDistance);
            }
            if (eMode == ECameraSelect.MICRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MicroScreenWidth;
                MoveStageRelativeX(moveDistance);
            }

            return SUCCESS;
        }

        public int MoveStageScreenPlusY(ECameraSelect eMode)
        {
            double moveDistance = 0.0;

            // + 방향으로 이동
            if (eMode == ECameraSelect.MACRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MacroScreenHeight;
                MoveStageRelativeY(moveDistance);
            }
            if (eMode == ECameraSelect.MICRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MicroScreenHeight;
                MoveStageRelativeY(moveDistance);
            }

            return SUCCESS;
        }

        public int MoveStageScreenPlusT(ECameraSelect eMode)
        {
            double moveDistance = 0.0;

            // + 방향으로 이동
            if (eMode == ECameraSelect.MACRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MacroScreenRotate;
                MoveStageRelativeT(moveDistance);
            }
            if (eMode == ECameraSelect.MICRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MicroScreenRotate;
                MoveStageRelativeT(moveDistance);
            }

            return SUCCESS;
        }

        public int MoveStageScreenMinusX(ECameraSelect eMode)
        {
            double moveDistance = 0.0;

            // - 방향으로 이동
            if (eMode == ECameraSelect.MACRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MacroScreenWidth;
                MoveStageRelativeX(-moveDistance);
            }
            if (eMode == ECameraSelect.MICRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MicroScreenWidth;
                MoveStageRelativeX(-moveDistance);
            }

            return SUCCESS;
        }

        public int MoveStageScreenMinusY(ECameraSelect eMode)
        {
            double moveDistance = 0.0;

            // - 방향으로 이동
            if (eMode == ECameraSelect.MACRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MacroScreenHeight;
                MoveStageRelativeY(-moveDistance);
            }
            if (eMode == ECameraSelect.MICRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MicroScreenHeight;
                MoveStageRelativeY(-moveDistance);
            }

            return SUCCESS;
        }

        public int MoveStageScreenMinusT(ECameraSelect eMode)
        {
            double moveDistance = 0.0;

            // - 방향으로 이동
            if (eMode == ECameraSelect.MACRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MacroScreenRotate;
                MoveStageRelativeT(-moveDistance);
            }
            if (eMode == ECameraSelect.MICRO)
            {
                moveDistance = CMainFrame.DataManager.SystemData_Align.MacroScreenRotate;
                MoveStageRelativeT(-moveDistance);
            }

            return SUCCESS;
        }
        /// <summary>
        /// Stage를 LOAD, UNLOAD등의 목표위치로 이동시킬때에 좀더 편하게 이동시킬수 있도록 간편화한 함수
        /// Z축만 움직일 경우엔 Position Info를 업데이트 하지 않는다. 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveAllAxis"></param>
        /// <param name="bMoveXYT"></param>
        /// <param name="bMoveZ"></param>
        /// <returns></returns>
        public int MoveStagePos(int iPos, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                return MoveStagePos(iPos);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveStagePos(iPos, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveStagePos(iPos, false, bMoveFlag);
            }

            return SUCCESS;
        }

        public int MoveStageToLoadPos(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.LOAD;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToUnloadPos(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.UNLOAD;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToWaitPos(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.WAIT;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }


        public int MoveStageToWaferCenterPre()
        {
            int index = (int)EStagePos.STAGE_CENTER_PRE;

            // Stage Center 위치를 읽어온다.
            var movePos = new CPos_XYTZ();   
            movePos = GetTargetPosition(index);

            // Theta Align 값 보정
            var ThetaPos = new CPos_XYTZ();
            GetThetaAlignPosA(out ThetaPos);
            movePos.dT = ThetaPos.dT;

            return MoveStagePos(movePos);
        }

        public int MoveStageToWaferCenterFine()
        {
            int index = (int)EStagePos.STAGE_CENTER_FINE;
            
            // Stage Center 위치를 읽어온다.
            var movePos = new CPos_XYTZ();
            movePos = GetTargetPosition(index);

            // Theta Align 값 보정
            var ThetaPos = new CPos_XYTZ();
            GetThetaAlignPosA(out ThetaPos);
            movePos.dT = ThetaPos.dT;

            return MoveStagePos(movePos);
        }
        public int MoveStageToStageCenterPre()
        {
            int index = (int)EStagePos.STAGE_CENTER_PRE;

            // Stage Center 위치를 읽어온다.
            var movePos = new CPos_XYTZ();
            movePos = GetTargetPosition(index, false);
            
            // Theta Align 값 보정
            var ThetaPos = new CPos_XYTZ();
            GetThetaAlignPosA(out ThetaPos);
            movePos.dT = ThetaPos.dT;

            return MoveStagePos(movePos);
        }

        public int MoveStageToStageCenterFine()
        {
            int index = (int)EStagePos.STAGE_CENTER_FINE;

            // Stage Center 위치를 읽어온다.
            var movePos = new CPos_XYTZ();            
            movePos = GetTargetPosition(index,false);

            // Theta Align 값 보정
            var ThetaPos = new CPos_XYTZ();
            GetThetaAlignPosA(out ThetaPos);
            movePos.dT = ThetaPos.dT;

            return MoveStagePos(movePos);
        }

        public int MoveStageToStageCenterInspect()
        {
            int index = (int)EStagePos.STAGE_CENTER_INSPECT;

            // Stage Center 위치를 읽어온다.
            var movePos = new CPos_XYTZ();
            movePos = GetTargetPosition(index, false);

            // Theta Align 값 보정
            var ThetaPos = new CPos_XYTZ();
            GetThetaAlignPosA(out ThetaPos);
            movePos.dT = ThetaPos.dT;

            return MoveStagePos(movePos);
        }

        public int MoveStageToEdgeAlignPos1(bool bHighMagnitude=false)
        {
            int iPosIndex = (int)EStagePos.EDGE_ALIGN_1;

            // Theta Align A pos를 저장
            CPos_XYTZ movePos = AxStageInfo.GetTargetPos(iPosIndex,false);

            // 고배율 일때는 Offset을 적용해서 이동한다.
            if (bHighMagnitude)
            {
                movePos.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                movePos.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
            }
            //  회전값을 적용함.
            movePos.dT = -90.0;

            CPos_XYTZ edgeAlignPos = new CPos_XYTZ();
            GetAlignData(out edgeAlignPos);

            // Wafer의 중심 Offset 적용 ( Offset의 회전 변환 적용)
            movePos.dX += Math.Cos(DegToRad(-movePos.dT)) * edgeAlignPos.dX -
                          Math.Sin(DegToRad(-movePos.dT)) * edgeAlignPos.dY;
            movePos.dY += Math.Sin(DegToRad(-movePos.dT)) * edgeAlignPos.dX +
                          Math.Cos(DegToRad(-movePos.dT)) * edgeAlignPos.dY;

            // Wafer Size 편차를 대입함
            movePos.dX -= CMainFrame.DataManager.SystemData_Align.WaferSizeOffset * Math.Cos(DegToRad(135));
            movePos.dY -= CMainFrame.DataManager.SystemData_Align.WaferSizeOffset * Math.Sin(DegToRad(135));

            return MoveStagePos(movePos, true);
        }

        public int MoveStageToEdgeAlignPos2(bool bHighMagnitude=false)
        {
            int iPosIndex = (int)EStagePos.EDGE_ALIGN_1;
            
            // Theta Align A pos를 저장
            CPos_XYTZ movePos = AxStageInfo.GetTargetPos(iPosIndex, false);
            
            // 고배율 일때는 Offset을 적용해서 이동한다.
            if (bHighMagnitude)
            {
                movePos.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                movePos.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
            }
            //  회전값을 적용함.
            movePos.dT = 0.0;

            CPos_XYTZ edgeAlignPos = new CPos_XYTZ();
            GetAlignData(out edgeAlignPos);

            // Wafer의 중심 Offset 적용 ( Offset의 회전 변환 적용)
            movePos.dX += Math.Cos(DegToRad(-movePos.dT)) * edgeAlignPos.dX -
                          Math.Sin(DegToRad(-movePos.dT)) * edgeAlignPos.dY;
            movePos.dY += Math.Sin(DegToRad(-movePos.dT)) * edgeAlignPos.dX +
                          Math.Cos(DegToRad(-movePos.dT)) * edgeAlignPos.dY;

            // Wafer Size 편차를 대입함
            movePos.dX -= CMainFrame.DataManager.SystemData_Align.WaferSizeOffset * Math.Cos(DegToRad(135));
            movePos.dY -= CMainFrame.DataManager.SystemData_Align.WaferSizeOffset * Math.Sin(DegToRad(135));

            return MoveStagePos(movePos,true);

        }

        public int MoveStageToEdgeAlignPos3(bool bHighMagnitude=false)
        {
            int iPosIndex = (int)EStagePos.EDGE_ALIGN_1;

            // Theta Align A pos를 저장
            CPos_XYTZ movePos = AxStageInfo.GetTargetPos(iPosIndex, false);
            
            // 고배율 일때는 Offset을 적용해서 이동한다.
            if (bHighMagnitude)
            {
                movePos.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                movePos.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
            }
            //  회전값을 적용함.
            movePos.dT = 90.0;

            CPos_XYTZ edgeAlignPos = new CPos_XYTZ();
            GetAlignData(out edgeAlignPos);

            // Wafer의 중심 Offset 적용 ( Offset의 회전 변환 적용)
            movePos.dX += Math.Cos(DegToRad(-movePos.dT)) * edgeAlignPos.dX -
                          Math.Sin(DegToRad(-movePos.dT)) * edgeAlignPos.dY;
            movePos.dY += Math.Sin(DegToRad(-movePos.dT)) * edgeAlignPos.dX +
                          Math.Cos(DegToRad(-movePos.dT)) * edgeAlignPos.dY;

            // Wafer Size 편차를 대입함
            movePos.dX -= CMainFrame.DataManager.SystemData_Align.WaferSizeOffset * Math.Cos(DegToRad(135));
            movePos.dY -= CMainFrame.DataManager.SystemData_Align.WaferSizeOffset * Math.Sin(DegToRad(135));

            return MoveStagePos(movePos,true);
        }

        public int MoveStageToEdgeAlignPos4(bool bHighMagnitude=false)
        {
            int iPosIndex = (int)EStagePos.EDGE_ALIGN_1;

            // Theta Align A pos를 저장
            CPos_XYTZ movePos = AxStageInfo.GetTargetPos(iPosIndex, false);

            // 고배율 일때는 Offset을 적용해서 이동한다.
            if (bHighMagnitude)
            {
                movePos.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                movePos.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
            }
            //  회전값을 적용함.
            movePos.dT = 180.0;

            CPos_XYTZ edgeAlignPos = new CPos_XYTZ();
            GetAlignData(out edgeAlignPos);

            // Wafer의 중심 Offset 적용 ( Offset의 회전 변환 적용)
            movePos.dX += Math.Cos(DegToRad(-movePos.dT)) * edgeAlignPos.dX -
                          Math.Sin(DegToRad(-movePos.dT)) * edgeAlignPos.dY;
            movePos.dY += Math.Sin(DegToRad(-movePos.dT)) * edgeAlignPos.dX +
                          Math.Cos(DegToRad(-movePos.dT)) * edgeAlignPos.dY;

            // Wafer Size 편차를 대입함
            movePos.dX -= CMainFrame.DataManager.SystemData_Align.WaferSizeOffset * Math.Cos(DegToRad(135));
            movePos.dY -= CMainFrame.DataManager.SystemData_Align.WaferSizeOffset * Math.Sin(DegToRad(135));

            return MoveStagePos(movePos, true);

        }

        public int MoveStageToMacroCam(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.MACRO_CAM_POS;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }
        public int MoveStageToMacroTeachA(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            // Stage Center 위치 읽음
            int iPosIndex = (int)EStagePos.STAGE_CENTER_PRE;
            CPos_XYTZ movePos = AxStageInfo.GetTargetPos(iPosIndex);

            // Center에서 수평이동 값을 계산
            double moveDistance = CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen / 2;

            movePos.dX -= moveDistance;

            // Theta Align 값 (현재 위치의 Theta값 적용)
            var ThetaPos = new CPos_XYTZ();
            GetStageCmdPos(out ThetaPos);
            
            movePos.dT = ThetaPos.dT;

            return MoveStagePos(movePos, true);
        }

        public int MoveStageToMacroTeachB(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            // Teach 1 위치를 계산
            // Stage Center 위치 읽음
            int iPosIndex = (int)EStagePos.STAGE_CENTER_PRE;
            CPos_XYTZ movePos = AxStageInfo.GetTargetPos(iPosIndex);

            // Center에서 수평이동 값을 계산
            double moveDistance = CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen / 2;

            movePos.dX -= moveDistance;

            // 떨어져 있는 Mark 2 위치를 Die Pitch 간격으로 계산함.
            // 수평 이동 값을 적용한다.            
            int iPichNum = (int)(CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen / CMainFrame.DataManager.SystemData_Align.DieIndexWidth);

            moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexWidth * iPichNum;

            movePos.dX += moveDistance;

            // Theta Align 값 보정
            var ThetaPos = new CPos_XYTZ();
            GetStageCmdPos(out ThetaPos);
            movePos.dT = ThetaPos.dT;

            return MoveStagePos(movePos, true);
        }

        public int MoveStageToMacroAlignA(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.MACRO_ALIGN;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToMacroAlignB(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            // Macro Mark 1위치 읽음
            int iPosIndex = (int)EStagePos.MACRO_ALIGN;            
            CPos_XYTZ movePos = AxStageInfo.GetTargetPos(iPosIndex);

            // 떨어져 있는 Mark 2 위치를 Die Pitch 간격으로 계산함.
            // 수평 이동 값을 적용한다.            
            int iPichNum = (int)(CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen / CMainFrame.DataManager.SystemData_Align.DieIndexWidth);

            double moveDistance = CMainFrame.DataManager.SystemData_Align.DieIndexWidth * iPichNum;

            movePos.dX += moveDistance;

            // Theta Align 값 (현재 위치의 Theta값 적용)
            var ThetaPos = new CPos_XYTZ();
            GetThetaAlignPosA(out ThetaPos);
            movePos.dT = ThetaPos.dT;

            return MoveStagePos(movePos, true);
        }
        

        public int MoveStageToMicroAlignA(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.MICRO_ALIGN;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }
        

        public int MoveStageToMicroAlignTurnA(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.MICRO_ALIGN_TURN;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }
        

        public int MoveStageToProcessPos(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.LASER_PROCESS;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToProcessTurnPos(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.LASER_PROCESS_TURN;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveChangeMicroCam()
        {
            int iResult = -1;

            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, false, false };
            CPos_XYTZ MoveDistance = new CPos_XYTZ();
            
            // 수평으로 Align Mark 거리 만큼 이동함.
            MoveDistance.dX = CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
            MoveDistance.dY = CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;

            iResult = MoveStageRelative(MoveDistance,bMoveFlag);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveChangeMacroCam()
        {
            int iResult = -1;

            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, false, false };
            CPos_XYTZ MoveDistance = new CPos_XYTZ();
            
            // 수평으로 Align Mark 거리 만큼 이동함.
            MoveDistance.dX = -CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
            MoveDistance.dY = -CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;

            iResult = MoveStageRelative(MoveDistance, bMoveFlag);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }


        public int JogStageStop(int iAxis)
        {
            int selectAxis = 0;

            if (iAxis == (int)EACS_Axis.STAGE1_X) selectAxis = DEF_X;
            if (iAxis == (int)EACS_Axis.STAGE1_Y) selectAxis = DEF_Y;
            if (iAxis == (int)EACS_Axis.STAGE1_T) selectAxis = DEF_T;

            return 0;
        }
        
#endregion

        // Camera Servo 구동
#region Camera Move 동작

        public int GetCameraCurPos(out CPos_XYTZ pos)
        {
            int iResult =0;
            CPos_XYTZ temp = new CPos_XYTZ();
            pos = temp;
            return iResult;
        }
        
        
        
        /// <summary>
        /// Stage의 각축의 상대 이동
        /// </summary>
        /// <param name="dMoveLength"></param>
        /// <returns></returns>

        public int MoveCameraRelative(double dMoveLength)
        {
            MoveStageRelative(DEF_Z, dMoveLength);
            return SUCCESS;
        }

        /// <summary>
        /// Camera를 LOAD, UNLOAD등의 목표위치로 이동시킬때에 좀더 편하게 이동시킬수 있도록 간편화한 함수
        /// Z축만 움직일 경우엔 Position Info를 업데이트 하지 않는다. 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveAllAxis"></param>
        /// <param name="bMoveXYT"></param>
        /// <param name="bMoveZ"></param>
        /// <returns></returns>



#endregion
        

        // 모드 변경 및 Align Data Set
#region Control Mode & Align Data Set
        
        public int SetAlignData(CPos_XYTZ offset)
        {
            //int iResult = SUCCESS;
            var curPos = new CPos_XYTZ();

            // 현재 Align Offset 값을 읽어옴
            AxStageInfo.GetAlignOffset(out curPos);
            // AlignData 적용
            curPos += offset;

            // AlignOffet 적용
            AxStageInfo.SetAlignOffset(offset);

            return SUCCESS;
        }

        public int GetAlignData(out CPos_XYTZ alignData)
        {
            //int iResult = SUCCESS;
            var offset = new CPos_XYTZ();

            // AlignOffet 적용
            AxStageInfo.GetAlignOffset(out offset);

            alignData = offset;

            return SUCCESS;
        }

        public void SetAlignDataInit()
        {
            AxStageInfo.InitAlignOffset();
        }
                
        public void SetThetaAlignPosA(CPos_XYTZ pPos)
        {
            var offset = new CPos_XYTZ();
            GetAlignData(out offset);

            AxStageInfo.Pos_Fixed.Pos[(int)EStagePos.STAGE_CENTER_INSPECT] = pPos - offset;

            //CMainFrame.DataManager.Pos_Fixed.Pos_Stage1.Pos[(int)EStagePos.STAGE_CENTER_INSPECT] = AxStageInfo.Pos_Fixed.Pos[(int)EStagePos.STAGE_CENTER_INSPECT];
        }
        public int GetThetaAlignPosA(out CPos_XYTZ pPos)
        {
            int index = (int)EStagePos.STAGE_CENTER_INSPECT;
            pPos =  GetTargetPosition(index);

            return SUCCESS;
        }

        public void SetThetaAlignTurnPosA(CPos_XYTZ pPos)
        {
            var offset = new CPos_XYTZ();
            GetAlignData(out offset);

            AxStageInfo.Pos_Fixed.Pos[(int)EStagePos.THETA_ALIGN_TURN_A] = pPos - offset;

            CMainFrame.DataManager.Pos_Fixed.Pos_Stage1.Pos[(int)EStagePos.THETA_ALIGN_TURN_A] = AxStageInfo.Pos_Fixed.Pos[(int)EStagePos.THETA_ALIGN_TURN_A];
        }
        public int GetThetaAlignTurnPosA(out CPos_XYTZ pPos)
        {
            int index = (int)EStagePos.THETA_ALIGN_TURN_A;
            pPos = GetTargetPosition(index);

            return SUCCESS;
        }

        public int SetMicroCamPos(CPos_XYTZ pPos)
        {
            int index = (int)EStagePos.MACRO_CAM_POS;
            AxStageInfo.Pos_Fixed.Pos[index] = pPos;

            return SUCCESS;
        }

        public int GetMicroCamPos(out CPos_XYTZ pPos)
        {
            int index = (int)EStagePos.MACRO_CAM_POS;
            pPos = AxStageInfo.Pos_Fixed.Pos[index];

            return SUCCESS;
        }
        
        public double GetScreenIndexTheta()
        {
            double moveDistance = CMainFrame.DataManager.SystemData_Align.MicroScreenRotate;
            return moveDistance;
        }
#endregion
        
        

        // Interlock 조건 확인
#region Interlock 확인
        public int IsObjectDetected(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InDetectObject, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }


        public int GetStageAxZone(int axis, out int curZone)
        {
            bool bStatus;
            curZone = (int)EStagePos.NONE;
            for (int i = 0; i < (int)EStagePos.MAX; i++)
            {
                if (m_Data.StageZone.Axis[axis].ZoneAddr[i] == -1) continue; // if io is not allocated, continue;
                int iResult = m_RefComp.IO.IsOn(m_Data.StageZone.Axis[axis].ZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int IsStageAxisInSafetyZone(out bool bStatus)
        {
            bStatus = false;
            int curZone;
            int iResult = SUCCESS;

            // X축 확인
            iResult = GetStageAxZone(DEF_X, out curZone);
            if (iResult != SUCCESS) return iResult;
            if(curZone != (int)EStageXAxZone.SAFETY) return SUCCESS;
         
            // Y축 확인
            iResult = GetStageAxZone(DEF_Y, out curZone);
            if (iResult != SUCCESS) return iResult;
            if (curZone != (int)EStageYAxZone.SAFETY) return SUCCESS;
            // T축 확인
            iResult = GetStageAxZone(DEF_T, out curZone);
            if (iResult != SUCCESS) return iResult;
            if (curZone != (int)EStageTAxZone.SAFETY) return SUCCESS;

            bStatus = true;
            return SUCCESS;
        }
        

        public int CheckForStageCylMove()
        {

            // 조건없이 True

            //// check object
            //int iResult = IsObjectDetected(out bStatus);
            //if (iResult != SUCCESS) return iResult;
            //if (bStatus)
            //{
            //    IsAbsorbed(out bStatus);
            //    if (iResult != SUCCESS) return iResult;
            //    if (bStatus == false)
            //    {
            //        return GenerateErrorCode(ERR_STAGE_OBJECT_DETECTED_BUT_NOT_ABSORBED);
            //    }
            //}
            //else
            //{
            //    IsReleased(out bStatus);
            //    if (iResult != SUCCESS) return iResult;
            //    if (bStatus == false)
            //    {
            //        return GenerateErrorCode(ERR_STAGE_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED);
            //    }
            //}

            return SUCCESS;
        }

        #endregion

        private double RadToDeg(double pRad)
        {
            double mDeg = pRad * 180 / Math.PI;
            return mDeg;
        }

        private double DegToRad(double pDeg)
        {
            double mRad = pDeg * Math.PI / 180;
            return mRad;
        }

    }
}
