using System.Collections.Generic;
using Godot;
using NLog;
using Source.Networking.Protobuf;
using y1000.Source.DynamicObject;

namespace y1000.Source.Networking.Server;

public class DynamicObjectInterpolation : IServerMessage
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private DynamicObjectInterpolation(string name, long id, string shape, Vector2I coordinate, int start, int end, int elapsed,
        IEnumerable<Vector2I> coordinates,
        DynamicObjectType type,
        string requiredItem, bool loop)
    {
        Name = name;
        Id = id;
        Shape = shape;
        Coordinate = coordinate;
        FrameStart = start;
        FrameEnd = end;
        Elapsed = elapsed;
        Coordinates = coordinates;
        Type = type;
        RequiredItem = requiredItem;
        Loop = loop;
    }
    
    public Vector2I Coordinate { get; }
    
    public string Name { get; }
    
    public long Id { get; }

    public string Shape { get; }
    public int FrameStart { get; }
    public int FrameEnd { get; }
    public int Elapsed { get; }
    
    public string RequiredItem { get; }
    
    public DynamicObjectType Type { get; }
    
    public bool Loop { get; }
    
    
    public IEnumerable<Vector2I> Coordinates { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static DynamicObjectInterpolation FromPacket(ShowDynamicObjectPacket packet)
    {
        List<Vector2I> coordinates = new List<Vector2I>();
        for (var i = 0; i < packet.GuardX.Count; i++)
        {
            var x = packet.GuardX[i];
            var y = packet.GuardY[i];
            coordinates.Add(new Vector2I(x, y));
        }
        return new DynamicObjectInterpolation(packet.HasName ? packet.Name : "", packet.Id, packet.Shape, 
            new Vector2I(packet.X, packet.Y), packet.Start, packet.End, packet.Elapsed, coordinates,
             (DynamicObjectType)packet.Type,
            packet.HasRequiredItem ? packet.RequiredItem : "", packet.Loop);
    }
}