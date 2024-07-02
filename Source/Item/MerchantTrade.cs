using System.Collections.Generic;
using System.Linq;
using y1000.Source.Creature.Monster;

namespace y1000.Source.Item;

public class MerchantTrade
{
    public class InventoryItem
    {
        public InventoryItem(int slotId, ICharacterItem item)
        {
            Slot = slotId;
            Item = item;
        }

        public int Slot { get; }
        public ICharacterItem Item { get; }
        public string Name => Item.ItemName;
        public long Number => Item is CharacterStackItem stackItem ? stackItem.Number : 1;
    }

    private readonly List<InventoryItem> _playerBuying = new ();
    
    private InventoryItem? _money;

    public InventoryItem? Money => _money;

    public List<InventoryItem> Items => _playerBuying;
    
    public void AddPlayerBuying(ICharacterItem item, int slot,
        CharacterStackItem money, int moneySlot)
    {
        if (item is not CharacterStackItem addStackItem)
        {
            _playerBuying.Add(new InventoryItem(slot, item));
        }
        else
        {
            bool added = false;
            foreach (var loseItem in _playerBuying)
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
                _playerBuying.Add(new InventoryItem(slot, item));
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

    public bool IsEmpty => _playerBuying.Count == 0;

    public bool Contains(string name)
    {
        return _playerBuying.Any(i => i.Item.ItemName.Equals(name));
    }
}