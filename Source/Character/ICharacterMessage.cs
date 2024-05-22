namespace y1000.Source.Character;

public interface ICharacterMessage
{
    void Accept(ICharacterMessageVisitor visitor);
}