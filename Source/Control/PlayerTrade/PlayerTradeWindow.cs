using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Sprite;

namespace y1000.Source.Control.PlayerTrade;

public partial class PlayerTradeWindow : NinePatchRect, ISlotDoubleClickHandler
{
    private InventorySlotView[] _mySlots = new InventorySlotView[4];
    
    private InventorySlotView[] _playerSlots = new InventorySlotView[4];

    private Label _myNameLabel;
    
    private Label _anoterNameLabel;

    private TextureButton _confirmButton;
    
    private TextureButton _cancelButton;
    
    private readonly IconReader _itemIconReader = IconReader.ItemIconReader;

    private TradeInputWindow _tradeInputWindow;
    
    private CharacterInventory _inventory;
    private readonly ItemFactory _itemFactory = ItemFactory.Instance;
    
    private EventMediator? EventMediator { get; set; }

    public override void _Ready()
    {
        for (int i = 1; i <= 4; i++)
        {
            _mySlots[i - 1] = GetNode<InventorySlotView>("Slot" + i);
            _mySlots[i - 1].OnMouseInputEvent += OnMouseSlotEvent;
        }
        for (int i = 5; i <= 8; i++)
        {
            _playerSlots[i - 5] = GetNode<InventorySlotView>("Slot" + i);
        }
        _myNameLabel = GetNode<Label>("MyNameLabel");
        _anoterNameLabel = GetNode<Label>("AnotherNameLabel");
        _confirmButton = GetNode<TextureButton>("ConfirmButton");
        _cancelButton = GetNode<TextureButton>("CancelButton");
        _confirmButton.Pressed += OnConfirmPressed;
        _cancelButton.Pressed += OnCancelPressed;
        Visible = false;
    }


    public void Initialize(TradeInputWindow tradeInputWindow, EventMediator eventMediator)
    {
        _tradeInputWindow = tradeInputWindow;
        EventMediator = eventMediator;
    }

    private void OnMouseSlotEvent(object? sender, SlotMouseEvent slotMouseEvent)
    {
        if (sender is not InventorySlotView slotView)
        {
            return;
        }
        if (slotMouseEvent.EventType == SlotMouseEvent.Type.MOUSE_LEFT_DOUBLE_CLICK && slotView.Number < 5)
        {
            EventMediator?.NotifyServer(ClientUpdateTradeEvent.RemoveItem(slotView.Number));
        }
    }


    private void Clear()
    {
        foreach (var slot in _mySlots)
        {
            slot.ClearTextureAndTip();
        }
        foreach (var slot in _playerSlots)
        {
            slot.ClearTextureAndTip();
        }
        _myNameLabel.Text = "";
        _anoterNameLabel.Text = "";
        _confirmButton.ButtonPressed = false;
    }

    public void Open(CharacterImpl character, string anotherName, int slot = 0)
    {
        if (Visible || (slot != 0 && !character.Inventory.HasItem(slot)))
        {
            return;
        }
        character.Inventory.RegisterRightClickHandler(this);
        Clear();
        _myNameLabel.Text = character.EntityName;
        _anoterNameLabel.Text = anotherName;
        if (slot != 0)
        {
            var item = character.Inventory.GetOrThrow(slot);
            _tradeInputWindow.Open(new TradeInputWindow.InventoryTradeItem(item.ItemName, slot), 1, OnInputWindowEvent);
        }
        _inventory = character.Inventory;
        Visible = true;
    }

    private void AddItem(UpdateTradeWindowMessage message)
    {
        var item = _itemFactory.CreateCharacterItem(message.Name, message.Color, message.Number);
        var texture2D = _itemIconReader.Get(item.IconId);
        if (texture2D == null)
        {
            return;
        }
        if (message.Self)
        {
            _mySlots[message.Slot - 1].PutTextureAndTooltip(texture2D, item.ItemName + ":" + message.Number, item.Color);
        }
        else
        {
            _playerSlots[message.Slot - 1].PutTextureAndTooltip(texture2D, item.ItemName + ":" + message.Number, item.Color);
        }
    }

    private void RemoveItem(UpdateTradeWindowMessage message)
    {
        if (message.Self)
        {
            _mySlots[message.Slot - 1].ClearTextureAndTip();
        }
        else
        {
            _playerSlots[message.Slot - 1].ClearTextureAndTip();
        }
        
    }

    public void Update(UpdateTradeWindowMessage message)
    {
        if (message.Type == UpdateTradeWindowMessage.UpdateType.CLOSE_WINDOW)
        {
            Visible = false;
        }
        else if (message.Type == UpdateTradeWindowMessage.UpdateType.ADD_ITEM)
        {
            AddItem(message);
        }
        else if (message.Type == UpdateTradeWindowMessage.UpdateType.REMOVE_ITEM)
        {
            RemoveItem(message);
        }
    }


    private void OnInputWindowEvent(bool confirmed)
    {
        if (!confirmed)
        {
            return;
        }
        var tradeItem = _tradeInputWindow.TradeItem;
        if (tradeItem == null)
        {
            return;
        }
        if (!_inventory.HasEnough(tradeItem.Slot, tradeItem.Name, _tradeInputWindow.Number))
        {
            EventMediator?.NotifyTextArea("持有数量不足。");
            return;
        }
        EventMediator?.NotifyServer(ClientUpdateTradeEvent.AddItem(tradeItem.Slot, _tradeInputWindow.Number));
    }

    private void OnCancelPressed()
    {
        EventMediator?.NotifyServer(ClientUpdateTradeEvent.Cancel());
    }
    
    private void OnConfirmPressed()
    {
        EventMediator?.NotifyServer(ClientUpdateTradeEvent.Confirm());
    }
    

    public bool HandleInventorySlotDoubleClick(CharacterInventory inventory, int slot)
    {
        if (!Visible)
        {
            return false;
        }
        var item = inventory.GetOrThrow(slot);
        _tradeInputWindow.Open(new TradeInputWindow.InventoryTradeItem(item.ItemName, slot), 1, OnInputWindowEvent);
        return true;
    }
}