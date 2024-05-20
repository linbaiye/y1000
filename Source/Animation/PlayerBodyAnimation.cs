using System.Collections.Generic;
using y1000.Source.Creature;

namespace y1000.Source.Animation;

public class PlayerBodyAnimation : AbstractPlayerAnimation<PlayerBodyAnimation>
{

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
    
    public static readonly PlayerBodyAnimation Female = ForMale();

    private static PlayerBodyAnimation ForMale()
    {
        SpriteReader N02 = SpriteReader.LoadOffsetMalePlayerSprites("N02");
        SpriteReader N01 = SpriteReader.LoadOffsetMalePlayerSprites("N01");
        SpriteReader N04 = SpriteReader.LoadOffsetMalePlayerSprites("N04");
        SpriteReader N00 = SpriteReader.LoadOffsetMalePlayerSprites("N00");
        var playerAnimation = new PlayerBodyAnimation();
        return playerAnimation.ConfigureState(CreatureState.IDLE, AtdReader, N02)
                .ConfigureState(CreatureState.FLY, AtdReader, N02)
                .ConfigureState(CreatureState.WALK, AtdReader, N02)
                .ConfigureState(CreatureState.RUN, AtdReader, N02)
                .ConfigureState(CreatureState.ENFIGHT_WALK, AtdReader, N02)
                .ConfigureState(CreatureState.HURT, AtdReader, N02)
                .ConfigureState(CreatureState.COOLDOWN, AtdReader, N02)
                .ConfigureState(CreatureState.KICK, AtdReader, N01)
                .ConfigureState(CreatureState.FIST, AtdReader, N01)
                .ConfigureState(CreatureState.BOW, AtdReader, N04)
                .ConfigureState(CreatureState.SWORD, AtdReader, N00)
                .ConfigureState(CreatureState.SWORD2H, AtdReader, N00)
            ;
    }
}