using System;
using y1000.Source.Control;
using y1000.Source.Control.Bottom.Shortcut;
using y1000.Source.Item;

namespace y1000.Source.Character.Event;

public class InventoryShortcutEvent : EventArgs
{
    public InventoryShortcutEvent(ShortcutContext context, IItem item)
    {
        Context = context;
        Item = item;
    }
    
    
    public IItem Item { get; }
    
    public ShortcutContext Context { get; }
}