using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LWDicer.UI
{
    public partial class FormMessageBox : Form
    {
        private string strMsg_Eng, strMsg_System;
        private bool bMode_OkCancel;

        public FormMessageBox()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.Fixed3D;
        }

        public void SetMessage(string strMsg_Eng, string strMsg_System, bool bMode_OkCancel)
        {
            this.bMode_OkCancel = bMode_OkCancel;
            this.strMsg_Eng = strMsg_Eng;
            this.strMsg_System = strMsg_System;
        }

        private void FormUtilMsg_Load(object sender, EventArgs e)
        {
            LabelTextEng.Text = strMsg_Eng;
            LabelTextSystem.Text = strMsg_System;

            if (bMode_OkCancel == true)
            {
                BtnConfirm.Visible = true;
                //BtnOK.Visible = true;
                //BtnCancel.Visible = true;
                BtnCancel.Text = "Cancel";
            }
            else
            {
                BtnConfirm.Visible = false;
                //BtnOK.Visible = false;
                //BtnCancel.Visible = false;
                BtnCancel.Text = "Ok";
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if(bMode_OkCancel) this.DialogResult = DialogResult.Cancel;
            else this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
