using System;
using System.Collections.Generic;
using Godot;
using NLog;
using y1000.code;
using y1000.Source.Character.State;
using y1000.Source.Creature;
using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public class PlayerMoveState : AbstractCreatureMoveState<Player>, IPlayerState
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private PlayerMoveState(SpriteManager spriteManager, Direction towards, long elapsedMillis = 0) : base(spriteManager, towards, elapsedMillis)
    {
    }
    
    protected override ILogger Logger => LOGGER;

    public static PlayerMoveState WalkTowards(bool male, Direction direction, long elapsed = 0L)
    {
        return new PlayerMoveState(SpriteManager.LoadForPlayer(male, CreatureState.WALK), direction, elapsed); 
    }
    
    public override void Update(Player player, long delta)
    {
        Move(player, delta);
        if (ElapsedMillis >= SpriteManager.AnimationLength)
        {
            player.NotifyAnimationFinished();
        }
    }

    public static PlayerMoveState RunTowards(bool male, Direction direction, long elapsed = 0L)
    {
        return new PlayerMoveState(SpriteManager.LoadForPlayer(male, CreatureState.RUN), direction, elapsed); 
    }
    
    public static PlayerMoveState FlyTowards(bool male, Direction direction, long elapsed = 0L)
    {
        return new PlayerMoveState(SpriteManager.LoadForPlayer(male, CreatureState.FLY), direction, elapsed); 
    }

}