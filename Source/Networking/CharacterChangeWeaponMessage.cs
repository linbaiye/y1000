using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.Item;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class CharacterChangeWeaponMessage : IServerMessage, ICharacterMessage
{
    public CharacterChangeWeaponMessage(CharacterWeapon characterWeapon, int affectedSlotId, ICharacterItem? newItem, IAttackKungFu? attackKungFu)
    {
        Weapon = characterWeapon;
        AffectedSlotId = affectedSlotId;
        NewItem = newItem;
        AttackKungFu = attackKungFu;
    }

    public CharacterWeapon Weapon { get; }
    
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