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
    public partial class FormCylinderData : Form
    {
        public FormCylinderData()
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

        private void FormCylinderData_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(1, 100);
        }

        private void FormCylinderData_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }
    }
}
