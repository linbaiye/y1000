namespace y1000.Source.Networking.Server
{
    public class MoveEventResponse : AbstractPredictableResponse
    {

        private readonly AbstractPositionMessage _positionMessage;

        public MoveEventResponse(long sequence, AbstractPositionMessage positionMessage) : base(sequence)
        {
            _positionMessage = positionMessage;
        }

        public AbstractPositionMessage PositionMessage => _positionMessage;

        public override string ToString()
        {
            return "Seq:" + Sequence + ", Msg:" + _positionMessage;
        }

        public override void HandleBy(IServerMessageHandler handler)
        {
            handler.Handle(this);
        }
    }
}