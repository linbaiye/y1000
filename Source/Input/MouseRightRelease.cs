using Source.Networking.Protobuf;
using Godot;

namespace y1000.Source.Input
{
    public class MouseRightRelease : AbstractPredictableInput, IRightClickInput
    {
        public MouseRightRelease(long s) : base(s)
        {
        }

        public override InputType Type => InputType.MOUSE_RIGHT_RELEASE;

        public override string ToString()
        {
            return "Type: RightRelease," + ", Seq: " + Sequence;
        }

        public InputPacket ToRightClickPacket()
        {
            return new InputPacket()
            {
                Sequence = Sequence,
                Type = (int)Type,
            };
        }
    }
}