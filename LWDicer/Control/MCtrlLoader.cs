using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CCtrlLoaderRefComp
    {
        //public CCtrlLoaderRefComp()
        //{
        //}
        public override string ToString()
        {
            return $"CCtrlLoaderRefComp : ";
        }
    }

    public class CCtrlLoaderData
    {
    }

    public class MCtrlLoader : MCtrlLayer
    {
        private CCtrlLoaderRefComp m_RefComp;
        private CCtrlLoaderData m_Data;

        public MCtrlLoader(CObjectInfo objInfo,
            CCtrlLoaderRefComp refComp, CCtrlLoaderData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        #region Common : Manage Data, Position, Use Flag and Initialize
        public int SetData(CCtrlLoaderData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            
            return SUCCESS;
        }

        public int GetData(out CCtrlLoaderData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }
        #endregion

        #region Cylinder, Vacuum, Detect Object
        public int IsPanelDetected(out bool bState)
        {
            bState = false;
            return SUCCESS;
        }
        #endregion

        public int MoveToWaitPos(bool bObsolite)
        {
            return SUCCESS;
        }

        public int MoveToLoadPos()
        {
            return SUCCESS;
        }
    }
}
