using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Character;
using y1000.Source.Creature;
using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Control.Bottom;

public partial class Avatar : NinePatchRect
{
	private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
	private readonly ISpriteRepository _spriteRepository;
	private TextureRect _body;
	private Vector2 _bodyOffset;
	private TextureRect _hand;
	private TextureRect _chest;
	private TextureRect _hair;
	private TextureRect _hat;
	private TextureRect _wrist;
	private TextureRect _boot;
	private static readonly TextureRect MAKE_COMPILER_HAPPY = new();
	
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
		_chest.GuiInput += OnEvent;
	}

	public void OnEvent(InputEvent inputEvent)
	{
		Logger.Debug("On event {0}.", inputEvent);
	}
	

	private void DrawBody(bool male)
	{
		var atzSprite = _spriteRepository.LoadByName(male ? "N00" : "A00");
		var offsetTexture = atzSprite.Get(AvatarIndex);
		_body.Texture = offsetTexture.Texture;
		_body.Position = new Vector2((Size.X - offsetTexture.OriginalSize.X) / 2, (Size.Y - offsetTexture.OriginalSize.Y) / 2);
		_bodyOffset = _body.Position - offsetTexture.Offset;
	}

	public void DrawCharacter(CharacterImpl character)
	{
		DrawBody(character.IsMale);
		DrawWeapon(character.Weapon);
		DrawChest(character.Chest);
		DrawHair(character.Hair);
		DrawHat(character.Hat);
		DrawBoot(character.Boot);
		DrawWrist(character.Wrist);
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

	private void DrawArmor(string name, TextureRect rect)
	{
		var sprite = _spriteRepository.LoadByName(name);
		var offsetTexture = sprite.Get(AvatarIndex);
		rect.Size = new Vector2(0, 0);
		rect.Position = _bodyOffset + offsetTexture.Offset;
		rect.Texture = offsetTexture.Texture;
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
