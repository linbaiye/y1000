using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientTradePlayerEvent : IClientEvent
{
    public ClientTradePlayerEvent(long targetId, int slot)
    {
        TargetId = targetId;
        Slot = slot;
    }

    private long TargetId { get; }
    private int Slot { get; }
    
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            TradeRequest = new ClientTradePlayerPacket()
            {
                Slot = Slot,
                TargetId = TargetId
            }
        };
    }
}