using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;

namespace LWDicer.Control
{
    public abstract class MObject
    {
        protected int ObjectID;
        protected CObjectInfo ObjInfo;
        protected MLog LogManager;

        public MObject(CObjectInfo ObjInfo)
        {
            ObjectID     = ObjInfo.ID;
            this.ObjInfo = ObjInfo;
            LogManager   = new MLog(ObjInfo.ID);
            LogManager.SetLogAttribute(ObjInfo.DebugTableName, ObjInfo.LogLevel, ObjInfo.LogDays);
            LogManager.SetDBInfo(CObjectInfo.DBInfo);

            string str = $"{this} Created OK";
            WriteLog(str);
        }

        public int GenerateErrorCode(int error, bool writeLog = true)
        {
            if (error == SUCCESS) // Success
            {
                return SUCCESS;
            }

            int errorCode = (ObjInfo.ID << 16) + (ObjInfo.ErrorBase + error);
            return errorCode;
        }

        public override string ToString()
        {
            return $"[Object] ID : {ObjInfo.ID}, Name : {ObjInfo.Name}";
        }

        public int WriteLog(string strLog, ELogType logType = ELogType.Debug, ELogWType writeType = ELogWType.D_Normal, bool ShowOutputWindow = false, int skipFrames = 2)
        {
            // for break when error occured
            if(logType == ELogType.Debug && writeType == ELogWType.D_Error)
            {
                int i = 0;
            }
            return LogManager.WriteLog(strLog, logType, writeType, ShowOutputWindow, skipFrames);
        }

        public void WriteExLog(string strLog, ELogType logType = ELogType.Debug, ELogWType writeType = ELogWType.D_Error, bool ShowOutputWindow = true, int skipFrames = 3)
        {
            LogManager.WriteLog(strLog, logType, writeType, ShowOutputWindow, skipFrames);
        }

        public void Sleep<T>(T millisec)
        {
            try
            {
                int time = Convert.ToInt32(millisec);
                Thread.Sleep(time);
            }catch (Exception ex)
            {
                WriteExLog(ex.ToString());
            }
        }

        protected void Assert(bool condition)
        {
            Debug.Assert(condition);
        }

        protected void WriteLine(object msg)
        {
            Debug.WriteLine(msg.ToString());
        }

    }
}
