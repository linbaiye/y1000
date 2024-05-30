using System;
using Godot;
using y1000.Source.Control.RightSide.Inventory;

namespace y1000.Source.Control.RightSide;

public abstract partial class AbstractInventoryView : NinePatchRect
{
    public override void _Ready()
    {
        Visible = false;
        GetNode<TextureButton>("CloseButton").Pressed += OnClosed;
    }

    public void OnClosed()
    {
        Visible = false;
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

    protected InventorySlotView GetSlot(int nr)
    {
        return GetNode<InventorySlotView>("Slots/Slot" + nr);
    }
    
    public void ButtonClicked()
    {
        Visible = !Visible;
    }
    
}