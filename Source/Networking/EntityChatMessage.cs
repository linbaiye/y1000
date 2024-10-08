using Source.Networking.Protobuf;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class EntityChatMessage : AbstractEntityMessage
{
    public string Content { get; }
    
    public string? Player { get; }
    
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    private EntityChatMessage(long id, string content,
        string? player = null) : base(id)
    {
        Content = content;
        Player = player;
    }

    public static EntityChatMessage Parse(ChatPacket packet)
    {
        return new EntityChatMessage(packet.Id, packet.Content, packet.HasFromPlayer? packet.FromPlayer : null);
    }

    public TextMessage ToTextMessage()
    {
        return new TextMessage(Content, TextMessage.TextLocation.DOWN, Player);
    }
}