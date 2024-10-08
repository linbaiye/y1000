using y1000.Source.Character;

namespace y1000.Source.Networking.Server;

public class PlayerChangeNameColorMessage : AbstractEntityMessage
{
    public PlayerChangeNameColorMessage(long id, int color) : base(id)
    {
        Color = color;
    }

    public int Color { get; }
    
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}