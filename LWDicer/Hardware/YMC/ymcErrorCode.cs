//===========================================================
// C#
//
// Error code constant definition module for ymcPACPI.Dll ÅÉymcErrorCode.csÅÑ
//
// Version (date)		ÅF  Ver.1.0.0.0 (12/02/27)
//
// Remarks              ÅF	
//
//===========================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MotionYMC
{
	public partial class CMotionAPI
	{
	    public const UInt32 MP_SUCCESS                          = 0x00000000;	/* API function normal completion                                                                                                                      */
	    public const UInt32 MP_FAIL 							= 0x4000FFFF;   /* API function erroneous completion                                                                                                                   */
	    public const UInt32 WDT_OVER_ERR 						= 0x81000001;   /*                                                                                                                                                     */
	    public const UInt32 MANUAL_RESET_ERR 					= 0x82000020;   /* Manual reset error                                                                                                                                  */
	    public const UInt32 TLB_MLTHIT_ERR 						= 0x82000140;   /* TLB multi hit error                                                                                                                                 */
	    public const UInt32 UBRK_ERR 							= 0x820001E0;   /* User break execution error                                                                                                                          */
	    public const UInt32 ADR_RD_ERR 							= 0x820000E0;   /* Address read error error                                                                                                                            */
	    public const UInt32 TLB_MIS_RD_ERR 						= 0x82000040;   /* TLB read mis error                                                                                                                                  */
	    public const UInt32 TLB_PROTECTION_RD_ERR 				= 0x820000A0;   /* TLB protection read vaiolation error                                                                                                                */
	    public const UInt32 GENERAL_INVALID_INS_ERR 			= 0x82000180;   /* General invalid instruction error                                                                                                                   */
	    public const UInt32 SLOT_ERR 							= 0x820001A0;   /* Slot invalid instruction error                                                                                                                      */
	    public const UInt32 GENERAL_FPU_DISABLE_ERR 			= 0x82000800;   /* General FPU disable error                                                                                                                           */
	    public const UInt32 SLOT_FPU_ERR 						= 0x82000820;   /* Slot FPU exception error                                                                                                                            */
	    public const UInt32 ADR_WR_ERR 							= 0x82000100;   /* Data address write error error                                                                                                                      */
	    public const UInt32 TLB_MIS_WR_ERR 						= 0x82000060;   /* TLB write mis error                                                                                                                                 */
	    public const UInt32 TLB_PROTECTION_WR_ERR 				= 0x820000C0;   /* TLB protection write vaiolation error                                                                                                               */
	    public const UInt32 FPU_EXP_ERR 						= 0x82000120;   /* FPU exception error                                                                                                                                 */
	    public const UInt32 INITIAL_PAGE_EXP_ERR 				= 0x82000080;   /* Initial page write exception error                                                                                                                  */
	    public const UInt32 ROM_ERR 							= 0x81000041;   /* ROM  error                                                                                                                                          */
	    public const UInt32 RAM_ERR 							= 0x81000042;   /* RAM  error                                                                                                                                          */
	    public const UInt32 MPU_ERR 							= 0x81000043;   /* CPU  error                                                                                                                                          */
	    public const UInt32 FPU_ERR 							= 0x81000044;   /* FPU  error                                                                                                                                          */
	    public const UInt32 CERF_ERR 							= 0x81000049;   /* CERF error                                                                                                                                          */
	    public const UInt32 EXIO_ERR 							= 0x81000050;   /* EXIO error                                                                                                                                          */
	    public const UInt32 BUSIF_ERR 							= 0x8100005F;   /* Common RAM error for OEM                                                                                                                            */
	    public const UInt32 ALM_NO_ALM 							= 0x00000000;   /* No alarm                                                                                                                                            */
	    public const UInt32 ALM_MK_DEBUG 						= 0x67050300;   /* Minor failure: Alarm code for debugging                                                                                                             */
	    public const UInt32 ALM_MK_ROUND_ERR 					= 0x67050301;   /* Minor failure: Improper specification of one cycle at radius specification                                                                          */
	    public const UInt32 ALM_MK_FSPEED_OVER 					= 0x67050302;   /* Minor failure: Feeding speed exceeded                                                                                                               */
	    public const UInt32 ALM_MK_FSPEED_NOSPEC 				= 0x67050303;   /* Minor failure: Feeding speed not specified                                                                                                          */
	    public const UInt32 ALM_MK_PRM_OVER 					= 0x67050304;   /* Minor failure: Value after conversion of acceleration or deceleration parameter is out of range.                                                    */
	    public const UInt32 ALM_MK_ARCLEN_OVER 					= 0x67050305;   /* Minor failure: Arc length exceeds LONG_MAX.                                                                                                         */
	    public const UInt32 ALM_MK_VERT_NOSPEC 					= 0x67050306;   /* Minor failure: Vertical axis by plane specification not specified                                                                                   */
	    public const UInt32 ALM_MK_HORZ_NOSPEC 					= 0x67050307;   /* Minor failure: Horizontal axis by plane specification not specified                                                                                 */
	    public const UInt32 ALM_MK_TURN_OVER 					= 0x67050308;   /* Minor failure: Specified number of turns exceeded                                                                                                   */
	    public const UInt32 ALM_MK_RADIUS_OVER 					= 0x67050309;   /* Minor failure: Radius exceeds LONG_MAX.                                                                                                             */
	    public const UInt32 ALM_MK_CENTER_ERR 					= 0x6705030A;   /* Minor failure: Illegal center point specification                                                                                                   */
	    public const UInt32 ALM_MK_BLOCK_OVER 					= 0x6705030B;   /* Minor failure: Linear interpolation block moving amount exceeded                                                                                    */
	    public const UInt32 ALM_MK_MAXF_NOSPEC 					= 0x6705030C;   /* Minor failure: maxf not defined                                                                                                                     */
	    public const UInt32 ALM_MK_TDATA_ERR 					= 0x6705030D;   /* Minor failure: Address T data error                                                                                                                 */
	    public const UInt32 ALM_MK_REG_ERR 						= 0x6705030E;   /* Minor failure: REG data error and PP fault                                                                                                          */
	    public const UInt32 ALM_MK_COMMAND_CODE_ERR 			= 0x6705030F;   /* Minor failure: Out-of-range command                                                                                                                 */
	    public const UInt32 ALM_MK_AXIS_CONFLICT 				= 0x67050310;   /* Minor failure: Use of logical axis being prohibited                                                                                                 */
	    public const UInt32 ALM_MK_POSMAX_OVER 					= 0x67050311;   /* Minor failure: Infinite length axis, MVM or ABS coordinate designation exceeds POSMAX.                                                              */
	    public const UInt32 ALM_MK_DIST_OVER 					= 0x67050312;   /* Minor failure: Axis moving distance is other than (LONG_MIN, LONG_MAX).                                                                             */
	    public const UInt32 ALM_MK_MODE_ERR 					= 0x67050313;   /* Minor failure: Illegal control mode                                                                                                                 */
	    public const UInt32 ALM_MK_CMD_CONFLICT 				= 0x67050314;   /* Minor failure: Motion command overlapping instruction                                                                                               */
	    public const UInt32 ALM_MK_RCMD_CONFLICT 				= 0x67050315;   /* Minor failure: Motion response command overlapping instruction                                                                                      */
	    public const UInt32 ALM_MK_CMD_MODE_ERR 				= 0x67050316;   /* Minor failure: Illegal motion command mode                                                                                                          */
	    public const UInt32 ALM_MK_CMD_NOT_ALLOW 				= 0x67050317;   /* Minor failure: Command cannot be executed ih this Module.                                                                                           */
	    public const UInt32 ALM_MK_CMD_DEN_FAIL 				= 0x67050318;   /* Minor failure: Distribution is not completed.                                                                                                       */
	    public const UInt32 ALM_MK_H_MOVE_ERR 					= 0x67050319;   /* Minor failure: Illegal hMove                                                                                                                        */
	    public const UInt32 ALM_MK_MOVE_NOT_SUPPORT 			= 0x6705031A;   /* Minor failure: Non-supported Move defined                                                                                                           */
	    public const UInt32 ALM_MK_EVENT_NOT_SUPPORT 			= 0x6705031B;   /* Minor failure: Non-supported Move Event defined                                                                                                     */
	    public const UInt32 ALM_MK_ACTION_NOT_SUPPORT 			= 0x6705031C;   /* Minor failure: Non-supported Move Action defined                                                                                                    */
	    public const UInt32 ALM_MK_MOVE_TYPE_ERR 				= 0x6705031D;   /* Minor failure: MoveType specification error                                                                                                         */
	    public const UInt32 ALM_MK_VTYPE_ERR 					= 0x6705031E;   /* Minor failure: VelocityType specification error                                                                                                     */
	    public const UInt32 ALM_MK_ATYPE_ERR 					= 0x6705031F;   /* Minor failure: AccelerationType specification error                                                                                                 */
	    public const UInt32 ALM_MK_HOMING_METHOD_ERR 			= 0x67050320;   /* Minor failure: Homing_method specification error                                                                                                    */
	    public const UInt32 ALM_MK_ACC_ERR 						= 0x67050321;   /* Minor failure: Acceleration setting error                                                                                                           */
	    public const UInt32 ALM_MK_DEC_ERR 						= 0x67050322;   /* Minor failure: Deceleration setting error                                                                                                           */
	    public const UInt32 ALM_MK_POS_TYPE_ERR 				= 0x67050323;   /* Minor failure: Position reference type error                                                                                                        */
	    public const UInt32 ALM_MK_INVALID_EVENT_ERR 			= 0x67050324;   /* Minor failure: Illegal EVENT type                                                                                                                   */
	    public const UInt32 ALM_MK_INVALID_ACTION_ERR 			= 0x67050325;   /* Minor failure: Illegal ACTION type                                                                                                                  */
	    public const UInt32 ALM_MK_MOVE_NOT_ACTIVE 				= 0x67050326;   /* Minor failure: Action for Move that has not been executed                                                                                           */
	    public const UInt32 ALM_MK_MOVELIST_NOT_ACTIVE 			= 0x67050327;   /* Minor failure: Action for MoveList that has not been executed                                                                                       */
	    public const UInt32 ALM_MK_TBL_INVALID_DATA 			= 0x67050328;   /* Minor failure: Illegal table data                                                                                                                   */
	    public const UInt32 ALM_MK_TBL_INVALID_SEG_NUM 			= 0x67050329;   /* Minor failure: Illegal number of table interpolation execution segments                                                                             */
	    public const UInt32 ALM_MK_TBL_INVALID_AXIS_NUM 		= 0x6705032A;   /* Minor failure: Illegal number of table interpolation axes specified                                                                                 */
	    public const UInt32 ALM_MK_TBL_INVALID_ST_SEG 			= 0x6705032B;   /* Minor failure: Illegal table interpolation starting segment number                                                                                  */
	    public const UInt32 ALM_MK_STBL_INVALID_EXE 			= 0x6705032C;   /* Minor failure: Execution error during Static table file being written                                                                               */
	    public const UInt32 ALM_MK_DTBL_DUPLICATE_EXE 			= 0x6705032D;   /* Minor failure: Dynamic table duplicated execution error                                                                                             */
	    public const UInt32 ALM_MK_LATCH_CONFLICT 				= 0x6705032E;   /* Minor failure: LATCH request overlapping instruction error                                                                                          */
	    public const UInt32 ALM_MK_INVALID_AXISTYPE 			= 0x6705032F;   /* Minor failure: Illegal axis type (finite length axis/inifinite length axis)                                                                         */
	    public const UInt32 ALM_MK_NOT_SVCRDY 					= 0x67050330;   /* Minor failure: Move object executed when Motion Driver operation is not ready                                                                       */
	    public const UInt32 ALM_MK_NOT_SVCRUN 					= 0x67050331;   /* Minor failure: Move object executed at servo OFF                                                                                                    */
	    public const UInt32 ALM_MK_MDALARM 						= 0x67050332;   /* Minor failure: Move object executed at occurrence of Motion Driver alarm                                                                            */
	    public const UInt32 ALM_MK_SUPERPOSE_MASTER_ERR 		= 0x67050333;   /* Minor failure: Distribution synthetic object master axis condition error                                                                            */
	    public const UInt32 ALM_MK_SUPERPOSE_SLAVE_ERR 			= 0x67050334;   /* Minor failure: Distribution synthetic object slave axis condition error                                                                             */
	    public const UInt32 ALM_MK_MDWARNING 					= 0x57050335;   /* Warning: Motion Driver warning                                                                                                                      */
	    public const UInt32 ALM_MK_MDWARNING_POSERR 			= 0x57050336;   /* Warning: Motion Driver deviation warning                                                                                                            */
	    public const UInt32 ALM_MK_NOT_INFINITE_ABS 			= 0x67050337;   /* Minor failure: Specified axis cannot be used as ABS infinite length axis.                                                                           */
	    public const UInt32 ALM_MK_INVALID_LOGICAL_NUM 			= 0x67050338;   /* Minor failure: Illegal logical axis number has been specified.                                                                                      */
	    public const UInt32 ALM_MK_MAX_VELOCITY_ERR 			= 0x67050339;   /* Minor failure: Maximum speed setting error                                                                                                          */
	    public const UInt32 ALM_MK_VELOCITY_ERR 				= 0x6705033A;   /* Minor failure: Speed setting error                                                                                                                  */
	    public const UInt32 ALM_MK_APPROACH_ERR 				= 0x6705033B;   /* Minor failure: Approach speed setting error                                                                                                         */
	    public const UInt32 ALM_MK_CREEP_ERR 					= 0x6705033C;   /* Minor failure: Creep speed setting error                                                                                                            */
	    public const UInt32 ALM_MK_OS_ERR_MBOX1 				= 0x83050340;   /* Major failure: Mail box creation error (mail box for request for Motion Kernel Move object execution)                                               */
	    public const UInt32 ALM_MK_OS_ERR_MBOX2 				= 0x83050341;   /* Major failure: Mail box creation error (mail box for request for Motion Kernel action execution)                                                    */
	    public const UInt32 ALM_MK_OS_ERR_SEND_MSG1 			= 0x83050342;   /* Major failure: Message sending error at OS level (MK to EM: Notification of event detection)                                                        */
	    public const UInt32 ALM_MK_OS_ERR_SEND_MSG2 			= 0x83050343;   /* Major failure: Message sending error at OS level (MK to MM: Move completion message)                                                                */
	    public const UInt32 ALM_MK_OS_ERR_SEND_MSG3 			= 0x83050344;   /* Major failure: Message sending error at OS level (EM to MM: Notification of Action)                                                                 */
	    public const UInt32 ALM_MK_OS_ERR_SEND_MSG4 			= 0x83050345;   /* Major failure: Message sending error at OS level (Others)                                                                                           */
	    public const UInt32 ALM_MK_ACTION_ERR1 					= 0x53050346;   /* Warning: Illegal response message received (action execution request to waiting status for response of notification of action execution completion) */
	    public const UInt32 ALM_MK_ACTION_ERR2 					= 0x53050347;   /* Warning: Illegal response message received(not an action for Move object currently executed)                                                        */
	    public const UInt32 ALM_MK_ACTION_ERR3 					= 0x53050348;   /* Warning: Illegal response message received (not an action for MoveLIST object currently executed)                                                   */
	    public const UInt32 ALM_MK_RCV_INV_MSG1 				= 0x53050349;   /* Warning: Illegal command message received (not a MOVE object handle)                                                                                */
	    public const UInt32 ALM_MK_RCV_INV_MSG2 				= 0x5305034A;   /* Warning: Illegal command message received (command from other than Motion Manager)                                                                  */
	    public const UInt32 ALM_MK_RCV_INV_MSG3 				= 0x5305034B;   /* Warning: Illegal command message received (not a command message)                                                                                   */
	    public const UInt32 ALM_MK_RCV_INV_MSG4 				= 0x5305034C;   /* Warning: Illegal command message received (message other than command/response)                                                                     */
	    public const UInt32 ALM_MK_RCV_INV_MSG5 				= 0x5305034D;   /* Warning: Illegal command message received (command message from other than Event Manager)                                                           */
	    public const UInt32 ALM_MK_RCV_INV_MSG6 				= 0x5305034E;   /* Warning: Illegal command message received (command message other than request for action execution)                                                 */
	    public const UInt32 ALM_MK_RCV_INV_MSG7 				= 0x5305034F;   /* Warning: Illegal response message received (response message from other than Event Manager)                                                         */
	    public const UInt32 ALM_MK_RCV_INV_MSG8 				= 0x53050350;   /* Warning: Illegal response message received (response message other than event notification completion/action completion notification)               */
	    public const UInt32 ALM_MK_AXIS_HANDLE_ERROR 			= 0x67050360;   /* Minor failure: Axis handle number is incorrect.                                                                                                     */
	    public const UInt32 ALM_MK_SLAVE_USED_AS_MASTER 		= 0x67050361;   /* Minor failure: An attempt was made to use a slave axis as a master axis.                                                                            */
	    public const UInt32 ALM_MK_MASTER_USED_AS_SLAVE 		= 0x67050362;   /* Minor failure: An attempt was made to use a master axis as a slave axis.                                                                            */
	    public const UInt32 ALM_MK_SLAVE_HAS_2_MASTERS 			= 0x67050363;   /* Minor failure: More than two master axes were specified for the same slave axis.                                                                    */
	    public const UInt32 ALM_MK_SLAVE_HAS_NOT_WORK 			= 0x67050364;   /* Minor failure: Work axis cannot be assured for a slave axis.                                                                                        */
	    public const UInt32 ALM_MK_PARAM_OUT_OF_RANGE 			= 0x67050365;   /* Minor failure: Parameter is out of range.                                                                                                           */
	    public const UInt32 ALM_MK_NNUM_MAX_OVER 				= 0x67050366;   /* Minor failure: Maximum number of averaging times exceeded                                                                                           */
	    public const UInt32 ALM_MK_FGNTBL_INVALID 				= 0x67050367;   /* Minor failure: Contents of the FGN table are illegal.                                                                                               */
	    public const UInt32 ALM_MK_PARAM_OVERFLOW 				= 0x67050368;   /* Minor failure: Set value overflows.                                                                                                                 */
	    public const UInt32 ALM_MK_ALREADY_COMMANDED 			= 0x67050369;   /* Minor failure: CAM or GEAR has already been under execution.                                                                                        */
	    public const UInt32 ALM_MK_MULTIPLE_SHIFTS 				= 0x6705036A;   /* Minor failure: Position Offset/Cam Shift was executed during execution of Position Offset/Cam Shift.                                                */
	    public const UInt32 ALM_MK_CAMTBL_INVALID 				= 0x6705036B;   /* Minor failure: CAM table is illegal (address, contents, etc.).                                                                                      */
	    public const UInt32 ALM_MK_ABORTED_BY_STOPMTN 			= 0x6705036C;   /* Minor failure: CAM/GEAR is aborted by STOP MOTION, etc.                                                                                             */
	    public const UInt32 ALM_MK_HMETHOD_INVALID 				= 0x6705036D;   /* Minor failure: Non-supported zero point return method                                                                                               */
	    public const UInt32 ALM_MK_MASTER_INVALID 				= 0x6705036E;   /* Minor failure: Master axis is not specified for monitoring.                                                                                         */
	    public const UInt32 ALM_MK_DATA_HANDLE_INVALID 			= 0x6705036F;   /* Minor failure: Register/global data handle is illegal.                                                                                              */
	    public const UInt32 ALM_MK_UNKNOWN_CAM_GEAR_ERR 		= 0x67050370;   /* Minor failure: Unclear error related to CAM/GEAR                                                                                                    */
	    public const UInt32 ALM_MK_REG_SIZE_INVALID 			= 0x67050371;   /* Minor failure: Register handle size is illegal.                                                                                                     */
	    public const UInt32 ALM_MK_ACTION_HANDLE_ERROR 			= 0x67050372;   /* Minor failure: Action handle is illegal.                                                                                                            */
	    public const UInt32 ALM_MM_OS_ERR_MBOX1 				= 0x83040380;   /* Major failure: Mail box creation error (mail box to start up Motion Manager)                                                                        */
	    public const UInt32 ALM_MM_OS_ERR_SEND_MSG1 			= 0x83040381;   /* Major failure: Message sending error at OS level (Motion Manager to Motion Kernel)                                                                  */
	    public const UInt32 ALM_MM_OS_ERR_SEND_MSG2 			= 0x83040382;   /* Major failure: Message sending error at OS level (Motion Manager to Event Manager)                                                                  */
	    public const UInt32 ALM_MM_OS_ERR_RCV_MSG1 				= 0x83040383;   /* Major failure: Message receiving error at OS level                                                                                                  */
	    public const UInt32 ALM_MM_MK_BUSY 						= 0x67040384;   /* Minor failure: All Motion Kernels are in use.                                                                                                       */
	    public const UInt32 ALM_MM_RCV_INV_MSG1 				= 0x53040385;   /* Warning: Illegal command message received (illegal handle 1: Not hMOVE.)                                                                            */
	    public const UInt32 ALM_MM_RCV_INV_MSG2 				= 0x53040386;   /* Warning: Illegal command message received (illegal handle 2: hMOVE does not exist.)                                                                 */
	    public const UInt32 ALM_MM_RCV_INV_MSG3 				= 0x53040387;   /* Warning: Illegal command message received (Not request for start action execution)                                                                  */
	    public const UInt32 ALM_MM_RCV_INV_MSG4 				= 0x53040388;   /* Warning: Illegal response message received (response message other than event notification completion)                                              */
	    public const UInt32 ALM_MM_RCV_INV_MSG5 				= 0x53040389;   /* Warning: Illegal response message received (Other messages received with action completion response message)                                        */
	    public const UInt32 ALM_IM_DEVICEID_ERR 				= 0x53060480;   /* DeviceID error or non-supported Device                                                                                                              */
	    public const UInt32 ALM_IM_REGHANDLE_ERR 				= 0x53060481;   /* Register handle error                                                                                                                               */
	    public const UInt32 ALM_IM_GLOBALHANDLE_ERR 			= 0x53060482;   /* Global data handle error                                                                                                                            */
	    public const UInt32 ALM_IM_DEVICETYPE_ERR 				= 0x53060483;   /* Non-supported data type                                                                                                                             */
	    public const UInt32 ALM_IM_OFFSET_ERR 					= 0x53060484;   /* Incorrect offset value                                                                                                                              */
	    public const UInt32 AM_ER_UNDEF_COMMAND 				= 0x57020501;   /* Illegal command code                                                                                                                                */
	    public const UInt32 AM_ER_UNDEF_CMNDTYPE 				= 0x57020502;   /* Illegal command type                                                                                                                                */
	    public const UInt32 AM_ER_UNDEF_OBJTYPE 				= 0x57020503;   /* Illegal object type                                                                                                                                 */
	    public const UInt32 AM_ER_UNDEF_HANDLETYPE 				= 0x57020504;   /* Illegal handle type                                                                                                                                 */
	    public const UInt32 AM_ER_UNDEF_PKTDAT 					= 0x57020505;   /* Illegal packet data                                                                                                                                 */
	    public const UInt32 AM_ER_UNDEF_AXIS 					= 0x57020506;   /* axis not defined                                                                                                                                    */
	    public const UInt32 AM_ER_MSGBUF_GET_FAULT 				= 0x57020510;   /* Acquisition failure of  message buffer managed table                                                                                                */
	    public const UInt32 AM_ER_ACTSIZE_GET_FAULT 			= 0x57020511;   /* Acquisition failure of ACT size                                                                                                                     */
	    public const UInt32 AM_ER_APIBUF_GET_FAULT 				= 0x57020512;   /* Acquisition failure of API buffer managed table                                                                                                     */
	    public const UInt32 AM_ER_MOVEOBJ_GET_FAULT 			= 0x57020513;   /* Acquisition failure of MOVE object managed table                                                                                                    */
	    public const UInt32 AM_ER_EVTTBL_GET_FAULT 				= 0x57020514;   /* Acquisition failure of event managed table                                                                                                          */
	    public const UInt32 AM_ER_ACTTBL_GET_FAULT 				= 0x57020515;   /* Acquisition failure of Action managed table                                                                                                         */
	    public const UInt32 AM_ER_1BY1APIBUF_GET_FAULT 			= 0x57020516;   /* Acquisition failure of Sequence managed table                                                                                                       */
	    public const UInt32 AM_ER_AXSTBL_GET_FAULT 				= 0x57020517;   /* Acquisition failure of AXIS handle managed table                                                                                                    */
	    public const UInt32 AM_ER_SUPERPOSEOBJ_GET_FAULT 		= 0x57020518;   /* Acquisition failure of Distribution synthetic object managed table                                                                                  */
	    public const UInt32 AM_ER_SUPERPOSEOBJ_CLEAR_FAULT 		= 0x57020519;   /* Deletion failure of Distribution synthetic object                                                                                                   */
	    public const UInt32 AM_ER_AXIS_IN_USE 					= 0x5702051A;   /* axis in use                                                                                                                                         */
	    public const UInt32 AM_ER_HASHTBL_GET_FAULT 			= 0x5702051B;   /* Hash table acquisition failure for axial name management                                                                                            */
	    public const UInt32 AM_ER_UNMATCH_OBJHNDL 				= 0x57020530;   /* MOVE object handle mismatched                                                                                                                       */
	    public const UInt32 AM_ER_UNMATCH_OBJECT 				= 0x57020531;   /* Object mismatched                                                                                                                                   */
	    public const UInt32 AM_ER_UNMATCH_APIBUF 				= 0x57020532;   /* API buffer mismatched                                                                                                                               */
	    public const UInt32 AM_ER_UNMATCH_MSGBUF 				= 0x57020533;   /* Message buffer mismatched                                                                                                                           */
	    public const UInt32 AM_ER_UNMATCH_ACTBUF 				= 0x57020534;   /* Action execution management buffer mismatched                                                                                                       */
	    public const UInt32 AM_ER_UNMATH_SEQUENCE 				= 0x57020535;   /* Sequence number mismatched                                                                                                                          */
	    public const UInt32 AM_ER_UNMATCH_1BY1APIBUF 			= 0x57020536;   /* Sequential API management table mismatched                                                                                                          */
	    public const UInt32 AM_ER_UNMATCH_MOVEOBJTABLE 			= 0x57020537;   /* MOVE object management table mismatched                                                                                                             */
	    public const UInt32 AM_ER_UNMATCH_MOVELISTTABLE 		= 0x57020538;   /* MOVE LIST management table mismatched                                                                                                               */
	    public const UInt32 AM_ER_UNMATCH_MOVELIST_OBJECT 		= 0x57020539;   /* MOVE LIST object mismatched                                                                                                                         */
	    public const UInt32 AM_ER_UNMATCH_MOVELIST_OBJHNDL 		= 0x5702053A;   /* MOVE LIST object handle mismatched                                                                                                                  */
	    public const UInt32 AM_ER_UNGET_MOVEOBJTABLE 			= 0x57020550;   /* MOVE object management table not assured                                                                                                            */
	    public const UInt32 AM_ER_UNGET_MOVELISTTABLE 			= 0x57020551;   /* MOVE LIST object management table not assured                                                                                                       */
	    public const UInt32 AM_ER_UNGET_1BY1APIBUFTABLE 		= 0x57020552;   /* Sequential API management table not assured                                                                                                         */
	    public const UInt32 AM_ER_NOEMPTYTBL_ERROR 				= 0x57020560;   /* No unused table among interpolation tables                                                                                                          */
	    public const UInt32 AM_ER_NOTGETSEM_ERROR 				= 0x57020561;   /* Failure to get AM-MK semaphore  (Dynamic)                                                                                                           */
	    public const UInt32 AM_ER_NOTGETTBLADD_ERROR 			= 0x57020562;   /* Failure to get interpolation table address                                                                                                          */
	    public const UInt32 AM_ER_NOTWRTTBL_ERROR 				= 0x57020563;   /* Failure to write in table at execution (Static)                                                                                                     */
	    public const UInt32 AM_ER_TBLINDEX_ERROR 				= 0x57020564;   /* Index setting error (Static)                                                                                                                        */
	    public const UInt32 AM_ER_ILLTBLTYPE_ERROR 				= 0x57020565;   /* Invalid table type specified                                                                                                                        */
	    public const UInt32 AM_ER_UNSUPORTED_EVENT 				= 0x57020570;   /* Event not supported or argument error                                                                                                               */
	    public const UInt32 AM_ER_WRONG_SEQUENCE 				= 0x57020571;   /* Sequence error                                                                                                                                      */
	    public const UInt32 AM_ER_MOVEOBJ_BUSY 					= 0x57020572;   /* MOVE object under execution                                                                                                                         */
	    public const UInt32 AM_ER_MOVELIST_BUSY 				= 0x57020573;   /* MOVE LIST under execution                                                                                                                           */
	    public const UInt32 AM_ER_MOVELIST_ADD_FAULT 			= 0x57020574;   /* MOVE OBJ cannot be registered.                                                                                                                      */
	    public const UInt32 AM_ER_CONFLICT_PHI_AXS 				= 0x57020575;   /* Physical axes overlapped                                                                                                                            */
	    public const UInt32 AM_ER_CONFLICT_LOG_AXS 				= 0x57020576;   /* Logic axes overlapped                                                                                                                               */
	    public const UInt32 AM_ER_PKTSTS_ERROR 					= 0x57020577;   /* Receiving packet status error                                                                                                                       */
	    public const UInt32 AM_ER_CONFLICT_NAME 				= 0x57020578;   /* Axis name overlapped                                                                                                                                */
	    public const UInt32 AM_ER_ILLEGAL_NAME 					= 0x57020579;   /* Incorrect axis name                                                                                                                                 */
	    public const UInt32 AM_ER_SEMAPHORE_ERROR 				= 0x5702057A;   /* Incorrect semaphore at host PC interruption                                                                                                         */
	    public const UInt32 AM_ER_LOG_AXS_OVER 					= 0x5702057B;   /* Logical axis number exceeded                                                                                                                        */
	    public const UInt32 IM_STATION_ERR 						= 0x55060B00;   /* Warning: Link communication station error                                                                                                           */
	    public const UInt32 IM_IO_ERR 							= 0x55060B01;   /* Warning: I/O error                                                                                                                                  */
	    public const UInt32 MP_FILE_ERR_GENERAL 				= 0x53168001;   /* General error.                                                                                                                                      */
	    public const UInt32 MP_FILE_ERR_NOT_SUPPORTED 			= 0x53168002;   /* Feature not supported.                                                                                                                              */
	    public const UInt32 MP_FILE_ERR_INVALID_ARGUMENT 		= 0x53168003;   /* Invalid argument                                                                                                                                    */
	    public const UInt32 MP_FILE_ERR_INVALID_HANDLE 			= 0x53168004;   /* Invalid handle                                                                                                                                      */
	    public const UInt32 MP_FILE_ERR_NO_FILE 				= 0x53168064;   /* No such file (or directory).                                                                                                                        */
	    public const UInt32 MP_FILE_ERR_INVALID_PATH 			= 0x53168065;   /* Invalid path.                                                                                                                                       */
	    public const UInt32 MP_FILE_ERR_EOF 					= 0x53168066;   /* End of file detected.                                                                                                                               */
	    public const UInt32 MP_FILE_ERR_PERMISSION_DENIED 		= 0x53168067;   /* Not arrowed to access the file.                                                                                                                     */
	    public const UInt32 MP_FILE_ERR_TOO_MANY_FILES 			= 0x53168068;   /* Too many files opened.                                                                                                                              */
	    public const UInt32 MP_FILE_ERR_FILE_BUSY 				= 0x53168069;   /* File is in use.                                                                                                                                     */
	    public const UInt32 MP_FILE_ERR_TIMEOUT 				= 0x5316806A;   /* Timeout occured.                                                                                                                                    */
	    public const UInt32 MP_FILE_ERR_BAD_FS 					= 0x531680C8;   /* Invalid or unexepected logical filesystem in the media                                                                                              */
	    public const UInt32 MP_FILE_ERR_FILESYSTEM_FULL 		= 0x531680C9;   /* LFS (ie the media) is full.                                                                                                                         */
	    public const UInt32 MP_FILE_ERR_INVALID_LFM 			= 0x531680CA;   /* Invalid LFM specified.                                                                                                                              */
	    public const UInt32 MP_FILE_ERR_TOO_MANY_LFM 			= 0x531680CB;   /* LFM table is full.                                                                                                                                  */
	    public const UInt32 MP_FILE_ERR_INVALID_PDM 			= 0x5316812C;   /* Invalid PDM specified.                                                                                                                              */
	    public const UInt32 MP_FILE_ERR_INVALID_MEDIA 			= 0x5316812D;   /* Invalid media specified.                                                                                                                            */
	    public const UInt32 MP_FILE_ERR_TOO_MANY_PDM 			= 0x5316812E;   /* Too many PDM.                                                                                                                                       */
	    public const UInt32 MP_FILE_ERR_TOO_MANY_MEDIA 			= 0x5316812F;   /* Too many media.                                                                                                                                     */
	    public const UInt32 MP_FILE_ERR_WRITE_PROTECTED 		= 0x53168130;   /* Write protected media.                                                                                                                              */
	    public const UInt32 MP_FILE_ERR_INVALID_DEVICE 			= 0x53168190;   /* Invalid device specified.                                                                                                                           */
	    public const UInt32 MP_FILE_ERR_DEVICE_IO 				= 0x53168191;   /* Error occured in accessing the device.                                                                                                              */
	    public const UInt32 MP_FILE_ERR_DEVICE_BUSY 			= 0x53168192;   /* Device is in use.                                                                                                                                   */
	    public const UInt32 MP_FILE_ERR_NO_CARD 				= 0x5316A711;   /* CF CARD not mounted.                                                                                                                                */
	    public const UInt32 MP_FILE_ERR_CARD_POWER 				= 0x5316A712;   /* CF CARD Power-OFF.                                                                                                                                  */
	    public const UInt32 MP_CARD_SYSTEM_ERR 					= 0x53178FFF;   /* Card System Error.                                                                                                                                  */
	    public const UInt32 ERROR_CODE_GET_DIREC_OFFSET 		= 0x83001A01;   /* Directory area offset cannot be got.                                                                                                                */
	    public const UInt32 ERROR_CODE_GET_DIREC_INFO 			= 0x83001A02;   /* Failure to get directory information                                                                                                                */
        public const UInt32 ERROR_CODE_FUNC_TABLE               = 0x83001A03;   /* Failure to get system call function table                                                                                                           */
        public const UInt32 ERROR_CODE_SLEEP_TASK               = 0x83001A04;   /* Sleep error                                                                                                                                         */
        public const UInt32 ERROR_CODE_DEVICE_HANDLE_FULL       = 0x43001A41;   /* Number of device handles exceeds the maximum value.                                                                                                 */
        public const UInt32 ERROR_CODE_ALLOC_MEMORY             = 0x43001A42;   /* Failure to get the area.                                                                                                                            */
        public const UInt32 ERROR_CODE_BUFCOPY                  = 0x43001A43;   /* MemoryCopy(),name_copy() error                                                                                                                      */
        public const UInt32 ERROR_CODE_GET_COMMEM_OFFSET        = 0x43001A44;   /* Failure to get common memory offset value                                                                                                           */
        public const UInt32 ERROR_CODE_CREATE_SEMPH             = 0x43001A45;   /* Semaphore creation error                                                                                                                            */
        public const UInt32 ERROR_CODE_DELETE_SEMPH             = 0x43001A46;   /* Semaphore deletion error                                                                                                                            */
        public const UInt32 ERROR_CODE_LOCK_SEMPH               = 0x43001A47;   /* Error at semaphore lock                                                                                                                             */
        public const UInt32 ERROR_CODE_UNLOCK_SEMPH             = 0x43001A48;   /* Error at semaphore release                                                                                                                          */
        public const UInt32 ERROR_CODE_PACKETSIZE_OVER          = 0x43001A49;   /* Error when controller is being initialized                                                                                                          */
        public const UInt32 ERROR_CODE_UNREADY                  = 0x43001A4A;   /* Error when CPU is stopping                                                                                                                          */
        public const UInt32 ERROR_CODE_CPUSTOP                  = 0x43001A4B;   /* CPU number is illegal                                                                                                                               */
        public const UInt32 ERROR_CODE_CNTRNO                   = 0x470B1A81;   /* Device number                                                                                                                                       */
        public const UInt32 ERROR_CODE_SELECTION                = 0x470B1A82;   /* Illegal selected value (0 or 1)                                                                                                                     */
        public const UInt32 ERROR_CODE_LENGTH                   = 0x470B1A83;   /* Data length                                                                                                                                         */
        public const UInt32 ERROR_CODE_OFFSET                   = 0x470B1A84;   /* Offset value                                                                                                                                        */
        public const UInt32 ERROR_CODE_DATACOUNT                = 0x470B1A85;   /* Number of data items                                                                                                                                */
        public const UInt32 ERROR_CODE_DATREAD                  = 0x46001A86;   /* Failure to read out from common memory                                                                                                              */
        public const UInt32 ERROR_CODE_DATWRITE                 = 0x46001A87;   /* Failure to write in to common memory                                                                                                                */
        public const UInt32 ERROR_CODE_BITWRITE                 = 0x46001A88;   /* Failure to write in bit data to common memory                                                                                                       */
        public const UInt32 ERROR_CODE_DEVCNTR                  = 0x46001A89;   /* DeviceIoControl() completed erroneously.                                                                                                            */
        public const UInt32 ERROR_CODE_NOTINIT                  = 0x460F1A8A;   /* Driver initialization error                                                                                                                         */
        public const UInt32 ERROR_CODE_SEMPHLOCK                = 0x41001A8B;   /* Packet sending semaphore locked                                                                                                                     */
        public const UInt32 ERROR_CODE_SEMPHUNLOCK              = 0x41001A8C;   /* Packet receiving semaphore not locked                                                                                                               */
        public const UInt32 ERROR_CODE_DRV_PROC                 = 0x460F1A8D;   /* Driver processing completed erroneously.                                                                                                            */
        public const UInt32 ERROR_CODE_GET_DRIVER_HANDLE        = 0x460F1A8E;   /* Failure to get driver file handle                                                                                                                   */
        public const UInt32 ERROR_CODE_SEND_MSG                 = 0x450E1AC1;   /* Message sending error                                                                                                                               */
        public const UInt32 ERROR_CODE_RECV_MSG                 = 0x450E1AC2;   /* Message receiving error                                                                                                                             */
        public const UInt32 ERROR_CODE_INVALID_RESPONSE         = 0x450E1AC3;   /* Receiving packet illegal                                                                                                                            */
        public const UInt32 ERROR_CODE_INVALID_ID               = 0x450E1AC4;   /* Receiving packet ID illegal                                                                                                                         */
        public const UInt32 ERROR_CODE_INVALID_STATUS           = 0x450E1AC5;   /* Receiving packet status illegal                                                                                                                     */
        public const UInt32 ERROR_CODE_INVALID_CMDCODE          = 0x450E1AC6;   /* Receiving packet command code illegal                                                                                                               */
        public const UInt32 ERROR_CODE_INVALID_SEQNO            = 0x450E1AC7;   /* Receiving packet sequence number illegal                                                                                                            */
        public const UInt32 ERROR_CODE_SEND_RETRY_OVER          = 0x450E1AC8;   /* Number of retries exceeded (packet sending)                                                                                                         */
        public const UInt32 ERROR_CODE_RECV_RETRY_OVER          = 0x450E1AC9;   /* Number of retries exceeded (packet receiving)                                                                                                       */
        public const UInt32 ERROR_CODE_RESPONSE_TIMEOUT         = 0x450E1ACA;   /* Response waiting timeout error                                                                                                                      */
        public const UInt32 ERROR_CODE_WAIT_FOR_EVENT           = 0x450E1ACB;   /* Event waiting error                                                                                                                                 */
        public const UInt32 ERROR_CODE_EVENT_OPEN               = 0x450E1ACC;   /* Failure to open event                                                                                                                               */
        public const UInt32 ERROR_CODE_EVENT_RESET              = 0x450E1ACD;   /* Failure to reset event                                                                                                                              */
        public const UInt32 ERROR_CODE_EVENT_READY              = 0x450E1ACE;   /* Failure to prepare for waiting for event                                                                                                            */
        public const UInt32 ERROR_CODE_PROCESSNUM               = 0x43001B01;   /* Number of processes exceeded                                                                                                                        */
        public const UInt32 ERROR_CODE_GET_PROC_INFO            = 0x43001B02;   /* Process information getting error                                                                                                                   */
        public const UInt32 ERROR_CODE_THREADNUM                = 0x43001B03;   /* Number of threads exceeded                                                                                                                          */
        public const UInt32 ERROR_CODE_GET_THRD_INFO            = 0x43001B04;   /* Thread information getting error                                                                                                                    */
        public const UInt32 ERROR_CODE_CREATE_MBOX              = 0x43001B05;   /* Mail box creation error                                                                                                                             */
        public const UInt32 ERROR_CODE_DELETE_MBOX              = 0x43001B06;   /* Mail box deletion error                                                                                                                             */
        public const UInt32 ERROR_CODE_GET_TASKID               = 0x83001B07;   /* Failure to get task ID                                                                                                                              */
        public const UInt32 ERROR_CODE_NO_THREADINFO            = 0x43001B08;   /* Specified thread information does not exist.                                                                                                        */
        public const UInt32 ERROR_CODE_COM_INITIALIZE           = 0x43001B09;   /* COM initialization error                                                                                                                            */
        public const UInt32 ERROR_CODE_COMDEVICENUM             = 0x430F1B41;   /* Number of ComDevice exceeded                                                                                                                        */
        public const UInt32 ERROR_CODE_GET_COM_DEVICE_HANDLE    = 0x430F1B42;   /* Failure to get ComDevice information structure                                                                                                      */
        public const UInt32 ERROR_CODE_COM_DEVICE_FULL          = 0x430F1B43;   /* ComDevice exceeds the maximum number.                                                                                                               */
        public const UInt32 ERROR_CODE_CREATE_PANELOBJ          = 0x430F1B44;   /* Failure to create panel command object                                                                                                              */
        public const UInt32 ERROR_CODE_OPEN_PANELOBJ            = 0x430F1B45;   /* Failure to open panel command object                                                                                                                */
        public const UInt32 ERROR_CODE_CLOSE_PANELOBJ           = 0x430F1B46;   /* Failure to close panel command object                                                                                                               */
        public const UInt32 ERROR_CODE_PROC_PANELOBJ            = 0x430F1B47;   /* Failure to process panel command object                                                                                                             */
        public const UInt32 ERROR_CODE_CREATE_CNTROBJ           = 0x430F1B48;   /* Failure to create panel command object                                                                                                              */
        public const UInt32 ERROR_CODE_OPEN_CNTROBJ             = 0x430F1B49;   /* Failure to open panel command object                                                                                                                */
        public const UInt32 ERROR_CODE_CLOSE_CNTROBJ            = 0x430F1B4A;   /* Failure to close panel command object                                                                                                               */
        public const UInt32 ERROR_CODE_PROC_CNTROBJ             = 0x430F1B4B;   /* Failure to process panel command object                                                                                                             */
        public const UInt32 ERROR_CODE_CREATE_COMDEV_MUTEX      = 0x430F1B4C;   /* Failure to create Mutex for ComDevice table                                                                                                         */
        public const UInt32 ERROR_CODE_CREATE_COMDEV_MAPFILE    = 0x430F1B4D;   /* Failure to create MapFile for ComDevice table                                                                                                       */
        public const UInt32 ERROR_CODE_OPEN_COMDEV_MAPFILE      = 0x430F1B4E;   /* Failure to open MapFile for ComDevice table                                                                                                         */
        public const UInt32 ERROR_CODE_NOT_OBJECT_TYPE          = 0x430F1B4F;   /* Object type error                                                                                                                                   */
        public const UInt32 ERROR_CODE_COM_NOT_OPENED           = 0x430F1B50;   /* Not opened                                                                                                                                          */
        public const UInt32 ERROR_CODE_PNLCMD_CPU_CONTROL       = 0x43081B80;   /* CPU control error                                                                                                                                   */
        public const UInt32 ERROR_CODE_PNLCMD_SFILE_READ        = 0x43081B81;   /* Failure to read out source file                                                                                                                     */
        public const UInt32 ERROR_CODE_PNLCMD_SFILE_WRITE       = 0x43081B82;   /* Failure to write in source file                                                                                                                     */
        public const UInt32 ERROR_CODE_PNLCMD_REGISTER_READ     = 0x43081B83;   /* Failure to read out register                                                                                                                        */
        public const UInt32 ERROR_CODE_PNLCMD_REGISTER_WRITE    = 0x43081B84;   /* Failure to write in register                                                                                                                        */
        public const UInt32 ERROR_CODE_PNLCMD_API_SEND          = 0x43081B85;   /* API Send command error                                                                                                                              */
        public const UInt32 ERROR_CODE_PNLCMD_API_RECV          = 0x43081B86;   /* API Recv command error                                                                                                                              */
        public const UInt32 ERROR_CODE_PNLCMD_NO_RESPONSE       = 0x43081B87;   /* No response packet is received at API Recv.                                                                                                         */
        public const UInt32 ERROR_CODE_PNLCMD_PACKET_READ       = 0x43081B88;   /* Failure to read packet                                                                                                                              */
        public const UInt32 ERROR_CODE_PNLCMD_PACKET_WRITE      = 0x43081B89;   /* Failure to write packet                                                                                                                             */
        public const UInt32 ERROR_CODE_PNLCMD_STATUS_READ       = 0x43081B8A;   /* Failure to read status                                                                                                                              */
        public const UInt32 ERROR_CODE_NOT_CONTROLLER_RDY       = 0x440D1BA0;   /*                                                                                                                                                     */
        public const UInt32 ERROR_CODE_DOUBLE_CMD               = 0x440D1BA1;   /*                                                                                                                                                     */
        public const UInt32 ERROR_CODE_DOUBLE_RCMD              = 0x440D1BA2;   /*                                                                                                                                                     */
        public const UInt32 ERROR_CODE_FILE_NOT_OPENED          = 0x43001BC1;   /* File is not opened.                                                                                                                                 */
        public const UInt32 ERROR_CODE_OPENFILE_NUM             = 0x43001BC2;   /*                                                                                                                                                     */
        public const UInt32 MP_CTRL_SYS_ERROR                   = 0x4308106f;   /*                                                                                                                                                     */
        public const UInt32 MP_CTRL_PARAM_ERR                   = 0x43081092;   /*                                                                                                                                                     */
        public const UInt32 MP_CTRL_FILE_NOT_FOUND              = 0x43081094;   /*                                                                                                                                                     */
        public const UInt32 MP_NOTMOVEHANDLE 					= 0x470B1100;   /* Undefined Move handle                                                                                                                               */
	    public const UInt32 MP_NOTTIMERHANDLE 					= 0x470B1101;   /* Undefined timer handle                                                                                                                              */
	    public const UInt32 MP_NOTINTERRUPT 					= 0x470B1102;   /* Undefined virtual interruption number                                                                                                               */
	    public const UInt32 MP_NOTEVENTHANDLE 					= 0x470B1103;   /* Undefined event handle                                                                                                                              */
	    public const UInt32 MP_NOTMESSAGEHANDLE 				= 0x470B1104;   /* Undefined message handle                                                                                                                            */
	    public const UInt32 MP_NOTUSERFUNCTIONHANDLE 			= 0x470B1105;   /* Undefined user function handle                                                                                                                      */
	    public const UInt32 MP_NOTACTIONHANDLE 					= 0x470B1106;   /* Undefined action handle                                                                                                                             */
	    public const UInt32 MP_NOTSUBSCRIBEHANDLE 				= 0x470B1107;   /* Undefined Subscribe handle                                                                                                                          */
	    public const UInt32 MP_NOTDEVICEHANDLE 					= 0x470B1108;   /* Undefined device handle                                                                                                                             */
	    public const UInt32 MP_NOTAXISHANDLE 					= 0x470B1109;   /* Undefined axis handle                                                                                                                               */
	    public const UInt32 MP_NOTMOVELISTHANDLE 				= 0x470B110A;   /* Undefined MoveList handle                                                                                                                           */
	    public const UInt32 MP_NOTTRACEHANDLE 					= 0x470B110B;   /* Undefined Trace handle                                                                                                                              */
	    public const UInt32 MP_NOTGLOBALDATAHANDLE 				= 0x470B110C;   /* Undefined global data handle                                                                                                                        */
	    public const UInt32 MP_NOTSUPERPOSEHANDLE 				= 0x470B110D;   /* Undefined Superpose handle                                                                                                                          */
	    public const UInt32 MP_NOTCONTROLLERHANDLE 				= 0x470B110E;   /* Undefined Controller handle                                                                                                                         */
	    public const UInt32 MP_NOTFILEHANDLE 					= 0x470B110F;   /* Undefined file handle                                                                                                                               */
	    public const UInt32 MP_NOTREGISTERDATAHANDLE 			= 0x470B1110;   /* Undefined register handle                                                                                                                           */
	    public const UInt32 MP_NOTALARMHANDLE 					= 0x470B1111;   /* Undefined alarm handle                                                                                                                              */
	    public const UInt32 MP_NOTCAMHANDLE 					= 0x470B1112;   /* Undefined CAM handle                                                                                                                                */
	    public const UInt32 MP_NOTHANDLE 						= 0x470B11FF;   /* Undefined handle                                                                                                                                    */
	    public const UInt32 MP_NOTEVENTTYPE 					= 0x470B1200;   /* Undefined event type                                                                                                                                */
	    public const UInt32 MP_NOTDEVICETYPE 					= 0x470B1201;   /* Undefined device type                                                                                                                               */
	    public const UInt32 MP_NOTDATAUNITSIZE 					= 0x4B0B1202;   /* Undefined unit data size                                                                                                                            */
	    public const UInt32 MP_NOTDEVICE 						= 0x470B1203;   /* Undefined device                                                                                                                                    */
	    public const UInt32 MP_NOTACTIONTYPE 					= 0x470B1204;   /* Undefined action type                                                                                                                               */
	    public const UInt32 MP_NOTPARAMNAME 					= 0x4B0B1205;   /* Undefined parameter name                                                                                                                            */
	    public const UInt32 MP_NOTDATATYPE 						= 0x470B1206;   /* Undefined data type                                                                                                                                 */
	    public const UInt32 MP_NOTBUFFERTYPE 					= 0x470B1207;   /* Undefined buffer type                                                                                                                               */
	    public const UInt32 MP_NOTMOVETYPE 						= 0x4B0B1208;   /* Undefined Move type                                                                                                                                 */
	    public const UInt32 MP_NOTANGLETYPE 					= 0x470B1209;   /* Undefined Angle type                                                                                                                                */
	    public const UInt32 MP_NOTDIRECTION 					= 0x4B0B120A;   /* Undefined rotating direction                                                                                                                        */
	    public const UInt32 MP_NOTAXISTYPE 						= 0x4B0B120B;   /* Undefined axis type                                                                                                                                 */
	    public const UInt32 MP_NOTUNITTYPE 						= 0x4B0B120C;   /* Undefined unit type                                                                                                                                 */
	    public const UInt32 MP_NOTCOMDEVICETYPE 				= 0x470B120D;   /* Undefined ComDevice type                                                                                                                            */
	    public const UInt32 MP_NOTCONTROLTYPE 					= 0x470B120E;   /* Undefined Control type                                                                                                                              */
	    public const UInt32 MP_NOTFILETYPE 						= 0x4B0B120F;   /* Undefined file type                                                                                                                                 */
	    public const UInt32 MP_NOTSEMAPHORETYPE 				= 0x470B1210;   /* Undefined semaphore type                                                                                                                            */
	    public const UInt32 MP_NOTSYSTEMOPTION 					= 0x470B1211;   /* Undefined system option                                                                                                                             */
	    public const UInt32 MP_TIMEOUT_ERR 						= 0x470B1212;   /* Timeout error                                                                                                                                       */
	    public const UInt32 MP_TIMEOUT 							= 0x470B1213;   /* Timeout                                                                                                                                             */
	    public const UInt32 MP_NOTSUBSCRIBETYPE 				= 0x470B1214;   /* Undefined scan type                                                                                                                                 */
	    public const UInt32 MP_NOTSCANTYPE 						= 0x4B0B1214;   /* Undefined scan type                                                                                                                                 */
	    public const UInt32 MP_DEVICEOFFSETOVER 				= 0x470B1300;   /* Out-of-range device offset                                                                                                                          */
	    public const UInt32 MP_DEVICEBITOFFSETOVER 				= 0x470B1301;   /* Out-of-range bit offset                                                                                                                             */
	    public const UInt32 MP_UNITCOUNTOVER 					= 0x4B0B1302;   /* Out-of-range quantity                                                                                                                               */
	    public const UInt32 MP_COMPAREVALUEOVER 				= 0x4B0B1303;   /* Out-of-range compared value                                                                                                                         */
	    public const UInt32 MP_FCOMPAREVALUEOVER 				= 0x4B0B1304;   /* Out-of-range floating-point compared value                                                                                                          */
	    public const UInt32 MP_EVENTCOUNTOVER 					= 0x470B1305;   /* Out-of-range virtual interruption number                                                                                                            */
	    public const UInt32 MP_VALUEOVER 						= 0x470B1306;   /* Out-of-range value                                                                                                                                  */
	    public const UInt32 MP_FVALUEOVER 						= 0x470B1307;   /* Out-of-range floating point                                                                                                                         */
	    public const UInt32 MP_PSTOREDVALUEOVER 				= 0x470B1308;   /* Out-of-range storage position pointer                                                                                                               */
	    public const UInt32 MP_PSTOREDFVALUEOVER 				= 0x470B1309;   /* Out-of-range storage position pointer (floating decimal point value)                                                                                */
	    public const UInt32 MP_SIZEOVER 						= 0x470B130A;   /* Out-of-range size                                                                                                                                   */
	    public const UInt32 MP_RECEIVETIMEROVER 				= 0x470B1310;   /* Out-of-range waiting time value for receiving                                                                                                       */
	    public const UInt32 MP_RECNUMOVER 						= 0x470B1311;   /* Out-of-range number of records (MoveList)                                                                                                           */
	    public const UInt32 MP_PARAMOVER 						= 0x4B0B1312;   /* Out-of-range parameter                                                                                                                              */
	    public const UInt32 MP_FRAMEOVER 						= 0x470B1313;   /* Out-of-range number of frames                                                                                                                       */
	    public const UInt32 MP_RADIUSOVER 						= 0x4B0B1314;   /* Out-of-range radius                                                                                                                                 */
	    public const UInt32 MP_CONTROLLERNOOVER 				= 0x470B1315;   /* Out-of-range device number                                                                                                                          */
	    public const UInt32 MP_AXISNUMOVER 						= 0x4B0B1316;   /* Out-of-range number of axes                                                                                                                         */
	    public const UInt32 MP_DIGITOVER 						= 0x4B0B1317;   /* Out-of-range number of digits                                                                                                                       */
	    public const UInt32 MP_CALENDARVALUEOVER 				= 0x4B0B1318;   /* Out-of-range calendar data                                                                                                                          */
	    public const UInt32 MP_OFFSETOVER 						= 0x470B1319;   /* Out-of-range offset                                                                                                                                 */
	    public const UInt32 MP_NUMBEROVER 						= 0x470B131A;   /* Out-of-range number of registers has been specified.                                                                                                */
	    public const UInt32 MP_RACKOVER 						= 0x470B131B;   /* Out-of-range rack number has been specified.                                                                                                        */
	    public const UInt32 MP_SLOTOVER 						= 0x470B131C;   /* Out-of-range slot number has been specified.                                                                                                        */
	    public const UInt32 MP_FIXVALUEOVER 					= 0x470B131D;   /* Fixed decimal point type data has been out of range.                                                                                                */
        public const UInt32 MP_REGISTERINFOROVER                = 0x470B131E;	/* Out-of-range number of register infomation has been specified.                                                                                      */
	    public const UInt32 PC_MEMORY_ERR 						= 0x430B1400;   /* PC memory shortage                                                                                                                                  */
	    public const UInt32 MP_NOCOMMUDEVICETYPE 				= 0x470B1500;   /* Communication device type specification error                                                                                                       */
	    public const UInt32 MP_NOTPROTOCOLTYPE 					= 0x470B1501;   /* Invalid protocol type                                                                                                                               */
	    public const UInt32 MP_NOTFUNCMODE 						= 0x470B1502;   /* Invalid function mode                                                                                                                               */
	    public const UInt32 MP_MYADDROVER 						= 0x470B1503;   /* Out-of-range local station address has been set.                                                                                                    */
	    public const UInt32 MP_NOTPORTTYPE 						= 0x470B1504;   /* Invalid port type                                                                                                                                   */
	    public const UInt32 MP_NOTPROTMODE 						= 0x470B1505;   /* Invalid protocol mode                                                                                                                               */
	    public const UInt32 MP_NOTCHARSIZE 						= 0x470B1506;   /* Invalid character bit length                                                                                                                        */
	    public const UInt32 MP_NOTPARITYBIT 					= 0x470B1507;   /* Invalid parity bit                                                                                                                                  */
	    public const UInt32 MP_NOTSTOPBIT 						= 0x470B1508;   /* Invalid stop bit                                                                                                                                    */
	    public const UInt32 MP_NOTBAUDRAT 						= 0x470B1509;   /* Invalid transmission speed                                                                                                                          */
	    public const UInt32 MP_TRANSDELAYOVER 					= 0x470B1510;   /* Out-of-range sending delay has been specified.                                                                                                      */
	    public const UInt32 MP_PEERADDROVER 					= 0x470B1511;   /* Out-of-range remote station address has been specified.                                                                                             */
	    public const UInt32 MP_SUBNETMASKOVER 					= 0x470B1512;   /* Out-of-range subnet mask has been specified.                                                                                                        */
	    public const UInt32 MP_GWADDROVER 						= 0x470B1513;   /* Out-of-range GW address has been specified.                                                                                                         */
	    public const UInt32 MP_DIAGPORTOVER 					= 0x470B1514;   /* Out-of-range diagnostic port has been specified.                                                                                                    */
	    public const UInt32 MP_PROCRETRYOVER 					= 0x470B1515;   /* Out-of-range number of retries has been specified.                                                                                                  */
	    public const UInt32 MP_TCPZEROWINOVER 					= 0x470B1516;   /* Out-of-range TCP zero window timer                                                                                                                  */
	    public const UInt32 MP_TCPRETRYOVER 					= 0x470B1517;   /* Out-of-range TCP resending timer value                                                                                                              */
	    public const UInt32 MP_TCPFINOVER 						= 0x470B1518;   /* Out-of-range completion timer value                                                                                                                 */
	    public const UInt32 MP_IPASSEMBLEOVER 					= 0x470B1519;   /* Out-of-range IP incorporating timer value                                                                                                           */
	    public const UInt32 MP_MAXPKTLENOVER 					= 0x470B1520;   /* Out-of-range maximum packet length                                                                                                                  */
	    public const UInt32 MP_PEERPORTOVER 					= 0x470B1521;   /* Out-of-range remote station port                                                                                                                    */
	    public const UInt32 MP_MYPORTOVER 						= 0x470B1522;   /* Out-of-range local station port                                                                                                                     */
	    public const UInt32 MP_NOTTRANSPROT 					= 0x470B1523;   /* Invalid transport layer protocol                                                                                                                    */
	    public const UInt32 MP_NOTAPPROT 						= 0x470B1524;   /* Invalid application layer protocol                                                                                                                  */
	    public const UInt32 MP_NOTCODETYPE 						= 0x470B1525;   /* Invalid code type                                                                                                                                   */
	    public const UInt32 MP_CYCTIMOVER 						= 0x470B1526;   /* Out-of-range communication cycle has been specified.                                                                                                */
	    public const UInt32 MP_NOTIOMAPCOM 						= 0x470B1527;   /* Invalid I/O commands                                                                                                                                */
	    public const UInt32 MP_NOTIOTYPE 						= 0x470B1528;   /* Invalid I/O type specification                                                                                                                      */
	    public const UInt32 MP_IOOFFSETOVER 					= 0x470B1529;   /* Out-of-range I/O offset has been allocated.                                                                                                         */
	    public const UInt32 MP_IOSIZEOVER 						= 0x470B1530;   /* Out-of-range I/O size has been allocated (individualy).                                                                                             */
	    public const UInt32 MP_TIOSIZEOVER 						= 0x470B1531;   /* Out-of-range I/O size has been allocated (total).                                                                                                   */
	    public const UInt32 MP_MEMORY_ERR 						= 0x470B1532;   /* Controller memory shortage                                                                                                                          */
	    public const UInt32 MP_NOTPTR 							= 0x470B1533;   /* Invalid pointer (communication device specification structure/device inherent information/pointer error to objective communication device handle)   */
	    public const UInt32 MP_TABLEOVER 						= 0x43001800;   /* Event detection table resource cannot be got.                                                                                                       */
	    public const UInt32 MP_ALARM 							= 0x43001801;   /* Alarm has occurred.                                                                                                                                 */
	    public const UInt32 MP_MEMORYOVER 						= 0x43001802;   /* Memory resource cannot be got.                                                                                                                      */
	    public const UInt32 MP_EXEC_ERR 						= 0x470B1803;   /* System execution error                                                                                                                              */
	    public const UInt32 MP_NOTLOGICALAXIS 					= 0x470B1804;   /* Logical axis number error                                                                                                                           */
	    public const UInt32 MP_NOTSUPPORTED 					= 0x470B1805;   /* Not supported                                                                                                                                       */
	    public const UInt32 MP_ILLTEXT 							= 0x470B1806;   /* Out-of-range length of character string was input.                                                                                                  */
	    public const UInt32 MP_NOFILENAME 						= 0x470B1807;   /* File name has not been set.                                                                                                                         */
	    public const UInt32 MP_NOTREGSTERNAME 					= 0x470B1808;   /* Specified register data name cannot be found.                                                                                                       */
	    public const UInt32 MP_FILEALREADYOPEN 					= 0x4B0B1809;   /* The same file has already been opened.                                                                                                              */
	    public const UInt32 MP_NOFILEPACKET 					= 0x470B180A;   /* Specified source file packet cannot be found.                                                                                                       */
	    public const UInt32 MP_NOTFILEPACKETSIZE 				= 0x470B180B;   /* Source file packet size is incorrect.                                                                                                               */
	    public const UInt32 MP_NOTUSERFUNCTION 					= 0x4B0B180C;   /* Specified user funtion does not exist.                                                                                                              */
	    public const UInt32 MP_NOTPARAMETERTYPE 				= 0x4B0B180D;   /* Specified parameter type does not exist.                                                                                                            */
	    public const UInt32 MP_ILLREGHANDLETYPE 				= 0x470B180E;   /* Erroneous register handle type specified.                                                                                                           */
	    public const UInt32 MP_ILLREGTYPE 						= 0x470B1810;   /* Erroneous register type specified.                                                                                                                  */
	    public const UInt32 MP_ILLREGSIZE 						= 0x470B1811;   /* Erroneous register size specified.(other than WORD)                                                                                                 */
	    public const UInt32 MP_REGNUMOVER 						= 0x470B1812;   /* Out-of-range register                                                                                                                               */
	    public const UInt32 MP_RELEASEWAIT 						= 0x470B1813;   /* Waiting status released                                                                                                                             */
	    public const UInt32 MP_PRIORITYOVER 					= 0x470B1814;   /* Priority that can not be set                                                                                                                        */
	    public const UInt32 MP_NOTCHANGEPRIORITY 				= 0x470B1815;   /* Priority that cannot be changed                                                                                                                     */
	    public const UInt32 MP_SEMAPHOREOVER 					= 0x470B1816;   /* Semaphore definition over                                                                                                                           */
	    public const UInt32 MP_NOTSEMAPHOREHANDLE 				= 0x470B1817;   /* Undefined semaphore handle specification                                                                                                            */
	    public const UInt32 MP_SEMAPHORELOCKED 					= 0x470B1818;   /* Specified semaphore handle being locked                                                                                                             */
	    public const UInt32 MP_CONTINUE_RELEASEWAIT 			= 0x470B1819;   /* Waiting status released during ymcContinueWaitForCompletion                                                                                         */
	    public const UInt32 MP_NOTTABLENAME 					= 0x4B0B1820;   /* Undefined Table name                                                                                                                                */
	    public const UInt32 MP_ILLTABLETYPE 					= 0x470B1821;   /* Illegal Table Type                                                                                                                                  */
	    public const UInt32 MP_TIMEOUTVALUEOVER 				= 0x470B1822;   /* Out-of-range timeout value has been specified                                                                                                       */
	    public const UInt32 MP_OTHER_ERR 						= 0x470B19FF;   /* Other errors                                                                                                                                        */

        public static Dictionary<string, string> ErrorDictionary = new Dictionary<string, string>()
        {
            { "00000000", "MP_SUCCESS                               " },
            { "4000FFFF", "MP_FAIL 							        " },
            { "81000001", "WDT_OVER_ERR 						    " },
            { "82000020", "MANUAL_RESET_ERR 					    " },
            { "82000140", "TLB_MLTHIT_ERR 						    " },
            { "820001E0", "UBRK_ERR 							    " },
            { "820000E0", "ADR_RD_ERR 							    " },
            { "82000040", "TLB_MIS_RD_ERR 						    " },
            { "820000A0", "TLB_PROTECTION_RD_ERR 				    " },
            { "82000180", "GENERAL_INVALID_INS_ERR 			        " },
            { "820001A0", "SLOT_ERR 							    " },
            { "82000800", "GENERAL_FPU_DISABLE_ERR 			        " },
            { "82000820", "SLOT_FPU_ERR 						    " },
            { "82000100", "ADR_WR_ERR 							    " },
            { "82000060", "TLB_MIS_WR_ERR 						    " },
            { "820000C0", "TLB_PROTECTION_WR_ERR 				    " },
            { "82000120", "FPU_EXP_ERR 						        " },
            { "82000080", "INITIAL_PAGE_EXP_ERR 				    " },
            { "81000041", "ROM_ERR 							        " },
            { "81000042", "RAM_ERR 							        " },
            { "81000043", "MPU_ERR 							        " },
            { "81000044", "FPU_ERR 							        " },
            { "81000049", "CERF_ERR 							    " },
            { "81000050", "EXIO_ERR 							    " },
            { "8100005F", "BUSIF_ERR 							    " },
            //{ "00000000", "ALM_NO_ALM 							    " },
            { "67050300", "ALM_MK_DEBUG 						    " },
            { "67050301", "ALM_MK_ROUND_ERR 					    " },
            { "67050302", "ALM_MK_FSPEED_OVER 					    " },
            { "67050303", "ALM_MK_FSPEED_NOSPEC 				    " },
            { "67050304", "ALM_MK_PRM_OVER 					        " },
            { "67050305", "ALM_MK_ARCLEN_OVER 					    " },
            { "67050306", "ALM_MK_VERT_NOSPEC 					    " },
            { "67050307", "ALM_MK_HORZ_NOSPEC 					    " },
            { "67050308", "ALM_MK_TURN_OVER 					    " },
            { "67050309", "ALM_MK_RADIUS_OVER 					    " },
            { "6705030A", "ALM_MK_CENTER_ERR 					    " },
            { "6705030B", "ALM_MK_BLOCK_OVER 					    " },
            { "6705030C", "ALM_MK_MAXF_NOSPEC 					    " },
            { "6705030D", "ALM_MK_TDATA_ERR 					    " },
            { "6705030E", "ALM_MK_REG_ERR 						    " },
            { "6705030F", "ALM_MK_COMMAND_CODE_ERR 			        " },
            { "67050310", "ALM_MK_AXIS_CONFLICT 				    " },
            { "67050311", "ALM_MK_POSMAX_OVER 					    " },
            { "67050312", "ALM_MK_DIST_OVER 					    " },
            { "67050313", "ALM_MK_MODE_ERR 					        " },
            { "67050314", "ALM_MK_CMD_CONFLICT 				        " },
            { "67050315", "ALM_MK_RCMD_CONFLICT 				    " },
            { "67050316", "ALM_MK_CMD_MODE_ERR 				        " },
            { "67050317", "ALM_MK_CMD_NOT_ALLOW 				    " },
            { "67050318", "ALM_MK_CMD_DEN_FAIL 				        " },
            { "67050319", "ALM_MK_H_MOVE_ERR 					    " },
            { "6705031A", "ALM_MK_MOVE_NOT_SUPPORT 			        " },
            { "6705031B", "ALM_MK_EVENT_NOT_SUPPORT 			    " },
            { "6705031C", "ALM_MK_ACTION_NOT_SUPPORT 			    " },
            { "6705031D", "ALM_MK_MOVE_TYPE_ERR 				    " },
            { "6705031E", "ALM_MK_VTYPE_ERR 					    " },
            { "6705031F", "ALM_MK_ATYPE_ERR 					    " },
            { "67050320", "ALM_MK_HOMING_METHOD_ERR 			    " },
            { "67050321", "ALM_MK_ACC_ERR 						    " },
            { "67050322", "ALM_MK_DEC_ERR 						    " },
            { "67050323", "ALM_MK_POS_TYPE_ERR 				        " },
            { "67050324", "ALM_MK_INVALID_EVENT_ERR 			    " },
            { "67050325", "ALM_MK_INVALID_ACTION_ERR 			    " },
            { "67050326", "ALM_MK_MOVE_NOT_ACTIVE 				    " },
            { "67050327", "ALM_MK_MOVELIST_NOT_ACTIVE 			    " },
            { "67050328", "ALM_MK_TBL_INVALID_DATA 			        " },
            { "67050329", "ALM_MK_TBL_INVALID_SEG_NUM 			    " },
            { "6705032A", "ALM_MK_TBL_INVALID_AXIS_NUM 		        " },
            { "6705032B", "ALM_MK_TBL_INVALID_ST_SEG 			    " },
            { "6705032C", "ALM_MK_STBL_INVALID_EXE 			        " },
            { "6705032D", "ALM_MK_DTBL_DUPLICATE_EXE 			    " },
            { "6705032E", "ALM_MK_LATCH_CONFLICT 				    " },
            { "6705032F", "ALM_MK_INVALID_AXISTYPE 			        " },
            { "67050330", "ALM_MK_NOT_SVCRDY 					    " },
            { "67050331", "ALM_MK_NOT_SVCRUN 					    " },
            { "67050332", "ALM_MK_MDALARM 						    " },
            { "67050333", "ALM_MK_SUPERPOSE_MASTER_ERR 		        " },
            { "67050334", "ALM_MK_SUPERPOSE_SLAVE_ERR 			    " },
            { "57050335", "ALM_MK_MDWARNING 					    " },
            { "57050336", "ALM_MK_MDWARNING_POSERR 			        " },
            { "67050337", "ALM_MK_NOT_INFINITE_ABS 			        " },
            { "67050338", "ALM_MK_INVALID_LOGICAL_NUM 			    " },
            { "67050339", "ALM_MK_MAX_VELOCITY_ERR 			        " },
            { "6705033A", "ALM_MK_VELOCITY_ERR 				        " },
            { "6705033B", "ALM_MK_APPROACH_ERR 				        " },
            { "6705033C", "ALM_MK_CREEP_ERR 					    " },
            { "83050340", "ALM_MK_OS_ERR_MBOX1 				        " },
            { "83050341", "ALM_MK_OS_ERR_MBOX2 				        " },
            { "83050342", "ALM_MK_OS_ERR_SEND_MSG1 			        " },
            { "83050343", "ALM_MK_OS_ERR_SEND_MSG2 			        " },
            { "83050344", "ALM_MK_OS_ERR_SEND_MSG3 			        " },
            { "83050345", "ALM_MK_OS_ERR_SEND_MSG4 			        " },
            { "53050346", "ALM_MK_ACTION_ERR1 					    " },
            { "53050347", "ALM_MK_ACTION_ERR2 					    " },
            { "53050348", "ALM_MK_ACTION_ERR3 					    " },
            { "53050349", "ALM_MK_RCV_INV_MSG1 				        " },
            { "5305034A", "ALM_MK_RCV_INV_MSG2 				        " },
            { "5305034B", "ALM_MK_RCV_INV_MSG3 				        " },
            { "5305034C", "ALM_MK_RCV_INV_MSG4 				        " },
            { "5305034D", "ALM_MK_RCV_INV_MSG5 				        " },
            { "5305034E", "ALM_MK_RCV_INV_MSG6 				        " },
            { "5305034F", "ALM_MK_RCV_INV_MSG7 				        " },
            { "53050350", "ALM_MK_RCV_INV_MSG8 				        " },
            { "67050360", "ALM_MK_AXIS_HANDLE_ERROR 			    " },
            { "67050361", "ALM_MK_SLAVE_USED_AS_MASTER 		        " },
            { "67050362", "ALM_MK_MASTER_USED_AS_SLAVE 		        " },
            { "67050363", "ALM_MK_SLAVE_HAS_2_MASTERS 			    " },
            { "67050364", "ALM_MK_SLAVE_HAS_NOT_WORK 			    " },
            { "67050365", "ALM_MK_PARAM_OUT_OF_RANGE 			    " },
            { "67050366", "ALM_MK_NNUM_MAX_OVER 				    " },
            { "67050367", "ALM_MK_FGNTBL_INVALID 				    " },
            { "67050368", "ALM_MK_PARAM_OVERFLOW 				    " },
            { "67050369", "ALM_MK_ALREADY_COMMANDED 			    " },
            { "6705036A", "ALM_MK_MULTIPLE_SHIFTS 				    " },
            { "6705036B", "ALM_MK_CAMTBL_INVALID 				    " },
            { "6705036C", "ALM_MK_ABORTED_BY_STOPMTN 			    " },
            { "6705036D", "ALM_MK_HMETHOD_INVALID 				    " },
            { "6705036E", "ALM_MK_MASTER_INVALID 				    " },
            { "6705036F", "ALM_MK_DATA_HANDLE_INVALID 			    " },
            { "67050370", "ALM_MK_UNKNOWN_CAM_GEAR_ERR 		        " },
            { "67050371", "ALM_MK_REG_SIZE_INVALID 			        " },
            { "67050372", "ALM_MK_ACTION_HANDLE_ERROR 			    " },
            { "83040380", "ALM_MM_OS_ERR_MBOX1 				        " },
            { "83040381", "ALM_MM_OS_ERR_SEND_MSG1 			        " },
            { "83040382", "ALM_MM_OS_ERR_SEND_MSG2 			        " },
            { "83040383", "ALM_MM_OS_ERR_RCV_MSG1 				    " },
            { "67040384", "ALM_MM_MK_BUSY 						    " },
            { "53040385", "ALM_MM_RCV_INV_MSG1 				        " },
            { "53040386", "ALM_MM_RCV_INV_MSG2 				        " },
            { "53040387", "ALM_MM_RCV_INV_MSG3 				        " },
            { "53040388", "ALM_MM_RCV_INV_MSG4 				        " },
            { "53040389", "ALM_MM_RCV_INV_MSG5 				        " },
            { "53060480", "ALM_IM_DEVICEID_ERR 				        " },
            { "53060481", "ALM_IM_REGHANDLE_ERR 				    " },
            { "53060482", "ALM_IM_GLOBALHANDLE_ERR 			        " },
            { "53060483", "ALM_IM_DEVICETYPE_ERR 				    " },
            { "53060484", "ALM_IM_OFFSET_ERR 					    " },
            { "57020501", "AM_ER_UNDEF_COMMAND 				        " },
            { "57020502", "AM_ER_UNDEF_CMNDTYPE 				    " },
            { "57020503", "AM_ER_UNDEF_OBJTYPE 				        " },
            { "57020504", "AM_ER_UNDEF_HANDLETYPE 				    " },
            { "57020505", "AM_ER_UNDEF_PKTDAT 					    " },
            { "57020506", "AM_ER_UNDEF_AXIS 					    " },
            { "57020510", "AM_ER_MSGBUF_GET_FAULT 				    " },
            { "57020511", "AM_ER_ACTSIZE_GET_FAULT 			        " },
            { "57020512", "AM_ER_APIBUF_GET_FAULT 				    " },
            { "57020513", "AM_ER_MOVEOBJ_GET_FAULT 			        " },
            { "57020514", "AM_ER_EVTTBL_GET_FAULT 				    " },
            { "57020515", "AM_ER_ACTTBL_GET_FAULT 				    " },
            { "57020516", "AM_ER_1BY1APIBUF_GET_FAULT 			    " },
            { "57020517", "AM_ER_AXSTBL_GET_FAULT 				    " },
            { "57020518", "AM_ER_SUPERPOSEOBJ_GET_FAULT 		    " },
            { "57020519", "AM_ER_SUPERPOSEOBJ_CLEAR_FAULT 		    " },
            { "5702051A", "AM_ER_AXIS_IN_USE 					    " },
            { "5702051B", "AM_ER_HASHTBL_GET_FAULT 			        " },
            { "57020530", "AM_ER_UNMATCH_OBJHNDL 				    " },
            { "57020531", "AM_ER_UNMATCH_OBJECT 				    " },
            { "57020532", "AM_ER_UNMATCH_APIBUF 				    " },
            { "57020533", "AM_ER_UNMATCH_MSGBUF 				    " },
            { "57020534", "AM_ER_UNMATCH_ACTBUF 				    " },
            { "57020535", "AM_ER_UNMATH_SEQUENCE 				    " },
            { "57020536", "AM_ER_UNMATCH_1BY1APIBUF 			    " },
            { "57020537", "AM_ER_UNMATCH_MOVEOBJTABLE 			    " },
            { "57020538", "AM_ER_UNMATCH_MOVELISTTABLE 		        " },
            { "57020539", "AM_ER_UNMATCH_MOVELIST_OBJECT 		    " },
            { "5702053A", "AM_ER_UNMATCH_MOVELIST_OBJHNDL 		    " },
            { "57020550", "AM_ER_UNGET_MOVEOBJTABLE 			    " },
            { "57020551", "AM_ER_UNGET_MOVELISTTABLE 			    " },
            { "57020552", "AM_ER_UNGET_1BY1APIBUFTABLE 		        " },
            { "57020560", "AM_ER_NOEMPTYTBL_ERROR 				    " },
            { "57020561", "AM_ER_NOTGETSEM_ERROR 				    " },
            { "57020562", "AM_ER_NOTGETTBLADD_ERROR 			    " },
            { "57020563", "AM_ER_NOTWRTTBL_ERROR 				    " },
            { "57020564", "AM_ER_TBLINDEX_ERROR 				    " },
            { "57020565", "AM_ER_ILLTBLTYPE_ERROR 				    " },
            { "57020570", "AM_ER_UNSUPORTED_EVENT 				    " },
            { "57020571", "AM_ER_WRONG_SEQUENCE 				    " },
            { "57020572", "AM_ER_MOVEOBJ_BUSY 					    " },
            { "57020573", "AM_ER_MOVELIST_BUSY 				        " },
            { "57020574", "AM_ER_MOVELIST_ADD_FAULT 			    " },
            { "57020575", "AM_ER_CONFLICT_PHI_AXS 				    " },
            { "57020576", "AM_ER_CONFLICT_LOG_AXS 				    " },
            { "57020577", "AM_ER_PKTSTS_ERROR 					    " },
            { "57020578", "AM_ER_CONFLICT_NAME 				        " },
            { "57020579", "AM_ER_ILLEGAL_NAME 					    " },
            { "5702057A", "AM_ER_SEMAPHORE_ERROR 				    " },
            { "5702057B", "AM_ER_LOG_AXS_OVER 					    " },
            { "55060B00", "IM_STATION_ERR 						    " },
            { "55060B01", "IM_IO_ERR 							    " },
            { "53168001", "MP_FILE_ERR_GENERAL 				        " },
            { "53168002", "MP_FILE_ERR_NOT_SUPPORTED 			    " },
            { "53168003", "MP_FILE_ERR_INVALID_ARGUMENT 		    " },
            { "53168004", "MP_FILE_ERR_INVALID_HANDLE 			    " },
            { "53168064", "MP_FILE_ERR_NO_FILE 				        " },
            { "53168065", "MP_FILE_ERR_INVALID_PATH 			    " },
            { "53168066", "MP_FILE_ERR_EOF 					        " },
            { "53168067", "MP_FILE_ERR_PERMISSION_DENIED 		    " },
            { "53168068", "MP_FILE_ERR_TOO_MANY_FILES 			    " },
            { "53168069", "MP_FILE_ERR_FILE_BUSY 				    " },
            { "5316806A", "MP_FILE_ERR_TIMEOUT 				        " },
            { "531680C8", "MP_FILE_ERR_BAD_FS 					    " },
            { "531680C9", "MP_FILE_ERR_FILESYSTEM_FULL 		        " },
            { "531680CA", "MP_FILE_ERR_INVALID_LFM 			        " },
            { "531680CB", "MP_FILE_ERR_TOO_MANY_LFM 			    " },
            { "5316812C", "MP_FILE_ERR_INVALID_PDM 			        " },
            { "5316812D", "MP_FILE_ERR_INVALID_MEDIA 			    " },
            { "5316812E", "MP_FILE_ERR_TOO_MANY_PDM 			    " },
            { "5316812F", "MP_FILE_ERR_TOO_MANY_MEDIA 			    " },
            { "53168130", "MP_FILE_ERR_WRITE_PROTECTED 		        " },
            { "53168190", "MP_FILE_ERR_INVALID_DEVICE 			    " },
            { "53168191", "MP_FILE_ERR_DEVICE_IO 				    " },
            { "53168192", "MP_FILE_ERR_DEVICE_BUSY 			        " },
            { "5316A711", "MP_FILE_ERR_NO_CARD 				        " },
            { "5316A712", "MP_FILE_ERR_CARD_POWER 				    " },
            { "53178FFF", "MP_CARD_SYSTEM_ERR 					    " },
            { "83001A01", "ERROR_CODE_GET_DIREC_OFFSET 		        " },
            { "83001A02", "ERROR_CODE_GET_DIREC_INFO 			    " },
            { "83001A03", "ERROR_CODE_FUNC_TABLE                    " },
            { "83001A04", "ERROR_CODE_SLEEP_TASK                    " },
            { "43001A41", "ERROR_CODE_DEVICE_HANDLE_FULL            " },
            { "43001A42", "ERROR_CODE_ALLOC_MEMORY                  " },
            { "43001A43", "ERROR_CODE_BUFCOPY                       " },
            { "43001A44", "ERROR_CODE_GET_COMMEM_OFFSET             " },
            { "43001A45", "ERROR_CODE_CREATE_SEMPH                  " },
            { "43001A46", "ERROR_CODE_DELETE_SEMPH                  " },
            { "43001A47", "ERROR_CODE_LOCK_SEMPH                    " },
            { "43001A48", "ERROR_CODE_UNLOCK_SEMPH                  " },
            { "43001A49", "ERROR_CODE_PACKETSIZE_OVER               " },
            { "43001A4A", "ERROR_CODE_UNREADY                       " },
            { "43001A4B", "ERROR_CODE_CPUSTOP                       " },
            { "470B1A81", "ERROR_CODE_CNTRNO                        " },
            { "470B1A82", "ERROR_CODE_SELECTION                     " },
            { "470B1A83", "ERROR_CODE_LENGTH                        " },
            { "470B1A84", "ERROR_CODE_OFFSET                        " },
            { "470B1A85", "ERROR_CODE_DATACOUNT                     " },
            { "46001A86", "ERROR_CODE_DATREAD                       " },
            { "46001A87", "ERROR_CODE_DATWRITE                      " },
            { "46001A88", "ERROR_CODE_BITWRITE                      " },
            { "46001A89", "ERROR_CODE_DEVCNTR                       " },
            { "460F1A8A", "ERROR_CODE_NOTINIT                       " },
            { "41001A8B", "ERROR_CODE_SEMPHLOCK                     " },
            { "41001A8C", "ERROR_CODE_SEMPHUNLOCK                   " },
            { "460F1A8D", "ERROR_CODE_DRV_PROC                      " },
            { "460F1A8E", "ERROR_CODE_GET_DRIVER_HANDLE             " },
            { "450E1AC1", "ERROR_CODE_SEND_MSG                      " },
            { "450E1AC2", "ERROR_CODE_RECV_MSG                      " },
            { "450E1AC3", "ERROR_CODE_INVALID_RESPONSE              " },
            { "450E1AC4", "ERROR_CODE_INVALID_ID                    " },
            { "450E1AC5", "ERROR_CODE_INVALID_STATUS                " },
            { "450E1AC6", "ERROR_CODE_INVALID_CMDCODE               " },
            { "450E1AC7", "ERROR_CODE_INVALID_SEQNO                 " },
            { "450E1AC8", "ERROR_CODE_SEND_RETRY_OVER               " },
            { "450E1AC9", "ERROR_CODE_RECV_RETRY_OVER               " },
            { "450E1ACA", "ERROR_CODE_RESPONSE_TIMEOUT              " },
            { "450E1ACB", "ERROR_CODE_WAIT_FOR_EVENT                " },
            { "450E1ACC", "ERROR_CODE_EVENT_OPEN                    " },
            { "450E1ACD", "ERROR_CODE_EVENT_RESET                   " },
            { "450E1ACE", "ERROR_CODE_EVENT_READY                   " },
            { "43001B01", "ERROR_CODE_PROCESSNUM                    " },
            { "43001B02", "ERROR_CODE_GET_PROC_INFO                 " },
            { "43001B03", "ERROR_CODE_THREADNUM                     " },
            { "43001B04", "ERROR_CODE_GET_THRD_INFO                 " },
            { "43001B05", "ERROR_CODE_CREATE_MBOX                   " },
            { "43001B06", "ERROR_CODE_DELETE_MBOX                   " },
            { "83001B07", "ERROR_CODE_GET_TASKID                    " },
            { "43001B08", "ERROR_CODE_NO_THREADINFO                 " },
            { "43001B09", "ERROR_CODE_COM_INITIALIZE                " },
            { "430F1B41", "ERROR_CODE_COMDEVICENUM                  " },
            { "430F1B42", "ERROR_CODE_GET_COM_DEVICE_HANDLE         " },
            { "430F1B43", "ERROR_CODE_COM_DEVICE_FULL               " },
            { "430F1B44", "ERROR_CODE_CREATE_PANELOBJ               " },
            { "430F1B45", "ERROR_CODE_OPEN_PANELOBJ                 " },
            { "430F1B46", "ERROR_CODE_CLOSE_PANELOBJ                " },
            { "430F1B47", "ERROR_CODE_PROC_PANELOBJ                 " },
            { "430F1B48", "ERROR_CODE_CREATE_CNTROBJ                " },
            { "430F1B49", "ERROR_CODE_OPEN_CNTROBJ                  " },
            { "430F1B4A", "ERROR_CODE_CLOSE_CNTROBJ                 " },
            { "430F1B4B", "ERROR_CODE_PROC_CNTROBJ                  " },
            { "430F1B4C", "ERROR_CODE_CREATE_COMDEV_MUTEX           " },
            { "430F1B4D", "ERROR_CODE_CREATE_COMDEV_MAPFILE         " },
            { "430F1B4E", "ERROR_CODE_OPEN_COMDEV_MAPFILE           " },
            { "430F1B4F", "ERROR_CODE_NOT_OBJECT_TYPE               " },
            { "430F1B50", "ERROR_CODE_COM_NOT_OPENED                " },
            { "43081B80", "ERROR_CODE_PNLCMD_CPU_CONTROL            " },
            { "43081B81", "ERROR_CODE_PNLCMD_SFILE_READ             " },
            { "43081B82", "ERROR_CODE_PNLCMD_SFILE_WRITE            " },
            { "43081B83", "ERROR_CODE_PNLCMD_REGISTER_READ          " },
            { "43081B84", "ERROR_CODE_PNLCMD_REGISTER_WRITE         " },
            { "43081B85", "ERROR_CODE_PNLCMD_API_SEND               " },
            { "43081B86", "ERROR_CODE_PNLCMD_API_RECV               " },
            { "43081B87", "ERROR_CODE_PNLCMD_NO_RESPONSE            " },
            { "43081B88", "ERROR_CODE_PNLCMD_PACKET_READ            " },
            { "43081B89", "ERROR_CODE_PNLCMD_PACKET_WRITE           " },
            { "43081B8A", "ERROR_CODE_PNLCMD_STATUS_READ            " },
            { "440D1BA0", "ERROR_CODE_NOT_CONTROLLER_RDY            " },
            { "440D1BA1", "ERROR_CODE_DOUBLE_CMD                    " },
            { "440D1BA2", "ERROR_CODE_DOUBLE_RCMD                   " },
            { "43001BC1", "ERROR_CODE_FILE_NOT_OPENED               " },
            { "43001BC2", "ERROR_CODE_OPENFILE_NUM                  " },
            { "4308106f", "MP_CTRL_SYS_ERROR                        " },
            { "43081092", "MP_CTRL_PARAM_ERR                        " },
            { "43081094", "MP_CTRL_FILE_NOT_FOUND                   " },
            { "470B1100", "MP_NOTMOVEHANDLE 					    " },
            { "470B1101", "MP_NOTTIMERHANDLE 					    " },
            { "470B1102", "MP_NOTINTERRUPT 					        " },
            { "470B1103", "MP_NOTEVENTHANDLE 					    " },
            { "470B1104", "MP_NOTMESSAGEHANDLE 				        " },
            { "470B1105", "MP_NOTUSERFUNCTIONHANDLE 			    " },
            { "470B1106", "MP_NOTACTIONHANDLE 					    " },
            { "470B1107", "MP_NOTSUBSCRIBEHANDLE 				    " },
            { "470B1108", "MP_NOTDEVICEHANDLE 					    " },
            { "470B1109", "MP_NOTAXISHANDLE 					    " },
            { "470B110A", "MP_NOTMOVELISTHANDLE 				    " },
            { "470B110B", "MP_NOTTRACEHANDLE 					    " },
            { "470B110C", "MP_NOTGLOBALDATAHANDLE 				    " },
            { "470B110D", "MP_NOTSUPERPOSEHANDLE 				    " },
            { "470B110E", "MP_NOTCONTROLLERHANDLE 				    " },
            { "470B110F", "MP_NOTFILEHANDLE 					    " },
            { "470B1110", "MP_NOTREGISTERDATAHANDLE 			    " },
            { "470B1111", "MP_NOTALARMHANDLE 					    " },
            { "470B1112", "MP_NOTCAMHANDLE 					        " },
            { "470B11FF", "MP_NOTHANDLE 						    " },
            { "470B1200", "MP_NOTEVENTTYPE 					        " },
            { "470B1201", "MP_NOTDEVICETYPE 					    " },
            { "4B0B1202", "MP_NOTDATAUNITSIZE 					    " },
            { "470B1203", "MP_NOTDEVICE 						    " },
            { "470B1204", "MP_NOTACTIONTYPE 					    " },
            { "4B0B1205", "MP_NOTPARAMNAME 					        " },
            { "470B1206", "MP_NOTDATATYPE 						    " },
            { "470B1207", "MP_NOTBUFFERTYPE 					    " },
            { "4B0B1208", "MP_NOTMOVETYPE 						    " },
            { "470B1209", "MP_NOTANGLETYPE 					        " },
            { "4B0B120A", "MP_NOTDIRECTION 					        " },
            { "4B0B120B", "MP_NOTAXISTYPE 						    " },
            { "4B0B120C", "MP_NOTUNITTYPE 						    " },
            { "470B120D", "MP_NOTCOMDEVICETYPE 				        " },
            { "470B120E", "MP_NOTCONTROLTYPE 					    " },
            { "4B0B120F", "MP_NOTFILETYPE 						    " },
            { "470B1210", "MP_NOTSEMAPHORETYPE 				        " },
            { "470B1211", "MP_NOTSYSTEMOPTION 					    " },
            { "470B1212", "MP_TIMEOUT_ERR 						    " },
            { "470B1213", "MP_TIMEOUT 							    " },
            { "470B1214", "MP_NOTSUBSCRIBETYPE 				        " },
            { "4B0B1214", "MP_NOTSCANTYPE 						    " },
            { "470B1300", "MP_DEVICEOFFSETOVER 				        " },
            { "470B1301", "MP_DEVICEBITOFFSETOVER 				    " },
            { "4B0B1302", "MP_UNITCOUNTOVER 					    " },
            { "4B0B1303", "MP_COMPAREVALUEOVER 				        " },
            { "4B0B1304", "MP_FCOMPAREVALUEOVER 				    " },
            { "470B1305", "MP_EVENTCOUNTOVER 					    " },
            { "470B1306", "MP_VALUEOVER 						    " },
            { "470B1307", "MP_FVALUEOVER 						    " },
            { "470B1308", "MP_PSTOREDVALUEOVER 				        " },
            { "470B1309", "MP_PSTOREDFVALUEOVER 				    " },
            { "470B130A", "MP_SIZEOVER 						        " },
            { "470B1310", "MP_RECEIVETIMEROVER 				        " },
            { "470B1311", "MP_RECNUMOVER 						    " },
            { "4B0B1312", "MP_PARAMOVER 						    " },
            { "470B1313", "MP_FRAMEOVER 						    " },
            { "4B0B1314", "MP_RADIUSOVER 						    " },
            { "470B1315", "MP_CONTROLLERNOOVER 				        " },
            { "4B0B1316", "MP_AXISNUMOVER 						    " },
            { "4B0B1317", "MP_DIGITOVER 						    " },
            { "4B0B1318", "MP_CALENDARVALUEOVER 				    " },
            { "470B1319", "MP_OFFSETOVER 						    " },
            { "470B131A", "MP_NUMBEROVER 						    " },
            { "470B131B", "MP_RACKOVER 						        " },
            { "470B131C", "MP_SLOTOVER 						        " },
            { "470B131D", "MP_FIXVALUEOVER 					        " },
            { "470B131E", "MP_REGISTERINFOROVER                     " },
            { "430B1400", "PC_MEMORY_ERR 						    " },
            { "470B1500", "MP_NOCOMMUDEVICETYPE 				    " },
            { "470B1501", "MP_NOTPROTOCOLTYPE 					    " },
            { "470B1502", "MP_NOTFUNCMODE 						    " },
            { "470B1503", "MP_MYADDROVER 						    " },
            { "470B1504", "MP_NOTPORTTYPE 						    " },
            { "470B1505", "MP_NOTPROTMODE 						    " },
            { "470B1506", "MP_NOTCHARSIZE 						    " },
            { "470B1507", "MP_NOTPARITYBIT 					        " },
            { "470B1508", "MP_NOTSTOPBIT 						    " },
            { "470B1509", "MP_NOTBAUDRAT 						    " },
            { "470B1510", "MP_TRANSDELAYOVER 					    " },
            { "470B1511", "MP_PEERADDROVER 					        " },
            { "470B1512", "MP_SUBNETMASKOVER 					    " },
            { "470B1513", "MP_GWADDROVER 						    " },
            { "470B1514", "MP_DIAGPORTOVER 					        " },
            { "470B1515", "MP_PROCRETRYOVER 					    " },
            { "470B1516", "MP_TCPZEROWINOVER 					    " },
            { "470B1517", "MP_TCPRETRYOVER 					        " },
            { "470B1518", "MP_TCPFINOVER 						    " },
            { "470B1519", "MP_IPASSEMBLEOVER 					    " },
            { "470B1520", "MP_MAXPKTLENOVER 					    " },
            { "470B1521", "MP_PEERPORTOVER 					        " },
            { "470B1522", "MP_MYPORTOVER 						    " },
            { "470B1523", "MP_NOTTRANSPROT 					        " },
            { "470B1524", "MP_NOTAPPROT 						    " },
            { "470B1525", "MP_NOTCODETYPE 						    " },
            { "470B1526", "MP_CYCTIMOVER 						    " },
            { "470B1527", "MP_NOTIOMAPCOM 						    " },
            { "470B1528", "MP_NOTIOTYPE 						    " },
            { "470B1529", "MP_IOOFFSETOVER 					        " },
            { "470B1530", "MP_IOSIZEOVER 						    " },
            { "470B1531", "MP_TIOSIZEOVER 						    " },
            { "470B1532", "MP_MEMORY_ERR 						    " },
            { "470B1533", "MP_NOTPTR 							    " },
            { "43001800", "MP_TABLEOVER 						    " },
            { "43001801", "MP_ALARM 							    " },
            { "43001802", "MP_MEMORYOVER 						    " },
            { "470B1803", "MP_EXEC_ERR 						        " },
            { "470B1804", "MP_NOTLOGICALAXIS 					    " },
            { "470B1805", "MP_NOTSUPPORTED 					        " },
            { "470B1806", "MP_ILLTEXT 							    " },
            { "470B1807", "MP_NOFILENAME 						    " },
            { "470B1808", "MP_NOTREGSTERNAME 					    " },
            { "4B0B1809", "MP_FILEALREADYOPEN 					    " },
            { "470B180A", "MP_NOFILEPACKET 					        " },
            { "470B180B", "MP_NOTFILEPACKETSIZE 				    " },
            { "4B0B180C", "MP_NOTUSERFUNCTION 					    " },
            { "4B0B180D", "MP_NOTPARAMETERTYPE 				        " },
            { "470B180E", "MP_ILLREGHANDLETYPE 				        " },
            { "470B1810", "MP_ILLREGTYPE 						    " },
            { "470B1811", "MP_ILLREGSIZE 						    " },
            { "470B1812", "MP_REGNUMOVER 						    " },
            { "470B1813", "MP_RELEASEWAIT 						    " },
            { "470B1814", "MP_PRIORITYOVER 					        " },
            { "470B1815", "MP_NOTCHANGEPRIORITY 				    " },
            { "470B1816", "MP_SEMAPHOREOVER 					    " },
            { "470B1817", "MP_NOTSEMAPHOREHANDLE 				    " },
            { "470B1818", "MP_SEMAPHORELOCKED 					    " },
            { "470B1819", "MP_CONTINUE_RELEASEWAIT 			        " },
            { "4B0B1820", "MP_NOTTABLENAME 					        " },
            { "470B1821", "MP_ILLTABLETYPE 					        " },
            { "470B1822", "MP_TIMEOUTVALUEOVER 				        " },
            { "470B19FF", "MP_OTHER_ERR 						    " },
        };
    }
}