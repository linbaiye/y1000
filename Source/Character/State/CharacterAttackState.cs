using System;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking.Server;
using y1000.Source.Player;

namespace y1000.Source.Character.State;

public class CharacterAttackState : ICharacterState
{
    private readonly PlayerAttackState _playerAttackState;

    public CharacterAttackState(PlayerAttackState playerAttackState)
    {
        _playerAttackState = playerAttackState;
    }

    public void OnWrappedPlayerAnimationFinished(Character character)
    {
        character.ChangeState(CharacterCooldownState.Cooldown(character.IsMale));
    }

    public IPlayerState WrappedState => _playerAttackState;

    public static CharacterAttackState Quanfa(bool male, bool below50, int spriteMillis)
    {
        var playerAttackState = PlayerAttackState.Quanfa(male, below50, spriteMillis);
        return new CharacterAttackState(playerAttackState);
    }

    public static CharacterAttackState FromMessage(Character character, PlayerAttackMessage message)
    {
        if (character.AttackKungFu is QuangFa)
        {
            return Quanfa(character.IsMale, message.Below50, message.MillisPerSprite);
        }
        throw new Exception();
    }
}