using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using y1000.Source.Item;

namespace y1000.Source.Character;

public class CharacterInventory
{
    
    public const int MaxSize = 30;
    
    
    private readonly IDictionary<int, ICharacterItem> _items = new Dictionary<int, ICharacterItem>(MaxSize);

    public event EventHandler<EventArgs>? InventoryChanged;

    public bool IsFull => _items.Count >= MaxSize;

    public bool AddItem(ICharacterItem item)
    {
        if (IsFull)
        {
            return false;
        }
        for (var i = 1; i <= MaxSize; i++)
        {
            if (_items.TryAdd(i, item))
            {
                InventoryChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
        }
        return false;
    }

    public ICharacterItem? Find(int slot)
    {
        return _items.TryGetValue(slot, out var item) ? item : null;
    }

    public bool AddItem(int slot, ICharacterItem item)
    {
        if (slot < 1 || slot > MaxSize)
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


    public void AddItems(Collection<ICharacterItem> items)
    {
        if (items.Count + _items.Count >= MaxSize)
        {
            return;
        }
        foreach (var item in items)
        {
            AddItem(item);
        }
        InventoryChanged?.Invoke(this, EventArgs.Empty);
    }
}