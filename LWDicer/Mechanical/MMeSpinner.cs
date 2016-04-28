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

        public enum ESpinnerType
        {
            NONE = -1,
            AXIS = 0,
            CYL,
        }

        public enum ENozzlePos
        {
            NONE = -1,
            READY = 0,
            EDGE,
            CENTER,
            MAX,
        }

        public enum EChuckVacuum
        {
            SELF,           // 자체 발생 진공
            FACTORY,        // 공장 진공
            OBJECT,         // LCD 패널의 PCB같은 걸 집는 용도
            EXTRA_SELF,     // 
            EXTRA_FACTORY,  //
            EXTRA_OBJECT,   //
            MAX,
        }

        public class CMeSpinnerRefComp
        {
            public IIO IO;

            // Cylinder
            public ICylinder UpDownCyl;
            public ICylinder Nozzle1SolCyl;
            public ICylinder Nozzle2SolCyl;
            public ICylinder RingBlow;

            public MMultiAxes_YMC AxSpinRotate;
            public MMultiAxes_YMC AxSpinNozzle1;
            public MMultiAxes_YMC AxSpinNozzle2;

            // Vacuum
            public IVacuum[] Vacuum = new IVacuum[(int)EChuckVacuum.MAX];
        }


        public class CMeSpinnerData
        {
            // Coater Type
            public ESpinnerType[] SpinnerType = new ESpinnerType[DEF_MAX_COORDINATE];

            public int InDetectObject;

            public CMeSpinnerData(ESpinnerType[] SpinnerType = null)
            {
                if(SpinnerType == null)
                {
                    for(int i=0;i<this.SpinnerType.Length;i++)
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

        private CMovingObject AxNozzeleInfo = new CMovingObject((int)ENozzlePos.MAX);

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

        public int SetNozzlePosition(CUnitPos FixedPos, CUnitPos ModelPos, CUnitPos OffsetPos)
        {
            AxNozzeleInfo.SetPosition(FixedPos, ModelPos, OffsetPos);
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

        public int GetRotateCurPos(out CPos_XYTZ pos)
        {
            m_RefComp.AxSpinRotate.GetCurPos(out pos);
            return SUCCESS;
        }

        public int GetNozzle1CurPos(out CPos_XYTZ pos)
        {
            m_RefComp.AxSpinNozzle1.GetCurPos(out pos);
            return SUCCESS;
        }

        public int GetNozzle2CurPos(out CPos_XYTZ pos)
        {
            m_RefComp.AxSpinNozzle2.GetCurPos(out pos);
            return SUCCESS;
        }


        public int MoveToRotateStop()
        {
            return m_RefComp.AxSpinRotate.SetStop(DEF_MAX_COORDINATE, DEF_STOP);
        }

        public int MoveToRotateCW(int nSpeed)
        {
            return m_RefComp.AxSpinRotate.JogMoveVelocity(DEF_MAX_COORDINATE, true, nSpeed);
        }

        public int MoveToRotateCCW(int nSpeed)
        {
            return m_RefComp.AxSpinRotate.JogMoveVelocity(DEF_MAX_COORDINATE, false, nSpeed);
        }

        public int MoveToNozzle1ReadyPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.READY;

            return MoveNozzle1Pos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToNozzle1EdgePos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.EDGE;

            return MoveNozzle1Pos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToNozzle1CenterPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.CENTER;

            return MoveNozzle1Pos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveNozzle1Pos(CPos_XYTZ sPos, int iPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
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
                    m_RefComp.AxSpinNozzle1.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxSpinNozzle1.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Nozzle x y t axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }

            
            // set working pos
            if (iPos > (int)ENozzlePos.NONE)
            {
                AxNozzeleInfo.PosInfo = iPos;
            }

            string str = $"success : move Nozzle to pos:{iPos} {sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.Normal);

            return SUCCESS;
        }

        public int MoveNozzle1Pos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos = AxNozzeleInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            if (bUpdatedPosInfo == false)
            {
                iPos = (int)ENozzlePos.NONE;
            }
            iResult = MoveNozzle1Pos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveNozzle1Pos(int iPos, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                return MoveNozzle1Pos(iPos);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveNozzle1Pos(iPos, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveNozzle1Pos(iPos, false, bMoveFlag);
            }

            return SUCCESS;
        }

        public int MoveToNozzle2ReadyPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.READY;

            return MoveNozzle2Pos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToNozzle2EdgePos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.EDGE;

            return MoveNozzle2Pos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveToNozzle2CenterPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)ENozzlePos.CENTER;

            return MoveNozzle2Pos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveNozzle2Pos(CPos_XYTZ sPos, int iPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
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
                    m_RefComp.AxSpinNozzle2.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxSpinNozzle2.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Nozzle x y t axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }


            // set working pos
            if (iPos > (int)ENozzlePos.NONE)
            {
                AxNozzeleInfo.PosInfo = iPos;
            }

            string str = $"success : move Nozzle to pos:{iPos} {sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.Normal);

            return SUCCESS;
        }

        public int MoveNozzle2Pos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos = AxNozzeleInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            if (bUpdatedPosInfo == false)
            {
                iPos = (int)ENozzlePos.NONE;
            }
            iResult = MoveNozzle2Pos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveNozzle2Pos(int iPos, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                return MoveNozzle2Pos(iPos);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveNozzle2Pos(iPos, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveNozzle2Pos(iPos, false, bMoveFlag);
            }

            return SUCCESS;
        }
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


        public int CylUp(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.UpDownCyl.Up(bSkipSensor);
            return iResult;
        }


        public int CylDown(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.UpDownCyl.Down(bSkipSensor);
            return iResult;
        }

        public int Nozzle1ValveOpen(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.Nozzle1SolCyl.Open(bSkipSensor);
            return iResult;
        }

        public int Nozzle1ValveClose(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.Nozzle1SolCyl.Close(bSkipSensor);
            return iResult;
        }
        public int Nozzle2ValveOpen(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.Nozzle2SolCyl.Open(bSkipSensor);
            return iResult;
        }

        public int Nozzle2ValveClose(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.Nozzle2SolCyl.Close(bSkipSensor);
            return iResult;
        }
        public int RingBlowOn(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.RingBlow.Open(bSkipSensor);
            return iResult;
        }

        public int RingBlowOff(bool bSkipSensor = false)
        {
            int iResult = m_RefComp.RingBlow.Close(bSkipSensor);
            return iResult;
        }


        public int IsCylUp(out bool bStatus)
        {
            int iResult;

            iResult= m_RefComp.UpDownCyl.IsUp(out bStatus);

            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return SUCCESS;
        }

        public int IsCylDown(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.UpDownCyl.IsUp(out bStatus);

            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return SUCCESS;
        }

        public int IsNozzle1ValveOpen(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.Nozzle1SolCyl.IsOpen(out bStatus);

            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return SUCCESS;
        }

        public int IsNozzle1ValveClose(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.Nozzle1SolCyl.IsClose(out bStatus);

            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return SUCCESS;
        }

        public int IsNozzle2ValveOpen(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.Nozzle2SolCyl.IsOpen(out bStatus);

            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return SUCCESS;
        }

        public int IsNozzle2ValveClose(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.Nozzle2SolCyl.IsClose(out bStatus);

            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return SUCCESS;
        }
        public int IsRindBlowOn(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.RingBlow.IsOpen(out bStatus);

            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return SUCCESS;
        }

        public int IsRindBlowOff(out bool bStatus)
        {
            int iResult;

            iResult = m_RefComp.RingBlow.IsClose(out bStatus);

            if (iResult != SUCCESS) return iResult;
            if (bStatus == false) return SUCCESS;

            return SUCCESS;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InDetectObject, out bStatus);
            if (iResult != SUCCESS) return iResult;

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
    }
}
