using System;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class CharacterStateEventArgs : AbstractCharacterEventArgs
{
    public CharacterStateEventArgs(IPrediction prediction,
        IClientEvent @event)
    {
        Prediction = prediction;
        Event = @event;
    }


    public IPrediction Prediction { get; }
    
    public IClientEvent Event { get; }
    
}