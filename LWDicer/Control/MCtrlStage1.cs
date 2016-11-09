using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using static LWDicer.Layers.DEF_System;
using static LWDicer.Layers.DEF_Motion;
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
        public const int ERR_CTRLSTAGE_CAM_HANDEL_FAIL = 40;
        
        // SYSTEM 설정
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

        public enum EThetaAlignPos
        {
            INIT=-1,
            POS_A,
            POS_B,
            MAX
        }

        public enum ERotateCenterStep
        {
            INIT,
            PRE_POS_A,
            PRE_POS_B,
            FINE_POS_A,
            FINE_POS_B,
            MARK_POS,
            MAX,
        }

        public enum EEdgeAlignTeachPos
        {
            INIT=-1,
            POS1=0,
            POS2,
            POS3,
            MAX,
        }

        public enum EVisionPattern
        {
            MACRO_A = 0,
            MACRO_B,
            MICRO_A,
            MICOR_B,
            MAX,
        }

        public class CCtrlAlignData
        {
            // Edge Align Teach Pos
            public CPos_XYTZ[] EdgeTeachPos  = new CPos_XYTZ[(int)EEdgeAlignTeachPos.MAX];
            // Theta Align Teach Pos
            public CPos_XYTZ[] ThetaTeachPos = new CPos_XYTZ[(int)EThetaAlignPos.MAX];
            // Vision Pattern Mark Data
            public CSearchData[] VisionPattern = new CSearchData[(int)EVisionPattern.MAX];

            public CCtrlAlignData()
            {
                for (int i = 0; i < EdgeTeachPos.Length; i++) EdgeTeachPos[i] = new CPos_XYTZ();
                for (int i = 0; i < ThetaTeachPos.Length; i++) ThetaTeachPos[i] = new CPos_XYTZ();
                for (int i = 0; i < VisionPattern.Length; i++) VisionPattern[i] = new CSearchData();                
            }
        }

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

        private EStatgeMode eStageMode = EStatgeMode.RETURN;
        private EThetaAlignPos eThetaAlignStep = EThetaAlignPos.INIT;
        private ERotateCenterStep eRotateCenterStep = ERotateCenterStep.INIT;
        private EEdgeAlignTeachPos eEdgeTeachStep = EEdgeAlignTeachPos.INIT;
        
        private CPos_XYTZ[] StageRotatePos = new CPos_XYTZ[(int)ERotateCenterStep.MAX];

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

            // 내부 변수 초기화.
            for (int i = 0; i < StageRotatePos.Length; i++) StageRotatePos[i] = new CPos_XYTZ();

            // 등록된 패턴 Reload
            if(m_Data.Align==null) return SUCCESS;
            m_RefComp.Vision.ReLoadPatternMark(PRE__CAM, PATTERN_A, m_Data.Align.VisionPattern[(int)EVisionPattern.MACRO_A]);
            m_RefComp.Vision.ReLoadPatternMark(PRE__CAM, PATTERN_B, m_Data.Align.VisionPattern[(int)EVisionPattern.MACRO_B]);
            m_RefComp.Vision.ReLoadPatternMark(FINE_CAM, PATTERN_A, m_Data.Align.VisionPattern[(int)EVisionPattern.MICRO_A]);
            m_RefComp.Vision.ReLoadPatternMark(FINE_CAM, PATTERN_B, m_Data.Align.VisionPattern[(int)EVisionPattern.MICOR_B]);

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
        public int IsObjectDetected(out bool bStatus)
        {
            int iResult = m_RefComp.Stage.IsObjectDetected(out bStatus);
            return iResult;
        }
        #endregion

        public int SetPosition(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            return m_RefComp.Stage.SetPosition_Stage(Pos_Fixed, Pos_Model, Pos_Offset);
        }

        public int SetAlignData(CPos_XYTZ offset)
        {
            return m_RefComp.Stage.SetAlignData(offset);
        }
        // Stage의 상태를 확인하는 동작
        #region Stage 상태 확인

        public int IsAbsorbed(out bool bStatus)
        {
            return m_RefComp.Stage.IsAbsorbed(out bStatus);
        }

        public int IsReleased(out bool bStatus)
        {
            return m_RefComp.Stage.IsReleased(out bStatus);
        }

        public int IsClampOpen(out bool bStatus)
        {
            return m_RefComp.Stage.IsClampOpen(out bStatus);
        }

        public int IsClampClose(out bool bStatus)
        {
            return m_RefComp.Stage.IsClampClose(out bStatus);
        }

        public int IsStageSafetyZone(out bool bStatus)
        {
            return m_RefComp.Stage.IsStageAxisInSafetyZone(out bStatus);
        }

        public int IsOrignReturn(out bool bStatus)
        {
            return m_RefComp.Stage.IsStageOrignReturn(out bStatus);
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
            m_RefComp.Stage.GetStageCurPos(out originPos);

            // Pattern 위치로 Move
            originPos1.dX = originPos.dX;
            originPos1.dY = originPos.dY - (double)CMainFrame.DataManager.ModelData.ProcData.PatternOffset1;
            originPos1.dT = originPos.dT;
            m_RefComp.Stage.MoveStagePos(originPos1);

            // Pattern 1 Process ===================================================================
            

            //// Bmp & Config.ini File Download
            //m_RefComp.Scanner.SendConfig("T:\\SFA\\LWDicer\\ScannerData\\config_job1.ini");
            //m_RefComp.Scanner.SendBitmap("T:\\SFA\\LWDicer\\ImageData\\image_job1.bmp");

            // Marking Process (Step & Go)
            for (int i = 0; i < nPatternCount; i++)
            {
               

                m_ProcsTimer.StartTimer();

                for (int j = 0; j < nStepCount; j++)
                {
                    // Process Stop 확인
                    if (CMainFrame.DataManager.ModelData.ProcData.ProcessStop) return SUCCESS;

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
            //m_RefComp.Scanner.SendConfig("T:\\SFA\\LWDicer\\ScannerData\\config_job2.ini");
            //m_RefComp.Scanner.SendBitmap("T:\\SFA\\LWDicer\\ImageData\\image_job2.bmp");

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
            CPositionSet fixedPos;
            CPositionSet modelPos;
            CPositionSet offsetPos;
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


        public int MoveToWaferCenterPre()
        {
            return m_RefComp.Stage.MoveStageToWaferCenterPre();
        }

        public int MoveToWaferCenterFine()
        {
            return m_RefComp.Stage.MoveStageToWaferCenterFine();
        }

        public int MoveToStageCenterPre()
        {            
            return m_RefComp.Stage.MoveStageToStageCenterPre();   
        }

        public int MoveToStageCenterFine()
        {
            return m_RefComp.Stage.MoveStageToStageCenterFine();            
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


        public int MoveToEdgeAlignTeachPos1()
        {
            int iResult;
            CPos_XYTZ movePos = m_Data.Align.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS1].Copy(); //CMainFrame.LWDicer.m_DataManager.ModelData.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS1].Copy();

            if (GetCurrentCam() == FINE_CAM)
            {
                movePos.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                movePos.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
            }
            
            iResult = m_RefComp.Stage.MoveStagePos(movePos);

            return iResult;
        }

        public int MoveToEdgeAlignTeachPos2()
        {
            int iResult;
            //CPos_XYTZ movePos = CMainFrame.LWDicer.m_DataManager.ModelData.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS2].Copy();
            CPos_XYTZ movePos = m_Data.Align.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS2].Copy();

            if (GetCurrentCam() == FINE_CAM)
            {
                movePos.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                movePos.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
            }

            iResult = m_RefComp.Stage.MoveStagePos(movePos);

            return iResult;
        }

        public int MoveToEdgeAlignTeachPos3()
        {
            int iResult;
            //CPos_XYTZ movePos = CMainFrame.LWDicer.m_DataManager.ModelData.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS3].Copy();
            CPos_XYTZ movePos = m_Data.Align.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS3].Copy();

            if (GetCurrentCam() == FINE_CAM)
            {
                movePos.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                movePos.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
            }

            iResult = m_RefComp.Stage.MoveStagePos(movePos);

            return iResult;
        }        

        public int MoveToEdgeAlignPos1()
        {
            int iResult;
            if (GetCurrentCam() == PRE__CAM)
                iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos1();
            else
                iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos1(true);

            return iResult;
        }

        public int MoveToEdgeAlignPos2()
        {
            int iResult;
            if (GetCurrentCam() == PRE__CAM)
                iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos2();
            else
                iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos2(true);

            return iResult;
        }

        public int MoveToEdgeAlignPos3()
        {
            int iResult;
            if (GetCurrentCam() == PRE__CAM)
                iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos3();
            else
                iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos3(true);

            return iResult;
        }

        public int MoveToEdgeAlignPos4()
        {
            int iResult;
            if (GetCurrentCam() == PRE__CAM)
                iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos4();
            else
                iResult = m_RefComp.Stage.MoveStageToEdgeAlignPos4(true);

            return iResult;
        }

        public int MoveToMacroCam()
        {
            return m_RefComp.Stage.MoveStageToMacroCam();
        }

        /// <summary>
        /// Macro Align할 Mark A 위치로 이동
        /// (Wafer Center 기준에서 Align Width 값으로 이동함)
        /// </summary>
        /// <returns></returns>
        public int MoveToMacroTeachA()
        {
            return m_RefComp.Stage.MoveStageToMacroTeachA();
        }
        /// <summary>
        /// Macro Align할 Mark B 위치로 이동
        /// (Wafer Center 기준에서 Align Width 값으로 이동함)
        /// </summary>
        /// <returns></returns>
        public int MoveToMacroTeachB()
        {
            return m_RefComp.Stage.MoveStageToMacroTeachB();
        }

        /// <summary>
        /// Macro Align할 Mark A 위치로 이동
        /// (Wafer Center 기준에서 Align Width 값으로 이동함)
        /// </summary>
        /// <returns></returns>
        public int TeachMacroAlignA()
        {            
            CPos_XYTZ movePos;
            CPos_XYTZ offSet;

            // 현재 위치를 읽어옴.
            m_RefComp.Stage.GetStageCurPos(out movePos);
            m_RefComp.Stage.GetAlignData(out offSet);

            movePos.dX -= offSet.dX;
            movePos.dY -= offSet.dY;
            
            CMainFrame.LWDicer.m_DataManager.Pos_Fixed.Pos_Stage1.Pos[(int)EStagePos.MACRO_ALIGN] = movePos;

            return SUCCESS;
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

        public int ScreenClickMove(Size pSize, Point pPoint)
        {
            Size picSize = pSize;
            Point clickPos = pPoint;

            Point centerPic = new Point(0, 0);
            Point moveDistance = new Point(0, 0);

            double ratioMove = 0.0;
            CPos_XYTZ movePos = new CPos_XYTZ();

            centerPic.X = picSize.Width / 2;
            centerPic.Y = picSize.Height / 2;

            moveDistance.X = centerPic.X - clickPos.X;
            moveDistance.Y = centerPic.Y - clickPos.Y;


            if (GetCurrentCam() == FINE_CAM)
            {
                ratioMove = CMainFrame.DataManager.SystemData_Align.MicroScreenWidth / (double)picSize.Width;
                movePos.dX = (double)moveDistance.X * ratioMove;

                ratioMove = CMainFrame.DataManager.SystemData_Align.MicroScreenHeight / (double)picSize.Height;
                movePos.dY = -(double)moveDistance.Y * ratioMove;

                return MoveStageRelative(movePos);
            }

            if (GetCurrentCam() == PRE__CAM)
            {
                ratioMove = CMainFrame.DataManager.SystemData_Align.MacroScreenWidth / (double)picSize.Width;
                movePos.dX = (double)moveDistance.X * ratioMove;

                ratioMove = CMainFrame.DataManager.SystemData_Align.MacroScreenHeight / (double)picSize.Height;
                movePos.dY = -(double)moveDistance.Y * ratioMove;

                return MoveStageRelative(movePos);
            }

            return SUCCESS;
        }

        #endregion

        // Camera 위치 구동 지령
        #region Camera 축 구동

        public int MoveToCameraWaitPos()
        {
            return m_RefComp.Stage.MoveCameraToWaitPos();
        }

        public int MoveToCameraWorkPos()
        {
            return m_RefComp.Stage.MoveCameraToWorkPos();
        }

        public int MoveToCameraFocusPosInpect()
        {
            return m_RefComp.Stage.MoveCameraToFocusPosInspect();
        }

        public int MoveToCameraFocusPosFine()
        {
            return m_RefComp.Stage.MoveCameraToFocusPosFine();
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

        public int ChangeMacroCam()
        {
            IntPtr viewHandle;
            viewHandle = CMainFrame.LWDicer.m_Vision.GetLocalViewHandle(PRE__CAM);
            if (viewHandle == null) return GenerateErrorCode(ERR_CTRLSTAGE_CAM_HANDEL_FAIL);

            CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, viewHandle);

            return SUCCESS;
        }

        public int ChangeMicroCam()
        {
            IntPtr viewHandle;
            viewHandle = CMainFrame.LWDicer.m_Vision.GetLocalViewHandle(FINE_CAM);
            if (viewHandle == null) return GenerateErrorCode(ERR_CTRLSTAGE_CAM_HANDEL_FAIL);

            CMainFrame.LWDicer.m_Vision.DestroyLocalView(PRE__CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(FINE_CAM, viewHandle);

            return SUCCESS;
        }
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
            CEdgeData pEdgeData = new CEdgeData();
            iResult = m_RefComp.Vision.FindEdge(iCam, out pEdgeData);

            if (iResult != SUCCESS) return iResult;

            // 결과 확인            
            if (pEdgeData.m_iEdgeNum < 1)
            {
                // 중복적으로 Edge가 검출될때는 Error를 리턴한다.
                return GenerateErrorCode(ERR_CTRLSTAGE_EDGE_POINT_OVER);
            }
            //Edge 위치 결과 대입
            pPos = pEdgeData.EdgePos;

            // Edge Pos Overlay Display
            Point pointEdge = new Point(0, 0);
            pointEdge.X = (int)pEdgeData.EdgePos.dX;
            pointEdge.Y = (int)pEdgeData.EdgePos.dY;

          //  m_RefComp.Vision.ClearOverlay();
            m_RefComp.Vision.ShowRectRoi();
            m_RefComp.Vision.DrawOverlayCrossMark(50, 50, pointEdge,Color.LightGreen);

            // Camera의 틀어짐 보정
            //CPos_XY mCenter = new CPos_XY(); // 회전 중심은 (0,0)으로 한다
            //pPos = CoordinateRotate(m_Data.Vision.Camera[iCam].CameraTilt, pPos, mCenter);

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

        public double GetHairLineWidth()
        {
            int iHairLineWidth = 0;
            double dHairLineWidth = 0.0;

            iHairLineWidth = m_RefComp.Vision.GetHairLineWidth();
            
            if(GetCurrentCam()==PRE__CAM)
            {
                dHairLineWidth = (double) CMainFrame.DataManager.SystemData_Align.PixelResolution[(int)ECameraSelect.MACRO] * (double)iHairLineWidth /500;
            }
            else
            {
                dHairLineWidth = (double)CMainFrame.DataManager.SystemData_Align.PixelResolution[(int)ECameraSelect.MICRO] * (double)iHairLineWidth /500;
            }

            return dHairLineWidth;
        }

        public SizeF GetRoiSize()
        {
            Size szRoi = m_RefComp.Vision.GetRoiSize();
            float dRoiWidth = 0.0f;
            float dRoiHeight = 0.0f;

            if (GetCurrentCam() == PRE__CAM)
            {
                dRoiWidth = (float)CMainFrame.DataManager.SystemData_Align.PixelResolution[(int)ECameraSelect.MACRO] * (float)szRoi.Width / 1000;
                dRoiHeight = (float)CMainFrame.DataManager.SystemData_Align.PixelResolution[(int)ECameraSelect.MACRO] * (float)szRoi.Height / 1000;

            }
            else
            {
                dRoiWidth = (float)CMainFrame.DataManager.SystemData_Align.PixelResolution[(int)ECameraSelect.MICRO] * (float)szRoi.Width / 1000;
                dRoiHeight = (float)CMainFrame.DataManager.SystemData_Align.PixelResolution[(int)ECameraSelect.MICRO] * (float)szRoi.Height / 1000;

            }

            return new SizeF(dRoiWidth, dRoiHeight);
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
        public int ChangeVisionMagnitude(int iCam, IntPtr pHandle, EVisionOverlayMode OverlayMode)
        {            
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

            // 모드에 따라서 Line의 종류를 다르게 해서 보여줌.

            // Hair Line
            if(OverlayMode== EVisionOverlayMode.HAIR_LINE)
                m_RefComp.Vision.ShowHairLine();
            if (OverlayMode == EVisionOverlayMode.EDGE)
                //m_RefComp.Vision.ShowHairLine();
                m_RefComp.Vision.ShowRectRoi();
            if (OverlayMode == EVisionOverlayMode.ROI)
                m_RefComp.Vision.ShowRectRoi();

            return SUCCESS;
        }

        public int ChangeMacroVision(IntPtr pHandle, EVisionOverlayMode OverlayMode)
        {
            return ChangeVisionMagnitude(PRE__CAM, pHandle, OverlayMode);
        }

        public int ChangeMicroVision(IntPtr pHandle, EVisionOverlayMode OverlayMode)
        {
            return ChangeVisionMagnitude(FINE_CAM, pHandle, OverlayMode);
        }

        public int GetCurrentCam()
        {
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
            eThetaAlignStep = EThetaAlignPos.INIT;
        }

        /// <summary>
        /// Theta Align 동작으로 PosB위치에서 PosA위치로 이동명령
        /// PosA,PosB 위치를 확인하고, Theta Align 적용 후
        /// Theta 회전 및 PosA위치를 재 설정
        /// </summary>
        /// <returns></returns>
        public int MoveThetaAlignPosA()
        {
            // Pos A로 이동한다.
            return m_RefComp.Stage.MoveStagePos(GetThetaAlignPosA()); ;
        }
        /// <summary>
        /// Theta Align에서 PosA위치만 확인하고, PosB위치로 이동 명령
        /// 현재 위치가 PosA가 아닐 경우 실행하지 않음.
        /// </summary>
        /// <returns></returns>
        public int MoveThetaAlignPosB(double distance)
        {  
            // Pos B로 이동한다.            
            return m_RefComp.Stage.MoveStageRelativeX(distance);

        }

        private void SetThetaAlignPosA(CPos_XYTZ pPos)
        {
            m_Data.Align.ThetaTeachPos[(int)EThetaAlignPos.POS_A] = pPos.Copy();
        }

        private CPos_XYTZ GetThetaAlignPosA()
        {
            return m_Data.Align.ThetaTeachPos[(int)EThetaAlignPos.POS_A];
        }

        private void SetThetaAlignPosB(CPos_XYTZ pPos)
        {
            m_Data.Align.ThetaTeachPos[(int)EThetaAlignPos.POS_B] = pPos.Copy();
        }
        
        private CPos_XYTZ GetThetaAlignPosB()
        {
            return m_Data.Align.ThetaTeachPos[(int)EThetaAlignPos.POS_B];
        }

        private void CalsThetaAlign()
        {
            var stagePos1 = new CPos_XYTZ();
            var stagePos2 = new CPos_XYTZ();
            var markPos1 = new CPos_XYTZ();
            var markPos2 = new CPos_XYTZ();
            var alignAdjPos = new CPos_XYTZ();
            var alignTeachPos = new CPos_XYTZ();
            
            // A위치를 읽어온다.
            stagePos1 = GetThetaAlignPosA();

            // B위치를 읽어온다. (현재 위치)
            m_RefComp.Stage.GetStageCurPos(out stagePos2);

            // 세로축으로 변화를 확인한다.
            if ((stagePos1.dY - stagePos2.dY) != 0.0)
            {
                // Mark 좌표를 Stage 위치를 고려해서 Stage 회전 중심 기준으로 변환한다.
                markPos1 = ChagneStageToMarkPos(stagePos1.Copy(), markPos1);
                markPos2 = ChagneStageToMarkPos(stagePos2.Copy(), markPos2);

                // Theta Align은 연산
                CalsThetaAlign(markPos1, markPos2, out alignAdjPos);

                // PosA 위치 Offset 적용
                alignTeachPos.dX = stagePos1.dX - alignAdjPos.dX;
                alignTeachPos.dY = stagePos1.dY - alignAdjPos.dY;
                alignTeachPos.dT = stagePos1.dT - alignAdjPos.dT;

                SetThetaAlignPosA(alignTeachPos);
            }
        }

        /// <summary>
        /// Stage의 좌표에서 X,Y 값만 Mark 좌표로 변환한다.
        /// (T축은 변화를 주지 않는다) 
        /// </summary>
        /// <param name="pStage : Stage의 좌표값"></param>
        /// <returns></returns>
        private CPos_XYTZ ChagneStageToMarkPos(CPos_XYTZ pStage, CPos_XY pMarkPos)
        {
            var posMark = new CPos_XYTZ();
            var posCenter = new CPos_XYTZ();

            // 사용하는 Camera의 중심 위치를 구함.
            if (GetCurrentCam() == PRE__CAM)
            {
                posCenter = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER_PRE);
            }
            if (GetCurrentCam() == FINE_CAM)
            {
                posCenter = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER_PRE);

                posCenter.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                posCenter.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
            }

            // Stage의 위치와 차로.. Camera 중심위치 대비 위치를 확인함.
            posMark.dX = posCenter.dX - pStage.dX;
            posMark.dY = posCenter.dY - pStage.dY;
            posMark.dT = pStage.dT;

            // Offset이 있을 경우 적용함.
            posMark.dX = posMark.dX + MarkToPos(pMarkPos).dX;
            posMark.dY = posMark.dY + MarkToPos(pMarkPos).dY;

            return posMark;
        }

        private CPos_XYTZ ChagneStageToMarkPos(CPos_XYTZ pStage, CPos_XYTZ pOffSet)
        {
            var posMark = new CPos_XYTZ();

            // 사용하는 Camera의 중심 위치를 구함.
            if (GetCurrentCam() == PRE__CAM)
            {
                posMark = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER_PRE);
            }
            if (GetCurrentCam() == FINE_CAM)
            {
                posMark = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER_PRE);

                posMark.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                posMark.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
            }

            // Stage의 위치와 차로.. Camera 중심위치 대비 위치를 확인함.
            posMark -= pStage;

            // Offset이 있을 경우 적용함.
            posMark += MarkToPos(pOffSet);

            return posMark;
        }

        /// <summary>
        /// Mark를 카메라의 중심을 기준으로 실제 위치(Calibration)로 변환함.
        /// </summary>
        /// <param name="pMark"> 카메라상의 Pixel 위치</param>
        /// <returns> Calibration된 카메라 중심으로한 실제 위치</returns>
        private CPos_XYTZ MarkToPos(CPos_XYTZ pMark)
        {
            int iCamNum = GetCurrentCam();
            var posMark = new CPos_XYTZ();
            var camPixelNum = new Size();
            var markPoint = new CPos_XYTZ();

            // 중심 위치를 기준으로 좌표를 설정함
            camPixelNum = m_RefComp.Vision.GetCameraPixelNum(iCamNum);
            markPoint.dX = (double)camPixelNum.Width / 2 + pMark.dX;
            markPoint.dY = (double)camPixelNum.Height / 2 - pMark.dY;

            // Resolution에 따른 실제 위치값을 계산함
            posMark.dX = markPoint.dX * CMainFrame.DataManager.SystemData_Align.PixelResolution[iCamNum]/1000;
            posMark.dY = markPoint.dY * CMainFrame.DataManager.SystemData_Align.PixelResolution[iCamNum]/1000;

            // 카메라의 설치 회전 오차값을 적용함.
            posMark.dX = Math.Cos(DegToRad(CMainFrame.DataManager.SystemData_Align.CameraTilt[iCamNum])) * posMark.dX -
                         Math.Sin(DegToRad(CMainFrame.DataManager.SystemData_Align.CameraTilt[iCamNum])) * posMark.dY;

            posMark.dY = Math.Sin(DegToRad(CMainFrame.DataManager.SystemData_Align.CameraTilt[iCamNum])) * posMark.dX +
                         Math.Cos(DegToRad(CMainFrame.DataManager.SystemData_Align.CameraTilt[iCamNum])) * posMark.dY;

            return posMark;

        }

        private CPos_XY MarkToPos(CPos_XY pMark)
        {
            int iCamNum = GetCurrentCam();
            var posMark = new CPos_XY();
            var camPixelNum = new Size();
            var markPoint = new CPos_XY();

            // 중심 위치를 기준으로 좌표를 설정함
            camPixelNum = m_RefComp.Vision.GetCameraPixelNum(iCamNum);
            markPoint.dX = -(double)camPixelNum.Width / 2 + pMark.dX;
            markPoint.dY = (double)camPixelNum.Height / 2 - pMark.dY;

            // Resolution에 따른 실제 위치값을 계산함
            posMark.dX = markPoint.dX * CMainFrame.DataManager.SystemData_Align.PixelResolution[iCamNum] / 1000;
            posMark.dY = markPoint.dY * CMainFrame.DataManager.SystemData_Align.PixelResolution[iCamNum] / 1000;

            // 카메라의 설치 회전 오차값을 적용함.
            posMark.dX = Math.Cos(DegToRad(CMainFrame.DataManager.SystemData_Align.CameraTilt[iCamNum])) * posMark.dX -
                         Math.Sin(DegToRad(CMainFrame.DataManager.SystemData_Align.CameraTilt[iCamNum])) * posMark.dY;

            posMark.dY = Math.Sin(DegToRad(CMainFrame.DataManager.SystemData_Align.CameraTilt[iCamNum])) * posMark.dX +
                         Math.Cos(DegToRad(CMainFrame.DataManager.SystemData_Align.CameraTilt[iCamNum])) * posMark.dY;

            return posMark;

        }
        public void InitRotateCenterCals()
        {
            eRotateCenterStep = ERotateCenterStep.INIT;
        }

        public int DoRotateCenterCals(ref CPos_XYTZ RotateCenter)
        {
            // Data 초기화
            CPos_XYTZ rotateCenter = new CPos_XYTZ();

            if (GetCurrentCam() == PRE__CAM)
            {
                if (eRotateCenterStep == ERotateCenterStep.PRE_POS_B || eRotateCenterStep == ERotateCenterStep.FINE_POS_B)
                {
                    MoveRotateCenterPrePosB(ref rotateCenter);
                    // 결과값 리턴
                    RotateCenter = rotateCenter.Copy();
                }
                else
                    MoveRotateCenterPrePosA();
            }

            if (GetCurrentCam() == FINE_CAM)
            {
                if (eRotateCenterStep == ERotateCenterStep.PRE_POS_B || eRotateCenterStep == ERotateCenterStep.FINE_POS_B)
                {
                    MoveRotateCenterFinePosB(ref rotateCenter);

                    // 결과값 리턴
                    RotateCenter = rotateCenter.Copy();
                }
                else
                    MoveRotateCenterFinePosA();
            }            

            return SUCCESS;
        }

        private int MoveRotateCenterPrePosA()
        {
            int iResult=0;
            var stageCurPos = new CPos_XYTZ();
            var stageMovePos = new CPos_XYTZ();

            var markPos1 = new CPos_XYTZ();
            var markPos2 = new CPos_XYTZ();
            var rotateCenter = new CPos_XYTZ();

            // 현재 위치를 확인한다.
            m_RefComp.Stage.GetStageCurPos(out stageCurPos);

            if (eRotateCenterStep == ERotateCenterStep.INIT)
            {
                // 초기 위치를 저장한다.
                StageRotatePos[(int)ERotateCenterStep.INIT] = stageCurPos;

                // Pos A로 이동한다. ( CW로 회전한다 )
                var moveMark = new CPos_XYTZ();
                moveMark = ChagneStageToMarkPos(stageCurPos, moveMark);
                moveMark.dT = -5;

                //  회전 변환 했을 경우 X,Y 이동 좌표 연산
                stageMovePos.dX = Math.Cos(DegToRad(moveMark.dT)) * (moveMark.dX) - Math.Sin(DegToRad(moveMark.dT)) * (moveMark.dY);
                stageMovePos.dY = Math.Sin(DegToRad(moveMark.dT)) * (moveMark.dX) + Math.Cos(DegToRad(moveMark.dT)) * (moveMark.dY);

                // Stage는 CW가 (+)
                stageMovePos.dX = -(stageMovePos.dX - moveMark.dX);
                stageMovePos.dY = -(stageMovePos.dY - moveMark.dY);
                stageMovePos.dT = -moveMark.dT;

                iResult = m_RefComp.Stage.MoveStageRelative(stageMovePos);

                eRotateCenterStep = ERotateCenterStep.PRE_POS_A;

                return SUCCESS;
            }

            if (eRotateCenterStep == ERotateCenterStep.PRE_POS_A)
            {
                // A위치를 읽어온다. (현재 위치)
                StageRotatePos[(int)ERotateCenterStep.PRE_POS_A] = stageCurPos;

                // Init으로 이동한다. ( CW로 회전한다 )
                stageMovePos = StageRotatePos[(int)ERotateCenterStep.INIT].Copy();
                m_RefComp.Stage.MoveStagePos(stageMovePos);

                Sleep(1000);

                // Pos B로 이동한다. ( CCW로 회전한다 )
                var moveMark = new CPos_XYTZ();
                moveMark = ChagneStageToMarkPos(StageRotatePos[(int)ERotateCenterStep.INIT], moveMark);
                moveMark.dT = 5;

                // 회전 변환 했을 경우 X,Y 이동 좌표 연산
                stageMovePos.dX = Math.Cos(DegToRad(moveMark.dT)) * (moveMark.dX) - Math.Sin(DegToRad(moveMark.dT)) * (moveMark.dY);
                stageMovePos.dY = Math.Sin(DegToRad(moveMark.dT)) * (moveMark.dX) + Math.Cos(DegToRad(moveMark.dT)) * (moveMark.dY);

                // Stage는 CW가 (+)
                stageMovePos.dX = -(stageMovePos.dX - moveMark.dX);
                stageMovePos.dY = -(stageMovePos.dY - moveMark.dY);
                stageMovePos.dT = -moveMark.dT;

                iResult = m_RefComp.Stage.MoveStageRelative(stageMovePos);

                eRotateCenterStep = ERotateCenterStep.PRE_POS_B;
            }

            if (eRotateCenterStep == ERotateCenterStep.FINE_POS_A)
            {
                // A위치를 읽어온다. (현재 위치)
                StageRotatePos[(int)ERotateCenterStep.FINE_POS_A] = stageCurPos;

                // Init으로 이동한다. ( CW로 회전한다 )
                stageMovePos = StageRotatePos[(int)ERotateCenterStep.INIT].Copy();
                m_RefComp.Stage.MoveStagePos(stageMovePos);

                Sleep(1000);

                // Pos B로 이동한다. ( CCW로 회전한다 )
                var moveMark = new CPos_XYTZ();
                moveMark.dX = -StageRotatePos[(int)ERotateCenterStep.MARK_POS].dX;
                moveMark.dY = -StageRotatePos[(int)ERotateCenterStep.MARK_POS].dY;
                moveMark.dT =  45;

                // 45도 회전 변환 했을 경우 X,Y 이동 좌표 연산
                stageMovePos.dX = Math.Cos(DegToRad(moveMark.dT)) * (moveMark.dX) - Math.Sin(DegToRad(moveMark.dT)) * (moveMark.dY);
                stageMovePos.dY = Math.Sin(DegToRad(moveMark.dT)) * (moveMark.dX) + Math.Cos(DegToRad(moveMark.dT)) * (moveMark.dY);

                // Stage는 CW가 (+)
                stageMovePos.dX = -(stageMovePos.dX - moveMark.dX);
                stageMovePos.dY = -(stageMovePos.dY - moveMark.dY);
                stageMovePos.dT = -moveMark.dT;

                iResult = m_RefComp.Stage.MoveStageRelative(stageMovePos);
                

                eRotateCenterStep = ERotateCenterStep.FINE_POS_B;
            }

            return iResult;            
        }

        private int MoveRotateCenterPrePosB(ref CPos_XYTZ RotateCenter)
        {
            int iResult = 0;
            var stageCurPos = new CPos_XYTZ();
            var stageMovePos = new CPos_XYTZ();

            var markPos1 = new CPos_XYTZ();
            var markPos2 = new CPos_XYTZ();
            var rotateCenter = new CPos_XYTZ();

            var alignTeachPos = new CPos_XYTZ();

            // 현재 위치를 확인한다
            m_RefComp.Stage.GetStageCurPos(out stageCurPos);

            if (eRotateCenterStep == ERotateCenterStep.PRE_POS_B)
            {
                // B위치를 읽어온다. (현재 위치)
                StageRotatePos[(int)ERotateCenterStep.PRE_POS_B] = stageCurPos;

                // 처음 시작 Point를 기준으로 위치 확인 좌표 A,B를 계산한다.
                markPos1.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - StageRotatePos[(int)ERotateCenterStep.PRE_POS_A].dX;
                markPos1.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - StageRotatePos[(int)ERotateCenterStep.PRE_POS_A].dY;

                markPos2.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - StageRotatePos[(int)ERotateCenterStep.PRE_POS_B].dX;
                markPos2.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - StageRotatePos[(int)ERotateCenterStep.PRE_POS_B].dY;

                // Cam의 위치를 기준으로 회전 중심을 계산한다.
                StageRotatePos[(int)ERotateCenterStep.MARK_POS] = CalsRotateCenter(markPos1, markPos2, 10.0);

                // 초기위치 이동한다. ( CW로 회전한다 )
                stageMovePos = StageRotatePos[(int)ERotateCenterStep.INIT].Copy();
                m_RefComp.Stage.MoveStagePos(stageMovePos);

                Sleep(1000);

                // Stage를 기준으로 Cam의 중심 위치를 확인함.
                var moveMark = new CPos_XYTZ();
                moveMark.dX = -StageRotatePos[(int)ERotateCenterStep.MARK_POS].dX;
                moveMark.dY = -StageRotatePos[(int)ERotateCenterStep.MARK_POS].dY;
                moveMark.dT = -45;

                // -45도 회전 변환 했을 경우 X,Y 이동 좌표 연산
                stageMovePos.dX = Math.Cos(DegToRad(moveMark.dT)) * (moveMark.dX) - Math.Sin(DegToRad(moveMark.dT)) * (moveMark.dY);
                stageMovePos.dY = Math.Sin(DegToRad(moveMark.dT)) * (moveMark.dX) + Math.Cos(DegToRad(moveMark.dT)) * (moveMark.dY);

                // Stage는 CW가 (+)
                stageMovePos.dX = -(stageMovePos.dX - moveMark.dX);
                stageMovePos.dY = -(stageMovePos.dY - moveMark.dY);
                stageMovePos.dT = -moveMark.dT;

                iResult = m_RefComp.Stage.MoveStageRelative(stageMovePos);

                eRotateCenterStep = ERotateCenterStep.FINE_POS_A;
            }

            if (eRotateCenterStep == ERotateCenterStep.FINE_POS_B)
            {
                // B위치를 읽어온다. (현재 위치)
                StageRotatePos[(int)ERotateCenterStep.FINE_POS_B] = stageCurPos.Copy();

                // 처음 시작 Point를 기준으로 위치 확인 좌표 A,B를 계산한다.
                markPos1.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - StageRotatePos[(int)ERotateCenterStep.FINE_POS_A].dX;
                markPos1.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - StageRotatePos[(int)ERotateCenterStep.FINE_POS_A].dY;

                markPos2.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - StageRotatePos[(int)ERotateCenterStep.FINE_POS_B].dX;
                markPos2.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - StageRotatePos[(int)ERotateCenterStep.FINE_POS_B].dY;

                // 회전 중심을 계산한다.
                rotateCenter = CalsRotateCenter(markPos1, markPos2, 90.0);

                // Stage의 중심을 재 설정한다.
                RotateCenter.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - rotateCenter.dX;
                RotateCenter.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - rotateCenter.dY;
                RotateCenter.dT = 0.0;

                //CMainFrame.DataManager.Pos_Fixed.Pos_Stage1.Pos[(int)EStagePos.STAGE_CENTER_PRE].dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - rotateCenter.dX;
                //CMainFrame.DataManager.Pos_Fixed.Pos_Stage1.Pos[(int)EStagePos.STAGE_CENTER_PRE].dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - rotateCenter.dY;
                //CMainFrame.DataManager.Pos_Fixed.Pos_Stage1.Pos[(int)EStagePos.STAGE_CENTER_PRE].dT = 0.0;                

                //CMainFrame.LWDicer.SavePosition(CMainFrame.DataManager.Pos_Fixed, true, EPositionObject.STAGE1);

                // Init으로 이동한다. ( CW로 회전한다 )
                stageMovePos = StageRotatePos[(int)ERotateCenterStep.INIT].Copy();
                m_RefComp.Stage.MoveStagePos(stageMovePos);

            }

            return iResult;
            //return (int)eRotateCenterStep;
        }


        private int MoveRotateCenterFinePosA()
        {
            int iResult = 0;
            var stageCurPos = new CPos_XYTZ();
            var stageMovePos = new CPos_XYTZ();

            var markPos1 = new CPos_XYTZ();
            var markPos2 = new CPos_XYTZ();
            var rotateCenter = new CPos_XYTZ();

            // 현재 위치를 확인한다.
            m_RefComp.Stage.GetStageCurPos(out stageCurPos);

            if (eRotateCenterStep == ERotateCenterStep.INIT)
            {
                // 초기 위치를 저장한다.
                StageRotatePos[(int)ERotateCenterStep.INIT] = stageCurPos.Copy();

                // Pos A로 이동한다. ( CCW로 회전한다 )
                var moveMark = new CPos_XYTZ();
                moveMark = ChagneStageToMarkPos(stageCurPos, moveMark);
                moveMark.dT = -1;

                // 회전 변환 했을 경우 X,Y 이동 좌표 연산
                var RadAngle = DegToRad(moveMark.dT);
                stageMovePos.dX = Math.Cos(RadAngle) * (moveMark.dX) - Math.Sin(RadAngle) * (moveMark.dY);
                stageMovePos.dY = Math.Sin(RadAngle) * (moveMark.dX) + Math.Cos(RadAngle) * (moveMark.dY);

                // Stage는 CW가 (+)
                stageMovePos.dX = -(stageMovePos.dX - moveMark.dX);
                stageMovePos.dY = -(stageMovePos.dY - moveMark.dY);
                stageMovePos.dT = -moveMark.dT;

                iResult = m_RefComp.Stage.MoveStageRelative(stageMovePos);

                eRotateCenterStep = ERotateCenterStep.PRE_POS_A;
                
                return SUCCESS;
            }

            if (eRotateCenterStep == ERotateCenterStep.PRE_POS_A)
            {
                // A위치를 읽어온다. (현재 위치)
                StageRotatePos[(int)ERotateCenterStep.PRE_POS_A] = stageCurPos.Copy();

                // Init으로 이동한다. ( CW로 회전한다 )
                stageMovePos = StageRotatePos[(int)ERotateCenterStep.INIT].Copy();
                m_RefComp.Stage.MoveStagePos(stageMovePos);

                Sleep(1000);

                // Pos B로 이동한다. ( CCW로 회전한다 )
                var moveMark = new CPos_XYTZ();
                moveMark = ChagneStageToMarkPos(StageRotatePos[(int)ERotateCenterStep.INIT], moveMark);
                moveMark.dT = 1;

                // 회전 변환 했을 경우 X,Y 이동 좌표 연산
                var RadAngle = DegToRad(moveMark.dT);
                stageMovePos.dX = Math.Cos(RadAngle) * (moveMark.dX) - Math.Sin(RadAngle) * (moveMark.dY);
                stageMovePos.dY = Math.Sin(RadAngle) * (moveMark.dX) + Math.Cos(RadAngle) * (moveMark.dY);

                // Stage는 CW가 (+)
                stageMovePos.dX = -(stageMovePos.dX - moveMark.dX);
                stageMovePos.dY = -(stageMovePos.dY - moveMark.dY);
                stageMovePos.dT = -moveMark.dT;

                iResult = m_RefComp.Stage.MoveStageRelative(stageMovePos);

                eRotateCenterStep = ERotateCenterStep.PRE_POS_B;
            }


            if (eRotateCenterStep == ERotateCenterStep.FINE_POS_A)
            {
                // A위치를 읽어온다. (현재 위치)
                StageRotatePos[(int)ERotateCenterStep.FINE_POS_A] = stageCurPos.Copy();

                // Init으로 이동한다. ( CW로 회전한다 )
                stageMovePos = StageRotatePos[(int)ERotateCenterStep.INIT].Copy();
                m_RefComp.Stage.MoveStagePos(stageMovePos);

                Sleep(1000);

                // Pos B로 이동한다. ( CCW로 회전한다 )
                // Stage를 기준으로 Cam의 중심 위치를 확인함.
                var moveMark = new CPos_XYTZ();
                moveMark.dX = -StageRotatePos[(int)ERotateCenterStep.MARK_POS].dX;
                moveMark.dY = -StageRotatePos[(int)ERotateCenterStep.MARK_POS].dY;
                moveMark.dT = 45;

                // 회전 변환 했을 경우 X,Y 이동 좌표 연산
                var RadAngle = DegToRad(moveMark.dT);
                stageMovePos.dX = Math.Cos(RadAngle) * (moveMark.dX) - Math.Sin(RadAngle) * (moveMark.dY);
                stageMovePos.dY = Math.Sin(RadAngle) * (moveMark.dX) + Math.Cos(RadAngle) * (moveMark.dY);

                // Stage는 CW가 (+)
                stageMovePos.dX = -(stageMovePos.dX - moveMark.dX);
                stageMovePos.dY = -(stageMovePos.dY - moveMark.dY);
                stageMovePos.dT = -moveMark.dT;

                iResult = m_RefComp.Stage.MoveStageRelative(stageMovePos);


                eRotateCenterStep = ERotateCenterStep.FINE_POS_B;
            }

            return iResult;
        }

        private int MoveRotateCenterFinePosB(ref CPos_XYTZ RotateCenter)
        {
            int iResult = 0;
            var stageCurPos = new CPos_XYTZ();
            var stageMovePos = new CPos_XYTZ();

            var markPos1 = new CPos_XYTZ();
            var markPos2 = new CPos_XYTZ();
            var rotateCenter = new CPos_XYTZ();

            var alignTeachPos = new CPos_XYTZ();

            // 현재 위치를 확인한다
            m_RefComp.Stage.GetStageCurPos(out stageCurPos);

            if (eRotateCenterStep == ERotateCenterStep.PRE_POS_B)
            {
                // B위치를 읽어온다. (현재 위치)
                StageRotatePos[(int)ERotateCenterStep.PRE_POS_B] = stageCurPos.Copy();

                // 처음 시작 Point를 기준으로 위치 확인 좌표 A,B를 계산한다.
                markPos1.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - StageRotatePos[(int)ERotateCenterStep.PRE_POS_A].dX;
                markPos1.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - StageRotatePos[(int)ERotateCenterStep.PRE_POS_A].dY;

                markPos2.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - StageRotatePos[(int)ERotateCenterStep.PRE_POS_B].dX;
                markPos2.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - StageRotatePos[(int)ERotateCenterStep.PRE_POS_B].dY;

                // Cam의 위치를 기준으로 회전 중심을 계산한다.
                StageRotatePos[(int)ERotateCenterStep.MARK_POS] = CalsRotateCenter(markPos1, markPos2, 2.0);

                // 초기위치 이동한다. ( CW로 회전한다 )
                stageMovePos = StageRotatePos[(int)ERotateCenterStep.INIT].Copy();
                m_RefComp.Stage.MoveStagePos(stageMovePos);

                Sleep(1000);

                // Stage를 기준으로 Cam의 중심 위치를 확인함.
                var moveMark = new CPos_XYTZ();
                moveMark.dX = -StageRotatePos[(int)ERotateCenterStep.MARK_POS].dX;
                moveMark.dY = -StageRotatePos[(int)ERotateCenterStep.MARK_POS].dY;
                moveMark.dT = -45;
                

                // 회전 변환 했을 경우 X,Y 이동 좌표 연산
                var RadAngle = DegToRad(moveMark.dT);
                stageMovePos.dX = Math.Cos(RadAngle) * (moveMark.dX) - Math.Sin(RadAngle) * (moveMark.dY);
                stageMovePos.dY = Math.Sin(RadAngle) * (moveMark.dX) + Math.Cos(RadAngle) * (moveMark.dY);

                // Stage는 CW가 (+)
                stageMovePos.dX = -(stageMovePos.dX - moveMark.dX);
                stageMovePos.dY = -(stageMovePos.dY - moveMark.dY);
                stageMovePos.dT = -moveMark.dT;

                iResult = m_RefComp.Stage.MoveStageRelative(stageMovePos);

                eRotateCenterStep = ERotateCenterStep.FINE_POS_A;
            }

            if (eRotateCenterStep == ERotateCenterStep.FINE_POS_B)
            {
                // B위치를 읽어온다. (현재 위치)
                StageRotatePos[(int)ERotateCenterStep.FINE_POS_B] = stageCurPos.Copy();

                // 처음 시작 Point를 기준으로 위치 확인 좌표 A,B를 계산한다.
                markPos1.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - StageRotatePos[(int)ERotateCenterStep.FINE_POS_A].dX;
                markPos1.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - StageRotatePos[(int)ERotateCenterStep.FINE_POS_A].dY;

                markPos2.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - StageRotatePos[(int)ERotateCenterStep.FINE_POS_B].dX;
                markPos2.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - StageRotatePos[(int)ERotateCenterStep.FINE_POS_B].dY;

                // 회전 중심을 계산한다.
                rotateCenter = CalsRotateCenter(markPos1, markPos2, 90.0);

                // Stage의 중심을 재 설정한다.
                RotateCenter.dX = StageRotatePos[(int)ERotateCenterStep.INIT].dX - rotateCenter.dX;
                RotateCenter.dY = StageRotatePos[(int)ERotateCenterStep.INIT].dY - rotateCenter.dY;
                RotateCenter.dT = 0.0;

                // Init으로 이동한다. ( CW로 회전한다 )
                stageMovePos = StageRotatePos[(int)ERotateCenterStep.INIT].Copy();
                m_RefComp.Stage.MoveStagePos(stageMovePos);

            }
            return iResult;
        }

        private CPos_XYTZ CalsRotateCenter(CPos_XYTZ mark1, CPos_XYTZ mark2, double dAngle)
        {
            var rotateCenter = new CPos_XYTZ();
            double sin = Math.Sin(DegToRad(dAngle));
            double cos = Math.Cos(DegToRad(dAngle));

            // 역회전 공식으로 유도한 회전 중심 공식
            rotateCenter.dX = (mark1.dX + mark2.dX) / 2 + (sin * (mark1.dY - mark2.dY)) / (2 * (1 - cos));
            rotateCenter.dY = (mark1.dY + mark2.dY) / 2 + ((1 + cos) * (mark2.dX - mark1.dX)) / (2 * sin);

            return rotateCenter;
        }

        public int LaserAlignInit()
        {
            var stagePos = new CPos_XYTZ();
            // 현재 위치와 Theta Align  기준이 A 위치를 확인함
            m_RefComp.Stage.GetStageCurPos(out stagePos);
            // 현재 위치를 ThetaAlignPosA로 저장한다. 
            SetThetaAlignPosA(stagePos);
            
            return SUCCESS;
        }
        public int MoveLaserAlignPosA()
        {
            CalsThetaAlign();
            MoveThetaAlignPosA();
            return SUCCESS;
        }

        public int MoveLaserAlignPosB(double distance)
        {
            MoveThetaAlignPosB(distance);
            return SUCCESS;
        }

        /// <summary>
        /// GUI에서 실행할  ThetaAlign 명령
        /// 위치 상태에 따라서 A,B 위치를 번갈아 이동함.
        /// </summary>
        /// <returns></returns>
        public int DoThetaAlign()
        {
            bool checkPos;
            var defaultPosA = new CPos_XYTZ();
            var defaultPosB = new CPos_XYTZ();
            var deviatePosA = new CPos_XYTZ();
            var deviatePosB = new CPos_XYTZ();

            var stagePos = new CPos_XYTZ();
            var stagePos1 = new CPos_XYTZ();
            var stagePos2 = new CPos_XYTZ();

            // ----------------------------------------------------------
            // 현재 위치와 Theta Align  기준이 A 위치를 확인함
            m_RefComp.Stage.GetStageCurPos(out stagePos);

            // Stage의 중심 위치를 읽어옴.
            int indexPos = 0;

            if (GetCurrentCam() == PRE__CAM) indexPos = (int)EStagePos.STAGE_CENTER_PRE;
            if (GetCurrentCam() == FINE_CAM) indexPos = (int)EStagePos.STAGE_CENTER_FINE;

            defaultPosA = m_RefComp.Stage.GetTargetPosition(indexPos);
            defaultPosB = m_RefComp.Stage.GetTargetPosition(indexPos);

            // Stage Center위치에서  Align 간격의 절반 거리로 위치를 정함.
            defaultPosA.dX -= CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen / 2;
            defaultPosB.dX += CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen / 2;

            // 각도가 일정한 값 (2도) 이상일 경우에 0 혹은 90도로 초기화 함.
            if (Math.Abs(stagePos.dT) < 2) ;
            else if (Math.Abs(stagePos.dT) > 2 && Math.Abs(stagePos.dT) < 88) stagePos.dT = 0.0;
            else if (Math.Abs(stagePos.dT) < 88 || Math.Abs(stagePos.dT) > 92) stagePos.dT = 90.0;

            // 편차를 구함
            deviatePosA = stagePos - defaultPosA;
            deviatePosB = stagePos - defaultPosB;
            // 편차 거리를 구함
            double distancePosA = Math.Sqrt(deviatePosA.dX * deviatePosA.dX + deviatePosA.dY * deviatePosA.dY);
            double distancePosB = Math.Sqrt(deviatePosB.dX * deviatePosB.dX + deviatePosB.dY * deviatePosB.dY);

            if (distancePosA < 10)
            {
                // 현재 위치를 ThetaAlignPosA로 저장한다. 
                SetThetaAlignPosA(stagePos);

                MoveThetaAlignPosB(CMainFrame.DataManager.SystemData_Align.AlignMarkWidthLen);
            }
            else if (distancePosB < 10)
            {
                CalsThetaAlign();
                MoveThetaAlignPosA();
            }
            else
            {
                defaultPosA.dT = stagePos.dT;
                SetThetaAlignPosA(defaultPosA);
                MoveThetaAlignPosA();
            }

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
                RotateCenter = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER_PRE);
            }
            if (GetCurrentCam() == FINE_CAM)
            {
                RotateCenter = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER_PRE);

                RotateCenter.dX -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dX;
                RotateCenter.dY -= CMainFrame.DataManager.SystemData_Align.CamEachOffset.dY;
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
        

        /// <summary>
        /// UI에서 User가 Button을 클릭하면서 4 Point 위치값 Offset조절
        /// </summary>
        /// <returns></returns>
        public int SetEdgeTeachPosNext(ref CCtrlAlignData pAlignData)
        {
            var posEdge     = new CPos_XY();
            var posStage    = new CPos_XYTZ();
            var posCenter   = new CPos_XYTZ();
            var posTeach    = new CPos_XYTZ();

            int iResult = -1;

            // 현재 위치 값을 읽어온다.
            iResult = m_RefComp.Stage.GetStageCurPos(out posStage);
            if (iResult != SUCCESS) return iResult;

            // Stage Center 값을 읽어온다
            posCenter = m_RefComp.Stage.GetStageTeachPos((int)EStagePos.STAGE_CENTER_PRE);

            // Edge Pos 1 위치 이동
            if (eEdgeTeachStep == EEdgeAlignTeachPos.INIT)
            {
                // Edge 영역을 설정한다.
                m_RefComp.Vision.SetEdgeFinderArea(PRE__CAM);

                // Pos1으로 이동함.
                double dLength = WAFER_SIZE_12_INCH / 2.0 * Math.Cos(Math.PI / 180 * 45);                

                posTeach.dX = CMainFrame.DataManager.Pos_Fixed.Pos_Stage1.Pos[(int)EStagePos.STAGE_CENTER_PRE].dX + dLength;
                posTeach.dY = CMainFrame.DataManager.Pos_Fixed.Pos_Stage1.Pos[(int)EStagePos.STAGE_CENTER_PRE].dY - dLength;
                posTeach.dT = 0.0;

                iResult = m_RefComp.Stage.MoveStagePos(posCenter + posTeach);
                
                Sleep(1000);
                FindEdgePoint(out posEdge);

                if (iResult == SUCCESS) eEdgeTeachStep = EEdgeAlignTeachPos.POS1;

                return iResult;
            }

            // Edge Pos 1 저장 및 Edge Pos 2 위치 이동
            if (eEdgeTeachStep == EEdgeAlignTeachPos.POS1)
            {
                // 매뉴얼로 Edge를 Center로 이동함.
                                
                // Stage 위치 저장
                //CMainFrame.LWDicer.m_DataManager.ModelData.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS1] = posStage.Copy();
                pAlignData.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS1] = posStage.Copy();

                // Pos2으로 이동함.
                iResult = MoveStageRelativeT(90.0);
                if (iResult != SUCCESS) return iResult;

                Sleep(1000);
                FindEdgePoint(out posEdge);

                eEdgeTeachStep = EEdgeAlignTeachPos.POS2;

                return iResult;
            }
            // Edge Pos 2 저장 및 Edge Pos 3 위치 이동
            if (eEdgeTeachStep == EEdgeAlignTeachPos.POS2)
            {
                // 매뉴얼로 Edge를 Center로 이동함.

                // Stage 위치를 저장함.
                //CMainFrame.LWDicer.m_DataManager.ModelData.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS2] = posStage.Copy();
                pAlignData.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS2] = posStage.Copy();

                // Pos3으로 이동함.
                iResult = MoveStageRelativeT(90.0);
                if (iResult != SUCCESS) return iResult;

                Sleep(1000);
                FindEdgePoint(out posEdge);

                eEdgeTeachStep = EEdgeAlignTeachPos.POS3;

                return SUCCESS;
            }
            // Edge Pos 3 저장 및 동작 종료
            if (eEdgeTeachStep == EEdgeAlignTeachPos.POS3)
            {
                // 매뉴얼로 Edge를 Center로 이동함.
                
                // Stage 위치를 저장함.
                //CMainFrame.LWDicer.m_DataManager.ModelData.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS3] = posStage.Copy();
                pAlignData.EdgeTeachPos[(int)EEdgeAlignTeachPos.POS3] = posStage.Copy();

                //MoveToEdgeAlignTeachPos1();

                eEdgeTeachStep = EEdgeAlignTeachPos.INIT;

                return SUCCESS;
            }

            return SUCCESS;
        }
        
        /// <summary> 
        /// 3 점으로부터 원의 중심점을 구해온다. 
        /// </summary> 
        /// <param>시작 점</param> 
        /// <param>중간 점</param> 
        /// <param>종료 점</param> 
        /// <returns>원의 중심 점</returns> 
        public CPos_XY CalsCenterOf3P(CPos_XY Pos1, CPos_XY Pos2, CPos_XY Pos3)
        {
            double d1 = (Pos2.dY - Pos1.dY) / (Pos2.dX - Pos1.dX);
            double d2 = (Pos3.dY - Pos2.dY) / (Pos3.dX - Pos2.dX);

            if (double.IsInfinity(d1))
                d1 = 1000000000000;
            if (double.IsInfinity(d2))
                d2 = 1000000000000;
            if (d1 == 0f)
                d1 = 0.000000000001;
            if (d2 == 0f)
                d2 = 0.000000000001;

            //원의 중점을 구함
            // Cx = ((y1 - y2) * d1 * d2  + (x1 + x2) * d2 - (x2 + x3) * d1) / (2 * (d2 - d1))
            // Cy = -(Cx - (x1 + x2) / 2) /d1 + (y1 + y2) / 2

            double centerX = (d1 * d2 * (Pos1.dY - Pos3.dY) +
                              d2 * (Pos1.dX + Pos2.dX) -
                              d1 * (Pos2.dX + Pos3.dX)) / (2 * (d2 - d1));
            double centerY = (-1 * (centerX - (Pos1.dX + Pos2.dX) / 2) / d1) +
                             ((Pos1.dY + Pos2.dY) / 2);

            return new CPos_XY(centerX,centerY);
        }

        public CPos_XY CalsCenterOf3P(CPos_XYTZ Pos1, CPos_XYTZ Pos2, CPos_XYTZ Pos3)
        {
            double d1 = (Pos2.dY - Pos1.dY) / (Pos2.dX - Pos1.dX);
            double d2 = (Pos3.dY - Pos2.dY) / (Pos3.dX - Pos2.dX);

            if (double.IsInfinity(d1))
                d1 = 1000000000000;
            if (double.IsInfinity(d2))
                d2 = 1000000000000;
            if (d1 == 0f)
                d1 = 0.000000000001;
            if (d2 == 0f)
                d2 = 0.000000000001;

            //원의 중점을 구함
            // Cx = ((y1 - y2) * d1 * d2  + (x1 + x2) * d2 - (x2 + x3) * d1) / (2 * (d2 - d1))
            // Cy = -(Cx - (x1 + x2) / 2) /d1 + (y1 + y2) / 2

            double centerX = (d1 * d2 * (Pos1.dY - Pos3.dY) +
                              d2 * (Pos1.dX + Pos2.dX) -
                              d1 * (Pos2.dX + Pos3.dX)) / (2 * (d2 - d1));
            double centerY = (-1 * (centerX - (Pos1.dX + Pos2.dX) / 2) / d1) +
                             ((Pos1.dY + Pos2.dY) / 2);

            return new CPos_XY(centerX, centerY);
        }

        public static CPos_XY CalsCenterOf3P_2(CPos_XY Pos1, CPos_XY Pos2, CPos_XY Pos3)
        {
            double d1 = (Pos2.dX - Pos1.dX) / (Pos2.dY - Pos1.dY);
            double d2 = (Pos3.dX - Pos2.dX) / (Pos3.dY - Pos2.dY);

            if (double.IsInfinity(d1))
                d1 = 1000000000000;
            if (double.IsInfinity(d2))
                d2 = 1000000000000;
            if (d1 == 0f)
                d1 = 0.000000000001;
            if (d2 == 0f)
                d2 = 0.000000000001;

            //원의 중점을 구함
            // Cx = ((y3 - y1) + (x2 + x3) * d2 - (x1 + x2) * d1) / (2 * (d2 - d1))
            // Cy = -d1 * (Cx - (x1 + x2) / 2) + (y1 + y2) / 2

            double centerX = ((Pos3.dY - Pos1.dY) +
                              d2 * (Pos2.dX + Pos3.dX) -
                              d1 * (Pos1.dX + Pos2.dX)) / (2 * (d2 - d1));
            double centerY = (-1 * d1 * (centerX - (Pos1.dX + Pos2.dX) / 2)) +
                             ((Pos1.dY + Pos2.dY) / 2);

            return new CPos_XY(centerX, centerY);
        }
    
#endregion

        // Align 동작
        #region Align 동작

        // Edge Align 동작=======================================
        public int DoEdgeAlign()
        {
            int iResult = -1;
            int iSleepTime = 200;
            IntPtr viewHandle;
            CPos_XY WaferCenter = new CPos_XY();
            CPos_XYTZ StageCurPos = new CPos_XYTZ();

            CPos_XY[] EdgePixelPos  = new CPos_XY[(int)EEdgeAlignTeachPos.MAX];
                for (int i = 0; i < EdgePixelPos.Length; i++) EdgePixelPos[i] = new CPos_XY();
            CPos_XYTZ[] EdgePos     = new CPos_XYTZ[(int)EEdgeAlignTeachPos.MAX];
                for (int i = 0; i < EdgePos.Length; i++) EdgePos[i] = new CPos_XYTZ();
            CPos_XYTZ[] EdgeRealPos = new CPos_XYTZ[(int)EEdgeAlignTeachPos.MAX];
                for (int i = 0; i < EdgeRealPos.Length; i++) EdgeRealPos[i] = new CPos_XYTZ();
                      
            // Align 기존 값 초기화
            m_RefComp.Stage.SetAlignDataInit();

            // Cam을 Pre Cam으로 변환
            viewHandle = CMainFrame.LWDicer.m_Vision.GetLocalViewHandle(PRE__CAM);
            if (viewHandle == null) return GenerateErrorCode(ERR_CTRLSTAGE_CAM_HANDEL_FAIL);

            CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, viewHandle);

            // Edge 1번으로 이동 & Edge 확인 =============================================================
            iResult = MoveToEdgeAlignTeachPos1();
            if (iResult != SUCCESS) return iResult;

            Sleep(iSleepTime);

            // Edge를 확인한다
            iResult = FindEdgePoint(out EdgePixelPos[(int)EEdgeAlignTeachPos.POS1]);
            if (iResult != SUCCESS) return iResult;

            // 현재 위치 값을 읽어온다.
            iResult = m_RefComp.Stage.GetStageCurPos(out StageCurPos);
            if (iResult != SUCCESS) return iResult;

            // Edge를 Stage를 중심으로한 좌표값을 구한다.
            EdgePos[(int)EEdgeAlignTeachPos.POS1] = ChagneStageToMarkPos(StageCurPos, EdgePixelPos[(int)EEdgeAlignTeachPos.POS1]);
            
            EdgeRealPos[(int)EEdgeAlignTeachPos.POS1] = EdgePos[(int)EEdgeAlignTeachPos.POS1];
            // Edge 2번으로 이동 & Edge 확인 =============================================================
            iResult = MoveToEdgeAlignTeachPos2();
            if (iResult != SUCCESS) return iResult;

            Sleep(iSleepTime);

            // Edge를 확인한다
            iResult = FindEdgePoint(out EdgePixelPos[(int)EEdgeAlignTeachPos.POS2]);
            if (iResult != SUCCESS) return iResult;

            // 현재 위치 값을 읽어온다.
            iResult = m_RefComp.Stage.GetStageCurPos(out StageCurPos);
            if (iResult != SUCCESS) return iResult;

            // Edge를 Stage를 중심으로한 좌표값을 구한다.
            EdgePos[(int)EEdgeAlignTeachPos.POS2] = ChagneStageToMarkPos(StageCurPos, EdgePixelPos[(int)EEdgeAlignTeachPos.POS2]);
            // -90도 회전 변환을 한다.
            EdgeRealPos[(int)EEdgeAlignTeachPos.POS2].dX = Math.Cos(DegToRad(90)) * EdgePos[(int)EEdgeAlignTeachPos.POS2].dX -
                                                            Math.Sin(DegToRad(90)) * EdgePos[(int)EEdgeAlignTeachPos.POS2].dY;
            EdgeRealPos[(int)EEdgeAlignTeachPos.POS2].dY = Math.Sin(DegToRad(90)) * EdgePos[(int)EEdgeAlignTeachPos.POS2].dX +
                                                            Math.Cos(DegToRad(90)) * EdgePos[(int)EEdgeAlignTeachPos.POS2].dY;

            // Edge 3번으로 이동 & Edge 확인 =============================================================
            iResult = MoveToEdgeAlignTeachPos3();
            if (iResult != SUCCESS) return iResult;

            Sleep(iSleepTime);

            // Edge를 확인한다
            iResult = FindEdgePoint(out EdgePixelPos[(int)EEdgeAlignTeachPos.POS3]);
            if (iResult != SUCCESS) return iResult;

            // 현재 위치 값을 읽어온다.
            iResult = m_RefComp.Stage.GetStageCurPos(out StageCurPos);
            if (iResult != SUCCESS) return iResult;

            // Edge를 Stage를 중심으로한 좌표값을 구한다.
            EdgePos[(int)EEdgeAlignTeachPos.POS3] = ChagneStageToMarkPos(StageCurPos, EdgePixelPos[(int)EEdgeAlignTeachPos.POS3]);
            // -180도 회전 변환을 한다.
            EdgeRealPos[(int)EEdgeAlignTeachPos.POS3].dX = Math.Cos(DegToRad(180)) * EdgePos[(int)EEdgeAlignTeachPos.POS3].dX -
                                                            Math.Sin(DegToRad(180)) * EdgePos[(int)EEdgeAlignTeachPos.POS3].dY;
            EdgeRealPos[(int)EEdgeAlignTeachPos.POS3].dY = Math.Sin(DegToRad(180)) * EdgePos[(int)EEdgeAlignTeachPos.POS3].dX +
                                                            Math.Cos(DegToRad(180)) * EdgePos[(int)EEdgeAlignTeachPos.POS3].dY;

            // Edge Align 연산
            WaferCenter =  CalsCenterOf3P(EdgeRealPos[0], EdgeRealPos[1], EdgeRealPos[2]);            

            // Stage 현재 위치 확인
            iResult = m_RefComp.Stage.GetStageCurPos(out StageCurPos);
            if (iResult != SUCCESS) return iResult;

            // Wafer의 중심과 Stage의 위치 차이를 Offset으로 저장한다.
            CMainFrame.DataManager.SystemData_Align.WaferOffsetX = WaferCenter.dX;
            CMainFrame.DataManager.SystemData_Align.WaferOffsetY = WaferCenter.dY;

            double dLenX = EdgeRealPos[(int)EEdgeAlignTeachPos.POS1].dX - WaferCenter.dX;
            double dLenY = EdgeRealPos[(int)EEdgeAlignTeachPos.POS1].dY - WaferCenter.dY;

            CMainFrame.DataManager.SystemData_Align.WaferSizeOffset = (150 - Math.Sqrt(dLenX * dLenX + dLenY * dLenY));
            // Wafer의 크기 오차 보정이 잘 되지 않아.. 우선 초기화 값을 사용... 그럭저럭 잘 맞음.
            //CMainFrame.DataManager.SystemData_Align.WaferSizeOffset = 0;
            // Align 기존 값 초기화
            m_RefComp.Stage.SetAlignDataInit();

            // Align Data Set
            var alignOffset = new CPos_XYTZ();
            alignOffset.dX = -WaferCenter.dX;
            alignOffset.dY = -WaferCenter.dY;

            SetAlignData(alignOffset);

            return SUCCESS;
        }

        public int DoAlign(int iMode)
        {
            int iResult = -1;
            int iCount = 0;
            int iSleepTime = 200;

            IntPtr viewHandle;
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
            // Cam을 Pre Cam으로 변환
            if (iMode == MACRO_ALIGN_MODE)
            {
                viewHandle = CMainFrame.LWDicer.m_Vision.GetLocalViewHandle(PRE__CAM);
                if (viewHandle == null) return GenerateErrorCode(ERR_CTRLSTAGE_CAM_HANDEL_FAIL);

                CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
                CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, viewHandle);
            }
            // Cam을 Fine Cam으로 변환
            if (iMode == MICRO_ALIGN_MODE_CH1 || iMode == MICRO_ALIGN_MODE_CH2)
            {
                viewHandle = CMainFrame.LWDicer.m_Vision.GetLocalViewHandle(FINE_CAM);
                if (viewHandle == null) return GenerateErrorCode(ERR_CTRLSTAGE_CAM_HANDEL_FAIL);

                CMainFrame.LWDicer.m_Vision.DestroyLocalView(PRE__CAM);
                CMainFrame.LWDicer.m_Vision.InitialLocalView(FINE_CAM, viewHandle);
            }

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
