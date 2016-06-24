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
using LWDicer.Control;
using static LWDicer.Control.DEF_Thread;

namespace LWDicer.UI
{
    public partial class FormUnitInit : Form
    {

        private bool[] SelectedPart = new bool[(int)EInitiableUnit.MAX];

        private ButtonAdv[] BtnList = new ButtonAdv[(int)EInitiableUnit.MAX];

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

            ButtonAdv BtnList = sender as ButtonAdv;

            nNo = Convert.ToInt16(BtnList.Tag);

            SelectPart(nNo);
        }

        private void ResouceMapping()
        {
            BtnList[0] = BtnLoader;
            BtnList[1] = BtnPushPull;
            BtnList[2] = BtnSpinner1;
            BtnList[3] = BtnSpinner2;
            BtnList[4] = BtnHandler;
            BtnList[5] = BtnStage;

            for(int i=0;i< (int)EInitiableUnit.MAX; i++)
            {
                BtnList[i].Image = Image.Images[0];
            }
        }

        private void SelectPart(int nNo)
        {
            if (SelectedPart[nNo] == false)
            {
                SelectedPart[nNo] = true;
                BtnList[nNo].Image = Image.Images[1];
            }
            else
            {
                SelectedPart[nNo] = false;
                BtnList[nNo].Image = Image.Images[0];
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EInitiableUnit.MAX; i++)
            {
                SelectedPart[i] = true;
                BtnList[i].Image = Image.Images[1];
            }
        }

        private void BtnCancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EInitiableUnit.MAX; i++)
            {
                SelectedPart[i] = false;
                BtnList[i].Image = Image.Images[0];
            }
        }

        private void BtnExecuteIni_Click(object sender, EventArgs e)
        {
            if (!CMainFrame.LWDicer.DisplayMsg("선택한 Part를 초기화 하시겠습니까?"))
            {
                return;
            }

            if(SelectedPart[(int)EInitiableUnit.LOADER] == true)
            {
                //CMainFrame.LWDicer.m_trs
            }
        }
    }
}
