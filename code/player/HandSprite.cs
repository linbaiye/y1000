using Godot;
using System;
using y1000.code.player;

public partial class HandSprite : AbstractBodyPart
{
    protected override PositionedTexture PositionedTexture => GetParent<Character>().BodyTexture;

}
