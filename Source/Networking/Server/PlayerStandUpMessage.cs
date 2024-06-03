namespace y1000.Source.Networking.Server;

public class PlayerStandUpMessage : AbstractEntityMessage
{
    public PlayerStandUpMessage(long id) : base(id)
    {
    }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}