using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class PlayerArmorAnimation : AbstractPlayerBodyAnimation<PlayerArmorAnimation>
{

    public static PlayerArmorAnimation Create(AbstractArmor armor)
    {
        PlayerArmorAnimation animation = new PlayerArmorAnimation();
        AtzSprite N0 = SpriteRepository.LoadByNameAndOffset(armor.FirstAtzName);
        AtzSprite N1 = SpriteRepository.LoadByNameAndOffset(armor.SecondAtzName);
        AtzSprite N2 = SpriteRepository.LoadByNameAndOffset(armor.ThirdAtzName);
        AtzSprite N3 = SpriteRepository.LoadByNameAndOffset(armor.FourthAtzName);
        AtzSprite N4 = SpriteRepository.LoadByNameAndOffset(armor.FifthAtzName);
        return Config(animation, N0, N1, N2, N3, N4);
    }
    
}