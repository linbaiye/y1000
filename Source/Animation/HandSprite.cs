using y1000.Source.Player;

namespace y1000.Source.Animation;

public partial class HandSprite : AbstractPartSprite
{
    protected override OffsetTexture? OffsetTexture => GetParent<IPlayer>().HandTexture;
}