
using y1000.Source.Character;
using y1000.Source.Character.Event;

namespace y1000.Source.Networking.Server;

public interface IServerMessageVisitor 
{
    void Visit(IEntityMessage message) {}

    void Visit(PlayerMoveMessage moveMessage)
    {
        Visit((IEntityMessage)moveMessage);
    }
    
    void Visit(DraggedMessage draggedMessage)
    {
        Visit((IEntityMessage)draggedMessage);
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

    void Visit(JoinedRealmMessage message) {}

    void Visit(MoveEventResponse moveEventResponse)
    {
        Visit((IPredictableResponse)moveEventResponse);
    }
    
    void Visit(RemoveEntityMessage removeEntityMessage)
    {
        
    }
    
    void Visit(NpcInterpolation npcInterpolation) {}

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

    void Visit(PlayerCooldownMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(CharacterAttributeMessage message)
    {
        Visit((ICharacterMessage)message);
    }
    
    void Visit(EntitySoundMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(CreatureDieMessage message)
    {
        Visit((IEntityMessage)message);
    }
    
    void Visit(PlayerReviveMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(ProjectileMessage message)
    {
        
    }

    void Visit(MonsterMoveMessage message)
    {
        Visit((IEntityMessage)message);
    }
    
    void Visit(PlayerLearnKungFuMessage message)
    {
        Visit((ICharacterMessage)message);
    }
    
    void Visit(KungFuOrItemAttributeMessage message) {
        Visit((ICharacterMessage)message);
    }
    
    void Visit(PlayerAttributeMessage message) {
        Visit((ICharacterMessage)message);
    }

    void Visit(OpenTradeWindowMessage message)
    {
        
    }

    void Visit(UpdateTradeWindowMessage message)
    {
        
    }

    void Visit(DynamicObjectInterpolation message)
    {
        
    }

    void Visit(UpdateDynamicObjectMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(TeleportMessage teleportMessage)
    {
        Visit((ICharacterMessage)teleportMessage);
    }

    void Visit(LifebarMessage lifebarMessage)
    {
        Visit((IEntityMessage)lifebarMessage);
    }
    
    void Visit(UpdateKungFuSlotMessage message)
    {
        Visit((ICharacterMessage)message);
    }

    void Visit(TeleportInterpolation message)
    {
        
    }

    void Visit(DragEndedMessage message)
    {
        Visit((ICharacterMessage)message);
    }

    void Visit(NpcPositionMessage message)
    {
    }

    void Visit(EntityChatMessage message)
    {
    }

    void Visit(OpenBankMessage message)
    {
        
    }

    void Visit(BankOperationMessage operationMessage)
    {
        
    }

    void Visit(PlayerChangeNameColorMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(PlayerUpdateGuildMessage message)
    {
        Visit((IEntityMessage)message);
    }

    void Visit(UpdateGuildKungFuMessage message)
    {

    }

    void Visit(UpdateQuestWindowMessage message)
    {

    }
}