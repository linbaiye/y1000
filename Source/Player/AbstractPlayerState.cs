using System.Collections.Generic;
using NLog;
using y1000.code;
using y1000.code.player;
using y1000.Source.Character.State;
using y1000.Source.Creature.State;
using y1000.Source.Sprite;

namespace y1000.Source.Player;

public abstract class AbstractPlayerState : AbstractCreatureState<Player>, IPlayerState
{
    protected AbstractPlayerState(SpriteManager spriteManager, long elapsedMillis = 0) : base(spriteManager, elapsedMillis)
    {
    }

    protected void NotifyIfElapsed(Player player, long delta)
    {
        if (ElapsedMillis < SpriteManager.AnimationLength)
        {
            ElapsedMillis += delta;
        }
        if (ElapsedMillis >= SpriteManager.AnimationLength)
        {
            player.NotifyAnimationFinished();
        }
    }

}