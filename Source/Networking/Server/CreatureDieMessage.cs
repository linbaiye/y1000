namespace y1000.Source.Networking.Server;

public class CreatureDieMessage : AbstractEntityMessage
{
    public CreatureDieMessage(long id) : base(id)
    {
    }
    
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}