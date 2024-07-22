namespace y1000.Source.Networking.Server;

public class UpdateDynamicObjectMessage : IEntityMessage
{
    public UpdateDynamicObjectMessage(long id, int frameStart, int frameEnd)
    {
        FrameStart = frameStart;
        FrameEnd = frameEnd;
        Id = id;
    }

    public int FrameStart { get; }
    
    public int FrameEnd { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public long Id { get; }
}