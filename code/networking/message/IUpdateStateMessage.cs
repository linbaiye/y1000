using y1000.Source.Networking.Server;

namespace y1000.code.networking.message
{
    public interface IUpdateStateMessage : IEntityMessage
    {
       CreatureState ToState { get; }
    }
}