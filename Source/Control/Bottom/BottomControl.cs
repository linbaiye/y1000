using System;
using Godot;
using y1000.Source.Character.Event;

namespace y1000.Source.Control.Bottom;

public partial class BottomControl : Godot.Control
{
    private void WhenCoordinateUpdated(object? sender, EventArgs args)
    {
        if (sender is Character.CharacterImpl && args is CharacterMoveEventArgs eventArgs)
        {
            UpdateCoordinate(eventArgs.Coordinate);
        }
    }

    private TextArea? _textArea;

    public override void _Ready()
    {
        _textArea = GetNode<TextArea>("Container/TextArea");
    }

    private void UpdateCoordinate(Vector2I coordinate)
    {
        var label = GetNode<Label>("Container/Coordinate");
        if (label != null)
        {
            label.Text = coordinate.X + "." + coordinate.Y;
        }
    }

    public void DisplayMessage(TextEvent @event)
    {
        _textArea?.Display(@event);
    }
    
    public void DisplayMessage(string message)
    {
        _textArea?.Display(new TextEvent(message));
    }

    public void BindCharacter(Character.CharacterImpl character)
    {
        character.WhenCharacterUpdated += WhenCoordinateUpdated;
        UpdateCoordinate(character.Coordinate);
    }


    public TextureButton InventoryButton => GetNode<TextureButton>("Container/InventoryButton");


}