using System;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class CharacterEventArgs : AbstractCharacterEventArgs
{
    public CharacterEventArgs(IClientEvent @event) : base(@event)
    {
    }
    
}