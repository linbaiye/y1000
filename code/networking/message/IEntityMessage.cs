using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;

namespace y1000.code.networking.message
{
    public interface IEntityMessage
    {
        long Id { get; }
    }
}