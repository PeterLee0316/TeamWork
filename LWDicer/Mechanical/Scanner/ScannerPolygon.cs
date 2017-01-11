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
        public const int IMAGE_BLOCK_MAX_SIZE = 104857600; // 100MB

        public enum EMonitorMode { Controller, Head };

        #region Bitmap 파일 관련 structure

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPFILEHEADER
        {
            public UInt16 bfType;            // BMP 파일 매직 넘버. 비트맵 파일이 맞는지 확인하는데 사용하며 
                                             // ASCII 코드로 0x42(B), 0x4D(M)가 저장됩니다.
            public UInt32 bfSize;            // 파일 크기(바이트)
            public UInt16 bfReserved1;       // 현재는 사용하지 않으며 미래를 위해 예약된 공간
            public UInt16 bfReserved2;       // 현재는 사용하지 않으며 미래를 위해 예약된 공간
            public UInt32 bfOffBits;         // 비트맵 데이터의 시작 위치
        };

        public struct BITMAPINFOHEADER
        {
            public UInt32 biSize;            //  현재 비트맵 정보(BITMAPINFOHEADER)의 크기
            public UInt32 biWidth;           //  비트맵 이미지의 가로 크기(픽셀)
            public UInt32 biHeight;          //	 비트맵 이미지의 세로 크기(픽셀).
            public UInt16 biPlanes;          //  사용하는 색상판의 수. 항상 1입니다
            public UInt16 biBitCount;        //  픽셀 하나를 표현하는 비트 수
            public UInt32 biCompression;     //  압축 방식. 보통 비트맵은 압축을 하지 않으므로 0입니다
            public UInt32 biSizeImage;       //  비트맵 이미지의 픽셀 데이터 크기(압축 되지 않은 크기)
            public UInt32 biXPelsPerMeter;   //  그림의 가로 해상도(미터당 픽셀)
            public UInt32 biYPelsPerMeter;   //  그림의 세로 해상도(미터당 픽셀)
            public UInt32 biClrUsed;         //  색상 테이블에서 실제 사용되는 색상 수
            public UInt32 biClrImportant;    //  비트맵을 표현하기 위해 필요한 색상 인덱스 수
        };


        public struct COLORTABLE
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }
        public struct RASTERDATA
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        #endregion

        public class CScannerRefComp
        {
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

    public class MMeScannerPolygon : MMechanicalLayer, IMarkingScanner
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
        private int BmpBlockHeight = 10;
        private byte[] SaveObjectBytes = new byte[1];

        byte[] BmpScanLine = new byte[1];
        private int[] point = new int[4];
        
        private double ratioWidth = 0;
        private double ratioHeight = 0;

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

        private void AddObject(EObjectType type, CPos_XY start, CPos_XY end)
        {
            m_ScanManager.AddObject(type, start, end);
        }
        public void AddLine(CPos_XY start, CPos_XY end)
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

        public int CalsSizeBmpByte(EScannerIndex Index= EScannerIndex.SCANNER1)
        {
            int num = (int)Index;

            if (m_RefComp.DataManager.SystemData_Scan == null) return RUN_FAIL;

            if (m_RefComp.DataManager.SystemData_Scan.Config[num].InScanResolution <= 0 || 
                m_RefComp.DataManager.SystemData_Scan.Config[num].CrossScanResolution <= 0)
                return RUN_FAIL;

            Point ptStart = new Point(0, 0);
            Point ptEnd = new Point(0, 0);
            double MaxHeight = 0.0f;
            double tempHeight = 0.0f;
            Point ptTemp = new Point(0, 0);
            

            // 객체 수량 확인
            int iObjectCount = m_ScanManager.ObjectList.Count;
            if (iObjectCount < 1) return SHAPE_LIST_DISABLE;

            // 각 객체를 확인해서.. Bmp의 높이를 측정함.
            for (int i = 0; i < iObjectCount; i++)
            {
                tempHeight = m_ScanManager.ObjectList[i].ptObjectStartPos.dY;
                if (tempHeight > MaxHeight) MaxHeight = tempHeight;

                tempHeight = m_ScanManager.ObjectList[i].ptObjectEndPos.dY;
                if (tempHeight > MaxHeight) MaxHeight = tempHeight;
            }

            // 가로 세로 크기를 Pixel 단위로 변환
            ptEnd.X = (int)(BMT_SCAN_WIDTH / (m_RefComp.DataManager.SystemData_Scan.Config[num].InScanResolution) + 0.5);
            ptEnd.Y = (int)(MaxHeight / (m_RefComp.DataManager.SystemData_Scan.Config[num].CrossScanResolution) + 0.5);

            // Bmp의 가로 세로 크기 설정
            BmpImageWidth = ptEnd.X;
            BmpImageHeight = ptEnd.Y + 1;

            // 실제 크기와 Pixel과 배율을 결정함  (각 객체를 Pixel로 전환할때 사용함)
            ratioWidth = (float)(BmpImageWidth) / BMT_SCAN_WIDTH;
            if (MaxHeight == 0) ratioHeight = 0;
            else ratioHeight = (double)ptEnd.Y / MaxHeight;

            // 가로 사이즈는 32배수로 크기를 정한다. 
            // 가로 사이즈는 올림으로 계산한다.
            BmpImageWidth = (int)Math.Ceiling((double)ptEnd.X / BMP_DATA_SIZE);
            BmpImageWidth *= BMP_DATA_SIZE;

            // Bmp 크기를 계산해서 Stream으로 반복 저장 횟수를 설정함.
            // Pixel이 bit 단위이므로 Byte 크기를 사용할땐 8을 나눔.
            if (BmpImageHeight <= 0) BmpImageHeight = 1;
            long sizeCalsBmp = (long)BmpImageWidth * (long)BmpImageHeight / 8;
            BmpBlockHeight = BmpImageHeight;

            // Bmp 용량이 Block 최대치인 100MB를 넣을 경우
            // Bmp의 Height를 Block 단위로 사용한다. (Stream 방식)
            if (sizeCalsBmp > IMAGE_BLOCK_MAX_SIZE)
            {
                // 가로의 Byte 크기로 전체 용량을 나누어 높이 개수를 구함.
                BmpBlockHeight = IMAGE_BLOCK_MAX_SIZE / (BmpImageWidth / 8);
            }

            return SUCCESS;
        }

        public int SetSizeBmp(EScannerIndex Index = EScannerIndex.SCANNER1)
        {
            int num = (int)Index;

            if (m_RefComp.DataManager.SystemData_Scan == null) return RUN_FAIL;

            if (m_RefComp.DataManager.SystemData_Scan.Config[num].InScanResolution <= 0 || 
                m_RefComp.DataManager.SystemData_Scan.Config[num].CrossScanResolution <= 0)
                return RUN_FAIL;

            Point ptStart = new Point(0, 0);
            Point ptEnd = new Point(0, 0);
            double MaxHeight = 0.0f;
            double tempHeight = 0.0f;
            Point ptTemp = new Point(0, 0);

            // 객체 수량 확인
            int iObjectCount = m_ScanManager.ObjectList.Count;
            if (iObjectCount < 1) return SHAPE_LIST_DISABLE;

            // 각 객체를 확인해서.. Bmp의 높이를 측정함.
            for (int i=0; i < iObjectCount; i++)
            {
                tempHeight = m_ScanManager.ObjectList[i].ptObjectStartPos.dY;               
                if (tempHeight > MaxHeight) MaxHeight = tempHeight;

                tempHeight = m_ScanManager.ObjectList[i].ptObjectEndPos.dY;
                if (tempHeight > MaxHeight) MaxHeight = tempHeight;
            }
            
            // 가로 세로 크기를 Pixel 단위로 변환
            ptEnd.X = (int)(BMT_SCAN_WIDTH / (m_RefComp.DataManager.SystemData_Scan.Config[num].InScanResolution) + 0.5);
            ptEnd.Y = (int)(MaxHeight / (m_RefComp.DataManager.SystemData_Scan.Config[num].CrossScanResolution)+0.5);

            // Bmp의 가로 세로 크기 설정
            BmpImageWidth = ptEnd.X;
            BmpImageHeight = ptEnd.Y+1;

            // 실제 크기와 Pixel과 배율을 결정함  (각 객체를 Pixel로 전환할때 사용함)
            ratioWidth = (float)(BmpImageWidth) / BMT_SCAN_WIDTH;
            if (MaxHeight == 0) ratioHeight = 0;
            else  ratioHeight = (float)ptEnd.Y / MaxHeight;

            //// 가로 사이즈는 32배수로 크기를 정한다. 
            //// 가로 사이즈는 올림으로 계산한다.
            //BmpImageWidth = (int)Math.Ceiling((double)ptEnd.X / BMP_DATA_SIZE);
            //BmpImageWidth *= BMP_DATA_SIZE;            

            // BMP File의 가로 한줄의 Byte Array의 크기를 설정한다.
            // 1bit BMP이므로 8를 나눈 값으로 설정함.
            Array.Resize<byte>(ref BmpScanLine, BmpImageWidth / 8);

            // BMP file의 크기를 설정한다
            // 2G 이상의 크기는 설정이 불가능함.
            if (BmpImageHeight <= 0) BmpImageHeight = 1;
            try
            {
                m_Bitmap = new Bitmap(BmpImageWidth, BmpImageHeight, PixelFormat.Format1bppIndexed);
            }
            catch
            {
                //return RUN_FAIL;
            }
            
            BmpInit();
           
            return SUCCESS;
        }
                
        private void BmpInit(bool bWhite=true)
        {
            if (m_Bitmap == null) return;
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
        public int SaveScanFile(string filePath)
        {
            int bytesStart = 0;
            int bytesEnd = BmpBlockHeight;
            bool checkObjectActive;
            Point ptObjectStart = new Point(0, 0);
            Point ptObjectEnd = new Point(0, 0);

            // 저장할 File의  Head & Info 부분을 저장함
            WriteBitmapHead(filePath, BmpImageWidth, BmpImageHeight);

            // 저장할 File의 크기 계산
            int saveByteSize = BmpImageWidth * BmpBlockHeight / 8;
            // 나누어서 저장할때 남은 Heigth개수
            int leftImageHeigth = BmpImageHeight;

            if (leftImageHeigth > BmpBlockHeight)
            {
                // 저장할 Block 사이즈를 조정
                Array.Resize(ref SaveObjectBytes, saveByteSize);

                // Max Block 단위로 Object List를 File로 저장함.
                for (int i = BmpBlockHeight; i < BmpImageHeight; i += BmpBlockHeight)
                {
                    // 해당 ByteArray Clear
                    Array.Clear(SaveObjectBytes, 0, SaveObjectBytes.Length);
                    // ObjectList를 Byte Block에 Draw함
                    DrawObjectList(bytesStart, bytesEnd);
                    // Byte Array를 Stream 방식으로 저장함. 
                    WriteBitmapImage(filePath, SaveObjectBytes);

                    // 저장할 Block의 구역을 정함. ( 전체 크기에서 Block 단위로 위치를 변경함 )
                    bytesStart += BmpBlockHeight;
                    bytesEnd   += BmpBlockHeight;
                    // 남은 Image의 Heigth를 
                    leftImageHeigth -= BmpBlockHeight;

                    
                }

                // 반복으로 저장 한 후 남은 Image를 저장함.
                // 저장할 Block 사이즈를 조정
                if(leftImageHeigth<0) leftImageHeigth += BmpBlockHeight;
                BmpBlockHeight = leftImageHeigth;
                saveByteSize = BmpImageWidth * BmpBlockHeight / 8;
                Array.Resize(ref SaveObjectBytes, saveByteSize);
                // 해당 ByteArray Clear
                Array.Clear(SaveObjectBytes, 0, SaveObjectBytes.Length);
                // ObjectList를 Byte Block에 Draw함
                DrawObjectList(bytesStart, bytesEnd);
                // Byte Array를 Stream 방식으로 저장함. 
                WriteBitmapImage(filePath, SaveObjectBytes);
            }
            else
            {
                // 저장할 Block 사이즈를 조정
                BmpBlockHeight = leftImageHeigth;
                saveByteSize = BmpImageWidth * BmpBlockHeight / 8;
                Array.Resize(ref SaveObjectBytes, saveByteSize);
                // 해당 ByteArray Clear
                Array.Clear(SaveObjectBytes, 0, SaveObjectBytes.Length);
                // ObjectList를 Byte Block에 Draw함
                DrawObjectList(bytesStart, bytesEnd);
                // Byte Array를 Stream 방식으로 저장함. 
                WriteBitmapImage(filePath, SaveObjectBytes);
            }

            // Byte Array Memory 
            Array.Resize(ref SaveObjectBytes, 1);

            return SUCCESS;
        }

        private int DrawObjectList(int HeightStart, int HeigthEnd)
        {
            int bytesStart = HeightStart;
            int bytesEnd   = HeigthEnd;
            bool checkObjectActive;

            Point ptObjectStart = new Point(0, 0);
            Point ptObjectEnd = new Point(0, 0);

            foreach (CMarkingObject pObject in m_ScanManager.ObjectList)
            {
                // 객체의 위치를  Pixel 단위로 변경함
                ptObjectStart = PointToPixel(pObject.ptObjectStartPos);
                ptObjectEnd = PointToPixel(pObject.ptObjectEndPos);

                // 객체가 Save할 위치에 있는지를 확인함.
                checkObjectActive = (ptObjectStart.Y >= bytesStart && ptObjectStart.Y < bytesEnd) ||
                                    (ptObjectEnd.Y   >= bytesStart && ptObjectEnd.Y   < bytesEnd);

                if (checkObjectActive)
                {
                    // 생성된 BMP 파일에 Object Draw
                    DrawObjectBytes(pObject, bytesStart);
                }
                
            }

            return SUCCESS;
        }

        private int WriteBitmapHead(string filename, long width, long height)
        {
            bool TypeLSE;
            // File Type 설정
            string extension = Path.GetExtension(filename);
            if (string.Compare(extension, ".lse", true) == 0) TypeLSE = true;
            else TypeLSE = false;

            // Data 구조체.. 생성
            var fileHead                = new BITMAPFILEHEADER();
            var fileInfo                = new BITMAPINFOHEADER();
            var colorTable              = new COLORTABLE();
            var rasterData              = new RASTERDATA();

            fileHead.bfType             = 0x4D42;
            fileHead.bfSize             = (uint)((width * height) / 8) + 0x3E;
            fileHead.bfReserved1        = 0;
            fileHead.bfReserved2        = 0;
            fileHead.bfOffBits          = 0x3E;

            fileInfo.biSize             = 0x28;
            fileInfo.biWidth            = (uint)width;
            fileInfo.biHeight           = (uint)height;
            fileInfo.biPlanes           = 0x01;
            fileInfo.biBitCount         = 0x01;
            fileInfo.biCompression      = 0x00;
            fileInfo.biSizeImage        = (uint)((width * height) / 8);
            fileInfo.biXPelsPerMeter    = 0x00;
            fileInfo.biYPelsPerMeter    = 0x00;
            fileInfo.biClrUsed          = 0x00;
            fileInfo.biClrImportant     = 0x00;

            colorTable.rgbBlue          = 0x00;
            colorTable.rgbGreen         = 0x00;
            colorTable.rgbRed           = 0x00;
            colorTable.rgbReserved      = 0x00;

            rasterData.rgbBlue          = 0xFF;
            rasterData.rgbGreen         = 0xFF;
            rasterData.rgbRed           = 0xFF;
            rasterData.rgbReserved      = 0xFF;

            if (TypeLSE)
            {
                fileInfo.biXPelsPerMeter    = 0x0F61;
                fileInfo.biYPelsPerMeter    = 0x0F61;
                fileInfo.biClrUsed          = 0x02;
                fileInfo.biClrImportant     = 0x02;

                colorTable.rgbBlue          = 0xFF;
                colorTable.rgbGreen         = 0xFF;
                colorTable.rgbRed           = 0xFF;
                colorTable.rgbReserved      = 0x00;

                rasterData.rgbBlue          = 0x00;
                rasterData.rgbGreen         = 0x00;
                rasterData.rgbRed           = 0x00;
                rasterData.rgbReserved      = 0x00;
            }

            // Stream 방식으로 File을 저장함.
            var FS = new FileStream(filename, FileMode.Create);
            var BS = new BufferedStream(FS);

            byte[] wBytes;
            int lengthBytes;

            wBytes = StructToByte(fileHead);
            lengthBytes = wBytes.Length;
            BS.Write(wBytes, 0, lengthBytes);

            wBytes = StructToByte(fileInfo);
            lengthBytes = wBytes.Length;
            BS.Write(wBytes, 0, lengthBytes);

            wBytes = StructToByte(colorTable);
            lengthBytes = wBytes.Length;
            BS.Write(wBytes, 0, lengthBytes);

            wBytes = StructToByte(rasterData);
            lengthBytes = wBytes.Length;
            BS.Write(wBytes, 0, lengthBytes);

            BS.Close();
            FS.Close();

            return 0;

        }

        private int WriteBitmapImage(string filename, byte[] wBytes)
        {
            var FS = new FileStream(filename, FileMode.Append);
            var BS = new BufferedStream(FS);

            int lengthBytes = wBytes.Length;

            BS.Write(wBytes, 0, lengthBytes);

            BS.Close();
            FS.Close();

            return 0;
        }

        private byte[] StructToByte(object obj)
        {
            int datasize = Marshal.SizeOf(obj);             //((PACKET_DATA)obj).TotalBytes; // 구조체에 할당된 메모리의 크기를 구한다.
            IntPtr buff = Marshal.AllocHGlobal(datasize);   // 비관리 메모리 영역에 구조체 크기만큼의 메모리를 할당한다.
            Marshal.StructureToPtr(obj, buff, false);       // 할당된 구조체 객체의 주소를 구한다.
            byte[] data = new byte[datasize];               // 구조체가 복사될 배열
            Marshal.Copy(buff, data, 0, datasize);          // 구조체 객체를 배열에 복사
            Marshal.FreeHGlobal(buff);                      // 비관리 메모리 영역에 할당했던 메모리를 해제함
            return data;
        }

        private byte[] ReverseData(byte[] imageData)
        {
            int lengthData = imageData.Length;
            byte[] convertData = new byte[lengthData];

            for (int i = 0; i < lengthData; i++)
            {
                // Data가 없을 경우 Pass
                if (imageData[i] == 0) continue;
                // white-black reverse
                //convertData[i] = SubByte(0xff, imageData[i]);
                // big endian 
                convertData[i] = ReverseByte(convertData[i]);
            }

            return convertData;
        }

        private byte SubByte(byte Left, byte Right)
        {
            short num = (short)(Left - Right);
            if (num > 0xff)
            {
                return (byte)num;
            }
            return (byte)num;
        }

        // Byte의 Bit 배열 순서를 바꾼다.
        public byte ReverseByte(byte originalByte)
        {
            int result = 0;
            for (int i = 0; i < 8; i++)
            {
                result = result << 1;
                result += originalByte & 1;
                originalByte = (byte)(originalByte >> 1);
            }

            return (byte)result;
        }

        /// <summary>
        /// Manager에 저장된 Object 전체를 BMP 파일로 저장함.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int ConvertBmpFile(string filePath, double shiftLen = 0.0)
        {
            try
            {
                foreach(CMarkingObject pObject in m_ScanManager.ObjectList)
                {
                    // 생성된 BMP 파일에 Object Draw
                    DrawBmpFile(pObject, shiftLen);
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
        private int DrawObjectBytes(CMarkingObject pObject,int HeightOffset=0)
        {
            Point ptStart = new Point(0, 0);
            Point ptEnd   = new Point(0, 0);

            // Object의 위치 값을 Pixel 로 변환
            ptStart = PointToPixel(pObject.ptObjectStartPos);
            ptEnd   = PointToPixel(pObject.ptObjectEndPos);

            // Height Offset을 적용함
            ptStart.Y -= HeightOffset;
            ptEnd.Y -= HeightOffset;

            switch (pObject.ObjectType)
            {
                case EObjectType.DOT:
                    SetBytePixel(ptStart.X, ptStart.Y);
                    break;
                case EObjectType.LINE:
                    DrawBytesLine(ptStart, ptEnd);
                    break;
                case EObjectType.RECTANGLE:
                    DrawBytesLine(ptStart.X, ptStart.Y, ptEnd.X,   ptStart.Y);
                    DrawBytesLine(ptStart.X, ptStart.Y, ptStart.X, ptEnd.Y);
                    DrawBytesLine(ptStart.X, ptEnd.Y,   ptEnd.X,   ptEnd.Y);
                    DrawBytesLine(ptEnd.X,   ptStart.Y, ptEnd.X,   ptEnd.Y);
                    break;
                case EObjectType.CIRCLE:
                    DrawBytesEllipse(ptStart, ptEnd);
                    break;
                case EObjectType.GROUP:
                    CObjectGroup pGroup;
                    pGroup = (CObjectGroup)(pObject);
                    // 재귀적 방식으로  Object를 Draw를 진행함.                    
                    foreach (CMarkingObject G in pGroup.ObjectGroup)
                        DrawObjectBytes(G, HeightOffset);
                    break;
            }

            return SUCCESS;
        }

        /// <summary>
        /// 1bit BMP 파일에 Object를 Draw하는 함수
        /// </summary>
        /// <param name="pObject"></param>
        /// <returns></returns>
        private int DrawBmpFile(CMarkingObject pObject, double shiftLen = 0.0)
        {
            Point ptStart = new Point(0, 0);
            Point ptEnd = new Point(0, 0);
            CPos_XY posStart = new CPos_XY();
            CPos_XY posEnd = new CPos_XY();

            // 객체의 위치값을 읽어옴.
            posStart = pObject.ptObjectStartPos.Copy();
            posEnd   = pObject.ptObjectEndPos.Copy();

            // 시작 위치 편차를 적용 (각각의 Scanner Field의 차이를 적용함)
            posStart.dX -= shiftLen;
            posEnd.dX -= shiftLen;

            ptStart = PointToPixel(posStart);
            ptEnd = PointToPixel(posEnd);

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
                case EObjectType.CIRCLE:
                    DrawEllipse(ptStart, ptEnd);
                    break;
                case EObjectType.GROUP:
                    CObjectGroup pGroup;
                    pGroup = (CObjectGroup)(pObject);
                    // 재귀적 방식으로  Object를 Draw를 진행함.                    
                    foreach (CMarkingObject G in pGroup.ObjectGroup)
                        DrawBmpFile(G, shiftLen);
                    break;
            }         

            return SUCCESS;
        }

        private Point PointToPixel(CPos_XY pPos)
        {
            Point ptTemp = new Point(0, 0);

            ptTemp.X = (int)(pPos.dX * ratioWidth + 0.5);
            ptTemp.Y = (int)(pPos.dY * ratioHeight + 0.5);

            return ptTemp;
        }

        private int HeightToPixel(int pPos)
        {            
            return (int)(pPos * ratioHeight + 0.5);            
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

        private void SetBytePixel(int PosX, int PosY)
        {
            int iWidth = BmpImageWidth;
            int iHeight = BmpBlockHeight;

            // 위치 확인
            bool checkResion = (PosX < 0 || PosY < 0) || (PosX >= iWidth || PosY >= iHeight);
            if (checkResion) return;
                       
            // start byte address  
            int ByteAddress = PosX / BYTE_SIZE + (BmpImageWidth / BYTE_SIZE) * PosY;

            // Byte 데이터를 Bit 단위로 연산한다.
            // Black으로 표기
            //SaveObjectBytes[ByteAddress] &= (byte)~(0x80 >> (PosX % 8));
            // White으로 표기
            //SaveObjectBytes[ByteAddress] |= (byte)(0x80 >> (PosX % 8));
            SaveObjectBytes[ByteAddress] |= (byte)(0x01 << (PosX % 8));
        }


        private void DrawBytesLine(Point ptStart, Point ptEnd)
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
                        SetBytePixel(CurrentValueX, CurrentValueY);

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
                        SetBytePixel(CurrentValueX, CurrentValueY);

                    BeforeValueX = CurrentValueX;
                    BeforeValueY = CurrentValueY;
                }
                return;
            }

            ////////////////////////////////////////////////////
            // Case 4
            // 기울기가 있을 경우
            ////////////////////////////////////////////////////            

            Point ptTemp = new Point(0, 0);

            // Start가 End Point보다 클 경우 Point를 swat한다.
            if (ptStart.X > ptEnd.X)
            {
                ptTemp = ptStart;
                ptStart = ptEnd;
                ptEnd = ptTemp;
            }

            // Y축 방향으로 기울기를 구함.
            // 영상의 방향을 Y축이 반대로 되어 있음.
            float dSlope = (float)(ptEnd.Y - ptStart.Y) / (float)(ptEnd.X - ptStart.X);

            float dIncValue = Math.Abs(1 / dSlope);
            float dIncCount = Math.Abs(1 / dSlope);

            // X축 대비 Y축 증감이 1보다 크면... 1을 넣어준다.
            // (Pixel 단위로 증감을 위해서)
            if (dIncValue > 1) dIncValue = 1;

            // 시작점을 대입한다.
            float dValueX = (float)ptStart.X;
            float dValueY = (float)ptStart.Y;

            // X축의 시작점과 끝나는 점 확인 (이에 따라서.. 증감을 결정함)
            float dStartValue = (float)ptStart.X;
            float dEndValue = (float)ptEnd.X;

            for (float dX = dStartValue; dX <= dEndValue; dX = dX + dIncValue)
            {
                // 연산된 값을 Int형으로 변환 (반올림)
                CurrentValueX = (int)(dValueX + 0.5);
                CurrentValueY = (int)(dValueY + 0.5);

                // 이전 값과 현재 값을 확인함
                // 다를 경우에 해당 좌표 Bitmap Pixel값을 변경함.
                if (BeforeValueX != CurrentValueX || BeforeValueY != CurrentValueY)
                    SetBytePixel(CurrentValueX, CurrentValueY);

                // 이전값을 기억함.
                BeforeValueX = CurrentValueX;
                BeforeValueY = CurrentValueY;

                // 각 Point값을 연산함
                dValueX = dValueX + dIncValue;
                dValueY = dSlope * (dValueX - ptStart.X) + ptStart.Y;
            }
        }


        private void DrawBytesLine(int PosX1, int PosY1, int PosX2, int PosY2)
        {
            Point posLine1 = new Point(PosX1, PosY1);
            Point posLine2 = new Point(PosX2, PosY2);

            DrawBytesLine(posLine1, posLine2);
        }

        private void DrawBytesEllipse(Point ptStart, Point ptEnd)
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

                SetBytePixel((int)(dValueX1), (int)(dValueY1));
                SetBytePixel((int)(dValueX1), (int)(dValueY2));
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

                SetBytePixel((int)(dValueX1), (int)(dValueY1));
                SetBytePixel((int)(dValueX2), (int)(dValueY1));
                count++;
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

            Point ptTemp = new Point(0, 0);

            // Start가 End Point보다 클 경우 Point를 swat한다.
            if (ptStart.X > ptEnd.X)
            {
                ptTemp = ptStart;
                ptStart = ptEnd;
                ptEnd = ptTemp;
            }

            // Y축 방향으로 기울기를 구함.
            // 영상의 방향을 Y축이 반대로 되어 있음.
            float dSlope = (float)(ptEnd.Y - ptStart.Y) / (float)(ptEnd.X - ptStart.X);

            float dIncValue = Math.Abs(1 / dSlope);
            float dIncCount = Math.Abs(1 / dSlope);

            // X축 대비 Y축 증감이 1보다 크면... 1을 넣어준다.
            // (Pixel 단위로 증감을 위해서)
            if (dIncValue> 1) dIncValue = 1;

            // 시작점을 대입한다.
            float dValueX = (float)ptStart.X;
            float dValueY = (float)ptStart.Y;

            // X축의 시작점과 끝나는 점 확인 (이에 따라서.. 증감을 결정함)
            float dStartValue = (float)ptStart.X;
            float dEndValue = (float)ptEnd.X;

            for (float dX = dStartValue; dX <= dEndValue; dX = dX + dIncValue)
            {
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
        

        public bool SaveConfigPara(string strName, EScannerIndex Index = EScannerIndex.SCANNER1)
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

            int num = (int)Index;
            
            //----------------------------------------------------------------------
            section = "Job Settings";

            key = "InScanResolution";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].InScanResolution / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "CrossScanResolution";
            value = string.Format("{0:F7}", m_RefComp.DataManager.SystemData_Scan.Config[num].CrossScanResolution / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "InScanOffset";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].InScanOffset / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "StopMotorBetweenJobs";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].StopMotorBetweenJobs);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PixInvert";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].PixInvert);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "JobStartBufferTime";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].JobStartBufferTime);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PrecedingBlankLines";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].PrecedingBlankLines);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "Laser Configuration";

            key = "LaserOperationMode";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].LaserOperationMode);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "SeedClockFrequency";
            value = string.Format("{0:F0}",m_RefComp.DataManager.SystemData_Scan.Config[num].SeedClockFrequency * 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "RepetitionRate";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].RepetitionRate * 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PulsePickWidth";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].PulsePickWidth);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PixelWidth";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].PixelWidth);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "PulsePickAlgor";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].PulsePickAlgor);
            bRet = CUtils.SetValue(section, key, value, filePath);
            

            //----------------------------------------------------------------------
            section = "CrossScan Configuration";

            key = "CrossScanEncoderResol";
            value = string.Format("{0:F7}", m_RefComp.DataManager.SystemData_Scan.Config[num].CrossScanEncoderResol / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "CrossScanMaxAccel";
            value = string.Format("{0:F2}", m_RefComp.DataManager.SystemData_Scan.Config[num].CrossScanMaxAccel);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "EnCarSig";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].EnCarSig);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "SwapCarSig";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].SwapCarSig);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "Head Configuration";

            key = "SerialNumber";
            value = string.Format("{0:F7}", m_RefComp.DataManager.SystemData_Scan.Config[num].SerialNumber);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FThetaConstant";
            value = string.Format("{0:F7}", m_RefComp.DataManager.SystemData_Scan.Config[num].FThetaConstant);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "ExposeLineLength";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].ExposeLineLength / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "EncoderIndexDelay";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].EncoderIndexDelay);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay0";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay0 /1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay1";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay1 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay2";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay2 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay3";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay3 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay4";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay4 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay5";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay5 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay6";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay6 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay7";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay7 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "InterleaveRatio";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].InterleaveRatio);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay0";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay0 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay1";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay1 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay2";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay2 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay3";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay3 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay4";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay4 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay5";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay5 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay6";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay6 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "FacetFineDelay7";
            value = string.Format("{0:F6}", m_RefComp.DataManager.SystemData_Scan.Config[num].FacetFineDelay7 / 1000.0f);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "StartFacet";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].StartFacet);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "AutoIncrementStartFacet";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].AutoIncrementStartFacet);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "Polygon motor Configuration";

            //key = "InternalMotorDriverClk";
            //value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].InternalMotorDriverClk);
            //bRet = CUtils.SetValue(section, key, value, filePath);

            key = "MotorDriverType";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].MotorDriverType);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //key = "MotorSpeed";
            //value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].MotorSpeed);
            //bRet = CUtils.SetValue(section, key, value, filePath);

            //key = "SimEncSel";
            //value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].SimEncSel);
            //bRet = CUtils.SetValue(section, key, value, filePath);

            key = "MinMotorSpeed";
            value = string.Format("{0:F2}", m_RefComp.DataManager.SystemData_Scan.Config[num].MinMotorSpeed);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "MaxMotorSpeed";
            value = string.Format("{0:F2}", m_RefComp.DataManager.SystemData_Scan.Config[num].MaxMotorSpeed);
            bRet = CUtils.SetValue(section, key, value, filePath);
            

            key = "SyncWaitTime";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].SyncWaitTime);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "MotorStableTime";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].MotorStableTime);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "ShaftEncoderPulseCount";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].ShaftEncoderPulseCount);
            bRet = CUtils.SetValue(section, key, value, filePath);

            //----------------------------------------------------------------------
            section = "Other Settings";

            key = "InterruptFreq";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].InterruptFreq);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "HWDebugSelection";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].HWDebugSelection);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "ExpoDebugSelection";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].ExpoDebugSelection);
            bRet = CUtils.SetValue(section, key, value, filePath);

            key = "AutoRepeat";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].AutoRepeat);
            bRet = CUtils.SetValue(section, key, value, filePath);
            
            key = "JobstartAutorepeat";
            value = Convert.ToString(m_RefComp.DataManager.SystemData_Scan.Config[num].JobstartAutorepeat);
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

            if (File.Exists(strPath) == false) return false; 

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

            if (File.Exists(strPath) == false) return false;

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
        public bool SendBitmap(string strFile, bool IsStream=false)
        {
            string strFTP = GetControlAddress(); // ex) "92.168.22.60"
            //string strPath = string.Format("{0:s}{1:s}", m_RefComp.DataManager.DBInfo.ImageDataDir, strFile);  
            string strPath = strFile;

            if (File.Exists(strPath) == false) return false;

            if (IsStream == false)
            {
                if (SendTFTPFile(strFTP, strPath) == true) return true;
                else return false;
            }
            else
            {
                //filePath = "T:\\CadFile\\stemco_50um.bmp T:\\CadFile\\stemco_50um.lse";
                if (SendStreamFile(strFTP, strPath) == true) return true;
                else return false;
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
                //procScanner.StandardInput.Write(@"C:\\NST\\NSTC\\nstc upload " + strIP + " " + strFilePath + Environment.NewLine);
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

        private bool SendStreamFile(string strIP, string strFilePath)
        {
            string strResult = "";

            try
            {
                procScanner.Start();
                // procScanner.StandardInput.Write("C:\\NST\\nstc send " + strIP + " " + strFilePath + Environment.NewLine);
                procScanner.StandardInput.Write("c:\\nst\\nstc send " + strIP + " " + strFilePath + Environment.NewLine); // 
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
            if (processMode == EScannerMode.STEP) bufferNum = DEF_SCANNER_STILL_RUN;

            int iResult = m_RefComp.Process.LaserProcess(bufferNum);


            return iResult;
        }

        public int LaserProcessCount(int countSet)
        {
            int iResult = SUCCESS;
            // ACS 내부 Buffer변수명 지정
            string strVariable = "ProcessSet";            

            // Buffer Memory Write
            iResult = m_RefComp.Process.WriteBufferMemory(strVariable,countSet);

            return iResult;
        }

        public int LaserProcessRun()
        {
            int iResult = SUCCESS;
            // ACS 내부 Buffer변수명 지정
            string strVariable = "ProcessStop";

            // Buffer Memory Write
            iResult = m_RefComp.Process.WriteBufferMemory(strVariable, 0);

            return iResult;
        }

        public int LaserProcessStop()
        {
            int iResult = SUCCESS;
            // ACS 내부 Buffer변수명 지정
            string strVariable = "ProcessStop";

            // Buffer Memory Write
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
