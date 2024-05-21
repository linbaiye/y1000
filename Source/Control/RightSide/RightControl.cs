using y1000.Source.Character;
using y1000.Source.Control.RightSide.Inventory;

namespace y1000.Source.Control.RightSide;

public partial class RightControl : Godot.Control
{
    private InventoryView? _inventory;
    
    public override void _Ready()
    {
        _inventory = GetNode<InventoryView>("Inventory");
        BindButtons();
    }

    public void OnInventoryButtonClicked()
    {
        _inventory?.ButtonClicked();
    }

    private void BindButtons()
    {
    }
    
    public void BindCharacter(CharacterImpl character)
    {
        _inventory?.BindInventory(character.Inventory);
    }
}