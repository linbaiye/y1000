using y1000.Source.Creature;

namespace y1000.Source.Networking.Server;

public class PlayerChangeWeaponMessage : AbstractEntityMessage
{
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public PlayerChangeWeaponMessage(long id, string weaponName, CreatureState state) : base(id)
    {
        State = state;
        WeaponName = weaponName;
    }
    
    public CreatureState State { get; }
    
    public string WeaponName { get; }


    public override string ToString()
    {
        return $"{nameof(State)}: {State}, {nameof(WeaponName)}: {WeaponName}";
    }
}