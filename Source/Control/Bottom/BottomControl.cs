using System;
using Godot;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Control.Bottom.Shortcut;
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
	
	private TextureProgressBar? _kungFuExpUpBar;
	
	private TextureProgressBar? _kungFuExpDownBar;
	
	private TextureProgressBar? _headLifeBar;
	private TextureProgressBar? _armLifeBar;
	private TextureProgressBar? _legLifeBar;

	private Shortcuts? _shortcuts;

	private InputEdit? _inputEdit;

	public override void _Ready()
	{
		_textArea = GetNode<TextArea>("Container/TextArea");
		_avatar = GetNode<Avatar>("Container/Avatar");
		_kungFuView = GetNode<UsedKungFuView>("Container/UsedKungFuView");
		_lifeBar = GetNode<TextureProgressBar>("Container/LifeBar");
		_powerBar = GetNode<TextureProgressBar>("Container/PowerBar");
		_innerPowerBar = GetNode<TextureProgressBar>("Container/InnerPowerBar");
		_outerPowerBar = GetNode<TextureProgressBar>("Container/OuterPowerBar");
		_kungFuExpUpBar = GetNode<TextureProgressBar>("Container/ExpUpBar");
		_kungFuExpDownBar = GetNode<TextureProgressBar>("Container/ExpDownBar");
		_headLifeBar = GetNode<TextureProgressBar>("Container/HeadLifeBar");
		_armLifeBar = GetNode<TextureProgressBar>("Container/ArmLifeBar");
		_legLifeBar = GetNode<TextureProgressBar>("Container/LegLifeBar");
		_shortcuts = GetNode<Shortcuts>("Container/Shortcuts");
		_inputEdit = GetNode<InputEdit>("Container/InputLine");
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
			case KungFuChangedEvent : OnKungFuChanged(character);
				break;
			case PlayerAttributeEvent: BindAttributeBars(character);
				break;
			case GainExpEventArgs gainExp: OnGainExp(character, gainExp);
				break;
			case CharacterTeleportedArgs teleportedArgs:
				UpdateCoordinate(teleportedArgs.Coordinate);
				UpdateRealmName(teleportedArgs.Realm);
				break;
		}
	}

	private void UpdateRealmName(string name)
	{
		
		var label = GetNode<Label>("Container/RealmName");
		if (label != null)
		{
			label.Text = name;
		}
	}

	private void OnKungFuChanged(CharacterImpl character)
	{
		_kungFuView?.DisplayUsedKungFus(character);
		BindAttackKungFuExpBars(character);
	}

	private void OnGainExp(CharacterImpl character, GainExpEventArgs gainExp)
	{
		if (gainExp.IsKungFu)
		{
			BindAttackKungFuExpBars(character);
			_kungFuView?.BlinkGainExpKungFu(gainExp.Name); 
		}
		else
		{
			_avatar?.BlinkText(gainExp.Name);
		}
	}

	public void BindCharacter(CharacterImpl character, string realmName)
	{
		character.WhenCharacterUpdated += WhenCharacterUpdated;
		_avatar?.BindCharacter(character);
		UpdateCoordinate(character.Coordinate);
		_kungFuView?.DisplayUsedKungFus(character);
		BindAttributeBars(character);
		BindAttackKungFuExpBars(character);
		UpdateRealmName(realmName);
		_shortcuts?.Bind(character);
		_inputEdit?.BindCharacter(character);
	}


	private void BindAttributeBar(TextureProgressBar? bar, ValueBar valueBar)
	{
		if (bar == null)
		{
			return;
		}

		bar.TooltipText = valueBar.Text;
		bar.Value = valueBar.Percent;
	}

	private void BindPercentLifeBar(TextureProgressBar? bar, int percent)
	{
		if (bar == null)
		{
			return;
		}
		bar.Value = percent;
		bar.TooltipText = percent.ToString();
	}


	private void BindAttributeBars(CharacterImpl character)
	{
		BindAttributeBar(_lifeBar, character.HealthBar);
		BindAttributeBar(_powerBar, character.PowerBar);
		BindAttributeBar(_innerPowerBar, character.InnerPowerBar);
		BindAttributeBar(_outerPowerBar, character.OuterPowerBar);
		BindPercentLifeBar(_headLifeBar, character.HeadPercent);
		BindPercentLifeBar(_armLifeBar, character.ArmPercent);
		BindPercentLifeBar(_legLifeBar, character.LegPercent);
	}

	private void BindAttackKungFuExpBars(CharacterImpl character)
	{
		var up = character.AttackKungFu.Level % 100;
		if (_kungFuExpUpBar != null)
			_kungFuExpUpBar.Value = up;
		var down = character.AttackKungFu.Level / 100;
		if (_kungFuExpDownBar != null)
			_kungFuExpDownBar.Value = down;
	}

	public Button InventoryButton => GetNode<Button>("Container/InventoryButton");
	public Button KungFuButton => GetNode<Button>("Container/KungFuButton");
	public Button AssistantButton => GetNode<Button>("Container/AssistantButton");

}
