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
    
    public override void _Ready()
    {
        base._Ready();
        ForeachSlot(view => view.OnInputEvent += OnSlotEvent);
        _textLabel = GetNode<Label>("TextLabel");
    }

    private void OnSlotEvent(object? sender, SlotEvent @event)
    {
        if (sender is not InventorySlotView slot)
        {
            return;
        }
        var type = @event.EventType;
        if (type == SlotEvent.Type.MOUSE_ENTERED)
        {
            OnMouseEntered(slot);
        }
        else if (type == SlotEvent.Type.MOUSE_GONE)
        {
            OnMouseGone();
        }
        else if (type == SlotEvent.Type.MOUSE_LEFT_RELEASE)
        {
            OnMouseLeftRelease(slot);
        }
        else if (type == SlotEvent.Type.MOUSE_LEFT_DOUBLE_CLICK)
        {
            _inventory.OnUIDoubleClick(slot.Number);
        }
    }

    private string Format(ICharacterItem item)
    {
        if (item is CharacterStackItem stackItem)
        {
            return item.ItemName + ":" + stackItem.Number;
        }

        return item.ItemName;
    }

    private void OnMouseEntered(InventorySlotView slot)
    {
        _currentFocused = slot;
        var item = _inventory.Find(slot.Number);
        if (item != null && _textLabel != null)
        {
            _textLabel.Text = Format(item);
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
        if (picked.Number == _currentFocused.Number)
        {
            return;
        }
        _inventory.OnUISwap(picked.Number, _currentFocused.Number);
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
            ForeachSlot(slot=>slot.ClearTexture());
            inventory.Foreach(SetIconToSlot);
        }
    }
    private void SetIconToSlot(int slot, ICharacterItem item)
    {
        var texture = TEXTURE_READER.Get(item.IconId);
        if (texture != null)
        {
            GetNode<InventorySlotView>("Slots/Slot" + slot).PutItem(texture);
        }
    }
    public void ButtonClicked()
    {
        Visible = !Visible;
    }
}