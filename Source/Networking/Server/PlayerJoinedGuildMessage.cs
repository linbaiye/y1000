namespace y1000.Source.Networking.Server;

public class PlayerJoinedGuildMessage : AbstractEntityMessage
{
    public PlayerJoinedGuildMessage(long id, string name) : base(id)
    {
        Name = name;
    }
    
    public string Name { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}