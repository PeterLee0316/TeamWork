using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_IO;
using static LWDicer.Control.DEF_Vacuum;
using static LWDicer.Control.DEF_MeSpinner;
using static LWDicer.Control.DEF_DataManager;

namespace LWDicer.Control
{
    public class DEF_MeSpinner
    {
        public const int ERR_SPINNER_UNABLE_TO_USE_IO = 1;
        public const int ERR_SPINNER_UNABLE_TO_USE_CYL = 2;
        public const int ERR_SPINNER_UNABLE_TO_USE_VCC = 3;
        public const int ERR_SPINNER_UNABLE_TO_USE_AXIS = 4;
        public const int ERR_SPINNER_UNABLE_TO_USE_VISION = 5;
        public const int ERR_SPINNER_NOT_ORIGIN_RETURNED = 6;
        public const int ERR_SPINNER_INVALID_AXIS = 7;
        public const int ERR_SPINNER_INVALID_PRIORITY = 8;
        public const int ERR_SPINNER_NOT_SAME_POSITION = 9;
        public const int ERR_SPINNER_LCD_VCC_ABNORMALITY = 10;
        public const int ERR_SPINNER_VACUUM_ON_TIME_OUT = 11;
        public const int ERR_SPINNER_VACUUM_OFF_TIME_OUT = 12;
        public const int ERR_SPINNER_INVALID_PARAMETER = 13;
        public const int ERR_SPINNER_OBJECT_DETECTED_BUT_NOT_ABSORBED = 14;
        public const int ERR_SPINNER_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED = 15;
        public const int ERR_SPINNER_TABLE_UP_INTERLOCK = 16;
        public const int ERR_SPINNER_CLEANER_NOZZLE_NOT_SAFETY_POS = 17;
        public const int ERR_SPINNER_COATER_NOZZLE_NOT_SAFETY_POS = 18;

        public enum ESpinnerType
        {
            NONE = -1,
            AXIS = 0,
            CYL,
        }

        public enum ENozzlePos
        {
            NONE = -1,
            SAFETY = 0,
            START,
            END,
            MAX,
        }

        public enum ENozzleAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum ERotatePos
        {
            NONE = -1,
            LOAD,
            MAX,
        }

        public enum EChuckVacuum
        {
            SELF,           // 자체 발생 진공
            //FACTORY,        // 공장 진공
            //OBJECT,         // LCD 패널의 PCB같은 걸 집는 용도
            //EXTRA_SELF,     // 
            //EXTRA_FACTORY,  //
            //EXTRA_OBJECT,   //
            MAX,
        }

        public class CMeSpinnerRefComp
        {
            public IIO IO;

            // Cylinder
            public ICylinder ChuckTableUDCyl;
            public ICylinder CleanNozzleSolCyl;
            public ICylinder CoatNozzleSolCyl;

            public MMultiAxes_YMC AxSpinRotate;
            public MMultiAxes_YMC AxSpinCleanNozzle;
            public MMultiAxes_YMC AxSpinCoatNozzle;

            // Vacuum
            public IVacuum[] Vacuum = new IVacuum[(int)EChuckVacuum.MAX];
        }


        public class CMeSpinnerData
        {
            // Coater Type
            public ESpinnerType[] SpinnerType = new ESpinnerType[DEF_MAX_COORDINATE];

            public int InDetectObject;

            public int OutRingBlow;


            public CMeSpinnerData(ESpinnerType[] SpinnerType = null)
            {
                if (SpinnerType == null)
                {
                    for (int i = 0; i < this.SpinnerType.Length; i++)
                    {
                        this.SpinnerType[i] = ESpinnerType.NONE;
                    }
                }
                else
                {
                    Array.Copy(SpinnerType, this.SpinnerType, SpinnerType.Length);
                }

            }
        }
    }

    public class MMeSpinner : MMechanicalLayer
    {
        private CMeSpinnerRefComp m_RefComp;
        private CMeSpinnerData m_Data;

        private CMovingObject AxRotateInfo = new CMovingObject((int)ERotatePos.MAX);
        private CMovingObject AxCleanNozzleInfo = new CMovingObject((int)ENozzlePos.MAX);
        private CMovingObject AxCoatNozzleInfo = new CMovingObject((int)ENozzlePos.MAX);

        // Vacuum
        private bool[] UseVccFlag = new bool[(int)EChuckVacuum.MAX];

        MTickTimer m_waitTimer = new MTickTimer();

        public MMeSpinner(CObjectInfo objInfo, CMeSpinnerRefComp refComp, CMeSpinnerData data)
            : base(objInfo)
        {
            m_RefComp = refComp;

            SetData(data);

            for (int i = 0; i < UseVccFlag.Length; i++)
            {
                UseVccFlag[i] = false;
            }

        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CMeSpinnerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CMeSpinnerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetRotatePosition(CUnitPos FixedPos, CUnitPos ModelPos, CUnitPos OffsetPos)
        {
            AxRotateInfo.SetPosition(FixedPos, ModelPos, OffsetPos);
            return SUCCESS;
        }

        public int SetCleanPosition(CUnitPos FixedPos, CUnitPos ModelPos, CUnitPos OffsetPos)
        {
            AxCleanNozzleInfo.SetPosition(FixedPos, ModelPos, OffsetPos);
            return SUCCESS;
        }

        public int SetCoatPosition(CUnitPos FixedPos, CUnitPos ModelPos, CUnitPos OffsetPos)
        {
            AxCoatNozzleInfo.SetPosition(FixedPos, ModelPos, OffsetPos);
            return SUCCESS;
        }

        public int SetVccUseFlag(bool[] UseVccFlag = null)
        {
            if (UseVccFlag != null)
            {
                Array.Copy(UseVccFlag, this.UseVccFlag, UseVccFlag.Length);
            }
            return SUCCESS;
        }
        #endregion

        #region Cylinder, Vacuum, Detect Object
        public int Absorb(bool bSkipSensor)
        {
            bool bStatus;
            int iResult = SUCCESS;
            bool[] bWaitFlag = new bool[(int)EChuckVacuum.MAX];
            CVacuumTime[] sData = new CVacuumTime[(int)EChuckVacuum.MAX];
            bool bNeedWait = false;

            for (int i = 0; i < (int)EChuckVacuum.MAX; i++)
            {
                if (UseVccFlag[i] == false) continue;

                m_RefComp.Vacuum[i].GetVacuumTime(out sData[i]);
                iResult = m_RefComp.Vacuum[i].IsOn(out bStatus);
                if (iResult != SUCCESS) return iResult;

                // 흡착되지 않은 상태라면 흡착시킴  
                if (bStatus == false)
                {
                    iResult = m_RefComp.Vacuum[i].On(true);
                    if (iResult != SUCCESS) return iResult;

                    bWaitFlag[i] = true;
                    bNeedWait = true;
                }

                Sleep(10);
            }

            if (bSkipSensor == true) return SUCCESS;

            m_waitTimer.StartTimer();
            while (bNeedWait)
            {
                bNeedWait = false;

                for (int i = 0; i < (int)EChuckVacuum.MAX; i++)
                {
                    if (bWaitFlag[i] == false) continue;

                    iResult = m_RefComp.Vacuum[i].IsOn(out bStatus);
                    if (iResult != SUCCESS) return iResult;

                    if (bStatus == true) // if on
                    {
                        bWaitFlag[i] = false;
                        //Sleep(sData[i].OnSettlingTime * 1000);
                    }
                    else // if off
                    {
                        bNeedWait = true;
                        if (m_waitTimer.MoreThan(sData[i].TurningTime * 1000))
                        {
                            return GenerateErrorCode(ERR_SPINNER_VACUUM_ON_TIME_OUT);
                        }
                    }

                }
            }

            return SUCCESS;
        }

        public int Release(bool bSkipSensor)
        {
            bool bStatus;
            int iResult = SUCCESS;
            bool[] bWaitFlag = new bool[(int)EChuckVacuum.MAX];
            CVacuumTime[] sData = new CVacuumTime[(int)EChuckVacuum.MAX];
            bool bNeedWait = false;

            for (int i = 0; i < (int)EChuckVacuum.MAX; i++)
            {
                if (UseVccFlag[i] == false) continue;

                m_RefComp.Vacuum[i].GetVacuumTime(out sData[i]);
                iResult = m_RefComp.Vacuum[i].IsOff(out bStatus);
                if (iResult != SUCCESS) return iResult;

                if (bStatus == false)
                {
                    iResult = m_RefComp.Vacuum[i].Off(true);
                    if (iResult != SUCCESS) return iResult;

                    bWaitFlag[i] = true;
                    bNeedWait = true;
                }

                Sleep(10);
            }

            if (bSkipSensor == true) return SUCCESS;

            m_waitTimer.StartTimer();
            while (bNeedWait)
            {
                bNeedWait = false;

                for (int i = 0; i < (int)EChuckVacuum.MAX; i++)
                {
                    if (bWaitFlag[i] == false) continue;

                    iResult = m_RefComp.Vacuum[i].IsOff(out bStatus);
                    if (iResult != SUCCESS) return iResult;

                    if (bStatus == true) // if on
                    {
                        bWaitFlag[i] = false;
                        //Sleep(sData[i].OffSettlingTime * 1000);
                    }
                    else // if off
                    {
                        bNeedWait = true;
                        if (m_waitTimer.MoreThan(sData[i].TurningTime * 1000))
                        {
                            return GenerateErrorCode(ERR_SPINNER_VACUUM_OFF_TIME_OUT);
                        }
                    }

                }
            }

            return SUCCESS;
        }

        public int IsAbsorbed(out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;
            bool bTemp;

            for (int i = 0; i < (int)EChuckVacuum.MAX; i++)
            {
                if (UseVccFlag[i] == false) continue;

                iResult = m_RefComp.Vacuum[i].IsOn(out bTemp);
                if (iResult != SUCCESS) return iResult;

                if (bTemp == false) return SUCCESS;
            }

            bStatus = true;
            return SUCCESS;
        }

        public int IsReleased(out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;
            bool bTemp;

            for (int i = 0; i < (int)EChuckVacuum.MAX; i++)
            {
                if (UseVccFlag[i] == false) continue;

                iResult = m_RefComp.Vacuum[i].IsOff(out bTemp);
                if (iResult != SUCCESS) return iResult;

                if (bTemp == false) return SUCCESS;
            }

            bStatus = true;
            return SUCCESS;
        }
        public int ChuckTableUp(bool bSkipSensor = false)
        {
            if (CheckForCleanNozzleSafety() != SUCCESS)
            {
                WriteLog("fail : Cleaner Nozzle Not Safety Position", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_SPINNER_CLEANER_NOZZLE_NOT_SAFETY_POS);
            }

            if (CheckForCoatNozzleSafety() != SUCCESS)
            {
                WriteLog("fail : Coater Nozzle Not Safety Position", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_SPINNER_COATER_NOZZLE_NOT_SAFETY_POS);
            }

            int iResult = m_RefComp.ChuckTableUDCyl.Up(bSkipSensor);
            return iResult;
        }


        public int ChuckTableDown(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.ChuckTableUDCyl.Down(bSkipSensor);
            return iResult;
        }

        public int IsChuckTableUp(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.ChuckTableUDCyl.IsUp(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsChuckTableDown(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.ChuckTableUDCyl.IsUp(out bStatus);
            if (bStatus == false) return SUCCESS;

            return SUCCESS;
        }

        public int CleanNozzleValveOpen(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.CleanNozzleSolCyl.Open(bSkipSensor);
            return iResult;
        }

        public int CleanNozzleValveClose(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.CleanNozzleSolCyl.Close(bSkipSensor);
            return iResult;
        }
        public int IsCleanNozzleValveOpen(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.CleanNozzleSolCyl.IsOpen(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCleanNozzleValveClose(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.CleanNozzleSolCyl.IsClose(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int CoatNozzleValveOpen(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.CoatNozzleSolCyl.Open(bSkipSensor);
            return iResult;
        }

        public int CoatNozzleValveClose(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.CoatNozzleSolCyl.Close(bSkipSensor);
            return iResult;
        }

        public int IsCoatNozzleValveOpen(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.CoatNozzleSolCyl.IsOpen(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCoatNozzleValveClose(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.CoatNozzleSolCyl.IsClose(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public void RingBlowOn()
        {
            m_RefComp.IO.OutputOn(m_Data.OutRingBlow);
        }

        public void RingBlowOff()
        {
            m_RefComp.IO.OutputOff(m_Data.OutRingBlow);
        }

        public int IsRingBlowOn(out bool bStatus)
        {
            m_RefComp.IO.IsOn(m_Data.OutRingBlow, out bStatus);

            return SUCCESS;
        }

        public int IsRingBlowOff(out bool bStatus)
        {
            m_RefComp.IO.IsOff(m_Data.OutRingBlow, out bStatus);

            return SUCCESS;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InDetectObject, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        #endregion

        #region Axis Move, Check Interlock
        public int GetRotateCurPos(out CPos_XYTZ pos)
        {
            int iResult = m_RefComp.AxSpinRotate.GetCurPos(out pos);
            return iResult;
        }

        public int GetCleanNozzleCurPos(out CPos_XYTZ pos)
        {
            int iResult = m_RefComp.AxSpinCleanNozzle.GetCurPos(out pos);
            return iResult;
        }

        public int GetCoatNozzleCurPos(out CPos_XYTZ pos)
        {
            int iResult = m_RefComp.AxSpinCoatNozzle.GetCurPos(out pos);
            return iResult;
        }


        public int RotateStop()
        {
            return m_RefComp.AxSpinRotate.SetStop(DEF_MAX_COORDINATE, DEF_STOP);
        }

        public int StartRotateCW(int nSpeed)
        {
            return m_RefComp.AxSpinRotate.JogMoveVelocity(DEF_MAX_COORDINATE, true, nSpeed);
        }

        public int StartRotateCCW(int nSpeed)
        {
            return m_RefComp.AxSpinRotate.JogMoveVelocity(DEF_MAX_COORDINATE, false, nSpeed);
        }

        public int MoveRotateToLoadPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ERotatePos.LOAD;

            return MoveRotatePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveCleanNozzleToSafetyPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.SAFETY;

            return MoveCleanNozzlePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveCleanNozzleToStartPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.START;
            bool bStatus = false;

            // check chuck table
            int iResult = CheckChuckTableSafetyForNozzleMoving();
            if (iResult != SUCCESS) return iResult;

            // check other nozzle

            return MoveCleanNozzlePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveCleanNozzleToEndPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.END;
            bool bStatus = false;

            // Chuck Table Up Interlock
            if (IsChuckTableUp(out bStatus) == SUCCESS)
            {
                WriteLog("fail : ChuckTable Cylinder Up", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_SPINNER_TABLE_UP_INTERLOCK);
            }

            return MoveCleanNozzlePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveRotatePos(CPos_XYTZ sPos, int iPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            if (bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, true };
            }

            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                // set priority
                if (bUsePriority == true && movePriority != null)
                {
                    m_RefComp.AxSpinCleanNozzle.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxSpinCleanNozzle.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Nozzle x y t axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }


            // set working pos
            if (iPos > (int)ENozzlePos.NONE)
            {
                AxCleanNozzleInfo.PosInfo = iPos;
            }

            string str = $"success : move Nozzle to pos:{iPos} {sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.Normal);

            return SUCCESS;
        }

        public int MoveRotatePos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos = AxCleanNozzleInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            iResult = MoveCleanNozzlePos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxRotateInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }

        public int MoveRotatePos(int iPos, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                return MoveCleanNozzlePos(iPos);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveCleanNozzlePos(iPos, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveCleanNozzlePos(iPos, false, bMoveFlag);
            }

            return SUCCESS;
        }

        public int MoveCleanNozzlePos(CPos_XYTZ sPos, int iPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            if (bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, true };
            }

            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                // set priority
                if (bUsePriority == true && movePriority != null)
                {
                    m_RefComp.AxSpinCleanNozzle.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxSpinCleanNozzle.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Nozzle x y t axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }


            // set working pos
            if (iPos > (int)ENozzlePos.NONE)
            {
                AxCleanNozzleInfo.PosInfo = iPos;
            }

            string str = $"success : move Nozzle to pos:{iPos} {sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.Normal);

            return SUCCESS;
        }

        public int MoveCleanNozzlePos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos = AxCleanNozzleInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            iResult = MoveCleanNozzlePos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxCleanNozzleInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }

        public int MoveCleanNozzlePos(int iPos, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                return MoveCleanNozzlePos(iPos);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveCleanNozzlePos(iPos, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveCleanNozzlePos(iPos, false, bMoveFlag);
            }

            return SUCCESS;
        }

        public int MoveCoatNozzletoSafetyPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.SAFETY;

            return MoveCoatNozzlePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveCoatNozzleToStartPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.START;

            return MoveCoatNozzlePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveCoatNozzleToEndPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.END;

            return MoveCoatNozzlePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveCoatNozzlePos(CPos_XYTZ sPos, int iPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            if (bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, true };
            }

            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                // set priority
                if (bUsePriority == true && movePriority != null)
                {
                    m_RefComp.AxSpinCoatNozzle.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxSpinCoatNozzle.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Nozzle x y t axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }


            // set working pos
            if (iPos > (int)ENozzlePos.NONE)
            {
                AxCoatNozzleInfo.PosInfo = iPos;
            }

            string str = $"success : move Nozzle to pos:{iPos} {sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.Normal);

            return SUCCESS;
        }

        public int MoveCoatNozzlePos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos = AxCoatNozzleInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            iResult = MoveCoatNozzlePos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxCoatNozzleInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }

        public int MoveCoatNozzlePos(int iPos, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                return MoveCoatNozzlePos(iPos);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveCoatNozzlePos(iPos, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveCoatNozzlePos(iPos, false, bMoveFlag);
            }

            return SUCCESS;
        }


        public int CheckForSpinnerCylMove(bool bCheckVacuum = true)
        {
            bool bStatus;

            // check object
            int iResult = IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                IsAbsorbed(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false)
                {
                    return GenerateErrorCode(ERR_SPINNER_OBJECT_DETECTED_BUT_NOT_ABSORBED);
                }
            }
            else
            {
                IsReleased(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false)
                {
                    return GenerateErrorCode(ERR_SPINNER_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED);
                }
            }

            return SUCCESS;
        }


        public int CheckForCleanNozzleSafety()
        {
            int iResult = SUCCESS;

            CPos_XYTZ targetPos = AxCleanNozzleInfo.GetTargetPos((int)ENozzlePos.SAFETY);
            if (iResult != SUCCESS) return iResult;

            iResult = CompareCleanNozzlePos(targetPos);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int CompareRotatePos(CPos_XYTZ sPos)
        {
            int iResult = SUCCESS;

            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = m_RefComp.AxSpinRotate.ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }


        public int CompareCleanNozzlePos(CPos_XYTZ sPos)
        {
            int iResult = SUCCESS;

            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = m_RefComp.AxSpinCleanNozzle.ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int CheckForRotateLoad()
        {
            int iResult = SUCCESS;

            CPos_XYTZ targetPos = AxRotateInfo.GetTargetPos((int)ERotatePos.LOAD);

            if (iResult != SUCCESS) return iResult;

            iResult = CompareRotatePos(targetPos);

            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }


        public int CheckForCoatNozzleSafety()
        {
            int iResult = SUCCESS;

            CPos_XYTZ targetPos = AxCoatNozzleInfo.GetTargetPos((int)ENozzlePos.SAFETY);
            if (iResult != SUCCESS) return iResult;

            iResult = CompareCoatNozzlePos(targetPos);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int CompareCoatNozzlePos(CPos_XYTZ sPos)
        {
            int iResult = SUCCESS;

            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = m_RefComp.AxSpinCoatNozzle.ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        int CheckChuckTableSafetyForNozzleMoving()
        {
            bool bStatus;
            int iResult = IsChuckTableDown(out bStatus);
            if (iResult != SUCCESS) return iResult;

            if (bStatus == false)
            {
                WriteLog("fail : ChuckTable Cylinder Up", ELogType.Debug, ELogWType.Error);
                return GenerateErrorCode(ERR_SPINNER_TABLE_UP_INTERLOCK);
            }

            return SUCCESS;
        }

        #endregion
    }
}
