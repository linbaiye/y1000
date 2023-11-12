using Godot;
using System;
using System.Threading;
using y1000.code.creatures;
using y1000.code.player;

public partial class BodySprite : AbstractCreatureBodySprite
{
    protected override PositionedTexture GetPositionedTexture()
    {
		var parent = GetParent<AbstractCreature>();
		var texture = parent.BodyTexture;
		return new PositionedTexture(texture.Texture, texture.Offset + new Vector2(16, -12));
    }

}
