using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;

using LWDicer.UI;
using LWDicer.Control;

using static LWDicer.Control.DEF_PolygonScanner;
using static LWDicer.Control.DEF_Thread;

namespace LWDicer.UI
{
    public partial class FormTeachScreen : Form
    {
        private FormTeaching m_Teach;

        public FormTeachScreen()
        {
            InitializeComponent();

            m_Teach = new FormTeaching();
            m_Teach.SetUnit((int)ETeachUnit.LOADER);

            InitializeForm();
        }
        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void BtnTeachLoader_Click(object sender, EventArgs e)
        {
            m_Teach.SetUnit((int)ETeachUnit.LOADER);
            m_Teach.ShowDialog();
        }

        private void BtnTeachPushPull_Click(object sender, EventArgs e)
        {
            m_Teach.SetUnit((int)ETeachUnit.PUSHPULL);
            m_Teach.ShowDialog();
        }

        private void BtnTeachSpinner1_Click(object sender, EventArgs e)
        {
            m_Teach.SetUnit((int)ETeachUnit.CLEANER1);
            m_Teach.ShowDialog();
        }

        private void BtnTeachSpinner2_Click(object sender, EventArgs e)
        {
            m_Teach.SetUnit((int)ETeachUnit.CLEANER2);
            m_Teach.ShowDialog();
        }

        private void BtnTeachHandler_Click(object sender, EventArgs e)
        {
            m_Teach.SetUnit((int)ETeachUnit.HANDLER);
            m_Teach.ShowDialog();
        }

        private void BtnTeachStage_Click(object sender, EventArgs e)
        {
            m_Teach.SetUnit((int)ETeachUnit.STAGE);
            m_Teach.ShowDialog();
        }

        private void BtnCameraScanner_Click(object sender, EventArgs e)
        {
            m_Teach.SetUnit((int)ETeachUnit.CAMERA);  
            m_Teach.ShowDialog();
        }

        private void BtnScanner_Click(object sender, EventArgs e)
        {
            m_Teach.SetUnit((int)ETeachUnit.SCANNER);
            m_Teach.ShowDialog();
        }
    }
}
