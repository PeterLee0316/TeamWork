using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_IO;
using static LWDicer.Layers.DEF_CtrlLoader;
using static LWDicer.Layers.DEF_MeElevator;
using static LWDicer.Layers.DEF_DataManager;

namespace LWDicer.Layers
{
    public class DEF_CtrlLoader
    {

        public const int ERR_CTRL_LOADER_UNABLE_TO_USE_LOADER                      = 1;
        public const int ERR_CTRL_LOADER_UNABLE_TO_USE_LOG                         = 2;
        public const int ERR_CTRL_LOADER_OBJECT_ABSORBED                           = 3;
        public const int ERR_CTRL_LOADER_OBJECT_NOT_ABSORBED                       = 4;
        public const int ERR_CTRL_LOADER_OBJECT_EXIST                              = 5;
        public const int ERR_CTRL_LOADER_OBJECT_NOT_EXIST                          = 6;
        public const int ERR_CTRL_LOADER_CHECK_RUN_BEFORE_FAILED                   = 7;
        public const int ERR_CTRL_LOADER_CYLINDER_TIMEOUT                          = 8;
        public const int ERR_CTRL_LOADER_NOT_UP                                    = 9;
        public const int ERR_CTRL_LOADER_CANNOT_DETECT_POSINFO                     = 10;
        public const int ERR_CTRL_LOADER_PUSHPULL_DETECTED                             = 11;

        public class CCtrlLoaderRefComp
        {
            public IIO IO;
            public MMeElevator Elevator;

            public CCtrlLoaderRefComp()
            {
            }
            public override string ToString()
            {
                return $"CCtrlLoaderRefComp : ";
            }
        }

        public class CCtrlLoaderData
        {
            public CCtrlLoaderData()
            {

            }
        }
    }

    public class MCtrlLoader : MCtrlLayer
    {
        private CCtrlLoaderRefComp m_RefComp;
        private CCtrlLoaderData m_Data;

        public MCtrlLoader(CObjectInfo objInfo,
            CCtrlLoaderRefComp refComp, CCtrlLoaderData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CCtrlLoaderData source)
        {
            m_Data = ObjectExtensions.Copy(source);

            return SUCCESS;
        }

        public int GetData(out CCtrlLoaderData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public override int Initialize()
        {
            int iResult;
            bool bStatus, bStatus1;
            // UpperHandler
            // check wafer cassett exist & 정위치에 있는지
            iResult = IsCassetteExist(out bStatus);
            if (iResult != SUCCESS) return iResult;

            // check pushpull detected
            iResult = IsPushPullDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus) return GenerateErrorCode(ERR_CTRL_LOADER_PUSHPULL_DETECTED);

            // check wafer cassett 안에 wafer가 있는지 
            iResult = SearchCassetteWafer();
            if (iResult != SUCCESS) return iResult;

            // 기존 wafer 정보와 비교하는 부분

            // move to 첫번째 wafer가 위치해 있는 slot으로
            iResult = MoveToNextSlotPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        #endregion

        #region Cylinder, Vacuum, Detect Object
        public int SetLoaderPosition(CPosition FixedPos, CPosition ModelPos, CPosition OffsetPos)
        {
            m_RefComp.Elevator.SetElevatorPosition(FixedPos, ModelPos, OffsetPos);
            return SUCCESS;
        }

        public void SetPosInfo(int posInfo)
        {
            m_RefComp.Elevator.SetElevatorPosInfo(posInfo);
        }

        public int ComparePosInfo(int iPosIndex, out int iSlotNum)
        {
            int iResult;
            iResult = m_RefComp.Elevator.GetElevatorPosInfo(out iPosIndex, out iSlotNum);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        public int IsOrignReturn(out bool bStatus)
        {
            m_RefComp.Elevator.IsElevatorOrignReturn(out bStatus);

            return SUCCESS;
        }
        
        /// <summary>
        /// Push-Pull Gripper의 정위치 센서로 Interlock을 체크한다.
        /// </summary>
        /// <param name="bStatus"></param>
        /// <returns></returns>
        public int IsPushPullDetected(out bool bStatus)
        {
            int iResult = m_RefComp.Elevator.IsPushPullDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }


        public int MoveToBottomPos(bool bPanelTransfer = true)
        {
            int iResult = CheckSafety_forMoving((int)EElevatorPos.BOTTOM);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Elevator.MoveElevatorToBottomPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToLoadPos(bool bPanelTransfer = true)
        {
            int iResult = CheckSafety_forMoving((int)EElevatorPos.LOAD);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Elevator.MoveElevatorToLoadPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToSlotPos(int iSlotNum = 0, bool bPanelTransfer = true)
        {
            int iResult = CheckSafety_forMoving((int)EElevatorPos.SLOT);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Elevator.MoveElevatorToSlotPos(iSlotNum);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToTopPos(bool bPanelTransfer = true)
        {
            int iResult = CheckSafety_forMoving((int)EElevatorPos.TOP);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Elevator.MoveElevatorToTopPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToSafetyPos(bool bPanelTransfer = true)
        {
            int iResult = CheckSafety_forMoving((int)EElevatorPos.SAFETY);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Elevator.MoveElevatorToSafetyPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToNextSlotPos(bool bDirect = true, bool bPanelTransfer = true)
        {
            int iResult = CheckSafety_forMoving((int)EElevatorPos.SLOT);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Elevator.MoveElevatorNextSlot(bDirect);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToNextEmptySlotPos(bool bPanelTransfer = true)
        {
            int iResult = CheckSafety_forMoving((int)EElevatorPos.BOTTOM);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Elevator.MoveElevatorNextEmptySlot();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToNextProcessSlotPos(bool bPanelTransfer = true)
        {
            int iResult = CheckSafety_forMoving((int)EElevatorPos.BOTTOM);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Elevator.MoveElevatorNextProcessWaferSlot();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SearchCassetteWafer()
        {
            int iResult = CheckSafety_forMoving((int)EElevatorPos.SLOT);
            if (iResult != SUCCESS) return iResult;

            iResult = m_RefComp.Elevator.SearchElevatorCassetteWafer();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetAxZone(int axis, out int curZone)
        {
            int iResult;
            iResult = m_RefComp.Elevator.GetElevatorAxZone(axis, out curZone);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsAxisInSafetyZone(int axis, out bool bStatus)
        {
            int iResult;
            iResult = m_RefComp.Elevator.IsElevatorAxisInSafetyZone(axis, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsWaferDetected(out bool bState)
        {
            bState = false;
            int iResult;

            iResult = m_RefComp.Elevator.IsWaferDetected(out bState);

            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }
        #endregion

        public int IsCassetteExist(out bool bStatus)
        {
            int iResult = m_RefComp.Elevator.IsCassetteExist(out bStatus);
            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }

        public int CheckSafety_forMoving(int nTargetPos)
        {
            int iResult = SUCCESS;
            bool bStatus;

            // check cassette detected
            iResult = IsCassetteExist(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if(bStatus == true)
            {
                iResult = IsPushPullDetected(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if(bStatus == true)
                {
                    return GenerateErrorCode(ERR_CTRL_LOADER_PUSHPULL_DETECTED);
                }
            }

            // check return origin & cassette
            iResult = m_RefComp.Elevator.CheckSafety_forMoving();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
    }

}
