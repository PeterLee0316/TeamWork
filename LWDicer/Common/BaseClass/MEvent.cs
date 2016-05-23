using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWDicer.Control
{
    /// <summary>
    /// thread간에 event message를 전송하기 위한 class. 
    /// 특별히 wParam, lParam으로 사용하진 않아도 되나, c++ 에서 사용하던 포맷을 유지함.
    /// </summary>
    public class MEvent
    {
        // Message 종류
        public int Msg { get; set; }
        // 호출자 (ex. sender thread's id)
        public int wParam { get; set; }
        // Message 값 (ex. alarm code : GenerateErrorCode 로 생성된 값)
        public int lParam { get; set; }

        private DateTime MsgTime;
        private int Index;

        private static int stCounter = 0;
        private object _Lock = new object();

        public MEvent(int Msg, int wParam, int lParam)
        {
            lock(_Lock)
            {
                this.Msg = Msg;
                this.wParam = wParam;
                this.lParam = lParam;
                MsgTime = DateTime.Now;

                if (stCounter >= Int32.MaxValue) stCounter = 0;
                Index = stCounter++;
            }
        }

        public override string ToString()
        {
            return $"[Event] Idx_{Index}, Msg : {Msg}, wParam : { wParam}, lParam : {lParam}, Created : {MsgTime.ToString("yyyy-MM-dd HH:mm:ss.ffff")}";
        }
    }
}
