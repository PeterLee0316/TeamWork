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
        public Point ptStartViewCenter { get; set; } = new Point();

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

        public void MoveViewFieldCenter()
        {
            Point viewCenter = new Point(0, 0);         

            // View중심 초기화
            SetViewCenter(viewCenter);

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
            SetViewCenter(viewCenter);
        }

        public void MoveViewDrawCenter(Point pPoint,float extend)
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
            SetViewCenter(viewCorner);
            
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////
    }
}
