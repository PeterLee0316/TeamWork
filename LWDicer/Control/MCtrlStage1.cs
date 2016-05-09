using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_MeStage;
using static LWDicer.Control.DEF_CtrlStage;

namespace LWDicer.Control
{
    public class DEF_CtrlStage
    {

        // VISION 관련 오류
        public const int ERR_CTRLSTAGE_EDGE_POINT_NONE = 1;
        public const int ERR_CTRLSTAGE_EDGE_POINT_OVER = 2;

        public const int ERR_CTRLSTAGE_THETA_POS_UNSUITABLE = 10;

        // STAGE 관련 오류

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

        public enum ELightType
        {
            DIRECT = 0,
            RING,
            NUM,
        }
        public class CCtrlStageData
        {
            public double ThetaAdjSpeedAxisX;

        }

        public class CCtrlVisionData
        {
            public int[] PixelSizeX         = new int[DEF_MAX_CAMERA_NO];
            public int[] PixelSizeY         = new int[DEF_MAX_CAMERA_NO];
            public int[] LenMagnification   = new int[DEF_MAX_CAMERA_NO];
            
            // 렌즈 Resolution & 카메라 Position
            public double[] PixelResolutionX = new double[DEF_MAX_CAMERA_NO];
            public double[] PixelResolutionY = new double[DEF_MAX_CAMERA_NO];
            public CPos_XY[] CameraPosition = new CPos_XY[DEF_MAX_CAMERA_NO];
            public double[] CameraTilt = new double[DEF_MAX_CAMERA_NO];
            
        }
        public class CCtrlLightData
        {
            public int[] DefaultLightLevel = new int[(int)ELightType.NUM];
            public int[] CurrentLightLevel = new int[(int)ELightType.NUM];

        }

        public class CCtrlStage1Data
        {
            public CCtrlStageData Stage;
            public CCtrlVisionData Vision;
            public CCtrlLightData Light;
        }

    }

    public class MCtrlStage1 : MCtrlLayer
    {
        private CCtrlStage1RefComp m_RefComp;
        private CCtrlStage1Data m_Data;
        private int m_iCurrentCam = -1;
        private bool bThetaAlignInit = false;
        private bool bEdgeAlignTeachInit = false;

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

        public int MoveToWaitPos()
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

        public int MoveToThetaAlignPosA()
        {
            return m_RefComp.Stage.MoveStageToThetaAlignPosA();
        }

        public int MoveToThetaAlignPosB()
        {
            return m_RefComp.Stage.MoveStageToThetaAlignPosB();
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
        public void MoveIndexPlusT()
        {
            m_RefComp.Stage.MoveStageIndexPlusT();
        }
        public void MoveIndexMinusX()
        {
            m_RefComp.Stage.MoveStageIndexMinusX();
        }
        public void MoveIndexMinusY()
        {
            m_RefComp.Stage.MoveStageIndexMinusY();
        }
        
        public void MoveIndexMinusT()
        {
            m_RefComp.Stage.MoveStageIndexMinusT();
        }
        public void MoveScreenPlusX()
        {
            m_RefComp.Stage.MoveStageScreenPlusX();
        }

        public void MoveScreenPlusY()
        {
            m_RefComp.Stage.MoveStageScreenPlusY();
        }

        public void MoveScreenPlusT()
        {
            m_RefComp.Stage.MoveStageScreenPlusT();
        }
        public void MoveScreenMinusX()
        {
            m_RefComp.Stage.MoveStageScreenMinusX();
        }
        public void MoveScreenMinusY()
        {
            m_RefComp.Stage.MoveStageScreenMinusY();
        }
        public void MoveScreenMinusT()
        {
            m_RefComp.Stage.MoveStageScreenMinusT();
        }
        public void JogMovePlusX()
        {
            m_RefComp.Stage.JogStagePlusX();
        }
        public void JogMoveMinusX()
        {
            m_RefComp.Stage.JogStageMinusX();
        }

        public void JogMovePlusY()
        {
            m_RefComp.Stage.JogStagePlusY();
        }
        public void JogMoveMinusY()
        {
            m_RefComp.Stage.JogStageMinusY();
        }

        public void JogMovePlusT()
        {
            m_RefComp.Stage.JogStagePlusT();
        }
        public void JogMoveMinusT()
        {
            m_RefComp.Stage.JogStageMinusT();
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
            sPos = new CPos_XY();
            
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
            sPos = pEdgeData.EdgePos[0];

            // Camera의 틀어짐 보정
            CPos_XY mCenter = new CPos_XY(); // 회전 중심은 (0,0)으로 한다
            sPos = CoordinateRotate(m_Data.Vision.CameraTilt[iCam], sPos, mCenter);
            
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
            sPos = CoordinateRotate(m_Data.Vision.CameraTilt[iCam], sPos, mCenter);
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
            sPos = CoordinateRotate(m_Data.Vision.CameraTilt[iCam], sPos, mCenter);
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
            sPos = CoordinateRotate(m_Data.Vision.CameraTilt[iCam], sPos, mCenter);
            // Pixel값을 실제 위치값으로 변환한다.
            sPos = PixelToPostion(iCam, pResult.m_PixelPos);

            return SUCCESS;
        }
        
        // Hair Line
        public void ShowHairLine()
        {
            m_RefComp.Vision.DrawOverLayHairLine();
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
            sPos.dX = sPos.dX * m_Data.Vision.PixelResolutionX[iCam];
            sPos.dY = sPos.dY * m_Data.Vision.PixelResolutionX[iCam];

            return sPos;
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

            mPos.dX = Math.Cos(2);
            return sPos;
        }

        /// <summary>
        /// 임의의 2점의 선을 수평 기준으로 각도를 구한다.
        /// </summary>
        /// <param name="pMinusPos"></param>
        /// <param name="pPlusPos"></param>
        /// <returns></returns>
        private double CalsRotateAngle(CPos_XY pMinusPos, CPos_XY pPlusPos)
        {
            double RotateAngle = 0.0;
            if (pPlusPos == pMinusPos) return RotateAngle;
            
            // 각도 계산 
            double dWidth  = pPlusPos.dX - pMinusPos.dX;
            double dHeight = pPlusPos.dY - pMinusPos.dY;
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
            m_Data.Vision.CameraPosition[iCam] = pCamPos;
        }
        public CPos_XY GetCameraPos(int iCam)
        {
            return m_Data.Vision.CameraPosition[iCam];
        }

        /// <summary>
        /// Index 데이터 설정
        /// </summary>
        /// <param name="dIndexLen"></param>
        public void WidthIndexDataSet(double dIndexLen)
        {
            m_RefComp.Stage.SetWidthIndexData(dIndexLen);
        }
        public void WidthIndexDataClear()
        {
            m_RefComp.Stage.ClearWidthIndexData();
        }
        public void HeightIndexDataSet(double dIndexLen)
        {
            m_RefComp.Stage.SetWidthIndexData(dIndexLen);
        }
        public void HeightIndexDataClear()
        {
            m_RefComp.Stage.ClearWidthIndexData();
        }


        #endregion
        // Calibration  및 매뉴얼 동작 
        #region Calibration

        // 카메라 배율 변경
        public int ChangeVisionMagnitude(int iCam)
        {
            int iResult = -1;
            if (m_iCurrentCam == iCam) return SUCCESS;

            if (iCam == FINE_CAM)
            {
                iResult = m_RefComp.Stage.MoveChangeMicroCam();
                if (iResult != SUCCESS) return iResult;
                iResult = m_RefComp.Vision.DestroyLocalView(PRE__CAM);
                if (iResult != SUCCESS) return iResult;
            }
            if (iCam == PRE__CAM)
            {
                m_RefComp.Stage.MoveChangeMacroCam();
                if (iResult != SUCCESS) return iResult;
                m_RefComp.Vision.DestroyLocalView(FINE_CAM);
                if (iResult != SUCCESS) return iResult;
            }

            m_RefComp.Vision.ChangeLocalView(iCam);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        public int ChangeMacroVision()
        {
            return ChangeVisionMagnitude(PRE__CAM);
        }

        public int ChangeMicroVision()
        {
            return ChangeVisionMagnitude(FINE_CAM);
        }

        // 카메라 위치 계산 및 설정
        // Fine 카메라만 자동으로 연산함
        // Pre 카메라는 Fine과 차이를 수동으로 설정함.

        public int TeachMicroCamPos()
        {
            // Micro Mark A가 패턴 등록이 되어 있어야 함.
            // ??? 조건문 확인

            CPos_XYTZ CurPos = new CPos_XYTZ();
            CPos_XY StagePos = new CPos_XY();
            CPos_XY MarkPosA = new CPos_XY();
            CPos_XY MarkPosB = new CPos_XY();

            CResultData MarkData = new CResultData();
            int iResult = 0;

            iResult = m_RefComp.Stage.GetStageCurPos(out CurPos);
            if (iResult != SUCCESS) return iResult;

            StagePos.dX = CurPos.dX;
            StagePos.dY = CurPos.dY;

            /////////////////////////////////////////////////////////////////////
            //  회전 이동하며, Mark 2 Point를 확인함

            // Screen Index Theta + 이동
            iResult = m_RefComp.Stage.MoveStageScreenPlusT();
            if (iResult != SUCCESS) return iResult;

            Sleep(500);

            // Mark 확인
            iResult = FindMicroMarkA(out MarkPosA);
            if (iResult != SUCCESS) return iResult;

            // Screen Index Theta 원점 이동
            iResult = m_RefComp.Stage.MoveStageScreenPlusT();
            if (iResult != SUCCESS) return iResult;

            // Screen Index Theta - 이동
            iResult = m_RefComp.Stage.MoveStageScreenPlusT();
            if (iResult != SUCCESS) return iResult;

            Sleep(500);
            // Mark 확인
            iResult = FindMicroMarkA(out MarkPosB);
            if (iResult != SUCCESS) return iResult;

            // Screen Index Theta 원점 이동
            iResult = m_RefComp.Stage.MoveStageScreenPlusT();
            if (iResult != SUCCESS) return iResult;

            // 1차 카메라의 위치를 구함.
            double dAngle = m_RefComp.Stage.GetScreenIndexTheta()*2;
            CPos_XY RotatePos = new CPos_XY();
            RotatePos = CalsRotateCenter(dAngle, MarkPosA, MarkPosB);

            m_Data.Vision.CameraPosition[FINE_CAM] = StagePos;

            /////////////////////////////////////////////////////////////////////
            //  회전 각도를 키워서.. 정확도를 높히는 동작
            //  +/- 2도를 회전 시킨다.
            //  Stage를 회전할때.. Mark가 FOV 밖에 나가므로, Stage를 이동하여 영상에 들어오게 한다.

            CPos_XY TransPos = new CPos_XY();
            TransPos = CoordinateRotate(2.0, m_Data.Vision.CameraPosition[FINE_CAM], StagePos);

            CPos_XYTZ StageMovePos = new CPos_XYTZ();

            // Fov에 Mark가 들기 위해.. Stage 이동 거리 대입
            StageMovePos.dX = TransPos.dX;
            StageMovePos.dY = TransPos.dY;
            StageMovePos.dT = 2.0;
            StageMovePos.dZ = 0.0;
            

            // Theta + 이동
            iResult = m_RefComp.Stage.MoveStageRelativeXYT(StageMovePos);
            if (iResult != SUCCESS) return iResult;

            Sleep(500);

            // Mark 확인
            iResult = FindMicroMarkA(out MarkPosA);
            if (iResult != SUCCESS) return iResult;


            // Screen Index T - 이동
            iResult = m_RefComp.Stage.MoveStageRelativeT(-2.0);
            if (iResult != SUCCESS) return iResult;

            Sleep(500);
            // Mark 확인
            iResult = FindMicroMarkA(out MarkPosB);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        #endregion


        // Teaching 동작 
        #region Teaching

        // Theta Align Manual Set

        /// <summary>
        /// Theta Align 동작으로 PosB위치에서 PosA위치로 이동명령
        /// PosA,PosB 위치를 확인하고, Theta Align 적용 후
        /// Theta 회전 및 PosA위치를 재 설정
        /// </summary>
        /// <returns></returns>
        public int MoveThetaAlignPosA()
        {
            CPos_XYTZ mPos1 = new CPos_XYTZ();
            CPos_XYTZ mPos2 = new CPos_XYTZ();
            CPos_XYTZ AlignAdjPos = new CPos_XYTZ();

            // A위치를 읽어온다.
            m_RefComp.Stage.GetThetaAlignPosA(out mPos1);

            if (bThetaAlignInit == true)
            {
                // B위치를 읽어온다.
                m_RefComp.Stage.GetStageCurPos(out mPos2);
                // 가로 세로 거리를 측정한다.

                if ((mPos1.dY - mPos2.dY) != 0.0)
                {
                    // Theta Align은 연산
                    CalsThetaAlign(m_iCurrentCam, mPos1, mPos2, out AlignAdjPos);
                    // ThetaAlignPosA 위치 Offset 적용
                    m_RefComp.Stage.SetThetaAlignPosA(AlignAdjPos);
                }
            }

            // UI MSG Display
            // ???

            
            return m_RefComp.Stage.MoveStageToThetaAlignPosA();
        }
        /// <summary>
        /// Theta Align에서 PosA위치만 확인하고, PosB위치로 이동 명령
        /// 현재 위치가 PosA가 아닐 경우 실행하지 않음.
        /// </summary>
        /// <returns></returns>
        public int MoveThetaAlignPosB()
        {
            // 현재 위치 값을 PosA을 경우에 실행한다.
            int iPos = 0;
            m_RefComp.Stage.GetStagePosInfo(out iPos);
            if (iPos != (int)EStagePos.THETA_ALIGN) return GenerateErrorCode(ERR_CTRLSTAGE_THETA_POS_UNSUITABLE);

            CPos_XYTZ sPos = new CPos_XYTZ();
            // 현재 위치를 ThetaAlignPosA로 저장한다. 
            m_RefComp.Stage.GetThetaAlignPosA(out sPos);
            m_RefComp.Stage.SetThetaAlignPosA(sPos);

            // UI MSG Display
            // ???

            bThetaAlignInit = true;
            // ThetaAlignPosB로 이동한다.
            return m_RefComp.Stage.MoveStageToThetaAlignPosB();
        }
        public int MoveThetaAlign()
        {
            int iPos = 0;
            m_RefComp.Stage.GetStagePosInfo(out iPos);

            if (iPos == (int)EStagePos.THETA_ALIGN)
                MoveThetaAlignPosB();
            else
                MoveThetaAlignPosA();

            return SUCCESS;
        }
        private int CalsThetaAlign(int iCam, CPos_XYTZ pPosA, CPos_XYTZ pPosB, out CPos_XYTZ pAlignPos)
        {
            int iResult = -1;
            CPos_XYTZ mAlignPos = new CPos_XYTZ();
            // Align 결과 초기화
            pAlignPos = mAlignPos;

            // 회전 값을 계산한다.
            double dAngle  = 0.0;
            double dWidth  = pPosA.dX - pPosB.dX;
            double dHeight = pPosA.dY - pPosB.dY;

            dAngle = CalsRotateAngle(dWidth, dHeight);

            // A Pos 기준으로 Cam의 중심 위치를 회전변환한다.
            CPos_XY mCamPos = new CPos_XY();            
            // 선택 카메라에 따라 카메라 위치값 대입
            mCamPos = m_Data.Vision.CameraPosition[iCam];
            // 회전 중심값 대입 
            CPos_XY RotateCenter = new CPos_XY();
            RotateCenter.dX = pPosA.dX;
            RotateCenter.dY = pPosA.dY;
            // Cam중심을 A Pos 중심으로 회전
            CoordinateRotate(dAngle, mCamPos, RotateCenter);

            // Align 결과 값 대입
            // 
            mAlignPos.dX = -(mCamPos.dX - m_Data.Vision.CameraPosition[iCam].dX);
            mAlignPos.dY = -(mCamPos.dY - m_Data.Vision.CameraPosition[iCam].dY);
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
            // Edge Pos 2 위치 설정
            if (iCurPosIndex == (int)EStagePos.EDGE_ALIGN_2)
            {
                SetEdgePosOffset(iCurPosIndex, mPos);

                iResult = MoveToEdgeAlignPos3();
                if (iResult != SUCCESS) return iResult;

                return SUCCESS;
            }
            // Edge Pos 3 위치 설정
            if (iCurPosIndex == (int)EStagePos.EDGE_ALIGN_3)
            {
                SetEdgePosOffset(iCurPosIndex, mPos);

                iResult = MoveToEdgeAlignPos4();
                if (iResult != SUCCESS) return iResult;

                return SUCCESS;
            }
            // Edge Pos 4 위치 설정
            if (iCurPosIndex == (int)EStagePos.EDGE_ALIGN_4)
            {
                SetEdgePosOffset(iCurPosIndex, mPos);

                bEdgeAlignTeachInit = false;
                return SUCCESS;
            }

            return SUCCESS;
        }

        #endregion


        // Align 동작
        #region Align 동작

        // Edge Align 동작

        // Macro Align 동작

        // Micro Algin 동작





        #endregion


    }
}
