namespace y1000.Source.Networking;

public interface IServerMessage
{
    void Accept(IServerMessageHandler handler);
}