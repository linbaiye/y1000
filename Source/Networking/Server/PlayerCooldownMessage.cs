namespace y1000.Source.Networking.Server;

public class PlayerCooldownMessage : AbstractEntityMessage
{
    public PlayerCooldownMessage(long id) : base(id)
    {
    }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}