﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Layers.DEF_Thread;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;

namespace LWDicer.Layers
{
    /// <summary>
    /// 해당 Layer class에서 공통된 특성을 나중에 따로 묶을수 있도록 define
    /// Abstract class 이기 때문에 refComp 혹은  data class는 define 하지 않는다.
    /// </summary>
    public abstract class MMechanicalLayer : MObject
    {
        public EAutoManual AutoManualMode { get; private set; } = EAutoManual.MANUAL; // AUTO, MANUAL
        public EAutoRunMode AutoRunMode { get; private set; } = EAutoRunMode.NORMAL_RUN; // NORMAL_RUN, PASS_RUN, DRY_RUN, REPAIR_RUN

        public MMechanicalLayer(CObjectInfo objInfo) : base(objInfo)
        {
        }

        public void SetOperationMode(EAutoRunMode mode)
        {
            if (AutoRunMode != mode)
                AutoRunMode = mode;
        }

        public void SetAutoManual(EAutoManual mode)
        {
            if (AutoManualMode != mode)
                AutoManualMode = mode;
        }
    }
}
