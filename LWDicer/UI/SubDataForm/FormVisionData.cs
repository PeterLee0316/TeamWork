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
    public partial class FormVisionData : Form
    {
        public FormVisionData()
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

        private void FormVisionData_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);
        }

        private void FormVisionData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }
    }
}
