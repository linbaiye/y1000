using System;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Item;

namespace y1000.Source.Control.RightSide.Inventory;

public partial class InventoryView : NinePatchRect
{
    
    private static readonly ItemTextureReader TEXTURE_READER = ItemTextureReader.LoadItems();
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    private InventorySlotView? _currentFocused;

    private CharacterInventory _inventory = CharacterInventory.Empty;

    private Label? _textLabel;
    
    public override void _Ready()
    {
        Visible = false;
        GetNode<TextureButton>("CloseButton").Pressed += OnClosed;
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
            return item.Name + ":" + stackItem.Number;
        }

        return item.Name;
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

    private void ForeachSlot(Action<InventorySlotView> action)
    {
        var container = GetNode<Godot.GridContainer>("InventorySlots");
        foreach (var child in container.GetChildren())
        {
            if (child is InventorySlotView slot)
            {
                action.Invoke(slot);
            }
        }
    }

    private void SetIconToSlot(int slot, ICharacterItem item)
    {
        var texture = TEXTURE_READER.Get(item.IconId);
        if (texture != null)
        {
            GetNode<InventorySlotView>("InventorySlots/Slot" + slot).PutItem(texture);
        }
    }


    public void ButtonClicked()
    {
        Visible = !Visible;
    }

    private void OnClosed()
    {
        Visible = false;
    }
}