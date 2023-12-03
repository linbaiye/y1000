using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.networking;
using y1000.code.networking.message;
using y1000.code.player.state;

namespace y1000.code.character.state
{
    public interface IStateSnapshot
    {
        State State {get;}

        bool Match(IUpdateStateMessage message);

        
    }
}