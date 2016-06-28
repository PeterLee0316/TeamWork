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
        public Graphics m_Grapic;
        public Image Image;

        private int AlarmCode;
        private EAlarmGroup AlarmGroup;

        private bool bToggle;

        private int dXPos, dYPos;

        Point MousePoint;

        public FormAlarmDisplay(int AlarmCode, EAlarmGroup AlarmGroup)
        {
            InitializeComponent();

            this.AlarmCode = AlarmCode;
            this.AlarmGroup = AlarmGroup;

            this.DesktopLocation = new Point(110, 196);

            TmrAlarm.Enabled = true;
            TmrAlarm.Interval = 500;
        }

        private void FormClose()
        {
            TmrAlarm.Stop();
            TmrAlarm.Enabled = false;

            this.Hide();
        }

        private void BtnBuzzerOff_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_OpPanel.SetBuzzerStatus(false);

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

        //private void BtnLanguage_Click(object sender, EventArgs e)
        //{
        //    Button BtnLanguage = sender as Button;

        //    switch(BtnLanguage.Text)
        //    {
        //        case "KOR":
        //            UpdateAlarmText(ELanguage.KOREAN, AlarmCode);
        //            break;
        //        case "ENG":
        //            UpdateAlarmText(ELanguage.ENGLISH, AlarmCode);
        //            break;
        //        case "CHN":
        //            UpdateAlarmText(ELanguage.CHINESE, AlarmCode);
        //            break;
        //        case "JPN":
        //            UpdateAlarmText(ELanguage.JAPANESE, AlarmCode);
        //            break;
        //    }
        //}

        private void DrawAlarmPos(Color LineColor, float fX, float fY)
        {
            Pen m_Pen = new Pen(LineColor, 5);

            Image = new Bitmap(PicAlarmPos.Width, PicAlarmPos.Height);
            m_Grapic = Graphics.FromImage(Image);
            PicAlarmPos.Image = Image;

            m_Grapic.DrawEllipse(m_Pen, fX-20, fY-20, 40, 40);
        }


        private void UpdateAlarmText(ELanguage Language, int nCode)
        {
            string strAlarmText, strTroubles, strAlarmCode, strESC;

            int i;

            List<CAlarmInfo> AlarmInfo = CMainFrame.LWDicer.m_DataManager.AlarmInfoList;

            foreach(CAlarmInfo info in AlarmInfo)
            {
                if(info.Index == AlarmCode)
                {
                    LabelAlarmText1.Text = info.Description[(int)ELanguage.ENGLISH];
                    LabelTrouble1.Text = info.Solution[(int)ELanguage.ENGLISH];

                    LabelAlarmText2.Text = info.Description[(int)Language];
                    LabelTrouble2.Text = info.Solution[(int)Language];

                    strAlarmCode = "Alarm Code : " + Convert.ToString(info.Index);
                    LabelAlarmCode.Text = strAlarmCode;

                    LabelTrouble1.TextAlign = ContentAlignment.TopLeft;
                    LabelTrouble2.TextAlign = ContentAlignment.TopLeft;

                    PicAlarmPos.BackgroundImageLayout = ImageLayout.Center;

                    strESC = info.Esc;

                    strESC = strESC.Replace("X:","");
                    strESC = strESC.Replace("Y:", "");

                    // Data를 ',' 구분하여 잘라냄
                    string[] strPos = strESC.Split(',');

                    dXPos = Convert.ToInt16(strPos[0]);
                    dYPos = Convert.ToInt16(strPos[1]);

                    //Alarm 위치를 위한 Base Image
                    switch (AlarmGroup)
                    {
                        case EAlarmGroup.SYSTEM:
                            PicAlarmPos.BackgroundImage = null;
                            break;
                        case EAlarmGroup.LOADER:
                            PicAlarmPos.BackgroundImage = Properties.Resources.Loader;
                            break;
                        case EAlarmGroup.PUSHPULL:
                            PicAlarmPos.BackgroundImage = Properties.Resources.PushPull;
                            break;
                        case EAlarmGroup.HANDLER:
                            PicAlarmPos.BackgroundImage = Properties.Resources.Handler;
                            break;
                        case EAlarmGroup.SPINNER:
                            PicAlarmPos.BackgroundImage = Properties.Resources.Spinner;
                            break;
                        case EAlarmGroup.STAGE:
                            PicAlarmPos.BackgroundImage = Properties.Resources.LaserScanner;
                            break;
                        case EAlarmGroup.SCANNER:
                            PicAlarmPos.BackgroundImage = Properties.Resources.LaserScanner;
                            break;
                        case EAlarmGroup.LASER:
                            PicAlarmPos.BackgroundImage = Properties.Resources.LaserScanner;
                            break;
                    }

                    break;
                }
            }
        }

        private void FormAlarmDisplay_Load(object sender, EventArgs e)
        {
            UpdateAlarmText(CMainFrame.LWDicer.m_DataManager.SystemData.Language, AlarmCode);

            TmrAlarm.Start();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if(!CMainFrame.LWDicer.DisplayMsg("", "Alarm 내용을 수정을 하시겠습니까?"))
            {
                return;
            }

            string strAlarm, strTrouble;

            FormAlarmEdit dlg = new FormAlarmEdit();
            dlg.SetAlarmText(LabelAlarmText1.Text, LabelAlarmText2.Text, LabelTrouble1.Text, LabelTrouble2.Text);
            dlg.ShowDialog();

            if(dlg.DialogResult != DialogResult.OK)
            {
                return;
            }

            if(CMainFrame.LWDicer.m_DataManager.SystemData.Language == ELanguage.ENGLISH)
            {
                LabelAlarmText1.Text = dlg.strAlarm_Eng;
                LabelTrouble1.Text = dlg.strTrouble_Eng;

                LabelAlarmText2.Text = dlg.strAlarm_Eng;
                LabelTrouble2.Text = dlg.strTrouble_Eng;
            } else
            {
                LabelAlarmText1.Text = dlg.strAlarm_Eng;
                LabelTrouble1.Text = dlg.strTrouble_Eng;

                LabelAlarmText2.Text = dlg.strAlarm_System;
                LabelTrouble2.Text = dlg.strTrouble_System;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("", "수정된 Alarm 내용을 저장 하시겠습니까?"))
            {
                return;
            }

            string strPos;

            List<CAlarmInfo> AlarmInfo = CMainFrame.LWDicer.m_DataManager.AlarmInfoList;

            foreach (CAlarmInfo info in AlarmInfo)
            {
                if (info.Group == AlarmGroup)
                {
                    if (info.Index == AlarmCode)
                    {
                        strPos = string.Format("X:{0:d},Y:{1:d}", dXPos, dYPos);
                        info.Esc = strPos;

                        info.Description[(int)ELanguage.ENGLISH] = LabelAlarmText1.Text;
                        info.Solution[(int)ELanguage.ENGLISH] = LabelTrouble1.Text;

                        info.Description[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language] = LabelAlarmText2.Text;
                        info.Solution[(int)CMainFrame.LWDicer.m_DataManager.SystemData.Language] = LabelTrouble2.Text;

                        CMainFrame.LWDicer.m_DataManager.AlarmInfoList.RemoveAt(info.Index);
                        CMainFrame.LWDicer.m_DataManager.AlarmInfoList.Insert(info.Index, info);

                        break;
                    }
                }
            }

            CMainFrame.LWDicer.m_DataManager.SaveAlarmInfoList();
        }

        private void PicAlarmPos_MouseClick(object sender, MouseEventArgs e)
        {
            dXPos = e.X;
            dYPos = e.Y;
        }

        private void TmrAlarm_Tick(object sender, EventArgs e)
        {
            if (bToggle)
            {
                bToggle = false;
                DrawAlarmPos(Color.DarkRed, dXPos, dYPos);
            }
            else
            {
                bToggle = true;
                DrawAlarmPos(Color.LightBlue, dXPos, dYPos);
            }
        }
    }
}
