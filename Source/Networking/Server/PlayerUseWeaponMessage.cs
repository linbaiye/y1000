namespace y1000.Source.Networking.Server;

public class PlayerUseWeaponMessage : AbstractEntityMessage
{
    public PlayerUseWeaponMessage(long id, string weaponName) : base(id)
    {
        WeaponName = weaponName;
    }
    
    public string WeaponName { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        throw new System.NotImplementedException();
    }
}