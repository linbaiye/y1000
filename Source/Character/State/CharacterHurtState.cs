using y1000.Source.Player;

namespace y1000.Source.Character.State;

public sealed class CharacterHurtState : ICharacterState
{
    private CharacterHurtState(IPlayerState playerHurtState)
    {
        WrappedState = playerHurtState;
    }

    public void OnWrappedPlayerAnimationFinished(Character character)
    {
        if (WrappedState is not PlayerHurtState hurtState)
        {
            return;
        }
        var interruptedState = hurtState.InterruptedState;
        character.ChangeState(CharacterCooldownState.Cooldown());
    }

    public IPlayerState WrappedState { get; }

    public static CharacterHurtState Hurt(ICharacterState state)
    {
        return new CharacterHurtState(IPlayerState.Hurt(state.WrappedState));
    }
}