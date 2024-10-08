using System.Collections.Generic;
using Google.Protobuf.Collections;
using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class InventoryItemMessage
{
    public InventoryItemMessage(string name, int slotId, long number, int color)
    {
        Name = name;
        SlotId = slotId;
        Number = number;
        Color = color;
    }
    
    public string Name { get; }
    public int SlotId { get; }
            
    public long Number { get; }
            
    public int Color { get; }
    
    public static List<InventoryItemMessage> ItemMessages(RepeatedField<InventoryItemPacket> inventoryItems)
    {
        List<InventoryItemMessage> itemMessages = new List<InventoryItemMessage>();
        foreach (var inventoryItem in inventoryItems)
        {
            itemMessages.Add(new InventoryItemMessage(inventoryItem.Name,  inventoryItem.SlotId, inventoryItem.Number, inventoryItem.Color));
        }
        return itemMessages;
    }
}