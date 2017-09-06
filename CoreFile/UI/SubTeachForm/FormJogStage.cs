using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Core.Layers.DEF_System;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_DataManager;
using static Core.Layers.DEF_Motion;
using static Core.Layers.MYaskawa;

using static Core.Layers.DEF_Yaskawa;
using static Core.Layers.DEF_ACS;
using static Core.Layers.DEF_Error;

namespace Core.UI
{
    public partial class FormJogStage : Form
    {
        private enum EStageMoveType 
        {
            JOG,
            STEP_INDEX,
            SCREEN_INDEX,
            DIE_INDEX
        }
        private enum EJogSpeed
        {
            SLOW,
            FAST
        }

        private EStageMoveType StageMoveType;
        private EJogSpeed JogSpeed;

        public FormJogStage()
        {
            InitializeComponent();
        }

        private void FormJogStage_Load(object sender, EventArgs e)
        {
            ChangeStageMode(EStageMoveType.DIE_INDEX);
            ChangeJogSpeed(EJogSpeed.SLOW);
        }
        
        private void BtnSelectStepIndex_Click(object sender, EventArgs e)
        {
            ChangeStageMode(EStageMoveType.STEP_INDEX);
        }

        private void BtnSelectScreenIndex_Click(object sender, EventArgs e)
        {
            ChangeStageMode(EStageMoveType.SCREEN_INDEX);
        }
        private void BtnSelectDieIndex_Click(object sender, EventArgs e)
        {
            ChangeStageMode(EStageMoveType.DIE_INDEX);
        }

        private void ChangeStageMode(EStageMoveType pMode)
        {
            switch (pMode)
            {
                case EStageMoveType.STEP_INDEX:
                    BtnSelectStepIndex.BackColor = SystemColors.Highlight;
                    BtnSelectScreenIndex.BackColor = SystemColors.Control;
                    BtnSelectDieIndex.BackColor       = SystemColors.Control;
                    break;
                case EStageMoveType.SCREEN_INDEX:
                    BtnSelectStepIndex.BackColor = SystemColors.Control;
                    BtnSelectScreenIndex.BackColor = SystemColors.Highlight;
                    BtnSelectDieIndex.BackColor       = SystemColors.Control;
                    break;
                case EStageMoveType.DIE_INDEX:
                    BtnSelectStepIndex.BackColor = SystemColors.Control;
                    BtnSelectScreenIndex.BackColor = SystemColors.Control;
                    BtnSelectDieIndex.BackColor       = SystemColors.Highlight;
                    break;
            }

            StageMoveType = pMode;
        }

        private void BtnStageMoveX_F_Click(object sender, EventArgs e)
        {
            MoveAxis(EACS_Axis.STAGE1_X, true);            
        }

        private void BtnStageMoveX_B_Click(object sender, EventArgs e)
        {
            MoveAxis(EACS_Axis.STAGE1_X, false);
        }

        private void BtnStageMoveY_F_Click(object sender, EventArgs e)
        {
            MoveAxis(EACS_Axis.STAGE1_Y, true);
        }

        private void BtnStageMoveY_B_Click(object sender, EventArgs e)
        {
            MoveAxis(EACS_Axis.STAGE1_Y, false);
        }

        private void BtnStageMoveT_CCW_Click(object sender, EventArgs e)
        {
            MoveAxis(EACS_Axis.STAGE1_T, true);
        }

        private void BtnStageMoveT_CW_Click(object sender, EventArgs e)
        {
            MoveAxis(EACS_Axis.STAGE1_T, false);
        }        

        private void MoveAxis(EACS_Axis SelectedAxis, bool bDirect)
        {
            int iResult = SUCCESS;
            
            // Step Index Move
            if (StageMoveType == EStageMoveType.STEP_INDEX)
            {                
                if (SelectedAxis == EACS_Axis.STAGE1_X)
                {
                    if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveStageRelativeX(0.001,true);
                    else         iResult = CMainFrame.Core.m_ctrlStage1.MoveStageRelativeX(0.001,false);
                }

                if (SelectedAxis == EACS_Axis.STAGE1_Y)
                {
                    if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveStageRelativeY(0.001, true);
                    else         iResult = CMainFrame.Core.m_ctrlStage1.MoveStageRelativeY(0.001, false);
                }

                if (SelectedAxis == EACS_Axis.STAGE1_T)
                {
                    if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveStageRelativeT(0.001, true);
                    else         iResult = CMainFrame.Core.m_ctrlStage1.MoveStageRelativeT(0.001, false);
                }
            }

            // Screen Index Move
            else if (StageMoveType == EStageMoveType.SCREEN_INDEX)
            {
                if (CMainFrame.Core.m_ctrlStage1.CheckMicroCam())
                {

                    if (SelectedAxis == EACS_Axis.STAGE1_X)
                    {
                        if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveMicroScreenPlusX();
                        else         iResult = CMainFrame.Core.m_ctrlStage1.MoveMicroScreenMinusX();
                    }

                    if (SelectedAxis == EACS_Axis.STAGE1_Y)
                    {
                        if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveMicroScreenPlusY();
                        else         iResult = CMainFrame.Core.m_ctrlStage1.MoveMicroScreenMinusY();
                    }

                    if (SelectedAxis == EACS_Axis.STAGE1_T)
                    {
                        if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveMicroScreenPlusT();
                        else         iResult = CMainFrame.Core.m_ctrlStage1.MoveMicroScreenMinusT();
                    }
                }

                if (CMainFrame.Core.m_ctrlStage1.CheckMacroCam())
                {

                    if (SelectedAxis == EACS_Axis.STAGE1_X)
                    {
                        if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveMacroScreenPlusX();
                        else         iResult = CMainFrame.Core.m_ctrlStage1.MoveMacroScreenMinusX();
                    }

                    if (SelectedAxis == EACS_Axis.STAGE1_Y)
                    {
                        if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveMacroScreenPlusY();
                        else         iResult = CMainFrame.Core.m_ctrlStage1.MoveMacroScreenMinusY();
                    }

                    if (SelectedAxis == EACS_Axis.STAGE1_T)
                    {
                        if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveMacroScreenPlusT();
                        else         iResult = CMainFrame.Core.m_ctrlStage1.MoveMacroScreenMinusT();
                    }
                }
            }

            // Die Index Move
            else if (StageMoveType == EStageMoveType.DIE_INDEX)
            {
                if (SelectedAxis == EACS_Axis.STAGE1_X)
                {
                    if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveIndexPlusX();
                    else         iResult = CMainFrame.Core.m_ctrlStage1.MoveIndexMinusX();
                }

                if (SelectedAxis == EACS_Axis.STAGE1_Y)
                {
                    if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveIndexPlusY();
                    else         iResult = CMainFrame.Core.m_ctrlStage1.MoveIndexMinusY();
                }

                if (SelectedAxis == EACS_Axis.STAGE1_T)
                {
                    if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.MoveIndexPlusT();
                    else         iResult = CMainFrame.Core.m_ctrlStage1.MoveIndexMinusT();
                }
            }
            if (iResult != SUCCESS)
            {
                CMainFrame.DisplayAlarm(iResult);
            }
        }

        private void BtnJogSlow_Click(object sender, EventArgs e)
        {
            ChangeJogSpeed(EJogSpeed.SLOW);
        }

        private void BtnJogFast_Click(object sender, EventArgs e)
        {
            ChangeJogSpeed(EJogSpeed.FAST);
        }

        private void ChangeJogSpeed(EJogSpeed speed)
        {
            if (speed == EJogSpeed.SLOW)
            {
                BtnJogSlow.BackColor = SystemColors.Highlight;
                BtnJogFast.BackColor = SystemColors.Control;

                JogSpeed = EJogSpeed.SLOW;

            }

            if (speed == EJogSpeed.FAST)
            {
                BtnJogSlow.BackColor = SystemColors.Control;
                BtnJogFast.BackColor = SystemColors.Highlight;

                JogSpeed = EJogSpeed.FAST;
            }
        }

        private void BtnAxisJogX_F_MouseDown(object sender, MouseEventArgs e)
        {
            AxisJog(EACS_Axis.STAGE1_X, true);
        }

        private void BtnAxisJogX_B_MouseDown(object sender, MouseEventArgs e)
        {
            AxisJog(EACS_Axis.STAGE1_X, false);
        }

        private void BtnAxisJogY_F_MouseDown(object sender, MouseEventArgs e)
        {
            AxisJog(EACS_Axis.STAGE1_Y, true);
        }

        private void BtnAxisJogY_B_MouseDown(object sender, MouseEventArgs e)
        {
            AxisJog(EACS_Axis.STAGE1_Y, false);
        }

        private void BtnAxisJogT_CW_MouseDown(object sender, MouseEventArgs e)
        {
            AxisJog(EACS_Axis.STAGE1_T, true);
        }

        private void BtnAxisJogT_CCW_MouseDown(object sender, MouseEventArgs e)
        {
            AxisJog(EACS_Axis.STAGE1_T, false);
        }

        private void BtnAxisJogX_F_MouseUp(object sender, MouseEventArgs e)
        {
            AxisStop(EACS_Axis.STAGE1_X);
        }

        private void BtnAxisJogX_B_MouseUp(object sender, MouseEventArgs e)
        {
            AxisStop(EACS_Axis.STAGE1_X);
        }

        private void BtnAxisJogY_F_MouseUp(object sender, MouseEventArgs e)
        {
            AxisStop(EACS_Axis.STAGE1_Y);
        }

        private void BtnAxisJogY_B_MouseUp(object sender, MouseEventArgs e)
        {
            AxisStop(EACS_Axis.STAGE1_Y);
        }

        private void BtnAxisJogT_CW_MouseUp(object sender, MouseEventArgs e)
        {
            AxisStop(EACS_Axis.STAGE1_T);
        }

        private void BtnAxisJogT_CCW_MouseUp(object sender, MouseEventArgs e)
        {
            AxisStop(EACS_Axis.STAGE1_T);
        }

        private int AxisJog(EACS_Axis SelectedAxis, bool bDirect)
        {
            int iResult = 0;
            bool IsFast = JogSpeed == EJogSpeed.FAST ? true : false;


            if (SelectedAxis == EACS_Axis.STAGE1_X)
            {
                if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.JogMovePlusX(IsFast);
                else         iResult = CMainFrame.Core.m_ctrlStage1.JogMoveMinusX(IsFast);
            }

            if (SelectedAxis == EACS_Axis.STAGE1_Y)
            {
                if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.JogMovePlusY(IsFast);
                else         iResult = CMainFrame.Core.m_ctrlStage1.JogMoveMinusY(IsFast);
            }

            if (SelectedAxis == EACS_Axis.STAGE1_T)
            {
                if (bDirect) iResult = CMainFrame.Core.m_ctrlStage1.JogMovePlusT(IsFast);
                else         iResult = CMainFrame.Core.m_ctrlStage1.JogMoveMinusT(IsFast);
            }
            
            return iResult;
        }

        private int AxisStop(EACS_Axis SelectedAxis)
        {
            return CMainFrame.Core.m_ctrlStage1.JogStageStop((int)SelectedAxis);            
        }

        private void BtnStageJogT_CCW_Click(object sender, EventArgs e)
        {

        }

        
    }
}
