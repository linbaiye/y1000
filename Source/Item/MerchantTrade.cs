using System.Collections.Generic;
using System.Linq;
using Godot.NativeInterop;
using y1000.Source.Character;

namespace y1000.Source.Item;

public class MerchantTrade
{

    public long NpcId {get;}

    private List<MerchantItem> MerchantItems {get;} 

    public bool Selling {get; }

    public long TotalMoney => Money?.Number ?? 0;

    public MerchantTrade() :this(0, new List<MerchantItem>())
    {

    }

    public MerchantTrade(long id, List<MerchantItem> items,
        bool sell = false)
    {
        NpcId = id;
        MerchantItems = items;
        Selling = sell;
    }


    public class TradingItem
    {
        public TradingItem(int slotId, IItem item)
        {
            Slot = slotId;
            Item = item;
        }

        public int Slot { get; }
        public IItem Item { get; }
        public string Name => Item.ItemName;
        public long Number => Item is CharacterStackItem stackItem ? stackItem.Number : 1;
    }

    private readonly List<TradingItem> _tradingItems = new();

    private TradingItem? _money;

    private TradingItem? Money => _money;

    public List<TradingItem> TradingItems => _tradingItems;
    
    public void AddTrade(IItem item, int slot,
        CharacterStackItem money, int moneySlot)
    {
        if (item is not CharacterStackItem addStackItem)
        {
            _tradingItems.Add(new TradingItem(slot, item.Duplicate()));
        }
        else
        {
            bool added = false;
            foreach (var tradingItem in _tradingItems)
            {
                if (tradingItem.Item.ItemName.Equals(item.ItemName) &&
                    tradingItem.Item is CharacterStackItem stackItem)
                {
                    stackItem.Number += addStackItem.Number;
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                _tradingItems.Add(new TradingItem(slot, addStackItem.Duplicate()));
            }
        }
        if (_money == null)
        {
            _money = new TradingItem(moneySlot, money.Duplicate());
        }
        else
        {
            ((CharacterStackItem)_money.Item).Number += money.Number;
        }
    }

    public bool IsBuying => !Selling;

    public string? CanHandleInventoryDoubleClick(string clickedItemName)
    {
        if (!IsBuying)
            return "交易中的窗口不可变更。";
        return MerchantItems.Any(i => i.ItemName.Equals(clickedItemName)) ? null : "不买此种物品。";
    }

    public bool IsEmpty => _tradingItems.Count == 0;


    private string? SellItemsToPlayer(string itemName, long number, CharacterInventory inventory)
    {
        var merchantItem = FindMerchantItem(itemName);
        if (merchantItem == null)
            return "不卖该物品。";
        if ((!merchantItem.CanStack && number > 1) || number > 10000000)
            return "超出数量。";
        var cost = merchantItem.Price * number;
        if (!inventory.HasEnoughMoney(cost))
        {
            return "没有足够钱币。";
        }
        if (!inventory.HasSpace(itemName))
        {
            return "物品栏已满。";
        }
        var item = merchantItem.ToItem(number);
        int itemSlot = inventory.Add(item);
        int moneySlot = inventory.FindMoneySlot();
        var money = inventory.DecreaseMoney(cost);
        AddTrade(item, itemSlot, money, moneySlot);
        return null;
    }

    
    private string? BuyItemsFromPlayer(string itemName, long number,
     CharacterInventory inventory, ItemFactory itemFactory, int slot)
    {
        var merchantItem = FindMerchantItem(itemName);
        if (merchantItem == null)
            return "不买该物品。";
        if (!inventory.HasEnough(slot, number))
            return "没有足够数量。";
        if (!inventory.HasMoneySpace())
            return "物品栏已满。";
        var cost = merchantItem.Price * number;
        var item = inventory.Remove(slot, number);
        var money = (CharacterStackItem)itemFactory.CreateCharacterItem("钱币", cost);
        int moneySlot = inventory.Add(money);
        AddTrade(item, slot, money, moneySlot);
        return null;
    }


    public long ComputeTradingItemNumber(string name)
    {
        long count = 0;
        foreach (var item in TradingItems)
        {
            if (!item.Item.ItemName.Equals(name))
            {
                continue;
            }

            if (item.Item is CharacterStackItem stackItem)
            {
                count += stackItem.Number;
            }
            else
            {
                count += 1;
            }
        }
        return count;
    }

    public MerchantItem? FindMerchantItem(string name)
    {
        return MerchantItems.FirstOrDefault(i => i.ItemName.Equals(name));
    }

    public string? OnInputNumberWindowConfirmed(string item, long number, CharacterInventory inventory, ItemFactory itemFactory, int slot = 0)
    {
        if (number <= 0)
            return "请输入正确数量。";
        return Selling ? SellItemsToPlayer(item, number, inventory) :
            BuyItemsFromPlayer(item, number, inventory, itemFactory, slot);
    }


    private void RollbackSelling(CharacterInventory inventory)
    {
        foreach (var tradingItem in TradingItems)
        {
            if (tradingItem.Item is CharacterItem)
            {
                inventory.Remove(tradingItem.Slot);
            }
            else if (tradingItem.Item is CharacterStackItem stackItem)
            {
                inventory.Remove(tradingItem.Slot, stackItem.Number);
            }
        }
        if (Money != null)
            inventory.Add(Money.Item, Money.Slot);
    }

    private void RollbackBuying(CharacterInventory inventory)
    {
        if (Money != null)
            inventory.Remove(Money.Slot, Money.Number);
        foreach (var tradingItem in TradingItems)
        {
            inventory.Add(tradingItem.Item, tradingItem.Slot);
        }
    }

    public void Rollback(CharacterInventory inventory)
    {
        if (IsBuying)
            RollbackBuying(inventory);
        else
            RollbackSelling(inventory);
    }
}