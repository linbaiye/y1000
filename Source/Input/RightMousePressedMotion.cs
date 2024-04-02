using Godot;
using y1000.code;

namespace y1000.Source.Input
{
    public class RightMousePressedMotion : AbstractRightClickInput
    {

        public RightMousePressedMotion(long s, Direction d) : base(s, d)
        {
        }
        public override InputType Type => InputType.MOUSE_RIGHT_MOTION;

    }
}