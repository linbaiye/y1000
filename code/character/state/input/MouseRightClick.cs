using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;

namespace y1000.code.character.state.input
{
    public class MouseRightClick : AbstractInput
    {
        private readonly Direction direction;


        public MouseRightClick(long s, Direction d) : base(s)
        {
            direction = d;
        }

        public Direction Direction => direction;

        public override InputType Type => InputType.MOUSE_RIGH_CLICK;

        public override Packet ToPacket()
        {
            return new Packet()
            {
                InputPacket = new InputPacket() 
                {
                    Sequence = Sequence,
                    ClickedDirection = (int)Direction,
                    Type = (int)Type,
                    Timestamp = Timestamp,
                }
            };
        }
    }
}