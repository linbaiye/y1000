using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State;

public sealed class CharacterCooldownState : AbstractCharacterIdleState
{
    private CharacterCooldownState(IPlayerState wrappedState) : base(wrappedState)
    {
    }
    
    public static CharacterCooldownState Cooldown()
    {
        return new CharacterCooldownState(IPlayerState.Cooldown());
    }

    protected override ICharacterState MoveState(Character character, AbstractRightClickInput rightClickInput)
    {
        return CharacterMoveState.EnfightWalk(rightClickInput);
    }
}