using System;
using System.Collections.Generic;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.KungFu.Attack;

namespace y1000.Source.Animation;

public class PlayerAnimation : AbstractCreatureAnimation<PlayerAnimation>
{
    
    private static readonly IDictionary<Direction, int> IDLE_OFFSET = new Dictionary<Direction, int>()
    {
        { Direction.UP, 48},
        { Direction.UP_RIGHT, 51},
        { Direction.RIGHT, 54},
        { Direction.DOWN_RIGHT, 57},
        { Direction.DOWN, 60},
        { Direction.DOWN_LEFT, 63},
        { Direction.LEFT, 66},
        { Direction.UP_LEFT, 69},
    };
        
    private static readonly Dictionary<Direction, int> WALK_OFFSET = new()
    {
        { Direction.UP, 0},
        { Direction.UP_RIGHT, 6},
        { Direction.RIGHT, 12},
        { Direction.DOWN_RIGHT, 18},
        { Direction.DOWN, 24},
        { Direction.DOWN_LEFT, 30},
        { Direction.LEFT, 36},
        { Direction.UP_LEFT, 42},
    };
        
        
    private static readonly Dictionary<Direction, int> MALE_BELOW50_FIST_OFFSET = new()
    {
        { Direction.UP, 55},
        { Direction.UP_RIGHT, 60},
        { Direction.RIGHT, 65},
        { Direction.DOWN_RIGHT, 70},
        { Direction.DOWN, 75},
        { Direction.DOWN_LEFT, 80},
        { Direction.LEFT, 85},
        { Direction.UP_LEFT, 90},
    };
        
    private static readonly Dictionary<Direction, int> MALE_ABOVE50_FIST_SPRITE = new()
    {
        { Direction.UP, 0},
        { Direction.UP_RIGHT, 6},
        { Direction.RIGHT, 13},
        { Direction.DOWN_RIGHT, 20},
        { Direction.DOWN, 27},
        { Direction.DOWN_LEFT, 34},
        { Direction.LEFT, 41},
        { Direction.UP_LEFT, 48}
    };
        
        
    private static readonly Dictionary<Direction, int> COOLDOWN_OFFSET = new()
    {
        { Direction.UP, 120},
        { Direction.UP_RIGHT, 123},
        { Direction.RIGHT, 126},
        { Direction.DOWN_RIGHT, 129},
        { Direction.DOWN, 132},
        { Direction.DOWN_LEFT, 135},
        { Direction.LEFT, 138},
        { Direction.UP_LEFT, 141},
    };
    
            
    private static readonly Dictionary<Direction, int> HURT_OFFSET = new()
    {
        { Direction.UP, 184 },
        { Direction.UP_RIGHT, 188 },
        { Direction.RIGHT, 192 },
        { Direction.DOWN_RIGHT, 196 },
        { Direction.DOWN, 200 },
        { Direction.DOWN_LEFT, 204 },
        { Direction.LEFT, 208 },
        { Direction.UP_LEFT, 212 },
    };


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



    public static readonly PlayerAnimation Male = ForMale();
    
    public static readonly PlayerAnimation Female = ForMale();
    
    private static PlayerAnimation ForMale()
    {
        SpriteReader N02 = SpriteReader.LoadOffsetMalePlayerSprites("N02");
        SpriteReader N01 = SpriteReader.LoadOffsetMalePlayerSprites("N01");
        AtdReader atdReader = AtdReader.Load("0.atd");
        var playerAnimation = new PlayerAnimation();
        return playerAnimation.ConfigureState(CreatureState.IDLE, atdReader, N02)
                .ConfigureState(CreatureState.FLY, atdReader, N02)
                .ConfigureState(CreatureState.WALK, atdReader, N02)
                .ConfigureState(CreatureState.RUN, atdReader, N02)
                .ConfigureState(CreatureState.HURT, atdReader, N02)
                .ConfigureState(CreatureState.COOLDOWN, atdReader, N02)
                //.ConfigureState(CreatureState.KICK, atdReader, N01)
                //.ConfigureState(CreatureState.FIST, atdReader, N01)
            ;
    }
}