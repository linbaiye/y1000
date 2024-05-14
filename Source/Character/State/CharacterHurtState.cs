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
        character.ChangeState(CharacterCooldownState.Cooldown());
    }

    public IPlayerState WrappedState { get; }

    public static CharacterHurtState Hurt()
    {
        return new CharacterHurtState(IPlayerState.Hurt());
    }
}