using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.Creature;
using y1000.Source.Item;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class CharacterChangeWeaponMessage : IServerMessage, ICharacterMessage
{
    public CharacterChangeWeaponMessage(string name, int affectedSlotId, ICharacterItem? newItem, IAttackKungFu? attackKungFu, CreatureState state)
    {
        AffectedSlotId = affectedSlotId;
        NewItem = newItem;
        AttackKungFu = attackKungFu;
        State = state;
        WeaponName = name;
    }

    public CreatureState State { get; }
    
    public string WeaponName { get; }
    
    public int AffectedSlotId { get; }
    
    public ICharacterItem? NewItem { get; }
    
    public IAttackKungFu? AttackKungFu { get; }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}