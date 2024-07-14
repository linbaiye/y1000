using Godot;
using y1000.Source.Character;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.RightSide.Inventory;
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

    public override void _Ready()
    {
        for (int i = 1; i <= 4; i++)
        {
            _mySlots[i - 1] = GetNode<InventorySlotView>("Slot" + i);
            _mySlots[i - 1].OnInputEvent += OnSlotEvent;
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


    public void Initialize(TradeInputWindow tradeInputWindow)
    {
        _tradeInputWindow = tradeInputWindow;
    }

    private void OnSlotEvent(object? sender, SlotEvent slotEvent)
    {
        
    }


    private void Clear()
    {
        foreach (var slot in _mySlots)
        {
            slot.Clear();
        }
        foreach (var slot in _playerSlots)
        {
            slot.Clear();
        }

        _myNameLabel.Text = "";
        _anoterNameLabel.Text = "";
    }

    public void Open(CharacterImpl character, string anotherName, int slot = 0)
    {
        if (Visible || (slot != 0 && !character.Inventory.HasItem(slot)))
        {
            return;
        }
        Clear();
        _myNameLabel.Text = character.EntityName;
        _anoterNameLabel.Text = anotherName;
        if (slot != 0)
        {
            var item = character.Inventory.GetOrThrow(slot);
            _tradeInputWindow.Open(item.ItemName, 1, OnInputWindowEvent);
        }
        Visible = true;
    }

    public void Update(UpdateTradeWindowMessage message)
    {
        if (message.Type == UpdateTradeWindowMessage.UpdateType.CLOSE)
        {
            Visible = false;
        }
    }


    private void OnInputWindowEvent(bool confirmed)
    {
        
    }

    private void OnCancelPressed()
    {
        Visible = false;
    }
    
    private void OnConfirmPressed()
    {
        Visible = false;
    }
    

    public bool Handle(CharacterInventory inventory, int slot)
    {
        if (!Visible)
        {
            return false;
        }

        return true;
    }
}