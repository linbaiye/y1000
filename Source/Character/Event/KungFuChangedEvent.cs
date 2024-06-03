using System;

namespace y1000.Source.Character.Event;

public class KungFuChangedEvent : EventArgs
{

    public static readonly KungFuChangedEvent Instance = new KungFuChangedEvent();
    private KungFuChangedEvent() {}

}