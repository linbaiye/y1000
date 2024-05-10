using System.Collections.Generic;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class MonsterAnimation : AbstractCreatureAnimation
{

    public static readonly MonsterAnimation Instance = new MonsterAnimation(new Dictionary<CreatureState, DirectionIndexedSpriteReader>(),
        new Dictionary<CreatureState, int>(), new Dictionary<CreatureState, int>());

    private MonsterAnimation(IDictionary<CreatureState, DirectionIndexedSpriteReader>
        stateSpriteReaders, IDictionary<CreatureState, int> stateMillisPerSprite,
        IDictionary<CreatureState, int> stateSpriteNumber) : base(stateSpriteReaders, stateMillisPerSprite, stateSpriteNumber)
    {
    }

    public override OffsetTexture OffsetTexture(CreatureState state, Direction direction, int millis)
    {
        var reader = GetSpriteReader(state);
        var total = GetSpriteNumber(state);
        var unit = GetMillisPerSprite(state);
        var nr = MillsToSpriteNumber(unit, millis, total);
        return reader.Get(direction, nr);
    }
    
    
    private static readonly Dictionary<Direction, int> BUFFALO_IDLE_OFFSET = new()
    {
        { Direction.UP, 18},
        { Direction.UP_RIGHT, 41},
        { Direction.RIGHT, 64},
        { Direction.DOWN_RIGHT, 87},
        { Direction.DOWN, 110},
        { Direction.DOWN_LEFT, 133},
        { Direction.LEFT, 156},
        { Direction.UP_LEFT, 179},
    };
        
    private static readonly Dictionary<Direction, int> BUFFALO_WALK_OFFSET = new()
    {
        { Direction.UP, 0},
        { Direction.UP_RIGHT, 23},
        { Direction.RIGHT, 46},
        { Direction.DOWN_RIGHT, 69},
        { Direction.DOWN, 92},
        { Direction.DOWN_LEFT, 115},
        { Direction.LEFT, 138},
        { Direction.UP_LEFT, 161},
    };
    
    private static readonly Dictionary<CreatureState, int> BUFFALO_SPRITE_MILLIS = new()
    {
        { CreatureState.IDLE, 400 },
        { CreatureState.WALK, 150 },
        { CreatureState.HURT, 100 },
    };

    private static readonly Dictionary<CreatureState, int> BUFFALO_SPRITE_NUMBER = new()
    {
        { CreatureState.IDLE, 5 },
        { CreatureState.WALK, 7 },
        { CreatureState.HURT, 3 },
    };

    private static Dictionary<CreatureState, DirectionIndexedSpriteReader> BuildReaders(SpriteReader reader)
    {
        DirectionIndexedSpriteReader idle = new DirectionIndexedSpriteReader(reader, BUFFALO_IDLE_OFFSET);
        return new Dictionary<CreatureState, DirectionIndexedSpriteReader>()
        {
            { CreatureState.IDLE, idle},
            { CreatureState.WALK, new DirectionIndexedSpriteReader(reader, BUFFALO_WALK_OFFSET)},
        };
    }


    private static MonsterAnimation CreateBuffalo(SpriteReader reader)
    {
        var directionIndexedSpriteReaders = BuildReaders(reader);
        return new MonsterAnimation(directionIndexedSpriteReaders, BUFFALO_SPRITE_MILLIS , BUFFALO_SPRITE_NUMBER);
    }
    

    public static MonsterAnimation LoadFor(string name)
    {
        switch (name)
        {
            case "牛":
                return CreateBuffalo(SpriteReader.LoadSprites("buffalo"));
        }
        return Instance;
    }
}