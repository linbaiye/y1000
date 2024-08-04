using System;
using Godot;

namespace y1000.Source.Control.RightSide;

public class SlotKeyEvent : EventArgs
{
    public SlotKeyEvent(Key key)
    {
        Key = key;
    }

    public Key Key { get; }
    
}