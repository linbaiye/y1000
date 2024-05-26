using y1000.Source.Character;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class DropItemMessage : ICharacterMessage
{
    public DropItemMessage(int slot, int numberLeft)
    {
        Slot = slot;
        NumberLeft = numberLeft;
    }

    public int Slot { get; }
    
    public int NumberLeft { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}