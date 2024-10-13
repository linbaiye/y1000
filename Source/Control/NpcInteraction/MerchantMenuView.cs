using System.Collections.Generic;
using System.Linq;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.Dialog;
using y1000.Source.Creature.Monster;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Sprite;

namespace y1000.Source.Control.NpcInteraction;

public partial class MerchantMenuView  : NinePatchRect, ISlotDoubleClickHandler 
{
    
    private Label _nameLabel;
    
    private RichTextLabel _dialog;

    private TextureRect _avatar;

    private Button _close;
    
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly IconReader _iconReader = IconReader.ItemIconReader;

    private TradeInputWindow? _tradeInputWindow;

    private TextureButton _confirmButton;
    
    private Label _total;

    private EventMediator? _eventMediator;

    private CharacterInventory? _inventory;

    private ItemFactory? _itemFactory;

    private ItemsContainer _itemsContainer;

    private MerchantTrade _trade = new();

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
        _itemsContainer.ItemDoubleClicked += OnMerchantItemDoubleClicked;
        Close();
    }

    private void OnMerchantItemDoubleClicked(Dialog.Item item)
    {
        if (_tradeInputWindow == null || !_trade.Selling)
        {
            return;
        }
        var itemName = item.ItemName;
        var merchantItem = _trade.FindMerchantItem(itemName);
        if (merchantItem == null)
            return;
        if (merchantItem.CanStack)
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
        if (_trade.IsBuying)
            _eventMediator?.NotifyServer(ClientTradeEvent.PlayerSell(_trade, _trade.NpcId));
        else
            _eventMediator?.NotifyServer(ClientTradeEvent.PlayerBuy(_trade, _trade.NpcId));
        Visible = false;
    }

    public void BindInventory(CharacterInventory inventory)
    {
        _inventory = inventory;
        _inventory.RegisterRightClickHandler(this);
    }

    private void Close()
    {
        if (_inventory != null)
            _trade?.Rollback(_inventory);
        Visible = false;
        _itemsContainer.Clear();
    }

    private void OnInputWindowAction(bool confirmed)
    {
        if (!confirmed || _trade == null || _tradeInputWindow == null ||
            _tradeInputWindow.ItemName == null || _inventory == null || _itemFactory == null)
        {
            return;
        }
        var ret = _trade.OnInputNumberWindowConfirmed(_tradeInputWindow.ItemName, _tradeInputWindow.Number, _inventory, _itemFactory);
        if (ret != null)
        {
            _eventMediator?.NotifyTextArea(ret);
            return;
        }
        var tradeItem = _trade.FindInventoryItem(_tradeInputWindow.ItemName);
        if (tradeItem == null)
            return;
        var i = _itemsContainer.FindItem(_tradeInputWindow.ItemName);
        i?.UpdateText((_trade.Selling ? "购买数量：" : "出售数量：") + tradeItem.Number);
        _total.Text = _trade.TotalMoney.ToString();
    }


    private void RefreshItemList(List<MerchantItem> items)
    {
        _itemsContainer.Clear();
        foreach (var item in items)
        {
            var icon = _iconReader.Get(item.IconId);
            if (icon != null)
                _itemsContainer.AddItem(item.ItemName, icon, item.Color, item.Price);
        }
    }

    public void Initialize(TradeInputWindow tradeInputWindow,
        EventMediator eventMediator, ItemFactory itemFactory)
    {
        _tradeInputWindow = tradeInputWindow;
        _eventMediator = eventMediator;
        _itemFactory = itemFactory;
    }


    public void Handle(MerchantMenuMessage message)
    {
        _nameLabel.Text = message.Name;
        _avatar.Texture = message.Avatar;
        _dialog.Text = message.Text;
        RefreshItemList(message.Items);
        if (message.Sell)
        {
            _confirmButton.TexturePressed = (Texture2D)ResourceLoader.Load("res://assets/ui/buy_down.png");
            _confirmButton.TextureNormal = (Texture2D)ResourceLoader.Load("res://assets/ui/buy_normal.png");
        }
        else
        {
            _confirmButton.TexturePressed = (Texture2D)ResourceLoader.Load("res://assets/ui/input_confirm_down.png");
            _confirmButton.TextureNormal = (Texture2D)ResourceLoader.Load("res://assets/ui/confirm_normal.png");
        }
        _total.Text = "0";
        _trade = new MerchantTrade(message.Id, message.Items, message.Sell);
        Visible = true;
    }


    public bool IsTrading
    {
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
        if (!Visible)
        {
            return false;
        }
        var characterItem = inventory.Find(slot);
        if (characterItem == null)
        {
            return false;
        }
        var ret = _trade.CanHandleInventoryDoubleClick(characterItem.ItemName);
        if (ret != null)
        {
            _eventMediator?.NotifyTextArea(ret);
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