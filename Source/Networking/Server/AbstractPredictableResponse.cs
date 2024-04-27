using y1000.Source.Character.State.Prediction;

namespace y1000.Source.Networking.Server;

public abstract class AbstractPredictableResponse :  IPredictableResponse
{
    protected AbstractPredictableResponse(long s)
    {
        Sequence = s;
    }

    public long Sequence { get; }
    
    public abstract void HandleBy(IServerMessageHandler handler);
}