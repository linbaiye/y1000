using y1000.code.networking.message;

namespace y1000.Source.Networking;

public interface IServerMessageHandler
{
    void Handle(PlayerInterpolation playerInterpolation);

    void Handle(LoginMessage loginMessage);
}