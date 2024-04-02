namespace y1000.code.networking.message
{
    public interface IUpdateStateMessage : IEntityMessage
    {
       CreatureState ToState { get; }
    }
}