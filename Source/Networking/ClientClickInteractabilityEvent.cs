using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientClickInteractabilityEvent  : IClientEvent
{

    private readonly ClientPacket _clientPacket;

    public ClientClickInteractabilityEvent(long id, string name) 
    {
        _clientPacket = new ClientPacket()
        {
            Interact = new ClientClickInteractabilityPacket()
            {
                Id = id,
                Name= name,
            }
        };
    }


    public ClientPacket ToPacket()
    {
        return _clientPacket;
    }
}