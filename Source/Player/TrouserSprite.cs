using y1000.Source.Animation;

namespace y1000.Source.Player;

public partial class TrouserSprite : AbstractDyablePartSprite
{
    protected override OffsetTexture? OffsetTexture => GetParent<IPlayerAnimation>().TrouserTexture;
}