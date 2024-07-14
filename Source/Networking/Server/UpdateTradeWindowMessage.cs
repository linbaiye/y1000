using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class UpdateTradeWindowMessage : IServerMessage
{
    public UpdateTradeWindowMessage(UpdateType type, string name, long number, int slot)
    {
        Type = type;
        Name = name;
        Number = number;
        Slot = slot;
    }

    public enum UpdateType 
    {
        CLOSE = 1,
        UPDATE = 2,
    }
    
    public string Name { get; }
    public long Number { get; }
    
    public int Slot { get; }
    public UpdateType Type { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static UpdateTradeWindowMessage FromPacket(UpdateTradeWindowPacket packet)
    {
        var name = packet.HasName ? packet.Name : "";
        return new UpdateTradeWindowMessage((UpdateType)packet.Type, name, packet.HasNumber ? packet.Number : 0, packet.HasSlot ? packet.Slot : 0);
    }
}