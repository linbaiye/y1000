using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientStandEvent : IClientEvent
{
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            StandUp = new ClientStandUpPacket()
            {
                Padding = true
            }
        };
    }
}