using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientTriggerDynamicObjectEvent : IClientEvent
{
    public ClientTriggerDynamicObjectEvent(long id, int slot)
    {
        Id = id;
        Slot = slot;
    }

    private long Id { get; }
    private int Slot { get; }
    
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            TriggerDynamicObject = new ClientTriggerDynamicObjectPacket()
            {
                Id = Id,
                UseSlot = Slot
            }
        };
    }
}