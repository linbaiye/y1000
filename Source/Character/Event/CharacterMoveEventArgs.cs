using System;
using Godot;

namespace y1000.Source.Character.Event;

public class CharacterMoveEventArgs  : EventArgs
{
    public CharacterMoveEventArgs(Vector2I coordinate)
    {
        Coordinate = coordinate;
    }

    public Vector2I Coordinate { get; }
}