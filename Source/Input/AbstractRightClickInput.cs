using Source.Networking.Protobuf;
using Godot;
using y1000.code;

namespace y1000.Source.Input
{
    public abstract class AbstractRightClickInput : AbstractInput
    {
        protected AbstractRightClickInput(long s, Direction clickedDirection) : base(s)
        {
            Direction = clickedDirection;
        }

        public Direction Direction { get; }

        public override InputPacket ToPacket()
        {
            return new InputPacket()
            {
                Sequence = Sequence,
                ClickedDirection = (int)Direction,
                Type = (int)Type,
            };
        }


        public override string ToString()
        {
            return "Type: " + Type + ", Seq: " + Sequence + ", Dir:" + Direction;
        }
    }
}