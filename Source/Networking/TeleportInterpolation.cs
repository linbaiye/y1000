using Godot;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class TeleportInterpolation : IServerMessage
{
    public TeleportInterpolation(long id, string name, int shape, Vector2I coordinate)
    {
        Id = id;
        Name = name;
        Shape = shape;
        Coordinate = coordinate;
    }

    public long Id { get; }
    
    public string Name { get; }
    public int Shape { get; }
    public Vector2I Coordinate { get; }
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}