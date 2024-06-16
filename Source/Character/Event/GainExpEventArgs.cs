using System;

namespace y1000.Source.Character.Event;

public class GainExpEventArgs : EventArgs
{
    public GainExpEventArgs(string name)
    {
        Name = name;
    }

    public String Name { get; }
    
}