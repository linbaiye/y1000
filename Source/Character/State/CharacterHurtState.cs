using y1000.Source.Creature;
using y1000.Source.Player;

namespace y1000.Source.Character.State;

public sealed class CharacterHurtState : ICharacterState
{
    private CharacterHurtState(PlayerHurtState playerHurtState)
    {
        WrappedState = playerHurtState;
    }

    public void OnWrappedPlayerAnimationFinished(CharacterImpl character)
    {
        var st = ((PlayerHurtState)WrappedState).AfterHurt;
        character.ChangeState(st == CreatureState.COOLDOWN ? CharacterCooldownState.Cooldown() : CharacterIdleState.Idle());
    }

    public IPlayerState WrappedState { get; }

    public static CharacterHurtState Hurt(CreatureState after)
    {
        return new CharacterHurtState(IPlayerState.Hurt(after));
    }
}