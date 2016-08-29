using System;
using System.IO;
using System.Net;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Windows.Forms;

using static LWDicer.Layers.DEF_Scanner;
using static LWDicer.Layers.DEF_Polygon;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_System;
using LWDicer.UI;

namespace LWDicer.Layers
{
    public class DEF_Polygon
    {
        public enum EMonitorMode { Controller, Head };

        public class CScannerRefComp
        {
            //public CMarkingManager Manager;
            //public CMarkingWindow Window;            
            //public FormScanWindow FormScanner;

            public MSocketClient ControlComm;
            public MSocketClient ScanHeadComm;
            public MDataManager DataManager;
            public MACS Process;
        }

        public class CScannerData
        {
            private string ControlAddressIP = "";
            private string HeadAddressIP = "";
        }        
    }

    public class MMeScannerPolygon:MObject,IMarkingScanner
    {
        #region 맴버 변수 설정
        
        private const float BMT_SCAN_WIDTH = 300.0f;
        private const int BMP_DATA_SIZE = 32;
        private const int BYTE_SIZE = 8;
                        
        public CScannerRefComp m_RefComp;
        public CScannerData m_Data;

        // tFtp를 사용하기 위핸 Process 맴버
        private ProcessStartInfo procTFTPInfo = new ProcessStartInfo();
        private Process procScanner = new Process();

        // 1bit Bmp 생성을 위한 변수
        private Bitmap m_Bitmap;
        BitmapData BmpData;
        private int BmpImageWidth = 10;
        private int BmpImageHeight = 10;

        byte[] BmpScanLine = new byte[1];
        private int[] point = new int[4];
        
        private float ratioWidth = 0;
        private float ratioHeight = 0;

        private string ControlAddress = "";
        private string ScanHeadAddress = "";
        private int ControlPortNum;
        private int ScanHeadPortNum;

        #endregion

        public MMeScannerPolygon(CObjectInfo objInfo, CScannerRefComp refComp) :base(objInfo)
        {
            m_RefComp = refComp;            

            SetControlAddress("172.18.7.160",1111);
            SetScanHeadAddress("172.18.7.161",1111);            

            // 통신 tFTP 통신 초기화
            InitializeTFTP();

            m_ScanManager = new CMarkingManager();
            m_ScanWindow = new CMarkingWindow();
            m_FormScanner = new FormScanWindow();

            //ConnetTelnet(EMonitorMode.Controller);
            //ConnetTelnet(EMonitorMode.Head);

            // Reset.ini 파일 생성
            // CreateResetIni();

        }

        public int SetData(CScannerData source)
        {
            m_Data = ObjectExtensions.Copy(source);

            return SUCCESS;
        }

        public int GetData(out CScannerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public void SetDataManager(MDataManager source)
        {
            m_RefComp.DataManager = source;
        }

        #region Scan 내부 Function 

        public void ShowScanWindow()
        {
            m_FormScanner.Show();
        }

        private void AddObject(EObjectType type, PointF start, PointF end)
        {
            m_ScanManager.AddObject(type, start, end);
        }
        public void AddLine(PointF start, PointF end)
        {
            AddObject(EObjectType.LINE, start,end);
        }
        public void DeleteObject(int nIndex)
        {
            m_ScanManager.DeleteObject(nIndex);
        }

        public void DeleteAllObject()
        {
            m_ScanManager.DeleteAllObject();
        }

        public void AddListView(CMarkingObject shape)
        {
            m_FormScanner.AddObjectList(shape);
        }

        public CMarkingObject GetObjectList(int nIndex)
        {
            return m_ScanManager.ObjectList[nIndex];
        }
        
        public CMarkingObject GetLastObject()
        {
            return m_ScanManager.GetLastObject();
        }

        public List<CMarkingObject> GetObjectAllList()
        {
            return m_ScanManager.ObjectList;
        }

        public void SetObjectAllList(List<CMarkingObject> objectList)
        {
            //m_ScanManager.ObjectList = objectList;
            m_ScanManager.ObjectList = ObjectExtensions.Copy(objectList);
        }


        #endregion

        #region BMP File Set & Draw

        public int SetSizeBmp()
        {
            if (m_RefComp.DataManager.ModelData.ScanData == null) return RUN_FAIL;
            if (m_RefComp.DataManager.ModelData.ScanData.InScanResolution <= 0 || m_RefComp.DataManager.ModelData.ScanData.CrossScanResolution <= 0) return RUN_FAIL;

            Point ptStart = new Point(0, 0);
            Point ptEnd = new Point(0, 0);
            float MaxHeight = 0.0f;
            float tempHeight = 0.0f;
            Point ptTemp = new Point(0, 0);

            int iObjectCount = m_ScanManager.ObjectList.Count;
            if (iObjectCount < 1) return SHAPE_LIST_DISABLE;

            for (int i=0; i < iObjectCount; i++)
            {
                tempHeight = m_ScanManager.ObjectList[i].ptObjectStartPos.Y;               
                if (tempHeight > MaxHeight) MaxHeight = tempHeight;

                tempHeight = m_ScanManager.ObjectList[i].ptObjectEndPos.Y;
                if (tempHeight > MaxHeight) MaxHeight = tempHeight;
            }
            
            ptEnd.X = (int)(BMT_SCAN_WIDTH / (m_RefComp.DataManager.ModelData.ScanData.InScanResolution) + 0.5);
            ptEnd.Y = (int)(MaxHeight / (m_RefComp.DataManager.ModelData.ScanData.CrossScanResolution) + 0.5);

            BmpImageWidth  = ptEnd.X;
            BmpImageHeight = ptEnd.Y;

            // 가로 세로의 각각의 배율을 구함.
            ratioWidth = (float)(BmpImageWidth) / BMT_SCAN_WIDTH;
            ratioHeight = (float)BmpImageHeight / MaxHeight;

            // 가로 사이즈는 32배수로 크기를 정한다. 
            // 소수점을 버리기 위해서... 32로 나누고.. 곱한다.(32배수 밑은 버림).
            BmpImageWidth = (int)(ptEnd.X / BMP_DATA_SIZE+0.5);
            BmpImageWidth *= BMP_DATA_SIZE;            

            // BMP File의 가로 한줄의 Byte Array의 크기를 설정한다.
            // 1bit BMP이므로 8를 나눈 값으로 설정함.
            Array.Resize<byte>(ref BmpScanLine, BmpImageWidth / 8);
            
            // BMP file의 크기를 설정한다
            m_Bitmap = new Bitmap(BmpImageWidth, BmpImageHeight+1, PixelFormat.Format1bppIndexed);
            
            BmpInit();
           
            return SUCCESS;
        }
                
        private void BmpInit(bool bWhite=true)
        {
            int iWidth = m_Bitmap.Width;
            int iHeight = m_Bitmap.Height;

            //Buffer.BlockCopy()
            if (bWhite)
                Parallel.For(0, BmpScanLine.Length, index => BmpScanLine[index] = 255);            
            else
                System.Array.Clear(BmpScanLine, 0, iWidth / 8);

            // BitmapData 생성
            Rectangle RecImage = new Rectangle(0, 0, iWidth, iHeight);
            BmpData = m_Bitmap.LockBits(RecImage, ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);

            for (int y = 0; y < RecImage.Height; y++)
            {
                Marshal.Copy(BmpScanLine, 0, (IntPtr)((long)BmpData.Scan0 + BmpData.Stride * y), BmpScanLine.Length);
            }

            m_Bitmap.UnlockBits(BmpData);         
        }

        public Bitmap ExpandBmpFile(Bitmap sourceBmp, int expandNum=8)
        {
            BitmapData sourceBmpData;
            BitmapData targetBmpData;
            Rectangle recSource;
            Rectangle recTarget;

            int iWidth  = sourceBmp.Width;
            int iHeight = sourceBmp.Height * expandNum;

            // BMP File의 가로 한줄의 Byte Array의 크기를 설정한다.
            // 1bit BMP이므로 8를 나눈 값으로 설정함.
            m_Bitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format1bppIndexed);
            Array.Resize<byte>(ref BmpScanLine, iWidth / 8);
            

            // Source BitmapData 생성
            recSource = new Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height);
            sourceBmpData = sourceBmp.LockBits(recSource, ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);

            // Target BitmapData 생성
            recTarget = new Rectangle(0, 0, iWidth, iHeight);
            targetBmpData = m_Bitmap.LockBits(recTarget, ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);

            for (int y = 0; y < recSource.Height; y++)
            {
                // source Image에서 가로 한줄을 byte[]로 Copy함
                Marshal.Copy((IntPtr)((long)sourceBmpData.Scan0 + sourceBmpData.Stride * y),
                                            BmpScanLine, 0, BmpScanLine.Length);

                for (int x = 0; x < expandNum; x++)
                {
                    Marshal.Copy(BmpScanLine, 0, (IntPtr)((long)targetBmpData.Scan0 + targetBmpData.Stride * (y * expandNum + x)), BmpScanLine.Length);
                }
            }

            sourceBmp.UnlockBits(sourceBmpData);
            m_Bitmap.UnlockBits(targetBmpData);

            return m_Bitmap;
        }

        /// <summary>
        /// Manager에 저장된 Object 전체를 BMP 파일로 저장함.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int ConvertBmpFile(string filePath)
        {
            try
            {
                int iObjectCount = m_ScanManager.ObjectList.Count;
                if (iObjectCount < 1) return SHAPE_LIST_DISABLE;

                for (int i = 0; i < iObjectCount; i++)
                {
                    // 생성된 BMP 파일에 Object Draw
                    DrawBmpFile(m_ScanManager.ObjectList[i]);
                }
                
                // 생성된 BMP을 파일 저장함.
                m_Bitmap.Save(filePath, ImageFormat.Bmp);

                m_Bitmap.Dispose();
            }
            catch
            { }

            return SUCCESS;
        }

        /// <summary>
        /// 1bit BMP 파일에 Object를 Draw하는 함수
        /// </summary>
        /// <param name="pObject"></param>
        /// <returns></returns>
        private int DrawBmpFile(CMarkingObject pObject)
        {
            Point ptStart = new Point(0, 0);
            Point ptEnd = new Point(0, 0);
            
            ptStart = PointToPixel(pObject.ptObjectStartPos);
            ptEnd = PointToPixel(pObject.ptObjectEndPos);

            switch (pObject.ObjectType)
            {
                case EObjectType.DOT:
                    SetIndexPixel(ptStart.X, ptStart.Y);
                    break;
                case EObjectType.LINE:
                    DrawLine(ptStart, ptEnd);
                    break;
                case EObjectType.RECTANGLE:
                    DrawLine(ptStart.X, ptStart.Y, ptEnd.X, ptStart.Y);
                    DrawLine(ptStart.X, ptStart.Y, ptStart.X, ptEnd.Y);
                    DrawLine(ptStart.X, ptEnd.Y, ptEnd.X, ptEnd.Y);
                    DrawLine(ptEnd.X, ptStart.Y, ptEnd.X, ptEnd.Y);

                    break;
                case EObjectType.ELLIPSE:
                    DrawEllipse(ptStart, ptEnd);
                    break;
                case EObjectType.GROUP:
                    CObjectGroup pGroup;
                    pGroup = (CObjectGroup)(pObject);
                    // 재귀적 방식으로  Object를 Draw를 진행함.                    
                    foreach (CMarkingObject G in pGroup.ObjectGroup)
                        DrawBmpFile(G);

                    break;
            }         

            return SUCCESS;
        }

        private Point PointToPixel(PointF pPos)
        {
            Point ptTemp = new Point(0, 0);

            ptTemp.X = (int)(pPos.X * ratioWidth + 0.5);
            ptTemp.Y = (int)(pPos.Y * ratioHeight + 0.5);

            return ptTemp;
        }
        private void ParallelCals(int i, int y)
        {
            // Line Check
            if (i < BmpImageWidth / 3 && y < BmpImageHeight / 3)
            {
                BmpScanLine[i / BYTE_SIZE] |= (byte)(0x80 >> (i % 8));
            }
            else
            {
                BmpScanLine[i / BYTE_SIZE] = 0;
            }
        }

        private void SetIndexPixel(int PosX, int PosY, bool bData = true)
        {
            int iWidth = m_Bitmap.Width;
            int iHeight = m_Bitmap.Height;

            if (PosX < 0 || PosY < 0) return;
            if (PosX >= iWidth || PosY >= iHeight) return;
            try
            {
                // BitmapData 생성
                Rectangle RecImage = new Rectangle(0, 0, BmpImageWidth, BmpImageHeight);
                BmpData = m_Bitmap.LockBits(RecImage, ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);

                // start byte address 
                int ByteAddress = PosX / BYTE_SIZE;

                // 해당 Byte의 이미지 데이터를 읽어온다.
                Marshal.Copy((IntPtr)((long)BmpData.Scan0 + BmpData.Stride * PosY + ByteAddress), BmpScanLine, ByteAddress, 1);

                // Byte 데이터를 Bit 단위로 연산한다.
                if (bData == true) // true 이면 검정색
                    BmpScanLine[PosX / BYTE_SIZE] &= (byte)~(0x80 >> (PosX % 8));
                else             // false 이면 흰색
                    BmpScanLine[PosX / BYTE_SIZE] |= (byte)(0x80 >> (PosX % 8));

                // 해당 이미지에 Byte데이터를 쓴다.
                Marshal.Copy(BmpScanLine, ByteAddress, (IntPtr)((long)BmpData.Scan0 + BmpData.Stride * PosY + ByteAddress), 1);

                m_Bitmap.UnlockBits(BmpData);
            }
            catch
            { }
        }

        private void DrawLine(Point ptStart, Point ptEnd)
        {
            // 변수 생성 및 초기화
            int CurrentValueX = 0;
            int CurrentValueY = 0;
            int BeforeValueX = -1;
            int BeforeValueY = -1;


            ////////////////////////////////////////////////////
            // Case 1
            // Point 1,2가 동일한 경우 리턴함.
            ////////////////////////////////////////////////////
            if (ptStart == ptEnd) return;

            ////////////////////////////////////////////////////
            // Case 2
            // 기울기가 0 인 경우 (추후 연산을 빠르게 하기 위해 Byte 단위별 쓰기 기능 추가 검토)
            ////////////////////////////////////////////////////
            if (ptEnd.Y == ptStart.Y)
            {
                CurrentValueY = ptEnd.Y;

                for (int i = 0; i <= Math.Abs(ptEnd.X - ptStart.X); i++)
                {
                    if ((ptEnd.X - ptStart.X) >= 0) CurrentValueX = ptStart.X + i;
                    if ((ptEnd.X - ptStart.X) < 0) CurrentValueX = ptStart.X - i;

                    if (BeforeValueX != CurrentValueX || BeforeValueY != CurrentValueY)
                        SetIndexPixel(CurrentValueX, CurrentValueY);

                    BeforeValueX = CurrentValueX;
                    BeforeValueY = CurrentValueY;
                }

                return;
            }

            ////////////////////////////////////////////////////
            // Case 3
            // 기울기가 무한대 인 경우
            ////////////////////////////////////////////////////
            if (ptEnd.X == ptStart.X)
            {
                CurrentValueX = ptEnd.X;

                for (int i = 0; i <= Math.Abs(ptEnd.Y - ptStart.Y); i++)
                {
                    if ((ptEnd.Y - ptStart.Y) >= 0) CurrentValueY = ptStart.Y + i;
                    if ((ptEnd.Y - ptStart.Y) < 0) CurrentValueY = ptStart.Y - i;

                    if (BeforeValueX != CurrentValueX || BeforeValueY != CurrentValueY)
                        SetIndexPixel(CurrentValueX, CurrentValueY);

                    BeforeValueX = CurrentValueX;
                    BeforeValueY = CurrentValueY;
                }
                return;
            }

            ////////////////////////////////////////////////////
            // Case 4
            // 기울기가 있을 경우
            ////////////////////////////////////////////////////            

            // Y축 방향으로 기울기를 구함.
            // 영상의 방향을 Y축이 반대로 되어 있음.
            float dSlope = (float)(ptEnd.Y - ptStart.Y) / (float)(ptEnd.X - ptStart.X);

            float dIncValue = 1 / dSlope;
            float dIncCount = Math.Abs(1 / dSlope);

            // X축 대비 Y축 증감이 1보다 크면... 1을 넣어준다.
            // (Pixel 단위로 증감을 위해서)
            if (Math.Abs(dIncValue) > 1) dIncValue = 1;

            // 시작점을 대입한다.
            float dValueX = (float)ptStart.X;
            float dValueY = (float)ptStart.Y;

            // X축의 시작점과 끝나는 점 확인 (이에 따라서.. 증감을 결정함)
            float dStartValue = (float)ptStart.X;
            float dEndValue = (float)ptEnd.X;
            dIncValue *= (dStartValue < dEndValue) ? 1 : -1;

            for (float dX = dStartValue; ; dX = dX + dIncValue)
            {
                if (dIncValue > 0 && dX >= dEndValue) return;
                if (dIncValue < 0 && dX <= dEndValue) return;

                // 연산된 값을 Int형으로 변환 (반올림)
                CurrentValueX = (int)(dValueX + 0.5);
                CurrentValueY = (int)(dValueY + 0.5);

                // 이전 값과 현재 값을 확인함
                // 다를 경우에 해당 좌표 Bitmap Pixel값을 변경함.
                if (BeforeValueX != CurrentValueX || BeforeValueY != CurrentValueY)
                    SetIndexPixel(CurrentValueX, CurrentValueY);

                // 이전값을 기억함.
                BeforeValueX = CurrentValueX;
                BeforeValueY = CurrentValueY;

                // 각 Point값을 연산함
                dValueX = dValueX + dIncValue;
                dValueY = dSlope * (dValueX - ptStart.X) + ptStart.Y;
            }
        }

        private void DrawLine(int PosX1, int PosY1, int PosX2, int PosY2)
        {
            Point posLine1 = new Point(PosX1, PosY1);
            Point posLine2 = new Point(PosX2, PosY2);

            DrawLine(posLine1, posLine2);
        }

        private void DrawEllipse(Point ptStart, Point ptEnd)
        {
            float dLengthX = Math.Abs((float)(ptStart.X - ptEnd.X)) - 1;
            float dLengthY = Math.Abs((float)(ptStart.Y - ptEnd.Y)) - 1;

            float dCenterX = (float)(ptStart.X + ptEnd.X) / 2;
            float dCenterY = (float)(ptStart.Y + ptEnd.Y) / 2;

            float dEllipseA = dLengthX / 2;
            float dEllipseB = dLengthY / 2;

            float dValueX1 = 0.0f;
            float dValueY1 = 0.0f;
            float dValueX2 = 0.0f;
            float dValueY2 = 0.0f;
            int count = 0;

            for (float dIncX = -dEllipseA; dIncX <= dEllipseA; dIncX = dIncX + 1)
            {
                dValueX1 = dIncX;
                dValueY1 = (float)Math.Sqrt(dEllipseB * dEllipseB * (1 - (dIncX * dIncX) / (dEllipseA * dEllipseA)));
                dValueY2 = -dValueY1;

                dValueX1 = dValueX1 + dCenterX;
                dValueY1 = dValueY1 + dCenterY;
                dValueY2 = dValueY2 + dCenterY;

                SetIndexPixel((int)(dValueX1), (int)(dValueY1));
                SetIndexPixel((int)(dValueX1), (int)(dValueY2));
                count++;
            }

            for (float dIncY = -dEllipseB; dIncY <= dEllipseB; dIncY = dIncY + 1)
            {
                dValueX1 = (float)Math.Sqrt(dEllipseA * dEllipseA * (1 - (dIncY * dIncY) / (dEllipseB * dEllipseB)));
                dValueX2 = -dValueX1;
                dValueY1 = dIncY;

                dValueX1 = dValueX1 + dCenterX;
                dValueX2 = dValueX2 + dCenterX;
                dValueY1 = dValueY1 + dCenterY;

                SetIndexPixel((int)(dValueX1), (int)(dValueY1));
                SetIndexPixel((int)(dValueX2), (int)(dValueY1));
                count++;
            }

        }
        #endregion

        #region Process, Polygon Save
        
        private void CreateResetIni()
        {
            string filePath = "";
            string fileName = "";

            // Job 파일을 설정한다.

            filePath = m_RefComp.DataManager.DBInfo.ScannerDataDir;// CMainFrame.DBInfo.ScannerDataDir;//m_RefComp.DataManager.DBInfo.ScannerDataDir;
            fileName = "reset.ini";

            if (!File.Exists(filePath + fileName))
            {
                var myFile = File.Create(filePath + fileName);
                myFile.Close();
            }
        }
        

        public bool SaveConfigPara(string strName)
        {
            string section = "";
            string key = "";
            string value = "";
            string filePath = "";
            bool bRet = false;

            filePath = string.Format("{0:s}{1:s}.ini", m_RefComp.DataManager.DBInfo.ScannerDataDir, strName);

            if (!File.Exists(filePath))
            {
                var myFile = File.Create(filePath);
                myFile.Close();
            }
            
            //----------------------------------------------------------------------
            section = "Job Settings";

            key = "InScanResolution";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.InScanResolution / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "CrossScanResolution";
            value = string.Format("{0:F7}", m_RefComp.DataManager.ModelData.ScanData.CrossScanResolution / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "InScanOffset";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.InScanOffset / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "StopMotorBetweenJobs";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.StopMotorBetweenJobs);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PixInvert";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.PixInvert);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "JobStartBufferTime";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.JobStartBufferTime);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PrecedingBlankLines";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.PrecedingBlankLines);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "Laser Configuration";

            key = "LaserOperationMode";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.LaserOperationMode);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "SeedClockFrequency";
            value = string.Format("{0:F0}",m_RefComp.DataManager.ModelData.ScanData.SeedClockFrequency * 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "RepetitionRate";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.RepetitionRate * 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PulsePickWidth";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.PulsePickWidth);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PixelWidth";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.PixelWidth);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PulsePickAlgor";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.PulsePickAlgor);
            bRet = CUtils.SetValue(section, key, value, filePath);
            

            //----------------------------------------------------------------------
            section = "CrossScan Configuration";

            key = "CrossScanEncoderResol";
            value = string.Format("{0:F7}", m_RefComp.DataManager.ModelData.ScanData.CrossScanEncoderResol / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "CrossScanMaxAccel";
            value = string.Format("{0:F2}", m_RefComp.DataManager.ModelData.ScanData.CrossScanMaxAccel);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "EnCarSig";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.EnCarSig);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "SwapCarSig";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.SwapCarSig);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "Head Configuration";

            key = "SerialNumber";
            value = string.Format("{0:F7}", m_RefComp.DataManager.ModelData.ScanData.SerialNumber);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FThetaConstant";
            value = string.Format("{0:F7}", m_RefComp.DataManager.ModelData.ScanData.FThetaConstant);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "ExposeLineLength";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.ExposeLineLength / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "EncoderIndexDelay";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.EncoderIndexDelay);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay0";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelay0 /1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay1";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelay1 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay2";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelay2 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay3";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelay3 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay4";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelay4 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay5";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelay5 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay6";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelay6 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay7";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelay7 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "InterleaveRatio";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.InterleaveRatio);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelayOffset0";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelayOffset0 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelayOffset1";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelayOffset1 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelayOffset2";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelayOffset2 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelayOffset3";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelayOffset3 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelayOffset4";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelayOffset4 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelayOffset5";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelayOffset5 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelayOffset6";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelayOffset6 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelayOffset7";
            value = string.Format("{0:F6}", m_RefComp.DataManager.ModelData.ScanData.FacetFineDelayOffset7 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "StartFacet";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.StartFacet);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "AutoIncrementStartFacet";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.AutoIncrementStartFacet);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "Polygon motor Configuration";

            //key = "InternalMotorDriverClk";
            //value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.InternalMotorDriverClk);
            //bRet = CUtils.SetValue(section, key, value, filePath);

            key = "MotorDriverType";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.MotorDriverType);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //key = "MotorSpeed";
            //value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.MotorSpeed);
            //bRet = CUtils.SetValue(section, key, value, filePath);

            //key = "SimEncSel";
            //value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.SimEncSel);
            //bRet = CUtils.SetValue(section, key, value, filePath);

            key = "MinMotorSpeed";
            value = string.Format("{0:F2}", m_RefComp.DataManager.ModelData.ScanData.MinMotorSpeed);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "MaxMotorSpeed";
            value = string.Format("{0:F2}", m_RefComp.DataManager.ModelData.ScanData.MaxMotorSpeed);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "MotorEffectivePoles";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.MotorEffectivePoles);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "SyncWaitTime";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.SyncWaitTime);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "MotorStableTime";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.MotorStableTime);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "ShaftEncoderPulseCount";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.ShaftEncoderPulseCount);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "Other Settings";

            key = "InterruptFreq";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.InterruptFreq);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "HWDebugSelection";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.HWDebugSelection);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "ExpoDebugSelection";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.ExpoDebugSelection);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "AutoRepeat";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.AutoRepeat);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PixAlwaysOn";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.PixAlwaysOn);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "ExtCamTrig";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.ExtCamTrig);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "EncoderExpo";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.EncoderExpo);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetTest";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.FacetTest);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "SWTest";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.SWTest);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "JobstartAutorepeat";
            value = Convert.ToString(m_RefComp.DataManager.ModelData.ScanData.JobstartAutorepeat);
            bRet = CUtils.SetValue(section, key, value, filePath);


            return true;
        }

        public bool SaveIsnPara(string strName)
        {
            string section = "";
            string key = "";
            string value = "";
            string filePath = "";
            bool bRet = false;

            filePath = string.Format("{0:s}{1:s}.ini", m_RefComp.DataManager.DBInfo.ScannerDataDir, strName);

            if (!File.Exists(filePath))
            {
                var myFile = File.Create(filePath);
                myFile.Close();
            }
            
            //----------------------------------------------------------------------
            section = "Global";

            key = "Enabled";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnEnabled);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "Home";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnHome);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "ProfileCtrl";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnProfileCtrl);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "CTRLPOS";

            key = "PF0S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF0S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF0E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF0E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF1S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF1S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF1E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF1E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF2S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF2S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF2E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF2E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF3S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF3S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF3E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF3E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF4S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF4S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF4E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF4E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF5S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF5S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF5E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF5E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF6S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF6S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF6E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF6E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF7S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF7S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF7E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.IsnPF7E);
            bRet = CUtils.SetValue(section, key, value, filePath);


            //----------------------------------------------------------------------
            section = "CTRLVAL";

            key = "VF0S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstXpos1);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF0E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos1);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF1S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstXpos2);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF1E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos2);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF2S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstXpos3);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF2E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos3);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF3S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstXpos4);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF3E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos4);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF4S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstXpos5);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF4E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos5);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF5S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstXpos6);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF5E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos6);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF6S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstXpos7);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF6E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos7);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF7S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstXpos8);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF7E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Xpos8);
            bRet = CUtils.SetValue(section, key, value, filePath);


            return true;
        }

        public bool SaveCsnPara(string strName)
        {
            string section = "";
            string key = "";
            string value = "";
            string filePath = "";
            bool bRet = false;

            filePath = string.Format("{0:s}{1:s}.ini", m_RefComp.DataManager.DBInfo.ScannerDataDir, strName);

            if (!File.Exists(filePath))
            {
                var myFile = File.Create(filePath);
                myFile.Close();
            }
            
            //----------------------------------------------------------------------
            section = "Global";

            key = "Enabled";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnEnabled);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "Home";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnHome);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "ProfileCtrl";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnProfileCtrl);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "CTRLPOS";

            key = "PF0S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF0S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF0E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF0E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF1S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF1S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF1E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF1E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF2S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF2S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF2E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF2E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF3S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF3S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF3E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF3E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF4S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF4S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF4E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF4E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF5S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF5S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF5E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF5E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF6S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF6S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF6E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF6E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF7S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF7S);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PF7E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.CsnPF7E);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "CTRLVAL";

            key = "VF0S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstYpos1);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF0E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos1);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF1S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstYpos2);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF1E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos2);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF2S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstYpos3);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF2E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos3);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF3S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstYpos4);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF3E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos4);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF4S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstYpos5);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF4E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos5);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF5S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstYpos6);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF5E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos6);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF6S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstYpos7);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF6E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos7);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF7S";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectFirstYpos8);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "VF7E";
            value = string.Format("{0:F0}", m_RefComp.DataManager.ModelData.ScanData.FacetCorrectLast_Ypos8);
            bRet = CUtils.SetValue(section, key, value, filePath);


            return true;
        }
        #endregion

        #region tFtp 통신 (PC <--> Controller & Head)
        

        /*------------------------------------------------------------------------------------
        * Date : 2016.03.03
        * Author : HSLEE
        * Function : SetControlAddress(int scannerIndex, string strIP)
        * Description : 
        * Parameter : int scannerIndex - Scanner No.
        *             string strIP - Scanner IP Address
        ------------------------------------------------------------------------------------*/
        public void SetControlAddress(string HostAddress, int HostPort)
        {
            ControlAddress = HostAddress;
            ControlPortNum = HostPort;
        }

        /*------------------------------------------------------------------------------------
       * Date : 2016.03.03
       * Author : HSLEE
       * Function : SetControlAddress(int scannerIndex, string strIP)
       * Description : 
       * Parameter : int scannerIndex - Scanner No.
       *             string strIP - Scanner IP Address
       ------------------------------------------------------------------------------------*/
        public void SetScanHeadAddress(string HostAddress, int HostPort)
        {
            ScanHeadAddress = HostAddress;
            ScanHeadPortNum = HostPort;
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : GetControlAddress(int scannerIndex)
         * Description :  
         * Parameter : int scannerIndex - Scanner No.
         * return : ex) ControlAddressIP "ftp://192.168.22.60:21/"
         ------------------------------------------------------------------------------------*/
        public string GetControlAddress()
        {
            return ControlAddress;
            //return "172.18.7.160";
        }

        public string GetScanHeadAddress()
        {
            return ScanHeadAddress;
            //return "172.18.7.161";
        }

        public int GetControlPortNum()
        {
            return ControlPortNum;
            //return "172.18.7.160";
        }

        public int GetScanHeadPortNum()
        {
            return ScanHeadPortNum;
            //return "172.18.7.161";
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SendConfig(int scannerIndex, string strFile)
         * Description : Scanner에 Configure ini 파일전송 
         *               File Path = SFA\LWDicer\ScannerLog
         * Parameter :   int scannerIndex - Scanner No.
         *               string strFile - 전송하고 자하는 ini File Name
         ------------------------------------------------------------------------------------*/
        public bool SendConfig(string strPath)
        {
            string strFTP = GetControlAddress(); // ex) "172.18.7.160
            
            if (SendTFTPFile(strFTP,strPath) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendTrueRaster(string strFile)
        {
            string strFTP = GetScanHeadAddress(); // ex) "172.18.7.160

            string strPath = string.Format("{0:s}", strFile);  // ex) "SFA\LWDicer\ScannerLog\configure.ini"

            if (SendTFTPFile(strFTP,strPath) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SendBitmap(string strFile)
         * Description : Scanner에 Configure BitMap 파일전송 
         *               File Path = SFA\LWDicer\ScannerLog
         * Parameter :   int scannerIndex - Scanner No.
         *               string strFile - 전송하고 자하는 BitMap File Name
         ------------------------------------------------------------------------------------*/
        public bool SendBitmap(string strFile)
        {
            string strFTP = GetControlAddress(); // ex) "92.168.22.60"
            //string strPath = string.Format("{0:s}{1:s}", m_RefComp.DataManager.DBInfo.ImageDataDir, strFile);  
            string filePath = strFile;

            if (SendTFTPFile(strFTP, filePath) == true)
            {
                return true;
            }
            else
            {
                return false;
            } 
        }
                

        /// <summary>
        /// Date : 2016.03.11
        /// Author : HSLEE
        /// Function : InitializeTFTP()
        /// Description : Scanner LSE Controller TFTP Data을 위한 초기화 구문
        /// </summary>

        private void InitializeTFTP()
        {
            //=================================================================================
            //  파일 전송 tFtp 설정
            // 32bit & 64bit OS에 따라서 cmd 실행 폴더 구분함.
            //if (IntPtr.Size == 4) procTFTPInfo.FileName = "C:\\Windows\\Sysnative\\cmd.exe";
            //else procTFTPInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
            
            procTFTPInfo.FileName = "C:\\Windows\\System32\\cmd.exe";

            procTFTPInfo.WorkingDirectory = "C:\\Windows\\System32";
            procTFTPInfo.WindowStyle = ProcessWindowStyle.Hidden;  // cmd창이 숨겨지도록 설정
            procTFTPInfo.CreateNoWindow = true; // cmd창을 띄우지 안도록 하기
            procTFTPInfo.UseShellExecute = false;

            procTFTPInfo.RedirectStandardOutput = true; // cmd창에서 데이터를 가져오기
            procTFTPInfo.RedirectStandardInput = true; // cmd창에서 데이터를 보내기
            procTFTPInfo.RedirectStandardError = true; // cmd창에서 오류내용 가져오기   

            // tFtp 파일 전송을 위한 프로그램 실행
            procScanner.StartInfo = procTFTPInfo;
        }

        public void ConnetTelnet(EMonitorMode pMode)
        {

        }

        public void TestCmd(string strCmd)
        { 

        }

        public string ReadControllerStatus()
        {
            string strData = "";

            return strData;
        }

        public string ReadHeadStatus()
        {
            string strData = "";


            return strData;
        }

        /// <summary>
        /// Date : 2016.03.11
        /// Author : HSLEE
        /// Function : SendTFTPFile(string strIP, string strFTP)
        /// Description : Scanner LSE Controller TFTP Data 전송
        ///               ex) tftp -i 192.168.22.76 put t:\test.bmp
        /// </summary>
        /// <param name="strIP"></param>  : TFTP Server IP
        /// <param name="strFilePath"></param> : 전송하고자 하는 File Path
        /// 
        private bool SendTFTPFile(string strIP, string strFilePath)
        {
            string strResult = "";

            try
            {
                procScanner.Start();
                procScanner.StandardInput.Write("tftp -i " + strIP + " put " + strFilePath + Environment.NewLine);
                procScanner.StandardInput.Close();
                //procScanner.BeginOutputReadLine();                

                if (procScanner.WaitForExit(2000) == false)
                {
                    procScanner.Close();
                    return false;
                }

                procScanner.Close();
            }
            catch
            {
                procScanner.Close();
            }

            return true;

        }

        #endregion

        #region Laser Process 동작
        public int LaserProcess(EScannerMode processMode)
        {
            LaserProcessRun();

            int bufferNum = 0;

            if (processMode == EScannerMode.MOF) bufferNum = DEF_SCANNER_MOF_RUN;
            if (processMode == EScannerMode.STILL) bufferNum = DEF_SCANNER_STILL_RUN;

            int iResult = m_RefComp.Process.LaserProcess(bufferNum);


            return iResult;
        }

        public int LaserProcessCount(int countSet)
        {
            int iResult = SUCCESS;
            string strVariable = "ProcessSet";            

            iResult = m_RefComp.Process.WriteBufferMemory(strVariable,countSet);

            return iResult;
        }

        public int LaserProcessRun()
        {
            int iResult = SUCCESS;
            string strVariable = "ProcessStop";

            iResult = m_RefComp.Process.WriteBufferMemory(strVariable, 0);

            return iResult;
        }

        public int LaserProcessStop()
        {
            int iResult = SUCCESS;
            string strVariable = "ProcessStop";

            iResult = m_RefComp.Process.WriteBufferMemory(strVariable, 1);

            return iResult;
        }

        public bool IsScannerBusy()
        {
            return m_RefComp.Process.IsScannerBusy();
        }

        public bool IsScannerJobStart()
        {
            return m_RefComp.Process.IsScannerJobStart();
        }

        public int GetScannerRunCount()
        {
            return m_RefComp.Process.GetScannerRunCount();
        }


        #endregion
    }

}
