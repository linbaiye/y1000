using System;
using Godot;
using Source.Networking.Protobuf;

namespace y1000.Source.Input;

public class KeyboardPredictableInput : AbstractPredictableInput
{
    private readonly Key _key;
    
    public KeyboardPredictableInput(long s, Key key) : base(s)
    {
        _key = key;
    }

    public override InputType Type => InputType.KEY_PRESSED;

}