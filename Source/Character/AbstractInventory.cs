using System;
using System.Collections.Generic;
using System.Linq;
using y1000.Source.Item;

namespace y1000.Source.Character;

public abstract class AbstractInventory
{
    protected readonly IDictionary<int, ICharacterItem> _items;
    public virtual event EventHandler<EventArgs>? InventoryChanged;
    public bool IsFull => _items.Count >= Capacity;

    public AbstractInventory(int capacity)
    {
        Capacity = capacity;
        _items = new Dictionary<int, ICharacterItem>();
    }
    
    public int Capacity { get; }

    public bool HasSpace(string name)
    {
        var item = _items.Values.FirstOrDefault(i => i.ItemName.Equals(name));
        return item is CharacterStackItem || !IsFull;
    }

    public bool HasItem(int slot)
    {
        return _items.ContainsKey(slot);
    }

    public bool HasEnough(string name, long number)
    {
        var item = _items.Values.FirstOrDefault(i => i.ItemName.Equals(name));
        return item is CharacterStackItem stackItem && stackItem.Number >= number
               || item is CharacterItem;
    }

    public bool HasEnough(int slot, string name, long number)
    {
        if (!_items.TryGetValue(slot, out var item))
        {
            return false;
        }
        if (!item.ItemName.Equals(name))
        {
            return false;
        }
        return item is CharacterStackItem stackItem && stackItem.Number >= number
               || item is CharacterItem;
    }

    public ICharacterItem? Find(int slot)
    {
        return _items.TryGetValue(slot, out var item) ? item : null;
    }

    public void Update(int slot, ICharacterItem? item)
    {
        _items.Remove(slot);
        if (item != null)
        {
            _items.TryAdd(slot, item);
        }
        Notify();
    }
    
    protected void Notify()
    {
        InventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    private int AddToEmptySlot(ICharacterItem item)
    {
        if (IsFull)
        {
            return 0;
        }
        for (var i = 1; i <= Capacity; i++)
        {
            if (_items.TryAdd(i, item))
            {
                return i;
            }
        }
        return 0;
    }

    protected int AddItem(ICharacterItem item)
    {
        if (item is CharacterStackItem stackItem)
        {
            for (int i = 1; i <= Capacity; i++)
            {
                if (_items.TryGetValue(i, out var slotItem) && slotItem.ItemName.Equals(item.ItemName) && slotItem is CharacterStackItem slotStackItem)
                {
                    slotStackItem.Number += stackItem.Number;
                    return i;
                }
            }
        }
        return AddToEmptySlot(item);
    }

    public bool PutItem(int slot, ICharacterItem item)
    {
        if (slot < 1 || slot > Capacity)
        {
            throw new ArgumentException("Invalid slot " + slot);
        }
        var ret = !IsFull && _items.TryAdd(slot, item);
        if (ret)
        {
            InventoryChanged?.Invoke(this, EventArgs.Empty);
        }
        return ret;
    }

    public void Foreach(Action<int, ICharacterItem> consumer)
    {
        foreach (var keyValuePair in _items)
        {
            consumer.Invoke(keyValuePair.Key, keyValuePair.Value);
        }
    }

    public void Swap(int slot1, int slot2)
    {
        _items.TryGetValue(slot1, out var item1);
        _items.TryGetValue(slot2, out var item2);
        if (item1 != null || item2 != null)
        {
            _items.Remove(slot1);
            _items.Remove(slot2);
            if (item1 != null)
            {
                _items.TryAdd(slot2, item1);
            }
            if (item2 != null)
            {
                _items.TryAdd(slot1, item2);
            }
            InventoryChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}