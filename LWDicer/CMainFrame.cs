using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Configuration;

using LWDicer.Control;
using LWDicer.UI;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.EWindowMessage;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Cylinder;
using static LWDicer.Control.DEF_SerialPort;
using static LWDicer.Control.DEF_Vision;

using static LWDicer.Control.DEF_UI;

//#pragma warning disable CS0219

namespace LWDicer.UI
{
    public partial class CMainFrame : Form
    {
        public static MLWDicer LWDicer = new MLWDicer(new CObjectInfo());
        public static MDataManager DataManager;
        public static CDBInfo DBInfo;

        public static CMainFrame MainFrame = null;

        public static EFormType PrevScreen;
        public CDisplayManager DisplayManager = new CDisplayManager();

        private FormTopScreen TopScreen;
        private FormBottomScreen BottomScreen;

        //private FormAutoScreen AutoScreen;
        //private FormManualScreen ManualScreen;
        //private FormDataScreen DataScreen;
        //private FormTeachScreen TeachScreen;
        //private FormLogScreen LogScreen;
        //private FormHelpScreen HelpScreen;

        private Form[] MainFormArray = new Form[(int)EFormType.MAX];

        public CMainFrame()
        {
            bool bRtn = InitializeLWDicer();
            if(bRtn == false)
            {
            }

            InitializeComponent();

            InitializeForm();

            CreateForms();
            InitScreen();

            MainFrame = this;            
        }

        public void InitScreen()
        {
            AttachEventHandlers();

            TopScreen.Show();
            BottomScreen.Show();
            SelectFormChange(EFormType.AUTO);

            LWDicer.m_Vision.InitialLocalView(0, pnlPic.Handle);
            LWDicer.m_Vision.InitialLocalView(1, pnlPic.Handle);
            LWDicer.m_Vision.InitialLocalView(2, pnlPic.Handle);
        }


        private void AttachEventHandlers()
        {
            DisplayManager.FormHandler += new FormSelectEventHandler(SelectFormChange);
        }

        private void DetachEventHandlers()
        {
            DisplayManager.FormHandler -= new FormSelectEventHandler(SelectFormChange);
        }

        private void SelectFormChange(EFormType type)
        {
            MainFormArray[(int)PrevScreen].Hide();
            MainFormArray[(int)type].Top = TopScreen.Location.Y + TopScreen.Height + 2;
            MainFormArray[(int)type].Show();
            PrevScreen = type;
        }

        public void ProcessMsg(MEvent evnt)
        {
            string msg = "MainFrame got message from control : " + evnt;
            Debug.WriteLine("===================================================");
            Debug.WriteLine(msg);
            Debug.WriteLine("===================================================");

            switch(evnt.Msg)
            {
                case (int)EWindowMessage.WM_ALARM_MSG:
                    DisplayAlarm(evnt.lParam, evnt.wParam);
                    break;
            }
        }

        protected virtual void InitializeForm()
        {
            Screen[] sc = System.Windows.Forms.Screen.AllScreens;

            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;

            if (sc.Length > 1)
            {
                this.DesktopLocation = new Point(FORM_POS_X + sc[0].Bounds.Width, FORM_POS_Y); // 다중 모니터
            }
            else
            {
                this.DesktopLocation = new Point(FORM_POS_X, FORM_POS_Y); // 기본 모니터
            }

            this.DesktopLocation = new Point(FORM_POS_X, FORM_POS_Y); // 기본 모니터
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            this.Size = new Size(FORM_SIZE_WIDTH, FORM_SIZE_HEIGHT);
            this.IsMdiContainer = true;
            
        }

        private void CreateForms()
        {
            TopScreen = new FormTopScreen();
            BottomScreen = new FormBottomScreen();

            MainFormArray[(int)EFormType.AUTO]   = new FormAutoScreen();
            MainFormArray[(int)EFormType.MANUAL] = new FormManualScreen();
            MainFormArray[(int)EFormType.DATA]   = new FormDataScreen();
            MainFormArray[(int)EFormType.TEACH]  = new FormTeachScreen();
            MainFormArray[(int)EFormType.LOG]    = new FormLogScreen();
            MainFormArray[(int)EFormType.HELP]   = new FormHelpScreen();

            SetProperty(TopScreen);
            SetProperty(BottomScreen);

            foreach(Form form in MainFormArray)
            {
                SetProperty(form);
            }
        }

        private void SetProperty(Form form)
        {
            form.TopLevel = true;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.MdiParent = this;
        }

        public bool InitializeLWDicer()
        {
            //int iResult = LWDicer.Initialize(MainFrame);
            int iResult = LWDicer.Initialize(this);
            if (iResult != SUCCESS)
            {
                // Show Error Message & 프로그램 종료?
                LWDicer.ShowAlarmWhileInit(iResult);
                return false;
            }

            DataManager = LWDicer.m_DataManager;            

            return true;
        }

        private void CMainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Program 종료를 위해 Thread Kill
            LWDicer.StopThreads();
            
            DetachEventHandlers();

            this.Dispose();
            this.Close();
        }

        static public void DisplayAlarm(int alarmcode, int pid = 0, bool saveLog = true)
        {
            if (alarmcode == SUCCESS)
            {
                DisplayMsg("요청한 작업이 성공했습니다.");
            }
            else
            {
                CAlarm alarm = LWDicer.GetAlarmInfo(alarmcode, pid, saveLog);
                var dlg = new FormAlarmDisplay(alarm);
                dlg.ShowDialog();
            }
        }

        static public bool DisplayMsg(string strMsg)
        {
            var dlg = new FormMessageBox();
            dlg.SetMessage(strMsg, EMessageType.OK);
            dlg.TopMost = true;
            dlg.ShowDialog();

            if (dlg.DialogResult == DialogResult.OK)
                return true;
            else return false;
        }

        static public bool InquireMsg(string strMsg)
        {
            var dlg = new FormMessageBox();
            dlg.SetMessage(strMsg, EMessageType.OK_CANCEL);
            dlg.TopMost = true;
            dlg.ShowDialog();

            if (dlg.DialogResult == DialogResult.OK)
                return true;
            else return false;
        }

        static public void DisplayJog()
        {
            var dlg = new FormJogOperation();
            dlg.TopMost = true;
            dlg.Show();
        }

        static public bool GetKeyPad(string strCurrent, out string strModify)
        {
            var dlg = new FormKeyPad();
            dlg.SetValue(strCurrent);
            dlg.TopMost = true;
            dlg.ShowDialog();

            if (dlg.DialogResult == DialogResult.OK)
            {
                if (dlg.ModifyNo.Text == "")
                {
                    strModify = "0";
                }
                else
                {
                    strModify = dlg.ModifyNo.Text;
                }
            }
            else
            {
                strModify = strCurrent;
                dlg.Dispose();
                return false;
            }
            dlg.Dispose();
            return true;
        }

        static public bool GetKeyboard(out string strModify, string title = "Input", bool SecretMode = false)
        {
            var dlg = new FormKeyBoard(title, SecretMode);
            dlg.TopMost = true;
            dlg.ShowDialog();

            if (dlg.DialogResult == DialogResult.OK)
            {
                strModify = dlg.InputString;
                return true;
            }
            else
            {
                strModify = "";
                return false;
            }
        }

        /// <summary>
        /// Message Index Number를 이용해서 MessageInfo 를 Loading
        /// index를 이용해서 호출하면, 실제 프로그램 코드에선 알아보기가 힘들것 같아서
        /// 일부러 주석처리하고 사용하지 않기로 함
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        //static public bool DisplayMsg(int index)
        //{
        //    var dlg = new FormMessageBox();
        //    dlg.SetMessage(index);
        //    dlg.ShowDialog();

        //    if (dlg.DialogResult == DialogResult.OK || dlg.DialogResult == DialogResult.OK)
        //        return true;
        //    else return false;
        //}
    }

    public delegate void FormSelectEventHandler(EFormType type);

    public class CDisplayManager
    {
        public event FormSelectEventHandler FormHandler = null;

        public void FormSelectChange(EFormType type)
        {
            if (FormHandler != null)
            {
                FormHandler(type);
            }
        }
    }

}
