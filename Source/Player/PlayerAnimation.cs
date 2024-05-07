using System;
using System.Collections.Generic;
using Godot;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Entity.Animation;
using y1000.Source.KungFu.Attack;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerAnimation : ICreatureAnimation
{

    private readonly IDictionary<CreatureState, SpriteReader> _stateSpriteReaders = new Dictionary<CreatureState, SpriteReader>();


    private readonly IDictionary<CreatureState, int> _animationSpriteNumber = new Dictionary<CreatureState, int>()
    {
        { CreatureState.WALK, 6 },
        { CreatureState.RUN, 6 },
        { CreatureState.IDLE, 3 },
        { CreatureState.FLY, 3 },
    };

    private static readonly IDictionary<Direction, SpriteReader> WALK = new Dictionary<Direction, SpriteReader>();
    private static readonly IDictionary<Direction, SpriteReader> IDLE = new Dictionary<Direction, SpriteReader>();

    private const int DefaultMillisPerSprite = 100;
    
    private readonly IDictionary<AttackKungFuType, SpriteReader> _below50 = new Dictionary<AttackKungFuType, SpriteReader>();
    
    private readonly IDictionary<AttackKungFuType, SpriteReader> _above50 = new Dictionary<AttackKungFuType, SpriteReader>();



    private int ComputeIdleOrRunSpriteIndex(int millis)
    {
        _animationSpriteNumber.TryGetValue(CreatureState.IDLE, out var v);
        return v;
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
        return (millis / millisPerSprite + (millis % millisPerSprite != 0 ? 1 : 0)) % v;
    }
    

    public OffsetTexture AttackOffsetTexture(AttackKungFuType type, Direction direction, bool above50, int millis)
    {
        if (above50)
        {
            
        }
    }


    private OffsetTexture GetOffsetSprite(IDictionary<Direction, SpriteReader> readers, Direction direction, int nr)
    {
        if (readers.TryGetValue(direction, out var reader))
        {
            return reader.Get(nr);
        }
        throw new NotImplementedException();
    }
    
    
    public OffsetTexture NoneOffsetTexture(CreatureState state, Direction direction, int millis)
    {
        int nr = ComputerSpriteIndex(state, millis);
        switch (state)
        {
            case CreatureState.WALK:
            case CreatureState.RUN:
                return GetOffsetSprite(WALK, direction, nr);
            case CreatureState.FLY:
            case CreatureState.IDLE:
                return GetOffsetSprite(IDLE, direction, nr);
        }
    }
}