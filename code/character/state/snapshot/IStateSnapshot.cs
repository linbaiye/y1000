using y1000.code.networking.message;

namespace y1000.code.character.state.snapshot
{
    public interface IStateSnapshot
    {
        bool Match(IUpdateStateMessage message);

        
    }
}