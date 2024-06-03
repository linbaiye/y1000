using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public abstract class AbstractPlayerBodyAnimation<TA> : AbstractPlayerAnimation<TA> where TA: AbstractPlayerBodyAnimation<TA>
{
    protected static readonly ISpriteRepository SpriteRepository = FilesystemSpriteRepository.Instance;
    
    protected static TA Config(TA animation, AtzSprite normal, AtzSprite fistKick, AtzSprite swordBladeThrow, AtzSprite axeSpear, AtzSprite bow)
    {
        return animation
                .ConfigureState(CreatureState.SWORD, AtdReader, swordBladeThrow)
                .ConfigureState(CreatureState.SWORD2H, AtdReader, swordBladeThrow)
                .ConfigureState(CreatureState.BLADE, AtdReader, swordBladeThrow)
                .ConfigureState(CreatureState.BLADE2H, AtdReader, swordBladeThrow)
                .ConfigureState(CreatureState.IDLE, AtdReader, normal)
                .ConfigureState(CreatureState.AXE, AtdReader, axeSpear)
                .ConfigureState(CreatureState.FLY, AtdReader, normal)
                .ConfigureState(CreatureState.WALK, AtdReader, normal)
                .ConfigureState(CreatureState.RUN, AtdReader, normal)
                .ConfigureState(CreatureState.ENFIGHT_WALK, AtdReader, normal)
                .ConfigureState(CreatureState.HURT, AtdReader, normal)
                .ConfigureState(CreatureState.COOLDOWN, AtdReader, normal)
                .ConfigureState(CreatureState.SIT, AtdReader, normal)
                .ConfigureState(CreatureState.HELLO, AtdReader, normal)
                .ConfigureState(CreatureState.STANDUP, AtdReader, normal)
                .ConfigureState(CreatureState.DIE, AtdReader, normal)
                .ConfigureState(CreatureState.KICK, AtdReader, fistKick)
                .ConfigureState(CreatureState.FIST, AtdReader, fistKick)
                .ConfigureState(CreatureState.BOW, AtdReader, bow)
                .ConfigureState(CreatureState.SPEAR, AtdReader, axeSpear)
                .ConfigureState(CreatureState.THROW, AtdReader, swordBladeThrow)
            ;
    }
}