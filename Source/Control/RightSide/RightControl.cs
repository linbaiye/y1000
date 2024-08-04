using System;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Control.RightSide.Book;
using y1000.Source.Control.RightSide.Inventory;

namespace y1000.Source.Control.RightSide;

public partial class RightControl : Godot.Control
{
    private InventoryView? _inventory;
    private KungFuBookView? _kungFuBookView;
    
    public override void _Ready()
    {
        _inventory = GetNode<InventoryView>("Inventory");
        _kungFuBookView = GetNode<KungFuBookView>("KungFuBook");
    }
    
    private void WhenCharacterUpdated(object? sender, EventArgs args)
    {
        if (sender is not CharacterImpl)
        {
            return;
        }
        switch (args)
        {
            case GainExpEventArgs expEventArgs: if (expEventArgs.IsKungFu) _kungFuBookView?.RefreshPage();
                break;
            case LearnKungFuEventArgs: _kungFuBookView?.RefreshPage();
                break;
        }
    }

    public InventoryView? InventoryView => _inventory;
    
    public KungFuBookView? KungFuBookView => _kungFuBookView;

    public void OnInventoryButtonClicked()
    {
        _kungFuBookView?.OnClosed();
        _inventory?.ButtonClicked();
    }
    
    public void OnKungFuButtonClicked()
    {
        _inventory?.OnClosed();
        _kungFuBookView?.ButtonClicked();
    }

    
    public void BindCharacter(CharacterImpl character)
    {
        _inventory?.BindInventory(character.Inventory);
        _kungFuBookView?.BindKungFuBook(character.KungFuBook);
        character.WhenCharacterUpdated += WhenCharacterUpdated;
    }
}