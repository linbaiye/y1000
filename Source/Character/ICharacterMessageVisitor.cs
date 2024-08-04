using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character;

public interface ICharacterMessageVisitor
{
    void Visit(SwapInventorySlotMessage message);

    void Visit(DropItemMessage message);

    void Visit(UpdateInventorySlotMessage message);
    
    void Visit(CharacterAttributeMessage message);
    
    void Visit(GainExpMessage message);

    void Visit(PlayerLearnKungFuMessage message);
    
    void Visit(KungFuOrItemAttributeMessage message);

    void Visit(PlayerAttributeMessage message);
    
    void Visit(TeleportMessage message);
    
    void Visit(UpdateKungFuSlotMessage message);
}