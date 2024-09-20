using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientChangeTeamEvent : IClientEvent
{
    private readonly ClientPacket _packet;

    public ClientChangeTeamEvent(int team)
    {
        _packet = new ClientPacket()
        {
            ChangeTeam = new ClientChangeTeamPacket()
            {
                TeamNumber = team
            }
        };
    }

    public ClientPacket ToPacket()
    {
        return _packet;
    }
}