
using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class OpenTradeWindowMessage : IServerMessage
{
    public OpenTradeWindowMessage(int slotId, long targetId)
    {
        SlotId = slotId;
        TargetId = targetId;
    }

    public long TargetId { get; }
    
    public int SlotId { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static OpenTradeWindowMessage FromPacket(OpenTradeWindowPacket packet)
    {
        var slot = packet.HasSlot ? packet.Slot : 0;
        return new OpenTradeWindowMessage(slot, packet.AnotherPlayerId);
    }
}