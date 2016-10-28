using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Common.ETimeType;

namespace LWDicer.Layers
{
    public class MTickTimer
    {
        Stopwatch Timer;
        bool bIsTimerStarted;

        public MTickTimer()
        {
            Timer = new Stopwatch();
        }
        public int StartTimer()
        {
            Timer.Reset();
            Timer.Start();
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

        public double GetElapsedTime(ETimeType type = SECOND)
        {
            double gap = Timer.ElapsedTicks;

            switch(type)
            {
                case NANOSECOND:
                    break;

                case MICROSECOND:
                    gap /= 10.0;
                    break;

                case MILLISECOND:
                    gap = Timer.ElapsedMilliseconds;
                    break;

                case SECOND:
                    gap = Timer.ElapsedMilliseconds / (1000.0);
                    break;

                case MINUTE:
                    gap = Timer.ElapsedMilliseconds / (1000.0 * 60);
                    break;

                case HOUR:
                    gap = Timer.ElapsedMilliseconds / (1000.0 * 60 * 60);
                    break;
            }
            return gap;
        }

        public bool LessThan(double CompareTime, ETimeType type = SECOND)
        {
            double gap = GetElapsedTime(type);
            if (gap < CompareTime) return true;
            else return false;
        }

        public bool MoreThan(double CompareTime, ETimeType type = SECOND)
        {
            double gap = GetElapsedTime(type);
            if (gap > CompareTime) return true;
            else return false;
        }

        public override string ToString()
        {
            return $"{GetElapsedTime(HOUR)}:{GetElapsedTime(MINUTE)%60:00}:{GetElapsedTime(SECOND)%60:00}.{GetElapsedTime(MILLISECOND)%1000:000}";
        }
    }
}
