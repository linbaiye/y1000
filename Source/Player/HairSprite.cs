using y1000.Source.Animation;

namespace y1000.Source.Player;

public partial class HairSprite  : AbstractDyablePartSprite
{
	protected override OffsetTexture? OffsetTexture => GetParent<IPlayerAnimation>().HairTexture;
	
}
