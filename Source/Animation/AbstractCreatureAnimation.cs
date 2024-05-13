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

    private const int FrameTimeInMillis = 10;

    private static readonly IDictionary<string, Direction> DIRECTION_MAP = new Dictionary<string, Direction>()
    {
        { "DR_0", Direction.UP },
        { "DR_1", Direction.UP_RIGHT },
        { "DR_2", Direction.RIGHT },
        { "DR_3", Direction.DOWN_RIGHT },
        { "DR_4", Direction.DOWN },
        { "DR_5", Direction.DOWN_LEFT },
        { "DR_6", Direction.LEFT },
        { "DR_7", Direction.UP_LEFT },
    };

    private class StateAnimation
    {
        public StateAnimation(int frameNumber, int millisPerFrame)
        {
            FrameNumber = frameNumber;
            MillisPerFrame = millisPerFrame;
            DirectionFrames = new Dictionary<Direction, OffsetTexture[]>();
        }

        private int FrameNumber { get; }

        private int MillisPerFrame { get; }
        
        public int TotalMillis => FrameNumber * MillisPerFrame;
        
        private IDictionary<Direction, OffsetTexture[]> DirectionFrames { get; }

        private OffsetTexture Get(Direction direction, int nr)
        {
            if (DirectionFrames.TryGetValue(direction, out var off))
            {
                return off[nr];
            }
            throw new NotImplementedException("No direction configured: " + direction);
        }

        private int MillisToFrameNumber(int millis)
        {
            millis %= TotalMillis;
            return millis / MillisPerFrame;
        }

        public OffsetTexture GetFrame(Direction direction, int millis)
        {
            return Get(direction, MillisToFrameNumber(millis));
        }
        
        public void Add(AtdStruct atdStruct, SpriteReader reader)
        {
            if (!DIRECTION_MAP.TryGetValue(atdStruct.Direction, out var dir))
            {
                throw new NotImplementedException("No direction mapping.");
            }

            if (DirectionFrames.ContainsKey(dir))
            {
                return;
            }
            OffsetTexture[] textures = new OffsetTexture[atdStruct.Frame];
            int index = 0;
            foreach (var descriptor in atdStruct.FrameDescriptors)
            {
                var offsetTexture = reader.Get(descriptor.Number);
                var texture = offsetTexture.Add(descriptor.XOffset, descriptor.YOffset);
                textures[index++] = texture;
            }
            LOGGER.Debug("Added for direction {0} of state {1}.", dir, atdStruct.Action);
            DirectionFrames.TryAdd(dir, textures);
        }
    }
    
    
    private readonly IDictionary<CreatureState, StateAnimation> _animations = new Dictionary<CreatureState, StateAnimation>();
    
    protected TA ConfigureState(CreatureState state, int totalNumber, int millisPer, IDictionary<Direction, int> offset, SpriteReader spriteReader)
    {
        return (TA)this;
    }
    
    protected TA ConfigureState(CreatureState state, AtdReader atdReader, SpriteReader spriteReader)
    {
        if (_animations.ContainsKey(state))
        {
            return (TA)this;
        }
        var atdStruct = atdReader.FindFirst(state);
        var stateAnimation = new StateAnimation(atdStruct.Frame, atdStruct.FrameTime * FrameTimeInMillis);
        var atdStructs = atdReader.Find(state);
        foreach (var @struct in atdStructs)
        {
            stateAnimation.Add(@struct, spriteReader);
        }
        _animations.TryAdd(state, stateAnimation);
        LOGGER.Debug("Added animation for {0}.", state);
        return (TA)this;
    }

    private StateAnimation GetOrThrow(CreatureState state)
    {
        if (_animations.TryGetValue(state, out var config))
        {
            return config;
        }
        throw new NotImplementedException("No animation for state " + state);
    }

    public int AnimationMillis(CreatureState state)
    {
        return GetOrThrow(state).TotalMillis;
    }

    public OffsetTexture OffsetTexture(CreatureState state, Direction direction, int millis)
    {
        LOGGER.Debug("Getting animation for state {0}.", state);
        return GetOrThrow(state).GetFrame(direction, millis);
    }
}