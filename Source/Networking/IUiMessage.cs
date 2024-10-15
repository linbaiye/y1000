
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public interface IUiMessage : IServerMessage
{
    void Accept(IUiMessageVisitor visitor);
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}