using Godot;
using System;
using System.Collections.Generic;
using y1000.code.player;
using y1000.code;
namespace y1000.code.player;
public partial class Body : AbstractBodyPart
{
    protected override PositionedTexture PositionedTexture => GetParent<Character>().BodyTexture;

}