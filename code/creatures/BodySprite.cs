using Godot;
using System;
using System.Threading;
using y1000.code.creatures;
using y1000.code.player;

public partial class BodySprite : AbstractCreatureBodySprite
{
    protected override OffsetTexture GetPositionedTexture()
    {
		var parent = GetParent<AbstractCreature>();
		var texture = parent.BodyTexture;
		return new OffsetTexture(texture.Texture, texture.Offset + new Vector2(16, -12));
    }

}
