using y1000.Source.Input;

namespace y1000.Source.Networking.Server;

public interface IPredictableResponse : IServerMessage
{
    long Sequence { get; }
    
}