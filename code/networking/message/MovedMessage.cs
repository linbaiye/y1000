using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.networking.message
{
    public class MovedMessage : IUpdateCharacterStateMessage
    {
        public long Sequence => throw new NotImplementedException();

        public CreatureState ToState => throw new NotImplementedException();

        public long Id => throw new NotImplementedException();

        public long Timestamp => throw new NotImplementedException();
    }
}