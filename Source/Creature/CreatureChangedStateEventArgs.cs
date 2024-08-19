using System;
using y1000.Source.Player;

namespace y1000.Source.Creature;

public class CreatureChangedStateEventArgs : EventArgs
{
    public CreatureChangedStateEventArgs(IPlayerState newState)
    {
        NewState = newState;
    }

    public IPlayerState NewState { get; }
    
}