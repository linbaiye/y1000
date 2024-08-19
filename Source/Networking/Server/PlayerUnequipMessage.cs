using y1000.Source.Item;

namespace y1000.Source.Networking.Server;

public class PlayerUnequipMessage : AbstractEntityMessage
{
    public PlayerUnequipMessage(long id, EquipmentType unequipped) : base((id))
    {
        Unequipped = unequipped;
    }

    public EquipmentType Unequipped { get; }
    
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}