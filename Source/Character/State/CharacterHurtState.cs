using y1000.Source.Player;

namespace y1000.Source.Character.State;

public sealed class CharacterHurtState : ICharacterState
{
    private readonly ICharacterState _beforeHurtState;
    private CharacterHurtState(IPlayerState playerHurtState, ICharacterState beforeHurtState)
    {
        WrappedState = playerHurtState;
        _beforeHurtState = beforeHurtState;
    }

    public void OnWrappedPlayerAnimationFinished(Character character)
    {
        character.ChangeState(_beforeHurtState);
    }

    public IPlayerState WrappedState { get; }

    public static CharacterHurtState Hurt(ICharacterState state)
    {
        return new CharacterHurtState(IPlayerState.Hurt(state.WrappedState), state);
    }
}