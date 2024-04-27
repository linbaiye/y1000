using System.Numerics;
using Godot;
using y1000.code;
using y1000.Source.Creature;

namespace y1000.Source.Input
{
    public class MouseRightClick : AbstractRightClickInput
    {
        public MouseRightClick(long s, Direction d) : base(s, d)
        {
        }

        public override InputType Type => InputType.MOUSE_RIGHT_CLICK;
    }
}