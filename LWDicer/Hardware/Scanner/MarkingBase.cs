﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;


using static LWDicer.Layers.DEF_Scanner;
using static LWDicer.Layers.DEF_Common;
using LWDicer.UI;

namespace LWDicer.Layers
{

    public class DEF_Scanner
    {
        /////////////////////////////////////////////////////////////////////////////////////////
        public const int CANVAS_WIDTH_MIN = 100;
        public const int CANVAS_HEIGHT_MIN = 100;

        public const float SCAN_FIELD_WIDTH = 300.0f;
        public const float SCAN_FIELD_HEIGHT = 300.0f;
        public const float SCAN_FIELD_MAX = 1500.0f;
        public const float SCAN_FIELD_MIN = 50.0f;
        public const float SCAN_FIELD_RATIO = 0.7f;
        public const float SCAN_RESOLUTION_X = 0.1f;
        public const float SCAN_RESOLUTION_Y = 0.1f;


        public const float ZOOM_FACTOR_MAX = 20.0f;
        public const float ZOOM_FACTOR_MIN = 0.2f;
        public const float CALIB_FACTOR_MAX = 50.0f;
        public const float CALIB_FACTOR_MIN = 0.01f;        

        public const int CANVAS_MARGIN = 20;
        public const int DRAW_DOT_SIZE = 2;
        public const int DRAW_DIMENSION_SIZE = 2;

        /////////////////////////////////////////////////////////////////////////////////////////
        // System Define

        public const int SHAPE_POS_DISABLE = 1;
        public const int SHAPE_LIST_DISABLE = 10;

        /////////////////////////////////////////////////////////////////////////////////////////
        public enum EObjectType { NONE = 0, DOT, LINE, ARC ,M_LINE, RECTANGLE, CIRCLE, ELLIPSE, FONT,BMP,GROUP, MAX }
        public enum EDrawPenType {GRID_BRIGHT,GRID_DARK,ACTIVE_BRIGHT,ACTIVE_DARK,OBJECT_DRAG,INACTIVE,DRAW,DIMENSION,SELECT, MAX}
        public enum EDrawBrushType { ACTIVE_BRIGHT, ACTIVE_DARK, OBJECT_DRAG, INACTIVE, DRAW, MAX }
        public enum EPenDashStye { DASH,DASHDOT,DASHDOTDOT,DOT,SOLID }
        public enum ECanvasColorStyle { DARK,BRIGHT }

        public struct stObjectInfo
        {
            public int SelectNum;
            public CPos_XY pStartPos;
            public CPos_XY pEndPos;
            public double pAngle;
        }

        /////////////////////////////////////////////////////////////////////////////////////////

        public static CMarkingManager m_ScanManager;
        public static CMarkingWindow m_ScanWindow;
        public static FormScanWindow m_FormScanner;


        #region 맴버 변수 설정

        public static bool BaseDrawFlipX { get; private set; } = false;
        public static void SetFlipX(bool pFlip)
        {
            BaseDrawFlipX = pFlip;
        }
        public static bool BaseDrawFlipY { get; private set; } = false;
        public static void SetFlipY(bool pFlip)
        {
            BaseDrawFlipY = pFlip;
        }
        /// Canvas의 Draw Zoom값 설정
        public static float BaseZoomFactor { get; private set; } = 1.0f;
        public static int SetZoomFactor(float pZoom)
        {
            if (pZoom < ZOOM_FACTOR_MIN || pZoom > ZOOM_FACTOR_MAX) return RUN_FAIL;
            BaseZoomFactor = pZoom;

            return SUCCESS;
        }

        /// Calibration 값 설정 (실제 Laser Scan Image와 맞지 않을 경우 보정함)
        public static PointF BaseCalibFactor { get; private set; } = new PointF(1, 1);
        public static void SetCalibFactor(PointF pCalib)
        {
            if (pCalib.X < CALIB_FACTOR_MIN || pCalib.X > CALIB_FACTOR_MAX) return;
            if (pCalib.Y < CALIB_FACTOR_MIN || pCalib.Y > CALIB_FACTOR_MAX) return;

            BaseCalibFactor = pCalib;
        }

        /// Canvas의 중심점을 설정함 
        public static Point BaseFieldCenter { get; private set; } = new Point(0, 0);
        public static void SetBaseCenter(Point ptPoint)
        {
            if (ptPoint.X < 0 || ptPoint.Y < 0) return;
            BaseFieldCenter = ptPoint;
        }

        /// Canvas의 중심점을 설정함 
        private static Point BaseViewCenter = new Point(0, 0);
        public static Point GetViewCorner()
        {
            Point tempPoint = new Point();

            tempPoint.X = BaseViewCenter.X;
            tempPoint.Y = BaseViewCenter.Y;
            
            return tempPoint;
        }
        public static void SetViewCenter(Point ptPoint)
        {
            //if (ptPoint.X < 0 || ptPoint.Y < 0) return;
            // 기준값을 보정함
            Point tempPoint = new Point();
            tempPoint.X = (ptPoint.X);
            tempPoint.Y = (ptPoint.Y);


            BaseViewCenter = ptPoint;
        }

        // Draw Canvas Size 설정.. (Zoom과 연동이 필요함)
        public static Size BaseCanvasSize { get; private set; } = new Size(CANVAS_WIDTH_MIN, CANVAS_HEIGHT_MIN);

        public static void SetCanvasSize(Size pSize)
        {            
            // 초기 값보단 작게 설정은 되지 않음.
            if (pSize.Width < CANVAS_WIDTH_MIN || pSize.Height < CANVAS_HEIGHT_MIN) return;

            BaseCanvasSize = pSize;
        }

        public static SizeF BaseScanFieldSize { get; private set; } = new SizeF(SCAN_FIELD_WIDTH, SCAN_FIELD_HEIGHT);

        public static void SetScanFieldSize(SizeF pSize)
        {
            // 초기 값보단 작게 설정은 되지 않음.
            if (pSize.Width < SCAN_FIELD_MIN || pSize.Width > SCAN_FIELD_MAX) return;
            if (pSize.Height < SCAN_FIELD_MIN || pSize.Height > SCAN_FIELD_MAX) return;

            BaseScanFieldSize = pSize;
        }

        public static SizeF BaseScanResolution { get; private set; } = new SizeF(SCAN_RESOLUTION_X, SCAN_RESOLUTION_Y);
        public static void SetScanResolution(SizeF pResolution)
        {
            // 초기 값보단 작게 설정은 되지 않음.
            if (pResolution.Width <= 0) return;
            if (pResolution.Height <= 0) return;

            BaseScanResolution = pResolution;
        }

        public static Pen[] BaseDrawPen { get; private set; } = new Pen[(int)EDrawPenType.MAX];
        public static void SetBaseDrawPen(int nIndex, Color pColor, EPenDashStye DashStyle = EPenDashStye.SOLID)
        {
            if (BaseDrawPen[nIndex] == null) BaseDrawPen[nIndex] = new Pen(pColor);

            BaseDrawPen[nIndex].Color = pColor;

            //if(DashStyle==true)
            switch (DashStyle)
            {
                case EPenDashStye.DASH:
                    BaseDrawPen[nIndex].DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    break;
                case EPenDashStye.DASHDOT:
                    BaseDrawPen[nIndex].DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    break;
                case EPenDashStye.DASHDOTDOT:
                    BaseDrawPen[nIndex].DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                    break;
                case EPenDashStye.DOT:
                    BaseDrawPen[nIndex].DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    break;
                case EPenDashStye.SOLID:
                    BaseDrawPen[nIndex].DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
            }
        }
        public static void SetDrawPen(EDrawPenType pUsePen)
        {
            BaseDrawPen[(int)EDrawPenType.DRAW] = BaseDrawPen[(int)pUsePen];
        }
        public static SolidBrush[] BaseDrawBrush { get; private set; } = new SolidBrush[(int)EDrawBrushType.MAX];
        public static void SetBaseDrawBrush(int nIndex, Color pColor)
        {
            if (BaseDrawBrush[nIndex] == null) BaseDrawBrush[nIndex] = new SolidBrush(pColor);

            BaseDrawBrush[nIndex].Color = pColor;            
        }
        public static void SetDrawBrush(Color pColor)
        {
            SetBaseDrawBrush((int)EDrawBrushType.DRAW ,pColor);
        }
        public static ECanvasColorStyle BaseCanavsColorMode { get; private set; } = ECanvasColorStyle.DARK;
        public static void SetCanvasColorMode(ECanvasColorStyle pStyle)
        {
            BaseCanavsColorMode = pStyle;
        }
        
        #endregion        
   
        /////////////////////////////////////////////////////////////////////////////////////////

        #region 함수
        public static CPos_XY PixelToField(Point pPixel)
        {
            CPos_XY tempPoint = new CPos_XY();

            // 기준값을 보정함
            tempPoint.dX += (double)BaseFieldCenter.X * BaseZoomFactor;
            tempPoint.dY -= (double)BaseFieldCenter.Y * BaseZoomFactor;

            // Zoom 값 & Calib 값을 나눔 (Flip 확인)
            tempPoint.dX = (double)(pPixel.X - BaseViewCenter.X) / BaseZoomFactor / BaseCalibFactor.X * (BaseDrawFlipX ? -1.0f : 1.0f);
            tempPoint.dY = (double)(pPixel.Y - BaseViewCenter.Y) / BaseZoomFactor / BaseCalibFactor.Y * (BaseDrawFlipX ? -1.0f : 1.0f);

            //// View Center 적용
            //tempPoint.dX -= (float)BaseViewCenter.X; ;
            //tempPoint.dY -= (float)BaseViewCenter.Y; ;

            return tempPoint;
        }

        public static Point AbsFieldToPixel(CPos_XY pPos)
        {
            Point tempPoint = new Point(0, 0);

            double PosX, PosY = 0;            

            // Zoom 값 & Calib 값을 나눔 (Flip 확인)
            PosX = (pPos.dX ) * BaseZoomFactor * BaseCalibFactor.X * (BaseDrawFlipX ? -1.0f : 1.0f);
            PosY = (pPos.dY ) * BaseZoomFactor * BaseCalibFactor.Y * (BaseDrawFlipX ? -1.0f : 1.0f);

            //// 기준값을 보정함
            //tempPoint.X += (int)((float)BaseViewCenter.X);/// BaseZoomFactor / BaseCalibFactor.X);
            //tempPoint.Y += (int)((float)BaseViewCenter.Y);// / BaseZoomFactor / BaseCalibFactor.Y);

            PosX += BaseViewCenter.X;
            PosY += BaseViewCenter.Y;

            tempPoint.X += (int)(PosX+0.5);
            tempPoint.Y += (int)(PosY+0.5);

            return tempPoint;
        }

        public static double Rad2Deg(double pRad)
        {
            double mDeg = pRad * 180 / Math.PI;
            return mDeg;
        }

        public static double Deg2Rad(double pDeg)
        {
            double mRad = pDeg * Math.PI / 180;
            return mRad;
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////
    }
}
