using System;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Control.Bottom;

public partial class Avatar : NinePatchRect
{
	private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();
	private readonly ISpriteRepository _spriteRepository;
	private AvatarPart _body;
	private Vector2 _bodyOffset;
	private AvatarPart _hand;
	private AvatarPart _chest;
	private AvatarPart _hair;
	private AvatarPart _hat;
	private AvatarPart _wrist;
	private AvatarPart _boot;
	private AvatarPart _clothing;
	private AvatarPart _trouser;
	private Label _equipmentText;
	private BlinkingText? _blinkingText;
	private static readonly AvatarPart MAKE_COMPILER_HAPPY = new();

	private Action<EquipmentType>? _doubleClickHandler;
	
	private const int AvatarIndex = 57;

	public Avatar()
	{
		_spriteRepository = FilesystemSpriteRepository.Instance;
		_body = MAKE_COMPILER_HAPPY;
		_hand = MAKE_COMPILER_HAPPY;
		_chest = MAKE_COMPILER_HAPPY;
		_hair = MAKE_COMPILER_HAPPY;
		_hat = MAKE_COMPILER_HAPPY;
		_wrist = MAKE_COMPILER_HAPPY;
		_boot = MAKE_COMPILER_HAPPY;
		_clothing = MAKE_COMPILER_HAPPY;
		_trouser = MAKE_COMPILER_HAPPY;
	}

	public override void _Ready()
	{
		_body = GetNode<AvatarPart>("Body");
		_hand = GetNode<AvatarPart>("Hand");
		_chest = GetNode<AvatarPart>("Chest");
		_hair = GetNode<AvatarPart>("Hair");
		_hat = GetNode<AvatarPart>("Hat");
		_boot = GetNode<AvatarPart>("Boot");
		_wrist = GetNode<AvatarPart>("Wrist");
		_clothing = GetNode<AvatarPart>("Clothing");
		_trouser = GetNode<AvatarPart>("Trouser");
		_equipmentText = GetNode<Label>("EquipmentText");
		_equipmentText.Visible = false;
		_blinkingText = GetNode<BlinkingText>("BlinkingText");
	}


	private void DrawBody(bool male)
	{
		var atzSprite = _spriteRepository.LoadByNumberAndOffset(male ? "N00" : "A00");
		var offsetTexture = atzSprite.Get(AvatarIndex);
		_body.Texture = offsetTexture.Texture;
		_body.Position = new Vector2((Size.X - offsetTexture.OriginalSize.X) / 2, (Size.Y - offsetTexture.OriginalSize.Y) / 2);
		_bodyOffset = _body.Position - offsetTexture.Offset;
	}

	public void BindCharacter(CharacterImpl character)
	{
		DrawBody(character.IsMale);
		DrawWeapon(character.Weapon);
		DrawChest(character.Chest);
		DrawHair(character.Hair);
		DrawHat(character.Hat);
		DrawBoot(character.Boot);
		DrawWrist(character.Wrist);
		DrawClothing(character.Clothing);
		DrawTrouser(character.Trouser);
		_doubleClickHandler = character.OnAvatarDoubleClick;
	}

	public void BlinkText(string text)
	{
		_blinkingText?.Blink(text);
	}

	public void OnCharacterEquipmentChanged(CharacterImpl character, EquipmentType changedType)
	{
		switch (changedType)
		{
			case EquipmentType.BOOT: DrawBoot(character.Boot);
				break;
			case EquipmentType.HAT: DrawHat(character.Hat);
				break;
			case EquipmentType.CHEST: DrawChest(character.Chest);
				break;
			case EquipmentType.CLOTHING: DrawClothing(character.Clothing);
				break;
			case EquipmentType.WRIST: DrawWrist(character.Wrist);
				break;
			case EquipmentType.HAIR: DrawHair(character.Hair);
				break;
			case EquipmentType.TROUSER: DrawTrouser(character.Trouser);
				break;
			case EquipmentType.WEAPON: DrawWeapon(character.Weapon);
				break;
		}
	}

	public void DrawWeapon(PlayerWeapon ? weapon)
	{
		if (weapon == null)
		{
			_hand.Visible = false;
			return;
		}
		DrawEquipment(weapon.NonAttackAnimation, _hand);
		_hand.Text = weapon.Name;
	}

	public void DrawChest(PlayerChest? chest)
	{
		if (chest == null)
		{
			_chest.Visible = false;
		}
		DrawArmor(chest, _chest);
	}
	
	public void DrawHair(PlayerHair? hair)
	{
		if (hair == null)
		{
			_hair.Visible = false;
		}
		DrawArmor(hair, _hair);
	}
	
	public void DrawHat(PlayerHat? hat)
	{
		if (hat == null)
		{
			_hat.Visible = false;
		}
		DrawArmor(hat, _hat);
	}
	
	public void DrawWrist(Wrist? wrist)
	{
		if (wrist == null)
		{
			_wrist.Visible = false;
		}
		DrawArmor(wrist, _wrist);
	}
	
	public void DrawBoot(Boot? boot)
	{
		if (boot == null)
		{
			_boot.Visible = false;
		}
		DrawArmor(boot, _boot);
	}
	
	public void DrawTrouser(Trouser? trouser)
	{
		if (trouser == null)
		{
			_trouser.Visible = false;
		}
		DrawArmor(trouser, _trouser);
	}
	
	public void DrawClothing(Clothing? clothing)
	{
		if (clothing == null)
		{
			_clothing.Visible = false;
		}
		DrawArmor(clothing, _clothing);
	}

	private void DrawEquipment(string spriteName, AvatarPart rect)
	{
		var sprite = _spriteRepository.LoadByNumberAndOffset(spriteName);
		var offsetTexture = sprite.Get(AvatarIndex);
		rect.Visible = true;
		rect.Size = new Vector2(0, 0);
		rect.Position = _bodyOffset + offsetTexture.Offset;
		rect.Texture = offsetTexture.Texture;
	}

	public void OnPartClicked(AvatarPart part, string name)
	{
		switch (name)
		{
			case "Chest":  _doubleClickHandler?.Invoke(EquipmentType.CHEST);
				break;
			case "Hat":  _doubleClickHandler?.Invoke(EquipmentType.HAT);
				break;
			case "Boot":  _doubleClickHandler?.Invoke(EquipmentType.BOOT);
				break;
			case "Hair":  _doubleClickHandler?.Invoke(EquipmentType.HAIR);
				break;
			case "Wrist":  _doubleClickHandler?.Invoke(EquipmentType.WRIST);
				break;
			case "Hand":  _doubleClickHandler?.Invoke(EquipmentType.WEAPON);
				break;
			case "Trouser":  _doubleClickHandler?.Invoke(EquipmentType.TROUSER);
				break;
			case "Clothing":  _doubleClickHandler?.Invoke(EquipmentType.CLOTHING);
				break;
		}
		LOG.Debug("Name {0} was clicked", name);
	}

	public void OnMouseEntered(AvatarPart part)
	{
		if (part.Text.StartsWith("男子") || part.Text.StartsWith("女子"))
		{
			_equipmentText.Text = part.Text[2..];
		} else {
			_equipmentText.Text = part.Text;
		}
		_equipmentText.Visible = true;
	}
	
	public void OnMouseExited(AvatarPart part)
	{
		_equipmentText.Text = "";
		_equipmentText.Visible = false;
	}

	private void DrawArmor(AbstractArmor? armor, AvatarPart rect)
	{
		if (armor == null)
		{
			return;
		}
		DrawEquipment(armor.FirstAtzName, rect);
		rect.Text = armor.Name;
	}
}
