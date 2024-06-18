using System;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Event;
using y1000.Source.Util;

namespace y1000.Source.Control.Bottom;

public partial class BottomControl : Godot.Control
{

	private TextArea? _textArea;

	private Avatar? _avatar;

	private UsedKungFuView? _kungFuView;

	private TextureProgressBar? _lifeBar;
	
	private TextureProgressBar? _powerBar;
	
	private TextureProgressBar? _innerPowerBar;
	
	private TextureProgressBar? _outerPowerBar;

	public override void _Ready()
	{
		_textArea = GetNode<TextArea>("Container/TextArea");
		_avatar = GetNode<Avatar>("Container/Avatar");
		_kungFuView = GetNode<UsedKungFuView>("Container/UsedKungFuView");
		_lifeBar = GetNode<TextureProgressBar>("Container/LifeBar");
		_powerBar = GetNode<TextureProgressBar>("Container/PowerBar");
		_innerPowerBar = GetNode<TextureProgressBar>("Container/InnerPowerBar");
		_outerPowerBar = GetNode<TextureProgressBar>("Container/OuterPowerBar");
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
	
	private void WhenCharacterUpdated(object? sender, EventArgs args)
	{
		if (sender is not CharacterImpl character)
		{
			return;
		}
		switch (args)
		{
			case CharacterMoveEventArgs move: UpdateCoordinate(move.Coordinate);
				break;
			case WeaponChangedEvent weaponChangedEvent : _avatar?.DrawWeapon(weaponChangedEvent.Weapon);
				break;
			case EquipmentChangedEvent changedEvent : _avatar?.OnCharacterEquipmentChanged(character, changedEvent.EquipmentType);
				break;
			case KungFuChangedEvent : _kungFuView?.DisplayUsedKungFu(character);
				break;
			case PlayerAttributeEvent: BindAttributeBars(character);
				break;
			case GainExpEventArgs gainExp: _kungFuView?.BlinkGainExpKungFu(gainExp.Name);
				break;
		}
	}

	public void BindCharacter(CharacterImpl character)
	{
		character.WhenCharacterUpdated += WhenCharacterUpdated;
		_avatar?.BindCharacter(character);
		UpdateCoordinate(character.Coordinate);
		_kungFuView?.DisplayUsedKungFu(character);
		BindAttributeBars(character);
	}


	private void BindBar(TextureProgressBar? bar, ValueBar valueBar)
	{
		if (bar == null)
		{
			return;
		}

		bar.TooltipText = valueBar.Text;
		bar.Value = valueBar.Percent;
	}


	private void BindAttributeBars(CharacterImpl character)
	{
		BindBar(_lifeBar, character.HealthBar);
		BindBar(_powerBar, character.PowerBar);
		BindBar(_innerPowerBar, character.InnerPowerBar);
		BindBar(_outerPowerBar, character.OuterPowerBar);
	}

	public Button InventoryButton => GetNode<Button>("Container/InventoryButton");
	public Button KungFuButton => GetNode<Button>("Container/KungFuButton");

}
