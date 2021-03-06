using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static Core.Layers.DEF_Thread;
using static Core.Layers.DEF_Thread.EThreadStep;
using static Core.Layers.DEF_Thread.EThreadMessage;
using static Core.Layers.DEF_Thread.EThreadChannel;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_LCNet;
using static Core.Layers.DEF_CtrlStage;

namespace Core.Layers
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
        public bool ThreadHandshake_byOneStep = true;
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

        public override int Initialize()
        {
            // Do initialize
            InitializeMsg();
            InitializeInterface();

            // Do Action
            int iResult = m_RefComp.ctrlStage1.Initialize();
            if (iResult != SUCCESS) return iResult;

            EThreadStep iStep1 = TRS_STAGE1_MOVETO_WAIT_POS;

            // finally
            SetStep(iStep1);

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
            EProcessPhase tPhase = GetMyNextWorkPhase();

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

                        // Do Thread Step
                        switch (ThreadStep)
                        {
                            case TRS_STAGE1_MOVETO_WAIT_POS:

                                SetStep(TRS_STAGE1_WAITFOR_MESSAGE);
                                break;

                            case TRS_STAGE1_WAITFOR_MESSAGE:
                                iResult = m_RefComp.ctrlStage1.IsObjectDetected(out bStatus);
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                tPhase = GetMyNextWorkPhase();
                                if (bStatus == true) // if wafer detected
                                {
                                    switch(tPhase)
                                    {
                                        case EProcessPhase.MACRO_ALIGN:
                                            SetStep(TRS_STAGE1_MOVETO_MACRO_ALIGN_POS);
                                            break;

                                        case EProcessPhase.MICRO_ALIGN:
                                            SetStep(TRS_STAGE1_MOVETO_MICRO_ALIGN_POS);
                                            break;

                                        case EProcessPhase.DICING:
                                            SetStep(TRS_STAGE1_MOVETO_DICING_POS);
                                            break;

                                        case EProcessPhase.STAGE_WAIT_UNLOAD:
                                        case EProcessPhase.STAGE_UNLOAD:
                                            if(GetMyWorkPiece().Process[(int)tPhase].IsStarted == false)
                                            {
                                                GetMyWorkPiece().StartPhase(tPhase); // 대기 시작
                                            }

                                     
                                            GetMyWorkPiece().FinishPhase(EProcessPhase.STAGE_WAIT_UNLOAD);
                                            GetMyWorkPiece().StartPhase(EProcessPhase.STAGE_UNLOAD);
                                            SetStep(TRS_STAGE1_UNLOADING_TO_HANDLER_ONESTEP);
                                            break;
                                    }
                                } else // if not detected
                                {                                    
                                    SetStep(TRS_STAGE1_LOADING_FROM_HANDLER_ONESTEP);
                                }
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process load with handler
                            case TRS_STAGE1_LOADING_FROM_HANDLER_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_STAGE1_MOVETO_LOAD_POS);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.HANDLER;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();

                                TInterface.Stage1_Handler_BeginHandshake_Load = true;
                                while (TInterface.Handler_Stage1_BeginHandshake_Unload == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                                    {
                                        TInterface.TimeOver[TSelf] = true;
                                        break;
                                    }
                                    Sleep(ThreadSleepTime);
                                }
                                if (TInterface.ErrorOccured[TOpponent]) break;
                                if (TInterface.TimeOver[TSelf])
                                {
                                    // do something, if it is needed
                                    TInterface.ResetInterface(TSelf);
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_STAGE1_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1

                                TInterface.Stage1_Handler_LoadStep1 = true;

                                while (TInterface.Handler_Stage1_UnloadStep1 == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                                    {
                                        TInterface.TimeOver[TSelf] = true;
                                        break;
                                    }
                                    Sleep(ThreadSleepTime);
                                }
                                if (TInterface.ErrorOccured[TOpponent]) break;
                                if (TInterface.TimeOver[TSelf])
                                {
                                    // do something, if it is needed
                                    TInterface.ResetInterface(TSelf);
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_STAGE1_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.Stage1_Handler_LoadStep2 = true;

                                while (TInterface.Handler_Stage1_UnloadStep2 == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                                    {
                                        TInterface.TimeOver[TSelf] = true;
                                        break;
                                    }
                                    Sleep(ThreadSleepTime);
                                }
                                if (TInterface.ErrorOccured[TOpponent]) break;
                                if (TInterface.TimeOver[TSelf])
                                {
                                    // do something, if it is needed
                                    TInterface.ResetInterface(TSelf);
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_STAGE1_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }


                                // finish
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.Stage1_Handler_FinishHandshake_Load = true;
                                while (TInterface.Handler_Stage1_FinishHandshake_Unload == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                                    {
                                        TInterface.TimeOver[TSelf] = true;
                                        break;
                                    }
                                    Sleep(ThreadSleepTime);
                                }
                                if (TInterface.ErrorOccured[TOpponent]) break;
                                if (TInterface.TimeOver[TSelf])
                                {
                                    // do something, if it is needed
                                    TInterface.ResetInterface(TSelf);
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_STAGE1_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);
                                //GetMyWorkPiece().FinishPhase(tPhase);
                                DataManager.ChangeWorkPieceUnit(ELCNetUnitPos.UPPER_HANDLER, ELCNetUnitPos.STAGE1);

                                SetStep(TRS_STAGE1_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // process unload with handler
                            case TRS_STAGE1_UNLOADING_TO_HANDLER_ONESTEP:
                                // branch
                                if (m_Data.ThreadHandshake_byOneStep == false)
                                {
                                    SetStep(TRS_STAGE1_MOVETO_UNLOAD_POS);
                                    break;
                                }

                                // init
                                TInterface.ResetInterface(TSelf);
                                TOpponent = (int)EThreadUnit.HANDLER;
                                if (TInterface.ErrorOccured[TOpponent]) break;

                                // begin
                                TTimer.StartTimer();
                                //GetMyWorkPiece().StartPhase(EProcessPhase.STAGE1_UNLOAD_TO_HANDLER);

                                TInterface.Stage1_Handler_BeginHandshake_Unload = true;
                                while (TInterface.Handler_Stage1_BeginHandshake_Load == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                                    {
                                        TInterface.TimeOver[TSelf] = true;
                                        break;
                                    }
                                    Sleep(ThreadSleepTime);
                                }
                                if (TInterface.ErrorOccured[TOpponent]) break;
                                if (TInterface.TimeOver[TSelf])
                                {
                                    // do something, if it is needed
                                    TInterface.ResetInterface(TSelf);
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_STAGE1_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step1
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.Stage1_Handler_UnloadStep1 = true;

                                while (TInterface.Handler_Stage1_LoadStep1 == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                                    {
                                        TInterface.TimeOver[TSelf] = true;
                                        break;
                                    }
                                    Sleep(ThreadSleepTime);
                                }
                                if (TInterface.ErrorOccured[TOpponent]) break;
                                if (TInterface.TimeOver[TSelf])
                                {
                                    // do something, if it is needed
                                    TInterface.ResetInterface(TSelf);
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_STAGE1_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // step2
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.Stage1_Handler_UnloadStep2 = true;

                                while (TInterface.Handler_Stage1_LoadStep2 == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                                    {
                                        TInterface.TimeOver[TSelf] = true;
                                        break;
                                    }
                                    Sleep(ThreadSleepTime);
                                }
                                if (TInterface.ErrorOccured[TOpponent]) break;
                                if (TInterface.TimeOver[TSelf])
                                {
                                    // do something, if it is needed
                                    TInterface.ResetInterface(TSelf);
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_STAGE1_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                // finish
                                if (iResult != SUCCESS) { ReportAlarm(iResult); break; }

                                TInterface.Stage1_Handler_FinishHandshake_Unload = true;
                                while (TInterface.Handler_Stage1_FinishHandshake_Load == false)
                                {
                                    if (TInterface.ErrorOccured[TOpponent]) break;
                                    if (TTimer.MoreThan(TInterface.TimeLimit, ETimeType.SECOND))
                                    {
                                        TInterface.TimeOver[TSelf] = true;
                                        break;
                                    }
                                    Sleep(ThreadSleepTime);
                                }
                                if (TInterface.ErrorOccured[TOpponent]) break;
                                if (TInterface.TimeOver[TSelf])
                                {
                                    // do something, if it is needed
                                    TInterface.ResetInterface(TSelf);
                                    ReportAlarm(GenerateErrorCode(ERR_TRS_STAGE1_INTERFACE_TIMELIMIT_OVER));
                                    break;
                                }

                                GetMyWorkPiece().FinishPhase(EProcessPhase.STAGE_UNLOAD);
                                DataManager.ChangeWorkPieceUnit(ELCNetUnitPos.STAGE1, ELCNetUnitPos.LOWER_HANDLER);

                                // reset
                                TInterface.ResetInterface(TSelf);
                                // 상대편의 마지막 신호는 keepontime동안 유지되기때문에, 먼저 끝나는쪽에서 상대편것도 리셋 필요
                                TInterface.ResetInterface(TOpponent);

                                SetStep(TRS_STAGE1_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // align
                            case TRS_STAGE1_MOVETO_MACRO_ALIGN_POS:
                                GetMyWorkPiece().StartPhase(EProcessPhase.MACRO_ALIGN);

                                SetStep(TRS_STAGE1_DO_MACRO_ALIGN);
                                break;

                            case TRS_STAGE1_DO_MACRO_ALIGN:

                                GetMyWorkPiece().FinishPhase(EProcessPhase.MACRO_ALIGN);
                                SetStep(TRS_STAGE1_WAITFOR_MESSAGE);
                                break;

                            case TRS_STAGE1_MOVETO_MICRO_ALIGN_POS:
                                GetMyWorkPiece().StartPhase(EProcessPhase.MICRO_ALIGN);

                                SetStep(TRS_STAGE1_DO_MICRO_ALIGN);
                                break;

                            case TRS_STAGE1_DO_MICRO_ALIGN:

                                GetMyWorkPiece().FinishPhase(EProcessPhase.MICRO_ALIGN);
                                SetStep(TRS_STAGE1_WAITFOR_MESSAGE);
                                break;

                            ///////////////////////////////////////////////////////////////////
                            // dicing
                            case TRS_STAGE1_MOVETO_DICING_POS:
                                GetMyWorkPiece().StartPhase(EProcessPhase.DICING);

                                SetStep(TRS_STAGE1_DO_DICING);
                                break;

                            case TRS_STAGE1_DO_DICING:

                                GetMyWorkPiece().FinishPhase(EProcessPhase.DICING);
                                SetStep(TRS_STAGE1_WAITFOR_MESSAGE);
                                break;

                            default:
                                break;
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
