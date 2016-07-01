using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;

using static LWDicer.Control.DEF_PolygonScanner;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.UI
{
    public partial class FormScannerData : Form
    {
        private CPolygonIni m_Polygon = null;

        private int scannerIndex;

        Point MousePoint;

        // 12 Inch -> 3:7  300mm Wafer UI Pixel Size X : 700, Y : 700
        // 8  Inch -> 2:5  200mm Wafer UI Pixel Size X : 500, Y : 500
        public double RATIO_12INCH = 2.3333;
        public double RATIO_8INCH = 2.5;

        public int SIZE_12INCH = 300;
        public int SIZE_8INCH = 200;

        public int SHAPE_ROUND = 0;
        public int SHAPE_SQUARE = 1;

        public float fPitch = 0;
        public float fOffsetX = 0;
        public float fOffsetY = 0;
        public int nShape = 0;
        public int nWaferSize = -1;
        public int nLineCount = 0;

        public bool bLayout;
        private CDBInfo m_DBInfo;

        public Graphics m_Grapic;
        public Image Image;

        public LineData m_CutData = new LineData();

        public FormScannerData()
        {
            InitializeComponent();

            bLayout = true;

            m_DBInfo = new CDBInfo();

            Image = new Bitmap(PicWafer.Width, PicWafer.Height);
            m_Grapic = Graphics.FromImage(Image);
            PicWafer.Image = Image;

            InitConfigureGrid();
            InitCutLineGrid();

            ClearPicture();

        }

        private void FormScannerData_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);

            ClearPicture();

            UpdateConfigureData(m_Polygon);
            UpdateImageData();

        }

        // Image Data
        public void ClearPicture()
        {
            LabelX.Hide();
            LabelY.Hide();
            PointXY.Hide();

            m_Grapic.Clear(Color.White);

            PicWafer.Refresh();
        }

        private void GridDeviceData_CellClick(object sender, Syncfusion.Windows.Forms.Grid.GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";
            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol == 0 || nRow == 0)
            {
                return;
            }

            strCurrent = GridCutLine[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridCutLine[nRow, nCol].Text = strModify;
        }

        
        private void InitCutLineGrid()
        {
            int i = 0, j = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridCutLine.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCutLine.Properties.RowHeaders = true;
            GridCutLine.Properties.ColHeaders = true;

            // Column,Row 개수
            GridCutLine.ColCount = 5;
            GridCutLine.RowCount = 0;

            // Column 가로 크기설정
            GridCutLine.ColWidths.SetSize(0, 40);
            GridCutLine.ColWidths.SetSize(1, 60);
            GridCutLine.ColWidths.SetSize(2, 60);
            GridCutLine.ColWidths.SetSize(3, 60);
            GridCutLine.ColWidths.SetSize(4, 60);
            GridCutLine.ColWidths.SetSize(5, 17);


            // Text Display
            GridCutLine[0, 0].Text = "Line";
            GridCutLine[0, 1].Text = "X1";
            GridCutLine[0, 2].Text = "Y1";
            GridCutLine[0, 3].Text = "X2";
            GridCutLine[0, 4].Text = "Y2";
            GridCutLine[0, 5].Text = "-";

            GridCutLine.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCutLine.ResizeColsBehavior = 0;
            GridCutLine.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCutLine.Refresh();
        }

        private void LabelPitch_Click(object sender, EventArgs e)
        {
            int nPitch = 0;

            string strCurrent = "", strModify = "";

            strCurrent = LabelPitch.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            LabelPitch.Text = strModify;

            fPitch = Convert.ToSingle(strModify);
        }

        public void ClearLayout()
        {
            int PicX = 0, PicY = 0;

            if (nWaferSize == SIZE_12INCH)
            {
                // 300 mm
                PicX = 20;
                PicY = 20;

                m_Grapic.DrawEllipse(Pens.White, PicX, PicY, 700, 700);

                // 왼쪽 상단 코너
                m_Grapic.DrawLine(Pens.White, PicX, PicY, PicX + 20, PicY);
                m_Grapic.DrawLine(Pens.White, PicX, PicY, PicX, PicY + 20);

                // 왼쪽 하단 코너
                m_Grapic.DrawLine(Pens.White, PicX, PicY + 680, PicX, PicY + 700);
                m_Grapic.DrawLine(Pens.White, PicX, PicY + 700, PicX + 20, PicY + 700);

                // 오른쪽 상단 코너
                m_Grapic.DrawLine(Pens.White, PicX + 680, PicY, PicX + 700, PicY);
                m_Grapic.DrawLine(Pens.White, PicX + 700, PicY, PicX + 700, PicY + 20);

                // 오른쪽 하단 코너
                m_Grapic.DrawLine(Pens.White, PicX + 680, PicY + 700, PicX + 700, PicY + 700);
                m_Grapic.DrawLine(Pens.White, PicX + 700, PicY + 700, PicX + 700, PicY + 680);
            }

            if (nWaferSize == SIZE_8INCH)
            {
                // 200 mm
                PicX = 120;
                PicY = 120;

                m_Grapic.DrawEllipse(Pens.White, PicX, PicY, 500, 500);

                // 왼쪽 상단 코너
                m_Grapic.DrawLine(Pens.White, PicX, PicY, PicX + 20, PicY);
                m_Grapic.DrawLine(Pens.White, PicX, PicY, PicX, PicY + 20);

                // 왼쪽 하단 코너
                m_Grapic.DrawLine(Pens.White, PicX, PicY + 480, PicX, PicY + 500);
                m_Grapic.DrawLine(Pens.White, PicX, PicY + 500, PicX + 20, PicY + 500);

                // 오른쪽 상단 코너
                m_Grapic.DrawLine(Pens.White, PicX + 480, PicY, PicX + 500, PicY);
                m_Grapic.DrawLine(Pens.White, PicX + 500, PicY, PicX + 500, PicY + 20);

                // 오른쪽 하단 코너
                m_Grapic.DrawLine(Pens.White, PicX + 480, PicY + 500, PicX + 500, PicY + 500);
                m_Grapic.DrawLine(Pens.White, PicX + 500, PicY + 500, PicX + 500, PicY + 480);
            }

            PicWafer.Refresh();
        }

        public void DrawCutLine(LineData CutLineData)
        {
            int i = 0;

            double dPicX1 = 0, dPicY1 = 0, dPicX2 = 0, dPicY2 = 0;

            float PicX = 0;
            float PicY = 0;

            ClearLayout();

            if (nWaferSize == SIZE_12INCH)
            {
                PicX = 20;
                PicY = 20;

                for (i = 0; i < nLineCount; i++)
                {
                    dPicX1 = (CutLineData.fLineData[i, 0] * RATIO_12INCH) + PicX;
                    dPicY1 = (CutLineData.fLineData[i, 1] * RATIO_12INCH) + PicY;
                    dPicX2 = (CutLineData.fLineData[i, 2] * RATIO_12INCH) + PicX;
                    dPicY2 = (CutLineData.fLineData[i, 3] * RATIO_12INCH) + PicY;

                    SetDrawLine(dPicX1, dPicY1, dPicX2, dPicY2, 1, Color.Black);
                }
            }

            if (nWaferSize == SIZE_8INCH)
            {
                PicX = 120;
                PicY = 120;

                for (i = 0; i < nLineCount; i++)
                {
                    dPicX1 = (CutLineData.fLineData[i, 0] * RATIO_8INCH) + PicX;
                    dPicY1 = (CutLineData.fLineData[i, 1] * RATIO_8INCH) + PicY;
                    dPicX2 = (CutLineData.fLineData[i, 2] * RATIO_8INCH) + PicX;
                    dPicY2 = (CutLineData.fLineData[i, 3] * RATIO_8INCH) + PicY;

                    SetDrawLine(dPicX1, dPicY1, dPicX2, dPicY2, 1, Color.Black);
                }
            }
        }

        public void UpdateCutLine(int nShape, int nSize)
        {
            int i = 0, j = 0;
            double X1 = 0.0, X2 = 0.0, dPitch = 0.0;
            double dA = 0.0, dB = 0.0, dSum = 0.0;

            if (nSize == SIZE_12INCH)
            {
                dPitch = fPitch;

                nLineCount = (int)(SIZE_12INCH / dPitch);

                if (nShape == SHAPE_ROUND)
                {
                    for (i = 0; i < nLineCount; i++)
                    {
                        dA = Math.Pow((nWaferSize / 2) - dPitch, 2);
                        dB = Math.Pow((nWaferSize / 2), 2);
                        dSum = dB - dA;

                        X1 = (nSize / 2) - Math.Sqrt(dSum); // X1
                        X2 = ((Math.Sqrt(dSum)) * 2) + X1;  // X2

                        m_CutData.fLineData[i, 0] = Convert.ToSingle(string.Format("{0:f4}", X1 - fOffsetX));
                        m_CutData.fLineData[i, 1] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y1
                        m_CutData.fLineData[i, 2] = Convert.ToSingle(string.Format("{0:f4}", X2 + fOffsetX));
                        m_CutData.fLineData[i, 3] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y2

                        dPitch = dPitch + fPitch;
                    }
                }

                if (nShape == SHAPE_SQUARE)
                {
                    for (i = 0; i < nLineCount; i++)
                    {
                        m_CutData.fLineData[i, 0] = Convert.ToSingle(string.Format("{0:f4}", 0 - fOffsetX));
                        m_CutData.fLineData[i, 1] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y1
                        m_CutData.fLineData[i, 2] = Convert.ToSingle(string.Format("{0:f4}", nSize + fOffsetX));
                        m_CutData.fLineData[i, 3] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y2

                        dPitch = dPitch + fPitch;
                    }
                }
            }

            if (nSize == SIZE_8INCH)
            {
                dPitch = fPitch;

                nLineCount = (int)(SIZE_8INCH / dPitch);

                if (nShape == SHAPE_ROUND)
                {

                    for (i = 0; i < nLineCount; i++)
                    {
                        dA = Math.Pow((nWaferSize / 2) - dPitch, 2);
                        dB = Math.Pow((nWaferSize / 2), 2);
                        dSum = dB - dA;

                        X1 = (nSize / 2) - Math.Sqrt(dSum); // X1
                        X2 = ((Math.Sqrt(dSum)) * 2) + X1;  // X2

                        m_CutData.fLineData[i, 0] = Convert.ToSingle(string.Format("{0:f4}", X1 - fOffsetX));
                        m_CutData.fLineData[i, 1] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y1
                        m_CutData.fLineData[i, 2] = Convert.ToSingle(string.Format("{0:f4}", X2 + fOffsetX));
                        m_CutData.fLineData[i, 3] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y2

                        dPitch = dPitch + fPitch;
                    }
                }

                if (nShape == SHAPE_SQUARE)
                {
                    for (i = 0; i < nLineCount; i++)
                    {
                        m_CutData.fLineData[i, 0] = Convert.ToSingle(string.Format("{0:f4}", 0 - fOffsetX));
                        m_CutData.fLineData[i, 1] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y1
                        m_CutData.fLineData[i, 2] = Convert.ToSingle(string.Format("{0:f4}", nSize + fOffsetX));
                        m_CutData.fLineData[i, 3] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y2

                        dPitch = dPitch + fPitch;
                    }
                }
            }

            GridCutLine.RowCount = nLineCount + 1;

            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < nLineCount + 1; j++)
                {
                    if (j == nLineCount + 1)
                    {
                        continue;
                    }

                    // Font Style - Bold
                    GridCutLine[j, 0].Font.Bold = true;
                    GridCutLine[0, i].Font.Bold = true;

                    GridCutLine[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCutLine[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (i = 0; i < nLineCount + 1; i++)
            {
                if (i == nLineCount + 1)
                {
                    continue;
                }

                GridCutLine[i + 1, 1].BackColor = Color.FromArgb(255, 230, 255);
                GridCutLine[i + 1, 2].BackColor = Color.FromArgb(255, 230, 255);
                GridCutLine[i + 1, 3].BackColor = Color.FromArgb(230, 210, 255);
                GridCutLine[i + 1, 4].BackColor = Color.FromArgb(230, 210, 255);

                GridCutLine[i + 1, 1].Text = Convert.ToString(m_CutData.fLineData[i, 0]);
                GridCutLine[i + 1, 2].Text = Convert.ToString(m_CutData.fLineData[i, 1]);
                GridCutLine[i + 1, 3].Text = Convert.ToString(m_CutData.fLineData[i, 2]);
                GridCutLine[i + 1, 4].Text = Convert.ToString(m_CutData.fLineData[i, 3]);

                GridCutLine[i + 1, 5].CellType = GridCellTypeName.CheckBox;
                GridCutLine[i + 1, 5].CheckBoxOptions = new GridCheckBoxCellInfo("True", "False", "", true);
            }
        }

        public void SetDrawLine(double X1, double Y1, double X2, double Y2, float Width, Color LineColor)
        {
            Pen m_Pen = new Pen(LineColor, Width);

            m_Grapic.DrawLine(m_Pen, (float)X1, (float)Y1, (float)X2, (float)Y2);

            PicWafer.Refresh();
        }

        private void BtnInch_Click(object sender, EventArgs e)
        {
            string strText = "";
            Button Btn = sender as Button;

            strText = Btn.Text;

            SelectInch(strText);
        }

        private void SelectInch(string strSize)
        {
            if (strSize == "8 Inch")
            {
                Btn12Inch.Image = ImagePolygon.Images[0];
                Btn8Inch.Image = ImagePolygon.Images[1];
                nWaferSize = SIZE_8INCH;
            }

            if (strSize == "12 Inch")
            {
                Btn8Inch.Image = ImagePolygon.Images[0];
                Btn12Inch.Image = ImagePolygon.Images[1];
                nWaferSize = SIZE_12INCH;
            }
        }

        private void SelectShape(int nShape)
        {
            if (nShape == SHAPE_ROUND)
            {
                BtnCycle.BackColor = Color.LightPink;
                BtnSquare.BackColor = Color.Transparent;
            }

            if (nShape == SHAPE_SQUARE)
            {
                BtnCycle.BackColor = Color.Transparent;
                BtnSquare.BackColor = Color.LightPink;
            }
        }

        private void BtnShape_Click(object sender, EventArgs e)
        {
            Button Btn = sender as Button;

            if (Btn.Name == "BtnCycle")
            {
                nShape = SHAPE_ROUND;
                SelectShape(nShape);
            }

            if (Btn.Name == "BtnSquare")
            {
                nShape = SHAPE_SQUARE;
                SelectShape(nShape);
            }

            if (nWaferSize == SIZE_12INCH)
            {
                UpdateCutLine(nShape, SIZE_12INCH);
            }

            if (nWaferSize == SIZE_8INCH)
            {
                UpdateCutLine(nShape, SIZE_8INCH);
            }
        }

        private void LabelOffsetX_Click(object sender, EventArgs e)
        {
            int nPitch = 0;

            string strCurrent = "", strModify = "";

            strCurrent = LabelOffsetX.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            LabelOffsetX.Text = strModify;
            fOffsetX = Convert.ToSingle(strModify);
        }

        private void PicWafer_MouseMove(object sender, MouseEventArgs e)
        {
            float fX = 0, fY = 0;
            int PicX = 0, PicY = 0;

            if (e.Button == MouseButtons.Left)
            {
                Graphics g = PicWafer.CreateGraphics();

                g.DrawLine(Pens.DarkBlue, MousePoint.X, MousePoint.Y, e.X, e.Y);

                MousePoint.X = e.X; MousePoint.Y = e.Y;
                g.Dispose();
            }

            if (nWaferSize == SIZE_12INCH)
            {
                PicX = 20;
                PicY = 20;

                fX = (float)((e.X - PicX) / RATIO_12INCH);
                fY = (float)((e.Y - PicY) / RATIO_12INCH);
            }

            if (nWaferSize == SIZE_8INCH)
            {
                PicX = 120;
                PicY = 120;

                fX = (float)((e.X - PicX) / RATIO_8INCH);
                fY = (float)((e.Y - PicY) / RATIO_8INCH);
            }

            PointXY.Text = string.Format("X : {0:f4}   Y : {1:f4}", fX, fY);
        }

        private void LabelOffsetY_Click(object sender, EventArgs e)
        {
            int nPitch = 0;

            string strCurrent = "", strModify = "";

            strCurrent = LabelOffsetY.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            LabelOffsetY.Text = strModify;
            fOffsetY = Convert.ToSingle(strModify);
        }

        private void PicWafer_MouseDown(object sender, MouseEventArgs e)
        {
            MousePoint.X = e.X;
            MousePoint.Y = e.Y;
        }

        private void GridCutLine_CheckBoxClick(object sender, GridCellClickEventArgs e)
        {
            int nRow = 0;

            nRow = e.RowIndex;

            float fX1 = 0, fY1 = 0, fX2 = 0, fY2 = 0, PicX = 0, PicY = 0;

            if (nRow == 0)
            {
                return;
            }

            if (nWaferSize == SIZE_12INCH)
            {
                PicX = 20;
                PicY = 20;

                fX1 = Convert.ToSingle(GridCutLine[nRow, 1].Text);
                fX1 = (float)(fX1 * RATIO_12INCH) + PicX;

                fY1 = Convert.ToSingle(GridCutLine[nRow, 2].Text);
                fY1 = (float)(fY1 * RATIO_12INCH) + PicY;

                fX2 = Convert.ToSingle(GridCutLine[nRow, 3].Text);
                fX2 = (float)(fX2 * RATIO_12INCH) + PicX;

                fY2 = Convert.ToSingle(GridCutLine[nRow, 4].Text);
                fY2 = (float)(fY2 * RATIO_12INCH) + PicY;
            }

            if (nWaferSize == SIZE_8INCH)
            {
                PicX = 120;
                PicY = 120;

                fX1 = Convert.ToSingle(GridCutLine[nRow, 1].Text);
                fX1 = (float)(fX1 * RATIO_8INCH) + PicX;

                fY1 = Convert.ToSingle(GridCutLine[nRow, 2].Text);
                fY1 = (float)(fY1 * RATIO_8INCH) + PicY;

                fX2 = Convert.ToSingle(GridCutLine[nRow, 3].Text);
                fX2 = (float)(fX2 * RATIO_8INCH) + PicX;

                fY2 = Convert.ToSingle(GridCutLine[nRow, 4].Text);
                fY2 = (float)(fY2 * RATIO_8INCH) + PicY;
            }


            if (GridCutLine[nRow, 5].CheckBoxOptions.FlatLook == true)
            {
                GridCutLine[nRow, 5].CheckBoxOptions.FlatLook = false;

                SetDrawLine(fX1, fY1, fX2, fY2, 1, Color.White);
            }
            else
            {
                GridCutLine[nRow, 5].CheckBoxOptions.FlatLook = true;

                SetDrawLine(fX1, fY1, fX2, fY2, 1, Color.Black);
            }
        }

        /*------------------------------------------------------------------------------------
        * Date : 2016.04.11
        * Author : HSLEE
        * Function : ImageCodecInfo GetEncoderInfo(string mimeType)
        * Description : Image File의 속성을 변경하기 위해 ImageCodec을 반환한다.
        * Parameter : string mimeType - Image File 형식
        * return : ImageCodecInfo codecs - 해당 Image의 codec을 반환
        ------------------------------------------------------------------------------------*/
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // 입력한 String 형식의 Codec을 받아온다.
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        public void ImageSave(string strPath)
        {
            Bitmap bmp = new Bitmap(PicWafer.Width, PicWafer.Height);
            PicWafer.DrawToBitmap(bmp, new Rectangle(0, 0, PicWafer.Width, PicWafer.Height));

            // 흑백색으로 구성된 단색 Bitmap 형식으로 변환해야함 [Scanner에서 단색 비트맵 인식]
            // BMP 파일 비트 수준 : 1
            // 1. 단색 비트맵을 저장을 위한 Bitmap 생성
            Bitmap SaveImage = new Bitmap(PicWafer.Width, PicWafer.Height, PixelFormat.Format1bppIndexed);

            // 2. 사용자가 입력한 Image Size에 해당하는 복사본을 만들위한 Rectangle 생성
            Rectangle rectangle = new Rectangle(0, 0, PicWafer.Width, PicWafer.Height);

            // 3. 원본 이미지에 단색 Bitmap 속성을 바꾼 복사본을 만든다.
            SaveImage = bmp.Clone(rectangle, PixelFormat.Format1bppIndexed);

            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 1);

            SaveImage.Save(strPath, GetEncoderInfo("image/bmp"), parameters);
        }

        public void DrawLayout()
        {
            int PicX = 0, PicY = 0;

            if (nWaferSize == SIZE_12INCH)
            {
                PicX = 20;
                PicY = 20;

                m_Grapic.DrawEllipse(Pens.Red, PicX, PicY, 700, 700);

                // 왼쪽 상단 코너
                m_Grapic.DrawLine(Pens.Red, PicX, PicY, PicX + 20, PicY);
                m_Grapic.DrawLine(Pens.Red, PicX, PicY, PicX, PicY + 20);

                // 왼쪽 하단 코너
                m_Grapic.DrawLine(Pens.Red, PicX, PicY + 680, PicX, PicY + 700);
                m_Grapic.DrawLine(Pens.Red, PicX, PicY + 700, PicX + 20, PicY + 700);

                // 오른쪽 상단 코너
                m_Grapic.DrawLine(Pens.Red, PicX + 680, PicY, PicX + 700, PicY);
                m_Grapic.DrawLine(Pens.Red, PicX + 700, PicY, PicX + 700, PicY + 20);

                // 오른쪽 하단 코너
                m_Grapic.DrawLine(Pens.Red, PicX + 680, PicY + 700, PicX + 700, PicY + 700);
                m_Grapic.DrawLine(Pens.Red, PicX + 700, PicY + 700, PicX + 700, PicY + 680);
            }

            if (nWaferSize == SIZE_8INCH)
            {
                PicX = 120;
                PicY = 120;

                m_Grapic.DrawEllipse(Pens.Red, PicX, PicY, 500, 500);

                // 왼쪽 상단 코너
                m_Grapic.DrawLine(Pens.Red, PicX, PicY, PicX + 20, PicY);
                m_Grapic.DrawLine(Pens.Red, PicX, PicY, PicX, PicY + 20);

                // 왼쪽 하단 코너
                m_Grapic.DrawLine(Pens.Red, PicX, PicY + 480, PicX, PicY + 500);
                m_Grapic.DrawLine(Pens.Red, PicX, PicY + 500, PicX + 20, PicY + 500);

                // 오른쪽 상단 코너
                m_Grapic.DrawLine(Pens.Red, PicX + 480, PicY, PicX + 500, PicY);
                m_Grapic.DrawLine(Pens.Red, PicX + 500, PicY, PicX + 500, PicY + 20);

                // 오른쪽 하단 코너
                m_Grapic.DrawLine(Pens.Red, PicX + 480, PicY + 500, PicX + 500, PicY + 500);
                m_Grapic.DrawLine(Pens.Red, PicX + 500, PicY + 500, PicX + 500, PicY + 480);
            }

            PicWafer.Refresh();

        }

        public void ShowLayout(bool bShow)
        {
            if (bShow == true)
            {
                LabelX.Show();
                LabelY.Show();
                PointXY.Show();

                DrawLayout();
            }
            else
            {
                LabelX.Hide();
                LabelY.Hide();
                PointXY.Hide();

                ClearLayout();
            }
        }

        private void BtnDraw_Click(object sender, EventArgs e)
        {
            DrawCutLine(m_CutData);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearPicture();
        }

        private void BtnLayout_Click(object sender, EventArgs e)
        {
            if (bLayout == true)
            {
                BtnLayout.Image = ImagePolygon.Images[1];
                bLayout = false;

                ShowLayout(true);
            }
            else
            {
                BtnLayout.Image = ImagePolygon.Images[0];
                bLayout = true;

                ShowLayout(false);
            }
        }

        private void BtnImageSave_Click(object sender, EventArgs e)
        {
            string strFile = "";

            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Title = "이미지 파일저장";
            dlg.OverwritePrompt = true;
            dlg.Filter = "Bitmap Image|*.bmp";

            dlg.InitialDirectory = m_DBInfo.ScannerLogDir;
            dlg.RestoreDirectory = true;

            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                strFile = dlg.FileName;
                ImageSave(strFile);
            }
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void BtnImageDataSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.DisplayMsg(12))
            {
                return;
            }

            CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.nLineCount = nLineCount;
            CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.nWaferSize = nWaferSize;

            CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fPitch = Convert.ToSingle(LabelPitch.Text);
            CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.nShape = nShape;

            CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fOffsetX = Convert.ToSingle(LabelOffsetX.Text);
            CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fOffsetY = Convert.ToSingle(LabelOffsetY.Text);

            int i = 0;

            for (i = 0; i < nLineCount; i++)
            {
                CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fLineData[i, 0] = Convert.ToSingle(GridCutLine[i + 1, 1].Text);
                CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fLineData[i, 1] = Convert.ToSingle(GridCutLine[i + 1, 2].Text);
                CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fLineData[i, 2] = Convert.ToSingle(GridCutLine[i + 1, 3].Text);
                CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fLineData[i, 3] = Convert.ToSingle(GridCutLine[i + 1, 4].Text);
            }

            CMainFrame.LWDicer.m_DataManager.SaveModelData(CMainFrame.LWDicer.m_DataManager.ModelData);

            m_CutData = CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData;

        }

        private void UpdateImageData()
        {
            fPitch = CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fPitch;
            fOffsetX = CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fOffsetX;
            fOffsetY = CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fOffsetY;
            nLineCount = CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.nLineCount;

            LabelPitch.Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fPitch);
            LabelOffsetX.Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fOffsetX);
            LabelOffsetY.Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.fOffsetY);

            if (CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.nWaferSize == SIZE_12INCH)
            {
                nWaferSize = SIZE_12INCH;
                SelectInch("12 Inch");
            }

            if (CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.nWaferSize == SIZE_8INCH)
            {
                nWaferSize = SIZE_8INCH;
                SelectInch("8 Inch");
            }

            if(CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.nShape == SHAPE_ROUND)
            {
                nShape = SHAPE_ROUND;
                SelectShape(nShape);
            }

            if (CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData.nShape == SHAPE_SQUARE)
            {
                nShape = SHAPE_SQUARE;
                SelectShape(nShape);
            }

            m_CutData = CMainFrame.LWDicer.m_DataManager.ModelData.WaferLineData;

            UpdateCutLine(nShape, nWaferSize);

            DrawCutLine(m_CutData);

        }


        // Polygon Scanner Configure Data
        private void InitConfigureGrid()
        {
            int i = 0, j = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridConfigure.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridConfigure.Properties.RowHeaders = true;
            GridConfigure.Properties.ColHeaders = true;

            // Column,Row 개수
            GridConfigure.ColCount = 3;
            GridConfigure.RowCount = 25;

            // Column 가로 크기설정
            GridConfigure.ColWidths.SetSize(0, 200);
            GridConfigure.ColWidths.SetSize(1, 80);
            GridConfigure.ColWidths.SetSize(2, 100);
            GridConfigure.ColWidths.SetSize(3, 650);

            for (i = 0; i < 26; i++)
            {
                GridConfigure.RowHeights[i] = 27;
            }

            for (i = 0; i < 25; i++)
            {
                GridConfigure[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                GridConfigure[i + 1, 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            // Text Display
            GridConfigure[0, 0].Text = "Parameter";
            GridConfigure[0, 1].Text = "Unit";
            GridConfigure[0, 2].Text = "Data";
            GridConfigure[0, 3].Text = "Description";

            GridConfigure[1, 0].Text = "InScanResolution";
            GridConfigure[2, 0].Text = "CrossScanResolution";
            GridConfigure[3, 0].Text = "InScanOffset";
            GridConfigure[4, 0].Text = "StopMotorBetweenJobs";
            GridConfigure[5, 0].Text = "PixInvert";
            GridConfigure[6, 0].Text = "JobStartBufferTime";
            GridConfigure[7, 0].Text = "PrecedingBlankLines";
            GridConfigure[8, 0].Text = "SeedClockFrequency";
            GridConfigure[9, 0].Text = "RepetitionRate";
            GridConfigure[10, 0].Text = "CrossScanEncoderResol";
            GridConfigure[11, 0].Text = "CrossScanMaxAccel";
            GridConfigure[12, 0].Text = "EnCarSig";
            GridConfigure[13, 0].Text = "SwapCarSig";
            GridConfigure[14, 0].Text = "InterLeaveRatio";
            GridConfigure[15, 0].Text = "FacetFineDelayOffset0";
            GridConfigure[16, 0].Text = "FacetFineDelayOffset1";
            GridConfigure[17, 0].Text = "FacetFineDelayOffset2";
            GridConfigure[18, 0].Text = "FacetFineDelayOffset3";
            GridConfigure[19, 0].Text = "FacetFineDelayOffset4";
            GridConfigure[20, 0].Text = "FacetFineDelayOffset5";
            GridConfigure[21, 0].Text = "FacetFineDelayOffset6";
            GridConfigure[22, 0].Text = "FacetFineDelayOffset7";
            GridConfigure[23, 0].Text = "StartFacet";
            GridConfigure[24, 0].Text = "AutoIncrementStartFacet";
            GridConfigure[25, 0].Text = "MotorStableTime";

            GridConfigure[1, 1].Text = "[u]";
            GridConfigure[2, 1].Text = "[u]";
            GridConfigure[3, 1].Text = "[u]";
            GridConfigure[4, 1].Text = "[-]";
            GridConfigure[5, 1].Text = "[-]";
            GridConfigure[6, 1].Text = "[sec]";
            GridConfigure[7, 1].Text = "[-]";
            GridConfigure[8, 1].Text = "[kHz]";
            GridConfigure[9, 1].Text = "[kHz]";
            GridConfigure[10, 1].Text = "[u]";
            GridConfigure[11, 1].Text = "[m/s^2]";
            GridConfigure[12, 1].Text = "[-]";
            GridConfigure[13, 1].Text = "[-]";
            GridConfigure[14, 1].Text = "[-]";
            GridConfigure[15, 1].Text = "[u]";
            GridConfigure[16, 1].Text = "[u]";
            GridConfigure[17, 1].Text = "[u]";
            GridConfigure[18, 1].Text = "[u]";
            GridConfigure[19, 1].Text = "[u]";
            GridConfigure[20, 1].Text = "[u]";
            GridConfigure[21, 1].Text = "[u]";
            GridConfigure[22, 1].Text = "[u]";
            GridConfigure[23, 1].Text = "[-]";
            GridConfigure[24, 1].Text = "[-]";
            GridConfigure[25, 1].Text = "[ms]";

            GridConfigure[1, 3].Text = "[Job Settings] Scanline에서 이웃한 두 pixel 사이 X축 거리";
            GridConfigure[2, 3].Text = "[Job Settings] 이웃한 두 Scanline 사이 Y축 거리";
            GridConfigure[3, 3].Text = "[Job Settings] 모든 Scanline의 X축 시작 위치를 조정";
            GridConfigure[4, 3].Text = "[Job Settings] Exposure 이후 polygonmirror 정지 여부 결정";
            GridConfigure[5, 3].Text = "[Job Settings] LaserPulse Exposure 되는 Bitmap의 색상 선택";
            GridConfigure[6, 3].Text = "[Job Settings] Bitmap Uploading 시, exposure 하기전 대기 시간";
            GridConfigure[7, 3].Text = "[Job Settings] Stage 가속시의 충분한 Settle-time을 위한 Dummy scanline 수";
            GridConfigure[8, 3].Text = "[Laser Configuration] Laser의 Seed Clock 주파수 설정";
            GridConfigure[9, 3].Text = "[Laser Configuration] 가공에 적용할 펄스 반복률(REP_RATE) 설정";
            GridConfigure[10, 3].Text = "[CrossScan Configuration] StageEncoder 분해능 값 설정";
            GridConfigure[11, 3].Text = "[CrossScan Configuration] Stagestart-up 과정의 최대 가속도";
            GridConfigure[12, 3].Text = "[CrossScan Configuration] Stage Control Encoder signal 출력 여부";
            GridConfigure[13, 3].Text = "[CrossScan Configuration] Stage movement direction 선택";
            GridConfigure[14, 3].Text = "[Head Configuration] FacetFineDelayOffset 자동 설정 기능";
            GridConfigure[15, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[16, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[17, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[18, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[19, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[20, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[21, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[22, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[23, 3].Text = "[Head Configuration] exposure의 첫 scanline에 해당하는 facet 지정";
            GridConfigure[24, 3].Text = "[Head Configuration] 새로운job started 마다 StartFacet 값 증가";
            GridConfigure[25, 3].Text = "[Polygon motor Configuration] speed-up 이후 exposure 시작 이전에 spinning 안정화 대기 시간";


            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 26; j++)
                {
                    // Font Style - Bold
                    GridConfigure[j, i].Font.Bold = true;

                    GridConfigure[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i != 3)
                    {
                        GridConfigure[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }
                }
            }

            GridConfigure.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridConfigure.ResizeColsBehavior = 0;
            GridConfigure.ResizeRowsBehavior = 0;

            GetPolygonPara(scannerIndex);

            // Grid Display Update
            GridConfigure.Refresh();
        }

        private CPolygonIni GetPolygonPara(int objIndex)
        {
            return m_Polygon = CMainFrame.LWDicer.m_Scanner[objIndex].GetPolygonPara(objIndex);
        }

        private void GridConfigure_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string strCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol != 2 || nRow == 0)
            {
                return;
            }

            strCurrent = GridConfigure[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            GridConfigure[nRow, nCol].Text = strModify;
        }

        private void LabelIP_Click(object sender, EventArgs e)
        {
            string strModify = "";

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify))
            {
                return;
            }

            LabelIP.Text = strModify;

            CMainFrame.LWDicer.m_DataManager.SystemData_Scanner.Scanner[scannerIndex].strIP = LabelIP.Text;

        }

        private void LabelPort_Click(object sender, EventArgs e)
        {
            string strCurrent = "", strModify = "";

            strCurrent = LabelPort.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(strCurrent, out strModify))
            {
                return;
            }

            LabelPort.Text = strModify;

            CMainFrame.LWDicer.m_DataManager.SystemData_Scanner.Scanner[scannerIndex].strPort = LabelPort.Text;
        }

        private void BtnConfigSave_Click(object sender, EventArgs e)
        {

            if (!CMainFrame.DisplayMsg(13))
            {
                return;
            }

            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetPixelGridX(scannerIndex, Convert.ToDouble(GridConfigure[1, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetPixelGridY(scannerIndex, Convert.ToDouble(GridConfigure[2, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetStartOffset(scannerIndex, Convert.ToDouble(GridConfigure[3, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetMotorBetweenJob(scannerIndex, Convert.ToInt16(GridConfigure[4, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetBitMapColor(scannerIndex, Convert.ToInt16(GridConfigure[5, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetBufferTime(scannerIndex, Convert.ToInt16(GridConfigure[6, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetDummyBlankLine(scannerIndex, Convert.ToInt16(GridConfigure[7, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSeedClock(scannerIndex, Convert.ToDouble(GridConfigure[8, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetRepRate(scannerIndex, Convert.ToDouble(GridConfigure[9, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetEncoderResol(scannerIndex, Convert.ToDouble(GridConfigure[10, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetMaxAccel(scannerIndex, Convert.ToDouble(GridConfigure[11, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetEnCarSig(scannerIndex, Convert.ToInt16(GridConfigure[12, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSwapCarSig(scannerIndex, Convert.ToInt16(GridConfigure[13, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetLeaveRatio(scannerIndex, Convert.ToInt16(GridConfigure[14, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[15, 2].Text), Facet0);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[16, 2].Text), Facet1);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[17, 2].Text), Facet2);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[18, 2].Text), Facet3);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[19, 2].Text), Facet4);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[20, 2].Text), Facet5);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[21, 2].Text), Facet6);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[22, 2].Text), Facet7);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetStartFacet(scannerIndex, Convert.ToInt16(GridConfigure[23, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetAutoIncStartFacet(scannerIndex, Convert.ToInt16(GridConfigure[24, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetMotorStableTime(scannerIndex, Convert.ToInt16(GridConfigure[25, 2].Text));

            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetScannerIP(scannerIndex, LabelIP.Text);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetScannerPort(scannerIndex, LabelPort.Text);

            CMainFrame.LWDicer.m_DataManager.SystemData_Scanner.Scanner[scannerIndex] = GetPolygonPara(scannerIndex);

            CMainFrame.LWDicer.m_Scanner[scannerIndex].SavePolygonPara(CMainFrame.LWDicer.m_DataManager.SystemData_Scanner.Scanner[scannerIndex], "config");

            UpdateConfigureData(m_Polygon);
        }

        private void UpdateConfigureData(CPolygonIni m_PolygonPara)
        {
            // User Enable Para
            GridConfigure[1, 2].Text = string.Format("{0:f}", m_PolygonPara.InScanResolution * 1000000);
            GridConfigure[2, 2].Text = string.Format("{0:f}", m_PolygonPara.CrossScanResolution * 1000000);
            GridConfigure[3, 2].Text = string.Format("{0:f}", m_PolygonPara.InScanOffset * 1000000);
            GridConfigure[4, 2].Text = Convert.ToString(m_PolygonPara.StopMotorBetweenJobs);
            GridConfigure[5, 2].Text = Convert.ToString(m_PolygonPara.PixInvert);
            GridConfigure[6, 2].Text = Convert.ToString(m_PolygonPara.JobStartBufferTime);
            GridConfigure[7, 2].Text = Convert.ToString(m_PolygonPara.PrecedingBlankLines);

            GridConfigure[8, 2].Text = string.Format("{0:f}", m_PolygonPara.SeedClockFrequency / 1000);
            GridConfigure[9, 2].Text = string.Format("{0:f}", m_PolygonPara.RepetitionRate / 1000);

            GridConfigure[10, 2].Text = string.Format("{0:f}", m_PolygonPara.CrossScanEncoderResol * 1000000);

            GridConfigure[11, 2].Text = Convert.ToString(m_PolygonPara.CrossScanMaxAccel);
            GridConfigure[12, 2].Text = Convert.ToString(m_PolygonPara.EnCarSig);
            GridConfigure[13, 2].Text = Convert.ToString(m_PolygonPara.SwapCarSig);
            GridConfigure[14, 2].Text = Convert.ToString(m_PolygonPara.InterleaveRatio);
            GridConfigure[15, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset0 * 1000000);
            GridConfigure[16, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset1 * 1000000);
            GridConfigure[17, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset2 * 1000000);
            GridConfigure[18, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset3 * 1000000);
            GridConfigure[19, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset4 * 1000000);
            GridConfigure[20, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset5 * 1000000);
            GridConfigure[21, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset6 * 1000000);
            GridConfigure[22, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset7 * 1000000);
            GridConfigure[23, 2].Text = Convert.ToString(m_PolygonPara.StartFacet);
            GridConfigure[24, 2].Text = Convert.ToString(m_PolygonPara.AutoIncrementStartFacet);
            GridConfigure[25, 2].Text = Convert.ToString(m_PolygonPara.MotorStableTime);
        }

        private void BtnImageCreate_Click(object sender, EventArgs e)
        {
            string strImage, strIP, strFilePath;

            int i = 0, nSize = 0, nYPitch = 1;

            for (i = 0; i < 20; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                nSize = 32 * i;

                CMainFrame.LWDicer.m_Scanner[0].SetPicSize(nSize, nSize);

                CMainFrame.LWDicer.m_Scanner[0].SetDrawLine(0, nYPitch, nSize, nYPitch, 5);

                strImage = string.Format("ScannerImage_{0:d}", i);

                CMainFrame.LWDicer.m_Scanner[0].SaveImage(strImage);

                nYPitch++;

                strIP = "192.168.22.123";

                strFilePath = string.Format("T:\\SFA\\LWDicer\\ScannerLog\\{0:s}.bmp", strImage);

                CMainFrame.LWDicer.m_Scanner[0].SendTFTPFile(strIP, strFilePath);
            }
        }
    }
}
