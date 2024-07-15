using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientAddTradeItemEvent : IClientEvent
{
    public ClientAddTradeItemEvent(int slot, long number)
    {
        Slot = slot;
        Number = number;
    }

    private int Slot { get; }
    private long Number { get; }
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            AddTradeItem = new ClientAddTradeItemPacket()
            {
                ItemNumber = Number,
                InventorySlot = Slot
            }
        };
    }
}