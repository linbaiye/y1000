using System.Collections.Generic;
using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class OpenBankMessage : IServerMessage
{
    public OpenBankMessage(int capacity, int unlocked, List<InventoryItemMessage> itemMessages)
    {
        Capacity = capacity;
        Unlocked = unlocked;
        ItemMessages = itemMessages;
    }

    public int Capacity { get; set; }
    
    public int Unlocked { get; set; }
    
    public List<InventoryItemMessage> ItemMessages { get; set; }

    public static OpenBankMessage FromPacket(OpenBankPacket packet)
    {
        return new OpenBankMessage(packet.Capacity, packet.Unlocked, InventoryItemMessage.ItemMessages(packet.Items));
    }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}