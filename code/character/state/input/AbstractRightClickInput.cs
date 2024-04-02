using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;

namespace y1000.code.character.state.input
{
    public abstract class AbstractRightClickInput : AbstractInput
    {

        private readonly Direction _clickedDirection;

        protected AbstractRightClickInput(long s, Direction clickedDirection) : base(s)
        {
            _clickedDirection = clickedDirection;
        }

        public Direction Direction => _clickedDirection;

        public override Packet ToPacket()
        {
            return new Packet()
            {
                InputPacket = new InputPacket() 
                {
                    Sequence = Sequence,
                    ClickedDirection = (int)Direction,
                    Type = (int)Type,
                }
            };
        }


        public override string ToString()
        {
            return "Type: " + Type + ", Seq: " + Sequence + ", Dir:" + Direction;
        }
    }
}