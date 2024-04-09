using y1000.code.networking.message;

namespace y1000.Source.Networking;

public interface IServerMessageHandler
{
    void Handle(PlayerInterpolation playerInterpolation);

    void Handle(LoginMessage loginMessage);
    
    void Handle(InputResponseMessage inputResponseMessage);

    void Handle(IEntityMessage message);

    void Handle(MoveMessage moveMessage)
    {
        Handle((IEntityMessage)moveMessage);
    }

    void Handle(SetPositionMessage setPositionMessage)
    {
        Handle((IEntityMessage)setPositionMessage);
    }

    void Handle(TurnMessage turnMessage)
    {
        Handle((IEntityMessage)turnMessage);
    }
}