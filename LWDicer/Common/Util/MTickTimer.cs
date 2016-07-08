using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.MTickTimer.ETimeType;

namespace LWDicer.Control
{
    public class MTickTimer
    {
        public enum ETimeType
        {
            NANOSECOND,
            MICROSECOND,
            MILLISECOND,
            SECOND,
            MINUTE,
            HOUR,
        }

        Stopwatch Timer;
        bool bIsTimerStarted;

        public MTickTimer()
        {
            Timer = new Stopwatch();
        }
        public int StartTimer()
        {
            Timer.Restart();
            //Timer.Start();
            bIsTimerStarted = true;

            return SUCCESS;
        }

        public int StopTimer()
        {
            //if (bIsTimerStarted == false) return;
            Timer.Stop();
            bIsTimerStarted = false;
            return SUCCESS;
        }

        public long GetElapsedTime(ETimeType type = MILLISECOND)
        {
            long gap = Timer.ElapsedTicks;

            switch(type)
            {
                case NANOSECOND:
                    break;

                case MICROSECOND:
                    gap /= 10;
                    break;

                case MILLISECOND:
                    gap = Timer.ElapsedMilliseconds;
                    break;

                case SECOND:
                    gap = Timer.ElapsedMilliseconds / (1000);
                    break;

                case MINUTE:
                    gap = Timer.ElapsedMilliseconds / (1000 * 60);
                    break;

                case HOUR:
                    gap = Timer.ElapsedMilliseconds / (1000 * 60 * 60);
                    break;
            }
            return gap;
        }

        public bool LessThan(long CompareTime, ETimeType type = MILLISECOND)
        {
            long gap = GetElapsedTime(type);
            if (gap < CompareTime) return true;
            else return false;
        }

        public bool MoreThan(long CompareTime, ETimeType type = MILLISECOND)
        {
            long gap = GetElapsedTime(type);
            if (gap > CompareTime) return true;
            else return false;
        }

        public bool LessThan(double CompareTime, ETimeType type = MILLISECOND)
        {
            CompareTime = (long)CompareTime;
            long gap = GetElapsedTime(type);
            if (gap < CompareTime) return true;
            else return false;
        }

        public bool MoreThan(double CompareTime, ETimeType type = MILLISECOND)
        {
            CompareTime = (long)CompareTime;
            long gap = GetElapsedTime(type);
            if (gap > CompareTime) return true;
            else return false;
        }

        public override string ToString()
        {
            return $"{GetElapsedTime(HOUR)}:{GetElapsedTime(MINUTE)%60:00}:{GetElapsedTime(SECOND)%60:00}.{GetElapsedTime(MILLISECOND)%1000:000}";
        }
    }
}
