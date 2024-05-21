using y1000.Source.Creature;

namespace y1000.Source.Animation;

public class PlayerHandAnimation: AbstractPlayerAnimation<PlayerHandAnimation>
{

    private PlayerHandAnimation ConfigureNoneAttack(SpriteReader nonattack)
    {
        return ConfigureState(CreatureState.FLY, AtdReader, nonattack)
            .ConfigureState(CreatureState.WALK, AtdReader, nonattack)
            .ConfigureState(CreatureState.RUN, AtdReader, nonattack)
            .ConfigureState(CreatureState.ENFIGHT_WALK, AtdReader, nonattack)
            .ConfigureState(CreatureState.HURT, AtdReader, nonattack)
            .ConfigureState(CreatureState.COOLDOWN, AtdReader, nonattack);
    }
    public static PlayerHandAnimation LoadSword(string name, string attackName)
    {
        SpriteReader nonattack = SpriteReader.LoadOffsetWeaponSprites(name);
        SpriteReader attack = SpriteReader.LoadOffsetWeaponSprites(attackName);
        var playerAnimation = new PlayerHandAnimation();
        return playerAnimation.ConfigureState(CreatureState.IDLE, AtdReader, nonattack)
            .ConfigureNoneAttack(nonattack)
            .ConfigureState(CreatureState.SWORD, AtdReader, attack)
            .ConfigureState(CreatureState.SWORD2H, AtdReader, attack)
            ;
    }

    public static PlayerHandAnimation LoadBow(string name, string attackName)
    {
        SpriteReader nonattack = SpriteReader.LoadOffsetWeaponSprites(name);
        SpriteReader attack = SpriteReader.LoadOffsetWeaponSprites(attackName);
        var playerAnimation = new PlayerHandAnimation();
        return playerAnimation.ConfigureState(CreatureState.IDLE, AtdReader, nonattack)
                .ConfigureNoneAttack(nonattack)
                .ConfigureState(CreatureState.BOW, AtdReader, attack)
            ;
    }
}