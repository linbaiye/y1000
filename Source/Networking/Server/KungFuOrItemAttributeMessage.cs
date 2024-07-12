using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.Event;

namespace y1000.Source.Networking.Server;

public class KungFuOrItemAttributeMessage : ICharacterMessage, IUiEvent
{
    public KungFuOrItemAttributeMessage(RightClickType type, int slotId, string description, int page = 0)
    {
        Type = type;
        SlotId = slotId;
        Description = description;
        Page = page;
    }

    public RightClickType Type { get; }
    public int SlotId { get; }
    public int Page { get; }
    
    public string Description { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static KungFuOrItemAttributeMessage FromPacket(ItemAttributePacket packet)
    {
        return new KungFuOrItemAttributeMessage((RightClickType)packet.Type, packet.SlotId, packet.Text, packet.Page);
    }
}