using Godot;
using y1000.Source.Character.Event;

namespace y1000.Source.Control.Bottom;

public partial class BottomControl : Godot.Control
{

    private void WhenCharacterUpdated(object? sender, AbstractCharacterEventArgs eventArgs)
    {
        if (sender is Character.Character character)
        {
            UpdateCoordinate(character);
        }
    }

    private void UpdateCoordinate(Character.Character character)
    {
        var coordinate = character.Coordinate;
        var label = GetNode<Label>("Container/Control/FirstArea/Coordinate");
        if (label != null)
        {
            label.Text = coordinate.X + "." + coordinate.Y;
        }
    }

    public void BindCharacter(Character.Character character)
    {
        character.WhenCharacterUpdated += WhenCharacterUpdated;
        UpdateCoordinate(character);
    }
    
}