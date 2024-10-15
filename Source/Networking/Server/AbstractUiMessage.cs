namespace y1000.Source.Networking.Server;

public abstract class AbstractUiMessage : IUiMessage
{
    public abstract void Accept(IUiMessageVisitor visitor);

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}