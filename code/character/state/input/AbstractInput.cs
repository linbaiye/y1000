using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.character.state.input
{
    public abstract class AbstractInput : IInput
    {
        private readonly int sequence;

        protected AbstractInput(int s)
        {
            sequence = s;
        }

        public int Sequence => sequence;

        public abstract InputType Type { get; }
    }
}