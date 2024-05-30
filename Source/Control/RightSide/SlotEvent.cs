using System;

namespace y1000.Source.Control.RightSide;

public class SlotEvent : EventArgs
{
    public enum Type
    {
        MOUSE_ENTERED,
        MOUSE_GONE,
        MOUSE_LEFT_CLICK,
        MOUSE_RIGHT_CLICK,
        MOUSE_LEFT_RELEASE,
        MOUSE_LEFT_DOUBLE_CLICK,
    }
    
    public SlotEvent(Type type)
    {
        EventType = type;
    }
    
    public Type EventType { get; }
}