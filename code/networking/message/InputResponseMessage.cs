using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.networking.message
{
    public class InputResponseMessage
    {

        private readonly long _sequence;

        private readonly AbstractPositionMessage _positionMessage;

        public InputResponseMessage(long sequence, AbstractPositionMessage positionMessage)
        {
            _sequence = sequence;
            _positionMessage = positionMessage;
        }

        public long Sequence => _sequence;

        public AbstractPositionMessage PositionMessage => _positionMessage;
    }
}