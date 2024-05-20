using y1000.Source.Animation;

namespace y1000.Source.Player;

public partial class HandSprite : AbstractBodyPart
{
    protected override OffsetTexture OffsetTexture => GetParent<IPlayer>().HandTexture;
}