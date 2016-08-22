using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using static LWDicer.Control.DEF_Scanner;
using static LWDicer.Control.DEF_Common;


namespace LWDicer.Control
{
    [Serializable]
    public class CMarkingObject
    {
        /////////////////////////////////////////////////////////////////////////////////////////

        #region 맴버변수        

        public EObjectType ObjectType { get; private set;} = EObjectType.NONE;
        public void SetObjectType(EObjectType pType)
        {
            ObjectType = pType;
        }
        //---------------------------------------------------------------------------
        /// Shape가 선택했는지 확인
        public bool IsSelectedObject { get; set; } = false;

        //---------------------------------------------------------------------------
        /// Shape가 선택했는지 확인
        public bool IsGroupObject { get; private set; } = false;
        public void SetGroupObject(bool bSet)
        {
            IsGroupObject = bSet;
        }
        //---------------------------------------------------------------------------
        /// 생성시 순차 번호      
        public static int CreateSortNum {get; protected set; }
        public static void SetCreateSortNum(int pNum)
        {
            if (pNum < 0 ) return;

            CreateSortNum = pNum;
        }

        public int GroupObjectCount { get; set; } = 0;
        //---------------------------------------------------------------------------
        /// ObjectName              
        public string ObjectName { get; private set; } = "";
        protected void SetObjectName(string pName)
        {
            if (pName == null) return;

            ObjectName = pName;
        }
        //---------------------------------------------------------------------------
        /// Object의 Gruop을 포함 생성 번호 
        public int ObjectSortFlag { get; private set; } = 0;
        protected void SetObjectSortFlag(int pFlag)
        {
            if (pFlag <0) return;

            ObjectSortFlag = pFlag;
        }
        //---------------------------------------------------------------------------
        /// Object의 Start 위치
        public PointF ptObjectStartPos { get; private set; } = new PointF(0, 0);
        public int SetObjectStartPos(PointF pPos)
        {
            //if (pPos.X < 0 || pPos.X > BaseScanFieldSize.Width) return SHAPE_POS_DISABLE;
            //if (pPos.Y < 0 || pPos.Y > BaseScanFieldSize.Height) return SHAPE_POS_DISABLE;

            ptObjectStartPos = pPos;

            return SUCCESS;
        }

        //---------------------------------------------------------------------------
        /// Object의 End 위치
        public PointF ptObjectEndPos { get; private set; } = new PointF(0, 0);
        public int SetObjectEndPos(PointF pPos)
        {
            //if (pPos.X < 0 || pPos.X > BaseScanFieldSize.Width) return SHAPE_POS_DISABLE;
            //if (pPos.Y < 0 || pPos.Y > BaseScanFieldSize.Height) return SHAPE_POS_DISABLE;

            ptObjectEndPos = pPos;

            return SUCCESS;
        }
        //---------------------------------------------------------------------------
        /// Object의 회전 각도 위치
        public float ObjectRotateAngle { get; private set; } = 0;

        public void SetObjectRatateAngle(float pAngle)
        {
            if (pAngle < -360 || pAngle > 360) return;
            ObjectRotateAngle = pAngle;
        }

        private Rectangle ObjectDemension = new Rectangle(0, 0,0,0);
        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////

        #region 함수
        //public CMarkingObject(PointF pStart, PointF pEnd, float pAngle=0)
        //{
        //    ptObjectStartPos    = pStart;
        //    ptObjectEndPos      = pEnd;
        //    ObjectRotateAngle   = pAngle;
        //}

        public virtual void DrawObject(Graphics g)
        {
            // Select한 상태를 확인
            if (IsSelectedObject)
            {
                // Object의 외각 Demension을 그린다.
                // Gruop일 경우 개별적을 그리지 않는다
                //if(IsGroupObject == false)
                DrawObjectDemension(g);
            }
        }

        private void DrawObjectDemension(Graphics g)
        {
            int nMargin = DRAW_DIMENSION_SIZE;
            Rectangle pRect = new Rectangle(0, 0, 0, 0);

            Point StartPos  = new Point(0, 0);
            Point EndPos    = new Point(0, 0);            

            StartPos = AbsFieldToPixel(ptObjectStartPos);
            EndPos = AbsFieldToPixel(ptObjectEndPos);            

            if (ObjectType == EObjectType.DOT) EndPos = StartPos;

            pRect.X = StartPos.X < EndPos.X ? StartPos.X : EndPos.X;
            pRect.Y = StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y;
            pRect.Width = StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X);
            pRect.Height = StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y);

            pRect.X -= nMargin;
            pRect.Y -= nMargin;
            pRect.Width  += nMargin*2;
            pRect.Height += nMargin*2;

            if (ObjectType == EObjectType.DOT)
            {
                pRect.X -= DRAW_DOT_SIZE/2;
                pRect.Y -= DRAW_DOT_SIZE/2;
                pRect.Width += DRAW_DOT_SIZE -1;
                pRect.Height += DRAW_DOT_SIZE -1;
            }

            g.DrawRectangle(BaseDrawPen[(int)EDrawPenType.DIMENSION], pRect);
        }

        public virtual void MoveObject( PointF pPos)
        {
            //if (this.ObjectType == EObjectType.GROUP)// MoveObject(pPos);
            //    for(int i=0; i<this.GroupObjectCount;i++)
            //    {
                    
            //    }



            //PointF objectCurrentPos = new PointF(0,0);

            ////--------------------------------------------------------------------------------
            //// Start Position Move
            //objectCurrentPos.X = this.ptObjectStartPos.X;
            //objectCurrentPos.Y = this.ptObjectStartPos.Y;
            //objectCurrentPos.X += pPos.X;
            //objectCurrentPos.Y += pPos.Y;
            //this.SetObjectStartPos(objectCurrentPos);

            ////--------------------------------------------------------------------------------
            //// End Position Move
            //objectCurrentPos.X = this.ptObjectEndPos.X;
            //objectCurrentPos.Y = this.ptObjectEndPos.Y;
            //objectCurrentPos.X += pPos.X;
            //objectCurrentPos.Y += pPos.Y;
            //this.SetObjectEndPos(objectCurrentPos);

        }
        public virtual CMarkingObject PullGroupObject()
        {
            return null;
        }
        #endregion
        /////////////////////////////////////////////////////////////////////////////////////////
    }
    [Serializable]
    public class CObjectDot : CMarkingObject
    {
        public CObjectDot(PointF pStart, float pAngle = 0)
        {
            SetObjectStartPos(pStart);
            SetObjectRatateAngle(pAngle);

            SetObjectType(EObjectType.DOT);
            SetObjectName("Dot");
            
            CMarkingObject.CreateSortNum++;
            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectDot(CObjectDot pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);

            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);            
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public override void DrawObject(Graphics g)
        {
            base.DrawObject(g);
            Point DotPos = new Point(0,0);            
            
            DotPos = AbsFieldToPixel(ptObjectStartPos);

            Rectangle rectDot = new Rectangle(DotPos.X - DRAW_DOT_SIZE/2, DotPos.Y- DRAW_DOT_SIZE/2,
                                              DRAW_DOT_SIZE, DRAW_DOT_SIZE);

            g.FillRectangle(BaseDrawBrush[(int)EDrawBrushType.DRAW], rectDot);
        }

        public override void MoveObject(PointF pPos)
        {
            base.MoveObject(pPos);
            PointF objectCurrentPos = new PointF(0, 0);

            //--------------------------------------------------------------------------------
            // Start Position Move
            objectCurrentPos.X = ptObjectStartPos.X;
            objectCurrentPos.Y = ptObjectStartPos.Y;
            objectCurrentPos.X += pPos.X;
            objectCurrentPos.Y += pPos.Y;
            SetObjectStartPos(objectCurrentPos);
                        
        }
    }
    [Serializable]
    public class CObjectLine : CMarkingObject
    {
        public CObjectLine(PointF pStart, PointF pEnd, float pAngle = 0)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectRatateAngle(pAngle);
            
            SetObjectType(EObjectType.LINE);
            SetObjectName("Line");

            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectLine(CObjectLine pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectEndPos(pObject.ptObjectEndPos);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public override void DrawObject(Graphics g)
        {
            base.DrawObject(g);
            Pen drawPen=new Pen(Color.Black);

            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);

            StartPos = AbsFieldToPixel(ptObjectStartPos);
            EndPos = AbsFieldToPixel(ptObjectEndPos);            

            g.DrawLine(BaseDrawPen[(int)EDrawPenType.DRAW], StartPos, EndPos);
        }

        public override void MoveObject(PointF pPos)
        {
            base.MoveObject(pPos);
            PointF objectCurrentPos = new PointF(0, 0);

            //--------------------------------------------------------------------------------
            // Start Position Move
            objectCurrentPos.X = ptObjectStartPos.X;
            objectCurrentPos.Y = ptObjectStartPos.Y;
            objectCurrentPos.X += pPos.X;
            objectCurrentPos.Y += pPos.Y;
            SetObjectStartPos(objectCurrentPos);

            //--------------------------------------------------------------------------------
            // End Position Move
            objectCurrentPos.X = ptObjectEndPos.X;
            objectCurrentPos.Y = ptObjectEndPos.Y;
            objectCurrentPos.X += pPos.X;
            objectCurrentPos.Y += pPos.Y;
            SetObjectEndPos(objectCurrentPos);
        }

    }
    [Serializable]
    public class CObjectArc : CMarkingObject
    {
        public CObjectArc()
        {
            SetObjectName("Arc");

            CreateSortNum++;
        }
    }
    [Serializable]
    public class CObjectRectagle : CMarkingObject
    {
        public CObjectRectagle(PointF pStart, PointF pEnd, float pAngle = 0)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectRatateAngle(pAngle);

            SetObjectType(EObjectType.RECTANGLE);
            SetObjectName("Rectagle");

            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectRectagle(CObjectRectagle pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectEndPos(pObject.ptObjectEndPos);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public override void DrawObject(Graphics g)
        {
            base.DrawObject(g);

            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);

            StartPos = AbsFieldToPixel(ptObjectStartPos);
            EndPos = AbsFieldToPixel(ptObjectEndPos);

            g.DrawRectangle(BaseDrawPen[(int)EDrawPenType.DRAW], 
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y), 
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
        }

        public override void MoveObject(PointF pPos)
        {
            base.MoveObject(pPos);
            PointF objectCurrentPos = new PointF(0, 0);

            //--------------------------------------------------------------------------------
            // Start Position Move
            objectCurrentPos.X = ptObjectStartPos.X;
            objectCurrentPos.Y = ptObjectStartPos.Y;
            objectCurrentPos.X += pPos.X;
            objectCurrentPos.Y += pPos.Y;
            SetObjectStartPos(objectCurrentPos);

            //--------------------------------------------------------------------------------
            // End Position Move
            objectCurrentPos.X = ptObjectEndPos.X;
            objectCurrentPos.Y = ptObjectEndPos.Y;
            objectCurrentPos.X += pPos.X;
            objectCurrentPos.Y += pPos.Y;
            SetObjectEndPos(objectCurrentPos);
        }

    }
    [Serializable]
    public class CObjectEllipse : CMarkingObject
    {
        public CObjectEllipse(PointF pStart, PointF pEnd, float pAngle = 0)
        {

            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectRatateAngle(pAngle);

            SetObjectType(EObjectType.ELLIPSE);
            SetObjectName("Ellipse");

            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectEllipse(CObjectEllipse pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectEndPos(pObject.ptObjectEndPos);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public override void DrawObject(Graphics g)
        {
            base.DrawObject(g);

            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);

            StartPos = AbsFieldToPixel(ptObjectStartPos);
            EndPos = AbsFieldToPixel(ptObjectEndPos);

            g.DrawEllipse(BaseDrawPen[(int)EDrawPenType.DRAW],
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));
        }

        public override void MoveObject(PointF pPos)
        {
            base.MoveObject(pPos);
            PointF objectCurrentPos = new PointF(0, 0);

            //--------------------------------------------------------------------------------
            // Start Position Move
            objectCurrentPos.X = ptObjectStartPos.X;
            objectCurrentPos.Y = ptObjectStartPos.Y;
            objectCurrentPos.X += pPos.X;
            objectCurrentPos.Y += pPos.Y;
            SetObjectStartPos(objectCurrentPos);

            //--------------------------------------------------------------------------------
            // End Position Move
            objectCurrentPos.X = ptObjectEndPos.X;
            objectCurrentPos.Y = ptObjectEndPos.Y;
            objectCurrentPos.X += pPos.X;
            objectCurrentPos.Y += pPos.Y;
            SetObjectEndPos(objectCurrentPos);
        }
    }
    [Serializable]
    public class CObjectFont : CMarkingObject
    {
        public CObjectFont(PointF pStart, PointF pEnd, float pAngle = 0)
        {
            SetObjectName("Font");

            CreateSortNum++;
        }
    }
    [Serializable]
    public class CObjectBmp : CMarkingObject
    {
        public CObjectBmp()
        {
            SetObjectName("Bmp");

            CreateSortNum++;
        }
    }
    [Serializable]
    public class CObjectCAD : CMarkingObject
    {
        public CObjectCAD()
        {
            SetObjectName("Cad");

            CreateSortNum++;
        }
    }
    [Serializable]
    public class CObjectGroup : CMarkingObject
    {
        public List<CMarkingObject> ObjectGroup = new List<CMarkingObject>();

        public CObjectGroup(CMarkingObject[] pGroup)
        {
            CreateGroup(pGroup);
            
        }

        public CObjectGroup(CObjectGroup pObject)
        {
            int nObjectCount = pObject.ObjectGroup.Count;
            CMarkingObject[] pGroup = new CMarkingObject[nObjectCount];

            for (int i = 0; i < nObjectCount; i++)
                pGroup[i] = pObject.ObjectGroup[i].Copy();

            CreateGroup(pGroup);

            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectEndPos(pObject.ptObjectEndPos);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        private void CreateGroup(CMarkingObject[] pGroup)
        {
            if (pGroup == null) return;
            SetObjectPosition(pGroup);
            SetObjectRatateAngle(0.0f);

            SetObjectType(EObjectType.GROUP);
            SetObjectName("Group");

            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);

            foreach (CMarkingObject pObject in pGroup)
            {
                // 만약 pObject가 Group이면.. 재귀적으로 다시 부픔.
                AddGroupObject(pObject);
            }
        }
        private void AddGroupObject(CMarkingObject pObject)
        {
            ObjectGroup.Add(pObject);
            GroupObjectCount = ObjectGroup.Count;
        }

        public override CMarkingObject PullGroupObject()
        {
            if (ObjectGroup.Count <= 0) return null;

            CMarkingObject pObject = new CMarkingObject();
            pObject = ObjectGroup.ElementAt(0);
            ObjectGroup.RemoveAt(0);
            GroupObjectCount = ObjectGroup.Count;
            return pObject;
        }
        private void SetObjectPosition(CMarkingObject[] pGroup)
        {
            if (pGroup == null || pGroup.Length <= 0) return;

            PointF pStart = new PointF(0, 0);
            PointF pEnd = new PointF(0, 0);

            int iCount = 0;
            foreach (CMarkingObject pObject in pGroup)
            {
                if (pObject.ObjectType == EObjectType.DOT) pObject.SetObjectEndPos(pObject.ptObjectStartPos);

                if (iCount == 0)
                {
                    pStart = pObject.ptObjectStartPos;
                    pEnd = pObject.ptObjectEndPos;
                }

                if (pObject.ptObjectStartPos.X < pStart.X) pStart.X = pObject.ptObjectStartPos.X;
                if (pObject.ptObjectStartPos.Y < pStart.Y) pStart.Y = pObject.ptObjectStartPos.Y;
                if (pObject.ptObjectEndPos.X < pStart.X) pStart.X = pObject.ptObjectEndPos.X;
                if (pObject.ptObjectEndPos.Y < pStart.Y) pStart.Y = pObject.ptObjectEndPos.Y;

                if (pObject.ptObjectStartPos.X > pEnd.X) pEnd.X = pObject.ptObjectStartPos.X;
                if (pObject.ptObjectStartPos.Y > pEnd.Y) pEnd.Y = pObject.ptObjectStartPos.Y;
                if (pObject.ptObjectEndPos.X > pEnd.X) pEnd.X = pObject.ptObjectEndPos.X;
                if (pObject.ptObjectEndPos.Y > pEnd.Y) pEnd.Y = pObject.ptObjectEndPos.Y;



                iCount++;
            }

            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
        }

        public override void DrawObject(Graphics g)
        {
            base.DrawObject(g);

            // if (ObjectGroup.Count <= 0) return;

            PointF pStart = new PointF(0, 0);
            PointF pEnd = new PointF(0, 0);

            foreach (CMarkingObject pObject in ObjectGroup)
            {
                pObject.DrawObject(g);
            }

        }

        public override void MoveObject(PointF pPos)
        {
            base.MoveObject(pPos);
            PointF objectCurrentPos = new PointF(0, 0);

            for (int i = 0; i < this.GroupObjectCount; i++)
            {
                // Group안에 Group이 있을 경우 재귀적 방식으로 Recall함.
                if (ObjectGroup[i].ObjectType == EObjectType.GROUP) ObjectGroup[i].MoveObject(pPos);

                //--------------------------------------------------------------------------------
                // Start Position Move
                objectCurrentPos.X = ObjectGroup[i].ptObjectStartPos.X;
                objectCurrentPos.Y = ObjectGroup[i].ptObjectStartPos.Y;
                objectCurrentPos.X += pPos.X;
                objectCurrentPos.Y += pPos.Y;
                ObjectGroup[i].SetObjectStartPos(objectCurrentPos);

                //--------------------------------------------------------------------------------
                // End Position Move
                objectCurrentPos.X = ObjectGroup[i].ptObjectEndPos.X;
                objectCurrentPos.Y = ObjectGroup[i].ptObjectEndPos.Y;
                objectCurrentPos.X += pPos.X;
                objectCurrentPos.Y += pPos.Y;
                ObjectGroup[i].SetObjectEndPos(objectCurrentPos);

            }

            //--------------------------------------------------------------------------------
            // Start Position Move
            objectCurrentPos.X = ptObjectStartPos.X;
            objectCurrentPos.Y = ptObjectStartPos.Y;
            objectCurrentPos.X += pPos.X;
            objectCurrentPos.Y += pPos.Y;
            SetObjectStartPos(objectCurrentPos);

            //--------------------------------------------------------------------------------
            // End Position Move
            objectCurrentPos.X = ptObjectEndPos.X;
            objectCurrentPos.Y = ptObjectEndPos.Y;
            objectCurrentPos.X += pPos.X;
            objectCurrentPos.Y += pPos.Y;
            SetObjectEndPos(objectCurrentPos);

        }
    }
}
