using System;
using System.Collections.Generic;
using System.Linq;
using y1000.Source.Item;

namespace y1000.Source.Character;

public abstract class AbstractInventory
{
    protected readonly IDictionary<int, IItem> _items;
    public bool IsFull => _items.Count >= Capacity;

    public AbstractInventory(int capacity)
    {
        Capacity = capacity;
        _items = new Dictionary<int, IItem>();
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
        if (item is CharacterStackItem stackItem)
            return stackItem.Number >= number;
        return number == 1 && item != null;
    }
    
    protected bool CanPut(IItem item, int slot)
    {
        var bankItem = Get(slot);
        if (bankItem == null)
        {
            return true;
        }
        return bankItem is CharacterStackItem bankStack
               && item is CharacterStackItem stackItem
               && bankStack.ItemName.Equals(stackItem.ItemName);
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
        if (item is CharacterStackItem stackItem) {
            return stackItem.Number >= number;
        }
        return true;
    }

    public IItem? Find(int slot)
    {
        return _items.TryGetValue(slot, out var item) ? item : null;
    }
    
    public int FindSlot(string name)
    {
        foreach (var itemsKey in _items.Keys)
        {
            if (_items.TryGetValue(itemsKey, out var i) && i.ItemName.Equals(name))
            {
                return itemsKey;
            }
        }
        return 0;
    }

    public void Update(int slot, IItem? item)
    {
        _items.Remove(slot);
        if (item != null)
        {
            _items.TryAdd(slot, item);
        }
        Notify();
    }

    protected abstract void Notify();

    private int AddToEmptySlot(IItem item)
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

    protected int AddItem(IItem item)
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

    public bool PutItem(int slot, IItem item)
    {
        if (slot < 1 || slot > Capacity)
        {
            throw new ArgumentException("Invalid slot " + slot);
        }
        var ret = !IsFull && _items.TryAdd(slot, item);
        if (ret)
        {
            Notify();
        }
        return ret;
    }

    public void Foreach(Action<int, IItem> consumer)
    {
        foreach (var keyValuePair in _items)
        {
            consumer.Invoke(keyValuePair.Key, keyValuePair.Value);
        }
    }

    public IItem? Get(int slot)
    {
        return HasItem(slot) ? GetOrThrow(slot) : null;
    }

    public IItem GetOrThrow(int slot)
    {
        if (_items.TryGetValue(slot, out var item))
        {
            return item;
        }
        throw new KeyNotFoundException("Slot not present " + slot);
    }
}