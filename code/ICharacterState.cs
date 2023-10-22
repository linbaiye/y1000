using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.cide
{
    public interface ICharacterState
    {
        State State { get; }

        Direction Direction { get; }

    }
}