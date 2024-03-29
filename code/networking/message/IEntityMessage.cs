using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;

namespace y1000.code.networking.message
{
    public interface IEntityMessage
    {
        long Id { get; }
    }
}