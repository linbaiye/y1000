using y1000.Source.Player;

namespace y1000.Source.Character.State;

public abstract class AbstractCharacterMoveState: ICharacterState
{
    protected AbstractCharacterMoveState(IPlayerState wrappedState)
    {
        WrappedState = wrappedState;
    }

    public ICharacterState AfterHurt()
    {
        throw new System.NotImplementedException();
    }

    public IPlayerState WrappedState { get; }
}