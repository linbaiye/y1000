using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class RemoveEntityMessage : IEntityMessage
{
    public RemoveEntityMessage(long id)
    {
        Id = id;
    }

    public long Id { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}