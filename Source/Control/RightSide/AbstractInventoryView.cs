using System;
using System.Collections.Generic;
using Godot;
using y1000.Source.Assistant;
using y1000.Source.Control.RightSide.Inventory;

namespace y1000.Source.Control.RightSide;

public abstract partial class AbstractInventoryView : NinePatchRect
{
    public override void _Ready()
    {
        Visible = false;
        GetNode<TextureButton>("CloseButton").Pressed += OnClosed;
        ToggleMouseFilter();
    }

    public void OnClosed()
    {
        Visible = false;
        ToggleMouseFilter();
    }

    protected void ToggleMouseFilter()
    {
        MouseFilter = Visible ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
    }

    protected void ForeachSlot(Action<InventorySlotView> action)
    {
        var container = GetNode<Godot.GridContainer>("Slots");
        foreach (var child in container.GetChildren())
        {
            if (child is InventorySlotView slot)
            {
                action.Invoke(slot);
            }
        }
    }
    
    protected void UpdateHotKeys(Hotkeys hotkeys, IEnumerable<ShortcutContext> contexts)
    {
        ForeachSlot(slot=>slot.ClearText());
        foreach (var context in contexts)
        {
            var keyString = hotkeys.GetKeyString(context.Key);
            GetNode<InventorySlotView>("Slots/Slot" + context.Slot).PutText(keyString);
        }
    }

    protected InventorySlotView GetSlot(int nr)
    {
        return GetNode<InventorySlotView>("Slots/Slot" + nr);
    }
    
}