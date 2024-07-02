using System.Collections.Generic;
using System.Linq;
using Source.Networking.Protobuf;
using y1000.Source.Input;
using y1000.Source.Item;

namespace y1000.Source.Networking;

public class ClientSellEvent : IClientEvent
{
    public ClientSellEvent(MerchantTrade trade, long merchantId)
    {
        Trade = trade;
        MerchantId = merchantId;
    }
    
    private long MerchantId { get; }

    private MerchantTrade Trade { get; }

    private List<InventoryItemPacket> Items => Trade.Items.Select(i => new InventoryItemPacket() {SlotId = i.Slot, Name = i.Name, Number = i.Number}).ToList();
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            SellItems = new ClientSellItemsPacket()
            {
                MerchantId = MerchantId,
                Items = { Items }
            }
        };
    }
}