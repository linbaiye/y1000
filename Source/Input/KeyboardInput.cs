using System;
using Godot;
using Source.Networking.Protobuf;

namespace y1000.Source.Input;

public class KeyboardInput : AbstractInput
{
    private readonly Key _key;
    
    public KeyboardInput(long s, Key key) : base(s)
    {
        _key = key;
    }

    public override InputType Type => InputType.KEY_PRESSED;
    
    public override InputPacket ToPacket()
    {
        throw new NotImplementedException();
    }
}