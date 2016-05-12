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

        public DisplayManager m_DisPlayManager = null;

        private FormTopScreen m_FormTopScreen;

        private FormAutoScreen m_FormAutoScreen;
        private FormManualScreen m_FormManualScreen;
        private FormDataScreen m_FormDataScreen;
        private FormTeachScreen m_FormTeachScreen;
        private FormLogScreen m_FormLogScreen;
        private FormHelpScreen m_FormHelpScreen;

        private FormBottomScreen m_FormBottomScreen;

        public CMainFrame()
        {
            InitializeLWDicer();

            InitializeComponent();

            InitializeForm();

            CreateForms();

            InitScreen();

            MainFrame = this;

#if !SIMULATION_VISION
            // View Object select & Cam Live Set
            LWDicer.m_Vision.InitialLocalView(PRE__CAM, CMainFrame.MainFrame.m_FormManualOP.VisionView1.Handle);
#endif

        }

        public void InitScreen()
        {
            m_DisPlayManager = new DisplayManager();

            AttachEventHandlers();

            m_FormTopScreen.Show();

            m_FormBottomScreen.Show();

            SelectFormChange(DEF_UI.SelectScreenType.Auto_Scr);
        }


        private void AttachEventHandlers()
        {
            m_DisPlayManager.m_FormSelectEvent += new FormSelectEventHandler(SelectFormChange);
        }

        private void DetachEventHandlers()
        {
            m_DisPlayManager.m_FormSelectEvent -= new FormSelectEventHandler(SelectFormChange);
        }

        private void SelectFormChange(DEF_UI.SelectScreenType type)
        {
            if (nPrevScr == DEF_UI.SelectScreenType.Auto_Scr) { m_FormAutoScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Manual_Scr) { m_FormManualScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Data_Scr) { m_FormDataScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Teach_Scr) { m_FormTeachScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Log_Scr) { m_FormLogScreen.Hide(); }
            if (nPrevScr == DEF_UI.SelectScreenType.Help_Scr) { m_FormHelpScreen.Hide(); }

            switch (type)
            {
                case DEF_UI.SelectScreenType.Auto_Scr:
                    m_FormAutoScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Manual_Scr:
                    m_FormManualScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Data_Scr:
                    m_FormDataScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Teach_Scr:
                    m_FormTeachScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Log_Scr:
                    m_FormLogScreen.Show();
                    nPrevScr = type;
                    break;
                case DEF_UI.SelectScreenType.Help_Scr:
                    m_FormHelpScreen.Show();
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
            //this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            this.Size = new Size(DEF_UI.FORM_SIZE_WIDTH, DEF_UI.FORM_SIZE_HEIGHT);
            this.IsMdiContainer = true;
            
        }

        private void CreateForms()
        {
            m_FormTopScreen = new FormTopScreen();

            m_FormAutoScreen = new FormAutoScreen();
            m_FormManualScreen = new FormManualScreen();
            m_FormDataScreen = new FormDataScreen();
            m_FormTeachScreen = new FormTeachScreen();
            m_FormLogScreen = new FormLogScreen();
            m_FormHelpScreen = new FormHelpScreen();

            m_FormBottomScreen = new FormBottomScreen();


            SetProperty(m_FormTopScreen);

            SetProperty(m_FormAutoScreen);
            SetProperty(m_FormManualScreen);
            SetProperty(m_FormDataScreen);
            SetProperty(m_FormTeachScreen);
            SetProperty(m_FormLogScreen);
            SetProperty(m_FormHelpScreen);

            SetProperty(m_FormBottomScreen);
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
    }

    public delegate void FormSelectEventHandler(DEF_UI.SelectScreenType type);

    public class DisplayManager
    {
        public event FormSelectEventHandler m_FormSelectEvent = null;

        public void FormSelectChange(DEF_UI.SelectScreenType type)
        {
            if (m_FormSelectEvent != null)
            {
                m_FormSelectEvent(type);
            }
        }
    }

}
