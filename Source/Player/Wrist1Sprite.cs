using y1000.Source.Animation;

namespace y1000.Source.Player;

public partial class Wrist1Sprite : AbstractPartSprite
{
    protected override OffsetTexture? OffsetTexture => GetParent<IPlayerAnimation>().Wrist1Texture;
}