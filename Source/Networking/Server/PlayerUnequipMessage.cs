using y1000.Source.Creature;
using y1000.Source.Item;

namespace y1000.Source.Networking.Server;

public class PlayerUnequipMessage : AbstractEntityMessage
{
    public PlayerUnequipMessage(long id, EquipmentType unequipped, int quanfaLevel, CreatureState? newState = null) : base((id))
    {
        Unequipped = unequipped;
        QuanfaLevel = quanfaLevel;
        NewState = newState;
    }

    public EquipmentType Unequipped { get; }
    public CreatureState? NewState { get; }
    
    public int QuanfaLevel { get; }
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}