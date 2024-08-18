using System;
using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public abstract class AbstractCreatureAnimation<TA> : IAnimation  where TA : AbstractCreatureAnimation<TA>
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

        public int MillisToFrameNumber(int millis)
        {
            if (millis == TotalMillis)
            {
                return (TotalMillis / MillisPerFrame) - 1;
            }
            millis %= TotalMillis;
            return millis / MillisPerFrame;
        }

        public OffsetTexture GetFrame(Direction direction, int millis)
        {
            return Get(direction, MillisToFrameNumber(millis));
        }
        
        public void Add(AtdAction atdAction, AtzSprite reader)
        {
            if (!DIRECTION_MAP.TryGetValue(atdAction.Direction, out var dir))
            {
                throw new NotImplementedException("No direction mapping.");
            }

            if (DirectionFrames.ContainsKey(dir))
            {
                return;
            }
            OffsetTexture[] textures = new OffsetTexture[atdAction.Frame];
            for (int i = 0; i < atdAction.Frame; i++) {
                var descriptor = atdAction.FrameDescriptors[i];
                var descriptorNumber = descriptor.Number;
                var number = descriptorNumber % 500;
                var offsetTexture = reader.Get(number);
                textures[i] = offsetTexture;
            }
            DirectionFrames.TryAdd(dir, textures);
        }
    }
    
    
    private readonly IDictionary<CreatureState, StateAnimation> _animations = new Dictionary<CreatureState, StateAnimation>();
    
    /// <summary>
    /// Use replacingState animation configuration to replace expectedState's animation. Some state has no animations, like HURT,
    /// DIE, so FRONZEN could be used.
    /// </summary>
    /// <param name="expectedState"></param>
    /// <param name="replacingState"></param>
    /// <param name="atdStructure"></param>
    /// <param name="atzSprite"></param>
    /// <returns></returns>
    public TA ConfigureState(CreatureState expectedState, CreatureState replacingState, AtdStructure atdStructure, AtzSprite atzSprite)
    {
        if (_animations.ContainsKey(expectedState))
        {
            return (TA)this;
        }
        if (atdStructure.HasState(expectedState))
        {
            replacingState = expectedState;
        }
        var atdStruct = atdStructure.FindFirst(replacingState);
        var stateAnimation = new StateAnimation(atdStruct.Frame, atdStruct.FrameTime * FrameTimeInMillis);
        var atdStructs = atdStructure.Find(replacingState);
        foreach (var @struct in atdStructs)
        {
            stateAnimation.Add(@struct, atzSprite);
        }
        _animations.TryAdd(expectedState, stateAnimation);
        return (TA)this;
    }
    
    public TA ConfigureState(CreatureState state, AtdStructure atdStructure, AtzSprite atzSprite)
    {
        return ConfigureState(state, state, atdStructure, atzSprite);
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
        return GetOrThrow(state).GetFrame(direction, millis);
    }
}