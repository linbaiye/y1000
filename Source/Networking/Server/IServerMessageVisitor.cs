
using y1000.Source.Character.Event;

namespace y1000.Source.Networking.Server;

public interface IServerMessageVisitor 
{
    void Visit(IEntityMessage message) {}

    void Visit(MoveMessage moveMessage)
    {
        Visit((IEntityMessage)moveMessage);
    }
    
    void Visit(RunMessage runMessage)
    {
        Visit((IEntityMessage)runMessage);
    }
    
    void Visit(FlyMessage flyMessage)
    {
        Visit((IEntityMessage)flyMessage);
    }

    void Visit(SetPositionMessage setPositionMessage)
    {
        Visit((IEntityMessage)setPositionMessage);
    }

    void Visit(TurnMessage turnMessage)
    {
        Visit((IEntityMessage)turnMessage);
    }

    void Visit(HurtMessage hurtMessage)
    {
        Visit((IEntityMessage)hurtMessage);
    }

    void Visit(PlayerAttackMessage attackMessage)
    {
        Visit((IEntityMessage)attackMessage);
    }
    
    void Visit(PlayerInterpolation playerInterpolation) {}
    

    void Visit(IPredictableResponse response) {}

    void Visit(JoinedRealmMessage joinedRealmMessage) {}

    void Visit(MoveEventResponse moveEventResponse)
    {
        Visit((IPredictableResponse)moveEventResponse);
    }
    
    void Visit(RemoveEntityMessage removeEntityMessage)
    {
        
    }
    
    void Visit(CreatureInterpolation creatureInterpolation) {}

    void Visit(CharacterAttackEventResponse response)
    {
        Visit((IPredictableResponse)response);
    }
}