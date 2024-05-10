using Godot;
using System;
using System.Collections.Generic;
using y1000.Source.Animation;
using y1000.Source.Entity.Animation;

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