using y1000.Source.Networking;

namespace y1000.Source.Character;

public interface ICharacterMessageVisitor
{
    void Visit(SwapInventorySlotMessage message);

    void Visit(CharacterChangeWeaponMessage message);

    void Visit(DropItemMessage message);

    void Visit(UpdateInventorySlotMessage message);
}