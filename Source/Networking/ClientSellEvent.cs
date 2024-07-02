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

    private List<string> Names => Trade.Items.Select(i => i.Name).ToList();
    private List<int> Numbers => Trade.Items.Select(i => i.Number).ToList();
    
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            SellItems = new ClientSellItemsPacket()
            {
                MerchantId = MerchantId,
                ItemNames = { Names },
                ItemNumbers = { Numbers },
            }
        };
    }
}