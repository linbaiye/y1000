using y1000.Source.Character;

namespace y1000.Source.Networking.Server;

public class DragEndedMessage : ICharacterMessage
{
    public static readonly DragEndedMessage Instance = new DragEndedMessage();
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}