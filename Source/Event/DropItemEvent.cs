using System;
using y1000.Source.Item;

namespace y1000.Source.Event;

public class DropItemEvent : LocalEvent
{
    public DropItemEvent(ICharacterItem item, int slot)
    {
        Item = item;
        Slot = slot;
    }

    public int Slot { get; }
    
    public ICharacterItem Item { get; }
    
}