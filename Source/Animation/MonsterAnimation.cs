using System.Collections.Generic;
using Godot;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Creature.Monster;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class MonsterAnimation : AbstractCreatureAnimation<MonsterAnimation>
{
    public static readonly MonsterAnimation Instance = new();
    
    private MonsterAnimation()
    {
        
    }

    public override OffsetTexture OffsetTexture(CreatureState state, Direction direction, int millis) => GetOrThrow(state).GetFrame(direction, millis);
    
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
    

    
    private static readonly Dictionary<Direction, int> BUFFALO_HURT_OFFSET = new()
    {
        { Direction.UP, 12},
        { Direction.UP_RIGHT, 35},
        { Direction.RIGHT, 58},
        { Direction.DOWN_RIGHT, 81},
        { Direction.DOWN, 104},
        { Direction.DOWN_LEFT, 127},
        { Direction.LEFT, 150},
        { Direction.UP_LEFT, 173},
    };

    private static MonsterAnimation CreateBuffalo(SpriteReader reader)
    {
        return new MonsterAnimation()
            .ConfigureState(CreatureState.IDLE, 5, 400, BUFFALO_IDLE_OFFSET, reader)
            .ConfigureState(CreatureState.WALK, 7, 150, BUFFALO_WALK_OFFSET, reader)
            .ConfigureState(CreatureState.HURT, 3, 100, BUFFALO_HURT_OFFSET, reader);
    }
    
    public static MonsterAnimation LoadFor(string name)
    {
        switch (name)
        {
            case "牛":
                return CreateBuffalo(SpriteReader.LoadOffsetMonsterSprites("buffalo"));
        }
        return Instance;
    }
}