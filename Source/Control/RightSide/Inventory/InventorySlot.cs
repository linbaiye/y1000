using Godot;

namespace y1000.Source.Control.RightSide.Inventory;

public partial class InventorySlot : Panel
{
    public void PutItem(Texture2D texture2D)
    {
        GetNode<TextureRect>("CenterContainer/TextureRect").Texture = texture2D;
    }
}