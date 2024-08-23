using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class PlayerChatMessage : AbstractEntityMessage
{
    public string Content { get; }
    
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public PlayerChatMessage(long id, string content) : base(id)
    {
        Content = content;
    }
}