using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Motion;
using static LWDicer.Layers.DEF_IO;
using static LWDicer.Layers.DEF_Vacuum;
using static LWDicer.Layers.DEF_MeSpinner;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.Layers
{
    public class DEF_MeSpinner
    {
        public const int ERR_SPINNER_UNABLE_TO_USE_IO                     = 1;
        public const int ERR_SPINNER_UNABLE_TO_USE_CYL                    = 2;
        public const int ERR_SPINNER_UNABLE_TO_USE_VCC                    = 3;
        public const int ERR_SPINNER_UNABLE_TO_USE_AXIS                   = 4;
        public const int ERR_SPINNER_UNABLE_TO_USE_VISION                 = 5;
        public const int ERR_SPINNER_NOT_ORIGIN_RETURNED                  = 6;
        public const int ERR_SPINNER_INVALID_AXIS                         = 7;
        public const int ERR_SPINNER_INVALID_PRIORITY                     = 8;
        public const int ERR_SPINNER_NOT_SAME_POSITION                    = 9;
        public const int ERR_SPINNER_LCD_VCC_ABNORMALITY                  = 10;
        public const int ERR_SPINNER_VACUUM_ON_TIME_OUT                   = 11;
        public const int ERR_SPINNER_VACUUM_OFF_TIME_OUT                  = 12;
        public const int ERR_SPINNER_INVALID_PARAMETER                    = 13;
        public const int ERR_SPINNER_OBJECT_DETECTED_BUT_NOT_ABSORBED     = 14;
        public const int ERR_SPINNER_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED = 15;
        public const int ERR_SPINNER_CHUCKTABLE_NOT_DOWN                  = 16;
        public const int ERR_SPINNER_CLEAN_NOZZLE_NOT_IN_SAFETY_ZONE      = 17;
        public const int ERR_SPINNER_COAT_NOZZLE_NOT_IN_SAFETY_ZONE       = 18;

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

            public int InRotateLoadPos; // 회전체이기때문에 pos 값보다는 sensor를 이용하면 한바퀴안에 LoadPos으로 이동 가능하기때문에

            // IO Address for manual check axis zone
            public CMAxisZoneCheck CleanNozzleZoneCheck = new CMAxisZoneCheck((int)ENozzleAxZone.MAX, (int)ENozzleAxZone.MAX,
            (int)ENozzleAxZone.MAX, (int)ENozzleAxZone.MAX);
            public CMAxisZoneCheck CoatNozzleZoneCheck = new CMAxisZoneCheck((int)ENozzleAxZone.MAX, (int)ENozzleAxZone.MAX,
            (int)ENozzleAxZone.MAX, (int)ENozzleAxZone.MAX);

            // Safety Position
            public CPos_XYTZ CleanNozzleSafetyPos;
            public CPos_XYTZ CoatNozzleSafetyPos;

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

        public CMovingObject AxRotateInfo { get; private set; } = new CMovingObject((int)ERotatePos.MAX);
        public CMovingObject AxCleanNozzleInfo { get; private set; } = new CMovingObject((int)ENozzlePos.MAX);
        public CMovingObject AxCoatNozzleInfo { get; private set; } = new CMovingObject((int)ENozzlePos.MAX);

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

        public int SetPosition_Rotate(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            AxRotateInfo.SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
            return SUCCESS;
        }

        public int SetPosition_CleanNozzle(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            AxCleanNozzleInfo.SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
            return SUCCESS;
        }

        public int SetPosition_CoatNozzle(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            AxCoatNozzleInfo.SetPositionSet(Pos_Fixed, Pos_Model, Pos_Offset);
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
        public int Absorb(bool bSkipSensor = false)
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

        public int Release(bool bSkipSensor = false)
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
            int iResult = CheckForChuckCylMoving();
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.ChuckTableUDCyl.Up(bSkipSensor);
            return iResult;
        }


        public int ChuckTableDown(bool bSkipSensor = false)
        {
            int iResult = CheckForChuckCylMoving();
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.ChuckTableUDCyl.Down(bSkipSensor);
            return iResult;
        }

        public int IsChuckTableUp(out bool bStatus)
        {
            int iResult = m_RefComp.ChuckTableUDCyl.IsUp(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsChuckTableDown(out bool bStatus)
        {
            int iResult = m_RefComp.ChuckTableUDCyl.IsDown(out bStatus);
            if (iResult != SUCCESS) return iResult;

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
            int iResult = m_RefComp.CleanNozzleSolCyl.IsOpen(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCleanNozzleValveClose(out bool bStatus)
        {
            int iResult = m_RefComp.CleanNozzleSolCyl.IsClose(out bStatus);
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
            int iResult = m_RefComp.CoatNozzleSolCyl.IsOpen(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsCoatNozzleValveClose(out bool bStatus)
        {
            int iResult = m_RefComp.CoatNozzleSolCyl.IsClose(out bStatus);
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
        public int IsAllAxisOrignReturned(out bool bStatus)
        {
            bool[] bAxisStatus;
            int iResult = m_RefComp.AxSpinRotate.IsOriginReturned(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            iResult = m_RefComp.AxSpinCleanNozzle.IsOriginReturned(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            iResult = m_RefComp.AxSpinCoatNozzle.IsOriginReturned(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return iResult;
        }

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

        public int CheckForChuckCylMoving()
        {
            bool bStatus, bStatus1;

            // check origin
            //int iResult = IsAllAxisOrignReturned(out bStatus);
            //if (iResult != SUCCESS) return iResult;
            //if (bStatus == false)
            //{
            //    return GenerateErrorCode(ERR_SPINNER_NOT_ORIGIN_RETURNED);
            //}

            // check vacuum
            int iResult = IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                iResult = IsAbsorbed(out bStatus1);
                if (iResult != SUCCESS) return iResult;
                if (bStatus1 == false) return GenerateErrorCode(ERR_SPINNER_OBJECT_DETECTED_BUT_NOT_ABSORBED);
            }
            else
            {
                iResult = IsReleased(out bStatus1);
                if (iResult != SUCCESS) return iResult;
                if (bStatus1 == false) return GenerateErrorCode(ERR_SPINNER_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED);
            }

            // check nozzle
            iResult = CheckCoatNozzleInSafetyZone(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return GenerateErrorCode(ERR_SPINNER_COAT_NOZZLE_NOT_IN_SAFETY_ZONE);

            iResult = CheckCleanNozzleInSafetyZone(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return GenerateErrorCode(ERR_SPINNER_CLEAN_NOZZLE_NOT_IN_SAFETY_ZONE);

            return SUCCESS;
        }

        public int CheckForRotateMoving()
        {
            bool bStatus;

            // check origin
            int iResult = IsAllAxisOrignReturned(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false)
            {
                return GenerateErrorCode(ERR_SPINNER_NOT_ORIGIN_RETURNED);
            }

            // check chuck is down
            iResult = IsChuckTableDown(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if(bStatus == false)
            {
#if !SIMULATION_TEST
                return GenerateErrorCode(ERR_SPINNER_CHUCKTABLE_NOT_DOWN);
#endif
            }

            // check vacuum
            iResult = IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                iResult = Absorb();
                if (iResult != SUCCESS) return iResult;
            } else
            {
                iResult = Release();
                if (iResult != SUCCESS) return iResult;
            }

            // check cylinder

            return SUCCESS;
        }

        public int CheckForCleanNozzleMoving()
        {
            bool bStatus;

            // check default
            int iResult = CheckForRotateMoving();
            if (iResult != SUCCESS) return iResult;

            // check other nozzle
            iResult = CheckCoatNozzleInSafetyZone(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return GenerateErrorCode(ERR_SPINNER_COAT_NOZZLE_NOT_IN_SAFETY_ZONE);

            return SUCCESS;
        }

        public int CheckForCoatNozzleMoving()
        {
            bool bStatus;

            // check default
            int iResult = CheckForRotateMoving();
            if (iResult != SUCCESS) return iResult;

            // check other nozzle
            iResult = CheckCleanNozzleInSafetyZone(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return GenerateErrorCode(ERR_SPINNER_CLEAN_NOZZLE_NOT_IN_SAFETY_ZONE);

            return SUCCESS;
        }

        public int IsRotateInLoadPos(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InRotateLoadPos, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////////
        // Rotate

        public int StopRotate()
        {
            return m_RefComp.AxSpinRotate.SetStop(DEF_T, DEF_STOP);
        }

        public int StartRotate(int nSpeed, bool bCWDir = true)
        {
            return m_RefComp.AxSpinRotate.JogMoveVelocity(DEF_T, bCWDir, nSpeed);
        }

        public int MoveRotateToLoadPos(double dMoveOffset = 0)
        {
            int iPos = (int)ERotatePos.LOAD;

            return MoveRotatePos(iPos, true, dMoveOffset);
        }

        private int MoveRotatePos(CPos_XYTZ sPos, bool bUseBacklash = false)
        {
            int iResult = SUCCESS;

            // safety check
            iResult = CheckForRotateMoving();
            if (iResult != SUCCESS) return iResult;

            // assume move all axis if bMoveFlag is null
            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, true, false };

            // trans to array
            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // backlash
            if (bUseBacklash)
            {
                // 나중에 작업
            }

            // 1. move Z Axis to Safety Up. but when need to move z axis only, don't need to move z to safety pos

            // 2. move X, Y, T
            //if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                iResult = m_RefComp.AxSpinRotate.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Rotate t axis", ELogType.Debug, ELogWType.D_Error);
                    return iResult;
                }
            }

            // 3. move Z Axis

            string str = $"success : move Rotate to pos:{sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        public int MoveRotatePos(int iPos, bool bUpdatedPosInfo = true, double dMoveOffset = 0, bool bUseBacklash = false)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos = AxRotateInfo.GetTargetPos(iPos);
            sTargetPos.dT += dMoveOffset;

            iResult = MoveRotatePos(sTargetPos, bUseBacklash);
            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxRotateInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////////
        // Clean Nozzle

        public int MoveCleanNozzleToSafetyPos(double dMoveOffset = 0)
        {
            int iPos = (int)ENozzlePos.SAFETY;

            return MoveCleanNozzlePos(iPos, true, dMoveOffset);
        }

        public int MoveCleanNozzleToStartPos(double dMoveOffset = 0)
        {
            int iPos = (int)ENozzlePos.START;

            return MoveCleanNozzlePos(iPos, true, dMoveOffset);
        }

        public int MoveCleanNozzleToEndPos(double dMoveOffset = 0)
        {
            int iPos = (int)ENozzlePos.END;

            return MoveCleanNozzlePos(iPos, true, dMoveOffset);
        }


        public int MoveCleanNozzlePos(CPos_XYTZ sPos, bool bUseBacklash = false)
        {
            int iResult = SUCCESS;

            // safety check
            iResult = CheckForCleanNozzleMoving();
            if (iResult != SUCCESS) return iResult;

            // assume move all axis if bMoveFlag is null
            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, true, false };

            // trans to array
            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // backlash
            if (bUseBacklash)
            {
                // 나중에 작업
            }

            // 1. move Z Axis to Safety Up. but when need to move z axis only, don't need to move z to safety pos

            // 2. move X, Y, T
            //if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                iResult = m_RefComp.AxSpinCleanNozzle.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move CleanNozzle t axis", ELogType.Debug, ELogWType.D_Error);
                    return iResult;
                }
            }

            // 3. move Z Axis

            string str = $"success : move CleanNozzle to pos:{sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        public int MoveCleanNozzlePos(int iPos, bool bUpdatedPosInfo = true, double dMoveOffset = 0, bool bUseBacklash = false)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos = AxCleanNozzleInfo.GetTargetPos(iPos);
            sTargetPos.dT += dMoveOffset;

            iResult = MoveCleanNozzlePos(sTargetPos, bUseBacklash);
            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxCleanNozzleInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////////
        // Coat Nozzle

        public int MoveCoatNozzleToSafetyPos(double dMoveOffset = 0)
        {
            int iPos = (int)ENozzlePos.SAFETY;

            return MoveCoatNozzlePos(iPos, true, dMoveOffset);
        }

        public int MoveCoatNozzleToStartPos(double dMoveOffset = 0)
        {
            int iPos = (int)ENozzlePos.START;

            return MoveCoatNozzlePos(iPos, true, dMoveOffset);
        }

        public int MoveCoatNozzleToEndPos(double dMoveOffset = 0)
        {
            int iPos = (int)ENozzlePos.END;

            return MoveCoatNozzlePos(iPos, true, dMoveOffset);
        }


        public int MoveCoatNozzlePos(CPos_XYTZ sPos, bool bUseBacklash = false)
        {
            int iResult = SUCCESS;

            // safety check
            iResult = CheckForCoatNozzleMoving();
            if (iResult != SUCCESS) return iResult;

            // assume move all axis if bMoveFlag is null
            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, true, false };

            // trans to array
            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // backlash
            if (bUseBacklash)
            {
                // 나중에 작업
            }

            // 1. move Z Axis to Safety Up. but when need to move z axis only, don't need to move z to safety pos

            // 2. move X, Y, T
            //if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                iResult = m_RefComp.AxSpinCoatNozzle.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move CoatNozzle t axis", ELogType.Debug, ELogWType.D_Error);
                    return iResult;
                }
            }

            // 3. move Z Axis

            string str = $"success : move CoatNozzle to pos:{sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.D_Normal);

            return SUCCESS;
        }

        public int MoveCoatNozzlePos(int iPos, bool bUpdatedPosInfo = true, double dMoveOffset = 0, bool bUseBacklash = false)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos = AxCoatNozzleInfo.GetTargetPos(iPos);
            sTargetPos.dT += dMoveOffset;

            iResult = MoveCoatNozzlePos(sTargetPos, bUseBacklash);
            if (iResult != SUCCESS) return iResult;
            if (bUpdatedPosInfo == true)
            {
                AxCoatNozzleInfo.PosInfo = iPos;
            }

            return SUCCESS;
        }

        ///////////////////////////////////////////////////////////////////////////////
        //

        public int CheckCleanNozzleInSafetyZone(out bool bStatus)
        {
#if SIMULATION_TEST
            bStatus = true;
            return SUCCESS;
#endif
            int iResult = m_RefComp.IO.IsOn(m_Data.CleanNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY], out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int CheckCoatNozzleInSafetyZone(out bool bStatus)
        {
#if SIMULATION_TEST
            bStatus = true;
            return SUCCESS;
#endif
            int iResult = m_RefComp.IO.IsOn(m_Data.CoatNozzleZoneCheck.Axis[DEF_T].ZoneAddr[(int)ENozzleAxZone.SAFETY], out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        #endregion
    }
}
