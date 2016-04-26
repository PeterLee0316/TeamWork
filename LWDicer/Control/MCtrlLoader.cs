using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_IO;
using static LWDicer.Control.DEF_CtrlLoader;

namespace LWDicer.Control
{
    public class DEF_CtrlLoader
    {

        public const int ERR_CTRL_LOADER_UNABLE_TO_USE_LOADER = 1;
        public const int ERR_CTRL_LOADER_UNABLE_TO_USE_LOG = 2;
        public const int ERR_CTRL_LOADER_OBJECT_ABSORBED = 3;
        public const int ERR_CTRL_LOADER_OBJECT_NOT_ABSORBED = 4;
        public const int ERR_CTRL_LOADER_OBJECT_EXIST = 5;
        public const int ERR_CTRL_LOADER_OBJECT_NOT_EXIST = 6;
        public const int ERR_CTRL_LOADER_CHECK_RUN_BEFORE_FAILED = 7;
        public const int ERR_CTRL_LOADER_CYLINDER_TIMEOUT = 8;
        public const int ERR_CTRL_LOADER_NOT_UP = 9;
        public const int ERR_CTRL_LOADER_CANNOT_DETECT_POSINFO = 10;
        public const int ERR_CTRL_LOADER_PCB_DOOR_OPEN = 11;
        public const int ERR_CTRL_LOADER_ULOADER_IN_DOWN_AND_LLOADER_IN_SAME_XZONE = 12;
        public const int ERR_CTRL_LOADER_ULOADER_NEED_DOWN_AND_LLOADER_IN_SAME_XZONE = 13;
        public const int ERR_CTRL_LOADER_LLOADER_NEED_MOVE_AND_ULOADER_IN_DOWN = 14;
        public const int ERR_CTRL_LOADER_XAX_POS_NOT_MATCH_ZONE = 15;

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
            public int InPushPullSafety = IO_ADDR_NOT_DEFINED;
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

        public int SetLoaderPosition(CUnitPos FixedPos, CUnitPos ModelPos, CUnitPos OffsetPos)
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
            int iResult = 0;
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
        public int IsPushPullSafetyPos(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InPushPullSafety, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        

        public int MoveToBottomPos(bool bObsolite)
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.MoveElevatorToBottomPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToLoadPos()
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.MoveElevatorToLoadPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToSlotPos(int iSlotNum = 0)
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.MoveElevatorToSlotPos(iSlotNum);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToTopPos()
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.MoveElevatorToTopPos();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToNextSlotPos(bool bDirect = true)
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.MoveElevatorNextSlot(bDirect);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToNextEmptySlotPos()
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.MoveElevatorNextEmptySlot();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToNextProcessSlotPos()
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.MoveElevatorNextProcessWaferSlot();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SearchCassetteWafer()
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.SearchElevatorCassetteWafer();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetAxZone(int axis, out int curZone)
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.GetElevatorAxZone(axis, out curZone);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsAxisInSafetyZone(int axis, out bool bStatus)
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.IsElevatorAxisInSafetyZone(axis, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int CheckForAxisMove(out bool bStatus)
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.CheckForElevatorAxisMove(out bStatus);
            if (iResult != SUCCESS)
            {
                bStatus = false;
                return iResult;
            }

            bStatus = true;
            return SUCCESS;
        }

        public int IsWaferDetected(out bool bState)
        {
            bState = false;
            int iResult = 0;

            iResult = m_RefComp.Elevator.IsObjectDetected(out bState);

            if (iResult != SUCCESS) return iResult;
            return SUCCESS;
        }

        public int IsCassetteExist(out bool bStatus)
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.IsElevatorCassetteExist(out bStatus);
            if (iResult != SUCCESS)
            {
                bStatus = false;
                return iResult;
            }

            bStatus = true;
            return SUCCESS;
        }

        public int IsCassetteNone(out bool bStatus)
        {
            int iResult = 0;
            iResult = m_RefComp.Elevator.IsElevatorCassetteNone(out bStatus);
            if (iResult != SUCCESS)
            {
                bStatus = false;
                return iResult;
            }

            bStatus = true;
            return SUCCESS;
        }
    }


}
