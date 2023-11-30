using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.character.state.input
{
    public class MouseMoveInput : AbstractInput
    {
        private readonly Direction direction;

        public MouseMoveInput(int s, Direction d) : base(s)
        {
            direction = d;
        }

        public Direction Direction => direction;

        public override InputType Type => InputType.MOUSE_CLICK_MOVE;
    }
}