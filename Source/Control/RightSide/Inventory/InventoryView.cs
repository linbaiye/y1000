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

    private CharacterInventory _inventory = new();

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
            OnMouseLeft();
        }
        else if (type == SlotEvent.Type.MOUSE_LEFT_RELEASE)
        {
            SwapItem(slot);
        }
        LOGGER.Debug("Received event {0} from slot {1}.", type, slot.Number);
    }

    private void OnMouseEntered(InventorySlotView slot)
    {
        _currentFocused = slot;
        var item = _inventory.Find(slot.Number);
        if (item != null && _textLabel != null)
        {
            _textLabel.Text = item.Name;
        }
    }

    private void OnMouseLeft()
    {
        _currentFocused = null;
        if (_textLabel != null)
        {
            _textLabel.Text = null;
        }
    }

    private void SwapItem(InventorySlotView picked)
    {
        if (_currentFocused == null || picked.Number == _currentFocused.Number)
        {
            return;
        }
        LOGGER.Debug("Swap {0} and {1}.", picked.Number, _currentFocused.Number);
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