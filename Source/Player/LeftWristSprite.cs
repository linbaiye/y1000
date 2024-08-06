using y1000.Source.Animation;

namespace y1000.Source.Player;

public partial class LeftWristSprite : AbstractPartSprite
{
	protected override OffsetTexture? OffsetTexture => GetParent<IPlayerAnimation>().LeftWristTexture;
}
