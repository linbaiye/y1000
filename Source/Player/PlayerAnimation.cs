using System;
using System.Collections.Generic;
using y1000.code;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.KungFu.Attack;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerAnimation : AbstractCreatureAnimation
{
    public static readonly PlayerAnimation Male = ForMale();
    
    
    public static readonly PlayerAnimation Female = ForMale();
    

    private static readonly IDictionary<CreatureState, int> ANIMATION_SPRITE_NUMBER = new Dictionary<CreatureState, int>()
    {
        { CreatureState.WALK, 6 },
        { CreatureState.RUN, 6 },
        { CreatureState.IDLE, 6 },
        { CreatureState.FLY, 6 },
        { CreatureState.HURT, 4 },
        { CreatureState.COOLDOWN, 3 },
    };
    
    private static readonly IDictionary<CreatureState, int> STATE_MILLIS_PER_SPRITE = new Dictionary<CreatureState, int>()
    {
        { CreatureState.WALK, 100 },
        { CreatureState.RUN, 60 },
        { CreatureState.IDLE, 300 },
        { CreatureState.FLY, 60 },
        { CreatureState.HURT, 100 },
        { CreatureState.ATTACK, 100 },
        { CreatureState.COOLDOWN, 300 },
    };
    

    private readonly IDictionary<AttackKungFuType, int> _above50KungFuSpriteNumber = new Dictionary<AttackKungFuType, int>()
    {
        { AttackKungFuType.QUANFA, 7 },
    };
    
    private readonly IDictionary<AttackKungFuType, int> _below50KungFuSpriteNumber = new Dictionary<AttackKungFuType, int>()
    {
        { AttackKungFuType.QUANFA, 5 },
    };

    private readonly IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> _below50AttackSpriteReaders;
    
    private readonly IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> _above50AttackSpriteReaders;


    private PlayerAnimation(IDictionary<CreatureState, DirectionIndexedSpriteReader> stateSpriteReaders,
        IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> below50AttackSpriteReaders,
        IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> above50AttackSpriteReaders) : base(stateSpriteReaders, STATE_MILLIS_PER_SPRITE, ANIMATION_SPRITE_NUMBER)
    {
        _below50AttackSpriteReaders = below50AttackSpriteReaders;
        _above50AttackSpriteReaders = above50AttackSpriteReaders;
    }

    private int PingPongSpriteIndex(int total, int computed)
    {
        int half = total / 2;
        return computed >= half ? total - computed : computed;
    }

    private int ComputerSpriteIndex(CreatureState state, int millis)
    {
        var millisPerSprite = GetMillisPerSprite(state);
        var total = GetSpriteNumber(state);
        int spriteNumber = MillsToSpriteNumber(millisPerSprite, millis, total);
        return state is CreatureState.IDLE or CreatureState.FLY or CreatureState.COOLDOWN?
            PingPongSpriteIndex(total, spriteNumber) : spriteNumber;
    }

 
    public OffsetTexture AttackOffsetTexture(AttackKungFuType type, bool above50, Direction direction, int millis)
    {
        throw new NotImplementedException();
    }

    public int AttackAnimationMillis(AttackKungFuType type, bool above50)
    {
        throw new NotImplementedException();
    }

    
    public override OffsetTexture OffsetTexture(CreatureState state, Direction direction, int millis)
    {
        var nr = ComputerSpriteIndex(state, millis);
        var spriteReader = GetSpriteReader(state);
        return spriteReader.Get(direction, nr);
    }


    private static readonly Dictionary<Direction, int> MALE_IDLE_DIRECTION_SPRITE_OFFSET = new()
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
        
    private static readonly Dictionary<Direction, int> MALE_WALK_DIRECTION_SPRITE_OFFSET = new()
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
        
        
    private static readonly Dictionary<Direction, int> MALE_COOLDOWN = new()
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
        

    private static IDictionary<CreatureState, DirectionIndexedSpriteReader> BuildNoneAttackSpriteReaders()
    {
        SpriteReader N02 = SpriteReader.LoadOffsetMalePlayerSprites("N02");
        var walk = new DirectionIndexedSpriteReader(N02, MALE_WALK_DIRECTION_SPRITE_OFFSET);
        var run = walk;
        var fly = new DirectionIndexedSpriteReader(N02, MALE_IDLE_DIRECTION_SPRITE_OFFSET);
        var idle = fly;
        return  new Dictionary<CreatureState, DirectionIndexedSpriteReader>()
        {
            { CreatureState.WALK, walk },
            { CreatureState.RUN, walk },
            { CreatureState.IDLE, fly },
            { CreatureState.FLY, fly },
            { CreatureState.COOLDOWN, new DirectionIndexedSpriteReader(N02, MALE_COOLDOWN)},
        };
    }

    private static IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> BuildAbove50AttackSpriteReaders()
    {
        SpriteReader N01 = SpriteReader.LoadOffsetMalePlayerSprites("N01");
        var fist = new DirectionIndexedSpriteReader(N01, MALE_ABOVE50_FIST_SPRITE);
        return new Dictionary<AttackKungFuType, DirectionIndexedSpriteReader>()
        {
            { AttackKungFuType.QUANFA, fist }
        };
    }
    
    
    private static IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> BuildBelow50AttackSpriteReaders()
    {
        SpriteReader N01 = SpriteReader.LoadOffsetMalePlayerSprites("N01");
        var fist = new DirectionIndexedSpriteReader(N01, MALE_BELOW50_FIST_OFFSET);
        return new Dictionary<AttackKungFuType, DirectionIndexedSpriteReader>()
        {
            { AttackKungFuType.QUANFA, fist }
        };
    }

    
    private static PlayerAnimation ForMale()
    {
        var noneAttack = BuildNoneAttackSpriteReaders();
        var below50 = BuildBelow50AttackSpriteReaders();
        var above50 = BuildAbove50AttackSpriteReaders();
        return new PlayerAnimation(noneAttack, below50, above50);
    }
}