using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class UpdateDynamicObjectMessage : IEntityMessage
{
    public UpdateDynamicObjectMessage(long id, int frameStart, int frameEnd, bool loop)
    {
        FrameStart = frameStart;
        FrameEnd = frameEnd;
        Loop = loop;
        Id = id;
    }

    public int FrameStart { get; }
    
    public int FrameEnd { get; }
    
    public bool Loop { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public long Id { get; }


    public static UpdateDynamicObjectMessage FromPacket(UpdateDynamicObjectPacket packet)
    {
        return new UpdateDynamicObjectMessage(packet.Id, packet.Start, packet.End, packet.Loop);
    }
        
}