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
    public partial class FormUnitInit : Form
    {

        enum EPart
        {
            LOADER = 0,
            PUSHPULL,
            SPINNER1,
            SPINNER2,
            LOWER_TR,
            UPPER_TR,
            STAGE,
            MAX,
        }

        private bool[] nSelPart = new bool[(int)EPart.MAX];

        private ButtonAdv[] InitPart = new ButtonAdv[(int)EPart.MAX];

        public FormUnitInit()
        {
            InitializeComponent();

            ResouceMapping();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BtnInitPart_Click(object sender, EventArgs e)
        {
            int nNo = 0;

            ButtonAdv InitPart = sender as ButtonAdv;

            nNo = Convert.ToInt16(InitPart.Tag);

            SelectPart(nNo);
        }

        private void ResouceMapping()
        {
            InitPart[0] = BtnLoader;
            InitPart[1] = BtnPushPull;
            InitPart[2] = BtnSpinner1;
            InitPart[3] = BtnSpinner2;
            InitPart[4] = BtnLowerTR;
            InitPart[5] = BtnUpperTR;
            InitPart[6] = BtnStage;

            for(int i=0;i< (int)EPart.MAX; i++)
            {
                InitPart[i].Image = Image.Images[0];
            }
        }

        private void SelectPart(int nNo)
        {
            if (nSelPart[nNo] == false)
            {
                nSelPart[nNo] = true;
                InitPart[nNo].Image = Image.Images[1];
            }
            else
            {
                nSelPart[nNo] = false;
                InitPart[nNo].Image = Image.Images[0];
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EPart.MAX; i++)
            {
                nSelPart[i] = true;
                InitPart[i].Image = Image.Images[1];
            }
        }

        private void BtnCancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EPart.MAX; i++)
            {
                nSelPart[i] = false;
                InitPart[i].Image = Image.Images[0];
            }
        }

        private void BtnExecuteIni_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("선택한 Part를 초기화 하시겠습니까?"))
            {
                return;
            }
        }
    }
}
