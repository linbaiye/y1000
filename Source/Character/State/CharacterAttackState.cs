using y1000.Source.Creature;
using y1000.Source.Networking.Server;
using y1000.Source.Player;

namespace y1000.Source.Character.State;

public class CharacterAttackState : ICharacterState
{
    private CharacterAttackState(IPlayerState playerAttackState)
    {
        WrappedState = playerAttackState;
    }

    public void OnWrappedPlayerAnimationFinished(CharacterImpl character)
    {
        character.ChangeState(CharacterCooldownState.Cooldown());
    }

    public IPlayerState WrappedState { get; }

    public static CharacterAttackState FromMessage(PlayerAttackMessage message)
    {
        var playerState = IPlayerState.Attack(message);
        return new CharacterAttackState(playerState);
    }

    public bool CanSitDown()
    {
        return true;
    }

    public bool CanAttack()
    {
        return true;
    }

    public static CharacterAttackState Attack(CreatureState state)
    {
        return new CharacterAttackState(IPlayerState.Attack(state));
    }
}