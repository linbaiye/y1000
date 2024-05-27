using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class PlayerArmorAnimation : AbstractPlayerBodyAnimation<PlayerArmorAnimation>
{

    public static PlayerArmorAnimation Create(PlayerChest chest)
    {
        PlayerArmorAnimation animation = new PlayerArmorAnimation();
        AtzSprite N0 = SpriteRepository.LoadByName(chest.First);
        AtzSprite N1 = SpriteRepository.LoadByName(chest.Second);
        AtzSprite N2 = SpriteRepository.LoadByName(chest.Third);
        AtzSprite N3 = SpriteRepository.LoadByName(chest.Fourth);
        AtzSprite N4 = SpriteRepository.LoadByName(chest.Fifth);
        return Config(animation, N0, N1, N2, N3, N4);
    }
    
}