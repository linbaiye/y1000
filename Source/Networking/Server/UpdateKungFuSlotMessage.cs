using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.KungFu;

namespace y1000.Source.Networking.Server;

public class UpdateKungFuSlotMessage : ICharacterMessage
{
    private UpdateKungFuSlotMessage(IKungFu? kungFu, int slot)
    {
        KungFu = kungFu;
        Slot = slot;
    }

    public IKungFu? KungFu { get; }
    
    public int Slot { get; }
        
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static UpdateKungFuSlotMessage FromPacket(KungFuPacket packet)
    {
        if (string.IsNullOrEmpty(packet.Name))
        {
            return new UpdateKungFuSlotMessage(null, packet.Slot);
        }
        return new UpdateKungFuSlotMessage(PlayerLearnKungFuMessage.FromPacket(packet).KungFu, packet.Slot);
    }
}