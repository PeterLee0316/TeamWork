using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


using static LWDicer.Layers.DEF_Scanner;
using static LWDicer.Layers.DEF_Common;


namespace LWDicer.Layers
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
        /// Shape가 Group되었는지 확인
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
        public CPos_XY ptObjectStartPos { get; private set; } = new CPos_XY(0, 0);
        public int SetObjectStartPos(CPos_XY pPos)
        {
            ptObjectStartPos = pPos.Copy();
            return SUCCESS;
        }

        //---------------------------------------------------------------------------
        /// Object의 End 위치
        public CPos_XY ptObjectEndPos { get; private set; } = new CPos_XY(0, 0);
        public int SetObjectEndPos(CPos_XY pPos)
        {
            ptObjectEndPos = pPos.Copy();
            return SUCCESS;
        }

        //---------------------------------------------------------------------------
        /// Object의 중심 위치
        public CPos_XY ptObjectCenterPos { get; private set; } = new CPos_XY(0, 0);
        public int SetObjectCenterPos(CPos_XY pPos)
        {
            ptObjectCenterPos = pPos.Copy();
            return SUCCESS;
        }

        //---------------------------------------------------------------------------
        /// Object의 가로 크기 (혹은 반지름,길이)
        public double ObjectWidth { get; private set; } = 0;

        public void SetObjectWidth(double pWidth)
        {
            if (pWidth <= 0) return;
            ObjectWidth = pWidth;
        }

        //---------------------------------------------------------------------------
        /// Object의 가로 크기 (혹은 반지름,길이)
        public double ObjectHeight { get; private set; } = 0;

        public void SetObjectHeight(double pHeight)
        {
            if (pHeight <= 0) return;
            ObjectHeight = pHeight;
        }

        //---------------------------------------------------------------------------
        /// Object의 회전 각도 위치
        public double ObjectRotateAngle { get; private set; } = 0;

        public void SetObjectRatateAngle(double pAngle)
        {
            if (pAngle < -360 || pAngle > 360) return;
            ObjectRotateAngle = pAngle;
        }

        //---------------------------------------------------------------------------
        /// Object의 Dimension 표시
        public CPos_XY[] ObjectDimension = new CPos_XY[(int)EObjectDimension.MAX];
        //public Point[] DisplayDimension = new Point[(int)EObjectDimension.MAX];

        public void SetObjectDimension(CPos_XY pStart, CPos_XY pEnd,double angle = 0.0)
        {
            if (ObjectType == EObjectType.DOT) pEnd = pStart;

            if (ObjectType == EObjectType.LINE)
            {
                ObjectDimension[0].dX = pStart.dX;
                ObjectDimension[0].dY = pStart.dY;

                ObjectDimension[1].dX = pEnd.dX;
                ObjectDimension[1].dY = pEnd.dY;

                ObjectDimension[2].dX = pEnd.dX;
                ObjectDimension[2].dY = pEnd.dY;

                ObjectDimension[3].dX = pStart.dX;
                ObjectDimension[3].dY = pStart.dY;
            }
            else
            {
                ObjectDimension[0].dX = pStart.dX;
                ObjectDimension[0].dY = pStart.dY;

                ObjectDimension[1].dX = pEnd.dX;
                ObjectDimension[1].dY = pStart.dY;

                ObjectDimension[2].dX = pEnd.dX;
                ObjectDimension[2].dY = pEnd.dY;

                ObjectDimension[3].dX = pStart.dX;
                ObjectDimension[3].dY = pEnd.dY;
            }
            
            
        }
        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////

        #region 함수
        public  CMarkingObject()
        {
            for (int i = 0; i < (int)EObjectDimension.MAX; i++)
            {
                ObjectDimension[i] = new CPos_XY();
            }
        }

        public virtual void DrawObject(BufferedGraphics bg,Pen pPen)
        {
            // Select한 상태를 확인
            if (IsSelectedObject)
            {
                DrawObjectDemension(bg);                
            }            
        }

        private void DrawObjectDemension(BufferedGraphics bg)
        {
            int nMargin = DRAW_DIMENSION_SIZE;
            Rectangle pRect = new Rectangle(0, 0, 0, 0);

            Point[] posDimension = new Point[(int)EObjectDimension.MAX];          

            // Dimension Start, End위치 읽기
            for(int i=0; i< (int)EObjectDimension.MAX;i++)
            {
                posDimension[i] = new Point();
                posDimension[i] = AbsFieldToPixel(ObjectDimension[i]);                               
            }

            //posDimension[0].X -= nMargin;
            //posDimension[0].Y -= nMargin;
            //posDimension[1].X += nMargin;
            //posDimension[1].Y -= nMargin;
            //posDimension[2].X += nMargin;
            //posDimension[2].Y += nMargin;
            //posDimension[3].X -= nMargin;
            //posDimension[3].Y += nMargin;


            bg.Graphics.DrawPolygon(BaseDrawPen[(int)EDrawPenType.DIMENSION], posDimension);
            

            //if (ObjectType == EObjectType.DOT) endPos = startPos;

            //// Dimension Rect Size 설정함
            //pRect.X = startPos.X < endPos.X ? startPos.X : endPos.X;
            //pRect.Y = startPos.Y < endPos.Y ? startPos.Y : endPos.Y;
            //pRect.Width = startPos.X > endPos.X ? (startPos.X - endPos.X) : -(startPos.X - endPos.X);
            //pRect.Height = startPos.Y > endPos.Y ? (startPos.Y - endPos.Y) : -(startPos.Y - endPos.Y);

            //// Dimension Margin 크기를 적용함
            //pRect.X -= nMargin;
            //pRect.Y -= nMargin;
            //pRect.Width  += nMargin*2;
            //pRect.Height += nMargin*2;

            //// Dot은 예외적으로 적용함.
            //if (ObjectType == EObjectType.DOT)
            //{
            //    pRect.X -= DRAW_DOT_SIZE/2;
            //    pRect.Y -= DRAW_DOT_SIZE/2;
            //    pRect.Width += DRAW_DOT_SIZE -1;
            //    pRect.Height += DRAW_DOT_SIZE -1;
            //}

            //// Rect과 Ellipse는 회전 Dimension을 적용함
            //if (ObjectType == EObjectType.RECTANGLE || ObjectType == EObjectType.ELLIPSE)
            //{
            //    Matrix matrix = new Matrix();
            //    PointF centerPos = new PointF();
            //    centerPos.X = (float)pRect.X + (float)pRect.Width / 2;
            //    centerPos.Y = (float)pRect.Y + (float)pRect.Height / 2;

            //    matrix.RotateAt((float)ObjectRotateAngle, centerPos);

            //    bg.Graphics.Transform = matrix;
            //}

            //bg.Graphics.DrawRectangle(BaseDrawPen[(int)EDrawPenType.DIMENSION], pRect);

            //bg.Graphics.ResetTransform();

        }

        public virtual void MoveObject( CPos_XY pPos)
        {

            //CPos_XY objectCurrentPos = new CPos_XY(0,0);

            ////--------------------------------------------------------------------------------
            //// Start Position Move
            //objectCurrentPos.dX = this.ptObjectStartPos.dX;
            //objectCurrentPos.dY = this.ptObjectStartPos.dY;
            //objectCurrentPos.dX += pPos.dX;
            //objectCurrentPos.dY += pPos.dY;
            //this.SetObjectStartPos(objectCurrentPos);

            ////--------------------------------------------------------------------------------
            //// End Position Move
            //objectCurrentPos.dX = this.ptObjectEndPos.dX;
            //objectCurrentPos.dY = this.ptObjectEndPos.dY;
            //objectCurrentPos.dX += pPos.dX;
            //objectCurrentPos.dY += pPos.dY;
            //this.SetObjectEndPos(objectCurrentPos);

        }

        public virtual void SetObjectProperty(CPos_XY pCenter)
        {
        }
        public virtual void SetObjectProperty(CPos_XY pStart, CPos_XY pEnd)
        {
        }

        public virtual void SetObjectProperty(CPos_XY pCenter, double pWitdh, double pHeight, double pAngle)
        {
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
        public  CObjectDot(CPos_XY pStart, CPos_XY pEnd)
        {
            SetObjectType(EObjectType.DOT);
            SetObjectName("Dot");
            SetObjectProperty(pStart, pEnd);    
            
            CMarkingObject.CreateSortNum++;
            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectDot(CObjectDot pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectCenterPos(pObject.ptObjectStartPos);
            SetObjectCenterPos(pObject.ptObjectCenterPos);
            SetObjectWidth(pObject.ObjectWidth);
            SetObjectHeight(pObject.ObjectHeight);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);            
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public CObjectDot(CObjectDot pObject,CPos_XY pCenter)
        {
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectProperty(pCenter, pObject.ObjectWidth, pObject.ObjectHeight, pObject.ObjectRotateAngle);            
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public override void SetObjectProperty(CPos_XY pStart,CPos_XY pEnd)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pStart);
            SetObjectCenterPos(pStart);

            double dAngle = 0.0;
            SetObjectRatateAngle(Rad2Deg(dAngle));
            SetObjectDimension(pStart, pEnd);
        }

        public override void SetObjectProperty(CPos_XY pStart, double length, double pHeight, double andgle)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pStart);
            SetObjectCenterPos(pStart);

            double dAngle = 0.0;
            SetObjectRatateAngle(Rad2Deg(dAngle));
        }

        public override void DrawObject(BufferedGraphics bg, Pen pPen)
        {
            
            Point DotPos = new Point(0,0);            
            
            DotPos = AbsFieldToPixel(ptObjectStartPos);

            Rectangle rectDot = new Rectangle(DotPos.X - DRAW_DOT_SIZE/2, DotPos.Y- DRAW_DOT_SIZE/2,
                                              DRAW_DOT_SIZE, DRAW_DOT_SIZE);

            bg.Graphics.FillRectangle(BaseDrawBrush[(int)EDrawBrushType.DRAW], rectDot);

            base.DrawObject(bg, pPen);
        }

        public override void MoveObject(CPos_XY pPos)
        {
            base.MoveObject(pPos);
            CPos_XY objectCurrentPos = new CPos_XY(0, 0);

            //--------------------------------------------------------------------------------
            // Start Position Move
            objectCurrentPos.dX = ptObjectStartPos.dX;
            objectCurrentPos.dY = ptObjectStartPos.dY;
            objectCurrentPos.dX += pPos.dX;
            objectCurrentPos.dY += pPos.dY;
            SetObjectStartPos(objectCurrentPos);
                        
        }
    }
    [Serializable]
    public class CObjectLine : CMarkingObject
    {
        public CObjectLine(CPos_XY pStart, CPos_XY pEnd)
        {
            SetObjectType(EObjectType.LINE);
            SetObjectName("Line");
            SetObjectProperty(pStart, pEnd);            
            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);
        }

        /// <summary>
        /// Object를 Copy할때 생성한다.
        /// </summary>
        /// <param name="pObject" Copy할 대상></param>
        public CObjectLine(CObjectLine pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectEndPos(pObject.ptObjectEndPos);
            SetObjectCenterPos(pObject.ptObjectCenterPos);
            SetObjectWidth(pObject.ObjectWidth);
            SetObjectHeight(pObject.ObjectHeight);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public CObjectLine(CObjectLine pObject, CPos_XY pCenter)
        {
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectProperty(pCenter, pObject.ObjectWidth, pObject.ObjectHeight, pObject.ObjectRotateAngle);            
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        // Dimension으로 설정함
        public override void SetObjectProperty(CPos_XY pStart, CPos_XY pEnd)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectCenterPos((pStart + pEnd) / 2);
            double dLength = Math.Sqrt(Math.Pow(pStart.dX - pEnd.dX, 2) +
                                       Math.Pow(pStart.dY - pEnd.dY, 2));
            SetObjectWidth(dLength);

            double dAngle = 0.0;
            if(pStart.dX != pEnd.dX)
                dAngle = Math.Atan((pStart.dY - pEnd.dY) / (pStart.dX - pEnd.dX));
            SetObjectRatateAngle(Rad2Deg(dAngle));
            SetObjectDimension(pStart, pEnd);
        }

        // Property로 설정함
        public override void SetObjectProperty(CPos_XY pCenter, double pLength, double pHeight, double pAngle)
        {
            SetObjectCenterPos(pCenter);
            SetObjectWidth(pLength);            
            SetObjectRatateAngle(pAngle);

            CPos_XY pStart = new CPos_XY();
            CPos_XY pEnd = new CPos_XY();
            double width  = Math.Cos(Deg2Rad(pAngle)) * pLength/2;
            double height = Math.Sin(Deg2Rad(pAngle)) * pLength/2;

            pStart = pCenter.Copy();
            pStart.dX -= width;
            pStart.dY -= height;
            SetObjectStartPos(pStart);

            pEnd = pCenter.Copy();
            pEnd.dX += width;
            pEnd.dY += height;
            SetObjectEndPos(pEnd);

            SetObjectDimension(pStart, pEnd);

        }

        public override void DrawObject(BufferedGraphics bg, Pen pPen)
        {
            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);

            StartPos = AbsFieldToPixel(ptObjectStartPos);
            EndPos = AbsFieldToPixel(ptObjectEndPos);            

            bg.Graphics.DrawLine(pPen, StartPos, EndPos);

            base.DrawObject(bg, pPen);
        }

        public override void MoveObject(CPos_XY pPos)
        {
            base.MoveObject(pPos);
            CPos_XY startPos = new CPos_XY(0, 0);
            CPos_XY endPos = new CPos_XY(0, 0);

            //--------------------------------------------------------------------------------
            // Start Position Move
            startPos.dX = ptObjectStartPos.dX + pPos.dX;
            startPos.dY = ptObjectStartPos.dY + pPos.dY;

            //--------------------------------------------------------------------------------
            // End Position Move
            endPos.dX = ptObjectEndPos.dX + pPos.dX;
            endPos.dY = ptObjectEndPos.dY + pPos.dY;

            SetObjectProperty(startPos, endPos);
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

        public CObjectRectagle(CPos_XY pStart, CPos_XY pEnd, double pAngle = 0)
        {
            SetObjectType(EObjectType.RECTANGLE);
            SetObjectName("Rectagle");
            SetObjectProperty(pStart, pEnd, pAngle);           

            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectRectagle(CObjectRectagle pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectEndPos(pObject.ptObjectEndPos);
            SetObjectCenterPos(pObject.ptObjectCenterPos);
            SetObjectWidth(pObject.ObjectWidth);
            SetObjectHeight(pObject.ObjectHeight);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public CObjectRectagle(CObjectRectagle pObject,CPos_XY pCenter)
        {
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectProperty(pCenter, pObject.ObjectWidth, pObject.ObjectHeight, pObject.ObjectRotateAngle);            
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        private void SetRectCornerPos()
        {
            CPos_XY cornerPos = new CPos_XY();
            double angle = Deg2Rad(ObjectRotateAngle);
            double width = ptObjectCenterPos.dX - ptObjectStartPos.dX;
            //cornerPos.dX = Math.Cos(angle)

        }

        public void SetObjectProperty(CPos_XY pStart, CPos_XY pEnd, double pAngle=0.0)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectCenterPos((pStart + pEnd) / 2);
            SetObjectWidth(Math.Abs(pStart.dX - pEnd.dX));
            SetObjectHeight(Math.Abs(pStart.dY - pEnd.dY));            
            SetObjectRatateAngle(pAngle);
            SetObjectDimension(pStart, pEnd, pAngle);
        }

        
        public override void SetObjectProperty(CPos_XY pCenter, double pWidth, double pHeigth, double pAngle)
        {
            CPos_XY startPos = new CPos_XY(0, 0);
            CPos_XY endPos = new CPos_XY(0, 0);

            startPos.dX = pCenter.dX - pWidth / 2;
            startPos.dY = pCenter.dY - pHeigth / 2;

            endPos.dX = pCenter.dX + pWidth / 2;
            endPos.dY = pCenter.dY + pHeigth / 2;

            SetObjectCenterPos(pCenter);
            SetObjectStartPos(startPos);
            SetObjectEndPos(endPos);            
            SetObjectWidth(pWidth);
            SetObjectHeight(pHeigth);
            SetObjectRatateAngle(pAngle);

            SetObjectDimension(startPos, endPos, pAngle);
        }

        public override void SetObjectProperty(CPos_XY pCenter)
        {
            CPos_XY startPos = new CPos_XY(0, 0);
            CPos_XY endPos = new CPos_XY(0, 0);
            double width = ObjectWidth;
            double height = ObjectHeight;

            startPos.dX = pCenter.dX - width / 2;
            startPos.dY = pCenter.dY - height / 2;

            endPos.dX = pCenter.dX + width / 2;
            endPos.dY = pCenter.dY + height / 2;

            SetObjectCenterPos(pCenter);
            SetObjectStartPos(startPos);
            SetObjectEndPos(endPos);
            SetObjectDimension(startPos, endPos, ObjectRotateAngle);
        }

        public override void DrawObject(BufferedGraphics bg, Pen pPen)
        {
            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);            

            StartPos = AbsFieldToPixel(ptObjectStartPos);
            EndPos = AbsFieldToPixel(ptObjectEndPos);            

            // Object 회전 변환---------------------------
            Point centerPos = new Point();
            Matrix matrix = new Matrix();
            PointF rotateCenter = new PointF();
            centerPos = AbsFieldToPixel(ptObjectCenterPos);
            rotateCenter.X = (float)centerPos.X;
            rotateCenter.Y = (float)centerPos.Y;
            matrix.RotateAt((float)ObjectRotateAngle, rotateCenter);
            bg.Graphics.Transform = matrix;
            //---------------------------------------------

            bg.Graphics.DrawRectangle(pPen, 
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y), 
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));

            bg.Graphics.ResetTransform();

            base.DrawObject(bg, pPen);
        }

        public override void MoveObject(CPos_XY pPos)
        {
            base.MoveObject(pPos);
            CPos_XY centerPos = new CPos_XY(0, 0);
            
            //  Position Move
            centerPos = ptObjectCenterPos + pPos;

            SetObjectProperty(centerPos);
        }

    }

    [Serializable]
    public class CObjectCircle : CMarkingObject
    {
        public CObjectCircle(CPos_XY pStart, CPos_XY pEnd, float pAngle = 0)
        {
            SetObjectType(EObjectType.CIRCLE);
            SetObjectName("Circle");
            SetObjectProperty(pStart, pEnd);            

            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectCircle(CObjectCircle pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectEndPos(pObject.ptObjectEndPos);
            SetObjectCenterPos(pObject.ptObjectCenterPos);
            SetObjectWidth(pObject.ObjectWidth);
            SetObjectHeight(pObject.ObjectHeight);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public CObjectCircle(CObjectCircle pObject,CPos_XY pCenter)
        {
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectProperty(pCenter, pObject.ObjectWidth, pObject.ObjectHeight, pObject.ObjectRotateAngle);            
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        // Drag Point로 설정함
        public override void SetObjectProperty(CPos_XY pStart, CPos_XY pEnd)
        {
            // Circle의 경우 Start가 원의 중심이고.. End와 의 거리가 반지름임.
            // 이를 반영하여 Dimension을 계산함.

            double radius = Math.Sqrt(Math.Pow((pStart.dX - pEnd.dX), 2) +
                                      Math.Pow((pStart.dY - pEnd.dY), 2));

            CPos_XY startPos = new CPos_XY();
            CPos_XY endPos = new CPos_XY();

            SetObjectProperty(pStart, radius, radius * 2, 0.0);
        }

        // Property로 설정함
        public override void SetObjectProperty(CPos_XY pCenter, double pRadius, double pHeight, double pAngle)
        {
            SetObjectCenterPos(pCenter);
            SetObjectWidth(pRadius);
            SetObjectRatateAngle(pAngle);

            CPos_XY startPos = new CPos_XY();
            CPos_XY endPos = new CPos_XY();

            startPos = pCenter.Copy();
            startPos.dX -= pRadius;
            startPos.dY -= pRadius;
            SetObjectStartPos(startPos);

            endPos = pCenter.Copy();
            endPos.dX += pRadius;
            endPos.dY += pRadius;
            SetObjectEndPos(endPos);

            SetObjectDimension(startPos, endPos, pAngle);
        }

        public override void DrawObject(BufferedGraphics bg, Pen pPen)
        {
            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);

            StartPos = AbsFieldToPixel(ptObjectStartPos);
            EndPos = AbsFieldToPixel(ptObjectEndPos);

            bg.Graphics.DrawEllipse(pPen,
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)));

            base.DrawObject(bg, pPen);
        }

        public override void MoveObject(CPos_XY pPos)
        {
            base.MoveObject(pPos);
            CPos_XY objectPos = new CPos_XY(0, 0);
            double radius = 0.0;

            //--------------------------------------------------------------------------------
            // Start Position Move
            objectPos.dX = ptObjectStartPos.dX;
            objectPos.dY = ptObjectStartPos.dY;
            objectPos.dX += pPos.dX;
            objectPos.dY += pPos.dY;
            SetObjectStartPos(objectPos);

            //--------------------------------------------------------------------------------
            // End Position Move
            objectPos.dX = ptObjectEndPos.dX;
            objectPos.dY = ptObjectEndPos.dY;
            objectPos.dX += pPos.dX;
            objectPos.dY += pPos.dY;
            SetObjectEndPos(objectPos);

            // 원의 중심 구함
            objectPos = (ptObjectStartPos + ptObjectEndPos) / 2;
            radius = Math.Abs(objectPos.dX - ptObjectStartPos.dX);

            SetObjectProperty(objectPos, radius,0.0,0.0);
        }
    }


    [Serializable]
    public class CObjectEllipse : CMarkingObject
    {
        public CObjectEllipse(CPos_XY pStart, CPos_XY pEnd, float pAngle = 0)
        {
            SetObjectType(EObjectType.ELLIPSE);
            SetObjectName("Ellipse");
            SetObjectProperty(pStart, pEnd);            

            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectEllipse(CObjectEllipse pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectEndPos(pObject.ptObjectEndPos);
            SetObjectCenterPos(pObject.ptObjectCenterPos);
            SetObjectWidth(pObject.ObjectWidth);
            SetObjectHeight(pObject.ObjectHeight);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public CObjectEllipse(CObjectEllipse pObject,CPos_XY pCenter)
        {
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectProperty(pCenter, pObject.ObjectWidth, pObject.ObjectHeight, pObject.ObjectRotateAngle);            
            SetObjectSortFlag(pObject.ObjectSortFlag);
        }

        public void SetObjectProperty(CPos_XY pStart, CPos_XY pEnd, double pAngle = 0.0)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectCenterPos((pStart + pEnd) / 2);
            SetObjectWidth(Math.Abs(pStart.dX - pEnd.dX));
            SetObjectHeight(Math.Abs(pStart.dY - pEnd.dY));
            SetObjectRatateAngle(pAngle);
            SetObjectDimension(pStart, pEnd, pAngle);
        }


        public override void SetObjectProperty(CPos_XY pCenter, double pWidth, double pHeigth, double pAngle)
        {
            CPos_XY startPos = new CPos_XY(0, 0);
            CPos_XY endPos = new CPos_XY(0, 0);

            startPos.dX = pCenter.dX - pWidth / 2;
            startPos.dY = pCenter.dY - pHeigth / 2;

            endPos.dX = pCenter.dX + pWidth / 2;
            endPos.dY = pCenter.dY + pHeigth / 2;

            SetObjectCenterPos(pCenter);
            SetObjectStartPos(startPos);
            SetObjectEndPos(endPos);
            SetObjectWidth(pWidth);
            SetObjectHeight(pHeigth);
            SetObjectRatateAngle(pAngle);
            SetObjectDimension(startPos, endPos, pAngle);
        }

        public override void SetObjectProperty(CPos_XY pCenter)
        {
            CPos_XY startPos = new CPos_XY(0, 0);
            CPos_XY endPos = new CPos_XY(0, 0);
            double width = ObjectWidth;
            double height = ObjectHeight;

            startPos.dX = pCenter.dX - width / 2;
            startPos.dY = pCenter.dY - height / 2;

            endPos.dX = pCenter.dX + width / 2;
            endPos.dY = pCenter.dY + height / 2;

            SetObjectCenterPos(pCenter);
            SetObjectStartPos(startPos);
            SetObjectEndPos(endPos);
            SetObjectDimension(startPos, endPos, ObjectRotateAngle);
        }

        public override void DrawObject(BufferedGraphics bg, Pen pPen)
        {
            Point StartPos = new Point(0, 0);
            Point EndPos = new Point(0, 0);

            StartPos = AbsFieldToPixel(ptObjectStartPos);
            EndPos = AbsFieldToPixel(ptObjectEndPos);

            // Object 회전 변환---------------------------
            Point centerPos = new Point();
            Matrix matrix = new Matrix();
            PointF rotateCenter = new PointF();
            centerPos = AbsFieldToPixel(ptObjectCenterPos);
            rotateCenter.X = (float)centerPos.X;
            rotateCenter.Y = (float)centerPos.Y;
            matrix.RotateAt((float)ObjectRotateAngle, rotateCenter);
            bg.Graphics.Transform = matrix;
            //---------------------------------------------

            bg.Graphics.DrawEllipse(pPen,
                            (StartPos.X < EndPos.X ? StartPos.X : EndPos.X),
                            (StartPos.Y < EndPos.Y ? StartPos.Y : EndPos.Y),
                            (StartPos.X > EndPos.X ? (StartPos.X - EndPos.X) : -(StartPos.X - EndPos.X)),
                            (StartPos.Y > EndPos.Y ? (StartPos.Y - EndPos.Y) : -(StartPos.Y - EndPos.Y)));

            bg.Graphics.ResetTransform();

            base.DrawObject(bg, pPen);
        }

        public override void MoveObject(CPos_XY pPos)
        {
            base.MoveObject(pPos);
            CPos_XY centerPos = new CPos_XY(0, 0);

            //  Position Move
            centerPos = ptObjectCenterPos + pPos;

            SetObjectProperty(centerPos);
        }
    }
    [Serializable]
    public class CObjectFont : CMarkingObject
    {
        public CObjectFont(CPos_XY pStart, CPos_XY pEnd, float pAngle = 0)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectCenterPos((pStart + pEnd) / 2);
            SetObjectRatateAngle(pAngle);

            SetObjectName("Font");

            CreateSortNum++;
        }
    }
    [Serializable]
    public class CObjectBmp : CMarkingObject
    {
        private Bitmap m_SrcBitmap;
        private Bitmap m_CvtBitmap;
        private Rectangle rectSourceImage = new Rectangle(0,0,0,0);
        private Rectangle rectDisplay = new Rectangle(0, 0, 0, 0);

        public CObjectBmp(string fileName, CPos_XY pStart, CPos_XY pEnd, float pAngle = 0)
        {
            m_SrcBitmap = new Bitmap(fileName);
            m_CvtBitmap = m_SrcBitmap.Copy();

            Threshold(ref m_CvtBitmap,500);

            var sizeDisplay = new CPos_XY(pEnd.dX - pStart.dX, pEnd.dY - pStart.dY);

            rectSourceImage.Location = new Point(0, 0);
            rectSourceImage.Width = m_SrcBitmap.Width;
            rectSourceImage.Height = m_SrcBitmap.Height;

            rectDisplay.Location = AbsFieldToPixel(pStart);
            rectDisplay.Width = AbsFieldToPixel(sizeDisplay).X;
            rectDisplay.Height = AbsFieldToPixel(sizeDisplay).Y;

            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectCenterPos((pStart + pEnd) / 2);
            SetObjectRatateAngle(pAngle);

            SetObjectName("Bmp");

            CreateSortNum++;
        }

        public override void DrawObject(BufferedGraphics bg, Pen pPen)
        {           

            //ImageAttributes imageAttr = new ImageAttributes();
            //imageAttr.SetThreshold(0.8f);
            

            bg.Graphics.DrawImage(m_CvtBitmap, rectDisplay, rectSourceImage, GraphicsUnit.Pixel);

            base.DrawObject(bg, pPen);
            //g.Dispose();
            //g.DrawImage(m_SrcBitmap, rectDisplay, rectSourceImage.dX, rectSourceImage.dY, rectSourceImage.Width, rectSourceImage.Height, GraphicsUnit.Pixel, imageAttr);
        }

        private void Grayscale(ref Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
/*
            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                while ((int)ptr != stopAddress)
                {
                    *ptr = (byte)((ptr[2] * .299) + (ptr[1] * .587) + (ptr[0] * .114));
                    ptr[1] = *ptr;
                    ptr[2] = *ptr;

                    ptr += 3;
                }
            }
            */
            bmp.UnlockBits(bmpData);
        }

        private  void Threshold(ref Bitmap bmp, short thresholdValue)
        {
            int MaxVal = 768;

            if (thresholdValue < 0) return;
            else if (thresholdValue > MaxVal) return;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
                                              ImageLockMode.ReadWrite,PixelFormat.Format24bppRgb);
/*
            unsafe
            {
                int TotalRGB;

                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                //while ((int)ptr != stopAddress)
                //{
                //    TotalRGB = ptr[0] + ptr[1] + ptr[2];

                //    if (TotalRGB <= thresholdValue)
                //    {
                //        ptr[2] = 0;
                //        ptr[1] = 0;
                //        ptr[0] = 0;
                //    }
                //    else
                //    {
                //        ptr[2] = 255;
                //        ptr[1] = 255;
                //        ptr[0] = 255;
                //    }

                //    ptr += 3;
                //}
            }
            */
            bmp.UnlockBits(bmpData);
        }


        public override void MoveObject(CPos_XY pPos)
        {
            base.MoveObject(pPos);
            CPos_XY objectCurrentPos = new CPos_XY(0, 0);

            //--------------------------------------------------------------------------------
            // Start Position Move
            objectCurrentPos.dX = ptObjectStartPos.dX;
            objectCurrentPos.dY = ptObjectStartPos.dY;
            objectCurrentPos.dX += pPos.dX;
            objectCurrentPos.dY += pPos.dY;
            SetObjectStartPos(objectCurrentPos);

            //--------------------------------------------------------------------------------
            // End Position Move
            objectCurrentPos.dX = ptObjectEndPos.dX;
            objectCurrentPos.dY = ptObjectEndPos.dY;
            objectCurrentPos.dX += pPos.dX;
            objectCurrentPos.dY += pPos.dY;
            SetObjectEndPos(objectCurrentPos);
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
            SetObjectCenterPos((pObject.ptObjectStartPos + pObject.ptObjectEndPos) / 2);
            SetObjectRatateAngle(pObject.ObjectRotateAngle);
            SetObjectType(pObject.ObjectType);
            SetObjectName(pObject.ObjectName);
            SetObjectSortFlag(pObject.ObjectSortFlag);

        }

        private void CreateGroup(CMarkingObject[] pGroup)
        {
            if (pGroup == null) return;

            SetObjectPosition(pGroup);
            SetObjectRatateAngle(0.0);

            SetObjectType(EObjectType.GROUP);
            SetObjectName("Group");

            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);

            foreach (CMarkingObject pObject in pGroup)
            {
                // 만약 pObject가 Group이면.. 재귀적으로 다시 Call
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

            CPos_XY pStart = new CPos_XY(0, 0);
            CPos_XY pEnd = new CPos_XY(0, 0);

            int iCount = 0;
            foreach (CMarkingObject pObject in pGroup)
            {
                if (pObject == null) continue;

                if (pObject.ObjectType == EObjectType.DOT) pObject.SetObjectEndPos(pObject.ptObjectStartPos);

                // 첫 Object의 Start,End의 위치를 저장함. 
                if (iCount == 0)
                {
                    pStart = pObject.ptObjectStartPos.Copy();
                    pEnd = pObject.ptObjectEndPos.Copy();
                }

                if (pObject.ptObjectStartPos.dX < pStart.dX) pStart.dX = pObject.ptObjectStartPos.dX;
                if (pObject.ptObjectStartPos.dY < pStart.dY) pStart.dY = pObject.ptObjectStartPos.dY;
                if (pObject.ptObjectEndPos.dX < pStart.dX) pStart.dX = pObject.ptObjectEndPos.dX;
                if (pObject.ptObjectEndPos.dY < pStart.dY) pStart.dY = pObject.ptObjectEndPos.dY;

                if (pObject.ptObjectStartPos.dX > pEnd.dX) pEnd.dX = pObject.ptObjectStartPos.dX;
                if (pObject.ptObjectStartPos.dY > pEnd.dY) pEnd.dY = pObject.ptObjectStartPos.dY;
                if (pObject.ptObjectEndPos.dX > pEnd.dX) pEnd.dX = pObject.ptObjectEndPos.dX;
                if (pObject.ptObjectEndPos.dY > pEnd.dY) pEnd.dY = pObject.ptObjectEndPos.dY;
                
                iCount++;
            }

            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectCenterPos((pStart + pEnd) / 2);
        }

        public override void DrawObject(BufferedGraphics bg, Pen pPen)
        {
           // base.DrawObject(bg);

            // if (ObjectGroup.Count <= 0) return;

            CPos_XY pStart = new CPos_XY(0, 0);
            CPos_XY pEnd = new CPos_XY(0, 0);

            foreach (CMarkingObject pObject in ObjectGroup)
            {
                pObject.DrawObject(bg,pPen);
            }
            
        }

        public override void MoveObject(CPos_XY pPos)
        {
            base.MoveObject(pPos);
            CPos_XY centerPos = new CPos_XY(0, 0);

            foreach (CMarkingObject pObject in ObjectGroup)
            {
                // Group안에 Group이 있을 경우 재귀적 방식으로 Recall함.
                if (pObject.ObjectType == EObjectType.GROUP)
                {
                    pObject.MoveObject(pPos);
                    continue;
                }
                // Group 내의 Object들의 위치 Position 이동
                centerPos = pObject.ptObjectCenterPos;
                centerPos += pPos;
                pObject.MoveObject(pPos);
            }
            // Group의 Dimension 이동
            SetObjectStartPos(ptObjectStartPos + pPos);
            SetObjectEndPos(ptObjectEndPos + pPos);

        }
    }
}
