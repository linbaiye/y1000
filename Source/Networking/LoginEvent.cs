using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class LoginEvent : IClientEvent
{
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            LoginPacket = new PlayerLoginPacket()
            {
                Token = "test"
            }
        };
    }
}