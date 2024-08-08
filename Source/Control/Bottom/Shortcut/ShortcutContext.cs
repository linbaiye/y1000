using Godot;

namespace y1000.Source.Control.Bottom.Shortcut;

public class ShortcutContext
{
    public ShortcutContext(int slot, Component receiver, int page, Key key)
    {
        Slot = slot;
        Receiver = receiver;
        Page = page;
        Key = key;
    }
    
    public int Page { get; }

    public int Slot { get; }
    
    public Key Key { get; }
    
    public Component Receiver { get; }

    public static ShortcutContext OfKungFu(int page, int slot, Key key)
    {
        return new ShortcutContext(slot, Component.KUNGFU_BOOK, page, key);
    }

    public static ShortcutContext OfInventory(int slot, Key key)
    {
        return new ShortcutContext(slot, Component.INVENTORY, 0, key);
    }

    public override string ToString()
    {
        return $"{nameof(Page)}: {Page}, {nameof(Slot)}: {Slot}, {nameof(Key)}: {Key}, {nameof(Receiver)}: {Receiver}";
    }

    public enum Component
    {
        INVENTORY,
        KUNGFU_BOOK,
    }
}