using Godot;
using System;
using y1000.code.creatures;
using y1000.code.player;

public partial class HandSprite : AbstractBodyPart
{
    protected override OffsetTexture OffsetTexture => GetParent<AbstractCreature>().BodyTexture;

}
