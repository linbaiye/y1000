using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class PickItemEvent : IClientEvent
{
    public PickItemEvent(int id)
    {
        Id = id;
    }

    public int Id { get; }
    
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            PickItem = new PickItemPacket()
            {
                Id = Id,
            }
        };
    }
}