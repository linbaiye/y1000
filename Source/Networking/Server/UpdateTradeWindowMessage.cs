using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class UpdateTradeWindowMessage : IServerMessage
{
    public UpdateTradeWindowMessage(UpdateType type, string name, long number, int slot, bool self)
    {
        Type = type;
        Name = name;
        Number = number;
        Slot = slot;
        Self = self;
    }

    public enum UpdateType 
    {
        CLOSE_WINDOW = 1,
        ADD_ITEM = 2,
        REMOVE_ITEM = 3,
    }
    
    public string Name { get; }
    public long Number { get; }
    public int Slot { get; }
    public bool Self { get; }
    public UpdateType Type { get; }
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
    public static UpdateTradeWindowMessage FromPacket(UpdateTradeWindowPacket packet)
    {
        var name = packet.HasName ? packet.Name : "";
        return new UpdateTradeWindowMessage((UpdateType)packet.Type, name, packet.HasNumber ? packet.Number : 0, packet.HasSlot ? packet.Slot : 0, packet.HasSelf && packet.Self);
    }
}