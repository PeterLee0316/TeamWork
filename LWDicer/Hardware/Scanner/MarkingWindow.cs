using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Scanner;
using LWDicer.UI;

namespace LWDicer.Layers
{
    public class CMarkingWindow
    {
        /////////////////////////////////////////////////////////////////////////////////////////

        #region 맴버 변수 설정

        public EObjectType SelectObjectType { get; private set; } = EObjectType.NONE;
        public void SetObjectType(EObjectType pType)
        {
            if (pType < EObjectType.NONE || pType > EObjectType.MAX) return;

            SelectObjectType = pType;
        }

        public CPos_XY ptObjectStartPos { get; private set; } = new CPos_XY(0, 0);
        public void SetObjectStartPos(CPos_XY pPos)
        {
            //if (pPos.dX < 0 || pPos.dY < 0) return;
            ptObjectStartPos = pPos;
        }

        public CPos_XY ptObjectEndPos { get; private set; } = new CPos_XY();
        public void SetObjectEndPos(CPos_XY pPos)
        {
            //if (pPos.dX < 0 || pPos.dY < 0) return;
            ptObjectEndPos = pPos;
        }

        public CPos_XY ptMousePos { get;  set;}= new CPos_XY();

        public Point ptPanStartPos { get; set; } = new Point();
        public Point ptCurrentViewCorner { get; set; } = new Point();

        public bool MouseDragZoom;

        //public FormScanWindow m_FormScanWindow;

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////

        #region 함수

        public CMarkingWindow()
        {

        }
        public void AddObject(bool CheckWindow=true)
        {
            if (m_ScanManager == null) return;

            m_ScanManager.AddObject(SelectObjectType, ptObjectStartPos, ptObjectEndPos);

            if (CheckWindow)
                m_FormScanner.AddObjectList(m_ScanManager.GetLastObject());
        }
        
        public int ChangeCanvasZoom(float ZoomRatio)
        {
            if (SetZoomFactor(ZoomRatio) != SUCCESS) return RUN_FAIL;
                        
            // Canvas ReDraw
            m_FormScanner.ReDrawCanvas();

            return SUCCESS;
        }

        public int FieldZoomAll()
        {
            float currentZoom = 1.0f;

            Point scanField = new Point();
            // Scan Field의 크기를 읽고 픽셀 크기로 변환
            scanField.X = (int)BaseScanFieldSize.Width;
            scanField.Y = (int)BaseScanFieldSize.Height;

            // Canvas Size 읽어옴
            Size CanvasSize = new Size(0, 0);
            m_FormScanner.GetCanvasSize(out CanvasSize);

            // 가로 세로 비율을 각각 구함
            SizeF zoomRatio = new SizeF();
            zoomRatio.Width = (float)CanvasSize.Width / (float)scanField.X;
            zoomRatio.Height = (float)CanvasSize.Height / (float)scanField.Y;

            // 가로 세로 배율중에 작은 값을 적용함
            currentZoom = zoomRatio.Width < zoomRatio.Height ? zoomRatio.Width : zoomRatio.Height;

            // Field는 약간 작게 적용함.
            currentZoom /= 1.1f;

            m_ScanWindow.ChangeCanvasZoom(currentZoom);
            m_ScanWindow.MoveViewFieldCenter();

            return SUCCESS;
        }

        public int FieldZoomPlus()
        {
            //m_ScanWindow.
            float changeZoom = 1.1f;
            float currentZoom = BaseZoomFactor;
            currentZoom *= changeZoom;

            // Canvas Size 읽어옴
            Size canvasSize = new Size(0, 0);
            m_FormScanner.GetCanvasSize(out canvasSize);
            // Canvas의 중심 계산
            Point posCanvasCenter = new Point();
            posCanvasCenter.X = canvasSize.Width / 2;
            posCanvasCenter.Y = canvasSize.Height / 2;

            if (ChangeCanvasZoom(currentZoom) != SUCCESS) return RUN_FAIL;

            MoveFieldPointView(posCanvasCenter, changeZoom);

            return SUCCESS;
        }
        public int FieldZoomMinus()
        {
            float changeZoom = 1.1f;
            float currentZoom = BaseZoomFactor;
            currentZoom /= changeZoom;

            // Canvas Size 읽어옴
            Size canvasSize = new Size(0, 0);
            m_FormScanner.GetCanvasSize(out canvasSize);

            // Canvas의 중심 계산
            Point posCanvasCenter = new Point();
            posCanvasCenter.X = canvasSize.Width / 2;
            posCanvasCenter.Y = canvasSize.Height / 2;


            if (m_ScanWindow.ChangeCanvasZoom(currentZoom) != SUCCESS) return RUN_FAIL;
            m_ScanWindow.MoveFieldPointView(posCanvasCenter, 1 / changeZoom);

            return SUCCESS;
        }

        public int SelectFieldZoom(Rectangle zoomSize)
        {            
            Point scanField = new Point();
            // Scan Field의 크기를 읽고 픽셀 크기로 변환
            scanField.X = (int)zoomSize.Width;
            scanField.Y = (int)zoomSize.Height;

            // Canvas Size 읽어옴
            Size canvasSize = new Size(0, 0);
            m_FormScanner.GetCanvasSize(out canvasSize);

            // Zoom 중심 위치
            Point zoomPoint = new Point();
            zoomPoint.X = zoomSize.X + zoomSize.Width / 2;
            zoomPoint.Y = zoomSize.Y + zoomSize.Height / 2;

            // 중앙으로 이동함.
            MoveViewObjectCenter(zoomPoint);            

            // 가로 세로 비율을 각각 구함
            SizeF zoomRatio = new SizeF();
            zoomRatio.Width = (float)canvasSize.Width / (float)scanField.X;
            zoomRatio.Height = (float)canvasSize.Height / (float)scanField.Y;
            
            float changeZoom = 1.0f;
            float currentZoom = BaseZoomFactor;           

            // 가로 세로 배율중에 작은 값을 적용함
            changeZoom = zoomRatio.Width < zoomRatio.Height ? zoomRatio.Width : zoomRatio.Height;
            currentZoom *= changeZoom;

            if (ChangeCanvasZoom(currentZoom) != SUCCESS) return RUN_FAIL;

            Point canvasCenter = new Point();
            canvasCenter.X = canvasSize.Width / 2;
            canvasCenter.Y = canvasSize.Height / 2;

            MoveFieldPointView(canvasCenter, changeZoom);

            return SUCCESS;
        }

        public void MoveViewFieldCenter()
        {
            Point viewCenter = new Point(0, 0);         

            // View중심 초기화
            SetViewCorner(viewCenter);

            CPos_XY scanField = new CPos_XY();
            Point scanPixel = new Point();
            // Scan Field의 크기를 읽고 픽셀 크로로 변환
            scanField.dX = (double)BaseScanFieldSize.Width;
            scanField.dY = (double)BaseScanFieldSize.Height;
            scanPixel = AbsFieldToPixel(scanField);

            // Canvas Size 읽어옴
            Size CanvasSize = new Size(0, 0);
            m_FormScanner.GetCanvasSize(out CanvasSize);

            // Canvas와 ScanField의 중심 오차를 확인홤
            viewCenter.X = (CanvasSize.Width - scanPixel.X) / 2;
            viewCenter.Y = (CanvasSize.Height - scanPixel.Y) / 2;
            // View중심 재설정
            SetViewCorner(viewCenter);
        }
        public void MoveViewObjectCenter(Point pPoint)
        {
            Point viewCenter = new Point(0, 0);

            // View Corner 위치 읽기
            viewCenter = GetViewCorner();

            // Canvas Size 읽어옴
            Size CanvasSize = new Size(0, 0);
            m_FormScanner.GetCanvasSize(out CanvasSize);

            // Canvas와 ScanField의 중심 오차를 확인홤
            viewCenter.X += (CanvasSize.Width / 2 - pPoint.X);
            viewCenter.Y += (CanvasSize.Height/ 2 - pPoint.Y);
            // View중심 재설정
            SetViewCorner(viewCenter);
        }

        public void MoveFieldPointView(Point pPoint,float extend)
        {
            Point viewCorner = new Point();
            Point lengthCorner = new Point();
            PointF changeCorner = new PointF();

            // View Corner 위치 확인
            viewCorner = GetViewCorner();
            // 중심 위치에서 거리 값
            lengthCorner.X = pPoint.X - viewCorner.X;
            lengthCorner.Y = pPoint.Y - viewCorner.Y;
            // 거리 변화 값
            changeCorner.X = (float)lengthCorner.X * (extend - 1.0f);// + 0.5f;
            changeCorner.Y = (float)lengthCorner.Y * (extend - 1.0f);// + 0.5f;

            // 변화값을 적용한다.
            viewCorner.X -= (int)changeCorner.X;
            viewCorner.Y -= (int)changeCorner.Y;

            // View중심 재설정
            SetViewCorner(viewCorner);
            
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////
    }
}
