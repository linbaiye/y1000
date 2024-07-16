using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientUpdateTradeEvent : IClientEvent
{
    private ClientUpdateTradeEvent(int inventorySlot, long number, UpdateType type, int tradeWindowSlot)
    {
        InventorySlot = inventorySlot;
        Number = number;
        Type = type;
        TradeWindowSlot = tradeWindowSlot;
    }
    
    private enum UpdateType
    {
        ADD_ITEM = 1,
        REMOVE_ITEM = 2,
        CONFIRM = 3,
        CANCEL = 4,
    }
    private int InventorySlot { get; }
    private long Number { get; }
    
    private UpdateType Type { get; }
    
    private int TradeWindowSlot { get; }
    
    
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            UpdateTrade= new ClientUpdateTradePacket()
            {
                ItemNumber= Number,
                InventorySlot = InventorySlot,
                Type = (int)Type,
                TradeWindowSlot = TradeWindowSlot,
            }
        };
    }

    public static ClientUpdateTradeEvent AddItem(int inventorySlot, long number)
    {
        return new(inventorySlot, number, UpdateType.ADD_ITEM, 0);
    }

    public static ClientUpdateTradeEvent RemoveItem(int tradeWindowSlot)
    {
        return new (0, 0,UpdateType.REMOVE_ITEM, tradeWindowSlot);
    }

    public static ClientUpdateTradeEvent Cancel()
    {
        return new (0, 0,UpdateType.CANCEL, 0);
    }
    
    public static ClientUpdateTradeEvent Confirm()
    {
        return new (0, 0,UpdateType.CONFIRM, 0);
    }
}