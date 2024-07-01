using System.Collections.Generic;
using System.Text.RegularExpressions;
using Godot;
using NLog;
using y1000.Source.Creature.Monster;
using y1000.Source.Sprite;
using y1000.Source.Util;

namespace y1000.Source.Control.Dialog;

public partial class MerchantTrading : AbstractMerchantControl
{
    private ItemList _itemList;
    
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly ItemSdbReader _itemSdbReader = ItemSdbReader.ItemSdb;
    
    private readonly IconReader _iconReader = IconReader.ItemIconReader;

    private TradeInputWindow? _tradeInputWindow;

    private bool _selling;
    
    private TextureButton _confirmButton;
    
    private Label _total;
    
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

    private void AddToTotal()
    {
        _total.Text = "";
    }

    public override void _Ready()
    {
        base._Ready();
        _itemList = GetNode<ItemList>("ItemList");
        _confirmButton = GetNode<TextureButton>("ConfirmButton");
        GetNode<TextureButton>("CancelButton").Pressed += Close;
        _total = GetNode<Label>("Total");
        _itemList.ItemClicked += OnItemClicked;
        Close();
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

        var name = _tradeInputWindow.ItemName;
        var selectedItems = _itemList.GetSelectedItems();
        foreach (var selectedItem in selectedItems)
        {
            var text = _itemList.GetItemText(selectedItem);
            if (text.Contains(name))
            {
                var item = Item.Parse(text);
                _itemList.SetItemText(selectedItem, text + "    " + (_selling ? "出售" : "购买") + "数量: " + _tradeInputWindow.Number);
                Logger.Debug("Trade item {}, number {}.", item.Name, _tradeInputWindow.Number);
                break;
            }
        }
    }

    private void RefreshItemList(List<string> items)
    {
        _itemList.Clear();
        foreach (var item in items)
        {
            if (!_itemSdbReader.Contains(item))
            {
                continue;
            }
            var price = _itemSdbReader.GetPrice(item);
            var icon = _iconReader.Get(_itemSdbReader.GetIconId(item));
            _itemList.AddItem(item + "  " + price, icon);
        }
    }

    public void Initialize(TradeInputWindow tradeInputWindow)
    {
        _tradeInputWindow = tradeInputWindow;
    }

    public void Buy(Merchant merchant, ISpriteRepository spriteRepository)
    {
        PopulateCommonFields(merchant, spriteRepository, "侠士需要什么？我这里物廉价美。");
        RefreshItemList(merchant.SellItems);
        _selling = false;
        _confirmButton.TexturePressed = (Texture2D)ResourceLoader.Load("res://assets/ui/buy_down.png");
        _confirmButton.TextureNormal = (Texture2D)ResourceLoader.Load("res://assets/ui/buy.png");
        _total.Text = "0";
        Open();
    }
    
    public void Sell(Merchant merchant, ISpriteRepository spriteRepository)
    {
        PopulateCommonFields(merchant, spriteRepository, "侠士想卖东西吗？我这里高价收购。");
        RefreshItemList(merchant.BuyItems);
        _confirmButton.TexturePressed = (Texture2D)ResourceLoader.Load("res://assets/ui/input_confirm_down.png");
        _confirmButton.TextureNormal = (Texture2D)ResourceLoader.Load("res://assets/ui/confirm_normal.png");
        _selling = true;
        _total.Text = "0";
        Open();
    }
}