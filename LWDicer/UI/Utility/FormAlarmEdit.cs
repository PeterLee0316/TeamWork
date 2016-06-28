using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LWDicer.UI
{
    public partial class FormAlarmEdit : Form
    {
        string strAlarm, strTrouble;

        public FormAlarmEdit()
        {
            InitializeComponent();
        }

        public void SetAlarmText(string strAlarmText, string strTroubleText)
        {
            strAlarm = strAlarmText;
            strTrouble = strTroubleText;
        }

        public string GetNewAlarmText()
        {
            return TextAlarm.Text;
        }


        public string GetNewTroubleText()
        {
            return TextTrouble.Text;
        }

        private void FormAlarmEdit_Load(object sender, EventArgs e)
        {
            TextAlarm.Text = strAlarm;
            TextTrouble.Text = strTrouble;
        }

        private void FormAlarmEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            FormClose();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            FormClose();
        }
    }
}
