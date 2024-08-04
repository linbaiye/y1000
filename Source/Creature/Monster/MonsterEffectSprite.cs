using Godot;
using y1000.Source.Animation;

namespace y1000.Source.Creature.Monster;

public partial class MonsterEffectSprite  : Sprite2D
{
    public override void _Process(double delta)
    {
        OffsetTexture? offsetTexture = GetParent<Monster>().EffectOffsetTexture;
        if (offsetTexture == null)
        {
            return;
        }
        Texture = offsetTexture.Texture;
        Offset = offsetTexture.Offset;
    }
}