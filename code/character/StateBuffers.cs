using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character.state;

namespace y1000.code.character
{
    public class StateBuffers
    {
        private readonly List<IInput> inputs;

        private readonly List<IStateSnapshot> snapshots;


        public StateBuffers()
        {
            inputs = new List<IInput>();
            snapshots = new List<IStateSnapshot>();
        }

        public void Add(IInput input, IStateSnapshot current)
        {
            inputs.Add(input);
            snapshots.Add(current);
        }

    }
}