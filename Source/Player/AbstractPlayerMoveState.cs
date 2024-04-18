using System.Collections.Generic;
using Godot;
using NLog;
using y1000.code;
using y1000.Source.Character.State;
using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public abstract class AbstractPlayerMoveState: AbstractCreatureMoveState<Player>, IPlayerState
{
    
    protected AbstractPlayerMoveState(SpriteManager spriteManager, Direction towards, long elapsedMillis = 0) : base(spriteManager, towards, elapsedMillis)
    {
    }

    public override void Update(Player player, long delta)
    {
        if (ElapsedMillis >= SpriteManager.AnimationLength)
        {
            player.NotifyAnimationFinished();
        }
    }
}