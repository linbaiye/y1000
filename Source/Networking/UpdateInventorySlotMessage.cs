using y1000.Source.Character;
using y1000.Source.Item;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class UpdateInventorySlotMessage :  ICharacterMessage
{
    public UpdateInventorySlotMessage(int slotId, IItem? item)
    {
        SlotId = slotId;
        Item = item;
    }
    
    public IItem? Item { get; }

    public int SlotId { get; }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}