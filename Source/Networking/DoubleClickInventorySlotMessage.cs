using Source.Networking.Protobuf;
using y1000.Source.Character.Event;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class DoubleClickInventorySlotMessage :  AbstractInventoryEvent,  IClientEvent
{
    public DoubleClickInventorySlotMessage(int slot) : base(slot)
    {
    }

    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            DoubleClickInventorySlotPacket = new DoubleClickInventorySlotPacket()
            {
                Slot = Slot,
            }
        };
    }
}