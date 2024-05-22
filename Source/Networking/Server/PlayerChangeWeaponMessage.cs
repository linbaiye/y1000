using y1000.Source.Item;

namespace y1000.Source.Networking.Server;

public class PlayerChangeWeaponMessage : AbstractEntityMessage
{
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public PlayerChangeWeaponMessage(long id, PlayerWeapon weapon) : base(id)
    {
        Weapon = weapon;
    }
    
    public PlayerWeapon Weapon { get; }
}