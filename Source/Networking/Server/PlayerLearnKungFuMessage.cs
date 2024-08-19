using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.KungFu;

namespace y1000.Source.Networking.Server;

public class PlayerLearnKungFuMessage : ICharacterMessage
{
    private PlayerLearnKungFuMessage(IKungFu kungFu, int slotId)
    {
        KungFu = kungFu;
        SlotId = slotId;
    }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public int SlotId { get; }
    
    public IKungFu KungFu { get; }
    
    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static PlayerLearnKungFuMessage FromPacket(KungFuPacket kungFuPacket)
    {
        var kungFu = IKungFu.Create(kungFuPacket);
        return new PlayerLearnKungFuMessage(kungFu, kungFuPacket.Slot);
    }
}