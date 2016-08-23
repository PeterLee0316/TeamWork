using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using static LWDicer.Control.DEF_Scanner;
using static LWDicer.Control.DEF_Common;


namespace LWDicer.Control
{
    [Serializable]
    public class CMarkingManager
    {
        /////////////////////////////////////////////////////////////////////////////////////////

        #region 맴버 변수 설정
        public List<CMarkingObject> ObjectList = new List<CMarkingObject>();


        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////

        #region 함수

        public CMarkingManager()
        {
            InitializePenBrush();
        }

        public int Initialize()
        {
            InitializePenBrush();
            return SUCCESS;
        }

        private int InitializePenBrush()
        {
            //---------------------------------------------------------------------------
            //  Pen Setting           
            SetBaseDrawPen((int)EDrawPenType.GRID_BRIGHT,   Color.DimGray, EPenDashStye.DOT);
            SetBaseDrawPen((int)EDrawPenType.GRID_BRIGHT,   Color.DimGray, EPenDashStye.DOT);
            SetBaseDrawPen((int)EDrawPenType.ACTIVE_BRIGHT, Color.White);
            SetBaseDrawPen((int)EDrawPenType.ACTIVE_DARK,   Color.Black);
            SetBaseDrawPen((int)EDrawPenType.INACTIVE,      Color.DarkSlateGray);
            SetBaseDrawPen((int)EDrawPenType.OBJECT_DRAG,   Color.Red);
            SetBaseDrawPen((int)EDrawPenType.DIMENSION,     Color.LightGreen);
            SetBaseDrawPen((int)EDrawPenType.SELECT,        Color.SkyBlue, EPenDashStye.DOT);

            // 사용할 Pen
            SetBaseDrawPen((int)EDrawPenType.DRAW, Color.Black);

            //---------------------------------------------------------------------------
            //  Brush Setting
            SetBaseDrawBrush((int)EDrawBrushType.ACTIVE_BRIGHT, Color.White);
            SetBaseDrawBrush((int)EDrawBrushType.ACTIVE_DARK, Color.Black);
            SetBaseDrawBrush((int)EDrawBrushType.INACTIVE, Color.DarkGray);
            SetBaseDrawBrush((int)EDrawBrushType.OBJECT_DRAG, Color.Red);

            SetDrawBrush(Color.Black);

            return SUCCESS;
        }

        public int GetObject(int nIndex, out CMarkingObject pObject)
        {
            if (nIndex < 0 || nIndex > ObjectList.Count)
            {
                pObject = null;
                return SHAPE_LIST_DISABLE;
            }
            pObject = ObjectList[nIndex-1];

            return SUCCESS;
        }

        public CMarkingObject GetLastObject()
        {
            int nResult = -1;
            CMarkingObject pObject;

            if (ObjectList.Count < 1) return null;

            nResult = GetObject(ObjectList.Count, out pObject);
            if (nResult == SUCCESS)
                return pObject;
            else
                return null;
        }

        public void AddObject(EObjectType pType,PointF pStart, PointF pEnd, CMarkingObject[] pObject =null)
        {                        
            switch (pType)
            {
                case (EObjectType.DOT):
                    CObjectDot pDot = new CObjectDot(pStart);
                    ObjectList.Add(pDot);
                    break;

                case (EObjectType.LINE):
                    CObjectLine pLine = new CObjectLine(pStart, pEnd);
                    ObjectList.Add(pLine);
                    break;
                case (EObjectType.RECTANGLE):
                    CObjectRectagle pRect = new CObjectRectagle(pStart, pEnd);
                    ObjectList.Add(pRect);
                    break;
                case (EObjectType.ELLIPSE):
                    CObjectEllipse pCircle = new CObjectEllipse(pStart, pEnd);
                    ObjectList.Add(pCircle);
                    break;
                case (EObjectType.GROUP):
                    CObjectGroup pGroup = new CObjectGroup(pObject);
                    ObjectList.Add(pGroup);
                    break;
            }
        }
        public void AddObject(CMarkingObject pObject)
        {
            switch (pObject.ObjectType)
            {
                case (EObjectType.DOT):
                    CObjectDot pDot = new CObjectDot((CObjectDot)pObject);
                    ObjectList.Add(pDot);
                    break;
                case (EObjectType.LINE):
                    CObjectLine pLine = new CObjectLine((CObjectLine)pObject);
                    ObjectList.Add(pLine);
                    break;
                case (EObjectType.RECTANGLE):
                    CObjectRectagle pRect = new CObjectRectagle((CObjectRectagle)pObject);
                    ObjectList.Add(pRect);
                    break;
                case (EObjectType.ELLIPSE):
                    CObjectEllipse pCircle = new CObjectEllipse((CObjectEllipse)pObject);
                    ObjectList.Add(pCircle);
                    break;
                case (EObjectType.GROUP):
                    CObjectGroup pGroup = new CObjectGroup((CObjectGroup)pObject);
                    ObjectList.Add(pGroup);
                    break;
                default:

                    break;
            }
        }

        public void InsertObject(int nIndex, EObjectType pType, PointF pStart, PointF pEnd)
        {
            CMarkingObject pObject = new CMarkingObject();
            pObject.SetObjectType(pType);

            pObject.SetObjectStartPos(pStart);
            pObject.SetObjectStartPos(pEnd);

            ObjectList.Insert(nIndex,pObject);
        }

        public void DeleteObject(int nIndex)
        {
            // Object 상태를 확인함.

            if (ObjectList[nIndex] == null) return;
                        
            ObjectList.RemoveAt(nIndex);
        }

        public void DeleteAllObject()
        {
            ObjectList.Clear();
            CMarkingObject.SetCreateSortNum(0);
        }

        public void DrawObject(PaintEventArgs g)
        {
            foreach (CMarkingObject s in this.ObjectList.ToArray<CMarkingObject>())
            {
                s.DrawObject(g.Graphics);
            }
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////

    }
}
