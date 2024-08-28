using y1000.Source.Item;

namespace y1000.Source.Networking.Server;

public class BankOperationMessage : IServerMessage
{
    
    public BankOperationMessage(Type type, int slotId = 0, IItem? item = null)
    {
        SlotId = slotId;
        Item = item;
        Operation = type;
    }
    
    private Type Operation { get; }
    
    public enum Type
    {
        UPDATE = 1,
        
        CLOSE = 2,
    }

    public bool IsUpdate => Operation == Type.UPDATE;
    public bool IsClose => Operation == Type.CLOSE;
    public IItem? Item { get; }

    public int SlotId { get; }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}