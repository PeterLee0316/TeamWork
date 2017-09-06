using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static Core.Layers.DEF_Common;
using static Core.Layers.DEF_Error;
using static Core.Layers.DEF_IO;
using static Core.Layers.DEF_Cylinder;
using static Core.Layers.DEF_Cylinder.ECylinderTime;
using static Core.Layers.DEF_Cylinder.ESolenoidType;

namespace Core.Layers
{
    public class MCylinder : MObject, ICylinder
    {
        IIO m_IO;
        CCylinderData m_Data;

        public MCylinder(CObjectInfo objInfo, IIO pIO, CCylinderData data) 
            : base(objInfo)
        {
            m_IO = pIO;

            SetData(data);
        }
        
        ~MCylinder()
        {
        }

        public int SetCylinderTime(CCylinderTime time)
        {
            m_Data.Time = time;
            return SUCCESS;
        }

        public int GetCylinderTime(out CCylinderTime time)
        {
            time = ObjectExtensions.Copy(m_Data.Time);
            return SUCCESS;
        }

        public int IsUp(out bool bStatus)
        {
            int iResult = SUCCESS;
            bool bTemp1, bTemp2;

            bStatus = false;

            for (int i = 0; i < DEF_MAX_CYLINDER_SENSOR; i++)   // 모든 실린더에 대해 체크
            {
                bTemp1 = bTemp2 = true;

                // UP Sensor가 On인지 확인
                if (m_Data.UpSensor[i] >= INPUT_ORIGIN)  // 센서가 Null이 아닐 경우만
                {
                    iResult = m_IO.IsOn(m_Data.UpSensor[i], out bTemp1);
                    if (iResult != SUCCESS) return iResult;
                }
                // Down Sensor가 Off인지 확인
                if (m_Data.DownSensor[i] >= INPUT_ORIGIN) // 센서가 Null이 아닐 경우만
                {
                    iResult = m_IO.IsOff(m_Data.DownSensor[i], out bTemp2);
                    if (iResult != SUCCESS) return iResult;
                }
                // 2개중 하나라도 True가 아니면
                if (!bTemp1 || !bTemp2)
                {
                    bStatus = false;
                    return SUCCESS;
                }
            }

            // 모든 Sensor에대해 확인이 완료 되었다.
            // 아니면 중간에서 false 리턴 했을 테니까 !!!
            bStatus = true;
            return SUCCESS;
        }

        public int IsDown(out bool bStatus)
        {
            int iResult = SUCCESS;
            bool bTemp1, bTemp2;
            bStatus = false;

            for (int i = 0; i < DEF_MAX_CYLINDER_SENSOR; i++)   // 모든 실린더에 대해 체크
            {
                bTemp1 = bTemp2 = true;

                // UP Sensor가 Off인지 확인
                if (m_Data.UpSensor[i] >= INPUT_ORIGIN)  // 센서가 Null이 아닐 경우만
                {
                    iResult = m_IO.IsOff(m_Data.UpSensor[i], out bTemp1);
                    if (iResult != SUCCESS) return iResult;
                }
                // Down Sensor가 On인지 확인
                if (m_Data.DownSensor[i] >= INPUT_ORIGIN) // 센서가 Null이 아닐 경우만
                {
                    iResult = m_IO.IsOn(m_Data.DownSensor[i], out bTemp2);
                    if (iResult != SUCCESS) return iResult;
                }
                // 2개중 하나라도 True가 아니면
                if (!bTemp1 || !bTemp2)
                {
                    bStatus = false;
                    return SUCCESS;
                }
            }

            // 모든 Sensor에대해 확인이 완료 되었다.
            // 아니면 중간에서 false 리턴 했을 테니까 !!!
            bStatus = true;
            return SUCCESS;
        }

        public int IsMiddle(out bool bStatus)
        {
            Assert(m_Data.SolenoidType == DOUBLE_3WAY_SOLENOID);

            int iResult = SUCCESS;
            bool bTemp1;
            bStatus = false;

            for (int i = 0; i < DEF_MAX_CYLINDER_SENSOR; i++)   // 모든 실린더에 대해 체크
            {
                bTemp1 = true;

                // Middle Sensor가 On 인지 확인
                if (m_Data.MiddleSensor[i] >= INPUT_ORIGIN)  // 센서가 Null이 아닐 경우만
                {
                    iResult = m_IO.IsOn(m_Data.MiddleSensor[i], out bTemp1);
                    if (iResult != SUCCESS) return iResult;
                }

                if (!bTemp1)
                {
                    bStatus = false;
                    return SUCCESS;
                }
            }

            // 모든 Sensor에대해 확인이 완료 되었다.
            // 아니면 중간에서 false 리턴 했을 테니까 !!!
            bStatus = true;
            return SUCCESS;
        }


        public int IsLeft(out bool bStatus)
        {
            return IsUp(out bStatus);
        }

        public int IsRight(out bool bStatus)
        {
            return IsDown(out bStatus);
        }

        public int IsFront(out bool bStatus)
        {
            return IsUp(out bStatus);
        }

        public int IsBack(out bool bStatus)
        {
            return IsDown(out bStatus);
        }

        public int IsUpstr(out bool bStatus)
        {
            return IsUp(out bStatus);
        }

        public int IsDownstr(out bool bStatus)
        {
            return IsDown(out bStatus);
        }

        public int IsCW(out bool bStatus)
        {
            return IsUp(out bStatus);
        }

        public int IsCCW(out bool bStatus)
        {
            return IsDown(out bStatus);
        }

        public int IsOpen(out bool bStatus)
        {
            return IsUp(out bStatus);
        }

        public int IsClose(out bool bStatus)
        {
            return IsDown(out bStatus);
        }

        public int Up(bool skip_sensor = false)
        {
            switch (m_Data.SolenoidType)
            {
                case SINGLE_SOLENOID:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    break;
                case REVERSE_SINGLE_SOLENOID:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    break;
                case DOUBLE_SOLENOID:
                case DOUBLE_3WAY_SOLENOID:
                case DOUBLE_SOLENOID_VARIOUS_VELOCITY:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                default:
                    break;
            }

            // 센서를 체크하지 않으면 바로 리턴 한다.
            if (skip_sensor)
            {
                Sleep(WhileSleepTime);
                return SUCCESS;
            }

            return Wait4UpComplete();
        }

        public int Down(bool skip_sensor = false)
        {
            switch (m_Data.SolenoidType)
            {
                case SINGLE_SOLENOID:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    break;
                case REVERSE_SINGLE_SOLENOID:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    break;
                case DOUBLE_SOLENOID:
                case DOUBLE_3WAY_SOLENOID:
                case DOUBLE_SOLENOID_VARIOUS_VELOCITY:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOn(m_Data.Solenoid[1]);
                    break;
                default:
                    break;
            }

            // 센서를 체크하지 않으면 바로 리턴 한다.
            if (skip_sensor)
            {
                Sleep(WhileSleepTime);
                return SUCCESS;
            }

            return Wait4DownComplete();
        }

        public int Middle(bool skip_sensor = false)    //중간정지..3way sv만허용...
        {
            Assert(m_Data.SolenoidType == DOUBLE_3WAY_SOLENOID);

            int iResult = SUCCESS;   // Integer Return 
            bool bVal;  // Boolean Status
            bool bDir;  // 중간 정지 이동 방향

            //Down Check
            if ((iResult = IsDown(out bVal)) != SUCCESS)
            {
                return iResult;    // IO Device Fail
            }

            if (bVal)
            {
                bDir = true;
                Up();       // Down 상태이면 Up() 
            }
            else
            {   // Down 상태가 아니다.

                //Up Check
                if ((iResult = IsUp(out bVal)) != SUCCESS)   // IO Device Fail
                {
                    return iResult;    // IO Device Fail
                }
                if (bVal)
                {
                    bDir = false;
                    Down();     // Up 상태이면 Down() 
                }
                else    // Down 상태도 아니고 Up상태도 아니다.
                {
                    return GenerateErrorCode(ERR_CYLINDER_INVALID_POS);
                }
            }

            // 센서를 체크하지 않으면 바로 리턴 한다.
            if (skip_sensor)
            {
                Sleep(WhileSleepTime);
                return SUCCESS;
            }

            return Wait4MiddleComplete(bDir);
        }

        //--------------- Left / Right -------------

        public int Left(bool skip_sensor = false)
        {
            return Up(skip_sensor);
        }

        public int Right(bool skip_sensor = false)
        {
            return Down(skip_sensor);
        }

        //--------------- Front / Back -------------

        public int Front(bool skip_sensor = false)
        {
            return Up(skip_sensor);
        }

        public int Back(bool skip_sensor = false)
        {
            return Down(skip_sensor);
        }

        //--------------- Downstr / Upstr -------------

        public int Upstr(bool skip_sensor = false)
        {
            return Up(skip_sensor);
        }

        public int Downstr(bool skip_sensor = false)
        {
            return Down(skip_sensor);
        }

        //--------------- CW / CCW -------------

        public int CW(bool skip_sensor = false)
        {
            return Up(skip_sensor);
        }

        public int CCW(bool skip_sensor = false)
        {
            return Down(skip_sensor);
        }

        //--------------- Open / Close -------------

        public int Open(bool skip_sensor = false)
        {
            return Up(skip_sensor);
        }

        public int Close(bool skip_sensor = false)
        {
            return Down(skip_sensor);
        }


        /*----------- 실린더 이동 시작  -----------------------*/
        /**
        * Cylinder를 Up으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartUp()
        {
            return Up(true);
        }

        /**
        * Cylinder를 Down으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartDown()
        {
            return Down(true);
        }

        /**
        * Cylinder를 Left로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartLeft()
        {
            return Up(true);
        }

        /**
        * Cylinder를 Right로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartRight()
        {
            return Down(true);
        }

        /**
        * Cylinder를 Front로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartFront()
        {
            return Up(true);
        }

        /**
        * Cylinder를 Back으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartBack()
        {
            return Down(true);
        }

        /**
        * Cylinder를 Downstr으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartDownstr()
        {
            return Down(true);
        }

        /**
        * Cylinder를 Upstr으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartUpstr()
        {
            return Up(true);
        }

        /**
        * Cylinder를 CW로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartCW()
        {
            return Up(true);
        }

        /**
        * Cylinder를 CCW으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartCCW()
        {
            return Down(true);
        }

        /**
        * Cylinder를 Open으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartOpen()
        {
            return Up(true);
        }

        /**
        * Cylinder를 Close으로 이동을 시작한다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartClose()
        {
            return Down(true);
        }

        /**
        * Cylinder를 중간 위치에서 정지 시킨다.
        * @return 0 = Success, 그 외 = Error
*/
        public int StartMiddle()    //중간정지..3way sv만허용...
        {
            return Middle(true);
        }


        /*----------- 실린더 동작완료때까지 Sleep   -----------------------*/

        /**
        * Cylinder가 Up 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4UpComplete()
        {
#if SIMULATION_TEST
            Sleep(SimulationSleepTime);
            return SUCCESS;
#endif
            int iResult = SUCCESS;   // Integer Return 
            bool bVal;  // Boolean Status

            m_waitTimer.StartTimer();

            if (m_Data.UpSensor[0] >= INPUT_ORIGIN)  // 센서가 지정되어 있음
            {
                while (true)
                {
                    // IO 동작이 완료 되었는지 체크한다.
                    iResult = IsUp(out bVal);

                    // IO Driver Check
                    if (iResult != SUCCESS)   // IO Driver Fail
                    {
                        return iResult;
                    }
                    else
                    {
                        if (bVal) break; // IO 동작이 완료 되었음
                    }

                    // Cylinder Timeout 을 체크함
                    if (m_waitTimer.MoreThan(m_Data.Time.MovingTime, ETimeType.SECOND))
                    {
                        switch (m_Data.SolenoidType)
                        {
                            case SINGLE_SOLENOID:
                            case REVERSE_SINGLE_SOLENOID:
                                break;
                            case DOUBLE_SOLENOID:
                            case DOUBLE_3WAY_SOLENOID:
                                m_IO.OutputOff(m_Data.Solenoid[0]);
                                break;
                            case DOUBLE_SOLENOID_VARIOUS_VELOCITY:
                                m_IO.OutputOff(m_Data.Solenoid[0]);
                                m_IO.OutputOff(m_Data.AccSolenoid[0]);
                                break;
                            default:
                                break;
                        }
                        return GenerateErrorCode(ERR_CYLINDER_TIMEOUT);
                    }

                    // Decel Sol On
                    if (m_Data.SolenoidType == DOUBLE_SOLENOID_VARIOUS_VELOCITY)
                    {
                        iResult = m_IO.IsOn(m_Data.AccSensor[0], out bVal);
                        if (iResult != SUCCESS) return iResult;  // IO Driver Fail

                        if (bVal) // 감속 센서를 체크 했으면
                            m_IO.OutputOn(m_Data.AccSolenoid[0]);
                    }
                    Sleep(WhileSleepTime);
                }
            }
            // Sensor가 없을때는 Delay 처리
            else
                Sleep(m_Data.Time.NoSenMovingTime * 1000);

            // MCylinder 동작완료를 위한 Delay
            Sleep(m_Data.Time.SettlingTime1 * 1000);

            // 단동식이 아닐 경우 Solenoid Off
            //    if(m_Data.SolenoidType != SINGLE_SOLENOID && m_Data.SolenoidType != REVERSE_SINGLE_SOLENOID )
            //		m_IO.OutputOff(m_Data.Solenoid[0]);

            // 감속 Solenoid Off
            if (m_Data.SolenoidType == DOUBLE_SOLENOID_VARIOUS_VELOCITY)
                m_IO.OutputOff(m_Data.AccSolenoid[0]);

            return SUCCESS;
        }

        /**
        * Cylinder가 Down 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4DownComplete()
        {
#if SIMULATION_TEST
            Sleep(SimulationSleepTime);
            return SUCCESS;
#endif
            int iResult = SUCCESS;   // Integer Return 
            bool bVal;  // Boolean Status

            m_waitTimer.StartTimer();

            if (m_Data.DownSensor[0] >= INPUT_ORIGIN)
            {

                while (true)
                {
                    // IO 동작이 완료 되었는지 체크한다.
                    iResult = IsDown(out bVal);


                    // IO Driver Check
                    if (iResult != SUCCESS) // IO Driver Fail
                    {
                        return iResult;
                    }
                    else
                    {
                        if (bVal) break; // IO 동작이 완료 되었음
                    }

                    if (m_waitTimer.MoreThan(m_Data.Time.MovingTime, ETimeType.SECOND))
                    {
                        switch (m_Data.SolenoidType)
                        {
                            case SINGLE_SOLENOID:
                            case REVERSE_SINGLE_SOLENOID:
                                break;
                            case DOUBLE_SOLENOID:
                            case DOUBLE_3WAY_SOLENOID:
                                m_IO.OutputOff(m_Data.Solenoid[1]);
                                break;
                            case DOUBLE_SOLENOID_VARIOUS_VELOCITY:
                                m_IO.OutputOff(m_Data.Solenoid[1]);
                                m_IO.OutputOff(m_Data.AccSolenoid[1]);
                                break;
                            default:
                                break;
                        }

                        return GenerateErrorCode(ERR_CYLINDER_TIMEOUT);
                    }
                    // Decel Sol On
                    if (m_Data.SolenoidType == DOUBLE_SOLENOID_VARIOUS_VELOCITY)
                    {

                        iResult = m_IO.IsOn(m_Data.AccSensor[1], out bVal);
                        if (iResult != SUCCESS) return iResult;    // IO Driver Fail

                        if (bVal) // 감속 센서를 체크 했으면
                            m_IO.OutputOn(m_Data.AccSolenoid[1]);
                    }
                    Sleep(WhileSleepTime);
                }
            }
            // Sensor가 없을때는 Delay 처리
            else
                Sleep(m_Data.Time.NoSenMovingTime * 1000);

            // MCylinder 동작완료를 위한 Delay
            Sleep(m_Data.Time.SettlingTime2 * 1000);

            // Sol Off
            //    if(m_Data.SolenoidType != SINGLE_SOLENOID  && m_Data.SolenoidType != REVERSE_SINGLE_SOLENOID )
            //		m_IO.OutputOff(m_Data.Solenoid[1]);

            // Decel Sol Off
            if (m_Data.SolenoidType == DOUBLE_SOLENOID_VARIOUS_VELOCITY)
                m_IO.OutputOff(m_Data.AccSolenoid[1]);

            return SUCCESS;
        }

        /**
        * Cylinder가 MiddlePoint 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4MiddleComplete(bool bDir) //중간정지..3way sv만허용...
        {
            int iResult = SUCCESS;   // Integer Return 
            bool bVal;  // Boolean Status

            m_waitTimer.StartTimer();

            Assert(m_Data.MiddleSensor[0] != 0);

            while (true)   // Middle Sensor
            {
                iResult = IsMiddle(out bVal); // Middle Sensor 체크
                if (iResult != SUCCESS) return iResult;

                if (bVal) break;    // Middle Sensor가 체크됨

                // Timeout 체크
                if (m_waitTimer.MoreThan(m_Data.Time.MovingTime))
                {
                    if (m_Data.SolenoidType != SINGLE_SOLENOID)
                    {
                        m_IO.OutputOff(m_Data.Solenoid[0]);
                        m_IO.OutputOff(m_Data.Solenoid[1]);
                    }
                    return GenerateErrorCode(ERR_CYLINDER_TIMEOUT);
                }
                Sleep(WhileSleepTime);
            }

            if (m_Data.SolenoidType != SINGLE_SOLENOID)
            {
                m_IO.OutputOff(m_Data.Solenoid[0]);
                m_IO.OutputOff(m_Data.Solenoid[1]);
            }

            // Cylinder 동작완료를 위한 Delay
            if (bDir) Sleep(m_Data.Time.SettlingTime1 * 1000);
            else Sleep(m_Data.Time.SettlingTime2 * 1000);

            return SUCCESS;
        }

        /**
        * Cylinder가 Left 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4LeftComplete()
        {
            return (Wait4UpComplete());
        }

        /**
        * Cylinder가 Right 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4RightComplete()
        {
            return (Wait4DownComplete());
        }

        /**
        * Cylinder가 Front 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4FrontComplete()
        {
            return (Wait4UpComplete());
        }

        /**
        * Cylinder가 Back 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4BackComplete()
        {
            return (Wait4DownComplete());
        }

        /**
        * Cylinder가 CW 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4CWComplete()
        {
            return (Wait4UpComplete());
        }

        /**
        * Cylinder가 CCW 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4CCWComplete()
        {
            return (Wait4DownComplete());
        }

        /**
        * Cylinder가 Open 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4OpenComplete()
        {
            return (Wait4UpComplete());
        }

        /**
        * Cylinder가 Close 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4CloseComplete()
        {
            return (Wait4DownComplete());
        }

        /**
        * Cylinder가 Downstr 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4DownstrComplete()
        {
            return (Wait4DownComplete());
        }

        /**
        * Cylinder가 Upstr 동작이 완료될때까지 기다린다.
        * @return 0 = Success, 그 외 = Error
*/
        public int Wait4UpstrComplete()
        {
            return (Wait4UpComplete());
        }

        /**
         * Solenoid을 Off한다.
         *
         * @param   bDir : true . 1동작, false . 2동작
         */
        public void OffSolenoid(bool bDir)
        {
            switch (m_Data.SolenoidType)
            {
                case SINGLE_SOLENOID:
                case REVERSE_SINGLE_SOLENOID:
                    // Single Solenoid는 Solenoid를 Off 할 수 없다.
                    break;
                case DOUBLE_SOLENOID:
                case DOUBLE_3WAY_SOLENOID:
                    if (bDir) m_IO.OutputOff(m_Data.Solenoid[0]);
                    else m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case DOUBLE_SOLENOID_VARIOUS_VELOCITY:
                    if (bDir)
                    {
                        m_IO.OutputOff(m_Data.Solenoid[0]);
                        m_IO.OutputOff(m_Data.AccSolenoid[0]);  // 감속센서 Disable
                    }
                    else
                    {
                        m_IO.OutputOff(m_Data.Solenoid[1]);
                        m_IO.OutputOff(m_Data.AccSolenoid[1]);  // 감속센서 Disable
                    }
                    break;
                default:
                    break;
            }
        }

        /**
         * 모든 Solenoid를 Off한다.
         */
        public void OffSolenoid()
        {
            switch (m_Data.SolenoidType)
            {
                case SINGLE_SOLENOID:
                case REVERSE_SINGLE_SOLENOID:
                    // Single Solenoid는 Solenoid를 Off 할 수 없다.
                    break;
                case DOUBLE_SOLENOID:
                case DOUBLE_3WAY_SOLENOID:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case DOUBLE_SOLENOID_VARIOUS_VELOCITY:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    m_IO.OutputOff(m_Data.AccSolenoid[0]);
                    m_IO.OutputOff(m_Data.AccSolenoid[1]);
                    break;
                default:
                    break;
            }
        }

        /**
         * 실린더 동작 완료를 확인한다.
         *
         * @param   bDir  : true . 1동작, false . 2동작
         * @return  true  : 동작완료 확인
                    false : 동작완료 확인 안됨
         */
        public int IsMoveComplete(bool bDir, out bool bStatus)
        {
            int usSensor, usSensorDecel, usSolDecel;
            int iResult = SUCCESS;   // Integer Return 
            bool bVal;  // Boolean Status
            bStatus = false;

            // 방향에 따라 확인해야 될 센서 지정
            if (bDir)
            {
                usSensor = m_Data.UpSensor[0];
                usSensorDecel = m_Data.AccSensor[0];
                usSolDecel = m_Data.Solenoid[0];
            }
            else
            {
                usSensor = m_Data.DownSensor[1];
                usSensorDecel = m_Data.AccSensor[1];
                usSolDecel = m_Data.AccSolenoid[1];
            }

            // Decel Sol On
            if (m_Data.SolenoidType == DOUBLE_SOLENOID_VARIOUS_VELOCITY)
            {
                if ((iResult = m_IO.IsOn(usSensorDecel, out bVal)) != SUCCESS) return iResult;
                if (bVal)
                    m_IO.OutputOn(usSolDecel);
            }

            if (usSensor != 0)  // 센서를 지정하지 않을 수도 있음
            {
                return m_IO.IsOn(usSensor, out bStatus);
            }

            return SUCCESS;
        }


        #region Common : Manage Data, Position, Use Flag and Initialize
        public int AssignComponents(IIO pIO)
        {
            Assert(pIO != null);

            m_IO = pIO;

            return SUCCESS;
        }

        public int SetData(CCylinderData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CCylinderData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }
        #endregion
    }
}
