using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State;

public class CharacterEnfightState : AbstractCharacterIdleState
{
    public CharacterEnfightState(IPlayerState wrappedState) : base(wrappedState)
    {
    }

    protected override ICharacterState MoveState(AbstractRightClickInput rightClickInput)
    {
        return CharacterMoveState.EnfightWalk(rightClickInput);
    }
}