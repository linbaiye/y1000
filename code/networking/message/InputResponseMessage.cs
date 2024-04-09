using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using y1000.Source.Networking;

namespace y1000.code.networking.message
{
    public class InputResponseMessage : IServerMessage
    {

        private readonly long _sequence;

        private readonly AbstractPositionMessage _positionMessage;

        public InputResponseMessage(long sequence, AbstractPositionMessage positionMessage)
        {
            _sequence = sequence;
            _positionMessage = positionMessage;
        }

        public long Sequence => _sequence;

        public T CastMessage<T>() where T : AbstractPositionMessage
        {
            return (T)_positionMessage;
        }

        public AbstractPositionMessage PositionMessage => _positionMessage;

        public override string ToString()
        {
            return "Seq:" + _sequence + ", Msg:" + _positionMessage;
        }

        public void Accept(IServerMessageHandler handler)
        {
            handler.Handle(this);
        }
    }
}