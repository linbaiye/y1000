using y1000.Source.KungFu;

namespace y1000.Source.Networking.Server;

public class PlayerToggleKungFuMessage : AbstractEntityMessage
{
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public PlayerToggleKungFuMessage(long id, string name, int level, KungFuType type) : base(id)
    {
        Name = name;
        Level = level;
        Type = type;
    }
    
    public KungFuType Type { get; }
    public string Name { get; }
    public int Level { get; }
}