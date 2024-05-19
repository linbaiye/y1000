using System;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public class PlayerRangedAttackEventArgs : EventArgs
{
    public PlayerRangedAttackEventArgs(long id)
    {
        TargetId = id;
    }

    public long TargetId { get; }
    
}