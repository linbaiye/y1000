using System;
using System.Collections.Generic;
using Godot;
using NLog;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public abstract class AbstractCreatureAnimation<TA> : ICreatureAnimation  where TA : AbstractCreatureAnimation<TA>
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    protected class StateAnimation
    {
        public StateAnimation(int frameNumber, int millisPerSprite, IDictionary<Direction, int> offset, SpriteReader spriteReader)
        {
            FrameNumber = frameNumber;
            MillisPerSprite = millisPerSprite;
            SpriteReader = spriteReader;
            Offset = offset;
        }

        public int FrameNumber { get; }

        private int MillisPerSprite { get; }

        private SpriteReader SpriteReader { get; }
        
        private IDictionary<Direction, int> Offset { get; }
        
        public int TotalMillis => FrameNumber * MillisPerSprite;

        public OffsetTexture Get(Direction direction, int nr)
        {
            if (Offset.TryGetValue(direction, out var off))
            {
                return SpriteReader.Get(off+ nr);
            }
            throw new NotImplementedException();
        }

        public int MillisToFrameNumber(int millis)
        {
            millis %= TotalMillis;
            return millis / MillisPerSprite;
        }

        public OffsetTexture GetFrame(Direction direction, int millis)
        {
            return Get(direction, MillisToFrameNumber(millis));
        }
    }
    
    private readonly IDictionary<CreatureState, StateAnimation> _configs = new Dictionary<CreatureState, StateAnimation>();
    
    protected TA ConfigureState(CreatureState state, int totalNumber, int millisPer, IDictionary<Direction, int> offset, SpriteReader spriteReader)
    {
        _configs.TryAdd(state, new StateAnimation(totalNumber, millisPer, offset, spriteReader));
        return (TA)this;
    }

    protected StateAnimation GetOrThrow(CreatureState state)
    {
        if (_configs.TryGetValue(state, out var config))
        {
            return config;
        }
        throw new NotImplementedException();
    }

    public int AnimationMillis(CreatureState state)
    {
        return GetOrThrow(state).TotalMillis;
    }
    
    public abstract OffsetTexture OffsetTexture(CreatureState state, Direction direction, int millis);
}