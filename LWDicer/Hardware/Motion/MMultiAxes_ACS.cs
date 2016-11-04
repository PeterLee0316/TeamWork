using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionYMC;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_ACS;
using static LWDicer.Layers.DEF_IO;
using static LWDicer.Layers.DEF_Motion;
using static LWDicer.Layers.DEF_MultiAxesACS;

namespace LWDicer.Layers
{
    public class DEF_MultiAxesACS
    {
        public const int ERR_MAXES_ACS_INVALID_AXIS_ID = 1;
        public const int ERR_MAXES_ACS_INVALID_AXIS_NUMBER = 2;

        public class CMutliAxesACSRefComp
        {
            public MACS Motion;  // 수정 예정
        }

        public class CMultiAxesACSData
        {
            public int[] AxisList = new int[DEF_MAX_COORDINATE];

            public CMultiAxesACSData(int DeviceNo, int[] AxisList)
            {
                Array.Copy(AxisList, this.AxisList, AxisList.Length);
            }
        }

    }

    public class MMultiAxes_ACS : MObject
    {
        private CMutliAxesACSRefComp m_RefComp;
        private CMultiAxesACSData m_Data;

        private int[] MovePriority = new int[DEF_MAX_COORDINATE];   // 축 이동시에 우선 순위
        private int[] OriginPriority = new int[DEF_MAX_COORDINATE];   // 축 원점복귀시에 우선 순위
        private CACSServoStatus[] ServoStatus = new CACSServoStatus[DEF_MAX_COORDINATE];

        public MMultiAxes_ACS(CObjectInfo objInfo, CMutliAxesACSRefComp refComp, CMultiAxesACSData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            for (int i = 0; i < DEF_MAX_COORDINATE; i++)
            {
                ServoStatus[i] = new CACSServoStatus();
            }
        }

        public int SetData(CMultiAxesACSData source)
        {
            m_Data = ObjectExtensions.Copy(source);

            return SUCCESS;
        }

        public int GetData(out CMultiAxesACSData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetStop(int iCoordID = DEF_ALL_COORDINATE, short siType = DEF_STOP)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
                //iResult = m_RefComp.Motion.ServoMotionStopAll()
                ;
            else
                //iResult = m_RefComp.Motion.StopServoMotion(iCoordID)
                ;


            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SetAxesMovePriority(int[] iPriorities, int iCoordID = DEF_ALL_COORDINATE)
        {
            int iResult = SUCCESS;

            for (int i = 0; i < DEF_MAX_COORDINATE; i++)
            {
                MovePriority[i] = (int)EPriority.NONE;
            }

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                for (int i = 0; i < iPriorities.Length; i++)
                {
                    MovePriority[i] = iPriorities[i];
                }
            }
            else
            {
                MovePriority[iCoordID] = iPriorities[0];
            }

            return SUCCESS;
        }

        public int SetAxesOriginPriority(int[] iPriorities, int iCoordID = DEF_ALL_COORDINATE)
        {
            int iResult = SUCCESS;

            for (int i = 0; i < DEF_MAX_COORDINATE; i++)
            {
                OriginPriority[i] = (int)EPriority.NONE;
            }

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                for (int i = 0; i < iPriorities.Length; i++)
                {
                    OriginPriority[i] = iPriorities[i];
                }
            }
            else
            {
                OriginPriority[iCoordID] = iPriorities[0];
            }

            return SUCCESS;
        }

        /// <summary>
        /// 축의 현재좌표를 읽는다.
        /// </summary>
        public int GetCurPos(out double[] dPos, int iCoordID = DEF_ALL_COORDINATE)
        {
            int iResult = SUCCESS;

            UpdateAxisStatus();
            if (iCoordID == DEF_ALL_COORDINATE)
            {
                dPos = new double[DEF_MAX_COORDINATE];
                for (int i = 0; i < DEF_MAX_COORDINATE; i++)
                {
                    if (m_Data.AxisList[i] == DEF_AXIS_NONE_ID) dPos[i] = 0;
                    else dPos[i] = ServoStatus[i].EncoderPos;
                }
            }
            else
            {
                dPos = new double[1];
                dPos[0] = ServoStatus[iCoordID].EncoderPos;
            }
            return SUCCESS;
        }

        public int GetCurPos(out CPos_XYTZ pos, int iCoordID = DEF_ALL_COORDINATE)
        {
            int iResult = SUCCESS;

            double[] dPos;
            GetCurPos(out dPos, iCoordID);

            pos = new CPos_XYTZ();
            if (iCoordID == DEF_ALL_COORDINATE)
            {
                pos.TransFromArray(dPos);
            }
            else
            {
                pos.SetPosition(iCoordID, dPos[0]);
            }
            return SUCCESS;
        }

        /// <summary>
        /// 축의 현재좌표를 읽는다.
        /// </summary>
        public int GetCmdPos(out double[] dPos, int iCoordID = DEF_ALL_COORDINATE)
        {
            int iResult = SUCCESS;

            UpdateAxisStatus();
            if (iCoordID == DEF_ALL_COORDINATE)
            {
                dPos = new double[DEF_MAX_COORDINATE];
                for (int i = 0; i < DEF_MAX_COORDINATE; i++)
                {
                    if (m_Data.AxisList[i] == DEF_AXIS_NONE_ID) dPos[i] = 0;
                    else dPos[i] = ServoStatus[i].CommandPos;
                }
            }
            else
            {
                dPos = new double[1];
                dPos[0] = ServoStatus[iCoordID].CommandPos;
            }
            return SUCCESS;
        }

        public int GetCmdPos(out CPos_XYTZ pos, int iCoordID = DEF_ALL_COORDINATE)
        {
            int iResult = SUCCESS;

            double[] dPos;
            GetCmdPos(out dPos, iCoordID);

            pos = new CPos_XYTZ();
            if (iCoordID == DEF_ALL_COORDINATE)
            {
                pos.TransFromArray(dPos);
            }
            else
            {
                pos.SetPosition(iCoordID, dPos[0]);
            }
            return SUCCESS;
        }

        public bool IsBusy(int iCoordID = DEF_ALL_COORDINATE)
        {
            bool bRunBit=false;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                for (int i = 0; i < DEF_MAX_COORDINATE; i++)
                {
                    if (m_Data.AxisList[i] == DEF_AXIS_NONE_ID) continue;

                    if (ServoStatus[i].IsBusy) return true; 
                }

                bRunBit = false;
            }
            else
            {
                bRunBit = ServoStatus[iCoordID].IsBusy;
            }

            return bRunBit;
        }

        /// <summary>
        /// Move 함수, 전체축 이동일 경우엔 bMoveUse와 Priority를 이용하여 순차 이동, 선택 이동 가능
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="bMoveUse"></param>
        /// <param name="dPosition"></param>
        /// <param name="bUsePriority"></param>
        /// <param name="tempSpeed"></param>
        /// <returns></returns>
        public int Move(int iCoordID, bool[] bMoveUse, double[] dPosition, bool bUsePriority = false,
            CMotorSpeedData[] tempSpeed = null)
        {
            int iResult = SUCCESS;
            int iAxisID;
            int iAxisCount = 0;
            bool[] bPartUse = new bool[DEF_MAX_COORDINATE];
            bool bPartMove;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                // call api by axis group
                if (bUsePriority == true)
                {
                    for (int i = 0; i < (int)EPriority.MAX; i++)
                    {
                        bPartMove = false;
                        for (int j = 0; j < DEF_MAX_COORDINATE; j++)
                            bPartUse[j] = false;

                        // 우선순위안의 축 모두 이동
                        for (int j = 0; j < DEF_MAX_COORDINATE; j++)
                        {
                            if (bMoveUse[j] == false) continue;
                            if (MovePriority[j] == i)
                            {
                                bPartMove = true;
                                bPartUse[j] = true;
                            }
                        }

                        if (bPartMove == true)
                        {
                            iResult = m_RefComp.Motion.MoveToPos(m_Data.AxisList, bPartUse, dPosition, tempSpeed);
                            if (iResult != SUCCESS) return iResult;
                        }
                    }
                }
                else
                {
                    iResult = m_RefComp.Motion.MoveToPos(m_Data.AxisList, bMoveUse, dPosition, tempSpeed);
                    if (iResult != SUCCESS) return iResult;
                }
            }
            else
            {
                // call api by each axis(one device)
                iResult = m_RefComp.Motion.MoveToPos(m_Data.AxisList[iCoordID], dPosition[iCoordID]);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        // 잘 사용하지 않음...
        public int StartMove(int iCoordID, bool[] bMoveUse, double[] dPosition, CMotorSpeedData[] tempSpeed = null)
        {
            int iResult = SUCCESS;
            if (iCoordID == DEF_ALL_COORDINATE)
            {
                // call api by axis group
                iResult = m_RefComp.Motion.StartMoveToPos(m_Data.AxisList, bMoveUse, dPosition, tempSpeed);
                if (iResult != SUCCESS) return iResult;
            }
            else
            {
                // call api by each axis(one device)
                iResult = m_RefComp.Motion.StartMoveToPos(m_Data.AxisList[iCoordID], dPosition[iCoordID], tempSpeed[iCoordID]);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        /// <summary>
        /// 상대위치 이동 함수
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="bMoveUse"></param>
        /// <param name="dPosition"></param>
        /// <param name="bUsePriority"></param>
        /// <param name="tempSpeed"></param>
        /// <returns></returns>
        public int RMove(int iCoordID, bool[] bMoveUse, double[] dPosition, bool bUsePriority = false,
    CMotorSpeedData[] tempSpeed = null)
        {
            int iResult = SUCCESS;

            double[] dCurPos;
            iResult = GetCurPos(out dCurPos, iCoordID);
            if (iResult != SUCCESS) return iResult;
            Array.Copy(dCurPos, dPosition, dCurPos.Length);

            iResult = Move(iCoordID, bMoveUse, dPosition, bUsePriority, tempSpeed);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int StartRMove(int iCoordID, bool[] bMoveUse, double[] dPosition, CMotorSpeedData[] tempSpeed = null)
        {
            int iResult = SUCCESS;

            double[] dCurPos;
            iResult = GetCurPos(out dCurPos, iCoordID);
            if (iResult != SUCCESS) return iResult;
            Array.Copy(dCurPos, dPosition, dCurPos.Length);

            iResult = StartMove(iCoordID, bMoveUse, dPosition, tempSpeed);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// 등속 이동 함수. 
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="dVelocity"></param>
        /// <param name="iAccelerate"></param>
        /// <param name="bDir"></param>
        /// <returns></returns>
        public int VMove(int iCoordID, double dVelocity, int iAccelerate, bool bDir)
        {
            // 나중에 구현 

            return SUCCESS;
        }

        /// <summary>
        /// 축 이동 완료 체크
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="bMoveUse"></param>
        /// <returns></returns>
        public int Wait4Done(int iCoordID = DEF_ALL_COORDINATE, bool[] bMoveUse = null)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                bMoveUse = new bool[DEF_MAX_COORDINATE] { true, true, true, true };
                iResult = m_RefComp.Motion.Wait4Done(m_Data.AxisList, bMoveUse);
            }
            else
            {
                int[] tAxes = new int[1] { m_Data.AxisList[iCoordID] };
                bMoveUse = new bool[1] { true };
                iResult = m_RefComp.Motion.Wait4Done(tAxes, bMoveUse);
            }

            return iResult;
        }

        /// <summary>
        /// 축이 이동 완료되었는지 확인한다.
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="bDone"></param>
        /// <returns></returns>
        public int CheckDone(int iCoordID, out bool[] bDone)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.CheckMoveComplete(m_Data.AxisList, out bDone);
            }
            else
            {
                int[] tAxes = new int[1] { m_Data.AxisList[iCoordID] };
                iResult = m_RefComp.Motion.CheckMoveComplete(tAxes, out bDone);
            }

            return iResult;

        }

        public int JogMovePitch(int iCoordID, bool bDir, double dPitch)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                return GenerateErrorCode(ERR_MAXES_ACS_INVALID_AXIS_ID);
            }
            else
            {
                iResult = m_RefComp.Motion.StartJogMove(m_Data.AxisList[iCoordID], bDir, false);
            }

            return iResult;
        }

        public int JogMoveVelocity(int iCoordID, bool bDir, bool IsFastMove)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                return GenerateErrorCode(ERR_MAXES_ACS_INVALID_AXIS_ID);
            }
            else
            {
                iResult = m_RefComp.Motion.StartJogMove(m_Data.AxisList[iCoordID], bDir, IsFastMove);
            }

            return iResult;
        }

        public int EStop(int iCoordID)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.StopAllServoMotion();
            }
            else
            {
                iResult = m_RefComp.Motion.StopServoMotion(m_Data.AxisList[iCoordID]);
            }

            return SUCCESS;
        }

        public int ServoOn(int iCoordID)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.AllServoOn();
            }
            else
            {
                iResult = m_RefComp.Motion.ServoOn(m_Data.AxisList[iCoordID]);
            }

            return SUCCESS;
        }

        public int ServoOff(int iCoordID)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.AllServoOff();
            }
            else
            {
                iResult = m_RefComp.Motion.ServoOff(m_Data.AxisList[iCoordID]);
            }

            return iResult;
        }

        public void UpdateAxisStatus()
        {
            for (int i = 0; i < DEF_MAX_COORDINATE; i++)
            {
                if (m_Data.AxisList[i] == DEF_AXIS_NONE_ID) continue;
                ServoStatus[i] = ObjectExtensions.Copy(m_RefComp.Motion.ServoStatus[m_Data.AxisList[i]]);
            }
        }

        private int GetCoordLength(int iCoordID)
        {
            int length = (iCoordID == DEF_ALL_COORDINATE) ? m_Data.AxisList.Length : 1;

            return length;
        }

        public int CheckHomeSensor(int iCoordID, out bool[] bStatus)
        {
            UpdateAxisStatus();
            bStatus = new bool[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                bStatus[i] = ServoStatus[i].DetectHomeSensor;
            }

            return SUCCESS;
        }

        public int CheckPositiveSensor(int iCoordID, out bool[] bStatus)
        {
            UpdateAxisStatus();
            bStatus = new bool[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                bStatus[i] = ServoStatus[i].DetectPlusSensor;
            }

            return SUCCESS;
        }

        public int CheckNegativeSensor(int iCoordID, out bool[] bStatus)
        {
            UpdateAxisStatus();
            bStatus = new bool[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                bStatus[i] = ServoStatus[i].DetectMinusSensor;
            }

            return SUCCESS;
        }

        public int GetAmpEnable(int iCoordID, out bool[] bStatus)
        {
            UpdateAxisStatus();
            bStatus = new bool[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                if (ServoStatus[i].IsDriverFault == false && ServoStatus[i].IsServoOn == true)
                    bStatus[i] = true;
                else bStatus[i] = false;
            }

            return SUCCESS;
        }

        public int GetAmpFault(int iCoordID, out bool[] bStatus)
        {
            GetAmpEnable(iCoordID, out bStatus);
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                bStatus[i] = !bStatus[i];
            }

            return SUCCESS;
        }

        /// <summary>
        /// 축의 상태를 초기화 한다. (한개의 축 혹은 구성된 모든 축에 대해 초기화)
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <returns></returns>
        public int ClearAxis(int iCoordID)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.ResetAlarm();
            }
            else
            {
                iResult = m_RefComp.Motion.ResetAlarm(m_Data.AxisList[iCoordID]);
            }

            return SUCCESS;
        }

        public int GetServoAlarm(int iCoordID, out bool[] alarm, out int[] alarmCode)
        {
            UpdateAxisStatus();
            alarm = new bool[GetCoordLength(iCoordID)];
            alarmCode = new int[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                alarm[i] = ServoStatus[i].IsDriverFault;
            }

            return SUCCESS;
        }

        public int ComparePosition(double[] dTargetPos, out bool[] bJudge, int iCoordID = DEF_ALL_COORDINATE)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.ComparePosition(m_Data.AxisList, dTargetPos, out bJudge);
                if (iResult != SUCCESS) return iResult;
            }
            else
            {
                int[] tAxes = new int[1] { m_Data.AxisList[iCoordID] };
                iResult = m_RefComp.Motion.ComparePosition(tAxes, dTargetPos, out bJudge);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int IsOriginReturned(int iCoordID, out bool bResult, out bool[] bStatus)
        {
            UpdateAxisStatus();
            bStatus = new bool[GetCoordLength(iCoordID)];

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                bResult = true;
                for (int i = 0; i < DEF_MAX_COORDINATE; i++)
                {
                    if (m_Data.AxisList[i] == DEF_AXIS_NONE_ID) continue;
                    bStatus[i] = ServoStatus[i].IsOriginReturned;
                    if (bStatus[i] == false) bResult = false;
                }

            }
            else
            {
                bStatus[0] = ServoStatus[iCoordID].IsOriginReturned;
                bResult = bStatus[0];
            }

            return SUCCESS;
        }

        public bool HasAxis(int iCoordID)
        {
            if (m_Data.AxisList[iCoordID] != (int)EACS_Axis.NULL)
            {
                return true;
            }
            return false;
        }
    }
}
