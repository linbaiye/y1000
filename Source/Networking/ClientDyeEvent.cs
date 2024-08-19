using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientDyeEvent : IClientEvent
{
    public ClientDyeEvent(int dyedSlotId, int dyeSlotId)
    {
        DyedSlotId = dyedSlotId;
        DyeSlotId = dyeSlotId;
    }

    private int DyedSlotId { get; }
    private int DyeSlotId { get; }
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            Dye = new ClientDyePacket()
            {
                DyedSlotId = DyedSlotId,
                DyeSlotId = DyeSlotId,
            }
        };
    }
}