using Godot;
using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class DynamicObjectInterpolation : IServerMessage
{
    private DynamicObjectInterpolation(string name, long id, string shape, Vector2I coordinate, int start, int end, int elapsed)
    {
        Name = name;
        Id = id;
        Shape = shape;
        Coordinate = coordinate;
        FrameStart = start;
        FrameEnd = end;
        Elapsed = elapsed;
    }
    
    public Vector2I Coordinate { get; }
    
    public string Name { get; }
    
    public long Id { get; }

    public string Shape { get; }
    public int FrameStart { get; }
    public int FrameEnd { get; }
    public int Elapsed { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static DynamicObjectInterpolation FromPacket(ShowDynamicObjectPacket packet)
    {
        return new DynamicObjectInterpolation(packet.HasName ? packet.Name : "", packet.Id, packet.Shape, 
            new Vector2I(packet.X, packet.Y), packet.Start, packet.End, packet.Elapsed);
    }
}