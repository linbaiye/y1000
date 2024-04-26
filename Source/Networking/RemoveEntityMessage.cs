namespace y1000.Source.Networking;

public class RemoveEntityMessage : IServerMessage
{
    public RemoveEntityMessage(long id)
    {
        Id = id;
    }

    public long Id { get; }
    
    public void Accept(IServerMessageHandler handler)
    {
        handler.Handle(this);
    }
}