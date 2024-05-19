using System;
using Godot;
using y1000.Source.Character.Event;

namespace y1000.Source.Control.Bottom;

public partial class BottomControl : Godot.Control
{

    private void WhenCoordinateUpdated(object? sender, EventArgs args)
    {
        if (sender is Character.Character && args is CharacterMoveEventArgs eventArgs)
        {
            UpdateCoordinate(eventArgs.Coordinate);
        }
    }

    private void UpdateCoordinate(Vector2I coordinate)
    {
        var label = GetNode<Label>("Container/Control/FirstArea/Coordinate");
        if (label != null)
        {
            label.Text = coordinate.X + "." + coordinate.Y;
        }
    }

    public void BindCharacter(Character.Character character)
    {
        character.WhenCharacterUpdated += WhenCoordinateUpdated;
        UpdateCoordinate(character.Coordinate);
    }
    
}