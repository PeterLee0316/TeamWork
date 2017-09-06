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
using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Error;

namespace Core.UI
{
    public partial class FormTest : Form
    {
        List<MObject> CtrlList = new List<MObject>();
        List<MObject> ProcessList = new List<MObject>();

        public FormTest()
        {
            InitializeComponent();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            comboProcess.Items.Clear();
            comboProcess.Items.Add(EThreadChannel.TrsAutoManager);
            comboProcess.Items.Add(EThreadChannel.TrsLoader     );
            comboProcess.Items.Add(EThreadChannel.TrsPushPull   );
            comboProcess.Items.Add(EThreadChannel.TrsStage1     );
            comboProcess.Items.Add(EThreadChannel.TrsSpinner1   );
            comboProcess.Items.Add(EThreadChannel.TrsSpinner2   );
            comboProcess.Items.Add(EThreadChannel.TrsHandler    );
            comboProcess.SelectedIndex = 0;

            ProcessList.Add(CMainFrame.Core.m_trsAutoManager);
            ProcessList.Add(CMainFrame.Core.m_trsLoader     );
            ProcessList.Add(CMainFrame.Core.m_trsPushPull   );
            ProcessList.Add(CMainFrame.Core.m_trsStage1     );
            ProcessList.Add(CMainFrame.Core.m_trsSpinner1   );
            ProcessList.Add(CMainFrame.Core.m_trsSpinner2   );
            ProcessList.Add(CMainFrame.Core.m_trsHandler    );

            comboControll.Items.Clear();
            comboControll.Items.Add("DataManager         ");
            comboControll.Items.Add("CtrlLoader          ");
            comboControll.Items.Add("CtrlPushPull        ");
            comboControll.Items.Add("CtrlStage1          ");
            comboControll.Items.Add("CtrlSpinner1        ");
            comboControll.Items.Add("CtrlSpinner2        ");
            comboControll.Items.Add("CtrlHandler         ");
            comboControll.Items.Add("MeElevator          ");
            comboControll.Items.Add("MeUpperHandler      ");
            comboControll.Items.Add("MeLowerHandler      ");
            comboControll.Items.Add("MeStage             ");
            comboControll.Items.Add("MePushPull          ");
            comboControll.Items.Add("MeSpinner1          ");
            comboControll.Items.Add("MeSpinner2          ");
            comboControll.Items.Add("PushPullGripperCyl  ");
            comboControll.Items.Add("PushPullUDCyl       ");
            comboControll.Items.Add("Spinner1UDCyl       ");
            comboControll.Items.Add("Spinner1DICyl       ");
            comboControll.Items.Add("Spinner1PVACyl      ");
            comboControll.Items.Add("Spinner2UDCyl       ");
            comboControll.Items.Add("Spinner2DICyl       ");
            comboControll.Items.Add("Spinner2PVACyl      ");
            comboControll.Items.Add("StageClamp1         ");
            comboControll.Items.Add("StageClamp2         ");
            comboControll.Items.Add("Stage1Vac           ");
            comboControll.Items.Add("UHandlerSelfVac     ");
            comboControll.Items.Add("UHandlerFactoryVac  ");
            comboControll.Items.Add("LHandlerSelfVac     ");
            comboControll.Items.Add("LHandlerFactoryVac  ");
            comboControll.Items.Add("Spinner1Vac         ");
            comboControll.Items.Add("Spinner2Vac         ");
            comboControll.Items.Add("AxStage1            ");
            comboControll.Items.Add("AxLoader            ");
            comboControll.Items.Add("AxPushPull          ");
            comboControll.Items.Add("AxCentering1        ");
            comboControll.Items.Add("AxRotate1           ");
            comboControll.Items.Add("AxCleanNozzle1      ");
            comboControll.Items.Add("AxCoatNozzle1       ");
            comboControll.Items.Add("AxCentering2        ");
            comboControll.Items.Add("AxRotate2           ");
            comboControll.Items.Add("AxCleanNozzle2      ");
            comboControll.Items.Add("AxCoatNozzle2       ");
            comboControll.Items.Add("AxUpperHandler      ");
            comboControll.Items.Add("AxLowerHandler      ");
            comboControll.Items.Add("AxCamera1           ");
            comboControll.Items.Add("AxLaser1            ");
            comboControll.SelectedIndex = 0;

            CtrlList.Add((MObject)CMainFrame.DataManager         );
            CtrlList.Add((MObject)CMainFrame.Core.m_ctrlLoader          );
            CtrlList.Add((MObject)CMainFrame.Core.m_ctrlPushPull        );
            CtrlList.Add((MObject)CMainFrame.Core.m_ctrlStage1          );
            CtrlList.Add((MObject)CMainFrame.Core.m_ctrlSpinner1        );
            CtrlList.Add((MObject)CMainFrame.Core.m_ctrlSpinner2        );
            CtrlList.Add((MObject)CMainFrame.Core.m_ctrlHandler         );
            CtrlList.Add((MObject)CMainFrame.Core.m_MeElevator          );
            CtrlList.Add((MObject)CMainFrame.Core.m_MeUpperHandler      );
            CtrlList.Add((MObject)CMainFrame.Core.m_MeLowerHandler      );
            CtrlList.Add((MObject)CMainFrame.Core.m_MeStage             );
            CtrlList.Add((MObject)CMainFrame.Core.m_MePushPull          );
            CtrlList.Add((MObject)CMainFrame.Core.m_MeSpinner1          );
            CtrlList.Add((MObject)CMainFrame.Core.m_MeSpinner2          );
            CtrlList.Add((MObject)CMainFrame.Core.m_PushPullGripperCyl  );
            CtrlList.Add((MObject)CMainFrame.Core.m_PushPullUDCyl       );
            CtrlList.Add((MObject)CMainFrame.Core.m_Spinner1UDCyl       );
            CtrlList.Add((MObject)CMainFrame.Core.m_Spinner1DICyl       );
            CtrlList.Add((MObject)CMainFrame.Core.m_Spinner1PVACyl      );
            CtrlList.Add((MObject)CMainFrame.Core.m_Spinner2UDCyl       );
            CtrlList.Add((MObject)CMainFrame.Core.m_Spinner2DICyl       );
            CtrlList.Add((MObject)CMainFrame.Core.m_Spinner2PVACyl      );
            CtrlList.Add((MObject)CMainFrame.Core.m_StageClamp1         );
            CtrlList.Add((MObject)CMainFrame.Core.m_StageClamp2         );
            CtrlList.Add((MObject)CMainFrame.Core.m_Stage1Vac           );
            CtrlList.Add((MObject)CMainFrame.Core.m_UHandlerSelfVac     );
            CtrlList.Add((MObject)CMainFrame.Core.m_UHandlerFactoryVac  );
            CtrlList.Add((MObject)CMainFrame.Core.m_LHandlerSelfVac     );
            CtrlList.Add((MObject)CMainFrame.Core.m_LHandlerFactoryVac  );
            CtrlList.Add((MObject)CMainFrame.Core.m_Spinner1Vac         );
            CtrlList.Add((MObject)CMainFrame.Core.m_Spinner2Vac         );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxStage1            );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxLoader            );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxPushPull          );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxCentering1        );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxRotate1           );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxCleanNozzle1      );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxCoatNozzle1       );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxCentering2        );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxRotate2           );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxCleanNozzle2      );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxCoatNozzle2       );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxUpperHandler      );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxLowerHandler      );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxCamera1           );
            CtrlList.Add((MObject)CMainFrame.Core.m_AxScannerZ1         );

            TextTest1.Text = "0";
        }

        private void buttonAdv1_Click(object sender, EventArgs e)
        {
            int alarm; // = CMainFrame.Core.m_ctrlPushPull.GenerateErrorCode(Convert.ToInt32(TextAlarmIndex.Text));
            //CMainFrame.Core.m_trsPushPull.ReportAlarm(alarm);

            if(CtrlList[comboControll.SelectedIndex] != null)
            {
                alarm = CtrlList[comboControll.SelectedIndex].GenerateErrorCode(Convert.ToInt32(TextAlarmIndex.Text));
                ((MWorkerThread)ProcessList[comboProcess.SelectedIndex]).ReportAlarm(alarm);
            }
        }

        private void LabelTest1_Click(object sender, EventArgs e)
        {
      
            int servoNo = Convert.ToInt32(TextTest1.Text);
            CMainFrame.Core.m_YMC.GetServoStatus(servoNo);
        }
    }
}
