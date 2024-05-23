using y1000.Source.Creature;
using y1000.Source.Item;

namespace y1000.Source.Networking.Server;

public class PlayerChangeWeaponMessage : AbstractEntityMessage
{
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public PlayerChangeWeaponMessage(long id, PlayerWeapon weapon, CreatureState state) : base(id)
    {
        Weapon = weapon;
        State = state;
    }
    
    public CreatureState State { get; }
    
    public PlayerWeapon Weapon { get; }
}