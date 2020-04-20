using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Grid;

using Core.Layers;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_LCNet;

namespace Core.UI
{
    public partial class FormAutoScreen : Form
    {
        int m_nStartReady = 0;      // 0:Off, 1:Ready, 2:Run

        //FormWorkPieceInquiry Dlg_WPInquiry = new FormWorkPieceInquiry();

        public FormAutoScreen()
        {
            InitializeComponent();
            InitializeForm();

            //timer1.Interval = UITimerInterval;
            TimerUI.Interval = 10;
            TimerUI.Enabled = true;
            TimerUI.Start();

        }
        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
            
        }


        private void FormAutoScreen_Activated(object sender, EventArgs e)
        {
           
        }

        protected override void WndProc(ref Message wMessage)
        {
            switch (wMessage.Msg)
            {
                default:
                    break;
            }

            base.WndProc(ref wMessage);
        }

        public void WindowProc(MEvent evnt)
        {
            string msg = "FormAutoScreen got message from MainFrame : " + evnt.ToWindowMessage();
            Debug.WriteLine("===================================================");
            Debug.WriteLine(msg);
            Debug.WriteLine("===================================================");

            // 변수 선언 및 작업의 편리성을 위해서 일부러 switch대신에 if/else if 구문을 사용함
            if (false)
            {

            }
            else if (evnt.Msg == (int)EWindowMessage.WM_START_READY_MSG)
            {
                //m_dlgStart.ShowWindow(SW_SHOW);
                //m_dlgErrorStop.ShowWindow(SW_HIDE);
                //m_dlgStepStop.ShowWindow(SW_HIDE);

                // Button Disable 
                m_nStartReady = 1;
                SetButtonStatus(true);
            }
            else if (evnt.Msg == (int)EWindowMessage.WM_START_RUN_MSG)
            {
                //m_dlgStart.ShowWindow(SW_HIDE);
                //m_dlgErrorStop.ShowWindow(SW_HIDE);
                //m_dlgStepStop.ShowWindow(SW_HIDE);

                // Button Disable 
                m_nStartReady = 2;
                SetButtonStatus(true);
            }
            else if (evnt.Msg == (int)EWindowMessage.WM_START_MANUAL_MSG)
            {
                //m_dlgStart.ShowWindow(SW_HIDE);
                //m_dlgErrorStop.ShowWindow(SW_HIDE);
                //m_dlgStepStop.ShowWindow(SW_HIDE);

                // Button Disable 
                m_nStartReady = 0;
                SetButtonStatus(false);
            }
            else if (evnt.Msg == (int)EWindowMessage.WM_STEPSTOP_MSG)
            {
                //m_dlgStart.ShowWindow(SW_HIDE);
                //m_dlgErrorStop.ShowWindow(SW_HIDE);
                //m_dlgStepStop.ShowWindow(SW_SHOW);

                SetButtonStatus(true);
            }
            else if (evnt.Msg == (int)EWindowMessage.WM_ERRORSTOP_MSG)
            {
                //m_dlgStart.ShowWindow(SW_HIDE);
                //m_dlgErrorStop.ShowWindow(SW_SHOW);
                //m_dlgStepStop.ShowWindow(SW_HIDE);

            }
            else if (evnt.Msg == (int)EWindowMessage.WM_ALARM_MSG)
            {
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_PANEL_DISTANCE_MSG1)
            {
                //m_sMeasuredDistCell1.SetWindowText(m_pTrsStage1->GetAlignResult());
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_TACTTIME_MSG)
            {
                //// EQ Tact Time 기록하기 
                //str.Format("%.2f", *(double*)wParam);
                //m_LblEqTactTime.SetWindowText(str);

                //// Line Tact Time 기록하기 
                //str.Format("%.2f", *(double*)lParam);
                //m_LblLineTactTime.SetWindowText(str);
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_PRODUCT_IN_MSG)
            {
                //m_ProductData.uiProductQuantity_forIn++;
                //str.Format("IN %d, OUT %d", m_ProductData.uiProductQuantity_forIn, m_ProductData.uiProductQuantity_forOut);
                //m_LblProductCnt.SetWindowText(str);
                //MSiSystem.SaveProductData(m_ProductData);
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_PRODUCT_OUT_MSG)
            {
                //m_ProductData.uiProductQuantity_forOut++;
                //str.Format("IN %d, OUT %d", m_ProductData.uiProductQuantity_forIn, m_ProductData.uiProductQuantity_forOut);
                //m_LblProductCnt.SetWindowText(str);
                //MSiSystem.SaveProductData(m_ProductData);

                //str.Format("%s", *(CString*)wParam);
                //m_LblAfterEquipId.SetWindowText(str);
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_EQ_STATE)
            {
                //m_LblEqState.SetWindowText(m_strEqState[m_pTrsLCNet->m_eEqState]);
            }
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_EQ_PROC_STATE)
            {
                //m_LblEqProcessState.SetWindowText(m_strEqProcState[m_pTrsLCNet->m_eEqProcState]);
            }
            /*	else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_TERMINAL_MSG)
                {
                    m_ListTerminalMsg.DeleteString(1);
                    CTime t = CTime::GetCurrentTime();
                    CString temp;
                    temp = *(CString*)lParam;
                    sprintf(buf, "[%02d:%02d:%02d] %s", t.GetHour(), t.GetMinute(), t.GetSecond(), temp);

                    m_ListTerminalMsg.InsertString(0, buf);
                    if(m_ListTerminalMsg.GetCount()>5)
                        m_ListTerminalMsg.DeleteString(m_ListTerminalMsg.GetCount()-1);
                    m_ListTerminalMsg.ShowWindow(SW_SHOW);
                    m_ListTerminalMsg.UpdateWindow();
                }
            */
            else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_RUN_MODE)
            {
                //UpdateDataMembers();
            }
            //else if (evnt.Msg ==(int)EWindowMessage.WM_DISP_MODEL_NAME)
            //{
            //    UpdateDataMembers();
            //}
            //else if (evnt.Msg ==(int)EWindowMessage.WM_NSMC_CONTROL_PANEL_SUPPLY_START)
            //{
            //    BOOL bTest = FALSE;
            //    m_BtnCellSupplyStop.SetValue(bTest);

            //    if (bTest == (int)EWindowMessage.TRUE)  // 버튼 눌려졌을 때
            //    {
            //        m_pTrsStage1->SendMsg(MSG_PANEL_SUPPLY_STOP);
            //    }
            //    else
            //    {
            //        m_pTrsStage1->SendMsg(MSG_PANEL_SUPPLY_START);
            //    }
            //}
            //else if (evnt.Msg ==(int)EWindowMessage.WM_NSMC_CONTROL_PANEL_SUPPLY_STOP)
            //{
            //    BOOL bTest = TRUE;
            //    m_BtnCellSupplyStop.SetValue(bTest);

            //    if (bTest == (int)EWindowMessage.TRUE)  // 버튼 눌려졌을 때
            //    {
            //        m_pTrsStage1->SendMsg(MSG_PANEL_SUPPLY_STOP);
            //    }
            //    else
            //    {
            //        m_pTrsStage1->SendMsg(MSG_PANEL_SUPPLY_START);
            //    }
            //}
            else
            {
                msg = "FormAutoScreen unknown message : " + evnt;
                Debug.WriteLine("***************************************************");
                Debug.WriteLine(msg);
                Debug.WriteLine("***************************************************");
            }

        }



        void SetButtonStatus(bool bRunStart)
        {
            bool bEnable = bRunStart ? false : true;
            if(bRunStart)
            {
                //BtnStart.Text = "Stop AutoRun";

            }
            else
            {
                //BtnStart.Text = "Start AutoRun";

            }
   
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {

        }

        private void FormAutoScreen_Load(object sender, EventArgs e)
        {

        }
        

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
