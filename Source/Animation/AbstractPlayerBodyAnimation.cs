using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public abstract class AbstractPlayerBodyAnimation<TA> : AbstractPlayerAnimation<TA> where TA: AbstractPlayerBodyAnimation<TA>
{
    protected static readonly ISpriteRepository SpriteRepository = FilesystemSpriteRepository.Instance;
    
    protected static TA Config(TA animation, AtzSprite N00, AtzSprite N01, AtzSprite N02, AtzSprite N03, AtzSprite N04)
    {
        return animation.ConfigureState(CreatureState.IDLE, AtdReader, N00)
            .ConfigureState(CreatureState.FLY, AtdReader, N00)
            .ConfigureState(CreatureState.WALK, AtdReader, N00)
            .ConfigureState(CreatureState.RUN, AtdReader, N00)
            .ConfigureState(CreatureState.ENFIGHT_WALK, AtdReader, N00)
            .ConfigureState(CreatureState.HURT, AtdReader, N00)
            .ConfigureState(CreatureState.COOLDOWN, AtdReader, N00)
            .ConfigureState(CreatureState.SIT, AtdReader, N00)
            .ConfigureState(CreatureState.STANDUP, AtdReader, N00)
            .ConfigureState(CreatureState.DIE, AtdReader, N00)
            .ConfigureState(CreatureState.KICK, AtdReader, N01)
            .ConfigureState(CreatureState.FIST, AtdReader, N01)
            .ConfigureState(CreatureState.BOW, AtdReader, N04)
            .ConfigureState(CreatureState.SWORD, AtdReader, N02)
            .ConfigureState(CreatureState.SWORD2H, AtdReader, N02)
            .ConfigureState(CreatureState.BLADE, AtdReader, N02)
            .ConfigureState(CreatureState.BLADE2H, AtdReader, N02)
            .ConfigureState(CreatureState.AXE, AtdReader, N03)
            .ConfigureState(CreatureState.SPEAR, AtdReader, N03)
            .ConfigureState(CreatureState.THROW, AtdReader, N03);
    }
}