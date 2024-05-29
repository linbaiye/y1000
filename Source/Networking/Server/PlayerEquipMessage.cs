
namespace y1000.Source.Networking.Server;

public class PlayerEquipMessage : AbstractEntityMessage
{
    public PlayerEquipMessage(long id, string equipmentName) : base(id)
    {
        EquipmentName = equipmentName;
    }
    
    public string EquipmentName { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}