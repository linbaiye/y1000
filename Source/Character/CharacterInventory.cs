using System;
using System.Collections.Generic;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Networking;

namespace y1000.Source.Character;

public class CharacterInventory
{
    private const int MaxSize = 30;

    public static readonly CharacterInventory Empty = new();
    
    
    private readonly IDictionary<int, ICharacterItem> _items = new Dictionary<int, ICharacterItem>(MaxSize);

    public event EventHandler<EventArgs>? InventoryChanged;


    private EventMediator? _eventMediator;

    public bool IsFull => _items.Count >= MaxSize;
    

    public void DropItem(int slot, int numberLeft)
    {
        if (numberLeft == 0)
        {
            _items.Remove(slot);
        }

        if (_items.TryGetValue(slot, out var itm))
        {
            if (itm is CharacterStackItem stackItem)
            {
                stackItem.Number = numberLeft;
            }
        }
        Notify();
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

    public void Update(int slot, ICharacterItem item)
    {
        _items.Remove(slot);
        _items.TryAdd(slot, item);
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

    public void OnUIDoubleClick(int slot)
    {
        if (_items.ContainsKey(slot))
        {
            _eventMediator?.NotifyServer(new DoubleClickInventorySlotMessage(slot));
        }
    }

    public void OnUIDragItem(int slot)
    {
        if (_items.TryGetValue(slot, out var item))
        {
            _eventMediator?.NotifyUiEvent(new DragInventorySlotEvent(slot, item));
        }
    }

    public void SetEventMediator(EventMediator eventMediator)
    {
        _eventMediator = eventMediator;
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
            _eventMediator?.NotifyServer(new SwapInventorySlotMessage(pickedSlot, slot2));
        }
    }
}