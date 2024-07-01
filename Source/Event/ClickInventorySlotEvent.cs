using y1000.Source.Character;
using y1000.Source.Character.Event;

namespace y1000.Source.Event;

public class ClickInventorySlotEvent : AbstractInventoryEvent
{
    public CharacterInventory Inventory { get; }

    public ClickInventorySlotEvent(int slot,
        CharacterInventory inventory) : base(slot)
    {
        Inventory = inventory;
    }
}