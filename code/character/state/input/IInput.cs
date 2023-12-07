using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using y1000.code.networking;

namespace y1000.code.character.state
{
    public interface IInput : I2ServerGameMessage
    {
        long Sequence { get; }

        InputType Type { get; }

        long Timestamp { get; }


   
    }
}