using y1000.Source.Item;

namespace y1000.Source.Control.Dialog;

public class SellingItem : TradeInputWindow.ITradeWindowItem
{
    public SellingItem(int slot, ICharacterItem item)
    {
        Slot = slot;
        Item = item;
    }

    public ICharacterItem Item { get; }
    
    public int Slot { get; }

    public string Name => Item.ItemName;
    
}