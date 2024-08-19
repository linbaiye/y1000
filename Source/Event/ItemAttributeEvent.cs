using y1000.Source.Item;

namespace y1000.Source.Event;

public class ItemAttributeEvent : IUiEvent
{
    public ItemAttributeEvent(IItem item, string description, int slot)
    {
        Item = item;
        Description = description;
        Slot = slot;
    }
    
    public int Slot { get; }

    public IItem Item { get; }
    public string Description { get; }
    
}