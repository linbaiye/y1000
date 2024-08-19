namespace y1000.Source.Networking.Server;

public abstract class AbstractEntityMessage : IEntityMessage
{
    protected AbstractEntityMessage(long id)
    {
        Id = id;
    }
    public long Id { get; }
    
    public abstract void Accept(IServerMessageVisitor visitor);
}