using Source.Networking.Protobuf;
using y1000.Source.Creature;

namespace y1000.Source.Input
{
    public abstract class AbstractRightClickInput : AbstractPredictableInput, IRightClickInput
    {
        protected AbstractRightClickInput(long s, Direction clickedDirection) : base(s)
        {
            Direction = clickedDirection;
        }

        public Direction Direction { get; }

        public override string ToString()
        {
            return "Type: " + Type + ", Seq: " + Sequence + ", Dir:" + Direction;
        }

        public InputPacket ToRightClickPacket()
        {
            return new InputPacket()
            {
                Sequence = Sequence,
                ClickedDirection = (int)Direction,
                Type = (int)Type,
            };
        }
    }
}