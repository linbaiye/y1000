using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class PlayerArmorAnimation : AbstractPlayerBodyAnimation<PlayerArmorAnimation>
{

    public static PlayerArmorAnimation Create(AbstractArmor armor)
    {
        PlayerArmorAnimation animation = new PlayerArmorAnimation();
        AtzSprite N0 = SpriteRepository.LoadByNumber(armor.FirstAtzName);
        AtzSprite N1 = SpriteRepository.LoadByNumber(armor.SecondAtzName);
        AtzSprite N2 = SpriteRepository.LoadByNumber(armor.ThirdAtzName);
        AtzSprite N3 = SpriteRepository.LoadByNumber(armor.FourthAtzName);
        AtzSprite N4 = SpriteRepository.LoadByNumber(armor.FifthAtzName);
        return Config(animation, N0, N1, N2, N3, N4);
    }
    
}