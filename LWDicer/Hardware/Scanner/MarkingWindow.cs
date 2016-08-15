using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Scanner;
using LWDicer.UI;

namespace LWDicer.Control
{
    public class CMarkingWindow:MObject
    {
        /////////////////////////////////////////////////////////////////////////////////////////

        #region 맴버 변수 설정

        public EObjectType SelectObjectType { get; private set; } = EObjectType.NONE;
        public void SetObjectType(EObjectType pType)
        {
            if (pType < EObjectType.NONE || pType > EObjectType.MAX) return;

            SelectObjectType = pType;
        }

        public PointF ptObjectStartPos { get; private set; } = new PointF(0, 0);
        public void SetObjectStartPos(PointF pPos)
        {
            if (pPos.X < 0 || pPos.Y < 0) return;
            ptObjectStartPos = pPos;
        }

        public PointF ptObjectEndPos { get; private set; } = new PointF(0, 0);
        public void SetObjectEndPos(PointF pPos)
        {
            if (pPos.X < 0 || pPos.Y < 0) return;
            ptObjectEndPos = pPos;
        }

        public PointF ptMousePos{ get;  set;}= new PointF(0,0);

        //public FormScanWindow m_FormScanWindow;

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////

        #region 함수

        public CMarkingWindow(CObjectInfo objInfo) : base(objInfo)
        {

        }
        public void AddObject(bool CheckWindow=true)
        {
            if (CMainFrame.LWDicer.m_PolyGonScanner.m_RefComp.Manager == null) return;

            CMainFrame.LWDicer.m_PolyGonScanner.m_RefComp.Manager.AddObject(SelectObjectType, ptObjectStartPos, ptObjectEndPos);

            if (CheckWindow)
                CMainFrame.LWDicer.m_PolyGonScanner.m_RefComp.FormScanner.AddObjectList(CMainFrame.LWDicer.m_PolyGonScanner.m_RefComp.Manager.GetLastObject());
        }
        
        public void ChangeCanvasZoom(float ZoomRatio)
        {
            SetZoomFactor(ZoomRatio);

            float currentZoom = BaseZoomFactor;

            currentZoom *= ZoomRatio;

            //// Canvas Size 재조정
            //Size CanvasSize = new Size(0, 0);
            //m_FormScanWindow.GetCanvasSize(out CanvasSize);
            //CanvasSize.Width = (int)(CanvasSize.Width * currentZoom + 0.5);
            //CanvasSize.Height = (int)(CanvasSize.Height * currentZoom + 0.5);
            //m_FormScanWindow.SetCanvasSize(CanvasSize);

            //// Canvas ReDraw
            //m_FormScanWindow.ReDrawCanvas();

        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////
    }
}
