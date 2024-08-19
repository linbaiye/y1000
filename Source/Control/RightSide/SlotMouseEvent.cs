using System;

namespace y1000.Source.Control.RightSide;

public class SlotMouseEvent : EventArgs
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
    
    public SlotMouseEvent(Type type)
    {
        EventType = type;
    }
    
    public Type EventType { get; }
    
}