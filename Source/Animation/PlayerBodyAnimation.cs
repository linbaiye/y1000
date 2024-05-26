using System.Collections.Generic;
using Godot;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class PlayerBodyAnimation : AbstractPlayerAnimation<PlayerBodyAnimation>
{

    private static readonly ISpriteRepository REPOSITORY = FilesystemSpriteRepository.Instance;
    private static readonly Vector2 DEFAULT_OFFSET = new(16, -12);

    private static readonly ISet<CreatureState> PLAYER_STATES = new HashSet<CreatureState>()
    {
        CreatureState.IDLE,

        CreatureState.WALK,

        CreatureState.RUN,
        
        CreatureState.STANDUP,

        CreatureState.HURT,

        CreatureState.DIE,

        CreatureState.ENFIGHT_WALK,

        CreatureState.BOW,

        CreatureState.SIT,
        
        CreatureState.FLY,
        
        CreatureState.COOLDOWN,
        
        CreatureState.HELLO,
        
        CreatureState.FIST,
        
        CreatureState.KICK,
        
        CreatureState.SWORD,
        
        CreatureState.SWORD2H,
        
        CreatureState.BLADE,
        
        CreatureState.BLADE2H,
        
        CreatureState.AXE,
        
        CreatureState.SPEAR,
        
        CreatureState.THROW,
    };

    private PlayerBodyAnimation()
    {
    }


    public static readonly PlayerBodyAnimation Male = ForMale();
    
    public static readonly PlayerBodyAnimation Female = ForFemale();

    private static PlayerBodyAnimation Config(string prefix)
    {
        AtzSprite N00 = REPOSITORY.LoadByName(prefix + "00", DEFAULT_OFFSET);
        AtzSprite N01 = REPOSITORY.LoadByName(prefix + "01", DEFAULT_OFFSET);
        AtzSprite N04 = REPOSITORY.LoadByName(prefix + "04", DEFAULT_OFFSET);
        AtzSprite N02 = REPOSITORY.LoadByName(prefix + "02", DEFAULT_OFFSET);
        var playerAnimation = new PlayerBodyAnimation();
        return playerAnimation.ConfigureState(CreatureState.IDLE, AtdReader, N00)
                .ConfigureState(CreatureState.FLY, AtdReader, N00)
                .ConfigureState(CreatureState.WALK, AtdReader, N00)
                .ConfigureState(CreatureState.RUN, AtdReader, N00)
                .ConfigureState(CreatureState.ENFIGHT_WALK, AtdReader, N00)
                .ConfigureState(CreatureState.HURT, AtdReader, N00)
                .ConfigureState(CreatureState.COOLDOWN, AtdReader, N00)
                .ConfigureState(CreatureState.KICK, AtdReader, N01)
                .ConfigureState(CreatureState.FIST, AtdReader, N01)
                .ConfigureState(CreatureState.BOW, AtdReader, N04)
                .ConfigureState(CreatureState.SWORD, AtdReader, N02)
                .ConfigureState(CreatureState.SWORD2H, AtdReader, N02)
                .ConfigureState(CreatureState.BLADE, AtdReader, N02)
                .ConfigureState(CreatureState.BLADE2H, AtdReader, N02)
            ;
    }

    private static PlayerBodyAnimation ForFemale()
    {
        return Config("A");
    }

    private static PlayerBodyAnimation ForMale()
    {
        return Config("N");
    }
}