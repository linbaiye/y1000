using y1000.Source.Networking;

namespace y1000.Source.Character;

public interface ICharacterMessage : IServerMessage
{
    void Accept(ICharacterMessageVisitor visitor);

}