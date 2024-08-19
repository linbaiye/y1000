using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;

namespace y1000.Source.Networking.Server;

public class MonsterMoveMessage : AbstractEntityMessage
{
    public MonsterMoveMessage(long id, Direction direction, int speed, Vector2I coordinate) : base(id)
    {
        Direction = direction;
        Speed = speed;
        Coordinate = coordinate;
    }
    
    public Direction Direction { get; }
    public int Speed { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public Vector2I Coordinate { get; }

    public override string ToString()
    {
        return $"{nameof(Direction)}: {Direction}, {nameof(Speed)}: {Speed}";
    }

    public static MonsterMoveMessage FromPacket(MonsterMoveEventPacket packet)
    {
        return new MonsterMoveMessage(packet.Id, (Direction)packet.Direction, packet.Speed, new Vector2I(packet.X, packet.Y));
    }
}