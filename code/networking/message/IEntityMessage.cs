using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;
using y1000.Source.Networking;

namespace y1000.code.networking.message
{
    public interface IEntityMessage : IServerMessage
    {
        long Id { get; }
    }
}