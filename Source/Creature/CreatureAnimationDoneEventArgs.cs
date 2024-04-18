using System;
using y1000.Source.Player;

namespace y1000.Source.Creature;

public class CreatureAnimationDoneEventArgs : EventArgs
{
    public CreatureAnimationDoneEventArgs(IPlayerState finishedState)
    {
        FinishedState = finishedState;
    }

    public IPlayerState FinishedState { get; }
}