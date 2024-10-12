using System.Collections.Generic;
using System.Linq;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.Dialog;
using y1000.Source.Creature.Monster;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Control.NpcInteraction;

public partial class MerchantMenuView  : NinePatchRect
{
    
    private Label _nameLabel;
    
    private RichTextLabel _dialog;

    private TextureRect _avatar;

    private Button _close;
    
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly IconReader _iconReader = IconReader.ItemIconReader;

    private TradeInputWindow? _tradeInputWindow;

    private bool _playerSelling;
    
    private TextureButton _confirmButton;
    
    private Label _total;

    private EventMediator? _eventMediator;

    private CharacterInventory? _inventory;

    private MerchantTrade _trade = new();
    
    private ItemFactory? _itemFactory;

    private ItemsContainer _itemsContainer;
    
    private List<Item> SellItems { get; set; } = new();

    private List<Item> BuyItems { get; set; } = new ();
    
    private 

    public class Item
    {
        public Item(string name, int price, int iconId, int color = 0)
        {
            Price = price;
            IconId = iconId;
            Color = color;
            Name = name;
        }
        public string Name { get; }
        public int Price { get; }
        public int IconId { get; }
        
        public int Color { get; }
    }

    public Item? FindInSell(string name)
    {
        return SellItems.FirstOrDefault(i => i.Name.Equals(name));
    }
    
    public Item? FindInBuy(string name)
    {
        return BuyItems.FirstOrDefault(i => i.Name.Equals(name));
    }

    private bool Buys(string name)
    {
        return BuyItems.Any(i => i.Name.Equals(name));
    }
    
    public override void _Ready()
    {
        _nameLabel = GetNode<Label>("Name");
        _dialog = GetNode<RichTextLabel>("Dialog");
        _avatar = GetNode<TextureRect>("Avatar");
        _close = GetNode<Button>("Close");
        _close.Pressed += Close;
        _confirmButton = GetNode<TextureButton>("ConfirmButton");
        _confirmButton.Pressed += OnConfirmTrade;
        GetNode<TextureButton>("CancelButton").Pressed += Close;
        _total = GetNode<Label>("Total");
        _itemsContainer = GetNode<ItemsContainer>("ItemsContainer");
        _itemsContainer.ItemDoubleClicked += OnItemDoubleClicked;
        Close();
    }
    
        private void AddToTotal(long delta)
    {
        long current = string.IsNullOrEmpty(_total.Text) ? 0 : long.Parse(_total.Text);
        _total.Text = (current + delta).ToString();
    }

    private void OnItemDoubleClicked(Dialog.Item item)
    {
        if (_tradeInputWindow == null || _itemFactory == null)
        {
            return;
        }
        var itemName = item.ItemName;
        if (_itemFactory.IsStackItem(itemName))
        {
            _tradeInputWindow.Open(itemName, OnInputWindowAction);
        }
        else
        {
            _tradeInputWindow.OpenUniqueItem(itemName, OnInputWindowAction);
        }
    }
    private void OnConfirmTrade()
    {
        if (_trade.IsEmpty)
        {
            return;
        }
        if (_playerSelling)
        {
            _inventory?.OnSell(Id, _trade);
        }
        else
        {
            _inventory?.OnBuy(Merchant.Id, _trade);
        }
        Visible = false;
    }

    public void BindInventory(CharacterInventory inventory)
    {
        _inventory = inventory;
    }

    public void Close()
    {
        if (_playerSelling)
        {
            _inventory?.RollbackSelling(_trade);
        }
        else
        {
            _inventory?.RollbackBuying(_trade);
        }
        Visible = false;
        _itemsContainer.Clear();
    }

    private void OnInputWindowAction(bool confirmed)
    {
        if (!confirmed)
        {
            return;
        }
        if (_playerSelling)
        {
            OnConfirmSellItem();
        }
        else
        {
            OnConfirmBuyItem();
        }
    }

    private void OnConfirmBuyItem()
    {
        if (_tradeInputWindow == null || _tradeInputWindow.ItemName == null ||
            _itemFactory == null || Merchant == null || _inventory == null)
        {
            return;
        }
        var merchantSellingItem = Merchant.FindInSell(_tradeInputWindow.ItemName);
        if (merchantSellingItem == null)
        {
            return;
        }
        long total = merchantSellingItem.Price * _tradeInputWindow.Number;
        if (!_inventory.HasEnoughMoney(total))
        {
            _eventMediator?.NotifyTextArea("持有的钱币不足。");
            return;
        }
        if (!_inventory.HasSpace(_tradeInputWindow.ItemName))
        {
            _eventMediator?.NotifyTextArea("物品栏已满。");
            return;
        }
        var i = _itemsContainer.FindItem(_tradeInputWindow.ItemName);
        if (i == null)
        {
            return;
        }
        var item = _itemFactory.CreateCharacterItem(_tradeInputWindow.ItemName, i.IconColor, _tradeInputWindow.Number);
        if (!_inventory.Buy(item, total, _trade))
        {
            return;
        }
        i.Lock("购买数量: " + _tradeInputWindow.Number);
        AddToTotal(_tradeInputWindow.Number * i.Price);
    }


    private void OnConfirmSellItem()
    {
        if (_tradeInputWindow == null || _inventory == null || _itemFactory == null || !SellItems.Any())
        {
            return;
        }
        var name = _tradeInputWindow.ItemName;
        if (name == null)
        {
            return;
        }
        var buyingItem = FindInBuy(name);
        if (buyingItem == null)
        {
            return;
        }
        if (!_inventory.HasMoneySpace())
        {
            _eventMediator?.NotifyTextArea("物品栏已满。");
            return;
        }
        var number = _tradeInputWindow.Number;
        if (!_inventory.HasEnough(name, number))
        {
            _eventMediator?.NotifyTextArea("数量不足。");
            return;
        }
        var characterItem = _itemFactory.CreateCharacterItem(name, number);
        var money = (CharacterStackItem)_itemFactory.CreateCharacterItem(CharacterStackItem.MoneyName, number * buyingItem.Price);
        if (!_inventory.Sell(characterItem, money, _trade))
        {
            return;
        }
        var i = _itemsContainer.FindItem(name);
        if (i != null)
        {
            i.Lock("出售数量: " + _tradeInputWindow.Number);
            AddToTotal(_tradeInputWindow.Number * i.Price);
        }
    }

    private void RefreshItemList(List<Merchant.Item> items)
    {
        _itemsContainer.Clear();
        foreach (var item in items)
        {
            var icon = _iconReader.Get(item.IconId);
            if (icon != null)
                _itemsContainer.AddItem(item.Name, icon, item.Color, item.Price);
        }
    }

    public void Initialize(TradeInputWindow tradeInputWindow,
        EventMediator eventMediator, ItemFactory itemFactory)
    {
        _tradeInputWindow = tradeInputWindow;
        _eventMediator = eventMediator;
        _itemFactory = itemFactory;
    }

    public void Buy(Merchant merchant, ISpriteRepository spriteRepository)
    {
        PopulateCommonFields(merchant, spriteRepository, "侠士需要什么？我这里物廉价美。");
        RefreshItemList(merchant.SellItems);
        _playerSelling = false;
        _confirmButton.TexturePressed = (Texture2D)ResourceLoader.Load("res://assets/ui/buy_down.png");
        _confirmButton.TextureNormal = (Texture2D)ResourceLoader.Load("res://assets/ui/buy_normal.png");
        _total.Text = "0";
        _trade = new MerchantTrade();
        Open();
    }
    
    public void Sell(Merchant merchant, ISpriteRepository spriteRepository)
    {
        PopulateCommonFields(merchant, spriteRepository, "侠士要卖什么？我这里高价收购。");
        RefreshItemList(merchant.BuyItems);
        _confirmButton.TexturePressed = (Texture2D)ResourceLoader.Load("res://assets/ui/input_confirm_down.png");
        _confirmButton.TextureNormal = (Texture2D)ResourceLoader.Load("res://assets/ui/confirm_normal.png");
        _playerSelling = true;
        _total.Text = "0";
        _trade = new MerchantTrade();
        Open();
    }
    
    public bool IsTrading {
        get
        {
            if (_tradeInputWindow != null && _tradeInputWindow.Visible)
            {
                return true;
            }
            return Visible;
        }
    }

    public bool HandleInventorySlotDoubleClick(CharacterInventory inventory, int slot)
    {
        if (!Visible || !_playerSelling || Merchant == null)
        {
            return false;
        }
        var characterItem = inventory.Find(slot);
        if (characterItem == null)
        {
            return false;
        }
        if (!Merchant.Buys(characterItem.ItemName))
        {
            _eventMediator?.NotifyTextArea("不买此种物品。");
            return true;
        }
        if (_trade.Contains(characterItem.ItemName))
        {
            _eventMediator?.NotifyTextArea("交易中的物品无法变更。");
            return true;
        }
        if (characterItem is CharacterStackItem stackItem)
        {
            _tradeInputWindow?.Open(stackItem.ItemName, stackItem.Number, OnInputWindowAction);
        }
        else
        {
            _tradeInputWindow?.Open(characterItem.ItemName, 1, OnInputWindowAction);
        }
        return true;
    }

    
}