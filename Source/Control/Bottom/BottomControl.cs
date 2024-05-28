using System;
using Godot;
using y1000.Source.Character;
using y1000.Source.Character.Event;

namespace y1000.Source.Control.Bottom;

public partial class BottomControl : Godot.Control
{
	private void WhenCoordinateUpdated(object? sender, EventArgs args)
	{
		if (sender is not CharacterImpl)
		{
			return;
		}
		switch (args)
		{
			case CharacterMoveEventArgs move: UpdateCoordinate(move.Coordinate);
				break;
			case WeaponChangedEvent weaponChangedEvent : _avatar?.DrawWeapon(weaponChangedEvent.Weapon);
				break;
		}
	}

	private TextArea? _textArea;

	private Avatar? _avatar;

	public override void _Ready()
	{
		_textArea = GetNode<TextArea>("Container/TextArea");
		_avatar = GetNode<Avatar>("Container/Avatar");
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

	public void BindCharacter(CharacterImpl character)
	{
		character.WhenCharacterUpdated += WhenCoordinateUpdated;
		_avatar?.BindCharacter(character);
		UpdateCoordinate(character.Coordinate);
	}


	public TextureButton InventoryButton => GetNode<TextureButton>("Container/InventoryButton");


}
