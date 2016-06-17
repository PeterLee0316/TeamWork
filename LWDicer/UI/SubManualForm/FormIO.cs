﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Syncfusion.Windows.Forms.Tools;

namespace LWDicer.UI
{
    public partial class FormIO : Form
    {
        private GradientLabel[] X_Title = new GradientLabel[16];
        private GradientLabel[] X_Name = new GradientLabel[16];
        private GradientLabel[] Y_Title = new GradientLabel[16];
        private GradientLabel[] Y_Name = new GradientLabel[16];

        private int nIOPage = 0;

        public FormIO()
        {
            InitializeComponent();

            ResouceMapping();
        }

        private void tmr_IO_Tick(object sender, EventArgs e)
        {

        }

        private void UpdateIO(int nBoardNo)
        {
            int i = 0, nNo = 0;
            string hex;

            for (i = 0; i < 16; i++)
            {
                if (nBoardNo > 0)
                {
                    nNo = i + (nBoardNo * 16);
                }
                else
                {
                    nNo = i;
                }

                X_Title[i].Text = string.Format("X{0:X4}", nNo);

                X_Name[i].Text = CMainFrame.LWDicer.m_DataManager.InputArray[nNo].Name[0];

                Y_Title[i].Text = string.Format("Y{0:X4}", nNo);

                Y_Name[i].Text = CMainFrame.LWDicer.m_DataManager.OutputArray[nNo].Name[0];

            }
        }

        private void ResouceMapping()
        {
            X_Title[0] = Title_IO_X1;
            X_Title[1] = Title_IO_X2;
            X_Title[2] = Title_IO_X3;
            X_Title[3] = Title_IO_X4;
            X_Title[4] = Title_IO_X5;
            X_Title[5] = Title_IO_X6;
            X_Title[6] = Title_IO_X7;
            X_Title[7] = Title_IO_X8;
            X_Title[8] = Title_IO_X9;
            X_Title[9] = Title_IO_X10;
            X_Title[10] = Title_IO_X11;
            X_Title[11] = Title_IO_X12;
            X_Title[12] = Title_IO_X13;
            X_Title[13] = Title_IO_X14;
            X_Title[14] = Title_IO_X15;
            X_Title[15] = Title_IO_X16;

            X_Name[0] = IO_X1_Name;
            X_Name[1] = IO_X2_Name;
            X_Name[2] = IO_X3_Name;
            X_Name[3] = IO_X4_Name;
            X_Name[4] = IO_X5_Name;
            X_Name[5] = IO_X6_Name;
            X_Name[6] = IO_X7_Name;
            X_Name[7] = IO_X8_Name;
            X_Name[8] = IO_X9_Name;
            X_Name[9] = IO_X10_Name;
            X_Name[10] = IO_X11_Name;
            X_Name[11] = IO_X12_Name;
            X_Name[12] = IO_X13_Name;
            X_Name[13] = IO_X14_Name;
            X_Name[14] = IO_X15_Name;
            X_Name[15] = IO_X16_Name;

            Y_Title[0] = Title_IO_Y1;
            Y_Title[1] = Title_IO_Y2;
            Y_Title[2] = Title_IO_Y3;
            Y_Title[3] = Title_IO_Y4;
            Y_Title[4] = Title_IO_Y5;
            Y_Title[5] = Title_IO_Y6;
            Y_Title[6] = Title_IO_Y7;
            Y_Title[7] = Title_IO_Y8;
            Y_Title[8] = Title_IO_Y9;
            Y_Title[9] = Title_IO_Y10;
            Y_Title[10] = Title_IO_Y11;
            Y_Title[11] = Title_IO_Y12;
            Y_Title[12] = Title_IO_Y13;
            Y_Title[13] = Title_IO_Y14;
            Y_Title[14] = Title_IO_Y15;
            Y_Title[15] = Title_IO_Y16;

            Y_Name[0] = IO_Y1_Name;
            Y_Name[1] = IO_Y2_Name;
            Y_Name[2] = IO_Y3_Name;
            Y_Name[3] = IO_Y4_Name;
            Y_Name[4] = IO_Y5_Name;
            Y_Name[5] = IO_Y6_Name;
            Y_Name[6] = IO_Y7_Name;
            Y_Name[7] = IO_Y8_Name;
            Y_Name[8] = IO_Y9_Name;
            Y_Name[9] = IO_Y10_Name;
            Y_Name[10] = IO_Y11_Name;
            Y_Name[11] = IO_Y12_Name;
            Y_Name[12] = IO_Y13_Name;
            Y_Name[13] = IO_Y14_Name;
            Y_Name[14] = IO_Y15_Name;
            Y_Name[15] = IO_Y16_Name;
        }

        private void FormIO_Load(object sender, EventArgs e)
        {
            tmr_IO.Enabled = true;
            tmr_IO.Interval = 100;
            tmr_IO.Start();

            UpdateIO(nIOPage);
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            if (nIOPage != 0)
            {
                nIOPage--;
            }
            UpdateIO(nIOPage);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (nIOPage < 15)
            {
                nIOPage++;
            }

            UpdateIO(nIOPage);
        }

        private void IO_Y_Click(object sender, EventArgs e)
        {
            string strText;
            int nNo = 0;

            GradientLabel OutPut = sender as GradientLabel;

            nNo = Convert.ToInt16(OutPut.Tag);

            strText = string.Format("{0:s} 강제 출력하시겠습니까?", OutPut.Text);

            if (!CMainFrame.LWDicer.DisplayMsg(strText))
            {
                return;
            }

            // Output 출력

        }

        private void FormClose()
        {
            tmr_IO.Enabled = false;
            tmr_IO.Stop();

            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            nIOPage = 0;
            UpdateIO(nIOPage);
        }

        private void BtnEnd_Click(object sender, EventArgs e)
        {
            nIOPage = 14;
            UpdateIO(nIOPage);
        }
    }
}
