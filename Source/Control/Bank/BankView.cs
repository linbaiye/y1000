using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Networking;

namespace y1000.Source.Control.Bank;

public partial class BankView : NinePatchRect
{
    private BankSlots _slots;
    
    private InventoryView? _inventoryView;
    
    private EventMediator? _eventMediator;
    private TextureButton _confirmButton;
    private CharacterInventory? _inventory;
    private CharacterBank? _bank;
    private Label _textLabel;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private InventorySlotView? _currentFocusedSlot;
    private TradeInputWindow? _tradeInputWindow;
        

    public void Initialize(EventMediator eventMediator, TradeInputWindow tradeInputWindow)
    {
        _eventMediator = eventMediator;
        _tradeInputWindow = tradeInputWindow;
    }
    
    public override void _Ready()
    {
        _slots = GetNode<BankSlots>("Slots");
        _slots.Foreach(s => s.OnMouseInputEvent += OnSlotEvent);
        _confirmButton = GetNode<TextureButton>("ConfirmButton");
        _confirmButton.Pressed += OnConfirmed;
        _textLabel = GetNode<Label>("TextLabel");
    }

    private void OnConfirmed()
    {
        Close();
    }

    private void OnSlotEvent(object? sender, SlotMouseEvent mouseEvent)
    {
        if (sender is not InventorySlotView slot)
        {
            return;
        }
        if (mouseEvent.EventType == SlotMouseEvent.Type.MOUSE_ENTERED)
        {
            _currentFocusedSlot = slot;
            var item = _bank?.Get(slot.Number);
            if (item != null)
                _textLabel.Text = item.ItemName;
        }
        else if (mouseEvent.EventType == SlotMouseEvent.Type.MOUSE_GONE)
        {
            _currentFocusedSlot = null;
            _textLabel.Text = "";
        }
        else if (mouseEvent.EventType == SlotMouseEvent.Type.MOUSE_LEFT_RELEASE)
        {
            Logger.Debug("Released for slot {0}.", slot.Number);
        }
        Logger.Debug("Event {0}.", mouseEvent.EventType);
    }

    public void Open(CharacterBank bank, CharacterInventory inventory, InventoryView? inventoryView)
    {
        if (inventoryView == null)
        {
            return;
        }
        _slots.Clear();
        _slots.Display(bank);
        _bank = bank;
        _inventory = inventory;
        inventoryView.EnableBankMode(OnInventorySlotDragEvent);
        Visible = true;
    }

    private void OnMouseEntered()
    {
        //var item = _bank.Get();
    }

    public void Close()
    {
        _bank = null;
        _inventory = null;
        _inventoryView?.DisableBankMode();
        Visible = false;
    }

    private void OnInventorySlotDragEvent(InventorySlotView slot)
    {
        if (_inventory == null || _currentFocusedSlot == null)
        {
            return;
        }
        var item = _inventory.Get(slot.Number);
        if (item is CharacterStackItem)
        {
        }
        Logger.Debug("Drag inventory {0}.", slot.Number);
    }
    
}