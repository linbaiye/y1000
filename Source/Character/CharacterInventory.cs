using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using NLog;
using y1000.Source.Character.Event;
using y1000.Source.Control;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Networking;

namespace y1000.Source.Character;

public class CharacterInventory : AbstractInventory
{
    private const int MaxSize = 30;

    public static readonly CharacterInventory Empty = new();

    public CharacterInventory() : base(MaxSize)
    {
    }

    public event EventHandler<EventArgs>? InventoryChanged;
    
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public event EventHandler<InventoryShortcutEvent>? ShortcutEvent;


    private EventMediator? _eventMediator;
    private const string Money = "钱币";

    private readonly List<ISlotDoubleClickHandler> _rightClickHandlers = new();

    public void RegisterRightClickHandler(ISlotDoubleClickHandler handler)
    {
        if (!_rightClickHandlers.Contains(handler))
            _rightClickHandlers.Add(handler);
    }
    
    public void DeregisterRightClickHandler(ISlotDoubleClickHandler handler)
    {
        _rightClickHandlers.Remove(handler);
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
            Notify();
        }
    }

    public bool CanPut(int slot, IItem item)
    {
        return CanPut(item, slot);
    }

    public bool HasMoneySpace()
    {
        return HasSpace(Money);
    }
    
    public CharacterStackItem? DecreaseMoney(long number)
    {
        int slot = FindMoneySlot();
        if (slot != 0)
        {
            var money = (CharacterStackItem)Get(slot)!;
            Remove(slot, number);
            return money.WithNumber(number);
        }
        return null;
    }

    public int FindMoneySlot()
    {
        return FindSlot(Money);
    }

    public bool HasEnoughMoney(long number)
    {
        var item = _items.Values.FirstOrDefault(i => i.ItemName.Equals(Money));
        return item is CharacterStackItem money && money.Number >= number;
    }

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

    public void OnUIDoubleClick(int slot)
    {
        if (!_items.ContainsKey(slot))
        {
            return;
        }
        foreach (var handler in _rightClickHandlers)
        {
            if (handler.HandleInventorySlotDoubleClick(this, slot))
            {
                return;
            }
        }
        var item = Get(slot);
        if (item == null)
        {
            return;
        }
        if (item.ItemName.Equals("福袋"))
        {
            _eventMediator?.NotifyServer(ClientOperateBankEvent.Unlock(slot));
        }
        else if (item.ItemName.Equals("门派石"))
        {
            _eventMediator?.NotifyServer(new ClientFoundGuildEvent("门派名字不能太长了",  new Vector2I(507, 516), 1));
        }
        else
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
    
    public void OnRightClick(int slot)
    {
        _eventMediator?.NotifyServer(new ClientRightClickEvent(RightClickType.INVENTORY, slot));
    }

    public void OnUISwap(int pickedSlot, int slot2)
    {
        if (_items.ContainsKey(pickedSlot))
        {
            _eventMediator?.NotifyServer(new SwapInventorySlotMessage(pickedSlot, slot2));
        }
    }

    public IItem? Remove(int slot, long number = 1)
    {
        IItem? removed = null;
        var slotItem = Get(slot);
        if (slotItem is CharacterItem)
        {
            _items.Remove(slot);
            removed = slotItem;
        } 
        else if (slotItem is CharacterStackItem stackItem)
        {
            stackItem.Number -= number;
            if (stackItem.Number <= 0)
            {
                _items.Remove(slot);
            }
            removed = stackItem.WithNumber(number);
        }
        if (removed != null)
            Notify();
        return removed;
    }

    public int Add(IItem item)
    {
        var slot = FindSlot(item.ItemName);
        if (slot != 0)
        {
            if (Add(item, slot))
                return slot;
        }
        for (int i = 1; i <= MaxSize; i++)
        {
            if (_items.TryAdd(i, item))
            {
                slot = i;
                break;
            }
        }
        Notify();
        return slot;
    }

    public bool Add(IItem item, int slot)
    {
        bool added = false;
        var slotItem = Get(slot);
        if (slotItem == null)
        {
            _items.Add(slot, item);
            added = true;
        }
        else if (slotItem is CharacterStackItem slotStackItem && item is CharacterStackItem stackItem &&
                 slotItem.ItemName.Equals(item.ItemName))
        {
            slotStackItem.Number += stackItem.Number;
            added = true;
        }
        if (added)
            Notify();
        return added;
    }

    public void OnViewKeyPressed(int slot, Key key)
    {
        if (_items.TryGetValue(slot, out var item))
        {
            ShortcutEvent?.Invoke(this, new InventoryShortcutEvent(ShortcutContext.OfInventory(slot, key), item));
        }
    }

    protected override void Notify()
    {
        InventoryChanged?.Invoke(this, EventArgs.Empty);
    }
}