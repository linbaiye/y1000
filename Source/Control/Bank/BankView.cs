using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Control.Bank;

public partial class BankView : NinePatchRect
{
    private BankSlots _slots;
    
    private InventoryView? _inventoryView;
    
    private EventMediator _eventMediator;
    private TextureButton _confirmButton;
    private CharacterInventory? _inventory;
    private CharacterBank? _bank;
    private Label _textLabel;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private InventorySlotView? _currentFocusedSlot;
    private TradeInputWindow _tradeInputWindow;
        

    public void Initialize(EventMediator eventMediator, TradeInputWindow tradeInputWindow)
    {
        _eventMediator = eventMediator;
        _tradeInputWindow = tradeInputWindow;
    }

    private enum TxType
    {
        INV_TO_BANK,
        BANK_TO_INV,
    }
    private class TradeWindowContext
    {
        public TradeWindowContext(TxType type, int fromSlot, int toSlot)
        {
            Type = type;
            FromSlot = fromSlot;
            ToSlot = toSlot;
        }

        public TxType Type { get; }
        public int FromSlot { get; }
        public int ToSlot { get; }

        public static TradeWindowContext BankToInv(int from, int to)
        {
            return new TradeWindowContext(TxType.BANK_TO_INV, from, to);
        }

        public static TradeWindowContext InvToBank(int from, int to)
        {
            return new TradeWindowContext(TxType.INV_TO_BANK, from, to);
        }
        
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
        _eventMediator.NotifyServer(ClientOperateBankEvent.Close());
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
                _textLabel.Text = item.ToString();
        }
        else if (mouseEvent.EventType == SlotMouseEvent.Type.MOUSE_GONE)
        {
            _currentFocusedSlot = null;
            _textLabel.Text = "";
        }
        else if (mouseEvent.EventType == SlotMouseEvent.Type.MOUSE_LEFT_RELEASE)
        {
            OnBankSlotDragEvent(slot);
            Logger.Debug("Released for slot {0}.", slot.Number);
        }
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
        _inventoryView = inventoryView;
    }


    private void OnBankSlotDragEvent(InventorySlotView bankSlot)
    {
        if (_inventoryView == null || _inventoryView.FocusedSlotView == null || _bank == null || 
            _inventory == null)
        {
            return;
        }
        var item = _bank.Get(bankSlot.Number);
        if (item == null)
        {
            return;
        }
        var invFocusedSlot = _inventoryView.FocusedSlotView;
        if (!_inventory.CanPut(invFocusedSlot.Number, item))
        {
            return;
        }
        if (item is CharacterStackItem stackItem)
            _tradeInputWindow.Open(new TradeInputWindow.InventoryTradeItem(item.ItemName, 0, TradeWindowContext.BankToInv(bankSlot.Number, invFocusedSlot.Number)), 
                stackItem.Number, OnInputWindowClicked);
        else
            _eventMediator.NotifyServer(ClientOperateBankEvent.BankToInventory(bankSlot.Number, invFocusedSlot.Number, 1));
    }

    public void Operate(BankOperationMessage message)
    {
        if (!Visible || _bank == null)
        {
            return;
        }
        if (message.IsClose)
        {
            Close();
        } 
        else if (message.IsUpdate)
        {
            _bank.Update(message.SlotId, message.Item);
            _slots.Clear();
            _slots.Display(_bank);
        }
    }


    private void Close()
    {
        _bank = null;
        _inventory = null;
        _inventoryView?.DisableBankMode();
        Visible = false;
    }

    private void OnInputWindowClicked(bool confirm)
    {
        if (!confirm || _tradeInputWindow.TradeItem == null)
            return;
        var context = _tradeInputWindow.TradeItem.ExtData<TradeWindowContext>();
        if (context == null)
        {
            return;
        }
        if (context.Type == TxType.INV_TO_BANK)
        {
            _eventMediator.NotifyServer(ClientOperateBankEvent.InventoryToBank(context.FromSlot, context.ToSlot, _tradeInputWindow.Number));
        }
        else if (context.Type == TxType.BANK_TO_INV)
        {
            _eventMediator.NotifyServer(ClientOperateBankEvent.BankToInventory(context.FromSlot, context.ToSlot, _tradeInputWindow.Number));
        }
    }

    private void OnInventorySlotDragEvent(InventorySlotView slot)
    {
        if (_inventory == null || _currentFocusedSlot == null || _bank == null)
        {
            return;
        }
        var item = _inventory.Get(slot.Number);
        if (item == null || !_bank.CanPut(_currentFocusedSlot.Number, item))
        {
            return;
        }
        if (item is CharacterStackItem stackItem)
            _tradeInputWindow.Open(new TradeInputWindow.InventoryTradeItem(item.ItemName, slot.Number, TradeWindowContext.InvToBank(slot.Number, _currentFocusedSlot.Number)), 
                stackItem.Number, OnInputWindowClicked);
        else
            _eventMediator.NotifyServer(ClientOperateBankEvent.InventoryToBank(slot.Number, _currentFocusedSlot.Number, 1));
        Logger.Debug("Drag inventory {0}.", slot.Number);
    }
    
}