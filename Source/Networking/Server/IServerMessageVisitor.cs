
using y1000.Source.Character;
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

    void Visit(CreatureAttackMessage attackMessage)
    {
        Visit((IEntityMessage)attackMessage);
    }

    void Visit(ChangeStateMessage stateMessage)
    {
        Visit((IEntityMessage)stateMessage);
    }

    void Visit(RewindMessage rewindMessage)
    {
        Visit((IEntityMessage)rewindMessage);
    }

    void Visit(ICharacterMessage characterMessage)
    {
    }

    void Visit(PlayerChangeWeaponMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(ShowItemMessage message)
    {
        
    }

    void Visit(TextMessage message)
    {
        
    }

    void Visit(PlayerUnequipMessage unequipMessage)
    {
        Visit((IEntityMessage)unequipMessage);
    }
    
    void Visit(PlayerEquipMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(PlayerToggleKungFuMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(PlayerSitDownMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(PlayerStandUpMessage message)
    {
        Visit((IEntityMessage)message);
    }
}