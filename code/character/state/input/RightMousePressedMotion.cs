using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;

namespace y1000.code.character.state.input
{
    public class RightMousePressedMotion : AbstractInput
    {
        private readonly Direction _direction;

        public RightMousePressedMotion(long s, Direction d) : base(s)
        {
            _direction = d;
        }

        public Direction Direction => _direction;

        public override InputType Type => InputType.MOUSE_RIGHT_MOTION;

        public override Packet ToPacket()
        {
            return new Packet()
            {
                InputPacket = new InputPacket()
                {
                    Type = (int)Type,
                    ClickedDirection = (int)_direction,
                    Sequence = Sequence,
                }
            };
        }
    }
}