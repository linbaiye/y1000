using y1000.Source.Item;

namespace y1000.Source.Event;

public class ItemAttributeEvent : IUiEvent
{
    public ItemAttributeEvent(IItem item, string description)
    {
        Item = item;
        Description = description;
    }

    public IItem Item { get; }
    public string Description { get; }
    
}