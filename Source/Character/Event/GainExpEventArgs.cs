using System;

namespace y1000.Source.Character.Event;

public class GainExpEventArgs : EventArgs
{
    public GainExpEventArgs(string name, bool isKungFu)
    {
        Name = name;
        IsKungFu = isKungFu;
    }

    public String Name { get; }
    
    public bool IsKungFu { get; }
    
}