using Godot;
using y1000.Source.Item;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class ShowItemMessage : AbstractEntityMessage
{
    public string Name { get; }
    
    public ItemType Type { get; }
    
    public int Number { get; }
    
    public Vector2I Coordinate { get; }
    
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public ShowItemMessage(long id, string name, int number, Vector2I coordinate) : base(id)
    {
        Name = name;
        Number = number;
        Coordinate = coordinate;
    }
}