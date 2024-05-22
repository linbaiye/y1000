using System;
using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.Input;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class SwapInventorySlotMessage : EventArgs, IServerMessage, IClientEvent, ICharacterMessage
{
    public SwapInventorySlotMessage(int slot1, int slot2)
    {
        Slot1 = slot1;
        Slot2 = slot2;
    }

    public int Slot1 { get; }
    
    public int Slot2 { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            SwapInventorySlotPacket =
                new SwapInventorySlotPacket()
                {
                    Slot1 = Slot1,
                    Slot2 = Slot2
                }
        };
    }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static SwapInventorySlotMessage FromPacket(SwapInventorySlotPacket packet)
    {
        return new SwapInventorySlotMessage(packet.Slot1, packet.Slot2);
    }
}