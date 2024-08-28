using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using NLog;
using y1000.Source.Character.Event;
using y1000.Source.Control.Bottom.Shortcut;
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

    public bool CanBuy(string name, long totalMoney)
    {
        return HasEnoughMoney(totalMoney) && HasSpace(name);
    }

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

    public void RollbackSelling(MerchantTrade trade)
    {
        if (trade.IsEmpty)
        {
            return;
        }
        foreach (var inventoryItem in trade.Items)
        {
            if (_items.TryGetValue(inventoryItem.Slot, out var slotItem))
            {
                if (slotItem is CharacterStackItem stackItem)
                {
                    stackItem.Number += inventoryItem.Number;
                }
            }
            else
            {
                _items.TryAdd(inventoryItem.Slot, inventoryItem.Item);
            }
        }
        int slot = FindMoneySlot(out var money);
        if (money != null)
        {
            money.Number -= trade.Money?.Number ?? 0;
            if (money.Number <= 0)
            {
                _items.Remove(slot);
            }
        }
        Notify();
    }

    public void RollbackBuying(MerchantTrade trade)
    {
        if (trade.IsEmpty || trade.Money == null)
        {
            return;
        }
        foreach (var tradeItem in trade.Items)
        {
            bool needRemove = false;
            if (_items.TryGetValue(tradeItem.Slot, out var characterItem))
            {
                if (characterItem is CharacterStackItem stackItem)
                {
                    stackItem.Number -= tradeItem.Number;
                    needRemove = stackItem.Number <= 0;
                }
                else
                {
                    needRemove = true;
                }
            }
            if (needRemove)
            {
                _items.Remove(tradeItem.Slot);
            }
        }
        if (_items.TryGetValue(trade.Money.Slot, out var moneyInSlot))
        {
            ((CharacterStackItem)moneyInSlot).Number += trade.Money.Number;
        }
        else
        {
            _items.TryAdd(trade.Money.Slot, trade.Money.Item);
        }
        Notify();
    }


    public bool HasEnoughMoney(long number)
    {
        var item = _items.Values.FirstOrDefault(i => i.ItemName.Equals(Money));
        return item is CharacterStackItem money && money.Number >= number;
    }

    private int FindMoneySlot(out CharacterStackItem? stackItem)
    {
        for (int i = 1; i <= MaxSize; i++)
        {
            if (_items.TryGetValue(i, out var slotItem) && slotItem.ItemName.Equals(Money) && 
                slotItem is CharacterStackItem characterStackItem)
            {
                stackItem = characterStackItem;
                return i;
            }
        }
        stackItem = null;
        return 0;
    }

    public bool Sell(IItem sellingItem, CharacterStackItem money, MerchantTrade trade)
    {
        if (!HasMoneySpace())
        {
            return false;
        }
        IItem? slotItem = null;
        int i = 1;
        for (; i <= MaxSize; i++)
        {
            if (_items.TryGetValue(i, out slotItem) &&
                slotItem.ItemName.Equals(sellingItem.ItemName))
            {
                break;
            }
        }
        if (slotItem == null)
        {
            return false;
        }
        if (slotItem is CharacterItem)
        {
            _items.Remove(i);
        } 
        else if (slotItem is CharacterStackItem stackItem)
        {
            stackItem.Number -= ((CharacterStackItem)sellingItem).Number;
            if (stackItem.Number <= 0)
            {
                _items.Remove(i);
            }
        }
        int moneySlot = FindMoneySlot(out var moneyItem);
        if (moneyItem != null)
        {
            moneyItem.Number += money.Number;
        }
        else
        {
            AddItem(money);
        }
        trade.AddItem(sellingItem, i, money, moneySlot);
        Notify();
        return true;
    }

    public bool Buy(IItem item, long totalMoney, MerchantTrade trade)
    {
        if (!CanBuy(item.ItemName, totalMoney))
        {
            return false;
        }
        var moneySlot = FindMoneySlot(out var money);
        if (money == null)
        {
            return false;
        }
        var slot = AddItem(item);
        if (slot != 0)
        {
            money.Number -= totalMoney;
            if (money.Number <= 0)
            {
                _items.Remove(moneySlot);
            }
            trade.AddItem(item, slot, new CharacterStackItem(money.IconId, money.ItemName, totalMoney), moneySlot);
            Notify();
        }
        return slot != 0;
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
        if (item != null && item.ItemName.Equals("福袋"))
        {
            _eventMediator?.NotifyServer(ClientOperateBankEvent.Unlock(slot));
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


    public void OnSell(long merchantId, MerchantTrade trade)
    {
        _eventMediator?.NotifyServer(ClientTradeEvent.Sell(trade, merchantId));
    }

    
    public void OnBuy(long merchantId, MerchantTrade trade)
    {
        _eventMediator?.NotifyServer(ClientTradeEvent.Buy(trade, merchantId));
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