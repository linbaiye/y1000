using Godot;
using System;
using System.Collections.Generic;
namespace y1000.code.player;
public partial class Body : AbstractBodyPart
{
    protected override OffsetTexture OffsetTexture => GetParent<Player>().BodyTexture;


    public void OnBufaEnabled()
    {
    }
    public void OnBufaDisabled()
    {

    }
}