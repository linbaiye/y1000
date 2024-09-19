using Godot;

namespace y1000.Source.Control;

public class ShortcutContext
{
    private ShortcutContext(int slot, Component receiver, int page, Key key)
    {
        Slot = slot;
        Receiver = receiver;
        Page = page;
        Key = key;
    }

    public ShortcutContext(): this(0, Component.INVENTORY, 0, Key.A)
    {
        
    }
    
    public int Page { get; set; }

    public int Slot { get; set; }
    
    public Key Key { get; set; }
    
    public Component Receiver { get; set; }

    public static ShortcutContext OfKungFu(int page, int slot, Key key)
    {
        return new ShortcutContext(slot, Component.KUNGFU_BOOK, page, key);
    }

    public static ShortcutContext OfInventory(int slot, Key key)
    {
        return new ShortcutContext(slot, Component.INVENTORY, 0, key);
    }

    public enum Component
    {
        INVENTORY,
        KUNGFU_BOOK,
    }
}