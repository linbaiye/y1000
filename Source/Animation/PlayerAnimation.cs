using System;
using System.Collections.Generic;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.KungFu.Attack;

namespace y1000.Source.Animation;

public class PlayerAnimation : AbstractCreatureAnimation<PlayerAnimation>
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


    private PlayerAnimation()
    {
    }

    private static readonly AtdReader AtdReader = AtdReader.LoadPlayer("0.atd");

    public static readonly PlayerAnimation Male = ForMale();
    
    public static readonly PlayerAnimation Female = ForMale();

    private static PlayerAnimation ForMale()
    {
        SpriteReader N02 = SpriteReader.LoadOffsetMalePlayerSprites("N02");
        SpriteReader N01 = SpriteReader.LoadOffsetMalePlayerSprites("N01");
        var playerAnimation = new PlayerAnimation();
        return playerAnimation.ConfigureState(CreatureState.IDLE, AtdReader, N02)
                .ConfigureState(CreatureState.FLY, AtdReader, N02)
                .ConfigureState(CreatureState.WALK, AtdReader, N02)
                .ConfigureState(CreatureState.RUN, AtdReader, N02)
                .ConfigureState(CreatureState.HURT, AtdReader, N02)
                .ConfigureState(CreatureState.COOLDOWN, AtdReader, N02)
                .ConfigureState(CreatureState.KICK, AtdReader, N01)
                .ConfigureState(CreatureState.FIST, AtdReader, N01)
            ;
    }
}