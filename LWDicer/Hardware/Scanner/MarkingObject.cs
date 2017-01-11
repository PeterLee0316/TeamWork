using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

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
        /// Object의 End 위치
        public CPos_XY ptObjectCenterPos { get; private set; } = new CPos_XY(0, 0);
        public int SetObjectCenterPos(CPos_XY pPos)
        {
            ptObjectCenterPos = pPos.Copy();

            return SUCCESS;
        }

        //---------------------------------------------------------------------------
        /// Object의 회전 각도 위치
        public double ObjectRotateAngle { get; private set; } = 0;

        public void SetObjectRatateAngle(double pAngle)
        {
            if (pAngle < -360 || pAngle > 360) return;
            ObjectRotateAngle = pAngle;
        }

        private Rectangle ObjectDemension = new Rectangle(0, 0,0,0);
        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////

        #region 함수
        //public CMarkingObject(CPos_XY pStart, CPos_XY pEnd, float pAngle=0)
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
                //if (IsGroupObject == false)
                //if(ObjectType != EObjectType.GROUP)
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

        public virtual void MoveObject( CPos_XY pPos)
        {
            //if (this.ObjectType == EObjectType.GROUP)// MoveObject(pPos);
            //    for(int i=0; i<this.GroupObjectCount; i++)
            //    {
                    
            //    }



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
        public CObjectDot(CPos_XY pStart, float pAngle = 0)
        {
            SetObjectStartPos(pStart);
            SetObjectCenterPos(pStart);
            SetObjectRatateAngle(pAngle);

            SetObjectType(EObjectType.DOT);
            SetObjectName("Dot");
            
            CMarkingObject.CreateSortNum++;
            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectDot(CObjectDot pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectCenterPos(pObject.ptObjectStartPos);

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
        public CObjectLine(CPos_XY pStart, CPos_XY pEnd, float pAngle = 0)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectCenterPos((pStart + pEnd)/2);
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
            SetObjectCenterPos((pObject.ptObjectStartPos + pObject.ptObjectEndPos) / 2);
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
        public CObjectRectagle(CPos_XY pStart, CPos_XY pEnd, float pAngle = 0)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectCenterPos((pStart + pEnd) / 2);
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
            SetObjectCenterPos((pObject.ptObjectStartPos + pObject.ptObjectEndPos) / 2);
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
    public class CObjectCircle : CMarkingObject
    {
        public CObjectCircle(CPos_XY pStart, CPos_XY pEnd, float pAngle = 0)
        {
            SetObjectStartPos(pStart);
            SetObjectEndPos(pEnd);
            SetObjectCenterPos((pStart + pEnd) / 2);
            SetObjectRatateAngle(pAngle);

            SetObjectType(EObjectType.CIRCLE);
            SetObjectName("Circle");

            CMarkingObject.CreateSortNum++;

            SetObjectSortFlag(CreateSortNum);
        }

        public CObjectCircle(CObjectCircle pObject)
        {
            SetObjectStartPos(pObject.ptObjectStartPos);
            SetObjectEndPos(pObject.ptObjectEndPos);
            SetObjectCenterPos((pObject.ptObjectStartPos + pObject.ptObjectEndPos) / 2);
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

        public override void DrawObject(Graphics g)
        {
            base.DrawObject(g);

            //ImageAttributes imageAttr = new ImageAttributes();
            //imageAttr.SetThreshold(0.8f);
            

            g.DrawImage(m_CvtBitmap, rectDisplay, rectSourceImage, GraphicsUnit.Pixel);
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

        public override void DrawObject(Graphics g)
        {
            base.DrawObject(g);

            // if (ObjectGroup.Count <= 0) return;

            CPos_XY pStart = new CPos_XY(0, 0);
            CPos_XY pEnd = new CPos_XY(0, 0);

            foreach (CMarkingObject pObject in ObjectGroup)
            {
                pObject.DrawObject(g);
            }

        }

        public override void MoveObject(CPos_XY pPos)
        {
            base.MoveObject(pPos);
            CPos_XY objectCurrentPos = new CPos_XY(0, 0);

            for (int i = 0; i < this.GroupObjectCount; i++)
            {
                // Group안에 Group이 있을 경우 재귀적 방식으로 Recall함.
                if (ObjectGroup[i].ObjectType == EObjectType.GROUP)
                {
                    ObjectGroup[i].MoveObject(pPos);
                    continue;
                }

                // Group 내의 Object들의 위치 Position 이동
                //--------------------------------------------------------------------------------
                // Start Position Move
                objectCurrentPos.dX = ObjectGroup[i].ptObjectStartPos.dX;
                objectCurrentPos.dY = ObjectGroup[i].ptObjectStartPos.dY;
                objectCurrentPos.dX += pPos.dX;
                objectCurrentPos.dY += pPos.dY;
                ObjectGroup[i].SetObjectStartPos(objectCurrentPos);

                //--------------------------------------------------------------------------------
                // End Position Move
                objectCurrentPos.dX = ObjectGroup[i].ptObjectEndPos.dX;
                objectCurrentPos.dY = ObjectGroup[i].ptObjectEndPos.dY;
                objectCurrentPos.dX += pPos.dX;
                objectCurrentPos.dY += pPos.dY;
                ObjectGroup[i].SetObjectEndPos(objectCurrentPos);

            }

            // Group의 위치 Position 이동
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
}
