﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;

using static LWDicer.Control.DEF_PolygonScanner;

namespace LWDicer.Control
{
    public class DEF_PolygonScanner
    {
        public const int BLACK = 0;  //0 = Black에서Laser On
        public const int WHITE = 1;  //1 = White 에서Laser On

        public const int SPIN = 0;  // 계속 spinning 유지
        public const int STOP = 1;  // 정지 

        public const int RATIO_0 = 0;
        public const int RATIO_2 = 2;
        public const int RATIO_4 = 4;
        public const int RATIO_8 = 8;

        public const int Facet0 = 0;
        public const int Facet1 = 1;
        public const int Facet2 = 2;
        public const int Facet3 = 3;
        public const int Facet4 = 4;
        public const int Facet5 = 5;
        public const int Facet6 = 6;
        public const int Facet7 = 7;

        public enum LaserOP
        {
            SuperSync=0,        // SuperSync 동작
            NotUsed,            // Not-used
            PulseOut,           // PULSE_OUT 출력만사용, SuperSync 중지
            NoneSeedClock,      // Trumph TruMicro Laser 전용(SEED CLOCK 미사용)
            InternalSeedClock   // AVIA 등나노초레이저용(내부SEED CLOCK 사용)
        };


        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(
        String section,
        String key,
        String def,
        StringBuilder retVal,
        int size,
        String filePath);

        [DllImport("kernel32.dll")]
        public static extern bool WritePrivateProfileString(
            String section,
            String key,
            String val,
            String filePath);

        public class CPolygonScannerData
        {
            public CPolygonScannerData()
            {

            }
        }

        public class CPolygonIni
        {
            public string strIP;
            public string strPort;

            /* [Job Settings] */
            public double InScanResolution;     // USER ENABLE
            public double CrossScanResolution;  // USER ENABLE
            public double InScanOffset;         // USER ENABLE
            public int StopMotorBetweenJobs;    // USER ENABLE
            public int PixInvert;               // USER ENABLE
            public int JobStartBufferTime;      // USER ENABLE
            public int PrecedingBlankLines;     // USER ENABLE

            /* [Laser Configuration] */
            public int LaserOperationMode;         // USER ENABLE 
            public double SeedClockFrequency;      // USER ENABLE
            public double RepetitionRate;          // USER ENABLE
            public int PulsePickWidth;             // Seed Clock과 Rep Rate값이 변하면 Width 변경해야함
            public int PixelWidth;                 // Seed Clock과 Rep Rate값이 변하면 Width 변경해야함
            public int PulsePickAlgor;
            public int UseCoaxIf;

            /* [CrossScan Configuration] */
            public double CrossScanEncoderResol;    // USER ENABLE
            public double CrossScanMaxAccel;        // USER ENABLE  
            public int EnCarSig;                    // USER ENABLE
            public int SwapCarSig;                  // USER ENABLE

            /* [Head Configuration] */
            public double SerialNumber;
            public double FThetaConstant;
            public double ExposeLineLength;
            public int EncoderIndexDelay;
            public double FacetFineDelay0;
            public double FacetFineDelay1;
            public double FacetFineDelay2;
            public double FacetFineDelay3;
            public double FacetFineDelay4;
            public double FacetFineDelay5;
            public double FacetFineDelay6;
            public double FacetFineDelay7;
            public int InterleaveRatio;         // USER ENABLE
            public double FacetFineDelayOffset0;// USER ENABLE
            public double FacetFineDelayOffset1;// USER ENABLE
            public double FacetFineDelayOffset2;// USER ENABLE
            public double FacetFineDelayOffset3;// USER ENABLE
            public double FacetFineDelayOffset4;// USER ENABLE
            public double FacetFineDelayOffset5;// USER ENABLE
            public double FacetFineDelayOffset6;// USER ENABLE
            public double FacetFineDelayOffset7;// USER ENABLE
            public int StartFacet;              // USER ENABLE
            public int AutoIncrementStartFacet; // USER ENABLE

            /* [Polygon motor Configuration] */
            public int InternalMotorDriverClk;
            public int MotorDriverType;
            public int MotorSpeed;
            public int SimEncSel;
            public double MinMotorSpeed;
            public double MaxMotorSpeed;
            public int MotorEffectivePoles;
            public int SyncWaitTime;
            public int MotorStableTime;         // USER ENABLE

            /* [Other Settings] */
            public int InterruptFreq;
            public int HWDebugSelection;
            public int AutoRepeat;
            public int PixAlwaysOn;
            public int ExtCamTrig;
            public int EncoderExpo;
            public int FacetTest;
            public int SWTest;
            public int JobstartAutorepeat;
        }

        public class LineData
        {
            public int nLineCount;

            public float[,] fLineData = new float[500, 4];
                   
            public int nWaferSize;
                   
            public float fPitch;

            public int nShape;

            public float fOffsetX;

            public float fOffsetY;
        }

    }
    public interface IPolygonScanner
    {
        // LSE Controller Serial Interface  Function
        int GetSerialData(out string Message);

        // ini File 처리 Function
        bool LoadPolygonPara(int objIndex, CPolygonIni PolygonPara);
        bool SavePolygonPara(CPolygonIni m_PolygonData, string strFile);

        // LSE Controller FTP Interface Function
        bool SendConfig(int objIndex, string strFile);
        bool SendBitMap(int objIndex, string strFile);
        void SetScannerIP(int objIndex, string strIP);
        void SetScannerPort(int objIndex, string strPort);
        string GetIPData(int objIndex);

        // LSE Controller Parameter Function
        CPolygonIni GetPolygonPara(int objIndex);
        void SetPixelGridX(int objIndex, double pX);
        void SetPixelGridY(int objIndex, double pY);
        void SetBitMapColor(int objIndex, int nColor);
        void SetSuperSync(int objIndex, double Offset, int nNo);
        void SetStartOffset(int objIndex, double Offset);
        void SetSeedClock(int objIndex, double Frequency);
        void SetRepRate(int objIndex, double Frequency);
        void SetPixelWidth(int objIndex, double SeedClock, double RepRate);
        void SetLaserOP(int objIndex, int nOption);
        void SetBufferTime(int objIndex, int nSec);
        void SetDummyBlankLine(int objIndex, int nScanLine);
        void SetMotorBetweenJob(int objIndex, int nOption);
        void SetMotorStableTime(int objIndex, int nTime);
        void SetLeaveRatio(int objIndex, int nRatio);
        void SetEncoderResol(int objIndex, double dResol);
        void SetMaxAccel(int objIndex, double dAcc);
        void SetEnCarSig(int objIndex, int nSig);
        void SetSwapCarSig(int objIndex, int nSig);
        void SetStartFacet(int objIndex, int nFaceTNo);
        void SetAutoIncStartFacet(int objIndex, int nSig);

        // Image 생성을 위한 Function
        void SetPicSize(int nX, int nY);
        void SetDrawLine(float X1, float Y1, float X2, float Y2, float Width);
        void SaveImage(string strBMP);
        void DrawRound(LineData Data);
        void DrawSquare(LineData Data);

        int LSEPortOpen();
        int LSEPortClose();

        bool SendTFTPFile(string strIP, string strFilePath);
    }
}
