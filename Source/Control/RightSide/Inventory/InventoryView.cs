using System.Collections.Generic;
using Godot;
using y1000.Source.Item;

namespace y1000.Source.Control.RightSide.Inventory;

public partial class InventoryView : NinePatchRect
{
    private static readonly ItemTextureReader TEXTURE_READER = ItemTextureReader.LoadItems();
    public override void _Ready()
    {
        Visible = false;
        GetNode<TextureButton>("CloseButton").Pressed += OnClosed;
        AddItems(new int[] {1, // 长剑
            1, //  长刀
            4, //  弓
            });
    }

    private void AddToSlot(int slot, Texture2D texture)
    {
        GetNode<InventorySlotView>("InventorySlots/Slot" + slot).PutItem(texture);
    }

    public void AddItems(IEnumerable<int> shapeIds)
    {
        int slot = 1;
        foreach (var shapeId in shapeIds)
        {
            var texture = TEXTURE_READER.Get(shapeId);
            if (texture != null)
            {
                GetNode<InventorySlotView>("InventorySlots/Slot" + slot).PutItem(texture);
                slot++;
            }
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