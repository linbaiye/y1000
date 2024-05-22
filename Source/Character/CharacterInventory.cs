using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using y1000.Source.Input;
using y1000.Source.Item;
using y1000.Source.Networking;

namespace y1000.Source.Character;

public class CharacterInventory
{
    
    public const int MaxSize = 30;

    public static readonly CharacterInventory Empty = new(i => { });
    
    
    private readonly IDictionary<int, ICharacterItem> _items = new Dictionary<int, ICharacterItem>(MaxSize);

    public event EventHandler<EventArgs>? InventoryChanged;

    private readonly Action<IClientEvent> _eventSender;

    public CharacterInventory(Action<IClientEvent> eventSender)
    {
        _eventSender = eventSender;
    }


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

    public void PutOrRemove(int slot, ICharacterItem? item)
    {
        _items.Remove(slot);
        if (item != null)
        {
            _items.TryAdd(slot, item);
        }
        Notify();
    }

    private void Notify()
    {
        InventoryChanged?.Invoke(this, EventArgs.Empty);
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

    public void OnDoubleClick(int slot)
    {
        if (_items.ContainsKey(slot))
        {
            _eventSender.Invoke(new DoubleClickInventorySlotMessage(slot));
        }
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

    public void OnUISwap(int pickedSlot, int slot2)
    {
        if (_items.ContainsKey(pickedSlot))
        {
            _eventSender.Invoke(new SwapInventorySlotMessage(pickedSlot, slot2));
        }
    }
}