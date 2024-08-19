using y1000.Source.Animation;

namespace y1000.Source.Player;

public partial class HatSprite : AbstractDyablePartSprite
{
	protected override OffsetTexture? OffsetTexture => GetParent<IPlayerAnimation>().HatTexture;
}
