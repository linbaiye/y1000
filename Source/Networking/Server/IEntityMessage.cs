namespace y1000.Source.Networking.Server
{
    public interface IEntityMessage : IServerMessage
    {
        long Id { get; }
    }
}