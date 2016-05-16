using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Syncfusion.Windows.Forms;

namespace LWDicer.UI
{
    public partial class FormOriginReturn : Form
    {
        enum EAxis
        {
            LIFTER=0,
            FEEDER,
            CENTERING1,
            CENTERING2,
            SP1_ROTATE,
            SP1_NOZZLE1,
            SP1_NOZZLE2,
            SP2_ROTATE,
            SP2_NOZZLE1,
            SP2_NOZZLE2,
            UPTR_Z,
            UPTR_Y,
            LOTR_Z,
            LOTR_Y,
            STAGE_X,
            STAGE_Z,
            STAGE_T,
            SCANNER_Z,
            CAMERA_Z,
            MAX,
        }

        private bool [] nSelAxis = new bool[(int)EAxis.MAX];

        private ButtonAdv[] Axis = new ButtonAdv[(int)EAxis.MAX];


        public FormOriginReturn()
        {
            InitializeComponent();

            ResouceMapping();
        }

        private void ResouceMapping()
        {
            Axis[0]  = BtnAxis1;  Axis[1]  = BtnAxis2;  Axis[2]  = BtnAxis3;  Axis[3]  = BtnAxis4;  Axis[4]  = BtnAxis5;
            Axis[5]  = BtnAxis6;  Axis[6]  = BtnAxis7;  Axis[7]  = BtnAxis8;  Axis[8]  = BtnAxis9;  Axis[9]  = BtnAxis10;
            Axis[10] = BtnAxis11; Axis[11] = BtnAxis12; Axis[12] = BtnAxis13; Axis[13] = BtnAxis14; Axis[14] = BtnAxis15;
            Axis[15] = BtnAxis16; Axis[16] = BtnAxis17; Axis[17] = BtnAxis18; Axis[18] = BtnAxis19;

            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                Axis[i].Image = Image.Images[0];
            }
        }
        private void BtnAxis_Click(object sender, EventArgs e)
        {
            int nNo = 0;

            ButtonAdv Axis = sender as ButtonAdv;

            nNo = Convert.ToInt16(Axis.Tag);

            SelectAxis(nNo);
        }

        private void SelectAxis(int nNo)
        {
            if(nSelAxis[nNo] == false)
            {
                nSelAxis[nNo] = true;
                Axis[nNo].Image = Image.Images[1];
            }
            else
            {
                nSelAxis[nNo] = false;
                Axis[nNo].Image = Image.Images[0];
            }
        }


        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for(int i=0;i< (int)EAxis.MAX; i++)
            {
                nSelAxis[i] = true;
                Axis[i].Image = Image.Images[1];
            }

        }

        private void BtnCancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EAxis.MAX; i++)
            {
                nSelAxis[i] = false;
                Axis[i].Image = Image.Images[0];
            }
        }

        private void BtnOriginReturn_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("선택한 축에 대해 원점 복귀를 하시겠습니까?"))
            {
                return;
            }
        }

        private void BtnServoOn_Click(object sender, EventArgs e)
        {
            if(!CMainFrame.LWDicer.DisplayMsg("선택한 축을 Servo On 하시겠습니까?"))
            {
                return;
            }
        }

        private void BtnServoOff_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("선택한 축을 Servo Off 하시겠습니까?"))
            {
                return;
            }
        }

        private void FormOriginReturn_Load(object sender, EventArgs e)
        {
            int i=0;
            i++;   
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormOriginReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }
    }
}
