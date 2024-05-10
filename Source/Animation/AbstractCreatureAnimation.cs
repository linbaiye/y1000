using System;
using System.Collections.Generic;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Entity.Animation;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public abstract class AbstractCreatureAnimation : ICreatureAnimation
{
    private readonly IDictionary<CreatureState, DirectionIndexedSpriteReader> _stateSpriteReaders;
    private readonly IDictionary<CreatureState, int> _stateMillisPerSprite;
    private readonly IDictionary<CreatureState, int> _stateSpriteNumber;

    protected AbstractCreatureAnimation(IDictionary<CreatureState, DirectionIndexedSpriteReader> stateSpriteReaders,
        IDictionary<CreatureState, int> stateMillisPerSprite, IDictionary<CreatureState, int> stateSpriteNumber)
    {
        _stateSpriteReaders = stateSpriteReaders;
        _stateMillisPerSprite = stateMillisPerSprite;
        _stateSpriteNumber = stateSpriteNumber;
    }

    protected class DirectionIndexedSpriteReader
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

    protected int GetMillisPerSprite(CreatureState state)
    {
        if (_stateMillisPerSprite.TryGetValue(state, out var millis))
        {
            return millis;
        }

        throw new NotImplementedException();
    }

    protected DirectionIndexedSpriteReader GetSpriteReader(CreatureState state)
    {
        if (_stateSpriteReaders.TryGetValue(state, out var reader))
        {
            return reader;
        }
        throw new NotImplementedException();
    }

    protected int GetSpriteNumber(CreatureState state)
    {
        if (_stateSpriteNumber.TryGetValue(state, out var number))
        {
            return number;
        }
        throw new NotImplementedException();
    }

    public int AnimationMillis(CreatureState state)
    {
        var spriteNumber = GetSpriteNumber(state);
        var millisPerSprite = GetMillisPerSprite(state);
        return spriteNumber * millisPerSprite;
    }
    
    protected int MillsToSpriteNumber(int millisPerSprite, int millis, int totalSprite)
    {
        int totalMillis = millisPerSprite * totalSprite;
        millis %= totalMillis;
        return millis / millisPerSprite;
    }

    public abstract OffsetTexture OffsetTexture(CreatureState state, Direction direction, int millis);
}