using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public interface IServerMessage
{
    void Accept(IServerMessageVisitor visitor);
}