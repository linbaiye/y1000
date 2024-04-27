using y1000.code.networking.message;

namespace y1000.Source.Networking.Server;

public interface IServerMessageHandler
{
    void Handle(PlayerInterpolation playerInterpolation);

    void Handle(IPredictableResponse response);

    void Handle(JoinedRealmMessage joinedRealmMessage);

    void Handle(MoveEventResponse moveEventResponse)
    {
        Handle((IPredictableResponse)moveEventResponse);
    }

    void Handle(IEntityMessage message);

    void Handle(MoveMessage moveMessage)
    {
        Handle((IEntityMessage)moveMessage);
    }
    
    void Handle(RunMessage runMessage)
    {
        Handle((IEntityMessage)runMessage);
    }
    
    void Handle(FlyMessage flyMessage)
    {
        Handle((IEntityMessage)flyMessage);
    }

    void Handle(SetPositionMessage setPositionMessage)
    {
        Handle((IEntityMessage)setPositionMessage);
    }

    void Handle(TurnMessage turnMessage)
    {
        Handle((IEntityMessage)turnMessage);
    }

    void Handle(RemoveEntityMessage removeEntityMessage)
    {
        
    }

    void Handle(CreatureInterpolation creatureInterpolation);
    
}