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

    
    private readonly IDictionary<AttackKungFuType, StateAnimation> _above50AttackAnimations;
    
    private readonly IDictionary<AttackKungFuType, StateAnimation> _below50AttackAnimations;

    private PlayerAnimation()
    {
        _above50AttackAnimations = new Dictionary<AttackKungFuType, StateAnimation>();
        _below50AttackAnimations = new Dictionary<AttackKungFuType, StateAnimation>();
    }

    private int PingPongSpriteIndex(int total, int computed)
    {
        int half = total / 2;
        return computed >= half ? total - 1 - computed : computed;
    }

    private int ComputerSpriteIndex(CreatureState state, int millis)
    {
        var stateAni = GetOrThrow(state);
        int spriteNumber = stateAni.MillisToFrameNumber(millis);
        return state is CreatureState.IDLE or CreatureState.FLY or CreatureState.COOLDOWN?
            PingPongSpriteIndex(stateAni.FrameNumber, spriteNumber) : spriteNumber;
    }

    public OffsetTexture Above50AttackTexture(AttackKungFuType type, Direction direction, int millis)
    {
        return GetOrThrow(type, true).GetFrame(direction, millis);
    }

    private StateAnimation GetOrThrow(AttackKungFuType attackKungFuType, bool above50)
    {
        if (above50)
        {
            if (_above50AttackAnimations.TryGetValue(attackKungFuType, out var animation))
            {
                return animation;
            }
        }
        else
        {
            if (_below50AttackAnimations.TryGetValue(attackKungFuType, out var animation))
            {
                return animation;
            }
        }
        
        throw new NotImplementedException();
    }
    
    public OffsetTexture Below50AttackTexture(AttackKungFuType type, Direction direction, int millis)
    {
        return GetOrThrow(type, false).GetFrame(direction, millis);
    }
    
    public int AttackAnimationMillis(AttackKungFuType type, bool above50)
    {
        return GetOrThrow(type, above50).TotalMillis;
    }

    public override OffsetTexture OffsetTexture(CreatureState state, Direction direction, int millis)
    {
        var nr = ComputerSpriteIndex(state, millis);
        return GetOrThrow(state).Get(direction, nr);
    }

    private PlayerAnimation ConfigureAbove50Attack(AttackKungFuType type, int totalNumber, int millisPerSprite,
        Dictionary<Direction, int> offset, SpriteReader reader)
    {
        _above50AttackAnimations.TryAdd(type, new StateAnimation(totalNumber, millisPerSprite, offset, reader));
        return this;
    }
    
    private PlayerAnimation ConfigureBelow50Attack(AttackKungFuType type, int totalNumber, int millisPerSprite,
        Dictionary<Direction, int> offset, SpriteReader reader)
    {
        _below50AttackAnimations.TryAdd(type, new StateAnimation(totalNumber, millisPerSprite, offset, reader));
        return this;
    }


    public static readonly PlayerAnimation Male = ForMale();
    
    public static readonly PlayerAnimation Female = ForMale();
    
    private static PlayerAnimation ForMale()
    {
        SpriteReader N02 = SpriteReader.LoadOffsetMalePlayerSprites("N02");
        SpriteReader N01 = SpriteReader.LoadOffsetMalePlayerSprites("N01");
        var playerAnimation = new PlayerAnimation();
        return playerAnimation.ConfigureState(CreatureState.IDLE, 6, 300, IDLE_OFFSET, N02)
                .ConfigureState(CreatureState.FLY, 6, 60, IDLE_OFFSET, N02)
                .ConfigureState(CreatureState.WALK, 6, 100, WALK_OFFSET, N02)
                .ConfigureState(CreatureState.RUN, 6, 60, WALK_OFFSET, N02)
                .ConfigureState(CreatureState.HURT, 4, 100, HURT_OFFSET, N02)
                .ConfigureState(CreatureState.COOLDOWN, 6, 300, COOLDOWN_OFFSET, N02)
                .ConfigureAbove50Attack(AttackKungFuType.QUANFA, 7, 100, MALE_ABOVE50_FIST_SPRITE, N01)
                .ConfigureBelow50Attack(AttackKungFuType.QUANFA, 5, 100, MALE_BELOW50_FIST_OFFSET, N01);
    }
}