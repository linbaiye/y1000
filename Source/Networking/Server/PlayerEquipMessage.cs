
namespace y1000.Source.Networking.Server;

public class PlayerEquipMessage : AbstractEntityMessage
{
    public PlayerEquipMessage(long id, string equipmentName, int color) : base(id)
    {
        EquipmentName = equipmentName;
        Color = color;
    }
    
    public string EquipmentName { get; }
    
    public int Color { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}