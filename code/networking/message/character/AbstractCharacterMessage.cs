using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.networking.message.character
{
    public abstract class AbstractCharacterMessage : ICharacterMessage
    {

        private readonly long _sequence;

        protected AbstractCharacterMessage(long sequence)
        {
            _sequence = sequence;
        }

        public long InputSeqeunce => _sequence;
    }
}