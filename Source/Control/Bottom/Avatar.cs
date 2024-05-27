using Godot;
using y1000.Source.Animation;
using y1000.Source.Character;
using y1000.Source.Creature;
using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Control.Bottom;

public partial class Avatar : NinePatchRect
{
	private readonly ISpriteRepository _spriteRepository;
	private TextureRect _body;
	private Vector2 _bodyOffset;
	private TextureRect _hand;
	private static readonly TextureRect MAKE_COMPILER_HAPPY = new();
	
	private const int AvatarIndex = 57;

	public Avatar()
	{
		_spriteRepository = FilesystemSpriteRepository.Instance;
		_body = MAKE_COMPILER_HAPPY;
		_hand = MAKE_COMPILER_HAPPY;
	}

	public override void _Ready()
	{
		_body = GetNode<TextureRect>("Body");
		_hand = GetNode<TextureRect>("Hand");
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
	}

	public void DrawWeapon(CharacterWeapon? weapon)
	{
		if (weapon == null)
		{
			return;
		}
		var sprite = _spriteRepository.LoadByName(weapon.NonAttackAnimation);
		var offsetTexture = sprite.Get(AvatarIndex);
		_hand.Position = _bodyOffset + offsetTexture.Offset;
		//_hand.Position = offsetTexture.Offset;
		_hand.Texture = offsetTexture.Texture;
	}
}
