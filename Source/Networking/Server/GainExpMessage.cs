using y1000.Source.Character;

namespace y1000.Source.Networking.Server;

public class GainExpMessage :ICharacterMessage
{
    public GainExpMessage(string name, int level, bool isKungFu)
    {
        Name = name;
        Level = level;
        IsKungFu = isKungFu;
    }
    
    public bool IsKungFu { get; }


    public string Name { get; }
    
    public int Level { get; }
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}