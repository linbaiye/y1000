using y1000.Source.Animation;

namespace y1000.Source.Player;

public partial class ChestSprite : AbstractDyablePartSprite
{
    protected override OffsetTexture? OffsetTexture => GetParent<IPlayerAnimation>().ChestTexture;
}