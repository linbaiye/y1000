using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Code.Networking.Gen;

namespace y1000.code.character.state.input
{
    public class MouseRightRelease : AbstractInput
    {
        public MouseRightRelease(long s) : base(s)
        {
        }

        public override InputType Type => InputType.MOUSE_RIGHT_RELEASE;

        public override Packet ToPacket()
        {
            return new Packet()
            {
                InputPacket = new InputPacket()
                {
                    Sequence = Sequence,
                    Type = (int)Type,
                }
            };
        }

        public override string ToString()
        {
            return "Type: RightRelease," + ", Seq: " + Sequence;
        }
    }
}