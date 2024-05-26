using y1000.Source.Character;

namespace y1000.Source.Networking.Server;

public class TextMessage : IServerMessage
{
    public TextMessage(string text)
    {
        Text = text;
    }

    public string Text { get; }
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}