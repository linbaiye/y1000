using System.Collections.Generic;
using System.Text.RegularExpressions;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Creature.Monster;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Control.Dialog;

public partial class MerchantTrading : AbstractMerchantControl
{
    private ItemList _itemList;
    
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly IconReader _iconReader = IconReader.ItemIconReader;

    private TradeInputWindow? _tradeInputWindow;

    private bool _playerSelling;
    
    private TextureButton _confirmButton;
    
    private Label _total;

    private EventMediator? _eventMediator;

    private CharacterInventory? _inventory;

    private readonly MerchantTrade _trade = new();
    
    private class Item
    {
        private Item(int price, string name)
        {
            Price = price;
            Name = name;
        }
        public string Name { get; }
        public int Price { get; }

        public static Item Parse(string text)
        {
            var tokens = Regex.Split(text, @"\s+");
            return new Item(int.Parse(tokens[1]), tokens[0]);
        }
    }

    private void AddToTotal(int delta)
    {
        int current = string.IsNullOrEmpty(_total.Text) ? 0 : int.Parse(_total.Text);
        _total.Text = (current + delta).ToString();
    }

    public override void _Ready()
    {
        base._Ready();
        _itemList = GetNode<ItemList>("ItemList");
        _confirmButton = GetNode<TextureButton>("ConfirmButton");
        _confirmButton.Pressed += OnConfirmTrade;
        GetNode<TextureButton>("CancelButton").Pressed += Close;
        _total = GetNode<Label>("Total");
        _itemList.ItemClicked += OnItemClicked;
        Close();
    }

    private void OnConfirmTrade()
    {
        if (_trade.IsEmpty || Merchant == null)
        {
            return;
        }

        if (_playerSelling)
        {
            _inventory?.OnSell(Merchant.Id, _trade);
        }
        Visible = false;
    }

    public void BindInventory(CharacterInventory inventory)
    {
        _inventory = inventory;
    }

    public override void Close()
    {
        Visible = false;
        _itemList.Clear();
    }

    private void OnItemClicked(long index, Vector2 vector2, long buttonIndex)
    {
        if ((int)buttonIndex != (int)MouseButtonMask.Left || _tradeInputWindow == null)
        {
            return;
        }
        var itemText = _itemList.GetItemText((int)index);
        Item item = Item.Parse(itemText);
        _tradeInputWindow.Open(item.Name, OnWindowAction);
    }

    private void OnWindowAction(bool confirmed)
    {
        if (!confirmed)
        {
            return;
        }

        if (_playerSelling)
        {
            OnConfirmSell();
        }
        else
        {
            OnConfirmBuy();
        }
    }

    private void OnConfirmBuy()
    {
        var selectedItems = _itemList.GetSelectedItems();
        OnConfirmItem(selectedItems);
    }

    private void OnConfirmItem(int[] indices)
    {
        if (_tradeInputWindow == null || _tradeInputWindow.ItemName == null)
        {
            return;
        }
        var name = _tradeInputWindow.ItemName;
        foreach (var idx in indices)
        {
            var text = _itemList.GetItemText(idx);
            if (text == null)
            {
                continue;
            }
            var item = Item.Parse(text);
            if (item.Name.Equals(name))
            {
                _itemList.SetItemText(idx, text + "    " + (_playerSelling ? "出售" : "购买") + "数量: " + _tradeInputWindow.Number);
                _itemList.SetItemDisabled(idx, true);
                AddToTotal(_tradeInputWindow.Number * item.Price);
                _trade.Add(name, _tradeInputWindow.Number);
                break;
            }
        }
    }

    private void OnConfirmSell()
    {
        var array = new List<int>();
        for (var i = 0; i < _itemList.ItemCount; i++)
        {
            array.Add(i);
        }
        OnConfirmItem(array.ToArray());
    }

    private void RefreshItemList(List<Merchant.Item> items)
    {
        _itemList.Clear();
        foreach (var item in items)
        {
            var icon = _iconReader.Get(item.IconId);
            _itemList.AddItem(item.Name + "  " + item.Price, icon);
        }
    }

    public void Initialize(TradeInputWindow tradeInputWindow, EventMediator eventMediator)
    {
        _tradeInputWindow = tradeInputWindow;
        _eventMediator = eventMediator;
    }

    public void Buy(Merchant merchant, ISpriteRepository spriteRepository)
    {
        PopulateCommonFields(merchant, spriteRepository, "侠士需要什么？我这里物廉价美。");
        RefreshItemList(merchant.SellItems);
        _playerSelling = false;
        _confirmButton.TexturePressed = (Texture2D)ResourceLoader.Load("res://assets/ui/buy_down.png");
        _confirmButton.TextureNormal = (Texture2D)ResourceLoader.Load("res://assets/ui/buy.png");
        _total.Text = "0";
        _trade.Clear();
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
        _trade.Clear();
        Open();
    }

    public bool OnInventorySlotClick(ClickInventorySlotEvent slotEvent)
    {
        if (!Visible || !_playerSelling || Merchant == null)
        {
            return false;
        }
        var characterItem = slotEvent.Inventory.Find(slotEvent.Slot);
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
            _tradeInputWindow?.Open(stackItem.ItemName, stackItem.Number, OnWindowAction);
        }
        else
        {
            _tradeInputWindow?.Open(characterItem.ItemName, OnWindowAction);
        }
        return true;
    }
}