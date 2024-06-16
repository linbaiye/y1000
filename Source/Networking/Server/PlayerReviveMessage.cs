namespace y1000.Source.Networking.Server;

public class PlayerReviveMessage : AbstractEntityMessage
{
    public PlayerReviveMessage(long id) : base(id)
    {
    }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}