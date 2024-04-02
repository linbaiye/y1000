using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;

namespace y1000.code.character.state.input
{
    public class RightMousePressedMotion : AbstractRightClickInput
    {

        public RightMousePressedMotion(long s, Direction d) : base(s, d)
        {
        }
        public override InputType Type => InputType.MOUSE_RIGHT_MOTION;

    }
}