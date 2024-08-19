namespace y1000.Source.Networking.Server;

public class PlayerSitDownMessage : AbstractEntityMessage
{
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public PlayerSitDownMessage(long id) : base(id)
    {
    }
}