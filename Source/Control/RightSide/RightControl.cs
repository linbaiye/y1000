using System;
using Godot;
using y1000.Source.Assistant;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Control.Assistance;
using y1000.Source.Control.RightSide.Book;
using y1000.Source.Control.RightSide.Inventory;

namespace y1000.Source.Control.RightSide;

public partial class RightControl : Godot.Control
{
    private InventoryView? _inventory;
    private KungFuBookView? _kungFuBookView;

    private Hotkeys? _hotkeys;

    private AssistantView _assistantView;
    
    public override void _Ready()
    {
        _inventory = GetNode<InventoryView>("Inventory");
        _kungFuBookView = GetNode<KungFuBookView>("KungFuBook");
        _assistantView = GetNode<AssistantView>("Assistance");
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
        _assistantView.OnClosed();
        _inventory?.ButtonClicked();
    }
    
    public void OnKungFuButtonClicked()
    {
        _inventory?.OnClosed();
        _assistantView.OnClosed();
        _kungFuBookView?.ButtonClicked();
    }
    
    public void OnAssistantClicked()
    {
        _inventory?.OnClosed();
        _kungFuBookView?.OnClosed();
        _assistantView.OnAssistantButtonClicked();
    }

    
    public void BindCharacter(CharacterImpl character, AutoFillAssistant assistant,
        AutoLootAssistant? autoLootAssistant, Hotkeys hotkeys)
    {
        _inventory?.BindInventory(character.Inventory, hotkeys);
        _kungFuBookView?.BindKungFuBook(character.KungFuBook, hotkeys);
        character.WhenCharacterUpdated += WhenCharacterUpdated;
        _assistantView.BindCharacter(character, assistant, autoLootAssistant);
    }
}