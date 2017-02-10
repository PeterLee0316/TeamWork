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
    class CWindowSelector
    {
        private List<CMarkingObject> ObjectList;

        public int GetObjectCloseToPoint(List<CMarkingObject> objectList, Point referencePoint, int distance=2)
        {
            Point[] pointObject = new Point[2];
            for (int i = 0; i < pointObject.Count(); i++) pointObject[i] = new Point();

            int nCount = 0;            

            foreach (CMarkingObject pObject in objectList)
            {
                nCount++;
                
                // Start, End 위치 읽기
                pointObject[0] = AbsFieldToPixel(pObject.ptObjectStartPos);
                pointObject[1] = AbsFieldToPixel(pObject.ptObjectEndPos);
                
                // Point가 Object의 반경 내에 있는지를 확인함.
                if (CheckPointInsideBound(pointObject[0], pointObject[1], referencePoint) == false) continue;

                // Object 확인
                if (CheckObjectCloseToPoint(pObject, referencePoint, distance)) 
                    m_FormScanner.SelectObjectListView(nCount-1);
                
            }

            return SUCCESS;
        }

        private bool CheckObjectCloseToPoint(CMarkingObject pObject, Point referencePoint, int distance)
        {
            Point[] pointObject = new Point[4];
            for (int i = 0; i < pointObject.Count(); i++) pointObject[i] = new Point();

            // Start, End 위치 읽기
            pointObject[0] = AbsFieldToPixel(pObject.ptObjectStartPos);
            pointObject[1] = AbsFieldToPixel(pObject.ptObjectEndPos);

            switch (pObject.ObjectType)
            {
                case (EObjectType.DOT):
                    if (CalsDistancePoint(referencePoint, pointObject[0]) <= distance) pObject.IsSelectedObject = true;
                    break;
                case (EObjectType.LINE):
                    if (CheckPointWithLine(pointObject[0], pointObject[1], referencePoint)) pObject.IsSelectedObject = true;
                    break;
                case (EObjectType.RECTANGLE):
                    CalsObjectCornerPoint(pObject, ref pointObject);
                    if (CheckPointWithLine(pointObject[0], pointObject[1], referencePoint)) { pObject.IsSelectedObject = true; break; }
                    if (CheckPointWithLine(pointObject[1], pointObject[2], referencePoint)) { pObject.IsSelectedObject = true; break; }
                    if (CheckPointWithLine(pointObject[2], pointObject[3], referencePoint)) { pObject.IsSelectedObject = true; break; }
                    if (CheckPointWithLine(pointObject[3], pointObject[0], referencePoint)) { pObject.IsSelectedObject = true; break; }
                    break;
                case (EObjectType.CIRCLE):
                    if (CheckPointWithCircle(pointObject[0], pointObject[1], referencePoint)) pObject.IsSelectedObject = true;
                    break;
                case (EObjectType.ELLIPSE):
                    if (CheckPointWithEllipse(pointObject[0], pointObject[1], pObject.ObjectRotateAngle, referencePoint)) pObject.IsSelectedObject = true;
                    break;
                case (EObjectType.GROUP):
                    // 재귀적으로 호출
                    CObjectGroup pGroup;
                    pGroup = (CObjectGroup)(pObject);
                    foreach (CMarkingObject G in pGroup.ObjectGroup)
                    {
                        G.IsSelectedObject = false;
                        // Start, End 위치 읽기
                        pointObject[0] = AbsFieldToPixel(G.ptObjectStartPos);
                        pointObject[1] = AbsFieldToPixel(G.ptObjectEndPos);

                        // Point가 Object의 반경 내에 있는지를 확인함.
                        if (CheckPointInsideBound(pointObject[0], pointObject[1], referencePoint) == false) continue;

                        //if( CheckObjectCloseToPoint(G, referencePoint, distance)) pObject.IsSelectedObject = true;
                    }
                    break;
                default:
                    break;
            }
            // 상태를 리턴한다.
            return pObject.IsSelectedObject;

        }

        public int GetObjectInRectangle(List<CMarkingObject> objectList, Rectangle referenceRect)
        {
            Point pointObject = new Point();
            Rectangle rectObject = new Rectangle();
            int nCount = 0;

            foreach (CMarkingObject pObject in objectList)
            {
                switch (pObject.ObjectType)
                {
                    case (EObjectType.DOT):
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect))
                        {
                            pObject.IsSelectedObject = true;
                            m_FormScanner.SelectObjectListView(nCount);
                        }
                        break;
                    case (EObjectType.LINE):
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect))
                        {
                            pointObject = AbsFieldToPixel(pObject.ptObjectEndPos);
                            if (CheckPointInsideRectagle(pointObject, referenceRect))
                            {
                                pObject.IsSelectedObject = true;
                                m_FormScanner.SelectObjectListView(nCount);
                            }
                        }
                        break;
                    case (EObjectType.RECTANGLE):
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        rectObject.X = pointObject.X;
                        rectObject.Y = pointObject.Y;
                        rectObject.Width  = AbsFieldToPixelX(pObject.ObjectWidth);
                        rectObject.Height = AbsFieldToPixelY(pObject.ObjectHeight);

                        if (CheckRectInsideRetagle(rectObject, referenceRect, pObject.ObjectRotateAngle))
                        {
                            pObject.IsSelectedObject = true;
                            m_FormScanner.SelectObjectListView(nCount);
                        }
                        break;
                    case (EObjectType.CIRCLE):
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        rectObject.X = pointObject.X;
                        rectObject.Y = pointObject.Y;
                        rectObject.Width = AbsFieldToPixelX(pObject.ObjectWidth);
                        rectObject.Height = AbsFieldToPixelY(pObject.ObjectHeight);

                        if (CheckRectInsideRetagle(rectObject, referenceRect))
                        {
                            pObject.IsSelectedObject = true;
                            m_FormScanner.SelectObjectListView(nCount);
                        }
                        break;
                    case (EObjectType.ELLIPSE):
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        rectObject.X = pointObject.X;
                        rectObject.Y = pointObject.Y;
                        rectObject.Width = AbsFieldToPixelX(pObject.ObjectWidth);
                        rectObject.Height = AbsFieldToPixelY(pObject.ObjectHeight);

                        if (CheckEllipseInsideRectagle(rectObject, referenceRect, pObject.ObjectRotateAngle))
                        {
                            pObject.IsSelectedObject = true;
                            m_FormScanner.SelectObjectListView(nCount);
                        }
                        break;
                    case (EObjectType.GROUP):
                        break;

                    default:

                        break;
                }

                nCount++;
            }
                       

            return SUCCESS;
        }

        public int GetObjectPartiallyInRectangle(List<CMarkingObject> objectList, Rectangle referenceRect)
        {
            int nCount = 0;

            foreach (CMarkingObject pObject in objectList)
            {
                switch (pObject.ObjectType)
                {
                    case (EObjectType.DOT):
                        Point pointObject = new Point();
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect))
                        {
                            pObject.IsSelectedObject = true;
                            m_FormScanner.SelectObjectListView(nCount);
                        }
                            break;
                    case (EObjectType.LINE):
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect))
                        {                            
                            pObject.IsSelectedObject = true;
                            m_FormScanner.SelectObjectListView(nCount);
                            break;
                        }
                        pointObject = AbsFieldToPixel(pObject.ptObjectEndPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect))
                        {
                            pObject.IsSelectedObject = true;
                            m_FormScanner.SelectObjectListView(nCount);
                            break;
                        }
                        break;
                    case (EObjectType.RECTANGLE):
                        break;
                    case (EObjectType.CIRCLE):
                        break;
                    case (EObjectType.ELLIPSE):
                        break;
                    case (EObjectType.GROUP):
                        break;
                    default:

                        break;
                }

                nCount++;
            }

            return SUCCESS;
        }

        private int CalsObjectCornerPoint(CMarkingObject pObject, ref Point[] pCorner )
        {
            Point posStart = new Point();
            Point posEnd = new Point();

            if (pObject.ObjectRotateAngle ==0.0)
            {
                posStart = AbsFieldToPixel(pObject.ptObjectStartPos);
                posEnd = AbsFieldToPixel(pObject.ptObjectEndPos);

                pCorner[0] = posStart;
                pCorner[2] = posEnd;

                pCorner[1].X = posEnd.X;
                pCorner[1].Y = posStart.Y;
                pCorner[3].X = posStart.X;
                pCorner[3].Y = posEnd.Y;
            }
            else
            {
                CPos_XY posPoint = new CPos_XY();
                CPos_XY posCenter = new CPos_XY();

                posCenter = pObject.ptObjectCenterPos;

                posPoint = pObject.ptObjectStartPos;
                posPoint = RotateCoordinate(posPoint, posCenter, pObject.ObjectRotateAngle);
                pCorner[0] = AbsFieldToPixel(posPoint);

                posPoint = pObject.ptObjectEndPos;
                posPoint = RotateCoordinate(posPoint, posCenter, pObject.ObjectRotateAngle);
                pCorner[2] = AbsFieldToPixel(posPoint);

                posPoint.dX = pObject.ptObjectEndPos.dX;
                posPoint.dY = pObject.ptObjectStartPos.dY;
                posPoint = RotateCoordinate(posPoint, posCenter, pObject.ObjectRotateAngle);
                pCorner[1] = AbsFieldToPixel(posPoint);

                posPoint.dX = pObject.ptObjectStartPos.dX;
                posPoint.dY = pObject.ptObjectEndPos.dY;
                posPoint = RotateCoordinate(posPoint, posCenter, pObject.ObjectRotateAngle);
                pCorner[3] = AbsFieldToPixel(posPoint);
            }


            return SUCCESS;
        }

        private int CalsDistancePoint(Point ptPos1, Point ptPos2)
        {
            double distanceX = (double)(ptPos1.X - ptPos2.X);
            double distanceY = (double)(ptPos1.Y - ptPos2.Y);

            int posDistance = (int)Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));

            return posDistance;
        }
        
        private bool CheckPointInsideBound(Point ptStart, Point ptEnd, Point referencePoint, int distance = 2)
        {
            Point ptCenter = new Point();
            ptCenter.X = (ptStart.X + ptEnd.X) / 2;
            ptCenter.Y = (ptStart.Y + ptEnd.Y) / 2;

            double widthBound  = (double)(ptEnd.X - ptCenter.X);
            double heightBound = (double)(ptEnd.Y - ptCenter.Y);
            double radiusBound = Math.Sqrt(Math.Pow(widthBound, 2) + Math.Pow(heightBound, 2));

            double widthPoint = (double)(referencePoint.X - ptCenter.X);
            double heightPoint = (double)(referencePoint.Y - ptCenter.Y);
            double radiusPoint = Math.Sqrt(Math.Pow(widthPoint, 2) + Math.Pow(heightPoint, 2));
            
            if ((radiusPoint- distance) < radiusBound) return true;
            else return false;
        }

        private bool CheckPointInsideRectagle(Point ptPos, Rectangle objectRect)
        {
            bool checkAxisX = ptPos.X > objectRect.X && ptPos.X < (objectRect.X + objectRect.Width);
            bool checkAxisY = ptPos.Y > objectRect.Y && ptPos.Y < (objectRect.Y + objectRect.Height);

            return checkAxisX & checkAxisY;
        }

        private bool CheckRectInsideRetagle(Rectangle objectRect,Rectangle roiRect, double pAngle=0.0)
        {
            bool checkStartPos;
            bool checkEndPos;
            if (pAngle == 0.0)
            {
                checkStartPos = (objectRect.X > roiRect.X && objectRect.Y > roiRect.Y);

                checkEndPos   = (objectRect.X + objectRect.Width)  < (roiRect.X + roiRect.Width) &&
                                (objectRect.Y + objectRect.Height) < (roiRect.Y + roiRect.Height);

                return checkStartPos & checkEndPos;
            }
            else
            {
                Point[] pointObject = new Point[4];
                for (int i = 0; i < pointObject.Count(); i++) pointObject[i] = new Point();

                // 중심을 구함.
                Point pointCenter = new Point();
                pointCenter.X = objectRect.X + objectRect.Width / 2;
                pointCenter.Y = objectRect.Y + objectRect.Height / 2;

                // Rectagle의 각 코너의 위치 확인
                pointObject[0].X = objectRect.X;
                pointObject[0].Y = objectRect.Y;
                pointObject[1].X = objectRect.X + objectRect.Width;
                pointObject[1].Y = objectRect.Y;
                pointObject[2].X = objectRect.X + objectRect.Width;
                pointObject[2].Y = objectRect.Y + objectRect.Height;
                pointObject[3].X = objectRect.X;
                pointObject[3].Y = objectRect.Y + objectRect.Height;

                for (int i = 0; i < pointObject.Count(); i++)
                {
                    // 코너를 회전 변환
                    pointObject[i] = RotateCoordinate(pointObject[i], pointCenter, pAngle);

                    // 회전된 코너가 Roi 밖에 있으면 False
                    if (CheckPointInsideRectagle(pointObject[i], roiRect) == false) return false;
                }
            }

            return true;
        }

        private bool CheckEllipseInsideRectagle(Rectangle objectRect, Rectangle roiRect, double pAngle = 0.0)
        {
            bool checkStartPos;
            bool checkEndPos;

            if (pAngle == 0.0)
            {
                checkStartPos = (objectRect.X > roiRect.X && objectRect.Y > roiRect.Y);

                checkEndPos = (objectRect.X + objectRect.Width) < (roiRect.X + roiRect.Width) &&
                                (objectRect.Y + objectRect.Height) < (roiRect.Y + roiRect.Height);

                return checkStartPos & checkEndPos;
            }
            else
            {
                double ellipsA = (double)objectRect.Width / 2;
                double ellipsB = (double)objectRect.Height / 2;
                double rotateSin = Math.Pow( Math.Sin(Deg2Rad(pAngle)), 2);
                double rotateCos = Math.Pow( Math.Cos(Deg2Rad(pAngle)), 2);
                double ellipseWidth  = ellipsA * ellipsB / Math.Sqrt(ellipsB * ellipsB * rotateCos + ellipsA * ellipsA * rotateSin);
                double ellipseHeight = ellipsA * ellipsB / Math.Sqrt(ellipsA * ellipsA * rotateCos + ellipsB * ellipsB * rotateSin);

                // 중심을 구함.
                Point pointCenter = new Point();
                pointCenter.X = objectRect.X + objectRect.Width / 2;
                pointCenter.Y = objectRect.Y + objectRect.Height / 2;

                Point pointStart = new Point();
                Point pointEnd = new Point();

                pointStart.X = pointCenter.X - (int)ellipseWidth;
                pointStart.Y = pointCenter.Y - (int)ellipseHeight;
                pointEnd.X = pointCenter.X + (int)ellipseWidth;
                pointEnd.Y = pointCenter.Y + (int)ellipseHeight;

                checkStartPos = (pointStart.X > roiRect.X && pointStart.Y > roiRect.Y);

                checkEndPos = (pointEnd.X) < (roiRect.X + roiRect.Width) &&
                              (pointEnd.Y) < (roiRect.Y + roiRect.Height);

                return checkStartPos & checkEndPos;
                
            }

            return true;
        }
        private bool CheckPointWithLine(Point ptLineStart, Point ptLineEnd, Point referencePoint, int distance = 2)
        {
            // Line 공식 적용
            double lineA = (double)(ptLineEnd.Y - ptLineStart.Y);
            double lineB = (double)(ptLineStart.X - ptLineEnd.X);
            double lineC = (double)((ptLineEnd.X - ptLineStart.X) * ptLineStart.Y) + //
                           (double)((ptLineStart.Y - ptLineEnd.Y) * ptLineStart.X);

            // 직선과의 거리 측정
            double distanceLine = Math.Abs(lineA * (double)referencePoint.X + lineB * (double)referencePoint.Y + lineC) /
                                  Math.Sqrt(lineA * lineA + lineB * lineB);

            if (distanceLine < (double)distance) return true;
            else return false;
        }

        private bool CheckPointWithCircle(Point ptStart, Point ptEnd, Point referencePoint, int distance = 2)
        {
            Point ptCenter = new Point();
            ptCenter.X = (ptStart.X + ptEnd.X) / 2;
            ptCenter.Y = (ptStart.Y + ptEnd.Y) / 2;
            
            double radiusBound = Math.Abs(ptCenter.X - ptStart.X);

            double widthPoint = (double)(referencePoint.X - ptCenter.X);
            double heightPoint = (double)(referencePoint.Y - ptCenter.Y);
            double radiusPoint = Math.Sqrt(Math.Pow(widthPoint, 2) + Math.Pow(heightPoint, 2));

            if (Math.Abs(radiusPoint - radiusBound) < distance) return true;
            else return false;
        }

        private bool CheckPointWithEllipse(Point ptStart, Point ptEnd,double rotateAngle, Point referencePoint, int distance = 2)
        {
            double ellipseA = Math.Abs(ptEnd.X - ptStart.X)/2;
            double ellipseB = Math.Abs(ptEnd.Y - ptStart.Y)/2;
            double ellipseLength = 0.0;

            // 타원의 촛점 구하기
            CPos_XY ellipseFocus1 = new CPos_XY();
            CPos_XY ellipseFocus2 = new CPos_XY();
            
            if (ellipseA > ellipseB)
            {
                ellipseFocus1.dX = Math.Sqrt(Math.Pow(ellipseA, 2) - Math.Pow(ellipseB, 2));
                ellipseFocus1.dY = 0.0;
                ellipseFocus2.dX = -ellipseFocus1.dX;
                ellipseFocus2.dY = 0.0;
                // 초점과 타원의 선의 Point와 거리
                ellipseLength = ellipseA * 2;
            }
            else
            {
                ellipseFocus1.dX = 0.0;
                ellipseFocus1.dY = Math.Sqrt(Math.Pow(ellipseB, 2) - Math.Pow(ellipseA, 2));
                ellipseFocus2.dX = 0.0;
                ellipseFocus2.dY = -ellipseFocus1.dY;
                // 초점과 타원의 선의 Point와 거리
                ellipseLength = ellipseB * 2;
            }

            // 타원의 중심 적용
            CPos_XY ellipseCenter = new CPos_XY();

            ellipseCenter.dX = (double)(ptStart.X + ptEnd.X) / 2;
            ellipseCenter.dY = (double)(ptStart.Y + ptEnd.Y) / 2;

            ellipseFocus1 += ellipseCenter;
            ellipseFocus2 += ellipseCenter;

            // 타원이 회전되었을 경우에 회전 변환함.
            if(rotateAngle !=0.0)
            {
                ellipseFocus1 = RotateCoordinate(ellipseFocus1, ellipseCenter, rotateAngle);
                ellipseFocus2 = RotateCoordinate(ellipseFocus2, ellipseCenter, rotateAngle);
            }

            // reference 위치 변환 
            double referencePosX = (double)referencePoint.X;
            double referencePosY = (double)referencePoint.Y;
            
            // reference 위치과 초점과의 거리를 계산함.
            double lengthPoint = Math.Sqrt(Math.Pow((ellipseFocus1.dX - referencePosX), 2) + Math.Pow((ellipseFocus1.dY - referencePosY), 2)) +
                                 Math.Sqrt(Math.Pow((ellipseFocus2.dX - referencePosX), 2) + Math.Pow((ellipseFocus2.dY - referencePosY), 2));

            if (Math.Abs(lengthPoint - ellipseLength) < distance)
                return true;
            else
                return false;
        }

    }
}
