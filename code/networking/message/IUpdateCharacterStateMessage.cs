using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.player.state;

namespace y1000.code.networking.message
{
    public interface IUpdateCharacterStateMessage : IUpdateStateMessage
    {
       long Sequence { get; }
    }
}