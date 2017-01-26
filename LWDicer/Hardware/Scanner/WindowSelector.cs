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
            for (int i = 0; i < 2; i++) pointObject[i] = new Point();

            foreach (CMarkingObject pObject in objectList)
            {
                // Select 초기화
                pObject.IsSelectedObject = false;
                // Start, End 위치 읽기
                pointObject[0] = AbsFieldToPixel(pObject.ptObjectStartPos);
                pointObject[1] = AbsFieldToPixel(pObject.ptObjectEndPos);

                // Point가 Object의 반경 내에 있는지를 확인함.
                if (CheckPointInsideBound(pointObject[0], pointObject[1], referencePoint) == false) continue;

                // Object 확인
                CheckObjectCloseToPoint(pObject, referencePoint, distance);
            }

            return SUCCESS;
        }

        private void CheckObjectCloseToPoint(CMarkingObject pObject, Point referencePoint, int distance)
        {
            Point[] pointObject = new Point[4];
            for (int i = 0; i < 4; i++) pointObject[i] = new Point();

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
                    if (CheckPointWithLine(pointObject[0], pointObject[1], referencePoint)) pObject.IsSelectedObject = true;
                    if (CheckPointWithLine(pointObject[1], pointObject[2], referencePoint)) pObject.IsSelectedObject = true;
                    if (CheckPointWithLine(pointObject[2], pointObject[3], referencePoint)) pObject.IsSelectedObject = true;
                    if (CheckPointWithLine(pointObject[3], pointObject[0], referencePoint)) pObject.IsSelectedObject = true;
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
        }

        public int GetObjectInRectangle(List<CMarkingObject> objectList, Rectangle referenceRect)
        {
            Point pointObject = new Point();
            foreach (CMarkingObject pObject in objectList)
            {
                pObject.IsSelectedObject = false;

                switch (pObject.ObjectType)
                {
                    case (EObjectType.DOT):
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect)) pObject.IsSelectedObject = true;
                        break;
                    case (EObjectType.LINE):
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect))
                        {
                            pointObject = AbsFieldToPixel(pObject.ptObjectEndPos);
                            if (CheckPointInsideRectagle(pointObject, referenceRect))
                                pObject.IsSelectedObject = true;
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
            }
                       

            return SUCCESS;
        }

        public int GetObjectPartiallyInRectangle(List<CMarkingObject> objectList, Rectangle referenceRect)
        {
            foreach (CMarkingObject pObject in objectList)
            {
                pObject.IsSelectedObject = false;

                switch (pObject.ObjectType)
                {
                    case (EObjectType.DOT):
                        Point pointObject = new Point();
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect)) pObject.IsSelectedObject = true;
                        break;
                    case (EObjectType.LINE):
                        pointObject = AbsFieldToPixel(pObject.ptObjectStartPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect))
                        {                            
                            pObject.IsSelectedObject = true;
                        }
                        pointObject = AbsFieldToPixel(pObject.ptObjectEndPos);
                        if (CheckPointInsideRectagle(pointObject, referenceRect))
                        {
                            pObject.IsSelectedObject = true;
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

        private bool CheckPointInsideRectagle(Point ptPos, Rectangle rectSize)
        {
            bool checkAxisX = ptPos.X > rectSize.X && ptPos.X < (rectSize.X + rectSize.Width);
            bool checkAxisY = ptPos.Y > rectSize.Y && ptPos.Y < (rectSize.Y + rectSize.Height);

            return checkAxisX & checkAxisY;
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

        private bool CheckPointWithEllipse(Point ptStart, Point ptEnd,double rotateAngle, Point referencePoint, int distance = 4)
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
