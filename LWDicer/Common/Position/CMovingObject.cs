using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;

namespace LWDicer.Control
{
    public class CPosition
    {
        public int Length { get; private set; }
        public CPos_XYTZ[] Pos;

        public CPosition(int Length)
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
        public CPosition FixedPos;       // 고정좌표
        public CPosition ModelPos;       // 모델 크기에 따라 자동으로 변경되는 좌표 
        public CPosition OffsetPos;      // 모델 Offset 좌표

        public int PosInfo;             // 현재 좌표값과 일치하는 Position Index
        public bool IsMarkAligned;      // 얼라인되었는지의 여부
        public CPos_XYTZ AlignOffset;   // 얼라인 결과값

        public CMovingObject(int PosLength)
        {
            Debug.Assert(0 < PosLength);

            this.PosLength = PosLength;
            FixedPos = new CPosition(PosLength);
            ModelPos = new CPosition(PosLength);
            OffsetPos = new CPosition(PosLength);

            PosInfo = (int)EPosition.NONE;
            AlignOffset = new CPos_XYTZ();
        }

        public void InitAll()
        {
            FixedPos.Init();
            ModelPos.Init();
            OffsetPos.Init();
            InitAlignOffset();
        }

        public int SetPosition(CPosition FixedPos, CPosition ModelPos, CPosition OffsetPos)
        {
            this.FixedPos = ObjectExtensions.Copy(FixedPos);
            this.ModelPos = ObjectExtensions.Copy(ModelPos);
            this.OffsetPos = ObjectExtensions.Copy(OffsetPos);
            return SUCCESS;
        }

        public int GetPosition(out CPosition FixedPos, out CPosition ModelPos, out CPosition OffsetPos)
        {
            FixedPos = ObjectExtensions.Copy(this.FixedPos);
            ModelPos = ObjectExtensions.Copy(this.ModelPos);
            OffsetPos = ObjectExtensions.Copy(this.OffsetPos);
            return SUCCESS;
        }

        public void InitAlignOffset()
        {
            IsMarkAligned = false;
            AlignOffset.Init();
        }
        
        public CPos_XYTZ GetTargetPos(int index)
        {
            Debug.Assert((int)EPosition.LOAD <= index && index < PosLength);
            CPos_XYTZ target = FixedPos.Pos[index] + ModelPos.Pos[index] + OffsetPos.Pos[index];
            // index가 Loading 위치가 아니고, 얼라인이 되어있다면 얼라인 보정값 적용
            if(index != (int)EPosition.LOAD && IsMarkAligned == true)
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
