using y1000.Source.Creature;

namespace y1000.Source.Networking.Server;

public class MonsterMoveMessage : AbstractEntityMessage
{
    public MonsterMoveMessage(long id, Direction direction, int speed) : base(id)
    {
        Direction = direction;
        Speed = speed;
    }
    
    public Direction Direction { get; }
    public int Speed { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{nameof(Direction)}: {Direction}, {nameof(Speed)}: {Speed}";
    }
}