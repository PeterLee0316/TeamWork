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

//#pragma warning disable CS0219

namespace LWDicer.UI
{
    public partial class CMainFrame : Form
    {
        public static MLWDicer LWDicer = new MLWDicer(new CObjectInfo());

        public static CMainFrame MainFrame = null;

        public static DEF_UI.SelectScreenType nPrevScr;

        public CDisplayManager DisplayManager = new CDisplayManager();

        private FormTopScreen TopScreen;

        private FormAutoScreen AutoScreen;
        private FormManualScreen ManualScreen;
        private FormDataScreen DataScreen;
        private FormTeachScreen TeachScreen;
        private FormLogScreen LogScreen;
        private FormHelpScreen HelpScreen;

        private FormBottomScreen BottomScreen;

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

            SelectFormChange(DEF_UI.SelectScreenType.Auto_Scr);
        }


        private void AttachEventHandlers()
        {
            DisplayManager.FormHandler += new FormSelectEventHandler(SelectFormChange);
        }

        private void DetachEventHandlers()
        {
            DisplayManager.FormHandler -= new FormSelectEventHandler(SelectFormChange);
        }

        private void SelectFormChange(DEF_UI.SelectScreenType type)
        {
            if (nPrevScr == DEF_UI.SelectScreenType.Auto_Scr) { AutoScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Manual_Scr) { ManualScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Data_Scr) { DataScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Teach_Scr) { TeachScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Log_Scr) { LogScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Help_Scr) { HelpScreen.Hide(); }

            switch (type)
            {
                case DEF_UI.SelectScreenType.Auto_Scr:
                    AutoScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Manual_Scr:
                    ManualScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Data_Scr:
                    DataScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Teach_Scr:
                    TeachScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Log_Scr:
                    LogScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Help_Scr:
                    HelpScreen.Show();
                    nPrevScr = type;
                    break;
            }
        }

        public void ProcessMsg(MEvent evnt)
        {
            string msg = "Get Message from Control : " + evnt;
            Debug.WriteLine("===================================================");
            Debug.WriteLine(msg);
        }

        protected virtual void InitializeForm()
        {
            Screen[] sc = System.Windows.Forms.Screen.AllScreens;

            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;

            if (sc.Length > 1)
            {
                this.DesktopLocation = new Point(DEF_UI.FORM_POS_X + sc[0].Bounds.Width, DEF_UI.FORM_POS_Y); // 다중 모니터
            }
            else
            {
                this.DesktopLocation = new Point(DEF_UI.FORM_POS_X, DEF_UI.FORM_POS_Y); // 기본 모니터
            }

            this.DesktopLocation = new Point(DEF_UI.FORM_POS_X, DEF_UI.FORM_POS_Y); // 기본 모니터
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            this.Size = new Size(DEF_UI.FORM_SIZE_WIDTH, DEF_UI.FORM_SIZE_HEIGHT);
            this.IsMdiContainer = true;
            
        }

        private void CreateForms()
        {
            TopScreen = new FormTopScreen();

            AutoScreen = new FormAutoScreen();
            ManualScreen = new FormManualScreen();
            DataScreen = new FormDataScreen();
            TeachScreen = new FormTeachScreen();
            LogScreen = new FormLogScreen();
            HelpScreen = new FormHelpScreen();

            BottomScreen = new FormBottomScreen();


            SetProperty(TopScreen);

            SetProperty(AutoScreen);
            SetProperty(ManualScreen);
            SetProperty(DataScreen);
            SetProperty(TeachScreen);
            SetProperty(LogScreen);
            SetProperty(HelpScreen);

            SetProperty(BottomScreen);
        }

        private void SetProperty(Form TargetForm)
        {
            TargetForm.TopLevel = false;
            TargetForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            TargetForm.Dock = DockStyle.Fill;
            TargetForm.MdiParent = this;

        }


        public bool InitializeLWDicer()
        {
            int iResult = LWDicer.Initialize(MainFrame);
            if (iResult != SUCCESS)
            {
                // Show Error Message & 프로그램 종료?
                LWDicer.ShowAlarmWhileInit(iResult);
                return false;
            }
            return true;
        }

        private void CMainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Program 종료를 위해 Thread Kill
            LWDicer.StopThreads();

            LWDicer.m_Scanner[0].LSEPortClose();

            DetachEventHandlers();

            this.Dispose();
            this.Close();
        }

        static public void DisplayAlarm(int alarmcode, int pid = 0, bool saveLog = true)
        {
            if (alarmcode == SUCCESS)
            {
                DisplayMsg("", "요청한 작업이 성공했습니다.", false);

            }
            else
            {
                CAlarm alarm = LWDicer.GetAlarmInfo(alarmcode, pid, saveLog);
                var dlg = new FormAlarmDisplay(alarm);
                dlg.ShowDialog();
            }
        }

        static public bool DisplayMsg(string strMsg_Eng, string strMsg_System, bool bOkCancel = true)
        {
            FormMessageBox dlg = new FormMessageBox();
            dlg.SetMessage(strMsg_Eng, strMsg_System, bOkCancel);
            dlg.ShowDialog();

            if (dlg.DialogResult == DialogResult.OK || dlg.DialogResult == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public delegate void FormSelectEventHandler(DEF_UI.SelectScreenType type);

    public class CDisplayManager
    {
        public event FormSelectEventHandler FormHandler = null;

        public void FormSelectChange(DEF_UI.SelectScreenType type)
        {
            if (FormHandler != null)
            {
                FormHandler(type);
            }
        }
    }

}
