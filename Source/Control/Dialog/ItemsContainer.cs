using System;
using System.Linq;
using Godot;
using y1000.Source.Control.RightSide;

namespace y1000.Source.Control.Dialog;

public partial class ItemsContainer : ScrollContainer
{

    private VBoxContainer _itemContainer;

    public event Action<Item>? ItemDoubleClicked;
    public override void _Ready()
    {
        _itemContainer = GetNode<VBoxContainer>("ItemContainer");
    }
    
    public void Clear()
    {
        foreach (var child in _itemContainer.GetChildren())
        {
            _itemContainer.RemoveChild(child);
        }
    }

    public void AddItem(string name, Texture2D icon, int iconColor, int price)
    {
        Item item = Item.Create();
        _itemContainer.AddChild(item);
        item.SetDetails(name, icon, iconColor, price);
        item.Clicked += ItemClicked;
    }

    public void DisableItem(string text)
    {
        
    }
    
    private void ItemClicked(object? sender, SlotMouseEvent eventArgs)
    {
        if (sender is not Item clicked)
        {
            return;
        }
        if (eventArgs.EventType == SlotMouseEvent.Type.MOUSE_LEFT_CLICK)
        {
            foreach (var item in _itemContainer.GetChildren().OfType<Item>())
            {
                item.ToggleHighlight(false);
            }
            clicked.ToggleHighlight(true);
        }
        else
        {
            ItemDoubleClicked?.Invoke(clicked);
        }
    }
}