using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.networking.message
{
    public class MoveStateMessage : IUpdateStateMessage
    {
        public long Sequence => throw new NotImplementedException();

        public CreatureState ToState => throw new NotImplementedException();

        public Point Cooridnate => throw new NotImplementedException();

        public Direction Direction => throw new NotImplementedException();

        public int Id => throw new NotImplementedException();

        public long Timestamp => throw new NotImplementedException();
    }
}