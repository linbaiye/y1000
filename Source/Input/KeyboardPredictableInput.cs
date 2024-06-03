using System;
using Godot;
using Source.Networking.Protobuf;

namespace y1000.Source.Input;

public class KeyboardPredictableInput : AbstractPredictableInput
{
    public KeyboardPredictableInput(long s, Key key) : base(s)
    {
        Key = key;
    }

    public Key Key { get; }

    public override InputType Type => InputType.KEY_PRESSED;

}