using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;


using static LWDicer.Control.DEF_Scanner;
using static LWDicer.Control.DEF_Common;
//using LaserSystem.UI;

namespace LWDicer.Control
{

    public class DEF_Scanner
    {
        /////////////////////////////////////////////////////////////////////////////////////////
        public const int CANVAS_WIDTH_MIN = 100;
        public const int CANVAS_HEIGHT_MIN = 100;

        public const float SCAN_FIELD_WIDTH = 300.0f;
        public const float SCAN_FIELD_HEIGHT = 300.0f;
        public const float SCAN_FIELD_MAX = 500.0f;
        public const float SCAN_FIELD_MIN = 50.0f;
        public const float SCAN_FIELD_RATIO = 0.7f;

        public const float ZOOM_FACTOR_MAX = 10.0f;
        public const float ZOOM_FACTOR_MIN = 0.5f;
        public const float CALIB_FACTOR_MAX = 50.0f;
        public const float CALIB_FACTOR_MIN = 0.01f;        

        public const int CANVAS_MARGIN = 10;
        public const int DRAW_DOT_SIZE = 4;
        public const int DRAW_DIMENSION_SIZE = 2;

        /////////////////////////////////////////////////////////////////////////////////////////
        // System Define

        public const int SHAPE_POS_DISABLE = 1;
        public const int SHAPE_LIST_DISABLE = 10;

        /////////////////////////////////////////////////////////////////////////////////////////
        public enum EObjectType { NONE = 0, DOT, LINE, M_LINE, RECTANGLE, ELLIPSE,FONT,BMP,DXF,GROUP, MAX }

        public enum EDrawPenType {GRID_BRIGHT,GRID_DARK,ACTIVE_BRIGHT,ACTIVE_DARK,OBJECT_DRAG,INACTIVE,DRAW,DIMENSION,SELECT, MAX}
        public enum EDrawBrushType { ACTIVE_BRIGHT, ACTIVE_DARK, OBJECT_DRAG, INACTIVE, DRAW, MAX }
        public enum EPenDashStye { DASH,DASHDOT,DASHDOTDOT,DOT,SOLID }
        public enum ECanvasColorStyle { DARK,BRIGHT }

        public struct stObjectInfo
        {
            public int SelectNum;
            public PointF pStartPos;
            public PointF pEndPos;
            public float pAngle;
        }
    
        /////////////////////////////////////////////////////////////////////////////////////////

        //public static CMarkingManager m_ScanManager;        
        //public static CMarkingWindow m_ScanWindow;
        //public static MSocketClient m_ControlComm;
        //public static MSocketClient m_ScanHeadComm;


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
        public static void SetZoomFactor(float pZoom)
        {
            if (pZoom < ZOOM_FACTOR_MIN || pZoom > ZOOM_FACTOR_MAX) return;
            BaseZoomFactor = pZoom;
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
        public static PointF PixelToField(Point pPixel)
        {
            PointF tempPoint = new PointF(0, 0);

            // 기준값을 보정함
            tempPoint.X += (float)BaseFieldCenter.X * BaseZoomFactor;
            tempPoint.Y -= (float)BaseFieldCenter.Y * BaseZoomFactor;

            // Zoom 값 & Calib 값을 나눔 (Flip 확인)
            tempPoint.X = (float)pPixel.X / BaseZoomFactor / BaseCalibFactor.X * (BaseDrawFlipX ? -1.0f : 1.0f);
            tempPoint.Y = (float)pPixel.Y / BaseZoomFactor / BaseCalibFactor.Y * (BaseDrawFlipX ? -1.0f : 1.0f);
            
            return tempPoint;
        }

        public static Point AbsFieldToPixel(PointF pPos)
        {
            Point tempPoint = new Point(0, 0);

            float PosX, PosY = 0;

            // Zoom 값 & Calib 값을 나눔 (Flip 확인)
            PosX = pPos.X * BaseZoomFactor * BaseCalibFactor.X * (BaseDrawFlipX ? -1.0f : 1.0f);
            PosY = pPos.Y * BaseZoomFactor * BaseCalibFactor.Y * (BaseDrawFlipX ? -1.0f : 1.0f);
            
            tempPoint.X = (int)(PosX+0.5);
            tempPoint.Y = (int)(PosY+0.5);

            return tempPoint;
        }


      //  /*------------------------------------------------------------------------------------
      //* Date : 2016.02.24
      //* Author : HSLEE
      //* Function : GetValue(String Section, String Key, String iniPath)
      //* Description : Text File Data Load 처리
      //------------------------------------------------------------------------------------*/
      //  public static String GetValue(String Section, String Key, String iniPath)
      //  {
      //      StringBuilder temp = new StringBuilder(255);
      //      int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
      //      return temp.ToString();
      //  }

      //  /*------------------------------------------------------------------------------------
      //   * Date : 2016.02.24
      //   * Author : HSLEE
      //   * Function : SetValue(String Section, String Key, String Value, String iniPath)
      //   * Description : Text File Data Save 처리
      //   ------------------------------------------------------------------------------------*/
      //  public static bool SetValue(String Section, String Key, String Value, String iniPath)
      //  {
      //      bool bRet = WritePrivateProfileString(Section, Key, Value, iniPath);
      //      return WritePrivateProfileString(Section, Key, Value, iniPath);
      //  }


      //  [DllImport("kernel32.dll")]
      //  public static extern int GetPrivateProfileString(
      //                              String section,
      //                              String key,
      //                              String def,
      //                              StringBuilder retVal,
      //                              int size,
      //                              String filePath);

      //  [DllImport("kernel32.dll")]
      //  public static extern bool WritePrivateProfileString(
      //                              String section,
      //                              String key,
      //                              String val,
      //                              String filePath);

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////
    }
}
