using Godot;
using y1000.Source.Animation;

namespace y1000.Source.Player;

public partial class AttackEffectSprite  : Sprite2D
{
    public override void _Process(double delta)
    {
        OffsetTexture? offsetTexture = GetParent<IPlayerAnimation>().AttackEffect;
        
        if (offsetTexture == null)
        {
            Texture = null;
        }
        else
        {
            Texture = offsetTexture.Texture;
            Offset = offsetTexture.Offset;
        }
    }
}