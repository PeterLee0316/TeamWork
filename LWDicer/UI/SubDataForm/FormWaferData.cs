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
    public partial class FormWaferData : Form
    {
        public FormWaferData()
        {
            InitializeComponent();
        }

        private void FormClose()
        {
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void FormWaferData_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);
        }

        private void FormWaferData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }
    }
}
