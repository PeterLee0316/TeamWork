using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_MeStage;
using static LWDicer.Layers.DEF_CtrlStage;
using static LWDicer.Layers.DEF_DataManager;
using LWDicer.UI;

using static LWDicer.Layers.MTickTimer.ETimeType;

namespace LWDicer.Layers
{
    public class DEF_CtrlStage
    {

        // VISION 관련 오류
        public const int ERR_CTRLSTAGE_EDGE_POINT_NONE = 1;
        public const int ERR_CTRLSTAGE_EDGE_POINT_OVER = 2;

        public const int ERR_CTRLSTAGE_THETA_POS_UNSUITABLE = 10;

        public const int ERR_CTRLSTAGE_CAM_CALS_CENTERMOVE_FAIL = 20;
        public const int ERR_CTRLSTAGE_MACRO_ALIGN_FAIL = 30;


        public const double CAM_POS_CALS_ROATE_ANGLE = 2.0;
        public const int MACRO_ALIGN_MODE = 0;
        public const int MICRO_ALIGN_MODE_CH1 = 1;
        public const int MICRO_ALIGN_MODE_CH2 = 1;

        public const double MACRO_POS_TOLERANCE = 0.01;
        public const double MACRO_ANGLE_TOLERANCE = 0.001;
        public const double MICRO_POS_TOLERANCE = 0.001;
        public const double MICRO_ANGLE_TOLERANCE = 0.0001;

        public enum EStatgeMode
        {
            TURN,
            RETURN
        }

        public enum EThetaAlignStep
        {
            INIT,
            POS_A,
            POS_B,

        }
        // STAGE 관련 오류

        public class CCtrlStage1RefComp
        {
            public IIO IO;
            public MVision Vision;
            public MMeStage Stage;
            public MMeScannerPolygon Scanner;

            public CCtrlStage1RefComp()
            {
            }
            public override string ToString()
            {
                return $"CCtrlStage1RefComp : ";
            }
        }


        public class CCtrlAlignData
        {
            public CPos_XY WaferPosOffset;

        }
        public class CCtrlStage1Data
        {
            public CSystemData_Align Vision;
            public CSystemData_Light Light;
            public CCtrlAlignData Align;
        }

    }

    public class MCtrlStage1 : MCtrlLayer
    {
        private CCtrlStage1RefComp m_RefComp;
        private CCtrlStage1Data m_Data;
        private int m_iCurrentCam = PRE__CAM;
        private bool bEdgeAlignTeachInit = false;

        private EStatgeMode eStageMode = EStatgeMode.RETURN;
        private EThetaAlignStep eThetaAlignStep = EThetaAlignStep.INIT;

        private MTickTimer m_ProcsTimer = new MTickTimer();

        public MCtrlStage1(CObjectInfo objInfo, CCtrlStage1RefComp refComp, CCtrlStage1Data data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
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

        public override int Initialize()
        {
            return SUCCESS;
        }

        #endregion

        #region Cylinder, Vacuum, Detect Object
        public int IsPanelDetected(out bool bState)
        {
            bState = false;
            return SUCCESS;
        }
        #endregion

        public int SetPosition(CPosition FixedPos, CPosition ModelPos, CPosition OffsetPos)
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
        
        // Laser 가공 Process
        #region Laser Process


        public async void LaserProcessMof()
        {
            int iResult;
            var taskProcess = Task<int>.Run(() => LaserProcessMofRun());

            iResult = await taskProcess;
        }

        public int LaserProcessMofRun()
        {
            return m_RefComp.Scanner.LaserProcess(EScannerMode.MOF);
        }

        public int LaserProcessStep1()
        {
            var originPos  = new CPos_XYTZ();
            var originPos1 = new CPos_XYTZ();
            var originPos2 = new CPos_XYTZ();
            var stepPitch = new CPos_XYTZ();
            var patternPitch = new CPos_XYTZ();
            var patternMove = new CPos_XYTZ();
            int nStepCount = 0, nPatternCount=0;

            // Paterrn 1 Data 설정
            nStepCount = CMainFrame.DataManager.ModelData.ProcData.ProcessCount1;
            nPatternCount = CMainFrame.DataManager.ModelData.ProcData.PatternCount1;
            stepPitch.dX = -(double)CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX1;
            stepPitch.dY = -(double)CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY1;
            patternPitch.dX = 0.0;
            patternPitch.dY = (double)CMainFrame.DataManager.ModelData.ProcData.PatternPitch1;

            // 현재 지령 위치 저장
            m_RefComp.Stage.GetStageCmdPos(out originPos);

            // Pattern 위치로 Move
            originPos1.dX = originPos.dX;
            originPos1.dY = originPos.dY - (double)CMainFrame.DataManager.ModelData.ProcData.PatternOffset1;
            m_RefComp.Stage.MoveStagePos(originPos1);

            // Pattern 1 Process ===================================================================
            

            // Bmp & Config.ini File Download
            m_RefComp.Scanner.SendConfig("T:\\SFA\\LWDicer\\ScannerData\\config_job1.ini");
            m_RefComp.Scanner.SendBitmap("T:\\SFA\\LWDicer\\ImageData\\image_job1.bmp");

            // Marking Process (Step & Go)
            for (int i = 0; i < nPatternCount; i++)
            {
                m_ProcsTimer.StartTimer();

                for (int j = 0; j < nStepCount; j++)
                {
                    // Laser Process                    
                    m_RefComp.Scanner.LaserProcess(EScannerMode.STILL);
                                       
                    // Step Move
                    if (j >= (nStepCount - 1)) continue;
                    MoveStageRelative(stepPitch, true);
                }
                // Pattern Move
                patternMove.dX = originPos1.dX - patternPitch.dX * (i + 1);
                patternMove.dY = originPos1.dY - patternPitch.dY * (i + 1);

                if (i >= (nPatternCount - 1)) continue;
                m_RefComp.Stage.MoveStagePos(patternMove);

                while (m_ProcsTimer.LessThan(CMainFrame.DataManager.ModelData.ProcData.ProcessInterval, MTickTimer.ETimeType.SECOND))
                {
                    Sleep(100);
                }
                m_ProcsTimer.StopTimer();

            }

            // Pattern 위치로 Move
            originPos2.dX = originPos.dX;
            originPos2.dY = originPos.dY - (double)CMainFrame.DataManager.ModelData.ProcData.PatternOffset2;
            m_RefComp.Stage.MoveStagePos(originPos2);


            // Paterrn 1 Data 설정
            nStepCount = CMainFrame.DataManager.ModelData.ProcData.ProcessCount2;
            nPatternCount = CMainFrame.DataManager.ModelData.ProcData.PatternCount2;
            stepPitch.dX = -(double)CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX2;
            stepPitch.dY = -(double)CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY2;
            patternPitch.dX = 0.0;
            patternPitch.dY = (double)CMainFrame.DataManager.ModelData.ProcData.PatternPitch2;

            // Pattern 2 Process ===================================================================

            // Bmp & Config.ini File Download
            m_RefComp.Scanner.SendConfig("T:\\SFA\\LWDicer\\ScannerData\\config_job2.ini");
            m_RefComp.Scanner.SendBitmap("T:\\SFA\\LWDicer\\ImageData\\image_job2.bmp");

            // Marking Process (Step & Go)
            for (int i = 0; i < nPatternCount; i++)
            {
                m_ProcsTimer.StartTimer();

                for (int j = 0; j < nStepCount; j++)
                {
                    // Laser Process
                    m_RefComp.Scanner.LaserProcess(EScannerMode.STILL);

                    // Step Move
                    if (j >= (nStepCount - 1)) continue;
                    MoveStageRelative(stepPitch, true);
                }
                // Pattern Move
                patternMove.dX = originPos2.dX - patternPitch.dX * (i + 1);
                patternMove.dY = originPos2.dY - patternPitch.dY * (i + 1);

                if (i >= (nPatternCount - 1)) continue;
                m_RefComp.Stage.MoveStagePos(patternMove);

                while (m_ProcsTimer.LessThan(CMainFrame.DataManager.ModelData.ProcData.ProcessInterval, MTickTimer.ETimeType.SECOND))
                {
                    Sleep(100);
                }
                m_ProcsTimer.StopTimer();
            }

            // 초기 위치로 이동함
            m_RefComp.Stage.MoveStagePos(originPos);

            return SUCCESS;
        }

        public async Task<int> LaserProcessStep2()
        {
            // 비동기 방식으로 프로세스를 진행.
           // var taskProcess = Task<int>.Run(() => LaserProcessOneByOne());
            int iResult = await LaserProcessOneByOne();

            return iResult;
                        
        }
        private async Task<int> LaserProcessOneByOne()
        {
            var originPos = new CPos_XYTZ();
            var originPos1 = new CPos_XYTZ();
            var originPos2 = new CPos_XYTZ();
            var stepPitch = new CPos_XYTZ();
            var patternPitch = new CPos_XYTZ();
            var patternMove = new CPos_XYTZ();
            int nStepCount = 0, nPatternCount = 0;

            nPatternCount = CMainFrame.DataManager.ModelData.ProcData.PatternCount1;

            // 현재 지령 위치 저장
            m_RefComp.Stage.GetStageCmdPos(out originPos);

            for (int i = 0; i < nPatternCount; i++)
            {
                // Process정지 수행
                if (CMainFrame.DataManager.ModelData.ProcData.ProcessStop) goto ProcessStop;

                // Paterrn 1 Data 설정
                nStepCount = CMainFrame.DataManager.ModelData.ProcData.ProcessCount1;
                stepPitch.dX = -(double)CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX1;
                stepPitch.dY = -(double)CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY1;
                patternPitch.dX = 0.0;
                patternPitch.dY = (double)CMainFrame.DataManager.ModelData.ProcData.PatternPitch1;

                // Pattern 위치로 Move
                originPos1.dX = originPos.dX;
                originPos1.dY = originPos.dY - (double)CMainFrame.DataManager.ModelData.ProcData.PatternOffset1
                                             - patternPitch.dY * (i);
                m_RefComp.Stage.MoveStagePos(originPos1);

                // Pattern 1 Process ===================================================================
                m_ProcsTimer.StartTimer();

                // Bmp & Config.ini File Download
                m_RefComp.Scanner.SendConfig("T:\\SFA\\LWDicer\\ScannerData\\config_job1.ini");
                m_RefComp.Scanner.SendBitmap("T:\\SFA\\LWDicer\\ImageData\\image_job1.bmp");
                
                for (int j = 0; j < nStepCount; j++)
                {
                    // Process정지 수행
                    if (CMainFrame.DataManager.ModelData.ProcData.ProcessStop) goto ProcessStop;

                    // Laser Process
                    m_RefComp.Scanner.LaserProcess(EScannerMode.STILL);

                    // Step Move
                    if (j >= (nStepCount - 1)) continue;
                    MoveStageRelative(stepPitch, true);
                }

                // Process정지 수행
                if (CMainFrame.DataManager.ModelData.ProcData.ProcessStop) goto ProcessStop;

                // 2nd Pattern Move
                originPos2.dX = originPos.dX;
                originPos2.dY = originPos.dY - (double)CMainFrame.DataManager.ModelData.ProcData.PatternOffset2
                                              - patternPitch.dY * (i); 
                m_RefComp.Stage.MoveStagePos(originPos2);


                // Paterrn 1 Data 설정
                nStepCount = CMainFrame.DataManager.ModelData.ProcData.ProcessCount2;
                nPatternCount = CMainFrame.DataManager.ModelData.ProcData.PatternCount2;
                stepPitch.dX = -(double)CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetX2;
                stepPitch.dY = -(double)CMainFrame.DataManager.ModelData.ProcData.ProcessOffsetY2;
                patternPitch.dX = 0.0;
                patternPitch.dY = (double)CMainFrame.DataManager.ModelData.ProcData.PatternPitch2;

                // Pattern 2 Process ===================================================================

                // Bmp & Config.ini File Download
                m_RefComp.Scanner.SendConfig("T:\\SFA\\LWDicer\\ScannerData\\config_job2.ini");
                m_RefComp.Scanner.SendBitmap("T:\\SFA\\LWDicer\\ImageData\\image_job2.bmp");

                for (int j = 0; j < nStepCount; j++)
                {
                    // Process정지 수행
                    if (CMainFrame.DataManager.ModelData.ProcData.ProcessStop) goto ProcessStop;

                    // Laser Process
                    m_RefComp.Scanner.LaserProcess(EScannerMode.STILL);

                    // Step Move
                    if (j >= (nStepCount - 1)) continue;
                    MoveStageRelative(stepPitch, true);
                }

                while(m_ProcsTimer.LessThan(CMainFrame.DataManager.ModelData.ProcData.ProcessInterval,MTickTimer.ETimeType.SECOND))
                {
                    // Process정지 수행
                    if (CMainFrame.DataManager.ModelData.ProcData.ProcessStop) goto ProcessStop;

                    //Sleep(100);
                    await Task.Delay(100);                   
                }

                m_ProcsTimer.StopTimer();

            }

            // 초기 위치로 이동함
            m_RefComp.Stage.MoveStagePos(originPos);
            CMainFrame.DataManager.ModelData.ProcData.ProcessStop = false; ;

            return SUCCESS;


            ProcessStop:
            // 초기 위치로 이동함
            m_RefComp.Stage.MoveStagePos(originPos);
            CMainFrame.DataManager.ModelData.ProcData.ProcessStop = false; 
            m_ProcsTimer.StopTimer();

            return SUCCESS;

        }

        #endregion

        // Stage 위치 구동 지령
        #region Stage 구동


        public int ClampOpen()
        {
            return m_RefComp.Stage.ClampOpen();
        }

        public int ClampClose()
        {
            return m_RefComp.Stage.ClampClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bDir"></param>
        /// <returns></returns>
        public int MoveStageRelative(int iPos,bool bDir=true)
        {
            CPos_XYTZ sTargetPos = new CPos_XYTZ();
            CPosition fixedPos;
            CPosition modelPos;
            CPosition offsetPos;
            m_RefComp.Stage.GetStagePosition(out fixedPos, out modelPos,out offsetPos);

            if (bDir)
                sTargetPos = fixedPos.Pos[iPos];
            else
            {
                sTargetPos.dX = -fixedPos.Pos[iPos].dX;
                sTargetPos.dY = -fixedPos.Pos[iPos].dY;
                sTargetPos.dT = -fixedPos.Pos[iPos].dT;
            }
            return m_RefComp.Stage.MoveStageRelativeXYT(sTargetPos);            
        }

        public int MoveStageRelative(CPos_XYTZ sTargetPos, bool bDir = true)
        {            
            if (bDir==false)
            { 
                sTargetPos.dX = -sTargetPos.dX;
                sTargetPos.dY = -sTargetPos.dY;
                sTargetPos.dT = -sTargetPos.dT;
            }
            return m_RefComp.Stage.MoveStageRelativeXYT(sTargetPos);
        }
        public int MoveStageRelativeX(double sPos, bool bDir = true)
        {
            var movePos = new CPos_XYTZ();
            movePos.dX = bDir? sPos : -sPos;

            return m_RefComp.Stage.MoveStageRelativeXYT(movePos);
        }
        public int MoveStageRelativeY(double sPos, bool bDir = true)
        {
            var movePos = new CPos_XYTZ();
            movePos.dY = bDir ? sPos : -sPos;

            return m_RefComp.Stage.MoveStageRelativeXYT(movePos);
        }
        public int MoveStageRelativeT(double sPos, bool bDir = true)
        {
            var movePos = new CPos_XYTZ();
            movePos.dT = bDir ? sPos : -sPos;

            return m_RefComp.Stage.MoveStageRelativeXYT(movePos);
        }

        public int MoveToStageTurn()
        {
            // Theta Align한 dT 값을 읽음
            int iResult;
            var thetaPos = new CPos_XYTZ();
            m_RefComp.Stage.GetThetaAlignPosA(out thetaPos);
            // 현재 위치를 읽음
            var curPos = new CPos_XYTZ();
            m_RefComp.Stage.GetStageCurPos(out curPos);

            var movePos = new CPos_XYTZ();

            // 현재 위치에서 dT축만 Theta Align 값에서 DieIndex의 회전 값만 적용
            movePos = curPos;
            movePos.dT = thetaPos.dT + CMainFrame.DataManager.SystemData_Align.DieIndexRotate;
            
            iResult=  m_RefComp.Stage.MoveStagePos(movePos);
            if(iResult == SUCCESS) eStageMode = EStatgeMode.TURN;
            return iResult;
        }

        public int MoveToStageReturn()
        {
            // Theta Align한 dT 값을 읽음
            int iResult;
            var thetaPos = new CPos_XYTZ();
            m_RefComp.Stage.GetThetaAlignPosA(out thetaPos);
            // 현재 위치를 읽음
            var curPos = new CPos_XYTZ();
            m_RefComp.Stage.GetStageCurPos(out curPos);

            var movePos = new CPos_XYTZ();

            // 현재 위치에서 dT축만 Theta Align 값으로
            movePos = curPos;
            movePos.dT = thetaPos.dT;
            
            iResult = m_RefComp.Stage.MoveStagePos(movePos);
            if (iResult == SUCCESS) eStageMode = EStatgeMode.RETURN;
            return iResult;
        }

        public int MoveToStageWaitPos()
        {
            return m_RefComp.Stage.MoveStageToWaitPos();
        }
        
        public int MoveToStageLoadPos()
        {
            return m_RefComp.Stage.MoveStageToLoadPos();
        }

        public int MoveToStageUnloadPos()
        {
            return m_RefComp.Stage.MoveStageToUnloadPos();
        }

        public int MoveToStageCenter()
        {
            int iResult;
            if(GetCurrentCam()==PRE__CAM)
                iResult =  m_RefComp.Stage.MoveStageToStageCenter();
            else
                iResult =  m_RefComp.Stage.MoveStageToStageCenter(true);

            return iResult;
                        
        }

        public int MoveToThetaAlignPosA()
        {
            int iResult;
            
            if(GetCurrentCam()==PRE__CAM)
                iResult = m_RefComp.Stage.MoveStageToThetaAlignPosA(true);
            else
                iResult = m_RefComp.Stage.MoveStageToThetaAlignPosA();

            if (iResult==SUCCESS) eStageMode = EStatgeMode.RETURN;

            return iResult;
        }        

        public int MoveToThetaAlignPosB()
        {
            int iResult;

            if (GetCurrentCam() == PRE__CAM)
                iResult = m_RefComp.Stage.MoveStageToThetaAlignPosB(true);
            else
                iResult = m_RefComp.Stage.MoveStageToThetaAlignPosB();

            if (iResult == SUCCESS) eStageMode = EStatgeMode.RETURN;

            return iResult;

        }

        public int MoveToThetaAlignTurnPosA()
        {
            int iResult;

            if (GetCurrentCam() == PRE__CAM)
                iResult = m_RefComp.Stage.MoveStageToThetaAlignTurnPosA(true);
            else
                iResult = m_RefComp.Stage.MoveStageToThetaAlignTurnPosA();

            if (iResult == SUCCESS) eStageMode = EStatgeMode.TURN;

            return iResult;
        }

        public int MoveToThetaAlignTurnPosB()
        {
            int iResult;

            if (GetCurrentCam() == PRE__CAM)
                iResult = m_RefComp.Stage.MoveStageToThetaAlignTurnPosB();
            else
                iResult = m_RefComp.Stage.MoveStageToThetaAlignTurnPosB(true);

            if (iResult == SUCCESS) eStageMode = EStatgeMode.TURN;

            return iResult;
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

        public int MoveToMacroCam()
        {
            return m_RefComp.Stage.MoveStageToMacroCam();
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

        public int MoveIndexPlusX()
        {
            int iResult;

            if (eStageMode == EStatgeMode.RETURN)
                iResult = m_RefComp.Stage.MoveStageIndexPlusX();
            else
                iResult = m_RefComp.Stage.MoveStageIndexPlusX(true);

            return iResult;
        }
        public int MoveIndexPlusY()
        {
            int iResult;

            if (eStageMode == EStatgeMode.RETURN)
                iResult = m_RefComp.Stage.MoveStageIndexPlusY();
            else
                iResult = m_RefComp.Stage.MoveStageIndexPlusY(true);

            return iResult;
        }
        public int MoveIndexPlusT()
        {
            return m_RefComp.Stage.MoveStageIndexPlusT();
        }
        public int MoveIndexMinusX()
        {
            int iResult;

            if (eStageMode == EStatgeMode.RETURN)
                iResult = m_RefComp.Stage.MoveStageIndexMinusX();
            else
                iResult = m_RefComp.Stage.MoveStageIndexMinusX(true);

            return iResult;
        }
        public int MoveIndexMinusY()
        {
            int iResult;

            if (eStageMode == EStatgeMode.RETURN)
                iResult = m_RefComp.Stage.MoveStageIndexMinusY();
            else
                iResult = m_RefComp.Stage.MoveStageIndexMinusY(true);

            return iResult;
        }
        
        public int MoveIndexMinusT()
        {
            return m_RefComp.Stage.MoveStageIndexMinusT();
        }
        public int MoveMacroScreenPlusX()
        {
            return m_RefComp.Stage.MoveStageScreenPlusX(ECameraSelect.MACRO);
        }

        public int MoveMacroScreenPlusY()
        {
            return m_RefComp.Stage.MoveStageScreenPlusY(ECameraSelect.MACRO);
        }

        public int MoveMacroScreenPlusT()
        {
            return m_RefComp.Stage.MoveStageScreenPlusT(ECameraSelect.MACRO);
        }
        public int MoveMacroScreenMinusX()
        {
            return m_RefComp.Stage.MoveStageScreenMinusX(ECameraSelect.MACRO);
        }
        public int MoveMacroScreenMinusY()
        {
            return m_RefComp.Stage.MoveStageScreenMinusY(ECameraSelect.MACRO);
        }
        public int MoveMacroScreenMinusT()
        {
            return m_RefComp.Stage.MoveStageScreenMinusT(ECameraSelect.MACRO);
        }
        public int MoveMicroScreenPlusX()
        {
            return m_RefComp.Stage.MoveStageScreenPlusX(ECameraSelect.MICRO);
        }

        public int MoveMicroScreenPlusY()
        {
            return m_RefComp.Stage.MoveStageScreenPlusY(ECameraSelect.MICRO);
        }

        public int MoveMicroScreenPlusT()
        {
            return m_RefComp.Stage.MoveStageScreenPlusT(ECameraSelect.MICRO);
        }
        public int MoveMicroScreenMinusX()
        {
            return m_RefComp.Stage.MoveStageScreenMinusX(ECameraSelect.MICRO);
        }
        public int MoveMicroScreenMinusY()
        {
            return m_RefComp.Stage.MoveStageScreenMinusY(ECameraSelect.MICRO);
        }
        public int MoveMicroScreenMinusT()
        {
            return m_RefComp.Stage.MoveStageScreenMinusT(ECameraSelect.MICRO);
        }
        public int JogMovePlusX(bool IsFastMove)
        {
            return m_RefComp.Stage.JogStagePlusX(IsFastMove);
        }
        public int JogMoveMinusX(bool IsFastMove)
        {
            return m_RefComp.Stage.JogStageMinusX(IsFastMove);
        }

        public int JogMovePlusY(bool IsFastMove)
        {
            return m_RefComp.Stage.JogStagePlusY(IsFastMove);
        }
        public int JogMoveMinusY(bool IsFastMove)
        {
            return m_RefComp.Stage.JogStageMinusY(IsFastMove);
        }

        public int JogMovePlusT(bool IsFastMove)
        {
            return m_RefComp.Stage.JogStagePlusT(IsFastMove);
        }
        public int JogMoveMinusT(bool IsFastMove)
        {
            return m_RefComp.Stage.JogStageMinusT(IsFastMove);
        }

        public int JogStageStop(int Axis)
        {
            return m_RefComp.Stage.JogStageStop(Axis);
        }

        #endregion

        // Camera 위치 구동 지령
        #region Camera 구동

        public int MoveToCameraWaitPos()
        {
            return m_RefComp.Stage.MoveCameraToWaitPos();
        }

        public int MoveToCameraWorkPos()
        {
            return m_RefComp.Stage.MoveCameraToWorkPos();
        }

        public int MoveToCameraFocusPos1()
        {
            return m_RefComp.Stage.MoveCameraToFocusPos1();
        }

        public int MoveToCameraFocusPos2()
        {
            return m_RefComp.Stage.MoveCameraToFocusPos2();
        }

        public int MoveToCameraFocusPos3()
        {
            return m_RefComp.Stage.MoveCameraToFocusPos3();
        }

        public int MoveCameraJog(bool bDir, bool IsFast)
        {
            return m_RefComp.Stage.MoveCameraJog(bDir, IsFast);
        }

        public int CameraJogStop()
        {
            return m_RefComp.Stage.JogCameraStop();
        }


        #endregion

        // Scanner 위치 구동 지령
        #region Scanner 구동

        public int MoveToScannerWaitPos()
        {
            return m_RefComp.Stage.MoveScannerToWaitPos();
        }

        public int MoveToScannerWorkPos()
        {
            return m_RefComp.Stage.MoveScannerToWorkPos();
        }

        public int MoveToScannerFocusPos1()
        {
            return m_RefComp.Stage.MoveScannerToFocusPos1();
        }

        public int MoveToScannerFocusPos2()
        {
            return m_RefComp.Stage.MoveScannerToFocusPos2();
        }

        public int MoveToScannerFocusPos3()
        {
            return m_RefComp.Stage.MoveScannerToFocusPos3();
        }

        #endregion

        // Vision 동작
        #region Vision 동작

        /// <summary>
        /// Wafer의 Edge를 찾는다.
        /// Macro Len(Pre Cam)에서만 사용한다.
        /// </summary>
        /// <param name="sPos"></param>
        /// <returns></returns>
        public int FindEdgePoint(out CPos_XY sPos)
        {
            int iCam = PRE__CAM;
            int iResult = 0;
            CPos_XY pPos = new CPos_XY();
            sPos = pPos;

            // Edge 검색
            CEdgeData pEdgeData;
            iResult = m_RefComp.Vision.FindEdge(iCam, out pEdgeData);

            if (iResult != SUCCESS) return iResult;

            // 결과 확인            
            if (pEdgeData.m_iEdgeNum > 1)
            {
                // 중복적으로 Edge가 검출될때는 Error를 리턴한다.
                return GenerateErrorCode(ERR_CTRLSTAGE_EDGE_POINT_OVER);
            }
            // Edge 위치 결과 대입
            pPos = pEdgeData.EdgePos[0];

            // Camera의 틀어짐 보정
            CPos_XY mCenter = new CPos_XY(); // 회전 중심은 (0,0)으로 한다
            pPos = CoordinateRotate(m_Data.Vision.Camera[iCam].CameraTilt, pPos, mCenter);

            // Pixel to Pos
            pPos = PixelToPostion(PRE__CAM, pPos);

            sPos = pPos;
            return SUCCESS;
        }

        public int FindMacroMark(out CPos_XY sPos)
        {
            int iCam = PRE__CAM;
            int iModelNo = PATTERN_A;
            int iResult = 0;
            sPos = new CPos_XY();

            // Find Mark 
            CResultData pResult;
            iResult = m_RefComp.Vision.RecognitionPatternMark(iCam, iModelNo, out pResult);
            if (iResult != SUCCESS) return iResult;

            // Camera의 틀어짐 보정
            CPos_XY mCenter = new CPos_XY(); // 회전 중심은 (0,0)으로 한다
            sPos = CoordinateRotate(m_Data.Vision.Camera[iCam].CameraTilt, sPos, mCenter);
            // Pixel값을 실제 위치값으로 변환한다.
            sPos = PixelToPostion(iCam,pResult.m_PixelPos);

            return SUCCESS;
        }

        public int FindMicroMarkA(out CPos_XY sPos)
        {
            int iCam = FINE_CAM;
            int iModelNo = PATTERN_A;
            int iResult = 0;
            sPos = new CPos_XY();

            // Find Mark 
            CResultData pResult;
            iResult = m_RefComp.Vision.RecognitionPatternMark(iCam, iModelNo, out pResult);
            if (iResult != SUCCESS) return iResult;

            // Camera의 틀어짐 보정
            CPos_XY mCenter = new CPos_XY(); // 회전 중심은 (0,0)으로 한다
            sPos = CoordinateRotate(m_Data.Vision.Camera[iCam].CameraTilt, sPos, mCenter);
            // Pixel값을 실제 위치값으로 변환한다.
            sPos = PixelToPostion(iCam, pResult.m_PixelPos);

            return SUCCESS;
        }

        public int FindMicroMarkB(out CPos_XY sPos)
        {
            int iCam = FINE_CAM;
            int iModelNo = PATTERN_B;
            int iResult = 0;
            sPos = new CPos_XY();

            // Find Mark 
            CResultData pResult;
            iResult = m_RefComp.Vision.RecognitionPatternMark(iCam, iModelNo, out pResult);
            if (iResult != SUCCESS) return iResult;

            // Camera의 틀어짐 보정
            CPos_XY mCenter = new CPos_XY(); // 회전 중심은 (0,0)으로 한다
            sPos = CoordinateRotate(m_Data.Vision.Camera[iCam].CameraTilt, sPos, mCenter);
            // Pixel값을 실제 위치값으로 변환한다.
            sPos = PixelToPostion(iCam, pResult.m_PixelPos);

            return SUCCESS;
        }
        
        // Hair Line
        public void ShowHairLine()
        {
            m_RefComp.Vision.ShowHairLine();
        }

        public void NarrowHairLine()
        {
            m_RefComp.Vision.NarrowHairLine();
        }

        public void WidenHairLine()
        {
            m_RefComp.Vision.WidenHairLine();
        }
           

        #endregion

        // 회전 변환 및 Calibration 관련 함수
        #region Calculation Function

        private CPos_XY PixelToPostion(int iCam,CPos_XY sPos)
        {
            CPos_XY pPos = new CPos_XY();

            pPos.dX = sPos.dX * m_Data.Vision.Camera[iCam].PixelResolutionX;
            pPos.dY = sPos.dY * m_Data.Vision.Camera[iCam].PixelResolutionY;

            // 카메라 위치값을 더하여 실제 좌표의 위치를 나타냄.
            pPos += m_Data.Vision.Camera[iCam].Position;

            return pPos;
        }        

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

        /// <summary>
        /// 좌표 회전 변환 함수 , Degree단위를 사용한다.
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="Rotate" Degree 단위..></param>
        /// <returns></returns>
        private CPos_XY CoordinateRotate(double dAngle, CPos_XY sPos, CPos_XY sCenter)
        {
            var mPos = new CPos_XY();
            if (dAngle == 0.0) return sPos;

            // 라디안 값으로 각도 변경
            var RadAngle = DegToRad(dAngle);
            // 회전 변환 ( sCenter 기준으로 변환)
            mPos.dX =  (sPos.dX - sCenter.dX) * Math.Cos(RadAngle) - (sPos.dY - sCenter.dY) * Math.Sin(RadAngle) + sCenter.dX;
            mPos.dY =  (sPos.dX - sCenter.dX) * Math.Sin(RadAngle) + (sPos.dY - sCenter.dY) * Math.Cos(RadAngle) + sCenter.dY;
            
            return mPos;
        }

        private CPos_XY CoordinateRotate(double dAngle, CPos_XY sPos, CPos_XYTZ sCenter)
        {
            var mPos = new CPos_XY();
            if (dAngle == 0.0) return sPos;

            // 라디안 값으로 각도 변경
            var RadAngle = DegToRad(dAngle);
            // 회전 변환 ( sCenter 기준으로 변환)
            mPos.dX = (sPos.dX - sCenter.dX) * Math.Cos(RadAngle) - (sPos.dY - sCenter.dY) * Math.Sin(RadAngle) + sCenter.dX;
            mPos.dY = (sPos.dX - sCenter.dX) * Math.Sin(RadAngle) + (sPos.dY - sCenter.dY) * Math.Cos(RadAngle) + sCenter.dY;

            return mPos;
        }

        private CPos_XYTZ CoordinateRotate(double dAngle, CPos_XYTZ sPos, CPos_XYTZ sCenter)
        {
            var mPos = new CPos_XYTZ();
            if (dAngle == 0.0) return sPos;

            // 라디안 값으로 각도 변경
            var RadAngle = DegToRad(dAngle);
            // 회전 변환 ( sCenter 기준으로 변환)
            mPos.dX = (sPos.dX - sCenter.dX) * Math.Cos(RadAngle) - (sPos.dY - sCenter.dY) * Math.Sin(RadAngle) + sCenter.dX;
            mPos.dY = (sPos.dX - sCenter.dX) * Math.Sin(RadAngle) + (sPos.dY - sCenter.dY) * Math.Cos(RadAngle) + sCenter.dY;

            return mPos;
        }


        /// <summary>
        /// 임의의 2점의 선을 수평 기준으로 각도를 구한다.
        /// </summary>
        /// <param name="pMinusPos"></param>
        /// <param name="pPlusPos"></param>
        /// <returns></returns>
        private double CalsRotateAngle(CPos_XY pLeftPos, CPos_XY pRightPos)
        {
            double RotateAngle = 0.0;
            if (pRightPos == pLeftPos) return RotateAngle;
            
            // 각도 계산 
            double dWidth  = pRightPos.dX - pLeftPos.dX;
            double dHeight = pRightPos.dY - pLeftPos.dY;
            RotateAngle = Math.Atan(dHeight / dWidth);

            // Degree 값으로 변환
            RotateAngle = RadToDeg(RotateAngle);

            return RotateAngle;
        }

        private double CalsRotateAngle(double dWidth, double dHeight)
        {
            double RotateAngle = 0.0;
            if (dHeight == 0.0 || dWidth ==0) return RotateAngle;
            
            RotateAngle = Math.Atan(dHeight / dWidth);

            // Degree 값으로 변환
            RotateAngle = RadToDeg(RotateAngle);

            return RotateAngle;
        }

        /// <summary>
        /// 실제 움직은 거리에 카메라에서 Pixel의 이동 거리를 비교하여 Resolution을 구함.
        /// </summary>
        /// <param name="MoveDistance"></param>
        /// <param name="pMinusPixel"></param>
        /// <param name="pPlusPixel"></param>
        /// <returns></returns>
        private double CalsPixelResolution(double MoveDistance,CPos_XY pMinusPixel, CPos_XY pPlusPixel)
        {
            double PixelResolution = 0.0;
            double dWidth  = pPlusPixel.dX - pMinusPixel.dX;
            double dHeight = pPlusPixel.dY - pMinusPixel.dY;
            // Pixel 간의 거리를 계산한다.
            double PixelDistance = Math.Sqrt(dWidth * dWidth + dHeight * dHeight);

            // 실제 이동 거리를 Pixel간의 거리로 나누어... 
            // 한 Pixel 당 움직이는 거리 PixelResolution을 구한다.
            PixelResolution = MoveDistance / PixelDistance;

            return PixelResolution;
        }

        /// <summary>
        /// 일정 각도를 회전하고 2개의 Mark값을 확인하여
        /// 회전 중심을 구하는 함수
        /// 각도 단위는: Degree, 이동 좌표값은 : mm 단위 사용함.
        /// </summary>
        /// <param name="dAngle"></param>
        /// <param name="pMinusPos"></param>
        /// <param name="pPlusPos"></param>
        /// <returns></returns>
        private CPos_XY CalsRotateCenter(double dAngle, CPos_XY pMinusPos, CPos_XY pPlusPos)
        {
            var mPos = new CPos_XY();
            if (dAngle == 0.0) return mPos;

            var mPos1 = pMinusPos;
            var mPos2 = pPlusPos;
            // 라디안 값으로 각도 변경
            var RadAngle = DegToRad(dAngle);

            // 회전 중심 구하는 식
            mPos.dX = (mPos1.dX + mPos2.dX)/2 + (     Math.Sin(RadAngle)  * (mPos1.dY - mPos2.dY)) / ( 2 * ( 1 - Math.Cos(RadAngle)));
            mPos.dY = (mPos1.dY + mPos2.dY)/2 + ((1 + Math.Cos(RadAngle)) * (mPos2.dX - mPos1.dX)) / ( 2 *       Math.Sin(RadAngle));

            return mPos;
        }



        #endregion

        // Data 설정 동작 
        #region Data Set
        /// <summary>
        /// 카메라 위치를 좌표에 설정한다.
        /// </summary>
        /// <param name="iCam"></param>
        /// <param name="pCamPos"></param>
        public void SetCameraPos(int iCam, CPos_XY pCamPos)
        {
            m_Data.Vision.Camera[iCam].Position= pCamPos;
        }
        public CPos_XY GetCameraPos(int iCam)
        {
            return m_Data.Vision.Camera[iCam].Position;
        }

        /// <summary>
        /// Index 데이터 설정
        /// </summary>
        /// <param name="dIndexLen"></param>



        #endregion

        // Calibration  및 매뉴얼 동작 
        #region Calibration

        // 카메라 배율 변경
        public int ChangeVisionMagnitude(int iCam, IntPtr pHandle)
        {

#if SIMULATION_VISION
            return SUCCESS;
#endif
            int iResult = -1;            

            if (iCam == FINE_CAM)
            {
                iResult = m_RefComp.Stage.MoveChangeMacroCam();
                if (iResult != SUCCESS) return iResult;
                iResult = m_RefComp.Vision.DestroyLocalView(PRE__CAM);                
                if (iResult != SUCCESS) return iResult;
                iResult = m_RefComp.Vision.InitialLocalView(FINE_CAM, pHandle);
                if (iResult != SUCCESS) return iResult;
                m_iCurrentCam = FINE_CAM;
            }
            if (iCam == PRE__CAM)
            {
                iResult = m_RefComp.Stage.MoveChangeMicroCam();
                if (iResult != SUCCESS) return iResult;
                iResult = m_RefComp.Vision.DestroyLocalView(FINE_CAM);
                iResult = m_RefComp.Vision.InitialLocalView(PRE__CAM, pHandle);
                if (iResult != SUCCESS) return iResult;
                if (iResult != SUCCESS) return iResult;
                m_iCurrentCam = PRE__CAM;
            }

            m_RefComp.Vision.ShowHairLine();

            return SUCCESS;
        }

        public int ChangeMacroVision(IntPtr pHandle)
        {
            return ChangeVisionMagnitude(PRE__CAM, pHandle);
        }

        public int ChangeMicroVision(IntPtr pHandle)
        {
            return ChangeVisionMagnitude(FINE_CAM, pHandle);
        }

        public int GetCurrentCam()
        {

#if SIMULATION_VISION
            return SUCCESS;
#endif
            return m_RefComp.Vision.m_iCurrentViewNum;

        }

        public bool CheckMicroCam()
        {
            if (GetCurrentCam() == FINE_CAM) return true;
            else return false;
        }

        public bool CheckMacroCam()
        {
            if (GetCurrentCam() == PRE__CAM) return true;
            else return false;
        }

        // 카메라 위치 계산 및 설정
        // Fine 카메라만 자동으로 연산함
        // Pre 카메라는 Fine과 차이를 수동으로 설정함.
        public int AutoTeachMicroCamPos()
        {
            // Micro Key Mark가 패턴 등록이 되어 있어야 함.
            // Key Mark 기준으로 Theta Align이 되어 있어야 함.
            // ??? 조건문 확인

            CPos_XYTZ CurPos = new CPos_XYTZ();
            CPos_XY StagePos = new CPos_XY();
            CPos_XY CamPos = new CPos_XY();
            CPos_XY MarkPosA = new CPos_XY();
            CPos_XY MarkPosB = new CPos_XY();
            CPos_XY TransPos = new CPos_XY();
            CPos_XYTZ StageMovePos = new CPos_XYTZ();
            CResultData MarkData = new CResultData();

            int iResult = 0;
            int iCount = 0;

            /////////////////////////////////////////////////////////////////////
            //  현재 Mark의 위치를 카메라의 중심으로 맞춘다.

            // Mark 확인하여.. 영상의 중심으로 Mark를 맞춘다.
            do
            {
                // 처음에는 Stage를 움직이지 않는다 (Mark 위치를 모름)
                if (iCount > 0)
                {
                    iResult = m_RefComp.Stage.MoveStageRelativeXYT(StageMovePos);
                    if (iResult != SUCCESS) return iResult;
                }

                Sleep(500);
            
                // Mark  위치 확인
                iResult = FindMicroMarkA(out MarkPosA);
                if (iResult != SUCCESS) return iResult;

                // 위치 보정값 대입
                StageMovePos.dX = -MarkPosA.dX;
                StageMovePos.dY = -MarkPosA.dY;                

                // 5회 반복이 넘을 경우 Fail
                if(iCount > 5) return GenerateErrorCode(ERR_CTRLSTAGE_CAM_CALS_CENTERMOVE_FAIL);
                
                iCount++;                

            } while (Math.Abs(MarkPosA.dX) < 0.001 && Math.Abs(MarkPosA.dY) < 0.001);
            

            // 현재 Stage 위치 읽어오기
            iResult = m_RefComp.Stage.GetStageCurPos(out CurPos);
            if (iResult != SUCCESS) return iResult;

            StagePos.dX = CurPos.dX;
            StagePos.dY = CurPos.dY;

            // Key Mark가 Micro Cam의 Center일때 Stage 위치 저장
            iResult = m_RefComp.Stage.SetMicroCamPos(CurPos);
            if (iResult != SUCCESS) return iResult;
            /////////////////////////////////////////////////////////////////////
            //  회전 이동하며, Mark 2 Point를 확인함

            // Screen Index Theta + 이동
            iResult = m_RefComp.Stage.MoveStageScreenPlusT(ECameraSelect.MICRO);
            if (iResult != SUCCESS) return iResult;

            Sleep(500);

            // Mark 확인
            iResult = FindMicroMarkA(out MarkPosA);
            if (iResult != SUCCESS) return iResult;

            // Screen Index Theta 원점 이동
            iResult = m_RefComp.Stage.MoveStageScreenMinusT(ECameraSelect.MICRO);
            if (iResult != SUCCESS) return iResult;

            // Screen Index Theta - 이동
            iResult = m_RefComp.Stage.MoveStageScreenMinusT(ECameraSelect.MICRO);
            if (iResult != SUCCESS) return iResult;

            Sleep(500);
            // Mark 확인
            iResult = FindMicroMarkA(out MarkPosB);
            if (iResult != SUCCESS) return iResult;

            // Screen Index Theta 원점 이동
            iResult = m_RefComp.Stage.MoveStageScreenPlusT(ECameraSelect.MICRO);
            if (iResult != SUCCESS) return iResult;

            // 회전 중심값을 구함
            double dAngle = m_RefComp.Stage.GetScreenIndexTheta()*2;
            CPos_XY RotatePos = new CPos_XY();
            RotatePos = CalsRotateCenter(dAngle, MarkPosA, MarkPosB);

            // Mark의 좌표는 카메라 중심 (0,0) 기준으로 확인
            // Stage위치가 회전중심임
            // 현재 Stage 위치에서 계산된 회전 중심을 빼면
            // 카메라의 위치가 됨. 
            CamPos = StagePos - RotatePos;

            SetCameraPos(FINE_CAM, CamPos);

            /////////////////////////////////////////////////////////////////////
            //  회전 각도를 키워서.. 정확도를 높히는 동작
            //  +/- 2도를 회전 시킨다.
            //  Stage를 회전할때.. Mark가 FOV 밖에 나가므로, Stage를 이동하여 영상에 들어오게 한다.

            // Position A 위치 확인 ------------------------------
            // Fov에 Mark가 들기 위해.. 이동할 Angle로 회전 변환하고, 이때 움직이는
            // X,Y축 이동 거리를 반대로 이동한다. 
            TransPos = CoordinateRotate(CAM_POS_CALS_ROATE_ANGLE, 
                                        m_Data.Vision.Camera[(int)ECameraSelect.MICRO].Position, StagePos);            
            // Stage 이동 거리 대입
            StageMovePos.dX = -TransPos.dX;
            StageMovePos.dY = -TransPos.dY;
            StageMovePos.dT = CAM_POS_CALS_ROATE_ANGLE;
            StageMovePos.dZ = 0.0;

            // Theta + 이동
            iResult = m_RefComp.Stage.MoveStageRelativeXYT(StageMovePos);
            if (iResult != SUCCESS) return iResult;

            Sleep(500);

            // Mark 확인
            iResult = FindMicroMarkA(out MarkPosA);
            if (iResult != SUCCESS) return iResult;

            // Position B 위치 확인 ------------------------------
            // Fov에 Mark가 들기 위해.. 이동할 Angle로 회전 변환하고, 이때 움직이는
            // X,Y축 이동 거리를 반대로 이동한다. 
            TransPos = CoordinateRotate(-CAM_POS_CALS_ROATE_ANGLE, 
                                        m_Data.Vision.Camera[(int)ECameraSelect.MICRO].Position, StagePos);
            // Stage 이동 거리 대입
            StageMovePos.dX = -TransPos.dX;
            StageMovePos.dY = -TransPos.dY;
            StageMovePos.dT = -CAM_POS_CALS_ROATE_ANGLE;
            StageMovePos.dZ = 0.0;

            // Theta - 이동
            iResult = m_RefComp.Stage.MoveStageRelativeXYT(StageMovePos);
            if (iResult != SUCCESS) return iResult;

            Sleep(500);

            // Mark 확인
            iResult = FindMicroMarkA(out MarkPosB);
            if (iResult != SUCCESS) return iResult;

            // 회전 중심값을 구함
            dAngle = CAM_POS_CALS_ROATE_ANGLE * 2;

            RotatePos = CalsRotateCenter(dAngle, MarkPosA, MarkPosB);

            // Mark의 좌표는 카메라 중심 (0,0) 기준으로 확인
            // Stage위치가 회전중심임
            // 현재 Stage 위치에서 계산된 회전 중심을 빼면
            // 카메라의 위치가 됨. 
            CamPos = StagePos - RotatePos;

            SetCameraPos(FINE_CAM, CamPos);

            // Stage 원위치
            iResult = m_RefComp.Stage.MoveStagePos(CurPos);
            if (iResult != SUCCESS) return iResult;

            Sleep(500);

            return SUCCESS;
        }

        public int ManualMacroCamPosSet()
        {
            CPos_XYTZ StageCurPos = new CPos_XYTZ();
            CPos_XYTZ MicroCamStagePos = new CPos_XYTZ();
            CPos_XY CamPos = new CPos_XY();

            int iResult = 0;

            // 현재 Stage 위치 읽어오기
            iResult = m_RefComp.Stage.GetStageCurPos(out StageCurPos);
            if (iResult != SUCCESS) return iResult;

            // Micro에서 Key가 영상의 Center일때 Stage 위치 읽어오기
            iResult = m_RefComp.Stage.GetMicroCamPos(out MicroCamStagePos);
            if (iResult != SUCCESS) return iResult;

            CamPos.dX = MicroCamStagePos.dX - StageCurPos.dX;
            CamPos.dY = MicroCamStagePos.dY - StageCurPos.dY;

            SetCameraPos(PRE__CAM, CamPos);

            return SUCCESS;
        }

        #endregion

        // Teaching 동작 
        #region Teaching

        // Theta Align Manual Set

        public void ThetaAlignStepInit()
        {
            eThetaAlignStep = EThetaAlignStep.INIT;
        }

        /// <summary>
        /// Theta Align 동작으로 PosB위치에서 PosA위치로 이동명령
        /// PosA,PosB 위치를 확인하고, Theta Align 적용 후
        /// Theta 회전 및 PosA위치를 재 설정
        /// </summary>
        /// <returns></returns>
        public int MoveThetaAlignPosA()
        {
            int iResult;
            var stagePos1 = new CPos_XYTZ();
            var stagePos2 = new CPos_XYTZ();
            var markPos1 = new CPos_XYTZ();
            var markPos2 = new CPos_XYTZ();
            var alignAdjPos = new CPos_XYTZ();
            var alignTeachPos = new CPos_XYTZ();
            
            if (eThetaAlignStep == EThetaAlignStep.POS_B)
            {
                // A위치를 읽어온다.
                if(eStageMode == EStatgeMode.RETURN)
                    m_RefComp.Stage.GetThetaAlignPosA(out stagePos1);
                if (eStageMode == EStatgeMode.TURN)
                    m_RefComp.Stage.GetThetaAlignTurnPosA(out stagePos1);
                                
                // B위치를 읽어온다. (현재 위치)
                m_RefComp.Stage.GetStageCurPos(out stagePos2);

                // Pre Cam일 경우 위치값을 보정한다.
                if (GetCurrentCam() == PRE__CAM)
                {
                    stagePos1.dX += CMainFrame.DataManager.SystemData_Align.CamEachOffsetX;
                    stagePos1.dY += CMainFrame.DataManager.SystemData_Align.CamEachOffsetY;
                }

                // 세로축으로 변화를 확인한다.
                if ((stagePos1.dY - stagePos2.dY) != 0.0)
                {
                    // Stage 좌표값을 Mark 좌표로 변환한다.
                    markPos1 = ChagneStageToMarkPos(stagePos1.Copy());
                    markPos2 = ChagneStageToMarkPos(stagePos2.Copy());

                    // Theta Align은 연산
                    CalsThetaAlign(markPos1, markPos2, out alignAdjPos);

                    // PosA 위치 Offset 적용
                    alignTeachPos.dX = stagePos1.dX - alignAdjPos.dX;
                    alignTeachPos.dY = stagePos1.dY - alignAdjPos.dY;
                    alignTeachPos.dT = stagePos1.dT - alignAdjPos.dT;

                    // Pre Cam일 경우에 Offset을 적용한다.
                    if (GetCurrentCam() == PRE__CAM)
                    {
                        alignTeachPos.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffsetX;
                        alignTeachPos.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffsetY;
                    }
                    
                    if (eStageMode == EStatgeMode.RETURN)   m_RefComp.Stage.SetThetaAlignPosA(alignTeachPos);
                    if (eStageMode == EStatgeMode.TURN)     m_RefComp.Stage.SetThetaAlignTurnPosA(alignTeachPos);

                }
            }

            // Pos A로 이동한다.
            if (eStageMode == EStatgeMode.RETURN)
            {
                if(GetCurrentCam()==PRE__CAM)
                    iResult = m_RefComp.Stage.MoveStageToThetaAlignPosA(true);
                else
                    iResult = m_RefComp.Stage.MoveStageToThetaAlignPosA();
            }
            else if (eStageMode == EStatgeMode.TURN)
            {
                if (GetCurrentCam() == PRE__CAM)
                    iResult = m_RefComp.Stage.MoveStageToThetaAlignTurnPosA(true);
                else
                    iResult = m_RefComp.Stage.MoveStageToThetaAlignTurnPosA();
            }
            else
                iResult = -1;

            if(iResult==SUCCESS) eThetaAlignStep = EThetaAlignStep.POS_A;

            return iResult;
        }
        /// <summary>
        /// Theta Align에서 PosA위치만 확인하고, PosB위치로 이동 명령
        /// 현재 위치가 PosA가 아닐 경우 실행하지 않음.
        /// </summary>
        /// <returns></returns>
        public int MoveThetaAlignPosB()
        {           
            int iResult;
            var posCur = new CPos_XYTZ();
            
            // 현재 위치를 읽어온다            
            m_RefComp.Stage.GetStageCurPos(out posCur);

            // PreCam일 경우 Offset을 적용한다. (Fine Cam 기준으로 위치 저장)
            if (GetCurrentCam() == PRE__CAM)
            {
                posCur.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffsetX;
                posCur.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffsetY;
            }

            // 현재 위치를 ThetaAlignPosA로 저장한다. 
            if (eStageMode == EStatgeMode.RETURN)  m_RefComp.Stage.SetThetaAlignPosA(posCur);            
            if (eStageMode == EStatgeMode.TURN)    m_RefComp.Stage.SetThetaAlignTurnPosA(posCur);            

            // ThetaAlignPosB로 이동한다.
            if (eStageMode == EStatgeMode.RETURN)
            {
                if (GetCurrentCam() == PRE__CAM)
                    iResult = m_RefComp.Stage.MoveStageToThetaAlignPosB(true);
                else
                    iResult = m_RefComp.Stage.MoveStageToThetaAlignPosB();
            }
            else if (eStageMode == EStatgeMode.TURN)
            {
                if (GetCurrentCam() == PRE__CAM)
                    iResult = m_RefComp.Stage.MoveStageToThetaAlignTurnPosB(true);
                else
                    iResult = m_RefComp.Stage.MoveStageToThetaAlignTurnPosB();
            }
            else
                iResult = -1;

            if(iResult == SUCCESS) eThetaAlignStep = EThetaAlignStep.POS_B;

            return iResult;

        }

        /// <summary>
        /// Stage의 좌표에서 X,Y 값만 Mark 좌표로 변환한다.
        /// (T축은 변화를 주지 않는다) 
        /// </summary>
        /// <param name="pStage : Stage의 좌표값"></param>
        /// <returns></returns>
        private CPos_XYTZ ChagneStageToMarkPos(CPos_XYTZ pStage)
        {
            var posMark = new CPos_XYTZ();
            var posCenter = new CPos_XYTZ();

            // 사용하는 Camera의 중심 위치를 구함.
            if (GetCurrentCam() == PRE__CAM)
            {
                posCenter = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER);
            }
            if (GetCurrentCam() == FINE_CAM)
            {
                posCenter = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER);

                posCenter.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffsetX;
                posCenter.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffsetY;
            }

            // Stage의 위치와 차로.. Camera 중심위치 대비 위치를 확인함.
            posMark.dX = posCenter.dX - pStage.dX;
            posMark.dY = posCenter.dY - pStage.dY;
            posMark.dT = pStage.dT;

            return posMark;
        }

        private CPos_XYTZ ChagneStageToMarkPos(CPos_XYTZ pStage, CPos_XYTZ pOffSet)
        {
            var posMark = new CPos_XYTZ();

            // 사용하는 Camera의 중심 위치를 구함.
            if (GetCurrentCam() == PRE__CAM)
            {
                posMark = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER);
            }
            if (GetCurrentCam() == FINE_CAM)
            {
                posMark = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER);

                posMark.dX += CMainFrame.DataManager.SystemData_Align.CamEachOffsetX;
                posMark.dY += CMainFrame.DataManager.SystemData_Align.CamEachOffsetY;
            }

            // Stage의 위치와 차로.. Camera 중심위치 대비 위치를 확인함.
            posMark -= pStage;

            // Offset이 있을 경우 적용함.
            posMark += pOffSet;

            return posMark;
        }

        public void InitThetaAlign()
        {
            eThetaAlignStep = EThetaAlignStep.INIT;
        }
        /// <summary>
        /// GUI에서 실행할  ThetaAlign 명령
        /// 위치 상태에 따라서 A,B 위치를 번갈아 이동함.
        /// </summary>
        /// <returns></returns>
        public int DoThetaAlign()
        {
            bool checkPos;
            m_RefComp.Stage.CompareStagePos((int)EStagePos.THETA_ALIGN_A, out checkPos,false);

            if (checkPos || eThetaAlignStep==EThetaAlignStep.POS_A)
                MoveThetaAlignPosB();
            else
                MoveThetaAlignPosA();

            return SUCCESS;
        }
        /// <summary>
        /// Theta Align 계산 연산 PosA 값을 기준으로 결과를 리턴한다.
        /// </summary>
        /// <param name="pPosA" Position A : 기준되는 Point></param>
        /// <param name="pPosB" Position B : 기준 Point에서 가로로 이동한 지점></param>
        /// <param name="pAlignPos" 계산된 X,Y,T의 값></param>
        /// <returns></returns>
        private int CalsThetaAlign(CPos_XYTZ pPosA, CPos_XYTZ pPosB, out CPos_XYTZ pAlignPos)
        {
            int iResult = -1;
            CPos_XYTZ mAlignPos = new CPos_XYTZ();
            // 회전 중심값은 항상 (0,0) 
            var RotateCenter = new CPos_XYTZ();
            if (GetCurrentCam() == PRE__CAM)
            {
                RotateCenter = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER);
            }
            if (GetCurrentCam() == FINE_CAM)
            {
                RotateCenter = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER);

                RotateCenter.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffsetX;
                RotateCenter.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffsetY;
            }

            // Align 결과 초기화
            pAlignPos = mAlignPos;

            // 위치 편차 적용
            double dAngle  = 0.0;
            double dWidth  = pPosA.dX - pPosB.dX;
            double dHeight = pPosA.dY - pPosB.dY;

            pPosA.dX = RotateCenter.dX + pPosA.dX;
            pPosA.dY = RotateCenter.dY + pPosA.dY;

            // 회전 각도 계산
            dAngle = CalsRotateAngle(dWidth, dHeight);

            var mCamPos = new CPos_XYTZ();
            
            // Cam중심을 A Pos 중심으로 회전
            mCamPos = CoordinateRotate(-dAngle, pPosA, RotateCenter);

            // Align 결과 값 대입
            mAlignPos.dX = mCamPos.dX - pPosA.dX;
            mAlignPos.dY = mCamPos.dY - pPosA.dY;
            mAlignPos.dT = -dAngle;
            mAlignPos.dZ = 0.0;

            pAlignPos = mAlignPos;
            return SUCCESS;
        }

        // Edge Align Pos 설정
        public void SetEdgePosOffset(int index,CPos_XYTZ pPos)
        {
            m_RefComp.Stage.SetEdgeAlignOffset(index, pPos);
        }
        public int GetEdgePosOffset(int index, out CPos_XYTZ pPos)
        {
            return m_RefComp.Stage.GetEdgeAlignOffset(index, out pPos);
        }
        /// <summary>
        /// UI에서 User가 Button을 클릭하면서 4 Point 위치값 Offset조절
        /// </summary>
        /// <returns></returns>
        public int SetEdgePosOffsetNext()
        {
            CPos_XYTZ mPos = new CPos_XYTZ();
            int iCurPosIndex = 0;
            int iResult = -1;

            // 현재 위치 값 & Index를 읽어온다.
            iResult = m_RefComp.Stage.GetStageCurPos(out mPos);
            if (iResult != SUCCESS) return iResult;
            iResult = m_RefComp.Stage.GetStagePosInfo(out iCurPosIndex);
            if (iResult != SUCCESS) return iResult;

            // Edge Pos 1 위치 이동
            if (bEdgeAlignTeachInit = false)
            {
                iResult = MoveToEdgeAlignPos1();
                if (iResult != SUCCESS) return iResult;

                bEdgeAlignTeachInit = true;
                return SUCCESS;
            }
            // Edge Pos 1 위치 설정
            if (iCurPosIndex == (int)EStagePos.EDGE_ALIGN_1)
            {
                SetEdgePosOffset(iCurPosIndex, mPos);

                iResult = MoveToEdgeAlignPos2();
                if (iResult != SUCCESS) return iResult;

                return SUCCESS;
            }
            //// Edge Pos 2 위치 설정
            //if (iCurPosIndex == (int)EStagePos.EDGE_ALIGN_2)
            //{
            //    SetEdgePosOffset(iCurPosIndex, mPos);

            //    iResult = MoveToEdgeAlignPos3();
            //    if (iResult != SUCCESS) return iResult;

            //    return SUCCESS;
            //}
            //// Edge Pos 3 위치 설정
            //if (iCurPosIndex == (int)EStagePos.EDGE_ALIGN_3)
            //{
            //    SetEdgePosOffset(iCurPosIndex, mPos);

            //    iResult = MoveToEdgeAlignPos4();
            //    if (iResult != SUCCESS) return iResult;

            //    return SUCCESS;
            //}
            //// Edge Pos 4 위치 설정
            //if (iCurPosIndex == (int)EStagePos.EDGE_ALIGN_4)
            //{
            //    SetEdgePosOffset(iCurPosIndex, mPos);

            //    bEdgeAlignTeachInit = false;
            //    return SUCCESS;
            //}

            return SUCCESS;
        }

        #endregion

        // Align 동작
        #region Align 동작

        // Edge Align 동작=======================================
        public int DoEdgeAlign()
        {
            int iResult = -1;
            int iSleepTime = 200;
            CPos_XY[] EdgePos = new CPos_XY[4];
            CPos_XY WaferCenter = new CPos_XY();
            CPos_XYTZ StageCurPos = new CPos_XYTZ();

            // Edge 1번으로 이동 & Edge 확인
            iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos1();
            if (iResult != SUCCESS) return iResult;

            Sleep(iSleepTime);

            iResult = FindEdgePoint(out EdgePos[0]);
            if (iResult != SUCCESS) return iResult;

            // Edge 2번으로 이동 & Edge 확인
            iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos2();
            if (iResult != SUCCESS) return iResult;

            Sleep(iSleepTime);

            iResult = FindEdgePoint(out EdgePos[1]);
            if (iResult != SUCCESS) return iResult;

            // Edge 3번으로 이동 & Edge 확인
            iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos3();
            if (iResult != SUCCESS) return iResult;

            Sleep(iSleepTime);

            iResult = FindEdgePoint(out EdgePos[2]);
            if (iResult != SUCCESS) return iResult;

            // Edge 4번으로 이동 & Edge 확인
            iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos4();
            if (iResult != SUCCESS) return iResult;

            Sleep(iSleepTime);

            iResult = FindEdgePoint(out EdgePos[3]);
            if (iResult != SUCCESS) return iResult;

            // Edge Align 연산
            WaferCenter.dX = (EdgePos[0].dX + EdgePos[1].dX + EdgePos[2].dX + EdgePos[3].dX) / 4;
            WaferCenter.dY = (EdgePos[0].dY + EdgePos[1].dY + EdgePos[2].dY + EdgePos[3].dY) / 4;

            // Stage 현재 위치 확인
            iResult = m_RefComp.Stage.GetStageCurPos(out StageCurPos);
            if (iResult != SUCCESS) return iResult;

            // Wafer의 중심과 Stage의 위치 차이를 Offset으로 저장한다.
            m_Data.Align.WaferPosOffset.dX = StageCurPos.dX - WaferCenter.dX;
            m_Data.Align.WaferPosOffset.dY = StageCurPos.dY - WaferCenter.dY;

            return SUCCESS;
        }

        public int DoAlign(int iMode)
        {
            int iResult = -1;
            int iCount = 0;
            int iSleepTime = 200;

            CPos_XY MarkPosA = new CPos_XY();
            CPos_XY MarkPosB = new CPos_XY();
            CPos_XYTZ StageCurPos = new CPos_XYTZ();

            double dTolerancePos = 0.0;
            double dToleranceAngle = 0.0;

            // Tolerance 설정
            if (iMode == MACRO_ALIGN_MODE)  dTolerancePos   = MACRO_POS_TOLERANCE;
            else                            dTolerancePos   = MICRO_POS_TOLERANCE;

            if (iMode == MACRO_ALIGN_MODE)  dToleranceAngle = MACRO_ANGLE_TOLERANCE;
            else                            dToleranceAngle = MICRO_ANGLE_TOLERANCE;

            // 모드에 따라서 렌즈 선택

            // AutoFocus 진행 (Micro Align A만 진행)

            // Align 기존 값 초기화
            m_RefComp.Stage.SetAlignDataInit();

            while (true)
            {
                //====================================================
                // Mark A,B의 위치 확인
                if (iMode == MACRO_ALIGN_MODE)
                {
                    // Macro Mark A 위치 이동
                    iResult = m_RefComp.Stage.MoveStageToMacroAlignA();
                    if (iResult != SUCCESS) return iResult;

                    Sleep(iSleepTime);

                    iResult = FindMacroMark(out MarkPosA);
                    if (iResult != SUCCESS) return iResult;

                    // Macro Mark B 위치 이동
                    iResult = m_RefComp.Stage.MoveStageToMacroAlignB();
                    if (iResult != SUCCESS) return iResult;

                    Sleep(iSleepTime);

                    iResult = FindMacroMark(out MarkPosB);
                    if (iResult != SUCCESS) return iResult;
                }

                if (iMode == MICRO_ALIGN_MODE_CH1)
                {
                    // Macro Mark A 위치 이동
                    iResult = m_RefComp.Stage.MoveStageToMicroAlignA();
                    if (iResult != SUCCESS) return iResult;

                    Sleep(iSleepTime);

                    iResult = FindMicroMarkA(out MarkPosA);
                    if (iResult != SUCCESS) return iResult;

                    // Macro Mark B 위치 이동
                    iResult = m_RefComp.Stage.MoveStageToMicroAlignB();
                    if (iResult != SUCCESS) return iResult;

                    Sleep(iSleepTime);

                    iResult = FindMicroMarkA(out MarkPosB);
                    if (iResult != SUCCESS) return iResult;
                }

                if (iMode == MICRO_ALIGN_MODE_CH2)
                {
                    // Macro Mark A 위치 이동
                    iResult = m_RefComp.Stage.MoveStageToMicroAlignTurnA();
                    if (iResult != SUCCESS) return iResult;

                    Sleep(iSleepTime);

                    iResult = FindMicroMarkB(out MarkPosA);
                    if (iResult != SUCCESS) return iResult;

                    // Macro Mark B 위치 이동
                    iResult = m_RefComp.Stage.MoveStageToMicroAlignTurnB();
                    if (iResult != SUCCESS) return iResult;

                    Sleep(iSleepTime);

                    iResult = FindMicroMarkB(out MarkPosB);
                    if (iResult != SUCCESS) return iResult;
                }

                //====================================================
                // Align Cals

                // 회전 각도 계산
                double dAngle = CalsRotateAngle(MarkPosA, MarkPosB);
                // 현재 Stage 위치 읽기
                iResult = m_RefComp.Stage.GetStageCurPos(out StageCurPos);
                if (iResult != SUCCESS) return iResult;

                // Align 기준점
                CPos_XY CamPos = new CPos_XY();
                if (iMode == MACRO_ALIGN_MODE)  CamPos = GetCameraPos(PRE__CAM);
                else                            CamPos = GetCameraPos(FINE_CAM);


                CPos_XY MarkNewPos = new CPos_XY();
                CPos_XYTZ AlignPos = new CPos_XYTZ();

                // Mark A가 회전 변환했을 때 신규 좌표
                MarkNewPos = CoordinateRotate(dAngle, MarkPosA, StageCurPos);

                AlignPos.dX = CamPos.dX - MarkNewPos.dX;
                AlignPos.dX = CamPos.dY - MarkNewPos.dX;
                AlignPos.dT = dAngle;
                AlignPos.dZ = 0.0;

                // Align 완료 조건 확인
                if (Math.Abs(AlignPos.dX) < dTolerancePos && Math.Abs(AlignPos.dY) < dTolerancePos && Math.Abs(AlignPos.dT) < dToleranceAngle)
                {
                    return SUCCESS;
                }

                // Align Adjust 설정
                m_RefComp.Stage.SetAlignData(AlignPos);

                // Mark A로 이동 및 확인
                if (iMode == MACRO_ALIGN_MODE)
                {
                    // Macro Mark A 위치 이동
                    iResult = m_RefComp.Stage.MoveStageToMacroAlignA();
                    if (iResult != SUCCESS) return iResult;

                    Sleep(iSleepTime);

                    iResult = FindMacroMark(out MarkPosA);
                    if (iResult != SUCCESS) return iResult;
                }

                if (iMode == MICRO_ALIGN_MODE_CH1)
                {
                    // Macro Mark A 위치 이동
                    iResult = m_RefComp.Stage.MoveStageToMicroAlignA();
                    if (iResult != SUCCESS) return iResult;

                    Sleep(iSleepTime);

                    iResult = FindMicroMarkA(out MarkPosA);
                    if (iResult != SUCCESS) return iResult;
                }

                if (iMode == MICRO_ALIGN_MODE_CH2)
                {
                    // Macro Mark A 위치 이동
                    iResult = m_RefComp.Stage.MoveStageToMicroAlignTurnA();
                    if (iResult != SUCCESS) return iResult;

                    Sleep(iSleepTime);

                    iResult = FindMicroMarkB(out MarkPosA);
                    if (iResult != SUCCESS) return iResult;
                }
                // Mark A의 위치의 정도 확인                
                if (Math.Abs(MarkPosA.dX) < dTolerancePos && Math.Abs(MarkPosA.dY) < dTolerancePos)
                {
                    return SUCCESS;
                }

                // 5회 반복이 넘을 경우 Fail
                if (iCount > 5) return GenerateErrorCode(ERR_CTRLSTAGE_MACRO_ALIGN_FAIL);

                iCount++;

            }

            return SUCCESS;
        }

        // Macro Align 동작
        public int DoMacroAlign()
        {
            return DoAlign(MACRO_ALIGN_MODE);            
        }

        // Micro Algin-A 동작
        public int DoMicroAlignA()
        {
            return DoAlign(MICRO_ALIGN_MODE_CH1);
        }
        // Micro Algin-B 동작
        public int DoMicroAlignB()
        {
            return DoAlign(MICRO_ALIGN_MODE_CH2);
        }

        public int DoAlignProcess()
        {
            int iResult = -1;

            iResult = DoEdgeAlign();
            if(iResult != SUCCESS) return iResult;

            iResult = DoMacroAlign();
            if (iResult != SUCCESS) return iResult;

            iResult = DoMicroAlignA();
            if (iResult != SUCCESS) return iResult;
           
            iResult = DoMicroAlignB();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        
        #endregion


    }
}
