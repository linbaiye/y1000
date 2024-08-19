using Godot;
using y1000.Source.Character.Event;
using y1000.Source.Item;

namespace y1000.Source.Event;

public class DragInventorySlotEvent : AbstractInventoryEvent
{
    public DragInventorySlotEvent(int slot, ICharacterItem item) : base(slot)
    {
        Item = item;
    }
    
    public Vector2 AtPosition { get; set; }
    
    
    public ICharacterItem Item { get; }
    
}