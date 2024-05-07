using Godot;
using y1000.code.player;
using y1000.Source.Entity.Animation;

namespace y1000.code.creatures;

public partial class BodySprite : AbstractCreatureBodySprite
{


	protected override OffsetTexture GetPositionedTexture()
	{
		var parent = GetParent<AbstractCreature>();
		var texture = parent.BodyTexture;
		return new OffsetTexture(texture.Texture, texture.Offset + new Vector2(16, -12));
	}

}