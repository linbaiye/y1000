using System;
using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class DragInventorySlotMessage : EventArgs, IClientEvent
{
    public DragInventorySlotMessage(int slot)
    {
        Slot = slot;
    }

    public int Slot { get; }
    
    public ClientPacket ToPacket()
    {
        throw new NotImplementedException();
    }
}