using Godot;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class PlayerHandAnimation: AbstractPlayerAnimation<PlayerHandAnimation>
{

    private PlayerHandAnimation ConfigureNoneAttack(AtzSprite nonattack)
    {
        return ConfigureState(CreatureState.FLY, AtdReader, nonattack)
            .ConfigureState(CreatureState.WALK, AtdReader, nonattack)
            .ConfigureState(CreatureState.RUN, AtdReader, nonattack)
            .ConfigureState(CreatureState.ENFIGHT_WALK, AtdReader, nonattack)
            .ConfigureState(CreatureState.HURT, AtdReader, nonattack)
            .ConfigureState(CreatureState.COOLDOWN, AtdReader, nonattack)
            .ConfigureState(CreatureState.IDLE, AtdReader, nonattack)
            ;
    }

    private static PlayerHandAnimation Load(string name, string attackName, CreatureState state1, CreatureState? state2 = null)
    {
        AtzSprite nonattack = FilesystemSpriteRepository.Instance.LoadByName(name, new Vector2(16, -12));
        //AtzSprite attack = AtzSprite.LoadOffsetWeaponSprites(attackName);
        AtzSprite attack = FilesystemSpriteRepository.Instance.LoadByName(attackName, new Vector2(16, -12));
        var playerAnimation = new PlayerHandAnimation();
        playerAnimation.ConfigureNoneAttack(nonattack)
                .ConfigureState(state1, AtdReader, attack);
        if (state2 != null)
        {
            playerAnimation.ConfigureState(state2.Value, AtdReader, attack);
        }

        return playerAnimation;
    }
    
    
    public static PlayerHandAnimation LoadSword(string name, string attackName)
    {
        return Load(name, attackName, CreatureState.SWORD, CreatureState.SWORD2H);
    }

    public static PlayerHandAnimation LoadBow(string name, string attackName)
    {
        return Load(name, attackName, CreatureState.BOW);
    }

    public static PlayerHandAnimation LoadBlade(string name, string attackName)
    {
        return Load(name, attackName, CreatureState.BLADE, CreatureState.BLADE2H);
    }
        
}