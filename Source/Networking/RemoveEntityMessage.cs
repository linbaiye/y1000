using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class RemoveEntityMessage : IServerMessage
{
    public RemoveEntityMessage(long id)
    {
        Id = id;
    }

    public long Id { get; }
    
    public void HandleBy(IServerMessageHandler handler)
    {
        handler.Handle(this);
    }
}