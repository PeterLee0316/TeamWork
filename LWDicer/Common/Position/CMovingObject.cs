using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using static LWDicer.Layers.DEF_Common;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Motion;

namespace LWDicer.Layers
{
    /// <summary>
    /// 하나의 MovingObject 단위가 가지고 있는 좌표 세트. 
    /// </summary>
    public class CPositionSet
    {
        public int Length { get; private set; }
        public CPos_XYTZ[] Pos;

        public CPositionSet(int Length)
        {
            this.Length = Length;
            Pos = new CPos_XYTZ[Length];
            Init();
        }

        public void Init()
        {
            for (int i = 0; i < Pos.Length; i++)
            {
                Pos[i] = new CPos_XYTZ();
            }
        }
    }

    /// <summary>
    /// 실제로 쓰이지는 않지만, MovingObject 들의 Position 기본 정보 처리를 위해 정의함
    /// </summary>
    public enum EPosition
    {
        NONE = -1,
        WAIT,
        LOAD,
        UNLOAD,
    }

    /// <summary>
    /// MultiAxes 와 1:1로 맵핑되어 티칭 좌표등을 종합적으로 관리
    /// </summary>
    public class CMovingObject
    {
        public int PosLength;           // 좌표의 갯수
        public CPositionSet Pos_Fixed;       // 고정좌표
        public CPositionSet Pos_Model;       // 모델 크기에 따라 자동으로 변경되는 좌표 
        public CPositionSet Pos_Offset;      // 모델 Offset 좌표

        public int PosInfo;             // 현재 좌표값과 일치하는 Position Index
        public bool IsMarkAligned;      // 얼라인되었는지의 여부
        public CPos_XYTZ AlignOffset;   // 얼라인 결과값

        public CMovingObject(int PosLength)
        {
            Debug.Assert(0 <= PosLength);

            this.PosLength = PosLength;
            Pos_Fixed = new CPositionSet(PosLength);
            Pos_Model = new CPositionSet(PosLength);
            Pos_Offset = new CPositionSet(PosLength);

            PosInfo = (int)EPosition.NONE;
            AlignOffset = new CPos_XYTZ();
        }

        public void InitAll()
        {
            Pos_Fixed.Init();
            Pos_Model.Init();
            Pos_Offset.Init();
            InitAlignOffset();
        }

        public int SetPositionSet(CPositionSet Pos_Fixed, CPositionSet Pos_Model, CPositionSet Pos_Offset)
        {
            this.Pos_Fixed = ObjectExtensions.Copy(Pos_Fixed);
            this.Pos_Model = ObjectExtensions.Copy(Pos_Model);
            this.Pos_Offset = ObjectExtensions.Copy(Pos_Offset);
            return SUCCESS;
        }

        public int GetPositionSet(out CPositionSet Pos_Fixed, out CPositionSet Pos_Model, out CPositionSet Pos_Offset)
        {
            Pos_Fixed = ObjectExtensions.Copy(this.Pos_Fixed);
            Pos_Model = ObjectExtensions.Copy(this.Pos_Model);
            Pos_Offset = ObjectExtensions.Copy(this.Pos_Offset);
            return SUCCESS;
        }

        public void InitAlignOffset()
        {
            IsMarkAligned = false;
            AlignOffset.Init();
        }

        public double GetPosition(int index, int direction, out double dFixed, out double dModel, out double dOffset, out double dAlign)
        {
            if (index < 0 || index >= PosLength) index = 0; // for avoiding error occured
            if (direction < DEF_X || direction >= DEF_XYTZ) direction = DEF_X; // for avoiding error occured

            dFixed  = Pos_Fixed.Pos[index].GetAt(direction);
            dModel  = Pos_Model.Pos[index].GetAt(direction);
            dOffset = Pos_Offset.Pos[index].GetAt(direction);
            dAlign  = (IsMarkAligned == true) ? AlignOffset.GetAt(direction) : 0.0;

            double dTarget = dFixed + dModel + dOffset + dAlign;
            return dTarget;
        }

        public CPos_XYTZ GetTargetPos(int index)
        {
            if (index < 0 || index >= PosLength) index = 0; // for avoiding error occured
            CPos_XYTZ target = Pos_Fixed.Pos[index] + Pos_Model.Pos[index] + Pos_Offset.Pos[index];
            // index가 Loading 위치가 아니고, 얼라인이 되어있다면 얼라인 보정값 적용
            if(index != (int)EPosition.LOAD && IsMarkAligned == true)
            {
                target = target + AlignOffset;
            }
            return target;
        }

        public CPos_XYTZ GetTargetPos(int index, bool WithAlign)
        {
            // Debug.Assert((int)EPosition.LOAD <= index && index < PosLength);
            CPos_XYTZ target = Pos_Fixed.Pos[index] + Pos_Model.Pos[index] + Pos_Offset.Pos[index];
            // index가 Loading 위치가 아니고, 얼라인이 되어있다면 얼라인 보정값 적용
            if (WithAlign)
            {
                target = target + AlignOffset;
            }
            return target;
        }

        public void SetAlignOffset(CPos_XYTZ offset)
        {
            IsMarkAligned = true;
            AlignOffset = ObjectExtensions.Copy(offset);
        }

        public void GetAlignOffset(out CPos_XYTZ offset)
        {
            offset = ObjectExtensions.Copy(AlignOffset);
        }
    }
}
