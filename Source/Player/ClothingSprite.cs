using y1000.Source.Animation;

namespace y1000.Source.Player;

public partial class ClothingSprite : AbstractDyablePartSprite
{
    protected override OffsetTexture? OffsetTexture => GetParent<IPlayerAnimation>().ClothingTexture;
    
}