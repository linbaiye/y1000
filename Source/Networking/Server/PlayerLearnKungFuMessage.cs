using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.KungFu;

namespace y1000.Source.Networking.Server;

public class PlayerLearnKungFuMessage : ICharacterMessage
{
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public int SlotId { get; }
    
    public string Name { get; }
    
    public string KungFuType { get; }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static PlayerLearnKungFuMessage FromPacket(KungFuPacket kungFuPacket)
    {
        
        
    }
}