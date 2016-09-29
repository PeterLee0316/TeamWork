﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Thread.ETrsStage1Step;
using static LWDicer.Layers.DEF_Thread.EThreadMessage;
using static LWDicer.Layers.DEF_Thread.EThreadChannel;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_LCNet;

namespace LWDicer.Layers
{
    public class CTrsStage1RefComp
    {
        public MCtrlStage1 ctrlStage1;

        public override string ToString()
        {
            return $"CTrsStage1RefComp : {ctrlStage1}";
        }
    }

    public class CTrsStage1Data
    {
    }

    public class MTrsStage1 : MWorkerThread
    {
        private CTrsStage1RefComp m_RefComp;
        private CTrsStage1Data m_Data;

        // Message 변수
        bool m_bWorkbench_LoadRequest;
        bool m_bWorkbench_LoadComplete;

        bool m_bAuto_PanelSupplyStop;

        public MTrsStage1(CObjectInfo objInfo, EThreadChannel SelfChannelNo, MDataManager DataManager, ELCNetUnitPos LCNetUnitPos,
            CTrsStage1RefComp refComp, CTrsStage1Data data)
             : base(objInfo, SelfChannelNo, DataManager, LCNetUnitPos)
        {
            m_RefComp = refComp;
            SetData(data);
            TSelf = (int)EThreadUnit.STAGE1;
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CTrsStage1Data source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsStage1Data target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        override public string GetStep1()
        {
            ETrsStage1Step cnvt = (ETrsStage1Step)Enum.Parse(typeof(ETrsStage1Step), ThreadStep1.ToString());
            return cnvt.ToString();
        }

        public override int Initialize()
        {
            // Do initialize
            InitializeMsg();
            InitializeInterface();

            // Do Action
            int iResult = m_RefComp.ctrlStage1.Initialize();
            if (iResult != SUCCESS) return iResult;

            int iStep1 = (int)TRS_STAGE1_MOVETO_WAIT_POS;

            // finally
            SetStep1(iStep1);

            return base.Initialize();
        }

        public int InitializeMsg()
        {
            m_bWorkbench_LoadComplete = false;
            m_bWorkbench_LoadRequest = false;

            return SUCCESS;
        }

        private int InitializeInterface()
        {

            return SUCCESS;
        }
        #endregion

        protected override int ProcessMsg(MEvent evnt)
        {
              Debug.WriteLine($"[{ToString()}] received message : {evnt.ToThreadMessage()}");
            switch (evnt.Msg)
            {
                // if need to change response for common message, then add case state here.
                default:
                    base.ProcessMsg(evnt);
                    break;

                case (int)MSG_PANEL_SUPPLY_STOP:
                    m_bAuto_PanelSupplyStop = true;
                    break;

                case (int)MSG_PANEL_SUPPLY_START:
                    m_bAuto_PanelSupplyStop = false;
                    break;

            }
            return 0;
        }

        protected override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bStatus, bStatus1, bStatus2;
            EProcessPhase processPhase;

            while (true)
            {
                // if thread has been suspended
                if (IsAlive == false)
                {
                    Sleep(ThreadSuspendedTime);
                    continue;
                }

                // check message from other thread
                CheckMsg(1);

                switch (RunStatus)
                {
                    case EAutoRunStatus.STS_MANUAL: // Manual Mode
                        //m_RefComp.ctrlStage1.SetAutoManual(EAutoManual.MANUAL);
                        break;

                    case EAutoRunStatus.STS_ERROR_STOP: // Error Stop
                        break;

                    case EAutoRunStatus.STS_STEP_STOP: // Step Stop
                        break;

                    case EAutoRunStatus.STS_RUN_READY: // Run Ready
                        break;

                    case EAutoRunStatus.STS_CYCLE_STOP: // Cycle Stop
                        //if (ThreadStep1 == TRS_STAGE1_MOVETO_LOAD_POS)
                        break;

                    case EAutoRunStatus.STS_RUN: // auto run
                        m_RefComp.ctrlStage1.SetAutoManual(EAutoManual.AUTO);

                        // Do Thread Step
                        switch (ThreadStep1)
                        {
                            case (int)TRS_STAGE1_MOVETO_LOAD_POS:
                                if (m_bAuto_PanelSupplyStop) break;

                                //        PostMsg(TrsAutoManager, (int)MSG_STAGE_LOADING_END);

                                //        iResult = m_RefComp.ctrlStage1.MoveToLoadPos();
                                //        if (iResult != SUCCESS) { SendAlarmTo(iResult); break; }

                                //SetStep1((int)TRS_STAGE1_WAIT_MOVETO_LOAD);
                                break;

                                //    case (int)TRS_STAGE1_WAIT_MOVETO_LOAD:
                                //        if (m_bAuto_PanelSupplyStop) break;

                                //    SetStep1((int)TRS_STAGE1_LOAD_PANEL);
                                //    break;

                                //    case (int)TRS_STAGE1_LOAD_PANEL: //2

                                //        PostMsg(TrsAutoManager, (int)MSG_PANEL_INPUT);
                                //        PostMsg(TrsAutoManager, (int)MSG_STAGE_LOADING_END);

                                //    SetStep1((int)TRS_STAGE1_CAMERA_MARK_POS);
                                //    break;

                                //    case (int)TRS_STAGE1_UNLOAD_COMPLETE: //7
                                //                                     //				if(!m_bWorkbench_SafetyPos) break;

                                //        iResult = m_RefComp.ctrlStage1.MoveToWaitPos();
                                //        if (iResult != SUCCESS) { SendAlarmTo(iResult); break; }

                                //        //PostMsg(TrsWorkbench, MSG_STAGE1_WORKBENCH_SAFETY_POS);

                                //    SetStep1((int)TRS_STAGE1_MOVETO_LOAD_POS);
                                //    break;

                                //    default:
                                //        break;
                                //}
                        }
                        break;

                    default:
                        break;
                }

                Sleep(ThreadSleepTime);
                //Debug.WriteLine(ToString() + " Thread running..");
            }

        }
    }

}
