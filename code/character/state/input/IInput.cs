using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.character.state
{
    public interface IInput
    {
        int Sequence { get; }

        InputType Type { get; }
        
    }
}