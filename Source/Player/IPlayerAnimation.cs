using y1000.Source.Animation;

namespace y1000.Source.Player;

public interface IPlayerAnimation 
{
    OffsetTexture? HandTexture { get; }
    
    OffsetTexture? ChestTexture { get; }
    
    OffsetTexture? HairTexture { get; }
    
    OffsetTexture? HatTexture { get; }
    
    OffsetTexture? WristTexture { get; }
    
    OffsetTexture? BootTexture { get; }
    
    OffsetTexture? ClothingTexture { get; }
    
    OffsetTexture? TrouserTexture { get; }
}