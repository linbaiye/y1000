using System;
using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class DoubleClickInventorySlotMessage :  EventArgs,  IClientEvent
{
    public DoubleClickInventorySlotMessage(int slot)
    {
        Slot = slot;
    }

    private int Slot { get; }
    
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