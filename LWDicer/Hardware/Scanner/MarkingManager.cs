﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_Scanner;
using static LWDicer.Layers.DEF_Common;

using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Drawing;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;
using WW.Math;
using WW.Math.Geometry;
using WW.Drawing;

namespace LWDicer.Layers
{
    [Serializable]
    public class CMarkingManager
    {
        /////////////////////////////////////////////////////////////////////////////////////////

        #region 맴버 변수 설정
        public List<CMarkingObject> ObjectList = new List<CMarkingObject>();
        
        public DxfModel cadModel;

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////////////////////////////////////

        #region  설정

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
            SetBaseDrawPen((int)EDrawPenType.GRID_BRIGHT, System.Drawing.Color.DimGray, EPenDashStye.DOT);
            SetBaseDrawPen((int)EDrawPenType.GRID_BRIGHT, System.Drawing.Color.DimGray, EPenDashStye.DOT);
            SetBaseDrawPen((int)EDrawPenType.ACTIVE_BRIGHT, System.Drawing.Color.White);
            SetBaseDrawPen((int)EDrawPenType.ACTIVE_DARK, System.Drawing.Color.Black);
            SetBaseDrawPen((int)EDrawPenType.INACTIVE, System.Drawing.Color.DarkSlateGray);
            SetBaseDrawPen((int)EDrawPenType.OBJECT_DRAG, System.Drawing.Color.Red);
            SetBaseDrawPen((int)EDrawPenType.DIMENSION, System.Drawing.Color.LightGreen);
            SetBaseDrawPen((int)EDrawPenType.SELECT, System.Drawing.Color.Purple);

            // 사용할 Pen
            SetBaseDrawPen((int)EDrawPenType.DRAW, System.Drawing.Color.Black);

            //---------------------------------------------------------------------------
            //  Brush Setting
            SetBaseDrawBrush((int)EDrawBrushType.ACTIVE_BRIGHT, System.Drawing.Color.White);
            SetBaseDrawBrush((int)EDrawBrushType.ACTIVE_DARK, System.Drawing.Color.Black);
            SetBaseDrawBrush((int)EDrawBrushType.INACTIVE, System.Drawing.Color.DarkGray);
            SetBaseDrawBrush((int)EDrawBrushType.OBJECT_DRAG, System.Drawing.Color.Red);

            SetDrawBrush(System.Drawing.Color.Black);

            return SUCCESS;
        }

        public void SetFieldSize(SizeF pSize)
        {
            SetScanFieldSize(pSize);
        }

        public void SetResolution(SizeF pSize)
        {
            SetScanResolution(pSize);
        }

        #endregion


        #region  Object 관련함수

        public int GetObject(int nIndex, out CMarkingObject pObject)
        {
            if (nIndex < 0 || nIndex > ObjectList.Count)
            {
                pObject = null;
                return SHAPE_LIST_DISABLE;
            }
            pObject = ObjectList[nIndex - 1];

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

        public CMarkingObject MakeObject(EObjectType pType, CPos_XY pStart, CPos_XY pEnd)
        {
            CMarkingObject pObject;

            CPos_XY newStart = new CPos_XY();
            CPos_XY newEnd = new CPos_XY();

            newStart = pStart.Copy();
            newEnd = pEnd.Copy();

            switch (pType)
            {
                case (EObjectType.DOT):
                    pObject = new CObjectDot(newStart, newEnd);
                    break;
                case (EObjectType.LINE):
                    pObject = new CObjectLine(newStart, newEnd);
                    break;
                case (EObjectType.RECTANGLE):
                    pObject = new CObjectRectagle(newStart, newEnd);
                    break;
                case (EObjectType.CIRCLE):
                    pObject = new CObjectCircle(newStart, newEnd);
                    break;
                case (EObjectType.ELLIPSE):
                    pObject = new CObjectEllipse(newStart, newEnd);
                    break;
                default:
                    return null;
            }

            return pObject;
        }

        public CMarkingObject MakeObject(CMarkingObject pObject, CPos_XY pCenter)
        {
            CMarkingObject objectCopy;

            CPos_XY centerPos = new CPos_XY();
            centerPos = pCenter.Copy();

            switch (pObject.ObjectType)
            {
                case (EObjectType.DOT):
                    objectCopy = new CObjectDot((CObjectDot)pObject, centerPos);
                    break;
                case (EObjectType.LINE):
                    objectCopy = new CObjectLine((CObjectLine)pObject, centerPos);
                    break;
                case (EObjectType.RECTANGLE):
                    objectCopy = new CObjectRectagle((CObjectRectagle)pObject, centerPos);
                    break;
                case (EObjectType.CIRCLE):
                    objectCopy = new CObjectCircle((CObjectCircle)pObject, centerPos);
                    break;
                case (EObjectType.ELLIPSE):
                    objectCopy = new CObjectEllipse((CObjectEllipse)pObject, centerPos);
                    break;
                default:
                    return null;
            }


            return objectCopy;
        }

        public void AddObject(EObjectType pType, CPos_XY pStart, CPos_XY pEnd, CMarkingObject[] pObject = null)
        {
            switch (pType)
            {
                case (EObjectType.DOT):
                    var pDot = new CObjectDot(pStart,pEnd);
                    ObjectList.Add(pDot);
                    break;
                case (EObjectType.LINE):
                    var pLine = new CObjectLine(pStart, pEnd);
                    ObjectList.Add(pLine);
                    break;
                case (EObjectType.RECTANGLE):
                    var pRect = new CObjectRectagle(pStart, pEnd);
                    ObjectList.Add(pRect);
                    break;
                case (EObjectType.CIRCLE):
                    var pCircle = new CObjectCircle(pStart, pEnd);
                    ObjectList.Add(pCircle);
                    break;
                case (EObjectType.ELLIPSE):
                    var pEllipse = new CObjectEllipse(pStart, pEnd);
                    ObjectList.Add(pEllipse);
                    break;
                case (EObjectType.BMP):
                    string filename = null;
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            filename = openFileDialog.FileName;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error occurred: " + ex.Message);
                        }
                    }                    

                    var pBmp = new CObjectBmp(filename, pStart, pEnd);
                    ObjectList.Add(pBmp);

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
                case (EObjectType.CIRCLE):
                    CObjectCircle pCircle = new CObjectCircle((CObjectCircle)pObject);
                    ObjectList.Add(pCircle);
                    break;
                case (EObjectType.ELLIPSE):
                    CObjectEllipse pEllipse = new CObjectEllipse((CObjectEllipse)pObject);
                    ObjectList.Add(pEllipse);
                    break;
                case (EObjectType.GROUP):
                    CObjectGroup pGroup = new CObjectGroup((CObjectGroup)pObject);
                    ObjectList.Add(pGroup);
                    break;
                default:

                    break;
            }
        }

        public void InsertObject(int nIndex, EObjectType pType, CPos_XY pStart, CPos_XY pEnd)
        {
            CMarkingObject pObject = new CMarkingObject();
            pObject.SetObjectType(pType);

            pObject.SetObjectStartPos(pStart);
            pObject.SetObjectStartPos(pEnd);

            ObjectList.Insert(nIndex, pObject);
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

        public void DrawObject(BufferedGraphics bg)
        {
            Pen drawPen = BaseDrawPen[(int)EDrawPenType.DRAW];

            foreach (CMarkingObject s in this.ObjectList.ToArray<CMarkingObject>())
            {
                if (s.ObjectType == EObjectType.GROUP && s.IsSelectedObject)
                    drawPen = BaseDrawPen[(int)EDrawPenType.SELECT];
                else
                    drawPen = BaseDrawPen[(int)EDrawPenType.DRAW];

                s.DrawObject(bg, drawPen);
            }

            // 병렬 프로세싱은 안됨
            //int n = this.ObjectList.Count;
            //Parallel.For(0, n, (index) => this.ObjectList[index].DrawObject(bg));

        }

        public void LoadCadFile(string filePath)
        {
            if (filePath == null) return;

            // File Load ==============================================
            string extension = Path.GetExtension(filePath);
            if (string.Compare(extension, ".dwg", true) == 0)
            {
                try
                {
                    cadModel = DwgReader.Read(filePath);
                }
                catch
                {
                    return;
                }
            }
            else
            {
                cadModel = DxfReader.Read(filePath);
            }

            // Test Shpae
            //InsertDxfShapeLine(cadModel);
            //return;

            // Object Parsing ==========================================

            string strModel;
            Point3D posCircle, posLineStart, posLineEnd;
            double radiusCircle = 0.0;

            foreach (DxfEntity ent in cadModel.Entities)
            {
                strModel = ent.EntityType;

                switch (strModel)
                {
                    case "CIRCLE":
                        var circle = ent as DxfCircle;
                        InsertDxfCircle(circle.Center, circle.Radius);
                        break;
                    case "LINE":
                        var line = ent as DxfLine;
                        posLineStart = line.Start;
                        posLineEnd = line.End;
                        InsertDxfLine(line.Start, line.End);
                        break;
                    case "ELLIPSE":
                        InsertDxfPolyLine(ent, false);
                        break;
                    case "TEXT":
                        InsertDxfPolyLine(ent, true);
                        break;
                    case "MTEXT":
                        InsertDxfPolyLine(ent, true);
                        break;
                    case "LWPOLYLINE":
                        InsertDxfPolyLine(ent,true);
                        break;
                    case "ARC":
                        InsertDxfPolyLine(ent, false);
                        break;
                    case "SPLINE":
                        InsertDxfPolyLine(ent, false);
                        break;
                    default:
                        InsertDxfPolyLine(ent);
                        break;

                }
            }
            m_FormScanner.ReDrawCanvas();
        }

        private void InsertDxfLine(Point3D startPos, Point3D endPos)
        {
            CPos_XY posStart = new CPos_XY();
            CPos_XY posEnd = new CPos_XY();

            posStart = DxfToField(startPos);
            posEnd   = DxfToField(endPos);

            AddObject(EObjectType.LINE, posStart, posEnd);
            m_FormScanner.AddObjectList(GetLastObject());
        }
        private void InsertDxfPolyLine(DxfEntity polyLine, bool bCloseLine=true)
        {
            List<CPos_XY> dxfPolyLIne = new List<CPos_XY>();

            // Line을 얻어오는 Class 를 호출함
            CoordinatesCollector coordinatesCollector = new CoordinatesCollector();
            DrawContext.Wireframe drawContext = 
                      new DrawContext.Wireframe.ModelSpace(cadModel,GraphicsConfig.BlackBackground,Matrix4D.Identity);
            // List구조에 Line 정보를 Copy함
            polyLine.Draw(drawContext, coordinatesCollector);
            
            // Group할 Array 크기를 설정함.
            int iObjectNum = 0;
            for(int i=0; i< CoordinatesCollector.drawPolyLine.Count; i++)
            {
                int iLineNum = CoordinatesCollector.drawPolyLine[i].Count;
                // 그룹을 설정함. Close 타입의 Line을 경우와 아닐 경우 Line의 개수가 차이기 난다.
                if (bCloseLine == false) iLineNum--;
                iObjectNum += iLineNum;
            }

            CMarkingObject[] pGroup = new CMarkingObject[iObjectNum];

            int iGroupCount = 0;
            // Copy된 Line List를 Object로 삽입하여 기록함.
            foreach (Polyline4D polyLIne in CoordinatesCollector.drawPolyLine)
            {
                // Line List 초기화
                dxfPolyLIne.Clear();

                // PolyLIne 의 Line을 읽어 List에 추가함.
                foreach (Vector4D vector in polyLIne)
                {
                    Point3D point = (Point3D)vector;
                    dxfPolyLIne.Add(DxfToField(point));
                }                
                int iNum;
                // 각각의 Object를 만들에 Group에 추가한다.
                for (iNum = 0; iNum < dxfPolyLIne.Count - 1; iNum++)
                {
                    pGroup[iGroupCount] = MakeObject(EObjectType.LINE, dxfPolyLIne[iNum], dxfPolyLIne[iNum + 1]);
                    iGroupCount++;
                }
                // 패 Loop 도형일 경우에
                // 마지막 Line을 연결하기 위해... 시작 Point와 마지막 Point를 연결한다.
                if (bCloseLine == true)
                {                    
                    pGroup[iGroupCount ] = MakeObject(EObjectType.LINE, dxfPolyLIne[iNum], dxfPolyLIne[0]);
                    iGroupCount++;
                }                
            }
            // Group을 추가함.
            CPos_XY start = new CPos_XY(0, 0);
            CPos_XY end = new CPos_XY(0, 0);
            m_ScanManager.AddObject(EObjectType.GROUP, start, end, pGroup);
            m_FormScanner.AddObjectList(m_ScanManager.GetLastObject());

        }        

        private void InsertDxfCircle(Point3D posCircle, double radiusCircle)
        {
            CPos_XY posStart, posEnd;
            posStart = new CPos_XY();
            posEnd = new CPos_XY();
            double endWidth = Math.Cos(Deg2Rad(45)) * radiusCircle;

            // Circle과 Dot을 구분한다.
            if (radiusCircle < CIRCLE_SIZE_MIN)
            {
                posStart = DxfToField(posCircle);

                AddObject(EObjectType.DOT, posStart, posEnd);
            }
            else
            {
                posStart = DxfToField(posCircle);
                posEnd = DxfToField(posCircle, endWidth);
                AddObject(EObjectType.CIRCLE, posStart, posEnd);
            }

            m_FormScanner.AddObjectList(GetLastObject());
        }

        private void InsertDxfEllipse(Point3D posCircle, Vector3D axisLong, double axisRatio)
        {
            //PointF posStart, posEnd;
            //posStart = new PointF(0, 0);
            //posEnd = new PointF(0, 0);

            //// Circle과 Dot을 구분한다.
            //if (radiusCircle < 0.1)
            //{
            //    posStart = DxfToField(posCircle);

            //    AddObject(EObjectType.DOT, posStart, posEnd);
            //}
            //else
            //{
            //    posStart = DxfToField(posCircle, -radiusCircle);
            //    posEnd = DxfToField(posCircle, radiusCircle);

            //    AddObject(EObjectType.CIRCLE, posStart, posEnd);
            //}

            // m_FormScanner.AddObjectList(GetLastObject());
        }

        public CPos_XY DxfToField(Point3D posObject, double offSet = 0.0)
        {
            CPos_XY changePos = new CPos_XY();

            changePos.dX = (float)(posObject.X - offSet);
            changePos.dY = BaseScanFieldSize.Height - (float)(posObject.Y - offSet);

            return changePos;
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////

    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  
    class CoordinatesCollector : BaseWireframeGraphicsFactory
    {
        //public static List<PointF> drawPolyLine = new List<PointF>();
        public static IList<Polyline4D> drawPolyLine;// = new List<Polyline4D>();

        public override void CreateDot(
            DxfEntity entity,
            DrawContext.Wireframe drawContext,
            ArgbColor color,
            bool forText,
            Vector4D position
        )
        {
            Point3D point = (Point3D)position;
            // Console.WriteLine("Dot: {0}", point.ToString());
        }

        public override void CreateLine(
            DxfEntity entity,
            DrawContext.Wireframe drawContext,
            ArgbColor color,
            bool forText,
            Vector4D start,
            Vector4D end
        )
        {
            Point3D point1 = (Point3D)start;
            Point3D point2 = (Point3D)end;
           // Console.WriteLine("Line, start: {0}, end: {1}", start.ToString(), end.ToString());
        }

        public override void CreatePath(
            DxfEntity entity,
            DrawContext.Wireframe drawContext,
            ArgbColor color,
            bool forText,
            IList<Polyline4D> polylines,
            bool fill,
            bool correctForBackgroundColor
        )
        {
            WritePolylines(polylines);
        }

        public override void CreatePathAsOne(
            DxfEntity entity,
            DrawContext.Wireframe drawContext,
            ArgbColor color,
            bool forText,
            IList<Polyline4D> polylines,
            bool fill,
            bool correctForBackgroundColor
        )
        {
            WritePolylines(polylines);
        }        

        public override void CreateShape(
            DxfEntity entity,
            DrawContext.Wireframe drawContext,
            ArgbColor color,
            bool forText,
            IShape4D shape
        )
        {
            WritePolylines(shape.ToPolylines4D(ShapeTool.DefaultEpsilon));
            //WritePolylines(shape.ToPolylines4D(-0.001));
        }

        private static void WritePolylines(IList<Polyline4D> polylines)
        {
            drawPolyLine = polylines;            
        }
    }
    
}
