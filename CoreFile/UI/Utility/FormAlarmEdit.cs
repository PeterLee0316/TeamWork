using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Layers;
using static Core.Layers.DEF_Common;

namespace Core.UI
{
    public partial class FormAlarmEdit : Form
    {
        public string strAlarm_Eng, strAlarm_System;
        public string strTrouble_Eng, strTrouble_System;

        public FormAlarmEdit()
        {
            InitializeComponent();
        }

        public void SetAlarmText(string strAlarm_Eng, string strAlarm_System, string strTrouble_Eng, string strTrouble_System)
        {
            this.strAlarm_Eng      = strAlarm_Eng;
            this.strAlarm_System   = strAlarm_System;
            this.strTrouble_Eng    = strTrouble_Eng;
            this.strTrouble_System = strTrouble_System;
        }

        private void FormAlarmEdit_Load(object sender, EventArgs e)
        {
            TextAlarm_Eng.Text      = strAlarm_Eng;
            TextTrouble_Eng.Text    = strTrouble_Eng;

            TextAlarm_System.Text   = strAlarm_System;
            TextTrouble_System.Text = strTrouble_System;

            LabelAlarm_System.Text = $"Alarm Text [{MSysCore.Language.ToString()}]";
            LabelTrouble_System.Text = $"Troubleshooting [{MSysCore.Language.ToString()}]";
            if (MSysCore.Language == ELanguage.ENGLISH)
            {
                LabelAlarm_System.Visible = false;
                LabelTrouble_System.Visible = false;

                TextAlarm_System.Visible = false;
                TextTrouble_System.Visible = false;
            }
        }

        private void FormAlarmEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            strAlarm_Eng      = TextAlarm_Eng.Text     ;
            strAlarm_System   = TextAlarm_System.Text  ;
            strTrouble_Eng    = TextTrouble_Eng.Text   ;
            strTrouble_System = TextTrouble_System.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
