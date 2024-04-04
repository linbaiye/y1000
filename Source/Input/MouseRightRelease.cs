using Code.Networking.Gen;
using Godot;

namespace y1000.Source.Input
{
    public class MouseRightRelease : AbstractInput
    {
        public MouseRightRelease(long s) : base(s)
        {
        }

        public override InputType Type => InputType.MOUSE_RIGHT_RELEASE;

        public override InputPacket ToPacket()
        {
            return new InputPacket()
            {
                Sequence = Sequence,
                Type = (int)Type,
            };
        }

        public override string ToString()
        {
            return "Type: RightRelease," + ", Seq: " + Sequence;
        }
    }
}