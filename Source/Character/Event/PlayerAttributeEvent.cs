using System;

namespace y1000.Source.Character.Event;

public class PlayerAttributeEvent : EventArgs
{
    public static readonly PlayerAttributeEvent Instance = new PlayerAttributeEvent();
}