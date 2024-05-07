using System;
using System.Collections.Generic;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Entity.Animation;
using y1000.Source.KungFu.Attack;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerAnimation : ICreatureAnimation
{

    public static readonly PlayerAnimation Male = ForMale();
    
    public static readonly PlayerAnimation Female = ForMale();
    private class DirectionIndexedSpriteReader
    {
        public DirectionIndexedSpriteReader(SpriteReader reader, Dictionary<Direction, int> directionIndex)
        {
            Reader = reader;
            DirectionIndex = directionIndex;
        }

        private SpriteReader Reader { get; }

        private Dictionary<Direction, int> DirectionIndex { get; }

        public OffsetTexture Get(Direction direction, int nr)
        {
            if (DirectionIndex.TryGetValue(direction, out var n))
            {
                return Reader.Get(nr + n);
            }

            throw new IndexOutOfRangeException();
        }
    }


    private readonly IDictionary<CreatureState, int> _animationSpriteNumber = new Dictionary<CreatureState, int>()
    {
        { CreatureState.WALK, 6 },
        { CreatureState.RUN, 6 },
        { CreatureState.IDLE, 6 },
        { CreatureState.FLY, 6 },
        { CreatureState.HURT, 4 },
        { CreatureState.COOLDOWN, 3 },
    };

    private readonly IDictionary<AttackKungFuType, int> _above50KungFuSpriteNumber = new Dictionary<AttackKungFuType, int>()
    {
        { AttackKungFuType.QUANFA, 7 },
    };
    
    private readonly IDictionary<AttackKungFuType, int> _below50KungFuSpriteNumber = new Dictionary<AttackKungFuType, int>()
    {
        { AttackKungFuType.QUANFA, 5 },
    };

    private readonly IDictionary<CreatureState, DirectionIndexedSpriteReader> _stateSpriteReaders;

    private readonly IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> _below50AttackSpriteReaders;
    
    private readonly IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> _above50AttackSpriteReaders;


    private const int DefaultMillisPerSprite = 100;
    
    private PlayerAnimation(IDictionary<CreatureState, DirectionIndexedSpriteReader> stateSpriteReaders,
        IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> below50AttackSpriteReaders,
        IDictionary<AttackKungFuType, DirectionIndexedSpriteReader> above50AttackSpriteReaders)
    {
        _stateSpriteReaders = stateSpriteReaders;
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
        if (!_animationSpriteNumber.TryGetValue(state, out var v))
        {
            throw new NotImplementedException();
        }
        var millisPerSprite = DefaultMillisPerSprite;
        if (state == CreatureState.RUN || state == CreatureState.FLY)
        {
            millisPerSprite = 50;
        }
        int spriteNumber = MillsToSpriteNumber(millisPerSprite, millis, v);
        return state is CreatureState.IDLE or CreatureState.FLY or CreatureState.COOLDOWN?
            PingPongSpriteIndex(v, spriteNumber) : spriteNumber;
    }


    private int MillsToSpriteNumber(int millisPerSprite, int millis, int totalSprite)
    {
        int totalMillis = millisPerSprite * totalSprite;
        millis %= totalMillis;
        return millis / millisPerSprite;
    }

    private OffsetTexture ComputeAbove50OffsetTexture(AttackKungFuType type, Direction direction, int millis)
    {
        if (!_above50KungFuSpriteNumber.TryGetValue(type, out var total))
        {
            throw new NotImplementedException();
        }
    }


    public OffsetTexture AttackOffsetTexture(AttackKungFuType type, bool above50, Direction direction, int millis)
    {
        if (above50)
        {
            int spriteNumber = ((millis / DefaultMillisPerSprite + (millis % DefaultMillisPerSprite != 0 ? 1 : 0)) % v) - 1;
        }
    }

    
    public OffsetTexture NoneAttackOffsetTexture(CreatureState state, Direction direction, int millis)
    {
        var nr = ComputerSpriteIndex(state, millis);
        if (_stateSpriteReaders.TryGetValue(state, out var spriteReader))
        {
            return spriteReader.Get(direction, nr);
        }
        throw new NotImplementedException();
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