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
	private TextureRect _body;
	private Vector2 _bodyOffset;
	private TextureRect _hand;
	private TextureRect _chest;
	private TextureRect _hair;
	private TextureRect _hat;
	private TextureRect _wrist;
	private TextureRect _boot;
	private TextureRect _clothing;
	private TextureRect _trouser;
	private static readonly TextureRect MAKE_COMPILER_HAPPY = new();

	private Action<EquipmentType>? _doubleClickHandler;
	
	private const int AvatarIndex = 57;

	public Avatar()
	{
		_spriteRepository = FilesystemSpriteRepository.Instance;
		_body = MAKE_COMPILER_HAPPY;
		_hand = MAKE_COMPILER_HAPPY;
		_chest = MAKE_COMPILER_HAPPY;
		_hair = MAKE_COMPILER_HAPPY;
		_hair = MAKE_COMPILER_HAPPY;
		_wrist = MAKE_COMPILER_HAPPY;
		_boot = MAKE_COMPILER_HAPPY;
		_clothing = MAKE_COMPILER_HAPPY;
		_trouser = MAKE_COMPILER_HAPPY;
	}

	public override void _Ready()
	{
		_body = GetNode<TextureRect>("Body");
		_hand = GetNode<TextureRect>("Hand");
		_chest = GetNode<TextureRect>("Chest");
		_hair = GetNode<TextureRect>("Hair");
		_hat = GetNode<TextureRect>("Hat");
		_boot = GetNode<TextureRect>("Boot");
		_wrist = GetNode<TextureRect>("Wrist");
		_clothing = GetNode<TextureRect>("Clothing");
		_trouser = GetNode<TextureRect>("Trouser");
	}


	private void DrawBody(bool male)
	{
		var atzSprite = _spriteRepository.LoadByName(male ? "N00" : "A00");
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

	public void DrawWeapon(PlayerWeapon ? weapon)
	{
		if (weapon == null)
		{
			return;
		}
		DrawArmor(weapon.NonAttackAnimation, _hand);
	}

	public void DrawChest(PlayerChest? chest)
	{
		DrawArmor(chest, _chest);
	}
	
	public void DrawHair(PlayerHair? hair)
	{
		DrawArmor(hair, _hair);
	}
	
	public void DrawHat(PlayerHat? hat)
	{
		DrawArmor(hat, _hat);
	}
	
	public void DrawWrist(Wrist? wrist)
	{
		DrawArmor(wrist, _wrist);
	}
	
	public void DrawBoot(Boot? boot)
	{
		DrawArmor(boot, _boot);
	}
	
	public void DrawTrouser(Trouser? trouser)
	{
		DrawArmor(trouser, _trouser);
	}
	
	public void DrawClothing(Clothing? clothing)
	{
		DrawArmor(clothing, _clothing);
	}

	private void DrawArmor(string name, TextureRect rect)
	{
		var sprite = _spriteRepository.LoadByName(name);
		var offsetTexture = sprite.Get(AvatarIndex);
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

	private void DrawArmor(AbstractArmor? armor, TextureRect rect)
	{
		if (armor == null)
		{
			return;
		}
		DrawArmor(armor.FirstAtzName, rect);
	}
}
