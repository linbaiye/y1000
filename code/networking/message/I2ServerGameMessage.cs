using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using y1000.code.networking.message;

namespace y1000.code.networking
{
    public interface I2ServerGameMessage : IGameMessage
    {
        Packet ToPacket();
    }
}