using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Layers;
using static Core.Layers.DEF_Common;
using static Core.Layers.MDataManager;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_Error;

namespace Core.UI
{
    public partial class FormAlarmDisplay : Form
    {
        private CAlarm Alarm;

        private bool bToggle;
        private int dXPos, dYPos;

        private bool IsEdited = false;


        public FormAlarmDisplay(CAlarm Alarm)
        {
            InitializeComponent();

            this.Alarm = Alarm;

            this.DesktopLocation = new Point(110, 196);

            TmrAlarm.Enabled = true;
            TmrAlarm.Interval = 500;
            CMainFrame.IsAlarmPopup = true;
        }

        private void BtnBuzzerOff_Click(object sender, EventArgs e)
        {
            CMainFrame.mCore.m_OpPanel.SetBuzzerStatus(false);

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {

        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if(IsEdited == true)
            {
                if (!CMainFrame.InquireMsg("Alarm data is not saved. Continue?"))
                {
                    return;
                }

            }

            this.Close();
        }

        private void FormAlarmDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            CMainFrame.IsAlarmPopup = false;
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
            Pen pen = new Pen(LineColor, 5);
            PicAlarmPos.Image = new Bitmap(PicAlarmPos.Width, PicAlarmPos.Height);
            Graphics graphic = Graphics.FromImage(PicAlarmPos.Image);

            graphic.DrawEllipse(pen, fX-20, fY-20, 40, 40);
        }


        private void UpdateAlarmText(ELanguage language)
        {
            TextProcessName.Text = String.Format($"[{Alarm.ProcessType}] {Alarm.ProcessName}");
            TextObjectName.Text  = String.Format($"[{Alarm.ObjectType}] {Alarm.ObjectName}");

            LabelAlarmText1.Text = Alarm.Info.Description[(int)ELanguage.ENGLISH];
            LabelTrouble1.Text   = Alarm.Info.Solution[(int)ELanguage.ENGLISH];

            LabelAlarmText2.Text = Alarm.Info.Description[(int)language];
            LabelTrouble2.Text   = Alarm.Info.Solution[(int)language];

            string str = "Alarm Code : " + Alarm.GetIndex();
            LabelAlarmCode.Text = str;

            LabelTrouble1.TextAlign = ContentAlignment.TopLeft;
            LabelTrouble2.TextAlign = ContentAlignment.TopLeft;

            PicAlarmPos.BackgroundImageLayout = ImageLayout.Center;

            str = Alarm.Info.Esc;
            if(str != null)
            {
                str = str.Replace("X:", "");
                str = str.Replace("Y:", "");

                // Data를 ',' 구분하여 잘라냄
                string[] strPos = str.Split(',');

                dXPos = Convert.ToInt16(strPos[0]);
                dYPos = Convert.ToInt16(strPos[1]);
            }

            //Alarm 위치를 위한 Base Image
            switch (Alarm.Info.Group)
            {
                case EAlarmGroup.SYSTEM:
                    PicAlarmPos.BackgroundImage = null;
                    break;
                //case EAlarmGroup.LOADER:
                //    PicAlarmPos.BackgroundImage = Core.Properties.Resources.Loader;
                //    break;
                //case EAlarmGroup.PUSHPULL:
                //    PicAlarmPos.BackgroundImage = Core.Properties.Resources.PushPull;
                //    break;
                //case EAlarmGroup.HANDLER:
                //    PicAlarmPos.BackgroundImage = Core.Properties.Resources.Handler;
                //    break;
                //case EAlarmGroup.SPINNER:
                //    PicAlarmPos.BackgroundImage = Core.Properties.Resources.Spinner;
                //    break;
                //case EAlarmGroup.STAGE:
                //    PicAlarmPos.BackgroundImage = Core.Properties.Resources.LaserScanner;
                //    break;
                //case EAlarmGroup.SCANNER:
                //    PicAlarmPos.BackgroundImage = Core.Properties.Resources.LaserScanner;
                //    break;
                //case EAlarmGroup.LASER:
                //    PicAlarmPos.BackgroundImage = Core.Properties.Resources.LaserScanner;
                //    break;
            }

        }

        private void FormAlarmDisplay_Load(object sender, EventArgs e)
        {
            UpdateAlarmText(MSysCore.Language);

            Label_System.Text = $"[{MSysCore.Language.ToString()}]";
            if (MSysCore.Language == ELanguage.ENGLISH)
            {
                Label_System.Visible = false;
                LabelAlarmText2.Visible = false;
                LabelTrouble2.Visible = false;
            }

            LabelSubAlarmText.Text = MSysCore.ErrorSubMsg;
            if(MSysCore.ErrorSubMsg == "")
            {
                Label_SubTitle.Visible = false;
                LabelSubAlarmText.Visible = false;
            }

            TmrAlarm.Start();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.InquireMsg("Edit Alarm Information?"))
            {
                return;
            }

            var dlg = new FormAlarmEdit();
            dlg.SetAlarmText(LabelAlarmText1.Text, LabelAlarmText2.Text, LabelTrouble1.Text, LabelTrouble2.Text);
            dlg.TopMost = true;
            dlg.ShowDialog();

            if(dlg.DialogResult != DialogResult.OK)
            {
                return;
            }

            IsEdited = true;
            if(MSysCore.Language == ELanguage.ENGLISH)
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
            //if (!CMainFrame.InquireMsg("Save Alarm Information?"))
            //{
            //    return;
            //}

            IsEdited = false;
            string strPos = string.Format("X:{0:d},Y:{1:d}", dXPos, dYPos);
            Alarm.Info.Esc = strPos;

            Alarm.Info.Description[(int)ELanguage.ENGLISH] = LabelAlarmText1.Text;
            Alarm.Info.Solution[(int)ELanguage.ENGLISH] = LabelTrouble1.Text;

            Alarm.Info.Description[(int)MSysCore.Language] = LabelAlarmText2.Text;
            Alarm.Info.Solution[(int)MSysCore.Language] = LabelTrouble2.Text;

            int iResult = CMainFrame.DataManager.UpdateAlarmInfo(Alarm.Info);
            CMainFrame.DisplayAlarm(iResult);
        }

        private void PicAlarmPos_MouseClick(object sender, MouseEventArgs e)
        {
            dXPos = e.X;
            dYPos = e.Y;
        }

        private void TimerUI_Tick(object sender, EventArgs e)
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
