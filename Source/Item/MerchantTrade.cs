using System.Collections.Generic;
using System.Linq;
using y1000.Source.Character;

namespace y1000.Source.Item;

public class MerchantTrade
{

    public long NpcId {get;} 

    public List<MerchantItem> MerchantItems {get;} 

    public bool Selling {get; set;}

    public long TotalMoney {get; set;}

    public MerchantTrade() :this(0, new List<MerchantItem>())
    {

    }

    public MerchantTrade(long id, List<MerchantItem> items,
        bool sell = false)
    {
        NpcId = id;
        MerchantItems = items;
        Selling = sell;
        TotalMoney = 0;
    }


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

    private readonly List<InventoryItem> _items = new();

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

    public bool IsBuying => !Selling;

    public string? CanHandleInventoryDoubleClick(string clickedItemName)
    {
        if (!IsBuying)
            return "交易中的窗口不可变更。";
        if (!MerchantItems.Where(i => i.ItemName.Equals(clickedItemName)).Any())
            return "不买此种物品。";
        return Items.Where(i => i.Item.Equals(clickedItemName)).Any() ? "交易中的物品不可变更。" : null;
    }

    public bool IsEmpty => _items.Count == 0;

    public bool Contains(string name)
    {
        return _items.Any(i => i.Item.ItemName.Equals(name));
    }


    private string? SellItmesToPlayer(string itemName, long number, CharacterInventory inventory)
    {
        var merchantItem = FindMerchantItem(itemName);
        if (merchantItem == null)
            return "不卖该物品。";
        if (!merchantItem.CanStack && number > 1)
            return "超出数量。";
        var cost = merchantItem.Price * number;
        if (!inventory.HasEnoughMoney(cost))
        {
            return "没有足够钱币。";
        }
        TotalMoney += cost;
        var item = merchantItem.ToItem(number);
        inventory.Buy(item, cost, this);
        return null;
    }

    
    private string? BuyItemsFromPlayer(string itemName, long number,
     CharacterInventory inventory, ItemFactory itemFactory)
    {

        var merchantItem = FindMerchantItem(itemName);
        if (merchantItem == null)
            return "不买该物品。";
        if (!inventory.HasEnough(itemName, number))
            return "没有足够数量。";
        if (!inventory.HasMoneySpace())
            return "物品栏已满。";
        var cost = merchantItem.Price * number;
        TotalMoney += cost;
        var item = merchantItem.ToItem(number);
        var money = (CharacterStackItem)itemFactory.CreateCharacterItem("钱币", TotalMoney);
        inventory.Sell(item, money, this);
        return null;
    }


    public InventoryItem? FindInventoryItem(string name)
    {
        return Items.First(i => i.Name.Equals(name));
    }

    public MerchantItem? FindMerchantItem(string name)
    {
        return MerchantItems.First(i => i.ItemName.Equals(name));
    }

    public string? OnInputNumberWindowConfirmed(string item, long number, CharacterInventory inventory, ItemFactory itemFactory)
    {
        if (number <= 0)
            return "请输入正确数量。";
        if (Selling)
        {
            return SellItmesToPlayer(item, number, inventory);
        }
        else
        {
            return BuyItemsFromPlayer(item, number, inventory, itemFactory);
        }
    }

    public void Rollback(CharacterInventory inventory)
    {
        if (IsBuying)
            inventory.RollbackSelling(this);
        else
            inventory.RollbackBuying(this);
    }
}