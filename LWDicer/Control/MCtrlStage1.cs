using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_MeStage;
using static LWDicer.Control.DEF_CtrlStage;

namespace LWDicer.Control
{
    public class DEF_CtrlStage
    {
        public class CCtrlStage1RefComp
        {
            public IIO IO;
            public MVision Vision;
            public MMeStage Stage;

            public CCtrlStage1RefComp()
            {
            }
            public override string ToString()
            {
                return $"CCtrlStage1RefComp : ";
            }
        }

        public class CCtrlStage1Data
        {
        }

    }

    public class MCtrlStage1 : MCtrlLayer
    {
        private CCtrlStage1RefComp m_RefComp;
        private CCtrlStage1Data m_Data;

        public MCtrlStage1(CObjectInfo objInfo, CCtrlStage1RefComp refComp, CCtrlStage1Data data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CCtrlStage1Data source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CCtrlStage1Data target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetPosition(CUnitPos FixedPos, CUnitPos ModelPos, CUnitPos OffsetPos)
        {
            return m_RefComp.Stage.SetStagePosition(FixedPos, ModelPos, OffsetPos);
        }

        public int SetAlignData(CPos_XYTZ offset)
        {
            return m_RefComp.Stage.SetAlignData(offset);
        }
        // Stage의 상태를 확인하는 동작
        #region Stage 상태 확인
        public int IsWaferDetected(out bool bState)
        {
            return m_RefComp.Stage.IsObjectDetected(out bState);
        }

        public int IsAbsorbed(out bool bState)
        {
            return m_RefComp.Stage.IsAbsorbed(out bState);
        }

        public int IsReleased(out bool bState)
        {
            return m_RefComp.Stage.IsReleased(out bState);
        }

        public int IsClampOpen(out bool bState)
        {
            return m_RefComp.Stage.IsClampOpen(out bState);
        }

        public int IsClampClose(out bool bState)
        {
            return m_RefComp.Stage.IsClampClose(out bState);
        }

        public int IsStageSafetyZone(out bool bState)
        {
            return m_RefComp.Stage.IsStageAxisInSafetyZone(out bState);
        }

        public int IsOrignReturn(out bool bState)
        {
            return m_RefComp.Stage.IsStageOrignReturn(out bState);
        }

        public int CheckForStageMove()
        {
            return m_RefComp.Stage.CheckForStageAxisMove();
        }

        public int CheckForCylMove()
        {
            return m_RefComp.Stage.CheckForStageCylMove();
        }

        public int GetStagePosInfo(out int PosInfo)
        {
            return m_RefComp.Stage.GetStagePosInfo(out PosInfo);
        }

        #endregion
        
        // Stage 구동 관련된 지령
        #region Stage 구동
        
        public void SetCtlModePC()
        {
            int nCtlMode = (int)EStageCtlMode.PC;
            m_RefComp.Stage.SetStageCtlMode(nCtlMode);
        }

        public void SetCtlModeLaser()
        {
            int nCtlMode = (int)EStageCtlMode.LASER;
            m_RefComp.Stage.SetStageCtlMode(nCtlMode);
        }

        public int ClampOpen()
        {
            return m_RefComp.Stage.ClampOpen();
        }

        public int ClampClsoe()
        {
            return m_RefComp.Stage.ClampClose();
        }

        public int MoveToWaitPos(bool bObsolite)
        {
            return m_RefComp.Stage.MoveStageToWaitPos();
        }
        
        public int MoveToLoadPos()
        {
            return m_RefComp.Stage.MoveStageToLoadPos();
        }

        public int MoveToUnloadPos()
        {
            return m_RefComp.Stage.MoveStageToUnloadPos();
        }
        
        public int MoveToEdgeAlignPos1()
        {
            return m_RefComp.Stage.MoveStageToEdgeAlignPos1();
        }

        public int MoveToEdgeAlignPos2()
        {
            return m_RefComp.Stage.MoveStageToEdgeAlignPos2();
        }

        public int MoveToEdgeAlignPos3()
        {
            return m_RefComp.Stage.MoveStageToEdgeAlignPos3();
        }

        public int MoveToEdgeAlignPos4()
        {
            return m_RefComp.Stage.MoveStageToEdgeAlignPos4();
        }

        public int MoveToMacroAlignA()
        {
            return m_RefComp.Stage.MoveStageToMacroAlignA();
        }

        public int MoveToMacroAlignB()
        {
            return m_RefComp.Stage.MoveStageToMacroAlignB();
        }

        public int MoveToMicroAlignA()
        {
            return m_RefComp.Stage.MoveStageToMicroAlignA();
        }

        public int MoveToMicroAlignB()
        {
            return m_RefComp.Stage.MoveStageToMicroAlignB();
        }

        public int MoveToMicroAlignTurnA()
        {
            return m_RefComp.Stage.MoveStageToMicroAlignTurnA();
        }

        public int MoveToMicroAlignTurnB()
        {
            return m_RefComp.Stage.MoveStageToMicroAlignTurnB();
        }

        public int MoveToProcessPos()
        {
            return m_RefComp.Stage.MoveStageToProcessPos();
        }

        public int MoveToProcessTurnPos()
        {
            return m_RefComp.Stage.MoveStageToProcessTurnPos();
        }

        public void MoveIndexPlusX()
        {
            m_RefComp.Stage.MoveStageIndexPlusX();
        }
        public void MoveIndexPlusY()
        {
            m_RefComp.Stage.MoveStageIndexPlusY();
        }

        public void MoveIndexMinusX()
        {
            m_RefComp.Stage.MoveStageIndexMinusX();
        }
        public void MoveIndexMinusY()
        {
            m_RefComp.Stage.MoveStageIndexMinusY();
        }

        public void MoveScreenPlusX()
        {
            m_RefComp.Stage.MoveStageScreenPlusX();
        }
        public void MoveScreenPlusY()
        {
            m_RefComp.Stage.MoveStageScreenPlusY();
        }

        public void MoveScreenMinusX()
        {
            m_RefComp.Stage.MoveStageScreenMinusX();
        }
        public void MoveScreenMinusY()
        {
            m_RefComp.Stage.MoveStageScreenMinusY();
        }

        #endregion

        // Vision 동작
        #region Vision 동작

        #endregion

        // Align 동작
        #region Align 동작

        // Edge Align 동작

        // Macro Align 동작

        // Micro Algin 동작





        #endregion

        // Align 연산
        #region Align 연산



        #endregion
    }
}
