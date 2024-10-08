using System.Collections.Generic;
using System.Linq;

namespace y1000.Source.Item;

public class MerchantTrade
{
    public class InventoryItem
    {
        public InventoryItem(int slotId, IItem item)
        {
            Slot = slotId;
            Item = item;
        }

        public int Slot { get; }
        public IItem Item { get; }
        public string Name => Item.ItemName;
        public long Number => Item is CharacterStackItem stackItem ? stackItem.Number : 1;
    }

    private readonly List<InventoryItem> _items = new ();
    
    private InventoryItem? _money;

    public InventoryItem? Money => _money;

    public List<InventoryItem> Items => _items;

    public void AddItem(IItem item, int slot,
        CharacterStackItem money, int moneySlot)
    {
        if (item is not CharacterStackItem addStackItem)
        {
            _items.Add(new InventoryItem(slot, item));
        }
        else
        {
            bool added = false;
            foreach (var loseItem in _items)
            {
                if (loseItem.Item.ItemName.Equals(item.ItemName) &&
                    loseItem.Item is CharacterStackItem stackItem)
                {
                    stackItem.Number += addStackItem.Number;
                    added = true;
                }
            }
            if (!added)
            {
                _items.Add(new InventoryItem(slot, item));
            }
        }
        if (_money == null)
        {
            _money = new InventoryItem(moneySlot, money);
        }
        else
        {
            ((CharacterStackItem)_money.Item).Number += money.Number;
        }
    }

    public bool IsEmpty => _items.Count == 0;

    public bool Contains(string name)
    {
        return _items.Any(i => i.Item.ItemName.Equals(name));
    }
}