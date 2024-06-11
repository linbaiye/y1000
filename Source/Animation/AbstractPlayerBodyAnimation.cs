using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public abstract class AbstractPlayerBodyAnimation<TA> : AbstractPlayerAnimation<TA> where TA: AbstractPlayerBodyAnimation<TA>
{
    protected static readonly ISpriteRepository SpriteRepository = FilesystemSpriteRepository.Instance;
    
    protected static TA Config(TA animation, AtzSprite normal, AtzSprite fistKick, AtzSprite swordBladeThrow, AtzSprite axeSpear, AtzSprite bow)
    {
        return animation
                .ConfigureState(CreatureState.SWORD, AtdStructure, swordBladeThrow)
                .ConfigureState(CreatureState.SWORD2H, AtdStructure, swordBladeThrow)
                .ConfigureState(CreatureState.BLADE, AtdStructure, swordBladeThrow)
                .ConfigureState(CreatureState.BLADE2H, AtdStructure, swordBladeThrow)
                .ConfigureState(CreatureState.IDLE, AtdStructure, normal)
                .ConfigureState(CreatureState.AXE, AtdStructure, axeSpear)
                .ConfigureState(CreatureState.FLY, AtdStructure, normal)
                .ConfigureState(CreatureState.WALK, AtdStructure, normal)
                .ConfigureState(CreatureState.RUN, AtdStructure, normal)
                .ConfigureState(CreatureState.ENFIGHT_WALK, AtdStructure, normal)
                .ConfigureState(CreatureState.HURT, AtdStructure, normal)
                .ConfigureState(CreatureState.COOLDOWN, AtdStructure, normal)
                .ConfigureState(CreatureState.SIT, AtdStructure, normal)
                .ConfigureState(CreatureState.HELLO, AtdStructure, normal)
                .ConfigureState(CreatureState.STANDUP, AtdStructure, normal)
                .ConfigureState(CreatureState.DIE, AtdStructure, normal)
                .ConfigureState(CreatureState.KICK, AtdStructure, fistKick)
                .ConfigureState(CreatureState.FIST, AtdStructure, fistKick)
                .ConfigureState(CreatureState.BOW, AtdStructure, bow)
                .ConfigureState(CreatureState.SPEAR, AtdStructure, axeSpear)
                .ConfigureState(CreatureState.THROW, AtdStructure, swordBladeThrow)
            ;
    }
}