using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LWDicer.Control;

using static LWDicer.Control.DEF_Common;

using static LWDicer.Control.MDataManager;

using static LWDicer.Control.DEF_DataManager;

using static LWDicer.Control.DEF_Error;

namespace LWDicer.UI
{
    public partial class FormAlarmDisplay : Form
    {

        enum EAlarmUnit
        {
            NONE = -1,
            SYSTEM,
            LOADER,
            PUSHPULL,
            LO_HANDLER,
            UP_HANDLER,
            SPINNER1,
            SPINNER2,
            STAGE,
            SCANNER,
            LASER,
            MAX,
        }

        public Graphics m_Grapic;
        public Image Image;

        private int nAlarmCode;

        private bool bToggle;

        private float fXPos, fYPos;

        Button[] BtnLanguage = new Button[(int)ELanguage.MAX];

        public FormAlarmDisplay()
        {
            InitializeComponent();

            this.DesktopLocation = new Point(110, 196);

            ResouceMapping();
                       
            TmrAlarm.Enabled = true;
            TmrAlarm.Interval = 500;
        }

        public void SetAlarmCode(int nCode)
        {
            nAlarmCode = nCode;
        }

        public int GetAlarmCode()
        {
            return nAlarmCode;
        }

        private void FormClose()
        {
            TmrAlarm.Stop();
            TmrAlarm.Enabled = false;

            this.Hide();
        }

        private void ResouceMapping()
        {
            BtnLanguage[0] = BtnKor; BtnLanguage[1] = BtnEng; BtnLanguage[2] = BtnChn; BtnLanguage[3] = BtnJpn;
        }

        private void BtnBuzzerOff_Click(object sender, EventArgs e)
        {

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {

        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormAlarmDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void BtnLanguage_Click(object sender, EventArgs e)
        {
            Button BtnLanguage = sender as Button;

            switch(BtnLanguage.Text)
            {
                case "KOR":
                    UpdateAlarmText(ELanguage.KOREAN, GetAlarmCode());
                    break;
                case "ENG":
                    UpdateAlarmText(ELanguage.ENGLISH, GetAlarmCode());
                    break;
                case "CHN":
                    UpdateAlarmText(ELanguage.CHINESE, GetAlarmCode());
                    break;
                case "JPN":
                    UpdateAlarmText(ELanguage.JAPANESE, GetAlarmCode());
                    break;
            }
        }

        private void DrawAlarmPos(Color LineColor, float fX, float fY)
        {
            Pen m_Pen = new Pen(LineColor, 5);

            Image = new Bitmap(PicAlarmPos.Width, PicAlarmPos.Height);
            m_Grapic = Graphics.FromImage(Image);
            PicAlarmPos.Image = Image;

            m_Grapic.DrawEllipse(m_Pen, fX, fY, 40, 40);
        }


        private void UpdateAlarmText(ELanguage Language, int nCode)
        {
            string strAlarmText, strTroubles, strAlarmCode;

            int i;

            List<CAlarmInfo> AlarmInfo = CMainFrame.LWDicer.m_DataManager.AlarmInfoList;

            for(i=0;i<(int)ELanguage.MAX;i++)
            {
                BtnLanguage[i].BackColor = Color.LightSlateGray;
            }

            foreach(CAlarmInfo info in AlarmInfo)
            {
                if(info.Index == GetAlarmCode())
                {
                    switch (Language)
                    {
                        case ELanguage.KOREAN:
                            LabelAlarmText.Text = info.Description[(int)ELanguage.KOREAN];
                            LabelTrouble.Text = info.Solution[(int)ELanguage.KOREAN];
                            BtnLanguage[(int)ELanguage.KOREAN].BackColor = Color.Tan;
                            break;

                        case ELanguage.ENGLISH:
                            LabelAlarmText.Text = info.Description[(int)ELanguage.ENGLISH];
                            LabelTrouble.Text = info.Solution[(int)ELanguage.ENGLISH];
                            BtnLanguage[(int)ELanguage.ENGLISH].BackColor = Color.Tan;
                            break;

                        case ELanguage.CHINESE:
                            LabelAlarmText.Text = info.Description[(int)ELanguage.CHINESE];
                            LabelTrouble.Text = info.Solution[(int)ELanguage.CHINESE];
                            BtnLanguage[(int)ELanguage.CHINESE].BackColor = Color.Tan;
                            break;

                        case ELanguage.JAPANESE:
                            LabelAlarmText.Text = info.Description[(int)ELanguage.JAPANESE];
                            LabelTrouble.Text = info.Solution[(int)ELanguage.JAPANESE];
                            BtnLanguage[(int)ELanguage.JAPANESE].BackColor = Color.Tan;
                            break;
                    }

                    strAlarmCode = "Alarm Code : " + Convert.ToString(info.Index);
                    LabelAlarmCode.Text = strAlarmCode;

                    LabelTrouble.TextAlign = ContentAlignment.TopLeft;

                    PicAlarmPos.BackgroundImageLayout = ImageLayout.Center;


                    //Alarm 위치를 위한 Base Image
                    switch (GetAlarmUnit(GetAlarmCode()))
                    {
                        case (int)EAlarmUnit.SYSTEM:
                            fXPos = 10; fYPos = 10;
                            PicAlarmPos.BackgroundImage = null;
                            break;
                        case (int)EAlarmUnit.LOADER:
                            fXPos = 345; fYPos = 330;
                            PicAlarmPos.BackgroundImage = Properties.Resources.Loader;
                            break;
                        case (int)EAlarmUnit.PUSHPULL:
                            fXPos = 353; fYPos = 145;
                            PicAlarmPos.BackgroundImage = Properties.Resources.PushPull;
                            break;
                        case (int)EAlarmUnit.LO_HANDLER:
                            fXPos = 370; fYPos = 340;
                            PicAlarmPos.BackgroundImage = Properties.Resources.Handler;
                            break;
                        case (int)EAlarmUnit.UP_HANDLER:
                            fXPos = 100; fYPos = 185;
                            PicAlarmPos.BackgroundImage = Properties.Resources.Handler;
                            break;
                        case (int)EAlarmUnit.SPINNER1:
                            fXPos = 210; fYPos = 320;
                            PicAlarmPos.BackgroundImage = Properties.Resources.Spinner;
                            break;
                        case (int)EAlarmUnit.SPINNER2:
                            fXPos = 325; fYPos = 365;
                            PicAlarmPos.BackgroundImage = Properties.Resources.Spinner;
                            break;
                        case (int)EAlarmUnit.STAGE:
                            fXPos = 290; fYPos = 280; 
                            PicAlarmPos.BackgroundImage = Properties.Resources.LaserScanner;
                            break;
                        case (int)EAlarmUnit.SCANNER:
                            fXPos = 290; fYPos = 220; 
                            PicAlarmPos.BackgroundImage = Properties.Resources.LaserScanner;
                            break;
                        case (int)EAlarmUnit.LASER:
                            fXPos = 220; fYPos = 130; 
                            PicAlarmPos.BackgroundImage = Properties.Resources.LaserScanner;
                            break;
                    }

                    break;
                }
            }
        }

        private int GetAlarmUnit(int nCodeNo)
        {
            // Alarm 영역
            if (nCodeNo >= 0 && nCodeNo <= 99) return (int)EAlarmUnit.SYSTEM;
            if (nCodeNo >= 100 && nCodeNo <= 199) return (int)EAlarmUnit.LOADER;
            if (nCodeNo >= 200 && nCodeNo <= 299) return (int)EAlarmUnit.PUSHPULL;
            if (nCodeNo >= 300 && nCodeNo <= 399) return (int)EAlarmUnit.LO_HANDLER;
            if (nCodeNo >= 400 && nCodeNo <= 499) return (int)EAlarmUnit.UP_HANDLER;
            if (nCodeNo >= 500 && nCodeNo <= 599) return (int)EAlarmUnit.SPINNER1;
            if (nCodeNo >= 600 && nCodeNo <= 699) return (int)EAlarmUnit.SPINNER2;
            if (nCodeNo >= 700 && nCodeNo <= 799) return (int)EAlarmUnit.STAGE;
            if (nCodeNo >= 800 && nCodeNo <= 899) return (int)EAlarmUnit.SCANNER;
            if (nCodeNo >= 900 && nCodeNo <= 1000) return (int)EAlarmUnit.LASER;

            return (int)EAlarmUnit.NONE;
        }

        private void FormAlarmDisplay_Load(object sender, EventArgs e)
        {
            switch (CMainFrame.LWDicer.m_DataManager.SystemData.Language)
            {
                case ELanguage.KOREAN:
                    UpdateAlarmText((int)ELanguage.KOREAN, GetAlarmCode());
                    break;
                case ELanguage.ENGLISH:
                    UpdateAlarmText(ELanguage.ENGLISH, GetAlarmCode());
                    break;
                case ELanguage.CHINESE:
                    UpdateAlarmText(ELanguage.CHINESE, GetAlarmCode());
                    break;
                case ELanguage.JAPANESE:
                    UpdateAlarmText(ELanguage.JAPANESE, GetAlarmCode());
                    break;
            }

            TmrAlarm.Start();
        }

        private void TmrAlarm_Tick(object sender, EventArgs e)
        {
            if (bToggle)
            {
                bToggle = false;
                DrawAlarmPos(Color.DarkRed, fXPos, fYPos);
            }
            else
            {
                bToggle = true;
                DrawAlarmPos(Color.LightBlue, fXPos, fYPos);
            }
        }
    }
}
