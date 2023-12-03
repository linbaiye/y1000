using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.networking.message;

namespace y1000.code.networking
{
    public interface IUpdateStateMessage : IGameMessage
    {
       State ToState { get; }
    }
}