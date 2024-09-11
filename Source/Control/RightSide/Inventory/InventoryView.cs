using System;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Control.RightSide.Inventory;

public partial class InventoryView : AbstractInventoryView
{

    private static readonly IconReader TEXTURE_READER = IconReader.ItemIconReader;
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    private InventorySlotView? _currentFocused;

    private CharacterInventory _inventory = CharacterInventory.Empty;

    private Label? _textLabel;

    private bool _bankMode;

    private Action<InventorySlotView>? _dragHandler;
    
    public override void _Ready()
    {
        base._Ready();
        ForeachSlot(view =>
        {
            view.OnMouseInputEvent += OnSlotEvent;
            view.OnKeyboardEvent += OnSlotKeyEvent;
        });
        _textLabel = GetNode<Label>("TextLabel");
        _bankMode = false;
        Visible = false;
    }

    public void EnableBankMode(Action<InventorySlotView> dragHandler)
    {
        _bankMode = true;
        _dragHandler = dragHandler;
    }

    public void DisableBankMode()
    {
        _bankMode = false;
    }

    private void OnBankModeSlotEvent(InventorySlotView slot, SlotMouseEvent mouseEvent)
    {
        var type = mouseEvent.EventType;
        if (type == SlotMouseEvent.Type.MOUSE_LEFT_RELEASE)
        {
            _dragHandler?.Invoke(slot);
        }
    }

    private void OnInventorySlotEvent(InventorySlotView slot, SlotMouseEvent mouseEvent)
    {
        var type = mouseEvent.EventType;
        if (type == SlotMouseEvent.Type.MOUSE_LEFT_RELEASE)
        {
            OnMouseLeftRelease(slot);
        }
        else if (type == SlotMouseEvent.Type.MOUSE_LEFT_DOUBLE_CLICK)
        {
            _inventory.OnUIDoubleClick(slot.Number);
        }
        else if (type == SlotMouseEvent.Type.MOUSE_RIGHT_CLICK)
        {
            _inventory.OnRightClick(slot.Number);
        }
    }
    

    private void OnSlotEvent(object? sender, SlotMouseEvent mouseEvent)
    {
        if (sender is not InventorySlotView slot)
        {
            return;
        }
        var type = mouseEvent.EventType;
        if (type == SlotMouseEvent.Type.MOUSE_ENTERED)
        {
            OnMouseEntered(slot);
        }
        else if (type == SlotMouseEvent.Type.MOUSE_GONE)
        {
            OnMouseGone();
        }
        else
        {
            if (_bankMode)
            {
                OnBankModeSlotEvent(slot, mouseEvent);
            }
            else
            {
                OnInventorySlotEvent(slot, mouseEvent);
            }
        }
    }
    
    private void OnSlotKeyEvent(object? sender, SlotKeyEvent keyEvent)
    {
        if (sender is InventorySlotView slotView)
            _inventory.OnViewKeyPressed(slotView.Number, keyEvent.Key);
    }



    private void OnMouseEntered(InventorySlotView slot)
    {
        _currentFocused = slot;
        var item = _inventory.Find(slot.Number);
        if (item != null && _textLabel != null)
        {
            _textLabel.Text = item.ToString();
        }
    }

    private void OnMouseGone()
    {
        _currentFocused = null;
        if (_textLabel != null)
        {
            _textLabel.Text = null;
        }
    }

    private void OnMouseLeftRelease(InventorySlotView picked)
    {
        if (_currentFocused == null)
        {
            _inventory.OnUIDragItem(picked.Number);
            return;
        }
        if (picked.Number != _currentFocused.Number)
        {
            _inventory.OnUISwap(picked.Number, _currentFocused.Number);
        }
    }
    
    public void BindInventory(CharacterInventory inventory)
    {
        inventory.Foreach(SetIconToSlot);
        inventory.InventoryChanged += OnInventoryUpdated;
        _inventory = inventory;
    }

    private void OnInventoryUpdated(object? sender, EventArgs eventArgs)
    {
        if (sender is CharacterInventory inventory)
        {
            ForeachSlot(slot=>slot.Clear());
            inventory.Foreach(SetIconToSlot);
        }
    }
    
    private void SetIconToSlot(int slot, IItem item)
    {
        var texture = TEXTURE_READER.Get(item.IconId);
        if (texture != null)
        {
            GetNode<InventorySlotView>("Slots/Slot" + slot).PutTexture(texture, item.Color);
        }
    }

    public InventorySlotView? FocusedSlotView => _currentFocused;
    
    public void ButtonClicked()
    {
        Visible = !Visible;
        ToggleMouseFilter();
    }
}