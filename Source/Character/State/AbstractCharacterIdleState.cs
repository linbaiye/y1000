using y1000.Source.Character.Event;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State;

public abstract class AbstractCharacterIdleState : ICharacterState
{
    protected AbstractCharacterIdleState(IPlayerState wrappedState)
    {
        WrappedState = wrappedState;
    }
    public IPlayerState WrappedState { get; }
    
    public void OnMouseRightClicked(Character character, MouseRightClick rightClick)
    {
        HandleRightClick(character, rightClick);
    }

    
    protected abstract ICharacterState MoveState(Character character, AbstractRightClickInput rightClickInput);

    private void HandleRightClick(Character character, AbstractRightClickInput rightClick)
    {
        if (!character.CanMoveOneUnit(rightClick.Direction))
        {
            if (character.Direction != rightClick.Direction)
            {
                character.EmitEvent(new SetPositionPrediction(rightClick, character.Coordinate, rightClick.Direction), 
                    new MovementEvent(rightClick, character.Coordinate));
                character.Direction = rightClick.Direction;
                WrappedState.Reset();
            }
        }
        else
        {
            character.EmitEvent(new MovePrediction(rightClick, character.Coordinate, rightClick.Direction),
                new MovementEvent(rightClick, character.Coordinate));
            character.ChangeState(MoveState(character, rightClick));
        }
    }

    public bool AcceptInput()
    {
        return true;
    }

    public bool IsValid(IPredictableInput input)
    {
        return true;
    }
        
    public void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion)
    {
        HandleRightClick(character, mousePressedMotion);
    }

    public void Attack(Character character, AttackInput input)
    {
        character.AttackKungFu?.Attack(character, input);
    }
    
    public void OnWrappedPlayerAnimationFinished(Character character)
    {
        WrappedState.Reset();
    }
}