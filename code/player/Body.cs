using Godot;
using System;
using System.Collections.Generic;
namespace y1000.code.player;
public partial class Body : AbstractBodyPart
{
    protected override OffsetTexture PositionedTexture => GetParent<AbstractPlayer>().BodyTexture;

}