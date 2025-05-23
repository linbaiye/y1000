﻿using Godot;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class ShowItemMessage : AbstractEntityMessage
{
    public string Name { get; }
    
    public int Number { get; }
    
    public Vector2I Coordinate { get; }
    
    public int Color { get; }
    
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public ShowItemMessage(long id, string name, int number, Vector2I coordinate, int color) : base(id)
    {
        Name = name;
        Number = number;
        Coordinate = coordinate;
        Color = color;
    }
}