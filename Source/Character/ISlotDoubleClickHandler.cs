using y1000.Source.Item;

namespace y1000.Source.Character;

public interface ISlotDoubleClickHandler
{
    bool HandleInventorySlotDoubleClick(CharacterInventory inventory, int slot);
}