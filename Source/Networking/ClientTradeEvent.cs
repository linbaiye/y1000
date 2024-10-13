using System.Collections.Generic;
using System.Linq;
using Source.Networking.Protobuf;
using y1000.Source.Input;
using y1000.Source.Item;

namespace y1000.Source.Networking;

public class ClientTradeEvent : IClientEvent
{
    public ClientTradeEvent(MerchantTrade trade, long merchantId, bool selling = true)
    {
        Trade = trade;
        MerchantId = merchantId;
        Selling = selling;
    }
    
    public static ClientTradeEvent PlayerSell(MerchantTrade trade, long merchantId)
    {
        return new ClientTradeEvent(trade, merchantId);
    }

    public static ClientTradeEvent PlayerBuy(MerchantTrade trade, long merchantId)
    {
        return new ClientTradeEvent(trade, merchantId, false);
    }
    
    private bool Selling { get; }
    
    private long MerchantId { get; }

    private MerchantTrade Trade { get; }

    private List<InventoryItemPacket> Items => Trade.Items.Select(i => new InventoryItemPacket() {SlotId = i.Slot, Name = i.Name, Number = i.Number}).ToList();
    public ClientPacket ToPacket()
    {
        return Selling ? new ClientPacket()
            {
                SellItems = new ClientMerchantTradeItemsPacket()
                {
                    MerchantId = MerchantId,
                    Items = { Items },
                }
            }
            : new ClientPacket()
            {
                BuyItems = new ClientMerchantTradeItemsPacket()
                {
                    MerchantId = MerchantId,
                    Items = { Items },
                }
            };
    }
}