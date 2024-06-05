using y1000.Source.KungFu;

namespace y1000.Source.Networking.Server;

public class PlayerToggleKungFuMessage : AbstractEntityMessage
{
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public PlayerToggleKungFuMessage(long id, string name, int level, KungFuType type, bool quietly = false) : base(id)
    {
        Name = name;
        Level = level;
        Type = type;
        Quietly = quietly;
    }
    
    public bool Quietly { get; }
    public KungFuType Type { get; }
    public string Name { get; }
    public int Level { get; }
}