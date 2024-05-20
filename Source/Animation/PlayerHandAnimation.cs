using y1000.Source.Creature;

namespace y1000.Source.Animation;

public class PlayerHandAnimation: AbstractPlayerAnimation<PlayerHandAnimation>
{
    public static PlayerHandAnimation LoadSword(string name)
    {
        SpriteReader weaponSprites = SpriteReader.LoadOffsetWeaponSprites(name);
        var playerAnimation = new PlayerHandAnimation();
        return playerAnimation.ConfigureState(CreatureState.IDLE, AtdReader, weaponSprites)
            .ConfigureState(CreatureState.FLY, AtdReader, weaponSprites)
            .ConfigureState(CreatureState.WALK, AtdReader, weaponSprites)
            .ConfigureState(CreatureState.RUN, AtdReader, weaponSprites)
            .ConfigureState(CreatureState.ENFIGHT_WALK, AtdReader, weaponSprites)
            .ConfigureState(CreatureState.HURT, AtdReader, weaponSprites)
            .ConfigureState(CreatureState.COOLDOWN, AtdReader, weaponSprites)
            .ConfigureState(CreatureState.SWORD, AtdReader, weaponSprites)
            .ConfigureState(CreatureState.SWORD2H, AtdReader, weaponSprites)
            ;
    }
}